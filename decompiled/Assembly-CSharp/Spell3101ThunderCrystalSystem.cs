using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(EnhanceSpellSystemGroup))]
public class Spell3101ThunderCrystalSystem : SystemBase
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

	private EntityQuery __query_304095916_0;

	private EntityQuery __query_304095916_1;

	private EntityQuery __query_304095916_2;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<Spell3101NewThunderHitData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		Entity singletonEntity = __query_304095916_0.GetSingletonEntity();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		foreach (Spell3101NewThunderHitData item in __query_304095916_1.GetSingletonBuffer<Spell3101NewThunderHitData>())
		{
			GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, "3101_Thunder", item.StartPos)
			{
				Size = item.Scale.x / SpellConfig.dic[31011].float1
			};
			__query_304095916_2.GetSingletonBuffer<GlobalParticleEmitParams>().Add(elem);
			FixedString32Bytes seName = "Hit";
			entityCommandBuffer.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(3101, in seName), SEPlayMode.Replay, 3, 0.2f));
		}
		__query_304095916_1.GetSingletonBuffer<Spell3101NewThunderHitData>().Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_304095916_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_304095916_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_304095916_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell3101ThunderCrystalSystem()
	{
	}
}
