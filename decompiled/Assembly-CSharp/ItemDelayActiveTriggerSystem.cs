using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Transforms;

[CompilerGenerated]
[BurstCompile]
internal struct ItemDelayActiveTriggerSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_497967444_0
	{
		public struct ResolvedChunk
		{
			public EnabledMask item1_EnabledMask;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<EnabledRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<EnabledRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(item1_EnabledMask.GetEnabledRefRW<ItemDelayActiveTriggerData>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<ItemDelayActiveTriggerData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<ItemDelayActiveTriggerData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<ItemDelayActiveTriggerData> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<ItemDelayActiveTriggerData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<ItemDelayActiveTriggerData>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_EnabledMask = archetypeChunk.GetEnabledMask(ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<EnabledRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<EnabledRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<ItemDelayActiveTriggerData>();
			state.EntityManager.CompleteDependencyBeforeRW<ItemDelayActiveTriggerData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_497967444_0.TypeHandle __IFE_497967444_0_TypeHandle;

		public ComponentLookup<ItemDelayActiveTriggerData> __ItemDelayActiveTriggerData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_497967444_0_TypeHandle = new IFE_497967444_0.TypeHandle(ref state);
			__ItemDelayActiveTriggerData_RW_ComponentLookup = state.GetComponentLookup<ItemDelayActiveTriggerData>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005305_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005305_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005305_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_497967444_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<ItemDelayActiveTriggerData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
		foreach (QueryEnumerableWithEntity<EnabledRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item4 in IFE_497967444_0.Query(__query_497967444_0, __TypeHandle.__IFE_497967444_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			EnabledRefRW<ItemDelayActiveTriggerData> enabledRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<ItemDelayActiveTriggerData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			Entity entity2 = entity;
			if (!enabledRefRW.ValueRW)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.DelayTimer -= deltaTime;
			if (uncheckedRefRW.ValueRW.DelayTimer <= 0f)
			{
				if (uncheckedRefRW.ValueRW.isCoin)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__ItemDelayActiveTriggerData_RW_ComponentLookup, ref state, entity2, value: false);
				}
				PhysicsCollider pc = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state, entity2);
				DTool.SetCollider(in pc, 262144u);
				if (uncheckedRefRW.ValueRW.isCoin)
				{
					SEMgr.Inst.itemDropCoin.PlaySE(uncheckedRefRO.ValueRO.Position);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<ItemDelayActiveTriggerData>();
		__query_497967444_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005305_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((ItemDelayActiveTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((ItemDelayActiveTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((ItemDelayActiveTriggerSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
