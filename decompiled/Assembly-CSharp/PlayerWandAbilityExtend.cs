using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerWandAbilityExtend
{
	private static IEnumerable<Wand> FilterWandByAbility(this PlayerMgr self, WandAbility ability)
	{
		return self.Wands.Where((Wand e) => (object)e != null && e.WandCfg != null && e.WandCfg.specialAbility == ability);
	}

	public static float MpCostRatioFromWandAbility(this PlayerMgr self)
	{
		return self.FilterWandByAbility(WandAbility.AllWandMpCostRatio).Aggregate(1f, (float value, Wand wand) => value * wand.WandCfg.float1 / 100f);
	}

	public static float MaxMpRatioFromWandAbility(this PlayerMgr self)
	{
		return self.FilterWandByAbility(WandAbility.AllWandMaxMpRatio).Aggregate(1f, (float v, Wand w) => v * w.WandCfg.float1 / 100f);
	}

	public static float PostSlotChargeRatioFromWandAbility(this PlayerMgr self)
	{
		return self.FilterWandByAbility(WandAbility.AllWandPostSlotChargeRatioUp).Aggregate(1f, (float v, Wand w) => v + w.WandCfg.float1 / 100f);
	}

	public static float MpGainAmountFixFromWandAbility(this PlayerMgr self)
	{
		return self.FilterWandByAbility(WandAbility.AllWandMpGainAmountFix).Sum((Wand e) => e.WandCfg.float1);
	}

	public static float EffectRadiuRatioFromWandAbility(this PlayerMgr self)
	{
		return self.FilterWandByAbility(WandAbility.AllWandEffectRadiuRatio).Sum((Wand e) => e.WandCfg.float1);
	}

	public static float MaxManaAmountFromWandAbility(this PlayerMgr self, Wand ignoreWand = null)
	{
		return (from e in self.FilterWandByAbility(WandAbility.Battery)
			where e != ignoreWand
			select e).Sum((Wand e) => e.MaxMP);
	}

	public static float CurrentManaAmountFromWandAbility(this PlayerMgr self, Wand ignoreWand = null)
	{
		return (from e in self.FilterWandByAbility(WandAbility.Battery)
			where e != ignoreWand
			select e).Sum((Wand e) => e.CurrentMP);
	}

	public static void CostMpFromWandAbility(this PlayerMgr self, Wand sourceWand, float cost)
	{
		foreach (Wand item in from e in self.Wands
			where (object)e != null && e.WandCfg != null && e != sourceWand
			where e.WandCfg.specialAbility == WandAbility.Battery
			select e)
		{
			float num = Mathf.Min(cost, item.CurrentMP);
			item.CostMp(num);
			cost -= num;
			if (cost <= 0f)
			{
				break;
			}
		}
		if (cost > 0f)
		{
			Debug.LogError($"\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdص\ufffdħ\ufffd\ufffdʱ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd {cost} \ufffd\ufffdħ\ufffd\ufffd\ufffd\ufffd\ufffdĲ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdΪ\ufffd\ufffd\ufffd\ufffdħ\ufffd\ufffd\ufffd\ufffd\ufffd㣡");
		}
	}

	public static float PlayerMoveSpeedRatioFromWandAbility(this PlayerMgr self)
	{
		float num = 1f;
		foreach (Wand wand in self.Wands)
		{
			if (!(wand == null) && wand.WandCfg != null && !(self.SelectedWand != wand))
			{
				if (!wand.passiveAutoWand)
				{
					num += wand.passiveOwnerSpeedUpRatio;
				}
				if (wand.WandCfg.specialAbility == WandAbility.HoldingWandCharacterAdditiveMoveSpeedRatio || wand.WandCfg.specialAbility == WandAbility.HoldingWandCharacterFly)
				{
					num += wand.WandCfg.float1 / 100f;
				}
			}
		}
		return num;
	}
}
