using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(SpellEndSystemGroup))]
internal class Spell1025DragonBreathDisappearSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1524401008_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1025DragonBreathData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellDestroyTag>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1025DragonBreathData> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellDestroyTag> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1025DragonBreathData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellDestroyTag>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1025DragonBreathData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellDestroyTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1524401008_0.TypeHandle __IFE_1524401008_0_TypeHandle;

		public BufferLookup<Spell1025FireGroundEffectBuffer> __Spell1025FireGroundEffectBuffer_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<SpellKeepCastingAttach> __SpellKeepCastingAttach_RO_ComponentLookup;

		public ComponentLookup<SpellKeepCastingAttach> __SpellKeepCastingAttach_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1524401008_0_TypeHandle = new IFE_1524401008_0.TypeHandle(ref state);
			__Spell1025FireGroundEffectBuffer_RW_BufferLookup = state.GetBufferLookup<Spell1025FireGroundEffectBuffer>();
			__SpellKeepCastingAttach_RO_ComponentLookup = state.GetComponentLookup<SpellKeepCastingAttach>(isReadOnly: true);
			__SpellKeepCastingAttach_RW_ComponentLookup = state.GetComponentLookup<SpellKeepCastingAttach>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1524401008_0;

	private EntityQuery __query_1524401008_1;

	private EntityQuery __query_1524401008_2;

	[Preserve]
	protected override void OnUpdate()
	{
		Entity singletonEntity = __query_1524401008_1.GetSingletonEntity();
		EntityCommandBuffer entityCommandBuffer = __query_1524401008_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData>, InternalCompilerInterface.UncheckedRefRO<SpellDestroyTag>> item5 in IFE_1524401008_0.Query(__query_1524401008_0, __TypeHandle.__IFE_1524401008_0_TypeHandle, ref base.CheckedStateRef))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var _, out var entity);
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<Spell1025DragonBreathData> uncheckedRefRW3 = item3;
			Entity entity2 = entity;
			FixedString32Bytes seName = "End";
			entityCommandBuffer.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(1025, in seName), SEPlayMode.Unique));
			if (!uncheckedRefRW2.ValueRO.IsFallSpell)
			{
				if (uncheckedRefRW2.ValueRO.Type != SpellSpecialMovementType.Rotation)
				{
					uncheckedRefRW.ValueRW.Position += uncheckedRefRW2.ValueRO.Direction * uncheckedRefRW3.ValueRO.maxAttackDistance / 2f;
					continue;
				}
				ParticleSystem[] componentsInChildren = uncheckedRefRW3.ValueRO.SpellEffectObj.Value.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Stop();
				}
				continue;
			}
			DynamicBuffer<Spell1025FireGroundEffectBuffer> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell1025FireGroundEffectBuffer_RW_BufferLookup, ref base.CheckedStateRef, entity2);
			float3 position = float3.zero;
			foreach (Spell1025FireGroundEffectBuffer item6 in bufferAfterCompletingDependency)
			{
				position = item6.position;
				position.z = uncheckedRefRW.ValueRO.Position.z;
			}
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellKeepCastingAttach_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellKeepCastingAttach_RW_ComponentLookup, ref base.CheckedStateRef, entity2, value: false);
			}
			uncheckedRefRW.ValueRW.Position = position;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDestroyTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1025DragonBreathData>();
		__query_1524401008_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1524401008_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1524401008_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell1025DragonBreathDisappearSystem()
	{
	}
}
