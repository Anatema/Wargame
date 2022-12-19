using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Damage
{
    public int MinDamage;
    public int MaxDamage;

    public int GetDamage()
    {
        return UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
    }
    //Damage type
}

