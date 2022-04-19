using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField, Min(0)]
    public int GridRadius = 0;
    //public float CellSizeMultiplicator;

    public Cell CellPrefab;
    public Text CellLabelPrefab;
    private List<Cell> _cells;

    public HexGrid HexGrid;
    public Canvas GridCanvas;


    public void GenerateGrid()
    {
        ClearGrid();
        
        
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
        position.x = (x * HexMetrics.INNER_RADIUS * 2f + y * HexMetrics.INNER_RADIUS);
        position.z = y * HexMetrics.OUTER_RADIUS * 2f - HexMetrics.OUTER_RADIUS/2 * y;
        //position *= CellSizeMultiplicator;


        //position.x = (x + z * 0.5f - z / 2) * (HexMetrics.INNER_RADIUS * 2f);
        /*position.x = (x + z * 0.5f - z / 2) * (HexMetrics.INNER_RADIUS * 2f);
        //position.z = z * (HexMetrics.OUTER_RADIUS * 1.5f);*/


        Cell cell = Instantiate(CellPrefab);
        cell.coordinates = new HexCoordinates(x, z);
        cell.transform.SetParent(HexGrid.transform, false);
        cell.transform.localPosition = position;
    }
}
