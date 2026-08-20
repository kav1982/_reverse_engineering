using Unity.Entities;
using Unity.Mathematics;

public struct CreateBloodSplatRequest : IBufferElementData
{
	public float3 point;

	public float rotationZ;

	public bool directional;

	public float size;
}
