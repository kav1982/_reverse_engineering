using Unity.Entities;

public struct TeammateFusePairBuffer : IBufferElementData
{
	public Entity FuseMainTeammateEntity;

	public Entity FuseSubTeammateEntity;

	public TeammateData FuseMainTeammateData;

	public TeammateData FuseSubTeammateData;
}
