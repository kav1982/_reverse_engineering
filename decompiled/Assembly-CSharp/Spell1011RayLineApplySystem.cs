using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
public class Spell1011RayLineApplySystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_870909054_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<DisintegrationRayBodyPoint> item3_BufferAccessor;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<DisintegrationRayBodyPoint>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1011DisintegrationRayData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), item3_BufferAccessor[index]);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1011DisintegrationRayData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<DisintegrationRayBodyPoint> item3_BufferTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1011DisintegrationRayData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<DisintegrationRayBodyPoint>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<DisintegrationRayBodyPoint>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1011DisintegrationRayData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<DisintegrationRayBodyPoint>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1011DisintegrationRayData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<DisintegrationRayBodyPoint>();
		}
	}

	private struct TypeHandle
	{
		public IFE_870909054_0.TypeHandle __IFE_870909054_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_870909054_0_TypeHandle = new IFE_870909054_0.TypeHandle(ref state);
		}
	}

	private static Dictionary<GameObject, Spell1011RayCustomCtrl> LineCostomCache = new Dictionary<GameObject, Spell1011RayCustomCtrl>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_870909054_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1011DisintegrationRayData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var item4 in IFE_870909054_0.Query(__query_870909054_0, __TypeHandle.__IFE_870909054_0_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<Spell1011DisintegrationRayData> item = item4.Item1;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> item2 = item4.Item2;
			DynamicBuffer<DisintegrationRayBodyPoint> item3 = item4.Item3;
			UnityObjectRef<GameObject> trailEffectGameObject = item2.ValueRO.TrailEffectGameObject;
			if (!(trailEffectGameObject.Value == null))
			{
				if (!LineCostomCache.TryGetValue(trailEffectGameObject, out var value))
				{
					value = trailEffectGameObject.Value.GetComponent<Spell1011RayCustomCtrl>();
					LineCostomCache[trailEffectGameObject] = value;
				}
				Vector3[] array = new Vector3[item3.Length];
				Vector3[] array2 = new Vector3[item3.Length];
				for (int i = 0; i < item3.Length; i++)
				{
					DisintegrationRayBodyPoint disintegrationRayBodyPoint = item3[i];
					float3 layerPosition = DTool.GetLayerPosition(in disintegrationRayBodyPoint.Value, LayerCorrectType.Coordinate);
					array[i] = layerPosition + item3[i].Value;
					float3 rootPosition = new float3(item3[i].Value.xy, 0f);
					float3 layerPosition2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					array2[i] = layerPosition2 + new float3(item3[i].Value.xy, 0f);
				}
				value.SetLinePos(array, 0);
				value.SetLinePos(array2, 1);
				if (array.Length != 0)
				{
					value.SetNode(array[0], 0);
				}
				if (item.ValueRO.IsHideLine)
				{
					value.HideLine();
				}
			}
		}
	}

	[Preserve]
	protected override void OnDestroy()
	{
		LineCostomCache.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1011DisintegrationRayData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<DisintegrationRayBodyPoint>();
		__query_870909054_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1011RayLineApplySystem()
	{
	}
}
