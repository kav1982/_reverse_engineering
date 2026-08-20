using UnityEngine;

public class Elite15Child_Tentacle : MonoBehaviour
{
	public LineRenderer lr;

	public int tentacleNodeCount;

	public VariableFloat tentacleRootOffsetY;

	public VariableFloat tentacleRootOffsetXZ;

	public VariableFloat initialAngle;

	public float anglePerNode;

	public float lengthPerNode;

	private Elite15_Child elite15Child;

	private Vector3[] nodePoints;

	public void SingleInitial(Elite15_Child elite15Child)
	{
		this.elite15Child = elite15Child;
		Vector3 vector = new Vector3(0f, tentacleRootOffsetY.RandomResult(), 0f);
		vector += new Vector3(tentacleRootOffsetXZ.RandomResult(), 0f, tentacleRootOffsetXZ.RandomResult() * 0.01f);
		base.transform.up = Tool2D.GetDir(initialAngle.RandomResult());
		float degree = ((initialAngle.result > 0f) ? (0f - anglePerNode) : anglePerNode);
		lr.positionCount = tentacleNodeCount;
		nodePoints = new Vector3[tentacleNodeCount];
		for (int i = 0; i < nodePoints.Length; i++)
		{
			switch (i)
			{
			case 0:
				nodePoints[i] = vector;
				break;
			case 1:
				nodePoints[i] = nodePoints[i - 1] + base.transform.up * lengthPerNode;
				break;
			default:
				nodePoints[i] = nodePoints[i - 1] + Tool2D.GetDir(nodePoints[i - 1] - nodePoints[i - 2], degree) * lengthPerNode;
				break;
			}
			lr.SetPosition(i, nodePoints[i]);
		}
	}

	private void Update()
	{
		if (lr.startColor != elite15Child.myPpt.BaseColor)
		{
			lr.startColor = elite15Child.myPpt.BaseColor;
			lr.endColor = elite15Child.myPpt.BaseColor;
		}
	}
}
