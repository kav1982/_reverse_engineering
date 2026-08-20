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

[CompilerGenerated]
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[BurstCompile]
public struct Spell2002EssenceLegsSystemJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public BufferTypeHandle<EssenceLegsData> __EssenceLegsData_RW_BufferTypeHandle;

			public BufferTypeHandle<EssenceLegAttackedEntity> __EssenceLegAttackedEntity_RW_BufferTypeHandle;

			public BufferTypeHandle<FuseHeadEntity> __FuseHeadEntity_RW_BufferTypeHandle;

			public ComponentTypeHandle<AnimationCurveData> __AnimationCurveData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TeammateData> __TeammateData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<Spell2002Data> __Spell2002Data_RW_ComponentTypeHandle;

			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__EssenceLegsData_RW_BufferTypeHandle = state.GetBufferTypeHandle<EssenceLegsData>();
				__EssenceLegAttackedEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<EssenceLegAttackedEntity>();
				__FuseHeadEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<FuseHeadEntity>();
				__AnimationCurveData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimationCurveData>();
				__TeammateData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
				__Spell2002Data_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2002Data>();
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
			}

			public void Update(ref SystemState state)
			{
				__EssenceLegsData_RW_BufferTypeHandle.Update(ref state);
				__EssenceLegAttackedEntity_RW_BufferTypeHandle.Update(ref state);
				__FuseHeadEntity_RW_BufferTypeHandle.Update(ref state);
				__AnimationCurveData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RO_ComponentTypeHandle.Update(ref state);
				__Spell2002Data_RW_ComponentTypeHandle.Update(ref state);
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegsData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EssenceLegAttackedEntity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<FuseHeadEntity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<AnimationCurveData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2002Data>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
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
		public void Run(ref Spell2002EssenceLegsSystemJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2002EssenceLegsSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2002EssenceLegsSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2002EssenceLegsSystemJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2002EssenceLegsSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2002EssenceLegsSystemJob job, EntityManager entityManager)
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

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	public Entity GlobalParticleEmitBufferEntity;

	public bool IsSafeMode;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref DynamicBuffer<EssenceLegsData> essenceLegsData, ref DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities, DynamicBuffer<FuseHeadEntity> fuseHeadBuffer, AnimationCurveData animationCurve, in TeammateData teammateData, Spell2002Data spell2002Data, SpellAspect spellAspect, [ChunkIndexInQuery] int chunkIndex)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			for (int i = 0; i < essenceLegsData.Length; i++)
			{
				EssenceLegsData legsData = essenceLegsData[i];
				UpdateLegsMove(spell2002Data, spellAspect.Transform.ValueRO, fuseHeadBuffer, ref legsData, i, isHoldByTeammate6: true);
				essenceLegsData[i] = legsData;
			}
			return;
		}
		for (int j = 0; j < essenceLegsData.Length; j++)
		{
			EssenceLegsData legsData2 = essenceLegsData[j];
			UpdateLegsMove(spell2002Data, spellAspect.Transform.ValueRO, fuseHeadBuffer, ref legsData2, j);
			UpdateLegsAttackDamage(essenceLegAttackedEntities, animationCurve, spell2002Data, spellAspect, chunkIndex, ref legsData2, spellAspect.Entity);
			essenceLegsData[j] = legsData2;
		}
	}

	private void UpdateLegsAttackDamage(DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities, AnimationCurveData animationCurve, Spell2002Data spell2002Data, SpellAspect spellAspect, int chunkIndex, ref EssenceLegsData legsData, Entity entity)
	{
		if (!legsData.IsAttacking)
		{
			return;
		}
		NativeList<ColliderCastHit> hitList = new NativeList<ColliderCastHit>(Allocator.Temp);
		ref float3 endPoint = ref legsData.EndPoint;
		float3 rayEnd = legsData.EndPoint + legsData.AttackDir;
		float width = 0.5f;
		UnitType selfCamp = UnitType.Player;
		SpellTools.GetAttackableEntitiesInSphereCast(in endPoint, in rayEnd, in width, in selfCamp, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList);
		foreach (ColliderCastHit item in hitList)
		{
			bool flag = false;
			for (int i = 0; i < essenceLegAttackedEntities.Length; i++)
			{
				if (!(essenceLegAttackedEntities[i].Entity != item.Entity))
				{
					flag = true;
					break;
				}
			}
			if (!flag && LocalTransformLookUp.HasComponent(item.Entity))
			{
				essenceLegAttackedEntities.Add(new EssenceLegAttackedEntity
				{
					Entity = item.Entity
				});
				TakeDamageInfo_Dots damage = spellAspect.MakeDamageInfo(costPenetrate: false);
				damage.damage = spellAspect.Config.ValueRO.Damage.Calculate() * spell2002Data.EssenceDamageRatio;
				damage.spell.HitPosition = LocalTransformLookUp[item.Entity].Position;
				damage.damageRecordId = 3120;
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity target = item.Entity;
				cMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
				spellAspect.Config.ValueRO.ColorType.ColorEnumToString(out var result);
				float value = math.min(LocalTransformLookUp[entity].Scale, 3f);
				if (IsSafeMode)
				{
					result = $"Safe{result}";
				}
				GlobalParticleEmitParams element = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"2002_Hit_{result}", damage.spell.HitPosition + new float3(0f, 0.2f, 0f))
				{
					Size = value,
					Velocity = legsData.AttackDir
				};
				CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element);
			}
		}
		legsData.EssenceAttackTimer += DeltaTime;
		float valueToClamp = legsData.EssenceAttackTimer / 0.25f;
		valueToClamp = math.clamp(valueToClamp, 0f, 1f);
		legsData.LegLerpValue = animationCurve.Curve1.Evaluate(valueToClamp);
		if (valueToClamp >= 0.45f && !legsData.StabBack)
		{
			legsData.ResetEssenceLegAttackData(ref Random, chunkIndex);
		}
		if (legsData.EssenceAttackTimer / legsData.EssenceAttackDuration >= 1f)
		{
			legsData.IsAttacking = false;
			legsData.EssenceAttackTimer -= legsData.EssenceAttackDuration;
			essenceLegAttackedEntities.Clear();
		}
	}

	private void UpdateLegsMove(Spell2002Data spell2002Data, LocalTransform transform, DynamicBuffer<FuseHeadEntity> fuseHeadBuffer, ref EssenceLegsData legsData, int legIndex, bool isHoldByTeammate6 = false)
	{
		if (isHoldByTeammate6)
		{
			legsData.CurrentEndPoint = transform.Position;
			legsData.MiddlePoint = transform.Position;
			legsData.EndPoint = transform.Position;
			legsData.AttackTarget = transform.Position;
			legsData.IdleLookPoint = transform.Position;
			legsData.HeadPos = transform.Position;
			return;
		}
		legsData.LegsFloatTimer += DeltaTime;
		int num = ((legIndex % 2 == 0) ? 1 : (-1));
		float legLerpValue = legsData.LegLerpValue;
		float t = 0.2f;
		float3 @float = (legsData.HeadPos = (legsData.IsFuseLeg ? fuseHeadBuffer[legsData.FuseHeadIndex].HeadPos : spell2002Data.MainHeadPos));
		legsData.IdleLookPoint = math.lerp(legsData.IdleLookPoint, @float + spell2002Data.Direction * 2f, t);
		if (spell2002Data.EssenceLockTarget != Entity.Null)
		{
			if (LocalTransformLookUp.TryGetComponent(spell2002Data.EssenceLockTarget, out var componentData))
			{
				legsData.CurrentEndPoint = math.lerp(legsData.CurrentEndPoint, componentData.Position, t);
			}
		}
		else
		{
			legsData.CurrentEndPoint = math.lerp(legsData.CurrentEndPoint, legsData.IdleLookPoint, t);
		}
		float3 float2 = @float;
		float num2 = math.sin(legsData.LegsFloatTimer * 240f * (MathF.PI / 180f)) * 4f;
		if (num2 <= 0f)
		{
			num2 *= 0.5f;
		}
		float3 currentEndPoint = legsData.CurrentEndPoint;
		float3 oldDir = math.normalizesafe(currentEndPoint - float2);
		float3 float3 = float2 + DTool.GetDir(in oldDir, (legsData.StabMiddlePointAngle + num2) * (float)num) * legsData.StabMiddlePointDistance;
		float3 oldDir2 = math.normalizesafe(currentEndPoint - float3);
		float3 float4 = float3 + DTool.GetDir(in oldDir2, 0f - num2) * legsData.StabEndPointDistance;
		float3 end = float3 + math.normalizesafe(float4 - float3) * (math.distance(currentEndPoint, float3) + legsData.OverStabDistance - legsData.StabEndPointDistance);
		float3 float5 = currentEndPoint + math.normalizesafe(currentEndPoint - float3) * legsData.OverStabDistance;
		legsData.MiddlePoint = math.lerp(float3, end, legLerpValue);
		legsData.EndPoint = math.lerp(float4, float5, legLerpValue);
		legsData.AttackTarget = float5;
		legsData.AttackDir = math.normalizesafe(float5 - float4);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		BufferAccessor<EssenceLegsData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__EssenceLegsData_RW_BufferTypeHandle);
		BufferAccessor<EssenceLegAttackedEntity> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__EssenceLegAttackedEntity_RW_BufferTypeHandle);
		BufferAccessor<FuseHeadEntity> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__FuseHeadEntity_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__AnimationCurveData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TeammateData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2002Data_RW_ComponentTypeHandle);
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				DynamicBuffer<EssenceLegsData> essenceLegsData = bufferAccessor[i];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities = bufferAccessor2[i];
				DynamicBuffer<FuseHeadEntity> fuseHeadBuffer = bufferAccessor3[i];
				ref AnimationCurveData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, i);
				ref Spell2002Data reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr3, i);
				SpellAspect spellAspect = resolvedChunk[i];
				Execute(ref essenceLegsData, ref essenceLegAttackedEntities, fuseHeadBuffer, reference, in teammateData, reference2, spellAspect, chunkIndexInQuery);
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
					DynamicBuffer<EssenceLegsData> essenceLegsData2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities2 = bufferAccessor2[nextRangeBegin];
					DynamicBuffer<FuseHeadEntity> fuseHeadBuffer2 = bufferAccessor3[nextRangeBegin];
					ref AnimationCurveData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, nextRangeBegin);
					ref Spell2002Data reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr3, nextRangeBegin);
					SpellAspect spellAspect2 = resolvedChunk[nextRangeBegin];
					Execute(ref essenceLegsData2, ref essenceLegAttackedEntities2, fuseHeadBuffer2, reference3, in teammateData2, reference4, spellAspect2, chunkIndexInQuery);
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
				DynamicBuffer<EssenceLegsData> essenceLegsData3 = bufferAccessor[j];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities3 = bufferAccessor2[j];
				DynamicBuffer<FuseHeadEntity> fuseHeadBuffer3 = bufferAccessor3[j];
				ref AnimationCurveData reference5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, j);
				ref Spell2002Data reference6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr3, j);
				SpellAspect spellAspect3 = resolvedChunk[j];
				Execute(ref essenceLegsData3, ref essenceLegAttackedEntities3, fuseHeadBuffer3, reference5, in teammateData3, reference6, spellAspect3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				DynamicBuffer<EssenceLegsData> essenceLegsData4 = bufferAccessor[k];
				DynamicBuffer<EssenceLegAttackedEntity> essenceLegAttackedEntities4 = bufferAccessor2[k];
				DynamicBuffer<FuseHeadEntity> fuseHeadBuffer4 = bufferAccessor3[k];
				ref AnimationCurveData reference7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AnimationCurveData>(nativeArrayPtr, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, k);
				ref Spell2002Data reference8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr3, k);
				SpellAspect spellAspect4 = resolvedChunk[k];
				Execute(ref essenceLegsData4, ref essenceLegAttackedEntities4, fuseHeadBuffer4, reference7, in teammateData4, reference8, spellAspect4, chunkIndexInQuery);
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
