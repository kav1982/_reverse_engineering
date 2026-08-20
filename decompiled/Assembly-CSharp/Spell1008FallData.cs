using Unity.Entities;

public struct Spell1008FallData : IComponentData, IQueryTypeParameter
{
	public bool FinishDamageApply;

	public bool IsFullRangeDamage;

	public float HoverDamageTimer;

	public float HoverTimer;

	public float Radius;

	public bool IsVoidColor;

	public Entity SpellEntity;

	public float HoverDuration;

	public float DurationTimer;
}
