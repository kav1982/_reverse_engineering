using Steamworks;
using UnityEngine;

public class SteamMatchmakingServersTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private HServerListRequest m_ServerListRequest;

	private HServerQuery m_ServerQuery;

	private ISteamMatchmakingServerListResponse m_ServerListResponse;

	private ISteamMatchmakingPingResponse m_PingResponse;

	private ISteamMatchmakingPlayersResponse m_PlayersResponse;

	private ISteamMatchmakingRulesResponse m_RulesResponse;

	public void OnEnable()
	{
		m_ServerListRequest = HServerListRequest.Invalid;
		m_ServerQuery = HServerQuery.Invalid;
		m_ServerListResponse = new ISteamMatchmakingServerListResponse(OnServerResponded, OnServerFailedToRespond, OnRefreshComplete);
		m_PingResponse = new ISteamMatchmakingPingResponse(OnServerResponded, OnServerFailedToRespond);
		m_PlayersResponse = new ISteamMatchmakingPlayersResponse(OnAddPlayerToList, OnPlayersFailedToRespond, OnPlayersRefreshComplete);
		m_RulesResponse = new ISteamMatchmakingRulesResponse(OnRulesResponded, OnRulesFailedToRespond, OnRulesRefreshComplete);
	}

	private void OnDisable()
	{
		ReleaseRequest();
		CancelServerQuery();
	}

	private void ReleaseRequest()
	{
		if (m_ServerListRequest != HServerListRequest.Invalid)
		{
			SteamMatchmakingServers.ReleaseRequest(m_ServerListRequest);
			m_ServerListRequest = HServerListRequest.Invalid;
			MonoBehaviour.print("SteamMatchmakingServers.ReleaseRequest(m_ServerListRequest)");
		}
	}

	private void CancelServerQuery()
	{
		if (m_ServerQuery != HServerQuery.Invalid)
		{
			SteamMatchmakingServers.CancelServerQuery(m_ServerQuery);
			m_ServerQuery = HServerQuery.Invalid;
			MonoBehaviour.print("SteamMatchmakingServers.CancelServerQuery(m_ServerQuery)");
		}
	}

	private string GameServerItemFormattedString(gameserveritem_t gsi)
	{
		string[] obj = new string[37]
		{
			"m_NetAdr: ",
			gsi.m_NetAdr.GetConnectionAddressString(),
			"\nm_nPing: ",
			gsi.m_nPing.ToString(),
			"\nm_bHadSuccessfulResponse: ",
			gsi.m_bHadSuccessfulResponse.ToString(),
			"\nm_bDoNotRefresh: ",
			gsi.m_bDoNotRefresh.ToString(),
			"\nm_szGameDir: ",
			gsi.GetGameDir(),
			"\nm_szMap: ",
			gsi.GetMap(),
			"\nm_szGameDescription: ",
			gsi.GetGameDescription(),
			"\nm_nAppID: ",
			gsi.m_nAppID.ToString(),
			"\nm_nPlayers: ",
			gsi.m_nPlayers.ToString(),
			"\nm_nMaxPlayers: ",
			gsi.m_nMaxPlayers.ToString(),
			"\nm_nBotPlayers: ",
			gsi.m_nBotPlayers.ToString(),
			"\nm_bPassword: ",
			gsi.m_bPassword.ToString(),
			"\nm_bSecure: ",
			gsi.m_bSecure.ToString(),
			"\nm_ulTimeLastPlayed: ",
			gsi.m_ulTimeLastPlayed.ToString(),
			"\nm_nServerVersion: ",
			gsi.m_nServerVersion.ToString(),
			"\nm_szServerName: ",
			gsi.GetServerName(),
			"\nm_szGameTags: ",
			gsi.GetGameTags(),
			"\nm_steamID: ",
			null,
			null
		};
		CSteamID steamID = gsi.m_steamID;
		obj[35] = steamID.ToString();
		obj[36] = "\n";
		return string.Concat(obj);
	}

	private void OnServerResponded(HServerListRequest hRequest, int iServer)
	{
		HServerListRequest hServerListRequest = hRequest;
		Debug.Log("OnServerResponded: " + hServerListRequest.ToString() + " - " + iServer);
	}

	private void OnServerFailedToRespond(HServerListRequest hRequest, int iServer)
	{
		HServerListRequest hServerListRequest = hRequest;
		Debug.Log("OnServerFailedToRespond: " + hServerListRequest.ToString() + " - " + iServer);
	}

	private void OnRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
	{
		HServerListRequest hServerListRequest = hRequest;
		Debug.Log("OnRefreshComplete: " + hServerListRequest.ToString() + " - " + response);
	}

	private void OnServerResponded(gameserveritem_t gsi)
	{
		Debug.Log("OnServerResponded: " + gsi?.ToString() + "\n" + GameServerItemFormattedString(gsi));
	}

	private void OnServerFailedToRespond()
	{
		Debug.Log("OnServerFailedToRespond");
	}

	private void OnAddPlayerToList(string pchName, int nScore, float flTimePlayed)
	{
		Debug.Log("OnAddPlayerToList: " + pchName + " - " + nScore + " - " + flTimePlayed);
	}

	private void OnPlayersFailedToRespond()
	{
		Debug.Log("OnPlayersFailedToRespond");
	}

	private void OnPlayersRefreshComplete()
	{
		Debug.Log("OnPlayersRefreshComplete");
	}

	private void OnRulesResponded(string pchRule, string pchValue)
	{
		Debug.Log("OnRulesResponded: " + pchRule + " - " + pchValue);
	}

	private void OnRulesFailedToRespond()
	{
		Debug.Log("OnRulesFailedToRespond");
	}

	private void OnRulesRefreshComplete()
	{
		Debug.Log("OnRulesRefreshComplete");
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		HServerListRequest serverListRequest = m_ServerListRequest;
		GUILayout.Label("m_ServerListRequest: " + serverListRequest.ToString());
		HServerQuery serverQuery = m_ServerQuery;
		GUILayout.Label("m_ServerQuery: " + serverQuery.ToString());
		GUILayout.Label("m_ServerListResponse: " + m_ServerListResponse);
		GUILayout.Label("m_PingResponse: " + m_PingResponse);
		GUILayout.Label("m_PlayersResponse: " + m_PlayersResponse);
		GUILayout.Label("m_RulesResponse: " + m_RulesResponse);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("RequestInternetServerList(TestConstants.Instance.k_AppId_TeamFortress2, filters, (uint)filters.Length, m_ServerListResponse)"))
		{
			ReleaseRequest();
			MatchMakingKeyValuePair_t[] array = new MatchMakingKeyValuePair_t[3];
			MatchMakingKeyValuePair_t matchMakingKeyValuePair_t = new MatchMakingKeyValuePair_t
			{
				m_szKey = "appid"
			};
			AppId_t k_AppId_TeamFortress = TestConstants.Instance.k_AppId_TeamFortress2;
			matchMakingKeyValuePair_t.m_szValue = k_AppId_TeamFortress.ToString();
			array[0] = matchMakingKeyValuePair_t;
			array[1] = new MatchMakingKeyValuePair_t
			{
				m_szKey = "gamedir",
				m_szValue = "tf"
			};
			array[2] = new MatchMakingKeyValuePair_t
			{
				m_szKey = "gametagsand",
				m_szValue = "beta"
			};
			MatchMakingKeyValuePair_t[] array2 = array;
			m_ServerListRequest = SteamMatchmakingServers.RequestInternetServerList(TestConstants.Instance.k_AppId_TeamFortress2, array2, (uint)array2.Length, m_ServerListResponse);
			string[] obj = new string[10] { "SteamMatchmakingServers.RequestInternetServerList(", null, null, null, null, null, null, null, null, null };
			k_AppId_TeamFortress = TestConstants.Instance.k_AppId_TeamFortress2;
			obj[1] = k_AppId_TeamFortress.ToString();
			obj[2] = ", ";
			obj[3] = array2?.ToString();
			obj[4] = ", ";
			obj[5] = ((uint)array2.Length).ToString();
			obj[6] = ", ";
			obj[7] = m_ServerListResponse?.ToString();
			obj[8] = ") : ";
			serverListRequest = m_ServerListRequest;
			obj[9] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("RequestLANServerList(new AppId_t(440), m_ServerListResponse)"))
		{
			ReleaseRequest();
			m_ServerListRequest = SteamMatchmakingServers.RequestLANServerList(new AppId_t(440u), m_ServerListResponse);
			string[] obj2 = new string[6]
			{
				"SteamMatchmakingServers.RequestLANServerList(",
				new AppId_t(440u).ToString(),
				", ",
				m_ServerListResponse?.ToString(),
				") : ",
				null
			};
			serverListRequest = m_ServerListRequest;
			obj2[5] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("RequestFriendsServerList(new AppId_t(440), null, 0, m_ServerListResponse)"))
		{
			ReleaseRequest();
			m_ServerListRequest = SteamMatchmakingServers.RequestFriendsServerList(new AppId_t(440u), null, 0u, m_ServerListResponse);
			string[] obj3 = new string[8]
			{
				"SteamMatchmakingServers.RequestFriendsServerList(",
				new AppId_t(440u).ToString(),
				", , ",
				0.ToString(),
				", ",
				m_ServerListResponse?.ToString(),
				") : ",
				null
			};
			serverListRequest = m_ServerListRequest;
			obj3[7] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("RequestFavoritesServerList(new AppId_t(440), null, 0, m_ServerListResponse)"))
		{
			ReleaseRequest();
			m_ServerListRequest = SteamMatchmakingServers.RequestFavoritesServerList(new AppId_t(440u), null, 0u, m_ServerListResponse);
			string[] obj4 = new string[8]
			{
				"SteamMatchmakingServers.RequestFavoritesServerList(",
				new AppId_t(440u).ToString(),
				", , ",
				0.ToString(),
				", ",
				m_ServerListResponse?.ToString(),
				") : ",
				null
			};
			serverListRequest = m_ServerListRequest;
			obj4[7] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("RequestHistoryServerList(new AppId_t(440), null, 0, m_ServerListResponse)"))
		{
			ReleaseRequest();
			m_ServerListRequest = SteamMatchmakingServers.RequestHistoryServerList(new AppId_t(440u), null, 0u, m_ServerListResponse);
			string[] obj5 = new string[8]
			{
				"SteamMatchmakingServers.RequestHistoryServerList(",
				new AppId_t(440u).ToString(),
				", , ",
				0.ToString(),
				", ",
				m_ServerListResponse?.ToString(),
				") : ",
				null
			};
			serverListRequest = m_ServerListRequest;
			obj5[7] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("RequestSpectatorServerList(new AppId_t(440), null, 0, m_ServerListResponse)"))
		{
			ReleaseRequest();
			m_ServerListRequest = SteamMatchmakingServers.RequestSpectatorServerList(new AppId_t(440u), null, 0u, m_ServerListResponse);
			string[] obj6 = new string[8]
			{
				"SteamMatchmakingServers.RequestSpectatorServerList(",
				new AppId_t(440u).ToString(),
				", , ",
				0.ToString(),
				", ",
				m_ServerListResponse?.ToString(),
				") : ",
				null
			};
			serverListRequest = m_ServerListRequest;
			obj6[7] = serverListRequest.ToString();
			MonoBehaviour.print(string.Concat(obj6));
		}
		if (GUILayout.Button("ReleaseRequest(m_ServerListRequest)"))
		{
			ReleaseRequest();
		}
		if (GUILayout.Button("GetServerDetails(m_ServerListRequest, 0)"))
		{
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(m_ServerListRequest, 0);
			string[] obj7 = new string[6] { "SteamMatchmakingServers.GetServerDetails(", null, null, null, null, null };
			serverListRequest = m_ServerListRequest;
			obj7[1] = serverListRequest.ToString();
			obj7[2] = ", ";
			obj7[3] = 0.ToString();
			obj7[4] = ") : ";
			obj7[5] = serverDetails?.ToString();
			MonoBehaviour.print(string.Concat(obj7));
			MonoBehaviour.print(GameServerItemFormattedString(serverDetails));
		}
		if (GUILayout.Button("CancelQuery(m_ServerListRequest)"))
		{
			SteamMatchmakingServers.CancelQuery(m_ServerListRequest);
			serverListRequest = m_ServerListRequest;
			MonoBehaviour.print("SteamMatchmakingServers.CancelQuery(" + serverListRequest.ToString() + ")");
		}
		if (GUILayout.Button("RefreshQuery(m_ServerListRequest)"))
		{
			SteamMatchmakingServers.RefreshQuery(m_ServerListRequest);
			serverListRequest = m_ServerListRequest;
			MonoBehaviour.print("SteamMatchmakingServers.RefreshQuery(" + serverListRequest.ToString() + ")");
		}
		GUILayout.Label("IsRefreshing(m_ServerListRequest) : " + SteamMatchmakingServers.IsRefreshing(m_ServerListRequest));
		GUILayout.Label("GetServerCount(m_ServerListRequest) : " + SteamMatchmakingServers.GetServerCount(m_ServerListRequest));
		if (GUILayout.Button("RefreshServer(m_ServerListRequest, 0)"))
		{
			SteamMatchmakingServers.RefreshServer(m_ServerListRequest, 0);
			string[] obj8 = new string[5] { "SteamMatchmakingServers.RefreshServer(", null, null, null, null };
			serverListRequest = m_ServerListRequest;
			obj8[1] = serverListRequest.ToString();
			obj8[2] = ", ";
			obj8[3] = 0.ToString();
			obj8[4] = ")";
			MonoBehaviour.print(string.Concat(obj8));
		}
		if (GUILayout.Button("PingServer(TestConstants.k_IpAddress208_78_165_233_uint, TestConstants.k_Port27015, m_PingResponse)"))
		{
			CancelServerQuery();
			m_ServerQuery = SteamMatchmakingServers.PingServer(3494815209u, 27015, m_PingResponse);
			string[] obj9 = new string[8]
			{
				"SteamMatchmakingServers.PingServer(",
				3494815209u.ToString(),
				", ",
				((ushort)27015).ToString(),
				", ",
				m_PingResponse?.ToString(),
				") : ",
				null
			};
			serverQuery = m_ServerQuery;
			obj9[7] = serverQuery.ToString();
			MonoBehaviour.print(string.Concat(obj9));
		}
		if (GUILayout.Button("PlayerDetails(TestConstants.k_IpAddress208_78_165_233_uint, TestConstants.k_Port27015, m_PlayersResponse)"))
		{
			CancelServerQuery();
			m_ServerQuery = SteamMatchmakingServers.PlayerDetails(3494815209u, 27015, m_PlayersResponse);
			string[] obj10 = new string[8]
			{
				"SteamMatchmakingServers.PlayerDetails(",
				3494815209u.ToString(),
				", ",
				((ushort)27015).ToString(),
				", ",
				m_PlayersResponse?.ToString(),
				") : ",
				null
			};
			serverQuery = m_ServerQuery;
			obj10[7] = serverQuery.ToString();
			MonoBehaviour.print(string.Concat(obj10));
		}
		if (GUILayout.Button("ServerRules(TestConstants.k_IpAddress208_78_165_233_uint, TestConstants.k_Port27015, m_RulesResponse)"))
		{
			CancelServerQuery();
			m_ServerQuery = SteamMatchmakingServers.ServerRules(3494815209u, 27015, m_RulesResponse);
			string[] obj11 = new string[8]
			{
				"SteamMatchmakingServers.ServerRules(",
				3494815209u.ToString(),
				", ",
				((ushort)27015).ToString(),
				", ",
				m_RulesResponse?.ToString(),
				") : ",
				null
			};
			serverQuery = m_ServerQuery;
			obj11[7] = serverQuery.ToString();
			MonoBehaviour.print(string.Concat(obj11));
		}
		if (GUILayout.Button("CancelServerQuery(m_ServerQuery)"))
		{
			CancelServerQuery();
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}
}
