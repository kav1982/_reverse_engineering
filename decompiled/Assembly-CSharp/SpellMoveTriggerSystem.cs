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

[CompilerGenerated]
[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateBefore(typeof(SpellShootSystem))]
public class SpellMoveTriggerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2003404593_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMoveTriggerComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<PhysicsVelocity>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellMoveTriggerComponentData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<PhysicsVelocity> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMoveTriggerComponentData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellMoveTriggerComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<PhysicsVelocity>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2003404593_0.TypeHandle __IFE_2003404593_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2003404593_0_TypeHandle = new IFE_2003404593_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2003404593_0;

	private EntityQuery __query_2003404593_1;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<SpellMoveTriggerComponentData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		Entity e = __query_2003404593_1.GetSingleton<SpellSingleton>().Prefabs["OnMove_Trigger"];
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> item6 in IFE_2003404593_0.Query(__query_2003404593_0, __TypeHandle.__IFE_2003404593_0_TypeHandle, ref base.CheckedStateRef))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpellMoveTriggerComponentData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> data = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			InternalCompilerInterface.UncheckedRefRO<PhysicsVelocity> uncheckedRefRO2 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO3 = item5;
			Entity shooterSpell = entity;
			if (data.ValueRO.SubGroupEntity == Entity.Null || math.distance(uncheckedRefRO.ValueRO.Position.x, CamController.Inst.tsf_Focus.position.x) >= 23f || math.distance(uncheckedRefRO.ValueRO.Position.y, CamController.Inst.tsf_Focus.position.y) >= 18f)
			{
				continue;
			}
			ref SpellMoveTriggerComponentData valueRW = ref uncheckedRefRW.ValueRW;
			if (uncheckedRefRO3.ValueRO.Type != SpellSpecialMovementType.Rotation)
			{
				valueRW.DistanceCounter += math.length(uncheckedRefRO2.ValueRO.Linear) * base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
			}
			float num = valueRW.SubGroupMpCost / valueRW.TriggerDistanceRatio;
			if (valueRW.DistanceCounter >= num && (object)data.ValueRO.Wand.Value != null && data.ValueRO.Wand.Value.CheckCurrentMpEnough(valueRW.SubGroupMpCost))
			{
				data.ValueRO.Wand.Value.CostMp(valueRW.SubGroupMpCost);
				valueRW.DistanceCounter -= num;
				valueRW.TriggerDirectionFlag = !valueRW.TriggerDirectionFlag;
				SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(data.ValueRO.SubGroupEntity).SubGroup;
				SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
				builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
				{
					parameter.shootFromPostSlots = data.ValueRO.FromPostSlot;
				};
				float3 oldDir = uncheckedRefRO3.ValueRO.Direction;
				float3 shiftedDir = DTool.GetShiftedDir(in oldDir, valueRW.TriggerDirectionFlag ? 90 : (-90));
				ShootSpellSpatialInfo shootSpellSpatialInfo = ShootSpellSpatialInfo.ToPoint(uncheckedRefRO.ValueRO.Position, uncheckedRefRO.ValueRO.Position + shiftedDir);
				shootSpellBuffer.ShootByTrigger(shooterSpell, data.ValueRO, subGroup, shootSpellSpatialInfo, builder);
				Entity e2 = entityCommandBuffer.Instantiate(e);
				float2 dir = ((float3)shootSpellSpatialInfo.Direction).xy;
				quaternion rotation = DTool.DirectionToRotation(in dir);
				entityCommandBuffer.SetComponent(e2, LocalTransform.FromPositionRotation(uncheckedRefRO.ValueRO.Position, rotation));
			}
		}
		shootSpellBuffer.Playback();
		entityCommandBuffer.Playback(base.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMoveTriggerComponentData>();
		__query_2003404593_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2003404593_1 = entityQueryBuilder2.Build(ref state);
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
	public SpellMoveTriggerSystem()
	{
	}
}
