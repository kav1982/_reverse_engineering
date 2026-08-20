using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIEndlessRankingList")]
public class UIEndlessRankingList : GameUISingletonMono<UIEndlessRankingList>
{
	public UIFinishBuildShow finishbuildshow;

	public UIEndlessFinishPanel endlessFinishPanel;

	public Animator anima;

	public Color SelectedColor;

	public Color UnselectedColor;

	public Button btn_Steam;

	public Button btn_Local;

	public Button btn_Close;

	public GameObject go_MyRankSegment;

	public Custom_ScrollRect Custom_ScrollRect;

	public Custom_ScrollRect Custom_ScrollRectSteam;

	public int slotPerPageMax;

	[Header("状态提示遮罩/物体")]
	public GameObject go_FriendsLoading;

	public GameObject panel_Loading;

	public GameObject go_LoadingBG;

	public GameObject go_LoadingFail;

	public GameObject panel_LocalNoRecords;

	public GameObject panel_SteamNoRecords;

	public GameObject panel_SteamNoConnect;

	[Header("面板与容器")]
	public GameObject panel_SteamRanklist;

	public GameObject panel_LocalRanklist;

	public GameObject go_SteamRanklist_Content;

	public GameObject go_LocalRanklist_Content;

	public GameObject go_Myleaderboard_Content;

	public GameObject go_BtnSteamOutline;

	public GameObject go_BtnLocalOutline;

	public UpdatButtonShow[] updatebuttonshows;

	[Header("多语言文本")]
	public Text text_Loading;

	public Text text_LoadingRetry;

	public Text text_LoadingTitle;

	public Text text_FriendtBtn;

	public Text text_FriendRank;

	public Text text_FriendName;

	public Text text_FriendScore;

	public Text text_FriendNoRecord;

	public Text text_FriendNoConnected;

	public Text text_LocalBtn;

	public Text text_LocalRank;

	public Text text_LocalTime;

	public Text text_LocalScore;

	public Text text_LocalNoRecords;

	[SerializeField]
	private Button BtnSortDate;

	[SerializeField]
	private Button BtnSortScore;

	private bool b_SortByDateNewToOld;

	private bool b_SortByScoreMaxToMin;

	private readonly List<LocalEndlessRankingSlot> _localLeaderBoards = new List<LocalEndlessRankingSlot>();

