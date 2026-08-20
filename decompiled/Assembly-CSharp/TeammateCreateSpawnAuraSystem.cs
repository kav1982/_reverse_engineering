using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateAfter(typeof(TeammateRegisterSystem))]
[BurstCompile]
[CompilerGenerated]
public class TeammateCreateSpawnAuraSystem : SystemBase
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

	private EntityQuery __query_78269789_0;

	private EntityQuery __query_78269789_1;

	private EntityQuery __query_78269789_2;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<SummonAuraBuffer>();
		base.EntityManager.CreateSingletonBuffer<SummonSoulmateAuraBuffer>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<SummonAuraBuffer> singletonBuffer = __query_78269789_0.GetSingletonBuffer<SummonAuraBuffer>();
		DynamicBuffer<SummonSoulmateAuraBuffer> singletonBuffer2 = __query_78269789_1.GetSingletonBuffer<SummonSoulmateAuraBuffer>();
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer3 = __query_78269789_2.GetSingletonBuffer<GlobalParticleEmitParams>();
		foreach (SummonAuraBuffer item in singletonBuffer)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_SummonEffect", Tool2D.GetLayerPoint(item.spawnPosition, LayerCorrectType.Shadow), item.spawnScale * Vector3.one, 2f);
			SEMgr.Inst.PlaySE("SE_TeammateSummon");
		}
		foreach (SummonSoulmateAuraBuffer item2 in singletonBuffer2)
		{
			GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, "3127_Aura", item2.spawnPosition)
			{
				Size = item2.spawnScale
			};
			singletonBuffer3.Add(elem);
		}
		singletonBuffer.Clear();
		singletonBuffer2.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SummonAuraBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269789_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SummonSoulmateAuraBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269789_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269789_2 = entityQueryBuilder2.Build(ref state);
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
	public TeammateCreateSpawnAuraSystem()
	{
	}
}
