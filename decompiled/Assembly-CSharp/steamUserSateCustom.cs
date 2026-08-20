using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class steamUserSateCustom : MonoBehaviour
{
	private int returnCount;

	public static steamUserSateCustom Inst;

	public Coroutine RequestAllFriendsUserStats;

	public UserStateState UserStateState;

	private Vector2 m_ScrollPos;

	private int m_NumGamesStat;

	private float m_FeetTraveledStat;

	private bool m_AchievedWinOneGame;

	private SteamLeaderboard_t m_SteamLeaderboard;

	private SteamLeaderboardEntries_t m_SteamLeaderboardEntries;

	private Texture2D m_Icon;

	private Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserStatsStored_t> m_UserStatsStored;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	protected Callback<UserStatsUnloaded_t> m_UserStatsUnloaded;

	protected Callback<UserAchievementIconFetched_t> m_UserAchievementIconFetched;

	private CallResult<UserStatsReceived_t> OnUserStatsReceivedCallResult = new CallResult<UserStatsReceived_t>();

	private CallResult<UserStatsReceived_t> OnUserStatsReceivedCallResult2;

	private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult;

	private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult;

	private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;

	private CallResult<NumberOfCurrentPlayers_t> OnNumberOfCurrentPlayersCallResult;

	private CallResult<GlobalAchievementPercentagesReady_t> OnGlobalAchievementPercentagesReadyCallResult;

	private CallResult<LeaderboardUGCSet_t> OnLeaderboardUGCSetCallResult;

	private CallResult<GlobalStatsReceived_t> OnGlobalStatsReceivedCallResult;

	private CallResult<UserStatsReceived_t> userStatsCallResult;

	private Dictionary<CSteamID, CallResult<UserStatsReceived_t>> userStatsCallResults = new Dictionary<CSteamID, CallResult<UserStatsReceived_t>>();

	public void GetAllFriendsUserState(bool onebyone = true)
	{
		if (UserStateState != UserStateState.Downloading)
		{
			if (onebyone)
			{
				if (RequestAllFriendsUserStats == null)
				{
					RequestAllFriendsUserStats = StartCoroutine(IE_GetAllFriendsUserStateOneByOne());
					return;
				}
				StopCoroutine(RequestAllFriendsUserStats);
				RequestAllFriendsUserStats = StartCoroutine(IE_GetAllFriendsUserStateOneByOne());
			}
			else
			{
				RequestStatsForFriends();
			}
		}
		else
		{
			Debug.Log("正在获取数据中,不会重新获取");
		}
	}

	public void RequestStatsForFriends()
	{
		Debug.Log("拉取好友数据");
		returnCount = 0;
		foreach (CSteamID friend in SteamFriendsCustom.friends)
		{
			CallResult<UserStatsReceived_t> callResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);
			userStatsCallResults.Add(friend, callResult);
			SteamAPICall_t hAPICall = SteamUserStats.RequestUserStats(friend);
			callResult.Set(hAPICall);
		}
	}

	public IEnumerator IE_GetAllFriendsUserStateOneByOne()
	{
		EventMgr.FriendsUserStateUpdateStart?.Invoke();
		yield return StartCoroutine(IE_GetMyUserState());
		Debug.Log("开始获取朋友统计");
		if (SteamFriendsCustom.friends.Count > 0)
		{
			returnCount = 0;
			foreach (CSteamID friend in SteamFriendsCustom.friends)
			{
				CallResult<UserStatsReceived_t> callResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);
				SteamAPICall_t hAPICall = SteamUserStats.RequestUserStats(friend);
				callResult.Set(hAPICall);
			}
		}
		else
		{
			Debug.Log("没朋友");
			UserStateState = UserStateState.Idle;
		}
		while (UserStateState != UserStateState.AllComplete)
		{
			yield return new WaitForEndOfFrame();
		}
		RequestAllFriendsUserStats = null;
	}

	public void GetUserStateByID(CSteamID a, bool mydata = false)
	{
		if (UserStateState != UserStateState.Downloading)
		{
			ulong uGCID = GetUGCID(a);
			ulong uGCID2 = GetUGCID(a, DifficultyType.Normal);
			ulong uGCID3 = GetUGCID(a, DifficultyType.Hard);
			ulong uGCID4 = GetUGCID(a, DifficultyType.Nightmare1);
			ulong uGCID5 = GetUGCID(a, DifficultyType.Nightmare2);
			ulong uGCID6 = GetUGCID(a, DifficultyType.Nightmare3);
			if (uGCID != 0L || uGCID2 != 0L || uGCID3 != 0L)
			{
				SteamUserStats.GetUserStat(a, "time_normal", out int pData);
				SteamUserStats.GetUserStat(a, "time_hard", out int pData2);
				SteamUserStats.GetUserStat(a, "time_nightmare", out int pData3);
				SteamUserStats.GetUserStat(a, "time_newNightmare1", out int pData4);
				SteamUserStats.GetUserStat(a, "time_newNightmare2", out int pData5);
				SteamUserStats.GetUserStat(a, "time_newNightmare3", out int pData6);
				RankData rankData = new RankData();
				rankData.csteamid = a;
				rankData.ugc = new UGCHandle_t(uGCID);
				rankData.ugchard = new UGCHandle_t(uGCID2);
				rankData.ugcnightmare = new UGCHandle_t(uGCID3);
				rankData.ugcNewNightmare1 = new UGCHandle_t(uGCID4);
				rankData.ugcNewNightmare2 = new UGCHandle_t(uGCID5);
				rankData.ugcNewNightmare3 = new UGCHandle_t(uGCID6);
				rankData.score = pData;
				rankData.scorehard = pData2;
				rankData.scorenightmare = pData3;
				rankData.scoreNewNightmare1 = pData4;
				rankData.scoreNewNightmare2 = pData5;
				rankData.scoreNewNightmare3 = pData6;
				rankData.name = SteamFriends.GetFriendPersonaName(a);
				bool flag = false;
				for (int i = 0; i < SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count; i++)
				{
					RankData rankData2 = SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas[i];
					if (rankData2.csteamid == rankData.csteamid)
					{
						flag = true;
						if (rankData2.score != rankData.score || rankData2.scorehard != rankData.scorehard || rankData2.scorenightmare != rankData.scorenightmare || rankData2.scoreNewNightmare1 != rankData.scoreNewNightmare1 || rankData2.scoreNewNightmare2 != rankData.scoreNewNightmare2 || rankData2.scoreNewNightmare3 != rankData.scoreNewNightmare3)
						{
							SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Remove(rankData2);
							AddToRankData(rankData);
						}
						break;
					}
				}
				if (!flag)
				{
					AddToRankData(rankData);
				}
			}
		}
		UserStateState = UserStateState.Idle;
		void AddToRankData(RankData newRankData)
		{
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Add(newRankData);
			EventMgr.FriendsUserStateUpdate?.Invoke();
			if (mydata)
			{
				SteamLeadBoardManager.Inst.myrankdata = newRankData;
			}
		}
	}

	public void UploadMyUserState(int score, ulong ugcid, DifficultyType difficulty = DifficultyType.Easy)
	{
		Debug.Log("上传统计数据");
		string text = "normal";
		switch (difficulty)
		{
		case DifficultyType.Easy:
			text = "normal";
			break;
		case DifficultyType.Normal:
			text = "hard";
			break;
		case DifficultyType.Hard:
			text = "nightmare";
			break;
		case DifficultyType.Nightmare1:
			text = "newNightmare1";
			break;
		case DifficultyType.Nightmare2:
			text = "newNightmare2";
			break;
		case DifficultyType.Nightmare3:
			text = "newNightmare3";
			break;
		}
		Debug.Log($"要上传的ugcid{ugcid}");
		GeneralTool.ulongToTwoInts(ugcid, out var part, out var part2);
		SteamUserStats.SetStat("ugc_id1_" + text, part);
		SteamUserStats.SetStat("ugc_id2_" + text, part2);
		SteamUserStats.SetStat("time_" + text, score);
		Debug.Log("上传我的统计数据:" + ugcid);
		if (SteamUserStats.StoreStats())
		{
			Debug.LogWarning("上传到Steam");
			Inst.GetAllFriendsUserState();
		}
		else
		{
			Debug.LogWarning("上传到Steam失败");
		}
	}

	public void GetMyUserState()
	{
		StartCoroutine(IE_GetMyUserState());
	}

	public IEnumerator IE_GetMyUserState(DifficultyType difficulty = DifficultyType.Easy)
	{
		if (UserStateState != UserStateState.Downloading)
		{
			UserStateState = UserStateState.Downloading;
			CSteamID mysteamid = SteamUser.GetSteamID();
			CallResult<UserStatsReceived_t> callResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceivedMine);
			SteamAPICall_t hAPICall = SteamUserStats.RequestUserStats(mysteamid);
			callResult.Set(hAPICall);
			while (UserStateState == UserStateState.Downloading)
			{
				yield return new WaitForEndOfFrame();
			}
			CSteamID cSteamID = mysteamid;
			Debug.Log("我的数据获取完成steamID" + cSteamID.ToString());
			GetUserStateByID(mysteamid, mydata: true);
			yield return new WaitForEndOfFrame();
		}
	}

	public ulong GetUGCID(CSteamID steamid, DifficultyType difficulty = DifficultyType.Easy)
	{
		string text = "normal";
		switch (difficulty)
		{
		case DifficultyType.Easy:
			text = "normal";
			break;
		case DifficultyType.Normal:
			text = "hard";
			break;
		case DifficultyType.Hard:
			text = "nightmare";
			break;
		case DifficultyType.Nightmare1:
			text = "newNightmare1";
			break;
		case DifficultyType.Nightmare2:
			text = "newNightmare2";
			break;
		case DifficultyType.Nightmare3:
			text = "newNightmare3";
			break;
		}
		string friendPersonaName = SteamFriends.GetFriendPersonaName(steamid);
		int pData = 0;
		SteamUserStats.GetUserStat(steamid, "ugc_id1_" + text, out pData);
		if (pData != 0)
		{
			int pData2 = 0;
			SteamUserStats.GetUserStat(steamid, "ugc_id2_" + text, out pData2);
			if (pData2 != 0)
			{
				return GeneralTool.twoIntsToUlong(pData, pData2);
			}
			Debug.Log(friendPersonaName + "没有获取到 ugc_id2_" + text + "或ugc_id2_ + " + text + "不存在");
		}
		return 0uL;
	}

	public void Init()
	{
		if (Inst == null)
		{
			Inst = this;
			if (SteamManager.Initialized)
			{
				SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Clear();
				Inst.GetAllFriendsUserState();
			}
			else
			{
				Debug.Log("Steam未连接，不加载好友数据");
			}
		}
		else
		{
			Object.Destroy(this);
		}
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback, bool bIOFailure)
	{
		returnCount++;
		if (returnCount < SteamFriendsCustom.Inst.FrientCount)
		{
			GetUserStateByID(pCallback.m_steamIDUser);
		}
		else if (returnCount == SteamFriendsCustom.Inst.FrientCount)
		{
			GetUserStateByID(pCallback.m_steamIDUser);
			UserStateState = UserStateState.AllComplete;
			EventMgr.FriendsUserStateUpdateComplete?.Invoke();
		}
	}

	private void OnUserStatsReceivedMine(UserStatsReceived_t pCallback, bool bIOFailure)
	{
		GetUserStateByID(pCallback.m_steamIDUser);
		UserStateState = UserStateState.CompleteOne;
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		Debug.Log("[" + 1102 + " - UserStatsStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	private void OnUserAchievementStored(UserAchievementStored_t pCallback)
	{
		Debug.Log("[" + 1103 + " - UserAchievementStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_bGroupAchievement + " -- " + pCallback.m_rgchAchievementName + " -- " + pCallback.m_nCurProgress + " -- " + pCallback.m_nMaxProgress);
	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1104.ToString(),
			" - LeaderboardFindResult] - ",
			null,
			null,
			null
		};
		SteamLeaderboard_t hSteamLeaderboard = pCallback.m_hSteamLeaderboard;
		obj[3] = hSteamLeaderboard.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_bLeaderboardFound.ToString();
		Debug.Log(string.Concat(obj));
		if (pCallback.m_bLeaderboardFound != 0)
		{
			m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
		}
	}

	private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1105.ToString(),
			" - LeaderboardScoresDownloaded] - ",
			null,
			null,
			null,
			null,
			null
		};
		SteamLeaderboard_t hSteamLeaderboard = pCallback.m_hSteamLeaderboard;
		obj[3] = hSteamLeaderboard.ToString();
		obj[4] = " -- ";
		SteamLeaderboardEntries_t hSteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;
		obj[5] = hSteamLeaderboardEntries.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_cEntryCount.ToString();
		Debug.Log(string.Concat(obj));
		m_SteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;
	}

	private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[14]
		{
			"[",
			1106.ToString(),
			" - LeaderboardScoreUploaded] - ",
			pCallback.m_bSuccess.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		SteamLeaderboard_t hSteamLeaderboard = pCallback.m_hSteamLeaderboard;
		obj[5] = hSteamLeaderboard.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_nScore.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_bScoreChanged.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nGlobalRankNew.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_nGlobalRankPrevious.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1107 + " - NumberOfCurrentPlayers] - " + pCallback.m_bSuccess + " -- " + pCallback.m_cPlayers);
	}

	private void OnUserStatsUnloaded(UserStatsUnloaded_t pCallback)
	{
		string text = 1108.ToString();
		CSteamID steamIDUser = pCallback.m_steamIDUser;
		Debug.Log("[" + text + " - UserStatsUnloaded] - " + steamIDUser.ToString());
	}

	private void OnUserAchievementIconFetched(UserAchievementIconFetched_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			1109.ToString(),
			" - UserAchievementIconFetched] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		CGameID nGameID = pCallback.m_nGameID;
		obj[3] = nGameID.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_rgchAchievementName;
		obj[6] = " -- ";
		obj[7] = pCallback.m_bAchieved.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nIconHandle.ToString();
		Debug.Log(string.Concat(obj));
		m_Icon = SteamUtilsTest.GetSteamImageAsTexture2D(pCallback.m_nIconHandle);
	}

	private void OnGlobalAchievementPercentagesReady(GlobalAchievementPercentagesReady_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1110 + " - GlobalAchievementPercentagesReady] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	private void OnLeaderboardUGCSet(LeaderboardUGCSet_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1111.ToString(),
			" - LeaderboardUGCSet] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		SteamLeaderboard_t hSteamLeaderboard = pCallback.m_hSteamLeaderboard;
		obj[5] = hSteamLeaderboard.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGlobalStatsReceived(GlobalStatsReceived_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1112 + " - GlobalStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}
}
