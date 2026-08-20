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

[CompilerGenerated]
[BurstCompile]
[UpdateAfter(typeof(SpellEffectSystem))]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
internal struct Spell1029DimensionTravellerFallDamageSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct Spell1029FallDamageJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<SpellGroundedTag> __SpellGroundedTag_RW_ComponentTypeHandle;

				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<Spell1029DimensionTravellerData> __Spell1029DimensionTravellerData_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellGroundedTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellGroundedTag>();
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__Spell1029DimensionTravellerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1029DimensionTravellerData>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__SpellGroundedTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__Spell1029DimensionTravellerData_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1029DimensionTravellerData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGroundedTag>();
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
			public void Run(ref Spell1029FallDamageJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1029FallDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1029FallDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1029FallDamageJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1029FallDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1029FallDamageJob job, EntityManager entityManager)
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

		public Entity SEPlayerEntity;

		public Entity EffectRequireEntity;

		public Entity EffectDestroyEntity;

		public Entity GlobalParticleEntity;

		public EntityCommandBuffer.ParallelWriter CMD;

		public SpellSingleton SpellSingleton;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellRefractionData> refractionDataLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<GlobalParticle.EmitDistanceCounter> EmitDistanceCounterLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public BufferLookup<SpellRefractionHitEntities> refractedEntitiesLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public PhysicsWorldSingleton PhysicsWorld;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[ReadOnly]
		public Entity ScreenShakeSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private bool TryRefract(Entity spellEntity, ref SpellMovementComponentData movement, in SpellConfigComponentData config, ref LocalTransform transform, in Spell1029DimensionTravellerData spellData, in NativeArray<Entity> hitList)
		{
			if (hitList.Length == 0)
			{
				return false;
			}
			if (refractionDataLookup.HasComponent(spellEntity))
			{
				ref float3 position = ref transform.Position;
				UnitType shooterType = config.ShooterType;
				ref SpellRefractionData valueRW = ref refractionDataLookup.GetRefRW(spellEntity).ValueRW;
				DynamicBuffer<SpellRefractionHitEntities> refractedEntities = refractedEntitiesLookup[spellEntity];
				if (SpellTools.TryRefract(in position, shooterType, ref valueRW, in refractedEntities, ref movement, in CurrentRoomEntities, in hitList, out var _))
				{
					return true;
				}
			}
			return false;
		}

		[BurstCompile]
		private void Execute(SpellGroundedTag _, SpellAspect spell, in Spell1029DimensionTravellerData spellData, [ChunkIndexInQuery] int index)
		{
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEPlayerEntity = SEPlayerEntity;
			int prefabId = spell.Data.ValueRO.PrefabId;
			FixedString32Bytes seName = "Hit";
			cMD.AppendToBuffer(index, sEPlayerEntity, new SEData(DTool.GetSpellSEName(prefabId, in seName), SEPlayMode.Unique, 10));
			CMD.AppendToBuffer(index, ScreenShakeSingleton, new ScreenShakeData
			{
				Radius = 0.05f,
				Speed = 1f,
				Time = 0.05f
			});
			if (spell.Config.ValueRW.DurationTimer >= spell.Config.ValueRW.Duration.Calculate())
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(index, spell.Entity, value: true);
				return;
			}
			spell.Config.ValueRW.ColorType.ColorEnumToString(out var result);
			NativeHashMap<FixedString64Bytes, SpellEffect> nativeHashMap = SpellSingleton.Effects[1029];
			ref LocalTransform valueRW = ref spell.Transform.ValueRW;
			CMD.AppendToBuffer(index, GlobalParticleEntity, new GlobalParticleEmitParams
			{
				Position = valueRW.Position,
				Size = spell.Config.ValueRW.Radius.CalculateIgnoreFall(),
				Name = $"1029_Disappear_{result}"
			});
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			ref float3 position = ref valueRW.Position;
			float radius = spell.Config.ValueRO.Radius.Calculate();
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in spell.Config.ValueRO.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
			TakeDamageInfo_Dots.NewInfo(spell.Entity, CostPenetrate: false, in spell.Config.ValueRO, in spell.Movement.ValueRO, in valueRW, in spell.ElementEffect.ValueRO, in spell.Data.ValueRO, out var info);
			info.spell.IgnoreHitEffect = true;
			foreach (Entity item in entities)
			{
				Entity target = item;
				info.SetKnockbackForceIgnoreZBySpell(TransformLookup[target].Position - valueRW.Position);
				CMD.TryAttackEntity(index, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
			}
			Entity entity = spell.Entity;
			ref SpellMovementComponentData valueRW2 = ref spell.Movement.ValueRW;
			ref readonly SpellConfigComponentData valueRO = ref spell.Config.ValueRO;
			ref LocalTransform valueRW3 = ref spell.Transform.ValueRW;
			NativeArray<Entity> hitList = entities.ToArray(Allocator.Temp);
			if (TryRefract(entity, ref valueRW2, in valueRO, ref valueRW3, in spellData, in hitList))
			{
				spell.Movement.ValueRW.Gravity = 25f;
				spell.Movement.ValueRW.ReboundFallSpeed();
				spell.Config.ValueRW.Duration.Extra += 1f;
			}
			else
			{
				spell.Movement.ValueRW.CurrentFallSpeed = spell.Movement.ValueRW.OriginalSpellHorizontalSpeed;
				spell.Transform.ValueRW.Position.z = spellData.InitialPositionZ * 2f;
				EmitDistanceCounterLookup.GetRefRW(spell.Data.ValueRO.SpellEffectEntity).ValueRW.startCounter = false;
				float3 layerPosition = DTool.GetLayerPosition(in spell.Transform.ValueRW.Position, LayerCorrectType.Coordinate);
				TransformLookup.GetRefRW(spell.Data.ValueRO.SpellEffectEntity).ValueRW.Position = layerPosition + spell.Transform.ValueRW.Position;
				spell.Movement.ValueRW.Gravity = 0f;
				CMD.AppendToBuffer(index, EffectDestroyEntity, new SpellEffectSystem.Destroy
				{
					Entity = spell.Entity,
					Name = "Trail"
				});
				CMD.AppendToBuffer(index, EffectRequireEntity, new SpellEffectSystem.Require
				{
					Settings = nativeHashMap["Trail"],
					Entity = spell.Entity,
					Color = result,
					SpellId = 1029
				});
			}
			CMD.SetComponentEnabled<SpellGroundedTag>(index, spell.Entity, value: false);
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Spell1029DimensionTravellerData_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					Execute(default(SpellGroundedTag), spell, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, i), chunkIndexInQuery);
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
						Execute(default(SpellGroundedTag), spell2, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, nextRangeBegin), chunkIndexInQuery);
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
					Execute(default(SpellGroundedTag), spell3, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, j), chunkIndexInQuery);
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
					Execute(default(SpellGroundedTag), spell4, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, k), chunkIndexInQuery);
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

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public ComponentLookup<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RW_ComponentLookup;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public Spell1029FallDamageJob.InternalCompilerQueryAndHandleData __Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.EmitDistanceCounter>();
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006F89_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006F89_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006F89_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00006F8A_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006F8A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006F8A_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnUpdate_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1740269954_0;

	private EntityQuery __query_1740269954_1;

	private EntityQuery __query_1740269954_2;

	private EntityQuery __query_1740269954_3;

	private EntityQuery __query_1740269954_4;

	private EntityQuery __query_1740269954_5;

	private EntityQuery __query_1740269954_6;

	private EntityQuery __query_1740269954_7;

	private EntityQuery __query_1740269954_8;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<SpellEffectSystem.Destroy>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_1740269954_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		Entity singletonEntity = __query_1740269954_1.GetSingletonEntity();
		Entity singletonEntity2 = __query_1740269954_2.GetSingletonEntity();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1029FallDamageJob
		{
			SEPlayerEntity = __query_1740269954_3.GetSingletonEntity(),
			EffectDestroyEntity = singletonEntity2,
			EffectRequireEntity = singletonEntity,
			GlobalParticleEntity = __query_1740269954_4.GetSingletonEntity(),
			CMD = entityCommandBuffer.AsParallelWriter(),
			SpellSingleton = __query_1740269954_5.GetSingleton<SpellSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			PhysicsWorld = __query_1740269954_6.GetSingleton<PhysicsWorldSingleton>(),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_1740269954_7.GetSingleton<CurrentRoomEntitiesSingleton>(),
			refractedEntitiesLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			EmitDistanceCounterLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup, ref state),
			refractionDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			ScreenShakeSingleton = __query_1740269954_8.GetSingletonEntity()
		}, __TypeHandle.__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1029FallDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1029DimensionTravellerFallDamageSystem_Spell1029FallDamageJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1740269954_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00006F89_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006F8A_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerFallDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerFallDamageSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerFallDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
