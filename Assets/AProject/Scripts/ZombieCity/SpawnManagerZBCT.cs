using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManagerZBCT : MonoBehaviour {
    public static SpawnManagerZBCT instance;

    [Header("------------Ref------------")]
    public GameObject[] arrObj_enemyPrefab;
    private TypeZombie typeZombieNormal;
    private TypeZombie typeZombieArmor;
    private TypeZombie typeZombieBoss;
    public Transform trans_player;
    public Transform[] listTrans_pointSpawns;
    public Transform trans_poolEnemy;
    public List<(GameObject obj, EnemyFollow enemyFollow)> list_zombieNormal = new List<(GameObject, EnemyFollow)>();
    public List<(GameObject obj, EnemyFollow enemyFollow)> list_zombieArmor = new List<(GameObject, EnemyFollow)>();
    public List<(GameObject obj, EnemyFollow enemyFollow)> list_zombieBoss = new List<(GameObject, EnemyFollow)>();

    [Header("------------Variable------------")]
    public int int_maxEnemiesAlive = 20;
    public int int_maxEnemiesNormalInstantiate = 20;
    public int int_maxEnemiesArmorInstantiate = 5;
    public int int_maxEnemiesBossInstantiate = 1;
    private int int_enemyNormal = 0;
    private int int_enemyArmor = 0;
    private int int_enemyBoss = 0;
    public int int_totalEnemiesToSpawn = 50;
    public int int_enemiesSpawned = 0;
    public int int_enemiesDead = 0;
    public int getAmountEnemyRemaining() {
        return int_totalEnemiesToSpawn - int_enemiesDead;
    }

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void InitSpawn() {
        List<ZombieSpawnData> zombieDatas = DataManager.Ins.levelDatabase.getLevelZombieByID(DataManager.Ins.gameSave.levelZombie).zombies;
        foreach (ZombieSpawnData zombieData in zombieDatas) {
            if (zombieData.zombieTypeData.zombieType == ZombieType.Normal) {
                int_enemyNormal = zombieData.count;
                typeZombieNormal = zombieData.zombieTypeData;
            }
            else if (zombieData.zombieTypeData.zombieType == ZombieType.Armor) {
                int_enemyArmor = zombieData.count;
                typeZombieArmor = zombieData.zombieTypeData;
            }
            else if (zombieData.zombieTypeData.zombieType == ZombieType.Boss) {
                int_enemyBoss = zombieData.count;
                typeZombieBoss = zombieData.zombieTypeData;
            }
        }
        int_totalEnemiesToSpawn = int_enemyNormal + int_enemyArmor + int_enemyBoss;
        //for (int i = 0; i < maxEnemiesAlive && enemiesSpawned < totalEnemiesToSpawn; i++)
        //{
        //    SpawnEnemy(prefabZombieNormal);
        //    enemiesSpawned++;
        //}
        SetupInstancePoolEnemy();
        SetupEnemyShow();
    }

    public void SetupInstancePoolEnemy() {
        list_zombieNormal.Clear();
        list_zombieArmor.Clear();
        list_zombieBoss.Clear();
        while(list_zombieNormal.Count < int_maxEnemiesNormalInstantiate && list_zombieNormal.Count < int_enemyNormal) {
            list_zombieNormal.Add(SpawnEnemy(typeZombieNormal));
        } 
        while (list_zombieArmor.Count < int_maxEnemiesArmorInstantiate && list_zombieArmor.Count < int_enemyArmor) {
            list_zombieArmor.Add(SpawnEnemy(typeZombieArmor));
        }
        while (list_zombieBoss.Count < int_maxEnemiesBossInstantiate && list_zombieBoss.Count < int_enemyBoss) {
            list_zombieBoss.Add(SpawnEnemy(typeZombieBoss));
        }
    }

    public (GameObject, EnemyFollow) SpawnEnemy(TypeZombie typeZombie) {
        GameObject newEnemy = Instantiate(typeZombie.prefab, GetRandomSpawnPosition(), Quaternion.identity, trans_poolEnemy);
        newEnemy.SetActive(false);
        EnemyFollow scriptEnemy = newEnemy.GetComponent<EnemyFollow>();
        scriptEnemy.Init(typeZombie);
        return (newEnemy, scriptEnemy);
    }

    public void SetupEnemyShow() {
        while(int_enemiesSpawned < int_totalEnemiesToSpawn && int_enemiesSpawned < int_maxEnemiesAlive) {
            int_enemiesSpawned++;
            GetEnemy(GetTypeZombieToSpawn()).Item1.SetActive(true);
        }
    }

    public (GameObject, EnemyFollow) GetEnemy(TypeZombie typeZombie) {
        //Normal
        if(typeZombie.zombieType == ZombieType.Normal) {
            foreach (var enemy in list_zombieNormal) {
                if (!enemy.obj.activeInHierarchy) {
                    return enemy;
                }
            }
        }
        //Armor
        if(typeZombie.zombieType == ZombieType.Armor) {
            foreach (var enemy in list_zombieArmor) {
                if (!enemy.obj.activeInHierarchy) {
                    return enemy;
                }
            }
        }
        //Boss
        if (typeZombie.zombieType == ZombieType.Boss) {
            foreach (var enemy in list_zombieBoss) {
                if (!enemy.obj.activeInHierarchy) {
                    return enemy;
                }
            }
        }
        return (null, null);
    }

    public TypeZombie GetTypeZombieToSpawn() {
        if (int_enemiesSpawned >= int_totalEnemiesToSpawn * 2 / 3 && int_enemyBoss != 0) {
            Debug.Log("Spawn Boss!");
            int_enemyBoss--;
            return typeZombieBoss;
        }
        else if (int_enemiesSpawned >= int_totalEnemiesToSpawn / (int_enemyArmor + 1) && int_enemyArmor != 0) {
            Debug.Log("Spawn Armor!");
            int_enemyArmor--;
            return typeZombieArmor;
        }
        Debug.Log("Spawn Normal!");
        return typeZombieNormal;
    }

    public void EnemyDied() {
        int_enemiesDead++;
        SetupRespawnZombieDie();
    }

    public void SetupRespawnZombieDie() {
        if(int_enemiesSpawned < int_totalEnemiesToSpawn) {
            int_enemiesSpawned++;
            var enemy = GetEnemy(GetTypeZombieToSpawn());
            enemy.Item1.transform.position = GetRandomSpawnPosition();
            enemy.Item1.SetActive(true);
            enemy.Item2.Init();
        }
    }

    Vector3 GetRandomSpawnPosition() {
        Transform randomPointSpawn = listTrans_pointSpawns[Random.Range(0, listTrans_pointSpawns.Length)];
        return randomPointSpawn.position;
    }
}
