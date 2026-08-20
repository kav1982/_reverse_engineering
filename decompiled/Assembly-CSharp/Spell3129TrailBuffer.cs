using Unity.Entities;
using Unity.Mathematics;

public struct Spell3129TrailBuffer : IBufferElementData
{
	public float3 StartPos;

	public float Duration;

	public Entity TargetEntity;
}
