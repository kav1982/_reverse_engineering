using System;
using System.Collections;
using System.Collections.Generic;
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
using UnityEngine;

[CompilerGenerated]
[UpdateAfter(typeof(Spell1025DragonBreathCreateEffectSystem))]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
public struct Spell1025DragonBreathEffectSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[WithAll(new Type[] { typeof(SpellFallTag) })]
	[BurstCompile]
	private struct Spell1025FallGroundEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				public BufferTypeHandle<Spell1025FireGroundEffectBuffer> __Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle;

				public ComponentTypeHandle<Spell1025DragonBreathData> __Spell1025DragonBreathData_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
					__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell1025FireGroundEffectBuffer>();
					__Spell1025DragonBreathData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1025DragonBreathData>();
				}

				public void Update(ref SystemState state)
				{
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
					__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle.Update(ref state);
					__Spell1025DragonBreathData_RW_ComponentTypeHandle.Update(ref state);
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
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellFallTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025FireGroundEffectBuffer>();
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
			public void Run(ref Spell1025FallGroundEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1025FallGroundEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1025FallGroundEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1025FallGroundEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1025FallGroundEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1025FallGroundEffectJob job, EntityManager entityManager)
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

		public float DeltaTime;

		public SpellSingleton SpellSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(in SpellConfigComponentData config, in SpellComponentData data, ref DynamicBuffer<Spell1025FireGroundEffectBuffer> fireGroundEffectBuffer, ref Spell1025DragonBreathData breathData, [ChunkIndexInQuery] int chunkIndex)
		{
			breathData.FallGroundEffectTimer -= DeltaTime;
			if (breathData.FallGroundEffectTimer > 0f)
			{
				return;
			}
			breathData.FallGroundEffectTimer = 0.1f;
			foreach (Spell1025FireGroundEffectBuffer item in fireGroundEffectBuffer)
			{
				Spell1025FireGroundEffectBuffer current = item;
				ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
				FixedString32Bytes effectName = "FallGround";
				cMD.CreateSpellGlobalParticle(chunkIndex, in effectName, in current.position, in breathData.FallDamageRange, in config, in data, in SpellSingleton, in float3.zero);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			BufferAccessor<Spell1025FireGroundEffectBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell1025FireGroundEffectBuffer_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1025DragonBreathData_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, i);
					DynamicBuffer<Spell1025FireGroundEffectBuffer> fireGroundEffectBuffer = bufferAccessor[i];
					Execute(in config, in data, ref fireGroundEffectBuffer, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr3, i), chunkIndexInQuery);
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
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, nextRangeBegin);
						DynamicBuffer<Spell1025FireGroundEffectBuffer> fireGroundEffectBuffer2 = bufferAccessor[nextRangeBegin];
						Execute(in config2, in data2, ref fireGroundEffectBuffer2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr3, nextRangeBegin), chunkIndexInQuery);
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
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, j);
					DynamicBuffer<Spell1025FireGroundEffectBuffer> fireGroundEffectBuffer3 = bufferAccessor[j];
					Execute(in config3, in data3, ref fireGroundEffectBuffer3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr3, j), chunkIndexInQuery);
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
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr2, k);
					DynamicBuffer<Spell1025FireGroundEffectBuffer> fireGroundEffectBuffer4 = bufferAccessor[k];
					Execute(in config4, in data4, ref fireGroundEffectBuffer4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1025DragonBreathData>(nativeArrayPtr3, k), chunkIndexInQuery);
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
	private readonly struct IFE_1905977874_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1025DragonBreathData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellComponentData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1025DragonBreathData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1025DragonBreathData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1025DragonBreathData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1905977874_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell1025DragonBreathData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1025DragonBreathData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1025DragonBreathData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1025DragonBreathData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1905977874_0.TypeHandle __IFE_1905977874_0_TypeHandle;

		public IFE_1905977874_1.TypeHandle __IFE_1905977874_1_TypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellRemoteShootTag> __SpellRemoteShootTag_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public Spell1025FallEffectUpdateJob.InternalCompilerQueryAndHandleData __Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle;

		public Spell1025NormalEffectUpdateJob.InternalCompilerQueryAndHandleData __Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle;

		public Spell1025FallGroundEffectJob.InternalCompilerQueryAndHandleData __Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1905977874_0_TypeHandle = new IFE_1905977874_0.TypeHandle(ref state);
			__IFE_1905977874_1_TypeHandle = new IFE_1905977874_1.TypeHandle(ref state);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__SpellRemoteShootTag_RO_ComponentLookup = state.GetComponentLookup<SpellRemoteShootTag>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1905977874_0;

	private EntityQuery __query_1905977874_1;

	private EntityQuery __query_1905977874_2;

	private EntityQuery __query_1905977874_3;

	private EntityQuery __query_1905977874_4;

	private EntityQuery __query_1905977874_5;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<Spell1025DragonBreathData>();
	}

	private bool TryGetLinkEffect(FixedString32Bytes name, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out GameObject linkedObject)
	{
		foreach (SpellGameObjectEffectLink item in linkBuffer)
		{
			SpellGameObjectEffectLink current = item;
			if (current.EffectName == name)
			{
				UnityObjectRef<GameObject> gameObject = current.GameObject;
				linkedObject = gameObject.Value;
				return true;
			}
		}
		linkedObject = null;
		return false;
	}

	public void OnDestroy(ref SystemState state)
	{
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSingleton spellSingleton = __query_1905977874_2.GetSingleton<SpellSingleton>();
		Entity requireEntity = __query_1905977874_3.GetSingletonEntity();
		EntityCommandBuffer.ParallelWriter parallelWriter = __query_1905977874_4.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		Entity entity;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> item9 in IFE_1905977874_0.Query(__query_1905977874_0, __TypeHandle.__IFE_1905977874_0_TypeHandle, ref state))
		{
			item9.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out entity);
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW5 = item5;
			Entity spellEntity = entity;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRO.Shooter) && !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellRemoteShootTag_RO_ComponentLookup, ref state, spellEntity) && !uncheckedRefRW3.ValueRW.IsFallSpell && uncheckedRefRW3.ValueRO.Type != SpellSpecialMovementType.Rotation)
			{
				ref UnitProperty_Dots valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRO.Shooter).ValueRW;
				float num = 1f;
				if (valueRW.unitCfg.IsSameCamp(UnitType.Player))
				{
					num = SpellShootGroupExtend.GetRecoilRatio(uncheckedRefRW.ValueRO.Wand).CurrentMulRatio;
				}
				valueRW.TakeKnockback(9f * state.WorldUnmanaged.Time.DeltaTime * -uncheckedRefRW3.ValueRO.Direction * num);
			}
			if (!uncheckedRefRW4.ValueRO.SpellEffectObj.Value)
			{
				InitializeSpellEffect(ref uncheckedRefRW4.ValueRW, ref uncheckedRefRW3.ValueRW, ref uncheckedRefRW.ValueRW, in uncheckedRefRW5.ValueRO, ref uncheckedRefRW2.ValueRW, in requireEntity, in spellSingleton, in spellEntity, InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, spellEntity));
			}
			if (!uncheckedRefRW3.ValueRO.IsFallSpell && (bool)uncheckedRefRW4.ValueRW.FireParticle && uncheckedRefRW3.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				uncheckedRefRW4.ValueRW.SpellEffectObj.Value.transform.position = uncheckedRefRW3.ValueRO.AroundCenter;
			}
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> item10 in IFE_1905977874_1.Query(__query_1905977874_1, __TypeHandle.__IFE_1905977874_1_TypeHandle, ref state))
		{
			item10.Deconstruct(out var _, out var item7, out var item8, out entity);
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO = item7;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO2 = item8;
			Entity spell = entity;
			if (uncheckedRefRO2.ValueRO.HoverDuration > 0f && uncheckedRefRO2.ValueRO.HoverTimer > 0f)
			{
				EntityManager em = state.EntityManager;
				SpellTools.StopKeepCasting(in em, in uncheckedRefRO.ValueRO.Shooter, in spell);
			}
		}
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1025FallEffectUpdateJob
		{
			TransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			cmd = parallelWriter
		}, __TypeHandle.__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Spell1025NormalEffectUpdateJob
		{
			SpellSingleton = __query_1905977874_2.GetSingleton<SpellSingleton>(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			cmd = parallelWriter,
			Random = __query_1905977874_5.GetSingleton<GlobalRandom>()
		}, __TypeHandle.__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_2(new Spell1025FallGroundEffectJob
		{
			SpellSingleton = __query_1905977874_2.GetSingleton<SpellSingleton>(),
			CMD = parallelWriter,
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime
		}, __TypeHandle.__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	private void InitializeSpellEffect(ref Spell1025DragonBreathData spell, ref SpellMovementComponentData movement, ref SpellComponentData data, in SpellConfigComponentData config, ref LocalTransform transform, in Entity requireEntity, in SpellSingleton spellSingleton, in Entity spellEntity, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer)
	{
		if (!movement.IsFallSpell && movement.Type == SpellSpecialMovementType.Rotation && TryGetLinkEffect("RotationSpell", linkBuffer, out var linkedObject))
		{
			spell.SpellEffectObj = linkedObject;
			float3 @float = DTool.IgnoreZPosition(in movement.AroundCenter, -1f);
			spell.SpellEffectObj.Value.transform.position = @float;
			Transform transform2 = spell.SpellEffectObj.Value.transform;
			spell.FireParticle = transform2.Find("Fire").GetComponent<ParticleSystem>();
			spell.SmokeParticle = transform2.Find("Smoke").GetComponent<ParticleSystem>();
			spell.EmberParticle = transform2.Find("Ember").GetComponent<ParticleSystem>();
			ParticleSystem.EmissionModule emission = spell.FireParticle.Value.emission;
			ParticleSystem.EmissionModule emission2 = spell.SmokeParticle.Value.emission;
			ParticleSystem.EmissionModule emission3 = spell.EmberParticle.Value.emission;
			ParticleSystem.ShapeModule shape = spell.FireParticle.Value.shape;
			ParticleSystem.ShapeModule shape2 = spell.SmokeParticle.Value.shape;
			ParticleSystem.ShapeModule shape3 = spell.EmberParticle.Value.shape;
			ParticleSystem.MainModule main = spell.FireParticle.Value.main;
			ParticleSystem.MainModule main2 = spell.SmokeParticle.Value.main;
			ParticleSystem.MainModule main3 = spell.EmberParticle.Value.main;
			float finalSpellTransparent = DataMgr.settingData.FinalSpellTransparent;
			finalSpellTransparent = Mathf.Pow(finalSpellTransparent, 2f * (finalSpellTransparent + 0.3f));
			float aroundRadius = movement.AroundRadius;
			int num = 1;
			main.startSize = 2f * (float)num;
			main2.startSize = 2f * (float)num;
			main3.startSize = 0.1f * (float)num;
			shape.radius = aroundRadius * (float)num;
			shape2.radius = aroundRadius * (float)num;
			shape3.radius = aroundRadius * (float)num;
			emission.rateOverTime = 120f * aroundRadius * (float)num * finalSpellTransparent;
			emission2.rateOverTime = 20f * aroundRadius * (float)num * finalSpellTransparent;
			emission3.rateOverTime = 70f * aroundRadius * (float)num * finalSpellTransparent;
			if (config.ColorType == SpellColorType.Void)
			{
				spell.VoidFireParticle = spell.SpellEffectObj.Value.transform.Find("FireVoid").GetComponent<ParticleSystem>();
				ParticleSystem.MainModule main4 = spell.VoidFireParticle.Value.main;
				ParticleSystem.EmissionModule emission4 = spell.VoidFireParticle.Value.emission;
				ParticleSystem.ShapeModule shape4 = spell.VoidFireParticle.Value.shape;
				main4.startSize = 2f * (float)num;
				shape4.radius = aroundRadius * (float)num;
				emission4.rateOverTime = 200f * aroundRadius * finalSpellTransparent * (float)num;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1025FallEffectUpdateJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1025FallEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Spell1025NormalEffectUpdateJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1025NormalEffectUpdateJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_2(Spell1025FallGroundEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1025DragonBreathEffectSystem_Spell1025FallGroundEffectJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025DragonBreathData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		__query_1905977874_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellKeepCastingAttach>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1025DragonBreathData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		__query_1905977874_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977874_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977874_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977874_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1905977874_5 = entityQueryBuilder2.Build(ref state);
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
		((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1025DragonBreathEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
