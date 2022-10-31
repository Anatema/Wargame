using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public HexGrid Grid;
    [SerializeField]
    public List<Unit> Units;

    public void AddUnit(Unit unit)
    {
        if (Units == null)
        {
            Units = new List<Unit>();
        }
        unit.transform.SetParent(transform);
        Units.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }
}
