using Unity.Entities;

public struct SpellElementEffectComponentData : IComponentData, IQueryTypeParameter
{
	public float VenomDuration;

	public float VenomApplyCount;

	public float MucusMoveSpeedRatio;

	public float MucusSpellSpeedRatio;

	public float MucusDuration;

	public float FireHpBurnPercent;

	public float FireBurnDuration;

	public float FrozenDuration;

	public float VoidInstantKillThreshold;

	public float VoidExplosionHpDamageRatio;

	public float VoidExplosionRange;

	public float ThunderHitRadius;

	public float ThunderHitDamageRatio;
}
