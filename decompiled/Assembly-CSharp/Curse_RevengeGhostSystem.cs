using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[CompilerGenerated]
[BurstCompile]
public struct Curse_RevengeGhostSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_636245500_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<StatefulTriggerEvent> item3_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Curse_RevengeGhost>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Curse_RevengeGhost> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			private BufferTypeHandle<StatefulTriggerEvent> item3_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Curse_RevengeGhost>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Curse_RevengeGhost>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_636245500_0.TypeHandle __IFE_636245500_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_636245500_0_TypeHandle = new IFE_636245500_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00004F1C_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00004F1C_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00004F1C_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_636245500_0;

	private EntityQuery __query_636245500_1;

	private EntityQuery __query_636245500_2;

	private EntityQuery __query_636245500_3;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Curse_RevengeGhost>();
	}

	public void OnUpdate(ref SystemState state)
	{
		CurrentRoomEntitiesSingleton singleton = __query_636245500_1.GetSingleton<CurrentRoomEntitiesSingleton>();
		RefRW<GlobalRandom> singletonRW = __query_636245500_2.GetSingletonRW<GlobalRandom>();
		EntityCommandBuffer entityCommandBuffer = __query_636245500_3.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> item4 in IFE_636245500_0.Query(__query_636245500_0, __TypeHandle.__IFE_636245500_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Curse_RevengeGhost> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item2;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item3;
			Entity entity2 = entity;
			if (uncheckedRefRW.ValueRW.isHit)
			{
				continue;
			}
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW.ValueRW.efGO = ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_RevengeGhostMono", uncheckedRefRW2.ValueRW.Position);
				uncheckedRefRW.ValueRW.currentDir = DTool.GetDir(ref singletonRW.ValueRW.random);
			}
			float3 layerPosition = DTool.GetLayerPosition(in uncheckedRefRW2.ValueRW.Position, LayerCorrectType.Coordinate);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Layer).ValueRW.Position = layerPosition;
			uncheckedRefRW.ValueRW.efGO.Value.transform.position = uncheckedRefRW2.ValueRW.Position + layerPosition;
			uncheckedRefRW.ValueRW.rotateSpeed += uncheckedRefRW.ValueRW.rotateAccekeration * state.WorldUnmanaged.Time.DeltaTime;
			if (uncheckedRefRW.ValueRW.rotateSpeed > uncheckedRefRW.ValueRW.rotateMaxSpeed)
			{
				uncheckedRefRW.ValueRW.rotateSpeed = uncheckedRefRW.ValueRW.rotateMaxSpeed;
			}
			uncheckedRefRW.ValueRW.checkIntervalTimer += state.WorldUnmanaged.Time.DeltaTime;
			float3 targetPosition;
			if (uncheckedRefRW.ValueRW.checkIntervalTimer >= uncheckedRefRW.ValueRW.checkTargetInterval)
			{
				uncheckedRefRW.ValueRW.checkIntervalTimer = 0f;
				if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.targetEtt))
				{
					singleton.FindNearestTarget(uncheckedRefRW2.ValueRW.Position, UnitType.Monster, out uncheckedRefRW.ValueRW.targetEtt, out targetPosition, out var _);
				}
			}
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.targetEtt))
			{
				ref Curse_RevengeGhost valueRW = ref uncheckedRefRW.ValueRW;
				ref float3 currentDir = ref uncheckedRefRW.ValueRW.currentDir;
				LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.targetEtt);
				targetPosition = DTool.IgnoreZDir(in componentAfterCompletingDependency.Position, in uncheckedRefRW2.ValueRW.Position);
				valueRW.currentDir = DTool.DirMoveTowardsIgnoreZ(in currentDir, in targetPosition, uncheckedRefRW.ValueRW.rotateSpeed * state.WorldUnmanaged.Time.DeltaTime);
			}
			else
			{
				uncheckedRefRW.ValueRW.currentDir = DTool.GetDir(in uncheckedRefRW.ValueRW.currentDir, uncheckedRefRW.ValueRW.rotateSpeed * state.WorldUnmanaged.Time.DeltaTime);
			}
			uncheckedRefRW2.ValueRW.Position += uncheckedRefRW.ValueRW.currentDir * uncheckedRefRW.ValueRW.moveSpeed * state.WorldUnmanaged.Time.DeltaTime;
			uncheckedRefRW.ValueRW.durationTimer += state.WorldUnmanaged.Time.DeltaTime;
			if (uncheckedRefRW.ValueRW.durationTimer >= uncheckedRefRW.ValueRW.duration)
			{
				ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.efGO);
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_RevengeGhostMono_Hit", uncheckedRefRW2.ValueRW.Position, 2f);
				entityCommandBuffer.DestroyEntity(entity2);
				continue;
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (dynamicBuffer[i].State == StatefulEventState.Enter)
				{
					Entity targetEtt = dynamicBuffer[i].GetOtherEntity(entity2);
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, targetEtt) && InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, targetEtt).unitCfg.IsSameCamp(UnitType.Player))
					{
						TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						damageInfo.damage = uncheckedRefRW.ValueRW.damage;
						damageInfo.knockbackForce = uncheckedRefRW.ValueRW.currentDir * uncheckedRefRW.ValueRW.knockback;
						UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, state.EntityManager);
						uncheckedRefRW.ValueRW.isHit = true;
						ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.efGO);
						ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_RevengeGhostMono_Hit", uncheckedRefRW2.ValueRW.Position, 2f);
						entityCommandBuffer.DestroyEntity(entity2);
						break;
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Curse_RevengeGhost>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		__query_636245500_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_636245500_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_636245500_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_636245500_3 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00004F1C_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Curse_RevengeGhostSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Curse_RevengeGhostSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Curse_RevengeGhostSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
