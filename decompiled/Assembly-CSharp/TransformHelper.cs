using UnityEngine;

public static class TransformHelper
{
	public static void DestroyAllChildImmediate(this Transform tsf)
	{
		while (tsf.childCount != 0)
		{
			Object.DestroyImmediate(tsf.GetChild(0).gameObject);
		}
	}

	public static void DestroyAllChild(this Transform tsf)
	{
		for (int num = tsf.childCount - 1; num >= 0; num--)
		{
			Object.Destroy(tsf.GetChild(num).gameObject);
		}
	}

	public static void DestroyChildByName(this Transform tsf, string name)
	{
		for (int num = tsf.childCount - 1; num >= 0; num--)
		{
			Transform child = tsf.GetChild(num);
			if (child.name == name)
			{
				Object.Destroy(child.gameObject);
			}
		}
	}

	public static void ChangeAllLayer(this Transform tsf, string layerName)
	{
		if (LayerMask.NameToLayer(layerName) == -1)
		{
			Debug.Log("Layer中不存在,请手动添加LayerName");
		}
		else
		{
			ChangeLayerAndChild(tsf, layerName);
		}
	}

	public static void ChangeLayer(this Transform tsf, string layerName)
	{
		if (LayerMask.NameToLayer(layerName) == -1)
		{
			Debug.Log("Layer中不存在,请手动添加LayerName");
		}
		else
		{
			tsf.gameObject.layer = LayerMask.NameToLayer(layerName);
		}
	}

	public static void ChangeLayerAndChild(Transform tsf, string layerName)
	{
		tsf.gameObject.layer = LayerMask.NameToLayer(layerName);
		for (int i = 0; i < tsf.childCount; i++)
		{
			ChangeLayerAndChild(tsf.GetChild(i), layerName);
		}
	}

	public static void HideAllChild(this Transform tsf)
	{
		for (int i = 0; i < tsf.childCount; i++)
		{
			tsf.GetChild(i).gameObject.SetActive(value: false);
		}
	}

	public static void ShowAllChild(this Transform tsf)
	{
		for (int i = 0; i < tsf.childCount; i++)
		{
			tsf.GetChild(i).gameObject.SetActive(value: true);
		}
	}

	public static void IgnoreZPoint(this Transform tsf, float z = 0f)
	{
		tsf.position = new Vector3(tsf.position.x, tsf.position.y, z);
	}
}
