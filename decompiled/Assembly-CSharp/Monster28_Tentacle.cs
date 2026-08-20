using UnityEngine;

public class Monster28_Tentacle : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int nodeCount;

	public float segmentLength;

	public float lerp;

	public VariableFloat rotateAngle;

	public float zOffset;

	private Vector3[] nodePoints;

	private Monster28 monster28;

	private Vector3 RootPoint => monster28.transform.position + base.transform.localPosition + new Vector3(monster28.tsf_Motion.localPosition.x, 0f, 0f - monster28.tsf_Motion.localPosition.y);

	private void LateUpdate()
	{
		if (lr_Leg.startColor != monster28.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster28.myPpt.BaseColor;
			lr_Leg.endColor = monster28.myPpt.BaseColor;
		}
		if (monster28.myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			return;
		}
		float z = (Tool2D.GetLayerPoint(monster28.transform).z / 0.01f + monster28.tsf_Motion.localPosition.z + zOffset) * 0.01f;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = RootPoint;
			}
			else
			{
				nodePoints[i] = Vector3.Lerp(nodePoints[i], nodePoints[i - 1] + Tool2D.GetDir(base.transform.up, rotateAngle.result * (float)i) * segmentLength, lerp * Time.deltaTime);
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
	}

	public void SingleInitial(Monster28 monster28)
	{
		this.monster28 = monster28;
		lr_Leg.positionCount = nodeCount;
		lr_Shadow.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
	}

	public void EveryInitial()
	{
		rotateAngle.RandomResult();
		float z = (Tool2D.GetLayerPoint(monster28.transform).z / 0.01f + monster28.tsf_Motion.localPosition.z + zOffset) * 0.01f;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = RootPoint;
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + Tool2D.GetDir(base.transform.up, rotateAngle.result * (float)i) * segmentLength;
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount; i++)
		{
			nodePoints[i] += changeValue;
		}
	}
}
