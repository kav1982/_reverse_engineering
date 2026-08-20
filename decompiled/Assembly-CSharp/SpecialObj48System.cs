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
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
internal struct SpecialObj48System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_730954991_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<SpecialObj48_Dots>, IRoomCtrller_Dots, LocalTransform) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj48_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<IRoomCtrller_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj48_Dots> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<IRoomCtrller_Dots> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj48_Dots>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<IRoomCtrller_Dots>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<SpecialObj48_Dots>, IRoomCtrller_Dots, LocalTransform)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<SpecialObj48_Dots>, IRoomCtrller_Dots, LocalTransform) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj48_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<IRoomCtrller_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_730954991_0.TypeHandle __IFE_730954991_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_730954991_0_TypeHandle = new IFE_730954991_0.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005FED_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005FED_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005FED_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_730954991_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpecialObj48_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		foreach (var item3 in IFE_730954991_0.Query(__query_730954991_0, __TypeHandle.__IFE_730954991_0_TypeHandle, ref state))
		{
			InternalCompilerInterface.UncheckedRefRW<SpecialObj48_Dots> item = item3.Item1;
			IRoomCtrller_Dots item2 = item3.Item2;
			ref SpecialObj48_Dots valueRW = ref item.ValueRW;
			if (!valueRW.isInitialized)
			{
				valueRW.isInitialized = true;
				Dictionary<Vector2Int, RoomController> roomCtrllers = LevelMgr.Inst.RoomCtrllers;
				UnityObjectRef<RoomController> belongRoom = item2.belongRoom;
				if (roomCtrllers.ContainsKey(belongRoom.Value.MapPos + new Vector2Int(-1, 0)))
				{
					entityCommandBuffer.DestroyEntity(valueRW.ett_LeftCollider);
					EntityManager entityManager = state.EntityManager;
					Entity ett_AccessTriggerLR = valueRW.ett_AccessTriggerLR;
					belongRoom = item2.belongRoom;
					Vector3 v = belongRoom.Value.transform.position;
					CreateTrigger(entityManager, ett_AccessTriggerLR, v.GetFloat3() + valueRW.accessTriggerPosL, FourDir.Left);
				}
				else
				{
					entityCommandBuffer.DestroyEntity(valueRW.ett_LeftAccess);
				}
				Dictionary<Vector2Int, RoomController> roomCtrllers2 = LevelMgr.Inst.RoomCtrllers;
				belongRoom = item2.belongRoom;
				if (roomCtrllers2.ContainsKey(belongRoom.Value.MapPos + new Vector2Int(1, 0)))
				{
					entityCommandBuffer.DestroyEntity(valueRW.ett_RightCollider);
					EntityManager entityManager2 = state.EntityManager;
					Entity ett_AccessTriggerLR2 = valueRW.ett_AccessTriggerLR;
					belongRoom = item2.belongRoom;
					Vector3 v = belongRoom.Value.transform.position;
					CreateTrigger(entityManager2, ett_AccessTriggerLR2, v.GetFloat3() + valueRW.accessTriggerPosR, FourDir.Right);
				}
				else
				{
					entityCommandBuffer.DestroyEntity(valueRW.ett_RightAccess);
				}
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	public void CreateTrigger(EntityManager ettMgr, Entity tirggerPrefab, float3 pos, FourDir dir)
	{
		Entity entity = ettMgr.Instantiate(tirggerPrefab);
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
		componentData.Position = pos;
		ettMgr.SetComponentData(entity, componentData);
		AccessTrigger componentData2 = ettMgr.GetComponentData<AccessTrigger>(entity);
		componentData2.Dir = dir;
		ettMgr.SetComponentData(entity, componentData2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<IRoomCtrller_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj48_Dots>();
		__query_730954991_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005FED_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpecialObj48System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpecialObj48System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj48System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
