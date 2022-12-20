using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Actions/Attack")]
public class Attack : Action
{
    public Damage Damage;
    public int NumberOfAttacks;

    //hit modifiers
    public override bool CheckConditions(Unit caster, Cell target)
    {
        if(target.GroundUnit == null)
        {
            return false;
        }
        return true;
    }

    public override void Invoke(Unit caster, Cell target)
    {

        int numberOfAttacks;
        numberOfAttacks = NumberOfAttacks * caster.CurrentUnitSize;
        //number of attacks
        //line of view
        //main chance to hit
        //adittional chances to hit.

        List<Damage> damages = new List<Damage>();
        for(int i = 0; i < numberOfAttacks; i++)
        {
            Damage damage = Damage;
            damages.Add(damage);
        }

        
        target.GroundUnit.Health.TakeDamage(caster, damages);
    }
}
