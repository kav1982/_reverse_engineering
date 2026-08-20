using Unity.Entities;

public struct Spell4013HitTriggerData : IComponentData, IQueryTypeParameter
{
	public float DamageTimer;

	public Entity Spell;

	public Entity Parent;

	public float ThunderDamageTimer;
}
