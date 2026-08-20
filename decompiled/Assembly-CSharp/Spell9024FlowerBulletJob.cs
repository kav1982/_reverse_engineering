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
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Spell9024FlowerBulletJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell9024RotateOutData> __Spell9024RotateOutData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell9024RotateOutData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell9024RotateOutData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
			}

			public void Update(ref SystemState state)
			{
				__Spell9024RotateOutData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell9024RotateOutData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
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
		public void Run(ref Spell9024FlowerBulletJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell9024FlowerBulletJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell9024FlowerBulletJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell9024FlowerBulletJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell9024FlowerBulletJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell9024FlowerBulletJob job, EntityManager entityManager)
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

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell9024RotateOutData data, ref SpellMovementComponentData movement, in SpellConfigComponentData config, ref LocalTransform transform)
	{
		if (!data.Initialized)
		{
			data.Initialized = true;
			data.initialPoint = transform.Position;
			data.rotateSpeedRatio = config.Float1;
			data.straightSpeedRatio = config.Float2;
		}
		float3 x = transform.Position - data.initialPoint;
		x.z = 0f;
		x = math.normalizesafe(x, movement.Direction);
		float3 @float = math.rotate(quaternion.RotateZ(-90f), x);
		movement.Direction = math.normalizesafe(x * movement.Speed * data.straightSpeedRatio + @float * data.rotateSpeedRatio * movement.Speed, movement.Direction);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell9024RotateOutData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9024RotateOutData>(nativeArrayPtr, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i));
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
					Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9024RotateOutData>(nativeArrayPtr, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin));
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
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9024RotateOutData>(nativeArrayPtr, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9024RotateOutData>(nativeArrayPtr, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k));
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
