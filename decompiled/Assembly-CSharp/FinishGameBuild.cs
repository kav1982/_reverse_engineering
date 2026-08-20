using System;
using System.Collections.Generic;

[Serializable]
public class FinishGameBuild
{
	public UnitConfig PlayerConfig;

	public long time;

	public string username;

	public int Difficulty;

	public float timeuse;

	public PlayerLook playerLook;

	public int coinCount;

	public int keyCount;

	public int bagCount = 10;

	public int selectedSetID;

	public float moveSpeed;

	public float damageRatio;

	public List<WandConfig> wandCfgs = new List<WandConfig>();

	public List<RelicConfig> relicCfgs = new List<RelicConfig>();

	public List<int> curseIDs = new List<int>();

	public List<int> curseLevels = new List<int>();

	public List<int> potionIDs = new List<int>();

	public List<SlotData> bagSpellDatas = new List<SlotData>();

	public static int SortByDate(FinishGameBuild a, FinishGameBuild b)
	{
		if (a.time < b.time)
		{
			return 1;
		}
		if (a.time > b.time)
		{
			return -1;
		}
		return 0;
	}

	public static int SortByDateReverse(FinishGameBuild a, FinishGameBuild b)
	{
		if (a.time < b.time)
		{
			return -1;
		}
		if (a.time > b.time)
		{
			return 1;
		}
		return 0;
	}

	public static int SortByScore(FinishGameBuild a, FinishGameBuild b)
	{
		if (a.timeuse < b.timeuse)
		{
			return -1;
		}
		if (a.timeuse > b.timeuse)
		{
			return 1;
		}
		return 0;
	}

	public static int SortByScoreReverse(FinishGameBuild a, FinishGameBuild b)
	{
		if (a.timeuse < b.timeuse)
		{
			return 1;
		}
		if (a.timeuse > b.timeuse)
		{
			return -1;
		}
		return 0;
	}
}
