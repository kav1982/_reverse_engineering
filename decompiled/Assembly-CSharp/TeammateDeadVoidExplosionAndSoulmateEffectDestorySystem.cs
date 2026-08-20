using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;

[UpdateInGroup(typeof(SpellEndSystemGroup))]
[UpdateAfter(typeof(TeammateDeadEventSystem))]
[CompilerGenerated]
internal struct TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
		}
	}

	private EntityQuery _deadTeammatesQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1629008050_0;

	private EntityQuery __query_1629008050_1;

	private EntityQuery __query_1629008050_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell3129DamageRequestBuffer>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		_deadTeammatesQuery = state.EntityManager.CreateEntityQuery(typeof(TeammateDeadTag), typeof(TeammateData), typeof(UnitProperty_Dots), typeof(SpellElementEffectComponentData));
	}

	public void OnUpdate(ref SystemState state)
	{
		NativeArray<Entity> nativeArray = _deadTeammatesQuery.ToEntityArray(Allocator.Temp);
		EntityCommandBuffer CMD = __query_1629008050_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		Entity singletonEntity = __query_1629008050_1.GetSingletonEntity();
		Entity singletonEntity2 = __query_1629008050_2.GetSingletonEntity();
		foreach (Entity item in nativeArray)
		{
			UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, item);
			if (componentAfterCompletingDependency.voidExplosionData.HpToDmgRatio > 0f)
			{
				CMD.ApplyVoidExplosion(singletonEntity, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, item).Position, componentAfterCompletingDependency.unitCfg.maxHP, componentAfterCompletingDependency.voidExplosionData, singletonEntity2, 1f);
			}
			LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(item);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008050_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008050_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3129DamageRequestBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008050_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateDeadVoidExplosionAndSoulmateEffectDestorySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
