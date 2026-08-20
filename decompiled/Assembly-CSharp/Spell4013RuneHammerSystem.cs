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

[CompilerGenerated]
[UpdateInGroup(typeof(SpellSimulationSystemGroup))]
internal class Spell4013RuneHammerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_702560528_0
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
			public QueryEnumerableWithEntity<EnabledRefRO<Spell4013RuneHammerInitTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<EnabledRefRO<Spell4013RuneHammerInitTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>>(item1_EnabledMask.GetEnabledRefRO<Spell4013RuneHammerInitTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsVelocity>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4013RuneHammerData>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell4013RuneHammerInitTag> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<PhysicsVelocity> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item6_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell4013RuneHammerData> item7_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell4013RuneHammerInitTag>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsVelocity>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item7_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4013RuneHammerData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
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
				result.item1_EnabledMask = archetypeChunk.GetEnabledMask(ref item1_ComponentTypeHandle_RO);
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<EnabledRefRO<Spell4013RuneHammerInitTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<EnabledRefRO<Spell4013RuneHammerInitTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell4013RuneHammerInitTag>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsVelocity>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell4013RuneHammerData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_702560528_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4013RuneHammerData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4013TransformRightData>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpellComponentData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell4013RuneHammerData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell4013TransformRightData> item6_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4013RuneHammerData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4013TransformRightData>();
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
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell4013RuneHammerData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell4013TransformRightData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_702560528_0.TypeHandle __IFE_702560528_0_TypeHandle;

		public IFE_702560528_1.TypeHandle __IFE_702560528_1_TypeHandle;

		public ComponentLookup<HammerAttackRangeInWandSingleton> __HammerAttackRangeInWandSingleton_RW_ComponentLookup;

		public ComponentLookup<Spell4013RuneHammerInitTag> __Spell4013RuneHammerInitTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		public ComponentLookup<Spell4013RuneHammerData> __Spell4013RuneHammerData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellRevertDirection> __SpellRevertDirection_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4013PrefabCleanUpData> __Spell4013PrefabCleanUpData_RO_ComponentLookup;

		public ComponentLookup<Spell4013PrefabCleanUpData> __Spell4013PrefabCleanUpData_RW_ComponentLookup;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<Spell4013SpiltEntityData> __Spell4013SpiltEntityData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<Spell4013TransformRightData> __Spell4013TransformRightData_RW_ComponentLookup;

		public ComponentLookup<Spell4013SpiltEntityData> __Spell4013SpiltEntityData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_702560528_0_TypeHandle = new IFE_702560528_0.TypeHandle(ref state);
			__IFE_702560528_1_TypeHandle = new IFE_702560528_1.TypeHandle(ref state);
			__HammerAttackRangeInWandSingleton_RW_ComponentLookup = state.GetComponentLookup<HammerAttackRangeInWandSingleton>();
			__Spell4013RuneHammerInitTag_RW_ComponentLookup = state.GetComponentLookup<Spell4013RuneHammerInitTag>();
			__SpellNeedResize_RO_ComponentLookup = state.GetComponentLookup<SpellNeedResize>(isReadOnly: true);
			__SpellSplitComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__Spell4013RuneHammerData_RW_ComponentLookup = state.GetComponentLookup<Spell4013RuneHammerData>();
			__SpellRevertDirection_RO_ComponentLookup = state.GetComponentLookup<SpellRevertDirection>(isReadOnly: true);
			__Spell4013PrefabCleanUpData_RO_ComponentLookup = state.GetComponentLookup<Spell4013PrefabCleanUpData>(isReadOnly: true);
			__Spell4013PrefabCleanUpData_RW_ComponentLookup = state.GetComponentLookup<Spell4013PrefabCleanUpData>();
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
			__Spell4013SpiltEntityData_RO_ComponentLookup = state.GetComponentLookup<Spell4013SpiltEntityData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell4013TransformRightData_RW_ComponentLookup = state.GetComponentLookup<Spell4013TransformRightData>();
			__Spell4013SpiltEntityData_RW_ComponentLookup = state.GetComponentLookup<Spell4013SpiltEntityData>();
		}
	}

	private NativeHashMap<UnityObjectRef<Wand>, float> WandsAngleMap;

	private float BaseAngle;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_702560528_0;

	private EntityQuery __query_702560528_1;

	private EntityQuery __query_702560528_2;

	private EntityQuery __query_702560528_3;

	private EntityQuery __query_702560528_4;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<PhysicsWorldSingleton>();
		RequireForUpdate<SEData>();
		RequireForUpdate<ScreenShakeData>();
		RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<PlayerController_Dots>();
		WandsAngleMap = new NativeHashMap<UnityObjectRef<Wand>, float>(16, Allocator.Persistent);
		Entity entity = base.EntityManager.CreateSingleton<HammerAttackRangeInWandSingleton>();
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__HammerAttackRangeInWandSingleton_RW_ComponentLookup, ref base.CheckedStateRef, new HammerAttackRangeInWandSingleton
		{
			WandsFirstHammerAttackRange = new NativeHashMap<UnityObjectRef<Wand>, float>(16, Allocator.Persistent)
		}, entity);
		RequireForUpdate<Spell4013RuneHammerData>();
	}

	private void GetOverlayAngle(bool isSplit, in SpellComponentData data, in SpellMovementComponentData movement, int index, int total, float deltaTime, bool reserve, out float angle)
	{
		float num = BaseAngle / (movement.IsFallSpell ? 2f : 1f);
		if (index == 1 && !isSplit)
		{
			WandsAngleMap[data.Wand] += 30f * movement.Speed * deltaTime / (float)total;
		}
		float num2 = GetWandInitialIndependentAngle(in data) + WandsAngleMap[data.Wand];
		angle = num2 + num;
		angle += GetHammerSplitIndexAngleShift(index, total);
		angle *= (isSplit ? (-1f) : 1f);
		angle *= (reserve ? (-1f) : 1f);
	}

	private float GetHammerSplitIndexAngleShift(int index, int max)
	{
		if (max <= 0)
		{
			return 0f;
		}
		return 360f / (float)max * (float)index;
	}

	private float GetWandInitialIndependentAngle(in SpellComponentData data)
	{
		NativeList<UnityObjectRef<Wand>> list = new NativeList<UnityObjectRef<Wand>>(Allocator.Temp);
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg != null && wand.passiveRuneHammerEnable)
			{
				UnityObjectRef<Wand> value = wand;
				list.Add(in value);
			}
		}
		int length = list.Length;
		int num = list.IndexOf(data.Wand);
		list.Dispose();
		return 360f / (float)length * (float)num;
	}

	private float GetLengthRatio(in SpellConfigComponentData config)
	{
		return math.max(0.7f, 1f * (1f + config.Radius.AddRatio) * config.Radius.MulRatio);
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer entityCommandBuffer = __query_702560528_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.EntityManager.World.Unmanaged);
		float deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
		BaseAngle += 450f * deltaTime;
		SpellSingleton singleton = __query_702560528_3.GetSingleton<SpellSingleton>();
		NativeHashMap<UnityObjectRef<Wand>, NativeList<Entity>> nativeHashMap = new NativeHashMap<UnityObjectRef<Wand>, NativeList<Entity>>(0, Allocator.Temp);
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		NativeList<(Entity, Entity)> nativeList2 = new NativeList<(Entity, Entity)>(Allocator.Temp);
		NativeList<Entity> nativeList3 = new NativeList<Entity>(Allocator.Temp);
		Entity singletonEntity = __query_702560528_4.GetSingletonEntity();
		InternalCompilerInterface.UncheckedRefRW<LocalTransform> item3;
		InternalCompilerInterface.UncheckedRefRW<SpellComponentData> item4;
		InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> item5;
		InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> item6;
		InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData> item7;
		Entity entity;
		foreach (QueryEnumerableWithEntity<EnabledRefRO<Spell4013RuneHammerInitTag>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>> item10 in IFE_702560528_0.Query(__query_702560528_0, __TypeHandle.__IFE_702560528_0_TypeHandle, ref base.CheckedStateRef))
		{
			item10.Deconstruct(out var _, out var item2, out item3, out item4, out item5, out item6, out item7, out entity);
			InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW4 = item5;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW5 = item6;
			InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData> uncheckedRefRW6 = item7;
			Entity value = entity;
			Wand value2 = uncheckedRefRW3.ValueRO.Wand.Value;
			if ((bool)value2 && value2.WandCfg != null && value2.passiveRuneHammerEnable)
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell4013RuneHammerInitTag_RW_ComponentLookup, ref base.CheckedStateRef, value, value: false);
				uncheckedRefRW.ValueRW = PhysicsVelocity.Zero;
				if (!WandsAngleMap.ContainsKey(value2))
				{
					WandsAngleMap.Add(value2, 0f);
				}
				if (!nativeHashMap.ContainsKey(value2))
				{
					nativeHashMap.Add(value2, new NativeList<Entity>(Allocator.Temp) { in value });
				}
				else
				{
					nativeHashMap[value2].Add(in value);
				}
				uncheckedRefRW4.ValueRW.AroundRadius = Mathf.Max(uncheckedRefRW4.ValueRO.IsFallSpell ? 1.5f : 0.4f, uncheckedRefRW4.ValueRO.AroundRadius);
				float damage = singleton.Configs[uncheckedRefRW5.ValueRW.Id].damage;
				if (damage > 0f)
				{
					uncheckedRefRW2.ValueRW.Scale = SpellTools.CallulateSpellScaleByDamage(damage, uncheckedRefRW5.ValueRO.Damage.Calculate());
				}
				uncheckedRefRW2.ValueRW.Scale += InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, value).ExtraSizeRatio;
				ref Spell4013RuneHammerData valueRW = ref uncheckedRefRW6.ValueRW;
				float num = GetLengthRatio(in uncheckedRefRW5.ValueRO) - 1f;
				valueRW.HammerLength = 1.5f + num * 1.95f;
				uncheckedRefRW5.ValueRW.Radius.Base = valueRW.HammerLength;
				nativeList3.Add(in value);
				valueRW.IsRotateAroundWandSpirit = uncheckedRefRW3.ValueRW.Wand.Value.passiveAutoWand;
				if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref base.CheckedStateRef, value).Count > 0)
				{
					uncheckedRefRW5.ValueRW.Damage.MulRatio *= 0.33f;
					valueRW.HasSplitSpell = true;
					nativeList.Add(in value);
				}
				if (!uncheckedRefRW4.ValueRW.IsFallSpell && !valueRW.HasSplitSpell)
				{
					(Entity, Entity) value3 = (value, value);
					nativeList2.Add(in value3);
				}
				string arg = (uncheckedRefRW4.ValueRO.IsFallSpell ? "FallSpell" : "NormalSpell");
				uncheckedRefRW5.ValueRO.ColorType.ColorEnumToString(out var result);
				string path = string.Format("{0}4013/4013_{1}_{2}", "Prefabs/Spell/", arg, result);
				GameObject gO = ObjPoolMgr.Inst.GetGO(path, float3.zero);
				entityCommandBuffer.AddComponent<Spell4013PrefabCleanUpData>(value);
				entityCommandBuffer.SetComponent(value, new Spell4013PrefabCleanUpData
				{
					effectObject = gO,
					shadowObject = gO.transform.Find("Shadow").gameObject
				});
			}
		}
		foreach (Entity item11 in nativeList)
		{
			Entity Parent = item11;
			int count = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref base.CheckedStateRef, Parent).Count;
			SpellConfigComponentData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref base.CheckedStateRef, Parent);
			LocalTransform componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, Parent);
			SpellMovementComponentData componentAfterCompletingDependency3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref base.CheckedStateRef, Parent);
			bool isFallSpell = componentAfterCompletingDependency3.IsFallSpell;
			componentAfterCompletingDependency.ColorType.ColorEnumToString(out var result2);
			string text = (isFallSpell ? "FallSplit" : "Split");
			for (int i = 1; i <= count; i++)
			{
				EntityManager entityManager = base.EntityManager;
				SpellTools.SpawnChildIgnoreColor(in singleton, in entityManager, 4013, text, in Parent, out var child);
				entityCommandBuffer.SetComponent(child, new LocalTransform
				{
					Scale = componentAfterCompletingDependency2.Scale,
					Rotation = quaternion.identity,
					Position = float3.zero
				});
				base.EntityManager.AddComponent<Spell4013SpiltEntityData>(child);
				if (!isFallSpell)
				{
					(Entity, Entity) value3 = (Parent, child);
					nativeList2.Add(in value3);
				}
				nativeList3.Add(in child);
				string arg2 = (componentAfterCompletingDependency3.IsFallSpell ? "FallSpell" : "NormalSpell");
				string path2 = string.Format("{0}4013/4013_{1}_{2}", "Prefabs/Spell/", arg2, result2);
				GameObject gO2 = ObjPoolMgr.Inst.GetGO(path2, float3.zero);
				entityCommandBuffer.AddComponent<Spell4013PrefabCleanUpData>(child);
				entityCommandBuffer.SetComponent(child, new Spell4013PrefabCleanUpData
				{
					effectObject = gO2,
					shadowObject = gO2.transform.Find("Shadow").gameObject
				});
			}
		}
		foreach (Entity item12 in nativeList3)
		{
			base.EntityManager.AddComponent<Spell4013TransformRightData>(item12);
		}
		nativeList3.Dispose();
		foreach (KVPair<UnityObjectRef<Wand>, NativeList<Entity>> item13 in nativeHashMap)
		{
			ref NativeList<Entity> value4 = ref item13.Value;
			int num2 = 1;
			foreach (Entity item14 in value4)
			{
				ref Spell4013RuneHammerData valueRW2 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013RuneHammerData_RW_ComponentLookup, ref base.CheckedStateRef, item14).ValueRW;
				valueRW2.currentIndex = num2;
				valueRW2.maxHammerCount = value4.Length;
				num2++;
			}
			value4.Dispose();
		}
		nativeHashMap.Dispose();
		foreach (var (spell, entity2) in nativeList2)
		{
			singleton.Prefabs.TryGetValue("4013_HitTrigger", out var item8);
			Entity entity3 = base.EntityManager.Instantiate(item8);
			base.EntityManager.AddComponent<Spell4013HitTriggerEntity>(entity2);
			base.EntityManager.SetComponentData(entity2, new Spell4013HitTriggerEntity
			{
				Entity = entity3
			});
			base.EntityManager.SetComponentData(entity3, new Spell4013HitTriggerData
			{
				Spell = spell,
				Parent = entity2
			});
			base.EntityManager.GetBuffer<LinkedEntityGroup>(entity2).Add(new LinkedEntityGroup
			{
				Value = entity3
			});
		}
		nativeList2.Dispose();
		EntityCommandBuffer cmd = new EntityCommandBuffer(Allocator.Temp);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData>> item15 in IFE_702560528_1.Query(__query_702560528_1, __TypeHandle.__IFE_702560528_1_TypeHandle, ref base.CheckedStateRef))
		{
			item15.Deconstruct(out item4, out item7, out item5, out item3, out item6, out var item9, out entity);
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW7 = item4;
			InternalCompilerInterface.UncheckedRefRW<Spell4013RuneHammerData> uncheckedRefRW8 = item7;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW9 = item5;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW10 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW11 = item6;
			InternalCompilerInterface.UncheckedRefRW<Spell4013TransformRightData> uncheckedRefRW12 = item9;
			Entity entity4 = entity;
			if (uncheckedRefRW9.ValueRO.IsFallSpell)
			{
				uncheckedRefRW11.ValueRW.DamageTimer += base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
			}
			bool revert = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellRevertDirection_RO_ComponentLookup, ref base.CheckedStateRef, entity4).Revert;
			if (uncheckedRefRW9.ValueRW.Type != SpellSpecialMovementType.ChaseEnemy)
			{
				GetOverlayAngle(isSplit: false, in uncheckedRefRW7.ValueRO, in uncheckedRefRW9.ValueRO, uncheckedRefRW8.ValueRO.currentIndex, uncheckedRefRW8.ValueRO.maxHammerCount, deltaTime, revert, out var angle);
				uncheckedRefRW12.ValueRW.TransformRight = DTool.GetDir(MathF.PI / 180f * angle);
			}
			bool resize = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity4).ExtraSizeRatio > 0f;
			int count2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref base.CheckedStateRef, entity4).Count;
			RefRW<Spell4013PrefabCleanUpData> componentRWAfterCompletingDependency;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4013PrefabCleanUpData_RO_ComponentLookup, ref base.CheckedStateRef, entity4))
			{
				componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013PrefabCleanUpData_RW_ComponentLookup, ref base.CheckedStateRef, entity4);
				ref Spell4013PrefabCleanUpData valueRW3 = ref componentRWAfterCompletingDependency.ValueRW;
				if (!valueRW3.Initialized)
				{
					InitHammerGameObject(ref valueRW3, uncheckedRefRW11, uncheckedRefRW8, resize, count2, uncheckedRefRW10.ValueRO.Scale, isSplit: false, uncheckedRefRW9.ValueRO.IsFallSpell);
					CreateEmber(uncheckedRefRW11, in singleton, cmd, entity4, IsSplit: false, uncheckedRefRW10, uncheckedRefRW8.ValueRO);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__HammerAttackRangeInWandSingleton_RW_ComponentLookup, ref base.CheckedStateRef, singletonEntity).ValueRW.SetWandAttackRange(uncheckedRefRW7.ValueRO.Wand, 1.92f * GetLengthRatio(in uncheckedRefRW11.ValueRO) * uncheckedRefRW10.ValueRW.Scale);
				}
				else
				{
					valueRW3.effectObject.Value.transform.position = Tool2D.GetLayerPoint(uncheckedRefRW10.ValueRO.Position, LayerCorrectType.Coordinate);
					if (!uncheckedRefRW9.ValueRO.IsFallSpell)
					{
						valueRW3.effectObject.Value.transform.rotation = uncheckedRefRW10.ValueRO.Rotation;
					}
					valueRW3.effectObject.Value.transform.localScale = uncheckedRefRW10.ValueRO.Scale * Vector3.one;
					float3 layerPosition = DTool.GetLayerPosition(in uncheckedRefRW10.ValueRO.Position, LayerCorrectType.Shadow);
					layerPosition.xy = uncheckedRefRW10.ValueRO.Position.xy;
					valueRW3.shadowObject.Value.transform.position = layerPosition;
					SpellMovementComponentData valueRO = uncheckedRefRW9.ValueRO;
					if (valueRO.IsFallSpell && valueRO.Type == SpellSpecialMovementType.ChaseMouse)
					{
						valueRW3.shadowObject.Value.transform.position += new Vector3(0f, -0.5f, 0f);
					}
				}
			}
			DynamicBuffer<LinkedEntityGroup> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref base.CheckedStateRef, entity4);
			NativeList<Entity> nativeList4 = new NativeList<Entity>(Allocator.Temp);
			foreach (LinkedEntityGroup item16 in bufferAfterCompletingDependency)
			{
				LinkedEntityGroup current3 = item16;
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4013SpiltEntityData_RO_ComponentLookup, ref base.CheckedStateRef, current3.Value) && !(current3.Value == entity4))
				{
					nativeList4.Add(in current3.Value);
				}
			}
			int num3 = 1;
			foreach (Entity item17 in nativeList4)
			{
				ref LocalTransform valueRW4 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, item17).ValueRW;
				if (uncheckedRefRW9.ValueRW.Type != SpellSpecialMovementType.ChaseEnemy)
				{
					GetOverlayAngle(isSplit: true, in uncheckedRefRW7.ValueRO, in uncheckedRefRW9.ValueRO, num3, count2, deltaTime, revert, out var angle2);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013TransformRightData_RW_ComponentLookup, ref base.CheckedStateRef, item17).ValueRW.TransformRight = DTool.GetDir(MathF.PI / 180f * angle2);
				}
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4013PrefabCleanUpData_RO_ComponentLookup, ref base.CheckedStateRef, item17))
				{
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013PrefabCleanUpData_RW_ComponentLookup, ref base.CheckedStateRef, item17);
					ref Spell4013PrefabCleanUpData valueRW5 = ref componentRWAfterCompletingDependency.ValueRW;
					if (!valueRW5.Initialized)
					{
						InitHammerGameObject(ref valueRW5, uncheckedRefRW11, uncheckedRefRW8, resize, count2, uncheckedRefRW10.ValueRO.Scale, isSplit: true, uncheckedRefRW9.ValueRO.IsFallSpell);
						CreateEmber(uncheckedRefRW11, in singleton, cmd, item17, IsSplit: true, uncheckedRefRW10, uncheckedRefRW8.ValueRO);
					}
					else
					{
						valueRW5.effectObject.Value.transform.position = Tool2D.GetLayerPoint(valueRW4.Position, LayerCorrectType.Coordinate);
						if (!uncheckedRefRW9.ValueRO.IsFallSpell)
						{
							valueRW5.effectObject.Value.transform.rotation = valueRW4.Rotation;
						}
						valueRW5.effectObject.Value.transform.localScale = valueRW4.Scale * Vector3.one;
						float3 layerPosition2 = DTool.GetLayerPosition(in valueRW4.Position, LayerCorrectType.Shadow);
						layerPosition2.xy = valueRW4.Position.xy;
						valueRW5.shadowObject.Value.transform.position = layerPosition2;
						SpellMovementComponentData valueRO = uncheckedRefRW9.ValueRO;
						if (valueRO.IsFallSpell && valueRO.Type == SpellSpecialMovementType.ChaseMouse)
						{
							valueRW5.shadowObject.Value.transform.position += new Vector3(0f, -0.5f, 0f);
						}
					}
				}
				num3++;
			}
		}
		cmd.Playback(base.EntityManager);
		cmd.Dispose();
	}

	private void CreateEmber(RefRW<SpellConfigComponentData> config, in SpellSingleton spellSingleton, EntityCommandBuffer cmd, Entity spell, bool IsSplit, RefRW<LocalTransform> transform, Spell4013RuneHammerData data)
	{
		_ = data.HammerLength / 2f;
		config.ValueRO.ColorType.ColorEnumToString(out var result);
		Entity entity = base.EntityManager.Instantiate(spellSingleton.Prefabs[$"4013_Ember_{result}"]);
		cmd.SetComponent(entity, new LocalTransform
		{
			Scale = transform.ValueRO.Scale
		});
		cmd.AppendToBuffer(spell, new LinkedEntityGroup
		{
			Value = entity
		});
		if (!IsSplit)
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013RuneHammerData_RW_ComponentLookup, ref base.CheckedStateRef, spell).ValueRW.EmberEntity = entity;
		}
		else
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4013SpiltEntityData_RW_ComponentLookup, ref base.CheckedStateRef, spell).ValueRW.EmberEntity = entity;
		}
		cmd.SetComponent(entity, new GlobalParticle.Emitter
		{
			ParticleName = $"4013_GEmber_{result}",
			RandomPositionOffset = data.HammerLength / 2f
		});
	}

	private void InitHammerGameObject(ref Spell4013PrefabCleanUpData prefabCleanUpData, RefRW<SpellConfigComponentData> config, RefRW<Spell4013RuneHammerData> hammerData, bool Resize, int splitCount, float scale, bool isSplit, bool IsFallSpell)
	{
		bool flag = !hammerData.ValueRO.HasSplitSpell || isSplit;
		float lengthRatio = GetLengthRatio(in config.ValueRO);
		Vector3 localPosition = new Vector3(hammerData.ValueRW.HammerLength, 0f, 0f);
		if (!IsFallSpell)
		{
			Transform transform = prefabCleanUpData.effectObject.Value.transform;
			float y = lengthRatio * 1.5f * 1.28f;
			SpriteRenderer component = transform.Find("Hammer").GetComponent<SpriteRenderer>();
			component.size = new Vector2(1.28f, y);
			SpriteRenderer component2 = transform.Find("HammerBase").GetComponent<SpriteRenderer>();
			component2.size = new Vector2(1.28f, y);
			SpriteRenderer component3 = transform.Find("HammerStick").GetComponent<SpriteRenderer>();
			component3.size = new Vector2(1.28f, y);
			SpriteRenderer component4 = transform.Find("HammerStickBase").GetComponent<SpriteRenderer>();
			component4.size = new Vector2(1.28f, y);
			transform.GetComponent<Spell4013HammerTransparencyController>().SetHammerBonusTransparencyRatio(GetHammerBonusTransparencyRadio());
			component.enabled = flag;
			component2.enabled = flag;
			component3.enabled = !flag;
			component4.enabled = !flag;
			int num = hammerData.ValueRO.maxHammerCount * (splitCount + 1);
			bool active = flag;
			if (!isSplit)
			{
				if (num == 1 || (flag && (float)num > 1.95f && (hammerData.ValueRO.currentIndex - 1) % 2 == 0))
				{
					active = false;
				}
			}
			else if (hammerData.ValueRO.currentIndex % 2 == 0)
			{
				active = false;
			}
			transform.Find("Trail").gameObject.SetActive(active);
			transform.Find("Trail").transform.localPosition = localPosition;
			Transform transform2 = transform.Find("CenterEmber");
			transform2.gameObject.SetActive(flag);
			transform2.localPosition = localPosition;
			foreach (Transform item in transform2)
			{
				ParticleSystem component5 = item.GetComponent<ParticleSystem>();
				if ((bool)component5)
				{
					component5.Clear();
					component5.Play();
				}
			}
			Transform transform3 = transform.Find("Shadow");
			SpriteRenderer component6 = transform3.transform.Find("HammerBase").GetComponent<SpriteRenderer>();
			SpriteRenderer component7 = transform3.transform.Find("StickHammerBase").GetComponent<SpriteRenderer>();
			component6.size = new Vector2(1.28f, y);
			component7.size = new Vector2(1.28f, y);
			component6.enabled = flag;
			component7.enabled = !flag;
			if (Resize)
			{
				component6.enabled = false;
				component7.enabled = false;
			}
			prefabCleanUpData.Initialized = true;
		}
		else
		{
			Transform transform4 = prefabCleanUpData.effectObject.Value.transform;
			SpriteRenderer component8 = transform4.Find("Hammer").GetComponent<SpriteRenderer>();
			SpriteRenderer component9 = transform4.Find("HammerBase").GetComponent<SpriteRenderer>();
			SpriteRenderer component10 = transform4.Find("HammerStick").GetComponent<SpriteRenderer>();
			SpriteRenderer component11 = transform4.Find("HammerStickBase").GetComponent<SpriteRenderer>();
			transform4.GetComponent<Spell4013HammerTransparencyController>().SetHammerBonusTransparencyRatio(GetHammerBonusTransparencyRadio());
			component8.enabled = flag;
			component9.enabled = flag;
			component10.enabled = !flag;
			component11.enabled = !flag;
			transform4.Find("Shadow").Find("Shadow").Find("HammerBase")
				.GetComponent<SpriteRenderer>()
				.enabled = !Resize;
			prefabCleanUpData.Initialized = true;
		}
		float GetHammerBonusTransparencyRadio()
		{
			float num2 = 1f;
			num2 -= math.min((scale - 1f) * 0.1f, 0.6f);
			num2 -= math.min((float)(hammerData.ValueRO.maxHammerCount * (splitCount + 1) - 1) * 0.02f, 0.6f);
			return math.max(0.2f, num2);
		}
	}

	[Preserve]
	protected override void OnDestroy()
	{
		WandsAngleMap.Dispose();
		Entity singletonEntity = __query_702560528_4.GetSingletonEntity();
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__HammerAttackRangeInWandSingleton_RW_ComponentLookup, ref base.CheckedStateRef, singletonEntity).ValueRW.WandsFirstHammerAttackRange.Dispose();
		base.EntityManager.DestroyEntity(singletonEntity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell4013RuneHammerInitTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4013RuneHammerData>();
		__query_702560528_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4013RuneHammerData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4013TransformRightData>();
		__query_702560528_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_702560528_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_702560528_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<HammerAttackRangeInWandSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_702560528_4 = entityQueryBuilder2.Build(ref state);
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
	public Spell4013RuneHammerSystem()
	{
	}
}
