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

[CompilerGenerated]
[BurstCompile]
public struct Spell3015WormJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell3015WormComponent> __Spell3015WormComponent_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell3015WormComponent_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell3015WormComponent>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell3015WormComponent_RW_ComponentTypeHandle.Update(ref state);
				__PathFinding_RW_ComponentTypeHandle.Update(ref state);
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
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3015WormComponent>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
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
		public void Run(ref Spell3015WormJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell3015WormJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell3015WormJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell3015WormJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell3015WormJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell3015WormJob job, EntityManager entityManager)
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

	public float deltaTime;

	[ReadOnly]
	public CurrentRoomEntitiesSingleton currentRoomEntitiesSingleton;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> localTransformLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<MatOverrideFrameIndex> matOverrideFrameIndexLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	public SpellSingleton spellSingleton;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public Entity GlobalParticleEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref Spell3015WormComponent worm, ref PathFinding pathFinding, ref SpellConfigComponentData config, Entity entity)
	{
		worm.checkIntervalTimer += deltaTime;
		if (worm.checkIntervalTimer > 1f)
		{
			worm.checkIntervalTimer = 0f;
			currentRoomEntitiesSingleton.FindNearestTarget(localTransformLookup.GetRefRO(entity).ValueRO.Position, UnitType.Teammate, out worm.nearestTarget, out var _, out worm.unitProperty_Dots);
		}
		float3 position = localTransformLookup.GetRefRO(entity).ValueRO.Position;
		RefRW<LocalTransform> refRW = localTransformLookup.GetRefRW(entity);
		ref LocalTransform valueRW = ref refRW.ValueRW;
		worm.exitTimer -= deltaTime;
		if (worm.exitTimer <= 0f)
		{
			Explosion(position, config, chunkIndexInQuery, worm, entity);
		}
		RefRW<MatOverrideFrameIndex> refRW2;
		if (worm.nearestTarget != Entity.Null && localTransformLookup.HasComponent(worm.nearestTarget))
		{
			float3 position2 = localTransformLookup.GetRefRO(worm.nearestTarget).ValueRO.Position;
			pathFinding.UpdatePath(position, position2, 16);
			refRW = localTransformLookup.GetRefRW(entity);
			refRW.ValueRW.Position += (float3)Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, position) * worm.wormInfo.moveSpeed * deltaTime;
			refRW2 = matOverrideFrameIndexLookup.GetRefRW(worm.meshEntity);
			ref MatOverrideFrameIndex valueRW2 = ref refRW2.ValueRW;
			valueRW2.FrameIndex += deltaTime * 70f;
			if (valueRW2.FrameIndex >= 16f)
			{
				valueRW2.FrameIndex = 0f;
			}
			if (Tool2D.IgnoreZDistanceSqr(position, position2) < worm.unitProperty_Dots.size + config.Radius.CalculateWithNewBaseValue(1f))
			{
				Explosion(position, config, chunkIndexInQuery, worm, entity);
			}
			return;
		}
		gRandom.ValueRW.NextFloatByChunkIndex(chunkIndexInQuery * 10000);
		if (worm.idleTimer >= 0f)
		{
			worm.idleTimer -= deltaTime;
			return;
		}
		worm.randomMoveTimer -= deltaTime;
		if (worm.randomMoveTimer <= 0f)
		{
			worm.idleTimer = 0.7f;
			worm.randomMoveTargetPoint = position + DTool.GetDir(ref gRandom.ValueRW.random) * gRandom.ValueRW.random.NextFloat(1.5f, 2.5f);
			worm.randomMoveTimer = 3f;
			pathFinding.UpdatePath(position, worm.randomMoveTargetPoint, 16);
			return;
		}
		if (pathFinding.allCornerArrived)
		{
			worm.randomMoveTargetPoint = position + DTool.GetDir(ref gRandom.ValueRW.random) * gRandom.ValueRW.random.NextFloat(1.5f, 2.5f);
		}
		pathFinding.UpdatePath(position, worm.randomMoveTargetPoint, 16);
		valueRW.Position += (float3)Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, position) * worm.wormInfo.moveSpeed * deltaTime;
		refRW2 = matOverrideFrameIndexLookup.GetRefRW(worm.meshEntity);
		ref MatOverrideFrameIndex valueRW3 = ref refRW2.ValueRW;
		valueRW3.FrameIndex += deltaTime * 70f;
		if (valueRW3.FrameIndex >= 16f)
		{
			valueRW3.FrameIndex = 0f;
		}
	}

	public void Explosion(float3 thisPosition, SpellConfigComponentData config, int chunkIndexInQuery, Spell3015WormComponent worm, Entity entity)
	{
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		float radius = config.Radius.CalculateWithNewBaseValue(1f);
		UnitType selfCamp = UnitType.Teammate;
		SpellTools.GetAttackableEntitiesInRange(in thisPosition, in radius, in selfCamp, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		foreach (Entity item in entities)
		{
			Entity target = item;
			TakeDamageInfo_Dots damage = worm.damageInfo;
			damage.spell.HitPosition = localTransformLookup[target].Position;
			CMD.TryAttackEntity(chunkIndexInQuery, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
		}
		CMD.AppendToBuffer(chunkIndexInQuery, GlobalParticleEntity, new GlobalParticleEmitParams
		{
			Position = thisPosition,
			Size = config.Radius.CalculateWithNewBaseValue(1f),
			Name = "3015_Explosion"
		});
		CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndexInQuery, entity, value: true);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell3015WormComponent_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell3015WormComponent worm = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3015WormComponent>(nativeArrayPtr, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr2, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
				Execute(chunkIndexInQuery, ref worm, ref pathFinding, ref config, entity);
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
					ref Spell3015WormComponent worm2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3015WormComponent>(nativeArrayPtr, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr2, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
					Execute(chunkIndexInQuery, ref worm2, ref pathFinding2, ref config2, entity2);
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
				ref Spell3015WormComponent worm3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3015WormComponent>(nativeArrayPtr, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr2, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
				Execute(chunkIndexInQuery, ref worm3, ref pathFinding3, ref config3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell3015WormComponent worm4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3015WormComponent>(nativeArrayPtr, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr2, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
				Execute(chunkIndexInQuery, ref worm4, ref pathFinding4, ref config4, entity4);
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
