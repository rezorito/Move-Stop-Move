using UnityEngine;

[System.Serializable]
public class DetailLevelZombie
{
    public int LevelID;
    public int expRequire;
}

[CreateAssetMenu(menuName = "SetupLevel/LevelCharZombie", fileName = "LevelCharZombie")]
public class LevelCharZombie : ScriptableObject
{
    public DetailLevelZombie[] allLevels;
}