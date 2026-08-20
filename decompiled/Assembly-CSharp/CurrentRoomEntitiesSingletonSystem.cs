using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
[CompilerGenerated]
internal struct CurrentRoomEntitiesSingletonSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2010037363_0;

	private EntityQuery __query_2010037363_1;

	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingleton<CurrentRoomEntitiesSingleton>();
	}

	public void OnDestroy(ref SystemState state)
	{
		state.Dependency.Complete();
		if (__query_2010037363_0.HasSingleton<CurrentRoomEntitiesSingleton>())
		{
			ref CurrentRoomEntitiesSingleton valueRW = ref __query_2010037363_1.GetSingletonRW<CurrentRoomEntitiesSingleton>().ValueRW;
			if (valueRW.TargetableEntities.IsCreated)
			{
				valueRW.TargetableEntities.Dispose();
			}
			if (valueRW.TargetableTransforms.IsCreated)
			{
				valueRW.TargetableTransforms.Dispose();
			}
			if (valueRW.TargetableUnitProperties.IsCreated)
			{
				valueRW.TargetableUnitProperties.Dispose();
			}
			if (valueRW.TargetablePlayerTeamEntities.IsCreated)
			{
				valueRW.TargetablePlayerTeamEntities.Dispose();
			}
			if (valueRW.TargetablePlayerTeamTransforms.IsCreated)
			{
				valueRW.TargetablePlayerTeamTransforms.Dispose();
			}
			if (valueRW.TargetablePlayerTeamProperties.IsCreated)
			{
				valueRW.TargetablePlayerTeamProperties.Dispose();
			}
		}
	}

	public void OnUpdate(ref SystemState state)
	{
		ref CurrentRoomEntitiesSingleton valueRW = ref __query_2010037363_1.GetSingletonRW<CurrentRoomEntitiesSingleton>().ValueRW;
		if (valueRW.TargetableEntities.IsCreated)
		{
			valueRW.TargetableEntities.Clear();
		}
		else
		{
			valueRW.TargetableEntities = new NativeList<Entity>(0, Allocator.Persistent);
		}
		if (valueRW.TargetableTransforms.IsCreated)
		{
			valueRW.TargetableTransforms.Clear();
		}
		else
		{
			valueRW.TargetableTransforms = new NativeList<LocalTransform>(0, Allocator.Persistent);
		}
		if (valueRW.TargetableUnitProperties.IsCreated)
		{
			valueRW.TargetableUnitProperties.Clear();
		}
		else
		{
			valueRW.TargetableUnitProperties = new NativeList<UnitProperty_Dots>(0, Allocator.Persistent);
		}
		if (valueRW.TargetablePlayerTeamEntities.IsCreated)
		{
			valueRW.TargetablePlayerTeamEntities.Clear();
		}
		else
		{
			valueRW.TargetablePlayerTeamEntities = new NativeList<Entity>(0, Allocator.Persistent);
		}
		if (valueRW.TargetablePlayerTeamTransforms.IsCreated)
		{
			valueRW.TargetablePlayerTeamTransforms.Clear();
		}
		else
		{
			valueRW.TargetablePlayerTeamTransforms = new NativeList<LocalTransform>(0, Allocator.Persistent);
		}
		if (valueRW.TargetablePlayerTeamProperties.IsCreated)
		{
			valueRW.TargetablePlayerTeamProperties.Clear();
		}
		else
		{
			valueRW.TargetablePlayerTeamProperties = new NativeList<UnitProperty_Dots>(0, Allocator.Persistent);
		}
		if (!LevelMgr.Inst || !LevelMgr.Inst.CurrentRoomCtrller)
		{
			return;
		}
		for (int num = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count - 1; num >= 0; num--)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[num];
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, entity))
			{
				ref NativeList<Entity> targetableEntities = ref valueRW.TargetableEntities;
				Entity value = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[num];
				targetableEntities.Add(in value);
			}
			else
			{
				LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.RemoveAt(num);
				Debug.LogError("当前房间有不存在的敌方单位");
			}
		}
		UpdateTransformAndPptArray(ref state, ref valueRW.TargetableEntities, ref valueRW.TargetableTransforms, ref valueRW.TargetableUnitProperties);
		if (PlayerMgr.Inst.PlayerEtt == Entity.Null && !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, PlayerMgr.Inst.PlayerEtt))
		{
			for (int num2 = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.Count - 1; num2 >= 0; num2--)
			{
				Entity entity2 = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[num2];
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity2) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, entity2))
				{
					ref NativeList<Entity> targetablePlayerTeamEntities = ref valueRW.TargetablePlayerTeamEntities;
					Entity value = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[num2];
					targetablePlayerTeamEntities.Add(in value);
				}
				else
				{
					LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.RemoveAt(num2);
					Debug.LogError("当前房间有不存在的我方单位");
				}
			}
		}
		else
		{
			ref NativeList<Entity> targetablePlayerTeamEntities2 = ref valueRW.TargetablePlayerTeamEntities;
			Entity value = PlayerMgr.Inst.PlayerEtt;
			targetablePlayerTeamEntities2.Add(in value);
			for (int num3 = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.Count - 1; num3 >= 0; num3--)
			{
				Entity entity3 = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[num3];
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity3) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, entity3))
				{
					ref NativeList<Entity> targetablePlayerTeamEntities3 = ref valueRW.TargetablePlayerTeamEntities;
					value = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[num3];
					targetablePlayerTeamEntities3.Add(in value);
				}
				else
				{
					LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.RemoveAt(num3);
					Debug.LogError("当前房间有不存在的我方单位");
				}
			}
		}
		UpdateTransformAndPptArray(ref state, ref valueRW.TargetablePlayerTeamEntities, ref valueRW.TargetablePlayerTeamTransforms, ref valueRW.TargetablePlayerTeamProperties);
	}

	[BurstCompile]
	private void UpdateTransformAndPptArray(ref SystemState state, ref NativeList<Entity> units, ref NativeList<LocalTransform> transformArray, ref NativeList<UnitProperty_Dots> unitPropertyArray)
	{
		for (int i = 0; i < units.Length; i++)
		{
			Entity entity = units[i];
			UnitProperty_Dots value = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, entity);
			LocalTransform value2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity);
			transformArray.Add(in value2);
			unitPropertyArray.Add(in value);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2010037363_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2010037363_1 = entityQueryBuilder2.Build(ref state);
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
		((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((CurrentRoomEntitiesSingletonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
