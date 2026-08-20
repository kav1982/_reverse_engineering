using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameServer;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ICJNOGPFMAM
{
	[CompilerGenerated]
	private sealed class AHGDNAIEKIA
	{
		public ServerAPI.GameItem item;

		internal bool _003CIsHaveGameItemInCache_003Eb__0(string x)
		{
			NLEKMOCGJJF CS_0024_003C_003E8__locals0 = new NLEKMOCGJJF
			{
				x = x
			};
			return ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == CS_0024_003C_003E8__locals0.x).itemContains.Contains(item);
		}
	}

	[CompilerGenerated]
	private sealed class NLEKMOCGJJF
	{
		public string x;

		internal bool _003CIsHaveGameItemInCache_003Eb__1((string name, string id, ServerAPI.GameItem[] itemContains) value)
		{
			return value.id == x;
		}
	}

	[CompilerGenerated]
	private sealed class KIILOOCBCIC
	{
		public ServerAPI.ProductItem productItem;

		internal bool _003CGetCost_003Eb__0(ServerAPI.CommodityInfo x)
		{
			return x.item == ServerAPI.DicProducts[productItem].id;
		}
	}

	[CompilerGenerated]
	private sealed class PJCAACIFHCH
	{
		public ICJNOGPFMAM _003C_003E4__this;

		public Action onResponse;

		internal void _003CSyncExtensionInfo_003Eb__0(Response<ServerAPI.ExtensionAccountInfo> response)
		{
			if (response.code == StatusCode.Success)
			{
				_003C_003E4__this.SetExtensionInfo(response.data);
				onResponse?.Invoke();
			}
			else
			{
				Debug.LogError($"RoleServerData.SyncExtensionInfo -> {response.code}");
			}
		}
	}

	private static ICJNOGPFMAM CNHGICDNFGC;

	public long BFDIONPBHBG;

	private HashSet<string> DKKDIBDGPMD = new HashSet<string>();

	public ServerAPI.CommodityInfo[] DGNJLGDEMAP;

	public int? JKBPGJFFJNN;

	public int? CMIKFLKFNPD;

	public int CNKAOEAMNCC;

	public static ICJNOGPFMAM KEMAJLGHMEL => CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());

	public HashSet<string> LNGKEJDDNGN => DKKDIBDGPMD.ToHashSet();

	public static bool MIFJADDOODN
	{
		get
		{
			switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
			{
			case TestItemForceAvailable.Disabled:
				if (GameMgr.IsMobile_Static)
				{
					return KEMAJLGHMEL.IsHaveGame();
				}
				return true;
			case TestItemForceAvailable.Have:
				return true;
			case TestItemForceAvailable.DontHave:
				return false;
			default:
				return false;
			}
		}
	}

	public static bool OBKJLONPFGA
	{
		get
		{
			if (!FIKDMCBJPCO && !ACPKKMJKOJD)
			{
				return BHEHHIFGJOE;
			}
			return true;
		}
	}

	public static bool IMFNIOLONJP
	{
		get
		{
			if (FIKDMCBJPCO && ACPKKMJKOJD)
			{
				return BHEHHIFGJOE;
			}
			return false;
		}
	}

	public static bool GGPJCCLPBJL => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveDaveDLC, ServerAPI.GameItem.DaveDlc);

	public static bool HLMJIJADLNC => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveEndlessDLC, ServerAPI.GameItem.EndlessDlc);

	public static bool FIKDMCBJPCO => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.SuitHalloween);

	public static bool BHEHHIFGJOE => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSummerDLC, ServerAPI.GameItem.SuitSummer);

	public static bool ACPKKMJKOJD => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.SuitSpring);

	public static bool MADIIMLEMNP => HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveChristmasDLC, ServerAPI.GameItem.SuitChristmas);

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_12()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_58()
	{
		return __BB_OBFUSCATOR_17(ScriptableObjMgr.staticTestCtrller.OverrideHaveSummerDLC, ServerAPI.GameItem.SuitChristmas);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_79()
	{
		if (!__BB_OBFUSCATOR_65() && !__BB_OBFUSCATOR_41())
		{
			return __BB_OBFUSCATOR_58();
		}
		return true;
	}

	public static bool DuplicateItem(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().Any(HaveItemConsiderOverride);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_57()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveChristmasDLC, ServerAPI.GameItem.EndlessDlc);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_22()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveEndlessDLC, ServerAPI.GameItem.SuitSummer);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_71()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_108().__BB_OBFUSCATOR_114();
			}
			return true;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return false;
		}
	}

	public bool __BB_OBFUSCATOR_0()
	{
		return true;
	}

	private bool __BB_OBFUSCATOR_132(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	public bool __BB_OBFUSCATOR_82()
	{
		return false;
	}

	public bool IsHaveChristmas()
	{
		return false;
	}

	private bool __BB_OBFUSCATOR_1(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_20()
	{
		return __BB_OBFUSCATOR_147(ScriptableObjMgr.staticTestCtrller.OverrideHaveEndlessDLC, ServerAPI.GameItem.SuitHalloween);
	}

	public void __BB_OBFUSCATOR_134(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Disabled)
		{
			DKKDIBDGPMD.Add("已解锁完整版");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	public static string __BB_OBFUSCATOR_122(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (__BB_OBFUSCATOR_4().DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = __BB_OBFUSCATOR_146().DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return " ";
			}
			return ((float)commodityInfo.current_price / 879f).ToString("登录成功:控制台开启").TrimEnd('\uffdd').TrimEnd('/');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "登录展示公告", 
			ServerAPI.ProductItem.SuitHalloween => "检查登录状态", 
			ServerAPI.ProductItem.HalloweenBundle => "登录成功:控制台开启", 
			ServerAPI.ProductItem.SuitSpring => "已解锁", 
			ServerAPI.ProductItem.SuitSummer => "B站服务器请求失败", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "https://api.ipify.org", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "com.bilibili.mfgy.product002", 
			ServerAPI.ProductItem.SuitSummerBundle => " ", 
			ServerAPI.ProductItem.SuitDeluxe => "RoleServerData.SyncExtensionInfo -> ", 
			ServerAPI.ProductItem.GameDeluxe => "服务器连接失败", 
			_ => "登录展示公告", 
		};
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_129()
	{
		return __BB_OBFUSCATOR_83(ScriptableObjMgr.staticTestCtrller.OverrideHaveChristmasDLC, (ServerAPI.GameItem)8);
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_104()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	public bool __BB_OBFUSCATOR_86()
	{
		return true;
	}

	private bool IsHaveGameItemInCache(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_85()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveDaveDLC, (ServerAPI.GameItem)8);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_65()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.SuitHalloween);
	}

	private static bool __BB_OBFUSCATOR_147(TestItemForceAvailable FDGPPMFOLGF, ServerAPI.GameItem IJGKDFEGAMM)
	{
		switch (FDGPPMFOLGF)
		{
		case TestItemForceAvailable.Disabled:
		{
			int value = IJGKDFEGAMM switch
			{
				ServerAPI.GameItem.SuitHalloween => -46, 
				ServerAPI.GameItem.SuitSpring => 8, 
				ServerAPI.GameItem.EndlessDlc => 99, 
				ServerAPI.GameItem.Game => -69, 
				ServerAPI.GameItem.SuitChristmas => 92, 
				ServerAPI.GameItem.DaveDlc => 52, 
				ServerAPI.GameItem.SuitSummer => 22, 
				_ => throw new ArgumentOutOfRangeException("B站服务器请求失败", IJGKDFEGAMM, null), 
			};
			if (SteamManager.Initialized)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t((uint)value));
			}
			return false;
		}
		case TestItemForceAvailable.Have:
			return false;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return true;
		}
	}

	public bool __BB_OBFUSCATOR_107()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return true;
		}
		return IsHaveGameItemInCache(ServerAPI.GameItem.Game);
	}

	private bool __BB_OBFUSCATOR_113(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	public static bool __BB_OBFUSCATOR_120(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return !ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(__BB_OBFUSCATOR_78);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_84()
	{
		return __BB_OBFUSCATOR_147(ScriptableObjMgr.staticTestCtrller.OverrideHaveSummerDLC, ServerAPI.GameItem.SuitSpring);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_6()
	{
		return __BB_OBFUSCATOR_17(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.Game);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_34()
	{
		return __BB_OBFUSCATOR_83(ScriptableObjMgr.staticTestCtrller.OverrideHaveSummerDLC, ServerAPI.GameItem.SuitChristmas);
	}

	private bool __BB_OBFUSCATOR_69(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Any((string x) => ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == x).itemContains.Contains(HLKIMJAGGOK));
	}

	public bool __BB_OBFUSCATOR_16()
	{
		return false;
	}

	public void __BB_OBFUSCATOR_143(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_130(EMICNIMLIHL);
		SyncExtensionInfo(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i++)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 4294967141u)
			{
				switch (num)
				{
				case 4294967133u:
					if (!(text == "正在登陆检查"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("F2");
					continue;
				case 139u:
					if (!(text == "已解锁"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("登录失败:账号被禁");
					continue;
				case 4294967197u:
					if (!(text == ","))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("服务器正在维护");
					continue;
				}
			}
			else if (num <= 4294967139u)
			{
				switch (num)
				{
				case 159u:
					if (!(text == "RoleServerData.AddItem -> 添加新物品"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("uiMainMenu.WaitForLogCheck -> 检查版本错误");
					__BB_OBFUSCATOR_46();
					continue;
				case 43u:
					if (!(text == "激活码不可用"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit(",");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 198u:
					if (!(text == "已解锁"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
					__BB_OBFUSCATOR_3();
					continue;
				case 175u:
					if (!(text == "ServerMgr.CheckServerEnvironment -> 检查服务器环境"))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.ActivateGirl);
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("com.bilibili.mfgy.product002");
		}
	}

	public bool __BB_OBFUSCATOR_137()
	{
		return false;
	}

	public void __BB_OBFUSCATOR_67(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("31" + first_recharge_time);
		Debug.Log("com.bilibili.mfgy.product004" + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	public void __BB_OBFUSCATOR_26(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("43" + first_recharge_time);
		Debug.Log(" " + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_14()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveSummerDLC, ServerAPI.GameItem.DaveDlc);
	}

	private bool IsHaveGameItemInCache(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Any((string x) => ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == x).itemContains.Contains(HLKIMJAGGOK));
	}

	public static bool __BB_OBFUSCATOR_53(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().Any(HaveItemConsiderOverride);
	}

	internal static void __BB_OBFUSCATOR_3()
	{
		if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
		{
			GameUISingletonMono<UIFullGame>.Inst.Hide();
		}
		GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
		{
			if (DataMgr.selectedWorldData.InBuyGameRoom)
			{
				GameMgr.Inst.RecycleAllPool();
				SceneManager.LoadScene("Battle");
			}
		};
	}

	public static string __BB_OBFUSCATOR_11(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (__BB_OBFUSCATOR_108().DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = __BB_OBFUSCATOR_2().DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "gameItem";
			}
			return ((float)commodityInfo.current_price / 1381f).ToString("6").TrimEnd('￥').TrimEnd('\'');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "已解锁", 
			ServerAPI.ProductItem.SuitHalloween => "RoleServerData.SyncExtensionInfo -> ", 
			ServerAPI.ProductItem.HalloweenBundle => "ServerMgr.SyncServerEnvironment -> 请求错误 ", 
			ServerAPI.ProductItem.SuitSpring => "Battle", 
			ServerAPI.ProductItem.SuitSummer => ",", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "uiMainMenu.WaitForLogCheck -> 检查更新", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "Battle", 
			ServerAPI.ProductItem.SuitSummerBundle => "检查登录失败", 
			ServerAPI.ProductItem.SuitDeluxe => "com.bilibili.mfgy.product001", 
			ServerAPI.ProductItem.GameDeluxe => "正在登陆检查", 
			_ => "com.bilibili.mfgy.product002", 
		};
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_90()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	public static bool __BB_OBFUSCATOR_47(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().Any(__BB_OBFUSCATOR_78);
	}

	public static string __BB_OBFUSCATOR_138(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (KEMAJLGHMEL.DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = __BB_OBFUSCATOR_104().DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "无法购买商品";
			}
			return ((float)commodityInfo.current_price / 316f).ToString("12").TrimEnd('ﾡ').TrimEnd('￩');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "", 
			ServerAPI.ProductItem.SuitHalloween => "账号被禁用", 
			ServerAPI.ProductItem.HalloweenBundle => "魔法服务器请求成功", 
			ServerAPI.ProductItem.SuitSpring => "检查登录状态", 
			ServerAPI.ProductItem.SuitSummer => "com.bilibili.mfgy.product005", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "正在登陆检查", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "登录失败:账号被禁", 
			ServerAPI.ProductItem.SuitSummerBundle => "创建角色请求失败", 
			ServerAPI.ProductItem.SuitDeluxe => "com.bilibili.mfgy.product005", 
			ServerAPI.ProductItem.GameDeluxe => "Battle", 
			_ => "未找到账户", 
		};
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_2()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	public void __BB_OBFUSCATOR_117(string[] BMDEJICFJOC)
	{
		for (int i = 1; i < BMDEJICFJOC.Length; i += 0)
		{
			string text = BMDEJICFJOC[i];
			if (DKKDIBDGPMD.Add(text))
			{
				Debug.Log("RoleServerData.SyncExtensionInfo -> " + text);
				EventMgr.RoleItemChange?.Invoke();
			}
		}
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_41()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.SuitSpring);
	}

	internal static void __BB_OBFUSCATOR_55()
	{
		if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
		{
			GameUISingletonMono<UIFullGame>.Inst.Hide();
		}
		GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
		{
			if (DataMgr.selectedWorldData.InBuyGameRoom)
			{
				GameMgr.Inst.RecycleAllPool();
				SceneManager.LoadScene("Battle");
			}
		};
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_45()
	{
		if (__BB_OBFUSCATOR_6() && __BB_OBFUSCATOR_92())
		{
			return __BB_OBFUSCATOR_14();
		}
		return true;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_51()
	{
		return __BB_OBFUSCATOR_147(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.SuitHalloween);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_27()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.Game);
	}

	public void __BB_OBFUSCATOR_95(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_117(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i++)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 0u:
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
			case 16u:
			case 17u:
			case 18u:
			case 19u:
			case 20u:
			case 21u:
			case 22u:
			case 23u:
			case 24u:
			case 25u:
			case 26u:
			case 27u:
			case 28u:
			case 29u:
			case 30u:
			case 31u:
			case 32u:
			case 33u:
			case 34u:
			case 35u:
			case 36u:
			case 37u:
			case 38u:
			case 39u:
			case 40u:
			case 41u:
			case 42u:
			case 43u:
			case 44u:
			case 46u:
			case 47u:
			case 48u:
			case 49u:
			case 50u:
			case 51u:
			case 52u:
			case 53u:
			case 54u:
			case 55u:
			case 56u:
			case 57u:
			case 58u:
			case 59u:
			case 60u:
			case 61u:
			case 62u:
			case 63u:
				switch (num)
				{
				case 73u:
					if (!(text == "已解锁"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("创建角色请求失败");
					continue;
				case 0u:
					if (!(text == "6"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("服务器正在维护");
					continue;
				}
				break;
			case 64u:
			case 65u:
			case 66u:
			case 67u:
			case 68u:
			case 69u:
			case 70u:
			case 71u:
				switch (num)
				{
				case 21u:
					if (!(text == "获取服务器价格表失败,将显示默认价格"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit(",");
					__BB_OBFUSCATOR_46();
					continue;
				case 41u:
					if (!(text == "6"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("F2");
					continue;
				}
				break;
			case 4294967105u:
				if (!(text == "激活码兑换成功"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("可以开始游戏");
				__BB_OBFUSCATOR_56();
				continue;
			case 175u:
				if (!(text == "角色已经存在"))
				{
					break;
				}
				GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.Research);
				continue;
			case 45u:
				if (!(text == "com.bilibili.mfgy.product005"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit(",");
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit("创建角色");
		}
	}

	public void __BB_OBFUSCATOR_105(string[] BMDEJICFJOC)
	{
		for (int i = 1; i < BMDEJICFJOC.Length; i++)
		{
			string text = BMDEJICFJOC[i];
			if (DKKDIBDGPMD.Add(text))
			{
				Debug.Log("B站服务器请求成功" + text);
				EventMgr.RoleItemChange?.Invoke();
			}
		}
	}

	public bool IsHaveGame()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return true;
		}
		return IsHaveGameItemInCache(ServerAPI.GameItem.Game);
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_108()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	public static bool __BB_OBFUSCATOR_63(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return !ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(__BB_OBFUSCATOR_78);
	}

	internal static void __BB_OBFUSCATOR_46()
	{
		if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
		{
			GameUISingletonMono<UIFullGame>.Inst.Hide();
		}
		GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
		{
			if (DataMgr.selectedWorldData.InBuyGameRoom)
			{
				GameMgr.Inst.RecycleAllPool();
				SceneManager.LoadScene("Battle");
			}
		};
	}

	public bool __BB_OBFUSCATOR_112()
	{
		return false;
	}

	public bool __BB_OBFUSCATOR_114()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return true;
		}
		return __BB_OBFUSCATOR_76(ServerAPI.GameItem.Game);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_68()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_2().__BB_OBFUSCATOR_62();
			}
			return true;
		case TestItemForceAvailable.Have:
			return false;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return false;
		}
	}

	public void __BB_OBFUSCATOR_28(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_105(EMICNIMLIHL);
		__BB_OBFUSCATOR_109(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 141)
			{
				switch (num)
				{
				case 111u:
					if (!(text == "检查登录失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("");
					continue;
				case 4294967250u:
					if (!(text == "激活码不可用"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("12");
					continue;
				case 4294967207u:
					if (!(text == "-------------"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("请求结束");
					continue;
				}
			}
			else if (num <= 4294967179u)
			{
				switch (num)
				{
				case 4294967143u:
					if (!(text == "com.bilibili.mfgy.product006"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("Failed to get public IP: ");
					__BB_OBFUSCATOR_55();
					continue;
				case 5u:
					if (!(text == "账号被禁用"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("uiMainMenu.WaitForLogCheck -> 检查版本错误");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 58u:
					if (!(text == "服务器正在维护"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("服务器正在维护");
					__BB_OBFUSCATOR_46();
					continue;
				case 196u:
					if (!(text == "已解锁"))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.TrainingRoom);
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("B站服务器请求失败");
		}
	}

	public void __BB_OBFUSCATOR_99(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Have)
		{
			DKKDIBDGPMD.Add("没有更新的");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_9()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.EndlessDlc);
	}

	public void __BB_OBFUSCATOR_36(string[] BMDEJICFJOC)
	{
		for (int i = 1; i < BMDEJICFJOC.Length; i += 0)
		{
			string text = BMDEJICFJOC[i];
			if (DKKDIBDGPMD.Add(text))
			{
				Debug.Log("已解锁" + text);
				EventMgr.RoleItemChange?.Invoke();
			}
		}
	}

	public bool __BB_OBFUSCATOR_62()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return false;
		}
		return IsHaveGameItemInCache(ServerAPI.GameItem.SuitHalloween);
	}

	internal static void __BB_OBFUSCATOR_89()
	{
		if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
		{
			GameUISingletonMono<UIFullGame>.Inst.Hide();
		}
		GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
		{
			if (DataMgr.selectedWorldData.InBuyGameRoom)
			{
				GameMgr.Inst.RecycleAllPool();
				SceneManager.LoadScene("Battle");
			}
		};
	}

	public bool __BB_OBFUSCATOR_116()
	{
		return true;
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_111()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	public void __BB_OBFUSCATOR_136(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Disabled)
		{
			DKKDIBDGPMD.Add("43");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	public bool __BB_OBFUSCATOR_49()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return true;
		}
		return __BB_OBFUSCATOR_69(ServerAPI.GameItem.Game);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_87()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_108().__BB_OBFUSCATOR_62();
			}
			return false;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return false;
		}
	}

	public void OnPurchaseSuccess(string[] EMICNIMLIHL)
	{
		AddItem(EMICNIMLIHL);
		SyncExtensionInfo(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 0; i < EMICNIMLIHL.Length; i++)
		{
			switch (EMICNIMLIHL[i])
			{
			case "com.bilibili.mfgy.product001":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁完整版");
				OnPurchaseGame();
				break;
			case "com.bilibili.mfgy.product003":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				OnPurchaseGame();
				break;
			case "com.bilibili.mfgy.product002":
				GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.UnlockDLC1);
				break;
			case "com.bilibili.mfgy.product004":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				break;
			case "com.bilibili.mfgy.product007":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				break;
			case "com.bilibili.mfgy.product005":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				break;
			case "com.bilibili.mfgy.product006":
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				break;
			default:
				GameUISingletonMono<UICommonHint>.ShowInit("激活码兑换成功");
				break;
			}
		}
		static void OnPurchaseGame()
		{
			if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
			{
				GameUISingletonMono<UIFullGame>.Inst.Hide();
			}
			GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
			{
				if (DataMgr.selectedWorldData.InBuyGameRoom)
				{
					GameMgr.Inst.RecycleAllPool();
					SceneManager.LoadScene("Battle");
				}
			};
		}
	}

	public static bool __BB_OBFUSCATOR_103(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return !ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_60()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_104().__BB_OBFUSCATOR_62();
			}
			return true;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return false;
		}
	}

	public static bool __BB_OBFUSCATOR_124(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return !ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_142()
	{
		return __BB_OBFUSCATOR_83(ScriptableObjMgr.staticTestCtrller.OverrideHaveDaveDLC, ServerAPI.GameItem.SuitChristmas);
	}

	public bool __BB_OBFUSCATOR_141()
	{
		return true;
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_94()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	public void SyncExtensionInfo(Action EJIKDDEMHNH = null)
	{
		IEnumerator extensionAccountInfo = ServerAPI.GetExtensionAccountInfo(delegate(Response<ServerAPI.ExtensionAccountInfo> response)
		{
			if (response.code == StatusCode.Success)
			{
				SetExtensionInfo(response.data);
				EJIKDDEMHNH?.Invoke();
			}
			else
			{
				Debug.LogError($"RoleServerData.SyncExtensionInfo -> {response.code}");
			}
		}, delegate(UnityWebRequest err)
		{
			Debug.LogError("RoleServerData.SyncExtensionInfo -> " + err.error);
		});
		GameMgr.Inst.StartCoroutine(extensionAccountInfo);
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_23()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	public static string GetCost(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (KEMAJLGHMEL.DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = KEMAJLGHMEL.DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "";
			}
			return ((float)commodityInfo.current_price / 100f).ToString("F2").TrimEnd('0').TrimEnd('.');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "25", 
			ServerAPI.ProductItem.SuitHalloween => "6", 
			ServerAPI.ProductItem.HalloweenBundle => "31", 
			ServerAPI.ProductItem.SuitSpring => "6", 
			ServerAPI.ProductItem.SuitSummer => "6", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "12", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "37", 
			ServerAPI.ProductItem.SuitSummerBundle => "31", 
			ServerAPI.ProductItem.SuitDeluxe => "18", 
			ServerAPI.ProductItem.GameDeluxe => "43", 
			_ => "", 
		};
	}

	public void __BB_OBFUSCATOR_48(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("Failed to get public IP: " + first_recharge_time);
		Debug.Log("" + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_32()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveDaveDLC, ServerAPI.GameItem.Game);
	}

	private bool __BB_OBFUSCATOR_76(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Any((string x) => ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == x).itemContains.Contains(HLKIMJAGGOK));
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_92()
	{
		return __BB_OBFUSCATOR_147(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.SuitHalloween);
	}

	public bool __BB_OBFUSCATOR_126()
	{
		return true;
	}

	public static string __BB_OBFUSCATOR_125(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (__BB_OBFUSCATOR_90().DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = __BB_OBFUSCATOR_104().DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "已解锁";
			}
			return ((float)commodityInfo.current_price / 1890f).ToString("无法购买商品").TrimEnd('ﾭ').TrimEnd('ﾾ');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "创建角色请求失败", 
			ServerAPI.ProductItem.SuitHalloween => "RoleServerData.AddItem -> 添加新物品", 
			ServerAPI.ProductItem.HalloweenBundle => "角色已创建", 
			ServerAPI.ProductItem.SuitSpring => "gameItem", 
			ServerAPI.ProductItem.SuitSummer => "F2", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "无法购买商品", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "订单验证失败", 
			ServerAPI.ProductItem.SuitSummerBundle => "18", 
			ServerAPI.ProductItem.SuitDeluxe => "无法购买商品", 
			ServerAPI.ProductItem.GameDeluxe => "com.bilibili.mfgy.product007", 
			_ => "获取服务器价格表失败,将显示默认价格", 
		};
	}

	public void __BB_OBFUSCATOR_118(string[] EMICNIMLIHL)
	{
		AddItem(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i++)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 4294967258u:
			case 4294967259u:
			case 4294967260u:
			case 4294967261u:
			case 4294967262u:
			case 4294967263u:
			case 4294967264u:
			case 4294967265u:
			case 4294967266u:
			case 4294967267u:
			case 4294967268u:
			case 4294967269u:
			case 4294967270u:
			case 4294967271u:
			case 4294967272u:
			case 4294967273u:
			case 4294967274u:
			case 4294967275u:
			case 4294967276u:
			case 4294967277u:
			case 4294967278u:
			case 4294967279u:
			case 4294967280u:
			case 4294967281u:
			case 4294967282u:
			case 4294967283u:
			case 4294967284u:
			case 4294967285u:
			case 4294967286u:
			case 4294967287u:
			case 4294967288u:
			case 4294967289u:
			case 4294967290u:
			case 4294967291u:
			case 4294967292u:
			case 4294967293u:
			case 4294967294u:
			case uint.MaxValue:
				switch (num)
				{
				case 0u:
				case 1u:
				case 2u:
				case 3u:
				case 4u:
				case 5u:
				case 6u:
				case 7u:
				case 8u:
				case 9u:
				case 10u:
				case 11u:
				case 12u:
				case 13u:
				case 14u:
				case 15u:
				case 16u:
				case 17u:
				case 18u:
				case 19u:
				case 20u:
				case 21u:
				case 22u:
				case 23u:
				case 24u:
				case 25u:
				case 26u:
				case 27u:
				case 28u:
					switch (num)
					{
					case 4294967261u:
						if (!(text == "角色已经存在"))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit(",");
						__BB_OBFUSCATOR_89();
						continue;
					case 113u:
						if (!(text == "com.bilibili.mfgy.product007"))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
						continue;
					}
					break;
				case 85u:
					if (!(text == "com.bilibili.mfgy.product005"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("Battle");
					__BB_OBFUSCATOR_55();
					continue;
				case 4294967260u:
					if (!(text == "RoleServerData.SyncExtensionInfo -> "))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit((UIUnlockSystem.UIUnlockSystemType)7);
					continue;
				}
				break;
			case 4294967183u:
				if (!(text == " "))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
				continue;
			case 4294967231u:
				if (!(text == "uiMainMenu.WaitForLogCheck -> 检查版本错误"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("魔法服务器请求成功");
				continue;
			case 4294967211u:
				if (!(text == "登录展示公告"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("25");
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit("com.bilibili.mfgy.product003");
		}
	}

	private bool __BB_OBFUSCATOR_61(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_121()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.SuitHalloween);
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_131()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	public void SetExtensionInfo(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("FirstOrderTimeStamp: " + first_recharge_time);
		Debug.Log("RechargeAmount: " + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_140()
	{
		if (__BB_OBFUSCATOR_38() && __BB_OBFUSCATOR_51())
		{
			return __BB_OBFUSCATOR_34();
		}
		return true;
	}

	public void __BB_OBFUSCATOR_66(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("登录成功:测试用户" + first_recharge_time);
		Debug.Log("没有更新的" + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	public static bool __BB_OBFUSCATOR_97(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	public void __BB_OBFUSCATOR_109(Action EJIKDDEMHNH = null)
	{
		IEnumerator extensionAccountInfo = ServerAPI.GetExtensionAccountInfo(delegate(Response<ServerAPI.ExtensionAccountInfo> response)
		{
			if (response.code == StatusCode.Success)
			{
				SetExtensionInfo(response.data);
				EJIKDDEMHNH?.Invoke();
			}
			else
			{
				Debug.LogError($"RoleServerData.SyncExtensionInfo -> {response.code}");
			}
		}, delegate(UnityWebRequest err)
		{
			Debug.LogError("RoleServerData.SyncExtensionInfo -> " + err.error);
		});
		GameMgr.Inst.StartCoroutine(extensionAccountInfo);
	}

	public bool __BB_OBFUSCATOR_30()
	{
		return true;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_96()
	{
		if (!__BB_OBFUSCATOR_27() && !__BB_OBFUSCATOR_92())
		{
			return BHEHHIFGJOE;
		}
		return true;
	}

	public static bool __BB_OBFUSCATOR_115(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().Any(HaveItemConsiderOverride);
	}

	public bool __BB_OBFUSCATOR_75()
	{
		return true;
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_4()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	private bool __BB_OBFUSCATOR_106(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Any((string x) => ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == x).itemContains.Contains(HLKIMJAGGOK));
	}

	public static bool __BB_OBFUSCATOR_93(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	private bool __BB_OBFUSCATOR_13(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	private bool __BB_OBFUSCATOR_100(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_5()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.EndlessDlc);
	}

	public static bool __BB_OBFUSCATOR_59(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().Any(__BB_OBFUSCATOR_78);
	}

	public void AddItem(string[] BMDEJICFJOC)
	{
		foreach (string text in BMDEJICFJOC)
		{
			if (DKKDIBDGPMD.Add(text))
			{
				Debug.Log("RoleServerData.AddItem -> 添加新物品" + text);
				EventMgr.RoleItemChange?.Invoke();
			}
		}
	}

	public bool __BB_OBFUSCATOR_145()
	{
		return false;
	}

	public void __BB_OBFUSCATOR_81(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Disabled)
		{
			DKKDIBDGPMD.Add(",");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_88()
	{
		if (!__BB_OBFUSCATOR_65() && !__BB_OBFUSCATOR_5())
		{
			return __BB_OBFUSCATOR_84();
		}
		return false;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_98()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_146().__BB_OBFUSCATOR_62();
			}
			return false;
		case TestItemForceAvailable.Have:
			return false;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return true;
		}
	}

	private static bool HaveItemCheck(TestItemForceAvailable FDGPPMFOLGF, ServerAPI.GameItem IJGKDFEGAMM)
	{
		switch (FDGPPMFOLGF)
		{
		case TestItemForceAvailable.Disabled:
		{
			int value = IJGKDFEGAMM switch
			{
				ServerAPI.GameItem.SuitHalloween => 4100710, 
				ServerAPI.GameItem.SuitSpring => 4293030, 
				ServerAPI.GameItem.EndlessDlc => 66666, 
				ServerAPI.GameItem.Game => 6666, 
				ServerAPI.GameItem.SuitChristmas => 4100711, 
				ServerAPI.GameItem.DaveDlc => 4099830, 
				ServerAPI.GameItem.SuitSummer => 4838370, 
				_ => throw new ArgumentOutOfRangeException("gameItem", IJGKDFEGAMM, null), 
			};
			if (SteamManager.Initialized)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t((uint)value));
			}
			return false;
		}
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return false;
		}
	}

	public bool __BB_OBFUSCATOR_21()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return true;
		}
		return __BB_OBFUSCATOR_8(ServerAPI.GameItem.Game);
	}

	private bool __BB_OBFUSCATOR_8(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Any((string x) => ServerAPI.DicProducts.Values.First(((string name, string id, ServerAPI.GameItem[] itemContains) value) => value.id == x).itemContains.Contains(HLKIMJAGGOK));
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_19()
	{
		if (__BB_OBFUSCATOR_65() && __BB_OBFUSCATOR_92())
		{
			return __BB_OBFUSCATOR_34();
		}
		return false;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_148()
	{
		if (!FIKDMCBJPCO && !__BB_OBFUSCATOR_92())
		{
			return __BB_OBFUSCATOR_14();
		}
		return false;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_15()
	{
		if (!__BB_OBFUSCATOR_70() && !__BB_OBFUSCATOR_5())
		{
			return __BB_OBFUSCATOR_14();
		}
		return true;
	}

	public static string __BB_OBFUSCATOR_80(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (__BB_OBFUSCATOR_2().DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = __BB_OBFUSCATOR_108().DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "com.bilibili.mfgy.product005";
			}
			return ((float)commodityInfo.current_price / 664f).ToString("uiMainMenu.WaitForLogCheck -> 检查版本错误").TrimEnd(')').TrimEnd('t');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "正在登陆检查", 
			ServerAPI.ProductItem.SuitHalloween => "37", 
			ServerAPI.ProductItem.HalloweenBundle => "角色已创建", 
			ServerAPI.ProductItem.SuitSpring => "\n获取公告失败", 
			ServerAPI.ProductItem.SuitSummer => "-------------当前SeverEnviroment：", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => ",", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "-------------当前SeverEnviroment：", 
			ServerAPI.ProductItem.SuitSummerBundle => "无法购买商品", 
			ServerAPI.ProductItem.SuitDeluxe => "已解锁", 
			ServerAPI.ProductItem.GameDeluxe => "uiMainMenu.WaitForLogCheck -> 当前版本{0},服务器最新版本{1}", 
			_ => "com.bilibili.mfgy.product001", 
		};
	}

	public static bool __BB_OBFUSCATOR_24(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(__BB_OBFUSCATOR_78);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_38()
	{
		return __BB_OBFUSCATOR_83(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.Game);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_139()
	{
		return __BB_OBFUSCATOR_10(ScriptableObjMgr.staticTestCtrller.OverrideHaveDaveDLC, ServerAPI.GameItem.SuitHalloween);
	}

	public bool IsHaveDave()
	{
		return true;
	}

	public void __BB_OBFUSCATOR_33(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_117(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 0; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 4294967103u)
			{
				switch (num)
				{
				case 4294967174u:
					if (!(text == "6"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("25");
					continue;
				case 4294967269u:
					if (!(text == "角色已创建"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("创建角色请求失败");
					continue;
				case 4294967126u:
					if (!(text == "没有更新的"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("com.bilibili.mfgy.product007");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 0u:
				case 1u:
				case 2u:
				case 3u:
				case 4u:
				case 5u:
				case 6u:
				case 7u:
				case 8u:
				case 9u:
				case 10u:
				case 11u:
				case 12u:
				case 13u:
				case 14u:
				case 15u:
				case 16u:
				case 17u:
				case 18u:
				case 19u:
					switch (num)
					{
					case 41u:
						if (!(text == " "))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit("可以开始游戏");
						__BB_OBFUSCATOR_3();
						continue;
					case 4294967225u:
						if (!(text == "https://api.ipify.org"))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit("创建角色");
						continue;
					}
					break;
				case 129u:
					if (!(text == "Battle"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("12");
					__BB_OBFUSCATOR_46();
					continue;
				case 145u:
					if (!(text == " "))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.SpellDisable);
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("gameItem");
		}
	}

	public void SetAllItem(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Have)
		{
			DKKDIBDGPMD.Add("com.bilibili.mfgy.product001");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	public void __BB_OBFUSCATOR_42(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_130(EMICNIMLIHL);
		__BB_OBFUSCATOR_109(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 0; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 4294967150u)
			{
				switch (num)
				{
				case 4294967138u:
					if (!(text == "com.bilibili.mfgy.product003"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("请求结束");
					continue;
				case 4294967224u:
					if (!(text == "com.bilibili.mfgy.product004"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("com.bilibili.mfgy.product002");
					continue;
				case 2u:
					if (!(text == " "))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("激活码兑换成功");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 4294967259u:
				case 4294967260u:
				case 4294967261u:
				case 4294967262u:
				case 4294967263u:
				case 4294967264u:
				case 4294967265u:
				case 4294967266u:
				case 4294967267u:
				case 4294967268u:
				case 4294967269u:
				case 4294967270u:
				case 4294967271u:
				case 4294967272u:
				case 4294967273u:
				case 4294967274u:
				case 4294967275u:
				case 4294967276u:
				case 4294967277u:
				case 4294967278u:
				case 4294967279u:
				case 4294967280u:
				case 4294967281u:
				case 4294967282u:
				case 4294967283u:
				case 4294967284u:
				case 4294967285u:
				case 4294967286u:
				case 4294967287u:
				case 4294967288u:
				case 4294967289u:
				case 4294967290u:
				case 4294967291u:
				case 4294967292u:
				case 4294967293u:
				case 4294967294u:
				case uint.MaxValue:
					switch (num)
					{
					case 4294967260u:
						if (!(text == "可以开始游戏"))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit(" ");
						OnPurchaseGame();
						continue;
					case 26u:
						if (!(text == "RoleServerData.SyncExtensionInfo -> {0}"))
						{
							break;
						}
						GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.TrainingRoom);
						continue;
					}
					break;
				case 125u:
					if (!(text == "创建角色成功"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("服务器正在维护");
					__BB_OBFUSCATOR_3();
					continue;
				case 4294967187u:
					if (!(text == "检查登录失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("登录展示公告");
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
		}
		static void OnPurchaseGame()
		{
			if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
			{
				GameUISingletonMono<UIFullGame>.Inst.Hide();
			}
			GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
			{
				if (DataMgr.selectedWorldData.InBuyGameRoom)
				{
					GameMgr.Inst.RecycleAllPool();
					SceneManager.LoadScene("Battle");
				}
			};
		}
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_7()
	{
		return __BB_OBFUSCATOR_147(ScriptableObjMgr.staticTestCtrller.OverrideHaveEndlessDLC, ServerAPI.GameItem.SuitSummer);
	}

	public void __BB_OBFUSCATOR_91(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_130(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 4294967282u:
			case 4294967283u:
			case 4294967284u:
			case 4294967285u:
			case 4294967286u:
			case 4294967287u:
			case 4294967288u:
			case 4294967289u:
			case 4294967290u:
			case 4294967291u:
			case 4294967292u:
			case 4294967293u:
			case 4294967294u:
			case uint.MaxValue:
				if (num <= 4294967189u)
				{
					switch (num)
					{
					case 46u:
						if (!(text == "登录失败:账号被禁"))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit("");
						__BB_OBFUSCATOR_3();
						continue;
					case 4294967219u:
						if (!(text == "FirstOrderTimeStamp: "))
						{
							break;
						}
						GameUISingletonMono<UICommonHint>.ShowInit("请求结束");
						continue;
					}
					break;
				}
				switch (num)
				{
				case 25u:
					if (!(text == "服务器连接失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("18");
					__BB_OBFUSCATOR_46();
					continue;
				case 4294967117u:
					if (!(text == "没有更新的"))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.Set);
					continue;
				}
				break;
			case 58u:
				if (!(text == "已解锁"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("31");
				continue;
			case 4294967238u:
				if (!(text == "com.bilibili.mfgy.product003"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("服务器连接失败");
				continue;
			case 4294967258u:
				if (!(text == "激活码不可用"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("订单验证失败");
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit("登录展示公告");
		}
	}

	public bool __BB_OBFUSCATOR_39()
	{
		return true;
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_144()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_70()
	{
		return __BB_OBFUSCATOR_83(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.SuitHalloween);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_25()
	{
		if (!__BB_OBFUSCATOR_65() && !__BB_OBFUSCATOR_51())
		{
			return __BB_OBFUSCATOR_58();
		}
		return false;
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_50()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_4().__BB_OBFUSCATOR_127();
			}
			return true;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return true;
		}
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_119()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	[SpecialName]
	public HashSet<string> __BB_OBFUSCATOR_37()
	{
		return DKKDIBDGPMD.ToHashSet();
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_64()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_90().__BB_OBFUSCATOR_49();
			}
			return true;
		case TestItemForceAvailable.Have:
			return false;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return true;
		}
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_123()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_2().__BB_OBFUSCATOR_49();
			}
			return false;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return false;
		}
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_52()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, ServerAPI.GameItem.DaveDlc);
	}

	public bool __BB_OBFUSCATOR_127()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return false;
		}
		return __BB_OBFUSCATOR_8(ServerAPI.GameItem.SuitHalloween);
	}

	private static bool __BB_OBFUSCATOR_78(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return HLKIMJAGGOK switch
		{
			ServerAPI.GameItem.Game => __BB_OBFUSCATOR_71(), 
			ServerAPI.GameItem.SuitHalloween => __BB_OBFUSCATOR_27(), 
			ServerAPI.GameItem.SuitSpring => ACPKKMJKOJD, 
			ServerAPI.GameItem.SuitSummer => __BB_OBFUSCATOR_34(), 
			_ => true, 
		};
	}

	public void __BB_OBFUSCATOR_29(string[] BMDEJICFJOC)
	{
		DKKDIBDGPMD = BMDEJICFJOC.ToHashSet();
		if (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame == TestItemForceAvailable.Disabled)
		{
			DKKDIBDGPMD.Add("创建角色");
		}
		EventMgr.RoleItemChange?.Invoke();
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_35()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	internal static void __BB_OBFUSCATOR_56()
	{
		if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
		{
			GameUISingletonMono<UIFullGame>.Inst.Hide();
		}
		GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
		{
			if (DataMgr.selectedWorldData.InBuyGameRoom)
			{
				GameMgr.Inst.RecycleAllPool();
				SceneManager.LoadScene("Battle");
			}
		};
	}

	[SpecialName]
	public static ICJNOGPFMAM __BB_OBFUSCATOR_146()
	{
		return CNHGICDNFGC ?? (CNHGICDNFGC = new ICJNOGPFMAM());
	}

	private bool __BB_OBFUSCATOR_74(ServerAPI.ProductItem HLKIMJAGGOK)
	{
		return DKKDIBDGPMD.Contains(ServerAPI.DicProducts[HLKIMJAGGOK].id);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_110()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveSpringDLC, (ServerAPI.GameItem)7);
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_77()
	{
		switch (ScriptableObjMgr.staticTestCtrller.OverrideHaveGame)
		{
		case TestItemForceAvailable.Disabled:
			if (GameMgr.IsMobile_Static)
			{
				return __BB_OBFUSCATOR_104().IsHaveGame();
			}
			return true;
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return true;
		}
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_31()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveHolloweenDLC, ServerAPI.GameItem.SuitHalloween);
	}

	public bool __BB_OBFUSCATOR_73()
	{
		return true;
	}

	public void __BB_OBFUSCATOR_133(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_105(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		foreach (string text in EMICNIMLIHL)
		{
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 4294967220u:
			case 4294967221u:
			case 4294967222u:
			case 4294967223u:
			case 4294967224u:
			case 4294967225u:
			case 4294967226u:
			case 4294967227u:
			case 4294967228u:
			case 4294967229u:
			case 4294967230u:
			case 4294967231u:
			case 4294967232u:
			case 4294967233u:
			case 4294967234u:
			case 4294967235u:
			case 4294967236u:
			case 4294967237u:
			case 4294967238u:
			case 4294967239u:
			case 4294967240u:
			case 4294967241u:
			case 4294967242u:
			case 4294967243u:
			case 4294967244u:
			case 4294967245u:
			case 4294967246u:
			case 4294967247u:
			case 4294967248u:
			case 4294967249u:
			case 4294967250u:
			case 4294967251u:
			case 4294967252u:
			case 4294967253u:
			case 4294967254u:
			case 4294967255u:
			case 4294967256u:
			case 4294967257u:
			case 4294967258u:
			case 4294967259u:
			case 4294967260u:
			case 4294967261u:
			case 4294967262u:
			case 4294967263u:
			case 4294967264u:
			case 4294967265u:
			case 4294967266u:
			case 4294967267u:
			case 4294967268u:
			case 4294967269u:
			case 4294967270u:
			case 4294967271u:
			case 4294967272u:
			case 4294967273u:
			case 4294967274u:
			case 4294967275u:
			case 4294967276u:
			case 4294967277u:
			case 4294967278u:
			case 4294967279u:
			case 4294967280u:
			case 4294967281u:
			case 4294967282u:
			case 4294967283u:
			case 4294967284u:
			case 4294967285u:
			case 4294967286u:
			case 4294967287u:
			case 4294967288u:
			case 4294967289u:
			case 4294967290u:
			case 4294967291u:
			case 4294967292u:
			case 4294967293u:
			case 4294967294u:
			case uint.MaxValue:
				switch (num)
				{
				case 4294967217u:
				case 4294967218u:
				case 4294967219u:
				case 4294967220u:
				case 4294967221u:
				case 4294967222u:
				case 4294967223u:
				case 4294967224u:
				case 4294967225u:
				case 4294967226u:
				case 4294967227u:
				case 4294967228u:
				case 4294967229u:
				case 4294967230u:
				case 4294967231u:
				case 4294967232u:
				case 4294967233u:
				case 4294967234u:
				case 4294967235u:
				case 4294967236u:
				case 4294967237u:
				case 4294967238u:
				case 4294967239u:
				case 4294967240u:
				case 4294967241u:
				case 4294967242u:
				case 4294967243u:
				case 4294967244u:
				case 4294967245u:
				case 4294967246u:
				case 4294967247u:
				case 4294967248u:
				case 4294967249u:
				case 4294967250u:
				case 4294967251u:
				case 4294967252u:
				case 4294967253u:
				case 4294967254u:
				case 4294967255u:
				case 4294967256u:
				case 4294967257u:
				case 4294967258u:
				case 4294967259u:
				case 4294967260u:
				case 4294967261u:
				case 4294967262u:
				case 4294967263u:
				case 4294967264u:
				case 4294967265u:
				case 4294967266u:
				case 4294967267u:
				case 4294967268u:
				case 4294967269u:
				case 4294967270u:
				case 4294967271u:
				case 4294967272u:
				case 4294967273u:
				case 4294967274u:
				case 4294967275u:
				case 4294967276u:
				case 4294967277u:
				case 4294967278u:
				case 4294967279u:
				case 4294967280u:
				case 4294967281u:
				case 4294967282u:
				case 4294967283u:
				case 4294967284u:
				case 4294967285u:
				case 4294967286u:
				case 4294967287u:
				case 4294967288u:
				case 4294967289u:
				case 4294967290u:
				case 4294967291u:
				case 4294967292u:
				case 4294967293u:
				case 4294967294u:
					if (num != 142 || !(text == "com.bilibili.mfgy.product001"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("检查登录失败");
					__BB_OBFUSCATOR_46();
					continue;
				case 4294967140u:
					if (!(text == "检查登录失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("6");
					__BB_OBFUSCATOR_56();
					continue;
				case uint.MaxValue:
					if (!(text == ","))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.Research);
					continue;
				case 174u:
					if (!(text == "F2"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("");
					continue;
				}
				break;
			case 99u:
				if (!(text == " "))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("获取服务器价格表失败,将显示默认价格");
				continue;
			case 25u:
				if (!(text == "已解锁"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("激活码兑换成功");
				continue;
			case 4294967202u:
				if (!(text == "检查登录状态"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("正在登陆检查");
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit("");
		}
	}

	public void __BB_OBFUSCATOR_72(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("" + first_recharge_time);
		Debug.Log("uiMainMenu.WaitForLogCheck -> 检查更新" + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	public void __BB_OBFUSCATOR_54(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_117(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i++)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 4294967122u)
			{
				switch (num)
				{
				case 158u:
					if (!(text == "Battle"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("");
					continue;
				case 108u:
					if (!(text == ","))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("31");
					continue;
				case 164u:
					if (!(text == ","))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("\n获取公告失败");
					continue;
				}
			}
			else if (num <= 166)
			{
				switch (num)
				{
				case 3u:
					if (!(text == "获取服务器价格表失败,将显示默认价格"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("ServerMgr.SyncServerEnvironment -> 请求错误 ");
					__BB_OBFUSCATOR_46();
					continue;
				case 4294967164u:
					if (!(text == "com.bilibili.mfgy.product002"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 175u:
					if (!(text == "com.bilibili.mfgy.product005"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("com.bilibili.mfgy.product005");
					__BB_OBFUSCATOR_56();
					continue;
				case 4294967236u:
					if (!(text == "-------------当前SeverEnviroment："))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.UnlockDLC1);
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("6");
		}
	}

	public static string __BB_OBFUSCATOR_101(ServerAPI.ProductItem IHNDODPEMBF)
	{
		if (__BB_OBFUSCATOR_2().DGNJLGDEMAP != null)
		{
			ServerAPI.CommodityInfo commodityInfo = KEMAJLGHMEL.DGNJLGDEMAP.FirstOrDefault((ServerAPI.CommodityInfo x) => x.item == ServerAPI.DicProducts[IHNDODPEMBF].id);
			if (commodityInfo == null)
			{
				return "检查服务器状态";
			}
			return ((float)commodityInfo.current_price / 903f).ToString("-------------").TrimEnd('&').TrimEnd('ﾌ');
		}
		return IHNDODPEMBF switch
		{
			ServerAPI.ProductItem.Game => "31", 
			ServerAPI.ProductItem.SuitHalloween => "FirstOrderTimeStamp: ", 
			ServerAPI.ProductItem.HalloweenBundle => "登录失败:账号被禁", 
			ServerAPI.ProductItem.SuitSpring => "没有更新的", 
			ServerAPI.ProductItem.SuitSummer => "com.bilibili.mfgy.product002", 
			ServerAPI.ProductItem.SuitDeluxeWithoutSummerDlc => "F2", 
			ServerAPI.ProductItem.GameDeluxeWithoutSummerDlc => "F2", 
			ServerAPI.ProductItem.SuitSummerBundle => "显示维护公告", 
			ServerAPI.ProductItem.SuitDeluxe => " ", 
			ServerAPI.ProductItem.GameDeluxe => "已解锁", 
			_ => "登录成功:控制台开启", 
		};
	}

	[SpecialName]
	public static bool __BB_OBFUSCATOR_43()
	{
		return HaveItemCheck(ScriptableObjMgr.staticTestCtrller.OverrideHaveEndlessDLC, ServerAPI.GameItem.Game);
	}

	private static bool __BB_OBFUSCATOR_17(TestItemForceAvailable FDGPPMFOLGF, ServerAPI.GameItem IJGKDFEGAMM)
	{
		switch (FDGPPMFOLGF)
		{
		case TestItemForceAvailable.Disabled:
		{
			int value = IJGKDFEGAMM switch
			{
				ServerAPI.GameItem.SuitHalloween => -147, 
				ServerAPI.GameItem.SuitSpring => 160, 
				ServerAPI.GameItem.EndlessDlc => 172, 
				ServerAPI.GameItem.Game => 138, 
				ServerAPI.GameItem.SuitChristmas => 67, 
				ServerAPI.GameItem.DaveDlc => -100, 
				ServerAPI.GameItem.SuitSummer => -48, 
				_ => throw new ArgumentOutOfRangeException("创建角色请求失败", IJGKDFEGAMM, null), 
			};
			if (SteamManager.Initialized)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t((uint)value));
			}
			return true;
		}
		case TestItemForceAvailable.Have:
			return false;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return false;
		}
	}

	public static bool __BB_OBFUSCATOR_18(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	public void __BB_OBFUSCATOR_102(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_36(EMICNIMLIHL);
		__BB_OBFUSCATOR_109(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			if (num <= 4294967173u)
			{
				switch (num)
				{
				case 4294967157u:
					if (!(text == "uiMainMenu.WaitForLogCheck -> 检查版本错误"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("检查登录失败");
					continue;
				case 4294967284u:
					if (!(text == "账号被禁用"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("请求结束");
					continue;
				case 4294967114u:
					if (!(text == "12"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit(",");
					continue;
				}
			}
			else if (num <= 182)
			{
				switch (num)
				{
				case 4294967148u:
					if (!(text == "获取服务器价格表失败,将显示默认价格"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("激活码兑换成功");
					OnPurchaseGame();
					continue;
				case 4294967112u:
					if (!(text == "服务器连接失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("\n获取公告失败");
					continue;
				}
			}
			else
			{
				switch (num)
				{
				case 4294967176u:
					if (!(text == "6"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("获取服务器价格表失败,将显示默认价格");
					__BB_OBFUSCATOR_56();
					continue;
				case 4294967235u:
					if (!(text == "FirstOrderTimeStamp: "))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.TrainingRoom);
					continue;
				}
			}
			GameUISingletonMono<UICommonHint>.ShowInit("服务器连接失败");
		}
		static void OnPurchaseGame()
		{
			if (GameUISingletonMono<UIFullGame>.StaticIsOpen)
			{
				GameUISingletonMono<UIFullGame>.Inst.Hide();
			}
			GameUISingletonMono<UICommonHint>.Inst.ActionOnClose = delegate
			{
				if (DataMgr.selectedWorldData.InBuyGameRoom)
				{
					GameMgr.Inst.RecycleAllPool();
					SceneManager.LoadScene("Battle");
				}
			};
		}
	}

	private static bool __BB_OBFUSCATOR_10(TestItemForceAvailable FDGPPMFOLGF, ServerAPI.GameItem IJGKDFEGAMM)
	{
		switch (FDGPPMFOLGF)
		{
		case TestItemForceAvailable.Disabled:
		{
			int value = IJGKDFEGAMM switch
			{
				ServerAPI.GameItem.SuitHalloween => 193, 
				ServerAPI.GameItem.SuitSpring => -152, 
				ServerAPI.GameItem.EndlessDlc => 126, 
				ServerAPI.GameItem.Game => 84, 
				ServerAPI.GameItem.SuitChristmas => -79, 
				ServerAPI.GameItem.DaveDlc => 77, 
				ServerAPI.GameItem.SuitSummer => -139, 
				_ => throw new ArgumentOutOfRangeException("已解锁", IJGKDFEGAMM, null), 
			};
			if (SteamManager.Initialized)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t((uint)value));
			}
			return false;
		}
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return false;
		default:
			return false;
		}
	}

	public void __BB_OBFUSCATOR_44(string[] EMICNIMLIHL)
	{
		__BB_OBFUSCATOR_117(EMICNIMLIHL);
		__BB_OBFUSCATOR_128(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		for (int i = 1; i < EMICNIMLIHL.Length; i += 0)
		{
			string text = EMICNIMLIHL[i];
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 0u:
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
			case 16u:
			case 17u:
			case 18u:
			case 19u:
			case 20u:
			case 21u:
			case 22u:
			case 23u:
			case 24u:
			case 25u:
			case 26u:
			case 27u:
			case 28u:
			case 29u:
			case 30u:
			case 31u:
			case 32u:
			case 33u:
			case 34u:
			case 35u:
			case 36u:
			case 37u:
			case 38u:
			case 39u:
			case 40u:
			case 41u:
			case 42u:
			case 43u:
			case 44u:
			case 45u:
			case 46u:
			case 47u:
			case 48u:
			case 49u:
			case 50u:
			case 51u:
				switch (num)
				{
				case 111u:
					if (!(text == "登录成功:测试用户"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("F2");
					continue;
				case 4294967219u:
					if (!(text == "显示维护公告"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("Battle");
					continue;
				case 100u:
					if (!(text == "获取更新公告失败"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
					continue;
				}
				break;
			case 52u:
			case 53u:
			case 54u:
			case 55u:
			case 56u:
			case 57u:
			case 58u:
			case 59u:
			case 60u:
			case 61u:
			case 62u:
			case 63u:
			case 64u:
			case 65u:
			case 66u:
			case 67u:
			case 68u:
			case 69u:
			case 70u:
			case 71u:
			case 72u:
			case 73u:
			case 74u:
			case 75u:
			case 76u:
			case 77u:
			case 78u:
			case 79u:
			case 80u:
			case 81u:
			case 82u:
			case 83u:
			case 84u:
			case 85u:
			case 86u:
			case 87u:
			case 88u:
			case 89u:
			case 90u:
			case 91u:
			case 92u:
			case 93u:
			case 94u:
			case 95u:
			case 96u:
			case 97u:
			case 98u:
			case 99u:
			case 100u:
			case 101u:
			case 102u:
			case 103u:
			case 104u:
			case 105u:
			case 106u:
			case 107u:
			case 108u:
				switch (num)
				{
				case 4294967239u:
					if (!(text == "18"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("登录展示公告");
					__BB_OBFUSCATOR_89();
					continue;
				case 4294967185u:
					if (!(text == "激活码兑换成功"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("uiMainMenu.WaitForLogCheck -> 检查版本错误");
					continue;
				}
				break;
			case 185u:
				if (!(text == "登陆失败"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("RoleServerData.SyncExtensionInfo -> {0}");
				__BB_OBFUSCATOR_56();
				continue;
			case 4294967183u:
				if (!(text == "显示维护公告"))
				{
					break;
				}
				GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.ActivateGirl);
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit("37");
		}
	}

	private static bool HaveItemConsiderOverride(ServerAPI.GameItem HLKIMJAGGOK)
	{
		return HLKIMJAGGOK switch
		{
			ServerAPI.GameItem.Game => MIFJADDOODN, 
			ServerAPI.GameItem.SuitHalloween => FIKDMCBJPCO, 
			ServerAPI.GameItem.SuitSpring => ACPKKMJKOJD, 
			ServerAPI.GameItem.SuitSummer => BHEHHIFGJOE, 
			_ => false, 
		};
	}

	private static bool __BB_OBFUSCATOR_83(TestItemForceAvailable FDGPPMFOLGF, ServerAPI.GameItem IJGKDFEGAMM)
	{
		switch (FDGPPMFOLGF)
		{
		case TestItemForceAvailable.Disabled:
		{
			int value = IJGKDFEGAMM switch
			{
				ServerAPI.GameItem.SuitHalloween => -172, 
				ServerAPI.GameItem.SuitSpring => -192, 
				ServerAPI.GameItem.EndlessDlc => -54, 
				ServerAPI.GameItem.Game => 192, 
				ServerAPI.GameItem.SuitChristmas => 12, 
				ServerAPI.GameItem.DaveDlc => -42, 
				ServerAPI.GameItem.SuitSummer => -49, 
				_ => throw new ArgumentOutOfRangeException("创建角色成功", IJGKDFEGAMM, null), 
			};
			if (SteamManager.Initialized)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t((uint)value));
			}
			return true;
		}
		case TestItemForceAvailable.Have:
			return true;
		case TestItemForceAvailable.DontHave:
			return true;
		default:
			return true;
		}
	}

	public void __BB_OBFUSCATOR_128(Action EJIKDDEMHNH = null)
	{
		IEnumerator extensionAccountInfo = ServerAPI.GetExtensionAccountInfo(delegate(Response<ServerAPI.ExtensionAccountInfo> response)
		{
			if (response.code == StatusCode.Success)
			{
				SetExtensionInfo(response.data);
				EJIKDDEMHNH?.Invoke();
			}
			else
			{
				Debug.LogError($"RoleServerData.SyncExtensionInfo -> {response.code}");
			}
		}, delegate(UnityWebRequest err)
		{
			Debug.LogError("RoleServerData.SyncExtensionInfo -> " + err.error);
		});
		GameMgr.Inst.StartCoroutine(extensionAccountInfo);
	}

	public void __BB_OBFUSCATOR_135(ServerAPI.ExtensionAccountInfo EPNFNLMGANN)
	{
		int? first_recharge_time = EPNFNLMGANN.first_recharge_time;
		Debug.Log("," + first_recharge_time);
		Debug.Log("登录失败:账号被禁" + EPNFNLMGANN.recharge_amount);
		CMIKFLKFNPD = EPNFNLMGANN.create_role_time;
		JKBPGJFFJNN = EPNFNLMGANN.first_recharge_time;
		CNKAOEAMNCC = EPNFNLMGANN.recharge_amount;
	}

	public void __BB_OBFUSCATOR_40(string[] EMICNIMLIHL)
	{
		AddItem(EMICNIMLIHL);
		SyncExtensionInfo(delegate
		{
			PluginActivity.Inst.UploadUserSnapshot(2);
		});
		foreach (string text in EMICNIMLIHL)
		{
			uint num = _003CPrivateImplementationDetails_003E.ComputeStringHash(text);
			switch (num)
			{
			case 4294967258u:
			case 4294967259u:
			case 4294967260u:
			case 4294967261u:
			case 4294967262u:
			case 4294967263u:
			case 4294967264u:
			case 4294967265u:
			case 4294967266u:
			case 4294967267u:
			case 4294967268u:
			case 4294967269u:
			case 4294967270u:
			case 4294967271u:
			case 4294967272u:
			case 4294967273u:
			case 4294967274u:
			case 4294967275u:
			case 4294967276u:
			case 4294967277u:
			case 4294967278u:
			case 4294967279u:
			case 4294967280u:
			case 4294967281u:
			case 4294967282u:
			case 4294967283u:
			case 4294967284u:
			case 4294967285u:
			case 4294967286u:
			case 4294967287u:
			case 4294967288u:
			case 4294967289u:
			case 4294967290u:
			case 4294967291u:
			case 4294967292u:
			case 4294967293u:
			case 4294967294u:
			case uint.MaxValue:
				switch (num)
				{
				case 4294967099u:
					if (!(text == "已解锁"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("登录成功:控制台开启");
					__BB_OBFUSCATOR_55();
					continue;
				case 4294967262u:
					if (!(text == "12"))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit(" ");
					__BB_OBFUSCATOR_89();
					continue;
				case 4294967246u:
					if (!(text == "31"))
					{
						break;
					}
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.ActivateGirl);
					continue;
				case 130u:
					if (!(text == "RoleServerData.SyncExtensionInfo -> "))
					{
						break;
					}
					GameUISingletonMono<UICommonHint>.ShowInit("已解锁");
					continue;
				}
				break;
			case 4294967105u:
				if (!(text == "显示维护公告"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("6");
				continue;
			case 4294967180u:
				if (!(text == "获取公告失败"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("创建角色成功");
				continue;
			case 137u:
				if (!(text == "服务器连接失败"))
				{
					break;
				}
				GameUISingletonMono<UICommonHint>.ShowInit("uiMainMenu.WaitForLogCheck -> 当前版本{0},服务器最新版本{1}");
				continue;
			}
			GameUISingletonMono<UICommonHint>.ShowInit(",");
		}
	}

	public static bool CanBuyItem(ServerAPI.ProductItem IHNDODPEMBF)
	{
		return !ServerAPI.DicProducts[IHNDODPEMBF].itemContains.ToList().All(HaveItemConsiderOverride);
	}

	public void __BB_OBFUSCATOR_130(string[] BMDEJICFJOC)
	{
		for (int i = 0; i < BMDEJICFJOC.Length; i += 0)
		{
			string text = BMDEJICFJOC[i];
			if (DKKDIBDGPMD.Add(text))
			{
				Debug.Log("," + text);
				EventMgr.RoleItemChange?.Invoke();
			}
		}
	}
}
