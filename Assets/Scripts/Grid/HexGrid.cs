using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexGrid : MonoBehaviour
{
	//public MapGenerator generator;
	public List<Cell> Cells;
	public HexCoordinates HexCoordintaes;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//HandleInput();
		}
	}


    public void SetGrid(List<Cell> cells)
    {
		Cells = cells;
	}
	
	
	public List<Cell> CalculatePath(Cell startCell, Cell targetCell, bool includeTargetUnit = false)
    {
		Heap<Cell> openSet = new Heap<Cell>(Cells.Count);
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
                
				return RetracePath(startCell, targetCell);
				//return;
            }

			foreach(Cell neighbour in currentCell.Neighbors)
            {
				bool neighbourGroundUnit = false;
				if (includeTargetUnit)
				{
					neighbourGroundUnit = (neighbour.GroundUnit && neighbour != targetCell);
				}
				else
                {
					neighbourGroundUnit = neighbour.GroundUnit;
				}
				if (!neighbour.IsReachable || closedSet.Contains(neighbour) || neighbourGroundUnit)
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
		return null;
	}
	public List<Cell> CalculatePath(Cell startCell, Cell targetCell, int movement, bool includeTargetUnit = false)
	{
		List<Cell> path = CalculatePath(startCell, targetCell, includeTargetUnit);
		List<Cell> newPath = new List<Cell>();
        foreach(Cell cell in path)
        {
			if(movement - cell.movementCost < 0)
            {
				break;
            }
			movement -= cell.movementCost;
			newPath.Add(cell);

		}
		return newPath;
	}

	private List<Cell> RetracePath(Cell startCell, Cell endCell)
    {
		List<Cell> path = new List<Cell>();
		Cell currentCell = endCell;
		while(currentCell != startCell)
        {
			path.Add(currentCell);
			currentCell = currentCell.parent;
        }

		return path;
    }
	public void ClearGrid()
    {
		foreach (Cell cell in Cells)
		{
			cell.SetAsNormal();
		}
	}
    
	public List<Cell> CalculateReach(Cell startCell, int movement, bool veighed = true, bool countObstacles = true)
	{
		//ClearGrid();
		Dictionary<Cell, Cell> visitedCells = new Dictionary<Cell, Cell>();
		Dictionary<Cell, int> costSoFar = new Dictionary<Cell, int>();
		Queue<Cell> nodesToVisitQueue = new Queue<Cell>();

		nodesToVisitQueue.Enqueue(startCell);
		costSoFar.Add(startCell, 0);

		while(nodesToVisitQueue.Count > 0)
        {
			Cell currentNode = nodesToVisitQueue.Dequeue();
			foreach (Cell neighbour in currentNode.Neighbors)
			{
				if (countObstacles && (!neighbour.IsReachable || neighbour.GroundUnit))
				{
					continue;
				}
				int nodeCost = 1;
				if(veighed)
                {
					nodeCost = neighbour.movementCost;
				}
				int currentCost = costSoFar[currentNode];
				int newCost = currentCost + nodeCost;

				if (newCost <= movement)
                {
					if (!visitedCells.ContainsKey(neighbour))
                    {
						visitedCells[neighbour] = currentNode;
						costSoFar[neighbour] = newCost;
						nodesToVisitQueue.Enqueue(neighbour);
                    }
					else if(costSoFar[neighbour] > newCost)
                    {
						costSoFar[neighbour] = newCost;
						visitedCells[neighbour] = currentNode;
					}
                }
			}
        }
		return visitedCells.Keys.ToList();
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
		cell = (from c in Cells
				where (c.coordinates == coordinates)
				select c).First();

		return cell;
    }
	public static Vector3Int CubeSubstract(HexCoordinates a, HexCoordinates b) 
	{
		return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static int CubeDistance(Cell a, Cell b)
    {
		return CubeDistance(a.coordinates, b.coordinates);
    }
	public static int CubeDistance(HexCoordinates a, HexCoordinates b) 
	{

		Vector3Int vector = CubeSubstract(a, b);

		return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	    // or: max(abs(a.q - b.q), abs(a.r - b.r), abs(a.s - b.s))
	}
}
