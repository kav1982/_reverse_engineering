using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
[BurstCompile]
[CompilerGenerated]
public class SpellSingletonSystem : SystemBase
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<SpellSingleton> __SpellSingleton_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellSingleton_RO_ComponentLookup = state.GetComponentLookup<SpellSingleton>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1056228640_0;

	private EntityQuery __query_1056228640_1;

	private EntityQuery __query_1056228640_2;

	private EntityQuery __query_1056228640_3;

	private EntityQuery __query_1056228640_4;

	private EntityQuery __query_1056228640_5;

	private EntityQuery __query_1056228640_6;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<SpellSingletonTag>();
		RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
	}

	[Preserve]
	protected override void OnDestroy()
	{
		if (__query_1056228640_0.HasSingleton<SpellSingleton>())
		{
			__query_1056228640_0.GetSingleton<SpellSingleton>().Dispose();
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		Entity singletonEntity = __query_1056228640_1.GetSingletonEntity();
		RefRW<SpellSingleton> singletonRW;
		if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellSingleton_RO_ComponentLookup, ref base.CheckedStateRef, singletonEntity))
		{
			base.EntityManager.AddComponent<SpellSingleton>(singletonEntity);
			SpellSingleton.Create(out var result);
			base.EntityManager.SetComponentData(singletonEntity, result);
			singletonRW = __query_1056228640_2.GetSingletonRW<SpellSingleton>();
			ref SpellSingleton valueRW = ref singletonRW.ValueRW;
			ProcessSpellPrefabRegister(ref valueRW);
			ProcessSpellEffectRegister(ref valueRW);
			ProcessSpellConfigStructs(ref valueRW);
		}
		singletonRW = __query_1056228640_2.GetSingletonRW<SpellSingleton>();
		ref SpellSingleton valueRW2 = ref singletonRW.ValueRW;
		valueRW2.Harmonious = GameMgr.IsChAge14_Static;
		valueRW2.UnfollowingEffectRequireBufferEntity = __query_1056228640_3.GetSingletonEntity();
		valueRW2.GlobalParticleBufferEntity = __query_1056228640_4.GetSingletonEntity();
		valueRW2.SpellTransparency = DataMgr.settingData.SpellTransparent;
		valueRW2.TeammateTransparency = DataMgr.settingData.SummonTransparent;
	}

	[BurstCompile]
	private void ProcessSpellPrefabRegister(ref SpellSingleton singleton)
	{
		if (!singleton.Prefabs.IsCreated)
		{
			singleton.Prefabs = new NativeHashMap<FixedString128Bytes, Entity>(64, Allocator.Persistent);
		}
		DynamicBuffer<SpellSingletonAuthoring.SpellPrefabBuffer> singletonBuffer = __query_1056228640_5.GetSingletonBuffer<SpellSingletonAuthoring.SpellPrefabBuffer>();
		foreach (SpellSingletonAuthoring.SpellPrefabBuffer item in singletonBuffer)
		{
			SpellSingletonAuthoring.SpellPrefabBuffer current = item;
			singleton.Prefabs.Remove(current.Id);
			singleton.Prefabs[current.Id] = current.Prefab;
		}
		singletonBuffer.Clear();
	}

	[BurstCompile]
	private void ProcessSpellEffectRegister(ref SpellSingleton singleton)
	{
		if (!singleton.Effects.IsCreated)
		{
			singleton.Effects = new NativeHashMap<int, NativeHashMap<FixedString64Bytes, SpellEffect>>(64, Allocator.Persistent);
		}
		DynamicBuffer<SpellSingletonAuthoring.SpellEffectBuffer> singletonBuffer = __query_1056228640_6.GetSingletonBuffer<SpellSingletonAuthoring.SpellEffectBuffer>();
		foreach (SpellSingletonAuthoring.SpellEffectBuffer item in singletonBuffer)
		{
			SpellSingletonAuthoring.SpellEffectBuffer current = item;
			if (!singleton.Effects.ContainsKey(current.SpellId))
			{
				singleton.Effects[current.SpellId] = new NativeHashMap<FixedString64Bytes, SpellEffect>(4, Allocator.Persistent);
			}
			NativeHashMap<FixedString64Bytes, SpellEffect> nativeHashMap = singleton.Effects[current.SpellId];
			nativeHashMap.Remove(current.Effect.Name);
			nativeHashMap[current.Effect.Name] = current.Effect;
		}
		singletonBuffer.Clear();
	}

	private void ProcessSpellConfigStructs(ref SpellSingleton singleton)
	{
		if (!singleton.Configs.IsCreated)
		{
			singleton.Configs = new NativeHashMap<int, SpellConfig_Struct>(256, Allocator.Persistent);
		}
		if (singleton.Configs.Count >= SpellConfig.list.Count)
		{
			return;
		}
		foreach (SpellConfig item in SpellConfig.list)
		{
			singleton.Configs.Remove(item.id);
			singleton.Configs.Add(item.id, SpellConfig_Struct.FromSpellConfigClass(item));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingletonTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSingletonAuthoring.SpellPrefabBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSingletonAuthoring.SpellEffectBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1056228640_6 = entityQueryBuilder2.Build(ref state);
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
	public SpellSingletonSystem()
	{
	}
}
