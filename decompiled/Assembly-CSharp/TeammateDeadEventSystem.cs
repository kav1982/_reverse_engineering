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
[UpdateInGroup(typeof(SpellEndSystemGroup))]
[UpdateBefore(typeof(SpellDestroySystem))]
[CompilerGenerated]
internal struct TeammateDeadEventSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct TeammateDeadEventJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<TeammateDeadTag> __TeammateDeadTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__TeammateDeadTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateDeadTag>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				}

				public void Update(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__TeammateDeadTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateDeadTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
			public void Run(ref TeammateDeadEventJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref TeammateDeadEventJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref TeammateDeadEventJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref TeammateDeadEventJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref TeammateDeadEventJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref TeammateDeadEventJob job, EntityManager entityManager)
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

		public Entity DeadBloodSingletonBufferEntity;

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity spell3015WormSpawnBufferEntity;

		public Entity spell3118SelfSacrificeSpawnBufferEntity;

		public Entity spell3127DestroyBufferEntity;

		public Entity spellUnfollowingRequireEntity;

		[NativeDisableUnsafePtrRestriction]
		public GlobalRandom gRandom;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<Spell3127SoulMateComponent> spell3127LookUp;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		public DynamicOptimizeData DynamicOptimizeData;

		private int _wormSpawnComplexityInThisFrame;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(TeammateData teammateData, LocalTransform trans, Entity entity, SpellConfigComponentData config, TeammateDeadTag _, [ChunkIndexInQuery] int chunkIndex, ref SpellComponentData data, ref SpellElementEffectComponentData element, ref UnitProperty_Dots teammatePpt)
		{
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
			data.DisableSplitEffect = true;
			teammatePpt.CanBeTarget = false;
			if (!teammateData.IsFuseMaterial && config.AbilityType != SpellAbilityType.Summon4)
			{
				CMD.AppendToBuffer(chunkIndex, DeadBloodSingletonBufferEntity, new TeammateDeadBloodEffectBuffer
				{
					spawnPosition = trans.Position,
					spawnScale = 0.8f
				});
			}
			SpellComponentData data2 = data;
			SpellElementEffectComponentData element2 = element;
			SpellConfigComponentData config2 = config;
			data2.SpellEffectEntity = Entity.Null;
			data2.TrailEffectEntity = Entity.Null;
			config2.AbilityType = SpellAbilityType.ParasiticWorm;
			if (teammateData.OnDeathSpawnWormCount > 0)
			{
				Spell3015WormSpawnBuffer spell3015WormSpawnBuffer = default(Spell3015WormSpawnBuffer);
				spell3015WormSpawnBuffer.moveSpeed = 3f;
				spell3015WormSpawnBuffer.radius = config2.Radius.Calculate();
				spell3015WormSpawnBuffer.data = data2;
				spell3015WormSpawnBuffer.config = config2;
				spell3015WormSpawnBuffer.element = element2;
				spell3015WormSpawnBuffer.wormColorType = config2.ColorType;
				Spell3015WormSpawnBuffer wormSpawnBuffer = spell3015WormSpawnBuffer;
				wormSpawnBuffer.config.Damage.Base = 15f;
				int optimizedWormSpawnCount = GetOptimizedWormSpawnCount(teammateData.OnDeathSpawnWormCount, ref wormSpawnBuffer);
				float degree = gRandom.random.NextFloat(0f, 180f);
				for (int i = 0; i < optimizedWormSpawnCount; i++)
				{
					wormSpawnBuffer.spawnPosition = trans.Position + (float3)Tool2D.GetDir(Tool2D.GetDir(Vector3.up, degree), 360f / (float)optimizedWormSpawnCount * (float)i) * 0.5f;
					wormSpawnBuffer.spawnPosition = Tool2D.IgnoreZPoint(wormSpawnBuffer.spawnPosition);
					CMD.AppendToBuffer(chunkIndex, spell3015WormSpawnBufferEntity, wormSpawnBuffer);
				}
			}
			if (teammateData.ExplodeRange > 0f)
			{
				SpellComponentData data3 = data;
				SpellElementEffectComponentData elementEffect = element;
				SpellConfigComponentData config3 = config;
				data3.SpellEffectEntity = Entity.Null;
				data3.TrailEffectEntity = Entity.Null;
				config3.AbilityType = SpellAbilityType.TeammateSacrifice;
				config3.Damage.Base = math.max(teammatePpt.unitCfg.maxHP * 0.5f, teammatePpt.unitCfg.currentHP) * teammateData.ExplodeHpDamageRatio;
				SpellMovementComponentData movement = default(SpellMovementComponentData);
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config3, in movement, in trans, in elementEffect, in data3, out var info);
				NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
				ref float3 position = ref trans.Position;
				float radius = SpellConfigLookup.GetRefRO(entity).ValueRO.Radius.CalculateWithNewBaseValue(teammateData.ExplodeRange);
				UnitType selfCamp = UnitType.Teammate;
				SpellTools.GetAttackableEntitiesInRange(in position, in radius, in selfCamp, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
				foreach (Entity item in entities)
				{
					Entity target = item;
					CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
				}
				SpellTools.GetTeammateSacrificeRange(teammateData.ExplodeRange, in config, out var result);
				CMD.AppendToBuffer(chunkIndex, spell3118SelfSacrificeSpawnBufferEntity, new Spell3118SelfSacrificeSpawnBuffer
				{
					spawnPosition = trans.Position,
					spellColorType = SpellConfigLookup.GetRefRO(entity).ValueRO.ColorType,
					ExplosionRange = result
				});
			}
			if (teammateData.SpellSummonGainOwnerHpRatio > 0f && spell3127LookUp.HasComponent(entity) && spell3127LookUp.GetRefRO(entity).ValueRO.RingEffect != Entity.Null)
			{
				CMD.AppendToBuffer(chunkIndex, spell3127DestroyBufferEntity, new Spell3127DestoryBuffer
				{
					entity = entity,
					effect = spell3127LookUp.GetRefRO(entity).ValueRO.RingEffect
				});
			}
		}

		private int GetOptimizedWormSpawnCount(int spawnCount, ref Spell3015WormSpawnBuffer wormSpawnBuffer)
		{
			if (spawnCount <= 0)
			{
				return 0;
			}
			int num = (DynamicOptimizeData.IsMobilePlatform ? 30 : 60);
			int threshold = (DynamicOptimizeData.IsMobilePlatform ? 45 : 75);
			float lowFpsActiveThreshold = SpellTools.GetLowFpsActiveThreshold(DynamicOptimizeData.IsMobilePlatform);
			int num2 = SpellTools.CalculateSpellComplexity(SpellAbilityType.ParasiticWorm);
			int num3 = num2 * spawnCount;
			if (DynamicOptimizeData.IsLowFpsOptimizeActive(lowFpsActiveThreshold) || _wormSpawnComplexityInThisFrame + num3 >= num)
			{
				float currentFPS = DynamicOptimizeData.CurrentFPS;
				float maxOptimizeFPSThreshold = SpellTools.GetMaxOptimizeFPSThreshold(DynamicOptimizeData.IsMobilePlatform);
				float num4 = math.floor((float)spawnCount * (currentFPS / lowFpsActiveThreshold));
				num4 = SpellTools.GetFinalSpawnCountWithLimitCount(num, 3, threshold, 1, _wormSpawnComplexityInThisFrame, (int)num4);
				if (currentFPS <= maxOptimizeFPSThreshold || num4 < 1f)
				{
					num4 = 1f;
				}
				float num5 = (float)spawnCount / num4;
				wormSpawnBuffer.config.Damage.MulRatio *= num5;
				wormSpawnBuffer.data.SpellEfficiency *= num5;
				wormSpawnBuffer.element.VenomApplyCount *= num5;
				spawnCount = (int)num4;
			}
			_wormSpawnComplexityInThisFrame += num2 * spawnCount;
			return spawnCount;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref TeammateData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, i);
					ref LocalTransform reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
					ref SpellConfigComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
					ref SpellElementEffectComponentData element = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
					ref UnitProperty_Dots teammatePpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, i);
					Execute(reference, reference2, entity, reference3, default(TeammateDeadTag), chunkIndexInQuery, ref data, ref element, ref teammatePpt);
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
						ref TeammateData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform reference5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
						ref SpellConfigComponentData reference6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellElementEffectComponentData element2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref UnitProperty_Dots teammatePpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, nextRangeBegin);
						Execute(reference4, reference5, entity2, reference6, default(TeammateDeadTag), chunkIndexInQuery, ref data2, ref element2, ref teammatePpt2);
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
					ref TeammateData reference7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, j);
					ref LocalTransform reference8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
					ref SpellConfigComponentData reference9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
					ref SpellElementEffectComponentData element3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
					ref UnitProperty_Dots teammatePpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, j);
					Execute(reference7, reference8, entity3, reference9, default(TeammateDeadTag), chunkIndexInQuery, ref data3, ref element3, ref teammatePpt3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref TeammateData reference10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, k);
					ref LocalTransform reference11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
					ref SpellConfigComponentData reference12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
					ref SpellElementEffectComponentData element4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
					ref UnitProperty_Dots teammatePpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, k);
					Execute(reference10, reference11, entity4, reference12, default(TeammateDeadTag), chunkIndexInQuery, ref data4, ref element4, ref teammatePpt4);
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
	private readonly struct IFE_1629008127_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateOwner>, InternalCompilerInterface.UncheckedRefRW<TeammateDeadTag>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateOwner>, InternalCompilerInterface.UncheckedRefRW<TeammateDeadTag>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateOwner>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateDeadTag>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<TeammateOwner> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateDeadTag> item2_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateOwner>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateDeadTag>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateOwner>, InternalCompilerInterface.UncheckedRefRW<TeammateDeadTag>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateOwner>, InternalCompilerInterface.UncheckedRefRW<TeammateDeadTag>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<TeammateOwner>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateDeadTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1629008127_0.TypeHandle __IFE_1629008127_0_TypeHandle;

		public ComponentLookup<Spell3127SoulMateComponent> __Spell3127SoulMateComponent_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public TeammateDeadEventJob.InternalCompilerQueryAndHandleData __TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1629008127_0_TypeHandle = new IFE_1629008127_0.TypeHandle(ref state);
			__Spell3127SoulMateComponent_RW_ComponentLookup = state.GetComponentLookup<Spell3127SoulMateComponent>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00009094_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00009094_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00009094_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1629008127_0;

	private EntityQuery __query_1629008127_1;

	private EntityQuery __query_1629008127_2;

	private EntityQuery __query_1629008127_3;

	private EntityQuery __query_1629008127_4;

	private EntityQuery __query_1629008127_5;

	private EntityQuery __query_1629008127_6;

	private EntityQuery __query_1629008127_7;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<TeammateGhostEffectData>();
		state.RequireForUpdate<TeammateDeadBloodEffectBuffer>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<Spell3015WormSpawnBuffer>();
		state.RequireForUpdate<Spell3118SelfSacrificeSpawnBuffer>();
		state.RequireForUpdate<Spell3127DestoryBuffer>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<TeammateDeadTag>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<TeammateOwner>, InternalCompilerInterface.UncheckedRefRW<TeammateDeadTag>> item3 in IFE_1629008127_0.Query(__query_1629008127_0, __TypeHandle.__IFE_1629008127_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var _, out var _, out var entity);
			Entity childEntity = entity;
			SpellTools.KillAllChildTeammates(state.EntityManager, in childEntity);
		}
		__ScheduleViaJobChunkExtension_0(new TeammateDeadEventJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			DeadBloodSingletonBufferEntity = __query_1629008127_1.GetSingletonEntity(),
			spell3015WormSpawnBufferEntity = __query_1629008127_2.GetSingletonEntity(),
			gRandom = __query_1629008127_3.GetSingleton<GlobalRandom>(),
			spell3118SelfSacrificeSpawnBufferEntity = __query_1629008127_4.GetSingletonEntity(),
			spell3127DestroyBufferEntity = __query_1629008127_5.GetSingletonEntity(),
			spell3127LookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell3127SoulMateComponent_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			PhysicsWorld = __query_1629008127_6.GetSingleton<PhysicsWorldSingleton>(),
			DynamicOptimizeData = __query_1629008127_7.GetSingleton<DynamicOptimizeData>()
		}, __TypeHandle.__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(TeammateDeadEventJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__TeammateDeadEventSystem_TeammateDeadEventJob_WithDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateOwner>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateDeadTag>();
		__query_1629008127_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateDeadBloodEffectBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3015WormSpawnBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3118SelfSacrificeSpawnBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3127DestoryBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1629008127_7 = entityQueryBuilder2.Build(ref state);
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
		((TeammateDeadEventSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00009094_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateDeadEventSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateDeadEventSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
