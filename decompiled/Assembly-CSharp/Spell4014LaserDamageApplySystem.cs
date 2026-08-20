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

[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[UpdateAfter(typeof(Spell4014ManaCostSystem))]
[CompilerGenerated]
internal struct Spell4014LaserDamageApplySystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Spell4014LaserDamageJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell4014LaserCrystalData> __Spell4014LaserCrystalData_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<MultiShootData> __MultiShootData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<ManaCostRatio> __ManaCostRatio_RO_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				public BufferTypeHandle<CrystalLaserPoint> __CrystalLaserPoint_RW_BufferTypeHandle;

				public BufferTypeHandle<HittedEntity> __HittedEntity_RW_BufferTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__Spell4014LaserCrystalData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4014LaserCrystalData>();
					__MultiShootData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MultiShootData>(isReadOnly: true);
					__ManaCostRatio_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ManaCostRatio>(isReadOnly: true);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__CrystalLaserPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<CrystalLaserPoint>();
					__HittedEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<HittedEntity>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell4014LaserCrystalData_RW_ComponentTypeHandle.Update(ref state);
					__MultiShootData_RO_ComponentTypeHandle.Update(ref state);
					__ManaCostRatio_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__CrystalLaserPoint_RW_BufferTypeHandle.Update(ref state);
					__HittedEntity_RW_BufferTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<MultiShootData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<ManaCostRatio>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CrystalLaserPoint>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HittedEntity>();
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
			public void Run(ref Spell4014LaserDamageJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4014LaserDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4014LaserDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4014LaserDamageJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4014LaserDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4014LaserDamageJob job, EntityManager entityManager)
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

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		public float DeltaTime;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		public Entity GlobalParticleSystemBufferEntity;

		public Entity SEPlayerSingleton;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		public Entity Spell3101Buffer;

		public GlobalRandom Random;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(Entity entity, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData componentData, ref Spell4014LaserCrystalData crystalData, in MultiShootData multiShootData, in ManaCostRatio manaCostRatio, ref LocalTransform transform, ref SpellElementEffectComponentData elementEffect, DynamicBuffer<CrystalLaserPoint> crystalPoints, DynamicBuffer<HittedEntity> laserHitteds, [ChunkIndexInQuery] int chunkIndex)
		{
			if (crystalData.IsHit && crystalData.IsHitFrame)
			{
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
				AttributeValue damage = config.Damage;
				damage.AddBase += crystalData.KeepHittingRate * (float)config.Int1;
				float damage2 = (info.damage = damage.Calculate() * config.DamageInterval);
				HitDamageApply(in laserHitteds, info, chunkIndex);
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Hit";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(4014, in seName)));
				config.ColorType.ColorEnumToString(out var result);
				if ((float)Random.random.NextInt(100) < crystalData.ThunderStoneRate)
				{
					CMD.GenerateThunderDamage(chunkIndex, Spell3101Buffer, damage2, crystalPoints[crystalPoints.Length - 1].Position, UnitPropertyLookup, PhysicsWorld, in config, in movement, in transform, in elementEffect, in componentData, entity);
				}
				if (movement.IsFallSpell)
				{
					GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
					globalParticleEmitParams.Name = $"3119_Fall_{result}";
					globalParticleEmitParams.Alpha = 1f;
					globalParticleEmitParams.Position = new float3(crystalData.TouchGroundPos.xy, 1.08f);
					globalParticleEmitParams.Size = config.Radius.Calculate();
					GlobalParticleEmitParams element = globalParticleEmitParams;
					CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
				}
			}
			if (crystalData.IsHit)
			{
				crystalData.KeepHittingRate += DeltaTime * config.Float2 / 100f;
			}
			else
			{
				crystalData.KeepHittingRate -= DeltaTime * config.Float2 / 100f;
				crystalData.KeepHittingRate = math.clamp(crystalData.KeepHittingRate, 1f, float.PositiveInfinity);
			}
			crystalData.LaserWidth = math.pow(crystalData.KeepHittingRate, 0.75f) * crystalData.LaserWidthBase;
			crystalData.LaserWidth = math.clamp(crystalData.LaserWidth, crystalData.LaserWidthBase, 1f);
			laserHitteds.Clear();
		}

		[BurstCompile]
		private void HitDamageApply(in DynamicBuffer<HittedEntity> hitList, TakeDamageInfo_Dots dmgInfo, [ChunkIndexInQuery] int chunkIndex)
		{
			foreach (HittedEntity hit in hitList)
			{
				HittedEntity current = hit;
				if (TransformLookup.HasComponent(current.Entity))
				{
					dmgInfo.spell.HitPosition = TransformLookup[current.Entity].Position;
					dmgInfo.spell.IgnoreHitEffect = false;
					dmgInfo.beHitShakeDir = current.Direction;
					CMD.TryAttackEntity(chunkIndex, in current.Entity, in dmgInfo, in UnitPropertyLookup, in SpellConfigLookup);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4014LaserCrystalData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__MultiShootData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__ManaCostRatio_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			BufferAccessor<CrystalLaserPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__CrystalLaserPoint_RW_BufferTypeHandle);
			BufferAccessor<HittedEntity> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__HittedEntity_RW_BufferTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
					ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i);
					ref Spell4014LaserCrystalData crystalData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, i);
					ref MultiShootData multiShootData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, i);
					ref ManaCostRatio manaCostRatio = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, i);
					ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, i);
					DynamicBuffer<CrystalLaserPoint> crystalPoints = bufferAccessor[i];
					DynamicBuffer<HittedEntity> laserHitteds = bufferAccessor2[i];
					Execute(entity, ref config, ref movement, ref componentData, ref crystalData, in multiShootData, in manaCostRatio, ref transform, ref elementEffect, crystalPoints, laserHitteds, chunkIndexInQuery);
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
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref Spell4014LaserCrystalData crystalData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, nextRangeBegin);
						ref MultiShootData multiShootData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, nextRangeBegin);
						ref ManaCostRatio manaCostRatio2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, nextRangeBegin);
						ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, nextRangeBegin);
						DynamicBuffer<CrystalLaserPoint> crystalPoints2 = bufferAccessor[nextRangeBegin];
						DynamicBuffer<HittedEntity> laserHitteds2 = bufferAccessor2[nextRangeBegin];
						Execute(entity2, ref config2, ref movement2, ref componentData2, ref crystalData2, in multiShootData2, in manaCostRatio2, ref transform2, ref elementEffect2, crystalPoints2, laserHitteds2, chunkIndexInQuery);
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
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
					ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j);
					ref Spell4014LaserCrystalData crystalData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, j);
					ref MultiShootData multiShootData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, j);
					ref ManaCostRatio manaCostRatio3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, j);
					ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, j);
					DynamicBuffer<CrystalLaserPoint> crystalPoints3 = bufferAccessor[j];
					DynamicBuffer<HittedEntity> laserHitteds3 = bufferAccessor2[j];
					Execute(entity3, ref config3, ref movement3, ref componentData3, ref crystalData3, in multiShootData3, in manaCostRatio3, ref transform3, ref elementEffect3, crystalPoints3, laserHitteds3, chunkIndexInQuery);
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
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
					ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k);
					ref Spell4014LaserCrystalData crystalData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4014LaserCrystalData>(nativeArrayPtr5, k);
					ref MultiShootData multiShootData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MultiShootData>(nativeArrayPtr6, k);
					ref ManaCostRatio manaCostRatio4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ManaCostRatio>(nativeArrayPtr7, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr8, k);
					ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr9, k);
					DynamicBuffer<CrystalLaserPoint> crystalPoints4 = bufferAccessor[k];
					DynamicBuffer<HittedEntity> laserHitteds4 = bufferAccessor2[k];
					Execute(entity4, ref config4, ref movement4, ref componentData4, ref crystalData4, in multiShootData4, in manaCostRatio4, ref transform4, ref elementEffect4, crystalPoints4, laserHitteds4, chunkIndexInQuery);
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
	private readonly struct IFE_1798367060_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> Get(int index)
			{
				return InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1798367060_0.TypeHandle __IFE_1798367060_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell4014LaserDamageJob.InternalCompilerQueryAndHandleData __Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1798367060_0_TypeHandle = new IFE_1798367060_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private float highestTotalTime;

	private float loopTime;

	private bool doPlaySE;

	private float SoundVolumn;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1798367060_0;

	private EntityQuery __query_1798367060_1;

	private EntityQuery __query_1798367060_2;

	private EntityQuery __query_1798367060_3;

	private EntityQuery __query_1798367060_4;

	private EntityQuery __query_1798367060_5;

	private EntityQuery __query_1798367060_6;

	private EntityQuery __query_1798367060_7;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<LoopSEData>();
		loopTime = 99998f;
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer cmd = __query_1798367060_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		ComponentLookup<LocalTransform> componentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state);
		highestTotalTime = 0f;
		doPlaySE = false;
		foreach (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> item in IFE_1798367060_0.Query(__query_1798367060_0, __TypeHandle.__IFE_1798367060_0_TypeHandle, ref state))
		{
			if (item.ValueRO.IsHit)
			{
				doPlaySE = true;
				if (highestTotalTime < item.ValueRO.KeepHittingRate)
				{
					highestTotalTime = item.ValueRO.KeepHittingRate;
				}
			}
		}
		SoundVolumn = (doPlaySE ? DTool.Lerp(SoundVolumn, 1f, 10f * state.WorldUnmanaged.Time.DeltaTime) : DTool.Lerp(SoundVolumn, 0f, 10f * state.WorldUnmanaged.Time.DeltaTime));
		bool num = Time.timeScale == 0f;
		if (num)
		{
			SoundVolumn = 0f;
			doPlaySE = false;
		}
		loopTime += Time.deltaTime;
		if (loopTime > 99999f)
		{
			loopTime -= 99999f;
			CrystalAudioTryPlay(cmd, __query_1798367060_2.GetSingletonEntity());
		}
		if ((bool)SEMgr.Inst)
		{
			AudioSource value;
			if (doPlaySE)
			{
				if (!SEMgr.Inst.loopSEs.ContainsKey("SE_Spell4014Loop"))
				{
					SEMgr.Inst.PlayLoopSE("SE_Spell4014Loop", 9999f, 0f);
				}
				if (!SEMgr.Inst.loopSEs["SE_Spell4014Loop"].isPlaying)
				{
					SEMgr.Inst.loopSEs["SE_Spell4014Loop"].Play();
				}
				SEMgr.Inst.loopSEs["SE_Spell4014Loop"].pitch = Mathf.Min(0.5f + highestTotalTime / 40f, 2f);
				SEMgr.Inst.loopSEs["SE_Spell4014Loop"].volume = SoundVolumn * DataMgr.settingData.GetFinalSound();
				SEMgr.Inst.loopSEs["SE_Spell4014Loop"].mute = false;
			}
			else if (SEMgr.Inst.loopSEs.TryGetValue("SE_Spell4014Loop", out value))
			{
				value.volume = SoundVolumn * DataMgr.settingData.GetFinalSound();
			}
		}
		if (!num)
		{
			state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4014LaserDamageJob
			{
				CMD = cmd.AsParallelWriter(),
				UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
				TransformLookup = componentLookup,
				DeltaTime = Time.deltaTime,
				SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
				GlobalParticleSystemBufferEntity = __query_1798367060_3.GetSingletonEntity(),
				SEPlayerSingleton = __query_1798367060_4.GetSingletonEntity(),
				PhysicsWorld = __query_1798367060_5.GetSingleton<PhysicsWorldSingleton>(),
				Random = __query_1798367060_6.GetSingleton<GlobalRandom>(),
				Spell3101Buffer = __query_1798367060_7.GetSingletonEntity()
			}, __TypeHandle.__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		}
	}

	private void CrystalAudioTryPlay(EntityCommandBuffer cmd, Entity sePlayerSingleton)
	{
		if ((bool)SEMgr.Inst)
		{
			FixedString32Bytes seName = "Loop";
			FixedString32Bytes spellSEName = DTool.GetSpellSEName(4014, in seName);
			if (!SEMgr.Inst.loopSEs.ContainsKey(spellSEName.ToString()))
			{
				cmd.AppendToBuffer(sePlayerSingleton, new LoopSEData(spellSEName, 99999f, 0f));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4014LaserDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4014LaserDamageApplySystem_Spell4014LaserDamageJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4014LaserCrystalData>();
		__query_1798367060_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<LoopSEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3101NewThunderHitData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1798367060_7 = entityQueryBuilder2.Build(ref state);
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
		((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4014LaserDamageApplySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
