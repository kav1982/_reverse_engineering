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
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
[CompilerGenerated]
public struct AccessCampSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_51121654_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<StatefulTriggerEvent> item3_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessCamp>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessCamp>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<AccessCamp>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index), item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<AccessCamp> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<StatefulTriggerEvent> item3_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<AccessCamp>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessCamp>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessCamp>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<AccessCamp>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_51121654_0.TypeHandle __IFE_51121654_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Item> __Item_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_51121654_0_TypeHandle = new IFE_51121654_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Item_RO_ComponentLookup = state.GetComponentLookup<Item>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000568A_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000568A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000568A_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_51121654_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<AccessCamp>();
	}

	public void OnUpdate(ref SystemState state)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<AccessCamp>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> item4 in IFE_51121654_0.Query(__query_51121654_0, __TypeHandle.__IFE_51121654_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<AccessCamp> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item2;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item3;
			Entity entity2 = entity;
			if (uncheckedRefRW.ValueRW.needWaitAFrame)
			{
				uncheckedRefRW.ValueRW.needWaitAFrame = false;
				return;
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (dynamicBuffer[i].State != StatefulEventState.Enter)
				{
					continue;
				}
				if (dynamicBuffer[i].GetOtherEntity(entity2) == PlayerMgr.Inst.PlayerEtt)
				{
					if (PlayerMgr.Inst.PlayerCtrller.IsKeepCasting || PlayerMgr.Inst.PlayerCtrller.IsCharging)
					{
						RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity2);
						switch (uncheckedRefRW.ValueRW.dir)
						{
						case FourDir.Up:
							componentRWAfterCompletingDependency.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(0f, -1f, 0f);
							break;
						case FourDir.Right:
							componentRWAfterCompletingDependency.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(-1f, 0f, 0f);
							break;
						case FourDir.Down:
							componentRWAfterCompletingDependency.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(0f, 1f, 0f);
							break;
						case FourDir.Left:
							componentRWAfterCompletingDependency.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(1f, 0f, 0f);
							break;
						default:
							Debug.LogError(uncheckedRefRW.ValueRW.dir);
							break;
						}
						return;
					}
					RoomController currentRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
					switch (uncheckedRefRW.ValueRW.dir)
					{
					case FourDir.Down:
						LevelMgr.Inst.CurrentRoomMapPos += new Vector2Int(0, -1);
						PlayerMgr.Inst.SetPlayerPoint(CampMgr.Inst.endlessEntryPos);
						if (DataMgr.selectedWorldData.endless_LevelOfExtraDamage <= 0)
						{
							CampMgr.Inst.SetEttEnable(CampMgr.Inst.CurrentCampSkin.ett_Gallery, enable: false);
						}
						UIPlayerDataMgr.Inst.UpdateAllInfo();
						DataMgr.SaveSelectedWorldData();
						MusicMgr.Inst.UpdateCampBGM();
						break;
					case FourDir.Up:
						LevelMgr.Inst.CurrentRoomMapPos += new Vector2Int(0, 1);
						PlayerMgr.Inst.SetPlayerPoint(CampMgr.Inst.endlessExitPos);
						UIPlayerDataMgr.Inst.UpdateAllInfo();
						DataMgr.SaveSelectedWorldData();
						MusicMgr.Inst.UpdateCampBGM();
						break;
					case FourDir.Right:
					{
						LevelMgr.Inst.CurrentRoomMapPos += new Vector2Int(1, 0);
						float3 float2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, CampMgr.Inst.CurrentCampSkin.ett_AccessL).Position + new float3(2f, 0f, 0f);
						PlayerMgr.Inst.SetPlayerPoint(float2);
						if (!uncheckedRefRW.ValueRW.createdScarecrow)
						{
							uncheckedRefRW.ValueRW.createdScarecrow = true;
							flag = true;
						}
						break;
					}
					case FourDir.Left:
					{
						foreach (Wand wand2 in PlayerMgr.Inst.Wands)
						{
							wand2.ReleaseCharge();
						}
						PlayerMgr.Inst.PlayerCtrller.NonInteractiveClear();
						LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.Clear();
						LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.Clear();
						LevelMgr.Inst.CurrentRoomCtrller.ClearAllTeammates(LevelMgr.Inst.CurrentRoomCtrller);
						UIPlayerDataMgr.Inst.HideAllInfoPanel();
						for (int num = PlayerMgr.Inst.BaData.relicCfgs.Count - 1; num >= 0; num--)
						{
							PlayerMgr.Inst.ItemCtrller.RelicRemove(PlayerMgr.Inst.BaData.relicCfgs[num].id, 9999);
						}
						PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
						playerPpt.ClearAllRegister();
						state.EntityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
						LevelMgr.Inst.CurrentRoomMapPos += new Vector2Int(-1, 0);
						float3 @float = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, CampMgr.Inst.CurrentCampSkin.ett_AccessR).Position + new float3(-2f, 0f, 0f);
						PlayerMgr.Inst.SetPlayerPoint(@float);
						flag2 = true;
						break;
					}
					default:
						Debug.LogError(uncheckedRefRW.ValueRW.dir);
						break;
					}
					uncheckedRefRW.ValueRW.needWaitAFrame = true;
					currentRoomCtrller.RoomLeave();
					currentRoomCtrller.fogCtrller.Show();
					LevelMgr.Inst.CurrentRoomCtrller.RoomEnter();
					if (uncheckedRefRW.ValueRW.dir == FourDir.Left)
					{
						float3 float3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, CampMgr.Inst.CurrentCampSkin.ett_AccessR).Position + new float3(-1f, 0f, 0f);
						LevelMgr.Inst.CurrentRoomCtrller.fogCtrller.Hide(float3);
					}
					else if (uncheckedRefRW.ValueRW.dir == FourDir.Right)
					{
						float3 float4 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, CampMgr.Inst.CurrentCampSkin.ett_AccessL).Position + new float3(1f, 0f, 0f);
						LevelMgr.Inst.CurrentRoomCtrller.fogCtrller.Hide(float4);
					}
					else if (uncheckedRefRW.ValueRW.dir == FourDir.Up)
					{
						LevelMgr.Inst.CurrentRoomCtrller.fogCtrller.Hide(CampMgr.Inst.endlessExitPos);
					}
					else
					{
						LevelMgr.Inst.CurrentRoomCtrller.fogCtrller.Hide(CampMgr.Inst.endlessEntryPos);
					}
					UIPlayerDataMgr.Inst.ClearDragData();
					GeneralTool.SyncTeammatesPosition(PlayerMgr.Inst.PlayerPointIgnoreZ);
				}
				else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Item_RO_ComponentLookup, ref state, dynamicBuffer[i].GetOtherEntity(entity2)))
				{
					RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity2);
					switch (uncheckedRefRW.ValueRW.dir)
					{
					case FourDir.Up:
						componentRWAfterCompletingDependency2.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(0f, -1f, 0f);
						break;
					case FourDir.Right:
						componentRWAfterCompletingDependency2.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(-1f, 0f, 0f);
						break;
					case FourDir.Down:
						componentRWAfterCompletingDependency2.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(0f, 1f, 0f);
						break;
					case FourDir.Left:
						componentRWAfterCompletingDependency2.ValueRW.Position = uncheckedRefRO.ValueRO.Position + new float3(1f, 0f, 0f);
						break;
					default:
						Debug.LogError(uncheckedRefRW.ValueRW.dir);
						break;
					}
				}
			}
		}
		if (flag)
		{
			List<Vector3> list = new List<Vector3>();
			for (int j = 0; j < CampMgr.Inst.tsf_ScarecrowParent.childCount; j++)
			{
				list.Add(CampMgr.Inst.tsf_ScarecrowParent.GetChild(j).position);
			}
			CampMgr.Inst.DestroyScarecrowPoints();
			for (int k = 0; k < list.Count; k++)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Units/" + 10501), list[k], quaternion.identity, CampMgr.Inst.tsf_ScarecrowParent).SetActive(value: true);
			}
		}
		if (!flag2)
		{
			return;
		}
		for (int l = 0; l < PlayerMgr.Inst.Wands.Count; l++)
		{
			Wand wand = PlayerMgr.Inst.Wands[l];
			PlayerMgr.Inst.CancelAutoControlWand(wand);
		}
		PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.Clear();
		PlayerMgr.Inst.BaData.relicCfgs.Clear();
		UIPlayerDataMgr.Inst.RelicUpdate();
		PlayerMgr.Inst.RefreshPlayer(cancelAutoWand: false);
		PlayerMgr.Inst.InvincibleRegister();
		foreach (Wand wand3 in PlayerMgr.Inst.Wands)
		{
			wand3.UpdateHandDisplay();
		}
		ClearPoolInCamp();
	}

	public static void ClearPoolInCamp()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		UnitProperty[] componentsInChildren = CampMgr.Inst.tsf_ScarecrowParent.GetComponentsInChildren<UnitProperty>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(componentsInChildren[i].myEntity, value: false);
		}
		if (CampMgr.Inst.npc1Vivian.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc1Vivian.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc2Nimue.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc2Nimue.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc3.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc3.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc4.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc4.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc5.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc5.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc6.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc6.belongEtt, value: false);
		}
		if (CampMgr.Inst.npc9.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc9.belongEtt, value: false);
		}
		GameMgr.Inst.DestroyAllTeammate();
		GameMgr.Inst.ClearAllPool();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(componentsInChildren[j].myEntity, value: true);
		}
		if (CampMgr.Inst.npc1Vivian.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc1Vivian.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc2Nimue.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc2Nimue.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc3.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc3.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc4.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc4.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc5.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc5.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc6.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc6.belongEtt, value: true);
		}
		if (CampMgr.Inst.npc9.belongEtt != Entity.Null)
		{
			entityManager.SetComponentEnabled<EnterDoorDestroy>(CampMgr.Inst.npc9.belongEtt, value: true);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<AccessCamp>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		__query_51121654_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000568A_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((AccessCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((AccessCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((AccessCampSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
