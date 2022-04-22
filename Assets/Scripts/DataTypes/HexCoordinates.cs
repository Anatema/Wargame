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
	public static bool operator ==(HexCoordinates c1, HexCoordinates c2)
	{
		return c1.X == c2.X && c1.Z == c2.Z;
	}

	public static bool operator !=(HexCoordinates c1, HexCoordinates c2)
	{
		return !(c1.X == c2.X && c1.Z == c2.Z);
	}

	public override string ToString()
	{
		return $"(x: {X}, y: {Y}, z: {Z})";
	}
	public string ToStringOnSeparateLines()
	{
		return $"{X},\n {Y},\n {Z}";
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

    public override bool Equals(object obj)
    {
		return obj is HexCoordinates coordinates &&
			   _x == coordinates._x &&
			   _z == coordinates._z;
    }

    public override int GetHashCode()
    {
        int hashCode = 1239827070;
        hashCode = hashCode * -1521134295 + _x.GetHashCode();
        hashCode = hashCode * -1521134295 + _z.GetHashCode();
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Z.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }


	public static HexCoordinates operator +(HexCoordinates first, HexCoordinates second)
	{
		int x = first.X + second.X;
		int z = first.Z + second.Z;
        return new HexCoordinates 
		{
			_x = first.X + second.X,
			_z = first.Z + second.Z,
		};

    }
}
