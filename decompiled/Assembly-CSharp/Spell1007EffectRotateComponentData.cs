using Unity.Entities;

[UpdateInGroup(typeof(SpellEffectSystemGroup))]
public struct Spell1007EffectRotateComponentData : IComponentData, IQueryTypeParameter
{
	public float Speed;
}
