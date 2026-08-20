using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct PotionConfig
{
	public class Initializer : DataMgr.ConfigInitializer<PotionConfig>
	{
		public override void ApplyResult(List<PotionConfig> result)
		{
			if (!ICJNOGPFMAM.MIFJADDOODN && !GameMgr.IsMobile_Static)
			{
				list = new List<PotionConfig>();
				for (int i = 0; i < result.Count / 2; i++)
				{
					list.Add(result[i]);
				}
			}
			else
			{
				list = result;
			}
			dic = new Dictionary<int, PotionConfig>();
			foreach (PotionConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, PotionConfig> dic;

	public static List<PotionConfig> list;

	public int id;

	public int priceCoin;

	public PotionAbilityType abilityType;

	public int int1;

	public int int2;

	public float float1;

	public float float2;

	public bool InEndlessMode;

	public static PotionConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id];
	}

	public string GetName()
	{
		return (id + 3000000).GetText();
	}

	public string GetInfo()
	{
		StringBuilder stringBuilder = new StringBuilder("◆\u00a0\u200a" + (id + 3100000).GetText());
		stringBuilder = stringBuilder.Replace("\\", "\n◆\u00a0\u200a");
		stringBuilder = stringBuilder.Replace("int1", int1.ToString());
		stringBuilder = stringBuilder.Replace("int2", int2.ToString());
		stringBuilder = stringBuilder.Replace("float1", float1.ToString());
		stringBuilder = stringBuilder.Replace("float2", float2.ToString());
		if (abilityType == PotionAbilityType.Refresh && BattleMgr.Inst != null && (BattleMgr.Inst.CurrentStage == 9 || BattleMgr.Inst.CurrentStage == 10))
		{
			stringBuilder.Append("\n\n" + GameConst.htmlColor_InfoGrey + 1002039.GetText().Replace("str1", 1001706.GetText()) + "</color>");
		}
		return stringBuilder.ToString();
	}

	public string GetIconPath()
	{
		return "Textures/PotionIcons/" + id;
	}
}
