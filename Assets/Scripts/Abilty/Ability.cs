using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilty")]
public class Ability: ScriptableObject
{
    public string Name;

    public PlayerState State;

    public bool CanMoveAndAct;
    public bool CanTargetGround;

    public List<Targeter> Targeters;
    public bool Invoke(Unit caster, Cell target)
    {
        return true;
    }

    private void Move()
    {

    }
}
