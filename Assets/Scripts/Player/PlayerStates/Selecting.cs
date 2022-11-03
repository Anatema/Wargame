using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Selecting : PlayerState
{
    public Selecting(PlayerController playerController)
    {
        PlayerController = playerController;
        StateName = "Selecting";
    }

    public override void Update()
    {
        CastRay();
    }
    private void CastRay()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 64))
        {
            if (hit.collider.GetComponent<Cell>())
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                string data = GetData(cell);
                //data += targetCell.coordinates.ToString() + "\n";
                if (cell.GroundUnit != null)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (!cell.GroundUnit.IsEnded)
                        {
                            PlayerController.SelectUnit(cell.GroundUnit);
                            PlayerController.SelectState(PlayerController.Acting);
                            return;
                        }
                    }
                }

                PlayerController.DataPanel.ShowData(data, Input.mousePosition);
                return;
            }
        }
        PlayerController.DataPanel.HideData();
    }
    private string GetData(Cell target)
    {
        string data = "";
        data += target.coordinates.ToString() + "\n";
        if (target.GroundUnit != null)
        {
            data += "Name :" + target.GroundUnit.name + "\n";
            data += "Health :" + target.GroundUnit.CurrentHealth + "\\" + target.GroundUnit.MaxHealth + "\n";
            data += "Squad size :" + target.GroundUnit.CurrentUnitSize + "\\" + target.GroundUnit.MaxUnitSize + "\n";
        }
        return data;
    }

    public override void EndState()
    {
        PlayerController.DataPanel.HideData();
    }
    public override void EnterState()
    {
        if (PlayerController.SelectedUnit)
        {
            PlayerController.DeselectUnit();
        }
    }
}
