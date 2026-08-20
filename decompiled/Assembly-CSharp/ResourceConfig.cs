using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct ResourceConfig
{
	public class Initializer : DataMgr.ConfigInitializer<ResourceConfig>
	{
		public override void ApplyResult(List<ResourceConfig> result)
		{
			list = result;
			dic = new Dictionary<int, ResourceConfig>();
			foreach (ResourceConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, ResourceConfig> dic;

	public static List<ResourceConfig> list;

	public int id;

	public int priceCoin;

	public int priceHP;

	public ResourceAbilityType abilityType;

	public int int1;

	public string dropSE;

	public string pickSE;

	public static ResourceConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id];
	}

	public string GetName()
	{
		return (id + 6000000).GetText();
	}

	public string GetInfo()
	{
		return new StringBuilder("◆\u00a0\u200a" + (id + 6100000).GetText()).Replace("\\", "\n◆\u00a0\u200a").Replace("int1", int1.ToString()).ToString();
	}
}
