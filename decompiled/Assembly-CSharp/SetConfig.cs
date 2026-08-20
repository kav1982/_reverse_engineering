using System.Collections.Generic;

public class SetConfig
{
	public class Initializer : DataMgr.ConfigInitializer<SetConfig>
	{
		public override void ApplyResult(List<SetConfig> result)
		{
			list = result;
			dic = new Dictionary<int, SetConfig>();
			foreach (SetConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, SetConfig> dic;

	public static List<SetConfig> list;

	public int id;

	public int[] WandIDs;

	public int relicID;

	public int unlockCost;

	public int unlockInt1;

	public int[] upgradeCosts;

	public string GetName()
	{
		return (id + 10000000).GetText().Split("/")[0];
	}

	public string GetDes()
	{
		string[] array = (id + 10000000).GetText().Split("/");
		if (array.Length > 1)
		{
			return array[1];
		}
		return "";
	}

	public string GetUnlockDesc()
	{
		return (1002103.GetText() + ": " + (id + 10100000).GetText()).Replace("int1", unlockInt1.ToString());
	}
}
