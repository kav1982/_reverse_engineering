using System;
using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIRankingList")]
public class UI_RankingList : GameUISingletonMono<UI_RankingList>
{
	private DifficultyType CurrentFriendDifficulty;

	public Color SelectedColor;

	public Color UnselectedColor;

	public Button Steam;

	public Button Local;

	public Animator anima;

	public GameObject segmentMyRank;

	public Custom_ScrollRect Custom_ScrollRect;

	public Custom_ScrollRect Custom_ScrollRectSteam;

	public int slotPerPageMax = 999;

	public GameObject FriendsLoading;

	public GameObject gameobject_loading;

	public GameObject gameObject_loadingwait;

	public GameObject gameobject_loadingfail;

	public GameObject gameobject_nolocalrecord;

	public GameObject gameobjet_noFriendRankingList;

	public GameObject NoConnect;

	[Header("Gameobject")]
	public GameObject gameobject_SteamRanklist;

	public GameObject gameobject_LocalRanklist;

	public GameObject gameobject_SteamRanklist_Content;

	public GameObject gameobject_LocalRanklist_Content;

	public GameObject gameobject_Myleaderboard_Content;

	public GameObject gameobject_SteamRanklistFrame;

	public GameObject gameobject_LocalRanklistFrame;

	public GameObject gameobject_dificultyText;

	public UIFinishBuildShow finishbuildshow;

	public GameObject gameobject_GamepadSwitchRankListShortcut;

	public GameObject gameobject_ButtonSwitchRankList;

	public UpdatButtonShow[] updatebuttonshows;

	public GameObject Sorting0Frame;

	public GameObject Sorting1Frame;

	public Text page;

	public Text Leaderboardname;

	public Text Loading;

	public Text Retry;

	public Text CheckSteamConnect;

	public Text NoLocalRecords;

	[Header("Text_Steam")]
	public Text text_TitleSteaml;

	public Text text_SteamName;

	public Text text_Rank;

	public Text text_Score;

	public Text text_NoFriendRecord;

	public Text text_SteamNotConnected;

	[Header("Text_Local")]
	public Text text_TitleLocal;

	public Text text_Rank_Local;

	public Text text_Score_Local;

	public Text text_FinishTime;

	private bool boolSortDate;

	private bool boolSortScore;

	private int steampage = 1;

	private int localpage;

	private int selectindex;

	private int sortingselect;

