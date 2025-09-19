using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkinEnemy : MonoBehaviour
{
    [Header("------------Ref------------")]
    public EnemyIndicatorController enemyIndicatorController;
    public Billboard billBoard;
    public Transform trans_parentWeaponShow;
    public Transform trans_poolWeapon;
    public Renderer rend_Skin;
    public Renderer rend_Pant;
    public Transform trans_parentHair1;
    public Transform trans_parentHair2;
    public Transform trans_parentShield;
    public Transform trans_parentWing;
    public Transform trans_parentTail;

    public GameObject obj_weaponShow;
    public GameObject obj_weaponAttack;
    private GameObject obj_shieldShow;
    private GameObject obj_hairShow;
    private GameObject obj_wingShow;
    private GameObject obj_tailShow;
    public Weapon weaponAttackScript;


    private List<(ItemBase item, GameObject obj_weaponShow, GameObject obj_weaponAttack, Weapon weaponScript)> list_WeaponIns = new List<(ItemBase, GameObject, GameObject, Weapon)>();
    private List<(ItemBase item, GameObject obj_hair)> list_HairIns = new List<(ItemBase, GameObject)>();
    private List<(ItemBase item, GameObject obj_shield)> list_ShieldIns = new List<(ItemBase, GameObject)>();
    private List<(ItemBase item, GameObject obj_hair, GameObject obj_wing, GameObject obj_tail)> list_SetIns = new List<(ItemBase, GameObject, GameObject, GameObject)>();

    private List<ItemBase> list_WeaponData = new List<ItemBase>();
    private List<ItemBase> list_PantsData = new List<ItemBase>();
    private List<ItemBase> list_HairData = new List<ItemBase>();
    private List<ItemBase> list_ShieldData = new List<ItemBase>();
    private List<ItemBase> list_SetData = new List<ItemBase>();

    public Color colorUsed;
    public void Awake() {
        if(GameManager.instance.currentMode == GameMode.Normal) {
            list_WeaponIns.Clear();
            list_HairIns.Clear();
            list_ShieldIns.Clear();
            list_SetIns.Clear();
            list_WeaponData.Clear();
            list_PantsData.Clear();
            list_HairData.Clear();
            list_ShieldData.Clear();
            list_SetData.Clear();
            list_WeaponData = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Weapon).ToList();
            list_PantsData = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Pant).ToList();
            list_ShieldData = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Shield).ToList();
            list_HairData = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Hair).ToList();
            list_SetData = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Set).ToList();
        }
    }

    public void Init() {
        SetupWeapon();
        SetupOutfit();
    }

    public void SetupWeapon() { //Setup weapon hiển thị & tấn công
        if (list_WeaponData != null) {
            if(obj_weaponShow != null) obj_weaponShow.SetActive(false);
            ItemBase weaponItem = list_WeaponData[Random.Range(0, list_WeaponData.Count)];
            var foundWeapon = list_WeaponIns.FirstOrDefault(x => x.item == weaponItem);
            if (foundWeapon.item != null) {
                obj_weaponShow = foundWeapon.obj_weaponShow;
                obj_weaponShow.SetActive(true);
                obj_weaponAttack = foundWeapon.obj_weaponAttack;
                weaponAttackScript = foundWeapon.weaponScript;
                return;
            }
            GameObject weaponshow = obj_weaponShow = Instantiate(weaponItem.modelPrefab, trans_parentWeaponShow);
            GameObject weaponAttack = obj_weaponAttack = Instantiate(weaponItem.modelPrefab, Player.instance.playerController.skinPlayer.trans_poolWeapon, false);
            obj_weaponAttack.SetActive(false);
            Weapon weaponScript = obj_weaponAttack.AddComponent<Weapon>();
            weaponScript.itemWeapon = weaponItem;
            weaponAttackScript = weaponScript;
            list_WeaponIns.Add((weaponItem, weaponshow, weaponAttack, weaponScript));
        }
    }

    public void SetupOutfit() { //setup outfit enemy
        bool isHaveSet = Random.Range(0f, 1f) >= 0.7;
        Color randomColor = SetupColor();
        Material matColor = CreateMaterialColor(randomColor);
        if (isHaveSet) {
            SetupSet(list_SetData[Random.Range(0, list_SetData.Count)]);
        }
        else {
            bool isHavePant = Random.Range(0, 2) == 1;
            bool isHaveHair = Random.Range(0, 2) == 1;
            bool isHaveShield = Random.Range(0, 2) == 1;
            //Set màu nhân vật
            SetupSkin(matColor);
            //Set quần
            if (isHavePant && list_PantsData != null) {
                SetupPant(list_PantsData[Random.Range(0, list_PantsData.Count)]);
            }
            else {
                SetupPant(null, matColor);
            }
            //Set mũ
            if (isHaveHair && list_HairData != null) {
                SetupHair(list_HairData[Random.Range(0, list_HairData.Count)]);
            }
            else {
                if (obj_hairShow != null) obj_hairShow.SetActive(false);
            }
            //Set Khiên
            if (isHaveShield && obj_shieldShow != null) {
                SetupShield(list_ShieldData[Random.Range(0, list_ShieldData.Count)]);
            }
            else {
                if (obj_shieldShow != null) obj_shieldShow.SetActive(false);
            }
        }
    }

    public void SetupSet(ItemBase setItem) {
        var foundSet = list_SetIns.FirstOrDefault(x => x.item == setItem);
        if (setItem.skinMaterial != null) {
            SetupSkin(setItem.skinMaterial);
            SetupPant(null, setItem.skinMaterial);
        } else {
            Debug.Log("Set ko có skin");
        }
        if (foundSet.item != null) {
            foreach (ItemBase it in setItem.subItems) {
                if (it.itemType == ItemType.Hair) foundSet.obj_hair.SetActive(true);
                if (it.itemType == ItemType.Wing) foundSet.obj_wing.SetActive(true);
                if (it.itemType == ItemType.Tail) foundSet.obj_tail.SetActive(true);
            }
        }
        else {
            GameObject obj_hair = null;
            GameObject obj_wing = null;
            GameObject obj_tail = null;
            foreach (ItemBase it in setItem.subItems) {
                if (it.itemType == ItemType.Hair) obj_hair = SetupHair(it);
                if (it.itemType == ItemType.Wing) obj_wing = SetupWing(it, false);
                if (it.itemType == ItemType.Tail) obj_tail = SetupTail(it, false);
            }
            list_SetIns.Add((setItem, obj_hair, obj_wing, obj_tail));
        }
    }

    public void SetupSkin(Material mat_randomColor) {
        Material[] skinMaterials = rend_Skin.materials;
        for (int i = 0; i < skinMaterials.Length; i++) {
            skinMaterials[i] = mat_randomColor;
        }
        rend_Skin.materials = skinMaterials;
    }

    public void SetupPant(ItemBase pantItem = null, Material mat_randomColor = null) {
        if(pantItem != null) {
            Material[] mats = rend_Pant.materials;
            for (int i = 0; i < mats.Length; i++) {
                mats[i] = pantItem.outfitMaterial;
            }
            rend_Pant.materials = mats;
        } else {
            Material[] mats = rend_Pant.materials;
            for (int i = 0; i < mats.Length; i++) {
                mats[i] = mat_randomColor;
            }
            rend_Pant.materials = mats;
        }
    }

    public GameObject SetupHair(ItemBase hairItem) {
        if (obj_hairShow != null) obj_hairShow.SetActive(false);
        if (hairItem != null) {
            var foundHair = list_HairIns.FirstOrDefault(x => x.item == hairItem);
            if (foundHair.item != null) {
                obj_hairShow = foundHair.obj_hair;
                obj_hairShow.SetActive(true);
                return null;
            }
            else {
                GameObject obj_hair;
                if (!hairItem.highHair) {
                    obj_hairShow = obj_hair = Instantiate(hairItem.modelPrefab, trans_parentHair1);
                }
                else {
                    obj_hairShow = obj_hair = Instantiate(hairItem.modelPrefab, trans_parentHair2);
                }
                obj_hairShow.SetActive(true);
                list_HairIns.Add((hairItem, obj_hair));
                return obj_hair;
            }
        }
        Debug.Log("Error!");
        return null;
    }

    public GameObject SetupShield(ItemBase shieldItem) {
        if (obj_shieldShow != null) obj_shieldShow.SetActive(false);
        if (shieldItem != null) {
            var foundShield = list_ShieldIns.FirstOrDefault(x => x.item == shieldItem);
            if (foundShield.item != null) {
                obj_shieldShow = foundShield.obj_shield;
                obj_shieldShow.SetActive(true);
                return null;
            }
            else {
                GameObject shieldShow = obj_shieldShow = Instantiate(shieldItem.modelPrefab, trans_parentShield);
                obj_shieldShow.SetActive(true);
                list_ShieldIns.Add((shieldItem, shieldShow));
                return shieldShow;
            }
        }
        Debug.Log("Error!");
        return null;
    }

    public GameObject SetupWing(ItemBase wingItem, bool isIns) {
        if (obj_wingShow != null) obj_wingShow.SetActive(false);
        if(!isIns && wingItem != null) {
            GameObject wingShow = obj_wingShow = Instantiate(wingItem.modelPrefab, trans_parentWing);
            obj_wingShow.SetActive(true);
            return wingShow;
        }
        Debug.Log("Error!");
        return null;
    }

    public GameObject SetupTail(ItemBase tailItem, bool isIns) {
        if (obj_tailShow != null) obj_tailShow.SetActive(false);
        if (!isIns && tailItem != null) {
            GameObject tailShow = obj_tailShow = Instantiate(tailItem.modelPrefab, trans_parentTail);
            obj_tailShow.SetActive(true);
            return tailShow;
        }
        Debug.Log("Error!");
        return null;
    }

    //Random màu da / quần (nếu k có) nhân vật
    public Material CreateMaterialColor (Color randomColor) {
        Material tempMat = new Material(Shader.Find("Standard"));
        tempMat.color = randomColor;
        return tempMat;
    }

    public Color SetupColor() {
        colorUsed = ColorManager.instance.GetDistinctRandomColor();
        SetupBillboardAIndicator(colorUsed);
        return colorUsed;
    }

    public void SetupBillboardAIndicator(Color randomColor) {
        enemyIndicatorController.Init(randomColor);
        billBoard.InitColor(randomColor);
    }
}
