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
public struct Spell4027BackMpSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1539333249_0
	{
		public struct ResolvedChunk
		{
			public BufferAccessor<SpellHitEntity> item1_BufferAccessor;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>(item1_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4027BlueRuneData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private BufferTypeHandle<SpellHitEntity> item1_BufferTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell4027BlueRuneData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellHitEntity>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4027BlueRuneData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_BufferTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item1_BufferTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4027BlueRuneData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1539333249_0.TypeHandle __IFE_1539333249_0_TypeHandle;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1539333249_0_TypeHandle = new IFE_1539333249_0.TypeHandle(ref state);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1539333249_0;

	private EntityQuery __query_1539333249_1;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell4027BlueRuneData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		BufferLookup<TakeDamageInfo_Dots> bufferLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref state);
		foreach (QueryEnumerableWithEntity<DynamicBuffer<SpellHitEntity>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> item6 in IFE_1539333249_0.Query(__query_1539333249_0, __TypeHandle.__IFE_1539333249_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			DynamicBuffer<SpellHitEntity> dynamicBuffer = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW3 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW4 = item5;
			Entity entity2 = entity;
			if (uncheckedRefRW.ValueRO.IsSplitSpell || (uncheckedRefRW2.ValueRO.MpRefillAmount <= 0f && uncheckedRefRW3.ValueRO.Int2 <= 0))
			{
				continue;
			}
			bool flag = false;
			foreach (SpellHitEntity item7 in dynamicBuffer)
			{
				if (uncheckedRefRW2.ValueRO.MpRefillAmount <= 0f && uncheckedRefRW3.ValueRO.Int2 <= 0)
				{
					break;
				}
				if (!bufferLookup.HasBuffer(item7.Entity))
				{
					Debug.LogError("为什么目标身上没有 TakeDamageInfo Buffer？");
					continue;
				}
				DynamicBuffer<BlueRuneIncreaseTakeDamageRatioData> singletonBuffer = __query_1539333249_1.GetSingletonBuffer<BlueRuneIncreaseTakeDamageRatioData>();
				foreach (TakeDamageInfo_Dots item8 in bufferLookup[item7.Entity])
				{
					if (item8.spell.Entity != entity2 || item8.targetAlreadyDeadBeforeDamage)
					{
						continue;
					}
					if (uncheckedRefRW2.ValueRW.MpRefillAmount > 0f)
					{
						uncheckedRefRW.ValueRW.Wand.Value.GainMP(uncheckedRefRW2.ValueRO.MpRefillAmount);
						if (!uncheckedRefRW4.ValueRO.IsFallSpell)
						{
							uncheckedRefRW2.ValueRW.MpRefillAmount = 0f;
						}
						else
						{
							flag = true;
						}
					}
					if (uncheckedRefRW3.ValueRO.Int2 > 0)
					{
						BlueRuneIncreaseTakeDamageRatioData blueRuneIncreaseTakeDamageRatioData = default(BlueRuneIncreaseTakeDamageRatioData);
						blueRuneIncreaseTakeDamageRatioData.TargetEntity = item7.Entity;
						blueRuneIncreaseTakeDamageRatioData.EffectDuration = 5f;
						blueRuneIncreaseTakeDamageRatioData.EffectRatio = (float)uncheckedRefRW3.ValueRO.Int2 / 10000f;
						BlueRuneIncreaseTakeDamageRatioData elem = blueRuneIncreaseTakeDamageRatioData;
						if (!uncheckedRefRW4.ValueRO.IsFallSpell)
						{
							uncheckedRefRW3.ValueRW.Int2 = 0;
						}
						else
						{
							flag = true;
						}
						singletonBuffer.Add(elem);
					}
					break;
				}
			}
			if (flag)
			{
				uncheckedRefRW2.ValueRW.MpRefillAmount = 0f;
				uncheckedRefRW3.ValueRW.Int2 = 0;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellHitEntity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4027BlueRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		__query_1539333249_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<BlueRuneIncreaseTakeDamageRatioData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1539333249_1 = entityQueryBuilder2.Build(ref state);
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
		((Spell4027BackMpSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4027BackMpSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4027BackMpSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
