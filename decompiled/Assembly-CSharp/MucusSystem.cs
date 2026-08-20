using System;
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
[UpdateInGroup(typeof(LiquidSystemGroup))]
public class MucusSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
		}
	}

	public static List<CreateMucusRequest> requests = new List<CreateMucusRequest>();

	public static MucusSystem Inst;

	public EntityQuery mucusQuery;

	public MucusController nowMucusCtrler;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1658030249_0;

	private EntityQuery __query_1658030249_1;

	private EntityQuery __query_1658030249_2;

	public static void CreateMucus(float3 point, float radius)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateMucusRequest(point, radius));
		}
	}

	public static void CreateMucus(float3 point1, float3 point2, float radius)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateMucusRequest(point1, point2, radius));
		}
	}

	public static void CreateBodyStatic(in float3 position, in quaternion orientation, in BlobAssetReference<Unity.Physics.Collider> collider, in EntityManager entityManager, out Entity entity)
	{
		entity = entityManager.CreateEntity(new ComponentType[0]);
		entityManager.AddComponentData(entity, default(LocalToWorld));
		entityManager.AddComponentData(entity, LocalTransform.FromPositionRotation(position, orientation));
		PhysicsCollider physicsCollider = default(PhysicsCollider);
		physicsCollider.Value = collider;
		PhysicsCollider componentData = physicsCollider;
		entityManager.AddComponentData(entity, componentData);
		entityManager.AddSharedComponent(entity, default(PhysicsWorldIndex));
	}

	public void Combine(MucusController controller)
	{
		for (int i = 0; i < controller.uncombinedCollidersList.Count; i++)
		{
			base.EntityManager.DestroyEntity(controller.uncombinedCollidersList[i]);
		}
		controller.uncombinedCollidersList.Clear();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 128u;
		collisionFilter.CollidesWith = 1073741824u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		BlobAssetReference<Unity.Physics.Collider> collider = Unity.Physics.MeshCollider.Create(LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CombinedMesh, filter, Unity.Physics.Material.Default);
		float3 position = Vector3.zero;
		EntityManager entityManager = base.EntityManager;
		CreateBodyStatic(in position, in quaternion.identity, in collider, in entityManager, out var entity);
		base.EntityManager.AddComponent<MucusTag>(entity);
		base.EntityManager.AddComponent<EnterDoorDestroy>(entity);
		controller.combinedAssetList.Add(collider);
		controller.combinedEntityList.Add(entity);
		LevelMgr.Inst.CurrentRoomCtrller.CamMucusRenderOnce();
	}

	public void Clear(MucusController controller, bool clearAsset)
	{
		if (World.DefaultGameObjectInjectionWorld == null || !World.DefaultGameObjectInjectionWorld.IsCreated)
		{
			return;
		}
		EntityCommandBuffer entityCommandBuffer = __query_1658030249_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		List<Entity> combinedEntityList = controller.combinedEntityList;
		for (int i = 0; i < combinedEntityList.Count; i++)
		{
			entityCommandBuffer.DestroyEntity(combinedEntityList[i]);
		}
		combinedEntityList.Clear();
		for (int j = 0; j < controller.uncombinedEntityList.Count; j++)
		{
			entityCommandBuffer.DestroyEntity(controller.uncombinedEntityList[j]);
		}
		controller.uncombinedEntityList.Clear();
		if (clearAsset)
		{
			List<BlobAssetReference<Unity.Physics.Collider>> combinedAssetList = controller.combinedAssetList;
			for (int k = 0; k < combinedAssetList.Count; k++)
			{
				combinedAssetList[k].Dispose();
			}
			combinedAssetList.Clear();
		}
		DynamicBuffer<CreateMucusRequest> singletonBuffer = __query_1658030249_1.GetSingletonBuffer<CreateMucusRequest>();
		if (singletonBuffer.Length > 0)
		{
			singletonBuffer.Clear();
		}
		if (!clearAsset)
		{
			LevelMgr.Inst.CurrentRoomCtrller.CamMucusRenderOnce();
		}
		requests.Clear();
	}

	public void ClearAll()
	{
		EntityCommandBuffer entityCommandBuffer = __query_1658030249_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		NativeArray<Entity> entities = mucusQuery.ToEntityArray(Allocator.Temp);
		entityCommandBuffer.DestroyEntity(entities);
		requests.Clear();
	}

	[Preserve]
	protected override void OnDestroy()
	{
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			EntityCommandBuffer entityCommandBuffer = __query_1658030249_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
			NativeArray<Entity> entities = mucusQuery.ToEntityArray(Allocator.Temp);
			entityCommandBuffer.DestroyEntity(entities);
			requests.Clear();
		});
	}

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<CreateMucusRequest>();
		RequireForUpdate<AllMixedEtt>();
		mucusQuery = base.EntityManager.CreateEntityQuery(typeof(Mucus_Dots));
		Inst = this;
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			EntityCommandBuffer entityCommandBuffer = __query_1658030249_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
			NativeArray<Entity> entities = mucusQuery.ToEntityArray(Allocator.Temp);
			entityCommandBuffer.DestroyEntity(entities);
			requests.Clear();
		});
	}

	[Preserve]
	protected unsafe override void OnUpdate()
	{
		if (!LevelMgr.Inst || !LevelMgr.Inst.CurrentRoomCtrller)
		{
			return;
		}
		DynamicBuffer<CreateMucusRequest> singletonBuffer = __query_1658030249_1.GetSingletonBuffer<CreateMucusRequest>();
		if (singletonBuffer.Length > 0)
		{
			for (int i = 0; i < singletonBuffer.Length; i++)
			{
				requests.Add(singletonBuffer[i]);
			}
			singletonBuffer.Clear();
		}
		if (GameMgr.IsMobile_Static)
		{
			requests.Clear();
		}
		else
		{
			if (requests.Count <= 0)
			{
				return;
			}
			AllMixedEtt singleton = __query_1658030249_2.GetSingleton<AllMixedEtt>();
			CollisionFilter collisionFilter = default(CollisionFilter);
			collisionFilter.BelongsTo = 128u;
			collisionFilter.CollidesWith = 1073741824u;
			collisionFilter.GroupIndex = 0;
			CollisionFilter collisionFilter2 = collisionFilter;
			MucusController mucusCtrller = LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller;
			for (int j = 0; j < requests.Count; j++)
			{
				if (mucusCtrller.pointCount > 2000)
				{
					break;
				}
				CreateMucusRequest createMucusRequest = requests[j];
				Entity entity;
				RefRW<LocalTransform> componentRWAfterCompletingDependency;
				RefRW<PhysicsCollider> componentRWAfterCompletingDependency2;
				if (createMucusRequest.isCircle)
				{
					entity = base.EntityManager.Instantiate(singleton.map["LiquidSphere"]);
					LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(createMucusRequest.point, createMucusRequest.radius);
					Liquid_Dots componentData = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createMucusRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData.colliderEntity);
					ref LocalTransform valueRW = ref componentRWAfterCompletingDependency.ValueRW;
					valueRW.Position = Vector3.zero;
					valueRW.Scale = createMucusRequest.radius / 0.5f;
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, componentData.colliderEntity);
					ref PhysicsCollider valueRW2 = ref componentRWAfterCompletingDependency2.ValueRW;
					valueRW2.MakeUnique(in componentData.colliderEntity, base.EntityManager);
					valueRW2.ColliderPtr->SetCollisionFilter(collisionFilter2);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData.imageEntity);
					ref LocalTransform valueRW3 = ref componentRWAfterCompletingDependency.ValueRW;
					valueRW3.Position = Tool2D.IgnoreZPoint(Vector3.zero, -120f);
					valueRW3.Scale = createMucusRequest.radius / 0.5f;
					base.EntityManager.AddComponent<MucusTag>(componentData.colliderEntity);
					mucusCtrller.uncombinedEntityList.Add(entity);
					mucusCtrller.uncombinedCollidersList.Add(componentData.colliderEntity);
				}
				else
				{
					entity = base.EntityManager.Instantiate(singleton.map["LiquidRectangle"]);
					LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(createMucusRequest.point1, createMucusRequest.point2, createMucusRequest.radius);
					Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(createMucusRequest.point2, createMucusRequest.point1);
					float y = Tool2D.IgnoreZDistance(createMucusRequest.point1, createMucusRequest.point2) * 1.1f;
					Liquid_Dots componentData2 = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createMucusRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.colliderEntity);
					componentRWAfterCompletingDependency.ValueRW.Position = Vector3.zero;
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.colliderEntity);
					ref PhysicsCollider valueRW4 = ref componentRWAfterCompletingDependency2.ValueRW;
					valueRW4.MakeUnique(in componentData2.colliderEntity, base.EntityManager);
					valueRW4.ColliderPtr->SetCollisionFilter(collisionFilter2);
					Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)valueRW4.ColliderPtr;
					BoxGeometry geometry = colliderPtr->Geometry;
					geometry.Size = new float3(createMucusRequest.radius * 2f, y, 0.05f);
					colliderPtr->Geometry = geometry;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.colliderEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, vector), new float3(1f, 1f, 1f));
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.imageEntity);
					componentRWAfterCompletingDependency.ValueRW.Position = Tool2D.IgnoreZPoint(Vector3.zero, -120f);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.imageEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, vector), new float3(createMucusRequest.radius * 2f, y, 1f));
					base.EntityManager.AddComponent<MucusTag>(componentData2.colliderEntity);
					mucusCtrller.uncombinedEntityList.Add(entity);
					mucusCtrller.uncombinedCollidersList.Add(componentData2.colliderEntity);
				}
				base.EntityManager.AddComponent<Mucus_Dots>(entity);
			}
			requests.Clear();
			LevelMgr.Inst.CurrentRoomCtrller.CamMucusRenderOnce();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030249_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateMucusRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030249_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030249_2 = entityQueryBuilder2.Build(ref state);
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
	public MucusSystem()
	{
	}
}
