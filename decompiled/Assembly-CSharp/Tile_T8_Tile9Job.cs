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
public struct Tile_T8_Tile9Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public ComponentTypeHandle<Tile_T8_Tile9_Dots> __Tile_T8_Tile9_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TileBase_Dots> __TileBase_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Tile_T8_Tile9_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tile_T8_Tile9_Dots>(isReadOnly: true);
				__TileBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TileBase_Dots>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Tile_T8_Tile9_Dots_RO_ComponentTypeHandle.Update(ref state);
				__TileBase_Dots_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Tile_T8_Tile9_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TileBase_Dots>();
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
		public void Run(ref Tile_T8_Tile9Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Tile_T8_Tile9Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Tile_T8_Tile9Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Tile_T8_Tile9Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Tile_T8_Tile9Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Tile_T8_Tile9Job job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter ecb;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, in Tile_T8_Tile9_Dots tile, in TileBase_Dots tileBase, Entity ett)
	{
		RefRW<LocalTransform> refRW = cluLocalTsf.GetRefRW(ett);
		refRW.ValueRW.Position = tileBase.roomPosition + tileBase.selfPosition.GetFloat3();
		RefRW<LocalTransform> refRW2 = cluLocalTsf.GetRefRW(tile.ett_LayerBase);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.BoundaryLow);
		refRW2 = cluLocalTsf.GetRefRW(tile.ett_LayerWall);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Coordinate);
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
		bool flag22 = false;
		bool flag23 = false;
		bool flag24 = false;
		bool flag25 = false;
		if (!flag && !flag3 && !flag5 && !flag7)
		{
			flag12 = true;
		}
		else if (flag && !flag3 && !flag5 && !flag7)
		{
			flag16 = true;
		}
		else if (!flag && flag3 && !flag5 && !flag7)
		{
			flag13 = true;
		}
		else if (!flag && !flag3 && flag5 && !flag7)
		{
			flag9 = true;
		}
		else if (!flag && !flag3 && !flag5 && flag7)
		{
			flag13 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (!flag && flag3 && !flag5 && flag7)
		{
			flag10 = true;
		}
		else if (flag && !flag3 && flag5 && !flag7)
		{
			flag17 = true;
		}
		else if (flag && flag3 && !flag5 && !flag7)
		{
			flag18 = true;
			if (!flag2)
			{
				flag20 = true;
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			flag14 = true;
			if (!flag4)
			{
				flag21 = true;
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			flag14 = true;
			if (!flag6)
			{
				flag21 = true;
			}
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			flag18 = true;
			if (!flag8)
			{
				flag20 = true;
			}
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			flag11 = true;
			if (!flag8)
			{
				flag23 = true;
			}
			if (!flag2)
			{
				flag20 = true;
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			flag19 = true;
			if (!flag2)
			{
				flag20 = true;
			}
			if (!flag4)
			{
				flag21 = true;
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			flag15 = true;
			if (!flag4)
			{
				flag21 = true;
			}
			if (!flag6)
			{
				flag22 = true;
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			flag19 = true;
			if (!flag6)
			{
				flag21 = true;
			}
			if (!flag8)
			{
				flag20 = true;
			}
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (flag2 && flag4 && flag6 && flag8)
			{
				flag25 = true;
				refRW2 = cluLocalTsf.GetRefRW(tile.ett_LayerWall);
				refRW2.ValueRW.Position += new float3(0f, 0f, -0.011f);
			}
			else
			{
				flag24 = true;
			}
			if (!flag2)
			{
				flag20 = true;
			}
			if (!flag4)
			{
				flag21 = true;
			}
			if (!flag6)
			{
				flag22 = true;
			}
			if (!flag8)
			{
				flag23 = true;
			}
		}
		if (!flag9)
		{
			ecb.DestroyEntity(index, tile.ett_Base_D);
			ecb.DestroyEntity(index, tile.ett_Wall_D);
		}
		if (!flag10)
		{
			ecb.DestroyEntity(index, tile.ett_Base_LR);
			ecb.DestroyEntity(index, tile.ett_Wall_LR);
		}
		if (!flag11)
		{
			ecb.DestroyEntity(index, tile.ett_Base_LUR);
			ecb.DestroyEntity(index, tile.ett_Wall_LUR);
		}
		if (!flag12)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Null);
		}
		if (!flag13)
		{
			ecb.DestroyEntity(index, tile.ett_Base_R);
			ecb.DestroyEntity(index, tile.ett_Wall_R);
		}
		if (!flag14)
		{
			ecb.DestroyEntity(index, tile.ett_Base_RD);
			ecb.DestroyEntity(index, tile.ett_Wall_RD);
		}
		if (!flag15)
		{
			ecb.DestroyEntity(index, tile.ett_Base_RDL);
			ecb.DestroyEntity(index, tile.ett_Wall_RDL);
		}
		if (!flag16)
		{
			ecb.DestroyEntity(index, tile.ett_Base_U);
			ecb.DestroyEntity(index, tile.ett_Wall_U);
		}
		if (!flag17)
		{
			ecb.DestroyEntity(index, tile.ett_Base_UD);
			ecb.DestroyEntity(index, tile.ett_Wall_UD);
		}
		if (!flag18)
		{
			ecb.DestroyEntity(index, tile.ett_Base_UR);
			ecb.DestroyEntity(index, tile.ett_Wall_UR);
		}
		if (!flag19)
		{
			ecb.DestroyEntity(index, tile.ett_Base_URD);
			ecb.DestroyEntity(index, tile.ett_Wall_URD);
		}
		if (!flag20)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Corner_UR);
		}
		if (!flag21)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Corner_RD);
		}
		if (!flag22)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Corner_DL);
		}
		if (!flag23)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Corner_LU);
		}
		if (!flag24)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_Full);
		}
		if (!flag25)
		{
			ecb.DestroyEntity(index, tile.ett_Wall_FullFog);
		}
		ecb.SetComponentEnabled<Tile_T8_Tile9_Dots>(index, ett, value: false);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Tile_T8_Tile9_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TileBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Tile_T8_Tile9_Dots tile = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile9_Dots>(nativeArrayPtr, i);
				ref TileBase_Dots tileBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, in tile, in tileBase, ett);
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
					ref Tile_T8_Tile9_Dots tile2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile9_Dots>(nativeArrayPtr, nextRangeBegin);
					ref TileBase_Dots tileBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, in tile2, in tileBase2, ett2);
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
				ref Tile_T8_Tile9_Dots tile3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile9_Dots>(nativeArrayPtr, j);
				ref TileBase_Dots tileBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, in tile3, in tileBase3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Tile_T8_Tile9_Dots tile4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Tile_T8_Tile9_Dots>(nativeArrayPtr, k);
				ref TileBase_Dots tileBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TileBase_Dots>(nativeArrayPtr2, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, in tile4, in tileBase4, ett4);
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
