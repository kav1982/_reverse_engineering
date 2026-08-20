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

[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitPropertySystem))]
[BurstCompile]
[CompilerGenerated]
internal struct TeammateGhostFireVisualEffectSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	public struct ApplyTeammateGhostEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
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
			public void Run(ref ApplyTeammateGhostEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref ApplyTeammateGhostEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref ApplyTeammateGhostEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref ApplyTeammateGhostEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref ApplyTeammateGhostEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref ApplyTeammateGhostEffectJob job, EntityManager entityManager)
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
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<EffectsCollectorData> EffectsCollectorLookUp;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<MatOverrideGhostEffect> GhostTeammateEffectLookUp;

		public EntityCommandBuffer.ParallelWriter CMD;

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[ReadOnly]
		public NativeList<Entity> ApplyTargets;

		[ReadOnly]
		public BufferLookup<Spell2003TentacleEffectData> Spell2003TentacleEffectData;

		[ReadOnly]
		public BufferLookup<Spell2007FuseBuffer> Spell2007FuseData;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, TeammateData teammateData, SpellComponentData spellData, SpellConfigComponentData config)
		{
			if (!ApplyTargets.Contains(entity))
			{
				return;
			}
			CMD.AddComponent(chunkIndex, entity, new GlobalParticle.Emitter
			{
				Type = GlobalParticleType.Spell,
				ParticleName = "3122_GlobalTrail",
				RandomPositionOffset = 0f
			});
			CMD.AddComponent(chunkIndex, entity, new GlobalParticle.EmitTimer
			{
				Interval = 0.1f
			});
			FixedString32Bytes fs = "3122_GhostEffect";
			Entity e = CMD.Instantiate(chunkIndex, SpellSingleton.Prefabs[fs]);
			CMD.AddComponent(chunkIndex, e, new TeammateGhostFireSyncData
			{
				Teammate = entity
			});
			switch (teammateData.TeammateType)
			{
			case TeammateType.teammate1:
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect1).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect2).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect3).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect4).ValueRW.ApplyGhostEffect = 1f;
				break;
			case TeammateType.teammate2:
				CMD.SetComponentEnabled<Spell2002StartGhostTag>(chunkIndex, entity, value: true);
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp[spellData.SpellEffectEntity].Effect1).ValueRW.ApplyGhostEffect = 1f;
				break;
			case TeammateType.teammate3:
			{
				Spell2003TentacleEffectData.TryGetBuffer(entity, out var bufferData2);
				{
					foreach (Spell2003TentacleEffectData item in bufferData2)
					{
						GhostTeammateEffectLookUp.GetRefRW(item.IdleEffectEntity).ValueRW.ApplyGhostEffect = 1f;
						GhostTeammateEffectLookUp.GetRefRW(item.AttackEffectEntity).ValueRW.ApplyGhostEffect = 1f;
					}
					break;
				}
			}
			case TeammateType.teammate5:
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1).ValueRW.Effect3).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1).ValueRW.Effect2).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect1).ValueRW.Effect1).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2).ValueRW.Effect3).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2).ValueRW.Effect2).ValueRW.ApplyGhostEffect = 1f;
				GhostTeammateEffectLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(EffectsCollectorLookUp.GetRefRW(spellData.SpellEffectEntity).ValueRW.Effect2).ValueRW.Effect1).ValueRW.ApplyGhostEffect = 1f;
				break;
			case TeammateType.teammate6:
				CMD.SetComponentEnabled<Spell2006GhostTag>(chunkIndex, entity, value: true);
				break;
			case TeammateType.teammate7:
			{
				Spell2007FuseData.TryGetBuffer(entity, out var bufferData);
				{
					foreach (Spell2007FuseBuffer item2 in bufferData)
					{
						RefRW<EffectsCollectorData> refRW = EffectsCollectorLookUp.GetRefRW(item2.Entity);
						GhostTeammateEffectLookUp.GetRefRW(refRW.ValueRW.Effect2).ValueRW.ApplyGhostEffect = 1f;
					}
					break;
				}
			}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
					ref TeammateData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, i);
					ref SpellComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
					Execute(config: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i), chunkIndex: chunkIndexInQuery, entity: entity, teammateData: reference, spellData: reference2);
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
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, nextRangeBegin);
						ref TeammateData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
						Execute(config: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin), chunkIndex: chunkIndexInQuery, entity: entity2, teammateData: reference3, spellData: reference4);
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
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, j);
					ref TeammateData reference5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, j);
					ref SpellComponentData reference6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
					Execute(config: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j), chunkIndex: chunkIndexInQuery, entity: entity3, teammateData: reference5, spellData: reference6);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, k);
					ref TeammateData reference7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr2, k);
					ref SpellComponentData reference8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
					Execute(config: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k), chunkIndex: chunkIndexInQuery, entity: entity4, teammateData: reference7, spellData: reference8);
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
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<MatOverrideGhostEffect> __MatOverrideGhostEffect_RW_ComponentLookup;

		public BufferLookup<Spell2003TentacleEffectData> __Spell2003TentacleEffectData_RW_BufferLookup;

		public BufferLookup<Spell2007FuseBuffer> __Spell2007FuseBuffer_RW_BufferLookup;

		public ApplyTeammateGhostEffectJob.InternalCompilerQueryAndHandleData __TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__MatOverrideGhostEffect_RW_ComponentLookup = state.GetComponentLookup<MatOverrideGhostEffect>();
			__Spell2003TentacleEffectData_RW_BufferLookup = state.GetBufferLookup<Spell2003TentacleEffectData>();
			__Spell2007FuseBuffer_RW_BufferLookup = state.GetBufferLookup<Spell2007FuseBuffer>();
			__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009172_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009172_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009172_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00009173_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00009173_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00009173_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1625628562_0;

	private EntityQuery __query_1625628562_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<TeammateData>();
		state.EntityManager.CreateSingletonBuffer<TeammateGhostEffectData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		DynamicBuffer<TeammateGhostEffectData> singletonBuffer = __query_1625628562_0.GetSingletonBuffer<TeammateGhostEffectData>();
		NativeList<Entity> applyTargets = new NativeList<Entity>(singletonBuffer.Length, Allocator.TempJob);
		foreach (TeammateGhostEffectData item in singletonBuffer)
		{
			TeammateGhostEffectData current = item;
			applyTargets.Add(in current.Entity);
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		JobHandle jobHandle2 = (state.Dependency = __ScheduleViaJobChunkExtension_0(new ApplyTeammateGhostEffectJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			EffectsCollectorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			GhostTeammateEffectLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideGhostEffect_RW_ComponentLookup, ref state),
			ApplyTargets = applyTargets,
			Spell2003TentacleEffectData = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Spell2003TentacleEffectData_RW_BufferLookup, ref state),
			SpellSingleton = __query_1625628562_1.GetSingleton<SpellSingleton>(),
			Spell2007FuseData = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Spell2007FuseBuffer_RW_BufferLookup, ref state)
		}, __TypeHandle.__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false));
		JobHandle jobHandle3 = jobHandle2;
		jobHandle3.Complete();
		applyTargets.Dispose(state.Dependency);
		singletonBuffer.Clear();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(ApplyTeammateGhostEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__TeammateGhostFireVisualEffectSystem_ApplyTeammateGhostEffectJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateGhostEffectData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1625628562_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1625628562_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00009172_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00009173_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((TeammateGhostFireVisualEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateGhostFireVisualEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((TeammateGhostFireVisualEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
