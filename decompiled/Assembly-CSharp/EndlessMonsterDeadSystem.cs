using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitTakeDamageDeadSystem))]
[CompilerGenerated]
internal struct EndlessMonsterDeadSystem : ISystem, ISystemCompilerGenerated
{
	public struct traceRequest
	{
		public float3 position;

		public float size;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_241435421_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<EndlessMonsterTag>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<UnitProperty_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<EndlessMonsterTag> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<UnitProperty_Dots> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<UnitProperty_Dots> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<EndlessMonsterTag>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitProperty_Dots>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<EndlessMonsterTag>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_241435421_0.TypeHandle __IFE_241435421_0_TypeHandle;

		public ComponentLookup<Monster316RingEffect> __Monster316RingEffect_RW_ComponentLookup;

		public ComponentLookup<Monster314RingEffect> __Monster314RingEffect_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_241435421_0_TypeHandle = new IFE_241435421_0.TypeHandle(ref state);
			__Monster316RingEffect_RW_ComponentLookup = state.GetComponentLookup<Monster316RingEffect>();
			__Monster314RingEffect_RW_ComponentLookup = state.GetComponentLookup<Monster314RingEffect>();
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_241435421_0;

	private EntityQuery __query_241435421_1;

	private EntityQuery __query_241435421_2;

	private EntityQuery __query_241435421_3;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndlessMonsterTag>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (!GameMgr.InEndlessMode)
		{
			return;
		}
		NativeList<float3> nativeList = new NativeList<float3>(Allocator.Temp);
		NativeList<int> nativeList2 = new NativeList<int>(Allocator.Temp);
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = __query_241435421_1.GetSingletonBuffer<GlobalParticleEmitParams>();
		NativeList<Entity> nativeList3 = new NativeList<Entity>(Allocator.Temp);
		NativeList<float3> nativeList4 = new NativeList<float3>(Allocator.Temp);
		NativeList<traceRequest> nativeList5 = new NativeList<traceRequest>(Allocator.Temp);
		ComponentLookup<Monster316RingEffect> componentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Monster316RingEffect_RW_ComponentLookup, ref state);
		ComponentLookup<Monster314RingEffect> componentLookup2 = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Monster314RingEffect_RW_ComponentLookup, ref state);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item5 in IFE_241435421_0.Query(__query_241435421_0, __TypeHandle.__IFE_241435421_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRO<EndlessMonsterTag> uncheckedRefRO = item;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots> uncheckedRefRO2 = item3;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO3 = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRO.isDead)
			{
				continue;
			}
			float3 position = Tool2D.GetLayerPoint(uncheckedRefRO3.ValueRO.Position) + new Vector3(0f, 0f, -0.005f);
			if (uncheckedRefRO2.ValueRO.ett_Motion != Entity.Null)
			{
				position = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state, uncheckedRefRO2.ValueRO.ett_Motion).ValueRO.Position;
			}
			traceRequest value = new traceRequest
			{
				position = uncheckedRefRO3.ValueRO.Position,
				size = uncheckedRefRW.ValueRO.size
			};
			nativeList5.Add(in value);
			singletonBuffer.Add(new GlobalParticleEmitParams(GlobalParticleType.EF, "EF_EndlessMonsterDead", position)
			{
				Size = uncheckedRefRW.ValueRO.size
			});
			if (uncheckedRefRO.ValueRO.has316Buff)
			{
				nativeList3.Add(in uncheckedRefRO.ValueRO.buffEntity);
			}
			SEMgr.Inst.endlessMonsterDead.PlaySE(uncheckedRefRO3.ValueRO.Position, SEPlayMode.Replay, 5).pitch = UnityEngine.Random.Range(0.9f, 1f);
			if (uncheckedRefRW.ValueRO.id == 131601 && componentLookup.TryGetComponent(entity2, out var componentData) && componentData.ringEffect.IsValid())
			{
				ObjPoolMgr.Inst.RecycleGO(componentData.ringEffect.Value);
			}
			if (uncheckedRefRW.ValueRO.id == 131401 && componentLookup2.TryGetComponent(entity2, out var componentData2) && componentData2.ringEffect.IsValid())
			{
				ObjPoolMgr.Inst.RecycleGO(componentData2.ringEffect.Value);
			}
			if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
			{
				uncheckedRefRW.ValueRW.unitCfg.triggerDeadEvent = false;
				continue;
			}
			float3 value2;
			if (uncheckedRefRW.ValueRO.id == 132001)
			{
				value2 = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRO3.ValueRO.Position);
				nativeList4.Add(in value2);
			}
			value2 = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRO3.ValueRO.Position);
			nativeList.Add(in value2);
			int value3 = uncheckedRefRO.ValueRO.dropCount;
			nativeList2.Add(in value3);
		}
		AllMixedEtt singleton = __query_241435421_2.GetSingleton<AllMixedEtt>();
		foreach (traceRequest item6 in nativeList5)
		{
			Entity entity3 = state.EntityManager.Instantiate(singleton.map["EF_EndlessMonsterDeadTrace"]);
			ref LocalTransform valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW;
			valueRW.Position = Tool2D.GetLayerPoint(item6.position, LayerCorrectType.GroundEffect);
			valueRW.Scale = item6.size;
		}
		nativeList5.Dispose();
		foreach (Entity item7 in nativeList3)
		{
			state.EntityManager.DestroyEntity(item7);
		}
		nativeList3.Dispose();
		__query_241435421_3.GetSingletonBuffer<TextFloatVFXBED>();
		for (int i = 0; i < nativeList.Length; i++)
		{
			PlayerMgr.Inst.ChangeGear(nativeList2[i]);
			PlayerMgr.Inst.ChangeCoin(nativeList2[i]);
			UIPlayerEndlessPick.Inst.OnCoinChange(nativeList2[i]);
		}
		nativeList.Dispose();
		nativeList2.Dispose();
		if (nativeList4.Length > 0)
		{
			for (int j = 0; j < nativeList4.Length; j++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr.GetEndlessChestDrone(), nativeList4[j]);
			}
		}
	}

	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndlessMonsterTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_241435421_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_241435421_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_241435421_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_241435421_3 = entityQueryBuilder2.Build(ref state);
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
		((EndlessMonsterDeadSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((EndlessMonsterDeadSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((EndlessMonsterDeadSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((EndlessMonsterDeadSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
