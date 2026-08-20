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
[WithNone(new Type[] { typeof(SpellFallTag) })]
[CompilerGenerated]
public struct SpellHoverDamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			public ComponentTypeHandle<SpellHoverDamageData> __SpellHoverDamageData_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__SpellHoverDamageData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellHoverDamageData>();
			}

			public void Update(ref SystemState state)
			{
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__SpellHoverDamageData_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHoverDamageData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<SpellFallTag>();
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
		public void Run(ref SpellHoverDamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref SpellHoverDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref SpellHoverDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref SpellHoverDamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref SpellHoverDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref SpellHoverDamageJob job, EntityManager entityManager)
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
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public BufferLookup<StatefulTriggerEvent> HitTriggerLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public SpellSingleton SpellSingleton;

	public Entity SEData;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(SpellAspect spell, [ChunkIndexInQuery] int chunkIndexInQuery, ref SpellHoverDamageData hover)
	{
		bool flag = false;
		if (!HitTriggerLookup.TryGetBuffer(spell.Entity, out var bufferData))
		{
			return;
		}
		foreach (StatefulTriggerEvent item in bufferData)
		{
			if (item.State != StatefulEventState.Stay)
			{
				flag = true;
				break;
			}
		}
		if (bufferData.Length == 0 || flag)
		{
			hover.AttackTimer = 0f;
			return;
		}
		hover.AttackTimer += DeltaTime;
		if (!(hover.AttackTimer >= hover.Interval))
		{
			return;
		}
		hover.AttackTimer = 0f;
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref readonly float3 position = ref spell.Transform.ValueRO.Position;
		float radius = spell.Config.ValueRO.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in position, in radius, in spell.Config.ValueRW.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		TakeDamageInfo_Dots damage = spell.MakeDamageInfo(costPenetrate: false);
		bool flag2 = false;
		int num = 0;
		foreach (Entity item2 in entities)
		{
			Entity target = item2;
			spell.Config.ValueRW.Penetrate.CostPenetrateValue();
			num++;
			damage.spell.HitPosition = TransformLookup[target].Position;
			CMD.TryAttackEntity(chunkIndexInQuery, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
			if (spell.Config.ValueRW.Penetrate.Calculate() <= 0)
			{
				flag2 = true;
				break;
			}
		}
		if (num > 0 && hover.ShowHitEffect)
		{
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEData = SEData;
			SpellAbilityType abilityType = spell.Config.ValueRO.AbilityType;
			FixedString32Bytes seName = "Hit";
			cMD.AppendToBuffer(chunkIndexInQuery, sEData, new SEData(DTool.GetSpellSEName((int)abilityType, in seName)));
			ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
			ref SpellSingleton spellSingleton = ref SpellSingleton;
			ref readonly SpellConfigComponentData valueRO = ref spell.Config.ValueRO;
			ref readonly SpellComponentData valueRO2 = ref spell.Data.ValueRO;
			ref readonly float3 position2 = ref spell.Transform.ValueRO.Position;
			float3 direction = spell.Movement.ValueRO.Direction;
			cMD2.CreateSpellHitEffect(chunkIndexInQuery, in spellSingleton, in valueRO, in valueRO2, in position2, in direction, spell.Transform.ValueRO.Scale);
		}
		if (flag2)
		{
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndexInQuery, spell.Entity, value: true);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellHoverDamageData_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				SpellAspect spell = resolvedChunk[i];
				Execute(spell, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellHoverDamageData>(nativeArrayPtr, i));
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
					Execute(spell2, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellHoverDamageData>(nativeArrayPtr, nextRangeBegin));
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
				Execute(spell3, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellHoverDamageData>(nativeArrayPtr, j));
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
				Execute(spell4, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellHoverDamageData>(nativeArrayPtr, k));
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
