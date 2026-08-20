using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniObjPool : MonoBehaviour
{
	public enum PreloadType
	{
		Unit,
		Simple
	}

	public Dictionary<string, List<HideSelf>> pool = new Dictionary<string, List<HideSelf>>();

	private Dictionary<string, int> iteratorIndex = new Dictionary<string, int>();

	private Dictionary<string, GameObject> pfbs = new Dictionary<string, GameObject>();

	private Vector3 recyclePoint = new Vector3(0f, 1000f, 0f);

	private List<GameObject> allObjects = new List<GameObject>();

	public GameObject GetGO(string path, Vector3 point, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, Quaternion.identity, Vector3.one, duration, parentT);
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

	public GameObject GetGO(string path, Vector3 point, Quaternion rotation, Transform parentT)
	{
		return FinalGetGO(path, point, rotation, Vector3.one, 0f, parentT);
	}

	public GameObject GetGO(string path, Vector3 point, Quaternion rotation, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, rotation, Vector3.one, duration, parentT);
	}

	public GameObject GetGO(string path, Vector3 point, Quaternion rotation, Vector3 scale, float duration = 0f, Transform parentT = null)
	{
		return FinalGetGO(path, point, rotation, scale, duration, parentT);
	}

	private GameObject FinalGetGO(string path, Vector3 point, Quaternion rotation, Vector3 scale, float duration, Transform parentT, bool autoActive = true)
	{
		if (!pool.ContainsKey(path))
		{
			pool.Add(path, new List<HideSelf>());
			pfbs.Add(path, Resources.Load<GameObject>(path));
			iteratorIndex.Add(path, 0);
			GameObject gameObject = pfbs[path];
			if ((object)gameObject == null)
			{
				Debug.LogWarning("????????????????" + path);
			}
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(value: false);
			}
			HideSelf hideSelf = null;
			if (parentT == UIMgr.Inst.rtsf_Canvas2)
			{
				hideSelf = Object.Instantiate(gameObject, UIMgr.Inst.rtsf_Canvas2).AddComponent<HideSelf>();
			}
			else if (parentT == null)
			{
				hideSelf = Object.Instantiate(gameObject, base.transform).AddComponent<HideSelf>();
			}
			if ((bool)hideSelf)
			{
				pool[path].Add(hideSelf);
				allObjects.Add(hideSelf.gameObject);
			}
		}
		List<HideSelf> list = pool[path];
		for (int i = iteratorIndex[path]; i < list.Count; i++)
		{
			if (!list[i].gameObject.activeSelf)
			{
				HideSelf hideSelf2 = list[i];
				hideSelf2.transform.position = point;
				hideSelf2.transform.rotation = rotation;
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
				iteratorIndex[path] = ((i != list.Count - 1) ? (i + 1) : 0);
				return hideSelf2.gameObject;
			}
		}
		for (int j = 0; j < iteratorIndex[path]; j++)
		{
			if (!list[j].gameObject.activeSelf)
			{
				HideSelf hideSelf3 = list[j];
				hideSelf3.transform.position = point;
				hideSelf3.transform.rotation = rotation;
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
				iteratorIndex[path] = ((j != list.Count - 1) ? (j + 1) : 0);
				return hideSelf3.gameObject;
			}
		}
		GameObject original = pfbs[path];
		HideSelf hideSelf4 = ((!(parentT == UIMgr.Inst.rtsf_Canvas2)) ? Object.Instantiate(original, base.transform).AddComponent<HideSelf>() : Object.Instantiate(original, UIMgr.Inst.rtsf_Canvas2).AddComponent<HideSelf>());
		list.Add(hideSelf4);
		allObjects.Add(hideSelf4.gameObject);
		iteratorIndex[path] = 0;
		hideSelf4.transform.position = point;
		hideSelf4.transform.rotation = rotation;
		hideSelf4.transform.localScale = scale;
		if (autoActive)
		{
			hideSelf4.gameObject.SetActive(value: true);
		}
		hideSelf4.SetDuration(duration);
		if (parentT != null)
		{
			hideSelf4.transform.SetParent(parentT);
		}
		return hideSelf4.gameObject;
	}

	public void PreloadGO(string path, float count, PreloadType type = PreloadType.Simple)
	{
		if (pool.ContainsKey(path))
		{
			return;
		}
		pool.Add(path, new List<HideSelf>());
		pfbs.Add(path, Resources.Load<GameObject>(path));
		iteratorIndex.Add(path, 0);
		GameObject gameObject = pfbs[path];
		if ((object)gameObject == null)
		{
			Debug.LogWarning("????????????????" + path);
		}
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(value: false);
		}
		for (int i = 0; (float)i < count; i++)
		{
			HideSelf hideSelf = Object.Instantiate(gameObject, base.transform).AddComponent<HideSelf>();
			allObjects.Add(hideSelf.gameObject);
			pool[path].Add(hideSelf);
			if (type == PreloadType.Unit)
			{
				hideSelf.GetComponent<UnitProperty>().SingleInitial();
			}
		}
	}

	public void RecycleGO(GameObject go)
	{
		go.SetActive(value: false);
		go.transform.position = recyclePoint;
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

	private void OnDestroy()
	{
		foreach (GameObject allObject in allObjects)
		{
			if ((bool)allObject)
			{
				Object.Destroy(allObject);
			}
		}
	}
}
