using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class RelicGroupConfig
{
	public class Initializer : DataMgr.ConfigInitializer<RelicGroupConfig>
	{
		public override void ApplyResult(List<RelicGroupConfig> result)
		{
			list = result;
			dic = new Dictionary<int, RelicGroupConfig>();
			foreach (RelicGroupConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, RelicGroupConfig> dic = new Dictionary<int, RelicGroupConfig>();

	public static List<RelicGroupConfig> list = new List<RelicGroupConfig>();

	public int id;

	public RelicGroupAbilityType abilityType;

	public int[] items;

	public ItemAbilityInt int1;

	public ItemAbilityInt int2;

	public ItemAbilityInt int3;

	public ItemAbilityFloat float1;

	public ItemAbilityFloat float2;

	public ItemAbilityFloat float3;

	public static int? GetRelicGroupIdByRelicId(int relicId)
	{
		foreach (RelicGroupConfig item in list)
		{
			if (item.items.Contains(relicId))
			{
				return item.id;
			}
		}
		return null;
	}

	public static bool TryGetRelicGroupIdByRelicId(int relicId, out int groupId)
	{
		int? relicGroupIdByRelicId = GetRelicGroupIdByRelicId(relicId);
		groupId = relicGroupIdByRelicId ?? (-1);
		return relicGroupIdByRelicId.HasValue;
	}

	public bool CheckRelicGroupIsActive(IEnumerable<int> relicIds)
	{
		return relicIds.Where((int e) => items.Contains(e)).ToHashSet().Count == items.Length;
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Clear()
	{
		dic.Clear();
		list.Clear();
	}

	public RelicGroupConfig Copy()
	{
		return new RelicGroupConfig
		{
			id = id,
			abilityType = abilityType,
			items = items,
			int1 = int1,
			int2 = int2,
			int3 = int3,
			float1 = float1,
			float2 = float2,
			float3 = float3
		};
	}

	public void CalculateAbility()
	{
		int1.RandomResult(1);
		int2.RandomResult(1);
		int3.RandomResult(1);
		float1.RandomResult(1);
		float2.RandomResult(1);
		float3.RandomResult(1);
	}

	public string GetName()
	{
		return (15000000 + id).GetText();
	}

	public string GetDesc()
	{
		return (15100000 + id).GetText().Replace("int1", int1.result.ToString()).Replace("int2", int2.result.ToString())
			.Replace("int3", int3.result.ToString())
			.Replace("float1", float1.result.ToString(CultureInfo.InvariantCulture))
			.Replace("float2", float2.result.ToString(CultureInfo.InvariantCulture))
			.Replace("float3", float3.result.ToString(CultureInfo.InvariantCulture));
	}
}
