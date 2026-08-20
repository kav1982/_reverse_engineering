using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBattleSpawnInfo
{
	[Serializable]
	public class StageSpawnInfo
	{
		public List<SpawnChance> spawnChances = new List<SpawnChance>();

		public int dropCount;

		public float duration;

		public StageSpawnInfo Copy()
		{
			return new StageSpawnInfo
			{
				spawnChances = spawnChances,
				dropCount = dropCount,
				duration = duration
			};
		}

		public float GetSummonChance(EndlessUnitType unitType)
		{
			float result = 0f;
			for (int i = 0; i < spawnChances.Count; i++)
			{
				if (spawnChances[i].unitType == unitType)
				{
					result = spawnChances[i].chance;
					break;
				}
			}
			return result;
		}

		public void RemoveSpawnUnit(EndlessUnitGroup belongGroup)
		{
			List<EndlessUnitType> list = new List<EndlessUnitType>();
			for (int i = 0; i < spawnChances.Count; i++)
			{
				if (spawnChances[i].unitGroup == belongGroup)
				{
					list.Add(spawnChances[i].unitType);
				}
			}
			if (list.Count <= 0)
			{
				return;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			for (int j = 0; j < spawnChances.Count; j++)
			{
				if (spawnChances[j].unitType == list[index])
				{
					spawnChances.RemoveAt(j);
					break;
				}
			}
		}

		public void AddSpawnUnit(EndlessUnitGroup belongGroup)
		{
			List<EndlessUnitType> list = new List<EndlessUnitType>();
			List<EndlessUnitType> list2 = new List<EndlessUnitType>();
			for (int i = 0; i < spawnChances.Count; i++)
			{
				list2.Add(spawnChances[i].unitType);
			}
			for (int j = 0; j < EndlessUnitConfig.list.Count; j++)
			{
				EndlessUnitConfig endlessUnitConfig = EndlessUnitConfig.list[j];
				if (endlessUnitConfig.groupType != (int)belongGroup || endlessUnitConfig.stage > BattleMgr.Inst.EndlessCurrentStage)
				{
					continue;
				}
				int id = EndlessUnitConfig.list[j].id;
				if (id % 10 == 2)
				{
					EndlessUnitType item = (EndlessUnitType)(id - 1);
					if (!list2.Contains(item))
					{
						continue;
					}
				}
				list.Add((EndlessUnitType)EndlessUnitConfig.list[j].id);
			}
			for (int k = 0; k < spawnChances.Count; k++)
			{
				for (int l = 0; l < list.Count; l++)
				{
					if (spawnChances[k].unitType == list[l])
					{
						list.Remove(spawnChances[k].unitType);
						break;
					}
				}
			}
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				SpawnChance spawnChance = default(SpawnChance);
				spawnChance.unitType = list[index];
				spawnChance.unitGroup = belongGroup;
				spawnChance.chance = EndlessUnitConfig.dic[(int)list[index]].chance;
				SpawnChance item2 = spawnChance;
				spawnChances.Add(item2);
			}
		}

		public void ReplaceSpawnUnit(EndlessUnitGroup belongGroup, float replaceCount)
		{
			List<EndlessUnitType> list = new List<EndlessUnitType>();
			List<EndlessUnitType> list2 = new List<EndlessUnitType>();
			for (int i = 0; i < spawnChances.Count; i++)
			{
				list2.Add(spawnChances[i].unitType);
			}
			for (int j = 0; j < EndlessUnitConfig.list.Count; j++)
			{
				int id = EndlessUnitConfig.list[j].id;
				if (id % 10 == 2)
				{
					EndlessUnitType item = (EndlessUnitType)(id - 1);
					if (!list2.Contains(item))
					{
						continue;
					}
				}
				EndlessUnitConfig endlessUnitConfig = EndlessUnitConfig.list[j];
				if (endlessUnitConfig.groupType == (int)belongGroup && endlessUnitConfig.stage <= BattleMgr.Inst.EndlessCurrentStage)
				{
					list.Add((EndlessUnitType)EndlessUnitConfig.list[j].id);
				}
			}
			for (int k = 0; k < spawnChances.Count; k++)
			{
				for (int l = 0; l < list.Count; l++)
				{
					if (spawnChances[k].unitType == list[l])
					{
						list.Remove(spawnChances[k].unitType);
						break;
					}
				}
			}
			int num = (int)Mathf.Min(list.Count, replaceCount, spawnChances.Count);
			if (num > 0)
			{
				for (int m = 0; m < num; m++)
				{
					int index = UnityEngine.Random.Range(0, spawnChances.Count);
					spawnChances.RemoveAt(index);
				}
				for (int n = 0; n < num; n++)
				{
					int index2 = UnityEngine.Random.Range(0, list.Count);
					SpawnChance spawnChance = default(SpawnChance);
					spawnChance.unitType = list[index2];
					spawnChance.unitGroup = belongGroup;
					spawnChance.chance = EndlessUnitConfig.dic[(int)list[index2]].chance;
					SpawnChance item2 = spawnChance;
					spawnChances.Add(item2);
				}
			}
		}
	}

	[Serializable]
	public struct SpawnChance
	{
		public EndlessUnitType unitType;

		public EndlessUnitGroup unitGroup;

		public float chance;
	}

	[Serializable]
	public struct DropCounts
	{
		public int unitID;

		public int dropCount;
	}

	public enum EndlessUnitType
	{
		None = 0,
		M_Walker = 130101,
		M_Shooter = 130201,
		M_Charger = 130301,
		E_HeavyCharger = 130302,
		M_Fly = 130401,
		E_Fly = 130402,
		E_Coil = 130501,
		E_Turret = 130601,
		M_SpeedWalker = 130701,
		M_Cannon = 130901,
		E_HeavyCannon = 130902,
		M_Jumper = 131001,
		E_HeavyJumper = 131002,
		M_Laser = 131101,
		E_Laser = 131102,
		M_Tp1 = 131201,
		E_Tp2 = 131202,
		M_FireBomb = 131301,
		M_Repair = 131401,
		E_Shield = 131501,
		E_Buff = 131601,
		M_Helicopter = 131701,
		E_Helicopter = 131702,
		M_Team = 131801,
		M_Wanderer = 131901,
		E_ChestDrone = 132001,
		M_ExplosionDrone = 132101,
		M_RepairDrone = 132201,
		M_RandomWalker = 132501,
		M_SnakeWalker = 132601,
		M_MissileLancher = 132701
	}

	public enum EndlessUnitGroup
	{
		Unused,
		M_Base,
		E_Base,
		M_Melee,
		E_Melee,
		M_Shooter,
		E_Shooter,
		Buff
	}

	[Serializable]
	public struct RawSpawnChance
	{
		public EndlessUnitGroup unitGroup;

		public float count;
	}

	public class RawStageSpawnInfo
	{
		public List<RawSpawnChance> rawSpawnChances = new List<RawSpawnChance>();

		public int dropCount;

		public int duration;

		public int wave;

		public RawStageSpawnInfo Copy()
		{
			return new RawStageSpawnInfo
			{
				rawSpawnChances = rawSpawnChances,
				dropCount = dropCount,
				duration = duration,
				wave = wave
			};
		}

		public List<RawSpawnChance> GetChanges(RawStageSpawnInfo originRawInfo)
		{
			List<RawSpawnChance> list = new List<RawSpawnChance>();
			if (originRawInfo.rawSpawnChances.Count == 0)
			{
				for (int i = 0; i < rawSpawnChances.Count; i++)
				{
					if (rawSpawnChances[i].count > 0f)
					{
						list.Add(new RawSpawnChance
						{
							unitGroup = rawSpawnChances[i].unitGroup,
							count = rawSpawnChances[i].count
						});
					}
				}
			}
			else
			{
				for (int j = 0; j < rawSpawnChances.Count; j++)
				{
					if (originRawInfo.rawSpawnChances[j].count != rawSpawnChances[j].count)
					{
						RawSpawnChance rawSpawnChance = default(RawSpawnChance);
						rawSpawnChance.unitGroup = rawSpawnChances[j].unitGroup;
						rawSpawnChance.count = rawSpawnChances[j].count - originRawInfo.rawSpawnChances[j].count;
						RawSpawnChance item = rawSpawnChance;
						list.Add(item);
					}
				}
			}
			return list;
		}

		public StageSpawnInfo GetEndlessWaveConfig(StageSpawnInfo originSpawnInfo)
		{
			if (originSpawnInfo == null)
			{
				originSpawnInfo = new StageSpawnInfo();
			}
			StageSpawnInfo stageSpawnInfo = new StageSpawnInfo
			{
				dropCount = dropCount,
				duration = duration,
				spawnChances = originSpawnInfo.spawnChances
			};
			int num = wave - 1;
			if (num > 0 && num % 5 == 1 && num != 0 && num > 1)
			{
				num--;
			}
			RawStageSpawnInfo originRawInfo = new RawStageSpawnInfo();
			if (num >= 0 && originSpawnInfo.spawnChances.Count > 0)
			{
				originRawInfo = RawEndlessWaveConfig.rawSpawnInfoList[num - 1];
			}
			List<RawSpawnChance> changes = GetChanges(originRawInfo);
			for (int i = 0; i < changes.Count; i++)
			{
				int num2 = (int)Mathf.Abs(changes[i].count);
				for (int j = 0; j < num2; j++)
				{
					if (changes[i].count > 0f)
					{
						stageSpawnInfo.AddSpawnUnit(changes[i].unitGroup);
					}
					else
					{
						stageSpawnInfo.RemoveSpawnUnit(changes[i].unitGroup);
					}
				}
			}
			if (changes.Count == 0)
			{
				int num3 = 1;
				if (BattleMgr.Inst.EndlessCurrentStage > 4)
				{
					num3 = 2;
				}
				for (int k = 0; k < num3; k++)
				{
					EndlessUnitGroup endlessUnitGroup = (EndlessUnitGroup)UnityEngine.Random.Range(1, 8);
					bool flag = false;
					for (int l = 0; l < changes.Count; l++)
					{
						if (changes[l].unitGroup == endlessUnitGroup)
						{
							flag = true;
							RawSpawnChance value = changes[l];
							value.count += 1f;
							changes[l] = value;
							break;
						}
					}
					if (!flag)
					{
						changes.Add(new RawSpawnChance
						{
							unitGroup = endlessUnitGroup,
							count = 1f
						});
					}
				}
				for (int m = 0; m < changes.Count; m++)
				{
					stageSpawnInfo.ReplaceSpawnUnit(changes[m].unitGroup, changes[m].count);
				}
			}
			return stageSpawnInfo;
		}
	}

	public static List<int> SpawnInfoToIDList(StageSpawnInfo info)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < info.spawnChances.Count; i++)
		{
			list.Add((int)info.spawnChances[i].unitType);
		}
		return list;
	}

	public static StageSpawnInfo IDListToSpawnInfo(List<int> idList, int level)
	{
		StageSpawnInfo stageSpawnInfo = new StageSpawnInfo();
		for (int i = 0; i < idList.Count; i++)
		{
			EndlessUnitConfig endlessUnitConfig = EndlessUnitConfig.dic[idList[i]];
			stageSpawnInfo.spawnChances.Add(new SpawnChance
			{
				unitType = (EndlessUnitType)endlessUnitConfig.id,
				unitGroup = (EndlessUnitGroup)endlessUnitConfig.groupType,
				chance = endlessUnitConfig.chance
			});
		}
		level = Mathf.Clamp(level - 1, 0, RawEndlessWaveConfig.list.Count - 1);
		stageSpawnInfo.dropCount = RawEndlessWaveConfig.list[level].dropCount;
		stageSpawnInfo.duration = RawEndlessWaveConfig.list[level].duration;
		return stageSpawnInfo;
	}
}
