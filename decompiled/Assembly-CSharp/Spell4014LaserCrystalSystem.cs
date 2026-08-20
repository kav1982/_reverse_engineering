using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
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
[BurstCompile]
internal struct Spell4014LaserCrystalSystem : ISystem, ISystemCompilerGenerated
{
	private struct CrystalCenterData
	{
		public float3 center;

		public float3 direction;

		public NativeList<Entity> turrents;

		public bool isUpdated;

		public bool haveTarget;

		public float3 chaseTargetPosition;

		public float3 vDir;

		public float v;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CrystalPassDoorPosFix : IBufferElementData
	{
	}

	private enum CrystalCenterType
	{
		Normal,
		FallNormal,
		Rotate,
		FallRotate,
		FallChaseEnemy,
		FallChaseMouse
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1276459262_0
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<LocalTransform> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell4014LaserCrystalData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
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
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1276459262_0.TypeHandle __IFE_1276459262_0_TypeHandle;

		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentLookup;

		public BufferLookup<CrystalPassDoorPosFix> __Spell4014LaserCrystalSystem_CrystalPassDoorPosFix_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public BufferLookup<SpellRefractionHitEntities> __SpellRefractionHitEntities_RW_BufferLookup;

		public ComponentLookup<SpellRefractionData> __SpellRefractionData_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell4014LaserCrystalJob.InternalCompilerQueryAndHandleData __Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1276459262_0_TypeHandle = new IFE_1276459262_0.TypeHandle(ref state);
			__SpellSplitComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>();
			__Spell4014LaserCrystalSystem_CrystalPassDoorPosFix_RW_BufferLookup = state.GetBufferLookup<CrystalPassDoorPosFix>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellRefractionHitEntities_RW_BufferLookup = state.GetBufferLookup<SpellRefractionHitEntities>();
			__SpellRefractionData_RW_ComponentLookup = state.GetComponentLookup<SpellRefractionData>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellSplitComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00007807_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00007807_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00007807_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnDestroy_00007809_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00007809_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00007809_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private NativeHashMap<int, NativeHashMap<Entity, CrystalCenterData>> OrbitCenterDataMap;

	private float totalRotation;

	private CurrentRoomEntitiesSingleton RoomEtt;

	private Entity PassDoorBufferEntity;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1276459262_0;

	private EntityQuery __query_1276459262_1;

	private EntityQuery __query_1276459262_2;

	private EntityQuery __query_1276459262_3;

	private EntityQuery __query_1276459262_4;

	private EntityQuery __query_1276459262_5;

	private EntityQuery __query_1276459262_6;

	private EntityQuery __query_1276459262_7;

	private EntityQuery __query_1276459262_8;

	private EntityQuery __query_1276459262_9;

	private EntityQuery __query_1276459262_10;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<CrystalPassDoorPosFix>();
		state.RequireForUpdate<Spell3101NewThunderHitData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<SpellSplitComponentData>();
		state.RequireForUpdate<Spell4014LaserCrystalData>();
		OrbitCenterDataMap = new NativeHashMap<int, NativeHashMap<Entity, CrystalCenterData>>(5, Allocator.Persistent);
		for (int i = 0; i < 6; i++)
		{
			OrbitCenterDataMap.Add(i, new NativeHashMap<Entity, CrystalCenterData>(7, Allocator.Persistent));
		}
		state.EntityManager.CreateSingletonBuffer<CrystalPassDoorPosFix>();
		PassDoorBufferEntity = __query_1276459262_1.GetSingletonEntity();
	}

