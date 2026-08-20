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
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SpellEndSystemGroup), OrderLast = true)]
public struct SpellDestroySystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1373312991_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (SpellDestroyTag, LocalTransform, SpellConfigComponentData, SpellComponentData, SpellEndTeleportTag) Get(int index)
			{
				return (default(SpellDestroyTag), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellComponentData>(item4_IntPtr, index), default(SpellEndTeleportTag));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(SpellDestroyTag, LocalTransform, SpellConfigComponentData, SpellComponentData, SpellEndTeleportTag)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (SpellDestroyTag, LocalTransform, SpellConfigComponentData, SpellComponentData, SpellEndTeleportTag) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1373312991_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellDestroyTag>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<SpellDestroyTag> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellDestroyTag>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<SpellDestroyTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1373312991_0.TypeHandle __IFE_1373312991_0_TypeHandle;

		public IFE_1373312991_1.TypeHandle __IFE_1373312991_1_TypeHandle;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public SpellDestroyEventJob.InternalCompilerQueryAndHandleData __SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellChargeData> __SpellChargeData_RO_ComponentLookup;

		public ComponentLookup<Spell1020ManaCoinData> __Spell1020ManaCoinData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1029DimensionTravellerData> __Spell1029DimensionTravellerData_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1373312991_0_TypeHandle = new IFE_1373312991_0.TypeHandle(ref state);
			__IFE_1373312991_1_TypeHandle = new IFE_1373312991_1.TypeHandle(ref state);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellSplitComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellChargeData_RO_ComponentLookup = state.GetComponentLookup<SpellChargeData>(isReadOnly: true);
			__Spell1020ManaCoinData_RW_ComponentLookup = state.GetComponentLookup<Spell1020ManaCoinData>();
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__Spell1029DimensionTravellerData_RO_ComponentLookup = state.GetComponentLookup<Spell1029DimensionTravellerData>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_000082A0_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_000082A0_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_000082A0_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1373312991_0;

	private EntityQuery __query_1373312991_1;

	private EntityQuery __query_1373312991_2;

	private EntityQuery __query_1373312991_3;

	private EntityQuery __query_1373312991_4;

	private EntityQuery __query_1373312991_5;

	private EntityQuery __query_1373312991_6;

	private EntityQuery __query_1373312991_7;

	private EntityQuery __query_1373312991_8;

	private EntityQuery __query_1373312991_9;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<GlobalRandom>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new SpellDestroyEventJob
		{
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			Physics = __query_1373312991_2.GetSingleton<PhysicsWorldSingleton>(),
			spell3101Buffer = __query_1373312991_3.GetSingletonEntity(),
			CMD = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.CompleteDependency();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		ProcessDestroy(ref state);
	}

	private void ProcessDestroy(ref SystemState state)
	{
		SpellSingleton spellSingleton = __query_1373312991_4.GetSingleton<SpellSingleton>();
		RefRW<GlobalRandom> singletonRW = __query_1373312991_5.GetSingletonRW<GlobalRandom>();
		singletonRW.ValueRW.NextFloatByChunkIndex(1234);
		int splitCountInThisFrame = 0;
		DynamicBuffer<SpellSpawnParams> singletonBuffer = __query_1373312991_6.GetSingletonBuffer<SpellSpawnParams>();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		Entity singletonEntity = __query_1373312991_7.GetSingletonEntity();
		Entity singletonEntity2 = __query_1373312991_8.GetSingletonEntity();
		foreach (var item5 in IFE_1373312991_0.Query(__query_1373312991_0, __TypeHandle.__IFE_1373312991_0_TypeHandle, ref state))
		{
			LocalTransform item = item5.Item2;
			SpellConfigComponentData item2 = item5.Item3;
			SpellComponentData item3 = item5.Item4;
			if (item2.Int3 <= 0 || item2.AbilityType != SpellAbilityType.HighPressureWasher)
			{
				Vector3 navMeshPoint = Tool2D.GetNavMeshPoint(item.Position);
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, item3.OwnerEntity))
				{
					ref LocalTransform valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, item3.OwnerEntity).ValueRW;
					float num = (navMeshPoint.z = valueRW.Position.z);
					valueRW.Position = navMeshPoint;
					FixedString32Bytes seName = "Teleport";
					entityCommandBuffer.AppendToBuffer(singletonEntity2, new SEData(DTool.GetSpellSEName(3116, in seName)));
					entityCommandBuffer.AppendToBuffer(singletonEntity, new GlobalParticleEmitParams
					{
						Position = navMeshPoint,
						Name = $"{3116}_SpellEndTeleport",
						Size = 1f
					});
				}
			}
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> item6 in IFE_1373312991_1.Query(__query_1373312991_1, __TypeHandle.__IFE_1373312991_1_TypeHandle, ref state))
		{
			item6.Deconstruct(out var _, out var entity);
			Entity entity2 = entity;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref state, entity2) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, entity2) && !InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, entity2).DisableSplitEffect)
			{
				Split(ref state, entity2, in spellSingleton, ref singletonRW.ValueRW, singletonBuffer, ref splitCountInThisFrame);
			}
			entityCommandBuffer.DestroyEntity(entity2);
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	private void Split(ref SystemState state, Entity entity, in SpellSingleton spellSingleton, ref GlobalRandom random, DynamicBuffer<SpellSpawnParams> shootBuffer, ref int splitCountInThisFrame)
	{
		SpellSplitComponentData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref state, entity);
		SpellConfigComponentData componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, entity);
		SpellSpawnParams elem = spellSingleton.SpellSpawnParamsStorage[entity].ToSplit(entity, componentAfterCompletingDependency);
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargeData_RO_ComponentLookup, ref state, entity))
		{
			elem.ChargeTimer = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargeData_RO_ComponentLookup, ref state, entity).ChargeTimer;
		}
		float3 position = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity).ValueRO.Position;
		float num = random.random.NextFloat(360f);
		int num2 = componentAfterCompletingDependency.Count;
		if (num2 > 0)
		{
			DynamicOptimizeData singleton = __query_1373312991_9.GetSingleton<DynamicOptimizeData>();
			int num3 = (singleton.IsMobilePlatform ? 45 : 90);
			int threshold = (singleton.IsMobilePlatform ? 60 : 120);
			float lowFpsActiveThreshold = SpellTools.GetLowFpsActiveThreshold(singleton.IsMobilePlatform);
			int num4 = SpellTools.CalculateSpellComplexity(componentAfterCompletingDependency2.AbilityType);
			if (singleton.IsLowFpsOptimizeActive(lowFpsActiveThreshold) || splitCountInThisFrame >= num3)
			{
				float currentFPS = singleton.CurrentFPS;
				float maxOptimizeFPSThreshold = SpellTools.GetMaxOptimizeFPSThreshold(singleton.IsMobilePlatform);
				float num5 = math.floor((float)num2 * (currentFPS / lowFpsActiveThreshold));
				num5 = SpellTools.GetFinalSpawnCountWithLimitCount(num3, 3, threshold, 1, splitCountInThisFrame, (int)num5);
				if (currentFPS <= maxOptimizeFPSThreshold || num5 < 1f)
				{
					num5 = 1f;
				}
				float num6 = (float)num2 / num5;
				elem.ConfigComponentData.Damage.MulRatio *= num6;
				elem.SpellEfficiency *= num6;
				num2 = (int)num5;
			}
			splitCountInThisFrame += num4 * num2;
		}
		for (int i = 0; i < num2; i++)
		{
			float num7 = num + 360f * ((float)i / (float)num2);
			elem.MovementComponentData.Direction = Tool2D.GetDir(num7);
			elem.MovementComponentData.AroundAngle = num7;
			elem.SourceShootDir = elem.MovementComponentData.Direction.xy;
			float num8 = elem.ConfigComponentData.Radius.Calculate();
			if (elem.MovementComponentData.IsFallSpell)
			{
				elem.SpawnPosition = position;
			}
			else if (num8 > 0f)
			{
				elem.SpawnPosition = position + elem.MovementComponentData.Direction * num8;
			}
			else
			{
				elem.SpawnPosition = position + elem.MovementComponentData.Direction * 0.333f;
			}
			switch (elem.ConfigComponentData.AbilityType)
			{
			case SpellAbilityType.JudgementBlade:
				elem.SpawnPosition = position + elem.MovementComponentData.Direction * (elem.MovementComponentData.IsFallSpell ? 0.5f : 1f);
				elem.MovementComponentData.Direction *= -1f;
				break;
			case SpellAbilityType.ManaCoin:
				elem.ConfigComponentData.Int3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell1020ManaCoinData_RW_ComponentLookup, ref state, entity).ValueRO.CoinUseCount;
				elem.ConfigComponentData.Float3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell1020ManaCoinData_RW_ComponentLookup, ref state, entity).ValueRO.BuffRatio;
				break;
			case SpellAbilityType.DimensionTraveller:
			{
				elem.MovementComponentData.Gravity = 0f;
				RefRO<SpellMovementComponentData> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, entity);
				elem.MovementComponentData.CurrentFallSpeed = componentROAfterCompletingDependency.ValueRO.CurrentFallSpeed;
				RefRO<Spell1029DimensionTravellerData> componentROAfterCompletingDependency2 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Spell1029DimensionTravellerData_RO_ComponentLookup, ref state, entity);
				elem.ConfigComponentData.Float2 = componentROAfterCompletingDependency2.ValueRO.BonusAddDamage;
				elem.ConfigComponentData.Float3 = componentROAfterCompletingDependency2.ValueRO.BonusDuration;
				break;
			}
			case SpellAbilityType.MagicBreaker:
				elem.ConfigComponentData.Int3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, entity).Int3;
				if (!elem.MovementComponentData.IsFallSpell)
				{
					elem.MovementComponentData.AroundCenter = position;
				}
				break;
			case SpellAbilityType.ThunderAura:
				elem.MovementComponentData.Gravity = 0f;
				elem.MovementComponentData.CurrentFallSpeed = 0f;
				break;
			}
			elem.SourceShootTargetPosition = elem.SpawnPosition.xy + elem.SourceShootDir;
			if (elem.MovementComponentData.Gravity != 0f)
			{
				elem.SpawnPosition.z = -0.1f;
			}
			if (elem.MovementComponentData.IsFallSpell)
			{
				float num9 = ((num8 > 0f) ? num8 : 1f);
				elem.MovementComponentData.FallTargetPosition = elem.SpawnPosition + elem.MovementComponentData.Direction * num9;
			}
			shootBuffer.Add(elem);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(SpellDestroyEventJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellDestroyEventJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellEndTeleportTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellDestroyTag>();
		__query_1373312991_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDestroyTag>();
		__query_1373312991_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1373312991_9 = entityQueryBuilder2.Build(ref state);
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
		((SpellDestroySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_000082A0_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpellDestroySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
