using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvisibleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static InvisibleJoystick instance;
    private Vector2 startTouchPos;
    private Vector2 inputVector;
    [Header("------------Ref------------")]
    public RectTransform rectTrans_joystickBG;     // Vòng ngoài
    public RectTransform rectTrans_joystickHandle; // Nút bên trong
    [Header("------------Variable------------")]
    public float flt_maxDistance = 100f;
    public bool isUsedJoystickFirst = false;

    public Vector2 Input => inputVector;
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }
    private void Start() {
        ResetInput();
    }
    private void Update()
    {
        if(GameManager.instance.IsGameStatePlay() && !Player.instance.IsPlayerStateDie() && !Player.instance.IsPlayerStateRevive())
        {
            if (Input.sqrMagnitude < 0.000001f && Input.sqrMagnitude > -0.000001f) {
                rectTrans_joystickBG.gameObject.SetActive(false);
                rectTrans_joystickHandle.gameObject.SetActive(false);
            } else {
                if (!Player.instance.IsPlayerStateAttack()) {
                    if (!isUsedJoystickFirst) isUsedJoystickFirst = true;
                    rectTrans_joystickBG.gameObject.SetActive(true);
                    rectTrans_joystickHandle.gameObject.SetActive(true);
                }
            }
            return;
        }
        inputVector = Vector2.zero;
        rectTrans_joystickBG.gameObject.SetActive(false);
        rectTrans_joystickHandle.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Lấy vị trí chạm
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, null, out startTouchPos);
        // Hiện joystick tại vị trí chạm
        rectTrans_joystickBG.gameObject.SetActive(true);
        rectTrans_joystickHandle.gameObject.SetActive(true);
        rectTrans_joystickBG.position = eventData.position;
        rectTrans_joystickHandle.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentTouchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, null, out currentTouchPos);

        Vector2 delta = currentTouchPos - startTouchPos;
        delta = Vector2.ClampMagnitude(delta, flt_maxDistance);
        inputVector = delta / flt_maxDistance;

        // ✅ Giữ joystick handle trong vòng tròn ngoàiss
        rectTrans_joystickHandle.localPosition = delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;

        // Ẩn joystick khi nhả
        rectTrans_joystickBG.gameObject.SetActive(false);
        rectTrans_joystickHandle.gameObject.SetActive(false);
    }

    public void ResetInput()
    {
        inputVector = Vector2.zero;
        if (rectTrans_joystickBG != null) rectTrans_joystickBG.gameObject.SetActive(false);
        if (rectTrans_joystickHandle != null) rectTrans_joystickHandle.gameObject.SetActive(false);
    }
}
