using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster1_Leg : MonoBehaviour
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

	public float moveSpeed;

	public float rootOffsetX;

	public float middleHeight;

	public float normalDistance;

	public VariableFloat outDistance;

	public float correctExtraDistance;

	public VariableFloat moveCorrectDistanceRatio;

	public VariableFloat idleCorrectDistanceRatio;

	public Material mt_origin;

	public Material mt_safe;

	public float jumpHeightOffset;

	[Header("Longleg")]
	public bool isLongLeg;

	public float longLegMiddleMaxOffset;

	private float originWidthMultiplier;

	private float originShadowWidthMuitiplier;

	private Vector3 beforeFallRootPoint;

	private List<Vector3> beforeFallNodePoints = new List<Vector3>();

	private List<Vector3> beforeFallShadowPoints = new List<Vector3>();

	private LegState state;

	private Monster1 monster1;

	private Vector3 rootHorizontalOffset;

	private Vector3 legDir;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 currentShadowEndPoint;

	private Vector3 jumpDeltaPoint;

	private Vector3 jumpDeltaPointZFixed;

	private float originalMotionLocalY;

	private Vector3 RootPoint => monster1.transform.position + rootHorizontalOffset * monster1.transform.lossyScale.x + monster1.myPpt.Tsf_BeHit.localPosition * monster1.transform.lossyScale.x + monster1.tsf_Motion.localPosition * monster1.transform.lossyScale.x;

	private Vector3 RootPointZFixed => RootPoint + new Vector3(0f, 0f - monster1.tsf_Motion.localPosition.y, monster1.tsf_Motion.localPosition.y * 0.01f);

	private Vector3 NormalPoint => monster1.transform.position + legDir * normalDistance;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSaveMode));
		SetSaveMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSaveMode));
	}

	private void SetSaveMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			lr_Leg.enabled = false;
			lr_Shadow.enabled = false;
		}
		else
		{
			lr_Leg.enabled = true;
			lr_Shadow.enabled = true;
		}
	}

	private void LateUpdate()
	{
		float x = monster1.transform.lossyScale.x;
		lr_Leg.widthMultiplier = x * originWidthMultiplier;
		lr_Shadow.widthMultiplier = x * originShadowWidthMuitiplier;
		if (monster1.myPpt.Affect_InAbyss)
		{
			for (int i = 0; i < nodeCount; i++)
			{
				lr_Leg.SetPosition(i, (beforeFallNodePoints[i] - beforeFallRootPoint) * x + RootPoint);
			}
			for (int j = 0; j < 2; j++)
			{
				lr_Shadow.SetPosition(j, (beforeFallShadowPoints[j] - monster1.transform.position) * x + monster1.transform.position);
			}
			return;
		}
		beforeFallRootPoint = RootPoint;
		switch (state)
		{
		case LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > (outDistance.result + correctExtraDistance) * (outDistance.result + correctExtraDistance))
			{
				state = LegState.Move;
				if (monster1.IsMove)
				{
					moveToEndPoint = NormalPoint + monster1.CurrentMotion.normalized * outDistance.RandomResult() * moveCorrectDistanceRatio.RandomResult();
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
		case LegState.JumpPrepare:
			jumpDeltaPoint = RootPoint + legDir * 0.2f - currentEndPoint;
			jumpDeltaPointZFixed = RootPointZFixed + legDir * 0.2f - currentEndPoint;
			break;
		case LegState.Jump:
			jumpDeltaPoint = Vector3.Lerp(jumpDeltaPoint, Vector3.zero, moveSpeed * Time.deltaTime);
			jumpDeltaPointZFixed = Vector3.Lerp(jumpDeltaPointZFixed, Vector3.zero, moveSpeed * Time.deltaTime);
			currentEndPoint = RootPoint + legDir * 0.2f - jumpDeltaPoint + new Vector3(0f, 0f, 0f - jumpHeightOffset);
			currentShadowEndPoint = RootPointZFixed + legDir * 0.2f - jumpDeltaPointZFixed;
			break;
		case LegState.Fly:
			currentEndPoint = RootPoint + legDir * 0.2f + new Vector3(0f, 0f, 0f - jumpHeightOffset);
			currentShadowEndPoint = monster1.transform.position + legDir * 0.2f;
			break;
		default:
			Debug.LogError(state);
			break;
		}
		Vector3 zero = Vector3.zero;
		if (isLongLeg)
		{
			float num = Mathf.Lerp(0f, 1f, originalMotionLocalY / monster1.tsf_Motion.localPosition.y);
			zero = currentEndPoint + new Vector3(0f, middleHeight, 0f) + legDir * num * longLegMiddleMaxOffset;
		}
		else
		{
			zero = (RootPoint + currentEndPoint) / 2f + new Vector3(0f, middleHeight, 0f);
		}
		for (int k = 0; k < nodeCount; k++)
		{
			Vector3 rootPoint = GeneralTool.QuadraticBezierCurve(RootPoint, zero, currentEndPoint, (float)k / ((float)nodeCount - 1f));
			lr_Leg.SetPosition(k, Tool2D.GetLayerPoint(rootPoint) + new Vector3(0f, 0f, (0f - (rootPoint.y - currentEndPoint.y)) * 0.01f));
			beforeFallNodePoints[k] = lr_Leg.GetPosition(k);
		}
		if (state == LegState.Jump || state == LegState.Fly)
		{
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(monster1.transform.position, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentShadowEndPoint, 1.05f));
		}
		else
		{
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(monster1.transform.position, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		}
		beforeFallShadowPoints[0] = lr_Shadow.GetPosition(0);
		beforeFallShadowPoints[1] = lr_Shadow.GetPosition(1);
		if (lr_Leg.startColor != monster1.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster1.myPpt.BaseColor;
			lr_Leg.endColor = monster1.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster1 monster1, Vector3 legDir)
	{
		this.monster1 = monster1;
		this.legDir = legDir;
		originalMotionLocalY = monster1.tsf_Motion.localPosition.y;
		rootHorizontalOffset = legDir * rootOffsetX;
		lr_Leg.positionCount = nodeCount;
		originWidthMultiplier = lr_Leg.widthMultiplier;
		originShadowWidthMuitiplier = lr_Shadow.widthMultiplier;
		for (int i = 0; i < nodeCount; i++)
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
		moveToEndPoint = NormalPoint + Tool2D.GetDir() * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
		StopFly();
		LateUpdate();
	}

	public void SetJump()
	{
		state = LegState.JumpPrepare;
	}

	public void SetJumpRelease()
	{
		state = LegState.Jump;
	}

	public void SetFly()
	{
		state = LegState.Fly;
	}

	public void StopFly()
	{
		state = LegState.Idle;
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		currentEndPoint += changeValue;
	}
}
