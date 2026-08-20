using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
internal class Spell1018FallEffectSystem : SystemBase
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

	private EntityQuery __query_1732132266_0;

	private EntityQuery __query_1732132266_1;

	private EntityQuery __query_1732132266_2;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<Spell1018FallExplosionBuffer>();
		base.EntityManager.CreateSingletonBuffer<Spell1018FallLightingBuffer>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<Spell1018FallExplosionBuffer> singletonBuffer = __query_1732132266_0.GetSingletonBuffer<Spell1018FallExplosionBuffer>();
		foreach (Spell1018FallExplosionBuffer item in singletonBuffer)
		{
			Spell1018FallExplosionBuffer current = item;
			current.spellColorType.ColorEnumToString(out var result);
			DynamicBuffer<GlobalParticleEmitParams> singletonBuffer2 = __query_1732132266_1.GetSingletonBuffer<GlobalParticleEmitParams>();
			GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1018_FallExplosion_{result}", current.currentPosition)
			{
				Size = current.scale
			};
			singletonBuffer2.Add(elem);
			if (!current.isFinalBound)
			{
				LineRenderer component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/1018/1018_FallLighting_" + current.spellColorType, current.currentPosition, 1f).GetComponent<LineRenderer>();
				float3 @float = current.nextPosition + current.currentPosition;
				@float = new float3(@float.x * 0.5f, @float.y * 0.5f + 2f, -4f);
				component.positionCount = 21;
				for (int i = 0; i <= 20; i++)
				{
					float3 float2 = DTool.QuadraticBezierCurve(in current.currentPosition, in @float, in current.nextPosition, (float)i / 20f);
					component.SetPosition(i, float2);
				}
			}
		}
		singletonBuffer.Clear();
		DynamicBuffer<Spell1018FallLightingBuffer> singletonBuffer3 = __query_1732132266_2.GetSingletonBuffer<Spell1018FallLightingBuffer>();
		foreach (Spell1018FallLightingBuffer item2 in singletonBuffer3)
		{
			LineRenderer component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/1018/1018_FallLighting_" + item2.spellColorType, item2.position, 1f).GetComponent<LineRenderer>();
			component2.positionCount = 2;
			component2.SetPosition(0, Tool2D.GetLayerPoint(item2.position));
			component2.SetPosition(1, Tool2D.GetLayerPoint(item2.endPosition));
		}
		singletonBuffer3.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1018FallExplosionBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1732132266_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1732132266_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1018FallLightingBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1732132266_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell1018FallEffectSystem()
	{
	}
}
