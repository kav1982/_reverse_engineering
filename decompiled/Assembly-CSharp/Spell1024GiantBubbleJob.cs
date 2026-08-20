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
public struct Spell1024GiantBubbleJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<Spell1024GiantBubbleData> __Spell1024GiantBubbleData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Spell1024GiantBubbleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1024GiantBubbleData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Spell1024GiantBubbleData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1024GiantBubbleData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
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
		public void Run(ref Spell1024GiantBubbleJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1024GiantBubbleJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1024GiantBubbleJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1024GiantBubbleJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1024GiantBubbleJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1024GiantBubbleJob job, EntityManager entityManager)
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

	public Entity VenomEntity;

	public Entity WaterEntity;

	public Entity MucusEntity;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellGroundedTag> GroundTypeLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<PhysicsCollider> ColliderLookUp;

	public EntityCommandBuffer.ParallelWriter Cmd;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public Entity ScreenShakeSingleton;

	public SpellSingleton SpellSingleton;

	public float DeltaTime;

	public Entity EffectEntity;

	public Entity SpellRequire;

	public EntityCommandBuffer.ParallelWriter EffectCmd;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1024GiantBubbleData> BubbleLookUp;

	public Entity GlobalParticleSingleton;

	public Entity SEPlayerSingleton;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(Entity entity, ref Spell1024GiantBubbleData bubble, ref SpellMovementComponentData movement, ref LocalTransform localTransform, ref SpellConfigComponentData config, in SpellComponentData data, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		if (bubble.IsInit)
		{
			bubble.IsInit = false;
			config.Duration.Extra += 2f;
			NativeHashMap<FixedString64Bytes, SpellEffect> nativeHashMap = SpellSingleton.Effects[1024];
			CreateEffect(chunkIndex, result, nativeHashMap["EffectRange"], entity);
			CreateEffect(chunkIndex, result, nativeHashMap["Spell"], entity);
		}
		if (!bubble.IsCollapse)
		{
			float num = 0.08f;
			bubble.ChargeCollisionRange += num * DeltaTime * config.Radius.Calculate();
			localTransform.Scale += num * DeltaTime * config.Radius.Calculate();
			CollisionFilter collisionFilter = default(CollisionFilter);
			collisionFilter.BelongsTo = uint.MaxValue;
			collisionFilter.CollidesWith = 71936u;
			collisionFilter.GroupIndex = 0;
			CollisionFilter filter = collisionFilter;
			NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
			if (PhysicsWorld.OverlapSphere(localTransform.Position, bubble.ChargeCollisionRange, ref outHits, filter))
			{
				bubble.IsCollapse = true;
				if (!movement.IsFallSpell && movement.ReboundCount <= 0)
				{
					PhysicsCollider collider = ColliderLookUp[entity];
					SpellTools.DisableSpellTrigger(in collider);
				}
				float3 @float = (movement.IsFallSpell ? new float3(0f, 0f, 0f) : new float3(0f, 0.6f, 0f));
				CreateUnFollowingEffect(@float, chunkIndex, result, ref movement, ref config, ref localTransform, "ChargeEnd", 1f);
				Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
				{
					Position = Tool2D.GetLayerPoint(localTransform.Position + @float),
					Size = config.Radius.Calculate(),
					Name = $"1024_ChargeEndGlobal_{result}"
				});
				ref EntityCommandBuffer.ParallelWriter effectCmd = ref EffectCmd;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Collapse";
				effectCmd.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1024, in seName)));
			}
			if (config.DurationTimer >= config.Duration.Calculate() - 2f && !movement.IsFallSpell)
			{
				bubble.IsCollapse = true;
				CreateUnFollowingEffect(new float3(0f, 0.6f, 0f), chunkIndex, result, ref movement, ref config, ref localTransform, "ChargeEnd", 1f);
				Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
				{
					Position = Tool2D.GetLayerPoint(localTransform.Position + new float3(0f, 0.6f, 0f)),
					Size = config.Radius.Calculate(),
					Name = $"1024_ChargeEndGlobal_{result}"
				});
				ref EntityCommandBuffer.ParallelWriter effectCmd2 = ref EffectCmd;
				Entity sEPlayerSingleton2 = SEPlayerSingleton;
				FixedString32Bytes seName = "Collapse";
				effectCmd2.AppendToBuffer(chunkIndex, sEPlayerSingleton2, new SEData(DTool.GetSpellSEName(1024, in seName)));
			}
			if (movement.IsFallSpell && GroundTypeLookup.IsComponentEnabled(entity))
			{
				if (SpellTools.TryReboundWhenFall(ref movement))
				{
					Cmd.SetComponentEnabled<SpellGroundedTag>(chunkIndex, entity, value: false);
					return;
				}
				bubble.IsCollapse = true;
				CreateUnFollowingEffect(new float3(0f, 0f, 0f), chunkIndex, result, ref movement, ref config, ref localTransform, "ChargeEnd", 1f);
				Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
				{
					Position = Tool2D.GetLayerPoint(localTransform.Position),
					Size = config.Radius.Calculate(),
					Name = $"1024_ChargeEndGlobal_{result}"
				});
				ref EntityCommandBuffer.ParallelWriter effectCmd3 = ref EffectCmd;
				Entity sEPlayerSingleton3 = SEPlayerSingleton;
				FixedString32Bytes seName = "Collapse";
				effectCmd3.AppendToBuffer(chunkIndex, sEPlayerSingleton3, new SEData(DTool.GetSpellSEName(1024, in seName)));
			}
		}
		else
		{
			movement.Speed = 0f;
			movement.CurrentFallSpeed = 0f;
			movement.AroundTarget = Entity.Null;
			movement.Type = SpellSpecialMovementType.Normal;
			bubble.CollapseTimer += DeltaTime;
			if (localTransform.Scale > 0.01f)
			{
				localTransform.Scale -= 1.33f * DeltaTime * config.Radius.Calculate();
			}
			else
			{
				localTransform.Scale = 0.01f;
			}
			if (bubble.CollapseTimer >= 0.8f)
			{
				Boom(entity, ref bubble, ref movement, ref localTransform, ref config, in elementEffect, in data, chunkIndex, EffectCmd, BubbleLookUp, LocalTransformLookup);
				Cmd.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
		}
	}

	private void Boom(Entity entity, ref Spell1024GiantBubbleData bubble, ref SpellMovementComponentData movement, ref LocalTransform localTransform, ref SpellConfigComponentData config, in SpellElementEffectComponentData elementEffect, in SpellComponentData data, [ChunkIndexInQuery] int chunkIndex, EntityCommandBuffer.ParallelWriter effectCmd, ComponentLookup<Spell1024GiantBubbleData> bubbleDataLookUp, ComponentLookup<LocalTransform> localTransformLookUp)
	{
		config.ColorType.ColorEnumToString(out var result);
		float num = 1f + config.DurationTimer * config.Float2 / 100f;
		Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
		{
			Position = Tool2D.GetLayerPoint(localTransform.Position),
			Size = config.Radius.Calculate() * num,
			Name = $"1024_Explosion_{result}"
		});
		Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
		{
			Position = Tool2D.GetLayerPoint(localTransform.Position),
			Size = config.Radius.Calculate() * num,
			Name = $"1024_ExplosionGround_{result}"
		});
		Entity sEPlayerSingleton = SEPlayerSingleton;
		FixedString32Bytes seName = "Explosion";
		effectCmd.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1024, in seName)));
		if (config.DurationTimer >= 6.5f)
		{
			NativeHashMap<FixedString64Bytes, SpellEffect> nativeHashMap = SpellSingleton.Effects[1024];
			effectCmd.AppendToBuffer(chunkIndex, SpellRequire, new SpellEffectSystem.Require
			{
				Settings = nativeHashMap["EndRain"],
				Color = result,
				SpellId = 1024,
				Entity = entity
			});
			effectCmd.AppendToBuffer(chunkIndex, SpellRequire, new SpellEffectSystem.Require
			{
				Settings = nativeHashMap["EndRainGround"],
				Color = result,
				SpellId = 1024,
				Entity = entity
			});
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = uint.MaxValue;
		collisionFilter.CollidesWith = 16777216u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		if (PhysicsWorld.OverlapSphere(localTransform.Position, config.Radius.Calculate() * num, ref outHits, filter))
		{
			foreach (DistanceHit item in outHits)
			{
				if (bubbleDataLookUp.HasComponent(item.Entity))
				{
					Entity value = item.Entity;
					nativeList.Add(in value);
				}
			}
		}
		foreach (Entity item2 in nativeList)
		{
			RefRW<Spell1024GiantBubbleData> refRW = bubbleDataLookUp.GetRefRW(item2);
			if (!refRW.ValueRW.IsCollapse)
			{
				refRW.ValueRW.IsCollapse = true;
				LocalTransform localTransform2 = localTransformLookUp[item2];
				CreateUnFollowingEffect(new float3(0f, 0.6f, 0f), chunkIndex, result, ref movement, ref config, ref localTransform2, "ChargeEnd", 1f);
				ref EntityCommandBuffer.ParallelWriter effectCmd2 = ref EffectCmd;
				Entity sEPlayerSingleton2 = SEPlayerSingleton;
				seName = "Collapse";
				effectCmd2.AppendToBuffer(chunkIndex, sEPlayerSingleton2, new SEData(DTool.GetSpellSEName(1024, in seName)));
			}
		}
		float radius = config.Radius.Calculate() * num;
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		SpellTools.GetAttackableEntitiesInRange(in localTransform.Position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		config.Damage.AddBase = config.Damage.Base * (config.DurationTimer * config.Float3 / 100f);
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in localTransform, in elementEffect, in data, out var info);
		if (config.ColorType == SpellColorType.Venom)
		{
			Cmd.AppendToBuffer(chunkIndex, VenomEntity, new CreateVenomRequest(localTransform.Position, radius, 2f));
		}
		else if (config.ColorType == SpellColorType.Mucus)
		{
			Cmd.AppendToBuffer(chunkIndex, MucusEntity, new CreateMucusRequest(localTransform.Position, radius));
		}
		else if (config.ColorType == SpellColorType.Frozen || config.ColorType == SpellColorType.Player)
		{
			Cmd.AppendToBuffer(chunkIndex, WaterEntity, new CreateWaterRequest(localTransform.Position, radius));
		}
		foreach (Entity item3 in entities)
		{
			Entity target = item3;
			if (LocalTransformLookup.HasComponent(target))
			{
				info.spell.HitPosition = LocalTransformLookup[target].Position;
				LocalTransform localTransform3 = LocalTransformLookup[target];
				Cmd.AppendToBuffer(chunkIndex, GlobalParticleSingleton, new GlobalParticleEmitParams
				{
					Position = LocalTransformLookup[target].Position + new float3(0f, 0.3f, 0f),
					Size = 1f,
					Name = $"1024_Hit_{result}",
					Velocity = DTool.IgnoreZDir(in localTransform3.Position, in localTransform.Position)
				});
				Cmd.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
			}
		}
		Cmd.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
		{
			Radius = 0.2f,
			Speed = 4f,
			Time = 0.3f
		});
		collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = uint.MaxValue;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter2 = collisionFilter;
		NativeList<DistanceHit> outHits2 = new NativeList<DistanceHit>(Allocator.Temp);
		if (!PhysicsWorld.OverlapSphere(localTransform.Position, radius, ref outHits2, filter2))
		{
			return;
		}
		foreach (DistanceHit item4 in outHits2)
		{
			if (UnitPropertyLookup.HasComponent(item4.Entity))
			{
				float3 @float = math.normalize(LocalTransformLookup[item4.Entity].Position - localTransform.Position);
				UnitPropertyLookup.GetRefRW(item4.Entity).ValueRW.TakeKnockback(config.Knockback * @float * 0.05f);
			}
		}
	}

	private void CreateUnFollowingEffect(float3 positionOffset, [ChunkIndexInQuery] int chunkIndex, FixedString32Bytes colorName, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, ref LocalTransform localTransform, string effectName, float scaleNum, bool useToTargetDirection = false, float3 totargetDiretion = default(float3))
	{
		ref EntityCommandBuffer.ParallelWriter cmd = ref Cmd;
		Entity effectEntity = EffectEntity;
		SpellEffectSystem.UnfollowingRequire element = new SpellEffectSystem.UnfollowingRequire
		{
			Settings = new SpellEffect
			{
				Name = effectName,
				DestroyDelay = 1f,
				Layer = LayerCorrectType.Coordinate
			},
			Color = colorName,
			SpellId = 1024,
			StartPosition = localTransform.Position + positionOffset
		};
		quaternion startRotation;
		if (!useToTargetDirection)
		{
			startRotation = localTransform.Rotation;
		}
		else
		{
			float2 dir = totargetDiretion.xy;
			startRotation = DTool.DirectionToRotation(in dir);
		}
		element.StartRotation = startRotation;
		element.Scale = config.Radius.Calculate() * scaleNum;
		cmd.AppendToBuffer(chunkIndex, effectEntity, element);
	}

	private void CreateEffect([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes colorName, SpellEffect settings, Entity entity)
	{
		Cmd.AppendToBuffer(chunkIndex, SpellRequire, new SpellEffectSystem.Require
		{
			Settings = settings,
			Color = colorName,
			SpellId = 1024,
			Entity = entity
		});
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1024GiantBubbleData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				Execute(entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1024GiantBubbleData>(nativeArrayPtr2, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, i), chunkIndexInQuery);
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
					Execute(entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1024GiantBubbleData>(nativeArrayPtr2, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, nextRangeBegin), chunkIndexInQuery);
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
				Execute(entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1024GiantBubbleData>(nativeArrayPtr2, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, j), chunkIndexInQuery);
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
				Execute(entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1024GiantBubbleData>(nativeArrayPtr2, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, k), chunkIndexInQuery);
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
