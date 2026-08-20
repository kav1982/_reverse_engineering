using System.Linq;
using UnityEngine;

public static class WandExtend
{
	public static void TryTriggerEchoEffect(Wand sourceWand)
	{
		foreach (Wand item in PlayerMgr.Inst.Wands.Where((Wand e) => (object)e != null && e.WandCfg != null))
		{
			if (!(item == sourceWand) && item.passiveEchoShootChance > 0f && Random.Range(0f, 100f) <= item.passiveEchoShootChance)
			{
				item.TryShoot(fromEcho: true);
			}
		}
	}

	public static bool ReplaceWandSpell(this Wand wand, WandSlotType slotType, int slotIndex, SlotData spell, bool toLock)
	{
		if (wand == null || wand.WandCfg == null)
		{
			return false;
		}
		SlotData[] slotsData = wand.WandCfg.GetSlotsData(slotType);
		bool[] slotsLockState = wand.WandCfg.GetSlotsLockState(slotType);
		if (slotsLockState[slotIndex])
		{
			return false;
		}
		if (!slotsData.Bag_CanSetSlotWithPush(slotsLockState, spell, slotIndex))
		{
			if (slotsLockState.BagLocks_GetCanMoveSpace(slotIndex) < spell.GetFinalSlotCost())
			{
				return false;
			}
			if (slotsData[slotIndex] != null)
			{
				int num = slotIndex;
				if (slotsData[slotIndex].isSealSlot)
				{
					num = slotsData.Bag_GetOwnerSlotIndex(slotIndex);
					if (slotsLockState[num])
					{
						return false;
					}
				}
				if (slotsData.Bag_CanPushToRight(slotsLockState, num))
				{
					slotsData.Bag_PushToRight(slotsLockState, num);
				}
				else
				{
					wand.PopWandSpellToBag(slotType, num, instantlyResetWand: false);
				}
			}
			while (!slotsData.Bag_CanSetSlotWithPush(slotsLockState, spell, slotIndex))
			{
				int num2 = slotsData.Bag_GetNextSpell(slotIndex);
				if (num2 < 0 || slotsLockState[num2])
				{
					break;
				}
				Debug.Log($"POP RIGHT {num2}");
				wand.PopWandSpellToBag(slotType, num2, instantlyResetWand: false);
			}
			while (!slotsData.Bag_CanSetSlotWithPush(slotsLockState, spell, slotIndex))
			{
				int num3 = slotsData.Bag_GetPreviousSpell(slotIndex);
				if (num3 < 0 || slotsLockState[num3])
				{
					break;
				}
				Debug.Log($"POP LEFT {num3}");
				wand.PopWandSpellToBag(slotType, num3, instantlyResetWand: false);
			}
		}
		slotsData.Bag_SetSlotWithPush(slotsLockState, spell, slotIndex);
		slotsLockState[slotIndex] = true;
		wand.ResetAndRecheck();
		UIPlayerDataMgr.Inst.WandUpdate(PlayerMgr.Inst.Wands.IndexOf(wand));
		return true;
	}

	public static bool PopWandSpellToBag(this Wand wand, WandSlotType slotType, int slotIndex, bool instantlyResetWand = true)
	{
		if (wand == null || wand.WandCfg == null)
		{
			return false;
		}
		SlotData[] slotsData = wand.WandCfg.GetSlotsData(slotType);
		SlotData spellData = slotsData[slotIndex];
		slotsData.Bag_RemoveSlot(slotIndex);
		PlayerMgr.Inst.SpellPick(spellData);
		if (instantlyResetWand)
		{
			wand.ResetAndRecheck();
		}
		UIPlayerDataMgr.Inst.WandUpdate(PlayerMgr.Inst.Wands.IndexOf(wand));
		return true;
	}

	public static void SetWandSlotLockState(this Wand wand, WandSlotType slotType, int slotIndex, bool lockState)
	{
		if ((object)wand != null && wand.WandCfg != null)
		{
			wand.WandCfg.GetSlotsLockState(slotType)[slotIndex] = lockState;
			UIPlayerDataMgr.Inst.WandUpdate(PlayerMgr.Inst.Wands.IndexOf(wand));
		}
	}

	public static int ReplaceWandSpellToFirstSlot(this Wand wand, WandSlotType slotType, SlotData spell, bool toLock)
	{
		if (wand == null || wand.WandCfg == null)
		{
			return -1;
		}
		SlotData[] slotsData = wand.WandCfg.GetSlotsData(slotType);
		for (int i = 0; i < slotsData.Length; i++)
		{
			if (wand.ReplaceWandSpell(slotType, i, spell, toLock))
			{
				return slotsData.Bag_GetOwnerSlotIndex(i);
			}
		}
		return -1;
	}

	public static bool CheckWandEnableMirrorOfSoul(this Wand wand)
	{
		if (!checkSlots(isPost: false))
		{
			return checkSlots(isPost: true);
		}
		return true;
		bool checkSlots(bool isPost)
		{
			SlotData[] slotsData = wand.WandCfg.GetSlotsData(isPost ? WandSlotType.Post : WandSlotType.Normal);
			bool[] slotsLockState = wand.WandCfg.GetSlotsLockState(isPost ? WandSlotType.Post : WandSlotType.Normal);
			for (int i = 0; i < slotsData.Length; i++)
			{
				if (slotsData[i] != null && !slotsData[i].isSealSlot)
				{
					SpellConfig finalConfig = slotsData[i].GetFinalConfig();
					if (finalConfig.abilityType == SpellAbilityType.WandSpirit && finalConfig.level >= 2 && !slotsData[i].isAllFieldSharedSpell && slotsLockState[i])
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public static void ApplyMirrorOfSoulToWand(this Wand wand)
	{
		if (!wand.CheckWandEnableMirrorOfSoul() && wand.ReplaceWandSpellToFirstSlot(WandSlotType.Normal, new SlotData(40052), toLock: true) < 0 && wand.ReplaceWandSpellToFirstSlot(WandSlotType.Post, new SlotData(40052), toLock: true) < 0)
		{
			Debug.LogError($"灵魂镜子的杖灵应用失败?! wandIndex={PlayerMgr.Inst.Wands.IndexOf(wand)}  wandId={wand.WandCfg.id}");
		}
	}
}
