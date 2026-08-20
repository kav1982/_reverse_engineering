using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

[BurstCompile]
[UpdateBefore(typeof(SpellDestroySystem))]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
[CompilerGenerated]
public class TeammateCreateDeadBloodSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct TypeHandle
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1629008014_0;

	private EntityQuery __query_1629008014_1;

	private EntityQuery __query_1629008014_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<GlobalRandom>();
		base.EntityManager.CreateSingletonBuffer<TeammateDeadBloodEffectBuffer>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer entityCommandBuffer = __query_1629008014_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.DefaultGameObjectInjectionWorld.Unmanaged);
		DynamicBuffer<TeammateDeadBloodEffectBuffer> singletonBuffer = __query_1629008014_1.GetSingletonBuffer<TeammateDeadBloodEffectBuffer>();
		foreach (TeammateDeadBloodEffectBuffer item in singletonBuffer)
		{
			float3 @float = new float3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0f);
			FixedString32Bytes fs = default(FixedString32Bytes);
			FixedStringMethods.CopyFrom(ref fs, GameMgr.IsChAge14_Static ? "EF_Dead_GhostG" : "EF_Dead_BloodG");
			entityCommandBuffer.AppendToBuffer(__query_1629008014_2.GetSingletonEntity(), new GlobalParticleEmitParams
			{
				Position = item.spawnPosition + @float,
				Size = item.spawnScale,
				Name = fs,
				Type = GlobalParticleType.EF
			});
		}
		singletonBuffer.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008014_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateDeadBloodEffectBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008014_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008014_2 = entityQueryBuilder2.Build(ref state);
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
	public TeammateCreateDeadBloodSystem()
	{
	}
}
