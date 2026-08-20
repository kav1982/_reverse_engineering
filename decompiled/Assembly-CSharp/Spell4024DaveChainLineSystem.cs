using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[UpdateAfter(typeof(SpellEffectSystem))]
[CompilerGenerated]
public class Spell4024DaveChainLineSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2047054128_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<HarpoonChainData> item3_BufferAccessor;

			public IntPtr item4_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<HarpoonChainData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4024DaveHarpoonData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item4_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4024DaveHarpoonData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<HarpoonChainData> item3_BufferTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4024DaveHarpoonData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<HarpoonChainData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<HarpoonChainData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<HarpoonChainData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4024DaveHarpoonData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<HarpoonChainData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2047054128_0.TypeHandle __IFE_2047054128_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2047054128_0_TypeHandle = new IFE_2047054128_0.TypeHandle(ref state);
		}
	}

	public static Dictionary<GameObject, Spell4024DaveHarpoonChainCtrl> LineCostomCache = new Dictionary<GameObject, Spell4024DaveHarpoonChainCtrl>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2047054128_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4024DaveHarpoonData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var item5 in IFE_2047054128_0.Query(__query_2047054128_0, __TypeHandle.__IFE_2047054128_0_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<Spell4024DaveHarpoonData> item = item5.Item1;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> item2 = item5.Item2;
			DynamicBuffer<HarpoonChainData> item3 = item5.Item3;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> item4 = item5.Item4;
			UnityObjectRef<GameObject> trailEffectGameObject = item2.ValueRO.TrailEffectGameObject;
			if (!(trailEffectGameObject.Value == null))
			{
				if (!LineCostomCache.TryGetValue(trailEffectGameObject, out var value))
				{
					value = trailEffectGameObject.Value.GetComponent<Spell4024DaveHarpoonChainCtrl>();
					LineCostomCache[trailEffectGameObject] = value;
				}
				int num = 0;
				if (item.ValueRO.ChainState == ChainState.Returning)
				{
					num = 1;
				}
				Vector3[] array = new Vector3[item3.Length - num + 1];
				Vector3[] array2 = new Vector3[item3.Length - num + 1];
				for (int i = 0; i < item3.Length - num; i++)
				{
					HarpoonChainData harpoonChainData = item3[i];
					float3 layerPosition = DTool.GetLayerPosition(in harpoonChainData.Position, LayerCorrectType.Coordinate);
					array[i] = layerPosition + item3[i].Position;
					float3 rootPosition = new float3(item3[i].Position.xy, 0f);
					float3 layerPosition2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					array2[i] = layerPosition2 + new float3(item3[i].Position.xy, 0f);
				}
				if (array.Length >= 1)
				{
					float3 layerPosition = DTool.GetLayerPosition(in item4.ValueRO.Position, LayerCorrectType.Coordinate);
					float3 rootPosition = new float3(item4.ValueRO.Position.xy, 0f);
					float3 layerPosition2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					array[^1] = item4.ValueRO.Position + layerPosition;
					array2[^1] = new float3(item4.ValueRO.Position.xy, 0f) + layerPosition2;
				}
				else
				{
					Debug.Log("线点数为0");
				}
				value.SetLinePos(array, 0);
				value.SetLinePos(array2, 1);
				if (item.ValueRW.GateEffect.IsValid())
				{
					float3 layerPosition3 = DTool.GetLayerPosition(in item.ValueRW.StartPos, LayerCorrectType.Coordinate);
					item.ValueRW.GateEffect.Value.transform.position = layerPosition3 + item.ValueRW.StartPos;
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
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4024DaveHarpoonData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<HarpoonChainData>();
		__query_2047054128_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4024DaveChainLineSystem()
	{
	}
}
