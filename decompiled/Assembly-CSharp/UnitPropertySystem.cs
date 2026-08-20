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
using UnityEngine;

[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public struct UnitPropertySystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_659727650_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<UnitProperty_Dots> item1_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_659727650_0.TypeHandle __IFE_659727650_0_TypeHandle;

		public ComponentLookup<Relic_ShowUnitHP> __Relic_ShowUnitHP_RW_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		public ComponentLookup<PlayerController_Dots> __PlayerController_Dots_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public UnitPropertyJob.InternalCompilerQueryAndHandleData __UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_659727650_0_TypeHandle = new IFE_659727650_0.TypeHandle(ref state);
			__Relic_ShowUnitHP_RW_ComponentLookup = state.GetComponentLookup<Relic_ShowUnitHP>();
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__PlayerController_Dots_RW_ComponentLookup = state.GetComponentLookup<PlayerController_Dots>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private ComponentLookup<LocalTransform> cluLocalTsf;

	private ComponentLookup<MatOverrideColor> cluMOC;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_659727650_0;

	private EntityQuery __query_659727650_1;

	private EntityQuery __query_659727650_2;

	private EntityQuery __query_659727650_3;

	private EntityQuery __query_659727650_4;

	private EntityQuery __query_659727650_5;

	private EntityQuery __query_659727650_6;

	private EntityQuery __query_659727650_7;

	private EntityQuery __query_659727650_8;

	private EntityQuery __query_659727650_9;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<AllMixedEtt>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GetGOByJobBED>();
		state.RequireForUpdate<TeammateGhostEffectData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<UITextFloatByJobBED>();
		state.RequireForUpdate<TextFloatVFXBED>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<DamageRecordBuffer>();
		cluLocalTsf = state.GetComponentLookup<LocalTransform>();
		cluMOC = state.GetComponentLookup<MatOverrideColor>();
	}

	public void OnUpdate(ref SystemState state)
	{
		AllMixedEtt singleton = __query_659727650_1.GetSingleton<AllMixedEtt>();
		PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
		int num = ((itemCtrller.relicCfg_ShowUnitHPUI != null) ? itemCtrller.relicCfg_ShowUnitHPUI.level : 0);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>> item2 in IFE_659727650_0.Query(__query_659727650_0, __TypeHandle.__IFE_659727650_0_TypeHandle, ref state))
		{
			item2.Deconstruct(out var item, out var entity);
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW = item;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.registered)
			{
				if (uncheckedRefRW.ValueRO.id == 0)
				{
					Debug.LogError("有单位未指定id");
					continue;
				}
				uncheckedRefRW.ValueRW.registered = true;
				uncheckedRefRW.ValueRW.Initialize(UnitConfig.map);
				LevelMgr.Inst.CurrentRoomCtrller.UnitRegister(entity2);
			}
			if (num > 0 && !uncheckedRefRW.ValueRW.isRelicShowUnitHPCreate && uncheckedRefRW.ValueRO.unitCfg.relicShowHPUIHight > 0f)
			{
				uncheckedRefRW.ValueRW.isRelicShowUnitHPCreate = true;
				if (uncheckedRefRW.ValueRW.unitCfg.unitType == UnitType.Monster || uncheckedRefRW.ValueRW.unitCfg.unitType == UnitType.Teammate || uncheckedRefRW.ValueRW.unitCfg.unitType == UnitType.TeammateNotAttack)
				{
					uncheckedRefRW.ValueRW.ett_RelicShowUnitHP = state.EntityManager.Instantiate(singleton.map["Relic_ShowUnitHP"]);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Relic_ShowUnitHP_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_RelicShowUnitHP).ValueRW.Initialized(entity2, num);
				}
			}
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		cluLocalTsf.Update(ref state);
		cluMOC.Update(ref state);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new UnitPropertyJob
		{
			cluLocalTsf = cluLocalTsf,
			cluMOC = cluMOC,
			gRandom = __query_659727650_2.GetSingletonRW<GlobalRandom>(),
			deltaTime = state.WorldUnmanaged.Time.DeltaTime,
			ecb = entityCommandBuffer.AsParallelWriter(),
			textFloatVFXBufferEtt = __query_659727650_3.GetSingletonEntity(),
			uiTextFloatByJobBufferEtt = __query_659727650_4.GetSingletonEntity(),
			getGOByJobEtt = __query_659727650_5.GetSingletonEntity(),
			TeammateDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state),
			SpellEffectEntity = __query_659727650_6.GetSingletonEntity(),
			spellSingleton = __query_659727650_7.GetSingleton<SpellSingleton>(),
			ColliderLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state),
			playerControllerLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__PlayerController_Dots_RW_ComponentLookup, ref state),
			unitPptLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			TeammateGhostEffectEntity = __query_659727650_8.GetSingletonEntity(),
			DamageRecordSingletonBufferEntity = __query_659727650_9.GetSingletonEntity(),
			settingNeedTextFloat = DataMgr.settingData.textFloat,
			isSupportVFX = GameMgr.IsSupportVFX,
			relic_SeckillChance = ((itemCtrller.relicCfg_Seckill != null) ? (itemCtrller.relicCfg_Seckill.float1.result / 100f) : 0f),
			relic_ResurgenceExist = (itemCtrller.relic_Resurgence != null),
			curse_MonsterRecoverHPPerSecond = ((itemCtrller.curseCfg_MonsterRecover != null) ? itemCtrller.curseCfg_MonsterRecover.int1.result : 0)
		}, __TypeHandle.__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.CompleteDependency();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(UnitPropertyJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__UnitPropertyJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitProperty_Dots>();
		__query_659727650_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<UITextFloatByJobBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GetGOByJobBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateGhostEffectData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DamageRecordBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_659727650_9 = entityQueryBuilder2.Build(ref state);
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
		((UnitPropertySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((UnitPropertySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((UnitPropertySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
