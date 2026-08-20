using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public static class ProgramInfo
{
	public enum SteamDLLState
	{
		Safe,
		NotFound,
		Piracy
	}

	private static readonly string CheckDLLPathR = "lld.46ipa_maets\\46_68x\\snigulP\\ataD_tfarcigaM\\";

	private static readonly string TargetHash = "\u00b8R\u00b4°V\\LN\u00b4²^°ZV\u00b4®XZN°R^LTN¶P¶\\°²\u00b8";

	private static readonly string[] MD5CheckFiles = new string[2] { "\\Magicraft_Data\\Managed\\Assembly-CSharp.dll", "\\Magicraft.exe" };

	public static bool CheckBepInEx()
	{
		try
		{
			return (from name in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory, "*").Concat(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*")).Select(Path.GetFileName)
				select name.ToLower()).Any((string nameLower) => nameLower == "bepinex" || nameLower == "winhttp.dll");
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static string MakeProgramInfo()
	{
		List<string> list = new List<string>();
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		list.Add("======================================================================");
		list.Add("Launch Time: " + DateTime.Now.ToString("yyyy-M-d HH:mm:ss"));
		list.Add("OS: " + SystemInfo.operatingSystem);
		list.Add($"Device: {SystemInfo.deviceType}");
		list.Add("DeviceModel: " + SystemInfo.deviceModel);
		list.Add("Current Directory: " + Environment.CurrentDirectory);
		list.Add("Domain Base Directory: " + AppDomain.CurrentDomain.BaseDirectory);
		list.AddRange(GetGameDirFilesInfo());
		list.AddRange(GetMainFileMD5CheckInfo());
		stopwatch.Stop();
		list.Add("Check time cost: " + (float)stopwatch.ElapsedMilliseconds / 1000f);
		list.Add("======================================================================");
		return string.Join("\n", list);
	}

	private static string[] GetGameDirFilesInfo()
	{
		try
		{
			List<string> list = new List<string>();
			list.Add("Files in game directory: ");
			list.AddRange(from e in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory, "*")
				select "\t - " + e + "\\");
			list.AddRange(from e in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*")
				select "\t - " + e);
			return list.ToArray();
		}
		catch (Exception ex)
		{
			return new string[1] { "GET FILES INFO ERROR: " + ex.Message };
		}
	}

	private static string[] GetMainFileMD5CheckInfo()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		List<string> list = new List<string> { "MD5 Check:" };
		string[] mD5CheckFiles = MD5CheckFiles;
		foreach (string text in mD5CheckFiles)
		{
			try
			{
				string fileMD = GetFileMD5(baseDirectory + text);
				list.Add("\t - " + text + " >>> " + fileMD);
			}
			catch (Exception ex)
			{
				list.Add("\t - " + text + " >>> Error: " + ex.Message);
			}
		}
		return list.ToArray();
	}

	private static string GetFileMD5(string filePath)
	{
		byte[] buffer = File.ReadAllBytes(filePath);
		byte[] source = new MD5CryptoServiceProvider().ComputeHash(buffer);
		return string.Join("", source.Select((byte e) => e.ToString("x2")));
	}

	public static SteamDLLState CheckSteamDLL()
	{
		try
		{
			string text = string.Join("", CheckDLLPathR.Reverse());
			UnityEngine.Debug.Log(text);
			string fileMD = GetFileMD5(AppDomain.CurrentDomain.BaseDirectory + text);
			UnityEngine.Debug.Log(fileMD);
			string text2 = fileMD.ToLower().Aggregate("", (string current, char t) => current + (char)(t * 2 - 20));
			UnityEngine.Debug.Log(text2);
			return (!(text2 == TargetHash)) ? SteamDLLState.Piracy : SteamDLLState.Safe;
		}
		catch (Exception)
		{
			UnityEngine.Debug.Log("NOT FOUND");
			return SteamDLLState.NotFound;
		}
	}
}
