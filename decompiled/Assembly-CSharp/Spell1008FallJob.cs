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

[CompilerGenerated]
[BurstCompile]
public struct Spell1008FallJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1008ReadyToDeathTag> __Spell1008ReadyToDeathTag_RW_ComponentTypeHandle;

			public BufferTypeHandle<Spell1008HitExplosionEffectData> __Spell1008HitExplosionEffectData_RW_BufferTypeHandle;

			public ComponentTypeHandle<Spell1008ArcaneExplosionData> __Spell1008ArcaneExplosionData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1008ReadyToDeathTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1008ReadyToDeathTag>();
				__Spell1008HitExplosionEffectData_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell1008HitExplosionEffectData>();
				__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1008ArcaneExplosionData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell1008ReadyToDeathTag_RW_ComponentTypeHandle.Update(ref state);
				__Spell1008HitExplosionEffectData_RW_BufferTypeHandle.Update(ref state);
				__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1008ReadyToDeathTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008HitExplosionEffectData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008ArcaneExplosionData>();
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
		public void Run(ref Spell1008FallJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1008FallJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1008FallJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1008FallJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1008FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1008FallJob job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell1008FallData> fallDataLookup;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(Spell1008ReadyToDeathTag _, DynamicBuffer<Spell1008HitExplosionEffectData> buffers, [ChunkIndexInQuery] int chunkIndex, ref Spell1008ArcaneExplosionData data, Entity e)
	{
		bool flag = true;
		foreach (Spell1008HitExplosionEffectData item in buffers)
		{
			if (fallDataLookup.HasComponent(item.EffectEntity) && !fallDataLookup[item.EffectEntity].FinishDamageApply)
			{
				flag = false;
			}
		}
		if (flag)
		{
			CMD.SetComponentEnabled<Spell1008ReadyToDeathTag>(chunkIndex, e, value: false);
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, e, value: true);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		BufferAccessor<Spell1008HitExplosionEffectData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1008HitExplosionEffectData_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				DynamicBuffer<Spell1008HitExplosionEffectData> buffers = bufferAccessor[i];
				ref Spell1008ArcaneExplosionData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, i);
				Entity e = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(default(Spell1008ReadyToDeathTag), buffers, chunkIndexInQuery, ref data, e);
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
					DynamicBuffer<Spell1008HitExplosionEffectData> buffers2 = bufferAccessor[nextRangeBegin];
					ref Spell1008ArcaneExplosionData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, nextRangeBegin);
					Entity e2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(default(Spell1008ReadyToDeathTag), buffers2, chunkIndexInQuery, ref data2, e2);
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
				DynamicBuffer<Spell1008HitExplosionEffectData> buffers3 = bufferAccessor[j];
				ref Spell1008ArcaneExplosionData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, j);
				Entity e3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(default(Spell1008ReadyToDeathTag), buffers3, chunkIndexInQuery, ref data3, e3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				DynamicBuffer<Spell1008HitExplosionEffectData> buffers4 = bufferAccessor[k];
				ref Spell1008ArcaneExplosionData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, k);
				Entity e4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(default(Spell1008ReadyToDeathTag), buffers4, chunkIndexInQuery, ref data4, e4);
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
