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

[CompilerGenerated]
[BurstCompile]
public struct Spell4013FallDamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public ComponentTypeHandle<Spell4013RuneHammerData> __Spell4013RuneHammerData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<SpellFallTag> __SpellFallTag_RW_ComponentTypeHandle;

			public BufferTypeHandle<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__Spell4013RuneHammerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4013RuneHammerData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellFallTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellFallTag>();
				__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle = state.GetBufferTypeHandle<LinkedEntityGroup>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__Spell4013RuneHammerData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellFallTag_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4013RuneHammerData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellFallTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LinkedEntityGroup>();
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
		public void Run(ref Spell4013FallDamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4013FallDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4013FallDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4013FallDamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4013FallDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4013FallDamageJob job, EntityManager entityManager)
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
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellDecreaseRadiusData> DecreaseRadiusLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell4013SpiltEntityData> SplitLookUp;

	public Entity ScreenShakeSingleton;

	public Entity SEPlayerSingleton;

	public Unity.Mathematics.Random GlobalRandom;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public SpellSingleton SpellSingleton;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity Spell3101Buffer;

	public Entity GlobalParticleSystemBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int chunkIndex, SpellAspect spellAspect, ref Spell4013RuneHammerData runeHammer, Entity entity, SpellFallTag _, DynamicBuffer<LinkedEntityGroup> splitBuffer)
	{
		if (!spellAspect.Movement.ValueRO.IsFallSpell || !(spellAspect.Config.ValueRW.DamageTimer >= 0.5f))
		{
			return;
		}
		spellAspect.Config.ValueRW.DamageTimer = 0f;
		ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
		Entity sEPlayerSingleton = SEPlayerSingleton;
		FixedString32Bytes seName = "FallHitSE";
		cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(4013, in seName), SEPlayMode.Replay, 3, 0.05f, GlobalRandom.NextFloat(0.8f, 1.2f)));
		CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
		{
			Radius = 0.15f,
			Speed = 0.25f,
			Time = 0.1f
		});
		NativeList<Entity> targets = new NativeList<Entity>(Allocator.Temp);
		if (!runeHammer.HasSplitSpell)
		{
			AttackInRangeEnemies(chunkIndex, spellAspect.Transform.ValueRO.Position, entity, ref targets, in spellAspect);
		}
		else
		{
			foreach (LinkedEntityGroup item in splitBuffer)
			{
				if (SplitLookUp.HasComponent(item.Value))
				{
					float3 position = LocalTransformLookUp[item.Value].Position;
					AttackInRangeEnemies(chunkIndex, position, entity, ref targets, in spellAspect);
					targets.Clear();
				}
			}
		}
		targets.Dispose();
	}

	[BurstCompile]
	private void AttackInRangeEnemies(int chunkIndex, float3 hitPosition, Entity entity, ref NativeList<Entity> targets, in SpellAspect spellAspect)
	{
		float radius = spellAspect.Config.ValueRO.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in hitPosition, in radius, in spellAspect.Config.ValueRO.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref targets);
		ref readonly SpellConfigComponentData valueRO = ref spellAspect.Config.ValueRO;
		ref readonly SpellMovementComponentData valueRO2 = ref spellAspect.Movement.ValueRO;
		LocalTransform transform = LocalTransformLookUp[entity];
		SpellElementEffectComponentData elementEffect = SpellElementLookup[entity];
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in valueRO, in valueRO2, in transform, in elementEffect, in spellAspect.Data.ValueRO, out var info);
		if (DecreaseRadiusLookup.TryGetComponent(entity, out var componentData))
		{
			info.damage *= GeneralTool.GetSpellRadiusToDamageRatio(spellAspect.Config.ValueRO.Radius.Calculate(), componentData.RadiusMult, componentData.RadiusToDamageRatio);
		}
		foreach (Entity target2 in targets)
		{
			Entity target = target2;
			CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
		spellAspect.Config.ValueRO.ColorType.ColorEnumToString(out var result);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"3119_Fall_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3((hitPosition + new float3(0f, hitPosition.z, 0f)).xy, 1.08f);
		globalParticleEmitParams.Size = spellAspect.Config.ValueRO.Radius.Calculate();
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, hitPosition, UnitPropertyLookup, PhysicsWorld, in spellAspect.Config.ValueRO, in spellAspect.Movement.ValueRO, in spellAspect.Transform.ValueRO, in spellAspect.ElementEffect.ValueRO, in spellAspect.Data.ValueRO, entity);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4013RuneHammerData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		BufferAccessor<LinkedEntityGroup> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spellAspect = resolvedChunk[i];
				ref Spell4013RuneHammerData runeHammer = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				DynamicBuffer<LinkedEntityGroup> splitBuffer = bufferAccessor[i];
				Execute(chunkIndexInQuery, spellAspect, ref runeHammer, entity, default(SpellFallTag), splitBuffer);
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
					SpellAspect spellAspect2 = resolvedChunk[nextRangeBegin];
					ref Spell4013RuneHammerData runeHammer2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					DynamicBuffer<LinkedEntityGroup> splitBuffer2 = bufferAccessor[nextRangeBegin];
					Execute(chunkIndexInQuery, spellAspect2, ref runeHammer2, entity2, default(SpellFallTag), splitBuffer2);
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
				SpellAspect spellAspect3 = resolvedChunk[j];
				ref Spell4013RuneHammerData runeHammer3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				DynamicBuffer<LinkedEntityGroup> splitBuffer3 = bufferAccessor[j];
				Execute(chunkIndexInQuery, spellAspect3, ref runeHammer3, entity3, default(SpellFallTag), splitBuffer3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				SpellAspect spellAspect4 = resolvedChunk[k];
				ref Spell4013RuneHammerData runeHammer4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				DynamicBuffer<LinkedEntityGroup> splitBuffer4 = bufferAccessor[k];
				Execute(chunkIndexInQuery, spellAspect4, ref runeHammer4, entity4, default(SpellFallTag), splitBuffer4);
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
