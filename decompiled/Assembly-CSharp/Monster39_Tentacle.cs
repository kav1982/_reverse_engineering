using UnityEngine;

public class Monster39_Tentacle : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public Vector3 offset;

	public VariableInt nodeCount;

	public VariableFloat segmentLength;

	public float lerp;

	private Vector3[] nodePoints;

	public Monster39_Drone monster39Drone;

	private void Update()
	{
		float z = Tool2D.GetLayerPoint(monster39Drone.transform).z + monster39Drone.tsf_Motion.localPosition.z + 0.1f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position;
			}
			else
			{
				nodePoints[i] = Vector3.Lerp(nodePoints[i], nodePoints[i - 1] - base.transform.up * segmentLength.result, lerp * Time.deltaTime);
			}
			lr_Leg.SetPosition(i, nodePoints[i] + new Vector3(0f, 0f, z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
		if (lr_Leg.startColor != monster39Drone.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster39Drone.myPpt.BaseColor;
			lr_Leg.endColor = monster39Drone.myPpt.BaseColor;
		}
	}

	public void Initialize(Monster39_Drone monster39Drone, float angle)
	{
		this.monster39Drone = monster39Drone;
		base.transform.up = Tool2D.GetDir(angle);
		base.transform.position += offset;
		nodeCount.RandomResult();
		segmentLength.RandomResult();
		lr_Leg.positionCount = nodeCount.result;
		lr_Shadow.positionCount = nodeCount.result;
		nodePoints = new Vector3[nodeCount.result];
		float z = Tool2D.GetLayerPoint(monster39Drone.transform).z + monster39Drone.tsf_Motion.localPosition.z + 0.1f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + new Vector3(0f, 0f, 0f - monster39Drone.tsf_Motion.localPosition.y);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] - base.transform.up * segmentLength.result;
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
	}
}
