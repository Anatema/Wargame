using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Health
{
    private Unit _unit;

    [SerializeField]
    private int _defence;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _currentHealth;

    public int Defence => _defence;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    public Health(UnitData unitData)
    {
        _maxHealth = unitData.MaxHealth;
        _currentHealth = _maxHealth;
    }
    public void SetUnit(Unit unit)
    {
        _unit = unit;
    }
    public void TakeDamage(List<Damage> damage)
    {
        foreach(Damage d in damage)
        {
            _currentHealth -= d.DamageValue;
            if(_currentHealth < 1)
            {
                _unit.RemoveModel();
                if(_unit.CurrentUnitSize < 1)
                {
                    Death();
                    break;
                }
                _currentHealth = _maxHealth;
            }
        }
    }

    private void Death()
    {
        _unit.Death();
    }

}
