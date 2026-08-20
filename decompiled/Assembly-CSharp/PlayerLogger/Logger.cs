using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace PlayerLogger;

public static class Logger
{
	[DllImport("bds-sdk-pc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDKInitBDS")]
	private static extern int SDKInitBDSPtr(string strGameId, string strChannelId, string strENV);

	[DllImport("bds-sdk-pc", CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDKReportCustomEvent(string strEventName, string strPlayerInfo, string strExtension, string strCpParam);

	private static bool EnsureInit()
	{
		return false;
	}

	public static void ReportForce(EventModel data)
	{
		if (!Application.isMobilePlatform)
		{
			string text = JsonConvert.SerializeObject(data, new StringEnumConverter());
			string eventName = data.GetEventName();
			if (ScriptableObjMgr.Inst.testCtrller.CommandLine)
			{
				LocalLog(eventName, text);
			}
			if (EnsureInit())
			{
				string strPlayerInfo = JsonConvert.SerializeObject(new PlayerInfo(), new StringEnumConverter());
				SDKReportCustomEvent(eventName, strPlayerInfo, "{}", text);
			}
		}
	}

	private static void LocalLog(string eventName, string json)
	{
		string text = Path.Combine(Application.persistentDataPath, "logs");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = Path.Combine(text, eventName + ".jsonlines");
		json = json.Insert(1, "\"report_time\":\"" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\",");
		File.AppendAllText(path, json + "\n");
	}
}
