using UnityEngine;

public class Monster24_Leg : MonoBehaviour
{
	public LineRenderer lr_Leg1;

	public LineRenderer lr_Leg2;

	public LineRenderer lr_Shadow;

	public Transform tsf_FootRoot;

	public MeshRenderer mr_Foot;

	public float moveSpeedRatio;

	public float offsetX;

	public float normalDistance;

	public float correctDistance;

	public VariableFloat correctDistanceRatio;

	public float liftHeight;

	public float totalLength;

	[Header("Special Fix")]
	public float specialFixDistanceRatio;

	public float specialFixLerp;

	public float specialFixSpeedRatio;

	private Monster24 monster24;

	private Monster24_Leg otherLeg;

	private bool isLeftLeg;

	private Vector3 rootHorizontalOffset;

	private Vector3 normalOffset;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private bool speicalFix;

	private Vector3 liftBeforePoint;

	private Vector3 liftMiddlePoint;

	private float liftBeforeDistance;

	[Header("和谐模式")]
	public Material mt_H;

	public Material footMt_H;

	public Monster24LegState LegState { get; private set; }

	private Vector3 RootPoint => monster24.transform.position + monster24.tsf_Motion.localPosition + rootHorizontalOffset;

	private Vector3 NormalPoint => monster24.transform.position + normalOffset;

	private void Update()
	{
		switch (LegState)
		{
		case Monster24LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > correctDistance * correctDistance && otherLeg.LegState == Monster24LegState.Idle)
			{
				LegState = Monster24LegState.Move;
				if (monster24.CurrentMotion.sqrMagnitude > monster24.myPpt.Rigid.linearVelocity.sqrMagnitude)
				{
					moveToEndPoint = NormalPoint + monster24.CurrentMotion.normalized * correctDistance * correctDistanceRatio.RandomResult();
				}
				else
				{
					moveToEndPoint = NormalPoint + monster24.myPpt.Rigid.linearVelocity.normalized * correctDistance * correctDistanceRatio.RandomResult();
				}
				if (Physics.Raycast(NormalPoint, moveToEndPoint - NormalPoint, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss")) && (NormalPoint - hitInfo.point).sqrMagnitude < (NormalPoint - moveToEndPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
				}
				speicalFix = false;
				liftBeforePoint = currentEndPoint;
				liftBeforeDistance = Vector3.Distance(liftBeforePoint, moveToEndPoint);
				liftMiddlePoint = (currentEndPoint + moveToEndPoint) / 2f + new Vector3(0f, liftHeight, 0f);
			}
			break;
		case Monster24LegState.Move:
		{
			if ((currentEndPoint - moveToEndPoint).sqrMagnitude > correctDistance * correctDistance * specialFixDistanceRatio * specialFixDistanceRatio)
			{
				if (!speicalFix)
				{
					speicalFix = true;
				}
				currentEndPoint = Vector3.Lerp(currentEndPoint, moveToEndPoint, specialFixLerp);
				break;
			}
			float num = moveSpeedRatio;
			if (speicalFix)
			{
				num = specialFixSpeedRatio;
			}
			currentEndPoint = Vector3.MoveTowards(currentEndPoint, moveToEndPoint, monster24.myPpt.unitCfg.moveSpeed * num * Time.deltaTime);
			if (currentEndPoint == moveToEndPoint)
			{
				LegState = Monster24LegState.Idle;
			}
			break;
		}
		default:
			Debug.LogError(LegState);
			break;
		}
		float t = Mathf.Clamp01(Vector3.Distance(liftBeforePoint, currentEndPoint) / liftBeforeDistance);
		Vector3 vector = GeneralTool.QuadraticBezierCurve(liftBeforePoint, liftMiddlePoint, moveToEndPoint, t);
		Vector3 vector2 = (RootPoint + vector) / 2f;
		if ((RootPoint - vector).sqrMagnitude < totalLength * totalLength)
		{
			vector2 += Tool2D.GetDir(vector - RootPoint, (rootHorizontalOffset.x > 0f) ? 90 : (-90)).normalized * Mathf.Sqrt(totalLength / 2f * totalLength / 2f - (RootPoint - vector2).sqrMagnitude);
		}
		lr_Leg1.SetPosition(0, Tool2D.GetLayerPoint(RootPoint));
		lr_Leg1.SetPosition(1, Tool2D.GetLayerPoint(vector2));
		lr_Leg2.SetPosition(0, Tool2D.GetLayerPoint(vector2));
		lr_Leg2.SetPosition(1, Tool2D.GetLayerPoint(vector));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(monster24.transform.position, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(vector, 1.05f));
		tsf_FootRoot.transform.position = Tool2D.GetLayerPoint(vector);
		if (lr_Leg1.startColor != monster24.myPpt.BaseColor)
		{
			lr_Leg1.startColor = monster24.myPpt.BaseColor;
			lr_Leg1.endColor = monster24.myPpt.BaseColor;
			lr_Leg2.startColor = monster24.myPpt.BaseColor;
			lr_Leg2.endColor = monster24.myPpt.BaseColor;
			mr_Foot.material.color = monster24.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster24 monster24, Monster24_Leg otherLeg, bool isLeftLeg)
	{
		this.monster24 = monster24;
		this.otherLeg = otherLeg;
		this.isLeftLeg = isLeftLeg;
		if (GameMgr.IsHarmony_Static)
		{
			lr_Leg1.material = mt_H;
			lr_Leg2.material = mt_H;
			mr_Foot.material = footMt_H;
		}
	}

	public void EveryInitial()
	{
		rootHorizontalOffset = new Vector3(isLeftLeg ? (0f - offsetX) : offsetX, 0f, 0f);
		normalOffset = rootHorizontalOffset + new Vector3(isLeftLeg ? (0f - normalDistance) : normalDistance, 0f, 0f);
		moveToEndPoint = NormalPoint;
		currentEndPoint = moveToEndPoint;
		liftBeforePoint = currentEndPoint + new Vector3(1f, 0f, 0f);
		liftBeforeDistance = Vector3.Distance(liftBeforePoint, moveToEndPoint);
		liftMiddlePoint = (currentEndPoint + moveToEndPoint) / 2f + new Vector3(0f, liftHeight, 0f);
	}

	public void ChangePointImmediate(Vector3 changeValue)
	{
		moveToEndPoint += changeValue;
		currentEndPoint += changeValue;
	}
}
