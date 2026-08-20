using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct DynamicOptimizeData : IComponentData, IQueryTypeParameter
{
	public float CurrentFPS;

	[MarshalAs(UnmanagedType.U1)]
	public bool EnableLowFPSOptimize;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsMobilePlatform;

	public float LastFrameTimeScale;

	public float PoolEffectShowRatio;

	public bool IsLowFpsOptimizeActive(float activeFPSThreshold)
	{
		if (EnableLowFPSOptimize)
		{
			return CurrentFPS <= activeFPSThreshold;
		}
		return false;
	}

	public float GetLowFrameDamageIntervalTimeScale(float startOptimizeFPS, float maxOptimizeFPS, float maxTimeScale)
	{
		if (!IsLowFpsOptimizeActive(startOptimizeFPS) || startOptimizeFPS < 0f || maxOptimizeFPS < 0f || maxTimeScale <= 1f || startOptimizeFPS <= maxOptimizeFPS)
		{
			return 1f;
		}
		if (CurrentFPS <= maxOptimizeFPS)
		{
			return maxTimeScale;
		}
		return math.lerp(1f, maxTimeScale, math.clamp(1f - (CurrentFPS - maxOptimizeFPS) / (startOptimizeFPS - maxOptimizeFPS), 0f, 1f));
	}
}
