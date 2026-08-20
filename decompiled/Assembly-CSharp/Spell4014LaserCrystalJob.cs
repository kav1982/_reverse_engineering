using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Spell4014LaserCrystalJob : IJobEntity, IJobChunk
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct RaycastHitDistanceComparer : IComparer<RaycastHit>
	{
		public int Compare(RaycastHit a, RaycastHit b)
		{
			return a.Fraction.CompareTo(b.Fraction);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct RaycastEnenmyHitDistanceComparer : IComparer<(RaycastHit, SpellTools.HitType)>
	{
		public int Compare((RaycastHit, SpellTools.HitType) a, (RaycastHit, SpellTools.HitType) b)
		{
			return a.Item1.Fraction.CompareTo(b.Item1.Fraction);
		}
	}

	private struct CrystalReadonlyData
	{
		public SpellSpecialMovementType moveType;

		public float length;

		public UnitType selfType;

		public Entity crystalEntity;

		public float normalRotateSpeed;

		public float chaseEnemyRotateSpeed;

		public float3 laserStartPos;

		public float stepLength;

		public float3 ownerDirection;

		public float3 ownerPosition;

		public bool isRefract;

		public int refractCount;

		public float3 mousePosition;

		public float chaseMouseLerpSpeed;

		public float rotateRadius;

		public float3 rotateCenter;

		public bool isFall;

		public float fallLockRadius;

		public float fallHitRadius;

		public float3 targetPosInRange;
	}

	private struct CrystalEditableData
	{
		public float3 stepDirection;

		public float3 stepPosition;

		public float3 stepVelocity;

		public float remainDistance;

		public int remainPenetrate;

		public int remainRefractCount;

		public DynamicBuffer<SpellRefractionHitEntities> refractHitteds;

		public NativeList<Entity> hitList;

		public NativeList<float3> hitDataList;

		public float3 mouseLerpFloat3;

		public float3 tempTargetPosition;

		public bool isIgnoreWallRelic;
	}

	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell4014LaserCrystalData> __Spell4014LaserCrystalData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<MultiShootData> __MultiShootData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<ManaCostRatio> __ManaCostRatio_RO_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public BufferTypeHandle<CrystalLaserPoint> __CrystalLaserPoint_RW_BufferTypeHandle;

			public BufferTypeHandle<HittedEntity> __HittedEntity_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Spell4014LaserCrystalData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				__MultiShootData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MultiShootData>(isReadOnly: true);
				__ManaCostRatio_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ManaCostRatio>(isReadOnly: true);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__CrystalLaserPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<CrystalLaserPoint>();
				__HittedEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<HittedEntity>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell4014LaserCrystalData_RW_ComponentTypeHandle.Update(ref state);
				__MultiShootData_RO_ComponentTypeHandle.Update(ref state);
				__ManaCostRatio_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__CrystalLaserPoint_RW_BufferTypeHandle.Update(ref state);
				__HittedEntity_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<MultiShootData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<ManaCostRatio>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CrystalLaserPoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HittedEntity>();
			DefaultQuery = entityQueryBuilder2.Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		public void Init(ref SystemState state, bool assignDefaultQuery)
		{
			if (assignDefaultQuery)
			{
				__AssignQueries(ref state);
			}
			__TypeHandle.__AssignHandles(ref state);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Run(ref Spell4014LaserCrystalJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4014LaserCrystalJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4014LaserCrystalJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4014LaserCrystalJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4014LaserCrystalJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4014LaserCrystalJob job, EntityManager entityManager)
		{
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InternalCompiler
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		public static void CheckForErrors(int scheduleType)
		{
		}
	}

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellRefractionData> SpellRefractionLookup;

	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellSplitComponentData> SpellSplitLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractionHitEntitiesLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[ReadOnly]
	public PlayerController_Dots PlayerCtrller;

	public float DeltaTime;

	public bool IsPause;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public bool IsIgnoreWallRelic;

	public GlobalRandom Random;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(Entity entity, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData componentData, ref Spell4014LaserCrystalData crystalData, in MultiShootData multiShootData, in ManaCostRatio manaCostRatio, ref LocalTransform transform, ref SpellElementEffectComponentData elementEffect, DynamicBuffer<CrystalLaserPoint> crystalPoints, DynamicBuffer<HittedEntity> laserHitteds, [ChunkIndexInQuery] int chunkIndex)
	{
		if (!crystalData.IsInitialized)
		{
			crystalData.IsInitialized = true;
			GetRadius(in config, 1f, out crystalData.LaserWidthBase);
			crystalData.LaserWidthBase *= 0.1f;
			if (componentData.IsSplitSpell)
			{
				crystalData.LaserWidthBase *= 0.6f;
			}
			if (SpellSplitLookup.HasComponent(entity))
			{
				crystalData.SplitCount = SpellSplitLookup.GetRefRO(entity).ValueRO.Count;
			}
			crystalData.ThunderStoneCd = Random.random.NextFloat(0.5f);
		}
		if (crystalData.IsSplitCenter)
		{
			return;
		}
		if (IsPause)
		{
			crystalData.IsHit = false;
			return;
		}
		crystalPoints.Clear();
		SetCrystalTempData(in crystalData, in transform, in componentData, in movement, in entity, in config, out var cReadData, out var cTempData);
		crystalPoints.Add(new CrystalLaserPoint
		{
			Position = cReadData.laserStartPos
		});
		LaserLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, out var isHitUnit);
		if (crystalData.ForceCooling)
		{
			if (crystalData.CurrentWandMana / crystalData.CurrentWandMaxMana >= 0.05f)
			{
				crystalData.ForceCooling = false;
			}
			crystalData.IsHit = false;
			crystalData.ManaCostThisFrame = 0f;
			crystalData.CurrentDmgLoopTime = 0f;
		}
		else
		{
			crystalData.CurrentDmgLoopTime += DeltaTime;
			crystalData.IsHit = isHitUnit;
			crystalData.IsHitFrame = false;
			crystalData.ManaCostThisFrame = (isHitUnit ? (config.Float1 * crystalData.KeepHittingRate * config.DamageInterval * manaCostRatio.ratio * crystalData.MpCostRatio) : 0f);
		}
		if (!(crystalData.CurrentDmgLoopTime >= config.DamageInterval))
		{
			return;
		}
		crystalData.CurrentDmgLoopTime -= config.DamageInterval;
		crystalData.IsHitFrame = true;
		if (isHitUnit)
		{
			for (int i = 0; i < cTempData.hitList.Length; i++)
			{
				HittedEntity hittedEntity = default(HittedEntity);
				hittedEntity.Entity = cTempData.hitList[i];
				hittedEntity.Direction = cTempData.hitDataList[i];
				HittedEntity elem = hittedEntity;
				laserHitteds.Add(elem);
			}
		}
	}

	private void LaserLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, out bool isHitUnit)
	{
		isHitUnit = false;
		switch (cReadData.moveType)
		{
		case SpellSpecialMovementType.Normal:
			LaserNormalLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, ref isHitUnit);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			LaserChaseEnemyLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, out isHitUnit);
			break;
		case SpellSpecialMovementType.ChaseMouse:
			LaserChaseMouseLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, out isHitUnit);
			break;
		case SpellSpecialMovementType.ChaseOwner:
			LaserChaseOwnerLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, out isHitUnit);
			break;
		case SpellSpecialMovementType.Rotation:
			LaserRotationLineUpdate(ref crystalData, in cReadData, ref cTempData, ref crystalPoints, out isHitUnit);
			break;
		}
	}

	private void LaserNormalLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, ref bool isHitUnit)
	{
		CrystalLaserPoint crystalLaserPoint;
		if (cReadData.isFall)
		{
			crystalPoints.Insert(crystalPoints.Length, new CrystalLaserPoint
			{
				Position = cReadData.targetPosInRange
			});
			crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
			isHitUnit = EnemyCheckInRadius(in cReadData, ref cTempData, in crystalLaserPoint.Position, in cReadData.fallHitRadius);
			crystalData.TouchGroundPos = crystalPoints[crystalPoints.Length - 1].Position;
			return;
		}
		crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
		bool chaseEnemyPositionMinAngle = GetChaseEnemyPositionMinAngle(in crystalLaserPoint.Position, in cReadData.ownerDirection, in cReadData.selfType, out cTempData.tempTargetPosition);
		cTempData.tempTargetPosition += new float3(0f, 0f, -0.4f);
		if (chaseEnemyPositionMinAngle)
		{
			NormalChaseLineUpdate(ref crystalPoints, in cReadData, ref cTempData, ref isHitUnit);
		}
		else
		{
			StraightGoLineUpdate(in cReadData, ref cTempData, ref crystalPoints, ref isHitUnit);
		}
	}

	private void GetGroundPositionInputInCircle(float3 center, float radius, float3 inputPos, out float3 posOnGround)
	{
		float2 x = inputPos.xy - center.xy;
		if (math.length(x) < radius)
		{
			posOnGround = inputPos;
		}
		else
		{
			posOnGround = new float3(math.normalizesafe(x), 0f) * radius + center;
		}
	}

	private void LaserChaseEnemyLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, out bool isHitUnit)
	{
		isHitUnit = false;
		if (cReadData.isFall)
		{
			if (!crystalData.HaveTarget)
			{
				crystalData.TouchGroundPos = new float3(crystalPoints[crystalPoints.Length - 1].Position.xy, 0f);
				crystalPoints.Insert(crystalPoints.Length, new CrystalLaserPoint
				{
					Position = crystalData.TouchGroundPos
				});
			}
			else
			{
				GetGroundPositionInputInCircle(new float3(crystalData.OrbitCenter.xy, 0f), cReadData.fallLockRadius, crystalData.TargetPosition, out var posOnGround);
				crystalPoints.Insert(crystalPoints.Length, new CrystalLaserPoint
				{
					Position = posOnGround
				});
			}
			CrystalLaserPoint crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
			isHitUnit = EnemyCheckInRadius(in cReadData, ref cTempData, in crystalLaserPoint.Position, in cReadData.fallHitRadius);
			crystalData.TouchGroundPos = crystalPoints[crystalPoints.Length - 1].Position;
		}
		else
		{
			CrystalLaserPoint crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
			if (GetChaseEnemyPositionMinAngle(in crystalLaserPoint.Position, in cReadData.ownerDirection, in cReadData.selfType, out var targetPosition))
			{
				targetPosition += new float3(0f, 0f, -0.4f);
				EnemyChaseLineUpdate(targetPosition, ref crystalPoints, in cReadData, ref cTempData, ref isHitUnit);
			}
			else
			{
				StraightGoLineUpdate(in cReadData, ref cTempData, ref crystalPoints, ref isHitUnit);
			}
		}
	}

	private void LaserChaseMouseLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, out bool isHitUnit)
	{
		isHitUnit = false;
		if (cReadData.isFall)
		{
			GetGroundPositionInputInCircle(new float3(crystalData.OrbitCenter.xy, 0f), cReadData.fallLockRadius, PlayerCtrller.mousePosition, out var posOnGround);
			crystalPoints.Insert(crystalPoints.Length, new CrystalLaserPoint
			{
				Position = posOnGround
			});
			CrystalLaserPoint crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
			isHitUnit = EnemyCheckInRadius(in cReadData, ref cTempData, in crystalLaserPoint.Position, in cReadData.fallHitRadius);
			crystalData.TouchGroundPos = crystalPoints[crystalPoints.Length - 1].Position;
			return;
		}
		int num = 400;
		while (cTempData.remainDistance > 0.01f)
		{
			num--;
			if (num > 0)
			{
				float3 end = math.normalizesafe(new float3(cReadData.mousePosition.xy - crystalPoints[crystalPoints.Length - 1].Position.xy, 0f));
				float t = math.clamp(cReadData.chaseMouseLerpSpeed, 0f, 1f);
				cTempData.mouseLerpFloat3 = math.lerp(cTempData.mouseLerpFloat3, end, t);
				cTempData.stepVelocity = cTempData.mouseLerpFloat3 * cReadData.stepLength;
				cTempData.stepDirection = math.normalizesafe(cTempData.stepVelocity);
				if (math.length(cTempData.stepVelocity) > cTempData.remainDistance)
				{
					cTempData.stepVelocity = cTempData.stepDirection * cTempData.remainDistance;
				}
				cTempData.stepPosition = crystalPoints[crystalPoints.Length - 1].Position + cTempData.stepVelocity;
				if (!GetHitDataAndRefract(in cReadData, ref cTempData, ref crystalPoints, out var isResetDirection, ref isHitUnit))
				{
					if (isResetDirection)
					{
						cTempData.mouseLerpFloat3 = cTempData.stepDirection * 1.5f;
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	private void LaserChaseOwnerLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, out bool isHitUnit)
	{
		isHitUnit = false;
		if (cReadData.isFall)
		{
			int length = crystalPoints.Length;
			CrystalLaserPoint elem = default(CrystalLaserPoint);
			elem.Position = new float3(crystalPoints[crystalPoints.Length - 1].Position.xy, 0f);
			crystalPoints.Insert(length, elem);
			elem = crystalPoints[crystalPoints.Length - 1];
			isHitUnit = EnemyCheckInRadius(in cReadData, ref cTempData, in elem.Position, in cReadData.fallHitRadius);
			crystalData.TouchGroundPos = crystalPoints[crystalPoints.Length - 1].Position;
			return;
		}
		float3 target = math.normalizesafe(new float3(cReadData.ownerPosition.xy - crystalPoints[crystalPoints.Length - 1].Position.xy, 0f));
		float rotationDirection = GetRotationDirection(cReadData.ownerDirection.xy, target.xy);
		int num = 400;
		while (cTempData.remainDistance > 0.01f)
		{
			num--;
			if (num > 0)
			{
				DirMoveTowardsCounterClockwise(in cTempData.stepDirection, in target, cReadData.chaseEnemyRotateSpeed * cReadData.stepLength, rotationDirection, out cTempData.stepDirection);
				ref readonly float stepLength = ref cReadData.stepLength;
				CrystalLaserPoint elem = crystalPoints[crystalPoints.Length - 1];
				StepGoDataSet(in stepLength, ref cTempData, in elem.Position);
				if (!GetHitDataAndRefract(in cReadData, ref cTempData, ref crystalPoints, out var isResetDirection, ref isHitUnit))
				{
					target = math.normalizesafe(new float3(cReadData.ownerPosition.xy - crystalPoints[crystalPoints.Length - 1].Position.xy, 0f));
					if (isResetDirection)
					{
						rotationDirection = GetRotationDirection(cTempData.stepDirection.xy, target.xy);
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	private void LaserRotationLineUpdate(ref Spell4014LaserCrystalData crystalData, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, out bool isHitUnit)
	{
		isHitUnit = false;
		if (cReadData.isFall)
		{
			int length = crystalPoints.Length;
			CrystalLaserPoint elem = default(CrystalLaserPoint);
			elem.Position = new float3(crystalPoints[crystalPoints.Length - 1].Position.xy, 0f);
			crystalPoints.Insert(length, elem);
			elem = crystalPoints[crystalPoints.Length - 1];
			isHitUnit = EnemyCheckInRadius(in cReadData, ref cTempData, in elem.Position, in cReadData.fallHitRadius);
			crystalData.TouchGroundPos = crystalPoints[crystalPoints.Length - 1].Position;
			return;
		}
		int valueToClamp = (int)(MathF.PI * 2f / (2f / cReadData.rotateRadius));
		valueToClamp = math.clamp(valueToClamp, 20, 40);
		float degree = 360f / (float)valueToClamp;
		for (int i = 0; i < valueToClamp; i++)
		{
			float2 xy = crystalPoints[crystalPoints.Length - 1].Position.xy - cReadData.rotateCenter.xy;
			cTempData.stepPosition = DTool.RotateDir(new float3(xy, 0f), degree);
			cTempData.stepPosition += cReadData.rotateCenter;
			cTempData.stepVelocity = cTempData.stepPosition - crystalPoints[crystalPoints.Length - 1].Position;
			CrystalLaserPoint elem = crystalPoints[crystalPoints.Length - 1];
			bool lineEnd = CheckHitWallStep(ref cTempData, in elem.Position);
			elem = crystalPoints[crystalPoints.Length - 1];
			CheckHitEnemyByStepVelocity(in cReadData, ref cTempData, in elem.Position, ref lineEnd, out var _, ref isHitUnit);
			crystalPoints.Insert(crystalPoints.Length, new CrystalLaserPoint
			{
				Position = cTempData.stepPosition
			});
			if (lineEnd)
			{
				break;
			}
		}
	}

	private void NormalChaseLineUpdate(ref DynamicBuffer<CrystalLaserPoint> crystalPoints, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref bool isHitUnit)
	{
		isHitUnit = false;
		float3 target = math.normalizesafe(cTempData.tempTargetPosition - crystalPoints[crystalPoints.Length - 1].Position);
		float rotationDirection = GetRotationDirection(cReadData.ownerDirection.xy, target.xy);
		int num = 400;
		while (cTempData.remainDistance > 0.01f)
		{
			num--;
			if (num > 0)
			{
				DirMoveTowardsCounterClockwise(in cTempData.stepDirection, in target, cReadData.normalRotateSpeed, rotationDirection, out cTempData.stepDirection);
				ref readonly float stepLength = ref cReadData.stepLength;
				CrystalLaserPoint crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
				StepGoDataSet(in stepLength, ref cTempData, in crystalLaserPoint.Position);
				if (!GetHitDataAndRefract(in cReadData, ref cTempData, ref crystalPoints, out var isResetDirection, ref isHitUnit))
				{
					target = new float3((cTempData.tempTargetPosition - crystalPoints[crystalPoints.Length - 1].Position).xy, 0f);
					if (isResetDirection)
					{
						rotationDirection = GetRotationDirection(cTempData.stepDirection.xy, target.xy);
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	private void EnemyChaseLineUpdate(float3 targetPos, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref bool isHitUnit)
	{
		isHitUnit = false;
		float3 target = math.normalizesafe(new float3((targetPos - crystalPoints[crystalPoints.Length - 1].Position).xy, 0f));
		float rotationDirection = GetRotationDirection(cReadData.ownerDirection.xy, target.xy);
		int num = 400;
		while (cTempData.remainDistance > 0.01f)
		{
			num--;
			if (num > 0)
			{
				DirMoveTowardsCounterClockwise(in cTempData.stepDirection, in target, cReadData.chaseEnemyRotateSpeed, rotationDirection, out cTempData.stepDirection);
				ref readonly float stepLength = ref cReadData.stepLength;
				CrystalLaserPoint crystalLaserPoint = crystalPoints[crystalPoints.Length - 1];
				StepGoDataSet(in stepLength, ref cTempData, in crystalLaserPoint.Position);
				if (!GetHitDataAndRefract(in cReadData, ref cTempData, ref crystalPoints, out var isResetDirection, ref isHitUnit))
				{
					target = math.normalizesafe(new float3((targetPos - crystalPoints[crystalPoints.Length - 1].Position).xy, 0f));
					if (isResetDirection)
					{
						rotationDirection = GetRotationDirection(cTempData.stepDirection.xy, target.xy);
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	private void StraightGoLineUpdate(in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> crystalPoints, ref bool isHitUnit)
	{
		isHitUnit = false;
		int num = 400;
		while (cTempData.remainDistance > 0.01f)
		{
			num--;
			if (num > 0)
			{
				cTempData.stepVelocity = cReadData.ownerDirection * cTempData.remainDistance;
				cTempData.stepPosition = crystalPoints[crystalPoints.Length - 1].Position + cTempData.stepVelocity;
				if (GetHitDataAndRefract(in cReadData, ref cTempData, ref crystalPoints, out var _, ref isHitUnit))
				{
					break;
				}
				continue;
			}
			break;
		}
	}

	[BurstCompile]
	private void StepGoDataSet(in float stepLength, ref CrystalEditableData cTempData, in float3 orgPosition)
	{
		float num = ((cTempData.remainDistance > stepLength) ? stepLength : cTempData.remainDistance);
		cTempData.stepVelocity = cTempData.stepDirection * num;
		cTempData.stepPosition = orgPosition + cTempData.stepVelocity;
	}

	[BurstCompile]
	private bool GetHitDataAndRefract(in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, ref DynamicBuffer<CrystalLaserPoint> rayBodyPoints, out bool isResetDirection, ref bool isHitUnit)
	{
		bool lineEnd = false;
		CrystalLaserPoint crystalLaserPoint = rayBodyPoints[rayBodyPoints.Length - 1];
		isResetDirection = CheckWallHitAndEnemyHitStep(in cReadData, ref cTempData, in crystalLaserPoint.Position, ref lineEnd, ref isHitUnit);
		rayBodyPoints.Insert(rayBodyPoints.Length, new CrystalLaserPoint
		{
			Position = cTempData.stepPosition
		});
		if (lineEnd)
		{
			return true;
		}
		cTempData.remainDistance -= math.length(cTempData.stepVelocity);
		if (isResetDirection)
		{
			float remainDistance = cTempData.remainDistance;
			if (remainDistance < 0.2f && remainDistance > 0f)
			{
				cTempData.remainDistance += 0.2f;
			}
		}
		return false;
	}

	[BurstCompile]
	private bool CheckWallHitAndEnemyHitStep(in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, in float3 prevPos, ref bool lineEnd, ref bool isHitUnit)
	{
		lineEnd = CheckHitWallStep(ref cTempData, in prevPos);
		CheckHitEnemyByStepVelocity(in cReadData, ref cTempData, in prevPos, ref lineEnd, out var isRefracted, ref isHitUnit);
		return isRefracted;
	}

	[BurstCompile]
	private bool CheckHitWallStep(ref CrystalEditableData cTempData, in float3 fromPos)
	{
		if (IsIgnoreWallRelic)
		{
			return false;
		}
		return CheckHitWallByStepVelocity(in fromPos, ref cTempData.stepPosition, ref cTempData.stepVelocity);
	}

	[BurstCompile]
	private bool CheckHitWallByStepVelocity(in float3 previousPos, ref float3 normalStepPos, ref float3 stepVelocity)
	{
		if (TryRaycstWallHit(previousPos, normalStepPos, out var wallHit, in PhysicsWorld))
		{
			stepVelocity *= wallHit.Fraction;
			normalStepPos = previousPos + stepVelocity;
			return true;
		}
		return false;
	}

	[BurstCompile]
	private bool TryRaycstWallHit(float3 startOnLine, float3 endOnLine, out RaycastHit wallHit, in PhysicsWorldSingleton physicsWorld)
	{
		wallHit = default(RaycastHit);
		return RaycastWallHits(startOnLine, endOnLine, out wallHit, in physicsWorld);
	}

	[BurstCompile]
	private bool RaycastWallHits(float3 start, float3 end, out RaycastHit hitResult, in PhysicsWorldSingleton physicsWorld)
	{
		hitResult = default(RaycastHit);
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = start;
		raycastInput.End = end;
		raycastInput.Filter = new CollisionFilter
		{
			BelongsTo = 16777216u,
			CollidesWith = 256u,
			GroupIndex = 0
		};
		RaycastInput input = raycastInput;
		NativeList<RaycastHit> allHits = new NativeList<RaycastHit>(Allocator.Temp);
		if (physicsWorld.CastRay(input, ref allHits) && allHits.Length > 0)
		{
			allHits.Sort(default(RaycastHitDistanceComparer));
			for (int i = 0; i < allHits.Length; i++)
			{
				if (allHits[i].Fraction > 0.0001f)
				{
					hitResult = allHits[i];
					return true;
				}
			}
		}
		return false;
	}

	[BurstCompile]
	private void CheckHitEnemyByStepVelocity(in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, in float3 previousPos, ref bool lineEnd, out bool isRefracted, ref bool isHitUnit)
	{
		isRefracted = false;
		if (!RayCastEnemyHit(in previousPos, in cTempData.stepPosition, cReadData.selfType, out var hitList))
		{
			return;
		}
		hitList.Sort<(RaycastHit, SpellTools.HitType), RaycastEnenmyHitDistanceComparer>(default(RaycastEnenmyHitDistanceComparer));
		for (int i = 0; i < hitList.Length; i++)
		{
			if (hitList[i].Item1.Fraction <= 0.001f)
			{
				continue;
			}
			if (!cTempData.hitList.Contains(hitList[i].Item1.Entity))
			{
				ref NativeList<Entity> hitList2 = ref cTempData.hitList;
				Entity value = hitList[i].Item1.Entity;
				hitList2.Add(in value);
				cTempData.hitDataList.Add(in cTempData.stepDirection);
			}
			if (hitList[i].Item2 == SpellTools.HitType.Brittleness || hitList[i].Item2 == SpellTools.HitType.Butterfly)
			{
				continue;
			}
			if (hitList[i].Item2 == SpellTools.HitType.Unit)
			{
				isHitUnit = true;
			}
			if (cReadData.isRefract && cTempData.remainRefractCount > 0)
			{
				if (hitList[i].Item2 == SpellTools.HitType.Unit)
				{
					ref float3 stepDirection = ref cTempData.stepDirection;
					ref float3 tempTargetPosition = ref cTempData.tempTargetPosition;
					float3 hitPosition = hitList[i].Item1.Position;
					ref readonly UnitType selfType = ref cReadData.selfType;
					DynamicBuffer<SpellRefractionHitEntities> refractHitteds = cTempData.refractHitteds;
					Entity value = hitList[i].Item1.Entity;
					if (RefractionDirectionSet(ref stepDirection, ref tempTargetPosition, in hitPosition, in selfType, refractHitteds, in value))
					{
						isRefracted = true;
						float fraction = hitList[i].Item1.Fraction;
						cTempData.stepVelocity *= fraction;
						cTempData.stepPosition = previousPos + cTempData.stepVelocity;
						cTempData.remainRefractCount--;
						lineEnd = false;
						break;
					}
				}
			}
			else
			{
				if (cTempData.remainPenetrate <= 0)
				{
					float fraction2 = hitList[i].Item1.Fraction;
					cTempData.stepVelocity *= fraction2;
					cTempData.stepPosition = previousPos + cTempData.stepVelocity;
					lineEnd = true;
					break;
				}
				cTempData.remainPenetrate--;
				lineEnd = false;
			}
		}
	}

	[BurstCompile]
	private bool RefractionDirectionSet(ref float3 stepDirection, ref float3 targetPosition, in float3 hitPosition, in UnitType unitType, DynamicBuffer<SpellRefractionHitEntities> hitEntities, in Entity currentHitEntity)
	{
		NativeList<Entity> currentHitEntities = new NativeList<Entity>(1, Allocator.Temp) { in currentHitEntity };
		if (!RefractionTransformGet(hitEntities, in currentHitEntities, in hitPosition, in unitType, out var targetPos))
		{
			return false;
		}
		targetPosition = new float3(targetPos.xy, -0.4f);
		float3 input = math.normalizesafe(targetPosition - hitPosition);
		float3 target = input.IgnoreZ();
		float num = DTool.GetDegree(target) - DTool.GetDegree(stepDirection);
		if (num > 180f)
		{
			num = 360f - num;
		}
		if (num < -180f)
		{
			num = 360f + num;
		}
		stepDirection = DTool.DirMoveTowardsIgnoreZ(in stepDirection, in target, math.abs(num * 0.7f));
		return true;
	}

	[BurstCompile]
	private bool RayCastEnemyHit(in float3 startOnLine, in float3 endOnLine, UnitType shooterType, out NativeList<(RaycastHit, SpellTools.HitType)> hitList)
	{
		hitList = default(NativeList<(RaycastHit, SpellTools.HitType)>);
		if (SpellTools.GetAttackablesInRaycast(in startOnLine, in endOnLine, in shooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, out var hitList2))
		{
			hitList = new NativeList<(RaycastHit, SpellTools.HitType)>(Allocator.Temp);
			foreach (RaycastHit item in hitList2)
			{
				Entity target = item.Entity;
				SpellTools.HitType entityHitType = SpellTools.GetEntityHitType(in target, in shooterType, in UnitPropertyLookup, in SpellConfigLookup);
				if ((uint)(entityHitType - 3) <= 3u)
				{
					(RaycastHit, SpellTools.HitType) value = (item, entityHitType);
					hitList.Add(in value);
				}
			}
			return true;
		}
		return false;
	}

	[BurstCompile]
	private bool HaveRefractionCheck(Entity entity, out int count)
	{
		count = 0;
		if (SpellRefractionLookup.HasComponent(entity))
		{
			count = SpellRefractionLookup.GetRefRO(entity).ValueRO.RemainCount;
			return true;
		}
		return false;
	}

	[BurstCompile]
	private bool GetChaseEnemyPositionMinAngle(in float3 startPos, in float3 direction, in UnitType shooterType, out float3 targetPosition)
	{
		Entity target;
		UnitProperty_Dots targetPpt;
		return CurrentRoomEntities.FindMinAngleTarget(startPos, direction, shooterType, out target, out targetPosition, out targetPpt);
	}

	[BurstCompile]
	private bool RefractionTransformGet(DynamicBuffer<SpellRefractionHitEntities> hitEntities, in NativeList<Entity> currentHitEntities, in float3 hitPos, in UnitType selfType, out float3 targetPos)
	{
		targetPos = float3.zero;
		if (currentHitEntities.Length == 0)
		{
			return false;
		}
		float3 startPoint = DTool.IgnoreZPosition(in hitPos);
		foreach (Entity currentHitEntity in currentHitEntities)
		{
			hitEntities.Add(new SpellRefractionHitEntities
			{
				Entity = currentHitEntity
			});
		}
		NativeHashSet<Entity> ignoreEntities = new NativeHashSet<Entity>(hitEntities.Length, Allocator.Temp);
		foreach (SpellRefractionHitEntities item in hitEntities)
		{
			ignoreEntities.Add(item.Entity);
		}
		Entity target;
		UnitProperty_Dots targetPpt;
		bool flag = CurrentRoomEntities.FindReflectionTarget(startPoint, selfType, in ignoreEntities, out target, out targetPos, out targetPpt);
		if (!flag)
		{
			hitEntities.Clear();
			foreach (Entity currentHitEntity2 in currentHitEntities)
			{
				hitEntities.Add(new SpellRefractionHitEntities
				{
					Entity = currentHitEntity2
				});
			}
			ignoreEntities.Clear();
			foreach (Entity currentHitEntity3 in currentHitEntities)
			{
				ignoreEntities.Add(currentHitEntity3);
			}
			flag = CurrentRoomEntities.FindReflectionTarget(startPoint, selfType, in ignoreEntities, out target, out targetPos, out targetPpt);
		}
		return flag;
	}

	[BurstCompile]
	private float GetRotationDirection(float2 from, float2 to)
	{
		return from.x * to.y - from.y * to.x;
	}

	[BurstCompile]
	private void DirMoveTowardsClockV2(in float2 source, in float2 target, float maxDeltaAngleDeg, bool clockwise, out float2 result)
	{
		float2 x = math.normalizesafe(source);
		float2 @float = math.normalizesafe(target);
		float x2 = math.clamp(math.dot(x, @float), -1f, 1f);
		float num = math.atan2(x.x * @float.y - x.y * @float.x, x2);
		float num2 = math.abs(num);
		if (num2 < math.radians(90f))
		{
			if (clockwise && num > 0f)
			{
				result = @float;
				return;
			}
			if (!clockwise && num < 0f)
			{
				result = @float;
				return;
			}
		}
		float y = math.radians(maxDeltaAngleDeg);
		float x3 = math.min(num2, y);
		float num3 = math.cos(x3);
		float num4 = math.sin(x3);
		if (clockwise)
		{
			result = new float2(x.x * num3 + x.y * num4, (0f - x.x) * num4 + x.y * num3);
		}
		else
		{
			result = new float2(x.x * num3 - x.y * num4, x.x * num4 + x.y * num3);
		}
		result = math.normalize(result);
	}

	private void DirMoveTowardsCounterClockwise(in float3 source, in float3 target, float maxDeltaAngleDeg, float counterClock, out float3 result)
	{
		float2 source2 = source.xy;
		float2 target2 = target.xy;
		DirMoveTowardsClockV2(in source2, in target2, maxDeltaAngleDeg, counterClock < 0f, out var result2);
		result = new float3(result2, 0f);
	}

	private bool EnemyCheckInRadius(in CrystalReadonlyData cReadData, ref CrystalEditableData cTempData, in float3 center, in float radius)
	{
		SpellTools.GetAttackableEntitiesInRange(in center, in radius, in cReadData.selfType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref cTempData.hitList);
		bool result = false;
		NativeList<int> nativeList = new NativeList<int>(Allocator.Temp);
		for (int i = 0; i < cTempData.hitList.Length; i++)
		{
			if (!TransformLookup.TryGetComponent(cTempData.hitList[i], out var componentData))
			{
				nativeList.Add(in i);
				continue;
			}
			Entity target = cTempData.hitList[i];
			switch (SpellTools.GetEntityHitType(in target, in cReadData.selfType, in UnitPropertyLookup, in SpellConfigLookup))
			{
			case SpellTools.HitType.Unknown:
			case SpellTools.HitType.SameCamp:
			case SpellTools.HitType.IgnoreSpell:
				nativeList.Add(in i);
				continue;
			case SpellTools.HitType.Unit:
			case SpellTools.HitType.RollBall:
			case SpellTools.HitType.Butterfly:
				result = true;
				break;
			}
			ref NativeList<float3> hitDataList = ref cTempData.hitDataList;
			float3 value = math.normalizesafe(componentData.Position - center);
			hitDataList.Add(in value);
		}
		for (int num = nativeList.Length - 1; num >= 0; num--)
		{
			cTempData.hitList.RemoveAt(nativeList[num]);
		}
		return result;
	}

	[BurstCompile]
	private void GetRadius(in SpellConfigComponentData config, float baseR, out float rResult)
	{
		RadiusAttributeValue radius = config.Radius;
		radius.Base = 1.2f;
		rResult = radius.CalculateIgnoreFall();
	}

	private void SetCrystalTempData(in Spell4014LaserCrystalData crystalData, in LocalTransform transform, in SpellComponentData componentData, in SpellMovementComponentData movement, in Entity entity, in SpellConfigComponentData config, out CrystalReadonlyData cReadData, out CrystalEditableData cTempData)
	{
		cReadData = default(CrystalReadonlyData);
		cTempData = default(CrystalEditableData);
		cReadData.length = 7f + movement.Speed;
		cReadData.moveType = movement.Type;
		cReadData.normalRotateSpeed = 1f * (1f + math.min(3f, crystalData.KeepHittingRate * 0.8f));
		cReadData.chaseEnemyRotateSpeed = movement.ChaseRotateSpeed / 2f;
		cReadData.selfType = config.ShooterType;
		cReadData.stepLength = 0.5f;
		cReadData.laserStartPos = transform.Position;
		cReadData.ownerDirection = crystalData.OwnerDirection;
		cReadData.ownerPosition = movement.UpdateSelfChasePosition(TransformLookup, componentData.OwnerEntity);
		cReadData.isRefract = HaveRefractionCheck(entity, out cReadData.refractCount);
		cReadData.mousePosition = PlayerCtrller.mousePosition;
		cReadData.chaseMouseLerpSpeed = movement.ChaseMouseLerpSpeed;
		cReadData.rotateRadius = movement.AroundRadius;
		cReadData.rotateCenter = new float3(cReadData.ownerPosition.xy, -0.3f);
		if (componentData.IsSplitSpell && TransformLookup.TryGetComponent(componentData.Shooter, out var componentData2))
		{
			cReadData.rotateCenter = componentData2.Position;
		}
		cReadData.isFall = movement.IsFallSpell;
		cReadData.fallLockRadius = FallLockRadiusCauculate(in config);
		cReadData.fallHitRadius = config.Radius.Calculate();
		GetGroundPositionInputInCircle(new float3(cReadData.ownerPosition.xy, 0f), cReadData.fallLockRadius, cReadData.mousePosition, out cReadData.targetPosInRange);
		cTempData.stepPosition = transform.Position;
		cTempData.stepDirection = crystalData.OwnerDirection;
		cTempData.remainDistance = cReadData.length;
		cTempData.remainPenetrate = config.Penetrate.Calculate();
		cTempData.remainRefractCount = cReadData.refractCount;
		if (cReadData.isRefract)
		{
			cTempData.refractHitteds = SpellRefractionHitEntitiesLookup[entity];
			cTempData.refractHitteds.Clear();
		}
		cTempData.hitList = new NativeList<Entity>(Allocator.Temp);
		cTempData.hitDataList = new NativeList<float3>(Allocator.Temp);
		cTempData.mouseLerpFloat3 = crystalData.OwnerDirection;
	}

	private float FallLockRadiusCauculate(in SpellConfigComponentData config)
	{
		RadiusAttributeValue radius = config.Radius;
		radius.Base = 5.5f;
		return radius.CalculateIgnoreFall();
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4014LaserCrystalData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__MultiShootData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__ManaCostRatio_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		BufferAccessor<CrystalLaserPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__CrystalLaserPoint_RW_BufferTypeHandle);
		BufferAccessor<HittedEntity> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__HittedEntity_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i);
				ref Spell4014LaserCrystalData crystalData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, i);
				ref MultiShootData multiShootData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, i);
				ref ManaCostRatio manaCostRatio = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, i);
				DynamicBuffer<CrystalLaserPoint> crystalPoints = bufferAccessor[i];
				DynamicBuffer<HittedEntity> laserHitteds = bufferAccessor2[i];
				Execute(entity, ref config, ref movement, ref componentData, ref crystalData, in multiShootData, in manaCostRatio, ref transform, ref elementEffect, crystalPoints, laserHitteds, chunkIndexInQuery);
				num++;
			}
			return;
		}
		if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
		{
			int nextRangeBegin = 0;
			int nextRangeEnd = 0;
			while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, nextRangeEnd, out nextRangeBegin, out nextRangeEnd))
			{
				while (nextRangeBegin < nextRangeEnd)
				{
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref Spell4014LaserCrystalData crystalData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, nextRangeBegin);
					ref MultiShootData multiShootData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, nextRangeBegin);
					ref ManaCostRatio manaCostRatio2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, nextRangeBegin);
					DynamicBuffer<CrystalLaserPoint> crystalPoints2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<HittedEntity> laserHitteds2 = bufferAccessor2[nextRangeBegin];
					Execute(entity2, ref config2, ref movement2, ref componentData2, ref crystalData2, in multiShootData2, in manaCostRatio2, ref transform2, ref elementEffect2, crystalPoints2, laserHitteds2, chunkIndexInQuery);
					nextRangeBegin++;
					num++;
				}
			}
			return;
		}
		ulong num2 = chunkEnabledMask.ULong0;
		int num3 = math.min(64, count);
		for (int j = 0; j < num3; j++)
		{
			if ((num2 & 1) != 0L)
			{
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j);
				ref Spell4014LaserCrystalData crystalData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, j);
				ref MultiShootData multiShootData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, j);
				ref ManaCostRatio manaCostRatio3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, j);
				DynamicBuffer<CrystalLaserPoint> crystalPoints3 = bufferAccessor[j];
				DynamicBuffer<HittedEntity> laserHitteds3 = bufferAccessor2[j];
				Execute(entity3, ref config3, ref movement3, ref componentData3, ref crystalData3, in multiShootData3, in manaCostRatio3, ref transform3, ref elementEffect3, crystalPoints3, laserHitteds3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k);
				ref Spell4014LaserCrystalData crystalData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, k);
				ref MultiShootData multiShootData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, k);
				ref ManaCostRatio manaCostRatio4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, k);
				DynamicBuffer<CrystalLaserPoint> crystalPoints4 = bufferAccessor[k];
				DynamicBuffer<HittedEntity> laserHitteds4 = bufferAccessor2[k];
				Execute(entity4, ref config4, ref movement4, ref componentData4, ref crystalData4, in multiShootData4, in manaCostRatio4, ref transform4, ref elementEffect4, crystalPoints4, laserHitteds4, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
	}

	private JobHandle __ThrowCodeGenException()
	{
		throw new Exception("This method should have been replaced by source gen.");
	}

	public void Run()
	{
		__ThrowCodeGenException();
	}

	public void RunByRef()
	{
		__ThrowCodeGenException();
	}

	public void Run(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void RunByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public JobHandle Schedule(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleByRef(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public void Schedule()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleByRef()
	{
		__ThrowCodeGenException();
	}

	public void Schedule(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void ScheduleByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
	{
		return __ThrowCodeGenException();
	}

	public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
	{
		return __ThrowCodeGenException();
	}

	public void ScheduleParallel()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallelByRef()
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallel(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	public void ScheduleParallelByRef(EntityQuery query)
	{
		__ThrowCodeGenException();
	}

	void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
	}
}
