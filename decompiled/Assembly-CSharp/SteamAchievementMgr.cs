using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamAchievementMgr : MonoBehaviour
{
	public static Dictionary<SteamAchievementType, string> achievementDic = new Dictionary<SteamAchievementType, string>
	{
		{
			SteamAchievementType.FirstLaunchGame,
			"first launch game"
		},
		{
			SteamAchievementType.FinishEasy,
			"finish easy"
		},
		{
			SteamAchievementType.FinishNormal,
			"finish normal"
		},
		{
			SteamAchievementType.FinishHard,
			"finish hard"
		},
		{
			SteamAchievementType.FinishNightmare1,
			"finish nightmare"
		},
		{
			SteamAchievementType.FinishNightmare2,
			"finish nightmare2"
		},
		{
			SteamAchievementType.FinishNightmare3,
			"finish nightmare3"
		},
		{
			SteamAchievementType.ProducerKiller,
			"producer killer"
		},
		{
			SteamAchievementType.GetLevel3Spell,
			"get level3 spell"
		},
		{
			SteamAchievementType.GetSpecialSpell,
			"get special spell"
		},
		{
			SteamAchievementType.GetEpicSpell,
			"get epic spell"
		},
		{
			SteamAchievementType.Get15Curse,
			"get 15 curse"
		},
		{
			SteamAchievementType.Coin1000,
			"coin 1000"
		},
		{
			SteamAchievementType.KillSelf,
			"kill self"
		},
		{
			SteamAchievementType.DPS100K,
			"dps100k"
		}
	};

	private static bool isDPS100KFinish = false;

	private static void Unlock(SteamAchievementType achievementType)
	{
		if (SteamUserStats.SetAchievement(achievementDic[achievementType]))
		{
			Debug.Log("\ufffd\ufffdɳɾ\ufffd" + achievementDic[achievementType] + "\ufffdɹ\ufffd");
		}
		else
		{
			Debug.Log("\ufffd\ufffdɳɾ\ufffd" + achievementDic[achievementType] + "ʧ\ufffd\ufffd");
		}
	}

	private static void Upload()
	{
		if (SteamUserStats.StoreStats())
		{
			Debug.Log("\ufffdϴ\ufffd\ufffd\ufffdSteam");
		}
		else
		{
			Debug.Log("\ufffdϴ\ufffd\ufffd\ufffdSteamʧ\ufffd\ufffd");
		}
	}

	public static void UnlockAndUpload(SteamAchievementType achievementType)
	{
		if (DataMgr.settingData.isTouristMode || !SteamManager.Initialized)
		{
			return;
		}
		if (achievementType == SteamAchievementType.DPS100K)
		{
			if (isDPS100KFinish)
			{
				return;
			}
			isDPS100KFinish = true;
		}
		SteamUserStats.GetUserAchievementAndUnlockTime(SteamUser.GetSteamID(), achievementDic[achievementType], out var pbAchieved, out var _);
		if (!pbAchieved)
		{
			Debug.Log("\ufffd\ufffdɳɾ\ufffd" + achievementType);
			Unlock(achievementType);
			Upload();
		}
	}

	public static void ClearAll()
	{
		foreach (KeyValuePair<SteamAchievementType, string> item in achievementDic)
		{
			SteamUserStats.ClearAchievement(item.Value);
		}
		Upload();
	}
}
