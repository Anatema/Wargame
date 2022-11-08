using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargeterUtility
{
    public static bool CheckTarget(TargetType type, Unit caster, Cell target)
    {
        switch (type)
        {
            case TargetType.Any:
                return true;
            case TargetType.Self:
                return CheckSelf(caster, target);
            case TargetType.Other:
                return !CheckSelf(caster, target);
            case TargetType.Ally:
                return CheckAlly(caster, target);
            case TargetType.Enemy:
                return !CheckAlly(caster, target);
            case TargetType.Ground:
                return CheckGround(target);
            default:
                return false;
        }
    }
    private static bool CheckGround(Cell target)
    {
        return !target.GroundUnit;
    }
    private static bool CheckSelf(Unit caster, Cell target)
    {
        if (target.GroundUnit && caster == target.GroundUnit)
        {
            return true;
        }
        return false;
    }
    private static bool CheckAlly(Unit caster, Cell target)
    {
        if (target.GroundUnit && caster.PlayerIndex == target.GroundUnit.PlayerIndex)
        {
            return true;
        }
        return false;
    }
}
