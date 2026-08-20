using System.Collections.Generic;

public struct MultiDifficultyConfig
{
	public class Initializer : DataMgr.ConfigInitializer<MultiDifficultyConfig>
	{
		public override void ApplyResult(List<MultiDifficultyConfig> result)
		{
			list = result;
			dic = new Dictionary<int, MultiDifficultyConfig>();
			foreach (MultiDifficultyConfig item in list)
			{
				dic.Add(item.normalUnitID, item);
			}
		}
	}

	public static Dictionary<int, MultiDifficultyConfig> dic;

	public static List<MultiDifficultyConfig> list;

	public int normalUnitID;

	public float hardChance;

	public int hardUnitID;

	public float nightmareChance;

	public int nightmareUnitID;
}
