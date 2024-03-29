using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Circle", menuName = "Targeter/Ground/Circle")]
public class GroundCircleTargeter : Targeter
{
    public int Size;
    public override void GetTargets(Unit caster, Cell targetCell, out List<Cell> targets, out List<Cell> cellPattern)
    {
        //Define pattern of Cells and give fitting Cells based on target cell
        cellPattern = GetShape(targetCell);

        //Check if cells in pattern go with the conditions
        targets = new List<Cell>();

        foreach (Cell target in cellPattern)
        {
            if (target.GroundUnit)
            {
                continue;
            }
            targets.Add(target);
        }
        
    }

    public override bool IsTarget(Unit caster, Cell targetCell)
    {
        return !targetCell.GroundUnit;
    }
    public override List<Cell> GetShape(Cell target)
    {
        //REDO!
        List<Cell> closedSet = new List<Cell>();

        closedSet.Add(target);
        
        
        for(int i = 0; i < Size; i++)
        {
            List<Cell> openedSet = new List<Cell>();
            foreach (Cell cell in closedSet)
            {
                foreach (Cell neighboud in cell.Neighbors)
                {
                    if (!openedSet.Contains(neighboud))
                    {
                        openedSet.Add(neighboud);
                    }
                }
            }
            foreach (Cell cell in openedSet)
            {
                if (!closedSet.Contains(cell))
                {
                    closedSet.Add(cell);
                }
            }
        }

        return closedSet;
    }

   
}
