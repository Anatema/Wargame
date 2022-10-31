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
                data += cell.coordinates.ToString() + "\n";
                if (cell.GroundUnit != null)
                {
                    data += cell.GroundUnit.name;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        PlayerController.SelectedUnit = cell.GroundUnit;
                        PlayerController.SelectState(PlayerController.Acting);
                        return;
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
