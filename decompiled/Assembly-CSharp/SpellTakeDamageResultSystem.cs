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
[UpdateBefore(typeof(UnitTakeDamageClearSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitTakeDamageDeadSystem))]
[CompilerGenerated]
public struct SpellTakeDamageResultSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct SpellTakeDamageResultJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				public BufferTypeHandle<SpellHitEntity> __SpellHitEntity_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__SpellHitEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<SpellHitEntity>();
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__SpellHitEntity_RW_BufferTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHitEntity>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
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
			public void Run(ref SpellTakeDamageResultJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref SpellTakeDamageResultJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref SpellTakeDamageResultJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref SpellTakeDamageResultJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref SpellTakeDamageResultJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref SpellTakeDamageResultJob job, EntityManager entityManager)
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

		public Entity Spell3112Buffer;

		public Entity Spell4025Buffer;

		public Entity Spell1006HitTargetsPositionBufferEntity;

		public Entity SpellSpawnParamsBufferEntity;

		public Entity OnHitWandChargeEventEntity;

		public Entity OnCriticalWandChargeEventEntity;

		public Entity OnKillWandChargeEventEntity;

		public Entity OnHighDamageWandChargeEventEntity;

		public Entity OnKillDropCoinOrCrystalEventEntity;

		public EntityCommandBuffer.ParallelWriter CMD;

		[ReadOnly]
		public BufferLookup<TakeDamageInfo_Dots> TakeDamageInfoLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public BufferLookup<SpellRefractionHitEntities> SpellRefractionHitEntitiesLookup;

		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellRefractionData> SpellRefractionLookup;

		[NativeDisableContainerSafetyRestriction]
		[ReadOnly]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertiesLookup;

		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<Spell1010SnakeData> Spell1010SnakeLookup;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		public Entity SEPlayerSingleton;

		public Entity GlobalParticleSingleton;

		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellHitTriggerComponentData> SpellHitTriggerLookup;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[ReadOnly]
		public ComponentLookup<SpellOnKillDropCoin> OnKillDropCoinLookup;

		[ReadOnly]
		public ComponentLookup<SpellOnKillDropCrystal> OnKillDropCrystalLookup;

		public GlobalRandom Random;

		public Entity RecheckRefractDirectionBuffer;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(SpellAspect spell, DynamicBuffer<SpellHitEntity> hitEntities, [ChunkIndexInQuery] int chunkIndex)
		{
			foreach (SpellHitEntity item in hitEntities)
			{
				if (!TakeDamageInfoLookup.HasBuffer(item.Entity))
				{
					UnityEngine.Debug.LogError("为什么目标身上没有 TakeDamageInfo Buffer？");
					continue;
				}
				foreach (TakeDamageInfo_Dots item2 in TakeDamageInfoLookup[item.Entity])
				{
					if (!(item2.spell.Entity != spell.Entity) && !item2.targetAlreadyDeadBeforeDamage)
					{
						OnMakeDamage(spell, item.Entity, item2, chunkIndex);
					}
				}
			}
			hitEntities.Clear();
			if (spell.Config.ValueRO.Penetrate.Calculate() < 0)
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, spell.Entity, value: true);
			}
		}

		[BurstCompile]
		private void OnMakeDamage(SpellAspect spell, Entity targetEntity, TakeDamageInfo_Dots info, int chunkIndex)
		{
			if (spell.Data.ValueRO.PlayHitSE && !info.spell.IgnoreHitEffect)
			{
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				int prefabId = spell.Data.ValueRO.PrefabId;
				FixedString32Bytes seName = "Hit";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(prefabId, in seName)));
			}
			UnitProperty_Dots unitProperty_Dots = UnitPropertiesLookup[targetEntity];
			if (unitProperty_Dots.unitCfg.unitType != UnitType.Brittleness && !info.spell.IgnoreHitEffect)
			{
				if (spell.Config.ValueRO.Id / 10 == 9004)
				{
					float3 rootPosition = TransformLookup[targetEntity].Position;
					float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					info.spell.HitPosition = rootPosition + math.abs(layerPosition);
				}
				ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
				ref SpellSingleton spellSingleton = ref SpellSingleton;
				ref readonly SpellConfigComponentData valueRO = ref spell.Config.ValueRO;
				ref readonly SpellComponentData valueRO2 = ref spell.Data.ValueRO;
				ref float3 hitPosition = ref info.spell.HitPosition;
				float3 direction = spell.Movement.ValueRO.Direction;
				cMD2.CreateSpellHitEffect(chunkIndex, in spellSingleton, in valueRO, in valueRO2, in hitPosition, in direction, spell.Transform.ValueRO.Scale);
			}
			if (SpellHitTriggerLookup.HasComponent(spell.Entity) && SpellHitTriggerLookup.IsComponentEnabled(spell.Entity))
			{
				ref SpellHitTriggerComponentData valueRW = ref SpellHitTriggerLookup.GetRefRW(spell.Entity).ValueRW;
				if (valueRW.CooldownOver)
				{
					valueRW.ResetCooldown();
					valueRW.NeedTrigger = true;
					valueRW.TriggerPoint = TransformLookup[targetEntity].Position;
				}
			}
			switch (spell.Config.ValueRO.AbilityType)
			{
			case SpellAbilityType.Rollball:
				if (spell.Config.ValueRO.Penetrate.Calculate() <= 0 && info.isTargetDead && info.spell.CostPenetrate)
				{
					spell.Config.ValueRW.Damage.Extra -= math.min(info.realDamage, spell.Config.ValueRW.Damage.Calculate());
					if (spell.Config.ValueRW.Damage.Calculate() > 0f)
					{
						spell.Config.ValueRW.Penetrate.Base++;
						CMD.SetComponentEnabled<SpellNeedResize>(chunkIndex, spell.Entity, value: true);
					}
					else
					{
						CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, spell.Entity, value: true);
					}
				}
				break;
			case SpellAbilityType.HoverTorch:
				CMD.AppendToBuffer(chunkIndex, Spell1006HitTargetsPositionBufferEntity, new Spell1006HitPositionData
				{
					Camp = info.spell.Config.ShooterType,
					HitPositions = info.spell.HitPosition
				});
				break;
			case SpellAbilityType.BlackHole:
				if (DTool.IsSameCamp(spell.Config.ValueRO.ShooterType, UnitType.Player))
				{
					float3 @float = DTool.IgnoreZDir(in spell.Transform.ValueRO.Position, in info.spell.HitPosition);
					UnitPropertiesLookup.GetRefRW(targetEntity).ValueRW.TakeKnockback(@float * spell.Config.ValueRO.Float2 * spell.Config.ValueRO.DamageInterval);
				}
				break;
			case SpellAbilityType.ArcaneExplosion:
				if (spell.Config.ValueRO.Int1 != 0 && info.isTargetDead && info.isTriggerDeadEvent)
				{
					SpellSpawnParams element = SpellSingleton.SpellSpawnParamsStorage[spell.Entity].BuildArcaneExplosion(TransformLookup.GetRefRO(targetEntity).ValueRO.Position, targetEntity.Index);
					CMD.AppendToBuffer(chunkIndex, SpellSpawnParamsBufferEntity, element);
				}
				break;
			case SpellAbilityType.DeathAdder:
				if (UnitPropertiesLookup[targetEntity].unitCfg.unitType != UnitType.NotAttack && info.isTargetDead)
				{
					spell.Config.ValueRO.ColorType.ColorEnumToString(out var result);
					CMD.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
					{
						Position = TransformLookup.GetRefRO(targetEntity).ValueRO.Position + new float3(0f, 0.3f, 0f),
						Size = 1f,
						Name = $"1017_DeadHit_{result}"
					});
				}
				break;
			}
			if (info.isTargetDead && info.isTriggerDeadEvent)
			{
				float3 hitPosition2 = info.spell.HitPosition;
				hitPosition2.z = 0f;
				if (OnKillDropCoinLookup.TryGetComponent(spell.Entity, out var componentData) && Random.NextFloatByChunkIndex(chunkIndex) < componentData.DropRatio)
				{
					CMD.AppendToBuffer(chunkIndex, OnKillDropCoinOrCrystalEventEntity, new SpellOnKillDropCoinAndCrystalSystem.Require
					{
						ItemId = 11,
						Position = hitPosition2
					});
				}
				if (OnKillDropCrystalLookup.TryGetComponent(spell.Entity, out var componentData2) && Random.NextFloatByChunkIndex(chunkIndex) < componentData2.DropRatio)
				{
					CMD.AppendToBuffer(chunkIndex, OnKillDropCoinOrCrystalEventEntity, new SpellOnKillDropCoinAndCrystalSystem.Require
					{
						ItemId = 101,
						Position = hitPosition2
					});
				}
			}
			bool flag = false;
			if (info.spell.CostRefraction && SpellRefractionLookup.HasComponent(spell.Entity) && !spell.Movement.ValueRO.IsFallSpell)
			{
				DynamicBuffer<SpellRefractionHitEntities> refractedEntities = SpellRefractionHitEntitiesLookup[spell.Entity];
				ref SpellRefractionData valueRW2 = ref SpellRefractionLookup.GetRefRW(spell.Entity).ValueRW;
				NativeArray<Entity> theEntitiesHitByThisDamage = new NativeArray<Entity>(1, Allocator.Temp) { [0] = targetEntity };
				flag = SpellTools.TryRefract(in spell.Transform.ValueRO.Position, spell.Config.ValueRO.ShooterType, ref valueRW2, in refractedEntities, ref spell.Movement.ValueRW, in CurrentRoomEntities, in theEntitiesHitByThisDamage, out var targetRefractPosition);
				if (flag)
				{
					CMD.AppendToBuffer(chunkIndex, RecheckRefractDirectionBuffer, new SpellRecheckRefractDirectionData
					{
						TargetPos = targetRefractPosition,
						SpellEntity = spell.Entity
					});
				}
				if (flag && spell.Config.ValueRO.AbilityType == SpellAbilityType.SnakeWalk)
				{
					Spell1010SnakeLookup.GetRefRW(spell.Entity).ValueRW.TargetDirection = spell.Movement.ValueRO.Direction;
				}
			}
			if (!flag && info.spell.CostPenetrate)
			{
				spell.Config.ValueRW.Penetrate.CostPenetrateValue();
			}
			if (info.isDamageCritical)
			{
				CMD.ActiveGravitationalForceCrystal(chunkIndex, info.damage, in info.spell.HitPosition, in Spell3112Buffer, in CurrentRoomEntities, in spell, in targetEntity);
			}
			UnitType unitType;
			if (TransformLookup.TryGetComponent(targetEntity, out var componentData3) && spell.Data.ValueRW.EnableTriggerRedRune && spell.Config.ValueRW.AbilityType != SpellAbilityType.RedRune)
			{
				unitType = unitProperty_Dots.unitCfg.unitType;
				if (unitType != UnitType.Brittleness && unitType != UnitType.NotAttack)
				{
					CMD.TryRecordRedRune(chunkIndex, in spell, in targetEntity, in componentData3.Position, in Spell4025Buffer, info.isDamageCritical);
				}
			}
			if (!spell.Data.ValueRO.Wand || spell.Data.ValueRO.FromPostSlot)
			{
				return;
			}
			unitType = unitProperty_Dots.unitCfg.unitType;
			if (unitType != UnitType.Brittleness && unitType != UnitType.NotAttack)
			{
				WandChargeEvent element2 = new WandChargeEvent(spell.Data.ValueRO.Wand);
				CMD.AppendToBuffer(chunkIndex, OnHitWandChargeEventEntity, element2);
				if (info.damage > 45f)
				{
					CMD.AppendToBuffer(chunkIndex, OnHighDamageWandChargeEventEntity, element2);
				}
				if (info.isDamageCritical)
				{
					CMD.AppendToBuffer(chunkIndex, OnCriticalWandChargeEventEntity, element2);
				}
				if (info.isTargetDead)
				{
					CMD.AppendToBuffer(chunkIndex, OnKillWandChargeEventEntity, element2);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			BufferAccessor<SpellHitEntity> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SpellHitEntity_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					DynamicBuffer<SpellHitEntity> hitEntities = bufferAccessor[i];
					Execute(spell, hitEntities, chunkIndexInQuery);
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
						SpellAspect spell2 = resolvedChunk[nextRangeBegin];
						DynamicBuffer<SpellHitEntity> hitEntities2 = bufferAccessor[nextRangeBegin];
						Execute(spell2, hitEntities2, chunkIndexInQuery);
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
					SpellAspect spell3 = resolvedChunk[j];
					DynamicBuffer<SpellHitEntity> hitEntities3 = bufferAccessor[j];
					Execute(spell3, hitEntities3, chunkIndexInQuery);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					SpellAspect spell4 = resolvedChunk[k];
					DynamicBuffer<SpellHitEntity> hitEntities4 = bufferAccessor[k];
					Execute(spell4, hitEntities4, chunkIndexInQuery);
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

	protected internal struct WandChargeEvent : IBufferElementData
	{
		public readonly UnityObjectRef<Wand> Wand;

		public WandChargeEvent(UnityObjectRef<Wand> wand)
		{
			Wand = wand;
		}
	}

	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		public ComponentLookup<SpellHitTriggerComponentData> __SpellHitTriggerComponentData_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public ComponentLookup<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentLookup;

		public ComponentLookup<SpellOnKillDropCoin> __SpellOnKillDropCoin_RW_ComponentLookup;

		public ComponentLookup<SpellOnKillDropCrystal> __SpellOnKillDropCrystal_RW_ComponentLookup;

		public SpellTakeDamageResultJob.InternalCompilerQueryAndHandleData __SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle;

		public BufferLookup<WandChargeEvent> __SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__SpellHitTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellHitTriggerComponentData>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__Spell1010SnakeData_RW_ComponentLookup = state.GetComponentLookup<Spell1010SnakeData>();
			__SpellOnKillDropCoin_RW_ComponentLookup = state.GetComponentLookup<SpellOnKillDropCoin>();
			__SpellOnKillDropCrystal_RW_ComponentLookup = state.GetComponentLookup<SpellOnKillDropCrystal>();
			__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup = state.GetBufferLookup<WandChargeEvent>();
		}
	}

	private Entity OnHitWandChargeEventEntity;

	private Entity OnCriticalWandChargeEventEntity;

	private Entity OnKillWandChargeEventEntity;

	private Entity OnHighDamageWandChargeEventEntity;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_814669811_0;

	private EntityQuery __query_814669811_1;

	private EntityQuery __query_814669811_2;

	private EntityQuery __query_814669811_3;

	private EntityQuery __query_814669811_4;

	private EntityQuery __query_814669811_5;

	private EntityQuery __query_814669811_6;

	private EntityQuery __query_814669811_7;

	private EntityQuery __query_814669811_8;

	private EntityQuery __query_814669811_9;

	private EntityQuery __query_814669811_10;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellRecheckRefractDirectionData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<Spell1006HitPositionData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<Spell3112NewChainSingleton>();
		state.RequireForUpdate<Spell4025RuneSlashSpawnData>();
		state.RequireForUpdate<SpellOnKillDropCoinAndCrystalSystem.Require>();
		OnHitWandChargeEventEntity = state.EntityManager.CreateEntity(typeof(WandChargeEvent));
		OnCriticalWandChargeEventEntity = state.EntityManager.CreateEntity(typeof(WandChargeEvent));
		OnKillWandChargeEventEntity = state.EntityManager.CreateEntity(typeof(WandChargeEvent));
		OnHighDamageWandChargeEventEntity = state.EntityManager.CreateEntity(typeof(WandChargeEvent));
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		__ScheduleViaJobChunkExtension_0(new SpellTakeDamageResultJob
		{
			Spell3112Buffer = __query_814669811_0.GetSingletonEntity(),
			Spell4025Buffer = __query_814669811_1.GetSingletonEntity(),
			Spell1006HitTargetsPositionBufferEntity = __query_814669811_2.GetSingletonEntity(),
			SpellSpawnParamsBufferEntity = __query_814669811_3.GetSingletonEntity(),
			SpellSingleton = __query_814669811_4.GetSingleton<SpellSingleton>(),
			SEPlayerSingleton = __query_814669811_5.GetSingletonEntity(),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertiesLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			TakeDamageInfoLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref state),
			SpellHitTriggerLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellHitTriggerComponentData_RW_ComponentLookup, ref state),
			SpellRefractionHitEntitiesLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			SpellRefractionLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			Spell1010SnakeLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1010SnakeData_RW_ComponentLookup, ref state),
			CMD = entityCommandBuffer.AsParallelWriter(),
			OnHitWandChargeEventEntity = OnHitWandChargeEventEntity,
			OnCriticalWandChargeEventEntity = OnCriticalWandChargeEventEntity,
			OnKillWandChargeEventEntity = OnKillWandChargeEventEntity,
			OnHighDamageWandChargeEventEntity = OnHighDamageWandChargeEventEntity,
			CurrentRoomEntities = __query_814669811_6.GetSingleton<CurrentRoomEntitiesSingleton>(),
			OnKillDropCoinLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellOnKillDropCoin_RW_ComponentLookup, ref state),
			OnKillDropCrystalLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellOnKillDropCrystal_RW_ComponentLookup, ref state),
			Random = __query_814669811_7.GetSingleton<GlobalRandom>(),
			OnKillDropCoinOrCrystalEventEntity = __query_814669811_8.GetSingletonEntity(),
			GlobalParticleSingleton = __query_814669811_9.GetSingletonEntity(),
			RecheckRefractDirectionBuffer = __query_814669811_10.GetSingletonEntity()
		}, __TypeHandle.__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		foreach (WandChargeEvent item in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnHitWandChargeEventEntity))
		{
			WandPostSlotTrigger.PostSlotSpellHitTriggerCheck(item.Wand.Value.WandCfg);
		}
		InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnHitWandChargeEventEntity).Clear();
		foreach (WandChargeEvent item2 in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnKillWandChargeEventEntity))
		{
			WandPostSlotTrigger.PostSlotKillEnemyTriggerCheck(item2.Wand.Value.WandCfg);
		}
		InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnKillWandChargeEventEntity).Clear();
		foreach (WandChargeEvent item3 in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnCriticalWandChargeEventEntity))
		{
			WandPostSlotTrigger.PostSlotSpellCriticalHitTriggerCheck(item3.Wand.Value.WandCfg);
		}
		InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnCriticalWandChargeEventEntity).Clear();
		foreach (WandChargeEvent item4 in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnHighDamageWandChargeEventEntity))
		{
			WandPostSlotTrigger.PostSlotHighDamageTriggerCheck(item4.Wand.Value.WandCfg);
		}
		InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellTakeDamageResultSystem_WandChargeEvent_RW_BufferLookup, ref state, OnHighDamageWandChargeEventEntity).Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(SpellTakeDamageResultJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellTakeDamageResultSystem_SpellTakeDamageResultJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3112NewChainSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell4025RuneSlashSpawnData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1006HitPositionData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellOnKillDropCoinAndCrystalSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellRecheckRefractDirectionData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_814669811_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((SpellTakeDamageResultSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpellTakeDamageResultSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellTakeDamageResultSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
