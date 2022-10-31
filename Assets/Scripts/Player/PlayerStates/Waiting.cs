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
                }

                PlayerController.DataPanel.ShowData(data, Input.mousePosition);
                return;
            }
        }
        PlayerController.DataPanel.HideData();
    }
}
