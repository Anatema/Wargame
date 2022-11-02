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
                string data = "";
                //data += targetCell.coordinates.ToString() + "\n";
                if (cell.GroundUnit != null)
                {
                    data += "Name :" + cell.GroundUnit.name + "\n";
                    data += "Health :" + cell.GroundUnit.CurrentHealth + "\\" + cell.GroundUnit.MaxHealth + "\n";
                    data += "Squad size :" + cell.GroundUnit.CurrentUnitSize + "\\" + cell.GroundUnit.MaxUnitSize + "\n";
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (!cell.GroundUnit.IsEnded)
                        {
                            PlayerController.SelectedUnit = cell.GroundUnit;
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

    public override void EndState()
    {
        PlayerController.DataPanel.HideData();
    }

    public override void EnterState()
    {
    }
}
