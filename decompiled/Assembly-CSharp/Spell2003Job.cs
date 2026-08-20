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

[BurstCompile]
[CompilerGenerated]
[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
public struct Spell2003Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell2003TentacleData> __Spell2003TentacleData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RW_ComponentTypeHandle;

			public BufferTypeHandle<Spell2003TentacleEffectData> __Spell2003TentacleEffectData_RW_BufferTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell2003TentacleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2003TentacleData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSpeedRatioValueData>();
				__Spell2003TentacleEffectData_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell2003TentacleEffectData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
			}

			public void Update(ref SystemState state)
			{
				__Spell2003TentacleData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__TeammateData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				__SpellSpeedRatioValueData_RW_ComponentTypeHandle.Update(ref state);
				__Spell2003TentacleEffectData_RW_BufferTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2003TentacleData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellSpeedRatioValueData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2003TentacleEffectData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
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
		public void Run(ref Spell2003Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell2003Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell2003Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell2003Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell2003Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell2003Job job, EntityManager entityManager)
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
	public ComponentLookup<MatOverrideFrameIndex> FrameAnimeLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	public CurrentRoomEntitiesSingleton CurrentRoomEntities;

	public EntityCommandBuffer.ParallelWriter CMD;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectsCollectorLookup;

	[ReadOnly]
	public SpellSingleton SpellSingleton;

	public Entity SEPlayerSingleton;

	public GlobalRandom Random;

	public Entity InvisibleTentacleSpawnerEntity;

	public Entity SplitTentacleSpawnerEntity;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellSplitComponentData> SplitSpellLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<PhysicsCollider> ColliderLookUp;

	public Entity SpellEffectEntity;

	public Entity TeammateGhostEffectEntity;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<TeammateData> TeammateDataLookup;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	public Entity Spell3101Buffer;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	private void Execute(ref Spell2003TentacleData tentacleData, ref SpellComponentData spellData, ref SpellMovementComponentData movement, ref TeammateData teammateData, ref PhysicsVelocity velocity, ref LocalTransform localTransform, [ChunkIndexInQuery] int chunkIndex, ref UnitBase_Dots unitBase, Entity entity, ref SpellConfigComponentData config, ref UnitProperty_Dots unitPpt, SpellSpeedRatioValueData spellSpeedRatioValueData, ref DynamicBuffer<Spell2003TentacleEffectData> tentacleEffectData, ref SpellElementEffectComponentData elementEffect)
	{
		if (teammateData.IsHoldByTeammate6)
		{
			return;
		}
		bool flag = config.ColorType == SpellColorType.Fire;
		if (config.DurationTimer >= tentacleData.LifeDuration)
		{
			CMD.TeammateDeadTryActiveTeammateDelayDeathEffect(ref unitPpt, ref TeammateDataLookup, entity, SpellEffectEntity, chunkIndex, ColliderLookUp, TeammateGhostEffectEntity);
			config.Duration.Base = float.PositiveInfinity;
		}
		float num = teammateData.TeammateSpeedRatio + (float)teammateData.AdvanceSkillLevel * 20f / 100f;
		float num2 = DeltaTime * num;
		UpdateChaseTarget(ref tentacleData, ref movement, entity);
		UpdateTargetLastFramePosition(ref tentacleData, ref movement);
		switch (tentacleData.State)
		{
		case Spell2003State.Initialize:
			movement.Speed = 0f;
			velocity.Linear = float3.zero;
			tentacleData.AttackCoolDownTime = 2f / (float)(teammateData.TeammateCurrentFuseLevel + 1) / (num + 20f * (float)teammateData.AdvanceSkillLevel / 100f);
			tentacleData.AttackCoolDownTimer = tentacleData.AttackCoolDownTime / 2f;
			tentacleData.ChainTentacleAccountRequirement = 4 - teammateData.AdvanceSkillLevel;
			tentacleData.State = Spell2003State.Idle;
			break;
		case Spell2003State.Idle:
		{
			if (tentacleEffectData.Length <= 0)
			{
				break;
			}
			if (LocalTransformLookUp.TryGetComponent(movement.ChaseTarget, out var _, out var entityExists) && entityExists)
			{
				if (tentacleData.AttackCoolDownTimer < tentacleData.AttackCoolDownTime)
				{
					tentacleData.AttackCoolDownTimer += DeltaTime;
				}
				else
				{
					Spell2003TentacleEffectData value = tentacleEffectData[tentacleData.CurrentAttackingTentacleIndex];
					value.StartAttack = true;
					tentacleEffectData[tentacleData.CurrentAttackingTentacleIndex] = value;
					HideTargetAnimaEntity(tentacleEffectData[tentacleData.CurrentAttackingTentacleIndex].IdleEffectEntity, flag);
					ResetTargetAnimaEntityFrame(tentacleEffectData[tentacleData.CurrentAttackingTentacleIndex].AttackEffectEntity, flag);
					tentacleData.CurrentAttackingTentacleIndex++;
					tentacleData.AttackCoolDownTimer -= tentacleData.AttackCoolDownTime;
					if (tentacleData.CurrentAttackingTentacleIndex > teammateData.TeammateCurrentFuseLevel)
					{
						tentacleData.CurrentAttackingTentacleIndex = 0;
					}
				}
			}
			else
			{
				CurrentRoomEntities.FindNearestTarget(LocalTransformLookUp[entity].Position, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
			}
			for (int i = 0; i < tentacleEffectData.Length; i++)
			{
				Spell2003TentacleEffectData value2 = tentacleEffectData[i];
				if (!value2.StartAttack)
				{
					continue;
				}
				FrameAnimeLookUp.GetRefRW(value2.AttackEffectEntity).ValueRW.FrameIndex = value2.AttackTimer * 16f;
				if (flag)
				{
					FrameAnimeLookUp.GetRefRW(GetFireOutlineEntity(value2.AttackEffectEntity)).ValueRW.FrameIndex = value2.AttackTimer * 16f;
				}
				if (value2.AttackTimer < 1f)
				{
					if (value2.AttackTimer > 0.5f && value2.AttackingHoldTimer > 0f)
					{
						value2.AttackingHoldTimer -= num2;
						if (value2.AttackingHoldTimer < 0f)
						{
							value2.AttackTimer -= value2.AttackingHoldTimer;
						}
					}
					else
					{
						value2.AttackTimer += num2 * 2f;
					}
					if (!value2.AttackFinished && value2.AttackTimer > 0.5f)
					{
						value2.AttackFinished = true;
						ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
						Entity sEPlayerSingleton = SEPlayerSingleton;
						FixedString32Bytes seName = "Attack";
						cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(2003, in seName), SEPlayMode.Replay, 3, 0.1f));
						if (teammateData.AdvanceSkillLevel > 0 && tentacleData.CurrentAttackAccount >= tentacleData.ChainTentacleAccountRequirement)
						{
							float3 direction = DTool.IgnoreZDir(in tentacleData.TargetLastFramePosition, in localTransform.Position);
							float speed = (40f + spellSpeedRatioValueData.Speed.AddBase) * (spellSpeedRatioValueData.Speed.AddRatio + 1f * spellSpeedRatioValueData.Speed.MulRatio);
							CMD.AppendToBuffer(chunkIndex, InvisibleTentacleSpawnerEntity, new Teammate3InvisibleTentacleSpawnerData
							{
								CurrentPosition = localTransform.Position,
								Direction = direction,
								Speed = speed,
								RemainDuration = 1.75f,
								Shooter = entity,
								SpawnTimer = 0f,
								Target = movement.ChaseTarget,
								RotateAngle = Random.random.NextFloat(0f, 360f)
							});
							tentacleData.CurrentAttackAccount = 0;
						}
						else
						{
							if (teammateData.AdvanceSkillLevel > 0)
							{
								tentacleData.CurrentAttackAccount++;
							}
							LocalTransform transform = LocalTransformLookUp[entity];
							SpellElementEffectComponentData elementEffect2 = SpellElementLookup[entity];
							TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in elementEffect2, in spellData, out var info);
							info.spell.CostPenetrate = false;
							info.spell.CostRefraction = false;
							info.damage = config.Damage.Calculate();
							CMD.TryAttackEntity(chunkIndex, in movement.ChaseTarget, in info, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: false);
							float3 targetPosition = Random.random.NextFloat3Direction();
							float3 @float = DTool.IgnoreZPosition(in targetPosition);
							float num3 = Random.random.NextFloat(0f, 0.15f);
							float3 position = tentacleData.TargetLastFramePosition + @float * num3;
							ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
							ref SpellSingleton spellSingleton = ref SpellSingleton;
							seName = "Spike";
							cMD2.CreateSpellEffect(chunkIndex, in spellSingleton, in spellData, in config, in position, in seName, 1f, in float3.zero);
							ref EntityCommandBuffer.ParallelWriter cMD3 = ref CMD;
							seName = "SpikeHit";
							FixedString32Bytes colorName = "General";
							cMD3.CreateSpecificSpellEffect(0, in seName, in colorName, in SpellSingleton, in config, in position, in float3.zero);
							CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, position, UnitPropertyLookup, Physics, in config, in movement, in localTransform, in elementEffect, in spellData, entity);
							if (SplitSpellLookUp.HasComponent(entity))
							{
								CMD.AppendToBuffer(chunkIndex, SplitTentacleSpawnerEntity, new Teammate3SplitTentacleSpawnerData
								{
									TargetPosition = position,
									Shooter = entity,
									SpawnDelayTimer = 0.5f,
									SplitCount = SplitSpellLookUp[entity].Count
								});
							}
						}
					}
				}
				else
				{
					value2.AttackFinished = false;
					value2.AttackTimer = 0f;
					value2.StartAttack = false;
					value2.AttackingHoldTimer = 0.4f;
					ResetTargetAnimaEntityFrame(tentacleEffectData[i].IdleEffectEntity, flag);
					HideTargetAnimaEntity(tentacleEffectData[i].AttackEffectEntity, flag);
				}
				tentacleEffectData[i] = value2;
			}
			break;
		}
		}
		foreach (Spell2003TentacleEffectData tentacleEffectDatum in tentacleEffectData)
		{
			ref LocalTransform valueRW = ref LocalTransformLookUp.GetRefRW(tentacleEffectDatum.EffectEntity).ValueRW;
			DTool.SetLocalTransformLayerPosition(in localTransform, ref valueRW, LayerCorrectType.Coordinate);
			valueRW.Position += new float3((float)(-teammateData.TeammateCurrentFuseLevel) / 2f * 0.2f + (float)tentacleEffectDatum.TentacleIndex * 0.2f, 0f, -0.001f * (float)tentacleEffectDatum.TentacleIndex);
		}
	}

	private void UpdateChaseTarget(ref Spell2003TentacleData tentacleData, ref SpellMovementComponentData movement, Entity entity, bool forceUpdate = false)
	{
		tentacleData.RecheckTargetTimer += DeltaTime;
		if (tentacleData.RecheckTargetTimer >= 0.2f || !LocalTransformLookUp.HasComponent(movement.ChaseTarget) || forceUpdate)
		{
			CurrentRoomEntities.FindRandomTarget(Random, UnitType.Teammate, out movement.ChaseTarget, out var _, out var _);
			UpdateTargetLastFramePosition(ref tentacleData, ref movement);
			tentacleData.RecheckTargetTimer = 0f;
		}
	}

	private void UpdateTargetLastFramePosition(ref Spell2003TentacleData tentacleData, ref SpellMovementComponentData movement)
	{
		if (LocalTransformLookUp.HasComponent(movement.ChaseTarget))
		{
			tentacleData.TargetLastFramePosition = LocalTransformLookUp[movement.ChaseTarget].Position;
		}
	}

	private void ResetTargetAnimaEntityFrame(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = 0f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(GetFireOutlineEntity(targetEntity)).ValueRW.FrameIndex = 0f;
		}
	}

	private void HideTargetAnimaEntity(Entity targetEntity, bool isFireColor)
	{
		FrameAnimeLookUp.GetRefRW(targetEntity).ValueRW.FrameIndex = -0.1f;
		if (isFireColor)
		{
			FrameAnimeLookUp.GetRefRW(GetFireOutlineEntity(targetEntity)).ValueRW.FrameIndex = -0.1f;
		}
	}

	private Entity GetFireOutlineEntity(Entity targetEntity)
	{
		return EffectsCollectorLookup[targetEntity].Effect1;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2003TentacleData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr10 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr11 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellSpeedRatioValueData_RW_ComponentTypeHandle);
		BufferAccessor<Spell2003TentacleEffectData> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell2003TentacleEffectData_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr12 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell2003TentacleData tentacleData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2003TentacleData>(nativeArrayPtr, i);
				ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
				ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, i);
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr5, i);
				ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, i);
				ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, i);
				ref UnitProperty_Dots unitPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr10, i);
				ref SpellSpeedRatioValueData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, i);
				DynamicBuffer<Spell2003TentacleEffectData> tentacleEffectData = bufferAccessor[i];
				Execute(elementEffect: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr12, i), tentacleData: ref tentacleData, spellData: ref spellData, movement: ref movement, teammateData: ref teammateData, velocity: ref velocity, localTransform: ref localTransform, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase, entity: entity, config: ref config, unitPpt: ref unitPpt, spellSpeedRatioValueData: reference, tentacleEffectData: ref tentacleEffectData);
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
					ref Spell2003TentacleData tentacleData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2003TentacleData>(nativeArrayPtr, nextRangeBegin);
					ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, nextRangeBegin);
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr5, nextRangeBegin);
					ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, nextRangeBegin);
					ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, nextRangeBegin);
					ref UnitProperty_Dots unitPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr10, nextRangeBegin);
					ref SpellSpeedRatioValueData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, nextRangeBegin);
					DynamicBuffer<Spell2003TentacleEffectData> tentacleEffectData2 = bufferAccessor[nextRangeBegin];
					Execute(elementEffect: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr12, nextRangeBegin), tentacleData: ref tentacleData2, spellData: ref spellData2, movement: ref movement2, teammateData: ref teammateData2, velocity: ref velocity2, localTransform: ref localTransform2, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase2, entity: entity2, config: ref config2, unitPpt: ref unitPpt2, spellSpeedRatioValueData: reference2, tentacleEffectData: ref tentacleEffectData2);
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
				ref Spell2003TentacleData tentacleData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2003TentacleData>(nativeArrayPtr, j);
				ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
				ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, j);
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr5, j);
				ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, j);
				ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, j);
				ref UnitProperty_Dots unitPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr10, j);
				ref SpellSpeedRatioValueData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, j);
				DynamicBuffer<Spell2003TentacleEffectData> tentacleEffectData3 = bufferAccessor[j];
				Execute(elementEffect: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr12, j), tentacleData: ref tentacleData3, spellData: ref spellData3, movement: ref movement3, teammateData: ref teammateData3, velocity: ref velocity3, localTransform: ref localTransform3, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase3, entity: entity3, config: ref config3, unitPpt: ref unitPpt3, spellSpeedRatioValueData: reference3, tentacleEffectData: ref tentacleEffectData3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell2003TentacleData tentacleData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2003TentacleData>(nativeArrayPtr, k);
				ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
				ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr4, k);
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr5, k);
				ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr6, k);
				ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr9, k);
				ref UnitProperty_Dots unitPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr10, k);
				ref SpellSpeedRatioValueData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSpeedRatioValueData>(nativeArrayPtr11, k);
				DynamicBuffer<Spell2003TentacleEffectData> tentacleEffectData4 = bufferAccessor[k];
				Execute(elementEffect: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr12, k), tentacleData: ref tentacleData4, spellData: ref spellData4, movement: ref movement4, teammateData: ref teammateData4, velocity: ref velocity4, localTransform: ref localTransform4, chunkIndex: chunkIndexInQuery, unitBase: ref unitBase4, entity: entity4, config: ref config4, unitPpt: ref unitPpt4, spellSpeedRatioValueData: reference4, tentacleEffectData: ref tentacleEffectData4);
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
