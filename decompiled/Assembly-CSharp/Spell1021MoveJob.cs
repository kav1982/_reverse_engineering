using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[CompilerGenerated]
[BurstCompile]
public struct Spell1021MoveJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1021MagicBreakerData> __Spell1021MagicBreakerData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1021MagicBreakerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1021MagicBreakerData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
				__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>(isReadOnly: true);
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell1021MagicBreakerData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsCollider>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021MagicBreakerData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
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
		public void Run(ref Spell1021MoveJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1021MoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1021MoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1021MoveJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1021MoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1021MoveJob job, EntityManager entityManager)
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

	public Unity.Mathematics.Random random;

	public float DeltaTime;

	public Entity GlobalParticle;

	public GlobalRandom Random;

	public EntityCommandBuffer.ParallelWriter CMD;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellHalfLifeTeleportData> HalfLifeTeleportLookup;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private float GetCurveValue(float percent)
	{
		percent = math.clamp(percent, 0f, 1f);
		float2 p = new float2(0.2f, 0.5f);
		float2 p2 = new float2(0.6f, 0.95f);
		percent = DTool.GetCurveY(in p, in p2, percent);
		return percent;
	}

	[BurstCompile]
	private void HalfLifeRandomTeleport(ref GlobalRandom random, ref SpellMovementComponentData movement, ref Spell1021MagicBreakerData spell, ref SpellConfigComponentData config)
	{
		movement.AroundCenter += random.random.NextFloat(3f, 5f) * DTool.GetDir(random.random.NextFloat(0f, 360f)) * config.Radius.MulRatio * (1f + config.Radius.AddRatio);
		movement.AroundTarget = Entity.Null;
		config.DamageTimer = 0f;
		movement.Speed += 2f;
		spell.BaseDirection = DTool.GetDir(random.random.NextFloat(0f, 360f));
		spell.lastAngle = 0f;
	}

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref Spell1021MagicBreakerData spell, ref SpellComponentData data, in PhysicsVelocity velocity, in PhysicsCollider collider, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, Entity ett)
	{
		ref LocalTransform valueRW = ref LocalTransformLookup.GetRefRW(ett).ValueRW;
		if (spell.readyToDestroy)
		{
			spell.FadeTimer += DeltaTime;
			if (spell.FadeTimer >= 0.5f + config.HoverDuration)
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(index, ett, value: true);
				return;
			}
		}
		if (HalfLifeTeleportLookup.TryGetComponent(ett, out var componentData) && componentData.TeleportCount > 0 && config.DamageTimer > spell.SlashTime * 0.4f)
		{
			CMD.AppendToBuffer(index, GlobalParticle, new GlobalParticleEmitParams
			{
				Position = movement.AroundCenter,
				Name = $"{3130}_Teleport",
				Size = valueRW.Scale
			});
			data.Shooter = Entity.Null;
			componentData.TeleportCount--;
			movement.Speed += 2f;
			config.DamageTimer = 0f;
			spell.SlashStage = Spell1021SlashStage.Before;
			CMD.SetComponent(index, ett, componentData);
			HalfLifeRandomTeleport(ref Random, ref movement, ref spell, ref config);
			CMD.AppendToBuffer(index, GlobalParticle, new GlobalParticleEmitParams
			{
				Position = movement.AroundCenter,
				Name = $"{3130}_Teleport",
				Size = valueRW.Scale
			});
		}
		if (movement.IsFallSpell)
		{
			float3 @float = velocity.Linear;
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				@float = movement.OriginalSpellHorizontalSpeed * movement.Direction;
			}
			float3 rootPosition = @float + new float3(0f, 0f, movement.CurrentFallSpeed);
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			rootPosition += layerPosition;
			quaternion quaternion = (valueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(rootPosition.y, rootPosition.x)));
			return;
		}
		float num = ((movement.Type != SpellSpecialMovementType.Rotation) ? 0.05f : movement.AroundRadius);
		float num2 = GetCurveValue(config.DamageTimer / spell.SlashTime) * config.Scatter * (float)(spell.FlipY ? 1 : (-1));
		movement.Direction = DTool.RotateDir(spell.BaseDirection, num2);
		float2 dir = movement.Direction.xy;
		valueRW.Rotation = DTool.DirectionToRotation(in dir);
		UpdatePosition(index, ett, ref spell, collider, ref valueRW, ref movement, ref config, num);
		if (spell.lastAngle != 0f)
		{
			spell.moveAngle += math.abs(num2 - spell.lastAngle);
			float num3 = 12f;
			if (spell.moveAngle >= num3)
			{
				int num4 = ((num2 > spell.lastAngle) ? 90 : (-90));
				config.ColorType.ColorEnumToString(out var result);
				float y = (1f + config.Radius.AddRatio) * config.Radius.MulRatio * (data.IsSplitSpell ? 0.8f : 1f);
				float max = 1.28f * math.max(0.75f, y) * valueRW.Scale * 1.75f;
				float min = 0.5f * valueRW.Scale;
				float num5 = spell.moveAngle / num3;
				for (int i = 0; (float)i < num5; i++)
				{
					float degree = spell.lastAngle + (float)i * num3 * random.NextFloat((0f - num3) / 5f, num3 / 5f);
					float3 float2 = DTool.RotateDir(spell.BaseDirection, degree);
					float num6 = random.NextFloat(min, max);
					float3 position = movement.AroundCenter + float2 * num + float2 * num6;
					CMD.AppendToBuffer(index, GlobalParticle, new GlobalParticleEmitParams
					{
						Name = $"1021_Ember_{result}",
						Size = valueRW.Scale,
						Position = position,
						Velocity = DTool.RotateDir(float2, num4) * random.NextFloat(8f, 10f)
					});
				}
				spell.moveAngle -= num3 * num5;
			}
		}
		spell.lastAngle = num2;
	}

	private void UpdatePosition(int index, Entity entity, ref Spell1021MagicBreakerData spell, PhysicsCollider collider, ref LocalTransform transform, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, float aroundRadius)
	{
		switch (spell.SlashStage)
		{
		case Spell1021SlashStage.Before:
			if (LocalTransformLookup.HasComponent(movement.AroundTarget))
			{
				movement.AroundCenter = LocalTransformLookup[movement.AroundTarget].Position;
			}
			config.DamageTimer += DeltaTime;
			if (spell.FadeTimer < 0.2f)
			{
				spell.FadeTimer += DeltaTime;
			}
			else
			{
				spell.SlashStage = Spell1021SlashStage.Normal;
				spell.FadeTimer = 0f;
			}
			transform.Position = movement.AroundCenter + movement.Direction * aroundRadius;
			break;
		case Spell1021SlashStage.Normal:
			if (LocalTransformLookup.HasComponent(movement.AroundTarget))
			{
				movement.AroundCenter = LocalTransformLookup[movement.AroundTarget].Position;
			}
			config.DamageTimer += DeltaTime;
			if (config.DamageTimer >= spell.SlashTime * 0.8f)
			{
				if (config.HoverDuration > 0f && config.HoverTimer < config.HoverDuration)
				{
					config.HoverTimer += DeltaTime;
					break;
				}
				SpellTools.DisableSpellTrigger(in collider);
				spell.SlashStage = Spell1021SlashStage.After;
			}
			else
			{
				transform.Position = movement.AroundCenter + movement.Direction * aroundRadius;
			}
			break;
		case Spell1021SlashStage.After:
			if (config.HoverDuration <= 0f)
			{
				config.DamageTimer += DeltaTime;
				if (config.Scatter >= 10f)
				{
					spell.BaseDirection = DTool.RotateDir(spell.BaseDirection, 4f * movement.Speed * (float)(spell.FlipY ? 1 : (-1)) * DeltaTime);
				}
			}
			if (spell.FadeTimer < 0.25f)
			{
				spell.FadeTimer += DeltaTime;
				break;
			}
			if (!movement.IsFallSpell)
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(index, entity, value: true);
				break;
			}
			config.DamageTimer = 0f;
			spell.FadeTimer = 0f;
			spell.readyToDestroy = true;
			break;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1021MagicBreakerData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1021MagicBreakerData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, i);
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr3, i);
				ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr4, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, i);
				Execute(chunkIndexInQuery, ref spell, ref data, in velocity, in collider, ref movement, ref config, ett);
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
					ref Spell1021MagicBreakerData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr3, nextRangeBegin);
					ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr4, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, nextRangeBegin);
					Execute(chunkIndexInQuery, ref spell2, ref data2, in velocity2, in collider2, ref movement2, ref config2, ett2);
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
				ref Spell1021MagicBreakerData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, j);
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr3, j);
				ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr4, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, j);
				Execute(chunkIndexInQuery, ref spell3, ref data3, in velocity3, in collider3, ref movement3, ref config3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1021MagicBreakerData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, k);
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr3, k);
				ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr4, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, k);
				Execute(chunkIndexInQuery, ref spell4, ref data4, in velocity4, in collider4, ref movement4, ref config4, ett4);
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
