using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[CompilerGenerated]
internal class Spell2004PillarParticleChangeSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1609050440_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (Spell2004NoRotationTag, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Parent) Get(int index)
			{
				return (default(Spell2004NoRotationTag), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Parent>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<Parent> item3_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Parent>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(Spell2004NoRotationTag, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Parent)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (Spell2004NoRotationTag, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Parent) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<Parent>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1609050440_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<Spell2004PillarBuffer> item3_BufferAccessor;

			public BufferAccessor<Spell2004WallBuffer> item4_BufferAccessor;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (Spell2004PillarOfLightData, TeammateData, DynamicBuffer<Spell2004PillarBuffer>, DynamicBuffer<Spell2004WallBuffer>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Spell2004PillarOfLightData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<TeammateData>(item2_IntPtr, index), item3_BufferAccessor[index], item4_BufferAccessor[index]);
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell2004PillarOfLightData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<TeammateData> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<Spell2004PillarBuffer> item3_BufferTypeHandle_RW;

			private BufferTypeHandle<Spell2004WallBuffer> item4_BufferTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2004PillarOfLightData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<Spell2004PillarBuffer>();
				item4_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<Spell2004WallBuffer>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				item4_BufferTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.item4_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item4_BufferTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(Spell2004PillarOfLightData, TeammateData, DynamicBuffer<Spell2004PillarBuffer>, DynamicBuffer<Spell2004WallBuffer>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (Spell2004PillarOfLightData, TeammateData, DynamicBuffer<Spell2004PillarBuffer>, DynamicBuffer<Spell2004WallBuffer>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell2004PillarOfLightData>();
			state.EntityManager.CompleteDependencyBeforeRO<TeammateData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell2004PillarBuffer>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell2004WallBuffer>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1609050440_0.TypeHandle __IFE_1609050440_0_TypeHandle;

		public IFE_1609050440_1.TypeHandle __IFE_1609050440_1_TypeHandle;

		public ComponentLookup<Spell2004RotateAngleMaterialProperty> __Spell2004RotateAngleMaterialProperty_RW_ComponentLookup;

		public ComponentLookup<Spell2004HpRatioMaterialProperty> __Spell2004HpRatioMaterialProperty_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public Spell2004UpdatePillarJob.InternalCompilerQueryAndHandleData __Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1609050440_0_TypeHandle = new IFE_1609050440_0.TypeHandle(ref state);
			__IFE_1609050440_1_TypeHandle = new IFE_1609050440_1.TypeHandle(ref state);
			__Spell2004RotateAngleMaterialProperty_RW_ComponentLookup = state.GetComponentLookup<Spell2004RotateAngleMaterialProperty>();
			__Spell2004HpRatioMaterialProperty_RW_ComponentLookup = state.GetComponentLookup<Spell2004HpRatioMaterialProperty>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1609050440_0;

	private EntityQuery __query_1609050440_1;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell2004PillarBuffer>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		base.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2004UpdatePillarJob
		{
			angleMatLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell2004RotateAngleMaterialProperty_RW_ComponentLookup, ref base.CheckedStateRef),
			hpRatioMatLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell2004HpRatioMaterialProperty_RW_ComponentLookup, ref base.CheckedStateRef),
			effectCollectorLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref base.CheckedStateRef),
			DeltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime,
			transformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef)
		}, __TypeHandle.__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
		base.Dependency.Complete();
		foreach (var item5 in IFE_1609050440_0.Query(__query_1609050440_0, __TypeHandle.__IFE_1609050440_0_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> item = item5.Item2;
			Parent item2 = item5.Item3;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item2.Value))
			{
				quaternion rotation = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, item2.Value).Rotation;
				item.ValueRW.Rotation = math.conjugate(rotation);
			}
		}
		foreach (var item6 in IFE_1609050440_1.Query(__query_1609050440_1, __TypeHandle.__IFE_1609050440_1_TypeHandle, ref base.CheckedStateRef))
		{
			TeammateData item3 = item6.Item2;
			DynamicBuffer<Spell2004WallBuffer> item4 = item6.Item4;
			foreach (Spell2004WallBuffer item7 in item4)
			{
				float3 position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref base.CheckedStateRef, item7.Entity).Position;
				if (!item3.IsFuseMaterial)
				{
					UnityObjectRef<LineRenderer> lineRenderer = item7.LineRenderer;
					lineRenderer.Value.SetPosition(0, position + item7.WallDir * item7.WallDistance);
					lineRenderer = item7.LineRenderer;
					lineRenderer.Value.SetPosition(1, position - item7.WallDir * item7.WallDistance);
				}
				else
				{
					UnityObjectRef<LineRenderer> lineRenderer = item7.LineRenderer;
					lineRenderer.Value.SetPosition(0, new Vector3(9999f, 9999f, 9999f));
					lineRenderer = item7.LineRenderer;
					lineRenderer.Value.SetPosition(1, new Vector3(9999f, 9999f, 9999f));
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2004UpdatePillarJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2004UpdatePillarJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Parent>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004NoRotationTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		__query_1609050440_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2004PillarOfLightData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004WallBuffer>();
		__query_1609050440_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public Spell2004PillarParticleChangeSystem()
	{
	}
}
