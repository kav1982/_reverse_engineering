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
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateBefore(typeof(UnitBeforeTakeDamageSystem))]
[UpdateAfter(typeof(UnitEnvironmentSystem))]
[BurstCompile]
public struct TakeDamageInfoPreProcessSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	private struct TakeDamageInfoPreProcessJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public BufferTypeHandle<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__TakeDamageInfo_Dots_RW_BufferTypeHandle = state.GetBufferTypeHandle<TakeDamageInfo_Dots>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__TakeDamageInfo_Dots_RW_BufferTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TakeDamageInfo_Dots>();
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
			public void Run(ref TakeDamageInfoPreProcessJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref TakeDamageInfoPreProcessJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref TakeDamageInfoPreProcessJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref TakeDamageInfoPreProcessJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref TakeDamageInfoPreProcessJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref TakeDamageInfoPreProcessJob job, EntityManager entityManager)
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
		public ComponentLookup<LocalTransform> cluLocalTsf;

		public EntityCommandBuffer.ParallelWriter ecb;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> unitPptLookUp;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> gRandom;

		public Entity uiTextFloatByJobEtt;

		public float relic_ReduceDamageRatio;

		public float relic_DodgeChance;

		public float relic_AddCriticleDamageRatio;

		public float relic_PowerfulManIncreaseDamage;

		public float curse_TargetedTrapExtraDamageRatio;

		public float curse_VulnerabilityExtraDamageRatio;

		public float curse_MoreMoneyMoreInjuredExtraDamageRatio;

		public float curse_EnemyReduceDamageRatio;

		public int GreenRuneDecreaseDamageReceiveAmount;

		public float endlessPercentDamageReduce;

		public float endlessMonsterReceiveDamageReduce;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer, ref UnitProperty_Dots unitPpt, Entity entity, [ChunkIndexInQuery] int index)
		{
			if (unitPpt.disabled)
			{
				return;
			}
			RefRO<LocalTransform> refRO = cluLocalTsf.GetRefRO(entity);
			for (int i = 0; i < takeDamageBuffer.Length; i++)
			{
				ref TakeDamageInfo_Dots reference = ref takeDamageBuffer.ElementAt(i);
				if (unitPpt.IsInvincible)
				{
					reference.immuneDamage = true;
				}
				if (unitPpt.unitCfg.isSolidObj && !reference.isUndifferDamage)
				{
					reference.immuneDamage = true;
				}
				if (unitPpt.isDead)
				{
					reference.targetAlreadyDeadBeforeDamage = true;
					continue;
				}
				if (reference.spell.Entity != Entity.Null)
				{
					switch (reference.spell.Config.Id / 10)
					{
					case 9002:
					case 9004:
					case 9006:
					case 9017:
					case 9045:
					{
						UnitType unitType = unitPpt.unitCfg.unitType;
						if (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
						{
							reference.damage *= 2f;
						}
						break;
					}
					case 9012:
					case 9020:
					case 9043:
					{
						UnitType unitType = unitPpt.unitCfg.unitType;
						if (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
						{
							reference.damage *= 3f;
						}
						break;
					}
					case 9021:
					case 9022:
					case 9026:
					case 9027:
					case 9044:
					{
						UnitType unitType = unitPpt.unitCfg.unitType;
						if (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
						{
							reference.damage *= 3f;
						}
						break;
					}
					case 9015:
						if (unitPpt.unitCfg.unitType == UnitType.Player)
						{
							reference.damage *= 0.05f;
						}
						break;
					}
				}
				switch (unitPpt.unitCfg.unitType)
				{
				case UnitType.Player:
					if (reference.attackerType == AttackerType.FromUI)
					{
						unitPpt.unitCfg.currentHP = 0f;
						unitPpt.unitCfg.shield = 0f;
						unitPpt.unitCfg.shieldTemp = 0f;
						break;
					}
					if (!reference.ignorePlayerInvincibleFrame && unitPpt.IsPlayerInInvincibleFrame)
					{
						reference.knockbackForce = Vector3.zero;
						reference.immuneDamage = true;
					}
					if (reference.isTrapDamage && curse_TargetedTrapExtraDamageRatio > 0f)
					{
						reference.damage *= 1f + curse_TargetedTrapExtraDamageRatio;
					}
					if (relic_DodgeChance > 0f && !reference.ignoreRelicDodge && DTool.RandomValue(ref gRandom.ValueRW.random) <= relic_DodgeChance)
					{
						reference.knockbackForce = Vector3.zero;
						reference.immuneDamage = true;
						ecb.AppendToBuffer(index, uiTextFloatByJobEtt, new UITextFloatByJobBED
						{
							textID = 1002001,
							type = UITextFloatType.Normal,
							worldPos = refRO.ValueRO.Position
						});
					}
					if (relic_ReduceDamageRatio > 0f && !reference.ignoreRelicOrCurseDamageRatioChange)
					{
						reference.damage *= 1f - relic_ReduceDamageRatio;
					}
					if (relic_PowerfulManIncreaseDamage > 0f && !reference.ignoreRelicOrCurseDamageRatioChange)
					{
						reference.damage *= 1f + relic_PowerfulManIncreaseDamage;
					}
					if (curse_VulnerabilityExtraDamageRatio > 0f && !reference.ignoreRelicOrCurseDamageRatioChange)
					{
						reference.damage *= 1f + curse_VulnerabilityExtraDamageRatio;
					}
					if (curse_MoreMoneyMoreInjuredExtraDamageRatio > 0f && !reference.ignoreRelicOrCurseDamageRatioChange)
					{
						reference.damage *= 1f + curse_MoreMoneyMoreInjuredExtraDamageRatio;
					}
					if (reference.teammateTakeDamageRatio != 1f)
					{
						reference.damage *= reference.playerTakeDamageRatio;
					}
					if (reference.isUndifferDamage)
					{
						if (reference.spell.Entity != Entity.Null)
						{
							reference.damage *= math.min(reference.spell.Config.UndifferDamageRatio, 0.3333f);
						}
						else
						{
							reference.damage *= 0.3333f;
						}
						if (reference.spell.Config.MaxUndifferDamageReceive >= 0f)
						{
							reference.damage = math.min(reference.spell.Config.MaxUndifferDamageReceive, reference.damage);
						}
					}
					reference.damage -= GreenRuneDecreaseDamageReceiveAmount;
					reference.damage = math.max(0.1f, reference.damage);
					break;
				case UnitType.Teammate:
				case UnitType.TeammateNotAttack:
					if (reference.isTrapDamage && curse_TargetedTrapExtraDamageRatio > 0f)
					{
						reference.damage *= 1f + curse_TargetedTrapExtraDamageRatio;
					}
					reference.damage *= reference.teammateTakeDamageRatio;
					break;
				case UnitType.Monster:
				case UnitType.Elite:
				case UnitType.Boss:
					if (curse_EnemyReduceDamageRatio > 0f && !reference.isPercentageDamage)
					{
						reference.damage *= 1f - curse_EnemyReduceDamageRatio;
					}
					if (endlessPercentDamageReduce > 0f && reference.isPercentageDamage)
					{
						reference.damage *= 1f - endlessPercentDamageReduce;
					}
					if (endlessMonsterReceiveDamageReduce > 0f)
					{
						reference.damage *= endlessMonsterReceiveDamageReduce;
					}
					break;
				}
				if (unitPpt.HasTakeDamageIncreaseEffect && !reference.isPercentageDamage)
				{
					unitPpt.GetTakeDamageIncreaseRatio(out var ratio);
					reference.damage *= ratio;
				}
				if (reference.isDamageCritical)
				{
					continue;
				}
				float num = reference.extraCriticalChance;
				if (reference.spell.Entity != Entity.Null)
				{
					num = reference.spell.Config.CriticalChance;
				}
				if (num > 0f && DTool.RandomValue(ref gRandom.ValueRW.random) <= num)
				{
					reference.isDamageCritical = true;
					if (relic_AddCriticleDamageRatio > 0f && reference.attackerEntity != Entity.Null && unitPptLookUp.TryGetComponent(reference.attackerEntity, out var _) && unitPptLookUp.GetRefRO(reference.attackerEntity).ValueRO.unitCfg.IsSameCamp(UnitType.Player))
					{
						reference.damage *= relic_AddCriticleDamageRatio;
					}
					else
					{
						reference.damage *= 2f;
					}
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			BufferAccessor<TakeDamageInfo_Dots> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer = bufferAccessor[i];
					ref UnitProperty_Dots unitPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(takeDamageBuffer, ref unitPpt, entity, chunkIndexInQuery);
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
						DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer2 = bufferAccessor[nextRangeBegin];
						ref UnitProperty_Dots unitPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(takeDamageBuffer2, ref unitPpt2, entity2, chunkIndexInQuery);
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
					DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer3 = bufferAccessor[j];
					ref UnitProperty_Dots unitPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(takeDamageBuffer3, ref unitPpt3, entity3, chunkIndexInQuery);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					DynamicBuffer<TakeDamageInfo_Dots> takeDamageBuffer4 = bufferAccessor[k];
					ref UnitProperty_Dots unitPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(takeDamageBuffer4, ref unitPpt4, entity4, chunkIndexInQuery);
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
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public TakeDamageInfoPreProcessJob.InternalCompilerQueryAndHandleData __TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009012_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009012_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009012_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_312111373_0;

	private EntityQuery __query_312111373_1;

	private EntityQuery __query_312111373_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<UITextFloatByJobBED>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PlayerController_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if ((bool)PlayerMgr.Inst && (bool)PlayerMgr.Inst.ItemCtrller && (bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
			EntityCommandBuffer entityCommandBuffer = __query_312111373_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			state.Dependency = __ScheduleViaJobChunkExtension_0(new TakeDamageInfoPreProcessJob
			{
				cluLocalTsf = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
				gRandom = __query_312111373_1.GetSingletonRW<GlobalRandom>(),
				ecb = entityCommandBuffer.AsParallelWriter(),
				uiTextFloatByJobEtt = __query_312111373_2.GetSingletonEntity(),
				unitPptLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
				relic_ReduceDamageRatio = ((itemCtrller.relicCfg_ReduceDamage != null) ? ((float)itemCtrller.relicCfg_ReduceDamage.int1.result / 100f) : 0f),
				relic_DodgeChance = ((itemCtrller.relicCfg_Dodge != null) ? ((float)itemCtrller.relicCfg_Dodge.int1.result / 100f) : 0f),
				relic_AddCriticleDamageRatio = ((itemCtrller.relicCfg_AddCriticalDamage != null) ? ((float)itemCtrller.relicCfg_AddCriticalDamage.int1.result / 100f) : 0f),
				relic_PowerfulManIncreaseDamage = ((itemCtrller.relicCfg_PowerfulMan != null) ? ((float)itemCtrller.relicCfg_PowerfulMan.int2.result / 100f) : 0f),
				curse_TargetedTrapExtraDamageRatio = ((itemCtrller.curseCfg_TargetedTrap != null) ? ((float)itemCtrller.curseCfg_TargetedTrap.int1.result / 100f) : 0f),
				curse_VulnerabilityExtraDamageRatio = ((itemCtrller.curseCfg_Vulnerability != null) ? ((float)itemCtrller.curseCfg_Vulnerability.int1.result / 100f) : 0f),
				curse_MoreMoneyMoreInjuredExtraDamageRatio = ((itemCtrller.curseCfg_MoreMoneyMoreInjured != null) ? itemCtrller.curseCfg_MoreMoneyMoreInjured.floatTimer : 0f),
				curse_EnemyReduceDamageRatio = ((itemCtrller.curseCfg_EnemyReduceDamage != null) ? ((float)itemCtrller.curseCfg_EnemyReduceDamage.int1.result / 100f) : 0f),
				GreenRuneDecreaseDamageReceiveAmount = PlayerMgr.Inst.PlayerCtrller.GetGreenRuneDecreaseDamageAmount(),
				endlessPercentDamageReduce = ((GameMgr.InEndlessMode && (bool)SpecialObj301EndlessMonsterSpawner.Inst) ? SpecialObj301EndlessMonsterSpawner.Inst.percentDamageReduce : 0f)
			}, __TypeHandle.__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(TakeDamageInfoPreProcessJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__TakeDamageInfoPreProcessSystem_TakeDamageInfoPreProcessJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_312111373_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_312111373_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<UITextFloatByJobBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_312111373_2 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00009012_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((TakeDamageInfoPreProcessSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TakeDamageInfoPreProcessSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TakeDamageInfoPreProcessSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
