using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexGrid : MonoBehaviour
{
	public MapGenerator generator;
	public HexCoordinates HexCoordintaes;


	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			TouchCell(hit.point);
		}
	}

	void TouchCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		//CalculateLine(coordinates);
		CalculateReach();
	}
	private void CalculateReach()
    {
		int movement = 4;
		List<Cell> visited = new List<Cell>();
		visited.Add(GetCellByCoordintes(HexCoordintaes));

		List<Cell> borders = new List<Cell>();
		borders.Add(GetCellByCoordintes(HexCoordintaes));

		for (int i = 0; i < movement; i++)
        {
			List<Cell> newBorders = new List<Cell>();
			if(borders.Count < 1)
            {
				break;
            }
			foreach(Cell cell in borders)
            {
				foreach(Cell borderCell in cell.Neighbors)
                {
					if(!visited.Contains(borderCell) && borderCell.IsReachable)
                    {
						visited.Add(borderCell);
						newBorders.Add(borderCell);

					}
                }
            }
			borders = newBorders;

		}

		foreach(Cell cell in visited)
        {
			cell.Higlight();
        }



	}
	private void CalculateLine(HexCoordinates coordinates)
    {
		if (coordinates == HexCoordintaes)
        {
			return;
        }
		int N = CubeDistance(HexCoordintaes, coordinates);

		for (int i = 0; i <= N; i++)
        {
			Vector3 vector = new Vector3();
			vector.x = HexCoordintaes.X + (coordinates.X - HexCoordintaes.X) * 1.0f / N * i;
			vector.y = HexCoordintaes.Y + (coordinates.Y - HexCoordintaes.Y) * 1.0f / N * i;
			vector.z = -vector.x - vector.y;

			GetCellByCoordintes(HexCoordinates.RoundHex(vector)).Higlight();
		}

	}
	public Cell GetCellByCoordintes(HexCoordinates coordinates)
    {
		Cell cell;
		cell = (from c in generator.Cells
				where (c.coordinates == coordinates)
				select c).First();

		return cell;
    }
	public Vector3Int CubeSubstract(HexCoordinates a, HexCoordinates b) 
	{
		return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public int CubeDistance(HexCoordinates a, HexCoordinates b) 
	{

		Vector3Int vector = CubeSubstract(a, b);

		return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	    // or: max(abs(a.q - b.q), abs(a.r - b.r), abs(a.s - b.s))
	}
}
