using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Movement
{
    [SerializeField]
    private Unit _unit;
    public int MovingPoints;
    public Cell Cell;
    public Movement(Unit unit, Cell cell)
    {
        _unit = unit;
        Cell = cell;
        Cell.SetUnit(unit);
        unit.transform.position = cell.transform.position + Vector3.up*3;
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
            Debug.Log(cell.coordinates);
            Cell.RemoveGroundUnit();
            Cell = cell;
            Cell.SetUnit(_unit);
            _unit.transform.position = cell.transform.position + Vector3.up * 3;
        }
    }
   
    public List<Cell> GetReach()
    {       
        return Cell.Grid.CalculateReach(Cell, MovingPoints);
    }
    public void RemoveCell()
    {
        Cell = null;
    }
}
