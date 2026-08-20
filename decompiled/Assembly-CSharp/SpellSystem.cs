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
using Unity.Physics.Stateful;

[UpdateInGroup(typeof(SpellPhysicsSystemGroup), OrderFirst = true)]
[CompilerGenerated]
[BurstCompile]
public struct SpellSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct SpellJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				public ComponentTypeHandle<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__SpellSplitComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSplitComponentData>();
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__SpellSplitComponentData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSplitComponentData>();
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
			public void Run(ref SpellJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref SpellJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref SpellJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref SpellJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref SpellJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref SpellJob job, EntityManager entityManager)
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
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[ReadOnly]
		public ComponentLookup<SpellDisableTriggerDamageTag> DisableTriggerDamageLookup;

		public EntityCommandBuffer.ParallelWriter CMD;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public BufferLookup<StatefulTriggerEvent> TriggerLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[ReadOnly]
		public ComponentLookup<SpellOverSplitTriggerComponentData> OverSplitTriggerLookup;

		[ReadOnly]
		public ComponentLookup<SpellOverTriggerComponentData> OverTriggerLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellHalfLifeTeleportData> HalfLifeTeleportLookup;

		public Entity MucusBufferEntity;

		public Entity VenomBufferEntity;

		public Entity WaterBufferEntity;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellChargingTag> ChargingTagLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<IgnorePlayerSpellHitTag> IgnorePlayerSpellHitTagLookup;

		[ReadOnly]
		public ComponentLookup<IgnoreSpellHitTag> IgnoreSpellHitTagLookup;

		public float DeltaTime;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		public Entity SEPlayerSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(SpellAspect spell, [ChunkIndexInQuery] int chunkIndexInQuery, SpellSplitComponentData split)
		{
			if (!ChargingTagLookup.HasComponent(spell.Entity) || !ChargingTagLookup.IsComponentEnabled(spell.Entity))
			{
				bool reduceSizeWhenLifeEnd = split.Count == 0 && spell.Data.ValueRO.ReduceSizeWhenLifeEnd;
				if (spell.Data.ValueRO.SubGroupEntity != Entity.Null && (OverSplitTriggerLookup.IsComponentEnabled(spell.Entity) || OverTriggerLookup.IsComponentEnabled(spell.Entity)))
				{
					reduceSizeWhenLifeEnd = false;
				}
				UpdateDuration(spell, chunkIndexInQuery, reduceSizeWhenLifeEnd, out var destroySelf);
				if (destroySelf)
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndexInQuery, spell.Entity, value: true);
					return;
				}
			}
			if (DisableTriggerDamageLookup.HasComponent(spell.Entity) || !TriggerLookup.TryGetBuffer(spell.Entity, out var bufferData))
			{
				return;
			}
			bool flag = false;
			foreach (StatefulTriggerEvent item in bufferData)
			{
				if (item.State != StatefulEventState.Enter)
				{
					continue;
				}
				Entity target = item.GetOtherEntity(spell.Entity);
				if (IgnoreSpellHitTagLookup.HasComponent(target) || (DTool.IsSameCamp(spell.Config.ValueRO.ShooterType, UnitType.Player) && IgnorePlayerSpellHitTagLookup.HasComponent(target)))
				{
					continue;
				}
				TakeDamageInfo_Dots damage = spell.MakeDamageInfo(costPenetrate: true);
				damage.SetKnockbackForceIgnoreZBySpell(spell.Movement.ValueRO.Direction);
				SpellTools.HitType hitType = CMD.TryAttackEntity(chunkIndexInQuery, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
				if (hitType == SpellTools.HitType.RollBall)
				{
					CreateHitEffect(spell, chunkIndexInQuery);
					spell.Config.ValueRW.Penetrate.CostPenetrateValue();
					if (spell.Config.ValueRW.Penetrate.Calculate() < 0)
					{
						flag = true;
						break;
					}
				}
				if (hitType != 0 || spell.Data.ValueRO.CanThroughWall || spell.Movement.ValueRO.ReboundCount > 0)
				{
					continue;
				}
				CreateHitEffect(spell, chunkIndexInQuery);
				flag = true;
				if (spell.Config.ValueRW.AbilityType == SpellAbilityType.Rollball)
				{
					if (spell.Config.ValueRO.ColorType == SpellColorType.Venom)
					{
						CMD.AppendToBuffer(chunkIndexInQuery, VenomBufferEntity, new CreateVenomRequest(spell.Transform.ValueRO.Position, spell.Transform.ValueRO.Scale / 2f, 2f));
					}
					else if (spell.Config.ValueRO.ColorType == SpellColorType.Mucus)
					{
						CMD.AppendToBuffer(chunkIndexInQuery, MucusBufferEntity, new CreateMucusRequest(spell.Transform.ValueRO.Position, spell.Transform.ValueRO.Scale / 2f));
					}
					else if (spell.Config.ValueRO.ColorType == SpellColorType.Frozen)
					{
						CMD.AppendToBuffer(chunkIndexInQuery, WaterBufferEntity, new CreateWaterRequest(spell.Transform.ValueRO.Position, spell.Transform.ValueRO.Scale / 2f));
					}
				}
				break;
			}
			if (flag)
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndexInQuery, spell.Entity, value: true);
			}
		}

		[BurstCompile]
		private void CreateHitEffect(SpellAspect spell, int chunkIndex)
		{
			if (spell.Data.ValueRO.PlayHitSE)
			{
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				int prefabId = spell.Data.ValueRO.PrefabId;
				FixedString32Bytes seName = "Hit";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(prefabId, in seName)));
			}
			ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
			ref SpellSingleton spellSingleton = ref SpellSingleton;
			ref readonly SpellConfigComponentData valueRO = ref spell.Config.ValueRO;
			ref readonly SpellComponentData valueRO2 = ref spell.Data.ValueRO;
			ref readonly float3 position = ref spell.Transform.ValueRO.Position;
			float3 direction = spell.Movement.ValueRO.Direction;
			cMD2.CreateSpellHitEffect(chunkIndex, in spellSingleton, in valueRO, in valueRO2, in position, in direction, spell.Transform.ValueRO.Scale);
		}

		private void UpdateDuration(SpellAspect spell, int chunkIndex, bool reduceSizeWhenLifeEnd, out bool destroySelf)
		{
			destroySelf = false;
			ref SpellConfigComponentData valueRW = ref spell.Config.ValueRW;
			if (valueRW.DurationTimer >= spell.Config.ValueRW.Duration.Calculate() / 2f && HalfLifeTeleportLookup.TryGetComponent(spell.Entity, out var componentData) && !HalfLifeTeleportLookup.IsComponentEnabled(spell.Entity) && componentData.TeleportCount > 0 && valueRW.AbilityType != SpellAbilityType.MagicBreaker)
			{
				CMD.SetComponentEnabled<SpellHalfLifeTeleportData>(chunkIndex, spell.Entity, value: true);
				return;
			}
			if (!spell.Movement.ValueRO.IsFallSpell)
			{
				SpellAbilityType abilityType = spell.Config.ValueRO.AbilityType;
				if (abilityType != SpellAbilityType.Laser && abilityType != SpellAbilityType.RuneHammer && abilityType != SpellAbilityType.LaserBeam && abilityType != SpellAbilityType.BiAnLethalBlade && abilityType != SpellAbilityType.Umbrella && abilityType != SpellAbilityType.HighPressureWasher && abilityType != SpellAbilityType.DaveHarpoons)
				{
					if (valueRW.DurationTimer < valueRW.Duration.Calculate())
					{
						valueRW.DurationTimer += DeltaTime;
						return;
					}
					spell.Movement.ValueRW.Speed = 0f;
					if (valueRW.HoverTimer < valueRW.HoverDuration)
					{
						valueRW.HoverTimer += DeltaTime;
					}
					else if (spell.Config.ValueRO.AbilityType == SpellAbilityType.DimensionTraveller && spell.Movement.ValueRO.CurrentFallSpeed > 0f)
					{
						destroySelf = false;
					}
					else if (reduceSizeWhenLifeEnd)
					{
						spell.Transform.ValueRW.Scale -= 5f * DeltaTime;
						if (spell.Transform.ValueRO.Scale <= 0.01f)
						{
							spell.Transform.ValueRW.Scale = 0.01f;
							destroySelf = true;
						}
					}
					else
					{
						destroySelf = true;
					}
					return;
				}
			}
			valueRW.DurationTimer += DeltaTime;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellSplitComponentData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					Execute(spell, chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr, i));
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
						Execute(spell2, chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr, nextRangeBegin));
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
					Execute(spell3, chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr, j));
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
					Execute(spell4, chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr, k));
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

	private struct TypeHandle
	{
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellOverSplitTriggerComponentData> __SpellOverSplitTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellOverTriggerComponentData> __SpellOverTriggerComponentData_RW_ComponentLookup;

		public BufferLookup<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferLookup;

		public ComponentLookup<SpellDisableTriggerDamageTag> __SpellDisableTriggerDamageTag_RW_ComponentLookup;

		public ComponentLookup<IgnorePlayerSpellHitTag> __IgnorePlayerSpellHitTag_RW_ComponentLookup;

		public ComponentLookup<IgnoreSpellHitTag> __IgnoreSpellHitTag_RW_ComponentLookup;

		public ComponentLookup<SpellHalfLifeTeleportData> __SpellHalfLifeTeleportData_RW_ComponentLookup;

		public ComponentLookup<SpellChargingTag> __SpellChargingTag_RW_ComponentLookup;

		public SpellJob.InternalCompilerQueryAndHandleData __SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellOverSplitTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellOverSplitTriggerComponentData>();
			__SpellOverTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellOverTriggerComponentData>();
			__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferLookup = state.GetBufferLookup<StatefulTriggerEvent>();
			__SpellDisableTriggerDamageTag_RW_ComponentLookup = state.GetComponentLookup<SpellDisableTriggerDamageTag>();
			__IgnorePlayerSpellHitTag_RW_ComponentLookup = state.GetComponentLookup<IgnorePlayerSpellHitTag>();
			__IgnoreSpellHitTag_RW_ComponentLookup = state.GetComponentLookup<IgnoreSpellHitTag>();
			__SpellHalfLifeTeleportData_RW_ComponentLookup = state.GetComponentLookup<SpellHalfLifeTeleportData>();
			__SpellChargingTag_RW_ComponentLookup = state.GetComponentLookup<SpellChargingTag>();
			__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_940598088_0;

	private EntityQuery __query_940598088_1;

	private EntityQuery __query_940598088_2;

	private EntityQuery __query_940598088_3;

	private EntityQuery __query_940598088_4;

	private EntityQuery __query_940598088_5;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CreateWaterRequest>();
		state.RequireForUpdate<CreateVenomRequest>();
		state.RequireForUpdate<CreateMucusRequest>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell3112NewChainSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
	}

	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new SpellJob
		{
			CMD = __query_940598088_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			SpellSingleton = __query_940598088_1.GetSingleton<SpellSingleton>(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			SEPlayerSingleton = __query_940598088_2.GetSingletonEntity(),
			OverSplitTriggerLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellOverSplitTriggerComponentData_RW_ComponentLookup, ref state),
			OverTriggerLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellOverTriggerComponentData_RW_ComponentLookup, ref state),
			TriggerLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferLookup, ref state),
			DisableTriggerDamageLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellDisableTriggerDamageTag_RW_ComponentLookup, ref state),
			IgnorePlayerSpellHitTagLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__IgnorePlayerSpellHitTag_RW_ComponentLookup, ref state),
			IgnoreSpellHitTagLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__IgnoreSpellHitTag_RW_ComponentLookup, ref state),
			HalfLifeTeleportLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellHalfLifeTeleportData_RW_ComponentLookup, ref state),
			ChargingTagLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellChargingTag_RW_ComponentLookup, ref state),
			MucusBufferEntity = __query_940598088_3.GetSingletonEntity(),
			VenomBufferEntity = __query_940598088_4.GetSingletonEntity(),
			WaterBufferEntity = __query_940598088_5.GetSingletonEntity()
		}, __TypeHandle.__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(SpellJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellSystem_SpellJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateMucusRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateVenomRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateWaterRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_940598088_5 = entityQueryBuilder2.Build(ref state);
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
		((SpellSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpellSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
