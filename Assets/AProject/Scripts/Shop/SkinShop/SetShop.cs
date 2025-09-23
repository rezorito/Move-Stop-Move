using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class SetShop : MonoBehaviour
{
    public UIShopSkin uiShopSkin;
    public GameObject obj_itemPrefab;
    public Transform trams_contentParent;
    public Material mat_skinStart;

    private List<(Material mat_skin, Material mat_pant ,GameObject obj_hair1, GameObject obj_hair2, GameObject obj_shield, GameObject obj_wing, GameObject obj_tail , ItemPrefabs itemPrefabs, ItemBase item, bool isInit)> list_setIns 
        = new List<(Material, Material, GameObject, GameObject, GameObject, GameObject, GameObject, ItemPrefabs, ItemBase, bool)>();
    public ItemBase chooseItemPrevious = null;
    private bool isPlayerSet;
    public ItemPrefabs itemPrefabChooseItem = null;
    private ItemPrefabs itemPrefabPrevious = null;
    private ItemBase chooseItem = null;
    public ItemPrefabs itemEquipFirst = null;

    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if (!isInit) {
            isInit = true;
            GenerateShop();
        }
        SetupStartItem();
    }

    void GenerateShop() {
        foreach (ItemBase item in DataManager.Ins.list_SetData) {
            ItemPrefabs itemprefab = Instantiate(obj_itemPrefab, trams_contentParent).GetComponent<ItemPrefabs>();
            //Mở khóa các cái nhân vật đã có
            itemprefab.Init(item);
            list_setIns.Add((null, null, null, null, null, null, null, itemprefab, item, false));
            itemprefab.btn_selectItem.onClick.RemoveAllListeners();
            itemprefab.btn_selectItem.onClick.AddListener(() => {
                if (itemprefab == itemPrefabChooseItem) return;
                AudioManager.Ins.PlaySound_ButtonClick();
                chooseItem = item;
                ActionWithItem(item, itemprefab);
            });
        }

    }

    public void SetupStartItem() {
        isPlayerSet = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentSetID);
        if (!isPlayerSet) {
            chooseItem = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentSetID);
            uiShopSkin.chosseItem = chooseItem;
            var result = list_setIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
                itemEquipFirst = result.itemPrefabs;
                itemEquipFirst.UIUpdateEquipedItem();
            }
        }
        else {
            chooseItem = DataManager.Ins.list_SetData[0];
            var result = list_setIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
    }
    public void Close() {
        gameObject.SetActive(false);
    }

    public void ActionWithItem(ItemBase itemBase, ItemPrefabs itemPrefabs) {
        ResetSet();
        itemPrefabChooseItem = itemPrefabs;
        uiShopSkin.chosseItem = itemBase;
        uiShopSkin.SetupGroupBuyBtn();
        UpdateUIChangeSelectedItem(itemPrefabs);
        int index = list_setIns.FindIndex(x => x.item == itemBase);
        if (index >= 0) {
            if (!list_setIns[index].isInit) {
                var insItem = InsItem(itemBase);
                list_setIns[index] = (insItem.mat_skin, insItem.mat_pant, insItem.obj_hair1, insItem.obj_hair2, insItem.obj_shield, insItem.obj_wing, insItem.obj_tail, itemPrefabs, itemBase, true);
            }
            InsSkin(list_setIns[index].mat_skin, list_setIns[index].mat_pant);
            if (list_setIns[index].obj_hair1 != null) list_setIns[index].obj_hair1.SetActive(true);
            if (list_setIns[index].obj_hair2 != null) list_setIns[index].obj_hair2.SetActive(true);
            if (list_setIns[index].obj_shield != null) list_setIns[index].obj_shield.SetActive(true);
            if (list_setIns[index].obj_wing != null) list_setIns[index].obj_wing.SetActive(true);
            if (list_setIns[index].obj_tail != null) list_setIns[index].obj_tail.SetActive(true);
        }
    }

    public void UpdateUIChangeSelectedItem(ItemPrefabs itemPrefabs) {
        if (itemPrefabPrevious == null) {
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        }
        else {
            itemPrefabPrevious.UIUnselectItem();
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        }
    }

    public (Material mat_skin, Material mat_pant, GameObject obj_hair1, GameObject obj_hair2, GameObject obj_shield, GameObject obj_wing, GameObject obj_tail) InsItem(ItemBase item) {
        if (uiShopSkin.player_Clone.trans_parentHairHigh1 == null || uiShopSkin.player_Clone.trans_parentHairHigh2 == null ||
            uiShopSkin.player_Clone.trans_parentShield == null || uiShopSkin.player_Clone.trans_parentSwing == null || uiShopSkin.player_Clone.trans_parentTail == null ) {
            Debug.Log("chưa gán vị trí trên đầu");
            return (null, null, null, null, null, null, null);
        }
        Material _mat_skin;
        if (item.skinMaterial != null) _mat_skin = item.skinMaterial;
        else _mat_skin = mat_skinStart;
        InsSkin(_mat_skin, _mat_skin);
        GameObject _obj_hair1 = null;
        GameObject _obj_hair2 = null;
        GameObject _obj_shield = null;
        GameObject _obj_wing = null;
        GameObject _obj_tail = null;
        foreach (ItemBase it in item.subItems) {
            if (it.itemType == ItemType.Hair) {
                if (!it.highHair) {
                    _obj_hair1 = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh1);
                }
                else {
                    _obj_hair2 = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh2);
                }
            } else if(it.itemType == ItemType.Shield) {
                _obj_shield = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentShield);
            }else if (it.itemType == ItemType.Wing) {
                _obj_wing = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentSwing);
            } else if (it.itemType == ItemType.Tail) {
                _obj_tail = Instantiate(it.modelPrefab, uiShopSkin.player_Clone.trans_parentTail);
            }
        }
        return (_mat_skin, _mat_skin, _obj_hair1, _obj_hair2, _obj_shield, _obj_wing, _obj_tail);
    }

    public void InsSkin(Material _mat_skin, Material _mat_pant) {
        Material[] skinMaterials = uiShopSkin.player_Clone.rend_skin.materials;
        for (int i = 0; i < skinMaterials.Length; i++) {
            skinMaterials[i] = _mat_skin;
        }
        uiShopSkin.player_Clone.rend_skin.materials = skinMaterials;
        Material[] pantMaterials = uiShopSkin.player_Clone.rend_pant.materials;
        for (int i = 0; i < pantMaterials.Length; i++) {
            pantMaterials[i] = _mat_pant;
        }
        uiShopSkin.player_Clone.rend_pant.materials = pantMaterials;
    }


    public void ResetSet() {
        if (list_setIns.Count != 0) {
            InsSkin(mat_skinStart, mat_skinStart);
            foreach (var item in list_setIns) {
                if (item.obj_hair1 != null) item.obj_hair1.SetActive(false);
                if (item.obj_hair2 != null) item.obj_hair2.SetActive(false);
                if (item.obj_shield != null) item.obj_shield.SetActive(false);
                if (item.obj_wing != null) item.obj_wing.SetActive(false);
                if (item.obj_tail != null) item.obj_tail.SetActive(false);
            }
        }
    }

    public void OnDisable() {
        chooseItem = null;
        if (itemPrefabPrevious != null) itemPrefabPrevious.UIUnselectItem();
        itemPrefabPrevious = null;
        itemPrefabChooseItem = null;
        ResetSet();
    }

    public void UpdateEquipedItem() {
        if (DataManager.Ins.gameSave.str_currentSetID == chooseItem.id) {
            itemPrefabChooseItem.UIUpdateUnequipedItem();
        }
        else {
            if (itemEquipFirst != null) itemEquipFirst.UIUpdateUnequipedItem();
            itemEquipFirst = itemPrefabChooseItem;
            itemEquipFirst.UIUpdateEquipedItem();
        }
    }
}
