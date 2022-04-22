using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IHeapItem<Cell>
{
    public bool IsReachable;
    public HexCoordinates coordinates;    
    public Text label;

    public List<Cell> Neighbors;
    public Color OldColor;

    public int GCost;
    public int HCost;
    public Cell parent;
    public int movementCost;
    public int FCost => GCost + HCost;
    private int _heapIndex;

    public void Awake()
    {
        OldColor = GetComponent<MeshRenderer>().material.color;
    }
    public void Higlight()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        StartCoroutine(GetColorBack());
    }
    public IEnumerator GetColorBack()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().material.color = OldColor;
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
