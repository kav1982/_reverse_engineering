using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Spell1010SnakeOnGroundDamageJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public BufferTypeHandle<SnakeTouchGroundPoint> __SnakeTouchGroundPoint_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1010SnakeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SnakeTouchGroundPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeTouchGroundPoint>();
			}

			public void Update(ref SystemState state)
			{
				__Spell1010SnakeData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SnakeTouchGroundPoint_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1010SnakeData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeTouchGroundPoint>();
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
		public void Run(ref Spell1010SnakeOnGroundDamageJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1010SnakeOnGroundDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1010SnakeOnGroundDamageJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1010SnakeOnGroundDamageJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1010SnakeOnGroundDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1010SnakeOnGroundDamageJob job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public Entity SEPlayerSingleton;

	[ReadOnly]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public Entity GlobalParticleSystemBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell1010SnakeData data, Entity spellEntity, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, in SpellComponentData componentData, DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < touchGroundPoints.Length; i++)
		{
			ref SnakeTouchGroundPoint reference = ref touchGroundPoints.ElementAt(i);
			reference.currentDamageLoopTime += DeltaTime;
			if (reference.currentDamageLoopTime > data.OnGroundDmgLoop)
			{
				DoDamage(spellEntity, in config, in movement, in transform, in elementEffect, in componentData, reference, chunkIndex);
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				SpellAbilityType abilityType = config.AbilityType;
				FixedString32Bytes seName = "Hit";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName((int)abilityType, in seName)));
				reference.currentDamageLoopTime -= data.OnGroundDmgLoop;
			}
		}
	}

	[BurstCompile]
	private void DoDamage(Entity spellEntity, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, in SpellComponentData data, SnakeTouchGroundPoint point, [ChunkIndexInQuery] int chunkIndex)
	{
		float3 rootPosition = point.Value;
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			rootPosition += movement.AroundCenter;
		}
		if (config.ColorType == SpellColorType.Player)
		{
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = "1010_FallExplosion_Player";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(rootPosition) + layerPosition;
			globalParticleEmitParams.Size = config.Radius.Calculate();
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}
		else
		{
			config.ColorType.ColorEnumToString(out var result);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"3119_Fall_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(rootPosition.xy, 1.08f);
			globalParticleEmitParams.Size = config.Radius.Calculate();
			GlobalParticleEmitParams element2 = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element2);
		}
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		float radius = config.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in rootPosition, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		foreach (Entity item in entities)
		{
			Entity target = item;
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in data, out var info, CostRefraction: false);
			info.spell.HitPosition = rootPosition;
			info.spell.IgnoreHitEffect = true;
			CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1010SnakeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		BufferAccessor<SnakeTouchGroundPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeTouchGroundPoint_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1010SnakeData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, i);
				Entity spellEntity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr7, i);
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints = bufferAccessor[i];
				Execute(ref data, spellEntity, in config, in movement, in transform, in elementEffect, in componentData, touchGroundPoints, chunkIndexInQuery);
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
					ref Spell1010SnakeData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, nextRangeBegin);
					Entity spellEntity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr7, nextRangeBegin);
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints2 = bufferAccessor[nextRangeBegin];
					Execute(ref data2, spellEntity2, in config2, in movement2, in transform2, in elementEffect2, in componentData2, touchGroundPoints2, chunkIndexInQuery);
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
				ref Spell1010SnakeData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, j);
				Entity spellEntity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr7, j);
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints3 = bufferAccessor[j];
				Execute(ref data3, spellEntity3, in config3, in movement3, in transform3, in elementEffect3, in componentData3, touchGroundPoints3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1010SnakeData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, k);
				Entity spellEntity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr7, k);
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints4 = bufferAccessor[k];
				Execute(ref data4, spellEntity4, in config4, in movement4, in transform4, in elementEffect4, in componentData4, touchGroundPoints4, chunkIndexInQuery);
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
