using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerZombie : MonoBehaviour
{
    public static UIControllerZombie instance;

    public UIMenuStartZombie uiMenuStartZombie;
    public GameObject obj_UITutPrefab;
    public GameObject obj_UIInPlayZombiePrefab;
    public GameObject obj_UIGameEndZombiePrefab;
    public UITut uiTut;
    public UIInPlayZombie uiInPlayZombie;
    public UIGameEndZombie uiGameEndZombie;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void OpenUIMenuStartZombie() {
        uiMenuStartZombie.Init();
    }

    public void CloseUIMenuStartZombie() {
        uiMenuStartZombie.Close();
    }

    public void OpenUIInPlayZombie() {
        if (uiInPlayZombie == null) {
            uiInPlayZombie = Instantiate(obj_UIInPlayZombiePrefab, this.gameObject.transform, false).gameObject.GetComponent<UIInPlayZombie>();
        }
        uiInPlayZombie.Init();
    }

    public void CloseUIInPlayZombie() {
        uiInPlayZombie.Close();
    }

    public void OpenUITut() {
        if (uiTut == null) {
            uiTut = Instantiate(obj_UITutPrefab, this.gameObject.transform, false).gameObject.GetComponent<UITut>();
        }
        uiTut.InitZombie();
    }

    public void OpenUIEndZombie(bool revive = false) {
        if (uiGameEndZombie == null) {
            uiGameEndZombie = Instantiate(obj_UIGameEndZombiePrefab, this.gameObject.transform, false).gameObject.GetComponent<UIGameEndZombie>();
        }
        if (revive) {
            uiGameEndZombie.InitPopupRevive();
        }
        else {
            uiGameEndZombie.InitPopupDetail();
        }
    }

    public void CloseUIEndZombie() {
        uiGameEndZombie.Close();
    }
}
