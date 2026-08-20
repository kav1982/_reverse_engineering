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
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
internal struct Spell4024DaveHarpoonSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	public struct Spell4024HarpoonRotateTargetGetJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell4024DaveHarpoonData> __Spell4024DaveHarpoonData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public BufferTypeHandle<HarpoonChainData> __HarpoonChainData_RW_BufferTypeHandle;

				public BufferTypeHandle<HighspeedHarpoonSphereCastPos> __HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__HarpoonChainData_RW_BufferTypeHandle = state.GetBufferTypeHandle<HarpoonChainData>();
					__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle = state.GetBufferTypeHandle<HighspeedHarpoonSphereCastPos>();
				}

				public void Update(ref SystemState state)
				{
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__HarpoonChainData_RW_BufferTypeHandle.Update(ref state);
					__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle.Update(ref state);
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
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HarpoonChainData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HighspeedHarpoonSphereCastPos>();
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
			public void Run(ref Spell4024HarpoonRotateTargetGetJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4024HarpoonRotateTargetGetJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4024HarpoonRotateTargetGetJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4024HarpoonRotateTargetGetJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4024HarpoonRotateTargetGetJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4024HarpoonRotateTargetGetJob job, EntityManager entityManager)
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

		public float3 MousePosition;

		public GlobalRandom Random;

		public EntityCommandBuffer.ParallelWriter CMD;

		public LightningHarpoonRelicData RelicData;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<EffectsCollectorData> EffectCtrlLookUp;

		[ReadOnly]
		public EntityStorageInfoLookup EntityExists;

		public Entity SEPlayerSingleton;

		public Entity GlobalParticleSystemBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref PhysicsCollider collider, ref SpellMovementComponentData movement, ref SpellConfigComponentData config, ref SpellComponentData componentData, ref SpellElementEffectComponentData elementEffect, Entity entity, DynamicBuffer<HarpoonChainData> lineBuffer, DynamicBuffer<HighspeedHarpoonSphereCastPos> collPos, [ChunkIndexInQuery] int chunkIndex)
		{
			if (!data.IsInitialized)
			{
				data.IsInitialized = true;
				data.EndPinned = true;
				data.ShowBubble = true;
				data.ShowRelicLightning = RelicData.DamageRate > 0f;
				data.HarpoonState = HarpoonState.Shooting;
				data.ChainState = ChainState.Spawning;
				data.ChainLength = 4f + config.Radius.CalculateWithNewBaseValue(4f);
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "HarpoonShoot";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1030, in seName)));
				SpellTools.EnableSpellReboundCollider(in collider);
				if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					SpellTools.DisableSpellTrigger(in collider);
					SpellTools.DisableSpellReboundCollider(in collider);
					data.ChainLength = 7f + movement.AroundRadius + config.Radius.Calculate();
				}
				data.RebounceTimeCurrent = movement.ReboundCount;
				movement.ReboundCount = 99999;
				config.Penetrate.Base = 99999;
				data.MouseLerpVelocity = 60f * movement.Direction;
				data.HarpoonHideRate = -999f;
				data.StartPos = transform.Position;
				data.NodeDistance = data.ChainLength / 30f;
				data.Iterations = 30;
				data.VelocityDampen = 0.9f;
				data.GravityStrength = new float3(0f, 0f, 30f);
				data.GroundZ = -0.05f;
				data.AllowStretch = true;
				data.StretchStiffness = 0.9f;
				if (EffectCtrlLookUp.TryGetComponent(componentData.SpellEffectEntity, out var componentData2))
				{
					data.HarpoonMat = componentData2.Effect1;
				}
				if (movement.IsFallSpell)
				{
					movement.CurrentFallSpeed = 420f / math.sqrt(53f);
					movement.OriginalSpellHorizontalSpeed = 60f;
					movement.Speed = 120f / math.sqrt(53f);
					transform.Position = new float3(transform.Position.xy, -7f);
					if (movement.Type == SpellSpecialMovementType.Rotation)
					{
						transform.Position = new float3(movement.AroundCenter.xy, -7f);
					}
					float num = Random.random.NextFloat(config.Scatter * 0.06f);
					float2 xy = transform.Position.xy + movement.Direction.xy * 2f + Random.random.NextFloat2Direction() * num;
					movement.Direction = math.normalizesafe(new float3(xy, 0f) - transform.Position);
					data.ChainLength = 15f;
					if (movement.Type == SpellSpecialMovementType.Rotation)
					{
						data.ChainLength = math.sqrt(49f + movement.AroundRadius * movement.AroundRadius);
						data.ChainLength += 3f;
						movement.CurrentFallSpeed = 120f / math.sqrt(53f);
						movement.Speed = 420f / math.sqrt(53f);
					}
					data.HarpoonState = HarpoonState.FallingFirst;
					data.GravityStrength = new float3(0f, 0f, 30f);
					data.FallStartPos = transform.Position;
					data.StartPos = transform.Position;
					data.NodeDistance = data.ChainLength / 20f;
					data.MouseLerpVelocity = new float3((movement.Direction * 60f).xy, 0f);
				}
				PlayGlobalEffectDirectionScale(chunkIndex, "Shoot", 4024, data.StartPos, movement.Direction, 1f, in config);
				lineBuffer.Clear();
				lineBuffer.Add(new HarpoonChainData
				{
					Position = data.StartPos,
					PrevPosition = data.StartPos
				});
				data.ShowGate = true;
			}
			if (movement.IsFallSpell || movement.Type != SpellSpecialMovementType.Rotation || data.HarpoonState != 0)
			{
				return;
			}
			int num2 = 4;
			collPos.Clear();
			collPos.ResizeUninitialized(num2);
			float num3 = 60f * config.DurationTimer;
			float num4 = 60f * math.max(0f, DeltaTime) / (float)(num2 - 1);
			float num5 = num3;
			if (TransformLookup.TryGetComponent(movement.AroundTarget, out var componentData3))
			{
				movement.AroundCenter = componentData3.Position;
			}
			int num6 = 0;
			while (num6 < num2)
			{
				collPos[num6] = new HighspeedHarpoonSphereCastPos
				{
					Position = movement.AroundCenter + PosAtArcXY(0.1f, movement.AroundRadius, movement.AroundAngle, num5)
				};
				num6++;
				num5 += num4;
			}
			NativeList<ColliderCastHit> hitList = new NativeList<ColliderCastHit>(Allocator.Temp);
			for (int i = 1; i < num2; i++)
			{
				HighspeedHarpoonSphereCastPos highspeedHarpoonSphereCastPos = collPos[i - 1];
				ref float3 position = ref highspeedHarpoonSphereCastPos.Position;
				HighspeedHarpoonSphereCastPos highspeedHarpoonSphereCastPos2 = collPos[i];
				ref float3 position2 = ref highspeedHarpoonSphereCastPos2.Position;
				float width = 0.2f;
				SpellTools.GetAttackableEntitiesInSphereCast(in position, in position2, in width, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList);
				if (hitList.Length <= 0)
				{
					continue;
				}
				for (int j = 0; j < hitList.Length; j++)
				{
					Entity entity2 = hitList[j].Entity;
					if (EntityExists.Exists(entity2) && TransformLookup.TryGetComponent(entity2, out var componentData4))
					{
						TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
						info.spell.HitPosition = componentData4.Position;
						info.spell.IgnoreHitEffect = false;
						ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
						Entity target = hitList[j].Entity;
						cMD2.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
						if (UnitPropertyLookup.HasComponent(hitList[j].Entity) && CheckHitCanCatch(hitList[j].Entity))
						{
							data.CatchEntity = hitList[j].Entity;
							hitList.Dispose();
							return;
						}
					}
				}
				hitList.Clear();
			}
			hitList.Dispose();
		}

		private float3 PosAtArcXY(float r0, float R, float ang0, float s)
		{
			float num = 0.3f;
			float num2 = Kappa(num);
			float ang;
			if (num2 * (R - r0) <= 0f)
			{
				ang = ang0 + s / math.max(R, 1E-06f);
				return ToXY(R, ang);
			}
			float num3 = (R - r0) / math.max(num2, 1E-06f);
			float num4;
			if (s <= num3)
			{
				num4 = math.max(1E-06f, r0 + num2 * s);
				ang = ang0 + math.rcp(math.max(num, 1E-06f)) * math.log(num4 / math.max(r0, 1E-06f));
			}
			else
			{
				num4 = R;
				ang = ang0 + math.rcp(math.max(num, 1E-06f)) * math.log(math.max(R, 1E-06f) / math.max(r0, 1E-06f)) + (s - num3) * math.rcp(math.max(R, 1E-06f));
			}
			return ToXY(num4, ang);
		}

		private float Kappa(float b)
		{
			return b / math.sqrt(1f + b * b);
		}

		private float3 ToXY(float r, float ang)
		{
			return new float3(r * new float2(math.cos(ang), math.sin(ang)), 0f);
		}

		private bool CheckHitCanCatch(Entity hitEntity)
		{
			if (!EntityExists.Exists(hitEntity) || !UnitPropertyLookup.HasComponent(hitEntity))
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

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			BufferAccessor<HarpoonChainData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__HarpoonChainData_RW_BufferTypeHandle);
			BufferAccessor<HighspeedHarpoonSphereCastPos> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell4024DaveHarpoonData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i);
					ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i);
					ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
					DynamicBuffer<HarpoonChainData> lineBuffer = bufferAccessor[i];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> collPos = bufferAccessor2[i];
					Execute(ref data, ref transform, ref collider, ref movement, ref config, ref componentData, ref elementEffect, entity, lineBuffer, collPos, chunkIndexInQuery);
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
						ref Spell4024DaveHarpoonData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
						DynamicBuffer<HarpoonChainData> lineBuffer2 = bufferAccessor[nextRangeBegin];
						DynamicBuffer<HighspeedHarpoonSphereCastPos> collPos2 = bufferAccessor2[nextRangeBegin];
						Execute(ref data2, ref transform2, ref collider2, ref movement2, ref config2, ref componentData2, ref elementEffect2, entity2, lineBuffer2, collPos2, chunkIndexInQuery);
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
					ref Spell4024DaveHarpoonData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j);
					ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j);
					ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
					DynamicBuffer<HarpoonChainData> lineBuffer3 = bufferAccessor[j];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> collPos3 = bufferAccessor2[j];
					Execute(ref data3, ref transform3, ref collider3, ref movement3, ref config3, ref componentData3, ref elementEffect3, entity3, lineBuffer3, collPos3, chunkIndexInQuery);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell4024DaveHarpoonData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k);
					ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k);
					ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
					DynamicBuffer<HarpoonChainData> lineBuffer4 = bufferAccessor[k];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> collPos4 = bufferAccessor2[k];
					Execute(ref data4, ref transform4, ref collider4, ref movement4, ref config4, ref componentData4, ref elementEffect4, entity4, lineBuffer4, collPos4, chunkIndexInQuery);
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

	[BurstCompile]
	[CompilerGenerated]
	public struct Spell4024DaveHarpoonMoveJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell4024DaveHarpoonData> __Spell4024DaveHarpoonData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<Spell4024DaveHarpoonMultiShootData> __Spell4024DaveHarpoonMultiShootData_RO_ComponentTypeHandle;

				public BufferTypeHandle<HarpoonChainData> __HarpoonChainData_RW_BufferTypeHandle;

				public BufferTypeHandle<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferTypeHandle;

				public BufferTypeHandle<HighspeedHarpoonSphereCastPos> __HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
					__Spell4024DaveHarpoonMultiShootData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonMultiShootData>(isReadOnly: true);
					__HarpoonChainData_RW_BufferTypeHandle = state.GetBufferTypeHandle<HarpoonChainData>();
					__SpellGameObjectEffectLink_RW_BufferTypeHandle = state.GetBufferTypeHandle<SpellGameObjectEffectLink>();
					__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle = state.GetBufferTypeHandle<HighspeedHarpoonSphereCastPos>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
					__Spell4024DaveHarpoonMultiShootData_RO_ComponentTypeHandle.Update(ref state);
					__HarpoonChainData_RW_BufferTypeHandle.Update(ref state);
					__SpellGameObjectEffectLink_RW_BufferTypeHandle.Update(ref state);
					__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell4024DaveHarpoonMultiShootData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4024DaveHarpoonData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HarpoonChainData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGameObjectEffectLink>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HighspeedHarpoonSphereCastPos>();
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
			public void Run(ref Spell4024DaveHarpoonMoveJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4024DaveHarpoonMoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4024DaveHarpoonMoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonMoveJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4024DaveHarpoonMoveJob job, EntityManager entityManager)
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

		public float DeltaTime;

		public GlobalRandom Random;

		public Entity EffectRequireBufferEntity;

		public Entity EffectRecycleBufferEntity;

		public float3 MousePosition;

		public LightningHarpoonRelicData RelicData;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[ReadOnly]
		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		public SpellSingleton SpellSingleton;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<Spell4024HarpoonHideUnderGroundMat> HarpoonUnderGroundMatDataLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<Spell4024HarpoonOverlayColor> HarpoonGlowMatDataLookUp;

		[ReadOnly]
		public EntityStorageInfoLookup EntityExists;

		public Entity UnfollowEffectRequireBufferEntity;

		public Entity SEPlayerSingleton;

		public Entity Spell3101Buffer;

		public Entity GlobalParticleSystemBufferEntity;

		public float3 PlayerShootPosition;

		public float3 PlayerDir;

		public Entity PlayerEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(Entity entity, ref LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData componentData, ref SpellElementEffectComponentData elementEffect, ref Spell4024DaveHarpoonData data, ref PhysicsCollider collider, ref PhysicsVelocity velocity, in Spell4024DaveHarpoonMultiShootData multiShootData, DynamicBuffer<HarpoonChainData> lineBuffer, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, DynamicBuffer<HighspeedHarpoonSphereCastPos> rotatePosBuffer, [ChunkIndexInQuery] int chunkIndex)
		{
			if (!movement.IsFallSpell)
			{
				if (!componentData.IsSplitSpell)
				{
					SpellStartOffsetGet(in multiShootData, in data.ShootPos, out data.StartPos);
				}
				else if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					data.StartPos = new float3(movement.AroundCenter.xy, -0.3f);
				}
			}
			else if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				if (TransformLookup.TryGetComponent(movement.AroundTarget, out var componentData2))
				{
					movement.AroundCenter = componentData2.Position;
				}
				data.StartPos = new float3(movement.AroundCenter.xy, -7f);
			}
			float3 arrayToBullet = transform.Position - data.StartPos;
			float3 @float = Unity.Mathematics.float3.zero;
			float num = 0f;
			switch (data.HarpoonState)
			{
			case HarpoonState.Shooting:
				num = config.Duration.Calculate() + 0.3f;
				if (config.DurationTimer >= num)
				{
					CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
					DurationOutWhenShoot(ref transform, ref data, lineBuffer, ref config, ref collider);
					@float = 10f * movement.Direction;
					break;
				}
				if (math.length(arrayToBullet) >= data.ChainLength)
				{
					CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
					ChainLengthMaxWhenShoot(in arrayToBullet, ref data, ref transform, ref config, ref movement, ref collider, lineBuffer);
					break;
				}
				@float = 60f * movement.Direction;
				switch (movement.Type)
				{
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseOwner:
				{
					GetValidChasePosition(ref movement, in componentData, in config, transform, out var hasTarget2, out var targetPosition2);
					if (hasTarget2)
					{
						float3 source = movement.Direction;
						float3 target = DTool.IgnoreZDir(in targetPosition2, in transform.Position);
						movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, 60f * movement.ChaseRotateSpeed * DeltaTime);
					}
					break;
				}
				case SpellSpecialMovementType.ChaseMouse:
				{
					float3 end2 = DTool.IgnoreZDir(in MousePosition, in transform.Position) * 60f;
					data.MouseLerpVelocity = DTool.Lerp(in data.MouseLerpVelocity, in end2, 30f * DeltaTime * movement.ChaseMouseLerpSpeed);
					@float = data.MouseLerpVelocity;
					movement.Direction = math.normalizesafe(@float);
					break;
				}
				case SpellSpecialMovementType.Rotation:
					@float = Unity.Mathematics.float3.zero;
					if (data.CatchEntity != Entity.Null)
					{
						HarpoonGoCatch(ref data, ref config, in movement, ref transform, chunkIndex);
						ChainGoCatching(ref data);
						SpellTools.DisableSpellTrigger(in collider);
						SpellTools.DisableSpellReboundCollider(in collider);
					}
					else
					{
						transform.Position = rotatePosBuffer[rotatePosBuffer.Length - 1].Position;
						movement.Direction = DTool.RotateDir(math.normalizesafe(transform.Position - data.ShootPos), 90f);
					}
					break;
				}
				data.EndPos = transform.Position;
				break;
			case HarpoonState.Slowdown:
				num = 0.2f;
				@float = 30f * movement.Direction;
				if (config.DurationTimer >= num)
				{
					@float = Unity.Mathematics.float3.zero;
					CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
					HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
				}
				else
				{
					@float *= (num - config.DurationTimer) / num;
					data.EndPos = transform.Position;
				}
				break;
			case HarpoonState.Waiting:
			{
				num = 0.5f;
				if (config.DurationTimer >= num)
				{
					HarpoonGoReturning(ref data, in transform);
					ChainGoReturning(ref data, ref movement, ref transform, ref config, lineBuffer);
				}
				TrySetHeadPosition(lineBuffer, ref transform);
				float degree = DTool.GetDegree(movement.Direction);
				float degree2 = DTool.GetDegree(math.normalizesafe(lineBuffer[lineBuffer.Length - 1].Position - lineBuffer[lineBuffer.Length - 2].Position));
				movement.Direction = DTool.GetDir(DTool.Lerp(degree, degree2, 0.1f) * (MathF.PI / 180f));
				break;
			}
			case HarpoonState.Returning:
				TrySetHeadPosition(lineBuffer, ref transform);
				TrySetHeadDirection(lineBuffer, ref movement);
				break;
			case HarpoonState.WallHitRebouncing:
			{
				@float = Unity.Mathematics.float3.zero;
				if (math.length(arrayToBullet) >= data.ChainLength)
				{
					transform.Position = data.ShootPos + math.normalizesafe(arrayToBullet) * data.ChainLength;
					ChainGoStraightening(ref data, ref transform, ref config, lineBuffer);
					HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
					break;
				}
				num = 0.5f;
				if (config.DurationTimer >= num)
				{
					HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
					break;
				}
				float num12 = (num - config.DurationTimer) / num;
				transform.Position += num12 * data.WallHitReboudVelocity * DeltaTime;
				data.EndPos = transform.Position;
				movement.Direction = DTool.RotateDir(movement.Direction, num12 * data.RebounceRotateRandomResult);
				break;
			}
			case HarpoonState.Catching:
			{
				num = 2f + config.Duration.Calculate();
				if (config.DurationTimer >= num)
				{
					if (math.length(arrayToBullet) >= data.NodeDistance * (float)(lineBuffer.Length - 1))
					{
						for (int i = 0; i < 30 - lineBuffer.Length; i++)
						{
							lineBuffer.Add(new HarpoonChainData
							{
								Position = transform.Position,
								PrevPosition = transform.Position
							});
						}
					}
					CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
					HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
					ChainGoWaiting(ref data);
					break;
				}
				if (math.length(arrayToBullet) >= data.ChainLength + 2f || !EntityExists.Exists(data.CatchEntity) || !TransformLookup.TryGetComponent(data.CatchEntity, out var componentData3))
				{
					CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
					HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
					ChainGoWaiting(ref data);
					break;
				}
				data.ShakeVelocity = Random.random.NextFloat3(new float3(-0.02f, -0.02f, 0f), new float3(0.02f, 0.02f, 0f));
				transform.Position = componentData3.Position + data.ShakeVelocity + data.CatchPosRandom;
				data.EndPos = transform.Position;
				if (UnitPropertyLookup.HasComponent(data.CatchEntity))
				{
					UnitPropertyLookup.GetRefRW(data.CatchEntity).ValueRW.TakeKnockback(data.DragVelocity);
				}
				data.CatchDmgLoop += DeltaTime;
				if (data.CatchDmgLoop >= 0.1f)
				{
					data.CatchDmgLoop -= 0.1f;
					PlayGlobalEffect(chunkIndex, "HoldHit", 4024, transform.Position, in config);
					PlayAudio(chunkIndex, "LoopHIt");
					TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
					info.spell.HitPosition = componentData3.Position;
					info.spell.IgnoreHitEffect = false;
					info.damage /= 5f;
					CMD.TryAttackEntity(chunkIndex, in data.CatchEntity, in info, in UnitPropertyLookup, in SpellConfigLookup);
				}
				CheckThunderRelic(chunkIndex, entity, ref data, in transform, in movement, in componentData, in config, in elementEffect);
				break;
			}
			case HarpoonState.FallingFirst:
			{
				@float = movement.Direction * 60f;
				float num7 = 7f - movement.CurrentFallSpeed * config.DurationTimer;
				switch (movement.Type)
				{
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseOwner:
				{
					GetValidChasePosition(ref movement, in componentData, in config, transform, out var hasTarget3, out var targetPosition3);
					if (hasTarget3)
					{
						float3 x3 = new float3(@float.xy, 0f);
						float num10 = math.length(x3);
						float3 source = math.normalizesafe(x3);
						float3 target = DTool.IgnoreZDir(in targetPosition3, in transform.Position);
						@float = new float3((DTool.DirMoveTowardsIgnoreZ(in source, in target, num10 * movement.ChaseRotateSpeed * DeltaTime) * num10).xy, @float.z);
					}
					break;
				}
				case SpellSpecialMovementType.ChaseMouse:
				{
					float num11 = math.length(new float3(MousePosition.xy - transform.Position.xy, 0f));
					float3 end3 = DTool.IgnoreZDir(in MousePosition, in transform.Position) * num11;
					data.MouseLerpVelocity = DTool.Lerp(in data.MouseLerpVelocity, in end3, num11 * DeltaTime * movement.ChaseMouseLerpSpeed);
					@float = new float3(data.MouseLerpVelocity.xy, @float.z);
					break;
				}
				case SpellSpecialMovementType.Rotation:
				{
					float num8 = 7f - config.DurationTimer * movement.CurrentFallSpeed;
					if (num8 < 0f)
					{
						num8 = 0f;
					}
					float num9 = movement.Speed * DeltaTime / movement.AroundRadius;
					data.FallRotateCurrentR = (7f - num8) / 7f * movement.AroundRadius;
					movement.AroundAngle += num9 * 57.29578f;
					float3 dir = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f));
					transform.Position.xy = movement.AroundCenter.xy + math.normalizesafe(dir).xy * data.FallRotateCurrentR;
					float3 x2 = DTool.RotateDir(dir, 90f) * movement.Speed + new float3(0f, 0f, movement.CurrentFallSpeed);
					movement.Direction = math.normalizesafe(x2);
					break;
				}
				}
				transform.Position += @float * DeltaTime;
				transform.Position.z = 0f - num7;
				if (transform.Position.z >= -0.3f)
				{
					HarpoonGoFallLanding(ref data, entity, ref transform, in config, in componentData, in movement, in elementEffect, chunkIndex);
					if (movement.Type == SpellSpecialMovementType.Rotation)
					{
						data.StretchStiffness = 0.1f;
						data.VelocityDampen = 0.9f;
						data.AllowStretch = true;
						ChainGoFallStraightening(ref data, ref config);
						ClearChainVelocity(lineBuffer);
					}
					else
					{
						ChainGoFallStraightReturning(ref data, ref config);
					}
					if (data.CatchEntity == Entity.Null && movement.Direction.y > 0f)
					{
						movement.Direction = new float3(movement.Direction.x, movement.Direction.y * -1f, movement.Direction.z);
					}
				}
				else
				{
					data.EndPos = transform.Position;
					@float = Unity.Mathematics.float3.zero;
				}
				break;
			}
			case HarpoonState.FallLanding:
			{
				transform.Position = data.FallGroundPos;
				if (TransformLookup.TryGetComponent(data.CatchEntity, out var componentData4))
				{
					transform.Position = componentData4.Position + new float3(0f, 0f, -0.3f);
					CheckThunderRelic(chunkIndex, entity, ref data, in transform, in movement, in componentData, in config, in elementEffect);
				}
				if (data.ChainState == ChainState.FallStraightening)
				{
					data.ShakeVelocity = Random.random.NextFloat3(new float3(-0.02f, -0.02f, 0f), new float3(0.02f, 0.02f, 0f));
					transform.Position += data.ShakeVelocity;
				}
				data.EndPos = transform.Position;
				break;
			}
			case HarpoonState.FallRebouncing:
			{
				float3 float2 = new float3(0f, 0f, 50f);
				transform.Position += data.FallRebounceVelocity * DeltaTime + 0.5f * float2 * DeltaTime * DeltaTime;
				data.FallRebounceVelocity += float2 * DeltaTime;
				switch (movement.Type)
				{
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseOwner:
				{
					GetValidChaseEnemyPosition(ref movement, in config, in transform, out var hasTarget, out var targetPosition);
					if (hasTarget)
					{
						float3 x = new float3(data.FallRebounceVelocity.xy, 0f);
						float num5 = math.length(x);
						float3 source = math.normalizesafe(x);
						float3 target = DTool.IgnoreZDir(in targetPosition, in transform.Position);
						data.FallRebounceVelocity = new float3((DTool.DirMoveTowardsIgnoreZ(in source, in target, num5 * movement.ChaseRotateSpeed * DeltaTime * 3f) * num5).xy, data.FallRebounceVelocity.z);
					}
					break;
				}
				case SpellSpecialMovementType.ChaseMouse:
				{
					float num6 = math.length(new float3(MousePosition.xy - transform.Position.xy, 0f));
					float3 end = DTool.IgnoreZDir(in MousePosition, in transform.Position) * num6;
					data.MouseLerpVelocity = DTool.Lerp(in data.MouseLerpVelocity, in end, num6 * DeltaTime * movement.ChaseMouseLerpSpeed * 5f);
					data.FallRebounceVelocity = new float3(data.MouseLerpVelocity.xy, data.FallRebounceVelocity.z);
					break;
				}
				case SpellSpecialMovementType.Rotation:
				{
					velocity.Linear = Unity.Mathematics.float3.zero;
					float num2 = movement.Speed * 0.08f;
					float2 xy = math.normalizesafe(arrayToBullet.xy);
					float2 float3 = num2 * DTool.RotateDir(new float3(xy, 0f), -90f).xy;
					transform.Position = new float3(transform.Position.xy + float3 * DeltaTime, transform.Position.z);
					arrayToBullet = transform.Position - data.StartPos;
					float num3 = movement.AroundRadius - math.length(arrayToBullet.xy);
					float2 float4 = Unity.Mathematics.float2.zero;
					float num4 = math.clamp(num2 * 0.1f, 0.1f, 1f);
					xy = math.normalizesafe(arrayToBullet.xy);
					if (math.abs(num3) > 0.01f)
					{
						float4 = num3 * num4 * xy;
					}
					transform.Position = new float3(transform.Position.xy + float4, transform.Position.z);
					data.FallRebounceVelocity = new float3(float3, data.FallRebounceVelocity.z);
					data.GravityStrength = new float3(float3 * 10f, 50f);
					break;
				}
				}
				if (transform.Position.z >= -0.3f && data.FallRebounceVelocity.z > 0f)
				{
					HarpoonGoFallLanding(ref data, entity, ref transform, in config, in componentData, in movement, in elementEffect, chunkIndex);
					if (movement.Type == SpellSpecialMovementType.Rotation)
					{
						ChainGoFallStraightening(ref data, ref config);
						ClearChainVelocity(lineBuffer);
					}
					else
					{
						ChainGoFallStraightReturning(ref data, ref config);
					}
				}
				else
				{
					data.EndPos = transform.Position;
				}
				break;
			}
			}
			switch (data.ChainState)
			{
			case ChainState.Spawning:
				if (movement.IsFallSpell)
				{
					ChainSpawnFall(movement.Direction, data.FallStartPos, lineBuffer);
				}
				else
				{
					ChainSpawnNormal(movement.Direction, data.StartPos, lineBuffer);
				}
				data.ChainState = ChainState.Waiting;
				break;
			case ChainState.Returning:
			{
				ChainLineReturn(40f, data.StartPos, ref data, ref config, lineBuffer, out var flag);
				if (flag)
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
					return;
				}
				break;
			}
			case ChainState.Straightening:
				if (config.DurationTimer > 0.1f)
				{
					ChainGoWaiting(ref data);
				}
				break;
			case ChainState.Catching:
			{
				if (lineBuffer.Length > 20)
				{
					for (int l = 0; l < 2; l++)
					{
						lineBuffer.RemoveAt(0);
					}
					HarpoonChainData value2 = lineBuffer[0];
					value2.Position = data.ShootPos;
					lineBuffer[0] = value2;
					data.StartPos = data.ShootPos;
				}
				float num15 = math.length(arrayToBullet.xy);
				float3 float10 = math.normalizesafe(new float3(-arrayToBullet.xy, 0f));
				if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					float valueToClamp = num15 - movement.AroundRadius;
					valueToClamp = math.clamp(valueToClamp, -1f, 1f);
					data.DragVelocity = float10 * valueToClamp + DTool.RotateDir(float10, 90f);
					data.GravityStrength = data.DragVelocity * 100f;
				}
				else if ((float)lineBuffer.Length * data.NodeDistance <= num15)
				{
					float valueToClamp2 = num15 - (float)lineBuffer.Length * data.NodeDistance;
					valueToClamp2 = math.clamp(valueToClamp2, 0.1f, 1f);
					data.DragVelocity = float10 * valueToClamp2;
				}
				else
				{
					data.DragVelocity = Unity.Mathematics.float3.zero;
				}
				data.EndPos = transform.Position;
				break;
			}
			case ChainState.FallStraightening:
			{
				if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					float3 forceDir = DTool.RotateDir(new float3(math.normalizesafe(arrayToBullet.xy), 0f), -90f);
					if (config.DurationTimer > 0.5f)
					{
						HarpoonDoPullOutExplosion(forceDir, ref data, entity, in transform, in componentData, in config, in movement, in elementEffect, chunkIndex);
						data.HarpoonHideRate = -999f;
						if (data.RebounceTimeCurrent > 0)
						{
							data.RebounceTimeCurrent--;
							movement.CurrentFallSpeed = -25f;
							data.HarpoonState = HarpoonState.FallRebouncing;
							data.ChainState = ChainState.ChainRecovery;
							config.DurationTimer = 0f;
							data.VelocityDampen = 0.9f;
							data.FallRebounceVelocity = new float3(forceDir.xy * movement.Speed * 0.08f, -25f);
						}
						else
						{
							CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
							HarpoonGoReturning(ref data, in transform);
							ChainGoFallWaitReturn(math.length(arrayToBullet), ref data, ref config, lineBuffer);
						}
					}
					else
					{
						float3 x4 = transform.Position - movement.AroundCenter;
						float3 float5 = (movement.AroundRadius - math.length(x4)) * math.normalizesafe(x4);
						float3 float6 = new float3(forceDir.xy + float5.xy, 0f);
						data.GravityStrength = new float3(float6.xy * 50f, -100f);
						if (TransformLookup.HasComponent(data.CatchEntity))
						{
							UnitPropertyLookup.GetRefRW(data.CatchEntity).ValueRW.TakeKnockback(float6 * 0.1f);
						}
					}
					break;
				}
				if (config.DurationTimer > 0.5f)
				{
					float3 float7 = math.normalizesafe(-arrayToBullet);
					HarpoonDoPullOutExplosion(float7, ref data, entity, in transform, in componentData, in config, in movement, in elementEffect, chunkIndex);
					data.HarpoonHideRate = -999f;
					if (data.RebounceTimeCurrent > 0)
					{
						for (int k = 1; k < lineBuffer.Length - 1; k++)
						{
							HarpoonChainData harpoonChainData = lineBuffer[k];
							harpoonChainData.PrevPosition = harpoonChainData.Position - float7 * 10f;
						}
						data.RebounceTimeCurrent--;
						data.HarpoonState = HarpoonState.FallRebouncing;
						data.ChainState = ChainState.ChainRecovery;
						data.FallRebounceVelocity = new float3((float7 * 20f).xy, -25f);
						config.DurationTimer = 0f;
						if (movement.Type == SpellSpecialMovementType.ChaseMouse)
						{
							data.MouseLerpVelocity = new float3(data.FallRebounceVelocity.xy, 0f);
						}
					}
					else
					{
						CheckThunderStone(chunkIndex, entity, ref data, in transform, in componentData, in movement, in config, in elementEffect);
						HarpoonGoReturning(ref data, in transform);
						ChainGoFallWaitReturn(math.length(arrayToBullet), ref data, ref config, lineBuffer);
					}
				}
				float num13 = math.length(arrayToBullet);
				if (num13 < (float)(lineBuffer.Length - 1) * data.NodeDistance)
				{
					data.NodeDistance = num13 / (float)lineBuffer.Length;
				}
				if (TransformLookup.HasComponent(data.CatchEntity))
				{
					UnitPropertyLookup.GetRefRW(data.CatchEntity).ValueRW.TakeKnockback(new float3(-arrayToBullet.xy, 0f) * 0.1f);
				}
				break;
			}
			case ChainState.FallStraightReturning:
			{
				if (!(config.DurationTimer > 0.3f))
				{
					break;
				}
				if (math.length(arrayToBullet) >= (float)(lineBuffer.Length - 1) * data.NodeDistance)
				{
					ChainGoFallStraightening(ref data, ref config);
					break;
				}
				for (int m = 0; m < 1; m++)
				{
					lineBuffer.RemoveAt(0);
				}
				HarpoonChainData value3 = lineBuffer[0];
				value3.Position = data.FallStartPos;
				lineBuffer[0] = value3;
				break;
			}
			case ChainState.ChainRecovery:
			{
				float num14 = math.length(arrayToBullet);
				if ((float)(lineBuffer.Length - 1) * data.NodeDistance <= num14 + 2f)
				{
					lineBuffer.Insert(0, new HarpoonChainData
					{
						Position = data.FallStartPos,
						PrevPosition = data.FallStartPos
					});
				}
				if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					float3 float8 = DTool.RotateDir(new float3(math.normalizesafe(arrayToBullet.xy), 0f), -90f);
					float3 x5 = transform.Position - movement.AroundCenter;
					float3 float9 = (movement.AroundRadius - math.length(x5)) * math.normalizesafe(x5);
					data.GravityStrength = new float3(new float3(float8.xy + float9.xy, 0f).xy * 100f, -100f);
				}
				break;
			}
			case ChainState.FallWaitReturn:
				if (lineBuffer.Length < 30)
				{
					lineBuffer.Insert(0, new HarpoonChainData
					{
						Position = data.FallStartPos,
						PrevPosition = data.FallStartPos
					});
					break;
				}
				if (config.DurationTimer > 1f)
				{
					ChainGoFallReturning(ref data, ref config);
				}
				data.NodeDistance = DTool.Lerp(data.NodeDistance, 1f / 3f, 0.1f);
				break;
			case ChainState.FallReturning:
			{
				if (config.DurationTimer < 0.3f)
				{
					break;
				}
				bool flag = false;
				for (int j = 0; j < 1; j++)
				{
					lineBuffer.RemoveAt(0);
					if (lineBuffer.Length <= 1)
					{
						flag = true;
						break;
					}
				}
				HarpoonChainData value = lineBuffer[0];
				value.Position = data.FallStartPos;
				lineBuffer[0] = value;
				if (flag)
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
					return;
				}
				break;
			}
			}
			Entity spellEffectEntity = componentData.SpellEffectEntity;
			if (EntityExists.Exists(spellEffectEntity) && TransformLookup.HasComponent(spellEffectEntity))
			{
				ref LocalTransform valueRW = ref TransformLookup.GetRefRW(componentData.SpellEffectEntity).ValueRW;
				float3 rootPosition = movement.Direction;
				float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				rootPosition += layerPosition;
				float z = math.atan2(rootPosition.y, rootPosition.x);
				valueRW.Rotation = quaternion.Euler(0f, 0f, z);
			}
			velocity.Linear = @float;
			CheckEffectLink("LandDust", ref data.ShowDust, ref data.RequiredDust, ref data.DustEffect, entity, in config, linkBuffer, chunkIndex);
			CheckEffectLink("StartGate", ref data.ShowGate, ref data.RequiredGate, ref data.GateEffect, entity, in config, linkBuffer, chunkIndex);
			ref bool showRelicLightning = ref data.ShowRelicLightning;
			ref bool requiredRelicLightning = ref data.RequiredRelicLightning;
			ref UnityObjectRef<GameObject> relicLightningEffect = ref data.RelicLightningEffect;
			SpellColorType colorType = SpellColorType.Thunder;
			CheckUniqueEffectLink("LightningHarpoon", ref showRelicLightning, ref requiredRelicLightning, ref relicLightningEffect, entity, in colorType, linkBuffer, chunkIndex);
			SetHideSwordRateWithScale(data.HarpoonHideRate, data.HarpoonMat, in transform, 0.75f);
		}

		private void HarpoonGoSlowdown(ref SpellConfigComponentData config, ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref PhysicsCollider collider)
		{
			config.DurationTimer = 0f;
			data.HarpoonState = HarpoonState.Slowdown;
			SpellTools.DisableSpellReboundCollider(in collider);
			SpellTools.DisableSpellTrigger(in collider);
		}

		private void HarpoonGoReturning(ref Spell4024DaveHarpoonData data, in LocalTransform transform)
		{
			data.HarpoonState = HarpoonState.Returning;
			data.ShowBubble = false;
		}

		private void HarpoonGoWaiting(ref SpellConfigComponentData config, ref Spell4024DaveHarpoonData data, ref SpellMovementComponentData movement, ref LocalTransform transform, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			config.DurationTimer = 0f;
			data.HarpoonState = HarpoonState.Waiting;
			data.GravityStrength = new float3(0f, 0f, 30f);
			float3 @float = new float3(math.normalizesafe(movement.Direction.xy) * data.NodeDistance + transform.Position.xy, lineBuffer[lineBuffer.Length - 1].Position.z);
			lineBuffer.Add(new HarpoonChainData
			{
				Position = @float,
				PrevPosition = @float
			});
			data.EndPos = @float;
			ClearChainVelocity(lineBuffer);
			data.VelocityDampen = 0.1f;
		}

		private void HarpoonGoCatch(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config, in SpellMovementComponentData movement, ref LocalTransform transform, int chunkIndex)
		{
			config.DurationTimer = 0f;
			config.ColorType.ColorEnumToString(out var result);
			PlayUnfollowEffect("DirectHit", 4024, transform.Position, result, DTool.GetDegree(math.normalizesafe(new float3(movement.Direction.xy, 0f))), 1f, SpellEffectSystem.ScaleMode.Scale, chunkIndex);
			PlayAudio(chunkIndex, "HarpoonHIt");
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

		private void TrySetHeadPosition(DynamicBuffer<HarpoonChainData> lineData, ref LocalTransform headTrans)
		{
			if (lineData.Length >= 2)
			{
				headTrans.Position = lineData[lineData.Length - 2].Position;
			}
		}

		private void TrySetHeadDirection(DynamicBuffer<HarpoonChainData> lineData, ref SpellMovementComponentData movement)
		{
			if (lineData.Length >= 2)
			{
				movement.Direction = math.normalizesafe(lineData[lineData.Length - 1].Position - lineData[lineData.Length - 2].Position);
			}
		}

		private void DurationOutWhenShoot(ref LocalTransform transform, ref Spell4024DaveHarpoonData data, DynamicBuffer<HarpoonChainData> lineBuffer, ref SpellConfigComponentData config, ref PhysicsCollider collider)
		{
			transform.Position = lineBuffer[lineBuffer.Length - 1].Position;
			HarpoonGoSlowdown(ref config, ref data, ref transform, ref collider);
		}

		private void ChainLengthMaxWhenShoot(in float3 arrayToBullet, ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref PhysicsCollider collider, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			transform.Position = data.StartPos + math.normalizesafe(arrayToBullet) * data.ChainLength;
			ChainGoStraightening(ref data, ref transform, ref config, lineBuffer);
			HarpoonGoWaiting(ref config, ref data, ref movement, ref transform, lineBuffer);
			SpellTools.DisableSpellReboundCollider(in collider);
			SpellTools.DisableSpellTrigger(in collider);
		}

		private void HarpoonGoFallLanding(ref Spell4024DaveHarpoonData data, Entity spellEntity, ref LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
		{
			data.HarpoonState = HarpoonState.FallLanding;
			transform.Position.z = -0.3f;
			data.FallGroundPos = transform.Position;
			DoGroundExplosion(transform.Position, in config, out var hitList, chunkIndex);
			HitDamageApplyExplosion(spellEntity, in hitList, in transform.Position, in componentData, in config, in movement, in transform, in elementEffect, chunkIndex);
			hitList.Clear();
			ref float3 position = ref transform.Position;
			float radius = 0.5f;
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList);
			data.HarpoonHideRate = 0.1f;
			if (hitList.Length > 0)
			{
				for (int i = 0; i < hitList.Length; i++)
				{
					if (CheckHitCanCatch(hitList[i]))
					{
						config.ColorType.ColorEnumToString(out var result);
						PlayUnfollowEffect("DirectHit", 4024, transform.Position, result, DTool.GetDegree(math.normalizesafe(new float3(movement.Direction.xy, 0f))), 1f, SpellEffectSystem.ScaleMode.Scale, chunkIndex);
						PlayAudio(chunkIndex, "HarpoonHIt");
						data.CatchEntity = hitList[i];
						data.HarpoonHideRate = -999f;
						CheckThunderRelic(chunkIndex, spellEntity, ref data, in transform, in movement, in componentData, in config, in elementEffect);
						return;
					}
				}
			}
			PlayAudio(chunkIndex, "HarpoonHItGround");
			data.CatchEntity = Entity.Null;
		}

		private void HarpoonDoPullOutExplosion(float3 forceDir, ref Spell4024DaveHarpoonData data, Entity spellEntity, in LocalTransform transform, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
		{
			DoGroundExplosion(transform.Position, in config, out var hitList, chunkIndex);
			PlayAudio(chunkIndex, "HarpoonPullBack");
			if (EntityExists.Exists(data.CatchEntity) && UnitPropertyLookup.HasComponent(data.CatchEntity))
			{
				UnitPropertyLookup.GetRefRW(data.CatchEntity).ValueRW.TakeKnockback(new float3(forceDir.xy, 0f));
				CatchDamageApplyExplosion(spellEntity, data.CatchEntity, in hitList, in transform.Position, in componentData, in config, in movement, in transform, in elementEffect, chunkIndex);
				data.CatchEntity = Entity.Null;
			}
			else
			{
				HitDamageApplyExplosion(spellEntity, in hitList, in transform.Position, in componentData, in config, in movement, in transform, in elementEffect, chunkIndex);
			}
			hitList.Dispose();
		}

		private void ChainSpawnNormal(float3 initDirection, float3 initPosition, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			int num = 30;
			for (int i = 0; i < num; i++)
			{
				float degree = Random.random.NextFloat(-90f, 90f);
				float num2 = Random.random.NextFloat(0.1f, 1f);
				lineBuffer.Add(new HarpoonChainData
				{
					Position = initPosition,
					PrevPosition = initPosition - DTool.RotateDir(initDirection, degree) * num2
				});
			}
		}

		private void ChainGoReturning(ref Spell4024DaveHarpoonData data, ref SpellMovementComponentData movement, ref LocalTransform transform, ref SpellConfigComponentData config, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			data.ChainState = ChainState.Returning;
			data.VelocityDampen = 0.9f;
			data.EndPinned = false;
			data.ShowBubble = false;
			data.ReturnSpeed = 0f;
			config.DurationTimer = 0f;
		}

		private void ChainGoStraightening(ref Spell4024DaveHarpoonData data, ref LocalTransform transform, ref SpellConfigComponentData config, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			data.ChainState = ChainState.Straightening;
			data.StretchStiffness = 1f;
			data.VelocityDampen = 0f;
			data.EndPinned = true;
			float3 x = transform.Position - data.ShootPos;
			data.NodeDistance = math.length(x) / 30f;
			config.DurationTimer = 0f;
			SetStraigtenPoss(lineBuffer, in data);
		}

		private void ChainGoWaiting(ref Spell4024DaveHarpoonData data)
		{
			data.ChainState = ChainState.Waiting;
			data.VelocityDampen = 0.9f;
			data.EndPinned = false;
			data.StretchStiffness = 0.9f;
		}

		private void ChainGoCatching(ref Spell4024DaveHarpoonData data)
		{
			data.ChainState = ChainState.Catching;
			data.VelocityDampen = 0.9f;
			data.AllowStretch = true;
			data.EndPinned = true;
		}

		private void ChainLineReturn(float speedAcc, float3 startPos, ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config, DynamicBuffer<HarpoonChainData> lineBuffer, out bool isDestroy)
		{
			isDestroy = false;
			int num = (int)math.floor((data.ReturnSpeed * config.DurationTimer + 0.5f * speedAcc * config.DurationTimer * config.DurationTimer) / data.NodeDistance);
			data.ReturnSpeed += config.DurationTimer * speedAcc;
			if (num <= 0)
			{
				return;
			}
			config.DurationTimer = 0f;
			for (int i = 0; i < num; i++)
			{
				lineBuffer.RemoveAt(0);
				if (lineBuffer.Length <= 1)
				{
					isDestroy = true;
					return;
				}
			}
			HarpoonChainData value = lineBuffer[0];
			value.Position = startPos;
			lineBuffer[0] = value;
			data.StartPos = startPos;
		}

		private void ClearChainVelocity(DynamicBuffer<HarpoonChainData> chain)
		{
			for (int i = 0; i < chain.Length; i++)
			{
				HarpoonChainData value = chain[i];
				value.PrevPosition = value.Position;
				chain[i] = value;
			}
		}

		private void SetStraigtenPoss(DynamicBuffer<HarpoonChainData> chain, in Spell4024DaveHarpoonData data)
		{
			float3 @float = (data.EndPos - data.StartPos) / chain.Length;
			for (int i = 0; i < chain.Length; i++)
			{
				HarpoonChainData value = chain[i];
				value.Position = data.StartPos + i * @float;
				chain[i] = value;
			}
		}

		private void ChainSpawnFall(float3 initDirection, float3 initPosition, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				Rotate3DRandom(initDirection, 90f, out var result);
				float num2 = Random.random.NextFloat(0.1f, 1f);
				lineBuffer.Add(new HarpoonChainData
				{
					Position = initPosition,
					PrevPosition = initPosition - result * num2
				});
			}
		}

		private void ChainGoFallStraightening(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config)
		{
			data.ChainState = ChainState.FallStraightening;
			config.DurationTimer = 0f;
		}

		private void ChainGoFallStraightReturning(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config)
		{
			data.ChainState = ChainState.FallStraightReturning;
			config.DurationTimer = 0f;
		}

		private void ChainGoFallReturning(ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config)
		{
			data.ChainState = ChainState.FallReturning;
			data.VelocityDampen = 0.1f;
			data.StretchStiffness = 1f;
			data.EndPinned = false;
			data.ShowBubble = false;
			data.ShowDust = false;
			data.ReturnSpeed = 0f;
			config.DurationTimer = 0f;
		}

		private void ChainGoFallWaitReturn(float lineLength, ref Spell4024DaveHarpoonData data, ref SpellConfigComponentData config, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			data.ChainState = ChainState.FallWaitReturn;
			data.StretchStiffness = 0.9f;
			data.VelocityDampen = 1f;
			data.EndPinned = false;
			data.ShowBubble = false;
			config.DurationTimer = 0f;
			data.GravityStrength = new float3(0f, 0f, 40f);
		}

		private void Rotate3DRandom(float3 input, float randomAngle, out float3 result)
		{
			float3 @float = math.normalizesafe(input, new float3(1f, 0f, 0f));
			float3 float2 = math.normalize(math.cross(@float, new float3(0f, 0f, 1f)));
			if (math.lengthsq(float2) < 1E-08f)
			{
				float2 = math.normalize(math.cross(@float, new float3(0f, 1f, 0f)));
			}
			float3 float3 = math.normalize(math.cross(@float, float2));
			float x = math.radians(1f);
			float x2 = math.radians(randomAngle);
			float max = math.cos(x);
			float min = math.cos(x2);
			float num = Random.random.NextFloat(min, max);
			float num2 = math.sqrt(math.max(0f, 1f - num * num));
			float x3 = Random.random.NextFloat(0f, MathF.PI * 2f);
			float3 float4 = float2 * (math.cos(x3) * num2) + float3 * (math.sin(x3) * num2);
			result = math.normalize(@float * num + float4);
		}

		private void CheckEffectLink(string name, ref bool showEffect, ref bool requiredEffect, ref UnityObjectRef<GameObject> effectObjRef, Entity spellEntity, in SpellConfigComponentData config, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, [ChunkIndexInQuery] int chunkIndex)
		{
			if (!showEffect)
			{
				TryRecycleEffect(effectObjRef, name, spellEntity, chunkIndex);
				effectObjRef = default(UnityObjectRef<GameObject>);
			}
			else
			{
				if (effectObjRef.IsValid())
				{
					return;
				}
				if (requiredEffect)
				{
					if (TryGetLinkEffect(linkBuffer, out effectObjRef, name))
					{
						requiredEffect = false;
					}
				}
				else
				{
					TrailEffectRequire(name, in config, spellEntity, chunkIndex);
					requiredEffect = true;
				}
			}
		}

		private void CheckUniqueEffectLink(string name, ref bool showEffect, ref bool requiredEffect, ref UnityObjectRef<GameObject> effectObjRef, Entity spellEntity, in SpellColorType colorType, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, [ChunkIndexInQuery] int chunkIndex)
		{
			if (!showEffect)
			{
				TryRecycleEffect(effectObjRef, name, spellEntity, chunkIndex);
				effectObjRef = default(UnityObjectRef<GameObject>);
			}
			else
			{
				if (effectObjRef.IsValid())
				{
					return;
				}
				if (requiredEffect)
				{
					if (TryGetLinkEffect(linkBuffer, out effectObjRef, name))
					{
						requiredEffect = false;
					}
				}
				else
				{
					UniqueEffectRequire(name, colorType, spellEntity, chunkIndex);
					requiredEffect = true;
				}
			}
		}

		private void TryRecycleEffect(UnityObjectRef<GameObject> goRef, string name, Entity spellEntity, [ChunkIndexInQuery] int chunkIndex)
		{
			if (goRef.IsValid())
			{
				CMD.AppendToBuffer(chunkIndex, EffectRecycleBufferEntity, new SpellEffectSystem.Destroy
				{
					Entity = spellEntity,
					Name = name
				});
			}
		}

		private bool TryGetLinkEffect(DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out UnityObjectRef<GameObject> linkedObject, string name)
		{
			foreach (SpellGameObjectEffectLink item in linkBuffer)
			{
				SpellGameObjectEffectLink current = item;
				if (current.EffectName == name)
				{
					linkedObject = current.GameObject;
					return true;
				}
			}
			linkedObject = default(UnityObjectRef<GameObject>);
			return false;
		}

		private void TrailEffectRequire(string name, in SpellConfigComponentData config, Entity spellEntity, [ChunkIndexInQuery] int chunkIndex)
		{
			config.ColorType.ColorEnumToString(out var result);
			CMD.AppendToBuffer(chunkIndex, EffectRequireBufferEntity, new SpellEffectSystem.Require
			{
				Entity = spellEntity,
				SpellId = 4024,
				Color = result,
				Settings = 
				{
					Name = name,
					Layer = LayerCorrectType.Coordinate,
					IgnoreColor = false,
					ClearParticle = true,
					ClearTrail = true,
					ScaleMode = SpellEffectSystem.ScaleMode.Ignore
				}
			});
		}

		private void UniqueEffectRequire(string name, SpellColorType colorType, Entity spellEntity, [ChunkIndexInQuery] int chunkIndex)
		{
			colorType.ColorEnumToString(out var result);
			CMD.AppendToBuffer(chunkIndex, EffectRequireBufferEntity, new SpellEffectSystem.Require
			{
				Entity = spellEntity,
				SpellId = 4024,
				Color = result,
				Settings = 
				{
					Name = name,
					Layer = LayerCorrectType.Coordinate,
					IgnoreColor = false,
					ClearParticle = true,
					ClearTrail = true,
					ScaleMode = SpellEffectSystem.ScaleMode.Ignore
				}
			});
		}

		private void PlayUnfollowEffect(string name, int id, float3 position, FixedString32Bytes colorName, float rotation, float scale, SpellEffectSystem.ScaleMode scaleMode, [ChunkIndexInQuery] int chunkIndex)
		{
			CMD.AppendToBuffer(chunkIndex, UnfollowEffectRequireBufferEntity, new SpellEffectSystem.UnfollowingRequire
			{
				SpellId = id,
				StartPosition = position,
				Color = colorName,
				Scale = scale,
				StartRotation = quaternion.RotateZ(rotation * (MathF.PI / 180f)),
				Settings = 
				{
					Name = name,
					Layer = LayerCorrectType.Coordinate,
					IgnoreColor = false,
					ClearParticle = true,
					ClearTrail = false,
					ScaleMode = scaleMode,
					DestroyDelay = 1f
				}
			});
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

		private void SpellStartOffsetGet(in Spell4024DaveHarpoonMultiShootData multiShootData, in float3 shootPos, out float3 offSetPos)
		{
			float3 shiftedDir = DTool.GetShiftedDir(in PlayerDir, 90f);
			float3 @float = multiShootData.Offset * shiftedDir;
			offSetPos = shootPos + @float;
		}

		private void GetValidChasePosition(ref SpellMovementComponentData movement, in SpellComponentData data, in SpellConfigComponentData config, LocalTransform transform, out bool hasTarget, out float3 targetPosition)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.ChaseOwner:
				hasTarget = true;
				targetPosition = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
				break;
			case SpellSpecialMovementType.ChaseEnemy:
				GetValidChaseEnemyPosition(ref movement, in config, in transform, out hasTarget, out targetPosition);
				break;
			default:
				hasTarget = false;
				targetPosition = transform.Position;
				break;
			}
		}

		private void GetValidChaseEnemyPosition(ref SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, out bool hasTarget, out float3 targetPosition)
		{
			hasTarget = false;
			targetPosition = float3.zero;
			Entity target;
			float3 targetPosition2;
			UnitProperty_Dots targetPpt;
			if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData, out var entityExists) && entityExists)
			{
				targetPosition = componentData.Position;
				hasTarget = true;
			}
			else if (CurrentRoomEntities.FindMinAngleTarget(transform.Position, movement.Direction, config.ShooterType, out target, out targetPosition2, out targetPpt))
			{
				hasTarget = true;
				targetPosition = targetPosition2;
				movement.ChaseTarget = target;
			}
		}

		private void DoGroundExplosion(float3 position, in SpellConfigComponentData config, out NativeList<Entity> hitList, [ChunkIndexInQuery] int chunkIndex)
		{
			config.ColorType.ColorEnumToString(out var result);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"3119_Fall_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(position.xy, 1.08f);
			globalParticleEmitParams.Size = config.Radius.Calculate();
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
			hitList = new NativeList<Entity>(Allocator.Temp);
			float radius = config.Radius.Calculate();
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList);
		}

		[BurstCompile]
		private void HitDamageApplyExplosion(Entity spellEntity, in NativeList<Entity> hitList, in float3 explosionPos, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
		{
			for (int i = 0; i < hitList.Length; i++)
			{
				TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
				if (EntityExists.Exists(hitList[i]) && TransformLookup.TryGetComponent(hitList[i], out var componentData2))
				{
					info.spell.HitPosition = componentData2.Position;
					info.SetKnockbackForceIgnoreZBySpell(math.normalizesafe(info.spell.HitPosition - explosionPos));
					ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
					Entity target = hitList[i];
					cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
				}
			}
		}

		private void CatchDamageApplyExplosion(Entity spellEntity, Entity catchEntity, in NativeList<Entity> hitList, in float3 explosionPos, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
		{
			for (int i = 0; i < hitList.Length; i++)
			{
				TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
				if (EntityExists.Exists(hitList[i]) && TransformLookup.TryGetComponent(hitList[i], out var componentData2))
				{
					info.spell.HitPosition = componentData2.Position;
					if (catchEntity != hitList[i])
					{
						info.SetKnockbackForceIgnoreZBySpell(math.normalizesafe(info.spell.HitPosition - explosionPos));
					}
					ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
					Entity target = hitList[i];
					cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
				}
			}
		}

		private bool CheckHitCanCatch(Entity hitEntity)
		{
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

		private void SetHideSwordRateWithScale(float rate, Entity effEntity, in LocalTransform spellTrans, float scale)
		{
			if (EntityExists.Exists(effEntity) && HarpoonUnderGroundMatDataLookUp.HasComponent(effEntity))
			{
				RefRW<Spell4024HarpoonHideUnderGroundMat> refRW = HarpoonUnderGroundMatDataLookUp.GetRefRW(effEntity);
				float3 layerPosition = DTool.GetLayerPosition(in spellTrans.Position, LayerCorrectType.Coordinate);
				refRW.ValueRW.Value = spellTrans.Position.y + layerPosition.y + (rate - 0.5f) * scale;
			}
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

		private void CheckThunderStone(int chunkIndex, Entity entity, ref Spell4024DaveHarpoonData data, in LocalTransform transform, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellConfigComponentData config, in SpellElementEffectComponentData elementEffect)
		{
			if (config.ColorType == SpellColorType.Thunder)
			{
				CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, transform.Position, UnitPropertyLookup, PhysicsWorld, in config, in movement, in transform, in elementEffect, in componentData, entity);
			}
		}

		private void PlayAudio([ChunkIndexInQuery] int chunkIndex, string name)
		{
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity sEPlayerSingleton = SEPlayerSingleton;
			FixedString32Bytes seName = name;
			cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1030, in seName)));
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
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonMultiShootData_RO_ComponentTypeHandle);
			BufferAccessor<HarpoonChainData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__HarpoonChainData_RW_BufferTypeHandle);
			BufferAccessor<SpellGameObjectEffectLink> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferTypeHandle);
			BufferAccessor<HighspeedHarpoonSphereCastPos> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__HighspeedHarpoonSphereCastPos_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
					ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
					ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
					ref Spell4024DaveHarpoonData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr7, i);
					ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, i);
					ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, i);
					ref Spell4024DaveHarpoonMultiShootData multiShootData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonMultiShootData>(nativeArrayPtr10, i);
					DynamicBuffer<HarpoonChainData> lineBuffer = bufferAccessor[i];
					DynamicBuffer<SpellGameObjectEffectLink> linkBuffer = bufferAccessor2[i];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> rotatePosBuffer = bufferAccessor3[i];
					Execute(entity, ref transform, ref config, ref movement, ref componentData, ref elementEffect, ref data, ref collider, ref velocity, in multiShootData, lineBuffer, linkBuffer, rotatePosBuffer, chunkIndexInQuery);
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
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref Spell4024DaveHarpoonData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr7, nextRangeBegin);
						ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, nextRangeBegin);
						ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, nextRangeBegin);
						ref Spell4024DaveHarpoonMultiShootData multiShootData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonMultiShootData>(nativeArrayPtr10, nextRangeBegin);
						DynamicBuffer<HarpoonChainData> lineBuffer2 = bufferAccessor[nextRangeBegin];
						DynamicBuffer<SpellGameObjectEffectLink> linkBuffer2 = bufferAccessor2[nextRangeBegin];
						DynamicBuffer<HighspeedHarpoonSphereCastPos> rotatePosBuffer2 = bufferAccessor3[nextRangeBegin];
						Execute(entity2, ref transform2, ref config2, ref movement2, ref componentData2, ref elementEffect2, ref data2, ref collider2, ref velocity2, in multiShootData2, lineBuffer2, linkBuffer2, rotatePosBuffer2, chunkIndexInQuery);
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
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
					ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
					ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
					ref Spell4024DaveHarpoonData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr7, j);
					ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, j);
					ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, j);
					ref Spell4024DaveHarpoonMultiShootData multiShootData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonMultiShootData>(nativeArrayPtr10, j);
					DynamicBuffer<HarpoonChainData> lineBuffer3 = bufferAccessor[j];
					DynamicBuffer<SpellGameObjectEffectLink> linkBuffer3 = bufferAccessor2[j];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> rotatePosBuffer3 = bufferAccessor3[j];
					Execute(entity3, ref transform3, ref config3, ref movement3, ref componentData3, ref elementEffect3, ref data3, ref collider3, ref velocity3, in multiShootData3, lineBuffer3, linkBuffer3, rotatePosBuffer3, chunkIndexInQuery);
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
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
					ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
					ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
					ref Spell4024DaveHarpoonData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr7, k);
					ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, k);
					ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, k);
					ref Spell4024DaveHarpoonMultiShootData multiShootData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonMultiShootData>(nativeArrayPtr10, k);
					DynamicBuffer<HarpoonChainData> lineBuffer4 = bufferAccessor[k];
					DynamicBuffer<SpellGameObjectEffectLink> linkBuffer4 = bufferAccessor2[k];
					DynamicBuffer<HighspeedHarpoonSphereCastPos> rotatePosBuffer4 = bufferAccessor3[k];
					Execute(entity4, ref transform4, ref config4, ref movement4, ref componentData4, ref elementEffect4, ref data4, ref collider4, ref velocity4, in multiShootData4, lineBuffer4, linkBuffer4, rotatePosBuffer4, chunkIndexInQuery);
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

	[CompilerGenerated]
	[BurstCompile]
	public struct Spell4024DaveHarpoonChainInertiaJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell4024DaveHarpoonData> __Spell4024DaveHarpoonData_RW_ComponentTypeHandle;

				public BufferTypeHandle<HarpoonChainData> __HarpoonChainData_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
					__HarpoonChainData_RW_BufferTypeHandle = state.GetBufferTypeHandle<HarpoonChainData>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle.Update(ref state);
					__HarpoonChainData_RW_BufferTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4024DaveHarpoonData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HarpoonChainData>();
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
			public void Run(ref Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4024DaveHarpoonChainInertiaJob job, EntityManager entityManager)
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

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(Entity entity, ref LocalTransform transform, ref Spell4024DaveHarpoonData data, DynamicBuffer<HarpoonChainData> lineBuffer, [ChunkIndexInQuery] int chunkIndex)
		{
			int length = lineBuffer.Length;
			if (!data.IsInitialized || length <= 0)
			{
				return;
			}
			math.max(1E-05f, DeltaTime);
			float3 gravityStrength = data.GravityStrength;
			float x = math.saturate(data.VelocityDampen);
			float3 startPos = data.StartPos;
			float3 endPos = data.EndPos;
			startPos.z = math.min(startPos.z, data.GroundZ);
			endPos.z = math.min(endPos.z, data.GroundZ);
			for (int i = 0; i < length; i++)
			{
				HarpoonChainData value = lineBuffer[i];
				if (i == 0)
				{
					value.PrevPosition = startPos;
					value.Position = startPos;
					lineBuffer[0] = value;
					continue;
				}
				if (i == length - 1 && data.EndPinned)
				{
					value.PrevPosition = endPos;
					value.Position = endPos;
					lineBuffer[i] = value;
					continue;
				}
				float3 position = value.Position;
				float3 prevPosition = value.PrevPosition;
				float3 @float = (position - prevPosition) * math.saturate(x);
				value.PrevPosition = position;
				position += @float + gravityStrength * (DeltaTime * DeltaTime);
				if (position.z > data.GroundZ)
				{
					position.z = data.GroundZ;
				}
				value.Position = position;
				lineBuffer[i] = value;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonData_RW_ComponentTypeHandle);
			BufferAccessor<HarpoonChainData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__HarpoonChainData_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref Spell4024DaveHarpoonData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr3, i);
					DynamicBuffer<HarpoonChainData> lineBuffer = bufferAccessor[i];
					Execute(entity, ref transform, ref data, lineBuffer, chunkIndexInQuery);
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
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref Spell4024DaveHarpoonData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr3, nextRangeBegin);
						DynamicBuffer<HarpoonChainData> lineBuffer2 = bufferAccessor[nextRangeBegin];
						Execute(entity2, ref transform2, ref data2, lineBuffer2, chunkIndexInQuery);
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
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref Spell4024DaveHarpoonData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr3, j);
					DynamicBuffer<HarpoonChainData> lineBuffer3 = bufferAccessor[j];
					Execute(entity3, ref transform3, ref data3, lineBuffer3, chunkIndexInQuery);
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
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref Spell4024DaveHarpoonData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr3, k);
					DynamicBuffer<HarpoonChainData> lineBuffer4 = bufferAccessor[k];
					Execute(entity4, ref transform4, ref data4, lineBuffer4, chunkIndexInQuery);
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

	[BurstCompile]
	[CompilerGenerated]
	public struct Spell4024DaveHarpoonChainVerletJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell4024DaveHarpoonData> __Spell4024DaveHarpoonData_RW_ComponentTypeHandle;

				public BufferTypeHandle<HarpoonChainData> __HarpoonChainData_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
					__HarpoonChainData_RW_BufferTypeHandle = state.GetBufferTypeHandle<HarpoonChainData>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__Spell4024DaveHarpoonData_RW_ComponentTypeHandle.Update(ref state);
					__HarpoonChainData_RW_BufferTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4024DaveHarpoonData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HarpoonChainData>();
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
			public void Run(ref Spell4024DaveHarpoonChainVerletJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4024DaveHarpoonChainVerletJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4024DaveHarpoonChainVerletJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonChainVerletJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4024DaveHarpoonChainVerletJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4024DaveHarpoonChainVerletJob job, EntityManager entityManager)
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

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(ref LocalTransform transform, ref Spell4024DaveHarpoonData data, DynamicBuffer<HarpoonChainData> lineBuffer)
		{
			int length = lineBuffer.Length;
			if (!data.IsInitialized || length < 2)
			{
				return;
			}
			int num = length - 1;
			float num2 = math.max(1E-05f, data.NodeDistance);
			int num3 = math.max(1, data.Iterations);
			float3 startPos = data.StartPos;
			float3 endPos = data.EndPos;
			startPos.z = math.min(startPos.z, data.GroundZ);
			endPos.z = math.min(endPos.z, data.GroundZ);
			float3 @float = endPos - startPos;
			float num4 = math.length(@float);
			float num5 = (float)num * num2;
			if (data.AllowStretch && data.EndPinned && num4 > num5 + 1E-06f)
			{
				float3 float2 = @float / num4;
				float num6 = num4 / (float)num;
				float t = math.saturate(data.StretchStiffness);
				HarpoonChainData value = lineBuffer[0];
				value.Position = startPos;
				lineBuffer[0] = value;
				HarpoonChainData value2 = lineBuffer[num];
				value2.Position = endPos;
				lineBuffer[num] = value2;
				for (int i = 1; i < num; i++)
				{
					HarpoonChainData value3 = lineBuffer[i];
					float3 end = startPos + float2 * (num6 * (float)i);
					value3.Position = math.lerp(value3.Position, end, t);
					if (value3.Position.z > data.GroundZ)
					{
						float3 position = value3.Position;
						position.z = data.GroundZ;
						value3.Position = position;
					}
					lineBuffer[i] = value3;
				}
				return;
			}
			for (int j = 0; j < num3; j++)
			{
				HarpoonChainData value4 = lineBuffer[0];
				value4.Position = startPos;
				lineBuffer[0] = value4;
				if (data.EndPinned)
				{
					HarpoonChainData value5 = lineBuffer[num];
					value5.Position = endPos;
					lineBuffer[num] = value5;
				}
				HarpoonChainData value6 = lineBuffer[1];
				float3 float3 = value6.Position - startPos;
				float num7 = math.length(float3);
				if (num7 > 1E-06f)
				{
					float3 float4 = float3 / num7;
					value6.Position = startPos + float4 * num2;
					if (value6.Position.z > data.GroundZ)
					{
						float3 position2 = value6.Position;
						position2.z = data.GroundZ;
						value6.Position = position2;
					}
					lineBuffer[1] = value6;
				}
				else
				{
					float3 position3 = value6.Position;
					position3.x += 0.0001f;
					value6.Position = position3;
					lineBuffer[1] = value6;
				}
				for (int k = 1; k < num; k++)
				{
					HarpoonChainData value7 = lineBuffer[k];
					HarpoonChainData value8 = lineBuffer[k + 1];
					float3 float5 = value8.Position - value7.Position;
					float num8 = math.length(float5);
					if (!(num8 < 1E-06f))
					{
						float3 float6 = float5 / num8;
						float num9 = num8 - num2;
						if (data.EndPinned && k + 1 == num)
						{
							value7.Position += num9 * float6;
						}
						else
						{
							float3 float7 = 0.5f * num9 * float6;
							value7.Position += float7;
							value8.Position -= float7;
						}
						if (value7.Position.z > data.GroundZ)
						{
							float3 position4 = value7.Position;
							position4.z = data.GroundZ;
							value7.Position = position4;
						}
						if (value8.Position.z > data.GroundZ)
						{
							float3 position5 = value8.Position;
							position5.z = data.GroundZ;
							value8.Position = position5;
						}
						lineBuffer[k] = value7;
						lineBuffer[k + 1] = value8;
					}
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonData_RW_ComponentTypeHandle);
			BufferAccessor<HarpoonChainData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__HarpoonChainData_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
					ref Spell4024DaveHarpoonData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, i);
					DynamicBuffer<HarpoonChainData> lineBuffer = bufferAccessor[i];
					Execute(ref transform, ref data, lineBuffer);
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
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, nextRangeBegin);
						ref Spell4024DaveHarpoonData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, nextRangeBegin);
						DynamicBuffer<HarpoonChainData> lineBuffer2 = bufferAccessor[nextRangeBegin];
						Execute(ref transform2, ref data2, lineBuffer2);
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
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, j);
					ref Spell4024DaveHarpoonData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, j);
					DynamicBuffer<HarpoonChainData> lineBuffer3 = bufferAccessor[j];
					Execute(ref transform3, ref data3, lineBuffer3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, k);
					ref Spell4024DaveHarpoonData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonData>(nativeArrayPtr2, k);
					DynamicBuffer<HarpoonChainData> lineBuffer4 = bufferAccessor[k];
					Execute(ref transform4, ref data4, lineBuffer4);
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
	private readonly struct IFE_707381362_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4024DaveHarpoonData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4024DaveHarpoonData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4024DaveHarpoonData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_707381362_0.TypeHandle __IFE_707381362_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public Spell4024HarpoonRotateTargetGetJob.InternalCompilerQueryAndHandleData __Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<Spell4024HarpoonHideUnderGroundMat> __Spell4024HarpoonHideUnderGroundMat_RW_ComponentLookup;

		public ComponentLookup<Spell4024HarpoonOverlayColor> __Spell4024HarpoonOverlayColor_RW_ComponentLookup;

		public Spell4024DaveHarpoonMoveJob.InternalCompilerQueryAndHandleData __Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle;

		public Spell4024DaveHarpoonChainInertiaJob.InternalCompilerQueryAndHandleData __Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle;

		public Spell4024DaveHarpoonChainVerletJob.InternalCompilerQueryAndHandleData __Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_707381362_0_TypeHandle = new IFE_707381362_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell4024HarpoonHideUnderGroundMat_RW_ComponentLookup = state.GetComponentLookup<Spell4024HarpoonHideUnderGroundMat>();
			__Spell4024HarpoonOverlayColor_RW_ComponentLookup = state.GetComponentLookup<Spell4024HarpoonOverlayColor>();
			__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private LightningHarpoonRelicData harpoonLightningRelicData;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_707381362_0;

	private EntityQuery __query_707381362_1;

	private EntityQuery __query_707381362_2;

	private EntityQuery __query_707381362_3;

	private EntityQuery __query_707381362_4;

	private EntityQuery __query_707381362_5;

	private EntityQuery __query_707381362_6;

	private EntityQuery __query_707381362_7;

	private EntityQuery __query_707381362_8;

	private EntityQuery __query_707381362_9;

	private EntityQuery __query_707381362_10;

	private EntityQuery __query_707381362_11;

	private EntityQuery __query_707381362_12;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellEffectSystem.Destroy>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell4024DaveHarpoonData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
	}

	private LightningHarpoonRelicData GetLightningHarpoonRelicData(RelicConfig relicConfig)
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_LightningHarpoonHead != null)
		{
			LightningHarpoonRelicData result = default(LightningHarpoonRelicData);
			result.DamageRate = relicConfig.float1.result;
			result.Radius = relicConfig.float2.result;
			return result;
		}
		return default(LightningHarpoonRelicData);
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item3 in IFE_707381362_0.Query(__query_707381362_0, __TypeHandle.__IFE_707381362_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var item, out var item2, out var _);
			InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW2 = item2;
			uncheckedRefRW.ValueRW.ShootPos = (uncheckedRefRW2.ValueRW.Wand.Value.passiveAutoWand ? uncheckedRefRW2.ValueRW.Wand.Value.passiveAutoWandShooterData.shootPosition : PlayerMgr.Inst.ShootPoint);
		}
		EntityCommandBuffer.ParallelWriter cMD = __query_707381362_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		GlobalRandom singleton = __query_707381362_2.GetSingleton<GlobalRandom>();
		float3 mousePosition = __query_707381362_3.GetSingleton<PlayerController_Dots>().mousePosition;
		EntityStorageInfoLookup entityStorageInfoLookup = state.GetEntityStorageInfoLookup();
		harpoonLightningRelicData = GetLightningHarpoonRelicData(PlayerMgr.Inst.ItemCtrller.relic_LightningHarpoonHead);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4024HarpoonRotateTargetGetJob
		{
			CMD = cMD,
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			PhysicsWorld = __query_707381362_4.GetSingleton<PhysicsWorldSingleton>(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			Random = singleton,
			MousePosition = mousePosition,
			EffectCtrlLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			EntityExists = entityStorageInfoLookup,
			SEPlayerSingleton = __query_707381362_5.GetSingletonEntity(),
			RelicData = harpoonLightningRelicData,
			GlobalParticleSystemBufferEntity = __query_707381362_6.GetSingletonEntity()
		}, __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Spell4024DaveHarpoonMoveJob
		{
			CMD = cMD,
			Random = singleton,
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			EffectRequireBufferEntity = __query_707381362_7.GetSingletonEntity(),
			EffectRecycleBufferEntity = __query_707381362_8.GetSingletonEntity(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_707381362_9.GetSingleton<CurrentRoomEntitiesSingleton>(),
			MousePosition = mousePosition,
			PhysicsWorld = __query_707381362_4.GetSingleton<PhysicsWorldSingleton>(),
			SpellSingleton = __query_707381362_10.GetSingleton<SpellSingleton>(),
			HarpoonUnderGroundMatDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4024HarpoonHideUnderGroundMat_RW_ComponentLookup, ref state),
			HarpoonGlowMatDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4024HarpoonOverlayColor_RW_ComponentLookup, ref state),
			EntityExists = entityStorageInfoLookup,
			UnfollowEffectRequireBufferEntity = __query_707381362_11.GetSingletonEntity(),
			SEPlayerSingleton = __query_707381362_5.GetSingletonEntity(),
			Spell3101Buffer = __query_707381362_12.GetSingletonEntity(),
			RelicData = harpoonLightningRelicData,
			GlobalParticleSystemBufferEntity = __query_707381362_6.GetSingletonEntity(),
			PlayerDir = PlayerMgr.Inst.PlayerDir,
			PlayerShootPosition = PlayerMgr.Inst.ShootPoint,
			PlayerEntity = __query_707381362_3.GetSingletonEntity()
		}, __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_2(new Spell4024DaveHarpoonChainInertiaJob
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime
		}, __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_3(default(Spell4024DaveHarpoonChainVerletJob), __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4024HarpoonRotateTargetGetJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024HarpoonRotateTargetGetJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Spell4024DaveHarpoonMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonMoveJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_2(Spell4024DaveHarpoonChainInertiaJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainInertiaJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_3(Spell4024DaveHarpoonChainVerletJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4024DaveHarpoonSystem_Spell4024DaveHarpoonChainVerletJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4024DaveHarpoonData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_707381362_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_11 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_707381362_12 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
