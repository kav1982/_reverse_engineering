using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Scripting;

[UpdateBefore(typeof(SpellDestroySystem))]
[CompilerGenerated]
[BurstCompile]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
public class Spell1023EndSystem : SystemBase
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<Spell1023JudgementBladeData> __Spell1023JudgementBladeData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell1023JudgementBladeData_RO_ComponentLookup = state.GetComponentLookup<Spell1023JudgementBladeData>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1501648326_0;

	private EntityQuery __query_1501648326_1;

	private EntityQuery __query_1501648326_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<GlobalParticleEmitParams>();
		RequireForUpdate<Spell1023JudgementBladeData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		Spell1023AroundDataSingleton singleton = __query_1501648326_0.GetSingleton<Spell1023AroundDataSingleton>();
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		NativeArray<Entity> nativeArray = GetEntityQuery(typeof(Spell1023JudgementBladeData)).ToEntityArray(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Entity entity = nativeArray[i];
			RefRO<Spell1023JudgementBladeData> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Spell1023JudgementBladeData_RO_ComponentLookup, ref base.CheckedStateRef, entity);
			if (base.EntityManager.IsComponentEnabled<SpellDestroyTag>(entity))
			{
				JudgementBladeState state = componentROAfterCompletingDependency.ValueRO.State;
				if ((uint)state <= 1u || state == JudgementBladeState.DestroyRiseUP)
				{
					BladeFadeOut(entity, isRiseUp: true);
				}
				else
				{
					BladeFadeOut(entity, isRiseUp: false);
				}
			}
		}
		foreach (KVPair<Entity, NativeList<Entity>> datum in singleton.Data)
		{
			if (!datum.Value.IsCreated)
			{
				Entity value = datum.Key;
				nativeList.Add(in value);
				continue;
			}
			for (int j = 0; j < datum.Value.Length; j++)
			{
				if (!base.EntityManager.Exists(datum.Value[j]) || !base.EntityManager.HasComponent<LocalTransform>(datum.Value[j]))
				{
					datum.Value.RemoveAt(j);
				}
			}
			if (!base.EntityManager.Exists(datum.Key) && datum.Value.Length <= 0)
			{
				datum.Value.Dispose();
				Entity value = datum.Key;
				nativeList.Add(in value);
			}
		}
		for (int k = 0; k < nativeList.Length; k++)
		{
			singleton.Data.Remove(nativeList[k]);
		}
	}

	private void BladeFadeOut(Entity spellEntity, bool isRiseUp)
	{
		SpellConfigComponentData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity);
		Entity spellEffectEntity = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity).SpellEffectEntity;
		SpellMovementComponentData componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity);
		RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity);
		ref LocalTransform valueRW = ref componentRWAfterCompletingDependency.ValueRW;
		valueRW.Position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity).Position;
		float3 position = valueRW.Position;
		float3 direction = (isRiseUp ? new float3(0f, 1f, 0f) : componentAfterCompletingDependency2.Direction);
		if (componentAfterCompletingDependency2.IsFallSpell)
		{
			if (isRiseUp)
			{
				valueRW.Position += new float3(0f, 0f - InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity).Position.z, 0f);
			}
			position = valueRW.Position;
		}
		else
		{
			position += new float3(0f, 0.3f, 0f);
		}
		float scale = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, spellEntity).Scale;
		componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, spellEffectEntity);
		componentRWAfterCompletingDependency.ValueRW.Scale = scale;
		math.atan2(direction.y, direction.x);
		if (!isRiseUp && componentAfterCompletingDependency2.IsFallSpell)
		{
			direction = float3.zero;
		}
		base.EntityManager.RemoveComponent<Parent>(spellEffectEntity);
		base.EntityManager.AddComponent<Spell1023FadeOutBladeData>(spellEffectEntity);
		base.EntityManager.SetComponentData(spellEffectEntity, new Spell1023FadeOutBladeData
		{
			Direction = direction,
			Timer = 0f,
			MoveSpeed = __query_1501648326_1.GetSingletonRW<GlobalRandom>().ValueRW.NewRandom().NextFloat(7.5f, 8.75f)
		});
		for (int num = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref base.CheckedStateRef, spellEntity).Length - 1; num >= 0; num--)
		{
			if (InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref base.CheckedStateRef, spellEntity)[num].Value == spellEffectEntity)
			{
				InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref base.CheckedStateRef, spellEntity).RemoveAt(num);
			}
		}
		componentAfterCompletingDependency.ColorType.ColorEnumToString(out var result);
		GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1023_End_{result}", position + new float3(0f, 0.1f, 0f))
		{
			Size = scale,
			Velocity = componentAfterCompletingDependency2.Direction * componentAfterCompletingDependency2.Speed
		};
		__query_1501648326_2.GetSingletonBuffer<GlobalParticleEmitParams>().Add(elem);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1023AroundDataSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648326_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648326_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648326_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell1023EndSystem()
	{
	}
}
