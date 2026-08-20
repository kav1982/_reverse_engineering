using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PlayerLogger.Events;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
public struct LevelRewardSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_282020234_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<LevelRewardInfoBED> item2_BufferAccessor;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LevelReward>, DynamicBuffer<LevelRewardInfoBED>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LevelReward>, DynamicBuffer<LevelRewardInfoBED>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, LocalTransform>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<LevelReward>(item1_IntPtr, index), item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LevelReward> item1_ComponentTypeHandle_RW;

			private BufferTypeHandle<LevelRewardInfoBED> item2_BufferTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LevelReward>();
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LevelRewardInfoBED>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LevelReward>, DynamicBuffer<LevelRewardInfoBED>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LevelReward>, DynamicBuffer<LevelRewardInfoBED>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<LevelReward>();
			state.EntityManager.CompleteDependencyBeforeRW<LevelRewardInfoBED>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_282020234_0.TypeHandle __IFE_282020234_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_282020234_0_TypeHandle = new IFE_282020234_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000053CD_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000053CD_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000053CD_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_282020234_0;

	private EntityQuery __query_282020234_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<LevelReward>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_282020234_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LevelReward>, DynamicBuffer<LevelRewardInfoBED>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, LocalTransform> item5 in IFE_282020234_0.Query(__query_282020234_0, __TypeHandle.__IFE_282020234_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<LevelReward> uncheckedRefRW = item;
			DynamicBuffer<LevelRewardInfoBED> infos = item2;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item3;
			LocalTransform localTransform = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				float3 layerPosition = DTool.GetLayerPosition(in localTransform.Position, LayerCorrectType.Coordinate);
				uncheckedRefRW.ValueRW.go_EF.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/LevelRewardMono", localTransform.Position + layerPosition);
				Entity entity3;
				Entity entity4;
				switch (uncheckedRefRW.ValueRW.rewardType)
				{
				default:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward0;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward0;
					break;
				case LevelRewardType.Spell:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward1;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward1;
					if (!DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
					{
						break;
					}
					foreach (KeyValuePair<int, int> lockSpellID in DataMgr.selectedWorldData.battleData9.lockSpellIDs)
					{
						if (lockSpellID.Key < infos.Length)
						{
							infos.ElementAt(lockSpellID.Key).info = new ItemInfo(ItemType.Spell, lockSpellID.Value);
							infos.ElementAt(lockSpellID.Key).isLock = true;
						}
					}
					break;
				case LevelRewardType.RuneWizardRune:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward8;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward8;
					break;
				case LevelRewardType.Relic:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward2;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward2;
					break;
				case LevelRewardType.MaxHP:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward3;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward3;
					break;
				case LevelRewardType.Coin:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward4;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward4;
					break;
				case LevelRewardType.Store:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward5;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward5;
					break;
				case LevelRewardType.Process:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward6;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward6;
					break;
				case LevelRewardType.Spring:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward7;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward7;
					break;
				case LevelRewardType.Elite:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward100;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward100;
					break;
				case LevelRewardType.Boss:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward101;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward101;
					break;
				case LevelRewardType.Chapter:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward131;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward131;
					break;
				case LevelRewardType.Shortcut:
					entity3 = uncheckedRefRW.ValueRW.ett_Outline_Reward200;
					entity4 = uncheckedRefRW.ValueRW.ett_Icon_Reward200;
					break;
				}
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity3).ValueRW.Scale = 1f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity4).ValueRW.Scale = 1f;
			}
			uncheckedRefRW.ValueRW.hoverTimer += uncheckedRefRW.ValueRW.hoverSpeed * state.WorldUnmanaged.Time.DeltaTime;
			float y = math.sin(uncheckedRefRW.ValueRW.hoverTimer) * uncheckedRefRW.ValueRW.hoverRange;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Hover).ValueRW.Position = new float3(0f, y, 0f);
			uncheckedRefRW.ValueRW.go_EF.Value.transform.position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Hover).Position;
			if (uncheckedRefRW2.ValueRW.onSelect)
			{
				uncheckedRefRW2.ValueRW.onSelect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW2.ValueRW.ett_Outline).ValueRW.Scale = 1f;
			}
			if (uncheckedRefRW2.ValueRW.onDeselect)
			{
				uncheckedRefRW2.ValueRW.onDeselect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW2.ValueRW.ett_Outline).ValueRW.Scale = 0f;
			}
			if (uncheckedRefRW2.ValueRW.onInteract)
			{
				uncheckedRefRW2.ValueRW.onInteract = false;
				switch (uncheckedRefRW.ValueRW.rewardType)
				{
				case LevelRewardType.Spell:
					if (DataMgr.selectedWorldData.ActivateGirlHave4Pick2() || DataMgr.selectedWorldData.ActivateGirlHave3Pick2())
					{
						GameUISingletonMono<UILevelReward>.ShowInit(entity2);
						Time.timeScale = 0f;
						PlayerMgr.Inst.PlayerCtrller.StopMotion();
					}
					else
					{
						QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, infos, localTransform.Position, 0.5f);
						uncheckedRefRW.ValueRW.UIUse();
					}
					break;
				case LevelRewardType.Wand:
				case LevelRewardType.Relic:
				case LevelRewardType.RuneWizardRune:
					GameUISingletonMono<UILevelReward>.ShowInit(entity2);
					Time.timeScale = 0f;
					PlayerMgr.Inst.PlayerCtrller.StopMotion();
					break;
				case LevelRewardType.MaxHP:
					PlayerMgr.Inst.ChangeHPMax(OutputMgr.RewardMaxHPValue);
					UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, OutputMgr.RewardMaxHPValue, state.EntityManager, needTextFloat: false, needCreateEF: false);
					LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.MaxHp
					{
						number = OutputMgr.RewardMaxHPValue
					});
					uncheckedRefRW.ValueRW.UIUse();
					break;
				case LevelRewardType.Coin:
					QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, infos, localTransform.Position, 1.5f);
					uncheckedRefRW.ValueRW.UIUse();
					break;
				default:
					Debug.LogError(uncheckedRefRW.ValueRW.rewardType);
					break;
				}
			}
			if (!uncheckedRefRW.ValueRW.isUse)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.isUse = false;
			if (!LevelMgr.Inst.CurrentRoomCtrller.levelRewardEttList.Contains(entity2))
			{
				continue;
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_LevelRewardDestroy", localTransform.Position, 2f);
			if (uncheckedRefRW.ValueRW.isUsePlayerSE)
			{
				SEMgr.Inst.levelRewardDestroy.PlaySE();
			}
			if (uncheckedRefRW.ValueRW.rewardType == LevelRewardType.Spell && DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
			{
				DataMgr.selectedWorldData.battleData9.lockSpellIDs.Clear();
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].isLock)
					{
						DataMgr.selectedWorldData.battleData9.lockSpellIDs.Add(i, infos[i].info.id);
					}
				}
			}
			ObjPoolMgr.Inst.RecycleGO(uncheckedRefRW.ValueRW.go_EF.Value);
			LevelMgr.Inst.CurrentRoomCtrller.LevelRewardUnregister(entity2);
			entityCommandBuffer.DestroyEntity(entity2);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LevelRewardInfoBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LevelReward>();
		__query_282020234_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_282020234_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000053CD_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((LevelRewardSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((LevelRewardSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((LevelRewardSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
