using Unity.Entities;
using Unity.Mathematics;

public struct Monster312TpBuffer : IBufferElementData
{
	public float3 targetPosition;

	public Entity selfEntity;

	public Entity targetEntity;

	public bool flip;

	public bool pattern2;
}
