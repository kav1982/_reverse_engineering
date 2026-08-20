using Unity.Entities;
using Unity.Mathematics;

public struct Spell1006GhostFireData : IComponentData, IQueryTypeParameter
{
	public float SelfMimicTimer;

	public float SelfMimicInterval;

	public float MinSpeed;

	public float InitialSpeed;

	public bool IsInitialize;

	public float3 PullForceByOtherGhostFire;
}
