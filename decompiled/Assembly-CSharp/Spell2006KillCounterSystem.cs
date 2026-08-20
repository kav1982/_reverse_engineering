using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(SpellTakeDamageResultSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitTakeDamageDeadSystem))]
public class Spell2006KillCounterSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_295454182_0
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<SpellHitEntity> item1_BufferAccessor;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(item1_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<SpellHitEntity> item1_BufferTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellHitEntity>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellHitEntity>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_295454182_0.TypeHandle __IFE_295454182_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<Spell2006Data> __Spell2006Data_RO_ComponentLookup;

		public ComponentLookup<Spell2006Data> __Spell2006Data_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_295454182_0_TypeHandle = new IFE_295454182_0.TypeHandle(ref state);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__Spell2006Data_RO_ComponentLookup = state.GetComponentLookup<Spell2006Data>(isReadOnly: true);
			__Spell2006Data_RW_ComponentLookup = state.GetComponentLookup<Spell2006Data>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_295454182_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell2006Data>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item3 in IFE_295454182_0.Query(__query_295454182_0, __TypeHandle.__IFE_295454182_0_TypeHandle, ref base.CheckedStateRef))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			DynamicBuffer<SpellHitEntity> dynamicBuffer = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW = item2;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRO.Wand.Value || uncheckedRefRW.ValueRO.Wand.Value.WandCfg == null)
			{
				break;
			}
			foreach (SpellHitEntity item4 in dynamicBuffer)
			{
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, item4.Entity) && DTool.IsSameCamp(InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, item4.Entity).unitCfg.unitType, UnitType.Player))
				{
					continue;
				}
				foreach (TakeDamageInfo_Dots item5 in InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef, item4.Entity))
				{
					if (!(item5.spell.Entity == entity2) || !item5.isTargetDead || !item5.isTriggerDeadEvent)
					{
						continue;
					}
					SlotData[] validSlotsData = uncheckedRefRW.ValueRW.Wand.Value.WandCfg.GetValidSlotsData(normal: true, post: true);
					RefRW<Spell2006Data> componentRWAfterCompletingDependency;
					foreach (SlotData slotData in validSlotsData)
					{
						if (slotData.GetConfigIgnoreMimic().abilityType == SpellAbilityType.Summon6)
						{
							slotData.specialInt++;
							if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RO_ComponentLookup, ref base.CheckedStateRef, item5.spell.Entity))
							{
								componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RW_ComponentLookup, ref base.CheckedStateRef, item5.spell.Entity);
								componentRWAfterCompletingDependency.ValueRW.KillCounter++;
							}
						}
					}
					foreach (SlotData bagSpellData in PlayerMgr.Inst.BaData.bagSpellDatas)
					{
						if (bagSpellData != null && bagSpellData.id != 0 && bagSpellData.GetConfigIgnoreMimic().abilityType == SpellAbilityType.Summon6)
						{
							bagSpellData.specialInt++;
							if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RO_ComponentLookup, ref base.CheckedStateRef, item5.spell.Entity))
							{
								componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2006Data_RW_ComponentLookup, ref base.CheckedStateRef, item5.spell.Entity);
								componentRWAfterCompletingDependency.ValueRW.KillCounter++;
							}
						}
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2006Data>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellHitEntity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_295454182_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell2006KillCounterSystem()
	{
	}
}
