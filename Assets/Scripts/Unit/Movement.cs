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
    private IEnumerator SlowMove(List<Cell> path)
    {
        foreach (Cell cell in path)
        {
            Cell.RemoveGroundUnit();
            Cell = cell;
            Cell.SetUnit(_unit);
            Vector3 oldPosition = _unit.transform.position;
            for (int i = 0; i < 100; i++)
            {
                _unit.transform.position += (Cell.transform.position + Vector3.up * 3 - oldPosition) /100;
                yield return new WaitForSeconds(0.01f);
            }
            _unit.transform.position = Cell.transform.position + Vector3.up * 3;
        }
        yield return null;
    }
    
    public void RemoveCell()
    {
        Cell = null;
    }
}
