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

[WithNone(new Type[] { typeof(Spell2007SuicideBugInitializeTag) })]
[BurstCompile]
[CompilerGenerated]
public struct Spell2007WormJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2007SuicideBugData> __Spell2007SuicideBugData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<EffectsCollectorData> __EffectsCollectorData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2007SuicideBugData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2007SuicideBugData>();
				__EffectsCollectorData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EffectsCollectorData>(isReadOnly: true);
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell2007SuicideBugData_RW_ComponentTypeHandle.Update(ref state);
				__EffectsCollectorData_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EffectsCollectorData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2007SuicideBugData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell2007SuicideBugInitializeTag>();
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
		public void Run(ref Spell2007WormJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2007WormJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2007WormJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2007WormJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2007WormJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2007WormJob job, EntityManager entityManager)
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

	public float3 MousePosition;

	public GlobalRandom Random;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public Entity SEPlayerEntity;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<PostTransformMatrix> MatrixLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public Entity spell3118SelfSacrificeSpawnBufferEntity;

	public Entity GlobalParticleEntity;

	public Entity spell3101Buffer;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Explosion(int chunkIndex, in Spell2007SuicideBugData data, in SpellConfigComponentData config, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellElementEffectComponentData element, in LocalTransform transform, Entity entity)
	{
		config.ColorType.ColorEnumToString(out var result);
		CMD.AppendToBuffer(chunkIndex, GlobalParticleEntity, new GlobalParticleEmitParams
		{
			Size = config.Radius.Calculate(),
			Position = transform.Position,
			Type = GlobalParticleType.Spell,
			Name = $"2007_BugExplosion_{result}"
		});
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref readonly float3 position = ref transform.Position;
		float radius = config.Radius.Calculate();
		UnitType selfCamp = UnitType.Player;
		SpellTools.GetAttackableEntitiesInRange(in position, in radius, in selfCamp, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in element, in componentData, out var info);
		foreach (Entity item in entities)
		{
			Entity target = item;
			CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref Spell2007SuicideBugData data, in EffectsCollectorData collector, ref SpellMovementComponentData movement, in SpellConfigComponentData config, in SpellComponentData componentData, in SpellElementEffectComponentData element, ref LocalTransform trans, Entity entity)
	{
		ref PostTransformMatrix valueRW = ref MatrixLookup.GetRefRW(collector.Effect1).ValueRW;
		data.LifeTimer += DeltaTime;
		if (data.LifeTimer >= data.LifeTime && data.State != Spell2007SuicideBugData.WormState.ReadyToExplode)
		{
			data.State = Spell2007SuicideBugData.WormState.ReadyToExplode;
		}
		data.CheckTargetTimer += DeltaTime;
		if (data.CheckTargetTimer >= 0.08f)
		{
			data.CheckTargetTimer = 0f;
			RefindTarget(ref movement, trans);
		}
		UpdateStateAndBodyHeight(index, ref data, in collector, in config, in componentData, in element, ref movement, in trans, entity);
		Moving(ref data, ref movement, ref trans, ref valueRW);
	}

	[BurstCompile]
	private void UpdateStateAndBodyHeight(int index, ref Spell2007SuicideBugData data, in EffectsCollectorData collector, in SpellConfigComponentData config, in SpellComponentData componentData, in SpellElementEffectComponentData element, ref SpellMovementComponentData movement, in LocalTransform trans, Entity entity)
	{
		ref LocalTransform valueRW = ref TransformLookup.GetRefRW(collector.Effect3).ValueRW;
		float num = data.LifeTimer * 2f % 1f;
		float num2 = 0f;
		if (num <= 0.5f)
		{
			float2 p = new float2(0.25f, 0.2f);
			float2 p2 = new float2(0.75f, 0.9f);
			num2 = DTool.GetCurveY(in p, in p2, num * 2f);
		}
		else
		{
			float2 p = new float2(0.25f, 0.2f);
			float2 p2 = new float2(0.75f, 0.9f);
			num2 = DTool.GetCurveY(in p, in p2, (1f - num) * 2f);
		}
		num2 *= 0.3f;
		switch (data.State)
		{
		case Spell2007SuicideBugData.WormState.Initial:
			valueRW.Position.y = DTool.Lerp(valueRW.Position.y, num2, DeltaTime * 10f);
			data.StateTimer += DeltaTime;
			if (data.StateTimer >= 0.5f)
			{
				data.State = Spell2007SuicideBugData.WormState.Idle;
				data.StateTimer = 0f;
			}
			break;
		case Spell2007SuicideBugData.WormState.Idle:
			valueRW.Position.y = num2;
			data.StateTimer += DeltaTime;
			if (data.StateTimer >= data.IdleTime)
			{
				data.StateTimer = 0f;
				if (RefindTarget(ref movement, trans))
				{
					data.State = Spell2007SuicideBugData.WormState.ChaseToTarget;
				}
			}
			break;
		case Spell2007SuicideBugData.WormState.ChaseToTarget:
		{
			valueRW.Position.y = num2;
			if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData2) && ReadyToExplosion(in config, movement.Type == SpellSpecialMovementType.Rotation, in trans, (movement.Type == SpellSpecialMovementType.ChaseMouse) ? MousePosition : componentData2.Position))
			{
				movement.CurrentFallSpeed = Random.random.NextFloat(0.3f, 0.4f);
				data.State = Spell2007SuicideBugData.WormState.ReadyToExplode;
			}
			else if (!RefindTarget(ref movement, trans))
			{
				data.IdleTime = 0.5f;
				data.State = Spell2007SuicideBugData.WormState.Idle;
			}
			break;
		}
		case Spell2007SuicideBugData.WormState.ReadyToExplode:
			movement.CurrentFallSpeed -= 2f * DeltaTime;
			valueRW.Position.y = math.max(valueRW.Position.y + movement.CurrentFallSpeed, -1.52f);
			if (!(valueRW.Position.y < -1.51f))
			{
				break;
			}
			CMD.AppendToBuffer(index, SEPlayerEntity, new SEData("SE_Teammate7_Explosion"));
			if (movement.ReboundCount > 0)
			{
				movement.CurrentFallSpeed = 0.5f;
				valueRW.Position.y = -1.5f;
				movement.ReboundCount--;
				Explosion(index, in data, in config, in componentData, in movement, in element, in trans, entity);
				if (config.ColorType == SpellColorType.Thunder)
				{
					CMD.CheckFallThunderDamage(index, spell3101Buffer, trans.Position, UnitPropertyLookup, PhysicsWorld, in config, in movement, in trans, in element, in componentData, entity);
				}
				break;
			}
			data.State = Spell2007SuicideBugData.WormState.Suicide;
			Explosion(index, in data, in config, in componentData, in movement, in element, in trans, entity);
			if (data.ExplodeRadius > 0f)
			{
				SpellConfigComponentData config2 = SpellConfigLookup.GetRefRO(entity).ValueRO;
				config2.Damage.Base = data.BugInheritHp * data.BugSacrificeExplosionDamageRatio;
				SpellMovementComponentData movement2 = default(SpellMovementComponentData);
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config2, in movement2, in trans, in element, in componentData, out var info);
				info.damageRecordId = 3118;
				NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
				ref readonly float3 position = ref trans.Position;
				float radius = SpellConfigLookup.GetRefRO(entity).ValueRO.Radius.CalculateWithNewBaseValue(data.ExplodeRadius);
				UnitType selfCamp = UnitType.Teammate;
				SpellTools.GetAttackableEntitiesInRange(in position, in radius, in selfCamp, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
				foreach (Entity item in entities)
				{
					Entity target = item;
					CMD.TryAttackEntity(index, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
				}
				SpellTools.GetTeammateSacrificeRange(data.ExplodeRadius, in config, out var result);
				CMD.AppendToBuffer(index, spell3118SelfSacrificeSpawnBufferEntity, new Spell3118SelfSacrificeSpawnBuffer
				{
					spawnPosition = trans.Position,
					spellColorType = SpellConfigLookup.GetRefRO(entity).ValueRO.ColorType,
					ExplosionRange = result
				});
			}
			data.State = Spell2007SuicideBugData.WormState.Die;
			break;
		}
	}

	[BurstCompile]
	private bool RefindTarget(ref SpellMovementComponentData movement, LocalTransform trans)
	{
		float3 targetPosition;
		UnitProperty_Dots targetPpt;
		return CurrentRoomEntities.FindNearestTarget(trans.Position, UnitType.Player, out movement.ChaseTarget, out targetPosition, out targetPpt);
	}

	[BurstCompile]
	private bool ReadyToExplosion(in SpellConfigComponentData config, bool isRotation, in LocalTransform transform, float3 targetPos)
	{
		float num = config.Radius.Calculate() + (isRotation ? 0f : 0.5f);
		return DTool.IgnoreZDistanceSqr(in transform.Position, in targetPos) < num * num * 0.8f;
	}

	[BurstCompile]
	private void Moving(ref Spell2007SuicideBugData data, ref SpellMovementComponentData movement, ref LocalTransform trans, ref PostTransformMatrix matrix)
	{
		DeltaTime = Mathf.Clamp(DeltaTime, 1E-06f, 0.1f);
		float num = movement.Speed + data.PushPower;
		data.PushPower = math.lerp(data.PushPower, 0f, math.clamp(50f * DeltaTime, 0f, 1f));
		float3 @float = Unity.Mathematics.float3.zero;
		switch (movement.Type)
		{
		case SpellSpecialMovementType.Normal:
		{
			if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData))
			{
				@float = DTool.IgnoreZDir(in componentData.Position, in trans.Position);
			}
			ref float3 velocity = ref data.Velocity;
			float3 oldDir = @float * num;
			data.Velocity = DTool.Lerp(in velocity, in oldDir, 5f * DeltaTime);
			break;
		}
		case SpellSpecialMovementType.ChaseEnemy:
		{
			if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData2))
			{
				@float = DTool.IgnoreZDir(in componentData2.Position, in trans.Position);
				float2 source = math.normalizesafe(data.Velocity).xy;
				float2 target = @float.xy;
				float2 float3 = DTool.DirMoveTowards(in source, in target, num * 15f * DeltaTime);
				data.Velocity = new float3(float3.x, float3.y, 0f) * num;
			}
			else
			{
				CurrentRoomEntities.FindMinAngleTarget(trans.Position, math.normalizesafe(data.Velocity), UnitType.Player, out movement.ChaseTarget, out var _, out var _);
				data.Velocity = DTool.Lerp(in data.Velocity, in Unity.Mathematics.float3.zero, 5f * DeltaTime);
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			@float = DTool.IgnoreZDir(in MousePosition, in trans.Position);
			ref float3 velocity2 = ref data.Velocity;
			float3 oldDir = @float * num;
			data.Velocity = DTool.Lerp(in velocity2, in oldDir, 2f * DeltaTime);
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			@float = DTool.IgnoreZDir(in movement.ChaseOwnerPosition, in trans.Position);
			float3 oldDir = math.normalizesafe(data.Velocity);
			@float = DTool.DirMoveTowardsIgnoreZ(in oldDir, in @float, num * movement.ChaseRotateSpeed * DeltaTime);
			data.Velocity = @float * num;
			break;
		}
		case SpellSpecialMovementType.Rotation:
			if (data.State != Spell2007SuicideBugData.WormState.ReadyToExplode)
			{
				float num2 = 360f / (MathF.PI * 2f * movement.AroundAngle / num) * DeltaTime;
				movement.AroundAngle += num2 * 57.29578f;
				float3 chaseOwnerPosition = movement.ChaseOwnerPosition;
				float3 oldDir = new float3(0f, 1f, 0f);
				float3 float2 = chaseOwnerPosition + DTool.GetDir(in oldDir, movement.AroundAngle) * data.CurrentAroundRadius;
				matrix.Value = Matrix4x4.Scale((Vector3)new float3((!(float2.x < trans.Position.x)) ? 1 : (-1), 1f, 1f));
				trans.Position.xy = float2.xy;
				if (data.CurrentAroundRadius < movement.AroundRadius)
				{
					data.CurrentAroundRadius += (num + data.PushPower) * 0.7f * DeltaTime;
					data.CurrentAroundRadius = math.clamp(data.CurrentAroundRadius, 0f, movement.AroundRadius);
				}
			}
			break;
		}
		if (movement.Type != SpellSpecialMovementType.Rotation)
		{
			trans.Position += data.Velocity * DeltaTime;
			matrix.Value = Matrix4x4.Scale((Vector3)new float3((!(data.Velocity.x < 0f)) ? 1 : (-1), 1f, 1f));
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2007SuicideBugData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__EffectsCollectorData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2007SuicideBugData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugData>(nativeArrayPtr, i);
				ref EffectsCollectorData collector = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData element = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				ref LocalTransform trans = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				Execute(chunkIndexInQuery, ref data, in collector, ref movement, in config, in componentData, in element, ref trans, entity);
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
					ref Spell2007SuicideBugData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugData>(nativeArrayPtr, nextRangeBegin);
					ref EffectsCollectorData collector2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData element2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref LocalTransform trans2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					Execute(chunkIndexInQuery, ref data2, in collector2, ref movement2, in config2, in componentData2, in element2, ref trans2, entity2);
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
				ref Spell2007SuicideBugData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugData>(nativeArrayPtr, j);
				ref EffectsCollectorData collector3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData element3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				ref LocalTransform trans3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				Execute(chunkIndexInQuery, ref data3, in collector3, ref movement3, in config3, in componentData3, in element3, ref trans3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2007SuicideBugData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugData>(nativeArrayPtr, k);
				ref EffectsCollectorData collector4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData element4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				ref LocalTransform trans4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				Execute(chunkIndexInQuery, ref data4, in collector4, ref movement4, in config4, in componentData4, in element4, ref trans4, entity4);
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
