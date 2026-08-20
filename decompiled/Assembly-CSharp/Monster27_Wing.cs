using UnityEngine;

public class Monster27_Wing : MonoBehaviour
{
	public LineRenderer lr;

	public int nodeCount;

	public float offset;

	public float segmentLength;

	public float lerp;

	public float rotateSpeed;

	public float rotateHalfAngle;

	public bool needHarmony;

	public Material mat_H;

	private Monster27 monster27;

	private Vector3 dir;

	private bool isLeft;

	private float timeSeed;

	private Vector3[] nodePoints;

	private void Update()
	{
		if (lr.startColor != monster27.myPpt.BaseColor)
		{
			lr.startColor = monster27.myPpt.BaseColor;
			lr.endColor = monster27.myPpt.BaseColor;
		}
		if (monster27.myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			return;
		}
		float z = Tool2D.GetLayerPoint(monster27.transform).z + 0.001f;
		float num = Mathf.Sin(Time.timeSinceLevelLoad * rotateSpeed + timeSeed) * rotateHalfAngle;
		Vector3 vector = Tool2D.GetDir(dir, isLeft ? num : (0f - num));
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + dir * offset + monster27.tsf_Motion.localPosition + monster27.bodyRoot.localPosition;
			}
			else
			{
				nodePoints[i] = Vector3.Lerp(nodePoints[i], nodePoints[i - 1] + vector * segmentLength, lerp * Time.deltaTime);
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
	}

	public void SingleInitial(Monster27 monster27, Vector3 dir, bool isLeft, float timeSeed)
	{
		this.monster27 = monster27;
		this.dir = dir;
		this.isLeft = isLeft;
		this.timeSeed = timeSeed;
		lr.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
		if (GameMgr.IsHarmony_Static && needHarmony)
		{
			Object.Destroy(lr.material);
			lr.material = mat_H;
		}
	}

	public void EveryInitial()
	{
		float z = Tool2D.GetLayerPoint(monster27.transform).z + 0.001f;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + dir * offset + monster27.tsf_Motion.localPosition;
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + dir * segmentLength;
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
	}
}
