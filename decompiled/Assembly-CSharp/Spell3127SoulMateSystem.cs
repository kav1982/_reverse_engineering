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
using Unity.Transforms;

[UpdateInGroup(typeof(SpellEndSystemGroup))]
[BurstCompile]
[CompilerGenerated]
[UpdateBefore(typeof(TeammateDeadEventSystem))]
internal struct Spell3127SoulMateSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_172898909_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell3127SoulMateComponent>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell3127SoulMateComponent>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item4_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<Spell3127SoulMateComponent> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<TeammateData> item4_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell3127SoulMateComponent>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell3127SoulMateComponent>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell3127SoulMateComponent>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<TeammateData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell3127SoulMateComponent>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_172898909_0.TypeHandle __IFE_172898909_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_172898909_0_TypeHandle = new IFE_172898909_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000761E_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000761E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000761E_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private ComponentLookup<LocalTransform> localTransformLookup;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_172898909_0;

	private EntityQuery __query_172898909_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell3127SoulMateComponent>();
		localTransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state);
		state.EntityManager.CreateSingletonBuffer<Spell3127DestoryBuffer>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (!LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
		{
			return;
		}
		localTransformLookup.Update(ref state);
		foreach (var (uncheckedRefRO, uncheckedRefRW, uncheckedRefRO2, uncheckedRefRW2) in IFE_172898909_0.Query(__query_172898909_0, __TypeHandle.__IFE_172898909_0_TypeHandle, ref state))
		{
			if (!localTransformLookup.HasComponent(uncheckedRefRO2.ValueRO.OwnerEntity))
			{
				continue;
			}
			if (!uncheckedRefRW.ValueRO.beDestoried && uncheckedRefRW.ValueRO.RingEffect == Entity.Null)
			{
				Entity entity = state.EntityManager.Instantiate(__query_172898909_1.GetSingleton<SpellSingleton>().Prefabs["3127_SoulMate"]);
				state.EntityManager.SetComponentData(entity, new LocalTransform
				{
					Position = uncheckedRefRO.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = uncheckedRefRO.ValueRO.Scale
				});
				uncheckedRefRW.ValueRW.RingEffect = entity;
				uncheckedRefRW.ValueRW.scaleZoomer = 0f;
			}
			if (uncheckedRefRW.ValueRW.RingEffect != Entity.Null && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.RingEffect))
			{
				RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.RingEffect);
				componentRWAfterCompletingDependency.ValueRW.Position = Tool2D.GetLayerPoint(uncheckedRefRO.ValueRO.Position, LayerCorrectType.GroundEffect);
				RefRW<EffectsCollectorData> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.RingEffect);
				RefRW<LocalTransform> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency2.ValueRW.Effect1);
				float3 @float = math.normalize(InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRO2.ValueRO.OwnerEntity).ValueRO.Position - uncheckedRefRO.ValueRO.Position);
				componentRWAfterCompletingDependency3.ValueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(@float.y, @float.x));
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency2.ValueRW.Effect2).ValueRW.Scale = ((!uncheckedRefRW2.ValueRO.IsHoldByTeammate6) ? 1 : 0);
				if (uncheckedRefRW.ValueRO.scaleZoomer < 1f)
				{
					uncheckedRefRW.ValueRW.scaleZoomer += state.WorldUnmanaged.Time.DeltaTime;
					componentRWAfterCompletingDependency.ValueRW.Scale = uncheckedRefRW.ValueRO.scaleZoomer;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell3127SoulMateComponent>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		__query_172898909_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_172898909_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000761E_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell3127SoulMateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell3127SoulMateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell3127SoulMateSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
