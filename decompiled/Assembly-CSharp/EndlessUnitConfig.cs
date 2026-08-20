using System.Collections.Generic;
using UnityEngine;

public struct EndlessUnitConfig
{
	public class Initializer : DataMgr.ConfigInitializer<EndlessUnitConfig>
	{
		public override void ApplyResult(List<EndlessUnitConfig> result)
		{
			list = result;
			dic = new Dictionary<int, EndlessUnitConfig>();
			unitDropCounts = new List<EndlessBattleSpawnInfo.DropCounts>();
			foreach (EndlessUnitConfig item in list)
			{
				dic.Add(item.id, item);
				unitDropCounts.Add(new EndlessBattleSpawnInfo.DropCounts
				{
					unitID = item.id,
					dropCount = item.dropCount
				});
			}
		}
	}

	public static Dictionary<int, EndlessUnitConfig> dic;

	public static List<EndlessUnitConfig> list;

	public static List<EndlessBattleSpawnInfo.DropCounts> unitDropCounts;

	public int id;

	public int groupType;

	public int dropCount;

	public float chance;

	public int stage;

	public EndlessUnitConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id];
	}
}
