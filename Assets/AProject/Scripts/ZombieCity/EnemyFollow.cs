using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [Header("------------Ref------------")]
    public NavMeshAgent agent;
    public GameObject bloodEffectPrefab;
    private Transform trans_player;
    private TypeZombie typeZombie;
    public Renderer rend_skin;
    public SoundController soundEnemyController;
    [Header("------------Variable------------")]
    public int int_maxHealth;
    public int int_healthCurrent;
    public float int_minSpeed = 0.9f;
    public float int_maxSpeed = 0.2f;
    public float int_maxDistance = 0.4f;
    public bool isInit = false;

    private void OnEnable() {
        Material tempMat = new Material(Shader.Find("Standard"));
        tempMat.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Material[] mats = rend_skin.materials;
        for (int i = 0; i < mats.Length; i++) {
            mats[i] = tempMat;
        }
        rend_skin.materials = mats;
    }

    public void Init(TypeZombie _typeZombie = null)
    {
        if(!isInit) {
            isInit = true;
            if(_typeZombie == null) {
                Debug.LogError("typeZombie đâu?");
                return;
            }
            typeZombie = _typeZombie;
            trans_player = Player.instance.transform;
        }
        int_maxHealth = typeZombie.health;
        int_healthCurrent = typeZombie.health;
    }

    void Update()
    {
        if (Player.instance.IsPlayerStateRevive() || Player.instance.IsPlayerStateDie()) {
            agent.isStopped = true;
            agent.GetComponent<Animator>().SetBool("IsWin", true);
            return;
        } else {
            agent.isStopped = false;
            agent.GetComponent<Animator>().SetBool("IsWin", false);
        }
        if (!GameManager.instance.IsGameStatePlay()) return;
        if (trans_player != null && agent != null)
        {
            agent.SetDestination(trans_player.position);
            Vector3 dir = agent.velocity;
            dir.y = 0;

            float distance = Vector3.Distance(transform.position, trans_player.position);
            float t = Mathf.Clamp01(distance / int_maxDistance);
            agent.speed = Mathf.Lerp(int_minSpeed, int_maxSpeed, t);

            if (dir.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            soundEnemyController.PlaySound(SoundData.SoundName.WeaponHit);
            Instantiate(bloodEffectPrefab, other.ClosestPoint(transform.position), Quaternion.identity);
            int_healthCurrent--;
            if(int_healthCurrent == 0)
            {
                other.GetComponent<Weapon>().AwardKillZombieToParent(typeZombie);
                this.gameObject.SetActive(false);
                SpawnManagerZBCT.instance.EnemyDied();
            }
        }
    }
}
