using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Atribute/Revive")]
public class Revive : AbilityBase {
    public int int_amountReviveBonus;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        // không dùng → để trống
    }

    public override void ApplyAttackEffectAbility(Weapon weapon) {
        // không dùng → để trống
    }

    public override void ApplyAtributeAbility(PlayerController playerController) {
        playerController.AddMaxReviveZombie(int_amountReviveBonus);
    }

    public override void ApplyEconomyAbility(Weapon weapon) {
        // không dùng → để trống
    }
}
