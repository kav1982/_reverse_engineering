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
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public struct Spell1016DashSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[WithAll(new Type[] { typeof(Spell1016DashData) })]
	[BurstCompile]
	public struct Spell1016FallJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<SpellGroundedTag> __SpellGroundedTag_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellGroundedTag_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellGroundedTag>(isReadOnly: true);
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__SpellGroundedTag_RO_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellGroundedTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1016DashData>();
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
			public void Run(ref Spell1016FallJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1016FallJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1016FallJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1016FallJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1016FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1016FallJob job, EntityManager entityManager)
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

		public Entity UnfollowingRequireEntity;

		public Entity ScreenShakeSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute([ChunkIndexInQuery] int chunkIndex, EnabledRefRO<SpellGroundedTag> groundedTag, in SpellConfigComponentData config, in LocalTransform transform)
		{
			if (groundedTag.ValueRO)
			{
				config.ColorType.ColorEnumToString(out var result);
				CMD.AppendToBuffer(chunkIndex, UnfollowingRequireEntity, new SpellEffectSystem.UnfollowingRequire
				{
					SpellId = (int)config.AbilityType,
					Color = result,
					StartPosition = transform.Position,
					Scale = config.Radius.Calculate(),
					Settings = new SpellEffect
					{
						Name = "Fall",
						DestroyDelay = 1f,
						Layer = LayerCorrectType.Coordinate
					}
				});
				CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
				{
					Radius = 0.1f,
					Speed = 1f,
					Time = 0.08f
				});
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			EnabledMask enabledMask = chunk.GetEnabledMask(ref __TypeHandle.__SpellGroundedTag_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr, i);
					Execute(transform: in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i), chunkIndex: chunkIndexInQuery, groundedTag: enabledMask.GetEnabledRefRO<SpellGroundedTag>(i), config: in config);
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
						Execute(transform: in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin), chunkIndex: chunkIndexInQuery, groundedTag: enabledMask.GetEnabledRefRO<SpellGroundedTag>(nextRangeBegin), config: in config2);
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
					Execute(transform: in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j), chunkIndex: chunkIndexInQuery, groundedTag: enabledMask.GetEnabledRefRO<SpellGroundedTag>(j), config: in config3);
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
					Execute(transform: in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k), chunkIndex: chunkIndexInQuery, groundedTag: enabledMask.GetEnabledRefRO<SpellGroundedTag>(k), config: in config4);
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
	private readonly struct IFE_573865035_0
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1016DashData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1016DashData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1016DashData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1016DashData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_573865035_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1016DashData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<PhysicsVelocity>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item5_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1016DashData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<PhysicsVelocity> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellComponentData> item5_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1016DashData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1016DashData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<PhysicsVelocity>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_573865035_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1016DirverCleanupData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1016DirverCleanupData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<PhysicsVelocity>(item4_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1016DirverCleanupData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<PhysicsVelocity> item4_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1016DirverCleanupData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1016DirverCleanupData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1016DirverCleanupData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1016DirverCleanupData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<PhysicsVelocity>();
		}
	}

	private struct TypeHandle
	{
		public IFE_573865035_0.TypeHandle __IFE_573865035_0_TypeHandle;

		public IFE_573865035_1.TypeHandle __IFE_573865035_1_TypeHandle;

		public IFE_573865035_2.TypeHandle __IFE_573865035_2_TypeHandle;

		public ComponentLookup<Spell1016InitTag> __Spell1016InitTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4004StartData> __Spell4004StartData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<GlobalParticle.Emitter> __GlobalParticle_Emitter_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RO_ComponentLookup;

		public ComponentLookup<GlobalParticle.Emitter> __GlobalParticle_Emitter_RW_ComponentLookup;

		public ComponentLookup<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RW_ComponentLookup;

		public Spell1016FallJob.InternalCompilerQueryAndHandleData __Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_573865035_0_TypeHandle = new IFE_573865035_0.TypeHandle(ref state);
			__IFE_573865035_1_TypeHandle = new IFE_573865035_1.TypeHandle(ref state);
			__IFE_573865035_2_TypeHandle = new IFE_573865035_2.TypeHandle(ref state);
			__Spell1016InitTag_RW_ComponentLookup = state.GetComponentLookup<Spell1016InitTag>();
			__Spell4004StartData_RO_ComponentLookup = state.GetComponentLookup<Spell4004StartData>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Spell4005WandSpiritData_RO_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__GlobalParticle_Emitter_RO_ComponentLookup = state.GetComponentLookup<GlobalParticle.Emitter>(isReadOnly: true);
			__GlobalParticle_EmitDistanceCounter_RO_ComponentLookup = state.GetComponentLookup<GlobalParticle.EmitDistanceCounter>(isReadOnly: true);
			__GlobalParticle_Emitter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.Emitter>();
			__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.EmitDistanceCounter>();
			__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_573865035_0;

	private EntityQuery __query_573865035_1;

	private EntityQuery __query_573865035_2;

	private EntityQuery __query_573865035_3;

	private EntityQuery __query_573865035_4;

	private EntityQuery __query_573865035_5;

	private EntityQuery __query_573865035_6;

	private EntityQuery __query_573865035_7;

	public void OnCreate(ref SystemState state)
	{
		Entity entity = state.EntityManager.CreateSingleton(new SpellDashDriverSingleton
		{
			OnDashDriver = new NativeHashSet<Entity>(8, Allocator.Persistent)
		});
		state.EntityManager.SetName(entity, "SpellDashDriverSingleton");
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellDashDriverSingleton>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<Spell1016DashData>();
	}

	public void OnDestroy(ref SystemState state)
	{
		state.Dependency.Complete();
		if (__query_573865035_3.HasSingleton<SpellDashDriverSingleton>())
		{
			SpellDashDriverSingleton singleton = __query_573865035_3.GetSingleton<SpellDashDriverSingleton>();
			if (singleton.OnDashDriver.IsCreated)
			{
				singleton.OnDashDriver.Dispose();
			}
		}
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer cmd = new EntityCommandBuffer(Allocator.TempJob);
		Entity singletonEntity = __query_573865035_4.GetSingletonEntity();
		Entity singletonEntity2 = __query_573865035_5.GetSingletonEntity();
		RefRW<SpellDashDriverSingleton> singletonRW = __query_573865035_6.GetSingletonRW<SpellDashDriverSingleton>();
		UpdateCleanupData(ref state, ref cmd);
		bool flag = false;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1016DashData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> item6 in IFE_573865035_0.Query(__query_573865035_0, __TypeHandle.__IFE_573865035_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1016DashData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW5 = item5;
			Entity entity2 = entity;
			if (uncheckedRefRW5.ValueRO.IsFallSpell)
			{
				continue;
			}
			uncheckedRefRW2.ValueRW.Scale = uncheckedRefRW4.ValueRO.Radius.Calculate();
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell1016InitTag_RW_ComponentLookup, ref state, entity2, value: false);
			Entity entity3 = uncheckedRefRW3.ValueRO.Shooter;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4004StartData_RO_ComponentLookup, ref state, entity3))
			{
				entity3 = uncheckedRefRW3.ValueRO.OwnerEntity;
			}
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, entity3))
			{
				continue;
			}
			RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, entity3);
			bool flag2 = InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RO_ComponentLookup, ref state, entity3);
			UnitType unitType = componentRWAfterCompletingDependency.ValueRO.unitCfg.unitType;
			if ((unitType != 0 && unitType != UnitType.Teammate && unitType != UnitType.Monster && !flag2) || singletonRW.ValueRW.IsShooterDriving(entity3) || (componentRWAfterCompletingDependency.ValueRO.unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.PlayerCtrller.isDashOverHeat))
			{
				continue;
			}
			singletonRW.ValueRW.ShooterDrive(entity3);
			if (componentRWAfterCompletingDependency.ValueRO.unitCfg.unitType == UnitType.Player)
			{
				singletonRW.ValueRW.SetDashTime(uncheckedRefRW4.ValueRW.Duration.Calculate());
			}
			componentRWAfterCompletingDependency.ValueRW.FlyRegister();
			componentRWAfterCompletingDependency.ValueRW.InvincibleRegister();
			cmd.AddComponent<Spell1016DirverCleanupData>(entity2);
			cmd.SetComponent(entity2, new Spell1016DirverCleanupData
			{
				Dirver = entity3,
				ColorType = uncheckedRefRW4.ValueRO.ColorType
			});
			uncheckedRefRW.ValueRW.Dirver = entity3;
			if (componentRWAfterCompletingDependency.ValueRO.unitCfg.unitType == UnitType.Player)
			{
				flag = true;
				if (uncheckedRefRW5.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					CamController.Inst.playerInRotateDash = true;
				}
			}
			if (uncheckedRefRW5.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				float3 position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity3).Position;
				uncheckedRefRW5.ValueRW.AroundCenter = position;
				CamController.Inst.dashCamFollowPoint = uncheckedRefRW5.ValueRW.AroundCenter;
				uncheckedRefRW5.ValueRW.AroundTarget = Entity.Null;
			}
		}
		if (flag)
		{
			__query_573865035_7.GetSingleton<PlayerController_Dots>().playerCtrllerMono.Value.OnPlayerDash();
		}
		foreach (var (uncheckedRefRW6, uncheckedRefRW7, uncheckedRefRW8, uncheckedRefRO, uncheckedRefRW9) in IFE_573865035_1.Query(__query_573865035_1, __TypeHandle.__IFE_573865035_1_TypeHandle, ref state))
		{
			if (uncheckedRefRW7.ValueRO.IsFallSpell || uncheckedRefRW6.ValueRO.Dirver == Entity.Null || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW6.ValueRO.Dirver))
			{
				continue;
			}
			RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW6.ValueRO.Dirver);
			float3 position2 = DTool.IgnoreZPosition(in uncheckedRefRW8.ValueRO.Position, componentRWAfterCompletingDependency2.ValueRW.Position.z);
			RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, uncheckedRefRW6.ValueRO.Dirver);
			if (uncheckedRefRW6.ValueRW.AcceessTheme6StopTrail && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_Emitter_RO_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RO_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity))
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_Emitter_RW_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity, value: true);
				GlobalParticle.EmitDistanceCounter componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RO_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity);
				componentAfterCompletingDependency.lastEmitPoint = uncheckedRefRW8.ValueRO.Position;
				componentAfterCompletingDependency.lastCountPoint = uncheckedRefRW8.ValueRO.Position;
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup, ref state, componentAfterCompletingDependency, uncheckedRefRW9.ValueRW.SpellEffectEntity);
				uncheckedRefRW6.ValueRW.AcceessTheme6StopTrail = false;
			}
			if (componentRWAfterCompletingDependency3.ValueRO.unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.inDashSpellAccessT6)
			{
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_Emitter_RO_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity))
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_Emitter_RW_ComponentLookup, ref state, uncheckedRefRW9.ValueRW.SpellEffectEntity, value: false);
					uncheckedRefRW6.ValueRW.AcceessTheme6StopTrail = true;
				}
				float3 position3 = DTool.IgnoreZPosition(in componentRWAfterCompletingDependency2.ValueRW.Position, componentRWAfterCompletingDependency2.ValueRW.Position.z);
				uncheckedRefRW8.ValueRW.Position = position3;
				PlayerMgr.Inst.inDashSpellAccessT6 = false;
			}
			else
			{
				componentRWAfterCompletingDependency2.ValueRW.Position = position2;
			}
			if (math.length(uncheckedRefRO.ValueRO.Linear) > 0f)
			{
				uncheckedRefRW6.ValueRW.LastLinear = uncheckedRefRO.ValueRO.Linear;
			}
			if (componentRWAfterCompletingDependency3.ValueRO.unitCfg.unitType != 0)
			{
				continue;
			}
			if (GameMgr.IsMobile_Static && uncheckedRefRW7.ValueRW.Type == SpellSpecialMovementType.ChaseMouse)
			{
				if (!uncheckedRefRW6.ValueRW.PauseMouseEffect && PlayerMgr.Inst.PlayerCtrller.inputLeftStick != Vector2.zero)
				{
					uncheckedRefRW7.ValueRW.Type = SpellSpecialMovementType.Normal;
					uncheckedRefRW6.ValueRW.PauseMouseEffect = true;
				}
				else if (uncheckedRefRW6.ValueRW.PauseMouseEffect && PlayerMgr.Inst.PlayerCtrller.inputLeftStick == Vector2.zero)
				{
					uncheckedRefRW7.ValueRW.Type = SpellSpecialMovementType.ChaseMouse;
					uncheckedRefRW6.ValueRW.PauseMouseEffect = false;
				}
			}
			Vector2 vector = ((PlayerMgr.Inst.PlayerCtrller.inputLeftStick != Vector2.zero) ? PlayerMgr.Inst.PlayerCtrller.inputLeftStick : ((Vector2)PlayerMgr.Inst.PlayerCtrller.inputRightStick));
			Vector2 vector2 = (GameMgr.IsMobile_Static ? vector : ControlMgr.Inst.GetInputWASD());
			if (!(vector2.sqrMagnitude <= 0f))
			{
				PlayerController_Dots singleton = __query_573865035_7.GetSingleton<PlayerController_Dots>();
				vector2 *= singleton.playerCtrllerMono.Value.myPpt.MoveSpeedRatio;
				if (uncheckedRefRW7.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					uncheckedRefRW7.ValueRW.AroundCenter += new float3(vector2.x, vector2.y, 0f) * state.WorldUnmanaged.Time.DeltaTime * singleton.playerCtrllerMono.Value.myPpt.MoveSpeed;
					CamController.Inst.dashCamFollowPoint = uncheckedRefRW7.ValueRW.AroundCenter;
				}
				else
				{
					vector2 = math.normalize(vector2);
					uncheckedRefRW7.ValueRW.Direction = new float3(vector2.x, vector2.y, uncheckedRefRW7.ValueRW.Direction.z);
				}
			}
		}
		__ScheduleViaJobChunkExtension_0(new Spell1016FallJob
		{
			CMD = cmd.AsParallelWriter(),
			ScreenShakeSingleton = singletonEntity2,
			UnfollowingRequireEntity = singletonEntity
		}, __TypeHandle.__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		cmd.Playback(state.EntityManager);
		cmd.Dispose();
	}

	private void UpdateCleanupData(ref SystemState state, ref EntityCommandBuffer cmd)
	{
		foreach (var (uncheckedRefRW, uncheckedRefRO, uncheckedRefRO2, uncheckedRefRO3) in IFE_573865035_2.Query(__query_573865035_2, __TypeHandle.__IFE_573865035_2_TypeHandle, ref state))
		{
			uncheckedRefRW.ValueRW.DashTimer = uncheckedRefRO2.ValueRO.DurationTimer;
			uncheckedRefRW.ValueRW.Radius = uncheckedRefRO2.ValueRO.Radius.Calculate();
			uncheckedRefRW.ValueRW.LastPosition = uncheckedRefRO.ValueRO.Position;
			uncheckedRefRW.ValueRW.LastLinear = uncheckedRefRO3.ValueRO.Linear;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1016FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1016DashSystem_Spell1016FallJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1016InitTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1016DashData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		__query_573865035_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1016DashData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_573865035_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1016DirverCleanupData>();
		__query_573865035_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDashDriverSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_573865035_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_573865035_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_573865035_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellDashDriverSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_573865035_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_573865035_7 = entityQueryBuilder2.Build(ref state);
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
		((Spell1016DashSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1016DashSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell1016DashSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1016DashSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
