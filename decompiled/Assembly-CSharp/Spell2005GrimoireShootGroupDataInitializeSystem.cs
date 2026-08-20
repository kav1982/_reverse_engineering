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

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[UpdateBefore(typeof(Spell2005System))]
[CompilerGenerated]
public class Spell2005GrimoireShootGroupDataInitializeSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_303478268_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<TeammateData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2005GrimoireData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<TeammateData>(item5_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2005GrimoireData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<TeammateData> item5_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2005GrimoireData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<TeammateData>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
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
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<TeammateData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<TeammateData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell2005GrimoireData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<TeammateData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_303478268_0.TypeHandle __IFE_303478268_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_303478268_0_TypeHandle = new IFE_303478268_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_303478268_0;

	[Preserve]
	protected override void OnUpdate()
	{
		foreach (var (uncheckedRefRW, uncheckedRefRO, uncheckedRefRO2, uncheckedRefRO3, uncheckedRefRO4) in IFE_303478268_0.Query(__query_303478268_0, __TypeHandle.__IFE_303478268_0_TypeHandle, ref base.CheckedStateRef))
		{
			if (uncheckedRefRW.ValueRO.State != 0)
			{
				continue;
			}
			if (uncheckedRefRO.ValueRO.SubGroupEntity == Entity.Null)
			{
				uncheckedRefRW.ValueRW.AttackRange = 100f;
				continue;
			}
			SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(uncheckedRefRO.ValueRO.SubGroupEntity).SubGroup;
			uncheckedRefRW.ValueRW.AttackRange = MathF.Min(float.PositiveInfinity, SpellGroupAttackDistanceCalculator.GetMinAttackDistance(subGroup, uncheckedRefRO.ValueRO.Wand));
			uncheckedRefRW.ValueRW.IsRotation = SpellGroupAttackDistanceCalculator.GetShootGroupMovementType(subGroup, uncheckedRefRO.ValueRO.Wand) == SpellSpecialMovementType.Rotation;
			uncheckedRefRW.ValueRW.ShootRecoil = subGroup.GetGroupHighestRecoil();
			uncheckedRefRW.ValueRW.AttackDuration = SpellGroupAttackDistanceCalculator.SpellGroupAttackDuration(subGroup, uncheckedRefRO.ValueRO.Wand);
			uncheckedRefRW.ValueRW.TeleportCoolDownTimer = 0.2f;
			if (!uncheckedRefRW.ValueRW.IsRotation)
			{
				uncheckedRefRW.ValueRW.AttackRange *= 0.9f;
			}
			if (uncheckedRefRO2.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				uncheckedRefRW.ValueRW.AttackRange = 20f;
			}
			uncheckedRefRW.ValueRW.MaxMpCapacity = MathF.Min(float.PositiveInfinity, subGroup.GetGroupManaCost(1f));
			uncheckedRefRW.ValueRW.CurrentMp = uncheckedRefRW.ValueRW.MaxMpCapacity;
			uncheckedRefRW.ValueRW.ManaRegenPerSecond = uncheckedRefRO.ValueRO.Wand.Value.GetWandMpRecoverSpeed() * uncheckedRefRO3.ValueRO.Float1 / 100f * uncheckedRefRO4.ValueRO.TeammateSpeedRatio;
			float x = uncheckedRefRW.ValueRW.MaxMpCapacity / math.max(uncheckedRefRW.ValueRW.ManaRegenPerSecond, 0.01f);
			float groupChargeDuration = SpellGroupAttackDistanceCalculator.GetGroupChargeDuration(subGroup);
			if (groupChargeDuration > 0f)
			{
				uncheckedRefRW.ValueRW.ReleaseChargeDuration = ((groupChargeDuration > 0f) ? math.max(x, groupChargeDuration) : 0f);
			}
			uncheckedRefRW.ValueRW.IsLowCostSpell = uncheckedRefRW.ValueRW.MaxMpCapacity <= uncheckedRefRW.ValueRW.ManaRegenPerSecond && uncheckedRefRW.ValueRW.ReleaseChargeDuration <= 1f;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2005GrimoireData>();
		__query_303478268_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell2005GrimoireShootGroupDataInitializeSystem()
	{
	}
}
