using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rukhanka;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
[CompilerGenerated]
internal struct AnimaPlayJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<AnimaPlay> __AnimaPlay_RW_ComponentTypeHandle;

			public BufferTypeHandle<AnimatorControllerLayerComponent> __Rukhanka_AnimatorControllerLayerComponent_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__AnimaPlay_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimaPlay>();
				__Rukhanka_AnimatorControllerLayerComponent_RW_BufferTypeHandle = state.GetBufferTypeHandle<AnimatorControllerLayerComponent>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__AnimaPlay_RW_ComponentTypeHandle.Update(ref state);
				__Rukhanka_AnimatorControllerLayerComponent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<AnimaPlay>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<AnimatorControllerLayerComponent>();
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
		public void Run(ref AnimaPlayJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref AnimaPlayJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref AnimaPlayJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref AnimaPlayJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref AnimaPlayJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref AnimaPlayJob job, EntityManager entityManager)
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
	public BufferLookup<AnimatorControllerParameterComponent> paraBufferLookUp;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute(ref AnimaPlay anima, DynamicBuffer<AnimatorControllerLayerComponent> layer, Entity entity)
	{
		for (int i = 0; i < layer.Length; i++)
		{
			AnimatorControllerLayerComponent value = layer[i];
			value.speed = anima.animaSpeed;
			layer[i] = value;
		}
		if (!paraBufferLookUp.TryGetBuffer(entity, out var bufferData))
		{
			return;
		}
		for (int j = 0; j < bufferData.Length; j++)
		{
			AnimatorControllerParameterComponent value2 = bufferData[j];
			value2.BoolValue = false;
			bufferData[j] = value2;
		}
		if (anima.needPlay)
		{
			anima.needPlay = false;
			if (anima.boolIndex < bufferData.Length)
			{
				AnimatorControllerParameterComponent value3 = bufferData[anima.boolIndex];
				value3.BoolValue = true;
				bufferData[anima.boolIndex] = value3;
				anima.boolIndex = -1;
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__AnimaPlay_RW_ComponentTypeHandle);
		BufferAccessor<AnimatorControllerLayerComponent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Rukhanka_AnimatorControllerLayerComponent_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref AnimaPlay anima = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimaPlay>(nativeArrayPtr, i);
				DynamicBuffer<AnimatorControllerLayerComponent> layer = bufferAccessor[i];
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(ref anima, layer, entity);
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
					ref AnimaPlay anima2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimaPlay>(nativeArrayPtr, nextRangeBegin);
					DynamicBuffer<AnimatorControllerLayerComponent> layer2 = bufferAccessor[nextRangeBegin];
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(ref anima2, layer2, entity2);
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
				ref AnimaPlay anima3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimaPlay>(nativeArrayPtr, j);
				DynamicBuffer<AnimatorControllerLayerComponent> layer3 = bufferAccessor[j];
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(ref anima3, layer3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref AnimaPlay anima4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimaPlay>(nativeArrayPtr, k);
				DynamicBuffer<AnimatorControllerLayerComponent> layer4 = bufferAccessor[k];
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(ref anima4, layer4, entity4);
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
