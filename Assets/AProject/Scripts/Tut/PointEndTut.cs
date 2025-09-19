using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEndTut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            gameObject.SetActive(false);
            DataManager.Ins.gameSave.isDoneTutGameplay = true;
            GameManager.instance.ReLoadScene();
        }
    }
}
