using Unity.Entities;

public struct Spell2005GrimoireData : IComponentData, IQueryTypeParameter
{
	public float ManaRegenPerSecond;

	public float MaxMpCapacity;

	public float CurrentMp;

	public float AttackDuration;

	public float AttackTimer;

	public float AttackRange;

	public float AnimationTimer;

	public float BookFloatingTimer;

	public float CurrentBaseHeight;

	public float BookFloatingHeight;

	public bool IsRotation;

	public bool IsLowCostSpell;

	public bool ReadyToAttack;

	public float ShootRecoil;

	public float CloseBookTimer;

	public float UpdateChaseTargetTimer;

	public int SpellCastCounter;

	public Spell2005State State;

	public bool IsChildTeammateReachLimit;

	public bool ReleaseChargeSpell;

	public float ReleaseChargeDuration;

	public float ReleaseChargeTimer;

	public float TeleportCoolDownTimer;

	public float TeleportProgressTimer;

	public bool Teleporting;

	public bool TeleportDone;
}
