using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTut : MonoBehaviour
{
    public TutController tutController;
    public SoundController soundEnemyController;
    public GameObject bloodEffectPrefab;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Weapon")) {
            soundEnemyController.PlaySound(SoundData.SoundName.WeaponHit);
            tutController.EnemyTutDie();
            gameObject.SetActive(false);
            Instantiate(bloodEffectPrefab, other.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
