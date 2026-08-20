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
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SceneEnterDoorClearPoolGroup))]
[BurstCompile]
[CompilerGenerated]
public struct Door_T11System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2131243574_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Door_T11_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<DoorBase_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Door_T11_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<DoorBase_Dots> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<InteractiveObj_Dots> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Door_T11_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<DoorBase_Dots>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Door_T11_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<DoorBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2131243574_0.TypeHandle __IFE_2131243574_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2131243574_0_TypeHandle = new IFE_2131243574_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005A27_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005A27_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005A27_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_2131243574_0;

	private EntityQuery __query_2131243574_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Door_T11_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		bool flag = false;
		DoorBase_Dots doorBase = default(DoorBase_Dots);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> item6 in IFE_2131243574_0.Query(__query_2131243574_0, __TypeHandle.__IFE_2131243574_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Door_T11_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<DoorBase_Dots> uncheckedRefRW3 = item3;
			LocalTransform localTransform = item4;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW4 = item5;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW3.ValueRW.onUpdateDisplay = true;
				EntityCommandBuffer ecb = __query_2131243574_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
				uncheckedRefRW2.ValueRW.MakeUnique(in entity2, ecb);
				uncheckedRefRW.ValueRW.doorSpineMono.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Door_T11Mono").GetComponent<Door_SpineMono>();
				uncheckedRefRW.ValueRW.doorSpineMono.Value.transform.position = localTransform.Position + DTool.GetLayerPosition(in localTransform.Position, LayerCorrectType.Coordinate);
			}
			if (uncheckedRefRW4.ValueRW.onSelect)
			{
				uncheckedRefRW4.ValueRW.onSelect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW4.ValueRW.ett_Outline).ValueRW.Scale = 1f;
			}
			if (uncheckedRefRW4.ValueRW.onDeselect)
			{
				uncheckedRefRW4.ValueRW.onDeselect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW4.ValueRW.ett_Outline).ValueRW.Scale = 0f;
			}
			if (uncheckedRefRW4.ValueRW.onInteract)
			{
				uncheckedRefRW4.ValueRW.onInteract = false;
				flag = true;
				doorBase = uncheckedRefRW3.ValueRW;
			}
			if (uncheckedRefRW3.ValueRW.onRefreshType)
			{
				uncheckedRefRW3.ValueRW.onRefreshType = false;
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_WhiteSmoke", localTransform.Position + uncheckedRefRW3.ValueRW.refreshEFOffset, 2f);
			}
			if (uncheckedRefRW3.ValueRW.onUpdateDisplay)
			{
				uncheckedRefRW3.ValueRW.onUpdateDisplay = false;
				if (uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward != Entity.Null)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward).ValueRW.Scale = 0f;
				}
				if (PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor)
				{
					uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward200;
				}
				else
				{
					switch (uncheckedRefRW3.ValueRW.rewardType)
					{
					case LevelRewardType.Wand:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward0;
						break;
					case LevelRewardType.Spell:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward1;
						break;
					case LevelRewardType.Relic:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward2;
						break;
					case LevelRewardType.MaxHP:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward3;
						break;
					case LevelRewardType.Coin:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward4;
						break;
					case LevelRewardType.Store:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward5;
						break;
					case LevelRewardType.Process:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward6;
						break;
					case LevelRewardType.Spring:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward7;
						break;
					case LevelRewardType.Elite:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward100;
						break;
					case LevelRewardType.Boss:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward101;
						break;
					case LevelRewardType.Shortcut:
						uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward = uncheckedRefRW3.ValueRW.ett_Reward131;
						break;
					default:
						Debug.LogError(uncheckedRefRW3.ValueRW.rewardType);
						break;
					case LevelRewardType.Chapter:
					case LevelRewardType.None:
					case LevelRewardType.Ruined:
						break;
					}
					if (uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward != Entity.Null)
					{
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW3.ValueRW.ett_CurrentDisplayReward).ValueRW.Scale = 1f;
					}
				}
			}
			if (uncheckedRefRW3.ValueRW.onOpen)
			{
				uncheckedRefRW3.ValueRW.onOpen = false;
				if (LevelMgr.Inst.CurrentRoomCtrller.AllLevelRewardPicked && uncheckedRefRW3.ValueRW.rewardType != LevelRewardType.Ruined)
				{
					uncheckedRefRW.ValueRW.doorSpineMono.Value.Open();
					uncheckedRefRW3.ValueRW.isOpening = true;
					SEMgr.Inst.openDoor_T0.PlaySE(localTransform.Position);
				}
			}
			if (uncheckedRefRW3.ValueRW.onOpenDirect)
			{
				uncheckedRefRW3.ValueRW.onOpenDirect = false;
				if (LevelMgr.Inst.CurrentRoomCtrller.AllLevelRewardPicked && uncheckedRefRW3.ValueRW.rewardType != LevelRewardType.Ruined)
				{
					uncheckedRefRW.ValueRW.doorSpineMono.Value.OpenDirect();
					DTool.SetCollider(in uncheckedRefRW2.ValueRO, 33554432u);
				}
			}
			if (uncheckedRefRW3.ValueRW.isOpening)
			{
				uncheckedRefRW.ValueRW.openDoorTimer += state.WorldUnmanaged.Time.DeltaTime;
				if (uncheckedRefRW.ValueRW.openDoorTimer >= uncheckedRefRW.ValueRW.openDoorTime)
				{
					uncheckedRefRW3.ValueRW.isOpening = false;
					DTool.SetCollider(in uncheckedRefRW2.ValueRO, 33554432u);
				}
			}
		}
		if (flag)
		{
			BattleMgr.Inst.PlayerEnterDoor(doorBase);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Door_T11_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<DoorBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		__query_2131243574_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2131243574_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005A27_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Door_T11System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Door_T11System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Door_T11System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
