using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SpellConfig
{
	public class Initializer : DataMgr.ConfigInitializer<SpellConfig>
	{
		public override void ApplyResult(List<SpellConfig> result)
		{
			if (!ICJNOGPFMAM.GGPJCCLPBJL && GameMgr.IsMobile_Static)
			{
				foreach (SpellConfig item in result)
				{
					int id = item.id;
					if (id == 10311 || id == 10312 || id == 10313 || id == 40241)
					{
						item.dropType = ItemDropType.None;
					}
				}
			}
			list = result;
			dic = new Dictionary<int, SpellConfig>();
			foreach (SpellConfig item2 in list)
			{
				dic.Add(item2.id, item2);
			}
		}
	}

	public static Dictionary<int, SpellConfig> dic;

	public static List<SpellConfig> list;

	public int id;

	public int priceCoin;

	public int priceHP;

	public string icon;

	public string iconH;

	public int level = 1;

	public bool canCompound = true;

	public ItemDropType dropType;

	public bool needActivate;

	public float shootIntervalAddSubRevise;

	public float criticalChance;

	public float coolDownAddSubRevise;

	public float coolDownRatio = 1f;

	public float angle;

	public int slotCost = 1;

	public int slotNumModifyValue;

	public SpellType useType;

	public string prefab;

	public int mpCost;

	public int shootCount = 1;

	public float damage;

	public bool isDPS;

	public float DPSDamageInterval = 0.25f;

	public bool isKeepCasting;

	public bool canCancelCasting;

	public bool isParentTypeSpell;

	public float speed;

	public float duration;

	public float knockback;

	public float recoil;

	public float mpCostAddSubCorrection;

	public float mpCostMulDivCorrection;

	public bool haveEffecforMissileSpell;

	public bool haveEffecforSummonSpell;

	public SpellAbilityType abilityType;

	public float upSpeed;

	public float gravity;

	public float radius;

	public int summonID;

	public int summonLimit;

	public float float1;

	public float float2;

	public float float3;

	public int int1;

	public int int2;

	public int int3;

	public bool isSplitSpell;

	public bool playShootSE = true;

	public static SpellConfig GetConfigCopy(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	public SpellConfig Copy()
	{
		return new SpellConfig
		{
			id = id,
			priceCoin = priceCoin,
			priceHP = priceHP,
			icon = icon,
			iconH = iconH,
			level = level,
			canCompound = canCompound,
			dropType = dropType,
			needActivate = needActivate,
			shootIntervalAddSubRevise = shootIntervalAddSubRevise,
			criticalChance = criticalChance,
			coolDownAddSubRevise = coolDownAddSubRevise,
			coolDownRatio = coolDownRatio,
			angle = angle,
			useType = useType,
			slotCost = slotCost,
			slotNumModifyValue = slotNumModifyValue,
			prefab = prefab,
			mpCost = mpCost,
			shootCount = shootCount,
			damage = damage,
			isDPS = isDPS,
			isKeepCasting = isKeepCasting,
			isParentTypeSpell = isParentTypeSpell,
			speed = speed,
			duration = duration,
			knockback = knockback,
			recoil = recoil,
			mpCostAddSubCorrection = mpCostAddSubCorrection,
			mpCostMulDivCorrection = mpCostMulDivCorrection,
			haveEffecforMissileSpell = haveEffecforMissileSpell,
			haveEffecforSummonSpell = haveEffecforSummonSpell,
			abilityType = abilityType,
			upSpeed = upSpeed,
			gravity = gravity,
			radius = radius,
			summonID = summonID,
			summonLimit = summonLimit,
			float1 = float1,
			float2 = float2,
			float3 = float3,
			int1 = int1,
			int2 = int2,
			int3 = int3,
			DPSDamageInterval = DPSDamageInterval,
			isSplitSpell = isSplitSpell,
			playShootSE = playShootSE
		};
	}

	public string GetStrType()
	{
		switch (useType)
		{
		case SpellType.Missile:
			return 1002205.GetText();
		case SpellType.Summon:
			return 1002206.GetText();
		case SpellType.Enhance:
			return 1002207.GetText();
		case SpellType.Passive:
			return 1002208.GetText();
		default:
			Debug.LogError(useType);
			return "";
		}
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

	public string GetName(bool includeLevel = true)
	{
		string text = (id + 7000000).GetText();
		if (includeLevel)
		{
			if (level == 2)
			{
				text += "+";
			}
			else if (level == 3)
			{
				text += "++";
			}
		}
		return text;
	}

	public string GetAssistDesc()
	{
		return (id / 10 * 10 + 1 + 7200000).GetText().Replace("\\", "\n\n");
	}

	public string GetInfo(float damageRatio = 1f, string newLineAdd = "", bool withDetailInfo = true)
	{
		if (SpellInfoFormatters.HasFormatter(id, idIsIgnoreLevel: false))
		{
			SlotData info = new SlotData(id);
			bool withDetailInfo2 = withDetailInfo;
			return info.FormatInfo(default(SpellInfoFormatters.Param), withDetailInfo2);
		}
		StringBuilder stringBuilder = new StringBuilder();
		DataTextColorType color = DataTextColorType.Default;
		if (damageRatio > 1f)
		{
			color = DataTextColorType.Green;
		}
		if (damageRatio < 1f)
		{
			color = DataTextColorType.Red;
		}
		if (summonLimit > 0)
		{
			stringBuilder.AppendIntField(14000304, summonLimit, DataTextColorType.Default, newLine: true, newLineAdd);
		}
		if (useType == SpellType.Summon)
		{
			float maxHP = UnitConfig.map[summonID].maxHP;
			stringBuilder.AppendFloatField(14000314, maxHP, DataTextColorType.Default, newLine: true, newLineAdd);
		}
		if (damage != 0f)
		{
			stringBuilder.AppendStringField(14000301, (damage * damageRatio).ToStringDamage(), color, newLine: true, newLineAdd);
			if (isDPS)
			{
				stringBuilder.AppendLink(" (", 14000302.GetText(forceApplyAlogia: true), ")");
			}
			if (abilityType == SpellAbilityType.SuperNova)
			{
				stringBuilder.AppendLink(" (", 1002024.GetText(), ")");
			}
			if (abilityType == SpellAbilityType.DragonBreath)
			{
				if (DataMgr.settingData.language == LanguageType.English || DataMgr.settingData.language == LanguageType.Spanish_spain || DataMgr.settingData.language == LanguageType.German || DataMgr.settingData.language == LanguageType.Russian)
				{
					stringBuilder.AppendLink(", ", "+" + float3 + "% ", 1002018.GetText());
				}
				else
				{
					stringBuilder.AppendLink(", ", 1002018.GetText(), float3 + "%");
				}
			}
		}
		if (shootCount > 1)
		{
			stringBuilder.AppendIntField(14000312, shootCount, DataTextColorType.Default, newLine: true, newLineAdd);
		}
		if (shootIntervalAddSubRevise != 0f)
		{
			string valuePrefix = ((shootIntervalAddSubRevise > 0f) ? "+" : null);
			string text = 1001101.GetText();
			stringBuilder.AppendFloatField(14000204, shootIntervalAddSubRevise, DataTextColorType.Default, newLine: true, newLineAdd, valuePrefix, text);
		}
		if (coolDownAddSubRevise != 0f)
		{
			string valuePrefix2 = ((coolDownAddSubRevise > 0f) ? "+" : null);
			string text2 = 1001101.GetText();
			stringBuilder.AppendFloatField(14000205, coolDownAddSubRevise, DataTextColorType.Default, newLine: true, newLineAdd, valuePrefix2, text2);
		}
		if (Math.Abs(coolDownRatio - 1f) > 0.01f)
		{
			stringBuilder.AppendFloatField(14000205, coolDownRatio * 100f, DataTextColorType.Default, newLine: true, newLineAdd, "×", "%");
		}
		if (criticalChance != 0f)
		{
			SpellType spellType = useType;
			string valuePrefix3 = ((spellType == SpellType.Enhance || spellType == SpellType.Passive) ? "+" : null);
			float fieldValue = Mathf.Max(0f, criticalChance);
			stringBuilder.AppendFloatField(14000307, fieldValue, DataTextColorType.Default, newLine: true, newLineAdd, valuePrefix3, "%");
		}
		if (angle != 0f)
		{
			string valuePrefix4 = null;
			if (angle > 0f)
			{
				SpellType spellType = useType;
				if (spellType == SpellType.Enhance || spellType == SpellType.Passive)
				{
					valuePrefix4 = "+";
				}
			}
			stringBuilder.AppendFloatField(14000206, angle, DataTextColorType.Default, newLine: true, newLineAdd, valuePrefix4, "°");
		}
		if (mpCostAddSubCorrection != 0f)
		{
			string valuePrefix5 = null;
			if (mpCostAddSubCorrection > 0f)
			{
				valuePrefix5 = "+";
			}
			stringBuilder.AppendFloatField(14000305, mpCostAddSubCorrection, DataTextColorType.Default, newLine: true, newLineAdd, valuePrefix5, "%");
		}
		if (Math.Abs(mpCostMulDivCorrection) > 0.01f)
		{
			stringBuilder.AppendFloatField(14000305, mpCostMulDivCorrection, DataTextColorType.Default, newLine: true, newLineAdd, "×", "%");
		}
		if (radius != 0f)
		{
			stringBuilder.AppendFloatField(14000303, radius, DataTextColorType.Default, newLine: true, newLineAdd, null, GameMgr.IsHarmony_Static ? "米" : "m");
		}
		if (slotCost > 1)
		{
			stringBuilder.AppendIntField(14000310, slotCost, DataTextColorType.Default, newLine: true, newLineAdd);
		}
		if (slotNumModifyValue > 0)
		{
			stringBuilder.AppendFloatField(14000311, slotNumModifyValue, DataTextColorType.Default, newLine: true, newLineAdd, "+");
		}
		else if (slotNumModifyValue < 0)
		{
			stringBuilder.AppendFloatField(14000311, slotNumModifyValue, DataTextColorType.Default, newLine: true, newLineAdd);
		}
		return stringBuilder.ToString().Trim();
	}

	public string GetDes(float damageRatio = 1f, string nextLineAdd = "", string color = "", string header = "")
	{
		if (SpellInfoFormatters.HasFormatter(id, idIsIgnoreLevel: false))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		DataTextColorType type = DataTextColorType.Default;
		if (damageRatio > 1f)
		{
			type = DataTextColorType.Green;
		}
		if (damageRatio < 1f)
		{
			type = DataTextColorType.Red;
		}
		StringBuilder stringBuilder2 = new StringBuilder((id + 7100000).GetText());
		if (stringBuilder2.ToString() != "")
		{
			stringBuilder2.Insert(0, color + header);
			string newValue = int1.ToString();
			if (abilityType == SpellAbilityType.LightningChain)
			{
				newValue = TextProcesser.GetColorText(Mathf.Ceil((float)int1 * damageRatio).ToString("F0"), type);
			}
			stringBuilder2 = stringBuilder2.Replace("int1", newValue);
			if (abilityType == SpellAbilityType.DimensionTraveller)
			{
				string newValue2 = GeneralTool.FloatToRetainDecimals(float3, 2);
				stringBuilder2 = stringBuilder2.Replace("float3", newValue2);
			}
			string newValue3 = int2.ToString();
			if (abilityType == SpellAbilityType.OnHitTrigger)
			{
				newValue3 = GeneralTool.FloatToRetainDecimals(1f / (float)int2, 1);
			}
			stringBuilder2 = stringBuilder2.Replace("int2", newValue3);
			stringBuilder2 = stringBuilder2.Replace("int3", int3.ToString());
			string newValue4 = GeneralTool.FloatToRetainDecimals(float1, 1);
			if (abilityType == SpellAbilityType.LifeLine)
			{
				newValue4 = TextProcesser.GetColorText(Mathf.Ceil(float1 * damageRatio).ToString("F0"), type);
			}
			if (abilityType == SpellAbilityType.ArcaneExplosion)
			{
				newValue4 = GeneralTool.FloatToRetainDecimals(float1, 1);
			}
			stringBuilder2 = stringBuilder2.Replace("float1", newValue4);
			stringBuilder2 = stringBuilder2.Replace("float2", GeneralTool.FloatToRetainDecimals(float2, 1));
			stringBuilder2 = stringBuilder2.Replace("float3", GeneralTool.FloatToRetainDecimals(float3, 1));
			stringBuilder2 = stringBuilder2.Replace("shootCount", shootCount.ToString());
			stringBuilder2 = stringBuilder2.Replace("spellDuration", GeneralTool.FloatToRetainDecimals(duration, 1));
			stringBuilder2 = stringBuilder2.Replace("DragonBreathSpecialDescription", float2.ToString("F0"));
			stringBuilder2 = stringBuilder2.Replace("\\", "\n" + nextLineAdd);
			if (color != "")
			{
				stringBuilder2.Append("</color>");
			}
			stringBuilder.StartField().Append(stringBuilder2);
		}
		return stringBuilder.ToString().Trim();
	}

	public string GetIconPath()
	{
		if (GameMgr.IsHarmony_Static && iconH != "" && iconH != null)
		{
			return "Textures/SpellIcons/" + iconH;
		}
		return "Textures/SpellIcons/" + icon;
	}
}
