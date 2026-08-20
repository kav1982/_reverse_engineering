using Unity.Entities;
using Unity.Mathematics;

public struct UITextFloatByJobBED : IBufferElementData
{
	public int textID;

	public float number;

	public UITextFloatType type;

	public float3 worldPos;
}
