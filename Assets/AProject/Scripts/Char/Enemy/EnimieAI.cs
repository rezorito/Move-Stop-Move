
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour {
    [Header("------------Ref------------")]
    public NavMeshAgent agent;
    public RangeDetect rangeDetect;
    public SkinEnemy skinEnemy;
    public Billboard billBoard;
    public SystemGameplay systemGameplaySelf;
    public LevelCharNormal levelCharNormal;
    public Animator anim_Enemy;
    public Collider col_Enemy;
    private SystemGameplay systemGameplayPlayer;
    public SoundController soundEnemyAIController;
    public GameObject bloodEffectPrefab;
    public Transform trans_pointWeaponAttack;

    public GameObject obj_weaponShow;
    public GameObject obj_weaponAttack;
    public Weapon weaponScript;
    [Header("------------Variable------------")]
    public bool isInit = false;
    public bool isAttack = false;
    public float flt_range;
    public float flt_attackDelay = 2f;
    public bool isDead = false;
    private bool isMoving = false;
    private bool isSpecialAttack = false;
    public string str_nameSelf = "";

    string[] foreignNames = {
        "Arlo", "Zane", "Finnley", "Theo", "Alfie", "Otis", "Silas", "Elias", "Beckham", "Corbin",
        "Luca", "Oscar", "Teddy", "Freddie", "Leon", "Saint", "Ronnie", "Maximus", "Reuben", "Cassian",
        "Jasper", "Orion", "Dashiell", "Kai", "Soren", "Axel", "Milo", "Ezra", "Phoenix", "Caspian",
        "Atticus", "Ronan", "Xander", "Lucian", "Evander", "Quentin", "Thorne", "Blaise", "Cyrus", "Zephyr",
        "Magnus", "Alaric", "Lysander", "Jett", "Kairo", "Onyx", "Sterling", "Apollo", "Enzo", "Nico"
    };

    private void Awake() {
        systemGameplayPlayer = Player.instance.playerController.systemGameplayPlayer;
    }

    private void Start() {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        Init(); //Khi mới sinh ra thì khởi tạo
    }

    public void Init() {
        skinEnemy.Init();
        obj_weaponShow = skinEnemy.obj_weaponShow;
        obj_weaponAttack = skinEnemy.obj_weaponAttack;
        weaponScript = skinEnemy.weaponAttackScript;
        ResetSelf();
        str_nameSelf = foreignNames[Random.Range(0, foreignNames.Length)];
        billBoard.InitName(str_nameSelf);
        gameObject.SetActive(true);
        setupLevelSelf();
        StartCoroutine(SetupAIEnemy());
    }

    public void ResetSelf() {
        col_Enemy.enabled = true;
        isDead = false;
        isMoving = false;
        isSpecialAttack = false;
    }

    public void setupLevelSelf() {
        int levelPlayer = systemGameplayPlayer.int_levelSelf;
        int levelSelf = Random.Range(0, levelPlayer + 1);
        int expCurrent = 0;
        if (levelSelf == 0) {
            expCurrent = Random.Range(0, levelCharNormal.allLevels[levelSelf].expRequire);
        }
        else {
            expCurrent = Random.Range(levelCharNormal.allLevels[levelSelf - 1].expRequire, levelCharNormal.allLevels[levelSelf].expRequire);
        }
        systemGameplaySelf.int_levelSelf = levelSelf;
        systemGameplaySelf.setScoreSelf(expCurrent);
        if (levelSelf > 0) {
            for (int i = 1; i <= levelSelf; i++) {
                systemGameplaySelf.UpdateUILevel();
            }
        }
    }

    IEnumerator SetupAIEnemy() {
        while (true) {
            if (Time.timeScale == 1) agent.isStopped = false;
            if (GameManager.instance.IsGameStateHome() || isDead) {
                if (agent != null && agent.isOnNavMesh)
                    agent.isStopped = true;

                yield return null;
                continue;
            }
            else {
                if (agent != null && agent.isOnNavMesh && !isAttack)
                    agent.isStopped = false;
            }
            if (agent != null && agent.isOnNavMesh) {
                if (!isMoving && agent.remainingDistance <= agent.stoppingDistance) {
                    Vector3 point;
                    if (RandomPoint(this.transform.position, flt_range, out point)) {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                        agent.SetDestination(point);
                        isAttack = true;
                        Vector3 dir = (agent.destination - transform.position).normalized;
                        dir.y = 0;
                        if (dir != Vector3.zero) {
                            transform.rotation = Quaternion.LookRotation(dir);
                        }
                        anim_Enemy.SetBool("IsIdle", false);
                        isMoving = true;
                    }
                }
            }
            if (isMoving && HasReachedDestination()) {
                isMoving = false;
                Debug.Log("Đã đến điểm đến!");
                anim_Enemy.SetBool("IsIdle", true);
                float time = 0f;
                float timeStart = 0f;
                float timeEnd = 1.5f;
                while (timeStart <= timeEnd) {
                    if (time > flt_attackDelay || isAttack) {
                        if (rangeDetect.GetEnemiesInRange().Count > 0) {
                            LookAtEnemy();
                            time = 0f;
                        }
                    }
                    time += Time.deltaTime;
                    timeStart += Time.deltaTime;
                    yield return null;
                }
            }
            yield return null;
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    bool HasReachedDestination() {
        if (agent != null && agent.isOnNavMesh) {
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void LookAtEnemy() {
        Transform trans_target = rangeDetect.ChooseTarget();
        if (trans_target == null) return;
        Vector3 dir = (trans_target.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(dir);
        }
        agent.isStopped = true;
        anim_Enemy.SetBool("IsAttack", true);
    }

    public void ActiveAttack() {
        Transform trans_target = rangeDetect.ChooseTarget();
        if (trans_target == null) {
            anim_Enemy.SetBool("IsAttack", false);
            anim_Enemy.SetBool("IsIdle", true);
            return;
        }
        if (!weaponScript.isInit) weaponScript.Init(this.gameObject, rangeDetect, systemGameplaySelf);
        weaponScript.FlyTo(trans_pointWeaponAttack, trans_target, 0f, isSpecialAttack);
        if (isSpecialAttack) {
            isSpecialAttack = false;
            rangeDetect.setOffSpecialAttack();
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Weapon")) {
            if (other.gameObject == skinEnemy.obj_weaponAttack) return;
            soundEnemyAIController.PlaySound(SoundData.SoundName.WeaponHit);
            SpawnManager.Instance.EnemyDied(this);
            anim_Enemy.SetBool("IsAttack", false);
            Instantiate(bloodEffectPrefab, other.ClosestPoint(transform.position), Quaternion.identity);
            isDead = true;
            anim_Enemy.SetBool("IsDead", true);
            col_Enemy.enabled = false;
            StartCoroutine(SetupDeadEnemy());
        }
        if (other.CompareTag("Gift")) {
            if(!isSpecialAttack) rangeDetect.setOnSpecialAttack();
            isSpecialAttack = true;
        }
    }

    IEnumerator SetupDeadEnemy() {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void ResetAttack() {
        obj_weaponShow.gameObject.SetActive(true);
        anim_Enemy.SetBool("IsAttack", false);
        isAttack = false;
    }
}
