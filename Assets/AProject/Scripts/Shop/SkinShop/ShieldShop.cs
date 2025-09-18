using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ShieldShop : MonoBehaviour
{
    public UIShopSkin uiShopSkin;
    public GameObject obj_itemPrefab;
    public Transform trams_contentParent;
    public GameObject obj_currentShield;

    private List<(GameObject obj, ItemPrefabs itemPrefabs, ItemBase item)> list_shieldIns = new List<(GameObject, ItemPrefabs, ItemBase)>();
    private bool isPlayerShield;
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

    void GenerateShop() {
        foreach (ItemBase item in DataManager.Ins.list_ShieldData) {
            ItemPrefabs itemprefab = Instantiate(obj_itemPrefab, trams_contentParent).GetComponent<ItemPrefabs>();
            //Mở khóa các cái nhân vật đã có
            itemprefab.Init(item);
            list_shieldIns.Add((null, itemprefab, item));
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
        isPlayerShield = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentShieldID);
        if (!isPlayerShield) {
            chooseItem = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentShieldID);
            uiShopSkin.chosseItem = chooseItem;
            var result = list_shieldIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
        else {
            chooseItem = DataManager.Ins.list_ShieldData[0];
            var result = list_shieldIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
    }
    public void Close() {
        gameObject.SetActive(false);
    }

    public void ActionWithItem(ItemBase itemBase, ItemPrefabs itemPrefabs) {
        if (obj_currentShield != null) obj_currentShield.SetActive(false);
        itemPrefabChooseItem = itemPrefabs;
        uiShopSkin.chosseItem = itemBase;
        uiShopSkin.SetupGroupBuyBtn();
        UpdateUIChangeSelectedItem(itemPrefabs);
        int index = list_shieldIns.FindIndex(x => x.item == itemBase);
        if (index >= 0) {
            if (list_shieldIns[index].obj == null) {
                GameObject obj = InsItem(itemBase);
                list_shieldIns[index] = (obj, itemPrefabs, itemBase);
            }
            obj_currentShield = list_shieldIns[index].obj;
            obj_currentShield.SetActive(true);
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


    public GameObject InsItem(ItemBase item) {
        if (uiShopSkin.player_Clone.trans_parentShield == null) {
            Debug.Log("chưa gán vị trí trên đầu");
        }
        if (item.modelPrefab != null) {
            GameObject modelPrefab = Instantiate(item.modelPrefab, uiShopSkin.player_Clone.trans_parentShield);
            modelPrefab.SetActive(false);
            return modelPrefab;
        }
        Debug.Log("???");
        return null;
    }


    public void ResetShield() {
        if (list_shieldIns.Count != 0) {
            foreach (var item in list_shieldIns) {
                if (item.obj != null) {
                    item.obj.SetActive(false);
                }
            }
        }
    }

    public void OnDisable() {
        chooseItem = null;
        obj_currentShield = null;
        if(itemPrefabPrevious != null) itemPrefabPrevious.UIUnselectItem();
        itemPrefabPrevious = null;
        itemPrefabChooseItem = null;
        ResetShield();
    }
}
