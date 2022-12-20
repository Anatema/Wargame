using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Damage
{
    public int MinDamage;
    public int MaxDamage;

    public int MinMoralDamage;
    public int MaxMoralDamage;

    public int GetDamage()
    {
        return UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
    }

    public int GetMoralDamage()
    {
        return UnityEngine.Random.Range(MinMoralDamage, MaxMoralDamage + 1);
    }
    //Damage type
}


