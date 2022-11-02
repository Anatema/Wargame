using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Actions/Move")]
public class Move : Action
{
    public bool ClosestPoint;
    public override bool CheckConditions(Unit caster, Cell target)
    {
        HexGrid grid = target.Grid;
        List<Cell> path = grid.CalculatePath(caster.Cell, target, ClosestPoint);
        if (path == null)
        {
            return false;
        }
        path.Reverse();
        if (ClosestPoint)
        {
            path.RemoveAt(path.Count - 1);
        }
        int movementLeft = caster.Movement.CurrentMovingPoints;
        foreach (Cell cell in path)
        {
            movementLeft -= cell.movementCost;
        }
        if (movementLeft < 0)
        {
            return false;
        }
        return true;
    }

    public override void Invoke(Unit caster, Cell target)
    {
        HexGrid grid = target.Grid;
        List<Cell> path = grid.CalculatePath(caster.Cell, target, true);
        path.Reverse();
        if (ClosestPoint)
        {
            path.RemoveAt(path.Count - 1);
        }

        caster.Movement.Move(path);
    }
}
