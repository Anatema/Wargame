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

    //Requred
    //Forbiden
    //Must have

    //property Filter


    //Requred - Must have at leat one
    //Forbiden - Must not have any
    //Must Have - Must have all

    //Return shape method

    public abstract void GetTargets(Unit caster, Cell targetCell, out List<Cell> targets, out List<Cell> cellPattern);
    //if can target ground

    
}
