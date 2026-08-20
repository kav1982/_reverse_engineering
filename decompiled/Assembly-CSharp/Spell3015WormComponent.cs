using Unity.Entities;
using Unity.Mathematics;

public struct Spell3015WormComponent : IComponentData, IQueryTypeParameter
{
	public float randomMoveTimer;

	public float idleTimer;

	public float3 randomMoveTargetPoint;

	public Entity meshEntity;

	public Spell3015WormSpawnBuffer wormInfo;

	public TakeDamageInfo_Dots damageInfo;

	public float exitTimer;

	public float checkIntervalTimer;

	public Entity nearestTarget;

	public UnitProperty_Dots unitProperty_Dots;
}
