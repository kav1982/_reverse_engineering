using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(Spell1023JudgementBladeSystem))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
public class Spell1023CheckBladeAndOwnerStateSystem : SystemBase
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

	private EntityQuery __query_1501648527_0;

	[Preserve]
	protected override void OnUpdate()
	{
		Spell1023AroundDataSingleton singleton = __query_1501648527_0.GetSingleton<Spell1023AroundDataSingleton>();
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		foreach (KVPair<Entity, NativeList<Entity>> datum in singleton.Data)
		{
			if (!datum.Value.IsCreated)
			{
				Entity value = datum.Key;
				nativeList.Add(in value);
				continue;
			}
			for (int i = 0; i < datum.Value.Length; i++)
			{
				if (!base.EntityManager.Exists(datum.Value[i]) || !base.EntityManager.HasComponent<LocalTransform>(datum.Value[i]))
				{
					datum.Value.RemoveAt(i);
				}
			}
			if (!base.EntityManager.Exists(datum.Key))
			{
				datum.Value.Dispose();
				Entity value = datum.Key;
				nativeList.Add(in value);
			}
		}
		for (int j = 0; j < nativeList.Length; j++)
		{
			singleton.Data.Remove(nativeList[j]);
		}
		nativeList.Dispose();
		nativeList = new NativeList<Entity>(Allocator.Temp);
		foreach (KVPair<Entity, Spell1023OwnerData> bladeDetectTargetDatum in singleton.BladeDetectTargetData)
		{
			if (!base.EntityManager.Exists(bladeDetectTargetDatum.Key) || !base.EntityManager.HasComponent<LocalTransform>(bladeDetectTargetDatum.Key))
			{
				Entity value = bladeDetectTargetDatum.Key;
				nativeList.Add(in value);
			}
		}
		for (int k = 0; k < nativeList.Length; k++)
		{
			singleton.BladeDetectTargetData.Remove(nativeList[k]);
		}
		nativeList.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1023AroundDataSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648527_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1023CheckBladeAndOwnerStateSystem()
	{
	}
}
