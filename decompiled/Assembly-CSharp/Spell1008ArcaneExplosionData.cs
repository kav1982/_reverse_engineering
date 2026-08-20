using Unity.Entities;

public struct Spell1008ArcaneExplosionData : IComponentData, IQueryTypeParameter
{
	public bool IsFullRangeDamage;

	public float HoverDamageTimer;

	public bool ReadyToDestroySelf;
}
