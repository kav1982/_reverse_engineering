using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

[BurstCompile]
[CompilerGenerated]
public class SEPlayerSystem : SystemBase
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

	private EntityQuery __query_928807218_0;

	private EntityQuery __query_928807218_1;

	[BurstCompile]
	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<SEData>();
		base.EntityManager.CreateSingletonBuffer<LoopSEData>();
	}

	[BurstCompile]
	[Preserve]
	protected override void OnUpdate()
	{
		foreach (SEData item in __query_928807218_0.GetSingletonBuffer<SEData>())
		{
			SEData current = item;
			SEMgr.Inst.PlaySE(current.SEName, current.SEMode, current.SEMaxCount, current.SEMinInterval).pitch = current.SEPitch;
		}
		foreach (LoopSEData item2 in __query_928807218_1.GetSingletonBuffer<LoopSEData>())
		{
			LoopSEData current2 = item2;
			SEMgr.Inst.PlayLoopSE(current2.SEName, current2.Duration, current2.Volume);
		}
		__query_928807218_0.GetSingletonBuffer<SEData>().Clear();
		__query_928807218_1.GetSingletonBuffer<LoopSEData>().Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_928807218_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LoopSEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_928807218_1 = entityQueryBuilder2.Build(ref state);
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
	public SEPlayerSystem()
	{
	}
}
