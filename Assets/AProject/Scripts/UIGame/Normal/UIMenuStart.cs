using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UIMenuStart : MonoBehaviour
{
    //public UIControllerNormal uiControllerNormal;
    [Header("------------Ref------------")]
    public GameObject obj_playerMain;
    public GameObject obj_cameraMainPlay;
    public GameObject obj_cameraShopSkin;
    public Animator anim_InputName;
    public TMP_InputField if_inputName;
    public Animator anim_GroupUILeft;
    public Button btn_showShopWeapon;
    public Button btn_showShopSkin;
    public Animator anim_GroupUIRight;
    public TextMeshProUGUI txt_coinOwn;
    public Button btn_ads;
    public Button btn_shakeAction;
    public Button btn_soundAction;
    public Button btn_changeZombieMode;
    public TextMeshProUGUI txt_lvZombieMode;
    public Button btn_play;
    public TextMeshProUGUI txt_bestScoreLevel;

    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if(!isInit) {
            isInit = true;
            SetupButton(); 
        }
        LoadCoinOwn();
        txt_lvZombieMode.text = DataManager.Ins.gameSave.levelZombie.ToString();
        if(DataManager.Ins.gameSave.highestScoreNormal == 0) {
            txt_bestScoreLevel.text = "Zone : " + DataManager.Ins.gameSave.levelNormal + " - Best : ";
        } else {
            txt_bestScoreLevel.text = "Zone : " + DataManager.Ins.gameSave.levelNormal + " - Best : " + DataManager.Ins.gameSave.highestScoreNormal;
        }
        if_inputName.text = DataManager.Ins.gameSave.namePlayer;
        SetupInputField();
    }

    public void SetupButton() {
        btn_showShopWeapon.onClick.RemoveAllListeners();
        btn_showShopSkin.onClick.RemoveAllListeners();
        btn_ads.onClick.RemoveAllListeners();
        btn_shakeAction.onClick.RemoveAllListeners();
        btn_soundAction.onClick.RemoveAllListeners();
        btn_changeZombieMode.onClick.RemoveAllListeners();
        btn_play.onClick.RemoveAllListeners();

        btn_showShopWeapon.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            SetupAnimOpenShop();
            UIControllerNormal.instance.OpenUIShopWeapon();
        });
        btn_showShopSkin.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            SetupAnimOpenShop();
            UIControllerNormal.instance.OpenUIShopSkin();
        });
        btn_ads.onClick.AddListener(() => {
        });
        btn_shakeAction.onClick.AddListener(() => {
        });
        btn_soundAction.onClick.AddListener(() => {
        });
        btn_changeZombieMode.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.LoadScene("LoadingZombieMode");
        });
        btn_play.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ChangeStateStartGame();
            UIControllerNormal.instance.OpenUIInPlay();
            Close();
        });
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void SetupAnimOpenShop() {
        anim_InputName.SetTrigger("MoveUp");
        anim_GroupUILeft.SetTrigger("MoveLeft");
        anim_GroupUIRight.SetTrigger("MoveRight2");
    }

    public void SetupAnimExitShop() {
        anim_InputName.SetTrigger("MoveAgain");
        anim_GroupUILeft.SetTrigger("MoveAgain");
        anim_GroupUIRight.SetTrigger("MoveAgain");
    }

    public void LoadCoinOwn() {
        txt_coinOwn.text = DataManager.Ins.gameSave.coin.ToString();
    }

    public void SetupInputField() {
        if_inputName.onEndEdit.AddListener((string text) => {
            Player.instance.playerController.skinPlayer.billboard.InitName(text);
            DataManager.Ins.SaveName(text);
        });
    }
}
