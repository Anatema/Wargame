using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [SerializeField, Min(0)]
    public int GridRadius = 0;
    //public float CellSizeMultiplicator;

    public Cell CellPrefab;
    public Text CellLabelPrefab;
    public List<Cell> Cells;

    public HexGrid HexGrid;
    public Canvas GridCanvas;

    public Text Label;


    public void GenerateGrid()
    {
        ClearGrid();
        Cells = new List<Cell>();

        for (int y = -GridRadius; y <= GridRadius; y++)
        {
            int r1 = Mathf.Max(-GridRadius, -y - GridRadius);
            int r2 = Mathf.Min(GridRadius, -y + GridRadius);
            for (int x = r1; x <= r2; x++)
            {
                //CreateCell(x, y, -x - y);
                CreateCell(x, y, -y - x);
            }
        }

        SetCellsNeighbours();
        //
    }

    public void ClearGrid()
    {
        while (HexGrid.transform.childCount > 0)
        {
            DestroyImmediate(HexGrid.transform.GetChild(0).gameObject);
        }
        while (GridCanvas.transform.childCount > 0)
        {
            DestroyImmediate(GridCanvas.transform.GetChild(0).gameObject);
        }
    }
    private void CreateCell(int x, int y, int z)
    {
        Vector3 position = new Vector3(0, 0, 0);
        position.x = (x * HexMetrics.INNER_RADIUS * 2f + z * HexMetrics.INNER_RADIUS);
        position.z = z * HexMetrics.OUTER_RADIUS * 2f - HexMetrics.OUTER_RADIUS/2 * z;
        //position *= CellSizeMultiplicator;

        Cell cell = Instantiate(CellPrefab);
        cell.coordinates = new HexCoordinates(x, z);
        cell.transform.SetParent(HexGrid.transform, false);
        cell.transform.localPosition = position;
        Cells.Add(cell);

        Text label = Instantiate(Label, GridCanvas.transform);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        label.transform.position = position + Vector3.up;
    }
    private void SetCellsNeighbours()
    {
       
        foreach (Cell cell in Cells)
        {
            cell.Neighbors = new List<Cell>();
            cell.Neighbors = (from c in Cells
                              where (c.coordinates.X == cell.coordinates.X && Mathf.Abs(c.coordinates.Y - cell.coordinates.Y) == 1)||
                              (c.coordinates.Y == cell.coordinates.Y && Mathf.Abs(c.coordinates.X - cell.coordinates.X) == 1)||
                              (c.coordinates.Z == cell.coordinates.Z && Mathf.Abs(c.coordinates.X - cell.coordinates.X) == 1)
                              select c).ToList();
        }
    }
}
