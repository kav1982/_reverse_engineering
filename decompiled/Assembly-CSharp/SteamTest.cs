using System;
using System.Collections;
using System.Text;
using Steamworks;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
	public enum EGUIState
	{
		SteamApps,
		SteamClient,
		SteamFriends,
		SteamHTMLSurface,
		SteamHTTP,
		SteamInput,
		SteamInventory,
		SteamMatchmaking,
		SteamMatchmakingServers,
		SteamMusic,
		SteamMusicRemote,
		SteamNetworking,
		SteamParentalSettings,
		SteamParties,
		SteamRemoteStorage,
		SteamScreenshots,
		SteamTimeline,
		SteamUGC,
		SteamUser,
		SteamUserStatsTest,
		SteamUtils,
		SteamVideo,
		MAX_STATES
	}

	private bool m_bInitialized;

	private static SteamTest m_SteamTest;

	private SteamAppsTest AppsTest;

	private SteamClientTest ClientTest;

	private SteamFriendsTest FriendsTest;

	private SteamHTMLSurfaceTest HTMLSurfaceTest;

	private SteamHTTPTest HTTPTest;

	private SteamInputTest InputTest;

	private SteamInventoryTest InventoryTest;

	private SteamMatchmakingServersTest MatchmakingServersTest;

	private SteamMatchmakingTest MatchmakingTest;

	private SteamMusicRemoteTest MusicRemoteTest;

	private SteamMusicTest MusicTest;

	private SteamNetworkingTest NetworkingTest;

	private SteamParentalSettingsTest ParentalSettingsTest;

	private SteamPartiesTest PartiesTest;

	private SteamRemoteStorageTest RemoteStorageTest;

	private SteamScreenshotsTest ScreenshotsTest;

	private SteamTimelineTest TimelineTest;

	private SteamUGCTest UGCTest;

	private SteamUserStatsTest UserStatsTest;

	private SteamUserTest UserTest;

	private SteamUtilsTest UtilsTest;

	private SteamVideoTest VideoTest;

	private SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;

	public EGUIState m_State { get; private set; }

	[MonoPInvokeCallback]
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
		if (m_SteamTest != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		m_SteamTest = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			throw new Exception("Packsize is wrong! You are likely using a Linux/OSX build on Windows or vice versa.");
		}
		if (!DllCheck.Test())
		{
			throw new Exception("DllCheck returned false.");
		}
		try
		{
			m_bInitialized = SteamAPI.Init();
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		if (!m_bInitialized)
		{
			Debug.LogError("SteamAPI_Init() failed", this);
			return;
		}
		SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
		SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);
		AppsTest = base.gameObject.AddComponent<SteamAppsTest>();
		ClientTest = base.gameObject.AddComponent<SteamClientTest>();
		FriendsTest = base.gameObject.AddComponent<SteamFriendsTest>();
		HTMLSurfaceTest = base.gameObject.AddComponent<SteamHTMLSurfaceTest>();
		HTTPTest = base.gameObject.AddComponent<SteamHTTPTest>();
		InputTest = base.gameObject.AddComponent<SteamInputTest>();
		InventoryTest = base.gameObject.AddComponent<SteamInventoryTest>();
		MatchmakingServersTest = base.gameObject.AddComponent<SteamMatchmakingServersTest>();
		MatchmakingTest = base.gameObject.AddComponent<SteamMatchmakingTest>();
		MusicRemoteTest = base.gameObject.AddComponent<SteamMusicRemoteTest>();
		MusicTest = base.gameObject.AddComponent<SteamMusicTest>();
		NetworkingTest = base.gameObject.AddComponent<SteamNetworkingTest>();
		ParentalSettingsTest = base.gameObject.AddComponent<SteamParentalSettingsTest>();
		PartiesTest = base.gameObject.AddComponent<SteamPartiesTest>();
		TimelineTest = base.gameObject.AddComponent<SteamTimelineTest>();
		RemoteStorageTest = base.gameObject.AddComponent<SteamRemoteStorageTest>();
		UGCTest = base.gameObject.AddComponent<SteamUGCTest>();
		UserStatsTest = base.gameObject.AddComponent<SteamUserStatsTest>();
		UserTest = base.gameObject.AddComponent<SteamUserTest>();
		UtilsTest = base.gameObject.AddComponent<SteamUtilsTest>();
		VideoTest = base.gameObject.AddComponent<SteamVideoTest>();
		ScreenshotsTest = base.gameObject.AddComponent<SteamScreenshotsTest>();
	}

	private void OnEnable()
	{
		if (m_SteamTest == null)
		{
			m_SteamTest = this;
		}
		if (m_bInitialized && SteamAPIWarningMessageHook == null)
		{
			SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);
		}
	}

	private void OnDestroy()
	{
		if (m_bInitialized)
		{
			SteamAPI.Shutdown();
		}
	}

	private void Update()
	{
		if (!m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			m_State++;
			if (m_State == EGUIState.MAX_STATES)
			{
				m_State = EGUIState.SteamApps;
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			m_State--;
			if (m_State == (EGUIState)(-1))
			{
				m_State = EGUIState.SteamVideo;
			}
		}
	}

	private void OnGUI()
	{
		if (!m_bInitialized)
		{
			GUILayout.Label("Steamworks is not Initialized");
			return;
		}
		GUILayout.Label("[" + (int)(m_State + 1) + " / " + 22 + "] " + m_State);
		switch (m_State)
		{
		case EGUIState.SteamApps:
			AppsTest.RenderOnGUI();
			break;
		case EGUIState.SteamClient:
			ClientTest.RenderOnGUI();
			break;
		case EGUIState.SteamFriends:
			FriendsTest.RenderOnGUI();
			break;
		case EGUIState.SteamHTMLSurface:
			HTMLSurfaceTest.RenderOnGUI();
			break;
		case EGUIState.SteamHTTP:
			HTTPTest.RenderOnGUI();
			break;
		case EGUIState.SteamInput:
			InputTest.RenderOnGUI();
			break;
		case EGUIState.SteamInventory:
			InventoryTest.RenderOnGUI();
			break;
		case EGUIState.SteamMatchmaking:
			MatchmakingTest.RenderOnGUI();
			break;
		case EGUIState.SteamMatchmakingServers:
			MatchmakingServersTest.RenderOnGUI();
			break;
		case EGUIState.SteamMusic:
			MusicTest.RenderOnGUI();
			break;
		case EGUIState.SteamMusicRemote:
			MusicRemoteTest.RenderOnGUI();
			break;
		case EGUIState.SteamNetworking:
			NetworkingTest.RenderOnGUI();
			break;
		case EGUIState.SteamParentalSettings:
			ParentalSettingsTest.RenderOnGUI();
			break;
		case EGUIState.SteamParties:
			PartiesTest.RenderOnGUI();
			break;
		case EGUIState.SteamRemoteStorage:
			RemoteStorageTest.RenderOnGUI();
			break;
		case EGUIState.SteamScreenshots:
			ScreenshotsTest.RenderOnGUI();
			break;
		case EGUIState.SteamTimeline:
			TimelineTest.RenderOnGUI();
			break;
		case EGUIState.SteamUGC:
			UGCTest.RenderOnGUI();
			break;
		case EGUIState.SteamUser:
			UserTest.RenderOnGUI();
			break;
		case EGUIState.SteamUserStatsTest:
			UserStatsTest.RenderOnGUI();
			break;
		case EGUIState.SteamUtils:
			UtilsTest.RenderOnGUI();
			break;
		case EGUIState.SteamVideo:
			VideoTest.RenderOnGUI();
			break;
		}
	}

	public static void PrintArray(string name, IList arr)
	{
		StringBuilder stringBuilder = new StringBuilder(name + "\n");
		for (int i = 0; i < arr.Count; i++)
		{
			stringBuilder.AppendLine(arr[i].ToString());
		}
		MonoBehaviour.print(stringBuilder);
	}
}
