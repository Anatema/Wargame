using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Movement
{
    private Unit _unit;
    public int CurrentMovingPoints;
    public int MaxMovingPoints;
    public Cell Cell;
    public Movement(Unit unit, Cell cell, UnitData unitData)
    {
        _unit = unit;
        Cell = cell;
        Cell.SetUnit(unit);
        unit.transform.position = cell.transform.position + Vector3.up * 6f;

        MaxMovingPoints = unitData.MaxActionPoints;
    }
    public void SetUnit(Unit unit)
    {
        _unit = unit;
    }
    
    public void Move(List<Cell> path)
    {
        MoveInstant(path);
        //_unit.StartCoroutine(SlowMove(path));
    }
    private void MoveInstant(List<Cell> path)
    {
        foreach (Cell cell in path)
        {
            Cell.RemoveGroundUnit();
            Cell = cell;
            Cell.SetUnit(_unit);
            _unit.transform.position = cell.transform.position + Vector3.up * 6f;

            CurrentMovingPoints -= cell.movementCost;
        }
    }
    
    public void RecoverMP()
    {
        CurrentMovingPoints = MaxMovingPoints;
    }
    public List<Cell> GetReach()
    {
        List<Cell> list = Cell.Grid.CalculateReach(Cell, CurrentMovingPoints);
        if (!list.Contains(_unit.Cell))
        {
            list.Add(_unit.Cell);
        }
        return list;
    }
    public void RemoveCell()
    {
        Cell = null;
    }
}
