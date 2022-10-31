using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Single", menuName = "Targeter/Single")]
public class TargeterSingle : Targeter
{
    public override void GetTargets(Cell targetCell, out List<Cell> targets, out List<Cell> cellPattern)
    {
        //Define all cells in cellPattern
        cellPattern = new List<Cell>();
        cellPattern.Add(targetCell);

        //Define acceptable targets from pattern
        //for now unitys
        targets = new List<Cell>();
        foreach (Cell target in cellPattern)
        {
            if (target.GroundUnit)
            {
                targets.Add(target);
            }
        }
        
    }
}
