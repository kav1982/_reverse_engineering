using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Networking;

namespace GameServer;

public static class ServerAPI
{
	public class CheckAIHelpTokenResultData
	{
		public string token;
	}

	public class CheckVerifyResultData
	{
		public BiliStatusCode biliStatus;

		public bool tester;

		public bool dev;

		public bool ban;

		public bool created_role;

		public string[] items;

		public ExtensionAccountInfo extension_info;
	}

	public class ExtensionAccountInfo
	{
		public int? create_role_time;

		public int? first_recharge_time;

		public int recharge_amount;
	}

	public class CheckHarmoniousResult
	{
		public bool harmonious;
	}

	public class GameVersion : IComparable<GameVersion>
	{
		public int version;

		public bool force;

		public string notice;

		public DateTime created;

		public int CompareTo(GameVersion other)
		{
			return version.CompareTo(other.version);
		}
	}

	public class Notice
	{
		public string rich_text;

		public string title;

		public string id = "";
	}

	public class Redirection
	{
		public bool redirection;

		[CanBeNull]
		public string[] servers;
	}

	public class CommodityInfo
	{
		public string item;

		public int source_price;

		public int current_price;
	}

	public class OpenOrderInfo
	{
		public string out_order_no;

		public int money;

		public string order_sign;
	}

	public enum OrderStatus
	{
		Processing,
		Success
	}

	public class CheckOrderResult
	{
		public OrderStatus state;
	}

	public enum ProductItem
	{
		Game,
		SuitHalloween,
		HalloweenBundle,
		SuitSpring,
		SuitDeluxeWithoutSummerDlc,
		GameDeluxeWithoutSummerDlc,
		SuitSummer,
		SuitSummerBundle,
		SuitDeluxe,
		GameDeluxe,
		EndlessDlc,
		EndlessBundle
	}

	public enum GameItem
	{
		Game,
		SuitHalloween,
		SuitSpring,
		SuitChristmas,
		EndlessDlc,
		DaveDlc,
		SuitSummer
	}

	public static readonly Dictionary<ProductItem, (string name, string id, GameItem[] itemContains)> DicProducts = new Dictionary<ProductItem, (string, string, GameItem[])>
	{
		{
			ProductItem.Game,
			("魔法工艺本体", "com.bilibili.mfgy.product001", new GameItem[1])
		},
		{
			ProductItem.SuitHalloween,
			("万圣派对", "com.bilibili.mfgy.product002", new GameItem[1] { GameItem.SuitHalloween })
		},
		{
			ProductItem.HalloweenBundle,
			("魔法万圣夜", "com.bilibili.mfgy.product003", new GameItem[2]
			{
				GameItem.Game,
				GameItem.SuitHalloween
			})
		},
		{
			ProductItem.SuitSpring,
			("马年新春", "com.bilibili.mfgy.product004", new GameItem[1] { GameItem.SuitSpring })
		},
		{
			ProductItem.SuitSummer,
			("清凉夏日", "com.bilibili.mfgy.product007", new GameItem[1] { GameItem.SuitSummer })
		},
		{
			ProductItem.SuitDeluxeWithoutSummerDlc,
			("节日套装", "com.bilibili.mfgy.product005", new GameItem[2]
			{
				GameItem.SuitHalloween,
				GameItem.SuitSpring
			})
		},
		{
			ProductItem.GameDeluxeWithoutSummerDlc,
			("全收录聚合包", "com.bilibili.mfgy.product006", new GameItem[3]
			{
				GameItem.Game,
				GameItem.SuitHalloween,
				GameItem.SuitSpring
			})
		},
		{
			ProductItem.SuitSummerBundle,
			("清凉夏日包", "com.bilibili.mfgy.product008", new GameItem[2]
			{
				GameItem.Game,
				GameItem.SuitHalloween
			})
		},
		{
			ProductItem.SuitDeluxe,
			("节日套装", "com.bilibili.mfgy.product009", new GameItem[3]
			{
				GameItem.SuitHalloween,
				GameItem.SuitSpring,
				GameItem.SuitSummer
			})
		},
		{
			ProductItem.GameDeluxe,
			("全收录聚合包", "com.bilibili.mfgy.product010", new GameItem[4]
			{
				GameItem.Game,
				GameItem.SuitHalloween,
				GameItem.SuitSpring,
				GameItem.SuitSummer
			})
		}
	};

	public const string purchaseItemFullGame = "魔法工艺本体";

