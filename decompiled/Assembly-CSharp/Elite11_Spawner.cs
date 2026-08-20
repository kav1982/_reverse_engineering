using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite11_Spawner : MonoBehaviour
{
	public List<Elite11_Child> children = new List<Elite11_Child>();

	private List<float> childrenAngle = new List<float>();

	private List<float> childrenAngleDelta = new List<float>();

	[Header("生成范围")]
	public VariableFloat spawnRadius;

	public float spawnInterval;

	[Header("生成事项")]
	public int childMaxCount;

	public void SummonChild()
	{
		StartCoroutine(SummonAllChild());
	}

	private IEnumerator SummonAllChild()
	{
		for (int i = 0; i < childMaxCount; i++)
		{
			Elite11_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/301121", base.transform.position + GetSortDir() * spawnRadius.RandomResult()).GetComponent<Elite11_Child>();
			children.Add(component);
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	private Vector3 GetSortDir()
	{
		if (children.Count < 3)
		{
			return Tool2D.GetDir();
		}
		children.Sort();
		childrenAngle.Clear();
		childrenAngleDelta.Clear();
		for (int i = 0; i < children.Count; i++)
		{
			float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, children[i].transform.position - base.transform.position);
			if (num < 0f)
			{
				num += 360f;
			}
			childrenAngle.Add(num);
		}
		for (int j = 0; j < childrenAngle.Count; j++)
		{
			int num2 = j + 1;
			if (num2 >= childrenAngle.Count)
			{
				num2 = 0;
			}
			float num3 = childrenAngle[j] - childrenAngle[num2];
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			childrenAngleDelta.Add(num3);
		}
		int index = 0;
		float num4 = 0f;
		for (int k = 0; k < childrenAngleDelta.Count; k++)
		{
			if (childrenAngleDelta[k] > num4)
			{
				index = k;
				num4 = childrenAngleDelta[k];
			}
		}
		return Tool2D.GetDir(Vector3.up, childrenAngle[index] - childrenAngleDelta[index] / 2f);
	}

	public void RespawnSingle()
	{
		Elite11_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/301121", base.transform.position + GetSortDir() * spawnRadius.RandomResult()).GetComponent<Elite11_Child>();
		children.Add(component);
	}

	public void SummonRest()
	{
		while (children.Count < childMaxCount)
		{
			Elite11_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/301121", base.transform.position + GetSortDir() * spawnRadius.RandomResult()).GetComponent<Elite11_Child>();
			children.Add(component);
		}
	}

	public void ReportDead(Elite11_Child deadChild)
	{
		children.Remove(deadChild);
	}

	public void OnEnable()
	{
	}

	private void Update()
	{
	}
}
