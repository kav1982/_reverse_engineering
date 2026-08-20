using Unity.Entities;
using Unity.Mathematics;

public struct MagicShieldDmgEvent : IBufferElementData
{
	public float3 Position;

	public float damage;

	public float radius;
}
