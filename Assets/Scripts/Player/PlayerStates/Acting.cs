using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[SerializeField]
public class Acting : PlayerState
{
    private Unit _activeUnit;

    private Ability _activeAbilty;
    private LineRenderer _line;
    

    private LineRenderer _attackLine;
    private Cell _previustarget = null;

    private List<Unit> _achivableUnits;
    private List<Cell> _achivableCells;
    private List<Cell> _currentPath;
    public Acting(PlayerController playerController)
    {
        PlayerController = playerController;
        StateName = "Acting";
    }
    public override void EndState()
    {
        if (!PlayerController.SelectedUnit)
        {
            return;
        }
        //UnityEngine.Object.Destroy(_line.gameObject);
        PlayerController.Battle.Grid.ClearGrid();

        PlayerController.SelectedUnit.Unactive();
        PlayerController.SelectedUnit = null;

        if (_line)
        {
            GameObject.Destroy(_line.gameObject);
        }
        if (_attackLine)
        {
            GameObject.Destroy(_attackLine.gameObject);
        }
    }
    public override void EnterState()
    {
        if (!PlayerController.SelectedUnit)
        {
            return;
        }

        PlayerController.SelectedUnit.Active();
        _activeUnit = PlayerController.SelectedUnit;
        if(_activeUnit.Abilities.Count <= 0)
        {
            PlayerController.SelectState(PlayerController.Waiting);
            return;

        }
        _activeAbilty = _activeUnit.Abilities[0];

        ShowPreview();

        //_line = new GameObject("Line").AddComponent<LineRenderer>();
        _line = GameObject.Instantiate(PlayerController.LinePrefab);

        _attackLine = GameObject.Instantiate(PlayerController.LinePrefab);
        _attackLine.startColor = Color.red;
    }
    public void ShowPreview()
    {
        if (_activeAbilty.CanMoveAndAct && !_activeAbilty.CanTargetGround)
        {
            _achivableCells = ShowReach();
        }
        _achivableUnits = CheckTargets(_achivableCells, 3);
        foreach (Unit unit in _achivableUnits)
        {
            if (unit != _activeUnit)
            {
                unit.Cell.Higlight();
            }
        }
        //ifMoveAndUse

    }




