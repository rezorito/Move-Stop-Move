using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBossCollision : MonoBehaviour
{
    private Vector3 minScale = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 scaleDefault;
    public EnemyFollow enemy;

    private void Awake()
    {
        scaleDefault = transform.localScale;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Weapon"))
        {
            float healthPercent = (float)enemy.int_healthCurrent / enemy.int_maxHealth;
            transform.localScale = Vector3.Lerp(minScale, scaleDefault, healthPercent);
            Debug.Log(transform.localScale);
        }
    }
}
