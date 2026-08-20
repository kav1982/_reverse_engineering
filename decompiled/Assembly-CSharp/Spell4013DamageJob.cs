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

[BurstCompile]
[CompilerGenerated]
public struct Spell4013DamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell4013HitTriggerData> __Spell4013HitTriggerData_RW_ComponentTypeHandle;

			public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell4013HitTriggerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4013HitTriggerData>();
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell4013HitTriggerData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4013HitTriggerData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
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
		public void Run(ref Spell4013DamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4013DamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4013DamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4013DamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4013DamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4013DamageJob job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellDecreaseRadiusData> DecreaseRadiusLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellMovementComponentData> MovementLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell4013RuneHammerData> HammerLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellComponentData> ComponentLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

	public Entity ScreenShakeSingleton;

	public Entity SEPlayerSingleton;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity GlobalParticleBuffer;

	public Entity Spell3101Buffer;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private bool IsAttackableSpell(Entity other)
	{
		if ((!SpellConfigLookup.TryGetComponent(other, out var componentData) || componentData.AbilityType != SpellAbilityType.Butterfly) && componentData.AbilityType != SpellAbilityType.Bat)
		{
			return componentData.AbilityType == SpellAbilityType.Rollball;
		}
		return true;
	}

	[BurstCompile]
	private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spell4013HitTriggerData trigger, ref DynamicBuffer<StatefulTriggerEvent> triggerEvent, ref LocalTransform transform, Entity entity)
	{
		if (trigger.Spell == Entity.Null)
		{
			return;
		}
		SpellConfigComponentData config = SpellConfigLookup[trigger.Spell];
		SpellMovementComponentData movement = MovementLookUp[trigger.Spell];
		config.ColorType.ColorEnumToString(out var result);
		SpellElementEffectComponentData elementEffect = SpellElementLookup[trigger.Spell];
		float3 position = LocalTransformLookUp[trigger.Parent].Position;
		SpellComponentData data = ComponentLookup[trigger.Spell];
		bool flag = false;
		bool flag2 = false;
		trigger.ThunderDamageTimer -= DeltaTime;
		foreach (StatefulTriggerEvent item in triggerEvent)
		{
			Entity otherEntity = item.GetOtherEntity(entity);
			if (!UnitPropertyLookup.HasComponent(otherEntity) && !IsAttackableSpell(otherEntity))
			{
				continue;
			}
			if (item.State == StatefulEventState.Enter)
			{
				if (TryAttackTargetEntity(chunkIndex, ref trigger, in transform, otherEntity, position, result, in config, in movement, in elementEffect, in data))
				{
					flag2 = true;
				}
			}
			else if (item.State == StatefulEventState.Stay)
			{
				flag = true;
			}
		}
		if (flag)
		{
			trigger.DamageTimer += DeltaTime;
			if (trigger.DamageTimer >= 0.5f)
			{
				trigger.DamageTimer -= 0.5f;
				foreach (StatefulTriggerEvent item2 in triggerEvent)
				{
					Entity otherEntity2 = item2.GetOtherEntity(entity);
					if (item2.State == StatefulEventState.Stay && (UnitPropertyLookup.HasComponent(otherEntity2) || IsAttackableSpell(otherEntity2)) && TryAttackTargetEntity(chunkIndex, ref trigger, in transform, otherEntity2, position, result, in config, in movement, in elementEffect, in data))
					{
						flag2 = true;
					}
				}
			}
		}
		else
		{
			trigger.DamageTimer = 0f;
		}
		if (flag2)
		{
			CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
			{
				Radius = 0.2f,
				Speed = 0.3f,
				Time = 0.15f
			});
		}
	}

	[BurstCompile]
	private bool TryAttackTargetEntity(int chunkIndex, ref Spell4013HitTriggerData trigger, in LocalTransform transform, Entity other, float3 parentPos, FixedString32Bytes colorName, in SpellConfigComponentData config, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, in SpellComponentData data)
	{
		TakeDamageInfo_Dots.NewInfo(trigger.Spell, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in data, out var info);
		if (DecreaseRadiusLookup.TryGetComponent(trigger.Spell, out var componentData))
		{
			float hammerLength = HammerLookup[trigger.Spell].HammerLength;
			info.damage *= GeneralTool.GetSpellRadiusToDamageRatio(hammerLength * componentData.RadiusMult, componentData.RadiusMult, componentData.RadiusToDamageRatio);
		}
		float3 position = LocalTransformLookUp[other].Position;
		info.spell.HitPosition = position;
		if (CMD.TryAttackEntity(chunkIndex, in other, in info, in UnitPropertyLookup, in SpellConfigLookup) == SpellTools.HitType.Unit)
		{
			CMD.AppendToBuffer(chunkIndex, GlobalParticleBuffer, new GlobalParticleEmitParams(GlobalParticleType.Spell, $"4013_Hit_{colorName}", position)
			{
				Size = transform.Scale,
				Velocity = DTool.IgnoreZDir(in info.spell.HitPosition, in parentPos)
			});
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			FixedString32Bytes seName = "HitSE";
			cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(4013, in seName)));
			if (trigger.ThunderDamageTimer <= 0f)
			{
				trigger.ThunderDamageTimer = 0.5f;
				CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, position, UnitPropertyLookup, Physics, in config, in movement, in transform, in elementEffect, in data, trigger.Spell);
			}
			return true;
		}
		return false;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4013HitTriggerData_RW_ComponentTypeHandle);
		BufferAccessor<StatefulTriggerEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell4013HitTriggerData trigger = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013HitTriggerData>(nativeArrayPtr, i);
				DynamicBuffer<StatefulTriggerEvent> triggerEvent = bufferAccessor[i];
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
				Execute(chunkIndexInQuery, ref trigger, ref triggerEvent, ref transform, entity);
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
					ref Spell4013HitTriggerData trigger2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013HitTriggerData>(nativeArrayPtr, nextRangeBegin);
					DynamicBuffer<StatefulTriggerEvent> triggerEvent2 = bufferAccessor[nextRangeBegin];
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
					Execute(chunkIndexInQuery, ref trigger2, ref triggerEvent2, ref transform2, entity2);
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
				ref Spell4013HitTriggerData trigger3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013HitTriggerData>(nativeArrayPtr, j);
				DynamicBuffer<StatefulTriggerEvent> triggerEvent3 = bufferAccessor[j];
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
				Execute(chunkIndexInQuery, ref trigger3, ref triggerEvent3, ref transform3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell4013HitTriggerData trigger4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013HitTriggerData>(nativeArrayPtr, k);
				DynamicBuffer<StatefulTriggerEvent> triggerEvent4 = bufferAccessor[k];
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
				Execute(chunkIndexInQuery, ref trigger4, ref triggerEvent4, ref transform4, entity4);
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
