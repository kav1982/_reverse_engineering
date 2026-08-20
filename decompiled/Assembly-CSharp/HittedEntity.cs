using Unity.Entities;
using Unity.Mathematics;

public struct HittedEntity : IBufferElementData
{
	public Entity Entity;

	public float3 Direction;
}
