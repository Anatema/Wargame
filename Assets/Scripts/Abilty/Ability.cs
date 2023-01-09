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
    public bool NoRetaliation;
    public int Range;
    

    public List<Targeter> Targeters;

    public Sprite Icon;


    public void Prepare(Unit caster)
    {
        //List<Cell> achivableCells = GetAchiavbleCells(caster);
        //achivableTargets = GetAvaliableTargets(achivableCells, caster);
    }
    private List<Cell> GetAchiavbleCells(Unit caster)
    {
        if (CanMoveAndAct)
        {
            return caster.Movement.GetReach();
        }
        else return new List<Cell>() { caster.Cell };
    }
    public bool Invoke(Unit caster, Cell target)
    {
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        List<Cell> avaliableCells = GetAchiavbleCells(caster);
        List<Cell> avaliableTargets = GetAchivableTargets(avaliableCells, caster);


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

    public bool InvokeAbilty(Unit caster, Cell target, bool isRetaliation = false)
    {
        //List<Cell> targeted;
        foreach (Targeter targeter in Targeters)
        {
            targeter.GetTargets(caster, target, out List<Cell> targets, out List<Cell> cellPattern);
            foreach (Cell cell in targets)
            {
                cell.GetComponent<MeshRenderer>().materials[0].color = Color.black;
                foreach (Action action in targeter.Actions)
                {
                    action.Invoke(caster, cell);
                    caster.IsEnded = true;

                }
            }
        }
        if (target.GroundUnit)
        {
            target.GroundUnit.TargetedByAbility(!isRetaliation && !NoRetaliation, caster);
        }
        return caster.IsEnded;
    }
    //SpendMovementPoints
    public List<Cell> GetAchivableTargets(List<Cell> achivableCells, Unit caster)
    {
        List<Cell> possibleTargets = new List<Cell>();
        HexGrid grid = GameObject.FindObjectOfType<HexGrid>();
        Battle battle = GameObject.FindObjectOfType<Battle>();

        
        if (CanMoveAndAct)
        {
            foreach (Unit unit in battle.Units)
            {
                List<Cell> cellsInUnitRange = grid.CalculateReach(unit.Cell, Range, false, false);
                foreach (Cell cell in cellsInUnitRange)
                {
                    if (achivableCells.Contains(cell) && !possibleTargets.Contains(unit.Cell))
                    {
                        possibleTargets.Add(unit.Cell);
                        break;
                    }
                }
            }
        }
        else
        {
            possibleTargets = grid.CalculateReach(caster.Cell, Range, false, false);
            if (!possibleTargets.Contains(caster.Cell))
            {
                possibleTargets.Add(caster.Cell);
            }
        }

        //check for validity
        List<Cell> validTargets = new List<Cell>();
        foreach (Cell cell in possibleTargets)
        {
            foreach (Targeter targeter in Targeters)
            {
                if (targeter.IsTarget(caster, cell))
                {
                    validTargets.Add(cell);
                    break;
                }
            }
        }
        return validTargets;



    }
}
