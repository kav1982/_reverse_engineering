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

[CompilerGenerated]
[BurstCompile]
public struct Spell1008FallEffectJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1008FallData> __Spell1008FallData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<EffectsCollectorData> __EffectsCollectorData_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1008FallData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1008FallData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__EffectsCollectorData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EffectsCollectorData>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell1008FallData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__EffectsCollectorData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EffectsCollectorData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008FallData>();
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
		public void Run(ref Spell1008FallEffectJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1008FallEffectJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1008FallEffectJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1008FallEffectJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1008FallEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1008FallEffectJob job, EntityManager entityManager)
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

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1008SpellMaterialProperty> SpellMaterialLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref Spell1008FallData fallData, ref LocalTransform transform, in EffectsCollectorData collector, Entity e)
	{
		if (fallData.DurationTimer < 0.6f)
		{
			fallData.DurationTimer += DeltaTime;
		}
		else
		{
			fallData.HoverTimer += DeltaTime;
		}
		float num = fallData.HoverDuration - fallData.HoverTimer;
		float num2 = 0.6f - fallData.DurationTimer;
		float num3 = 0.15f;
		if (fallData.HoverDuration > 0f && num >= num3 && num2 <= num3)
		{
			return;
		}
		float num4 = 0.6f;
		float num5 = fallData.DurationTimer;
		if ((double)fallData.HoverDuration > 0.001 && num <= num3)
		{
			fallData.FinishDamageApply = true;
			num5 = num4 - num;
		}
		float num6 = num5;
		float x = (num6 * 2f - math.pow(num6, 2f)) * 1.2f * fallData.Radius;
		transform.Scale = math.max(x, 0.01f);
		float num7 = num6 / num4;
		float num8 = num4 * 0.25f;
		num7 = ((!(num7 <= num8)) ? (1f - (num6 - num8) / (0.6f - num8)) : (num6 / num4));
		RefRW<Spell1008SpellMaterialProperty> refRW = SpellMaterialLookup.GetRefRW(collector.Effect1);
		refRW.ValueRW.Value = num7;
		refRW = SpellMaterialLookup.GetRefRW(collector.Effect2);
		refRW.ValueRW.Value = ((num7 <= 0.3f) ? 0f : num7);
		refRW = SpellMaterialLookup.GetRefRW(collector.Effect3);
		refRW.ValueRW.Value = num7;
		refRW = SpellMaterialLookup.GetRefRW(collector.Effect4);
		refRW.ValueRW.Value = num7;
		if (fallData.IsVoidColor)
		{
			refRW = SpellMaterialLookup.GetRefRW(collector.Effect5);
			refRW.ValueRW.Value = num7;
		}
		if (fallData.DurationTimer >= 0.3f && !fallData.IsFullRangeDamage)
		{
			fallData.IsFullRangeDamage = true;
			CMD.AppendToBuffer(index, fallData.SpellEntity, new Spell1008TakeDamageBuffer
			{
				HitPosition = transform.Position + new float3(0f, 0f, -0.3f),
				IsFullRangeDamage = true,
				EffectEntity = e
			});
			if (fallData.HoverDuration <= 0.001f)
			{
				fallData.FinishDamageApply = true;
			}
		}
		if (fallData.HoverDuration > 0.01f)
		{
			fallData.HoverDamageTimer += DeltaTime;
			if (fallData.HoverDamageTimer >= 0.4f)
			{
				fallData.HoverDamageTimer -= 0.4f;
				CMD.AppendToBuffer(index, fallData.SpellEntity, new Spell1008TakeDamageBuffer
				{
					HitPosition = transform.Position + new float3(0f, 0f, -0.3f),
					IsFullRangeDamage = true,
					EffectEntity = e
				});
			}
		}
		if (fallData.HoverTimer >= fallData.HoverDuration && fallData.DurationTimer >= 0.6f)
		{
			CMD.DestroyEntity(index, e);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1008FallData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__EffectsCollectorData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1008FallData fallData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008FallData>(nativeArrayPtr, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
				ref EffectsCollectorData collector = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr3, i);
				Entity e = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
				Execute(chunkIndexInQuery, ref fallData, ref transform, in collector, e);
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
					ref Spell1008FallData fallData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008FallData>(nativeArrayPtr, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
					ref EffectsCollectorData collector2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr3, nextRangeBegin);
					Entity e2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
					Execute(chunkIndexInQuery, ref fallData2, ref transform2, in collector2, e2);
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
				ref Spell1008FallData fallData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008FallData>(nativeArrayPtr, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
				ref EffectsCollectorData collector3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr3, j);
				Entity e3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
				Execute(chunkIndexInQuery, ref fallData3, ref transform3, in collector3, e3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1008FallData fallData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008FallData>(nativeArrayPtr, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
				ref EffectsCollectorData collector4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EffectsCollectorData>(nativeArrayPtr3, k);
				Entity e4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
				Execute(chunkIndexInQuery, ref fallData4, ref transform4, in collector4, e4);
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
