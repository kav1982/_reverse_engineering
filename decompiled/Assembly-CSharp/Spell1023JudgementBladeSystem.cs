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

[BurstCompile]
[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
public struct Spell1023JudgementBladeSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1501648610_0
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1023JudgementBladeData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1023JudgementBladeData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item5_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1023JudgementBladeData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1023JudgementBladeData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1501648610_0.TypeHandle __IFE_1501648610_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<Spell1023SpellMaterialProperty> __Spell1023SpellMaterialProperty_RW_ComponentLookup;

		public ComponentLookup<Spell1023ShadowMaterialOverride> __Spell1023ShadowMaterialOverride_RW_ComponentLookup;

		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public Spell1023Job.InternalCompilerQueryAndHandleData __Spell1023Job_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell1023JudgementBladeData> __Spell1023JudgementBladeData_RO_ComponentLookup;

		public ComponentLookup<Spell1023JudgementBladeData> __Spell1023JudgementBladeData_RW_ComponentLookup;

		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1501648610_0_TypeHandle = new IFE_1501648610_0.TypeHandle(ref state);
			__SpellNeedResize_RO_ComponentLookup = state.GetComponentLookup<SpellNeedResize>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Spell1023SpellMaterialProperty_RW_ComponentLookup = state.GetComponentLookup<Spell1023SpellMaterialProperty>();
			__Spell1023ShadowMaterialOverride_RW_ComponentLookup = state.GetComponentLookup<Spell1023ShadowMaterialOverride>();
			__EffectsCollectorData_RW_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Spell1023JudgementBladeData_RO_ComponentLookup = state.GetComponentLookup<Spell1023JudgementBladeData>(isReadOnly: true);
			__Spell1023JudgementBladeData_RW_ComponentLookup = state.GetComponentLookup<Spell1023JudgementBladeData>();
			__SpellMovementComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006BBC_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006BBC_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006BBC_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnUpdate_00006BBD_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006BBD_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006BBD_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnUpdate_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1501648610_0;

	private EntityQuery __query_1501648610_1;

	private EntityQuery __query_1501648610_2;

	private EntityQuery __query_1501648610_3;

	private EntityQuery __query_1501648610_4;

	private EntityQuery __query_1501648610_5;

	private EntityQuery __query_1501648610_6;

	private EntityQuery __query_1501648610_7;

	private EntityQuery __query_1501648610_8;

	private EntityQuery __query_1501648610_9;

	private EntityQuery __query_1501648610_10;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell1023ExtraData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<GlobalRandom>();
		state.EntityManager.CreateSingletonBuffer<Spell1023BladeOwnerData>();
		state.EntityManager.CreateSingleton(new Spell1023AroundDataSingleton
		{
			Data = new NativeHashMap<Entity, NativeList<Entity>>(256, Allocator.Persistent),
			BladeDetectTargetData = new NativeHashMap<Entity, Spell1023OwnerData>(256, Allocator.Persistent)
		});
		state.RequireForUpdate<Spell1023AroundDataSingleton>();
		state.RequireForUpdate<Spell1023JudgementBladeData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		Spell1023AroundDataSingleton singleton = __query_1501648610_1.GetSingleton<Spell1023AroundDataSingleton>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item6 in IFE_1501648610_0.Query(__query_1501648610_0, __TypeHandle.__IFE_1501648610_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1023JudgementBladeData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW5 = item5;
			Entity value = entity;
			if (uncheckedRefRW.ValueRW.IsInitialized)
			{
				continue;
			}
			if (uncheckedRefRW3.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				uncheckedRefRW3.ValueRW.AroundRadius += __query_1501648610_2.GetSingleton<GlobalRandom>().random.NextFloat(-0.4f, 0.4f);
			}
			if (InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref state, value).ExtraSizeRatio > 0f)
			{
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref state, value).Effect2).ValueRW.Scale = 0f;
			}
			if (!(uncheckedRefRW2.ValueRO.Shooter == Entity.Null) && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, uncheckedRefRW2.ValueRO.Shooter) && !uncheckedRefRW2.ValueRO.IsSplitSpell && !uncheckedRefRW3.ValueRO.IsFallSpell && uncheckedRefRW3.ValueRO.Type != SpellSpecialMovementType.Rotation)
			{
				if (state.EntityManager.Exists(uncheckedRefRW2.ValueRO.Shooter) && !singleton.Data.ContainsKey(uncheckedRefRW2.ValueRO.Shooter))
				{
					singleton.Data.Add(uncheckedRefRW2.ValueRW.Shooter, new NativeList<Entity>(Allocator.Persistent));
				}
				Unity.Mathematics.Random random = __query_1501648610_3.GetSingletonRW<GlobalRandom>().ValueRW.NewRandom();
				int length = singleton.Data[uncheckedRefRW2.ValueRO.Shooter].Length;
				NativeList<Entity> list = singleton.Data[uncheckedRefRW2.ValueRO.Shooter];
				if (length <= 4)
				{
					singleton.Data[uncheckedRefRW2.ValueRO.Shooter].Add(in value);
				}
				else
				{
					InsertIntoNativeList(ref list, random.NextInt(0, length), value);
				}
				uncheckedRefRW.ValueRW.IsBladeInQuery = true;
				if (!singleton.BladeDetectTargetData.ContainsKey(uncheckedRefRW2.ValueRO.Shooter))
				{
					singleton.BladeDetectTargetData.Add(uncheckedRefRW2.ValueRW.Shooter, new Spell1023OwnerData
					{
						Timer = 0f,
						Range = uncheckedRefRW4.ValueRO.Float1,
						ClosestTarget = Entity.Null
					});
				}
				float3 shiftedDir = DTool.GetShiftedDir((float)state.WorldUnmanaged.Time.ElapsedTime * -180f + 360f / (float)list.Length * (float)list.IndexOf(value));
				shiftedDir = math.normalizesafe(shiftedDir);
				uncheckedRefRW5.ValueRW.Position = uncheckedRefRW5.ValueRO.Position + shiftedDir * 1.5f * random.NextFloat(0.7f, 1.3f);
				Spell1023OwnerData spell1023OwnerData = singleton.BladeDetectTargetData[uncheckedRefRW2.ValueRO.Shooter];
				spell1023OwnerData.Range = math.max(spell1023OwnerData.Range, uncheckedRefRW4.ValueRO.Float1);
			}
		}
		UpdateAllBladeOwnerRecheckNearestTargetState(ref state, singleton);
		EntityCommandBuffer entityCommandBuffer = __query_1501648610_4.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1023Job
		{
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			CurrentTime = (float)state.WorldUnmanaged.Time.ElapsedTime,
			CMD = entityCommandBuffer.AsParallelWriter(),
			SpellMaterialLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1023SpellMaterialProperty_RW_ComponentLookup, ref state),
			ShadowMaterialLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell1023ShadowMaterialOverride_RW_ComponentLookup, ref state),
			Random = __query_1501648610_3.GetSingletonRW<GlobalRandom>().ValueRW.NewRandom(),
			EffectCollectorLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EffectsCollectorData_RW_ComponentLookup, ref state),
			SpellSingleton = __query_1501648610_5.GetSingleton<SpellSingleton>(),
			EffectRequireEntity = __query_1501648610_6.GetSingletonEntity(),
			SEPlayerSingleton = __query_1501648610_7.GetSingletonEntity(),
			Spell1023OwnerSingleton = __query_1501648610_1.GetSingleton<Spell1023AroundDataSingleton>(),
			MousePosition = __query_1501648610_8.GetSingleton<PlayerController_Dots>().mousePosition,
			ColliderLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state),
			CurrentRoomEntities = __query_1501648610_9.GetSingleton<CurrentRoomEntitiesSingleton>(),
			ExtraData = __query_1501648610_10.GetSingleton<Spell1023ExtraData>()
		}, __TypeHandle.__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.CompleteDependency();
	}

	private void InsertIntoNativeList(ref NativeList<Entity> list, int index, Entity item)
	{
		if (list.Length == list.Capacity)
		{
			list.Capacity++;
		}
		list.Add(in item);
		for (int num = list.Length - 1; num > index; num--)
		{
			list[num] = list[num - 1];
		}
		list[index] = item;
	}

	private void UpdateAllBladeOwnerRecheckNearestTargetState(ref SystemState state, Spell1023AroundDataSingleton aroundDataSingleton)
	{
		foreach (KVPair<Entity, Spell1023OwnerData> bladeDetectTargetDatum in aroundDataSingleton.BladeDetectTargetData)
		{
			bladeDetectTargetDatum.Value.Timer += state.WorldUnmanaged.Time.DeltaTime;
			if (!(bladeDetectTargetDatum.Value.Timer >= 0.1f))
			{
				continue;
			}
			bladeDetectTargetDatum.Value.Timer -= 0.1f;
			__query_1501648610_9.GetSingleton<CurrentRoomEntitiesSingleton>().FindNearestTarget(InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, bladeDetectTargetDatum.Key).Position, UnitType.Player, out var target, out var targetPosition, out var targetPpt);
			if (target != Entity.Null)
			{
				LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, bladeDetectTargetDatum.Key);
				if (DTool.IgnoreZDistance(in componentAfterCompletingDependency.Position, in targetPosition) > bladeDetectTargetDatum.Value.Range + targetPpt.size / 2f)
				{
					target = Entity.Null;
				}
			}
			bladeDetectTargetDatum.Value.ClosestTarget = target;
			foreach (Entity item in aroundDataSingleton.Data[bladeDetectTargetDatum.Key])
			{
				if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell1023JudgementBladeData_RO_ComponentLookup, ref state, item))
				{
					continue;
				}
				RefRW<Spell1023JudgementBladeData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell1023JudgementBladeData_RW_ComponentLookup, ref state, item);
				RefRW<SpellMovementComponentData> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state, item);
				if (componentRWAfterCompletingDependency.ValueRW.State == JudgementBladeState.DetectingTarget)
				{
					componentRWAfterCompletingDependency.ValueRW.Target = bladeDetectTargetDatum.Value.ClosestTarget;
					if (componentRWAfterCompletingDependency.ValueRW.Target != Entity.Null && componentRWAfterCompletingDependency2.ValueRO.Type != SpellSpecialMovementType.Rotation)
					{
						componentRWAfterCompletingDependency.ValueRW.TargetLastFramePosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.Target).Position;
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1023Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1023Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1023JudgementBladeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		__query_1501648610_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1023AroundDataSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_9 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1023ExtraData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1501648610_10 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006BBC_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006BBD_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1023JudgementBladeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1023JudgementBladeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1023JudgementBladeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
