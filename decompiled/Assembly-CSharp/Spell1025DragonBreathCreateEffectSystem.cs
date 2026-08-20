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

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEffectSystemGroup), OrderFirst = true)]
[UpdateBefore(typeof(SpellEffectSystem))]
[BurstCompile]
public struct Spell1025DragonBreathCreateEffectSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	private struct CreateEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell1025CreatEffectTag> __Spell1025CreatEffectTag_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				public ComponentTypeHandle<Spell1025DragonBreathData> __Spell1025DragonBreathData_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell1025CreatEffectTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025CreatEffectTag>();
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
					__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
					__Spell1025DragonBreathData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025DragonBreathData>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Spell1025CreatEffectTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					__Spell1025DragonBreathData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025CreatEffectTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025DragonBreathData>();
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
			public void Run(ref CreateEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref CreateEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref CreateEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref CreateEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref CreateEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref CreateEffectJob job, EntityManager entityManager)
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

		public Entity SEPlayer;

		public Entity LoopSEPlayer;

		public Entity RequireEntity;

		public EntityCommandBuffer.ParallelWriter ECB;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> unitLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> transLookup;

		public SpellSingleton SpellSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute([ChunkIndexInQuery] int index, Spell1025CreatEffectTag _, in SpellComponentData data, in SpellMovementComponentData movement, SpellConfigComponentData config, in LocalTransform localTransform, ref Spell1025DragonBreathData spell, Entity entity)
		{
			ECB.SetComponentEnabled<Spell1025CreatEffectTag>(index, entity, value: false);
			float3 position = DTool.CalculateLayerPosition(in localTransform.Position, LayerCorrectType.Coordinate);
			ref EntityCommandBuffer.ParallelWriter eCB = ref ECB;
			FixedString32Bytes effectName = "Start";
			float scale = 1f;
			eCB.CreateSpellGlobalParticle(index, in effectName, in position, in scale, in config, in data, in SpellSingleton, in float3.zero);
			if (config.ShooterType == UnitType.Player)
			{
				ref EntityCommandBuffer.ParallelWriter eCB2 = ref ECB;
				Entity loopSEPlayer = LoopSEPlayer;
				int prefabId = data.PrefabId;
				effectName = "Loop";
				eCB2.AppendToBuffer(index, loopSEPlayer, new LoopSEData(DTool.GetSpellSEName(prefabId, in effectName), config.Duration.Calculate()));
			}
			ref EntityCommandBuffer.ParallelWriter eCB3 = ref ECB;
			Entity sEPlayer = SEPlayer;
			int prefabId2 = data.PrefabId;
			effectName = "Start";
			eCB3.AppendToBuffer(index, sEPlayer, new SEData(DTool.GetSpellSEName(prefabId2, in effectName), SEPlayMode.Unique));
			if (!movement.IsFallSpell && movement.Type == SpellSpecialMovementType.Rotation)
			{
				CreateEffect(index, 0.5f, "RotationSpell", in config, in entity);
			}
		}

		[BurstCompile]
		private void CreateEffect([ChunkIndexInQuery] int index, float destroyDelay, FixedString32Bytes name, in SpellConfigComponentData config, in Entity spellEntity)
		{
			config.ColorType.ColorEnumToString(out var result);
			ECB.AppendToBuffer(index, RequireEntity, new SpellEffectSystem.Require
			{
				Settings = new SpellEffect
				{
					Name = name,
					DestroyDelay = destroyDelay,
					Layer = LayerCorrectType.Coordinate,
					ClearParticle = true
				},
				Entity = spellEntity,
				Color = result,
				SpellId = 1025
			});
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1025DragonBreathData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, i);
					ref SpellConfigComponentData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
					ref LocalTransform localTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i);
					ref Spell1025DragonBreathData spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, i);
					Execute(chunkIndexInQuery, default(Spell1025CreatEffectTag), in data, in movement, reference, in localTransform, ref spell, entity);
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
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellConfigComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref LocalTransform localTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin);
						ref Spell1025DragonBreathData spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, nextRangeBegin);
						Execute(chunkIndexInQuery, default(Spell1025CreatEffectTag), in data2, in movement2, reference2, in localTransform2, ref spell2, entity2);
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
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, j);
					ref SpellConfigComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
					ref LocalTransform localTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j);
					ref Spell1025DragonBreathData spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, j);
					Execute(chunkIndexInQuery, default(Spell1025CreatEffectTag), in data3, in movement3, reference3, in localTransform3, ref spell3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, k);
					ref SpellConfigComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
					ref LocalTransform localTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k);
					ref Spell1025DragonBreathData spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr5, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, k);
					Execute(chunkIndexInQuery, default(Spell1025CreatEffectTag), in data4, in movement4, reference4, in localTransform4, ref spell4, entity4);
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
		public CreateEffectJob.InternalCompilerQueryAndHandleData __Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00006C5D_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006C5D_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006C5D_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1989891596_0;

	private EntityQuery __query_1989891596_1;

	private EntityQuery __query_1989891596_2;

	private EntityQuery __query_1989891596_3;

	private EntityQuery __query_1989891596_4;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<LoopSEData>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellSingleton>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new CreateEffectJob
		{
			SEPlayer = __query_1989891596_0.GetSingletonEntity(),
			LoopSEPlayer = __query_1989891596_1.GetSingletonEntity(),
			RequireEntity = __query_1989891596_2.GetSingletonEntity(),
			ECB = __query_1989891596_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			SpellSingleton = __query_1989891596_4.GetSingleton<SpellSingleton>()
		}, __TypeHandle.__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(CreateEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1025DragonBreathCreateEffectSystem_CreateEffectJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1989891596_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<LoopSEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1989891596_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1989891596_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1989891596_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1989891596_4 = entityQueryBuilder2.Build(ref state);
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
		((Spell1025DragonBreathCreateEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006C5D_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1025DragonBreathCreateEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1025DragonBreathCreateEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
