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
using UnityEngine;
using UnityEngine.Scripting;

[UpdateAfter(typeof(SpellEffectSystem))]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[CompilerGenerated]
internal class Spell1029DimensionTravellerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_159335487_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public EnabledMask item5_EnabledMask;

			public IntPtr item6_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRW<Spell1029InitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), item5_EnabledMask.GetEnabledRefRW<Spell1029InitializedTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1029DimensionTravellerData>(item6_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1029InitializedTag> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1029DimensionTravellerData> item6_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1029InitializedTag>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1029DimensionTravellerData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_EnabledMask = archetypeChunk.GetEnabledMask(ref item5_ComponentTypeHandle_RW);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRW<Spell1029InitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRW<Spell1029InitializedTag>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1029InitializedTag>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1029DimensionTravellerData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_159335487_1
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1029DimensionTravellerData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1029DimensionTravellerData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1029DimensionTravellerData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1029DimensionTravellerData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_159335487_0.TypeHandle __IFE_159335487_0_TypeHandle;

		public IFE_159335487_1.TypeHandle __IFE_159335487_1_TypeHandle;

		public ComponentLookup<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_159335487_0_TypeHandle = new IFE_159335487_0.TypeHandle(ref state);
			__IFE_159335487_1_TypeHandle = new IFE_159335487_1.TypeHandle(ref state);
			__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup = state.GetComponentLookup<GlobalParticle.EmitDistanceCounter>();
		}
	}

	private HashSet<Wand> _mpClearWand = new HashSet<Wand>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_159335487_0;

	private EntityQuery __query_159335487_1;

	private EntityQuery __query_159335487_2;

	private EntityQuery __query_159335487_3;

	private EntityQuery __query_159335487_4;

	private EntityQuery __query_159335487_5;

	private EntityQuery __query_159335487_6;

	private EntityQuery __query_159335487_7;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<PlayerController_Dots>();
		RequireForUpdate<SEData>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<GlobalRandom>();
		RequireForUpdate<SpellEffectSystem.Require>();
		RequireForUpdate<SpellEffectSystem.Destroy>();
		RequireForUpdate<GlobalParticleEmitParams>();
		RequireForUpdate<PhysicsWorldSingleton>();
	}

	private void TravelEffect(EntityCommandBuffer ecb, in SpellComponentData comp, ref LocalTransform transform, ref SpellConfigComponentData spellConfig, ref SpellSingleton spellSingleton, ref SpellMovementComponentData movement, in Entity spellRequire, in Entity spellDestroy, in Entity spellEntity, in Entity globalParticle, in Entity sePlayerSingleton)
	{
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RW_ComponentLookup, ref base.CheckedStateRef, comp.SpellEffectEntity).ValueRW.startCounter = false;
		ecb.AppendToBuffer(spellDestroy, new SpellEffectSystem.Destroy
		{
			Entity = spellEntity,
			Name = "Trail"
		});
		Entity e = sePlayerSingleton;
		SpellAbilityType abilityType = spellConfig.AbilityType;
		FixedString32Bytes seName = "Travel";
		ecb.AppendToBuffer(e, new SEData(DTool.GetSpellSEName((int)abilityType, in seName)));
		spellConfig.ColorType.ColorEnumToString(out var result);
		NativeHashMap<FixedString64Bytes, SpellEffect> nativeHashMap = spellSingleton.Effects[1029];
		ecb.AppendToBuffer(spellRequire, new SpellEffectSystem.Require
		{
			Settings = nativeHashMap["Trail"],
			Color = result,
			SpellId = 1029,
			Entity = spellEntity
		});
		if (!movement.IsFallSpell)
		{
			ecb.AppendToBuffer(globalParticle, new GlobalParticleEmitParams
			{
				Position = transform.Position,
				Size = transform.Scale,
				Velocity = movement.Direction,
				Name = $"1029_Travel_{result}"
			});
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		Entity spellRequire = __query_159335487_2.GetSingletonEntity();
		Entity globalParticle = __query_159335487_3.GetSingletonEntity();
		Entity spellDestroy = __query_159335487_4.GetSingletonEntity();
		SpellSingleton spellSingleton = __query_159335487_5.GetSingleton<SpellSingleton>();
		EntityCommandBuffer ecb = __query_159335487_6.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		Vector3 vector = GameMgr.Inst.cameCtrller.cam_Main.ViewportToWorldPoint(new Vector2(0f, 0f));
		Vector3 vector2 = GameMgr.Inst.cameCtrller.cam_Main.ViewportToWorldPoint(new Vector2(1f, 1f));
		Entity sePlayerSingleton = __query_159335487_7.GetSingletonEntity();
		foreach (var item12 in IFE_159335487_0.Query(__query_159335487_0, __TypeHandle.__IFE_159335487_0_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> item = item12.Item1;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> item2 = item12.Item2;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> item3 = item12.Item3;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> item4 = item12.Item4;
			EnabledRefRW<Spell1029InitializedTag> item5 = item12.Item5;
			InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData> item6 = item12.Item6;
			item5.ValueRW = false;
			item.ValueRW.Scale = item3.ValueRW.Radius.CalculateIgnoreFall();
			item6.ValueRW.InitialPositionZ = item.ValueRW.Position.z;
			if (item2.ValueRW.IsFallSpell && item4.ValueRW.IsSplitSpell)
			{
				item6.ValueRW.InitialPositionZ = -7f;
				item.ValueRW.Position.z = -14f;
				item2.ValueRW.Gravity = 25f;
				item2.ValueRW.CurrentFallSpeed = 21f;
			}
			else
			{
				item6.ValueRW.InitialPositionZ = item.ValueRW.Position.z;
			}
			if (!item4.ValueRW.IsSplitSpell && item3.ValueRW.ShooterType == UnitType.Player && item4.ValueRW.Wand.Value != null)
			{
				float currentMP = item4.ValueRW.Wand.Value.CurrentMP;
				item6.ValueRW.BonusAddDamage = currentMP / item3.ValueRW.Float1 * item3.ValueRW.Float2;
				item6.ValueRW.BonusDuration = currentMP / item3.ValueRW.Float1 * item3.ValueRW.Float3;
				_mpClearWand.Add(item4.ValueRW.Wand.Value);
				item3.ValueRW.Damage.AddBase += item6.ValueRW.BonusAddDamage;
				item3.ValueRW.Duration.AddBase += item6.ValueRW.BonusDuration;
			}
			else if (item4.ValueRW.IsSplitSpell)
			{
				item3.ValueRW.Damage.AddBase += item3.ValueRO.Float2;
				item3.ValueRW.Duration.AddBase += item3.ValueRO.Float3;
			}
		}
		foreach (Wand item13 in _mpClearWand)
		{
			if (item13 != null)
			{
				item13.CostMp(item13.CurrentMP);
			}
		}
		_mpClearWand.Clear();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1029DimensionTravellerData>> item14 in IFE_159335487_1.Query(__query_159335487_1, __TypeHandle.__IFE_159335487_1_TypeHandle, ref base.CheckedStateRef))
		{
			item14.Deconstruct(out var item7, out var item8, out var item9, out var item10, out var _, out var entity);
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW = item7;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW2 = item8;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW3 = item9;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW4 = item10;
			Entity spellEntity = entity;
			if (uncheckedRefRW3.ValueRW.Type != SpellSpecialMovementType.Rotation)
			{
				float num = uncheckedRefRW4.ValueRW.Radius.Calculate();
				float3 position = uncheckedRefRW.ValueRW.Position;
				if (position.x <= vector.x - num - 0.5f)
				{
					uncheckedRefRW.ValueRW.Position = new float3(vector2.x + num, position.y, position.z);
					TravelEffect(ecb, in uncheckedRefRW2.ValueRO, ref uncheckedRefRW.ValueRW, ref uncheckedRefRW4.ValueRW, ref spellSingleton, ref uncheckedRefRW3.ValueRW, in spellRequire, in spellDestroy, in spellEntity, in globalParticle, in sePlayerSingleton);
				}
				else if (position.x >= vector2.x + num + 0.5f)
				{
					uncheckedRefRW.ValueRW.Position = new float3(vector.x - num, position.y, position.z);
					TravelEffect(ecb, in uncheckedRefRW2.ValueRO, ref uncheckedRefRW.ValueRW, ref uncheckedRefRW4.ValueRW, ref spellSingleton, ref uncheckedRefRW3.ValueRW, in spellRequire, in spellDestroy, in spellEntity, in globalParticle, in sePlayerSingleton);
				}
				else if (position.y <= vector.y - num - 0.5f)
				{
					uncheckedRefRW.ValueRW.Position = new float3(position.x, vector2.y + num, position.z);
					TravelEffect(ecb, in uncheckedRefRW2.ValueRO, ref uncheckedRefRW.ValueRW, ref uncheckedRefRW4.ValueRW, ref spellSingleton, ref uncheckedRefRW3.ValueRW, in spellRequire, in spellDestroy, in spellEntity, in globalParticle, in sePlayerSingleton);
				}
				else if (position.y >= vector2.y + num + 0.5f)
				{
					uncheckedRefRW.ValueRW.Position = new float3(position.x, vector.y - num, position.z);
					TravelEffect(ecb, in uncheckedRefRW2.ValueRO, ref uncheckedRefRW.ValueRW, ref uncheckedRefRW4.ValueRW, ref spellSingleton, ref uncheckedRefRW3.ValueRW, in spellRequire, in spellDestroy, in spellEntity, in globalParticle, in sePlayerSingleton);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1029DimensionTravellerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1029InitializedTag>();
		__query_159335487_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1029DimensionTravellerData>();
		__query_159335487_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_159335487_7 = entityQueryBuilder2.Build(ref state);
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
	public Spell1029DimensionTravellerSystem()
	{
	}
}
