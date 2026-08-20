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
using Unity.Physics;
using Unity.Transforms;

[UpdateAfter(typeof(Spell4024DaveHarpoonSystem))]
[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
internal struct Spell4024DaveHarpoonThunderRelicSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Spell4024HarpoonThunderRelicJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				public ComponentTypeHandle<Spell4024DaveHarpoonThunderRelicData> __Spell4024DaveHarpoonThunderRelicData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					__Spell4024DaveHarpoonThunderRelicData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4024DaveHarpoonThunderRelicData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
					__Spell4024DaveHarpoonThunderRelicData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4024DaveHarpoonThunderRelicData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
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
			public void Run(ref Spell4024HarpoonThunderRelicJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell4024HarpoonThunderRelicJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell4024HarpoonThunderRelicJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell4024HarpoonThunderRelicJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell4024HarpoonThunderRelicJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell4024HarpoonThunderRelicJob job, EntityManager entityManager)
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

		[ReadOnly]
		public SpellSingleton SpellSingleton;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[ReadOnly]
		public EntityStorageInfoLookup Exists;

		[ReadOnly]
		public BufferLookup<TakeDamageInfo_Dots> DamageBufLookup;

		public GlobalRandom Random;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, ref Spell4024DaveHarpoonThunderRelicData tRelicData, ref LocalTransform localTransform)
		{
			if (!tRelicData.IsInitialized)
			{
				tRelicData.IsInitialized = true;
				DoThunder(chunkIndex, isFirst: true, ref tRelicData);
			}
			tRelicData.Timer += DeltaTime;
			if (tRelicData.Timer < 0.2f)
			{
				return;
			}
			tRelicData.Timer -= 0.1f;
			tRelicData.Count++;
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			ref float3 currentPos = ref tRelicData.CurrentPos;
			ref float radius = ref tRelicData.Radius;
			UnitType selfCamp = UnitType.Player;
			SpellTools.GetAttackableEntitiesInRange(in currentPos, in radius, in selfCamp, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
			SpellTools.RemoveCannotAttackSpell(ref entities, UnitType.Player, in SpellConfigLookup);
			RemoveSameCampUnitAndNotAttackUnit(UnitType.Player, ref entities, tRelicData.CurrentEntity);
			if (entities.Length <= 0)
			{
				CMD.DestroyEntity(chunkIndex, entity);
				return;
			}
			int index = Random.random.NextInt(entities.Length);
			tRelicData.LastEntity = tRelicData.CurrentEntity;
			tRelicData.CurrentEntity = entities[index];
			DoThunder(chunkIndex, isFirst: false, ref tRelicData);
			if (tRelicData.Count >= 3)
			{
				CMD.DestroyEntity(chunkIndex, entity);
			}
		}

		private void DoThunder(int chunkIndex, bool isFirst, ref Spell4024DaveHarpoonThunderRelicData tRelicData)
		{
			LocalTransform componentData;
			if (isFirst)
			{
				if (!TransformLookUp.TryGetComponent(tRelicData.CurrentEntity, out componentData))
				{
					return;
				}
				tRelicData.CurrentPos = componentData.Position;
				tRelicData.LastPos = tRelicData.HarpoonStartPos;
			}
			else
			{
				if (TransformLookUp.TryGetComponent(tRelicData.LastEntity, out var componentData2))
				{
					tRelicData.LastPos = componentData2.Position;
				}
				if (TransformLookUp.TryGetComponent(tRelicData.CurrentEntity, out componentData))
				{
					tRelicData.CurrentPos = componentData.Position;
				}
			}
			Entity e = CMD.Instantiate(chunkIndex, SpellSingleton.Prefabs["4024_ChainEffect_Player"]);
			PosLayerCorrect(tRelicData.CurrentPos, out var _out);
			PosLayerCorrect(tRelicData.LastPos, out var _out2);
			CMD.SetComponent(chunkIndex, e, new Spell4024DaveHarpoonThunderRelicEffectData
			{
				pos1 = _out,
				pos2 = _out2,
				IsFirst = isFirst
			});
			tRelicData.LastPos = tRelicData.CurrentPos;
			TakeDamageInfo_Dots.NewInfo(tRelicData.HarpoonEntity, CostPenetrate: false, in tRelicData.HarpoonConfig, in tRelicData.HarpoonMove, in tRelicData.HarpoonTrans, in tRelicData.HarpoonEle, in tRelicData.HarpoonComp, out var info, CostRefraction: false);
			info.damage = tRelicData.Damage * math.pow(tRelicData.DamageRate, tRelicData.Count + 1);
			if (Exists.Exists(tRelicData.CurrentEntity) && DamageBufLookup.HasBuffer(tRelicData.CurrentEntity))
			{
				CMD.TryAttackEntity(chunkIndex, in tRelicData.CurrentEntity, in info, in UnitPropertyLookup, in SpellConfigLookup, checkCamp: true, recordTargetToSpell: false);
			}
		}

		private void PosLayerCorrect(float3 pos, out float3 _out)
		{
			float3 layerPosition = DTool.GetLayerPosition(in pos, LayerCorrectType.Coordinate);
			_out = pos + layerPosition;
		}

		private void RemoveSameCampUnitAndNotAttackUnit(UnitType selfCamp, ref NativeList<Entity> entities, Entity excludeEtt)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				if (UnitPropertyLookup.TryGetComponent(entities[i], out var componentData))
				{
					UnitType unitType = componentData.unitCfg.unitType;
					if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness || DTool.IsSameCamp(componentData.unitCfg.unitType, selfCamp) || excludeEtt == entities[i])
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
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4024DaveHarpoonThunderRelicData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
					Execute(chunkIndexInQuery, entity, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonThunderRelicData>(nativeArrayPtr2, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i));
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
						Execute(chunkIndexInQuery, entity2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonThunderRelicData>(nativeArrayPtr2, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin));
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
					Execute(chunkIndexInQuery, entity3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonThunderRelicData>(nativeArrayPtr2, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j));
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
					Execute(chunkIndexInQuery, entity4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4024DaveHarpoonThunderRelicData>(nativeArrayPtr2, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k));
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
		[ReadOnly]
		public EntityStorageInfoLookup __EntityStorageInfoLookup;

		[ReadOnly]
		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RO_BufferLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell4024HarpoonThunderRelicJob.InternalCompilerQueryAndHandleData __Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
			__TakeDamageInfo_Dots_RO_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007AD2_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007AD2_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007AD2_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00007AD3_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00007AD3_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00007AD3_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1983874753_0;

	private EntityQuery __query_1983874753_1;

	private EntityQuery __query_1983874753_2;

	private EntityQuery __query_1983874753_3;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell4024DaveHarpoonThunderRelicData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_1983874753_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		EntityStorageInfoLookup entityStorageInfoLookup = InternalCompilerInterface.GetEntityStorageInfoLookup(ref __TypeHandle.__EntityStorageInfoLookup, ref state);
		entityStorageInfoLookup.Update(ref state);
		BufferLookup<TakeDamageInfo_Dots> bufferLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TakeDamageInfo_Dots_RO_BufferLookup, ref state);
		bufferLookup.Update(ref state);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4024HarpoonThunderRelicJob
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = entityCommandBuffer.AsParallelWriter(),
			SpellSingleton = __query_1983874753_1.GetSingleton<SpellSingleton>(),
			PhysicsWorld = __query_1983874753_2.GetSingleton<PhysicsWorldSingleton>(),
			Random = __query_1983874753_3.GetSingleton<GlobalRandom>(),
			TransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			Exists = entityStorageInfoLookup,
			DamageBufLookup = bufferLookup
		}, __TypeHandle.__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4024HarpoonThunderRelicJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4024DaveHarpoonThunderRelicSystem_Spell4024HarpoonThunderRelicJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1983874753_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1983874753_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1983874753_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1983874753_3 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00007AD2_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00007AD3_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonThunderRelicSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonThunderRelicSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4024DaveHarpoonThunderRelicSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