	protected override void RegistarWhenInit()
	{
		EventMgr.GetUserName = (Action)Delegate.Combine(EventMgr.GetUserName, new Action(UpdateLeaderBoard));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(UpdateLanguage));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.WASD.performed += KeyDirect;
		base.inputActions.Player.GamepadEast.performed += GamepadBack;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.WASD.performed -= KeyDirect;
		base.inputActions.Player.GamepadEast.performed -= GamepadBack;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.GetUserName = (Action)Delegate.Remove(EventMgr.GetUserName, new Action(UpdateLeaderBoard));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(UpdateLanguage));
	}

	protected override IEnumerator OnInit()
	{
		UpdateLanguage();
		go_BtnSteamOutline.SetActive(value: false);
		go_BtnLocalOutline.SetActive(value: false);
		btn_Steam.onClick.AddListener(BtnSteamClick);
		btn_Local.onClick.AddListener(BtnLocalClick);
		btn_Close.onClick.AddListener(_Close);
		BtnSortDate.onClick.AddListener(UpdateRankByDate);
		BtnSortScore.onClick.AddListener(UpdateRankByScore);
		yield return null;
	}

	protected override void OnShow(object obj = null)
	{
		ShowLocalRankList();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		SEMgr.Inst.uiOpen.PlaySE();
		anima.Play("Show");
		ControlChange();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		go_FriendsLoading.SetActive(value: false);
		if (SteamManager.Initialized)
		{
			panel_SteamNoConnect.SetActive(value: false);
			go_FriendsLoading.SetActive(value: true);
			UpdateFriendLeaderBoard(new List<SteamLeadBoardManager.EndlessLeaderboardEntryData>());
			SteamLeadBoardManager.Inst.GetTop50Leaderboard(OnGetRankingList);
		}
		else
		{
			panel_SteamNoConnect.SetActive(value: true);
		}
	}

	private void OnGetRankingList(List<SteamLeadBoardManager.EndlessLeaderboardEntryData> leaderboard)
	{
		if (!(base.gameObject == null))
		{
			UpdateFriendLeaderBoard(leaderboard);
			go_FriendsLoading.SetActive(value: false);
			panel_SteamNoRecords.SetActive(value: false);
		}
	}

	private void BtnSteamClick()
	{
		if (base.IsOpen)
		{
			ShowFriendRankList();
		}
	}

	private void BtnLocalClick()
	{
		if (base.IsOpen)
		{
			ShowLocalRankList();
		}
	}

	private void UpdateLanguage()
	{
		text_FriendtBtn.text = 1003103.GetText();
		text_FriendRank.text = 1003105.GetText();
		text_FriendName.text = 1003104.GetText();
		text_FriendScore.text = 1003106.GetText();
		text_LocalBtn.text = 1003102.GetText();
		text_LocalRank.text = 1003203.GetText();
		text_LocalScore.text = 1003106.GetText();
		text_LocalTime.text = 1003107.GetText();
		string text3 = (text_FriendNoConnected.text = (text_FriendNoRecord.text = " "));
		text_Loading.text = 1003210.GetText();
		text_LoadingRetry.text = 1003211.GetText();
		text_LoadingTitle.text = 1003212.GetText();
		text_LocalNoRecords.text = 1003213.GetText();
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void KeyDirect(InputAction.CallbackContext context)
	{
		if (!base.IsOpen)
		{
			return;
		}
		Vector2 vector = context.ReadValue<Vector2>();
		if (vector == Vector2.left || vector == Vector2.right)
		{
			if (panel_SteamRanklist.activeSelf)
			{
				ShowLocalRankList();
			}
			else
			{
				ShowFriendRankList();
			}
		}
	}

	private void GamepadBack(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			if (finishbuildshow.IsOpen)
			{
				finishbuildshow._Close();
			}
			else if (endlessFinishPanel.IsOpen)
			{
				endlessFinishPanel.Close();
			}
			else if (base.IsOpen)
			{
				Hide();
			}
		}
	}

	private void ShowFriendRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		panel_Loading.SetActive(value: false);
		btn_Steam.image.color = SelectedColor;
		btn_Local.image.color = UnselectedColor;
		panel_SteamRanklist.SetActive(value: true);
		go_MyRankSegment.SetActive(value: true);
		go_BtnSteamOutline.SetActive(value: true);
		go_BtnLocalOutline.SetActive(value: false);
		panel_LocalRanklist.SetActive(value: false);
	}

	private void ShowLocalRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		b_SortByDateNewToOld = true;
		b_SortByScoreMaxToMin = false;
		btn_Local.image.color = SelectedColor;
		btn_Steam.image.color = UnselectedColor;
		panel_Loading.SetActive(value: false);
		panel_SteamRanklist.SetActive(value: false);
		go_MyRankSegment.SetActive(value: false);
		UpdateLocalLeaderBoards();
		panel_LocalRanklist.SetActive(value: true);
		go_BtnSteamOutline.SetActive(value: false);
		go_BtnLocalOutline.SetActive(value: true);
	}

	public void HideInfo()
	{
		if (finishbuildshow.IsOpen)
		{
			finishbuildshow._Close();
		}
		else if (endlessFinishPanel.IsOpen)
		{
			endlessFinishPanel.Close();
		}
	}

	public void StartLoadingFriendsUserStats()
	{
		go_FriendsLoading.SetActive(value: true);
	}

	public void ShowSteamRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		panel_Loading.SetActive(value: false);
		btn_Steam.image.color = SelectedColor;
		btn_Local.image.color = UnselectedColor;
		panel_SteamRanklist.SetActive(value: true);
		go_MyRankSegment.SetActive(value: true);
		go_BtnSteamOutline.SetActive(value: true);
		go_BtnLocalOutline.SetActive(value: false);
		StartCoroutine(LeaderBoardDownload(1, slotPerPageMax));
		panel_LocalRanklist.SetActive(value: false);
	}

	public IEnumerator LeaderBoardDownload(int start = 0, int end = 9, int index = 0)
	{
		go_Myleaderboard_Content.transform.DestroyAllChild();
		go_SteamRanklist_Content.transform.DestroyAllChild();
		panel_Loading.SetActive(value: true);
		go_LoadingFail.SetActive(value: false);
		go_LoadingBG.SetActive(value: true);
		if (SteamManager.Initialized)
		{
			if (SteamLeadBoardManager.Inst.GetLeadBoard(index).m_SteamLeaderboard == 0L)
			{
				if (!SteamLeadBoardManager.Inst.gettingleaderboard)
				{
					SteamLeadBoardManager.Inst.TestLeaderBoard(index);
				}
				yield return new WaitForEndOfFrame();
				StartCoroutine(LeaderBoardDownload(start, end, index));
				yield break;
			}
			if (!SteamLeadBoardManager.Inst.downloadingmyleaderboard)
			{
				SteamLeadBoardManager.Inst.DownloadEntriesForUser(index);
			}
			if (!SteamLeadBoardManager.Inst.downloadingleaderboards)
			{
				SteamLeadBoardManager.Inst.DownloadEntries(start, end, index);
			}
			while (SteamLeadBoardManager.Inst.downloadingmyleaderboard || SteamLeadBoardManager.Inst.downloadingleaderboards)
			{
				yield return new WaitForEndOfFrame();
			}
			if (SteamLeadBoardManager.Inst.leaderboardMineSuceed && SteamLeadBoardManager.Inst.leaderboardsSuceed)
			{
				panel_Loading.SetActive(value: false);
				yield break;
			}
			panel_Loading.SetActive(value: false);
			if (!SteamLeadBoardManager.Inst.leaderboardsSuceed && (bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen)
			{
				GameUISingletonMono<UI_RankingList>.Inst._Close();
				panel_Loading.SetActive(value: true);
				go_LoadingFail.SetActive(value: true);
				go_LoadingBG.SetActive(value: false);
			}
		}
		else
		{
			panel_Loading.SetActive(value: true);
			go_LoadingFail.SetActive(value: true);
			go_LoadingBG.SetActive(value: false);
		}
	}

	public void SteamRankPageUpdate()
	{
		panel_Loading.SetActive(value: false);
		btn_Steam.image.color = SelectedColor;
		btn_Local.image.color = UnselectedColor;
		panel_SteamRanklist.SetActive(value: true);
		go_MyRankSegment.SetActive(value: true);
		go_BtnSteamOutline.SetActive(value: true);
		go_BtnLocalOutline.SetActive(value: false);
		StartCoroutine(LeaderBoardDownload(1, slotPerPageMax));
		panel_LocalRanklist.SetActive(value: false);
	}

	public void Reload()
	{
		SteamRankPageUpdate();
	}

	public void UpdateFriendLeaderBoard(List<SteamLeadBoardManager.EndlessLeaderboardEntryData> _list)
	{
		go_Myleaderboard_Content.transform.DestroyAllChild();
		go_SteamRanklist_Content.transform.DestroyAllChild();
		ulong steamID = SteamUser.GetSteamID().m_SteamID;
		if (_list.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < _list.Count; i++)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIEndlessRankingListSteamSlot"), go_SteamRanklist_Content.transform).GetComponent<SteamEndlessRankingListSlot>().OnInitialize(_list[i]);
			if (_list[i].steamID == steamID)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIEndlessRankingListSteamSlot"), go_Myleaderboard_Content.transform).GetComponent<SteamEndlessRankingListSlot>().OnInitialize(_list[i]);
			}
		}
	}

	public IEnumerator UpdateMyFriendLeaderBoardIE()
	{
		go_Myleaderboard_Content.transform.DestroyAllChild();
		SteamLeadBoardManager.Inst.leaderboardsSuceed = true;
		go_SteamRanklist_Content.transform.DestroyAllChild();
		if (SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count > 0)
		{
			panel_SteamNoRecords.SetActive(value: false);
			go_SteamRanklist_Content.SetActive(value: true);
			int num = 0;
			for (int i = 0; i < SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count; i++)
			{
				RankData rankData = SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas[i];
				if (rankData.score != 0)
				{
					num++;
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), go_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugc, DifficultyType.Easy, num);
					if (rankData.csteamid == SteamUser.GetSteamID())
					{
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), go_Myleaderboard_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugc, DifficultyType.Easy, num);
					}
				}
			}
		}
		else
		{
			panel_SteamNoRecords.SetActive(value: true);
			go_SteamRanklist_Content.SetActive(value: false);
		}
		yield return null;
	}

	public void NextLocalPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		UpdateLocalLeaderBoards(sort: false);
	}

	public void PreviousLocalPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		UpdateLocalLeaderBoards(sort: false);
	}

	public void UpdateLocalLeaderBoards(bool sort = true)
	{
		StartCoroutine(UpdateLocalLeaderBoardsIE(sort));
	}

	public int SortByScoreMinToMax(FinishEndlessGameBuild a, FinishEndlessGameBuild b)
	{
		if (a.EndlessLevel < b.EndlessLevel)
		{
			return 1;
		}
		if (a.EndlessLevel > b.EndlessLevel)
		{
			return -1;
		}
		if (!b_SortByDateNewToOld)
		{
			return SortByDateOldToNew(a, b);
		}
		return SortByDateNewToOld(a, b);
	}

	public int SortByScoreMaxToMin(FinishEndlessGameBuild a, FinishEndlessGameBuild b)
	{
		if (a.EndlessLevel < b.EndlessLevel)
		{
			return -1;
		}
		if (a.EndlessLevel > b.EndlessLevel)
		{
			return 1;
		}
		if (!b_SortByDateNewToOld)
		{
			return SortByDateOldToNew(a, b);
		}
		return SortByDateNewToOld(a, b);
	}

	public int SortByDateNewToOld(FinishEndlessGameBuild a, FinishEndlessGameBuild b)
	{
		if (a.finishGameBuild.time < b.finishGameBuild.time)
		{
			return -1;
		}
		if (a.finishGameBuild.time > b.finishGameBuild.time)
		{
			return 1;
		}
		return 0;
	}

	public int SortByDateOldToNew(FinishEndlessGameBuild a, FinishEndlessGameBuild b)
	{
		if (a.finishGameBuild.time < b.finishGameBuild.time)
		{
			return 1;
		}
		if (a.finishGameBuild.time > b.finishGameBuild.time)
		{
			return -1;
		}
		return 0;
	}

	private void UpdateRankByDate()
	{
		b_SortByDateNewToOld = !b_SortByDateNewToOld;
		if (b_SortByDateNewToOld)
		{
			DataMgr.finishEndlessGameBuilds.finishGameBuilds.Sort(SortByDateNewToOld);
		}
		else
		{
			DataMgr.finishEndlessGameBuilds.finishGameBuilds.Sort(SortByDateOldToNew);
		}
		for (int i = 0; i < _localLeaderBoards.Count; i++)
		{
			_localLeaderBoards[i].initialize(DataMgr.finishEndlessGameBuilds.finishGameBuilds[i], i);
		}
	}

	private void UpdateRankByScore()
	{
		b_SortByScoreMaxToMin = !b_SortByScoreMaxToMin;
		if (b_SortByScoreMaxToMin)
		{
			DataMgr.finishEndlessGameBuilds.finishGameBuilds.Sort(SortByScoreMinToMax);
		}
		else
		{
			DataMgr.finishEndlessGameBuilds.finishGameBuilds.Sort(SortByScoreMaxToMin);
		}
		for (int i = 0; i < _localLeaderBoards.Count; i++)
		{
			_localLeaderBoards[i].initialize(DataMgr.finishEndlessGameBuilds.finishGameBuilds[i], i);
		}
	}

	private IEnumerator UpdateLocalLeaderBoardsIE(bool sort = true)
	{
		_localLeaderBoards.Clear();
		go_LocalRanklist_Content.transform.DestroyAllChild();
		if (DataMgr.finishEndlessGameBuilds != null && DataMgr.finishEndlessGameBuilds.finishGameBuilds.Count > 0)
		{
			if (sort)
			{
				DataMgr.finishEndlessGameBuilds.finishGameBuilds.Sort(SortByDateOldToNew);
			}
			panel_LocalNoRecords.SetActive(value: false);
			int num = Mathf.Min(slotPerPageMax, DataMgr.finishEndlessGameBuilds.finishGameBuilds.Count);
			for (int i = 0; i < num; i++)
			{
				LocalEndlessRankingSlot component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIEndlessRankingListLocalSlot"), go_LocalRanklist_Content.transform).GetComponent<LocalEndlessRankingSlot>();
				component.initialize(DataMgr.finishEndlessGameBuilds.finishGameBuilds[i], i);
				_localLeaderBoards.Add(component);
			}
			if (sort)
			{
				UpdateRankByDate();
			}
		}
		else
		{
			panel_LocalNoRecords.SetActive(value: true);
		}
		yield return null;
	}

	public void UpdateLeaderBoard()
	{
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		anima.Play("Hide");
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
	}

	public override void _Close()
	{
		if (base.IsOpen)
		{
			SEMgr.Inst.uiClick.PlaySE();
			Hide();
		}
	}
}
