using JetBrains.Annotations;
using UnityEngine;

public class TakeDamageInfo
{
	public AttackerType attackerType;

	public UnitProperty attackerPpt;

	public UnitProperty beHitPpt;

	public bool considerPlayerInInvincibleFrame = true;

	public bool considerUmbrella = true;

	public bool considerRelicDodge = true;

	public bool considerRelicOrCurseDamageRatioChange = true;

	public float damage;

	public float criticalChance;

	public bool isCriticalDamage;

	public bool isPlayHitSE = true;

	public bool isPlayDeadSE = true;

	public bool isCreateDeadEF = true;

	public bool isCreatebloodSplat = true;

	public bool isTargetDead;

	public bool immuneDamage;

	public bool beHitColor = true;

	public bool beHitShake = true;

	public bool isTrapDamage;

	public bool isFloatText = true;

	public bool canRebound = true;

	public bool stopAnnouncedDeath;

	public SpellBase spellBase;

	public Vector3 knockbackForce = Vector3.zero;

	[CanBeNull]
	public WandPostSlotChargeData wandChargeData;

	public bool isUndifferDamage;

	public float playerTakeDamageRatio = 1f;

	public float teammateTakeDamageRatio = 1f;

	public bool postSlotSpellTakeDamageTrigger = true;

	public bool isTeammateThrough;

	public bool isPercentageDamage;
}
