using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rukhanka;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
internal class SpecialObj4System : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_702191842_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj4_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<IRoomCtrller_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj4Chapter3Reposition>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj4_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<IRoomCtrller_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpecialObj4Chapter3Reposition> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item6_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj4_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<IRoomCtrller_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj4Chapter3Reposition>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj4_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<IRoomCtrller_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj4Chapter3Reposition>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_702191842_0.TypeHandle __IFE_702191842_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<CreateNavMeshObstacle> __CreateNavMeshObstacle_RW_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		public BufferLookup<AnimationEventComponent> __Rukhanka_AnimationEventComponent_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<SpecialObj4_Dots> __SpecialObj4_Dots_RO_ComponentLookup;

		public ComponentLookup<SpecialObj4_Dots> __SpecialObj4_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpecialObj4NoLock> __SpecialObj4NoLock_RO_ComponentLookup;

		public ComponentLookup<SpecialObj4NoLock> __SpecialObj4NoLock_RW_ComponentLookup;

		public ComponentLookup<IRoomCtrller_Dots> __IRoomCtrller_Dots_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_702191842_0_TypeHandle = new IFE_702191842_0.TypeHandle(ref state);
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__CreateNavMeshObstacle_RW_ComponentLookup = state.GetComponentLookup<CreateNavMeshObstacle>();
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__Rukhanka_AnimationEventComponent_RW_BufferLookup = state.GetBufferLookup<AnimationEventComponent>();
			__SpecialObj4_Dots_RO_ComponentLookup = state.GetComponentLookup<SpecialObj4_Dots>(isReadOnly: true);
			__SpecialObj4_Dots_RW_ComponentLookup = state.GetComponentLookup<SpecialObj4_Dots>();
			__SpecialObj4NoLock_RO_ComponentLookup = state.GetComponentLookup<SpecialObj4NoLock>(isReadOnly: true);
			__SpecialObj4NoLock_RW_ComponentLookup = state.GetComponentLookup<SpecialObj4NoLock>();
			__IRoomCtrller_Dots_RW_ComponentLookup = state.GetComponentLookup<IRoomCtrller_Dots>();
		}
	}

	private List<int> createId = new List<int>();

	private List<Vector3> createPosition = new List<Vector3>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_702191842_0;

	private EntityQuery __query_702191842_1;

	private EntityQuery __query_702191842_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<PlayerController_Dots>();
		RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<SpecialObj4_Dots>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer ecb = __query_702191842_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.EntityManager.World.Unmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item8 in IFE_702191842_0.Query(__query_702191842_0, __TypeHandle.__IFE_702191842_0_TypeHandle, ref base.CheckedStateRef))
		{
			item8.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj4_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpecialObj4Chapter3Reposition> uncheckedRefRW5 = item5;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW6 = item6;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW4.ValueRW.MakeUnique(in entity2, ecb);
				if (!uncheckedRefRW.ValueRW.onFly)
				{
					DTool.SetCollider(in uncheckedRefRW4.ValueRW, 33554432u);
				}
				if (uncheckedRefRW.ValueRW.chestType == ChestType.Curse)
				{
					if (GameMgr.IsHarmony_Static)
					{
						uncheckedRefRW.ValueRW.go_EF.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO4EFCurseH");
					}
					else
					{
						uncheckedRefRW.ValueRW.go_EF.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO4EFCurse");
					}
				}
				else
				{
					uncheckedRefRW.ValueRW.go_EF.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO4EF");
				}
			}
			if (uncheckedRefRW.ValueRW.go_EF.Value != null && uncheckedRefRW.ValueRW.go_EF.Value.activeSelf)
			{
				uncheckedRefRW.ValueRW.go_EF.Value.transform.position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Motion).Position;
			}
			if (uncheckedRefRW3.ValueRW.onSelect)
			{
				uncheckedRefRW3.ValueRW.onSelect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW3.ValueRW.ett_Outline).ValueRW.Scale = 1f;
			}
			if (uncheckedRefRW3.ValueRW.onDeselect)
			{
				uncheckedRefRW3.ValueRW.onDeselect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW3.ValueRW.ett_Outline).ValueRW.Scale = 0f;
			}
			if (uncheckedRefRW3.ValueRW.onInteract)
			{
				uncheckedRefRW3.ValueRW.onInteract = false;
				if (!uncheckedRefRW.ValueRW.alreadyOpen)
				{
					RefRW<AnimaPlay> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima);
					switch (uncheckedRefRW.ValueRW.chestType)
					{
					case ChestType.Spike:
						DTool.SetCollider(in uncheckedRefRW4.ValueRW, 0u);
						componentRWAfterCompletingDependency.ValueRW.Play(0);
						uncheckedRefRW.ValueRW.alreadyOpen = true;
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__CreateNavMeshObstacle_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRW.chestDisable = true;
						if (!uncheckedRefRW.ValueRW.isOpenByPotion)
						{
							Entity singletonEntity = __query_702191842_2.GetSingletonEntity();
							DynamicBuffer<TakeDamageInfo_Dots> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef, singletonEntity);
							TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
							elem.damage = 10f;
							elem.ignorePlayerInvincibleFrame = true;
							elem.ignoreRelicDodge = true;
							elem.ignoreRelicOrCurseDamageRatioChange = true;
							elem.ignoreUmbrella = true;
							bufferAfterCompletingDependency.Add(elem);
						}
						break;
					case ChestType.Curse:
						componentRWAfterCompletingDependency.ValueRW.Play(0);
						DTool.SetCollider(in uncheckedRefRW4.ValueRW, 0u);
						uncheckedRefRW.ValueRW.alreadyOpen = true;
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__CreateNavMeshObstacle_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRW.chestDisable = true;
						LevelMgr.Inst.RoomFinishLogger.cursed_chest.open_curse.Add(uncheckedRefRW.ValueRW.curseID);
						break;
					case ChestType.Lock:
						if (PlayerMgr.Inst.IsKeyEnough() || uncheckedRefRW.ValueRW.isOpenByPotion)
						{
							DTool.SetCollider(in uncheckedRefRW4.ValueRW, 0u);
							componentRWAfterCompletingDependency.ValueRW.Play(0);
							uncheckedRefRW.ValueRW.alreadyOpen = true;
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__CreateNavMeshObstacle_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRW.chestDisable = true;
							if (!uncheckedRefRW.ValueRW.isOpenByPotion)
							{
								PlayerMgr.Inst.ChangeKey(-PlayerMgr.Inst.NeedKeyCount(), TextFloatQueueType.DirectFloat);
							}
							LevelMgr.Inst.RoomFinishLogger.locked_chest.cost_keys += PlayerMgr.Inst.NeedKeyCount();
							LevelMgr.Inst.RoomFinishLogger.locked_chest.open_count++;
						}
						break;
					default:
						Debug.LogError(uncheckedRefRW.ValueRW.chestType);
						break;
					}
				}
			}
			if (uncheckedRefRW.ValueRW.onFly)
			{
				uncheckedRefRW.ValueRW.onFly = false;
				uncheckedRefRW.ValueRW.isFlying = true;
				uncheckedRefRW.ValueRW.flySpeed = Tool2D.IgnoreZDistance(uncheckedRefRW6.ValueRW.Position, uncheckedRefRW.ValueRW.flyPosition) / uncheckedRefRW.ValueRW.flyTime;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(1);
			}
			if (uncheckedRefRW.ValueRW.isFlying)
			{
				uncheckedRefRW6.ValueRW.Position = Vector3.MoveTowards((Vector3)uncheckedRefRW6.ValueRW.Position, (Vector3)uncheckedRefRW.ValueRW.flyPosition, uncheckedRefRW.ValueRW.flySpeed * base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
				if (DTool.IsEqual(in uncheckedRefRW6.ValueRW.Position, in uncheckedRefRW.ValueRW.flyPosition))
				{
					uncheckedRefRW6.ValueRW.Position = uncheckedRefRW.ValueRW.flyPosition;
					uncheckedRefRW.ValueRW.isFlying = false;
					DTool.SetCollider(in uncheckedRefRW4.ValueRW, 33554432u);
					SEMgr.Inst.chestFlyLand.PlaySE();
				}
			}
			if (uncheckedRefRW5.ValueRW.onChapter3Reposition)
			{
				uncheckedRefRW5.ValueRW.onChapter3Reposition = false;
				uncheckedRefRW6.ValueRW.Position += uncheckedRefRW5.ValueRW.repositionValue;
				uncheckedRefRW.ValueRW.flyPosition += uncheckedRefRW5.ValueRW.repositionValue;
			}
			if (uncheckedRefRW2.ValueRW.onRoomEnter)
			{
				uncheckedRefRW2.ValueRW.onRoomEnter = false;
				if (!uncheckedRefRW.ValueRW.alreadyHandleRoomEnter)
				{
					uncheckedRefRW.ValueRW.alreadyHandleRoomEnter = true;
					if (uncheckedRefRW.ValueRW.chestType == ChestType.Curse && uncheckedRefRW.ValueRW.curseID == 0)
					{
						uncheckedRefRW.ValueRW.curseID = PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
						DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Curse, uncheckedRefRW.ValueRW.curseID);
					}
					if (PlayerMgr.Inst.ItemCtrller.curseCfg_ChestMonster != null && UnityEngine.Random.value <= PlayerMgr.Inst.ItemCtrller.curseCfg_ChestMonster.float1.result)
					{
						int item7;
						switch (uncheckedRefRW.ValueRW.chestType)
						{
						case ChestType.NoLock:
							item7 = 199504;
							break;
						case ChestType.Lock:
							item7 = 199501;
							break;
						case ChestType.Spike:
							item7 = 199502;
							break;
						case ChestType.Curse:
							item7 = 199503;
							break;
						default:
							Debug.LogError(uncheckedRefRW.ValueRW.chestType);
							item7 = 199504;
							break;
						}
						ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.go_EF.Value);
						uncheckedRefRW.ValueRW.go_EF.Value = null;
						createId.Add(item7);
						createPosition.Add(uncheckedRefRW6.ValueRW.Position);
						ecb.DestroyEntity(entity2);
						break;
					}
				}
			}
			DynamicBuffer<AnimationEventComponent> bufferAfterCompletingDependency2 = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Rukhanka_AnimationEventComponent_RW_BufferLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima);
			for (int i = 0; i < bufferAfterCompletingDependency2.Length; i++)
			{
				if (bufferAfterCompletingDependency2[i].intParam == 1)
				{
					if (uncheckedRefRW.ValueRW.go_EF.Value == null)
					{
						continue;
					}
					ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.go_EF.Value);
					uncheckedRefRW.ValueRW.go_EF.Value = null;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Close).ValueRW.Scale = 0f;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Open).ValueRW.Scale = 1f;
					SEMgr.Inst.chestOpen.PlaySE();
					QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetSO4Chest(uncheckedRefRW.ValueRW.chestType), uncheckedRefRW6.ValueRW.Position, 1f);
					if (uncheckedRefRW.ValueRW.chestType == ChestType.Curse && !uncheckedRefRW.ValueRW.isOpenByPotion)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemCursePickup", uncheckedRefRW6.ValueRW.Position);
						PlayerMgr.Inst.ItemCtrller.CurseAdd(uncheckedRefRW.ValueRW.curseID, uncheckedRefRW6.ValueRW.Position);
					}
					if (PlayerMgr.Inst.ItemCtrller.relicCfg_EndlessChest != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_EndlessChest.int1.result / 100f)
					{
						ChestType chestType = (ChestType)UnityEngine.Random.Range(0, 4);
						int id = 401;
						switch (chestType)
						{
						case ChestType.NoLock:
							id = 404;
							break;
						case ChestType.Lock:
							id = 401;
							break;
						case ChestType.Spike:
							id = 402;
							break;
						case ChestType.Curse:
							id = 403;
							break;
						default:
							Debug.LogError(chestType);
							break;
						}
						Entity entity3 = QuickCreateSystem.Inst.CreateSpecialObj(id, uncheckedRefRW6.ValueRW.Position);
						Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRW6.ValueRW.Position, 1.5f);
						if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpecialObj4_Dots_RO_ComponentLookup, ref base.CheckedStateRef, entity3))
						{
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpecialObj4_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity3).ValueRW.SetFly(navMeshPointIngoreZ);
						}
						else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpecialObj4NoLock_RO_ComponentLookup, ref base.CheckedStateRef, entity3))
						{
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpecialObj4NoLock_RW_ComponentLookup, ref base.CheckedStateRef, entity3).ValueRW.SetFly(navMeshPointIngoreZ);
						}
						RefRW<IRoomCtrller_Dots> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__IRoomCtrller_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity3);
						componentRWAfterCompletingDependency2.ValueRW.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
						componentRWAfterCompletingDependency2.ValueRW.onRoomEnter = true;
					}
				}
				else
				{
					Debug.LogError(bufferAfterCompletingDependency2[i].intParam);
				}
			}
		}
		if (createId.Count > 0)
		{
			for (int j = 0; j < createId.Count; j++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + createId[j], createPosition[j]);
			}
			createId.Clear();
			createPosition.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<IRoomCtrller_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj4_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj4Chapter3Reposition>();
		__query_702191842_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_702191842_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_702191842_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public SpecialObj4System()
	{
	}
}
