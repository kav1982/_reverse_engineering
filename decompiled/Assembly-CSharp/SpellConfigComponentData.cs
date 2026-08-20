using Unity.Entities;

public struct SpellConfigComponentData : IComponentData, IQueryTypeParameter
{
	public SpellAbilityType AbilityType;

	public SpellColorType ColorType;

	public UnitType ShooterType;

	public int Id;

	public int Level;

	public AttributeValue Damage;

	public float UndifferDamageRatio;

	public float MaxUndifferDamageReceive;

	public float CriticalChance;

	public float Knockback;

	public RadiusAttributeValue Radius;

	public PenetrateValue Penetrate;

	public float Scatter;

	public AttributeValue Duration;

	public float DurationTimer;

	public float DamageInterval;

	public float DamageTimer;

	public float GravitationalAttackRange;

	public float GravitationalAttackPullForce;

	public int GravitationalMaxApplyCount;

	public float LightningChainDamage;

	public float HoverDuration;

	public float HoverTimer;

	public float Float1;

	public float Float2;

	public float Float3;

	public int Int1;

	public int Int2;

	public int Int3;

	public bool IsTeammate
	{
		get
		{
			int id = Id;
			if (id >= 20000)
			{
				return id < 30000;
			}
			return false;
		}
	}
}
