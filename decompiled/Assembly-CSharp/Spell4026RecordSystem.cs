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

[UpdateBefore(typeof(Spell4026System))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
internal struct Spell4026RecordSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1538284518_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4026GreenRuneData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4026GreenRuneData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4026GreenRuneData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4026GreenRuneData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1538284518_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4026GreenRuneData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4026GreenRuneData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4026GreenRuneData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4026GreenRuneData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1538284518_0.TypeHandle __IFE_1538284518_0_TypeHandle;

		public IFE_1538284518_1.TypeHandle __IFE_1538284518_1_TypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellDestroyTag> __SpellDestroyTag_RO_ComponentLookup;

		public ComponentLookup<SpellDestroyTag> __SpellDestroyTag_RW_ComponentLookup;

		public ComponentLookup<EnterDoorDestroy> __EnterDoorDestroy_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1538284518_0_TypeHandle = new IFE_1538284518_0.TypeHandle(ref state);
			__IFE_1538284518_1_TypeHandle = new IFE_1538284518_1.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellDestroyTag_RO_ComponentLookup = state.GetComponentLookup<SpellDestroyTag>(isReadOnly: true);
			__SpellDestroyTag_RW_ComponentLookup = state.GetComponentLookup<SpellDestroyTag>();
			__EnterDoorDestroy_RW_ComponentLookup = state.GetComponentLookup<EnterDoorDestroy>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
		}
	}

	private NativeList<Entity> RuneBallList;

	private NativeList<Entity> RotateMovementRuneBallList;

	private float ringAngle;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1538284518_0;

	private EntityQuery __query_1538284518_1;

	private EntityQuery __query_1538284518_2;

	private EntityQuery __query_1538284518_3;

	private EntityQuery __query_1538284518_4;

	private EntityQuery __query_1538284518_5;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<DynamicOptimizeData>();
		RuneBallList = new NativeList<Entity>(Allocator.Persistent);
		RotateMovementRuneBallList = new NativeList<Entity>(Allocator.Persistent);
		ringAngle = 0f;
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
	}

	public void OnDestroy(ref SystemState state)
	{
		state.Dependency.Complete();
		if (RuneBallList.IsCreated)
		{
			RuneBallList.Dispose();
		}
		if (RotateMovementRuneBallList.IsCreated)
		{
			RotateMovementRuneBallList.Dispose();
		}
	}

	public void OnUpdate(ref SystemState state)
	{
		for (int num = RuneBallList.Length - 1; num >= 0; num--)
		{
			Entity entity = RuneBallList[num];
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity) || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellDestroyTag_RO_ComponentLookup, ref state, entity))
			{
				RuneBallList.RemoveAt(num);
			}
		}
		for (int num2 = RotateMovementRuneBallList.Length - 1; num2 >= 0; num2--)
		{
			Entity entity2 = RotateMovementRuneBallList[num2];
			if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, entity2) || InternalCompilerInterface.IsComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellDestroyTag_RO_ComponentLookup, ref state, entity2))
			{
				RotateMovementRuneBallList.RemoveAt(num2);
			}
		}
		InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData> item;
		InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> item2;
		InternalCompilerInterface.UncheckedRefRW<SpellComponentData> item3;
		InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> item4;
		Entity entity3;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> item6 in IFE_1538284518_0.Query(__query_1538284518_0, __TypeHandle.__IFE_1538284518_0_TypeHandle, ref state))
		{
			item6.Deconstruct(out item, out item2, out item3, out item4, out entity3);
			InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW4 = item4;
			Entity value = entity3;
			if (uncheckedRefRW2.ValueRW.IsFallSpell)
			{
				continue;
			}
			if (uncheckedRefRW.ValueRO.IsInitialized)
			{
				if (uncheckedRefRW3.ValueRO.Wand.Value == null || uncheckedRefRW3.ValueRO.Wand.Value.passiveGreenRuneCount <= 0)
				{
					InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellDestroyTag_RW_ComponentLookup, ref state, value, value: true);
				}
			}
			else if (uncheckedRefRW4.ValueRW.Int3 <= 0)
			{
				uncheckedRefRW.ValueRW.IsRuneBall = true;
				if (uncheckedRefRW2.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					RotateMovementRuneBallList.Add(in value);
				}
				else
				{
					RuneBallList.Add(in value);
				}
				Wand value2 = uncheckedRefRW3.ValueRW.Wand.Value;
				if (value2.WandCfg != null)
				{
					value2.GreenRuneList.Add(value);
				}
			}
		}
		DynamicOptimizeData singleton = __query_1538284518_2.GetSingleton<DynamicOptimizeData>();
		float num3 = state.WorldUnmanaged.Time.DeltaTime * singleton.LastFrameTimeScale;
		ringAngle += 120f * num3;
		int num4 = 0;
		int num5 = 0;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item7 in IFE_1538284518_1.Query(__query_1538284518_1, __TypeHandle.__IFE_1538284518_1_TypeHandle, ref state))
		{
			item7.Deconstruct(out item, out item2, out item4, out item3, out entity3);
			InternalCompilerInterface.UncheckedRefRW<Spell4026GreenRuneData> uncheckedRefRW5 = item;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW6 = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW7 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW8 = item3;
			Entity entity4 = entity3;
			if (uncheckedRefRW5.ValueRO.IsRuneBall && !uncheckedRefRW6.ValueRO.IsFallSpell)
			{
				if (uncheckedRefRW6.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					uncheckedRefRW5.ValueRW.TargetAngle = ringAngle + 360f / (float)math.max(1, RotateMovementRuneBallList.Length) * (float)(RotateMovementRuneBallList.Length - num5);
					num5++;
				}
				else
				{
					uncheckedRefRW5.ValueRW.TargetAngle = ringAngle - 360f / (float)math.max(1, RuneBallList.Length) * (float)(RuneBallList.Length - num4);
					num4++;
				}
			}
			if (!uncheckedRefRW5.ValueRW.IsInitialized)
			{
				uncheckedRefRW5.ValueRW.IsInitialized = true;
				if (!uncheckedRefRW6.ValueRO.IsFallSpell)
				{
					uncheckedRefRW5.ValueRW.CurrentAngle = uncheckedRefRW5.ValueRW.TargetAngle;
					uncheckedRefRW6.ValueRW.AroundAngle = uncheckedRefRW5.ValueRW.TargetAngle;
				}
				uncheckedRefRW5.ValueRW.DirectExplosion = uncheckedRefRW7.ValueRO.Int1 > 0;
				uncheckedRefRW8.ValueRW.DisableAutoCreateFallEffect = true;
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__EnterDoorDestroy_RW_ComponentLookup, ref state, entity4, !uncheckedRefRW5.ValueRO.IsRuneBall);
				if (uncheckedRefRW6.ValueRW.IsFallSpell)
				{
					SpellSingleton singleton2 = __query_1538284518_3.GetSingleton<SpellSingleton>();
					Entity singletonEntity = __query_1538284518_4.GetSingletonEntity();
					EntityCommandBuffer.ParallelWriter parallelWriter = __query_1538284518_5.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
					uncheckedRefRW7.ValueRW.ColorType.ColorEnumToString(out var result);
					parallelWriter.AppendToBuffer(0, singletonEntity, new SpellEffectSystem.Require
					{
						Settings = singleton2.Effects[4026]["FallTrail"],
						Entity = entity4,
						Color = result,
						SpellId = 4026
					});
				}
				if (uncheckedRefRW5.ValueRO.IsRuneBall)
				{
					uncheckedRefRW8.ValueRW.DisableSplitEffect = true;
				}
				else if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, uncheckedRefRW8.ValueRW.SpellEffectEntity))
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW8.ValueRW.SpellEffectEntity).ValueRW.Scale = 0f;
				}
			}
			int item5 = PlayerMgr.Inst.GetPlayerRuneCount().GreenRune;
			uncheckedRefRW5.ValueRW.CurrentGreenRuneBaseDamage = (float)math.max(1, item5) * uncheckedRefRW7.ValueRO.Float1;
			int runeEffectLevel = PlayerMgr.Inst.GetRuneEffectLevel(item5);
			if (runeEffectLevel >= 2)
			{
				uncheckedRefRW5.ValueRW.BonusSpawnCount = (int)math.floor((float)item5 / 10f);
			}
			if (runeEffectLevel >= 3)
			{
				uncheckedRefRW5.ValueRW.CurrentGreenRuneBaseDamage += InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, PlayerMgr.Inst.PlayerEtt).ValueRO.unitCfg.maxHP * 0.4f;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4026GreenRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		__query_1538284518_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4026GreenRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1538284518_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284518_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284518_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284518_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1538284518_5 = entityQueryBuilder2.Build(ref state);
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
		((Spell4026RecordSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4026RecordSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell4026RecordSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4026RecordSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
