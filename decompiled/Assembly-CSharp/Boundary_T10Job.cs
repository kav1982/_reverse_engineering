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
public struct Boundary_T10Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public ComponentTypeHandle<Boundary_T10_Dots> __Boundary_T10_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<BoundaryBase_Dots> __BoundaryBase_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Boundary_T10_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Boundary_T10_Dots>(isReadOnly: true);
				__BoundaryBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BoundaryBase_Dots>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Boundary_T10_Dots_RO_ComponentTypeHandle.Update(ref state);
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
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Boundary_T10_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<BoundaryBase_Dots>();
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
		public void Run(ref Boundary_T10Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Boundary_T10Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Boundary_T10Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Boundary_T10Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Boundary_T10Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Boundary_T10Job job, EntityManager entityManager)
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

	public void Execute([ChunkIndexInQuery] int index, in Boundary_T10_Dots boundary, in BoundaryBase_Dots boundaryBase, Entity ett)
	{
		RefRW<LocalTransform> refRW = cluLocalTsf.GetRefRW(ett);
		refRW.ValueRW.Position = boundaryBase.roomPosition + boundaryBase.selfPosition.GetFloat3();
		float3 layerPosition = DTool.GetLayerPosition(in refRW.ValueRW.Position, LayerCorrectType.BoundaryAO);
		cluLocalTsf.GetRefRW(boundary.ett_LayerAO).ValueRW.Position = layerPosition + new float3(0f, 0f, DTool.Random(ref gRandom.ValueRW.random, -0.0049f, 0.0049f));
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
		if (flag && flag3 && !flag5 && !flag7)
		{
			flag19 = true;
			flag12 = true;
			refRW.ValueRW.Position += new float3(0f, 0f, -0.0001f);
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			flag17 = true;
			flag10 = true;
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			flag17 = true;
			flag10 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			flag19 = true;
			flag12 = true;
			refRW.ValueRW.Position += new float3(0f, 0f, -0.0001f);
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			flag16 = true;
			flag9 = true;
			RandomDetail(index, boundary, boundaryBase, boundary.ett_Detail_LUR, flip: false);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			flag20 = true;
			flag13 = true;
			RandomDetail(index, boundary, boundaryBase, boundary.ett_Detail_URD, flip: false);
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			flag18 = true;
			flag11 = true;
			RandomDetail(index, boundary, boundaryBase, boundary.ett_Detail_RDL, flip: false);
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			flag20 = true;
			flag13 = true;
			ecb.AddComponent(index, ett, new PostTransformMatrix
			{
				Value = float4x4.Scale(-1f, 1f, 1f)
			});
			RandomDetail(index, boundary, boundaryBase, boundary.ett_Detail_URD, flip: true);
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			refRW.ValueRW.Position += new float3(0f, 0f, 0.0001f);
			if (!flag2)
			{
				flag15 = true;
			}
			else if (!flag4)
			{
				flag14 = true;
			}
			else if (!flag6)
			{
				flag14 = true;
				ecb.AddComponent(index, ett, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
			else if (!flag8)
			{
				flag15 = true;
				ecb.AddComponent(index, ett, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
		}
		if (!flag9)
		{
			ecb.DestroyEntity(index, boundary.ett_AO_LUR);
		}
		if (!flag10)
		{
			ecb.DestroyEntity(index, boundary.ett_AO_RD);
		}
		if (!flag11)
		{
			ecb.DestroyEntity(index, boundary.ett_AO_RDL);
		}
		if (!flag12)
		{
			ecb.DestroyEntity(index, boundary.ett_AO_UR);
		}
		if (!flag13)
		{
			ecb.DestroyEntity(index, boundary.ett_AO_URD);
		}
		if (!flag14)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_Corner_RD);
		}
		if (!flag15)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_Corner_UR);
		}
		if (!flag16)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_LUR);
		}
		if (!flag17)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_RD);
		}
		if (!flag18)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_RDL);
		}
		if (!flag19)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_UR);
		}
		if (!flag20)
		{
			ecb.DestroyEntity(index, boundary.ett_Wall_URD);
		}
		ecb.SetComponentEnabled<Boundary_T10_Dots>(index, ett, value: false);
	}

	private void RandomDetail(int index, Boundary_T10_Dots boundary, BoundaryBase_Dots boundaryBase, Entity createEtt, bool flip)
	{
		if (boundaryBase.shouldCreateDetail && ((boundaryBase.selfPosition.x % 2f == 0f && boundaryBase.selfPosition.y % 2f == 0f) || ((boundaryBase.selfPosition.x + 1f) % 2f == 0f && (boundaryBase.selfPosition.y + 1f) % 2f == 0f)) && DTool.RandomValue(ref gRandom.ValueRW.random) <= boundary.detailChance)
		{
			Entity e = ecb.Instantiate(index, createEtt);
			ecb.SetComponent(index, e, new LocalTransform
			{
				Position = boundaryBase.roomPosition + boundaryBase.selfPosition.GetFloat3(),
				Rotation = quaternion.identity,
				Scale = 1f
			});
			if (flip)
			{
				ecb.AddComponent(index, e, new PostTransformMatrix
				{
					Value = float4x4.Scale(-1f, 1f, 1f)
				});
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Boundary_T10_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__BoundaryBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Boundary_T10_Dots boundary = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T10_Dots>(nativeArrayPtr, i);
				ref BoundaryBase_Dots boundaryBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, in boundary, in boundaryBase, ett);
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
					ref Boundary_T10_Dots boundary2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T10_Dots>(nativeArrayPtr, nextRangeBegin);
					ref BoundaryBase_Dots boundaryBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, in boundary2, in boundaryBase2, ett2);
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
				ref Boundary_T10_Dots boundary3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T10_Dots>(nativeArrayPtr, j);
				ref BoundaryBase_Dots boundaryBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, in boundary3, in boundaryBase3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Boundary_T10_Dots boundary4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary_T10_Dots>(nativeArrayPtr, k);
				ref BoundaryBase_Dots boundaryBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, in boundary4, in boundaryBase4, ett4);
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
