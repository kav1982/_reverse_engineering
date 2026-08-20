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
public class Spell1010LineApplySystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1493355883_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public BufferAccessor<SnakeBodyPoint> item4_BufferAccessor;

			public BufferAccessor<SpellGameObjectEffectLink> item5_BufferAccessor;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1010SnakeData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, DynamicBuffer<SnakeBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1010SnakeData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item3_IntPtr, index), item4_BufferAccessor[index], item5_BufferAccessor[index]);
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1010SnakeData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RO;

			private BufferTypeHandle<SnakeBodyPoint> item4_BufferTypeHandle_RW;

			private BufferTypeHandle<SpellGameObjectEffectLink> item5_BufferTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1010SnakeData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item4_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SnakeBodyPoint>();
				item5_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellGameObjectEffectLink>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_BufferTypeHandle_RW.Update(ref systemState);
				item5_BufferTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item4_BufferTypeHandle_RW);
				result.item5_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item5_BufferTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1010SnakeData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, DynamicBuffer<SnakeBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1010SnakeData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, DynamicBuffer<SnakeBodyPoint>, DynamicBuffer<SpellGameObjectEffectLink>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1010SnakeData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SnakeBodyPoint>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellGameObjectEffectLink>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1493355883_0.TypeHandle __IFE_1493355883_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1493355883_0_TypeHandle = new IFE_1493355883_0.TypeHandle(ref state);
		}
	}

	private static readonly int SpeedID = Shader.PropertyToID("_Speed");

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1493355883_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1010SnakeData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var (uncheckedRefRW, uncheckedRefRO, uncheckedRefRO2, dynamicBuffer, dynamicBuffer2) in IFE_1493355883_0.Query(__query_1493355883_0, __TypeHandle.__IFE_1493355883_0_TypeHandle, ref base.CheckedStateRef))
		{
			if ((object)uncheckedRefRW.ValueRO.BodyLine.Value == null && (bool)uncheckedRefRO.ValueRO.TrailEffectGameObject)
			{
				LineRenderer[] componentsInChildren = uncheckedRefRO.ValueRO.TrailEffectGameObject.Value.GetComponentsInChildren<LineRenderer>();
				if (componentsInChildren.Length != 0)
				{
					uncheckedRefRW.ValueRW.BodyLine = componentsInChildren[0];
				}
			}
			if (!uncheckedRefRW.ValueRO.BodyLine.Value)
			{
				continue;
			}
			if ((object)uncheckedRefRW.ValueRO.ShadowLine.Value == null)
			{
				foreach (SpellGameObjectEffectLink item in dynamicBuffer2)
				{
					SpellGameObjectEffectLink current = item;
					if (current.EffectName == "Shadow")
					{
						ref UnityObjectRef<LineRenderer> shadowLine = ref uncheckedRefRW.ValueRW.ShadowLine;
						UnityObjectRef<GameObject> gameObject = current.GameObject;
						shadowLine.Value = gameObject.Value.GetComponent<LineRenderer>();
					}
				}
			}
			ref Spell1010SnakeData valueRW = ref uncheckedRefRW.ValueRW;
			Vector3[] array = new Vector3[dynamicBuffer.Length];
			Vector3[] array2 = new Vector3[dynamicBuffer.Length];
			float3 zero = float3.zero;
			float3 zero2 = float3.zero;
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (uncheckedRefRO2.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					float3 rootPosition = dynamicBuffer[i].Value + uncheckedRefRO2.ValueRO.AroundCenter;
					zero = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					zero += uncheckedRefRO2.ValueRO.AroundCenter;
					rootPosition = new float3(dynamicBuffer[i].Value.xy, 0f) + uncheckedRefRO2.ValueRO.AroundCenter;
					zero2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					zero2 += new float3(uncheckedRefRO2.ValueRO.AroundCenter.xy, 0f);
				}
				else
				{
					SnakeBodyPoint snakeBodyPoint = dynamicBuffer[i];
					zero = DTool.GetLayerPosition(in snakeBodyPoint.Value, LayerCorrectType.Coordinate);
					float3 rootPosition = new float3(dynamicBuffer[i].Value.xy, 0f);
					zero2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				}
				array[i] = zero + dynamicBuffer[i].Value;
				array2[i] = zero2 + new float3(dynamicBuffer[i].Value.xy, 0f);
			}
			LineRenderer value = uncheckedRefRW.ValueRO.BodyLine.Value;
			value.positionCount = dynamicBuffer.Length;
			value.SetPositions(array);
			value.widthMultiplier = value.transform.transform.localScale.x * 0.4f;
			if (value.widthMultiplier < 0.01f)
			{
				value.widthMultiplier = 0f;
			}
			if (valueRW.IsFadingTail)
			{
				value.material.SetFloat(SpeedID, valueRW.OnGroundSpeed);
			}
			LineRenderer value2 = uncheckedRefRW.ValueRW.ShadowLine.Value;
			if ((bool)value2)
			{
				value2.positionCount = dynamicBuffer.Length;
				value2.SetPositions(array2);
				value2.widthMultiplier = value2.transform.transform.localScale.x * 0.4f;
				if (value2.widthMultiplier < 0.01f)
				{
					value2.widthMultiplier = 0f;
				}
				if (valueRW.IsFadingTail)
				{
					value2.material.SetFloat(SpeedID, valueRW.OnGroundSpeed);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1010SnakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeBodyPoint>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGameObjectEffectLink>();
		__query_1493355883_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1010LineApplySystem()
	{
	}
}
