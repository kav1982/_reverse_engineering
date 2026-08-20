using Unity.Mathematics;
using UnityEngine;

public struct Vector2Data
{
	public float x;

	public float y;

	public static Vector2Data Zero => new Vector2Data(0f, 0f);

	public static Vector2Data Up1000 => new Vector2Data(0f, 1000f);

	public Vector2Data(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public Vector3 GetVector3()
	{
		return new Vector3(x, y, 0f);
	}

	public readonly float3 GetFloat3()
	{
		return new float3(x, y, 0f);
	}

	public static bool operator ==(Vector2Data v1, Vector2Data v2)
	{
		if (v1.x == v2.x)
		{
			return v1.y == v2.y;
		}
		return false;
	}

	public static bool operator !=(Vector2Data v1, Vector2Data v2)
	{
		if (v1.x == v2.x)
		{
			return v1.y != v2.y;
		}
		return true;
	}

	public static Vector2Data operator +(Vector2Data v1, Vector2Data v2)
	{
		return new Vector2Data(v1.x + v2.x, v1.y + v2.y);
	}

	public static Vector2Data operator -(Vector2Data v1, Vector2Data v2)
	{
		return new Vector2Data(v1.x - v2.x, v1.y - v2.y);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static Vector2Data Convert(Vector3 v3)
	{
		return new Vector2Data(v3.x, v3.y);
	}
}
