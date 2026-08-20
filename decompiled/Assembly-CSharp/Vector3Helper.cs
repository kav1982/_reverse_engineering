using Unity.Mathematics;
using UnityEngine;

public static class Vector3Helper
{
	public static float3 GetFloat3(this in Vector3 v3)
	{
		return new float3(v3.x, v3.y, v3.z);
	}
}
