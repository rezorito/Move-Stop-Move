using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailGamaOverScript : MonoBehaviour
{
    public GameObject btnContinue;

    private void OnEnable()
    {
        btnContinue.SetActive(false);
        StartCoroutine(ShowBtnContinue());
    }

    IEnumerator ShowBtnContinue()
    {
        yield return new WaitForSeconds(2f);
        btnContinue.SetActive(true);
    }
}
