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
public class Spell4014RayLineApplySystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1642816819_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<CrystalLaserPoint> item3_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<CrystalLaserPoint>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<CrystalLaserPoint>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<CrystalLaserPoint> item3_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<CrystalLaserPoint>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<CrystalLaserPoint>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<CrystalLaserPoint>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<CrystalLaserPoint>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1642816819_0.TypeHandle __IFE_1642816819_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1642816819_0_TypeHandle = new IFE_1642816819_0.TypeHandle(ref state);
		}
	}

	private static Dictionary<GameObject, Spell4014RayCustomCtrl> LineCostomCache = new Dictionary<GameObject, Spell4014RayCustomCtrl>();

	private static Dictionary<GameObject, Spell4014CrystalPowerUpCtrl> PowerUpDic = new Dictionary<GameObject, Spell4014CrystalPowerUpCtrl>();

	private static Dictionary<GameObject, Spell4014CrystalCtrl> CrystalDic = new Dictionary<GameObject, Spell4014CrystalCtrl>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1642816819_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4014LaserCrystalData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, DynamicBuffer<CrystalLaserPoint>> item4 in IFE_1642816819_0.Query(__query_1642816819_0, __TypeHandle.__IFE_1642816819_0_TypeHandle, ref base.CheckedStateRef))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var _);
			InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO = item2;
			DynamicBuffer<CrystalLaserPoint> dynamicBuffer = item3;
			UnityObjectRef<GameObject> trailEffectGameObject = uncheckedRefRO.ValueRO.TrailEffectGameObject;
			if (trailEffectGameObject.Value != null)
			{
				if (!LineCostomCache.TryGetValue(trailEffectGameObject, out var value))
				{
					value = trailEffectGameObject.Value.GetComponent<Spell4014RayCustomCtrl>();
					LineCostomCache[trailEffectGameObject] = value;
				}
				value.gameObject.SetActive(!uncheckedRefRW.ValueRO.IsSplitCenter);
				if (dynamicBuffer.Length < 1)
				{
					continue;
				}
				float3[] array = new float3[dynamicBuffer.Length];
				for (int i = 0; i < dynamicBuffer.Length; i++)
				{
					float3 rootPosition = new float3(dynamicBuffer[i].Position.xy, 0f);
					float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
					array[i] = layerPosition + rootPosition;
				}
				value.SetLinePos(array, 1);
				value.SetLaserHitObject(uncheckedRefRW.ValueRO.IsHit);
				value.SetLaserWidth(uncheckedRefRW.ValueRO.LaserWidth, 1);
				for (int j = 0; j < dynamicBuffer.Length; j++)
				{
					CrystalLaserPoint crystalLaserPoint = dynamicBuffer[j];
					float3 layerPosition2 = DTool.GetLayerPosition(in crystalLaserPoint.Position, LayerCorrectType.Coordinate);
					array[j] = layerPosition2 + dynamicBuffer[j].Position;
				}
				if (uncheckedRefRW.ValueRO.IsHit)
				{
					value.SetLinePos(array, 0);
					value.SetLaserWidth(uncheckedRefRW.ValueRO.LaserWidth, 0);
				}
				else
				{
					value.SetLinePos(array, 2);
					value.SetLaserWidth(uncheckedRefRW.ValueRO.LaserWidth, 2);
				}
				value.SetLaserHitObject(uncheckedRefRW.ValueRO.IsHit);
				value.SetLaserNodePosAndScale(uncheckedRefRW.ValueRO.LaserWidth, uncheckedRefRW.ValueRO.IsHit, uncheckedRefRW.ValueRO.LaserWidthBase);
			}
			trailEffectGameObject = uncheckedRefRW.ValueRO.LaserCrystalPowerUpGO;
			if (trailEffectGameObject.Value != null)
			{
				if (!PowerUpDic.TryGetValue(trailEffectGameObject, out var value2))
				{
					value2 = trailEffectGameObject.Value.GetComponent<Spell4014CrystalPowerUpCtrl>();
					PowerUpDic[trailEffectGameObject] = value2;
				}
				value2.SetEffect(uncheckedRefRW.ValueRO.KeepHittingRate);
			}
			trailEffectGameObject = uncheckedRefRW.ValueRO.LaserCrystalGO;
			if (trailEffectGameObject.Value != null)
			{
				if (!CrystalDic.TryGetValue(trailEffectGameObject, out var value3))
				{
					value3 = trailEffectGameObject.Value.GetComponent<Spell4014CrystalCtrl>();
					CrystalDic[trailEffectGameObject] = value3;
				}
				value3.SetTrailWidth(uncheckedRefRW.ValueRO.LaserWidthBase);
			}
		}
	}

	[Preserve]
	protected override void OnDestroy()
	{
		LineCostomCache.Clear();
		PowerUpDic.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CrystalLaserPoint>();
		__query_1642816819_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4014RayLineApplySystem()
	{
	}
}
