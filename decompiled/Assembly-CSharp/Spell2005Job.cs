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
[BurstCompile]
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
public struct Spell2005Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2005GrimoireData> __Spell2005GrimoireData_RW_ComponentTypeHandle;

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

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2005GrimoireData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2005GrimoireData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__Spell2005GrimoireData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__PathFinding_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
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
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2005GrimoireData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
		public void Run(ref Spell2005Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2005Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2005Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2005Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2005Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2005Job job, EntityManager entityManager)
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

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitpropertyLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideFrameIndex> FrameAnimeLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideRepeatCounter> FuseBodyRepeatCounterLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideHideProgressEffect> HideProgressLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideTwirlProgressData> TwirlProgressLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<PostTransformMatrix> FreeScaleLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideProgress> ProgressBarLookUp;

	public EntityCommandBuffer.ParallelWriter CMD;

	public float DeltaTime;

	public GlobalRandom Random;

	public Entity SpellEffectEntity;

	[ReadOnly]
	public SpellDashDriverSingleton SpellDashDriverSingleton;

	public Entity GlobalParticleEmitBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(ref Spell2005GrimoireData bookData, ref SpellComponentData spellData, ref SpellMovementComponentData movement, ref TeammateData teammateData, ref LocalTransform localTransform, [ChunkIndexInQuery] int chunkIndex, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity, ref UnitProperty_Dots boboPpt, ref SpellConfigComponentData config, ref PhysicsVelocity velocity)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			return;
		}
		bool isFireColor = config.ColorType == SpellColorType.Fire;
		localTransform.Position.z = -0.05f;
		float num = 0f;
		if (UnitpropertyLookUp.HasComponent(movement.ChaseTarget))
		{
			num += UnitpropertyLookUp[movement.ChaseTarget].size / 2f;
		}
		float num2 = bookData.AttackRange + num;
		MoveToTarget(ref pathFinding, ref localTransform, ref movement, ref boboPpt, ref unitBase, ref bookData, ref velocity, num2);
		UpdataFloatingState(ref bookData, spellData, DeltaTime);
		UpdataCurrentMp(ref bookData);
		UpdateProgressBarState(bookData, spellData);
		UpdateChaseTarget(ref bookData, ref movement, entity);
		UpdateTeleportState(ref bookData, movement, teammateData, entity, Random, ref pathFinding, chunkIndex, spellData, num2, isFireColor);
		UpdateBookProgressBarHideState(ref bookData, spellData, teammateData);
		if (LocalTransformLookUp.HasComponent(movement.ChaseTarget))
		{
			LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
			movement.Direction = DTool.IgnoreZDir(in localTransform2.Position, in localTransform.Position);
		}
		Entity effect = EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1).ValueRW.Effect3;
		Entity effect2 = EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1).ValueRW.Effect2;
		Entity effect3 = EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2).ValueRW.Effect3;
		Entity effect4 = EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2).ValueRW.Effect2;
		switch (bookData.State)
		{
		case Spell2005State.Initialize:
			EnterState(Spell2005State.Initialize, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			EnterState(Spell2005State.Idle, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			break;
		case Spell2005State.Idle:
			if (!(spellData.SubGroupEntity == Entity.Null))
			{
				bool entityExists = default(bool);
				if (spellData.SubGroupEntity != Entity.Null && LocalTransformLookUp.TryGetComponent(movement.ChaseTarget, out var _, out entityExists) && entityExists)
				{
					EnterState(Spell2005State.ChasingTarget, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
				}
				else
				{
					CurrentRoomEntities.FindNearestTarget(LocalTransformLookUp[entity].Position, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
				}
			}
			break;
		case Spell2005State.ChasingTarget:
			if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget))
			{
				movement.ChaseTarget = Entity.Null;
				EnterState(Spell2005State.Idle, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			}
			else
			{
				if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget))
				{
					break;
				}
				LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
				if (DTool.IgnoreZDistance(in localTransform2.Position, in localTransform.Position) <= num2 && bookData.CurrentMp >= bookData.MaxMpCapacity)
				{
					Spell2005GrimoireData spell2005GrimoireData = bookData;
					if (!spell2005GrimoireData.Teleporting && !spell2005GrimoireData.IsChildTeammateReachLimit)
					{
						EnterState(Spell2005State.OpenBook, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
					}
				}
			}
			break;
		case Spell2005State.OpenBook:
			UpdateBookOpenFrame(teammateData.IsFuseTeammate ? effect3 : effect, teammateData.TeammateSpeedRatio, isFireColor);
			bookData.AnimationTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			if (bookData.AnimationTimer >= 0.3f)
			{
				EnterState(Spell2005State.Attacking, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			}
			break;
		case Spell2005State.Attacking:
		{
			UpdateLoopAttackFrame(teammateData.IsFuseTeammate ? effect4 : effect2, teammateData.TeammateSpeedRatio, isFireColor);
			if (bookData.AttackDuration > 0f)
			{
				bookData.AttackTimer -= DeltaTime;
			}
			bool flag = false;
			bookData.CloseBookTimer += DeltaTime;
			if (bookData.IsLowCostSpell)
			{
				if (bookData.CloseBookTimer >= 2f)
				{
					flag = true;
				}
			}
			else
			{
				Spell2005GrimoireData spell2005GrimoireData = bookData;
				if (spell2005GrimoireData.AttackTimer <= 0f && spell2005GrimoireData.CloseBookTimer >= 0.3f && (bookData.ReleaseChargeDuration <= 0f || bookData.ReleaseChargeSpell))
				{
					flag = true;
				}
			}
			if (!LocalTransformLookUp.HasComponent(movement.ChaseTarget))
			{
				flag = true;
			}
			if (bookData.IsChildTeammateReachLimit)
			{
				flag = true;
			}
			bool flag2 = SpellDashDriverSingleton.IsShooterDriving(entity);
			if (bookData.CurrentMp >= bookData.MaxMpCapacity && bookData.AttackTimer <= 0f && !flag2 && (bookData.ReleaseChargeDuration <= 0f || bookData.ReleaseChargeSpell) && LocalTransformLookUp.HasComponent(movement.ChaseTarget))
			{
				LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
				if (DTool.IgnoreZDistance(in localTransform2.Position, in localTransform.Position) <= num2 && !flag)
				{
					bookData.ReadyToAttack = true;
					bookData.AttackTimer += bookData.AttackDuration;
					bookData.CurrentMp -= bookData.MaxMpCapacity;
					bookData.CloseBookTimer = 0f;
				}
			}
			if (bookData.ReleaseChargeDuration >= 0f)
			{
				bookData.ReleaseChargeTimer = math.max(bookData.ReleaseChargeTimer - DeltaTime, 0f);
				if (bookData.ReleaseChargeTimer <= 0f)
				{
					bookData.ReleaseChargeSpell = true;
				}
			}
			if (flag)
			{
				EnterState(Spell2005State.CloseBook, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			}
			break;
		}
		case Spell2005State.CloseBook:
			UpdateBookCloseFrame(teammateData.IsFuseTeammate ? effect3 : effect, teammateData.TeammateSpeedRatio, isFireColor);
			bookData.AnimationTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			if (bookData.AnimationTimer >= 0.3f)
			{
				EnterState(Spell2005State.Idle, entity, spellData, ref bookData, ref teammateData, ref boboPpt, ref unitBase, ref movement, Random, isFireColor);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateBookProgressBarHideState(ref Spell2005GrimoireData bookData, SpellComponentData spellData, TeammateData teammateda)
	{
		Entity effect = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2;
		Entity effect2 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect4;
		Entity effect3 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect5;
		float progress = 0f;
		if (teammateda.IsFuseMaterial)
		{
			progress = 1f;
		}
		else if (teammateda.TeammateDelayDeathEffectActive)
		{
			progress = 0.5f;
		}
		else if (bookData.Teleporting)
		{
			progress = ((!bookData.TeleportDone) ? (bookData.TeleportProgressTimer / 0.35f) : (1f - (bookData.TeleportProgressTimer - 0.35f) / 0.25000003f));
		}
		HideProgressLookUp.GetRefRW(effect2).ValueRW.Progress = progress;
		HideProgressLookUp.GetRefRW(effect3).ValueRW.Progress = progress;
	}

	private void UpdateTeleportState(ref Spell2005GrimoireData bookData, SpellMovementComponentData movement, TeammateData teammateData, Entity entity, GlobalRandom random, ref PathFinding pathFinding, int chunkIndex, SpellComponentData spellData, float bookFinalAttakRange, bool isFireColor = false)
	{
		if (teammateData.AdvanceSkillLevel <= 0)
		{
			return;
		}
		bookData.TeleportCoolDownTimer -= DeltaTime;
		if (bookData.Teleporting)
		{
			float num = 0f;
			num = ((!bookData.TeleportDone) ? (bookData.TeleportProgressTimer / 0.35f) : (1f - (bookData.TeleportProgressTimer - 0.35f) / 0.25000003f));
			bookData.TeleportProgressTimer += DeltaTime * teammateData.TeammateSpeedRatio;
			Spell2005GrimoireData spell2005GrimoireData = bookData;
			if (spell2005GrimoireData.TeleportProgressTimer >= 0.35f && !spell2005GrimoireData.TeleportDone)
			{
				bookData.TeleportDone = true;
				float3 @float = (LocalTransformLookUp.HasComponent(movement.ChaseTarget) ? LocalTransformLookUp[movement.ChaseTarget].Position : LocalTransformLookUp.GetRefRW(entity).ValueRW.Position);
				float3 float2 = ((pathFinding.samplePointRequest.requestState == NavMeshRequestState.Completed) ? ((float3)pathFinding.samplePointRequest.result) : @float);
				LocalTransformLookUp.GetRefRW(entity).ValueRW.Position = float2;
				CMD.AppendToBuffer(chunkIndex, SpellEffectEntity, new SpellEffectSystem.UnfollowingRequire
				{
					StartPosition = float2 + new float3(0f, bookData.CurrentBaseHeight, 0f),
					SpellId = 2005,
					Color = "Player",
					StartRotation = quaternion.identity,
					Scale = 1f,
					Settings = new SpellEffect
					{
						ScaleMode = SpellEffectSystem.ScaleMode.Scale,
						DestroyDelay = 0.5f,
						IgnoreColor = true,
						Name = "Teleport",
						Layer = LayerCorrectType.Coordinate
					}
				});
			}
			if (bookData.TeleportProgressTimer >= 0.6f)
			{
				num = 0f;
				bookData.Teleporting = false;
				bookData.TeleportDone = false;
				bookData.TeleportCoolDownTimer += 1f;
				bookData.TeleportProgressTimer = 0f;
				if (pathFinding.samplePointRequest.requestState == NavMeshRequestState.Completed)
				{
					pathFinding.samplePointRequest.Reset();
				}
			}
			Entity effect = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1;
			Entity effect2 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect5;
			Entity effect3 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect1;
			Entity effect4 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect2;
			Entity effect5 = EffectsCollectorLookUp.GetRefRW(effect).ValueRW.Effect3;
			Entity effect6 = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2;
			Entity effect7 = EffectsCollectorLookUp.GetRefRW(effect6).ValueRW.Effect1;
			Entity effect8 = EffectsCollectorLookUp.GetRefRW(effect6).ValueRW.Effect2;
			Entity effect9 = EffectsCollectorLookUp.GetRefRW(effect6).ValueRW.Effect3;
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect3
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect4
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect5
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect7
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect8
			});
			CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
			{
				ett = effect9
			});
			TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect7 : effect3).ValueRW.Progress = num;
			TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect8 : effect4).ValueRW.Progress = num;
			TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect9 : effect5).ValueRW.Progress = num;
			TwirlProgressLookUp.GetRefRW(effect2).ValueRW.Progress = 1f - num;
			if (isFireColor)
			{
				Entity effect10 = EffectsCollectorLookUp.GetRefRW(effect3).ValueRW.Effect1;
				Entity effect11 = EffectsCollectorLookUp.GetRefRW(effect4).ValueRW.Effect1;
				Entity effect12 = EffectsCollectorLookUp.GetRefRW(effect5).ValueRW.Effect1;
				Entity effect13 = EffectsCollectorLookUp.GetRefRW(effect7).ValueRW.Effect1;
				Entity effect14 = EffectsCollectorLookUp.GetRefRW(effect8).ValueRW.Effect1;
				Entity effect15 = EffectsCollectorLookUp.GetRefRW(effect9).ValueRW.Effect1;
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect10
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect11
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect12
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect13
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect14
				});
				CMD.AppendToBuffer(chunkIndex, entity, new UnitMREttBED
				{
					ett = effect15
				});
				TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect13 : effect10).ValueRW.Progress = num;
				TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect14 : effect11).ValueRW.Progress = num;
				TwirlProgressLookUp.GetRefRW(teammateData.IsFuseTeammate ? effect15 : effect12).ValueRW.Progress = num;
			}
		}
		if (!(bookData.TeleportCoolDownTimer >= 0f) && LocalTransformLookUp.HasComponent(movement.ChaseTarget))
		{
			LocalTransform localTransform = LocalTransformLookUp[movement.ChaseTarget];
			ref float3 position = ref localTransform.Position;
			LocalTransform localTransform2 = LocalTransformLookUp[entity];
			float num2 = DTool.IgnoreZDistance(in position, in localTransform2.Position);
			if ((!(num2 < bookFinalAttakRange) || !(num2 > bookFinalAttakRange * 0.2f) || bookData.Teleporting) && !bookData.Teleporting)
			{
				bookData.Teleporting = true;
				float3 position2 = random.random.NextFloat3Direction();
				float3 float3 = DTool.IgnoreZPosition(in position2);
				float num3 = random.random.NextFloat(0.4f, 0.8f);
				pathFinding.samplePointRequest.SetRequest(LocalTransformLookUp[movement.ChaseTarget].Position + float3 * num3 * bookFinalAttakRange);
				float scale = LocalTransformLookUp[entity].Scale;
				GlobalParticleEmitParams element = new GlobalParticleEmitParams(GlobalParticleType.Spell, "2005_Teleport", LocalTransformLookUp[entity].Position + new float3(0f, bookData.CurrentBaseHeight * scale, 0f))
				{
					Size = scale
				};
				CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element);
			}
		}
	}

	private void MoveToTarget(ref PathFinding pathFinding, ref LocalTransform localTransform, ref SpellMovementComponentData movement, ref UnitProperty_Dots boboPpt, ref UnitBase_Dots unitBase, ref Spell2005GrimoireData bookData, ref PhysicsVelocity velocity, float bookFinalAttakRange)
	{
		if (LocalTransformLookUp.HasComponent(movement.ChaseTarget))
		{
			LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
			if (DTool.IgnoreZDistance(in localTransform2.Position, in localTransform.Position) > bookFinalAttakRange && movement.Type != SpellSpecialMovementType.Rotation)
			{
				pathFinding.UpdatePath(localTransform.Position, LocalTransformLookUp[movement.ChaseTarget].Position, boboPpt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * movement.Speed);
				return;
			}
		}
		if (LocalTransformLookUp.HasComponent(movement.ChaseTarget))
		{
			LocalTransform localTransform2 = LocalTransformLookUp[movement.ChaseTarget];
			if (DTool.IgnoreZDistance(in localTransform2.Position, in localTransform.Position) < bookFinalAttakRange - 0.5f && bookData.IsRotation)
			{
				pathFinding.UpdatePath(localTransform.Position, Tool2D.IgnoreZPoint(LocalTransformLookUp[movement.ChaseTarget].Position - localTransform.Position).normalized * bookFinalAttakRange, boboPpt.navAreaMask);
				unitBase.SetMoveWithVelocity(ref velocity, ref movement, Tool2D.IgnoreZPoint(pathFinding.walkToPoint - localTransform.Position).normalized * movement.Speed);
				return;
			}
		}
		unitBase.SetMoveWithVelocity(ref velocity, ref movement, float3.zero);
	}

	private void UpdateChaseTarget(ref Spell2005GrimoireData bookData, ref SpellMovementComponentData movement, Entity entity)
	{
		if (!(bookData.MaxMpCapacity <= 0f))
		{
			bookData.UpdateChaseTargetTimer += DeltaTime;
			if (bookData.UpdateChaseTargetTimer >= 0.2f || !LocalTransformLookUp.HasComponent(movement.ChaseTarget))
			{
				CurrentRoomEntities.FindNearestTarget(LocalTransformLookUp[entity].Position, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
				bookData.UpdateChaseTargetTimer = 0f;
			}
		}
	}

	private void UpdateProgressBarState(Spell2005GrimoireData bookData, SpellComponentData spellData)
	{
		if (bookData.MaxMpCapacity != 0f)
		{
			EffectsCollectorData valueRW = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW;
			ProgressBarLookUp.GetRefRW(valueRW.Effect3).ValueRW.Progress = math.clamp(bookData.CurrentMp / bookData.MaxMpCapacity, 0f, 1f);
		}
	}

	private void UpdataFloatingState(ref Spell2005GrimoireData BookData, SpellComponentData spellData, float DeltaTime)
	{
		float num = Mathf.Sin(BookData.BookFloatingTimer) * 0.15f;
		if (num < 0f)
		{
			num *= 0.5f;
		}
		BookData.BookFloatingTimer += DeltaTime * 3.5f;
		RefRW<LocalTransform> refRW = LocalTransformLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect4);
		switch (BookData.State)
		{
		case Spell2005State.Initialize:
		case Spell2005State.Idle:
		case Spell2005State.ChasingTarget:
			BookData.CurrentBaseHeight = DTool.Lerp(BookData.CurrentBaseHeight, 0.4f, 6f * DeltaTime);
			refRW.ValueRW.Position = new float3(0f, BookData.CurrentBaseHeight + num, 0f);
			BookData.BookFloatingHeight = BookData.CurrentBaseHeight + num;
			break;
		case Spell2005State.OpenBook:
		case Spell2005State.Attacking:
		case Spell2005State.CloseBook:
			BookData.CurrentBaseHeight = DTool.Lerp(BookData.CurrentBaseHeight, 0.3f, 6f * DeltaTime);
			refRW.ValueRW.Position = new float3(0f, BookData.CurrentBaseHeight + num / 2f, 0f);
			BookData.BookFloatingHeight = BookData.CurrentBaseHeight + num / 2f;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void EnterState(Spell2005State bookState, Entity entity, SpellComponentData spellData, ref Spell2005GrimoireData BookData, ref TeammateData teammateData, ref UnitProperty_Dots myPpt, ref UnitBase_Dots unitBase, ref SpellMovementComponentData movementData, GlobalRandom random, bool isFireColor)
	{
		BookData.State = bookState;
		EffectsCollectorData valueRW = EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW;
		EffectsCollectorData valueRW2 = EffectsCollectorLookUp.GetRefRW(valueRW.Effect1).ValueRW;
		EffectsCollectorData valueRW3 = EffectsCollectorLookUp.GetRefRW(valueRW.Effect2).ValueRW;
		switch (bookState)
		{
		case Spell2005State.Initialize:
			HideTargetAnimaEntity(valueRW2.Effect1, isFireColor);
			HideTargetAnimaEntity(valueRW2.Effect2, isFireColor);
			HideTargetAnimaEntity(valueRW2.Effect3, isFireColor);
			HideTargetAnimaEntity(valueRW3.Effect1, isFireColor);
			HideTargetAnimaEntity(valueRW3.Effect2, isFireColor);
			HideTargetAnimaEntity(valueRW3.Effect3, isFireColor);
			BookData.AnimationTimer = 0f;
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? valueRW3.Effect1 : valueRW2.Effect1, isFireColor);
			BookData.CurrentBaseHeight = 0.4f;
			if (teammateData.IsFuseTeammate)
			{
				FuseBodyRepeatCounterLookUp.GetRefRW(valueRW3.Effect1).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				FuseBodyRepeatCounterLookUp.GetRefRW(valueRW3.Effect2).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				FuseBodyRepeatCounterLookUp.GetRefRW(valueRW3.Effect3).ValueRW.RepeatCounter = teammateData.TeammateCurrentFuseLevel - 1;
				float3 @float = DTool.Float44ToFloat3(in FreeScaleLookUp.GetRefRW(valueRW3.Effect3).ValueRW.Value);
				@float.x = 1f + (float)teammateData.TeammateCurrentFuseLevel * 0.5f;
				FreeScaleLookUp.GetRefRW(valueRW3.Effect1).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
				FreeScaleLookUp.GetRefRW(valueRW3.Effect2).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
				FreeScaleLookUp.GetRefRW(valueRW3.Effect3).ValueRW.Value = Matrix4x4.Scale((Vector3)@float);
			}
			if (teammateData.AdvanceSkillLevel > 0)
			{
				BookData.SpellCastCounter = random.random.NextInt(0, 10);
			}
			if (BookData.MaxMpCapacity == 0f || BookData.ManaRegenPerSecond >= BookData.MaxMpCapacity)
			{
				LocalTransformLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect3).ValueRW.Scale = 0f;
			}
			break;
		case Spell2005State.Idle:
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? valueRW3.Effect1 : valueRW2.Effect1, isFireColor);
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? valueRW3.Effect2 : valueRW2.Effect2, isFireColor);
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? valueRW3.Effect3 : valueRW2.Effect3, isFireColor);
			break;
		case Spell2005State.OpenBook:
			BookData.CloseBookTimer = 0f;
			BookData.AnimationTimer = 0f;
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? valueRW3.Effect1 : valueRW2.Effect1, isFireColor);
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? valueRW3.Effect3 : valueRW2.Effect3, isFireColor);
			break;
		case Spell2005State.Attacking:
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? valueRW3.Effect3 : valueRW2.Effect3, isFireColor);
			ResetTargetAnimaEntityFrame(teammateData.IsFuseTeammate ? valueRW3.Effect2 : valueRW2.Effect2, isFireColor);
			BookData.AttackTimer = 0f;
			break;
		case Spell2005State.CloseBook:
			BookData.AnimationTimer = 0f;
			HideTargetAnimaEntity(teammateData.IsFuseTeammate ? valueRW3.Effect2 : valueRW2.Effect2, isFireColor);
			FrameAnimeLookUp.GetRefRW(teammateData.IsFuseTeammate ? valueRW3.Effect3 : valueRW2.Effect3).ValueRW.FrameIndex = 11.9f;
			break;
		default:
			throw new ArgumentOutOfRangeException("bookState", bookState, null);
		case Spell2005State.ChasingTarget:
			break;
		}
	}

	private void UpdataCurrentMp(ref Spell2005GrimoireData bookData)
	{
		if (!(bookData.CurrentMp >= bookData.MaxMpCapacity))
		{
			bookData.CurrentMp += bookData.ManaRegenPerSecond * DeltaTime;
		}
	}

	private void UpdateBookOpenFrame(Entity targetEntity, float speedRatio, bool isFireColor)
	{
		RefRW<MatOverrideFrameIndex> refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		float num = math.min(refRW.ValueRW.FrameIndex + DeltaTime * speedRatio * 36f, 11.5f);
		refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		refRW.ValueRW.FrameIndex = num;
		if (isFireColor)
		{
			refRW = FrameAnimeLookUp.GetRefRW(EffectsCollectorLookUp[targetEntity].Effect1);
			refRW.ValueRW.FrameIndex += num;
		}
	}

	private void UpdateBookCloseFrame(Entity targetEntity, float speedRatio, bool isFireColor)
	{
		RefRW<MatOverrideFrameIndex> refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		float num = math.max(refRW.ValueRW.FrameIndex - DeltaTime * speedRatio * 36f, 0f);
		refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		refRW.ValueRW.FrameIndex = num;
		if (isFireColor)
		{
			refRW = FrameAnimeLookUp.GetRefRW(EffectsCollectorLookUp[targetEntity].Effect1);
			refRW.ValueRW.FrameIndex += num;
		}
	}

	private void UpdateLoopAttackFrame(Entity targetEntity, float speedRatio, bool isFireColor)
	{
		RefRW<MatOverrideFrameIndex> refRW = FrameAnimeLookUp.GetRefRW(targetEntity);
		refRW.ValueRW.FrameIndex += DeltaTime * speedRatio * 18f;
		if (isFireColor)
		{
			refRW = FrameAnimeLookUp.GetRefRW(EffectsCollectorLookUp[targetEntity].Effect1);
			refRW.ValueRW.FrameIndex += DeltaTime * speedRatio * 18f;
		}
	}

	private void ResetTargetAnimaEntityFrame(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = 0f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(EffectsCollectorLookUp[targetEntity].Effect1).ValueRW.FrameIndex = 0f;
		}
	}

	private void HideTargetAnimaEntity(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = -0.1f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(EffectsCollectorLookUp[targetEntity].Effect1).ValueRW.FrameIndex = -0.1f;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2005GrimoireData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr11 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2005GrimoireData bookData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2005GrimoireData>(nativeArrayPtr, i);
				ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, i);
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				Execute(ref bookData, ref spellData, ref movement, ref teammateData, ref localTransform, chunkIndexInQuery, ref unitBase, ref pathFinding, entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr11, i));
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
					ref Spell2005GrimoireData bookData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2005GrimoireData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, nextRangeBegin);
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					Execute(ref bookData2, ref spellData2, ref movement2, ref teammateData2, ref localTransform2, chunkIndexInQuery, ref unitBase2, ref pathFinding2, entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr11, nextRangeBegin));
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
				ref Spell2005GrimoireData bookData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2005GrimoireData>(nativeArrayPtr, j);
				ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, j);
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				Execute(ref bookData3, ref spellData3, ref movement3, ref teammateData3, ref localTransform3, chunkIndexInQuery, ref unitBase3, ref pathFinding3, entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr11, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2005GrimoireData bookData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2005GrimoireData>(nativeArrayPtr, k);
				ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, k);
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr6, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				Execute(ref bookData4, ref spellData4, ref movement4, ref teammateData4, ref localTransform4, chunkIndexInQuery, ref unitBase4, ref pathFinding4, entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr9, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr10, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr11, k));
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
