using Unity.Entities;
using Unity.Mathematics;

public struct Teammate3InvisibleTentacleSpawnerData : IBufferElementData
{
	public float3 CurrentPosition;

	public float3 Direction;

	public float Speed;

	public float RemainDuration;

	public Entity Shooter;

	public float SpawnTimer;

	public Entity Target;

	public float RotateAngle;
}
