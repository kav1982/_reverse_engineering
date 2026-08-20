using Unity.Entities;
using Unity.Mathematics;

public struct SummonSoulmateAuraBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public float spawnScale;
}
