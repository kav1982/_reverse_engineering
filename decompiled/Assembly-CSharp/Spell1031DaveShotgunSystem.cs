using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
[CompilerGenerated]
internal struct Spell1031DaveShotgunSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<HarpoonsHitCounter> __HarpoonsHitCounter_RW_ComponentLookup;

		public ComponentLookup<SpellGroundedTag> __SpellGroundedTag_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell1031DaveShotgunJob.InternalCompilerQueryAndHandleData __Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__HarpoonsHitCounter_RW_ComponentLookup = state.GetComponentLookup<HarpoonsHitCounter>();
			__SpellGroundedTag_RW_ComponentLookup = state.GetComponentLookup<SpellGroundedTag>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_329456112_0;

	private EntityQuery __query_329456112_1;

	private EntityQuery __query_329456112_2;

	private EntityQuery __query_329456112_3;

	private EntityQuery __query_329456112_4;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell1031DaveShotgunData>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer.ParallelWriter ecb = __query_329456112_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		Entity value;
		bool haveHitCounter = __query_329456112_1.TryGetSingletonEntity<HarpoonsHitCounter>(out value);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1031DaveShotgunJob
		{
			Ecb = ecb,
			GlobalParticleBuffer = __query_329456112_2.GetSingletonEntity(),
			ScreenShakeSingleton = __query_329456112_3.GetSingletonEntity(),
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			HitCounterLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__HarpoonsHitCounter_RW_ComponentLookup, ref state),
			HaveHitCounter = haveHitCounter,
			HitCounterEntity = value,
			EntityExists = state.GetEntityStorageInfoLookup(),
			GroundTagLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellGroundedTag_RW_ComponentLookup, ref state),
			PhysicsWorld = __query_329456112_4.GetSingleton<PhysicsWorldSingleton>(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state)
		}, __TypeHandle.__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1031DaveShotgunJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1031DaveShotgunJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_329456112_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<HarpoonsHitCounter>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_329456112_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_329456112_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_329456112_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_329456112_4 = entityQueryBuilder2.Build(ref state);
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
		((Spell1031DaveShotgunSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1031DaveShotgunSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1031DaveShotgunSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
