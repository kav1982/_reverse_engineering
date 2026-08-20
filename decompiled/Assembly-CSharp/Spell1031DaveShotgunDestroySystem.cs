using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
[BurstCompile]
internal struct Spell1031DaveShotgunDestroySystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct Spell1031DaveShotgunDestroyJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				public ComponentTypeHandle<SpellDestroyTag> __SpellDestroyTag_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<Spell1031DaveShotgunData> __Spell1031DaveShotgunData_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
					__SpellDestroyTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellDestroyTag>();
					__Spell1031DaveShotgunData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1031DaveShotgunData>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					__SpellDestroyTag_RW_ComponentTypeHandle.Update(ref state);
					__Spell1031DaveShotgunData_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1031DaveShotgunData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellDestroyTag>();
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
			public void Run(ref Spell1031DaveShotgunDestroyJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1031DaveShotgunDestroyJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1031DaveShotgunDestroyJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1031DaveShotgunDestroyJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1031DaveShotgunDestroyJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1031DaveShotgunDestroyJob job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter Ecb;

		public Entity GlobalParticleBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(in SpellConfigComponentData config, SpellMovementComponentData movement, in LocalTransform transform, [ChunkIndexInQuery] int chunkIndex, SpellDestroyTag _, in Spell1031DaveShotgunData data)
		{
			if (!(transform.Scale < 0.05f))
			{
				config.ColorType.ColorEnumToString(out var result);
				float3 layerPosition = DTool.GetLayerPosition(in transform.Position, LayerCorrectType.Coordinate);
				Ecb.AppendToBuffer(chunkIndex, GlobalParticleBufferEntity, new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1031_Hit_{result}", layerPosition + transform.Position)
				{
					Size = transform.Scale,
					Velocity = movement.Direction
				});
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Spell1031DaveShotgunData_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, i);
					ref SpellMovementComponentData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
					ref Spell1031DaveShotgunData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr4, i);
					Execute(in config, reference, in transform, chunkIndexInQuery, default(SpellDestroyTag), in data);
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
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, nextRangeBegin);
						ref SpellMovementComponentData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
						ref Spell1031DaveShotgunData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr4, nextRangeBegin);
						Execute(in config2, reference2, in transform2, chunkIndexInQuery, default(SpellDestroyTag), in data2);
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
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, j);
					ref SpellMovementComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
					ref Spell1031DaveShotgunData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr4, j);
					Execute(in config3, reference3, in transform3, chunkIndexInQuery, default(SpellDestroyTag), in data3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, k);
					ref SpellMovementComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
					ref Spell1031DaveShotgunData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1031DaveShotgunData>(nativeArrayPtr4, k);
					Execute(in config4, reference4, in transform4, chunkIndexInQuery, default(SpellDestroyTag), in data4);
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
		public Spell1031DaveShotgunDestroyJob.InternalCompilerQueryAndHandleData __Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000701D_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000701D_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000701D_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_0000701E_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_0000701E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_0000701E_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1781298003_0;

	private EntityQuery __query_1781298003_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell1031DaveShotgunData>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1031DaveShotgunDestroyJob
		{
			Ecb = __query_1781298003_0.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			GlobalParticleBufferEntity = __query_1781298003_1.GetSingletonEntity()
		}, __TypeHandle.__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1031DaveShotgunDestroyJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1031DaveShotgunDestroySystem_Spell1031DaveShotgunDestroyJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1781298003_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1781298003_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000701D_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_0000701E_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1031DaveShotgunDestroySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1031DaveShotgunDestroySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1031DaveShotgunDestroySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