	protected override void RegistarWhenInit()
	{
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(UpdateLanguage));
		EventMgr.FriendsUserStateUpdate = (Action)Delegate.Combine(EventMgr.FriendsUserStateUpdate, new Action(UpdateFriendLeaderBoard));
		EventMgr.FriendsUserStateUpdateComplete = (Action)Delegate.Combine(EventMgr.FriendsUserStateUpdateComplete, new Action(CompleteFriend));
		EventMgr.FriendsUserStateUpdateStart = (Action)Delegate.Combine(EventMgr.FriendsUserStateUpdateStart, new Action(StartLoadingFriendsUserStats));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.WASD.performed += KeyDirect;
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadEast.performed += GamepadBack;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.GamepadLB.performed += GamepadRBPerformed;
		base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
		base.inputActions.Player.Drink.performed += GamepadNextDifficultyFriendRankList;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadEast.performed -= GamepadBack;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.GamepadLB.performed -= GamepadRBPerformed;
		base.inputActions.Player.GamepadRB.performed -= GamepadRBPerformed;
		base.inputActions.Player.Drink.performed -= GamepadNextDifficultyFriendRankList;
		base.inputActions.Player.WASD.performed -= KeyDirect;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(UpdateLanguage));
		EventMgr.FriendsUserStateUpdate = (Action)Delegate.Remove(EventMgr.FriendsUserStateUpdate, new Action(UpdateFriendLeaderBoard));
		EventMgr.FriendsUserStateUpdateStart = (Action)Delegate.Remove(EventMgr.FriendsUserStateUpdateStart, new Action(StartLoadingFriendsUserStats));
		EventMgr.FriendsUserStateUpdateComplete = (Action)Delegate.Remove(EventMgr.FriendsUserStateUpdateComplete, new Action(CompleteFriend));
	}

	private void OnEnable()
	{
		InputChange();
	}

	private void GamepadNextDifficultyRankList(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen && gameobject_SteamRanklist.activeSelf && !SteamLeadBoardManager.Inst.downloadingleaderboards)
		{
			Debug.Log("切换榜单");
			steampage = 1;
			SteamLeadBoardManager.Inst.int_currentleaderboard++;
			SteamRankPageUpdate();
		}
	}

	private void GamepadRBPerformed(InputAction.CallbackContext context)
	{
		if (!base.IsOpen)
		{
			return;
		}
		if (gameobject_SteamRanklist.activeSelf)
		{
			steampage = 1;
			if (selectindex != -1)
			{
				if (gameobject_SteamRanklist_Content.transform.childCount != 0)
				{
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
				}
				selectindex = -1;
			}
			ShowLocalRankList();
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
			page.text = localpage.ToString();
		}
		else
		{
			localpage = 1;
			ShowFriendRankList();
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: true);
			page.text = steampage.ToString();
		}
	}

	protected override IEnumerator OnInit()
	{
		UpdateLanguage();
		gameobject_SteamRanklistFrame.SetActive(value: false);
		gameobject_LocalRanklistFrame.SetActive(value: false);
		yield return null;
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (!base.IsOpen || UIMgr.Inst.InputType != PlayerInputType.Gamepad || finishbuildshow.IsOpen)
		{
			return;
		}
		if (gameobject_SteamRanklist.activeSelf)
		{
			if (gameobject_loadingfail.activeSelf)
			{
				Reload();
			}
			if (selectindex != -1)
			{
				gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().OnClick();
			}
		}
		else
		{
			if (!gameobject_LocalRanklist.activeSelf)
			{
				return;
			}
			if (selectindex == -1)
			{
				if (sortingselect == 0)
				{
					SortLocalDateButton();
				}
				else if (sortingselect == 1)
				{
					SortLocalScroeButton();
				}
			}
			else
			{
				gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().OnClick();
			}
		}
	}

	private void UpdateLanguage()
	{
		text_TitleSteaml.text = 1003103.GetText();
		text_SteamName.text = 1003104.GetText();
		text_Rank.text = 1003105.GetText();
		text_Score.text = 1003106.GetText();
		text_TitleLocal.text = 1003102.GetText();
		text_Rank_Local.text = 1003203.GetText();
		text_Score_Local.text = 1003106.GetText();
		text_FinishTime.text = 1003107.GetText();
		text_NoFriendRecord.text = 1003208.GetText();
		text_SteamNotConnected.text = 1003209.GetText();
		Loading.text = 1003210.GetText();
		Retry.text = 1003211.GetText();
		CheckSteamConnect.text = 1003212.GetText();
		NoLocalRecords.text = 1003213.GetText();
	}

	private void InputChange()
	{
		ControlChange();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
			Sorting0Frame.SetActive(value: false);
			Sorting1Frame.SetActive(value: false);
			if (selectindex != -1)
			{
				if (gameobject_SteamRanklist.activeSelf)
				{
					gameobject_ButtonSwitchRankList.SetActive(value: true);
					if (gameobject_SteamRanklist_Content.transform.childCount != 0)
					{
						gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.SetActive(value: false);
					}
				}
				else
				{
					gameobject_ButtonSwitchRankList.SetActive(value: false);
					if (gameobject_LocalRanklist_Content.transform.childCount != 0)
					{
						gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.SetActive(value: false);
					}
				}
			}
			selectindex = -1;
			break;
		case PlayerInputType.Gamepad:
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: true);
			gameobject_ButtonSwitchRankList.SetActive(value: false);
			if (gameobject_SteamRanklist.activeSelf)
			{
				gameobject_GamepadSwitchRankListShortcut.SetActive(value: true);
				gameobject_SteamRanklistFrame.SetActive(value: true);
				selectindex = 0;
				if (gameobject_SteamRanklist_Content.transform.childCount != 0)
				{
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				}
				UpdateSortingSelect();
			}
			else
			{
				gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
				gameobject_LocalRanklistFrame.SetActive(value: true);
				selectindex = 0;
				UpdateSortingSelect();
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		_ = base.IsOpen;
	}

	private void InputChangeAtStart()
	{
		ControlChange();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
			Sorting0Frame.SetActive(value: false);
			Sorting1Frame.SetActive(value: false);
			if (selectindex != -1)
			{
				if (gameobject_SteamRanklist.activeSelf)
				{
					gameobject_ButtonSwitchRankList.SetActive(value: true);
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.SetActive(value: false);
				}
				else
				{
					gameobject_ButtonSwitchRankList.SetActive(value: false);
					gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.SetActive(value: false);
				}
			}
			selectindex = -1;
			break;
		case PlayerInputType.Gamepad:
			gameobject_GamepadSwitchRankListShortcut.SetActive(value: true);
			gameobject_ButtonSwitchRankList.SetActive(value: false);
			if (gameobject_SteamRanklist.activeSelf)
			{
				gameobject_GamepadSwitchRankListShortcut.SetActive(value: true);
				gameobject_SteamRanklistFrame.SetActive(value: true);
				selectindex = -1;
				UpdateSortingSelect();
			}
			else
			{
				gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
				gameobject_LocalRanklistFrame.SetActive(value: true);
				selectindex = 0;
				UpdateSortingSelect();
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		_ = base.IsOpen;
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
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
			else if (base.IsOpen)
			{
				Hide();
			}
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			movedirection_nav(vector);
		}
	}

	private void KeyDirect(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			if (vector == Vector2.left)
			{
				GamepadRBPerformed(default(InputAction.CallbackContext));
			}
			else if (vector == Vector2.right)
			{
				GamepadRBPerformed(default(InputAction.CallbackContext));
			}
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			movedirection_nav(direct);
		}
	}

	private void movedirection_nav(Vector2 direct)
	{
		if (finishbuildshow.IsOpen || !(anima.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f) || gameobject_loading.activeSelf)
		{
			return;
		}
		if (direct == Vector2.left)
		{
			if (selectindex == -1)
			{
				sortingselect--;
				UpdateSortingSelect();
			}
			else if (gameobject_SteamRanklist.activeSelf)
			{
				FriendPreviousPage();
			}
			else if ((bool)gameobject_LocalRanklist)
			{
				PreviousLocalPage();
			}
		}
		else if (direct == Vector2.right)
		{
			if (selectindex == -1)
			{
				sortingselect++;
				UpdateSortingSelect();
			}
			else if (gameobject_SteamRanklist.activeSelf)
			{
				FriendNextPage();
			}
			else if ((bool)gameobject_LocalRanklist)
			{
				NextLocalPage();
			}
		}
		else if (direct == Vector2.down)
		{
			if (gameobject_SteamRanklist.activeSelf)
			{
				if (NoConnect.gameObject.activeSelf || gameobjet_noFriendRankingList.activeSelf)
				{
					return;
				}
				if (gameobject_SteamRanklist_Content.transform.childCount == 0)
				{
					Debug.LogWarning("没有数据！");
					selectindex = -1;
					return;
				}
				selectindex++;
				if (selectindex > gameobject_SteamRanklist_Content.transform.childCount - 1)
				{
					selectindex = gameobject_SteamRanklist_Content.transform.childCount - 1;
				}
				gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				if (selectindex > 0)
				{
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex - 1).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
				}
				UpdateGamepadScrollRectPointSteam(slideDirectionDown: true);
			}
			else if (gameobject_LocalRanklist_Content.transform.childCount == 0)
			{
				Debug.LogWarning("没有数据！");
				selectindex = -1;
			}
			else if (selectindex == -1)
			{
				selectindex = 0;
				Sorting0Frame.SetActive(value: false);
				Sorting1Frame.SetActive(value: false);
				gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				UpdateGamepadScrollRectPointLocal(slideDirectionDown: false);
			}
			else
			{
				selectindex++;
				if (selectindex > gameobject_LocalRanklist_Content.transform.childCount - 1)
				{
					selectindex = gameobject_LocalRanklist_Content.transform.childCount - 1;
				}
				gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				if (selectindex > 0)
				{
					gameobject_LocalRanklist_Content.transform.GetChild(selectindex - 1).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
				}
				UpdateGamepadScrollRectPointLocal(slideDirectionDown: true);
			}
		}
		else
		{
			if (!(direct == Vector2.up))
			{
				return;
			}
			if (gameobject_SteamRanklist.activeSelf)
			{
				if (selectindex > 0)
				{
					selectindex--;
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
					if (gameobject_SteamRanklist_Content.transform.GetChild(selectindex + 1).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.activeSelf)
					{
						gameobject_SteamRanklist_Content.transform.GetChild(selectindex + 1).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
					}
					UpdateGamepadScrollRectPointSteam(slideDirectionDown: false);
				}
			}
			else if (selectindex == -1 || gameobject_LocalRanklist_Content.transform.childCount == 0)
			{
				Debug.LogWarning("没有数据！");
				selectindex = -1;
			}
			else if (selectindex == 0)
			{
				selectindex = -1;
				sortingselect = 0;
				UpdateSortingSelect();
				gameobject_LocalRanklist_Content.transform.GetChild(0).GetComponent<LocalRankingSlot>().SelectFrame.SetActive(value: false);
			}
			else
			{
				selectindex--;
				gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				if (gameobject_LocalRanklist_Content.transform.GetChild(selectindex + 1).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.activeSelf)
				{
					gameobject_LocalRanklist_Content.transform.GetChild(selectindex + 1).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
				}
				UpdateGamepadScrollRectPointLocal(slideDirectionDown: false);
			}
		}
	}

	private void UpdateGamepadScrollRectPointSteam(bool slideDirectionDown)
	{
		int currentRow = Mathf.CeilToInt((selectindex + 1) / Custom_ScrollRectSteam.int_widthnum);
		Custom_ScrollRectSteam.ScrollUpdate(currentRow, slideDirectionDown);
	}

	private void UpdateGamepadScrollRectPointLocal(bool slideDirectionDown)
	{
		int currentRow = Mathf.CeilToInt((selectindex + 1) / Custom_ScrollRect.int_widthnum);
		Custom_ScrollRect.ScrollUpdate(currentRow, slideDirectionDown);
	}

	public void UpdateSortingSelect()
	{
		if (selectindex == -1)
		{
			if (sortingselect <= 0)
			{
				sortingselect = 0;
				Sorting0Frame.SetActive(value: true);
				Sorting1Frame.SetActive(value: false);
			}
			else if (sortingselect >= 1)
			{
				sortingselect = 1;
				Sorting0Frame.SetActive(value: false);
				Sorting1Frame.SetActive(value: true);
			}
		}
		else
		{
			sortingselect = 0;
			Sorting0Frame.SetActive(value: false);
			Sorting1Frame.SetActive(value: false);
		}
	}

	public void Update()
	{
		_ = base.IsOpen;
	}

	public void HideInfo()
	{
		finishbuildshow._Close();
	}

	protected override void OnShow(object obj = null)
	{
		steampage = 1;
		localpage = 1;
		ShowLocalRankList();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		SEMgr.Inst.uiOpen.PlaySE();
		anima.Play("Show");
		InputChangeAtStart();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		if (SteamManager.Initialized)
		{
			NoConnect.SetActive(value: false);
			if (steamUserSateCustom.Inst.RequestAllFriendsUserStats == null)
			{
				steamUserSateCustom.Inst.GetAllFriendsUserState();
			}
		}
		else
		{
			NoConnect.SetActive(value: true);
		}
	}

	private void GamepadNextDifficultyFriendRankList(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			NextDifficultyFliendRankList();
		}
	}

	public void CompleteFriend()
	{
		FriendsLoading.SetActive(value: false);
		UpdateFriendLeaderBoard();
	}

	public void StartLoadingFriendsUserStats()
	{
		FriendsLoading.SetActive(value: true);
	}

	public void ShowSteamRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		steampage = 1;
		gameobject_loading.SetActive(value: false);
		Steam.image.color = SelectedColor;
		Local.image.color = UnselectedColor;
		gameobject_SteamRanklist.SetActive(value: true);
		segmentMyRank.SetActive(value: true);
		gameobject_SteamRanklistFrame.SetActive(value: true);
		gameobject_LocalRanklistFrame.SetActive(value: false);
		StartCoroutine(LeaderBoardDownload(slotPerPageMax * (steampage - 1) + 1, slotPerPageMax * steampage, SteamLeadBoardManager.Inst.int_currentleaderboard));
		gameobject_LocalRanklist.SetActive(value: false);
		gameobject_ButtonSwitchRankList.SetActive(value: true);
		gameobject_dificultyText.SetActive(value: true);
		InputChange();
		updateLeaderBoardName();
		page.text = steampage.ToString();
		if (gameobject_LocalRanklist_Content.transform.childCount != 0)
		{
			selectindex = 0;
			gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
		}
	}

	public IEnumerator LeaderBoardDownload(int start = 0, int end = 9, int index = 0)
	{
		gameobject_Myleaderboard_Content.transform.DestroyAllChild();
		gameobject_SteamRanklist_Content.transform.DestroyAllChild();
		while (index < 0)
		{
			index += SteamLeadBoardManager.Inst.leaderboards.Count;
		}
		index %= SteamLeadBoardManager.Inst.leaderboards.Count;
		SteamLeadBoardManager.Inst.int_currentleaderboard = index;
		gameobject_loading.SetActive(value: true);
		gameobject_loadingfail.SetActive(value: false);
		gameObject_loadingwait.SetActive(value: true);
		while (true)
		{
			if (SteamManager.Initialized)
			{
				if (SteamLeadBoardManager.Inst.GetLeadBoard(index).m_SteamLeaderboard == 0L)
				{
					if (!SteamLeadBoardManager.Inst.gettingleaderboard)
					{
						Debug.Log("获取排行榜" + index);
						SteamLeadBoardManager.Inst.TestLeaderBoard(index);
						break;
					}
					yield return new WaitForEndOfFrame();
					continue;
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
					gameobject_loading.SetActive(value: false);
					Debug.Log("排行榜和我的排名下载完成");
					break;
				}
				if (!SteamLeadBoardManager.Inst.leaderboardMineSuceed)
				{
					Debug.Log("我的排行下载失败");
					gameobject_loading.SetActive(value: false);
				}
				if (!SteamLeadBoardManager.Inst.leaderboardsSuceed)
				{
					Debug.Log("所有排行下载失败");
					if ((bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen)
					{
						GameUISingletonMono<UI_RankingList>.Inst._Close();
						gameobject_loading.SetActive(value: true);
						gameobject_loadingfail.SetActive(value: true);
						gameObject_loadingwait.SetActive(value: false);
					}
				}
				break;
			}
			gameobject_loading.SetActive(value: true);
			gameobject_loadingfail.SetActive(value: true);
			gameObject_loadingwait.SetActive(value: false);
			Debug.LogWarning("没有连接到Steam");
			break;
		}
	}

	public void SteamRankPageUpdate()
	{
		gameobject_loading.SetActive(value: false);
		Steam.image.color = SelectedColor;
		Local.image.color = UnselectedColor;
		gameobject_SteamRanklist.SetActive(value: true);
		segmentMyRank.SetActive(value: true);
		gameobject_SteamRanklistFrame.SetActive(value: true);
		gameobject_LocalRanklistFrame.SetActive(value: false);
		StartCoroutine(LeaderBoardDownload(slotPerPageMax * (steampage - 1) + 1, slotPerPageMax * steampage, SteamLeadBoardManager.Inst.int_currentleaderboard));
		gameobject_LocalRanklist.SetActive(value: false);
		updateLeaderBoardName();
		page.text = localpage.ToString();
	}

	public void FriendNextPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		steampage++;
		int count = SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count;
		if ((steampage - 1) * (slotPerPageMax + 1) > count)
		{
			steampage--;
		}
		UpdateFriendLeaderBoard();
		page.text = steampage.ToString();
	}

	public void FriendPreviousPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		steampage--;
		if (steampage < 1)
		{
			steampage = 1;
		}
		UpdateFriendLeaderBoard();
		page.text = steampage.ToString();
	}

	public void Reload()
	{
		SteamRankPageUpdate();
	}

	private void updateLeaderBoardName()
	{
		switch (CurrentFriendDifficulty)
		{
		case DifficultyType.Easy:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002601.GetText();
			break;
		case DifficultyType.Normal:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002602.GetText();
			break;
		case DifficultyType.Hard:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002603.GetText();
			break;
		case DifficultyType.Nightmare1:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002605.GetText();
			break;
		case DifficultyType.Nightmare2:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002606.GetText();
			break;
		case DifficultyType.Nightmare3:
			Leaderboardname.text = 1003203.GetText() + ": " + 1002607.GetText();
			break;
		}
	}

	public void ShowLocalRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		localpage = 1;
		selectindex = 0;
		boolSortDate = true;
		boolSortScore = true;
		Local.image.color = SelectedColor;
		Steam.image.color = UnselectedColor;
		gameobject_loading.SetActive(value: false);
		gameobject_SteamRanklist.SetActive(value: false);
		segmentMyRank.SetActive(value: false);
		UpdateLocalLeaderBoards();
		gameobject_LocalRanklist.SetActive(value: true);
		gameobject_SteamRanklistFrame.SetActive(value: false);
		gameobject_LocalRanklistFrame.SetActive(value: true);
		gameobject_dificultyText.SetActive(value: false);
		gameobject_ButtonSwitchRankList.SetActive(value: false);
		gameobject_GamepadSwitchRankListShortcut.SetActive(value: false);
	}

	public void PreviousButton()
	{
		if (gameobject_LocalRanklist.activeSelf)
		{
			PreviousLocalPage();
		}
		else
		{
			FriendPreviousPage();
		}
	}

	public void NextButton()
	{
		if (gameobject_LocalRanklist.activeSelf)
		{
			NextLocalPage();
		}
		else
		{
			FriendNextPage();
		}
	}

	public void NextLocalPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		localpage++;
		if ((localpage - 1) * slotPerPageMax > DataMgr.finishGameBuilds.finishGameBuilds.Count)
		{
			localpage--;
		}
		UpdateLocalLeaderBoards(sort: false);
		page.text = localpage.ToString();
	}

	public void PreviousLocalPage()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		localpage--;
		if (localpage <= 0)
		{
			localpage = 1;
		}
		UpdateLocalLeaderBoards(sort: false);
		page.text = localpage.ToString();
	}

	public void NextDifficultyRankList()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		SteamLeadBoardManager.Inst.int_currentleaderboard++;
		steampage = 1;
		SteamRankPageUpdate();
	}

	public void PreviousDifficultyRankList()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		SteamLeadBoardManager.Inst.int_currentleaderboard--;
		steampage = 1;
		SteamRankPageUpdate();
	}

	public void ShowFriendRankList()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		steampage = 1;
		gameobject_loading.SetActive(value: false);
		Steam.image.color = SelectedColor;
		Local.image.color = UnselectedColor;
		gameobject_SteamRanklist.SetActive(value: true);
		segmentMyRank.SetActive(value: true);
		gameobject_SteamRanklistFrame.SetActive(value: true);
		gameobject_LocalRanklistFrame.SetActive(value: false);
		gameobject_LocalRanklist.SetActive(value: false);
		gameobject_ButtonSwitchRankList.SetActive(value: true);
		gameobject_dificultyText.SetActive(value: true);
		InputChange();
		CurrentFriendDifficulty = DifficultyType.Easy;
		UpdateFriendLeaderBoard();
		page.text = steampage.ToString();
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && gameobject_LocalRanklist_Content.transform.childCount > 0 && selectindex != -1)
		{
			gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: false);
		}
		selectindex = 0;
		updateLeaderBoardName();
	}

	public void NextDifficultyFliendRankList()
	{
		if (!base.IsOpen)
		{
			return;
		}
		SEMgr.Inst.uiSwitch.PlaySE();
		if (gameobject_SteamRanklist.activeSelf && !SteamLeadBoardManager.Inst.downloadingleaderboards)
		{
			switch (CurrentFriendDifficulty)
			{
			case DifficultyType.Easy:
				CurrentFriendDifficulty = DifficultyType.Normal;
				break;
			case DifficultyType.Normal:
				CurrentFriendDifficulty = DifficultyType.Hard;
				break;
			case DifficultyType.Hard:
				CurrentFriendDifficulty = DifficultyType.Nightmare1;
				break;
			case DifficultyType.Nightmare1:
				CurrentFriendDifficulty = DifficultyType.Nightmare2;
				break;
			case DifficultyType.Nightmare2:
				CurrentFriendDifficulty = DifficultyType.Nightmare3;
				break;
			case DifficultyType.Nightmare3:
				CurrentFriendDifficulty = DifficultyType.Easy;
				break;
			}
			steampage = 1;
			UpdateFriendLeaderBoard();
			updateLeaderBoardName();
		}
	}

	public void PreviousDifficultyFriendRankList()
	{
		if (!base.IsOpen)
		{
			return;
		}
		SEMgr.Inst.uiSwitch.PlaySE();
		if (gameobject_SteamRanklist.activeSelf && !SteamLeadBoardManager.Inst.downloadingleaderboards)
		{
			switch (CurrentFriendDifficulty)
			{
			case DifficultyType.Easy:
				CurrentFriendDifficulty = DifficultyType.Nightmare3;
				break;
			case DifficultyType.Normal:
				CurrentFriendDifficulty = DifficultyType.Easy;
				break;
			case DifficultyType.Hard:
				CurrentFriendDifficulty = DifficultyType.Normal;
				break;
			case DifficultyType.Nightmare1:
				CurrentFriendDifficulty = DifficultyType.Hard;
				break;
			case DifficultyType.Nightmare2:
				CurrentFriendDifficulty = DifficultyType.Nightmare1;
				break;
			case DifficultyType.Nightmare3:
				CurrentFriendDifficulty = DifficultyType.Nightmare2;
				break;
			}
			steampage = 1;
			UpdateFriendLeaderBoard();
			updateLeaderBoardName();
		}
	}

	public void UpdateFriendLeaderBoard()
	{
		switch (CurrentFriendDifficulty)
		{
		case DifficultyType.Easy:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNormal);
			break;
		case DifficultyType.Normal:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortHard);
			break;
		case DifficultyType.Hard:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNightmare);
			break;
		case DifficultyType.Nightmare1:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNewNightmare1);
			break;
		case DifficultyType.Nightmare2:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNewNightmare2);
			break;
		case DifficultyType.Nightmare3:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNewNightmare3);
			break;
		default:
			SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Sort(RankData.SortNormal);
			break;
		}
		StartCoroutine(UpdateMyFriendLeaderBoardIE());
	}

	public IEnumerator UpdateMyFriendLeaderBoardIE()
	{
		gameobject_Myleaderboard_Content.transform.DestroyAllChild();
		SteamLeadBoardManager.Inst.leaderboardsSuceed = true;
		gameobject_SteamRanklist_Content.transform.DestroyAllChild();
		if (SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count > 0)
		{
			gameobjet_noFriendRankingList.SetActive(value: false);
			gameobject_SteamRanklist_Content.SetActive(value: true);
			int num = 0;
			for (int i = 0; i < SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas.Count; i++)
			{
				RankData rankData = SteamLeadBoardManager.Inst.leadboardDataFriends.rankDatas[i];
				bool flag = false;
				switch (CurrentFriendDifficulty)
				{
				case DifficultyType.Easy:
					if (rankData.score != 0)
					{
						flag = true;
						num++;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugc, CurrentFriendDifficulty, num);
					}
					break;
				case DifficultyType.Normal:
					if (rankData.scorehard != 0)
					{
						num++;
						flag = true;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugc, CurrentFriendDifficulty, num);
					}
					break;
				case DifficultyType.Hard:
					if (rankData.scorenightmare != 0)
					{
						num++;
						flag = true;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugchard, CurrentFriendDifficulty, num);
					}
					break;
				case DifficultyType.Nightmare1:
					if (rankData.scoreNewNightmare1 != 0)
					{
						num++;
						flag = true;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugcNewNightmare1, CurrentFriendDifficulty, num);
					}
					break;
				case DifficultyType.Nightmare2:
					if (rankData.scoreNewNightmare2 != 0)
					{
						num++;
						flag = true;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugcNewNightmare2, CurrentFriendDifficulty, num);
					}
					break;
				case DifficultyType.Nightmare3:
					if (rankData.scoreNewNightmare3 != 0)
					{
						num++;
						flag = true;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_SteamRanklist_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugcNewNightmare3, CurrentFriendDifficulty, num);
					}
					break;
				}
				if (rankData.csteamid == SteamUser.GetSteamID() && flag)
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListSteamSlot"), gameobject_Myleaderboard_Content.transform).GetComponent<SteamRankingSlot>().InitializeFriend(rankData, rankData.ugchard, CurrentFriendDifficulty, num);
				}
			}
		}
		else
		{
			gameobjet_noFriendRankingList.SetActive(value: true);
			gameobject_SteamRanklist_Content.SetActive(value: false);
		}
		yield return new WaitForEndOfFrame();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Gamepad:
			if (gameobject_SteamRanklist.activeSelf && selectindex != -1 && gameobject_SteamRanklist_Content.transform.childCount != 0)
			{
				if (selectindex <= gameobject_SteamRanklist_Content.transform.childCount - 1)
				{
					gameobject_SteamRanklist_Content.transform.GetChild(selectindex).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
					break;
				}
				selectindex = 0;
				gameobject_SteamRanklist_Content.transform.GetChild(0).GetComponent<SteamRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		case PlayerInputType.Keyboard:
			break;
		}
	}

	public void UpdateLocalLeaderBoards(bool sort = true, bool FocusOnSorting = false)
	{
		StartCoroutine(UpdateLocalLeaderBoardsIE(sort, FocusOnSorting));
	}

	private IEnumerator UpdateLocalLeaderBoardsIE(bool sort = true, bool FocusOnSorting = false)
	{
		gameobject_LocalRanklist_Content.transform.DestroyAllChild();
		if (DataMgr.finishGameBuilds != null && DataMgr.finishGameBuilds.finishGameBuilds.Count > 0)
		{
			gameobject_nolocalrecord.SetActive(value: false);
			if (sort)
			{
				DataMgr.finishGameBuilds.finishGameBuilds.Sort(FinishGameBuild.SortByDate);
			}
			for (int i = (localpage - 1) * slotPerPageMax; i < localpage * slotPerPageMax && i != DataMgr.finishGameBuilds.finishGameBuilds.Count; i++)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRankingListLocalSlot"), gameobject_LocalRanklist_Content.transform).GetComponent<LocalRankingSlot>().initialize(DataMgr.finishGameBuilds.finishGameBuilds[i], i + 1);
			}
			yield return new WaitForEndOfFrame();
			if (FocusOnSorting)
			{
				yield break;
			}
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Gamepad:
				if (selectindex != -1 && selectindex <= gameobject_LocalRanklist_Content.transform.childCount - 1)
				{
					gameobject_LocalRanklist_Content.transform.GetChild(selectindex).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
					break;
				}
				selectindex = 0;
				gameobject_LocalRanklist_Content.transform.GetChild(0).GetComponent<LocalRankingSlot>().SelectFrame.gameObject.SetActive(value: true);
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			case PlayerInputType.Keyboard:
				break;
			}
		}
		else
		{
			gameobject_nolocalrecord.SetActive(value: true);
		}
	}

	public void SortLocalDateButton(bool sort = true)
	{
		gameobject_LocalRanklist_Content.transform.DestroyAllChild();
		if (DataMgr.finishGameBuilds == null)
		{
			return;
		}
		if (DataMgr.finishGameBuilds != null)
		{
			if (sort)
			{
				boolSortDate = !boolSortDate;
			}
			if (!boolSortDate)
			{
				DataMgr.finishGameBuilds.finishGameBuilds.Sort(FinishGameBuild.SortByDateReverse);
			}
			else
			{
				DataMgr.finishGameBuilds.finishGameBuilds.Sort(FinishGameBuild.SortByDate);
			}
		}
		UpdateLocalLeaderBoards(sort: false, FocusOnSorting: true);
	}

	public void SortLocalScroeButton(bool sort = true)
	{
		gameobject_LocalRanklist_Content.transform.DestroyAllChild();
		if (DataMgr.finishGameBuilds == null)
		{
			return;
		}
		if (DataMgr.finishGameBuilds != null)
		{
			if (sort)
			{
				boolSortScore = !boolSortScore;
			}
			if (!boolSortScore)
			{
				DataMgr.finishGameBuilds.finishGameBuilds.Sort(FinishGameBuild.SortByScoreReverse);
			}
			else
			{
				DataMgr.finishGameBuilds.finishGameBuilds.Sort(FinishGameBuild.SortByScore);
			}
		}
		UpdateLocalLeaderBoards(sort: false, FocusOnSorting: true);
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
