using System;
using System.Collections.Generic;
using UnityEngine;

public struct RawEndlessWaveConfig
{
	public class Initializer : DataMgr.ConfigInitializer<RawEndlessWaveConfig>
	{
		public override void ApplyResult(List<RawEndlessWaveConfig> result)
		{
			list = result;
			dic = new Dictionary<int, RawEndlessWaveConfig>();
			foreach (RawEndlessWaveConfig item in list)
			{
				dic.Add(item.wave, item);
			}
			RawEndlessWaveConfig rawEndlessWaveConfig = default(RawEndlessWaveConfig);
			rawSpawnInfoList = new List<EndlessBattleSpawnInfo.RawStageSpawnInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				rawSpawnInfoList.Add(rawEndlessWaveConfig.GetInfo(i + 1));
			}
		}
	}

	public static Dictionary<int, RawEndlessWaveConfig> dic;

	public static List<RawEndlessWaveConfig> list;

	public static List<EndlessBattleSpawnInfo.RawStageSpawnInfo> rawSpawnInfoList = new List<EndlessBattleSpawnInfo.RawStageSpawnInfo>();

	public int wave;

	public int dropCount;

	public int duration;

	public float[] unitTypes;

	public int unitTypesCount;

	public RawEndlessWaveConfig GetConfig(int wave)
	{
		if (!dic.ContainsKey(wave))
		{
			Debug.LogError("No Wave:" + wave);
		}
		return dic[wave];
	}

	public EndlessBattleSpawnInfo.RawStageSpawnInfo GetInfo(int wave)
	{
		RawEndlessWaveConfig config = GetConfig(wave);
		EndlessBattleSpawnInfo.RawStageSpawnInfo rawStageSpawnInfo = new EndlessBattleSpawnInfo.RawStageSpawnInfo();
		rawStageSpawnInfo.duration = config.duration;
		rawStageSpawnInfo.dropCount = config.dropCount;
		rawStageSpawnInfo.rawSpawnChances = new List<EndlessBattleSpawnInfo.RawSpawnChance>();
		rawStageSpawnInfo.wave = config.wave;
		Enum.GetValues(typeof(EndlessBattleSpawnInfo.EndlessUnitType));
		for (int i = 0; i < config.unitTypes.Length; i++)
		{
			rawStageSpawnInfo.rawSpawnChances.Add(new EndlessBattleSpawnInfo.RawSpawnChance
			{
				unitGroup = (EndlessBattleSpawnInfo.EndlessUnitGroup)(i + 1),
				count = config.unitTypes[i]
			});
		}
		return rawStageSpawnInfo;
	}
}
