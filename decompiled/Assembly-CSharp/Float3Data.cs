using Unity.Mathematics;

public struct Float3Data
{
	public float x;

	public float y;

	public float z;

	public static Float3Data zero => new Float3Data(0f, 0f, 0f);

	public Float3Data(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static Float3Data operator +(Float3Data a, Float3Data b)
	{
		return new Float3Data(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static float3 operator +(Float3Data a, float3 b)
	{
		return new float3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static float3 operator +(float3 a, Float3Data b)
	{
		return new float3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Float3Data operator -(Float3Data a, Float3Data b)
	{
		return new Float3Data(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static float3 operator -(Float3Data a, float3 b)
	{
		return new float3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static float3 operator -(float3 a, Float3Data b)
	{
		return new float3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Float3Data operator *(Float3Data a, float scalar)
	{
		return new Float3Data(a.x * scalar, a.y * scalar, a.z * scalar);
	}

	public static Float3Data operator *(float scalar, Float3Data a)
	{
		return a * scalar;
	}

	public static Float3Data operator /(Float3Data a, float scalar)
	{
		return new Float3Data(a.x / scalar, a.y / scalar, a.z / scalar);
	}

	public static Float3Data operator *(Float3Data a, Float3Data b)
	{
		return new Float3Data(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Float3Data operator /(Float3Data a, Float3Data b)
	{
		return new Float3Data(a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public override string ToString()
	{
		return $"({x}, {y}, {z})";
	}
}
