using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Movement Movement;
    public Health Health;
    public List<Ability> Abilities;
    private MeshRenderer _meshRenderer;

    public Cell Cell => Movement.Cell;

    public byte PlayerIndex;

    private float _maxMorale;
    private float _currentMorale;

    [SerializeField]
    private int _maxUnitSize;
    [SerializeField]
    private int _currentUnitSize;

    public int MaxUnitSize => _maxUnitSize;
    public int CurrentUnitSize => _currentUnitSize;
      

    public int Defence => Health.Defence;
    public int MaxHealth => Health.MaxHealth;
    public int CurrentHealth => Health.CurrentHealth;

    public bool IsEnded;

    //private List<Properties> _traits;
    //private GameObject _actior / private Actor _actor;


    public void RecoverUnit()
    {
        IsEnded = false;
        Movement.RecoverMP();
    }
    public void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        Health.SetUnit(this);
        Movement.SetUnit(this);

    }
    public void Instantiate(Cell cell, UnitData unitData)
    {
        Movement = new Movement(this, cell, unitData);
        Health = new Health(unitData);

        _maxMorale = unitData.MaxMorale;
        _currentMorale = _maxMorale;
        _maxUnitSize = unitData.MaxUnitSize;
        _currentUnitSize = _maxUnitSize;

        Abilities = unitData.Abilities;
    }

    public void Active()
    {
        _meshRenderer.material.color = Color.yellow;
    }

    public void Unactive()
    {
        _meshRenderer.material.color = Color.white;
    }

    public void Death()
    {
        if (FindObjectOfType<Battle>())
        {
            FindObjectOfType<Battle>().RemoveUnit(this);
        }
        Cell.RemoveGroundUnit();
        Debug.Log("Unit killed!");
        Destroy(this.gameObject);
    }
    public void RemoveModel()
    {
        _currentUnitSize--;
    }
}
