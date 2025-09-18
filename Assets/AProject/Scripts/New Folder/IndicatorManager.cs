using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Instance;
    public RectTransform tectTrans_hudCanvas;
    public GameObject obj_indicatorPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject CreateIndicator(Transform enemy)
    {
        GameObject ind = Instantiate(obj_indicatorPrefab, tectTrans_hudCanvas);
        //ind.GetComponent<EnemyIndicator>().Init(enemy, Camera.main);
        return ind;
    }
}
