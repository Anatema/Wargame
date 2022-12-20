using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IHeapItem<Cell>
{
    public HexGrid Grid;
    public List<Cell> Neighbors;
    public Color OldColor;
    
    public HexCoordinates coordinates;    
    public bool IsReachable;

    public int GCost;
    public int HCost;
    public Cell parent;

    public int movementCost;
    [SerializeField]
    private int _coverLevel;
    [SerializeField]
    private int _obcureLevel;


    private GroundType _groundType;
    public int FCost => GCost + HCost;
    private int _heapIndex;

    [SerializeField]
    private Unit _groundUnit;
    public Unit GroundUnit => _groundUnit;

    public void Awake()
    {
        OldColor = GetComponent<MeshRenderer>().material.color;
    }
    public int HeapIndex
    {
        get 
        {
            return _heapIndex;
        }
        set 
        {
            _heapIndex = value;
        }
    }

    public void SetGroundType(GroundType type)
    {
        _groundType = type;
        //
        switch (type)        
        {
            default:
                _coverLevel = 0;
                _obcureLevel = 0;
                break;
            case GroundType.None:
                _coverLevel = 0;
                _obcureLevel = 0;
                break;
            case GroundType.Road:
                _coverLevel = 0;
                _obcureLevel = 0;
                break;
            case GroundType.Field:
                _coverLevel = 0;
                _obcureLevel = 0;
                break;
            case GroundType.Forest:
                _coverLevel = 2;
                _obcureLevel = 3;
                break;
            case GroundType.Swamp:
                _coverLevel = -1;
                _obcureLevel = -2;
                break;
            case GroundType.Mountan:
                _coverLevel = 2;
                _obcureLevel = 2;
                break;
            case GroundType.Castle:
                _coverLevel = 3;
                _obcureLevel = 3;
                break;


        }
    }
    public void SetUnit(Unit unit)
    {
        _groundUnit = unit;
    }
    public void RemoveGroundUnit()
    {
        if (_groundUnit)
        {
            _groundUnit.Movement.RemoveCell();
            _groundUnit = null;
        }
    }

    //Actor
    public void HiglightBorder()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    //Actor
    public void Higlight()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    //Actor
    public void SetAsNormal()
    {
        GetComponent<MeshRenderer>().material.color = OldColor;
    }
    
    public int CompareTo(Cell cellToCompare)
    {
        int compare = FCost.CompareTo(cellToCompare.FCost);
        if(compare == 0)
        {
            compare = HCost.CompareTo(cellToCompare.HCost);
        }
        return -compare;
    }

}
