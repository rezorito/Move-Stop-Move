using System.Collections;
using UnityEngine;

public class CameraPos : MonoBehaviour {
    [Header("------------Ref------------")]
    public GameObject obj_player;
    public PlayerController playerController;
    public Camera cameraPos;

    [Header("------------Variable------------")]
    public float flt_smoothZoom = 5f; // tốc độ zoom mượt

    private Vector3 distance;
    private bool isSpecialAttack = false;
    private Vector3 currentOffset; // offset hiện tại (mượt)

    private void Awake() {
        distance = cameraPos.transform.position - obj_player.transform.position;
        currentOffset = distance; // ban đầu offset = distance mặc định
    }

    public void setOnSpecialAttack() {
        isSpecialAttack = true;
    }
    public IEnumerator setOffSpecialAttack() {
        isSpecialAttack = false;
        yield return new WaitForSeconds(0.5f);
    }

    public void setupCameraZombie() {
        Vector3 targetOffset = distance + new Vector3(0f,
        DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f,
        -DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f
        );

        // đặt luôn offset hiện tại và targetOffset trùng nhau
        currentOffset = targetOffset;

        // di chuyển camera ngay lập tức
        transform.position = obj_player.transform.position + currentOffset;
    }

    private void LateUpdate() {
        if (GameManager.instance.IsGameStateHome()) return;
        Vector3 targetOffset = distance;
        if (Time.timeScale == 0) {
            currentOffset = targetOffset; // bỏ smooth
            transform.position = obj_player.transform.position + currentOffset;
            return;
        }

        // offset gốc dựa theo mode
        if (GameManager.instance.currentMode == GameMode.Normal) {
            if (playerController.isSpecialAttack) {
                if (!isSpecialAttack) {
                    isSpecialAttack = true;
                }
                targetOffset += new Vector3(0f,
                    playerController.systemGameplayPlayer.int_levelSelf * 0.02f + 0.15f,
                    -playerController.systemGameplayPlayer.int_levelSelf * 0.02f - 0.15f
                );
            }
            else {
                if (isSpecialAttack) {
                    StartCoroutine(setOffSpecialAttack()); // chạy coroutine
                }
                targetOffset += new Vector3(0f,
                    playerController.systemGameplayPlayer.int_levelSelf * 0.02f,
                    -playerController.systemGameplayPlayer.int_levelSelf * 0.02f
                );
            }
        }
        else if (GameManager.instance.currentMode == GameMode.Zombie) {
            if (playerController.abilitySelected != null && playerController.abilitySelected.abilityID == "StartBiggerID") {
                float valueScaleBigger = playerController.flt_valueScaleBigger / 0.002f * 0.03f;
                targetOffset += new Vector3(0f,
                    DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f + valueScaleBigger,
                    -DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f - valueScaleBigger
                );
            }
            else {
                targetOffset += new Vector3(0f,
                DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f,
                -DataManager.Ins.gameSave.flt_rangeBonusZombie / 0.1f * 0.03f
                );
            }
        }

        // làm mượt chỉ offset (zoom in/out mượt)
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.unscaledDeltaTime * flt_smoothZoom);

        // camera luôn follow player trực tiếp
        transform.position = obj_player.transform.position + currentOffset;
        transform.LookAt(obj_player.transform.position);
    }
}
