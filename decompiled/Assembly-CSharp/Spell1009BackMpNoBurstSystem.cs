using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine;

[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitAfterTakeDamageSystem))]
[UpdateBefore(typeof(SpellTakeDamageResultSystem))]
[CompilerGenerated]
public struct Spell1009BackMpNoBurstSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_838182133_0
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<SpellHitEntity> item1_BufferAccessor;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData>>(item1_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1009BackMpData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<SpellHitEntity> item1_BufferTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1009BackMpData> item3_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellHitEntity>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1009BackMpData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1009BackMpData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_838182133_0.TypeHandle __IFE_838182133_0_TypeHandle;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<Spell2005GrimoireData> __Spell2005GrimoireData_RO_ComponentLookup;

		public ComponentLookup<Spell2005GrimoireData> __Spell2005GrimoireData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_838182133_0_TypeHandle = new IFE_838182133_0.TypeHandle(ref state);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__Spell2005GrimoireData_RO_ComponentLookup = state.GetComponentLookup<Spell2005GrimoireData>(isReadOnly: true);
			__Spell2005GrimoireData_RW_ComponentLookup = state.GetComponentLookup<Spell2005GrimoireData>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_838182133_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell1009BackMpData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		BufferLookup<TakeDamageInfo_Dots> bufferLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref state);
		foreach (QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData>> item4 in IFE_838182133_0.Query(__query_838182133_0, __TypeHandle.__IFE_838182133_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			DynamicBuffer<SpellHitEntity> dynamicBuffer = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<Spell1009BackMpData> uncheckedRefRW2 = item3;
			Entity entity2 = entity;
			if (uncheckedRefRW2.ValueRO.IsBackedMp)
			{
				continue;
			}
			foreach (SpellHitEntity item5 in dynamicBuffer)
			{
				if (uncheckedRefRW2.ValueRO.IsBackedMp)
				{
					break;
				}
				if (!bufferLookup.HasBuffer(item5.Entity))
				{
					Debug.LogError("为什么目标身上没有 TakeDamageInfo Buffer？");
					continue;
				}
				foreach (TakeDamageInfo_Dots item6 in bufferLookup[item5.Entity])
				{
					if (!(item6.spell.Entity != entity2) && !item6.targetAlreadyDeadBeforeDamage)
					{
						float needBackMp = uncheckedRefRW2.ValueRO.NeedBackMp;
						if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell2005GrimoireData_RO_ComponentLookup, ref state, uncheckedRefRW.ValueRW.OwnerEntity))
						{
							InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell2005GrimoireData_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.OwnerEntity).ValueRW.CurrentMp += needBackMp;
						}
						else if (uncheckedRefRW.ValueRW.Wand.Value != null)
						{
							uncheckedRefRW.ValueRW.Wand.Value.GainMP(needBackMp);
						}
						uncheckedRefRW2.ValueRW.IsBackedMp = true;
						break;
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHitEntity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1009BackMpData>();
		__query_838182133_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1009BackMpNoBurstSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
