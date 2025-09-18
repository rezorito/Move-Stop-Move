using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerNormal : MonoBehaviour
{
    public static UIControllerNormal instance;
    public UIMenuStart uiMenuStart; //Sẽ luôn có ko cần sinh
    public GameObject obj_UIShopWeaponPrefab;
    public GameObject obj_UIShopSkinPrefab;
    public GameObject obj_UITutPrefab;
    public GameObject obj_UIInPlayPrefab;
    public GameObject obj_UIGameEndPrefab;
    public UIShopWeapon uiShopWeapon;
    public UIShopSkin uiShopSkin;
    public UITut uiTut;
    public UIInPlay uiInPlay;
    public UIGameEnd uiGameEnd;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void OpenUIMenuStart() {
        uiMenuStart.Init();
    }

    public void CloseUIMenuStart() {
        uiMenuStart.Close();
    }

    public void OpenUIShopWeapon() {
        if(uiShopWeapon == null) {
            uiShopWeapon = Instantiate(obj_UIShopWeaponPrefab, this.gameObject.transform, false).gameObject.GetComponent<UIShopWeapon>();
        }
        uiShopWeapon.Init();
    }

    public void CloseUIShopWeapon() {
        uiShopWeapon.Close();
    }

    public void OpenUIShopSkin() {
        if (uiShopSkin == null) {
            uiShopSkin = Instantiate(obj_UIShopSkinPrefab, this.gameObject.transform, false).gameObject.GetComponent<UIShopSkin>();
        }
        uiShopSkin.Init();
    }

    public void CloseUIShopSkin() {
        uiShopSkin.Close();
    }

    public void OpenUITut() {
        if (uiTut == null) {
            uiTut = Instantiate(obj_UITutPrefab, this.gameObject.transform, false).gameObject.GetComponent<UITut>();
            uiTut.transform.SetSiblingIndex(InvisibleJoystick.instance.transform.GetSiblingIndex());
        }
        uiTut.InitNormal();
    }

    public void OpenUIInPlay() {
        if (uiInPlay == null) {
            uiInPlay = Instantiate(obj_UIInPlayPrefab, this.gameObject.transform, false).gameObject.GetComponent<UIInPlay>();
        }
        uiInPlay.Init();
    }

    public void CloseUIInPlay() {
        uiInPlay.Close();
    }

    public void OpenUIEnd(bool revive = false) {
        if (uiGameEnd == null) {
            uiGameEnd = Instantiate(obj_UIGameEndPrefab, this.gameObject.transform, false).gameObject.GetComponent<UIGameEnd>();
        }
        if (revive) {
            uiGameEnd.InitPopupRevive();
        }
        else {
            uiGameEnd.InitPopupDetail();
        }
    }

    public void CloseUIEnd() {
        uiGameEnd.Close();
    }
}
