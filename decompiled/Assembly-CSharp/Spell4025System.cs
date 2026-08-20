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

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell4025System : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Spell4025Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell4025RuneSlashData> __Spell4025RuneSlashData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell4025RuneSlashData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4025RuneSlashData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				}

				public void Update(ref SystemState state)
				{
					__Spell4025RuneSlashData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4025RuneSlashData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
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
			public void Run(ref Spell4025Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4025Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4025Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4025Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4025Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4025Job job, EntityManager entityManager)
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

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookUp;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellSplitComponentData> SpellSplitLookup;

		public EntityCommandBuffer.ParallelWriter CMD;

		[ReadOnly]
		public PhysicsWorldSingleton Physics;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		public DynamicOptimizeData DynamicOptimizeData;

		public Entity GlobalParticleEmitBufferEntity;

		public GlobalRandom Random;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		public Entity ShootSpellBufferEntity;

		public Entity UnfollowRequireEntity;

		public Entity EffectEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spell4025RuneSlashData data, Entity entity, ref LocalTransform transform, ref SpellConfigComponentData config, SpellMovementComponentData movement, ref SpellComponentData spellData)
		{
			float num = 0.32f;
			SpellSplitLookup.TryGetComponent(entity, out var componentData, out var entityExists);
			config.ColorType.ColorEnumToString(out var result);
			if (!data.IsInitialize)
			{
				data.IsInitialize = true;
				config.Damage.MulRatio *= 1f + config.Float3;
				config.Damage.Base = 15f + config.Float1 * (float)config.Int1;
				if (movement.IsFallSpell)
				{
					CMD.AppendToBuffer(0, EffectEntity, new SpellEffectSystem.Require
					{
						Settings = SpellSingleton.Effects[4025]["FallSpellTrail"],
						Entity = entity,
						Color = result,
						SpellId = 4025
					});
				}
			}
			if (spellData.IsSplitSpell)
			{
				num = Random.random.NextFloat(0.24f, 0.4f);
			}
			FixedString32Bytes fixedString32Bytes = "ASP";
			if (movement.IsFallSpell)
			{
				return;
			}
			if (data.NeedSpawnAOESlash && config.DurationTimer >= num)
			{
				data.NeedSpawnAOESlash = false;
				config.ColorType.ColorEnumToString(out var result2);
				float radius = config.Radius.CalculateWithNewBaseValue(config.Radius.Base + 2.4f);
				NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
				SpellTools.GetAttackableEntitiesInRange(in transform.Position, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in Physics, ref entities);
				FixedString32Bytes fixedString32Bytes2 = $"4025_AOESlash_{result2}";
				foreach (Entity item in entities)
				{
					Entity target = item;
					LocalTransform transform2 = LocalTransformLookUp[entity];
					SpellElementEffectComponentData elementEffect = SpellElementLookup[entity];
					TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform2, in elementEffect, in spellData, out var info);
					CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: false);
					if (LocalTransformLookUp.TryGetComponent(target, out var componentData2, out var entityExists2) && entityExists2)
					{
						fixedString32Bytes2 = $"4025_AOEHit_{result2}";
						float3 @float = DTool.IgnoreZPosition(in componentData2.Position);
						float3 position = Random.random.NextFloat3Direction();
						float3 float2 = DTool.IgnoreZPosition(in position);
						GlobalParticleEmitParams element = new GlobalParticleEmitParams(GlobalParticleType.Spell, fixedString32Bytes2, @float + new float3(0f, 0.3f, 0f) + float2 * 0.2f)
						{
							Size = Random.random.NextFloat(0.7f, 1.3f)
						};
						CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element);
					}
				}
				fixedString32Bytes2 = $"4025_AOESlash_{result2}";
				GlobalParticleEmitParams element2 = new GlobalParticleEmitParams(GlobalParticleType.Spell, fixedString32Bytes2, transform.Position + new float3(0f, 0.3f, 0.3f))
				{
					Size = radius
				};
				CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element2);
				fixedString32Bytes = "ASP";
				CMD.AppendToBuffer(chunkIndex, UnfollowRequireEntity, new SpellEffectSystem.UnfollowingRequire
				{
					SpellId = 4025,
					Color = result,
					Scale = radius,
					StartPosition = transform.Position,
					Settings = new SpellEffect
					{
						Name = fixedString32Bytes,
						Layer = LayerCorrectType.GroundEffect,
						DestroyDelay = 1f
					}
				});
			}
			if (entityExists && componentData.Count > 0 && !data.IsSpawnSplitSlash && config.Duration.Calculate() - config.DurationTimer <= DeltaTime)
			{
				data.IsSpawnSplitSlash = true;
				NativeList<Entity> entities2 = new NativeList<Entity>(Allocator.Temp);
				ref float3 position2 = ref transform.Position;
				float radius2 = math.max(4f, config.Radius.CalculateWithNewBaseValue(4f));
				SpellTools.GetAttackableEntitiesInRange(in position2, in radius2, in config.ShooterType, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in Physics, ref entities2);
				int num2 = (int)math.ceil((float)componentData.Count / DynamicOptimizeData.GetLowFrameDamageIntervalTimeScale(60f, 10f, 7f));
				float splitPower = (float)componentData.Count / (float)num2;
				LocalTransform componentData3 = default(LocalTransform);
				bool entityExists3 = default(bool);
				for (int i = 0; i < num2; i++)
				{
					float3 spawnPosition;
					if (entities2.Length > i && LocalTransformLookUp.TryGetComponent(entities2[i], out componentData3, out entityExists3) && entityExists3)
					{
						spawnPosition = DTool.IgnoreZPosition(in componentData3.Position, transform.Position.z);
					}
					else
					{
						int num3 = ((config.Float2 > 0f) ? 3 : 0);
						float3 position = transform.Position + DTool.GetDir(ref Random.random) * ((float)num3 + config.Radius.CalculateWithNewBaseValue(5f)) * math.pow(Random.random.NextFloat(0f, 1f), 1f);
						spawnPosition = DTool.IgnoreZPosition(in position, transform.Position.z);
					}
					SpellSpawnParams element3 = SpellSingleton.SpellSpawnParamsStorage[entity].BuildRedRuneSlash(spawnPosition, splitPower);
					CMD.AppendToBuffer(chunkIndex, ShootSpellBufferEntity, element3);
				}
				spellData.DisableSplitEffect = true;
			}
			if (data.IsSlashDone)
			{
				return;
			}
			data.IsSlashDone = true;
			data.NeedSpawnAOESlash = config.Int2 >= 4 && config.Float2 > 0f;
			if (data.NeedSpawnAOESlash)
			{
				config.Duration.Extra += 0.4f;
			}
			CurrentRoomEntities.FindNearestTarget(transform.Position, UnitType.Player, out var target2, out var targetPosition, out var _);
			if (DTool.IgnoreZDistance(in targetPosition, in transform.Position) < 0.8f)
			{
				LocalTransform transform2 = LocalTransformLookUp[entity];
				SpellElementEffectComponentData elementEffect = SpellElementLookup[entity];
				TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform2, in elementEffect, in spellData, out var info2);
				CMD.TryAttackEntity(chunkIndex, in target2, in info2, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: false);
				if (LocalTransformLookUp.TryGetComponent(target2, out var componentData4, out var entityExists4) && entityExists4)
				{
					config.ColorType.ColorEnumToString(out var result3);
					string text = $"4025_Hit_{result3}";
					float3 float3 = DTool.IgnoreZPosition(in componentData4.Position);
					float3 position = Random.random.NextFloat3Direction();
					float3 float4 = DTool.IgnoreZPosition(in position);
					GlobalParticleEmitParams element4 = new GlobalParticleEmitParams(GlobalParticleType.Spell, text, float3 + new float3(0f, 0.3f, 0f) + float4 * 0.2f)
					{
						Size = Random.random.NextFloat(1f, 1.6f)
					};
					CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element4);
				}
			}
			bool num4 = config.Float3 > 0f || config.Float2 > 0f;
			FixedString32Bytes name = $"4025_Slash_{result}";
			float num5 = (spellData.IsSplitSpell ? 0.85f : 1.2f);
			num5 *= math.clamp(0.8f + (float)config.Int1 * 0.01f, 0f, 1f);
			if (config.Float2 > 0f)
			{
				num5 *= config.Radius.CalculateWithNewBaseValue(1.45f);
			}
			GlobalParticleEmitParams element5 = new GlobalParticleEmitParams(GlobalParticleType.Spell, name, transform.Position + new float3(0f, 0.3f, 0f))
			{
				Size = num5
			};
			if (!data.NeedSpawnAOESlash)
			{
				CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element5);
				fixedString32Bytes = "SP";
				CMD.AppendToBuffer(chunkIndex, UnfollowRequireEntity, new SpellEffectSystem.UnfollowingRequire
				{
					SpellId = 4025,
					Color = result,
					Scale = num5,
					StartPosition = transform.Position + new float3(0f, 0.3f, 0f),
					Settings = new SpellEffect
					{
						Name = fixedString32Bytes,
						Layer = LayerCorrectType.Coordinate,
						DestroyDelay = 1f
					}
				});
			}
			if (num4)
			{
				name = $"4025_SuperSlash_{result}";
				element5 = new GlobalParticleEmitParams(GlobalParticleType.Spell, name, transform.Position + new float3(0f, 0.3f, 0f))
				{
					Size = num5 + 0.3f
				};
				CMD.AppendToBuffer(chunkIndex, GlobalParticleEmitBufferEntity, element5);
				CMD.AppendToBuffer(chunkIndex, UnfollowRequireEntity, new SpellEffectSystem.UnfollowingRequire
				{
					SpellId = 4025,
					Color = result,
					Scale = num5,
					StartPosition = transform.Position + new float3(0f, 0.3f, 0f),
					Settings = new SpellEffect
					{
						Name = "SSP",
						Layer = LayerCorrectType.Coordinate,
						DestroyDelay = 1f
					}
				});
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4025RuneSlashData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell4025RuneSlashData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4025RuneSlashData>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
					ref SpellMovementComponentData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, i);
					Execute(spellData: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i), chunkIndex: chunkIndexInQuery, data: ref data, entity: entity, transform: ref transform, config: ref config, movement: reference);
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
						ref Spell4025RuneSlashData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4025RuneSlashData>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellMovementComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, nextRangeBegin);
						Execute(spellData: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin), chunkIndex: chunkIndexInQuery, data: ref data2, entity: entity2, transform: ref transform2, config: ref config2, movement: reference2);
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
					ref Spell4025RuneSlashData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4025RuneSlashData>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
					ref SpellMovementComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, j);
					Execute(spellData: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j), chunkIndex: chunkIndexInQuery, data: ref data3, entity: entity3, transform: ref transform3, config: ref config3, movement: reference3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell4025RuneSlashData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4025RuneSlashData>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
					ref SpellMovementComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr5, k);
					Execute(spellData: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k), chunkIndex: chunkIndexInQuery, data: ref data4, entity: entity4, transform: ref transform4, config: ref config4, movement: reference4);
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
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentLookup;

		public Spell4025Job.InternalCompilerQueryAndHandleData __Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellElementEffectComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>();
			__SpellSplitComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>();
			__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007B0E_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007B0E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007B0E_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00007B0F_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007B0F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007B0F_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1546309095_0;

	private EntityQuery __query_1546309095_1;

	private EntityQuery __query_1546309095_2;

	private EntityQuery __query_1546309095_3;

	private EntityQuery __query_1546309095_4;

	private EntityQuery __query_1546309095_5;

	private EntityQuery __query_1546309095_6;

	private EntityQuery __query_1546309095_7;

	private EntityQuery __query_1546309095_8;

	private EntityQuery __query_1546309095_9;

	private EntityQuery __query_1546309095_10;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Spell4025RuneSlashData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4025Job
		{
			CMD = __query_1546309095_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			Random = __query_1546309095_1.GetSingleton<GlobalRandom>(),
			SpellSingleton = __query_1546309095_2.GetSingleton<SpellSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			Physics = __query_1546309095_3.GetSingleton<PhysicsWorldSingleton>(),
			SpellElementLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_1546309095_4.GetSingleton<CurrentRoomEntitiesSingleton>(),
			GlobalParticleEmitBufferEntity = __query_1546309095_5.GetSingletonEntity(),
			SpellSplitLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellSplitComponentData_RW_ComponentLookup, ref state),
			DynamicOptimizeData = __query_1546309095_6.GetSingleton<DynamicOptimizeData>(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime * __query_1546309095_7.GetSingletonRW<DynamicOptimizeData>().ValueRO.LastFrameTimeScale,
			ShootSpellBufferEntity = __query_1546309095_8.GetSingletonEntity(),
			UnfollowRequireEntity = __query_1546309095_9.GetSingletonEntity(),
			EffectEntity = __query_1546309095_10.GetSingletonEntity()
		}, __TypeHandle.__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4025Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4025System_Spell4025Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1546309095_10 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00007B0E_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007B0F_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4025System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4025System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4025System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
