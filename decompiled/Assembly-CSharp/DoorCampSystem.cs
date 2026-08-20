using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine.SceneManagement;

[CompilerGenerated]
[BurstCompile]
[UpdateInGroup(typeof(SceneEnterDoorClearPoolGroup))]
public struct DoorCampSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		public ComponentLookup<InteractiveObj_Dots> __InteractiveObj_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__InteractiveObj_Dots_RW_ComponentLookup = state.GetComponentLookup<InteractiveObj_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000059ED_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000059ED_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000059ED_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_83122385_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DoorCamp_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		Entity singletonEntity = __query_83122385_0.GetSingletonEntity();
		RefRW<InteractiveObj_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__InteractiveObj_Dots_RW_ComponentLookup, ref state, singletonEntity);
		if (componentRWAfterCompletingDependency.ValueRW.onSelect)
		{
			componentRWAfterCompletingDependency.ValueRW.onSelect = false;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.ett_Outline).ValueRW.Scale = 1f;
		}
		if (componentRWAfterCompletingDependency.ValueRW.onDeselect)
		{
			componentRWAfterCompletingDependency.ValueRW.onDeselect = false;
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentRWAfterCompletingDependency.ValueRW.ett_Outline).ValueRW.Scale = 0f;
		}
		if (!componentRWAfterCompletingDependency.ValueRW.onInteract)
		{
			return;
		}
		componentRWAfterCompletingDependency.ValueRW.onInteract = false;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		UIMgr.Inst.uiFade.Show(delegate
		{
			UIMgr.Inst.uiFade.Hide(0f);
			if (DataMgr.selectedWorldData.finishedDifficulty.Count > 0)
			{
				GameUISingletonMono<UIChapterThrough>.Inst.ShowAndSelect(delegate
				{
					GameMgr.Inst.ClearAllPool();
					DataMgr.selectedWorldData.timeuse = 1f;
					SceneManager.LoadScene("Battle");
				});
			}
			else
			{
				GameUISingletonMono<UIChapterThrough>.Inst.Show(0, delegate
				{
					GameMgr.Inst.ClearAllPool();
					DataMgr.selectedWorldData.timeuse = 1f;
					SceneManager.LoadScene("Battle");
				});
			}
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<DoorCamp_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_83122385_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000059ED_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((DoorCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((DoorCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((DoorCampSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
