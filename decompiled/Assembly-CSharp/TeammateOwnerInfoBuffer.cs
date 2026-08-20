using Unity.Entities;

[InternalBufferCapacity(0)]
public struct TeammateOwnerInfoBuffer : IBufferElementData
{
	public TeammateType TeammateType;

	public Entity TeammateEntity;
}
