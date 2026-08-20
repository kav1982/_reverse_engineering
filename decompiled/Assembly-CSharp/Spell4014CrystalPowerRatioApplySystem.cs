using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using UnityEngine.Scripting;

[UpdateBefore(typeof(Spell4014LaserCrystalSystem))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public class Spell4014CrystalPowerRatioApplySystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1276459224_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellElementEffectComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellElementEffectComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellElementEffectComponentData>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellElementEffectComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1276459224_0.TypeHandle __IFE_1276459224_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1276459224_0_TypeHandle = new IFE_1276459224_0.TypeHandle(ref state);
			__SpellSplitComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>(isReadOnly: true);
			__SpellSplitComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1276459224_0;

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item5 in IFE_1276459224_0.Query(__query_1276459224_0, __TypeHandle.__IFE_1276459224_0_TypeHandle, ref base.CheckedStateRef))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellElementEffectComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW4 = item4;
			Entity entity2 = entity;
			Wand value = uncheckedRefRW4.ValueRW.Wand.Value;
			if (value == null)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.WandCrystalCount = value.LaserCrystalCount;
			uncheckedRefRW.ValueRW.ThunderStoneRate = value.LaserCrystalThunderChance;
			if (uncheckedRefRW.ValueRW.IsInitialized)
			{
				continue;
			}
			if (value.LaserCrystalPowerRatio > 1f)
			{
				uncheckedRefRW2.ValueRW.Damage.MulRatio *= value.LaserCrystalPowerRatio;
				uncheckedRefRW3.ValueRW.VenomApplyCount *= value.LaserCrystalPowerRatio;
			}
			uncheckedRefRW.ValueRW.MpCostRatio = 1f / math.max(1f, value.LaserCrystalFinalMultiCount) / math.max(1f, value.LaserCrystalFinalSplitCount);
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				RefRW<SpellSplitComponentData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RW_ComponentLookup, ref base.CheckedStateRef, entity2);
				if (componentRWAfterCompletingDependency.ValueRW.Count > 0)
				{
					componentRWAfterCompletingDependency.ValueRW.Count = value.LaserCrystalFinalSplitCount;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4014LaserCrystalData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1276459224_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4014CrystalPowerRatioApplySystem()
	{
	}
}
