using Unity.Entities;

public struct TeammateFuseRequestBuffer : IBufferElementData
{
	public TeammateData TeammateData;

	public Entity TeammateEntity;

	public Entity OwnerUnit;

	public int ChunkIndex;
}
