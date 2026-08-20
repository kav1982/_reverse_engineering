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
[UpdateAfter(typeof(Spell1016DashEndSystem))]
[UpdateInGroup(typeof(SpellEndSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(SpellDestroySystem))]
public class Spell1016CleanupSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_538091119_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<Spell1016DirverCleanupData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1016DirverCleanupData> item1_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1016DirverCleanupData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1016DirverCleanupData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_538091119_0.TypeHandle __IFE_538091119_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_538091119_0_TypeHandle = new IFE_538091119_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_538091119_0;

	private EntityQuery __query_538091119_1;

	private EntityQuery __query_538091119_2;

	private EntityQuery __query_538091119_3;

	private EntityQuery __query_538091119_4;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell1016DirverCleanupData>();
		RequireForUpdate<SpellDashDriverSingleton>();
		RequireForUpdate<GlobalParticleEmitParams>();
		RequireForUpdate<PlayerController_Dots>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		SpellDashDriverSingleton singleton = __query_538091119_1.GetSingleton<SpellDashDriverSingleton>();
		EntityCommandBuffer entityCommandBuffer = __query_538091119_2.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		bool flag = false;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData>> item2 in IFE_538091119_0.Query(__query_538091119_0, __TypeHandle.__IFE_538091119_0_TypeHandle, ref base.CheckedStateRef))
		{
			item2.Deconstruct(out var item, out var entity);
			InternalCompilerInterface.UncheckedRefRO<Spell1016DirverCleanupData> uncheckedRefRO = item;
			Entity e = entity;
			entityCommandBuffer.RemoveComponent<Spell1016DirverCleanupData>(e);
			Entity dirver = uncheckedRefRO.ValueRO.Dirver;
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, dirver))
			{
				continue;
			}
			RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, dirver);
			if (componentRWAfterCompletingDependency.ValueRO.unitCfg.unitType == UnitType.Player)
			{
				PlayerController_Dots singleton2 = __query_538091119_3.GetSingleton<PlayerController_Dots>();
				if ((bool)singleton2.playerCtrllerMono)
				{
					flag = true;
					GetOverheatTime(uncheckedRefRO.ValueRO.DashTimer, out var overheatTime);
					singleton2.playerCtrllerMono.Value.DashOverHeatRegister(overheatTime);
					CamController.Inst.playerInRotateDash = false;
				}
			}
			RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, dirver);
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(componentRWAfterCompletingDependency2.ValueRO.Position);
			ref readonly float3 position = ref componentRWAfterCompletingDependency2.ValueRO.Position;
			float3 @float = navMeshPointIngoreZ;
			bool num = DTool.IgnoreZDistanceSqr(in position, in @float) > 0.1f;
			componentRWAfterCompletingDependency2.ValueRW.Position = navMeshPointIngoreZ;
			if (singleton.IsShooterDriving(dirver))
			{
				singleton.ShooterDriveEnd(dirver);
			}
			componentRWAfterCompletingDependency.ValueRW.FlyUnregister();
			componentRWAfterCompletingDependency.ValueRW.InvincibleUnregister();
			uncheckedRefRO.ValueRO.ColorType.ColorEnumToString(out var result);
			if (!num)
			{
				componentRWAfterCompletingDependency.ValueRW.TakeKnockback(uncheckedRefRO.ValueRO.LastLinear * UnityEngine.Random.Range(0.3f, 0.5f));
			}
			DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = __query_538091119_4.GetSingletonBuffer<GlobalParticleEmitParams>();
			GlobalParticleEmitParams elem = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"1016_DashWave_{result}", uncheckedRefRO.ValueRO.LastPosition)
			{
				Size = uncheckedRefRO.ValueRO.Radius
			};
			singletonBuffer.Add(elem);
		}
		if (flag)
		{
			PlayerMgr.Inst.PlayerCtrller.OnPlayerDashEnd();
		}
	}

	private void GetOverheatTime(float duration, out float overheatTime)
	{
		if (duration <= 1f)
		{
			overheatTime = duration;
		}
		else if (duration <= 8f)
		{
			overheatTime = 0.357f * duration + 0.643f;
		}
		else if (duration <= 50f)
		{
			overheatTime = 0.107f * duration + 2.65f;
		}
		else
		{
			overheatTime = 8f;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1016DirverCleanupData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithNone<LocalTransform>();
		__query_538091119_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDashDriverSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538091119_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538091119_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538091119_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538091119_4 = entityQueryBuilder2.Build(ref state);
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
	public Spell1016CleanupSystem()
	{
	}
}
