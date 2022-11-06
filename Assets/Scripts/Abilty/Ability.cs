using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilty")]
public class Ability: ScriptableObject
{
    public string Name;

    public PlayerState State;

    public bool CanMoveAndAct;
    public bool CanTargetGround;

    public int Range;
    

    public List<Targeter> Targeters;

    public void Prepare(Unit caster, out List<Cell> achivableTargets)
    {
        List<Cell> achivableCells = GetAchiavbleCells(caster);
        achivableTargets = GetAvaliableTargets(achivableCells, caster);
    }

    private List<Cell> GetAchiavbleCells(Unit caster)
    {
        if (CanMoveAndAct && !CanTargetGround)
        {
            return caster.Movement.GetReach();
        }
        else return new List<Cell>() { caster.Cell };
    }

    
    public bool Invoke(Unit caster, Cell target)
    {
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        List<Cell> avaliableCells = GetAchiavbleCells(caster);
        List<Cell> avaliableTargets = GetAvaliableTargets(avaliableCells, caster);


        if (target.GroundUnit == null && avaliableCells.Contains(target))
        {
            Move(grid, caster, target);
        }
        else if (avaliableTargets.Contains(target) && HexGrid.CubeDistance(target, caster.Cell) <= Range)
        {
            Debug.Log("attack!");

            if(InvokeAbilty(caster, target))
            {
                caster.Movement.CurrentMovingPoints = 0;
            }
            //attack
        }
        else if (avaliableTargets.Contains(target) && HexGrid.CubeDistance(target, caster.Cell) > Range)
        {
            Debug.Log("Move and Attack!");

            MoveWithinReach(grid, caster, target);

            if (InvokeAbilty(caster, target))
            {
                caster.Movement.CurrentMovingPoints = 0;
            }
        }
        else
        {
            Debug.Log("Can`t do!");
            return false;
        }
        return !caster.IsEnded;

    }
    private void Move(HexGrid grid, Unit caster, Cell target)
    {
        Debug.Log("Move!");
        List<Cell> path = grid.CalculatePath(caster.Cell, target);
        caster.Movement.Move(path);
    }
    private void MoveWithinReach(HexGrid grid, Unit caster, Cell target)
    {
        List<Cell> path = grid.GetPathWithReach(caster.Cell, target, Range);
        if (path != null)
        {
            caster.Movement.Move(path);
        }
    }

    private bool InvokeAbilty(Unit caster, Cell target)
    {
        foreach (Targeter targeter in Targeters)
        {
            targeter.GetTargets(caster, target, out List<Cell> targets, out List<Cell> cellPattern);
            foreach (Cell cell in targets)
            {
                foreach (Action action in targeter.Actions)
                {
                    action.Invoke(caster, cell);
                    caster.IsEnded = true;

                }
            }
        }
        return caster.IsEnded;
    }
    //SpendMovementPoints

    public List<Cell> GetAvaliableTargets(List<Cell> cellsRange, Unit caster)
    {
        //HERE I CHECK TARGETERS

        List<Cell> targets = new List<Cell>();
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        Battle battle = GameObject.FindObjectOfType<Battle>();

        foreach (Unit unit in battle.Units)
        {
            foreach (Targeter targeter in Targeters)
            {
                targeter.GetTargets(caster, unit.Cell, out List<Cell> returnTargets, out List<Cell> cellPattern);

                List<Cell> CellsInRange = grid.CalculateReach(unit.Cell, Range, false, false);
                foreach (Cell cell in CellsInRange)
                {
                    foreach (Cell target in returnTargets)
                    {
                        if (cellsRange.Contains(cell) || cell.GroundUnit == caster)
                        {
                            targets.Add(target);
                            break;
                        }
                    }
                        //Check targeter
                    /*if ((cellsRange.Contains(cell) || cell.GroundUnit == caster) && unit)
                    {
                        targets.Add(unit.Cell);
                        break;
                    }*/
                }
            }
        }
        return targets;
    }
    
}
