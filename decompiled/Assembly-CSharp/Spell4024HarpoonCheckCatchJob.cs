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

[CompilerGenerated]
[BurstCompile]
public struct Spell4024HarpoonCheckCatchJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<Spell4024DaveHarpoonData> __Spell4024DaveHarpoonData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle;

			public BufferTypeHandle<StatefulCollisionEvent> __Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle;

			public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Spell4024DaveHarpoonData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>();
				__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulCollisionEvent>();
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Spell4024DaveHarpoonData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4024DaveHarpoonData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulCollisionEvent>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
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
		public void Run(ref Spell4024HarpoonCheckCatchJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4024HarpoonCheckCatchJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4024HarpoonCheckCatchJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4024HarpoonCheckCatchJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4024HarpoonCheckCatchJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4024HarpoonCheckCatchJob job, EntityManager entityManager)
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

	public GlobalRandom Random;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	public Entity SEPlayerSingleton;

	[ReadOnly]
	public EntityStorageInfoLookup EntityExists;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public Entity Spell3101Buffer;

	public LightningHarpoonRelicData RelicData;

	public float DeltaTime;

	public SpellSingleton SpellSingleton;

	public Entity GlobalParticleSystemBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, ref SpellComponentData componentData, DynamicBuffer<StatefulTriggerEvent> triggerBuffer, DynamicBuffer<StatefulCollisionEvent> collisionBuffer, ref PhysicsCollider collider, ref SpellElementEffectComponentData elementEffect, ref PhysicsVelocity velocity)
	{
		if (data.HarpoonState == HarpoonState.Shooting)
		{
			if (triggerBuffer.Length > 0 && IsCatchHitWhenShoot(triggerBuffer, entity, ref transform, ref data, in movement, ref config, ref collider, chunkIndex))
			{
				CheckThunderRelic(chunkIndex, entity, ref data, in transform, in movement, in componentData, in config, in elementEffect);
			}
			else if (collisionBuffer.Length > 0 && WallHitWhenShoot(collisionBuffer, ref data, ref transform, ref movement, ref velocity, ref config, ref collider))
			{
				CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
			}
		}
	}

	private bool IsCatchHitWhenShoot(DynamicBuffer<StatefulTriggerEvent> triggerBuffer, Entity spellEntity, ref LocalTransform transform, ref Spell4024DaveHarpoonData data, in SpellMovementComponentData movement, ref SpellConfigComponentData config, ref PhysicsCollider collider, int chunkIndex)
	{
		bool flag = false;
		foreach (StatefulTriggerEvent item in triggerBuffer)
		{
			if (!CheckHitCanCatch(item, spellEntity, out var hitEntity) || !TransformLookup.TryGetComponent(hitEntity, out var componentData))
			{
				continue;
			}
			if (TransformLookup.TryGetComponent(data.CatchEntity, out var componentData2))
			{
				if (math.lengthsq(componentData2.Position.xy - data.StartPos.xy) > math.lengthsq(componentData.Position.xy - data.StartPos.xy))
				{
					flag = true;
					data.CatchEntity = hitEntity;
				}
			}
			else
			{
				flag = true;
				data.CatchEntity = hitEntity;
			}
		}
		if (flag)
		{
			HarpoonGoCatch(ref data, ref config, in movement, ref transform, chunkIndex);
			ChainGoCatching(ref data);
			SpellTools.DisableSpellReboundCollider(in collider);
			SpellTools.DisableSpellTrigger(in collider);
			return true;
		}
		return false;
	}

	private void HarpoonGoCatch(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config, in SpellMovementComponentData movement, ref LocalTransform transform, int chunkIndex)
	{
		config.DurationTimer = 0f;
		PlayGlobalEffectDirectionScale(chunkIndex, "DirectHit", 4024, transform.Position, math.normalizesafe(new float3(movement.Direction.xy, 0f)), 1f, in config);
		ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
		Entity sEPlayerSingleton = SEPlayerSingleton;
		FixedString32Bytes seName = "HarpoonHIt";
		cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1030, in seName)));
		data.HarpoonHideRate = -999f;
		if (EntityExists.Exists(data.CatchEntity) && TransformLookup.TryGetComponent(data.CatchEntity, out var componentData))
		{
			data.HarpoonState = HarpoonState.Catching;
			data.CatchPosRandom = Random.random.NextFloat3(new float3(-0.2f, -0.2f, -0.2f), new float3(0.2f, 0.2f, -0.2f));
			transform.Position = componentData.Position + data.CatchPosRandom;
		}
		else
		{
			data.CatchEntity = Entity.Null;
			data.HarpoonState = HarpoonState.Waiting;
		}
	}

	[BurstCompile]
	private void PlayGlobalEffectDirectionScale([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes name, int id, float3 position, float3 direction, float scale, in SpellConfigComponentData config)
	{
		config.ColorType.ColorEnumToString(out var result);
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"{id}_{name}_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(position) + layerPosition;
		globalParticleEmitParams.Velocity = direction;
		globalParticleEmitParams.Size = scale;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
	}

	private void PlayGlobalEffect([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes name, int id, float3 position, in SpellConfigComponentData config)
	{
		config.ColorType.ColorEnumToString(out var result);
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"{id}_{name}_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(position) + layerPosition;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
	}

	private void ChainGoCatching(ref Spell4024DaveHarpoonData data)
	{
		data.ChainState = ChainState.Catching;
		data.VelocityDampen = 0.9f;
		data.AllowStretch = true;
		data.EndPinned = true;
	}

	private void CheckThunderStone(int chunkIndex, Entity entity, ref Spell4024DaveHarpoonData data, in LocalTransform transform, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellConfigComponentData config, in SpellElementEffectComponentData elementEffect)
	{
		if (config.ColorType == SpellColorType.Thunder)
		{
			CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, transform.Position, UnitPropertyLookup, PhysicsWorld, in config, in movement, in transform, in elementEffect, in componentData, entity);
		}
	}

	private bool WallHitWhenShoot(DynamicBuffer<StatefulCollisionEvent> collisionBuffer, ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref SpellMovementComponentData movement, ref PhysicsVelocity velocity, ref SpellConfigComponentData config, ref PhysicsCollider collider)
	{
		foreach (StatefulCollisionEvent item in collisionBuffer)
		{
			StatefulCollisionEvent collEvent = item;
			transform.Position = collEvent.CollisionDetails.AverageContactPointPosition;
			if (data.RebounceTimeCurrent <= 0)
			{
				HarpoonGoWallRebouncing(ref data, ref config, in movement, in collEvent, in collider);
				return true;
			}
			movement.Direction = math.normalizesafe(velocity.Linear);
			data.RebounceTimeCurrent--;
		}
		return false;
	}

	private void HarpoonGoWallRebouncing(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config, in SpellMovementComponentData movement, in StatefulCollisionEvent collEvent, in PhysicsCollider collider)
	{
		config.DurationTimer = 0f;
		data.HarpoonState = HarpoonState.WallHitRebouncing;
		float degree = Random.random.NextFloat(-70f, 70f);
		data.WallHitReboudVelocity = DTool.RotateDir(collEvent.Normal, degree) * 10f;
		degree = Random.random.NextFloat(5f, 20f);
		data.RebounceRotateRandomResult = degree * (Random.random.NextBool() ? 1f : (-1f));
		SpellTools.DisableSpellReboundCollider(in collider);
		SpellTools.DisableSpellTrigger(in collider);
	}

	private bool CheckHitCanCatch(StatefulTriggerEvent triggerEvent, Entity spellEntity, out Entity hitEntity)
	{
		hitEntity = triggerEvent.GetOtherEntity(spellEntity);
		if (!CheckUnitPropertyExist(hitEntity))
		{
			return false;
		}
		UnitType unitType = UnitPropertyLookup[hitEntity].unitCfg.unitType;
		if ((uint)(unitType - 3) <= 3u)
		{
			return true;
		}
		return false;
	}

	private bool CheckUnitPropertyExist(Entity entity)
	{
		if (!EntityExists.Exists(entity))
		{
			return false;
		}
		if (!UnitPropertyLookup.HasComponent(entity))
		{
			return false;
		}
		return true;
	}

	private void CheckThunderRelic(int chunkIndex, Entity spellEntity, ref Spell4024DaveHarpoonData data, in LocalTransform transform, in SpellMovementComponentData move, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellElementEffectComponentData element)
	{
		if (!(RelicData.DamageRate <= 0f))
		{
			data.ThunderRelicTimer -= DeltaTime;
			if (data.ThunderRelicTimer <= 0f)
			{
				Entity e = CMD.Instantiate(chunkIndex, SpellSingleton.Prefabs["4024_Chain_Player"]);
				CMD.SetComponent(chunkIndex, e, new Spell4024DaveHarpoonThunderRelicData
				{
					Count = 0,
					CurrentEntity = data.CatchEntity,
					Damage = config.Damage.Calculate(),
					DamageRate = RelicData.DamageRate / 100f,
					Radius = RelicData.Radius,
					HarpoonEntity = spellEntity,
					HarpoonComp = componentData,
					HarpoonConfig = config,
					HarpoonEle = element,
					HarpoonMove = move,
					HarpoonTrans = transform,
					HarpoonStartPos = data.StartPos
				});
				data.ThunderRelicTimer += 0.5f;
			}
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		BufferAccessor<StatefulTriggerEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle);
		BufferAccessor<StatefulCollisionEvent> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				ref Spell4024DaveHarpoonData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i);
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer = bufferAccessor[i];
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer = bufferAccessor2[i];
				Execute(chunkIndexInQuery, entity, ref data, ref transform, ref movement, ref config, ref componentData, triggerBuffer, collisionBuffer, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr7, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, i));
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
					ref Spell4024DaveHarpoonData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin);
					DynamicBuffer<StatefulTriggerEvent> triggerBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<StatefulCollisionEvent> collisionBuffer2 = bufferAccessor2[nextRangeBegin];
					Execute(chunkIndexInQuery, entity2, ref data2, ref transform2, ref movement2, ref config2, ref componentData2, triggerBuffer2, collisionBuffer2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr7, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, nextRangeBegin));
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
				ref Spell4024DaveHarpoonData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j);
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer3 = bufferAccessor[j];
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer3 = bufferAccessor2[j];
				Execute(chunkIndexInQuery, entity3, ref data3, ref transform3, ref movement3, ref config3, ref componentData3, triggerBuffer3, collisionBuffer3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr7, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, j));
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
				ref Spell4024DaveHarpoonData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k);
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer4 = bufferAccessor[k];
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer4 = bufferAccessor2[k];
				Execute(chunkIndexInQuery, entity4, ref data4, ref transform4, ref movement4, ref config4, ref componentData4, triggerBuffer4, collisionBuffer4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr7, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, k));
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
