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

[CompilerGenerated]
[UpdateBefore(typeof(SpellTakeDamageResultSystem))]
[UpdateBefore(typeof(UnitTakeDamageClearSystem))]
[UpdateAfter(typeof(UnitPropertySystem))]
[BurstCompile]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public struct Spell1029DimensionTravellerHitEffectSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct Spell1029HitEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				public BufferTypeHandle<SpellHitEntity> __SpellHitEntity_RW_BufferTypeHandle;

				public ComponentTypeHandle<Spell1029DimensionTravellerData> __Spell1029DimensionTravellerData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
					__SpellHitEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<SpellHitEntity>();
					__Spell1029DimensionTravellerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1029DimensionTravellerData>();
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
					__SpellHitEntity_RW_BufferTypeHandle.Update(ref state);
					__Spell1029DimensionTravellerData_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHitEntity>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1029DimensionTravellerData>();
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
			public void Run(ref Spell1029HitEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1029HitEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1029HitEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1029HitEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1029HitEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1029HitEffectJob job, EntityManager entityManager)
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

		public Entity GlobalParticleEntity;

		public EntityCommandBuffer.ParallelWriter Cmd;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(SpellAspect spell, DynamicBuffer<SpellHitEntity> hitEntities, Spell1029DimensionTravellerData _, [ChunkIndexInQuery] int index)
		{
			if (hitEntities.Length > 0)
			{
				spell.Config.ValueRW.ColorType.ColorEnumToString(out var result);
				float3 layerPosition = DTool.GetLayerPosition(in spell.Transform.ValueRO.Position, LayerCorrectType.Coordinate);
				Cmd.AppendToBuffer(index, GlobalParticleEntity, new GlobalParticleEmitParams
				{
					Position = layerPosition + spell.Transform.ValueRO.Position,
					Size = spell.Transform.ValueRO.Scale,
					Name = $"1029_Hit_{result}",
					Velocity = spell.Movement.ValueRO.Direction
				});
				Cmd.AppendToBuffer(index, GlobalParticleEntity, new GlobalParticleEmitParams
				{
					Position = layerPosition + spell.Transform.ValueRO.Position,
					Size = spell.Transform.ValueRO.Scale,
					Name = $"1029_Disappear_{result}"
				});
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			BufferAccessor<SpellHitEntity> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SpellHitEntity_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1029DimensionTravellerData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spell = resolvedChunk[i];
					DynamicBuffer<SpellHitEntity> hitEntities = bufferAccessor[i];
					Execute(spell, hitEntities, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, i), chunkIndexInQuery);
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
						DynamicBuffer<SpellHitEntity> hitEntities2 = bufferAccessor[nextRangeBegin];
						Execute(spell2, hitEntities2, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, nextRangeBegin), chunkIndexInQuery);
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
					DynamicBuffer<SpellHitEntity> hitEntities3 = bufferAccessor[j];
					Execute(spell3, hitEntities3, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, j), chunkIndexInQuery);
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
					DynamicBuffer<SpellHitEntity> hitEntities4 = bufferAccessor[k];
					Execute(spell4, hitEntities4, InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1029DimensionTravellerData>(nativeArrayPtr, k), chunkIndexInQuery);
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
		public Spell1029HitEffectJob.InternalCompilerQueryAndHandleData __Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006FD9_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006FD9_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006FD9_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00006FDA_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006FDA_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006FDA_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_159335667_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<Spell1029DimensionTravellerData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		__ScheduleViaJobChunkExtension_0(new Spell1029HitEffectJob
		{
			GlobalParticleEntity = __query_159335667_0.GetSingletonEntity(),
			Cmd = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1029HitEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1029DimensionTravellerHitEffectSystem_Spell1029HitEffectJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335667_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006FD9_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006FDA_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerHitEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerHitEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1029DimensionTravellerHitEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
