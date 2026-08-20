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

[CompilerGenerated]
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[BurstCompile]
public struct Spell2002Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2002Data> __Spell2002Data_RW_ComponentTypeHandle;

			public BufferTypeHandle<LegsData> __LegsData_RW_BufferTypeHandle;

			public BufferTypeHandle<EssenceLegsData> __EssenceLegsData_RW_BufferTypeHandle;

			public BufferTypeHandle<LegsTarget> __LegsTarget_RW_BufferTypeHandle;

			public BufferTypeHandle<EssenceLegAttackedEntity> __EssenceLegAttackedEntity_RW_BufferTypeHandle;

			public BufferTypeHandle<FuseHeadEntity> __FuseHeadEntity_RW_BufferTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TeammateData> __TeammateData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<AnimationCurveData> __AnimationCurveData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2002Data_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2002Data>();
				__LegsData_RW_BufferTypeHandle = state.GetBufferTypeHandle<LegsData>();
				__EssenceLegsData_RW_BufferTypeHandle = state.GetBufferTypeHandle<EssenceLegsData>();
				__LegsTarget_RW_BufferTypeHandle = state.GetBufferTypeHandle<LegsTarget>();
				__EssenceLegAttackedEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<EssenceLegAttackedEntity>();
				__FuseHeadEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<FuseHeadEntity>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__TeammateData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
				__AnimationCurveData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AnimationCurveData>(isReadOnly: true);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__Spell2002Data_RW_ComponentTypeHandle.Update(ref state);
				__LegsData_RW_BufferTypeHandle.Update(ref state);
				__EssenceLegsData_RW_BufferTypeHandle.Update(ref state);
				__LegsTarget_RW_BufferTypeHandle.Update(ref state);
				__EssenceLegAttackedEntity_RW_BufferTypeHandle.Update(ref state);
				__FuseHeadEntity_RW_BufferTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__PathFinding_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RO_ComponentTypeHandle.Update(ref state);
				__AnimationCurveData_RO_ComponentTypeHandle.Update(ref state);
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
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<AnimationCurveData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2002Data>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegsData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsTarget>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegAttackedEntity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<FuseHeadEntity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
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
		public void Run(ref Spell2002Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2002Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2002Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2002Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2002Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2002Job job, EntityManager entityManager)
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

	public float DeltaTime;

	public GlobalRandom Random;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity SEPlayerSingleton;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPptLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

	private const int CantAttackLegCount = 4;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell2002Data spell2002Data, ref DynamicBuffer<LegsData> legsData, ref DynamicBuffer<EssenceLegsData> essenceLegsData, ref DynamicBuffer<LegsTarget> legsTargets, ref DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities, ref DynamicBuffer<FuseHeadEntity> headBuffer, ref LocalTransform localTransform, Entity entity, ref UnitProperty_Dots ppt, ref UnitBase_Dots unitBase, ref SpellMovementComponentData movement, ref PathFinding pathFinding, in SpellComponentData spellData, ref SpellConfigComponentData config, in TeammateData teammateData, in AnimationCurveData curve, [ChunkIndexInQuery] int chunkIndex, ref PhysicsVelocity velocity)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			return;
		}
		switch (spell2002Data.State)
		{
		case Spell2002State.Initialize:
		{
			ppt.id = 700201;
			spell2002Data.AttackRange = config.Radius.Calculate() * 2f;
			spell2002Data.IdleMoveCoolDownTimer = Random.random.NextFloat(2f, 3f);
			float num3 = 1f;
			if (config.Level == 1)
			{
				num3 = 0.5f;
			}
			else if (config.Level == 2)
			{
				num3 = 0.7f;
			}
			else if (config.Level == 3)
			{
				num3 = 0.9f;
			}
			ppt.unitCfg.currentHP = ppt.unitCfg.maxHP * num3;
			float num4 = Mathf.Pow(config.Damage.Calculate() / config.Damage.Base, 0.3333f);
			if (math.abs(num4 - 1f) >= 0.01f)
			{
				spell2002Data.DamageScaleRatio = num4;
			}
			else
			{
				spell2002Data.DamageScaleRatio = 1f;
			}
			config.Damage.AddBase += spell2002Data.ExtraDamage;
			config.Damage.Extra *= 3f;
			EffectsCollectorData valueRW = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW;
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = valueRW.Effect1
			});
			LocalTransform localTransform2 = LocalTransformLookUp[valueRW.Effect1];
			LocalTransform localTransform3 = LocalTransformLookUp[valueRW.Effect2];
			spell2002Data.MainHeadPos = localTransform.Position + localTransform3.Position + localTransform2.Position;
			spell2002Data.MainHeadRootPos = localTransform.Position + localTransform3.Position;
			CMD.SetComponentEnabled<MatOverrideFuseProgress>(chunkIndex, valueRW.Effect1, value: false);
			CMD.SetComponentEnabled<MatOverrideFuseProgress>(chunkIndex, valueRW.Effect3, value: false);
			if (valueRW.Effect4 != Entity.Null)
			{
				CMD.SetComponentEnabled<MatOverrideFuseProgress>(chunkIndex, valueRW.Effect4, value: false);
			}
			if (valueRW.Effect5 != Entity.Null)
			{
				CMD.SetComponentEnabled<MatOverrideFuseProgress>(chunkIndex, valueRW.Effect5, value: false);
			}
			for (int i = 0; i < headBuffer.Length; i++)
			{
				Entity entity2 = headBuffer[i].Entity;
				CMD.AddComponent<Parent>(chunkIndex, entity2);
				CMD.SetComponent(chunkIndex, entity2, new Parent
				{
					Value = valueRW.Effect2
				});
			}
			for (int j = 0; j < config.Int1; j++)
			{
				float3 dir = DTool.GetDir(360f / (float)config.Int1 * (float)j * (MathF.PI / 180f));
				RandomFloat legRadiusRatio = new RandomFloat(0.4f, 1.1f);
				float3 float2 = localTransform.Position + dir + DTool.GetDir(ref Random.random) * config.Radius.Calculate() * legRadiusRatio.RandomResult(ref Random.random);
				legsData.Add(new LegsData
				{
					LegState = LegState.Idle,
					LegRadiusRatio = legRadiusRatio,
					MoveToEndPoint = float2,
					CurrentEndPoint = float2,
					Dir = dir,
					IsCantAttackLeg = (j % (config.Int1 / 4) == 0),
					FuseHeadIndex = -1
				});
			}
			int num5 = config.Int1 - 4;
			for (int k = 0; k < teammateData.TeammateCurrentFuseLevel; k++)
			{
				for (int l = 0; l < num5; l++)
				{
					float3 dir2 = DTool.GetDir(360f / (float)num5 * (float)l * (MathF.PI / 180f));
					RandomFloat legRadiusRatio2 = new RandomFloat(0.4f, 1.1f);
					float3 float3 = localTransform.Position + dir2 + DTool.GetDir(ref Random.random) * config.Radius.Calculate() * legRadiusRatio2.RandomResult(ref Random.random);
					legsData.Add(new LegsData
					{
						LegState = LegState.Idle,
						LegRadiusRatio = legRadiusRatio2,
						MoveToEndPoint = float3,
						CurrentEndPoint = float3,
						Dir = dir2,
						FuseHeadIndex = k,
						IsCantAttackLeg = false
					});
				}
			}
			int num6 = teammateData.AdvanceSkillLevel * 2;
			int y = num6 * (teammateData.TeammateCurrentFuseLevel + 1);
			spell2002Data.EssenceLegGroupCount = math.min(13, y);
			spell2002Data.EssenceAttackInterval = 2f / teammateData.TeammateSpeedRatio / (float)spell2002Data.EssenceLegGroupCount;
			spell2002Data.EssenceAttackTimer = 0f;
			spell2002Data.EssenceDamageRatio = 0.6f;
			for (int m = 0; m < num6; m++)
			{
				EssenceLegsData elem = default(EssenceLegsData);
				elem.FuseHeadIndex = -1;
				elem.LegRadius = config.Radius.Calculate();
				elem.IdleLookPoint = movement.Direction * 5f + localTransform.Position;
				elem.ResetEssenceLegAttackData(ref Random, chunkIndex);
				essenceLegsData.Add(elem);
			}
			for (int n = 0; n < teammateData.TeammateCurrentFuseLevel; n++)
			{
				for (int num7 = 0; num7 < num6; num7++)
				{
					EssenceLegsData elem2 = default(EssenceLegsData);
					elem2.LegRadius = config.Radius.Calculate();
					float3 float4 = new float3(0f, (float)n + 0.54f, 0f);
					float3 float5 = localTransform.Position + localTransform3.Position + float4;
					elem2.IdleLookPoint = movement.Direction * 5f + float5;
					elem2.ResetEssenceLegAttackData(ref Random, chunkIndex);
					elem2.FuseHeadIndex = n;
					essenceLegsData.Add(elem2);
				}
			}
			spell2002Data.State = Spell2002State.IdleMove;
			break;
		}
		case Spell2002State.IdleMove:
		{
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				break;
			}
			if (LocalTransformLookUp.TryGetComponent(movement.ChaseTarget, out var _, out var entityExists) && entityExists)
			{
				spell2002Data.State = Spell2002State.ChasingTarget;
			}
			else
			{
				CurrentRoomEntities.FindNearestTarget(localTransform.Position, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
			}
			if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget) && spell2002Data.IdleMoveCoolDownTimer <= 0f)
			{
				pathFinding.UpdatePath(localTransform.Position, spell2002Data.IdleMoveTargetPos, ppt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * (ppt.unitCfg.moveSpeed * 0.5f * teammateData.TeammateSpeedRatio));
				if (DTool.IgnoreZDistance(in localTransform.Position, in pathFinding.walkToPoint) <= unitBase.moveThreshold)
				{
					unitBase.SetMoveWithVelocity(ref velocity, ref movement, float3.zero);
					spell2002Data.IdleMoveTargetPos = default(float3);
					spell2002Data.IdleMoveCoolDownTimer = Random.random.NextFloat(2f, 3f);
				}
			}
			else
			{
				spell2002Data.IdleMoveCoolDownTimer -= DeltaTime;
				if (spell2002Data.IdleMoveCoolDownTimer <= 0f)
				{
					UpdateNewIdleWalkPosition(ref spell2002Data, localTransform);
				}
			}
			break;
		}
		case Spell2002State.ChasingTarget:
		{
			bool num = LocalTransformLookUp.HasComponent(movement.ChaseTarget);
			UnitProperty_Dots componentData;
			bool flag = UnitPptLookUp.TryGetComponent(movement.ChaseTarget, out componentData);
			if (!num || !flag || !componentData.CanBeTarget || componentData.IsInvincible)
			{
				movement.ChaseTarget = Entity.Null;
				spell2002Data.IdleMoveCoolDownTimer = 0f;
				spell2002Data.State = Spell2002State.IdleMove;
				break;
			}
			float3 point = LocalTransformLookUp[movement.ChaseTarget].Position;
			float num2 = DTool.IgnoreZDistance(in point, in localTransform.Position);
			if (num2 >= config.Radius.Calculate() / 3f && num2 < config.Radius.Calculate())
			{
				float3 motion = DTool.Lerp(in unitBase.currentMotion, in float3.zero, 10f * DeltaTime);
				if (math.sqrt(motion.x * motion.x + motion.y * motion.y + motion.z * motion.z) <= 0.05f)
				{
					motion = float3.zero;
				}
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, motion);
			}
			else if (num2 <= config.Radius.Calculate())
			{
				float3 @float = math.normalize(point - localTransform.Position);
				float3 endPosition = point - @float * config.Radius.Calculate();
				pathFinding.UpdatePath(localTransform.Position, endPosition, ppt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * (movement.Speed * 0.5f));
			}
			else
			{
				pathFinding.UpdatePath(localTransform.Position, LocalTransformLookUp[movement.ChaseTarget].Position, ppt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * (movement.Speed * 0.5f));
			}
			break;
		}
		}
		SyncDirection(ref spell2002Data, unitBase, movement);
		UpdateHeadRootPos(ref spell2002Data, spellData, curve);
		UpdateHeadScale(ppt, spellData);
		UpdateEssenceLockTarget(ref spell2002Data, legsTargets, localTransform);
		UpdateEssenceAttack(ref spell2002Data, essenceLegsData, essenceLegAttackedEntities, chunkIndex);
		CheckPortal(ref spell2002Data, in config, in localTransform, ref legsData, ref essenceLegsData, ref movement);
	}

	private void CheckPortal(ref Spell2002Data spell2002Data, in SpellConfigComponentData config, in LocalTransform transform, ref DynamicBuffer<LegsData> legsData, ref DynamicBuffer<EssenceLegsData> essenceLegsData, ref SpellMovementComponentData movement)
	{
		if (spell2002Data.IsPortal)
		{
			movement.ChaseTarget = Entity.Null;
			spell2002Data.IdleMoveCoolDownTimer = 0f;
			spell2002Data.State = Spell2002State.IdleMove;
			float num = config.Radius.Calculate();
			float3 position = transform.Position;
			for (int i = 0; i < legsData.Length; i++)
			{
				LegsData legsData2 = legsData[i];
				legsData2.MoveToEndPoint = position + legsData2.Dir * num;
				AdjustLegsMoveEndPoint(position, ref legsData2);
				legsData2.CurrentEndPoint = legsData2.MoveToEndPoint;
				legsData[i] = legsData2;
			}
		}
	}

	private void AdjustLegsMoveEndPoint(float3 headPos, ref LegsData legsData)
	{
		if (DTool.RaycastWallHit(headPos.IgnoreZ(), legsData.MoveToEndPoint.IgnoreZ(), out var hitResult, in PhysicsWorld))
		{
			float3 input = hitResult.Position;
			legsData.MoveToEndPoint = input.IgnoreZ();
		}
	}

	private void UpdateHeadRootPos(ref Spell2002Data spell2002Data, SpellComponentData spellData, AnimationCurveData curve)
	{
		spell2002Data.HeadAnimationTimer += DeltaTime;
		EffectsCollectorData valueRW = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW;
		LocalTransform value = LocalTransformLookUp[valueRW.Effect2];
		float y = curve.Curve2.Evaluate(spell2002Data.HeadAnimationTimer);
		value.Position = new float3(0f, y, 0f);
		LocalTransformLookUp[valueRW.Effect2] = value;
	}

	private void UpdateEssenceAttack(ref Spell2002Data spell2002Data, DynamicBuffer<EssenceLegsData> essenceLegsData, DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities, int chunkIndex)
	{
		spell2002Data.EssenceAttackTimer += DeltaTime;
		if (spell2002Data.EssenceLockTarget == Entity.Null || spell2002Data.EssenceAttackTimer < spell2002Data.EssenceAttackInterval)
		{
			return;
		}
		spell2002Data.EssenceAttackTimer = 0f;
		for (int i = 0; i < essenceLegsData.Length; i++)
		{
			if (i % spell2002Data.EssenceLegGroupCount == spell2002Data.CurrentEssenceLegAttackIndex)
			{
				EssenceLegsData value = essenceLegsData[i];
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Stab";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(2002, in seName)));
				value.StartAttack();
				essenceLegsData[i] = value;
			}
		}
		spell2002Data.CurrentEssenceLegAttackIndex = ++spell2002Data.CurrentEssenceLegAttackIndex % spell2002Data.EssenceLegGroupCount;
	}

	private void UpdateEssenceLockTarget(ref Spell2002Data spell2002Data, DynamicBuffer<LegsTarget> legsTargets, LocalTransform localTransform)
	{
		LocalTransform componentData2;
		if (spell2002Data.EssenceLockTarget == Entity.Null)
		{
			for (int i = 0; i < legsTargets.Length; i++)
			{
				LegsTarget legsTarget = legsTargets[i];
				if (legsTarget.Status != 0 && LocalTransformLookUp.TryGetComponent(legsTarget.Target, out var componentData) && math.distance(componentData.Position, localTransform.Position) < spell2002Data.AttackRange)
				{
					spell2002Data.EssenceLockTarget = legsTarget.Target;
				}
			}
		}
		else if (LocalTransformLookUp.TryGetComponent(spell2002Data.EssenceLockTarget, out componentData2))
		{
			if (math.distance(componentData2.Position, localTransform.Position) >= spell2002Data.AttackRange)
			{
				spell2002Data.EssenceLockTarget = Entity.Null;
			}
		}
		else
		{
			spell2002Data.EssenceLockTarget = Entity.Null;
		}
	}

	private void UpdateHeadScale(UnitProperty_Dots ppt, SpellComponentData spellData)
	{
		float scale = math.lerp(0.8f, 1.5f, ppt.unitCfg.currentHP / ppt.unitCfg.maxHP);
		LocalTransform value = LocalTransformLookUp[spellData.SpellEffectEntity];
		value.Scale = scale;
		LocalTransformLookUp[spellData.SpellEffectEntity] = value;
	}

	private void SyncDirection(ref Spell2002Data spell2002Data, UnitBase_Dots unitBase, SpellMovementComponentData movement)
	{
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			spell2002Data.Direction = movement.Direction;
		}
		else if (math.lengthsq(unitBase.currentMotion) > 0.1f)
		{
			spell2002Data.Direction = math.lerp(spell2002Data.Direction, math.normalizesafe(unitBase.currentMotion), 0.2f);
		}
	}

	private void UpdateNewIdleWalkPosition(ref Spell2002Data spell2002Data, LocalTransform trans)
	{
		spell2002Data.IdleMoveTargetPos = trans.Position + Random.random.NextFloat(1f, 3f) * DTool.GetDir(ref Random.random);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2002Data_RW_ComponentTypeHandle);
		BufferAccessor<LegsData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__LegsData_RW_BufferTypeHandle);
		BufferAccessor<EssenceLegsData> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__EssenceLegsData_RW_BufferTypeHandle);
		BufferAccessor<LegsTarget> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__LegsTarget_RW_BufferTypeHandle);
		BufferAccessor<EssenceLegAttackedEntity> bufferAccessor4 = chunk.GetBufferAccessor(ref __TypeHandle.__EssenceLegAttackedEntity_RW_BufferTypeHandle);
		BufferAccessor<FuseHeadEntity> bufferAccessor5 = chunk.GetBufferAccessor(ref __TypeHandle.__FuseHeadEntity_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TeammateData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr11 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__AnimationCurveData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr12 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2002Data spell2002Data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, i);
				DynamicBuffer<LegsData> legsData = bufferAccessor[i];
				DynamicBuffer<EssenceLegsData> essenceLegsData = bufferAccessor2[i];
				DynamicBuffer<LegsTarget> legsTargets = bufferAccessor3[i];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities = bufferAccessor4[i];
				DynamicBuffer<FuseHeadEntity> headBuffer = bufferAccessor5[i];
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(ref spell2002Data, ref legsData, ref essenceLegsData, ref legsTargets, ref essenceLegAttackedEntities, ref headBuffer, ref localTransform, entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr8, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr10, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr11, i), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, i));
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
					ref Spell2002Data spell2002Data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, nextRangeBegin);
					DynamicBuffer<LegsData> legsData2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<EssenceLegsData> essenceLegsData2 = bufferAccessor2[nextRangeBegin];
					DynamicBuffer<LegsTarget> legsTargets2 = bufferAccessor3[nextRangeBegin];
					DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities2 = bufferAccessor4[nextRangeBegin];
					DynamicBuffer<FuseHeadEntity> headBuffer2 = bufferAccessor5[nextRangeBegin];
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(ref spell2002Data2, ref legsData2, ref essenceLegsData2, ref legsTargets2, ref essenceLegAttackedEntities2, ref headBuffer2, ref localTransform2, entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr8, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr10, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr11, nextRangeBegin), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, nextRangeBegin));
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
				ref Spell2002Data spell2002Data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, j);
				DynamicBuffer<LegsData> legsData3 = bufferAccessor[j];
				DynamicBuffer<EssenceLegsData> essenceLegsData3 = bufferAccessor2[j];
				DynamicBuffer<LegsTarget> legsTargets3 = bufferAccessor3[j];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities3 = bufferAccessor4[j];
				DynamicBuffer<FuseHeadEntity> headBuffer3 = bufferAccessor5[j];
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(ref spell2002Data3, ref legsData3, ref essenceLegsData3, ref legsTargets3, ref essenceLegAttackedEntities3, ref headBuffer3, ref localTransform3, entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr8, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr10, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr11, j), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2002Data spell2002Data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, k);
				DynamicBuffer<LegsData> legsData4 = bufferAccessor[k];
				DynamicBuffer<EssenceLegsData> essenceLegsData4 = bufferAccessor2[k];
				DynamicBuffer<LegsTarget> legsTargets4 = bufferAccessor3[k];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities4 = bufferAccessor4[k];
				DynamicBuffer<FuseHeadEntity> headBuffer4 = bufferAccessor5[k];
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(ref spell2002Data4, ref legsData4, ref essenceLegsData4, ref legsTargets4, ref essenceLegAttackedEntities4, ref headBuffer4, ref localTransform4, entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr8, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr10, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr11, k), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr12, k));
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
