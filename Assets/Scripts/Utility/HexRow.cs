using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexRow
{
    public List<Cell> Cells;
    public HexRow()
    {
        Cells = new List<Cell>();
    }
    public HexRow(List<Cell> cells)
    {
        Cells = cells;
    }
    public void Add(Cell cell)
    {
        Cells.Add(cell);
    }
}
