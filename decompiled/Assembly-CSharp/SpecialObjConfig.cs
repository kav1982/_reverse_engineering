using System.Collections.Generic;
using UnityEngine;

public struct SpecialObjConfig
{
	public class Initializer : DataMgr.ConfigInitializer<SpecialObjConfig>
	{
		public override void ApplyResult(List<SpecialObjConfig> result)
		{
			list = result;
			dic = new Dictionary<int, SpecialObjConfig>();
			foreach (SpecialObjConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, SpecialObjConfig> dic;

	public static List<SpecialObjConfig> list;

	public int id;

	public string name;

	public bool isHybirdSO;

	public bool loadArchiveCreate;

	public SpecialObjConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id];
	}
}
