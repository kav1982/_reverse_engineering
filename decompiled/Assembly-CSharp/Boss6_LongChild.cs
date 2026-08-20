using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_LongChild : UnitBase
{
	public enum MonsterState
	{
		Enter,
		Move,
		Attack
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("运动")]
	public float maxRotateAngle;

	public float waveAngle;

	public float waveSpeed;

	public VariableFloat repositionDistance;

	public float forceRepositionTime;

	public float nextPointDistance;

	private Vector3 targetPoint;

	private Vector3 baseDir;

	private Vector3 finalDir;

	private float waveAngleStartPhase;

	public float chanceToChaseTarget;

	[Header("入场")]
	public float enterSpeedFix;

	public VariableFloat enterFromCenterDistance;

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	[Header("体节")]
	public Transform tsf_Body;

	public Transform tsf_BodyShadow;

	public float bodyHeight;

	public float bodyInterval;

	public int bodyCount;

	public List<Vector3> recordPoints = new List<Vector3>();

	public float recordPointInterval;

	private float headFromFirstRecordPoint;

	public float legWaveAngle;

	public float legWaveOffset;

	public float legWaveSpeed;

	[Header("嘴")]
	public float handWaveAngle;

	public Transform tsf_LeftHand;

	public Transform tsf_RightHand;

	public Transform tsf_LeftLeg;

	public Transform tsf_RightLeg;

	public Transform tsf_LeftHandShadow;

	public Transform tsf_RightHandShadow;

	public Transform tsf_LeftLegShadow;

	public Transform tsf_RightLegShadow;

	public List<SpriteRenderer> SRs_Shadow;

	public Color shadowColor;

	[Header("伤害共享")]
	public List<SpellBase> hitList = new List<SpellBase>();

	private List<Boss6_LongChildBody> bodys = new List<Boss6_LongChildBody>();

	public float hitListClearTime;

	private float hitListClearTimer;

	public MonsterState state
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

	public void Initialize(float enterDistanceX, float enterDistanceY)
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		baseDir = Tool2D.GetDirByFourDir(Tool2D.GetRandomFourDir());
		if (baseDir == Vector3.up || baseDir == Vector3.down)
		{
			base.transform.position = roomCenter + UnityEngine.Random.Range(-0.4f, 0.4f) * Vector3.right * roomWidth - baseDir * (roomHeight / 2f + enterDistanceY);
		}
		else
		{
			base.transform.position = roomCenter + UnityEngine.Random.Range(-0.4f, 0.4f) * Vector3.up * roomHeight - baseDir * (roomWidth / 2f + enterDistanceX);
		}
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		for (int i = 0; i < SRs_Shadow.Count; i++)
		{
			SRs_Shadow[i].color = shadowColor;
			myPpt.RemoveSRFromArray(SRs_Shadow[i]);
		}
	}

	public override void EveryInitialCallback()
	{
		recordPoints.Clear();
		int num = Mathf.CeilToInt((float)(bodyCount + 3) * bodyInterval / recordPointInterval);
		for (int i = 0; i < num; i++)
		{
			recordPoints.Add(base.transform.position);
		}
		bodys.Clear();
		for (int j = 0; j < bodyCount; j++)
		{
			Boss6_LongChildBody component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500652, base.transform.position).GetComponent<Boss6_LongChildBody>();
			bodys.Add(component);
			component.master = this;
			component.hitList = hitList;
			if (j == bodyCount - 1)
			{
				component.SetTail(isTail: true);
			}
			else
			{
				component.SetTail(isTail: false);
			}
		}
		waveAngleStartPhase = UnityEngine.Random.Range(0f, MathF.PI * 2f);
		tsf_Body.localPosition = new Vector3(0f, bodyHeight, 0f - bodyHeight);
		baseDir = Tool2D.GetDir();
		state = MonsterState.Enter;
	}

	public Vector3 GetPositionByBody(Boss6_LongChildBody body, out Vector3 dir)
	{
		float num = (float)(bodys.IndexOf(body) + 1) * bodyInterval;
		int num2 = Mathf.CeilToInt((num - headFromFirstRecordPoint) / recordPointInterval);
		if (num2 <= 0)
		{
			num2 = 1;
		}
		float num3 = headFromFirstRecordPoint + (float)num2 * recordPointInterval - num;
		if (num3 > recordPointInterval)
		{
			Debug.Log("!");
		}
		dir = (recordPoints[num2 - 1] - recordPoints[num2]).normalized;
		return recordPoints[num2] - (recordPoints[num2] - recordPoints[num2 - 1]).normalized * num3;
	}

	public override void Update()
	{
		hitListClearTimer += Time.deltaTime;
		if (hitListClearTimer > hitListClearTime)
		{
			hitListClearTimer = 0f;
			hitList.Clear();
		}
		base.Update();
		if (base.IsLocked)
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
		baseDir = Tool2D.RotateTowardsAroundZAxis(baseDir, ToPointDir(targetPoint), Time.deltaTime * ((state == MonsterState.Enter) ? enterSpeedFix : 1f) * base.MoveSpeed * maxRotateAngle);
		float degree = Mathf.Sin(Time.time * waveSpeed * (MathF.PI / 180f) + waveAngleStartPhase) * waveAngle;
		finalDir = Tool2D.GetDir(baseDir, degree).normalized;
		SetMove(finalDir * ((state == MonsterState.Enter) ? enterSpeedFix : 1f) * base.MoveSpeed, isFlip: false);
		tsf_Body.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, finalDir));
		float magnitude = (base.transform.position - recordPoints[0]).magnitude;
		if (headFromFirstRecordPoint > magnitude)
		{
			headFromFirstRecordPoint += 0.02f;
			base.transform.position = recordPoints[0] + (base.transform.position - recordPoints[0]).normalized * headFromFirstRecordPoint;
		}
		else
		{
			headFromFirstRecordPoint = magnitude;
		}
		while (headFromFirstRecordPoint > recordPointInterval)
		{
			for (int num = recordPoints.Count - 1; num > 0; num--)
			{
				recordPoints[num] = recordPoints[num - 1];
			}
			recordPoints[0] = recordPoints[0] + (base.transform.position - recordPoints[0]).normalized * recordPointInterval;
			headFromFirstRecordPoint -= recordPointInterval;
		}
		float num2 = handWaveAngle * Mathf.Sin(MathF.PI / 180f * Time.time * legWaveSpeed - MathF.PI / 180f * legWaveOffset);
		tsf_LeftLeg.localEulerAngles = new Vector3(0f, 0f, num2);
		tsf_RightLeg.localEulerAngles = new Vector3(0f, 0f, 0f - num2);
		tsf_LeftHand.localEulerAngles = new Vector3(0f, 0f, num2);
		tsf_RightHand.localEulerAngles = new Vector3(0f, 0f, 0f - num2);
		tsf_LeftLegShadow.localEulerAngles = new Vector3(0f, 0f, num2);
		tsf_RightLegShadow.localEulerAngles = new Vector3(0f, 0f, 0f - num2);
		tsf_LeftHandShadow.localEulerAngles = new Vector3(0f, 0f, num2);
		tsf_RightHandShadow.localEulerAngles = new Vector3(0f, 0f, 0f - num2);
		tsf_BodyShadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		for (int i = 0; i < bodyCount; i++)
		{
			Vector3 dir;
			Vector3 positionByBody = GetPositionByBody(bodys[i], out dir);
			bodys[i].SetPositionAndDir(positionByBody, dir);
			float rotateAngle = legWaveAngle * Mathf.Sin(MathF.PI / 180f * Time.time * legWaveSpeed + (float)i * (MathF.PI / 180f) * legWaveOffset);
			bodys[i].SetHandDir(dir, rotateAngle);
			bodys[i].SetColor(myPpt.BaseColor);
		}
		switch (state)
		{
		case MonsterState.Enter:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				reference = roomCenter - enterFromCenterDistance.RandomResult() * (roomCenter - base.transform.position).normalized;
				targetPoint = Tool2D.GetNavMeshPoint(reference);
			}
			if (ToPointDistanceSqr(targetPoint) < nextPointDistance * nextPointDistance)
			{
				state = MonsterState.Move;
			}
			break;
		}
		case MonsterState.Move:
			if (changedState)
			{
				repositionDistance.RandomResult();
				GetNearestTarget();
				if (GeneralTool.ChanceResult(chanceToChaseTarget) && base.HaveTarget)
				{
					targetPoint = base.TargetPoint + Tool2D.GetDir() * repositionDistance.result;
				}
				else
				{
					targetPoint = base.transform.position + Tool2D.GetDir() * repositionDistance.result;
				}
				targetPoint.x = Mathf.Clamp(targetPoint.x, (0f - roomWidth) * 0.5f + roomCenter.x, roomWidth * 0.5f + roomCenter.x);
				targetPoint.y = Mathf.Clamp(targetPoint.y, (0f - roomHeight) * 0.5f + roomCenter.y, roomHeight * 0.5f + roomCenter.y);
			}
			if (stateExistTime > forceRepositionTime)
			{
				state = MonsterState.Move;
			}
			else if (ToPointDistanceSqr(targetPoint) < nextPointDistance * nextPointDistance)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Attack:
			_ = changedState;
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < bodyCount; i++)
		{
			bodys[i].DotsAnnouncedDeath();
		}
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		if (info.spellBase != null)
		{
			if (!hitList.Contains(info.spellBase))
			{
				hitList.Add(info.spellBase);
			}
			else
			{
				info.immuneDamage = true;
			}
			return;
		}
		Vector3 dir = Tool2D.GetDir();
		for (int i = 0; i < bodys.Count; i++)
		{
			bodys[i].myPpt.TakeBeHit(dir);
		}
	}
}
