using System;
using UnityEngine;

public class Elite1_Leg : MonoBehaviour
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

	public Material mt_origin1;

	public Material mt_origin2;

	public Material mt_origin3;

	public Material mt_safe1;

	public Material mt_safe2;

	public Material mt_safe3;

	public bool isSafeMode = true;

	private LegState state;

	private Elite1 elite1;

	private Vector3 rootHorizatalOffset;

	private Vector3 legDir;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 RootPoint => elite1.transform.position + rootHorizatalOffset + elite1.tsf_Motion.localPosition;

	private Vector3 NormalPoint => elite1.transform.position + legDir * normalDistance;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSaveMode));
		SetSaveMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSaveMode));
	}

	public void SetSaveMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			lr_Leg1.enabled = false;
			lr_Leg2.enabled = false;
			lr_Leg3.enabled = false;
			lr_Shadow.enabled = false;
		}
		else
		{
			lr_Leg1.enabled = true;
			lr_Leg2.enabled = true;
			lr_Leg3.enabled = true;
			lr_Shadow.enabled = true;
		}
	}

	private void Update()
	{
		switch (state)
		{
		case LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > (outDistance.result + correctExtraDistance) * (outDistance.result + correctExtraDistance))
			{
				state = LegState.Move;
				if (elite1.IsMove)
				{
					moveToEndPoint = NormalPoint + elite1.CurrentMotion.normalized * outDistance.RandomResult();
				}
				else
				{
					moveToEndPoint = NormalPoint + legDir * outDistance.RandomResult();
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
		Vector3 rootPoint = Vector3.Lerp(RootPoint, currentEndPoint, point1Lerp) + new Vector3(0f, point1OffsetY, 0f);
		Vector3 rootPoint2 = Vector3.Lerp(RootPoint, currentEndPoint, point2Lerp) + new Vector3(0f, point2OffsetY, 0f);
		lr_Leg1.SetPosition(0, Tool2D.GetLayerPoint(RootPoint));
		lr_Leg1.SetPosition(1, Tool2D.GetLayerPoint(rootPoint));
		lr_Leg2.SetPosition(0, Tool2D.GetLayerPoint(rootPoint));
		lr_Leg2.SetPosition(1, Tool2D.GetLayerPoint(rootPoint2));
		lr_Leg3.SetPosition(0, Tool2D.GetLayerPoint(rootPoint2));
		lr_Leg3.SetPosition(1, Tool2D.GetLayerPoint(currentEndPoint));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(elite1.transform.position, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		if (lr_Leg1.startColor != elite1.myPpt.BaseColor)
		{
			lr_Leg1.startColor = elite1.myPpt.BaseColor;
			lr_Leg1.endColor = elite1.myPpt.BaseColor;
			lr_Leg2.startColor = elite1.myPpt.BaseColor;
			lr_Leg2.endColor = elite1.myPpt.BaseColor;
			lr_Leg3.startColor = elite1.myPpt.BaseColor;
			lr_Leg3.endColor = elite1.myPpt.BaseColor;
		}
	}

	public void Initialize(Elite1 elite1, Vector3 legDir)
	{
		this.elite1 = elite1;
		this.legDir = legDir;
		rootHorizatalOffset = legDir * rootOffsetX;
		moveToEndPoint = elite1.transform.position + Tool2D.GetDir() * outDistance.RandomResult() * idleCorrectDistanceRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
	}
}
