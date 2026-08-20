using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public static class SpellShootDataExtend
{
	public static RatioValue GetSummonAttackSpeedRatio_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		return self.GetSummonAttackSpeedRatio();
	}

	public static RatioValue GetSpellMoveSpeed_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		RatioValue spellFlySpeed = self.GetSpellFlySpeed();
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceSpellSpeed != null)
		{
			spellFlySpeed.MulRatio((float)(100 - PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceSpellSpeed.int1.result) / 100f);
		}
		return spellFlySpeed;
	}

	public static RatioValue GetSpellShootScatter_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		RatioValue spellScatter = self.GetSpellScatter();
		spellScatter.AddBase(wand.WandCfg.GetWandScatter());
		if (PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus != null)
		{
			spellScatter.MulRatio(1f - PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus.Cfg.floatTimer);
		}
		return spellScatter;
	}

	public static int GetSpellFinalCount_FinalPlayerValue(this SpellShootData self, [CanBeNull] Wand wand, bool considerSplit = true)
	{
		int shootCount = self.Spell.GetFinalConfig().shootCount;
		int num = 1;
		int num2 = 0;
		int num3 = self.GetSpellMultiShootData().count;
		SpellShootData ownerShootData = self.OwnerGroup.OwnerShootData;
		if (ownerShootData == null)
		{
			if ((object)wand != null)
			{
				WandConfig wandCfg = wand.WandCfg;
				if (wandCfg != null && wandCfg.specialAbility == WandAbility.FourDirShoot)
				{
					num3 *= 4;
				}
			}
		}
		else
		{
			num *= ownerShootData.GetSpellFinalCount_FinalPlayerValue(wand, considerSplit: false);
			SpellConfig finalConfig = ownerShootData.Spell.GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.ArcaneNova)
			{
				num2 += finalConfig.int1;
			}
			if (finalConfig.abilityType == SpellAbilityType.PreFirework)
			{
				num2++;
			}
			SlotData[] triggers = ownerShootData.Triggers;
			for (int i = 0; i < triggers.Length; i++)
			{
				SpellConfig finalConfig2 = triggers[i].GetFinalConfig();
				switch (finalConfig2.abilityType)
				{
				case SpellAbilityType.OnOverTrigger:
					num2++;
					break;
				case SpellAbilityType.OnOverSplitTrigger:
				case SpellAbilityType.OnStartRotationTrigger:
					num2 += finalConfig2.int2;
					break;
				}
			}
		}
		int num4 = shootCount * num3 * math.max(1, num2) * num;
		if (considerSplit)
		{
			num4 *= self.GetSplitCount() + shootCount;
		}
		return num4;
	}

	public static RatioValue GetSpellDuration_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		return self.GetSpellDuration();
	}

	public static RatioValue GetSummonHpAmount_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		return self.GetSummonHp();
	}

	public static SpellSpecialMovementType GetSpellMovementType(this SpellShootData self, Wand wand)
	{
		for (int num = self.EnhanceList.Length - 1; num >= 0; num--)
		{
			switch (self.EnhanceList[num].GetFinalConfig().abilityType)
			{
			case SpellAbilityType.AroundOwner:
			case SpellAbilityType.RandomRotationRadiu:
				return SpellSpecialMovementType.Rotation;
			case SpellAbilityType.AroundMouse:
				return SpellSpecialMovementType.ChaseMouse;
			case SpellAbilityType.FollowTarget:
				return SpellSpecialMovementType.ChaseEnemy;
			case SpellAbilityType.FollowOwner:
				return SpellSpecialMovementType.ChaseOwner;
			}
		}
		return SpellSpecialMovementType.Normal;
	}

	public static float GetSummonHpRecover_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		return self.GetSummonHpRecover();
	}

	public static int GetSummonLimit_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		int num = self.GetSummonLimit();
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_SummonLimit != null)
		{
			num *= PlayerMgr.Inst.ItemCtrller.relicCfg_SummonLimit.int1.result;
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_SummonsReduce != null)
		{
			num = Mathf.CeilToInt((float)num / (float)PlayerMgr.Inst.ItemCtrller.curseCfg_SummonsReduce.int1.result);
		}
		return num;
	}

	public static RatioValue GetSpellCriticalChance_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		RatioValue spellCriticalChance = self.GetSpellCriticalChance();
		spellCriticalChance.AddBase(PlayerMgr.Inst.ExtraCriticalRatio);
		spellCriticalChance.AddBase(wand.WandCfg.criticalChance / 100f);
		return spellCriticalChance;
	}

	public static RatioValue GetSpellEffectRadius_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		RatioValue spellEffectRadius = self.GetSpellEffectRadius();
		spellEffectRadius.AddRatio(PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: true));
		spellEffectRadius.AddRatio(PlayerMgr.Inst.EffectRadiuRatioFromWandAbility() / 100f);
		return spellEffectRadius;
	}

	public static RatioValue GetSpellDamage_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		RatioValue spellDamage = self.GetSpellDamage();
		spellDamage.AddRatio(PlayerMgr.Inst.ExtraDamageRatio).MulRatio(wand.passiveDamageRatio).MulRatio(wand.WandCfg.damageCorrection / 100f);
		RatioValue spellEffectRadius_FinalPlayerValue = self.GetSpellEffectRadius_FinalPlayerValue(wand);
		(float, float) spellDecreaseRadiuSpeedInfo = self.GetSpellDecreaseRadiuSpeedInfo();
		if (spellDecreaseRadiuSpeedInfo.Item1 < 1f && (spellEffectRadius_FinalPlayerValue.BaseValue > 0.0 || self.GetSpellFallExplosionRadius() > 0f))
		{
			float num = (float)spellEffectRadius_FinalPlayerValue.BaseValue + self.GetSpellFallExplosionRadius();
			spellDamage.MulRatio(1f + GeneralTool.GetSpellRadiusToDamageRatio(num * spellEffectRadius_FinalPlayerValue.CurrentAddRatioStartOne * spellEffectRadius_FinalPlayerValue.CurrentMulRatio, spellDecreaseRadiuSpeedInfo.Item1, spellDecreaseRadiuSpeedInfo.Item2));
		}
		if (wand.WandCfg.specialAbility == WandAbility.CheapSpellHigherDamage && self.GetSpellManaCost_FinalPlayerValue(wand).Result <= (float)wand.WandCfg.int1)
		{
			spellDamage.AddRatio(wand.WandCfg.float1 / 100f);
		}
		return spellDamage;
	}

	public static (int count, float radius)? GetSpellRefractionInfo_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		return self.GetSpellRefractionInfo();
	}

	public static RatioValue GetSpellManaCost_FinalPlayerValue(this SpellShootData self, Wand wand)
	{
		float num = 1f;
		if (wand != null && wand.WandCfg != null && wand.WandCfg.specialAbility == WandAbility.LowerSummonManaCost)
		{
			num *= wand.WandCfg.float1 / 100f;
		}
		RatioValue spellManaCost = self.GetSpellManaCost(num);
		if (wand != null && wand.WandCfg != null)
		{
			spellManaCost.MulRatio(wand.GetWandMpCorrection());
		}
		SpellConfig spellConfig = SpellConfig.dic[self.Spell.id];
		if (spellConfig.abilityType == SpellAbilityType.DragonBreath)
		{
			spellManaCost.BaseValue = spellConfig.float2 * self.GetSpellDuration_FinalPlayerValue(wand).Result;
		}
		if (spellConfig.abilityType == SpellAbilityType.MagicBreaker && PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor != null)
		{
			spellManaCost.MulRatio((float)PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor.RelicCfg.int1.result / 100f);
		}
		return spellManaCost;
	}

	public static float GetSpellManaCostAndSubGroupManaCost_FinalPlayerValue(this SpellShootData self, Wand wand, bool ignoreParentGroup)
	{
		SpellShootData ownerShootData = null;
		if (ignoreParentGroup)
		{
			ownerShootData = self.OwnerGroup.OwnerShootData;
			self.OwnerGroup.OwnerShootData = null;
		}
		float num = self.GetSpellManaCost_FinalPlayerValue(wand).Result;
		if (self.NeedCostSubGroupMana())
		{
			num += self.SubGroup.GetGroupManaCost_FinalPlayerValue(wand);
		}
		if (ignoreParentGroup)
		{
			self.OwnerGroup.OwnerShootData = ownerShootData;
		}
		return num;
	}

	public static IEnumerable<SlotData> GetAllSlotData(this SpellShootData self)
	{
		List<SlotData> result = new List<SlotData>();
		if (!result.Contains(self.Spell))
		{
			result.Add(self.Spell);
		}
		result.AddRange(self.EnhanceList.Where((SlotData e) => !result.Contains(e)));
		result.AddRange(self.Triggers.Where((SlotData e) => !result.Contains(e)));
		if (self.SubGroup != null)
		{
			result.AddRange(from e in self.SubGroup.GetAllSlotData()
				where !result.Contains(e)
				select e);
		}
		return result.ToArray();
	}

	public static IEnumerable<SlotData> GetCanShootSlotData(this SpellShootData self)
	{
		List<SlotData> list = new List<SlotData> { self.Spell };
		if (self.SubGroup != null)
		{
			list.AddRange(self.SubGroup.GetCanShootSlotData());
		}
		return list.ToArray();
	}

	[CanBeNull]
	public static SpellConfig GetMaxLevelVolley(this SpellShootData self)
	{
		SpellConfig[] array = (from e in self.EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.Volley
			select e).ToArray();
		if (array.Length == 0)
		{
			return null;
		}
		int maxLevel = array.Max((SpellConfig e) => e.level);
		return array.FirstOrDefault((SpellConfig e) => e.level == maxLevel);
	}
}
