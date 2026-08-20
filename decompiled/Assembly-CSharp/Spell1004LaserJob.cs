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

[CompilerGenerated]
[BurstCompile]
public struct Spell1004LaserJob : IJobEntity, IJobChunk
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct RaycastEntityHitDistanceComparer : IComparer<(Unity.Physics.RaycastHit, SpellTools.HitType)>
	{
		public int Compare((Unity.Physics.RaycastHit, SpellTools.HitType) a, (Unity.Physics.RaycastHit, SpellTools.HitType) b)
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

	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<Spell1004LaserData> __Spell1004LaserData_RW_ComponentTypeHandle;

			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public BufferTypeHandle<LaserBodyPoint> __LaserBodyPoint_RW_BufferTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Spell1004LaserData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1004LaserData>();
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__LaserBodyPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<LaserBodyPoint>();
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Spell1004LaserData_RW_ComponentTypeHandle.Update(ref state);
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__LaserBodyPoint_RW_BufferTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1004LaserData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LaserBodyPoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
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
		public void Run(ref Spell1004LaserJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1004LaserJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1004LaserJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1004LaserJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1004LaserJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1004LaserJob job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractionHitEntitiesLookup;

	[ReadOnly]
	public PlayerController_Dots PlayerCtrller;

	[ReadOnly]
	public ComponentLookup<SpellRefractionData> SpellRefractionLookup;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public GlobalRandom Random;

	public Entity VfxEntity;

	public Entity GlobalParticleSystemBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(Entity entity, ref Spell1004LaserData data, SpellAspect spellAspect, DynamicBuffer<LaserBodyPoint> lineBuffer, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
	{
		ref SpellMovementComponentData valueRW = ref spellAspect.Movement.ValueRW;
		ref SpellConfigComponentData valueRW2 = ref spellAspect.Config.ValueRW;
		ref SpellComponentData valueRW3 = ref spellAspect.Data.ValueRW;
		ref LocalTransform valueRW4 = ref spellAspect.Transform.ValueRW;
		NativeList<float3> touchGList = new NativeList<float3>(Allocator.Temp);
		NativeList<float4> effNodeList = new NativeList<float4>(Allocator.Temp);
		if (!data.IsInitialize)
		{
			NativeList<Entity> hitList = new NativeList<Entity>(Allocator.Temp);
			NativeList<(float3, float3)> hitDataList = new NativeList<(float3, float3)>(Allocator.Temp);
			data.SpellSpeed = valueRW.Speed;
			lineBuffer.Insert(0, new LaserBodyPoint
			{
				Value = valueRW4.Position
			});
			NativeList<float> heightList = new NativeList<float>(Allocator.Temp);
			float num = data.SpellSpeed * 0.05f;
			float3 stepVelocity = float3.zero;
			float3 stepDirection = valueRW.Direction;
			float3 normalStepPos = default(float3);
			float remainMaxDistance = valueRW.Speed;
			float3 @float = valueRW.Direction;
			valueRW.Speed = 0f;
			data.totalTime = 0.2f + valueRW2.HoverDuration;
			int count;
			bool isRefract = HaveRefractionCheck(entity, in valueRW2, out count);
			DynamicBuffer<SpellRefractionHitEntities> refractHitteds = default(DynamicBuffer<SpellRefractionHitEntities>);
			if (isRefract)
			{
				refractHitteds = SpellRefractionHitEntitiesLookup[entity];
				refractHitteds.Clear();
			}
			int num2 = 0;
			switch (valueRW.Type)
			{
			case SpellSpecialMovementType.Normal:
				if (valueRW.IsFallSpell)
				{
					NormalFallLineGenerate(ref stepVelocity, ref normalStepPos, ref stepDirection, ref valueRW, in valueRW2, ref data, ref touchGList, ref effNodeList, ref heightList, ref lineBuffer, in isRefract, ref count, ref refractHitteds, in spellAspect.Data.ValueRO, in valueRW2.ShooterType);
					break;
				}
				while (remainMaxDistance > 0.01f)
				{
					num2++;
					if (num2 > 300)
					{
						break;
					}
					LaserBodyPoint laserBodyPoint = lineBuffer[0];
					StraightGoLinePosStep(ref stepVelocity, ref normalStepPos, in laserBodyPoint.Value, in valueRW, remainMaxDistance);
					bool lineEnd3 = false;
					laserBodyPoint = lineBuffer[0];
					CheckWallHitAndEnemyHitStep(ref stepVelocity, ref normalStepPos, ref stepDirection, in laserBodyPoint.Value, ref valueRW, ref valueRW2, in entity, ref lineEnd3, ref hitList, ref hitDataList, ref effNodeList, ref count);
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = lineBuffer[0].Value + stepVelocity
					});
					if (lineEnd3)
					{
						break;
					}
					remainMaxDistance -= math.length(stepVelocity);
				}
				break;
			case SpellSpecialMovementType.Rotation:
			{
				if (valueRW.IsFallSpell)
				{
					GetFallHeightList(ref heightList, 8, -2.5f);
					int num3 = 15;
					float num4 = -5f;
					float num5 = (0f - num4) / (float)num3;
					lineBuffer.ElementAt(0).Value.z = num4;
					for (int l = 0; l < num3 - 1; l++)
					{
						RotationGoLinePosStep(ref stepVelocity, ref normalStepPos, lineBuffer[0].Value, ref stepDirection, ref valueRW);
						normalStepPos.z += num5;
						lineBuffer.Insert(0, new LaserBodyPoint
						{
							Value = normalStepPos
						});
					}
					AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in normalStepPos);
					if (isRefract)
					{
						float refractRadius = valueRW2.Radius.Calculate();
						RotationFallRefraction(ref valueRW, in refractRadius, ref stepDirection, ref lineBuffer, ref count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
					}
					if (valueRW.ReboundCount <= 0)
					{
						break;
					}
					for (int m = 0; m < valueRW.ReboundCount; m++)
					{
						for (int n = 1; n < heightList.Length; n++)
						{
							RotationGoLinePosStep(ref stepVelocity, ref normalStepPos, lineBuffer[0].Value, ref stepDirection, ref valueRW, 6f);
							normalStepPos.z = heightList[n];
							lineBuffer.Insert(0, new LaserBodyPoint
							{
								Value = normalStepPos
							});
						}
						touchGList.Add(in normalStepPos);
						float4 value = new float4(normalStepPos, 90f);
						effNodeList.Add(in value);
						if (isRefract)
						{
							float refractRadius = valueRW2.Radius.Calculate();
							RotationFallRefraction(ref valueRW, in refractRadius, ref stepDirection, ref lineBuffer, ref count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
					}
					break;
				}
				for (int num6 = 0; num6 < 36; num6++)
				{
					RotationGoLinePosStep(ref stepVelocity, ref normalStepPos, lineBuffer[0].Value, ref stepDirection, ref valueRW);
					bool lineEnd2 = false;
					LaserBodyPoint laserBodyPoint = lineBuffer[0];
					CheckHitEnemyByStepVelocity(in laserBodyPoint.Value, ref normalStepPos, ref valueRW2, in spellAspect.Entity, ref hitList, ref hitDataList, ref lineEnd2, ref stepVelocity, ref stepDirection, ref count, ref effNodeList, out var isPreEnded);
					if (isPreEnded)
					{
						AddHitEffectNode(ref effNodeList, normalStepPos, -stepVelocity.xy);
					}
					valueRW.Direction = stepDirection;
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = lineBuffer[0].Value + stepVelocity
					});
					if (lineEnd2)
					{
						break;
					}
				}
				break;
			}
			case SpellSpecialMovementType.ChaseEnemy:
			{
				GetValidChaseEnemyPosition(ref valueRW, in valueRW2, in valueRW4, out var hasTarget, out var targetPosition);
				if (valueRW.IsFallSpell)
				{
					if (hasTarget)
					{
						float3 float2 = Random.random.NextFloat3Direction() * valueRW2.Scatter * 0.06f + PlayerCtrller.mousePosition;
						stepDirection = (valueRW.Direction = math.normalize(targetPosition - float2));
						float3 v7 = new float3(float2.xy, -8f);
						lineBuffer.ElementAt(0).Value = v7;
						float3 v8 = new float3(float2.xy, -5f);
						float3 float4 = stepDirection * valueRW.ChaseRotateSpeed * 0.15f;
						float3 pos2 = targetPosition;
						bool flag = true;
						if (math.length(float4) < math.length(targetPosition - float2))
						{
							pos2 = float2 + float4;
							flag = false;
						}
						pos2.z = 0f;
						for (int num10 = 1; num10 < 15; num10++)
						{
							normalStepPos = DTool.QuadraticBezierCurve(in v7, in v8, in pos2, 1f / 15f * (float)num10);
							lineBuffer.Insert(0, new LaserBodyPoint
							{
								Value = normalStepPos
							});
						}
						AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in pos2);
						bool isNotRefracted3 = isRefract;
						if (isNotRefracted3)
						{
							GetFallHeightList(ref heightList, 7, -3f);
							float refractRadius = valueRW2.Radius.Calculate();
							NormalFallRefraction(ref isNotRefracted3, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
						if (valueRW.ReboundCount <= 0)
						{
							break;
						}
						if (!flag)
						{
							valueRW.ReboundCount--;
							v7 = lineBuffer[0].Value;
							v8 = (targetPosition + v7) / 2f;
							v8.z = -4f;
							for (int num11 = 1; num11 < 20; num11++)
							{
								normalStepPos = DTool.QuadraticBezierCurve(in v7, in v8, in targetPosition, (float)num11 / 20f);
								lineBuffer.Insert(0, new LaserBodyPoint
								{
									Value = normalStepPos
								});
								valueRW4.Position = normalStepPos;
							}
							AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in targetPosition);
							if (isNotRefracted3)
							{
								float refractRadius = valueRW2.Radius.Calculate();
								NormalFallRefraction(ref isNotRefracted3, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
							}
						}
						if (valueRW.ReboundCount <= 0)
						{
							break;
						}
						float3 zero2 = float3.zero;
						for (int num12 = 0; num12 < valueRW.ReboundCount; num12++)
						{
							v7 = lineBuffer[0].Value;
							v8 = v7 + stepDirection * 2f;
							v8.z = -4f;
							stepDirection = DTool.RotateDir(stepDirection, 50f);
							zero2 = targetPosition + stepDirection * 2f;
							zero2.z = -4f;
							for (int num13 = 1; num13 < 20; num13++)
							{
								normalStepPos = DTool.CubicBezierCurve_Uniform(in v7, in v8, in zero2, in targetPosition, (float)num13 / 20f);
								lineBuffer.Insert(0, new LaserBodyPoint
								{
									Value = normalStepPos
								});
								valueRW4.Position = normalStepPos;
							}
							AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in targetPosition);
							if (isNotRefracted3)
							{
								float refractRadius = valueRW2.Radius.Calculate();
								NormalFallRefraction(ref isNotRefracted3, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
							}
							valueRW.Direction = stepDirection;
						}
					}
					else
					{
						NormalFallLineGenerate(ref stepVelocity, ref normalStepPos, ref stepDirection, ref valueRW, in valueRW2, ref data, ref touchGList, ref effNodeList, ref heightList, ref lineBuffer, in isRefract, ref count, ref refractHitteds, in spellAspect.Data.ValueRO, in valueRW2.ShooterType);
					}
					break;
				}
				while (remainMaxDistance > 0.01f)
				{
					num2++;
					if (num2 > 300)
					{
						break;
					}
					ChaseEnemyLinePosStep(ref stepVelocity, ref normalStepPos, ref stepDirection, lineBuffer[0].Value, num, in remainMaxDistance, ref valueRW, in valueRW2, in hasTarget, in targetPosition);
					bool lineEnd5 = false;
					CheckWallHitAndEnemyHitStep(ref stepVelocity, ref normalStepPos, ref stepDirection, in lineBuffer.ElementAt(0).Value, ref valueRW, ref valueRW2, in spellAspect.Entity, ref lineEnd5, ref hitList, ref hitDataList, ref effNodeList, ref count);
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = lineBuffer[0].Value + stepVelocity
					});
					if (lineEnd5)
					{
						break;
					}
					remainMaxDistance -= math.length(stepVelocity);
				}
				break;
			}
			case SpellSpecialMovementType.ChaseMouse:
				if (valueRW.IsFallSpell)
				{
					bool isNotRefracted2 = isRefract;
					if (spellAspect.Data.ValueRO.IsSplitSpell)
					{
						float3 v2 = lineBuffer[0].Value + stepDirection;
						v2.z = -4f;
						float3 source = Random.random.NextFloat3Direction() * valueRW2.Scatter * 0.06f + PlayerCtrller.mousePosition;
						float3 pos = DTool.IgnoreZPosition(in source);
						float3 v3 = lineBuffer[0].Value;
						for (int num7 = 1; num7 < 20; num7++)
						{
							normalStepPos = DTool.QuadraticBezierCurve(in v3, in v2, in pos, (float)num7 / 20f);
							lineBuffer.Insert(0, new LaserBodyPoint
							{
								Value = normalStepPos
							});
						}
						stepDirection = math.normalizesafe(pos - v3);
						AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in pos);
						if (isNotRefracted2)
						{
							GetFallHeightList(ref heightList, 7, -3f);
							float refractRadius = valueRW2.Radius.Calculate();
							NormalFallRefraction(ref isNotRefracted2, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
					}
					else
					{
						LaserBodyPoint laserBodyPoint = lineBuffer[0];
						ref float3 value2 = ref laserBodyPoint.Value;
						float dirTan = data.SpellSpeed / valueRW.CurrentFallSpeed;
						float3 source = valueRW.Direction;
						StraightGoFallGroundStep(ref stepVelocity, ref normalStepPos, in value2, dirTan, in source);
						AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in normalStepPos);
						if (isNotRefracted2)
						{
							GetFallHeightList(ref heightList, 7, -3f);
							float refractRadius = valueRW2.Radius.Calculate();
							NormalFallRefraction(ref isNotRefracted2, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
					}
					for (int num8 = 0; num8 < valueRW.ReboundCount; num8++)
					{
						float3 v4 = lineBuffer[0].Value + stepDirection;
						v4.z = -4f;
						float3 source = Random.random.NextFloat3Direction() * valueRW2.Scatter * 0.06f + PlayerCtrller.mousePosition;
						float3 v5 = DTool.IgnoreZPosition(in source);
						float3 v6 = lineBuffer[0].Value;
						for (int num9 = 1; num9 < 20; num9++)
						{
							normalStepPos = DTool.QuadraticBezierCurve(in v6, in v4, in v5, (float)num9 / 20f);
							lineBuffer.Insert(0, new LaserBodyPoint
							{
								Value = normalStepPos
							});
						}
						stepDirection = math.normalizesafe(v5 - v6);
						AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in v5);
						if (isNotRefracted2)
						{
							float refractRadius = valueRW2.Radius.Calculate();
							NormalFallRefraction(ref isNotRefracted2, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
					}
					valueRW.Direction = stepDirection;
					break;
				}
				while (remainMaxDistance > 0.01f)
				{
					num2++;
					if (num2 > 300)
					{
						break;
					}
					ref float3 mousePosition = ref PlayerCtrller.mousePosition;
					LaserBodyPoint laserBodyPoint = lineBuffer[0];
					float3 end = DTool.IgnoreZDir(in mousePosition, in laserBodyPoint.Value);
					float t = math.clamp(valueRW.ChaseMouseLerpSpeed * data.SpellSpeed * 0.08f, 0f, 1f);
					@float = math.lerp(@float, end, t);
					if (math.length(@float) < 0.2f && math.length(lineBuffer[0].Value - PlayerCtrller.mousePosition) < 0.5f)
					{
						break;
					}
					stepVelocity = @float * num;
					stepDirection = math.normalize(@float);
					normalStepPos = lineBuffer[0].Value + stepVelocity;
					bool lineEnd4 = false;
					if (CheckHitWallStep(ref stepVelocity, ref normalStepPos, ref stepDirection, lineBuffer[0].Value, ref valueRW, ref lineEnd4, out var wallHitType, out var wallHit))
					{
						@float = stepDirection;
					}
					laserBodyPoint = lineBuffer[0];
					CheckHitEnemyByStepVelocity(in laserBodyPoint.Value, ref normalStepPos, ref valueRW2, in spellAspect.Entity, ref hitList, ref hitDataList, ref lineEnd4, ref stepVelocity, ref stepDirection, ref count, ref effNodeList, out var isPreEnded2);
					AddHitEffectNodeInStepHit(in isPreEnded2, in wallHitType, ref effNodeList, in normalStepPos, in stepVelocity, in wallHit);
					valueRW.Direction = stepDirection;
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = lineBuffer[0].Value + stepVelocity
					});
					if (lineEnd4)
					{
						break;
					}
					remainMaxDistance -= math.length(stepVelocity);
				}
				break;
			case SpellSpecialMovementType.ChaseOwner:
			{
				float3 position = valueRW.UpdateSelfChasePosition(TransformLookup, valueRW3.Shooter);
				if (valueRW.IsFallSpell)
				{
					float3 from = new float3(lineBuffer[0].Value.xy, -8f);
					lineBuffer.ElementAt(0).Value = from;
					float3 v = new float3(lineBuffer[0].Value.xy, -4f);
					float3 to = DTool.IgnoreZPosition(in position);
					float3 zero = float3.zero;
					stepDirection = DTool.IgnoreZDir(in to, in from);
					for (int i = 1; i < 20; i++)
					{
						normalStepPos = DTool.QuadraticBezierCurve(in from, in v, in to, (float)i / 20f);
						lineBuffer.Insert(0, new LaserBodyPoint
						{
							Value = normalStepPos
						});
					}
					AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in to);
					bool isNotRefracted = isRefract;
					if (isNotRefracted)
					{
						float refractRadius = valueRW2.Radius.Calculate();
						NormalFallRefraction(ref isNotRefracted, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
					}
					for (int j = 0; j < valueRW.ReboundCount; j++)
					{
						from = lineBuffer[0].Value;
						v = stepDirection * 4f + lineBuffer[0].Value;
						v.z = -4f;
						stepDirection = DTool.RotateDir(stepDirection, -90f);
						zero = stepDirection * 4f + lineBuffer[0].Value;
						zero.z = -4f;
						for (int k = 1; k < 20; k++)
						{
							normalStepPos = DTool.CubicBezierCurve(in from, in v, in zero, in to, (float)k / 20f);
							lineBuffer.Insert(0, new LaserBodyPoint
							{
								Value = normalStepPos
							});
						}
						AddGroundPoint(ref lineBuffer, ref touchGList, ref effNodeList, in to);
						if (isNotRefracted)
						{
							float refractRadius = valueRW2.Radius.Calculate();
							NormalFallRefraction(ref isNotRefracted, in refractRadius, ref stepDirection, ref lineBuffer, in count, ref refractHitteds, in heightList, ref effNodeList, ref touchGList, valueRW2.ShooterType);
						}
					}
					break;
				}
				while (remainMaxDistance > 0.01f)
				{
					num2++;
					if (num2 > 300)
					{
						break;
					}
					float3 target = position - lineBuffer[0].Value;
					float lerp = Mathf.Abs(Mathf.Abs(GetDegreeIgnorZ(valueRW.Direction, target)) - 90f) / 90f;
					float3 source = valueRW.Direction;
					DirMoveTowardsCounterClockwise3d(in source, in target, num * valueRW.ChaseRotateSpeed, out stepDirection);
					stepVelocity = stepDirection * DTool.Lerp(0.6f, 1.4f, lerp) * num;
					normalStepPos = lineBuffer[0].Value + stepVelocity;
					bool lineEnd = false;
					CheckWallHitAndEnemyHitStep(ref stepVelocity, ref normalStepPos, ref stepDirection, in lineBuffer.ElementAt(0).Value, ref valueRW, ref valueRW2, in spellAspect.Entity, ref lineEnd, ref hitList, ref hitDataList, ref effNodeList, ref count);
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = lineBuffer[0].Value + stepVelocity
					});
					if (lineEnd)
					{
						break;
					}
					remainMaxDistance -= math.length(stepVelocity);
				}
				break;
			}
			}
			data.IsLaserHit = hitList.Length > 0;
			if (data.IsLaserHit)
			{
				data.totalTime = 0.2f;
			}
			RayHitGlobalEffect(in hitDataList, in valueRW2, chunkIndex);
			RayHitDamageApply(in hitList, in hitDataList, entity, in valueRW2, in valueRW, in valueRW4, in elementEffect, in valueRW3, chunkIndex);
			foreach (float3 item in touchGList)
			{
				float3 touchGroundPos = item;
				TouchGroundExplosion(in valueRW2, in spellAspect, in valueRW3, in valueRW, in elementEffect, in touchGroundPos, chunkIndex);
			}
			HitNodeEffectApply(ref effNodeList, in valueRW2, chunkIndex);
			data.IsInitialize = true;
			data.IsSetLinePos = true;
			valueRW4.Position = new Vector3(lineBuffer[0].Value.x, lineBuffer[0].Value.y, -0.3f);
			valueRW.Gravity = 0f;
			valueRW.CurrentFallSpeed = 0f;
		}
		if (valueRW2.HoverDuration > 0f && !data.IsLaserHit && !data.IsHoverHit && !valueRW.IsFallSpell)
		{
			NativeList<Entity> hitEntities = new NativeList<Entity>(Allocator.Temp);
			data.IsHoverHit = HoverHitCheck(lineBuffer, valueRW2.ShooterType, ref hitEntities, out var hitUnit);
			foreach (Entity item2 in hitEntities)
			{
				Entity target2 = item2;
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in valueRW2, in valueRW, in valueRW4, in elementEffect, in valueRW3, out var info, CostRefraction: false);
				CMD.TryAttackEntity(chunkIndex, in target2, in info, in UnitPropertyLookup, in SpellConfigLookup);
			}
			if (data.IsHoverHit)
			{
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in valueRW2, in valueRW, in valueRW4, in elementEffect, in valueRW3, out var info2, CostRefraction: false);
				info2.SetKnockbackForceIgnoreZBySpell(hitUnit.Item3);
				info2.spell.HitPosition = hitUnit.Item2;
				CMD.TryAttackEntity(chunkIndex, in hitUnit.Item1, in info2, in UnitPropertyLookup, in SpellConfigLookup);
				data.IsSetLinePos = true;
				data.totalTime = math.max(valueRW2.DurationTimer + 0.1f, 0.2f);
			}
		}
		if (valueRW2.DurationTimer > data.totalTime)
		{
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			SetBubbleVFX(in lineBuffer, in valueRW2, chunkIndex);
			valueRW4.Position = lineBuffer[0].Value;
		}
	}

	[BurstCompile]
	private void SetBubbleVFX(in DynamicBuffer<LaserBodyPoint> bodyPoints, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 1; i < bodyPoints.Length; i++)
		{
			LaserBodyPoint laserBodyPoint = bodyPoints[i];
			float3 start = DTool.GetLayerPosition(in laserBodyPoint.Value, LayerCorrectType.Coordinate);
			laserBodyPoint = bodyPoints[i - 1];
			float3 end = DTool.GetLayerPosition(in laserBodyPoint.Value, LayerCorrectType.Coordinate);
			start += bodyPoints[i].Value;
			end += bodyPoints[i - 1].Value;
			int num = (int)(math.distance(start, end) * 10f);
			for (int j = 0; j < num; j++)
			{
				float3 xyz = DTool.Lerp(in start, in end, (float)j / (float)(num - 1));
				float4 posAndColorType = new float4(xyz, (float)config.ColorType);
				CMD.AppendToBuffer(chunkIndex, VfxEntity, new Spell1004LaserTrailBED
				{
					posAndColorType = posAndColorType
				});
			}
		}
	}

	[BurstCompile]
	private bool RaycastCastHit(float3 startOnLine, float3 endOnLine, UnitType shooterType, out NativeList<(Unity.Physics.RaycastHit, SpellTools.HitType)> hitList)
	{
		hitList = default(NativeList<(Unity.Physics.RaycastHit, SpellTools.HitType)>);
		if (SpellTools.GetAttackablesInRaycast(in startOnLine, in endOnLine, in shooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, out var hitList2))
		{
			hitList = new NativeList<(Unity.Physics.RaycastHit, SpellTools.HitType)>(Allocator.Temp);
			foreach (Unity.Physics.RaycastHit item in hitList2)
			{
				Entity target = item.Entity;
				SpellTools.HitType entityHitType = SpellTools.GetEntityHitType(in target, in shooterType, in UnitPropertyLookup, in SpellConfigLookup);
				if ((uint)(entityHitType - 3) <= 3u)
				{
					(Unity.Physics.RaycastHit, SpellTools.HitType) value = (item, entityHitType);
					hitList.Add(in value);
				}
			}
			return true;
		}
		return false;
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
	private void RayHitGlobalEffect(in NativeList<(float3 dir, float3 pos)> hitDataList, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		foreach (var hitData in hitDataList)
		{
			(float3, float3) current = hitData;
			config.ColorType.ColorEnumToString(out var result);
			float3 layerPosition = DTool.GetLayerPosition(in current.Item2, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1004_Hit_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(current.Item2) + layerPosition;
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}
	}

	[BurstCompile]
	private void RayHitDamageApply(in NativeList<Entity> hitList, in NativeList<(float3 dir, float3 pos)> hitDataList, Entity spellEntity, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, in SpellComponentData data, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < hitList.Length; i++)
		{
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in data, out var info, CostRefraction: false);
			info.SetKnockbackForceIgnoreZBySpell(hitDataList[i].dir);
			info.spell.HitPosition = hitDataList[i].pos;
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity target = hitList[i];
			cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
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
	private void CheckWallHitAndEnemyHitStep(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, in float3 prevPos, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, in Entity spellEntity, ref bool lineEnd, ref NativeList<Entity> hitList, ref NativeList<(float3 dir, float3 pos)> hitDataList, ref NativeList<float4> hitEffectNode, ref int refractionCount)
	{
		CheckHitWallStep(ref stepVelocity, ref normalStepPos, ref stepDirection, prevPos, ref movement, ref lineEnd, out var wallHitType, out var wallHit);
		CheckHitEnemyByStepVelocity(in prevPos, ref normalStepPos, ref config, in spellEntity, ref hitList, ref hitDataList, ref lineEnd, ref stepVelocity, ref stepDirection, ref refractionCount, ref hitEffectNode, out var isPreEnded);
		AddHitEffectNodeInStepHit(in isPreEnded, in wallHitType, ref hitEffectNode, in normalStepPos, in stepVelocity, in wallHit);
		movement.Direction = stepDirection;
	}

	[BurstCompile]
	private bool CheckHitWallStep(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, float3 fromPos, ref SpellMovementComponentData movement, ref bool lineEnd, out int wallHitType, out Unity.Physics.RaycastHit wallHit)
	{
		if (movement.IsIgnoreWall)
		{
			wallHitType = 0;
			wallHit = default(Unity.Physics.RaycastHit);
			return false;
		}
		if (CheckHitWallByStepVelocity(in fromPos, ref normalStepPos, ref stepVelocity, out wallHit))
		{
			if (movement.ReboundCount <= 0)
			{
				wallHitType = 1;
				lineEnd = true;
			}
			else
			{
				movement.ReboundCount--;
				wallHitType = 2;
				stepDirection = math.reflect(movement.Direction, wallHit.SurfaceNormal);
			}
			return true;
		}
		wallHitType = 0;
		return false;
	}

	[BurstCompile]
	private void CheckHitEnemyByStepVelocity(in float3 previousPos, ref float3 normalStepPos, ref SpellConfigComponentData config, in Entity spellEntity, ref NativeList<Entity> hitList, ref NativeList<(float3 dir, float3 pos)> hitDataList, ref bool lineEnd, ref float3 stepVelocity, ref float3 stepDirection, ref int refractionCount, ref NativeList<float4> hitEffectNode, out bool isPreEnded)
	{
		isPreEnded = false;
		if (RaycastCastHit(previousPos, normalStepPos, config.ShooterType, out var hitList2))
		{
			hitList2.Sort<(Unity.Physics.RaycastHit, SpellTools.HitType), RaycastEntityHitDistanceComparer>(default(RaycastEntityHitDistanceComparer));
			for (int i = 0; i < hitList2.Length; i++)
			{
				if (hitList2[i].Item1.Fraction <= 0.0001f && hitDataList.Length > 0)
				{
					continue;
				}
				float3 item = hitList2[i].Item1.Fraction * stepVelocity + previousPos;
				if (!hitList.Contains(hitList2[i].Item1.Entity))
				{
					Entity value = hitList2[i].Item1.Entity;
					hitList.Add(in value);
					(float3, float3) value2 = (stepDirection, item);
					hitDataList.Add(in value2);
				}
				if (hitList2[i].Item2 == SpellTools.HitType.Brittleness || hitList2[i].Item2 == SpellTools.HitType.Butterfly)
				{
					continue;
				}
				if (refractionCount > 0 && hitList2[i].Item2 == SpellTools.HitType.Unit)
				{
					DynamicBuffer<SpellRefractionHitEntities> hitEntities = SpellRefractionHitEntitiesLookup[spellEntity];
					float3 hitPosition = hitList2[i].Item1.Position;
					Entity value = hitList2[i].Item1.Entity;
					if (RefractionDirectionSet(ref stepDirection, in hitPosition, ref config, hitEntities, in value))
					{
						stepVelocity *= hitList2[i].Item1.Fraction;
						normalStepPos = previousPos + stepVelocity;
						refractionCount--;
						isPreEnded = true;
						lineEnd = false;
						break;
					}
				}
				if (config.Penetrate.Calculate() <= 0)
				{
					lineEnd = true;
					isPreEnded = true;
					stepVelocity *= hitList2[i].Item1.Fraction;
					normalStepPos = previousPos + stepVelocity;
					break;
				}
				config.Penetrate.CostPenetrateValue();
			}
		}
		hitList2.Dispose();
	}

	private bool HoverHitCheck(DynamicBuffer<LaserBodyPoint> linePoints, UnitType shooterType, ref NativeList<Entity> hitEntities, out (Entity ett, float3 pos, float3 dir) hitUnit)
	{
		hitUnit = default((Entity, float3, float3));
		for (int num = linePoints.Length - 1; num > 0; num--)
		{
			if (CheckHitByPos(linePoints[num - 1].Value, linePoints[num].Value, shooterType, ref hitEntities, out hitUnit))
			{
				for (int i = 0; i < num; i++)
				{
					linePoints.RemoveAt(0);
				}
				linePoints.Insert(0, new LaserBodyPoint
				{
					Value = hitUnit.pos
				});
				return true;
			}
		}
		return false;
	}

	private bool CheckHitByPos(float3 startP, float3 endP, UnitType shooterType, ref NativeList<Entity> hitEntities, out (Entity ett, float3 pos, float3 dir) hitResult)
	{
		hitResult = default((Entity, float3, float3));
		float3 @float = endP - startP;
		if (RaycastCastHit(startP, endP, shooterType, out var hitList))
		{
			hitList.Sort<(Unity.Physics.RaycastHit, SpellTools.HitType), RaycastEntityHitDistanceComparer>(default(RaycastEntityHitDistanceComparer));
			for (int i = 0; i < hitList.Length; i++)
			{
				if (hitList[i].Item2 == SpellTools.HitType.Brittleness || hitList[i].Item2 == SpellTools.HitType.Butterfly)
				{
					Entity value = hitList[i].Item1.Entity;
					hitEntities.Add(in value);
					continue;
				}
				if (hitList[i].Item1.Fraction <= 0.0001f)
				{
					hitResult.pos = startP;
				}
				else
				{
					hitResult.pos = hitList[i].Item1.Fraction * @float + startP;
				}
				hitResult.ett = hitList[i].Item1.Entity;
				hitResult.dir = math.normalizesafe(endP - startP);
				return true;
			}
		}
		return false;
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
		else
		{
			AddHitEffectNode(ref hitEffectNode, normalStepPos, -stepVelocity.xy);
		}
	}

	[BurstCompile]
	private void StraightGoLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, in float3 fromPos, in SpellMovementComponentData movement, float remainMaxDistance)
	{
		stepVelocity = movement.Direction * remainMaxDistance;
		normalStepPos = fromPos + stepVelocity;
	}

	[BurstCompile]
	private void NormalFallLineGenerate(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, ref SpellMovementComponentData movement, in SpellConfigComponentData config, ref Spell1004LaserData data, ref NativeList<float3> touchGroundPos, ref NativeList<float4> hitEffectNode, ref NativeList<float> heightList, ref DynamicBuffer<LaserBodyPoint> lineBuffer, in bool isRefract, ref int refractCount, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in SpellComponentData spellData, in UnitType shooterType)
	{
		bool isNotRefracted = isRefract;
		float distancePerStep = 0.2f;
		float distance = 4f;
		GetFallHeightFixByDistance(ref heightList, in distance, ref distancePerStep);
		if (spellData.IsSplitSpell)
		{
			LaserBodyPoint laserBodyPoint;
			for (int i = 1; i < heightList.Length; i++)
			{
				laserBodyPoint = default(LaserBodyPoint);
				laserBodyPoint.Value = new float3((lineBuffer[0].Value + stepDirection * distancePerStep).xy, heightList[i]);
				LaserBodyPoint elem = laserBodyPoint;
				lineBuffer.Insert(0, elem);
			}
			float4 value = new float4(lineBuffer[0].Value, 90f);
			hitEffectNode.Add(in value);
			laserBodyPoint = lineBuffer[0];
			touchGroundPos.Add(in laserBodyPoint.Value);
			if (isNotRefracted)
			{
				distance = config.Radius.Calculate();
				NormalFallRefraction(ref isNotRefracted, in distance, ref stepDirection, ref lineBuffer, in refractCount, ref refractHitteds, in heightList, ref hitEffectNode, ref touchGroundPos, shooterType);
			}
		}
		else
		{
			LaserBodyPoint laserBodyPoint = lineBuffer[0];
			ref float3 value2 = ref laserBodyPoint.Value;
			float dirTan = data.SpellSpeed / movement.CurrentFallSpeed;
			float3 dirInXY = movement.Direction;
			StraightGoFallGroundStep(ref stepVelocity, ref normalStepPos, in value2, dirTan, in dirInXY);
			AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in normalStepPos);
		}
		if (isNotRefracted)
		{
			distance = config.Radius.Calculate();
			NormalFallRefraction(ref isNotRefracted, in distance, ref stepDirection, ref lineBuffer, in refractCount, ref refractHitteds, in heightList, ref hitEffectNode, ref touchGroundPos, shooterType);
		}
		if (movement.ReboundCount <= 0)
		{
			return;
		}
		for (int j = 0; j < movement.ReboundCount; j++)
		{
			LaserBodyPoint laserBodyPoint;
			for (int k = 1; k < heightList.Length; k++)
			{
				laserBodyPoint = default(LaserBodyPoint);
				laserBodyPoint.Value = new float3((lineBuffer[0].Value + stepDirection * distancePerStep).xy, heightList[k]);
				LaserBodyPoint elem2 = laserBodyPoint;
				lineBuffer.Insert(0, elem2);
			}
			float4 value = new float4(lineBuffer[0].Value, 90f);
			hitEffectNode.Add(in value);
			laserBodyPoint = lineBuffer[0];
			touchGroundPos.Add(in laserBodyPoint.Value);
			if (isNotRefracted)
			{
				distance = config.Radius.Calculate();
				NormalFallRefraction(ref isNotRefracted, in distance, ref stepDirection, ref lineBuffer, in refractCount, ref refractHitteds, in heightList, ref hitEffectNode, ref touchGroundPos, shooterType);
			}
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
	private void RotationGoLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, float3 previousPos, ref float3 stepDirection, ref SpellMovementComponentData movement, float angleSpeed = 10f)
	{
		movement.AroundAngle += angleSpeed;
		stepDirection = Tool2D.GetDir(movement.AroundAngle + 90f);
		normalStepPos = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
		normalStepPos.z = previousPos.z;
		stepVelocity = normalStepPos - previousPos;
	}

	[BurstCompile]
	private void ChaseEnemyLinePosStep(ref float3 stepVelocity, ref float3 normalStepPos, ref float3 stepDirection, float3 prevPos, float orgStepLength, in float remainMaxDistance, ref SpellMovementComponentData movement, in SpellConfigComponentData config, in bool hasTarget, in float3 targetPos)
	{
		if (hasTarget)
		{
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPos, in prevPos);
			stepDirection = DTool.DirMoveTowardsIgnoreZ(in source, in target, movement.ChaseRotateSpeed * orgStepLength);
			stepVelocity = stepDirection * orgStepLength;
			normalStepPos = prevPos + stepVelocity;
		}
		else
		{
			StraightGoLinePosStep(ref stepVelocity, ref normalStepPos, in prevPos, in movement, remainMaxDistance);
		}
	}

	[BurstCompile]
	private void HitNodeEffectApply(ref NativeList<float4> hitEffectNode, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
	{
		foreach (float4 item in hitEffectNode)
		{
			config.ColorType.ColorEnumToString(out var result);
			float valueToClamp = config.Radius.Calculate();
			valueToClamp = math.clamp(valueToClamp, 1f, float.MaxValue);
			float3 rootPosition = new float3(item.xyz);
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1004_Node_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(item.xyz) + layerPosition;
			globalParticleEmitParams.Size = valueToClamp;
			globalParticleEmitParams.Velocity = DTool.GetDir(item.w * (MathF.PI / 180f));
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}
	}

	[BurstCompile]
	private bool RefractionDirectionSet(ref float3 stepDirection, in float3 hitPosition, ref SpellConfigComponentData config, DynamicBuffer<SpellRefractionHitEntities> hitEntities, in Entity currentHitEntity)
	{
		float3 startPoint = DTool.IgnoreZPosition(in hitPosition);
		hitEntities.Add(new SpellRefractionHitEntities
		{
			Entity = currentHitEntity
		});
		NativeHashSet<Entity> ignoreEntities = new NativeHashSet<Entity>(hitEntities.Length, Allocator.Temp);
		foreach (SpellRefractionHitEntities item in hitEntities)
		{
			ignoreEntities.Add(item.Entity);
		}
		Entity target;
		float3 targetPosition;
		UnitProperty_Dots targetPpt;
		bool flag = CurrentRoomEntities.FindReflectionTarget(startPoint, config.ShooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
		if (!flag)
		{
			hitEntities.Clear();
			hitEntities.Add(new SpellRefractionHitEntities
			{
				Entity = currentHitEntity
			});
			ignoreEntities.Clear();
			ignoreEntities.Add(currentHitEntity);
			flag = CurrentRoomEntities.FindReflectionTarget(startPoint, config.ShooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
		}
		if (!flag)
		{
			return false;
		}
		float3 input = math.normalizesafe(targetPosition - hitPosition);
		stepDirection = input.IgnoreZ();
		return true;
	}

	[BurstCompile]
	private void GetValidChaseEnemyPosition(ref SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, out bool hasTarget, out float3 targetPosition, Entity? excludedEntity = null)
	{
		hasTarget = false;
		targetPosition = float3.zero;
		UnitProperty_Dots targetPpt;
		if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData, out var entityExists) && entityExists && !excludedEntity.HasValue)
		{
			targetPosition = componentData.Position;
			hasTarget = true;
		}
		else if (!CurrentRoomEntities.FindMinAngleTarget(transform.Position, movement.Direction, config.ShooterType, out movement.ChaseTarget, out targetPosition, out targetPpt))
		{
			if (excludedEntity.HasValue)
			{
				targetPosition = componentData.Position;
				hasTarget = true;
			}
		}
		else
		{
			hasTarget = true;
		}
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
	private bool HaveRefractionCheck(Entity entity, in SpellConfigComponentData config, out int count)
	{
		if (SpellRefractionLookup.HasComponent(entity))
		{
			count = SpellRefractionLookup.GetRefRO(entity).ValueRO.RemainCount + config.Int1 + config.Penetrate.Calculate();
			return count > 0;
		}
		if (config.Level > 1)
		{
			count = math.max(0, config.Int1 + config.Penetrate.Calculate());
			return count > 0;
		}
		count = 0;
		return false;
	}

	private void NormalFallRefraction(ref bool isNotRefracted, in float refractRadius, ref float3 stepDirection, ref DynamicBuffer<LaserBodyPoint> lineBuffer, in int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in NativeList<float> heightList, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPos, UnitType shooterType)
	{
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		LaserBodyPoint laserBodyPoint = lineBuffer[0];
		DTool.GetEnemyEntityInRange(in laserBodyPoint.Value, refractRadius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
		if (result.IsEmpty)
		{
			return;
		}
		isNotRefracted = false;
		for (int i = 0; i < refractRemain; i++)
		{
			DynamicBuffer<SpellRefractionHitEntities> hitEntities = refractHitteds;
			laserBodyPoint = lineBuffer[0];
			if (RefractionTransformGet(hitEntities, in result, in laserBodyPoint.Value, in shooterType, out var targetPos))
			{
				float3 @float = targetPos - lineBuffer[0].Value;
				stepDirection = math.normalizesafe(@float);
				float3 float2 = @float / (heightList.Length - 1);
				for (int j = 1; j < heightList.Length - 1; j++)
				{
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = new float3((lineBuffer[0].Value + float2).xy, heightList[j])
					});
				}
				if (!heightList.IsEmpty)
				{
					float2 xy = (lineBuffer[0].Value + float2).xy;
					NativeList<float> nativeList = heightList;
					float3 pos = new float3(xy, nativeList[nativeList.Length - 1]);
					AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in pos);
				}
				laserBodyPoint = lineBuffer[0];
				DTool.GetEnemyEntityInRange(in laserBodyPoint.Value, refractRadius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
				continue;
			}
			break;
		}
	}

	private void RotationFallRefraction(ref SpellMovementComponentData movement, in float refractRadius, ref float3 stepDirection, ref DynamicBuffer<LaserBodyPoint> lineBuffer, ref int refractRemain, ref DynamicBuffer<SpellRefractionHitEntities> refractHitteds, in NativeList<float> heightList, ref NativeList<float4> hitEffectNode, ref NativeList<float3> touchGroundPos, UnitType shooterType)
	{
		NativeList<Entity> currentHitEntities = new NativeList<Entity>(Allocator.Temp);
		while (refractRemain > 0)
		{
			currentHitEntities.Clear();
			LaserBodyPoint laserBodyPoint = lineBuffer[0];
			DTool.GetEnemyEntityInRange(in laserBodyPoint.Value, refractRadius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref currentHitEntities);
			if (currentHitEntities.IsEmpty)
			{
				return;
			}
			DynamicBuffer<SpellRefractionHitEntities> hitEntities = refractHitteds;
			laserBodyPoint = lineBuffer[0];
			if (RefractionTransformGet(hitEntities, in currentHitEntities, in laserBodyPoint.Value, in shooterType, out var targetPos))
			{
				refractRemain--;
				float3 @float = targetPos - movement.AroundCenter;
				float3 dir = lineBuffer[0].Value - movement.AroundCenter;
				float num = DTool.GetDegree(@float) - DTool.GetDegree(dir);
				float3 x = DTool.RotateDir(degree: (num != 0f) ? (math.abs(num) / num * 30f) : 30f, oldDir: math.normalizesafe(@float));
				x = math.normalizesafe(x) * movement.AroundRadius + movement.AroundCenter;
				float3 v = lineBuffer[0].Value;
				float3 v2 = (targetPos + v) / 2f;
				v2.z = -4f;
				float3 v3 = (targetPos + x) / 2f;
				v3.z = -4f;
				for (int i = 1; i < 15; i++)
				{
					float3 value = DTool.CubicBezierCurve_Uniform(in v, in v2, in v3, in x, (float)i / 15f, 15);
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = value
					});
				}
				AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in x);
				movement.AroundAngle = DTool.GetDegree(x - movement.AroundCenter);
				continue;
			}
			return;
		}
		for (int j = 0; j < refractRemain; j++)
		{
			DynamicBuffer<SpellRefractionHitEntities> hitEntities2 = refractHitteds;
			LaserBodyPoint laserBodyPoint = lineBuffer[0];
			if (RefractionTransformGet(hitEntities2, in currentHitEntities, in laserBodyPoint.Value, in shooterType, out var targetPos2))
			{
				float3 float2 = targetPos2 - lineBuffer[0].Value;
				stepDirection = math.normalizesafe(float2);
				float3 float3 = float2 / (heightList.Length - 1);
				for (int k = 1; k < heightList.Length - 1; k++)
				{
					lineBuffer.Insert(0, new LaserBodyPoint
					{
						Value = new float3((lineBuffer[0].Value + float3).xy, heightList[k])
					});
				}
				float2 xy = (lineBuffer[0].Value + float3).xy;
				NativeList<float> nativeList = heightList;
				float3 pos = new float3(xy, nativeList[nativeList.Length - 1]);
				AddGroundPoint(ref lineBuffer, ref touchGroundPos, ref hitEffectNode, in pos);
				laserBodyPoint = lineBuffer[0];
				DTool.GetEnemyEntityInRange(in laserBodyPoint.Value, refractRadius, shooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref currentHitEntities);
				continue;
			}
			break;
		}
	}

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

	private void AddGroundPoint(ref DynamicBuffer<LaserBodyPoint> bodyList, ref NativeList<float3> touchGList, ref NativeList<float4> effNodeList, in float3 pos)
	{
		bodyList.Insert(0, new LaserBodyPoint
		{
			Value = pos
		});
		touchGList.Add(in pos);
		float4 value = new float4(pos, 90f);
		effNodeList.Add(in value);
	}

	[BurstCompile]
	private void TouchGroundExplosion(in SpellConfigComponentData config, in SpellAspect spellAspect, in SpellComponentData data, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, in float3 touchGroundPos, [ChunkIndexInQuery] int chunkIndex)
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
		foreach (Entity item in entities)
		{
			Entity target = item;
			TakeDamageInfo_Dots damage = spellAspect.MakeDamageInfo(costPenetrate: false);
			damage.spell.HitPosition = TransformLookup[target].Position;
			CMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[BurstCompile]
	private void AddHitEffectNode(ref NativeList<float4> effNodeList, float3 position, float2 direction)
	{
		float w = math.atan2(direction.y, direction.x) * 57.29578f;
		float4 value = new float4(position, w);
		effNodeList.Add(in value);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1004LaserData_RW_ComponentTypeHandle);
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		BufferAccessor<LaserBodyPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__LaserBodyPoint_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				ref Spell1004LaserData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1004LaserData>(nativeArrayPtr2, i);
				SpellAspect spellAspect = resolvedChunk[i];
				DynamicBuffer<LaserBodyPoint> lineBuffer = bufferAccessor[i];
				Execute(entity, ref data, spellAspect, lineBuffer, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr3, i), chunkIndexInQuery);
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
					ref Spell1004LaserData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1004LaserData>(nativeArrayPtr2, nextRangeBegin);
					SpellAspect spellAspect2 = resolvedChunk[nextRangeBegin];
					DynamicBuffer<LaserBodyPoint> lineBuffer2 = bufferAccessor[nextRangeBegin];
					Execute(entity2, ref data2, spellAspect2, lineBuffer2, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr3, nextRangeBegin), chunkIndexInQuery);
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
				ref Spell1004LaserData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1004LaserData>(nativeArrayPtr2, j);
				SpellAspect spellAspect3 = resolvedChunk[j];
				DynamicBuffer<LaserBodyPoint> lineBuffer3 = bufferAccessor[j];
				Execute(entity3, ref data3, spellAspect3, lineBuffer3, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr3, j), chunkIndexInQuery);
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
				ref Spell1004LaserData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1004LaserData>(nativeArrayPtr2, k);
				SpellAspect spellAspect4 = resolvedChunk[k];
				DynamicBuffer<LaserBodyPoint> lineBuffer4 = bufferAccessor[k];
				Execute(entity4, ref data4, spellAspect4, lineBuffer4, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr3, k), chunkIndexInQuery);
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
