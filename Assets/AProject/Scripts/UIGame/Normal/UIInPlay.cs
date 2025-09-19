using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInPlay : MonoBehaviour
{
    [Header("------------Ref------------")]
    public Button btn_pauseGame;
    public GameObject obj_amountEnemyRemain;
    public TextMeshProUGUI txt_amountEnemyRemain;
    public GameObject obj_popupPause;
    public Button btn_soundAction;
    public Image img_btnSound;
    public Sprite sprite_soundOn;
    public Sprite sprite_soundOff;
    public GameObject obj_soundOn;
    public GameObject obj_soundOff;
    public Button btn_shakeAction;
    public Image img_btnShake;
    public Sprite sprite_shakeOn;
    public Sprite sprite_shakeOff;
    public GameObject obj_shakeOn;
    public GameObject obj_ShakeOff;
    public Button btn_goHome;
    public Button btn_continue;

    [Space]
    [Header("------------Variable------------")]
    public bool isInit = false;

    public void Init() {
        gameObject.SetActive(true);
        if(!isInit) {
            isInit = true;
            txt_amountEnemyRemain.text = "Alive : " + SpawnManager.Instance.getAmountEnemyRemaining();
            SetupButton();
            SetupUIBtnSound();
            SetupUIBtnShake();
        }
    }

    public void SetupButton() {
        btn_pauseGame.onClick.RemoveAllListeners();
        btn_soundAction.onClick.RemoveAllListeners();
        btn_shakeAction.onClick.RemoveAllListeners();
        btn_goHome.onClick.RemoveAllListeners();
        btn_continue.onClick.RemoveAllListeners();

        btn_pauseGame.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ChangeStatePauseGame();
            obj_amountEnemyRemain.SetActive(false);
            btn_pauseGame.gameObject.SetActive(false);
            obj_popupPause.SetActive(true);
        });
        btn_soundAction.onClick.AddListener(() => {
            AudioManager.Ins.UpdateVolumnSoundAMusic();
            SetupUIBtnSound();
            AudioManager.Ins.PlaySound_ButtonClick();
        });
        btn_shakeAction.onClick.AddListener(() => {
            AudioManager.Ins.UpdateVibration();
            SetupUIBtnShake();
            AudioManager.Ins.PlaySound_ButtonClick();
        });
        btn_goHome.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ReLoadScene();
        });
        btn_continue.onClick.AddListener(() => {
            AudioManager.Ins.PlaySound_ButtonClick();
            GameManager.instance.ChangeStateResumeGame();
            obj_amountEnemyRemain.SetActive(true);
            btn_pauseGame.gameObject.SetActive(true);
            obj_popupPause.SetActive(false);
        });
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    void Update()
    {
        txt_amountEnemyRemain.text = "Alive : " + SpawnManager.Instance.getAmountEnemyRemaining();
    }

    public void SetupUIBtnSound() {
        if (AudioManager.Ins.IsStatusSoundAMusic()) {
            img_btnSound.sprite = sprite_soundOn;
            obj_soundOn.SetActive(true);
            obj_soundOff.SetActive(false);
        }
        else {
            img_btnSound.sprite = sprite_soundOff;
            obj_soundOn.SetActive(false);
            obj_soundOff.SetActive(true);
        }
    }
    public void SetupUIBtnShake() {
        if (AudioManager.Ins.IsStatusVibration()) {
            img_btnShake.sprite = sprite_shakeOn;
            obj_shakeOn.SetActive(true);
            obj_ShakeOff.SetActive(false);
        }
        else {
            img_btnShake.sprite = sprite_shakeOff;
            obj_shakeOn.SetActive(false);
            obj_ShakeOff.SetActive(true);
        }
    }
}
