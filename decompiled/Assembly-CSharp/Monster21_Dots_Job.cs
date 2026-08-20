using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rukhanka;
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
public struct Monster21_Dots_Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Monster21_Dots> __Monster21_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Monster21_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster21_Dots>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Monster21_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster21_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
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
		public void Run(ref Monster21_Dots_Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Monster21_Dots_Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Monster21_Dots_Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Monster21_Dots_Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Monster21_Dots_Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Monster21_Dots_Job job, EntityManager entityManager)
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
	public ComponentLookup<LocalTransform> localTsfLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitpptLookUp;

	[NativeDisableParallelForRestriction]
	public BufferLookup<AnimatorControllerParameterComponent> animaLookUp;

	[NativeDisableParallelForRestriction]
	public EntityStorageInfoLookup entityLookUp;

	[NativeDisableParallelForRestriction]
	public PhysicsWorldSingleton pws;

	public float deltaTime;

	public Vector3 centerPoint;

	public Vector3 roomHW;

	public Vector3 roomHW1;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute(ref Monster21_Dots monster, ref UnitBase_Dots unitBase, Entity entity)
	{
		RefRW<LocalTransform> refRW = localTsfLookUp.GetRefRW(entity);
		RefRW<UnitProperty_Dots> refRW2 = UnitpptLookUp.GetRefRW(entity);
		animaLookUp.TryGetBuffer(unitBase.ett_AnimaRoot, out var bufferData);
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
		for (int i = 0; i < bufferData.Length; i++)
		{
			AnimatorControllerParameterComponent value = bufferData[i];
			value.BoolValue = false;
			bufferData[i] = value;
		}
		switch (monster.state)
		{
		case Monster21State.BornIdle:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value6 = bufferData[0];
				value6.BoolValue = true;
				bufferData[0] = value6;
			}
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			if (monster.stateExistTime >= 0.5f)
			{
				monster.state = Monster21State.MoveToTarget;
			}
			break;
		case Monster21State.Idle:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value3 = bufferData[0];
				value3.BoolValue = true;
				bufferData[0] = value3;
			}
			unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
			break;
		case Monster21State.RandomMove:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value5 = bufferData[1];
				value5.BoolValue = true;
				bufferData[1] = value5;
				monster.maxAngleDuration.RandomResult(ref gRandom.ValueRW.random);
				monster.angleToLeft = DTool.RandomValue(ref gRandom.ValueRW.random) > 0.5f;
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW.ValueRO.Position, 20f, UnitType.Monster, UnitpptLookUp, entityLookUp, pws);
				monster.blinkIntervalTimer = 0f;
				monster.randomMoveTrackPoint = new Vector3((float)DTool.Random(ref gRandom.ValueRW.random, -1, 1) * roomHW.x, (float)DTool.Random(ref gRandom.ValueRW.random, -1, 1) * roomHW.y, 0f) + centerPoint;
			}
			unitBase.checkTargetIntervalTimer += deltaTime;
			if (unitBase.checkTargetIntervalTimer >= 1f)
			{
				unitBase.checkTargetIntervalTimer = 0f;
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW.ValueRO.Position, 20f, UnitType.Monster, UnitpptLookUp, entityLookUp, pws);
				if (unitBase.targetEtt != Entity.Null)
				{
					monster.state = Monster21State.MoveToTarget;
				}
			}
			if (Tool2D.IgnoreZDistanceSqr(refRW.ValueRO.Position, monster.randomMoveTrackPoint) < 2f)
			{
				monster.randomMoveTrackPoint = new Vector3((float)DTool.Random(ref gRandom.ValueRW.random, -1, 1) * roomHW.x, (float)DTool.Random(ref gRandom.ValueRW.random, -1, 1) * roomHW.y, 0f) + centerPoint;
			}
			if (monster.angleToLeft)
			{
				monster.angleCounter -= monster.moveAngleOffsetSpeed * deltaTime;
				if (monster.angleCounter < 0f - monster.moveAngleOffset)
				{
					monster.angleCounter = 0f - monster.moveAngleOffset;
				}
				if (monster.angleCounter == 0f - monster.moveAngleOffset)
				{
					monster.maxAngleDurationTimer += deltaTime;
					if (monster.maxAngleDurationTimer >= monster.maxAngleDuration.result)
					{
						monster.maxAngleDurationTimer = 0f;
						monster.angleToLeft = false;
					}
				}
			}
			else
			{
				monster.angleCounter += monster.moveAngleOffsetSpeed * deltaTime;
				if (monster.angleCounter > monster.moveAngleOffset)
				{
					monster.angleCounter = monster.moveAngleOffset;
				}
				if (monster.angleCounter == monster.moveAngleOffset)
				{
					monster.maxAngleDurationTimer += deltaTime;
					if (monster.maxAngleDurationTimer >= monster.maxAngleDuration.result)
					{
						monster.maxAngleDurationTimer = 0f;
						monster.angleToLeft = true;
					}
				}
			}
			unitBase.SetMove(Tool2D.GetDir(monster.randomMoveTrackPoint, monster.angleCounter).normalized * refRW2.ValueRO.unitCfg.moveSpeed);
			unitBase.SetFlip((monster.randomMoveTrackPoint - refRW.ValueRO.Position).x < 0f);
			break;
		case Monster21State.MoveToTarget:
			if (monster.changedState)
			{
				AnimatorControllerParameterComponent value4 = bufferData[1];
				value4.BoolValue = true;
				bufferData[1] = value4;
				monster.maxAngleDuration.RandomResult(ref gRandom.ValueRW.random);
				monster.angleToLeft = DTool.RandomValue(ref gRandom.ValueRW.random) > 0.5f;
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW.ValueRO.Position, 20f, UnitType.Monster, UnitpptLookUp, entityLookUp, pws);
				monster.blinkIntervalTimer = 0f;
			}
			if (unitBase.targetEtt == Entity.Null || entityLookUp.Exists(unitBase.targetEtt) || localTsfLookUp.HasComponent(unitBase.targetEtt))
			{
				unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW.ValueRO.Position, 20f, UnitType.Monster, UnitpptLookUp, entityLookUp, pws);
			}
			if (unitBase.targetEtt == Entity.Null)
			{
				monster.state = Monster21State.RandomMove;
				break;
			}
			if (monster.angleToLeft)
			{
				monster.angleCounter -= monster.moveAngleOffsetSpeed * deltaTime;
				if (monster.angleCounter < 0f - monster.moveAngleOffset)
				{
					monster.angleCounter = 0f - monster.moveAngleOffset;
				}
				if (monster.angleCounter == 0f - monster.moveAngleOffset)
				{
					monster.maxAngleDurationTimer += deltaTime;
					if (monster.maxAngleDurationTimer >= monster.maxAngleDuration.result)
					{
						monster.maxAngleDurationTimer = 0f;
						monster.angleToLeft = false;
					}
				}
			}
			else
			{
				monster.angleCounter += monster.moveAngleOffsetSpeed * deltaTime;
				if (monster.angleCounter > monster.moveAngleOffset)
				{
					monster.angleCounter = monster.moveAngleOffset;
				}
				if (monster.angleCounter == monster.moveAngleOffset)
				{
					monster.maxAngleDurationTimer += deltaTime;
					if (monster.maxAngleDurationTimer >= monster.maxAngleDuration.result)
					{
						monster.maxAngleDurationTimer = 0f;
						monster.angleToLeft = true;
					}
				}
			}
			if (Tool2D.IgnoreZDistanceSqr(localTsfLookUp[unitBase.targetEtt].Position, refRW.ValueRO.Position) > 0.040000003f)
			{
				unitBase.SetMove(Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(localTsfLookUp[unitBase.targetEtt].Position, refRW.ValueRO.Position), (float)((!monster.angleToLeft) ? 1 : (-1)) * monster.moveAngleOffset) * refRW2.ValueRO.unitCfg.moveSpeed);
			}
			else
			{
				unitBase.SetMove(Vector3.zero, thisTimeShouldFlip: false);
			}
			unitBase.SetFlip((localTsfLookUp[unitBase.targetEtt].Position - refRW.ValueRO.Position).x < 0f);
			if (monster.pattern == AIPattern.Pattern2 || monster.pattern == AIPattern.Pattern4)
			{
				monster.blinkIntervalTimer += deltaTime;
				if (monster.blinkIntervalTimer >= monster.blinkInterval.result)
				{
					monster.blinkInterval.RandomResult(ref gRandom.ValueRW.random);
					monster.state = Monster21State.Blink;
				}
			}
			break;
		case Monster21State.Blink:
			if (monster.changedState)
			{
				if (unitBase.targetEtt == Entity.Null || entityLookUp.Exists(unitBase.targetEtt) || localTsfLookUp.HasComponent(unitBase.targetEtt))
				{
					unitBase.targetEtt = DTool.GetNearestTargetEtt(refRW.ValueRO.Position, 20f, UnitType.Monster, UnitpptLookUp, entityLookUp, pws);
				}
				if (unitBase.targetEtt == Entity.Null)
				{
					monster.state = Monster21State.MoveToTarget;
					break;
				}
				AnimatorControllerParameterComponent value2 = bufferData[2];
				value2.BoolValue = true;
				bufferData[2] = value2;
				Vector3 vector = localTsfLookUp[unitBase.targetEtt].Position;
				monster.blinkPoint = vector + Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(refRW.ValueRO.Position, vector), DTool.Random(ref gRandom.ValueRW.random, -0.5f, 0.5f) * monster.blinkToPlayerBackAngle) * Tool2D.IgnoreZDistance(vector, refRW.ValueRO.Position);
				monster.blinkPoint = new Vector3(Mathf.Clamp(monster.blinkPoint.x, centerPoint.x - roomHW1.x, centerPoint.x + roomHW1.x), Mathf.Clamp(monster.blinkPoint.y, centerPoint.y - roomHW1.y, centerPoint.x + roomHW1.y), 0f);
			}
			unitBase.SetMove(Vector3.zero, thisTimeShouldFlip: false);
			break;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster21_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Monster21_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster21_Dots>(nativeArrayPtr, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(ref monster, ref unitBase, entity);
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
					ref Monster21_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster21_Dots>(nativeArrayPtr, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(ref monster2, ref unitBase2, entity2);
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
				ref Monster21_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster21_Dots>(nativeArrayPtr, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(ref monster3, ref unitBase3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Monster21_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster21_Dots>(nativeArrayPtr, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(ref monster4, ref unitBase4, entity4);
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
