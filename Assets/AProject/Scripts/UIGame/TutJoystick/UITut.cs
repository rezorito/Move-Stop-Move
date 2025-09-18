using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITut : MonoBehaviour
{
    [Header("----------------Common----------------")]
    [Header("------------Ref------------")]
    public GameObject obj_tutJoystick;
    public RectTransform rect_handImage;         // Bàn tay
    public RectTransform[] rect_listPathPoints;      // Các waypoint trên Canvas
    public SoundController soundCountDownController;
    [Header("------------Variable------------")]
    public float flt_duration = 3f;             // Thời gian chạy hết đường
    public bool isLoop = true;
    public bool isShowTutJoyStick = false;
    public bool isInit = false;

    [Header("----------------Zombie----------------")]
    [Header("------------Ref------------")]
    public GameObject obj_waitToPlay;
    public TextMeshProUGUI txt_countDown;
    [Header("------------Variable------------")]
    public int int_timeWait = 3;

    public void InitNormal() {
        if (isInit) return;
        else isInit = true;
        StartPathLoop();
        StartCoroutine(WaitToUseJoyStick());
    }

    public void InitZombie() {
        if (isInit) return;
        else isInit = true;
        StartCoroutine(WaitToPlayZombie());
    }

    IEnumerator WaitToPlayZombie() {
        obj_waitToPlay.SetActive(true);
        int time = int_timeWait;
        while(time != 0) {
            soundCountDownController.PlaySound(SoundData.SoundName.TimeCountDown);
            txt_countDown.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        obj_waitToPlay.SetActive(false);
        GameManager.instance.ChangeStateStartGame();
        StartPathLoop();
    }

    void StartPathLoop() {
        obj_tutJoystick.SetActive(true);
        isShowTutJoyStick = true;
        // Lấy danh sách điểm
        Vector3[] positions = new Vector3[rect_listPathPoints.Length];
        for (int i = 0; i < rect_listPathPoints.Length; i++) {
            positions[i] = rect_listPathPoints[i].anchoredPosition;
        }

        // Cho bàn tay chạy theo đường
        var tween = rect_handImage.DOLocalPath(positions, flt_duration, PathType.CatmullRom)
                             .SetEase(Ease.Linear);

        if (isLoop) {
            tween.SetLoops(-1, LoopType.Restart); // chạy lặp lại
        }
    }

    IEnumerator WaitToUseJoyStick() {
        if(UIControllerNormal.instance != null && UIControllerZombie.instance != null) {
            while(!InvisibleJoystick.instance.isUsedJoystickFirst) { yield return null; }
            gameObject.SetActive(false);
        }
    }

    public void Update() {
        if ((InvisibleJoystick.instance.isUsedJoystickFirst && isShowTutJoyStick) || Player.instance.IsPlayerStateRevive() || Player.instance.IsPlayerStateDie()) {
            gameObject.SetActive(false);
        }
    }
}
