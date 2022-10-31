using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void Invoke(Unit caster, Cell target);
    public abstract bool CheckConditions(Unit caster, Cell target);
}
