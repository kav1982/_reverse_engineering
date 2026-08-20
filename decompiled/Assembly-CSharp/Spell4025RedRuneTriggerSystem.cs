using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;

[CompilerGenerated]
[UpdateAfter(typeof(UnitTakeDamageClearSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
internal struct Spell4025RedRuneTriggerSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_631017952_0;

	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingletonBuffer<Spell4025RuneSlashSpawnData>();
		state.RequireForUpdate<Spell4025RuneSlashSpawnData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		DynamicBuffer<Spell4025RuneSlashSpawnData> singletonBuffer = __query_631017952_0.GetSingletonBuffer<Spell4025RuneSlashSpawnData>();
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		foreach (Spell4025RuneSlashSpawnData item in singletonBuffer)
		{
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, item.SpellEntity))
			{
				Wand value = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, item.SpellEntity).ValueRW.Wand.Value;
				if ((bool)value && value.WandCfg != null)
				{
					value.TryShootRedRune(item.TargetPosition, item.IsCriticalHit);
				}
			}
		}
		singletonBuffer.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4025RuneSlashSpawnData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_631017952_0 = entityQueryBuilder2.Build(ref state);
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
		((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4025RedRuneTriggerSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
