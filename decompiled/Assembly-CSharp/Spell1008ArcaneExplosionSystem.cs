using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
internal struct Spell1008ArcaneExplosionSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1979089071_0
	{
		public struct ResolvedChunk
		{
			public EnabledMask item1_EnabledMask;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public BufferAccessor<SpellGameObjectEffectLink> item6_BufferAccessor;

			public IntPtr item7_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<EnabledRefRO<Spell1008InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1008ArcaneExplosionData>, DynamicBuffer<SpellGameObjectEffectLink>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>> Get(int index)
			{
				return new QueryEnumerableWithEntity<EnabledRefRO<Spell1008InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1008ArcaneExplosionData>, DynamicBuffer<SpellGameObjectEffectLink>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>>(item1_EnabledMask.GetEnabledRefRO<Spell1008InitializedTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1008ArcaneExplosionData>(item5_IntPtr, index), item6_BufferAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsVelocity>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1008InitializedTag> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<Spell1008ArcaneExplosionData> item5_ComponentTypeHandle_RW;

			private BufferTypeHandle<SpellGameObjectEffectLink> item6_BufferTypeHandle_RW;

			private ComponentTypeHandle<PhysicsVelocity> item7_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1008InitializedTag>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1008ArcaneExplosionData>();
				item6_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<SpellGameObjectEffectLink>();
				item7_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsVelocity>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_BufferTypeHandle_RW.Update(ref systemState);
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
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item6_BufferTypeHandle_RW);
				result.item7_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item7_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<EnabledRefRO<Spell1008InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1008ArcaneExplosionData>, DynamicBuffer<SpellGameObjectEffectLink>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<EnabledRefRO<Spell1008InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1008ArcaneExplosionData>, DynamicBuffer<SpellGameObjectEffectLink>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1008InitializedTag>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1008ArcaneExplosionData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellGameObjectEffectLink>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsVelocity>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1979089071_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<Spell1008HitExplosionEffectData> item3_BufferAccessor;

			public EnabledMask item4_EnabledMask;

			public IntPtr item5_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<Spell1008ArcaneExplosionData, SpellConfigComponentData, DynamicBuffer<Spell1008HitExplosionEffectData>, EnabledRefRO<SpellGroundedTag>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<Spell1008ArcaneExplosionData, SpellConfigComponentData, DynamicBuffer<Spell1008HitExplosionEffectData>, EnabledRefRO<SpellGroundedTag>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Spell1008ArcaneExplosionData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellConfigComponentData>(item2_IntPtr, index), item3_BufferAccessor[index], item4_EnabledMask.GetEnabledRefRO<SpellGroundedTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1008ArcaneExplosionData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RO;

			private BufferTypeHandle<Spell1008HitExplosionEffectData> item3_BufferTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellGroundedTag> item4_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item5_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1008ArcaneExplosionData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<Spell1008HitExplosionEffectData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellGroundedTag>(isReadOnly: true);
				item5_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.item4_EnabledMask = archetypeChunk.GetEnabledMask(ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<Spell1008ArcaneExplosionData, SpellConfigComponentData, DynamicBuffer<Spell1008HitExplosionEffectData>, EnabledRefRO<SpellGroundedTag>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<Spell1008ArcaneExplosionData, SpellConfigComponentData, DynamicBuffer<Spell1008HitExplosionEffectData>, EnabledRefRO<SpellGroundedTag>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1008ArcaneExplosionData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1008HitExplosionEffectData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellGroundedTag>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1979089071_0.TypeHandle __IFE_1979089071_0_TypeHandle;

		public IFE_1979089071_1.TypeHandle __IFE_1979089071_1_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public BufferLookup<SpellEffectSystem.Require> __SpellEffectSystem_Require_RW_BufferLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<Spell1008SpellMaterialProperty> __Spell1008SpellMaterialProperty_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public Spell1008Job.InternalCompilerQueryAndHandleData __Spell1008Job_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell1008OnGroundJob.InternalCompilerQueryAndHandleData __Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<Spell1008FallData> __Spell1008FallData_RW_ComponentLookup;

		public Spell1008FallJob.InternalCompilerQueryAndHandleData __Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle;

		public BufferLookup<Spell1008HitTargetsData> __Spell1008HitTargetsData_RW_BufferLookup;

		public Spell1008TakeDamageJob.InternalCompilerQueryAndHandleData __Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1979089071_0_TypeHandle = new IFE_1979089071_0.TypeHandle(ref state);
			__IFE_1979089071_1_TypeHandle = new IFE_1979089071_1.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellEffectSystem_Require_RW_BufferLookup = state.GetBufferLookup<SpellEffectSystem.Require>();
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell1008SpellMaterialProperty_RW_ComponentLookup = state.GetComponentLookup<Spell1008SpellMaterialProperty>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell1008FallData_RW_ComponentLookup = state.GetComponentLookup<Spell1008FallData>();
			__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell1008HitTargetsData_RW_BufferLookup = state.GetBufferLookup<Spell1008HitTargetsData>();
			__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private EntityQuery _unitQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1979089071_0;

	private EntityQuery __query_1979089071_1;

	private EntityQuery __query_1979089071_2;

	private EntityQuery __query_1979089071_3;

	private EntityQuery __query_1979089071_4;

	private EntityQuery __query_1979089071_5;

	private EntityQuery __query_1979089071_6;

	private EntityQuery __query_1979089071_7;

	private EntityQuery __query_1979089071_8;

	private EntityQuery __query_1979089071_9;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellEffectSystem.Destroy>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<Spell1008HitExplosionEffectData>();
		_unitQuery = state.EntityManager.CreateEntityQuery(typeof(UnitProperty_Dots), typeof(LocalTransform));
	}

	public void OnUpdate(ref SystemState state)
	{
		DynamicBuffer<SEData> singletonBuffer = __query_1979089071_2.GetSingletonBuffer<SEData>();
		NativeArray<UnitProperty_Dots> nativeArray = _unitQuery.ToComponentDataArray<UnitProperty_Dots>(Allocator.TempJob);
		NativeArray<LocalTransform> nativeArray2 = _unitQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
		NativeArray<Entity> nativeArray3 = _unitQuery.ToEntityArray(Allocator.TempJob);
		EntityCommandBuffer entityCommandBuffer = __query_1979089071_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		EntityCommandBuffer.ParallelWriter cmd = entityCommandBuffer.AsParallelWriter();
		SpellSingleton spellSingleton = __query_1979089071_4.GetSingleton<SpellSingleton>();
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		Entity singletonEntity = __query_1979089071_5.GetSingletonEntity();
		Entity singletonEntity2 = __query_1979089071_6.GetSingletonEntity();
		Entity singletonEntity3 = __query_1979089071_7.GetSingletonEntity();
		Entity entity;
		foreach (QueryEnumerableWithEntity<EnabledRefRO<Spell1008InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1008ArcaneExplosionData>, DynamicBuffer<SpellGameObjectEffectLink>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>> item14 in IFE_1979089071_0.Query(__query_1979089071_0, __TypeHandle.__IFE_1979089071_0_TypeHandle, ref state))
		{
			item14.Deconstruct(out var _, out var item2, out var item3, out var item4, out var _, out var item6, out var item7, out entity);
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO = item4;
			DynamicBuffer<SpellGameObjectEffectLink> dynamicBuffer = item6;
			InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity> uncheckedRefRW3 = item7;
			Entity value = entity;
			FixedString32Bytes seName;
			if (!uncheckedRefRW.ValueRO.IsFallSpell)
			{
				if (uncheckedRefRW2.ValueRO.SpellEffectEntity == Entity.Null)
				{
					nativeList.Add(in value);
					continue;
				}
				entityCommandBuffer.SetComponentEnabled<Spell1008InitializedTag>(value, value: false);
				entityCommandBuffer.AppendToBuffer(singletonEntity3, new ScreenShakeData
				{
					Radius = 0.05f,
					Speed = 5f,
					Time = 0.1f
				});
				seName = "Shoot";
				singletonBuffer.Add(new SEData(DTool.GetSpellSEName(1008, in seName)));
				float3 rootPosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value).Position;
				float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
				seName = "ExplosionTrail";
				float3 position = rootPosition + layerPosition;
				float scale = uncheckedRefRO.ValueRO.Radius.Calculate();
				cmd.CreateSpellGlobalParticle(0, in seName, in position, in scale, in uncheckedRefRO.ValueRO, in uncheckedRefRW2.ValueRO, in spellSingleton, in float3.zero);
				entityCommandBuffer.AppendToBuffer(value, new Spell1008TakeDamageBuffer
				{
					HitPosition = rootPosition + new float3(0f, 0f, -0.3f),
					IsFullRangeDamage = false,
					EffectEntity = uncheckedRefRW2.ValueRO.SpellEffectEntity
				});
				continue;
			}
			seName = "FallStart";
			singletonBuffer.Add(new SEData(DTool.GetSpellSEName(1008, in seName)));
			bool flag = false;
			foreach (SpellGameObjectEffectLink item15 in dynamicBuffer)
			{
				SpellGameObjectEffectLink current = item15;
				if (current.EffectName == "FallSpell")
				{
					ref UnityObjectRef<GameObject> trailEffectGameObject = ref uncheckedRefRW2.ValueRW.TrailEffectGameObject;
					UnityObjectRef<GameObject> gameObject = current.GameObject;
					trailEffectGameObject.Value = gameObject.Value;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				entityCommandBuffer.SetComponentEnabled<Spell1008InitializedTag>(value, value: false);
				continue;
			}
			uncheckedRefRW.ValueRW.OriginalSpellHorizontalSpeed += 8f;
			uncheckedRefRW.ValueRW.Gravity = 20f;
			uncheckedRefRW.ValueRW.CurrentFallSpeed = uncheckedRefRW.ValueRW.OriginalSpellHorizontalSpeed * (float)((!uncheckedRefRW2.ValueRO.IsSplitSpell) ? 1 : (-1));
			uncheckedRefRW.ValueRW.Speed = uncheckedRefRW.ValueRW.CurrentFallSpeed / math.tan(1.3089969f) * (float)((!uncheckedRefRW2.ValueRO.IsSplitSpell) ? 1 : (-1));
			uncheckedRefRO.ValueRO.ColorType.ColorEnumToString(out var result);
			uncheckedRefRW3.ValueRW.Linear = uncheckedRefRW.ValueRO.Speed * uncheckedRefRW.ValueRO.Direction;
			InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellEffectSystem_Require_RW_BufferLookup, ref state, singletonEntity).Add(new SpellEffectSystem.Require
			{
				Entity = value,
				Settings = new SpellEffect
				{
					Name = "FallSpell",
					ClearParticle = true,
					ClearTrail = true,
					Layer = LayerCorrectType.Coordinate
				},
				Color = result,
				SpellId = 1008
			});
		}
		foreach (QueryEnumerableWithEntity<Spell1008ArcaneExplosionData, SpellConfigComponentData, DynamicBuffer<Spell1008HitExplosionEffectData>, EnabledRefRO<SpellGroundedTag>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> item16 in IFE_1979089071_1.Query(__query_1979089071_1, __TypeHandle.__IFE_1979089071_1_TypeHandle, ref state))
		{
			item16.Deconstruct(out var _, out var item9, out var item10, out var _, out var item12, out entity);
			SpellConfigComponentData spellConfig = item9;
			DynamicBuffer<Spell1008HitExplosionEffectData> dynamicBuffer2 = item10;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO2 = item12;
			Entity entity2 = entity;
			float3 rootPosition2 = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity2).ValueRO.Position;
			spellConfig.ColorType.ColorEnumToString(out var result2);
			if (spellSingleton.Prefabs.TryGetValue($"1008_Explosion_{result2}", out var item13))
			{
				item13 = state.EntityManager.Instantiate(item13);
				state.EntityManager.SetComponentData(item13, new LocalTransform
				{
					Position = rootPosition2,
					Rotation = quaternion.identity,
					Scale = 0f
				});
				FixedString32Bytes seName = "Shoot";
				singletonBuffer.Add(new SEData(DTool.GetSpellSEName(1008, in seName)));
				dynamicBuffer2.Add(new Spell1008HitExplosionEffectData
				{
					EffectEntity = item13
				});
				float3 layerPosition2 = DTool.GetLayerPosition(in rootPosition2, LayerCorrectType.Coordinate);
				seName = "ExplosionTrail";
				float3 position = rootPosition2 + layerPosition2;
				float scale = spellConfig.Radius.Calculate();
				cmd.CreateSpellGlobalParticle(0, in seName, in position, in scale, in spellConfig, in uncheckedRefRO2.ValueRO, in spellSingleton, in float3.zero);
				entityCommandBuffer.AddComponent(item13, new Spell1008FallData
				{
					Radius = spellConfig.Radius.Calculate(),
					IsVoidColor = (spellConfig.ColorType == SpellColorType.Void),
					SpellEntity = entity2,
					HoverDuration = spellConfig.HoverDuration
				});
				entityCommandBuffer.AppendToBuffer(entity2, new Spell1008TakeDamageBuffer
				{
					HitPosition = rootPosition2 + new float3(0f, 0f, -0.3f),
					IsFullRangeDamage = false,
					EffectEntity = item13
				});
			}
		}
		foreach (Entity item17 in nativeList)
		{
			Entity Parent = item17;
			ref SpellComponentData valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, Parent).ValueRW;
			InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, Parent).ValueRO.ColorType.ColorEnumToString(out var result3);
			EntityManager entityManager = state.EntityManager;
			SpellTools.SpawnChild(in spellSingleton, in entityManager, 1008, "Explosion", result3, in Parent, out valueRW.SpellEffectEntity);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, new LocalTransform
			{
				Position = float3.zero,
				Rotation = quaternion.identity,
				Scale = 0f
			}, valueRW.SpellEffectEntity);
		}
		PhysicsWorldSingleton singleton = __query_1979089071_8.GetSingleton<PhysicsWorldSingleton>();
		nativeList.Dispose();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1008Job
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CMD = cmd,
			TransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			SpellMaterialLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1008SpellMaterialProperty_RW_ComponentLookup, ref state),
			EffectCollectorLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state)
		}, __TypeHandle.__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Spell1008OnGroundJob
		{
			SpellRefractionLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			SpellRefractHitLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			CurrentRoomEntities = __query_1979089071_9.GetSingleton<CurrentRoomEntitiesSingleton>(),
			EffectDestroyEntity = singletonEntity2,
			PhysicsWorld = singleton,
			ScreenShakeSingleton = singletonEntity3,
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			CMD = cmd
		}, __TypeHandle.__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_2(new Spell1008FallJob
		{
			fallDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1008FallData_RW_ComponentLookup, ref state),
			CMD = cmd
		}, __TypeHandle.__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_3(new Spell1008TakeDamageJob
		{
			CMD = cmd,
			PhysicsWorld = singleton,
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			SpellHitTargetLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Spell1008HitTargetsData_RW_BufferLookup, ref state)
		}, __TypeHandle.__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		nativeArray.Dispose(state.Dependency);
		nativeArray3.Dispose(state.Dependency);
		nativeArray2.Dispose(state.Dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1008Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1008Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Spell1008OnGroundJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1008OnGroundJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_2(Spell1008FallJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1008FallJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_3(Spell1008TakeDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1008TakeDamageJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1008InitializedTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008ArcaneExplosionData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGameObjectEffectLink>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
		__query_1979089071_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1008ArcaneExplosionData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellGroundedTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1008HitExplosionEffectData>();
		__query_1979089071_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1979089071_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1008ArcaneExplosionSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
