using System;
using UnityEngine;

public class Elite10_Wing : MonoBehaviour
{
	public enum WingState
	{
		Idle,
		Flap,
		Glide,
		FlapOnce,
		Drop,
		DropLong
	}

	[Header("翅膀基础属性")]
	public LineRenderer lr;

	public int nodeCount;

	public float offset;

	public float basicSegmentLength;

	public float lerp;

	public float minSegmentLength;

	public float maxSegmentLength;

	public float rotateSpeed;

	public float rotateHalfAngle;

	public float nowPhase;

	private float nowSegmentRatio;

	private Elite10 elite10;

	private Vector3 dir;

	private bool isLeft;

	private Vector3[] nodePoints;

	public Material mt_WingRFlipped;

	public Material mt_WingR;

	public Material mt_WingLFlipped;

	public Material mt_WingL;

	public Material mt_WingRFlipped_1;

	public Material mt_WingR_1;

	public Material mt_WingLFlipped_1;

	public Material mt_WingL_1;

	public Material mt_WingRFlipped_H;

	public Material mt_WingR_H;

	public Material mt_WingLFlipped_H;

	public Material mt_WingL_H;

	public Material mt_WingRFlipped_1_H;

	public Material mt_WingR_1_H;

	public Material mt_WingLFlipped_1_H;

	public Material mt_WingL_1_H;

