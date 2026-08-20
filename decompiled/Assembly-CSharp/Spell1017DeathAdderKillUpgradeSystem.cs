using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(SpellTakeDamageResultSystem))]
[UpdateAfter(typeof(UnitTakeDamageDeadSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public class Spell1017DeathAdderKillUpgradeSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1521137643_0
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<SpellHitEntity> item1_BufferAccessor;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>(item1_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<SpellHitEntity> item1_BufferTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellHitEntity>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1521137643_0.TypeHandle __IFE_1521137643_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1521137643_0_TypeHandle = new IFE_1521137643_0.TypeHandle(ref state);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1521137643_0;

	private EntityQuery __query_1521137643_1;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1017DeathAdderData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> item3 in IFE_1521137643_0.Query(__query_1521137643_0, __TypeHandle.__IFE_1521137643_0_TypeHandle, ref base.CheckedStateRef))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			DynamicBuffer<SpellHitEntity> dynamicBuffer = item;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO = item2;
			Entity entity2 = entity;
			bool flag = false;
			if (!uncheckedRefRO.ValueRO.Wand.Value || uncheckedRefRO.ValueRO.Wand.Value.WandCfg == null)
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
					flag = true;
					SlotData[] validSlotsData = uncheckedRefRO.ValueRO.Wand.Value.WandCfg.GetValidSlotsData(normal: true, post: true);
					foreach (SlotData slotData in validSlotsData)
					{
						if (slotData.GetConfigIgnoreMimic().abilityType == SpellAbilityType.DeathAdder)
						{
							KillSomeOne(slotData);
						}
					}
				}
			}
			if (flag)
			{
				DynamicBuffer<SEData> singletonBuffer = __query_1521137643_1.GetSingletonBuffer<SEData>();
				FixedString32Bytes seName = "DeadHit";
				singletonBuffer.Add(new SEData(DTool.GetSpellSEName(1017, in seName)));
			}
		}
	}

	public void KillSomeOne(SlotData slotData)
	{
		slotData.specialInt++;
		SpellConfig finalConfig = slotData.GetFinalConfig();
		if (slotData.specialInt >= finalConfig.int1)
		{
			int id = finalConfig.id;
			if (id > 10170 && id < 10173)
			{
				SpellUpgrade(slotData);
			}
		}
	}

	private void SpellUpgrade(SlotData slotData)
	{
		SpellConfig finalConfig = slotData.GetFinalConfig();
		slotData.specialInt -= finalConfig.int1;
		slotData.id++;
		ShowUpgradeText(slotData);
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, slotData.id);
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg != null && wand.WandCfg.GetValidSlotsData(normal: true, post: true).Contains(slotData))
			{
				UIPlayerDataMgr.Inst.WandUpdate(PlayerMgr.Inst.Wands.IndexOf(wand));
				return;
			}
		}
		if (PlayerMgr.Inst.BaData.bagSpellDatas.Contains(slotData))
		{
			UIPlayerDataMgr.Inst.UpdateBag();
		}
	}

	private void ShowUpgradeText(SlotData slotData)
	{
		if (slotData.id == 10173)
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002012.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerCtrller.transform.position);
		}
		else
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002011.GetText() + (slotData.id - 10170), UITextFloatType.Normal, PlayerMgr.Inst.PlayerCtrller.transform.position);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1017DeathAdderData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellHitEntity>();
		__query_1521137643_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1521137643_1 = entityQueryBuilder2.Build(ref state);
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
	public Spell1017DeathAdderKillUpgradeSystem()
	{
	}
}
