using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[UpdateBefore(typeof(UnitPropertySystem))]
[CompilerGenerated]
[UpdateAfter(typeof(UnitBeforeTakeDamageSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
public class PlayerTakeDamageChargeWandSystem : SystemBase
{
	private struct TypeHandle
	{
		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
		}
	}

	private TypeHandle __TypeHandle;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<PlayerController_Dots>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (!PlayerMgr.Inst || PlayerMgr.Inst.PlayerEtt == default(Entity) || !base.EntityManager.Exists(PlayerMgr.Inst.PlayerEtt) || InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef, PlayerMgr.Inst.PlayerEtt).Length <= 0)
		{
			return;
		}
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if ((object)wand != null)
			{
				WandConfig wandCfg = wand.WandCfg;
				if (wandCfg != null && wandCfg.postSlotTriggerType == WandPostSlotTriggerType.TakeDamage)
				{
					WandPostSlotTrigger.PostSlotSpellTakeDamageTriggerCheck();
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		new EntityQueryBuilder(Allocator.Temp).Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public PlayerTakeDamageChargeWandSystem()
	{
	}
}
