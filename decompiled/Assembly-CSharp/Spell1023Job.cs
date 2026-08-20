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

[BurstCompile]
[CompilerGenerated]
public struct Spell1023Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public ComponentTypeHandle<Spell1023JudgementBladeData> __Spell1023JudgementBladeData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__Spell1023JudgementBladeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1023JudgementBladeData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__Spell1023JudgementBladeData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1023JudgementBladeData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
		public void Run(ref Spell1023Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1023Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1023Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1023Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1023Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1023Job job, EntityManager entityManager)
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

	public float CurrentTime;

	public Unity.Mathematics.Random Random;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity EffectRequireEntity;

	[ReadOnly]
	public SpellSingleton SpellSingleton;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell1023SpellMaterialProperty> SpellMaterialLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell1023ShadowMaterialOverride> ShadowMaterialLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<PhysicsCollider> ColliderLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public Spell1023AroundDataSingleton Spell1023OwnerSingleton;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectCollectorLookup;

	public Spell1023ExtraData ExtraData;

	public float3 MousePosition;

	public Entity SEPlayerSingleton;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute([ChunkIndexInQuery] int chunkIndex, SpellAspect spell, ref Spell1023JudgementBladeData data, ref PhysicsVelocity velocity)
	{
		if (!data.IsInitialized)
		{
			data.IsInitialized = true;
			data.EnemyDetectRange = spell.Config.ValueRO.Float1 * (spell.Config.ValueRW.Radius.AddRatio + 1f) * spell.Config.ValueRW.Radius.MulRatio;
			PhysicsCollider collider = ColliderLookUp[spell.Entity];
			SpellTools.DisableSpellTrigger(in collider);
			collider = ColliderLookUp[spell.Entity];
			SpellTools.DisableSpellReboundCollider(in collider);
			velocity.Linear = float3.zero;
			data.OwnerLastFramePosition = (LocalTransformLookUp.HasComponent(spell.Data.ValueRO.Shooter) ? LocalTransformLookUp[spell.Data.ValueRO.Shooter].Position : spell.Transform.ValueRO.Position);
			if (spell.Movement.ValueRO.IsFallSpell)
			{
				spell.Movement.ValueRW.CurrentFallSpeed = 0f;
				ShadowMaterialLookup.GetRefRW(EffectCollectorLookup[spell.Entity].Effect2).ValueRW.Value = 0f;
			}
			EnterState(JudgementBladeState.Spawn, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			if (!spell.Movement.ValueRO.IsFallSpell)
			{
				CurrentRoomEntities.FindNearestTarget(spell.Transform.ValueRW.Position, UnitType.Player, out var target, out var targetPosition, out var targetPpt);
				if (target != Entity.Null && DTool.IgnoreZDistance(in spell.Transform.ValueRW.Position, in targetPosition) > data.EnemyDetectRange + targetPpt.size / 2f)
				{
					target = Entity.Null;
				}
				spell.Transform.ValueRW.Position.z = -0.65f;
				if (spell.Movement.ValueRO.Type == SpellSpecialMovementType.ChaseMouse)
				{
					data.TargetLastFramePosition = MousePosition;
					EnterState(JudgementBladeState.LockingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
				}
				else if (target != Entity.Null)
				{
					data.Target = target;
					data.TargetLastFramePosition = LocalTransformLookUp[data.Target].Position;
					EnterState(JudgementBladeState.LockingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
				}
				else if (data.State == JudgementBladeState.Spawn)
				{
					EnterState(JudgementBladeState.DetectingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
				}
			}
		}
		if (data.FadeInTimer <= 0.18f)
		{
			data.FadeInTimer += DeltaTime;
			if (spell.Data.ValueRO.IsSplitSpell && spell.Movement.ValueRO.IsFallSpell && spell.Movement.ValueRO.IsFallRebounded)
			{
				spell.Movement.ValueRW.IsFallRebounded = false;
				spell.Transform.ValueRW.Position.z = -7f;
			}
			else
			{
				SetBladeMaterialFadeProgress(EffectCollectorLookup[spell.Data.ValueRO.SpellEffectEntity].Effect1, math.min(data.FadeInTimer / 0.18f, 1f));
			}
		}
		else if (data.State == JudgementBladeState.Spawn)
		{
			if (spell.Movement.ValueRO.Type == SpellSpecialMovementType.ChaseMouse)
			{
				data.TargetLastFramePosition = MousePosition;
				EnterState(JudgementBladeState.LockingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
			else
			{
				EnterState(JudgementBladeState.DetectingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
		}
		switch (data.State)
		{
		case JudgementBladeState.Spawn:
			RotateBladeLookDirection(spell, new float3(0f, -1f, 0f));
			break;
		case JudgementBladeState.DetectingTarget:
		{
			if (data.IsBladeInQuery)
			{
				BladeRotateAroundOwnerEntity(spell, ref data);
			}
			else
			{
				BladeIdleFloat(spell);
			}
			SpellAspect spell2 = spell;
			SpellMovementComponentData valueRO = spell.Movement.ValueRO;
			RotateBladeLookDirection(spell2, (valueRO.Type == SpellSpecialMovementType.Rotation && !valueRO.IsFallSpell) ? spell.Movement.ValueRO.Direction : new float3(0f, -1f, 0f));
			if (!data.IsBladeInQuery && data.Target == Entity.Null)
			{
				data.BladeRecheckTargetTimer += DeltaTime;
				if (data.BladeRecheckTargetTimer >= 0.1f)
				{
					data.BladeRecheckTargetTimer -= 0.1f;
					CurrentRoomEntities.FindNearestTarget(spell.Transform.ValueRW.Position, UnitType.Player, out var target2, out var targetPosition2, out var targetPpt2);
					if (target2 != Entity.Null && DTool.IgnoreZDistance(in spell.Transform.ValueRW.Position, in targetPosition2) > data.EnemyDetectRange + targetPpt2.size / 2f)
					{
						target2 = Entity.Null;
					}
					data.Target = target2;
				}
			}
			if (data.Target != Entity.Null)
			{
				EnterState(JudgementBladeState.LockingTarget, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
			if (spell.Movement.ValueRO.IsFallSpell && spell.Config.ValueRO.DurationTimer >= spell.Config.ValueRW.Duration.Calculate())
			{
				EnterState(JudgementBladeState.DestroyRiseUP, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
			break;
		}
		case JudgementBladeState.LockingTarget:
		{
			float t = math.min(1f, data.LockingTargetTimer / 0.18f);
			if (!spell.Movement.ValueRO.IsFallSpell)
			{
				spell.Transform.ValueRW.Position.z = math.lerp(spell.Transform.ValueRW.Position.z, -0.3f, t);
			}
			if (data.Target != Entity.Null && UnitPropertyLookup.HasComponent(data.Target) && UnitPropertyLookup[data.Target].CanBeTarget)
			{
				data.TargetLastFramePosition = LocalTransformLookUp[data.Target].Position;
			}
			else if (spell.Movement.ValueRO.Type == SpellSpecialMovementType.ChaseMouse)
			{
				data.TargetLastFramePosition = MousePosition;
			}
			LockingTargetRotateBlade(ref data, spell);
			RotateShadowLookDirection(spell, spell.Movement.ValueRO.Direction);
			SetShadowMaterialFadeProgress(EffectCollectorLookup[spell.Entity].Effect2, math.clamp(data.LockingTargetTimer / 0.18f, 0f, 1f));
			if (data.LockingTargetTimer >= 0.18f)
			{
				spell.Movement.ValueRW.CurrentFallSpeed = spell.Movement.ValueRO.OriginalSpellHorizontalSpeed;
				EnterState(JudgementBladeState.AfterShoot, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
			data.LockingTargetTimer += DeltaTime;
			break;
		}
		case JudgementBladeState.AfterShoot:
			if (spell.Movement.ValueRO.IsFallSpell)
			{
				float3 rootPosition = spell.Movement.ValueRO.Speed * spell.Movement.ValueRO.Direction + new float3(0f, 0f, spell.Movement.ValueRO.CurrentFallSpeed);
				float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				rootPosition += layerPosition;
				RotateBladeLookDirection(spell, rootPosition);
			}
			else
			{
				RotateBladeLookDirection(spell, spell.Movement.ValueRO.Direction);
			}
			RotateShadowLookDirection(spell, spell.Movement.ValueRO.Direction);
			break;
		case JudgementBladeState.Hover:
			spell.Config.ValueRW.HoverTimer += DeltaTime;
			if (spell.Config.ValueRW.HoverTimer >= spell.Config.ValueRW.HoverDuration)
			{
				EnterState(JudgementBladeState.Destroy, chunkIndex, spell.Config.ValueRW, spell.Movement.ValueRW, spell.Transform.ValueRW, spell.Entity, ref data, in spell.Data.ValueRO);
			}
			break;
		case JudgementBladeState.Destroy:
		case JudgementBladeState.DestroyRiseUP:
			break;
		}
	}

	private void LockingTargetRotateBlade(ref Spell1023JudgementBladeData data, SpellAspect spell)
	{
		float3 direction = data.TargetLastFramePosition - spell.Transform.ValueRO.Position;
		float3 direction2 = data.LockTargetLookingDirection;
		float clockwiseAngleBetweenDirection = DTool.GetClockwiseAngleBetweenDirection(in direction2, in direction);
		clockwiseAngleBetweenDirection = math.degrees(clockwiseAngleBetweenDirection);
		if (!data.LockRotateInClockWise)
		{
			clockwiseAngleBetweenDirection = 360f - clockwiseAngleBetweenDirection;
		}
		float num = 0.25f;
		if (clockwiseAngleBetweenDirection < 60f)
		{
			num *= math.min(1f, 1f + 3f * (1f - clockwiseAngleBetweenDirection / 60f));
		}
		float num2 = math.lerp(0f, clockwiseAngleBetweenDirection, num);
		if (data.LockRotateInClockWise)
		{
			num2 *= -1f;
		}
		data.LockTargetLookingDirection = DTool.GetDir(in data.LockTargetLookingDirection, num2);
		float3 @float = DTool.IgnoreZDir(in data.TargetLastFramePosition, in spell.Transform.ValueRO.Position);
		spell.Movement.ValueRW.Direction = @float;
		if (!spell.Movement.ValueRO.IsFallSpell)
		{
			RotateBladeLookDirection(spell, data.LockTargetLookingDirection);
			return;
		}
		float2 xy = data.TargetLastFramePosition.xy;
		float2 xy2 = spell.Transform.ValueRO.Position.xy;
		float y = math.distance(xy, xy2);
		y = math.max(0.1f, y);
		spell.Movement.ValueRW.Speed = y / 7f * spell.Movement.ValueRO.OriginalSpellHorizontalSpeed;
		@float = spell.Movement.ValueRW.Speed * @float + new float3(0f, 0f, spell.Movement.ValueRO.OriginalSpellHorizontalSpeed);
		float3 layerPosition = DTool.GetLayerPosition(in @float, LayerCorrectType.Coordinate);
		@float += layerPosition;
		RotateBladeLookDirection(spell, @float);
	}

	private void BladeIdleFloat(SpellAspect spell)
	{
		if (spell.Movement.ValueRO.IsFallSpell)
		{
			spell.Transform.ValueRW.Position.z = -7f + math.sin(spell.Config.ValueRO.DurationTimer * 2f) * 0.2f;
		}
		else
		{
			spell.Transform.ValueRW.Position.z = -0.65f + math.sin(spell.Config.ValueRO.DurationTimer * 2f) * 0.2f;
		}
	}

	private void BladeRotateAroundOwnerEntity(SpellAspect spell, ref Spell1023JudgementBladeData data)
	{
		if (Spell1023OwnerSingleton.Data.TryGetValue(spell.Data.ValueRO.Shooter, out var item) && item.IsCreated)
		{
			if (LocalTransformLookUp.HasComponent(spell.Data.ValueRO.Shooter))
			{
				data.OwnerLastFramePosition = LocalTransformLookUp[spell.Data.ValueRO.Shooter].Position;
			}
			float3 shiftedDir = DTool.GetShiftedDir(CurrentTime * -180f + 360f / (float)item.Length * (float)item.IndexOf(spell.Entity));
			float num = 1.5f;
			if (item.Length > 40)
			{
				num += math.min(3f, (float)(item.Length - 40) * 0.005f);
			}
			float3 position = math.lerp(spell.Transform.ValueRO.Position, data.OwnerLastFramePosition + shiftedDir * num, 6f * DeltaTime);
			position.z = -0.65f;
			LocalTransformLookUp.GetRefRW(spell.Entity).ValueRW.Position = position;
		}
	}

	private void RotateBladeLookDirection(SpellAspect spell, float3 direction)
	{
		if (spell.Data.ValueRO.SpellEffectEntity != default(Entity))
		{
			direction.z = 0f;
			float z = math.atan2(direction.y, direction.x);
			LocalTransformLookUp.GetRefRW(spell.Data.ValueRO.SpellEffectEntity).ValueRW.Rotation = quaternion.Euler(0f, 0f, z);
		}
	}

	private void RotateShadowLookDirection(SpellAspect spell, float3 direction)
	{
		direction.z = 0f;
		float z = math.atan2(direction.y, direction.x);
		LocalTransformLookUp.GetRefRW(EffectCollectorLookup[spell.Entity].Effect2).ValueRW.Rotation = quaternion.Euler(0f, 0f, z);
	}

	private void EnterState(JudgementBladeState state, int chunkIndex, SpellConfigComponentData config, SpellMovementComponentData movement, LocalTransform trans, Entity spellEntity, ref Spell1023JudgementBladeData data, in SpellComponentData spellData)
	{
		data.State = state;
		switch (state)
		{
		case JudgementBladeState.Spawn:
			data.LockingTargetTimer += Random.NextFloat(-0.05f, 0.05f);
			break;
		case JudgementBladeState.LockingTarget:
		{
			ResetBladeRemainDuration(config);
			movement.Direction = new float3(0f, -1f, 0f);
			data.LockRotateInClockWise = data.TargetLastFramePosition.x >= trans.Position.x;
			float degree = DTool.GetDegree(movement.Direction);
			float degree2 = DTool.GetDegree(DTool.IgnoreZDir(in data.TargetLastFramePosition, in trans.Position));
			if (data.LockRotateInClockWise)
			{
				(float, float) tuple = DTool.MoveTowardsAngleClockWiseReTurn2Angle(degree, degree2, 60f * DeltaTime);
				data.BladeLockRotateLerpSpeed = (tuple.Item1 + tuple.Item2) / 10f;
			}
			else
			{
				(float, float) tuple2 = DTool.MoveTowardsAngleCounterClockWiseReTurn2Angle(degree, degree2, 60f * DeltaTime);
				data.BladeLockRotateLerpSpeed = (tuple2.Item1 + (360f - tuple2.Item2)) / 10f;
			}
			break;
		}
		case JudgementBladeState.AfterShoot:
		{
			if (!movement.IsFallSpell)
			{
				PhysicsCollider collider = ColliderLookUp[spellEntity];
				SpellTools.EnableSpellTrigger(in collider);
				collider = ColliderLookUp[spellEntity];
				SpellTools.EnableSpellReboundCollider(in collider);
			}
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			FixedString32Bytes seName = "StartShoot";
			cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1023, in seName)));
			float3 rotation = movement.Direction;
			if (movement.IsFallSpell)
			{
				rotation = movement.Speed * rotation + new float3(0f, 0f, movement.OriginalSpellHorizontalSpeed);
				float3 layerPosition = DTool.GetLayerPosition(in rotation, LayerCorrectType.Coordinate);
				rotation += layerPosition;
			}
			ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
			ref SpellSingleton spellSingleton = ref SpellSingleton;
			LocalTransform localTransform = LocalTransformLookUp[spellEntity];
			ref float3 position = ref localTransform.Position;
			seName = "Shoot";
			cMD2.CreateSpellEffect(chunkIndex, in spellSingleton, in spellData, in config, in position, in seName, 1f, in rotation);
			if (!ExtraData.IsMobile && SpellSingleton.Effects.TryGetValue(1023, out var item))
			{
				config.ColorType.ColorEnumToString(out var result);
				CMD.AppendToBuffer(chunkIndex, EffectRequireEntity, new SpellEffectSystem.Require
				{
					Settings = item["Trail"],
					Entity = spellEntity,
					Color = result,
					SpellId = 1023
				});
			}
			break;
		}
		case JudgementBladeState.Destroy:
		case JudgementBladeState.DestroyRiseUP:
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, spellEntity, value: true);
			break;
		case JudgementBladeState.DetectingTarget:
		case JudgementBladeState.Hover:
			break;
		}
	}

	private void ResetBladeRemainDuration(SpellConfigComponentData config)
	{
		config.DurationTimer = config.Duration.Calculate() - 0.18f - 2f;
		if (config.DurationTimer < 0f)
		{
			config.Duration.Extra -= config.DurationTimer;
			config.DurationTimer = 0f;
		}
	}

	private void SetBladeMaterialFadeProgress(Entity spellEffectEntity, float grogress)
	{
		SpellMaterialLookup.GetRefRW(spellEffectEntity).ValueRW.Value = grogress;
	}

	private void SetShadowMaterialFadeProgress(Entity spellEffectEntity, float grogress)
	{
		ShadowMaterialLookup.GetRefRW(spellEffectEntity).ValueRW.Value = grogress;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1023JudgementBladeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spell = resolvedChunk[i];
				Execute(chunkIndexInQuery, spell, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1023JudgementBladeData>(nativeArrayPtr, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, i));
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
					SpellAspect spell2 = resolvedChunk[nextRangeBegin];
					Execute(chunkIndexInQuery, spell2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1023JudgementBladeData>(nativeArrayPtr, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, nextRangeBegin));
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
				SpellAspect spell3 = resolvedChunk[j];
				Execute(chunkIndexInQuery, spell3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1023JudgementBladeData>(nativeArrayPtr, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				SpellAspect spell4 = resolvedChunk[k];
				Execute(chunkIndexInQuery, spell4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1023JudgementBladeData>(nativeArrayPtr, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, k));
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
