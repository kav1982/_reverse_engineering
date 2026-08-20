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

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct TeammateRegisterSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	public struct TeammateRegisterJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__SpellSpeedRatioValueData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSpeedRatioValueData>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				}

				public void Update(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellSpeedRatioValueData_RW_ComponentTypeHandle.Update(ref state);
					__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellSpeedRatioValueData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
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
			public void Run(ref TeammateRegisterJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref TeammateRegisterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref TeammateRegisterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref TeammateRegisterJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref TeammateRegisterJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref TeammateRegisterJob job, EntityManager entityManager)
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

		[NativeDisableUnsafePtrRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocaltransformLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableUnsafePtrRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitpropertyLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableUnsafePtrRestriction]
		public ComponentLookup<Spell4005WandSpiritData> WandSpiritLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableUnsafePtrRestriction]
		public ComponentLookup<SpellNeedResize> ResizeLookUp;

		public Entity PlayerEntity;

		public Entity SummonEffectSpawnerEntity;

		public Entity SummonSoumateEffectSpawnerEntity;

		public Entity LifeLineSpawnBufferEntity;

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity FuseRequestEntity;

		public Entity SoulMateSpawnBufferEntity;

		[NativeDisableUnsafePtrRestriction]
		public GlobalRandom Random;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(ref TeammateData teammateData, [ChunkIndexInQuery] int chunkIndex, ref SpellConfigComponentData config, ref SpellComponentData data, Entity entity, ref SpellMovementComponentData movementComponentData, ref SpellElementEffectComponentData element, SpellSpeedRatioValueData speedRatioData, ref UnitBase_Dots unitBase, ref PhysicsVelocity velocity)
		{
			if (!LocaltransformLookUp.HasComponent(data.OwnerEntity))
			{
				return;
			}
			RefRW<LocalTransform> refRW = LocaltransformLookUp.GetRefRW(entity);
			if (teammateData.IsInitialized)
			{
				return;
			}
			teammateData.IsInitialized = true;
			RefRW<UnitProperty_Dots> refRW2 = UnitpropertyLookUp.GetRefRW(entity);
			float3 position = Random.random.NextFloat3(0.05f, 0.1f);
			float3 @float = DTool.IgnoreZPosition(in position);
			refRW.ValueRW.Position += @float;
			refRW2.ValueRW.unitCfg.maxHP = refRW2.ValueRW.unitCfg.maxHP * teammateData.TeammateHpRatio.AddRatio * teammateData.TeammateHpRatio.MulRatio;
			refRW2.ValueRW.unitCfg.currentHP = refRW2.ValueRW.unitCfg.maxHP;
			if (teammateData.Born1Hp)
			{
				refRW2.ValueRW.unitCfg.currentHP = 1f;
			}
			refRW.ValueRW.Scale = math.pow(teammateData.TeammateHpRatio.AddRatio * teammateData.TeammateHpRatio.MulRatio / (float)(teammateData.TeammateCurrentFuseLevel + 1), 0.25f);
			if (ResizeLookUp.HasComponent(entity))
			{
				float extraSizeRatio = ResizeLookUp[entity].ExtraSizeRatio;
				refRW.ValueRW.Scale += extraSizeRatio;
			}
			refRW.ValueRW.Scale = math.min(5f, refRW.ValueRW.Scale);
			if (config.AbilityType == SpellAbilityType.Summon4 && movementComponentData.Type != SpellSpecialMovementType.Rotation)
			{
				float3 float2 = DTool.RotateDir(movementComponentData.Direction, -90f) * refRW.ValueRW.Scale * (data.InShootIndex + 1) / 2f * 0.5f * ((data.InShootIndex % 2 == 0) ? 1 : (-1));
				refRW.ValueRW.Position += float2;
			}
			if (config.AbilityType != SpellAbilityType.Summon4)
			{
				CMD.AddComponent<IgnoreDynamicOptimizeTag>(chunkIndex, entity);
			}
			CMD.AppendToBuffer(chunkIndex, SummonEffectSpawnerEntity, new SummonAuraBuffer
			{
				spawnPosition = refRW.ValueRW.Position,
				spawnScale = refRW.ValueRW.Scale
			});
			if (movementComponentData.Type == SpellSpecialMovementType.Rotation || config.AbilityType == SpellAbilityType.Summon4)
			{
				CMD.SetComponentEnabled<TeammateDisableSpellMovementTag>(chunkIndex, entity, value: false);
			}
			movementComponentData.Speed = (refRW2.ValueRW.unitCfg.moveSpeed + speedRatioData.Speed.AddBase) * (speedRatioData.Speed.AddRatio + 1f) * speedRatioData.Speed.MulRatio * teammateData.TeammateSpeedRatio;
			if (teammateData.LifeLineDamage > 0f)
			{
				SpellComponentData data2 = data;
				SpellElementEffectComponentData element2 = element;
				SpellConfigComponentData config2 = config;
				data2.SpellEffectEntity = Entity.Null;
				data2.TrailEffectEntity = Entity.Null;
				config2.AbilityType = SpellAbilityType.LifeLine;
				Spell3110LifeLineSpawnBuffer spell3110LifeLineSpawnBuffer = default(Spell3110LifeLineSpawnBuffer);
				spell3110LifeLineSpawnBuffer.data = data2;
				spell3110LifeLineSpawnBuffer.config = config2;
				spell3110LifeLineSpawnBuffer.element = element2;
				spell3110LifeLineSpawnBuffer.linkTarget1 = data.OwnerEntity;
				spell3110LifeLineSpawnBuffer.linkTarget2 = entity;
				spell3110LifeLineSpawnBuffer.lifeLineColorType = config2.ColorType;
				Spell3110LifeLineSpawnBuffer element3 = spell3110LifeLineSpawnBuffer;
				element3.config.Damage.Base = teammateData.LifeLineDamage;
				CMD.AppendToBuffer(chunkIndex, LifeLineSpawnBufferEntity, element3);
			}
			if (teammateData.SpellSummonGainOwnerHpRatio > 0f)
			{
				bool flag = true;
				if (UnitpropertyLookUp.TryGetComponent(data.OwnerEntity, out var componentData))
				{
					if (WandSpiritLookUp.HasComponent(data.OwnerEntity))
					{
						flag = UnitpropertyLookUp.TryGetComponent(PlayerEntity, out componentData);
					}
					if (flag)
					{
						refRW2.ValueRW.unitCfg.maxHP += componentData.unitCfg.maxHP * teammateData.SpellSummonGainOwnerHpRatio * teammateData.TeammateHpRatio.AddRatio * teammateData.TeammateHpRatio.MulRatio;
						refRW2.ValueRW.unitCfg.currentHP = refRW2.ValueRW.unitCfg.maxHP;
					}
				}
				CMD.AppendToBuffer(chunkIndex, SummonSoumateEffectSpawnerEntity, new SummonSoulmateAuraBuffer
				{
					spawnPosition = refRW.ValueRW.Position,
					spawnScale = refRW.ValueRW.Scale
				});
				CMD.AddComponent(chunkIndex, entity, default(Spell3127SoulMateComponent));
			}
			if (teammateData.TeammateMaxFuseLevel > 0)
			{
				CMD.AppendToBuffer(chunkIndex, FuseRequestEntity, new TeammateFuseRequestBuffer
				{
					TeammateData = teammateData,
					TeammateEntity = entity,
					OwnerUnit = data.OwnerEntity,
					ChunkIndex = chunkIndex
				});
			}
			else
			{
				CMD.TeammateRegister(chunkIndex, in entity, in data.OwnerEntity, in teammateData);
			}
			if (element.VoidExplosionHpDamageRatio > 0f)
			{
				refRW2.ValueRW.SetVoid(new Spell3129VoidExplosion.VoidExplosionData_Dots
				{
					ConstVoidEffect = true,
					ExplosionRange = element.VoidExplosionRange * (config.Radius.AddRatio + 1f) * config.Radius.MulRatio,
					HpToDmgRatio = element.VoidExplosionHpDamageRatio,
					InstantKillRatio = element.VoidInstantKillThreshold
				});
			}
			velocity.Linear = float3.zero;
			unitBase.currentMotion = float3.zero;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellSpeedRatioValueData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					ref SpellMovementComponentData movementComponentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
					ref SpellElementEffectComponentData element = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
					ref SpellSpeedRatioValueData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr7, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr8, i);
					Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, i), teammateData: ref teammateData, chunkIndex: chunkIndexInQuery, config: ref config, data: ref data, entity: entity, movementComponentData: ref movementComponentData, element: ref element, speedRatioData: reference, unitBase: ref unitBase);
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
						ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						ref SpellMovementComponentData movementComponentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellElementEffectComponentData element2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref SpellSpeedRatioValueData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr7, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr8, nextRangeBegin);
						Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, nextRangeBegin), teammateData: ref teammateData2, chunkIndex: chunkIndexInQuery, config: ref config2, data: ref data2, entity: entity2, movementComponentData: ref movementComponentData2, element: ref element2, speedRatioData: reference2, unitBase: ref unitBase2);
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
					ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					ref SpellMovementComponentData movementComponentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
					ref SpellElementEffectComponentData element3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
					ref SpellSpeedRatioValueData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr7, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr8, j);
					Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, j), teammateData: ref teammateData3, chunkIndex: chunkIndexInQuery, config: ref config3, data: ref data3, entity: entity3, movementComponentData: ref movementComponentData3, element: ref element3, speedRatioData: reference3, unitBase: ref unitBase3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					ref SpellMovementComponentData movementComponentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
					ref SpellElementEffectComponentData element4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
					ref SpellSpeedRatioValueData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr7, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr8, k);
					Execute(velocity: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, k), teammateData: ref teammateData4, chunkIndex: chunkIndexInQuery, config: ref config4, data: ref data4, entity: entity4, movementComponentData: ref movementComponentData4, element: ref element4, speedRatioData: reference4, unitBase: ref unitBase4);
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

	private struct TypeHandle
	{
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RW_ComponentLookup;

		public TeammateRegisterJob.InternalCompilerQueryAndHandleData __TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellNeedResize_RW_ComponentLookup = state.GetComponentLookup<SpellNeedResize>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Spell4005WandSpiritData_RW_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>();
			__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000091E6_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000091E6_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000091E6_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_000091E7_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_000091E7_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_000091E7_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_78269882_0;

	private EntityQuery __query_78269882_1;

	private EntityQuery __query_78269882_2;

	private EntityQuery __query_78269882_3;

	private EntityQuery __query_78269882_4;

	private EntityQuery __query_78269882_5;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<TeammateFuseRequestBuffer>();
		state.RequireForUpdate<Spell3110LifeLineSpawnBuffer>();
		state.RequireForUpdate<SummonAuraBuffer>();
		state.RequireForUpdate<SummonSoulmateAuraBuffer>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		__ScheduleViaJobChunkExtension_0(new TeammateRegisterJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			SummonEffectSpawnerEntity = __query_78269882_0.GetSingletonEntity(),
			SummonSoumateEffectSpawnerEntity = __query_78269882_1.GetSingletonEntity(),
			LifeLineSpawnBufferEntity = __query_78269882_2.GetSingletonEntity(),
			FuseRequestEntity = __query_78269882_3.GetSingletonEntity(),
			ResizeLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellNeedResize_RW_ComponentLookup, ref state),
			LocaltransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitpropertyLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			Random = __query_78269882_4.GetSingleton<GlobalRandom>(),
			WandSpiritLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref state),
			PlayerEntity = __query_78269882_5.GetSingletonEntity()
		}, __TypeHandle.__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(TeammateRegisterJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__TeammateRegisterSystem_TeammateRegisterJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SummonAuraBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SummonSoulmateAuraBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3110LifeLineSpawnBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateFuseRequestBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_78269882_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_000091E6_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_000091E7_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateRegisterSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateRegisterSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateRegisterSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
