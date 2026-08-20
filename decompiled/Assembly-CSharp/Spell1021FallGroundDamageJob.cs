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
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Spell1021FallGroundDamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellFallTag> __SpellFallTag_RO_ComponentTypeHandle;

			[ReadOnly]
			public BufferTypeHandle<Spell1021HitTargetBuffer> __Spell1021HitTargetBuffer_RO_BufferTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1021MagicBreakerData> __Spell1021MagicBreakerData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellFallTag_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellFallTag>(isReadOnly: true);
				__Spell1021HitTargetBuffer_RO_BufferTypeHandle = state.GetBufferTypeHandle<Spell1021HitTargetBuffer>(isReadOnly: true);
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__Spell1021MagicBreakerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1021MagicBreakerData>();
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellFallTag_RO_ComponentTypeHandle.Update(ref state);
				__Spell1021HitTargetBuffer_RO_BufferTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Spell1021MagicBreakerData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellFallTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1021HitTargetBuffer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021MagicBreakerData>();
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
		public void Run(ref Spell1021FallGroundDamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1021FallGroundDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1021FallGroundDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1021FallGroundDamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1021FallGroundDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1021FallGroundDamageJob job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[ReadOnly]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[ReadOnly]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellMovementComponentData> MovementLookup;

	[NativeDisableParallelForRestriction]
	[ReadOnly]
	public ComponentLookup<SpellComponentData> SpellDataLookup;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public SpellSingleton SpellSingleton;

	public Entity SEPlayerSingleton;

	public GlobalRandom Random;

	public float DeltaTime;

	public Entity SpawnParamsEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in SpellFallTag _, in DynamicBuffer<Spell1021HitTargetBuffer> hitTargetBuffer, in SpellComponentData data, ref Spell1021MagicBreakerData spell, in LocalTransform transform, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect)
	{
		if (!spell.readyToDestroy)
		{
			return;
		}
		ref SpellConfigComponentData valueRW = ref SpellConfigLookup.GetRefRW(entity).ValueRW;
		valueRW.DamageTimer += DeltaTime;
		if (!(valueRW.DamageTimer >= 0.049f))
		{
			return;
		}
		valueRW.DamageTimer -= 0.049f;
		NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
		DTool.GetEnemyEntityInRange(in transform.Position, transform.Scale, valueRW.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in PhysicsWorld, ref result);
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = 25165824u;
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		PhysicsWorld.OverlapSphere(transform.Position, transform.Scale, ref outHits, @default);
		foreach (DistanceHit item in outHits)
		{
			if (TransformLookup.HasComponent(item.Entity))
			{
				Entity value = item.Entity;
				result.Add(in value);
			}
		}
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in valueRW, in movement, in transform, in elementEffect, in data, out var info);
		foreach (Entity item2 in result)
		{
			Entity target = item2;
			bool flag = false;
			foreach (Spell1021HitTargetBuffer item3 in hitTargetBuffer)
			{
				if (item3.Target == target)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				CMD.AppendToBuffer(chunkIndex, entity, new Spell1021HitTargetBuffer
				{
					Target = target
				});
				float3 position = transform.Position;
				info.spell.HitPosition = position;
				info.SetKnockbackForceIgnoreZBySpell(TransformLookup[target].Position - position);
				info.knockbackForce *= 2f;
				info.spell.IgnoreHitEffect = true;
				SpellTools.HitType num = CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				ref SpellSingleton spellSingleton = ref SpellSingleton;
				float3 position2 = TransformLookup[target].Position + new float3(0f, 0.3f, 0f);
				float3 direction = DTool.GetDir(Random.random.NextFloat(0f, 360f) * (MathF.PI / 180f));
				cMD.CreateSpellHitEffect(chunkIndex, in spellSingleton, in valueRW, in data, in position2, in direction, transform.Scale);
				ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Hit";
				cMD2.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1021, in seName), SEPlayMode.Replay, 3, 0.05f, Random.random.NextFloat(0.8f, 1.2f)));
				if (num == SpellTools.HitType.IgnoreSpell)
				{
					ref EntityCommandBuffer.ParallelWriter cMD3 = ref CMD;
					ref SpellSingleton spellSingleton2 = ref SpellSingleton;
					ref Entity spawnParamsEntity = ref SpawnParamsEntity;
					LocalTransform transform2 = TransformLookup[entity];
					SpellConfigComponentData config = SpellConfigLookup[entity];
					SpellComponentData data2 = SpellDataLookup[entity];
					Spell1021MagicBreakerDamageSystem.TryReflectTargetSpell(ref cMD3, in spellSingleton2, in target, in spawnParamsEntity, in transform2, in config, in data2, chunkIndex, in SpellConfigLookup, in TransformLookup, in SpellDataLookup, in MovementLookup);
				}
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		BufferAccessor<Spell1021HitTargetBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1021HitTargetBuffer_RO_BufferTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1021MagicBreakerData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				DynamicBuffer<Spell1021HitTargetBuffer> hitTargetBuffer = bufferAccessor[i];
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref Spell1021MagicBreakerData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr3, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				SpellFallTag _ = default(SpellFallTag);
				Execute(chunkIndexInQuery, entity, in _, in hitTargetBuffer, in data, ref spell, in transform, in movement, in elementEffect);
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
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, nextRangeBegin);
					DynamicBuffer<Spell1021HitTargetBuffer> hitTargetBuffer2 = bufferAccessor[nextRangeBegin];
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref Spell1021MagicBreakerData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr3, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					SpellFallTag _ = default(SpellFallTag);
					Execute(chunkIndexInQuery, entity2, in _, in hitTargetBuffer2, in data2, ref spell2, in transform2, in movement2, in elementEffect2);
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
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, j);
				DynamicBuffer<Spell1021HitTargetBuffer> hitTargetBuffer3 = bufferAccessor[j];
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref Spell1021MagicBreakerData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr3, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				SpellFallTag _ = default(SpellFallTag);
				Execute(chunkIndexInQuery, entity3, in _, in hitTargetBuffer3, in data3, ref spell3, in transform3, in movement3, in elementEffect3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, k);
				DynamicBuffer<Spell1021HitTargetBuffer> hitTargetBuffer4 = bufferAccessor[k];
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref Spell1021MagicBreakerData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr3, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				SpellFallTag _ = default(SpellFallTag);
				Execute(chunkIndexInQuery, entity4, in _, in hitTargetBuffer4, in data4, ref spell4, in transform4, in movement4, in elementEffect4);
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
