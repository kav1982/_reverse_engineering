using Unity.Entities;
using Unity.Mathematics;

public struct Spell1016DirverCleanupData : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public Entity Dirver;

	public float DashTimer;

	public float3 LastPosition;

	public SpellColorType ColorType;

	public float3 LastLinear;

	public float Radius;
}
