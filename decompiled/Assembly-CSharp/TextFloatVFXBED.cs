using Unity.Entities;
using Unity.Mathematics;

public struct TextFloatVFXBED : IBufferElementData
{
	public float number;

	public UITextFloatType type;

	public float3 worldPos;
}
