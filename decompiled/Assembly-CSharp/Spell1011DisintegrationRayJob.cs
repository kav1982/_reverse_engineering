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
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
public struct Spell1011DisintegrationRayJob : IJobEntity, IJobChunk
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct ColliderHitDistanceComparer : IComparer<(ColliderCastHit, SpellTools.HitType)>
	{
		public int Compare((ColliderCastHit, SpellTools.HitType) a, (ColliderCastHit, SpellTools.HitType) b)
		{
			return a.Item1.Fraction.CompareTo(b.Item1.Fraction);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct RaycastHitDistanceComparer : IComparer<Unity.Physics.RaycastHit>
	{
		public int Compare(Unity.Physics.RaycastHit a, Unity.Physics.RaycastHit b)
		{
			return a.Fraction.CompareTo(b.Fraction);
		}
	}

	private struct SpellTempData
	{
		public SpellSpecialMovementType moveType;

		public bool isFall;

		public float3 initPosition;

		public float3 aroundCenter;

		public float3 stepVelocity;

		public float3 stepDirection;

		public bool isSplit;

		public float3 orgDirection;

		public float3 stepPosition;

		public float spellSpeed;

		public float rotateRadius;

		public float aroundAngle;

		public float chaseEnemyRotateSpeed;

		public float chaseMouseLerpSpeed;

		public float fallSpeed;

		public float width;

		public float remainDistance;

		public float3 chaseMouseLerpData;

		public float radius;

		public float3 chaseShooterPos;

		public int rebounceTime;

		public bool isIgnoreWall;
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

			public ComponentTypeHandle<Spell1011DisintegrationRayData> __Spell1011DisintegrationRayData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public BufferTypeHandle<DisintegrationRayBodyPoint> __DisintegrationRayBodyPoint_RW_BufferTypeHandle;

			public BufferTypeHandle<DisintegrationRayMousePoint> __DisintegrationRayMousePoint_RW_BufferTypeHandle;

			public BufferTypeHandle<DisintegrationRayHoverFallGroundPoint> __DisintegrationRayHoverFallGroundPoint_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Spell1011DisintegrationRayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1011DisintegrationRayData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__DisintegrationRayBodyPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<DisintegrationRayBodyPoint>();
				__DisintegrationRayMousePoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<DisintegrationRayMousePoint>();
				__DisintegrationRayHoverFallGroundPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<DisintegrationRayHoverFallGroundPoint>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell1011DisintegrationRayData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__DisintegrationRayBodyPoint_RW_BufferTypeHandle.Update(ref state);
				__DisintegrationRayMousePoint_RW_BufferTypeHandle.Update(ref state);
				__DisintegrationRayHoverFallGroundPoint_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1011DisintegrationRayData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<DisintegrationRayBodyPoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<DisintegrationRayMousePoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<DisintegrationRayHoverFallGroundPoint>();
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
		public void Run(ref Spell1011DisintegrationRayJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1011DisintegrationRayJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1011DisintegrationRayJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1011DisintegrationRayJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1011DisintegrationRayJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1011DisintegrationRayJob job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public PlayerController_Dots PlayerCtrller;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[ReadOnly]
	public ComponentLookup<SpellRefractionData> SpellRefractionLookup;

	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractionHitEntitiesLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellKeepCastingAttach> KeepCastingAttachLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public float DeltaTime;

	public GlobalRandom Random;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public Entity VfxEntity;

	public Entity SEPlayerSingleton;

	public Entity ScreenShakeEntity;

	public Entity GlobalParticleSystemBufferEntity;

	public DynamicOptimizeData OptimizeData;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(Entity entity, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData componentData, ref Spell1011DisintegrationRayData rayData, ref LocalTransform transform, ref SpellElementEffectComponentData elementEffect, ref PhysicsVelocity physicsVelocity, DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, DynamicBuffer<DisintegrationRayMousePoint> mouseRandomPoints, DynamicBuffer<DisintegrationRayHoverFallGroundPoint> fallHoverGroundPoints, [ChunkIndexInQuery] int chunkIndex)
	{
		NativeList<Entity> hitEtts = new NativeList<Entity>(Allocator.Temp);
		NativeList<float3> hitDataList = new NativeList<float3>(Allocator.Temp);
		NativeList<float4> hitEffectNode = new NativeList<float4>(Allocator.Temp);
		SpellTempData sTD = default(SpellTempData);
		if (!rayData.IsInitialize)
		{
			rayData.IsInitialize = true;
			InitData(ref mouseRandomPoints, ref rayData, ref movement, in config, ref physicsVelocity, ref transform);
			if (componentData.IsSplitSpell)
			{
				SplitInitData(ref movement);
			}
		}
		if (!rayData.IsInHover && config.HoverDuration > 0f && config.DurationTimer > config.Duration.Calculate())
		{
			rayData.IsInHover = true;
		}
		if (rayData.IsInHover)
		{
			if (!movement.IsFallSpell)
			{
				CheckHoverLineHit(rayBodyPoints, GetNonFallRayDamageWidth(transform.Scale), config.ShooterType, ref hitEtts, ref hitDataList);
			}
		}
		else
		{
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				float3 dir = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f));
				transform.Position = new float3((movement.AroundCenter + dir * movement.AroundRadius).xy, -0.3f);
				if (movement.IsFallSpell)
				{
					transform.Position.z = -5f;
				}
			}
			SetSpellTempData(ref sTD, ref movement, in componentData, in config, in transform.Position, in transform.Scale, in rayData);
			int count;
			bool isRefract = HaveRefractionCheck(entity, out count);
			LineDataRefresh(rayBodyPoints, ref sTD, in isRefract, in count, in config.ShooterType, entity, in movement.ChaseTarget, out hitEtts, out hitDataList, out hitEffectNode, out var touchGroundPoss);
			fallHoverGroundPoints.Clear();
			for (int i = 0; i < touchGroundPoss.Length; i++)
			{
				fallHoverGroundPoints.Add(new DisintegrationRayHoverFallGroundPoint
				{
					Value = touchGroundPoss[i]
				});
			}
		}
		rayData.CurrentDmgLoopTime += DeltaTime;
		if (rayData.CurrentDmgLoopTime >= config.DamageInterval)
		{
			rayData.CurrentDmgLoopTime -= config.DamageInterval;
			HitDamageApply(entity, in hitEtts, in hitDataList, in componentData, in config, in movement, in transform, in elementEffect, in OptimizeData, chunkIndex);
			RayHitGlobalEffect(in hitEtts, in config, chunkIndex);
			foreach (DisintegrationRayHoverFallGroundPoint item in fallHoverGroundPoints)
			{
				DisintegrationRayHoverFallGroundPoint current = item;
				TouchGroundExplosion(entity, in config, in transform, in componentData, in movement, in elementEffect, in current.Value, chunkIndex);
			}
		}
		CMD.AppendToBuffer(chunkIndex, ScreenShakeEntity, new ScreenShakeData
		{
			Radius = 0.01f,
			Speed = 2f,
			Time = 0.1f
		});
		HitNodeEffectApply(in hitEffectNode, ref rayData, in config, chunkIndex);
		if (config.DurationTimer > config.Duration.Calculate() + config.HoverDuration)
		{
			rayData.IsHideLine = true;
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			SetBubbleVFX(in rayBodyPoints, in config, chunkIndex);
			transform.Position = rayBodyPoints[rayBodyPoints.Length - 1].Value;
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			FixedString32Bytes seName = "End";
			cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1011, in seName)));
		}
	}

	[BurstCompile]
	private void SetBubbleVFX(in DynamicBuffer<DisintegrationRayBodyPoint> bodyPoints, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 1; i < bodyPoints.Length; i++)
		{
			DisintegrationRayBodyPoint disintegrationRayBodyPoint = bodyPoints[i];
			float3 start = DTool.GetLayerPosition(in disintegrationRayBodyPoint.Value, LayerCorrectType.Coordinate);
			disintegrationRayBodyPoint = bodyPoints[i - 1];
			float3 end = DTool.GetLayerPosition(in disintegrationRayBodyPoint.Value, LayerCorrectType.Coordinate);
			start += bodyPoints[i].Value;
			end += bodyPoints[i - 1].Value;
			int num = (int)(math.distance(start, end) * 10f);
			for (int j = 0; j < num; j++)
			{
				float3 xyz = DTool.Lerp(in start, in end, (float)j / (float)(num - 1));
				float4 posAndColorType = new float4(xyz, (float)config.ColorType);
				CMD.AppendToBuffer(chunkIndex, VfxEntity, new Spell1011RayTrailBED
				{
					posAndColorType = posAndColorType
				});
			}
		}
	}

	[BurstCompile]
	private void RayHitGlobalEffect(in NativeList<Entity> hitEtts, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		foreach (Entity hitEtt in hitEtts)
		{
			config.ColorType.ColorEnumToString(out var result);
			float3 rootPosition = TransformLookup[hitEtt].Position;
			rootPosition.z = -0.3f;
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1011_Hit_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = rootPosition + layerPosition;
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}
	}

	[BurstCompile]
	private void InitData(ref DynamicBuffer<DisintegrationRayMousePoint> mousePoints, ref Spell1011DisintegrationRayData rayData, ref SpellMovementComponentData movement, in SpellConfigComponentData config, ref PhysicsVelocity physicsVelocity, ref LocalTransform transform)
	{
		rayData.SpellSpeed = movement.Speed;
		rayData.SpellFallSpeed = movement.CurrentFallSpeed;
		movement.CurrentFallSpeed = 0f;
		movement.Speed = 0f;
		movement.OriginalSpellHorizontalSpeed = 0f;
		physicsVelocity.Linear = 0f;
		rayData.IsHideLine = false;
		if (movement.Type == SpellSpecialMovementType.ChaseMouse && movement.IsFallSpell)
		{
			float max = math.clamp(config.Scatter * 0.06f, 0.5f, float.PositiveInfinity);
			transform.Position.z = -4f;
			for (int i = 0; i < movement.ReboundCount + 1; i++)
			{
				float2 @float = new float2(Random.random.NextFloat2Direction() * Random.random.NextFloat(0.5f, max)) + PlayerCtrller.mousePosition.xy;
				float2 float2 = ((i != 0) ? (@float - mousePoints[0].Value) : (@float - transform.Position.xy));
				mousePoints.Insert(0, new DisintegrationRayMousePoint
				{
					Value = @float,
					V = float2 * movement.Speed
				});
			}
		}
	}

	private void SplitInitData(ref SpellMovementComponentData movement)
	{
		if (movement.IsFallSpell)
		{
			movement.Gravity = 0f;
			movement.CurrentFallSpeed = 0f;
		}
	}

	[BurstCompile]
	private void LineDataRefresh(DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, ref SpellTempData sTemp, in bool isRefract, in int refractRemain, in UnitType unitType, Entity entity, in Entity chaseEnemyEntity, out NativeList<Entity> hitList, out NativeList<float3> hitDataList, out NativeList<float4> hitEffectNode, out NativeList<float3> touchGroundPoss)
	{
		rayBodyPoints.Clear();
		hitList = new NativeList<Entity>(Allocator.Temp);
		hitDataList = new NativeList<float3>(Allocator.Temp);
		hitEffectNode = new NativeList<float4>(Allocator.Temp);
		touchGroundPoss = new NativeList<float3>(Allocator.Temp);
		rayBodyPoints.Insert(0, new DisintegrationRayBodyPoint
		{
			Value = sTemp.initPosition
		});
		float num = sTemp.spellSpeed * 0.05f;
		DynamicBuffer<SpellRefractionHitEntities> refractHitteds = default(DynamicBuffer<SpellRefractionHitEntities>);
		if (isRefract)
		{
			refractHitteds = SpellRefractionHitEntitiesLookup[entity];
			refractHitteds.Clear();
		}
		int refractRemain2 = refractRemain;
		int num2 = 0;
		switch (sTemp.moveType)
		{
		case SpellSpecialMovementType.Normal:
			if (sTemp.isFall)
			{
				NormalFallLineGenerate(ref sTemp, ref touchGroundPoss, ref hitEffectNode, ref rayBodyPoints, in isRefract, in refractRemain, ref refractHitteds, unitType, entity);
			}
			else
			{
				NormalStraightGo(ref rayBodyPoints, ref sTemp, in unitType, entity, in isRefract, in refractRemain, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode);
			}
			break;
		case SpellSpecialMovementType.Rotation:
		{
			float stepAngle = sTemp.aroundAngle;
			if (sTemp.isFall)
			{
				if (!sTemp.isSplit)
				{
					int num9 = 30;
					for (int num10 = 1; num10 < num9; num10++)
					{
						stepAngle += 5f;
						float3 dir = DTool.GetDir(stepAngle * (MathF.PI / 180f));
						float z = sTemp.initPosition.z - sTemp.initPosition.z / (float)num9 * (float)num10;
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = new float3((dir * sTemp.rotateRadius + sTemp.aroundCenter).xy, z)
						});
					}
					stepAngle += 5f;
					float3 dir2 = DTool.GetDir(stepAngle * (MathF.PI / 180f));
					rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
					{
						Value = new float3((dir2 * sTemp.rotateRadius + sTemp.aroundCenter).xy, 0f)
					});
				}
				DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
				touchGroundPoss.Add(in disintegrationRayBodyPoint.Value);
				float4 value2 = new float4(rayBodyPoints[rayBodyPoints.Length - 1].Value, 0f);
				hitEffectNode.Add(in value2);
				NativeList<float> heightList4 = new NativeList<float>(Allocator.Temp);
				GetFallHeightList(ref heightList4, 11, 2.5f);
				RotateFallRefractionCheck(ref stepAngle, in isRefract, ref refractRemain2, refractHitteds, in unitType, ref rayBodyPoints, ref sTemp, ref hitEffectNode, ref touchGroundPoss, in heightList4);
				for (int num11 = 0; num11 < sTemp.rebounceTime; num11++)
				{
					for (int num12 = 1; num12 < heightList4.Length; num12++)
					{
						stepAngle += 5f;
						float3 dir3 = DTool.GetDir(stepAngle * (MathF.PI / 180f));
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = new float3((dir3 * sTemp.rotateRadius + sTemp.aroundCenter).xy, 0f - heightList4[num12])
						});
					}
					disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
					touchGroundPoss.Add(in disintegrationRayBodyPoint.Value);
					value2 = new float4(rayBodyPoints[rayBodyPoints.Length - 1].Value, 0f);
					hitEffectNode.Add(in value2);
					RotateFallRefractionCheck(ref stepAngle, in isRefract, ref refractRemain2, refractHitteds, in unitType, ref rayBodyPoints, ref sTemp, ref hitEffectNode, ref touchGroundPoss, in heightList4);
				}
			}
			else
			{
				rayBodyPoints.ElementAt(0).Value = DTool.GetDir(stepAngle * (MathF.PI / 180f)) * sTemp.rotateRadius + sTemp.aroundCenter + new float3(0f, 0f, -0.3f);
				for (int num13 = 1; num13 < 37; num13++)
				{
					rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
					{
						Value = DTool.GetDir(((float)num13 * 10f + stepAngle) * (MathF.PI / 180f)) * sTemp.rotateRadius + sTemp.aroundCenter + new float3(0f, 0f, -0.3f)
					});
					sTemp.stepPosition = rayBodyPoints[rayBodyPoints.Length - 1].Value;
					bool lineEnd = false;
					DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 2];
					ref float3 value3 = ref disintegrationRayBodyPoint.Value;
					bool isRefract2 = false;
					CheckHitEnemyByStepVelocity(ref sTemp, in value3, in unitType, in entity, ref hitList, ref hitDataList, ref lineEnd, in isRefract2, ref refractRemain2, ref refractHitteds, out var _, out var _);
				}
				sTemp.remainDistance = 0f;
			}
			break;
		}
		case SpellSpecialMovementType.ChaseEnemy:
		{
			float3 targetPosition;
			bool chaseEnemyPositionMinAngle = GetChaseEnemyPositionMinAngle(in sTemp.initPosition, in sTemp.orgDirection, in unitType, out targetPosition);
			if (sTemp.isFall)
			{
				if (!chaseEnemyPositionMinAngle)
				{
					NormalFallLineGenerate(ref sTemp, ref touchGroundPoss, ref hitEffectNode, ref rayBodyPoints, in isRefract, in refractRemain, ref refractHitteds, unitType, entity);
					break;
				}
				float num3 = sTemp.chaseEnemyRotateSpeed * 0.15f;
				float3 from2 = sTemp.initPosition;
				sTemp.stepDirection = DTool.IgnoreZDir(in targetPosition, in from2);
				sTemp.initPosition = from2;
				float3 v2 = new float3(from2.xy, from2.z + 3f);
				float3 zero2 = float3.zero;
				float3 @float = sTemp.stepDirection * num3;
				float3 value = targetPosition;
				bool isEndOnTarget = true;
				if (!sTemp.isSplit)
				{
					if (num3 < math.length(targetPosition.xy - sTemp.initPosition.xy))
					{
						value = sTemp.initPosition + @float;
						isEndOnTarget = false;
					}
					value.z = 0f;
					for (int l = 1; l < 15; l++)
					{
						sTemp.stepPosition = DTool.QuadraticBezierCurve(in from2, in v2, in value, 1f / 15f * (float)l);
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = sTemp.stepPosition
						});
					}
					rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
					{
						Value = value
					});
				}
				else
				{
					for (int m = 1; m < 20; m++)
					{
						from2 = rayBodyPoints[rayBodyPoints.Length - 1].Value;
						v2 = new float3((from2 + targetPosition).xy / 2f, -4f);
						sTemp.stepPosition = DTool.QuadraticBezierCurve(in from2, in v2, in targetPosition, (float)m / 20f);
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = sTemp.stepPosition
						});
					}
					rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
					{
						Value = targetPosition
					});
					isEndOnTarget = false;
				}
				float4 value2 = new float4(value, 90f);
				hitEffectNode.Add(in value2);
				touchGroundPoss.Add(in value);
				bool isNotRefracted2 = isRefract;
				NativeList<float> heightList2 = new NativeList<float>(Allocator.Temp);
				GetFallHeightList(ref heightList2, 11, 2.5f);
				if (isNotRefracted2)
				{
					ChaseEnemyFallRefraction(ref isNotRefracted2, ref rayBodyPoints, ref sTemp, in refractRemain, ref refractHitteds, in heightList2, ref hitEffectNode, ref touchGroundPoss, unitType, ref isEndOnTarget, in targetPosition);
				}
				if (sTemp.rebounceTime <= 0)
				{
					break;
				}
				for (int n = 0; n < sTemp.rebounceTime; n++)
				{
					if (!isEndOnTarget)
					{
						for (int num4 = 1; num4 < 20; num4++)
						{
							from2 = rayBodyPoints[rayBodyPoints.Length - 1].Value;
							v2 = new float3((from2 + targetPosition).xy / 2f, -4f);
							sTemp.stepPosition = DTool.QuadraticBezierCurve(in from2, in v2, in targetPosition, (float)num4 / 20f);
							rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
							{
								Value = sTemp.stepPosition
							});
						}
						isEndOnTarget = true;
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = targetPosition
						});
					}
					else
					{
						from2 = targetPosition;
						value = targetPosition;
						float3 float2 = DTool.RotateDir(sTemp.stepDirection, (float)n * 360f / (float)sTemp.rebounceTime);
						v2 = new float3((targetPosition + float2 * 5f).xy, -1.5f);
						zero2 = new float3(targetPosition.xy, -3f);
						for (int num5 = 1; num5 < 20; num5++)
						{
							sTemp.stepPosition = DTool.CubicBezierCurve(in from2, in v2, in zero2, in value, (float)num5 / 20f);
							rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
							{
								Value = sTemp.stepPosition
							});
						}
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = targetPosition
						});
						touchGroundPoss.Add(in targetPosition);
						value2 = new float4(targetPosition, 90f);
						hitEffectNode.Add(in value2);
					}
					if (isNotRefracted2)
					{
						ChaseEnemyFallRefraction(ref isNotRefracted2, ref rayBodyPoints, ref sTemp, in refractRemain, ref refractHitteds, in heightList2, ref hitEffectNode, ref touchGroundPoss, unitType, ref isEndOnTarget, in targetPosition);
					}
				}
				break;
			}
			if (!chaseEnemyPositionMinAngle)
			{
				NormalStraightGo(ref rayBodyPoints, ref sTemp, in unitType, entity, in isRefract, in refractRemain, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode);
				break;
			}
			bool flag = true;
			while (sTemp.remainDistance > 0.01f)
			{
				num2++;
				if (num2 <= 200)
				{
					if (flag)
					{
						ref float3 stepVelocity = ref sTemp.stepVelocity;
						ref float3 stepPosition = ref sTemp.stepPosition;
						ref float3 stepDirection = ref sTemp.stepDirection;
						DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
						flag = ChaseFirstEnemyLinePosStep(ref stepVelocity, ref stepPosition, ref stepDirection, in disintegrationRayBodyPoint.Value, sTemp.spellSpeed * 0.05f, in sTemp.chaseEnemyRotateSpeed, in targetPosition);
					}
					else
					{
						ref float3 stepVelocity2 = ref sTemp.stepVelocity;
						ref float3 stepPosition2 = ref sTemp.stepPosition;
						ref float3 stepDirection2 = ref sTemp.stepDirection;
						DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
						ChaseEnemyLinePosStep(ref stepVelocity2, ref stepPosition2, ref stepDirection2, in disintegrationRayBodyPoint.Value, sTemp.spellSpeed * 0.05f, in sTemp.chaseEnemyRotateSpeed, in targetPosition);
					}
					if (!GetHitDataAndRebouceAndRefract(ref sTemp, ref rayBodyPoints, in unitType, entity, in isRefract, ref refractRemain2, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode, out var isResetDirection2))
					{
						if (isResetDirection2)
						{
							flag = true;
						}
						continue;
					}
					break;
				}
				break;
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
			if (sTemp.isFall)
			{
				float3 pos = new float3(PlayerCtrller.mousePosition.xy, 0f);
				AddGroundPoint(ref rayBodyPoints, ref touchGroundPoss, ref hitEffectNode, in pos);
				NativeList<float> heightList3 = new NativeList<float>(Allocator.Temp);
				GetFallHeightList(ref heightList3, 11, -2.5f);
				bool isNotRefracted3 = isRefract;
				if (isNotRefracted3)
				{
					NormalFallRefraction(ref isNotRefracted3, ref rayBodyPoints, ref sTemp, in refractRemain, ref refractHitteds, in heightList3, ref hitEffectNode, ref touchGroundPoss, unitType);
				}
				float3 oldDir = math.normalizesafe(new float3(rayBodyPoints[rayBodyPoints.Length - 1].Value.xy - rayBodyPoints[rayBodyPoints.Length - 2].Value.xy, 0f));
				float3 v3 = new float3(oldDir.xy * 5f + pos.xy, -1.5f);
				float num6 = 60f;
				if (sTemp.rebounceTime > 2)
				{
					num6 = ((sTemp.rebounceTime > 5) ? (360f / (float)sTemp.rebounceTime) : (180f / (float)sTemp.rebounceTime));
				}
				if (num6 < 30f)
				{
					num6 = 30f;
				}
				oldDir = DTool.RotateDir(oldDir, num6);
				float3 v4 = new float3(oldDir.xy * 5f + pos.xy, -1.5f);
				for (int num7 = 0; num7 < sTemp.rebounceTime; num7++)
				{
					for (int num8 = 1; num8 < 15; num8++)
					{
						sTemp.stepPosition = DTool.CubicBezierCurve_Uniform(in pos, in v3, in v4, in pos, (float)num8 / 15f, 15);
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = sTemp.stepPosition
						});
					}
					AddGroundPoint(ref rayBodyPoints, ref touchGroundPoss, ref hitEffectNode, in pos);
					v3 = v4;
					oldDir = DTool.RotateDir(oldDir, num6);
					v4 = new float3(oldDir.xy * 5f + pos.xy, -1.5f);
				}
				break;
			}
			while (sTemp.remainDistance > 0.01f)
			{
				num2++;
				if (num2 <= 200)
				{
					ref float3 mousePosition = ref PlayerCtrller.mousePosition;
					DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
					float3 end = DTool.IgnoreZDir(in mousePosition, in disintegrationRayBodyPoint.Value);
					float t = math.clamp(sTemp.chaseMouseLerpSpeed * 0.6f, 0f, 1f);
					sTemp.chaseMouseLerpData = math.lerp(sTemp.chaseMouseLerpData, end, t);
					sTemp.stepVelocity = sTemp.chaseMouseLerpData * num;
					sTemp.stepDirection = math.normalize(sTemp.chaseMouseLerpData);
					sTemp.stepPosition = rayBodyPoints[rayBodyPoints.Length - 1].Value + sTemp.stepVelocity;
					if (!GetHitDataAndRebouceAndRefract(ref sTemp, ref rayBodyPoints, in unitType, entity, in isRefract, ref refractRemain2, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode, out var isResetDirection3))
					{
						if (isResetDirection3)
						{
							sTemp.chaseMouseLerpData = sTemp.stepDirection * 1.5f;
						}
						continue;
					}
					break;
				}
				break;
			}
			break;
		case SpellSpecialMovementType.ChaseOwner:
			num = 0.1f;
			if (sTemp.isFall)
			{
				float3 from = rayBodyPoints[0].Value;
				float3 v = new float3(from.xy, -4f);
				float3 to = DTool.IgnoreZPosition(in sTemp.chaseShooterPos);
				float3 zero = float3.zero;
				sTemp.stepDirection = DTool.IgnoreZDir(in to, in from);
				for (int i = 1; i < 20; i++)
				{
					sTemp.stepPosition = DTool.QuadraticBezierCurve(in from, in v, in to, (float)i / 20f);
					rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
					{
						Value = sTemp.stepPosition
					});
				}
				AddGroundPoint(ref rayBodyPoints, ref touchGroundPoss, ref hitEffectNode, in to);
				bool isNotRefracted = isRefract;
				NativeList<float> heightList = new NativeList<float>(Allocator.Temp);
				GetFallHeightList(ref heightList, 11, 2.5f);
				if (isNotRefracted)
				{
					NormalFallRefraction(ref isNotRefracted, ref rayBodyPoints, ref sTemp, in refractRemain, ref refractHitteds, in heightList, ref hitEffectNode, ref touchGroundPoss, unitType);
				}
				for (int j = 0; j < sTemp.rebounceTime; j++)
				{
					from = rayBodyPoints[rayBodyPoints.Length - 1].Value;
					v = sTemp.stepDirection * 4f + rayBodyPoints[rayBodyPoints.Length - 1].Value;
					v.z = -2f;
					sTemp.stepDirection = DTool.RotateDir(sTemp.stepDirection, -80f);
					zero = sTemp.stepDirection * 4f + rayBodyPoints[rayBodyPoints.Length - 1].Value;
					zero.z = -2f;
					for (int k = 1; k < 20; k++)
					{
						sTemp.stepPosition = DTool.CubicBezierCurve_Uniform(in from, in v, in zero, in to, (float)k / 20f);
						rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
						{
							Value = sTemp.stepPosition
						});
					}
					sTemp.stepDirection = DTool.RotateDir(sTemp.stepDirection, 160f);
					AddGroundPoint(ref rayBodyPoints, ref touchGroundPoss, ref hitEffectNode, in to);
					if (isNotRefracted)
					{
						NormalFallRefraction(ref isNotRefracted, ref rayBodyPoints, ref sTemp, in refractRemain, ref refractHitteds, in heightList, ref hitEffectNode, ref touchGroundPoss, unitType);
					}
				}
				break;
			}
			while (sTemp.remainDistance > 0.01f)
			{
				num2++;
				if (num2 <= 200)
				{
					float3 target = sTemp.chaseShooterPos - rayBodyPoints[rayBodyPoints.Length - 1].Value;
					float lerp = Mathf.Abs(Mathf.Abs(GetDegreeIgnorZ(sTemp.orgDirection, target)) - 90f) / 90f;
					DirMoveTowardsCounterClockwise3d(in sTemp.orgDirection, in target, num * sTemp.chaseEnemyRotateSpeed, out sTemp.stepDirection);
					sTemp.stepVelocity = sTemp.stepDirection * DTool.Lerp(0.6f, 1.4f, lerp) * num;
					sTemp.stepPosition = rayBodyPoints[rayBodyPoints.Length - 1].Value + sTemp.stepVelocity;
					if (GetHitDataAndRebouceAndRefract(ref sTemp, ref rayBodyPoints, in unitType, entity, in isRefract, ref refractRemain2, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode, out var _))
					{
						break;
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	[BurstCompile]
	private void SphereCastEnemyHit(in float3 startOnLine, in float3 endOnLine, in float width, UnitType shooterType, ref NativeList<(ColliderCastHit, SpellTools.HitType)> hitList)
	{
		NativeList<ColliderCastHit> hitList2 = new NativeList<ColliderCastHit>(Allocator.Temp);
		SpellTools.GetAttackableEntitiesInSphereCast(in startOnLine, in endOnLine, in width, in shooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList2);
		foreach (ColliderCastHit item in hitList2)
		{
			Entity target = item.Entity;
			SpellTools.HitType entityHitType = SpellTools.GetEntityHitType(in target, in shooterType, in UnitPropertyLookup, in SpellConfigLookup);
			if ((uint)(entityHitType - 3) <= 3u)
			{
				(ColliderCastHit, SpellTools.HitType) value = (item, entityHitType);
				hitList.Add(in value);
			}
		}
	}

	[BurstCompile]
	private float GetNonFallRayDamageWidth(float spellScale)
	{
		return 0.13f * math.max(0.0001f, math.abs(spellScale));
	}

	[BurstCompile]
	private bool TryRaycstWallHit(float3 startOnLine, float3 endOnLine, out Unity.Physics.RaycastHit wallHit, in PhysicsWorldSingleton physicsWorld)
	{
		wallHit = default(Unity.Physics.RaycastHit);
		return RaycastWallHits(startOnLine, endOnLine, out wallHit, in physicsWorld);
	}

	[BurstCompile]
	private bool RaycastWallHits(float3 start, float3 end, out Unity.Physics.RaycastHit hitResult, in PhysicsWorldSingleton physicsWorld)
	{
		hitResult = default(Unity.Physics.RaycastHit);
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
		NativeList<Unity.Physics.RaycastHit> allHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Temp);
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
	private void HitDamageApply(Entity spellEntity, in NativeList<Entity> hitList, in NativeList<float3> hitDataList, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, in DynamicOptimizeData optimizeData, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < hitList.Length; i++)
		{
			SpellTools.GetSpellElementDataWithTimeScale(in elementEffect, in optimizeData, out var result);
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in result, in componentData, out var info, CostRefraction: false);
			info.SetKnockbackForceIgnoreZBySpell(hitDataList[i]);
			info.spell.HitPosition = TransformLookup[hitList[i]].Position;
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity target = hitList[i];
			cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[BurstCompile]
	private void HitDamageApplyExplosion(Entity spellEntity, in NativeList<Entity> hitList, in float3 explosionPos, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < hitList.Length; i++)
		{
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
			info.spell.HitPosition = TransformLookup[hitList[i]].Position;
			info.SetKnockbackForceIgnoreZBySpell(math.normalizesafe(info.spell.HitPosition - explosionPos));
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity target = hitList[i];
			cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[BurstCompile]
	private bool CheckWallHitAndEnemyHitStep(ref SpellTempData sTempData, in float3 prevPos, in UnitType unitType, in Entity spellEntity, ref bool lineEnd, in bool isRefract, ref int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList, ref NativeList<float4> hitEffectNode)
	{
		CheckHitWallStep(ref sTempData, in prevPos, ref lineEnd, out var wallHitType, out var wallHit);
		CheckHitEnemyByStepVelocity(ref sTempData, in prevPos, in unitType, in spellEntity, ref hitList, ref hitDataList, ref lineEnd, in isRefract, ref refractRemain, ref refractHitteds, out var isPreEnded, out var isRefracted);
		AddHitEffectNodeInStepHit(in isPreEnded, in wallHitType, ref hitEffectNode, in sTempData.stepPosition, in sTempData.stepVelocity, in wallHit);
		if (isRefracted)
		{
			return true;
		}
		if (wallHitType == 2)
		{
			return true;
		}
		return false;
	}

	[BurstCompile]
	private bool CheckHitWallStep(ref SpellTempData sTempData, in float3 fromPos, ref bool lineEnd, out int wallHitType, out Unity.Physics.RaycastHit wallHit)
	{
		if (sTempData.isIgnoreWall)
		{
			wallHit = default(Unity.Physics.RaycastHit);
			wallHitType = 0;
			return false;
		}
		if (CheckHitWallByStepVelocity(in fromPos, ref sTempData.stepPosition, ref sTempData.stepVelocity, out wallHit))
		{
			if (sTempData.rebounceTime <= 0)
			{
				wallHitType = 1;
				lineEnd = true;
			}
			else
			{
				sTempData.rebounceTime--;
				wallHitType = 2;
				sTempData.stepDirection = math.reflect(sTempData.orgDirection, wallHit.SurfaceNormal);
				float num = math.length(sTempData.stepVelocity);
				if (sTempData.remainDistance - num < 0.5f && sTempData.remainDistance - num > 0f)
				{
					sTempData.remainDistance = num + 0.5f;
				}
			}
			return true;
		}
		wallHitType = 0;
		return false;
	}

	[BurstCompile]
	private bool CheckHitWallByStepVelocity(in float3 previousPos, ref float3 normalStepPos, ref float3 stepVelocity, out Unity.Physics.RaycastHit wallHit)
	{
		wallHit = default(Unity.Physics.RaycastHit);
		if (TryRaycstWallHit(previousPos, normalStepPos, out wallHit, in PhysicsWorld))
		{
			stepVelocity *= wallHit.Fraction;
			normalStepPos = previousPos + stepVelocity;
			return true;
		}
		return false;
	}

	[BurstCompile]
	private void CheckHitEnemyByStepVelocity(ref SpellTempData sTempData, in float3 previousPos, in UnitType unitType, in Entity spellEntity, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList, ref bool lineEnd, in bool isRefract, ref int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> hitEntities, out bool isPreEnded, out bool isRefracted)
	{
		isPreEnded = false;
		isRefracted = false;
		NativeList<(ColliderCastHit, SpellTools.HitType)> hitList2 = new NativeList<(ColliderCastHit, SpellTools.HitType)>(Allocator.Temp);
		SphereCastEnemyHit(in previousPos, in sTempData.stepPosition, in sTempData.width, unitType, ref hitList2);
		if (hitList2.Length > 0)
		{
			hitList2.Sort<(ColliderCastHit, SpellTools.HitType), ColliderHitDistanceComparer>(default(ColliderHitDistanceComparer));
			for (int i = 0; i < hitList2.Length; i++)
			{
				if ((hitList2[i].Item1.Fraction <= 0.0001f && hitList.Contains(hitList2[i].Item1.Entity)) || hitList.Contains(hitList2[i].Item1.Entity))
				{
					continue;
				}
				Entity value = hitList2[i].Item1.Entity;
				hitList.Add(in value);
				hitDataList.Add(in sTempData.stepDirection);
				if (hitList2[i].Item2 == SpellTools.HitType.Brittleness || hitList2[i].Item2 == SpellTools.HitType.Butterfly || !isRefract || refractRemain <= 0 || hitList2[i].Item2 != SpellTools.HitType.Unit)
				{
					continue;
				}
				ref float3 stepDirection = ref sTempData.stepDirection;
				float3 hitPosition = hitList2[i].Item1.Position;
				DynamicBuffer<SpellRefractionHitEntities> hitEntities2 = hitEntities;
				value = hitList2[i].Item1.Entity;
				if (RefractionDirectionSet(ref stepDirection, in hitPosition, in unitType, hitEntities2, in value))
				{
					isRefracted = true;
					float fraction = hitList2[i].Item1.Fraction;
					sTempData.stepVelocity *= fraction;
					if (math.length(sTempData.stepVelocity) < 0.5f)
					{
						sTempData.stepVelocity = math.normalizesafe(sTempData.stepVelocity) * 0.5f;
					}
					float num = math.length(sTempData.stepVelocity);
					if (sTempData.remainDistance - num < 0.5f && sTempData.remainDistance - num > 0f)
					{
						sTempData.remainDistance = num + 0.5f;
					}
					sTempData.stepPosition = previousPos + sTempData.stepVelocity;
					refractRemain--;
					isPreEnded = true;
					lineEnd = false;
					break;
				}
			}
		}
		AddOverlappedAttackablesOnPoint(previousPos, sTempData.width, unitType, sTempData.stepDirection, ref hitList, ref hitDataList);
		AddOverlappedAttackablesOnPoint(sTempData.stepPosition, sTempData.width, unitType, sTempData.stepDirection, ref hitList, ref hitDataList);
		hitList2.Dispose();
	}

	private void CheckHoverLineHit(DynamicBuffer<DisintegrationRayBodyPoint> lineBuffer, float width, UnitType shooterType, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList)
	{
		NativeList<(ColliderCastHit, SpellTools.HitType)> hitList2 = new NativeList<(ColliderCastHit, SpellTools.HitType)>(Allocator.Temp);
		for (int i = 0; i < lineBuffer.Length - 1; i++)
		{
			hitList2.Clear();
			DisintegrationRayBodyPoint disintegrationRayBodyPoint = lineBuffer[i];
			ref float3 value = ref disintegrationRayBodyPoint.Value;
			DisintegrationRayBodyPoint disintegrationRayBodyPoint2 = lineBuffer[i + 1];
			SphereCastEnemyHit(in value, in disintegrationRayBodyPoint2.Value, in width, shooterType, ref hitList2);
			float3 value2 = math.normalizesafe(lineBuffer[i + 1].Value - lineBuffer[i].Value);
			for (int j = 0; j < hitList2.Length; j++)
			{
				if (!hitList.Contains(hitList2[j].Item1.Entity))
				{
					Entity value3 = hitList2[j].Item1.Entity;
					hitList.Add(in value3);
					hitDataList.Add(in value2);
				}
			}
			AddOverlappedAttackablesOnPoint(lineBuffer[i].Value, width, shooterType, value2, ref hitList, ref hitDataList);
			AddOverlappedAttackablesOnPoint(lineBuffer[i + 1].Value, width, shooterType, value2, ref hitList, ref hitDataList);
		}
		hitList2.Dispose();
	}

	private void AddOverlappedAttackablesOnPoint(float3 position, float radius, UnitType shooterType, float3 hitDirection, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList)
	{
		CollisionFilter filter = DTool.CreateOtherCampFilter(shooterType, containsBrittleness: true);
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		if (PhysicsWorld.OverlapSphere(position, radius, ref outHits, filter))
		{
			for (int i = 0; i < outHits.Length; i++)
			{
				if (UnitPropertyLookup.TryGetComponent(outHits[i].Entity, out var componentData) && componentData.CanBeTarget && !hitList.Contains(outHits[i].Entity))
				{
					Entity value = outHits[i].Entity;
					hitList.Add(in value);
					hitDataList.Add(in hitDirection);
				}
			}
		}
		outHits.Dispose();
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = (DTool.IsSameCamp(shooterType, UnitType.Player) ? 8388608u : 16777216u);
		NativeList<DistanceHit> outHits2 = new NativeList<DistanceHit>(Allocator.Temp);
		if (PhysicsWorld.OverlapSphere(position, radius, ref outHits2, @default))
		{
			for (int j = 0; j < outHits2.Length; j++)
			{
				if (SpellConfigLookup.HasComponent(outHits2[j].Entity) && !hitList.Contains(outHits2[j].Entity))
				{
					Entity value = outHits2[j].Entity;
					hitList.Add(in value);
					hitDataList.Add(in hitDirection);
				}
			}
		}
		outHits2.Dispose();
	}

	[BurstCompile]
	private void AddHitEffectNodeInStepHit(in bool isPreEnded, in int wallHitType, ref NativeList<float4> hitEffectNode, in float3 normalStepPos, in float3 stepVelocity, in Unity.Physics.RaycastHit wallHit)
	{
		if (!isPreEnded)
		{
			switch (wallHitType)
			{
			case 1:
				AddHitEffectNode(ref hitEffectNode, normalStepPos, -stepVelocity.xy);
				break;
			case 2:
				AddHitEffectNode(ref hitEffectNode, normalStepPos, wallHit.SurfaceNormal.xy);
				break;
			}
		}
	}

	[BurstCompile]
	private bool GetHitDataAndRebouceAndRefract(ref SpellTempData sTemp, ref DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, in UnitType unitType, Entity spellEntity, in bool isRefract, ref int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList, ref NativeList<float4> hitEffectNode, out bool isResetDirection)
	{
		bool lineEnd = false;
		DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
		isResetDirection = CheckWallHitAndEnemyHitStep(ref sTemp, in disintegrationRayBodyPoint.Value, in unitType, in spellEntity, ref lineEnd, in isRefract, ref refractRemain, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode);
		int length = rayBodyPoints.Length;
		disintegrationRayBodyPoint = default(DisintegrationRayBodyPoint);
		disintegrationRayBodyPoint.Value = rayBodyPoints[rayBodyPoints.Length - 1].Value + sTemp.stepVelocity;
		rayBodyPoints.Insert(length, disintegrationRayBodyPoint);
		sTemp.orgDirection = sTemp.stepDirection;
		sTemp.remainDistance -= math.length(sTemp.stepVelocity);
		if (lineEnd)
		{
			return true;
		}
		return false;
	}

	[BurstCompile]
	private void StraightGoLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, in float3 fromPos, float3 stepDirection, float remainMaxDistance)
	{
		stepVelocity = stepDirection * remainMaxDistance;
		normalStepPos = fromPos + stepVelocity;
	}

	[BurstCompile]
	private void NormalStraightGo(ref DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, ref SpellTempData sTemp, in UnitType unitType, Entity spellEntity, in bool isRefract, in int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, ref NativeList<Entity> hitList, ref NativeList<float3> hitDataList, ref NativeList<float4> hitEffectNode)
	{
		int refractRemain2 = refractRemain;
		int num = 0;
		while (sTemp.remainDistance > 0.01f)
		{
			num++;
			if (num <= 300)
			{
				ref float3 stepVelocity = ref sTemp.stepVelocity;
				ref float3 stepPosition = ref sTemp.stepPosition;
				DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
				StraightGoLinePosStep(ref stepVelocity, ref stepPosition, in disintegrationRayBodyPoint.Value, sTemp.stepDirection, sTemp.remainDistance);
				if (!GetHitDataAndRebouceAndRefract(ref sTemp, ref rayBodyPoints, in unitType, spellEntity, in isRefract, ref refractRemain2, ref refractHitteds, ref hitList, ref hitDataList, ref hitEffectNode, out var isResetDirection))
				{
					if (isResetDirection)
					{
						sTemp.chaseMouseLerpData = math.normalizesafe(sTemp.chaseMouseLerpData) * 1.5f;
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	[BurstCompile]
	private void NormalFallLineGenerate(ref SpellTempData spellTempData, ref NativeList<float3> touchGroundPos, ref NativeList<float4> hitEffectNode, ref DynamicBuffer<DisintegrationRayBodyPoint> lineBuffer, in bool isRefract, in int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, UnitType shooterType, Entity spellEntity)
	{
		NativeList<float> pointList = new NativeList<float>(Allocator.Temp);
		bool isNotRefracted = isRefract;
		float distancePerStep = 0.2f;
		float distance = 4f;
		GetFallHeightFixByDistance(ref pointList, in distance, ref distancePerStep);
		if (spellTempData.isSplit)
		{
			for (int i = 1; i < pointList.Length - 1; i++)
			{
				DisintegrationRayBodyPoint disintegrationRayBodyPoint = default(DisintegrationRayBodyPoint);
				disintegrationRayBodyPoint.Value = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * distancePerStep).xy, pointList[i]);
				DisintegrationRayBodyPoint elem = disintegrationRayBodyPoint;
				lineBuffer.Insert(lineBuffer.Length, elem);
			}
			float3 pos = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * distancePerStep).xy, pointList[pointList.Length - 1]);
			AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in pos);
			if (isNotRefracted)
			{
				NormalFallRefraction(ref isNotRefracted, ref lineBuffer, ref spellTempData, in refractRemain, ref refractHitteds, in pointList, ref hitEffectNode, ref touchGroundPos, shooterType);
			}
		}
		else
		{
			ref float3 stepVelocity = ref spellTempData.stepVelocity;
			ref float3 stepPosition = ref spellTempData.stepPosition;
			DisintegrationRayBodyPoint disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
			StraightGoFallGroundStep(ref stepVelocity, ref stepPosition, in disintegrationRayBodyPoint.Value, spellTempData.spellSpeed / spellTempData.fallSpeed, in spellTempData.stepDirection);
			AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in spellTempData.stepPosition);
		}
		if (isNotRefracted)
		{
			NormalFallRefraction(ref isNotRefracted, ref lineBuffer, ref spellTempData, in refractRemain, ref refractHitteds, in pointList, ref hitEffectNode, ref touchGroundPos, shooterType);
		}
		if (spellTempData.rebounceTime <= 0)
		{
			return;
		}
		for (int j = 0; j < spellTempData.rebounceTime; j++)
		{
			for (int k = 1; k < pointList.Length - 1; k++)
			{
				DisintegrationRayBodyPoint disintegrationRayBodyPoint = default(DisintegrationRayBodyPoint);
				disintegrationRayBodyPoint.Value = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * distancePerStep).xy, pointList[k]);
				DisintegrationRayBodyPoint elem2 = disintegrationRayBodyPoint;
				lineBuffer.Insert(lineBuffer.Length, elem2);
			}
			float3 pos = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * distancePerStep).xy, pointList[pointList.Length - 1]);
			AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in pos);
			if (isNotRefracted)
			{
				NormalFallRefraction(ref isNotRefracted, ref lineBuffer, ref spellTempData, in refractRemain, ref refractHitteds, in pointList, ref hitEffectNode, ref touchGroundPos, shooterType);
			}
		}
	}

	[BurstCompile]
	private void ChaseEnemyFallRefraction(ref bool isNotRefracted, ref DynamicBuffer<DisintegrationRayBodyPoint> lineBuffer, ref SpellTempData spellTempData, in int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in NativeList<float> heightList, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPos, UnitType shooterType, ref bool isEndOnTarget, in float3 targetPosition)
	{
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DisintegrationRayBodyPoint disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
		DTool.GetEnemyEntityInRange(in disintegrationRayBodyPoint.Value, spellTempData.radius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
		if (result.IsEmpty)
		{
			return;
		}
		for (int i = 0; i < refractRemain; i++)
		{
			DynamicBuffer<SpellRefractionHitEntities> hitEntities = refractHitteds;
			disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
			if (RefractionTransformGet(hitEntities, in result, in disintegrationRayBodyPoint.Value, in shooterType, out var targetPos))
			{
				isNotRefracted = false;
				float3 x = targetPos - lineBuffer[lineBuffer.Length - 1].Value;
				spellTempData.stepDirection = math.normalizesafe(x);
				float num = math.length(x) / (float)(heightList.Length - 1);
				for (int j = 1; j < heightList.Length; j++)
				{
					int length = lineBuffer.Length;
					disintegrationRayBodyPoint = default(DisintegrationRayBodyPoint);
					disintegrationRayBodyPoint.Value = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * num).xy, 0f - heightList[j]);
					lineBuffer.Insert(length, disintegrationRayBodyPoint);
				}
				isEndOnTarget = math.length(targetPosition - targetPos) < 0.01f;
				float4 value = new float4(lineBuffer[lineBuffer.Length - 1].Value, 90f);
				hitEffectNode.Add(in value);
				disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
				touchGroundPos.Add(in disintegrationRayBodyPoint.Value);
				disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
				DTool.GetEnemyEntityInRange(in disintegrationRayBodyPoint.Value, spellTempData.radius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
				continue;
			}
			break;
		}
	}

	[BurstCompile]
	private void NormalFallRefraction(ref bool isNotRefracted, ref DynamicBuffer<DisintegrationRayBodyPoint> lineBuffer, ref SpellTempData spellTempData, in int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in NativeList<float> heightList, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPos, UnitType shooterType)
	{
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DisintegrationRayBodyPoint disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
		DTool.GetEnemyEntityInRange(in disintegrationRayBodyPoint.Value, spellTempData.radius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
		if (result.IsEmpty)
		{
			return;
		}
		for (int i = 0; i < refractRemain; i++)
		{
			DynamicBuffer<SpellRefractionHitEntities> hitEntities = refractHitteds;
			disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
			if (RefractionTransformGet(hitEntities, in result, in disintegrationRayBodyPoint.Value, in shooterType, out var targetPos))
			{
				isNotRefracted = false;
				float3 x = targetPos - lineBuffer[lineBuffer.Length - 1].Value;
				spellTempData.stepDirection = math.normalizesafe(x);
				float num = math.length(x) / (float)(heightList.Length - 1);
				for (int j = 1; j < heightList.Length - 1; j++)
				{
					int length = lineBuffer.Length;
					disintegrationRayBodyPoint = default(DisintegrationRayBodyPoint);
					disintegrationRayBodyPoint.Value = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * num).xy, heightList[j]);
					lineBuffer.Insert(length, disintegrationRayBodyPoint);
				}
				float3 pos = new float3((lineBuffer[lineBuffer.Length - 1].Value + spellTempData.stepDirection * num).xy, 0f);
				AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in pos);
				disintegrationRayBodyPoint = lineBuffer[lineBuffer.Length - 1];
				DTool.GetEnemyEntityInRange(in disintegrationRayBodyPoint.Value, spellTempData.radius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
				continue;
			}
			break;
		}
	}

	[BurstCompile]
	private void RotateFallRefraction(ref float stepAngle, in NativeList<float> heightList, ref DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, ref SpellTempData sTemp, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPoss)
	{
		for (int i = 1; i < heightList.Length - 1; i++)
		{
			stepAngle += 5f;
			float3 dir = DTool.GetDir(stepAngle * (MathF.PI / 180f));
			rayBodyPoints.Insert(rayBodyPoints.Length, new DisintegrationRayBodyPoint
			{
				Value = new float3((dir * sTemp.rotateRadius + sTemp.aroundCenter).xy, (0f - heightList[i]) * 0.7f)
			});
		}
		stepAngle += 3f;
		float3 dir2 = DTool.GetDir(stepAngle * (MathF.PI / 180f));
		float2 xy = (dir2 * sTemp.rotateRadius + sTemp.aroundCenter).xy;
		NativeList<float> nativeList = heightList;
		float3 pos = new float3(xy, 0f - nativeList[nativeList.Length - 1]);
		AddGroundPoint(ref rayBodyPoints, ref touchGroundPoss, ref hitEffectNode, in pos);
	}

	[BurstCompile]
	private void RotateFallRefractionCheck(ref float stepAngle, in bool isRefract, ref int currentRefractRemain, DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in UnitType unitType, ref DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints, ref SpellTempData sTemp, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPoss, in NativeList<float> heightList)
	{
		if (!isRefract)
		{
			return;
		}
		while (currentRefractRemain > 0)
		{
			NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
			DisintegrationRayBodyPoint disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
			DTool.GetEnemyEntityInRange(in disintegrationRayBodyPoint.Value, sTemp.radius, unitType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
			if (!result.IsEmpty)
			{
				disintegrationRayBodyPoint = rayBodyPoints[rayBodyPoints.Length - 1];
				if (RefractionTransformGet(refractHitteds, in result, in disintegrationRayBodyPoint.Value, in unitType, out var _))
				{
					currentRefractRemain--;
					RotateFallRefraction(ref stepAngle, in heightList, ref rayBodyPoints, ref sTemp, ref hitEffectNode, ref touchGroundPoss);
					continue;
				}
				break;
			}
			break;
		}
	}

	[BurstCompile]
	private void StraightGoFallGroundStep(ref float3 stepVelocity, ref float3 normalStepPos, in float3 fromPos, float dirTan, in float3 dirInXY)
	{
		float num = dirTan * math.abs(fromPos.z);
		stepVelocity = new float3((dirInXY * num).xy, 0f - fromPos.z);
		normalStepPos = fromPos + stepVelocity;
	}

	[BurstCompile]
	private bool ChaseFirstEnemyLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, in float3 prevPos, float orgStepLength, in float chaseEnemySpeed, in float3 targetPos)
	{
		float3 target = DTool.IgnoreZDir(in targetPos, in prevPos);
		float num = math.degrees(math.acos(math.clamp(math.dot(stepDirection.xy, target.xy), -1f, 1f)));
		stepDirection = DTool.DirMoveTowardsIgnoreZ(in stepDirection, in target, chaseEnemySpeed * orgStepLength);
		stepVelocity = stepDirection * orgStepLength;
		normalStepPos = prevPos + stepVelocity;
		if (num <= chaseEnemySpeed * orgStepLength)
		{
			return false;
		}
		return true;
	}

	[BurstCompile]
	private void ChaseEnemyLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, in float3 prevPos, float orgStepLength, in float chaseEnemySpeed, in float3 targetPos)
	{
		float3 target = DTool.IgnoreZDir(in targetPos, in prevPos);
		DirMoveTowardsIgnoreZ(in stepDirection, in target, chaseEnemySpeed * orgStepLength, isClockWise: true, out stepDirection);
		stepVelocity = stepDirection * orgStepLength;
		normalStepPos = prevPos + stepVelocity;
	}

	[BurstCompile]
	private void HitNodeEffectApply(in NativeList<float4> hitEffectNode, ref Spell1011DisintegrationRayData rayData, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		rayData.HitNodeEffLoop += DeltaTime;
		int num = (int)math.floor(rayData.HitNodeEffLoop * 20f);
		if (num <= 0)
		{
			return;
		}
		rayData.HitNodeEffLoop -= (float)num / 20f;
		foreach (float4 item in hitEffectNode)
		{
			for (int i = 0; i < num; i++)
			{
				config.ColorType.ColorEnumToString(out var result);
				float3 rootPosition = item.xyz;
				float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
				globalParticleEmitParams.Name = $"1011_HitWall_{result}";
				globalParticleEmitParams.Alpha = 1f;
				globalParticleEmitParams.Position = new float3(item.xyz) + layerPosition;
				GlobalParticleEmitParams element = globalParticleEmitParams;
				CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
			}
		}
	}

	[BurstCompile]
	private bool RefractionDirectionSet(ref float3 stepDirection, in float3 hitPosition, in UnitType unitType, DynamicBuffer<SpellRefractionHitEntities> hitEntities, in Entity currentHitEntity)
	{
		NativeList<Entity> currentHitEntities = new NativeList<Entity>(1, Allocator.Temp) { in currentHitEntity };
		if (!RefractionTransformGet(hitEntities, in currentHitEntities, in hitPosition, in unitType, out var targetPos))
		{
			return false;
		}
		float3 input = math.normalizesafe(targetPos - hitPosition);
		stepDirection = input.IgnoreZ();
		return true;
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
	private float GetDegreeIgnorZ(float3 from, float3 to)
	{
		float2 x = math.normalize(new float2(from.x, from.y));
		float2 y = math.normalize(new float2(to.x, to.y));
		float num = math.degrees(math.acos(math.clamp(math.dot(x, y), -1f, 1f)));
		if (!(x.x * y.y - x.y * y.x >= 0f))
		{
			return 0f - num;
		}
		return num;
	}

	[BurstCompile]
	private void DirMoveTowardsCounterClockwise(in float2 source, in float2 target, float maxDeltaAngleDeg, out float2 result)
	{
		float2 x = math.normalize(source);
		float2 y = math.normalize(target);
		float x2 = math.acos(math.clamp(math.dot(x, y), -1f, 1f));
		float y2 = math.radians(maxDeltaAngleDeg);
		float x3 = math.min(x2, y2);
		float num = math.sin(x3);
		float num2 = math.cos(x3);
		result = new float2(x.x * num2 - x.y * num, x.x * num + x.y * num2);
		result = math.normalize(result);
	}

	private void DirMoveTowardsCounterClockwise3d(in float3 source, in float3 target, float maxDeltaAngleDeg, out float3 result)
	{
		float2 source2 = source.xy;
		float2 target2 = target.xy;
		DirMoveTowardsCounterClockwise(in source2, in target2, maxDeltaAngleDeg, out var result2);
		result = new float3(result2, 0f);
	}

	[BurstCompile]
	private void GetFallHeightList(ref NativeList<float> heightList, int countHalf, float maxH)
	{
		int num = 2 * countHalf - 1;
		heightList.Clear();
		float num2 = countHalf - 1;
		for (int i = 0; i < num; i++)
		{
			float num3 = (float)i - num2;
			float value = maxH * (1f - num3 * num3 / (num2 * num2));
			heightList.Add(in value);
		}
		heightList[0] = 0f;
		heightList[num - 1] = 0f;
		heightList[countHalf - 1] = maxH;
	}

	[BurstCompile]
	private void GetFallHeightFixByDistance(ref NativeList<float> pointList, in float distance, ref float distancePerStep, float maxH = -2f)
	{
		int num = (int)math.ceil(((float)(int)math.ceil(distance / distancePerStep) + 1f) / 2f);
		int num2 = num * 2 - 1;
		distancePerStep = distance / (float)num2;
		pointList.Clear();
		GetFallHeightList(ref pointList, num, maxH);
	}

	[BurstCompile]
	private void AddHitEffectNode(ref NativeList<float4> effNodeList, float3 position, float2 direction)
	{
		float w = math.atan2(direction.y, direction.x) * 57.29578f;
		float4 value = new float4(position, w);
		effNodeList.Add(in value);
	}

	[BurstCompile]
	private bool HaveRefractionCheck(Entity entity, out int count)
	{
		count = 0;
		if (SpellRefractionLookup.HasComponent(entity))
		{
			count = SpellRefractionLookup[entity].RemainCount;
			return true;
		}
		return false;
	}

	[BurstCompile]
	private bool GetChaseEnemyPositionMinAngle(in float3 startPos, in float3 direction, in UnitType shooterType, out float3 targetPosition)
	{
		targetPosition = float3.zero;
		Entity target;
		UnitProperty_Dots targetPpt;
		return CurrentRoomEntities.FindMinAngleTarget(startPos, direction, shooterType, out target, out targetPosition, out targetPpt);
	}

	[BurstCompile]
	private void DirMoveTowards(in float2 source, in float2 target, float maxDeltaDeg, bool clockwise, out float2 result)
	{
		float2 x = math.normalize(source);
		float2 @float = math.normalize(target);
		float num = math.acos(math.clamp(math.dot(x, @float), -1f, 1f));
		float num2 = math.radians(maxDeltaDeg);
		if (num <= num2)
		{
			result = @float;
			return;
		}
		float num3 = (clockwise ? (-1f) : 1f);
		float num4 = math.sin(num2 * num3);
		float num5 = math.cos(num2 * num3);
		result.x = x.x * num5 - x.y * num4;
		result.y = x.x * num4 + x.y * num5;
		result = math.normalize(result);
	}

	[BurstCompile]
	private void DirMoveTowardsIgnoreZ(in float3 source, in float3 target, float maxDelta, bool isClockWise, out float3 result)
	{
		float2 source2 = source.xy;
		float2 target2 = target.xy;
		DirMoveTowards(in source2, in target2, maxDelta, isClockWise, out var result2);
		result = new float3(result2, 0f);
	}

	[BurstCompile]
	private void AddGroundPoint(ref DynamicBuffer<DisintegrationRayBodyPoint> bodyList, ref NativeList<float3> touchGList, ref NativeList<float4> effNodeList, in float3 pos)
	{
		bodyList.Insert(bodyList.Length, new DisintegrationRayBodyPoint
		{
			Value = pos
		});
		touchGList.Add(in pos);
		float4 value = new float4(pos, 0f);
		effNodeList.Add(in value);
	}

	[BurstCompile]
	private void TouchGroundExplosion(Entity spellEntity, in SpellConfigComponentData config, in LocalTransform trans, in SpellComponentData data, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, in float3 touchGroundPos, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"3119_Fall_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(touchGroundPos.xy, 1.08f);
		globalParticleEmitParams.Size = config.Radius.Calculate();
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		float radius = config.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in touchGroundPos, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		HitDamageApplyExplosion(spellEntity, in entities, in touchGroundPos, in data, in config, in movement, in trans, in elementEffect, chunkIndex);
	}

	[BurstCompile]
	private void SetSpellTempData(ref SpellTempData sTD, ref SpellMovementComponentData movement, in SpellComponentData componentData, in SpellConfigComponentData config, in float3 initPosition, in float spellScale, in Spell1011DisintegrationRayData rayData)
	{
		sTD.moveType = movement.Type;
		sTD.isFall = movement.IsFallSpell;
		sTD.initPosition = initPosition;
		sTD.isSplit = componentData.IsSplitSpell;
		sTD.stepVelocity = float3.zero;
		if (movement.IsFallSpell)
		{
			sTD.stepDirection = movement.Direction;
			sTD.orgDirection = movement.Direction;
		}
		else
		{
			sTD.stepDirection = math.normalizesafe(new float3(movement.Direction.xy, 0f));
			sTD.orgDirection = math.normalizesafe(new float3(movement.Direction.xy, 0f));
		}
		sTD.stepPosition = initPosition;
		sTD.spellSpeed = rayData.SpellSpeed;
		sTD.chaseEnemyRotateSpeed = movement.ChaseRotateSpeed;
		sTD.chaseMouseLerpSpeed = movement.ChaseMouseLerpSpeed;
		sTD.fallSpeed = rayData.SpellFallSpeed;
		sTD.width = (movement.IsFallSpell ? 0.2f : GetNonFallRayDamageWidth(spellScale));
		sTD.remainDistance = rayData.SpellSpeed;
		sTD.chaseMouseLerpData = movement.Direction;
		sTD.rebounceTime = movement.ReboundCount;
		sTD.aroundAngle = movement.AroundAngle;
		sTD.aroundCenter = movement.AroundCenter;
		sTD.rotateRadius = movement.AroundRadius;
		sTD.chaseShooterPos = movement.UpdateSelfChasePosition(TransformLookup, componentData.Shooter);
		sTD.radius = config.Radius.Calculate();
		sTD.isIgnoreWall = movement.IsIgnoreWall;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1011DisintegrationRayData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		BufferAccessor<DisintegrationRayBodyPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__DisintegrationRayBodyPoint_RW_BufferTypeHandle);
		BufferAccessor<DisintegrationRayMousePoint> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__DisintegrationRayMousePoint_RW_BufferTypeHandle);
		BufferAccessor<DisintegrationRayHoverFallGroundPoint> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__DisintegrationRayHoverFallGroundPoint_RW_BufferTypeHandle);
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
				ref Spell1011DisintegrationRayData rayData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1011DisintegrationRayData>(nativeArrayPtr5, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, i);
				ref PhysicsVelocity physicsVelocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr8, i);
				DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints = bufferAccessor[i];
				DynamicBuffer<DisintegrationRayMousePoint> mouseRandomPoints = bufferAccessor2[i];
				DynamicBuffer<DisintegrationRayHoverFallGroundPoint> fallHoverGroundPoints = bufferAccessor3[i];
				Execute(entity, ref config, ref movement, ref componentData, ref rayData, ref transform, ref elementEffect, ref physicsVelocity, rayBodyPoints, mouseRandomPoints, fallHoverGroundPoints, chunkIndexInQuery);
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
					ref Spell1011DisintegrationRayData rayData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1011DisintegrationRayData>(nativeArrayPtr5, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, nextRangeBegin);
					ref PhysicsVelocity physicsVelocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr8, nextRangeBegin);
					DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<DisintegrationRayMousePoint> mouseRandomPoints2 = bufferAccessor2[nextRangeBegin];
					DynamicBuffer<DisintegrationRayHoverFallGroundPoint> fallHoverGroundPoints2 = bufferAccessor3[nextRangeBegin];
					Execute(entity2, ref config2, ref movement2, ref componentData2, ref rayData2, ref transform2, ref elementEffect2, ref physicsVelocity2, rayBodyPoints2, mouseRandomPoints2, fallHoverGroundPoints2, chunkIndexInQuery);
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
				ref Spell1011DisintegrationRayData rayData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1011DisintegrationRayData>(nativeArrayPtr5, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, j);
				ref PhysicsVelocity physicsVelocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr8, j);
				DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints3 = bufferAccessor[j];
				DynamicBuffer<DisintegrationRayMousePoint> mouseRandomPoints3 = bufferAccessor2[j];
				DynamicBuffer<DisintegrationRayHoverFallGroundPoint> fallHoverGroundPoints3 = bufferAccessor3[j];
				Execute(entity3, ref config3, ref movement3, ref componentData3, ref rayData3, ref transform3, ref elementEffect3, ref physicsVelocity3, rayBodyPoints3, mouseRandomPoints3, fallHoverGroundPoints3, chunkIndexInQuery);
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
				ref Spell1011DisintegrationRayData rayData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1011DisintegrationRayData>(nativeArrayPtr5, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, k);
				ref PhysicsVelocity physicsVelocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr8, k);
				DynamicBuffer<DisintegrationRayBodyPoint> rayBodyPoints4 = bufferAccessor[k];
				DynamicBuffer<DisintegrationRayMousePoint> mouseRandomPoints4 = bufferAccessor2[k];
				DynamicBuffer<DisintegrationRayHoverFallGroundPoint> fallHoverGroundPoints4 = bufferAccessor3[k];
				Execute(entity4, ref config4, ref movement4, ref componentData4, ref rayData4, ref transform4, ref elementEffect4, ref physicsVelocity4, rayBodyPoints4, mouseRandomPoints4, fallHoverGroundPoints4, chunkIndexInQuery);
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
