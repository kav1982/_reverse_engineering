using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class SlotData
{
	private record ConfigCache
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ConfigCache);
			}
		}

		private int _id = -1;

		private SpellConfig _config;

		public SpellConfig Get(int id)
		{
			if (id == _id)
			{
				return _config;
			}
			_id = id;
			_config = SpellConfig.dic[_id];
			return _config;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ConfigCache");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			return false;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(_id)) * -1521134295 + EqualityComparer<SpellConfig>.Default.GetHashCode(_config);
		}

		[CompilerGenerated]
		public virtual bool Equals(ConfigCache? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(_id, other!._id))
				{
					return EqualityComparer<SpellConfig>.Default.Equals(_config, other!._config);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected ConfigCache(ConfigCache original)
		{
			_id = original._id;
			_config = original._config;
		}

		public ConfigCache()
		{
		}
	}

	public int id;

	public int specialInt;

	public bool isAllFieldSharedSpell;

	private readonly ConfigCache cache = new ConfigCache();

	private readonly ConfigCache cacheIgnoreMimic = new ConfigCache();

	public int mimicSpellID { get; set; }

	public SlotData sealSlotOwner { get; set; }

	public bool isSealSlot => sealSlotOwner != null;

	public int slotSpellExtraLevel { get; set; }

	public SlotData()
	{
	}

	public SlotData(int id)
	{
		this.id = id;
		specialInt = 0;
	}

	public SlotData(int id, int specialInt)
	{
		this.id = id;
		this.specialInt = specialInt;
	}

	public SlotData Copy()
	{
		return new SlotData(id, specialInt)
		{
			sealSlotOwner = sealSlotOwner,
			mimicSpellID = mimicSpellID,
			isAllFieldSharedSpell = isAllFieldSharedSpell,
			slotSpellExtraLevel = slotSpellExtraLevel
		};
	}

	public int GetFinalSlotCost()
	{
		if (GetFinalConfig().abilityType == SpellAbilityType.ManaTendril)
		{
			return specialInt + 1;
		}
		return GetFinalConfig().slotCost;
	}

	public int GetFinalId()
	{
		if (mimicSpellID != 0)
		{
			return GetFinalLevelId(mimicSpellID);
		}
		return GetFinalLevelId(id);
	}

	public SpellConfig GetFinalConfig()
	{
		return cache.Get(GetFinalId());
	}

	public SpellConfig GetConfigIgnoreMimic()
	{
		return cacheIgnoreMimic.Get(GetFinalLevelId(id));
	}

	public int GetFinalLevel()
	{
		if (mimicSpellID == 0)
		{
			return GetFinalLevelId(id) % 10;
		}
		int a = GetFinalLevelId(id) % 10;
		int b = GetFinalLevelId(mimicSpellID) % 10;
		return Mathf.Min(a, b);
	}

	public int GetLevelIgnoreMimic()
	{
		return GetFinalLevelId(id) % 10;
	}

	public int GetFinalLevelId(int id)
	{
		int num = (int)SpellConfig.dic[id].abilityType * 10;
		int num2 = Mathf.Min(GetSlotSpellMaxLevel(id), id % 10 + slotSpellExtraLevel);
		return num + num2;
	}

	public int GetSlotSpellMaxLevel(int id)
	{
		int num = (int)SpellConfig.dic[id].abilityType * 10;
		int i;
		for (i = 0; SpellConfig.dic.ContainsKey(num + i + 1) && i <= 5; i++)
		{
		}
		return i;
	}

	public string GetAdditionalInfo([CanBeNull] Wand wand)
	{
		SpellConfig configIgnoreMimic = GetConfigIgnoreMimic();
		SpellAbilityType abilityType = configIgnoreMimic.abilityType;
		StringBuilder stringBuilder = new StringBuilder();
		switch (abilityType)
		{
		case SpellAbilityType.DeathAdder:
			if (GetLevelIgnoreMimic() != 3)
			{
				stringBuilder.AppendStringField(1002013, specialInt + "/" + GetFinalConfig().int1, DataTextColorType.Grey, newLine: true, "◆\u00a0\u200a");
			}
			break;
		case SpellAbilityType.Summon6:
			stringBuilder.AppendStringField(1002035, "+" + GeneralTool.FloatToRetainDecimals(GetFinalConfig().float1 * (float)specialInt, 1) + "%", DataTextColorType.Grey, newLine: true, "◆\u00a0\u200a");
			break;
		case SpellAbilityType.Unyielding:
		case SpellAbilityType.WandSpirit:
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002709.GetText());
			break;
		case SpellAbilityType.Umbrella:
		case SpellAbilityType.RuneHammer:
		case SpellAbilityType.LaserBeam:
		case SpellAbilityType.BiAnLethalBlade:
		case SpellAbilityType.DaveHarpoons:
		case SpellAbilityType.RedRune:
		case SpellAbilityType.GreenRune:
		case SpellAbilityType.BlueRune:
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002037.GetText());
			break;
		}
		if ((object)wand != null && wand.WandCfg != null)
		{
			if (configIgnoreMimic.abilityType == SpellAbilityType.BiAnLethalBlade)
			{
				stringBuilder.AppendFloatField(1002023, wand.passiveBiAnBladeTotalCount, DataTextColorType.Default, newLine: true, "◆\u00a0\u200a");
			}
			switch (configIgnoreMimic.useType)
			{
			case SpellType.Enhance:
				if (!IsTrigger() && abilityType != SpellAbilityType.Mimic && abilityType != SpellAbilityType.SpellLevelEnhance)
				{
					stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002701.GetText());
				}
				break;
			case SpellType.Passive:
				stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002702.GetText());
				break;
			}
			switch (configIgnoreMimic.abilityType)
			{
			case SpellAbilityType.MrBingArrow:
				stringBuilder.AppendStringField(1002041, Spell1028MrBingArrow.shootCounter + "/" + SpellConfig.dic[10281].int1, DataTextColorType.Grey, newLine: true, "◆\u00a0\u200a");
				break;
			case SpellAbilityType.Summon5:
			{
				float summon5MPRecovery_FinalPlayerValue = CanShootSpellUtils.GetSummon5MPRecovery_FinalPlayerValue(this, wand);
				stringBuilder.AppendFloatField(14000203, summon5MPRecovery_FinalPlayerValue, DataTextColorType.Default, newLine: true, "◆\u00a0\u200a", null, "/s");
				break;
			}
			case SpellAbilityType.ManaCoin:
				stringBuilder.AppendStringField(1002006, (CanShootSpellUtils.GetManaCoinDamageRatio_FinalPlayerValue(configIgnoreMimic, PlayerMgr.Inst.CoinCount) * 100f).ToString("F0"), DataTextColorType.Default, newLine: true, "◆\u00a0\u200a");
				break;
			case SpellAbilityType.Summon2:
				if (configIgnoreMimic.float1 > 0f)
				{
					float mrKeHeadDamageRatioFromMana_FinalPlayerValue = CanShootSpellUtils.GetMrKeHeadDamageRatioFromMana_FinalPlayerValue(wand.GetShootDataByShootableSpell(this), wand);
					stringBuilder.AppendFloatField(1002006, mrKeHeadDamageRatioFromMana_FinalPlayerValue, DataTextColorType.Default, newLine: true, "◆\u00a0\u200a");
				}
				break;
			case SpellAbilityType.ManaTendril:
				stringBuilder.AppendStringField(1002036, " " + (100f + GetFinalConfig().float1 * (float)(specialInt + 1)).ToString("F0") + "%", DataTextColorType.Grey, newLine: true, "◆\u00a0\u200a");
				break;
			}
		}
		if (configIgnoreMimic.dropType == ItemDropType.Epic)
		{
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002705.GetText());
		}
		else if (configIgnoreMimic.dropType == ItemDropType.Special)
		{
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1002707.GetText());
		}
		if (TextConfig.TryGetText(configIgnoreMimic.id / 10 * 10 + 1 + 7300000, out var text))
		{
			stringBuilder.StartField();
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("<size=10>\n</size>");
			}
			string colorText = TextProcesser.GetColorText(text, DataTextColorType.Dark);
			stringBuilder.Append(colorText);
		}
		if (stringBuilder.Length == 0)
		{
			return "";
		}
		return TextProcesser.GetColorText(stringBuilder.ToString(), DataTextColorType.Grey);
	}

	public string GetInfoInPlayerWand([CanBeNull] Wand wand, string newLineStartAs = "◆\u00a0\u200a")
	{
		SpellConfig configIgnoreMimic = GetConfigIgnoreMimic();
		if (SpellInfoFormatters.HasFormatter(configIgnoreMimic.id, idIsIgnoreLevel: false))
		{
			return this.FormatInfo(new SpellInfoFormatters.Param
			{
				Wand = wand,
				PlayerEffect = true
			});
		}
		if ((object)wand == null || wand.WandCfg == null)
		{
			Debug.LogError("不是法杖中的 SlotData 不应该走这个方法！！！应该直接调用 SpellConfig 的 GetInfo。");
			return configIgnoreMimic.GetInfo(1f, "◆\u00a0\u200a");
		}
		WandConfig wandCfg = wand.WandCfg;
		if (!wandCfg.GetValidSlotsData(normal: true, post: true).Contains(this))
		{
			Debug.LogError("法杖不包含 这个 SlotData。");
			return configIgnoreMimic.GetInfo(1f, "◆\u00a0\u200a");
		}
		SpellType useType = configIgnoreMimic.useType;
		if (useType != 0 && useType != SpellType.Summon)
		{
			SlotData[] source = (wandCfg.postSlots.Contains(this) ? wandCfg.GetValidSlotsData(normal: false, post: true) : wandCfg.GetValidSlotsData(normal: true, post: false));
			RatioValue damage_FinalPlayerValue = CanShootSpellUtils.GetDamage_FinalPlayerValue(GetFinalConfig().damage, source.Select((SlotData e) => e.GetFinalConfig()).ToArray(), GetFinalConfig(), wand);
			return configIgnoreMimic.GetInfo(damage_FinalPlayerValue.CurrentFinalRatio, "◆\u00a0\u200a");
		}
		SpellShootData shootDataByShootableSpell = wand.GetShootDataByShootableSpell(this);
		if (shootDataByShootableSpell == null)
		{
			Debug.LogWarning("SLotData 中获取 ShootData 失败。提前返回 Info 信息。");
			return configIgnoreMimic.GetInfo(1f, "◆\u00a0\u200a");
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (configIgnoreMimic.summonLimit > 0)
		{
			int summonLimit_FinalPlayerValue = shootDataByShootableSpell.GetSummonLimit_FinalPlayerValue(wand);
			stringBuilder.AppendIntField(14000304, summonLimit_FinalPlayerValue, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.summonLimit, summonLimit_FinalPlayerValue), newLine: true, newLineStartAs);
		}
		if (configIgnoreMimic.useType == SpellType.Summon)
		{
			int resultCeilToInt = shootDataByShootableSpell.GetSummonHpAmount_FinalPlayerValue(wand).ResultCeilToInt;
			float maxHP = UnitConfig.map[configIgnoreMimic.summonID].maxHP;
			stringBuilder.AppendIntField(14000314, resultCeilToInt, SpellInfoExtend.ColorByValue_BigGood(maxHP, resultCeilToInt), newLine: true, newLineStartAs);
		}
		if (configIgnoreMimic.damage != 0f)
		{
			RatioValue spellDamage_FinalPlayerValue = shootDataByShootableSpell.GetSpellDamage_FinalPlayerValue(wand);
			if ((object)wand != null && wand.WandCfg != null)
			{
				switch (configIgnoreMimic.abilityType)
				{
				case SpellAbilityType.Summon1:
				{
					(float, float) spellDecreaseRadiuSpeedInfo = shootDataByShootableSpell.GetSpellDecreaseRadiuSpeedInfo();
					if (spellDecreaseRadiuSpeedInfo.Item1 < 1f && CanShootSpellUtils.GetSummonAdvanceSkillType1Level_FinalPlayerValue(shootDataByShootableSpell, wand) > 0)
					{
						spellDamage_FinalPlayerValue.MulRatio(1f + GeneralTool.GetSpellRadiusToDamageRatio(2.2f * shootDataByShootableSpell.GetSpellEffectRadius_FinalPlayerValue(wand).CurrentFinalRatio, spellDecreaseRadiuSpeedInfo.Item1, spellDecreaseRadiuSpeedInfo.Item2));
					}
					break;
				}
				case SpellAbilityType.Summon2:
					if (configIgnoreMimic.float1 > 0f)
					{
						float num = CanShootSpellUtils.GetMrKeHeadDamageRatioFromMana_FinalPlayerValue(shootDataByShootableSpell, wand) / 100f;
						spellDamage_FinalPlayerValue.BaseValue += num * 100f;
					}
					break;
				case SpellAbilityType.ManaCoin:
				{
					float num2 = CanShootSpellUtils.GetManaCoinDamageRatio_FinalPlayerValue(configIgnoreMimic, PlayerMgr.Inst.CoinCount) * 100f;
					spellDamage_FinalPlayerValue.AddBase(num2);
					break;
				}
				case SpellAbilityType.Harpoons:
					if (PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead != null)
					{
						spellDamage_FinalPlayerValue.MulRatio(1f + (float)PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead.int1.result / 100f);
					}
					if (PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead != null)
					{
						spellDamage_FinalPlayerValue.MulRatio(1f + (float)PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead.int1.result / 100f);
					}
					break;
				}
			}
			stringBuilder.AppendStringField(14000301, spellDamage_FinalPlayerValue.Result.ToStringDamage(), SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.damage, spellDamage_FinalPlayerValue.Result), newLine: true, newLineStartAs);
			if (configIgnoreMimic.isDPS)
			{
				stringBuilder.AppendLink(" (", 14000302.GetText(), ")");
			}
			if (configIgnoreMimic.abilityType == SpellAbilityType.DragonBreath)
			{
				if (DataMgr.settingData.language == LanguageType.English || DataMgr.settingData.language == LanguageType.Spanish_spain || DataMgr.settingData.language == LanguageType.German || DataMgr.settingData.language == LanguageType.Russian)
				{
					stringBuilder.AppendLink(", ", "+" + configIgnoreMimic.float3 + "% ", 1002018.GetText());
				}
				else
				{
					stringBuilder.AppendLink(", ", 1002018.GetText(), configIgnoreMimic.float3 + "%");
				}
			}
		}
		int num3 = configIgnoreMimic.shootCount * shootDataByShootableSpell.GetSpellMultiShootData().count;
		if (num3 > 1 || configIgnoreMimic.shootCount > 1)
		{
			stringBuilder.AppendIntField(14000312, num3, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.shootCount, num3), newLine: true, newLineStartAs);
		}
		if (configIgnoreMimic.shootIntervalAddSubRevise != 0f)
		{
			string valuePrefix = ((configIgnoreMimic.shootIntervalAddSubRevise > 0f) ? "+" : null);
			string text = 1001101.GetText();
			stringBuilder.AppendFloatField(14000204, configIgnoreMimic.shootIntervalAddSubRevise, DataTextColorType.Default, newLine: true, newLineStartAs, valuePrefix, text);
		}
		if (configIgnoreMimic.coolDownAddSubRevise != 0f)
		{
			string valuePrefix2 = ((configIgnoreMimic.coolDownAddSubRevise > 0f) ? "+" : null);
			string text2 = 1001101.GetText();
			stringBuilder.AppendFloatField(14000205, configIgnoreMimic.coolDownAddSubRevise, DataTextColorType.Default, newLine: true, newLineStartAs, valuePrefix2, text2);
		}
		if (Math.Abs(configIgnoreMimic.coolDownRatio - 1f) > 0.01f)
		{
			stringBuilder.AppendFloatField(14000205, configIgnoreMimic.coolDownRatio * 100f, DataTextColorType.Default, newLine: true, newLineStartAs, "×", "%");
		}
		float num4 = shootDataByShootableSpell.GetSpellCriticalChance_FinalPlayerValue(wand).Result * 100f;
		if (configIgnoreMimic.criticalChance != 0f || num4 != 0f)
		{
			stringBuilder.AppendFloatField(14000307, num4, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.criticalChance, num4), newLine: true, newLineStartAs, null, "%");
		}
		float result = shootDataByShootableSpell.GetSpellShootScatter_FinalPlayerValue(wand).Result;
		result = Mathf.Max(0f, result);
		if (configIgnoreMimic.angle != 0f || result != 0f)
		{
			stringBuilder.AppendFloatField(14000206, result, SpellInfoExtend.ColorByValue_LowGood(configIgnoreMimic.angle, result), newLine: true, newLineStartAs, null, "°");
		}
		if (configIgnoreMimic.mpCostAddSubCorrection != 0f)
		{
			string valuePrefix3 = null;
			if (configIgnoreMimic.mpCostAddSubCorrection > 0f)
			{
				valuePrefix3 = "+";
			}
			stringBuilder.AppendFloatField(14000305, configIgnoreMimic.mpCostAddSubCorrection, DataTextColorType.Default, newLine: true, newLineStartAs, valuePrefix3, "%");
		}
		if (Math.Abs(configIgnoreMimic.mpCostMulDivCorrection) > 0.01f)
		{
			stringBuilder.AppendFloatField(14000305, configIgnoreMimic.mpCostMulDivCorrection, DataTextColorType.Default, newLine: true, newLineStartAs, "×", "%");
		}
		SpellAbilityType abilityType = configIgnoreMimic.abilityType;
		if (abilityType != SpellAbilityType.DragonBreath && abilityType != SpellAbilityType.SuperNova && (configIgnoreMimic.radius != 0f || shootDataByShootableSpell.GetSpellFallExplosionRadius() > 0f))
		{
			float num5 = shootDataByShootableSpell.GetSpellEffectRadius_FinalPlayerValue(wand).Result + shootDataByShootableSpell.GetSpellFallExplosionRadius() * (shootDataByShootableSpell.GetSpellEffectRadius().CurrentAddRatioStartOne + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: true)) * shootDataByShootableSpell.GetSpellEffectRadius().CurrentMulRatio;
			stringBuilder.AppendFloatField(14000303, num5, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.radius, num5), newLine: true, newLineStartAs, null, GameMgr.IsHarmony_Static ? "米" : "m");
		}
		if (configIgnoreMimic.slotCost > 1)
		{
			stringBuilder.AppendIntField(14000310, configIgnoreMimic.slotCost, DataTextColorType.Default, newLine: true, newLineStartAs);
		}
		if (configIgnoreMimic.slotNumModifyValue > 0)
		{
			stringBuilder.AppendFloatField(14000311, configIgnoreMimic.slotNumModifyValue, DataTextColorType.Default, newLine: true, newLineStartAs, "+");
		}
		else if (configIgnoreMimic.slotNumModifyValue < 0)
		{
			stringBuilder.AppendFloatField(14000311, configIgnoreMimic.slotNumModifyValue, DataTextColorType.Default, newLine: true, newLineStartAs);
		}
		return stringBuilder.ToString().Trim();
	}

	public string GetDesInPlayerWand([CanBeNull] Wand wand, string nextLineAdd = "", string color = "", string header = "")
	{
		SpellConfig configIgnoreMimic = GetConfigIgnoreMimic();
		if (SpellInfoFormatters.HasFormatter(configIgnoreMimic.id, idIsIgnoreLevel: false))
		{
			return "";
		}
		if ((object)wand == null || wand.WandCfg == null)
		{
			Debug.LogError("不是法杖中的 SlotData 不应该走这个方法！！！应该直接调用 SpellConfig 的 GetInfo。");
			return "";
		}
		WandConfig wandCfg = wand.WandCfg;
		if (!wandCfg.GetValidSlotsData(normal: true, post: true).Contains(this))
		{
			Debug.LogError("法杖不包含 这个 SlotData。");
			return "";
		}
		SpellType useType = configIgnoreMimic.useType;
		if (useType != 0 && useType != SpellType.Summon)
		{
			SlotData[] source = (wandCfg.postSlots.Contains(this) ? wandCfg.GetValidSlotsData(normal: false, post: true) : wandCfg.GetValidSlotsData(normal: true, post: false));
			RatioValue damage_FinalPlayerValue = CanShootSpellUtils.GetDamage_FinalPlayerValue(GetFinalConfig().damage, source.Select((SlotData e) => e.GetFinalConfig()).ToArray(), GetFinalConfig(), wand);
			return configIgnoreMimic.GetDes(damage_FinalPlayerValue.CurrentFinalRatio, "◆\u00a0\u200a", "", "◆\u00a0\u200a");
		}
		SpellShootData shootDataByShootableSpell = wand.GetShootDataByShootableSpell(this);
		if (shootDataByShootableSpell == null)
		{
			Debug.LogWarning("SLotData 中获取 ShootData 失败。提前返回 Info 信息。");
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder((GetFinalLevelId(id) + 7100000).GetText());
		bool flag = true;
		if (stringBuilder2.ToString() != "")
		{
			flag = false;
			stringBuilder2.Insert(0, color + header);
			switch (configIgnoreMimic.abilityType)
			{
			case SpellAbilityType.DimensionTraveller:
			{
				string newValue = GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float3, 2);
				stringBuilder2 = stringBuilder2.Replace("float3", newValue);
				break;
			}
			case SpellAbilityType.Summon7:
			{
				float currentFinalRatio = shootDataByShootableSpell.GetSummonAttackSpeedRatio_FinalPlayerValue(wand).CurrentFinalRatio;
				string colorText = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float2 / currentFinalRatio, 1), TextProcesser.GetColor_BigIsGood(1f, currentFinalRatio));
				stringBuilder2 = stringBuilder2.Replace("float2", colorText);
				break;
			}
			}
			stringBuilder2 = stringBuilder2.Replace("int1", configIgnoreMimic.int1.ToString());
			stringBuilder2 = stringBuilder2.Replace("int2", configIgnoreMimic.int2.ToString());
			stringBuilder2 = stringBuilder2.Replace("int3", configIgnoreMimic.int3.ToString());
			stringBuilder2 = stringBuilder2.Replace("float1", GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float1, 1));
			stringBuilder2 = stringBuilder2.Replace("float2", GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float2, 1));
			stringBuilder2 = stringBuilder2.Replace("float3", GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float3, 1));
			stringBuilder2 = stringBuilder2.Replace("shootCount", configIgnoreMimic.shootCount.ToString());
			if (stringBuilder2.ToString().IndexOf("spellDuration", StringComparison.Ordinal) >= 0)
			{
				float result = shootDataByShootableSpell.GetSpellDuration().Result;
				float duration = configIgnoreMimic.duration;
				string colorText2 = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(result, 1), TextProcesser.GetColor_BigIsGood(duration, result));
				stringBuilder2 = stringBuilder2.Replace("spellDuration", colorText2);
			}
			if (configIgnoreMimic.abilityType == SpellAbilityType.DragonBreath)
			{
				float currentFinalRatio2 = shootDataByShootableSpell.GetSpellManaCost_FinalPlayerValue(wand).CurrentFinalRatio;
				string colorText3 = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(configIgnoreMimic.float2 * currentFinalRatio2, 1), TextProcesser.GetColor_SmallIsGood(1f, currentFinalRatio2));
				stringBuilder2 = stringBuilder2.Replace("DragonBreathSpecialDescription", colorText3);
			}
			stringBuilder2 = stringBuilder2.Replace("\\", "\n" + nextLineAdd);
			stringBuilder.StartField().Append(stringBuilder2);
		}
		if (configIgnoreMimic.useType == SpellType.Summon && CanShootSpellUtils.GetSummonSpiritEssence_FinalPlayerValue(this, wand) > 0)
		{
			if (flag)
			{
				flag = false;
				stringBuilder.StartField(newLine: true, color + header);
			}
			else
			{
				stringBuilder.StartField(newLine: true, header);
			}
			int summonSpiritEssence_FinalPlayerValue = CanShootSpellUtils.GetSummonSpiritEssence_FinalPlayerValue(this, wand);
			switch (configIgnoreMimic.abilityType)
			{
			case SpellAbilityType.Summon1:
			{
				StringBuilder stringBuilder8 = new StringBuilder(1003700.GetText() + 1003701.GetText());
				float num7 = (0.2f + 0.1f * (float)summonSpiritEssence_FinalPlayerValue) * 100f;
				string newValue3 = GeneralTool.FloatToRetainDecimals(5f + 15f * (float)summonSpiritEssence_FinalPlayerValue, 2);
				stringBuilder8.Replace("bombchance", num7.ToString());
				stringBuilder8.Replace("bombDamage", newValue3);
				stringBuilder.Append(stringBuilder8);
				break;
			}
			case SpellAbilityType.Summon2:
			{
				StringBuilder stringBuilder9 = new StringBuilder(1003700.GetText() + 1003702.GetText());
				stringBuilder9.Replace("enhanceratio", (summonSpiritEssence_FinalPlayerValue * 2).ToString());
				stringBuilder9.Replace("DamageRatio", 60f.ToString());
				stringBuilder.Append(stringBuilder9);
				break;
			}
			case SpellAbilityType.Summon3:
			{
				StringBuilder stringBuilder7 = new StringBuilder(1003700.GetText());
				int num5 = 4 - summonSpiritEssence_FinalPlayerValue;
				float num6 = 20f * (float)summonSpiritEssence_FinalPlayerValue;
				if (num5 > 0)
				{
					stringBuilder7.Append(1003703.GetText());
					stringBuilder7.Replace("attackcount", num5.ToString());
				}
				stringBuilder7.Append(1003704.GetText());
				stringBuilder7.Replace("attackspeed", num6.ToString());
				stringBuilder7.Replace("damageRatio", "70");
				stringBuilder.Append(stringBuilder7);
				break;
			}
			case SpellAbilityType.Summon4:
			{
				SpellShootData shootDataByShootableSpell3 = wand.GetShootDataByShootableSpell(this);
				string text = GeneralTool.FloatToRetainDecimals(2.5f / shootDataByShootableSpell3.GetSummonAttackSpeedRatio_FinalPlayerValue(wand).CurrentFinalRatio, 2);
				string text2 = GeneralTool.FloatToRetainDecimals(1.5f * shootDataByShootableSpell3.GetSpellEffectRadius_FinalPlayerValue(wand).CurrentFinalRatio, 2);
				float num4 = 100f * (1f + (float)(summonSpiritEssence_FinalPlayerValue - 1) / 2f);
				StringBuilder stringBuilder6 = new StringBuilder(1003700.GetText() + 1003705.GetText());
				stringBuilder6.Replace("time", text.ToString());
				stringBuilder6.Replace("effectradiu", text2.ToString());
				stringBuilder6.Replace("HpDamageRatio", num4.ToString());
				stringBuilder.Append(stringBuilder6);
				break;
			}
			case SpellAbilityType.Summon5:
			{
				float num2 = Mathf.Max(0.6f, 3f - (float)(summonSpiritEssence_FinalPlayerValue - 1) * 0.5f);
				int num3 = 1 + summonSpiritEssence_FinalPlayerValue * 4;
				StringBuilder stringBuilder5 = new StringBuilder(1003700.GetText() + 1003706.GetText());
				stringBuilder5.Replace("cooldown", num2.ToString());
				stringBuilder5.Replace("shootCount", num3.ToString());
				stringBuilder.Append(stringBuilder5);
				break;
			}
			case SpellAbilityType.Summon6:
			{
				StringBuilder stringBuilder4 = new StringBuilder(1003700.GetText() + 1003707.GetText());
				stringBuilder4.Replace("BackUpAmmoCount", (summonSpiritEssence_FinalPlayerValue * 4).ToString());
				stringBuilder.Append(stringBuilder4);
				break;
			}
			case SpellAbilityType.Summon7:
			{
				StringBuilder stringBuilder3 = new StringBuilder(1003700.GetText() + 1003708.GetText());
				SpellShootData shootDataByShootableSpell2 = wand.GetShootDataByShootableSpell(this);
				string newValue2 = GeneralTool.FloatToRetainDecimals(3.5f * shootDataByShootableSpell2.GetSpellEffectRadius_FinalPlayerValue(wand).CurrentFinalRatio, 2);
				float num = 180f * (float)summonSpiritEssence_FinalPlayerValue;
				stringBuilder3.Replace("attackRadiu", newValue2);
				stringBuilder3.Replace("hpdamageratio", num.ToString());
				stringBuilder.Append(stringBuilder3);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (color != "" && !flag)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString().Trim();
	}

	public static int GetMimicSpellLevel(int mimicID, int targetSpellID)
	{
		int num = mimicID % 10;
		int num2 = targetSpellID % 10;
		if (num2 > num)
		{
			return targetSpellID - num2 + num;
		}
		return targetSpellID;
	}

	public bool IsTrigger()
	{
		SpellAbilityType abilityType = GetFinalConfig().abilityType;
		return abilityType == SpellAbilityType.OnOverTrigger || abilityType == SpellAbilityType.OnOverSplitTrigger || abilityType == SpellAbilityType.OnMoveTrigger || abilityType == SpellAbilityType.OnStartRotationTrigger || abilityType == SpellAbilityType.OnHitTrigger;
	}

	public bool IsPostSlotExtender()
	{
		SpellAbilityType abilityType = GetFinalConfig().abilityType;
		if (abilityType != SpellAbilityType.PostSlotExtenderMove && abilityType != SpellAbilityType.PostSlotExtenderCastSpell && abilityType != SpellAbilityType.PostSlotExtenderTime)
		{
			return abilityType == SpellAbilityType.PostSlotExtenderStand;
		}
		return true;
	}

	public bool HindInSubGroup()
	{
		return GetFinalConfig().abilityType == SpellAbilityType.Multishot;
	}

	public void OnWillLeaveSlots(SlotData[] slots, bool[] locks, bool isBag)
	{
		if (!isBag && GetFinalConfig().abilityType == SpellAbilityType.ManaTendril)
		{
			slots.Bag_ClearSeal(Array.IndexOf(slots, this));
			specialInt = 0;
		}
	}

	public bool CheckCanOverrideInSlots(SlotData[] slots, bool[] locks, int targetIndex, bool isBag)
	{
		if (!isBag && GetFinalConfig().abilityType == SpellAbilityType.ManaTendril && slots[targetIndex] != this)
		{
			return true;
		}
		return false;
	}

	public void OverrideInSlots(SlotData[] slots, bool[] locks, int targetIndex, bool isBag)
	{
		if (!isBag && GetFinalConfig().abilityType == SpellAbilityType.ManaTendril)
		{
			slots.Bag_ClearSeal(targetIndex - 1);
			int num = Array.IndexOf(slots, this);
			specialInt = targetIndex - num - 1;
		}
	}
}
