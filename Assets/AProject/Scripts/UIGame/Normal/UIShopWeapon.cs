using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopWeapon : MonoBehaviour
{
    //public UIControllerNormal uiControllerNormal;
    [Header("------------Ref------------")]
    public WeaponShopViewer weaponShopViewer;
    public PanelSkinWeapon panelSkinWeapon;

    public Button btn_exitWeaponShop;
    public TextMeshProUGUI txt_nameWeapon;
    public TextMeshProUGUI txt_StatusOwn;
    public TextMeshProUGUI txt_effectWeapon;
    public Button btn_arrowLeft;
    public Button btn_arrowRight;
    public GameObject obj_groupBtnBuyWeapon;
    public Button btn_buyWeapon;
    public TextMeshProUGUI txt_priceWeapon;
    public Button btn_buyWithADS;
    public Button btn_equipWeapon;
    public TextMeshProUGUI txt_btnEquip;
    public Button btn_buySkinWithADS;

    private List<ItemBase> list_ItemsWeapon = new List<ItemBase>();
    public ItemSkinWeapon itemSkinWeapon;   //Script để điều khiển ui btn SkinWeapon

    [Space]
    [Header("------------Variable------------")]
    public int int_indexWeaponChoose;
    public int int_indexWeaponSkinChoose;
    public bool isInit = false;

    public void Init() {
        if (!isInit) {
            isInit = true;
            loadListWeapon();
            setupBtnOnClick();
        }
        int_indexWeaponChoose = list_ItemsWeapon.FindIndex(item => item.id == DataManager.Ins.gameSave.str_currentWeaponID);
        Player.instance.gameObject.SetActive(false);
        gameObject.SetActive(true);
        weaponShopViewer.gameObject.SetActive(true);
        LoadWeapon(list_ItemsWeapon[int_indexWeaponChoose]);
    }

    private void loadListWeapon() {
        list_ItemsWeapon = DataManager.Ins.itemDatabase.allItems.Where(item => item.itemType == ItemType.Weapon).ToList();
        list_ItemsWeapon.Sort((a, b) => a.price.CompareTo(b.price));
    }

    private void LoadWeapon(ItemBase item) {
        txt_nameWeapon.text = item.itemName;
        weaponShopViewer.ShowWeapon(item);
        txt_effectWeapon.text = "+ " + item.atributes.valueString + " " + item.atributes.AtributeName;
        if (DataManager.Ins.gameSave.list_WeaponIDOwn.Contains(item.id)) {
            if(DataManager.Ins.gameSave.str_currentWeaponID == item.id) {
                int_indexWeaponSkinChoose = DataManager.Ins.gameSave.int_skinChooseWeapon;
            } else {
                int_indexWeaponSkinChoose = 1;
            }
            setUIGroupBtn(item);
        }
        else {
            setUIGroupBtn(item);
        }
        panelSkinWeapon.setupUI(item);
    }

    public void setUIGroupBtn(ItemBase item) {
        bool ownItem = DataManager.Ins.gameSave.list_WeaponIDOwn.Contains(item.id);
        if (ownItem) {
            bool lockSkin = DataManager.Ins.gameSave.list_SkinWeaponID.Contains(item.listSkinWeapon[int_indexWeaponSkinChoose].skinName);
            if (lockSkin) {
                btn_buySkinWithADS.gameObject.SetActive(false);
                obj_groupBtnBuyWeapon.SetActive(false);
                btn_equipWeapon.gameObject.SetActive(true);
            }
            else {
                btn_buySkinWithADS.gameObject.SetActive(true);
                obj_groupBtnBuyWeapon.SetActive(false);
                btn_equipWeapon.gameObject.SetActive(false);
            }

            if (item.id == DataManager.Ins.gameSave.str_currentWeaponID) {
                if(int_indexWeaponSkinChoose == DataManager.Ins.gameSave.int_skinChooseWeapon) {
                    txt_btnEquip.text = "Equiped";
                } else {
                    txt_btnEquip.text = "Select";
                }
            }
            else {
                txt_btnEquip.text = "Select";
            }
        }
        else {
            btn_buySkinWithADS.gameObject.SetActive(false);
            obj_groupBtnBuyWeapon.SetActive(true);
            btn_equipWeapon.gameObject.SetActive(false);
            txt_priceWeapon.text = item.price.ToString();
        }
    }

    private void setupBtnOnClick() {
        btn_exitWeaponShop.onClick.RemoveAllListeners();
        btn_arrowLeft.onClick.RemoveAllListeners();
        btn_arrowRight.onClick.RemoveAllListeners();
        btn_equipWeapon.onClick.RemoveAllListeners();
        btn_buyWeapon.onClick.RemoveAllListeners();
        btn_buyWithADS.onClick.RemoveAllListeners();
        btn_buySkinWithADS.onClick.RemoveAllListeners();

        btn_exitWeaponShop.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            Close();
        });
        btn_arrowLeft.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            onClickArrow(-1);
        });
        btn_arrowRight.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            onClickArrow(1);
        });
        btn_equipWeapon.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            DataManager.Ins.UpdateEquip(list_ItemsWeapon[int_indexWeaponChoose].id, ItemType.Weapon);
            DataManager.Ins.UpdateSkinChooseWeapon(int_indexWeaponSkinChoose);
            LoadWeapon(list_ItemsWeapon[int_indexWeaponChoose]);
        });
        btn_buyWeapon.onClick.AddListener(() => {
            if (DataManager.Ins.gameSave.coin > list_ItemsWeapon[int_indexWeaponChoose].price) {
                AudioManager.Ins.PlaySound_ButtonClick();
                DataManager.Ins.SaveBuyItem(list_ItemsWeapon[int_indexWeaponChoose].id, list_ItemsWeapon[int_indexWeaponChoose].price, ItemType.Weapon);
                LoadWeapon(list_ItemsWeapon[int_indexWeaponChoose]);
                UIControllerNormal.instance.uiMenuStart.LoadCoinOwn();
            } else {
                Debug.Log("Tiền đâu?");
            }
        });
        btn_buyWithADS.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            DataManager.Ins.SaveBuyItem(list_ItemsWeapon[int_indexWeaponChoose].id, list_ItemsWeapon[int_indexWeaponChoose].price, ItemType.Weapon);
            LoadWeapon(list_ItemsWeapon[int_indexWeaponChoose]);
        });
        btn_buySkinWithADS.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            DataManager.Ins.SaveAddSkinWeapon(list_ItemsWeapon[int_indexWeaponChoose].listSkinWeapon[int_indexWeaponSkinChoose].skinName);
            setUIGroupBtn(list_ItemsWeapon[int_indexWeaponChoose]);
            itemSkinWeapon.setupLockUI(false);
        });
    }

    private void onClickArrow(int direct) {
        if (int_indexWeaponChoose + direct < 0 || int_indexWeaponChoose + direct > list_ItemsWeapon.Count - 1) return;
        int_indexWeaponChoose += direct;
        LoadWeapon(list_ItemsWeapon[int_indexWeaponChoose]);
    }

    public void Close() {
        weaponShopViewer.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Player.instance.gameObject.SetActive(true);
        UIControllerNormal.instance.uiMenuStart.SetupAnimExitShop();
    }
}
