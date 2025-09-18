using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Abilities/Attack/ContinousAttack")]
public class ContinousAttack : AbilityBase {
    int int_bullet;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        int_bullet = playerController.GetBullet();
        float angel = 0;
        for (int i = 0; i < int_bullet; i++) {
            Weapon weapon = playerController.skinPlayer.GetBullet();
            if (!weapon.isInit) weapon.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
            weapon.FlyTo(playerController.trans_pointWeaponAttack, target, angel);
            angel += Mathf.Pow(-1, i + 1) * 40f * (i + 1);
        }
        playerController.StartCoroutine(ThrowContinous(0.15f, playerController, target));
    }

    IEnumerator ThrowContinous(float time, PlayerController playerController, Transform target) {
        yield return new WaitForSeconds(time);
        float angel = 0;
        for (int i = 0; i < int_bullet; i++) {
            Weapon weapon = playerController.skinPlayer.GetBullet();
            if (!weapon.isInit) weapon.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
            weapon.FlyTo(playerController.trans_pointWeaponAttack, target, angel);
            angel += Mathf.Pow(-1, i + 1) * 40f * (i + 1);
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
