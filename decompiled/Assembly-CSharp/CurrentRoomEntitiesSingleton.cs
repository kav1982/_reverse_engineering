using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[Serializable]
[BurstCompile]
public struct CurrentRoomEntitiesSingleton : IComponentData, IQueryTypeParameter
{
	[NativeDisableParallelForRestriction]
	[NativeDisableUnsafePtrRestriction]
	public NativeList<Entity> TargetableEntities;

	[NativeDisableUnsafePtrRestriction]
	[NativeDisableParallelForRestriction]
	public NativeList<LocalTransform> TargetableTransforms;

	[NativeDisableParallelForRestriction]
	[NativeDisableUnsafePtrRestriction]
	public NativeList<UnitProperty_Dots> TargetableUnitProperties;

	[NativeDisableUnsafePtrRestriction]
	[NativeDisableParallelForRestriction]
	public NativeList<Entity> TargetablePlayerTeamEntities;

	[NativeDisableParallelForRestriction]
	[NativeDisableUnsafePtrRestriction]
	public NativeList<LocalTransform> TargetablePlayerTeamTransforms;

	[NativeDisableParallelForRestriction]
	[NativeDisableUnsafePtrRestriction]
	public NativeList<UnitProperty_Dots> TargetablePlayerTeamProperties;

	[BurstCompile]
	public readonly bool FindMinAngleTarget(float3 startPos, float3 direction, UnitType selfUnitType, out Entity target, out float3 targetPosition, out UnitProperty_Dots targetPpt)
	{
		NativeList<Entity> nativeList = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableEntities : TargetablePlayerTeamEntities);
		NativeList<UnitProperty_Dots> nativeList2 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableUnitProperties : TargetablePlayerTeamProperties);
		NativeList<LocalTransform> nativeList3 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableTransforms : TargetablePlayerTeamTransforms);
		NativeArray<UnitProperty_Dots> UnitsArray = nativeList2.AsArray();
		NativeArray<LocalTransform> UnitsTransformArray = nativeList3.AsArray();
		int minAngleTargetIndex = DTool.GetMinAngleTargetIndex(in UnitsArray, in UnitsTransformArray, in startPos, in direction, selfUnitType);
		if (minAngleTargetIndex < 0)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		target = nativeList[minAngleTargetIndex];
		targetPosition = nativeList3[minAngleTargetIndex].Position;
		targetPpt = nativeList2[minAngleTargetIndex];
		return true;
	}

	[BurstCompile]
	public readonly bool FindNearestTarget(float3 checkPoint, UnitType selfUnitType, out Entity target, out float3 targetPosition, out UnitProperty_Dots targetPpt)
	{
		NativeList<Entity> nativeList = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableEntities : TargetablePlayerTeamEntities);
		NativeList<UnitProperty_Dots> nativeList2 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableUnitProperties : TargetablePlayerTeamProperties);
		NativeList<LocalTransform> nativeList3 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableTransforms : TargetablePlayerTeamTransforms);
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < nativeList.Length; i++)
		{
			UnitType unitType = nativeList2[i].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(selfUnitType, unitType) && nativeList2[i].CanBeTarget)
			{
				float num3 = math.distancesq(nativeList3[i].Position, checkPoint);
				if (!(num3 > num2))
				{
					num2 = num3;
					num = i;
				}
			}
		}
		if (num < 0)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		target = nativeList[num];
		targetPosition = nativeList3[num].Position;
		targetPpt = nativeList2[num];
		return true;
	}

	[BurstCompile]
	public readonly bool FindNearestNonFullHpTarget(float3 checkPoint, UnitType selfUnitType, Entity entity, out Entity target, out float3 targetPosition, out UnitProperty_Dots targetPpt)
	{
		NativeList<Entity> nativeList = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableEntities : TargetablePlayerTeamEntities);
		NativeList<UnitProperty_Dots> nativeList2 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableUnitProperties : TargetablePlayerTeamProperties);
		NativeList<LocalTransform> nativeList3 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableTransforms : TargetablePlayerTeamTransforms);
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < nativeList.Length; i++)
		{
			UnitType unitType = nativeList2[i].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(selfUnitType, unitType) && nativeList2[i].CanBeTarget && !(nativeList2[i].unitCfg.currentHP >= nativeList2[i].unitCfg.maxHP) && !entity.Equals(nativeList[i]))
			{
				float num3 = math.distancesq(nativeList3[i].Position, checkPoint);
				if (!(num3 > num2))
				{
					num2 = num3;
					num = i;
				}
			}
		}
		if (num < 0)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		target = nativeList[num];
		targetPosition = nativeList3[num].Position;
		targetPpt = nativeList2[num];
		return true;
	}

	[BurstCompile]
	public readonly bool FindRandomTarget(GlobalRandom Random, UnitType selfUnitType, out Entity target, out float3 targetPosition, out UnitProperty_Dots targetPpt)
	{
		NativeList<Entity> nativeList = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableEntities : TargetablePlayerTeamEntities);
		NativeList<UnitProperty_Dots> nativeList2 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableUnitProperties : TargetablePlayerTeamProperties);
		NativeList<LocalTransform> nativeList3 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableTransforms : TargetablePlayerTeamTransforms);
		if (nativeList.Length == 0)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		int num = -1;
		int num2 = 0;
		while (num2 < nativeList.Length)
		{
			num2++;
			int num3 = Random.random.NextInt(nativeList.Length);
			UnitType unitType = nativeList2[num3].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(selfUnitType, unitType) && nativeList2[num3].CanBeTarget)
			{
				num = num3;
				break;
			}
		}
		if (num == -1)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		target = nativeList[num];
		targetPosition = nativeList3[num].Position;
		targetPpt = nativeList2[num];
		return true;
	}

	[BurstCompile]
	public readonly void FindValidTargetsInRange(float3 checkPoint, float range, UnitType selfUnitType, out NativeList<Entity> target, out NativeList<float3> targetPosition, out NativeList<UnitProperty_Dots> targetPpt, Allocator allocator = Allocator.Temp)
	{
		NativeList<Entity> nativeList = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableEntities : TargetablePlayerTeamEntities);
		NativeList<UnitProperty_Dots> nativeList2 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableUnitProperties : TargetablePlayerTeamProperties);
		NativeList<LocalTransform> nativeList3 = (DTool.IsSameCamp(selfUnitType, UnitType.Player) ? TargetableTransforms : TargetablePlayerTeamTransforms);
		target = new NativeList<Entity>(allocator);
		targetPosition = new NativeList<float3>(allocator);
		targetPpt = new NativeList<UnitProperty_Dots>(allocator);
		for (int i = 0; i < nativeList.Length; i++)
		{
			UnitType unitType = nativeList2[i].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(selfUnitType, unitType) && nativeList2[i].CanBeTarget && !(math.distancesq(nativeList3[i].Position, checkPoint) > range * range))
			{
				Entity value = nativeList[i];
				target.Add(in value);
				UnitProperty_Dots value2 = nativeList2[i];
				targetPpt.Add(in value2);
				LocalTransform localTransform = nativeList3[i];
				targetPosition.Add(in localTransform.Position);
			}
		}
	}

	[BurstCompile]
	public readonly bool FindReflectionTarget(float3 startPoint, UnitType selfUnitType, in NativeHashSet<Entity> ignoreEntities, out Entity target, out float3 targetPosition, out UnitProperty_Dots targetPpt)
	{
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < TargetableEntities.Length; i++)
		{
			if (ignoreEntities.Contains(TargetableEntities[i]))
			{
				continue;
			}
			UnitType unitType = TargetableUnitProperties[i].unitCfg.unitType;
			if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(selfUnitType, unitType) && TargetableUnitProperties[i].CanBeTarget)
			{
				float num3 = math.distancesq(TargetableTransforms[i].Position, startPoint);
				if (!(num3 > num2))
				{
					num2 = num3;
					num = i;
				}
			}
		}
		if (num < 0)
		{
			target = Entity.Null;
			targetPosition = float3.zero;
			targetPpt = default(UnitProperty_Dots);
			return false;
		}
		target = TargetableEntities[num];
		targetPosition = TargetableTransforms[num].Position;
		targetPpt = TargetableUnitProperties[num];
		return true;
	}
}
