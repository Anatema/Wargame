using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Single", menuName = "Targeter/Single")]
public class TargeterSingle : Targeter
{
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
                foreach(TargetType targetType in RequredTarget)
                {
                    if (TargeterUtility.CheckTarget(targetType, caster, target))
                    {
                        targets.Add(target);
                        continue;
                    }
                }
            }
        }
        
    }
    public override List<Cell> GetShape(Cell target)
    {
        List<Cell>cellPattern = new List<Cell>();
        cellPattern.Add(target);
        return cellPattern;
    }
}
