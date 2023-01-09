using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField]
    private int _maxMorale;
    [SerializeField]
    private int _currentMorale;
     
    public int Defence => _defence;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int MaxMoral => _maxMorale;
    public int CurrentMoral => _currentMorale;

    public Health(UnitData unitData)
    {
        _maxMorale = unitData.MaxMorale;
        _currentMorale = _maxMorale;
        _maxHealth = unitData.MaxHealth;
        _currentHealth = _maxHealth;
    }
    public void SetUnit(Unit unit)
    {
        _unit = unit;
        
    }
    public void TakeDamage(Unit caster, List<Damage> damage)
    {
        if(damage.Count > 0)
        {
            TakeWillDamage(damage[0]);
        }
        foreach (Damage d in damage)
        {
            TakePhysicalDamage(d);
        }
        CheckMorale();
        if (_unit.CurrentUnitSize < 1)
        {
            Death();
        }
        //Moral damage
    }
    private void CheckMorale()
    {
        Debug.Log((float)_currentMorale / (float)_maxMorale);
    }
    private void TakeWillDamage(Damage damage)
    {
        int moralDamage = damage.GetMoralDamage();
        _currentMorale -= moralDamage;
        Debug.Log("moral damage" + moralDamage);
    }
    private void TakePhysicalDamage(Damage d)
    {
        int damVal = CheckArmour(d);
        _currentHealth -= damVal;
        if (_currentHealth < 1)
        {
            _unit.RemoveModel();
            TakeWillDamage(d);
            _currentHealth = _maxHealth;
        }
    }
    private int CheckArmour(Damage damage)
    {
        int damageAmount = damage.GetDamage();

        if (damageAmount <= 0)
        {
            return 0;
        }

        if (damageAmount <= _defence)
        {
            float armourDamage = (float)damageAmount / (float)(_defence + damageAmount);

            if(armourDamage < UnityEngine.Random.Range(0f, 1f)) 
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return damageAmount -= _defence;
        }
    }

    
    private void Death()
    {
        _unit.Death();
    }

}
