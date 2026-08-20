using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct TakeDamageInfo_Dots : IBufferElementData
{
	public struct SpellData
	{
		public float3 HitPosition;

		public Entity Entity;

		[MarshalAs(UnmanagedType.U1)]
		public bool IgnoreHitEffect;

		[MarshalAs(UnmanagedType.U1)]
		public bool CostPenetrate;

		[MarshalAs(UnmanagedType.U1)]
		public bool CostRefraction;

		public LocalTransform Transform;

		public SpellConfigComponentData Config;

		public SpellMovementComponentData Movement;

		public SpellElementEffectComponentData ElementEffect;
	}

	public AttackerType attackerType;

	public SpellData spell;

	public Entity attackerEntity;

	public Entity targetEntity;

	[MarshalAs(UnmanagedType.U1)]
	public bool isTargetDead;

	[MarshalAs(UnmanagedType.U1)]
	public bool isTriggerDeadEvent;

	[MarshalAs(UnmanagedType.U1)]
	public bool isDamageCritical;

	[MarshalAs(UnmanagedType.U1)]
	public bool targetAlreadyDeadBeforeDamage;

	[MarshalAs(UnmanagedType.U1)]
	public bool immuneDamage;

	public float damage;

	public float realDamage;

	public float hitMagicShieldDamage;

	public float3 beHitShakeDir;

	public float3 knockbackForce;

	[MarshalAs(UnmanagedType.U1)]
	public bool dontCreateDeadEF;

	[MarshalAs(UnmanagedType.U1)]
	public bool dontCreatebloodSplat;

	[MarshalAs(UnmanagedType.U1)]
	public bool dontPlayDeadSE;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignorePlayerInvincibleFrame;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreRelicDodge;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreRelicSeckill;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreRelicOrCurseDamageRatioChange;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreUmbrella;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignorePostSlotSpellTakeDamageTrigger;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreFloatText;

	[MarshalAs(UnmanagedType.U1)]
	public bool ignoreBeHitColor;

	[MarshalAs(UnmanagedType.U1)]
	public bool isUndifferDamage;

	[MarshalAs(UnmanagedType.U1)]
	public bool stopAnnouncedDeath;

	public float extraCriticalChance;

	public float teammateTakeDamageRatio;

	public float playerTakeDamageRatio;

	[MarshalAs(UnmanagedType.U1)]
	public bool isTrapDamage;

	[MarshalAs(UnmanagedType.U1)]
	public bool isPercentageDamage;

	public int damageRecordId;

	public static TakeDamageInfo_Dots NewInfo(Entity attackEntity)
	{
		NewInfo(out var info);
		info.attackerEntity = attackEntity;
		return info;
	}

	public static TakeDamageInfo_Dots NewInfo(AttackerType attackerType)
	{
		NewInfo(out var info);
		info.attackerType = attackerType;
		return info;
	}

	public static void NewInfo(out TakeDamageInfo_Dots info)
	{
		info = new TakeDamageInfo_Dots
		{
			beHitShakeDir = new float3(0f, 1f, 0f),
			teammateTakeDamageRatio = 1f,
			playerTakeDamageRatio = 1f
		};
		info.spell.CostRefraction = true;
	}

	public static void NewInfo(float damage, out TakeDamageInfo_Dots info)
	{
		NewInfo(out info);
		info.damage = damage;
	}

	public static void NewInfo(Entity spellEntity, bool CostPenetrate, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, in SpellComponentData data, out TakeDamageInfo_Dots info, bool CostRefraction = true)
	{
		NewInfo(out info);
		info.attackerEntity = data.OwnerEntity;
		info.damage = ((config.DamageInterval > 0f) ? (config.Damage.Calculate() * config.DamageInterval) : config.Damage.Calculate());
		info.spell = new SpellData
		{
			Entity = spellEntity,
			CostPenetrate = CostPenetrate,
			Config = config,
			Movement = movement,
			Transform = transform,
			ElementEffect = elementEffect,
			HitPosition = transform.Position,
			IgnoreHitEffect = false,
			CostRefraction = CostRefraction
		};
	}

	public void SetKnockbackForceIgnoreZBySpell(float3 direction)
	{
		knockbackForce = direction.IgnoreZ();
		knockbackForce = math.normalizesafe(knockbackForce);
		knockbackForce *= spell.Config.Knockback;
	}
}
