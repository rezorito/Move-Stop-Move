using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Normal", menuName = "Game/Level Normal")]
public class LevelNormal : ScriptableObject
{
    public int levelID;
    public string levelName;
    public Sprite imgLevel;
    public int countEnenmy;
}
