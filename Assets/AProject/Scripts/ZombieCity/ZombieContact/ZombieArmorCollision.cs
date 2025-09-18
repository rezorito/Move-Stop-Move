using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieArmorCollision : MonoBehaviour
{
    public GameObject hairObject;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Weapon"))
        {
            hairObject.SetActive(false);
        }
    }
}
