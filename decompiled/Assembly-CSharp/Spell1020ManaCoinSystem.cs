using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
public class Spell1020ManaCoinSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_895942107_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1020ManaCoinData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1020ManaCoinData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1020ManaCoinData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1020ManaCoinData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_895942107_0.TypeHandle __IFE_895942107_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_895942107_0_TypeHandle = new IFE_895942107_0.TypeHandle(ref state);
			__SpellNeedResize_RO_ComponentLookup = state.GetComponentLookup<SpellNeedResize>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_895942107_0;

	private EntityQuery __query_895942107_1;

	private EntityQuery __query_895942107_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1020ManaCoinData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer cMD = new EntityCommandBuffer(Allocator.TempJob);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item5 in IFE_895942107_0.Query(__query_895942107_0, __TypeHandle.__IFE_895942107_0_TypeHandle, ref base.CheckedStateRef))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1020ManaCoinData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW4 = item4;
			Entity entity2 = entity;
			if (uncheckedRefRW.ValueRO.IsInitialized)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.IsInitialized = true;
			uncheckedRefRW3.ValueRW.Scale = SpellTools.CallulateSpellScaleByDamage(__query_895942107_1.GetSingleton<SpellSingleton>().Configs[uncheckedRefRW2.ValueRO.Id].damage, uncheckedRefRW2.ValueRO.Damage.Calculate()) + InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity2).ExtraSizeRatio;
			int num = Mathf.CeilToInt((float)PlayerMgr.Inst.CoinCount * 0.2f);
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW4.ValueRW.OwnerEntity))
			{
				if (!DTool.IsSameCamp(InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW4.ValueRW.OwnerEntity).ValueRW.unitCfg.unitType, UnitType.Player))
				{
					num = 0;
				}
			}
			else
			{
				num = 0;
			}
			if (uncheckedRefRW4.ValueRW.IsSplitSpell)
			{
				num = 0;
				uncheckedRefRW2.ValueRW.Damage.AddRatio += (float)uncheckedRefRW2.ValueRO.Int3 * uncheckedRefRW2.ValueRO.Float2 / 100f;
			}
			PlayerMgr.Inst.ChangeCoin(-num);
			uncheckedRefRW2.ValueRW.Damage.AddBase += (float)num * uncheckedRefRW2.ValueRO.Float2;
			FixedString32Bytes coinColor = "Gold";
			uncheckedRefRW2.ValueRO.ColorType.ColorEnumToString(out var result);
			if (uncheckedRefRW2.ValueRO.ColorType == SpellColorType.Void)
			{
				coinColor = "Void";
			}
			else if (uncheckedRefRW2.ValueRO.Int3 <= 0 && num <= 0)
			{
				coinColor = "Silver";
				if (result == "Player")
				{
					result = "Silver";
				}
			}
			CreateCoinEffects(entity2, coinColor, result, cMD, __query_895942107_1.GetSingleton<SpellSingleton>(), uncheckedRefRW4.ValueRW, __query_895942107_2.GetSingletonEntity());
			uncheckedRefRW.ValueRW.CoinUseCount = num;
		}
		cMD.Playback(base.EntityManager);
		cMD.Dispose();
	}

	private void CreateCoinEffects(Entity followingEntity, FixedString32Bytes coinColor, FixedString32Bytes ringColor, EntityCommandBuffer CMD, SpellSingleton spellSingleton, SpellComponentData spellComponentData, Entity spellEffectEntity)
	{
		FixedString32Bytes fs = $"1020_Coin_{coinColor}";
		Entity entity = CMD.Instantiate(spellSingleton.Prefabs[fs]);
		CMD.AddComponent<Parent>(entity);
		CMD.SetComponent(entity, new Parent
		{
			Value = followingEntity
		});
		CMD.SetComponent(entity, LocalTransform.Identity);
		CMD.AppendToBuffer(followingEntity, new LinkedEntityGroup
		{
			Value = entity
		});
		spellComponentData.SpellEffectEntity = entity;
		CMD.SetComponent(followingEntity, spellComponentData);
		fs = $"1020_Ring_{ringColor}";
		entity = CMD.Instantiate(spellSingleton.Prefabs[fs]);
		CMD.AddComponent<Parent>(entity);
		CMD.SetComponent(entity, new Parent
		{
			Value = followingEntity
		});
		CMD.SetComponent(entity, LocalTransform.Identity);
		CMD.AppendToBuffer(followingEntity, new LinkedEntityGroup
		{
			Value = entity
		});
		spellComponentData.TrailEffectEntity = entity;
		CMD.SetComponent(followingEntity, spellComponentData);
		if (!spellSingleton.Effects.TryGetValue(1020, out var item))
		{
			return;
		}
		foreach (SpellEffect item2 in item.GetValueArray(Allocator.Temp))
		{
			CMD.AppendToBuffer(spellEffectEntity, new SpellEffectSystem.Require
			{
				Settings = item2,
				Entity = followingEntity,
				Color = ringColor,
				SpellId = 1020
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1020ManaCoinData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_895942107_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_895942107_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_895942107_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell1020ManaCoinSystem()
	{
	}
}
