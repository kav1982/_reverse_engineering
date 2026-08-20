using Steamworks;
using UnityEngine;

public class SteamFriendsTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private CSteamID m_Friend;

	private CSteamID m_Clan;

	private CSteamID m_CoPlayFriend;

	private Texture2D m_SmallAvatar;

	private Texture2D m_MediumAvatar;

	private Texture2D m_LargeAvatar;

	protected Callback<PersonaStateChange_t> m_PersonaStateChange;

	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	protected Callback<GameServerChangeRequested_t> m_GameServerChangeRequested;

	protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;

	protected Callback<AvatarImageLoaded_t> m_AvatarImageLoaded;

	protected Callback<FriendRichPresenceUpdate_t> m_FriendRichPresenceUpdate;

	protected Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested;

	protected Callback<GameConnectedClanChatMsg_t> m_GameConnectedClanChatMsg;

	protected Callback<GameConnectedChatJoin_t> m_GameConnectedChatJoin;

	protected Callback<GameConnectedChatLeave_t> m_GameConnectedChatLeave;

	protected Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFriendChatMsg;

	protected Callback<UnreadChatMessagesChanged_t> m_UnreadChatMessagesChanged;

	protected Callback<OverlayBrowserProtocolNavigation_t> m_OverlayBrowserProtocolNavigation;

	protected Callback<EquippedProfileItemsChanged_t> m_EquippedProfileItemsChanged;

	private CallResult<ClanOfficerListResponse_t> OnClanOfficerListResponseCallResult;

	private CallResult<DownloadClanActivityCountsResult_t> OnDownloadClanActivityCountsResultCallResult;

	private CallResult<JoinClanChatRoomCompletionResult_t> OnJoinClanChatRoomCompletionResultCallResult;

	private CallResult<FriendsGetFollowerCount_t> OnFriendsGetFollowerCountCallResult;

	private CallResult<FriendsIsFollowing_t> OnFriendsIsFollowingCallResult;

	private CallResult<FriendsEnumerateFollowingList_t> OnFriendsEnumerateFollowingListCallResult;

	private CallResult<SetPersonaNameResponse_t> OnSetPersonaNameResponseCallResult;

	private CallResult<EquippedProfileItems_t> OnEquippedProfileItemsCallResult;

	public void OnEnable()
	{
		if (SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate) == 0)
		{
			Debug.LogError("You must have atleast one friend to use the SteamFriends test!");
			base.enabled = false;
			return;
		}
		if (SteamFriends.GetClanCount() == 0)
		{
			Debug.LogError("You must have atleast one clan to use the SteamFriends test!");
			base.enabled = false;
			return;
		}
		m_PersonaStateChange = Callback<PersonaStateChange_t>.Create(OnPersonaStateChange);
		m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		m_GameServerChangeRequested = Callback<GameServerChangeRequested_t>.Create(OnGameServerChangeRequested);
		m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
		m_AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
		m_FriendRichPresenceUpdate = Callback<FriendRichPresenceUpdate_t>.Create(OnFriendRichPresenceUpdate);
		m_GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
		m_GameConnectedClanChatMsg = Callback<GameConnectedClanChatMsg_t>.Create(OnGameConnectedClanChatMsg);
		m_GameConnectedChatJoin = Callback<GameConnectedChatJoin_t>.Create(OnGameConnectedChatJoin);
		m_GameConnectedChatLeave = Callback<GameConnectedChatLeave_t>.Create(OnGameConnectedChatLeave);
		m_GameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnGameConnectedFriendChatMsg);
		m_UnreadChatMessagesChanged = Callback<UnreadChatMessagesChanged_t>.Create(OnUnreadChatMessagesChanged);
		m_OverlayBrowserProtocolNavigation = Callback<OverlayBrowserProtocolNavigation_t>.Create(OnOverlayBrowserProtocolNavigation);
		m_EquippedProfileItemsChanged = Callback<EquippedProfileItemsChanged_t>.Create(OnEquippedProfileItemsChanged);
		OnClanOfficerListResponseCallResult = CallResult<ClanOfficerListResponse_t>.Create(OnClanOfficerListResponse);
		OnDownloadClanActivityCountsResultCallResult = CallResult<DownloadClanActivityCountsResult_t>.Create(OnDownloadClanActivityCountsResult);
		OnJoinClanChatRoomCompletionResultCallResult = CallResult<JoinClanChatRoomCompletionResult_t>.Create(OnJoinClanChatRoomCompletionResult);
		OnFriendsGetFollowerCountCallResult = CallResult<FriendsGetFollowerCount_t>.Create(OnFriendsGetFollowerCount);
		OnFriendsIsFollowingCallResult = CallResult<FriendsIsFollowing_t>.Create(OnFriendsIsFollowing);
		OnFriendsEnumerateFollowingListCallResult = CallResult<FriendsEnumerateFollowingList_t>.Create(OnFriendsEnumerateFollowingList);
		OnSetPersonaNameResponseCallResult = CallResult<SetPersonaNameResponse_t>.Create(OnSetPersonaNameResponse);
		OnEquippedProfileItemsCallResult = CallResult<EquippedProfileItems_t>.Create(OnEquippedProfileItems);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		CSteamID friend = m_Friend;
		GUILayout.Label("m_Friend: " + friend.ToString());
		friend = m_Clan;
		GUILayout.Label("m_Clan: " + friend.ToString());
		friend = m_CoPlayFriend;
		GUILayout.Label("m_CoPlayFriend: " + friend.ToString());
		GUILayout.Label("m_SmallAvatar:");
		GUILayout.Label(m_SmallAvatar);
		GUILayout.Label("m_MediumAvatar:");
		GUILayout.Label(m_MediumAvatar);
		GUILayout.Label("m_LargeAvatar:");
		GUILayout.Label(m_LargeAvatar);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("GetPersonaName() : " + SteamFriends.GetPersonaName());
		if (GUILayout.Button("SetPersonaName(SteamFriends.GetPersonaName())"))
		{
			SteamAPICall_t steamAPICall_t = SteamFriends.SetPersonaName(SteamFriends.GetPersonaName());
			OnSetPersonaNameResponseCallResult.Set(steamAPICall_t);
			string personaName = SteamFriends.GetPersonaName();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamFriends.SetPersonaName(" + personaName + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("GetPersonaState() : " + SteamFriends.GetPersonaState());
		GUILayout.Label("GetFriendCount(EFriendFlags.k_EFriendFlagImmediate) : " + SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate));
		m_Friend = SteamFriends.GetFriendByIndex(0, EFriendFlags.k_EFriendFlagImmediate);
		friend = m_Friend;
		GUILayout.Label("GetFriendByIndex(0, EFriendFlags.k_EFriendFlagImmediate) : " + friend.ToString());
		GUILayout.Label("GetFriendRelationship(m_Friend) : " + SteamFriends.GetFriendRelationship(m_Friend));
		GUILayout.Label("GetFriendPersonaState(m_Friend) : " + SteamFriends.GetFriendPersonaState(m_Friend));
		GUILayout.Label("GetFriendPersonaName(m_Friend) : " + SteamFriends.GetFriendPersonaName(m_Friend));
		FriendGameInfo_t pFriendGameInfo = default(FriendGameInfo_t);
		bool friendGamePlayed = SteamFriends.GetFriendGamePlayed(m_Friend, out pFriendGameInfo);
		string[] obj = new string[12]
		{
			"GetFriendGamePlayed(m_Friend, out fgi) : ",
			friendGamePlayed.ToString(),
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
		CGameID gameID = pFriendGameInfo.m_gameID;
		obj[3] = gameID.ToString();
		obj[4] = " -- ";
		obj[5] = pFriendGameInfo.m_unGameIP.ToString();
		obj[6] = " -- ";
		obj[7] = pFriendGameInfo.m_usGamePort.ToString();
		obj[8] = " -- ";
		obj[9] = pFriendGameInfo.m_usQueryPort.ToString();
		obj[10] = " -- ";
		friend = pFriendGameInfo.m_steamIDLobby;
		obj[11] = friend.ToString();
		GUILayout.Label(string.Concat(obj));
		GUILayout.Label("GetFriendPersonaNameHistory(m_Friend, 1) : " + SteamFriends.GetFriendPersonaNameHistory(m_Friend, 1));
		GUILayout.Label("GetFriendSteamLevel(m_Friend) : " + SteamFriends.GetFriendSteamLevel(m_Friend));
		GUILayout.Label("GetPlayerNickname(m_Friend) : " + SteamFriends.GetPlayerNickname(m_Friend));
		int friendsGroupCount = SteamFriends.GetFriendsGroupCount();
		GUILayout.Label("GetFriendsGroupCount() : " + friendsGroupCount);
		if (friendsGroupCount > 0)
		{
			FriendsGroupID_t friendsGroupIDByIndex = SteamFriends.GetFriendsGroupIDByIndex(0);
			FriendsGroupID_t friendsGroupID_t = friendsGroupIDByIndex;
			GUILayout.Label("SteamFriends.GetFriendsGroupIDByIndex(0) : " + friendsGroupID_t.ToString());
			GUILayout.Label("GetFriendsGroupName(FriendsGroupID) : " + SteamFriends.GetFriendsGroupName(friendsGroupIDByIndex));
			int friendsGroupMembersCount = SteamFriends.GetFriendsGroupMembersCount(friendsGroupIDByIndex);
			GUILayout.Label("GetFriendsGroupMembersCount(FriendsGroupID) : " + friendsGroupMembersCount);
			if (friendsGroupMembersCount > 0)
			{
				CSteamID[] array = new CSteamID[friendsGroupMembersCount];
				SteamFriends.GetFriendsGroupMembersList(friendsGroupIDByIndex, array, friendsGroupMembersCount);
				friend = array[0];
				GUILayout.Label("GetFriendsGroupMembersList(FriendsGroupID, FriendsGroupMembersList, FriendsGroupMembersCount) : " + friend.ToString());
			}
		}
		GUILayout.Label("HasFriend(m_Friend, EFriendFlags.k_EFriendFlagImmediate) : " + SteamFriends.HasFriend(m_Friend, EFriendFlags.k_EFriendFlagImmediate));
		GUILayout.Label("GetClanCount() : " + SteamFriends.GetClanCount());
		m_Clan = SteamFriends.GetClanByIndex(0);
		friend = m_Clan;
		GUILayout.Label("GetClanByIndex(0) : " + friend.ToString());
		GUILayout.Label("GetClanName(m_Clan) : " + SteamFriends.GetClanName(m_Clan));
		GUILayout.Label("GetClanTag(m_Clan) : " + SteamFriends.GetClanTag(m_Clan));
		int pnOnline;
		int pnInGame;
		int pnChatting;
		bool clanActivityCounts = SteamFriends.GetClanActivityCounts(m_Clan, out pnOnline, out pnInGame, out pnChatting);
		GUILayout.Label("GetClanActivityCounts(m_Clan, out Online, out InGame, out Chatting) : " + clanActivityCounts + " -- " + pnOnline + " -- " + pnInGame + " -- " + pnChatting);
		if (GUILayout.Button("DownloadClanActivityCounts(Clans, Clans.Length)"))
		{
			CSteamID[] array2 = new CSteamID[2]
			{
				m_Clan,
				TestConstants.Instance.k_SteamId_Group_SteamUniverse
			};
			SteamAPICall_t steamAPICall_t3 = SteamFriends.DownloadClanActivityCounts(array2, array2.Length);
			OnDownloadClanActivityCountsResultCallResult.Set(steamAPICall_t3);
			OnDownloadClanActivityCountsResultCallResult.Set(steamAPICall_t3);
			string[] obj2 = new string[6]
			{
				"SteamFriends.DownloadClanActivityCounts(",
				array2?.ToString(),
				", ",
				array2.Length.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			obj2[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		int friendCountFromSource = SteamFriends.GetFriendCountFromSource(m_Clan);
		GUILayout.Label("GetFriendCountFromSource(m_Clan) : " + friendCountFromSource);
		if (friendCountFromSource > 0)
		{
			GUILayout.Label("GetFriendFromSourceByIndex(m_Clan, 0) : " + SteamFriends.GetFriendFromSourceByIndex(m_Clan, 0).ToString());
		}
		GUILayout.Label("IsUserInSource(m_Friend, m_Clan) : " + SteamFriends.IsUserInSource(m_Friend, m_Clan));
		if (GUILayout.Button("SetInGameVoiceSpeaking(SteamUser.GetSteamID(), false)"))
		{
			SteamFriends.SetInGameVoiceSpeaking(SteamUser.GetSteamID(), bSpeaking: false);
			MonoBehaviour.print("SteamFriends.SetInGameVoiceSpeaking(" + SteamUser.GetSteamID().ToString() + ", " + false + ")");
		}
		if (GUILayout.Button("ActivateGameOverlay(\"Friends\")"))
		{
			SteamFriends.ActivateGameOverlay("Friends");
			MonoBehaviour.print("SteamFriends.ActivateGameOverlay(\"Friends\")");
		}
		if (GUILayout.Button("ActivateGameOverlayToUser(\"friendadd\", TestConstants.Instance.k_SteamId_rlabrecque)"))
		{
			SteamFriends.ActivateGameOverlayToUser("friendadd", TestConstants.Instance.k_SteamId_rlabrecque);
			friend = TestConstants.Instance.k_SteamId_rlabrecque;
			MonoBehaviour.print("SteamFriends.ActivateGameOverlayToUser(\"friendadd\", " + friend.ToString() + ")");
		}
		if (GUILayout.Button("ActivateGameOverlayToWebPage(\"http://steamworks.github.io\")"))
		{
			SteamFriends.ActivateGameOverlayToWebPage("http://steamworks.github.io");
			MonoBehaviour.print("SteamFriends.ActivateGameOverlayToWebPage(\"http://steamworks.github.io\")");
		}
		if (GUILayout.Button("ActivateGameOverlayToStore(TestConstants.Instance.k_AppId_TeamFortress2, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)"))
		{
			SteamFriends.ActivateGameOverlayToStore(TestConstants.Instance.k_AppId_TeamFortress2, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			string[] obj3 = new string[5] { "SteamFriends.ActivateGameOverlayToStore(", null, null, null, null };
			AppId_t k_AppId_TeamFortress = TestConstants.Instance.k_AppId_TeamFortress2;
			obj3[1] = k_AppId_TeamFortress.ToString();
			obj3[2] = ", ";
			obj3[3] = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None.ToString();
			obj3[4] = ")";
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("SetPlayedWith(TestConstants.Instance.k_SteamId_rlabrecque)"))
		{
			SteamFriends.SetPlayedWith(TestConstants.Instance.k_SteamId_rlabrecque);
			friend = TestConstants.Instance.k_SteamId_rlabrecque;
			MonoBehaviour.print("SteamFriends.SetPlayedWith(" + friend.ToString() + ")");
		}
		if (GUILayout.Button("ActivateGameOverlayInviteDialog(TestConstants.Instance.k_SteamId_rlabrecque)"))
		{
			SteamFriends.ActivateGameOverlayInviteDialog(TestConstants.Instance.k_SteamId_rlabrecque);
			friend = TestConstants.Instance.k_SteamId_rlabrecque;
			MonoBehaviour.print("SteamFriends.ActivateGameOverlayInviteDialog(" + friend.ToString() + ")");
		}
		if (GUILayout.Button("GetSmallFriendAvatar(m_Friend)"))
		{
			int smallFriendAvatar = SteamFriends.GetSmallFriendAvatar(m_Friend);
			friend = m_Friend;
			MonoBehaviour.print("SteamFriends.GetSmallFriendAvatar(" + friend.ToString() + ") : " + smallFriendAvatar);
			m_SmallAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(smallFriendAvatar);
		}
		if (GUILayout.Button("GetMediumFriendAvatar(m_Friend)"))
		{
			int mediumFriendAvatar = SteamFriends.GetMediumFriendAvatar(m_Friend);
			friend = m_Friend;
			MonoBehaviour.print("SteamFriends.GetMediumFriendAvatar(" + friend.ToString() + ") : " + mediumFriendAvatar);
			m_MediumAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(mediumFriendAvatar);
		}
		if (GUILayout.Button("GetLargeFriendAvatar(m_Friend)"))
		{
			int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(m_Friend);
			friend = m_Friend;
			MonoBehaviour.print("SteamFriends.GetLargeFriendAvatar(" + friend.ToString() + ") : " + largeFriendAvatar);
			m_LargeAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(largeFriendAvatar);
		}
		if (GUILayout.Button("RequestUserInformation(m_Friend, false)"))
		{
			bool flag = SteamFriends.RequestUserInformation(m_Friend, bRequireNameOnly: false);
			string[] obj4 = new string[6] { "SteamFriends.RequestUserInformation(", null, null, null, null, null };
			friend = m_Friend;
			obj4[1] = friend.ToString();
			obj4[2] = ", ";
			obj4[3] = false.ToString();
			obj4[4] = ") : ";
			obj4[5] = flag.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("RequestClanOfficerList(m_Clan)"))
		{
			SteamAPICall_t steamAPICall_t4 = SteamFriends.RequestClanOfficerList(m_Clan);
			OnClanOfficerListResponseCallResult.Set(steamAPICall_t4);
			friend = m_Clan;
			string text = friend.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t4;
			MonoBehaviour.print("SteamFriends.RequestClanOfficerList(" + text + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("GetClanOwner(m_Clan) : " + SteamFriends.GetClanOwner(m_Clan).ToString());
		GUILayout.Label("GetClanOfficerCount(m_Clan) : " + SteamFriends.GetClanOfficerCount(m_Clan));
		GUILayout.Label("GetClanOfficerByIndex(m_Clan, 0) : " + SteamFriends.GetClanOfficerByIndex(m_Clan, 0).ToString());
		GUILayout.Label("GetUserRestrictions() : " + SteamFriends.GetUserRestrictions());
		if (GUILayout.Button("SetRichPresence(\"status\", \"Testing 1.. 2.. 3..\")"))
		{
			MonoBehaviour.print("SteamFriends.SetRichPresence(\"status\", \"Testing 1.. 2.. 3..\") : " + SteamFriends.SetRichPresence("status", "Testing 1.. 2.. 3.."));
		}
		if (GUILayout.Button("ClearRichPresence()"))
		{
			SteamFriends.ClearRichPresence();
			MonoBehaviour.print("SteamFriends.ClearRichPresence()");
		}
		GUILayout.Label("GetFriendRichPresence(SteamUser.GetSteamID(), \"status\") : " + SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), "status"));
		GUILayout.Label("GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()) : " + SteamFriends.GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()));
		GUILayout.Label("GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0) : " + SteamFriends.GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0));
		if (GUILayout.Button("RequestFriendRichPresence(m_Friend)"))
		{
			SteamFriends.RequestFriendRichPresence(m_Friend);
			friend = m_Friend;
			MonoBehaviour.print("SteamFriends.RequestFriendRichPresence(" + friend.ToString() + ")");
		}
		if (GUILayout.Button("InviteUserToGame(SteamUser.GetSteamID(), \"testing\")"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamFriends.InviteUserToGame(SteamUser.GetSteamID(), "testing").ToString(), str0: "SteamFriends.InviteUserToGame(", str1: SteamUser.GetSteamID().ToString(), str2: ", \"testing\") : "));
		}
		GUILayout.Label("GetCoplayFriendCount() : " + SteamFriends.GetCoplayFriendCount());
		if (GUILayout.Button("GetCoplayFriend(0)"))
		{
			m_CoPlayFriend = SteamFriends.GetCoplayFriend(0);
			string text2 = 0.ToString();
			friend = m_CoPlayFriend;
			MonoBehaviour.print("SteamFriends.GetCoplayFriend(" + text2 + ") : " + friend.ToString());
		}
		GUILayout.Label("GetFriendCoplayTime(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayTime(m_CoPlayFriend));
		GUILayout.Label("GetFriendCoplayGame(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayGame(m_CoPlayFriend).ToString());
		if (GUILayout.Button("JoinClanChatRoom(m_Clan)"))
		{
			SteamAPICall_t steamAPICall_t5 = SteamFriends.JoinClanChatRoom(m_Clan);
			OnJoinClanChatRoomCompletionResultCallResult.Set(steamAPICall_t5);
			friend = m_Clan;
			string text3 = friend.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t5;
			MonoBehaviour.print("SteamFriends.JoinClanChatRoom(" + text3 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("LeaveClanChatRoom(m_Clan)"))
		{
			bool flag2 = SteamFriends.LeaveClanChatRoom(m_Clan);
			friend = m_Clan;
			MonoBehaviour.print("SteamFriends.LeaveClanChatRoom(" + friend.ToString() + ") : " + flag2);
		}
		GUILayout.Label("GetClanChatMemberCount(m_Clan) : " + SteamFriends.GetClanChatMemberCount(m_Clan));
		GUILayout.Label("GetChatMemberByIndex(m_Clan, 0) : " + SteamFriends.GetChatMemberByIndex(m_Clan, 0).ToString());
		if (GUILayout.Button("SendClanChatMessage(m_Clan, \"Test\")"))
		{
			bool flag3 = SteamFriends.SendClanChatMessage(m_Clan, "Test");
			friend = m_Clan;
			MonoBehaviour.print("SteamFriends.SendClanChatMessage(" + friend.ToString() + ", \"Test\") : " + flag3);
		}
		GUILayout.Label("IsClanChatAdmin(m_Clan, m_Friend) : " + SteamFriends.IsClanChatAdmin(m_Clan, m_Friend));
		GUILayout.Label("IsClanChatWindowOpenInSteam(m_Clan) : " + SteamFriends.IsClanChatWindowOpenInSteam(m_Clan));
		if (GUILayout.Button("OpenClanChatWindowInSteam(m_Clan)"))
		{
			bool flag4 = SteamFriends.OpenClanChatWindowInSteam(m_Clan);
			friend = m_Clan;
			MonoBehaviour.print("SteamFriends.OpenClanChatWindowInSteam(" + friend.ToString() + ") : " + flag4);
		}
		if (GUILayout.Button("CloseClanChatWindowInSteam(m_Clan)"))
		{
			bool flag5 = SteamFriends.CloseClanChatWindowInSteam(m_Clan);
			friend = m_Clan;
			MonoBehaviour.print("SteamFriends.CloseClanChatWindowInSteam(" + friend.ToString() + ") : " + flag5);
		}
		if (GUILayout.Button("SetListenForFriendsMessages(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamFriends.SetListenForFriendsMessages(bInterceptEnabled: true).ToString(), str0: "SteamFriends.SetListenForFriendsMessages(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("ReplyToFriendMessage(SteamUser.GetSteamID(), \"Testing!\")"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamFriends.ReplyToFriendMessage(SteamUser.GetSteamID(), "Testing!").ToString(), str0: "SteamFriends.ReplyToFriendMessage(", str1: SteamUser.GetSteamID().ToString(), str2: ", \"Testing!\") : "));
		}
		if (GUILayout.Button("GetFollowerCount(SteamUser.GetSteamID())"))
		{
			SteamAPICall_t followerCount = SteamFriends.GetFollowerCount(SteamUser.GetSteamID());
			OnFriendsGetFollowerCountCallResult.Set(followerCount);
			string text4 = SteamUser.GetSteamID().ToString();
			SteamAPICall_t steamAPICall_t2 = followerCount;
			MonoBehaviour.print("SteamFriends.GetFollowerCount(" + text4 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("IsFollowing(m_Friend)"))
		{
			SteamAPICall_t steamAPICall_t6 = SteamFriends.IsFollowing(m_Friend);
			OnFriendsIsFollowingCallResult.Set(steamAPICall_t6);
			friend = m_Friend;
			string text5 = friend.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t6;
			MonoBehaviour.print("SteamFriends.IsFollowing(" + text5 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("EnumerateFollowingList(0)"))
		{
			SteamAPICall_t steamAPICall_t7 = SteamFriends.EnumerateFollowingList(0u);
			OnFriendsEnumerateFollowingListCallResult.Set(steamAPICall_t7);
			string text6 = 0.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t7;
			MonoBehaviour.print("SteamFriends.EnumerateFollowingList(" + text6 + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("IsClanPublic(m_Clan) : " + SteamFriends.IsClanPublic(m_Clan));
		GUILayout.Label("IsClanOfficialGameGroup(m_Clan) : " + SteamFriends.IsClanOfficialGameGroup(m_Clan));
		GUILayout.Label("GetNumChatsWithUnreadPriorityMessages() : " + SteamFriends.GetNumChatsWithUnreadPriorityMessages());
		if (GUILayout.Button("ActivateGameOverlayRemotePlayTogetherInviteDialog(m_Friend)"))
		{
			SteamFriends.ActivateGameOverlayRemotePlayTogetherInviteDialog(m_Friend);
			friend = m_Friend;
			MonoBehaviour.print("SteamFriends.ActivateGameOverlayRemotePlayTogetherInviteDialog(" + friend.ToString() + ")");
		}
		if (GUILayout.Button("RegisterProtocolInOverlayBrowser(\"test\")"))
		{
			MonoBehaviour.print("SteamFriends.RegisterProtocolInOverlayBrowser(\"test\") : " + SteamFriends.RegisterProtocolInOverlayBrowser("test"));
		}
		if (GUILayout.Button("ActivateGameOverlayInviteDialogConnectString(\"test\")"))
		{
			SteamFriends.ActivateGameOverlayInviteDialogConnectString("test");
			MonoBehaviour.print("SteamFriends.ActivateGameOverlayInviteDialogConnectString(\"test\")");
		}
		if (GUILayout.Button("RequestEquippedProfileItems(SteamUser.GetSteamID())"))
		{
			SteamAPICall_t steamAPICall_t8 = SteamFriends.RequestEquippedProfileItems(SteamUser.GetSteamID());
			OnEquippedProfileItemsCallResult.Set(steamAPICall_t8);
			string text7 = SteamUser.GetSteamID().ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t8;
			MonoBehaviour.print("SteamFriends.RequestEquippedProfileItems(" + text7 + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("BHasEquippedProfileItem(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame) : " + SteamFriends.BHasEquippedProfileItem(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame));
		GUILayout.Label("GetProfileItemPropertyString(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame, ECommunityProfileItemProperty.k_ECommunityProfileItemProperty_Title) : " + SteamFriends.GetProfileItemPropertyString(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame, ECommunityProfileItemProperty.k_ECommunityProfileItemProperty_Title));
		GUILayout.Label("GetProfileItemPropertyUint(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame, ECommunityProfileItemProperty.k_ECommunityProfileItemProperty_AppID) : " + SteamFriends.GetProfileItemPropertyUint(SteamUser.GetSteamID(), ECommunityProfileItemType.k_ECommunityProfileItemType_AvatarFrame, ECommunityProfileItemProperty.k_ECommunityProfileItemProperty_AppID));
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnPersonaStateChange(PersonaStateChange_t pCallback)
	{
		Debug.Log("[" + 304 + " - PersonaStateChange] - " + pCallback.m_ulSteamID + " -- " + pCallback.m_nChangeFlags);
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			331.ToString(),
			" - GameOverlayActivated] - ",
			pCallback.m_bActive.ToString(),
			" -- ",
			pCallback.m_bUserInitiated.ToString(),
			" -- ",
			null,
			null,
			null
		};
		AppId_t nAppID = pCallback.m_nAppID;
		obj[7] = nAppID.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_dwOverlayPID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGameServerChangeRequested(GameServerChangeRequested_t pCallback)
	{
		Debug.Log("[" + 332 + " - GameServerChangeRequested] - " + pCallback.m_rgchServer + " -- " + pCallback.m_rgchPassword);
	}

	private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			333.ToString(),
			" - GameLobbyJoinRequested] - ",
			null,
			null,
			null
		};
		CSteamID steamIDLobby = pCallback.m_steamIDLobby;
		obj[3] = steamIDLobby.ToString();
		obj[4] = " -- ";
		steamIDLobby = pCallback.m_steamIDFriend;
		obj[5] = steamIDLobby.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnAvatarImageLoaded(AvatarImageLoaded_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			334.ToString(),
			" - AvatarImageLoaded] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamID = pCallback.m_steamID;
		obj[3] = steamID.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_iImage.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_iWide.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_iTall.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnClanOfficerListResponse(ClanOfficerListResponse_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			335.ToString(),
			" - ClanOfficerListResponse] - ",
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamIDClan = pCallback.m_steamIDClan;
		obj[3] = steamIDClan.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_cOfficers.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bSuccess.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnFriendRichPresenceUpdate(FriendRichPresenceUpdate_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			336.ToString(),
			" - FriendRichPresenceUpdate] - ",
			null,
			null,
			null
		};
		CSteamID steamIDFriend = pCallback.m_steamIDFriend;
		obj[3] = steamIDFriend.ToString();
		obj[4] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			337.ToString(),
			" - GameRichPresenceJoinRequested] - ",
			null,
			null,
			null
		};
		CSteamID steamIDFriend = pCallback.m_steamIDFriend;
		obj[3] = steamIDFriend.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_rgchConnect;
		Debug.Log(string.Concat(obj));
	}

	private void OnGameConnectedClanChatMsg(GameConnectedClanChatMsg_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			338.ToString(),
			" - GameConnectedClanChatMsg] - ",
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamIDClanChat = pCallback.m_steamIDClanChat;
		obj[3] = steamIDClanChat.ToString();
		obj[4] = " -- ";
		steamIDClanChat = pCallback.m_steamIDUser;
		obj[5] = steamIDClanChat.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_iMessageID.ToString();
		Debug.Log(string.Concat(obj));
		string prgchText;
		EChatEntryType peChatEntryType;
		CSteamID psteamidChatter;
		int clanChatMessage = SteamFriends.GetClanChatMessage(pCallback.m_steamIDClanChat, pCallback.m_iMessageID, out prgchText, 2048, out peChatEntryType, out psteamidChatter);
		string[] obj2 = new string[5]
		{
			clanChatMessage.ToString(),
			" ",
			null,
			null,
			null
		};
		steamIDClanChat = psteamidChatter;
		obj2[2] = steamIDClanChat.ToString();
		obj2[3] = ": ";
		obj2[4] = prgchText;
		MonoBehaviour.print(string.Concat(obj2));
	}

	private void OnGameConnectedChatJoin(GameConnectedChatJoin_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			339.ToString(),
			" - GameConnectedChatJoin] - ",
			null,
			null,
			null
		};
		CSteamID steamIDClanChat = pCallback.m_steamIDClanChat;
		obj[3] = steamIDClanChat.ToString();
		obj[4] = " -- ";
		steamIDClanChat = pCallback.m_steamIDUser;
		obj[5] = steamIDClanChat.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGameConnectedChatLeave(GameConnectedChatLeave_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			340.ToString(),
			" - GameConnectedChatLeave] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamIDClanChat = pCallback.m_steamIDClanChat;
		obj[3] = steamIDClanChat.ToString();
		obj[4] = " -- ";
		steamIDClanChat = pCallback.m_steamIDUser;
		obj[5] = steamIDClanChat.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bKicked.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_bDropped.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnDownloadClanActivityCountsResult(DownloadClanActivityCountsResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 341 + " - DownloadClanActivityCountsResult] - " + pCallback.m_bSuccess);
	}

	private void OnJoinClanChatRoomCompletionResult(JoinClanChatRoomCompletionResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			342.ToString(),
			" - JoinClanChatRoomCompletionResult] - ",
			null,
			null,
			null
		};
		CSteamID steamIDClanChat = pCallback.m_steamIDClanChat;
		obj[3] = steamIDClanChat.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eChatRoomEnterResponse.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGameConnectedFriendChatMsg(GameConnectedFriendChatMsg_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			343.ToString(),
			" - GameConnectedFriendChatMsg] - ",
			null,
			null,
			null
		};
		CSteamID steamIDUser = pCallback.m_steamIDUser;
		obj[3] = steamIDUser.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_iMessageID.ToString();
		Debug.Log(string.Concat(obj));
		string pvData;
		EChatEntryType peChatEntryType;
		int friendMessage = SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out pvData, 2048, out peChatEntryType);
		string[] obj2 = new string[5]
		{
			friendMessage.ToString(),
			" ",
			null,
			null,
			null
		};
		steamIDUser = pCallback.m_steamIDUser;
		obj2[2] = steamIDUser.ToString();
		obj2[3] = ": ";
		obj2[4] = pvData;
		MonoBehaviour.print(string.Concat(obj2));
	}

	private void OnFriendsGetFollowerCount(FriendsGetFollowerCount_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			344.ToString(),
			" - FriendsGetFollowerCount] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		CSteamID steamID = pCallback.m_steamID;
		obj[5] = steamID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_nCount.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnFriendsIsFollowing(FriendsIsFollowing_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			345.ToString(),
			" - FriendsIsFollowing] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		CSteamID steamID = pCallback.m_steamID;
		obj[5] = steamID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bIsFollowing.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnFriendsEnumerateFollowingList(FriendsEnumerateFollowingList_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 346 + " - FriendsEnumerateFollowingList] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_rgSteamID?.ToString() + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount);
	}

	private void OnSetPersonaNameResponse(SetPersonaNameResponse_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 347 + " - SetPersonaNameResponse] - " + pCallback.m_bSuccess + " -- " + pCallback.m_bLocalSuccess + " -- " + pCallback.m_result);
	}

	private void OnUnreadChatMessagesChanged(UnreadChatMessagesChanged_t pCallback)
	{
		Debug.Log("[" + 348 + " - UnreadChatMessagesChanged]");
	}

	private void OnOverlayBrowserProtocolNavigation(OverlayBrowserProtocolNavigation_t pCallback)
	{
		Debug.Log("[" + 349 + " - OverlayBrowserProtocolNavigation] - " + pCallback.rgchURI);
	}

	private void OnEquippedProfileItemsChanged(EquippedProfileItemsChanged_t pCallback)
	{
		string text = 350.ToString();
		CSteamID steamID = pCallback.m_steamID;
		Debug.Log("[" + text + " - EquippedProfileItemsChanged] - " + steamID.ToString());
	}

	private void OnEquippedProfileItems(EquippedProfileItems_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[16]
		{
			"[",
			351.ToString(),
			" - EquippedProfileItems] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
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
		CSteamID steamID = pCallback.m_steamID;
		obj[5] = steamID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bHasAnimatedAvatar.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_bHasAvatarFrame.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_bHasProfileModifier.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_bHasProfileBackground.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_bHasMiniProfileBackground.ToString();
		Debug.Log(string.Concat(obj));
	}
}
