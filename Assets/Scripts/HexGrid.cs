using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexGrid : MonoBehaviour
{
	public MapGenerator generator;
	public HexCoordinates HexCoordintaes;

	public int Movement;
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

		CalculatePath(GetCellByCoordintes(HexCoordintaes), GetCellByCoordintes(coordinates));
		//CalculateLine(coordinates);
		//CalculateReach();
	}

	private void CalculatePath(Cell startCell, Cell targetCell)
    {
		Heap<Cell> openSet = new Heap<Cell>(generator.Cells.Count);
		HashSet<Cell> closedSet = new HashSet<Cell>();
		openSet.Add(startCell);

		while(openSet.Count > 0)
        {
			Cell currentCell = openSet.RemoveFirst();
			/*for(int i = 0; i < openSet.Count; i++)
            {
				if(openSet[i].FCost < currentCell.FCost || openSet[i].FCost == currentCell.FCost && openSet[i].HCost < currentCell.HCost)
                {
					currentCell = openSet[i];
                }
            }
			openSet.Remove(currentCell);*/
			closedSet.Add(currentCell);

			if (currentCell == targetCell)
            {
				RetracePath(startCell, targetCell);
				return;
            }

			foreach(Cell neighbour in currentCell.Neighbors)
            {
				if(!neighbour.IsReachable || closedSet.Contains(neighbour))
                {
					continue;
                }

				int newMovementCostToNeighbour = currentCell.GCost + CubeDistance(currentCell, neighbour) + neighbour.movementCost;
				if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
				{
					neighbour.GCost = newMovementCostToNeighbour;
					neighbour.HCost = CubeDistance(neighbour, targetCell);
					neighbour.parent = currentCell;

                    if (!openSet.Contains(neighbour))
                    {
						openSet.Add(neighbour);
                    }
					else
                    {
						openSet.UpdateItem(neighbour);
                    }
                }
            }
			
		}
	}
	private void RetracePath(Cell startCell, Cell endCell)
    {
		List<Cell> path = new List<Cell>();
		Cell currentCell = endCell;
		while(currentCell != startCell)
        {
			path.Add(currentCell);
			currentCell = currentCell.parent;
        }	
		
		foreach(Cell cell in path)
        {
			cell.Higlight();
        }
    }
	//GetNeighbours;
	private void CalculateReach()
    {
		List<Cell> visited = new List<Cell>();
		visited.Add(GetCellByCoordintes(HexCoordintaes));

		List<Cell> borders = new List<Cell>();
		borders.Add(GetCellByCoordintes(HexCoordintaes));

		for (int i = 0; i < Movement; i++)
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

	public int CubeDistance(Cell a, Cell b)
    {
		return CubeDistance(a.coordinates, b.coordinates);
    }
	public int CubeDistance(HexCoordinates a, HexCoordinates b) 
	{

		Vector3Int vector = CubeSubstract(a, b);

		return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	    // or: max(abs(a.q - b.q), abs(a.r - b.r), abs(a.s - b.s))
	}
}
