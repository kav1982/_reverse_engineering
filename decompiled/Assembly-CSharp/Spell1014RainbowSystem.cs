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
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
internal struct Spell1014RainbowSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	public struct Spell1014Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell1014Data> __Spell1014Data_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Spell1014Data_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1014Data>();
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell1014Data_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1014Data>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
			public void Run(ref Spell1014Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1014Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1014Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1014Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1014Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1014Job job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter CMD;

		public SpellSingleton SpellSingleton;

		public Entity EffectEntity;

		public Entity ScreenShakeSingleton;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitpropertyLookUp;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(ref LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData spellData, ref SpellMovementComponentData movement, ref Spell1014Data data, ref PhysicsVelocity velocity, [ChunkIndexInQuery] int chunkIndex, Entity entity)
		{
			if (!data.IsInitialized)
			{
				data.IsInitialized = true;
				if (!movement.IsFallSpell)
				{
					CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
					{
						Radius = 1f,
						Speed = 0.8f,
						Time = 0.12f
					});
				}
				FixedString32Bytes result;
				if (config.ColorType == SpellColorType.Void)
				{
					result = "Void";
				}
				else if (config.ColorType == SpellColorType.Fire)
				{
					result = "Fire";
				}
				else
				{
					SpellTools.GetSpell1014RainbowColor(spellData.InShootIndex, out result);
				}
				bool flag = UnitpropertyLookUp.HasComponent(spellData.OwnerEntity);
				if ((SpellSingleton.Harmonious && flag && !UnitpropertyLookUp[spellData.OwnerEntity].unitCfg.IsSameCamp(UnitType.Player)) || !flag)
				{
					SpellTools.GetSpell1014RainbowColor(6, out result);
				}
				CMD.AddComponent(chunkIndex, spellData.SpellEffectEntity, new GlobalParticle.Emitter
				{
					Type = GlobalParticleType.Spell,
					ParticleName = $"1014_TrailParticle_{result}",
					RandomPositionOffset = 0f
				});
				CMD.AddComponent(chunkIndex, spellData.SpellEffectEntity, new GlobalParticle.EmitDistanceCounter
				{
					Interval = ((config.ColorType == SpellColorType.Fire) ? 0.05f : 0.2f)
				});
				foreach (SpellEffect item in SpellSingleton.Effects[1014].GetValueArray(Allocator.Temp))
				{
					CMD.AppendToBuffer(chunkIndex, EffectEntity, new SpellEffectSystem.Require
					{
						Settings = item,
						Entity = entity,
						Color = result,
						SpellId = 1014
					});
				}
				if (movement.IsFallSpell)
				{
					movement.Speed *= 0.7f;
				}
			}
			if (!movement.IsFallSpell && movement.Speed > 0f && !movement.IsFallSpell)
			{
				movement.Speed = math.lerp(movement.Speed, -1f, config.Float1 * DeltaTime);
				if (movement.Speed <= 0f)
				{
					movement.Speed = 0f;
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1014Data_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, i);
					ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
					ref Spell1014Data data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1014Data>(nativeArrayPtr5, i);
					ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, i);
					Execute(ref transform, in config, in spellData, ref movement, ref data, ref velocity, chunkIndexInQuery, entity);
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
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref Spell1014Data data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1014Data>(nativeArrayPtr5, nextRangeBegin);
						ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, nextRangeBegin);
						Execute(ref transform2, in config2, in spellData2, ref movement2, ref data2, ref velocity2, chunkIndexInQuery, entity2);
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
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, j);
					ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
					ref Spell1014Data data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1014Data>(nativeArrayPtr5, j);
					ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, j);
					Execute(ref transform3, in config3, in spellData3, ref movement3, ref data3, ref velocity3, chunkIndexInQuery, entity3);
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
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr2, k);
					ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
					ref Spell1014Data data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1014Data>(nativeArrayPtr5, k);
					ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr6, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, k);
					Execute(ref transform4, in config4, in spellData4, ref movement4, ref data4, ref velocity4, chunkIndexInQuery, entity4);
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
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public Spell1014Job.InternalCompilerQueryAndHandleData __Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000066A0_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000066A0_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000066A0_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_000066A1_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_000066A1_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_000066A1_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1425355377_0;

	private EntityQuery __query_1425355377_1;

	private EntityQuery __query_1425355377_2;

	private EntityQuery __query_1425355377_3;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell1014Data>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_1425355377_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1014Job
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = entityCommandBuffer.AsParallelWriter(),
			SpellSingleton = __query_1425355377_1.GetSingleton<SpellSingleton>(),
			EffectEntity = __query_1425355377_2.GetSingletonEntity(),
			ScreenShakeSingleton = __query_1425355377_3.GetSingletonEntity(),
			UnitpropertyLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state)
		}, __TypeHandle.__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1014Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1014RainbowSystem_Spell1014Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1425355377_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1425355377_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1425355377_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1425355377_3 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000066A0_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_000066A1_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1014RainbowSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1014RainbowSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1014RainbowSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
