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
[UpdateBefore(typeof(BloodSplatSystem))]
public class BloodSplatCreateSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<BloodSplat_Dots> __BloodSplat_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public BufferLookup<BloodSplatElement> __BloodSplatElement_RW_BufferLookup;

		public ComponentLookup<MatOverrideColor> __MatOverrideColor_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__BloodSplat_Dots_RW_ComponentLookup = state.GetComponentLookup<BloodSplat_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__BloodSplatElement_RW_BufferLookup = state.GetBufferLookup<BloodSplatElement>();
			__MatOverrideColor_RW_ComponentLookup = state.GetComponentLookup<MatOverrideColor>();
		}
	}

	public static BloodSplatCreateSystem Inst;

	public static List<CreateBloodSplatRequest> requests = new List<CreateBloodSplatRequest>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1658030098_0;

	private EntityQuery __query_1658030098_1;

	private EntityQuery __query_1658030098_2;

	public void CreateBloodSplat(CreateBloodSplatRequest request)
	{
		requests.Add(request);
	}

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		base.EntityManager.CreateSingletonBuffer<CreateBloodSplatRequest>();
		RequireForUpdate<AllMixedEtt>();
		requests.Clear();
		Inst = this;
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<CreateBloodSplatRequest> singletonBuffer = __query_1658030098_0.GetSingletonBuffer<CreateBloodSplatRequest>();
		if (singletonBuffer.Length == 0)
		{
			return;
		}
		for (int i = 0; i < singletonBuffer.Length; i++)
		{
			requests.Add(singletonBuffer[i]);
		}
		singletonBuffer.Clear();
		if (GameMgr.IsHarmony_Static)
		{
			requests.Clear();
			return;
		}
		AllMixedEtt singleton = __query_1658030098_1.GetSingleton<AllMixedEtt>();
		ref GlobalRandom valueRW = ref __query_1658030098_2.GetSingletonRW<GlobalRandom>().ValueRW;
		for (int j = 0; j < requests.Count; j++)
		{
			Entity entity = (requests[j].directional ? base.EntityManager.Instantiate(singleton.map["BloodSplatDirectional"]) : base.EntityManager.Instantiate(singleton.map["BloodSplat"]));
			ref BloodSplat_Dots valueRW2 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__BloodSplat_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW;
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, new LocalTransform
			{
				Position = new float3(requests[j].point.x, requests[j].point.y, 1.17f),
				Rotation = (requests[j].directional ? quaternion.EulerXYZ(0f, 0f, requests[j].rotationZ * (MathF.PI / 180f)) : quaternion.EulerXYZ(0f, 0f, (float)DTool.Random(ref valueRW.random, 0, 360) * (MathF.PI / 180f))),
				Scale = requests[j].size * valueRW2.startScalePercent
			}, entity);
			valueRW2.baseScale = requests[j].size;
			NativeArray<BloodSplatElement> nativeArray = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__BloodSplatElement_RW_BufferLookup, ref base.CheckedStateRef, entity).ToNativeArray(Allocator.Temp);
			float num = math.ceil(DTool.Random(ref valueRW.random, 0, nativeArray.Length));
			for (int k = 0; k < nativeArray.Length; k++)
			{
				if ((float)k == num)
				{
					valueRW2.bloodEntity = nativeArray[k].entity;
					Color red = Color.red;
					red.a = valueRW2.startAlphaPercent * valueRW2.baseAlpha;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref base.CheckedStateRef, valueRW2.bloodEntity).ValueRW.color = red;
				}
				else
				{
					base.EntityManager.DestroyEntity(nativeArray[k].entity);
				}
			}
		}
		requests.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateBloodSplatRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030098_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030098_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030098_2 = entityQueryBuilder2.Build(ref state);
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
	public BloodSplatCreateSystem()
	{
	}
}
