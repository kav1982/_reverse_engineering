using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GameServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerLogger;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PluginActivity : MonoBehaviour
{
	public enum LogoutReason
	{
		None,
		Manual,
		LoginFail
	}

	public enum ChannleID
	{
		B服 = 1001,
		AppleStore = 1002,
		东银河官包 = 1003,
		BILI漫画 = 1004,
		版署 = 1005,
		CPS = 1006,
		Tap = 1101,
		好游快爆 = 1102,
		应用宝 = 48
	}

	public class PayExtensionInfo
	{
		public string item;

		public PayExtensionInfo(string productID)
		{
			item = productID;
		}
	}

	[Serializable]
	public class PayResult
	{
		public string outTradeNo;

		public string orderNo;

		public string rechargeOrderNo;

		public string productCount;

		public string productName;

		public string productDesc;

		public string extension;

		public string amount;

		public string productId;
	}

	public abstract class MobileSDK
	{
		public const int GameId = 2103140;

		public void OnSDKInit()
		{
			SDKInited = true;
			Debug.Log("SDK 初始化成功");
			Inst.UpdateLogInfo();
		}

		public abstract void Initialize();

		public abstract void OneSDKLogout();

		public abstract void LogIn();

		public abstract void CloseAccount();

		public abstract void CreateCharacter();

		public abstract string GetADChanelId();

		public abstract void NotifyZone();

		public abstract void ShowUserProtocol(int area);

		public abstract void ShowPrivacyProtocol(int area);

		public abstract void OnPaySuccess(string json);

		public abstract void OnPayFailed(string json);

		public virtual void PayTest()
		{
			string orderNum = GenerateOrderNumber24();
			PayTest("支付测试", "", 1, orderNum, "", "");
		}

		public abstract void PurchaseItem(string productName, string productID, int amount, string orderNum, string order_sign, string extensionInfo, string notifyUrl);

		public virtual void OnLogIn(string msg = "")
		{
			if ((bool)UIMainMenuMgr.Inst)
			{
				MobileMgr.inst.PluginActivity.UpdateLogInfo();
				CNHCHFKLMOH.ServerCheckAfterLog();
			}
			UploadPlayerLogin();
		}

		public abstract void PayTest(string productName, string productID, int amount, string orderNum, string extensionInfo, string notifyUrl);

		public abstract string[] GetLogInfo();

		public abstract string GetPlatform();

		protected abstract void SDKUploadEvent(string eventName, Dictionary<string, object> param);

		public virtual string GetNetworkStatus()
		{
			return Application.internetReachability switch
			{
				NetworkReachability.ReachableViaLocalAreaNetwork => "WIFI", 
				NetworkReachability.ReachableViaCarrierDataNetwork => "MOBILE", 
				_ => "Disconnected", 
			};
		}

		public virtual string GetMacAddress()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public void OnLogFail(string msg, string channle)
		{
			SetToUnlog();
			if ((bool)UIMainMenuMgr.Inst)
			{
				if (channle == ChannleID.应用宝.ChannleID())
				{
					GameUISingletonMono<UICommonHint>.ShowInit(msg);
				}
				else
				{
					GameUISingletonMono<UICommonHint>.HideIfInited();
				}
				return;
			}
			TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
			SceneManager.LoadScene("MainMenu");
			if (GameUISingletonMono<UIDialogueMgr>.Inited)
			{
				GameUISingletonMono<UIDialogueMgr>.DestroyUI();
			}
		}

		public void OnLogOut(string msg = "")
		{
			SetToUnlog();
			if ((bool)MainMenuMgr.Inst)
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
				UIMainMenuMgr.Inst.uiMainMenu.btn_buyFullGame.gameObject.SetActive(value: false);
				return;
			}
			TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
			SceneManager.LoadScene("MainMenu");
			if (GameUISingletonMono<UIDialogueMgr>.Inited)
			{
				GameUISingletonMono<UIDialogueMgr>.DestroyUI();
			}
		}

		private void SetToUnlog()
		{
			UploadPlayerLogout();
			logUid = "";
			logAccessToken = "";
			channleID = "";
			brandID = "";
			areaID = "";
			ServerLogged = false;
			if ((bool)MainMenuMgr.Inst)
			{
				uiMainMenu.SetStartButtonToLog();
				if (UIMgr.Inst.uiSetting.IsOpen)
				{
					UIMgr.Inst.uiSetting.CloseAll();
				}
			}
		}

		public abstract void SDKQuitGame();

		public virtual void Toast(string msg)
		{
		}

		public abstract void UnInitAndQuit();

		public void UploadUserSnapshot(int snapShotType)
		{
			try
			{
				Dictionary<string, object> commonParams = GetCommonParams(isSnapShot: true);
				commonParams.Add("save_index", DataMgr.currentSelectWorldIndex);
				commonParams.Add("gold_surplus", OutBattleGoldStatue.GetJson());
				commonParams.Add("talent_status", TalentStatus.GetJson());
				commonParams.Add("suit", Suit.GetJson());
				commonParams.Add("unlock_spell_ids", JsonConvert.SerializeObject(DataMgr.selectedWorldData.activateGirlActivatedIDs2));
				commonParams.Add("researched_ids", JsonConvert.SerializeObject(DataMgr.selectedWorldData.researchedIDs));
				commonParams.Add("battleInfo", BattleInfo.GetJson());
				commonParams.Add("snapshot_type", snapShotType);
				commonParams.Add("b_eventname", "user_snapshot");
				if (ICJNOGPFMAM.KEMAJLGHMEL.JKBPGJFFJNN.HasValue)
				{
					commonParams.Add("first_order_time", GeneralTool.FormatTimestamp((long)ICJNOGPFMAM.KEMAJLGHMEL.JKBPGJFFJNN.Value + 28800L));
				}
				commonParams.Add("first_order_timestamp", ICJNOGPFMAM.KEMAJLGHMEL.JKBPGJFFJNN + 28800);
				commonParams.Add("role_ctime_timestamp", ICJNOGPFMAM.KEMAJLGHMEL.CMIKFLKFNPD + 28800);
				commonParams.Add("cumulative_payment", ICJNOGPFMAM.KEMAJLGHMEL.CNKAOEAMNCC);
				SDKUploadEvent("user_snapshot", commonParams);
			}
			catch (Exception ex)
			{
				Debug.LogError("埋点报错UploadUserSnapshot");
				Debug.LogError(ex.StackTrace);
			}
		}

		public void UploadItemSnapshot(int snapShotType)
		{
			try
			{
				Dictionary<string, object> commonParams = GetCommonParams(isSnapShot: true);
				commonParams.Add("save_index", DataMgr.currentSelectWorldIndex);
				commonParams.Add("item_info", PlayerEquips.GetJson());
				commonParams.Add("in_battle_gold", InBattleGoldStatue.GetJson());
				commonParams.Add("snapshot_type", snapShotType);
				commonParams.Add("b_eventname", "item_snapshot");
				commonParams.Add("current_difficulty", (int)DataMgr.selectedWorldData.selectedDifficulty);
				commonParams.Add("current_roomId", DataMgr.selectedWorldData.battleData9.currentRoomID);
				SDKUploadEvent("item_snapshot", commonParams);
			}
			catch (Exception ex)
			{
				Debug.LogError("埋点报错UploadItemSnapshot");
				Debug.LogError(ex.StackTrace);
			}
		}

		public void UploadPlayerLogin()
		{
			try
			{
				ICJNOGPFMAM.KEMAJLGHMEL.BFDIONPBHBG = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				string properties = JsonConvert.SerializeObject(new Dictionary<string, object>
				{
					{
						"model",
						SystemInfo.deviceModel
					},
					{
						"os_version",
						SystemInfo.operatingSystem
					},
					{
						"mac",
						GetMacAddress()
					},
					{
						"network",
						GetNetworkStatus()
					}
				});
				UploadEvent("player_login", properties);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		public void UploadPlayerLogout()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (ICJNOGPFMAM.KEMAJLGHMEL.BFDIONPBHBG > 0)
			{
				long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ICJNOGPFMAM.KEMAJLGHMEL.BFDIONPBHBG;
				dictionary.Add("online_time", num);
			}
			dictionary.Add("model", SystemInfo.deviceModel);
			dictionary.Add("os_version", SystemInfo.operatingSystem);
			dictionary.Add("mac", GetMacAddress());
			dictionary.Add("network", GetNetworkStatus());
			string properties = JsonConvert.SerializeObject(dictionary);
			UploadEvent("player_logout", properties);
		}

		public void UploadEvent(string eventName, string properties)
		{
			try
			{
				if (ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK && ScriptableObjMgr.Inst.testCtrller.UseServer)
				{
					Dictionary<string, object> commonParams = GetCommonParams();
					commonParams.Add("properties", properties);
					commonParams.Add("b_eventname", eventName);
					SDKUploadEvent(eventName, commonParams);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("埋点报错UploadEvent");
				Debug.LogError(ex.StackTrace);
			}
		}

		public Dictionary<string, object> GetCommonParams(bool isSnapShot = false)
		{
			long num = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			int num2 = UnityEngine.Random.Range(100000, 999999);
			string value = $"{2103140}{num % 1000000000}{num2}";
			int.TryParse(globalGameId, out var result);
			int.TryParse(channleID, out var result2);
			int.TryParse(brandID, out var result3);
			int.TryParse(areaID, out var result4);
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "b_log_id", value },
				{
					"b_udid",
					SystemInfo.deviceUniqueIdentifier
				},
				{ "b_sdk_uid", logUid },
				{
					"b_account_id",
					ClientSettings.Uid
				},
				{
					"b_tour_indicator",
					(!ICJNOGPFMAM.MIFJADDOODN) ? 1 : 0
				},
				{
					"b_role_id",
					ClientSettings.Uid
				},
				{
					"b_datetime",
					DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")
				},
				{ "b_global_game_id", result },
				{ "b_global_channel_id", result2 },
				{ "b_brand_id", result3 },
				{ "b_area_id", result4 },
				{
					"b_platform",
					GetPlatform()
				},
				{
					"b_ad_channel_id",
					StringToInt(adChannleID)
				},
				{
					"b_version",
					VersionSO.Inst.AsString()
				},
				{
					"b_env",
					CNHCHFKLMOH.GIOEIPNCMEF
				}
			};
			if (isSnapShot)
			{
				dictionary.Add("b_snapshot_date", DateTime.Now.ToString("yyyyMMdd"));
				dictionary.Add("b_snapshot_timestamp", num / 1000);
			}
			else
			{
				dictionary.Add("b_utc_timestamp", num);
			}
			return dictionary;
		}

		private int StringToInt(string str, int @default = 0)
		{
			if (string.IsNullOrEmpty(str))
			{
				return @default;
			}
			if (int.TryParse(str, out var result))
			{
				return result;
			}
			return @default;
		}

		public abstract void ShowCustomerService(bool fullScreen, Dictionary<string, object> param, bool showToolBar);

		public abstract void OpenArchiveManager();

		public abstract void TestCrash();
	}

	public class AndroidOneSDK : MobileSDK
	{
		private AndroidJavaObject AndroidCurrentActivity => new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

		public override void Initialize()
		{
		}

		public override void OneSDKLogout()
		{
			CallByNameMainThread("OneSDKLogout", null);
		}

		public override void LogIn()
		{
			CallByNameMainThread("OneSDKLogin", null);
		}

		public override void CloseAccount()
		{
			CallByNameMainThread("CloseAccount", null);
		}

		public override void CreateCharacter()
		{
			CallByNameMainThread("CreateCharacter", logUid, "test");
		}

		public override void NotifyZone()
		{
			CallByNameMainThread("notifyZone", logUid, "test");
		}

		public override void ShowUserProtocol(int area)
		{
			CallByNameMainThread("ShowUserProtocol", area);
		}

		public override void ShowPrivacyProtocol(int area)
		{
			CallByNameMainThread("ShowPrivacyProtocol", area);
		}

		public void ForceKillAndroid()
		{
			AndroidCurrentActivity.Call("RunOnMainThread", "ForceKillAndroid", null);
		}

		public override void OnPaySuccess(string json)
		{
		}

		public override void OnPayFailed(string json)
		{
		}

		public override void PurchaseItem(string productName, string productID, int amount, string orderNum, string order_sign, string extensionInfo, string notifyUrl)
		{
			CallByNameMainThread("payTest", productName, productID, amount.ToString(), logUid, orderNum, order_sign, extensionInfo, notifyUrl);
		}

		public override void PayTest(string productName, string productID, int amount, string orderNum, string extensionInfo, string notifyUrl)
		{
			CallByNameMainThread("payTest", productName, productID, amount.ToString(), logUid, orderNum, GenerateSignature(orderNum, amount, amount, productID, notifyUrl, "1d5ffd123fca41ab9bd74f0cf23a9612"), extensionInfo, notifyUrl);
		}

		public override void Toast(string msg)
		{
			if (AndroidCurrentActivity != null)
			{
				AndroidCurrentActivity.Call("ShowToast", msg);
			}
		}

		public override void UnInitAndQuit()
		{
		}

		public override string[] GetLogInfo()
		{
			_ = new string[6];
			return AndroidCurrentActivity.Call<string[]>("GetLogInfo", Array.Empty<object>());
		}

		public override string GetADChanelId()
		{
			return AndroidCurrentActivity.Call<string>("GetADChanelId", Array.Empty<object>());
		}

		public override string GetPlatform()
		{
			return "android";
		}

		protected override void SDKUploadEvent(string eventName, Dictionary<string, object> param)
		{
			try
			{
				AndroidJavaObject androidJavaObject = DicToJavaHashMap(param);
				CallByNameMainThread("SDKUploadEvent", eventName, androidJavaObject);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.StackTrace);
			}
		}

		public static AndroidJavaObject DicToJavaHashMap(Dictionary<string, object> dict)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			foreach (KeyValuePair<string, object> item in dict)
			{
				using AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", item.Key);
				using AndroidJavaObject androidJavaObject3 = ConvertToJavaObject(item.Value);
				jvalue[] array = new jvalue[2];
				array[0].l = androidJavaObject2.GetRawObject();
				array[1].l = androidJavaObject3?.GetRawObject() ?? IntPtr.Zero;
				AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, array);
			}
			return androidJavaObject;
		}

		private static AndroidJavaObject ConvertToJavaObject(object value)
		{
			if (value == null)
			{
				return new AndroidJavaObject("java.lang.String", "");
			}
			if (value is string text)
			{
				return new AndroidJavaObject("java.lang.String", text);
			}
			if (value is int num)
			{
				return new AndroidJavaObject("java.lang.Integer", num);
			}
			if (value is long num2)
			{
				return new AndroidJavaObject("java.lang.Long", num2);
			}
			if (value is float num3)
			{
				return new AndroidJavaObject("java.lang.Float", num3);
			}
			if (value is double num4)
			{
				return new AndroidJavaObject("java.lang.Double", num4);
			}
			if (value is bool flag)
			{
				return new AndroidJavaObject("java.lang.Boolean", flag);
			}
			if (value is IList list)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "add", "(Ljava/lang/Object;)Z");
				{
					foreach (object item in list)
					{
						using AndroidJavaObject androidJavaObject2 = ConvertToJavaObject(item);
						jvalue[] array = new jvalue[1];
						array[0].l = androidJavaObject2?.GetRawObject() ?? IntPtr.Zero;
						AndroidJNI.CallBooleanMethod(androidJavaObject.GetRawObject(), methodID, array);
					}
					return androidJavaObject;
				}
			}
			if (value is AndroidJavaObject result)
			{
				return result;
			}
			return new AndroidJavaObject("java.lang.String", value.ToString());
		}

		public override void SDKQuitGame()
		{
			CallByNameMainThread("QuitSDK", null);
		}

		private void CallByNameMainThread(string callname, params object[] parameters)
		{
			if (!ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK || !ScriptableObjMgr.Inst.testCtrller.UseServer || AndroidCurrentActivity == null)
			{
				return;
			}
			if (parameters == null || parameters.Length == 0)
			{
				AndroidCurrentActivity.Call("RunOnMainThread", callname, null);
				return;
			}
			AndroidJavaObject[] array = new AndroidJavaObject[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = ConvertToJavaObject(parameters[i]);
			}
			AndroidCurrentActivity.Call("RunOnMainThread", callname, array);
		}

		public override void ShowCustomerService(bool fullScreen, Dictionary<string, object> param, bool showToolBar)
		{
			AndroidJavaObject androidJavaObject = DicToJavaHashMap(param);
			CallByNameMainThread("ShowCustomerService", fullScreen, androidJavaObject, showToolBar);
		}

		public string GetBuildID()
		{
			try
			{
				return AndroidCurrentActivity.Call<string>("getBuildId", Array.Empty<object>());
			}
			catch
			{
				return "FallbackBuildID";
			}
		}

		public override void OpenArchiveManager()
		{
			DataMgr.CombineToCloudSaveFile();
			string text = DataMgr.dataPath + DataMgr.mobileCloudSaveDataFileName;
			string text2 = "云存档";
			long num = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			CallByNameMainThread("OpenArchiveManager", text, text2, num);
		}

		public override void TestCrash()
		{
			CallByNameMainThread("CrashTest", null);
		}
	}

	public class EditorSDK : MobileSDK
	{
		public override void Initialize()
		{
		}

		public override void OneSDKLogout()
		{
		}

		public override void LogIn()
		{
		}

		public override void CloseAccount()
		{
		}

		public override void CreateCharacter()
		{
		}

		public override string GetADChanelId()
		{
			return string.Empty;
		}

		public override void OpenArchiveManager()
		{
		}

		public override void TestCrash()
		{
		}

		public override void NotifyZone()
		{
		}

		public override void ShowUserProtocol(int area)
		{
		}

		public override void ShowPrivacyProtocol(int area)
		{
		}

		public override void OnPaySuccess(string json)
		{
		}

		public override void OnPayFailed(string json)
		{
		}

		public override void PurchaseItem(string productName, string productID, int amount, string orderNum, string order_sign, string extensionInfo, string notifyUrl)
		{
		}

		public override void PayTest(string productName, string productID, int amount, string orderNum, string extensionInfo, string notifyUrl)
		{
		}

		public override string[] GetLogInfo()
		{
			return new string[6] { brandID, areaID, "1000009", channleID, logUid, logAccessToken };
		}

		public override string GetPlatform()
		{
			return "windows";
		}

		protected override void SDKUploadEvent(string eventName, Dictionary<string, object> param)
		{
			AndroidOneSDK.DicToJavaHashMap(param);
		}

		public override void SDKQuitGame()
		{
		}

		public override void UnInitAndQuit()
		{
		}

		public override void ShowCustomerService(bool fullScreen, Dictionary<string, object> param, bool showToolBar)
		{
		}
	}

	public const string cpServerId = "test";

	public static PluginActivity Inst;

	public static bool SDKInited;

	public static bool ServerLogged;

	public static string brandID;

	public static string areaID;

	public static string channleID;

	public static string adChannleID;

	public static string logUid;

	public static string logAccessToken;

	public static string globalGameId;

	public static AndroidOneSDK androidSDK = new AndroidOneSDK();

	public static LogoutReason CurrentLogoutReason = LogoutReason.None;

	private const int ReTryTimeLimit = 10;

	private float checkOrderTimeCount = 10f;

	private float redPointRequestDelayTimer;

	private int _redPointRequestDelay;

	private string currentOrderProductId;

	private string currentOrderProductName;

	private ServerAPI.OpenOrderInfo currentOrderInfo;

	public static EditorSDK editorSDK = new EditorSDK();

	private int ReCheckTokenCount;

	private bool _hasUnReadMessage;

	private int redPointRequestDelay
	{
		get
		{
			return _redPointRequestDelay;
		}
		set
		{
			_redPointRequestDelay = value;
			PlayerPrefs.SetInt("redPointRequestDelay", value);
		}
	}

	private MobileSDK CurrentSDK => editorSDK;

	public bool HasUnReadMessage
	{
		get
		{
			return _hasUnReadMessage;
		}
		set
		{
			PlayerPrefs.SetInt("HasUnReadMessage", value ? 1 : 0);
			_hasUnReadMessage = value;
		}
	}

	public string GetBuildID()
	{
		return "";
	}

	public void OnMemoryWarningFromIOS(string msg)
	{
		Debug.LogWarning("iOS Memory Warning Received!");
		Resources.UnloadUnusedAssets();
	}

	private void Awake()
	{
		if (Inst == null)
		{
			Inst = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		HasUnReadMessage = PlayerPrefs.GetInt("HasUnReadMessage", 0) == 1;
		redPointRequestDelay = PlayerPrefs.GetInt("redPointRequestDelay", 0);
	}

	public void SetClientSetting()
	{
		if (brandID == "" || areaID == "" || channleID == "")
		{
			ClientSettings.BrandId = 101;
			ClientSettings.AreaId = 0;
			ClientSettings.ChannelId = 1001;
		}
		else
		{
			ClientSettings.BrandId = Convert.ToInt32(brandID);
			ClientSettings.AreaId = Convert.ToInt32(areaID);
			ClientSettings.ChannelId = Convert.ToInt32(channleID);
		}
		ClientSettings.Uid = logUid ?? "";
		ClientSettings.Token = logAccessToken ?? "";
	}

	public void OnSDKInit()
	{
		CurrentSDK.OnSDKInit();
	}

	public void CreateCharacter()
	{
		CurrentSDK.CreateCharacter();
	}

	public void NotifyZone()
	{
		CurrentSDK.NotifyZone();
	}

	public void PayTest()
	{
		CurrentSDK.PayTest();
	}

	private void OnApplicationQuit()
	{
		CurrentSDK.UploadPlayerLogout();
	}

	public void PayTest(string productName, string productID, int amount, string orderNum, string extensionInfoJson, string notifyUrl)
	{
		CurrentSDK.PayTest(productName, productID, amount, orderNum, extensionInfoJson, notifyUrl);
	}

	public void OneSDKLogout(LogoutReason logoutReason = LogoutReason.None)
	{
		CurrentLogoutReason = logoutReason;
		CurrentSDK.OneSDKLogout();
	}

	public void CloseAccount()
	{
		CurrentSDK.CloseAccount();
	}

	public void ShowPrivacyProtocol()
	{
		CurrentSDK.ShowPrivacyProtocol(1);
	}

	public void ShowUserProtocol()
	{
		CurrentSDK.ShowUserProtocol(1);
	}

	public void LogIn()
	{
		CurrentSDK.LogIn();
	}

	public void OnLogOut(string msg = "")
	{
		switch (CurrentLogoutReason)
		{
		case LogoutReason.Manual:
			GameUISingletonMono<UICommonHint>.ShowInit("退出登录成功");
			break;
		}
		CurrentLogoutReason = LogoutReason.None;
		CurrentSDK.OnLogOut(msg);
	}

	public void ShowCustomerService(bool fullScreen, bool showToolBar)
	{
		Dictionary<string, object> param = new Dictionary<string, object>
		{
			{ "roleId", logUid },
			{ "roleName", logUid },
			{ "cpServerId", "test" },
			{ "device_orientation", "portrait" }
		};
		CurrentSDK.ShowCustomerService(fullScreen, param, showToolBar);
		HasUnReadMessage = false;
	}

	public void GetUnReadMessageToken()
	{
		if (redPointRequestDelay <= 0)
		{
			redPointRequestDelay = 15;
			StartCoroutine(TryGetUnReadMessageToken());
		}
	}

	private IEnumerator TryGetUnReadMessageToken()
	{
		yield return ServerAPI.StartGetAIHelpToken(delegate
		{
			ReCheckTokenCount = 8;
			StartCoroutine(GetAIHelpToken(0f));
		}, delegate
		{
		});
	}

	private IEnumerator GetAIHelpToken(float delay)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		yield return ServerAPI.CheckAIHelpToken(delegate(Response<ServerAPI.CheckAIHelpTokenResultData> response)
		{
			if (response.code == StatusCode.NoAIHelpToken)
			{
				if (ReCheckTokenCount > 0)
				{
					ReCheckTokenCount--;
					StartCoroutine(GetAIHelpToken(1f));
				}
				else
				{
					HasUnReadMessage = false;
				}
			}
			else
			{
				ReCheckTokenCount = 0;
				if (response.data != null)
				{
					HasUnReadMessage = !string.IsNullOrEmpty(response.data.token);
					redPointRequestDelay = 300;
					redPointRequestDelayTimer = 0f;
				}
				else
				{
					HasUnReadMessage = false;
				}
			}
		}, delegate
		{
			HasUnReadMessage = false;
		});
	}

	public void OpenArchiveManager()
	{
		CurrentSDK.OpenArchiveManager();
	}

	public void UploadUserSnapshot(int snapShotType)
	{
		CurrentSDK.UploadUserSnapshot(snapShotType);
	}

	public void UploadItemSnapshot(int snapShotType)
	{
		CurrentSDK.UploadItemSnapshot(snapShotType);
	}

	public void PurchaseItem(string productName, string productID, int amount, string orderNum, string order_sign, string notifyUrl)
	{
		string extensionInfo = JsonConvert.SerializeObject(new PayExtensionInfo(productID));
		CurrentSDK.PurchaseItem(productName, productID, amount, orderNum, order_sign, extensionInfo, notifyUrl);
	}

	public void UploadEvent(string eventName, string properties)
	{
		CurrentSDK.UploadEvent(eventName, properties);
	}

	public void OnAccountClosed(string msg = "")
	{
		OneSDKLogout();
		if (!UIMainMenuMgr.Inst)
		{
			GameMgr.QuitGame();
		}
	}

	public void OnLoginFail(string msg = "")
	{
		CurrentSDK.OnLogFail(msg, GetChannleID());
	}

	public void OnLogIn(string msg = "")
	{
		CurrentSDK.OnLogIn(msg);
	}

	public void UnInitAndQuit()
	{
		CurrentSDK.UnInitAndQuit();
	}

	public void SDKQuitGame()
	{
		CurrentSDK.SDKQuitGame();
	}

	public void ShowQuitGame(string msg = "")
	{
		if ((bool)UIMainMenuMgr.Inst)
		{
			UIMainMenuMgr.Inst.uiMainMenu.confirmQuitPanel.SetActive(value: true);
		}
		else if (UIMgr.Inst.UIMenu.IsOpen)
		{
			UIMgr.Inst.UIMenu.text_ConfirmTitle.text = 1000206.GetText();
			UIMgr.Inst.UIMenu.Panel_Confirm.gameObject.SetActive(value: true);
		}
		else
		{
			GameMgr.QuitGame();
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ForceKillAndroid()
	{
	}

	public string GetChannleID()
	{
		string[] array = new string[6];
		array = CurrentSDK.GetLogInfo();
		if (array.Length < 6)
		{
			Debug.LogError("oneSDK_getLogInfo() 返回信息不完整！");
			return "错误：信息不足";
		}
		return array[3];
	}

	private void Update()
	{
		if (checkOrderTimeCount > 0f)
		{
			checkOrderTimeCount -= Time.unscaledDeltaTime;
			checkOrderTimeCount = Mathf.Clamp(checkOrderTimeCount, 0f, 10f);
		}
		if (redPointRequestDelay > 0)
		{
			redPointRequestDelayTimer += Time.deltaTime;
			if (redPointRequestDelayTimer >= 1f)
			{
				redPointRequestDelayTimer -= 1f;
				redPointRequestDelay--;
			}
		}
	}

	public string UpdateLogInfo()
	{
		string[] array = new string[6];
		array = CurrentSDK.GetLogInfo();
		if (array.Length < 6)
		{
			Debug.LogError("oneSDK_getLogInfo() 返回信息不完整！");
			return "错误：信息不足";
		}
		logUid = array[0];
		logAccessToken = array[1];
		globalGameId = array[2];
		channleID = array[3];
		brandID = array[4];
		areaID = array[5];
		adChannleID = CurrentSDK.GetADChanelId();
		return "uid:" + array[0] + "\ntoken:" + array[1] + "\nglobalGameId:" + array[2] + "\nglobalChannelID:" + array[3] + "\nbrandID:" + array[4] + "\nareaID:" + array[5] + "\n";
	}

	public void Toast(string msg)
	{
		CurrentSDK.Toast(msg);
	}

	public void OnPaySuccess(string data)
	{
		Debug.Log("PluginActivity.OnPaySuccess -> 支付成功");
		checkOrderTimeCount = 10f;
		CheckOrder();
	}

	public void OnPayFailed(string data)
	{
		GameUISingletonMono<UICommonHint>.ShowInit(("支付失败", true));
	}

	public void Buy(ServerAPI.ProductItem productItem)
	{
		var (productName, productID, _) = ServerAPI.DicProducts[productItem];
		GameUISingletonMono<UICommonHint>.ShowInit(("正在支付中，请稍后...", false));
		string notifyUrl = ClientSettings.Servers[0] + "store/order/callback";
		IEnumerator routine = ServerAPI.OpenOrder(productID, notifyUrl, delegate(Response<ServerAPI.OpenOrderInfo> response)
		{
			if (!CNHCHFKLMOH.ProcessLogMagicraftServerStatue(response.code).success)
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
			}
			else
			{
				currentOrderProductId = productID;
				currentOrderProductName = productName;
				currentOrderInfo = response.data;
				PurchaseItem(productName, productID, response.data.money, response.data.out_order_no, response.data.order_sign, notifyUrl);
			}
		}, delegate
		{
			GameUISingletonMono<UICommonHint>.ShowInit(("支付请求失败", true));
		});
		StartCoroutine(routine);
	}

	private void CheckOrder()
	{
		IEnumerator routine = ServerAPI.CheckOrderState(currentOrderInfo.out_order_no, delegate(Response<ServerAPI.CheckOrderResult> response)
		{
			if (!CNHCHFKLMOH.ProcessLogMagicraftServerStatue(response.code).success)
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
			}
			else
			{
				switch (response.data.state)
				{
				case ServerAPI.OrderStatus.Processing:
					StartCoroutine(WaitAndCheckOrder());
					Debug.Log("UIFullGame.OnOrderStateResponse -> Processing...");
					break;
				case ServerAPI.OrderStatus.Success:
					PurchaseSuccess();
					break;
				default:
					Debug.Log("检查支付错误");
					throw new ArgumentOutOfRangeException();
				}
			}
		}, delegate(UnityWebRequest err)
		{
			GameUISingletonMono<UICommonHint>.ShowInit(("检查订单请求错误", true));
			Debug.LogError("PluginActivity.CheckOrder -> 检查订单请求错误 " + err.error);
		});
		StartCoroutine(routine);
		void PurchaseSuccess()
		{
			GameUISingletonMono<UICommonHint>.HideIfInited();
			JObject jObject = new JObject
			{
				["order_id"] = currentOrderInfo.out_order_no,
				["amount"] = currentOrderInfo.money,
				["product_id"] = currentOrderProductId,
				["product_type"] = 300,
				["product_name"] = currentOrderProductName,
				["product_number"] = 1,
				["total_pay"] = ICJNOGPFMAM.KEMAJLGHMEL.CNKAOEAMNCC + currentOrderInfo.money
			};
			UploadEvent("recharge_flow", jObject.ToString(Formatting.None));
			ICJNOGPFMAM.KEMAJLGHMEL.OnPurchaseSuccess(new string[1] { currentOrderProductId });
			currentOrderProductId = string.Empty;
			currentOrderProductName = string.Empty;
			currentOrderInfo = null;
			Debug.Log("UIFullGame.OnOrderStateResponse -> Success");
		}
	}

	private IEnumerator WaitAndCheckOrder()
	{
		yield return new WaitForSecondsRealtime(1f);
		if (checkOrderTimeCount > 0f)
		{
			CheckOrder();
		}
		else
		{
			GameUISingletonMono<UICommonHint>.ShowInit(("检查订单超时", true));
		}
	}

	private static string GenerateOrderNumber24()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		int num2 = UnityEngine.Random.Range(100000, 999999);
		return $"{num % 1000000000}{num2:D6}";
	}

	private static string GenerateSignature(string outTradeNo, int amount, int gameMoney, string productId, string notifyUrl, string secretKey)
	{
		List<KeyValuePair<string, string>> list = new Dictionary<string, string>
		{
			{ "out_trade_no", outTradeNo },
			{
				"money",
				amount.ToString()
			},
			{
				"game_money",
				gameMoney.ToString()
			},
			{ "product_id", productId },
			{ "notify_url", notifyUrl }
		}.OrderBy((KeyValuePair<string, string> p) => p.Key).ToList();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> item in list)
		{
			stringBuilder.Append(item.Value);
		}
		return OrderMD5(stringBuilder.ToString() + secretKey);
	}

	private static string OrderMD5(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public void TestCrashNative()
	{
		CurrentSDK.TestCrash();
	}
}
