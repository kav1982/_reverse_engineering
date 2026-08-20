using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;

[CompilerGenerated]
internal struct NPCSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1314907921_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<NPC_Dots>, InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<NPC_Dots>, InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<NPC_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<NPCBaseComponent>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<NPC_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<NPCBaseComponent> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item3_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<NPC_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<NPCBaseComponent>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<NPC_Dots>, InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<NPC_Dots>, InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<NPC_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<NPCBaseComponent>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1314907921_0.TypeHandle __IFE_1314907921_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1314907921_0_TypeHandle = new IFE_1314907921_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1314907921_0;

	[BurstCompile]
	public void OnStart(ref SystemState state)
	{
		state.RequireForUpdate<NPCBaseComponent>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<NPC_Dots>, InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> item4 in IFE_1314907921_0.Query(__query_1314907921_0, __TypeHandle.__IFE_1314907921_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var _, out var item2, out var item3, out var _);
			InternalCompilerInterface.UncheckedRefRW<NPCBaseComponent> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item3;
			if (uncheckedRefRW2.ValueRW.onSelect)
			{
				uncheckedRefRW2.ValueRW.onSelect = false;
				uncheckedRefRW.ValueRW.npcBase.Value.Select();
			}
			if (uncheckedRefRW2.ValueRW.onDeselect)
			{
				uncheckedRefRW2.ValueRW.onDeselect = false;
				uncheckedRefRW.ValueRW.npcBase.Value.Unselect();
			}
			if (uncheckedRefRW2.ValueRW.onInteract)
			{
				uncheckedRefRW2.ValueRW.onInteract = false;
				uncheckedRefRW.ValueRW.npcBase.Value.Interact();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<NPC_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<NPCBaseComponent>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		__query_1314907921_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((NPCSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((NPCSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
