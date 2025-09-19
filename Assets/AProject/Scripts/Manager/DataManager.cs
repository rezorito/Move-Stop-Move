using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager Ins;
    public bool isReset = false;
    public bool isLoaded = false;
    public LevelDatabase levelDatabase;
    public AtributePlayerZombie atributePlayerZombie;
    public ItemDatabase itemDatabase;
    public List<ItemBase> list_WeaponData;
    public List<ItemBase> list_HairData;
    public List<ItemBase> list_PantData;
    public List<ItemBase> list_ShieldData;
    public List<ItemBase> list_SetData;
    public GameSave gameSave;
    public GameSave gameSave_BackUp;

    void OnApplicationPause(bool pause) {
        if (pause == true)
            SaveGame();
    }
    void OnApplicationQuit() { SaveGame(); }

    void Reset() {
        gameSave.isNew = true;
    }

    private void Awake() {
        Init();
    }

    private int _lastcoin;
    public void Init() {
        if (Ins == null) {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void LoadData() {
        if(isReset) PlayerPrefs.DeleteKey("GameSave");
        if (isLoaded == false) {
            if (PlayerPrefs.HasKey("GameSave"))
                gameSave = JsonUtility.FromJson<GameSave>(PlayerPrefs.GetString("GameSave"));
            if (gameSave.isNew) {
                InitData();
            }
            isLoaded = true;
        }
        LoadListItem();
        _lastcoin = gameSave.coin;
    }

    public void LoadListItem() {
        list_WeaponData = itemDatabase.allItems.Where(item => item.itemType == ItemType.Weapon).ToList();
        list_PantData = itemDatabase.allItems.Where(item => item.itemType == ItemType.Pant).ToList();
        list_ShieldData = itemDatabase.allItems.Where(item => item.itemType == ItemType.Shield).ToList();
        list_HairData = itemDatabase.allItems.Where(item => item.itemType == ItemType.Hair).ToList();
        list_SetData = itemDatabase.allItems.Where(item => item.itemType == ItemType.Set).ToList();
    }

    public void SaveGame() {
        try {
            if (!isLoaded)
                return;
            if (gameSave == null) {
                if (gameSave_BackUp != null) {
                    gameSave = gameSave_BackUp;
                    Debug.LogError("gameSave bị null , backup thành công ");
                }
                else {
                    InitData();
                    Debug.LogError("gameSave bị null , backup không thành công . Reset data");
                }
            }
            gameSave_BackUp = gameSave;
            PlayerPrefs.SetString("GameSave", JsonUtility.ToJson(gameSave));
            PlayerPrefs.Save();
        }
        catch (Exception ex) {
            Debug.LogError("Lỗi LoadData:" + ex);
        }
    }

    void InitData() {
        gameSave = new GameSave();
        gameSave.isNew = false;
        gameSave.list_WeaponIDOwn.Add("HammerID");
        gameSave.str_currentWeaponID = "HammerID";
        foreach(ItemBase item in itemDatabase.allItems.Where(item => item.itemType == ItemType.Weapon).ToList()) {
            gameSave.list_SkinWeaponID.Add(item.listSkinWeapon[0].skinName);
            gameSave.list_SkinWeaponID.Add(item.listSkinWeapon[1].skinName);
        }
        gameSave.int_skinChooseWeapon = 1;
        gameSave.levelNormal = gameSave.levelZombie = 1;
        gameSave.levelStartNormal = gameSave.levelStartZombie = 1;
        gameSave.levelEndNormal = levelDatabase.allLevelNormals.Length;
        gameSave.levelEndZombie = levelDatabase.allLevelZombies.Length;
        //gameSave.isFirstReward.timeStart = GameManager.Ins.GetCurrentGameTime().ToString();
        //gameSave.isFirstIAP.timeStart = GameManager.Ins.GetCurrentGameTime().ToString();
    }
    //Save NamePlayer
    public void SaveName(string _namePlayer) {
        gameSave.namePlayer = _namePlayer;
        SaveGame();
    }

    //Load/Save Level
    public void SaveLevelNormal() {
        if(gameSave.levelNormal < gameSave.levelEndNormal) gameSave.levelNormal++;
        SaveGame();
    }
    public void SaveLevelZombie() {
        if (gameSave.levelNormal < gameSave.levelEndNormal) gameSave.levelZombie++;
        SaveGame();
    }

    //Load/Save Item
    public void SaveBuyItem(string itemID, int coinItem, ItemType itemType) {
        UpdateCoin(-coinItem);
        if (itemType == ItemType.Weapon) gameSave.list_WeaponIDOwn.Add(itemID);
        if (itemType == ItemType.Hair) gameSave.list_HairIDOwn.Add(itemID);
        if (itemType == ItemType.Pant) gameSave.list_PantIDOwn.Add(itemID);
        if (itemType == ItemType.Shield) gameSave.list_ShieldIDOwn.Add(itemID);
        if (itemType == ItemType.Set) gameSave.list_SetIDOwn.Add(itemID);
        SaveGame();
    }
    public void UpdateEquip(string itemID, ItemType itemType) {
        if (itemType == ItemType.Weapon) {
            gameSave.str_currentWeaponID = itemID;
        }
        if (itemType == ItemType.Hair) {
            if(gameSave.str_currentHairID == itemID) gameSave.str_currentHairID = "";
            else gameSave.str_currentHairID = itemID;
        }
        if (itemType == ItemType.Pant) {
            if (gameSave.str_currentPantID == itemID) gameSave.str_currentPantID = "";
            else gameSave.str_currentPantID = itemID;
        }
        if (itemType == ItemType.Shield) {
            if (gameSave.str_currentShieldID == itemID) gameSave.str_currentShieldID = "";
            else gameSave.str_currentShieldID = itemID;
        }
        if (itemType == ItemType.Set) {
            if (gameSave.str_currentSetID == itemID) gameSave.str_currentSetID = "";
            else {
                gameSave.str_currentHairID = "";
                gameSave.str_currentPantID = "";
                gameSave.str_currentShieldID = "";
                gameSave.str_currentSetID = itemID; 
            }
        } else {
            gameSave.str_currentSetID = "";
        }
        SaveGame();
    }

    public void SaveAddSkinWeapon(string _skinName) {
        gameSave.list_SkinWeaponID.Add(_skinName);
    }

    public void UpdateSkinChooseWeapon(int index_skin) {
        gameSave.int_skinChooseWeapon = index_skin;
    }

    //Load/Save Atribute Player ZombieMode
    public void UpdateShieldBonus() {
        UpdateCoin(-gameSave.int_coinAtriShield);
        gameSave.int_amountShieldZombie += atributePlayerZombie.int_valueShieldBonus;
        gameSave.int_coinAtriShield += gameSave.int_coinAtriShield;
        SaveGame();
    }
    public void UpdateSpeedBonus() {
        UpdateCoin(-gameSave.int_coinAtriSpeed);
        gameSave.flt_speedBonusZombie += atributePlayerZombie.flt_valueSpeedBonus;
        gameSave.int_coinAtriSpeed += gameSave.int_coinAtriSpeed;
        SaveGame();
    }
    public void UpdateRangeBonus() {
        UpdateCoin(-gameSave.int_coinAtriRange);
        gameSave.flt_rangeBonusZombie += atributePlayerZombie.flt_valueRangeBonus;
        gameSave.int_coinAtriRange += gameSave.int_coinAtriRange;
        SaveGame();
    }
    public void UpdateMaxBulletBonus() {
        UpdateCoin(-gameSave.int_coinAtriMaxBullet);
        gameSave.int_amountMaxBullet += atributePlayerZombie.int_valueBulletBonus;
        gameSave.int_coinAtriMaxBullet += gameSave.int_coinAtriMaxBullet;
        SaveGame();
    }

    //Load/SaveVolumn
    public void UpdateSoundVolume(
        //float volume,
        int volume
        ) {
        gameSave.soundVolume = volume;
        SaveGame();
    }
    public void UpdateMusicVolume(
        //float volume,
        int volume
        ) {
        gameSave.musicVolume = volume;
        SaveGame();
    }
    public void UpdateVibrateAmount(
        //float amount,
        int amount
        ) {
        gameSave.vibrateAmount = amount;
        SaveGame();
    }
    public void UpdateCoin(int coin) {
        gameSave.coin += coin;
        SaveGame();
    }

    //KHi thay đổi coin trong inspector thì làm gì đó...
    private void OnValidate() {
        if (gameSave != null) {
            if (_lastcoin != gameSave.coin) {
                _lastcoin = gameSave.coin; // cập nhật giá trị mới
                if(UIControllerNormal.instance != null) {
                    if(UIControllerNormal.instance.uiMenuStart != null) {
                        UIControllerNormal.instance.uiMenuStart.LoadCoinOwn();
                    }
                }
                if (UIControllerZombie.instance != null) {
                    if (UIControllerZombie.instance.uiMenuStartZombie != null) {
                        UIControllerZombie.instance.uiMenuStartZombie.LoadCoinOwn();
                        UIControllerZombie.instance.uiMenuStartZombie.SetupGrowthAtribute();
                    }
                }
            }
        }
    }

    public int GetLevelId() {
        //int levelCur = GetLevel();
        //if (levelCur >= allLevelDataConfig.list_levelDataConfig.Count)
        //    levelCur = 40 + (levelCur - allLevelDataConfig.list_levelDataConfig.Count) % (allLevelDataConfig.list_levelDataConfig.Count - 40);
        //return levelCur;
        return 0;
    }
}