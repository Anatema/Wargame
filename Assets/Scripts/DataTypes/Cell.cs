using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IHeapItem<Cell>
{
    public HexGrid Grid;
    public bool IsReachable;
    public HexCoordinates coordinates;    

    public List<Cell> Neighbors;
    public Color OldColor;

    public int GCost;
    public int HCost;
    public Cell parent;

    public int movementCost;
    private int _coverLevel;
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
    public void HiglightBorder()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    public void Higlight()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        //StartCoroutine(GetColorBack());
    }
    public void SetAsNormal()
    {
        GetComponent<MeshRenderer>().material.color = OldColor;
    }
    public IEnumerator GetColorBack()
    {
        yield return new WaitForSeconds(0.2f);
        //GetComponent<MeshRenderer>().material.color = OldColor;
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
