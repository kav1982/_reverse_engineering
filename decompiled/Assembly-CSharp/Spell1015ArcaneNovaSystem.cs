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
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateBefore(typeof(SpellShootSystem))]
[CompilerGenerated]
public class Spell1015ArcaneNovaSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1573569454_0
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1015ArcaneNovaComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1015ArcaneNovaComponentData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1015ArcaneNovaComponentData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1015ArcaneNovaComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1573569454_0.TypeHandle __IFE_1573569454_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellDestroyTag> __SpellDestroyTag_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1573569454_0_TypeHandle = new IFE_1573569454_0.TypeHandle(ref state);
			__SpellDestroyTag_RO_ComponentLookup = state.GetComponentLookup<SpellDestroyTag>(isReadOnly: true);
		}
	}

	private EntityQuery _enemyQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1573569454_0;

	[Preserve]
	protected override void OnCreate()
	{
		_enemyQuery = base.EntityManager.CreateEntityQuery(typeof(UnitProperty_Dots), typeof(LocalTransform));
		RequireForUpdate<Spell1015ArcaneNovaComponentData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		float lowFpsActiveThreshold = SpellTools.GetLowFpsActiveThreshold(GameMgr.IsMobile_Static);
		float lowFpsMaxBonusTimeScale = SpellTools.GetLowFpsMaxBonusTimeScale(GameMgr.IsMobile_Static);
		bool flag = GeneralTool.IsLowFpsOptimizeActive(lowFpsActiveThreshold);
		float fps = GameMgr.Inst.GetFps();
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>> item6 in IFE_1573569454_0.Query(__query_1573569454_0, __TypeHandle.__IFE_1573569454_0_TypeHandle, ref base.CheckedStateRef))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1015ArcaneNovaComponentData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> data = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO2 = item4;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO3 = item5;
			Entity entity2 = entity;
			ref Spell1015ArcaneNovaComponentData valueRW = ref uncheckedRefRW.ValueRW;
			if (data.ValueRO.SubGroupEntity == Entity.Null || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellDestroyTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				continue;
			}
			float num = (flag ? SpellTools.GetLowFrameTimeScale(lowFpsMaxBonusTimeScale, fps, lowFpsActiveThreshold) : 1f);
			float shootIntervalScale = (flag ? SpellTools.GetLowFrameTimeScale(15f, fps, lowFpsActiveThreshold) : 1f);
			valueRW.CooldownTimer += base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime * num;
			float num2 = (uncheckedRefRO3.ValueRO.Duration.Calculate() - 0.4f) / (float)uncheckedRefRO3.ValueRO.Int1 * shootIntervalScale;
			if (valueRW.CooldownTimer <= num2)
			{
				continue;
			}
			valueRW.CooldownTimer -= num2;
			if (!(valueRW.ShootCounter >= 20f))
			{
				valueRW.ShootCounter += shootIntervalScale;
				NativeArray<UnitProperty_Dots> unitsArray = _enemyQuery.ToComponentDataArray<UnitProperty_Dots>(Allocator.Temp);
				NativeArray<LocalTransform> unitsTransformArray = _enemyQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
				int nearestEnemyIndex = DTool.GetNearestEnemyIndex(in unitsArray, in unitsTransformArray, in uncheckedRefRO3.ValueRO.ShooterType, in uncheckedRefRO.ValueRO.Position);
				float3 @float = uncheckedRefRO.ValueRO.Position + uncheckedRefRO2.ValueRO.Direction;
				if (nearestEnemyIndex >= 0)
				{
					@float = unitsTransformArray[nearestEnemyIndex].Position;
				}
				SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(data.ValueRO.SubGroupEntity).SubGroup;
				SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
				builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
				{
					parameter.finalSpeedRatio *= 0.85f;
					parameter.finalKnockBackRatio *= 0.7f;
					parameter.extraScatter += 45f;
					parameter.finalDamageRatio *= shootIntervalScale;
					parameter.shootFromPostSlots = data.ValueRO.FromPostSlot;
					parameter.VenomElementData.VenomApplyCount *= shootIntervalScale;
				};
				shootSpellBuffer.ShootByTrigger(entity2, data.ValueRO, subGroup, ShootSpellSpatialInfo.ToPoint(uncheckedRefRO.ValueRO.Position, @float), builder);
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
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1015ArcaneNovaComponentData>();
		__query_1573569454_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell1015ArcaneNovaSystem()
	{
	}
}
