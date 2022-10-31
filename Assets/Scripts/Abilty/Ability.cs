using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilty")]
public class Ability: ScriptableObject
{
    public string Name;

    public PlayerState State;
    public List<PreviewTag> PreviewTags;
    public List<Action> Actions;

    public bool CanMoveAndAct;
    public bool CanTargetGround;
    public void Prepare()
    {

    }

    public bool Invoke(Unit caster, Cell target)
    {
        /*foreach (Action action in Actions)
        {
            if(!action.CheckConditions(caster, target))
            {
                return false;
            }
        }*/
        foreach(Action action in Actions)
        {
            action.CheckConditions(caster, target);   
            action.Invoke(caster, target);
        }
        return true;
    }
}
