using System;
using UnityEngine;

public class Monster14_Leg : MonoBehaviour
{
	private enum LegState
	{
		Idle,
		Back,
		Out
	}

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int count;

	public int totalNodeCount;

	public float flexSpeed;

	[Header("Important")]
	public VariableFloat outDistance;

	public float correctDistance;

	public float tailEndOffset;

	public float tailLength;

	public float randomDirThreshold;

	[Header("MiddlePoint")]
	public float middlePointHeight;

	public float middlePointAngleDistance;

	public VariableFloat middlePointAngleSpeed;

	[Header("safe mode")]
	public Material Mt_originLeg;

	public Material Mt_safeLeg;

	private Monster14 monster14;

	private LineRenderer[] lr_Legs;

	private LineRenderer[] lr_Shadows;

	private LegState state = LegState.Out;

	private float[] middlePointRotateSpeeds;

	private float[] middlePointAngleValues;

	private Vector3[] endPointOffsets;

	private Vector3 moveToEndPoint;

	private float moveToPointDistance;

	private float currentEndPointLerp;

	private bool canChangeState = true;

	private bool isThisOutIsHang;

	private Vector3 RootPoint => monster14.transform.position + monster14.CurrentMotionLocalPoint;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			for (int i = 0; i < lr_Legs.Length; i++)
			{
				UnityEngine.Object.Destroy(lr_Legs[i].material);
				lr_Legs[i].material = Mt_safeLeg;
			}
		}
		else
		{
			for (int j = 0; j < lr_Legs.Length; j++)
			{
				UnityEngine.Object.Destroy(lr_Legs[j].material);
				lr_Legs[j].material = Mt_originLeg;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < middlePointAngleValues.Length; i++)
		{
			middlePointAngleValues[i] += middlePointRotateSpeeds[i] * Time.deltaTime;
		}
		switch (state)
		{
		case LegState.Idle:
			if ((RootPoint - moveToEndPoint).sqrMagnitude > correctDistance * correctDistance && canChangeState)
			{
				state = LegState.Back;
			}
			break;
		case LegState.Back:
			currentEndPointLerp = Mathf.MoveTowards(currentEndPointLerp, 0f, flexSpeed * Time.deltaTime);
			if (currentEndPointLerp == 0f)
			{
				state = LegState.Out;
				moveToPointDistance = outDistance.RandomResult();
				float sqrMagnitude = monster14.CurrentMotion.sqrMagnitude;
				float sqrMagnitude2 = monster14.myPpt.Rigid.linearVelocity.sqrMagnitude;
				float num = randomDirThreshold * randomDirThreshold;
				if (sqrMagnitude < num && sqrMagnitude2 < num)
				{
					moveToEndPoint = monster14.transform.position + Tool2D.GetDir() * moveToPointDistance;
				}
				else if (sqrMagnitude > sqrMagnitude2)
				{
					moveToEndPoint = monster14.transform.position + Tool2D.GetDir(monster14.CurrentMotion.normalized, UnityEngine.Random.Range(-60f, 60f)) * moveToPointDistance;
				}
				else
				{
					moveToEndPoint = monster14.transform.position + Tool2D.GetDir(monster14.myPpt.Rigid.linearVelocity.normalized, UnityEngine.Random.Range(-60f, 60f)) * moveToPointDistance;
				}
				if (Physics.Raycast(monster14.transform.position, moveToEndPoint - monster14.transform.position, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss")) && (monster14.transform.position - hitInfo.point).sqrMagnitude < (monster14.transform.position - moveToEndPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
				}
				if (monster14.IsHang)
				{
					isThisOutIsHang = true;
					moveToEndPoint = new Vector3(moveToEndPoint.x, moveToEndPoint.y, monster14.CurrentMotionLocalPoint.z + monster14.OriginalMotionLocalPoint.z * 2f - moveToEndPoint.z);
				}
				else
				{
					isThisOutIsHang = false;
				}
			}
			break;
		case LegState.Out:
			currentEndPointLerp = Mathf.MoveTowards(currentEndPointLerp, 1f, flexSpeed * monster14.MoveRatio * Time.deltaTime);
			if (currentEndPointLerp == 1f)
			{
				state = LegState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		for (int j = 0; j < lr_Legs.Length; j++)
		{
			float z = (isThisOutIsHang ? middlePointHeight : (0f - middlePointHeight));
			Vector3 v = (RootPoint + moveToEndPoint + endPointOffsets[j]) / 2f + new Vector3(0f, 0f, z) + Tool2D.GetDir(middlePointAngleValues[j]) * middlePointAngleDistance;
			for (int k = 0; k < totalNodeCount; k++)
			{
				float num2 = (float)k / ((float)totalNodeCount - 1f) * currentEndPointLerp;
				float num3 = moveToPointDistance / (moveToPointDistance + tailLength);
				Vector3 zero = Vector3.zero;
				if (num2 <= num3)
				{
					float t = (moveToPointDistance + tailLength) * num2 / moveToPointDistance;
					zero = GeneralTool.QuadraticBezierCurve(RootPoint, v, moveToEndPoint + endPointOffsets[j], t);
				}
				else
				{
					float t2 = ((moveToPointDistance + tailLength) * num2 - moveToPointDistance) / tailLength;
					zero = Vector3.Lerp(moveToEndPoint + endPointOffsets[j], moveToEndPoint + endPointOffsets[j] + endPointOffsets[j].normalized * tailLength, t2);
				}
				lr_Legs[j].SetPosition(k, Tool2D.GetLayerPoint(zero));
				lr_Shadows[j].SetPosition(k, Tool2D.IgnoreZPoint(zero, 1.05f));
			}
		}
		if (lr_Legs[0].startColor != monster14.myPpt.BaseColor)
		{
			for (int l = 0; l < lr_Legs.Length; l++)
			{
				lr_Legs[l].startColor = monster14.myPpt.BaseColor;
				lr_Legs[l].endColor = monster14.myPpt.BaseColor;
			}
		}
	}

	public void SingleInitial(Monster14 monster14)
	{
		this.monster14 = monster14;
		lr_Leg.positionCount = totalNodeCount;
		lr_Shadow.positionCount = totalNodeCount;
		lr_Legs = new LineRenderer[count];
		lr_Shadows = new LineRenderer[count];
		lr_Legs[0] = lr_Leg;
		lr_Shadows[0] = lr_Shadow;
		for (int i = 1; i < count; i++)
		{
			lr_Legs[i] = UnityEngine.Object.Instantiate(lr_Leg, base.transform);
			lr_Shadows[i] = UnityEngine.Object.Instantiate(lr_Shadow, base.transform);
		}
	}

	public void EveryInitial()
	{
		state = LegState.Out;
		currentEndPointLerp = 0f;
		canChangeState = true;
		isThisOutIsHang = false;
		middlePointAngleValues = new float[count];
		middlePointRotateSpeeds = new float[count];
		for (int i = 0; i < count; i++)
		{
			middlePointRotateSpeeds[i] = middlePointAngleSpeed.RandomResult();
		}
		moveToPointDistance = outDistance.RandomResult();
		moveToEndPoint = monster14.transform.position + Tool2D.GetDir() * moveToPointDistance;
		if (Physics.Raycast(monster14.transform.position, moveToEndPoint - monster14.transform.position, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss")) && (monster14.transform.position - hitInfo.point).sqrMagnitude < (monster14.transform.position - moveToEndPoint).sqrMagnitude)
		{
			moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
		}
		endPointOffsets = new Vector3[lr_Legs.Length];
		for (int j = 0; j < endPointOffsets.Length; j++)
		{
			endPointOffsets[j] = Tool2D.GetDir() * UnityEngine.Random.Range(0f, tailEndOffset);
		}
		SetSafeMode();
		Update();
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		moveToEndPoint += changeValue;
	}

	public void CanChangeState(bool canChange)
	{
		canChangeState = canChange;
	}
}
