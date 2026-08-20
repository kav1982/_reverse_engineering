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
public struct SuperNovaJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell1027SuperNovaData> __Spell1027SuperNovaData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellChargeData> __SpellChargeData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell1027SuperNovaData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1027SuperNovaData>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellChargeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellChargeData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell1027SuperNovaData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellChargeData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1027SuperNovaData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellChargeData>();
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
		public void Run(ref SuperNovaJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref SuperNovaJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref SuperNovaJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref SuperNovaJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref SuperNovaJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref SuperNovaJob job, EntityManager entityManager)
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
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellGroundedTag> GroundTypeLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellChargingTag> ChargingTagLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellRefractionData> SpellRefractLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell4004StartData> Spell4004StarLookUp;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellFromChargeModeStar> SpellAttachToStarLookUp;

	[NativeDisableParallelForRestriction]
	public BufferLookup<SpellRefractionHitEntities> SpellRefractHitEntitiesLookUp;

	public EntityCommandBuffer.ParallelWriter Cmd;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[ReadOnly]
	public Entity ScreenShakeSingleton;

	public SpellSingleton SpellSingleton;

	public float DeltaTime;

	public Entity EffectEntity;

	public Entity SpellRequire;

	public Entity DestroyEffectEntity;

	public EntityCommandBuffer.ParallelWriter EffectCmd;

	public Entity SEPlayerSingleton;

	public Entity GlobalParticleEmitBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell1027SuperNovaData novaData, ref SpellConfigComponentData config, ref LocalTransform localTransform, ref SpellMovementComponentData movement, ref SpellElementEffectComponentData elementEffect, ref SpellComponentData spellData, ref SpellChargeData chargeData, Entity entity, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		if (novaData.DestroyTimer != 0f)
		{
			localTransform.Scale = 1E-05f;
		}
		else if (chargeData.ChargeTimer + config.DurationTimer < 5.5f)
		{
			localTransform.Scale += 0.2f * DeltaTime;
		}
		else
		{
			localTransform.Scale -= 10f * DeltaTime;
			if (localTransform.Scale <= 1.5f)
			{
				localTransform.Scale = 1.5f;
			}
		}
		if (!novaData.InitOver)
		{
			CreateEffectDealy1Frame(chunkIndex, result, SpellSingleton.Effects[1027]["Charge"], entity);
			novaData.InitOver = true;
			novaData.AddBaseOriginal = config.Damage.AddBase;
			if (!movement.IsFallSpell)
			{
				movement.Type = SpellSpecialMovementType.Normal;
			}
		}
		if (ChargingTagLookup.IsComponentEnabled(entity))
		{
			if (chargeData.ChargeTimer > 6f)
			{
				Cmd.SetComponentEnabled<SpellChargingTag>(chunkIndex, entity, value: false);
				if (SpellAttachToStarLookUp.TryGetComponent(entity, out var componentData))
				{
					Spell4004StarLookUp.GetRefRW(componentData.StarEntity).ValueRW.NeedBreak = true;
				}
			}
			return;
		}
		if (!movement.IsFallSpell)
		{
			movement.Speed = 0f;
			EffectCmd.AppendToBuffer(chunkIndex, DestroyEffectEntity, new SpellEffectSystem.Destroy
			{
				Name = "Charge",
				Entity = entity
			});
			if (!novaData.BoomOver)
			{
				bool hitUnit = false;
				Boom(entity, ref novaData, ref movement, ref localTransform, ref config, in spellData, in chargeData, in elementEffect, LocalTransformLookup, ref hitUnit, chunkIndex);
				return;
			}
			novaData.DestroyTimer += DeltaTime;
			if (novaData.DestroyTimer >= 0.3f)
			{
				Cmd.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
			return;
		}
		if (!novaData.CreateFallStarTrailEffected)
		{
			CreateEffect(chunkIndex, result, SpellSingleton.Effects[1027]["FallStarTrail"], entity);
			novaData.CreateFallStarTrailEffected = true;
		}
		if (GroundTypeLookup.IsComponentEnabled(entity))
		{
			bool hitUnit2 = false;
			Boom(entity, ref novaData, ref movement, ref localTransform, ref config, in spellData, in chargeData, in elementEffect, LocalTransformLookup, ref hitUnit2, chunkIndex);
			NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
			ref float3 position = ref localTransform.Position;
			UnitType shooterType = config.ShooterType;
			ref ComponentLookup<SpellRefractionData> spellRefractLookUp = ref SpellRefractLookUp;
			ref BufferLookup<SpellRefractionHitEntities> spellRefractHitEntitiesLookUp = ref SpellRefractHitEntitiesLookUp;
			ref CurrentRoomEntitiesSingleton currentRoomEntities = ref CurrentRoomEntities;
			NativeArray<Entity> theEntitiesHitByThisDamage = nativeList.ToArray(Allocator.Temp);
			if (SpellTools.TryRefractOrReboundWhenFall(in entity, in position, shooterType, in spellRefractLookUp, in spellRefractHitEntitiesLookUp, ref movement, in currentRoomEntities, in theEntitiesHitByThisDamage, hitUnit2))
			{
				Cmd.SetComponentEnabled<SpellGroundedTag>(chunkIndex, entity, value: false);
			}
			else
			{
				Cmd.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
		}
	}

	private void Boom(Entity entity, ref Spell1027SuperNovaData novaData, ref SpellMovementComponentData movement, ref LocalTransform localTransform, ref SpellConfigComponentData config, in SpellComponentData data, in SpellChargeData chargeData, in SpellElementEffectComponentData elementEffect, ComponentLookup<LocalTransform> localTransformLookup, ref bool hitUnit, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		float chargeTimer = chargeData.ChargeTimer;
		int num = ((chargeTimer >= 4f) ? ((!(chargeTimer >= 6f)) ? 3 : 4) : ((!(chargeTimer >= 2f)) ? 1 : 2));
		int num2 = num;
		for (int i = 1; i <= num2; i++)
		{
			FixedString32Bytes name = $"1027_ExplosionC{i}_{result}";
			FixedString32Bytes name2 = $"1027_ExplosionG{i}_{result}";
			GlobalParticleEmitParams element = new GlobalParticleEmitParams(GlobalParticleType.Spell, name, localTransform.Position)
			{
				Size = 1f
			};
			Cmd.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element);
			GlobalParticleEmitParams element2 = new GlobalParticleEmitParams(GlobalParticleType.Spell, name2, localTransform.Position + new float3(0f, 0f, 0.8f + (float)i * 0.1f))
			{
				Size = 1f
			};
			Cmd.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element2);
			if (i == 4)
			{
				Cmd.AppendToBuffer(chunkIndex, EffectEntity, new SpellEffectSystem.UnfollowingRequire
				{
					Settings = new SpellEffect
					{
						Name = "ExplosionCE4",
						DestroyDelay = 1.5f,
						Layer = LayerCorrectType.Coordinate
					},
					Color = result,
					SpellId = 1027,
					StartPosition = localTransform.Position + new float3(0f, 0f, 0.1f),
					StartRotation = quaternion.identity,
					Scale = 1f
				});
			}
		}
		float num3 = chargeData.ChargeTimer + config.DurationTimer;
		if (num3 > 6f)
		{
			num3 = 6f;
		}
		config.Damage.AddBase = novaData.AddBaseOriginal + config.Damage.Base * num3 * config.Float1 / 100f;
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		ref float3 position = ref localTransform.Position;
		chargeTimer = 100f;
		SpellTools.GetAttackableEntitiesInRange(in position, in chargeTimer, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in localTransform, in elementEffect, in data, out var info);
		foreach (Entity item in entities)
		{
			Entity target = item;
			LocalTransform localTransform2 = localTransformLookup[target];
			info.spell.HitPosition = localTransform2.Position;
			float3 rootPosition = localTransform2.Position;
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			rootPosition += layerPosition;
			SpellTools.HitType hitType = Cmd.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
			hitUnit |= hitType == SpellTools.HitType.Unit;
			if (hitType != SpellTools.HitType.IgnoreSpell)
			{
				for (int j = 1; j <= num2; j++)
				{
					Cmd.AppendToBuffer(chunkIndex, SpellSingleton.GlobalParticleBufferEntity, new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1027_Hit_{j}_{result}", rootPosition)
					{
						Velocity = DTool.IgnoreZDir(in localTransform2.Position, in localTransform.Position)
					});
				}
			}
		}
		ref EntityCommandBuffer.ParallelWriter cmd = ref Cmd;
		Entity sEPlayerSingleton = SEPlayerSingleton;
		FixedString32Bytes seName = $"Explosion{num2}";
		cmd.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1027, in seName)));
		float num4 = 1f + 0.2f * num3;
		Cmd.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
		{
			Radius = 0.2f * num4,
			Speed = 5f * num4,
			Time = 0.2f
		});
		novaData.BoomOver = true;
	}

	private void CreateEffect([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes colorName, SpellEffect settings, Entity entity)
	{
		EffectCmd.AppendToBuffer(chunkIndex, SpellRequire, new SpellEffectSystem.Require
		{
			Settings = settings,
			Color = colorName,
			SpellId = 1027,
			Entity = entity
		});
	}

	private void CreateEffectDealy1Frame([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes colorName, SpellEffect settings, Entity entity)
	{
		Cmd.AppendToBuffer(chunkIndex, SpellRequire, new SpellEffectSystem.Require
		{
			Settings = settings,
			Color = colorName,
			SpellId = 1027,
			Entity = entity
		});
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1027SuperNovaData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellChargeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell1027SuperNovaData novaData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1027SuperNovaData>(nativeArrayPtr, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, i);
				ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i);
				ref SpellChargeData chargeData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellChargeData>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				Execute(ref novaData, ref config, ref localTransform, ref movement, ref elementEffect, ref spellData, ref chargeData, entity, chunkIndexInQuery);
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
					ref Spell1027SuperNovaData novaData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1027SuperNovaData>(nativeArrayPtr, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref SpellChargeData chargeData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellChargeData>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					Execute(ref novaData2, ref config2, ref localTransform2, ref movement2, ref elementEffect2, ref spellData2, ref chargeData2, entity2, chunkIndexInQuery);
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
				ref Spell1027SuperNovaData novaData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1027SuperNovaData>(nativeArrayPtr, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, j);
				ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j);
				ref SpellChargeData chargeData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellChargeData>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				Execute(ref novaData3, ref config3, ref localTransform3, ref movement3, ref elementEffect3, ref spellData3, ref chargeData3, entity3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell1027SuperNovaData novaData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1027SuperNovaData>(nativeArrayPtr, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr5, k);
				ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k);
				ref SpellChargeData chargeData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellChargeData>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				Execute(ref novaData4, ref config4, ref localTransform4, ref movement4, ref elementEffect4, ref spellData4, ref chargeData4, entity4, chunkIndexInQuery);
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
