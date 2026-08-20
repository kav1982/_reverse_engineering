using Unity.Entities;
using Unity.Mathematics;

public struct Spell1018ThunderAuraData : IComponentData, IQueryTypeParameter
{
	public float FallDelayTimer;

	public float3 FallPosition;

	public float OriginalSpeed;
}
