using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DG.Tweening;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SpellSimulationSystemGroup))]
internal class Spell1021MagicBreakerSystem : SystemBase
{
	public struct Spell1021HitTrigger
	{
		public Entity entity;

		public float3 size;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_730499402_0
	{
		public struct ResolvedChunk
		{
			public EnabledMask item1_EnabledMask;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr item7_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<EnabledRefRW<Spell1021InitEffectTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<EnabledRefRW<Spell1021InitEffectTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(item1_EnabledMask.GetEnabledRefRW<Spell1021InitEffectTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsVelocity>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1021MagicBreakerData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1021InitEffectTag> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsVelocity> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1021MagicBreakerData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item6_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item7_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1021InitEffectTag>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsVelocity>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1021MagicBreakerData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item7_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				item7_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_EnabledMask = archetypeChunk.GetEnabledMask(ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.item7_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item7_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<EnabledRefRW<Spell1021InitEffectTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<EnabledRefRW<Spell1021InitEffectTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1021InitEffectTag>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsVelocity>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1021MagicBreakerData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_730499402_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1021MagicBreakerData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1021MagicBreakerData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1021MagicBreakerData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1021MagicBreakerData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_730499402_0.TypeHandle __IFE_730499402_0_TypeHandle;

		public IFE_730499402_1.TypeHandle __IFE_730499402_1_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RO_ComponentLookup;

		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellRemoteShootTag> __SpellRemoteShootTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<MultiShootData> __MultiShootData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellRevertDirection> __SpellRevertDirection_RO_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1021InitEffectTag> __Spell1021InitEffectTag_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<SpellHalfLifeTeleportData> __SpellHalfLifeTeleportData_RW_ComponentLookup;

		public Spell1021MoveJob.InternalCompilerQueryAndHandleData __Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_730499402_0_TypeHandle = new IFE_730499402_0.TypeHandle(ref state);
			__IFE_730499402_1_TypeHandle = new IFE_730499402_1.TypeHandle(ref state);
			__SpellNeedResize_RO_ComponentLookup = state.GetComponentLookup<SpellNeedResize>(isReadOnly: true);
			__SpellNeedResize_RW_ComponentLookup = state.GetComponentLookup<SpellNeedResize>();
			__SpellRemoteShootTag_RO_ComponentLookup = state.GetComponentLookup<SpellRemoteShootTag>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__MultiShootData_RO_ComponentLookup = state.GetComponentLookup<MultiShootData>(isReadOnly: true);
			__SpellRevertDirection_RO_ComponentLookup = state.GetComponentLookup<SpellRevertDirection>(isReadOnly: true);
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__Spell1021InitEffectTag_RO_ComponentLookup = state.GetComponentLookup<Spell1021InitEffectTag>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellHalfLifeTeleportData_RW_ComponentLookup = state.GetComponentLookup<SpellHalfLifeTeleportData>();
			__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private bool FlipY;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_730499402_0;

	private EntityQuery __query_730499402_1;

	private EntityQuery __query_730499402_2;

	private EntityQuery __query_730499402_3;

	private EntityQuery __query_730499402_4;

	private EntityQuery __query_730499402_5;

	private EntityQuery __query_730499402_6;

	private EntityQuery __query_730499402_7;

	private EntityQuery __query_730499402_8;

	private void InitFlipY(ref Spell1021MagicBreakerData spell, in SpellMovementComponentData movement, ref SpellConfigComponentData config, bool IsSplitSpell)
	{
		if (IsSplitSpell)
		{
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				spell.FlipY = true;
			}
			else
			{
				spell.FlipY = config.Int3 == 1;
			}
		}
		else if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			spell.FlipY = true;
		}
		else
		{
			FlipY = !FlipY;
			spell.FlipY = FlipY;
			config.Int3 = (FlipY ? 1 : (-1));
		}
	}

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<SpellEffectSystem.Require>();
		RequireForUpdate<CurrentRoomEntitiesSingleton>();
		RequireForUpdate<GlobalRandom>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<GlobalParticleEmitParams>();
		RequireForUpdate<PlayerController_Dots>();
		RequireForUpdate<Spell1021MagicBreakerData>();
	}

	[Preserve]
	protected unsafe override void OnUpdate()
	{
		int num = Shader.PropertyToID("_Process");
		int nameID = Shader.PropertyToID("_EnableHiddenUnderGround");
		int nameID2 = Shader.PropertyToID("_GroundHiddenHeight");
		EntityCommandBuffer entityCommandBuffer = __query_730499402_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
		CurrentRoomEntitiesSingleton singleton = __query_730499402_3.GetSingleton<CurrentRoomEntitiesSingleton>();
		GlobalRandom singleton2 = __query_730499402_4.GetSingleton<GlobalRandom>();
		SpellSingleton singleton3 = __query_730499402_5.GetSingleton<SpellSingleton>();
		float3 to = __query_730499402_6.GetSingleton<PlayerController_Dots>().mousePosition;
		Entity singletonEntity = __query_730499402_7.GetSingletonEntity();
		NativeList<Spell1021HitTrigger> nativeList = new NativeList<Spell1021HitTrigger>(Allocator.Temp);
		float deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
		InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData> item3;
		InternalCompilerInterface.UncheckedRefRW<LocalTransform> item4;
		InternalCompilerInterface.UncheckedRefRW<SpellComponentData> item7;
		Entity target;
		foreach (QueryEnumerableWithEntity<EnabledRefRW<Spell1021InitEffectTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item9 in IFE_730499402_0.Query(__query_730499402_0, __TypeHandle.__IFE_730499402_0_TypeHandle, ref base.CheckedStateRef))
		{
			item9.Deconstruct(out var item, out var item2, out item3, out item4, out var item5, out var item6, out item7, out target);
			EnabledRefRW<Spell1021InitEffectTag> enabledRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW4 = item5;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW5 = item6;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW6 = item7;
			Entity entity = target;
			enabledRefRW.ValueRW = false;
			uncheckedRefRW4.ValueRW.Duration.Extra += 99999f;
			if ((1f + uncheckedRefRW4.ValueRO.Radius.AddRatio) * uncheckedRefRW4.ValueRO.Radius.MulRatio < 0.75f)
			{
				uncheckedRefRW4.ValueRW.Radius.AddRatio = -0.25f;
				uncheckedRefRW4.ValueRW.Radius.MulRatio = 1f;
			}
			float damage = singleton3.Configs[uncheckedRefRW4.ValueRO.Id].damage;
			float num2 = SpellTools.CallulateSpellScaleByDamage(damage, uncheckedRefRW4.ValueRO.Damage.Calculate());
			if (damage != 0f)
			{
				uncheckedRefRW3.ValueRW.Scale = num2;
			}
			uncheckedRefRW3.ValueRW.Scale += InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity).ExtraSizeRatio * num2;
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RW_ComponentLookup, ref base.CheckedStateRef, entity, value: false);
			InitFlipY(ref uncheckedRefRW2.ValueRW, in uncheckedRefRW5.ValueRO, ref uncheckedRefRW4.ValueRW, uncheckedRefRW6.ValueRO.IsSplitSpell);
			FixedString32Bytes seName = "Slash";
			entityCommandBuffer.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(1021, in seName), SEPlayMode.Replay, 3, 0.05f, singleton2.random.NextFloat(0.7f, 1.3f)));
			uncheckedRefRW4.ValueRW.ColorType.ColorEnumToString(out var result);
			string arg = (uncheckedRefRW5.ValueRO.IsFallSpell ? "FallSpell" : "NormalSpell");
			string path = string.Format("{0}1021/1021_{1}_{2}", "Prefabs/Spell/", arg, result);
			string text = (uncheckedRefRW5.ValueRO.IsFallSpell ? "Shadow" : "NormalShadow");
			string text2 = (uncheckedRefRW5.ValueRO.IsFallSpell ? "" : $"_{result}");
			string path2 = "Prefabs/Spell/1021/1021_" + text + text2;
			if (!uncheckedRefRW5.ValueRO.IsFallSpell)
			{
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellRemoteShootTag_RO_ComponentLookup, ref base.CheckedStateRef, entity))
				{
					uncheckedRefRW5.ValueRW.AroundCenter = uncheckedRefRW3.ValueRO.Position;
					uncheckedRefRW5.ValueRW.AroundTarget = Entity.Null;
				}
				uncheckedRefRW3.ValueRW.Position += uncheckedRefRW5.ValueRO.Direction * 0.01f;
				if (!uncheckedRefRW6.ValueRO.IsSplitSpell)
				{
					float3 @float = uncheckedRefRW5.ValueRO.Direction;
					switch (uncheckedRefRW5.ValueRO.Type)
					{
					case SpellSpecialMovementType.Rotation:
						@float = DTool.GetDir(singleton2.random.NextFloat(360f));
						break;
					case SpellSpecialMovementType.ChaseMouse:
						@float = DTool.IgnoreZDir(in to, in uncheckedRefRW5.ValueRO.AroundCenter);
						break;
					case SpellSpecialMovementType.ChaseOwner:
						if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW6.ValueRO.Shooter))
						{
							uncheckedRefRW5.ValueRW.ChaseOwnerPosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW6.ValueRO.Shooter).Position;
						}
						@float = DTool.IgnoreZDir(in uncheckedRefRW5.ValueRO.ChaseOwnerPosition, in uncheckedRefRW3.ValueRO.Position);
						break;
					case SpellSpecialMovementType.ChaseEnemy:
					{
						@float = ((!singleton.FindNearestTarget(uncheckedRefRW5.ValueRO.AroundCenter, uncheckedRefRW4.ValueRO.ShooterType, out target, out var targetPosition, out var _)) ? uncheckedRefRW5.ValueRO.Direction : DTool.IgnoreZDir(in targetPosition, in uncheckedRefRW5.ValueRO.AroundCenter));
						break;
					}
					}
					uncheckedRefRW2.ValueRW.SourceDirection = @float;
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__MultiShootData_RO_ComponentLookup, ref base.CheckedStateRef, entity))
					{
						@float = DTool.RotateDir(@float, singleton2.random.NextFloat(-6f, 6f) * (float)InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__MultiShootData_RO_ComponentLookup, ref base.CheckedStateRef, entity).Count);
					}
					bool revert = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellRevertDirection_RO_ComponentLookup, ref base.CheckedStateRef, entity).Revert;
					@float *= (float)((!revert) ? 1 : (-1));
					float num3 = uncheckedRefRW5.ValueRO.Speed / SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].speed;
					if (uncheckedRefRW4.ValueRO.Scatter > SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].angle)
					{
						num3 *= 1f + (uncheckedRefRW4.ValueRO.Scatter - SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].angle) * 0.001f;
					}
					uncheckedRefRW2.ValueRW.SlashTime = uncheckedRefRW4.ValueRO.Scatter / 360f / num3;
					uncheckedRefRW5.ValueRW.Direction = DTool.RotateDir(@float, uncheckedRefRW4.ValueRO.Scatter / 2f * (float)(uncheckedRefRW2.ValueRO.FlipY ? 1 : (-1)) * -1f);
					uncheckedRefRW2.ValueRW.BaseDirection = uncheckedRefRW5.ValueRO.Direction;
					ref LocalTransform valueRW = ref uncheckedRefRW3.ValueRW;
					float2 dir = uncheckedRefRW5.ValueRO.Direction.xy;
					valueRW.Rotation = DTool.DirectionToRotation(in dir);
				}
				else
				{
					float num4 = uncheckedRefRW5.ValueRO.Speed / SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].speed;
					if (uncheckedRefRW4.ValueRO.Scatter > SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].angle)
					{
						num4 *= 1f + (uncheckedRefRW4.ValueRO.Scatter - SpellConfig.dic[uncheckedRefRW4.ValueRO.Id].angle) * 0.001f;
					}
					uncheckedRefRW2.ValueRW.SlashTime = uncheckedRefRW4.ValueRO.Scatter / 360f / num4;
					uncheckedRefRW2.ValueRW.BaseDirection = uncheckedRefRW5.ValueRO.Direction;
					ref LocalTransform valueRW2 = ref uncheckedRefRW3.ValueRW;
					float2 dir = uncheckedRefRW5.ValueRO.Direction.xy;
					valueRW2.Rotation = DTool.DirectionToRotation(in dir);
				}
				uncheckedRefRW5.ValueRW.Gravity = 0f;
				uncheckedRefRW.ValueRW = PhysicsVelocity.Zero;
				if (uncheckedRefRW6.ValueRW.IsSplitSpell)
				{
					uncheckedRefRW5.ValueRW.AroundRadius *= 0.5f;
				}
				float num5 = ((uncheckedRefRW5.ValueRO.Type != SpellSpecialMovementType.Rotation) ? 0.05f : uncheckedRefRW5.ValueRO.AroundRadius);
				uncheckedRefRW3.ValueRW.Position = uncheckedRefRW5.ValueRO.AroundCenter + uncheckedRefRW5.ValueRO.Direction * num5;
				uncheckedRefRW6.ValueRW.SpellEffectGameObject = ObjPoolMgr.Inst.GetGO(path, uncheckedRefRW3.ValueRO.Position, uncheckedRefRW3.ValueRO.Rotation);
				uncheckedRefRW6.ValueRW.SpellEffectGameObject.Value.transform.localScale = Vector3.one * uncheckedRefRW3.ValueRO.Scale;
				uncheckedRefRW6.ValueRW.TrailEffectGameObject = ObjPoolMgr.Inst.GetGO(path2, new Vector3(9999f, 9999f, 9999f), uncheckedRefRW3.ValueRO.Rotation);
				uncheckedRefRW6.ValueRW.TrailEffectGameObject.Value.transform.localScale = Vector3.one * uncheckedRefRW3.ValueRO.Scale;
				SpriteRenderer component = uncheckedRefRW6.ValueRW.SpellEffectGameObject.Value.transform.GetComponent<SpriteRenderer>();
				component.flipY = uncheckedRefRW2.ValueRO.FlipY;
				component.material.SetFloat(0, num);
				component.material.DOFloat(1f, num, 0.2f).SetEase(Ease.OutCirc);
				float num6 = math.max(0.75f, (1f + uncheckedRefRW4.ValueRO.Radius.AddRatio) * uncheckedRefRW4.ValueRO.Radius.MulRatio * (uncheckedRefRW6.ValueRO.IsSplitSpell ? 0.8f : 1f));
				component.size = new Vector2(2.56f * num6, 2.56f);
				Spell1021HitTrigger value = new Spell1021HitTrigger
				{
					entity = entity,
					size = new float3(2.56f * num6, 0.5f, 10f) * uncheckedRefRW3.ValueRO.Scale
				};
				nativeList.Add(in value);
				GameObject gO = ObjPoolMgr.Inst.GetGO(string.Format("{0}1021/1021_TrailEmber_{1}", "Prefabs/Spell/", result), uncheckedRefRW3.ValueRO.Position, 0f, null, 30, $"1021_TrailEmber_{result}");
				uncheckedRefRW2.ValueRW.TrailEmber = gO;
				if (uncheckedRefRW2.ValueRW.TrailEmber.Value != null)
				{
					uncheckedRefRW2.ValueRW.TrailEmber.Value.transform.localScale = Vector3.one * uncheckedRefRW3.ValueRO.Scale;
					Transform transform = uncheckedRefRW2.ValueRW.TrailEmber.Value.transform;
					ParticleSystem component2 = transform.Find("Trail").GetComponent<ParticleSystem>();
					ParticleSystem.ShapeModule shape = component2.shape;
					float num7 = 1.28f * math.max(0.75f, num6);
					component2.gameObject.transform.localPosition = new Vector3(num7, 0f, component2.gameObject.transform.localPosition.z);
					shape.radius = num7;
					if (uncheckedRefRW4.ValueRO.ColorType == SpellColorType.Thunder)
					{
						ParticleSystem component3 = transform.Find("ThunderTrail").GetComponent<ParticleSystem>();
						shape = component3.shape;
						component3.gameObject.transform.localPosition = new Vector3(num7, 0f, component3.gameObject.transform.localPosition.z);
						shape.radius = num7;
					}
				}
				SpriteRenderer component4 = uncheckedRefRW6.ValueRW.TrailEffectGameObject.Value.transform.Find("Shadow").GetComponent<SpriteRenderer>();
				component4.transform.localPosition = Vector3.zero;
				component4.size = new Vector2(2.56f * num6, 2.56f);
				int num8 = (int)(uncheckedRefRW2.ValueRO.SlashTime * 0.8f / deltaTime);
				float num9 = math.tan(math.min(uncheckedRefRW4.ValueRO.Scatter / (float)num8, 30f) * (MathF.PI / 180f)) * 2.56f * num6;
				if (uncheckedRefRW4.ValueRO.Scatter <= 10f)
				{
					num9 = 0f;
				}
				float3 size = new float3(2.56f * num6, 0.5f + num9, 4f);
				ref PhysicsCollider valueRW3 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW;
				valueRW3.MakeUnique(in entity, base.EntityManager);
				Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)valueRW3.ColliderPtr;
				CollisionFilter collisionFilter = colliderPtr->GetCollisionFilter();
				BoxGeometry geometry = colliderPtr->Geometry;
				geometry.Size = size;
				geometry.Center = new float3(size.x / 2f, (uncheckedRefRW2.ValueRO.FlipY ? (-1f) : 1f) * num9 / 2f, 0f);
				colliderPtr->Geometry = geometry;
				colliderPtr->SetCollisionFilter(collisionFilter);
			}
			else
			{
				if (!uncheckedRefRW6.ValueRO.IsSplitSpell)
				{
					uncheckedRefRW3.ValueRW.Position.z = -10.5f;
				}
				SpellTools.DisableSpellTrigger(in InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref base.CheckedStateRef, entity).ValueRO);
				uncheckedRefRW5.ValueRW.FallingReboundForceRatio = 1.33f;
				if (!uncheckedRefRW6.ValueRO.IsSplitSpell)
				{
					float num10 = uncheckedRefRW5.ValueRO.CurrentFallSpeed + 25f;
					uncheckedRefRW5.ValueRW.OriginalSpellHorizontalSpeed = num10 * math.tan(MathF.PI / 12f);
					uncheckedRefRW5.ValueRW.CurrentFallSpeed = num10;
					uncheckedRefRW5.ValueRW.Speed = uncheckedRefRW5.ValueRW.OriginalSpellHorizontalSpeed;
				}
				else
				{
					uncheckedRefRW5.ValueRW.CurrentFallSpeed -= 5f;
				}
				uncheckedRefRW4.ValueRW.Float3 = uncheckedRefRW5.ValueRW.Speed * 0.7f;
				float3 float2 = uncheckedRefRW5.ValueRW.Speed * uncheckedRefRW5.ValueRW.Direction;
				if (uncheckedRefRW5.ValueRW.Type == SpellSpecialMovementType.Rotation)
				{
					float2 = uncheckedRefRW5.ValueRO.OriginalSpellHorizontalSpeed * uncheckedRefRW5.ValueRO.Direction;
				}
				float3 rootPosition = float2 + new float3(0f, 0f, uncheckedRefRW5.ValueRO.CurrentFallSpeed);
				float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				rootPosition += layerPosition;
				quaternion rotation2 = quaternion.Euler(0f, 0f, math.atan2(rootPosition.y, rootPosition.x));
				uncheckedRefRW3.ValueRW.Rotation = rotation2;
				uncheckedRefRW6.ValueRW.SpellEffectGameObject = ObjPoolMgr.Inst.GetGO(path, uncheckedRefRW3.ValueRO.Position, uncheckedRefRW3.ValueRO.Rotation);
				uncheckedRefRW6.ValueRW.SpellEffectGameObject.Value.transform.localScale = Vector3.one * uncheckedRefRW3.ValueRO.Scale;
				uncheckedRefRW6.ValueRW.TrailEffectGameObject = ObjPoolMgr.Inst.GetGO(path2, uncheckedRefRW3.ValueRO.Position).transform.GetChild(0).gameObject;
				uncheckedRefRW6.ValueRW.TrailEffectGameObject.Value.transform.parent.localScale = Vector3.one * uncheckedRefRW3.ValueRO.Scale;
				GameObject gO2 = ObjPoolMgr.Inst.GetGO(string.Format("{0}1021/1021_FallTrailEmber_{1}", "Prefabs/Spell/", result), uncheckedRefRW3.ValueRO.Position, 0f, null, GameMgr.IsMobile_Static ? 20 : 100, $"1021_FallTrailEmber_{result}");
				uncheckedRefRW2.ValueRW.FallTrailEmber = gO2;
				SpriteRenderer component5 = uncheckedRefRW6.ValueRW.SpellEffectGameObject.Value.transform.Find("Blade").GetComponent<SpriteRenderer>();
				component5.flipY = uncheckedRefRW2.ValueRO.FlipY;
				component5.material.SetFloat(0, num);
				component5.material.DOFloat(1f, num, 0.2f).SetEase(Ease.OutCirc);
				if (uncheckedRefRW5.ValueRO.ReboundCount <= 0)
				{
					component5.material.SetFloat(nameID, 1f);
					component5.material.SetFloat(nameID2, uncheckedRefRW3.ValueRO.Position.y);
				}
				else
				{
					component5.material.SetFloat(nameID, 0f);
				}
				component5.size = new Vector2(2.56f * math.max(0.75f, (1f + uncheckedRefRW4.ValueRO.Radius.AddRatio) * uncheckedRefRW4.ValueRO.Radius.MulRatio), 2.56f);
				component5.transform.localPosition = new Vector3((0f - component5.size.x) * singleton2.random.NextFloat(0.35f, 0.7f), 0f, 0f);
				if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity).ExtraSizeRatio > 0f)
				{
					uncheckedRefRW6.ValueRO.TrailEffectGameObject.Value.transform.parent.localScale = Vector3.zero;
				}
				entityCommandBuffer.AddBuffer<Spell1021HitTargetBuffer>(entity);
			}
		}
		nativeList.Dispose();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item10 in IFE_730499402_1.Query(__query_730499402_1, __TypeHandle.__IFE_730499402_1_TypeHandle, ref base.CheckedStateRef))
		{
			item10.Deconstruct(out item3, out var item8, out item7, out item4, out target);
			InternalCompilerInterface.UncheckedRefRW<Spell1021MagicBreakerData> uncheckedRefRW7 = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO = item8;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW8 = item7;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW9 = item4;
			Entity entity2 = target;
			if (InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell1021InitEffectTag_RO_ComponentLookup, ref base.CheckedStateRef, entity2))
			{
				continue;
			}
			if (uncheckedRefRO.ValueRO.IsFallSpell)
			{
				if (!uncheckedRefRW7.ValueRO.readyToDestroy)
				{
					uncheckedRefRW8.ValueRO.SpellEffectGameObject.Value.transform.rotation = uncheckedRefRW9.ValueRO.Rotation;
					float3 position2 = uncheckedRefRW9.ValueRO.Position;
					float3 float3 = position2;
					float3.y -= float3.z;
					float3.z = 0.01f;
					uncheckedRefRW8.ValueRO.SpellEffectGameObject.Value.transform.position = float3;
					UpdateTrailEmberPosition(ref uncheckedRefRW7.ValueRW, float3);
					position2.z = 0.9f;
					uncheckedRefRW8.ValueRO.TrailEffectGameObject.Value.transform.position = position2;
				}
				else if (uncheckedRefRW7.ValueRO.FallTrace == Entity.Null)
				{
					uncheckedRefRW9.ValueRW.Position.z = 0.012f;
					SpriteRenderer component6 = uncheckedRefRW8.ValueRW.SpellEffectGameObject.Value.transform.Find("Blade").GetComponent<SpriteRenderer>();
					component6.material.SetFloat(nameID, 1f);
					component6.material.SetFloat(nameID2, uncheckedRefRW9.ValueRO.Position.y);
					Entity entity3 = entityCommandBuffer.Instantiate(__query_730499402_5.GetSingleton<SpellSingleton>().Prefabs["1021_Trace"]);
					entityCommandBuffer.SetComponent(entity3, new LocalTransform
					{
						Position = uncheckedRefRW9.ValueRO.Position,
						Rotation = quaternion.identity,
						Scale = uncheckedRefRW9.ValueRO.Scale * 0.5f
					});
					uncheckedRefRW7.ValueRW.FallTrace = entity3;
				}
			}
			else if (!uncheckedRefRW7.ValueRO.readyToDestroy)
			{
				uncheckedRefRW8.ValueRO.SpellEffectGameObject.Value.transform.position = Tool2D.GetLayerPoint(uncheckedRefRW9.ValueRO.Position, LayerCorrectType.Coordinate) + ((uncheckedRefRO.ValueRO.AroundCenter.z == 0f) ? new Vector3(0f, 0.3f, 0f) : Vector3.zero);
				UpdateTrailEmberPosition(ref uncheckedRefRW7.ValueRW, uncheckedRefRW9.ValueRO.Position);
				uncheckedRefRW8.ValueRO.TrailEffectGameObject.Value.transform.position = Tool2D.GetLayerPoint(uncheckedRefRW9.ValueRO.Position, LayerCorrectType.Shadow);
				uncheckedRefRW8.ValueRO.SpellEffectGameObject.Value.transform.rotation = uncheckedRefRW9.ValueRO.Rotation;
				UpdateTrailEmberRotation(ref uncheckedRefRW7.ValueRW, uncheckedRefRW9.ValueRO.Rotation);
				uncheckedRefRW8.ValueRO.TrailEffectGameObject.Value.transform.rotation = uncheckedRefRW9.ValueRO.Rotation;
			}
		}
		base.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1021MoveJob
		{
			random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 10000)),
			LocalTransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef),
			CMD = entityCommandBuffer.AsParallelWriter(),
			Random = __query_730499402_4.GetSingleton<GlobalRandom>(),
			HalfLifeTeleportLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellHalfLifeTeleportData_RW_ComponentLookup, ref base.CheckedStateRef),
			DeltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime,
			GlobalParticle = __query_730499402_8.GetSingletonEntity()
		}, __TypeHandle.__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
		static void UpdateTrailEmberPosition(ref Spell1021MagicBreakerData data, float3 position)
		{
			if (data.TrailEmber.Value != null && data.TrailEmber.Value.activeInHierarchy)
			{
				data.TrailEmber.Value.transform.position = position;
			}
			else if (data.FallTrailEmber.Value != null && data.FallTrailEmber.Value.activeInHierarchy)
			{
				data.FallTrailEmber.Value.transform.position = position;
			}
		}
		static void UpdateTrailEmberRotation(ref Spell1021MagicBreakerData data, quaternion rotation)
		{
			if (data.TrailEmber.Value != null && data.TrailEmber.Value.activeInHierarchy)
			{
				data.TrailEmber.Value.transform.rotation = rotation;
			}
			else if (data.FallTrailEmber.Value != null && data.FallTrailEmber.Value.activeInHierarchy)
			{
				data.FallTrailEmber.Value.transform.rotation = rotation;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1021MoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1021MoveJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021MagicBreakerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021InitEffectTag>();
		__query_730499402_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021MagicBreakerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		__query_730499402_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_730499402_8 = entityQueryBuilder2.Build(ref state);
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
	public Spell1021MagicBreakerSystem()
	{
	}
}
