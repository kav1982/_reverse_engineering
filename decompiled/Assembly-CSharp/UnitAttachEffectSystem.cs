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

[BurstCompile]
[UpdateAfter(typeof(UnitEnvironmentSystem))]
[UpdateBefore(typeof(UnitBeforeTakeDamageSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[CompilerGenerated]
internal struct UnitAttachEffectSystem : ISystem, ISystemCompilerGenerated
{
	public struct UnitAttachEffectTag : IComponentData, IQueryTypeParameter
	{
		public Entity master;
	}

	public struct CreateAttachEffectRequest : IBufferElementData
	{
		public Entity master;

		public Vector3 pos;

		public float size;

		public UnitEnvironmentSystem.DamageEffectType effectType;
	}

	[CompilerGenerated]
	public struct UnitAttachEffectJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				DefaultQuery = entityQueryBuilder.WithAllRW<UnitProperty_Dots>().Build(ref state);
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
			public void Run(ref UnitAttachEffectJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref UnitAttachEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref UnitAttachEffectJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref UnitAttachEffectJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref UnitAttachEffectJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref UnitAttachEffectJob job, EntityManager entityManager)
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

		[NativeDisableParallelForRestriction]
		public EntityCommandBuffer.ParallelWriter ecb;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> tsfLookUp;

		public Entity createEffetRequestBufferEtt;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref UnitProperty_Dots ppt, Entity entity)
		{
			if (ppt.unitCfg.unitType == UnitType.Brittleness)
			{
				return;
			}
			RefRW<LocalTransform> refRW = tsfLookUp.GetRefRW(entity);
			ref LocalTransform valueRW = ref refRW.ValueRW;
			for (int i = 0; i < 6; i++)
			{
				if (i != 2)
				{
					continue;
				}
				ref Entity mucusEF = ref ppt.mucusEF;
				bool flag = false;
				if (i == 2)
				{
					mucusEF = ref ppt.mucusEF;
					flag = ppt.affect_IsMucusDecelerate && ppt.showAffect;
				}
				if (mucusEF == Entity.Null || !tsfLookUp.HasComponent(mucusEF))
				{
					if (flag)
					{
						ecb.AppendToBuffer(index, createEffetRequestBufferEtt, new CreateAttachEffectRequest
						{
							master = entity,
							pos = Tool2D.GetLayerPoint(valueRW.Position) + new Vector3(0f, 0f, -0.02f),
							size = 0.5f,
							effectType = (UnitEnvironmentSystem.DamageEffectType)i
						});
					}
				}
				else if (i == 2)
				{
					if (flag)
					{
						LocalTransform localTransform = LocalTransform.FromPosition(Tool2D.GetLayerPoint(valueRW.Position) + new Vector3(0f, 0f, -0.02f));
						localTransform.Scale = 0.5f;
						refRW = tsfLookUp.GetRefRW(mucusEF);
						refRW.ValueRW = localTransform;
					}
					else
					{
						ecb.DestroyEntity(index, mucusEF);
						mucusEF = Entity.Null;
					}
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref UnitProperty_Dots ppt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(chunkIndexInQuery, ref ppt, entity);
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
						ref UnitProperty_Dots ppt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(chunkIndexInQuery, ref ppt2, entity2);
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
					ref UnitProperty_Dots ppt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(chunkIndexInQuery, ref ppt3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref UnitProperty_Dots ppt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(chunkIndexInQuery, ref ppt4, entity4);
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
	private readonly struct IFE_183210482_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<UnitProperty_Dots> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_183210482_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<UnitEnvironmentSystem.ContinueEffectRef> Get(int index)
			{
				return new QueryEnumerableWithEntity<UnitEnvironmentSystem.ContinueEffectRef>(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<UnitEnvironmentSystem.ContinueEffectRef>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<UnitEnvironmentSystem.ContinueEffectRef> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitEnvironmentSystem.ContinueEffectRef>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<UnitEnvironmentSystem.ContinueEffectRef>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<UnitEnvironmentSystem.ContinueEffectRef> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<UnitEnvironmentSystem.ContinueEffectRef>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_183210482_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<UnitAttachEffectTag> Get(int index)
			{
				return new QueryEnumerableWithEntity<UnitAttachEffectTag>(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<UnitAttachEffectTag>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<UnitAttachEffectTag> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitAttachEffectTag>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<UnitAttachEffectTag>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<UnitAttachEffectTag> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<UnitAttachEffectTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_183210482_0.TypeHandle __IFE_183210482_0_TypeHandle;

		public IFE_183210482_1.TypeHandle __IFE_183210482_1_TypeHandle;

		public IFE_183210482_2.TypeHandle __IFE_183210482_2_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_183210482_0_TypeHandle = new IFE_183210482_0.TypeHandle(ref state);
			__IFE_183210482_1_TypeHandle = new IFE_183210482_1.TypeHandle(ref state);
			__IFE_183210482_2_TypeHandle = new IFE_183210482_2.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnDestroy_0000925F_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_0000925F_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_0000925F_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private EntityQuery effectRefQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_183210482_0;

	private EntityQuery __query_183210482_1;

	private EntityQuery __query_183210482_2;

	private EntityQuery __query_183210482_3;

	public void OnCreate(ref SystemState state)
	{
		effectRefQuery = state.EntityManager.CreateEntityQuery(typeof(UnitEnvironmentSystem.ContinueEffectRef));
		state.EntityManager.CreateSingletonBuffer<CreateAttachEffectRequest>();
		state.RequireForUpdate<SpellSingleton>();
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSingleton singleton = __query_183210482_3.GetSingleton<SpellSingleton>();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		Entity entity;
		UnitAttachEffectTag item4;
		UnitEnvironmentSystem.ContinueEffectRef item5;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item6 in IFE_183210482_0.Query(__query_183210482_0, __TypeHandle.__IFE_183210482_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out entity);
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item2;
			Entity entity2 = entity;
			ref UnitProperty_Dots valueRW = ref uncheckedRefRW.ValueRW;
			if (valueRW.unitCfg.unitType == UnitType.Brittleness)
			{
				continue;
			}
			Vector3 vector = uncheckedRefRO.ValueRO.Position;
			ref Entity reference = ref valueRW.mucusEF;
			bool flag = false;
			string path = UnitEnvironmentSystem.path_Weaken;
			for (int i = 0; i < 6; i++)
			{
				switch (i)
				{
				case 0:
					reference = ref valueRW.weakenEF;
					path = UnitEnvironmentSystem.path_Weaken;
					if (GameMgr.IsHarmony_Static)
					{
						path = UnitEnvironmentSystem.path_Weaken_H;
					}
					flag = valueRW.damageReciveIncresePercentTimer > 0f && valueRW.showAffect;
					break;
				case 1:
					reference = ref valueRW.frozenEF;
					path = UnitEnvironmentSystem.path_Frozen;
					flag = valueRW.FronzenState == UnitProperty.Affect_FrozenState.Frozening && valueRW.showAffect;
					break;
				case 2:
					reference = ref valueRW.mucusEF;
					path = UnitEnvironmentSystem.path_Mucus;
					flag = valueRW.affect_IsMucusDecelerate && valueRW.showAffect;
					break;
				case 3:
					reference = ref valueRW.reverseEF;
					path = UnitEnvironmentSystem.path_Reverse;
					flag = valueRW.affect_IsReverseMove && valueRW.showAffect;
					break;
				case 4:
				{
					reference = ref valueRW.voidEF;
					path = UnitEnvironmentSystem.path_Void;
					UnitProperty_Dots unitProperty_Dots = valueRW;
					flag = unitProperty_Dots.voidEffectTimer > 0f && !unitProperty_Dots.voidExplosionData.ConstVoidEffect && unitProperty_Dots.showAffect;
					break;
				}
				case 5:
					reference = ref valueRW.BlueRuneWeakenEF;
					path = UnitEnvironmentSystem.path_WeakenBlueRune;
					flag = valueRW.BlueRuneTakeDamageIncreaseRatio > 0f && valueRW.showAffect;
					break;
				}
				if (reference == Entity.Null || !state.EntityManager.Exists(reference))
				{
					if (!flag)
					{
						continue;
					}
					if (i == 2)
					{
						if (singleton.Prefabs.TryGetValue("3004_Decelerate", out var item3))
						{
							Entity entity3 = entityCommandBuffer.Instantiate(item3);
							LocalTransform component = LocalTransform.FromPosition(Tool2D.GetLayerPoint(vector) + new Vector3(0f, 0f, -0.02f));
							component.Scale = 0.5f;
							entityCommandBuffer.SetComponent(entity3, component);
							reference = entity3;
							entityCommandBuffer.SetComponent(entity2, valueRW);
							item4 = new UnitAttachEffectTag
							{
								master = entity2
							};
							entityCommandBuffer.AddComponent(entity3, item4);
						}
						continue;
					}
					Entity entity4 = entityCommandBuffer.CreateEntity();
					UnitEnvironmentSystem.ContinueEffectRef component2 = default(UnitEnvironmentSystem.ContinueEffectRef);
					component2.entity = entity2;
					component2.beforeFadeTime = 0.5f;
					ParticleSystem component3 = ObjPoolMgr.Inst.GetGO(path, vector).GetComponent<ParticleSystem>();
					component2.obj.Value = component3;
					if (i == 1 || i == 4)
					{
						component3.transform.localScale = new Vector3(Mathf.Max(1f, valueRW.size), Mathf.Max(1f, valueRW.size), 1f);
					}
					entityCommandBuffer.AddComponent(entity4, component2);
					reference = entity4;
					entityCommandBuffer.SetComponent(entity2, valueRW);
					continue;
				}
				if (i == 2)
				{
					if (flag)
					{
						LocalTransform component4 = LocalTransform.FromPosition(Tool2D.GetLayerPoint(vector) + new Vector3(0f, 0f, -0.02f));
						component4.Scale = 0.5f;
						entityCommandBuffer.SetComponent(reference, component4);
					}
					else
					{
						entityCommandBuffer.DestroyEntity(reference);
						reference = Entity.Null;
					}
					continue;
				}
				UnitEnvironmentSystem.ContinueEffectRef componentData = state.EntityManager.GetComponentData<UnitEnvironmentSystem.ContinueEffectRef>(reference);
				if (!componentData.obj.IsValid())
				{
					entityCommandBuffer.DestroyEntity(reference);
					reference = Entity.Null;
					continue;
				}
				componentData.obj.Value.transform.position = Tool2D.GetLayerPoint(vector) + new Vector3(0f, 0f, -0.02f);
				if (flag)
				{
					componentData.beforeFadeTime = 0.5f;
					if (!componentData.particlePlaying)
					{
						item5 = state.EntityManager.GetComponentData<UnitEnvironmentSystem.ContinueEffectRef>(reference);
						item5.obj.Value.Play();
						componentData.particlePlaying = true;
					}
					entityCommandBuffer.SetComponent(reference, componentData);
					continue;
				}
				if (componentData.particlePlaying)
				{
					item5 = state.EntityManager.GetComponentData<UnitEnvironmentSystem.ContinueEffectRef>(reference);
					item5.obj.Value.Stop();
					componentData.particlePlaying = false;
				}
				componentData.beforeFadeTime -= state.WorldUnmanaged.Time.DeltaTime;
				entityCommandBuffer.SetComponent(reference, componentData);
				if (componentData.beforeFadeTime < 0f)
				{
					item5 = state.EntityManager.GetComponentData<UnitEnvironmentSystem.ContinueEffectRef>(reference);
					ParticleSystem value = item5.obj.Value;
					ObjPoolMgr.Inst.RecycleGO(value.gameObject);
					entityCommandBuffer.DestroyEntity(reference);
					reference = Entity.Null;
				}
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		if (!effectRefQuery.IsEmpty)
		{
			entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
			foreach (QueryEnumerableWithEntity<UnitEnvironmentSystem.ContinueEffectRef> item7 in IFE_183210482_1.Query(__query_183210482_1, __TypeHandle.__IFE_183210482_1_TypeHandle, ref state))
			{
				item7.Deconstruct(out item5, out entity);
				UnitEnvironmentSystem.ContinueEffectRef continueEffectRef = item5;
				Entity e = entity;
				if (!state.EntityManager.Exists(continueEffectRef.entity))
				{
					UnityObjectRef<ParticleSystem> obj = continueEffectRef.obj;
					if (obj.IsValid())
					{
						ObjPoolMgr inst = ObjPoolMgr.Inst;
						obj = continueEffectRef.obj;
						inst.RecycleGO(obj.Value.gameObject);
					}
					entityCommandBuffer.DestroyEntity(e);
				}
			}
			entityCommandBuffer.Playback(state.EntityManager);
			entityCommandBuffer.Dispose();
		}
		entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		foreach (QueryEnumerableWithEntity<UnitAttachEffectTag> item8 in IFE_183210482_2.Query(__query_183210482_2, __TypeHandle.__IFE_183210482_2_TypeHandle, ref state))
		{
			item8.Deconstruct(out item4, out entity);
			UnitAttachEffectTag unitAttachEffectTag = item4;
			Entity e2 = entity;
			if (!state.EntityManager.Exists(unitAttachEffectTag.master))
			{
				entityCommandBuffer.DestroyEntity(e2);
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_183210482_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitEnvironmentSystem.ContinueEffectRef>();
		__query_183210482_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitAttachEffectTag>();
		__query_183210482_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_183210482_3 = entityQueryBuilder2.Build(ref state);
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
		((UnitAttachEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((UnitAttachEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_0000925F_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((UnitAttachEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((UnitAttachEffectSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
