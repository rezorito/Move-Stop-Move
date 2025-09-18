using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    private Transform target;
    private Camera cam;

    [Header("UI Elements")]
    public Image arrowImage;
    public Image iconImage;
    public RectTransform arrowImageTrans;
    public RectTransform iconImageTrans;
    public GameObject obj_scoreShow;
    public TextMeshProUGUI scoreText;

    [Header("Offsets")]
    public float edgeOffset = 50f;     
    public float gapArrowIcon = 40f; 

    public void Init(Transform target, Camera cam)
    {
        this.target = target;
        this.cam = cam;
        iconImageTrans.gameObject.SetActive(false);
        
        // Reset vị trí về ngoài màn hình để tránh nháy
        iconImageTrans.position = new Vector3(-1000, -1000, 0);
        arrowImageTrans.position = new Vector3(-1000, -1000, 0);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        bool isOnScreen = screenPos.z > 0 &&
                          screenPos.x > 0 && screenPos.x < Screen.width &&
                          screenPos.y > 0 && screenPos.y < Screen.height;

        if (isOnScreen)
        {
            // Icon nằm ngay trên object
            iconImage.enabled = false;
            arrowImage.enabled = false;
            obj_scoreShow.SetActive(false);
        }
        else
        {
            iconImage.enabled = true;
            arrowImage.enabled = true;
            obj_scoreShow.SetActive(true);

            if (screenPos.z < 0) screenPos *= -1;

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Vector3 dir = (screenPos - screenCenter).normalized;

            float slope = dir.y / dir.x;
            Vector3 edgePos = screenCenter;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                edgePos.x = dir.x > 0 ? Screen.width - edgeOffset : edgeOffset;
                edgePos.y = screenCenter.y + slope * (edgePos.x - screenCenter.x);
            }
            else
            {
                edgePos.y = dir.y > 0 ? Screen.height - edgeOffset : edgeOffset;
                edgePos.x = screenCenter.x + (edgePos.y - screenCenter.y) / slope;
            }

            iconImageTrans.position = edgePos;
            iconImageTrans.rotation = Quaternion.identity;

            Vector3 arrowPos = iconImageTrans.position + (dir * gapArrowIcon);
            arrowImageTrans.position = arrowPos;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowImageTrans.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

    }
}
