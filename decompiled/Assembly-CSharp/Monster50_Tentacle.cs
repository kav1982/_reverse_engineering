using UnityEngine;

public class Monster50_Tentacle : MonoBehaviour
{
	public LineRenderer lr_main;

	public int nodeCount;

	public VariableFloat segmentLength;

	public float lerp;

	public float rootHight;

	public float aaa;

	private Vector3[] nodePoints;

	private Vector3[] nodePoints2;

	public Monster50 master;

	public Transform tsf_Target;

	public float horizonOffset;

	private bool skipFrame1;

	public Transform tsf_Center;

	private float groundHeightDelta;

	public void Initialize(Monster50 master)
	{
		this.master = master;
		lr_main.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
		nodePoints2 = new Vector3[nodeCount];
		segmentLength.RandomResult();
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = Tool2D.IgnoreZPoint(tsf_Target.position);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + Vector3.down * segmentLength.result;
			}
			lr_main.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), tsf_Target.position.z));
		}
		groundHeightDelta = (tsf_Target.transform.position - tsf_Center.transform.position).y;
	}

	private void LateUpdate()
	{
		Vector3 down = Vector3.down;
		for (int i = 0; i < nodeCount; i++)
		{
			switch (i)
			{
			case 0:
				nodePoints[i] = Tool2D.IgnoreZPoint(tsf_Target.position);
				break;
			case 1:
				nodePoints[i] = nodePoints[i - 1] + down * segmentLength.result;
				break;
			default:
			{
				Vector3 b = nodePoints[i - 1] - nodePoints[i - 2];
				Vector3 vector = Vector3.Lerp(nodePoints[i] - nodePoints[i - 1], b, lerp);
				Vector3 target = nodePoints[i - 1] + vector.normalized * segmentLength.result;
				if (target.y < master.transform.position.y + groundHeightDelta)
				{
					target.y = master.transform.position.y + groundHeightDelta;
				}
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], target, ref nodePoints2[i], aaa);
				break;
			}
			}
			lr_main.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), tsf_Target.position.z));
		}
		if (skipFrame1)
		{
			if (lr_main.startColor != master.myPpt.BaseColor)
			{
				lr_main.startColor = master.myPpt.BaseColor;
				lr_main.endColor = master.myPpt.BaseColor;
			}
		}
		else
		{
			skipFrame1 = true;
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
