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
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;

[CompilerGenerated]
[BurstCompile]
public struct Spell1031DaveShotgunJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1031DaveShotgunData> __Spell1031DaveShotgunData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<Shadow_Dots> __Shadow_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RW_ComponentTypeHandle;

			public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Spell1031DaveShotgunData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1031DaveShotgunData>();
				__SpellSplitComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSplitComponentData>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Shadow_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Shadow_Dots>();
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSpeedRatioValueData>();
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell1031DaveShotgunData_RW_ComponentTypeHandle.Update(ref state);
				__SpellSplitComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Shadow_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSplitComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1031DaveShotgunData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Shadow_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellSpeedRatioValueData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
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
		public void Run(ref Spell1031DaveShotgunJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1031DaveShotgunJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1031DaveShotgunJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1031DaveShotgunJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1031DaveShotgunJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1031DaveShotgunJob job, EntityManager entityManager)
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

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public EntityCommandBuffer.ParallelWriter Ecb;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	public Entity GlobalParticleBuffer;

	[ReadOnly]
	public Entity ScreenShakeSingleton;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<HarpoonsHitCounter> HitCounterLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellGroundedTag> GroundTagLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public bool HaveHitCounter;

	public Entity HitCounterEntity;

	[ReadOnly]
	public EntityStorageInfoLookup EntityExists;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(ref LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData spell, ref SpellElementEffectComponentData elementEffect, ref Spell1031DaveShotgunData data, in SpellSplitComponentData split, in Entity entity, ref Shadow_Dots shadow, SpellSpeedRatioValueData speedRatioValueData, DynamicBuffer<StatefulTriggerEvent> hitTriggers, [ChunkIndexInQuery] int chunkIndex)
	{
		if (!movement.IsFallSpell)
		{
			AttributeValue speed = speedRatioValueData.Speed;
			data.MoveDistanceMax = (3.5f + (float)config.Level) * speed.MulRatio + speed.AddBase * 1f + config.Duration.Base * config.Duration.MulRatio * 1.5f;
		}
		if (!data.IsInitialized)
		{
			data.IsInitialized = true;
			if (!movement.IsFallSpell)
			{
				CreateShootEffect(chunkIndex, config.ColorType, transform.Position, movement.Direction, transform.Scale);
				SpellSpecialMovementType type = movement.Type;
				if (type == SpellSpecialMovementType.ChaseEnemy || type == SpellSpecialMovementType.ChaseMouse)
				{
					data.MoveDistanceMax *= 1.25f;
				}
				if (spell.IsSplitSpell && split.Count == 0)
				{
					data.MoveDistanceMax *= 0.6f;
				}
				else
				{
					Ecb.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
					{
						Radius = 0.05f,
						Speed = 1f,
						Time = 0.05f
					});
				}
			}
			else
			{
				float3 layerPosition = DTool.GetLayerPosition(in transform.Position, LayerCorrectType.Coordinate);
				float3 direction = movement.FallTargetPosition - (transform.Position + layerPosition);
				direction.z = 0f;
				CreateShootEffect(chunkIndex, config.ColorType, transform.Position, direction, transform.Scale);
			}
		}
		if (movement.IsFallSpell && GroundTagLookUp.IsComponentEnabled(entity))
		{
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			ref float3 position = ref transform.Position;
			float radius = config.Radius.Calculate();
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
			if (HaveHitCounter)
			{
				HitCounterLookup.GetRefRW(HitCounterEntity).ValueRW.HitCount += entities.Length;
			}
		}
		LocalTransformLookUp.GetRefRW(shadow.ett_Shadow).ValueRW.Rotation = quaternion.LookRotationSafe(movement.Direction, math.up());
		if (data.LastFramePosition.x != 0f)
		{
			float num = math.distance(transform.Position, data.LastFramePosition);
			if (movement.Type != SpellSpecialMovementType.Rotation && !movement.IsFallSpell)
			{
				data.MoveDistance += num;
				if (data.MoveDistance >= data.MoveDistanceMax)
				{
					config.DurationTimer = config.Duration.Calculate();
				}
			}
		}
		data.LastFramePosition = transform.Position;
	}

	private void CreateShootEffect(int chunkIndex, SpellColorType color, float3 position, float3 direction, float size)
	{
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		layerPosition += position + direction * size * 0.3f;
		color.ColorEnumToString(out var result);
		Ecb.AppendToBuffer(chunkIndex, GlobalParticleBuffer, new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1031_Shoot_{result}", layerPosition)
		{
			Size = size,
			Velocity = direction
		});
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1031DaveShotgunData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellSplitComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Shadow_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellSpeedRatioValueData_RW_ComponentTypeHandle);
		BufferAccessor<StatefulTriggerEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref SpellComponentData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, i);
				ref Spell1031DaveShotgunData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr6, i);
				ref SpellSplitComponentData split = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				ref Shadow_Dots shadow = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr9, i);
				ref SpellSpeedRatioValueData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr10, i);
				DynamicBuffer<StatefulTriggerEvent> hitTriggers = bufferAccessor[i];
				Execute(ref transform, ref config, ref movement, ref spell, ref elementEffect, ref data, in split, in entity, ref shadow, reference, hitTriggers, chunkIndexInQuery);
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
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref SpellComponentData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref Spell1031DaveShotgunData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr6, nextRangeBegin);
					ref SpellSplitComponentData split2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					ref Shadow_Dots shadow2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr9, nextRangeBegin);
					ref SpellSpeedRatioValueData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr10, nextRangeBegin);
					DynamicBuffer<StatefulTriggerEvent> hitTriggers2 = bufferAccessor[nextRangeBegin];
					Execute(ref transform2, ref config2, ref movement2, ref spell2, ref elementEffect2, ref data2, in split2, in entity2, ref shadow2, reference2, hitTriggers2, chunkIndexInQuery);
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
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref SpellComponentData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, j);
				ref Spell1031DaveShotgunData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr6, j);
				ref SpellSplitComponentData split3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				ref Shadow_Dots shadow3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr9, j);
				ref SpellSpeedRatioValueData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr10, j);
				DynamicBuffer<StatefulTriggerEvent> hitTriggers3 = bufferAccessor[j];
				Execute(ref transform3, ref config3, ref movement3, ref spell3, ref elementEffect3, ref data3, in split3, in entity3, ref shadow3, reference3, hitTriggers3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref SpellComponentData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, k);
				ref Spell1031DaveShotgunData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr6, k);
				ref SpellSplitComponentData split4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				ref Shadow_Dots shadow4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr9, k);
				ref SpellSpeedRatioValueData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr10, k);
				DynamicBuffer<StatefulTriggerEvent> hitTriggers4 = bufferAccessor[k];
				Execute(ref transform4, ref config4, ref movement4, ref spell4, ref elementEffect4, ref data4, in split4, in entity4, ref shadow4, reference4, hitTriggers4, chunkIndexInQuery);
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
