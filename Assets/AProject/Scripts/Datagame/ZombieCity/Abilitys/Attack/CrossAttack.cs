using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack/CrossAttack")]
public class CrossAttack : AbilityBase {
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        int int_bullet = playerController.GetBullet();
        float angel = 0;
        for (int i = 0; i < int_bullet; i++) {
            Weapon weapon = playerController.skinPlayer.GetBullet();
            if (!weapon.isInit) weapon.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
            weapon.FlyTo(playerController.trans_pointWeaponAttack, target, angel);
            angel += Mathf.Pow(-1, i + 1) * 40f * (i + 1);
        }
        Weapon weapon1 = playerController.skinPlayer.GetBullet();
        if (!weapon1.isInit) weapon1.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
        weapon1.FlyTo(playerController.trans_spawnWeaponLeft, target, -90f);
        Weapon weapon2 = playerController.skinPlayer.GetBullet();
        if (!weapon2.isInit) weapon2.Init(playerController.gameObject, playerController.rangeDetect, playerController.systemGameplayPlayer);
        weapon2.FlyTo(playerController.trans_spawnWeaponRight, target, 90f);
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