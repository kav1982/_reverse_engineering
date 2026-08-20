using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct GetGOByJobBED : IBufferElementData
{
	public FixedString128Bytes path;

	public float3 worldPos;

	public float duration;
}
