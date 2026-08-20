using System;
using Steamworks;
using UnityEngine;

public class SteamClientTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private HSteamPipe m_Pipe;

	private HSteamUser m_GlobalUser;

	private HSteamPipe m_LocalPipe;

	private HSteamUser m_LocalUser;

	public void OnEnable()
	{
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		HSteamPipe pipe = m_Pipe;
		GUILayout.Label("m_Pipe: " + pipe.ToString());
		HSteamUser globalUser = m_GlobalUser;
		GUILayout.Label("m_GlobalUser: " + globalUser.ToString());
		pipe = m_LocalPipe;
		GUILayout.Label("m_LocalPipe: " + pipe.ToString());
		globalUser = m_LocalUser;
		GUILayout.Label("m_LocalUser: " + globalUser.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("DON'T TOUCH THESE IF YOU DO NOT KNOW WHAT THEY DO, YOU COULD CRASH YOUR STEAM CLIENT");
		if (GUILayout.Button("CreateSteamPipe()"))
		{
			m_Pipe = SteamClient.CreateSteamPipe();
			pipe = m_Pipe;
			MonoBehaviour.print("SteamClient.CreateSteamPipe() : " + pipe.ToString());
		}
		if (GUILayout.Button("BReleaseSteamPipe(m_Pipe)"))
		{
			bool flag = SteamClient.BReleaseSteamPipe(m_Pipe);
			pipe = m_Pipe;
			MonoBehaviour.print("SteamClient.BReleaseSteamPipe(" + pipe.ToString() + ") : " + flag);
		}
		if (GUILayout.Button("ConnectToGlobalUser(m_Pipe)"))
		{
			m_GlobalUser = SteamClient.ConnectToGlobalUser(m_Pipe);
			pipe = m_Pipe;
			string text = pipe.ToString();
			globalUser = m_GlobalUser;
			MonoBehaviour.print("SteamClient.ConnectToGlobalUser(" + text + ") : " + globalUser.ToString());
		}
		if (GUILayout.Button("CreateLocalUser(out m_LocalPipe, EAccountType.k_EAccountTypeGameServer)"))
		{
			m_LocalUser = SteamClient.CreateLocalUser(out m_LocalPipe, EAccountType.k_EAccountTypeGameServer);
			string[] obj = new string[6]
			{
				"SteamClient.CreateLocalUser(out m_LocalPipe, ",
				EAccountType.k_EAccountTypeGameServer.ToString(),
				") : ",
				null,
				null,
				null
			};
			globalUser = m_LocalUser;
			obj[3] = globalUser.ToString();
			obj[4] = " -- ";
			pipe = m_LocalPipe;
			obj[5] = pipe.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("ReleaseUser(m_LocalPipe, m_LocalUser)"))
		{
			SteamClient.ReleaseUser(m_LocalPipe, m_LocalUser);
			string[] obj2 = new string[5] { "SteamClient.ReleaseUser(", null, null, null, null };
			pipe = m_LocalPipe;
			obj2[1] = pipe.ToString();
			obj2[2] = ", ";
			globalUser = m_LocalUser;
			obj2[3] = globalUser.ToString();
			obj2[4] = ")";
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("GetISteamUser(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSER_INTERFACE_VERSION)"))
		{
			IntPtr iSteamUser = SteamClient.GetISteamUser(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamUser023");
			MonoBehaviour.print("SteamClient.GetISteamUser(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamUser023) : " + iSteamUser);
		}
		if (GUILayout.Button("GetISteamGameServer(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVER_INTERFACE_VERSION)"))
		{
			IntPtr iSteamGameServer = SteamClient.GetISteamGameServer(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamGameServer015");
			MonoBehaviour.print("SteamClient.GetISteamGameServer(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamGameServer015) : " + iSteamGameServer);
		}
		if (GUILayout.Button("SetLocalIPBinding(ref IpAddress127_0_0_1, TestConstants.k_Port27015)"))
		{
			SteamIPAddress_t unIP = TestConstants.Instance.k_IpAddress127_0_0_1;
			SteamClient.SetLocalIPBinding(ref unIP, 27015);
			string text2 = ((ushort)27015).ToString();
			SteamIPAddress_t steamIPAddress_t = unIP;
			MonoBehaviour.print("SteamClient.SetLocalIPBinding(ref IpAddress127_0_0_1, " + text2 + ") -- " + steamIPAddress_t.ToString());
		}
		if (GUILayout.Button("GetISteamFriends(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMFRIENDS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamFriends = SteamClient.GetISteamFriends(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamFriends017");
			MonoBehaviour.print("SteamClient.GetISteamFriends(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamFriends017) : " + iSteamFriends);
		}
		if (GUILayout.Button("GetISteamUtils(SteamAPI.GetHSteamPipe(), Constants.STEAMUTILS_INTERFACE_VERSION)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamClient.GetISteamUtils(SteamAPI.GetHSteamPipe(), "SteamUtils010").ToString(), str0: "SteamClient.GetISteamUtils(", str1: SteamAPI.GetHSteamPipe().ToString(), str2: ", SteamUtils010) : "));
		}
		if (GUILayout.Button("GetISteamMatchmaking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKING_INTERFACE_VERSION)"))
		{
			IntPtr iSteamMatchmaking = SteamClient.GetISteamMatchmaking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamMatchMaking009");
			MonoBehaviour.print("SteamClient.GetISteamMatchmaking(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamMatchMaking009) : " + iSteamMatchmaking);
		}
		if (GUILayout.Button("GetISteamMatchmakingServers(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamMatchMakingServers002");
			MonoBehaviour.print("SteamClient.GetISteamMatchmakingServers(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamMatchMakingServers002) : " + iSteamMatchmakingServers);
		}
		if (GUILayout.Button("GetISteamGenericInterface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPTICKET_INTERFACE_VERSION)"))
		{
			IntPtr iSteamGenericInterface = SteamClient.GetISteamGenericInterface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMAPPTICKET_INTERFACE_VERSION001");
			MonoBehaviour.print("SteamClient.GetISteamGenericInterface(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMAPPTICKET_INTERFACE_VERSION001) : " + iSteamGenericInterface);
		}
		if (GUILayout.Button("GetISteamUserStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSERSTATS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamUserStats = SteamClient.GetISteamUserStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMUSERSTATS_INTERFACE_VERSION012");
			MonoBehaviour.print("SteamClient.GetISteamUserStats(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMUSERSTATS_INTERFACE_VERSION012) : " + iSteamUserStats);
		}
		if (GUILayout.Button("GetISteamGameServerStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVERSTATS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamGameServerStats = SteamClient.GetISteamGameServerStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamGameServerStats001");
			MonoBehaviour.print("SteamClient.GetISteamGameServerStats(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamGameServerStats001) : " + iSteamGameServerStats);
		}
		if (GUILayout.Button("GetISteamApps(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamApps = SteamClient.GetISteamApps(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMAPPS_INTERFACE_VERSION008");
			MonoBehaviour.print("SteamClient.GetISteamApps(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMAPPS_INTERFACE_VERSION008) : " + iSteamApps);
		}
		if (GUILayout.Button("GetISteamNetworking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMNETWORKING_INTERFACE_VERSION)"))
		{
			IntPtr iSteamNetworking = SteamClient.GetISteamNetworking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamNetworking006");
			MonoBehaviour.print("SteamClient.GetISteamNetworking(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamNetworking006) : " + iSteamNetworking);
		}
		if (GUILayout.Button("GetISteamRemoteStorage(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMREMOTESTORAGE_INTERFACE_VERSION)"))
		{
			IntPtr iSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMREMOTESTORAGE_INTERFACE_VERSION016");
			MonoBehaviour.print("SteamClient.GetISteamRemoteStorage(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMREMOTESTORAGE_INTERFACE_VERSION016) : " + iSteamRemoteStorage);
		}
		if (GUILayout.Button("GetISteamScreenshots(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMSCREENSHOTS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamScreenshots = SteamClient.GetISteamScreenshots(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMSCREENSHOTS_INTERFACE_VERSION003");
			MonoBehaviour.print("SteamClient.GetISteamScreenshots(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMSCREENSHOTS_INTERFACE_VERSION003) : " + iSteamScreenshots);
		}
		if (GUILayout.Button("GetISteamGameSearch(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESEARCH_INTERFACE_VERSION)"))
		{
			IntPtr iSteamGameSearch = SteamClient.GetISteamGameSearch(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamMatchGameSearch001");
			MonoBehaviour.print("SteamClient.GetISteamGameSearch(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamMatchGameSearch001) : " + iSteamGameSearch);
		}
		GUILayout.Label("GetIPCCallCount() : " + SteamClient.GetIPCCallCount());
		if (GUILayout.Button("BShutdownIfAllPipesClosed()"))
		{
			MonoBehaviour.print("SteamClient.BShutdownIfAllPipesClosed() : " + SteamClient.BShutdownIfAllPipesClosed());
		}
		if (GUILayout.Button("GetISteamHTTP(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTTP_INTERFACE_VERSION)"))
		{
			IntPtr iSteamHTTP = SteamClient.GetISteamHTTP(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMHTTP_INTERFACE_VERSION003");
			MonoBehaviour.print("SteamClient.GetISteamHTTP(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMHTTP_INTERFACE_VERSION003) : " + iSteamHTTP);
		}
		if (GUILayout.Button("GetISteamUGC(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUGC_INTERFACE_VERSION)"))
		{
			IntPtr iSteamUGC = SteamClient.GetISteamUGC(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMUGC_INTERFACE_VERSION020");
			MonoBehaviour.print("SteamClient.GetISteamUGC(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMUGC_INTERFACE_VERSION020) : " + iSteamUGC);
		}
		if (GUILayout.Button("GetISteamMusic(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSIC_INTERFACE_VERSION)"))
		{
			IntPtr iSteamMusic = SteamClient.GetISteamMusic(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMMUSIC_INTERFACE_VERSION001");
			MonoBehaviour.print("SteamClient.GetISteamMusic(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMMUSIC_INTERFACE_VERSION001) : " + iSteamMusic);
		}
		if (GUILayout.Button("GetISteamMusicRemote(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSICREMOTE_INTERFACE_VERSION)"))
		{
			IntPtr iSteamMusicRemote = SteamClient.GetISteamMusicRemote(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMMUSICREMOTE_INTERFACE_VERSION001");
			MonoBehaviour.print("SteamClient.GetISteamMusicRemote(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMMUSICREMOTE_INTERFACE_VERSION001) : " + iSteamMusicRemote);
		}
		if (GUILayout.Button("GetISteamHTMLSurface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTMLSURFACE_INTERFACE_VERSION)"))
		{
			IntPtr iSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
			MonoBehaviour.print("SteamClient.GetISteamHTMLSurface(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMHTMLSURFACE_INTERFACE_VERSION_005) : " + iSteamHTMLSurface);
		}
		if (GUILayout.Button("GetISteamInventory(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMINVENTORY_INTERFACE_VERSION)"))
		{
			IntPtr iSteamInventory = SteamClient.GetISteamInventory(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMINVENTORY_INTERFACE_V003");
			MonoBehaviour.print("SteamClient.GetISteamInventory(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMINVENTORY_INTERFACE_V003) : " + iSteamInventory);
		}
		if (GUILayout.Button("GetISteamVideo(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMVIDEO_INTERFACE_VERSION)"))
		{
			IntPtr iSteamVideo = SteamClient.GetISteamVideo(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMVIDEO_INTERFACE_V007");
			MonoBehaviour.print("SteamClient.GetISteamVideo(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMVIDEO_INTERFACE_V007) : " + iSteamVideo);
		}
		if (GUILayout.Button("GetISteamParentalSettings(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMPARENTALSETTINGS_INTERFACE_VERSION)"))
		{
			IntPtr iSteamParentalSettings = SteamClient.GetISteamParentalSettings(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
			MonoBehaviour.print("SteamClient.GetISteamParentalSettings(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMPARENTALSETTINGS_INTERFACE_VERSION001) : " + iSteamParentalSettings);
		}
		if (GUILayout.Button("GetISteamInput(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMINPUT_INTERFACE_VERSION)"))
		{
			IntPtr iSteamInput = SteamClient.GetISteamInput(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamInput006");
			MonoBehaviour.print("SteamClient.GetISteamInput(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamInput006) : " + iSteamInput);
		}
		if (GUILayout.Button("GetISteamParties(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMPARTIES_INTERFACE_VERSION)"))
		{
			IntPtr iSteamParties = SteamClient.GetISteamParties(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "SteamParties002");
			MonoBehaviour.print("SteamClient.GetISteamParties(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", SteamParties002) : " + iSteamParties);
		}
		if (GUILayout.Button("GetISteamRemotePlay(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMREMOTEPLAY_INTERFACE_VERSION)"))
		{
			IntPtr iSteamRemotePlay = SteamClient.GetISteamRemotePlay(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), "STEAMREMOTEPLAY_INTERFACE_VERSION002");
			MonoBehaviour.print("SteamClient.GetISteamRemotePlay(" + SteamAPI.GetHSteamUser().ToString() + ", " + SteamAPI.GetHSteamPipe().ToString() + ", STEAMREMOTEPLAY_INTERFACE_VERSION002) : " + iSteamRemotePlay);
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}
}