	[Header("翅膀状态机")]
	public WingState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("站立状态")]
	public float idleSpeedFix;

	public float idleAmplitudeFix;

	public float idlePhaseLerpTime;

	public float segmentLengthRatioIdle;

	public float phaseIdle;

	[Header("扇翅膀状态")]
	public float segmentLengthRatioFlap;

	[Header("滑翔状态")]
	public float glidePhaseLerpTime;

	public float segmentLengthRatioGlide;

	public float phaseGlide;

	[Header("扇一次翅膀状态")]
	public float flapOncePhaseLerpTime;

	public float flapOnceTime;

	public AnimationCurve flapOncePhaseCurve;

	public float segmentLengthRatioFlapOnce;

	[Header("下落状态")]
	public float phaseDrop;

	public float dropPhaseLerpTime;

	public float dropMaxTime;

	public float dropLongMaxTime;

	private bool isFlipped;

	public bool SwitchedStage;

	private float nowSegmentLength => basicSegmentLength * nowSegmentRatio;

	public WingState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	private void LateUpdate()
	{
		if (lr.startColor != elite10.myPpt.BaseColor)
		{
			lr.startColor = elite10.myPpt.BaseColor;
			lr.endColor = elite10.myPpt.BaseColor;
		}
		if (SwitchedStage)
		{
			SwitchedStage = false;
			if (isFlipped)
			{
				if (isLeft)
				{
					lr.material = mt_WingLFlipped_1;
				}
				else
				{
					lr.material = mt_WingRFlipped_1;
				}
			}
			else if (isLeft)
			{
				lr.material = mt_WingL_1;
			}
			else
			{
				lr.material = mt_WingR_1;
			}
		}
		if (elite10.inSecondStage || elite10.state == Elite10.MonsterState.SecondStageDrop)
		{
			if (elite10.tsf_FlipRoot.localScale.x < 0f && !isFlipped)
			{
				isFlipped = !isFlipped;
				if (isLeft)
				{
					lr.material = mt_WingLFlipped_1;
				}
				else
				{
					lr.material = mt_WingRFlipped_1;
				}
			}
			else if (elite10.tsf_FlipRoot.localScale.x >= 0f && isFlipped)
			{
				isFlipped = !isFlipped;
				if (isLeft)
				{
					lr.material = mt_WingL_1;
				}
				else
				{
					lr.material = mt_WingR_1;
				}
			}
		}
		else if (elite10.tsf_FlipRoot.localScale.x < 0f && !isFlipped)
		{
			isFlipped = !isFlipped;
			if (isLeft)
			{
				lr.material = mt_WingLFlipped;
			}
			else
			{
				lr.material = mt_WingRFlipped;
			}
		}
		else if (elite10.tsf_FlipRoot.localScale.x >= 0f && isFlipped)
		{
			isFlipped = !isFlipped;
			if (isLeft)
			{
				lr.material = mt_WingL;
			}
			else
			{
				lr.material = mt_WingR;
			}
		}
		if (elite10.myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		default:
			return;
		case WingState.Idle:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				nowSegmentRatio = segmentLengthRatioIdle;
			}
			if (stateExistTime < idlePhaseLerpTime)
			{
				nowPhase = Mathf.Lerp(reference3, phaseIdle * MathF.PI * 2f, stateExistTime / idlePhaseLerpTime);
			}
			else
			{
				nowPhase += Time.deltaTime * rotateSpeed * MathF.PI * 2f * idleSpeedFix;
			}
			break;
		}
		case WingState.Flap:
			if (changedState)
			{
				nowSegmentRatio = segmentLengthRatioFlap;
			}
			nowPhase += Time.deltaTime * rotateSpeed * MathF.PI * 2f;
			if (nowPhase > MathF.PI * 2f)
			{
				nowPhase -= MathF.PI * 2f;
				if (isLeft)
				{
					SEMgr.Inst.elite10WingFlap.PlaySE();
				}
			}
			break;
		case WingState.Glide:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				nowSegmentRatio = segmentLengthRatioGlide;
			}
			nowPhase = Mathf.Lerp(reference, phaseGlide * MathF.PI * 2f, stateExistTime / glidePhaseLerpTime);
			break;
		}
		case WingState.FlapOnce:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.elite10WingFlap.PlaySE();
				nowSegmentRatio = segmentLengthRatioFlapOnce;
			}
			nowPhase = Mathf.Lerp(reference2, flapOncePhaseCurve.Evaluate(stateExistTime / flapOnceTime) * MathF.PI * 2f, stateExistTime / flapOncePhaseLerpTime);
			if (stateExistTime > flapOnceTime)
			{
				state = WingState.Drop;
			}
			break;
		}
		case WingState.Drop:
			nowPhase = Mathf.Lerp(varMgr.RegFloat(0), phaseDrop * MathF.PI * 2f, stateExistTime / dropPhaseLerpTime);
			if (stateExistTime > dropMaxTime)
			{
				state = WingState.Idle;
			}
			break;
		case WingState.DropLong:
			nowPhase = Mathf.Lerp(varMgr.RegFloat(0), phaseDrop * MathF.PI * 2f, stateExistTime / dropPhaseLerpTime);
			if (stateExistTime > dropLongMaxTime)
			{
				state = WingState.Idle;
			}
			break;
		}
		float z = elite10.tsf_FootPoint.position.z + 0.005f;
		float num = Mathf.Sin(nowPhase) * rotateHalfAngle;
		if (state == WingState.Idle)
		{
			num *= idleAmplitudeFix;
		}
		Vector3 vector = Tool2D.GetDir(dir, isLeft ? num : (0f - num));
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = Tool2D.IgnoreZPoint(dir * offset + elite10.tsf_WingPoint.position);
			}
			else
			{
				nodePoints[i] = Tool2D.IgnoreZPoint(Vector3.Lerp(nodePoints[i], nodePoints[i - 1] + vector * nowSegmentLength, lerp * Time.deltaTime));
				if ((nodePoints[i] - nodePoints[i - 1]).sqrMagnitude > maxSegmentLength * maxSegmentLength)
				{
					nodePoints[i] = nodePoints[i - 1] + (nodePoints[i] - nodePoints[i - 1]).normalized * maxSegmentLength;
				}
				else if ((nodePoints[i] - nodePoints[i - 1]).sqrMagnitude < minSegmentLength * minSegmentLength)
				{
					nodePoints[i] = nodePoints[i - 1] + (nodePoints[i] - nodePoints[i - 1]).normalized * minSegmentLength;
				}
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
	}

	public void SingleInitial(Elite10 elite10, Vector3 dir, bool isLeft)
	{
		this.elite10 = elite10;
		this.dir = dir;
		this.isLeft = isLeft;
		lr.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
		if (GameMgr.IsHarmony_Static)
		{
			mt_WingLFlipped = mt_WingLFlipped_H;
			mt_WingL = mt_WingL_H;
			mt_WingR = mt_WingR_H;
			mt_WingRFlipped = mt_WingRFlipped_H;
			mt_WingLFlipped_1 = mt_WingLFlipped_1_H;
			mt_WingL_1 = mt_WingL_1_H;
			mt_WingRFlipped_1 = mt_WingRFlipped_1_H;
			mt_WingR_1 = mt_WingR_1_H;
		}
		isFlipped = elite10.tsf_FlipRoot.localScale.x < 0f;
	}

	public void EveryInitial()
	{
		float z = Tool2D.GetLayerPoint(elite10.transform).z + 0.1f;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = Tool2D.IgnoreZPoint(dir * offset + elite10.tsf_WingPoint.position);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + dir * nowSegmentLength;
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
		state = WingState.Idle;
	}
}
