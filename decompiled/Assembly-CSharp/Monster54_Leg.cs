using UnityEngine;

public class Monster54_Leg : MonoBehaviour
{
	private enum LegState
	{
		Idle,
		Move,
		Fly,
		Jump,
		JumpPrepare
	}

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int nodeCount;

	public int footCount;

	public float footLength;

	public float moveSpeed;

	public float rootOffsetX;

	public float curve1Length;

	public float curve1Percent;

	public float normalDistance;

	public VariableFloat outDistance;

	public float correctExtraDistance;

	public VariableFloat moveCorrectDistanceRatio;

	public VariableFloat idleCorrectDistanceRatio;

	private LegState state;

	private Monster54 monster54;

	private Vector3 rootHorizontalOffset;

	private Vector3 legDir;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 RootPoint => monster54.transform.position + rootHorizontalOffset + monster54.myPpt.Tsf_BeHit.localPosition + monster54.tsf_Motion.localPosition;

	private Vector3 RootPointZFixed => RootPoint + new Vector3(0f, 0f - monster54.tsf_Motion.localPosition.y, 0f - monster54.tsf_Motion.localPosition.y);

	private Vector3 NormalPoint => monster54.transform.position + legDir * normalDistance;

	private void Update()
	{
		switch (state)
		{
		case LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > (outDistance.result + correctExtraDistance) * (outDistance.result + correctExtraDistance))
			{
				state = LegState.Move;
				if (monster54.IsMove)
				{
					moveToEndPoint = NormalPoint + monster54.CurrentMotion.normalized * outDistance.RandomResult() * moveCorrectDistanceRatio.RandomResult();
				}
				else
				{
					moveToEndPoint = NormalPoint + legDir * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
				}
				if (Physics.Raycast(NormalPoint, moveToEndPoint - NormalPoint, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss", "Cliff")) && (NormalPoint - hitInfo.point).sqrMagnitude < (NormalPoint - moveToEndPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
				}
			}
			break;
		case LegState.Move:
			currentEndPoint = Vector3.MoveTowards(currentEndPoint, moveToEndPoint, moveSpeed * Time.deltaTime);
			if (currentEndPoint == moveToEndPoint)
			{
				state = LegState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		Vector3 vector = currentEndPoint - RootPointZFixed;
		Vector3 vector2 = Vector3.zero;
		if (vector.z != 0f)
		{
			vector2 = new Vector3(vector.x, vector.y, (0f - (vector.x * vector.x + vector.y * vector.y)) / vector.z);
		}
		vector2.Normalize();
		Vector3 v = RootPointZFixed + vector * curve1Percent + vector2 * curve1Length;
		Vector3 b = currentEndPoint + Tool2D.IgnoreZPoint(legDir).normalized * footLength;
		for (int i = 0; i < nodeCount; i++)
		{
			lr_Leg.SetPosition(i, Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(RootPointZFixed, v, currentEndPoint, (float)i / ((float)nodeCount - 1f))));
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(monster54.transform.position, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		for (int j = 0; j < footCount; j++)
		{
			lr_Leg.SetPosition(j + nodeCount, Tool2D.GetLayerPoint(Vector3.Lerp(currentEndPoint, b, (float)j / ((float)footCount - 1f))));
		}
		if (lr_Leg.startColor != monster54.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster54.myPpt.BaseColor;
			lr_Leg.endColor = monster54.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster54 monster54, Vector3 legDir)
	{
		this.monster54 = monster54;
		this.legDir = legDir;
		lr_Leg.positionCount = nodeCount + footCount;
		rootHorizontalOffset = legDir * rootOffsetX;
	}

	public void EveryInitial()
	{
		moveToEndPoint = NormalPoint + Tool2D.GetDir() * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
		Update();
	}

	public void Theme6Reposition(Vector3 delta)
	{
		moveToEndPoint += delta;
		currentEndPoint += delta;
	}
}
