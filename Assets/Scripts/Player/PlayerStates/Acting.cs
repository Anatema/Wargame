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

    private List<Cell> _achivableTargets;
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
        HiglightUnit();
        if (_activeUnit.Abilities.Count <= 0)
        {
            PlayerController.SelectState(PlayerController.Waiting);
            return;

        }
        _activeAbilty = _activeUnit.Abilities[0];
        _activeAbilty.Prepare(_activeUnit, out _achivableCells, out _achivableTargets);
        ShowPreview();

        //_line = new GameObject("Line").AddComponent<LineRenderer>();
        _line = GameObject.Instantiate(PlayerController.LinePrefab);

        _attackLine = GameObject.Instantiate(PlayerController.LinePrefab);
        _attackLine.startColor = Color.red;
    }
    private void HiglightUnit()
    {
        PlayerController.SelectedUnit.Active();
        _activeUnit = PlayerController.SelectedUnit;
    }
    public void ShowPreview()
    {
        ShowMoveReach();
        ShowAbiltyReach();
    }
    private void ShowMoveReach()
    {
        _achivableCells = new List<Cell>();
        if (_activeAbilty.CanMoveAndAct && !_activeAbilty.CanTargetGround)
        {
            ShowReach();
        }
        else
        {
            _achivableCells.Add(_activeUnit.Cell);
        }
    }
    private void ShowAbiltyReach()
    {        
        foreach (Cell cell in _achivableTargets)
        {
            if (cell.GroundUnit)
            {
                if (cell.GroundUnit != _activeUnit)
                {
                    cell.Higlight();
                }
            }
        }
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


                if (_achivableTargets.Contains(targetCell))
                {
                    ShowAttackLine(targetCell, _activeAbilty.Range);
                }
                else
                {
                    ShowLine(targetCell);
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _activeAbilty.Invoke(_activeUnit, targetCell);
                    PlayerController.SelectState(PlayerController.Selecting);
                }
                ShowData(targetCell);
                _previustarget = targetCell;
                return;
            }

        }
    }
    private void ShowData(Cell targetCell)
    {
        string data = "";
        //data += targetCell.coordinates.ToString() + "\n";
        if (targetCell.GroundUnit != null)
        {
            data += "Name :" + targetCell.GroundUnit.name + "\n";
            data += "Health :" + targetCell.GroundUnit.CurrentHealth + "\\" + targetCell.GroundUnit.MaxHealth + "\n";
            data += "Squad size :" + targetCell.GroundUnit.CurrentUnitSize + "\\" + targetCell.GroundUnit.MaxUnitSize + "\n";
            PlayerController.DataPanel.ShowData(data, Input.mousePosition);

        }

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
    private void ShowAttackLine(Cell targetCell, int weaponeRange)
    {
        if (targetCell == _previustarget)
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
        _currentPath = PlayerController.Battle.Grid.GetPathWithReach(_activeUnit.Cell, targetCell, _activeAbilty.Range);
        foreach (Cell cell in path)
        {
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
    private void ShowReach()
    {        
        foreach (Cell cell in _activeUnit.Movement.GetReach())
        {
            cell.HiglightBorder();

        }
    }
    
}

