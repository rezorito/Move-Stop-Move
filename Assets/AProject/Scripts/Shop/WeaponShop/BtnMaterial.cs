using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnMaterial : MonoBehaviour
{
    public GameObject chooseUI;

    public void setupChooseUI(bool isChoose)
    {
        if (isChoose)
        {
            chooseUI.SetActive(true);
        }
        else
        {
            chooseUI.SetActive(false);
        }
    }
}
