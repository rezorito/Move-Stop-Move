using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public string currentWeaponId;
    public string currentHairId;
    public string currentPantId;
    public string currentShieldId;
    public string currentSetId;
    public string[] ownedItemIds;
    public int money;
}
