using Unity.Mathematics;
using UnityEngine;

public static class Float3Helper
{
	public static Vector3 GetVector3(this in float3 f3)
	{
		return new Vector3(f3.x, f3.y, f3.z);
	}

	public static Vector2 GetVector2(this in float3 f3)
	{
		return new Vector2(f3.x, f3.y);
	}

	public static float3 IgnoreZ(this in float3 input, float z = 0f)
	{
		return new float3(input.x, input.y, z);
	}
}
