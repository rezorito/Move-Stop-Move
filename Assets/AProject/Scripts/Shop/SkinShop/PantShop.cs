using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PantShop : MonoBehaviour
{
    public UIShopSkin uiShopSkin;
    public GameObject obj_itemPrefab;
    public Transform trams_contentParent;
    public Material mat_skinStart;

    private List<(Material material, ItemPrefabs itemPrefabs, ItemBase item)> list_pantIns = new List<(Material, ItemPrefabs, ItemBase)>();
    private bool isPlayerPant;
    public ItemPrefabs itemPrefabChooseItem = null;
    private ItemPrefabs itemPrefabPrevious = null;
    private ItemBase chooseItem = null;

    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if (!isInit) {
            isInit = true;
            GenerateShop();
        }
        SetupStartItem();
    }
    public void Close() {
        gameObject.SetActive(false);
    }

    void GenerateShop() {
        foreach (ItemBase item in DataManager.Ins.list_PantData) {
            ItemPrefabs itemprefab = Instantiate(obj_itemPrefab, trams_contentParent).GetComponent<ItemPrefabs>();
            //Mở khóa các cái nhân vật đã có
            itemprefab.Init(item);
            list_pantIns.Add((null, itemprefab, item));
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
        isPlayerPant = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentPantID);
        if (!isPlayerPant) {
            chooseItem = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentPantID);
            uiShopSkin.chosseItem = chooseItem;
            var result = list_pantIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
        else {
            chooseItem = DataManager.Ins.list_PantData[0];
            var result = list_pantIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
    }
   
    public void ActionWithItem(ItemBase itemBase, ItemPrefabs itemPrefabs) {
        itemPrefabChooseItem = itemPrefabs;
        uiShopSkin.chosseItem = itemBase;
        uiShopSkin.SetupGroupBuyBtn();
        UpdateUIChangeSelectedItem(itemPrefabs);
        int index = list_pantIns.FindIndex(x => x.item == itemBase);
        if (index >= 0) {
            Material material = InsItem(itemBase);
            if (list_pantIns[index].material == null) {
                list_pantIns[index] = (material, itemPrefabs, itemBase);
            }
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


    public Material InsItem(ItemBase item) {
        Material[] pantMaterials = uiShopSkin.player_Clone.rend_pant.materials;
        for (int i = 0; i < pantMaterials.Length; i++) {
            pantMaterials[i] = item.outfitMaterial;
        }
        uiShopSkin.player_Clone.rend_pant.materials = pantMaterials;
        return item.outfitMaterial;
    }


    public void ResetPant() {
        Material[] pantMaterials = uiShopSkin.player_Clone.rend_pant.materials;
        for (int i = 0; i < pantMaterials.Length; i++) {
            pantMaterials[i] = mat_skinStart;
        }
        uiShopSkin.player_Clone.rend_pant.materials = pantMaterials;
    }

    public void OnDisable() {
        chooseItem = null;
        if (itemPrefabPrevious != null) itemPrefabPrevious.UIUnselectItem();
        itemPrefabPrevious = null;
        itemPrefabChooseItem = null;
        ResetPant();
    }
}
