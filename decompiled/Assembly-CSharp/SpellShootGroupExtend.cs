using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpellShootGroupExtend
{
	public static float GetGroupManaCost_FinalPlayerValue(this SpellShootGroup group, Wand wand, bool ignoreParentGroup = false)
	{
		return group?.Shoots.Sum((SpellShootData e) => e.GetSpellManaCostAndSubGroupManaCost_FinalPlayerValue(wand, ignoreParentGroup)) ?? 0f;
	}

	public static SlotData[] GetAllSlotData(this SpellShootGroup self)
	{
		List<SlotData> result = new List<SlotData>();
		SpellShootData[] shoots = self.Shoots;
		foreach (SpellShootData self2 in shoots)
		{
			result.AddRange(from e in self2.GetAllSlotData()
				where !result.Contains(e)
				select e);
		}
		return result.ToArray();
	}

	public static IEnumerable<SlotData> GetCanShootSlotData(this SpellShootGroup self)
	{
		return self.Shoots.SelectMany((SpellShootData e) => e.GetCanShootSlotData());
	}

	public static float GetMultipleGroupRecoil_FinalPlayerValue(this SpellShootGroup[] group, Wand wand)
	{
		float num = float.NegativeInfinity;
		float num2 = float.PositiveInfinity;
		for (int i = 0; i < group.Length; i++)
		{
			(float, float) groupRecoilData = group[i].GetGroupRecoilData();
			num = Mathf.Max(num, groupRecoilData.Item1);
			num2 = Mathf.Min(num2, groupRecoilData.Item2);
		}
		float overalRecoil = GetOveralRecoil(num, num2);
		RatioValue recoilRatio = GetRecoilRatio(wand);
		return overalRecoil * recoilRatio.CurrentMulRatio;
	}

	public static RatioValue GetRecoilRatio(Wand shootWand)
	{
		RatioValue ratioValue = new RatioValue(0.0);
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_AddRecoil != null)
		{
			ratioValue.MulRatio(1f + (float)PlayerMgr.Inst.ItemCtrller.curseCfg_AddRecoil.int1.result / 100f);
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_IsReverseRecoil)
		{
			ratioValue.MulRatio(-1.0);
		}
		if ((object)shootWand != null && shootWand.WandCfg != null)
		{
			if (shootWand.WandCfg.specialAbility == WandAbility.StandCastNoRecoil && PlayerMgr.Inst.PlayerCtrller.isStandInLastFrame)
			{
				ratioValue.MulRatio(0.0);
			}
			else if (shootWand.WandCfg.specialAbility == WandAbility.ReverseRecoil)
			{
				ratioValue.MulRatio(-1.0);
			}
		}
		return ratioValue;
	}

	private static float GetOveralRecoil(float highestRecoil, float lowestRecoil)
	{
		if (highestRecoil * lowestRecoil >= 0f)
		{
			if (!(Mathf.Abs(highestRecoil) >= Mathf.Abs(lowestRecoil)))
			{
				return lowestRecoil;
			}
			return highestRecoil;
		}
		return highestRecoil + lowestRecoil;
	}
}