    public override void Update()
    {
        RegisterCellUnderMouse();
    }
    private void RegisterCellUnderMouse()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 64))
        {
            if (hit.collider.GetComponent<Cell>())
            {
                Cell targetCell = hit.collider.GetComponent<Cell>();
                //Show targeter;

                
                if (_achivableUnits.Contains(targetCell.GroundUnit))
                {
                    ShowAttackLine(targetCell, 3);
                }
                else
                {
                    ShowLine(targetCell);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (targetCell.GroundUnit == null && _achivableCells.Contains(targetCell))
                    {
                        Debug.Log("Move!");
                        List<Cell> path = PlayerController.Battle.Grid.CalculatePath(PlayerController.SelectedUnit.Cell, targetCell);
                        Debug.Log(path.Count);
                        path.Reverse();
                        _activeUnit.Movement.Move(path);

                    }
                    else if (_achivableUnits.Contains(targetCell.GroundUnit) && HexGrid.CubeDistance(targetCell, _activeUnit.Cell) <= 3)
                    {
                        Debug.Log("attack!");
                        foreach(Targeter targeter in _activeAbilty.Targeters)
                        {
                            targeter.GetTargets(targetCell, out List<Cell> targets, out List<Cell> cellPattern);
                            foreach(Cell cell in targets)
                            {
                                foreach(Action action in targeter.actions)
                                {
                                    action.Invoke(_activeUnit, cell);
                                }
                            }
                        }
                        //attack
                    }
                    else if (_achivableUnits.Contains(targetCell.GroundUnit) && HexGrid.CubeDistance(targetCell, _activeUnit.Cell) > 3)
                    {
                        Debug.Log("Move and Attack!");
                        if (_currentPath != null)
                        {
                            _activeUnit.Movement.Move(_currentPath);
                        }
                        foreach (Targeter targeter in _activeAbilty.Targeters)
                        {
                            targeter.GetTargets(targetCell, out List<Cell> targets, out List<Cell> cellPattern);
                            foreach (Cell cell in targets)
                            {
                                foreach (Action action in targeter.actions)
                                {
                                    action.Invoke(_activeUnit, cell);
                                }
                            }
                        }
                        //move and attack
                        //attack
                    }
                    else
                    {
                        Debug.Log("Can`t do!");
                    }
                    PlayerController.SelectState(PlayerController.Selecting);
                }
                //{
                //if (_activeAbilty.Invoke(_activeUnit, targetCell))
                //{
                //_activeAbilty.Invoke(_activeUnit, targetCell);
                //PlayerController.SelectState(PlayerController.Waiting);
                //}
                ///}
                _previustarget = targetCell;
                return;
            }
            
        }
    }
    private void ShowData(Cell targetCell)
    {
        string data = "";
        data += targetCell.coordinates.ToString() + "\n";
        if (targetCell.GroundUnit != null)
        {
            data += targetCell.GroundUnit.name;
        }
        PlayerController.DataPanel.ShowData(data, Input.mousePosition);

    }
    private void ShowLine(Cell targetCell)
    {
        _line.positionCount = 1;
        _attackLine.positionCount = 1;
        List<Cell> path = PlayerController.Battle.Grid.CalculatePath(PlayerController.SelectedUnit.Cell, targetCell, true);
        if (path == null)
        {
            _line.positionCount = 0;
            return;
        }
        path.Reverse();
        //count++;



        _line.positionCount = path.Count() + 1;
        _line.SetPosition(0, PlayerController.SelectedUnit.Cell.transform.position + Vector3.up * 5);
        int count = 1;
        foreach (Cell cell in path)
        {
            _line.SetPosition(count, cell.transform.position + Vector3.up * 5);
            count++;
        }
    }
    private void ShowAttackLine(Cell targetCell, int weaponeRange = 2)
    {
        if(targetCell == _previustarget)
        {
            return;
        }
        _line.positionCount = 1;
        _attackLine.positionCount = 1;
        List<Cell> path = PlayerController.Battle.Grid.CalculatePath(PlayerController.SelectedUnit.Cell, targetCell, true);
        
        if (path == null)
        {
            return;
        }
        path.Reverse();

        if (!targetCell.GroundUnit)
        {
            return;
        }
        int count = 1;

        _attackLine.positionCount = 2;
        _line.SetPosition(0, PlayerController.SelectedUnit.Cell.transform.position + Vector3.up * 5);
        if (HexGrid.CubeDistance(PlayerController.SelectedUnit.Cell, targetCell) <= weaponeRange)
        {
            
            _attackLine.SetPosition(0, PlayerController.SelectedUnit.Cell.transform.position + Vector3.up * 5);
            _attackLine.SetPosition(1, targetCell.transform.position + Vector3.up * 5);
            return;
        }
        _currentPath = new List<Cell>();
        foreach (Cell cell in path)
        {
            _currentPath.Add(cell);
            _line.positionCount++;
            _line.SetPosition(count, cell.transform.position + Vector3.up * 5);
            count++;

            if (HexGrid.CubeDistance(cell, targetCell) <= weaponeRange)
            {
                _attackLine.SetPosition(0, cell.transform.position + Vector3.up * 5);
                _attackLine.SetPosition(1, targetCell.transform.position + Vector3.up * 5);
                break;
            }
        }

    }
    private List<Cell> ShowReach()
    {
        List<Cell> visitedCells = PlayerController.Battle.Grid.CalculateReach(_activeUnit.Cell, _activeUnit.Movement.MovingPoints, true);
        foreach (Cell cell in visitedCells)
        {
            cell.HiglightBorder();

        }
        return visitedCells;
    }
    private List<Unit> CheckTargets(List<Cell> cellsRange, int abiltyRange)
    {
        List<Unit> targets = new List<Unit>();
        foreach (Unit unit in PlayerController.Battle.Units)
        {
            List<Cell> CellsInRange = PlayerController.Battle.Grid.CalculateReach(unit.Cell, abiltyRange, false, false);
            foreach (Cell cell in CellsInRange)
            {
                //Check targeter
                if (cellsRange.Contains(cell) && unit)
                {
                    targets.Add(unit);
                    break;
                }
            }
        }
        return targets;
    }
}

