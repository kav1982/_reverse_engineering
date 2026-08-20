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
public struct Spell1007FallJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellGroundedTag> __SpellGroundedTag_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1007BlackHoleData> __Spell1007BlackHoleData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellGroundedTag_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellGroundedTag>(isReadOnly: true);
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
				__Spell1007BlackHoleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1007BlackHoleData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellGroundedTag_RO_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Spell1007BlackHoleData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellGroundedTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1007BlackHoleData>();
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
		public void Run(ref Spell1007FallJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1007FallJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1007FallJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1007FallJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1007FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1007FallJob job, EntityManager entityManager)
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

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellRefractionData> SpellRefractLookUp;

	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractHitEntitiesLookUp;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	public SpellSingleton SpellSingleton;

	public Entity ScreenShakeSingleton;

	public Entity seDataSingleton;

	public Entity EffectEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref SpellConfigComponentData config, in SpellGroundedTag ground, ref SpellComponentData data, in LocalTransform transform, ref SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex, Spell1007BlackHoleData _, Entity entity)
	{
		config.ColorType.ColorEnumToString(out var result);
		CMD.AppendToBuffer(chunkIndex, EffectEntity, new SpellEffectSystem.UnfollowingRequire
		{
			Scale = config.Radius.Calculate(),
			StartPosition = transform.Position,
			StartRotation = quaternion.identity,
			SpellId = 1007,
			Color = result,
			Settings = new SpellEffect
			{
				DestroyDelay = 1f,
				Name = "Explosion",
				Layer = LayerCorrectType.Coordinate
			}
		});
		CMD.AppendToBuffer(chunkIndex, EffectEntity, new SpellEffectSystem.UnfollowingRequire
		{
			Scale = config.Radius.Calculate(),
			StartPosition = transform.Position,
			StartRotation = quaternion.identity,
			SpellId = 1007,
			Color = result,
			Settings = new SpellEffect
			{
				DestroyDelay = 1f,
				Name = "ExplosionB",
				Layer = LayerCorrectType.Coordinate
			}
		});
		ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
		Entity e = seDataSingleton;
		FixedString32Bytes seName = "FallingGround";
		cMD.AppendToBuffer(chunkIndex, e, new SEData(DTool.GetSpellSEName(1007, in seName)));
		CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
		{
			Radius = 0.1f,
			Speed = 5f,
			Time = 0.2f
		});
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref readonly float3 position = ref transform.Position;
		float radius = config.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in data, out var info);
		bool flag = false;
		foreach (Entity item in entities)
		{
			Entity target = item;
			float3 position2 = TransformLookup[target].Position;
			info.spell.HitPosition = position2;
			info.SetKnockbackForceIgnoreZBySpell(transform.Position - position2);
			info.knockbackForce *= 4f;
			info.spell.IgnoreHitEffect = true;
			SpellTools.HitType hitType = CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
			float3 rootPosition = position2 + new float3(0f, 0f, -0.3f);
			float3 position3 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			position3 += position2 + new float3(0f, 0f, -0.3f);
			ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
			seName = "Hit";
			radius = 1f;
			cMD2.CreateSpellGlobalParticle(chunkIndex, in seName, in position3, in radius, in config, in data, in SpellSingleton, in float3.zero);
			flag = flag || hitType == SpellTools.HitType.Unit;
		}
		ref readonly float3 position4 = ref transform.Position;
		UnitType shooterType = config.ShooterType;
		ref ComponentLookup<SpellRefractionData> spellRefractLookUp = ref SpellRefractLookUp;
		ref BufferLookup<SpellRefractionHitEntities> spellRefractHitEntitiesLookUp = ref SpellRefractHitEntitiesLookUp;
		ref CurrentRoomEntitiesSingleton currentRoomEntities = ref CurrentRoomEntities;
		NativeArray<Entity> theEntitiesHitByThisDamage = entities.ToArray(Allocator.Temp);
		if (!SpellTools.TryRefractOrReboundWhenFall(in entity, in position4, shooterType, in spellRefractLookUp, in spellRefractHitEntitiesLookUp, ref movement, in currentRoomEntities, in theEntitiesHitByThisDamage, flag))
		{
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1007BlackHoleData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, i);
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, i);
				ref Spell1007BlackHoleData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1007BlackHoleData>(nativeArrayPtr6, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, i);
				SpellGroundedTag ground = default(SpellGroundedTag);
				Execute(ref config, in ground, ref data, in transform, ref movement, in elementEffect, chunkIndexInQuery, reference, entity);
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
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref Spell1007BlackHoleData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1007BlackHoleData>(nativeArrayPtr6, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, nextRangeBegin);
					SpellGroundedTag ground = default(SpellGroundedTag);
					Execute(ref config2, in ground, ref data2, in transform2, ref movement2, in elementEffect2, chunkIndexInQuery, reference2, entity2);
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
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, j);
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, j);
				ref Spell1007BlackHoleData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1007BlackHoleData>(nativeArrayPtr6, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, j);
				SpellGroundedTag ground = default(SpellGroundedTag);
				Execute(ref config3, in ground, ref data3, in transform3, ref movement3, in elementEffect3, chunkIndexInQuery, reference3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, k);
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, k);
				ref Spell1007BlackHoleData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1007BlackHoleData>(nativeArrayPtr6, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, k);
				SpellGroundedTag ground = default(SpellGroundedTag);
				Execute(ref config4, in ground, ref data4, in transform4, ref movement4, in elementEffect4, chunkIndexInQuery, reference4, entity4);
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
