using Unity.Entities;

[InternalBufferCapacity(6)]
public struct SpellHitEntity : IBufferElementData
{
	public Entity Entity;
}
