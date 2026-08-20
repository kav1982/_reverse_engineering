using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
internal struct SpecialObj10System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1203445036_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj10_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj10_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<PhysicsCollider> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj10_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj10_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1203445036_0.TypeHandle __IFE_1203445036_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1203445036_0_TypeHandle = new IFE_1203445036_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005DCB_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005DCB_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005DCB_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1203445036_0;

	private EntityQuery __query_1203445036_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpecialObj10_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_1203445036_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		bool flag = false;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> item5 in IFE_1203445036_0.Query(__query_1203445036_0, __TypeHandle.__IFE_1203445036_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj10_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW3 = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				uncheckedRefRW3.ValueRW.MakeUnique(in entity2, ecb);
				uncheckedRefRW.ValueRW.so10Mono.Value = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO10Mono", uncheckedRefRO.ValueRO.Position).GetComponent<SO10Mono>();
				uncheckedRefRW.ValueRW.so10Mono.Value.Initialize(entity2);
			}
			if (uncheckedRefRW2.ValueRW.onSelect)
			{
				uncheckedRefRW2.ValueRW.onSelect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW2.ValueRW.ett_Outline).ValueRW.Scale = 1f;
			}
			if (uncheckedRefRW2.ValueRW.onDeselect)
			{
				uncheckedRefRW2.ValueRW.onDeselect = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW2.ValueRW.ett_Outline).ValueRW.Scale = 0f;
			}
			if (!uncheckedRefRW2.ValueRW.onInteract)
			{
				continue;
			}
			uncheckedRefRW2.ValueRW.onInteract = false;
			if (!uncheckedRefRW.ValueRW.IsPlayerHaveCurse() || !uncheckedRefRW.ValueRW.IsHpAndShieldEnoughToBuy())
			{
				continue;
			}
			int afterDiscountCost = uncheckedRefRW.ValueRW.GetAfterDiscountCost();
			flag = true;
			TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			elem.damage = afterDiscountCost;
			elem.ignorePlayerInvincibleFrame = true;
			elem.ignoreRelicDodge = true;
			elem.ignoreRelicOrCurseDamageRatioChange = true;
			elem.ignoreUmbrella = true;
			InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref state, PlayerMgr.Inst.PlayerEtt).Add(elem);
			uncheckedRefRW.ValueRW.useTimer++;
			if (uncheckedRefRW.ValueRW.useTimer >= uncheckedRefRW.ValueRW.maxUseTime)
			{
				uncheckedRefRW.ValueRW.isOveruse = true;
				for (int i = 0; i < uncheckedRefRW.ValueRW.brokenEFCount; i++)
				{
					Vector3 point = (Vector3)uncheckedRefRO.ValueRO.Position + (Vector3)uncheckedRefRW.ValueRW.brokenEFOffset + Tool2D.GetDir() * UnityEngine.Random.Range(0f, uncheckedRefRW.ValueRW.brokenEFRadius);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", point, 2f);
				}
				SEMgr.Inst.dead_Rock.PlaySE();
				DTool.SetCollider(in uncheckedRefRW3.ValueRO, 0u);
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Normal).ValueRW.Scale = 0f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Used).ValueRW.Scale = 1f;
			}
			uncheckedRefRW.ValueRW.so10Mono.Value.PlayerHPOrShiledChange();
		}
		if (flag)
		{
			PlayerMgr.Inst.ItemCtrller.CurseRemoveByIndex(UnityEngine.Random.Range(0, PlayerMgr.Inst.BaData.curseIDs.Count));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj10_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		__query_1203445036_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1203445036_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005DCB_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpecialObj10System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpecialObj10System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj10System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
