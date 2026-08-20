using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[CompilerGenerated]
public struct Destructible1_T3Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Destructible1_T3_Dots> __Destructible1_T3_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentTypeHandle;

			public BufferTypeHandle<EntityBED1> __EntityBED1_RW_BufferTypeHandle;

			public BufferTypeHandle<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Destructible1_T3_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Destructible1_T3_Dots>();
				__UnitProperty_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>(isReadOnly: true);
				__EntityBED1_RW_BufferTypeHandle = state.GetBufferTypeHandle<EntityBED1>();
				__TakeDamageInfo_Dots_RW_BufferTypeHandle = state.GetBufferTypeHandle<TakeDamageInfo_Dots>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Destructible1_T3_Dots_RW_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RO_ComponentTypeHandle.Update(ref state);
				__EntityBED1_RW_BufferTypeHandle.Update(ref state);
				__TakeDamageInfo_Dots_RW_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Destructible1_T3_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EntityBED1>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TakeDamageInfo_Dots>();
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
		public void Run(ref Destructible1_T3Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Destructible1_T3Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Destructible1_T3Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Destructible1_T3Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Destructible1_T3Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Destructible1_T3Job job, EntityManager entityManager)
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
	public ComponentLookup<LocalTransform> cluLocalTsf;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<AnimaPlay> cluAnimaPlay;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	[ReadOnly]
	public NativeHashMap<FixedString128Bytes, Entity> allMixedEttList;

	public EntityCommandBuffer.ParallelWriter ecb;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, ref Destructible1_T3_Dots d1, in UnitProperty_Dots unitPpt, DynamicBuffer<EntityBED1> selfMRBuffer, DynamicBuffer<TakeDamageInfo_Dots> damageInfoBuffer, Entity ett)
	{
		if (!d1.isInitialized)
		{
			d1.isInitialized = true;
			int num = DTool.Random(ref gRandom.ValueRW.random, 0, selfMRBuffer.Length);
			for (int i = 0; i < selfMRBuffer.Length; i++)
			{
				if (i != num)
				{
					cluLocalTsf.GetRefRW(selfMRBuffer[i].ett).ValueRW.Scale = 0f;
				}
			}
			d1.rewardItemInfo = OutputMgr_Dots.GetRewardD1_T3(ref gRandom.ValueRW.random);
		}
		if (damageInfoBuffer.Length > 0)
		{
			cluAnimaPlay.GetRefRW(d1.ett_Anima).ValueRW.Play(0);
		}
		if (unitPpt.isDead && !d1.alreadyDropReward)
		{
			d1.alreadyDropReward = true;
			if (d1.rewardItemInfo.id != 0)
			{
				Entity e = ecb.Instantiate(index, allMixedEttList["Item"]);
				ecb.SetComponent(index, e, new LocalTransform
				{
					Position = cluLocalTsf.GetRefRO(ett).ValueRO.Position,
					Rotation = quaternion.identity,
					Scale = 1f
				});
				ecb.SetComponent(index, e, new Item
				{
					belongRoomMapPos = LevelMgr.Inst.CurrentRoomMapPos,
					info = d1.rewardItemInfo
				});
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Destructible1_T3_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RO_ComponentTypeHandle);
		BufferAccessor<EntityBED1> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__EntityBED1_RW_BufferTypeHandle);
		BufferAccessor<TakeDamageInfo_Dots> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Destructible1_T3_Dots d = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Destructible1_T3_Dots>(nativeArrayPtr, i);
				ref UnitProperty_Dots unitPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, i);
				DynamicBuffer<EntityBED1> selfMRBuffer = bufferAccessor[i];
				DynamicBuffer<TakeDamageInfo_Dots> damageInfoBuffer = bufferAccessor2[i];
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, ref d, in unitPpt, selfMRBuffer, damageInfoBuffer, ett);
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
					ref Destructible1_T3_Dots d2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Destructible1_T3_Dots>(nativeArrayPtr, nextRangeBegin);
					ref UnitProperty_Dots unitPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, nextRangeBegin);
					DynamicBuffer<EntityBED1> selfMRBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<TakeDamageInfo_Dots> damageInfoBuffer2 = bufferAccessor2[nextRangeBegin];
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, ref d2, in unitPpt2, selfMRBuffer2, damageInfoBuffer2, ett2);
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
				ref Destructible1_T3_Dots d3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Destructible1_T3_Dots>(nativeArrayPtr, j);
				ref UnitProperty_Dots unitPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, j);
				DynamicBuffer<EntityBED1> selfMRBuffer3 = bufferAccessor[j];
				DynamicBuffer<TakeDamageInfo_Dots> damageInfoBuffer3 = bufferAccessor2[j];
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, ref d3, in unitPpt3, selfMRBuffer3, damageInfoBuffer3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Destructible1_T3_Dots d4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Destructible1_T3_Dots>(nativeArrayPtr, k);
				ref UnitProperty_Dots unitPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, k);
				DynamicBuffer<EntityBED1> selfMRBuffer4 = bufferAccessor[k];
				DynamicBuffer<TakeDamageInfo_Dots> damageInfoBuffer4 = bufferAccessor2[k];
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, ref d4, in unitPpt4, selfMRBuffer4, damageInfoBuffer4, ett4);
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
