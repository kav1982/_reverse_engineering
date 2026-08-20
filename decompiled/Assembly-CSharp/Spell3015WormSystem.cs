using System;
using System.Runtime.CompilerServices;
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
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
internal struct Spell3015WormSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		public ComponentLookup<MatOverrideFrameIndex> __MatOverrideFrameIndex_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell3015WormJob.InternalCompilerQueryAndHandleData __Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellElementEffectComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__MatOverrideFrameIndex_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFrameIndex>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private EntityQuery wormQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_63469219_0;

	private EntityQuery __query_63469219_1;

	private EntityQuery __query_63469219_2;

	private EntityQuery __query_63469219_3;

	private EntityQuery __query_63469219_4;

	private EntityQuery __query_63469219_5;

	private EntityQuery __query_63469219_6;

	private EntityQuery __query_63469219_7;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.EntityManager.CreateSingletonBuffer<Spell3015WormSpawnBuffer>();
		wormQuery = state.EntityManager.CreateEntityQuery(typeof(Spell3015WormComponent));
	}

	public void OnDestroy(ref SystemState state)
	{
		wormQuery.Dispose();
	}

	public void OnUpdate(ref SystemState state)
	{
		NativeArray<Spell3015WormSpawnBuffer> nativeArray = __query_63469219_0.GetSingletonBuffer<Spell3015WormSpawnBuffer>().ToNativeArray(Allocator.Temp);
		foreach (Spell3015WormSpawnBuffer item in nativeArray)
		{
			Entity entity = state.EntityManager.Instantiate(__query_63469219_1.GetSingleton<SpellSingleton>().Prefabs["Spell_3015_Worm"]);
			state.EntityManager.SetComponentData(entity, new LocalTransform
			{
				Position = Tool2D.GetNavMeshPointIngoreZ(item.spawnPosition),
				Rotation = quaternion.identity,
				Scale = 1f
			});
			SpellConfigComponentData config = item.config;
			config.AbilityType = SpellAbilityType.ParasiticWorm;
			config.Id = 3015;
			config.ShooterType = UnitType.Teammate;
			config.ColorType = SpellColorType.Player;
			state.EntityManager.SetComponentData(entity, item.data);
			state.EntityManager.SetComponentData(entity, config);
			state.EntityManager.SetComponentData(entity, item.element);
			item.wormColorType.ColorEnumToString(out var result);
			Entity entity2 = state.EntityManager.Instantiate(__query_63469219_1.GetSingleton<SpellSingleton>().Prefabs[$"3015_Worm_{result}"]);
			state.EntityManager.AddComponent<Parent>(entity2);
			state.EntityManager.SetComponentData(entity2, new Parent
			{
				Value = entity
			});
			if (!state.EntityManager.HasBuffer<LinkedEntityGroup>(entity))
			{
				state.EntityManager.AddBuffer<LinkedEntityGroup>(entity);
			}
			else
			{
				state.EntityManager.GetBuffer<LinkedEntityGroup>(entity).Add(new LinkedEntityGroup
				{
					Value = entity2
				});
			}
			SpellConfigComponentData config2 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, entity).ValueRO;
			config2.DamageInterval = 0f;
			SpellMovementComponentData movement = default(SpellMovementComponentData);
			TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config2, in movement, in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity).ValueRO, in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, entity).ValueRO, in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, entity).ValueRO, out var info);
			state.EntityManager.SetComponentData(entity, new Spell3015WormComponent
			{
				damageInfo = info,
				wormInfo = item,
				idleTimer = 0f,
				randomMoveTimer = 3f,
				exitTimer = 6f + item.config.Duration.AddBase,
				randomMoveTargetPoint = item.spawnPosition + (float3)Tool2D.GetDir() * UnityEngine.Random.Range(2f, 4f),
				meshEntity = state.EntityManager.GetComponentData<EffectsCollectorData>(entity2).Effect1,
				checkIntervalTimer = __query_63469219_2.GetSingleton<GlobalRandom>().NewRandom().NextFloat(0f, 0.5f)
			});
		}
		if (!wormQuery.IsEmpty)
		{
			RefRW<GlobalRandom> singletonRW = __query_63469219_3.GetSingletonRW<GlobalRandom>();
			singletonRW.ValueRW.NewRandom();
			state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell3015WormJob
			{
				deltaTime = state.WorldUnmanaged.Time.DeltaTime,
				currentRoomEntitiesSingleton = __query_63469219_4.GetSingleton<CurrentRoomEntitiesSingleton>(),
				matOverrideFrameIndexLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideFrameIndex_RW_ComponentLookup, ref state),
				localTransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
				CMD = __query_63469219_5.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
				spellSingleton = __query_63469219_1.GetSingleton<SpellSingleton>(),
				gRandom = singletonRW,
				PhysicsWorld = __query_63469219_6.GetSingleton<PhysicsWorldSingleton>(),
				UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
				SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
				GlobalParticleEntity = __query_63469219_7.GetSingletonEntity()
			}, __TypeHandle.__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
			state.Dependency.Complete();
			nativeArray.Dispose();
			__query_63469219_0.GetSingletonBuffer<Spell3015WormSpawnBuffer>().Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell3015WormJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell3015WormJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3015WormSpawnBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_63469219_7 = entityQueryBuilder2.Build(ref state);
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
		((Spell3015WormSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell3015WormSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell3015WormSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell3015WormSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
