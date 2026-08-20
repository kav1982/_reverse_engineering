using Unity.Entities;
using Unity.Mathematics;

public struct SnakeBodyPoint : IBufferElementData
{
	public float3 Value;

	public float distance;
}
