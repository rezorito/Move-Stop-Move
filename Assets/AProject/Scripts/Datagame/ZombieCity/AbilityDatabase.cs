using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Database", menuName = "Game/Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    public AbilityBase[] allAbilitys;

    public AbilityBase getAbilityByID(string _abilityID)
    {
        foreach(AbilityBase ability in allAbilitys)
        {
            if(ability.abilityID == _abilityID)
            {
                return ability;
            }
        }
        return null;
    }
}
