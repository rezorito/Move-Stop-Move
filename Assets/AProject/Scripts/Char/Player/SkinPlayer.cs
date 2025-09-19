using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPlayer : MonoBehaviour
{
    public Billboard billboard;
    public Material mat_skinStart;

    public Transform trans_parentWeapon;
    public Transform trans_poolWeapon;
    public GameObject obj_skinPlayer;
    public GameObject obj_pantPlayer;
    public Transform trans_parentHairPlayer1;
    public Transform trans_parentHairPlayer2;
    public Transform trans_parentShieldPlayer;
    public Transform trans_parentWingPlayer;
    public Transform trans_parentTailPlayer;

    public GameObject obj_weaponShow;
    public GameObject obj_weaponAttack;
    public Weapon weaponAttackScript;
    private ItemBase weaponItemOwn;
    private AbilityBase abilityApplyWeapon = null;

    public List<(GameObject obj_weaponAttack, Weapon weaponScript)> list_weaponSpawnZombie = new List<(GameObject, Weapon)>();

    public void Init() {
        LoadOutfitPlayer();
        LoadWeaponPlayer();
        billboard.InitName(DataManager.Ins.gameSave.namePlayer);
    }

    public void LoadWeaponPlayer() {
        if (obj_weaponShow != null) obj_weaponShow.SetActive(false);
        if (!string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentWeaponID)) {
            ItemBase weaponItem = weaponItemOwn = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentWeaponID);
            obj_weaponShow = SetupWeapon(weaponItem, false);
            if (GameManager.instance.currentMode == GameMode.Normal) {
                if (obj_weaponAttack != null) obj_weaponAttack.SetActive(false);
                obj_weaponAttack = SetupWeapon(weaponItem, true);
                weaponAttackScript = SetupScriptWeaponAttack(obj_weaponAttack, weaponItem);
            }
            else if (GameManager.instance.currentMode == GameMode.Zombie) {
                SetupPoolWeaponZombie();
            }
        }
    }

    public GameObject SetupWeapon(ItemBase weaponItem, bool isWeaponAttack) {
        GameObject obj_weapon;
        if (!isWeaponAttack) {
            obj_weapon = Instantiate(weaponItem.modelPrefab, trans_parentWeapon);
        }
        else {
            obj_weapon = Instantiate(weaponItem.modelPrefab, trans_poolWeapon, false);
            obj_weapon.SetActive(false);
        }
        bool isCustom = weaponItem.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].isCustom;
        Material[] materials = obj_weapon.GetComponent<Renderer>().materials;
        if (isCustom) {
            Material[] customMats = weaponItem.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].customMaterial;

            for (int i = 0; i < materials.Length; i++) {
                if (i < customMats.Length) {
                    materials[i] = customMats[i];
                }
                else {
                    materials[i] = customMats[customMats.Length - 1];
                }
            }
        }
        else {
            Material defaultMat = weaponItem.listSkinWeapon[DataManager.Ins.gameSave.int_skinChooseWeapon].skinMaterial;

            for (int i = 0; i < materials.Length; i++) {
                materials[i] = defaultMat;
            }
        }
        obj_weapon.GetComponent<Renderer>().materials = materials;
        return obj_weapon;
    }

    public Weapon SetupScriptWeaponAttack(GameObject obj_weapon, ItemBase weaponItem) {
        Weapon weapon = obj_weapon.AddComponent<Weapon>();
        weapon.itemWeapon = weaponItem;
        return weapon;
    }

    private void ResetOutfitDefault() {
        foreach (Transform a in trans_parentHairPlayer1) {
            Destroy(a.gameObject);
        }
        foreach (Transform a in trans_parentHairPlayer2) {
            Destroy(a.gameObject);
        }
        foreach (Transform a in trans_parentShieldPlayer) {
            Destroy(a.gameObject);
        }
        foreach (Transform a in trans_parentWingPlayer) {
            Destroy(a.gameObject);
        }
        foreach (Transform a in trans_parentTailPlayer) {
            Destroy(a.gameObject);
        }

        Renderer rendererSkin = obj_skinPlayer.GetComponent<Renderer>();
        Material[] skinMaterials = rendererSkin.materials;
        for (int i = 0; i < skinMaterials.Length; i++) {
            skinMaterials[i] = mat_skinStart;
        }
        rendererSkin.materials = skinMaterials;

        Renderer rendererPant = obj_pantPlayer.GetComponent<Renderer>();
        Material[] pantMaterials = rendererPant.materials;
        for (int i = 0; i < pantMaterials.Length; i++) {
            pantMaterials[i] = mat_skinStart;
        }
        rendererPant.materials = pantMaterials;
    }

    public void LoadOutfitPlayer() {
        ResetOutfitDefault();
        if (!string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentHairID)) {
            ItemBase hair = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentHairID); ;
            if (!hair.highHair) {
                Instantiate(hair.modelPrefab, trans_parentHairPlayer1);
            }
            else {
                Instantiate(hair.modelPrefab, trans_parentHairPlayer2);
            }
        }
        if (!string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentPantID)) {
            ItemBase pant = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentPantID);
            Renderer renderer = obj_pantPlayer.GetComponent<Renderer>();
            Material[] mats = renderer.materials;
            for (int i = 0; i < mats.Length; i++) {
                mats[i] = pant.outfitMaterial;
            }
            renderer.materials = mats;
        }
        if (!string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentShieldID)) {
            ItemBase shield = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentShieldID);
            Instantiate(shield.modelPrefab, trans_parentShieldPlayer);
        }
        if (!string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentSetID)) {
            ItemBase set = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentSetID);

            Renderer rendererSkinSet = obj_skinPlayer.GetComponent<Renderer>();
            Material[] skinMaterialsSet = rendererSkinSet.materials;
            for (int i = 0; i < skinMaterialsSet.Length; i++) {
                skinMaterialsSet[i] = set.skinMaterial;
            }
            rendererSkinSet.materials = skinMaterialsSet;

            Renderer rendererPantSet = obj_pantPlayer.GetComponent<Renderer>();
            Material[] pantMaterialsSet = rendererPantSet.materials;
            for (int i = 0; i < pantMaterialsSet.Length; i++) {
                pantMaterialsSet[i] = set.skinMaterial;
            }
            rendererPantSet.materials = pantMaterialsSet;

            foreach (ItemBase it in set.subItems) {
                if (it.itemType == ItemType.Hair) {
                    if (!it.highHair) {
                        if (trans_parentHairPlayer1 != null) {
                            GameObject hairPlayer = Instantiate(it.modelPrefab, trans_parentHairPlayer1);
                        }
                        else {
                            Debug.Log("Chua co cho gan hair set");
                        }
                    }
                    else {
                        if (trans_parentHairPlayer2 != null) {
                            GameObject hairPlayer = Instantiate(it.modelPrefab, trans_parentHairPlayer2);
                        }
                        else {
                            Debug.Log("Chua co cho gan hair set");
                        }
                    }
                }
                if (it.itemType == ItemType.Shield) {
                    if (trans_parentShieldPlayer != null) {
                        GameObject shieldPlayer = Instantiate(it.modelPrefab, trans_parentShieldPlayer);
                    }
                    else {
                        Debug.Log("Chua co cho gan wing set");
                    }
                }
                if (it.itemType == ItemType.Wing) {
                    if (trans_parentWingPlayer != null) {
                        GameObject wingPlayer = Instantiate(it.modelPrefab, trans_parentWingPlayer);
                    }
                    else {
                        Debug.Log("Chua co cho gan wing set");
                    }
                }
                if (it.itemType == ItemType.Tail) {
                    if (trans_parentTailPlayer != null) {
                        GameObject wingPlayer = Instantiate(it.modelPrefab, trans_parentTailPlayer);
                    }
                    else {
                        Debug.Log("Chua co cho gan tail set");
                    }
                }
            }
        }
    }

    //Zombie setup poolWeapon
    public void SetupPoolWeaponZombie() {
        int int_bullet = Player.instance.playerController.int_bulletCount;
        for (int i = 0; i < int_bullet; i++) {
            AddPoolWeaponZombie();
        }
    }

    public Weapon AddPoolWeaponZombie() {
        GameObject obj_weaponTemp = SetupWeapon(weaponItemOwn, true);
        Weapon weaponAttackScriptTemp = SetupScriptWeaponAttack(obj_weaponTemp, weaponItemOwn);
        if (abilityApplyWeapon != null) ApplyAbilityForWeapon(weaponAttackScriptTemp);
        list_weaponSpawnZombie.Add((obj_weaponTemp, weaponAttackScriptTemp));
        return weaponAttackScriptTemp;
    }

    public void ApplyAbilityForListWeapon(AbilityBase ability) {
        abilityApplyWeapon = ability;
        for (int i = 0; i < list_weaponSpawnZombie.Count; i++) {
            ApplyAbilityForWeapon(list_weaponSpawnZombie[i].weaponScript);
        }
    }

    public void ApplyAbilityForWeapon(Weapon weaponScriptTemp) {
        if (abilityApplyWeapon.abilityType == AbilityType.AttackEffectAbility) {
            abilityApplyWeapon.ApplyAttackEffectAbility(weaponScriptTemp);
        }
        else if (abilityApplyWeapon.abilityType == AbilityType.EconomyAbility) {
            abilityApplyWeapon.ApplyEconomyAbility(weaponScriptTemp);
        }
    }

    public Weapon GetBullet() {
        // Tìm viên đạn chưa dùng
        foreach (var weapon in list_weaponSpawnZombie) {
            if (!weapon.obj_weaponAttack.activeInHierarchy) {
                return weapon.weaponScript;
            }
        }
        return AddPoolWeaponZombie();
    }
}
