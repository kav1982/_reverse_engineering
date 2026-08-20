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
public struct Spell1010SnakeTouchGroundJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public BufferTypeHandle<SnakeTouchGroundPoint> __SnakeTouchGroundPoint_RW_BufferTypeHandle;

			public ComponentTypeHandle<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellGroundedTag> __SpellGroundedTag_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SnakeTouchGroundPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeTouchGroundPoint>();
				__Spell1010SnakeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
				__SpellGroundedTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellGroundedTag>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__SnakeTouchGroundPoint_RW_BufferTypeHandle.Update(ref state);
				__Spell1010SnakeData_RW_ComponentTypeHandle.Update(ref state);
				__SpellGroundedTag_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeTouchGroundPoint>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1010SnakeData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGroundedTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
		public void Run(ref Spell1010SnakeTouchGroundJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1010SnakeTouchGroundJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1010SnakeTouchGroundJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1010SnakeTouchGroundJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1010SnakeTouchGroundJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1010SnakeTouchGroundJob job, EntityManager entityManager)
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
	public ComponentLookup<LocalTransform> LocalTransformLookup;

	[ReadOnly]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellRefractionData> SpellRefractionLookup;

	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractionHitEntitiesLookup;

	[ReadOnly]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public Entity GlobalParticleSystemBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints, ref Spell1010SnakeData data, SpellGroundedTag _, Entity entity, ref SpellMovementComponentData movement, in SpellComponentData spellComponentData, in SpellConfigComponentData configComponentData, ref PhysicsVelocity velocity, [ChunkIndexInQuery] int chunkIndex)
	{
		CMD.SetComponentEnabled<SpellGroundedTag>(chunkIndex, entity, value: false);
		RefRW<LocalTransform> refRW = LocalTransformLookup.GetRefRW(entity);
		ref LocalTransform valueRW = ref refRW.ValueRW;
		SnakeTouchGroundPoint snakeTouchGroundPoint = default(SnakeTouchGroundPoint);
		snakeTouchGroundPoint.Value = valueRW.Position;
		SnakeTouchGroundPoint elem = snakeTouchGroundPoint;
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			elem.Value -= movement.AroundCenter;
		}
		elem.distanceToHead = 0f;
		elem.currentDamageLoopTime = data.OnGroundDmgLoop;
		touchGroundPoints.Insert(0, elem);
		configComponentData.ColorType.ColorEnumToString(out var result);
		PlayUnfollowEffect("Fall", elem.Value, result, 0f, chunkIndex);
		if (SpellRefractionLookup.HasComponent(entity))
		{
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			ref SpellRefractionData valueRW2 = ref SpellRefractionLookup.GetRefRW(entity).ValueRW;
			if (valueRW2.RemainCount > 0)
			{
				ref float3 value = ref elem.Value;
				float radius = configComponentData.Radius.Calculate();
				SpellTools.GetAttackableEntitiesInRange(in value, in radius, in configComponentData.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities, checkUnitCamp: false);
				NativeList<Entity> hitList = new NativeList<Entity>(Allocator.Temp);
				if (entities.Length > 0)
				{
					foreach (Entity item in entities)
					{
						Entity target = item;
						if (SpellTools.GetEntityHitType(in target, in configComponentData.ShooterType, in UnitPropertyLookup, in SpellConfigLookup) == SpellTools.HitType.Unit)
						{
							hitList.Add(in target);
						}
					}
					if (TryRefract(ref valueRW2, configComponentData, ref movement, valueRW, entity, hitList, ref velocity))
					{
						return;
					}
				}
			}
		}
		if (movement.ReboundCount <= 0)
		{
			float x = movement.Speed * movement.Speed + movement.CurrentFallSpeed * movement.CurrentFallSpeed;
			data.OnGroundSpeed = math.sqrt(x);
			data.OnGroundDisapperDistance = 0f;
			movement.Speed = 0f;
			movement.CurrentFallSpeed = 0f;
			movement.Gravity = 0f;
			movement.OriginalSpellHorizontalSpeed = 0f;
			movement.ChaseMouseLerpSpeed = 0f;
			velocity.Linear = float3.zero;
			data.IsFadingTail = true;
			if (LocalTransformLookup.HasComponent(spellComponentData.SpellEffectEntity))
			{
				refRW = LocalTransformLookup.GetRefRW(spellComponentData.SpellEffectEntity);
				refRW.ValueRW.Scale = 0f;
			}
		}
		else
		{
			movement.ReboundFallSpeed();
			movement.ReboundCount--;
		}
	}

	private void PlayUnfollowEffect(string name, float3 position, FixedString32Bytes colorName, float rotation, [ChunkIndexInQuery] int chunkIndex)
	{
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"1010_Fall_{colorName}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(position) + layerPosition;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
	}

	private bool TryRefract(ref SpellRefractionData refract, SpellConfigComponentData config, ref SpellMovementComponentData movement, LocalTransform transform, Entity spell, NativeList<Entity> hitList, ref PhysicsVelocity velocity)
	{
		if (hitList.Length > 0)
		{
			NativeHashSet<Entity> ignoreEntities = new NativeHashSet<Entity>(hitList.Length, Allocator.Temp);
			if (SpellRefractionHitEntitiesLookup.TryGetBuffer(spell, out var bufferData))
			{
				foreach (Entity item in hitList)
				{
					bufferData.Add(new SpellRefractionHitEntities
					{
						Entity = item
					});
				}
				foreach (SpellRefractionHitEntities item2 in bufferData)
				{
					ignoreEntities.Add(item2.Entity);
				}
			}
			Entity target;
			float3 targetPosition;
			UnitProperty_Dots targetPpt;
			bool flag = CurrentRoomEntities.FindReflectionTarget(transform.Position, config.ShooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
			if (!flag)
			{
				bufferData.Clear();
				ignoreEntities.Clear();
				foreach (Entity item3 in hitList)
				{
					bufferData.Add(new SpellRefractionHitEntities
					{
						Entity = item3
					});
					ignoreEntities.Add(item3);
				}
				flag = CurrentRoomEntities.FindReflectionTarget(transform.Position, config.ShooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
			}
			if (flag)
			{
				refract.RemainCount--;
				movement.Direction = DTool.IgnoreZDir(in targetPosition, in transform.Position);
				velocity.Linear = movement.Direction * movement.Speed;
				movement.ReboundFallSpeed();
				return true;
			}
		}
		return false;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		BufferAccessor<SnakeTouchGroundPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeTouchGroundPoint_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1010SnakeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints = bufferAccessor[i];
				ref Spell1010SnakeData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(touchGroundPoints, ref data, default(SpellGroundedTag), entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, i), chunkIndexInQuery);
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
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints2 = bufferAccessor[nextRangeBegin];
					ref Spell1010SnakeData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(touchGroundPoints2, ref data2, default(SpellGroundedTag), entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, nextRangeBegin), chunkIndexInQuery);
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
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints3 = bufferAccessor[j];
				ref Spell1010SnakeData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(touchGroundPoints3, ref data3, default(SpellGroundedTag), entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, j), chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints4 = bufferAccessor[k];
				ref Spell1010SnakeData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(touchGroundPoints4, ref data4, default(SpellGroundedTag), entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, k), chunkIndexInQuery);
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
