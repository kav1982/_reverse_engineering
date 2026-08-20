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
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
public struct Boundary2_T10Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Boundary2_T10> __Boundary2_T10_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<BoundaryBase_Dots> __BoundaryBase_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Boundary2_T10_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Boundary2_T10>();
				__BoundaryBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BoundaryBase_Dots>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Boundary2_T10_RW_ComponentTypeHandle.Update(ref state);
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
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Boundary2_T10>();
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
		public void Run(ref Boundary2_T10Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Boundary2_T10Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Boundary2_T10Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Boundary2_T10Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Boundary2_T10Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Boundary2_T10Job job, EntityManager entityManager)
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

	public void Execute([ChunkIndexInQuery] int index, Boundary2_T10 boundary2, in BoundaryBase_Dots boundaryBase, Entity ett)
	{
		RefRW<LocalTransform> refRW = cluLocalTsf.GetRefRW(ett);
		refRW.ValueRW.Position = boundaryBase.roomPosition + boundaryBase.selfPosition.GetFloat3();
		if (boundaryBase.roomID == 202)
		{
			if (DTool.RandomValue(ref gRandom.ValueRW.random) <= boundary2.detailChancePotion && refRW.ValueRO.Position.x % (float)boundary2.intervalPotion == 0f && refRW.ValueRO.Position.y % (float)boundary2.intervalPotion == 0f)
			{
				Entity e = ecb.Instantiate(index, boundary2.ett_DetailPotion);
				float3 dir = DTool.GetDir(ref gRandom.ValueRW.random, 0f, boundary2.offsetPotion);
				ecb.SetComponent(index, e, new LocalTransform
				{
					Position = refRW.ValueRO.Position + dir,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
		}
		else if (boundaryBase.roomID == 201)
		{
			if (DTool.RandomValue(ref gRandom.ValueRW.random) <= boundary2.detailChanceStore && refRW.ValueRO.Position.x % (float)boundary2.intervalStore == 0f && refRW.ValueRO.Position.y % (float)boundary2.intervalStore == 0f)
			{
				Entity e2 = ecb.Instantiate(index, boundary2.ett_DetailStore);
				float3 dir2 = DTool.GetDir(ref gRandom.ValueRW.random, 0f, boundary2.offsetPotion);
				ecb.SetComponent(index, e2, new LocalTransform
				{
					Position = refRW.ValueRO.Position + dir2,
					Rotation = quaternion.identity,
					Scale = 1f
				});
			}
		}
		else if (boundaryBase.roomID != 211 && boundaryBase.roomID != 212)
		{
			UnityEngine.Debug.Log("T10场景的外围边界ID没在判定范围");
		}
		ecb.SetComponentEnabled<Boundary2_T10>(index, ett, value: false);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Boundary2_T10_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__BoundaryBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Boundary2_T10 reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary2_T10>(nativeArrayPtr, i);
				ref BoundaryBase_Dots boundaryBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, reference, in boundaryBase, ett);
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
					ref Boundary2_T10 reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary2_T10>(nativeArrayPtr, nextRangeBegin);
					ref BoundaryBase_Dots boundaryBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, reference2, in boundaryBase2, ett2);
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
				ref Boundary2_T10 reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary2_T10>(nativeArrayPtr, j);
				ref BoundaryBase_Dots boundaryBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, reference3, in boundaryBase3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Boundary2_T10 reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Boundary2_T10>(nativeArrayPtr, k);
				ref BoundaryBase_Dots boundaryBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<BoundaryBase_Dots>(nativeArrayPtr2, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, reference4, in boundaryBase4, ett4);
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
