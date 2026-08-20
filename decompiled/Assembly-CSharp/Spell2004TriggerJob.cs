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
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[CompilerGenerated]
public struct Spell2004TriggerJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2004PillarOfLightData> __Spell2004PillarOfLightData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2004PillarOfLightData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
			}

			public void Update(ref SystemState state)
			{
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarOfLightData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
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
		public void Run(ref Spell2004TriggerJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2004TriggerJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2004TriggerJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2004TriggerJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2004TriggerJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2004TriggerJob job, EntityManager entityManager)
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
	public ComponentLookup<UnitProperty_Dots> UnitLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> ConfigLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellMovementComponentData> MovementLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellComponentData> ComponentLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellElementEffectComponentData> ElementLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	public EntityCommandBuffer.ParallelWriter CMD;

	public float DeltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int index, ref Spell2004PillarOfLightData spell, Entity entity, TeammateData teammateData)
	{
		if (!teammateData.IsHoldByTeammate6)
		{
			SpellMovementComponentData movement = MovementLookup.GetRefRO(entity).ValueRO;
			LocalTransform transform = TransformLookup.GetRefRO(entity).ValueRO;
			SpellComponentData componentData = ComponentLookup.GetRefRO(entity).ValueRO;
			SpellElementEffectComponentData element = ElementLookup.GetRefRO(entity).ValueRO;
			SpellConfigComponentData config = ConfigLookup.GetRefRW(entity).ValueRW;
			UnitProperty_Dots unit = UnitLookup.GetRefRO(entity).ValueRO;
			ApplyDamage(ref spell, in transform, in movement, in componentData, in element, in config, in unit, entity, index, teammateData);
			ApplyDebuff(ref spell, in element, in config, transform);
		}
	}

	[BurstCompile]
	private void ApplyDamage(ref Spell2004PillarOfLightData data, in LocalTransform transform, in SpellMovementComponentData movement, in SpellComponentData componentData, in SpellElementEffectComponentData element, in SpellConfigComponentData config, in UnitProperty_Dots unit, Entity entity, int chunkIndex, TeammateData teammateData)
	{
		if (config.Float3 <= 0f || teammateData.IsHoldByTeammate6)
		{
			return;
		}
		data.AttackTimer += DeltaTime;
		if (data.AttackTimer < 0.5f)
		{
			return;
		}
		data.AttackTimer -= 0.5f;
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref readonly float3 position = ref transform.Position;
		float radius = 0.3f * transform.Scale;
		SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in ConfigLookup, in Physics, ref entities);
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in element, in componentData, out var info);
		info.damage = math.ceil(unit.unitCfg.maxHP * config.Float3 / 100f * (config.Damage.AddRatio + 1f) * config.Damage.MulRatio + config.Damage.Extra * 0.5f);
		foreach (Entity item in entities)
		{
			Entity target = item;
			CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in ConfigLookup, checkCamp: false);
		}
	}

	[BurstCompile]
	private void ApplyDebuff(ref Spell2004PillarOfLightData data, in SpellElementEffectComponentData element, in SpellConfigComponentData config, LocalTransform transform)
	{
		data.ApplyDebuffTimer += DeltaTime;
		data.ApplyVenomTimer += DeltaTime;
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref float3 position = ref transform.Position;
		float radius = 0.3f * transform.Scale;
		SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in ConfigLookup, in Physics, ref entities);
		RefRW<UnitProperty_Dots> refRW;
		if (data.ApplyDebuffTimer >= 0.15f)
		{
			data.ApplyDebuffTimer -= 0.15f;
			foreach (Entity item in entities)
			{
				if (!UnitLookup.HasComponent(item))
				{
					continue;
				}
				refRW = UnitLookup.GetRefRW(item);
				ref UnitProperty_Dots valueRW = ref refRW.ValueRW;
				if (!DTool.IsSameCamp(config.ShooterType, valueRW.unitCfg.unitType) && valueRW.unitCfg.unitType != UnitType.Brittleness && !valueRW.isDead)
				{
					if (element.FireHpBurnPercent > 0f)
					{
						valueRW.SetBurn(element.FireBurnDuration, element.FireHpBurnPercent);
					}
					if (element.FrozenDuration > 0f)
					{
						valueRW.SetFrozen(element.FrozenDuration);
					}
					if (element.MucusDuration > 0f)
					{
						valueRW.SetMucus(element.MucusDuration, element.MucusMoveSpeedRatio, element.MucusSpellSpeedRatio);
					}
					if (element.VoidExplosionHpDamageRatio > 0f)
					{
						valueRW.SetVoid(new Spell3129VoidExplosion.VoidExplosionData_Dots
						{
							ConstVoidEffect = true,
							ExplosionRange = element.VoidExplosionRange,
							HpToDmgRatio = element.VoidExplosionHpDamageRatio,
							InstantKillRatio = element.VoidInstantKillThreshold
						});
					}
				}
			}
		}
		if (!(data.ApplyVenomTimer >= 1f))
		{
			return;
		}
		data.ApplyVenomTimer -= 1f;
		foreach (Entity item2 in entities)
		{
			if (UnitLookup.HasComponent(item2))
			{
				refRW = UnitLookup.GetRefRW(item2);
				ref UnitProperty_Dots valueRW2 = ref refRW.ValueRW;
				if (!DTool.IsSameCamp(config.ShooterType, valueRW2.unitCfg.unitType) && valueRW2.unitCfg.unitType != UnitType.Brittleness && !valueRW2.isDead && element.VenomDuration > 0f)
				{
					valueRW2.SetVenom(element.VenomDuration, element.VenomApplyCount);
				}
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2004PillarOfLightData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2004PillarOfLightData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(chunkIndexInQuery, ref spell, entity, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr3, i));
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
					ref Spell2004PillarOfLightData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(chunkIndexInQuery, ref spell2, entity2, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr3, nextRangeBegin));
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
				ref Spell2004PillarOfLightData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(chunkIndexInQuery, ref spell3, entity3, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr3, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2004PillarOfLightData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(chunkIndexInQuery, ref spell4, entity4, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr3, k));
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
