using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateAfter(typeof(UnitTakeDamageClearSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public class Spell3112GravitationalChainSystem : SystemBase
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_738641025_0;

	private EntityQuery __query_738641025_1;

	private EntityQuery __query_738641025_2;

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<Spell3112NewChainSingleton>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<Spell3112NewChainSingleton> singletonBuffer = __query_738641025_0.GetSingletonBuffer<Spell3112NewChainSingleton>();
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		DynamicOptimizeData singleton = __query_738641025_1.GetSingleton<DynamicOptimizeData>();
		Entity singletonEntity = __query_738641025_2.GetSingletonEntity();
		foreach (Spell3112NewChainSingleton item in singletonBuffer)
		{
			if (!base.EntityManager.Exists(item.TargetEntity) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, item.TargetEntity))
			{
				continue;
			}
			entityCommandBuffer.AppendToBuffer(item.TargetEntity, item.Info);
			if (UnityEngine.Random.Range(0f, 1f) < singleton.PoolEffectShowRatio)
			{
				Spell3101PullCrystal component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, item.StartPos, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
				if (component.tsf_Layer != null)
				{
					component.tsf_Layer.localScale = item.LineScale;
				}
				component.SetColor(item.ColorType);
				component.SetChainTargetPosition(item.StartPos, item.EndPos);
			}
			FixedString32Bytes seName = "Energy";
			entityCommandBuffer.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(3121, in seName)));
		}
		__query_738641025_0.GetSingletonBuffer<Spell3112NewChainSingleton>().Clear();
		entityCommandBuffer.Playback(base.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3112NewChainSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_738641025_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_738641025_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_738641025_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell3112GravitationalChainSystem()
	{
	}
}
