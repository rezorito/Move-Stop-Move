using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager Instance;

    [Header("------------Ref------------")]
    public LevelDatabase allLevels;
    public Transform trans_player;
    public GameObject enemyPrefab;
    public Transform trans_poolEnemy;

    [Header("------------Variable------------")]
    [Header("Spawn Settings")]
    public int int_maxEnemiesAlive = 8;
    public int int_totalEnemiesToSpawn;
    public float flt_minDistanceFromPlayer = 0.3f;
    public int int_enemiesSpawned = 0;
    public int int_enemiesDead = 0;
    public float flt_timeDelaySpawn = 2f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public int getAmountEnemyRemaining() {
        return int_totalEnemiesToSpawn - int_enemiesDead;
    }

    public void InitSpawn() {
        int_totalEnemiesToSpawn = DataManager.Ins.levelDatabase.getLevelNormalByID(DataManager.Ins.gameSave.levelNormal).countEnenmy;
        for (int i = 0; i < int_maxEnemiesAlive && int_enemiesSpawned < int_totalEnemiesToSpawn; i++) {
            GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity, trans_poolEnemy);
            int_enemiesSpawned++;
        }
    }

    public void EnemyDied(EnemyAI enemyAI) {
        int_enemiesDead++;
        if (int_enemiesSpawned < int_totalEnemiesToSpawn) {
            int_enemiesSpawned++;
            StartCoroutine(SetupSpawnAgainEnemyDie(enemyAI));
        }
    }

    public IEnumerator SetupSpawnAgainEnemyDie(EnemyAI enemyAI) {
        enemyAI.gameObject.transform.position = GetRandomSpawnPosition();
        yield return new WaitForSeconds(flt_timeDelaySpawn);
        enemyAI.Init();
    }


    Vector3 GetRandomSpawnPosition() {  //random điểm spawn random trong navmesh ( cách player ) 
        Vector3 randomPos;
        if (DataManager.Ins.gameSave.levelNormal > 1) {
            float x = Random.Range(GameConstants.flt_spawn2MinX, GameConstants.flt_spawn2MaxX);
            float z = Random.Range(GameConstants.flt_spawn2MinZ, GameConstants.flt_spawn2MaxZ);
            randomPos = new Vector3(x, 0.435f, z);
        }
        else {
            float x = Random.Range(GameConstants.flt_spawnMinX, GameConstants.flt_spawnMaxX);
            float z = Random.Range(GameConstants.flt_spawnMinZ, GameConstants.flt_spawnMaxZ);
            randomPos = new Vector3(x, 0.435f, z);
        }

        // tìm vị trí hợp lệ trên NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 1.0f, NavMesh.AllAreas)) {
            if (Vector3.Distance(hit.position, trans_player.position) > flt_minDistanceFromPlayer) {
                return hit.position;
            }
        }
        Debug.LogWarning("Không tìm thấy vị trí hợp lệ trên NavMesh, thử lại...");
        return GetRandomSpawnPosition();
    }
}
