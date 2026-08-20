using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[CompilerGenerated]
public struct Spell9004SoundWaveJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell9004SoundWaveData> __Spell9004SoundWaveData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell9004SoundWaveData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell9004SoundWaveData>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
			}

			public void Update(ref SystemState state)
			{
				__Spell9004SoundWaveData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell9004SoundWaveData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
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
		public void Run(ref Spell9004SoundWaveJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell9004SoundWaveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell9004SoundWaveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell9004SoundWaveJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell9004SoundWaveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell9004SoundWaveJob job, EntityManager entityManager)
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

	public float DeltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell9004SoundWaveData soundWaveData, ref SpellConfigComponentData config, in SpellComponentData spell, ref SpellMovementComponentData movement, ref SpellElementEffectComponentData elementEffect, ref LocalTransform localTransform)
	{
		if (!soundWaveData.InitOver)
		{
			soundWaveData.InitOver = true;
		}
		soundWaveData.width = config.Level;
		int num = ((config.Level == 1) ? 6 : 15);
		localTransform.Scale = math.lerp(1f, num, config.DurationTimer / config.Duration.Calculate());
		localTransform.Position.z -= 0.05f * DeltaTime;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell9004SoundWaveData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9004SoundWaveData>(nativeArrayPtr, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, i));
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
					Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9004SoundWaveData>(nativeArrayPtr, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, nextRangeBegin));
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
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9004SoundWaveData>(nativeArrayPtr, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell9004SoundWaveData>(nativeArrayPtr, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, k));
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
