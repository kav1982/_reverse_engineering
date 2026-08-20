using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
public struct UnitPropertyJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitDead> __UnitDead_RW_ComponentTypeHandle;

			[ReadOnly]
			public BufferTypeHandle<UnitMREttBED> __UnitMREttBED_RO_BufferTypeHandle;

			public BufferTypeHandle<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__UnitDead_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitDead>();
				__UnitMREttBED_RO_BufferTypeHandle = state.GetBufferTypeHandle<UnitMREttBED>(isReadOnly: true);
				__TakeDamageInfo_Dots_RW_BufferTypeHandle = state.GetBufferTypeHandle<TakeDamageInfo_Dots>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__UnitDead_RW_ComponentTypeHandle.Update(ref state);
				__UnitMREttBED_RO_BufferTypeHandle.Update(ref state);
				__TakeDamageInfo_Dots_RW_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitMREttBED>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitDead>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TakeDamageInfo_Dots>();
			DefaultQuery = entityQueryBuilder2.Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		public void Init(ref SystemState state, bool assignDefaultQuery)
		{
			if (assignDefaultQuery)
			{
				__AssignQueries(ref state);
			}
			__TypeHandle.__AssignHandles(ref state);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Run(ref UnitPropertyJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref UnitPropertyJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref UnitPropertyJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref UnitPropertyJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref UnitPropertyJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref UnitPropertyJob job, EntityManager entityManager)
		{
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InternalCompiler
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		public static void CheckForErrors(int scheduleType)
		{
		}
	}

	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> cluLocalTsf;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideColor> cluMOC;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<TeammateData> TeammateDataLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<PhysicsCollider> ColliderLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<PlayerController_Dots> playerControllerLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> unitPptLookUp;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	[ReadOnly]
	public SpellSingleton spellSingleton;

	public EntityCommandBuffer.ParallelWriter ecb;

	public Entity textFloatVFXBufferEtt;

	public Entity uiTextFloatByJobBufferEtt;

	public Entity getGOByJobEtt;

	public Entity TeammateGhostEffectEntity;

	public Entity SpellEffectEntity;

	public Entity DamageRecordSingletonBufferEntity;

	public bool settingNeedTextFloat;

	public bool isSupportVFX;

	public float deltaTime;

	public float relic_SeckillChance;

	public bool relic_ResurgenceExist;

	public float curse_MonsterRecoverHPPerSecond;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, ref UnitProperty_Dots unitPpt, ref PhysicsVelocity velocity, ref UnitDead unitDead, in DynamicBuffer<UnitMREttBED> mrBuffer, DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer, Entity entity)
	{
		if (!unitPpt.isInitialed)
		{
			return;
		}
		if (unitPpt.disabled)
		{
			if (!unitPpt.IsImmuneKnockback)
			{
				if (unitPpt.unitCfg.unitType == UnitType.Player)
				{
					unitPpt.currentKnockback += unitPpt.thisFrameKnockback;
					if (unitPpt.IsVelocityDeclice)
					{
						unitPpt.currentKnockback = DTool.Lerp(in unitPpt.currentKnockback, in float3.zero, 5f * deltaTime);
					}
				}
				else
				{
					velocity.Linear += unitPpt.thisFrameKnockback;
				}
			}
			if (unitPpt.unitCfg.unitType != 0 && !DTool.IsTotallySame(in velocity.Linear, in float3.zero) && unitPpt.IsVelocityDeclice)
			{
				velocity.Linear = DTool.Lerp(in velocity.Linear, in float3.zero, 5f * deltaTime);
			}
			unitPpt.thisFrameKnockback = 0;
			return;
		}
		RefRO<LocalTransform> refRO = cluLocalTsf.GetRefRO(entity);
		for (int i = 0; i < takeDamageBuffer.Length; i++)
		{
			ref TakeDamageInfo_Dots reference = ref takeDamageBuffer.ElementAt(i);
			if (unitPpt.IsInvincible)
			{
				reference.immuneDamage = true;
			}
			if (unitPpt.unitCfg.isSolidObj && !reference.isUndifferDamage)
			{
				reference.immuneDamage = true;
			}
			if (unitPpt.isDead)
			{
				reference.targetAlreadyDeadBeforeDamage = true;
				continue;
			}
			if (unitPpt.unitCfg.unitType == UnitType.Player)
			{
				if (!reference.ignorePlayerInvincibleFrame && unitPpt.IsPlayerInInvincibleFrame)
				{
					reference.knockbackForce = Vector3.zero;
					reference.immuneDamage = true;
				}
				reference.damage = Mathf.Ceil(reference.damage);
			}
			if (reference.attackerType == AttackerType.FromUI)
			{
				reference.immuneDamage = false;
			}
			if (reference.immuneDamage || reference.damage <= 0f)
			{
				unitPpt.TakeKnockback(reference.knockbackForce);
				continue;
			}
			if (reference.spell.Entity != Entity.Null)
			{
				if (reference.spell.ElementEffect.VenomApplyCount > 0f)
				{
					unitPpt.SetVenom(reference.spell.ElementEffect.VenomDuration, reference.spell.ElementEffect.VenomApplyCount);
				}
				if (reference.spell.ElementEffect.FrozenDuration > 0f)
				{
					unitPpt.SetFrozen(reference.spell.ElementEffect.FrozenDuration);
				}
				if (reference.spell.ElementEffect.MucusDuration > 0f)
				{
					unitPpt.SetMucus(reference.spell.ElementEffect.MucusDuration, reference.spell.ElementEffect.MucusMoveSpeedRatio, reference.spell.ElementEffect.MucusSpellSpeedRatio);
				}
				if (reference.spell.ElementEffect.FireBurnDuration > 0f)
				{
					unitPpt.SetBurn(reference.spell.ElementEffect.FireBurnDuration, reference.spell.ElementEffect.FireHpBurnPercent);
				}
				if (reference.spell.Config.AbilityType == SpellAbilityType.Bullet && reference.spell.Config.Float2 > 0f)
				{
					unitPpt.BonusTakeDamageRatioRegister(reference.spell.Config.Float1, reference.spell.Config.Float2);
				}
				if (reference.spell.ElementEffect.VoidExplosionHpDamageRatio > 0f)
				{
					Spell3129VoidExplosion.VoidExplosionData_Dots voidExplosionData_Dots = default(Spell3129VoidExplosion.VoidExplosionData_Dots);
					voidExplosionData_Dots.ExplosionRange = reference.spell.ElementEffect.VoidExplosionRange;
					voidExplosionData_Dots.HpToDmgRatio = reference.spell.ElementEffect.VoidExplosionHpDamageRatio;
					voidExplosionData_Dots.InstantKillRatio = reference.spell.ElementEffect.VoidInstantKillThreshold;
					Spell3129VoidExplosion.VoidExplosionData_Dots @void = voidExplosionData_Dots;
					unitPpt.SetVoid(@void);
				}
			}
			if (reference.damage > 0f)
			{
				UnitType unitType = unitPpt.unitCfg.unitType;
				if (unitType != UnitType.Brittleness && unitType != UnitType.NotAttack)
				{
					DamageRecordBuffer damageRecordBuffer = default(DamageRecordBuffer);
					damageRecordBuffer.Damage = reference.damage;
					damageRecordBuffer.SpellOrRelicId = reference.damageRecordId;
					damageRecordBuffer.HitUnitId = unitPpt.unitCfg.id;
					DamageRecordBuffer element = damageRecordBuffer;
					if (reference.spell.Entity != default(Entity) && DTool.IsSameCamp(reference.spell.Config.ShooterType, UnitType.Player))
					{
						int abilityType = (int)reference.spell.Config.AbilityType;
						if (abilityType > 1000 && abilityType < 9000 && reference.damageRecordId == 0)
						{
							element.SpellOrRelicId = abilityType;
						}
					}
					if (element.SpellOrRelicId > 0)
					{
						ecb.AppendToBuffer(index, DamageRecordSingletonBufferEntity, element);
					}
				}
			}
			float damage = reference.damage;
			float3 f = reference.knockbackForce;
			if (!DTool.IsEqual(in f, in float3.zero))
			{
				unitPpt.TakeKnockback(f);
			}
			if (reference.damage > 0f && unitPpt.unitCfg.shieldTemp > 0f)
			{
				float shieldTemp = unitPpt.unitCfg.shieldTemp;
				if (reference.damage > unitPpt.unitCfg.shieldTemp)
				{
					unitPpt.unitCfg.shieldTemp = 0f;
					if (unitPpt.unitCfg.unitType == UnitType.Player)
					{
						playerControllerLookUp.GetRefRW(entity).ValueRW.needUpdateTempShieldUI = true;
					}
					reference.damage -= shieldTemp;
					reference.realDamage += shieldTemp;
				}
				else
				{
					unitPpt.unitCfg.shieldTemp -= reference.damage;
					if (unitPpt.unitCfg.unitType == UnitType.Player)
					{
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerLostTempShield,
								worldPos = refRO.ValueRO.Position
							});
						}
						else
						{
							ecb.AppendToBuffer(index, uiTextFloatByJobBufferEtt, new UITextFloatByJobBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerLostTempShield,
								worldPos = refRO.ValueRO.Position
							});
						}
						playerControllerLookUp.GetRefRW(entity).ValueRW.needUpdateTempShieldUI = true;
					}
					reference.realDamage += reference.damage;
					reference.damage = 0f;
				}
			}
			if (reference.damage > 0f && unitPpt.unitCfg.shield > 0f)
			{
				float shield = unitPpt.unitCfg.shield;
				if (reference.damage > unitPpt.unitCfg.shield)
				{
					unitPpt.unitCfg.shield = 0f;
					if (unitPpt.unitCfg.unitType == UnitType.Player)
					{
						playerControllerLookUp.GetRefRW(entity).ValueRW.needUpdateShieldUI = true;
					}
					reference.damage -= shield;
					reference.realDamage += shield;
				}
				else
				{
					unitPpt.unitCfg.shield -= reference.damage;
					if (unitPpt.unitCfg.unitType == UnitType.Player)
					{
						playerControllerLookUp.GetRefRW(entity).ValueRW.needUpdateShieldUI = true;
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerLostShield,
								worldPos = refRO.ValueRO.Position
							});
						}
						else
						{
							ecb.AppendToBuffer(index, uiTextFloatByJobBufferEtt, new UITextFloatByJobBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerLostShield,
								worldPos = refRO.ValueRO.Position
							});
						}
					}
					reference.realDamage += reference.damage;
					reference.damage = 0f;
				}
				playerControllerLookUp.GetRefRW(entity).ValueRW.isHurtThisFrameForPlayer = true;
			}
			if (reference.damage > 0f)
			{
				reference.realDamage += math.clamp(reference.damage, 0f, unitPpt.unitCfg.currentHP);
				unitPpt.unitCfg.currentHP -= reference.damage;
				if ((settingNeedTextFloat || unitPpt.unitCfg.unitType == UnitType.Player) && !reference.ignoreFloatText && unitPpt.unitCfg.unitType != UnitType.Brittleness)
				{
					if (reference.isDamageCritical)
					{
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.Critical,
								worldPos = refRO.ValueRO.Position
							});
						}
					}
					else if (reference.attackerType == AttackerType.Venom)
					{
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.Poison,
								worldPos = refRO.ValueRO.Position
							});
						}
					}
					else if (reference.attackerType == AttackerType.Burn)
					{
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.Burn,
								worldPos = refRO.ValueRO.Position
							});
						}
					}
					else if (unitPpt.unitCfg.unitType == UnitType.Player || unitPpt.unitCfg.unitType == UnitType.Teammate || unitPpt.unitCfg.unitType == UnitType.TeammateNotAttack)
					{
						if (isSupportVFX)
						{
							ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerTakeDamage,
								worldPos = refRO.ValueRO.Position
							});
						}
						else if (unitPpt.unitCfg.unitType == UnitType.Player)
						{
							ecb.AppendToBuffer(index, uiTextFloatByJobBufferEtt, new UITextFloatByJobBED
							{
								number = reference.damage,
								type = UITextFloatType.PlayerTakeDamage,
								worldPos = refRO.ValueRO.Position
							});
						}
					}
					else if (isSupportVFX)
					{
						ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
						{
							number = reference.damage,
							type = UITextFloatType.Damage,
							worldPos = refRO.ValueRO.Position
						});
					}
				}
				if (unitPpt.unitCfg.unitType == UnitType.Player)
				{
					playerControllerLookUp.GetRefRW(entity).ValueRW.isPlayerDropBlood = true;
					playerControllerLookUp.GetRefRW(entity).ValueRW.isHurtThisFrameForPlayer = true;
				}
				if (unitPpt.unitCfg.unitType != 0 && unitPpt.voidEffectTimer > 0f)
				{
					float num = unitPpt.voidExplosionData.InstantKillRatio;
					UnitType unitType = unitPpt.unitCfg.unitType;
					if (unitType == UnitType.Elite || unitType == UnitType.Boss)
					{
						num /= 2f;
					}
					if (unitPpt.unitCfg.currentHP / unitPpt.unitCfg.maxHP < num && !unitPpt.IsInvincible)
					{
						if (unitPpt.unitCfg.currentHP > 0f)
						{
							ecb.AppendToBuffer(index, DamageRecordSingletonBufferEntity, new DamageRecordBuffer
							{
								Damage = unitPpt.unitCfg.currentHP,
								HitUnitId = unitPpt.unitCfg.id,
								SpellOrRelicId = 3129
							});
						}
						unitPpt.unitCfg.currentHP = 0f;
					}
				}
				if (unitPpt.unitCfg.currentHP <= 0f)
				{
					if (unitPpt.unitCfg.unitType == UnitType.Player && relic_ResurgenceExist && reference.attackerType != AttackerType.FromUI)
					{
						playerControllerLookUp.GetRefRW(entity).ValueRW.isTriggerResurgence = true;
					}
					else if (TeammateDataLookup.HasComponent(entity))
					{
						ecb.TeammateDeadTryActiveTeammateDelayDeathEffect(ref unitPpt, ref TeammateDataLookup, entity, SpellEffectEntity, index, ColliderLookUp, TeammateGhostEffectEntity);
					}
					else
					{
						unitPpt.isDead = true;
						reference.isTargetDead = true;
						if (unitPpt.unitCfg.triggerDeadEvent)
						{
							reference.isTriggerDeadEvent = true;
						}
						unitDead.deadlyInfo = reference;
						unitDead.deadlyInfoIndex = i;
					}
				}
				else
				{
					float3 f2 = DTool.GetDir(ref gRandom.ValueRW.random);
					if (!DTool.IsTotallySame(in reference.knockbackForce, in float3.zero))
					{
						f2 = math.normalizesafe(reference.knockbackForce);
					}
					if (!DTool.IsEqual(in f2, in float3.zero))
					{
						unitPpt.BeHitShake(f2);
					}
					if (!reference.ignoreBeHitColor)
					{
						unitPpt.SetBeHitColor();
					}
					switch (unitPpt.unitCfg.unitType)
					{
					case UnitType.Monster:
					case UnitType.WillAttack:
					case UnitType.NotAttack:
					case UnitType.Brittleness:
						if (relic_SeckillChance > 0f && !reference.ignoreRelicSeckill && reference.attackerEntity != Entity.Null && unitPptLookUp.HasComponent(reference.attackerEntity) && unitPptLookUp[reference.attackerEntity].unitCfg.IsSameCamp(UnitType.Player) && DTool.RandomValue(ref gRandom.ValueRW.random) <= relic_SeckillChance && unitPpt.unitCfg.unitType == UnitType.Monster)
						{
							ecb.AppendToBuffer(index, getGOByJobEtt, new GetGOByJobBED
							{
								path = "Prefabs/Item/Relic_Seckill",
								worldPos = refRO.ValueRO.Position,
								duration = 2f
							});
							TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(reference.attackerEntity);
							elem.damage = 9999f;
							elem.ignoreRelicSeckill = true;
							takeDamageBuffer.Add(elem);
						}
						break;
					}
				}
			}
			if (unitPpt.unitCfg.unitType == UnitType.Player && !unitPpt.isDead && damage > 0f && !reference.ignorePlayerInvincibleFrame)
			{
				unitPpt.PlayerIntoInvisibleFrame();
			}
		}
		if (unitPpt.unitCfg.id == 800001 && unitPpt.playerInvincibleFrameTimer > 0f)
		{
			unitPpt.playerInvincibleFrameTimer -= deltaTime;
			if (unitPpt.playerInvincibleFrameTimer < 0f)
			{
				unitPpt.playerInvincibleFrameTwinkleTimer = 0f;
				unitPpt.playerInvincibleFrameTwinkleAlpha = 1f;
				unitPpt.ChangeAlpha(unitPpt.playerInvincibleFrameTwinkleAlpha);
			}
			else
			{
				unitPpt.playerInvincibleFrameTwinkleTimer += deltaTime;
				if (unitPpt.playerInvincibleFrameTwinkleTimer >= 0.05f)
				{
					unitPpt.playerInvincibleFrameTwinkleTimer = 0f;
					unitPpt.playerInvincibleFrameTwinkleAlpha = ((unitPpt.playerInvincibleFrameTwinkleAlpha == 1f) ? 0.3f : 1f);
					unitPpt.ChangeAlpha(unitPpt.playerInvincibleFrameTwinkleAlpha);
				}
			}
		}
		if (!unitPpt.IsImmuneKnockback)
		{
			if (unitPpt.unitCfg.unitType == UnitType.Player)
			{
				unitPpt.currentKnockback += unitPpt.thisFrameKnockback;
				if (unitPpt.IsVelocityDeclice)
				{
					unitPpt.currentKnockback = math.lerp(unitPpt.currentKnockback, float3.zero, 5f * deltaTime);
				}
			}
			else
			{
				velocity.Linear += unitPpt.thisFrameKnockback;
			}
		}
		if (unitPpt.unitCfg.unitType != 0 && !DTool.IsTotallySame(in velocity.Linear, in float3.zero) && unitPpt.IsVelocityDeclice)
		{
			velocity.Linear = math.lerp(velocity.Linear, float3.zero, 5f * deltaTime);
		}
		unitPpt.thisFrameKnockback = 0;
		if (unitPpt.isBeHitShake)
		{
			if (unitPpt.beHitNeedReset)
			{
				unitPpt.beHitNeedReset = false;
				unitPpt.beHitCurrentOffsetAmount = 0f;
			}
			float maxDelta = 6f * unitPpt.unitCfg.beHitRatio * deltaTime;
			if (unitPpt.beHitShakeOut)
			{
				float num2 = 0.2f * unitPpt.unitCfg.beHitRatio;
				unitPpt.beHitCurrentOffsetAmount = DTool.MoveTowards(unitPpt.beHitCurrentOffsetAmount, num2, maxDelta);
				if (unitPpt.beHitCurrentOffsetAmount == num2)
				{
					unitPpt.beHitShakeOut = false;
				}
			}
			else
			{
				unitPpt.beHitCurrentOffsetAmount = DTool.MoveTowards(unitPpt.beHitCurrentOffsetAmount, 0f, maxDelta);
				if (unitPpt.beHitCurrentOffsetAmount == 0f)
				{
					unitPpt.isBeHitShake = false;
				}
			}
			if (!unitPpt.unitCfg.isHybirdUnit && unitPpt.unitCfg.unitType != 0 && unitPpt.ett_BeHit != Entity.Null)
			{
				cluLocalTsf.GetRefRW(unitPpt.ett_BeHit).ValueRW.Position = unitPpt.beHitDir * unitPpt.beHitCurrentOffsetAmount;
			}
		}
		unitPpt.needSyncColor = false;
		if (unitPpt.isBeHitColor)
		{
			unitPpt.beHitColorTimer -= deltaTime;
			if (unitPpt.beHitColorTimer < 0f)
			{
				unitPpt.beHitColorTimer = 0f;
				unitPpt.isBeHitColor = false;
				for (int j = 0; j < mrBuffer.Length; j++)
				{
					if (cluMOC.HasComponent(mrBuffer[j].ett))
					{
						cluMOC.GetRefRW(mrBuffer[j].ett).ValueRW.color = Color.white;
					}
				}
				unitPpt.needSyncColor = true;
				unitPpt.baseColor = Color.white;
				Color color = Color.white;
				if (unitPpt.affect_IsMucusHit)
				{
					color = GameConst.color_BodyMucus;
				}
				if (unitPpt.affect_burnDurationTimer > 0f)
				{
					color = GameConst.color_BodyBurn;
				}
				if (unitPpt.affect_VenomDurationTimer > 0f)
				{
					color = GameConst.color_BodyVenom;
				}
				if (unitPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
				{
					color = GameConst.color_BodyFrozen;
				}
				unitPpt.ChangeColor(color);
			}
		}
		if (unitPpt.needChangeColor)
		{
			unitPpt.needChangeColor = false;
			for (int k = 0; k < mrBuffer.Length; k++)
			{
				if (cluMOC.HasComponent(mrBuffer[k].ett))
				{
					cluMOC.GetRefRW(mrBuffer[k].ett).ValueRW.color = unitPpt.baseColor;
				}
			}
			unitPpt.needSyncColor = true;
		}
		if (!(curse_MonsterRecoverHPPerSecond > 0f) || !unitPpt.unitCfg.IsSameCamp(UnitType.Monster) || !(unitPpt.unitCfg.currentHP < unitPpt.unitCfg.maxHP))
		{
			return;
		}
		unitPpt.curse_MonsterRecoveryTimer += deltaTime;
		if (unitPpt.curse_MonsterRecoveryTimer >= 1f)
		{
			unitPpt.curse_MonsterRecoveryTimer -= 1f;
			unitPpt.unitCfg.currentHP += curse_MonsterRecoverHPPerSecond;
			if (unitPpt.unitCfg.currentHP > unitPpt.unitCfg.maxHP)
			{
				unitPpt.unitCfg.currentHP = unitPpt.unitCfg.maxHP;
			}
			if (isSupportVFX)
			{
				ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
				{
					number = curse_MonsterRecoverHPPerSecond,
					type = UITextFloatType.Recover,
					worldPos = refRO.ValueRO.Position
				});
			}
			spellSingleton.Prefabs.TryGetValue("3108_Heal", out var item);
			Entity e = ecb.Instantiate(index, item);
			ecb.SetComponent(index, e, new FollowEntity
			{
				ett = entity
			});
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitDead_RW_ComponentTypeHandle);
		BufferAccessor<UnitMREttBED> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__UnitMREttBED_RO_BufferTypeHandle);
		BufferAccessor<TakeDamageInfo_Dots> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref UnitProperty_Dots unitPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, i);
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, i);
				ref UnitDead unitDead = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr3, i);
				DynamicBuffer<UnitMREttBED> mrBuffer = bufferAccessor[i];
				DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer = bufferAccessor2[i];
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
				Execute(chunkIndexInQuery, ref unitPpt, ref velocity, ref unitDead, in mrBuffer, takeDamageBuffer, entity);
				num++;
			}
			return;
		}
		if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
		{
			int nextRangeBegin = 0;
			int nextRangeEnd = 0;
			while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, nextRangeEnd, out nextRangeBegin, out nextRangeEnd))
			{
				while (nextRangeBegin < nextRangeEnd)
				{
					ref UnitProperty_Dots unitPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, nextRangeBegin);
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, nextRangeBegin);
					ref UnitDead unitDead2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr3, nextRangeBegin);
					DynamicBuffer<UnitMREttBED> mrBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer2 = bufferAccessor2[nextRangeBegin];
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
					Execute(chunkIndexInQuery, ref unitPpt2, ref velocity2, ref unitDead2, in mrBuffer2, takeDamageBuffer2, entity2);
					nextRangeBegin++;
					num++;
				}
			}
			return;
		}
		ulong num2 = chunkEnabledMask.ULong0;
		int num3 = math.min(64, count);
		for (int j = 0; j < num3; j++)
		{
			if ((num2 & 1) != 0L)
			{
				ref UnitProperty_Dots unitPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, j);
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, j);
				ref UnitDead unitDead3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr3, j);
				DynamicBuffer<UnitMREttBED> mrBuffer3 = bufferAccessor[j];
				DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer3 = bufferAccessor2[j];
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
				Execute(chunkIndexInQuery, ref unitPpt3, ref velocity3, ref unitDead3, in mrBuffer3, takeDamageBuffer3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref UnitProperty_Dots unitPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, k);
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, k);
				ref UnitDead unitDead4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr3, k);
				DynamicBuffer<UnitMREttBED> mrBuffer4 = bufferAccessor[k];
				DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer4 = bufferAccessor2[k];
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
				Execute(chunkIndexInQuery, ref unitPpt4, ref velocity4, ref unitDead4, in mrBuffer4, takeDamageBuffer4, entity4);
				num++;
			}
			num2 >>= 1;
		}
	}

	private JobHandle __ThrowCodeGenException()
	{
		throw new Exception("This method should have been replaced by source gen.");
	}

	public void Run()
	{
		__ThrowCodeGenException();
	}

	public void RunByRef()
	{
		__ThrowCodeGenException();
	}

	public void Run(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void RunByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public JobHandle Schedule(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleByRef(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public void Schedule()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleByRef()
	{
		__ThrowCodeGenException();
	}

	public void Schedule(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void ScheduleByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
	{
		return __ThrowCodeGenException();
	}

	public void ScheduleParallel()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallelByRef()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallel(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallelByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
	}
}
