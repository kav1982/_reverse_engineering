using Unity.Entities;
using Unity.Mathematics;

public struct Spell1028MrBingSubArrowEmitterData : IComponentData, IQueryTypeParameter
{
	public float subEmitTimer;

	public int remainSubArrowCount;

	public float3 shootDirection;

	public SpellSpawnParamsStorage spellSpawnParamsStorage;
}
