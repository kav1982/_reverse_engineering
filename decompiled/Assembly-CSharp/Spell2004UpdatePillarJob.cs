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
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[WithNone(new Type[] { typeof(Spell2004PillarInitializeTag) })]
[BurstCompile]
public struct Spell2004UpdatePillarJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell2004PillarOfLightData> __Spell2004PillarOfLightData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			public BufferTypeHandle<Spell2004PillarBuffer> __Spell2004PillarBuffer_RW_BufferTypeHandle;

			public BufferTypeHandle<Spell2004WallBuffer> __Spell2004WallBuffer_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2004PillarOfLightData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Spell2004PillarBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell2004PillarBuffer>();
				__Spell2004WallBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell2004WallBuffer>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Spell2004PillarOfLightData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Spell2004PillarBuffer_RW_BufferTypeHandle.Update(ref state);
				__Spell2004WallBuffer_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarOfLightData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarBuffer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004WallBuffer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell2004PillarInitializeTag>();
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
		public void Run(ref Spell2004UpdatePillarJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2004UpdatePillarJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2004UpdatePillarJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2004UpdatePillarJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2004UpdatePillarJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2004UpdatePillarJob job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<EffectsCollectorData> effectCollectorLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> transformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell2004HpRatioMaterialProperty> hpRatioMatLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell2004RotateAngleMaterialProperty> angleMatLookup;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private float GetPillarDistanceToCenterPoint(int totalPillarCount)
	{
		return totalPillarCount switch
		{
			1 => 0f, 
			2 => 1f, 
			_ => 1f / math.sin(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f)), 
		};
	}

	[BurstCompile]
	private float GetWallDistanceToCenterPoint(float toPillarDistance, int totalPillarCount)
	{
		if (totalPillarCount == 2)
		{
			return toPillarDistance;
		}
		return toPillarDistance * math.cos(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f));
	}

	[BurstCompile]
	private void Execute(ref LocalTransform trans, ref TeammateData teammateData, UnitProperty_Dots unit, ref Spell2004PillarOfLightData spellData, SpellMovementComponentData movement, in SpellConfigComponentData config, DynamicBuffer<Spell2004PillarBuffer> pillarBuffer, DynamicBuffer<Spell2004WallBuffer> wallBuffer)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			return;
		}
		RefRW<LocalTransform> refRW;
		if (spellData.AttackState == Spell2004PillarOfLightData.CrushAttackState.ReadyToAttack)
		{
			float num = 0.05f / teammateData.TeammateSpeedRatio;
			spellData.CrushAttackAnimTimer += DeltaTime;
			float num2 = math.clamp(spellData.CrushAttackAnimTimer / num, 0f, 1f);
			foreach (Spell2004PillarBuffer item in pillarBuffer)
			{
				refRW = transformLookup.GetRefRW(effectCollectorLookup[item.Entity].Effect1);
				ref LocalTransform valueRW = ref refRW.ValueRW;
				valueRW.Position.y = math.lerp(valueRW.Position.y, 0f, num2);
			}
			if (num2 >= 1f)
			{
				spellData.AttackState = Spell2004PillarOfLightData.CrushAttackState.DelayToAttack;
			}
		}
		float2 dir = movement.Direction.xy;
		trans.Rotation = DTool.DirectionToRotation(in dir);
		int num3 = teammateData.TeammateCurrentFuseLevel + 1;
		float num4 = math.atan2(movement.Direction.y, movement.Direction.x) / MathF.PI * 180f;
		if (num4 < 0f)
		{
			num4 += 360f;
		}
		float3 layerPosition = DTool.GetLayerPosition(in trans.Position, LayerCorrectType.Coordinate);
		layerPosition += trans.Position;
		float num5 = GetPillarDistanceToCenterPoint(num3) * trans.Scale;
		float num6 = ((num3 > 2) ? GetWallDistanceToCenterPoint(num5, num3) : 0f);
		float num7 = 360f / (float)num3;
		float num8 = num7 / 2f;
		float num9 = 0f;
		if (teammateData.AdvanceSkillLevel > 0)
		{
			spellData.PillarFloatTimer += DeltaTime;
			num9 = 0.6f + 0.2f * Mathf.Sin(spellData.PillarFloatTimer * 2f);
		}
		if (spellData.SelfScaleTimer < 1f)
		{
			spellData.SelfScaleTimer += DeltaTime;
			if (spellData.SelfScaleTimer >= 1f)
			{
				spellData.SelfScaleTimer = 1f;
			}
		}
		float value = unit.unitCfg.currentHP / unit.unitCfg.maxHP;
		for (int i = 0; i < num3; i++)
		{
			float num10 = num4 + num7 * (float)i;
			float3 dir2 = DTool.GetDir(num10 * (MathF.PI / 180f));
			EffectsCollectorData effectsCollectorData = effectCollectorLookup[pillarBuffer[i].Entity];
			refRW = transformLookup.GetRefRW(pillarBuffer[i].Entity);
			ref LocalTransform valueRW2 = ref refRW.ValueRW;
			if (spellData.SelfScaleTimer < 1f)
			{
				dir = new float2(0.5f, 0.7f);
				float2 p = new float2(0.8f, 1.1f);
				float curveY = DTool.GetCurveY(in dir, in p, spellData.SelfScaleTimer * 2.5f);
				valueRW2.Scale = curveY * trans.Scale;
			}
			refRW = transformLookup.GetRefRW(effectsCollectorData.Effect2);
			refRW.ValueRW.Position = unit.beHitDir * unit.beHitCurrentOffsetAmount;
			float3 @float = (valueRW2.Position = dir2 * num5 + layerPosition);
			hpRatioMatLookup.GetRefRW(effectsCollectorData.Effect3).ValueRW.Value = value;
			if (config.ColorType == SpellColorType.Fire || config.ColorType == SpellColorType.Void)
			{
				hpRatioMatLookup.GetRefRW(effectsCollectorData.Effect5).ValueRW.Value = value;
			}
			refRW = transformLookup.GetRefRW(effectsCollectorData.Effect1);
			ref LocalTransform valueRW3 = ref refRW.ValueRW;
			if (teammateData.AdvanceSkillLevel > 0)
			{
				valueRW3.Position.y = DTool.Lerp(valueRW3.Position.y, num9, spellData.CurrentFloatingLerpSpeed * DeltaTime);
				Entity effect = effectsCollectorData.Effect4;
				refRW = transformLookup.GetRefRW(effect);
				refRW.ValueRW.Scale = 1f - num9 * 0.5f;
			}
			if ((num3 != 2 || i != 1) && num3 != 1)
			{
				float value2 = num10 + num8;
				float3 dir3 = DTool.GetDir((num10 + num8) * (MathF.PI / 180f));
				EffectsCollectorData effectsCollectorData2 = effectCollectorLookup[wallBuffer[i].Entity];
				refRW = transformLookup.GetRefRW(wallBuffer[i].Entity);
				ref LocalTransform valueRW4 = ref refRW.ValueRW;
				valueRW4.Position = dir3 * num6 + layerPosition;
				if (spellData.SelfScaleTimer < 1f)
				{
					valueRW4.Scale = valueRW2.Scale;
				}
				if (teammateData.AdvanceSkillLevel > 0)
				{
					refRW = transformLookup.GetRefRW(effectsCollectorData2.Effect1);
					refRW.ValueRW.Position.y = valueRW3.Position.y;
				}
				refRW = transformLookup.GetRefRW(effectsCollectorData2.Effect2);
				refRW.ValueRW.Position = unit.beHitDir * unit.beHitCurrentOffsetAmount;
				angleMatLookup.GetRefRW(effectsCollectorData2.Effect3).ValueRW.Value = value2;
				angleMatLookup.GetRefRW(effectsCollectorData2.Effect4).ValueRW.Value = value2;
				hpRatioMatLookup.GetRefRW(effectsCollectorData2.Effect3).ValueRW.Value = value;
				hpRatioMatLookup.GetRefRW(effectsCollectorData2.Effect4).ValueRW.Value = value;
				Spell2004WallBuffer value3 = wallBuffer[i];
				value3.WallDir = DTool.RotateDir(dir3, 90f);
				value3.WallDistance = 0.83f * trans.Scale;
				wallBuffer[i] = value3;
				continue;
			}
			break;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2004PillarOfLightData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		BufferAccessor<Spell2004PillarBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell2004PillarBuffer_RW_BufferTypeHandle);
		BufferAccessor<Spell2004WallBuffer> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__Spell2004WallBuffer_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref LocalTransform trans = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, i);
				ref UnitProperty_Dots reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, i);
				ref Spell2004PillarOfLightData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr4, i);
				ref SpellMovementComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, i);
				DynamicBuffer<Spell2004PillarBuffer> pillarBuffer = bufferAccessor[i];
				DynamicBuffer<Spell2004WallBuffer> wallBuffer = bufferAccessor2[i];
				Execute(ref trans, ref teammateData, reference, ref spellData, reference2, in config, pillarBuffer, wallBuffer);
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
					ref LocalTransform trans2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, nextRangeBegin);
					ref UnitProperty_Dots reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, nextRangeBegin);
					ref Spell2004PillarOfLightData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellMovementComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, nextRangeBegin);
					DynamicBuffer<Spell2004PillarBuffer> pillarBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<Spell2004WallBuffer> wallBuffer2 = bufferAccessor2[nextRangeBegin];
					Execute(ref trans2, ref teammateData2, reference3, ref spellData2, reference4, in config2, pillarBuffer2, wallBuffer2);
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
				ref LocalTransform trans3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, j);
				ref UnitProperty_Dots reference5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, j);
				ref Spell2004PillarOfLightData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr4, j);
				ref SpellMovementComponentData reference6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, j);
				DynamicBuffer<Spell2004PillarBuffer> pillarBuffer3 = bufferAccessor[j];
				DynamicBuffer<Spell2004WallBuffer> wallBuffer3 = bufferAccessor2[j];
				Execute(ref trans3, ref teammateData3, reference5, ref spellData3, reference6, in config3, pillarBuffer3, wallBuffer3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref LocalTransform trans4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, k);
				ref UnitProperty_Dots reference7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, k);
				ref Spell2004PillarOfLightData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2004PillarOfLightData>(nativeArrayPtr4, k);
				ref SpellMovementComponentData reference8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, k);
				DynamicBuffer<Spell2004PillarBuffer> pillarBuffer4 = bufferAccessor[k];
				DynamicBuffer<Spell2004WallBuffer> wallBuffer4 = bufferAccessor2[k];
				Execute(ref trans4, ref teammateData4, reference7, ref spellData4, reference8, in config4, pillarBuffer4, wallBuffer4);
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
