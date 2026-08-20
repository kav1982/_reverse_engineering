using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
internal struct Spell4019BiAnBladeSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2020782573_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell4019BiAnBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4019BiAnBladeData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4019BiAnBladeData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4019BiAnBladeData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell4019BiAnBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell4019BiAnBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Spell4019BiAnBladeData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2020782573_0.TypeHandle __IFE_2020782573_0_TypeHandle;

		public BufferLookup<BladeWandSingletonData> __BladeWandSingletonData_RW_BufferLookup;

		public BufferLookup<BladeShootListenerData> __BladeShootListenerData_RW_BufferLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<Spell4019BladeHideUnderGroundMat> __Spell4019BladeHideUnderGroundMat_RW_ComponentLookup;

		public ComponentLookup<Spell4019BladeColorChangeMat> __Spell4019BladeColorChangeMat_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RW_ComponentLookup;

		public ComponentLookup<BreakLightningChain> __BreakLightningChain_RW_ComponentLookup;

		public ComponentLookup<GlobalParticle.Emitter> __GlobalParticle_Emitter_RW_ComponentLookup;

		public ComponentLookup<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RW_ComponentLookup;

		public Spell4019BiAnBladeJob.InternalCompilerQueryAndHandleData __Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2020782573_0_TypeHandle = new IFE_2020782573_0.TypeHandle(ref state);
			__BladeWandSingletonData_RW_BufferLookup = state.GetBufferLookup<BladeWandSingletonData>();
			__BladeShootListenerData_RW_BufferLookup = state.GetBufferLookup<BladeShootListenerData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Spell4019BladeHideUnderGroundMat_RW_ComponentLookup = state.GetComponentLookup<Spell4019BladeHideUnderGroundMat>();
			__Spell4019BladeColorChangeMat_RW_ComponentLookup = state.GetComponentLookup<Spell4019BladeColorChangeMat>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellNeedResize_RW_ComponentLookup = state.GetComponentLookup<SpellNeedResize>();
			__BreakLightningChain_RW_ComponentLookup = state.GetComponentLookup<BreakLightningChain>();
			__GlobalParticle_Emitter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.Emitter>();
			__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.EmitDistanceCounter>();
			__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	public static bool HideBlade;

	private Entity BladeListenerEntity;

	private Entity BladeWandEntity;

	private Entity ChainReqEntity;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2020782573_0;

	private EntityQuery __query_2020782573_1;

	private EntityQuery __query_2020782573_2;

	private EntityQuery __query_2020782573_3;

	private EntityQuery __query_2020782573_4;

	private EntityQuery __query_2020782573_5;

	private EntityQuery __query_2020782573_6;

	private EntityQuery __query_2020782573_7;

	private EntityQuery __query_2020782573_8;

	private EntityQuery __query_2020782573_9;

	private EntityQuery __query_2020782573_10;

	private EntityQuery __query_2020782573_11;

	private EntityQuery __query_2020782573_12;

	private EntityQuery __query_2020782573_13;

	public void OnCreate(ref SystemState state)
	{
		HideBlade = false;
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SpellEffectSystem.Destroy>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<BladeWandSingletonData>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<BladeShootListenerData>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<EffectsCollectorData>();
		state.RequireForUpdate<SpellConfigComponentData>();
		state.RequireForUpdate<Spell4019BiAnBladeData>();
		state.RequireForUpdate<Spell3007CreateRequest>();
		state.EntityManager.CreateSingletonBuffer<BladeShootListenerData>();
		BladeListenerEntity = __query_2020782573_1.GetSingletonEntity();
		state.EntityManager.CreateSingletonBuffer<BladeWandSingletonData>();
		BladeWandEntity = __query_2020782573_2.GetSingletonEntity();
		state.EntityManager.CreateSingletonBuffer<Spell3007CreateRequest>();
		ChainReqEntity = __query_2020782573_3.GetSingletonEntity();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_2020782573_4.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		PlayerController_Dots singleton = __query_2020782573_5.GetSingleton<PlayerController_Dots>();
		DynamicBuffer<BladeWandSingletonData> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__BladeWandSingletonData_RW_BufferLookup, ref state, BladeWandEntity);
		DynamicBuffer<BladeShootListenerData> bufferAfterCompletingDependency2 = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__BladeShootListenerData_RW_BufferLookup, ref state, BladeListenerEntity);
		for (int i = 0; i < bufferAfterCompletingDependency2.Length; i++)
		{
			BladeShootListenerData value = bufferAfterCompletingDependency2[i];
			for (int j = 0; j < bufferAfterCompletingDependency.Length; j++)
			{
				if (value.ShootingWandId == bufferAfterCompletingDependency[j].WandId)
				{
					value.ShootCount = bufferAfterCompletingDependency[j].ShootCount;
					bufferAfterCompletingDependency2[i] = value;
					break;
				}
			}
		}
		foreach (var (uncheckedRefRW, uncheckedRefRW2) in IFE_2020782573_0.Query(__query_2020782573_0, __TypeHandle.__IFE_2020782573_0_TypeHandle, ref state))
		{
			if (!uncheckedRefRW2.ValueRO.Wand)
			{
				break;
			}
			uncheckedRefRW.ValueRW.WandIndex = uncheckedRefRW2.ValueRO.Wand.Value.WandIndex;
			for (int k = 0; k < bufferAfterCompletingDependency2.Length; k++)
			{
				if (bufferAfterCompletingDependency2[k].EventType == 0 && bufferAfterCompletingDependency2[k].ShootingWandId == uncheckedRefRW.ValueRO.WandIndex && uncheckedRefRW.ValueRO.CanShoot)
				{
					BladeShootListenerData value2 = bufferAfterCompletingDependency2[k];
					if (value2.ShootCount > 0)
					{
						value2.ShootCount--;
						bufferAfterCompletingDependency2[k] = value2;
						uncheckedRefRW.ValueRW.StateNext = 1;
						uncheckedRefRW.ValueRW.CanShoot = false;
						break;
					}
				}
				if (bufferAfterCompletingDependency2[k].EventType == 1 && uncheckedRefRW.ValueRO.CanReturn)
				{
					uncheckedRefRW.ValueRW.StateNext = 6;
					uncheckedRefRW.ValueRW.CanReturn = false;
					break;
				}
			}
		}
		NativeArray<BladeWandSingletonData> bladeWands = bufferAfterCompletingDependency.AsNativeArray();
		bufferAfterCompletingDependency2.Clear();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4019BiAnBladeJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			Random = __query_2020782573_6.GetSingleton<GlobalRandom>(),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			EffectCtrlLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			BladeUnderGroundMatDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4019BladeHideUnderGroundMat_RW_ComponentLookup, ref state),
			BladeGlowMatDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4019BladeColorChangeMat_RW_ComponentLookup, ref state),
			PlayerController = singleton,
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			NeedResizeLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellNeedResize_RW_ComponentLookup, ref state),
			EffectRequireBufferEntity = __query_2020782573_7.GetSingletonEntity(),
			EffectRecycleBufferEntity = __query_2020782573_8.GetSingletonEntity(),
			PhysicsWorld = __query_2020782573_9.GetSingleton<PhysicsWorldSingleton>(),
			SEPlayerSingleton = __query_2020782573_10.GetSingletonEntity(),
			GlobalSeed = __query_2020782573_6.GetSingleton<GlobalRandom>().random.NextUInt(),
			ChainReqEntity = ChainReqEntity,
			LightningChainDestroyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__BreakLightningChain_RW_ComponentLookup, ref state),
			BladeWands = bladeWands,
			IsIgnoreWallRelic = PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall,
			Spell3101Buffer = __query_2020782573_11.GetSingletonEntity(),
			GlobalParticleSystemBufferEntity = __query_2020782573_12.GetSingletonEntity(),
			OptimizeData = __query_2020782573_13.GetSingleton<DynamicOptimizeData>(),
			GlobalParticleEmitterLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__GlobalParticle_Emitter_RW_ComponentLookup, ref state),
			GlobalParticleMoveEmitterLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup, ref state),
			Hide = HideBlade
		}, __TypeHandle.__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4019BiAnBladeJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4019BiAnBladeJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4019BiAnBladeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_2020782573_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<BladeShootListenerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<BladeWandSingletonData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3007CreateRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_11 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_12 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2020782573_13 = entityQueryBuilder2.Build(ref state);
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
		((Spell4019BiAnBladeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4019BiAnBladeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4019BiAnBladeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
