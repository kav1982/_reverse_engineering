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

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateBefore(typeof(UnitTakeDamageClearSystem))]
internal struct Spell2007SuicideBugNestBeHurtSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	private struct Spell2007HurtJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public BufferTypeHandle<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferTypeHandle;

				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell2007SuicideBugNestData> __Spell2007SuicideBugNestData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<TeammateData> __TeammateData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__TakeDamageInfo_Dots_RW_BufferTypeHandle = state.GetBufferTypeHandle<TakeDamageInfo_Dots>();
					__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Spell2007SuicideBugNestData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell2007SuicideBugNestData>();
					__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
					__TeammateData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateData>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
				}

				public void Update(ref SystemState state)
				{
					__TakeDamageInfo_Dots_RW_BufferTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell2007SuicideBugNestData_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
					__TeammateData_RW_ComponentTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TakeDamageInfo_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2007SuicideBugNestData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
			public void Run(ref Spell2007HurtJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell2007HurtJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell2007HurtJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell2007HurtJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell2007HurtJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell2007HurtJob job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter CMD;

		public SpellSingleton SpellSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int chunkIndex, DynamicBuffer<TakeDamageInfo_Dots> damageBuffer, SpellElementEffectComponentData element, LocalTransform transform, SpellComponentData data, SpellMovementComponentData movement, Spell2007SuicideBugNestData _, SpellConfigComponentData config, TeammateData teammateData, UnitProperty_Dots property)
		{
			for (int i = 0; i < damageBuffer.Length; i++)
			{
				CMD.SpawnSuicideBug(chunkIndex, config.Int1 * (teammateData.TeammateCurrentFuseLevel + 1), in data, in movement, in config, in transform, in teammateData, in element, in SpellSingleton, property.voidExplosionData.HpToDmgRatio, property.voidExplosionData.ExplosionRange, property.voidExplosionData.InstantKillRatio, property.voidExplosionData.ConstVoidEffect, property.unitCfg.maxHP, 1f);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			BufferAccessor<TakeDamageInfo_Dots> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell2007SuicideBugNestData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__TeammateData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					DynamicBuffer<TakeDamageInfo_Dots> damageBuffer = bufferAccessor[i];
					ref SpellElementEffectComponentData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr, i);
					ref LocalTransform reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref SpellComponentData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
					ref SpellMovementComponentData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
					ref Spell2007SuicideBugNestData reference5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr5, i);
					ref SpellConfigComponentData reference6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, i);
					ref TeammateData reference7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, i);
					Execute(property: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, i), chunkIndex: chunkIndexInQuery, damageBuffer: damageBuffer, element: reference, transform: reference2, data: reference3, movement: reference4, _: reference5, config: reference6, teammateData: reference7);
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
						DynamicBuffer<TakeDamageInfo_Dots> damageBuffer2 = bufferAccessor[nextRangeBegin];
						ref SpellElementEffectComponentData reference8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform reference9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref SpellComponentData reference10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellMovementComponentData reference11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref Spell2007SuicideBugNestData reference12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr5, nextRangeBegin);
						ref SpellConfigComponentData reference13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref TeammateData reference14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, nextRangeBegin);
						Execute(property: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, nextRangeBegin), chunkIndex: chunkIndexInQuery, damageBuffer: damageBuffer2, element: reference8, transform: reference9, data: reference10, movement: reference11, _: reference12, config: reference13, teammateData: reference14);
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
					DynamicBuffer<TakeDamageInfo_Dots> damageBuffer3 = bufferAccessor[j];
					ref SpellElementEffectComponentData reference15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr, j);
					ref LocalTransform reference16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref SpellComponentData reference17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
					ref SpellMovementComponentData reference18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
					ref Spell2007SuicideBugNestData reference19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr5, j);
					ref SpellConfigComponentData reference20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, j);
					ref TeammateData reference21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, j);
					Execute(property: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, j), chunkIndex: chunkIndexInQuery, damageBuffer: damageBuffer3, element: reference15, transform: reference16, data: reference17, movement: reference18, _: reference19, config: reference20, teammateData: reference21);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					DynamicBuffer<TakeDamageInfo_Dots> damageBuffer4 = bufferAccessor[k];
					ref SpellElementEffectComponentData reference22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr, k);
					ref LocalTransform reference23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref SpellComponentData reference24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
					ref SpellMovementComponentData reference25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
					ref Spell2007SuicideBugNestData reference26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell2007SuicideBugNestData>(nativeArrayPtr5, k);
					ref SpellConfigComponentData reference27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, k);
					ref TeammateData reference28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TeammateData>(nativeArrayPtr7, k);
					Execute(property: InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr8, k), chunkIndex: chunkIndexInQuery, damageBuffer: damageBuffer4, element: reference22, transform: reference23, data: reference24, movement: reference25, _: reference26, config: reference27, teammateData: reference28);
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
		public Spell2007HurtJob.InternalCompilerQueryAndHandleData __Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007482_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007482_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007482_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00007483_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007483_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007483_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_393133300_0;

	private EntityQuery __query_393133300_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell2007SuicideBugNestData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2007HurtJob
		{
			CMD = __query_393133300_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			SpellSingleton = __query_393133300_1.GetSingleton<SpellSingleton>()
		}, __TypeHandle.__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2007HurtJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2007SuicideBugNestBeHurtSystem_Spell2007HurtJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393133300_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_393133300_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00007482_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007483_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestBeHurtSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestBeHurtSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2007SuicideBugNestBeHurtSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
