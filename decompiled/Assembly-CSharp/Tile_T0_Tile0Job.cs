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

[BurstCompile]
[CompilerGenerated]
public struct Tile_T0_Tile0Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public ComponentTypeHandle<Tile_T0_Tile0_Dots> __Tile_T0_Tile0_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TileBase_Dots> __TileBase_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public BufferTypeHandle<EntityBED1> __EntityBED1_RO_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Tile_T0_Tile0_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tile_T0_Tile0_Dots>(isReadOnly: true);
				__TileBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TileBase_Dots>(isReadOnly: true);
				__EntityBED1_RO_BufferTypeHandle = state.GetBufferTypeHandle<EntityBED1>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Tile_T0_Tile0_Dots_RO_ComponentTypeHandle.Update(ref state);
				__TileBase_Dots_RO_ComponentTypeHandle.Update(ref state);
				__EntityBED1_RO_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Tile_T0_Tile0_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TileBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<EntityBED1>();
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
		public void Run(ref Tile_T0_Tile0Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Tile_T0_Tile0Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Tile_T0_Tile0Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Tile_T0_Tile0Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Tile_T0_Tile0Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Tile_T0_Tile0Job job, EntityManager entityManager)
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

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	public EntityCommandBuffer.ParallelWriter ecb;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute(in Tile_T0_Tile0_Dots tile, in TileBase_Dots tileBase, in DynamicBuffer<EntityBED1> buffer, [ChunkIndexInQuery] int chunkIndex, Entity ett)
	{
		cluLocalTsf.GetRefRW(ett).ValueRW.Position = tileBase.roomPosition + tileBase.selfPosition.GetFloat3();
		if (DTool.RandomValue(ref gRandom.ValueRW.random) <= tile.variationChance)
		{
			ecb.DestroyEntity(chunkIndex, tile.ett_Tile0Base);
			int num = DTool.Random(ref gRandom.ValueRW.random, 0, buffer.Length);
			for (int i = 0; i < buffer.Length; i++)
			{
				if (i != num)
				{
					ecb.DestroyEntity(chunkIndex, buffer[i].ett);
				}
			}
		}
		else
		{
			for (int j = 0; j < buffer.Length; j++)
			{
				ecb.DestroyEntity(chunkIndex, buffer[j].ett);
			}
		}
		if (tileBase.selfPosition.x % (float)tile.tile1CellWidth == 0f && tileBase.selfPosition.y % (float)tile.tile1CellWidth == 0f && DTool.RandomValue(ref gRandom.ValueRW.random) <= tile.tile1Chance)
		{
			RefRW<LocalTransform> refRW = cluLocalTsf.GetRefRW(tile.ett_Tile1);
			refRW.ValueRW.Scale = tile.tile1Scale;
			refRW.ValueRW.Position += new float3(DTool.Random(ref gRandom.ValueRW.random, -1, 2), DTool.Random(ref gRandom.ValueRW.random, -1, 2), 0f) + tile.tile1Offset;
		}
		else
		{
			ecb.DestroyEntity(chunkIndex, tile.ett_Tile1);
		}
		ecb.SetComponentEnabled<Tile_T0_Tile0_Dots>(chunkIndex, ett, value: false);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Tile_T0_Tile0_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TileBase_Dots_RO_ComponentTypeHandle);
		BufferAccessor<EntityBED1> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__EntityBED1_RO_BufferTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Tile_T0_Tile0_Dots tile = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T0_Tile0_Dots>(nativeArrayPtr, i);
				ref TileBase_Dots tileBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, i);
				DynamicBuffer<EntityBED1> buffer = bufferAccessor[i];
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(in tile, in tileBase, in buffer, chunkIndexInQuery, ett);
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
					ref Tile_T0_Tile0_Dots tile2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T0_Tile0_Dots>(nativeArrayPtr, nextRangeBegin);
					ref TileBase_Dots tileBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					DynamicBuffer<EntityBED1> buffer2 = bufferAccessor[nextRangeBegin];
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(in tile2, in tileBase2, in buffer2, chunkIndexInQuery, ett2);
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
				ref Tile_T0_Tile0_Dots tile3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T0_Tile0_Dots>(nativeArrayPtr, j);
				ref TileBase_Dots tileBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, j);
				DynamicBuffer<EntityBED1> buffer3 = bufferAccessor[j];
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(in tile3, in tileBase3, in buffer3, chunkIndexInQuery, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Tile_T0_Tile0_Dots tile4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T0_Tile0_Dots>(nativeArrayPtr, k);
				ref TileBase_Dots tileBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, k);
				DynamicBuffer<EntityBED1> buffer4 = bufferAccessor[k];
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(in tile4, in tileBase4, in buffer4, chunkIndexInQuery, ett4);
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
