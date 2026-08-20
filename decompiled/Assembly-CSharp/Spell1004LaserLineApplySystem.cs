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

[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[CompilerGenerated]
public class Spell1004LaserLineApplySystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1808877836_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<LaserBodyPoint> item2_BufferAccessor;

			public BufferAccessor<SpellGameObjectEffectLink> item3_BufferAccessor;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1004LaserData>, DynamicBuffer<LaserBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1004LaserData>(item1_IntPtr, index), item2_BufferAccessor[index], item3_BufferAccessor[index]);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1004LaserData> item1_ComponentTypeHandle_RW;

			private BufferTypeHandle<LaserBodyPoint> item2_BufferTypeHandle_RW;

			private BufferTypeHandle<SpellGameObjectEffectLink> item3_BufferTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1004LaserData>();
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<LaserBodyPoint>();
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellGameObjectEffectLink>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1004LaserData>, DynamicBuffer<LaserBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1004LaserData>, DynamicBuffer<LaserBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1004LaserData>();
			state.EntityManager.CompleteDependencyBeforeRW<LaserBodyPoint>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellGameObjectEffectLink>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1808877836_0.TypeHandle __IFE_1808877836_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1808877836_0_TypeHandle = new IFE_1808877836_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1808877836_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1004LaserData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var (uncheckedRefRW, dynamicBuffer, effects) in IFE_1808877836_0.Query(__query_1808877836_0, __TypeHandle.__IFE_1808877836_0_TypeHandle, ref base.CheckedStateRef))
		{
			if (!uncheckedRefRW.ValueRO.LineRendererCtrl)
			{
				if (!effects.Find("Trail", out var gameObject))
				{
					break;
				}
				uncheckedRefRW.ValueRW.LineRendererCtrl = gameObject.Value.GetComponent<Spell1004LaserCustomCtrl>();
				if (effects.Find("Shadow", out var gameObject2))
				{
					uncheckedRefRW.ValueRW.ShadowLineRenderer = gameObject2.Value.GetComponent<LineRenderer>();
				}
			}
			LineRenderer lineRenderer = uncheckedRefRW.ValueRO.LineRendererCtrl.Value.LineRenderer;
			LineRenderer value = uncheckedRefRW.ValueRO.ShadowLineRenderer.Value;
			if (uncheckedRefRW.ValueRO.IsSetLinePos)
			{
				uncheckedRefRW.ValueRW.IsSetLinePos = false;
				Vector3[] array = new Vector3[dynamicBuffer.Length];
				Vector3[] array2 = new Vector3[dynamicBuffer.Length];
				for (int i = 0; i < dynamicBuffer.Length; i++)
				{
					array2[i] = new float3(dynamicBuffer[i].Value.xy, 0f);
					LaserBodyPoint laserBodyPoint = dynamicBuffer[i];
					float3 layerPosition = DTool.GetLayerPosition(in laserBodyPoint.Value, LayerCorrectType.Coordinate);
					array[i] = (Vector3)dynamicBuffer[i].Value + (Vector3)layerPosition;
				}
				lineRenderer.positionCount = array.Length;
				lineRenderer.SetPositions(array);
				if ((bool)value)
				{
					value.positionCount = array2.Length;
					value.SetPositions(array2);
				}
				uncheckedRefRW.ValueRO.LineRendererCtrl.Value.isChanging = true;
				uncheckedRefRW.ValueRO.LineRendererCtrl.Value.totalTime = uncheckedRefRW.ValueRO.totalTime;
				if (uncheckedRefRW.ValueRO.IsHoverHit)
				{
					uncheckedRefRW.ValueRO.LineRendererCtrl.Value.totalTime = math.max(uncheckedRefRW.ValueRO.LineRendererCtrl.Value.currentTime + 0.1f, 0.2f);
				}
			}
			if ((bool)value)
			{
				value.widthMultiplier = lineRenderer.widthMultiplier;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1004LaserData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LaserBodyPoint>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGameObjectEffectLink>();
		__query_1808877836_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1004LaserLineApplySystem()
	{
	}
}
