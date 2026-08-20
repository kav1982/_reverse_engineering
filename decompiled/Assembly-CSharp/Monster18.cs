using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster18 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		Amaze,
		RunToTarget
	}

	public float followDistance;

	public float keepDistanceToTarget;

	public float warningRadius;

	public float warningInterval;

	[Header("Head")]
	public Animator anima_Head;

	public AnimaEvent ae_Head;

	public Transform tsf_Head;

	public Transform tsf_HeadSR;

	public float headHeight;

	public float headRecoveryLerp;

	[Header("Neck")]
	public LineRenderer lr_Neck;

	public int neckNodeCount;

	public float neckMiddlePointDistace;

	[Header("Tail")]
	public LineRenderer lr_Tail;

	public int tailNodeCount;

	public float tailNodeDistance;

	[Range(0f, 180f)]
	public float tailSwiggleAngle;

	public float tailSwiggleSpeed;

	[Header("Shadow")]
	public LineRenderer lr_Shadow;

	public float shadowYOffset;

	public AIPattern pattern;

	[Range(0f, 1f)]
	public float dropFaceChance;

	public int faceID;

	[Header("安全模式")]
	public SpriteRenderer SR_OriginHead;

	public SpriteRenderer SR_SafeHead;

	[Header("掉悬崖")]
	public Transform tsf_Body;

	public float fallTailSpeed;

	private float fallCounter;

	private float shadowOriginMultiplier;

	private float neckOriginMultiplier;

	private float tailOriginMultiplier;

	private MonsterState state;

	private float warningIntervalTimer;

	private List<Vector3> tailPoints = new List<Vector3>();

	private Vector3 headPoint;

	private Vector3 lastPoint;

	private float currentSwiggleValue;

	public static List<Monster18> mates = new List<Monster18>();

	public static bool listChecked = false;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode || GameMgr.IsHarmony_Static)
		{
			SR_OriginHead.gameObject.SetActive(value: false);
			SR_SafeHead.gameObject.SetActive(value: true);
		}
		else
		{
			SR_OriginHead.gameObject.SetActive(value: true);
			SR_SafeHead.gameObject.SetActive(value: false);
		}
	}

	public override void SingleInitialCallback()
	{
		ae_Head.DoAction = AnimaActionHead;
		lr_Neck.positionCount = neckNodeCount;
		lr_Tail.positionCount = tailNodeCount;
		lr_Shadow.positionCount = tailNodeCount;
		shadowOriginMultiplier = lr_Shadow.widthMultiplier;
		neckOriginMultiplier = lr_Neck.widthMultiplier;
		tailOriginMultiplier = lr_Tail.widthMultiplier;
	}

	public override void EveryInitialCallback()
	{
		mates.Add(this);
		state = MonsterState.BornIdle;
		warningIntervalTimer = 0f;
		lastPoint = base.transform.position;
		currentSwiggleValue = 0f;
		headPoint = base.transform.position + new Vector3(0f, headHeight, 0f);
		tsf_Head.position = headPoint;
		Vector3 dir = Tool2D.GetDir(90f);
		tailPoints.Clear();
		tailPoints.Add(base.transform.position);
		for (int i = 1; i < tailNodeCount; i++)
		{
			tailPoints.Add(tailPoints[i - 1] + dir * tailNodeDistance);
			dir = Tool2D.GetDir(dir, -450f / (float)tailNodeCount + (float)i * 1.5f);
		}
		CorrectBody();
		fallCounter = 0f;
	}

	private void Performance()
	{
		if (myPpt.Affect_InAbyss)
		{
			fallCounter += Time.deltaTime * fallTailSpeed;
			if (fallCounter > tailNodeDistance)
			{
				fallCounter = 0f;
				lastPoint = base.transform.position;
				tailPoints.RemoveAt(tailPoints.Count - 1);
				tailPoints.Insert(1, (base.transform.position + tailPoints[1]) / 2f);
			}
		}
		float x = base.transform.lossyScale.x;
		lr_Neck.widthMultiplier = neckOriginMultiplier * x;
		lr_Tail.widthMultiplier = tailOriginMultiplier * x;
		lr_Shadow.widthMultiplier = shadowOriginMultiplier * x;
		headPoint = Vector3.Lerp(headPoint, base.transform.position + new Vector3(0f, headHeight, 0f) * x, headRecoveryLerp * Time.deltaTime);
		tsf_Head.position = headPoint;
		tsf_HeadSR.transform.position = new Vector3(tsf_Head.transform.position.x, tsf_Head.transform.position.y, Tool2D.GetLayerPoint(base.transform).z - 0.01f);
		tailPoints[0] = base.transform.position;
		if ((base.transform.position - lastPoint).sqrMagnitude > tailNodeDistance * tailNodeDistance)
		{
			lastPoint = base.transform.position;
			tailPoints.RemoveAt(tailPoints.Count - 1);
			tailPoints.Insert(1, (base.transform.position + tailPoints[1]) / 2f);
		}
		if (lr_Neck.startColor != myPpt.BaseColor)
		{
			lr_Neck.startColor = myPpt.BaseColor;
			lr_Neck.endColor = myPpt.BaseColor;
			lr_Tail.startColor = myPpt.BaseColor;
			lr_Tail.endColor = myPpt.BaseColor;
		}
		CorrectBody();
	}

	private void CorrectBody()
	{
		Vector3 position = tsf_Head.position;
		Vector3 vector = tailPoints[0];
		Vector3 v = (position + vector) / 2f + (tailPoints[0] - tailPoints[1]).normalized * neckMiddlePointDistace;
		for (int i = 0; i < neckNodeCount; i++)
		{
			Vector3 v2 = GeneralTool.QuadraticBezierCurve(position, v, vector, (float)i / ((float)neckNodeCount - 1f));
			lr_Neck.SetPosition(i, Tool2D.IgnoreZPoint(v2, 0.01f * base.transform.position.y));
		}
		for (int j = 0; j < tailNodeCount; j++)
		{
			lr_Tail.SetPosition(j, Tool2D.IgnoreZPoint(tailPoints[j], 0.01f * base.transform.position.y + 0.001f));
			lr_Shadow.SetPosition(j, Tool2D.IgnoreZPoint(tailPoints[j], 1.05f) + new Vector3(0f, shadowYOffset));
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < followDistance * followDistance)
				{
					state = MonsterState.Amaze;
					anima_Head.SetTrigger("Amaze");
				}
			}
			break;
		case MonsterState.Amaze:
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.RunToTarget:
		{
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					anima_Head.SetTrigger("CloseEye");
					break;
				}
			}
			warningIntervalTimer += Time.deltaTime;
			if (warningIntervalTimer >= 0f)
			{
				warningIntervalTimer = 0f;
				WarningPeer();
			}
			GetNavInfo(base.TargetPoint);
			if (ToTargetDistanceSqr() < keepDistanceToTarget * keepDistanceToTarget)
			{
				SetMove(Vector3.zero);
				break;
			}
			currentSwiggleValue += tailSwiggleSpeed * Time.deltaTime;
			Vector3 oldDir = ToPointDir(navInfo.ToGoPoint);
			oldDir = Tool2D.GetDir(oldDir, Mathf.Sin(currentSwiggleValue) * tailSwiggleAngle / 2f);
			SetMove(oldDir * base.MoveSpeed);
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void LateUpdate()
	{
		Performance();
	}

	private void WarningPeer()
	{
		for (int num = mates.Count - 1; num >= 0; num--)
		{
			if (mates[num] == null || !mates[num].gameObject.activeSelf)
			{
				mates.RemoveAt(num);
			}
		}
		for (int i = 0; i < mates.Count; i++)
		{
			if ((base.transform.position - mates[i].transform.position).sqrMagnitude < warningRadius * warningRadius)
			{
				mates[i].Warning(targetEntity);
			}
		}
	}

	private void AnimaActionHead(string animaName)
	{
		if (animaName == "AmazeFinish")
		{
			state = MonsterState.RunToTarget;
			if (base.HaveTarget)
			{
				WarningPeer();
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (EntityIsValid(info.attackerEntity) && (state == MonsterState.Idle || state == MonsterState.BornIdle) && GetComponentData<UnitProperty_Dots>(info.attackerEntity).unitCfg.IsSameCamp(UnitType.Player))
		{
			targetEntity = info.attackerEntity;
			state = MonsterState.Amaze;
			anima_Head.SetTrigger("Amaze");
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		mates.Remove(this);
		if (pattern == AIPattern.Pattern2 && UnityEngine.Random.value <= dropFaceChance)
		{
			Monster18_Face component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + faceID, base.transform.position).GetComponent<Monster18_Face>();
			UnitProperty_Dots componentData = component.GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(myPpt.Rigid.linearVelocity);
			component.SetComponentData(componentData);
		}
	}

	public void Warning(Entity entity)
	{
		if (state == MonsterState.Idle || state == MonsterState.BornIdle)
		{
			targetEntity = entity;
			state = MonsterState.Amaze;
			anima_Head.SetTrigger("Amaze");
		}
	}
}
