using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class ShootSpellUtils
{
	public record SpellShootInfo
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SpellShootInfo);
			}
		}

		public int SpellId;

		public SpellShootData ShootData;

		public Vector3 CreatePosition;

		public Vector3 TargetPosition;

		public Vector3 Direction;

		public int MultiShootCount;

		public float MultiShootGap;

		public float LowFrameMultiShootDamageRatio;

		public int InMultiShootIndex;

		public int ShootCount;

		public int InShootCountIndex;

		public float MultiCastSpellEfficiency;

		public bool IsCopyShoot;

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SpellShootInfo");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("SpellId = ");
			builder.Append(SpellId.ToString());
			builder.Append(", ShootData = ");
			builder.Append(ShootData);
			builder.Append(", CreatePosition = ");
			builder.Append(CreatePosition.ToString());
			builder.Append(", TargetPosition = ");
			builder.Append(TargetPosition.ToString());
			builder.Append(", Direction = ");
			builder.Append(Direction.ToString());
			builder.Append(", MultiShootCount = ");
			builder.Append(MultiShootCount.ToString());
			builder.Append(", MultiShootGap = ");
			builder.Append(MultiShootGap.ToString());
			builder.Append(", LowFrameMultiShootDamageRatio = ");
			builder.Append(LowFrameMultiShootDamageRatio.ToString());
			builder.Append(", InMultiShootIndex = ");
			builder.Append(InMultiShootIndex.ToString());
			builder.Append(", ShootCount = ");
			builder.Append(ShootCount.ToString());
			builder.Append(", InShootCountIndex = ");
			builder.Append(InShootCountIndex.ToString());
			builder.Append(", MultiCastSpellEfficiency = ");
			builder.Append(MultiCastSpellEfficiency.ToString());
			builder.Append(", IsCopyShoot = ");
			builder.Append(IsCopyShoot.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((((((((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(SpellId)) * -1521134295 + EqualityComparer<SpellShootData>.Default.GetHashCode(ShootData)) * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(CreatePosition)) * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(TargetPosition)) * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Direction)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(MultiShootCount)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(MultiShootGap)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(LowFrameMultiShootDamageRatio)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(InMultiShootIndex)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(ShootCount)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(InShootCountIndex)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(MultiCastSpellEfficiency)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsCopyShoot);
		}

		[CompilerGenerated]
		public virtual bool Equals(SpellShootInfo? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(SpellId, other!.SpellId) && EqualityComparer<SpellShootData>.Default.Equals(ShootData, other!.ShootData) && EqualityComparer<Vector3>.Default.Equals(CreatePosition, other!.CreatePosition) && EqualityComparer<Vector3>.Default.Equals(TargetPosition, other!.TargetPosition) && EqualityComparer<Vector3>.Default.Equals(Direction, other!.Direction) && EqualityComparer<int>.Default.Equals(MultiShootCount, other!.MultiShootCount) && EqualityComparer<float>.Default.Equals(MultiShootGap, other!.MultiShootGap) && EqualityComparer<float>.Default.Equals(LowFrameMultiShootDamageRatio, other!.LowFrameMultiShootDamageRatio) && EqualityComparer<int>.Default.Equals(InMultiShootIndex, other!.InMultiShootIndex) && EqualityComparer<int>.Default.Equals(ShootCount, other!.ShootCount) && EqualityComparer<int>.Default.Equals(InShootCountIndex, other!.InShootCountIndex) && EqualityComparer<float>.Default.Equals(MultiCastSpellEfficiency, other!.MultiCastSpellEfficiency))
				{
					return EqualityComparer<bool>.Default.Equals(IsCopyShoot, other!.IsCopyShoot);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected SpellShootInfo(SpellShootInfo original)
		{
			SpellId = original.SpellId;
			ShootData = original.ShootData;
			CreatePosition = original.CreatePosition;
			TargetPosition = original.TargetPosition;
			Direction = original.Direction;
			MultiShootCount = original.MultiShootCount;
			MultiShootGap = original.MultiShootGap;
			LowFrameMultiShootDamageRatio = original.LowFrameMultiShootDamageRatio;
			InMultiShootIndex = original.InMultiShootIndex;
			ShootCount = original.ShootCount;
			InShootCountIndex = original.InShootCountIndex;
			MultiCastSpellEfficiency = original.MultiCastSpellEfficiency;
			IsCopyShoot = original.IsCopyShoot;
		}

		public SpellShootInfo()
		{
		}
	}

	public static void ShootSpellGroup(SpellShootGroup group, ShootSpellSpatialInfo spatialInfo, SpellInitialParameter.Builder parameterBuilder, float reverseCopyShootRate)
	{
		SpellShootInfo[] array = CreateShootInfoFromShootGroup(group, spatialInfo.Start, reverseCopyShootRate).ToArray();
		if (PlayerMgr.Inst.GetRuneEffectLevel(PlayerMgr.Inst.GetPlayerRuneCount().GreenRune) >= 4 && group.HasSomeCastTypeSpell(SpellType.Summon, deepSearch: false) && parameterBuilder.shooterWand.WandCfg != null && parameterBuilder.shooterWand.passiveGreenRuneCount > 0)
		{
			parameterBuilder.shooterWand.RecordLV5GreenRuneSummonCount();
		}
		SpellInitialParameter.Builder[] right = array.Select(delegate(SpellShootInfo e)
		{
			SpellInitialParameter.Builder builder = parameterBuilder.Copy();
			builder.ApplySpellShootDataEffect(e.ShootData);
			return builder;
		}).ToArray();
		SpellInitialParameter[] array2 = (from e in array.Zip(right)
			select e.Right.Build(spatialInfo)).ToArray();
		bool equalScatter = array2[0].equalScatter;
		if (!equalScatter)
		{
			IEnumerable<Vector3> right2 = CalculateMultiShootEffectPosition(array, spatialInfo.Direction);
			foreach (var item4 in array.Zip(right2))
			{
				SpellShootInfo item = item4.Left;
				Vector3 vector = (item.CreatePosition = item4.Right);
			}
		}
		(float, float)[] finalScatterModify = array2.Select((SpellInitialParameter e) => (e.extraScatter, e.finalScatterRatio)).ToArray();
		(equalScatter ? CalculateEqualScatterAngle(array, finalScatterModify) : CalculateRandomAngle(array, finalScatterModify)).Zip(array2).Action(delegate((float Left, SpellInitialParameter Right) e)
		{
			ApplyScatterAngleToSpellSIP(spatialInfo, e.Left, e.Right);
		});
		array.Zip(array2).Action(delegate((SpellShootInfo Left, SpellInitialParameter Right) e)
		{
			e.Left.Direction = e.Right.shootDirection;
			e.Left.TargetPosition = e.Right.finalShootSpatialInfo?.Target ?? Vector3.zero;
		});
		foreach (var item5 in array.Zip(array2))
		{
			SpellShootInfo item2 = item5.Left;
			SpellInitialParameter item3 = item5.Right;
			item3.multiShootSpace = item2.MultiShootGap;
			item3.multiShootCount = item2.MultiShootCount;
			item3.inMultiShootIndex = item2.InMultiShootIndex;
			item3.spellShootCount = item2.ShootCount;
			item3.inSpellShootIndex = item2.InShootCountIndex;
			item3.finalDamageRatio *= item2.LowFrameMultiShootDamageRatio;
			item3.spellEfficiency *= item2.MultiCastSpellEfficiency;
			ApplyMultiShootEffectToFallTarget(item2, item3);
		}
		ShootToDots(array.Zip(array2));
	}

	private static void ShootToDots(IEnumerable<(SpellShootInfo info, SpellInitialParameter sip)> shootInfos)
	{
		SpellShootSystem.Shoot(shootInfos.Select(((SpellShootInfo info, SpellInitialParameter sip) e) => SipToSSP(e.info, e.sip, PlayerMgr.Inst.PlayerCtrller.PlayerAroundCenterEtt, PlayerMgr.Inst.PlayerEtt, World.DefaultGameObjectInjectionWorld.EntityManager)));
	}

	private static SpellSpawnParams SipToSSP(SpellShootInfo info, SpellInitialParameter sip, Entity playerAroundCenterEntity, Entity playerEntity, EntityManager entityManager)
	{
		SpellConfig configCopy = SpellConfig.GetConfigCopy(info.SpellId);
		AttributeValue attributeValue = default(AttributeValue);
		attributeValue.Base = configCopy.speed;
		attributeValue.AddBase = ((sip.finalMovementType != SpellSpecialMovementType.Rotation) ? 0f : (sip.RotationMovementInfo?.extraSpeed ?? 0f)) + sip.bounsSpeed;
		attributeValue.AddRatio = sip.extraSpeedRatio;
		attributeValue.MulRatio = sip.finalSpeedRatio;
		attributeValue.Extra = 0f;
		AttributeValue speedAttribute = attributeValue;
		float result = (sip.shooterWand ? info.ShootData.GetSpellShootScatter_FinalPlayerValue(sip.shooterWand) : info.ShootData.GetSpellScatter()).Result;
		RatioValue spellManaCost_FinalPlayerValue = info.ShootData.GetSpellManaCost_FinalPlayerValue(sip.shooterWand);
		int spellFinalCount_FinalPlayerValue = info.ShootData.GetSpellFinalCount_FinalPlayerValue(sip.shooterWand);
		SpellSpawnParams spellSpawnParams = default(SpellSpawnParams);
		spellSpawnParams.RandomPosFocusMouse = (bool)sip.shooterWand && sip.shooterWand.passiveRandomPosShoot;
		spellSpawnParams.SpellEndTeleport = sip.spellEndTeleport;
		spellSpawnParams.HalfLifeTeleportCount = sip.HalfLifeTeleportCount;
		spellSpawnParams.HalfLifeTeleportRadius = sip.HalfLifeTeleportRadius;
		spellSpawnParams.ReserveDirection = sip.reverseDirection;
		spellSpawnParams.ManaCost = spellManaCost_FinalPlayerValue.Result / (float)spellFinalCount_FinalPlayerValue;
		spellSpawnParams.PrefabId = info.SpellId / 10;
		spellSpawnParams.SpawnPosition = info.CreatePosition;
		spellSpawnParams.InShootCountIndex = info.InShootCountIndex;
		spellSpawnParams.SpellExtraSizeRatio = info.ShootData.GetSpellVolumeRatio();
		spellSpawnParams.FromPostSlot = sip.shootFromPostSlots;
		spellSpawnParams.FromEcho = sip.shootFromEcho;
		spellSpawnParams.Offset = (float)(info.MultiShootCount - 1) * info.MultiShootGap * -0.5f + info.MultiShootGap * (float)info.InMultiShootIndex;
		spellSpawnParams.SourceShootDir = ((float3)(sip.originShootSpatialInfo?.Direction ?? info.Direction)).xy;
		spellSpawnParams.SourceShootTargetPosition = ((float3)(sip.originShootSpatialInfo?.Target ?? info.CreatePosition)).xy;
		spellSpawnParams.MultiShootAddictionCount = info.MultiShootCount - 1;
		spellSpawnParams.ManaCostRatio = spellManaCost_FinalPlayerValue.ResultRatio;
		spellSpawnParams.FourDirectionWandAngle = sip.FourDirWandAngle;
		spellSpawnParams.SpellEfficiency = sip.spellEfficiency;
		spellSpawnParams.EnableTriggerRedRune = sip.MayTriggerRedRune;
		spellSpawnParams.OverFlowCriticalChanceToDamage = sip.OverFlowCriticalChanceToDamage;
		spellSpawnParams.MovementComponentData = new SpellMovementComponentData
		{
			Type = sip.finalMovementType,
			Direction = info.Direction,
			Speed = speedAttribute.Calculate(),
			ReboundCount = sip.reboundInfo.count,
			ReboundAddTime = sip.reboundInfo.addTime,
			AroundCenter = info.CreatePosition,
			AroundAngle = UnityEngine.Random.Range(0, 360),
			AroundRadius = (sip.RotationMovementInfo?.rotationRadiu ?? 1f),
			ChaseRotateSpeed = ((sip.finalMovementType == SpellSpecialMovementType.ChaseEnemy) ? info.ShootData.GetChaseEnemyRotateSpeed() : info.ShootData.GetChaseOwnerRotateSpeed()),
			ChaseOwnerPosition = info.CreatePosition,
			ChaseMouseLerpSpeed = info.ShootData.GetChaseMouseLerpSpeed(),
			IsFallSpell = sip.spellIsFall,
			Gravity = 0f,
			OriginalSpellHorizontalSpeed = 0f,
			CurrentFallSpeed = 0f,
			FallingReboundForceRatio = 1f,
			FallTargetPosition = (sip.finalShootSpatialInfo.Target ?? info.CreatePosition),
			IsIgnoreWall = sip.IsIgnoreWall
		};
		spellSpawnParams.ConfigComponentData = new SpellConfigComponentData
		{
			Level = configCopy.level,
			AbilityType = configCopy.abilityType,
			ColorType = sip.ColorType,
			Id = info.SpellId,
			Radius = new RadiusAttributeValue(configCopy.radius)
			{
				FallRadius = sip.fallExplosionRadius,
				AddRatio = sip.extraSizeRatio,
				MulRatio = info.ShootData.GetSpellDecreaseRadiuSpeedInfo().radiusRatio
			},
			Damage = new AttributeValue(configCopy.damage)
			{
				AddRatio = sip.extraDamageRatio,
				MulRatio = sip.finalDamageRatio,
				Extra = sip.finalDamageExtra
			},
			UndifferDamageRatio = 1f,
			MaxUndifferDamageReceive = ((PlayerMgr.Inst.ItemCtrller.relicCfg_MaxUndifferDamage != null) ? PlayerMgr.Inst.ItemCtrller.relicCfg_MaxUndifferDamage.float1.result : (-1f)),
			CriticalChance = configCopy.criticalChance / 100f + sip.extraCriticalChance,
			Knockback = configCopy.knockback * sip.finalKnockBackRatio * (sip.extraKnockBackRatio + 1f),
			Duration = new AttributeValue(configCopy.duration)
			{
				AddBase = sip.extraDuration,
				MulRatio = sip.finalDurationRatio,
				Extra = ((sip.finalMovementType != SpellSpecialMovementType.Rotation) ? 0f : (sip.RotationMovementInfo?.extraDuration ?? 0f))
			},
			DurationTimer = 0f,
			Penetrate = new PenetrateValue(sip.PenetrateCount),
			Scatter = ((result < 0f) ? 0f : result),
			HoverTimer = 0f,
			HoverDuration = sip.SpellHoverDuration,
			DamageTimer = 0f,
			DamageInterval = (configCopy.isDPS ? configCopy.DPSDamageInterval : 0f),
			GravitationalAttackRange = sip.GravitationalCrystalData.PullRange,
			GravitationalAttackPullForce = sip.GravitationalCrystalData.PullForce,
			GravitationalMaxApplyCount = sip.GravitationalCrystalData.MaxPullCount,
			LightningChainDamage = sip.lightningChainDamage,
			Int1 = configCopy.int1,
			Int2 = configCopy.int2,
			Int3 = configCopy.int3,
			Float1 = configCopy.float1,
			Float2 = configCopy.float2,
			Float3 = configCopy.float3
		};
		spellSpawnParams.ElementComponentData = new SpellElementEffectComponentData
		{
			VenomDuration = sip.VenomElementData.VenomDuration,
			VenomApplyCount = sip.VenomElementData.VenomApplyCount * sip.VenomEffectRatio,
			MucusDuration = sip.MucusElementData.MucusDuration,
			MucusMoveSpeedRatio = sip.MucusElementData.MucusMoveSpeedRatio,
			MucusSpellSpeedRatio = sip.MucusElementData.MucusSpellSpeedRatio,
			FrozenDuration = sip.FrozenDuration,
			FireBurnDuration = sip.FireElementData.FireBurnDuration,
			FireHpBurnPercent = sip.FireElementData.FireHpBurnPercent,
			VoidExplosionRange = sip.VoidElementData.VoidExplosionRange,
			VoidInstantKillThreshold = sip.VoidElementData.VoidInstantKillThreshold,
			VoidExplosionHpDamageRatio = sip.VoidElementData.VoidExplosionHpDamageRatio,
			ThunderHitRadius = sip.ThunderElementData.ThunderHitRadius * (1f + sip.extraSizeRatio) * sip.finalSizeRatio * info.ShootData.GetSpellDecreaseRadiuSpeedInfo().radiusRatio,
			ThunderHitDamageRatio = sip.ThunderElementData.ThunderHitDamageRatio
		};
		spellSpawnParams.SplitComponentData = new SpellSplitComponentData
		{
			Count = info.ShootData.GetSplitCount(),
			DamageRatio = 0.33f
		};
		spellSpawnParams.RefractionData = new SpellRefractionData
		{
			RemainCount = (info.ShootData.GetSpellRefractionInfo()?.count ?? 0)
		};
		spellSpawnParams.SpeedAttribute = speedAttribute;
		spellSpawnParams.BonusPenetrate = sip.PenetrateCount;
		spellSpawnParams.TeammateComponentData = new TeammateData
		{
			IsInitialized = false,
			TeammateSpeedRatio = 1f + sip.summonExtraAttackSpeedRatio,
			AdvanceSkillLevel = sip.summonAdvanceSkillType1Level,
			TeammateHpRatio = new AttributeValue
			{
				AddRatio = sip.summonHpRatio.CurrentAddRatioStartOne,
				MulRatio = sip.summonHpRatio.CurrentMulRatio
			},
			TeammateHpRecoverAmountPerSecond = sip.summonHpRecover,
			TeammateHpDropAmountPerSecond = sip.parasiteWormData.summonHpDropPerSecond,
			TeammateHpEffectCalculateTimer = 0f,
			SeparateCalculateHpEffect = false,
			OnDeathSpawnWormCount = sip.parasiteWormData.parasiteCount,
			LifeLineDamage = sip.lifeLineData.damage,
			ExplodeRange = sip.selfSacrificeData.ExplodeRange,
			ExplodeHpDamageRatio = sip.selfSacrificeData.ExplodeHpDamageRatio,
			TeammateSuddenDeathHPThreshold = sip.selfSacrificeData.InstantDeathHpPercent,
			SpellSummonGainOwnerHpRatio = sip.SpellSummonGainOwnerHpRatio,
			SummonFollowOwnerThroughMapChance = sip.SummonFollowOwnerThroughMapChance,
			TeammateMaxFuseLevel = sip.fuseData.fuselevel,
			TeammateDelayDeathTime = sip.SpellSummonimmuteDeathTime,
			TeammateDelayDeathEffectActive = false
		};
		SpellSpawnParams ssp = spellSpawnParams;
		ssp.SetShooter(sip.Shooter, sip.OwnerUnit);
		RadiusAttributeValue radius = ssp.ConfigComponentData.Radius;
		radius.FallRadius = 0f;
		radius.Base = ssp.MovementComponentData.AroundRadius;
		ssp.MovementComponentData.AroundRadius = radius.Calculate();
		ssp.MovementComponentData.AroundTarget = ((sip.Shooter == playerEntity) ? playerAroundCenterEntity : sip.Shooter);
		if ((bool)sip.ChargeStar)
		{
			if (configCopy.isKeepCasting)
			{
				ssp.MovementComponentData.AroundTarget = sip.Shooter;
			}
			else if (sip.OwnerUnit == playerEntity)
			{
				ssp.MovementComponentData.AroundTarget = playerAroundCenterEntity;
			}
			else if (sip.shooterWand.passiveAutoWand)
			{
				ssp.MovementComponentData.AroundTarget = sip.OwnerUnit;
			}
		}
		if (sip.spelldataConfig.useType == SpellType.Summon)
		{
			TeammateType teammateType = TeammateType.teammate1;
			bool flag = false;
			switch (sip.spelldataConfig.abilityType)
			{
			case SpellAbilityType.Summon1:
				teammateType = TeammateType.teammate1;
				break;
			case SpellAbilityType.Summon2:
				teammateType = TeammateType.teammate2;
				break;
			case SpellAbilityType.Summon3:
				teammateType = TeammateType.teammate3;
				flag = true;
				break;
			case SpellAbilityType.Summon4:
				teammateType = TeammateType.teammate4;
				break;
			case SpellAbilityType.Summon5:
				teammateType = TeammateType.teammate5;
				break;
			case SpellAbilityType.Summon6:
				teammateType = TeammateType.teammate6;
				break;
			case SpellAbilityType.Summon7:
				teammateType = TeammateType.teammate7;
				break;
			}
			ssp.TeammateComponentData.TeammateType = teammateType;
			if (flag)
			{
				ssp.TeammateComponentData.TeammateId = (int)(teammateType + 1);
			}
			else
			{
				ssp.TeammateComponentData.TeammateId = (int)(teammateType + sip.spelldataConfig.level);
			}
		}
		if (info.ShootData.Spell.GetFinalConfig().abilityType == SpellAbilityType.Meteor)
		{
			ssp.MovementComponentData.IsFallSpell = true;
		}
		if (ssp.OverFlowCriticalChanceToDamage && ssp.ConfigComponentData.CriticalChance >= 1f)
		{
			ssp.ConfigComponentData.Damage.MulRatio *= ssp.ConfigComponentData.CriticalChance;
		}
		if ((bool)sip.shooterWand)
		{
			ssp.Wand = sip.shooterWand;
			switch (sip.shooterWand.WandCfg.specialAbility)
			{
			case WandAbility.LowerFriendlyFire:
				ssp.ConfigComponentData.UndifferDamageRatio = math.min(ssp.ConfigComponentData.UndifferDamageRatio, sip.shooterWand.WandCfg.float1 / 100f);
				break;
			case WandAbility.KillMonsterChanceDropCoin:
				ssp.DropCoinRatioOnKill = ssp.Wand.Value.WandCfg.float1 / 100f;
				break;
			case WandAbility.KillMonsterChanceDropCrystal:
				ssp.DropCrystalRatioOnKill = ssp.Wand.Value.WandCfg.float1 / 100f;
				break;
			}
		}
		if (info.ShootData.SubGroup != null)
		{
			ssp.SubGroupEntity = entityManager.CreateEntity(typeof(SpellSubGroupComponentData));
			entityManager.AddComponentObject(ssp.SubGroupEntity, new SpellSubGroupComponentData
			{
				SubGroup = info.ShootData.SubGroup
			});
		}
		if (info.ShootData.SubGroup != null)
		{
			SpellConfig[] source = info.ShootData.Triggers.Select((SlotData slot) => slot.GetFinalConfig()).ToArray();
			SpellConfig[] array = source.Where((SpellConfig slot) => slot.abilityType == SpellAbilityType.OnMoveTrigger).ToArray();
			if (array.Length != 0)
			{
				float num = (float)array.Select((SpellConfig slot) => slot.int1).Min() / 100f;
				ssp.MoveTriggerComponentData = new SpellMoveTriggerComponentData
				{
					SubGroupMpCost = info.ShootData.SubGroup.GetGroupManaCost_FinalPlayerValue(sip.shooterWand, ignoreParentGroup: true) * num,
					TriggerDistanceRatio = array.Sum((SpellConfig slot) => slot.float1),
					TriggerDirectionFlag = (UnityEngine.Random.Range(0, 2) == 0)
				};
			}
			SpellConfig[] array2 = source.Where((SpellConfig slot) => slot.abilityType == SpellAbilityType.OnOverSplitTrigger).ToArray();
			if (array2.Length != 0)
			{
				Entity entity = entityManager.CreateEntity(typeof(SpellOverSplitTriggerBuffer));
				DynamicBuffer<SpellOverSplitTriggerBuffer> buffer = entityManager.GetBuffer<SpellOverSplitTriggerBuffer>(entity);
				SpellConfig[] array3 = array2;
				foreach (SpellConfig spellConfig in array3)
				{
					buffer.Add(new SpellOverSplitTriggerBuffer
					{
						Count = spellConfig.int2,
						DamageRatio = (float)spellConfig.int3 / 100f
					});
				}
				ssp.OverSplitTriggerBufferEntity = entity;
			}
			SpellConfig[] array4 = source.Where((SpellConfig slot) => slot.abilityType == SpellAbilityType.OnHitTrigger).ToArray();
			if (array4.Length != 0)
			{
				float subGroupMp = info.ShootData.SubGroup.GetGroupManaCost_FinalPlayerValue(sip.shooterWand, ignoreParentGroup: true) * array4.Average((SpellConfig t) => (float)t.int1 / 100f);
				ssp.HitTriggerComponentData = new SpellHitTriggerComponentData
				{
					SubGroupMp = subGroupMp,
					Cooldown = 1f / (float)array4.Sum((SpellConfig t) => t.int2)
				};
			}
			SpellConfig[] array5 = source.Where((SpellConfig slot) => slot.abilityType == SpellAbilityType.OnOverTrigger).ToArray();
			if (array5.Length != 0)
			{
				SpellConfig[] array3 = array5;
				foreach (SpellConfig spellConfig2 in array3)
				{
					ssp.OverTriggerComponentData.AddRatio((float)spellConfig2.int2 / 100f);
				}
			}
			SpellConfig[] array6 = source.Where((SpellConfig slot) => slot.abilityType == SpellAbilityType.OnStartRotationTrigger).ToArray();
			if (array6.Length != 0)
			{
				ssp.TwineTriggerComponentData.Count = array6.Sum((SpellConfig e) => e.int2);
				ssp.TwineTriggerComponentData.Radius = array6.Max((SpellConfig e) => e.float1);
			}
		}
		if (sip.tags.Contains(SpellTag.Twine))
		{
			ssp.MovementComponentData.Type = SpellSpecialMovementType.Rotation;
			ssp.MovementComponentData.AroundAngle = sip.OverwriteRotationStartAngle.Value;
			ssp.MovementComponentData.AroundRadius = sip.RotationMovementInfo.Value.rotationRadiu;
			ssp.MovementComponentData.Speed += sip.RotationMovementInfo.Value.extraSpeed;
		}
		if ((bool)sip.ChargeStar)
		{
			ssp.ChargeStar = sip.ChargeStar;
			ssp.ChargeStarEntity = sip.ChargeStar.Entity;
		}
		if (ssp.MovementComponentData.IsFallSpell && !ssp.ConfigComponentData.IsTeammate)
		{
			SpellTools.SpellInitialFallData(ref ssp, sip, speedAttribute.Calculate());
		}
		ProcessPlayerEffect(ref ssp, info, sip);
		ProcessWandRandomColor(ref ssp, sip);
		SpellSpawnParamsProcessor.Process(info, sip, ref ssp);
		return ssp;
	}

	private static void ProcessWandRandomColor(ref SpellSpawnParams ssp, SpellInitialParameter sip)
	{
		Wand shooterWand = sip.shooterWand;
		if ((object)shooterWand == null || shooterWand.WandCfg == null)
		{
			return;
		}
		int? num = null;
		WandAbility specialAbility = sip.shooterWand.WandCfg.specialAbility;
		if (specialAbility != WandAbility.RandomBaseColor)
		{
			if (specialAbility == WandAbility.RandomAllColor)
			{
				goto IL_0056;
			}
			if (specialAbility == WandAbility.RandomHighLevelColor)
			{
				num = ssp.ConfigComponentData.ColorType.ToSpellId(2);
			}
		}
		else
		{
			SpellColorType colorType = ssp.ConfigComponentData.ColorType;
			if (colorType == SpellColorType.Venom || colorType == SpellColorType.Mucus || colorType == SpellColorType.Frozen)
			{
				goto IL_0056;
			}
		}
		goto IL_007c;
		IL_007c:
		if (num.HasValue)
		{
			ApplyElementSpell(num.Value, ref ssp);
		}
		return;
		IL_0056:
		num = ssp.ConfigComponentData.ColorType.ToSpellId(1);
		goto IL_007c;
	}

	private static void ProcessPlayerEffect(ref SpellSpawnParams spawnParams, SpellShootInfo info, SpellInitialParameter sip)
	{
		PlayerMgr inst = PlayerMgr.Inst;
		if ((object)inst == null || (object)inst.ItemCtrller == null)
		{
			return;
		}
		PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
		if (itemCtrller.relicCfg_SpellKnockback != null)
		{
			SpellConfig configCopy = SpellConfig.GetConfigCopy(info.SpellId);
			spawnParams.ConfigComponentData.Knockback += configCopy.knockback * (float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellKnockback.int1.result / 100f * sip.finalKnockBackRatio;
		}
		if ((bool)itemCtrller.relic_RainbowRibbon)
		{
			int? num = spawnParams.ConfigComponentData.ColorType.ToSpellId(1);
			if (num.HasValue)
			{
				ApplyElementSpell(num.Value, ref spawnParams);
			}
		}
		if (itemCtrller.curse_IsReverseKnockback)
		{
			spawnParams.ConfigComponentData.Knockback *= -1f;
		}
	}

	private static void ApplyElementSpell(int spellId, ref SpellSpawnParams ssp)
	{
		SpellConfig spellConfig = SpellConfig.dic[spellId];
		ref SpellElementEffectComponentData elementComponentData = ref ssp.ElementComponentData;
		switch (spellConfig.abilityType)
		{
		case SpellAbilityType.FireCrystal:
			elementComponentData.FireBurnDuration = Mathf.Max(elementComponentData.FireBurnDuration, spellConfig.float2);
			elementComponentData.FireHpBurnPercent += (float)spellConfig.int1 / 100f;
			ssp.ConfigComponentData.Damage.AddRatio += spellConfig.float1 / 100f;
			break;
		case SpellAbilityType.VenomCrystal:
			elementComponentData.VenomDuration = Mathf.Max(elementComponentData.VenomDuration, spellConfig.float1);
			elementComponentData.VenomApplyCount += spellConfig.int2;
			break;
		case SpellAbilityType.MucusCrystal:
			elementComponentData.MucusDuration = Mathf.Max(elementComponentData.MucusDuration, spellConfig.float1);
			elementComponentData.MucusMoveSpeedRatio *= spellConfig.float2 / 100f;
			elementComponentData.MucusSpellSpeedRatio *= spellConfig.float3 / 100f;
			break;
		case SpellAbilityType.ThunderCrystal:
			elementComponentData.ThunderHitRadius = Mathf.Max(elementComponentData.ThunderHitRadius, spellConfig.float1);
			elementComponentData.ThunderHitDamageRatio += spellConfig.float2 / 100f;
			break;
		case SpellAbilityType.Frozen:
			elementComponentData.FrozenDuration += spellConfig.float1;
			break;
		case SpellAbilityType.DeathInfect:
		{
			RadiusAttributeValue radius = ssp.ConfigComponentData.Radius;
			radius.Base = spellConfig.float1;
			elementComponentData.VoidExplosionRange = Mathf.Max(elementComponentData.VoidExplosionRange, radius.Calculate());
			elementComponentData.VoidExplosionHpDamageRatio += spellConfig.float2 / 100f;
			elementComponentData.VoidInstantKillThreshold = Mathf.Max(elementComponentData.VoidInstantKillThreshold, spellConfig.float3 / 100f);
			break;
		}
		}
	}

	private static IEnumerable<float> CalculateRandomAngle(SpellShootInfo[] spells, (float add, float mul)[] finalScatterModify)
	{
		return spells.Zip(finalScatterModify).Select(delegate((SpellShootInfo Left, (float add, float mul) Right) e)
		{
			float num = (e.Left.ShootData.Spell.GetFinalConfig().angle + e.Right.add) * e.Right.mul;
			if (num < 0f)
			{
				num = 0f;
			}
			float result = UnityEngine.Random.Range(num * -0.5f, num * 0.5f);
			if (e.Left.IsCopyShoot)
			{
				result = UnityEngine.Random.Range(0f, 360f);
			}
			return result;
		});
	}

	private static IEnumerable<float> CalculateEqualScatterAngle(SpellShootInfo[] spells, (float add, float mul)[] finalScatterModify)
	{
		float maxScatter = (from e in spells.Zip(finalScatterModify)
			select (e.Left.ShootData.Spell.GetFinalConfig().angle + e.Right.add) * e.Right.mul).Max();
		if (maxScatter < 0f)
		{
			maxScatter = 0f;
		}
		int spellCount = spells.Length;
		return spells.Select(delegate(SpellShootInfo e, int i)
		{
			float t = (float)i / (float)(spellCount - 1);
			if (spellCount == 1)
			{
				t = 0.5f;
			}
			float result = Mathf.Lerp(maxScatter * -0.5f, maxScatter * 0.5f, t);
			if (e.IsCopyShoot)
			{
				result = UnityEngine.Random.Range(0f, 360f);
			}
			return result;
		});
	}

	private static void ApplyEqualScatterAngleToAroundSpells(IEnumerable<SpellBase> spells)
	{
		SpellBase[] array = spells.Where((SpellBase e) => e.InitialParameter.equalScatter && e.spellAroundOwnerRadius != 0f).ToArray();
		if (array.Length != 0)
		{
			float num = UnityEngine.Random.Range(0f, 360f);
			int num2 = 360 / array.Length;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].spellAroundOwnerCurrentAngle = (float)(i * num2) + num;
			}
		}
	}

	private static IEnumerable<Vector3> CalculateMultiShootEffectPosition(SpellShootInfo[] spells, Vector3 lookDirection)
	{
		Vector3 dir = Tool2D.GetDir(lookDirection, 90f);
		return spells.Select(delegate(SpellShootInfo e)
		{
			float num = (float)(e.MultiShootCount - 1) * e.MultiShootGap * -0.5f + e.MultiShootGap * (float)e.InMultiShootIndex;
			return e.CreatePosition + num * dir;
		});
	}

	private static void ApplyScatterAngleToSpellSIP(ShootSpellSpatialInfo spatialInfo, float angle, SpellInitialParameter parameter)
	{
		if (!spatialInfo.Target.HasValue)
		{
			goto IL_0099;
		}
		if (!parameter.spellIsFall)
		{
			SpellAbilityType abilityType = parameter.initializedSpellCfg.abilityType;
			if (abilityType != SpellAbilityType.Meteor && abilityType != SpellAbilityType.DeathAdder)
			{
				goto IL_0099;
			}
		}
		Vector3 value = spatialInfo.Target.Value;
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		onUnitSphere.z = 0f;
		value += onUnitSphere * (angle * 0.06f);
		Vector3 direction = value - spatialInfo.Start;
		direction.z = 0f;
		ShootSpellSpatialInfo shootSpellSpatialInfo = ShootSpellSpatialInfo.ToPoint(spatialInfo.Start, value, direction);
		goto IL_00b1;
		IL_00b1:
		parameter.shootDirection = shootSpellSpatialInfo.Direction;
		parameter.finalShootSpatialInfo = shootSpellSpatialInfo;
		if (parameter.reverseDirection)
		{
			parameter.shootDirection *= -1f;
		}
		return;
		IL_0099:
		shootSpellSpatialInfo = ShootSpellSpatialInfo.ByDirection(spatialInfo.Start, Tool2D.GetDir(parameter.originShootDirection, angle));
		goto IL_00b1;
	}

	private static void ApplyMultiShootEffectToFallTarget(SpellShootInfo spell, SpellInitialParameter parameter)
	{
		if (parameter.spellIsFall && parameter != null)
		{
			ShootSpellSpatialInfo finalShootSpatialInfo = parameter.finalShootSpatialInfo;
			if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
			{
				Vector3 dir = Tool2D.GetDir(parameter.finalShootSpatialInfo.Direction, 90f);
				float num = (float)(spell.MultiShootCount - 1) * spell.MultiShootGap * -0.5f + spell.MultiShootGap * (float)spell.InMultiShootIndex;
				ShootSpellSpatialInfo finalShootSpatialInfo2 = parameter.finalShootSpatialInfo;
				finalShootSpatialInfo2.Target += num * dir;
			}
		}
	}

	private static IEnumerable<SpellShootInfo> CreateShootInfoFromShootGroup(SpellShootGroup group, Vector3 position, float copyShootRate)
	{
		return group.Shoots.SelectMany(delegate(SpellShootData e)
		{
			IEnumerable<SpellShootInfo> enumerable = CreateShootInfoFromShootData(e, position, isCopyShoot: false);
			return (!(UnityEngine.Random.Range(0f, 1f) < copyShootRate)) ? enumerable : enumerable.Concat(CreateShootInfoFromShootData(e, position, isCopyShoot: true));
		});
	}

	private static IEnumerable<SpellShootInfo> CreateShootInfoFromShootData(SpellShootData shoot, Vector3 position, bool isCopyShoot)
	{
		SpellConfig config = shoot.Spell.GetFinalConfig();
		(int count, float gap) multiShootData = shoot.GetSpellMultiShootData();
		if (config.abilityType == SpellAbilityType.BiAnLethalBlade || config.abilityType == SpellAbilityType.LaserBeam || config.abilityType == SpellAbilityType.BlueRune || config.abilityType == SpellAbilityType.RedRune || config.abilityType == SpellAbilityType.GreenRune)
		{
			multiShootData.count = 1;
			multiShootData.gap = 0f;
		}
		int multiShootCount = multiShootData.count;
		float fps = GameMgr.Inst.GetFps();
		float lowFrameMultiSHootDamageRatio = 1f;
		float lowFpsActiveThreshold = SpellTools.GetLowFpsActiveThreshold(GameMgr.IsMobile_Static);
		float maxOptimizeFPSThreshold = SpellTools.GetMaxOptimizeFPSThreshold(GameMgr.IsMobile_Static);
		int num = (GameMgr.IsMobile_Static ? 40 : 60);
		int threshold = (GameMgr.IsMobile_Static ? 60 : 80);
		if (multiShootCount > 1 && ((GeneralTool.IsLowFpsOptimizeActive(lowFpsActiveThreshold) && GameMgr.Inst.GetFps() <= lowFpsActiveThreshold) || SpellShootSystem.CurrentFrameMultiShootCount >= num) && config.useType != SpellType.Passive)
		{
			float num2 = math.floor((float)multiShootCount * fps / lowFpsActiveThreshold);
			num2 = SpellTools.GetFinalSpawnCountWithLimitCount(num, 2, threshold, 1, SpellShootSystem.CurrentFrameMultiShootCount, (int)num2);
			if (GameMgr.Inst.GetFps() <= maxOptimizeFPSThreshold || num2 < 1f)
			{
				num2 = 1f;
			}
			lowFrameMultiSHootDamageRatio = (float)multiShootCount / num2;
			multiShootCount = (int)num2;
			int num3 = SpellTools.CalculateSpellComplexity(config.abilityType);
			SpellShootSystem.CurrentFrameMultiShootCount += num3 * multiShootCount;
		}
		for (int inShootCountIndex = 0; inShootCountIndex < config.shootCount; inShootCountIndex++)
		{
			for (int inMultiShootIndex = 0; inMultiShootIndex < multiShootCount; inMultiShootIndex++)
			{
				yield return new SpellShootInfo
				{
					SpellId = shoot.Spell.GetFinalConfig().id,
					ShootData = shoot,
					MultiShootCount = multiShootCount,
					MultiShootGap = multiShootData.gap,
					InMultiShootIndex = inMultiShootIndex,
					ShootCount = config.shootCount,
					InShootCountIndex = inShootCountIndex,
					CreatePosition = position,
					IsCopyShoot = isCopyShoot,
					LowFrameMultiShootDamageRatio = lowFrameMultiSHootDamageRatio,
					MultiCastSpellEfficiency = lowFrameMultiSHootDamageRatio
				};
			}
		}
	}
}
