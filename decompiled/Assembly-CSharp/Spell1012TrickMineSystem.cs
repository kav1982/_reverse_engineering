using System;
using System.Collections;
using System.Collections.Generic;
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

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
[CompilerGenerated]
internal struct Spell1012TrickMineSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Spell1012Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell1012TrickMineData> __Spell1012TrickMineData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell1012TrickMineData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1012TrickMineData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
				}

				public void Update(ref SystemState state)
				{
					__Spell1012TrickMineData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1012TrickMineData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
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
			public void Run(ref Spell1012Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1012Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1012Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1012Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1012Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1012Job job, EntityManager entityManager)
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

		public GlobalRandom Random;

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity VenomEntity;

		public Entity WaterEntity;

		public Entity MucusEntity;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[ReadOnly]
		public PhysicsWorldSingleton Physics;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<Spell1012TrickmineFallToAbyssTag> FallToAbyssLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<Spell1012TrickMineData> Spell1012DataLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<Spell1012SpellMaterialProperty> SpellMaterialLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellRefractionData> SpellRefractLookUp;

		[NativeDisableParallelForRestriction]
		public BufferLookup<SpellRefractionHitEntities> SpellRefractHitEntitiesLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellGroundedTag> SpellGroundLookup;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public NativeList<Entity> TrickMineList;

		[NativeDisableParallelForRestriction]
		public BufferLookup<Child> ChildLookup;

		public Entity ScreenShakeSingleton;

		public Entity SEPlayerSingleton;

		public Entity GlobalParticleEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(ref Spell1012TrickMineData data, ref LocalTransform transform, ref SpellMovementComponentData movement, Entity entity, in SpellComponentData spellData, [ChunkIndexInQuery] int chunkIndex, PhysicsCollider physicsCollider)
		{
			ref SpellConfigComponentData valueRW = ref SpellConfigLookup.GetRefRW(entity).ValueRW;
			RefRW<LocalTransform> refRW;
			if (!data.IsInitialize)
			{
				data.IsInitialize = true;
				if (!movement.IsFallSpell)
				{
					valueRW.Duration.Extra += valueRW.HoverDuration;
					valueRW.HoverDuration = 0f;
					movement.ReboundCount = 100;
					movement.ReboundAddTime = 0f;
					movement.Gravity = 15f;
					movement.CurrentFallSpeed = -4f;
					movement.FallingReboundForceRatio = 0.33f;
				}
				else
				{
					movement.ChaseRotateSpeed *= 0.5f;
					SpellTools.DisableSpellReboundCollider(in physicsCollider);
					if (!spellData.IsSplitSpell)
					{
						movement.CurrentFallSpeed = movement.OriginalSpellHorizontalSpeed;
					}
					else
					{
						movement.CurrentFallSpeed = 0f - movement.OriginalSpellHorizontalSpeed;
					}
				}
				valueRW.Float3 = ((movement.Direction.x < 0f) ? 15f : (-15f));
				if (spellData.SpellEffectEntity != Entity.Null)
				{
					refRW = LocalTransformLookUp.GetRefRW(spellData.SpellEffectEntity);
					ref LocalTransform valueRW2 = ref refRW.ValueRW;
					valueRW2 = valueRW2.Rotate(quaternion.Euler(new float3(0f, 0f, Random.random.NextFloat(0f, 360f))));
				}
			}
			if (data.ChainExplosionImmuteTimer > 0f)
			{
				data.ChainExplosionImmuteTimer -= DeltaTime;
			}
			if (data.ExplosionCooldown > 0f)
			{
				data.ExplosionCooldown -= DeltaTime;
			}
			if (spellData.SpellEffectEntity != Entity.Null && valueRW.Float3 != 0f)
			{
				refRW = LocalTransformLookUp.GetRefRW(spellData.SpellEffectEntity);
				ref LocalTransform valueRW3 = ref refRW.ValueRW;
				valueRW3 = valueRW3.Rotate(quaternion.Euler(new float3(0f, 0f, valueRW.Float3 * DeltaTime)));
			}
			if (!movement.IsFallSpell)
			{
				if (FallToAbyssLookup.IsComponentEnabled(entity))
				{
					transform.Scale -= 0.03f;
					if (transform.Scale <= 0f)
					{
						Explosion(entity, ref valueRW, ref movement, chunkIndex, transform.Position, canPenetrate: false, spellData);
						CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
					}
					return;
				}
				if (transform.Position.z >= 0f)
				{
					float3 position = transform.Position.IgnoreZ();
					CollisionFilter @default = CollisionFilter.Default;
					@default.CollidesWith = 1024u;
					if (Physics.CheckSphere(position, 0.1f, @default))
					{
						CMD.SetComponentEnabled<Spell1012TrickmineFallToAbyssTag>(chunkIndex, entity, value: true);
						movement.Speed = 0f;
					}
				}
			}
			if (movement.IsFallSpell)
			{
				if (SpellGroundLookup.IsComponentEnabled(entity))
				{
					Explosion(entity, ref valueRW, ref movement, chunkIndex, transform.Position, canPenetrate: false, spellData);
				}
				return;
			}
			bool flag = false;
			if (data.ExplosionCooldown <= 0f)
			{
				NativeList<Entity> result = new NativeList<Entity>(Allocator.Temp);
				LocalTransform localTransform = LocalTransformLookUp[entity];
				Spell1012GetTargetsInrange(in localTransform.Position, 0.6f, valueRW.ShooterType, in UnitPropertyLookup, in Physics, ref result);
				if (result.Length > 0)
				{
					Explosion(entity, ref valueRW, ref movement, chunkIndex, transform.Position, canPenetrate: true, spellData);
					flag = true;
					data.ExplosionCooldown = 0.2f;
				}
			}
			if (!flag && valueRW.Duration.Calculate() <= valueRW.DurationTimer)
			{
				Explosion(entity, ref valueRW, ref movement, chunkIndex, transform.Position, canPenetrate: false, spellData);
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
			if (valueRW.Duration.Calculate() - valueRW.DurationTimer <= 1.5f && valueRW.Duration.Calculate() - valueRW.DurationTimer >= 0.05f && !data.EndingFlashEnable)
			{
				SetBombFlashMaterial(spellData.SpellEffectEntity);
				data.EndingFlashEnable = true;
			}
		}

		[BurstCompile]
		private void SetBombFlashMaterial(Entity spellEffectEntity)
		{
			if (!ChildLookup.HasBuffer(spellEffectEntity))
			{
				return;
			}
			foreach (Child item in ChildLookup[spellEffectEntity])
			{
				if (SpellMaterialLookup.HasComponent(item.Value))
				{
					SpellMaterialLookup.GetRefRW(item.Value).ValueRW.Value = 1f;
				}
			}
		}

		private void Spell1012GetTargetsInrange(in float3 startPoint, float checkRadius, UnitType selfUnitType, in ComponentLookup<UnitProperty_Dots> cluUnitPpt, in PhysicsWorldSingleton pws, ref NativeList<Entity> result)
		{
			CollisionFilter filter = DTool.CreateOtherCampFilter(selfUnitType, containsBrittleness: false);
			filter.CollidesWith |= 131072u;
			DTool.GetUnitEntityInRange(in startPoint, checkRadius, in filter, in cluUnitPpt, in pws, ref result);
		}

		public void Explosion(Entity entity, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, int chunkIndex, float3 explosionPosition, bool canPenetrate, SpellComponentData data)
		{
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			ref SpellSingleton spellSingleton = ref SpellSingleton;
			FixedString32Bytes EffectName = "Trace";
			cMD.CreateSpellEffect(chunkIndex, in spellSingleton, in data, in config, in explosionPosition, in EffectName, config.Radius.Calculate(), in float3.zero);
			config.ColorType.ColorEnumToString(out var result);
			CMD.AppendToBuffer(chunkIndex, GlobalParticleEntity, new GlobalParticleEmitParams
			{
				Position = explosionPosition,
				Size = config.Radius.Calculate() / 2f,
				Name = $"1012_Explosion_{result}"
			});
			ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			EffectName = "Explosion";
			cMD2.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1012, in EffectName)));
			CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
			{
				Radius = 0.1f,
				Speed = 5f,
				Time = 0.2f
			});
			if (config.ColorType == SpellColorType.Venom)
			{
				CMD.AppendToBuffer(chunkIndex, VenomEntity, new CreateVenomRequest(explosionPosition, config.Radius.Calculate(), 2f));
			}
			else if (config.ColorType == SpellColorType.Mucus)
			{
				CMD.AppendToBuffer(chunkIndex, MucusEntity, new CreateMucusRequest(explosionPosition, config.Radius.Calculate()));
			}
			else if (config.ColorType == SpellColorType.Frozen)
			{
				CMD.AppendToBuffer(chunkIndex, WaterEntity, new CreateWaterRequest(explosionPosition, config.Radius.Calculate()));
			}
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			LocalTransform localTransform = LocalTransformLookUp[entity];
			ref float3 position = ref localTransform.Position;
			float radius = config.Radius.Calculate();
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in Physics, ref entities, checkUnitCamp: false);
			Entity spellEntity = entity;
			localTransform = LocalTransformLookUp[entity];
			SpellElementEffectComponentData elementEffect = SpellElementLookup[entity];
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in localTransform, in elementEffect, in data, out var info);
			info.spell.CostPenetrate = false;
			info.spell.CostRefraction = false;
			bool flag = false;
			foreach (Entity item in entities)
			{
				Entity target = item;
				info.damage = config.Damage.Calculate();
				info.isUndifferDamage = true;
				info.SetKnockbackForceIgnoreZBySpell(LocalTransformLookUp[target].Position - explosionPosition);
				SpellTools.HitType hitType = CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: false);
				flag = flag || hitType == SpellTools.HitType.Unit;
			}
			foreach (Entity trickMine in TrickMineList)
			{
				float3 position2 = LocalTransformLookUp.GetRefRW(trickMine).ValueRO.Position;
				RefRW<Spell1012TrickMineData> refRW = Spell1012DataLookup.GetRefRW(trickMine);
				if (!(math.distance(explosionPosition, position2) > config.Radius.Calculate()) && !refRW.ValueRW.IsDenoteByOtherTrickMine && !(refRW.ValueRW.ChainExplosionImmuteTimer > 0f) && !(trickMine == entity))
				{
					RefRW<SpellConfigComponentData> refRW2 = SpellConfigLookup.GetRefRW(trickMine);
					refRW2.ValueRW.DurationTimer = math.max(refRW2.ValueRW.Duration.Calculate() - 0.15f, refRW2.ValueRW.Duration.Calculate() - refRW2.ValueRW.DurationTimer);
					refRW.ValueRW.IsDenoteByOtherTrickMine = true;
				}
			}
			if (movement.IsFallSpell)
			{
				UnitType shooterType = config.ShooterType;
				ref ComponentLookup<SpellRefractionData> spellRefractLookUp = ref SpellRefractLookUp;
				ref BufferLookup<SpellRefractionHitEntities> spellRefractHitEntitiesLookUp = ref SpellRefractHitEntitiesLookUp;
				ref CurrentRoomEntitiesSingleton currentRoomEntities = ref CurrentRoomEntities;
				NativeArray<Entity> theEntitiesHitByThisDamage = entities.ToArray(Allocator.Temp);
				if (!SpellTools.TryRefractOrReboundWhenFall(in entity, in explosionPosition, shooterType, in spellRefractLookUp, in spellRefractHitEntitiesLookUp, ref movement, in currentRoomEntities, in theEntitiesHitByThisDamage, flag))
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
				}
				return;
			}
			int num;
			if (SpellRefractLookUp.HasComponent(entity))
			{
				UnitType shooterType2 = config.ShooterType;
				ref SpellRefractionData valueRW = ref SpellRefractLookUp.GetRefRW(entity).ValueRW;
				DynamicBuffer<SpellRefractionHitEntities> refractedEntities = SpellRefractHitEntitiesLookUp[entity];
				ref CurrentRoomEntitiesSingleton currentRoomEntities2 = ref CurrentRoomEntities;
				NativeArray<Entity> theEntitiesHitByThisDamage = entities.ToArray(Allocator.Temp);
				num = (SpellTools.TryRefract(in explosionPosition, shooterType2, ref valueRW, in refractedEntities, ref movement, in currentRoomEntities2, in theEntitiesHitByThisDamage, out var _) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag2 = false;
			if (num == 0 && config.Penetrate.Calculate() > 0)
			{
				config.Penetrate.CostPenetrateValue();
				flag2 = true;
			}
			if (num == 0 && !flag2)
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1012TrickMineData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell1012TrickMineData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1012TrickMineData>(nativeArrayPtr, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(ref data, ref transform, ref movement, entity, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i), chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr6, i));
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
						ref Spell1012TrickMineData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1012TrickMineData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(ref data2, ref transform2, ref movement2, entity2, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin), chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr6, nextRangeBegin));
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
					ref Spell1012TrickMineData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1012TrickMineData>(nativeArrayPtr, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(ref data3, ref transform3, ref movement3, entity3, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j), chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr6, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell1012TrickMineData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1012TrickMineData>(nativeArrayPtr, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(ref data4, ref transform4, ref movement4, entity4, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k), chunkIndexInQuery, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr6, k));
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

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_197134880_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1012TrickMineData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1012TrickMineData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1012TrickMineData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Spell1012TrickMineData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_197134880_0.TypeHandle __IFE_197134880_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<Spell1012TrickMineData> __Spell1012TrickMineData_RW_ComponentLookup;

		public BufferLookup<Child> __Unity_Transforms_Child_RW_BufferLookup;

		public ComponentLookup<Spell1012SpellMaterialProperty> __Spell1012SpellMaterialProperty_RW_ComponentLookup;

		public ComponentLookup<SpellGroundedTag> __SpellGroundedTag_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public ComponentLookup<Spell1012TrickmineFallToAbyssTag> __Spell1012TrickmineFallToAbyssTag_RW_ComponentLookup;

		public Spell1012Job.InternalCompilerQueryAndHandleData __Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_197134880_0_TypeHandle = new IFE_197134880_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellElementEffectComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1012TrickMineData_RW_ComponentLookup = state.GetComponentLookup<Spell1012TrickMineData>();
			__Unity_Transforms_Child_RW_BufferLookup = state.GetBufferLookup<Child>();
			__Spell1012SpellMaterialProperty_RW_ComponentLookup = state.GetComponentLookup<Spell1012SpellMaterialProperty>();
			__SpellGroundedTag_RW_ComponentLookup = state.GetComponentLookup<SpellGroundedTag>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__Spell1012TrickmineFallToAbyssTag_RW_ComponentLookup = state.GetComponentLookup<Spell1012TrickmineFallToAbyssTag>();
			__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000065FF_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000065FF_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000065FF_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00006600_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006600_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006600_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnUpdate_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_197134880_0;

	private EntityQuery __query_197134880_1;

	private EntityQuery __query_197134880_2;

	private EntityQuery __query_197134880_3;

	private EntityQuery __query_197134880_4;

	private EntityQuery __query_197134880_5;

	private EntityQuery __query_197134880_6;

	private EntityQuery __query_197134880_7;

	private EntityQuery __query_197134880_8;

	private EntityQuery __query_197134880_9;

	private EntityQuery __query_197134880_10;

	private EntityQuery __query_197134880_11;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CreateVenomRequest>();
		state.RequireForUpdate<CreateMucusRequest>();
		state.RequireForUpdate<CreateWaterRequest>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<Spell1012TrickMineData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_197134880_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		NativeList<Entity> trickMineList = new NativeList<Entity>(Allocator.TempJob);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> item3 in IFE_197134880_0.Query(__query_197134880_0, __TypeHandle.__IFE_197134880_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1012TrickMineData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO = item2;
			Entity value = entity;
			if (!uncheckedRefRW.ValueRW.IsDenoteByOtherTrickMine && !uncheckedRefRO.ValueRO.IsFallSpell)
			{
				trickMineList.Add(in value);
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1012Job
		{
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellElementLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			Spell1012DataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1012TrickMineData_RW_ComponentLookup, ref state),
			ChildLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Unity_Transforms_Child_RW_BufferLookup, ref state),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = entityCommandBuffer.AsParallelWriter(),
			WaterEntity = __query_197134880_2.GetSingletonEntity(),
			VenomEntity = __query_197134880_3.GetSingletonEntity(),
			MucusEntity = __query_197134880_4.GetSingletonEntity(),
			Physics = __query_197134880_5.GetSingleton<PhysicsWorldSingleton>(),
			SpellMaterialLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1012SpellMaterialProperty_RW_ComponentLookup, ref state),
			TrickMineList = trickMineList,
			Random = __query_197134880_6.GetSingleton<GlobalRandom>(),
			SpellSingleton = __query_197134880_7.GetSingleton<SpellSingleton>(),
			ScreenShakeSingleton = __query_197134880_8.GetSingletonEntity(),
			SEPlayerSingleton = __query_197134880_9.GetSingletonEntity(),
			SpellGroundLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellGroundedTag_RW_ComponentLookup, ref state),
			SpellRefractHitEntitiesLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			SpellRefractLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_197134880_10.GetSingleton<CurrentRoomEntitiesSingleton>(),
			FallToAbyssLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1012TrickmineFallToAbyssTag_RW_ComponentLookup, ref state),
			GlobalParticleEntity = __query_197134880_11.GetSingletonEntity()
		}, __TypeHandle.__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		trickMineList.Dispose(state.Dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1012Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1012TrickMineSystem_Spell1012Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1012TrickMineData>();
		__query_197134880_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateWaterRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateVenomRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateMucusRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_197134880_11 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_000065FF_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006600_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1012TrickMineSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1012TrickMineSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1012TrickMineSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
