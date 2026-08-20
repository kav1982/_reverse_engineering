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

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell2007SuicideBugNestSystem : ISystem, ISystemCompilerGenerated
{
	private struct Spell2007Effect
	{
		public Entity SpellEntity;

		public FixedString32Bytes ColorName;

		public float Scale;

		public float3 Position;

		public float3 OffSet;
	}

	[WithDisabled(new Type[] { typeof(TeammateDeadTag) })]
	[WithNone(new Type[] { typeof(Spell2007SuicideBugNestInitializedTag) })]
	[CompilerGenerated]
	private struct Spell2007Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public BufferTypeHandle<Spell2007FuseBuffer> __Spell2007FuseBuffer_RW_BufferTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell2007SuicideBugNestData> __Spell2007SuicideBugNestData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Spell2007FuseBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell2007FuseBuffer>();
					__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__Spell2007SuicideBugNestData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2007SuicideBugNestData>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell2007FuseBuffer_RW_BufferTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell2007SuicideBugNestData_RW_ComponentTypeHandle.Update(ref state);
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
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateDeadTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2007FuseBuffer>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2007SuicideBugNestData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell2007SuicideBugNestInitializedTag>();
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
			public void Run(ref Spell2007Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell2007Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell2007Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell2007Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell2007Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell2007Job job, EntityManager entityManager)
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
		public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<PostTransformMatrix> MatrixLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> ConfigLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity ScreenShakeEntity;

		public Entity GlobalParticleEntity;

		public Entity SEPlayerEntity;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int chunkIndex, ref LocalTransform transform, ref TeammateData teammate, ref SpellMovementComponentData spellMovement, DynamicBuffer<Spell2007FuseBuffer> buffers, in SpellElementEffectComponentData element, ref SpellConfigComponentData config, ref SpellComponentData data, ref Spell2007SuicideBugNestData spell, ref UnitProperty_Dots property, Entity entity)
		{
			float teammateSpeedRatio = teammate.TeammateSpeedRatio;
			RefRW<PostTransformMatrix> refRW;
			switch (spell.AnimType)
			{
			case Spell2007SuicideBugNestData.Spell2007AnimType.Idle:
				spell.SpawnTimer += DeltaTime * teammateSpeedRatio;
				if (spell.CurrentAnimTimer <= 1f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					float x6 = 1f - spell.CurrentAnimTimer * 0.1f;
					float y6 = 1f + spell.CurrentAnimTimer * 0.1f;
					foreach (Spell2007FuseBuffer item in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(x6, y6, 1f));
					}
				}
				else if (spell.CurrentAnimTimer <= 2f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					float x7 = 1f - (2f - spell.CurrentAnimTimer) * 0.1f;
					float y7 = 1f + (2f - spell.CurrentAnimTimer) * 0.1f;
					foreach (Spell2007FuseBuffer item2 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item2.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(x7, y7, 1f));
					}
				}
				else
				{
					foreach (Spell2007FuseBuffer item3 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item3.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(1f, 1f, 1f));
					}
					spell.CurrentAnimTimer = 0f;
				}
				if ((double)spell.SpawnTimer >= (double)config.Float2 - 0.3)
				{
					spell.CurrentAnimTimer = 0f;
					spell.SpawnTimer = -0.45f;
					spell.AnimType = Spell2007SuicideBugNestData.Spell2007AnimType.Attack;
				}
				break;
			case Spell2007SuicideBugNestData.Spell2007AnimType.Attack:
				if (spell.CurrentAnimTimer <= 0.15f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					foreach (Spell2007FuseBuffer item4 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item4.Entity].Effect1);
						ref PostTransformMatrix valueRW = ref refRW.ValueRW;
						float x3 = DTool.Lerp(valueRW.Value.Scale().x, 0.7f, spell.CurrentAnimTimer * 6.67f);
						float y3 = DTool.Lerp(valueRW.Value.Scale().y, 1.4f, spell.CurrentAnimTimer * 6.67f);
						valueRW.Value = Matrix4x4.Scale((Vector3)new float3(x3, y3, 1f));
					}
					break;
				}
				if (spell.CurrentAnimTimer <= 0.3f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					foreach (Spell2007FuseBuffer item5 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item5.Entity].Effect1);
						ref PostTransformMatrix valueRW2 = ref refRW.ValueRW;
						float x4 = DTool.Lerp(0.7f, 2.1f, (spell.CurrentAnimTimer - 0.15f) * 6.67f);
						float y4 = DTool.Lerp(1.4f, 0.2f, (spell.CurrentAnimTimer - 0.15f) * 6.67f);
						valueRW2.Value = Matrix4x4.Scale((Vector3)new float3(x4, y4, 1f));
					}
					if (!(spell.CurrentAnimTimer > 0.3f))
					{
						break;
					}
					foreach (Spell2007FuseBuffer item6 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item6.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(2.1f, 0.2f, 1f));
					}
					if (!teammate.IsFuseMaterial)
					{
						TakeDamageInfo_Dots element2 = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						element2.damage = property.unitCfg.maxHP * 0.06f;
						CMD.AppendToBuffer(chunkIndex, entity, element2);
					}
					break;
				}
				if (spell.CurrentAnimTimer <= 0.45f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					float x5 = DTool.Lerp(2.1f, 1f, (spell.CurrentAnimTimer - 0.3f) * 6.67f);
					float y5 = DTool.Lerp(0.2f, 1f, (spell.CurrentAnimTimer - 0.3f) * 6.67f);
					foreach (Spell2007FuseBuffer item7 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item7.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(x5, y5, 1f));
					}
					break;
				}
				foreach (Spell2007FuseBuffer item8 in buffers)
				{
					refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item8.Entity].Effect1);
					refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(1f, 1f, 1f));
				}
				spell.CurrentAnimTimer = 0f;
				spell.AnimType = Spell2007SuicideBugNestData.Spell2007AnimType.Idle;
				break;
			case Spell2007SuicideBugNestData.Spell2007AnimType.Landing:
				if (spell.positionZ < -0.0001f)
				{
					float num = (36f + spellMovement.Speed) * teammate.TeammateSpeedRatio * (1f + (float)(teammate.TeammateCurrentFuseLevel - 1) * 0.2f);
					spell.positionZ += num * DeltaTime;
					if (spell.positionZ > 0f)
					{
						spell.positionZ = 0f;
					}
					break;
				}
				if (spell.CurrentAnimTimer <= 0.1f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					float x = DTool.Lerp(1f, 2f, spell.CurrentAnimTimer * 10f);
					float y = DTool.Lerp(1f, 0.33f, spell.CurrentAnimTimer * 10f);
					foreach (Spell2007FuseBuffer item9 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item9.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(x, y, 1f));
					}
					if (!(spell.CurrentAnimTimer > 0.1f))
					{
						break;
					}
					NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
					float radius = 3.5f * (config.Radius.AddRatio + 1f) * config.Radius.MulRatio;
					float damage = property.unitCfg.maxHP * 180f / 100f * (float)teammate.AdvanceSkillLevel * (config.Damage.AddRatio + 1f) * config.Damage.MulRatio + config.Damage.Extra;
					SpellTools.GetAttackableEntitiesInRange(in transform.Position, in radius, in config.ShooterType, containsBrittleness: true, in UnitLookup, in ConfigLookup, in PhysicsWorld, ref entities);
					TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in spellMovement, in transform, in element, in data, out var info);
					CMD.AppendToBuffer(chunkIndex, ScreenShakeEntity, new ScreenShakeData
					{
						Radius = 0.1f,
						Speed = 10f,
						Time = 0.2f
					});
					config.ColorType.ColorEnumToString(out var result);
					CMD.AppendToBuffer(chunkIndex, GlobalParticleEntity, new GlobalParticleEmitParams
					{
						Type = GlobalParticleType.Spell,
						Position = transform.Position + new float3(0f, 0f, 0.1f),
						Size = radius,
						Name = $"2007_SpawnBomb_{result}"
					});
					CMD.AppendToBuffer(chunkIndex, SEPlayerEntity, new SEData("SE_Teammate7_EssenceExplosion"));
					foreach (Entity item10 in entities)
					{
						Entity target = item10;
						info.spell.HitPosition = transform.Position;
						info.damage = damage;
						info.damageRecordId = 3120;
						CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitLookup, in ConfigLookup);
					}
					break;
				}
				if (spell.CurrentAnimTimer <= 0.2f)
				{
					spell.CurrentAnimTimer += teammateSpeedRatio * DeltaTime;
					float x2 = DTool.Lerp(2f, 1f, (spell.CurrentAnimTimer - 0.1f) * 10f);
					float y2 = DTool.Lerp(0.33f, 1f, (spell.CurrentAnimTimer - 0.1f) * 10f);
					foreach (Spell2007FuseBuffer item11 in buffers)
					{
						refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item11.Entity].Effect1);
						refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(x2, y2, 1f));
					}
					break;
				}
				foreach (Spell2007FuseBuffer item12 in buffers)
				{
					refRW = MatrixLookup.GetRefRW(EffectsCollectorLookUp[item12.Entity].Effect1);
					refRW.ValueRW.Value = Matrix4x4.Scale((Vector3)new float3(1f, 1f, 1f));
				}
				spell.CurrentAnimTimer = 0f;
				spell.AnimType = Spell2007SuicideBugNestData.Spell2007AnimType.Idle;
				break;
			}
			float z = transform.Position.z;
			transform.Position.z = spell.positionZ;
			foreach (Spell2007FuseBuffer item13 in buffers)
			{
				ref LocalTransform valueRW3 = ref TransformLookup.GetRefRW(item13.Entity).ValueRW;
				DTool.SetLocalTransformLayerPosition(in transform, ref valueRW3, LayerCorrectType.Coordinate);
				valueRW3.Position += item13.Offset + new float3(0f, 0f, spell.positionZ);
			}
			transform.Position.z = z;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			BufferAccessor<Spell2007FuseBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell2007FuseBuffer_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2007SuicideBugNestData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
					ref TeammateData teammate = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, i);
					ref SpellMovementComponentData spellMovement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i);
					DynamicBuffer<Spell2007FuseBuffer> buffers = bufferAccessor[i];
					ref SpellElementEffectComponentData element = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr4, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i);
					ref Spell2007SuicideBugNestData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr7, i);
					ref UnitProperty_Dots property = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, i);
					Execute(chunkIndexInQuery, ref transform, ref teammate, ref spellMovement, buffers, in element, ref config, ref data, ref spell, ref property, entity);
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
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, nextRangeBegin);
						ref TeammateData teammate2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellMovementComponentData spellMovement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin);
						DynamicBuffer<Spell2007FuseBuffer> buffers2 = bufferAccessor[nextRangeBegin];
						ref SpellElementEffectComponentData element2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref Spell2007SuicideBugNestData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr7, nextRangeBegin);
						ref UnitProperty_Dots property2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, nextRangeBegin);
						Execute(chunkIndexInQuery, ref transform2, ref teammate2, ref spellMovement2, buffers2, in element2, ref config2, ref data2, ref spell2, ref property2, entity2);
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
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, j);
					ref TeammateData teammate3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, j);
					ref SpellMovementComponentData spellMovement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j);
					DynamicBuffer<Spell2007FuseBuffer> buffers3 = bufferAccessor[j];
					ref SpellElementEffectComponentData element3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr4, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j);
					ref Spell2007SuicideBugNestData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr7, j);
					ref UnitProperty_Dots property3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, j);
					Execute(chunkIndexInQuery, ref transform3, ref teammate3, ref spellMovement3, buffers3, in element3, ref config3, ref data3, ref spell3, ref property3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, k);
					ref TeammateData teammate4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, k);
					ref SpellMovementComponentData spellMovement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k);
					DynamicBuffer<Spell2007FuseBuffer> buffers4 = bufferAccessor[k];
					ref SpellElementEffectComponentData element4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr4, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k);
					ref Spell2007SuicideBugNestData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr7, k);
					ref UnitProperty_Dots property4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, k);
					Execute(chunkIndexInQuery, ref transform4, ref teammate4, ref spellMovement4, buffers4, in element4, ref config4, ref data4, ref spell4, ref property4, entity4);
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
	private readonly struct IFE_393132632_0
	{
		public struct ResolvedChunk
		{
			public EnabledMask item1_EnabledMask;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr item7_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<EnabledRefRO<Spell2007SuicideBugNestInitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<EnabledRefRO<Spell2007SuicideBugNestInitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>(item1_EnabledMask.GetEnabledRefRO<Spell2007SuicideBugNestInitializedTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2007SuicideBugNestData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell2007SuicideBugNestInitializedTag> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<Spell2007SuicideBugNestData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitProperty_Dots> item4_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellMovementComponentData> item6_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item7_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2007SuicideBugNestInitializedTag>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2007SuicideBugNestData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item7_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				item7_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_EnabledMask = archetypeChunk.GetEnabledMask(ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.item7_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item7_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<EnabledRefRO<Spell2007SuicideBugNestInitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<EnabledRefRO<Spell2007SuicideBugNestInitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell2007SuicideBugNestInitializedTag>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell2007SuicideBugNestData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_393132632_0.TypeHandle __IFE_393132632_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<TeammateData> __TeammateData_RO_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PlayerController_Dots> __PlayerController_Dots_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		public ComponentLookup<MatOverrideFuseProgress> __MatOverrideFuseProgress_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		public Spell2007Job.InternalCompilerQueryAndHandleData __Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_393132632_0_TypeHandle = new IFE_393132632_0.TypeHandle(ref state);
			__TeammateDeadTag_RO_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>(isReadOnly: true);
			__TeammateData_RO_ComponentLookup = state.GetComponentLookup<TeammateData>(isReadOnly: true);
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__PlayerController_Dots_RO_ComponentLookup = state.GetComponentLookup<PlayerController_Dots>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__MatOverrideFuseProgress_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFuseProgress>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
			__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007418_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007418_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007418_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnDestroy_0000741A_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_0000741A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_0000741A_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_393132632_0;

	private EntityQuery __query_393132632_1;

	private EntityQuery __query_393132632_2;

	private EntityQuery __query_393132632_3;

	private EntityQuery __query_393132632_4;

	private EntityQuery __query_393132632_5;

	private EntityQuery __query_393132632_6;

	private EntityQuery __query_393132632_7;

	private EntityQuery __query_393132632_8;

	private EntityQuery __query_393132632_9;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Spell2007SuicideBugNestData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_393132632_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		float3 point = __query_393132632_2.GetSingleton<PlayerController_Dots>().mousePosition;
		CurrentRoomEntitiesSingleton singleton = __query_393132632_3.GetSingleton<CurrentRoomEntitiesSingleton>();
		GlobalRandom singleton2 = __query_393132632_4.GetSingleton<GlobalRandom>();
		SpellSingleton singleton3 = __query_393132632_5.GetSingleton<SpellSingleton>();
		NativeList<Spell2007Effect> nativeList = new NativeList<Spell2007Effect>(Allocator.Temp);
		foreach (QueryEnumerableWithEntity<EnabledRefRO<Spell2007SuicideBugNestInitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> item8 in IFE_393132632_0.Query(__query_393132632_0, __TypeHandle.__IFE_393132632_0_TypeHandle, ref state))
		{
			item8.Deconstruct(out var _, out var item2, out var item3, out var item4, out var item5, out var item6, out var item7, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2007SuicideBugNestData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW3 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO = item5;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW4 = item6;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO2 = item7;
			Entity entity2 = entity;
			if (InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref state, entity2))
			{
				continue;
			}
			entityCommandBuffer.SetComponentEnabled<Spell2007SuicideBugNestInitializedTag>(entity2, value: false);
			TeammateData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, entity2);
			if (componentAfterCompletingDependency.IsHoldByTeammate6)
			{
				return;
			}
			for (int i = 0; i < componentAfterCompletingDependency.TeammateCurrentFuseLevel + 1; i++)
			{
				float3 @float = new float3(0f, 0.5f * math.floor((float)i / 3f), 0.02f * math.floor((float)i / 3f));
				if (i % 3 == 1)
				{
					@float += new float3(-0.5f, 0.1f, 0.02f);
				}
				else if (i % 3 == 2)
				{
					@float += new float3(0.5f, 0.1f, 0.02f);
				}
				uncheckedRefRO.ValueRO.ColorType.ColorEnumToString(out var result);
				Spell2007Effect value = new Spell2007Effect
				{
					ColorName = result,
					SpellEntity = entity2,
					Scale = 1f,
					OffSet = @float + new float3(0f, -0.2f, 0f)
				};
				nativeList.Add(in value);
			}
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref state, entity2).ValueRW.Linear = float3.zero;
			uncheckedRefRW3.ValueRW.id = 700700 + uncheckedRefRO.ValueRO.Level;
			float num = uncheckedRefRO.ValueRO.Float2 / componentAfterCompletingDependency.TeammateSpeedRatio;
			uncheckedRefRW.ValueRW.SpawnTimer = num - Mathf.Min(num, 0.4f);
			bool flag = false;
			if (componentAfterCompletingDependency.AdvanceSkillLevel > 0)
			{
				UnitProperty_Dots targetPpt;
				switch (uncheckedRefRW4.ValueRO.Type)
				{
				case SpellSpecialMovementType.ChaseEnemy:
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__PlayerController_Dots_RO_ComponentLookup, ref state, uncheckedRefRO2.ValueRO.OwnerEntity))
					{
						if (singleton.FindNearestTarget(uncheckedRefRW2.ValueRO.Position, uncheckedRefRO.ValueRO.ShooterType, out entity, out var targetPosition2, out targetPpt))
						{
							if (DTool.IgnoreZDistanceSqr(in point, in targetPosition2) <= uncheckedRefRW4.ValueRO.ChaseRotateSpeed * 0.4f * (uncheckedRefRW4.ValueRO.ChaseRotateSpeed * 0.4f))
							{
								uncheckedRefRW2.ValueRW.Position.xy = targetPosition2.xy;
							}
							else
							{
								uncheckedRefRW2.ValueRW.Position.xy = point.xy;
							}
						}
					}
					else
					{
						singleton.FindNearestTarget(uncheckedRefRW2.ValueRO.Position, uncheckedRefRO.ValueRO.ShooterType, out entity, out var targetPosition3, out targetPpt);
						uncheckedRefRW2.ValueRW.Position.xy = targetPosition3.xy;
					}
					flag = true;
					break;
				case SpellSpecialMovementType.ChaseMouse:
					uncheckedRefRW2.ValueRW.Position.xy = point.xy;
					flag = true;
					break;
				case SpellSpecialMovementType.ChaseOwner:
					uncheckedRefRW2.ValueRW.Position = uncheckedRefRW4.ValueRO.UpdateSelfChasePosition(InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state), uncheckedRefRO2.ValueRO.Shooter);
					flag = true;
					break;
				case SpellSpecialMovementType.Rotation:
					uncheckedRefRW4.ValueRW.AroundAngle = singleton2.random.NextFloat(0f, 360f);
					uncheckedRefRW2.ValueRW.Position = uncheckedRefRW4.ValueRO.UpdateAroundFollowAndGetAroundPositionWhenAround(InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state));
					flag = true;
					break;
				case SpellSpecialMovementType.Normal:
				{
					float3 targetPosition;
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__PlayerController_Dots_RO_ComponentLookup, ref state, uncheckedRefRO2.ValueRO.OwnerEntity))
					{
						uncheckedRefRW2.ValueRW.Position.xy = point.xy;
						flag = true;
					}
					else if (singleton.FindNearestTarget(uncheckedRefRW2.ValueRO.Position, uncheckedRefRO.ValueRO.ShooterType, out entity, out targetPosition, out targetPpt))
					{
						uncheckedRefRW2.ValueRW.Position.xy = targetPosition.xy;
						flag = true;
					}
					break;
				}
				}
			}
			else if (uncheckedRefRW4.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				uncheckedRefRW4.ValueRW.AroundAngle = singleton2.random.NextFloat(0f, 360f);
				uncheckedRefRW2.ValueRW.Position = uncheckedRefRW4.ValueRO.UpdateAroundFollowAndGetAroundPositionWhenAround(InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state));
				flag = true;
			}
			uncheckedRefRW2.ValueRW.Position = Tool2D.GetNavMeshPointIngoreZ(uncheckedRefRW2.ValueRO.Position);
			if (flag)
			{
				ref float3 position = ref uncheckedRefRW2.ValueRW.Position;
				float3 float2 = position;
				float3 oldDir = new float3(0f, 1f, 0f);
				position = float2 + DTool.GetDir(in oldDir, singleton2.random.NextFloat(0f, 360f)) * uncheckedRefRO.ValueRO.Scatter * 0.03f;
			}
			uncheckedRefRW2.ValueRW.Position.z = 0f;
			if (componentAfterCompletingDependency.AdvanceSkillLevel > 0)
			{
				uncheckedRefRW.ValueRW.positionZ = -10f;
				uncheckedRefRW.ValueRW.AnimType = Spell2007SuicideBugNestData.Spell2007AnimType.Landing;
			}
		}
		foreach (Spell2007Effect item9 in nativeList)
		{
			Spell2007Effect current = item9;
			EntityManager entityManager = state.EntityManager;
			SpellTools.SpawnChild(in singleton3, in entityManager, 2007, "Nest", current.ColorName, in current.SpellEntity, out var child);
			state.EntityManager.SetComponentData(child, new LocalTransform
			{
				Scale = current.Scale,
				Position = new float3(0f, 9999f, 0f)
			});
			EffectsCollectorData componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, child);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, componentAfterCompletingDependency2.Effect2, value: false);
			entityCommandBuffer.AppendToBuffer(current.SpellEntity, new UnitMREttBED
			{
				ett = componentAfterCompletingDependency2.Effect2
			});
			entityCommandBuffer.AppendToBuffer(current.SpellEntity, new Spell2007FuseBuffer
			{
				Entity = child,
				Offset = current.OffSet
			});
		}
		nativeList.Dispose();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2007Job
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			PhysicsWorld = __query_393132632_6.GetSingleton<PhysicsWorldSingleton>(),
			ScreenShakeEntity = __query_393132632_7.GetSingletonEntity(),
			GlobalParticleEntity = __query_393132632_8.GetSingletonEntity(),
			SEPlayerEntity = __query_393132632_9.GetSingletonEntity(),
			UnitLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			ConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			EffectsCollectorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			MatrixLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref state),
			CMD = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2007Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2007SuicideBugNestSystem_Spell2007Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell2007SuicideBugNestInitializedTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2007SuicideBugNestData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_393132632_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393132632_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00007418_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_0000741A_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
