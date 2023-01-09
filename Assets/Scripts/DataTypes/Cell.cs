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
    [SerializeField]
    private bool _canSeeThrough;
    [SerializeField]
    private Ground _ground;

    //private GroundType _groundType;
    public int FCost => GCost + HCost;
    public bool CanSeeThrough => _canSeeThrough;
    private int _heapIndex;

    [SerializeField]
    private Unit _groundUnit;
    public Unit GroundUnit => _groundUnit;

    public void Awake()
    {
        OldColor = GetComponent<MeshRenderer>().material.color;
    }

    public void Initialize(int x, int z, HexGrid grid)
    {
        Vector3 position = new Vector3();
        position.x = (x * HexMetrics.INNER_RADIUS * 2f + z * HexMetrics.INNER_RADIUS);
        position.z = z * HexMetrics.OUTER_RADIUS * 2f - HexMetrics.OUTER_RADIUS / 2 * z;

        coordinates = new HexCoordinates(x, z);
        Grid = grid;
        transform.SetParent(grid.transform, false);
        transform.localPosition = position;
    }


    public void OnLandChanged(Ground groundType)
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        if (groundType.GroundPrefab)
        {
            Instantiate(groundType.GroundPrefab, transform);
        }
        SetGroundType(groundType);
    }
    public void SetGroundType(Ground ground)
    {
        _ground = ground;

        movementCost = ground.MovementCost;
        _coverLevel = ground.CoverLevel;
        _obcureLevel = ground.ObsucreLevel;
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
