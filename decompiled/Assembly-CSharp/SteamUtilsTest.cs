using Steamworks;
using UnityEngine;

public class SteamUtilsTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private Texture2D m_Image;

	private string m_FilterTextInputMessage;

	private bool m_GameLauncherMode;

	protected Callback<IPCountry_t> m_IPCountry;

	protected Callback<LowBatteryPower_t> m_LowBatteryPower;

	protected Callback<SteamShutdown_t> m_SteamShutdown;

	protected Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed;

	protected Callback<AppResumingFromSuspend_t> m_AppResumingFromSuspend;

	protected Callback<FloatingGamepadTextInputDismissed_t> m_FloatingGamepadTextInputDismissed;

	protected Callback<FilterTextDictionaryChanged_t> m_FilterTextDictionaryChanged;

	private CallResult<CheckFileSignature_t> OnCheckFileSignatureCallResult;

	public void OnEnable()
	{
		m_FilterTextInputMessage = "test, fuck, sorry";
		m_IPCountry = Callback<IPCountry_t>.Create(OnIPCountry);
		m_LowBatteryPower = Callback<LowBatteryPower_t>.Create(OnLowBatteryPower);
		m_SteamShutdown = Callback<SteamShutdown_t>.Create(OnSteamShutdown);
		m_GamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed);
		m_AppResumingFromSuspend = Callback<AppResumingFromSuspend_t>.Create(OnAppResumingFromSuspend);
		m_FloatingGamepadTextInputDismissed = Callback<FloatingGamepadTextInputDismissed_t>.Create(OnFloatingGamepadTextInputDismissed);
		m_FilterTextDictionaryChanged = Callback<FilterTextDictionaryChanged_t>.Create(OnFilterTextDictionaryChanged);
		OnCheckFileSignatureCallResult = CallResult<CheckFileSignature_t>.Create(OnCheckFileSignature);
	}

	public static Texture2D GetSteamImageAsTexture2D(int iImage)
	{
		Texture2D texture2D = null;
		if (SteamUtils.GetImageSize(iImage, out var pnWidth, out var pnHeight))
		{
			byte[] array = new byte[pnWidth * pnHeight * 4];
			if (SteamUtils.GetImageRGBA(iImage, array, (int)(pnWidth * pnHeight * 4)))
			{
				texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false, linear: true);
				texture2D.LoadRawTextureData(array);
				texture2D.Apply();
			}
		}
		return texture2D;
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Image:");
		GUILayout.Label(m_Image);
		GUILayout.Label("m_FilterTextInputMessage:");
		m_FilterTextInputMessage = GUILayout.TextField(m_FilterTextInputMessage, 40);
		GUILayout.Label("m_GameLauncherMode:");
		m_GameLauncherMode = false;
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("GetSecondsSinceAppActive() : " + SteamUtils.GetSecondsSinceAppActive());
		GUILayout.Label("GetSecondsSinceComputerActive() : " + SteamUtils.GetSecondsSinceComputerActive());
		GUILayout.Label("GetConnectedUniverse() : " + SteamUtils.GetConnectedUniverse());
		GUILayout.Label("GetServerRealTime() : " + SteamUtils.GetServerRealTime());
		GUILayout.Label("GetIPCountry() : " + SteamUtils.GetIPCountry());
		uint pnWidth = 0u;
		uint pnHeight = 0u;
		bool imageSize = SteamUtils.GetImageSize(1, out pnWidth, out pnHeight);
		GUILayout.Label("SteamUtils.GetImageSize(1, out ImageWidth, out ImageHeight) : " + imageSize + " -- " + pnWidth + " -- " + pnHeight);
		if (GUILayout.Button("SteamUtils.GetImageRGBA(1, Image, (int)(ImageWidth * ImageHeight * 4)") && pnWidth != 0 && pnHeight != 0)
		{
			byte[] array = new byte[pnWidth * pnHeight * 4];
			imageSize = SteamUtils.GetImageRGBA(1, array, (int)(pnWidth * pnHeight * 4));
			MonoBehaviour.print("SteamUtils.GetImageRGBA(1, " + array?.ToString() + ", " + (int)(pnWidth * pnHeight * 4) + ") - " + imageSize + " -- " + pnWidth + " -- " + pnHeight);
			if (imageSize)
			{
				m_Image = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false, linear: true);
				m_Image.LoadRawTextureData(array);
				m_Image.Apply();
			}
		}
		GUILayout.Label("GetCurrentBatteryPower() : " + SteamUtils.GetCurrentBatteryPower());
		GUILayout.Label("GetAppID() : " + SteamUtils.GetAppID().ToString());
		if (GUILayout.Button("SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight)"))
		{
			SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight);
			MonoBehaviour.print("SteamUtils.SetOverlayNotificationPosition(" + ENotificationPosition.k_EPositionTopRight.ToString() + ")");
		}
		GUILayout.Label("GetIPCCallCount() : " + SteamUtils.GetIPCCallCount());
		GUILayout.Label("IsOverlayEnabled() : " + SteamUtils.IsOverlayEnabled());
		GUILayout.Label("BOverlayNeedsPresent() : " + SteamUtils.BOverlayNeedsPresent());
		if (GUILayout.Button("CheckFileSignature(\"FileNotFound.txt\")"))
		{
			SteamAPICall_t steamAPICall_t = SteamUtils.CheckFileSignature("FileNotFound.txt");
			OnCheckFileSignatureCallResult.Set(steamAPICall_t);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamUtils.CheckFileSignature(\"FileNotFound.txt\") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, \"Description Test!\", 32, \"test\")"))
		{
			bool flag = SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "Description Test!", 32u, "test");
			MonoBehaviour.print("SteamUtils.ShowGamepadTextInput(" + EGamepadTextInputMode.k_EGamepadTextInputModeNormal.ToString() + ", " + EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine.ToString() + ", \"Description Test!\", " + 32 + ", \"test\") : " + flag);
		}
		GUILayout.Label("GetSteamUILanguage() : " + SteamUtils.GetSteamUILanguage());
		GUILayout.Label("IsSteamRunningInVR() : " + SteamUtils.IsSteamRunningInVR());
		if (GUILayout.Button("SetOverlayNotificationInset(400, 400)"))
		{
			SteamUtils.SetOverlayNotificationInset(400, 400);
			MonoBehaviour.print("SteamUtils.SetOverlayNotificationInset(" + 400 + ", " + 400 + ")");
		}
		GUILayout.Label("IsSteamInBigPictureMode() : " + SteamUtils.IsSteamInBigPictureMode());
		if (GUILayout.Button("StartVRDashboard()"))
		{
			SteamUtils.StartVRDashboard();
			MonoBehaviour.print("SteamUtils.StartVRDashboard()");
		}
		GUILayout.Label("IsVRHeadsetStreamingEnabled() : " + SteamUtils.IsVRHeadsetStreamingEnabled());
		if (GUILayout.Button("SetVRHeadsetStreamingEnabled(!SteamUtils.IsVRHeadsetStreamingEnabled())"))
		{
			SteamUtils.SetVRHeadsetStreamingEnabled(!SteamUtils.IsVRHeadsetStreamingEnabled());
			MonoBehaviour.print("SteamUtils.SetVRHeadsetStreamingEnabled(" + !SteamUtils.IsVRHeadsetStreamingEnabled() + ")");
		}
		GUILayout.Label("IsSteamChinaLauncher() : " + SteamUtils.IsSteamChinaLauncher());
		if (GUILayout.Button("InitFilterText()"))
		{
			MonoBehaviour.print("SteamUtils.InitFilterText() : " + SteamUtils.InitFilterText());
		}
		if (GUILayout.Button("GetIPv6ConnectivityState(ESteamIPv6ConnectivityProtocol.k_ESteamIPv6ConnectivityProtocol_HTTP)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamUtils.GetIPv6ConnectivityState(ESteamIPv6ConnectivityProtocol.k_ESteamIPv6ConnectivityProtocol_HTTP).ToString(), str0: "SteamUtils.GetIPv6ConnectivityState(", str1: ESteamIPv6ConnectivityProtocol.k_ESteamIPv6ConnectivityProtocol_HTTP.ToString(), str2: ") : "));
		}
		GUILayout.Label("IsSteamRunningOnSteamDeck() : " + SteamUtils.IsSteamRunningOnSteamDeck());
		if (GUILayout.Button("ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, 0, 0, 0, 0)"))
		{
			bool flag2 = SteamUtils.ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, 0, 0, 0, 0);
			MonoBehaviour.print("SteamUtils.ShowFloatingGamepadTextInput(" + EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine.ToString() + ", " + 0 + ", " + 0 + ", " + 0 + ", " + 0 + ") : " + flag2);
		}
		if (GUILayout.Button("SetGameLauncherMode(!m_GameLauncherMode)"))
		{
			SteamUtils.SetGameLauncherMode(!m_GameLauncherMode);
			MonoBehaviour.print("SteamUtils.SetGameLauncherMode(" + !m_GameLauncherMode + ")");
			m_GameLauncherMode = !m_GameLauncherMode;
		}
		if (GUILayout.Button("DismissFloatingGamepadTextInput()"))
		{
			MonoBehaviour.print("SteamUtils.DismissFloatingGamepadTextInput() : " + SteamUtils.DismissFloatingGamepadTextInput());
		}
		if (GUILayout.Button("DismissGamepadTextInput()"))
		{
			MonoBehaviour.print("SteamUtils.DismissGamepadTextInput() : " + SteamUtils.DismissGamepadTextInput());
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnIPCountry(IPCountry_t pCallback)
	{
		Debug.Log("[" + 701 + " - IPCountry]");
	}

	private void OnLowBatteryPower(LowBatteryPower_t pCallback)
	{
		Debug.Log("[" + 702 + " - LowBatteryPower] - " + pCallback.m_nMinutesBatteryLeft);
	}

	private void OnSteamShutdown(SteamShutdown_t pCallback)
	{
		Debug.Log("[" + 704 + " - SteamShutdown]");
	}

	private void OnCheckFileSignature(CheckFileSignature_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 705 + " - CheckFileSignature] - " + pCallback.m_eCheckFileSignature);
	}

	private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			714.ToString(),
			" - GamepadTextInputDismissed] - ",
			pCallback.m_bSubmitted.ToString(),
			" -- ",
			pCallback.m_unSubmittedText.ToString(),
			" -- ",
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[7] = unAppID.ToString();
		Debug.Log(string.Concat(obj));
		if (pCallback.m_bSubmitted)
		{
			Debug.Log("SteamUtils.GetEnteredGamepadTextLength() - " + SteamUtils.GetEnteredGamepadTextLength());
			Debug.Log("SteamUtils.GetEnteredGamepadTextInput(out Text, pCallback.m_unSubmittedText + 1) - " + SteamUtils.GetEnteredGamepadTextInput(out var pchText, pCallback.m_unSubmittedText + 1) + " -- " + pchText);
		}
	}

	private void OnAppResumingFromSuspend(AppResumingFromSuspend_t pCallback)
	{
		Debug.Log("[" + 736 + " - AppResumingFromSuspend]");
	}

	private void OnFloatingGamepadTextInputDismissed(FloatingGamepadTextInputDismissed_t pCallback)
	{
		Debug.Log("[" + 738 + " - FloatingGamepadTextInputDismissed]");
	}

	private void OnFilterTextDictionaryChanged(FilterTextDictionaryChanged_t pCallback)
	{
		Debug.Log("[" + 739 + " - FilterTextDictionaryChanged] - " + pCallback.m_eLanguage);
	}
}
