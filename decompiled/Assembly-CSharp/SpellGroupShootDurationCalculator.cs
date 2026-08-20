using UnityEngine;

public static class SpellGroupShootDurationCalculator
{
	public static float GetMaxCastDuration(SpellShootGroup targetGroup, Wand targetWand)
	{
		return Mathf.Max(GetMaxChargeCastDuration(targetGroup, targetWand), GetMaxKeepCastSpellDuration(targetGroup, targetWand));
	}

	public static float GetMaxChargeCastDuration(SpellShootGroup targetGroup, Wand targetWand)
	{
		if (targetGroup == null)
		{
			Debug.LogError("传入了空的法术组");
			return 0f;
		}
		float num = 0f;
		bool flag = targetWand == null || targetWand.WandCfg == null;
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			float num2 = 0f;
			RatioValue ratioValue = (flag ? spellShootData.GetSpellCriticalChance() : spellShootData.GetSpellCriticalChance_FinalPlayerValue(targetWand));
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			num = Mathf.Max(finalConfig.abilityType switch
			{
				SpellAbilityType.ShiningStar => Mathf.Max(0f, (finalConfig.float3 / 100f - ratioValue.Result) / (finalConfig.float1 / 100f)), 
				SpellAbilityType.SuperNova => finalConfig.float2, 
				_ => 0f, 
			}, num);
		}
		return num;
	}

	public static float GetMaxKeepCastSpellDuration(SpellShootGroup targetGroup, Wand targetWand)
	{
		if (targetGroup == null)
		{
			Debug.LogError("传入了空的法术组");
			return 0f;
		}
		float num = 0f;
		bool flag = targetWand == null || targetWand.WandCfg == null;
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			float num2 = 0f;
			RatioValue ratioValue = (flag ? spellShootData.GetSpellDuration() : spellShootData.GetSpellDuration_FinalPlayerValue(targetWand));
			if (!flag)
			{
				spellShootData.GetSpellCriticalChance_FinalPlayerValue(targetWand);
			}
			else
			{
				spellShootData.GetSpellCriticalChance();
			}
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			float num3 = 0f;
			bool flag2 = false;
			for (int j = 0; j < spellShootData.EnhanceList.Length; j++)
			{
				SpellConfig finalConfig2 = spellShootData.EnhanceList[j].GetFinalConfig();
				switch (finalConfig2.abilityType)
				{
				case SpellAbilityType.SpellHover:
					num3 += finalConfig2.float1;
					break;
				case SpellAbilityType.Fall:
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				switch (finalConfig.abilityType)
				{
				case SpellAbilityType.DisintegrationRay:
				case SpellAbilityType.HighPressureWasher:
				case SpellAbilityType.DragonBreath:
					num2 = ratioValue.Result;
					break;
				case SpellAbilityType.Dash:
					num2 = ratioValue.Result + num3;
					break;
				default:
					num2 = 0f;
					break;
				}
				num = Mathf.Max(num2, num);
			}
		}
		return num;
	}
}
