using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkinWeapon : MonoBehaviour
{
    public GameObject lockUI;
    public GameObject chooseUI;
    public Image imgSkin;
    public TextMeshProUGUI isCustomText;

    public void setupUI(SkinWeapon skinWeapon)
    {

    }

    public void setupSpriteSkin(Sprite _imgSkin)
    {
        imgSkin.sprite = _imgSkin;
    }

    public void setupCustomText(bool isCustom)
    {
        if (isCustom)
        {
            isCustomText.gameObject.SetActive(true);
        }
        else
        {
            isCustomText.gameObject.SetActive(false);
        }
    }

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

    public void setupLockUI(bool isLock)
    {
        if (isLock)
        {
            lockUI.SetActive(true);
        }
        else
        {
            lockUI.SetActive(false);
        }
    }
}
