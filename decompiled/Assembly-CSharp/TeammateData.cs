using System.Runtime.InteropServices;
using Unity.Entities;

public struct TeammateData : IComponentData, IQueryTypeParameter
{
	public TeammateType TeammateType;

	public int TeammateId;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public float TeammateSpeedRatio;

	public int AdvanceSkillLevel;

	public AttributeValue TeammateHpRatio;

	public float TeammateHpRecoverAmountPerSecond;

	public float TeammateHpDropAmountPerSecond;

	public float TeammateHpEffectCalculateTimer;

	public float TeammateSuddenDeathHPThreshold;

	[MarshalAs(UnmanagedType.U1)]
	public bool SeparateCalculateHpEffect;

	public int OnDeathSpawnWormCount;

	public float LifeLineDamage;

	public float ExplodeRange;

	public float ExplodeHpDamageRatio;

	public float SpellSummonGainOwnerHpRatio;

	public float SummonFollowOwnerThroughMapChance;

	public int TeammateCurrentFuseLevel;

	public int TeammateMaxFuseLevel;

	public float TeammateDelayDeathTime;

	[MarshalAs(UnmanagedType.U1)]
	public bool TeammateDelayDeathEffectActive;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFuseMaterial;

	[MarshalAs(UnmanagedType.U1)]
	public bool Born1Hp;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsHoldByTeammate6;

	public bool IsFuseTeammate => TeammateCurrentFuseLevel > 0;
}
