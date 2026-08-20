using UnityEngine;

public static class Vector2IntHelper
{
	public static Vector3 GetVector3(this Vector2Int vector2)
	{
		return new Vector3(vector2.x, vector2.y, 0f);
	}

	public static Vector3 GetVector3(this Vector2 vector2)
	{
		return new Vector3(vector2.x, vector2.y, 0f);
	}
}
