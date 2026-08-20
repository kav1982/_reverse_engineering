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

[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
[BurstCompile]
[CompilerGenerated]
public struct Spell2002LegsSystemJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public BufferTypeHandle<LegsData> __LegsData_RW_BufferTypeHandle;

			public BufferTypeHandle<LegsTarget> __LegsTarget_RW_BufferTypeHandle;

			public BufferTypeHandle<LegsAttackData> __LegsAttackData_RW_BufferTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<Spell2002Data> __Spell2002Data_RO_ComponentTypeHandle;

			public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RO_ComponentTypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<TeammateData> __TeammateData_RO_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__LegsData_RW_BufferTypeHandle = state.GetBufferTypeHandle<LegsData>();
				__LegsTarget_RW_BufferTypeHandle = state.GetBufferTypeHandle<LegsTarget>();
				__LegsAttackData_RW_BufferTypeHandle = state.GetBufferTypeHandle<LegsAttackData>();
				__Spell2002Data_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2002Data>(isReadOnly: true);
				__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
				__UnitBase_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>(isReadOnly: true);
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__TeammateData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
			}

			public void Update(ref SystemState state)
			{
				__LegsData_RW_BufferTypeHandle.Update(ref state);
				__LegsTarget_RW_BufferTypeHandle.Update(ref state);
				__LegsAttackData_RW_BufferTypeHandle.Update(ref state);
				__Spell2002Data_RO_ComponentTypeHandle.Update(ref state);
				__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RO_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RO_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell2002Data>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsTarget>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LegsAttackData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
		public void Run(ref Spell2002LegsSystemJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2002LegsSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2002LegsSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2002LegsSystemJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2002LegsSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2002LegsSystemJob job, EntityManager entityManager)
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

	public Entity TextFloatVFXBufferEtt;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

	[ReadOnly]
	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public const float cancelTargetRatio = 1.25f;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref DynamicBuffer<LegsData> legsBuffer, ref DynamicBuffer<LegsTarget> legsTargets, ref DynamicBuffer<LegsAttackData> legsAttack, in Spell2002Data spell2002Data, SpellAspect spellAspect, in PhysicsVelocity velocity, in UnitBase_Dots unitBase, ref UnitProperty_Dots ppt, in TeammateData teammateData, [ChunkIndexInQuery] int chunkIndex)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			for (int i = 0; i < legsBuffer.Length; i++)
			{
				LegsData value = legsBuffer[i];
				value.LegState = LegState.Idle;
				value.MoveToEndPoint = spellAspect.Transform.ValueRO.Position;
				value.CurrentEndPoint = spellAspect.Transform.ValueRO.Position;
				value.MoveToEndPoint = spellAspect.Transform.ValueRO.Position;
				legsBuffer[i] = value;
			}
		}
		else
		{
			legsAttack.Clear();
			UpdateLegsTarget(ref legsBuffer, ref legsTargets, spellAspect, spell2002Data);
			UpdateLegsStateMachine(ref legsAttack, ref legsBuffer, velocity, unitBase, ref legsTargets, ref ppt, spellAspect, teammateData, spell2002Data, chunkIndex);
		}
	}

	private void UpdateLegsTarget(ref DynamicBuffer<LegsData> legsBuffer, ref DynamicBuffer<LegsTarget> legsTargets, SpellAspect spellAspect, Spell2002Data spell2002Data)
	{
		for (int i = 0; i < legsBuffer.Length; i++)
		{
			LegsData legsData = legsBuffer[i];
			if (legsData.IsCantAttackLeg)
			{
				continue;
			}
			if (legsData.Target != Entity.Null)
			{
				if (LocalTransformLookUp.TryGetComponent(legsData.Target, out var componentData) && UnitPropertyLookup.TryGetComponent(legsData.Target, out var componentData2) && componentData2.CanBeTarget && !componentData2.IsInvincible)
				{
					float3 position = componentData.Position;
					if (math.distancesq(spellAspect.Transform.ValueRO.Position, position) > math.pow(spell2002Data.AttackRange * 1.25f, 2f))
					{
						CancelTarget(legsTargets, ref legsData);
						legsBuffer[i] = legsData;
					}
					else if (legsData.LegState == LegState.Move)
					{
						legsData.MoveToEndPoint = position;
						legsBuffer[i] = legsData;
					}
				}
				else
				{
					CancelTarget(legsTargets, ref legsData);
					legsBuffer[i] = legsData;
				}
				continue;
			}
			CurrentRoomEntities.FindValidTargetsInRange(spellAspect.Transform.ValueRO.Position, spell2002Data.AttackRange, UnitType.Teammate, out var target, out var _, out var _);
			foreach (Entity item in target)
			{
				if (IsEntityTarget(item, legsData, legsTargets))
				{
					continue;
				}
				if (UnitPropertyLookup.HasComponent(item))
				{
					RefRW<UnitProperty_Dots> refRW = UnitPropertyLookup.GetRefRW(item);
					if (!refRW.ValueRW.CanBeTarget || refRW.ValueRW.IsInvincible)
					{
						continue;
					}
				}
				legsData.LegState = LegState.Move;
				legsData.Target = item;
				legsData.MoveBeforeEndPoint = legsData.CurrentEndPoint;
				legsData.MoveToEndPoint = LocalTransformLookUp[item].Position;
				legsTargets.Add(new LegsTarget
				{
					Status = LegsTargetStatus.Locked,
					Target = item,
					AttackedFuseHeadLegIndex = legsData.FuseHeadIndex
				});
				legsBuffer[i] = legsData;
				break;
			}
		}
	}

	private void CancelTarget(DynamicBuffer<LegsTarget> legsTargets, ref LegsData legsData)
	{
		for (int i = 0; i < legsTargets.Length; i++)
		{
			LegsTarget legsTarget = legsTargets[i];
			if (legsData.Target == legsTarget.Target)
			{
				legsTargets.RemoveAt(i);
				break;
			}
		}
		legsData.LegState = LegState.Idle;
		legsData.Target = Entity.Null;
	}

	private bool IsEntityTarget(Entity entity, LegsData legsData, DynamicBuffer<LegsTarget> legsTargets)
	{
		foreach (LegsTarget item in legsTargets)
		{
			if (entity == item.Target && legsData.FuseHeadIndex == item.AttackedFuseHeadLegIndex)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateLegsStateMachine(ref DynamicBuffer<LegsAttackData> legsAttack, ref DynamicBuffer<LegsData> legsBuffer, PhysicsVelocity velocity, UnitBase_Dots unitBase, ref DynamicBuffer<LegsTarget> legsTargets, ref UnitProperty_Dots ppt, SpellAspect spellAspect, TeammateData teammateData, Spell2002Data spell2002Data, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < legsBuffer.Length; i++)
		{
			LegsData legsData = legsBuffer[i];
			switch (legsData.LegState)
			{
			case LegState.Idle:
			{
				float num2 = spellAspect.Config.ValueRO.Radius.Calculate();
				float3 position = spellAspect.Transform.ValueRO.Position;
				float3 @float = position + legsData.Dir * num2;
				if (math.lengthsq(@float - legsData.CurrentEndPoint) > math.pow(num2 * legsData.LegRadiusRatio.result, 2f))
				{
					legsData.LegState = LegState.Move;
					if (math.lengthsq(unitBase.currentMotion) > math.lengthsq(velocity.Linear))
					{
						legsData.MoveToEndPoint = @float + math.normalizesafe(unitBase.currentMotion) * num2 * legsData.LegRadiusRatio.RandomResult(ref Random.random);
					}
					else
					{
						legsData.MoveToEndPoint = @float + math.normalizesafe(velocity.Linear) * num2 * legsData.LegRadiusRatio.RandomResult(ref Random.random);
					}
					AdjustLegsMoveEndPoint(position, ref legsData);
					legsData.MoveBeforeEndPoint = legsData.CurrentEndPoint;
				}
				break;
			}
			case LegState.Move:
				UpdateLegsPosition(velocity, ppt, teammateData, ref legsData);
				if (!DTool.IsEqual(in legsData.CurrentEndPoint, in legsData.MoveToEndPoint))
				{
					break;
				}
				if (legsData.Target != Entity.Null)
				{
					for (int j = 0; j < legsTargets.Length; j++)
					{
						LegsTarget value = legsTargets[j];
						if (legsData.Target == value.Target)
						{
							value.Status = LegsTargetStatus.Attacked;
							legsTargets[j] = value;
							legsData.LegState = LegState.Attack;
							break;
						}
					}
				}
				else
				{
					legsData.LegState = LegState.Idle;
				}
				break;
			case LegState.Attack:
			{
				float3 hitPosition = (legsData.MoveToEndPoint = (legsData.CurrentEndPoint = LocalTransformLookUp[legsData.Target].Position + new float3(0f, 0.5f, 0f)));
				legsData.AttackTimer -= DeltaTime * teammateData.TeammateSpeedRatio;
				if (legsData.AttackTimer <= 0f)
				{
					legsData.AttackTimer = 0.35f;
					legsAttack.Add(new LegsAttackData
					{
						AttackType = LegsAttackType.Suck,
						LegIndex = i
					});
					TakeDamageInfo_Dots damage = spellAspect.MakeDamageInfo(costPenetrate: false);
					float num = 2.857143f;
					float x = spellAspect.Config.ValueRO.Damage.Calculate() / num;
					damage.damage = math.ceil(x);
					damage.spell.HitPosition = hitPosition;
					CMD.TryAttackEntity(chunkIndex, in legsData.Target, in damage, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: false);
					RecoverHp(ref ppt, chunkIndex, spellAspect.Config.ValueRO.Float2, ref spellAspect.Transform.ValueRW);
				}
				break;
			}
			}
			legsBuffer[i] = legsData;
		}
	}

	private void AdjustLegsMoveEndPoint(float3 headPos, ref LegsData legsData)
	{
		if (DTool.RaycastWallHit(headPos.IgnoreZ(), legsData.MoveToEndPoint.IgnoreZ(), out var hitResult, in PhysicsWorld))
		{
			float3 input = hitResult.Position;
			legsData.MoveToEndPoint = input.IgnoreZ();
		}
	}

	private void RecoverHp(ref UnitProperty_Dots teammatePpt, int chunkIndex, float healAmount, ref LocalTransform localTrans)
	{
		if (!(teammatePpt.unitCfg.currentHP >= teammatePpt.unitCfg.maxHP))
		{
			teammatePpt.unitCfg.currentHP = math.min(teammatePpt.unitCfg.currentHP + healAmount, teammatePpt.unitCfg.maxHP);
			CMD.AppendToBuffer(chunkIndex, TextFloatVFXBufferEtt, new TextFloatVFXBED
			{
				number = healAmount,
				type = UITextFloatType.Recover,
				worldPos = localTrans.Position
			});
		}
	}

	private void UpdateLegsPosition(PhysicsVelocity velocity, UnitProperty_Dots ppt, TeammateData teammateData, ref LegsData legsData)
	{
		float num = math.abs(ppt.unitCfg.moveSpeed);
		if (math.lengthsq(velocity.Linear) > num * num)
		{
			num = math.length(velocity.Linear);
		}
		legsData.CurrentEndPoint = DTool.MoveTowards(in legsData.CurrentEndPoint, in legsData.MoveToEndPoint, num * 2.2f * teammateData.TeammateSpeedRatio * DeltaTime);
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		BufferAccessor<LegsData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__LegsData_RW_BufferTypeHandle);
		BufferAccessor<LegsTarget> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__LegsTarget_RW_BufferTypeHandle);
		BufferAccessor<LegsAttackData> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__LegsAttackData_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Spell2002Data_RO_ComponentTypeHandle);
		SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__TeammateData_RO_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				DynamicBuffer<LegsData> legsBuffer = bufferAccessor[i];
				DynamicBuffer<LegsTarget> legsTargets = bufferAccessor2[i];
				DynamicBuffer<LegsAttackData> legsAttack = bufferAccessor3[i];
				ref Spell2002Data spell2002Data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, i);
				SpellAspect spellAspect = resolvedChunk[i];
				Execute(ref legsBuffer, ref legsTargets, ref legsAttack, in spell2002Data, spellAspect, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr5, i), chunkIndexInQuery);
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
					DynamicBuffer<LegsData> legsBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<LegsTarget> legsTargets2 = bufferAccessor2[nextRangeBegin];
					DynamicBuffer<LegsAttackData> legsAttack2 = bufferAccessor3[nextRangeBegin];
					ref Spell2002Data spell2002Data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, nextRangeBegin);
					SpellAspect spellAspect2 = resolvedChunk[nextRangeBegin];
					Execute(ref legsBuffer2, ref legsTargets2, ref legsAttack2, in spell2002Data2, spellAspect2, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr5, nextRangeBegin), chunkIndexInQuery);
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
				DynamicBuffer<LegsData> legsBuffer3 = bufferAccessor[j];
				DynamicBuffer<LegsTarget> legsTargets3 = bufferAccessor2[j];
				DynamicBuffer<LegsAttackData> legsAttack3 = bufferAccessor3[j];
				ref Spell2002Data spell2002Data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, j);
				SpellAspect spellAspect3 = resolvedChunk[j];
				Execute(ref legsBuffer3, ref legsTargets3, ref legsAttack3, in spell2002Data3, spellAspect3, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr5, j), chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				DynamicBuffer<LegsData> legsBuffer4 = bufferAccessor[k];
				DynamicBuffer<LegsTarget> legsTargets4 = bufferAccessor2[k];
				DynamicBuffer<LegsAttackData> legsAttack4 = bufferAccessor3[k];
				ref Spell2002Data spell2002Data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2002Data>(nativeArrayPtr, k);
				SpellAspect spellAspect4 = resolvedChunk[k];
				Execute(ref legsBuffer4, ref legsTargets4, ref legsAttack4, in spell2002Data4, spellAspect4, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr3, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr5, k), chunkIndexInQuery);
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
