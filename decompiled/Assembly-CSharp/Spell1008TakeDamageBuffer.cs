using Unity.Entities;
using Unity.Mathematics;

public struct Spell1008TakeDamageBuffer : IBufferElementData
{
	public Entity EffectEntity;

	public float3 HitPosition;

	public bool IsFullRangeDamage;
}