	public void OnUpdate(ref SystemState state)
	{
		totalRotation += Time.deltaTime * 180f;
		if (totalRotation >= 360f)
		{
			totalRotation -= 360f;
		}
		OrbitMapCheckNull();
		EntityCommandBuffer cmd = __query_1276459262_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		EntityCommandBuffer entityCommandBuffer = __query_1276459262_3.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		RoomEtt = __query_1276459262_4.GetSingleton<CurrentRoomEntitiesSingleton>();
		Entity singletonEntity = __query_1276459262_5.GetSingletonEntity();
		PlayerController_Dots playerController = __query_1276459262_6.GetSingleton<PlayerController_Dots>();
		ComponentLookup<SpellSplitComponentData> componentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellSplitComponentData_RW_ComponentLookup, ref state);
		SpellSingleton spellSingleton = __query_1276459262_7.GetSingleton<SpellSingleton>();
		GlobalRandom random = __query_1276459262_8.GetSingleton<GlobalRandom>();
		DynamicBuffer<SpellSpawnParams> singletonBuffer = __query_1276459262_9.GetSingletonBuffer<SpellSpawnParams>();
		DynamicBuffer<CrystalPassDoorPosFix> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Spell4014LaserCrystalSystem_CrystalPassDoorPosFix_RW_BufferLookup, ref state, PassDoorBufferEntity);
		bool isPassDoorFrame = bufferAfterCompletingDependency.Length != 0;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> item6 in IFE_1276459262_0.Query(__query_1276459262_0, __TypeHandle.__IFE_1276459262_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW4 = item5;
			Entity entity2 = entity;
			if (!OrbitCenterDataMap.IsCreated)
			{
				break;
			}
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW3.ValueRO.Shooter))
			{
				entityCommandBuffer.DestroyEntity(entity2);
				continue;
			}
			CrystalCenterType orbitType = GetCrystalOrbitType(in uncheckedRefRW4.ValueRO);
			if (uncheckedRefRW3.ValueRO.IsSplitSpell)
			{
				if (!uncheckedRefRW2.ValueRO.IsCreatedCrystal)
				{
					uncheckedRefRW2.ValueRW.IsCreatedCrystal = true;
					uncheckedRefRO.ValueRO.ColorType.ColorEnumToString(out var result);
					GenerateCrystalAndLineEffectRequire(cmd, singletonEntity, entity2, result);
					if (uncheckedRefRW4.ValueRO.Type == SpellSpecialMovementType.Rotation)
					{
						uncheckedRefRW2.ValueRW.RotateDegreeSpeed = 360f / (MathF.PI * 2f * uncheckedRefRW4.ValueRO.AroundRadius / 2f);
					}
				}
				ref Spell4014LaserCrystalData valueRW = ref uncheckedRefRW2.ValueRW;
				ref SpellComponentData valueRW2 = ref uncheckedRefRW3.ValueRW;
				ref readonly SpellConfigComponentData valueRO = ref uncheckedRefRO.ValueRO;
				DynamicBuffer<SpellGameObjectEffectLink> effectLink = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2);
				EffectLinkGetAndInitEffect(ref valueRW, ref valueRW2, in valueRO, in effectLink, 0.6f, in orbitType);
				SplitCrystalPositionUpdate(ref state, ref uncheckedRefRW.ValueRW, in uncheckedRefRW3.ValueRO, ref uncheckedRefRW4.ValueRW);
			}
			else
			{
				if (!uncheckedRefRW2.ValueRO.IsCreatedCrystal)
				{
					uncheckedRefRW2.ValueRW.IsCreatedCrystal = true;
					InitCrystalData(ref state, in orbitType, ref uncheckedRefRW2.ValueRW, in uncheckedRefRO.ValueRO, in uncheckedRefRW3.ValueRO, in uncheckedRefRW4.ValueRO, entity2, in playerController);
					componentLookup.TryGetComponent(entity2, out var componentData);
					uncheckedRefRW2.ValueRW.IsSplitCenter = componentData.Count > 0;
					if (uncheckedRefRW2.ValueRO.IsSplitCenter)
					{
						SplitCrystal(ref state, entity2, in spellSingleton, ref random, singletonBuffer);
					}
					else
					{
						uncheckedRefRO.ValueRO.ColorType.ColorEnumToString(out var result2);
						GenerateCrystalAndLineEffectRequire(cmd, singletonEntity, entity2, result2);
					}
				}
				if (!uncheckedRefRW2.ValueRW.IsSplitCenter)
				{
					ref Spell4014LaserCrystalData valueRW3 = ref uncheckedRefRW2.ValueRW;
					ref SpellComponentData valueRW4 = ref uncheckedRefRW3.ValueRW;
					ref readonly SpellConfigComponentData valueRO2 = ref uncheckedRefRO.ValueRO;
					DynamicBuffer<SpellGameObjectEffectLink> effectLink = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2);
					EffectLinkGetAndInitEffect(ref valueRW3, ref valueRW4, in valueRO2, in effectLink, 1f, in orbitType);
				}
				UpdateWandCenterMap(ref state, in uncheckedRefRW3.ValueRO, playerController, ref uncheckedRefRW2.ValueRW, RoomEtt, ref uncheckedRefRW4.ValueRW, in uncheckedRefRO.ValueRO, in uncheckedRefRW.ValueRO, in orbitType, isPassDoorFrame);
				GetTurrentRelativePosition(entity2, uncheckedRefRW3.ValueRO.OwnerEntity, ref uncheckedRefRW4.ValueRW, ref uncheckedRefRW2.ValueRW, out var position, in orbitType);
				TurretPositionRefresh(in position, ref uncheckedRefRW.ValueRW, in uncheckedRefRW3.ValueRO, in uncheckedRefRW2.ValueRO, orbitType);
			}
			if (OrbitCenterDataMap[(int)orbitType].ContainsKey(uncheckedRefRW3.ValueRO.OwnerEntity))
			{
				UpdateCrystalData(ref uncheckedRefRW2.ValueRW, in uncheckedRefRW3.ValueRO, in orbitType);
			}
		}
		bufferAfterCompletingDependency.Clear();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell4014LaserCrystalJob
		{
			PhysicsWorld = __query_1276459262_10.GetSingleton<PhysicsWorldSingleton>(),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellRefractionHitEntitiesLookup = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__SpellRefractionHitEntities_RW_BufferLookup, ref state),
			SpellRefractionLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellRefractionData_RW_ComponentLookup, ref state),
			SpellSplitLookup = componentLookup,
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			PlayerCtrller = playerController,
			DeltaTime = Time.deltaTime,
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			CurrentRoomEntities = RoomEtt,
			IsIgnoreWallRelic = PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall,
			Random = __query_1276459262_8.GetSingleton<GlobalRandom>(),
			IsPause = (Time.timeScale == 0f)
		}, __TypeHandle.__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		OrbitCenterDataMapReset();
	}

	private void InitCrystalData(ref SystemState state, in CrystalCenterType orbitType, ref Spell4014LaserCrystalData crystalData, in SpellConfigComponentData config, in SpellComponentData componentData, in SpellMovementComponentData movement, Entity spellEntity, in PlayerController_Dots playerController)
	{
		GetOrbitRadius(in config, ref crystalData);
		SetCrystalInitData(ref crystalData, in movement);
		AddCrystalToOrbitMap(ref state, in orbitType, ref crystalData, spellEntity, in componentData, in movement, in config, in playerController);
	}

	private void GetOrbitRadius(in SpellConfigComponentData config, ref Spell4014LaserCrystalData crystalData)
	{
		RadiusAttributeValue radius = config.Radius;
		radius.Base = 1.2f;
		crystalData.OrbitLongRadius = radius.CalculateIgnoreFall();
	}

	private void SetCrystalInitData(ref Spell4014LaserCrystalData crystalData, in SpellMovementComponentData movement)
	{
		switch (movement.Type)
		{
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.ChaseMouse:
			if (movement.IsFallSpell)
			{
				crystalData.CrystalCenterMoveSpeed = movement.Speed + 4f;
			}
			break;
		case SpellSpecialMovementType.Rotation:
			crystalData.CrystalCenterMoveSpeed = movement.Speed + 1f;
			crystalData.RotateDegreeSpeed = 360f / (MathF.PI * 2f * movement.AroundRadius / crystalData.CrystalCenterMoveSpeed);
			break;
		}
	}

	private void AddCrystalToOrbitMap(ref SystemState state, in CrystalCenterType orbitType, ref Spell4014LaserCrystalData crystalData, Entity spellEntity, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellConfigComponentData config, in PlayerController_Dots playerController)
	{
		ClearTurretEntityRecord(spellEntity);
		if (!OrbitCenterDataMap[(int)orbitType].ContainsKey(componentData.OwnerEntity))
		{
			CrystalCenterData crystalCenterData = default(CrystalCenterData);
			crystalCenterData.center = float3.zero;
			crystalCenterData.chaseTargetPosition = float3.zero;
			crystalCenterData.direction = new float3(1f, 0f, 0f);
			crystalCenterData.turrents = new NativeList<Entity>(Allocator.Persistent);
			crystalCenterData.isUpdated = false;
			CrystalCenterData item = crystalCenterData;
			switch (movement.Type)
			{
			case SpellSpecialMovementType.ChaseMouse:
				if (movement.IsFallSpell && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
				{
					LocalTransform componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity);
					item.center = componentAfterCompletingDependency2.Position;
					item.chaseTargetPosition = playerController.mousePosition;
					item.v = crystalData.CrystalCenterMoveSpeed;
					item.vDir = math.normalizesafe(playerController.mousePosition - componentAfterCompletingDependency2.Position);
				}
				break;
			case SpellSpecialMovementType.ChaseEnemy:
				if (movement.IsFallSpell && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
				{
					LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity);
					item.center = componentAfterCompletingDependency.Position + new float3(0f, 0.1f, 0f);
					item.chaseTargetPosition = componentAfterCompletingDependency.Position;
					item.vDir = new float3(1f, 0f, 0f);
					item.v = crystalData.CrystalCenterMoveSpeed;
					if (RoomEtt.FindNearestTarget(componentAfterCompletingDependency.Position, config.ShooterType, out var _, out var targetPosition, out var _))
					{
						item.vDir = math.normalizesafe(targetPosition - componentAfterCompletingDependency.Position);
						item.chaseTargetPosition = targetPosition;
						item.haveTarget = true;
					}
				}
				break;
			}
			item.turrents.Add(in spellEntity);
			OrbitCenterDataMap[(int)orbitType].Add(componentData.OwnerEntity, item);
		}
		else
		{
			CrystalCenterData data = OrbitCenterDataMap[(int)orbitType][componentData.OwnerEntity];
			data.turrents.Add(in spellEntity);
			SetOrbitMapData(orbitType, componentData.OwnerEntity, data);
		}
	}

	private void ClearTurretEntityRecord(Entity turretEntity)
	{
		NativeArray<int> keyArray = OrbitCenterDataMap.GetKeyArray(Allocator.Temp);
		for (int i = 0; i < keyArray.Length; i++)
		{
			int key = keyArray[i];
			NativeHashMap<Entity, CrystalCenterData> nativeHashMap = OrbitCenterDataMap[key];
			NativeArray<Entity> keyArray2 = nativeHashMap.GetKeyArray(Allocator.Temp);
			for (int j = 0; j < keyArray2.Length; j++)
			{
				Entity key2 = keyArray2[j];
				CrystalCenterData value = nativeHashMap[key2];
				for (int num = value.turrents.Length - 1; num >= 0; num--)
				{
					if (value.turrents[num] == turretEntity)
					{
						value.turrents.RemoveAtSwapBack(num);
					}
				}
				nativeHashMap[key2] = value;
			}
			keyArray2.Dispose();
		}
		keyArray.Dispose();
	}

	private CrystalCenterType GetCrystalOrbitType(in SpellMovementComponentData movement)
	{
		if (movement.IsFallSpell)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Rotation:
				return CrystalCenterType.FallRotate;
			case SpellSpecialMovementType.ChaseEnemy:
				return CrystalCenterType.FallChaseEnemy;
			case SpellSpecialMovementType.ChaseMouse:
				return CrystalCenterType.FallChaseMouse;
			case SpellSpecialMovementType.Normal:
			case SpellSpecialMovementType.ChaseOwner:
				return CrystalCenterType.FallNormal;
			default:
				return CrystalCenterType.Normal;
			}
		}
		if (movement.Type == SpellSpecialMovementType.Rotation)
		{
			return CrystalCenterType.Rotate;
		}
		return CrystalCenterType.Normal;
	}

	private void SetOrbitMapData(CrystalCenterType orbitType, Entity owner, CrystalCenterData data)
	{
		NativeHashMap<Entity, CrystalCenterData> value = OrbitCenterDataMap[(int)orbitType];
		value[owner] = data;
		OrbitCenterDataMap[(int)orbitType] = value;
	}

	private void GenerateCrystalAndLineEffectRequire(EntityCommandBuffer cmd, Entity crystalEffectRequire, Entity spellEntity, FixedString32Bytes colorName)
	{
		GenCrystalPart("Crystal");
		GenCrystalPartColor("Trail", colorName);
		void GenCrystalPart(string name)
		{
			cmd.AppendToBuffer(crystalEffectRequire, new SpellEffectSystem.Require
			{
				Entity = spellEntity,
				SpellId = 4014,
				Settings = 
				{
					Name = name,
					Layer = LayerCorrectType.Coordinate,
					IgnoreColor = true,
					ClearParticle = true,
					ClearTrail = true,
					ScaleMode = SpellEffectSystem.ScaleMode.Ignore
				}
			});
		}
		void GenCrystalPartColor(string name, FixedString32Bytes _colorName)
		{
			cmd.AppendToBuffer(crystalEffectRequire, new SpellEffectSystem.Require
			{
				Entity = spellEntity,
				SpellId = 4014,
				Settings = 
				{
					Name = name,
					Layer = LayerCorrectType.Coordinate,
					IgnoreColor = false,
					ClearParticle = true,
					ClearTrail = true,
					ScaleMode = SpellEffectSystem.ScaleMode.Ignore
				},
				Color = _colorName
			});
		}
	}

	private void EffectLinkGetAndInitEffect(ref Spell4014LaserCrystalData crystalData, ref SpellComponentData componentData, in SpellConfigComponentData config, in DynamicBuffer<SpellGameObjectEffectLink> effectLink, float rBase, in CrystalCenterType orbitType)
	{
		int num = CrystalFaceMultiplier(componentData.OwnerEntity, in orbitType);
		if (TryGetEffGO(ref crystalData.LaserCrystalGO, "Crystal", in config, in effectLink))
		{
			crystalData.LaserCrystalGO.Value.transform.localRotation = Quaternion.Euler(new Vector3(0f, num, 0f));
		}
		TryGetEffGO(ref componentData.TrailEffectGameObject, "Trail", in config, in effectLink);
		static float GetR(float baseR, in RadiusAttributeValue rAttValue)
		{
			RadiusAttributeValue radiusAttributeValue = rAttValue;
			radiusAttributeValue.Base = baseR;
			return radiusAttributeValue.CalculateIgnoreFall();
		}
		bool TryGetEffGO(ref UnityObjectRef<GameObject> goRef, string name, in SpellConfigComponentData config, in DynamicBuffer<SpellGameObjectEffectLink> effectLink)
		{
			if (!goRef.Value)
			{
				if (TryGetLinkEffect(name, effectLink, out var linkedObject2))
				{
					linkedObject2.Value.transform.localScale = Vector3.one * GetR(rBase, in config.Radius);
					goRef = linkedObject2;
					return true;
				}
				return false;
			}
			return true;
		}
		static bool TryGetLinkEffect(FixedString32Bytes name, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out UnityObjectRef<GameObject> linkedObject)
		{
			foreach (SpellGameObjectEffectLink item in linkBuffer)
			{
				SpellGameObjectEffectLink current = item;
				if (current.EffectName == name)
				{
					linkedObject = current.GameObject;
					return true;
				}
			}
			linkedObject = null;
			return false;
		}
	}

	private int CrystalFaceMultiplier(Entity ownerEntity, in CrystalCenterType orbitType)
	{
		if (OrbitCenterDataMap[(int)orbitType].ContainsKey(ownerEntity))
		{
			if (!(OrbitCenterDataMap[(int)orbitType][ownerEntity].direction.x >= 0f))
			{
				return 180;
			}
			return 0;
		}
		return 0;
	}

	private void UpdateWandCenterMap(ref SystemState state, in SpellComponentData componentData, PlayerController_Dots playerController, ref Spell4014LaserCrystalData crystalData, CurrentRoomEntitiesSingleton roomEtt, ref SpellMovementComponentData movement, in SpellConfigComponentData config, in LocalTransform transform, in CrystalCenterType orbitType, bool isPassDoorFrame)
	{
		if (OrbitCenterDataMap[(int)orbitType].ContainsKey(componentData.OwnerEntity) && !OrbitCenterDataMap[(int)orbitType][componentData.OwnerEntity].isUpdated)
		{
			CrystalCenterData orbitData = OrbitCenterDataMap[(int)orbitType][componentData.OwnerEntity];
			WandCenterPositionUpdate(ref state, ref movement, ref crystalData, playerController, roomEtt, in componentData, in config, in transform, in orbitType, ref orbitData, isPassDoorFrame);
			orbitData.direction = new float3(math.normalizesafe(playerController.mousePosition - orbitData.center).xy, 0f);
			orbitData.isUpdated = true;
			SetOrbitMapData(orbitType, componentData.OwnerEntity, orbitData);
		}
	}

	private void WandCenterPositionUpdate(ref SystemState state, ref SpellMovementComponentData movement, ref Spell4014LaserCrystalData crystalData, PlayerController_Dots playerController, CurrentRoomEntitiesSingleton roomEtt, in SpellComponentData componentData, in SpellConfigComponentData config, in LocalTransform transform, in CrystalCenterType orbitType, ref CrystalCenterData orbitData, bool passDoorFrame)
	{
		if (!OrbitCenterDataMap[(int)orbitType].ContainsKey(componentData.OwnerEntity))
		{
			return;
		}
		if (movement.IsFallSpell)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.ChaseEnemy:
			{
				if (passDoorFrame)
				{
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
					{
						orbitData.center = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity).Position;
					}
					break;
				}
				orbitData.haveTarget = roomEtt.FindNearestTarget(orbitData.center, config.ShooterType, out var _, out var targetPosition, out var _);
				if (!orbitData.haveTarget)
				{
					orbitData.haveTarget = false;
					if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
					{
						orbitData.chaseTargetPosition = playerController.mousePosition;
						break;
					}
					targetPosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity).Position;
				}
				orbitData.chaseTargetPosition = targetPosition;
				float3 target2 = math.normalizesafe(targetPosition - orbitData.center);
				float maxDelta = movement.ChaseRotateSpeed * crystalData.CrystalCenterMoveSpeed * 3f * Time.deltaTime;
				orbitData.vDir = DTool.DirMoveTowardsIgnoreZ(in orbitData.vDir, in target2, maxDelta);
				orbitData.center += orbitData.vDir * orbitData.v * Time.deltaTime;
				break;
			}
			case SpellSpecialMovementType.ChaseMouse:
			{
				if (passDoorFrame)
				{
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
					{
						orbitData.center = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity).Position;
					}
					break;
				}
				float3 end = math.normalizesafe(playerController.mousePosition - orbitData.center) * crystalData.CrystalCenterMoveSpeed;
				float3 start = orbitData.v * orbitData.vDir;
				start = DTool.Lerp(in start, in end, movement.ChaseMouseLerpSpeed * crystalData.CrystalCenterMoveSpeed * Time.deltaTime);
				orbitData.center += start * Time.deltaTime;
				orbitData.v = math.length(start);
				orbitData.vDir = math.normalizesafe(start);
				break;
			}
			default:
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
				{
					orbitData.center = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity).Position + new float3(0f, 0f, -0.4f);
				}
				break;
			}
		}
		else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity))
		{
			orbitData.center = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentData.OwnerEntity).Position + new float3(0f, 0f, -0.4f);
		}
	}

	private void TurretPositionRefresh(in float3 relativePosition, ref LocalTransform turretTrans, in SpellComponentData componentData, in Spell4014LaserCrystalData crystalData, CrystalCenterType orbitType)
	{
		if (OrbitCenterDataMap[(int)orbitType].ContainsKey(componentData.OwnerEntity))
		{
			turretTrans.Position = OrbitCenterDataMap[(int)orbitType][componentData.OwnerEntity].center + relativePosition;
		}
	}

	private void OrbitPositionGet(float orbitLongRadius, float startAngle, out float3 position, float yRotate = 0f, float yScale = 0.4f)
	{
		position = Quaternion.Euler(0f, 0f, startAngle + totalRotation) * new float3(1f, 0f, 0f);
		position *= orbitLongRadius;
		position.y *= yScale;
		position = Quaternion.Euler(0f, yRotate, 0f) * position;
	}

	private void GetTurrentRelativePosition(Entity crystalEntity, Entity ownerEntity, ref SpellMovementComponentData movement, ref Spell4014LaserCrystalData crystalData, out float3 position, in CrystalCenterType orbitType)
	{
		position = default(float3);
		if (OrbitCenterDataMap[(int)orbitType].ContainsKey(ownerEntity))
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
			case SpellSpecialMovementType.ChaseEnemy:
			case SpellSpecialMovementType.ChaseMouse:
			case SpellSpecialMovementType.ChaseOwner:
				NormalOrbitRelativePositionUpdate(crystalEntity, ownerEntity, in orbitType, in crystalData, in movement, out position);
				break;
			case SpellSpecialMovementType.Rotation:
			{
				int indexInNativeList = GetIndexInNativeList(OrbitCenterDataMap[(int)orbitType][ownerEntity].turrents, crystalEntity);
				int length = OrbitCenterDataMap[(int)orbitType][ownerEntity].turrents.Length;
				float num = (float)indexInNativeList * (360f / (float)length);
				crystalData.CurrentRotateDegree += crystalData.RotateDegreeSpeed * Time.deltaTime;
				crystalData.CurrentRotateDegree %= 360f;
				movement.AroundAngle = num + crystalData.CurrentRotateDegree;
				position = DTool.RotateDir(new float3(1f, 0f, 0f), movement.AroundAngle) * movement.AroundRadius;
				break;
			}
			}
			if (movement.IsFallSpell)
			{
				position += new float3(0f, 0f, -3f);
			}
		}
	}

	private void NormalOrbitRelativePositionUpdate(Entity crystalEntity, Entity ownerEntity, in CrystalCenterType orbitType, in Spell4014LaserCrystalData crystalData, in SpellMovementComponentData movement, out float3 position)
	{
		int indexInNativeList = GetIndexInNativeList(OrbitCenterDataMap[(int)orbitType][ownerEntity].turrents, crystalEntity);
		int length = OrbitCenterDataMap[(int)orbitType][ownerEntity].turrents.Length;
		int num = indexInNativeList % 3;
		int num2 = ((length % 3 > num) ? 1 : 0);
		int num3 = length / 3 + num2;
		float startAngle = (float)(indexInNativeList / 3) * (360f / (float)num3);
		GetOrbitPointByOrbitIndex(num, crystalData.OrbitLongRadius, startAngle, out position);
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		position = new float3(position.xy + layerPosition.xy, 0f);
	}

	private void GetOrbitPointByOrbitIndex(int orbitIndex, float orbitLongRadius, float startAngle, out float3 position)
	{
		position = default(float3);
		switch (orbitIndex)
		{
		case 0:
			OrbitPositionGet(orbitLongRadius, startAngle, out position);
			break;
		case 1:
			OrbitPositionGet(orbitLongRadius, startAngle + 180f, out position, 45f);
			break;
		case 2:
			OrbitPositionGet(orbitLongRadius, startAngle + 180f, out position, -45f);
			break;
		}
	}

	private int GetIndexInNativeList(NativeList<Entity> list, Entity entityToFind)
	{
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i] == entityToFind)
			{
				return i;
			}
		}
		return -1;
	}

	private void OrbitCenterDataMapReset()
	{
		if (!OrbitCenterDataMap.IsCreated)
		{
			return;
		}
		foreach (KVPair<int, NativeHashMap<Entity, CrystalCenterData>> item in OrbitCenterDataMap)
		{
			foreach (KVPair<Entity, CrystalCenterData> item2 in item.Value)
			{
				item2.Value.isUpdated = false;
			}
		}
	}

	private void OrbitMapCheckNull()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		foreach (KVPair<int, NativeHashMap<Entity, CrystalCenterData>> item in OrbitCenterDataMap)
		{
			NativeArray<Entity> keyArray = item.Value.GetKeyArray(Allocator.Temp);
			for (int i = 0; i < keyArray.Length; i++)
			{
				Entity entity = keyArray[i];
				CrystalCenterData value = item.Value[entity];
				if (!entityManager.Exists(entity))
				{
					value.turrents.Dispose();
					item.Value.Remove(entity);
					continue;
				}
				for (int num = value.turrents.Length - 1; num >= 0; num--)
				{
					if (!entityManager.Exists(value.turrents[num]))
					{
						value.turrents.RemoveAtSwapBack(num);
					}
				}
				item.Value[entity] = value;
				if (value.turrents.IsEmpty)
				{
					value.turrents.Dispose();
					item.Value.Remove(entity);
				}
			}
			keyArray.Dispose();
		}
	}

	private void SplitCrystalPositionUpdate(ref SystemState state, ref LocalTransform transform, in SpellComponentData comp, ref SpellMovementComponentData movement)
	{
		LocalTransform localTransform = (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, comp.Shooter) ? InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, comp.Shooter) : default(LocalTransform));
		movement.AroundAngle %= 360f;
		float3 dir = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f));
		transform.Position = localTransform.Position + movement.AroundRadius * dir;
		movement.AroundAngle += 180f * Time.deltaTime;
	}

	private void UpdateCrystalData(ref Spell4014LaserCrystalData crystalData, in SpellComponentData comp, in CrystalCenterType orbitType)
	{
		int key = (int)orbitType;
		crystalData.CurrentWandMana = GetWandMana(in comp);
		crystalData.CurrentWandMaxMana = GetWandMaxMana(in comp);
		crystalData.OrbitCenter = OrbitCenterDataMap[key][comp.OwnerEntity].center;
		crystalData.HaveTarget = OrbitCenterDataMap[key][comp.OwnerEntity].haveTarget;
		crystalData.TargetPosition = OrbitCenterDataMap[key][comp.OwnerEntity].chaseTargetPosition;
		crystalData.OwnerDirection = OrbitCenterDataMap[key][comp.OwnerEntity].direction;
	}

	private float GetWandMana(in SpellComponentData componentData)
	{
		if ((bool)componentData.Wand)
		{
			return componentData.Wand.Value.CurrentMP;
		}
		return 0f;
	}

	private float GetWandMaxMana(in SpellComponentData componentData)
	{
		if ((bool)componentData.Wand)
		{
			return componentData.Wand.Value.MaxMP;
		}
		return 0f;
	}

	private void SplitCrystal(ref SystemState state, Entity entity, in SpellSingleton spellSingleton, ref GlobalRandom random, DynamicBuffer<SpellSpawnParams> shootBuffer)
	{
		float3 position = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity).ValueRO.Position;
		position.z = -0.01f;
		SpellSplitComponentData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RO_ComponentLookup, ref state, entity);
		SpellSpawnParams elem = spellSingleton.SpellSpawnParamsStorage[entity].ToSplit(entity, componentAfterCompletingDependency);
		elem.MovementComponentData.AroundRadius = math.clamp(elem.MovementComponentData.AroundRadius, 0.5f, float.PositiveInfinity);
		float num = random.random.NextFloat(360f);
		for (int i = 0; i < componentAfterCompletingDependency.Count; i++)
		{
			float num2 = num + 360f * ((float)i / (float)componentAfterCompletingDependency.Count);
			elem.MovementComponentData.Direction = Tool2D.GetDir(num2 + 90f);
			elem.MovementComponentData.AroundAngle = num2;
			float3 @float = 0.5f * Tool2D.GetDir(num2);
			elem.SpawnPosition = position + @float;
			shootBuffer.Add(elem);
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
		if (!OrbitCenterDataMap.IsCreated)
		{
			return;
		}
		foreach (KVPair<int, NativeHashMap<Entity, CrystalCenterData>> item in OrbitCenterDataMap)
		{
			foreach (KVPair<Entity, CrystalCenterData> item2 in item.Value)
			{
				item2.Value.turrents.Dispose();
			}
			item.Value.Dispose();
		}
		OrbitCenterDataMap.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell4014LaserCrystalJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell4014LaserCrystalJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		__query_1276459262_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CrystalPassDoorPosFix>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1276459262_10 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00007807_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4014LaserCrystalSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00007809_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4014LaserCrystalSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4014LaserCrystalSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell4014LaserCrystalSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
