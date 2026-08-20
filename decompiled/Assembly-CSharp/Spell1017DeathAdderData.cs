using Unity.Entities;
using Unity.Mathematics;

public struct Spell1017DeathAdderData : IComponentData, IQueryTypeParameter
{
	public bool InitOver;

	public float3 BeginPosition;

	public float3 BoomPosition;

	public float RebondTimer;

	public Entity EffectEntity;
}
