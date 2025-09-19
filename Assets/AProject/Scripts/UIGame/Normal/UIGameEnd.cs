using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameEnd : MonoBehaviour
{
    [Header("------------Ref------------")]
    public ReviveNowScript reviveNowScript;
    public GameObject obj_popupReviveNow;
    public Button btn_exitRevive;
    public Animator anim_circleSpin;
    public Button btn_acceptRevive;
    public GameObject obj_popupDetailEndGame;
    public Image img_cuurentMap;
    public Image img_nextMap;
    public Image img_lockNextMap;
    public Sprite sprite_lockMap;
    public Sprite sprite_unlockMap;
    public RectTransform rectTrans_processPlayMap;
    public GameObject obj_popupGameOver;
    public GameObject obj_popupWin;
    public TextMeshProUGUI txt_killerName;
    public TextMeshProUGUI txt_rankPlayer;
    public TextMeshProUGUI txt_coinRevice;
    public Button btn_goHomeAfterEnd;

    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;

    public void InitPopupRevive() {
        UIControllerNormal.instance.CloseUIInPlay();
        SetupBtnRevivePopup();
        obj_popupReviveNow.SetActive(true);
        StartCoroutine(WaitForRevivePopupEnd());
    }

    public void InitPopupDetail() {
        UIControllerNormal.instance.CloseUIInPlay();
        SetupBtnDetailPopup();
        obj_popupDetailEndGame.SetActive(true);
        StartCoroutine(WaitForSetupPopupDetail());
    }

    public void Close() {

    }

    public void SetupBtnRevivePopup() {
        btn_exitRevive.onClick.RemoveAllListeners();
        btn_acceptRevive.onClick.RemoveAllListeners();

        btn_exitRevive.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            Player.instance.playerController.revivePlayer.isUnuseRevivePopup = true;
            obj_popupReviveNow.SetActive(false);
            InitPopupDetail();
        });
        btn_acceptRevive.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            UIControllerNormal.instance.OpenUIInPlay();
            Player.instance.playerController.revivePlayer.isRevivePopup = false;
            Player.instance.playerController.revivePlayer.isUseRevivePopup = true;
            obj_popupReviveNow.SetActive(false);
        });
    }

    public IEnumerator ShowBtnGoHome() {
        yield return new WaitForSeconds(1.5f);
        btn_goHomeAfterEnd.gameObject.SetActive(true);
    }

    public void SetupBtnDetailPopup() {
        btn_goHomeAfterEnd.onClick.RemoveAllListeners();
        btn_goHomeAfterEnd.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            ReceiceCoin();
            GameManager.instance.ReLoadScene();
        });
    }

    public IEnumerator WaitForSetupPopupDetail() {
        while(!Player.instance.IsPlayerStateDie() && !Player.instance.IsPlayerStateWin()) {
            yield return null;
        }
        if (Player.instance.IsPlayerStateDie()) {
            obj_popupGameOver.SetActive(true);
            txt_killerName.text = Player.instance.playerController.str_killerName;
            txt_rankPlayer.text = "#" + Player.instance.playerController.rank;
        }
        if (Player.instance.IsPlayerStateWin()) {
            obj_popupWin.SetActive(true);
        }
        img_cuurentMap.GetComponent<Image>().sprite = SpawnManager.Instance.allLevels.getLevelNormalByID(DataManager.Ins.gameSave.levelNormal).imgLevel;
        if (SpawnManager.Instance.allLevels.getLevelNormalByID(DataManager.Ins.gameSave.levelNormal + 1) == null) {
            img_nextMap.GetComponent<Image>().sprite = SpawnManager.Instance.allLevels.getLevelNormalByID(DataManager.Ins.gameSave.levelNormal).imgLevel;
        }
        else {
            img_nextMap.GetComponent<Image>().sprite = SpawnManager.Instance.allLevels.getLevelNormalByID(DataManager.Ins.gameSave.levelNormal + 1).imgLevel;
        }
        //Set thanh process màn chơi (width 260-> 550)
        Vector2 size = rectTrans_processPlayMap.sizeDelta;
        if (Player.instance.IsPlayerStateWin()) {
            size.x = 550f;
            img_lockNextMap.sprite = sprite_unlockMap;
        }
        else {
            size.x += (550f - 260f) / SpawnManager.Instance.int_totalEnemiesToSpawn * (SpawnManager.Instance.int_totalEnemiesToSpawn - (SpawnManager.Instance.getAmountEnemyRemaining() + 1));
            img_lockNextMap.sprite = sprite_lockMap;
        }
        rectTrans_processPlayMap.sizeDelta = size;
        txt_coinRevice.text = Player.instance.playerController.systemGameplayPlayer.getCoinReceive().ToString();
        StartCoroutine(ShowBtnGoHome());
    }

    public IEnumerator WaitForRevivePopupEnd() {
        while (!reviveNowScript.TimeShowEnd()) { yield return null; }
        Player.instance.playerController.revivePlayer.isUnuseRevivePopup = true;
        obj_popupReviveNow.SetActive(false);
        InitPopupDetail();
    }

    public void ReceiceCoin(bool x3 = false) {
        if (!x3) DataManager.Ins.UpdateCoin(Player.instance.playerController.systemGameplayPlayer.getCoinReceive());
        else DataManager.Ins.UpdateCoin(Player.instance.playerController.systemGameplayPlayer.getCoinReceive() * 3);
    }

    public void OnApplicationQuit() {
        if (GameManager.instance.IsGameStateEnd()) ReceiceCoin();
    }
}
