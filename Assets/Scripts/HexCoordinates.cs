using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
	[SerializeField]
	private int _x, _z;
	public int X => _x;
	public int Z => _z;
	public int Y
	{
		get {
			return -X - Z;
		}
	}
	public HexCoordinates(int x, int z)
	{
		_x = x;
		_z = z;
	}
	
	public override string ToString()
	{
		return $"(x: {X.ToString()}, y: {Y.ToString()}, z: {Z.ToString()})";
	}
	public string ToStringOnSeparateLines()
	{
		return $"{X.ToString()},\n {Y.ToString()},\n {Z.ToString()}";
	}
	public static HexCoordinates RoundHex(Vector3 position)
    {
		int iX = Mathf.RoundToInt(position.x);
		int iY = Mathf.RoundToInt(position.y);
		int iZ = Mathf.RoundToInt(-position.x - position.y);

		if (iX + iY + iZ != 0)
		{
			float dX = Mathf.Abs(position.x - iX);
			float dY = Mathf.Abs(position.y - iY);
			float dZ = Mathf.Abs(-position.x - position.y - iZ);

			if (dX > dY && dX > dZ)
			{
				iX = -iY - iZ;
			}
			else if (dZ > dY)
			{
				iZ = -iX - iY;
			}
		}

		return new HexCoordinates(iX, iZ);
	}
	public static HexCoordinates FromPosition(Vector3 position)
	{
		float x = position.x / (HexMetrics.INNER_RADIUS * 2f);
		float y = -x;
		float offset = position.z / (HexMetrics.OUTER_RADIUS * 3f);

		x -= offset;
		y -= offset;

		return RoundHex(new Vector3(x, y, -x - y));		
	}



}
