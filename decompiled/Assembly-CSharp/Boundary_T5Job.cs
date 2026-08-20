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
public struct Boundary_T5Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Boundary_T5_Dots> __Boundary_T5_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<BoundaryBase_Dots> __BoundaryBase_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Boundary_T5_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Boundary_T5_Dots>();
				__BoundaryBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BoundaryBase_Dots>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Boundary_T5_Dots_RW_ComponentTypeHandle.Update(ref state);
				__BoundaryBase_Dots_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<BoundaryBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Boundary_T5_Dots>();
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
		public void Run(ref Boundary_T5Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Boundary_T5Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Boundary_T5Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Boundary_T5Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Boundary_T5Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Boundary_T5Job job, EntityManager entityManager)
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

	public void Execute([ChunkIndexInQuery] int index, ref Boundary_T5_Dots boundary, in BoundaryBase_Dots boundaryBase, Entity ett)
	{
		RefRW<LocalTransform> refRW = cluLocalTsf.GetRefRW(ett);
		refRW.ValueRW.Position = boundaryBase.roomPosition + boundaryBase.selfPosition.GetFloat3();
		RefRW<LocalTransform> refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerCliff);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Cliff);
		refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Lava1);
		refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater2);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Lava2);
		float3 layerPosition = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Lava3);
		refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Coordinate);
		refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerTile);
		refRW2.ValueRW.Position = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.Tile0);
		Vector2Data offset = new Vector2Data(0f, 1f);
		bool flag = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(1f, 1f);
		bool flag2 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(1f, 0f);
		bool flag3 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(1f, -1f);
		bool flag4 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(0f, -1f);
		bool flag5 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(-1f, -1f);
		bool flag6 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(-1f, 0f);
		bool flag7 = DTool.BoundaryCheck(in boundaryBase, in offset);
		offset = new Vector2Data(-1f, 1f);
		bool flag8 = DTool.BoundaryCheck(in boundaryBase, in offset);
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
		bool flag26 = false;
		bool flag27 = false;
		bool flag28 = false;
		bool flag29 = false;
		bool flag30 = false;
		if (flag && flag3 && !flag5 && !flag7)
		{
			flag16 = true;
			flag22 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, 0f - boundary.stoneOffset, 0f);
			flag29 = true;
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			flag9 = true;
			flag14 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater);
			refRW2.ValueRW.Position = layerPosition + boundary.waterOffset;
			flag21 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, boundary.stoneOffset, 0f);
			flag27 = true;
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			flag9 = true;
			flag14 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater);
			refRW2.ValueRW.Position = layerPosition + boundary.waterOffset;
			flag21 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, boundary.stoneOffset, 0f);
			flag27 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			flag16 = true;
			flag22 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, 0f - boundary.stoneOffset, 0f);
			flag29 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			flag20 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f, 0f - boundary.stoneOffset, 0f);
			flag26 = true;
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			flag17 = true;
			flag23 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, 0f, 0f);
			flag30 = true;
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			flag9 = true;
			flag15 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater);
			refRW2.ValueRW.Position = layerPosition + boundary.waterOffset;
			flag20 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f, boundary.stoneOffset, 0f);
			flag28 = true;
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			flag17 = true;
			flag23 = true;
			refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
			refRW2.ValueRW.Position += new float3(0f - boundary.stoneOffset, 0f, 0f);
			flag30 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (!flag2)
			{
				flag10 = true;
				flag12 = true;
				flag13 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater2);
				refRW2.ValueRW.Position = layerPosition + boundary.waterOffset;
				flag19 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
				refRW2.ValueRW.Position += new float3(boundary.stoneOffset, boundary.stoneOffset, 0f);
				flag25 = true;
			}
			else if (!flag4)
			{
				flag11 = true;
				flag18 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
				refRW2.ValueRW.Position += new float3(boundary.stoneOffset, 0f - boundary.stoneOffset, -0.0001f);
				flag24 = true;
			}
			else if (!flag6)
			{
				flag11 = true;
				flag18 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
				refRW2.ValueRW.Position += new float3(boundary.stoneOffset, 0f - boundary.stoneOffset, -0.0001f);
				flag24 = true;
				ecb.AddComponent(index, ett, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
			else if (!flag8)
			{
				flag10 = true;
				flag12 = true;
				flag13 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerWater2);
				refRW2.ValueRW.Position = layerPosition + boundary.waterOffset;
				flag19 = true;
				refRW2 = cluLocalTsf.GetRefRW(boundary.ett_LayerStone);
				refRW2.ValueRW.Position += new float3(boundary.stoneOffset, boundary.stoneOffset, 0f);
				flag25 = true;
				ecb.AddComponent(index, ett, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
		}
		if (!flag9)
		{
			ecb.DestroyEntity(index, boundary.ett_Cliff_D);
		}
		if (!flag10)
		{
			ecb.DestroyEntity(index, boundary.ett_Cliff_DCorner);
		}
		if (!flag11)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_Corner_RD);
		}
		if (!flag12)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_Corner_UR);
		}
		if (!flag13)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_Corner_UR2);
		}
		if (!flag14)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_RD);
		}
		if (!flag15)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_RDL);
		}
		if (!flag16)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_UR);
		}
		if (!flag17)
		{
			ecb.DestroyEntity(index, boundary.ett_Lava_URD);
		}
		if (!flag18)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_Corner_RD);
		}
		if (!flag19)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_Corner_UR);
		}
		if (!flag20)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_LUR);
		}
		if (!flag21)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_RD);
		}
		if (!flag22)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_UR);
		}
		if (!flag23)
		{
			ecb.DestroyEntity(index, boundary.ett_Stone_URD);
		}
		if (!flag24)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_Corner_RD);
		}
		if (!flag25)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_Corner_UR);
		}
		if (!flag26)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_LUR);
		}
		if (!flag27)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_RD);
		}
		if (!flag28)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_RDL);
		}
		if (!flag29)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_UR);
		}
		if (!flag30)
		{
			ecb.DestroyEntity(index, boundary.ett_Tile_URD);
		}
		ecb.SetComponentEnabled<Boundary_T5_Dots>(index, ett, value: false);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Boundary_T5_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__BoundaryBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Boundary_T5_Dots boundary = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T5_Dots>(nativeArrayPtr, i);
				ref BoundaryBase_Dots boundaryBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, ref boundary, in boundaryBase, ett);
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
					ref Boundary_T5_Dots boundary2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T5_Dots>(nativeArrayPtr, nextRangeBegin);
					ref BoundaryBase_Dots boundaryBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, ref boundary2, in boundaryBase2, ett2);
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
				ref Boundary_T5_Dots boundary3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T5_Dots>(nativeArrayPtr, j);
				ref BoundaryBase_Dots boundaryBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, ref boundary3, in boundaryBase3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Boundary_T5_Dots boundary4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T5_Dots>(nativeArrayPtr, k);
				ref BoundaryBase_Dots boundaryBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, ref boundary4, in boundaryBase4, ett4);
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
