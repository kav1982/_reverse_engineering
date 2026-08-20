using Steamworks;
using UnityEngine;

public class SteamAppsTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	protected Callback<DlcInstalled_t> m_DlcInstalled;

	protected Callback<NewUrlLaunchParameters_t> m_NewUrlLaunchParameters;

	protected Callback<AppProofOfPurchaseKeyResponse_t> m_AppProofOfPurchaseKeyResponse;

	protected Callback<TimedTrialStatus_t> m_TimedTrialStatus;

	private CallResult<FileDetailsResult_t> OnFileDetailsResultCallResult;

	public void OnEnable()
	{
		m_DlcInstalled = Callback<DlcInstalled_t>.Create(OnDlcInstalled);
		m_NewUrlLaunchParameters = Callback<NewUrlLaunchParameters_t>.Create(OnNewUrlLaunchParameters);
		m_AppProofOfPurchaseKeyResponse = Callback<AppProofOfPurchaseKeyResponse_t>.Create(OnAppProofOfPurchaseKeyResponse);
		m_TimedTrialStatus = Callback<TimedTrialStatus_t>.Create(OnTimedTrialStatus);
		OnFileDetailsResultCallResult = CallResult<FileDetailsResult_t>.Create(OnFileDetailsResult);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("BIsSubscribed() : " + SteamApps.BIsSubscribed());
		GUILayout.Label("BIsLowViolence() : " + SteamApps.BIsLowViolence());
		GUILayout.Label("BIsCybercafe() : " + SteamApps.BIsCybercafe());
		GUILayout.Label("BIsVACBanned() : " + SteamApps.BIsVACBanned());
		GUILayout.Label("GetCurrentGameLanguage() : " + SteamApps.GetCurrentGameLanguage());
		GUILayout.Label("GetAvailableGameLanguages() : " + SteamApps.GetAvailableGameLanguages());
		GUILayout.Label("BIsSubscribedApp(SteamUtils.GetAppID()) : " + SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()));
		GUILayout.Label("BIsDlcInstalled(TestConstants.Instance.k_AppId_PieterwTestDLC) : " + SteamApps.BIsDlcInstalled(TestConstants.Instance.k_AppId_PieterwTestDLC));
		GUILayout.Label("GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()) : " + SteamApps.GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()));
		GUILayout.Label("BIsSubscribedFromFreeWeekend() : " + SteamApps.BIsSubscribedFromFreeWeekend());
		GUILayout.Label("GetDLCCount() : " + SteamApps.GetDLCCount());
		for (int i = 0; i < SteamApps.GetDLCCount(); i++)
		{
			AppId_t pAppID;
			bool pbAvailable;
			string pchName;
			bool flag = SteamApps.BGetDLCDataByIndex(i, out pAppID, out pbAvailable, out pchName, 128);
			string[] obj = new string[10]
			{
				"BGetDLCDataByIndex(",
				i.ToString(),
				", out AppID, out Available, out Name, 128) : ",
				flag.ToString(),
				" -- ",
				null,
				null,
				null,
				null,
				null
			};
			AppId_t appId_t = pAppID;
			obj[5] = appId_t.ToString();
			obj[6] = " -- ";
			obj[7] = pbAvailable.ToString();
			obj[8] = " -- ";
			obj[9] = pchName;
			GUILayout.Label(string.Concat(obj));
		}
		if (GUILayout.Button("InstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC)"))
		{
			SteamApps.InstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC);
			AppId_t appId_t = TestConstants.Instance.k_AppId_PieterwTestDLC;
			MonoBehaviour.print("SteamApps.InstallDLC(" + appId_t.ToString() + ")");
		}
		if (GUILayout.Button("UninstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC)"))
		{
			SteamApps.UninstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC);
			AppId_t appId_t = TestConstants.Instance.k_AppId_PieterwTestDLC;
			MonoBehaviour.print("SteamApps.UninstallDLC(" + appId_t.ToString() + ")");
		}
		if (GUILayout.Button("RequestAppProofOfPurchaseKey(SteamUtils.GetAppID())"))
		{
			SteamApps.RequestAppProofOfPurchaseKey(SteamUtils.GetAppID());
			MonoBehaviour.print("SteamApps.RequestAppProofOfPurchaseKey(" + SteamUtils.GetAppID().ToString() + ")");
		}
		string pchName2;
		bool currentBetaName = SteamApps.GetCurrentBetaName(out pchName2, 128);
		if (pchName2 == null)
		{
			pchName2 = "";
		}
		GUILayout.Label("GetCurrentBetaName(out Name, 128) : " + currentBetaName + " -- " + pchName2);
		if (GUILayout.Button("MarkContentCorrupt(true)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamApps.MarkContentCorrupt(bMissingFilesOnly: true).ToString(), str0: "SteamApps.MarkContentCorrupt(", str1: true.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), Depots, 32)"))
		{
			DepotId_t[] array = new DepotId_t[32];
			uint installedDepots = SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), array, 32u);
			for (int j = 0; j < installedDepots; j++)
			{
				string[] obj3 = new string[6]
				{
					"SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), Depots, 32) : ",
					installedDepots.ToString(),
					" -- #",
					j.ToString(),
					" -- ",
					null
				};
				DepotId_t depotId_t = array[j];
				obj3[5] = depotId_t.ToString();
				MonoBehaviour.print(string.Concat(obj3));
			}
		}
		string pchFolder;
		uint appInstallDir = SteamApps.GetAppInstallDir(SteamUtils.GetAppID(), out pchFolder, 260u);
		if (pchFolder == null)
		{
			pchFolder = "";
		}
		GUILayout.Label("GetAppInstallDir(SteamUtils.GetAppID(), out Folder, 260) : " + appInstallDir + " -- " + pchFolder);
		GUILayout.Label("BIsAppInstalled(SteamUtils.GetAppID()) : " + SteamApps.BIsAppInstalled(SteamUtils.GetAppID()));
		GUILayout.Label("GetAppOwner() : " + SteamApps.GetAppOwner().ToString());
		string launchQueryParam = SteamApps.GetLaunchQueryParam("test");
		GUILayout.Label("GetLaunchQueryParam(\"test\") : " + launchQueryParam);
		ulong punBytesDownloaded;
		ulong punBytesTotal;
		bool dlcDownloadProgress = SteamApps.GetDlcDownloadProgress(TestConstants.Instance.k_AppId_PieterwTestDLC, out punBytesDownloaded, out punBytesTotal);
		GUILayout.Label("GetDlcDownloadProgress(TestConstants.Instance.k_AppId_PieterwTestDLC, out BytesDownloaded, out BytesTotal) : " + dlcDownloadProgress + " -- " + punBytesDownloaded + " -- " + punBytesTotal);
		GUILayout.Label("GetAppBuildId() : " + SteamApps.GetAppBuildId());
		if (GUILayout.Button("RequestAllProofOfPurchaseKeys()"))
		{
			SteamApps.RequestAllProofOfPurchaseKeys();
			MonoBehaviour.print("SteamApps.RequestAllProofOfPurchaseKeys()");
		}
		if (GUILayout.Button("GetFileDetails(\"steam_api.dll\")"))
		{
			SteamAPICall_t fileDetails = SteamApps.GetFileDetails("steam_api.dll");
			OnFileDetailsResultCallResult.Set(fileDetails);
			SteamAPICall_t steamAPICall_t = fileDetails;
			MonoBehaviour.print("SteamApps.GetFileDetails(\"steam_api.dll\") : " + steamAPICall_t.ToString());
		}
		string pszCommandLine;
		int launchCommandLine = SteamApps.GetLaunchCommandLine(out pszCommandLine, 260);
		if (pszCommandLine == null)
		{
			pszCommandLine = "";
		}
		GUILayout.Label("GetLaunchCommandLine(out CommandLine, 260) : " + launchCommandLine + " -- " + pszCommandLine);
		GUILayout.Label("BIsSubscribedFromFamilySharing() : " + SteamApps.BIsSubscribedFromFamilySharing());
		uint punSecondsAllowed;
		uint punSecondsPlayed;
		bool flag2 = SteamApps.BIsTimedTrial(out punSecondsAllowed, out punSecondsPlayed);
		GUILayout.Label("BIsTimedTrial(out punSecondsAllowed, out punSecondsPlayed) : " + flag2 + " -- " + punSecondsAllowed + " -- " + punSecondsPlayed);
		if (GUILayout.Button("SetDlcContext((AppId_t)0)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamApps.SetDlcContext((AppId_t)0u).ToString(), str0: "SteamApps.SetDlcContext(", str1: ((AppId_t)0u).ToString(), str2: ") : "));
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnDlcInstalled(DlcInstalled_t pCallback)
	{
		string text = 1005.ToString();
		AppId_t nAppID = pCallback.m_nAppID;
		Debug.Log("[" + text + " - DlcInstalled] - " + nAppID.ToString());
	}

	private void OnNewUrlLaunchParameters(NewUrlLaunchParameters_t pCallback)
	{
		Debug.Log("[" + 1014 + " - NewUrlLaunchParameters]");
	}

	private void OnAppProofOfPurchaseKeyResponse(AppProofOfPurchaseKeyResponse_t pCallback)
	{
		Debug.Log("[" + 1021 + " - AppProofOfPurchaseKeyResponse] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_cchKeyLength + " -- " + pCallback.m_rgchKey);
	}

	private void OnFileDetailsResult(FileDetailsResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1023 + " - FileDetailsResult] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_ulFileSize + " -- " + pCallback.m_FileSHA?.ToString() + " -- " + pCallback.m_unFlags);
	}

	private void OnTimedTrialStatus(TimedTrialStatus_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			1030.ToString(),
			" - TimedTrialStatus] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[3] = unAppID.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_bIsOffline.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_unSecondsAllowed.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_unSecondsPlayed.ToString();
		Debug.Log(string.Concat(obj));
	}
}
