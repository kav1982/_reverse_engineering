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

[BurstCompile]
[CompilerGenerated]
internal struct Relic_ShowUnitHPJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Relic_ShowUnitHP> __Relic_ShowUnitHP_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Relic_ShowUnitHP_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Relic_ShowUnitHP>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Relic_ShowUnitHP_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			DefaultQuery = entityQueryBuilder.WithAllRW<Relic_ShowUnitHP>().Build(ref state);
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
		public void Run(ref Relic_ShowUnitHPJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Relic_ShowUnitHPJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Relic_ShowUnitHPJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Relic_ShowUnitHPJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Relic_ShowUnitHPJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Relic_ShowUnitHPJob job, EntityManager entityManager)
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

	[ReadOnly]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> cluUnitPpt;

	[NativeDisableUnsafePtrRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> cluLocalTsf;

	[NativeDisableUnsafePtrRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<PostTransformMatrix> cluPTM;

	[NativeDisableUnsafePtrRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideNumberAndLength> cluMatOverrideNumberAndLength;

	public EntityCommandBuffer.ParallelWriter ecb;

	public bool isChinese;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int index, ref Relic_ShowUnitHP relic, Entity ett)
	{
		if (!cluUnitPpt.HasComponent(relic.unitEtt))
		{
			ecb.DestroyEntity(index, ett);
			return;
		}
		UnitProperty_Dots unitProperty_Dots = cluUnitPpt[relic.unitEtt];
		LocalTransform localTransform = cluLocalTsf[relic.unitEtt];
		if (!relic.isInitialized)
		{
			relic.isInitialized = true;
			cluLocalTsf.GetRefRW(relic.ett_Hight).ValueRW.Position = new float3(0f, unitProperty_Dots.unitCfg.relicShowHPUIHight * localTransform.Scale, 0f);
			if (unitProperty_Dots.unitCfg.unitType == UnitType.Monster)
			{
				cluLocalTsf.GetRefRW(relic.ett_HPBar_Monster).ValueRW.Scale = 1f;
				cluLocalTsf.GetRefRW(relic.ett_HPBar_Teammate).ValueRW.Scale = 0f;
			}
			else
			{
				cluLocalTsf.GetRefRW(relic.ett_HPBar_Monster).ValueRW.Scale = 0f;
				cluLocalTsf.GetRefRW(relic.ett_HPBar_Teammate).ValueRW.Scale = 1f;
			}
			if (relic.relicLevel == 1)
			{
				cluLocalTsf.GetRefRW(relic.ett_TextRoot).ValueRW.Scale = 0f;
			}
			else
			{
				cluLocalTsf.GetRefRW(relic.ett_TextRoot).ValueRW.Scale = 1f;
				Relic_ShowUnitHPSystem.NumUnitIndex numUnitIndex = Relic_ShowUnitHPSystem.CaculateNumIndex(unitProperty_Dots.unitCfg.currentHP, out var numResult, out var numberLenth, isChinese);
				cluMatOverrideNumberAndLength.GetRefRW(relic.ett_CurrentHP).ValueRW.numberAndLength = new float3(numResult, numberLenth, Relic_ShowUnitHPSystem.GetUnitIndex((float)numUnitIndex));
				float num = ((numUnitIndex >= Relic_ShowUnitHPSystem.NumUnitIndex.K) ? 2 : 0);
				cluPTM.GetRefRW(relic.ett_CurrentHP).ValueRW.Value = float4x4.Scale(new float3(relic.oneNumberScale.x * ((float)numberLenth + num), relic.oneNumberScale.y, 1f));
				cluLocalTsf.GetRefRW(relic.ett_CurrentHP).ValueRW.Position = new float3((0f - relic.oneNumberScale.x) * ((float)numberLenth + num / 2f) / 2f - relic.hpOffset, 0f, 0f);
				numUnitIndex = Relic_ShowUnitHPSystem.CaculateNumIndex(unitProperty_Dots.unitCfg.maxHP, out var numResult2, out numberLenth, isChinese);
				cluMatOverrideNumberAndLength.GetRefRW(relic.ett_MaxHP).ValueRW.numberAndLength = new float3(numResult2, numberLenth, Relic_ShowUnitHPSystem.GetUnitIndex((float)numUnitIndex));
				cluPTM.GetRefRW(relic.ett_MaxHP).ValueRW.Value = float4x4.Scale(new float3(relic.oneNumberScale.x * ((float)numberLenth + num), relic.oneNumberScale.y, 1f));
				cluLocalTsf.GetRefRW(relic.ett_MaxHP).ValueRW.Position = new float3(relic.oneNumberScale.x * ((float)numberLenth + num) / 2f + relic.hpOffset, 0f, 0f);
				relic.lastRecordMaxHP = unitProperty_Dots.unitCfg.maxHP;
			}
		}
		bool flag = (unitProperty_Dots.unitCfg.unitType == UnitType.Teammate || unitProperty_Dots.unitCfg.unitType == UnitType.TeammateNotAttack) && relic.teammateTransparent == 0f;
		bool flag2 = unitProperty_Dots.showAffect && !flag;
		if (flag2 && cluLocalTsf.GetRefRW(ett).ValueRW.Scale == 0f)
		{
			cluLocalTsf.GetRefRW(ett).ValueRW.Scale = 1f;
		}
		else if (!flag2 && cluLocalTsf.GetRefRW(ett).ValueRW.Scale != 0f)
		{
			cluLocalTsf.GetRefRW(ett).ValueRW.Scale = 0f;
		}
		if (relic.lastRecordCurrentHP != unitProperty_Dots.unitCfg.currentHP)
		{
			relic.lastRecordCurrentHP = unitProperty_Dots.unitCfg.currentHP;
			float x = unitProperty_Dots.unitCfg.currentHP / unitProperty_Dots.unitCfg.maxHP;
			cluPTM.GetRefRW(relic.ett_HPBarRoot).ValueRW.Value = float4x4.Scale(new float3(x, 1f, 1f));
			if (relic.relicLevel > 1)
			{
				Relic_ShowUnitHPSystem.NumUnitIndex numUnitIndex2 = Relic_ShowUnitHPSystem.CaculateNumIndex(unitProperty_Dots.unitCfg.currentHP, out var numResult3, out var numberLenth2, isChinese);
				cluMatOverrideNumberAndLength.GetRefRW(relic.ett_CurrentHP).ValueRW.numberAndLength = new float3(numResult3, numberLenth2, Relic_ShowUnitHPSystem.GetUnitIndex((float)numUnitIndex2));
				float num2 = ((numUnitIndex2 >= Relic_ShowUnitHPSystem.NumUnitIndex.K) ? 2 : 0);
				cluPTM.GetRefRW(relic.ett_CurrentHP).ValueRW.Value = float4x4.Scale(new float3(relic.oneNumberScale.x * ((float)numberLenth2 + num2), relic.oneNumberScale.y, 1f));
				cluLocalTsf.GetRefRW(relic.ett_CurrentHP).ValueRW.Position = new float3((0f - relic.oneNumberScale.x) * ((float)numberLenth2 + num2 / 2f) / 2f - relic.hpOffset, 0f, 0f);
				if (relic.lastRecordMaxHP != unitProperty_Dots.unitCfg.maxHP)
				{
					relic.lastRecordMaxHP = unitProperty_Dots.unitCfg.maxHP;
					numUnitIndex2 = Relic_ShowUnitHPSystem.CaculateNumIndex(unitProperty_Dots.unitCfg.currentHP, out var numResult4, out numberLenth2, isChinese);
					num2 = ((numUnitIndex2 >= Relic_ShowUnitHPSystem.NumUnitIndex.K) ? 2 : 0);
					cluMatOverrideNumberAndLength.GetRefRW(relic.ett_MaxHP).ValueRW.numberAndLength = new float3(numResult4, numberLenth2, Relic_ShowUnitHPSystem.GetUnitIndex((float)numUnitIndex2));
					cluPTM.GetRefRW(relic.ett_MaxHP).ValueRW.Value = float4x4.Scale(new float3(relic.oneNumberScale.x * ((float)numberLenth2 + num2), relic.oneNumberScale.y, 1f));
					cluLocalTsf.GetRefRW(relic.ett_MaxHP).ValueRW.Position = new float3(relic.oneNumberScale.x * ((float)numberLenth2 + num2) / 2f + relic.hpOffset, 0f, 0f);
				}
			}
		}
		cluLocalTsf.GetRefRW(ett).ValueRW.Position = cluLocalTsf[relic.unitEtt].Position;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Relic_ShowUnitHP_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Relic_ShowUnitHP relic = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Relic_ShowUnitHP>(nativeArrayPtr, i);
				Entity ett = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(chunkIndexInQuery, ref relic, ett);
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
					ref Relic_ShowUnitHP relic2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Relic_ShowUnitHP>(nativeArrayPtr, nextRangeBegin);
					Entity ett2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(chunkIndexInQuery, ref relic2, ett2);
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
				ref Relic_ShowUnitHP relic3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Relic_ShowUnitHP>(nativeArrayPtr, j);
				Entity ett3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(chunkIndexInQuery, ref relic3, ett3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Relic_ShowUnitHP relic4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Relic_ShowUnitHP>(nativeArrayPtr, k);
				Entity ett4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(chunkIndexInQuery, ref relic4, ett4);
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
