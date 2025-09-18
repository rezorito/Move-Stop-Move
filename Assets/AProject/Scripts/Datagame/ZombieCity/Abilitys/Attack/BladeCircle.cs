using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Abilities/Attack/BladeCircle")]
public class BladeCircle : AbilityBase {
    public Transform playerTransform;
    public float radius = 0.15f;
    public float speed = 0.15f;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        playerTransform = playerController.gameObject.transform;
        playerController.StartCoroutine(RotateSword(playerController));
    }

    IEnumerator RotateSword(PlayerController playerController) {
        int bullet = playerController.GetBullet();

        List<GameObject> list_weapons = new List<GameObject>();

        for (int i = 0; i < bullet; i++) {
            Weapon weapon = SetupWeapon(playerController);
            list_weapons.Add(weapon.gameObject);
            playerController.StartCoroutine(RotateWeapon(playerController, weapon.gameObject, i, bullet, radius, speed));
        }

        while (true) {
            int newBullet = playerController.GetBullet();
            if (newBullet != list_weapons.Count) {
                foreach (var w in list_weapons) w.SetActive(false);
                list_weapons.Clear();

                for (int i = 0; i < newBullet; i++) {
                    Weapon weapon = SetupWeapon(playerController);
                    list_weapons.Add(weapon.gameObject);
                    playerController.StartCoroutine(RotateWeapon(playerController, weapon.gameObject, i, newBullet, radius, speed));
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public Weapon SetupWeapon(PlayerController playerController) {
        Weapon weapon = playerController.skinPlayer.GetBullet();
        if (!weapon.isInit) weapon.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
        weapon.gameObject.SetActive(true);
        weapon.isPiercing = true;
        return weapon;
    }

    IEnumerator RotateWeapon(PlayerController playerController, GameObject weapon, int index, int totalWeapons, float radius, float speed) {
        float angle = (360f / totalWeapons) * index;
        float fixedY = playerController.rangeDetect.transform.position.y + 0.02f;
        //weapon.transform.position.y = fixedY;
        while (weapon != null) {
            angle += speed * Time.deltaTime;

            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            weapon.transform.position = playerTransform.position + offset;

            Vector3 dir = (weapon.transform.position - playerTransform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir) * Quaternion.Euler(-90f, 90f, -90f);
            weapon.transform.rotation = targetRot;

            weapon.transform.localScale = Vector3.one;
            yield return null;
        }
    }

    public override void ApplyAttackEffectAbility(Weapon weapon) {
        // không dùng → để trống
    }

    public override void ApplyAtributeAbility(PlayerController playerController) {
        // không dùng → để trống
    }

    public override void ApplyEconomyAbility(Weapon weapon) {
        // không dùng → để trống
    }
}
