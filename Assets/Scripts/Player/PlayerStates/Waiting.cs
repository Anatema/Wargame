using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Waiting: PlayerState
{
    public Waiting(PlayerController playerController)
    {
        PlayerController = playerController;
        StateName = "Waiting";
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
}
