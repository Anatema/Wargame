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

    public void Prepare(Unit caster, out List<Cell> achivableCells, out List<Cell> achivableTargets)
    {
        achivableCells = GetAchiavbleCells(caster);
        achivableTargets = GetAvaliableTargets(achivableCells);
    }

    private List<Cell> GetAchiavbleCells(Unit caster)
    {
        return caster.Movement.GetReach();
    }

    
    public void Invoke(Unit caster, Cell target)
    {
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        List<Cell> avaliableCells = GetAchiavbleCells(caster);
        List<Cell> avaliableTargets = GetAvaliableTargets(avaliableCells);


        if (target.GroundUnit == null && avaliableCells.Contains(target))
        {
            Debug.Log("Move!");
            List<Cell> path = grid.CalculatePath(caster.Cell, target);
            path.Reverse();
            caster.Movement.Move(path);

        }
        else if (avaliableTargets.Contains(target) && HexGrid.CubeDistance(target, caster.Cell) <= Range)
        {
            Debug.Log("attack!");
            foreach (Targeter targeter in Targeters)
            {
                targeter.GetTargets(target, out List<Cell> targets, out List<Cell> cellPattern);
                foreach (Cell cell in targets)
                {
                    foreach (Action action in targeter.actions)
                    {
                        action.Invoke(caster, cell);
                    }
                }
            }
            //attack
        }
        else if (avaliableTargets.Contains(target) && HexGrid.CubeDistance(target, caster.Cell) > Range)
        {
            Debug.Log("Move and Attack!");

            List<Cell> path = grid.GetPathWithReach(caster.Cell, target, Range);
            if (path != null)
            {
                caster.Movement.Move(path);
            }

            foreach (Targeter targeter in Targeters)
            {
                targeter.GetTargets(target, out List<Cell> targets, out List<Cell> cellPattern);
                foreach (Cell cell in targets)
                {
                    foreach (Action action in targeter.actions)
                    {
                        action.Invoke(caster, cell);
                    }
                }
            }
            //move and attack
            //attack
        }
        else
        {
            Debug.Log("Can`t do!");
        }
    }

    public List<Cell> GetAvaliableTargets(List<Cell> cellsRange)
    {
        //HERE I CHECK TARGETERS

        List<Cell> targets = new List<Cell>();
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        Battle battle = GameObject.FindObjectOfType<Battle>();
        foreach (Unit unit in battle.Units)
        {
            List<Cell> CellsInRange = grid.CalculateReach(unit.Cell, Range, false, false);
            foreach (Cell cell in CellsInRange)
            {
                //Check targeter
                if (cellsRange.Contains(cell) && unit)
                {
                    targets.Add(unit.Cell);
                    break;
                }
            }
        }
        return targets;
    }
    private void Move()
    {

    }
}
