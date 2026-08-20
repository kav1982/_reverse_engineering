using Unity.Entities;
using Unity.Mathematics;

public struct TeammateDeadBloodEffectBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public float spawnScale;
}
