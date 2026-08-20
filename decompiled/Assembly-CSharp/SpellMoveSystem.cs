using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;

[UpdateInGroup(typeof(SpellPhysicsSystemGroup), OrderLast = true)]
[BurstCompile]
[CompilerGenerated]
public struct SpellMoveSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellParabolaComponentData> __SpellParabolaComponentData_RO_ComponentLookup;

		[ReadOnly]
		public BufferLookup<StatefulCollisionEvent> __Unity_Physics_Stateful_StatefulCollisionEvent_RO_BufferLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1003ButterflyData> __Spell1003ButterflyData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1006GhostFireData> __Spell1006GhostFireData_RO_ComponentLookup;

		public ComponentLookup<Spell1023JudgementBladeData> __Spell1023JudgementBladeData_RW_ComponentLookup;

		public ComponentLookup<Spell1022BoomerangData> __Spell1022BoomerangData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1021MagicBreakerData> __Spell1021MagicBreakerData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<TeammateData> __TeammateData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<SpellMoveTriggerComponentData> __SpellMoveTriggerComponentData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4019BiAnBladeData> __Spell4019BiAnBladeData_RO_ComponentLookup;

		public ComponentLookup<Spell4027BlueRuneData> __Spell4027BlueRuneData_RW_ComponentLookup;

		public SpellMoveJob.InternalCompilerQueryAndHandleData __SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellParabolaComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellParabolaComponentData>(isReadOnly: true);
			__Unity_Physics_Stateful_StatefulCollisionEvent_RO_BufferLookup = state.GetBufferLookup<StatefulCollisionEvent>(isReadOnly: true);
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__Spell1003ButterflyData_RO_ComponentLookup = state.GetComponentLookup<Spell1003ButterflyData>(isReadOnly: true);
			__Spell1006GhostFireData_RO_ComponentLookup = state.GetComponentLookup<Spell1006GhostFireData>(isReadOnly: true);
			__Spell1023JudgementBladeData_RW_ComponentLookup = state.GetComponentLookup<Spell1023JudgementBladeData>();
			__Spell1022BoomerangData_RW_ComponentLookup = state.GetComponentLookup<Spell1022BoomerangData>();
			__Spell1021MagicBreakerData_RO_ComponentLookup = state.GetComponentLookup<Spell1021MagicBreakerData>(isReadOnly: true);
			__TeammateData_RO_ComponentLookup = state.GetComponentLookup<TeammateData>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__SpellMoveTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMoveTriggerComponentData>();
			__Spell4019BiAnBladeData_RO_ComponentLookup = state.GetComponentLookup<Spell4019BiAnBladeData>(isReadOnly: true);
			__Spell4027BlueRuneData_RW_ComponentLookup = state.GetComponentLookup<Spell4027BlueRuneData>();
			__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00008425_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00008425_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00008425_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00008426_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00008426_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00008426_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1321656676_0;

	private EntityQuery __query_1321656676_1;

	private EntityQuery __query_1321656676_2;

	private EntityQuery __query_1321656676_3;

	private EntityQuery __query_1321656676_4;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		SpellSingleton singleton = __query_1321656676_0.GetSingleton<SpellSingleton>();
		PlayerController_Dots singleton2 = __query_1321656676_1.GetSingleton<PlayerController_Dots>();
		Entity singletonEntity = __query_1321656676_1.GetSingletonEntity();
		UnitProperty_Dots componentData = state.EntityManager.GetComponentData<UnitProperty_Dots>(singletonEntity);
		EntityCommandBuffer entityCommandBuffer = __query_1321656676_2.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new SpellMoveJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state),
			MousePosition = singleton2.mousePosition,
			PlayerMoveSpeed = componentData.unitCfg.moveSpeed,
			SpellReboundEffectPrefab = singleton.Prefabs["Spell_Rebound"],
			ParabolaLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellParabolaComponentData_RO_ComponentLookup, ref state),
			CollisionEventsLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Unity_Physics_Stateful_StatefulCollisionEvent_RO_BufferLookup, ref state),
			PhysicsColliderLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state),
			ButterFlyDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1003ButterflyData_RO_ComponentLookup, ref state),
			GhostFireDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1006GhostFireData_RO_ComponentLookup, ref state),
			JudgementBladeDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1023JudgementBladeData_RW_ComponentLookup, ref state),
			BoomerangDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1022BoomerangData_RW_ComponentLookup, ref state),
			MagicBreakerDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1021MagicBreakerData_RO_ComponentLookup, ref state),
			CurrentRoomEntities = __query_1321656676_3.GetSingleton<CurrentRoomEntitiesSingleton>(),
			TeammateDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state),
			UnitPropertyLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state),
			MoveTriggerLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellMoveTriggerComponentData_RW_ComponentLookup, ref state),
			BiAnBladeDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4019BiAnBladeData_RO_ComponentLookup, ref state),
			BlueRuneDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4027BlueRuneData_RW_ComponentLookup, ref state),
			Random = __query_1321656676_4.GetSingleton<GlobalRandom>()
		}, __TypeHandle.__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(SpellMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellMoveJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1321656676_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1321656676_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1321656676_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1321656676_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1321656676_4 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00008425_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00008426_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellMoveSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpellMoveSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpellMoveSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
