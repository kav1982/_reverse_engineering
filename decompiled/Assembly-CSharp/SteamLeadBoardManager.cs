using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class SteamLeadBoardManager : MonoBehaviour
{
	public enum LeaderboardTypes
	{
		Timed,
		Score
	}

	public class UpdateEvent : UnityEvent<bool>
	{
	}

	public struct EndlessLeaderboardEntryData
	{
		public ulong steamID;

		public string steamName;

		public int globalRank;

		public int score;
	}

	private UGCHandle_t m_UGCHandle;

	public LeadboardData leadboardData = new LeadboardData();

	public LeadboardData leadboardDataFriends = new LeadboardData();

	public RankData myrankdata;

	public static SteamLeadBoardManager Inst;

	public bool updating = true;

	public LeaderboardTypes type;

	public UGCState ugcstate;

	public bool gettingleaderboard;

	public bool downloadingleaderboards;

	public bool leaderboardsSuceed;

	public bool downloadingmyleaderboard;

	public bool leaderboardMineSuceed;

	public bool sharing;

	public bool downloadingUGC;

	public string downloadedUGCpath;

	public float timelimit = 10f;

	private float _time;

	public int int_currentleaderboard;

	public List<LeaderBoardCustom> leaderboards = new List<LeaderBoardCustom>
	{
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteNormalLeaderBoard"
		},
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteHardLeaderBoard"
		},
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteNightmareLeaderBoard"
		},
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteNewNightmare1LeaderBoard"
		},
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteNewNightmare2LeaderBoard"
		},
		new LeaderBoardCustom
		{
			leaderboardname = "QuickCompleteNewNightmare3LeaderBoard"
		}
	};

	private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate;

	private SteamLeaderboardEntries_t s_entriesTopTen;

	public LeaderboardEntry_t[] entriesTopTen;

	private SteamLeaderboardEntries_t s_entriesAroundUser;

	public LeaderboardEntry_t[] entriesAroundUser;

	private SteamLeaderboardEntries_t s_userEntry;

	public LeaderboardEntry_t entryUser;

	[HideInInspector]
	public bool s_initialized;

	private CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();

	private CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();

	private CallResult<LeaderboardScoresDownloaded_t> m_downloadResultTopTen = new CallResult<LeaderboardScoresDownloaded_t>();

	private CallResult<LeaderboardScoresDownloaded_t> m_downloadResultAroundUser = new CallResult<LeaderboardScoresDownloaded_t>();

	private CallResult<LeaderboardScoresDownloaded_t> m_downloadResultUser = new CallResult<LeaderboardScoresDownloaded_t>();

	private CallResult<RemoteStorageFileShareResult_t> OnRemoteStorageFileShareResultCallResult = new CallResult<RemoteStorageFileShareResult_t>();

	private CallResult<LeaderboardUGCSet_t> OnLeaderboardUGCSetCallResult = new CallResult<LeaderboardUGCSet_t>();

	private CallResult<RemoteStorageDownloadUGCResult_t> OnRemoteStorageDownloadUGCResultCallResult = new CallResult<RemoteStorageDownloadUGCResult_t>();

	private CallResult<LeaderboardFindResult_t> findLeadboard = new CallResult<LeaderboardFindResult_t>();

	[HideInInspector]
	public UpdateEvent OnLeaderboardUpdate = new UpdateEvent();

	private bool IsUploadingEndlessLeaderboard;

	private const string EndlessLeaderboardName = "EndlessLeaderboard";

	private int m_ScoreToUpload;

	private CallResult<LeaderboardFindResult_t> m_FindEndlessLeaderboardCallResult;

	private CallResult<LeaderboardScoreUploaded_t> m_UploadEndlessScoreCallResult;

	private Action<int> OnGetRankingCallBack;

	private bool IsDownloadingEndlessLeaderboard;

	private bool IsGettingRank;

	private CallResult<LeaderboardFindResult_t> m_FindLeaderboardForDownloadCallResult;

	private CallResult<LeaderboardScoresDownloaded_t> m_DownloadEndlessScoreCallResult;

	private Action<List<EndlessLeaderboardEntryData>> OnGetTop50Callback;

	private Timer timer1;

	public SteamLeaderboard_t GetLeadBoard(int index)
	{
		return leaderboards[index].leaderboard;
	}

	private LeaderboardEntry_t[] GetDownloadedEntries(SteamLeaderboardEntries_t entries, int count)
	{
		leadboardData.rankDatas.Clear();
		LeaderboardEntry_t[] array = new LeaderboardEntry_t[count];
		for (int i = 0; i < count; i++)
		{
			SteamUserStats.GetDownloadedLeaderboardEntry(entries, i, out var pLeaderboardEntry, new int[0], 0);
			array[i] = pLeaderboardEntry;
			RankData rankData = new RankData();
			rankData.csteamid = pLeaderboardEntry.m_steamIDUser;
			rankData.id = pLeaderboardEntry.m_steamIDUser.ToString();
			rankData.score = pLeaderboardEntry.m_nScore;
			rankData.rank = pLeaderboardEntry.m_nGlobalRank;
			rankData.ugc = pLeaderboardEntry.m_hUGC;
			leadboardData.rankDatas.Add(rankData);
		}
		StartCoroutine(GetAllRankName());
		return array;
	}

	private LeaderboardEntry_t GetDownloadedMyEntry(SteamLeaderboardEntries_t entries, int count)
	{
		LeaderboardEntry_t pLeaderboardEntry = default(LeaderboardEntry_t);
		SteamUserStats.GetDownloadedLeaderboardEntry(entries, 0, out pLeaderboardEntry, new int[0], 0);
		new RankData();
		myrankdata.csteamid = pLeaderboardEntry.m_steamIDUser;
		myrankdata.id = pLeaderboardEntry.m_steamIDUser.ToString();
		myrankdata.score = pLeaderboardEntry.m_nScore;
		myrankdata.rank = pLeaderboardEntry.m_nGlobalRank;
		myrankdata.ugc = pLeaderboardEntry.m_hUGC;
		myrankdata.name = SteamFriends.GetFriendPersonaName(myrankdata.csteamid);
		EventMgr.GetMyRank?.Invoke();
		return pLeaderboardEntry;
	}

	public void Init()
	{
		EventMgr.SteamConected = (Action)Delegate.Combine(EventMgr.SteamConected, new Action(SteamConected));
		EventMgr.GetUserName = (Action)Delegate.Combine(EventMgr.GetUserName, new Action(GetUserNameComplete));
		if (Inst == null)
		{
			Inst = this;
		}
	}

	public void OnDisable()
	{
		EventMgr.SteamConected = (Action)Delegate.Remove(EventMgr.SteamConected, new Action(SteamConected));
		EventMgr.GetUserName = (Action)Delegate.Remove(EventMgr.GetUserName, new Action(GetUserNameComplete));
	}

	public UGCHandle_t GetCurrentUGCHandle()
	{
		return m_UGCHandle;
	}

	public void SteamConected()
	{
		_ = SteamManager.Initialized;
	}

	public void FixedUpdate()
	{
		if (downloadingleaderboards || gettingleaderboard)
		{
			_time += Time.fixedDeltaTime;
		}
	}

	private void Start()
	{
		m_FindEndlessLeaderboardCallResult = CallResult<LeaderboardFindResult_t>.Create(OnEndlessLeaderboardFound);
		m_UploadEndlessScoreCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnUploadEndlessScore);
		m_FindLeaderboardForDownloadCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFoundForDownload);
		m_DownloadEndlessScoreCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
	}

	public void UploadScore(int index, int score)
	{
		if (!s_initialized)
		{
			Debug.Log("Can't upload to the leaderboard because isn't loaded yet");
			return;
		}
		bool flag = true;
		if (entryUser.m_nScore != 0)
		{
			if (type == LeaderboardTypes.Timed && entryUser.m_nScore < score)
			{
				flag = false;
			}
			if (type == LeaderboardTypes.Score && entryUser.m_nScore > score)
			{
				flag = false;
			}
		}
		if (flag)
		{
			Debug.Log("uploading score(" + score + ") to steam leaderboard(" + leaderboards[index].leaderboardname + ")");
			SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(leaderboards[index].leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, new int[0], 0);
			m_uploadResult.Set(hAPICall, OnLeaderboardUploadResult);
		}
		else
		{
			Debug.Log("better score exists, not uploading new score");
		}
	}

	public bool UploadScoreAndBuild(int score, int index = 0)
	{
		if (!s_initialized)
		{
			Debug.Log("Can't upload to the leaderboard because isn't loaded yet");
		}
		else
		{
			bool flag = true;
			if (entryUser.m_nScore != 0)
			{
				if (type == LeaderboardTypes.Timed && entryUser.m_nScore < score)
				{
					flag = false;
				}
				if (type == LeaderboardTypes.Score && entryUser.m_nScore > score)
				{
					flag = false;
				}
			}
			if (flag)
			{
				int_currentleaderboard = index;
				Debug.Log("uploading score(" + score + ") to steam leaderboard(" + leaderboards[index].leaderboardname + ")");
				SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(leaderboards[index].leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, new int[0], 0);
				m_uploadResult.Set(hAPICall, OnLeaderboardUploadResult);
				return true;
			}
			Debug.Log("better score exists, not uploading new score");
		}
		return false;
	}

	public void UploadScoreForceOverWride(int index, int score)
	{
		if (!s_initialized)
		{
			Debug.Log("Can't upload to the leaderboard because isn't loaded yet");
			return;
		}
		bool flag = true;
		if (entryUser.m_nScore != 0)
		{
			if (type == LeaderboardTypes.Timed && entryUser.m_nScore < score)
			{
				flag = false;
			}
			if (type == LeaderboardTypes.Score && entryUser.m_nScore > score)
			{
				flag = false;
			}
		}
		if (flag)
		{
			Debug.Log("uploading score(" + score + ") to steam leaderboard(" + leaderboards[index].leaderboardname + ")");
			SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(leaderboards[index].leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, score, new int[0], 0);
			m_uploadResult.Set(hAPICall, OnLeaderboardUploadResult);
		}
		else
		{
			Debug.Log("uploading score(" + score + ") to steam leaderboard(" + leaderboards[index].leaderboardname + ")");
			SteamAPICall_t hAPICall2 = SteamUserStats.UploadLeaderboardScore(leaderboards[index].leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, score, new int[0], 0);
			m_uploadResult.Set(hAPICall2, OnLeaderboardUploadResult);
			Debug.Log("better score exists, not uploading new score");
		}
	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
	{
		gettingleaderboard = false;
		if (pCallback.m_bLeaderboardFound == 0)
		{
			Debug.Log("没有找到排行榜");
			if ((bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen)
			{
				GameUISingletonMono<UI_RankingList>.Inst._Close();
			}
		}
		else
		{
			Debug.Log("找到了排行榜");
		}
		Debug.Log(" leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
		leaderboards[int_currentleaderboard].leaderboard = pCallback.m_hSteamLeaderboard;
		s_initialized = true;
	}

	private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
	{
		Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
		if (!failure)
		{
			DownloadEntries(int_currentleaderboard);
		}
	}

	private void OnLeaderboardFindTopTen(LeaderboardScoresDownloaded_t pCallback, bool failure)
	{
		if (pCallback.m_cEntryCount == 0)
		{
			downloadingleaderboards = false;
			leaderboardsSuceed = true;
			Debug.Log("排行榜没有数据");
		}
		else
		{
			s_entriesTopTen = pCallback.m_hSteamLeaderboardEntries;
			entriesTopTen = GetDownloadedEntries(s_entriesTopTen, pCallback.m_cEntryCount);
			OnLeaderboardUpdate.Invoke(failure);
		}
	}

	private void OnLeaderboardFindAroundUser(LeaderboardScoresDownloaded_t pCallback, bool failure)
	{
		Debug.Log("STEAM LEADERBOARDS: found " + pCallback.m_cEntryCount + " entries around user with failure: " + failure);
		s_entriesAroundUser = pCallback.m_hSteamLeaderboardEntries;
		entriesAroundUser = GetDownloadedEntries(s_entriesAroundUser, pCallback.m_cEntryCount);
	}

	private void OnLeaderboardFindUser(LeaderboardScoresDownloaded_t pCallback, bool failure)
	{
		if (pCallback.m_cEntryCount == 0)
		{
			Debug.Log("我的排行榜数据下载失败");
			downloadingmyleaderboard = false;
			leaderboardMineSuceed = false;
			return;
		}
		s_userEntry = pCallback.m_hSteamLeaderboardEntries;
		if (pCallback.m_cEntryCount == 1)
		{
			entryUser = GetDownloadedMyEntry(s_userEntry, pCallback.m_cEntryCount);
		}
		else
		{
			failure = true;
		}
	}

	public IEnumerator GetallLeaderBoard()
	{
		if (!SteamManager.Initialized || Inst.gettingleaderboard)
		{
			yield break;
		}
		for (int i = 0; i < leaderboards.Count; i++)
		{
			int_currentleaderboard = i;
			Inst.TestLeaderBoard(i);
			while (gettingleaderboard)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		int_currentleaderboard = 0;
		Debug.Log("获取所有排行榜");
	}

	public void TestLeaderBoard(int index = 0)
	{
		Debug.Log("尝试获取排行榜");
		gettingleaderboard = true;
		SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard(leaderboards[index].leaderboardname, ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds);
		findLeadboard.Set(hAPICall, OnLeaderboardFindResult);
	}

	public void DownloadEntries(int start = 0, int end = 9, int index = 0)
	{
		Debug.Log("尝试下载排行榜" + index + "," + start + "-" + end);
		downloadingleaderboards = true;
		leaderboardsSuceed = false;
		if (s_initialized)
		{
			SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboards[index].leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, start, end);
			m_downloadResultTopTen.Set(hAPICall, OnLeaderboardFindTopTen);
		}
	}

	public void DownloadEntriesForUser(int index = 0)
	{
		Debug.Log("尝试下载排行榜" + index);
		downloadingmyleaderboard = true;
		leaderboardMineSuceed = false;
		if (s_initialized)
		{
			SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntriesForUsers(leaderboards[index].leaderboard, new CSteamID[1] { SteamUser.GetSteamID() }, 1);
			m_downloadResultUser.Set(hAPICall, OnLeaderboardFindUser);
		}
	}

	public void StartUploadEndlessScore(int score, Action<int> callback)
	{
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Steam Manager 未初始化！");
			return;
		}
		if (IsUploadingEndlessLeaderboard)
		{
			Debug.LogWarning("已有排行上传任务在进行中...");
			return;
		}
		IsUploadingEndlessLeaderboard = true;
		m_ScoreToUpload = score;
		Debug.Log("[Steam] 开始寻找或创建排行榜: EndlessLeaderboard");
		SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard("EndlessLeaderboard", ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
		m_FindEndlessLeaderboardCallResult.Set(hAPICall);
		OnGetRankingCallBack = callback;
	}

	private void OnEndlessLeaderboardFound(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		if (bIOFailure || pCallback.m_bLeaderboardFound == 0)
		{
			Debug.LogError("[Steam] 查找/创建排行榜失败。");
			IsUploadingEndlessLeaderboard = false;
			return;
		}
		SteamLeaderboard_t hSteamLeaderboard = pCallback.m_hSteamLeaderboard;
		Debug.Log("[Steam] 排行榜创建/获取成功，开始上传分数...");
		SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(hSteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, m_ScoreToUpload, new int[0], 0);
		m_UploadEndlessScoreCallResult.Set(hAPICall);
	}

	private void OnUploadEndlessScore(LeaderboardScoreUploaded_t pCallback, bool bIOFailure)
	{
		IsUploadingEndlessLeaderboard = false;
		if (bIOFailure || pCallback.m_bSuccess == 0)
		{
			Debug.LogError("[Steam] 上传分数失败。");
			return;
		}
		bool flag = pCallback.m_bScoreChanged != 0;
		int nGlobalRankNew = pCallback.m_nGlobalRankNew;
		int nGlobalRankPrevious = pCallback.m_nGlobalRankPrevious;
		Debug.Log("[Steam] 分数上传成功！");
		Debug.Log("[Steam] 是否打破个人记录: " + (flag ? "是" : "否"));
		Debug.Log($"[Steam] 你的当前全球排名: 第 {nGlobalRankNew} 名 (此前为: 第 {nGlobalRankPrevious} 名)");
		OnGetRankingCallBack?.Invoke(nGlobalRankNew);
	}

	public void GetTop50Leaderboard(Action<List<EndlessLeaderboardEntryData>> callback)
	{
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Steam Manager 未初始化！");
			return;
		}
		if (IsDownloadingEndlessLeaderboard)
		{
			Debug.LogWarning("在下载排行中...");
			return;
		}
		if (IsGettingRank)
		{
			Debug.LogWarning("在加载排行中...");
			return;
		}
		IsGettingRank = true;
		IsDownloadingEndlessLeaderboard = true;
		OnGetTop50Callback = callback;
		Debug.Log("[Steam] 开始获取排行榜句柄以拉取前50名...");
		SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard("EndlessLeaderboard", ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
		m_FindLeaderboardForDownloadCallResult.Set(hAPICall);
	}

	private void OnLeaderboardFoundForDownload(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		if (bIOFailure || pCallback.m_bLeaderboardFound == 0)
		{
			Debug.LogError("[Steam] 下载排行时：查找排行榜失败。");
			IsDownloadingEndlessLeaderboard = false;
			IsGettingRank = false;
		}
		else
		{
			DownloadTopEntries(pCallback.m_hSteamLeaderboard);
		}
	}

	private void DownloadTopEntries(SteamLeaderboard_t leaderboardHandle)
	{
		SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 50);
		m_DownloadEndlessScoreCallResult.Set(hAPICall);
	}

	private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
	{
		IsDownloadingEndlessLeaderboard = false;
		if (bIOFailure)
		{
			Debug.LogError("[Steam] 下载排行榜数据失败 (IO Failure)。");
			IsGettingRank = false;
			OnGetTop50Callback?.Invoke(new List<EndlessLeaderboardEntryData>());
			return;
		}
		int cEntryCount = pCallback.m_cEntryCount;
		List<EndlessLeaderboardEntryData> list = new List<EndlessLeaderboardEntryData>();
		for (int i = 0; i < cEntryCount; i++)
		{
			if (SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out var pLeaderboardEntry, null, 0))
			{
				CSteamID steamIDUser = pLeaderboardEntry.m_steamIDUser;
				string text = SteamFriends.GetFriendPersonaName(steamIDUser);
				if (string.IsNullOrEmpty(text) || text == "[unknown]")
				{
					SteamFriends.RequestUserInformation(steamIDUser, bRequireNameOnly: true);
					text = steamIDUser.m_SteamID.ToString();
				}
				EndlessLeaderboardEntryData endlessLeaderboardEntryData = default(EndlessLeaderboardEntryData);
				endlessLeaderboardEntryData.steamName = text;
				endlessLeaderboardEntryData.steamID = steamIDUser.m_SteamID;
				endlessLeaderboardEntryData.globalRank = pLeaderboardEntry.m_nGlobalRank;
				endlessLeaderboardEntryData.score = pLeaderboardEntry.m_nScore;
				EndlessLeaderboardEntryData item = endlessLeaderboardEntryData;
				list.Add(item);
			}
		}
		StartCoroutine(WaitForLeaderboardGetName(list));
	}

	private IEnumerator WaitForLeaderboardGetName(List<EndlessLeaderboardEntryData> resultList)
	{
		yield return new WaitForSeconds(0.1f);
		bool needWaitForGetName = true;
		int counter = 3;
		while (needWaitForGetName && counter > 0)
		{
			needWaitForGetName = false;
			for (int i = 0; i < resultList.Count; i++)
			{
				string friendPersonaName = SteamFriends.GetFriendPersonaName(new CSteamID(resultList[i].steamID));
				if (string.IsNullOrEmpty(friendPersonaName) || friendPersonaName == "[unknown]")
				{
					needWaitForGetName = true;
					continue;
				}
				EndlessLeaderboardEntryData value = resultList[i];
				value.steamName = friendPersonaName;
				resultList[i] = value;
			}
			if (needWaitForGetName)
			{
				counter--;
				yield return new WaitForSeconds(1f);
			}
		}
		IsGettingRank = false;
		OnGetTop50Callback?.Invoke(resultList);
	}

	public static string FormatMilliseconds(int value)
	{
		int num = value;
		int num2 = 0;
		int num3 = 0;
		if (num >= 1000)
		{
			num2 = Mathf.FloorToInt((float)num / 1000f);
			num = (int)Mathf.Repeat(num, 1000f);
		}
		if (num2 >= 60)
		{
			num3 = Mathf.FloorToInt((float)num2 / 60f);
			num2 = (int)Mathf.Repeat(num2, 60f);
		}
		return num3.ToString("D2") + ":" + num2.ToString("D2") + "." + num.ToString("D3");
	}

	public void InitTimer()
	{
		timer1 = new Timer(timer1_Tick, null, 0, 1000);
	}

	private static void timer1_Tick(object state)
	{
		SteamAPI.RunCallbacks();
	}

	private IEnumerator GetSteamUsername(string steamId, string apiKey)
	{
		string text = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=" + apiKey + "&steamids=" + steamId;
		Debug.Log(text);
		using UnityWebRequest www = UnityWebRequest.Get(text);
		yield return www.SendWebRequest();
		if (www.result == UnityWebRequest.Result.Success)
		{
			SteamUserSummaryData steamUserSummaryData = JsonUtility.FromJson<SteamUserSummaryData>(www.downloadHandler.text);
			if (steamUserSummaryData != null && steamUserSummaryData.response != null && steamUserSummaryData.response.players != null && steamUserSummaryData.response.players.Length != 0)
			{
				string personaname = steamUserSummaryData.response.players[0].personaname;
				myrankdata.name = personaname;
				Debug.Log("Steam ID: " + steamId + ", Username: " + personaname);
			}
			else
			{
				Debug.LogError("Failed to get Steam username");
			}
		}
		else
		{
			Debug.LogError("Failed to retrieve data from Steam Web API");
		}
	}

	public void GetUserNameComplete()
	{
		Debug.Log("获取到名字");
		downloadingleaderboards = false;
		foreach (RankData rankData in leadboardData.rankDatas)
		{
			_ = rankData;
		}
		leaderboardsSuceed = true;
	}

	private IEnumerator GetAllRankName()
	{
		updating = false;
		for (int i = 0; i < leadboardData.rankDatas.Count; i++)
		{
			string friendPersonaName = SteamFriends.GetFriendPersonaName(leadboardData.rankDatas[i].csteamid);
			myrankdata.name = friendPersonaName;
			leadboardData.rankDatas[i].name = friendPersonaName;
			yield return new WaitForEndOfFrame();
		}
		updating = true;
		Debug.LogWarning("获取成功");
		EventMgr.GetUserName();
	}

	public void WriteBuildAndShare(FinishGameBuild build)
	{
		Debug.Log("上传build");
		SteamRemoteStorage.GetQuota(out var pnTotalBytes, out var _);
		string text = JsonConvert.SerializeObject(build);
		if ((ulong)Encoding.UTF8.GetByteCount(text) > pnTotalBytes)
		{
			MonoBehaviour.print("Remote Storage: Quota Exceeded! - Bytes: " + Encoding.UTF8.GetByteCount(text) + " - Max: " + pnTotalBytes);
			return;
		}
		byte[] array = new byte[Encoding.UTF8.GetByteCount(text)];
		Encoding.UTF8.GetBytes(text, 0, text.Length, array, 0);
		MonoBehaviour.print(string.Concat(str3: SteamRemoteStorage.FileWrite("LeaderboardBuild", array, array.Length).ToString(), str0: "FileWrite(LeaderboardBuild, Data, ", str1: array.Length.ToString(), str2: ") - "));
		SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileShare("LeaderboardBuild");
		OnRemoteStorageFileShareResultCallResult.Set(steamAPICall_t, OnRemoteStorageFileShareResult);
		SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
		MonoBehaviour.print("SteamRemoteStorage.FileShare(LeaderboardBuild) : " + steamAPICall_t2.ToString());
	}

	public IEnumerator WriteBuildAndShare1(FinishGameBuild build)
	{
		SteamRemoteStorage.GetQuota(out var pnTotalBytes, out var _);
		string text = JsonConvert.SerializeObject(build);
		if ((ulong)Encoding.UTF8.GetByteCount(text) > pnTotalBytes)
		{
			MonoBehaviour.print("Remote Storage: Quota Exceeded! - Bytes: " + Encoding.UTF8.GetByteCount(text) + " - Max: " + pnTotalBytes);
		}
		else
		{
			byte[] array = new byte[Encoding.UTF8.GetByteCount(text)];
			Encoding.UTF8.GetBytes(text, 0, text.Length, array, 0);
			MonoBehaviour.print(string.Concat(str3: SteamRemoteStorage.FileWrite("LeaderboardBuild", array, array.Length).ToString(), str0: "FileWrite(LeaderboardBuild, Data, ", str1: array.Length.ToString(), str2: ") - "));
			sharing = true;
			SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileShare("LeaderboardBuild");
			OnRemoteStorageFileShareResultCallResult.Set(steamAPICall_t, OnRemoteStorageFileShareResult);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamRemoteStorage.FileShare(LeaderboardBuild) : " + steamAPICall_t2.ToString());
		}
		yield return new WaitForEndOfFrame();
	}

	private void OnRemoteStorageFileShareResult(RemoteStorageFileShareResult_t pCallback, bool bIOFailure)
	{
		sharing = false;
		string[] obj = new string[8]
		{
			"[",
			1307.ToString(),
			" - RemoteStorageFileShareResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		UGCHandle_t hFile = pCallback.m_hFile;
		obj[5] = hFile.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgchFilename;
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			hFile = pCallback.m_hFile;
			Debug.Log("获取UGC成功:" + hFile.ToString());
			m_UGCHandle = pCallback.m_hFile;
			SteamAPICall_t steamAPICall_t = SteamUserStats.AttachLeaderboardUGC(leaderboards[int_currentleaderboard].leaderboard, m_UGCHandle);
			OnLeaderboardUGCSetCallResult.Set(steamAPICall_t, OnLeaderboardUGCSet);
			string[] obj2 = new string[6] { "SteamUserStats.AttachLeaderboardUGC(", null, null, null, null, null };
			SteamLeaderboard_t leaderboard = leaderboards[int_currentleaderboard].leaderboard;
			obj2[1] = leaderboard.ToString();
			obj2[2] = ", ";
			hFile = m_UGCHandle;
			obj2[3] = hFile.ToString();
			obj2[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			obj2[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		else
		{
			Debug.Log("获取UGC失败");
		}
	}

	public void ClearUGCAndScore(DifficultyType difficulty = DifficultyType.Easy)
	{
		StartCoroutine(IE_ClearUGCAndScore(difficulty));
	}

	public IEnumerator IE_ClearUGCAndScore(DifficultyType difficulty = DifficultyType.Easy)
	{
		FinishGameBuild build = new FinishGameBuild();
		if (Inst.myrankdata != null)
		{
			yield return StartCoroutine(IE_UploadUGC(build, difficulty));
			if (m_UGCHandle.m_UGCHandle != 0L)
			{
				steamUserSateCustom.Inst.UploadMyUserState(0, m_UGCHandle.m_UGCHandle, difficulty);
			}
		}
	}

	public void UploadUGCAndScore(int sore, FinishGameBuild build, DifficultyType difficulty = DifficultyType.Easy)
	{
		StartCoroutine(IE_UploadUGCAndScore(sore, build, difficulty));
	}

	public IEnumerator IE_UploadUGC(FinishGameBuild build, DifficultyType difflcilty = DifficultyType.Easy)
	{
		Debug.Log("上传build");
		ugcstate = UGCState.Upload;
		SteamRemoteStorage.GetQuota(out var pnTotalBytes, out var _);
		string text = JsonConvert.SerializeObject(build);
		string text2 = "LeaderboardBuild" + difflcilty;
		if ((ulong)Encoding.UTF8.GetByteCount(text) > pnTotalBytes)
		{
			MonoBehaviour.print("Remote Storage: Quota Exceeded! - Bytes: " + Encoding.UTF8.GetByteCount(text) + " - Max: " + pnTotalBytes);
			yield break;
		}
		byte[] array = new byte[Encoding.UTF8.GetByteCount(text)];
		Encoding.UTF8.GetBytes(text, 0, text.Length, array, 0);
		bool flag = SteamRemoteStorage.FileWrite(text2, array, array.Length);
		MonoBehaviour.print("FileWrite(" + text2 + ", Data, " + array.Length + ") - " + flag);
		SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileShare(text2);
		OnRemoteStorageFileShareResultCallResult.Set(steamAPICall_t, OnUGCUpload);
		SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
		MonoBehaviour.print("SteamRemoteStorage.FileShare(" + text2 + ") : " + steamAPICall_t2.ToString());
		while (ugcstate == UGCState.Upload)
		{
			yield return new WaitForEndOfFrame();
		}
		if (ugcstate == UGCState.UploadSuccess)
		{
			Debug.Log("上传成功");
			steamUserSateCustom.Inst.GetAllFriendsUserState();
		}
	}

	public IEnumerator IE_UploadUGCAndScore(int score, FinishGameBuild build, DifficultyType difficulty = DifficultyType.Easy)
	{
		Debug.Log("此次用时：" + build.timeuse);
		Debug.Log("记录用时:" + Inst.myrankdata.score);
		if (Inst.myrankdata != null)
		{
			int num = 0;
			switch (difficulty)
			{
			case DifficultyType.Easy:
				num = Inst.myrankdata.score;
				break;
			case DifficultyType.Normal:
				num = Inst.myrankdata.scorehard;
				break;
			case DifficultyType.Hard:
				num = Inst.myrankdata.scorenightmare;
				break;
			case DifficultyType.Nightmare1:
				num = Inst.myrankdata.scoreNewNightmare1;
				break;
			case DifficultyType.Nightmare2:
				num = Inst.myrankdata.scoreNewNightmare2;
				break;
			case DifficultyType.Nightmare3:
				num = Inst.myrankdata.scoreNewNightmare3;
				break;
			}
			if (num == 0)
			{
				Debug.Log("上传");
				yield return StartCoroutine(IE_UploadUGC(build, difficulty));
				if (m_UGCHandle.m_UGCHandle != 0L)
				{
					steamUserSateCustom.Inst.UploadMyUserState(score, m_UGCHandle.m_UGCHandle, difficulty);
				}
				else
				{
					Debug.LogWarning("不上传统计数据，因为上传ugc失败");
				}
			}
			else if ((float)num < build.timeuse)
			{
				Debug.Log("不是最佳成绩，不上传");
			}
			else
			{
				Debug.Log("是最佳成绩，上传");
				yield return StartCoroutine(IE_UploadUGC(build, difficulty));
				if (m_UGCHandle.m_UGCHandle != 0L)
				{
					steamUserSateCustom.Inst.UploadMyUserState(score, m_UGCHandle.m_UGCHandle, difficulty);
				}
				else
				{
					Debug.LogWarning("不上传统计数据，因为上传ugc失败");
				}
			}
		}
		else
		{
			yield return StartCoroutine(IE_UploadUGC(build, difficulty));
			if (m_UGCHandle.m_UGCHandle != 0L)
			{
				steamUserSateCustom.Inst.UploadMyUserState(score, m_UGCHandle.m_UGCHandle, difficulty);
			}
			else
			{
				Debug.LogWarning("不上传统计数据，因为上传ugc失败");
			}
		}
	}

	private void OnUGCUpload(RemoteStorageFileShareResult_t pCallback, bool bIOFailure)
	{
		sharing = false;
		string[] obj = new string[8]
		{
			"[",
			1307.ToString(),
			" - RemoteStorageFileShareResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		UGCHandle_t hFile = pCallback.m_hFile;
		obj[5] = hFile.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgchFilename;
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			ugcstate = UGCState.UploadSuccess;
			hFile = pCallback.m_hFile;
			Debug.Log("上传UGC成功:" + hFile.ToString());
			m_UGCHandle = pCallback.m_hFile;
		}
		else
		{
			Debug.Log("上传UGC失败");
		}
	}

	private void OnLeaderboardUGCSet(LeaderboardUGCSet_t pCallback, bool bIOFailure)
	{
		Debug.Log("附加UGC" + bIOFailure);
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

	public void downloadUGC(UGCHandle_t ugc, string Filename)
	{
		downloadingUGC = true;
		SteamAPICall_t steamAPICall_t = SteamRemoteStorage.UGCDownloadToLocation(ugc, Application.persistentDataPath + "\\" + Filename, 0u);
		OnRemoteStorageDownloadUGCResultCallResult.Set(steamAPICall_t, OnRemoteStorageUpdatePublishedFileResult);
		string[] obj = new string[6] { "SteamRemoteStorage.UGCDownload(", null, null, null, null, null };
		UGCHandle_t uGCHandle_t = ugc;
		obj[1] = uGCHandle_t.ToString();
		obj[2] = ", ";
		obj[3] = 0.ToString();
		obj[4] = ") : ";
		SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
		obj[5] = steamAPICall_t2.ToString();
		MonoBehaviour.print(string.Concat(obj));
	}

	private void OnRemoteStorageUpdatePublishedFileResult(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
	{
		downloadingUGC = false;
		downloadedUGCpath = param.m_pchFileName;
	}
}
