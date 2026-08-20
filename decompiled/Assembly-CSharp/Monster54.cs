using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Monster54 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Attack,
		MoveToTarget
	}

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("腿")]
	public Transform tsf_Motion;

	private List<Monster54_Leg> legs = new List<Monster54_Leg>();

	public Monster54_Leg pfb_Monster1Leg;

	public Transform headAdjust;

	[Header("随机移动")]
	public VariableFloat randomMoveRadius;

	public VariableFloat randomMoveTime;

	[Header("跟随目标")]
	public float followDistance;

	[Header("攻击")]
	public VariableFloat attackDistance;

	public VariableFloat attackCD;

	private float attackCDTimer;

	private bool attacking;

	public int attackTime;

	private int attackCounter;

	public VariableFloat attackBallRadius;

	public float attackHeight;

	[Header("二模式")]
	public AIPattern pattern;

	[Header("和谐")]
	public MeshRenderer MR;

	public Sprite sprite_H;

	public Light2D light2d;

	public GameObject particle;

	public GameObject particle_H;

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
		}
	}

	public bool IsMove
	{
		get
		{
			if (state != MonsterState.RandomMove)
			{
				return state == MonsterState.MoveToTarget;
			}
			return true;
		}
	}

	public override void SingleInitialCallback()
	{
		for (int i = 0; i < 6; i++)
		{
			legs.Add(Object.Instantiate(pfb_Monster1Leg, base.transform));
			float degree = 30f;
			switch (i)
			{
			case 1:
				degree = 90f;
				break;
			case 2:
				degree = 150f;
				break;
			case 3:
				degree = 210f;
				break;
			case 4:
				degree = 270f;
				break;
			case 5:
				degree = 330f;
				break;
			}
			legs[i].SingleInitial(this, Tool2D.GetDir(degree));
		}
		if (pattern == AIPattern.Pattern1)
		{
			if (GameMgr.IsHarmony_Static)
			{
				MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_H.texture);
				light2d.color = Color.magenta;
				particle.SetActive(value: false);
				particle_H.SetActive(value: true);
			}
			else
			{
				particle.SetActive(value: true);
				particle_H.SetActive(value: false);
			}
		}
	}

	public override void EveryInitialCallback()
	{
		attackCD.RandomResult();
		for (int i = 0; i < 6; i++)
		{
			legs[i].EveryInitial();
		}
		attacking = false;
		state = MonsterState.BornIdle;
		attackCDTimer = Random.Range(0f, attackCD.value2);
	}

	public override void Update()
	{
		headAdjust.localPosition = new Vector3(0f, 0f, (0f - tsf_Motion.localPosition.y) * 0.01f);
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Monster54_Idle");
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				if (!attacking)
				{
					base.Anima.Play("Monster54_Move");
				}
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
				checkTargetIntervalTimer = 0f;
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < followDistance * followDistance)
			{
				state = MonsterState.MoveToTarget;
			}
			if (navInfo.allCornerArrived || stateExistTime > randomMoveTime.result)
			{
				stateExistTime = 0f;
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			MoveAttack();
			break;
		case MonsterState.MoveToTarget:
			if (changedState && !attacking)
			{
				base.Anima.Play("Monster54_Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			if (navInfo.allCornerArrived || stateExistTime > randomMoveTime.result)
			{
				stateExistTime = 0f;
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, attackDistance, -ToTargetDir(), 30f));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			MoveAttack();
			break;
		case MonsterState.Idle:
		case MonsterState.Attack:
			break;
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < legs.Count; i++)
		{
			legs[i].Theme6Reposition(changeValue);
		}
		base.Theme6Reposition(changeValue);
	}

	public void MoveAttack()
	{
		if (!attacking)
		{
			attackCDTimer += Time.deltaTime;
		}
		if (attackCDTimer > attackCD.result)
		{
			attackCD.RandomResult();
			attackCDTimer = 0f;
			base.Anima.Play("Monster54_Attack");
			attackCounter = 0;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "AttackStart":
			attacking = true;
			break;
		case "Attack":
		{
			Vector3 from = Tool2D.GetDir();
			if (base.HaveTarget)
			{
				from = ToTargetDir();
			}
			Vector3 navMeshPoint = Tool2D.GetNavMeshPoint(base.transform.position, attackBallRadius, from, 100f);
			SEMgr.Inst.monster54_Attack.PlaySE();
			if (pattern == AIPattern.Pattern1)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_DelayLaser" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 6f).GetComponent<Monster54_DelayLaser>().Initialize(navMeshPoint, attackHeight, myPpt);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_DelayLaserCurved", base.transform.position, 8f).GetComponent<Monster54_DelayLaser>().Initialize(navMeshPoint, attackHeight, myPpt);
			}
			break;
		}
		case "AttackFinish":
			attackCounter++;
			if (attackCounter == attackTime)
			{
				base.Anima.Play("Monster54_Move");
				attacking = false;
			}
			else
			{
				base.Anima.Play("Monster54_Attack", 0, 0f);
			}
			break;
		}
	}
}
