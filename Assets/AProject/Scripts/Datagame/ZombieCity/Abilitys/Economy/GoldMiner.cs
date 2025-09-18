using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Economy/GoldMiner")]
public class GoldMiner : AbilityBase {
    public int expBonus;
    public int goldBonus;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        // không dùng → để trống
    }

    public override void ApplyAttackEffectAbility(Weapon weapon) {
        // không dùng → để trống
    }

    public override void ApplyAtributeAbility(PlayerController playerController) {
        // không dùng → để trống
    }

    public override void ApplyEconomyAbility(Weapon weapon) {
        weapon.int_bonusExp = 2;
        weapon.int_bonusCoin = 2;
    }
}
