using Unity.Entities;
using Unity.Mathematics;

public struct HarpoonChainData : IBufferElementData
{
	public float3 Position;

	public float3 PrevPosition;
}
