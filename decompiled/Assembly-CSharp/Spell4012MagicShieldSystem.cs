using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public class Spell4012MagicShieldSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2043539579_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public BufferAccessor<MagicShieldDmgEvent> item6_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData>, DynamicBuffer<MagicShieldDmgEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData>, DynamicBuffer<MagicShieldDmgEvent>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellElementEffectComponentData>(item5_IntPtr, index), item6_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellElementEffectComponentData> item5_ComponentTypeHandle_RO;

			private BufferTypeHandle<MagicShieldDmgEvent> item6_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
				item6_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<MagicShieldDmgEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				item6_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.item6_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item6_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData>, DynamicBuffer<MagicShieldDmgEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData>, DynamicBuffer<MagicShieldDmgEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellElementEffectComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<MagicShieldDmgEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2043539579_0.TypeHandle __IFE_2043539579_0_TypeHandle;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2043539579_0_TypeHandle = new IFE_2043539579_0.TypeHandle(ref state);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2043539579_0;

	private EntityQuery __query_2043539579_1;

	private EntityQuery __query_2043539579_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4012MagicShieldData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer.ParallelWriter cmd = __query_2043539579_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).AsParallelWriter();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData>, DynamicBuffer<MagicShieldDmgEvent>> item7 in IFE_2043539579_0.Query(__query_2043539579_0, __TypeHandle.__IFE_2043539579_0_TypeHandle, ref base.CheckedStateRef))
		{
			item7.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var entity);
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO2 = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO3 = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO4 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellElementEffectComponentData> uncheckedRefRO5 = item5;
			DynamicBuffer<MagicShieldDmgEvent> dynamicBuffer = item6;
			Entity spellEntity = entity;
			if (dynamicBuffer.Length <= 0)
			{
				continue;
			}
			PhysicsWorldSingleton physics = __query_2043539579_2.GetSingleton<PhysicsWorldSingleton>();
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			foreach (MagicShieldDmgEvent item8 in dynamicBuffer)
			{
				MagicShieldDmgEvent current = item8;
				ref float3 position = ref current.Position;
				ref float radius = ref current.radius;
				UnitType selfCamp = UnitType.Player;
				ComponentLookup<UnitProperty_Dots> unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
				ComponentLookup<SpellConfigComponentData> SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef);
				SpellTools.GetAttackableEntitiesInRange(in position, in radius, in selfCamp, containsBrittleness: true, in unitPptLookup, in SpellConfigLookup, in physics, ref entities);
				TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in uncheckedRefRO4.ValueRO, in uncheckedRefRO3.ValueRO, in uncheckedRefRO.ValueRO, in uncheckedRefRO5.ValueRO, in uncheckedRefRO2.ValueRO, out var info, CostRefraction: false);
				info.damage = current.damage;
				foreach (Entity item9 in entities)
				{
					Entity target = item9;
					unitPptLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
					SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef);
					cmd.TryAttackEntity(0, in target, in info, in unitPptLookup, in SpellConfigLookup);
				}
				entities.Clear();
			}
			entities.Dispose();
			dynamicBuffer.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell4012MagicShieldData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<MagicShieldDmgEvent>();
		__query_2043539579_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2043539579_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2043539579_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell4012MagicShieldSystem()
	{
	}
}
