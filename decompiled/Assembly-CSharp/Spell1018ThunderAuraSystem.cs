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

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell1018ThunderAuraSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	[WithNone(new Type[] { typeof(SpellFallTag) })]
	public struct Spell1018Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				public ComponentTypeHandle<Spell1018ThunderAuraData> __Spell1018ThunderAuraData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__Spell1018ThunderAuraData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1018ThunderAuraData>();
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__Spell1018ThunderAuraData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1018ThunderAuraData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithNone<SpellFallTag>();
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
			public void Run(ref Spell1018Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1018Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1018Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1018Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1018Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1018Job job, EntityManager entityManager)
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

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		public EntityCommandBuffer.ParallelWriter CMD;

		public GlobalRandom Random;

		public Entity ScreenShakeSingleton;

		public Entity SEPlayerSingleton;

		public DynamicOptimizeData OptimizeData;

		public Entity GlobalParticleSystemBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(SpellAspect spell, [ChunkIndexInQuery] int chunkIndex, ref Spell1018ThunderAuraData spellData)
		{
			ref SpellConfigComponentData valueRW = ref spell.Config.ValueRW;
			valueRW.DamageTimer += DeltaTime;
			if (!(valueRW.DamageTimer >= valueRW.DamageInterval))
			{
				return;
			}
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			ref readonly float3 position = ref spell.Transform.ValueRO.Position;
			float radius = spell.Config.ValueRO.Radius.Calculate();
			SpellTools.GetAttackableEntitiesInRange(in position, in radius, in spell.Config.ValueRO.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
			SpellTools.RemoveCannotAttackSpell(ref entities, spell.Config.ValueRO.ShooterType, in SpellConfigLookup);
			RemoveSameCampUnitAndNotAttackUnit(spell.Config.ValueRO.ShooterType, ref entities);
			if (entities.Length < valueRW.Int1)
			{
				GetLightningLineAttackTargets(ref entities, spell);
			}
			int num = math.min(valueRW.Int1, entities.Length);
			if (num > 0)
			{
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Hit";
				cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1018, in seName)));
			}
			SpellTools.GetSpellElementDataWithTimeScale(in spell.ElementEffect.ValueRO, in OptimizeData, out var result);
			for (int i = 0; i < num; i++)
			{
				Entity target = entities[i];
				if (TransformLookup.HasComponent(target))
				{
					TakeDamageInfo_Dots damage = spell.MakeDamageInfo(costPenetrate: false);
					damage.spell.HitPosition = TransformLookup[target].Position;
					if (num == 1)
					{
						damage.damage *= valueRW.Float1 / 100f;
					}
					damage.spell.ElementEffect = result;
					CMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
					float3 @float = new float3(0f, 0f, -0.3f);
					HitGlobalEffect(damage.spell.HitPosition + @float, in valueRW, chunkIndex);
					float duration = 0.3f;
					bool isFirstChain = false;
					float3 float2;
					float3 float3;
					if (i > 0)
					{
						Entity entity = entities[i - 1];
						float2 = TransformLookup[target].Position + @float;
						float3 = TransformLookup[entity].Position + @float;
					}
					else
					{
						float2 = TransformLookup[target].Position + new float3(0f, 0f, -15f) + @float;
						float3 = TransformLookup[target].Position + @float;
						isFirstChain = true;
					}
					valueRW.ColorType.ColorEnumToString(out var result2);
					Entity e = CMD.Instantiate(chunkIndex, SpellSingleton.Prefabs[$"1018_Chain_{result2}"]);
					CMD.SetComponent(chunkIndex, e, LocalTransform.FromPosition(float2 + (float3 - float2) / 2f));
					CMD.SetComponent(chunkIndex, e, new Spell1018ChainData
					{
						Position1 = float2,
						Position2 = float3,
						duration = duration,
						IsFirstChain = isFirstChain
					});
				}
			}
			valueRW.DamageTimer -= valueRW.DamageInterval + GetRandomDamageInterval(spell);
			if (num > 0)
			{
				CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
				{
					Radius = 0.025f,
					Speed = 1f,
					Time = 0.08f
				});
			}
		}

		[BurstCompile]
		private void HitGlobalEffect(float3 pos, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
		{
			config.ColorType.ColorEnumToString(out var result);
			float3 layerPosition = DTool.GetLayerPosition(in pos, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1018_ChainHit_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(pos) + layerPosition;
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}

		private float GetRandomDamageInterval(SpellAspect spell)
		{
			float num = spell.Config.ValueRO.DamageInterval * spell.Config.ValueRO.Float2;
			return Random.random.NextFloat(0f - num, num);
		}

		private void GetLightningLineAttackTargets(ref NativeList<Entity> startTargets, SpellAspect spell)
		{
			if (startTargets.Length == 0)
			{
				return;
			}
			int num = 0;
			while (startTargets.Length < spell.Config.ValueRO.Int1)
			{
				num++;
				if (num >= spell.Config.ValueRO.Int1)
				{
					break;
				}
				NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
				ref readonly float3 position = ref spell.Transform.ValueRO.Position;
				float radius = spell.Config.ValueRO.Radius.Calculate() * 1.5f;
				SpellTools.GetAttackableEntitiesInRange(in position, in radius, in spell.Config.ValueRO.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
				SpellTools.RemoveCannotAttackSpell(ref entities, spell.Config.ValueRO.ShooterType, in SpellConfigLookup);
				RemoveSameCampUnitAndNotAttackUnit(spell.Config.ValueRO.ShooterType, ref entities);
				for (int num2 = entities.Length - 1; num2 >= 0; num2--)
				{
					if (startTargets.Contains(entities[num2]))
					{
						entities.RemoveAt(num2);
					}
				}
				if (entities.Length != 0)
				{
					Entity value = entities[Random.random.NextInt(0, entities.Length)];
					startTargets.Add(in value);
					continue;
				}
				break;
			}
		}

		private void RemoveSameCampUnitAndNotAttackUnit(UnitType selfCamp, ref NativeList<Entity> entities)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				if (UnitPropertyLookup.TryGetComponent(entities[i], out var componentData))
				{
					UnitType unitType = componentData.unitCfg.unitType;
					if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness || DTool.IsSameCamp(componentData.unitCfg.unitType, selfCamp))
					{
						entities.RemoveAt(i);
						i--;
					}
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1018ThunderAuraData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					Execute(spell, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, i));
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
						SpellAspect spell2 = resolvedChunk[nextRangeBegin];
						Execute(spell2, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, nextRangeBegin));
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
					SpellAspect spell3 = resolvedChunk[j];
					Execute(spell3, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					SpellAspect spell4 = resolvedChunk[k];
					Execute(spell4, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, k));
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

	[BurstCompile]
	[CompilerGenerated]
	[WithAll(new Type[] { typeof(SpellFallTag) })]
	public struct Spell1018FallJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				public ComponentTypeHandle<Spell1018ThunderAuraData> __Spell1018ThunderAuraData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__Spell1018ThunderAuraData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1018ThunderAuraData>();
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__Spell1018ThunderAuraData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellFallTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1018ThunderAuraData>();
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
			public void Run(ref Spell1018FallJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1018FallJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1018FallJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1018FallJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1018FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1018FallJob job, EntityManager entityManager)
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

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[ReadOnly]
		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellRefractionData> SpellRefractLookUp;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public BufferLookup<SpellRefractionHitEntities> SpellRefractHitEntitiesLookUp;

		public EntityCommandBuffer.ParallelWriter CMD;

		public GlobalRandom Random;

		public Entity ScreenShakeSingleton;

		public Entity SEPlayerSingleton;

		public Entity spell1018FallExplosionBufferEntity;

		[ReadOnly]
		public PlayerController_Dots Player;

		public Entity GlobalParticleSystemBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(SpellAspect spell, [ChunkIndexInQuery] int chunkIndex, ref Spell1018ThunderAuraData data)
		{
			ref SpellConfigComponentData valueRW = ref spell.Config.ValueRW;
			if (spell.Movement.ValueRO.ReboundCount >= 0)
			{
				if (data.FallDelayTimer > 0.1f)
				{
					NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
					ref float3 fallPosition = ref data.FallPosition;
					float radius = valueRW.Radius.Calculate();
					SpellTools.GetAttackableEntitiesInRange(in fallPosition, in radius, in valueRW.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
					SpellTools.RemoveCannotAttackSpell(ref entities, valueRW.ShooterType, in SpellConfigLookup);
					RemoveSameCampUnitAndNotAttackUnit(valueRW.ShooterType, ref entities);
					ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
					Entity sEPlayerSingleton = SEPlayerSingleton;
					FixedString32Bytes seName = "Hit";
					cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1018, in seName)));
					CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
					{
						Radius = 0.025f,
						Speed = 1f,
						Time = 0.08f
					});
					int totalCount = math.min(valueRW.Int1, entities.Length);
					TryAttack(spell, chunkIndex, totalCount, entities, valueRW);
					float3 fallPosition2 = data.FallPosition;
					spell.Transform.ValueRW.Position = fallPosition2;
					if (!TryGetRefractionPos(ref entities, fallPosition2, spell.Entity, spell.Config.ValueRO.ShooterType, out var refractionPos))
					{
						CalcFallTargetPosition(ref spell.Movement.ValueRW, spell.Config.ValueRO, spell.Data.ValueRO, data.FallPosition, data.OriginalSpeed, out refractionPos, Random);
						spell.Movement.ValueRW.ReboundCount--;
					}
					data.FallPosition = refractionPos;
					spell.Movement.ValueRW.Direction = math.normalize(refractionPos - spell.Transform.ValueRW.Position);
					CMD.AppendToBuffer(chunkIndex, spell1018FallExplosionBufferEntity, new Spell1018FallExplosionBuffer
					{
						spellColorType = valueRW.ColorType,
						scale = valueRW.Radius.Calculate(),
						currentPosition = fallPosition2,
						nextPosition = data.FallPosition,
						isFinalBound = (spell.Movement.ValueRO.ReboundCount < 0)
					});
					data.FallDelayTimer = 0f;
				}
				else
				{
					data.FallDelayTimer += DeltaTime;
				}
			}
			else
			{
				CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, spell.Entity, value: true);
			}
		}

		private bool CanRefract(ref NativeList<Entity> hitList, UnitType shooterType)
		{
			foreach (Entity hit in hitList)
			{
				if (UnitPropertyLookup.TryGetComponent(hit, out var componentData))
				{
					UnitType unitType = componentData.unitCfg.unitType;
					if (unitType != UnitType.Brittleness && !DTool.IsSameCamp(shooterType, unitType))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool TryGetRefractionPos(ref NativeList<Entity> hitList, float3 startPosition, Entity spell, UnitType shooterType, out float3 refractionPos)
		{
			if (!SpellRefractLookUp.HasComponent(spell))
			{
				refractionPos = float3.zero;
				return false;
			}
			if (SpellRefractLookUp[spell].RemainCount > 0 && hitList.Length > 0 && CanRefract(ref hitList, shooterType))
			{
				NativeHashSet<Entity> ignoreEntities = new NativeHashSet<Entity>(hitList.Length, Allocator.Temp);
				if (SpellRefractHitEntitiesLookUp.TryGetBuffer(spell, out var bufferData))
				{
					foreach (Entity hit in hitList)
					{
						bufferData.Add(new SpellRefractionHitEntities
						{
							Entity = hit
						});
					}
					foreach (SpellRefractionHitEntities item in bufferData)
					{
						ignoreEntities.Add(item.Entity);
					}
				}
				Entity target;
				float3 targetPosition;
				UnitProperty_Dots targetPpt;
				bool flag = CurrentRoomEntities.FindReflectionTarget(startPosition, shooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
				if (!flag)
				{
					bufferData.Clear();
					ignoreEntities.Clear();
					foreach (Entity hit2 in hitList)
					{
						bufferData.Add(new SpellRefractionHitEntities
						{
							Entity = hit2
						});
						ignoreEntities.Add(hit2);
					}
					flag = CurrentRoomEntities.FindReflectionTarget(startPosition, shooterType, in ignoreEntities, out target, out targetPosition, out targetPpt);
				}
				if (flag)
				{
					SpellRefractLookUp.GetRefRW(spell).ValueRW.RemainCount--;
					refractionPos = targetPosition;
					ignoreEntities.Dispose();
					return true;
				}
				ignoreEntities.Dispose();
			}
			refractionPos = float3.zero;
			return false;
		}

		private void TryAttack(SpellAspect spell, int chunkIndex, int totalCount, NativeList<Entity> hitList, SpellConfigComponentData config)
		{
			for (int i = 0; i < totalCount; i++)
			{
				for (int j = 0; j < totalCount; j++)
				{
					Entity target = hitList[j];
					TakeDamageInfo_Dots damage = spell.MakeDamageInfo(costPenetrate: false);
					damage.damage = config.Damage.Calculate();
					if (totalCount == 1)
					{
						damage.damage *= config.Float1 / 100f;
					}
					damage.spell.HitPosition = TransformLookup[target].Position;
					CMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
					float3 @float = new float3(0f, 0f, -0.3f);
					HitGlobalEffect(damage.spell.HitPosition + @float, in config, chunkIndex);
					float duration = 0.3f;
					bool isFirstChain = false;
					float3 float2;
					float3 float3;
					if (j != i)
					{
						int index = ((j - 1 >= 0) ? (j - 1) : (totalCount - 1));
						Entity entity = hitList[index];
						float2 = TransformLookup[target].Position + @float;
						float3 = TransformLookup[entity].Position + @float;
					}
					else
					{
						float2 = TransformLookup[target].Position + new float3(0f, 0f, -15f) + @float;
						float3 = TransformLookup[target].Position + @float;
						isFirstChain = true;
					}
					config.ColorType.ColorEnumToString(out var result);
					Entity e = CMD.Instantiate(chunkIndex, SpellSingleton.Prefabs[$"1018_Chain_{result}"]);
					CMD.SetComponent(chunkIndex, e, LocalTransform.FromPosition(float2 + (float3 - float2) / 2f));
					CMD.SetComponent(chunkIndex, e, new Spell1018ChainData
					{
						Position1 = float2,
						Position2 = float3,
						duration = duration,
						IsFirstChain = isFirstChain
					});
				}
			}
		}

		[BurstCompile]
		private void HitGlobalEffect(float3 pos, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
		{
			config.ColorType.ColorEnumToString(out var result);
			float3 layerPosition = DTool.GetLayerPosition(in pos, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1018_ChainHit_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(pos) + layerPosition;
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}

		private void RemoveSameCampUnitAndNotAttackUnit(UnitType selfCamp, ref NativeList<Entity> entities)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				if (UnitPropertyLookup.TryGetComponent(entities[i], out var componentData))
				{
					UnitType unitType = componentData.unitCfg.unitType;
					if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness || DTool.IsSameCamp(componentData.unitCfg.unitType, selfCamp))
					{
						entities.RemoveAt(i);
						i--;
					}
				}
			}
		}

		private void CalcFallTargetPosition(ref SpellMovementComponentData movement, SpellConfigComponentData config, SpellComponentData data, float3 originalPosition, float originalSpeed, out float3 fallTargetPosition, GlobalRandom Random)
		{
			fallTargetPosition = originalPosition;
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
				fallTargetPosition = originalPosition + movement.Direction * config.Radius.Calculate();
				break;
			case SpellSpecialMovementType.ChaseEnemy:
			{
				GetValidChasePosition(ref movement, in data, in config, originalPosition, out var hasTarget, out var targetPosition);
				if (hasTarget)
				{
					float3 source = movement.Direction;
					float3 target = DTool.IgnoreZDir(in targetPosition, in originalPosition);
					float3 float5 = (movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, movement.ChaseRotateSpeed * 3f));
					fallTargetPosition = originalPosition + movement.Direction * config.Radius.Calculate();
				}
				else
				{
					fallTargetPosition = originalPosition + movement.Direction * config.Radius.Calculate();
				}
				break;
			}
			case SpellSpecialMovementType.ChaseMouse:
			{
				float3 point = Player.mousePosition;
				float3 float3 = ((DTool.IgnoreZDistance(in point, in originalPosition) <= 0.2f) ? DTool.GetDir(ref Random.random) : math.normalize(point - originalPosition));
				fallTargetPosition = originalPosition + float3 * config.Radius.Calculate();
				break;
			}
			case SpellSpecialMovementType.ChaseOwner:
			{
				float3 to = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
				float3 source = movement.Direction;
				float3 target = DTool.IgnoreZDir(in to, in originalPosition);
				float3 float2 = (movement.Direction = DTool.DirMoveTowardsIgnoreZ(in source, in target, movement.ChaseRotateSpeed * 3f));
				fallTargetPosition = originalPosition + movement.Direction * config.Radius.Calculate();
				break;
			}
			case SpellSpecialMovementType.Rotation:
			{
				GetRotationNextPos(ref movement, originalSpeed, out var nextPos);
				fallTargetPosition = nextPos;
				break;
			}
			}
		}

		private void GetRotationNextPos(ref SpellMovementComponentData movement, float originalSpeed, out float3 nextPos)
		{
			float num = 360f / (MathF.PI * 2f * movement.AroundRadius / (1f + originalSpeed * 0.02f));
			movement.AroundAngle += num;
			movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
			nextPos = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(TransformLookup);
		}

		private void GetValidChasePosition(ref SpellMovementComponentData movement, in SpellComponentData data, in SpellConfigComponentData config, float3 transform, out bool hasTarget, out float3 targetPosition)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.ChaseOwner:
				hasTarget = true;
				targetPosition = movement.UpdateSelfChasePosition(TransformLookup, data.Shooter);
				break;
			case SpellSpecialMovementType.ChaseEnemy:
				GetValidChaseEnemyPosition(ref movement, in config, transform, out hasTarget, out targetPosition);
				break;
			default:
				throw new Exception("法术弹道不是跟踪敌人或者跟踪施法者，不应该调用 GetValidChaseTarget");
			}
		}

		private void GetValidChaseEnemyPosition(ref SpellMovementComponentData movement, in SpellConfigComponentData config, float3 transform, out bool hasTarget, out float3 targetPosition)
		{
			hasTarget = false;
			targetPosition = float3.zero;
			if (TransformLookup.TryGetComponent(movement.ChaseTarget, out var componentData, out var entityExists) && entityExists)
			{
				targetPosition = componentData.Position;
				hasTarget = true;
			}
			else
			{
				hasTarget = CurrentRoomEntities.FindMinAngleTarget(transform, movement.Direction, config.ShooterType, out movement.ChaseTarget, out targetPosition, out var _);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1018ThunderAuraData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					Execute(spell, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, i));
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
						SpellAspect spell2 = resolvedChunk[nextRangeBegin];
						Execute(spell2, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, nextRangeBegin));
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
					SpellAspect spell3 = resolvedChunk[j];
					Execute(spell3, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					SpellAspect spell4 = resolvedChunk[k];
					Execute(spell4, chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1018ThunderAuraData>(nativeArrayPtr, k));
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
	private readonly struct IFE_991165285_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1018ThunderAuraData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1018ThunderAuraData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1018ThunderAuraData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1018ThunderAuraData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_991165285_0.TypeHandle __IFE_991165285_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<Spell1018ThunderAuraInitializeTag> __Spell1018ThunderAuraInitializeTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell1018Job.InternalCompilerQueryAndHandleData __Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public Spell1018FallJob.InternalCompilerQueryAndHandleData __Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_991165285_0_TypeHandle = new IFE_991165285_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell1018ThunderAuraInitializeTag_RW_ComponentLookup = state.GetComponentLookup<Spell1018ThunderAuraInitializeTag>();
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_991165285_0;

	private EntityQuery __query_991165285_1;

	private EntityQuery __query_991165285_2;

	private EntityQuery __query_991165285_3;

	private EntityQuery __query_991165285_4;

	private EntityQuery __query_991165285_5;

	private EntityQuery __query_991165285_6;

	private EntityQuery __query_991165285_7;

	private EntityQuery __query_991165285_8;

	private EntityQuery __query_991165285_9;

	private EntityQuery __query_991165285_10;

	private EntityQuery __query_991165285_11;

	private EntityQuery __query_991165285_12;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<Spell1018FallLightingBuffer>();
		state.RequireForUpdate<Spell1018FallExplosionBuffer>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<Spell1018ThunderAuraData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSingleton singleton = __query_991165285_1.GetSingleton<SpellSingleton>();
		DynamicBuffer<Spell1018FallLightingBuffer> singletonBuffer = __query_991165285_2.GetSingletonBuffer<Spell1018FallLightingBuffer>();
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item5 in IFE_991165285_0.Query(__query_991165285_0, __TypeHandle.__IFE_991165285_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1018ThunderAuraData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO = item3;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO2 = item4;
			Entity value = entity;
			if (!uncheckedRefRW2.ValueRO.IsFallSpell)
			{
				nativeList.Add(in value);
			}
			else
			{
				Spell1018FallLightingBuffer spell1018FallLightingBuffer = default(Spell1018FallLightingBuffer);
				spell1018FallLightingBuffer.spellColorType = uncheckedRefRO.ValueRO.ColorType;
				spell1018FallLightingBuffer.position = uncheckedRefRO2.ValueRO.Position;
				spell1018FallLightingBuffer.endPosition = uncheckedRefRW2.ValueRO.FallTargetPosition;
				Spell1018FallLightingBuffer elem = spell1018FallLightingBuffer;
				uncheckedRefRW.ValueRW.OriginalSpeed = uncheckedRefRW2.ValueRO.OriginalSpellHorizontalSpeed;
				uncheckedRefRW2.ValueRW.Speed = 0f;
				uncheckedRefRW2.ValueRW.CurrentFallSpeed = 0f;
				uncheckedRefRW2.ValueRW.OriginalSpellHorizontalSpeed = 0f;
				if (uncheckedRefRW2.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					float num = 360f / (MathF.PI * 2f * uncheckedRefRW2.ValueRO.AroundRadius / (1f + uncheckedRefRW2.ValueRO.OriginalSpellHorizontalSpeed * 0.02f));
					uncheckedRefRW2.ValueRW.AroundAngle += num;
					uncheckedRefRW2.ValueRW.Direction = Tool2D.GetDir(uncheckedRefRW2.ValueRO.AroundAngle + 90f);
					float3 @float = (elem.endPosition = uncheckedRefRW2.ValueRW.UpdateAroundFollowAndGetAroundPositionWhenAround(InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state)));
				}
				singletonBuffer.Add(elem);
				uncheckedRefRW.ValueRW.FallDelayTimer = 0f;
				uncheckedRefRW.ValueRW.FallPosition = elem.endPosition;
			}
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell1018ThunderAuraInitializeTag_RW_ComponentLookup, ref state, value, value: false);
		}
		foreach (Entity item6 in nativeList)
		{
			Entity Parent = item6;
			InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, Parent).ValueRO.ColorType.ColorEnumToString(out var result);
			EntityManager entityManager = state.EntityManager;
			SpellTools.SpawnChild(in singleton, in entityManager, 1018, "OnGround", result, in Parent, out var child);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, Parent).ValueRW.TrailEffectEntity = child;
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, LocalTransform.Identity, child);
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1018Job
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = __query_991165285_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			PhysicsWorld = __query_991165285_4.GetSingleton<PhysicsWorldSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			Random = __query_991165285_5.GetSingleton<GlobalRandom>(),
			SpellSingleton = singleton,
			ScreenShakeSingleton = __query_991165285_6.GetSingletonEntity(),
			SEPlayerSingleton = __query_991165285_7.GetSingletonEntity(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			OptimizeData = __query_991165285_8.GetSingleton<DynamicOptimizeData>(),
			GlobalParticleSystemBufferEntity = __query_991165285_9.GetSingletonEntity()
		}, __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Spell1018FallJob
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = __query_991165285_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			PhysicsWorld = __query_991165285_4.GetSingleton<PhysicsWorldSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			SpellSingleton = singleton,
			ScreenShakeSingleton = __query_991165285_6.GetSingletonEntity(),
			SEPlayerSingleton = __query_991165285_7.GetSingletonEntity(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			spell1018FallExplosionBufferEntity = __query_991165285_10.GetSingletonEntity(),
			Player = __query_991165285_11.GetSingleton<PlayerController_Dots>(),
			SpellRefractLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			SpellRefractHitEntitiesLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			CurrentRoomEntities = __query_991165285_12.GetSingleton<CurrentRoomEntitiesSingleton>(),
			GlobalParticleSystemBufferEntity = __query_991165285_9.GetSingletonEntity(),
			Random = __query_991165285_5.GetSingleton<GlobalRandom>()
		}, __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1018Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Spell1018FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1018ThunderAuraSystem_Spell1018FallJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1018ThunderAuraInitializeTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1018ThunderAuraData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		__query_991165285_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1018FallLightingBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1018FallExplosionBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_11 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_991165285_12 = entityQueryBuilder2.Build(ref state);
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
		((Spell1018ThunderAuraSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1018ThunderAuraSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1018ThunderAuraSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
