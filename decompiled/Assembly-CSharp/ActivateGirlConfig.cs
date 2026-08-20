using System.Collections.Generic;
using System.Text;

public class ActivateGirlConfig
{
	public class Initializer : DataMgr.ConfigInitializer<ActivateGirlConfig>
	{
		public override void ApplyResult(List<ActivateGirlConfig> result)
		{
			list = result;
			dic = new Dictionary<int, ActivateGirlConfig>();
			foreach (ActivateGirlConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, ActivateGirlConfig> dic;

	public static List<ActivateGirlConfig> list;

	public int id;

	public int preactivateID;

	public ActivateGirlSpecialType specialType;

	public int belongLayer;

	public int cost;

	public int[] spellIDs;

	public string GetName()
	{
		return (id + 11000000).GetText();
	}

	public string GetDesc()
	{
		StringBuilder stringBuilder = new StringBuilder((id + 11100000).GetText());
		if (specialType == ActivateGirlSpecialType.TentacleGirlReaction)
		{
			stringBuilder = stringBuilder.Replace("int1", 8.ToString());
			stringBuilder = stringBuilder.Replace("int2", 3.ToString());
			stringBuilder.Append("\n" + GameConst.htmlColor_InfoGrey + 1003310.GetText() + "</color>");
		}
		return stringBuilder.ToString();
	}
}
