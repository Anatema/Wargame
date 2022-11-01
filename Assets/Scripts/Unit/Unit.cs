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

    private int _maxActionPoints;
    private int _currentActionPoints;

    private int _speed;

    //private List<Trait> _traits;
    //private GameObject _actior / private Actor _actor;



    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public void Update()
    {
        
    }
    public void Instantiate(Cell cell)
    {
        Movement = new Movement(this, cell);
        Health = new Health(this);

        _currentMorale = _maxMorale;
        _currentUnitSize = _maxUnitSize;
        _currentActionPoints = _maxActionPoints;
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
        Debug.Log("Unit killed!");
    }
    public void RemoveModel()
    {
        _currentUnitSize--;
    }
}
