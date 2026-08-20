using Unity.Entities;
using Unity.Mathematics;

public struct Teammate3SplitTentacleSpawnerData : IBufferElementData
{
	public float3 TargetPosition;

	public Entity Shooter;

	public float SpawnDelayTimer;

	public int SplitCount;
}
