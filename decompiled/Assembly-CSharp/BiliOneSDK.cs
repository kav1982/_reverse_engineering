using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BiliOneSDK
{
	private delegate void InitCallback(string buf, int buflen);

	private delegate void LauncherCallBack(string buf, int buflen);

	private delegate void LoginCallback(string buf, int buflen);

	private delegate void AntiAddictionCallback(string buf, int buflen);

	private delegate void PayCallback(string buf, int buflen);

	private delegate void CrashCallBack(string buf, int buflen);

	private delegate void AgreementCallBack(string buf, int buflen);

	private delegate void CloseAccountCallBack(string buf, int buflen);

	private delegate void LegacyGameInfoCallBack(string buf, int buflen);

	private delegate void GetUserInfoCallBack(string buf, int buflen);

	private delegate void DLCCallBack(string buf, int buflen);

	private delegate void StatusCallBack(string buf, int buflen);

	private static InitCallback initCallback;

	private static CrashCallBack crashCallback;

	private static LauncherCallBack launcherCallback;

	private static GetUserInfoCallBack UserInfoCallback;

	private static DLCCallBack dlcCallback;

	private static AntiAddictionCallback antiAddictionCallback;

	private static PayCallback payCallback;

	private static AgreementCallBack agreementCallBack;

	private static CloseAccountCallBack closeAccountCallBack;

	private static LegacyGameInfoCallBack legacyCallBack;

	private static LoginCallback loginCallback;

	private static StatusCallBack statusCallback;

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKInit(string gameversion, IntPtr hwndParent, bool bExclusiveMode, bool bEnableMultiOpen, InitCallback callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKUnInit();

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKSetWindowsMode(bool bExclusiveMode);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKSetCrashHandler(CrashCallBack callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKLogin(LoginCallback callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKOfflineGameLogin(LoginCallback callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKGetUserInfo(GetUserInfoCallBack CallBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKLoginByQRCode(LoginCallback callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKStartHeartbeat(AntiAddictionCallback callBack);

	[DllImport("PCGameSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKPay(string info, PayCallback callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKLogout();

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKBindLauncher(LauncherCallBack CallBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OneSDKBindLauncher")]
	private static extern int OneSDKBindLauncherNoParent(LauncherCallBack CallBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKUnBindLauncher();

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OneSDKGetUserInfo")]
	private static extern int OneSDKGetDLCInfo(DLCCallBack CallBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKOpenLauncher(LauncherCallBack CallBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKCloseAccount(string info, CloseAccountCallBack callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKShowUserProtocol(int cpServerArea, AgreementCallBack callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKShowPrivacyProtocol(int cpServerArea, AgreementCallBack callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr OneSDKGetChannelInfo();

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKGetLegacyGameInfo(LegacyGameInfoCallBack callBack);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKTrackEvent(string eventId, string pageName, string extensions);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr OneSDKGetDeviceId();

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKShowCustomerService(string extraParam);

	[DllImport("PCOneSDK", CallingConvention = CallingConvention.Cdecl)]
	private static extern int OneSDKSetStatusCallback(StatusCallBack callBack);

	private void CrashResultCallBack(string buf, int buflen)
	{
	}

	private void AgreementResultCallBack(string buf, int buflen)
	{
	}

	private void LoginResultCallback(string buf, int buflen)
	{
		switch ((string?)JObject.Parse(buf)["code"])
		{
		case "0":
		{
			Debug.Log("\ufffd\ufffd¼\ufffdɹ\ufffd");
			int num = OneSDKStartHeartbeat(antiAddictionCallback);
			Debug.Log(num);
			switch (num)
			{
			case -1:
				Debug.Log("\ufffd\ufffd\ufffd\ufffd\ufffdԿ\ufffd\ufffd\ufffdʧ\ufffd\ufffd");
				LogOutReLog();
				break;
			case 1:
				Debug.Log("\ufffd\ufffd\ufffd\ufffd\ufffdԲ\ufffd\ufffd\ufffd");
				break;
			case 0:
				Debug.Log("\ufffd\ufffd\ufffd\ufffd\ufffdԿ\ufffd\ufffd\ufffd\ufffdɹ\ufffd");
				break;
			}
			break;
		}
		case "-1":
			Debug.Log("\ufffd\ufffd¼ʧ\ufffd\ufffd");
			UnInitAndQuit();
			break;
		case "-2":
			Debug.Log("ȡ\ufffd\ufffd\ufffd\ufffd¼");
			UnInitAndQuit();
			break;
		}
	}

	private void LegacyCallback(string buf, int buflen)
	{
	}

	private void LauncherResultCallBack(string buf, int buflen)
	{
	}

	private void UserInfoResultCallBack(string buf, int buflen)
	{
	}

	private void CloseAccountResultCallback(string buf, int buflen)
	{
	}

	private void PayResultCallback(string buf, int buflen)
	{
	}

	private void AntiAddictionResultCallback(string buf, int buflen)
	{
		string text = (string?)JObject.Parse(buf)["code"];
		Debug.Log("AntiAddictionResultCallback:" + text);
		if (text == "0")
		{
			Debug.Log("\ufffd\ufffdʾ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʾ");
		}
		else if (text == "1")
		{
			Debug.Log("\ufffd\ufffdʾ\ufffd\ufffd\ufffd\u05b7\ufffd\ufffd\ufffd\ufffdԳ\ufffdʱ\ufffd\ufffd\ufffd\ufffd\u0368\u05aa\ufffd\ufffdϷ\ufffd\ufffd\ufffd\ufffd");
			LogOutReLog();
		}
	}

	private void DLCResultCallBack(string buf, int buflen)
	{
	}

	private void InitResultCallback(string buf, int buflen)
	{
		string text = (string?)JObject.Parse(buf)["code"];
		Debug.Log("InitResultCallback:" + text);
		switch (text)
		{
		case "0":
			OneSDKLogin(loginCallback);
			break;
		case "-1":
		case "-2":
			Application.Quit();
			break;
		}
	}

	private void StatusResultCallback(string buf, int buflen)
	{
	}

	public void Init()
	{
		initCallback = InitResultCallback;
		launcherCallback = LauncherResultCallBack;
		UserInfoCallback = UserInfoResultCallBack;
		dlcCallback = DLCResultCallBack;
		antiAddictionCallback = AntiAddictionResultCallback;
		payCallback = PayResultCallback;
		agreementCallBack = AgreementResultCallBack;
		crashCallback = CrashResultCallBack;
		closeAccountCallBack = CloseAccountResultCallback;
		loginCallback = LoginResultCallback;
		legacyCallBack = LegacyCallback;
		statusCallback = StatusResultCallback;
		Debug.Log("\ufffd\ufffdʼ\ufffd\ufffdOneSDK");
		OneSDKInit("0.0.0.0", WindowHandle.GetApplicationWindowHandle(), bExclusiveMode: false, bEnableMultiOpen: false, initCallback);
	}

	private void LogIn()
	{
		OneSDKLogin(loginCallback);
	}

	public void UnInitAndQuit()
	{
		try
		{
			OneSDKLogout();
			OneSDKUnInit();
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
		}
		Application.Quit();
	}

	public void LogOutReLog()
	{
		if (OneSDKLogout() == 0)
		{
			LogIn();
		}
	}
}
