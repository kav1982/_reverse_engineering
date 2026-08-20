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
public struct Monster1Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Monster1_Dots> __Monster1_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Monster1_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster1_Dots>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Monster1_Dots_RW_ComponentTypeHandle.Update(ref state);
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
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster1_Dots>();
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
		public void Run(ref Monster1Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Monster1Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Monster1Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Monster1Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Monster1Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Monster1Job job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> cluUnitPpt;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> cluLocalTsf;

	[NativeDisableParallelForRestriction]
	public PhysicsWorldSingleton pws;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	[ReadOnly]
	public EntityStorageInfoLookup ettStorageInfoLookUp;

	public float deltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute(ref Monster1_Dots monster, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity ett)
	{
		if (!monster.isInitialized)
		{
			monster.isInitialized = true;
			monster.idleTime.RandomResult(ref gRandom.ValueRW.random);
			monster.randomWalkTime.RandomResult(ref gRandom.ValueRW.random);
		}
		LocalTransform valueRO = cluLocalTsf.GetRefRO(ett).ValueRO;
		UnitProperty_Dots valueRO2 = cluUnitPpt.GetRefRW(ett).ValueRO;
		switch (monster.state)
		{
		case Monster1State.BornIdle:
			_ = monster.changedState;
			unitBase.SetMove(float3.zero);
			monster.bornIdleTimer += deltaTime;
			if (monster.bornIdleTimer >= 0.5f)
			{
				monster.state = Monster1State.Idle;
			}
			break;
		case Monster1State.Idle:
			_ = monster.changedState;
			unitBase.SetMove(float3.zero);
			monster.idleTimer += deltaTime;
			if (!(monster.idleTimer >= monster.idleTime.result))
			{
				break;
			}
			monster.idleTimer = 0f;
			if (DTool.RandomValue(ref gRandom.ValueRW.random) < monster.followTargetChance)
			{
				unitBase.targetEtt = DTool.GetNearestTargetEtt(valueRO.Position, 999f, valueRO2.unitCfg.unitType, cluUnitPpt, ettStorageInfoLookUp, pws);
				if (unitBase.targetEtt != Entity.Null && DTool.IgnoreZDistanceSqr(in valueRO.Position, in cluLocalTsf.GetRefRO(unitBase.targetEtt).ValueRO.Position) < monster.followTargetDistance * monster.followTargetDistance)
				{
					monster.state = Monster1State.FollowTarget;
				}
			}
			if (monster.state != Monster1State.FollowTarget)
			{
				monster.state = Monster1State.RandomWalk;
				float3 dir = DTool.GetDir(ref gRandom.ValueRW.random, in monster.randomWalkDistance);
				monster.randomWalkPosition = valueRO.Position + dir;
			}
			break;
		case Monster1State.RandomWalk:
		{
			pathFinding.UpdatePath(valueRO.Position, monster.randomWalkPosition, 16);
			float3 @float = DTool.IgnoreZDir(in pathFinding.walkToPoint, in valueRO.Position);
			unitBase.SetMove(@float * valueRO2.unitCfg.moveSpeed);
			if (DTool.IgnoreZDistanceSqr(in monster.randomWalkPosition, in valueRO.Position) < unitBase.moveThreshold * unitBase.moveThreshold)
			{
				float3 dir2 = DTool.GetDir(ref gRandom.ValueRW.random, in monster.randomWalkDistance);
				monster.randomWalkPosition = valueRO.Position + dir2;
			}
			monster.randomWalkTimer += deltaTime;
			if (monster.randomWalkTimer >= monster.randomWalkTime.result)
			{
				monster.randomWalkTimer = 0f;
				monster.randomWalkTime.RandomResult(ref gRandom.ValueRW.random);
				monster.state = Monster1State.Idle;
			}
			break;
		}
		case Monster1State.FollowTarget:
			_ = monster.changedState;
			if (unitBase.targetEtt != Entity.Null && ettStorageInfoLookUp.Exists(unitBase.targetEtt) && cluLocalTsf.HasComponent(unitBase.targetEtt))
			{
				LocalTransform valueRO3 = cluLocalTsf.GetRefRO(unitBase.targetEtt).ValueRO;
				if (DTool.IgnoreZDistanceSqr(in valueRO3.Position, in valueRO.Position) < unitBase.moveThreshold * unitBase.moveThreshold)
				{
					unitBase.SetMove(float3.zero);
				}
				else
				{
					pathFinding.UpdatePath(valueRO.Position, valueRO3.Position, 16);
					unitBase.SetMove(DTool.IgnoreZDir(in pathFinding.walkToPoint, in valueRO.Position) * valueRO2.unitCfg.moveSpeed);
				}
			}
			else
			{
				monster.state = Monster1State.Idle;
			}
			monster.followTargetTimer += deltaTime;
			if (monster.followTargetTimer >= monster.followTargetTime)
			{
				monster.followTargetTimer = 0f;
				monster.state = Monster1State.Idle;
			}
			break;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster1_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Monster1_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster1_Dots>(nativeArrayPtr, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
				Execute(ref monster, ref unitBase, ref pathFinding, ett);
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
					ref Monster1_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster1_Dots>(nativeArrayPtr, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
					Execute(ref monster2, ref unitBase2, ref pathFinding2, ett2);
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
				ref Monster1_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster1_Dots>(nativeArrayPtr, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
				Execute(ref monster3, ref unitBase3, ref pathFinding3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Monster1_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster1_Dots>(nativeArrayPtr, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
				Execute(ref monster4, ref unitBase4, ref pathFinding4, ett4);
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
