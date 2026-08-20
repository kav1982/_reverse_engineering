using System.Collections.Generic;
using UnityEngine;

public class Monster40_Leg : MonoBehaviour
{
	private enum LegState
	{
		Idle,
		Move
	}

	public LineRenderer lr_Leg1;

	public LineRenderer lr_Leg2;

	public LineRenderer lr_Leg3;

	public LineRenderer lr_Shadow;

	public float moveSpeed;

	public float rootOffsetX;

	public float point1Lerp;

	public float point2Lerp;

	public float point1OffsetY;

	public float point2OffsetY;

	public float normalDistance;

	public VariableFloat outDistance;

	public float correctExtraDistance;

	public VariableFloat moveCorrectDistanceRatio;

	public VariableFloat idleCorrectDistanceRatio;

	public Transform foot;

	private LegState state;

	private Monster40 master;

	private Vector3 rootHorizontalOffset;

	private Vector3 legDir;

	private float legAngle;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 point1;

	private Vector3 point2;

	private Vector3 curLegDir;

	private float originWidthMultiplier1;

	private float originWidthMultiplier2;

	private float originShadowWidthMuitiplier;

	private float originFootScale;

	private Vector3 beforeFallOriginalPoint;

	private List<Vector3> beforeFallNodePoints = new List<Vector3>();

	private List<Vector3> beforeFallShadowPoints = new List<Vector3>();

	private Vector3 RootPoint => master.transform.position + rootHorizontalOffset * master.transform.lossyScale.x + master.tsf_Motion.localPosition * master.transform.lossyScale.x;

	private Vector3 NormalPoint => master.transform.position + curLegDir * normalDistance;

	private void Update()
	{
		float x = master.transform.lossyScale.x;
		lr_Leg2.widthMultiplier = originWidthMultiplier1 * x;
		lr_Leg3.widthMultiplier = originWidthMultiplier2 * x;
		lr_Shadow.widthMultiplier = originShadowWidthMuitiplier * x;
		foot.localScale = originFootScale * x * Vector3.one;
		if (master.myPpt.Affect_InAbyss)
		{
			for (int i = 0; i < 2; i++)
			{
				lr_Leg2.SetPosition(i, (beforeFallNodePoints[i] - beforeFallOriginalPoint) * x + RootPoint);
				lr_Leg3.SetPosition(i, (beforeFallNodePoints[i + 1] - beforeFallOriginalPoint) * x + RootPoint);
			}
			for (int j = 0; j < 2; j++)
			{
				lr_Shadow.SetPosition(j, (beforeFallShadowPoints[j] - master.transform.position) * x + master.transform.position);
			}
			return;
		}
		beforeFallOriginalPoint = RootPoint;
		beforeFallNodePoints[0] = lr_Leg2.GetPosition(0);
		beforeFallNodePoints[1] = lr_Leg2.GetPosition(1);
		beforeFallNodePoints[2] = lr_Leg3.GetPosition(1);
		beforeFallShadowPoints[0] = lr_Shadow.GetPosition(0);
		beforeFallShadowPoints[1] = lr_Shadow.GetPosition(1);
		curLegDir = Tool2D.GetDir(legAngle + Tool2D.GetDegree(master.faceDir));
		rootHorizontalOffset = curLegDir * rootOffsetX;
		switch (state)
		{
		case LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > (outDistance.result + correctExtraDistance) * (outDistance.result + correctExtraDistance))
			{
				state = LegState.Move;
				if (master.IsMove)
				{
					moveToEndPoint = NormalPoint + master.CurrentMotion.normalized * outDistance.RandomResult();
				}
				else
				{
					moveToEndPoint = NormalPoint + curLegDir * outDistance.RandomResult();
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
		point1 = Vector3.Lerp(RootPoint, currentEndPoint, point1Lerp) + new Vector3(0f, point1OffsetY, 0f);
		point2 = Vector3.Lerp(RootPoint, currentEndPoint, point2Lerp) + new Vector3(0f, point2OffsetY, 0f);
		lr_Leg1.SetPosition(0, Tool2D.GetLayerPoint(RootPoint));
		lr_Leg1.SetPosition(1, Tool2D.GetLayerPoint(RootPoint));
		lr_Leg2.SetPosition(0, Tool2D.GetLayerPoint(RootPoint));
		lr_Leg2.SetPosition(1, Tool2D.GetLayerPoint(point2));
		lr_Leg3.SetPosition(0, Tool2D.GetLayerPoint(point2));
		lr_Leg3.SetPosition(1, Tool2D.GetLayerPoint(currentEndPoint));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(master.transform.position, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		foot.position = Tool2D.GetLayerPoint(currentEndPoint) + Vector3.forward * 0.01f;
		foot.transform.up = -curLegDir;
		if (lr_Leg1.startColor != master.myPpt.BaseColor)
		{
			lr_Leg1.startColor = master.myPpt.BaseColor;
			lr_Leg1.endColor = master.myPpt.BaseColor;
			lr_Leg2.startColor = master.myPpt.BaseColor;
			lr_Leg2.endColor = master.myPpt.BaseColor;
			lr_Leg3.startColor = master.myPpt.BaseColor;
			lr_Leg3.endColor = master.myPpt.BaseColor;
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		state = LegState.Idle;
		currentEndPoint += changeValue;
	}

	public void Initialize(Monster40 master, float legAngle)
	{
		this.master = master;
		this.legAngle = legAngle;
		legDir = Tool2D.GetDir(legAngle);
		rootHorizontalOffset = legDir * rootOffsetX;
		moveToEndPoint = master.transform.position + Tool2D.GetDir() * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
		originShadowWidthMuitiplier = lr_Shadow.widthMultiplier;
		originWidthMultiplier1 = lr_Leg2.widthMultiplier;
		originWidthMultiplier2 = lr_Leg3.widthMultiplier;
		originFootScale = foot.localScale.x;
		for (int i = 0; i < 3; i++)
		{
			beforeFallNodePoints.Add(Vector3.zero);
		}
		for (int j = 0; j < 2; j++)
		{
			beforeFallShadowPoints.Add(Vector3.zero);
		}
	}

	public void EveryInitial()
	{
		moveToEndPoint = master.transform.position + Tool2D.GetDir() * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
		lr_Leg2.SetPosition(0, Vector3.zero);
		lr_Leg2.SetPosition(1, Vector3.zero);
		lr_Leg3.SetPosition(0, Vector3.zero);
		lr_Leg3.SetPosition(1, Vector3.zero);
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
	}
}
