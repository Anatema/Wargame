using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid _hexGrid;
    [SerializeField]
    private Battle _battle;
    public static LevelManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void CreateGrid(Cell cellPrefab, int gridRadius)
    {
        CheckComponents();
        DeleteGrid();
        _hexGrid.CreateGrid(cellPrefab, gridRadius);
    }
    public void DeleteGrid()
    {
        CheckComponents();
        RemoveAllUnits();
        _hexGrid.DeleteGrid();
    }

    public void ChangeHexGround(Ground groundType, Cell cell)
    {
        CheckComponents();
        cell.OnLandChanged(groundType);

    }

    public void AddUnit(UnitData unitData, Cell targetCell, int unitSide)
    {
        CheckComponents();
        if (!targetCell || targetCell.GroundUnit)
        {
            return;
        }
        _battle.AddUnit(unitData, targetCell, unitSide);
    }
    public void RemoveUnit(Cell targetCell)
    {
        CheckComponents();
        if (!targetCell || !targetCell.GroundUnit)
        {
            return;
        }
        Unit unit = targetCell.GroundUnit;
        _battle.RemoveUnit(unit);
        
    }
    public void RemoveAllUnits()
    {
        CheckComponents();
        _battle.RemovaAllUnits();
    }

    private void CheckComponents()
    {
        if (!_hexGrid)
        {
            if (FindObjectOfType<HexGrid>())
            {
                _hexGrid = FindObjectOfType<HexGrid>();
            }
            else
            {
                GameObject obj = new GameObject("Board");
                _hexGrid = obj.AddComponent<HexGrid>();
            }
        }

        if (!_battle)
        {
            if (FindObjectOfType<Battle>())
            {
                _battle = FindObjectOfType<Battle>();
            }
            else
            {
                GameObject obj = new GameObject("Battle");
                _battle = obj.AddComponent<Battle>();
            }
        }

    }
    public HexGrid GetGrid()
    {
        return _hexGrid;
    }
    public Battle GetBattle()
    {
        return _battle;
    }
}
