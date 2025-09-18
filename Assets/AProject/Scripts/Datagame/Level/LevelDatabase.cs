using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Database", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelNormal[] allLevelNormals;
    public LevelZombie[] allLevelZombies;

    public LevelNormal getLevelNormalByID(int id)
    {
        foreach(LevelNormal levelNormal in allLevelNormals)
        {
            if(levelNormal.levelID == id)
            {
                return levelNormal;
            }
        }
        return null;
    }

    public LevelZombie getLevelZombieByID(int id)
    {
        foreach(LevelZombie levelZombie in allLevelZombies)
        {
            if (levelZombie.levelID == id)
            {
                return levelZombie;
            }
        }
        return null;
    }
}
