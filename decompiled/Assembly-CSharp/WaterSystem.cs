using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(LiquidSystemGroup))]
public class WaterSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
		}
	}

	public static List<CreateWaterRequest> requests = new List<CreateWaterRequest>();

	public static WaterSystem Inst;

	public EntityQuery waterQuery;

	public float waterCount;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1658030569_0;

	private EntityQuery __query_1658030569_1;

	private EntityQuery __query_1658030569_2;

	private EntityQuery __query_1658030569_3;

	public static void CreateWater(float3 point, float radius)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateWaterRequest(point, radius));
		}
	}

	public static void CreateWater(float3 point1, float3 point2, float radius)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateWaterRequest(point1, point2, radius));
		}
	}

	public void Clear()
	{
		waterCount = 0f;
		NativeArray<Entity> entities = waterQuery.ToEntityArray(Allocator.Temp);
		__query_1658030569_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
		requests.Clear();
		DynamicBuffer<CreateMucusRequest> singletonBuffer = __query_1658030569_1.GetSingletonBuffer<CreateMucusRequest>();
		if (singletonBuffer.Length > 0)
		{
			singletonBuffer.Clear();
		}
	}

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<CreateWaterRequest>();
		RequireForUpdate<AllMixedEtt>();
		Inst = this;
		waterQuery = base.EntityManager.CreateEntityQuery(typeof(WaterTag));
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			waterCount = 0f;
			NativeArray<Entity> entities = waterQuery.ToEntityArray(Allocator.Temp);
			__query_1658030569_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
			requests.Clear();
			DynamicBuffer<CreateMucusRequest> singletonBuffer = __query_1658030569_1.GetSingletonBuffer<CreateMucusRequest>();
			if (singletonBuffer.Length > 0)
			{
				singletonBuffer.Clear();
			}
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			waterCount = 0f;
			NativeArray<Entity> entities = waterQuery.ToEntityArray(Allocator.Temp);
			__query_1658030569_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
			requests.Clear();
			DynamicBuffer<CreateMucusRequest> singletonBuffer = __query_1658030569_1.GetSingletonBuffer<CreateMucusRequest>();
			if (singletonBuffer.Length > 0)
			{
				singletonBuffer.Clear();
			}
		});
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (!LevelMgr.Inst || !LevelMgr.Inst.CurrentRoomCtrller)
		{
			return;
		}
		DynamicBuffer<CreateWaterRequest> singletonBuffer = __query_1658030569_2.GetSingletonBuffer<CreateWaterRequest>();
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
			AllMixedEtt singleton = __query_1658030569_3.GetSingleton<AllMixedEtt>();
			for (int j = 0; j < requests.Count; j++)
			{
				if (LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.pointCount > 2000)
				{
					requests.Clear();
					return;
				}
				LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CountWater();
				CreateWaterRequest createWaterRequest = requests[j];
				Entity entity;
				RefRW<LocalTransform> componentRWAfterCompletingDependency;
				if (createWaterRequest.isCircle)
				{
					entity = base.EntityManager.Instantiate(singleton.map["LiquidSphere"]);
					Liquid_Dots componentData = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					base.EntityManager.DestroyEntity(componentData.colliderEntity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createWaterRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData.imageEntity);
					ref LocalTransform valueRW = ref componentRWAfterCompletingDependency.ValueRW;
					valueRW.Position = Tool2D.IgnoreZPoint(Vector3.zero, -110f);
					valueRW.Scale = createWaterRequest.radius / 0.5f;
				}
				else
				{
					entity = base.EntityManager.Instantiate(singleton.map["LiquidRectangle"]);
					Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(createWaterRequest.point2, createWaterRequest.point1);
					float y = Tool2D.IgnoreZDistance(createWaterRequest.point1, createWaterRequest.point2) * 1.1f;
					Liquid_Dots componentData2 = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					base.EntityManager.DestroyEntity(componentData2.colliderEntity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createWaterRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.imageEntity);
					componentRWAfterCompletingDependency.ValueRW.Position = Tool2D.IgnoreZPoint(Vector3.zero, -110f);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.imageEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, vector), new float3(createWaterRequest.radius * 2f, y, 1f));
				}
				base.EntityManager.AddComponent<WaterTag>(entity);
			}
			LevelMgr.Inst.CurrentRoomCtrller.CamWaterRenderOnce();
			requests.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030569_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateMucusRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030569_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateWaterRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030569_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030569_3 = entityQueryBuilder2.Build(ref state);
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
	public WaterSystem()
	{
	}
}
