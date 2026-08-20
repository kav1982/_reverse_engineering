using System.Collections.Generic;

public class ResearchConfig
{
	public class Initializer : DataMgr.ConfigInitializer<ResearchConfig>
	{
		public override void ApplyResult(List<ResearchConfig> result)
		{
			list = result;
			dic = new Dictionary<int, ResearchConfig>();
			foreach (ResearchConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, ResearchConfig> dic;

	public static List<ResearchConfig> list;

	public int id;

	public ResearchAbilityType abilityType;

	public ResearchOpenType openType;

	public int int1;

	public int cost;

	public int preResearchID;

	public string icon;

	public int hdID;

	public bool canDisactive;

	public string GetName()
	{
		return (id + 9000000).GetText();
	}

	public string GetDesc()
	{
		return (id + 9100000).GetText().Replace("int1", int1.ToString()).AddZeroSpaceIfLanguageNeed();
	}

	public static int HavePostResearch(int id)
	{
		foreach (ResearchConfig item in list)
		{
			if (item.preResearchID == id)
			{
				return item.id;
			}
		}
		return -1;
	}
}
