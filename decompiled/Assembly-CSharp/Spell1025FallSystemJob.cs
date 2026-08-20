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
public struct Spell1025FallSystemJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellFallTag> __SpellFallTag_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1025DragonBreathData> __Spell1025DragonBreathData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public BufferTypeHandle<Spell1025FireLinePointsBuffer> __Spell1025FireLinePointsBuffer_RW_BufferTypeHandle;

			public BufferTypeHandle<Spell1025FireGroundEffectBuffer> __Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellFallTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellFallTag>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Spell1025DragonBreathData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025DragonBreathData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__Spell1025FireLinePointsBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell1025FireLinePointsBuffer>();
				__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell1025FireGroundEffectBuffer>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellFallTag_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell1025DragonBreathData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__Spell1025FireLinePointsBuffer_RW_BufferTypeHandle.Update(ref state);
				__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle.Update(ref state);
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
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellFallTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025DragonBreathData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025FireLinePointsBuffer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025FireGroundEffectBuffer>();
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
		public void Run(ref Spell1025FallSystemJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1025FallSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1025FallSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1025FallSystemJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1025FallSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1025FallSystemJob job, EntityManager entityManager)
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

	public float CreateInterval;

	public SpellSingleton SpellSingleton;

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsSingleton;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitsLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellRefractionData> RefractionLookUp;

	public float3 MousePosition;

	public float DeltaTime;

	public Unity.Mathematics.Random Random;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref SpellMovementComponentData movement, ref SpellComponentData data, ref LocalTransform transform, SpellFallTag _, ref SpellConfigComponentData config, ref Spell1025DragonBreathData spell, ref PhysicsVelocity phys, ref DynamicBuffer<Spell1025FireLinePointsBuffer> buffer, ref DynamicBuffer<Spell1025FireGroundEffectBuffer> groundEffectBuffer, Entity entity)
	{
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookUp);
		}
		GenerateFallingFireLine(transform, ref config, ref movement, in spell, in data, ref buffer, entity);
		ShootFireLineEntity(index, transform, entity, ref buffer, ref spell, data.IsSplitSpell, config.ColorType);
		UpdatePowerUpTimer(ref config, ref spell, DeltaTime);
	}

	[BurstCompile]
	private void ShootFireLineEntity([ChunkIndexInQuery] int index, LocalTransform transform, Entity ett, ref DynamicBuffer<Spell1025FireLinePointsBuffer> buffer, ref Spell1025DragonBreathData data, bool isSplit, SpellColorType color)
	{
		data.ShootTimer += DeltaTime;
		if (data.ShootTimer >= CreateInterval)
		{
			data.ShootTimer = 0f;
			float3 position = transform.Position + new float3(0f, 0f - transform.Position.z, (transform.Position.y + transform.Position.z) * 0.01f - transform.Position.z);
			color.ColorEnumToString(out var result);
			Entity entity = CMD.Instantiate(index, SpellSingleton.Prefabs[$"1025_FireLineEntity_{result}"]);
			CMD.AppendToBuffer(index, ett, new Spell1025DragonBreathFireLinePointBuffer
			{
				Entity = entity
			});
			CMD.SetComponent(index, entity, new LocalTransform
			{
				Position = position,
				Rotation = quaternion.Euler(0f, 0f, MathF.PI / 180f * Random.NextFloat(-60f, 60f)),
				Scale = 1f
			});
			CMD.SetComponent(index, entity, new Spell1025DragonBreathFireLinePointData
			{
				Parent = ett,
				offset = Random.NextFloat(-0.04f, 0.04f)
			});
		}
	}

	[BurstCompile]
	private void UpdatePowerUpTimer(ref SpellConfigComponentData config, ref Spell1025DragonBreathData spell, float deltaTime)
	{
		spell.powerUpStackDamageRatio += config.Float3 / 100f * deltaTime;
		config.Damage.AddRatio = spell.baseDamageAddRatio + spell.powerUpStackDamageRatio;
	}

	[BurstCompile]
	private float3 RefractionToNext(in LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref NativeHashSet<Entity> hitEntities, float3 findNearPos, ref Entity hitEntity)
	{
		Entity target;
		float3 targetPosition;
		UnitProperty_Dots targetPpt;
		bool flag = CurrentRoomEntities.FindReflectionTarget(findNearPos, config.ShooterType, in hitEntities, out target, out targetPosition, out targetPpt);
		if (!flag)
		{
			hitEntities.Clear();
			hitEntities.Add(hitEntity);
			flag = CurrentRoomEntities.FindReflectionTarget(findNearPos, config.ShooterType, in hitEntities, out target, out targetPosition, out targetPpt);
		}
		if (!flag)
		{
			return float3.zero;
		}
		hitEntity = target;
		hitEntities.Add(hitEntity);
		float3 @float = targetPosition;
		float3 input = math.normalizesafe(@float - findNearPos);
		movement.Direction = input.IgnoreZ();
		return @float;
	}

	[BurstCompile]
	private bool IsFloat3Zero(float3 pos)
	{
		if (pos.x == 0f && pos.y == 0f)
		{
			return pos.z == 0f;
		}
		return false;
	}

	[BurstCompile]
	private void GenerateFallingFireLine(LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, in Spell1025DragonBreathData spell, in SpellComponentData data, ref DynamicBuffer<Spell1025FireLinePointsBuffer> fireLinePointsBuffer, Entity entity)
	{
		NativeList<float3> pointsList = new NativeList<float3>(Allocator.Temp);
		float aroundAngle = movement.AroundAngle;
		float3 direction = movement.Direction;
		pointsList.Add(in transform.Position);
		int num = movement.ReboundCount;
		int num2 = 0;
		float3 hitPosition;
		if (data.IsSplitSpell)
		{
			AppendReboundingFireLinePoints(in data, in config, ref movement, ref pointsList, transform, in spell, pointsList[pointsList.Length - 1] + movement.Direction * spell.FallDamageRange, out hitPosition);
		}
		else
		{
			AppendFirstFireLinePoints(in data, ref movement, ref config, transform, ref pointsList, out hitPosition);
		}
		if (RefractionLookUp.TryGetComponent(entity, out var componentData))
		{
			num2 = componentData.RemainCount;
		}
		Entity hitEntity = Entity.Null;
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DTool.GetEnemyEntityInRange(in hitPosition, spell.FallDamageRange, config.ShooterType, containsBrittleness: true, in UnitsLookUp, in PhysicsSingleton, ref result);
		NativeHashSet<Entity> hitEntities = new NativeHashSet<Entity>(result.Length, Allocator.Temp);
		foreach (Entity item in result)
		{
			hitEntities.Add(item);
		}
		if (result.Length > 0)
		{
			hitEntity = result[0];
		}
		int num3 = num2 + num;
		for (int i = 0; i < num3; i++)
		{
			float3 @float = float3.zero;
			if (num2 > 0 && result.Length > 0)
			{
				@float = RefractionToNext(in transform, ref config, ref movement, ref hitEntities, pointsList[pointsList.Length - 1], ref hitEntity);
				if (!IsFloat3Zero(@float))
				{
					num2--;
				}
			}
			if (num > 0 && IsFloat3Zero(@float))
			{
				num--;
				@float = pointsList[pointsList.Length - 1] + movement.Direction * spell.FallDamageRange;
			}
			if (IsFloat3Zero(@float))
			{
				break;
			}
			AppendReboundingFireLinePoints(in data, in config, ref movement, ref pointsList, transform, in spell, @float, out hitPosition);
		}
		movement.Direction = direction;
		movement.AroundAngle = aroundAngle;
		fireLinePointsBuffer.Clear();
		foreach (float3 item2 in pointsList)
		{
			fireLinePointsBuffer.Add(new Spell1025FireLinePointsBuffer
			{
				Position = item2
			});
		}
	}

	[BurstCompile]
	private float3 GetFirstFallGroundPosition(float3 fromPos, float dirTan, float3 dirInXY)
	{
		return fromPos + new float3((dirInXY * dirTan * math.abs(fromPos.z)).xy, 0f - fromPos.z);
	}

	[BurstCompile]
	private void AppendFirstFireLinePoints(in SpellComponentData data, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, LocalTransform transform, ref NativeList<float3> points, out float3 firstGroundPointPosition)
	{
		firstGroundPointPosition = float3.zero;
		switch (movement.Type)
		{
		case SpellSpecialMovementType.Normal:
			firstGroundPointPosition = GetFirstFallGroundPosition(transform.Position, movement.Speed / movement.CurrentFallSpeed, movement.Direction);
			points.Add(in firstGroundPointPosition);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		{
			firstGroundPointPosition = GetFirstFallGroundPosition(transform.Position, movement.Speed / movement.CurrentFallSpeed, movement.Direction);
			if (CurrentRoomEntities.FindMinAngleTarget(firstGroundPointPosition, movement.Direction, config.ShooterType, out var _, out var targetPosition, out var _))
			{
				firstGroundPointPosition = DTool.IgnoreZPosition(in transform.Position);
				float3 @float = DTool.IgnoreZDir(in targetPosition, in firstGroundPointPosition);
				float x = DTool.IgnoreZDistance(in firstGroundPointPosition, in targetPosition);
				x = math.min(x, movement.ChaseRotateSpeed * 0.15f);
				movement.Direction = @float;
				firstGroundPointPosition += x * @float;
				points.Add(in firstGroundPointPosition);
			}
			else
			{
				points.Add(in firstGroundPointPosition);
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			points.Add(in MousePosition);
			float3 to2 = points[points.Length - 1];
			float3 from = points[points.Length - 2];
			movement.Direction = DTool.IgnoreZDir(in to2, in from);
			firstGroundPointPosition = MousePosition;
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * movement.AroundRadius) * 5f;
			float3 float2 = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f)) * movement.AroundRadius;
			movement.AroundAngle += num * 0.5f;
			float3 float3 = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f)) * movement.AroundRadius;
			movement.AroundAngle += num * 0.5f;
			float3 float4 = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f)) * movement.AroundRadius;
			points.RemoveAt(points.Length - 1);
			float3 to2 = movement.AroundCenter + float2;
			float3 value = DTool.IgnoreZPosition(in to2, data.IsSplitSpell ? 0f : (-7f));
			points.Add(in value);
			to2 = movement.AroundCenter + float3;
			float3 value2 = DTool.IgnoreZPosition(in to2, data.IsSplitSpell ? 0f : (-3.5f));
			points.Add(in value2);
			to2 = movement.AroundCenter + float4;
			float3 value3 = DTool.IgnoreZPosition(in to2);
			points.Add(in value3);
			firstGroundPointPosition = value3;
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			float3 to;
			if (TransformLookUp.TryGetComponent(data.Shooter, out var componentData))
			{
				to = componentData.Position;
				movement.Direction = DTool.IgnoreZDir(in to, in transform.Position);
				points.Add(in to);
			}
			else
			{
				to = DTool.IgnoreZPosition(in movement.AroundCenter);
				movement.Direction = DTool.IgnoreZDir(in to, in transform.Position);
				points.Add(in to);
			}
			firstGroundPointPosition = to;
			break;
		}
		}
	}

	[BurstCompile]
	private void AppendReboundingFireLinePoints(in SpellComponentData data, in SpellConfigComponentData config, ref SpellMovementComponentData movement, ref NativeList<float3> pointsList, LocalTransform transform, in Spell1025DragonBreathData spell, float3 TargetPosition, out float3 hitPosition)
	{
		hitPosition = float3.zero;
		float3 direction = movement.Direction;
		float fallDamageRange = spell.FallDamageRange;
		float3 value = pointsList[pointsList.Length - 1] + movement.Direction * fallDamageRange * 0.5f;
		TargetPosition = DTool.IgnoreZPosition(in TargetPosition);
		if (!IsFloat3Zero(TargetPosition))
		{
			value = (pointsList[pointsList.Length - 1] + TargetPosition) / 2f;
		}
		value.z = -3f;
		switch (movement.Type)
		{
		case SpellSpecialMovementType.Normal:
			pointsList.Add(in value);
			pointsList.Add(in TargetPosition);
			hitPosition = TargetPosition;
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		{
			if (CurrentRoomEntities.FindMinAngleTarget(transform.Position, movement.Direction, config.ShooterType, out var _, out var targetPosition, out var _))
			{
				float2 float4 = movement.Direction.xy;
				if (math.distancesq(targetPosition, pointsList[pointsList.Length - 1]) >= 0.001f)
				{
					float3 float5 = math.normalizesafe(targetPosition - pointsList[pointsList.Length - 1]);
					float2 source = direction.xy;
					float2 target2 = float5.xy;
					float4 = DTool.DirMoveTowards(in source, in target2, movement.ChaseRotateSpeed * 8f);
				}
				float3 to = pointsList[pointsList.Length - 1];
				float num2 = math.min(DTool.IgnoreZDistance(in targetPosition, in to), fallDamageRange);
				movement.Direction = new float3(float4.x, float4.y, 0f);
				float3 value6 = pointsList[pointsList.Length - 1] + direction * num2;
				value6.z = 0f;
				pointsList.Add(in value);
				pointsList.Add(in value6);
				hitPosition = value6;
			}
			else
			{
				pointsList.Add(in value);
				pointsList.Add(in TargetPosition);
				hitPosition = TargetPosition;
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			float3 v4 = pointsList[pointsList.Length - 1];
			float3 v5 = pointsList[pointsList.Length - 1] + direction * fallDamageRange * 2f;
			float3 v6 = pointsList[pointsList.Length - 1] + DTool.RotateDir(direction, 90f) * fallDamageRange * 2f;
			float3 v7 = MousePosition;
			v6.z = value.z;
			v5.z = value.z;
			for (int j = 0; j <= 20; j++)
			{
				float t2 = (float)j / 20f;
				float3 value5 = DTool.CubicBezierCurve(in v4, in v5, in v6, in v7, t2);
				pointsList.Add(in value5);
			}
			hitPosition = v7;
			float3 to = pointsList[pointsList.Length - 1];
			float3 from = pointsList[pointsList.Length - 2];
			movement.Direction = DTool.IgnoreZDir(in to, in from);
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * movement.AroundRadius) * 5f;
			movement.AroundAngle += num * 0.5f;
			float3 @float = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f)) * movement.AroundRadius;
			movement.AroundAngle += num * 0.5f;
			float3 float2 = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f)) * movement.AroundRadius;
			float3 float3 = DTool.IgnoreZPosition(in movement.AroundCenter);
			float3 to = float3 + @float;
			float3 value3 = DTool.IgnoreZPosition(in to, -3f);
			to = float3 + float2;
			float3 value4 = DTool.IgnoreZPosition(in to);
			pointsList.Add(in value3);
			pointsList.Add(in value4);
			hitPosition = value4;
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			float3 position = ((!TransformLookUp.TryGetComponent(data.Shooter, out var componentData)) ? transform.Position : componentData.Position);
			position = DTool.IgnoreZPosition(in position);
			float3 v = pointsList[pointsList.Length - 1];
			float3 v2 = pointsList[pointsList.Length - 1] + direction * fallDamageRange * 2f;
			float3 v3 = pointsList[pointsList.Length - 1] + DTool.RotateDir(direction, 90f) * fallDamageRange * 2f;
			v3.z = value.z;
			v2.z = value.z;
			for (int i = 0; i <= 20; i++)
			{
				float t = (float)i / 20f;
				float3 value2 = DTool.CubicBezierCurve(in v, in v2, in v3, in position, t);
				pointsList.Add(in value2);
			}
			hitPosition = position;
			float3 to = pointsList[pointsList.Length - 1];
			float3 from = pointsList[pointsList.Length - 2];
			movement.Direction = DTool.IgnoreZDir(in to, in from);
			break;
		}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1025DragonBreathData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		BufferAccessor<Spell1025FireLinePointsBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1025FireLinePointsBuffer_RW_BufferTypeHandle);
		BufferAccessor<Spell1025FireGroundEffectBuffer> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, i);
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
				ref Spell1025DragonBreathData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, i);
				ref PhysicsVelocity phys = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, i);
				DynamicBuffer<Spell1025FireLinePointsBuffer> buffer = bufferAccessor[i];
				DynamicBuffer<Spell1025FireGroundEffectBuffer> groundEffectBuffer = bufferAccessor2[i];
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, i);
				Execute(chunkIndexInQuery, ref movement, ref data, ref transform, default(SpellFallTag), ref config, ref spell, ref phys, ref buffer, ref groundEffectBuffer, entity);
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
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref Spell1025DragonBreathData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, nextRangeBegin);
					ref PhysicsVelocity phys2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, nextRangeBegin);
					DynamicBuffer<Spell1025FireLinePointsBuffer> buffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<Spell1025FireGroundEffectBuffer> groundEffectBuffer2 = bufferAccessor2[nextRangeBegin];
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, nextRangeBegin);
					Execute(chunkIndexInQuery, ref movement2, ref data2, ref transform2, default(SpellFallTag), ref config2, ref spell2, ref phys2, ref buffer2, ref groundEffectBuffer2, entity2);
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
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
				ref Spell1025DragonBreathData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, j);
				ref PhysicsVelocity phys3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, j);
				DynamicBuffer<Spell1025FireLinePointsBuffer> buffer3 = bufferAccessor[j];
				DynamicBuffer<Spell1025FireGroundEffectBuffer> groundEffectBuffer3 = bufferAccessor2[j];
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, j);
				Execute(chunkIndexInQuery, ref movement3, ref data3, ref transform3, default(SpellFallTag), ref config3, ref spell3, ref phys3, ref buffer3, ref groundEffectBuffer3, entity3);
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
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
				ref Spell1025DragonBreathData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, k);
				ref PhysicsVelocity phys4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, k);
				DynamicBuffer<Spell1025FireLinePointsBuffer> buffer4 = bufferAccessor[k];
				DynamicBuffer<Spell1025FireGroundEffectBuffer> groundEffectBuffer4 = bufferAccessor2[k];
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, k);
				Execute(chunkIndexInQuery, ref movement4, ref data4, ref transform4, default(SpellFallTag), ref config4, ref spell4, ref phys4, ref buffer4, ref groundEffectBuffer4, entity4);
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
