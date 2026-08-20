using Unity.Entities;
using Unity.Mathematics;

public struct SummonAuraBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public float spawnScale;
}
