using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Movement
{
    [SerializeField]
    private Unit _unit;
    public int CurrentMovingPoints;
    public int MaxMovingPoints;
    public Cell Cell;
    public Movement(Unit unit, Cell cell)
    {
        _unit = unit;
        Cell = cell;
        Cell.SetUnit(unit);
        unit.transform.position = cell.transform.position + Vector3.up * 6f;
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
   
    public List<Cell> GetReach()
    {
        
        return Cell.Grid.CalculateReach(Cell, CurrentMovingPoints);
    }
    public void RemoveCell()
    {
        Cell = null;
    }
}