	public const string purchaseItemDlc = "万圣派对";

	public const string purchaseItemDeluxeVersion = "魔法万圣夜";

	public const string purchaseItemSuitSummer = "清凉夏日";

	public const string purchaseItemSuitSpring = "马年新春";

	public const string purchaseItemSuitDeluxeWithoutSummerDlc = "节日套装";

	public const string purchaseItemGameDeluxeWithoutSummerDlc = "全收录聚合包";

	public const string purchaseItemSummerBundle = "清凉夏日包";

	public const string purchaseItemSuitDeluxe = "节日套装";

	public const string purchaseItemGameDeluxe = "全收录聚合包";

	public const string purchaseItemEndless = "无尽模式占位";

	public const string purchaseItemEndlessBundle = "无尽模式+本体占位";

	public const string ItemID_Game = "com.bilibili.mfgy.product001";

	public const string ItemID_SuitHalloween = "com.bilibili.mfgy.product002";

	public const string ItemID_HalloweenBundle = "com.bilibili.mfgy.product003";

	public const string ItemID_SuitSpring = "com.bilibili.mfgy.product004";

	public const string ItemID_SuitDeluxe_NoSummerDlc = "com.bilibili.mfgy.product005";

	public const string ItemID_GameDeluxe_NoSummerDlc = "com.bilibili.mfgy.product006";

	public const string ItemID_SuitSummer = "com.bilibili.mfgy.product007";

	public const string ItemID_SuitSummerBundle = "com.bilibili.mfgy.product008";

	public const string ItemID_SuitDeluxe = "com.bilibili.mfgy.product009";

	public const string ItemID_GameDeluxe = "com.bilibili.mfgy.product010";

	public const string ItemID_DaveDLC = "com.bilibili.mfgy.product00x";

	public const string ItemID_SuitChristmas = "com.bilibili.mfgy.product00xx";

	public const string ItemID_EndlessDLC = "com.bilibili.mfgy.product00xxx";

	public const string ItemID_EndlessBundle = "com.bilibili.mfgy.product00xxxx";

	public static IEnumerator StartGetAIHelpToken(Action<Response> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("aihelp/token", "").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckAIHelpToken(Action<Response<CheckAIHelpTokenResultData>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("aihelp/token").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator StartVerify(Action<Response> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/verify", "").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckVerifyResult(Action<Response<CheckVerifyResultData>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/verify", new Dictionary<string, object> { 
		{
			"uid",
			ClientSettings.Uid
		} }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CreateRole(Action<Response> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/create-role", "").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator UseCDKey(string cdkey, Action<Response<string[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/use-cdkey", "", new Dictionary<string, object> { { "cdkey_code", cdkey } }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetItems(Action<Response<string[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/get-items").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetExtensionAccountInfo(Action<Response<ExtensionAccountInfo>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("account/extension_info").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckHarmonious(Action<Response<CheckHarmoniousResult>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("game/h").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckNewVersion(int currentVersion, Action<Response<GameVersion[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("game/check-new-version", new Dictionary<string, object> { { "client_version", currentVersion } }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetNoticeById(string noticeId, Action<Response<Notice>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("game/notice", new Dictionary<string, object> { { "notice_id", noticeId } }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetNoticeByVersion(int version, Action<Response<Notice>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("game/notice", new Dictionary<string, object> { { "game_version", version } }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetDirectDisplayNotices(Action<Response<Notice[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("game/direct_display_notices").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetServerName(Action<Response<string>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("server/name").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetServerTags(Action<Response<string[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("server/tags").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetMaintainNotice(Action<Response<Notice>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("server/maintain-notice").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckRedirection(int gameVersion, Action<Response<Redirection>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("server/redirection", new Dictionary<string, object> { { "version", gameVersion } }).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator GetAllCommodity(Action<Response<CommodityInfo[]>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("store/commodity").SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator OpenOrder(string commodity, string notifyUrl, Action<Response<OpenOrderInfo>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("store/order", "", new Dictionary<string, object>
		{
			{ "commodity_id", commodity },
			{ "notify_url", notifyUrl }
		}).SendAndPlayback(callback, errorCallback);
	}

	public static IEnumerator CheckOrderState(string order, Action<Response<CheckOrderResult>> callback, Action<UnityWebRequest> errorCallback)
	{
		yield return new Request("store/order-check-status", new Dictionary<string, object> { { "order_no", order } }).SendAndPlayback(callback, errorCallback);
	}
}
