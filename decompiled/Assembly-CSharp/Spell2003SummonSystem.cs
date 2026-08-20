using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell2003SummonSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1475871886_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2003TentacleData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2003TentacleData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateData> item3_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2003TentacleData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell2003TentacleData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1475871886_0.TypeHandle __IFE_1475871886_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		public ComponentLookup<MatOverrideFrameIndex> __MatOverrideFrameIndex_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		public Spell2003Job.InternalCompilerQueryAndHandleData __Spell2003Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1475871886_0_TypeHandle = new IFE_1475871886_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__MatOverrideFrameIndex_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFrameIndex>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellElementEffectComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellSplitComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>();
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007206_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007206_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007206_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00007207_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007207_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007207_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1475871886_0;

	private EntityQuery __query_1475871886_1;

	private EntityQuery __query_1475871886_2;

	private EntityQuery __query_1475871886_3;

	private EntityQuery __query_1475871886_4;

	private EntityQuery __query_1475871886_5;

	private EntityQuery __query_1475871886_6;

	private EntityQuery __query_1475871886_7;

	private EntityQuery __query_1475871886_8;

	private EntityQuery __query_1475871886_9;

	private EntityQuery __query_1475871886_10;

	private EntityQuery __query_1475871886_11;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<TeammateGhostEffectData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.EntityManager.CreateSingletonBuffer<Teammate3InvisibleTentacleSpawnerData>();
		state.EntityManager.CreateSingletonBuffer<Teammate3SplitTentacleSpawnerData>();
		state.RequireForUpdate<Teammate3InvisibleTentacleSpawnerData>();
		state.RequireForUpdate<Teammate3SplitTentacleSpawnerData>();
		state.RequireForUpdate<Spell2003TentacleData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_1475871886_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		SpellSingleton singleton = __query_1475871886_2.GetSingleton<SpellSingleton>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> item4 in IFE_1475871886_0.Query(__query_1475871886_0, __TypeHandle.__IFE_1475871886_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2003TentacleData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<TeammateData> uncheckedRefRW3 = item3;
			Entity entity2 = entity;
			if (uncheckedRefRW.ValueRW.State != 0)
			{
				continue;
			}
			int num = ((uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel > 0) ? (1 / (uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel + 1)) : 0);
			bool flag = uncheckedRefRW2.ValueRO.ColorType == SpellColorType.Fire;
			for (int i = 0; i <= uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel; i++)
			{
				FixedString32Bytes effectName = "Body";
				if (singleton.TryGetSpellEffectEntity(2003, in effectName, uncheckedRefRW2.ValueRW.ColorType, out var entity3))
				{
					Entity entity4 = state.EntityManager.Instantiate(entity3);
					LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity2);
					componentAfterCompletingDependency.Position = new float3((float)(-uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel) / 2f * 0.2f + (float)i * 0.2f, 0f, 0f);
					componentAfterCompletingDependency.Scale = 1f;
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency, entity4);
					entityCommandBuffer.AddComponent<Parent>(entity4);
					entityCommandBuffer.SetComponent(entity4, new Parent
					{
						Value = entity2
					});
					entityCommandBuffer.AppendToBuffer(entity2, new LinkedEntityGroup
					{
						Value = entity4
					});
					EffectsCollectorData componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, entity4);
					HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect1).ValueRW);
					HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect2).ValueRW);
					HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect3).ValueRW);
					HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect4).ValueRW);
					if (flag)
					{
						Entity effect = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect1).ValueRW.Effect1;
						Entity effect2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect2).ValueRW.Effect1;
						Entity effect3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect3).ValueRW.Effect1;
						Entity effect4 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect4).ValueRW.Effect1;
						HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, effect).ValueRW);
						HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, effect2).ValueRW);
						HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, effect3).ValueRW);
						HideTargetAnimaEntity(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, effect4).ValueRW);
					}
					Entity entity5 = ((i > 0) ? componentAfterCompletingDependency2.Effect2 : componentAfterCompletingDependency2.Effect1);
					ResetTargetAnimaEntityFrame(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, entity5).ValueRW);
					if (flag)
					{
						Entity effect5 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, entity5).ValueRW.Effect1;
						ResetTargetAnimaEntityFrame(ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state, effect5).ValueRW);
					}
					uncheckedRefRW.ValueRW.LifeDuration = ((float)((uncheckedRefRW2.ValueRO.Level + 1) * 10) + uncheckedRefRW2.ValueRO.Duration.AddBase) * uncheckedRefRW2.ValueRW.Duration.MulRatio;
					entityCommandBuffer.AppendToBuffer(entity2, new Spell2003TentacleEffectData
					{
						EffectEntity = entity4,
						TentacleIndex = i,
						IdleEffectEntity = entity5,
						AttackEffectEntity = ((i > 0) ? componentAfterCompletingDependency2.Effect4 : componentAfterCompletingDependency2.Effect3),
						StartAttack = false,
						AttackTimer = -i * num,
						AttackFinished = false,
						AttackingHoldTimer = 0.4f
					});
				}
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2003Job
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			FrameAnimeLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state),
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_1475871886_3.GetSingleton<CurrentRoomEntitiesSingleton>(),
			CMD = entityCommandBuffer.AsParallelWriter(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellElementLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			SEPlayerSingleton = __query_1475871886_4.GetSingletonEntity(),
			SpellSingleton = __query_1475871886_2.GetSingleton<SpellSingleton>(),
			Random = __query_1475871886_5.GetSingleton<GlobalRandom>(),
			EffectsCollectorLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			InvisibleTentacleSpawnerEntity = __query_1475871886_6.GetSingletonEntity(),
			SplitTentacleSpawnerEntity = __query_1475871886_7.GetSingletonEntity(),
			SplitSpellLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellSplitComponentData_RW_ComponentLookup, ref state),
			SpellEffectEntity = __query_1475871886_8.GetSingletonEntity(),
			TeammateDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state),
			ColliderLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state),
			TeammateGhostEffectEntity = __query_1475871886_9.GetSingletonEntity(),
			Spell3101Buffer = __query_1475871886_10.GetSingletonEntity(),
			Physics = __query_1475871886_11.GetSingleton<PhysicsWorldSingleton>()
		}, __TypeHandle.__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		static void HideTargetAnimaEntity(ref MatOverrideFrameIndex frameData)
		{
			frameData.FrameIndex = -0.1f;
		}
		static void ResetTargetAnimaEntityFrame(ref MatOverrideFrameIndex frameData)
		{
			frameData.FrameIndex = 0f;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2003Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2003Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell2003TentacleData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		__query_1475871886_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Teammate3InvisibleTentacleSpawnerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Teammate3SplitTentacleSpawnerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateGhostEffectData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1475871886_11 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00007206_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007207_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2003SummonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2003SummonSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2003SummonSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
