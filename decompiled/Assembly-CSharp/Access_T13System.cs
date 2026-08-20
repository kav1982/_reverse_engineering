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
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

[CompilerGenerated]
[BurstCompile]
[UpdateInGroup(typeof(SceneGroup))]
public struct Access_T13System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1624251154_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots>, InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots>, InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Access_T13_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<AccessBase_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Access_T13_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<AccessBase_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Access_T13_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<AccessBase_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots>, InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots>, InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Access_T13_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<AccessBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1624251154_0.TypeHandle __IFE_1624251154_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<AccessTrigger> __AccessTrigger_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1624251154_0_TypeHandle = new IFE_1624251154_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AccessTrigger_RW_ComponentLookup = state.GetComponentLookup<AccessTrigger>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005704_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005704_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005704_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1624251154_0;

	private EntityQuery __query_1624251154_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Access_T13_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_1624251154_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots>, InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> item5 in IFE_1624251154_0.Query(__query_1624251154_0, __TypeHandle.__IFE_1624251154_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Access_T13_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<AccessBase_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW3 = item3;
			LocalTransform localTransform = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW3.ValueRW.MakeUnique(in entity2, ecb);
				Entity entity3;
				if (uncheckedRefRW2.ValueRW.Dir == FourDir.Left || uncheckedRefRW2.ValueRW.Dir == FourDir.Right)
				{
					entity3 = state.EntityManager.Instantiate(uncheckedRefRW2.ValueRW.ett_AccessTriggerLR);
					if (uncheckedRefRW2.ValueRW.Dir == FourDir.Left)
					{
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW.Position = localTransform.Position + new float3(-1f, 0f, 0f);
					}
					else
					{
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW.Position = localTransform.Position + new float3(1f, 0f, 0f);
					}
				}
				else
				{
					entity3 = state.EntityManager.Instantiate(uncheckedRefRW2.ValueRW.ett_AccessTriggerUD);
					if (uncheckedRefRW2.ValueRW.Dir == FourDir.Up)
					{
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW.Position = localTransform.Position + new float3(0f, 1f, 0f);
					}
					else
					{
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW.Position = localTransform.Position + new float3(0f, -1f, 0f);
					}
				}
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AccessTrigger_RW_ComponentLookup, ref state, entity3).ValueRW.Dir = uncheckedRefRW2.ValueRW.Dir;
			}
			if (uncheckedRefRW2.ValueRW.onOpen || uncheckedRefRW2.ValueRW.onOpenDirect)
			{
				uncheckedRefRW2.ValueRW.onOpen = false;
				uncheckedRefRW2.ValueRW.onOpenDirect = false;
				if (uncheckedRefRW2.ValueRW.Dir == FourDir.Down)
				{
					break;
				}
				DTool.SetCollider(in uncheckedRefRW3.ValueRO, 0u);
			}
			if (uncheckedRefRW2.ValueRW.onClose || uncheckedRefRW2.ValueRW.onCloseDirect)
			{
				uncheckedRefRW2.ValueRW.onClose = false;
				uncheckedRefRW2.ValueRW.onCloseDirect = false;
				DTool.SetCollider(in uncheckedRefRW3.ValueRO, 256u);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Access_T13_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<AccessBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		__query_1624251154_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1624251154_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005704_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Access_T13System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Access_T13System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Access_T13System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
