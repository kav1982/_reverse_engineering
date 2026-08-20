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
internal struct BackCampPortalSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_988955851_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<BackCampPortal>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<BackCampPortal>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<BackCampPortal>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<BackCampPortal> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<BackCampPortal>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<BackCampPortal>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<BackCampPortal>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<BackCampPortal>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_988955851_0.TypeHandle __IFE_988955851_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_988955851_0_TypeHandle = new IFE_988955851_0.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000501C_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000501C_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000501C_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_988955851_0;

	private EntityQuery __query_988955851_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<BackCampPortal>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_988955851_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<BackCampPortal>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, LocalTransform> item5 in IFE_988955851_0.Query(__query_988955851_0, __TypeHandle.__IFE_988955851_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<BackCampPortal> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW3 = item3;
			LocalTransform localTransform = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitailized)
			{
				uncheckedRefRW.ValueRW.isInitailized = true;
				uncheckedRefRW3.ValueRW.MakeUnique(in entity2, ecb);
				float3 layerPosition = DTool.GetLayerPosition(in localTransform.Position, LayerCorrectType.Coordinate);
				uncheckedRefRW.ValueRW.portalMono.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/BackCampPortalMono", localTransform.Position + layerPosition).GetComponent<BackCampPortalMono>();
			}
			if (uncheckedRefRW2.ValueRW.onSelect)
			{
				uncheckedRefRW2.ValueRW.onSelect = false;
				uncheckedRefRW.ValueRW.portalMono.Value.Select();
			}
			if (uncheckedRefRW2.ValueRW.onDeselect)
			{
				uncheckedRefRW2.ValueRW.onDeselect = false;
				uncheckedRefRW.ValueRW.portalMono.Value.Unselect();
			}
			if (uncheckedRefRW2.ValueRW.onInteract)
			{
				uncheckedRefRW2.ValueRW.onInteract = false;
				uncheckedRefRW.ValueRW.portalMono.Value.Interact();
				DTool.SetCollider(in uncheckedRefRW3.ValueRW, 256u);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<BackCampPortal>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		__query_988955851_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_988955851_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000501C_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((BackCampPortalSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((BackCampPortalSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((BackCampPortalSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
