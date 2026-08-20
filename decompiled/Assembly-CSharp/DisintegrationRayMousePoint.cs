using Unity.Entities;
using Unity.Mathematics;

public struct DisintegrationRayMousePoint : IBufferElementData
{
	public float2 Value;

	public float2 Dir;

	public float2 V;
}
