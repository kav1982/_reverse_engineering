using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(Spell4005WandDataInitializeSystem))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
public class Spell4005WandDataDestroyInvalidWandSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell4005WandSpiritData_RW_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_378481501_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4005WandSpiritData>();
		RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer entityCommandBuffer = __query_378481501_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.EntityManager.WorldUnmanaged);
		using EntityQuery entityQuery = base.EntityManager.CreateEntityQuery(typeof(LocalTransform), typeof(Spell4005WandSpiritData), typeof(UnitProperty_Dots));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Entity entity = nativeArray[i];
			if (InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.Wand.Value == null)
			{
				entityCommandBuffer.DestroyEntity(entity);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_378481501_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4005WandDataDestroyInvalidWandSystem()
	{
	}
}
