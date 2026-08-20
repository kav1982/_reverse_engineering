using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextConfig
{
	public class Initializer : DataMgr.ConfigInitializer<TextConfig>
	{
		private string part;

		public Initializer(string part)
		{
			this.part = part;
		}

		public override void ReadJson()
		{
			if (string.IsNullOrEmpty(part))
			{
				base.ReadJson();
			}
			else
			{
				_json = DataMgr.GetConfigText(part);
			}
		}

		public override void ApplyResult(List<TextConfig> result)
		{
			list.AddRange(result);
			foreach (TextConfig item in result)
			{
				dic.Add(item.id, item);
			}
		}

		public static List<Initializer> GetAllInitializer()
		{
			return new List<Initializer>
			{
				new Initializer(null),
				new Initializer("TextConfig_Curse"),
				new Initializer("TextConfig_Potion"),
				new Initializer("TextConfig_Relic"),
				new Initializer("TextConfig_Resource"),
				new Initializer("TextConfig_Wand"),
				new Initializer("TextConfig_Spell"),
				new Initializer("TextConfig_Story"),
				new Initializer("TextConfig_Unit"),
				new Initializer("TextConfig_Research"),
				new Initializer("TextConfig_Set"),
				new Initializer("TextConfig_ActivateGirl"),
				new Initializer("TextConfig_Handbook"),
				new Initializer("TextConfig_RelicGroup")
			};
		}
	}

	public static Dictionary<int, TextConfig> dic = new Dictionary<int, TextConfig>();

	public static List<TextConfig> list = new List<TextConfig>();

	public int id;

	public string chineseS;

	public string chineseT;

	public string english;

	public string german;

	public string japanese;

	public string korean;

	public string russian;

	public string spanish_spain;

	public string french;

	public string portuguese_brazil;

	public string swedish;

	public string polish;

	public string turkish;

	public string italian;

	public string czech;

	public string chineseH;

	public bool applyAlogia;

	public string currentStr;

	public static TextConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Clear()
	{
		dic.Clear();
		list.Clear();
	}

	public TextConfig Copy()
	{
		return new TextConfig
		{
			id = id,
			applyAlogia = applyAlogia,
			chineseS = chineseS,
			chineseT = chineseT
		};
	}

	public void RegetStr()
	{
		switch (DataMgr.settingData.language)
		{
		case LanguageType.ChineseS:
			if (GameMgr.IsHarmony_Static && !string.IsNullOrEmpty(chineseH))
			{
				currentStr = chineseH;
			}
			else
			{
				currentStr = chineseS;
			}
			break;
		case LanguageType.ChineseT:
			currentStr = chineseT;
			break;
		case LanguageType.English:
			currentStr = english;
			break;
		case LanguageType.German:
			currentStr = german;
			break;
		case LanguageType.Japanese:
			currentStr = japanese;
			break;
		case LanguageType.Korean:
			currentStr = korean;
			break;
		case LanguageType.Russian:
			currentStr = russian;
			break;
		case LanguageType.Spanish_spain:
			currentStr = spanish_spain;
			break;
		case LanguageType.French:
			currentStr = french;
			break;
		case LanguageType.Portuguese_brazil:
			currentStr = portuguese_brazil;
			break;
		case LanguageType.Swedish:
			currentStr = swedish;
			break;
		case LanguageType.Polish:
			currentStr = polish;
			break;
		case LanguageType.Turkish:
			currentStr = turkish;
			break;
		case LanguageType.Italian:
			currentStr = italian;
			break;
		case LanguageType.Czech:
			currentStr = czech;
			break;
		default:
			Debug.LogError(DataMgr.settingData.language);
			currentStr = english;
			break;
		}
	}

	public static bool TryGetText(int id, out string text, bool forceApplyAlogia = false)
	{
		if (dic.TryGetValue(id, out var value))
		{
			if (PlayerMgr.Inst.ItemCtrller.curse_IsIlliteracy && (bool)BattleMgr.Inst && (value.applyAlogia || forceApplyAlogia))
			{
				int num = value.currentStr.Length;
				if (DataMgr.settingData.language == LanguageType.English)
				{
					num = Mathf.CeilToInt((float)num * 0.5f);
				}
				text = new StringBuilder().Append('囧', num).ToString();
			}
			else
			{
				text = value.currentStr;
			}
			return true;
		}
		text = "";
		return false;
	}

	public static string GetText(int id, bool forceApplyAlogia = false)
	{
		if (!TryGetText(id, out var text, forceApplyAlogia))
		{
			Debug.LogError("no MultilingualConfig id:" + id);
		}
		return text;
	}

	public static void RegetAllText()
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].RegetStr();
		}
	}
}
