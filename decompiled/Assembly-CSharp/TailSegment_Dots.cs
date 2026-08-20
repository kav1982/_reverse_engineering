using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(8)]
public struct TailSegment_Dots : IBufferElementData
{
	public Entity Entity;

	public Entity ShadowEntity;

	public float3 LogicPosition;
}
