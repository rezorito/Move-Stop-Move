using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGameEndZombie : MonoBehaviour
{
    [Header("------------Ref------------")]
    public ReviveNowScript reviveNowScript;
    public GameObject obj_popupReviveNow;
    public Button btn_exitRevive;
    public Animator anim_circleSpin;
    public Button btn_acceptRevive;
    public GameObject obj_popupDetailEndGame;
    public Button btn_goHome;
    public TextMeshProUGUI txt_titleDetail;
    public GameObject obj_3Star;
    public Image[] arrImg_process;
    public TextMeshProUGUI[] arrTxt_survivalDay;
    public Sprite sprite_processWinCorner;
    public Sprite sprite_processWinUncorner;
    public Sprite sprite_processOverCorner;
    public Sprite sprite_processOverUncorner;
    public GameObject obj_starNani;
    public Button btn_claim;
    public Button btn_claimX3;
    public TextMeshProUGUI txt_coinReceive;
    public TextMeshProUGUI txt_coinReceivex3;
    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;

    public void InitPopupRevive() {
        obj_popupReviveNow.SetActive(true);
        UIControllerZombie.instance.CloseUIInPlayZombie();
        SetupBtnRevivePopup();
        StartCoroutine(WaitForRevivePopupEnd());
    }

    public void InitPopupDetail() {
        obj_popupDetailEndGame.SetActive(true);
        UIControllerZombie.instance.CloseUIInPlayZombie();
        SetupBtnDetailPopup();
        StartCoroutine(WaitForSetupPopupDetail());
    }

    public void Close() {

    }

    public void SetupBtnRevivePopup() {
        btn_exitRevive.onClick.RemoveAllListeners();
        btn_acceptRevive.onClick.RemoveAllListeners();

        btn_exitRevive.onClick.AddListener(() => {
            Player.instance.playerController.revivePlayer.isUnuseRevivePopup = true;
            obj_popupReviveNow.SetActive(false);
            InitPopupDetail();
        });
        btn_acceptRevive.onClick.AddListener(() => {
            UIControllerZombie.instance.OpenUIInPlayZombie();
            Player.instance.playerController.revivePlayer.isRevivePopup = false;
            Player.instance.playerController.revivePlayer.isUseRevivePopup = true;
            obj_popupReviveNow.SetActive(false);
        });
    }

    public void SetupBtnDetailPopup() {
        btn_goHome.onClick.RemoveAllListeners();
        btn_claim.onClick.RemoveAllListeners();
        btn_claimX3.onClick.RemoveAllListeners();
        btn_goHome.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ReLoadScene();
        });
        btn_claim.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            ReceiceCoin();
            GameManager.instance.ReLoadScene();
        });
        btn_claimX3.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            ReceiceCoin(true);
            GameManager.instance.ReLoadScene();
        });
    }

    public IEnumerator WaitForSetupPopupDetail() {
        while (!Player.instance.IsPlayerStateDie() && !Player.instance.IsPlayerStateWin()) {
            yield return null;
        }
        obj_popupDetailEndGame.SetActive(true);
        SetupDayProcess();
        txt_coinReceive.text = Player.instance.playerController.systemGameplayPlayer.getCoinReceive().ToString();
        txt_coinReceivex3.text = (Player.instance.playerController.systemGameplayPlayer.getCoinReceive() * 3).ToString();
        if (Player.instance.IsPlayerStateWin()) {
            txt_titleDetail.text = "you survived day " + DataManager.Ins.gameSave.levelZombie;
            obj_3Star.SetActive(true);
            obj_starNani.SetActive(true);
            SetupProcessZombie();
        }
        else if (Player.instance.IsPlayerStateDie()) {
            txt_titleDetail.text = "city lost, try again";
            SetupProcessZombie();
        }
        StartCoroutine(ShowBtnGoHome());
    }

    public void SetupDayProcess() {
        int dayScope = 0;
        int du = 0;
        if (Player.instance.IsPlayerStateDie()) {
            dayScope = Mathf.CeilToInt(DataManager.Ins.gameSave.levelZombie / 5);
            du = DataManager.Ins.gameSave.levelZombie % 5;
        }
        else if (Player.instance.IsPlayerStateWin()) {
            dayScope = Mathf.CeilToInt(DataManager.Ins.gameSave.levelZombie - 1) / 5;
            du = (DataManager.Ins.gameSave.levelZombie - 1) % 5;
        }
        if (dayScope >= 1 && du == 0) dayScope -= 1;
        for (int i = 0; i < arrTxt_survivalDay.Length; i++) {
            arrTxt_survivalDay[i].text = "Day " + (dayScope * 5 + (i + 1)).ToString();
        }
    }

    public void SetupProcessZombie() {
        int levelProcess = 0;
        if (Player.instance.IsPlayerStateDie()) {
            levelProcess = DataManager.Ins.gameSave.levelZombie % 5;
        }
        else if (Player.instance.IsPlayerStateWin()) {
            levelProcess = (DataManager.Ins.gameSave.levelZombie - 1) % 5;
        }
        if (levelProcess == 0) levelProcess = 5;

        for (int i = 0; i < levelProcess && i < arrImg_process.Length; i++) {
            Color color = arrImg_process[i].color;
            color.a = 1f;
            arrImg_process[i].color = color;

            if (Player.instance.IsPlayerStateDie()) {
                if (i != levelProcess - 1) {
                    if (i == 0 || i == 4)
                        arrImg_process[i].sprite = sprite_processWinCorner;
                    else
                        arrImg_process[i].sprite = sprite_processWinUncorner;
                }
                else {
                    if (i == 0 || i == 4)
                        arrImg_process[i].sprite = sprite_processOverCorner;
                    else
                        arrImg_process[i].sprite = sprite_processOverUncorner;

                    arrImg_process[i].transform.Find("Over").gameObject.SetActive(true);
                }
            }
            else if (Player.instance.IsPlayerStateWin()) {
                if (i == 0 || i == 4)
                    arrImg_process[i].sprite = sprite_processWinCorner;
                else
                    arrImg_process[i].sprite = sprite_processWinUncorner;
            }
        }
    }

    public IEnumerator ShowBtnGoHome() {
        yield return new WaitForSeconds(1.5f);
        btn_goHome.gameObject.SetActive(true);
    }

    public IEnumerator WaitForRevivePopupEnd() {
        while (!reviveNowScript.TimeShowEnd()) { yield return null; }
        Player.instance.playerController.revivePlayer.isUnuseRevivePopup = true;
        obj_popupReviveNow.SetActive(false);
        InitPopupDetail();
    }

    public void ReceiceCoin(bool x3 = false) {
        if(!x3) DataManager.Ins.UpdateCoin(Player.instance.playerController.systemGameplayPlayer.getCoinReceive());
        else DataManager.Ins.UpdateCoin(Player.instance.playerController.systemGameplayPlayer.getCoinReceive() * 3);
    }

    public void OnApplicationQuit() {
        if(GameManager.instance.IsGameStateEnd()) ReceiceCoin();
    }
}
