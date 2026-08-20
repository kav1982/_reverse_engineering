using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct UnitProperty_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public int id;

	public Entity ett_BeHit;

	public Entity ett_Motion;

	public bool isInitialed;

	public UnitConfig unitCfg;

	public bool registered;

	public bool isDead;

	public bool disabled;

	public BlobAssetReference<BlobArray<ItemInfo>> deadDropItemInfos;

	public float size;

	public bool CanTouch;

	public bool CanBeTarget;

	public int navAreaMask;

	public float curse_MonsterRecoveryTimer;

	public bool isRelicShowUnitHPCreate;

	public Entity ett_RelicShowUnitHP;

	public float3 thisFrameKnockback;

	public float3 currentKnockback;

	public float3 currentMotion;

	public bool IsVelocityDeclice;

	public bool isBeHitShake;

	public bool beHitShakeOut;

	public bool beHitNeedReset;

	public float3 beHitDir;

	public float beHitCurrentOffsetAmount;

	public bool isBeHitColor;

	public bool needChangeColor;

	public float beHitColorTimer;

	public bool needSyncColor;

	public Color baseColor;

	public bool isChAge14_Static;

	private int flyCounter;

	private int invincibleCounter;

	private int immuneKnockbackCounter;

	public float affect_SpikesTimer;

	public float monsterTouchTimer;

	private int immnueMucusCounter;

	public float playerInvincibleFrameTimer;

	public float playerInvincibleFrameTwinkleTimer;

	public float playerInvincibleFrameTwinkleAlpha;

	public bool showAffect;

	public Entity mucusEF;

	public bool affect_IsMucusHit;

	public bool affect_IsMucusDecelerate;

	public float affect_MucusHitTimer;

	public float affect_MucusMoveSpeedRatio;

	public float affect_MucusSpellSpeedRatio;

	private int immnueVenomCounter;

	public float affect_VenomDurationTimer;

	public float affect_VenomInjureTimer;

	public float affect_VenomCurrentStack;

	public float affect_burnDurationTimer;

	public float affect_burnAttackIntervalTimer;

	public float affect_burnHPRatio;

	public float affect_burnHPRatioPerHit;

	public bool Affect_InAbyss;

	public float3 affect_AbyssPoint;

	public UnitProperty.Affect_FrozenState FronzenState;

	public float affect_FrozenTime;

	public float affect_FrozenTimer;

	public bool frozenStateChanged;

	public Entity frozenEF;

	public bool frozenBeforeKinematic;

	public float3 frozenBeforeSpeed;

	private int immuneVoidCounter;

	public Spell3129VoidExplosion.VoidExplosionData_Dots voidExplosionData;

	public float voidEffectTimer;

	public Entity voidEF;

	public float affect_ReverseMoveTimer;

	public Entity reverseEF;

	private int immuneGroundAffectTimer;

	public float DamageReciveIncresePercent;

	public float damageReciveIncresePercentTimer;

	public Entity weakenEF;

	public float BlueRuneTakeDamageIncreaseRatio;

	public Entity BlueRuneWeakenEF;

	public float stuckCheckIntervalTimer;

	public bool isStuck;

	public float MoveSpeed => unitCfg.moveSpeed * (float)((!affect_IsReverseMove) ? 1 : (-1)) * affect_MucusMoveSpeedRatio;

	public bool IsFly => flyCounter > 0;

	public bool IsInvincible => invincibleCounter > 0;

	public bool IsImmuneKnockback => immuneKnockbackCounter > 0;

	public bool IsImmuneMucus => immnueMucusCounter > 0;

	public bool IsPlayerInInvincibleFrame => playerInvincibleFrameTimer > 0f;

	public bool IsImmuneVenom => immnueVenomCounter > 0;

	public bool LockMotion
	{
		get
		{
			if (FronzenState != UnitProperty.Affect_FrozenState.Frozening)
			{
				return Affect_InAbyss;
			}
			return true;
		}
	}

	public bool IsImmuneVoid => immuneVoidCounter > 0;

	public bool affect_IsReverseMove => affect_ReverseMoveTimer > 0f;

	public bool IsImmuneGroundAffect => immuneGroundAffectTimer > 0;

	public bool HasTakeDamageIncreaseEffect
	{
		get
		{
			if (!(DamageReciveIncresePercent > 0f))
			{
				return BlueRuneTakeDamageIncreaseRatio > 0f;
			}
			return true;
		}
	}

	public void GetTakeDamageIncreaseRatio(out float ratio)
	{
		ratio = 1f + DamageReciveIncresePercent + BlueRuneTakeDamageIncreaseRatio;
	}

	public void BeHitShake(float3 beHitDir)
	{
		isBeHitShake = true;
		beHitShakeOut = true;
		beHitNeedReset = true;
		this.beHitDir = beHitDir;
	}

	public void TakeKnockback(float3 knockBackForce)
	{
		knockBackForce.z = 0f;
		thisFrameKnockback += knockBackForce * unitCfg.knockbackRatio;
	}

	public void SetBeHitColor()
	{
		if (unitCfg.isBehitColor && unitCfg.unitType != 0)
		{
			isBeHitColor = true;
			beHitColorTimer = 0.1f;
			if (isChAge14_Static)
			{
				ChangeColor(GameConst.color_BeHit_H);
			}
			else
			{
				ChangeColor(GameConst.color_BeHit);
			}
		}
	}

	public void ChangeColor(Color color)
	{
		UnitType unitType = unitCfg.unitType;
		if (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack || unitCfg.id == 301441)
		{
			color.a = baseColor.a;
		}
		baseColor = color;
		needChangeColor = true;
	}

	public void ChangeAlpha(float alpha)
	{
		baseColor.a = alpha;
		needChangeColor = true;
	}

	public void Initialize(NativeHashMap<int, UnitConfig> unitConfigMap)
	{
		if (!isInitialed)
		{
			isInitialed = true;
			if (unitCfg.id != 800001)
			{
				unitCfg = unitConfigMap[id];
			}
			unitCfg.currentHP = unitCfg.maxHP;
			IsVelocityDeclice = true;
			CanBeTarget = true;
			CanTouch = true;
			showAffect = true;
			baseColor = Color.white;
			isChAge14_Static = GameMgr.IsHarmony_Static;
			navAreaMask = (unitCfg.isFly ? 32 : 16);
			if (unitCfg.isHybirdUnit && ScriptableObjMgr.Inst.testCtrller.MasterSwitch && ScriptableObjMgr.Inst.testCtrller.battleBossHpDivide > 1f && !unitCfg.IsSameCamp(UnitType.Player))
			{
				unitCfg.maxHP /= ScriptableObjMgr.Inst.testCtrller.battleBossHpDivide;
				unitCfg.maxHP = Mathf.Max(unitCfg.maxHP, 1f);
				unitCfg.currentHP = unitCfg.maxHP;
			}
			affect_MucusMoveSpeedRatio = 1f;
			affect_MucusSpellSpeedRatio = 1f;
			if (unitCfg.isFly)
			{
				FlyRegister();
			}
			if (unitCfg.immuneMucus)
			{
				ImmuneMucusRegister();
			}
			if (unitCfg.immuneVenom)
			{
				ImmuneVenomRegister();
			}
		}
	}

	public void ClearAllRegister()
	{
		flyCounter = 0;
		invincibleCounter = 0;
		immuneKnockbackCounter = 0;
		immnueMucusCounter = 0;
		immnueVenomCounter = 0;
		unitCfg.immuneAbyss = false;
		unitCfg.immuneSpike = false;
	}

	public void FlyRegister()
	{
		flyCounter++;
		if (affect_IsMucusDecelerate)
		{
			affect_IsMucusDecelerate = false;
		}
	}

	public void FlyRegisterWithMate()
	{
		FlyRegister();
	}

	public void FlyUnregister()
	{
		flyCounter--;
		if (flyCounter < 0)
		{
			flyCounter = 0;
			Debug.LogError("为什么飞翔计数器会小于0");
		}
	}

	public void FlyUnregisterWithMate()
	{
		FlyUnregister();
	}

	public void InvincibleRegister()
	{
		invincibleCounter++;
	}

	public void InvincibleUnregister()
	{
		invincibleCounter--;
		if (invincibleCounter < 0)
		{
			invincibleCounter = 0;
			Debug.LogWarning("为什么无敌计数器会小于0");
		}
	}

	public void ImmuneKnockbackRegister()
	{
		immuneKnockbackCounter++;
	}

	public void ImmuneKnockbackUnregister()
	{
		immuneKnockbackCounter--;
		if (immuneKnockbackCounter < 0)
		{
			immuneKnockbackCounter = 0;
			Debug.LogError("为什么免疫击退计数器会小于0");
		}
	}

	public void JumpStartSetting()
	{
		ImmuneKnockbackRegister();
		FlyRegister();
		IsVelocityDeclice = false;
	}

	public void JumpStopSetting()
	{
		ImmuneKnockbackUnregister();
		FlyUnregister();
		IsVelocityDeclice = true;
	}

	public void ImmuneMucusRegister()
	{
		immnueMucusCounter++;
		ClearMucusState();
		UpdateBodyColor();
	}

	public void ImmuneMucusUnregister()
	{
		immnueMucusCounter--;
		if (immnueMucusCounter < 0)
		{
			immnueMucusCounter = 0;
		}
	}

	public void ImmuneVenomRegister()
	{
		immnueVenomCounter++;
		ClearVenomState();
		UpdateBodyColor();
	}

	public void ImmuneVenomUnregister()
	{
		immnueVenomCounter--;
		if (immnueVenomCounter < 0)
		{
			immnueVenomCounter = 0;
		}
	}

	public void ImmuneVoidRegister()
	{
		immuneVoidCounter++;
		ClearVoidState();
		UpdateBodyColor();
	}

	public void ImmuneVoidUnregister()
	{
		immuneVoidCounter--;
		if (immuneVoidCounter < 0)
		{
			immuneVoidCounter = 0;
		}
	}

	public void ImmuneGroundAffectRegister()
	{
		immuneGroundAffectTimer++;
	}

	public void ImmuneGroundAffectUnregister()
	{
		immuneGroundAffectTimer--;
		if (immuneGroundAffectTimer < 0)
		{
			immuneGroundAffectTimer = 0;
			Debug.LogError("为什么免疫地面效果计数器会小于0");
		}
	}

	public void FallinAbyss(Vector3 fallPoint)
	{
		if (!Affect_InAbyss)
		{
			Affect_InAbyss = true;
			affect_AbyssPoint = fallPoint;
		}
	}

	public void SetSize(float size)
	{
		this.size = size;
	}

	public void SetReverseMove(float duration)
	{
		affect_ReverseMoveTimer = duration;
	}

	public void BonusTakeDamageRatioRegister(float ratioInPercent, float effectDuration)
	{
		DamageReciveIncresePercent = Mathf.Max(0f, ratioInPercent / 100f);
		damageReciveIncresePercentTimer = effectDuration;
	}

	public void SetMucus(float time, float moveSpeedRatio, float spellSpeedRatio, bool changeColor = true)
	{
		if (IsImmuneMucus || IsInvincible)
		{
			return;
		}
		if (!affect_IsMucusHit)
		{
			affect_IsMucusHit = true;
			if (changeColor)
			{
				ChangeColor(GameConst.color_BodyMucus);
			}
		}
		float num = time;
		if (unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Boss)
		{
			num *= unitCfg.frozenTimeRatio;
		}
		if (affect_MucusHitTimer < num)
		{
			affect_MucusHitTimer = num;
		}
		affect_MucusMoveSpeedRatio = Mathf.Clamp(moveSpeedRatio, 0f, 1f);
		affect_MucusSpellSpeedRatio = Mathf.Clamp(spellSpeedRatio, 0f, 1f);
	}

	public void SetVenom(float duration, float applyCount)
	{
		if (!IsImmuneVenom && !IsInvincible)
		{
			if (affect_VenomDurationTimer == 0f)
			{
				ChangeColor(GameConst.color_BodyVenom);
				affect_VenomInjureTimer = 0.5f;
			}
			if (affect_VenomDurationTimer < duration + 0.5f)
			{
				affect_VenomDurationTimer = duration + 0.5f;
			}
			affect_VenomCurrentStack += applyCount;
		}
	}

	public void GetVenomInfo(out float duration, out int applyCount)
	{
		duration = affect_VenomDurationTimer;
		applyCount = (int)affect_VenomCurrentStack;
	}

	public void ClearVenom()
	{
		affect_VenomDurationTimer = 0.0001f;
	}

	public void SetVoid(Spell3129VoidExplosion.VoidExplosionData_Dots voidData)
	{
		if (unitCfg.unitType != UnitType.NotAttack && unitCfg.unitType != UnitType.Brittleness && !IsImmuneVoid)
		{
			voidEffectTimer = 3f;
			voidExplosionData.ExplosionRange = Mathf.Max(voidExplosionData.ExplosionRange, voidData.ExplosionRange);
			voidExplosionData.HpToDmgRatio = Mathf.Max(voidExplosionData.HpToDmgRatio, voidData.HpToDmgRatio);
			voidExplosionData.InstantKillRatio = Mathf.Max(voidExplosionData.InstantKillRatio, voidData.InstantKillRatio);
			voidExplosionData.ConstVoidEffect = voidExplosionData.ConstVoidEffect || voidData.ConstVoidEffect;
			_ = voidExplosionData.ConstVoidEffect;
		}
	}

	public void SetFrozen(float time)
	{
		if (!(unitCfg.frozenTimeRatio <= 0f) && !IsInvincible && FronzenState == UnitProperty.Affect_FrozenState.Normal)
		{
			FronzenState = UnitProperty.Affect_FrozenState.Frozening;
			ChangeColor(GameConst.color_BodyFrozen);
			switch (unitCfg.unitType)
			{
			case UnitType.Player:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				frozenStateChanged = true;
				break;
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
			case UnitType.Monster:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				frozenStateChanged = true;
				break;
			case UnitType.Elite:
			case UnitType.Boss:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				frozenStateChanged = true;
				break;
			}
		}
	}

	public void SetBurn(float duration, float burnRatio)
	{
		affect_burnDurationTimer = Mathf.Max(affect_burnDurationTimer, duration);
		float num = burnRatio;
		if (unitCfg.unitType == UnitType.Boss || unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Player)
		{
			num *= 0.1f;
		}
		if (unitCfg.id == 10501)
		{
			num *= 1E-05f;
		}
		affect_burnHPRatio = Mathf.Max(affect_burnHPRatio, num);
		affect_burnHPRatioPerHit = Mathf.Max(affect_burnHPRatioPerHit, num);
	}

	public void GetBurnInfo(out float duration, out float burnRatio)
	{
		duration = affect_burnDurationTimer;
		burnRatio = affect_burnHPRatio;
		if (unitCfg.unitType == UnitType.Boss || unitCfg.unitType == UnitType.Elite)
		{
			burnRatio /= 0.1f;
		}
		if (unitCfg.unitType == UnitType.Player)
		{
			burnRatio /= 0.05f;
		}
		if (unitCfg.id == 10501)
		{
			burnRatio /= 1E-05f;
		}
	}

	public void PurifyAbnormalState()
	{
		ClearFrozenState();
		ClearBurnState();
		ClearMucusState();
		ClearVenomState();
		ClearVoidState();
		ResetBodyColor();
	}

	public void ClearMucusState()
	{
		affect_MucusHitTimer = 0f;
		affect_IsMucusHit = false;
		affect_IsMucusDecelerate = false;
		affect_MucusMoveSpeedRatio = 1f;
		affect_MucusSpellSpeedRatio = 1f;
	}

	public void ClearVoidState()
	{
		voidExplosionData = default(Spell3129VoidExplosion.VoidExplosionData_Dots);
		voidEffectTimer = 0f;
	}

	public void ClearFrozenState()
	{
		if (FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			frozenStateChanged = true;
		}
		FronzenState = UnitProperty.Affect_FrozenState.FrozenImmune;
		affect_FrozenTimer = 0f;
	}

	public void ClearBurnState()
	{
		affect_burnDurationTimer = 0f;
		affect_burnAttackIntervalTimer = 0f;
		affect_burnHPRatio = 0f;
		affect_burnHPRatioPerHit = 0f;
	}

	public void ClearVenomState()
	{
		affect_VenomDurationTimer = 0f;
		affect_VenomCurrentStack = 0f;
	}

	public void ResetBodyColor()
	{
		ChangeColor(Color.white);
	}

	public void UpdateBodyColor()
	{
		Color color = Color.white;
		if (affect_IsMucusHit)
		{
			color = GameConst.color_BodyMucus;
		}
		if (FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			color = GameConst.color_BodyFrozen;
		}
		if (affect_VenomDurationTimer > 0f)
		{
			color = GameConst.color_BodyVenom;
		}
		if (affect_burnDurationTimer > 0f)
		{
			color = GameConst.color_BodyBurn;
		}
		if (voidEffectTimer > 0f)
		{
			color = GameConst.color_BodyVoid;
		}
		if (isBeHitColor)
		{
			color = ((!isChAge14_Static) ? GameConst.color_BeHit : GameConst.color_BeHit_H);
		}
		ChangeColor(color);
	}

	public void PlayerIntoInvisibleFrame()
	{
		playerInvincibleFrameTimer = 0.33f;
		playerInvincibleFrameTwinkleAlpha = 0.3f;
		ChangeAlpha(playerInvincibleFrameTwinkleAlpha);
	}

	public void AnnouncedDeath(Entity thisEntity, bool ignoreBeforeAnnouncedDeath = true)
	{
		if (!isDead)
		{
			TakeDamageInfo_Dots info_Dots = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			info_Dots.isTargetDead = true;
			if (unitCfg.triggerDeadEvent)
			{
				info_Dots.isTriggerDeadEvent = true;
			}
			AnnouncedDeath(info_Dots, thisEntity, ignoreBeforeAnnouncedDeath);
		}
	}

	public void AnnouncedDeath(TakeDamageInfo_Dots info_Dots, Entity entity, bool ignoreBeforeAnnouncedDeath = true)
	{
		if (!isDead)
		{
			isDead = true;
			TakeDamageInfo_Dots takeDamageInfo_Dots = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			takeDamageInfo_Dots.isTargetDead = true;
			if (unitCfg.triggerDeadEvent)
			{
				takeDamageInfo_Dots.isTriggerDeadEvent = true;
			}
			DynamicBuffer<TakeDamageInfo_Dots> buffer = UnitDotsSyncSystem.GetBuffer<TakeDamageInfo_Dots>(entity);
			buffer.Add(info_Dots);
			UnitDead componentData = UnitDotsSyncSystem.GetComponentData<UnitDead>(entity);
			componentData.deadlyInfo = info_Dots;
			componentData.ignoreBeforeAnnouncedDeath = ignoreBeforeAnnouncedDeath;
			componentData.deadlyInfoIndex = buffer.Length - 1;
			UnitDotsSyncSystem.SetComponentData(componentData, entity);
		}
	}

	public void AnnouncedDeath(EntityCommandBuffer.ParallelWriter ecb, int index, DynamicBuffer<TakeDamageInfo_Dots> damageBuffer, ref UnitDead unitDead, Entity entity, bool ignoreBeforeAnnouncedDeath = false)
	{
		if (!isDead)
		{
			isDead = true;
			TakeDamageInfo_Dots takeDamageInfo_Dots = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			takeDamageInfo_Dots.isTargetDead = true;
			if (unitCfg.triggerDeadEvent)
			{
				takeDamageInfo_Dots.isTriggerDeadEvent = true;
			}
			ecb.AppendToBuffer(index, entity, takeDamageInfo_Dots);
			damageBuffer.Add(takeDamageInfo_Dots);
			unitDead.deadlyInfo = takeDamageInfo_Dots;
			unitDead.ignoreBeforeAnnouncedDeath = ignoreBeforeAnnouncedDeath;
			unitDead.deadlyInfoIndex = damageBuffer.Length;
		}
	}

	public void BossDeadStay()
	{
		CanTouch = false;
		CanBeTarget = false;
		disabled = true;
		InvincibleRegister();
		ChangeColor(Color.white);
		PurifyAbnormalState();
	}
}
