using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicatorController : MonoBehaviour {

    public Transform target;
    public SystemGameplay targetSystemGameplay;
    public Image imgSTT;
    private GameObject indicator;
    public EnemyAI enemyAI;
    private bool Spawn = false;

    public void Init(Color color) {
        if(indicator == null) {
            indicator = IndicatorManager.Instance.CreateIndicator(gameObject.transform);
            var indicatorScript = indicator.GetComponent<EnemyIndicator>();
            indicatorScript.Init(gameObject.transform, Camera.main);
        }
        indicator.SetActive(true);
        indicator.GetComponent<EnemyIndicator>().arrowImage.color = color;
        indicator.GetComponent<EnemyIndicator>().iconImage.color = color;
    }

    private void Update() {
        if (indicator == null) return;
        if (enemyAI.isDead) {
            indicator.SetActive(false);
            //DestroyIndicator();
            return;
        }
        indicator.GetComponent<EnemyIndicator>().scoreText.text = targetSystemGameplay.getScoreSelf().ToString();
        if (Spawn) return;
        if (!GameManager.instance.IsGameStatePlay() && !Spawn) {
            indicator.SetActive(false);
        }
        else {
            Spawn = true;
            indicator.SetActive(true);
        }
    }

    private void DestroyIndicator() {
        // Khi enemy bị disable hoặc destroy -> xóa indicator
        if (indicator != null) {
            indicator.SetActive(false);
            Destroy(indicator);
        }
    }
}
