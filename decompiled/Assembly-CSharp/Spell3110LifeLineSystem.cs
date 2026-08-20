using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
internal struct Spell3110LifeLineSystem : ISystem, ISystemCompilerGenerated
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
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		public ComponentLookup<Spell3110LifeLineComponent> __Spell3110LifeLineComponent_RW_ComponentLookup;

		public ComponentLookup<Spell3110LivingTieComponent> __Spell3110LivingTieComponent_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		public ComponentLookup<MatOverrideOffsetFloat> __MatOverrideOffsetFloat_RW_ComponentLookup;

		public Spell3110LifeLineSystemJob.InternalCompilerQueryAndHandleData __Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellElementEffectComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__Spell3110LifeLineComponent_RW_ComponentLookup = state.GetComponentLookup<Spell3110LifeLineComponent>();
			__Spell3110LivingTieComponent_RW_ComponentLookup = state.GetComponentLookup<Spell3110LivingTieComponent>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
			__MatOverrideOffsetFloat_RW_ComponentLookup = state.GetComponentLookup<MatOverrideOffsetFloat>();
			__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private EntityQuery lifeLineQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_150133181_0;

	private EntityQuery __query_150133181_1;

	private EntityQuery __query_150133181_2;

	private EntityQuery __query_150133181_3;

	private EntityQuery __query_150133181_4;

	private EntityQuery __query_150133181_5;

	private EntityQuery __query_150133181_6;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<Spell3110LifeLineSpawnBuffer>();
		state.RequireForUpdate<Spell3110LifeLineDestoryBuffer>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.EntityManager.CreateSingletonBuffer<Spell3110LifeLineSpawnBuffer>();
		state.EntityManager.CreateSingletonBuffer<Spell3110LifeLineDestoryBuffer>();
		lifeLineQuery = state.EntityManager.CreateEntityQuery(typeof(Spell3110LifeLineComponent));
	}

	public void OnDestroy(ref SystemState state)
	{
		lifeLineQuery.Dispose();
	}

	public void OnUpdate(ref SystemState state)
	{
		DynamicBuffer<Spell3110LifeLineSpawnBuffer> singletonBuffer = __query_150133181_0.GetSingletonBuffer<Spell3110LifeLineSpawnBuffer>();
		if (singletonBuffer.Length > 0)
		{
			NativeArray<Spell3110LifeLineSpawnBuffer> nativeArray = singletonBuffer.ToNativeArray(Allocator.Temp);
			foreach (Spell3110LifeLineSpawnBuffer item in nativeArray)
			{
				Entity entity = state.EntityManager.Instantiate(__query_150133181_1.GetSingleton<SpellSingleton>().Prefabs["Spell_3110"]);
				SpellComponentData data = item.data;
				data.DisableSplitEffect = true;
				state.EntityManager.SetComponentData(entity, data);
				state.EntityManager.SetComponentData(entity, item.config);
				state.EntityManager.SetComponentData(entity, item.element);
				item.lifeLineColorType.ColorEnumToString(out var result);
				Entity entity2 = state.EntityManager.Instantiate(__query_150133181_1.GetSingleton<SpellSingleton>().Prefabs[$"3110_LifeLine_{result}"]);
				state.EntityManager.SetComponentEnabled<EnterDoorDestroy>(entity, value: false);
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
				ref readonly SpellConfigComponentData valueRO = ref InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, entity).ValueRO;
				SpellMovementComponentData movement = default(SpellMovementComponentData);
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in valueRO, in movement, in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity).ValueRO, in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state, entity).ValueRO, in data, out var info);
				EntityManager entityManager = state.EntityManager;
				Spell3110LifeLineComponent componentData = new Spell3110LifeLineComponent
				{
					linkTarget1 = item.linkTarget1,
					linkTarget2 = item.linkTarget2,
					damageInfo = info
				};
				RefRO<EffectsCollectorData> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, entity2);
				componentData.line = componentROAfterCompletingDependency.ValueRO.Effect1;
				componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, entity2);
				componentData.shadow = componentROAfterCompletingDependency.ValueRO.Effect2;
				componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, entity2);
				_ = ref componentROAfterCompletingDependency.ValueRO;
				componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, entity2);
				componentData.fire = componentROAfterCompletingDependency.ValueRO.Effect3;
				componentData.distancePocess = 1f;
				entityManager.SetComponentData(entity, componentData);
				Entity entity3 = state.EntityManager.Instantiate(__query_150133181_1.GetSingleton<SpellSingleton>().Prefabs[$"3110_LivingTieCross_{result}"]);
				Entity tie = state.EntityManager.Instantiate(__query_150133181_1.GetSingleton<SpellSingleton>().Prefabs[$"3110_LivingTie_{result}"]);
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell3110LifeLineComponent_RW_ComponentLookup, ref state, entity).ValueRW.tie1 = entity3;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell3110LifeLineComponent_RW_ComponentLookup, ref state, entity).ValueRW.tie2 = tie;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell3110LivingTieComponent_RW_ComponentLookup, ref state, entity3).ValueRW.starting = true;
			}
			nativeArray.Dispose();
			__query_150133181_0.GetSingletonBuffer<Spell3110LifeLineSpawnBuffer>().Clear();
		}
		DynamicBuffer<Spell3110LifeLineDestoryBuffer> singletonBuffer2 = __query_150133181_2.GetSingletonBuffer<Spell3110LifeLineDestoryBuffer>();
		if (singletonBuffer2.Length > 0)
		{
			NativeArray<Spell3110LifeLineDestoryBuffer> nativeArray2 = singletonBuffer2.ToNativeArray(Allocator.Temp);
			foreach (Spell3110LifeLineDestoryBuffer item2 in nativeArray2)
			{
				state.EntityManager.DestroyEntity(item2.spellEntity);
				state.EntityManager.DestroyEntity(item2.spell.tie1);
				state.EntityManager.DestroyEntity(item2.spell.tie2);
			}
			nativeArray2.Dispose();
			__query_150133181_2.GetSingletonBuffer<Spell3110LifeLineDestoryBuffer>().Clear();
		}
		if (!lifeLineQuery.IsEmpty)
		{
			state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell3110LifeLineSystemJob
			{
				deltaTime = state.WorldUnmanaged.Time.DeltaTime,
				physicsWorldSingleton = __query_150133181_3.GetSingleton<PhysicsWorldSingleton>(),
				localTransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
				unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
				configLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
				CMD = __query_150133181_4.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
				DestorySpellBufferEntity = __query_150133181_5.GetSingletonEntity(),
				postTransformMatrixLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref state),
				livingTieLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell3110LivingTieComponent_RW_ComponentLookup, ref state),
				matOverrideLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideOffsetFloat_RW_ComponentLookup, ref state),
				GlobalParticle = __query_150133181_6.GetSingletonEntity()
			}, __TypeHandle.__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell3110LifeLineSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell3110LifeLineSystemJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3110LifeLineSpawnBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3110LifeLineDestoryBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3110LifeLineDestoryBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_150133181_6 = entityQueryBuilder2.Build(ref state);
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
		((Spell3110LifeLineSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell3110LifeLineSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell3110LifeLineSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell3110LifeLineSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
