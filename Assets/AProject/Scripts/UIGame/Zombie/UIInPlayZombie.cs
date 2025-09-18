using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInPlayZombie : MonoBehaviour
{
    [Header("------------Ref------------")]
    public TextMeshProUGUI txt_countDaySurvival;
    public TextMeshProUGUI txt_amountEnemyRemain;
    public Button btn_pauseGame;
    public GameObject obj_popupPause;
    public GameObject obj_emptyAbility;
    public Image img_abilityUsed;
    public Button btn_goHome;
    public Button btn_continue;

    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if (!isInit) {
            isInit = true;
            SetupButton();
        }
        txt_countDaySurvival.text = "Day " + DataManager.Ins.gameSave.levelZombie;
        txt_amountEnemyRemain.text = SpawnManager.Instance.getAmountEnemyRemaining().ToString();
    }

    public void SetupButton() {
        btn_pauseGame.onClick.RemoveAllListeners();
        btn_goHome.onClick.RemoveAllListeners();
        btn_continue.onClick.RemoveAllListeners();

        btn_pauseGame.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            obj_emptyAbility.SetActive(true);
            GameManager.instance.ChangeStatePauseGame();
            btn_pauseGame.gameObject.SetActive(false);
            obj_popupPause.SetActive(true);
        });
        btn_goHome.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ReLoadScene();
        });
        btn_continue.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ChangeStateResumeGame();
            btn_pauseGame.gameObject.SetActive(true);
            obj_popupPause.SetActive(false);
        });
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    void Update() {
        txt_amountEnemyRemain.text = SpawnManagerZBCT.instance.getAmountEnemyRemaining().ToString();
    }
}
