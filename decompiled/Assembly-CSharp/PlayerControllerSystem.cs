using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateAfter(typeof(TransformSystemGroup))]
[CompilerGenerated]
public class PlayerControllerSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<PlayerController_Dots> __PlayerController_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Item> __Item_RO_ComponentLookup;

		public ComponentLookup<Item> __Item_RW_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<Spell10201Coin> __Spell10201Coin_RO_ComponentLookup;

		public ComponentLookup<Spell10201Coin> __Spell10201Coin_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<InteractiveObj_Dots> __InteractiveObj_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__PlayerController_Dots_RW_ComponentLookup = state.GetComponentLookup<PlayerController_Dots>();
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Item_RO_ComponentLookup = state.GetComponentLookup<Item>(isReadOnly: true);
			__Item_RW_ComponentLookup = state.GetComponentLookup<Item>();
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__Spell10201Coin_RO_ComponentLookup = state.GetComponentLookup<Spell10201Coin>(isReadOnly: true);
			__Spell10201Coin_RW_ComponentLookup = state.GetComponentLookup<Spell10201Coin>();
			__InteractiveObj_Dots_RO_ComponentLookup = state.GetComponentLookup<InteractiveObj_Dots>(isReadOnly: true);
		}
	}

	private List<DistanceHit> interactiveObjHits = new List<DistanceHit>();

	private CollisionFilter interactiveCollisionFilter;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1481211065_0;

	private EntityQuery __query_1481211065_1;

	private EntityQuery __query_1481211065_2;

	public static PlayerControllerSystem Inst { get; private set; }

	[Preserve]
	protected override void OnCreate()
	{
		Inst = this;
		RequireForUpdate<PlayerController_Dots>();
		interactiveCollisionFilter = new CollisionFilter
		{
			BelongsTo = 512u,
			CollidesWith = 33816576u,
			GroupIndex = 0
		};
	}

	[Preserve]
	protected override void OnUpdate()
	{
		Entity singletonEntity = __query_1481211065_0.GetSingletonEntity();
		RefRW<PlayerController_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__PlayerController_Dots_RW_ComponentLookup, ref base.CheckedStateRef, singletonEntity);
		RefRO<LocalToWorld> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, singletonEntity);
		if (componentRWAfterCompletingDependency.ValueRW.playerCtrllerMono.Value != null && PlayerMgr.Inst.ItemCtrller.potion_Petrifaction == null)
		{
			componentRWAfterCompletingDependency.ValueRW.playerCtrllerMono.Value.transform.position = componentROAfterCompletingDependency.ValueRO.Position;
		}
		if (componentRWAfterCompletingDependency.ValueRW.needUpdateShieldUI)
		{
			componentRWAfterCompletingDependency.ValueRW.needUpdateShieldUI = false;
			UIPlayerDataMgr.Inst.UpdateShield();
		}
		if (componentRWAfterCompletingDependency.ValueRW.needUpdateTempShieldUI)
		{
			componentRWAfterCompletingDependency.ValueRW.needUpdateTempShieldUI = false;
			UIPlayerDataMgr.Inst.UpdateShieldTemp();
		}
		if (componentRWAfterCompletingDependency.ValueRW.isPlayerDropBlood)
		{
			componentRWAfterCompletingDependency.ValueRW.isPlayerDropBlood = false;
			LevelMgr.Inst.CurrentRoomCtrller.PlayerDropBlood();
		}
		if (componentRWAfterCompletingDependency.ValueRW.isTriggerResurgence)
		{
			componentRWAfterCompletingDependency.ValueRW.isTriggerResurgence = false;
			if (PlayerMgr.Inst.ItemCtrller.relic_Resurgence != null)
			{
				PlayerMgr.Inst.ItemCtrller.relic_Resurgence.TriggerResurgence();
			}
			else
			{
				Debug.LogError("不应该没有重生遗物，请检查代码！");
			}
		}
		RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, singletonEntity);
		DynamicBuffer<TextFloatVFXBED> singletonBuffer = __query_1481211065_1.GetSingletonBuffer<TextFloatVFXBED>();
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		__query_1481211065_2.GetSingleton<PhysicsWorldSingleton>().OverlapSphere(componentRWAfterCompletingDependency2.ValueRO.Position, componentRWAfterCompletingDependency.ValueRW.interactiveRadius, ref outHits, interactiveCollisionFilter);
		interactiveObjHits.Clear();
		foreach (DistanceHit item in outHits)
		{
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Item_RO_ComponentLookup, ref base.CheckedStateRef, item.Entity))
			{
				Item componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RO_ComponentLookup, ref base.CheckedStateRef, item.Entity);
				if (componentAfterCompletingDependency.onPick)
				{
					continue;
				}
				if (componentAfterCompletingDependency.info.type == ItemType.Resource && !componentAfterCompletingDependency.isStore)
				{
					ResourceConfig resourceConfig = ResourceConfig.dic[componentAfterCompletingDependency.info.id];
					switch (resourceConfig.abilityType)
					{
					case ResourceAbilityType.Coin:
						if (GameMgr.IsSupportVFX)
						{
							singletonBuffer.Add(new TextFloatVFXBED
							{
								number = resourceConfig.int1,
								worldPos = item.Position,
								type = UITextFloatType.GetCoin
							});
						}
						else
						{
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + resourceConfig.int1, UITextFloatType.GetCoin, item.Position);
						}
						PlayerMgr.Inst.ChangeCoin(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						if (PlayerMgr.Inst.ItemCtrller.curseCfg_GetCoinLoseHP != null && Random.value * 100f <= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_GetCoinLoseHP.int1.result)
						{
							DynamicBuffer<TakeDamageInfo_Dots> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef, singletonEntity);
							TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
							elem.damage = PlayerMgr.Inst.ItemCtrller.curseCfg_GetCoinLoseHP.int2.result;
							bufferAfterCompletingDependency.Add(elem);
						}
						break;
					case ResourceAbilityType.Key:
						if (GameMgr.IsSupportVFX)
						{
							singletonBuffer.Add(new TextFloatVFXBED
							{
								number = resourceConfig.int1,
								worldPos = item.Position,
								type = UITextFloatType.GetKey
							});
						}
						else
						{
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + resourceConfig.int1, UITextFloatType.GetKey, item.Position);
						}
						PlayerMgr.Inst.ChangeKey(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						break;
					case ResourceAbilityType.HP:
						if (PlayerMgr.Inst.IsFullHP)
						{
							if (PlayerMgr.Inst.ItemCtrller.relic_GreedSeed != null && PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer < PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result)
							{
								UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, resourceConfig.int1, base.EntityManager);
								componentAfterCompletingDependency.onPick = true;
								InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
								resourceConfig.pickSE.PlaySE();
							}
						}
						else
						{
							UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, resourceConfig.int1, base.EntityManager);
							componentAfterCompletingDependency.onPick = true;
							InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
							resourceConfig.pickSE.PlaySE();
						}
						break;
					case ResourceAbilityType.Shield:
						if (GameMgr.IsSupportVFX)
						{
							singletonBuffer.Add(new TextFloatVFXBED
							{
								number = resourceConfig.int1,
								worldPos = item.Position,
								type = UITextFloatType.GetShield
							});
						}
						else
						{
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + resourceConfig.int1, UITextFloatType.GetShield, item.Position);
						}
						PlayerMgr.Inst.ChangeShield(resourceConfig.int1, TextFloatQueueType.None);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						break;
					case ResourceAbilityType.MagicCrystal:
						if (GameMgr.IsSupportVFX)
						{
							singletonBuffer.Add(new TextFloatVFXBED
							{
								number = resourceConfig.int1,
								worldPos = item.Position,
								type = UITextFloatType.GetCrystal
							});
						}
						else
						{
							ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + resourceConfig.int1, UITextFloatType.GetCrystal, item.Position);
						}
						PlayerMgr.Inst.ChangeMagicCrystal(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						break;
					case ResourceAbilityType.AcientBlood:
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1000011.GetText() + "+" + resourceConfig.int1, UITextFloatType.GetAnchientBlood, item.Position);
						PlayerMgr.Inst.ChangeAncientBlood(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						break;
					case ResourceAbilityType.ChaosCore:
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1000020.GetText() + "+" + resourceConfig.int1, UITextFloatType.GetChaosCore, item.Position);
						PlayerMgr.Inst.ChangeChaosCore(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						if (CampMgr.Inst != null)
						{
							DataMgr.SaveSelectedWorldData();
						}
						break;
					case ResourceAbilityType.Gear:
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1000016.GetText() + "+" + resourceConfig.int1, UITextFloatType.GetGear, item.Position);
						PlayerMgr.Inst.ChangeGear(resourceConfig.int1);
						componentAfterCompletingDependency.onPick = true;
						InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item.Entity);
						resourceConfig.pickSE.PlaySE();
						if (CampMgr.Inst != null)
						{
							DataMgr.SaveSelectedWorldData();
						}
						break;
					default:
						Debug.LogError(resourceConfig.abilityType);
						break;
					}
				}
				else
				{
					interactiveObjHits.Add(item);
				}
			}
			else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell10201Coin_RO_ComponentLookup, ref base.CheckedStateRef, item.Entity))
			{
				RefRW<Spell10201Coin> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell10201Coin_RW_ComponentLookup, ref base.CheckedStateRef, item.Entity);
				if (componentRWAfterCompletingDependency3.ValueRW.onPick)
				{
					break;
				}
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + componentRWAfterCompletingDependency3.ValueRW.coinCount, UITextFloatType.GetCoin, item.Position);
				PlayerMgr.Inst.ChangeCoin(componentRWAfterCompletingDependency3.ValueRW.coinCount);
				SEMgr.Inst.itemPick_Coin.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemPickup", item.Position, 1f);
				componentRWAfterCompletingDependency3.ValueRW.onPick = true;
			}
			else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__InteractiveObj_Dots_RO_ComponentLookup, ref base.CheckedStateRef, item.Entity))
			{
				interactiveObjHits.Add(item);
			}
		}
		outHits.Dispose();
		Entity entity = Entity.Null;
		float num = 100000000f;
		for (int i = 0; i < interactiveObjHits.Count; i++)
		{
			if (entity == Entity.Null)
			{
				entity = interactiveObjHits[i].Entity;
				num = interactiveObjHits[i].Distance;
			}
			else if (interactiveObjHits[i].Distance < num)
			{
				num = interactiveObjHits[i].Distance;
				entity = interactiveObjHits[i].Entity;
			}
		}
		UIInteractiveObjMgr.Inst.InteractableCount = interactiveObjHits.Count;
		if (!PlayerMgr.Inst.PlayerCtrller.CanInteractive)
		{
			UIInteractiveObjMgr.Inst.InteractiveObjCheck(Entity.Null);
		}
		else
		{
			UIInteractiveObjMgr.Inst.InteractiveObjCheck(entity);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1481211065_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1481211065_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1481211065_2 = entityQueryBuilder2.Build(ref state);
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
	public PlayerControllerSystem()
	{
	}
}
