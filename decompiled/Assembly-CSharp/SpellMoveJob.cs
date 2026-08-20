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
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
[WithNone(new Type[]
{
	typeof(TeammateDisableSpellMovementTag),
	typeof(SpellKeepCastingAttach),
	typeof(Spell1025DragonBreathData),
	typeof(Spell4013RuneHammerData),
	typeof(Spell2007SuicideBugNestData),
	typeof(Spell2007SuicideBugData),
	typeof(Spell4012MagicShieldData),
	typeof(Spell4024DaveHarpoonData)
})]
public struct SpellMoveJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<TeammateDisableSpellMovementTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<SpellKeepCastingAttach>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell1025DragonBreathData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell4013RuneHammerData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell2007SuicideBugNestData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell2007SuicideBugData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell4012MagicShieldData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell4024DaveHarpoonData>();
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
		public void Run(ref SpellMoveJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref SpellMoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref SpellMoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref SpellMoveJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref SpellMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref SpellMoveJob job, EntityManager entityManager)
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

	public GlobalRandom Random;

	public Entity SpellReboundEffectPrefab;

	public float PlayerMoveSpeed;

	public float3 MousePosition;

	public float DeltaTime;

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[ReadOnly]
	public BufferLookup<StatefulCollisionEvent> CollisionEventsLookup;

	[ReadOnly]
	public ComponentLookup<PhysicsCollider> PhysicsColliderLookup;

	[ReadOnly]
	public ComponentLookup<SpellParabolaComponentData> ParabolaLookup;

	[ReadOnly]
	public ComponentLookup<Spell1003ButterflyData> ButterFlyDataLookUp;

	[ReadOnly]
	public ComponentLookup<Spell1006GhostFireData> GhostFireDataLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1023JudgementBladeData> JudgementBladeDataLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1022BoomerangData> BoomerangDataLookUp;

	[ReadOnly]
	public ComponentLookup<Spell1021MagicBreakerData> MagicBreakerDataLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell4027BlueRuneData> BlueRuneDataLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellMoveTriggerComponentData> MoveTriggerLookup;

	[ReadOnly]
	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[ReadOnly]
	public ComponentLookup<TeammateData> TeammateDataLookUp;

	[ReadOnly]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookUp;

	[ReadOnly]
	public ComponentLookup<Spell4019BiAnBladeData> BiAnBladeDataLookUp;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int chunkIndex, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref LocalTransform transform, ref SpellConfigComponentData config, ref SpellComponentData data, Entity spellEtt)
	{
		if (CollisionEventsLookup.TryGetBuffer(spellEtt, out var bufferData))
		{
			foreach (StatefulCollisionEvent item in bufferData)
			{
				if (item.State != StatefulEventState.Enter)
				{
					continue;
				}
				if (TeammateDataLookUp.HasComponent(spellEtt))
				{
					if (config.AbilityType == SpellAbilityType.Summon4 && movement.Type != SpellSpecialMovementType.ChaseMouse)
					{
						movement.Direction = math.normalize(velocity.Linear);
						movement.ReboundCount = 0;
					}
					continue;
				}
				float3 direction = math.normalize(velocity.Linear);
				direction.z = 0f;
				movement.Direction = direction;
				if (config.AbilityType != SpellAbilityType.Dash)
				{
					movement.ReboundCount--;
					config.Duration.Extra += movement.ReboundAddTime;
				}
				if (movement.ReboundCount <= 0)
				{
					PhysicsCollider collider = PhysicsColliderLookup[spellEtt];
					SpellTools.DisableSpellReboundCollider(in collider);
				}
				Entity e = CMD.Instantiate(chunkIndex, SpellReboundEffectPrefab);
				CMD.SetComponent(chunkIndex, e, LocalTransform.FromPosition(transform.Position));
				break;
			}
		}
		SpellConfigComponentData spellConfigComponentData = config;
		if (spellConfigComponentData.HoverTimer > 0f && spellConfigComponentData.HoverDuration > 0f)
		{
			movement.Speed = 0f;
			velocity.Linear = float3.zero;
		}
		else
		{
			velocity.Linear = velocity.Linear.IgnoreZ();
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
				switch (config.AbilityType)
				{
				case SpellAbilityType.Butterfly:
					Butterfly_Common(ref movement, ref velocity, ref config, spellEtt, transform);
					break;
				case SpellAbilityType.Summon4:
					if (movement.ReboundCount > 0)
					{
						Normal_Common(ref movement, ref velocity, ref config);
					}
					break;
				case SpellAbilityType.HoverTorch:
					GhostFire_Common(ref movement, ref velocity, ref config, spellEtt);
					break;
				case SpellAbilityType.JudgementBlade:
					JudgementBlade_Common(ref movement, ref velocity, ref config, spellEtt);
					break;
				case SpellAbilityType.Boomerang:
					Boomerang_Common(ref movement, ref velocity, ref config, ref transform, ref BoomerangDataLookUp.GetRefRW(spellEtt).ValueRW, in data);
					break;
				case SpellAbilityType.MagicBreaker:
					if (!movement.IsFallSpell || MagicBreakerDataLookUp[spellEtt].readyToDestroy)
					{
						break;
					}
					goto default;
				case SpellAbilityType.HighPressureWasher:
					if (config.Int3 == 0)
					{
						break;
					}
					goto default;
				case SpellAbilityType.BlueRune:
					if (movement.IsFallSpell)
					{
						Normal_Common(ref movement, ref velocity, ref config);
					}
					else
					{
						BlueRune_Common(ref movement, ref velocity, ref config, transform, in data, spellEtt);
					}
					break;
				default:
					Normal_Common(ref movement, ref velocity, ref config);
					break;
				}
				break;
			case SpellSpecialMovementType.Rotation:
				switch (config.AbilityType)
				{
				case SpellAbilityType.JudgementBlade:
					JudgementBlade_Rotation(ref movement, ref transform, ref velocity, ref config, spellEtt);
					break;
				case SpellAbilityType.Boomerang:
					Boomerang_Rotation(ref movement, ref velocity, ref config, ref transform, ref BoomerangDataLookUp.GetRefRW(spellEtt).ValueRW, in data, spellEtt);
					break;
				case SpellAbilityType.MagicBreaker:
					if (!movement.IsFallSpell || MagicBreakerDataLookUp[spellEtt].readyToDestroy)
					{
						break;
					}
					goto default;
				case SpellAbilityType.HighPressureWasher:
					if (config.Int3 == 0)
					{
						float3 position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
						position.z = (data.IsSplitSpell ? transform.Position.z : (movement.IsFallSpell ? (-7f) : (-0.3f)));
						transform.Position = position;
						break;
					}
					goto default;
				case SpellAbilityType.GreenRune:
					if (movement.IsFallSpell)
					{
						Normal_Rotation(ref movement, ref transform, ref velocity, spellEtt);
					}
					else
					{
						GreenRune_Rotation(ref movement, ref transform, ref velocity);
					}
					break;
				case SpellAbilityType.BlueRune:
					if (movement.IsFallSpell)
					{
						Normal_Rotation(ref movement, ref transform, ref velocity, spellEtt);
					}
					else
					{
						BlueRune_Rotation(ref movement, ref transform, ref velocity, ref data, ref config, spellEtt);
					}
					break;
				default:
					Normal_Rotation(ref movement, ref transform, ref velocity, spellEtt);
					break;
				case SpellAbilityType.Summon3:
				case SpellAbilityType.BiAnLethalBlade:
					break;
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
				switch (config.AbilityType)
				{
				case SpellAbilityType.Butterfly:
					Butterfly_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.HoverTorch:
					GhostFire_Mouse(ref transform, ref movement, ref velocity, spellEtt);
					break;
				case SpellAbilityType.Meteor:
					Meteor_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.Rainbow:
					Rainbow_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.JudgementBlade:
					JudgementBlade_ChaseMouse(ref movement, ref velocity, ref transform, ref config, spellEtt);
					break;
				case SpellAbilityType.Boomerang:
					Boomerang_ChaseMouse(ref movement, ref velocity, ref config, ref transform, ref BoomerangDataLookUp.GetRefRW(spellEtt).ValueRW, in data);
					break;
				case SpellAbilityType.Summon4:
					Summon4_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.MagicBreaker:
					if (!movement.IsFallSpell || MagicBreakerDataLookUp[spellEtt].readyToDestroy)
					{
						break;
					}
					goto default;
				case SpellAbilityType.SnakeWalk:
					Snake_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.Dash:
					Dash_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.HighPressureWasher:
					if (config.Int3 == 0)
					{
						break;
					}
					goto default;
				case SpellAbilityType.BlueRune:
					if (movement.IsFallSpell)
					{
						Normal_Mouse(ref transform, ref movement, ref velocity);
					}
					else
					{
						BlueRune_Mouse(ref transform, ref movement, ref velocity, ref config, spellEtt);
					}
					break;
				default:
					Normal_Mouse(ref transform, ref movement, ref velocity);
					break;
				case SpellAbilityType.BiAnLethalBlade:
					break;
				}
				break;
			case SpellSpecialMovementType.ChaseEnemy:
			case SpellSpecialMovementType.ChaseOwner:
				switch (config.AbilityType)
				{
				case SpellAbilityType.HoverTorch:
					GhostFire_ChaseTarget(ref movement, in data, ref velocity, ref config, transform, spellEtt);
					break;
				case SpellAbilityType.Meteor:
					Meteor_ChaseTarget(ref movement, in data, ref velocity, ref config, transform);
					break;
				case SpellAbilityType.JudgementBlade:
					JudgementBlade_ChaseTarget(ref movement, in data, ref velocity, ref config, transform, spellEtt);
					break;
				case SpellAbilityType.MagicBreaker:
					if (!movement.IsFallSpell || MagicBreakerDataLookUp[spellEtt].readyToDestroy)
					{
						break;
					}
					goto default;
				case SpellAbilityType.Boomerang:
					Boomerang_ChaseTarget(ref movement, ref velocity, ref config, ref transform, ref BoomerangDataLookUp.GetRefRW(spellEtt).ValueRW, in data);
					break;
				case SpellAbilityType.Summon4:
					Summon4_ChaseTarget(ref movement, in data, ref velocity, ref config, transform);
					break;
				case SpellAbilityType.BiAnLethalBlade:
				{
					if (BiAnBladeDataLookUp.TryGetComponent(spellEtt, out var componentData) && componentData.StateCurrent == 1)
					{
						Normal_ChaseTarget(ref movement, in data, ref velocity, ref config, transform);
					}
					break;
				}
				case SpellAbilityType.HighPressureWasher:
					if (config.Int3 == 0)
					{
						break;
					}
					goto default;
				case SpellAbilityType.BlueRune:
					if (movement.IsFallSpell)
					{
						Normal_ChaseTarget(ref movement, in data, ref velocity, ref config, transform);
					}
					else
					{
						BlueRune_Common(ref movement, ref velocity, ref config, transform, in data, spellEtt);
					}
					break;
				default:
					Normal_ChaseTarget(ref movement, in data, ref velocity, ref config, transform);
					break;
				}
				break;
			}
		}
		SpellAbilityType abilityType = config.AbilityType;
		if (abilityType != SpellAbilityType.FireBall)
		{
			if (abilityType != SpellAbilityType.HighPressureWasher || config.Int3 != 0)
			{
				goto IL_083e;
			}
		}
		else
		{
			if (movement.IsFallSpell)
			{
				goto IL_083e;
			}
			TrickMine_Fall(ref transform, ref movement, ref velocity, ref config);
		}
		goto IL_0861;
		IL_083e:
		Normal_Fall(ref transform, ref movement, ref velocity, ref config, spellEtt, out var fallGrounded);
		if (fallGrounded)
		{
			CMD.SetComponentEnabled<SpellGroundedTag>(chunkIndex, spellEtt, value: true);
		}
		goto IL_0861;
		IL_0861:
		transform.Position += new float3(0f, 0f, movement.CurrentFallSpeed * DeltaTime);
		transform.Position.z = math.min(0f, transform.Position.z);
	}

	private void TrickMine_Fall(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config)
	{
		if (!(movement.Gravity > 0f))
		{
			return;
		}
		movement.CurrentFallSpeed += movement.Gravity * DeltaTime;
		if (transform.Position.z >= -0.01f && movement.CurrentFallSpeed > 0f)
		{
			transform.Position = transform.Position.IgnoreZ();
			movement.CurrentFallSpeed *= (0f - movement.FallingReboundForceRatio) * 1.6f;
			movement.Speed *= 0.66f;
			config.Float3 *= 0.66f;
			if (math.abs(movement.CurrentFallSpeed) <= 0.2f || movement.Speed <= 0.2f)
			{
				movement.Speed = 0f;
				movement.Gravity = 0f;
				movement.CurrentFallSpeed = 0f;
				config.Float3 = 0f;
				movement.Type = SpellSpecialMovementType.Normal;
			}
		}
	}

	private void Normal_Fall(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spell, out bool fallGrounded)
	{
		fallGrounded = false;
		if (movement.IsFallRebounded || ParabolaLookup.HasComponent(spell))
		{
			movement.CurrentFallSpeed += movement.Gravity * DeltaTime;
		}
		if (!(transform.Position.z >= -0.001f) || !(movement.CurrentFallSpeed > 0f))
		{
			return;
		}
		fallGrounded = true;
		transform.Position = transform.Position.IgnoreZ();
		if (movement.ReboundCount > 0)
		{
			if (!movement.IsFallSpell && ParabolaLookup.TryGetComponent(spell, out var componentData))
			{
				movement.CurrentFallSpeed *= 0f - componentData.BounceRatio;
			}
			else
			{
				movement.ReboundFallSpeed();
			}
		}
		movement.IsFallRebounded = true;
	}

	private void Rainbow_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * movement.Speed;
		velocity.Linear = math.lerp(velocity.Linear, end, 100f * DeltaTime * movement.ChaseMouseLerpSpeed);
		movement.Direction = math.normalize(velocity.Linear);
	}

	private void Summon4_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		movement.Direction = Vector3.Lerp((Vector3)movement.Direction, (Vector3)DTool.IgnoreZDir(in MousePosition, in transform.Position), movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed);
		velocity.Linear = movement.Direction * movement.Speed;
	}

	private void BlueRune_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spell)
	{
		if (!BlueRuneDataLookUp.HasComponent(spell))
		{
			return;
		}
		RefRW<Spell4027BlueRuneData> refRW = BlueRuneDataLookUp.GetRefRW(spell);
		if (refRW.ValueRW.NeedResetChaseTargetAngleSpeed)
		{
			refRW.ValueRW.NeedResetChaseTargetAngleSpeed = false;
			refRW.ValueRW.ChaseMouseAngleRotateSpeed = Random.random.NextFloat(30f, 150f);
		}
		if (config.DurationTimer >= refRW.ValueRW.NormalIgnoreChaseDuration)
		{
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			if (refRW.ValueRW.IsSuperBlueRune)
			{
				num *= 1.5f;
			}
			if (DTool.IgnoreZDistance(in MousePosition, in transform.Position) <= 0.6f)
			{
				num = 0f;
			}
			float3 target = DTool.IgnoreZDir(in MousePosition, in transform.Position);
			float2 a = movement.Direction.xy;
			float2 a2 = target.xy;
			float num2 = math.abs(DTool.GetDirOffset(in a, in a2));
			if (num2 <= 15f && refRW.ValueRW.NeedResetChaseTargetAngleSpeed)
			{
				refRW.ValueRW.NeedResetChaseTargetAngleSpeed = false;
			}
			if (num2 >= 90f && !refRW.ValueRW.NeedResetChaseTargetAngleSpeed)
			{
				refRW.ValueRW.NeedResetChaseTargetAngleSpeed = true;
			}
			float3 source = movement.Direction;
			float3 x = DTool.DirMoveTowardsIgnoreZ(in source, in target, movement.Speed * 270f * movement.ChaseMouseLerpSpeed * DeltaTime);
			if (num > 0f)
			{
				velocity.Linear = math.lerp(velocity.Linear, movement.Direction * num, refRW.ValueRW.ChaseMouseAngleRotateSpeed * DeltaTime * movement.ChaseMouseLerpSpeed);
				movement.Direction = math.normalize(x);
			}
			else
			{
				velocity.Linear = math.lerp(velocity.Linear, movement.Direction * num, 0.8f);
				movement.Direction = math.normalize(x);
			}
		}
		else if (movement.Speed <= 0f)
		{
			velocity.Linear = 0;
		}
		else
		{
			float3 end = DTool.IgnoreZDir(in refRW.ValueRO.NoTargetPosition, in transform.Position) * movement.Speed;
			velocity.Linear = math.lerp(velocity.Linear, end, math.min(movement.Speed * DeltaTime * 18f * refRW.ValueRO.NormalChasePower, 1f));
			movement.Direction = math.normalize(velocity.Linear);
		}
	}

	private void Normal_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		if (movement.Speed <= 0f)
		{
			velocity.Linear = 0;
			return;
		}
		float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * movement.Speed;
		velocity.Linear = math.lerp(velocity.Linear, end, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed);
		movement.Direction = math.normalize(velocity.Linear);
	}

	private void Boomerang_ChaseMouse(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, ref LocalTransform transform, ref Spell1022BoomerangData boomerang, in SpellComponentData data)
	{
		boomerang.extraLerpSpeed += 5f * DeltaTime;
		float3 @float = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
		float num = ((config.DurationTimer / (config.Duration.Calculate() * 0.33f) > 1f) ? 1 : 0);
		float3 to = MousePosition + (@float - MousePosition) * num;
		float3 float2 = math.lerp(movement.Direction, DTool.IgnoreZDir(in to, in transform.Position), (movement.Speed + boomerang.extraLerpSpeed) * DeltaTime * 0.05f * 1.5f);
		velocity.Linear = movement.Speed * float2;
		movement.Direction = float2;
	}

	private void Butterfly_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * movement.Speed;
		velocity.Linear = math.lerp(velocity.Linear, end, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed * 4f);
		movement.Direction = math.normalize(velocity.Linear);
	}

	private void GhostFire_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, Entity spellEntity)
	{
		float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * movement.Speed;
		velocity.Linear = math.lerp(velocity.Linear, end, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed * 2f);
		movement.Direction = math.normalize(velocity.Linear);
		velocity.Linear += GhostFireDataLookUp[spellEntity].PullForceByOtherGhostFire;
	}

	private void Meteor_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * (movement.IsFallSpell ? (movement.OriginalSpellHorizontalSpeed * 1.5f) : movement.Speed);
		velocity.Linear = math.lerp(velocity.Linear, end, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed);
		movement.Direction = math.normalize(velocity.Linear);
	}

	private void Snake_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		if (!movement.IsFallSpell)
		{
			float3 @float = DTool.IgnoreZDir(in MousePosition, in transform.Position);
			movement.Direction = math.normalizesafe(math.lerp(movement.Direction, @float * movement.Speed, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed));
		}
		else
		{
			Normal_Mouse(ref transform, ref movement, ref velocity);
		}
	}

	private void Dash_Mouse(ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity)
	{
		float num = DTool.IgnoreZDistance(in MousePosition, in transform.Position);
		float num2 = math.clamp(num, 0.6f, 1f);
		float3 @float = DTool.IgnoreZDir(in MousePosition, in transform.Position);
		float playerMoveSpeed = PlayerMoveSpeed;
		float3 end = (movement.IsFallSpell ? (@float * movement.Speed) : (@float * playerMoveSpeed * movement.Speed * 0.25f));
		if (num < 0.4f)
		{
			end = 0;
		}
		float t = math.clamp(movement.IsFallSpell ? (movement.Speed * DeltaTime * num2 * movement.ChaseMouseLerpSpeed) : ((movement.Speed + playerMoveSpeed) * DeltaTime * num2 * movement.ChaseMouseLerpSpeed), 0f, 1f);
		velocity.Linear = math.lerp(velocity.Linear, end, t);
		movement.Direction = math.normalizesafe(velocity.Linear);
	}

	private void Normal_Rotation(ref SpellMovementComponentData movement, ref LocalTransform transform, ref PhysicsVelocity velocity, Entity spell)
	{
		float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
		float num2 = 360f / (MathF.PI * 2f * movement.AroundRadius / num) * DeltaTime;
		if (MoveTriggerLookup.HasComponent(spell))
		{
			MoveTriggerLookup.GetRefRW(spell).ValueRW.DistanceCounter += MathF.PI / 180f * num2 * movement.AroundRadius;
		}
		movement.AroundAngle += num2;
		movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
		float3 position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
		position.z = transform.Position.z;
		transform.Position = position;
		velocity.Linear = float3.zero;
	}

	private void GreenRune_Rotation(ref SpellMovementComponentData movement, ref LocalTransform transform, ref PhysicsVelocity velocity)
	{
		movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
		float3 position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
		position.z = transform.Position.z;
		transform.Position = position;
		velocity.Linear = float3.zero;
	}

	private void BlueRune_Rotation(ref SpellMovementComponentData movement, ref LocalTransform transform, ref PhysicsVelocity velocity, ref SpellComponentData spellData, ref SpellConfigComponentData config, Entity spell)
	{
		RefRW<Spell4027BlueRuneData> refRW = BlueRuneDataLookUp.GetRefRW(spell);
		float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
		float num2 = 360f / (MathF.PI * 2f * movement.AroundRadius / num) * DeltaTime;
		if (MoveTriggerLookup.HasComponent(spell))
		{
			MoveTriggerLookup.GetRefRW(spell).ValueRW.DistanceCounter += MathF.PI / 180f * num2 * movement.AroundRadius;
		}
		GetValidChaseEnemyPosition(ref movement, in config, in transform, out var hasTarget, out var targetPosition);
		TransformLookup.TryGetComponent(movement.AroundTarget, out var componentData, out var entityExists);
		float3 point = (entityExists ? componentData.Position : movement.AroundCenter);
		if (hasTarget)
		{
			float num3 = math.clamp(DTool.IgnoreZDistance(in point, in targetPosition), 1f, refRW.ValueRW.MaxRotationRadius);
			float num4 = math.min(math.abs(num3 - refRW.ValueRW.CurrentRotationRadius), movement.Speed * DeltaTime * 0.1f * (refRW.ValueRW.IsSuperBlueRune ? 1.5f : 1f));
			if (num3 < refRW.ValueRW.CurrentRotationRadius)
			{
				num4 *= -1f;
			}
			refRW.ValueRW.CurrentRotationRadius += num4;
		}
		movement.AroundRadius = refRW.ValueRW.CurrentRotationRadius;
		movement.AroundAngle += num2;
		movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
		float3 position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(point);
		position.z = transform.Position.z;
		transform.Position = position;
		transform.Position.xy = (movement.AroundCenter + (float3)Tool2D.GetDir(movement.AroundAngle) * movement.AroundRadius).xy;
		velocity.Linear = float3.zero;
	}

	private void JudgementBlade_Rotation(ref SpellMovementComponentData movement, ref LocalTransform transform, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spell)
	{
		switch (JudgementBladeDataLookUp[spell].State)
		{
		case JudgementBladeState.Spawn:
		case JudgementBladeState.DetectingTarget:
		{
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			float num2 = 360f / (MathF.PI * 2f * movement.AroundRadius / num) * DeltaTime / 2f;
			movement.AroundAngle += num2;
			movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
			if (MoveTriggerLookup.HasComponent(spell))
			{
				MoveTriggerLookup.GetRefRW(spell).ValueRW.DistanceCounter += MathF.PI / 180f * num2 * movement.AroundRadius;
			}
			float3 position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
			position.z = transform.Position.z;
			transform.Position = position;
			velocity.Linear = float3.zero;
			break;
		}
		case JudgementBladeState.AfterShoot:
			JudgementBlade_Common(ref movement, ref velocity, ref config, spell);
			break;
		case JudgementBladeState.LockingTarget:
			break;
		}
	}

	private void Boomerang_Rotation(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, ref LocalTransform transform, ref Spell1022BoomerangData boomerang, in SpellComponentData data, Entity spell)
	{
		movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
		float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
		float num2 = 360f / (MathF.PI * 2f * movement.AroundRadius / num) * DeltaTime;
		movement.AroundAngle += num2;
		if (MoveTriggerLookup.HasComponent(spell))
		{
			MoveTriggerLookup.GetRefRW(spell).ValueRW.DistanceCounter += MathF.PI / 180f * num2 * movement.AroundRadius;
		}
		movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
		float percent = GetPercent(config.DurationTimer / config.Duration.Calculate(), 0.15f, 0.85f);
		percent = math.clamp(percent, 0f, 1f);
		transform.Position.xy = (movement.AroundCenter + (float3)Tool2D.GetDir(movement.AroundAngle) * percent * movement.AroundRadius).xy;
		velocity.Linear = float3.zero;
	}

	private float GetPercent(float durationPercent, float percentToIncrease, float percentToDecrease, float start = 0f, float max = 1f, float end = 0.025f)
	{
		if (durationPercent <= percentToIncrease)
		{
			return (max - start) * durationPercent / percentToIncrease + start;
		}
		if (durationPercent >= percentToDecrease)
		{
			return max - end - (max - end) * (durationPercent - percentToDecrease) / (1f - percentToDecrease) + end;
		}
		return max;
	}

	private void Boomerang_ChaseTarget(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, ref LocalTransform transform, ref Spell1022BoomerangData boomerang, in SpellComponentData data)
	{
		float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
		boomerang.extraLerpSpeed += DeltaTime * 5f;
		GetValidChasePosition(ref movement, in data, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget && config.DurationTimer <= config.Duration.Calculate() * 0.33f)
		{
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			float3 end = DTool.DirMoveTowardsIgnoreZ(in source, in target, num * movement.ChaseRotateSpeed * DeltaTime);
			float t = math.clamp(num * movement.ChaseRotateSpeed * DeltaTime, 0f, 1f);
			movement.Direction = math.lerp(movement.Direction, end, t);
		}
		else
		{
			float3 to = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
			movement.Direction = math.lerp(movement.Direction, DTool.IgnoreZDir(in to, in transform.Position), (boomerang.extraLerpSpeed + num) * DeltaTime * 0.05f);
		}
		velocity.Linear = movement.Direction * num;
	}

	private void Summon4_ChaseTarget(ref SpellMovementComponentData movement, in SpellComponentData spellData, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform)
	{
		GetValidChasePosition(ref movement, in spellData, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget)
		{
			movement.ReboundCount = 1;
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, num * movement.ChaseRotateSpeed * DeltaTime);
			velocity.Linear = movement.Direction * num;
		}
		else if (movement.ReboundCount > 0)
		{
			Normal_Common(ref movement, ref velocity, ref config);
		}
	}

	private void Normal_ChaseTarget(ref SpellMovementComponentData movement, in SpellComponentData spellData, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform)
	{
		GetValidChasePosition(ref movement, in spellData, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget)
		{
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, num * movement.ChaseRotateSpeed * DeltaTime);
			velocity.Linear = movement.Direction * movement.Speed;
		}
		else
		{
			Normal_Common(ref movement, ref velocity, ref config);
		}
	}

	private void JudgementBlade_ChaseTarget(ref SpellMovementComponentData movement, in SpellComponentData spellData, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform, Entity spell)
	{
		if (JudgementBladeDataLookUp[spell].State != JudgementBladeState.AfterShoot)
		{
			return;
		}
		SpellMovementComponentData spellMovementComponentData = movement;
		if (spellMovementComponentData.Type == SpellSpecialMovementType.ChaseEnemy && !spellMovementComponentData.IsFallRebounded && spellMovementComponentData.IsFallSpell)
		{
			float3 @float = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
			return;
		}
		GetValidChasePosition(ref movement, in spellData, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget)
		{
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, num * movement.ChaseRotateSpeed * DeltaTime);
			velocity.Linear = movement.Direction * num;
		}
		else
		{
			Normal_Common(ref movement, ref velocity, ref config);
		}
	}

	private void GhostFire_ChaseTarget(ref SpellMovementComponentData movement, in SpellComponentData spellData, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform, Entity spellEntity)
	{
		GetValidChasePosition(ref movement, in spellData, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget)
		{
			float num = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, num * movement.ChaseRotateSpeed * DeltaTime);
			velocity.Linear = movement.Direction * num;
		}
		else
		{
			Normal_Common(ref movement, ref velocity, ref config);
		}
		velocity.Linear += GhostFireDataLookUp[spellEntity].PullForceByOtherGhostFire;
	}

	private void Meteor_ChaseTarget(ref SpellMovementComponentData movement, in SpellComponentData spellData, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform)
	{
		GetValidChasePosition(ref movement, in spellData, in config, transform, out var hasTarget, out var targetPosition);
		if (hasTarget)
		{
			float speed = movement.Speed;
			float3 source = movement.Direction;
			float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
			movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, math.max(speed, 50f) * movement.ChaseRotateSpeed * DeltaTime * 5f);
			velocity.Linear = movement.Direction * speed;
		}
		else
		{
			Normal_Common(ref movement, ref velocity, ref config);
		}
	}

	private void Butterfly_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spell, LocalTransform transform)
	{
		if (ButterFlyDataLookUp[spell].StartTraceTargets)
		{
			bool hasTarget = false;
			float3 targetPosition = float3.zero;
			GetValidChaseEnemyPosition(ref movement, in config, in transform, out hasTarget, out targetPosition);
			if (hasTarget)
			{
				float3 end = DTool.IgnoreZDir(in targetPosition, in transform.Position);
				movement.Direction = math.lerp(movement.Direction, end, 5f * DeltaTime);
				velocity.Linear = math.lerp(velocity.Linear, movement.Direction * movement.Speed, 5f * DeltaTime);
				return;
			}
		}
		velocity.Linear = movement.Direction * movement.Speed;
	}

	private void JudgementBlade_ChaseMouse(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref LocalTransform transform, ref SpellConfigComponentData config, Entity spell)
	{
		ref Spell1023JudgementBladeData valueRW = ref JudgementBladeDataLookUp.GetRefRW(spell).ValueRW;
		if (valueRW.State != JudgementBladeState.AfterShoot)
		{
			return;
		}
		if (movement.IsFallSpell)
		{
			if (transform.Position.z <= -1f && !movement.IsFallRebounded)
			{
				float3 @float = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
			}
			else if (movement.IsFallRebounded)
			{
				if (!valueRW.IsFirstOnGroundFrame)
				{
					valueRW.IsFirstOnGroundFrame = true;
					velocity.Linear = math.normalize(movement.Direction) * movement.Speed;
				}
				else
				{
					Normal_Mouse(ref transform, ref movement, ref velocity);
				}
			}
			else
			{
				velocity.Linear = float3.zero;
			}
		}
		else
		{
			float3 float2 = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
		}
	}

	private void JudgementBlade_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spell)
	{
		if (JudgementBladeDataLookUp[spell].State == JudgementBladeState.AfterShoot)
		{
			float3 @float = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
		}
	}

	private void BlueRune_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, LocalTransform transform, in SpellComponentData spellData, Entity spell)
	{
		if (!BlueRuneDataLookUp.HasComponent(spell))
		{
			return;
		}
		RefRW<Spell4027BlueRuneData> refRW = BlueRuneDataLookUp.GetRefRW(spell);
		if (config.DurationTimer >= refRW.ValueRW.NormalIgnoreChaseDuration)
		{
			int index = movement.ChaseTarget.Index;
			GetValidChaseEnemyPosition(ref movement, in config, in transform, out var hasTarget, out var targetPosition);
			if (movement.ChaseTarget.Index != index)
			{
				float num = 3f;
				refRW.ValueRW.NormalChasePower = 1f / num;
				refRW.ValueRW.DisableColliderTimer = refRW.ValueRW.NormalIgnoreChaseDuration / num;
			}
			if (hasTarget)
			{
				float num2 = (movement.IsFallSpell ? movement.OriginalSpellHorizontalSpeed : movement.Speed);
				float num3 = (refRW.ValueRO.IsSuperBlueRune ? 1.5f : 1f);
				float3 source = movement.Direction;
				float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
				float3 x = DTool.DirMoveTowardsIgnoreZ(in source, in target, num2 * 50f * DeltaTime * refRW.ValueRO.NormalChasePower);
				movement.Direction = math.normalize(x);
				velocity.Linear = movement.Direction * num2 * num3;
			}
		}
		else if (movement.Speed <= 0f)
		{
			velocity.Linear = 0;
		}
		else
		{
			float3 end = DTool.IgnoreZDir(in refRW.ValueRO.NoTargetPosition, in transform.Position) * movement.Speed * (refRW.ValueRO.IsSuperBlueRune ? 2f : 1f);
			float num4 = (refRW.ValueRO.IsSuperBlueRune ? 24f : 18f);
			velocity.Linear = math.lerp(velocity.Linear, end, math.min(movement.Speed * DeltaTime * num4 * refRW.ValueRO.NormalChasePower, 1f));
			movement.Direction = math.normalize(velocity.Linear);
		}
	}

	private void Normal_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config)
	{
		float3 @float = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
	}

	private void Boomerang_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, ref LocalTransform transform, ref Spell1022BoomerangData boomerang, in SpellComponentData data)
	{
		float3 to = ((!TransformLookup.HasComponent(data.OwnerEntity)) ? (transform.Position + movement.Direction) : TransformLookup[data.OwnerEntity].Position);
		boomerang.extraLerpSpeed += 5f * DeltaTime;
		float t = (movement.Speed + boomerang.extraLerpSpeed) * DeltaTime * 0.05f;
		float3 @float = math.lerp(movement.Direction, DTool.IgnoreZDir(in to, in transform.Position), t);
		velocity.Linear = @float * movement.Speed;
		movement.Direction = @float;
	}

	private void GhostFire_Common(ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, Entity spellEntity)
	{
		float3 linear = math.normalize(movement.Direction) * movement.Speed;
		linear += GhostFireDataLookUp[spellEntity].PullForceByOtherGhostFire;
		velocity.Linear = linear;
	}

	private void GetValidChasePosition(ref SpellMovementComponentData movement, in SpellComponentData data, in SpellConfigComponentData config, LocalTransform transform, out bool hasTarget, out float3 targetPosition)
	{
		switch (movement.Type)
		{
		case SpellSpecialMovementType.ChaseOwner:
			hasTarget = true;
			targetPosition = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			GetValidChaseEnemyPosition(ref movement, in config, in transform, out hasTarget, out targetPosition);
			break;
		default:
			throw new Exception("法术弹道不是跟踪敌人或者跟踪施法者，不应该调用 GetValidChaseTarget");
		}
	}

	private void GetValidChaseEnemyPosition(ref SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, out bool hasTarget, out float3 targetPosition)
	{
		hasTarget = false;
		targetPosition = float3.zero;
		Entity target;
		float3 targetPosition2;
		UnitProperty_Dots targetPpt;
		if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData) && UnitPropertyLookUp.TryGetComponent(movement.ChaseTarget, out var componentData2) && componentData2.CanBeTarget)
		{
			targetPosition = componentData.Position;
			hasTarget = true;
		}
		else if (CurrentRoomEntities.FindMinAngleTarget(transform.Position, movement.Direction, config.ShooterType, out target, out targetPosition2, out targetPpt))
		{
			hasTarget = true;
			targetPosition = targetPosition2;
			movement.ChaseTarget = target;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, i);
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
				Entity spellEtt = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, i);
				Execute(chunkIndexInQuery, ref movement, ref velocity, ref transform, ref config, ref data, spellEtt);
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
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, nextRangeBegin);
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
					Entity spellEtt2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, nextRangeBegin);
					Execute(chunkIndexInQuery, ref movement2, ref velocity2, ref transform2, ref config2, ref data2, spellEtt2);
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
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, j);
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
				Entity spellEtt3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, j);
				Execute(chunkIndexInQuery, ref movement3, ref velocity3, ref transform3, ref config3, ref data3, spellEtt3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, k);
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
				Entity spellEtt4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, k);
				Execute(chunkIndexInQuery, ref movement4, ref velocity4, ref transform4, ref config4, ref data4, spellEtt4);
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
