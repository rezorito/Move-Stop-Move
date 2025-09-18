using UnityEngine;

[System.Serializable]
public class DetailLevelNormal
{
    public int LevelID;
    public int expRequire;
}

[CreateAssetMenu(menuName = "SetupLevel/LevelCharNormal", fileName = "LevelCharNormal")]
public class LevelCharNormal : ScriptableObject
{
    public DetailLevelNormal[] allLevels;
}