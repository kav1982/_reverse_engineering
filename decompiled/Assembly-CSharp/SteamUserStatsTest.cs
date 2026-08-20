using Steamworks;
using UnityEngine;

public class SteamUserStatsTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private int m_NumGamesStat;

	private float m_FeetTraveledStat;

	private bool m_AchievedWinOneGame;

	private SteamLeaderboard_t m_SteamLeaderboard;

	private SteamLeaderboardEntries_t m_SteamLeaderboardEntries;

	private Texture2D m_Icon;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserStatsStored_t> m_UserStatsStored;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	protected Callback<UserStatsUnloaded_t> m_UserStatsUnloaded;

	protected Callback<UserAchievementIconFetched_t> m_UserAchievementIconFetched;

	private CallResult<UserStatsReceived_t> OnUserStatsReceivedCallResult;

	private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult;

	private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult;

	private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;

	private CallResult<NumberOfCurrentPlayers_t> OnNumberOfCurrentPlayersCallResult;

	private CallResult<GlobalAchievementPercentagesReady_t> OnGlobalAchievementPercentagesReadyCallResult;

	private CallResult<LeaderboardUGCSet_t> OnLeaderboardUGCSetCallResult;

	private CallResult<GlobalStatsReceived_t> OnGlobalStatsReceivedCallResult;

	public void OnEnable()
	{
		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);
		m_UserStatsUnloaded = Callback<UserStatsUnloaded_t>.Create(OnUserStatsUnloaded);
		m_UserAchievementIconFetched = Callback<UserAchievementIconFetched_t>.Create(OnUserAchievementIconFetched);
		OnUserStatsReceivedCallResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);
		OnLeaderboardFindResultCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
		OnLeaderboardScoresDownloadedCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
		OnLeaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);
		OnNumberOfCurrentPlayersCallResult = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
		OnGlobalAchievementPercentagesReadyCallResult = CallResult<GlobalAchievementPercentagesReady_t>.Create(OnGlobalAchievementPercentagesReady);
		OnLeaderboardUGCSetCallResult = CallResult<LeaderboardUGCSet_t>.Create(OnLeaderboardUGCSet);
		OnGlobalStatsReceivedCallResult = CallResult<GlobalStatsReceived_t>.Create(OnGlobalStatsReceived);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_NumGamesStat: " + m_NumGamesStat);
		GUILayout.Label("m_FeetTraveledStat: " + m_FeetTraveledStat);
		GUILayout.Label("m_AchievedWinOneGame: " + m_AchievedWinOneGame);
		SteamLeaderboard_t steamLeaderboard = m_SteamLeaderboard;
		GUILayout.Label("m_SteamLeaderboard: " + steamLeaderboard.ToString());
		SteamLeaderboardEntries_t steamLeaderboardEntries = m_SteamLeaderboardEntries;
		GUILayout.Label("m_SteamLeaderboardEntries: " + steamLeaderboardEntries.ToString());
		GUILayout.Label("m_Icon:");
		GUILayout.Label(m_Icon);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("RequestCurrentStats()"))
		{
			MonoBehaviour.print("SteamUserStats.RequestCurrentStats() : " + SteamUserStats.RequestCurrentStats());
		}
		GUILayout.Label("GetStat(\"NumGames\", out m_NumGamesStat) : " + SteamUserStats.GetStat("NumGames", out m_NumGamesStat) + " -- " + m_NumGamesStat);
		GUILayout.Label("GetStat(\"FeetTraveled\", out m_FeetTraveledStat) : " + SteamUserStats.GetStat("FeetTraveled", out m_FeetTraveledStat) + " -- " + m_FeetTraveledStat);
		if (GUILayout.Button("SetStat(\"NumGames\", m_NumGamesStat + 1)"))
		{
			bool flag = SteamUserStats.SetStat("NumGames", m_NumGamesStat + 1);
			MonoBehaviour.print("SteamUserStats.SetStat(\"NumGames\", " + m_NumGamesStat + 1 + ") : " + flag);
		}
		if (GUILayout.Button("SetStat(\"FeetTraveled\", m_FeetTraveledStat + 1)"))
		{
			bool flag2 = SteamUserStats.SetStat("FeetTraveled", m_FeetTraveledStat + 1f);
			MonoBehaviour.print("SteamUserStats.SetStat(\"FeetTraveled\", " + m_FeetTraveledStat + 1 + ") : " + flag2);
		}
		if (GUILayout.Button("UpdateAvgRateStat(\"AverageSpeed\", 100, 60.0)"))
		{
			bool flag3 = SteamUserStats.UpdateAvgRateStat("AverageSpeed", 100f, 60.0);
			MonoBehaviour.print("SteamUserStats.UpdateAvgRateStat(\"AverageSpeed\", " + 100 + ", " + 60.0 + ") : " + flag3);
		}
		GUILayout.Label("GetAchievement(\"ACH_WIN_ONE_GAME\", out m_AchievedWinOneGame) : " + SteamUserStats.GetAchievement("ACH_WIN_ONE_GAME", out m_AchievedWinOneGame) + " -- " + m_AchievedWinOneGame);
		if (GUILayout.Button("SetAchievement(\"ACH_WIN_ONE_GAME\")"))
		{
			MonoBehaviour.print("SteamUserStats.SetAchievement(\"ACH_WIN_ONE_GAME\") : " + SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME"));
		}
		if (GUILayout.Button("ClearAchievement(\"ACH_WIN_ONE_GAME\")"))
		{
			MonoBehaviour.print("SteamUserStats.ClearAchievement(\"ACH_WIN_ONE_GAME\") : " + SteamUserStats.ClearAchievement("ACH_WIN_ONE_GAME"));
		}
		bool pbAchieved;
		uint punUnlockTime;
		bool achievementAndUnlockTime = SteamUserStats.GetAchievementAndUnlockTime("ACH_WIN_ONE_GAME", out pbAchieved, out punUnlockTime);
		GUILayout.Label("GetAchievementAndUnlockTime(\"ACH_WIN_ONE_GAME\", out Achieved, out UnlockTime) : " + achievementAndUnlockTime + " -- " + pbAchieved + " -- " + punUnlockTime);
		if (GUILayout.Button("StoreStats()"))
		{
			MonoBehaviour.print("SteamUserStats.StoreStats() : " + SteamUserStats.StoreStats());
		}
		if (GUILayout.Button("GetAchievementIcon(\"ACH_WIN_ONE_GAME\")"))
		{
			int achievementIcon = SteamUserStats.GetAchievementIcon("ACH_WIN_ONE_GAME");
			MonoBehaviour.print("SteamUserStats.GetAchievementIcon(\"ACH_WIN_ONE_GAME\") : " + achievementIcon);
			if (achievementIcon != 0)
			{
				m_Icon = SteamUtilsTest.GetSteamImageAsTexture2D(achievementIcon);
			}
		}
		GUILayout.Label("GetAchievementDisplayAttribute(\"ACH_WIN_ONE_GAME\", \"name\") : " + SteamUserStats.GetAchievementDisplayAttribute("ACH_WIN_ONE_GAME", "name"));
		if (GUILayout.Button("IndicateAchievementProgress(\"ACH_WIN_100_GAMES\", 10, 100)"))
		{
			bool flag4 = SteamUserStats.IndicateAchievementProgress("ACH_WIN_100_GAMES", 10u, 100u);
			MonoBehaviour.print("SteamUserStats.IndicateAchievementProgress(\"ACH_WIN_100_GAMES\", " + 10 + ", " + 100 + ") : " + flag4);
		}
		GUILayout.Label("GetNumAchievements() : " + SteamUserStats.GetNumAchievements());
		GUILayout.Label("GetAchievementName(0) : " + SteamUserStats.GetAchievementName(0u));
		if (GUILayout.Button("RequestUserStats(TestConstants.Instance.k_SteamId_rlabrecque)"))
		{
			SteamAPICall_t steamAPICall_t = SteamUserStats.RequestUserStats(TestConstants.Instance.k_SteamId_rlabrecque);
			OnUserStatsReceivedCallResult.Set(steamAPICall_t);
			CSteamID k_SteamId_rlabrecque = TestConstants.Instance.k_SteamId_rlabrecque;
			string text = k_SteamId_rlabrecque.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamUserStats.RequestUserStats(" + text + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, \"NumWins\", out Data) : " + SteamUserStats.GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, "NumWins", out int pData) + " -- " + pData);
		GUILayout.Label("GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, \"MaxFeetTraveled\", out Data) : " + SteamUserStats.GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, "MaxFeetTraveled", out float pData2) + " -- " + pData2);
		GUILayout.Label("GetUserAchievement(TestConstants.Instance.k_SteamId_rlabrecque, \"ACH_TRAVEL_FAR_ACCUM\", out Achieved) : " + SteamUserStats.GetUserAchievement(TestConstants.Instance.k_SteamId_rlabrecque, "ACH_TRAVEL_FAR_ACCUM", out var pbAchieved2) + " -- " + pbAchieved2);
		bool pbAchieved3;
		uint punUnlockTime2;
		bool userAchievementAndUnlockTime = SteamUserStats.GetUserAchievementAndUnlockTime(TestConstants.Instance.k_SteamId_rlabrecque, "ACH_WIN_ONE_GAME", out pbAchieved3, out punUnlockTime2);
		GUILayout.Label("GetUserAchievementAndUnlockTime(TestConstants.Instance.k_SteamId_rlabrecque, \"ACH_WIN_ONE_GAME\", out Achieved, out UnlockTime) : " + userAchievementAndUnlockTime + " -- " + pbAchieved3 + " -- " + punUnlockTime2);
		if (GUILayout.Button("ResetAllStats(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamUserStats.ResetAllStats(bAchievementsToo: true).ToString(), str0: "SteamUserStats.ResetAllStats(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("FindOrCreateLeaderboard(\"Feet Traveled\", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric)"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamUserStats.FindOrCreateLeaderboard("Feet Traveled", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			OnLeaderboardFindResultCallResult.Set(steamAPICall_t3);
			string[] obj2 = new string[6]
			{
				"SteamUserStats.FindOrCreateLeaderboard(\"Feet Traveled\", ",
				ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending.ToString(),
				", ",
				ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			obj2[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("FindLeaderboard(\"Feet Traveled\")"))
		{
			SteamAPICall_t steamAPICall_t4 = SteamUserStats.FindLeaderboard("Feet Traveled");
			OnLeaderboardFindResultCallResult.Set(steamAPICall_t4);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t4;
			MonoBehaviour.print("SteamUserStats.FindLeaderboard(\"Feet Traveled\") : " + steamAPICall_t2.ToString());
		}
		if (m_SteamLeaderboard != new SteamLeaderboard_t(0uL))
		{
			GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardName(m_SteamLeaderboard));
			GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardEntryCount(m_SteamLeaderboard));
			GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardSortMethod(m_SteamLeaderboard));
			GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardDisplayType(m_SteamLeaderboard));
		}
		else
		{
			GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : ");
		}
		if (GUILayout.Button("DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5)"))
		{
			SteamAPICall_t steamAPICall_t5 = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5);
			OnLeaderboardScoresDownloadedCallResult.Set(steamAPICall_t5);
			string[] obj3 = new string[10] { "SteamUserStats.DownloadLeaderboardEntries(", null, null, null, null, null, null, null, null, null };
			steamLeaderboard = m_SteamLeaderboard;
			obj3[1] = steamLeaderboard.ToString();
			obj3[2] = ", ";
			obj3[3] = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal.ToString();
			obj3[4] = ", ";
			obj3[5] = 1.ToString();
			obj3[6] = ", ";
			obj3[7] = 5.ToString();
			obj3[8] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t5;
			obj3[9] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, Users, Users.Length)"))
		{
			CSteamID[] array = new CSteamID[1] { SteamUser.GetSteamID() };
			SteamAPICall_t steamAPICall_t6 = SteamUserStats.DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, array, array.Length);
			OnLeaderboardScoresDownloadedCallResult.Set(steamAPICall_t6);
			string[] obj4 = new string[8] { "SteamUserStats.DownloadLeaderboardEntriesForUsers(", null, null, null, null, null, null, null };
			steamLeaderboard = m_SteamLeaderboard;
			obj4[1] = steamLeaderboard.ToString();
			obj4[2] = ", ";
			obj4[3] = array?.ToString();
			obj4[4] = ", ";
			obj4[5] = array.Length.ToString();
			obj4[6] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t6;
			obj4[7] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out LeaderboardEntry, null, 0)"))
		{
			LeaderboardEntry_t pLeaderboardEntry;
			bool downloadedLeaderboardEntry = SteamUserStats.GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out pLeaderboardEntry, null, 0);
			string[] obj5 = new string[10] { "SteamUserStats.GetDownloadedLeaderboardEntry(", null, null, null, null, null, null, null, null, null };
			steamLeaderboardEntries = m_SteamLeaderboardEntries;
			obj5[1] = steamLeaderboardEntries.ToString();
			obj5[2] = ", ";
			obj5[3] = 0.ToString();
			obj5[4] = ", out LeaderboardEntry, , ";
			obj5[5] = 0.ToString();
			obj5[6] = ") : ";
			obj5[7] = downloadedLeaderboardEntry.ToString();
			obj5[8] = " -- ";
			obj5[9] = pLeaderboardEntry.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, null, 0)"))
		{
			SteamAPICall_t steamAPICall_t7 = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, null, 0);
			OnLeaderboardScoreUploadedCallResult.Set(steamAPICall_t7);
			string[] obj6 = new string[10] { "SteamUserStats.UploadLeaderboardScore(", null, null, null, null, null, null, null, null, null };
			steamLeaderboard = m_SteamLeaderboard;
			obj6[1] = steamLeaderboard.ToString();
			obj6[2] = ", ";
			obj6[3] = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate.ToString();
			obj6[4] = ", ";
			obj6[5] = ((int)m_FeetTraveledStat).ToString();
			obj6[6] = ", , ";
			obj6[7] = 0.ToString();
			obj6[8] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t7;
			obj6[9] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj6));
		}
		if (GUILayout.Button("AttachLeaderboardUGC(m_SteamLeaderboard, UGCHandle_t.Invalid)"))
		{
			SteamAPICall_t steamAPICall_t8 = SteamUserStats.AttachLeaderboardUGC(m_SteamLeaderboard, UGCHandle_t.Invalid);
			OnLeaderboardUGCSetCallResult.Set(steamAPICall_t8);
			string[] obj7 = new string[6] { "SteamUserStats.AttachLeaderboardUGC(", null, null, null, null, null };
			steamLeaderboard = m_SteamLeaderboard;
			obj7[1] = steamLeaderboard.ToString();
			obj7[2] = ", ";
			UGCHandle_t invalid = UGCHandle_t.Invalid;
			obj7[3] = invalid.ToString();
			obj7[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t8;
			obj7[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj7));
		}
		if (GUILayout.Button("GetNumberOfCurrentPlayers()"))
		{
			SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
			OnNumberOfCurrentPlayersCallResult.Set(numberOfCurrentPlayers);
			SteamAPICall_t steamAPICall_t2 = numberOfCurrentPlayers;
			MonoBehaviour.print("SteamUserStats.GetNumberOfCurrentPlayers() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("RequestGlobalAchievementPercentages()"))
		{
			SteamAPICall_t steamAPICall_t9 = SteamUserStats.RequestGlobalAchievementPercentages();
			OnGlobalAchievementPercentagesReadyCallResult.Set(steamAPICall_t9);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t9;
			MonoBehaviour.print("SteamUserStats.RequestGlobalAchievementPercentages() : " + steamAPICall_t2.ToString());
		}
		int mostAchievedAchievementInfo = SteamUserStats.GetMostAchievedAchievementInfo(out var pchName, 120u, out var pflPercent, out var pbAchieved4);
		if (mostAchievedAchievementInfo != -1)
		{
			GUILayout.Label("GetMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + mostAchievedAchievementInfo + " -- " + pchName + " -- " + pflPercent + " -- " + pbAchieved4);
		}
		else
		{
			GUILayout.Label("GetMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + mostAchievedAchievementInfo);
		}
		if (mostAchievedAchievementInfo != -1)
		{
			mostAchievedAchievementInfo = SteamUserStats.GetNextMostAchievedAchievementInfo(mostAchievedAchievementInfo, out var pchName2, 120u, out var pflPercent2, out var pbAchieved5);
			GUILayout.Label("GetNextMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + mostAchievedAchievementInfo + " -- " + pchName2 + " -- " + pflPercent2 + " -- " + pbAchieved5);
		}
		GUILayout.Label("GetAchievementAchievedPercent(\"ACH_WIN_100_GAMES\", out Percent) : " + SteamUserStats.GetAchievementAchievedPercent("ACH_WIN_100_GAMES", out var pflPercent3) + " -- " + pflPercent3);
		if (GUILayout.Button("RequestGlobalStats(3)"))
		{
			SteamAPICall_t steamAPICall_t10 = SteamUserStats.RequestGlobalStats(3);
			OnGlobalStatsReceivedCallResult.Set(steamAPICall_t10);
			string text2 = 3.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t10;
			MonoBehaviour.print("SteamUserStats.RequestGlobalStats(" + text2 + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("GetGlobalStat(\"\", out Data) : " + SteamUserStats.GetGlobalStat("", out double pData3) + " -- " + pData3);
		long[] array2 = new long[1];
		int globalStatHistory = SteamUserStats.GetGlobalStatHistory("", array2, (uint)array2.Length);
		if (globalStatHistory != 0)
		{
			GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)array2.Length + ") : " + globalStatHistory + " -- " + array2[0]);
		}
		else
		{
			GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)array2.Length + ") : " + globalStatHistory + " -- ");
		}
		double[] array3 = new double[1];
		int globalStatHistory2 = SteamUserStats.GetGlobalStatHistory("", array3, (uint)array3.Length);
		if (globalStatHistory2 != 0)
		{
			GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)array3.Length + ") : " + globalStatHistory2 + " -- " + array3[0]);
		}
		else
		{
			GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)array3.Length + ") : " + globalStatHistory2 + " -- ");
		}
		int pnMinProgress;
		int pnMaxProgress;
		bool achievementProgressLimits = SteamUserStats.GetAchievementProgressLimits("ACH_WIN_100_GAMES", out pnMinProgress, out pnMaxProgress);
		GUILayout.Label("GetAchievementProgressLimits(\"ACH_WIN_100_GAMES\", out MinProgress, out MaxProgress) : " + achievementProgressLimits + " -- " + pnMinProgress + " -- " + pnMaxProgress);
		float pfMinProgress;
		float pfMaxProgress;
		bool achievementProgressLimits2 = SteamUserStats.GetAchievementProgressLimits("ACH_TRAVEL_FAR_ACCUM", out pfMinProgress, out pfMaxProgress);
		GUILayout.Label("GetAchievementProgressLimits(\"ACH_TRAVEL_FAR_ACCUM\", out MinProgress, out MaxProgress) : " + achievementProgressLimits2 + " -- " + pfMinProgress + " -- " + pfMaxProgress);
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			1101.ToString(),
			" - UserStatsReceived] - ",
			pCallback.m_nGameID.ToString(),
			" -- ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		CSteamID steamIDUser = pCallback.m_steamIDUser;
		obj[7] = steamIDUser.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1101.ToString(),
			" - UserStatsReceived] - ",
			pCallback.m_nGameID.ToString(),
			" -- ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		CSteamID steamIDUser = pCallback.m_steamIDUser;
		obj[7] = steamIDUser.ToString();
		Debug.Log(string.Concat(obj));
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
