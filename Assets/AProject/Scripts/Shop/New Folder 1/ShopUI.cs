using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;
    public WeaponShopViewer viewer;
    public GameObject swordPrefab;
    public GameObject axePrefab;
    public Material material;

    void Start()
    {
        //viewer.ShowWeapon(swordPrefab, false);
    }

    //public void OnSelectSword() => viewer.ShowWeapon(swordPrefab, false);
    //public void OnSelectAxe() => viewer.ShowWeapon(axePrefab, false);

    public void OnHandleRed()
    {
        viewer.SetPartColor("Handle", Color.red);
    }

    public void OnBladeBlue()
    {
        viewer.SetPartColor("Blade", Color.blue);
    }
}
