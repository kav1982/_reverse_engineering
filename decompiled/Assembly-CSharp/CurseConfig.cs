using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CurseConfig
{
	public class Initializer : DataMgr.ConfigInitializer<CurseConfig>
	{
		public override void ApplyResult(List<CurseConfig> result)
		{
			list = result;
			dic = new Dictionary<int, CurseConfig>();
			foreach (CurseConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, CurseConfig> dic;

	public static List<CurseConfig> list;

	public int id;

	public string iconH;

	public int priceCoin;

	public int priceHP;

	public int count = 1;

	public ItemDropType dropType;

	public CurseAbilityType abilityType;

	public ItemAbilityInt int1;

	public ItemAbilityInt int2;

	public ItemAbilityFloat float1;

	public int level = 1;

	public int intTimer;

	public float floatTimer;

	public static CurseConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	public CurseConfig Copy()
	{
		return new CurseConfig
		{
			id = id,
			priceCoin = priceCoin,
			priceHP = priceHP,
			count = count,
			dropType = dropType,
			abilityType = abilityType,
			int1 = int1,
			int2 = int2,
			float1 = float1,
			level = level,
			intTimer = intTimer,
			floatTimer = floatTimer
		};
	}

	public void CalculateAbility()
	{
		int1.RandomResult(level);
		int2.RandomResult(level);
		float1.RandomResult(level);
	}

	public int GetPrice()
	{
		if (priceHP > 0)
		{
			return priceHP;
		}
		return priceCoin;
	}

	public string GetStrLevel()
	{
		return " " + 1006401.GetText() + level;
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

	public string GetName()
	{
		string text = (id + 2000000).GetText();
		if (level > 1)
		{
			text = text + " " + 1006401.GetText() + level;
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon != null && id == PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon.RandomCurseID)
		{
			text = PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon.CurseCfg.GetName() + "(" + text + ")";
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare != null && id == PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare.RandomCurseID)
		{
			text = PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare.CurseCfg.GetName() + "(" + text + ")";
		}
		return text;
	}

	public string GetInfo(bool includeExtraInfo = false)
	{
		CalculateAbility();
		StringBuilder stringBuilder = new StringBuilder("◆\u00a0\u200a" + (id + 2100000).GetText());
		stringBuilder = stringBuilder.Replace("\\", "\n◆\u00a0\u200a");
		stringBuilder = stringBuilder.Replace("int1", int1.result.ToString());
		stringBuilder = stringBuilder.Replace("int2", int2.result.ToString());
		stringBuilder = stringBuilder.Replace("float1", float1.result.ToString());
		if (includeExtraInfo)
		{
			UnitProperty_Dots playerPpt;
			if (abilityType == CurseAbilityType.MoreMoneyMoreInjured)
			{
				float num = (float)PlayerMgr.Inst.CoinCount / (float)int1.result * (float)int2.result;
				if (num > float1.result)
				{
					num = float1.result;
				}
				stringBuilder = stringBuilder.Append("\n\n" + GameConst.htmlColor_InfoGrey + 1002021.GetText() + ":+" + num + "%</color>");
			}
			else if (abilityType == CurseAbilityType.FullHPAddDamage && PlayerMgr.Inst.TryGetPlayerPpt(out playerPpt))
			{
				if (playerPpt.unitCfg.currentHP == playerPpt.unitCfg.maxHP)
				{
					floatTimer = int1.result;
				}
				else
				{
					floatTimer = int2.result;
				}
				stringBuilder = stringBuilder.Append("\n\n" + GameConst.htmlColor_InfoGrey + 1002006.GetText() + ":" + floatTimer + "%</color>");
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon != null && id == PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon.RandomCurseID)
		{
			stringBuilder.Insert(0, PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon.CurseCfg.GetInfo() + "\n");
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare != null && id == PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare.RandomCurseID)
		{
			stringBuilder.Insert(0, PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare.CurseCfg.GetInfo() + "\n");
		}
		return stringBuilder.ToString();
	}

	public string GetIconPath()
	{
		if (GameMgr.IsHarmony_Static && iconH != "" && iconH != null)
		{
			return "Textures/CurseIcons/" + iconH;
		}
		return "Textures/CurseIcons/" + id;
	}
}
