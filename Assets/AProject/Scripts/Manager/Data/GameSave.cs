using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave {
    public bool isNew;
    public string namePlayer;
    public int coin;
    //public int heart;
    public List<string> list_WeaponIDOwn, list_HairIDOwn, list_PantIDOwn, list_ShieldIDOwn, list_SetIDOwn;
    public string str_currentWeaponID, str_currentHairID, str_currentPantID, str_currentShieldID, str_currentSetID;
    public List<string> list_SkinWeaponID;
    public int int_skinChooseWeapon;
    public int amountBoosterAddSlotTemp, amountBoosterEmptySlotTemp, amountBoosterHammerBreak, amountBoosterMagnet;
    public bool isDoneTutGameplay;
    public bool isUnlockBoosterAddSlotTemp, isUnlockBoosterEmptySlotTemp, isUnlockBoosterHammerBreak, isUnlockBoosterMagnet;
    public int int_amountShieldZombie, int_amountMaxBullet;
    public float flt_speedBonusZombie, flt_rangeBonusZombie;
    public int int_coinAtriShield, int_coinAtriSpeed, int_coinAtriRange, int_coinAtriMaxBullet;
    public int levelNormal, levelZombie;
    public int highestScoreNormal;
    public int levelStartNormal, levelStartZombie;
    public int levelEndNormal, levelEndZombie;
    [Space]
    //Volumn cho game có thể điều chỉnh chính xác
    //public float soundVolume;
    //public float musicVolume;
    //public float vibrateAmount;
    //Volumn cho game chỉ có thể tắt bật
    public int soundVolume;
    public int musicVolume;
    public int vibrateAmount;
    public int daysPlayed;
    public int sessionStart;
    public bool isNoAds;
    //public FirstTimeAction isFirstReward = new FirstTimeAction();
    //public FirstTimeAction isFirstIAP = new FirstTimeAction();
    //public string heartTime;

    public GameSave() {
        isNew = true;
        namePlayer = "You";
        coin = 0;
        list_WeaponIDOwn = new List<string> {};
        str_currentWeaponID = "";
        list_SkinWeaponID = new List<string> {};
        int_skinChooseWeapon = 1;
        list_HairIDOwn = new List<string> { };
        list_PantIDOwn = new List<string> { };
        list_ShieldIDOwn = new List<string> { };
        list_SetIDOwn = new List<string> { };
        str_currentHairID = "";
        str_currentPantID = "";
        str_currentShieldID = "";
        str_currentSetID = "";
        int_amountShieldZombie = 0;
        int_amountMaxBullet = 2;
        flt_speedBonusZombie = 0;
        flt_rangeBonusZombie = 0;
        int_coinAtriShield = int_coinAtriMaxBullet = 1000;
        int_coinAtriSpeed = int_coinAtriRange = 500;
        levelNormal = levelZombie = 1;
        highestScoreNormal = 0;
        levelStartNormal = levelStartZombie = -1;
        levelEndNormal = levelEndZombie = -1;
        soundVolume = 1;
        musicVolume = 1;
        vibrateAmount = 1;
        isNoAds = false;
        sessionStart = 0;
        daysPlayed = 0;
        //isFirstReward.timeStart = DateTime.Now.ToString();
        //isFirstIAP.timeStart = DateTime.Now.ToString();
        amountBoosterAddSlotTemp = amountBoosterEmptySlotTemp = amountBoosterHammerBreak = amountBoosterMagnet = 1;
    }
}