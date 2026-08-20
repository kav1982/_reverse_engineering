using System;
using UnityEngine;

public static class ABResources
{
	public static void Init()
	{
		AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
	}

	public static bool Exists(string name)
	{
		return Resources.Load(name);
	}

	public static T LoadAsset<T>(string name) where T : UnityEngine.Object
	{
		return Resources.Load<T>(name);
	}

	public static T LoadHarmonizableAsset<T>(string name, string nameH) where T : UnityEngine.Object
	{
		if (GameMgr.IsHarmony_Static)
		{
			return LoadAsset<T>(nameH) ?? LoadAsset<T>(name);
		}
		return LoadAsset<T>(name);
	}

	public static T LoadHarmonizableAssetAge16<T>(string name, string nameH) where T : UnityEngine.Object
	{
		if (GameMgr.IsChAge14_Static)
		{
			return LoadAsset<T>(nameH) ?? LoadAsset<T>(name);
		}
		return LoadAsset<T>(name);
	}

	public static T LoadPlatformAsset<T>(string name, string nameMobile) where T : UnityEngine.Object
	{
		if (GameMgr.IsMobile_Static)
		{
			return LoadAsset<T>(nameMobile) ?? LoadAsset<T>(name);
		}
		return LoadAsset<T>(name);
	}

	public static string GetFullInABPath(string path)
	{
		if (path.StartsWith('$'))
		{
			return path.Substring(1, path.Length - 1);
		}
		return "Assets/Resources/" + path;
	}

	public static string CompressInABPath(string path)
	{
		if (path.StartsWith("Assets/Resources/", StringComparison.OrdinalIgnoreCase))
		{
			int length = "Assets/Resources/".Length;
			return path.Substring(length, path.Length - length);
		}
		return "$" + path;
	}
}
