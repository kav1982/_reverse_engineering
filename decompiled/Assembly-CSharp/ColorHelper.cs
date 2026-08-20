using Unity.Mathematics;
using UnityEngine;

public static class ColorHelper
{
	public static float4 ToFloat4(this Color color)
	{
		return new float4(color.r, color.g, color.b, color.a);
	}

	public static float3 GetFloat3(this in Color color)
	{
		return new float3(color.r, color.g, color.b);
	}
}
