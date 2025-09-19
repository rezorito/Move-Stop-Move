using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetect : MonoBehaviour
{
    [Header("------------Ref------------")]
    public CameraPos cameraPos;
    public Vector3 scaleDefault = new Vector3(0f, 0f, 0f);
    public Renderer rangeDetectRenderer;
    public LayerMask enemyLayer;
    public GameObject Self;

    [Header("------------Variable------------")]
    public float flt_detectionRadius = 0.15f;   
    private float flt_scaleWithPlayer = 0.2f;
    private Transform trans_previousClosestEnemy = null;
    public bool isPlayer = false;
    private bool isSpecialAttack = false;
    public void setOnSpecialAttack() {
        isSpecialAttack = true;
        transform.localScale += new Vector3(5f, 5f, 5f);
    }
    public void setOffSpecialAttack() {
        isSpecialAttack = false;
        transform.localScale -= new Vector3(5f, 5f, 5f);
    }
    private void Start()
    {
        if(GameManager.instance.currentMode == GameMode.Zombie) {
            scaleDefault = transform.localScale;
            setupRangeDetectZombie();
        } else {
            flt_detectionRadius = Self.transform.localScale.x / flt_scaleWithPlayer;
        }
    }
    public void setupRangeDetectZombie() {
        if (Player.instance.playerController.abilitySelected != null && Player.instance.playerController.abilitySelected.abilityID == "StartBiggerID") {
            flt_detectionRadius = Self.transform.localScale.x / flt_scaleWithPlayer + scaleDefault.x * DataManager.Ins.gameSave.flt_rangeBonusZombie * 0.015f + DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.005f;
        }
        else {
            flt_detectionRadius = Self.transform.localScale.x / flt_scaleWithPlayer + scaleDefault.x * DataManager.Ins.gameSave.flt_rangeBonusZombie * 0.015f;
        }
        transform.localScale = scaleDefault + scaleDefault * DataManager.Ins.gameSave.flt_rangeBonusZombie;
        cameraPos.setupCameraZombie();
    }
    void Update()
    {
        if (!GameManager.instance.IsGameStatePlay()) return;
        if (GameManager.instance.currentMode == GameMode.Normal)
        {
            if (!isSpecialAttack)
            {
                flt_detectionRadius = Self.transform.localScale.x / flt_scaleWithPlayer;
            }
            else
            {
                flt_detectionRadius = Self.transform.localScale.x / flt_scaleWithPlayer + 0.075f;
            }
        }
        if (isPlayer) {
            if (Player.instance.IsPlayerStateRevive() || Player.instance.IsPlayerStateDie()) {
                rangeDetectRenderer.enabled = false;
            } else {
                rangeDetectRenderer.enabled = true;
            }
            if (GameManager.instance.IsGameStateEnd()) {
                if (trans_previousClosestEnemy != null) {
                    var oldTarget = trans_previousClosestEnemy.Find("Target");
                    if (oldTarget) oldTarget.gameObject.SetActive(false);
                }
                return;
            }
            List<Transform> enemies = GetEnemiesInRange();
            if (enemies.Count == 0)
            {
                if (trans_previousClosestEnemy != null)
                {
                    var target = trans_previousClosestEnemy.Find("Target");
                    if (target) target.gameObject.SetActive(false);
                    trans_previousClosestEnemy = null;
                }
                return;
            }

            Transform closestEnemy = GetClosestEnemy(enemies);

            if (closestEnemy != trans_previousClosestEnemy)
            {
                if (trans_previousClosestEnemy != null)
                {
                    var oldTarget = trans_previousClosestEnemy.Find("Target");
                    if (oldTarget) oldTarget.gameObject.SetActive(false);
                }

                if (closestEnemy != null)
                {
                    var newTarget = closestEnemy.Find("Target");
                    if (newTarget) newTarget.gameObject.SetActive(true);
                }

                trans_previousClosestEnemy = closestEnemy;
            }
        }
    }

    public List<Transform> GetEnemiesInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, flt_detectionRadius, enemyLayer);
        List<Transform> result = new List<Transform>();

        foreach (var col in hitColliders)
        {
            if (col.gameObject == Self) continue;
            if (col != null && col.CompareTag("Enemy"))
            {
                result.Add(col.transform);
            }
        }
        return result;
    }

    public Transform ChooseTarget() {
        List<Transform> enemies = GetEnemiesInRange();
        if (enemies.Count == 0) return null;

        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = gameObject.transform.position;

        foreach (Transform enemy in enemies) {
            if (enemy.gameObject == Self) continue;
            float dis = Vector3.Distance(pos, enemy.position);
            if (dis < minDist) {
                minDist = dis;
                closest = enemy;
            }
        }
        return closest;
    }

    public Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (var enemy in enemies)
        {
            if (enemy.gameObject == Self) continue;
            float dist = Vector3.Distance(currentPos, enemy.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flt_detectionRadius);
    }
}
