using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[BurstCompile]
internal struct Spell2002System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_300188055_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public BufferAccessor<LinkedEntityGroup> item6_BufferAccessor;

			public BufferAccessor<FuseHeadEntity> item7_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<LinkedEntityGroup>, DynamicBuffer<FuseHeadEntity>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<LinkedEntityGroup>, DynamicBuffer<FuseHeadEntity>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell2002Data>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<TeammateData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item5_IntPtr, index), item6_BufferAccessor[index], item7_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell2002Data> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<TeammateData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item5_ComponentTypeHandle_RO;

			private BufferTypeHandle<LinkedEntityGroup> item6_BufferTypeHandle_RW;

			private BufferTypeHandle<FuseHeadEntity> item7_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2002Data>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item6_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LinkedEntityGroup>();
				item7_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<FuseHeadEntity>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				item6_BufferTypeHandle_RW.Update(ref systemState);
				item7_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.item6_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item6_BufferTypeHandle_RW);
				result.item7_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item7_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<LinkedEntityGroup>, DynamicBuffer<FuseHeadEntity>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<LinkedEntityGroup>, DynamicBuffer<FuseHeadEntity>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell2002Data>();
			state.EntityManager.CompleteDependencyBeforeRO<TeammateData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LinkedEntityGroup>();
			state.EntityManager.CompleteDependencyBeforeRW<FuseHeadEntity>();
		}
	}

	private struct TypeHandle
	{
		public IFE_300188055_0.TypeHandle __IFE_300188055_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<FuseHeadData> __FuseHeadData_RO_ComponentLookup;

		public BufferLookup<UnitMREttBED> __UnitMREttBED_RW_BufferLookup;

		public ComponentLookup<MatOverrideFuseProgress> __MatOverrideFuseProgress_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public Spell2002Job.InternalCompilerQueryAndHandleData __Spell2002Job_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell2002LegsSystemJob.InternalCompilerQueryAndHandleData __Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle;

		public Spell2002EssenceLegsSystemJob.InternalCompilerQueryAndHandleData __Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_300188055_0_TypeHandle = new IFE_300188055_0.TypeHandle(ref state);
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__FuseHeadData_RO_ComponentLookup = state.GetComponentLookup<FuseHeadData>(isReadOnly: true);
			__UnitMREttBED_RW_BufferLookup = state.GetBufferLookup<UnitMREttBED>();
			__MatOverrideFuseProgress_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFuseProgress>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_0000716B_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_0000716B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_0000716B_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_300188055_0;

	private EntityQuery __query_300188055_1;

	private EntityQuery __query_300188055_2;

	private EntityQuery __query_300188055_3;

	private EntityQuery __query_300188055_4;

	private EntityQuery __query_300188055_5;

	private EntityQuery __query_300188055_6;

	private EntityQuery __query_300188055_7;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<TextFloatVFXBED>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell2002Data>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		GlobalRandom singleton = __query_300188055_1.GetSingleton<GlobalRandom>();
		CurrentRoomEntitiesSingleton singleton2 = __query_300188055_2.GetSingleton<CurrentRoomEntitiesSingleton>();
		float deltaTime = state.WorldUnmanaged.Time.DeltaTime;
		PhysicsWorldSingleton singleton3 = __query_300188055_3.GetSingleton<PhysicsWorldSingleton>();
		SpellSingleton singleton4 = __query_300188055_4.GetSingleton<SpellSingleton>();
		Entity singletonEntity = __query_300188055_5.GetSingletonEntity();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell2002Data>, InternalCompilerInterface.UncheckedRefRO<TeammateData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<LinkedEntityGroup>, DynamicBuffer<FuseHeadEntity>> item8 in IFE_300188055_0.Query(__query_300188055_0, __TypeHandle.__IFE_300188055_0_TypeHandle, ref state))
		{
			item8.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var item7, out var entity);
			InternalCompilerInterface.UncheckedRefRO<Spell2002Data> uncheckedRefRO = item;
			InternalCompilerInterface.UncheckedRefRO<TeammateData> uncheckedRefRO2 = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO3 = item3;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO4 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO5 = item5;
			DynamicBuffer<LinkedEntityGroup> dynamicBuffer = item6;
			DynamicBuffer<FuseHeadEntity> dynamicBuffer2 = item7;
			Entity entity2 = entity;
			if (uncheckedRefRO.ValueRO.State != 0)
			{
				continue;
			}
			EffectsCollectorData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, uncheckedRefRO5.ValueRO.SpellEffectEntity);
			Entity effect = componentAfterCompletingDependency.Effect2;
			Entity effect2 = componentAfterCompletingDependency.Effect1;
			LocalTransform componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, effect2);
			Entity effect3 = componentAfterCompletingDependency.Effect3;
			LocalTransform componentAfterCompletingDependency3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, effect3);
			componentAfterCompletingDependency3.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
			componentAfterCompletingDependency2.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency2, effect2);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency3, effect3);
			uncheckedRefRO3.ValueRO.ColorType.ColorEnumToString(out var result);
			for (int i = 0; i < uncheckedRefRO2.ValueRO.TeammateCurrentFuseLevel; i++)
			{
				Entity entity3 = state.EntityManager.Instantiate(singleton4.Prefabs[$"2002_FuseHead_{result}"]);
				float3 @float = new float3(0f, (float)i + 0.54f, -0.01f * (float)(i + 1));
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, LocalTransform.FromPosition(@float), entity3);
				RefRO<FuseHeadData> componentROAfterCompletingDependency = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__FuseHeadData_RO_ComponentLookup, ref state, entity3);
				DynamicBuffer<UnitMREttBED> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__UnitMREttBED_RW_BufferLookup, ref state, entity2);
				bufferAfterCompletingDependency.Add(new UnitMREttBED
				{
					ett = componentROAfterCompletingDependency.ValueRO.HeadEntity
				});
				bufferAfterCompletingDependency.Add(new UnitMREttBED
				{
					ett = componentROAfterCompletingDependency.ValueRO.SafeHeadEntity
				});
				Entity headEntity = componentROAfterCompletingDependency.ValueRO.HeadEntity;
				Entity safeHeadEntity = componentROAfterCompletingDependency.ValueRO.SafeHeadEntity;
				LocalTransform componentAfterCompletingDependency4 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, headEntity);
				LocalTransform componentAfterCompletingDependency5 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, safeHeadEntity);
				componentAfterCompletingDependency5.Scale = (DataMgr.settingData.SafeMode ? 1 : 0);
				componentAfterCompletingDependency4.Scale = ((!DataMgr.settingData.SafeMode) ? 1 : 0);
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency4, headEntity);
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency5, safeHeadEntity);
				Entity fireEffectEntity = componentROAfterCompletingDependency.ValueRO.FireEffectEntity;
				if (fireEffectEntity != Entity.Null)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, fireEffectEntity, value: false);
				}
				Entity safeFireEffectEntity = componentROAfterCompletingDependency.ValueRO.SafeFireEffectEntity;
				if (safeFireEffectEntity != Entity.Null)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, safeFireEffectEntity, value: false);
				}
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, headEntity, value: false);
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__MatOverrideFuseProgress_RW_ComponentLookup, ref state, safeHeadEntity, value: false);
				dynamicBuffer.Add(new LinkedEntityGroup
				{
					Value = entity3
				});
				FuseHeadData componentData = state.EntityManager.GetComponentData<FuseHeadData>(entity3);
				dynamicBuffer2.Add(new FuseHeadEntity
				{
					Entity = entity3,
					LegsRoot = componentData.RootEntity,
					HeadPos = uncheckedRefRO4.ValueRO.Position + InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, effect).Position + @float
				});
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2002Job
		{
			Random = singleton,
			DeltaTime = deltaTime,
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPptLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			CurrentRoomEntities = singleton2,
			PhysicsWorld = singleton3,
			EffectsCollectorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			SEPlayerSingleton = singletonEntity,
			CMD = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Spell2002LegsSystemJob
		{
			Random = singleton,
			DeltaTime = deltaTime,
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			PhysicsWorld = singleton3,
			TextFloatVFXBufferEtt = __query_300188055_6.GetSingletonEntity(),
			CurrentRoomEntities = singleton2,
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			CMD = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		state.Dependency = __ScheduleViaJobChunkExtension_2(new Spell2002EssenceLegsSystemJob
		{
			Random = singleton,
			DeltaTime = deltaTime,
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			PhysicsWorld = singleton3,
			CMD = entityCommandBuffer.AsParallelWriter(),
			GlobalParticleEmitBufferEntity = __query_300188055_7.GetSingletonEntity(),
			IsSafeMode = DataMgr.settingData.SafeMode
		}, __TypeHandle.__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2002Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2002Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Spell2002LegsSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2002LegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_2(Spell2002EssenceLegsSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2002EssenceLegsSystemJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2002Data>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LinkedEntityGroup>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<FuseHeadEntity>();
		__query_300188055_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_300188055_7 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_0000716B_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell2002System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell2002System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell2002System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
