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
public struct Spell1009HitEffectJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public BufferTypeHandle<SpellHitEntity> __SpellHitEntity_RW_BufferTypeHandle;

			public ComponentTypeHandle<Spell1009BackMpData> __Spell1009BackMpData_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__SpellHitEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<SpellHitEntity>();
				__Spell1009BackMpData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1009BackMpData>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__SpellHitEntity_RW_BufferTypeHandle.Update(ref state);
				__Spell1009BackMpData_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHitEntity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1009BackMpData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
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
		public void Run(ref Spell1009HitEffectJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1009HitEffectJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1009HitEffectJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1009HitEffectJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1009HitEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1009HitEffectJob job, EntityManager entityManager)
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

	public Entity GlobalParticleEntity;

	public EntityCommandBuffer.ParallelWriter Cmd;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocaltransformLookup;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(SpellAspect spell, DynamicBuffer<SpellHitEntity> hitEntities, Spell1009BackMpData _, [ChunkIndexInQuery] int index)
	{
		if (hitEntities.Length <= 0)
		{
			return;
		}
		spell.Config.ValueRW.ColorType.ColorEnumToString(out var result);
		for (int i = 0; i < hitEntities.Length; i++)
		{
			if (LocaltransformLookup.HasComponent(hitEntities[i].Entity))
			{
				Cmd.AppendToBuffer(index, GlobalParticleEntity, new GlobalParticleEmitParams
				{
					Position = LocaltransformLookup[hitEntities[i].Entity].Position + new float3(0f, 0.3f, 0f),
					Size = spell.Transform.ValueRO.Scale * 1.5f,
					Name = $"1009_Hit_{result}"
				});
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		BufferAccessor<SpellHitEntity> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SpellHitEntity_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1009BackMpData_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spell = resolvedChunk[i];
				DynamicBuffer<SpellHitEntity> hitEntities = bufferAccessor[i];
				Execute(spell, hitEntities, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1009BackMpData>(nativeArrayPtr, i), chunkIndexInQuery);
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
					SpellAspect spell2 = resolvedChunk[nextRangeBegin];
					DynamicBuffer<SpellHitEntity> hitEntities2 = bufferAccessor[nextRangeBegin];
					Execute(spell2, hitEntities2, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1009BackMpData>(nativeArrayPtr, nextRangeBegin), chunkIndexInQuery);
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
				SpellAspect spell3 = resolvedChunk[j];
				DynamicBuffer<SpellHitEntity> hitEntities3 = bufferAccessor[j];
				Execute(spell3, hitEntities3, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1009BackMpData>(nativeArrayPtr, j), chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				SpellAspect spell4 = resolvedChunk[k];
				DynamicBuffer<SpellHitEntity> hitEntities4 = bufferAccessor[k];
				Execute(spell4, hitEntities4, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1009BackMpData>(nativeArrayPtr, k), chunkIndexInQuery);
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
