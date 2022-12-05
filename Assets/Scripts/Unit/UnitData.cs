using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public string Name;

    public GameObject Model;

    public int MaxHealth;
    public int MaxMorale;
    public int MaxUnitSize;
    public int Defence;
    public int MaxActionPoints;

    public List<Ability> Abilities;
}
