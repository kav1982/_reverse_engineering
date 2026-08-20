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
internal struct Spell1025DamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public ComponentTypeHandle<Spell1025DragonBreathData> __Spell1025DragonBreathData_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__Spell1025DragonBreathData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025DragonBreathData>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__Spell1025DragonBreathData_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1025DragonBreathData>();
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
		public void Run(ref Spell1025DamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1025DamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1025DamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1025DamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1025DamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1025DamageJob job, EntityManager entityManager)
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

	public Unity.Mathematics.Random random;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellDecreaseRadiusData> DecreaseRadiusLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public BufferLookup<Spell1025FireGroundEffectBuffer> GroundLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell4005WandSpiritData> WandSpiritLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public SpellSingleton SpellSingleton;

	public float DeltaTime;

	public DynamicOptimizeData OptimizeData;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(SpellAspect spell, ref Spell1025DragonBreathData data, [ChunkIndexInQuery] int chunkIndex)
	{
		if (spell.Movement.ValueRO.IsFallSpell)
		{
			float num = 1f;
			if (OptimizeData.IsLowFpsOptimizeActive(60f))
			{
				num = OptimizeData.GetLowFrameDamageIntervalTimeScale(60f, 10f, 15f);
			}
			spell.Config.ValueRW.DamageTimer += DeltaTime;
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			while (spell.Config.ValueRW.DamageTimer > spell.Config.ValueRO.DamageInterval * num && spell.Config.ValueRW.DurationTimer > 0.35f)
			{
				spell.Config.ValueRW.DamageTimer -= spell.Config.ValueRO.DamageInterval * num;
				if (!GroundLookUp.TryGetBuffer(spell.Entity, out var bufferData))
				{
					continue;
				}
				foreach (Spell1025FireGroundEffectBuffer item in bufferData)
				{
					Spell1025FireGroundEffectBuffer current = item;
					SpellTools.GetAttackableEntitiesInRange(in current.position, in data.FallDamageRange, in spell.Config.ValueRW.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
					TakeDamageInfo_Dots damage = spell.MakeDamageInfo(costPenetrate: false);
					damage.damage = spell.Config.ValueRW.Damage.Calculate() * num * spell.Config.ValueRW.DamageInterval;
					if (DecreaseRadiusLookup.TryGetComponent(spell.Entity, out var componentData))
					{
						damage.damage *= GeneralTool.GetSpellRadiusToDamageRatio(data.FallDamageRange * componentData.RadiusMult, componentData.RadiusMult, componentData.RadiusToDamageRatio);
					}
					foreach (Entity item2 in entities)
					{
						Entity target = item2;
						float3 to = TransformLookUp[target].Position;
						damage.spell.HitPosition = to;
						damage.SetKnockbackForceIgnoreZBySpell(to - current.position);
						damage.spell.IgnoreHitEffect = true;
						float3 direction = DTool.IgnoreZDir(in to, in current.position);
						if (CMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup) == SpellTools.HitType.Unit && !WandSpiritLookup.HasComponent(target))
						{
							ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
							ref SpellSingleton spellSingleton = ref SpellSingleton;
							ref readonly SpellConfigComponentData valueRO = ref spell.Config.ValueRO;
							ref readonly SpellComponentData valueRO2 = ref spell.Data.ValueRO;
							float3 position = to + new float3(1f, 1f, 0f) * random.NextFloat(-0.06f, 0.06f);
							cMD.CreateSpellHitEffect(chunkIndex, in spellSingleton, in valueRO, in valueRO2, in position, in direction, 1f);
						}
					}
					entities.Clear();
				}
			}
			return;
		}
		ref SpellConfigComponentData valueRW = ref spell.Config.ValueRW;
		ref SpellMovementComponentData valueRW2 = ref spell.Movement.ValueRW;
		ref LocalTransform valueRW3 = ref spell.Transform.ValueRW;
		SpellTools.GetSpellElementDataWithTimeScale(in spell.ElementEffect.ValueRO, in OptimizeData, out var result);
		valueRW.DamageTimer += DeltaTime;
		if (valueRW.DamageTimer < valueRW.DamageInterval)
		{
			return;
		}
		valueRW.DamageTimer -= valueRW.DamageInterval;
		float radius = ((valueRW2.Type == SpellSpecialMovementType.Rotation) ? (valueRW2.AroundRadius + 1f) : data.currentAttackDistance);
		float3 position2 = ((valueRW2.Type != SpellSpecialMovementType.Rotation) ? valueRW3.Position : valueRW2.AroundCenter);
		NativeList<Entity> entities2 = new NativeList<Entity>(Allocator.Temp);
		SpellTools.GetAttackableEntitiesInRange(in position2, in radius, in spell.Config.ValueRW.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities2);
		TakeDamageInfo_Dots damage2 = spell.MakeDamageInfo(costPenetrate: false);
		if (DecreaseRadiusLookup.TryGetComponent(spell.Entity, out var componentData2))
		{
			damage2.damage *= GeneralTool.GetSpellRadiusToDamageRatio(radius * componentData2.RadiusMult, componentData2.RadiusMult, componentData2.RadiusToDamageRatio);
		}
		float3 from = ((valueRW2.Type == SpellSpecialMovementType.Rotation) ? valueRW2.AroundCenter : spell.Transform.ValueRW.Position);
		foreach (Entity item3 in entities2)
		{
			Entity target2 = item3;
			float3 to2 = TransformLookUp[target2].Position;
			if (CanAttack(to2, in valueRW2, in valueRW, position2, ref data))
			{
				damage2.spell.HitPosition = to2;
				damage2.spell.IgnoreHitEffect = true;
				damage2.spell.ElementEffect = result;
				damage2.SetKnockbackForceIgnoreZBySpell(to2 - position2);
				float3 direction2 = DTool.IgnoreZDir(in to2, in from);
				if (CMD.TryAttackEntity(chunkIndex, in target2, in damage2, in UnitPropertyLookup, in SpellConfigLookup) == SpellTools.HitType.Unit && !WandSpiritLookup.HasComponent(target2))
				{
					ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
					ref SpellSingleton spellSingleton2 = ref SpellSingleton;
					ref readonly SpellConfigComponentData valueRO3 = ref spell.Config.ValueRO;
					ref readonly SpellComponentData valueRO4 = ref spell.Data.ValueRO;
					float3 position = to2 + new float3(1f, 1f, 0f) * random.NextFloat(-0.06f, 0.06f);
					cMD2.CreateSpellHitEffect(chunkIndex, in spellSingleton2, in valueRO3, in valueRO4, in position, in direction2, 1f);
				}
			}
		}
	}

	[BurstCompile]
	private bool CanAttack(float3 pos, in SpellMovementComponentData movement, in SpellConfigComponentData config, float3 center, ref Spell1025DragonBreathData spell)
	{
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			float num = movement.AroundRadius - 1f;
			num *= num;
			return Tool2D.IgnoreZDistanceSqr(pos, center) >= num;
		}
		if (Tool2D.IgnoreZDistanceSqr(pos, center) <= 1f)
		{
			return true;
		}
		float angleBetweenTwoDirection = Tool2D.GetAngleBetweenTwoDirection(movement.Direction, Tool2D.IgnoreZPoint(pos) - Tool2D.IgnoreZPoint(GetAngleCheckCenterPoint(center, ref spell, in config)));
		float num2 = math.max(config.Scatter / 2f, 30f);
		if (angleBetweenTwoDirection < num2)
		{
			return Tool2D.IgnoreZDistanceSqr(pos, center) <= spell.maxAttackDistance * spell.maxAttackDistance;
		}
		return false;
	}

	[BurstCompile]
	private float3 GetAngleCheckCenterPoint(float3 center, ref Spell1025DragonBreathData spell, in SpellConfigComponentData config)
	{
		if (config.Scatter >= 90f)
		{
			return center;
		}
		return Tool2D.IgnoreZPoint(center - math.normalize(spell.LastFrameDirection) * math.tan(config.Scatter / 2f * (MathF.PI / 180f)));
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1025DragonBreathData_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spell = resolvedChunk[i];
				Execute(spell, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr, i), chunkIndexInQuery);
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
					Execute(spell2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr, nextRangeBegin), chunkIndexInQuery);
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
				Execute(spell3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr, j), chunkIndexInQuery);
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
				Execute(spell4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr, k), chunkIndexInQuery);
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
