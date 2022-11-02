using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Actions/Attack")]
public class Attack : Action
{
    public Damage Damage;
    public int NumberOfAttacks;
    public bool isRanged = false;
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

        List<Damage> damages = new List<Damage>();
        Debug.Log(numberOfAttacks);
        for(int i = 0; i < numberOfAttacks; i++)
        {
            Damage damage = Damage;
            damages.Add(damage);
        }
        target.GroundUnit.Health.TakeDamage(damages);
    }
}
