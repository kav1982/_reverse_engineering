using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using Unity.Transforms;

[CompilerGenerated]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
public struct Relic_GluttonousSnakeBodySystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_137045710_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public BufferAccessor<StatefulTriggerEvent> item3_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Relic_GluttonousSnakeBody>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item2_IntPtr, index), item3_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Relic_GluttonousSnakeBody> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RW;

			private BufferTypeHandle<StatefulTriggerEvent> item3_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Relic_GluttonousSnakeBody>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item3_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item3_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Relic_GluttonousSnakeBody>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_137045710_0.TypeHandle __IFE_137045710_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<Spell1003ButterflyBeAttackedTag> __Spell1003ButterflyBeAttackedTag_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_137045710_0_TypeHandle = new IFE_137045710_0.TypeHandle(ref state);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1003ButterflyBeAttackedTag_RW_ComponentLookup = state.GetComponentLookup<Spell1003ButterflyBeAttackedTag>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00004F64_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00004F64_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00004F64_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_137045710_0;

	private EntityQuery __query_137045710_1;

	private EntityQuery __query_137045710_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Relic_GluttonousSnakeBody>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = __query_137045710_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, DynamicBuffer<StatefulTriggerEvent>> item4 in IFE_137045710_0.Query(__query_137045710_0, __TypeHandle.__IFE_137045710_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Relic_GluttonousSnakeBody> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item2;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item3;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				float x = uncheckedRefRW.ValueRW.damage / 10f;
				uncheckedRefRW2.ValueRW.Scale = math.pow(x, 0.2f);
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (dynamicBuffer[i].State != StatefulEventState.Enter)
				{
					continue;
				}
				bool flag = false;
				Entity targetEtt = dynamicBuffer[i].GetOtherEntity(entity2);
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, targetEtt) && DTool.IsSameCamp(InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, targetEtt).unitCfg.unitType, UnitType.Monster))
				{
					flag = true;
					TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
					damageInfo.damage = uncheckedRefRW.ValueRW.damage;
					damageInfo.damageRecordId = uncheckedRefRW.ValueRW.RelicID;
					damageInfo.extraCriticalChance = PlayerMgr.Inst.ExtraCriticalRatio;
					UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, state.EntityManager);
				}
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, targetEtt))
				{
					RefRW<SpellConfigComponentData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state, targetEtt);
					if (DTool.IsSameCamp(componentRWAfterCompletingDependency.ValueRW.ShooterType, UnitType.Monster))
					{
						SpellConfigComponentData valueRW = componentRWAfterCompletingDependency.ValueRW;
						if (valueRW.AbilityType == SpellAbilityType.Rollball && valueRW.Float1 > 0f)
						{
							flag = true;
							EntityManager manager = state.EntityManager;
							SpellTools.AttackRollball(in manager, uncheckedRefRW.ValueRW.damage, ref componentRWAfterCompletingDependency.ValueRW, in targetEtt);
						}
						else if (componentRWAfterCompletingDependency.ValueRW.AbilityType == SpellAbilityType.Butterfly)
						{
							InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__Spell1003ButterflyBeAttackedTag_RW_ComponentLookup, ref state, targetEtt, value: true);
						}
					}
				}
				if (flag && !uncheckedRefRW.ValueRW.isHit)
				{
					uncheckedRefRW.ValueRW.isHit = true;
					PlayerMgr.Inst.ItemCtrller.relic_GluttonousSnake.RemoveBody(entity2);
					SpellSingleton singleton = __query_137045710_2.GetSingleton<SpellSingleton>();
					FixedString32Bytes effectName = "Hit";
					singleton.TryGetSpellEffectEntity(1001, in effectName, SpellColorType.Player, out var entity3);
					Entity entity4 = state.EntityManager.Instantiate(entity3);
					RefRW<LocalTransform> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity4);
					componentRWAfterCompletingDependency2.ValueRW.Position = uncheckedRefRW2.ValueRW.Position;
					componentRWAfterCompletingDependency2.ValueRW.Scale = uncheckedRefRW2.ValueRW.Scale;
					SEMgr.Inst.spell1001Hit.PlaySE();
					entityCommandBuffer.DestroyEntity(entity2);
					break;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Relic_GluttonousSnakeBody>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		__query_137045710_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_137045710_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_137045710_2 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00004F64_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Relic_GluttonousSnakeBodySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Relic_GluttonousSnakeBodySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Relic_GluttonousSnakeBodySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
