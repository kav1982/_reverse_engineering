using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine.SceneManagement;

[CompilerGenerated]
[UpdateInGroup(typeof(SceneEnterDoorClearPoolGroup))]
internal struct DoorEndlessCampSystem : ISystem, ISystemCompilerGenerated
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_397251244_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DoorEndlessCamp>();
	}

	public void OnUpdate(ref SystemState state)
	{
		Entity singletonEntity = __query_397251244_0.GetSingletonEntity();
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
		if (componentRWAfterCompletingDependency.ValueRW.onInteract)
		{
			componentRWAfterCompletingDependency.ValueRW.onInteract = false;
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			UIMgr.Inst.uiFade.Show(delegate
			{
				UIMgr.Inst.uiFade.Hide(0f);
				GameMgr.Inst.ClearAllPool();
				DataMgr.selectedWorldData.timeuse = 1f;
				BattleMgr.EnterEndlessBattle = true;
				SceneManager.LoadScene("Battle");
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<DoorEndlessCamp>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_397251244_0 = entityQueryBuilder2.Build(ref state);
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
		((DoorEndlessCampSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((DoorEndlessCampSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((DoorEndlessCampSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
