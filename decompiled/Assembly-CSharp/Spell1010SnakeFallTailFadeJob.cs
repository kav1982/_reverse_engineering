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

[BurstCompile]
[CompilerGenerated]
public struct Spell1010SnakeFallTailFadeJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public BufferTypeHandle<SnakeBodyPoint> __SnakeBodyPoint_RW_BufferTypeHandle;

			public BufferTypeHandle<SnakeTouchGroundPoint> __SnakeTouchGroundPoint_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1010SnakeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SnakeBodyPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeBodyPoint>();
				__SnakeTouchGroundPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeTouchGroundPoint>();
			}

			public void Update(ref SystemState state)
			{
				__Spell1010SnakeData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SnakeBodyPoint_RW_BufferTypeHandle.Update(ref state);
				__SnakeTouchGroundPoint_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1010SnakeData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeBodyPoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeTouchGroundPoint>();
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
		public void Run(ref Spell1010SnakeFallTailFadeJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1010SnakeFallTailFadeJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1010SnakeFallTailFadeJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1010SnakeFallTailFadeJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1010SnakeFallTailFadeJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1010SnakeFallTailFadeJob job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	public float DeltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell1010SnakeData data, Entity entity, DynamicBuffer<SnakeBodyPoint> buffer, DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints, [ChunkIndexInQuery] int chunkIndex)
	{
		if (data.IsFadingTail)
		{
			data.OnGroundDisapperDistance += data.OnGroundSpeed * DeltaTime;
			int num = buffer.Length - 1;
			while (num >= 0 && !(data.OnGroundDisapperDistance < buffer[num].distance))
			{
				data.OnGroundDisapperDistance -= buffer[num].distance;
				data.LineLength -= buffer[num].distance;
				buffer.RemoveAt(num);
				num--;
			}
			int num2 = touchGroundPoints.Length - 1;
			while (num2 >= 0 && !(data.LineLength > touchGroundPoints[num2].distanceToHead))
			{
				touchGroundPoints.RemoveAt(num2);
				num2--;
			}
			if (buffer.Length <= 1)
			{
				buffer.Clear();
				touchGroundPoints.Clear();
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1010SnakeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		BufferAccessor<SnakeBodyPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeBodyPoint_RW_BufferTypeHandle);
		BufferAccessor<SnakeTouchGroundPoint> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeTouchGroundPoint_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1010SnakeData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				DynamicBuffer<SnakeBodyPoint> buffer = bufferAccessor[i];
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints = bufferAccessor2[i];
				Execute(ref data, entity, buffer, touchGroundPoints, chunkIndexInQuery);
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
					ref Spell1010SnakeData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					DynamicBuffer<SnakeBodyPoint> buffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints2 = bufferAccessor2[nextRangeBegin];
					Execute(ref data2, entity2, buffer2, touchGroundPoints2, chunkIndexInQuery);
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
				ref Spell1010SnakeData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				DynamicBuffer<SnakeBodyPoint> buffer3 = bufferAccessor[j];
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints3 = bufferAccessor2[j];
				Execute(ref data3, entity3, buffer3, touchGroundPoints3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1010SnakeData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				DynamicBuffer<SnakeBodyPoint> buffer4 = bufferAccessor[k];
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints4 = bufferAccessor2[k];
				Execute(ref data4, entity4, buffer4, touchGroundPoints4, chunkIndexInQuery);
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
