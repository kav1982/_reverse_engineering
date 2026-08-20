using Unity.Entities;
using Unity.Mathematics;

public struct FuseHeadEntity : IBufferElementData
{
	public Entity Entity;

	public Entity LegsRoot;

	public float3 HeadPos;
}
