using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Targeter : ScriptableObject
{
    public List<Action> Actions = new List<Action>();

    /// <summary>
    /// Target should have pass at least on of the following requrements
    /// </summary>
    public List<TargetType> RequredTarget;
    //Forbiden
    //Must have

    //property Filter

    //Requred - Must have at leat one
    //Forbiden - Must not have any
    //Must Have - Must have all

    //Return cells that can be targeted
    public abstract void GetTargets(Unit caster, Cell targetCell, out List<Cell> targets, out List<Cell> cellPattern);

    public abstract bool IsTarget(Unit caster, Cell targetCell);

    //Return shape method
    public abstract List<Cell> GetShape(Cell target);
    //if can target ground


}
