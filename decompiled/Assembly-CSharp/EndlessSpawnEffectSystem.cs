using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(UnitTakeDamageGroup))]
[CompilerGenerated]
public struct EndlessSpawnEffectSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	public struct EndlessSpawnEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<EndlessSpawnEffect> __EndlessSpawnEffect_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__EndlessSpawnEffect_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EndlessSpawnEffect>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__EndlessSpawnEffect_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				DefaultQuery = entityQueryBuilder.WithAllRW<EndlessSpawnEffect>().Build(ref state);
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
			public void Run(ref EndlessSpawnEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref EndlessSpawnEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref EndlessSpawnEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref EndlessSpawnEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref EndlessSpawnEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref EndlessSpawnEffectJob job, EntityManager entityManager)
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

		public float deltaTime;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> tsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<MatOverrideColor> colorLookUp;

		[NativeDisableParallelForRestriction]
		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity createUnitEntity;

		public Entity particleEmitEntity;

		public float3 playerPoint;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref EndlessSpawnEffect effect, Entity entity)
		{
			effect.lifeTimer += deltaTime;
			ref MatOverrideColor valueRW = ref colorLookUp.GetRefRW(effect.effectEntity).ValueRW;
			RefRW<LocalTransform> refRW = tsfLookUp.GetRefRW(entity);
			ref LocalTransform valueRW2 = ref refRW.ValueRW;
			refRW = tsfLookUp.GetRefRW(effect.scaleRoot);
			ref LocalTransform valueRW3 = ref refRW.ValueRW;
			if (effect.lifeTimer < 0f)
			{
				valueRW.color.a = 0f;
			}
			else if (effect.lifeTimer < effect.showTime)
			{
				valueRW3.Scale = (2f * (effect.showTime - effect.lifeTimer) / effect.showTime + 1f) * effect.scale;
				valueRW.color.a = effect.lifeTimer * effect.lifeTimer / (effect.showTime * effect.showTime);
			}
			else if (effect.lifeTimer < effect.showTime + effect.stayTime)
			{
				if (!effect.summoned && effect.lifeTimer > effect.showTime + effect.stayTime * 0.5f)
				{
					effect.summoned = true;
					ecb.AppendToBuffer(index, createUnitEntity, new CreateEndlessMonsterRequest
					{
						monsterID = effect.MonsterID,
						monsterPosition = valueRW2.Position,
						effectSize = effect.scale
					});
				}
				valueRW.color.a = 1f;
			}
			else
			{
				valueRW.color.a = 1f - (effect.lifeTimer - effect.showTime - effect.stayTime) / effect.fadeTime;
				if (effect.lifeTimer > effect.showTime + effect.stayTime + effect.fadeTime)
				{
					ecb.DestroyEntity(index, entity);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__EndlessSpawnEffect_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref EndlessSpawnEffect effect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessSpawnEffect>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(chunkIndexInQuery, ref effect, entity);
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
						ref EndlessSpawnEffect effect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessSpawnEffect>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(chunkIndexInQuery, ref effect2, entity2);
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
					ref EndlessSpawnEffect effect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessSpawnEffect>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(chunkIndexInQuery, ref effect3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref EndlessSpawnEffect effect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessSpawnEffect>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(chunkIndexInQuery, ref effect4, entity4);
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

	public struct CreateEndlessMonsterRequest : IBufferElementData
	{
		public int monsterID;

		public float3 monsterPosition;

		public float effectSize;
	}

	private struct TypeHandle
	{
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<MatOverrideColor> __MatOverrideColor_RW_ComponentLookup;

		public EndlessSpawnEffectJob.InternalCompilerQueryAndHandleData __EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__MatOverrideColor_RW_ComponentLookup = state.GetComponentLookup<MatOverrideColor>();
			__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	public static bool lastFrameStageFinished;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1606132159_0;

	private EntityQuery __query_1606132159_1;

	private EntityQuery __query_1606132159_2;

	private EntityQuery __query_1606132159_3;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CreateEndlessMonsterRequest>();
		state.RequireForUpdate<EndlessSpawnEffect>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.EntityManager.CreateSingletonBuffer<CreateEndlessMonsterRequest>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (!SpecialObj301EndlessMonsterSpawner.Inst)
		{
			return;
		}
		bool stageFinished = SpecialObj301EndlessMonsterSpawner.Inst.StageFinished;
		bool flag = stageFinished && !lastFrameStageFinished;
		lastFrameStageFinished = stageFinished;
		if (!stageFinished)
		{
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
			state.Dependency = __ScheduleViaJobChunkExtension_0(new EndlessSpawnEffectJob
			{
				deltaTime = state.WorldUnmanaged.Time.DeltaTime,
				tsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
				colorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref state),
				ecb = entityCommandBuffer.AsParallelWriter(),
				createUnitEntity = __query_1606132159_0.GetSingletonEntity(),
				particleEmitEntity = __query_1606132159_1.GetSingletonEntity(),
				playerPoint = PlayerMgr.Inst.PlayerPointIgnoreZ
			}, __TypeHandle.__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
			state.Dependency.Complete();
			entityCommandBuffer.Playback(state.EntityManager);
			entityCommandBuffer.Dispose();
			DynamicBuffer<CreateEndlessMonsterRequest> singletonBuffer = __query_1606132159_2.GetSingletonBuffer<CreateEndlessMonsterRequest>();
			NativeArray<CreateEndlessMonsterRequest> nativeArray = singletonBuffer.ToNativeArray(Allocator.Temp);
			singletonBuffer.Clear();
			float hpRatioFix = SpecialObj301EndlessMonsterSpawner.Inst.hpRatioFix;
			float knockBackRatioFix = SpecialObj301EndlessMonsterSpawner.Inst.knockBackRatioFix;
			float frozenTimeRatioFix = SpecialObj301EndlessMonsterSpawner.Inst.frozenTimeRatioFix;
			List<EndlessBattleSpawnInfo.DropCounts> dropCounts = SpecialObj301EndlessMonsterSpawner.Inst.dropCounts;
			NativeList<GlobalParticleEmitParams> nativeList = new NativeList<GlobalParticleEmitParams>(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				CreateEndlessMonsterRequest request = nativeArray[i];
				if (request.monsterID / 100000 == 3)
				{
					SpecialObj301EndlessMonsterSpawner.Inst.SpawnElite(request);
				}
				else
				{
					Entity entity = QuickCreateSystem.Inst.CreateUnit(request.monsterID, request.monsterPosition);
					UnitProperty_Dots componentData = state.EntityManager.GetComponentData<UnitProperty_Dots>(entity);
					componentData.Initialize(UnitConfig.map);
					componentData.unitCfg.knockbackRatio *= knockBackRatioFix;
					componentData.unitCfg.frozenTimeRatio *= frozenTimeRatioFix;
					componentData.unitCfg.maxHP *= hpRatioFix;
					componentData.unitCfg.maxHP = Mathf.Floor(componentData.unitCfg.maxHP);
					componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
					state.EntityManager.SetComponentData(entity, componentData);
					EndlessMonsterTag componentData2 = state.EntityManager.GetComponentData<EndlessMonsterTag>(entity);
					for (int j = 0; j < dropCounts.Count; j++)
					{
						if (request.monsterID == dropCounts[j].unitID)
						{
							componentData2.dropCount = dropCounts[j].dropCount;
							break;
						}
					}
					state.EntityManager.SetComponentData(entity, componentData2);
				}
				SEMgr.Inst.endlessMonsterSummon.PlaySE(request.monsterPosition);
				GlobalParticleEmitParams value = new GlobalParticleEmitParams(GlobalParticleType.EF, "EF_EndlessMonsterBorn", request.monsterPosition + new float3(0f, 0f, -0.03f))
				{
					Size = request.effectSize
				};
				nativeList.Add(in value);
			}
			nativeArray.Dispose();
			DynamicBuffer<GlobalParticleEmitParams> singletonBuffer2 = __query_1606132159_3.GetSingletonBuffer<GlobalParticleEmitParams>();
			for (int k = 0; k < nativeList.Length; k++)
			{
				singletonBuffer2.Add(nativeList[k]);
			}
			nativeList.Dispose();
		}
		else if (flag)
		{
			__query_1606132159_2.GetSingletonBuffer<CreateEndlessMonsterRequest>().Clear();
			EntityQuery entityQuery = state.EntityManager.CreateEntityQuery(typeof(EndlessSpawnEffect));
			NativeArray<Entity> nativeArray2 = entityQuery.ToEntityArray(Allocator.Temp);
			for (int l = 0; l < nativeArray2.Length; l++)
			{
				state.EntityManager.DestroyEntity(nativeArray2[l]);
			}
			nativeArray2.Dispose();
			entityQuery.Dispose();
		}
	}

	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(EndlessSpawnEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__EndlessSpawnEffectSystem_EndlessSpawnEffectJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateEndlessMonsterRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1606132159_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1606132159_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateEndlessMonsterRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1606132159_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1606132159_3 = entityQueryBuilder2.Build(ref state);
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
		((EndlessSpawnEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((EndlessSpawnEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((EndlessSpawnEffectSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((EndlessSpawnEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
