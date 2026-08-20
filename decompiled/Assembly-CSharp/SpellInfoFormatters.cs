using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public static class SpellInfoFormatters
{
	public struct Param
	{
		public bool PlayerEffect;

		[CanBeNull]
		public Wand Wand;

		public bool WithDetailInfo;
	}

	private static Dictionary<int, Func<SlotData, Param, string>> _formatters;

	private static Dictionary<int, Func<SlotData, Param, string>> Formatters
	{
		get
		{
			if (_formatters == null)
			{
				_formatters = new Dictionary<int, Func<SlotData, Param, string>>();
				InitFormatters();
			}
			return _formatters;
		}
	}

	public static void SetFormatter(int ignoreLevelId, Func<SlotData, Param, string> formatter)
	{
		Formatters[ignoreLevelId] = formatter;
	}

	public static bool HasFormatter(int id, bool idIsIgnoreLevel)
	{
		if (!idIsIgnoreLevel)
		{
			id /= 10;
		}
		return Formatters.ContainsKey(id);
	}

	public static string FormatInfo(this SlotData info, Param param = default(Param), bool withDetailInfo = true)
	{
		int num = info.id / 10;
		param.WithDetailInfo = withDetailInfo;
		if (!HasFormatter(num, idIsIgnoreLevel: true))
		{
			return "";
		}
		return Formatters[num](info, param);
	}

	private static void InitFormatters()
	{
		SetFormatter(4012, delegate(SlotData data, Param param)
		{
			UmbrellaShieldController component = PlayerMgr.Inst.PlayerPpt.GetComponent<UmbrellaShieldController>();
			component.UpdateShieldBuffState(param.Wand);
			SpellConfig configIgnoreMimic7 = data.GetConfigIgnoreMimic();
			StringBuilder stringBuilder4 = new StringBuilder();
			stringBuilder4.AppendSpellFieldSlotCost(configIgnoreMimic7.slotCost);
			StringBuilder stringBuilder5 = GetSpellDescAutoReplaceNewLine(configIgnoreMimic7.id).Replace("int2", configIgnoreMimic7.int2.ToString());
			stringBuilder5.Replace("float2", configIgnoreMimic7.float2.ToString("F0"));
			stringBuilder5.Insert(0, "◆\u00a0\u200a");
			float radiusRatio = component.GetRadiusRatio();
			string newValue = TextProcesser.GetColorText(type: TextProcesser.GetColor_BigIsGood(1f, radiusRatio), source: GeneralTool.FloatToRetainDecimals(configIgnoreMimic7.float3 * radiusRatio, 2));
			stringBuilder5.Replace("float3", newValue);
			float damageRatio = component.GetDamageRatio();
			string newValue2 = TextProcesser.GetColorText(type: TextProcesser.GetColor_BigIsGood(1f, damageRatio), source: GeneralTool.FloatToRetainDecimals((float)configIgnoreMimic7.int1 * damageRatio, 2));
			stringBuilder5.Replace("int1", newValue2);
			stringBuilder4.StartField().Append(stringBuilder5);
			float num18 = component.GetCriticalChance() * 100f;
			if (configIgnoreMimic7.criticalChance != 0f || num18 != 0f)
			{
				stringBuilder4.AppendFloatField(14000307, num18, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic7.criticalChance, num18), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			return stringBuilder4.ToString();
		});
		SetFormatter(4013, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic6 = data.GetConfigIgnoreMimic();
			StringBuilder stringBuilder3 = new StringBuilder();
			float num14 = 1f;
			SpellInitialParameter spellInitialParameter6 = new SpellInitialParameter();
			if ((bool)param.Wand)
			{
				spellInitialParameter6 = param.Wand.GetApplyWandAllEnhanceEffectSIP(40131, calculateSplitAsNormalEffect: true);
			}
			if (param.PlayerEffect)
			{
				num14 *= (1f + spellInitialParameter6.extraDamageRatio) * spellInitialParameter6.finalDamageRatio;
			}
			float num15 = Mathf.Ceil((float)configIgnoreMimic6.int1 * num14);
			stringBuilder3.AppendStringField(14000301, num15.ToStringDamage(), SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic6.int1, num15), newLine: false, "◆\u00a0\u200a");
			float num16 = spellInitialParameter6.extraCriticalChance * 100f;
			if (configIgnoreMimic6.criticalChance != 0f || num16 != 0f)
			{
				stringBuilder3.AppendFloatField(14000307, num16, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic6.criticalChance, num16), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			stringBuilder3.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic6.id).Replace("float2", Mathf.RoundToInt(configIgnoreMimic6.float2).ToString("F0")));
			float num17 = (spellInitialParameter6.fallExplosionRadius + 1.5f) * spellInitialParameter6.finalSizeRatio * (1f + spellInitialParameter6.extraSizeRatio);
			stringBuilder3.AppendFloatField(14000303, num17, SpellInfoExtend.ColorByValue_BigGood(1.5f, num17), newLine: true, "◆\u00a0\u200a", null, "m");
			return stringBuilder3.ToString();
		});
		SetFormatter(4014, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic5 = data.GetConfigIgnoreMimic();
			StringBuilder stringBuilder2 = new StringBuilder();
			float num11 = 1f;
			float num12 = 1f;
			SpellInitialParameter spellInitialParameter5 = new SpellInitialParameter();
			if ((bool)param.Wand)
			{
				spellInitialParameter5 = param.Wand.GetApplyWandAllEnhanceEffectSIP(40141, calculateSplitAsNormalEffect: true);
			}
			if (param.PlayerEffect)
			{
				num11 *= (1f + spellInitialParameter5.extraDamageRatio) * spellInitialParameter5.finalDamageRatio;
				num12 = param.Wand.GetWandMpCorrection() * param.Wand.GetWandAllEnhanceMpCorrection();
			}
			float num13 = spellInitialParameter5.extraCriticalChance * 100f;
			if (configIgnoreMimic5.criticalChance != 0f || num13 != 0f)
			{
				stringBuilder2.AppendFloatField(14000307, num13, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic5.criticalChance, num13), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			stringBuilder2.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic5.id));
			DataTextColorType color_BigIsGood = TextProcesser.GetColor_BigIsGood(1f, num11);
			string colorText = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(Mathf.CeilToInt((float)configIgnoreMimic5.int1 * num11 * configIgnoreMimic5.float2 / 100f), 2), color_BigIsGood);
			DataTextColorType color_SmallIsGood = TextProcesser.GetColor_SmallIsGood(1f, num12);
			string colorText2 = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(configIgnoreMimic5.float1 * num12 * configIgnoreMimic5.float2 / 100f, 1), color_SmallIsGood);
			stringBuilder2.Replace("int1", colorText);
			stringBuilder2.Replace("float1", colorText2);
			return stringBuilder2.ToString();
		});
		SetFormatter(4019, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic4 = data.GetConfigIgnoreMimic();
			StringBuilder stringBuilder = new StringBuilder();
			float num8 = 1f;
			SpellInitialParameter spellInitialParameter4 = new SpellInitialParameter();
			if ((bool)param.Wand)
			{
				spellInitialParameter4 = param.Wand.GetApplyWandAllEnhanceEffectSIP(40191, calculateSplitAsNormalEffect: true);
			}
			if (param.PlayerEffect)
			{
				num8 *= (1f + spellInitialParameter4.extraDamageRatio) * spellInitialParameter4.finalDamageRatio;
			}
			int num9 = Mathf.CeilToInt(configIgnoreMimic4.float2 * num8);
			stringBuilder.AppendFloatField(14000301, num9, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic4.float2, num9), newLine: false, "◆\u00a0\u200a");
			float num10 = configIgnoreMimic4.criticalChance + spellInitialParameter4.extraCriticalChance * 100f;
			if (configIgnoreMimic4.criticalChance != 0f || num10 != 0f)
			{
				stringBuilder.AppendFloatField(14000307, num10, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic4.criticalChance, num10), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			stringBuilder.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic4.id));
			stringBuilder.Replace("float1", configIgnoreMimic4.float1.ToString("F0"));
			stringBuilder.AppendFloatField(14000311, configIgnoreMimic4.slotNumModifyValue, DataTextColorType.Default, newLine: true, "◆\u00a0\u200a", "+");
			return stringBuilder.ToString();
		});
		SetFormatter(4025, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic3 = data.GetConfigIgnoreMimic();
			StringBuilder self3 = new StringBuilder();
			int item3 = PlayerMgr.Inst.GetPlayerRuneCount().RedRune;
			string text3 = (configIgnoreMimic3.id + 7000000).GetText();
			self3 = self3.StartField().Append("◆\u00a0\u200a" + text3 + "+1");
			SpellInitialParameter spellInitialParameter3 = new SpellInitialParameter();
			bool flag3 = false;
			if ((bool)param.Wand)
			{
				spellInitialParameter3 = param.Wand.GetApplyWandAllEnhanceEffectSIP(40251, calculateSplitAsNormalEffect: true);
				flag3 = param.Wand.GetWandSplitCountWithEnhance() > 0;
			}
			float num6 = (15f + (float)item3 * configIgnoreMimic3.float1) * (1f + spellInitialParameter3.extraDamageRatio) * spellInitialParameter3.finalDamageRatio;
			if (flag3)
			{
				num6 /= 0.33f;
			}
			self3.StartField().AppendFloatField(14000301, num6, SpellInfoExtend.ColorByValue_BigGood(0f, num6), newLine: false, "◆\u00a0\u200a");
			self3.Append(" = " + 15 + " + " + text3 + " * " + configIgnoreMimic3.float1.ToString("F0"));
			float num7 = configIgnoreMimic3.criticalChance + spellInitialParameter3.extraCriticalChance * 100f;
			if (configIgnoreMimic3.criticalChance != 0f || num7 != 0f)
			{
				self3.AppendFloatField(14000307, num7, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic3.criticalChance, num7), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			self3.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic3.id));
			self3.Replace("float2", configIgnoreMimic3.float2.ToString("F0"));
			if (param.WithDetailInfo)
			{
				int runeEffectLevel3 = PlayerMgr.Inst.GetRuneEffectLevel(item3);
				self3 = self3.StartField().StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[0].ToString("F0") + 1002054.GetFormatText() + "：" + 1002056.GetFormatText(), (runeEffectLevel3 >= 1) ? DataTextColorType.Green : DataTextColorType.Grey));
				self3 = self3.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[1].ToString("F0") + 1002054.GetFormatText() + "：" + 1002057.GetFormatText(), (runeEffectLevel3 >= 2) ? DataTextColorType.Green : DataTextColorType.Grey));
				self3 = self3.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[2].ToString("F0") + 1002054.GetFormatText() + "：" + 1002058.GetFormatText(), (runeEffectLevel3 >= 3) ? DataTextColorType.Green : DataTextColorType.Grey));
				self3 = self3.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[3].ToString("F0") + 1002054.GetFormatText() + "：" + 1002059.GetFormatText(), (runeEffectLevel3 >= 4) ? DataTextColorType.Green : DataTextColorType.Grey));
				self3.Replace("Radius", ((configIgnoreMimic3.radius + spellInitialParameter3.fallExplosionRadius) * (spellInitialParameter3.extraSizeRatio + 1f) * spellInitialParameter3.finalSizeRatio).ToString("F1"));
			}
			return self3.ToString();
		});
		SetFormatter(4026, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic2 = data.GetConfigIgnoreMimic();
			StringBuilder self2 = new StringBuilder();
			int item2 = PlayerMgr.Inst.GetPlayerRuneCount().GreenRune;
			string text2 = (configIgnoreMimic2.id + 7000000).GetText();
			self2 = self2.StartField().Append("◆\u00a0\u200a" + text2 + "+1");
			SpellInitialParameter spellInitialParameter2 = new SpellInitialParameter();
			bool flag2 = false;
			if ((bool)param.Wand)
			{
				spellInitialParameter2 = param.Wand.GetApplyWandAllEnhanceEffectSIP(40251, calculateSplitAsNormalEffect: true);
				flag2 = param.Wand.GetWandSplitCountWithEnhance() > 0;
			}
			float num4 = (float)item2 * configIgnoreMimic2.float1 * (1f + spellInitialParameter2.extraDamageRatio) * spellInitialParameter2.finalDamageRatio;
			if (flag2)
			{
				num4 /= 0.33f;
			}
			self2.StartField().AppendFloatField(14000301, num4, SpellInfoExtend.ColorByValue_BigGood(0f, num4), newLine: false, "◆\u00a0\u200a");
			self2.Append(" = " + text2 + " * " + configIgnoreMimic2.float1.ToString("F0"));
			float num5 = configIgnoreMimic2.criticalChance + spellInitialParameter2.extraCriticalChance * 100f;
			if (configIgnoreMimic2.criticalChance != 0f || num5 != 0f)
			{
				self2.AppendFloatField(14000307, num5, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic2.criticalChance, num5), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			self2.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic2.id));
			self2.Replace("float2", configIgnoreMimic2.float2.ToString("F1"));
			self2.Replace("int1", configIgnoreMimic2.int1.ToString("F0"));
			if (param.WithDetailInfo)
			{
				int runeEffectLevel2 = PlayerMgr.Inst.GetRuneEffectLevel(item2);
				self2 = self2.StartField().StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[0].ToString("F0") + 1002054.GetFormatText() + "：" + 1002060.GetFormatText(), (runeEffectLevel2 >= 1) ? DataTextColorType.Green : DataTextColorType.Grey));
				self2 = self2.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[1].ToString("F0") + 1002054.GetFormatText() + "：" + 1002061.GetFormatText(), (runeEffectLevel2 >= 2) ? DataTextColorType.Green : DataTextColorType.Grey));
				self2 = self2.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[2].ToString("F0") + 1002054.GetFormatText() + "：" + 1002062.GetFormatText(), (runeEffectLevel2 >= 3) ? DataTextColorType.Green : DataTextColorType.Grey));
				self2 = self2.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[3].ToString("F0") + 1002054.GetFormatText() + "：" + 1002063.GetFormatText(), (runeEffectLevel2 >= 4) ? DataTextColorType.Green : DataTextColorType.Grey));
			}
			return self2.ToString();
		});
		SetFormatter(4027, delegate(SlotData data, Param param)
		{
			SpellConfig configIgnoreMimic = data.GetConfigIgnoreMimic();
			StringBuilder self = new StringBuilder();
			int item = PlayerMgr.Inst.GetPlayerRuneCount().BlueRune;
			string text = (configIgnoreMimic.id + 7000000).GetText();
			self = self.StartField().Append("◆\u00a0\u200a" + text + "+1");
			SpellInitialParameter spellInitialParameter = new SpellInitialParameter();
			bool flag = false;
			float num = 0f;
			if ((bool)param.Wand)
			{
				spellInitialParameter = param.Wand.GetApplyWandAllEnhanceEffectSIP(40251, calculateSplitAsNormalEffect: true);
				flag = param.Wand.GetWandSplitCountWithEnhance() > 0;
				if (PlayerMgr.Inst.GetRuneEffectLevel(item) >= 3)
				{
					num = param.Wand.MaxMP * 0.2f;
				}
			}
			float num2 = (num + (float)item * configIgnoreMimic.float1) * (1f + spellInitialParameter.extraDamageRatio) * spellInitialParameter.finalDamageRatio;
			if (flag)
			{
				num2 /= 0.33f;
			}
			self.StartField().AppendFloatField(14000301, num2, SpellInfoExtend.ColorByValue_BigGood(0f, num2), newLine: false, "◆\u00a0\u200a");
			self.Append(" = " + text + " * " + configIgnoreMimic.float1.ToString("F0"));
			float num3 = configIgnoreMimic.criticalChance + spellInitialParameter.extraCriticalChance * 100f;
			if (configIgnoreMimic.criticalChance != 0f || num3 != 0f)
			{
				self.AppendFloatField(14000307, num3, SpellInfoExtend.ColorByValue_BigGood(configIgnoreMimic.criticalChance, num3), newLine: true, "◆\u00a0\u200a", null, "%");
			}
			self.StartField().Append("◆\u00a0\u200a" + GetSpellDescAutoReplaceNewLine(configIgnoreMimic.id));
			self.Replace("float2", configIgnoreMimic.float2.ToString("F0"));
			self.Replace("int1", configIgnoreMimic.int1.ToString("F0"));
			if (param.WithDetailInfo)
			{
				int runeEffectLevel = PlayerMgr.Inst.GetRuneEffectLevel(item);
				self = self.StartField().StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[0].ToString("F0") + 1002054.GetFormatText() + "：" + 1002064.GetFormatText(), (runeEffectLevel >= 1) ? DataTextColorType.Green : DataTextColorType.Grey));
				self = self.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[1].ToString("F0") + 1002054.GetFormatText() + "：" + 1002065.GetFormatText(), (runeEffectLevel >= 2) ? DataTextColorType.Green : DataTextColorType.Grey));
				self = self.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[2].ToString("F0") + 1002054.GetFormatText() + "：" + 1002066.GetFormatText(), (runeEffectLevel >= 3) ? DataTextColorType.Green : DataTextColorType.Grey));
				self = self.StartField().Append(TextProcesser.GetColorText("◆\u00a0\u200a" + GameConstManaged.LostCastleRuneLevelThreshold[3].ToString("F0") + 1002054.GetFormatText() + "：" + 1002067.GetFormatText(), (runeEffectLevel >= 4) ? DataTextColorType.Green : DataTextColorType.Grey));
			}
			return self.ToString();
		});
	}

	private static StringBuilder GetSpellDescAutoReplaceNewLine(int spellId)
	{
		return new StringBuilder((spellId + 7100000).GetText()).Replace("\\", "\n◆\u00a0\u200a");
	}

	private static StringBuilder AppendSpellFieldSlotCost(this StringBuilder sb, int slotCost)
	{
		if (slotCost == 1)
		{
			return sb;
		}
		return sb.AppendIntField(14000310, slotCost, DataTextColorType.Default, newLine: true, "◆\u00a0\u200a");
	}
}
