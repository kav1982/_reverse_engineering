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

[UpdateAfter(typeof(SpellTakeDamageResultSystem))]
[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public class SpellHitTriggerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1509161409_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellHitTriggerComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellHitTriggerComponentData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellHitTriggerComponentData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellHitTriggerComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1509161409_0.TypeHandle __IFE_1509161409_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1509161409_0_TypeHandle = new IFE_1509161409_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1509161409_0;

	[Preserve]
	protected override void OnUpdate()
	{
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item3 in IFE_1509161409_0.Query(__query_1509161409_0, __TypeHandle.__IFE_1509161409_0_TypeHandle, ref base.CheckedStateRef))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpellHitTriggerComponentData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> data = item2;
			Entity shooterSpell = entity;
			ref SpellHitTriggerComponentData valueRW = ref uncheckedRefRW.ValueRW;
			valueRW.UpdateCooldown(base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
			if (!valueRW.NeedTrigger)
			{
				continue;
			}
			valueRW.NeedTrigger = false;
			if ((bool)data.ValueRO.Wand.Value && data.ValueRO.Wand.Value.CheckCurrentMpEnough(valueRW.SubGroupMp))
			{
				data.ValueRO.Wand.Value.CostMp(valueRW.SubGroupMp);
				SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(data.ValueRO.SubGroupEntity).SubGroup;
				SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
				builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
				{
					parameter.shootFromPostSlots = data.ValueRO.FromPostSlot;
				};
				shootSpellBuffer.ShootByTrigger(shooterSpell, data.ValueRO, subGroup, ShootSpellSpatialInfo.ToPoint(valueRW.TriggerPoint, valueRW.TriggerPoint + (float3)Tool2D.GetDir()), builder);
			}
		}
		shootSpellBuffer.Playback();
		shootSpellBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellHitTriggerComponentData>();
		__query_1509161409_0 = entityQueryBuilder2.Build(ref state);
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
	public SpellHitTriggerSystem()
	{
	}
}
