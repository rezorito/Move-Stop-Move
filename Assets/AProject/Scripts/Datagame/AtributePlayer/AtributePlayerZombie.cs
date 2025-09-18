using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AtributeBonus
{
    public float valueDefault;
    public int coinDefault;
    public float valueBonusLevel;
    public int coinRequireNextLevel;
}

[System.Serializable]
public class AtributeDetail
{
    public int level;
    public float valueBonusTotal;
    public int coinRequireTotal;
    public AtributeBonus atributeBonus;
}
[CreateAssetMenu(fileName = "AtributePlayerZombie", menuName = "SetupLevel/Atribute/AtributePlayerZombie")]
public class AtributePlayerZombie : ScriptableObject
{
    //public AtributeDetail shieldBonus;
    //public AtributeDetail moveSpeedBonus;
    //public AtributeDetail rangeBonus;
    //public AtributeDetail maxbullet;
    public int int_valueShieldBonus;
    public float flt_valueSpeedBonus;
    public float flt_valueRangeBonus;
    public int int_valueBulletBonus;

    public enum TypeAtriButeZombie {
        Shield, 
        Speed, 
        Range,
        MaxBullet
    }
}
