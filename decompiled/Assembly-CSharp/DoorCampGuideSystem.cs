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
using Unity.Transforms;

[UpdateInGroup(typeof(SceneGroup))]
[CompilerGenerated]
[BurstCompile]
public struct DoorCampGuideSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1020639312_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<DoorCampGuide>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<DoorCampGuide>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<DoorCampGuide> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<DoorCampGuide>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<DoorCampGuide>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<DoorCampGuide>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<DoorCampGuide>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1020639312_0.TypeHandle __IFE_1020639312_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1020639312_0_TypeHandle = new IFE_1020639312_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000059D8_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000059D8_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000059D8_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1020639312_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DoorCampGuide>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (var (uncheckedRefRW, uncheckedRefRW2, uncheckedRefRW3) in IFE_1020639312_0.Query(__query_1020639312_0, __TypeHandle.__IFE_1020639312_0_TypeHandle, ref state))
		{
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
				GuideMgr.Inst.EnterDoor();
			}
			if (uncheckedRefRW.ValueRW.onHideMask)
			{
				uncheckedRefRW.ValueRW.onHideMask = false;
				DTool.SetCollider(in uncheckedRefRW3.ValueRO, 33554432u);
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_Portal).ValueRW.Scale = 1f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, uncheckedRefRW.ValueRW.ett_CloseMask).ValueRW.Scale = 0f;
				SEMgr.Inst.puzzleSucceed.PlaySE();
				GuideMgr.Inst.go_OpenDoorEF.SetActive(value: true);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<DoorCampGuide>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		__query_1020639312_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000059D8_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((DoorCampGuideSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((DoorCampGuideSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((DoorCampGuideSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
