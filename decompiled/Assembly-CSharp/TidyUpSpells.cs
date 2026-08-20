using System;
using System.Collections.Generic;
using System.Linq;
using PlayerLogger;
using PlayerLogger.Events;
using UnityEngine;

public static class TidyUpSpells
{
	public static void TidyUpBagAndAllWand()
	{
		if (UIPlayerDataMgr.Inst.uiSlotWand_Drag != null || UIPlayerDataMgr.Inst.uiSlotBag_Drag != null || UIPlayerDataMgr.Inst.uislotWandSelected != null || UIPlayerDataMgr.Inst.uislotBagSelected != null || UIPlayerDataMgr.Inst.isChangingSpell)
		{
			return;
		}
		List<int> bagSpellIds = GetBagSpellIds();
		AutoFullLogger autoFullLogger = new AutoFullLogger
		{
			resources = ResourcesStatus.CreateAuto(),
			before_in_bag_spells = bagSpellIds,
			before_wands = PlayerLogger.Wand.CreateAuto()
		};
		UIPlayerDataMgr.Inst.UISlotWandDragCancel();
		UIPlayerDataMgr.Inst.UISlotBagDragCancel();
		UIPlayerDataMgr.Inst.MobileChangeSpellRecover();
		TidyUpBag();
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			if (PlayerMgr.Inst.Wands[i].WandCfg != null)
			{
				TidyUpWand(i);
			}
		}
		autoFullLogger.after_wands = PlayerLogger.Wand.CreateAuto();
		autoFullLogger.after_in_bag_spells = GetBagSpellIds();
		autoFullLogger.Report();
	}

	private static List<int> GetBagSpellIds()
	{
		List<SlotData> bagSpellDatas = PlayerMgr.Inst.BaData.bagSpellDatas;
		List<int> list = new List<int>();
		for (int i = 0; i < bagSpellDatas.Count; i++)
		{
			if (bagSpellDatas[i] != null)
			{
				list.Add(bagSpellDatas[i].id);
			}
		}
		return list;
	}

	public static void TidyUpBag()
	{
		int num = ((PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox != null) ? PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox.int1.result : 0);
		List<SlotData> list = new List<SlotData>();
		for (int i = num; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			SlotData slotData = PlayerMgr.Inst.BaData.bagSpellDatas[i];
			if (slotData != null && !slotData.isSealSlot)
			{
				list.Add(slotData);
				PlayerMgr.Inst.BagSpellChange(i, null);
			}
		}
		list.Sort((SlotData x, SlotData y) => x.GetFinalId() - y.GetFinalId());
		SlotData[] array = PlayerMgr.Inst.BaData.bagSpellDatas.ToArray();
		bool[] locks = new bool[array.Length];
		for (int j = num; j < array.Length; j++)
		{
			if (array[j] == null)
			{
				if (list.Count == 0)
				{
					break;
				}
				array.Bag_SetSlot(locks, list[0], j);
				list.RemoveAt(0);
			}
		}
		PlayerMgr.Inst.BaData.bagSpellDatas = array.ToList();
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public static void TidyUpWand(int wandIndex)
	{
		Wand wand = PlayerMgr.Inst.Wands[wandIndex];
		try
		{
			TidyInWandSlots(wand.WandCfg.normalSlots, wand.WandCfg.normalSlotIsLock);
			TidyInWandSlots(wand.WandCfg.postSlots, wand.WandCfg.postSlotIsLock);
		}
		catch (Exception arg)
		{
			string text = $"\n整理法杖 {wandIndex} 存在错误 {arg}，法杖内容：\n";
			foreach (Wand wand2 in PlayerMgr.Inst.Wands)
			{
				text += $"{wand2.WandIndex}----------:\n";
				if (!(wand2 == null) && wand2.WandCfg != null)
				{
					text = text + SlotsToString(wand2.WandCfg.normalSlots, wand2.WandCfg.normalSlotIsLock) + "\n";
					text = text + SlotsToString(wand.WandCfg.postSlots, wand.WandCfg.postSlotIsLock) + "\n";
				}
			}
			Debug.LogError(text);
		}
		wand.ResetAndRecheck();
		UIPlayerDataMgr.Inst.WandUpdate(wandIndex);
	}

	private static string SlotsToString(SlotData[] slots, bool[] locks)
	{
		return string.Join(", ", slots.Select(delegate(SlotData e, int i)
		{
			if (e == null)
			{
				return "null";
			}
			return (!locks[i]) ? e.id.ToString() : $"[{e.id}]";
		}));
	}

	private static void TidyInWandSlots(SlotData[] slots, bool[] locks)
	{
		int? firstCanPushShootableSpellIndex = GetFirstCanPushShootableSpellIndex(slots, locks);
		if (firstCanPushShootableSpellIndex.HasValue)
		{
			int num = -1;
			for (int num2 = firstCanPushShootableSpellIndex.Value - 1; num2 >= 0; num2--)
			{
				if (slots[num2] != null && !slots[num2].isSealSlot && !locks[num2])
				{
					num = num2;
					break;
				}
			}
			while (num > 0 && slots.Bag_CanPushToLeft(locks, num))
			{
				slots.Bag_PushToLeft(locks, num);
				num--;
			}
			for (int i = firstCanPushShootableSpellIndex.Value; i < slots.Length && slots.Bag_CanPushToRight(locks, i); i++)
			{
				slots.Bag_PushToRight(locks, i);
			}
			return;
		}
		int num3 = -1;
		for (int num4 = slots.Length - 1; num4 >= 0; num4--)
		{
			if (slots[num4] != null && !slots[num4].isSealSlot && !locks[num4])
			{
				num3 = num4;
				break;
			}
		}
		while (num3 > 0 && slots.Bag_CanPushToLeft(locks, num3))
		{
			slots.Bag_PushToLeft(locks, num3);
			num3--;
		}
	}

	private static int? GetFirstCanPushShootableSpellIndex(SlotData[] slots, bool[] locks)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (!locks[i] && slots[i] != null && !slots[i].isSealSlot)
			{
				SpellType useType = slots[i].GetFinalConfig().useType;
				if (useType == SpellType.Missile || useType == SpellType.Summon)
				{
					return i;
				}
			}
		}
		return null;
	}
}
