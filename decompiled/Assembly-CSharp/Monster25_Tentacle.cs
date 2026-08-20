using UnityEngine;

public class Monster25_Tentacle : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public Vector3 offset;

	public VariableInt nodeCount;

	public VariableFloat segmentLength;

	public float lerp;

	private Vector3[] nodePoints;

	private Monster25 monster25;

	[Header("和谐模式")]
	public Material mt_H;

	private void Update()
	{
		float z = Tool2D.GetLayerPoint(monster25.transform).z + monster25.tsf_Motion.localPosition.z + 0.001f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + new Vector3(0f, 0f, 0f - monster25.tsf_Motion.localPosition.y);
			}
			else
			{
				nodePoints[i] = Vector3.Lerp(nodePoints[i], nodePoints[i - 1] - base.transform.up * segmentLength.result, lerp * Time.deltaTime);
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
		if (lr_Leg.startColor != monster25.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster25.myPpt.BaseColor;
			lr_Leg.endColor = monster25.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster25 monster25, float angle)
	{
		this.monster25 = monster25;
		base.transform.up = Tool2D.GetDir(angle);
		base.transform.position += offset;
		nodeCount.RandomResult();
		segmentLength.RandomResult();
		lr_Leg.positionCount = nodeCount.result;
		lr_Shadow.positionCount = nodeCount.result;
		nodePoints = new Vector3[nodeCount.result];
	}

	public void EveryInitial()
	{
		float num = Tool2D.GetLayerPoint(monster25.transform).z + monster25.tsf_Motion.localPosition.z + 0.1f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + new Vector3(0f, 0f, 0f - monster25.tsf_Motion.localPosition.y);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] - base.transform.up * segmentLength.result;
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), num * 0.01f));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
		if (GameMgr.IsHarmony_Static)
		{
			Object.Destroy(lr_Leg.material);
			lr_Leg.material = mt_H;
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount.result; i++)
		{
			nodePoints[i] += changeValue;
		}
	}
}
