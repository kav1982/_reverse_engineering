using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[CompilerGenerated]
internal class SpecialObj4NoLockSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1910472330_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public BufferAccessor<StatefulTriggerEvent> item4_BufferAccessor;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, DynamicBuffer<StatefulTriggerEvent>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, DynamicBuffer<StatefulTriggerEvent>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj4NoLock>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<IRoomCtrller_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), item4_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj4NoLock> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<IRoomCtrller_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			private BufferTypeHandle<StatefulTriggerEvent> item4_BufferTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj4NoLock>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<IRoomCtrller_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item4_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_BufferTypeHandle_RW.Update(ref systemState);
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
				result.item4_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item4_BufferTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, DynamicBuffer<StatefulTriggerEvent>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, DynamicBuffer<StatefulTriggerEvent>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj4NoLock>();
			state.EntityManager.CompleteDependencyBeforeRW<IRoomCtrller_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1910472330_0.TypeHandle __IFE_1910472330_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<CreateNavMeshObstacle> __CreateNavMeshObstacle_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

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
			__IFE_1910472330_0_TypeHandle = new IFE_1910472330_0.TypeHandle(ref state);
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__CreateNavMeshObstacle_RW_ComponentLookup = state.GetComponentLookup<CreateNavMeshObstacle>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpecialObj4_Dots_RO_ComponentLookup = state.GetComponentLookup<SpecialObj4_Dots>(isReadOnly: true);
			__SpecialObj4_Dots_RW_ComponentLookup = state.GetComponentLookup<SpecialObj4_Dots>();
			__SpecialObj4NoLock_RO_ComponentLookup = state.GetComponentLookup<SpecialObj4NoLock>(isReadOnly: true);
			__SpecialObj4NoLock_RW_ComponentLookup = state.GetComponentLookup<SpecialObj4NoLock>();
			__IRoomCtrller_Dots_RW_ComponentLookup = state.GetComponentLookup<IRoomCtrller_Dots>();
		}
	}

	private List<Vector3> createPosition = new List<Vector3>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1910472330_0;

	private EntityQuery __query_1910472330_1;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<SpecialObj4NoLock>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer ecb = __query_1910472330_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.EntityManager.World.Unmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock>, InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, DynamicBuffer<StatefulTriggerEvent>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item6 in IFE_1910472330_0.Query(__query_1910472330_0, __TypeHandle.__IFE_1910472330_0_TypeHandle, ref base.CheckedStateRef))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj4NoLock> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<IRoomCtrller_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW3 = item3;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item4;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW4 = item5;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW3.ValueRW.MakeUnique(in entity2, ecb);
				if (!uncheckedRefRW.ValueRW.onFly)
				{
					DTool.SetCollider(in uncheckedRefRW3.ValueRW, 33554432u);
				}
				uncheckedRefRW.ValueRW.go_EF.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO4EF");
			}
			if (uncheckedRefRW.ValueRW.go_EF.Value != null && uncheckedRefRW.ValueRW.go_EF.Value.activeSelf)
			{
				uncheckedRefRW.ValueRW.go_EF.Value.transform.position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Motion).Position;
			}
			if (uncheckedRefRW.ValueRW.onFly)
			{
				uncheckedRefRW.ValueRW.onFly = false;
				uncheckedRefRW.ValueRW.isFlying = true;
				uncheckedRefRW.ValueRW.flySpeed = Tool2D.IgnoreZDistance(uncheckedRefRW4.ValueRW.Position, uncheckedRefRW.ValueRW.flyPosition) / uncheckedRefRW.ValueRW.flyTime;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(1);
			}
			if (uncheckedRefRW.ValueRW.isFlying)
			{
				uncheckedRefRW4.ValueRW.Position = Vector3.MoveTowards((Vector3)uncheckedRefRW4.ValueRW.Position, (Vector3)uncheckedRefRW.ValueRW.flyPosition, uncheckedRefRW.ValueRW.flySpeed * base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
				if (DTool.IsEqual(in uncheckedRefRW4.ValueRW.Position, in uncheckedRefRW.ValueRW.flyPosition))
				{
					uncheckedRefRW4.ValueRW.Position = uncheckedRefRW.ValueRW.flyPosition;
					uncheckedRefRW.ValueRW.isFlying = false;
					DTool.SetCollider(in uncheckedRefRW3.ValueRW, 33554432u);
					SEMgr.Inst.chestFlyLand.PlaySE();
				}
			}
			if (!uncheckedRefRW.ValueRW.isTriggered)
			{
				for (int i = 0; i < dynamicBuffer.Length; i++)
				{
					if (dynamicBuffer[i].State == StatefulEventState.Enter && dynamicBuffer[i].GetOtherEntity(entity2) == PlayerMgr.Inst.PlayerEtt)
					{
						uncheckedRefRW.ValueRW.isTriggered = true;
						DTool.SetCollider(in uncheckedRefRW3.ValueRW, 0u);
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(0);
						uncheckedRefRW.ValueRW.isAnimaOpening = true;
						uncheckedRefRW.ValueRW.alreadyOpen = true;
						InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__CreateNavMeshObstacle_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRW.chestDisable = true;
						break;
					}
				}
			}
			if (uncheckedRefRW.ValueRW.isOpenByPotion)
			{
				uncheckedRefRW.ValueRW.isOpenByPotion = false;
				if (!uncheckedRefRW.ValueRW.alreadyOpen)
				{
					uncheckedRefRW.ValueRW.isTriggered = true;
					DTool.SetCollider(in uncheckedRefRW3.ValueRW, 0u);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(0);
					uncheckedRefRW.ValueRW.isAnimaOpening = true;
					uncheckedRefRW.ValueRW.alreadyOpen = true;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__CreateNavMeshObstacle_RW_ComponentLookup, ref base.CheckedStateRef, entity2).ValueRW.chestDisable = true;
				}
			}
			if (uncheckedRefRW2.ValueRW.onRoomEnter)
			{
				uncheckedRefRW2.ValueRW.onRoomEnter = false;
				if (!uncheckedRefRW.ValueRW.alreadyHandleRoomEnter)
				{
					uncheckedRefRW.ValueRW.alreadyHandleRoomEnter = true;
					if (PlayerMgr.Inst.ItemCtrller.curseCfg_ChestMonster != null && UnityEngine.Random.value <= PlayerMgr.Inst.ItemCtrller.curseCfg_ChestMonster.float1.result)
					{
						createPosition.Add(uncheckedRefRW4.ValueRW.Position);
						ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.go_EF.Value);
						uncheckedRefRW.ValueRW.go_EF.Value = null;
						ecb.DestroyEntity(entity2);
						break;
					}
				}
			}
			if (!uncheckedRefRW.ValueRW.isAnimaOpening)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.openTriggerTimer += base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
			if (!(uncheckedRefRW.ValueRW.openTriggerTimer >= uncheckedRefRW.ValueRW.openTriggerTime))
			{
				continue;
			}
			uncheckedRefRW.ValueRW.openTriggerTimer = 0f;
			uncheckedRefRW.ValueRW.isAnimaOpening = false;
			ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.go_EF.Value);
			uncheckedRefRW.ValueRW.go_EF.Value = null;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Close).ValueRW.Scale = 0f;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW.ValueRW.ett_Open).ValueRW.Scale = 1f;
			SEMgr.Inst.chestOpen.PlaySE();
			if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.id == 108)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, 30121), uncheckedRefRW4.ValueRO.Position);
			}
			else
			{
				QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetSO4Chest(ChestType.NoLock), uncheckedRefRW4.ValueRW.Position, 1f);
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
				Entity entity3 = QuickCreateSystem.Inst.CreateSpecialObj(id, uncheckedRefRW4.ValueRW.Position);
				Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRW4.ValueRW.Position, 1.5f);
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpecialObj4_Dots_RO_ComponentLookup, ref base.CheckedStateRef, entity3))
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpecialObj4_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity3).ValueRW.SetFly(navMeshPointIngoreZ);
				}
				else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpecialObj4NoLock_RO_ComponentLookup, ref base.CheckedStateRef, entity3))
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpecialObj4NoLock_RW_ComponentLookup, ref base.CheckedStateRef, entity3).ValueRW.SetFly(navMeshPointIngoreZ);
				}
				RefRW<IRoomCtrller_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__IRoomCtrller_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity3);
				componentRWAfterCompletingDependency.ValueRW.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
				componentRWAfterCompletingDependency.ValueRW.onRoomEnter = true;
			}
		}
		if (createPosition.Count > 0)
		{
			for (int j = 0; j < createPosition.Count; j++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 199504, createPosition[j]);
			}
			createPosition.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<IRoomCtrller_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj4NoLock>();
		__query_1910472330_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1910472330_1 = entityQueryBuilder2.Build(ref state);
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
	public SpecialObj4NoLockSystem()
	{
	}
}
