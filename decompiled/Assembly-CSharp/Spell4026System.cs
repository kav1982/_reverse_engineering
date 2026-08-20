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

[CompilerGenerated]
[WithNone(new Type[] { typeof(SpellFallTag) })]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
internal struct Spell4026System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1538284705_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4026GreenRuneData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4026GreenRuneData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4026GreenRuneData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4026GreenRuneData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1538284705_0.TypeHandle __IFE_1538284705_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentLookup;

		public Spell4026Job.InternalCompilerQueryAndHandleData __Spell4026Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1538284705_0_TypeHandle = new IFE_1538284705_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellElementEffectComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>();
			__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007B93_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007B93_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007B93_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00007B94_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007B94_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007B94_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private float explosionMinInterval;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1538284705_0;

	private EntityQuery __query_1538284705_1;

	private EntityQuery __query_1538284705_2;

	private EntityQuery __query_1538284705_3;

	private EntityQuery __query_1538284705_4;

	private EntityQuery __query_1538284705_5;

	private EntityQuery __query_1538284705_6;

	private EntityQuery __query_1538284705_7;

	private EntityQuery __query_1538284705_8;

	private EntityQuery __query_1538284705_9;

	private EntityQuery __query_1538284705_10;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		explosionMinInterval = 0f;
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		DynamicOptimizeData singleton = __query_1538284705_2.GetSingleton<DynamicOptimizeData>();
		float num = state.WorldUnmanaged.Time.DeltaTime * singleton.LastFrameTimeScale;
		if (explosionMinInterval > 0f)
		{
			explosionMinInterval -= num;
		}
		EntityCommandBuffer CMD = __query_1538284705_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		PlayerController_Dots singleton2 = __query_1538284705_4.GetSingleton<PlayerController_Dots>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item6 in IFE_1538284705_0.Query(__query_1538284705_0, __TypeHandle.__IFE_1538284705_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var _, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW4 = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.IsRuneBall || uncheckedRefRW.ValueRW.ReadyToExplosion || uncheckedRefRW4.ValueRW.IsFallSpell)
			{
				continue;
			}
			float3 position = float3.zero;
			if (uncheckedRefRW4.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				uncheckedRefRW.ValueRW.CurrentAngle = math.lerp(uncheckedRefRW.ValueRO.CurrentAngle, uncheckedRefRW.ValueRO.TargetAngle, 10f * num);
				float3 dir = DTool.GetDir(uncheckedRefRW.ValueRW.CurrentAngle * (MathF.PI / 180f));
				uncheckedRefRW.ValueRW.CurrentAngle = uncheckedRefRW.ValueRO.TargetAngle;
				uncheckedRefRW4.ValueRW.Direction = math.normalizesafe(dir);
				position = uncheckedRefRW4.ValueRW.UpdateAroundFollowAndGetAroundPositionWhenAround(InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state));
				position.z = uncheckedRefRW2.ValueRW.Position.z;
				uncheckedRefRW2.ValueRW.Position = position;
				uncheckedRefRW4.ValueRW.AroundAngle = uncheckedRefRW.ValueRW.CurrentAngle;
			}
			else
			{
				float3 dir2 = DTool.GetDir(uncheckedRefRW.ValueRW.CurrentAngle * (MathF.PI / 180f));
				uncheckedRefRW.ValueRW.CurrentAngle = uncheckedRefRW.ValueRO.TargetAngle;
				uncheckedRefRW4.ValueRW.Direction = math.normalizesafe(dir2);
				position.z = uncheckedRefRW2.ValueRW.Position.z;
				Entity singletonEntity = __query_1538284705_4.GetSingletonEntity();
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, singletonEntity))
				{
					LocalTransform valueRW = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, singletonEntity).ValueRW;
					float num2 = 0.5f;
					position = valueRW.Position + new float3(math.cos(uncheckedRefRW.ValueRW.CurrentAngle * (MathF.PI / 180f)) * num2 * 1.8f, math.sin(uncheckedRefRW.ValueRW.CurrentAngle * (MathF.PI / 180f)) * num2 * 0.55f + 0.19f, -0.19f) * valueRW.Scale;
				}
			}
			if (!uncheckedRefRW.ValueRO.DirectExplosion)
			{
				uncheckedRefRW2.ValueRW.Position = position;
			}
			if (uncheckedRefRW4.ValueRO.Type != SpellSpecialMovementType.Rotation && !uncheckedRefRW.ValueRO.DirectExplosion && explosionMinInterval > 0f)
			{
				continue;
			}
			__query_1538284705_5.GetSingleton<CurrentRoomEntitiesSingleton>().FindNearestTarget(uncheckedRefRW2.ValueRO.Position, UnitType.Player, out var target, out var targetPosition, out var _);
			if (!uncheckedRefRW.ValueRO.DirectExplosion)
			{
				if (!(target != Entity.Null))
				{
					continue;
				}
				CheckIsTargetPositionInRange(uncheckedRefRW4.ValueRW.Type, targetPosition, singleton2.mousePosition, uncheckedRefRW2.ValueRO.Position, uncheckedRefRW3.ValueRO.Radius.Calculate(), out var isInRange);
				if (!isInRange)
				{
					continue;
				}
			}
			uncheckedRefRW.ValueRW.ReadyToExplosion = true;
			float z = uncheckedRefRW2.ValueRO.Position.z;
			if (!uncheckedRefRW.ValueRW.DirectExplosion)
			{
				switch (uncheckedRefRW4.ValueRO.Type)
				{
				case SpellSpecialMovementType.ChaseEnemy:
					uncheckedRefRW2.ValueRW.Position = targetPosition;
					break;
				case SpellSpecialMovementType.ChaseMouse:
					uncheckedRefRW2.ValueRW.Position = singleton2.mousePosition;
					break;
				}
			}
			uncheckedRefRW2.ValueRW.Position.z = z;
			SpellSingleton singleton3 = __query_1538284705_6.GetSingleton<SpellSingleton>();
			float randomAngle = __query_1538284705_7.GetSingleton<GlobalRandom>().NewRandom().NextFloat(0f, 360f);
			SpawnRuneExplosionOnTargetPosition(ref CMD, uncheckedRefRW2.ValueRO.Position, uncheckedRefRW3.ValueRW, randomAngle, entity2, singleton3);
			if (uncheckedRefRW.ValueRW.BonusSpawnCount > 0)
			{
				GlobalRandom singleton4 = __query_1538284705_7.GetSingleton<GlobalRandom>();
				EntityQuery _query_1538284705_ = __query_1538284705_1;
				NativeArray<Entity> nativeArray = _query_1538284705_.ToEntityArray(Allocator.Temp);
				for (int num3 = nativeArray.Length - 1; num3 > 0; num3--)
				{
					int num4 = singleton4.NewRandom().NextInt(0, num3 + 1);
					int index = num3;
					int index2 = num4;
					entity = nativeArray[num4];
					Entity entity3 = nativeArray[num3];
					Entity entity5 = (nativeArray[index] = entity);
					entity5 = (nativeArray[index2] = entity3);
				}
				for (int i = 0; i < math.min(nativeArray.Length, uncheckedRefRW.ValueRW.BonusSpawnCount); i++)
				{
					SpawnRuneExplosionOnTargetPosition(ref CMD, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, nativeArray[i]).Position, uncheckedRefRW3.ValueRW, randomAngle, entity2, singleton3);
				}
			}
			CMD.SetComponentEnabled<SpellDestroyTag>(entity2, value: true);
			if (uncheckedRefRW4.ValueRO.Type != SpellSpecialMovementType.Rotation && !uncheckedRefRW.ValueRO.DirectExplosion)
			{
				explosionMinInterval += 0.06f;
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4026Job
		{
			CMD = __query_1538284705_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			Physics = __query_1538284705_8.GetSingleton<PhysicsWorldSingleton>(),
			SpellElementLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentLookup, ref state),
			GlobalParticleEmitBufferEntity = __query_1538284705_9.GetSingletonEntity(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime
		}, __TypeHandle.__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[BurstCompile]
	public void SpawnRuneExplosionOnTargetPosition(ref EntityCommandBuffer CMD, float3 spawnPosition, SpellConfigComponentData config, float randomAngle, Entity entity, SpellSingleton spellSingleton)
	{
		for (int i = 0; i < config.Int2; i++)
		{
			float degree = (randomAngle + 360f / (float)math.max(1, config.Int2) * (float)i) * (MathF.PI / 180f);
			if (config.Int2 > 1)
			{
				spawnPosition += DTool.GetDir(degree) * config.Radius.Calculate() / 1.5f;
			}
			SpellSpawnParams element = spellSingleton.SpellSpawnParamsStorage[entity].BuildGreenRuneExplosion(spawnPosition);
			CMD.AppendToBuffer(__query_1538284705_10.GetSingletonEntity(), element);
		}
	}

	[BurstCompile]
	public void CheckIsTargetPositionInRange(SpellSpecialMovementType movement, float3 targetUnitPosition, float3 playerMousePosition, float3 currentPosition, float explosionRadius, out bool isInRange)
	{
		switch (movement)
		{
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.Rotation:
		case SpellSpecialMovementType.ChaseOwner:
			isInRange = DTool.IgnoreZDistance(in currentPosition, in targetUnitPosition) <= explosionRadius / 1.5f;
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			isInRange = DTool.IgnoreZDistance(in currentPosition, in targetUnitPosition) <= explosionRadius * 1.5f;
			break;
		case SpellSpecialMovementType.ChaseMouse:
			isInRange = DTool.IgnoreZDistance(in currentPosition, in playerMousePosition) <= explosionRadius * 1.5f;
			break;
		default:
			isInRange = false;
			break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4026Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4026Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4026GreenRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1538284705_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateData, LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithDisabled<SpellDestroyTag>();
		__query_1538284705_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284705_10 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00007B93_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007B94_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4026System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4026System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4026System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
