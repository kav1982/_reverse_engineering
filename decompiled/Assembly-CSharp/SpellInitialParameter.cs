using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

public class SpellInitialParameter
{
	public class Builder
	{
		public delegate void OnBuildEventHandle(Builder self, SpellInitialParameter parameter);

		private bool isChargeSpell;

		private SpellShootData shootData;

		private SpellBase ownerSpell;

		private bool applyPlayerEffect;

		private bool applyWandEffect;

		private bool applySpellEffect;

		private bool applyUnitEffect;

		private bool applyOwnerSpellEffect;

		private bool applyOwnerTriggerSpellEffect;

		private WandPostSlotChargeData wandPostSlotChargeData;

		public Entity ownerPpt { get; private set; }

		public Wand shooterWand { get; private set; }

		public WandConfig shooterWandCfg { get; private set; }

		public Entity shooter { get; private set; }

		public Entity ownerUnit { get; private set; }

		public event OnBuildEventHandle OnBuildAfter;

		public void ApplyUnitEffect(Entity unit)
		{
			if (unit == default(Entity))
			{
				Debug.LogError("SIP Builder 应用单位效果失败，单位不能为空。");
				return;
			}
			if (applyPlayerEffect || applyUnitEffect)
			{
				Debug.LogError("SIP Builder 应用单位效果失败，因为已经应用过了玩家或单位效果。");
				return;
			}
			if (unit == PlayerMgr.Inst.PlayerEtt)
			{
				applyPlayerEffect = true;
			}
			else
			{
				applyUnitEffect = true;
			}
			ownerPpt = unit;
		}

		public void ApplyWandEffect(Wand wand, WandPostSlotChargeData wandChargeData, bool multiWand = false)
		{
			if (wand == null || wand.WandCfg == null)
			{
				Debug.LogError("SIP Builder 应用法杖效果失败，法杖不能为空。");
				return;
			}
			if (applyWandEffect && !multiWand)
			{
				Debug.LogError("SIP Builder 应用法杖效果失败，因为已经应用过了法杖效果。");
				return;
			}
			shooterWand = wand;
			shooterWandCfg = wand.WandCfg;
			applyWandEffect = true;
			wandPostSlotChargeData = wandChargeData;
		}

		public void ApplySpellShootDataEffect(SpellShootData shoot)
		{
			if (shoot == null)
			{
				Debug.LogError("SIP Builder 应用法术效果失败，法术不能为空。");
				return;
			}
			if (applySpellEffect)
			{
				Debug.LogError("SIP Builder 应用法术效果失败，因为已经应用过了法术效果。");
				return;
			}
			shootData = shoot;
			applySpellEffect = true;
		}

		public void ApplyOwnerSpellEffect(SpellBase targetBase)
		{
			ownerSpell = targetBase;
			applyOwnerSpellEffect = true;
		}

		public void ApplyOwnerTriggerSpellEffect(SpellBase targetBase)
		{
			ownerSpell = targetBase;
			applyOwnerTriggerSpellEffect = true;
		}

		public void ApplyShooterEntity(Entity shooterEntity, Entity ownerEntity)
		{
			shooter = shooterEntity;
			ownerUnit = ownerEntity;
		}

		public SpellInitialParameter Build(ShootSpellSpatialInfo shootSpellSpatialInfo)
		{
			if (!applySpellEffect)
			{
				Debug.LogError("没有法术信息，创建 SpellInitialParameter 失败。");
				return new SpellInitialParameter();
			}
			SpellInitialParameter spellInitialParameter = new SpellInitialParameter
			{
				shootFromSpellGroupV2 = true,
				originShootDirection = shootSpellSpatialInfo.Direction,
				originShootSpatialInfo = shootSpellSpatialInfo,
				Shooter = shooter,
				OwnerUnit = ownerUnit
			};
			if (applyPlayerEffect || (bool)shooterWand)
			{
				ProcessPlayerEffect(spellInitialParameter);
			}
			if (applyWandEffect)
			{
				ProcessWandEffect(spellInitialParameter);
			}
			if (applySpellEffect)
			{
				ProcessShootDataEffect(spellInitialParameter);
			}
			if (applyUnitEffect)
			{
				ProcessUnitEffect(spellInitialParameter);
			}
			if (applyPlayerEffect && applyWandEffect && applySpellEffect)
			{
				ProcessPlayerWandSpellShootDataEffect(spellInitialParameter);
			}
			if (applyOwnerSpellEffect)
			{
				ProcessOwnerSpellEffect(spellInitialParameter);
			}
			if (applyOwnerTriggerSpellEffect)
			{
				ProcessOwnerTriggerSpellEffect(spellInitialParameter);
			}
			ProcessShootCause(spellInitialParameter);
			ProcessSpellColor(spellInitialParameter);
			this.OnBuildAfter?.Invoke(this, spellInitialParameter);
			return spellInitialParameter;
		}

