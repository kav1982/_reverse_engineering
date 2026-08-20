using System.Collections.Generic;
using UnityEngine;

public struct HardDialogueConfig
{
	public class Initializer : DataMgr.ConfigInitializer<HardDialogueConfig>
	{
		public override void ApplyResult(List<HardDialogueConfig> result)
		{
			list = result;
			dic = new Dictionary<int, HardDialogueConfig>();
			foreach (HardDialogueConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, HardDialogueConfig> dic;

	public static List<HardDialogueConfig> list;

	public int id;

	public int[] portraits;

	public int[] textIDs;

	public string[] eventStrs;

	public int[] endOptions;

	public bool[] canOptionBackSibling;

	public bool canForceStop;

	public static HardDialogueConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	public HardDialogueConfig Copy()
	{
		HardDialogueConfig result = default(HardDialogueConfig);
		result.id = id;
		result.portraits = portraits;
		result.textIDs = textIDs;
		result.eventStrs = eventStrs;
		return result;
	}
}
