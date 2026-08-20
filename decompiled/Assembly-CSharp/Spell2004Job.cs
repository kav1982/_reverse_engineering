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
using UnityEngine;

[CompilerGenerated]
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[BurstCompile]
public struct Spell2004Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2004PillarOfLightData> __Spell2004PillarOfLightData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2004PillarOfLightData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarOfLightData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
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
		public void Run(ref Spell2004Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2004Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2004Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2004Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2004Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2004Job job, EntityManager entityManager)
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
	public ComponentLookup<SpellConfigComponentData> configLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> transformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> unitLookup;

	public float DeltaTime;

	[ReadOnly]
	public PhysicsWorldSingleton Singleton;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity GlobalParticle;

	public Entity SEData;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref Spell2004PillarOfLightData data, ref LocalTransform transform, in SpellMovementComponentData movement, ref UnitProperty_Dots unit, in SpellComponentData componentData, SpellElementEffectComponentData element, ref TeammateData teammateData, in SpellConfigComponentData config, ref DynamicBuffer<StatefulTriggerEvent> buffers, Entity entity)
	{
		TeammateData teammateData2 = teammateData;
		if (teammateData2.AdvanceSkillLevel > 0 && !teammateData2.IsHoldByTeammate6)
		{
			float2 dir = movement.Direction.xy;
			ApplyCrushAttack(ref data, in transform, in teammateData, in config, DTool.DirectionToRotation(in dir));
			UpdateCrushAttackAnim(index, ref data, in transform, in movement, in componentData, in element, ref unit, in teammateData, in config, entity);
		}
	}

	[BurstCompile]
	private void UpdateCrushAttackAnim(int index, ref Spell2004PillarOfLightData data, in LocalTransform transform, in SpellMovementComponentData movement, in SpellComponentData componentData, in SpellElementEffectComponentData element, ref UnitProperty_Dots unit, in TeammateData teammateData, in SpellConfigComponentData config, Entity entity)
	{
		switch (data.AttackState)
		{
		case Spell2004PillarOfLightData.CrushAttackState.DelayToAttack:
		{
			data.HeavyCrashTimer += DeltaTime;
			if (!(data.HeavyCrashTimer >= 0.03f / teammateData.TeammateSpeedRatio))
			{
				break;
			}
			data.HeavyCrashTimer = 0f;
			NativeList<Entity> hits = new NativeList<Entity>(Allocator.Temp);
			float2 dir = movement.Direction.xy;
			GetAttackableEnemies(in transform, in config, ref hits, DTool.DirectionToRotation(in dir));
			TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in element, in componentData, out var info);
			info.damage = math.ceil(unit.unitCfg.maxHP * (1f + (float)(teammateData.AdvanceSkillLevel - 1) / 2f) * (1f + config.Damage.AddRatio) * config.Damage.MulRatio + config.Damage.Extra * 0.5f);
			info.damageRecordId = 3120;
			config.ColorType.ColorEnumToString(out var result);
			CMD.AppendToBuffer(index, SEData, new SEData("SE_Teammate4HeavyAttack"));
			CMD.AppendToBuffer(index, GlobalParticle, new GlobalParticleEmitParams
			{
				Name = $"2004_HeavyCrashEffect_{result}",
				Position = transform.Position,
				Size = 1.5f * (config.Radius.AddRatio + 1f) * config.Radius.MulRatio
			});
			foreach (Entity item in hits)
			{
				Entity target = item;
				info.spell.HitPosition = transformLookup[target].Position;
				info.spell.IgnoreHitEffect = true;
				CMD.TryAttackEntity(index, in target, in info, in unitLookup, in configLookup);
			}
			data.AttackState = Spell2004PillarOfLightData.CrushAttackState.AfterAttack;
			break;
		}
		case Spell2004PillarOfLightData.CrushAttackState.AfterAttack:
			data.CrushAttackAnimTimer += DeltaTime;
			if (data.CrushAttackAnimTimer >= 2.5f - 0.9f / teammateData.TeammateSpeedRatio)
			{
				data.CrushAttackAnimTimer = 0f;
				data.AttackState = Spell2004PillarOfLightData.CrushAttackState.ResetFloatSpeed;
			}
			break;
		case Spell2004PillarOfLightData.CrushAttackState.ResetFloatSpeed:
			data.CrushAttackAnimTimer += DeltaTime;
			data.CurrentFloatingLerpSpeed = math.lerp(data.CurrentFloatingLerpSpeed, 15f, data.CrushAttackAnimTimer / (0.5f / teammateData.TeammateSpeedRatio));
			if (data.CrushAttackAnimTimer >= 0.5f / teammateData.TeammateSpeedRatio)
			{
				data.AttackState = Spell2004PillarOfLightData.CrushAttackState.FindEnemy;
			}
			break;
		}
	}

	[BurstCompile]
	private void ApplyCrushAttack(ref Spell2004PillarOfLightData data, in LocalTransform transform, in TeammateData teammateData, in SpellConfigComponentData config, Quaternion quaternion)
	{
		data.HeavyCrashTimer += DeltaTime * teammateData.TeammateSpeedRatio;
		data.HeavyCrashRecheckTimer += DeltaTime;
		if (!(data.HeavyCrashTimer < 2.5f) && !(data.HeavyCrashRecheckTimer < 0.1f))
		{
			data.HeavyCrashRecheckTimer = 0f;
			NativeList<Entity> hits = new NativeList<Entity>(Allocator.Temp);
			GetAttackableEnemies(in transform, in config, ref hits, quaternion);
			if (hits.Length > 0)
			{
				data.HeavyCrashTimer = 0f;
				data.CurrentFloatingLerpSpeed = 0f;
				data.AttackState = Spell2004PillarOfLightData.CrushAttackState.ReadyToAttack;
			}
		}
	}

	[BurstCompile]
	private void GetAttackableEnemies(in LocalTransform transform, in SpellConfigComponentData config, ref NativeList<Entity> hits, Quaternion quaternion)
	{
		float num = transform.Scale * (1f + config.Radius.AddRatio) * config.Radius.MulRatio;
		float x = 3.5f * num * 0.5f;
		float y = 1.5f * num * 0.5f;
		ref readonly float3 position = ref transform.Position;
		float3 HalfBoxSize = new float3(x, y, 1.5f);
		SpellTools.GetAttackableEntitiesInBox(in position, in HalfBoxSize, in config.ShooterType, containsBrittleness: false, in unitLookup, in configLookup, in Singleton, ref hits, checkUnitCamp: true, in quaternion);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2004PillarOfLightData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		BufferAccessor<StatefulTriggerEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2004PillarOfLightData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref UnitProperty_Dots unit = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr8, i);
				DynamicBuffer<StatefulTriggerEvent> buffers = bufferAccessor[i];
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, i);
				Execute(chunkIndexInQuery, ref data, ref transform, in movement, ref unit, in componentData, reference, ref teammateData, in config, ref buffers, entity);
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
					ref Spell2004PillarOfLightData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref UnitProperty_Dots unit2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr8, nextRangeBegin);
					DynamicBuffer<StatefulTriggerEvent> buffers2 = bufferAccessor[nextRangeBegin];
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, nextRangeBegin);
					Execute(chunkIndexInQuery, ref data2, ref transform2, in movement2, ref unit2, in componentData2, reference2, ref teammateData2, in config2, ref buffers2, entity2);
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
				ref Spell2004PillarOfLightData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref UnitProperty_Dots unit3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr8, j);
				DynamicBuffer<StatefulTriggerEvent> buffers3 = bufferAccessor[j];
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, j);
				Execute(chunkIndexInQuery, ref data3, ref transform3, in movement3, ref unit3, in componentData3, reference3, ref teammateData3, in config3, ref buffers3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2004PillarOfLightData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref UnitProperty_Dots unit4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr8, k);
				DynamicBuffer<StatefulTriggerEvent> buffers4 = bufferAccessor[k];
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, k);
				Execute(chunkIndexInQuery, ref data4, ref transform4, in movement4, ref unit4, in componentData4, reference4, ref teammateData4, in config4, ref buffers4, entity4);
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
