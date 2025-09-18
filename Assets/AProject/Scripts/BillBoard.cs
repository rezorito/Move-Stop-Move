using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    [Header("------------Ref------------")]
    private Camera cam;
    public GameObject obj_name;
    public TextMeshProUGUI txt_name;
    public GameObject obj_score;
    public Image img_score;
    public TextMeshProUGUI txt_score;
    public SystemGameplay systemGameplaySelf;
    public Animator anim_upScore;
    public TextMeshProUGUI txt_upScore;

    [Header("------------Variable------------")]
    public float flt_sizeOnScreen = 1f;
    public bool isShowBillboard = false;
    private int scorePlayer = 0;
    public bool isPlayer = false;
    public void InitColor(Color color) {
        img_score.color = color;
        txt_name.color = color;
    }

    public void InitName(string nameSelf) {
        txt_name.text = nameSelf;
    }

    void Start()
    {
        cam = Camera.main;
        //transform.localPosition = offset; // đặt đúng vị trí 1 lần
    }

    private void Update() {
        if (GameManager.instance.IsGameStatePlay() && !isShowBillboard && obj_name != null && obj_score != null) SetupCanvas();
        if(isPlayer && scorePlayer != systemGameplaySelf.int_scoreSelf) {
            int int_upScore = systemGameplaySelf.int_scoreSelf - scorePlayer;
            scorePlayer = systemGameplaySelf.int_scoreSelf;
            txt_upScore.text = "+ " + int_upScore;
            anim_upScore.SetTrigger("Up");
        }
        txt_score.text = systemGameplaySelf.int_scoreSelf.ToString();
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // Luôn hướng camera
        transform.LookAt(
            transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up
        );

        float distance = Vector3.Distance(cam.transform.position, transform.position);
        float scaleFactor = distance * 0.01f * flt_sizeOnScreen;
        transform.localScale = Vector3.one * scaleFactor;
    }

    void SetupCanvas() {
        isShowBillboard = true;
        obj_name.SetActive(true);
        obj_score.SetActive(true);
    }
}
