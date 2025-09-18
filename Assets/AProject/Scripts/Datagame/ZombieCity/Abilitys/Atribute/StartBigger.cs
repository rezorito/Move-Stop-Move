using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Atribute/StartBigger")]
public class StartBigger : AbilityBase
{
    public float flt_scaleBigger;
    public override void ApplyAttackAbility(PlayerController playerController, Transform target) {
        // không dùng → để trống
    }

    public override void ApplyAttackEffectAbility(Weapon weapon)
    {
        // không dùng → để trống
    }

    public override void ApplyAtributeAbility(PlayerController playerController)
    {
        playerController.flt_valueScaleBigger = flt_scaleBigger;
        playerController.gameObject.transform.localScale += new Vector3(flt_scaleBigger, flt_scaleBigger, flt_scaleBigger);
        playerController.rangeDetect.setupRangeDetectZombie();
    }

    public override void ApplyEconomyAbility(Weapon weapon)
    {
        // không dùng → để trống
    }
}
