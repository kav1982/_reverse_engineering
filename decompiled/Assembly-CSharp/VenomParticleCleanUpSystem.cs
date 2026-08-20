using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

[UpdateAfter(typeof(VenomSystem))]
[UpdateInGroup(typeof(LiquidSystemGroup))]
[CompilerGenerated]
public class VenomParticleCleanUpSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct TypeHandle
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
		}
	}

	private EntityQuery particleQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1658031133_0;

	[Preserve]
	protected override void OnCreate()
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		entityQueryBuilder = entityQueryBuilder.WithAll<Venom_DotsParticle>();
		entityQueryBuilder = entityQueryBuilder.WithNone<Venom_Dots>();
		particleQuery = entityQueryBuilder.Build(base.EntityManager);
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (particleQuery.IsEmpty)
		{
			return;
		}
		NativeArray<Venom_DotsParticle> nativeArray = particleQuery.ToComponentDataArray<Venom_DotsParticle>(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			if (nativeArray[i].particle.IsValid())
			{
				ObjPoolMgr.Inst.RecycleGO(nativeArray[i].particle.Value.gameObject);
			}
		}
		nativeArray.Dispose();
		EntityCommandBuffer entityCommandBuffer = __query_1658031133_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		NativeArray<Entity> entities = particleQuery.ToEntityArray(Allocator.Temp);
		entityCommandBuffer.RemoveComponent<Venom_DotsParticle>(entities);
		entities.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658031133_0 = entityQueryBuilder2.Build(ref state);
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
	public VenomParticleCleanUpSystem()
	{
	}
}
