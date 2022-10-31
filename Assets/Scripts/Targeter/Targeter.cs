using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Targeter : ScriptableObject
{
    public List<Action> actions = new List<Action>();
    //friendly fire

    //property Filter

    //Requred - Must have at leat one
    //Forbiden - Must not have any
    //Must Have - Must have all

    //Return shape method

    public abstract void GetTargets(Cell targetCell, out List<Cell> targets, out List<Cell> cellPattern);
    //if can target ground

    
}