		private void ProcessPlayerEffect(SpellInitialParameter data)
		{
			data.extraDamageRatio += PlayerMgr.Inst.ExtraDamageRatio;
			data.extraCriticalChance += PlayerMgr.Inst.ExtraCriticalRatio;
			data.extraSizeRatio += PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: true);
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceSpellSpeed != null)
			{
				data.finalSpeedRatio *= (float)(100 - PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceSpellSpeed.int1.result) / 100f;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus != null)
			{
				data.finalScatterRatio *= 1f - PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus.Cfg.floatTimer;
			}
			if (PlayerMgr.Inst.ItemCtrller.curse_IsReverseShoot)
			{
				data.reverseDirection = !data.reverseDirection;
			}
			if ((bool)PlayerMgr.Inst.ItemCtrller.relic_RainbowRibbon)
			{
				data.ColorType = SpellTools.GetRandomColor();
				data.IsRandomColor = true;
				int? num = data.ColorType.ToSpellId(1);
				if (num.HasValue)
				{
					data.extraEnhanceIds.Add(num.Value);
				}
			}
			Relic_Reaper relic_Reaper = PlayerMgr.Inst.ItemCtrller.relic_Reaper;
			if ((bool)relic_Reaper)
			{
				data.VenomEffectRatio *= (int)relic_Reaper.RelicCfg.float1.result;
			}
		}

		private void ProcessWandEffect(SpellInitialParameter data)
		{
			data.shooterWand = shooterWand;
			data.shooterWandCfg = shooterWand.WandCfg;
			data.finalDamageRatio *= shooterWand.WandCfg.damageCorrection / 100f;
			data.extraSizeRatio += PlayerMgr.Inst.EffectRadiuRatioFromWandAbility() / 100f;
			data.finalDamageRatio *= shooterWand.passiveDamageRatio;
			data.extraCriticalChance += shooterWand.WandCfg.criticalChance / 100f;
			data.extraScatter += shooterWand.WandCfg.GetWandScatter();
			data.equalScatter = shooterWand.passiveEqualAngleDistribution;
			data.zeroAngleShift = data.equalScatter;
			data.WandPostSlotChargeData = wandPostSlotChargeData;
			data.finalMovementType = data.shooterWand.spellFinalMovementType;
			data.MayTriggerRedRune = shooterWand.passiveRedRuneCount > 0;
			data.OverFlowCriticalChanceToDamage = shooterWand.EnableOverFlowCriticalChanceConvertToDamage;
			switch (shooterWandCfg.specialAbility)
			{
			case WandAbility.RandomAllColor:
			{
				data.ColorType = SpellTools.GetRandomColor();
				data.IsRandomColor = true;
				int? num2 = data.ColorType.ToSpellId(1);
				if (num2.HasValue)
				{
					data.extraEnhanceIds.Add(num2.Value);
				}
				break;
			}
			case WandAbility.RandomBaseColor:
			{
				data.ColorType = SpellTools.GetRandomBaseColor();
				data.IsRandomColor = true;
				int? num3 = data.ColorType.ToSpellId(1);
				if (num3.HasValue)
				{
					data.extraEnhanceIds.Add(num3.Value);
				}
				break;
			}
			case WandAbility.RandomHighLevelColor:
			{
				data.ColorType = SpellTools.GetRandomColorIncludeVoid();
				data.IsRandomColor = true;
				int? num = data.ColorType.ToSpellId(2);
				if (num.HasValue)
				{
					data.extraEnhanceIds.Add(num.Value);
				}
				break;
			}
			case WandAbility.HigherKnockBack:
				data.finalKnockBackRatio *= shooterWandCfg.float2 / 100f;
				break;
			case WandAbility.LowerFriendlyFire:
				data.undifferDamageRatio *= shooterWandCfg.float1 / 100f;
				break;
			case WandAbility.RandomRadiu:
				data.aroundCasterRadiuRatio = Random.Range(shooterWandCfg.float1, shooterWandCfg.float2);
				break;
			case WandAbility.SpellReverseDirection:
				data.reverseDirection = !data.reverseDirection;
				break;
			}
		}

		private void ProcessOwnerSpellEffect(SpellInitialParameter data)
		{
			data.OwnerTsf = ownerSpell.transform;
			data.OnwerPoint = ownerSpell.transform.position;
			data.OwnerSpell = ownerSpell;
		}

		private void ProcessOwnerTriggerSpellEffect(SpellInitialParameter data)
		{
			data.OwnerTsf = null;
			data.OnwerPoint = ownerSpell.transform.position;
			data.OwnerSpell = ownerSpell;
		}

		private void ProcessShootDataEffect(SpellInitialParameter data)
		{
			data.shootSpellID = (data.initializedSpellCfg = (data.spelldataConfig = shootData.Spell.GetFinalConfig())).id;
			data.shootSpellPreSpells = shootData.EnhanceList.Select((SlotData e) => e.GetFinalId()).ToList();
			Wand wand = shooterWand;
			if ((object)wand != null)
			{
				data.lightningChainDamage = CanShootSpellUtils.GetLightningChainDamage_FinalPlayerValue(shootData, wand);
				data.lifeLineData = CanShootSpellUtils.GetLifeLineDamage_FinalPlayerValue(shootData, wand);
				data.parasiteWormData = CanShootSpellUtils.GetParasiteWormEffect_FinalPlayerValue(shootData, wand);
				data.selfSacrificeData = CanShootSpellUtils.GetSelfSacrifice_FinalPlayerValue(shootData, wand);
				data.SpellSummonimmuteDeathTime = CanShootSpellUtils.GetTeammateImmuteDeath_FinalPlayerValue(shootData, wand);
				data.fuseData = CanShootSpellUtils.GetFuseSummonData_FinalPlayerValue(shootData, wand);
				data.summonAdvanceSkillType1Level = CanShootSpellUtils.GetSummonAdvanceSkillType1Level_FinalPlayerValue(shootData, wand);
				data.SummonFollowOwnerThroughMapChance = CanShootSpellUtils.GetSummonThroughMapChance(shootData, wand);
				data.SpellSummonGainOwnerHpRatio = CanShootSpellUtils.GetSummonGainOwnerHpRatio(shootData, wand);
				(data.reverseDirection, data.reverseCastBonusSpeed) = CanShootSpellUtils.GetReverseShootState(shootData, wand, data.reverseDirection);
			}
			RatioValue spellDamage = shootData.GetSpellDamage();
			data.extraDamageRatio += spellDamage.CurrentAddRatioStartZero;
			data.finalDamageRatio *= spellDamage.CurrentMulRatio;
			data.finalDamageExtra += Mathf.CeilToInt(spellDamage.CurrentAddExtra);
			RatioValue spellFlySpeed = shootData.GetSpellFlySpeed();
			data.extraSpeedRatio += spellFlySpeed.CurrentAddRatioStartZero;
			data.finalSpeedRatio *= spellFlySpeed.CurrentMulRatio;
			data.bounsSpeed += spellFlySpeed.CurrentAddBase;
			data.IsIgnoreWall = shootData.GetSpellIgnoreWall();
			data.finalMovementType = shootData.GetSpellMovementType(wand);
			data.extraCriticalChance += shootData.GetSpellCriticalChance().CurrentAddBase;
			RatioValue spellDuration = shootData.GetSpellDuration();
			data.extraDuration += spellDuration.CurrentAddBase;
			data.finalDurationRatio *= spellDuration.CurrentMulRatio;
			RatioValue spellEffectRadius = shootData.GetSpellEffectRadius();
			data.extraSizeRatio += spellEffectRadius.CurrentAddRatioStartZero;
			data.finalSizeRatio *= spellEffectRadius.CurrentMulRatio;
			data.SpellVolumeRatio *= shootData.GetSpellVolumeRatio();
			data.PenetrateCount = shootData.GetSpellPenetrateCount();
			data.SpellHoverDuration = shootData.GetSpellHoverDuration();
			data.GravitationalCrystalData = shootData.GetSpellGravitationalCrystalData();
			(float, float) spellDecreaseRadiuSpeedInfo = shootData.GetSpellDecreaseRadiuSpeedInfo();
			data.radiuDecreaseRatio = spellDecreaseRadiuSpeedInfo.Item1;
			data.radiuDcreaseTransIntoDamageRatio = spellDecreaseRadiuSpeedInfo.Item2;
			data.reboundInfo = shootData.GetReboundTime_FinalPlayerValue();
			if (spellDecreaseRadiuSpeedInfo.Item1 < 1f && (spellEffectRadius.BaseValue > 0.0 || shootData.GetSpellFallExplosionRadius() > 0f))
			{
				float num = (float)spellEffectRadius.BaseValue + shootData.GetSpellFallExplosionRadius();
				data.finalDamageRatio *= 1f + GeneralTool.GetSpellRadiusToDamageRatio(num * spellEffectRadius.CurrentAddRatioStartOne * spellEffectRadius.CurrentMulRatio, spellDecreaseRadiuSpeedInfo.Item1, spellDecreaseRadiuSpeedInfo.Item2);
			}
			if (data.reverseDirection)
			{
				data.bounsSpeed += data.reverseCastBonusSpeed;
			}
			RatioValue spellScatter = shootData.GetSpellScatter();
			data.extraScatter += spellScatter.CurrentAddBase;
			data.summonHpRatio.Merge(shootData.GetSummonHp());
			data.summonHpRecover += shootData.GetSummonHpRecover();
			data.summonExtraMoveSpeedRatio += shootData.GetSummonMoveSpeedRatio().CurrentAddRatioStartZero;
			data.summonExtraAttackSpeedRatio += shootData.GetSummonAttackSpeedRatio().CurrentAddRatioStartZero;
			if (applyWandEffect)
			{
				data.RefractionInfo = shootData.GetSpellRefractionInfo_FinalPlayerValue(wand);
			}
			else
			{
				data.RefractionInfo = shootData.GetSpellRefractionInfo();
			}
			data.VenomElementData = shootData.GetVenomElementData();
			data.MucusElementData = shootData.GetMucusElementData();
			data.FireElementData = shootData.GetFireElementData();
			data.ThunderElementData = shootData.GetThunderElementData();
			data.VoidElementData = shootData.GetVoidElementData();
			data.FrozenDuration = shootData.GetFrozenElementData();
			data.spellEndTeleport = shootData.GetSpellIsEndTeleport();
			data.fallExplosionRadius = shootData.GetSpellFallExplosionRadius();
			data.aroundCasterRadiuRatio *= ((wand != null) ? wand.GetPassiveRotateRadiuRatio() : 1f);
			SpellShootData spellShootData = shootData;
			int forceRandomRadius;
			if ((bool)wand)
			{
				WandConfig wandCfg = wand.WandCfg;
				forceRandomRadius = ((wandCfg != null && wandCfg.specialAbility == WandAbility.RandomRadiu) ? 1 : 0);
			}
			else
			{
				forceRandomRadius = 0;
			}
			data.RotationMovementInfo = spellShootData.GetSpellRotationInfo((byte)forceRandomRadius != 0);
			(int, float) halfLifeTeleportData = shootData.GetHalfLifeTeleportData();
			data.HalfLifeTeleportRadius = halfLifeTeleportData.Item2;
			data.HalfLifeTeleportCount = halfLifeTeleportData.Item1;
			(float, float) speedToDurationData = shootData.GetSpeedToDurationData();
			data.extraDuration += GeneralTool.GetSpellSpeedToBonusDuration(shootData.GetSpellFlySpeed().Result / speedToDurationData.Item1, speedToDurationData.Item1, speedToDurationData.Item2);
			(data.speedDecreaseRatio, data.speedDecreaseTransIntoDurationRatio) = speedToDurationData;
		}

		private void ProcessUnitEffect(SpellInitialParameter data)
		{
			if (World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<TeammateData>(ownerUnit))
			{
				ProcessPlayerEffect(data);
			}
		}

		private void ProcessPlayerWandSpellShootDataEffect(SpellInitialParameter data)
		{
			if (shooterWandCfg.specialAbility == WandAbility.CheapSpellHigherDamage && shootData.GetSpellManaCost_FinalPlayerValue(shooterWand).Result <= (float)shooterWand.WandCfg.int1)
			{
				data.extraDamageRatio += shooterWand.WandCfg.float1 / 100f;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null && !shooterWand.passiveAutoWand)
			{
				data.OwnerTsf = shooterWand.tsf_ShootPoint;
			}
		}

		private void ProcessShootCause(SpellInitialParameter data)
		{
			if (applyOwnerTriggerSpellEffect)
			{
				data.ShootCause = new ShootCause.BySpell(ownerSpell, byTrigger: true);
			}
			else if (applyOwnerSpellEffect)
			{
				data.ShootCause = new ShootCause.BySpell(ownerSpell, byTrigger: false);
			}
			else if (!applyUnitEffect && applyWandEffect)
			{
				data.ShootCause = new ShootCause.ByWand(shooterWand);
			}
		}

		private void ProcessSpellColor(SpellInitialParameter data)
		{
			data.ColorType = SpellTools.GetSpellColor(shootData, data.ColorType, data.IsRandomColor);
		}

		public Builder Copy()
		{
			return new Builder
			{
				ownerPpt = ownerPpt,
				shooterWand = shooterWand,
				shooterWandCfg = shooterWandCfg,
				isChargeSpell = isChargeSpell,
				shootData = shootData,
				applyPlayerEffect = applyPlayerEffect,
				applyWandEffect = applyWandEffect,
				applySpellEffect = applySpellEffect,
				applyUnitEffect = applyUnitEffect,
				OnBuildAfter = this.OnBuildAfter,
				wandPostSlotChargeData = wandPostSlotChargeData,
				ownerSpell = ownerSpell,
				applyOwnerSpellEffect = applyOwnerSpellEffect,
				applyOwnerTriggerSpellEffect = applyOwnerTriggerSpellEffect,
				shooter = shooter,
				ownerUnit = ownerUnit
			};
		}
	}

	public UnitProperty ownerPpt;

	public bool shootFromSpellGroupV2;

	public Vector3 shootDirection = Vector3.zero;

	[CanBeNull]
	public ShootSpellSpatialInfo finalShootSpatialInfo;

	[CanBeNull]
	public ShootSpellSpatialInfo originShootSpatialInfo;

	public Vector3 originShootDirection = Vector3.zero;

	public int shootSpellID;

	public SpellConfig spelldataConfig;

	public SpellConfig initializedSpellCfg;

	public List<int> shootSpellPreSpells = new List<int>();

	[CanBeNull]
	public Wand shooterWand;

	public WandConfig shooterWandCfg;

	public float extraSizeRatio;

	public float finalSizeRatio = 1f;

	public float extraDamageRatio;

	public float finalDamageRatio = 1f;

	public float finalDamageExtra;

	public float spellEfficiency = 1f;

	public bool IsIgnoreWall;

	public float extraSpeedRatio;

	public float finalSpeedRatio = 1f;

	public float bounsSpeed;

	public float extraKnockBackRatio;

	public float finalKnockBackRatio = 1f;

	public float extraScatter;

	public float finalScatterRatio = 1f;

	public float extraDuration;

	public float finalDurationRatio = 1f;

	public float extraCriticalChance;

	public float summonHpRecover;

	public float summonExtraAttackSpeedRatio;

	public float summonExtraMoveSpeedRatio;

	public float SpellSummonGainOwnerHpRatio;

	public float undifferDamageRatio = 1f;

	public float aroundCasterRadiuRatio = 1f;

	public bool zeroAngleShift;

	public bool reverseDirection;

	public float reverseCastBonusSpeed;

	public bool equalScatter;

	public SpellColorType ColorType;

	public bool IsRandomColor;

	public bool EnableThunderEffect;

	public float lightningChainDamage;

	public (int damage, float Interval) lifeLineData = (0, 0f);

	public (int fuselevel, float buffRatio) fuseData = (0, 0f);

	public (int summonHpDropPerSecond, int parasiteCount) parasiteWormData = (0, 0);

	public (float InstantDeathHpPercent, float ExplodeRange, float ExplodeHpDamageRatio) selfSacrificeData = (0f, 0f, 0f);

	public int summonAdvanceSkillType1Level;

	public float SpellSummonimmuteDeathTime;

	public float SummonFollowOwnerThroughMapChance;

	public WandPostSlotChargeData WandPostSlotChargeData;

	public Spell4004ChargeStars ChargeStar;

	public Transform OwnerTsf;

	public Vector3 OnwerPoint = Vector3.zero;

	public SpellBase OwnerSpell;

	public bool spellEndTeleport;

	public ShootCause ShootCause;

	public int inMultiShootIndex;

	public int multiShootCount;

	public float multiShootSpace;

	public int inSpellShootIndex;

	public int spellShootCount;

	public float splitFinalDamageRatio = 1f;

	public bool shootFromPostSlots;

	public bool shootFromEcho;

	public float SpellVolumeRatio = 1f;

	public int PenetrateCount;

	public float SpellHoverDuration;

	public (float PullRange, float PullForce, int MaxPullCount) GravitationalCrystalData = (0f, 0f, 0);

	public (float VenomDuration, float VenomApplyCount) VenomElementData = (0f, 0f);

	public float VenomEffectRatio = 1f;

	public (float MucusMoveSpeedRatio, float MucusSpellSpeedRatio, float MucusDuration) MucusElementData = (1f, 1f, 0f);

	public (float FireHpBurnPercent, float FireBurnDuration) FireElementData = (0f, 0f);

	public float FrozenDuration;

	public (float ThunderHitChance, float ThunderHitRadius, float ThunderHitDamageRatio) ThunderElementData = (0f, 0f, 0f);

	public (float VoidInstantKillThreshold, float VoidExplosionHpDamageRatio, float VoidExplosionRange, float VoidDuration) VoidElementData = (0f, 0f, 0f, 0f);

	public SpellSpecialMovementType finalMovementType;

	public bool MayTriggerRedRune;

	public bool OverFlowCriticalChanceToDamage;

	public float fallExplosionRadius;

	public float radiuDecreaseRatio;

	public float radiuDcreaseTransIntoDamageRatio;

	public float speedDecreaseRatio = 1f;

	public float speedDecreaseTransIntoDurationRatio;

	public (int count, float radius)? RefractionInfo;

	public (float extraSpeed, float extraDuration, float rotationRadiu)? RotationMovementInfo;

	public int SplitIndex;

	public float HalfLifeTeleportRadius;

	public int HalfLifeTeleportCount;

	public float? OverwriteRotationStartAngle;

	public Entity Shooter;

	public Entity OwnerUnit;

	public List<int> extraEnhanceIds = new List<int>();

	public HashSet<SpellTag> tags = new HashSet<SpellTag>();

	public (int count, float addTime) reboundInfo = (0, 0f);

	public float FourDirWandAngle;

	public RatioValue summonHpRatio { get; set; } = new RatioValue(0.0);


	public bool spellIsFall => fallExplosionRadius > 0f;

	public SpellInitialParameter Copy()
	{
		return new SpellInitialParameter
		{
			ownerPpt = ownerPpt,
			shooterWand = shooterWand,
			shootSpellID = shootSpellID,
			equalScatter = equalScatter,
			extraScatter = extraScatter,
			extraDuration = extraDuration,
			lifeLineData = lifeLineData,
			shootDirection = shootDirection,
			shooterWandCfg = shooterWandCfg,
			zeroAngleShift = zeroAngleShift,
			MayTriggerRedRune = MayTriggerRedRune,
			OverFlowCriticalChanceToDamage = OverFlowCriticalChanceToDamage,
			extraSizeRatio = extraSizeRatio,
			finalSizeRatio = finalSizeRatio,
			spelldataConfig = spelldataConfig,
			extraSpeedRatio = extraSpeedRatio,
			finalSpeedRatio = finalSpeedRatio,
			extraDamageRatio = extraDamageRatio,
			parasiteWormData = parasiteWormData,
			selfSacrificeData = selfSacrificeData,
			summonAdvanceSkillType1Level = summonAdvanceSkillType1Level,
			SummonFollowOwnerThroughMapChance = SummonFollowOwnerThroughMapChance,
			SpellSummonimmuteDeathTime = SpellSummonimmuteDeathTime,
			finalDamageRatio = finalDamageRatio,
			reverseDirection = reverseDirection,
			reverseCastBonusSpeed = reverseCastBonusSpeed,
			finalScatterRatio = finalScatterRatio,
			finalDurationRatio = finalDurationRatio,
			extraKnockBackRatio = extraKnockBackRatio,
			shootSpellPreSpells = shootSpellPreSpells,
			initializedSpellCfg = initializedSpellCfg,
			finalKnockBackRatio = finalKnockBackRatio,
			extraCriticalChance = extraCriticalChance,
			undifferDamageRatio = undifferDamageRatio,
			lightningChainDamage = lightningChainDamage,
			originShootDirection = originShootDirection,
			shootFromSpellGroupV2 = shootFromSpellGroupV2,
			aroundCasterRadiuRatio = aroundCasterRadiuRatio,
			summonExtraAttackSpeedRatio = summonExtraAttackSpeedRatio,
			summonHpRecover = summonHpRecover,
			SpellSummonGainOwnerHpRatio = SpellSummonGainOwnerHpRatio,
			summonExtraMoveSpeedRatio = summonExtraMoveSpeedRatio,
			bounsSpeed = bounsSpeed,
			finalDamageExtra = finalDamageExtra,
			WandPostSlotChargeData = WandPostSlotChargeData,
			OwnerTsf = OwnerTsf,
			OnwerPoint = OnwerPoint,
			ColorType = ColorType,
			OwnerSpell = OwnerSpell,
			fuseData = fuseData,
			spellEndTeleport = spellEndTeleport,
			summonHpRatio = summonHpRatio.Copy(),
			multiShootCount = multiShootCount,
			multiShootSpace = multiShootSpace,
			spellShootCount = spellShootCount,
			splitFinalDamageRatio = splitFinalDamageRatio,
			inMultiShootIndex = inMultiShootIndex,
			inSpellShootIndex = inSpellShootIndex,
			finalMovementType = finalMovementType,
			ShootCause = ShootCause,
			ChargeStar = ChargeStar,
			shootFromPostSlots = shootFromPostSlots,
			shootFromEcho = shootFromEcho,
			SpellVolumeRatio = SpellVolumeRatio,
			PenetrateCount = PenetrateCount,
			SpellHoverDuration = SpellHoverDuration,
			GravitationalCrystalData = GravitationalCrystalData,
			VenomElementData = VenomElementData,
			VenomEffectRatio = VenomEffectRatio,
			MucusElementData = MucusElementData,
			FrozenDuration = FrozenDuration,
			FireElementData = FireElementData,
			ThunderElementData = ThunderElementData,
			VoidElementData = VoidElementData,
			EnableThunderEffect = EnableThunderEffect,
			extraEnhanceIds = extraEnhanceIds.ToList(),
			IsRandomColor = IsRandomColor,
			RefractionInfo = RefractionInfo,
			originShootSpatialInfo = originShootSpatialInfo?.Copy(),
			finalShootSpatialInfo = finalShootSpatialInfo?.Copy(),
			fallExplosionRadius = fallExplosionRadius,
			radiuDecreaseRatio = radiuDecreaseRatio,
			radiuDcreaseTransIntoDamageRatio = radiuDcreaseTransIntoDamageRatio,
			speedDecreaseRatio = speedDecreaseRatio,
			speedDecreaseTransIntoDurationRatio = speedDecreaseTransIntoDurationRatio,
			RotationMovementInfo = RotationMovementInfo,
			HalfLifeTeleportRadius = HalfLifeTeleportRadius,
			HalfLifeTeleportCount = HalfLifeTeleportCount,
			SplitIndex = SplitIndex,
			tags = tags.ToHashSet()
		};
	}

	public SpellInitialParameter(UnitProperty unitProperty, Vector3 direction, int spellId = 0, SpellConfig spellCfg = null, Wand targetWand = null, List<int> preSpellIds = null)
	{
		ownerPpt = unitProperty;
		shootDirection = direction;
		if (spellId != 0 && spellCfg == null)
		{
			shootSpellID = spellId;
			spelldataConfig = SpellConfig.GetConfigCopy(shootSpellID);
		}
		else if (spellCfg != null)
		{
			shootSpellID = spellCfg.id;
			spelldataConfig = spellCfg.Copy();
			initializedSpellCfg = spellCfg.Copy();
		}
		else
		{
			Debug.LogWarning("传入的预处理信息没有包含法术ID或config");
		}
		if (targetWand != null)
		{
			shooterWand = targetWand;
		}
		if (preSpellIds != null)
		{
			shootSpellPreSpells = preSpellIds;
		}
	}

	public SpellInitialParameter()
	{
	}
}
