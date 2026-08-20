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
public struct Spell1017DeathAdderJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1017DeathAdderData> __Spell1017DeathAdderData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Shadow_Dots> __Shadow_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1017DeathAdderData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1017DeathAdderData>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Shadow_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Shadow_Dots>();
				__SpellSplitComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSplitComponentData>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell1017DeathAdderData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Shadow_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellSplitComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellSplitComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1017DeathAdderData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Shadow_Dots>();
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
		public void Run(ref Spell1017DeathAdderJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1017DeathAdderJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1017DeathAdderJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1017DeathAdderJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1017DeathAdderJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1017DeathAdderJob job, EntityManager entityManager)
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
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public EntityCommandBuffer.ParallelWriter Ecb;

	public CurrentRoomEntitiesSingleton currentRoom;

	public float3 MousePosition;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public Entity ScreenShakeSingleton;

	public Entity PlayerEntity;

	public float DeltaTime;

	[NativeDisableUnsafePtrRestriction]
	public RefRW<GlobalRandom> gRandom;

	public Entity SEPlayerSingleton;

	public Entity GlobalParticleSingleton;

	public NativeQueue<DeathAdderSpawnReq>.ParallelWriter SpawnQueue;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell1017DeathAdderData deathAdderData, in SpellComponentData spell, ref SpellConfigComponentData config, ref LocalTransform localTransform, ref SpellMovementComponentData movement, ref SpellElementEffectComponentData elementEffect, ref Shadow_Dots shadow, in SpellSplitComponentData split, Entity entity, [ChunkIndexInQuery] int chunkIndex)
	{
		if (!deathAdderData.InitOver)
		{
			deathAdderData.InitOver = true;
			config.Duration.Extra = 0f;
			config.Duration.Base = 6f;
			deathAdderData.BeginPosition = localTransform.Position;
			if (split.Count == 0 && spell.IsSplitSpell)
			{
				config.Radius.FallRadius = 0f;
			}
			ref EntityCommandBuffer.ParallelWriter ecb = ref Ecb;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			FixedString32Bytes seName = "All";
			ecb.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1017, in seName)));
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
				if (spell.IsSplitSpell && split.Count == 0)
				{
					deathAdderData.BoomPosition = deathAdderData.BeginPosition + math.normalizesafe(movement.Direction);
				}
				else
				{
					deathAdderData.BoomPosition = movement.FallTargetPosition;
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
				deathAdderData.BoomPosition = MousePosition;
				break;
			case SpellSpecialMovementType.ChaseOwner:
				deathAdderData.BoomPosition = LocalTransformLookup[PlayerEntity].Position;
				break;
			case SpellSpecialMovementType.ChaseEnemy:
			{
				new NativeList<DistanceHit>(Allocator.Temp);
				if (currentRoom.FindNearestTarget(deathAdderData.BoomPosition, config.ShooterType, out var target2, out var targetPosition2, out var _))
				{
					targetPosition2.z = 0f;
					float3 beginPosition = deathAdderData.BeginPosition;
					beginPosition.z = 0f;
					if (spell.IsSplitSpell && split.Count == 0)
					{
						if (math.distance(targetPosition2, beginPosition) < 5f)
						{
							deathAdderData.BoomPosition = LocalTransformLookup[target2].Position;
							break;
						}
						float3 float2 = math.normalizesafe(targetPosition2 - beginPosition);
						deathAdderData.BoomPosition = beginPosition + float2 * 5f;
					}
					else if (math.distance(targetPosition2, movement.FallTargetPosition) < 5f)
					{
						deathAdderData.BoomPosition = targetPosition2;
					}
					else
					{
						float3 float3 = math.normalizesafe(targetPosition2 - movement.FallTargetPosition);
						deathAdderData.BoomPosition = movement.FallTargetPosition + float3 * 5f;
					}
				}
				else if (spell.IsSplitSpell && split.Count == 0)
				{
					deathAdderData.BoomPosition = deathAdderData.BeginPosition + math.normalizesafe(movement.Direction);
				}
				else
				{
					deathAdderData.BoomPosition = movement.FallTargetPosition;
				}
				break;
			}
			case SpellSpecialMovementType.Rotation:
			{
				float3 @float = (((spell.IsSplitSpell && split.Count > 0) || !spell.IsSplitSpell) ? movement.AroundCenter : (movement.AroundCenter = localTransform.Position));
				float num = config.Radius.Calculate();
				deathAdderData.BoomPosition = @float + new float3(gRandom.ValueRW.random.NextFloat2Direction(), 0f) * movement.AroundRadius;
				currentRoom.FindValidTargetsInRange(@float, movement.AroundRadius + num, config.ShooterType, out var target, out var targetPosition, out var _);
				if (target.Length <= 0)
				{
					break;
				}
				for (int i = 0; i < target.Length; i++)
				{
					if (math.distance(targetPosition[i], @float) > movement.AroundRadius - num && math.distance(targetPosition[i], @float) < movement.AroundRadius + num)
					{
						deathAdderData.BoomPosition = targetPosition[i];
						break;
					}
				}
				break;
			}
			}
			Boom(entity, in spell, in split, ref deathAdderData, ref movement, ref localTransform, ref config, in spell, in elementEffect, LocalTransformLookup, SpellConfigLookup, chunkIndex);
		}
		if (movement.ReboundCount > 0)
		{
			deathAdderData.RebondTimer += DeltaTime;
			if (!(deathAdderData.RebondTimer > 0.15f))
			{
				return;
			}
			deathAdderData.RebondTimer = 0f;
			movement.ReboundCount--;
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
				deathAdderData.BoomPosition += movement.Direction * 2f;
				break;
			case SpellSpecialMovementType.ChaseMouse:
				deathAdderData.BoomPosition = MousePosition;
				break;
			case SpellSpecialMovementType.ChaseEnemy:
			{
				if (currentRoom.FindNearestTarget(deathAdderData.BoomPosition, config.ShooterType, out var _, out var targetPosition4, out var _))
				{
					if (math.distance(targetPosition4, deathAdderData.BeginPosition) < 5f)
					{
						deathAdderData.BoomPosition = targetPosition4;
						break;
					}
					float3 float5 = math.normalizesafe(targetPosition4 - deathAdderData.BeginPosition);
					deathAdderData.BoomPosition = deathAdderData.BeginPosition + float5 * 5f;
				}
				else
				{
					deathAdderData.BoomPosition += movement.Direction * 2f;
				}
				break;
			}
			case SpellSpecialMovementType.Rotation:
			{
				float3 float4 = (((!spell.IsSplitSpell || split.Count <= 0) && spell.IsSplitSpell) ? movement.AroundCenter : movement.AroundCenter);
				float num2 = config.Radius.Calculate();
				deathAdderData.BeginPosition = localTransform.Position;
				deathAdderData.BoomPosition = float4 + new float3(gRandom.ValueRW.random.NextFloat2Direction(), 0f) * movement.AroundRadius;
				currentRoom.FindValidTargetsInRange(float4, movement.AroundRadius + num2, config.ShooterType, out var target3, out var targetPosition3, out var _);
				if (target3.Length <= 0)
				{
					break;
				}
				for (int j = 0; j < target3.Length; j++)
				{
					if (math.distance(targetPosition3[j], float4) > movement.AroundRadius - num2 && math.distance(targetPosition3[j], float4) < movement.AroundRadius + num2)
					{
						deathAdderData.BoomPosition = targetPosition3[j];
						break;
					}
				}
				break;
			}
			case SpellSpecialMovementType.ChaseOwner:
				deathAdderData.BoomPosition = LocalTransformLookup[PlayerEntity].Position;
				break;
			}
			Boom(entity, in spell, in split, ref deathAdderData, ref movement, ref localTransform, ref config, in spell, in elementEffect, LocalTransformLookup, SpellConfigLookup, chunkIndex);
		}
		else if (movement.ReboundCount == 0)
		{
			if (config.DurationTimer >= 0.6f)
			{
				Ecb.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
			else
			{
				movement.Type = SpellSpecialMovementType.Normal;
			}
		}
	}

	private void Boom(Entity entity, in SpellComponentData spell, in SpellSplitComponentData split, ref Spell1017DeathAdderData deathAdderData, ref SpellMovementComponentData movement, ref LocalTransform localTransform, ref SpellConfigComponentData config, in SpellComponentData data, in SpellElementEffectComponentData elementEffect, ComponentLookup<LocalTransform> localTransformLookup, ComponentLookup<SpellConfigComponentData> spellConfigLookup, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		if (split.Count == 0 && spell.IsSplitSpell)
		{
			Spell1017DeathAdderEffectData spell1017DeathAdderEffectData = default(Spell1017DeathAdderEffectData);
			spell1017DeathAdderEffectData.BeginPosition = deathAdderData.BeginPosition;
			spell1017DeathAdderEffectData.BoomPosition = deathAdderData.BoomPosition;
			spell1017DeathAdderEffectData.CenterPoint = ((movement.Type == SpellSpecialMovementType.Rotation) ? movement.AroundCenter : deathAdderData.BeginPosition);
			spell1017DeathAdderEffectData.Type = movement.Type;
			spell1017DeathAdderEffectData.IsFallSpell = movement.IsFallSpell;
			spell1017DeathAdderEffectData.BaseHeight = localTransform.Position.z;
			spell1017DeathAdderEffectData.GroundScale = config.Radius.Calculate();
			spell1017DeathAdderEffectData.ColorType = config.ColorType;
			spell1017DeathAdderEffectData.AroundRadius = movement.AroundRadius;
			spell1017DeathAdderEffectData.LineWidth = 1f + config.Damage.AddRatio / 8f;
			spell1017DeathAdderEffectData.HoverDuration = config.HoverDuration;
			Spell1017DeathAdderEffectData data2 = spell1017DeathAdderEffectData;
			SpawnQueue.Enqueue(new DeathAdderSpawnReq
			{
				Prefab = deathAdderData.EffectEntity,
				Data = data2,
				Color = result
			});
		}
		else if (movement.IsFallSpell && movement.Type == SpellSpecialMovementType.Rotation)
		{
			float num = Unity.Mathematics.Random.CreateFromIndex((uint)entity.Index).NextFloat(0f, 360f);
			deathAdderData.BoomPosition = movement.AroundCenter + (float3)Tool2D.GetDir(num) * 3f;
			deathAdderData.BoomPosition = new float3(deathAdderData.BoomPosition.x, deathAdderData.BoomPosition.y, 0f);
			Spell1017DeathAdderEffectData spell1017DeathAdderEffectData = default(Spell1017DeathAdderEffectData);
			spell1017DeathAdderEffectData.BeginPosition = deathAdderData.BeginPosition;
			spell1017DeathAdderEffectData.BoomPosition = deathAdderData.BoomPosition;
			spell1017DeathAdderEffectData.CenterPoint = movement.AroundCenter;
			spell1017DeathAdderEffectData.Type = movement.Type;
			spell1017DeathAdderEffectData.IsFallSpell = movement.IsFallSpell;
			spell1017DeathAdderEffectData.BaseHeight = localTransform.Position.z;
			spell1017DeathAdderEffectData.GroundScale = config.Radius.Calculate();
			spell1017DeathAdderEffectData.ColorType = config.ColorType;
			spell1017DeathAdderEffectData.AroundRadius = movement.AroundRadius;
			spell1017DeathAdderEffectData.RandomAngle = num;
			spell1017DeathAdderEffectData.LineWidth = 1f + config.Damage.AddRatio / 8f;
			spell1017DeathAdderEffectData.HoverDuration = config.HoverDuration;
			Spell1017DeathAdderEffectData data3 = spell1017DeathAdderEffectData;
			SpawnQueue.Enqueue(new DeathAdderSpawnReq
			{
				Prefab = deathAdderData.EffectEntity,
				Data = data3,
				Color = result
			});
		}
		else
		{
			Spell1017DeathAdderEffectData spell1017DeathAdderEffectData = default(Spell1017DeathAdderEffectData);
			spell1017DeathAdderEffectData.BeginPosition = deathAdderData.BeginPosition;
			spell1017DeathAdderEffectData.BoomPosition = deathAdderData.BoomPosition;
			spell1017DeathAdderEffectData.CenterPoint = movement.AroundCenter;
			spell1017DeathAdderEffectData.Type = movement.Type;
			spell1017DeathAdderEffectData.IsFallSpell = movement.IsFallSpell;
			spell1017DeathAdderEffectData.BaseHeight = localTransform.Position.z;
			spell1017DeathAdderEffectData.GroundScale = config.Radius.Calculate();
			spell1017DeathAdderEffectData.ColorType = config.ColorType;
			spell1017DeathAdderEffectData.AroundRadius = movement.AroundRadius;
			spell1017DeathAdderEffectData.LineWidth = 1f + config.Damage.AddRatio / 8f;
			spell1017DeathAdderEffectData.HoverDuration = config.HoverDuration;
			Spell1017DeathAdderEffectData data4 = spell1017DeathAdderEffectData;
			SpawnQueue.Enqueue(new DeathAdderSpawnReq
			{
				Prefab = deathAdderData.EffectEntity,
				Data = data4,
				Color = result
			});
		}
		Ecb.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
		{
			Radius = 0.2f,
			Speed = 5f,
			Time = 0.2f
		});
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref float3 boomPosition = ref deathAdderData.BoomPosition;
		float radius = config.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in boomPosition, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in spellConfigLookup, in PhysicsWorld, ref entities, checkUnitCamp: false);
		Ecb.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
		{
			Position = Tool2D.GetLayerPoint(deathAdderData.BoomPosition),
			Size = config.Radius.Calculate(),
			Name = $"1017_Explode_{result}"
		});
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in localTransform, in elementEffect, in data, out var info);
		info.isUndifferDamage = true;
		foreach (Entity item in entities)
		{
			Entity target = item;
			info.spell.HitPosition = localTransformLookup[target].Position;
			LocalTransform localTransform2 = localTransformLookup[target];
			float3 knockbackForceIgnoreZBySpell = math.normalizesafe(localTransformLookup[target].Position - deathAdderData.BoomPosition);
			if (knockbackForceIgnoreZBySpell.x == 0f && knockbackForceIgnoreZBySpell.y == 0f && knockbackForceIgnoreZBySpell.z == 0f)
			{
				knockbackForceIgnoreZBySpell = math.normalizesafe(deathAdderData.BoomPosition - deathAdderData.BeginPosition);
			}
			info.SetKnockbackForceIgnoreZBySpell(knockbackForceIgnoreZBySpell);
			Ecb.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in spellConfigLookup, checkCamp: false);
			Ecb.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
			{
				Position = localTransform2.Position + new float3(0f, 0.3f, 0f),
				Size = 1f,
				Name = $"1017_Hit_{result}"
			});
		}
		localTransform.Position = deathAdderData.BoomPosition;
		deathAdderData.BeginPosition = deathAdderData.BoomPosition;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1017DeathAdderData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Shadow_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellSplitComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1017DeathAdderData deathAdderData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1017DeathAdderData>(nativeArrayPtr, i);
				ref SpellComponentData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				ref Shadow_Dots shadow = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr7, i);
				ref SpellSplitComponentData split = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr8, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, i);
				Execute(ref deathAdderData, in spell, ref config, ref localTransform, ref movement, ref elementEffect, ref shadow, in split, entity, chunkIndexInQuery);
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
					ref Spell1017DeathAdderData deathAdderData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1017DeathAdderData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref Shadow_Dots shadow2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr7, nextRangeBegin);
					ref SpellSplitComponentData split2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr8, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, nextRangeBegin);
					Execute(ref deathAdderData2, in spell2, ref config2, ref localTransform2, ref movement2, ref elementEffect2, ref shadow2, in split2, entity2, chunkIndexInQuery);
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
				ref Spell1017DeathAdderData deathAdderData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1017DeathAdderData>(nativeArrayPtr, j);
				ref SpellComponentData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				ref Shadow_Dots shadow3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr7, j);
				ref SpellSplitComponentData split3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr8, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, j);
				Execute(ref deathAdderData3, in spell3, ref config3, ref localTransform3, ref movement3, ref elementEffect3, ref shadow3, in split3, entity3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1017DeathAdderData deathAdderData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1017DeathAdderData>(nativeArrayPtr, k);
				ref SpellComponentData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				ref Shadow_Dots shadow4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Shadow_Dots>(nativeArrayPtr7, k);
				ref SpellSplitComponentData split4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr8, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, k);
				Execute(ref deathAdderData4, in spell4, ref config4, ref localTransform4, ref movement4, ref elementEffect4, ref shadow4, in split4, entity4, chunkIndexInQuery);
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
