using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType { AttackAbility, AttackEffectAbility, AtributeAbility, EconomyAbility}
public abstract class AbilityBase : ScriptableObject
{
    public string abilityID;
    public string abilityName;
    public AbilityType abilityType;
    public Sprite abilitySprite;

    public abstract void ApplyAttackAbility(PlayerController playerController, Transform target);
    public abstract void ApplyAttackEffectAbility(Weapon weapon);
    public abstract void ApplyAtributeAbility(PlayerController playerController);
    public abstract void ApplyEconomyAbility(Weapon weapon);
}
