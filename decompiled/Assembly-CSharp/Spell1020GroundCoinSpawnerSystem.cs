using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
public class Spell1020GroundCoinSpawnerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_895942228_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRO<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell1020ManaCoinData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1020ManaCoinData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1020ManaCoinData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRO<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRO<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1020ManaCoinData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_895942228_0.TypeHandle __IFE_895942228_0_TypeHandle;

		public ComponentLookup<Spell10201Coin> __Spell10201Coin_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_895942228_0_TypeHandle = new IFE_895942228_0.TypeHandle(ref state);
			__Spell10201Coin_RW_ComponentLookup = state.GetComponentLookup<Spell10201Coin>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_895942228_0;

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var (uncheckedRefRO, uncheckedRefRO2) in IFE_895942228_0.Query(__query_895942228_0, __TypeHandle.__IFE_895942228_0_TypeHandle, ref base.CheckedStateRef))
		{
			if (uncheckedRefRO.ValueRO.CoinUseCount <= 0)
			{
				continue;
			}
			Vector3 startPoint = uncheckedRefRO2.ValueRO.Position;
			Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			RoomConfig roomCfg = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
			if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
			{
				if (startPoint.x < centerPoint.x - (float)roomCfg.theme6Width / 2f + 1f)
				{
					startPoint.x = centerPoint.x - (float)roomCfg.theme6Width / 2f + 1f;
				}
				else if (startPoint.x > centerPoint.x + (float)roomCfg.theme6Width / 2f - 1f)
				{
					startPoint.x = centerPoint.x + (float)roomCfg.theme6Width / 2f - 1f;
				}
				if (startPoint.y < centerPoint.y - (float)roomCfg.theme6Height / 2f + 1f)
				{
					startPoint.y = centerPoint.y - (float)roomCfg.theme6Height / 2f + 1f;
				}
				else if (startPoint.y > centerPoint.y + (float)roomCfg.theme6Height / 2f - 1f)
				{
					startPoint.y = centerPoint.y + (float)roomCfg.theme6Height / 2f - 1f;
				}
			}
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("Spell10201Coin", startPoint);
			RefRW<Spell10201Coin> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell10201Coin_RW_ComponentLookup, ref base.CheckedStateRef, entity);
			componentRWAfterCompletingDependency.ValueRW.coinCount = uncheckedRefRO.ValueRO.CoinUseCount;
			componentRWAfterCompletingDependency.ValueRW.belongRoomMapPos = LevelMgr.Inst.CurrentRoomMapPos;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDestroyTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1020ManaCoinData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		__query_895942228_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1020GroundCoinSpawnerSystem()
	{
	}
}
