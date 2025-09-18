using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelSkinWeapon : MonoBehaviour
{
    public static PanelSkinWeapon instance;
    //public WeaponShop weaponShop;
    public UIShopWeapon uiShopWeapon;
    public WeaponShopViewer weaponShopViewer;

    public GameObject skinWeaponShowPrefab;
    public Transform panelSkinWeapon;
    public GameObject objectLock;
    List<SkinWeapon> listSkinWeapon = new List<SkinWeapon>();
    private Button previousButton = null;

    public GameObject panelChooseColorMaterial;
    public Transform panelChooseMaterial;
    public Transform panelChooseColor;
    public GameObject btnMaterial;
    public Button[] btnColors;
    private Button previousButtonMaterial = null;
    private int currentMaterialIndex = 0;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void setupUI(ItemBase item)
    {
        if(DataManager.Ins.gameSave.list_WeaponIDOwn.Contains(item.id)) {
            objectLock.SetActive(false);
            panelSkinWeapon.gameObject.SetActive(true);
            setupPanelSkinWeapon(item);
            if (item.listSkinWeapon[uiShopWeapon.int_indexWeaponSkinChoose].isCustom)
            {
                panelChooseColorMaterial.SetActive(true);
            } else
            {
                panelChooseColorMaterial.SetActive(false);
            }
        } else
        {
            objectLock.SetActive(true);
            panelSkinWeapon.gameObject.SetActive(false);
            panelChooseColorMaterial.SetActive(false);
        }
    }

    public void setupSkin(GameObject item, Material material)
    {

    }

    public void setupSkinCustom(GameObject item, Material[] materials)
    {

    }

    public void setupPanelSkinWeapon(ItemBase item)
    {
        foreach (Transform child in panelSkinWeapon)
        {
            Destroy(child.gameObject);
        }
        listSkinWeapon = item.listSkinWeapon;
        if (listSkinWeapon == null || listSkinWeapon.Count == 0) return;
        for (int i = 0; i < listSkinWeapon.Count; i++)
        {
            SkinWeapon skinWeapon = listSkinWeapon[i];
            Debug.Log(skinWeapon.customMaterial);
            GameObject newitem = Instantiate(skinWeaponShowPrefab, panelSkinWeapon);
            var scriptItemSkin = newitem.GetComponent<ItemSkinWeapon>();
            scriptItemSkin.setupSpriteSkin(skinWeapon.spriteSkin);
            scriptItemSkin.setupLockUI(!DataManager.Ins.gameSave.list_SkinWeaponID.Contains(skinWeapon.skinName));
            scriptItemSkin.setupCustomText(skinWeapon.isCustom);
            Button btn = newitem.GetComponent<Button>();
            if(DataManager.Ins.gameSave.str_currentWeaponID == item.id) {
                if (i == DataManager.Ins.gameSave.int_skinChooseWeapon) {
                    scriptItemSkin.setupChooseUI(true);
                    previousButton = btn; 
                } else {
                    scriptItemSkin.setupChooseUI(false);
                }
            } else {
                if(i == 1) {
                    scriptItemSkin.setupChooseUI(true);
                    previousButton = btn;
                } else {
                    scriptItemSkin.setupChooseUI(false);
                }
            }
            int index = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                AudioManager.Ins.PlaySound_ButtonClick();
                if (skinWeapon.isCustom)
                {
                    weaponShopViewer.SetupMaterialCustom(skinWeapon.customMaterial);
                    setupCustomMaterial(item, skinWeapon);
                }
                else
                {
                    weaponShopViewer.SetupMaterial(skinWeapon.skinMaterial);
                    panelChooseColorMaterial.SetActive(false);
                }
                uiShopWeapon.int_indexWeaponSkinChoose = index; ;
                uiShopWeapon.setUIGroupBtn(item);
                uiShopWeapon.itemSkinWeapon = scriptItemSkin;
                setupChooseBtn(btn);
            });
        }

    }

    public void setupChooseBtn(Button button)
    {
        if (previousButton != null)
        {
            previousButton.GetComponent<ItemSkinWeapon>().setupChooseUI(false);
        }

        button.GetComponent<ItemSkinWeapon>().setupChooseUI(true);
        previousButton = button;
    }

    public void setupCustomMaterial(ItemBase item, SkinWeapon skinWeapon)
    {
        panelChooseColorMaterial.SetActive(true);
        foreach (Transform child in panelChooseMaterial)
        {
            Destroy(child.gameObject);
        }
        for (int i=0; i<skinWeapon.customMaterial.Length; i++)
        {
            int index = i;
            GameObject btnObject = Instantiate(btnMaterial, panelChooseMaterial);
            Button btn = btnObject.GetComponent<Button>();
            btn.GetComponent<Image>().color = skinWeapon.customMaterial[i].color;
            if(i==0)
            {
                previousButtonMaterial = btn;
                setupChooseBtnMaterial(btn);
                currentMaterialIndex = index;
            }
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                setupChooseBtnMaterial(btn);
                currentMaterialIndex = index;
            });
        }
        foreach(Button btn in btnColors)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                AudioManager.Ins.PlaySound_ButtonClick();
                UnityEngine.Color color = btn.GetComponent<RawImage>().color;
                item.listSkinWeapon[0].customMaterial[currentMaterialIndex].color = color;
                previousButtonMaterial.GetComponent<Image>().color = color;
            });
        }
    }

    public void setupChooseBtnMaterial(Button button)
    {
        if (previousButton != null)
        {
            previousButtonMaterial.GetComponent<BtnMaterial>().setupChooseUI(false);
        }

        button.GetComponent<BtnMaterial>().setupChooseUI(true);
        previousButtonMaterial = button;
    }
}
