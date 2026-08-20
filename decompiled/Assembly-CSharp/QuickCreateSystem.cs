using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
public class QuickCreateSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<Item> __Item_RW_ComponentLookup;

		public ComponentLookup<ItemDrop> __ItemDrop_RW_ComponentLookup;

		public ComponentLookup<LevelReward> __LevelReward_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Item_RW_ComponentLookup = state.GetComponentLookup<Item>();
			__ItemDrop_RW_ComponentLookup = state.GetComponentLookup<ItemDrop>();
			__LevelReward_RW_ComponentLookup = state.GetComponentLookup<LevelReward>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
		}
	}

	public static QuickCreateSystem Inst;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_47382477_0;

	private EntityQuery __query_47382477_1;

	private EntityQuery __query_47382477_2;

	private EntityQuery __query_47382477_3;

	[Preserve]
	protected override void OnCreate()
	{
		Inst = this;
	}

	[Preserve]
	protected override void OnUpdate()
	{
	}

	public Entity CreateItem(Vector2Int roomMapPos, ItemInfo info, float3 worldPos, bool isStore = false, bool isEndless = false)
	{
		AllMixedEtt singleton = __query_47382477_0.GetSingleton<AllMixedEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map["Item"]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		RefRW<Item> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, entity);
		componentRWAfterCompletingDependency.ValueRW.belongRoomMapPos = roomMapPos;
		componentRWAfterCompletingDependency.ValueRW.info = info;
		componentRWAfterCompletingDependency.ValueRW.isStore = isStore;
		componentRWAfterCompletingDependency.ValueRW.isEndless = isEndless;
		if (info.type == ItemType.Wand)
		{
			base.EntityManager.GetComponentObject<WandConfigComponent>(entity).cfg = WandConfig.GetConfig(info.id);
		}
		return entity;
	}

	public Entity CreateItem(Vector2Int roomMapPos, WandConfig wandCfg, float3 worldPos, bool isStore = false, bool isEndless = false)
	{
		AllMixedEtt singleton = __query_47382477_0.GetSingleton<AllMixedEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map["Item"]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		RefRW<Item> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Item_RW_ComponentLookup, ref base.CheckedStateRef, entity);
		componentRWAfterCompletingDependency.ValueRW.belongRoomMapPos = roomMapPos;
		componentRWAfterCompletingDependency.ValueRW.info = new ItemInfo(ItemType.Wand, wandCfg.id);
		componentRWAfterCompletingDependency.ValueRW.isStore = isStore;
		componentRWAfterCompletingDependency.ValueRW.isEndless = isEndless;
		base.EntityManager.GetComponentObject<WandConfigComponent>(entity).cfg = wandCfg;
		return entity;
	}

	public Entity CreateItemDrop(Vector2Int roomMapPos, BlobAssetReference<BlobArray<ItemInfo>> infos, float3 worldPos, float dropRadius, float dropInterval = 0.1f)
	{
		AllMixedEtt singleton = __query_47382477_0.GetSingleton<AllMixedEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map["ItemDrop"]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		RefRW<ItemDrop> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__ItemDrop_RW_ComponentLookup, ref base.CheckedStateRef, entity);
		componentRWAfterCompletingDependency.ValueRW.belongRoomMapPos = roomMapPos;
		componentRWAfterCompletingDependency.ValueRW.infoArray = infos;
		componentRWAfterCompletingDependency.ValueRW.dropRadius = dropRadius;
		componentRWAfterCompletingDependency.ValueRW.dropInterval = dropInterval;
		return entity;
	}

	public Entity CreateItemDrop(Vector2Int roomMapPos, DynamicBuffer<LevelRewardInfoBED> infos, float3 worldPos, float dropRadius, float dropInterval = 0.1f)
	{
		BlobAssetReference<BlobArray<ItemInfo>> infos2 = default(BlobAssetReference<BlobArray<ItemInfo>>);
		using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
		{
			BlobBuilderArray<ItemInfo> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<ItemInfo>>(), infos.Length);
			for (int i = 0; i < infos.Length; i++)
			{
				blobBuilderArray[i] = infos[i].info;
			}
			infos2 = blobBuilder.CreateBlobAssetReference<BlobArray<ItemInfo>>(Allocator.Persistent);
		}
		return CreateItemDrop(roomMapPos, infos2, worldPos, dropRadius, dropInterval);
	}

	public Entity CreateLevelReward(LevelRewardType rewardType, List<ItemInfo> itemInfoList, float3 worldPos)
	{
		AllMixedEtt singleton = __query_47382477_0.GetSingleton<AllMixedEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map["LevelReward"]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__LevelReward_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.rewardType = rewardType;
		DynamicBuffer<LevelRewardInfoBED> buffer = base.EntityManager.GetBuffer<LevelRewardInfoBED>(entity);
		for (int i = 0; i < itemInfoList.Count; i++)
		{
			buffer.Add(new LevelRewardInfoBED(itemInfoList[i].type, itemInfoList[i].id));
		}
		return entity;
	}

	public Entity CreateUnit(int id, float3 worldPos)
	{
		AllUnitEtt singleton = __query_47382477_1.GetSingleton<AllUnitEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map[id]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref base.CheckedStateRef, entity))
		{
			PhysicsCollider collider = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref base.CheckedStateRef, entity);
			collider.MakeUnique(in entity, base.EntityManager);
		}
		return entity;
	}

	public Entity CreateSpecialObj(int id, float3 worldPos)
	{
		AllSpecialObjEtt singleton = __query_47382477_2.GetSingleton<AllSpecialObjEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map[id]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		return entity;
	}

	public Entity CreateMixedEtt(FixedString128Bytes name, float3 worldPos)
	{
		AllMixedEtt singleton = __query_47382477_0.GetSingleton<AllMixedEtt>();
		Entity entity = base.EntityManager.Instantiate(singleton.map[name]);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Position = worldPos;
		return entity;
	}

	public void CreateTextFloatVFX(float number, UITextFloatType type, float3 worldPos)
	{
		__query_47382477_3.GetSingletonBuffer<TextFloatVFXBED>().Add(new TextFloatVFXBED
		{
			number = number,
			type = type,
			worldPos = worldPos
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_47382477_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllUnitEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_47382477_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllSpecialObjEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_47382477_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_47382477_3 = entityQueryBuilder2.Build(ref state);
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
	public QuickCreateSystem()
	{
	}
}
