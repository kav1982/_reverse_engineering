using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PreloadAssetsList", menuName = "ScriptableObjects/PreloadAssetsList", order = 0)]
public class PreloadAssetsListSO : ScriptableObject
{
	public bool PreloadOnEditor = true;

	public List<string> Assets = new List<string>();

	public List<string> SubList = new List<string>();

	public IEnumerator LoadAsync()
	{
		if (Application.isEditor && !PreloadOnEditor)
		{
			yield break;
		}
		GameObject preloadObjectRoot = GameObject.Find("PreloadObjects");
		if (preloadObjectRoot == null)
		{
			preloadObjectRoot = new GameObject("PreloadObjects");
			Object.DontDestroyOnLoad(preloadObjectRoot.gameObject);
		}
		foreach (string asset in Assets)
		{
			ResourceRequest load = Resources.LoadAsync(asset);
			load.completed += delegate
			{
				if (load.asset is GameObject original)
				{
					Object.Instantiate(original, preloadObjectRoot.transform).SetActive(value: false);
				}
			};
			yield return load;
		}
		foreach (string sub in SubList)
		{
			PreloadAssetsListSO preloadAssetsListSO = ABResources.LoadAsset<PreloadAssetsListSO>(sub);
			if (preloadAssetsListSO != null)
			{
				yield return preloadAssetsListSO.LoadAsync();
			}
			else
			{
				Debug.LogWarning("不存在的预加载资源列表：" + sub);
			}
		}
	}
}
