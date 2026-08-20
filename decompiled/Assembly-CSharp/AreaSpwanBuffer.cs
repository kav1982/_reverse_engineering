using Unity.Entities;
using Unity.Mathematics;

public struct AreaSpwanBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public Entity attackerEntity;
}
