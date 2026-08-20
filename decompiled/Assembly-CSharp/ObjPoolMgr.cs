using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjPoolMgr : MonoBehaviour
{
	public enum PreloadType
	{
		Unit,
		Simple
	}

	private readonly Dictionary<string, List<HideSelf>> pool = new Dictionary<string, List<HideSelf>>();

	private readonly Dictionary<string, int> iteratorIndex = new Dictionary<string, int>();

	private readonly Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	private readonly Dictionary<string, int> counterPool = new Dictionary<string, int>();

	private static readonly Vector3 recyclePoint = new Vector3(0f, 1000f, 0f);

	public static ObjPoolMgr Inst { get; private set; }

	public int GetPoolObjectCount()
	{
		return pool.Sum((KeyValuePair<string, List<HideSelf>> e) => e.Value.Count);
	}

	public void Initialize()
	{
		Inst = this;
	}

	public GameObject GetUIGO(string path, float duration = 0f)
	{
		return FinalGetGO(path, recyclePoint, Quaternion.identity, Vector3.one, duration, UIMgr.Inst.rtsf_Canvas2);
	}

	public GameObject GetUIGO(string path, Vector3 point, float duration = 0f)
	{
		return FinalGetGO(path, point, Quaternion.identity, Vector3.one, duration, UIMgr.Inst.rtsf_Canvas2);
	}

	public GameObject GetUIGO(string path, Vector3 point, RectTransform rectTransform, float duration = 0f)
	{
		return FinalGetGO(path, point, Quaternion.identity, Vector3.one, duration, rectTransform);
	}

	public GameObject GetGO(string path, Vector3 point, float duration = 0f, Transform parentT = null, int maxInPoolCount = 0, string counterPoolName = null)
	{
		return FinalGetGO(path, point, Quaternion.identity, Vector3.one, duration, parentT, autoActive: true, maxInPoolCount, counterPoolName);
	}

	public GameObject GetGO(string path, Vector3 point, Vector3 scale, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, Quaternion.identity, scale, duration, parentT);
	}

	public GameObject GetGO(string path, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, recyclePoint, Quaternion.identity, Vector3.one, duration, parentT);
	}

	public GameObject GetGO(string path, Transform parentT)
	{
		return FinalGetGO(path, recyclePoint, Quaternion.identity, Vector3.one, 0f, parentT);
	}

	public GameObject GetGO(string path, Vector3 point, Transform parentT)
	{
		return FinalGetGO(path, point, Quaternion.identity, Vector3.one, 0f, parentT);
	}

	public GameObject GetGO(string path, Vector3 point, Quaternion rotation, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, rotation, Vector3.one, duration, parentT);
	}

	public GameObject GetGO(string path, Vector3 point, Quaternion rotation, Vector3 scale, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, rotation, scale, duration, parentT);
	}

	public GameObject GetGO(string path, Action<GameObject> activeBefore, float duration = 0f, Transform parentT = null)
	{
		GameObject gameObject = FinalGetGO(path, Vector3.zero, Quaternion.identity, Vector3.one, duration, parentT, autoActive: false);
		activeBefore?.Invoke(gameObject);
		gameObject.SetActive(value: true);
		return gameObject;
	}

	private List<HideSelf> GetOrCreatePool(string path, Transform parentT)
	{
		if (pool.TryGetValue(path, out var value))
		{
			return value;
		}
		List<HideSelf> list = new List<HideSelf>();
		GameObject gameObject = ABResources.LoadAsset<GameObject>(path);
		if ((object)gameObject == null)
		{
			Debug.LogWarning("获取对象池物体失败：" + path);
			return null;
		}
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(value: false);
		}
		if (parentT == UIMgr.Inst.rtsf_Canvas2)
		{
			list.Add(UnityEngine.Object.Instantiate(gameObject, UIMgr.Inst.rtsf_Canvas2).AddComponent<HideSelf>());
		}
		else if (parentT == null)
		{
			list.Add(UnityEngine.Object.Instantiate(gameObject, base.transform).AddComponent<HideSelf>());
		}
		pool.Add(path, list);
		prefabs.Add(path, gameObject);
		iteratorIndex.Add(path, 0);
		return list;
	}

	private GameObject FinalGetGO(string path, Vector3 point, Quaternion rotation, Vector3 scale, float duration, Transform parentT, bool autoActive = true, int maxInPoolCount = 0, string counterPoolName = null)
	{
		List<HideSelf> orCreatePool = GetOrCreatePool(path, parentT);
		int num = iteratorIndex[path];
		for (int i = num; i < orCreatePool.Count; i++)
		{
			if (!orCreatePool[i].gameObject.activeSelf)
			{
				HideSelf hideSelf = orCreatePool[i];
				hideSelf.transform.SetPositionAndRotation(point, rotation);
				hideSelf.transform.localScale = scale;
				if (autoActive)
				{
					hideSelf.gameObject.SetActive(value: true);
				}
				hideSelf.SetDuration(duration);
				if (parentT != null)
				{
					hideSelf.transform.SetParent(parentT);
				}
				iteratorIndex[path] = ((i != orCreatePool.Count - 1) ? (i + 1) : 0);
				return hideSelf.gameObject;
			}
		}
		for (int j = 0; j < num; j++)
		{
			if (!orCreatePool[j].gameObject.activeSelf)
			{
				HideSelf hideSelf2 = orCreatePool[j];
				hideSelf2.transform.SetPositionAndRotation(point, rotation);
				hideSelf2.transform.localScale = scale;
				if (autoActive)
				{
					hideSelf2.gameObject.SetActive(value: true);
				}
				hideSelf2.SetDuration(duration);
				if (parentT != null)
				{
					hideSelf2.transform.SetParent(parentT);
				}
				iteratorIndex[path] = ((j != orCreatePool.Count - 1) ? (j + 1) : 0);
				return hideSelf2.gameObject;
			}
		}
		if (maxInPoolCount > 0 && counterPoolName != null && counterPool.GetValueOrDefault(counterPoolName, 0) > maxInPoolCount)
		{
			return null;
		}
		GameObject original = prefabs[path];
		HideSelf hideSelf3 = ((!(parentT == UIMgr.Inst.rtsf_Canvas2)) ? UnityEngine.Object.Instantiate(original, base.transform).AddComponent<HideSelf>() : UnityEngine.Object.Instantiate(original, UIMgr.Inst.rtsf_Canvas2).AddComponent<HideSelf>());
		if (counterPoolName != null)
		{
			counterPool[counterPoolName] = counterPool.GetValueOrDefault(counterPoolName, 0) + 1;
		}
		orCreatePool.Add(hideSelf3);
		iteratorIndex[path] = 0;
		hideSelf3.transform.SetPositionAndRotation(point, rotation);
		hideSelf3.transform.localScale = scale;
		if (autoActive)
		{
			hideSelf3.gameObject.SetActive(value: true);
		}
		hideSelf3.SetDuration(duration);
		if (parentT != null)
		{
			hideSelf3.transform.SetParent(parentT);
		}
		return hideSelf3.gameObject;
	}

	public GameObject PreloadGO(string path, float count, PreloadType type = PreloadType.Simple)
	{
		if (pool.ContainsKey(path))
		{
			return prefabs[path];
		}
		List<HideSelf> list = new List<HideSelf>();
		GameObject gameObject = ABResources.LoadAsset<GameObject>(path);
		if ((object)gameObject == null)
		{
			Debug.LogWarning("获取对象池物体失败：" + path);
			return null;
		}
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(value: false);
		}
		for (int i = 0; (float)i < count; i++)
		{
			HideSelf hideSelf = UnityEngine.Object.Instantiate(gameObject, base.transform).AddComponent<HideSelf>();
			list.Add(hideSelf);
			if (type == PreloadType.Unit)
			{
				hideSelf.GetComponent<UnitProperty>().SingleInitial();
			}
		}
		pool.Add(path, list);
		prefabs.Add(path, gameObject);
		iteratorIndex.Add(path, 0);
		return prefabs[path];
	}

	public void RecycleGO(GameObject go)
	{
		if (go != null && go.activeInHierarchy)
		{
			go.SetActive(value: false);
			go.transform.position = recyclePoint;
			if (go.transform.parent != base.transform && go.transform.parent != UIMgr.Inst.rtsf_Canvas2)
			{
				go.transform.SetParent(base.transform);
			}
		}
	}

	public void RecycleGO(GameObject go, float delayTime)
	{
		StartCoroutine(RecycleGOIE(go, delayTime));
	}

	private IEnumerator RecycleGOIE(GameObject go, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		RecycleGO(go);
	}

	public void RecycleAll()
	{
		foreach (KeyValuePair<string, List<HideSelf>> item in pool)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				item.Value[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void RecycleSpecify(string path)
	{
		if (!pool.ContainsKey(path))
		{
			return;
		}
		for (int i = 0; i < pool[path].Count; i++)
		{
			if (pool[path][i].gameObject.activeSelf)
			{
				RecycleGO(pool[path][i].gameObject);
			}
		}
	}

	public List<HideSelf> GetSpecifyPool(string path)
	{
		if (pool.ContainsKey(path))
		{
			return pool[path];
		}
		return null;
	}

	public void ClearSpecify(string path)
	{
		if (pool.ContainsKey(path))
		{
			for (int num = pool[path].Count - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(pool[path][num].gameObject);
			}
			pool.Remove(path);
			iteratorIndex.Remove(path);
			prefabs.Remove(path);
		}
	}

	public void ClearAllPool()
	{
		foreach (List<HideSelf> value in pool.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				UnityEngine.Object.Destroy(value[i].gameObject);
			}
		}
		UIMgr.Inst.rtsf_Canvas2.DestroyAllChild();
		base.transform.DestroyAllChild();
		pool.Clear();
		prefabs.Clear();
		iteratorIndex.Clear();
		counterPool.Clear();
		GlobalParticleEmitSystem.ClearAllEmitter();
	}
}
