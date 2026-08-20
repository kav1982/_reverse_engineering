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

[UpdateBefore(typeof(SpellShootSystem))]
[CompilerGenerated]
[UpdateInGroup(typeof(SpellCreateSystemGroup))]
public class Spell2005GrimoireShootSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_303478124_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr item7_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2005GrimoireData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2005GrimoireData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RO;

			private ComponentTypeHandle<UnitProperty_Dots> item6_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateData> item7_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2005GrimoireData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item7_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				item7_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.item7_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item7_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_303478124_0.TypeHandle __IFE_303478124_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_303478124_0_TypeHandle = new IFE_303478124_0.TypeHandle(ref state);
			__TeammateDeadTag_RO_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>(isReadOnly: true);
		}
	}

	private EntityQuery _enemyQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_303478124_0;

	private EntityQuery __query_303478124_1;

	[Preserve]
	protected override void OnCreate()
	{
		_enemyQuery = base.EntityManager.CreateEntityQuery(typeof(UnitProperty_Dots), typeof(LocalTransform));
	}

	[Preserve]
	protected override void OnUpdate()
	{
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		int num = 0;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<TeammateData>> item8 in IFE_303478124_0.Query(__query_303478124_0, __TypeHandle.__IFE_303478124_0_TypeHandle, ref base.CheckedStateRef))
		{
			item8.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var item7, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2005GrimoireData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> data = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO2 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO3 = item5;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW2 = item6;
			InternalCompilerInterface.UncheckedRefRW<TeammateData> uncheckedRefRW3 = item7;
			Entity entity2 = entity;
			if (data.ValueRO.SubGroupEntity == Entity.Null || !uncheckedRefRW.ValueRW.ReadyToAttack || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2) || uncheckedRefRW3.ValueRW.IsFuseMaterial)
			{
				continue;
			}
			NativeArray<UnitProperty_Dots> unitsArray = _enemyQuery.ToComponentDataArray<UnitProperty_Dots>(Allocator.Temp);
			NativeArray<LocalTransform> unitsTransformArray = _enemyQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
			int nearestEnemyIndex = DTool.GetNearestEnemyIndex(in unitsArray, in unitsTransformArray, in uncheckedRefRO3.ValueRO.ShooterType, in uncheckedRefRO.ValueRO.Position);
			uncheckedRefRW.ValueRW.ReadyToAttack = false;
			DynamicOptimizeData singleton = __query_303478124_1.GetSingleton<DynamicOptimizeData>();
			int num2 = ((uncheckedRefRW.ValueRW.SpellCastCounter < 9) ? 1 : (1 + uncheckedRefRW3.ValueRW.AdvanceSkillLevel * 4));
			float shootPower = 1f;
			bool flag = num2 > 1;
			if (singleton.IsLowFpsOptimizeActive(40f) && flag)
			{
				int num3 = (int)math.max(1f, math.floor((float)num2 * singleton.CurrentFPS / 40f));
				num += num3;
				if (num >= 35)
				{
					num3 = 1;
				}
				shootPower = (float)num2 / (float)num3;
				num2 = num3;
			}
			if (uncheckedRefRW3.ValueRW.AdvanceSkillLevel > 0)
			{
				uncheckedRefRW.ValueRW.SpellCastCounter++;
			}
			SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(data.ValueRO.SubGroupEntity).SubGroup;
			if (uncheckedRefRW.ValueRW.ReleaseChargeDuration > 0f)
			{
				uncheckedRefRW.ValueRW.ReleaseChargeSpell = false;
				uncheckedRefRW.ValueRW.ReleaseChargeTimer = uncheckedRefRW.ValueRW.ReleaseChargeDuration;
			}
			data.ValueRW.OwnerEntity = entity2;
			float num4 = (float)(-uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel) / 2f;
			for (int i = 0; i <= uncheckedRefRW3.ValueRW.TeammateCurrentFuseLevel; i++)
			{
				float x = (num4 + (float)i) * 0.6f * uncheckedRefRO.ValueRO.Scale;
				for (int j = 0; j < num2; j++)
				{
					SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
					builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
					{
						parameter.shootFromPostSlots = data.ValueRO.FromPostSlot;
						parameter.finalDamageRatio *= shootPower;
						parameter.spellEfficiency *= shootPower;
					};
					float3 @float = DTool.IgnoreZPosition(in uncheckedRefRO.ValueRO.Position);
					float3 point = @float + uncheckedRefRO2.ValueRO.Direction;
					if (nearestEnemyIndex >= 0)
					{
						point = unitsTransformArray[nearestEnemyIndex].Position;
					}
					if (flag)
					{
						float num5 = DTool.IgnoreZDistance(in point, in @float);
						float3 oldDir = uncheckedRefRO2.ValueRO.Direction;
						float3 shiftedDir = DTool.GetShiftedDir(in oldDir, (float)j * 360f / (float)num2);
						point = @float + shiftedDir * num5;
					}
					float3 float2 = new float3(0f, 0f, 0f - uncheckedRefRW.ValueRO.BookFloatingHeight);
					SpellComponentData valueRW = data.ValueRW;
					shootSpellBuffer.ShootByTrigger(entity2, valueRW, subGroup, ShootSpellSpatialInfo.ToPoint(@float + float2 + new float3(x, 0f, 0f), point + float2), builder);
				}
			}
			if (flag)
			{
				uncheckedRefRW.ValueRW.SpellCastCounter -= 9;
			}
			uncheckedRefRW2.ValueRW.TakeKnockback(-uncheckedRefRO2.ValueRO.Direction * uncheckedRefRW.ValueRO.ShootRecoil * UnityEngine.Random.Range(0.9f, 1.1f));
			if (data.ValueRO.Wand.Value.WandCfg != null)
			{
				float postslotCastSpellChargeRatio = data.ValueRO.Wand.Value.WandCfg.PostslotCastSpellChargeRatio;
				if (postslotCastSpellChargeRatio > 0f)
				{
					data.ValueRO.Wand.Value.ChargePostSlots(postslotCastSpellChargeRatio);
				}
			}
		}
		shootSpellBuffer.Playback();
		shootSpellBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2005GrimoireData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_303478124_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_303478124_1 = entityQueryBuilder2.Build(ref state);
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
	public Spell2005GrimoireShootSystem()
	{
	}
}
