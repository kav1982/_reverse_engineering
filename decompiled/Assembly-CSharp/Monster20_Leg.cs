using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster20_Leg : MonoBehaviour
{
	public enum LegState
	{
		Idle,
		Move,
		Floating
	}

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public float moveSpeed;

	public float rootOffsetX;

	public float normalDistance;

	public float middleHeight;

	public float floatingLength;

	public float wingFrequency;

	public float wingAmplitude;

	private float originWidthMultiplier;

	private float originShadowWidthMuitiplier;

	private Vector3 beforeFallOriginalPoint;

	private List<Vector3> beforeFallNodePoints = new List<Vector3>();

	private List<Vector3> beforeFallShadowPoints = new List<Vector3>();

	public LegState state;

	private Monster20 monster20;

	private bool leftLeg;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 OriginalPoint => monster20.transform.position + Tool2D.GetDir(monster20.MoveDir, leftLeg ? (-90) : 90) * rootOffsetX * monster20.transform.lossyScale.x;

	private void LateUpdate()
	{
		float x = monster20.transform.lossyScale.x;
		lr_Leg.widthMultiplier = x * originWidthMultiplier;
		lr_Shadow.widthMultiplier = x * originShadowWidthMuitiplier;
		if (monster20.myPpt.Affect_InAbyss)
		{
			for (int i = 0; i < 3; i++)
			{
				lr_Leg.SetPosition(i, (beforeFallNodePoints[i] - beforeFallOriginalPoint) * x + OriginalPoint);
			}
			for (int j = 0; j < 2; j++)
			{
				lr_Shadow.SetPosition(j, (beforeFallShadowPoints[j] - monster20.transform.position) * x + monster20.transform.position);
			}
			return;
		}
		beforeFallOriginalPoint = OriginalPoint;
		switch (state)
		{
		case LegState.Idle:
			if ((OriginalPoint - currentEndPoint).sqrMagnitude > normalDistance * normalDistance)
			{
				state = LegState.Move;
				moveToEndPoint = OriginalPoint + Tool2D.GetDir(monster20.MoveDir, leftLeg ? (-10) : 10) * normalDistance;
				if (Physics.Raycast(OriginalPoint, moveToEndPoint - OriginalPoint, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss")) && (OriginalPoint - hitInfo.point).sqrMagnitude < (OriginalPoint - moveToEndPoint).sqrMagnitude)
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
		case LegState.Floating:
			currentEndPoint = monster20.transform.position + Tool2D.GetDir(monster20.MoveDir, leftLeg ? (-90) : 90) * floatingLength + new Vector3(0f, monster20.nowHeight + Mathf.Sin(wingFrequency * (monster20.nowPhase + MathF.PI * 4f / 5f)) * wingAmplitude, 0f);
			break;
		default:
			Debug.LogError(state);
			break;
		}
		Vector3 rootPoint;
		Vector3 vector;
		if (state != LegState.Floating)
		{
			rootPoint = (OriginalPoint + currentEndPoint) / 2f + new Vector3(0f, 0f, 0f - middleHeight);
			vector = OriginalPoint + new Vector3(0f, 0f, 0f - monster20.tsf_Motion.localPosition.y);
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		}
		else
		{
			vector = OriginalPoint + new Vector3(0f, 0f, 0f - monster20.tsf_Motion.localPosition.y);
			rootPoint = (vector + currentEndPoint) / 2f;
			lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector, 1.05f));
			lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(monster20.transform.position + Tool2D.GetDir(monster20.MoveDir, leftLeg ? (-90) : 90) * normalDistance, 1.05f));
		}
		beforeFallShadowPoints[0] = lr_Shadow.GetPosition(0);
		beforeFallShadowPoints[1] = lr_Shadow.GetPosition(1);
		lr_Leg.SetPosition(0, Tool2D.GetLayerPoint(vector));
		lr_Leg.SetPosition(1, Tool2D.GetLayerPoint(rootPoint));
		lr_Leg.SetPosition(2, Tool2D.GetLayerPoint(currentEndPoint));
		beforeFallNodePoints[0] = lr_Leg.GetPosition(0);
		beforeFallNodePoints[1] = lr_Leg.GetPosition(1);
		beforeFallNodePoints[2] = lr_Leg.GetPosition(2);
		if (lr_Leg.startColor != monster20.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster20.myPpt.BaseColor;
			lr_Leg.endColor = monster20.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster20 monster20, bool leftLeg)
	{
		lr_Leg.positionCount = 3;
		this.monster20 = monster20;
		this.leftLeg = leftLeg;
		originWidthMultiplier = lr_Leg.widthMultiplier;
		originShadowWidthMuitiplier = lr_Shadow.widthMultiplier;
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
		lr_Leg.SetPosition(0, Vector3.zero);
		lr_Leg.SetPosition(1, Vector3.zero);
		lr_Leg.SetPosition(2, Vector3.zero);
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
		lr_Leg.enabled = false;
		lr_Shadow.enabled = false;
	}

	public void Frame1Initail()
	{
		lr_Leg.enabled = true;
		lr_Shadow.enabled = true;
		moveToEndPoint = OriginalPoint + Tool2D.GetDir(monster20.MoveDir, leftLeg ? (-10) : 10) * normalDistance;
		currentEndPoint = moveToEndPoint;
		if (state != LegState.Floating)
		{
			state = LegState.Idle;
		}
		LateUpdate();
	}
}
