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
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
internal struct Spell1011DisintegrationRaySystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_681430067_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell1011DisintegrationRayData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1011DisintegrationRayData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1011DisintegrationRayData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1011DisintegrationRayData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_681430067_0.TypeHandle __IFE_681430067_0_TypeHandle;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<SpellKeepCastingAttach> __SpellKeepCastingAttach_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell1011DisintegrationRayJob.InternalCompilerQueryAndHandleData __Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_681430067_0_TypeHandle = new IFE_681430067_0.TypeHandle(ref state);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__SpellRefractionData_RO_ComponentLookup = state.GetComponentLookup<SpellRefractionData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellKeepCastingAttach_RW_ComponentLookup = state.GetComponentLookup<SpellKeepCastingAttach>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006546_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006546_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006546_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00006547_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006547_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006547_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_681430067_0;

	private EntityQuery __query_681430067_1;

	private EntityQuery __query_681430067_2;

	private EntityQuery __query_681430067_3;

	private EntityQuery __query_681430067_4;

	private EntityQuery __query_681430067_5;

	private EntityQuery __query_681430067_6;

	private EntityQuery __query_681430067_7;

	private EntityQuery __query_681430067_8;

	private EntityQuery __query_681430067_9;

	private EntityQuery __query_681430067_10;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Spell1011RayTrailBED>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Spell1011DisintegrationRayData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_681430067_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		if (Time.timeScale == 0f)
		{
			return;
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> item3 in IFE_681430067_0.Query(__query_681430067_0, __TypeHandle.__IFE_681430067_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			InternalCompilerInterface.UncheckedRefRO<Spell1011DisintegrationRayData> uncheckedRefRO = item;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO2 = item2;
			Entity spell = entity;
			if (uncheckedRefRO.ValueRO.IsInHover)
			{
				EntityManager em = state.EntityManager;
				SpellTools.StopKeepCasting(in em, in uncheckedRefRO2.ValueRO.Shooter, in spell);
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1011DisintegrationRayJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			PhysicsWorld = __query_681430067_2.GetSingleton<PhysicsWorldSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellRefractionHitEntitiesLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			SpellRefractionLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RO_ComponentLookup, ref state),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			KeepCastingAttachLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellKeepCastingAttach_RW_ComponentLookup, ref state),
			PlayerCtrller = __query_681430067_3.GetSingleton<PlayerController_Dots>(),
			DeltaTime = Time.deltaTime,
			Random = __query_681430067_4.GetSingleton<GlobalRandom>(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			VfxEntity = __query_681430067_5.GetSingletonEntity(),
			CurrentRoomEntities = __query_681430067_6.GetSingleton<CurrentRoomEntitiesSingleton>(),
			SEPlayerSingleton = __query_681430067_7.GetSingletonEntity(),
			ScreenShakeEntity = __query_681430067_8.GetSingletonEntity(),
			GlobalParticleSystemBufferEntity = __query_681430067_9.GetSingletonEntity(),
			OptimizeData = __query_681430067_10.GetSingleton<DynamicOptimizeData>()
		}, __TypeHandle.__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1011DisintegrationRayJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1011DisintegrationRayJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellKeepCastingAttach>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1011DisintegrationRayData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		__query_681430067_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1011RayTrailBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_681430067_10 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006546_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006547_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1011DisintegrationRaySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1011DisintegrationRaySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1011DisintegrationRaySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
