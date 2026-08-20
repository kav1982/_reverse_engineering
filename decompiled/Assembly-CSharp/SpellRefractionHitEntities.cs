using Unity.Entities;

[InternalBufferCapacity(8)]
public struct SpellRefractionHitEntities : IBufferElementData
{
	public Entity Entity;
}
