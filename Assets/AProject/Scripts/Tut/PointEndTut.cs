using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEndTut : MonoBehaviour
{
    public TutController tutController;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            gameObject.SetActive(false);
            tutController.SetupEndTut();
        }
    }
}
