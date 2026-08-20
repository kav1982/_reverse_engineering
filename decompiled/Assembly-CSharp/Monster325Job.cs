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
using Unity.Transforms;

[CompilerGenerated]
[BurstCompile]
public struct Monster325Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Monster325_Dots> __Monster325_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Monster325_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster325_Dots>();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Monster325_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__PathFinding_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster325_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
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
		public void Run(ref Monster325Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Monster325Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Monster325Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Monster325Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Monster325Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Monster325Job job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> Random;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<AnimaPlay> AnimaLookUp;

	public float DeltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, ref Monster325_Dots monster, ref UnitProperty_Dots unitPpt, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity)
	{
		float3 position = LocalTransformLookUp[entity].Position;
		if (!monster.Initialized)
		{
			monster.Initialized = true;
			monster.idleTime.RandomResult(ref Random.ValueRW.random);
			monster.randomWalkTime.RandomResult(ref Random.ValueRW.random);
			monster.state = Monster325State.BornIdle;
		}
		if (unitPpt.LockMotion)
		{
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			return;
		}
		if (monster.stateQuit)
		{
			monster.stateQuit = false;
			monster.changedState = true;
		}
		else
		{
			monster.changedState = false;
		}
		monster.stateExistTime += DeltaTime;
		if (unitBase.ett_AnimaRoot != Entity.Null && AnimaLookUp.HasComponent(unitBase.ett_AnimaRoot))
		{
			ref AnimaPlay valueRW = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			switch (monster.state)
			{
			case Monster325State.BornIdle:
			case Monster325State.Idle:
				valueRW.Play(0);
				valueRW.animaSpeed = 0f;
				break;
			case Monster325State.RandomWalk:
				valueRW.Play(1);
				valueRW.animaSpeed = 1f;
				break;
			}
		}
		switch (monster.state)
		{
		case Monster325State.BornIdle:
			BornIdle(ref monster, ref unitBase);
			break;
		case Monster325State.Idle:
			Idle(ref monster, ref unitBase, position);
			break;
		case Monster325State.RandomWalk:
			RandomWalk(ref monster, ref unitBase, ref pathFinding, unitPpt, position);
			break;
		}
	}

	private void BornIdle(ref Monster325_Dots monster, ref UnitBase_Dots unitBase)
	{
		unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
		if (monster.stateExistTime >= 0.5f)
		{
			monster.state = Monster325State.Idle;
		}
	}

	private void Idle(ref Monster325_Dots monster, ref UnitBase_Dots unitBase, float3 currentPos)
	{
		unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
		if (monster.changedState)
		{
			monster.idleTime.RandomResult(ref Random.ValueRW.random);
		}
		if (!(monster.stateExistTime < monster.idleTime.result))
		{
			monster.state = Monster325State.RandomWalk;
		}
	}

	private void RandomWalk(ref Monster325_Dots monster, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, UnitProperty_Dots unitPpt, float3 currentPos)
	{
		if (monster.changedState)
		{
			monster.randomWalkTime.RandomResult(ref Random.ValueRW.random);
			monster.randomWalkPoint = GetRandomWalkPointToTarget(currentPos, monster);
			pathFinding.UpdatePath(currentPos, monster.randomWalkPoint, 16, needUpdateNow: true);
		}
		if (DTool.IgnoreZDistanceSqr(in currentPos, in monster.randomWalkPoint) <= unitBase.moveThreshold * unitBase.moveThreshold)
		{
			monster.randomWalkPoint = GetRandomWalkPointToTarget(currentPos, monster);
			pathFinding.UpdatePath(currentPos, monster.randomWalkPoint, 16, needUpdateNow: true);
		}
		if (monster.stateExistTime >= monster.randomWalkTime.result)
		{
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			monster.state = Monster325State.Idle;
		}
		else
		{
			pathFinding.UpdatePath(currentPos, monster.randomWalkPoint, 16);
			float3 @float = DTool.IgnoreZDir(in pathFinding.walkToPoint, in currentPos);
			unitBase.SetMove(@float * unitPpt.MoveSpeed * monster.moveSpeedRatio);
		}
	}

	private float3 GetRandomWalkPointToTarget(float3 currentPos, Monster325_Dots monster)
	{
		if (CurrentRoomEntities.FindNearestTarget(currentPos, UnitType.Monster, out var _, out var targetPosition, out var _))
		{
			float3 @float = DTool.IgnoreZDir(in targetPosition, in currentPos);
			bool num = DTool.IgnoreZDistanceSqr(in currentPos, in targetPosition) <= monster.closeRandomWalkTriggerDistance * monster.closeRandomWalkTriggerDistance;
			float num2 = (num ? monster.closeRandomWalkToTargetAngle : monster.randomWalkToTargetAngle);
			RandomFloat randomFloat = (num ? monster.closeRandomWalkDistance : monster.randomWalkDistance);
			float3 float2 = Tool2D.GetDir(degree: Random.ValueRW.random.NextFloat(0f - num2, num2), oldDir: @float);
			float num3 = randomFloat.RandomResult(ref Random.ValueRW.random);
			return currentPos + float2 * num3;
		}
		float3 dir = DTool.GetDir(ref Random.ValueRW.random, in monster.randomWalkDistance);
		return currentPos + dir;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster325_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Monster325_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster325_Dots>(nativeArrayPtr, i);
				ref UnitProperty_Dots unitPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, i);
				Execute(chunkIndexInQuery, ref monster, ref unitPpt, ref unitBase, ref pathFinding, entity);
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
					ref Monster325_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster325_Dots>(nativeArrayPtr, nextRangeBegin);
					ref UnitProperty_Dots unitPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, nextRangeBegin);
					Execute(chunkIndexInQuery, ref monster2, ref unitPpt2, ref unitBase2, ref pathFinding2, entity2);
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
				ref Monster325_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster325_Dots>(nativeArrayPtr, j);
				ref UnitProperty_Dots unitPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, j);
				Execute(chunkIndexInQuery, ref monster3, ref unitPpt3, ref unitBase3, ref pathFinding3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Monster325_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster325_Dots>(nativeArrayPtr, k);
				ref UnitProperty_Dots unitPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, k);
				Execute(chunkIndexInQuery, ref monster4, ref unitPpt4, ref unitBase4, ref pathFinding4, entity4);
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
