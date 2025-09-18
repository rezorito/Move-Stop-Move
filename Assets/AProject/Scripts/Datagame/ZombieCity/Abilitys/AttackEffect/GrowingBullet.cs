using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AttackEffect/GrowingBullet")]
public class GrowingBullet : AbilityBase {
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        // không dùng → để trống
    }

    public override void ApplyAttackEffectAbility(Weapon weapon) {
        weapon.isSpecialAttack = true;
        weapon.isPiercing = true;
    }

    public override void ApplyAtributeAbility(PlayerController playerController) {
        // không dùng → để trống
    }

    public override void ApplyEconomyAbility(Weapon weapon) {
        // không dùng → để trống
    }
}
