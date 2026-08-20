using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RelicConfig
{
	public class Initializer : DataMgr.ConfigInitializer<RelicConfig>
	{
		public override void ApplyResult(List<RelicConfig> result)
		{
			if (!ICJNOGPFMAM.MIFJADDOODN && !GameMgr.IsMobile_Static)
			{
				list = new List<RelicConfig>();
				for (int i = 0; i < result.Count / 2; i++)
				{
					list.Add(result[i]);
				}
			}
			else if (!ICJNOGPFMAM.GGPJCCLPBJL && GameMgr.IsMobile_Static)
			{
				foreach (RelicConfig item in result)
				{
					int id = item.id;
					if (id == 938 || id == 961 || id == 962 || id == 963 || id == 964 || id == 965 || id == 966)
					{
						item.dropType = ItemDropType.None;
					}
				}
				list = result;
			}
			else
			{
				list = result;
			}
			dic = new Dictionary<int, RelicConfig>();
			foreach (RelicConfig item2 in list)
			{
				if (GameMgr.IsMobile_Static && item2.id == 11)
				{
					item2.dropType = ItemDropType.Common;
				}
				dic.Add(item2.id, item2);
			}
			if (GameMgr.IsHarmony_Static)
			{
				if (dic.ContainsKey(6))
				{
					dic[6].skinName = "";
				}
				if (dic.ContainsKey(7))
				{
					dic[7].skinName = "";
				}
				if (dic.ContainsKey(8))
				{
					dic[8].skinName = "";
				}
				if (dic.ContainsKey(10))
				{
					dic[10].skinName = "";
				}
				if (dic.ContainsKey(37))
				{
					dic[37].skinName = "";
				}
				if (dic.ContainsKey(48))
				{
					dic[48].skinName = "";
				}
				if (dic.ContainsKey(60))
				{
					dic[60].skinName = "";
				}
				if (dic.ContainsKey(937))
				{
					dic[937].skinName = "";
				}
			}
		}
	}

	public static Dictionary<int, RelicConfig> dic;

	public static List<RelicConfig> list;

	public int id;

	public string iconH;

	public int priceCoin;

	public int priceHP;

	public int maxCount;

	public ItemDropType dropType;

	public RelicAbilityType abilityType;

	public string skinName;

	public bool needActivate;

	public string skinNameTvMan;

	public string skinNameTomato;

	public string skinNameFrog;

	public string skinNameHalloween;

	public string skinNameDave;

	public string skinNameChristmas;

	public string skinNameSpring;

	public ItemAbilityInt int1;

	public ItemAbilityInt int2;

	public ItemAbilityInt int3;

	public ItemAbilityFloat float1;

	public ItemAbilityFloat float2;

	public ItemAbilityFloat float3;

	public int RedRunePoint;

	public int GreenRunePoint;

	public int BlueRunePoint;

	public bool InEndlessMode;

	public int level = 1;

	public int intTimer;

	public float floatTimer;

	public static RelicConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	public RelicConfig Copy()
	{
		return new RelicConfig
		{
			id = id,
			iconH = iconH,
			priceCoin = priceCoin,
			priceHP = priceHP,
			maxCount = maxCount,
			dropType = dropType,
			abilityType = abilityType,
			skinName = skinName,
			needActivate = needActivate,
			skinNameTvMan = skinNameTvMan,
			skinNameTomato = skinNameTomato,
			skinNameFrog = skinNameFrog,
			skinNameHalloween = skinNameHalloween,
			skinNameDave = skinNameDave,
			skinNameChristmas = skinNameChristmas,
			skinNameSpring = skinNameSpring,
			int1 = int1,
			int2 = int2,
			int3 = int3,
			float1 = float1,
			float2 = float2,
			float3 = float3,
			RedRunePoint = RedRunePoint,
			GreenRunePoint = GreenRunePoint,
			BlueRunePoint = BlueRunePoint,
			level = level,
			intTimer = intTimer,
			floatTimer = floatTimer
		};
	}

	private string GetDamageStr(int originalDMG)
	{
		int num = Mathf.CeilToInt((float)originalDMG * (1f + PlayerMgr.Inst.ExtraDamageRatio));
		if (originalDMG == num)
		{
			return originalDMG.ToString();
		}
		if (originalDMG > num)
		{
			return "<color=#FF0000>" + num + "</color>";
		}
		return "<color=#00FF00>" + num + "</color>";
	}

	private string GetDamageStr(float originalDMG)
	{
		float num = originalDMG * (1f + PlayerMgr.Inst.ExtraDamageRatio);
		string result = originalDMG.FormatWithUnit();
		string text = num.FormatWithUnit();
		if (Mathf.Approximately(originalDMG, num))
		{
			return result;
		}
		if (originalDMG > num)
		{
			return "<color=#FF0000>" + text + "</color>";
		}
		return "<color=#00FF00>" + text + "</color>";
	}

	private string GetRangeStr(float originalRange)
	{
		float num = originalRange * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false));
		if (originalRange == num)
		{
			return originalRange.ToString();
		}
		if (originalRange > num)
		{
			return "<color=#FF0000>" + num + "</color>";
		}
		return "<color=#00FF00>" + num + "</color>";
	}

	private string GetCDStr(float originalCD)
	{
		float num = originalCD;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD != null)
		{
			num *= 1f - (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceSkillCD.int1.result / 100f;
		}
		if (originalCD == num)
		{
			return originalCD.ToString();
		}
		if (originalCD > num)
		{
			return "<color=#00FF00>" + num + "</color>";
		}
		return "<color=#FF0000>" + num + "</color>";
	}

	public void CalculateAbility(int? calculateLevel = null)
	{
		int valueOrDefault = calculateLevel.GetValueOrDefault();
		if (!calculateLevel.HasValue)
		{
			valueOrDefault = level;
			calculateLevel = valueOrDefault;
		}
		int1.RandomResult(calculateLevel.Value);
		int2.RandomResult(calculateLevel.Value);
		int3.RandomResult(calculateLevel.Value);
		float1.RandomResult(calculateLevel.Value);
		float2.RandomResult(calculateLevel.Value);
		float3.RandomResult(calculateLevel.Value);
	}

	public string GetStrRarity()
	{
		switch (dropType)
		{
		case ItemDropType.None:
		case ItemDropType.Common:
			return 1001601.GetText();
		case ItemDropType.Rare:
			return 1001602.GetText();
		case ItemDropType.Epic:
			return 1001603.GetText();
		case ItemDropType.Special:
			return 1001604.GetText();
		default:
			Debug.LogError(dropType);
			return "";
		}
	}

	public string GetName(bool haveLevel = true)
	{
		string text = (id + 4000000).GetText();
		if (haveLevel && level > 1)
		{
			text = ((!GameMgr.IsHarmony_Static) ? (text + " " + 1006401.GetText() + level) : (text + level + "级"));
		}
		return text;
	}

	public string GetAdditionInfo()
	{
		return (id + 4300000).GetText();
	}

	public string GetInfo(bool includeExtraInfo, bool upgrade)
	{
		string text = (id + 4100000).GetText();
		StringBuilder stringBuilder = new StringBuilder(text);
		stringBuilder = stringBuilder.Replace("\\", "\n◆\u00a0\u200a");
		if (upgrade && level > 1)
		{
			CalculateAbility(level - 1);
			int result = int1.result;
			int result2 = int2.result;
			int result3 = int3.result;
			float result4 = float1.result;
			float result5 = float2.result;
			float result6 = float3.result;
			CalculateAbility();
			stringBuilder = (text.Contains("int1&DMG") ? stringBuilder.Replace("int1&DMG", UpgradeValue(GetDamageStr(result), GetDamageStr(int1.result))) : stringBuilder.Replace("int1", UpgradeValue(result.ToString(), int1.result.ToString())));
			stringBuilder = (text.Contains("int2&DMG") ? stringBuilder.Replace("int2&DMG", UpgradeValue(GetDamageStr(result2), GetDamageStr(int2.result))) : stringBuilder.Replace("int2", UpgradeValue(result2.ToString(), int2.result.ToString())));
			stringBuilder = (text.Contains("int3&DMG") ? stringBuilder.Replace("int3&DMG", UpgradeValue(GetDamageStr(result3), GetDamageStr(int3.result))) : stringBuilder.Replace("int3", UpgradeValue(result3.ToString(), int3.result.ToString())));
			stringBuilder = (text.Contains("float1&range") ? stringBuilder.Replace("float1&range", UpgradeValue(GetRangeStr(result4), GetRangeStr(float1.result))) : ((!text.Contains("float1&CD")) ? stringBuilder.Replace("float1", UpgradeValue(result4.ToString(), float1.result.ToString())) : stringBuilder.Replace("float1&CD", UpgradeValue(GetCDStr(result4), GetCDStr(float1.result)))));
			stringBuilder = (text.Contains("float2&range") ? stringBuilder.Replace("float2&range", UpgradeValue(GetRangeStr(result5), GetRangeStr(float2.result))) : ((!text.Contains("float2&CD")) ? stringBuilder.Replace("float2", UpgradeValue(result5.ToString(), float2.result.ToString())) : stringBuilder.Replace("float2&CD", UpgradeValue(GetCDStr(result5), GetCDStr(float2.result)))));
			stringBuilder = (text.Contains("float3&range") ? stringBuilder.Replace("float3&range", UpgradeValue(GetRangeStr(result6), GetRangeStr(float3.result))) : ((!text.Contains("float3&CD")) ? stringBuilder.Replace("float3", UpgradeValue(result6.ToString(), float3.result.ToString())) : stringBuilder.Replace("float3&CD", UpgradeValue(GetCDStr(result6), GetCDStr(float3.result)))));
			stringBuilder.Replace("SquareInt1", UpgradeValue(Mathf.Pow(result, 2f).ToString(), Mathf.Pow(int1.result, 2f).ToString()));
			stringBuilder.Replace("PowInt1&DMG", UpgradeValue(GetDamageStr((float)int1.value * Mathf.Pow(2f, level - 2)), GetDamageStr((float)int1.value * Mathf.Pow(2f, level - 1))));
			stringBuilder.Replace("PowInt2&DMG", UpgradeValue(GetDamageStr((float)int2.value * Mathf.Pow(2f, level - 2)), GetDamageStr((float)int2.value * Mathf.Pow(2f, level - 1))));
			stringBuilder.Replace("PowFloat2&DMG", UpgradeValue(GetDamageStr(float2.value * Mathf.Pow(2f, level - 2)), GetDamageStr(float2.value * Mathf.Pow(2f, level - 1))));
		}
		else
		{
			CalculateAbility();
			stringBuilder = (text.Contains("int1&DMG") ? stringBuilder.Replace("int1&DMG", GetDamageStr((float)int1.result)) : stringBuilder.Replace("int1", int1.result.ToString()));
			stringBuilder = (text.Contains("int2&DMG") ? stringBuilder.Replace("int2&DMG", GetDamageStr((float)int2.result)) : stringBuilder.Replace("int2", int2.result.ToString()));
			stringBuilder = (text.Contains("int3&DMG") ? stringBuilder.Replace("int3&DMG", GetDamageStr((float)int3.result)) : stringBuilder.Replace("int3", int3.result.ToString()));
			stringBuilder = (text.Contains("float1&range") ? stringBuilder.Replace("float1&range", GetRangeStr(float1.result)) : ((!text.Contains("float1&CD")) ? stringBuilder.Replace("float1", float1.result.ToString()) : stringBuilder.Replace("float1&CD", GetCDStr(float1.result))));
			stringBuilder = (text.Contains("float2&range") ? stringBuilder.Replace("float2&range", GetRangeStr(float2.result)) : ((!text.Contains("float2&CD")) ? stringBuilder.Replace("float2", float2.result.ToString()) : stringBuilder.Replace("float2&CD", GetCDStr(float2.result))));
			stringBuilder = (text.Contains("float3&range") ? stringBuilder.Replace("float3&range", GetRangeStr(float3.result)) : ((!text.Contains("float3&CD")) ? stringBuilder.Replace("float3", float3.result.ToString()) : stringBuilder.Replace("float3&CD", GetCDStr(float3.result))));
			stringBuilder.Replace("SquareInt1", int1.result.ToString());
			stringBuilder.Replace("PowInt1&DMG", GetDamageStr((float)int1.value * Mathf.Pow(2f, level - 1)));
			stringBuilder.Replace("PowInt2&DMG", GetDamageStr((float)int2.value * Mathf.Pow(2f, level - 1)));
			stringBuilder.Replace("PowFloat2&DMG", GetDamageStr(float2.value * Mathf.Pow(2f, level - 1)));
		}
		if (abilityType == RelicAbilityType.LightArmor)
		{
			string text2 = "";
			text2 = (GameMgr.IsMobile_Static ? 1002027.GetText() : ((UIMgr.Inst.InputType != 0) ? 1002026.GetText() : 1002025.GetFormatText()));
			stringBuilder = stringBuilder.Insert(0, text2 + " ");
			if (level > 1)
			{
				string text3 = 1002022.GetText();
				text3 = text3.Replace("int2", int2.result.ToString());
				stringBuilder = stringBuilder.Append(text3);
			}
		}
		else if (abilityType == RelicAbilityType.WarmSnow)
		{
			string text4 = "";
			text4 = (GameMgr.IsMobile_Static ? 1002030.GetText() : ((UIMgr.Inst.InputType != 0) ? 1002029.GetText() : 1002028.GetFormatText()));
			stringBuilder = stringBuilder.Insert(0, text4);
		}
		else if (abilityType == RelicAbilityType.Hunag)
		{
			string text5 = "";
			text5 = (GameMgr.IsMobile_Static ? 1002033.GetText() : ((UIMgr.Inst.InputType != 0) ? 1002032.GetText() : 1002031.GetFormatText()));
			stringBuilder = stringBuilder.Insert(0, text5);
		}
		else if (abilityType == RelicAbilityType.DivingSuit)
		{
			string text6 = "";
			text6 = (GameMgr.IsMobile_Static ? (1002053.GetText() + "\n◆\u00a0\u200a") : ((UIMgr.Inst.InputType != 0) ? (1002052.GetText() + "\n◆\u00a0\u200a") : (1002051.GetFormatText() + "\n◆\u00a0\u200a")));
			stringBuilder = stringBuilder.Insert(0, text6);
			if (level >= 2)
			{
				stringBuilder.Append(1002049.GetText());
			}
		}
		stringBuilder = stringBuilder.Insert(0, "◆\u00a0\u200a");
		if (includeExtraInfo)
		{
			stringBuilder.Append(GetExtraInfo());
		}
		if (!GameMgr.IsMobile_Static)
		{
			return $"<b>{stringBuilder}</b>";
		}
		return $"{stringBuilder}";
	}

	public string GetExtraInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (abilityType)
		{
		case RelicAbilityType.GreedSeed:
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002005.GetText() + ": " + intTimer + "/" + int1.result + "</color>");
			break;
		case RelicAbilityType.CurseWarrior:
		{
			float num = 0f;
			for (int i = 0; i < PlayerMgr.Inst.BaData.curseLevels.Count; i++)
			{
				num += (float)PlayerMgr.Inst.BaData.curseLevels[i];
			}
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002006.GetText() + ": " + num * (float)int1.result + "%</color>");
			break;
		}
		case RelicAbilityType.MoneyIsPower:
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002006.GetText() + ": " + PlayerMgr.Inst.BaData.coinCount * int2.result / int1.result + "%</color>");
			break;
		case RelicAbilityType.KeyIsPower:
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002006.GetText() + ": " + ((float)PlayerMgr.Inst.BaData.keyCount * (float)int2.result / (float)int1.result).FormatWithUnit() + "%</color>");
			break;
		case RelicAbilityType.MadEye:
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002006.GetText() + ": " + (playerPpt.unitCfg.maxHP - playerPpt.unitCfg.currentHP) * float1.result / (float)int1.result + "%</color>");
			}
			break;
		}
		case RelicAbilityType.MadWarrior:
		{
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt2))
			{
				stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002006.GetText() + ": " + ((playerPpt2.unitCfg.currentHP / playerPpt2.unitCfg.maxHP <= 0.5f) ? "50" : "0") + "%</color>");
			}
			break;
		}
		case RelicAbilityType.EnterDoorRemoveCurse:
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002007.GetText() + ": " + intTimer + "</color>");
			break;
		case RelicAbilityType.MedicineKit:
			if (int2.result > 1 && BattleMgr.Inst != null)
			{
				string text = 1002044.GetText().Replace("int1", (int2.result - intTimer).ToString());
				stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + text + "</color>\n");
			}
			break;
		case RelicAbilityType.RestartKey:
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002050.GetText() + "</color>");
			break;
		case RelicAbilityType.RuneWizard:
		{
			UiRelic_RuneWizard uiRelic_RuneWizard = PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard;
			if (!(uiRelic_RuneWizard == null))
			{
				stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 7040251.GetFormatText() + "：" + 14000307.GetText() + "+" + uiRelic_RuneWizard.GetRuneWizardSetBonusCriticalChance() + "%\n◆\u00a0\u200a" + 7040261.GetFormatText() + "：" + 1000505.GetText() + "+" + uiRelic_RuneWizard.GetRuneWizardSetBonusHp() + "\n◆\u00a0\u200a" + 7040271.GetFormatText() + "：" + 1000510.GetText() + "+" + uiRelic_RuneWizard.GetRuneWizardSetBonusMP() + "</color>\n");
			}
			break;
		}
		}
		if (dropType == ItemDropType.Special)
		{
			stringBuilder = stringBuilder.Append(GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + 1002706.GetText() + "\n◆\u00a0\u200a" + 1002020.GetText() + "</color>");
		}
		return stringBuilder.ToString();
	}

	public string GetIconPath()
	{
		if (GameMgr.IsHarmony_Static && !string.IsNullOrEmpty(iconH))
		{
			return "Textures/RelicIcons/" + iconH;
		}
		return "Textures/RelicIcons/" + id;
	}

	private string UpgradeValue(string oldValue, string newValue)
	{
		if (oldValue == newValue)
		{
			return oldValue;
		}
		if (oldValue.Contains("<color="))
		{
			int num = oldValue.IndexOf(">", StringComparison.Ordinal);
			int num2 = oldValue.LastIndexOf("<", StringComparison.Ordinal);
			oldValue = oldValue.Substring(num + 1, num2 - num - 1);
		}
		string text = "";
		text = ((!GameMgr.IsMobile_Static) ? "</b><color=#dddddd>\u200a\u200a➔\u200a\u200a</color><b>" : "<color=#dddddd>\u200a\u200a➔\u200a\u200a</color>");
		return "<color=#888888>" + oldValue + "</color>" + text + "<color=#44ff44>" + newValue + "</color>";
	}
}
