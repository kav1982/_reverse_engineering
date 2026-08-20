using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rukhanka;
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
public struct Monster8Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Monster8_Dots> __Monster8_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Monster8_Dots_Amaze> __Monster8_Dots_Amaze_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Monster8_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster8_Dots>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__Monster8_Dots_Amaze_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster8_Dots_Amaze>();
				__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Monster8_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Monster8_Dots_Amaze_RW_ComponentTypeHandle.Update(ref state);
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
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster8_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Monster8_Dots_Amaze>();
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
		public void Run(ref Monster8Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Monster8Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Monster8Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Monster8Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Monster8Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Monster8Job job, EntityManager entityManager)
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

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> localTransformLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> unitPptLookUp;

	[NativeDisableParallelForRestriction]
	public BufferLookup<AnimatorControllerParameterComponent> animaLookUp;

	[NativeDisableParallelForRestriction]
	public EntityStorageInfoLookup entityLookUp;

	[NativeDisableParallelForRestriction]
	public PhysicsWorldSingleton pws;

	[ReadOnly]
	public CollisionFilter collisionFilter;

	public float deltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute(ref Monster8_Dots monster, ref UnitBase_Dots unitBase, ref Monster8_Dots_Amaze amaze, ref PathFinding pathFinding, Entity entity)
	{
		animaLookUp.TryGetBuffer(unitBase.ett_AnimaRoot, out var bufferData);
		RefRW<UnitProperty_Dots> refRW = unitPptLookUp.GetRefRW(entity);
		if (!monster.isInitialized)
		{
			monster.isInitialized = true;
			monster.idleTime.RandomResult(ref gRandom.ValueRW.random);
			monster.randomWalkTime.RandomResult(ref gRandom.ValueRW.random);
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
		monster.stateExistTime += deltaTime;
		RefRW<LocalTransform> refRW2 = localTransformLookUp.GetRefRW(entity);
		for (int i = 0; i < bufferData.Length; i++)
		{
			AnimatorControllerParameterComponent value = bufferData[i];
			value.BoolValue = false;
			bufferData[i] = value;
		}
		amaze.informOthers = false;
		switch (monster.state)
		{
		case Monster8State.BornIdle:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value6 = bufferData[0];
				value6.BoolValue = true;
				bufferData[0] = value6;
			}
			if (monster.stateExistTime >= 0.5f)
			{
				monster.state = Monster8State.Idle;
			}
			unitBase.SetMove(float3.zero);
			break;
		case Monster8State.Idle:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value5 = bufferData[0];
				value5.BoolValue = true;
				bufferData[0] = value5;
				monster.idleTime.RandomResult(ref gRandom.ValueRW.random);
			}
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			if (monster.stateExistTime >= monster.idleTime.result)
			{
				monster.state = Monster8State.RandomWalk;
			}
			monster.checkIntervalTimer += deltaTime;
			if (monster.checkIntervalTimer >= 1f)
			{
				monster.checkIntervalTimer = 0f;
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW2.ValueRO.Position, monster.checkRadius, refRW.ValueRO.unitCfg.unitType, unitPptLookUp, entityLookUp, pws);
			}
			if (unitBase.targetEtt != Entity.Null)
			{
				monster.state = Monster8State.Amaze;
			}
			break;
		case Monster8State.RandomWalk:
		{
			if (monster.changedState)
			{
				monster.randomWalkTime.RandomResult(ref gRandom.ValueRW.random);
				AnimatorControllerParameterComponent value4 = bufferData[1];
				value4.BoolValue = true;
				bufferData[1] = value4;
				float3 dir = DTool.GetDir(ref gRandom.ValueRW.random, in monster.randomWalkRadius);
				monster.randomWalkPosition = refRW2.ValueRO.Position + dir;
			}
			float3 float2 = DTool.IgnoreZDir(in monster.randomWalkPosition, in refRW2.ValueRO.Position);
			unitBase.SetMove(float2 * refRW.ValueRO.unitCfg.moveSpeed);
			if (DTool.IgnoreZDistanceSqr(in monster.randomWalkPosition, in refRW2.ValueRO.Position) < monster.moveThreshold * monster.moveThreshold)
			{
				float3 dir2 = DTool.GetDir(ref gRandom.ValueRW.random, in monster.randomWalkRadius);
				monster.randomWalkPosition = refRW2.ValueRO.Position + dir2;
			}
			if (monster.stateExistTime >= monster.randomWalkTime.result)
			{
				monster.state = Monster8State.Idle;
			}
			monster.checkIntervalTimer += deltaTime;
			if (monster.checkIntervalTimer >= 1f)
			{
				monster.checkIntervalTimer = 0f;
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW2.ValueRO.Position, monster.checkRadius, refRW.ValueRO.unitCfg.unitType, unitPptLookUp, entityLookUp, pws);
			}
			if (unitBase.targetEtt != Entity.Null)
			{
				monster.state = Monster8State.Amaze;
			}
			break;
		}
		case Monster8State.Amaze:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value3 = bufferData[2];
				value3.BoolValue = true;
				bufferData[2] = value3;
				amaze.targetEtt = unitBase.targetEtt;
				amaze.informPosition = refRW2.ValueRO.Position;
			}
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			break;
		case Monster8State.FollowTarget:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value2 = bufferData[1];
				value2.BoolValue = true;
				bufferData[1] = value2;
			}
			if (unitBase.targetEtt != Entity.Null && entityLookUp.Exists(unitBase.targetEtt) && localTransformLookUp.HasComponent(unitBase.targetEtt))
			{
				LocalTransform valueRO = localTransformLookUp.GetRefRO(unitBase.targetEtt).ValueRO;
				pathFinding.UpdatePath(refRW2.ValueRO.Position, valueRO.Position, 16);
				float3 @float = DTool.IgnoreZDir(in pathFinding.walkToPoint, in refRW2.ValueRO.Position);
				unitBase.SetMove(@float * refRW.ValueRO.unitCfg.moveSpeed);
			}
			else
			{
				monster.state = Monster8State.Idle;
			}
			break;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster8_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster8_Dots_Amaze_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Monster8_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots>(nativeArrayPtr, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
				ref Monster8_Dots_Amaze amaze = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots_Amaze>(nativeArrayPtr3, i);
				ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, i);
				Execute(ref monster, ref unitBase, ref amaze, ref pathFinding, entity);
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
					ref Monster8_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots>(nativeArrayPtr, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					ref Monster8_Dots_Amaze amaze2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots_Amaze>(nativeArrayPtr3, nextRangeBegin);
					ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, nextRangeBegin);
					Execute(ref monster2, ref unitBase2, ref amaze2, ref pathFinding2, entity2);
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
				ref Monster8_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots>(nativeArrayPtr, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
				ref Monster8_Dots_Amaze amaze3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots_Amaze>(nativeArrayPtr3, j);
				ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, j);
				Execute(ref monster3, ref unitBase3, ref amaze3, ref pathFinding3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Monster8_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots>(nativeArrayPtr, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
				ref Monster8_Dots_Amaze amaze4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster8_Dots_Amaze>(nativeArrayPtr3, k);
				ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr4, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, k);
				Execute(ref monster4, ref unitBase4, ref amaze4, ref pathFinding4, entity4);
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
