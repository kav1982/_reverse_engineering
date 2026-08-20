using System;
using System.Text;
using Steamworks;
using UnityEngine;

public class SteamMatchmakingTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private CSteamID m_Lobby;

	protected Callback<FavoritesListChanged_t> m_FavoritesListChanged;

	protected Callback<LobbyInvite_t> m_LobbyInvite;

	protected Callback<LobbyEnter_t> m_LobbyEnter;

	protected Callback<LobbyDataUpdate_t> m_LobbyDataUpdate;

	protected Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;

	protected Callback<LobbyChatMsg_t> m_LobbyChatMsg;

	protected Callback<LobbyGameCreated_t> m_LobbyGameCreated;

	protected Callback<LobbyKicked_t> m_LobbyKicked;

	protected Callback<FavoritesListAccountsUpdated_t> m_FavoritesListAccountsUpdated;

	protected Callback<SearchForGameProgressCallback_t> m_SearchForGameProgressCallback;

	protected Callback<SearchForGameResultCallback_t> m_SearchForGameResultCallback;

	protected Callback<RequestPlayersForGameProgressCallback_t> m_RequestPlayersForGameProgressCallback;

	protected Callback<RequestPlayersForGameResultCallback_t> m_RequestPlayersForGameResultCallback;

	protected Callback<RequestPlayersForGameFinalResultCallback_t> m_RequestPlayersForGameFinalResultCallback;

	protected Callback<SubmitPlayerResultResultCallback_t> m_SubmitPlayerResultResultCallback;

	protected Callback<EndGameResultCallback_t> m_EndGameResultCallback;

	protected Callback<JoinPartyCallback_t> m_JoinPartyCallback;

	protected Callback<CreateBeaconCallback_t> m_CreateBeaconCallback;

	protected Callback<ReservationNotificationCallback_t> m_ReservationNotificationCallback;

	protected Callback<ChangeNumOpenSlotsCallback_t> m_ChangeNumOpenSlotsCallback;

	protected Callback<AvailableBeaconLocationsUpdated_t> m_AvailableBeaconLocationsUpdated;

	protected Callback<ActiveBeaconsUpdated_t> m_ActiveBeaconsUpdated;

	private CallResult<LobbyEnter_t> OnLobbyEnterCallResult;

	private CallResult<LobbyMatchList_t> OnLobbyMatchListCallResult;

	private CallResult<LobbyCreated_t> OnLobbyCreatedCallResult;

	public void OnEnable()
	{
		m_FavoritesListChanged = Callback<FavoritesListChanged_t>.Create(OnFavoritesListChanged);
		m_LobbyInvite = Callback<LobbyInvite_t>.Create(OnLobbyInvite);
		m_LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
		m_LobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
		m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
		m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
		m_LobbyGameCreated = Callback<LobbyGameCreated_t>.Create(OnLobbyGameCreated);
		m_LobbyKicked = Callback<LobbyKicked_t>.Create(OnLobbyKicked);
		m_FavoritesListAccountsUpdated = Callback<FavoritesListAccountsUpdated_t>.Create(OnFavoritesListAccountsUpdated);
		m_SearchForGameProgressCallback = Callback<SearchForGameProgressCallback_t>.Create(OnSearchForGameProgressCallback);
		m_SearchForGameResultCallback = Callback<SearchForGameResultCallback_t>.Create(OnSearchForGameResultCallback);
		m_RequestPlayersForGameProgressCallback = Callback<RequestPlayersForGameProgressCallback_t>.Create(OnRequestPlayersForGameProgressCallback);
		m_RequestPlayersForGameResultCallback = Callback<RequestPlayersForGameResultCallback_t>.Create(OnRequestPlayersForGameResultCallback);
		m_RequestPlayersForGameFinalResultCallback = Callback<RequestPlayersForGameFinalResultCallback_t>.Create(OnRequestPlayersForGameFinalResultCallback);
		m_SubmitPlayerResultResultCallback = Callback<SubmitPlayerResultResultCallback_t>.Create(OnSubmitPlayerResultResultCallback);
		m_EndGameResultCallback = Callback<EndGameResultCallback_t>.Create(OnEndGameResultCallback);
		m_JoinPartyCallback = Callback<JoinPartyCallback_t>.Create(OnJoinPartyCallback);
		m_CreateBeaconCallback = Callback<CreateBeaconCallback_t>.Create(OnCreateBeaconCallback);
		m_ReservationNotificationCallback = Callback<ReservationNotificationCallback_t>.Create(OnReservationNotificationCallback);
		m_ChangeNumOpenSlotsCallback = Callback<ChangeNumOpenSlotsCallback_t>.Create(OnChangeNumOpenSlotsCallback);
		m_AvailableBeaconLocationsUpdated = Callback<AvailableBeaconLocationsUpdated_t>.Create(OnAvailableBeaconLocationsUpdated);
		m_ActiveBeaconsUpdated = Callback<ActiveBeaconsUpdated_t>.Create(OnActiveBeaconsUpdated);
		OnLobbyEnterCallResult = CallResult<LobbyEnter_t>.Create(OnLobbyEnter);
		OnLobbyMatchListCallResult = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
		OnLobbyCreatedCallResult = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		CSteamID lobby = m_Lobby;
		GUILayout.Label("m_Lobby: " + lobby.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("GetFavoriteGameCount() : " + SteamMatchmaking.GetFavoriteGameCount());
		AppId_t pnAppID;
		uint pnIP;
		ushort pnConnPort;
		ushort pnQueryPort;
		uint punFlags;
		uint pRTime32LastPlayedOnServer;
		bool favoriteGame = SteamMatchmaking.GetFavoriteGame(0, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
		string[] obj = new string[14]
		{
			"GetFavoriteGame(0, out AppID, out IP, out ConnPort, out QueryPort, out Flags, out LastPlayedOnServer) : ",
			favoriteGame.ToString(),
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
		AppId_t appId_t = pnAppID;
		obj[3] = appId_t.ToString();
		obj[4] = " -- ";
		obj[5] = pnIP.ToString();
		obj[6] = " -- ";
		obj[7] = pnConnPort.ToString();
		obj[8] = " -- ";
		obj[9] = pnQueryPort.ToString();
		obj[10] = " -- ";
		obj[11] = punFlags.ToString();
		obj[12] = " -- ";
		obj[13] = pRTime32LastPlayedOnServer.ToString();
		GUILayout.Label(string.Concat(obj));
		if (GUILayout.Button("AddFavoriteGame(TestConstants.Instance.k_AppId_TeamFortress2, TestConstants.k_IpAddress208_78_165_233_uint, TestConstants.k_Port27015, TestConstants.k_Port27015, Constants.k_unFavoriteFlagFavorite, CurrentUnixTime)"))
		{
			uint rTime32LastPlayedOnServer = (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			int num = SteamMatchmaking.AddFavoriteGame(TestConstants.Instance.k_AppId_TeamFortress2, 3494815209u, 27015, 27015, 1u, rTime32LastPlayedOnServer);
			string[] obj2 = new string[14]
			{
				"SteamMatchmaking.AddFavoriteGame(", null, null, null, null, null, null, null, null, null,
				null, null, null, null
			};
			appId_t = TestConstants.Instance.k_AppId_TeamFortress2;
			obj2[1] = appId_t.ToString();
			obj2[2] = ", ";
			obj2[3] = 3494815209u.ToString();
			obj2[4] = ", ";
			obj2[5] = ((ushort)27015).ToString();
			obj2[6] = ", ";
			obj2[7] = ((ushort)27015).ToString();
			obj2[8] = ", ";
			obj2[9] = 1.ToString();
			obj2[10] = ", ";
			obj2[11] = rTime32LastPlayedOnServer.ToString();
			obj2[12] = ") : ";
			obj2[13] = num.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("RemoveFavoriteGame(TestConstants.Instance.k_AppId_TeamFortress2, TestConstants.k_IpAddress208_78_165_233_uint, TestConstants.k_Port27015, TestConstants.k_Port27015, Constants.k_unFavoriteFlagFavorite)"))
		{
			bool flag = SteamMatchmaking.RemoveFavoriteGame(TestConstants.Instance.k_AppId_TeamFortress2, 3494815209u, 27015, 27015, 1u);
			string[] obj3 = new string[12]
			{
				"SteamMatchmaking.RemoveFavoriteGame(", null, null, null, null, null, null, null, null, null,
				null, null
			};
			appId_t = TestConstants.Instance.k_AppId_TeamFortress2;
			obj3[1] = appId_t.ToString();
			obj3[2] = ", ";
			obj3[3] = 3494815209u.ToString();
			obj3[4] = ", ";
			obj3[5] = ((ushort)27015).ToString();
			obj3[6] = ", ";
			obj3[7] = ((ushort)27015).ToString();
			obj3[8] = ", ";
			obj3[9] = 1.ToString();
			obj3[10] = ") : ";
			obj3[11] = flag.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("RequestLobbyList()"))
		{
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.RequestLobbyList();
			OnLobbyMatchListCallResult.Set(steamAPICall_t);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamMatchmaking.RequestLobbyList() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("AddRequestLobbyListStringFilter(\"SomeStringKey\", \"SomeValue\", ELobbyComparison.k_ELobbyComparisonNotEqual)"))
		{
			SteamMatchmaking.AddRequestLobbyListStringFilter("SomeStringKey", "SomeValue", ELobbyComparison.k_ELobbyComparisonNotEqual);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListStringFilter(\"SomeStringKey\", \"SomeValue\", " + ELobbyComparison.k_ELobbyComparisonNotEqual.ToString() + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListNumericalFilter(\"SomeIntKey\", 0, ELobbyComparison.k_ELobbyComparisonNotEqual)"))
		{
			SteamMatchmaking.AddRequestLobbyListNumericalFilter("SomeIntKey", 0, ELobbyComparison.k_ELobbyComparisonNotEqual);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListNumericalFilter(\"SomeIntKey\", " + 0 + ", " + ELobbyComparison.k_ELobbyComparisonNotEqual.ToString() + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListNearValueFilter(\"SomeIntKey\", 0)"))
		{
			SteamMatchmaking.AddRequestLobbyListNearValueFilter("SomeIntKey", 0);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListNearValueFilter(\"SomeIntKey\", " + 0 + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListFilterSlotsAvailable(3)"))
		{
			SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(3);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(" + 3 + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide)"))
		{
			SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListDistanceFilter(" + ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide.ToString() + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListResultCountFilter(1)"))
		{
			SteamMatchmaking.AddRequestLobbyListResultCountFilter(1);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListResultCountFilter(" + 1 + ")");
		}
		if (GUILayout.Button("AddRequestLobbyListCompatibleMembersFilter((CSteamID)0)"))
		{
			SteamMatchmaking.AddRequestLobbyListCompatibleMembersFilter((CSteamID)0uL);
			MonoBehaviour.print("SteamMatchmaking.AddRequestLobbyListCompatibleMembersFilter(" + ((CSteamID)0uL).ToString() + ")");
		}
		if (GUILayout.Button("GetLobbyByIndex(0)"))
		{
			m_Lobby = SteamMatchmaking.GetLobbyByIndex(0);
			string text = 0.ToString();
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.GetLobbyByIndex(" + text + ") : " + lobby.ToString());
		}
		if (GUILayout.Button("CreateLobby(ELobbyType.k_ELobbyTypePublic, 1)"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 1);
			OnLobbyCreatedCallResult.Set(steamAPICall_t3);
			string[] obj4 = new string[6]
			{
				"SteamMatchmaking.CreateLobby(",
				ELobbyType.k_ELobbyTypePublic.ToString(),
				", ",
				1.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			obj4[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("JoinLobby(m_Lobby)"))
		{
			SteamAPICall_t steamAPICall_t4 = SteamMatchmaking.JoinLobby(m_Lobby);
			OnLobbyEnterCallResult.Set(steamAPICall_t4);
			lobby = m_Lobby;
			string text2 = lobby.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t4;
			MonoBehaviour.print("SteamMatchmaking.JoinLobby(" + text2 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("LeaveLobby(m_Lobby)"))
		{
			SteamMatchmaking.LeaveLobby(m_Lobby);
			m_Lobby = CSteamID.Nil;
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.LeaveLobby(" + lobby.ToString() + ")");
		}
		if (GUILayout.Button("InviteUserToLobby(m_Lobby, SteamUser.GetSteamID())"))
		{
			bool flag2 = SteamMatchmaking.InviteUserToLobby(m_Lobby, SteamUser.GetSteamID());
			string[] obj5 = new string[6] { "SteamMatchmaking.InviteUserToLobby(", null, null, null, null, null };
			lobby = m_Lobby;
			obj5[1] = lobby.ToString();
			obj5[2] = ", ";
			obj5[3] = SteamUser.GetSteamID().ToString();
			obj5[4] = ") : ";
			obj5[5] = flag2.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		GUILayout.Label("GetNumLobbyMembers(m_Lobby) : " + SteamMatchmaking.GetNumLobbyMembers(m_Lobby));
		GUILayout.Label("GetLobbyMemberByIndex(m_Lobby, 0) : " + SteamMatchmaking.GetLobbyMemberByIndex(m_Lobby, 0).ToString());
		GUILayout.Label("GetLobbyData(m_Lobby, \"name\") : " + SteamMatchmaking.GetLobbyData(m_Lobby, "name"));
		if (GUILayout.Button("SetLobbyData(m_Lobby, \"name\", \"Test Lobby!\")"))
		{
			bool flag3 = SteamMatchmaking.SetLobbyData(m_Lobby, "name", "Test Lobby!");
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.SetLobbyData(" + lobby.ToString() + ", \"name\", \"Test Lobby!\") : " + flag3);
		}
		GUILayout.Label("GetLobbyDataCount(m_Lobby) : " + SteamMatchmaking.GetLobbyDataCount(m_Lobby));
		string pchKey;
		string pchValue;
		bool lobbyDataByIndex = SteamMatchmaking.GetLobbyDataByIndex(m_Lobby, 0, out pchKey, 255, out pchValue, 255);
		GUILayout.Label("GetLobbyDataByIndex(m_Lobby, 0, out Key, 255, out Value, 255) : " + lobbyDataByIndex + " -- " + pchKey + " -- " + pchValue);
		if (GUILayout.Button("DeleteLobbyData(m_Lobby, \"name\")"))
		{
			bool flag4 = SteamMatchmaking.DeleteLobbyData(m_Lobby, "name");
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.DeleteLobbyData(" + lobby.ToString() + ", \"name\") : " + flag4);
		}
		GUILayout.Label("GetLobbyMemberData(m_Lobby, SteamUser.GetSteamID(), \"test\") : " + SteamMatchmaking.GetLobbyMemberData(m_Lobby, SteamUser.GetSteamID(), "test"));
		if (GUILayout.Button("SetLobbyMemberData(m_Lobby, \"test\", \"This is a test Key!\")"))
		{
			SteamMatchmaking.SetLobbyMemberData(m_Lobby, "test", "This is a test Key!");
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.SetLobbyMemberData(" + lobby.ToString() + ", \"test\", \"This is a test Key!\")");
		}
		if (GUILayout.Button("SendLobbyChatMsg(m_Lobby, MsgBody, MsgBody.Length)"))
		{
			byte[] bytes = Encoding.UTF8.GetBytes("Test Message!");
			bool flag5 = SteamMatchmaking.SendLobbyChatMsg(m_Lobby, bytes, bytes.Length);
			string[] obj6 = new string[8] { "SteamMatchmaking.SendLobbyChatMsg(", null, null, null, null, null, null, null };
			lobby = m_Lobby;
			obj6[1] = lobby.ToString();
			obj6[2] = ", ";
			obj6[3] = bytes?.ToString();
			obj6[4] = ", ";
			obj6[5] = bytes.Length.ToString();
			obj6[6] = ") : ";
			obj6[7] = flag5.ToString();
			MonoBehaviour.print(string.Concat(obj6));
		}
		if (GUILayout.Button("RequestLobbyData(m_Lobby)"))
		{
			bool flag6 = SteamMatchmaking.RequestLobbyData(m_Lobby);
			lobby = m_Lobby;
			MonoBehaviour.print("SteamMatchmaking.RequestLobbyData(" + lobby.ToString() + ") : " + flag6);
		}
		if (GUILayout.Button("SetLobbyGameServer(m_Lobby, TestConstants.k_IpAddress127_0_0_1_uint, TestConstants.k_Port27015, CSteamID.NonSteamGS)"))
		{
			SteamMatchmaking.SetLobbyGameServer(m_Lobby, 2130706433u, 27015, CSteamID.NonSteamGS);
			string[] obj7 = new string[9] { "SteamMatchmaking.SetLobbyGameServer(", null, null, null, null, null, null, null, null };
			lobby = m_Lobby;
			obj7[1] = lobby.ToString();
			obj7[2] = ", ";
			obj7[3] = 2130706433u.ToString();
			obj7[4] = ", ";
			obj7[5] = ((ushort)27015).ToString();
			obj7[6] = ", ";
			lobby = CSteamID.NonSteamGS;
			obj7[7] = lobby.ToString();
			obj7[8] = ")";
			MonoBehaviour.print(string.Concat(obj7));
		}
		uint punGameServerIP;
		ushort punGameServerPort;
		CSteamID psteamIDGameServer;
		bool lobbyGameServer = SteamMatchmaking.GetLobbyGameServer(m_Lobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
		string[] obj8 = new string[8]
		{
			"GetLobbyGameServer(m_Lobby, out GameServerIP, out GameServerPort, out SteamIDGameServer) : ",
			lobbyGameServer.ToString(),
			" -- ",
			punGameServerIP.ToString(),
			" -- ",
			punGameServerPort.ToString(),
			" -- ",
			null
		};
		lobby = psteamIDGameServer;
		obj8[7] = lobby.ToString();
		GUILayout.Label(string.Concat(obj8));
		if (GUILayout.Button("SetLobbyMemberLimit(m_Lobby, 6)"))
		{
			bool flag7 = SteamMatchmaking.SetLobbyMemberLimit(m_Lobby, 6);
			string[] obj9 = new string[6] { "SteamMatchmaking.SetLobbyMemberLimit(", null, null, null, null, null };
			lobby = m_Lobby;
			obj9[1] = lobby.ToString();
			obj9[2] = ", ";
			obj9[3] = 6.ToString();
			obj9[4] = ") : ";
			obj9[5] = flag7.ToString();
			MonoBehaviour.print(string.Concat(obj9));
		}
		GUILayout.Label("GetLobbyMemberLimit(m_Lobby) : " + SteamMatchmaking.GetLobbyMemberLimit(m_Lobby));
		if (GUILayout.Button("SetLobbyType(m_Lobby, ELobbyType.k_ELobbyTypePublic)"))
		{
			bool flag8 = SteamMatchmaking.SetLobbyType(m_Lobby, ELobbyType.k_ELobbyTypePublic);
			string[] obj10 = new string[6] { "SteamMatchmaking.SetLobbyType(", null, null, null, null, null };
			lobby = m_Lobby;
			obj10[1] = lobby.ToString();
			obj10[2] = ", ";
			obj10[3] = ELobbyType.k_ELobbyTypePublic.ToString();
			obj10[4] = ") : ";
			obj10[5] = flag8.ToString();
			MonoBehaviour.print(string.Concat(obj10));
		}
		if (GUILayout.Button("SetLobbyJoinable(m_Lobby, true)"))
		{
			bool flag9 = SteamMatchmaking.SetLobbyJoinable(m_Lobby, bLobbyJoinable: true);
			string[] obj11 = new string[6] { "SteamMatchmaking.SetLobbyJoinable(", null, null, null, null, null };
			lobby = m_Lobby;
			obj11[1] = lobby.ToString();
			obj11[2] = ", ";
			obj11[3] = true.ToString();
			obj11[4] = ") : ";
			obj11[5] = flag9.ToString();
			MonoBehaviour.print(string.Concat(obj11));
		}
		if (GUILayout.Button("GetLobbyOwner(m_Lobby)"))
		{
			CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(m_Lobby);
			lobby = m_Lobby;
			string text3 = lobby.ToString();
			lobby = lobbyOwner;
			MonoBehaviour.print("SteamMatchmaking.GetLobbyOwner(" + text3 + ") : " + lobby.ToString());
		}
		if (GUILayout.Button("SetLobbyOwner(m_Lobby, SteamUser.GetSteamID())"))
		{
			bool flag10 = SteamMatchmaking.SetLobbyOwner(m_Lobby, SteamUser.GetSteamID());
			string[] obj12 = new string[6] { "SteamMatchmaking.SetLobbyOwner(", null, null, null, null, null };
			lobby = m_Lobby;
			obj12[1] = lobby.ToString();
			obj12[2] = ", ";
			obj12[3] = SteamUser.GetSteamID().ToString();
			obj12[4] = ") : ";
			obj12[5] = flag10.ToString();
			MonoBehaviour.print(string.Concat(obj12));
		}
		if (GUILayout.Button("SetLinkedLobby(m_Lobby, m_Lobby)"))
		{
			bool flag11 = SteamMatchmaking.SetLinkedLobby(m_Lobby, m_Lobby);
			string[] obj13 = new string[6] { "SteamMatchmaking.SetLinkedLobby(", null, null, null, null, null };
			lobby = m_Lobby;
			obj13[1] = lobby.ToString();
			obj13[2] = ", ";
			lobby = m_Lobby;
			obj13[3] = lobby.ToString();
			obj13[4] = ") : ";
			obj13[5] = flag11.ToString();
			MonoBehaviour.print(string.Concat(obj13));
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnFavoritesListChanged(FavoritesListChanged_t pCallback)
	{
		string[] obj = new string[16]
		{
			"[",
			502.ToString(),
			" - FavoritesListChanged] - ",
			pCallback.m_nIP.ToString(),
			" -- ",
			pCallback.m_nQueryPort.ToString(),
			" -- ",
			pCallback.m_nConnPort.ToString(),
			" -- ",
			pCallback.m_nAppID.ToString(),
			" -- ",
			pCallback.m_nFlags.ToString(),
			" -- ",
			pCallback.m_bAdd.ToString(),
			" -- ",
			null
		};
		AccountID_t unAccountId = pCallback.m_unAccountId;
		obj[15] = unAccountId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnLobbyInvite(LobbyInvite_t pCallback)
	{
		Debug.Log("[" + 503 + " - LobbyInvite] - " + pCallback.m_ulSteamIDUser + " -- " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulGameID);
	}

	private void OnLobbyEnter(LobbyEnter_t pCallback)
	{
		Debug.Log("[" + 504 + " - LobbyEnter] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_rgfChatPermissions + " -- " + pCallback.m_bLocked + " -- " + pCallback.m_EChatRoomEnterResponse);
		m_Lobby = (CSteamID)pCallback.m_ulSteamIDLobby;
	}

	private void OnLobbyEnter(LobbyEnter_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 504 + " - LobbyEnter] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_rgfChatPermissions + " -- " + pCallback.m_bLocked + " -- " + pCallback.m_EChatRoomEnterResponse);
		m_Lobby = (CSteamID)pCallback.m_ulSteamIDLobby;
	}

	private void OnLobbyDataUpdate(LobbyDataUpdate_t pCallback)
	{
		Debug.Log("[" + 505 + " - LobbyDataUpdate] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDMember + " -- " + pCallback.m_bSuccess);
	}

	private void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
	{
		Debug.Log("[" + 506 + " - LobbyChatUpdate] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDUserChanged + " -- " + pCallback.m_ulSteamIDMakingChange + " -- " + pCallback.m_rgfChatMemberStateChange);
	}

	private void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
	{
		Debug.Log("[" + 507 + " - LobbyChatMsg] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDUser + " -- " + pCallback.m_eChatEntryType + " -- " + pCallback.m_iChatID);
		byte[] array = new byte[4096];
		CSteamID pSteamIDUser;
		EChatEntryType peChatEntryType;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry((CSteamID)pCallback.m_ulSteamIDLobby, (int)pCallback.m_iChatID, out pSteamIDUser, array, array.Length, out peChatEntryType);
		string[] obj = new string[12]
		{
			"GetLobbyChatEntry(",
			((CSteamID)pCallback.m_ulSteamIDLobby).ToString(),
			", ",
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
		int iChatID = (int)pCallback.m_iChatID;
		obj[3] = iChatID.ToString();
		obj[4] = ", out SteamIDUser, Data, Data.Length, out ChatEntryType) : ";
		obj[5] = lobbyChatEntry.ToString();
		obj[6] = " -- ";
		CSteamID cSteamID = pSteamIDUser;
		obj[7] = cSteamID.ToString();
		obj[8] = " -- ";
		obj[9] = Encoding.UTF8.GetString(array);
		obj[10] = " -- ";
		obj[11] = peChatEntryType.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnLobbyGameCreated(LobbyGameCreated_t pCallback)
	{
		Debug.Log("[" + 509 + " - LobbyGameCreated] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDGameServer + " -- " + pCallback.m_unIP + " -- " + pCallback.m_usPort);
	}

	private void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 510 + " - LobbyMatchList] - " + pCallback.m_nLobbiesMatching);
	}

	private void OnLobbyKicked(LobbyKicked_t pCallback)
	{
		Debug.Log("[" + 512 + " - LobbyKicked] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDAdmin + " -- " + pCallback.m_bKickedDueToDisconnect);
	}

	private void OnLobbyCreated(LobbyCreated_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 513 + " - LobbyCreated] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_ulSteamIDLobby);
		m_Lobby = (CSteamID)pCallback.m_ulSteamIDLobby;
	}

	private void OnFavoritesListAccountsUpdated(FavoritesListAccountsUpdated_t pCallback)
	{
		Debug.Log("[" + 516 + " - FavoritesListAccountsUpdated] - " + pCallback.m_eResult);
	}

	private void OnSearchForGameProgressCallback(SearchForGameProgressCallback_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			5201.ToString(),
			" - SearchForGameProgressCallback] - ",
			pCallback.m_ullSearchID.ToString(),
			" -- ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		CSteamID lobbyID = pCallback.m_lobbyID;
		obj[7] = lobbyID.ToString();
		obj[8] = " -- ";
		lobbyID = pCallback.m_steamIDEndedSearch;
		obj[9] = lobbyID.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nSecondsRemainingEstimate.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_cPlayersSearching.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnSearchForGameResultCallback(SearchForGameResultCallback_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			5202.ToString(),
			" - SearchForGameResultCallback] - ",
			pCallback.m_ullSearchID.ToString(),
			" -- ",
			pCallback.m_eResult.ToString(),
			" -- ",
			pCallback.m_nCountPlayersInGame.ToString(),
			" -- ",
			pCallback.m_nCountAcceptedGame.ToString(),
			" -- ",
			null,
			null,
			null
		};
		CSteamID steamIDHost = pCallback.m_steamIDHost;
		obj[11] = steamIDHost.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_bFinalCallback.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRequestPlayersForGameProgressCallback(RequestPlayersForGameProgressCallback_t pCallback)
	{
		Debug.Log("[" + 5211 + " - RequestPlayersForGameProgressCallback] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_ullSearchID);
	}

	private void OnRequestPlayersForGameResultCallback(RequestPlayersForGameResultCallback_t pCallback)
	{
		string[] obj = new string[22]
		{
			"[",
			5212.ToString(),
			" - RequestPlayersForGameResultCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			pCallback.m_ullSearchID.ToString(),
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
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamIDPlayerFound = pCallback.m_SteamIDPlayerFound;
		obj[7] = steamIDPlayerFound.ToString();
		obj[8] = " -- ";
		steamIDPlayerFound = pCallback.m_SteamIDLobby;
		obj[9] = steamIDPlayerFound.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_ePlayerAcceptState.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_nPlayerIndex.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_nTotalPlayersFound.ToString();
		obj[16] = " -- ";
		obj[17] = pCallback.m_nTotalPlayersAcceptedGame.ToString();
		obj[18] = " -- ";
		obj[19] = pCallback.m_nSuggestedTeamIndex.ToString();
		obj[20] = " -- ";
		obj[21] = pCallback.m_ullUniqueGameID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRequestPlayersForGameFinalResultCallback(RequestPlayersForGameFinalResultCallback_t pCallback)
	{
		Debug.Log("[" + 5213 + " - RequestPlayersForGameFinalResultCallback] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_ullSearchID + " -- " + pCallback.m_ullUniqueGameID);
	}

	private void OnSubmitPlayerResultResultCallback(SubmitPlayerResultResultCallback_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			5214.ToString(),
			" - SubmitPlayerResultResultCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			pCallback.ullUniqueGameID.ToString(),
			" -- ",
			null
		};
		CSteamID steamIDPlayer = pCallback.steamIDPlayer;
		obj[7] = steamIDPlayer.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnEndGameResultCallback(EndGameResultCallback_t pCallback)
	{
		Debug.Log("[" + 5215 + " - EndGameResultCallback] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.ullUniqueGameID);
	}

	private void OnJoinPartyCallback(JoinPartyCallback_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			5301.ToString(),
			" - JoinPartyCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[5] = ulBeaconID.ToString();
		obj[6] = " -- ";
		CSteamID steamIDBeaconOwner = pCallback.m_SteamIDBeaconOwner;
		obj[7] = steamIDBeaconOwner.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_rgchConnectString;
		Debug.Log(string.Concat(obj));
	}

	private void OnCreateBeaconCallback(CreateBeaconCallback_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			5302.ToString(),
			" - CreateBeaconCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[5] = ulBeaconID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnReservationNotificationCallback(ReservationNotificationCallback_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			5303.ToString(),
			" - ReservationNotificationCallback] - ",
			null,
			null,
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[3] = ulBeaconID.ToString();
		obj[4] = " -- ";
		CSteamID steamIDJoiner = pCallback.m_steamIDJoiner;
		obj[5] = steamIDJoiner.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnChangeNumOpenSlotsCallback(ChangeNumOpenSlotsCallback_t pCallback)
	{
		Debug.Log("[" + 5304 + " - ChangeNumOpenSlotsCallback] - " + pCallback.m_eResult);
	}

	private void OnAvailableBeaconLocationsUpdated(AvailableBeaconLocationsUpdated_t pCallback)
	{
		Debug.Log("[" + 5305 + " - AvailableBeaconLocationsUpdated]");
	}

	private void OnActiveBeaconsUpdated(ActiveBeaconsUpdated_t pCallback)
	{
		Debug.Log("[" + 5306 + " - ActiveBeaconsUpdated]");
	}
}
