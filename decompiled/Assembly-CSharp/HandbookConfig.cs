using System.Collections.Generic;

public class HandbookConfig
{
	public class Initializer : DataMgr.ConfigInitializer<HandbookConfig>
	{
		public override void ApplyResult(List<HandbookConfig> result)
		{
			list = result;
			dic = new Dictionary<int, HandbookConfig>();
			foreach (HandbookConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, HandbookConfig> dic;

	public static List<HandbookConfig> list;

	public int id;

	public HandbookBelongCategory belongCategory;

	public HandbookDemoType demoType;

	public int titleTextID;

	public int descTextID;

	public string GetTitle()
	{
		return (id + 14000000).GetText();
	}

	public string GetDesc()
	{
		return (id + 14100000).GetText();
	}
}
