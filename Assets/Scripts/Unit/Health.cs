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

    public int Defence => _defence;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    public UnityEvent OnAttacked;

    public Health(UnitData unitData)
    {
        _maxHealth = unitData.MaxHealth;
        _currentHealth = _maxHealth;
    }
    public void SetUnit(Unit unit)
    {
        _unit = unit;
        
    }
    public void TakeDamage(Unit caster, List<Damage> damage)
    {
        foreach(Damage d in damage)
        {
            int damVal = CheckArmour(d);
            _currentHealth -= damVal;
            Debug.Log("End damage" + damVal);
            if (_currentHealth < 1)
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
        OnAttacked?.Invoke();
    }
    private int CheckArmour(Damage damage)
    {
        int damageAmount = damage.GetDamage();
        Debug.Log("Start damage" + damageAmount);

        if (damageAmount <= 0)
        {
            return 0;
        }

        if (damageAmount <= _defence)
        {
            float armourDamage = (float)damageAmount / (float)(_defence + damageAmount);
            Debug.Log("armour damage K" + armourDamage);

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
