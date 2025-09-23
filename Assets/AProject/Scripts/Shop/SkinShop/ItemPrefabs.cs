using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefabs : MonoBehaviour
{
    public Button btn_selectItem;
    public GameObject obj_outBorderSelected;
    public GameObject obj_lockItem;
    public Image img_iconItem;
    public GameObject obj_showStatusEquip;

    public void Init(ItemBase itemBase) {
        img_iconItem.sprite = itemBase.icon;
        if (DataManager.Ins.gameSave.list_HairIDOwn.Contains(itemBase.id)) {
            obj_lockItem.SetActive(false);
        }
        else {
            obj_lockItem.SetActive(true);
        }
    }

    public void UIUpdateEquipedItem() {
        obj_showStatusEquip.SetActive(true);
    }

    public void UIUpdateUnequipedItem() {
        obj_showStatusEquip.SetActive(false);
    }

    public void UISelectItem() {
        obj_outBorderSelected.SetActive(true);
    }

    public void UIUnselectItem() {
        obj_outBorderSelected.SetActive(false);
    }


    public void UpdateUnlockItem(ItemBase itemBase) {
        obj_lockItem.SetActive(false);
    }
}
