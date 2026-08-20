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
using Unity.Physics.Extensions;
using Unity.Physics.Stateful;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[CompilerGenerated]
public struct AccessTriggerSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1469499426_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<StatefulTriggerEvent> item2_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessTrigger>, DynamicBuffer<StatefulTriggerEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessTrigger>, DynamicBuffer<StatefulTriggerEvent>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<AccessTrigger>(item1_IntPtr, index), item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<AccessTrigger> item1_ComponentTypeHandle_RW;

			private BufferTypeHandle<StatefulTriggerEvent> item2_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<AccessTrigger>();
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessTrigger>, DynamicBuffer<StatefulTriggerEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessTrigger>, DynamicBuffer<StatefulTriggerEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<AccessTrigger>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1469499426_0.TypeHandle __IFE_1469499426_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1469499426_0_TypeHandle = new IFE_1469499426_0.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000056BB_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000056BB_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000056BB_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	public NativeList<Entity> entitiesWaitMakeUnique;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1469499426_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<AccessTrigger>();
		entitiesWaitMakeUnique = new NativeList<Entity>(Allocator.Persistent);
	}

	public void OnDestroy(ref SystemState state)
	{
		if (entitiesWaitMakeUnique.IsCreated)
		{
			entitiesWaitMakeUnique.Dispose();
		}
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		bool flag = false;
		FourDir fourDir = (FourDir)0;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessTrigger>, DynamicBuffer<StatefulTriggerEvent>> item3 in IFE_1469499426_0.Query(__query_1469499426_0, __TypeHandle.__IFE_1469499426_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			InternalCompilerInterface.UncheckedRefRW<AccessTrigger> uncheckedRefRW = item;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item2;
			Entity value = entity;
			if (!uncheckedRefRW.ValueRW.initialized)
			{
				uncheckedRefRW.ValueRW.initialized = true;
				entitiesWaitMakeUnique.Add(in value);
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (dynamicBuffer[i].State == StatefulEventState.Enter && dynamicBuffer[i].GetOtherEntity(value) == PlayerMgr.Inst.PlayerEtt)
				{
					flag = true;
					fourDir = uncheckedRefRW.ValueRW.Dir;
				}
			}
		}
		foreach (Entity item4 in entitiesWaitMakeUnique)
		{
			Entity entity2 = item4;
			PhysicsCollider collider = state.EntityManager.GetComponentData<PhysicsCollider>(entity2);
			collider.MakeUnique(in entity2, state.EntityManager);
			collider.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
			state.EntityManager.SetComponentData(entity2, collider);
		}
		entitiesWaitMakeUnique.Clear();
		if (!flag)
		{
			return;
		}
		Vector2Int key = Vector2Int.zero;
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
		{
			if (roomCtrller.Value == LevelMgr.Inst.CurrentRoomCtrller)
			{
				key = roomCtrller.Key;
				break;
			}
		}
		switch (fourDir)
		{
		case FourDir.Left:
			key += new Vector2Int(-1, 0);
			break;
		case FourDir.Right:
			key += new Vector2Int(1, 0);
			break;
		case FourDir.Up:
			key += new Vector2Int(0, 1);
			break;
		default:
			key += new Vector2Int(0, -1);
			break;
		}
		if (LevelMgr.Inst.RoomCtrllers.ContainsKey(key) && LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
		{
			LevelMgr.Inst.PlayerEnterAccess(fourDir);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<AccessTrigger>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		__query_1469499426_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000056BB_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((AccessTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((AccessTriggerSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((AccessTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((AccessTriggerSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
