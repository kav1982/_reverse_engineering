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
[BurstCompile]
internal struct TeammateSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	public struct TeammateJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				}

				public void Update(ref SystemState state)
				{
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
			public void Run(ref TeammateJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref TeammateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref TeammateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref TeammateJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref TeammateJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref TeammateJob job, EntityManager entityManager)
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

		public float Deltatime;

		public EntityCommandBuffer.ParallelWriter CMD;

		public EntityCommandBuffer.ParallelWriter InstantCMD;

		public Entity textFloatVFXBufferEtt;

		public Entity HpHealEffectEntity;

		public Entity HpDropEffectEntity;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<TeammateData> TeammateDataLookup;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<PhysicsCollider> ColliderLookUp;

		public Entity TeammateGhostEffectEntity;

		public Entity SpellEffectEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(ref TeammateData teammateData, [ChunkIndexInQuery] int chunkIndex, ref LocalTransform localTrans, ref SpellConfigComponentData config, ref SpellComponentData data, Entity entity, ref SpellMovementComponentData movementComponentData, ref UnitProperty_Dots teammatePpt, ref SpellElementEffectComponentData element)
		{
			if (!LocalTransformLookUp.HasComponent(data.OwnerEntity))
			{
				InstantCMD.SetComponentEnabled<TeammateDeadTag>(chunkIndex, entity, value: true);
			}
			if (teammateData.TeammateHpRecoverAmountPerSecond > 0f || teammateData.TeammateHpDropAmountPerSecond > 0f)
			{
				teammateData.TeammateHpEffectCalculateTimer += Deltatime;
				if (teammateData.TeammateHpEffectCalculateTimer >= 1f)
				{
					float num = math.floor(teammateData.TeammateHpEffectCalculateTimer);
					teammateData.TeammateHpEffectCalculateTimer -= num;
					bool flag = teammatePpt.unitCfg.currentHP >= teammatePpt.unitCfg.maxHP;
					if (teammateData.SeparateCalculateHpEffect)
					{
						if (!flag && teammateData.TeammateHpRecoverAmountPerSecond > 0f)
						{
							RecoverHp(ref teammatePpt, chunkIndex, teammateData.TeammateHpRecoverAmountPerSecond * num, ref localTrans, entity);
							SpawnTeammateRecoverHpEffect(chunkIndex, entity);
						}
						if (teammateData.TeammateHpDropAmountPerSecond > 0f)
						{
							LoseHp(teammateData.TeammateHpDropAmountPerSecond * num, entity, chunkIndex);
							SpawnTeammateLoseHpEffect(chunkIndex, entity);
						}
					}
					else
					{
						float num2 = teammateData.TeammateHpRecoverAmountPerSecond - teammateData.TeammateHpDropAmountPerSecond;
						if (num2 > 0f)
						{
							if (flag)
							{
								return;
							}
							RecoverHp(ref teammatePpt, chunkIndex, num2 * num, ref localTrans, entity);
							SpawnTeammateRecoverHpEffect(chunkIndex, entity);
						}
						else if (num2 < 0f)
						{
							LoseHp((0f - num2) * num, entity, chunkIndex);
							SpawnTeammateLoseHpEffect(chunkIndex, entity);
						}
					}
				}
			}
			if (teammateData.TeammateSuddenDeathHPThreshold > 0f && teammatePpt.unitCfg.currentHP / teammatePpt.unitCfg.maxHP < teammateData.TeammateSuddenDeathHPThreshold)
			{
				CMD.TeammateDeadTryActiveTeammateDelayDeathEffect(ref teammatePpt, ref TeammateDataLookup, entity, SpellEffectEntity, chunkIndex, ColliderLookUp, TeammateGhostEffectEntity);
			}
		}

		private void LoseHp(float damage, Entity targetEntity, int chunkIndex)
		{
			TakeDamageInfo_Dots element = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			element.damage = damage;
			CMD.AppendToBuffer(chunkIndex, targetEntity, element);
		}

		private void RecoverHp(ref UnitProperty_Dots teammatePpt, int chunkIndex, float healAmount, ref LocalTransform localTrans, Entity entity)
		{
			teammatePpt.unitCfg.currentHP = math.min(teammatePpt.unitCfg.currentHP + healAmount, teammatePpt.unitCfg.maxHP);
			CMD.AppendToBuffer(chunkIndex, textFloatVFXBufferEtt, new TextFloatVFXBED
			{
				number = healAmount,
				type = UITextFloatType.Recover,
				worldPos = localTrans.Position
			});
		}

		private void SpawnTeammateRecoverHpEffect(int chunkIndex, Entity entity)
		{
			if (SpellSingleton.Prefabs.TryGetValue("3108_Heal", out var item))
			{
				Entity e = CMD.Instantiate(chunkIndex, item);
				CMD.AddComponent<Parent>(chunkIndex, e);
				CMD.SetComponent(chunkIndex, e, new Parent
				{
					Value = entity
				});
				CMD.SetComponent(chunkIndex, e, new LocalTransform
				{
					Position = float3.zero,
					Scale = 1f,
					Rotation = quaternion.identity
				});
			}
		}

		private void SpawnTeammateLoseHpEffect(int chunkIndex, Entity entity)
		{
			if (SpellSingleton.Prefabs.TryGetValue("3015_LoseHp", out var item))
			{
				Entity e = CMD.Instantiate(chunkIndex, item);
				CMD.AddComponent<Parent>(chunkIndex, e);
				CMD.SetComponent(chunkIndex, e, new Parent
				{
					Value = entity
				});
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref TeammateData teammateData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, i);
					ref LocalTransform localTrans = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, i);
					Execute(ref teammateData, chunkIndexInQuery, ref localTrans, ref config, ref data, entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, i));
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
						ref TeammateData teammateData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform localTrans2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, nextRangeBegin);
						Execute(ref teammateData2, chunkIndexInQuery, ref localTrans2, ref config2, ref data2, entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, nextRangeBegin));
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
					ref TeammateData teammateData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, j);
					ref LocalTransform localTrans3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, j);
					Execute(ref teammateData3, chunkIndexInQuery, ref localTrans3, ref config3, ref data3, entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref TeammateData teammateData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr, k);
					ref LocalTransform localTrans4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, k);
					Execute(ref teammateData4, chunkIndexInQuery, ref localTrans4, ref config4, ref data4, entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr7, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr8, k));
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

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		public TeammateJob.InternalCompilerQueryAndHandleData __TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009214_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009214_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009214_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_841264395_0;

	private EntityQuery __query_841264395_1;

	private EntityQuery __query_841264395_2;

	private EntityQuery __query_841264395_3;

	private EntityQuery __query_841264395_4;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<TeammateGhostEffectData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<TextFloatVFXBED>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_841264395_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		EntityCommandBuffer entityCommandBuffer2 = new EntityCommandBuffer(Allocator.TempJob);
		__ScheduleViaJobChunkExtension_0(new TeammateJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			InstantCMD = entityCommandBuffer2.AsParallelWriter(),
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			TeammateDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state),
			Deltatime = state.WorldUnmanaged.Time.DeltaTime,
			textFloatVFXBufferEtt = __query_841264395_1.GetSingletonEntity(),
			SpellSingleton = __query_841264395_2.GetSingleton<SpellSingleton>(),
			SpellEffectEntity = __query_841264395_3.GetSingletonEntity(),
			TeammateGhostEffectEntity = __query_841264395_4.GetSingletonEntity(),
			ColliderLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state)
		}, __TypeHandle.__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer2.Playback(state.EntityManager);
		entityCommandBuffer2.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(TeammateJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__TeammateSystem_TeammateJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_841264395_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_841264395_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_841264395_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_841264395_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TeammateGhostEffectData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_841264395_4 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00009214_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((TeammateSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
