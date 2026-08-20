using UnityEngine;

public struct Vector3Data
{
	public float x;

	public float y;

	public float z;

	public static Vector3Data Zero => new Vector3Data(0f, 0f, 0f);

	public Vector3Data(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3 GetVector3()
	{
		return new Vector3(x, y, z);
	}

	public static bool operator ==(Vector3Data v1, Vector3Data v2)
	{
		if (v1.x == v2.x && v1.y == v2.y)
		{
			return v1.z == v2.z;
		}
		return false;
	}

	public static bool operator !=(Vector3Data v1, Vector3Data v2)
	{
		if (v1.x == v2.x && v1.y == v2.y)
		{
			return v1.z != v2.z;
		}
		return true;
	}

	public static Vector3Data operator +(Vector3Data v1, Vector3Data v2)
	{
		return new Vector3Data(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
	}

	public static Vector3Data operator -(Vector3Data v1, Vector3Data v2)
	{
		return new Vector3Data(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static Vector3Data Convert(Vector3 v3)
	{
		return new Vector3Data(v3.x, v3.y, v3.z);
	}
}
