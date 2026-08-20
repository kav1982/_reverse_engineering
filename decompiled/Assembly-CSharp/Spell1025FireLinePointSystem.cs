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
using Unity.Transforms;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[BurstCompile]
[UpdateAfter(typeof(Spell1025DragonBreathSystem))]
public struct Spell1025FireLinePointSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct Spell1025FireLinePointMoveJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell1025DragonBreathFireLinePointData> __Spell1025DragonBreathFireLinePointData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell1025DragonBreathFireLinePointData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025DragonBreathFireLinePointData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Spell1025DragonBreathFireLinePointData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1025DragonBreathFireLinePointData>();
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
			public void Run(ref Spell1025FireLinePointMoveJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1025FireLinePointMoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1025FireLinePointMoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1025FireLinePointMoveJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1025FireLinePointMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1025FireLinePointMoveJob job, EntityManager entityManager)
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

		[NativeDisableContainerSafetyRestriction]
		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> MovementLookUp;

		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public BufferLookup<Spell1025FireLinePointsBuffer> FireLinePointsBufferLookUp;

		public EntityCommandBuffer.ParallelWriter CMD;

		public float DeltaTime;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute([ChunkIndexInQuery] int index, ref Spell1025DragonBreathFireLinePointData data, ref LocalTransform transform, Entity entity)
		{
			if (!FireLinePointsBufferLookUp.TryGetBuffer(data.Parent, out var bufferData))
			{
				transform.Scale -= 3f * DeltaTime;
				if (transform.Scale <= 0f)
				{
					CMD.DestroyEntity(index, entity);
				}
				return;
			}
			if (data.CurrentIndex >= bufferData.Length - 1)
			{
				CMD.DestroyEntity(index, entity);
				return;
			}
			float3 position = bufferData[data.CurrentIndex].Position;
			float3 position2 = bufferData[data.CurrentIndex + 1].Position;
			int num = MovementLookUp[data.Parent].Type switch
			{
				SpellSpecialMovementType.Normal => 1, 
				SpellSpecialMovementType.Rotation => 2, 
				_ => 0, 
			};
			int num2 = math.select(0, 1, num != 0);
			float num3 = ((num2 != 0) ? ((data.CurrentIndex >= num) ? 7.5f : 5f) : ((data.CurrentIndex >= 1) ? 30f : 5f));
			data.CurrentPercent += num3 * DeltaTime;
			if (data.CurrentPercent >= 1f)
			{
				data.CurrentPercent = 0f;
				data.CurrentIndex++;
				if (data.CurrentIndex >= bufferData.Length - 1)
				{
					CMD.DestroyEntity(index, entity);
					return;
				}
			}
			float num4 = math.clamp(data.CurrentPercent + data.offset, 0f, 1f);
			float3 @float = position * (1f - num4) + position2 * num4;
			float num5 = math.select(num4, easeInOut(num4), num2 == 1 && data.CurrentIndex >= num);
			float3 float2 = default(float3);
			float2.x = @float.x;
			float2.y = @float.y;
			float2.z = position.z * (1f - num5) + position2.z * num5;
			@float = float2;
			float3 layerPosition = DTool.GetLayerPosition(in @float, LayerCorrectType.Coordinate);
			transform.Position = @float + layerPosition;
		}

		private float easeInOut(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1025DragonBreathFireLinePointData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell1025DragonBreathFireLinePointData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathFireLinePointData>(nativeArrayPtr, i);
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
					Execute(chunkIndexInQuery, ref data, ref transform, entity);
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
						ref Spell1025DragonBreathFireLinePointData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathFireLinePointData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
						Execute(chunkIndexInQuery, ref data2, ref transform2, entity2);
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
					ref Spell1025DragonBreathFireLinePointData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathFireLinePointData>(nativeArrayPtr, j);
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
					Execute(chunkIndexInQuery, ref data3, ref transform3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell1025DragonBreathFireLinePointData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathFireLinePointData>(nativeArrayPtr, k);
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
					Execute(chunkIndexInQuery, ref data4, ref transform4, entity4);
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
	private readonly struct IFE_1905977759_0
	{
		public struct ResolvedChunk
		{
			public EnabledMask item1_EnabledMask;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (EnabledRefRW<Spell1025AnimaDataInitTag>, InternalCompilerInterface.UncheckedRefRW<Spell1025FirePointAnimaData>, InternalCompilerInterface.UncheckedRefRW<Spell1025ChangeFrameAnim>) Get(int index)
			{
				return (item1_EnabledMask.GetEnabledRefRW<Spell1025AnimaDataInitTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1025FirePointAnimaData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1025ChangeFrameAnim>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1025AnimaDataInitTag> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1025FirePointAnimaData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1025ChangeFrameAnim> item3_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1025AnimaDataInitTag>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1025FirePointAnimaData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1025ChangeFrameAnim>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_EnabledMask = archetypeChunk.GetEnabledMask(ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(EnabledRefRW<Spell1025AnimaDataInitTag>, InternalCompilerInterface.UncheckedRefRW<Spell1025FirePointAnimaData>, InternalCompilerInterface.UncheckedRefRW<Spell1025ChangeFrameAnim>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (EnabledRefRW<Spell1025AnimaDataInitTag>, InternalCompilerInterface.UncheckedRefRW<Spell1025FirePointAnimaData>, InternalCompilerInterface.UncheckedRefRW<Spell1025ChangeFrameAnim>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1025AnimaDataInitTag>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1025FirePointAnimaData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1025ChangeFrameAnim>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1905977759_0.TypeHandle __IFE_1905977759_0_TypeHandle;

		public BufferLookup<Spell1025FireLinePointsBuffer> __Spell1025FireLinePointsBuffer_RW_BufferLookup;

		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentLookup;

		public Spell1025FireLinePointMoveJob.InternalCompilerQueryAndHandleData __Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1905977759_0_TypeHandle = new IFE_1905977759_0.TypeHandle(ref state);
			__Spell1025FireLinePointsBuffer_RW_BufferLookup = state.GetBufferLookup<Spell1025FireLinePointsBuffer>();
			__SpellMovementComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>();
			__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006CCD_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006CCD_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006CCD_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1905977759_0;

	private EntityQuery __query_1905977759_1;

	private EntityQuery __query_1905977759_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell1025DragonBreathFireLinePointData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		GlobalRandom singleton = __query_1905977759_1.GetSingleton<GlobalRandom>();
		foreach (var item in IFE_1905977759_0.Query(__query_1905977759_0, __TypeHandle.__IFE_1905977759_0_TypeHandle, ref state))
		{
			var (enabledRefRW, uncheckedRefRW, uncheckedRefRW2) = item;
			uncheckedRefRW2.ValueRW.FrameIndex = singleton.random.NextInt(uncheckedRefRW.ValueRO.MinIndex, uncheckedRefRW.ValueRO.MaxIndex);
			enabledRefRW.ValueRW = false;
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1025FireLinePointMoveJob
		{
			FireLinePointsBufferLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Spell1025FireLinePointsBuffer_RW_BufferLookup, ref state),
			MovementLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state),
			CMD = __query_1905977759_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime
		}, __TypeHandle.__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1025FireLinePointMoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1025FireLinePointSystem_Spell1025FireLinePointMoveJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1025FirePointAnimaData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025ChangeFrameAnim>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025AnimaDataInitTag>();
		__query_1905977759_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977759_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977759_2 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006CCD_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1025FireLinePointSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1025FireLinePointSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1025FireLinePointSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
