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
[WithNone(new Type[]
{
	typeof(SpellFallTag),
	typeof(Spell1008InitializedTag)
})]
public struct Spell1008Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public ComponentTypeHandle<Spell1008ArcaneExplosionData> __Spell1008ArcaneExplosionData_RW_ComponentTypeHandle;

			public BufferTypeHandle<Spell1008HitTargetsData> __Spell1008HitTargetsData_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1008ArcaneExplosionData>();
				__Spell1008HitTargetsData_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell1008HitTargetsData>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle.Update(ref state);
				__Spell1008HitTargetsData_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1008ArcaneExplosionData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008HitTargetsData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<SpellFallTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell1008InitializedTag>();
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
		public void Run(ref Spell1008Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1008Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1008Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1008Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1008Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1008Job job, EntityManager entityManager)
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
	public ComponentLookup<LocalTransform> TransformLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1008SpellMaterialProperty> SpellMaterialLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectCollectorLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(SpellAspect spell, [ChunkIndexInQuery] int chunkIndex, ref Spell1008ArcaneExplosionData data, DynamicBuffer<Spell1008HitTargetsData> _)
	{
		float num = spell.Config.ValueRO.HoverDuration - spell.Config.ValueRO.HoverTimer;
		float num2 = spell.Config.ValueRO.Duration.Calculate() - spell.Config.ValueRO.DurationTimer;
		float num3 = 0.15f;
		if (!(spell.Config.ValueRO.HoverDuration > 0f) || !(num >= num3) || !(num2 <= num3))
		{
			float num4 = spell.Config.ValueRO.Duration.Calculate();
			float num5 = spell.Config.ValueRW.DurationTimer;
			if (spell.Config.ValueRO.HoverDuration > 0f && num <= num3)
			{
				num5 = num4 - num;
			}
			RefRW<LocalTransform> refRW = TransformLookUp.GetRefRW(spell.Data.ValueRO.SpellEffectEntity);
			spell.Config.ValueRW.DurationTimer = math.clamp(spell.Config.ValueRW.DurationTimer, 0f, 1f);
			float num6 = num5;
			float x = (num6 * 2f - math.pow(num6, 2f)) * 1.2f * spell.Config.ValueRO.Radius.Calculate();
			refRW.ValueRW.Scale = math.max(x, 0.01f);
			EffectsCollectorData effectsCollectorData = EffectCollectorLookup[spell.Data.ValueRO.SpellEffectEntity];
			float num7 = num5 / num4;
			float num8 = num4 * 0.25f;
			num7 = ((!(num7 <= num4 * 0.25f)) ? (1f - (num5 - num8) / (spell.Config.ValueRO.Duration.Calculate() - num8)) : (num5 / num4));
			num7 = math.clamp(num7, 0f, 1f);
			RefRW<Spell1008SpellMaterialProperty> refRW2 = SpellMaterialLookup.GetRefRW(effectsCollectorData.Effect1);
			refRW2.ValueRW.Value = num7;
			refRW2 = SpellMaterialLookup.GetRefRW(effectsCollectorData.Effect2);
			refRW2.ValueRW.Value = ((num7 <= 0.3f) ? 0f : num7);
			refRW2 = SpellMaterialLookup.GetRefRW(effectsCollectorData.Effect3);
			refRW2.ValueRW.Value = num7;
			refRW2 = SpellMaterialLookup.GetRefRW(effectsCollectorData.Effect4);
			refRW2.ValueRW.Value = num7;
			if (spell.Config.ValueRO.ColorType == SpellColorType.Void)
			{
				refRW2 = SpellMaterialLookup.GetRefRW(effectsCollectorData.Effect5);
				refRW2.ValueRW.Value = num7;
			}
		}
		if (spell.Config.ValueRO.DurationTimer >= 0.3f && !data.IsFullRangeDamage)
		{
			data.IsFullRangeDamage = true;
			CMD.AppendToBuffer(chunkIndex, spell.Entity, new Spell1008TakeDamageBuffer
			{
				HitPosition = spell.Transform.ValueRO.Position + new float3(0f, 0f, -0.3f),
				IsFullRangeDamage = true,
				EffectEntity = spell.Data.ValueRO.SpellEffectEntity
			});
		}
		if (spell.Config.ValueRO.HoverTimer > 0f)
		{
			data.HoverDamageTimer += DeltaTime;
			if (data.HoverDamageTimer >= 0.4f)
			{
				data.HoverDamageTimer -= 0.4f;
				CMD.AppendToBuffer(chunkIndex, spell.Entity, new Spell1008TakeDamageBuffer
				{
					HitPosition = spell.Transform.ValueRO.Position + new float3(0f, 0f, -0.3f),
					IsFullRangeDamage = true,
					EffectEntity = spell.Data.ValueRO.SpellEffectEntity
				});
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1008ArcaneExplosionData_RW_ComponentTypeHandle);
		BufferAccessor<Spell1008HitTargetsData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1008HitTargetsData_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spell = resolvedChunk[i];
				ref Spell1008ArcaneExplosionData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, i);
				DynamicBuffer<Spell1008HitTargetsData> _ = bufferAccessor[i];
				Execute(spell, chunkIndexInQuery, ref data, _);
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
					ref Spell1008ArcaneExplosionData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, nextRangeBegin);
					DynamicBuffer<Spell1008HitTargetsData> _2 = bufferAccessor[nextRangeBegin];
					Execute(spell2, chunkIndexInQuery, ref data2, _2);
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
				ref Spell1008ArcaneExplosionData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, j);
				DynamicBuffer<Spell1008HitTargetsData> _3 = bufferAccessor[j];
				Execute(spell3, chunkIndexInQuery, ref data3, _3);
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
				ref Spell1008ArcaneExplosionData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1008ArcaneExplosionData>(nativeArrayPtr, k);
				DynamicBuffer<Spell1008HitTargetsData> _4 = bufferAccessor[k];
				Execute(spell4, chunkIndexInQuery, ref data4, _4);
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
