using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSkin : MonoBehaviour
{
    [Header("------------Ref------------")]
    public HairShop hairShop;
    public PantShop pantShop;
    public ShieldShop shieldShop;
    public SetShop setShop;
    public GameObject obj_playerClonePrefab;
    public Player_Clone player_Clone;
    public Button btn_exitSkinShop;
    public Button btn_openHairShop;
    public Button btn_openPantShop;
    public Button btn_openShieldShop;
    public Button btn_openSetShop;
    private Button btn_previousShop;
    public TextMeshProUGUI txt_effectItem;
    public GameObject obj_groupBtnBuy;
    public Button btn_buyItem;
    public TextMeshProUGUI txt_priceItem;
    public Button btn_getItemADS;
    public Button btn_actionItem;
    public Image img_btnActionItem;
    public Sprite sprite_equipItem;
    public Sprite sprite_unequipItem;
    public TextMeshProUGUI txt_btnActionItem;
    public ItemBase chosseItem;

    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;
    public void Init() {
        gameObject.SetActive(true);
        if(!isInit) {
            isInit = true;
            SetupBtnSkinShop();
            player_Clone = Instantiate(obj_playerClonePrefab, Player.instance.transform.position, Player.instance.transform.rotation).GetComponent<Player_Clone>();
        }
        Player.instance.gameObject.SetActive(false);
        player_Clone.gameObject.SetActive(true);
        BtnOpenShopOnClick(btn_openHairShop);
        hairShop.Init();
        pantShop.gameObject.SetActive(false);
        shieldShop.gameObject.SetActive(false);
        setShop.gameObject.SetActive(false);
        UIControllerNormal.instance.uiMenuStart.obj_cameraMainPlay.SetActive(false);
        UIControllerNormal.instance.uiMenuStart.obj_cameraShopSkin.SetActive(true);
    }

    public void SetupBtnSkinShop() {
        btn_exitSkinShop.onClick.RemoveAllListeners();
        btn_openHairShop.onClick.RemoveAllListeners();
        btn_openPantShop.onClick.RemoveAllListeners();
        btn_openShieldShop.onClick.RemoveAllListeners();
        btn_openSetShop.onClick.RemoveAllListeners();

        btn_exitSkinShop.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            Close();
        });
        btn_openHairShop.onClick.AddListener(() => {
            if (!BtnOpenShopOnClick(btn_openHairShop)) return;
            AudioManager.Ins.PlaySound_ButtonClick();
            pantShop.Close();
            shieldShop.Close();
            hairShop.Init();
            setShop.Close();
        });
        btn_openPantShop.onClick.AddListener(() => {
            if (!BtnOpenShopOnClick(btn_openPantShop)) return;
            AudioManager.Ins.PlaySound_ButtonClick();
            hairShop.Close();
            shieldShop.Close();
            pantShop.Init();
            setShop.Close();
        });
        btn_openShieldShop.onClick.AddListener(() => {
            if (!BtnOpenShopOnClick(btn_openShieldShop)) return;
            AudioManager.Ins.PlaySound_ButtonClick();
            hairShop.Close();
            pantShop.Close();
            shieldShop.Init();
            setShop.Close();
        });
        btn_openSetShop.onClick.AddListener(() => {
            if (!BtnOpenShopOnClick(btn_openSetShop)) return;
            AudioManager.Ins.PlaySound_ButtonClick();
            hairShop.Close();
            pantShop.Close();
            shieldShop.Close();
            setShop.Init();
        });

        btn_buyItem.onClick.AddListener(() => {
            if(DataManager.Ins.gameSave.coin >= chosseItem.price) {
                AudioManager.Ins.PlaySound_ButtonClick();
                DataManager.Ins.SaveBuyItem(chosseItem.id, chosseItem.price, chosseItem.itemType);
                UIControllerNormal.instance.uiMenuStart.LoadCoinOwn();
                UpdateLockAfterBuy();
                SetupGroupBuyBtn();
            } else {
                Debug.Log("Tiền đâu");
            }
        });
        btn_getItemADS.onClick.AddListener(() => {
            DataManager.Ins.SaveBuyItem(chosseItem.id, 0, chosseItem.itemType);
            AudioManager.Ins.PlaySound_ButtonClick();
            UpdateLockAfterBuy();
            SetupGroupBuyBtn();
        });
        btn_actionItem.onClick.AddListener(() => {
            DataManager.Ins.UpdateEquip(chosseItem.id, chosseItem.itemType);
            AudioManager.Ins.PlaySound_ButtonClick();
            SetupGroupBuyBtn();
        });
    }

    public void UpdateLockAfterBuy() {
        if (chosseItem.itemType == ItemType.Hair) {
            hairShop.itemPrefabChooseItem.UpdateUnlockItem(chosseItem);
        }
        else if (chosseItem.itemType == ItemType.Pant) {
            pantShop.itemPrefabChooseItem.UpdateUnlockItem(chosseItem);
        }
        else if (chosseItem.itemType == ItemType.Shield) {
            shieldShop.itemPrefabChooseItem.UpdateUnlockItem(chosseItem);
        }
        else if (chosseItem.itemType == ItemType.Set) {
            //setShop.itemPrefabChooseItem.UpdateUnlockItem(chosseItem);
        }
    }

    public void Close() {
        player_Clone.gameObject.SetActive(false);
        Player.instance.gameObject.SetActive(true);
        BtnOpenShopOnClick(btn_openHairShop);
        hairShop.Close();
        pantShop.Close();
        UIControllerNormal.instance.uiMenuStart.obj_cameraMainPlay.SetActive(true);
        UIControllerNormal.instance.uiMenuStart.obj_cameraShopSkin.SetActive(false);
        UIControllerNormal.instance.uiMenuStart.SetupAnimExitShop();
        gameObject.SetActive(false);
    }

    private bool BtnOpenShopOnClick(Button btnClick) {
        if(btn_previousShop != null) {
            if (btnClick == btn_previousShop) return false;
            Image img_btnOnClick = btnClick.GetComponent<Image>();
            Color colorBtn = img_btnOnClick.color;
            colorBtn.a = 0f;
            img_btnOnClick.color = colorBtn;
            Image img_btnPreviousShop = btn_previousShop.GetComponent<Image>();
            Color colorBtnPrevious = img_btnOnClick.color;
            colorBtnPrevious.a = 150 / 255f;
            img_btnPreviousShop.color = colorBtnPrevious;
            btn_previousShop = btnClick;
            return true;
        } else {
            btn_previousShop = btnClick;
            Image img_btnOnClick = btnClick.GetComponent<Image>();
            Color colorBtn = img_btnOnClick.color;
            colorBtn.a = 0f;
            img_btnOnClick.color = colorBtn;
            return true;
        }
    }

    public void SetupGroupBuyBtn() {
        txt_effectItem.text = "+ " + chosseItem.atributes.valueString + " " + chosseItem.atributes.AtributeName;
        if (DataManager.Ins.gameSave.list_HairIDOwn.Contains(chosseItem.id) || DataManager.Ins.gameSave.list_PantIDOwn.Contains(chosseItem.id)
            || DataManager.Ins.gameSave.list_ShieldIDOwn.Contains(chosseItem.id) || DataManager.Ins.gameSave.list_SetIDOwn.Contains(chosseItem.id)) {
            obj_groupBtnBuy.SetActive(false);
            btn_actionItem.gameObject.SetActive(true);
            //Đổi sprite và chữ UI nút equip
            if (DataManager.Ins.gameSave.str_currentHairID == chosseItem.id || DataManager.Ins.gameSave.str_currentPantID == chosseItem.id
            || DataManager.Ins.gameSave.str_currentShieldID == chosseItem.id || DataManager.Ins.gameSave.str_currentSetID == chosseItem.id) {
                img_btnActionItem.sprite = sprite_unequipItem;
                txt_btnActionItem.text = "Unequip";
            }
            else {
                img_btnActionItem.sprite = sprite_equipItem;
                txt_btnActionItem.text = "Equip";
            }
        }
        else {
            btn_actionItem.gameObject.SetActive(false);
            obj_groupBtnBuy.SetActive(true);
            txt_priceItem.text = chosseItem.price.ToString();
        }
    }
}
