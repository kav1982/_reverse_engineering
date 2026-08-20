using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

public class HotFixBundle
{
	public readonly Dictionary<string, string[]> Index;

	public readonly int Version;

	public readonly string Dir;

	private readonly Dictionary<string, AssetBundle> _loadedBundle = new Dictionary<string, AssetBundle>();

	private AssetBundleManifest _manifest;

	public static HotFixBundle[] Loaded = Array.Empty<HotFixBundle>();

	private HotFixBundle(int version, string dir, Dictionary<string, string[]> index)
	{
		Version = version;
		Dir = dir;
		Index = index;
	}

	public IEnumerator LoadAssetBundleAsync(string abName)
	{
		string[] allDependencies = _manifest.GetAllDependencies(abName);
		foreach (string text in allDependencies)
		{
			if (!BundleIsLoaded(text))
			{
				yield return Load(text);
			}
		}
		yield return Load(abName);
		IEnumerator Load(string name)
		{
			string path = Path.Combine(Dir, "assets", name);
			AssetBundleCreateRequest load = AssetBundle.LoadFromFileAsync(path);
			yield return load;
			_loadedBundle[name] = load.assetBundle;
		}
	}

	public AssetBundle GetAssetBundle(string abName)
	{
		return _loadedBundle[abName];
	}

	public bool BundleIsLoaded(string abName)
	{
		return _loadedBundle.ContainsKey(abName);
	}

	[CanBeNull]
	public string GetCodePatchFilePath()
	{
		string text = Path.Combine(Dir, "fix");
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	public static IEnumerator InitAsync()
	{
		List<HotFixBundle> result = new List<HotFixBundle>();
		string path = Path.Combine(Application.persistentDataPath, "hotfix");
		if (Directory.Exists(path))
		{
			bool done = false;
			new Thread((ThreadStart)delegate
			{
				result.AddRange(from hfb in Directory.GetDirectories(path).Select(LoadHotFixBundle)
					where hfb != null
					select hfb);
				done = true;
			}).Start();
			while (!done)
			{
				yield return null;
			}
		}
		foreach (HotFixBundle bundle in result)
		{
			if (bundle.Index.Count != 0)
			{
				AssetBundleCreateRequest load = AssetBundle.LoadFromFileAsync(Path.Combine(bundle.Dir, "manifest"));
				yield return load;
				AssetBundle ab = load.assetBundle;
				AssetBundleRequest manifestLoad = ab.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
				yield return manifestLoad;
				bundle._manifest = (AssetBundleManifest)manifestLoad.asset;
				ab.Unload(unloadAllLoadedObjects: false);
			}
		}
		Loaded = result.ToArray();
	}

	[CanBeNull]
	private static HotFixBundle LoadHotFixBundle(string path)
	{
		int num = int.Parse(HotFix.DCSDecrypt(File.ReadAllText(Path.Combine(path, "version"))));
		if (num <= GameVersion.Build)
		{
			return null;
		}
		string path2 = Path.Combine(path, "index");
		Dictionary<string, string[]> index = ((!File.Exists(path2)) ? new Dictionary<string, string[]>() : JsonConvert.DeserializeObject<Dictionary<string, string[]>>(HotFix.DCSDecrypt(File.ReadAllText(path2))));
		return new HotFixBundle(num, path, index);
	}
}
