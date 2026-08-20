using System.Collections.Generic;
using UnityEngine;

public class Boss4_Leech : UnitBase
{
	private enum MonsterState
	{
		Jump,
		Idle,
		RandomMove,
		FollowTarget
	}

	public float keepDistanceToTarget;

	public VariableFloat seInterval;

	[Header("Jump")]
	public float jumpForwardForce;

	public float jumpUpForce;

	public float jumpGravity;

	[Header("Body")]
	public LineRenderer lr_Body;

	public LineRenderer lr_Shadow;

	public int bodyNodeCount;

	public float bodyNodeSegment;

	[Header("RandomMove")]
	public VariableFloat randomMoveDistance;

	[Range(0f, 180f)]
	public float tailSwiggleAngle;

	public float tailSwiggleSpeed;

	public float shadowYOffset;

	[Header("Stage3")]
	public AIPattern pattern;

	public float stage3ZOffset;

	public GameObject Head;

	public float headOffset;

	private MonsterState state;

	private Boss4 boss4;

	private List<Vector3> bodyPoints = new List<Vector3>();

	private Vector3 lastPoint;

	private float currentSwiggleValue;

	private bool isStage3;

	private float seIntervalTimer;

	private Vector3 randomMovePoint;

	private Vector3 roomCenter;

	private Vector3 roomScale;

	public override void SingleInitialCallback()
	{
		lr_Body.positionCount = bodyNodeCount;
		lr_Shadow.positionCount = bodyNodeCount;
		for (int i = 0; i < bodyNodeCount; i++)
		{
			bodyPoints.Add(base.transform.position);
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.Jump;
		for (int i = 0; i < bodyPoints.Count; i++)
		{
			bodyPoints[i] = base.transform.position;
		}
		lastPoint = base.transform.position;
		isStage3 = false;
		Performance();
		seInterval.RandomResult();
		if (GameMgr.IsMobile_Static)
		{
			myPpt.unitCfg.moveSpeed *= 0.9f;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.moveSpeed *= 0.9f;
			SetComponentData(componentData);
		}
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomScale = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			Performance();
			return;
		}
		seIntervalTimer += Time.deltaTime;
		if (seIntervalTimer >= seInterval.result)
		{
			seIntervalTimer = 0f;
			seInterval.RandomResult();
			SEMgr.Inst.boss4_Child1.PlaySE();
		}
		switch (state)
		{
		case MonsterState.Jump:
			if (base.transform.position.z > 0f && base.isFalling)
			{
				randomMovePoint = base.transform.position;
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.FollowTarget;
				}
				else
				{
					state = MonsterState.RandomMove;
				}
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.FollowTarget;
				}
			}
			break;
		case MonsterState.RandomMove:
		{
			currentSwiggleValue += tailSwiggleSpeed * Time.deltaTime;
			Vector3 oldDir2 = ToPointDir(randomMovePoint);
			oldDir2 = Tool2D.GetDir(oldDir2, Mathf.Sin(currentSwiggleValue) * tailSwiggleAngle / 2f);
			SetMove(oldDir2 * base.MoveSpeed);
			if (ToPointDistanceSqr(randomMovePoint) < 0.25f)
			{
				randomMovePoint = Tool2D.GetDir() * randomMoveDistance.RandomResult() + base.transform.position;
				randomMovePoint.x = Mathf.Clamp(randomMovePoint.x, roomCenter.x - roomScale.x / 2f + 0.5f, roomCenter.x + roomScale.x / 2f - 0.5f);
				randomMovePoint.y = Mathf.Clamp(randomMovePoint.y, roomCenter.y - roomScale.y / 2f + 0.5f, roomCenter.y + roomScale.y / 2f - 0.5f);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.FollowTarget;
				}
			}
			break;
		}
		case MonsterState.FollowTarget:
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
					state = MonsterState.RandomMove;
					return;
				}
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
		Performance();
	}

	private void Performance()
	{
		if (pattern == AIPattern.Pattern2)
		{
			Head.transform.position = Tool2D.GetLayerPoint(bodyPoints[0]) + new Vector3(0f, 0f, 0f - headOffset);
			if (base.CurrentMotion != Vector3.zero && state != 0)
			{
				Head.transform.right = -base.CurrentMotion;
			}
			else
			{
				Head.transform.right = -Tool2D.IgnoreZPoint(myPpt.Rigid.linearVelocity);
			}
		}
		bodyPoints[0] = base.transform.position;
		if ((base.transform.position - lastPoint).sqrMagnitude > bodyNodeSegment * bodyNodeSegment)
		{
			lastPoint = base.transform.position;
			bodyPoints.RemoveAt(bodyPoints.Count - 1);
			bodyPoints.Insert(1, (bodyPoints[0] + bodyPoints[1]) / 2f);
		}
		for (int i = 0; i < bodyNodeCount; i++)
		{
			if (isStage3 && state == MonsterState.Jump)
			{
				lr_Body.SetPosition(i, Tool2D.GetLayerPoint(bodyPoints[i]) + new Vector3(0f, 0f, stage3ZOffset));
			}
			else
			{
				lr_Body.SetPosition(i, Tool2D.GetLayerPoint(bodyPoints[i]));
			}
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(bodyPoints[i], 1.05f) + new Vector3(0f, shadowYOffset));
			if (i == bodyNodeCount - 1)
			{
				if (isStage3 && state == MonsterState.Jump)
				{
					lr_Body.SetPosition(i, Tool2D.GetLayerPoint((bodyPoints[i] - bodyPoints[i - 1]).normalized * (bodyNodeSegment - (base.transform.position - lastPoint).magnitude) + bodyPoints[i - 1]) + new Vector3(0f, 0f, stage3ZOffset));
				}
				else
				{
					lr_Body.SetPosition(i, Tool2D.GetLayerPoint((bodyPoints[i] - bodyPoints[i - 1]).normalized * (bodyNodeSegment - (base.transform.position - lastPoint).magnitude) + bodyPoints[i - 1]));
				}
				lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint((bodyPoints[i] - bodyPoints[i - 1]).normalized * (bodyNodeSegment - (base.transform.position - lastPoint).magnitude) + bodyPoints[i - 1], 1.05f) + new Vector3(0f, shadowYOffset));
			}
		}
		if (lr_Body.startColor != myPpt.BaseColor)
		{
			lr_Body.startColor = myPpt.BaseColor;
			lr_Body.endColor = myPpt.BaseColor;
		}
	}

	public void Fly(Boss4 boss4, Vector3 dir, bool isStage3 = false)
	{
		this.boss4 = boss4;
		this.isStage3 = isStage3;
		myPpt.Rigid.linearVelocity = dir * jumpForwardForce;
		SyncDotsVelocity();
		JumpStart_Dots(jumpUpForce, jumpGravity);
		SyncDotsPosition();
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < bodyPoints.Count; i++)
		{
			bodyPoints[i] += changeValue;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (isStage3)
		{
			boss4.Stage3LeechDead();
		}
	}
}
