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
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

[CompilerGenerated]
[BurstCompile]
public struct ItemSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_248711515_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public ManagedComponentAccessor<WandConfigComponent> item2_ManagedComponentAccessor;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Item>, WandConfigComponent, InternalCompilerInterface.UncheckedRefRW<Shadow_Dots>, InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Item>, WandConfigComponent, InternalCompilerInterface.UncheckedRefRW<Shadow_Dots>, InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Item>(item1_IntPtr, index), item2_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<Shadow_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LayerCorrect_Dots>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			private ComponentTypeHandle<Item> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<WandConfigComponent> item2_ManagedComponentTypeHandle_RO;

			private ComponentTypeHandle<Shadow_Dots> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LayerCorrect_Dots> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item6_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Item>();
				item2_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<WandConfigComponent>(isReadOnly: false);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Shadow_Dots>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LayerCorrect_Dots>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item2_ManagedComponentTypeHandle_RO, _entityManager);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Item>, WandConfigComponent, InternalCompilerInterface.UncheckedRefRW<Shadow_Dots>, InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Item>, WandConfigComponent, InternalCompilerInterface.UncheckedRefRW<Shadow_Dots>, InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Item>();
			state.EntityManager.CompleteDependencyBeforeRW<WandConfigComponent>();
			state.EntityManager.CompleteDependencyBeforeRW<Shadow_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LayerCorrect_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_248711515_0.TypeHandle __IFE_248711515_0_TypeHandle;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		public ComponentLookup<PhysicsMass> __Unity_Physics_PhysicsMass_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_248711515_0_TypeHandle = new IFE_248711515_0.TypeHandle(ref state);
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
			__Unity_Physics_PhysicsMass_RW_ComponentLookup = state.GetComponentLookup<PhysicsMass>();
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000537B_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000537B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000537B_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_248711515_0;

	private EntityQuery __query_248711515_1;

	private EntityQuery __query_248711515_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Item>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_248711515_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		float num = 0f;
		if (PlayerMgr.Inst != null && PlayerMgr.Inst.ItemCtrller != null && PlayerMgr.Inst.ItemCtrller.curseCfg_PastDueResource != null)
		{
			num = PlayerMgr.Inst.ItemCtrller.curseCfg_PastDueResource.float1.result;
		}
		AllMixedEtt singleton = __query_248711515_2.GetSingleton<AllMixedEtt>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Item>, WandConfigComponent, InternalCompilerInterface.UncheckedRefRW<Shadow_Dots>, InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> item7 in IFE_248711515_0.Query(__query_248711515_0, __TypeHandle.__IFE_248711515_0_TypeHandle, ref state))
		{
			item7.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Item> uncheckedRefRW = item;
			WandConfigComponent wandConfigComponent = item2;
			InternalCompilerInterface.UncheckedRefRW<Shadow_Dots> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRO<LayerCorrect_Dots> uncheckedRefRO = item4;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item5;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW4 = item6;
			Entity entity2 = entity;
			ref Item valueRW = ref uncheckedRefRW.ValueRW;
			bool flag = valueRW.itemMono.Value != null;
			if (!valueRW.isInitialized)
			{
				valueRW.isInitialized = true;
				RefRW<PhysicsCollider> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, entity2);
				componentRWAfterCompletingDependency.ValueRW.MakeUnique(in entity2, ecb);
				if (valueRW.info.type == ItemType.Resource && ResourceConfig.dic[valueRW.info.id].abilityType == ResourceAbilityType.Coin)
				{
					string text = ((GameMgr.CampSkinType == CampSkinType.Summer) ? "Item_CoinSummer" : "Item_Coin");
					switch (valueRW.info.id)
					{
					case 12:
						text = ((GameMgr.CampSkinType == CampSkinType.Summer) ? "Item_DiamandSummer" : "Item_Diamand");
						break;
					case 13:
						text = ((GameMgr.CampSkinType == CampSkinType.Summer) ? "Item_GreenCrystalSummer" : "Item_GreenCrystal");
						break;
					}
					Entity entity3 = state.EntityManager.Instantiate(singleton.map[text]);
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, LocalTransform.Identity, entity3);
					ecb.AddComponent<Parent>(entity3);
					ecb.AddComponent<LayerCorrect_Dots>(entity2);
					ecb.SetComponent(entity2, new LayerCorrect_Dots
					{
						updateEveryFrame = true,
						ett_Layer = entity3
					});
					ecb.SetComponent(entity3, new Parent
					{
						Value = entity2
					});
					InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref state, entity2).Add(new LinkedEntityGroup
					{
						Value = entity3
					});
					ecb.AddComponent(entity2, new ItemDelayActiveTriggerData
					{
						DelayTimer = 0.66f,
						isCoin = true
					});
				}
				else
				{
					valueRW.itemMono.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/ItemMono").GetComponent<ItemMono>();
				}
				if (valueRW.info.type == ItemType.Resource && ResourceConfig.dic[valueRW.info.id].abilityType == ResourceAbilityType.Gear)
				{
					ecb.AddComponent<EndlessItemPick>(entity2);
					ecb.SetComponent(entity2, new EndlessItemPick
					{
						startPoint = uncheckedRefRW3.ValueRO.Position,
						startLerpTime = 1.2f,
						lerpTime = math.min(Tool2D.IgnoreZDistance(uncheckedRefRW3.ValueRO.Position, PlayerMgr.Inst.PlayerPoint) / 10f, UnityEngine.Random.Range(0.7f, 1.5f))
					});
				}
				flag = valueRW.itemMono.Value != null;
				if (flag)
				{
					valueRW.itemMono.Value.itemEtt = entity2;
					valueRW.itemMono.Value.UpdateDisplay(valueRW.info);
				}
				if (valueRW.isStore)
				{
					RefRW<PhysicsMass> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsMass_RW_ComponentLookup, ref state, entity2);
					componentRWAfterCompletingDependency2.ValueRW.InverseMass = 0f;
					componentRWAfterCompletingDependency2.ValueRW.InverseInertia = float3.zero;
					DTool.SetCollider(in componentRWAfterCompletingDependency.ValueRO, 262144u);
					if (flag)
					{
						valueRW.itemMono.Value.UpdatePrice();
						valueRW.itemMono.Value.AnimaHover();
					}
					if (!valueRW.isEndless)
					{
						float3 worldPos = uncheckedRefRW3.ValueRO.Position + new float3(0f, valueRW.storeBaseOffsetY, 0f);
						if (valueRW.info.type == ItemType.Relic)
						{
							QuickCreateSystem.Inst.CreateMixedEtt("ItemStoreBaseWild", worldPos);
						}
						else
						{
							QuickCreateSystem.Inst.CreateMixedEtt("ItemStoreBase", worldPos);
						}
					}
				}
				else if (flag)
				{
					valueRW.itemMono.Value.AnimaFly();
				}
				switch (valueRW.info.type)
				{
				case ItemType.Wand:
				{
					DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Wand, valueRW.info.id);
					for (int i = 0; i < WandConfig.dic[valueRW.info.id].normalSlots.Length; i++)
					{
						if (WandConfig.dic[valueRW.info.id].normalSlots[i] != null)
						{
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, WandConfig.dic[valueRW.info.id].normalSlots[i].id);
						}
					}
					for (int j = 0; j < WandConfig.dic[valueRW.info.id].postSlots.Length; j++)
					{
						if (WandConfig.dic[valueRW.info.id].postSlots[j] != null)
						{
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, WandConfig.dic[valueRW.info.id].postSlots[j].id);
						}
					}
					break;
				}
				case ItemType.Spell:
					DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, valueRW.info.id);
					break;
				case ItemType.Relic:
					DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, valueRW.info.id);
					break;
				case ItemType.Potion:
					DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Potion, valueRW.info.id);
					break;
				}
			}
			if (uncheckedRefRW4.ValueRW.onSelect)
			{
				uncheckedRefRW4.ValueRW.onSelect = false;
				if (flag)
				{
					valueRW.itemMono.Value.OnSelect();
				}
			}
			if (uncheckedRefRW4.ValueRW.onDeselect)
			{
				uncheckedRefRW4.ValueRW.onDeselect = false;
				if (flag)
				{
					valueRW.itemMono.Value.OnDeselect();
				}
			}
			if (uncheckedRefRW4.ValueRW.onInteract)
			{
				uncheckedRefRW4.ValueRW.onInteract = false;
				ItemInfo info = valueRW.info;
				if (valueRW.isStore)
				{
					if (valueRW.IsAffordable())
					{
						if (info.type == ItemType.Relic)
						{
							if (RelicConfig.dic[info.id].dropType == ItemDropType.Epic)
							{
								PlayerMgr.Inst.ChangeHPMax(-valueRW.GetFinalPrice());
							}
							else if (GameMgr.InEndlessMode)
							{
								PlayerMgr.Inst.ChangeCoin(-valueRW.GetFinalPrice());
							}
						}
						else
						{
							PlayerMgr.Inst.ChangeCoin(-valueRW.GetFinalPrice());
						}
						if (GameMgr.InEndlessMode)
						{
							SpecialObj301EndlessMonsterSpawner.Inst.ItemBought(entity2);
						}
						switch (info.type)
						{
						case ItemType.Wand:
						{
							WandConfig _wandConfig2 = wandConfigComponent.cfg.Copy();
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002204.GetText() + ": " + WandConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
							int pickWandIndex = PlayerMgr.Inst.GetPickWandIndex();
							Vector3 position = UIPlayerDataMgr.Inst.uiWands[pickWandIndex].image_Icon.transform.position;
							Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, position);
							RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
							PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, info.id, RollRewardFly.DropType.Wand, uncheckedRefRW3.ValueRO.Position, localPoint + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
							{
								PlayerMgr.Inst.WandPickUp(_wandConfig2);
							});
							break;
						}
						case ItemType.Spell:
						{
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002203.GetText() + ": " + SpellConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
							SlotData _slotData2 = new SlotData(info.id, info.specialInt);
							Vector3 worldPoint = ((!GameMgr.IsMobile_Static) ? UIPlayerDataMgr.Inst.image_BagBtn.transform.position : UIPlayerDataMgr.Inst.image_BagBtn.transform.position);
							Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
							RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint2, null, out var localPoint2);
							PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _slotData2.id, RollRewardFly.DropType.Spell, uncheckedRefRW3.ValueRO.Position, localPoint2 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
							{
								PlayerMgr.Inst.SpellPick(_slotData2);
								if (GameMgr.IsMobile_Static)
								{
									UIPlayerDataMgr.Inst.BagShakeButton();
								}
							});
							break;
						}
						case ItemType.Relic:
						{
							PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ": " + RelicConfig.dic[info.id].GetName(haveLevel: false), UITextFloatType.Normal);
							Vector2 vector = uncheckedRefRW3.ValueRO.Position.GetVector2();
							PlayerMgr.Inst.ItemCtrller.AddRewardFly(info.id, RollRewardFly.DropType.Relic, vector);
							break;
						}
						case ItemType.Potion:
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002209.GetText() + ": " + PotionConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
							PlayerMgr.Inst.ItemCtrller.PotionPickup(info.id);
							break;
						case ItemType.Resource:
							switch (ResourceConfig.dic[info.id].abilityType)
							{
							case ResourceAbilityType.Key:
								PlayerMgr.Inst.ChangeKey(ResourceConfig.dic[info.id].int1, TextFloatQueueType.DirectFloat);
								break;
							case ResourceAbilityType.HP:
								if (PlayerMgr.Inst.IsFullHP)
								{
									QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, PlayerMgr.Inst.PlayerPointIgnoreZ);
								}
								else
								{
									UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, ResourceConfig.dic[info.id].int1, state.EntityManager);
								}
								break;
							case ResourceAbilityType.Shield:
								PlayerMgr.Inst.ChangeShield(ResourceConfig.dic[info.id].int1);
								break;
							default:
								Debug.LogError(ResourceConfig.dic[info.id].abilityType);
								break;
							}
							break;
						case ItemType.Curse:
							Debug.LogError("理论上不应该有可以交互诅咒");
							break;
						case ItemType.RuneWizardRune:
							if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
							{
								RoomController currentRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
								Vector3 doorToWalkablePoint = currentRoomCtrller.GetDoorToWalkablePoint(PlayerMgr.Inst.PlayerPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * UnityEngine.Random.Range(0.5f, 1f));
								doorToWalkablePoint = currentRoomCtrller.GetDoorToWalkablePoint((Vector3)uncheckedRefRW3.ValueRO.Position + UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * UnityEngine.Random.Range(0.5f, 1f));
								Entity ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.RuneWizardRune, OutputMgr_Dots.GetLevelReward(LevelRewardType.RuneWizardRune), doorToWalkablePoint);
								currentRoomCtrller.LevelRewardRegister(ett);
							}
							break;
						case ItemType.MaxHp:
						{
							int @int = ResourceConfig.dic[info.id].int1;
							PlayerMgr.Inst.ChangeHPMax(@int);
							UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, @int, state.EntityManager, needTextFloat: false, needCreateEF: false);
							LevelMgr.Inst.RoomFinishLogger.rewards.Add(new RoomFinishLogger.Reward.MaxHp
							{
								number = @int
							});
							break;
						}
						default:
							Debug.LogError(info.type);
							break;
						}
						if (valueRW.curseID != 0)
						{
							PlayerMgr.Inst.ItemCtrller.CurseAdd(valueRW.curseID, uncheckedRefRW3.ValueRO.Position);
						}
						valueRW.onPick = true;
						SEMgr.Inst.pickItem.PlaySE();
					}
				}
				else
				{
					switch (info.type)
					{
					case ItemType.Wand:
					{
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002204.GetText() + ": " + WandConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
						WandConfig _wandConfig = wandConfigComponent.cfg.Copy();
						int pickWandIndex2 = PlayerMgr.Inst.GetPickWandIndex();
						Vector3 position2 = UIPlayerDataMgr.Inst.uiWands[pickWandIndex2].image_Icon.transform.position;
						Vector2 screenPoint3 = RectTransformUtility.WorldToScreenPoint(null, position2);
						RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint3, null, out var localPoint3);
						PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _wandConfig.id, RollRewardFly.DropType.Wand, uncheckedRefRW3.ValueRO.Position, localPoint3 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
						{
							PlayerMgr.Inst.WandPickUp(_wandConfig);
						});
						break;
					}
					case ItemType.Spell:
					{
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002203.GetText() + ": " + SpellConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
						if (SpellConfig.dic[info.id].dropType == ItemDropType.Epic)
						{
							SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.GetEpicSpell);
						}
						else if (SpellConfig.dic[info.id].dropType == ItemDropType.Special)
						{
							SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.GetSpecialSpell);
						}
						if (DataMgr.selectedWorldData.firstPickSpell && !GameMgr.IsMobile_Static)
						{
							DataMgr.selectedWorldData.firstPickSpell = false;
							if (!UIPlayerDataMgr.Inst.IsBagOpen && UIGuideMgr.Inst == null)
							{
								UIPlayerDataMgr.Inst.BagOpen();
							}
						}
						SlotData _slotData = new SlotData(info.id, info.specialInt);
						if ((bool)Guide2Mgr.Inst && Guide2Mgr.Inst.state == Guide2Mgr.Guide2State.WaitPickSpell)
						{
							UIPlayerDataMgr.Inst.WandShow();
							PlayerMgr.Inst.ItemCtrller.UIRewardFlyToTransform(UIMgr.Inst.canvas_3, _slotData.id, RollRewardFly.DropType.Spell, uncheckedRefRW3.ValueRO.Position, UIPlayerDataMgr.Inst.image_BagBtn.transform, new Vector2(-10f, -10f), useParticleColor: true, delegate
							{
								PlayerMgr.Inst.SpellPick(_slotData);
								if (GameMgr.IsMobile_Static)
								{
									UIPlayerDataMgr.Inst.BagShakeButton();
								}
							});
							break;
						}
						Vector3 position3 = UIPlayerDataMgr.Inst.image_BagBtn.transform.position;
						Vector2 screenPoint4 = RectTransformUtility.WorldToScreenPoint(null, position3);
						RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint4, null, out var localPoint4);
						PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, _slotData.id, RollRewardFly.DropType.Spell, uncheckedRefRW3.ValueRO.Position, localPoint4 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
						{
							PlayerMgr.Inst.SpellPick(_slotData);
							if (GameMgr.IsMobile_Static)
							{
								UIPlayerDataMgr.Inst.BagShakeButton();
							}
							if (GameUISingletonMono<UICompound>.StaticIsOpen)
							{
								GameUISingletonMono<UICompound>.Inst.Hide();
							}
						});
						break;
					}
					case ItemType.Relic:
					{
						PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd(1002202.GetText() + ": " + RelicConfig.dic[info.id].GetName(haveLevel: false), UITextFloatType.Normal);
						int? relicFlyPreProcess = PlayerItemController.GetRelicFlyPreProcess(info.id);
						if (relicFlyPreProcess.HasValue)
						{
							Vector2 vector2 = uncheckedRefRW3.ValueRO.Position.GetVector2();
							PlayerMgr.Inst.ItemCtrller.AddRewardFly(relicFlyPreProcess.Value, RollRewardFly.DropType.Relic, vector2);
						}
						break;
					}
					case ItemType.Potion:
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002209.GetText() + ": " + PotionConfig.dic[info.id].GetName(), UITextFloatType.Normal, uncheckedRefRW3.ValueRO.Position);
						PlayerMgr.Inst.ItemCtrller.PotionPickup(info.id);
						break;
					case ItemType.Curse:
						Debug.LogError("理论上不应该有可以交互诅咒");
						break;
					default:
						Debug.LogError(info.type);
						break;
					case ItemType.Resource:
						break;
					}
					valueRW.onPick = true;
					SEMgr.Inst.pickItem.PlaySE();
				}
			}
			if (valueRW.onChapter3Reposition)
			{
				valueRW.onChapter3Reposition = false;
				uncheckedRefRW3.ValueRW.Position += valueRW.repositionValue;
			}
			if (!valueRW.isStore && LevelMgr.Inst.CurrentRoomMapPos == valueRW.belongRoomMapPos)
			{
				valueRW.stuckCheckIntervalTimer += state.WorldUnmanaged.Time.DeltaTime;
				if (valueRW.stuckCheckIntervalTimer >= 10f)
				{
					valueRW.stuckCheckIntervalTimer = 0f;
					if (NavMesh.SamplePosition(Tool2D.IgnoreZPoint(uncheckedRefRW3.ValueRW.Position, -0.05f), out var _, 0.5f, 16))
					{
						valueRW.isStuck = false;
					}
					else if (valueRW.isStuck)
					{
						valueRW.isStuck = false;
						uncheckedRefRW3.ValueRW.Position = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRW3.ValueRW.Position);
					}
					else
					{
						valueRW.isStuck = true;
					}
				}
			}
			if (flag)
			{
				valueRW.itemMono.Value.transform.position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state, uncheckedRefRO.ValueRO.ett_Layer).Position;
			}
			if (valueRW.onUpdatePrice)
			{
				valueRW.onUpdatePrice = false;
				if (flag)
				{
					valueRW.itemMono.Value.UpdatePrice();
				}
			}
			if (valueRW.onRefresh)
			{
				valueRW.onRefresh = false;
				if (valueRW.isStore)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Item/Potion_WhiteSmoke", uncheckedRefRW3.ValueRO.Position + new float3(0f, 0.2f, 0f), 2f);
					switch (valueRW.info.type)
					{
					case ItemType.Wand:
						PlayerMgr.Inst.BaData.BackWandToPool(valueRW.info.id);
						valueRW.info.id = PlayerMgr.Inst.BaData.GetWandFromPool(WandConfig.dic[valueRW.info.id].dropStage, valueRW.info.id);
						wandConfigComponent.cfg = WandConfig.GetConfig(valueRW.info.id);
						break;
					case ItemType.Spell:
						valueRW.info.id = PlayerMgr.Inst.BaData.GetSpellFromPool(SpellConfig.dic[valueRW.info.id].level, SpellConfig.dic[valueRW.info.id].dropType, valueRW.info.id);
						break;
					case ItemType.Relic:
						PlayerMgr.Inst.BaData.BackRelicToPool(valueRW.info.id, 1);
						valueRW.info.id = PlayerMgr.Inst.BaData.GetRelicFromPool(RelicConfig.dic[valueRW.info.id].dropType, new int[1] { valueRW.info.id });
						break;
					case ItemType.Potion:
						valueRW.info.id = PlayerMgr.Inst.BaData.GetPotionFromPool(valueRW.info.id);
						break;
					case ItemType.Resource:
					{
						List<int> list = new List<int> { 31, 32, 33, 41, 42, 43, 21, 22 };
						switch (valueRW.info.id)
						{
						case 31:
							list.Remove(31);
							break;
						case 32:
							list.Remove(32);
							break;
						case 33:
							list.Remove(33);
							break;
						case 41:
							list.Remove(41);
							break;
						case 42:
							list.Remove(42);
							break;
						case 43:
							list.Remove(43);
							break;
						case 21:
							list.Remove(21);
							break;
						case 22:
							list.Remove(22);
							break;
						default:
							Debug.LogError(valueRW.info.id);
							break;
						}
						valueRW.info.id = list[UnityEngine.Random.Range(0, list.Count)];
						break;
					}
					default:
						Debug.LogError(valueRW.info.type);
						break;
					}
					if (flag)
					{
						valueRW.itemMono.Value.UpdateDisplay(valueRW.info);
						valueRW.itemMono.Value.UpdatePrice();
					}
					if (valueRW.curseID != 0)
					{
						valueRW.curseID = PlayerMgr.Inst.BaData.GetCurseFromPool(CurseConfig.dic[valueRW.curseID].dropType);
					}
					try
					{
						switch (valueRW.info.type)
						{
						case ItemType.Wand:
						{
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Wand, valueRW.info.id);
							for (int k = 0; k < WandConfig.dic[valueRW.info.id].normalSlots.Length; k++)
							{
								if (WandConfig.dic[valueRW.info.id].normalSlots[k] != null)
								{
									DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, WandConfig.dic[valueRW.info.id].normalSlots[k].id);
								}
							}
							for (int l = 0; l < WandConfig.dic[valueRW.info.id].postSlots.Length; l++)
							{
								if (WandConfig.dic[valueRW.info.id].postSlots[l] != null)
								{
									DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, WandConfig.dic[valueRW.info.id].postSlots[l].id);
								}
							}
							break;
						}
						case ItemType.Spell:
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, valueRW.info.id);
							break;
						case ItemType.Relic:
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, valueRW.info.id);
							break;
						case ItemType.Potion:
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Potion, valueRW.info.id);
							break;
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("刷新商店解锁图鉴报错 " + ex);
					}
				}
			}
			if (valueRW.onPick)
			{
				if (flag)
				{
					valueRW.itemMono.Value.gameObject.SetActive(value: false);
				}
				ecb.DestroyEntity(entity2);
			}
			else if (num > 0f && !valueRW.isStore && valueRW.info.type == ItemType.Resource && PlayerMgr.Inst.PlayerCtrller.CanMotion)
			{
				valueRW.curse_DisappearDurationTimer += state.WorldUnmanaged.Time.DeltaTime;
				if (valueRW.curse_DisappearDurationTimer > 3f)
				{
					valueRW.curse_DisappearTwinkleTimer += state.WorldUnmanaged.Time.DeltaTime;
					if (valueRW.curse_DisappearTwinkleTimer >= 0.2f)
					{
						valueRW.curse_DisappearTwinkleTimer = 0f;
						if (flag)
						{
							if (valueRW.itemMono.Value.tsf_move.gameObject.activeSelf)
							{
								valueRW.itemMono.Value.tsf_move.gameObject.SetActive(value: false);
								uncheckedRefRW2.ValueRW.onHide = true;
							}
							else
							{
								valueRW.itemMono.Value.tsf_move.gameObject.SetActive(value: true);
								uncheckedRefRW2.ValueRW.onShow = true;
							}
						}
					}
				}
				if (valueRW.curse_DisappearDurationTimer >= num)
				{
					if (flag)
					{
						ObjPoolMgr.Inst.RecycleGO(valueRW.itemMono.Value.gameObject);
					}
					ecb.DestroyEntity(entity2);
				}
			}
			else if (flag && !valueRW.itemMono.Value.tsf_move.gameObject.activeSelf)
			{
				valueRW.itemMono.Value.tsf_move.gameObject.SetActive(value: true);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LayerCorrect_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Item>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<WandConfigComponent>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Shadow_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		__query_248711515_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_248711515_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_248711515_2 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000537B_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((ItemSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((ItemSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((ItemSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
