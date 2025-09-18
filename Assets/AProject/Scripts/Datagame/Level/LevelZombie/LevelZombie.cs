using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieSpawnData
{
    public TypeZombie zombieTypeData;
    public int count;
}

[CreateAssetMenu(fileName = "Level Zombie", menuName = "Game/Level Zombie")]
public class LevelZombie : ScriptableObject
{
    public int levelID;
    public string levelName;
    public List<ZombieSpawnData> zombies;  // ch?a t?t c? các lo?i và s? l??ng
}
