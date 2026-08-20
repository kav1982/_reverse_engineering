using System;
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

[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[CompilerGenerated]
[BurstCompile]
public struct Spell2001Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2001BoBoData> __Spell2001BoBoData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2001BoBoData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2001BoBoData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSpeedRatioValueData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__Spell2001BoBoData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__PathFinding_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2001BoBoData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellSpeedRatioValueData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
		public void Run(ref Spell2001Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2001Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2001Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2001Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2001Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2001Job job, EntityManager entityManager)
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

	public Entity SEPlayerSingleton;

	[ReadOnly]
	public SpellSingleton SpellSingleton;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideFrameIndex> FrameAnimeLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideRepeatCounter> FuseBodyRepeatCounterLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<PostTransformMatrix> FreeScaleLookUp;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity ShootSpellBufferEntity;

	public float DeltaTime;

	public GlobalRandom Random;

	public Entity SpellEffectEntity;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell2001BoBoData boboData, ref SpellComponentData spellData, ref SpellMovementComponentData movement, ref TeammateData teammateData, ref LocalTransform localTransform, [ChunkIndexInQuery] int chunkIndex, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity, ref UnitProperty_Dots boboPpt, ref SpellConfigComponentData config, SpellSpeedRatioValueData spellSpeedRatioValueData, ref PhysicsVelocity velocity)
	{
		EffectsCollectorData valueRW = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW;
		localTransform.Position.z = -0.05f;
		if (boboPpt.LockMotion || teammateData.IsHoldByTeammate6)
		{
			return;
		}
		bool isFireColor = config.ColorType == SpellColorType.Fire;
		switch (boboData.State)
		{
		case Spell2001State.Initialize:
			EnterState(Spell2001State.Initialize, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
			EnterState(Spell2001State.Idle, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
			break;
		case Spell2001State.Idle:
			EnterState(Spell2001State.IdleMove, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
			UpdateBoBoAnimaState(BoBoAnima.Idle, ref spellData, ref teammateData, ref boboData, localTransform.Position, pathFinding.endPosition);
			break;
		case Spell2001State.IdleMove:
		{
			if (boboData.AttackCoolDownTimer <= 0.8f)
			{
				boboData.AttackCoolDownTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			}
			if (LocalTransformLookUp.TryGetComponent(movement.ChaseTarget, out var componentData, out var entityExists) && entityExists)
			{
				boboData.TargetEntityLastFramePosition = componentData.Position;
				EnterState(Spell2001State.ChasingTarget, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
			}
			else
			{
				CurrentRoomEntities.FindNearestTarget(LocalTransformLookUp[entity].Position, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
			}
			if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget) && boboData.IdleMoveCoolDownTimer <= 0f && pathFinding.samplePointRequest.requestState == NavMeshRequestState.Completed)
			{
				float3 float2 = pathFinding.samplePointRequest.result;
				pathFinding.UpdatePath(localTransform.Position, float2, boboPpt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * movement.Speed * 0.3f);
				if (DTool.IgnoreZDistance(in localTransform.Position, in float2) <= unitBase.moveThreshold || boboData.fakeMoveTimer >= 0.2f)
				{
					unitBase.SetMoveWithVelocity(ref velocity, ref movement, float3.zero);
					boboData.IdleMoveCoolDownTimer = Random.random.NextFloat(1f, 2f);
					boboData.fakeMoveTimer = 0f;
					pathFinding.samplePointRequest.Reset();
				}
				UpdateBoBoFaceDirection(localTransform.Position, pathFinding.walkToPoint, valueRW.Effect5);
				UpdateSpellBodyMoveFrame(teammateData.IsFuseTeammate ? valueRW.Effect3 : valueRW.Effect1, 40f * teammateData.TeammateSpeedRatio * 0.7f, pauseAfterDone: false, isFireColor);
				if ((double)DTool.IgnoreZDistance(in boboData.LastFramePosition, in localTransform.Position) < 0.1 * (double)DeltaTime)
				{
					boboData.fakeMoveTimer += DeltaTime;
				}
			}
			else
			{
				boboData.IdleMoveCoolDownTimer -= DeltaTime * teammateData.TeammateSpeedRatio;
				float3 targetPos = (DTool.IsTotallySame(in pathFinding.walkToPoint, in float3.zero) ? boboData.TargetEntityLastFramePosition : LocalTransformLookUp[entity].Position);
				UpdateBoBoAnimaState(BoBoAnima.RunToTarget, ref spellData, ref teammateData, ref boboData, localTransform.Position, targetPos);
				UpdateSpellBodyMoveFrame(teammateData.IsFuseTeammate ? valueRW.Effect3 : valueRW.Effect1, 40f * teammateData.TeammateSpeedRatio, pauseAfterDone: true, isFireColor);
				if (boboData.IdleMoveCoolDownTimer <= 0f)
				{
					UpdateNewIdleWalkPosition(ref boboData, localTransform, ref pathFinding, LocalTransformLookUp[spellData.OwnerEntity].Position);
				}
			}
			break;
		}
		case Spell2001State.ChasingTarget:
		{
			if (boboData.AttackCoolDownTimer <= 0.8f)
			{
				boboData.AttackCoolDownTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			}
			if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget))
			{
				movement.ChaseTarget = Entity.Null;
				EnterState(Spell2001State.Idle, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
				break;
			}
			boboData.TargetEntityLastFramePosition = LocalTransformLookUp[movement.ChaseTarget].Position;
			LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
			if (DTool.IgnoreZDistance(in localTransform2.Position, in localTransform.Position) <= boboData.AttackRange && (!DTool.RaycastWallHit(localTransform.Position, LocalTransformLookUp[movement.ChaseTarget].Position, out var _, in Physics) || spellData.CanThroughWall))
			{
				UpdateSpellBodyMoveFrame(teammateData.IsFuseTeammate ? valueRW.Effect3 : valueRW.Effect1, 40f * teammateData.TeammateSpeedRatio, pauseAfterDone: true, isFireColor);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, float3.zero);
				UpdateBoBoFaceDirection(localTransform.Position, boboData.TargetEntityLastFramePosition, valueRW.Effect5);
				if (boboData.AttackCoolDownTimer >= 0.8f)
				{
					EnterState(Spell2001State.Attacking, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
				}
			}
			else
			{
				pathFinding.UpdatePath(localTransform.Position, LocalTransformLookUp[movement.ChaseTarget].Position, boboPpt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * movement.Speed);
				UpdateBoBoFaceDirection(localTransform.Position, boboData.TargetEntityLastFramePosition, valueRW.Effect5);
				UpdateSpellBodyMoveFrame(teammateData.IsFuseTeammate ? valueRW.Effect3 : valueRW.Effect1, 40f * teammateData.TeammateSpeedRatio, pauseAfterDone: false, isFireColor);
			}
			UpdateBoBoAnimaState(BoBoAnima.RunToTarget, ref spellData, ref teammateData, ref boboData, localTransform.Position, boboData.TargetEntityLastFramePosition);
			break;
		}
		case Spell2001State.Attacking:
			UpdateChaseTargetLastFramePosition(movement, ref boboData);
			UpdateBoBoFaceDirection(localTransform.Position, boboData.TargetEntityLastFramePosition, valueRW.Effect5);
			UpdateBoBoOpenMouseShootAnima(ref boboData, DeltaTime * teammateData.TeammateSpeedRatio, spellData.SpellEffectEntity, teammateData, valueRW, isFireColor);
			if (boboData.NormalBulletLeft > 0)
			{
				float num = (boboData.BoBoBombReady ? 0.6f : 0.15f);
				if (boboData.AttackIntervalTimer <= num)
				{
					BoBoAnima state = (boboData.BoBoBombReady ? BoBoAnima.EssenceAttack_Prepare : BoBoAnima.Attack_Prepare);
					UpdateBoBoAnimaState(state, ref spellData, ref teammateData, ref boboData, localTransform.Position, boboData.TargetEntityLastFramePosition);
					boboData.AttackIntervalTimer += DeltaTime * teammateData.TeammateSpeedRatio;
					break;
				}
				boboData.AttackIntervalTimer -= num;
				boboData.NormalBulletLeft--;
				BoBoAnima state2 = (boboData.BoBoBombReady ? BoBoAnima.EssenceAttack_Shoot : BoBoAnima.Attack_Shoot);
				UpdateBoBoAnimaState(state2, ref spellData, ref teammateData, ref boboData, localTransform.Position, boboData.TargetEntityLastFramePosition);
				PlayBoBoOpenMouseShootAnima(ref boboData, 0.12f / teammateData.TeammateSpeedRatio, spellData.SpellEffectEntity, teammateData, valueRW, isFireColor);
				float @base = config.Damage.Base;
				if (boboData.BoBoBombReady)
				{
					CMD.AppendToBuffer(chunkIndex, SEPlayerSingleton, new SEData("SE_Spell90012Shoot"));
					int num2 = -teammateData.TeammateCurrentFuseLevel / 2;
					float damageRatio = 5f + (float)teammateData.AdvanceSkillLevel * 15f;
					float damage = SpellSingleton.Configs[90012].damage;
					for (int i = 0; i <= teammateData.TeammateCurrentFuseLevel; i++)
					{
						float x = (float)(num2 + i) * 0.5f * localTransform.Scale;
						float3 input = math.normalizesafe(boboData.TargetEntityLastFramePosition - (localTransform.Position + new float3(x, 0f, 0f)));
						float3 direction = input.IgnoreZ();
						input = Random.random.NextFloat3Direction();
						float3 @float = DTool.IgnoreZPosition(in input);
						float num3 = math.distance(boboData.TargetEntityLastFramePosition + @float * config.Scatter * 0.06f, localTransform.Position + new float3(x, 0f, 0f));
						float num4 = ((num3 >= 1.5f) ? (num3 / 7.5f * Random.random.NextFloat(0.8f, 1.1f)) : 0.35f);
						float num5 = 13f * Random.random.NextFloat(1.5f, 2.6f);
						float upSpeed = (0f - num5) * num4 / 2f;
						float speed = num3 / num4;
						SpellSpawnParams element = SpellSingleton.SpellSpawnParamsStorage[entity].BuildBoboBomb(entity, localTransform.Position + new float3(x, 0f, -0.2f), boboData.TargetEntityLastFramePosition, direction, @base, damageRatio, damage, speed, num5, upSpeed);
						CMD.AppendToBuffer(chunkIndex, ShootSpellBufferEntity, element);
					}
				}
				else
				{
					CMD.AppendToBuffer(chunkIndex, SEPlayerSingleton, new SEData("SE_Spell9001Shoot"));
					int num6 = -teammateData.TeammateCurrentFuseLevel / 2;
					for (int j = 0; j <= teammateData.TeammateCurrentFuseLevel; j++)
					{
						float x2 = (float)(num6 + j) * 0.5f * localTransform.Scale;
						float3 input = math.normalizesafe(boboData.TargetEntityLastFramePosition - (localTransform.Position + new float3(x2, 0f, -0.2f)));
						float3 direction2 = input.IgnoreZ();
						SpellSpawnParams element2 = SpellSingleton.SpellSpawnParamsStorage[entity].BuildBoboBullet(entity, localTransform.Position + new float3(x2, 0f, -0.2f), boboData.TargetEntityLastFramePosition, direction2, @base, spellSpeedRatioValueData.Speed.CalculateWithNewBaseValue(7 + config.Level * 3), config.Duration.Extra + config.Duration.AddBase);
						CMD.AppendToBuffer(chunkIndex, ShootSpellBufferEntity, element2);
					}
				}
				break;
			}
			unitBase.SetMoveWithVelocity(ref velocity, ref movement, float3.zero);
			if (!(boboData.AttackMouseOpenAnimeTimer > 0f))
			{
				boboData.AfterAttackCoolDownTimer += DeltaTime * teammateData.TeammateSpeedRatio;
				UpdateBoBoAnimaState(BoBoAnima.Reset, ref spellData, ref teammateData, ref boboData, localTransform.Position, boboData.TargetEntityLastFramePosition);
				if (!(boboData.AfterAttackCoolDownTimer <= 0.2f))
				{
					boboData.AfterAttackCoolDownTimer = 0f;
					boboData.BoBoBombReady = false;
					EnterState(Spell2001State.Idle, entity, spellData, ref boboData, ref teammateData, ref boboPpt, ref unitBase, ref movement, spellSpeedRatioValueData, config, chunkIndex, ref pathFinding, isFireColor, ref velocity);
					movement.ChaseTarget = Entity.Null;
					pathFinding.endPosition = boboData.TargetEntityLastFramePosition;
					UpdateBoBoAnimaState(BoBoAnima.ImmediatelyReset, ref spellData, ref teammateData, ref boboData, localTransform.Position, boboData.TargetEntityLastFramePosition);
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		boboData.LastFramePosition = localTransform.Position;
	}

	private void UpdateBoBoAnimaState(BoBoAnima state, ref SpellComponentData spellData, ref TeammateData teammateData, ref Spell2001BoBoData boboData, float3 localPos, float3 targetPos)
	{
		Entity effect = EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect5;
		bool flag = targetPos.x >= localPos.x;
		float3 start = DTool.Float44ToFloat3(in FreeScaleLookUp.GetRefRW(effect).ValueRW.Value);
		float num = DeltaTime * teammateData.TeammateSpeedRatio;
		switch (state)
		{
		case BoBoAnima.Idle:
		{
			float num2 = math.sin(boboData.BodyAnimaTimer) * 0.1f;
			float3 end = new float3(flag ? (1f + num2) : (num2 - 1f), 1f - num2, 1f);
			float3 float2 = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float2);
			boboData.BodyAnimaTimer += MathF.PI * 2f * DeltaTime * teammateData.TeammateSpeedRatio;
			break;
		}
		case BoBoAnima.Walk:
		{
			float num3 = math.sin(boboData.BodyAnimaTimer) * 0.1f;
			float3 end = new float3(flag ? (1f + num3) : (num3 - 1f), 1f - num3, 1f);
			float3 float6 = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float6);
			boboData.BodyAnimaTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			break;
		}
		case BoBoAnima.RunToTarget:
		{
			float x = math.sin(boboData.BodyAnimaTimer) * 0.1f;
			float3 end = new float3(flag ? (1f - math.abs(x)) : (math.abs(x) - 1f), 1f + math.abs(x), 1f);
			float3 float3 = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float3);
			boboData.BodyAnimaTimer += DeltaTime * teammateData.TeammateSpeedRatio * 4f;
			break;
		}
		case BoBoAnima.Attack_Prepare:
		{
			float3 end = new float3(flag ? 1.2f : (-1.2f), 0.8f, 1f);
			float3 float7 = DTool.Lerp(in start, in end, 6f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float7);
			break;
		}
		case BoBoAnima.Attack_Shoot:
		{
			float3 end = new float3(flag ? 0.8f : (-0.8f), 1.5f, 1f);
			float3 float4 = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float4);
			break;
		}
		case BoBoAnima.Reset:
		{
			float3 end = new float3(flag ? 1 : (-1), 1f, 1f);
			float3 float8 = DTool.Lerp(in start, in end, 12f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float8);
			break;
		}
		case BoBoAnima.ImmediatelyReset:
		{
			float3 end = new float3(flag ? 1 : (-1), 1f, 1f);
			float3 float9 = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float9);
			break;
		}
		case BoBoAnima.EssenceAttack_Prepare:
		{
			float3 end = new float3(flag ? 1.5f : (-1.5f), 0.6f, 1f);
			float3 float5 = DTool.Lerp(in start, in end, 4f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)float5);
			break;
		}
		case BoBoAnima.EssenceAttack_Shoot:
		{
			float3 end = new float3(flag ? 0.6f : (-0.6f), 1.8f, 1f);
			float3 @float = DTool.Lerp(in start, in end, 50f * num);
			FreeScaleLookUp.GetRefRW(effect).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}

	private void UpdateChaseTargetLastFramePosition(SpellMovementComponentData movement, ref Spell2001BoBoData boboData)
	{
		if (LocalTransformLookUp.TryGetComponent(movement.ChaseTarget, out var componentData, out var entityExists) && entityExists)
		{
			boboData.TargetEntityLastFramePosition = componentData.Position;
		}
	}

	private void UpdateBoBoFaceDirection(float3 localPos, float3 targetPos, Entity targetEntity)
	{
		float3 @float = DTool.Float44ToFloat3(in FreeScaleLookUp.GetRefRW(targetEntity).ValueRW.Value);
		@float.x = ((targetPos.x >= localPos.x) ? math.abs(@float.x) : (0f - math.abs(@float.x)));
		FreeScaleLookUp.GetRefRW(targetEntity).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
	}

	private void PlayBoBoOpenMouseShootAnima(ref Spell2001BoBoData boboData, float duration, Entity bodyEntity, TeammateData teammateData, EffectsCollectorData effects, bool isFireColor)
	{
		boboData.AttackMouseOpenAnimeTimer += duration;
		ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? effects.Effect4 : effects.Effect2, isFireColor);
		HideTargetAnimaEntity(teammateData.IsFuseTeammate ? effects.Effect3 : effects.Effect1, isFireColor);
	}

	private void UpdateBoBoOpenMouseShootAnima(ref Spell2001BoBoData boboData, float deltaTime, Entity bodyEntity, TeammateData teammateData, EffectsCollectorData effects, bool isFireColor)
	{
		if (!(boboData.AttackMouseOpenAnimeTimer <= 0f))
		{
			boboData.AttackMouseOpenAnimeTimer -= deltaTime;
			if (!(boboData.AttackMouseOpenAnimeTimer > 0f))
			{
				_ = ref EffectsCollectorLookUp.GetRefRW(bodyEntity).ValueRW;
				ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? effects.Effect3 : effects.Effect1, isFireColor);
				HideTargetAnimaEntity(teammateData.IsFuseTeammate ? effects.Effect4 : effects.Effect2, isFireColor);
			}
		}
	}

	private void EnterState(Spell2001State boboState, Entity entity, SpellComponentData spellData, ref Spell2001BoBoData BoboData, ref TeammateData teammateData, ref UnitProperty_Dots myPpt, ref UnitBase_Dots unitBase, ref SpellMovementComponentData movementData, SpellSpeedRatioValueData speedRatioData, SpellConfigComponentData configData, [ChunkIndexInQuery] int chunkIndex, ref PathFinding pathFinding, bool isFireColor, ref PhysicsVelocity velocity)
	{
		BoboData.State = boboState;
		pathFinding.samplePointRequest.Reset();
		EffectsCollectorData effectsCollectorData = EffectsCollectorLookUp[spellData.SpellEffectEntity];
		switch (boboState)
		{
		case Spell2001State.Initialize:
			myPpt.id = 700100 + configData.Level;
			if (movementData.Type == SpellSpecialMovementType.Rotation || movementData.IsFallSpell)
			{
				BoboData.AttackRange = 20f;
				myPpt.FlyRegister();
			}
			else
			{
				BoboData.AttackRange = ((float)(7 + configData.Level * 3) + speedRatioData.Speed.Extra) * speedRatioData.Speed.MulRatio * 0.61538464f;
			}
			if (movementData.Type == SpellSpecialMovementType.Rotation)
			{
				myPpt.navAreaMask = 32;
			}
			BoboData.TargetEntityLastFramePosition = default(float3);
			HideTargetAnimaEntity(effectsCollectorData.Effect1, isFireColor);
			HideTargetAnimaEntity(effectsCollectorData.Effect2, isFireColor);
			HideTargetAnimaEntity(effectsCollectorData.Effect3, isFireColor);
			HideTargetAnimaEntity(effectsCollectorData.Effect4, isFireColor);
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effectsCollectorData.Effect1
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effectsCollectorData.Effect2
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effectsCollectorData.Effect3
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effectsCollectorData.Effect4
			});
			if (isFireColor)
			{
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = GetFireOutLineEntity(effectsCollectorData.Effect1)
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = GetFireOutLineEntity(effectsCollectorData.Effect2)
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = GetFireOutLineEntity(effectsCollectorData.Effect3)
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = GetFireOutLineEntity(effectsCollectorData.Effect4)
				});
			}
			BoboData.IdleMoveCoolDownTimer = Random.random.NextFloat(0.5f, 1.5f);
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? effectsCollectorData.Effect3 : effectsCollectorData.Effect1, isFireColor);
			if (teammateData.IsFuseTeammate)
			{
				FuseBodyRepeatCounterLookUp.GetRefRW(effectsCollectorData.Effect3).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				FuseBodyRepeatCounterLookUp.GetRefRW(effectsCollectorData.Effect4).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				float3 @float = DTool.Float44ToFloat3(in FreeScaleLookUp.GetRefRW(effectsCollectorData.Effect3).ValueRW.Value);
				@float.x = 1f + (float)teammateData.TeammateCurrentFuseLevel * 0.5f;
				FreeScaleLookUp.GetRefRW(effectsCollectorData.Effect3).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
				FreeScaleLookUp.GetRefRW(effectsCollectorData.Effect4).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
				if (isFireColor)
				{
					FuseBodyRepeatCounterLookUp.GetRefRW(GetFireOutLineEntity(effectsCollectorData.Effect3)).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
					FuseBodyRepeatCounterLookUp.GetRefRW(GetFireOutLineEntity(effectsCollectorData.Effect4)).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				}
			}
			break;
		case Spell2001State.Idle:
			BoboData.BodyAnimaTimer = 0f;
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? effectsCollectorData.Effect3 : effectsCollectorData.Effect1, isFireColor);
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? effectsCollectorData.Effect4 : effectsCollectorData.Effect2, isFireColor);
			UpdateBoBoAnimaState(BoBoAnima.ImmediatelyReset, ref spellData, ref teammateData, ref BoboData, LocalTransformLookUp[entity].Position, BoboData.TargetEntityLastFramePosition);
			break;
		case Spell2001State.IdleMove:
			UpdateSpellBodyMoveFrame(teammateData.IsFuseTeammate ? effectsCollectorData.Effect3 : effectsCollectorData.Effect1, 2.5f * teammateData.TeammateSpeedRatio, pauseAfterDone: false, isFireColor);
			unitBase.SetMoveWithVelocity(ref velocity, ref movementData, float3.zero);
			UpdateNewIdleWalkPosition(ref BoboData, LocalTransformLookUp[entity], ref pathFinding, LocalTransformLookUp[spellData.OwnerEntity].Position);
			UpdateBoBoAnimaState(BoBoAnima.ImmediatelyReset, ref spellData, ref teammateData, ref BoboData, LocalTransformLookUp[entity].Position, BoboData.TargetEntityLastFramePosition);
			break;
		case Spell2001State.Attacking:
		{
			UpdateBoBoAnimaState(BoBoAnima.ImmediatelyReset, ref spellData, ref teammateData, ref BoboData, LocalTransformLookUp[entity].Position, BoboData.TargetEntityLastFramePosition);
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? effectsCollectorData.Effect3 : effectsCollectorData.Effect1, isFireColor);
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? effectsCollectorData.Effect4 : effectsCollectorData.Effect2, isFireColor);
			bool flag = (BoboData.BoBoBombReady = teammateData.AdvanceSkillLevel > 0 && Random.random.NextFloat(0f, 1f) <= 0.2f);
			BoboData.NormalBulletLeft = (flag ? 1 : 5);
			BoboData.AttackIntervalTimer = 0f;
			BoboData.AttackCoolDownTimer -= 0.8f;
			BoboData.AttackMouseOpenAnimeTimer = 0f;
			if (flag)
			{
				configData.ColorType.ColorEnumToString(out var result);
				CMD.AppendToBuffer(chunkIndex, SpellEffectEntity, new SpellEffectSystem.Require
				{
					Entity = entity,
					SpellId = 2001,
					Color = result,
					Settings = new SpellEffect
					{
						ScaleMode = SpellEffectSystem.ScaleMode.Scale,
						DestroyDelay = 0.5f,
						Name = "Charge",
						Layer = LayerCorrectType.Coordinate
					}
				});
			}
			break;
		}
		case Spell2001State.ChasingTarget:
			break;
		}
	}

	private void UpdateNewIdleWalkPosition(ref Spell2001BoBoData BoboData, LocalTransform trans, ref PathFinding pathFinding, float3 ownerPos)
	{
		float3 input = ownerPos - trans.Position;
		float3 x = input.IgnoreZ();
		x = math.normalizesafe(x);
		float3 dir = DTool.GetDir(in x, Random.random.NextFloat(-30f, 30f));
		float num = DTool.IgnoreZDistance(in ownerPos, in trans.Position);
		float3 @float = trans.Position + Random.random.NextFloat(1f, 3f) * DTool.GetDir(ref Random.random) + math.min(3, dir * num / 5f);
		pathFinding.samplePointRequest.SetRequest(@float);
	}

	private Entity GetFireOutLineEntity(Entity targetEntity)
	{
		return EffectsCollectorLookUp[targetEntity].Effect1;
	}

	private void UpdateSpellBodyMoveFrame(Entity targetEntity, float speedRatio, bool pauseAfterDone, bool isFireColor)
	{
		RefRW<MatOverrideFrameIndex> refRW;
		if (pauseAfterDone)
		{
			refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
			if (refRW.ValueRW.FrameIndex % 16f <= DeltaTime * speedRatio * 3f)
			{
				refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
				refRW.ValueRW.FrameIndex = 0f;
				if (isFireColor)
				{
					refRW = FrameAnimeLookUp.GetRefRW(GetFireOutLineEntity(targetEntity));
					refRW.ValueRW.FrameIndex = 0f;
				}
				return;
			}
		}
		refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		refRW.ValueRW.FrameIndex += DeltaTime * speedRatio;
		if (isFireColor)
		{
			refRW = FrameAnimeLookUp.GetRefRW(GetFireOutLineEntity(targetEntity));
			refRW.ValueRW.FrameIndex += DeltaTime * speedRatio;
		}
	}

	private void ResetTargetAnimaEntityFrame(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = 0f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(GetFireOutLineEntity(targetEntity)).ValueRW.FrameIndex = 0f;
		}
	}

	private void HideTargetAnimaEntity(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = -0.1f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(GetFireOutLineEntity(targetEntity)).ValueRW.FrameIndex = -0.1f;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2001BoBoData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr11 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellSpeedRatioValueData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr12 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2001BoBoData boboData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2001BoBoData>(nativeArrayPtr, i);
				ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, i);
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				ref UnitProperty_Dots boboPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, i);
				ref SpellSpeedRatioValueData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, i);
				Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, i), boboData: ref boboData, spellData: ref spellData, movement: ref movement, teammateData: ref teammateData, localTransform: ref localTransform, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase, pathFinding: ref pathFinding, entity: entity, boboPpt: ref boboPpt, config: ref config, spellSpeedRatioValueData: reference);
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
					ref Spell2001BoBoData boboData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2001BoBoData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, nextRangeBegin);
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					ref UnitProperty_Dots boboPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, nextRangeBegin);
					ref SpellSpeedRatioValueData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, nextRangeBegin);
					Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, nextRangeBegin), boboData: ref boboData2, spellData: ref spellData2, movement: ref movement2, teammateData: ref teammateData2, localTransform: ref localTransform2, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase2, pathFinding: ref pathFinding2, entity: entity2, boboPpt: ref boboPpt2, config: ref config2, spellSpeedRatioValueData: reference2);
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
				ref Spell2001BoBoData boboData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2001BoBoData>(nativeArrayPtr, j);
				ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, j);
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				ref UnitProperty_Dots boboPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, j);
				ref SpellSpeedRatioValueData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, j);
				Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, j), boboData: ref boboData3, spellData: ref spellData3, movement: ref movement3, teammateData: ref teammateData3, localTransform: ref localTransform3, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase3, pathFinding: ref pathFinding3, entity: entity3, boboPpt: ref boboPpt3, config: ref config3, spellSpeedRatioValueData: reference3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2001BoBoData boboData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2001BoBoData>(nativeArrayPtr, k);
				ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, k);
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				ref UnitProperty_Dots boboPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, k);
				ref SpellSpeedRatioValueData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, k);
				Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, k), boboData: ref boboData4, spellData: ref spellData4, movement: ref movement4, teammateData: ref teammateData4, localTransform: ref localTransform4, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase4, pathFinding: ref pathFinding4, entity: entity4, boboPpt: ref boboPpt4, config: ref config4, spellSpeedRatioValueData: reference4);
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
