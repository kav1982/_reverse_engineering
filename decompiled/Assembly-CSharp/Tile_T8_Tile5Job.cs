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

[CompilerGenerated]
[BurstCompile]
public struct Tile_T8_Tile5Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public ComponentTypeHandle<Tile_T8_Tile5_Dots> __Tile_T8_Tile5_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TileBase_Dots> __TileBase_Dots_RO_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Tile_T8_Tile5_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tile_T8_Tile5_Dots>(isReadOnly: true);
				__TileBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TileBase_Dots>(isReadOnly: true);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Tile_T8_Tile5_Dots_RO_ComponentTypeHandle.Update(ref state);
				__TileBase_Dots_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Tile_T8_Tile5_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TileBase_Dots>();
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
		public void Run(ref Tile_T8_Tile5Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Tile_T8_Tile5Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Tile_T8_Tile5Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Tile_T8_Tile5Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Tile_T8_Tile5Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Tile_T8_Tile5Job job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter ecb;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, in Tile_T8_Tile5_Dots tile, in TileBase_Dots tileBase, ref LocalTransform localTsf, Entity ett)
	{
		localTsf.Position = tileBase.roomPosition + tileBase.selfPosition.GetFloat3();
		Vector2Data offset = new Vector2Data(0f, 1f);
		bool flag = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(1f, 1f);
		bool flag2 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(1f, 0f);
		bool flag3 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(1f, -1f);
		bool flag4 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(0f, -1f);
		bool flag5 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(-1f, -1f);
		bool flag6 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(-1f, 0f);
		bool flag7 = DTool.TileCheck(in tileBase, in offset);
		offset = new Vector2Data(-1f, 1f);
		bool flag8 = DTool.TileCheck(in tileBase, in offset);
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		bool flag14 = false;
		bool flag15 = false;
		bool flag16 = false;
		bool flag17 = false;
		bool flag18 = false;
		bool flag19 = false;
		bool flag20 = false;
		bool flag21 = false;
		if (flag && flag3 && !flag5 && !flag7)
		{
			flag16 = true;
			if (!flag2)
			{
				flag18 = true;
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			flag14 = true;
			if (!flag4)
			{
				flag19 = true;
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			flag9 = true;
			if (!flag6)
			{
				flag20 = true;
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			flag12 = true;
			if (!flag8)
			{
				flag21 = true;
			}
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			flag13 = true;
			if (!flag8)
			{
				flag21 = true;
			}
			if (!flag2)
			{
				flag18 = true;
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			flag17 = true;
			if (!flag2)
			{
				flag18 = true;
			}
			if (!flag4)
			{
				flag19 = true;
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			flag15 = true;
			if (!flag4)
			{
				flag19 = true;
			}
			if (!flag6)
			{
				flag20 = true;
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			flag10 = true;
			if (!flag6)
			{
				flag20 = true;
			}
			if (!flag8)
			{
				flag21 = true;
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			flag11 = true;
			if (!flag2)
			{
				flag18 = true;
			}
			if (!flag4)
			{
				flag19 = true;
			}
			if (!flag6)
			{
				flag20 = true;
			}
			if (!flag8)
			{
				flag21 = true;
			}
		}
		if (!flag9)
		{
			ecb.DestroyEntity(index, tile.ett_DL);
		}
		if (!flag10)
		{
			ecb.DestroyEntity(index, tile.ett_DLU);
		}
		if (!flag11)
		{
			ecb.DestroyEntity(index, tile.ett_Full);
		}
		if (!flag12)
		{
			ecb.DestroyEntity(index, tile.ett_LU);
		}
		if (!flag13)
		{
			ecb.DestroyEntity(index, tile.ett_LUR);
		}
		if (!flag14)
		{
			ecb.DestroyEntity(index, tile.ett_RD);
		}
		if (!flag15)
		{
			ecb.DestroyEntity(index, tile.ett_RDL);
		}
		if (!flag16)
		{
			ecb.DestroyEntity(index, tile.ett_UR);
		}
		if (!flag17)
		{
			ecb.DestroyEntity(index, tile.ett_URD);
		}
		if (!flag18)
		{
			ecb.DestroyEntity(index, tile.ett_CornerUR);
		}
		if (!flag19)
		{
			ecb.DestroyEntity(index, tile.ett_CornerRD);
		}
		if (!flag20)
		{
			ecb.DestroyEntity(index, tile.ett_CornerDL);
		}
		if (!flag21)
		{
			ecb.DestroyEntity(index, tile.ett_CornerLU);
		}
		ecb.SetComponentEnabled<Tile_T8_Tile5_Dots>(index, ett, value: false);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Tile_T8_Tile5_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TileBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Tile_T8_Tile5_Dots tile = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile5_Dots>(nativeArrayPtr, i);
				ref TileBase_Dots tileBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, i);
				ref LocalTransform localTsf = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
				Execute(chunkIndexInQuery, in tile, in tileBase, ref localTsf, ett);
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
					ref Tile_T8_Tile5_Dots tile2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile5_Dots>(nativeArrayPtr, nextRangeBegin);
					ref TileBase_Dots tileBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform localTsf2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
					Execute(chunkIndexInQuery, in tile2, in tileBase2, ref localTsf2, ett2);
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
				ref Tile_T8_Tile5_Dots tile3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile5_Dots>(nativeArrayPtr, j);
				ref TileBase_Dots tileBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, j);
				ref LocalTransform localTsf3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
				Execute(chunkIndexInQuery, in tile3, in tileBase3, ref localTsf3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Tile_T8_Tile5_Dots tile4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile5_Dots>(nativeArrayPtr, k);
				ref TileBase_Dots tileBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, k);
				ref LocalTransform localTsf4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
				Execute(chunkIndexInQuery, in tile4, in tileBase4, ref localTsf4, ett4);
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
