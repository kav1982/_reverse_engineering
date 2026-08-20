using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
internal struct SpecialObj21System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_910399913_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj21_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj21_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<PhysicsCollider> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj21_Dots>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj21_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
		}
	}

	private struct TypeHandle
	{
		public IFE_910399913_0.TypeHandle __IFE_910399913_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public BufferLookup<AnimationEventComponent> __Rukhanka_AnimationEventComponent_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_910399913_0_TypeHandle = new IFE_910399913_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__Rukhanka_AnimationEventComponent_RW_BufferLookup = state.GetBufferLookup<AnimationEventComponent>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00005E86_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00005E86_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00005E86_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_910399913_0;

	private EntityQuery __query_910399913_1;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpecialObj21_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer ecb = __query_910399913_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>> item5 in IFE_910399913_0.Query(__query_910399913_0, __TypeHandle.__IFE_910399913_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj21_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW3 = item4;
			Entity entity2 = entity;
			if (!uncheckedRefRW.ValueRW.isInitialized)
			{
				uncheckedRefRW.ValueRW.isInitialized = true;
				if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Recycler) == 0)
				{
					ecb.DestroyEntity(entity2);
					continue;
				}
				uncheckedRefRW3.ValueRW.MakeUnique(in entity2, ecb);
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
			if (uncheckedRefRW2.ValueRW.onInteract)
			{
				uncheckedRefRW2.ValueRW.onInteract = false;
				GameUISingletonMono<UISell>.ShowInit(entity2);
			}
			if (uncheckedRefRW.ValueRW.onSell)
			{
				uncheckedRefRW.ValueRW.onSell = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(0);
			}
			if (uncheckedRefRW.ValueRW.onBeforeBroken)
			{
				uncheckedRefRW.ValueRW.onBeforeBroken = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(1);
			}
			if (uncheckedRefRW.ValueRW.onBroken)
			{
				uncheckedRefRW.ValueRW.onBroken = false;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Anima).ValueRW.Play(2);
				DTool.SetCollider(in uncheckedRefRW3.ValueRO, 512u);
			}
			DynamicBuffer<AnimationEventComponent> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Rukhanka_AnimationEventComponent_RW_BufferLookup, ref state, uncheckedRefRW.ValueRW.ett_Anima);
			for (int i = 0; i < bufferAfterCompletingDependency.Length; i++)
			{
				switch (bufferAfterCompletingDependency[i].intParam)
				{
				case 0:
				{
					float3 @float = uncheckedRefRO.ValueRO.Position + uncheckedRefRW.ValueRW.brokenEFCenter + new float3(UnityEngine.Random.Range(0f - uncheckedRefRW.ValueRW.brokenEFOffset.x, uncheckedRefRW.ValueRW.brokenEFOffset.x), UnityEngine.Random.Range(0f - uncheckedRefRW.ValueRW.brokenEFOffset.y, uncheckedRefRW.ValueRW.brokenEFOffset.y), 0f);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", @float, 2f);
					SEMgr.Inst.so101_RerollEF.PlaySE();
					break;
				}
				case 1:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_SO101RerollBroken", uncheckedRefRO.ValueRO.Position, 2f);
					SEMgr.Inst.so101_RerollBroken.PlaySE();
					ecb.SetComponentEnabled<SpecialObj21_Dots>(entity2, value: false);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Normal).ValueRW.Scale = 0f;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Used).ValueRW.Scale = 1f;
					break;
				default:
					Debug.LogError(bufferAfterCompletingDependency[i].intParam);
					break;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj21_Dots>();
		__query_910399913_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_910399913_1 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00005E86_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpecialObj21System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpecialObj21System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpecialObj21System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
