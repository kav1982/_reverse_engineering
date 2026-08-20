using Unity.Entities;

public struct SpellHoverDamageData : IComponentData, IQueryTypeParameter
{
	public float AttackTimer;

	public float Interval;

	public bool ShowHitEffect;
}
