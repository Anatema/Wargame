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
        if (target.GroundUnit == null)
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
        int modificators = CalculateModificators(caster, target);

        List<Damage> damages = new List<Damage>();
        for(int i = 0; i < numberOfAttacks; i++)
        {
            if(IsHitSuccesfull(caster, target, 10)) 
            {
                Damage damage = Damage;
                damages.Add(damage);
            }
        }


        target.GroundUnit.Health.TakeDamage(caster, damages);
    }
    private bool IsHitSuccesfull(Unit caster, Cell target, int modificators)
    {
        int value = Random.Range(0, 100);
        if (value < 10)
        {
            Debug.Log("Auto fail " + value);
            return false;
        }
        if (value >= 90)
        {
            Debug.Log("Auto success " + value);
            return true;
        }

        int accuracy = caster.Accuracy;

        if (value + modificators < accuracy)
        {
            Debug.Log("Success " + value);
            return true;
        }
        Debug.Log("Fail " + value);
        return false;
    }
    private int CalculateModificators(Unit caster, Cell target)
    {
        Debug.Log("Unit cover" + target.GroundUnit.Size);
        HexGrid grid = caster.Cell.Grid;

        grid.CalculateLine(caster.Cell.coordinates, target.coordinates);
        //
        return 0;
    }

}
