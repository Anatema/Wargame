using UnityEngine;

public static class HexMetrics
{
    public const float OUTER_RADIUS = 10f;
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f;

	public static Vector3[] Corners = {
		new Vector3(0f, 0f, OUTER_RADIUS),
		new Vector3(INNER_RADIUS, 0f, 0.5f * OUTER_RADIUS),
		new Vector3(INNER_RADIUS, 0f, -0.5f * OUTER_RADIUS),
		new Vector3(0f, 0f, -OUTER_RADIUS),
		new Vector3(-INNER_RADIUS, 0f, -0.5f * OUTER_RADIUS),
		new Vector3(-INNER_RADIUS, 0f, 0.5f * OUTER_RADIUS)
	};

	public static HexCoordinates[] NeighboursDirections = new HexCoordinates[6]
	{
		new HexCoordinates(+1, 0),
		new HexCoordinates(+1, -1),
		new HexCoordinates(0, -1),
		new HexCoordinates(-1, 0),
		new HexCoordinates(-1, +1),
		new HexCoordinates(0, +1)
	};
}
