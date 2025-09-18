using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Atribute/MoveFast")]
public class MoveFast : AbilityBase {
    public float flt_speedBonus;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        // không dùng → để trống
    }

    public override void ApplyAttackEffectAbility(Weapon weapon) {
        // không dùng → để trống
    }

    public override void ApplyAtributeAbility(PlayerController playerController) {
        playerController.flt_speed += playerController.flt_speedDefault * flt_speedBonus;
    }

    public override void ApplyEconomyAbility(Weapon weapon) {
        // không dùng → để trống
    }
}
