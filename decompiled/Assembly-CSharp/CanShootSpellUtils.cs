using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CanShootSpellUtils
{
	public static RatioValue GetDamage(float baseDamage, IEnumerable<SpellConfig> enhances, SpellConfig stopTarget)
	{
		RatioValue ratioValue = new RatioValue(baseDamage);
		foreach (SpellConfig enhance in enhances)
		{
			if (enhance != stopTarget)
			{
				switch (enhance.abilityType)
				{
				case SpellAbilityType.EnhanceAttackRatio:
				case SpellAbilityType.FireCrystal:
				case SpellAbilityType.OverDrive:
					ratioValue.AddRatio(enhance.float1 / 100f);
					break;
				case SpellAbilityType.Refraction:
					ratioValue.MulRatio(enhance.float2 / 100f);
					break;
				case SpellAbilityType.PowerSavingMode:
					ratioValue.MulRatio(enhance.float3 / 100f);
					break;
				}
				continue;
			}
			return ratioValue;
		}
		return ratioValue;
	}

	public static RatioValue GetDamage_FinalPlayerValue(float baseDamage, SpellConfig[] enhances, SpellConfig stopTarget, Wand wand)
	{
		RatioValue damage = GetDamage(baseDamage, enhances, stopTarget);
		damage.AddRatio(PlayerMgr.Inst.ExtraDamageRatio).MulRatio(wand.passiveDamageRatio).MulRatio(wand.WandCfg.damageCorrection / 100f);
		return damage;
	}

	public static float GetManaCoinDamageRatio_FinalPlayerValue(SpellConfig spell, int totalCoinCount)
	{
		if (spell.abilityType != SpellAbilityType.ManaCoin)
		{
			Debug.LogError("不是金币弹不要调用这个方法！");
			return 0f;
		}
		return spell.float2 * (float)Mathf.CeilToInt((float)totalCoinCount * spell.float1 / 100f) / 100f;
	}

	public static float GetSummon5MPRecovery_FinalPlayerValue(SlotData spell, Wand wand)
	{
		SpellShootData shootDataByShootableSpell = wand.GetShootDataByShootableSpell(spell);
		if (shootDataByShootableSpell == null)
		{
			return 0f;
		}
		RatioValue summonAttackSpeedRatio_FinalPlayerValue = shootDataByShootableSpell.GetSummonAttackSpeedRatio_FinalPlayerValue(wand);
		float num = spell.GetFinalConfig().float1 / 100f * summonAttackSpeedRatio_FinalPlayerValue.CurrentFinalRatio;
		return wand.GetWandMpRecoverSpeed() * num;
	}

	public static int GetSummonSpiritEssence_FinalPlayerValue(SlotData spell, Wand wand)
	{
		return wand.GetShootDataByShootableSpell(spell)?.GetSummonSpiritEssenceLevel() ?? 0;
	}

	public static int GetLightningChainDamage_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		SpellConfig[] array = shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		float num = 0f;
		foreach (SpellConfig item in array.Where((SpellConfig e) => e.abilityType == SpellAbilityType.LightningChain))
		{
			num += GetDamage_FinalPlayerValue(item.int1, array, item, wand).Result;
		}
		return Mathf.CeilToInt(num);
	}

	public static (int damage, float Interval) GetLifeLineDamage_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		SpellConfig[] array = shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		IEnumerable<SpellConfig> enumerable = array.Where((SpellConfig e) => e.abilityType == SpellAbilityType.LifeLine);
		float num = 0f;
		float num2 = 0f;
		foreach (SpellConfig item in enumerable)
		{
			num += GetDamage_FinalPlayerValue((int)item.float1, array, item, wand).Result;
			num2 = item.float2;
		}
		return (Mathf.CeilToInt(num * num2), num2);
	}

	public static (int maxLevel, float buffRatio) GetFuseSummonData_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		IEnumerable<SpellConfig> enumerable = from e in shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray()
			where e.abilityType == SpellAbilityType.FusionSummon
			select e;
		int num = 0;
		float item = 0f;
		foreach (SpellConfig item2 in enumerable)
		{
			num += item2.int1;
			item = item2.float1 / 100f;
		}
		return (num, item);
	}

	public static int GetSummonAdvanceSkillType1Level_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		IEnumerable<SpellConfig> enumerable = from e in shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray()
			where e.abilityType == SpellAbilityType.TeammateSprite
			select e;
		int num = 0;
		foreach (SpellConfig item in enumerable)
		{
			num += item.int1;
		}
		return num;
	}

	public static float GetSummonThroughMapChance(SpellShootData shootData, Wand wand)
	{
		SpellConfig[] array = shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		float num = 0f;
		SpellConfig[] array2 = array;
		foreach (SpellConfig spellConfig in array2)
		{
			if (spellConfig.abilityType == SpellAbilityType.SoulMate)
			{
				num += spellConfig.float1 / 100f;
			}
		}
		return num;
	}

	public static float GetSummonGainOwnerHpRatio(SpellShootData shootData, Wand wand)
	{
		SpellConfig[] array = shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		float num = 0f;
		SpellConfig[] array2 = array;
		foreach (SpellConfig spellConfig in array2)
		{
			if (spellConfig.abilityType == SpellAbilityType.SoulMate)
			{
				num += spellConfig.float2 / 100f;
			}
		}
		return num;
	}

	public static (bool reverseState, float reverseBonusSpeed) GetReverseShootState(SpellShootData shootData, Wand wand, bool reverseState)
	{
		SpellConfig[] array = shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		float num = 0f;
		SpellConfig[] array2 = array;
		foreach (SpellConfig spellConfig in array2)
		{
			if (spellConfig.abilityType == SpellAbilityType.ReverseCast)
			{
				reverseState = !reverseState;
				num += spellConfig.float1;
			}
		}
		return (reverseState, num);
	}

	public static (int SummonHpDropPerSecond, int parasiteCount) GetParasiteWormEffect_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		float num = 0f;
		int num2 = 0;
		SpellConfig[] array = (from e in shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray()
			where e.abilityType == SpellAbilityType.ParasiticWorm
			select e).ToArray();
		foreach (SpellConfig spellConfig in array)
		{
			num += spellConfig.float1;
			num2 += spellConfig.int2;
		}
		return (Mathf.CeilToInt(num), num2);
	}

	public static (float InstantDeathHpPercent, float ExplodeRange, float ExplodeHpDamageRatio) GetSelfSacrifice_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		float item = 0f;
		float num = 0f;
		float num2 = 0f;
		SpellConfig[] array = (from e in shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray()
			where e.abilityType == SpellAbilityType.TeammateSacrifice
			select e).ToArray();
		foreach (SpellConfig spellConfig in array)
		{
			item = (float)spellConfig.int1 / 100f;
			num = Mathf.Max(num, spellConfig.float2);
			num2 += spellConfig.float3 / 100f;
		}
		return (item, num, num2);
	}

	public static float GetTeammateImmuteDeath_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		float num = 0f;
		SpellConfig[] array = (from e in shootData.EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray()
			where e.abilityType == SpellAbilityType.Unyielding
			select e).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			num = Mathf.Max(array[i].float1, num);
		}
		return num;
	}

	public static float GetMrKeHeadDamageRatioFromMana_FinalPlayerValue(SpellShootData shootData, Wand wand)
	{
		SpellConfig finalConfig = shootData.Spell.GetFinalConfig();
		shootData.OwnerGroup.GetGroupManaCost_FinalPlayerValue(wand);
		return wand.MaxMP * finalConfig.float1 / 100f;
	}
}
