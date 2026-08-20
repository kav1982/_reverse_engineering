using Unity.Entities;
using Unity.Mathematics;

public struct Monster321ExplosionData : IBufferElementData
{
	public float BaseDamage;

	public float DamageRange;

	public float3 CenterPoint;

	public float DelayExplosionDuration;

	public float Timer;

	public bool IsInitialized;
}
