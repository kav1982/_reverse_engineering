using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[UpdateAfter(typeof(UnitPropertySystem))]
[UpdateBefore(typeof(UnitTakeDamageDeadSystem))]
[CompilerGenerated]
internal struct UnitAfterTakeDamageSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1413569866_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public BufferAccessor<TakeDamageInfo_Dots> item2_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<UnitProperty_Dots>(item1_IntPtr, index), item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<UnitProperty_Dots> item1_ComponentTypeHandle_RO;

			private BufferTypeHandle<TakeDamageInfo_Dots> item2_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitProperty_Dots>(isReadOnly: true);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<TakeDamageInfo_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<TakeDamageInfo_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1413569866_0.TypeHandle __IFE_1413569866_0_TypeHandle;

		public ComponentLookup<PlayerController_Dots> __PlayerController_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1413569866_0_TypeHandle = new IFE_1413569866_0.TypeHandle(ref state);
			__PlayerController_Dots_RW_ComponentLookup = state.GetComponentLookup<PlayerController_Dots>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009246_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009246_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009246_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnDestroy_00009248_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00009248_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00009248_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1413569866_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<UnitProperty_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, DynamicBuffer<TakeDamageInfo_Dots>> item3 in IFE_1413569866_0.Query(__query_1413569866_0, __TypeHandle.__IFE_1413569866_0_TypeHandle, ref state))
		{
			item3.Deconstruct(out var item, out var item2, out var entity);
			InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots> uncheckedRefRO = item;
			DynamicBuffer<TakeDamageInfo_Dots> dynamicBuffer = item2;
			Entity entity2 = entity;
			if (dynamicBuffer.Length <= 0)
			{
				continue;
			}
			bool flag = false;
			if (uncheckedRefRO.ValueRO.unitCfg.isHybirdUnit)
			{
				UnitPptReference unitPptReference = null;
				for (int i = 0; i < dynamicBuffer.Length; i++)
				{
					if (!dynamicBuffer[i].immuneDamage && dynamicBuffer[i].realDamage > 0f)
					{
						flag = true;
						if (unitPptReference == null && uncheckedRefRO.ValueRO.unitCfg.isHybirdUnit)
						{
							unitPptReference = state.EntityManager.GetComponentObject<UnitPptReference>(entity2);
						}
						if (uncheckedRefRO.ValueRO.unitCfg.isHybirdUnit && !uncheckedRefRO.ValueRO.isDead)
						{
							unitPptReference.unitPpt.UnitBas.AfterTakeDamage_Dots(ref dynamicBuffer.ElementAt(i));
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < dynamicBuffer.Length; j++)
				{
					if (!dynamicBuffer[j].immuneDamage && dynamicBuffer[j].realDamage > 0f)
					{
						flag = true;
						break;
					}
				}
			}
			if (uncheckedRefRO.ValueRO.unitCfg.id == 800001)
			{
				ref PlayerController_Dots valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__PlayerController_Dots_RW_ComponentLookup, ref state, entity2).ValueRW;
				if (!valueRW.isHurtThisFrameForPlayer)
				{
					continue;
				}
				valueRW.isHurtThisFrameForPlayer = false;
				UIPlayerDataMgr.Inst.UpdateHP();
				if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
				{
					SEMgr.Inst.injured_Huang.PlaySE();
				}
				else if (PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons != null)
				{
					SEMgr.Inst.injured_Dave.PlaySE();
				}
				else if (PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null)
				{
					if (DataMgr.selectedWorldData.playerLook == PlayerLook.Frog)
					{
						SEMgr.Inst.injured_Frog.PlaySE();
					}
					else if (DataMgr.selectedWorldData.playerLook == PlayerLook.Horse)
					{
						SEMgr.Inst.injured_Horse.PlaySE();
					}
					else
					{
						SEMgr.Inst.injured_Player.PlaySE();
					}
				}
				else
				{
					SEMgr.Inst.injured_Player.PlaySE();
				}
				if (!GameMgr.IsHarmony_Static)
				{
					UIPlayerDataMgr.Inst.UIPlayerInjuredPlay();
				}
			}
			else if (flag && uncheckedRefRO.ValueRO.unitCfg.id != 800001)
			{
				UnitConfig.map[uncheckedRefRO.ValueRO.unitCfg.id].beHitSE.Value.PlaySE(SEPlayMode.Unique, 3, 0.2f);
			}
		}
		if (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, PlayerMgr.Inst.PlayerEtt))
		{
			return;
		}
		DynamicBuffer<TakeDamageInfo_Dots> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref state, PlayerMgr.Inst.PlayerEtt);
		UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, PlayerMgr.Inst.PlayerEtt);
		for (int k = 0; k < bufferAfterCompletingDependency.Length; k++)
		{
			TakeDamageInfo_Dots takeDamageInfo_Dots = bufferAfterCompletingDependency[k];
			if (!takeDamageInfo_Dots.immuneDamage && (takeDamageInfo_Dots.realDamage > 0f || takeDamageInfo_Dots.hitMagicShieldDamage > 0f) && !componentAfterCompletingDependency.isDead)
			{
				PlayerMgr.Inst.PlayerCtrller.AfterTakeDamage(ref bufferAfterCompletingDependency.ElementAt(k));
			}
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<UnitProperty_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TakeDamageInfo_Dots>();
		__query_1413569866_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00009246_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((UnitAfterTakeDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00009248_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((UnitAfterTakeDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((UnitAfterTakeDamageSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((UnitAfterTakeDamageSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
