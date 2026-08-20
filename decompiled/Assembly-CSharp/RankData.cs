using System;
using Steamworks;

[Serializable]
public class RankData
{
	public CSteamID csteamid;

	public string id;

	public string name;

	public int rank;

	public int score;

	public int scorehard;

	public int scorenightmare;

	public int scoreNewNightmare1;

	public int scoreNewNightmare2;

	public int scoreNewNightmare3;

	public bool includeugc;

	public UGCHandle_t ugc;

	public UGCHandle_t ugchard;

	public UGCHandle_t ugcnightmare;

	public UGCHandle_t ugcNewNightmare1;

	public UGCHandle_t ugcNewNightmare2;

	public UGCHandle_t ugcNewNightmare3;

	public static int SortNormal(RankData a, RankData b)
	{
		if (a.score < b.score)
		{
			return -1;
		}
		if (a.score > b.score)
		{
			return 1;
		}
		return 0;
	}

	public static int SortHard(RankData a, RankData b)
	{
		if (a.scorehard < b.scorehard)
		{
			return -1;
		}
		if (a.scorehard > b.scorehard)
		{
			return 1;
		}
		return 0;
	}

	public static int SortNightmare(RankData a, RankData b)
	{
		if (a.scorenightmare < b.scorenightmare)
		{
			return -1;
		}
		if (a.scorenightmare > b.scorenightmare)
		{
			return 1;
		}
		return 0;
	}

	public static int SortNewNightmare1(RankData a, RankData b)
	{
		if (a.scoreNewNightmare1 < b.scoreNewNightmare1)
		{
			return -1;
		}
		if (a.scoreNewNightmare1 > b.scoreNewNightmare1)
		{
			return 1;
		}
		return 0;
	}

	public static int SortNewNightmare2(RankData a, RankData b)
	{
		if (a.scoreNewNightmare2 < b.scoreNewNightmare2)
		{
			return -1;
		}
		if (a.scoreNewNightmare2 > b.scoreNewNightmare2)
		{
			return 1;
		}
		return 0;
	}

	public static int SortNewNightmare3(RankData a, RankData b)
	{
		if (a.scoreNewNightmare3 < b.scoreNewNightmare3)
		{
			return -1;
		}
		if (a.scoreNewNightmare3 > b.scoreNewNightmare3)
		{
			return 1;
		}
		return 0;
	}
}
