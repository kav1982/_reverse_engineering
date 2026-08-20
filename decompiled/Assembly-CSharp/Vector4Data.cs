using UnityEngine;

public struct Vector4Data
{
	public float x;

	public float y;

	public float z;

	public float w;

	public static Vector4Data One { get; } = new Vector4Data(1f, 1f, 1f, 1f);


	public static Vector4Data Zero { get; } = new Vector4Data(0f, 0f, 0f, 0f);


	public Vector4Data(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public Color GetColor()
	{
		return new Color(x, y, z, w);
	}

	public static bool operator ==(Vector4Data v1, Vector4Data v2)
	{
		if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
		{
			return v1.w == v2.w;
		}
		return false;
	}

	public static bool operator !=(Vector4Data v1, Vector4Data v2)
	{
		if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
		{
			return v1.w != v2.w;
		}
		return true;
	}

	public static Vector4Data operator +(Vector4Data v1, Vector4Data v2)
	{
		return new Vector4Data(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
	}

	public static Vector4Data operator -(Vector4Data v1, Vector4Data v2)
	{
		return new Vector4Data(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static Vector4Data Convert(Color color)
	{
		return new Vector4Data(color.r, color.g, color.b, color.a);
	}
}
