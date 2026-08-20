using Unity.Entities;
using Unity.Mathematics;

public struct SnakeTouchGroundPoint : IBufferElementData
{
	public float3 Value;

	public float distanceToHead;

	public float currentDamageLoopTime;
}
