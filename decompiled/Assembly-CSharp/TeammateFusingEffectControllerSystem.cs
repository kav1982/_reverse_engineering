using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateAfter(typeof(TeammateFuseSystem))]
[CompilerGenerated]
public class TeammateFusingEffectControllerSystem : SystemBase
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RW_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__TeammateDeadTag_RW_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>();
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
		}
	}

	private static TeammateFusingEffectControllerSystem _inst;

	private readonly List<FuseEffectSyncData> _syncData = new List<FuseEffectSyncData>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1702763338_0;

	private EntityQuery __query_1702763338_1;

	private EntityQuery __query_1702763338_2;

	[Preserve]
	protected override void OnCreate()
	{
		_inst = this;
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (TeammateFusePairBuffer item in __query_1702763338_0.GetSingletonBuffer<TeammateFusePairBuffer>())
		{
			ProcessNewFuseEffect(item);
		}
		__query_1702763338_0.GetSingletonBuffer<TeammateFusePairBuffer>().Clear();
		for (int num = _syncData.Count - 1; num >= 0; num--)
		{
			FuseEffectSyncData value = _syncData[num];
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, value.SubEntity) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, value.MainEntity) || value.FuseParticleController1.IsDestroyed() || value.FuseParticleController2.IsDestroyed())
			{
				_syncData.RemoveAt(num);
			}
			else
			{
				value.FuseParticleController1.UpdateFuseParticleEffect(InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, value.MainEntity).ValueRW.Position);
				value.FuseParticleController2.UpdateFuseParticleEffect(InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, value.SubEntity).ValueRW.Position);
				value.FuseTimer += base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
				if (value.FuseTimer >= 1f)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RW_ComponentLookup, ref base.CheckedStateRef, value.MainEntity, value: true);
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RW_ComponentLookup, ref base.CheckedStateRef, value.SubEntity, value: true);
					ObjPoolMgr.Inst.GetGO("Prefabs/Spell/3115/3115_SummonDone", value.FusePosition, 2f);
					SpellSpawnParams elem = __query_1702763338_1.GetSingleton<SpellSingleton>().SpellSpawnParamsStorage[value.MainEntity].BuildFuseTeammate(value.MainEntity, value.FusePosition, value.FuseData.FuseMainTeammateData, value.FuseData.FuseSubTeammateData);
					RefRW<TeammateData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, value.MainEntity);
					RefRW<TeammateData> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref base.CheckedStateRef, value.SubEntity);
					int num2 = componentRWAfterCompletingDependency.ValueRW.TeammateCurrentFuseLevel + 1;
					int num3 = componentRWAfterCompletingDependency2.ValueRW.TeammateCurrentFuseLevel + 1;
					elem.TeammateComponentData.TeammateHpRatio.MulRatio *= (float)(num2 + num3) / (float)num2;
					SpellComponentData componentData = base.EntityManager.GetComponentData<SpellComponentData>(value.MainEntity);
					if (componentData.SubGroupEntity != Entity.Null)
					{
						SpellSubGroupComponentData componentObject = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(componentData.SubGroupEntity);
						if (componentObject != null && componentObject.SubGroup != null)
						{
							elem.SubGroupEntity = base.EntityManager.CreateEntity(typeof(SpellSubGroupComponentData));
							base.EntityManager.AddComponentObject(elem.SubGroupEntity, new SpellSubGroupComponentData
							{
								SubGroup = componentObject.SubGroup
							});
						}
					}
					elem.MovementComponentData.AroundTarget = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref base.CheckedStateRef, value.MainEntity).AroundTarget;
					elem.IsFuseTeammate = true;
					if (componentRWAfterCompletingDependency.ValueRO.TeammateType == TeammateType.teammate6)
					{
						elem.ConfigComponentData.Int1 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref base.CheckedStateRef, value.SubEntity).Int1;
					}
					__query_1702763338_2.GetSingletonBuffer<SpellSpawnParams>().Add(elem);
					_inst._syncData.RemoveAt(num);
				}
				else
				{
					_syncData[num] = value;
				}
			}
		}
	}

	private void ProcessNewFuseEffect(TeammateFusePairBuffer buffer)
	{
		Vector3 vector = base.EntityManager.GetComponentData<LocalTransform>(buffer.FuseMainTeammateEntity).Position;
		Vector3 vector2 = base.EntityManager.GetComponentData<LocalTransform>(buffer.FuseSubTeammateEntity).Position;
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(vector + (vector2 - vector) / 2f + Tool2D.IgnoreZPoint(Random.insideUnitSphere) * Random.Range(0f, Vector3.Distance(vector2, vector) / 5f));
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/3115/3115_ForceCenter", navMeshPointIngoreZ, 2f);
		Spell3115ForceController component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/3115/3115_PullingForce", vector2).GetComponent<Spell3115ForceController>();
		component.Initialize(vector, navMeshPointIngoreZ);
		Spell3115ForceController component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/3115/3115_PullingForce", vector2).GetComponent<Spell3115ForceController>();
		component2.Initialize(vector2, navMeshPointIngoreZ);
		_inst._syncData.Add(new FuseEffectSyncData
		{
			MainEntity = buffer.FuseMainTeammateEntity,
			SubEntity = buffer.FuseSubTeammateEntity,
			FuseParticleController1 = component,
			FuseParticleController2 = component2,
			FusePosition = navMeshPointIngoreZ,
			FuseData = buffer,
			FuseTimer = 0f
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateFusePairBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1702763338_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1702763338_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1702763338_2 = entityQueryBuilder2.Build(ref state);
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
	public TeammateFusingEffectControllerSystem()
	{
	}
}
