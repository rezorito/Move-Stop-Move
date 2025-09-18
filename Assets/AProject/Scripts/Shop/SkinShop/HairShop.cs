using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class HairShop : MonoBehaviour
{
    public UIShopSkin uiShopSkin;
    public GameObject obj_itemPrefab;
    public Transform trams_contentParent;
    public GameObject obj_currentHair;

    private List<(GameObject obj, ItemPrefabs itemPrefabs, ItemBase item)> list_hairIns = new List<(GameObject, ItemPrefabs, ItemBase)>();
    private bool isPlayerHair;
    public ItemPrefabs itemPrefabChooseItem = null;
    private ItemPrefabs itemPrefabPrevious = null;
    private ItemBase chooseItem = null;

    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if(!isInit) {
            isInit = true;
            GenerateShop();
        }
        SetupStartItem();
    }

    void GenerateShop() {
        foreach (ItemBase item in DataManager.Ins.list_HairData) {
            ItemPrefabs itemprefab = Instantiate(obj_itemPrefab, trams_contentParent).GetComponent<ItemPrefabs>();
            //Mở khóa các cái nhân vật đã có
            itemprefab.Init(item);
            list_hairIns.Add((null, itemprefab, item));
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
        isPlayerHair = string.IsNullOrEmpty(DataManager.Ins.gameSave.str_currentHairID);
        if (!isPlayerHair) {
            chooseItem = DataManager.Ins.itemDatabase.GetItemById(DataManager.Ins.gameSave.str_currentHairID);
            uiShopSkin.chosseItem = chooseItem;
            var result = list_hairIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
        else {
            chooseItem = DataManager.Ins.list_HairData[0];
            var result = list_hairIns.FirstOrDefault(x => x.item == chooseItem);
            if (result.item != null) {
                ActionWithItem(result.item, result.itemPrefabs);
            }
        }
    }
    public void Close() {
        gameObject.SetActive(false);
    }

    public void ActionWithItem(ItemBase itemBase, ItemPrefabs itemPrefabs) {
        if(obj_currentHair != null) obj_currentHair.SetActive(false);
        itemPrefabChooseItem = itemPrefabs;
        uiShopSkin.chosseItem = itemBase;
        uiShopSkin.SetupGroupBuyBtn();
        UpdateUIChangeSelectedItem(itemPrefabs);
        int index = list_hairIns.FindIndex(x => x.item == itemBase);
        if (index >= 0) {
            if (list_hairIns[index].obj == null) {
                GameObject obj = InsItem(itemBase);
                list_hairIns[index] = (obj, itemPrefabs, itemBase);
            }
            obj_currentHair = list_hairIns[index].obj;
            obj_currentHair.SetActive(true);
        }
    }

    public void UpdateUIChangeSelectedItem(ItemPrefabs itemPrefabs) {
        if(itemPrefabPrevious == null) {
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        } else {
            itemPrefabPrevious.UIUnselectItem();
            itemPrefabs.UISelectItem();
            itemPrefabPrevious = itemPrefabs;
        }
    }
    

    public GameObject InsItem(ItemBase item) {
        if (uiShopSkin.player_Clone.trans_parentHairHigh1 == null && uiShopSkin.player_Clone.trans_parentHairHigh2 == null) {
            Debug.Log("chưa gán vị trí trên đầu");
        }
        if (item.modelPrefab != null) {
            if (!item.highHair) {
                GameObject modelPrefab = Instantiate(item.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh1);
                modelPrefab.SetActive(false);
                return modelPrefab;
            }
            else {
                GameObject modelPrefab = Instantiate(item.modelPrefab, uiShopSkin.player_Clone.trans_parentHairHigh2);
                modelPrefab.SetActive(false);
                return modelPrefab;
            }
        }
        Debug.Log("???");
        return null;
    }


    public void ResetHair() {
        if(list_hairIns.Count != 0) {
            foreach(var item in list_hairIns) {
                if(item.obj != null) {
                    item.obj.SetActive(false);
                }
            }
        }
    }

    public void OnDisable() {
        chooseItem = null;
        obj_currentHair = null;
        if (itemPrefabPrevious != null) itemPrefabPrevious.UIUnselectItem();
        itemPrefabPrevious = null;
        itemPrefabChooseItem = null;
        ResetHair();
    }
}
