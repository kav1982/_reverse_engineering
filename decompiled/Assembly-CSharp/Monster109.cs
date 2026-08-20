using UnityEngine;

public class Monster109 : UnitBase
{
	public enum MonsterState
	{
		Idle,
		RandomMove,
		Follow,
		Attack,
		Dead
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("攻击")]
	public bool isAttacking;

	public VariableFloat attackCD;

	public float attackCDTimer;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

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

	public override void EveryInitialCallback()
	{
		state = MonsterState.Idle;
		attackCD.RandomResult();
		isAttacking = false;
	}

	public override void Update()
	{
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
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Idle");
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval && PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				state = MonsterState.Follow;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		}
		case MonsterState.Follow:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTarget();
			}
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD.result)
			{
				state = MonsterState.Attack;
				isAttacking = true;
				attackCDTimer = 0f;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero);
			break;
		}
		if (base.HaveTarget)
		{
			SetFlip(base.TargetPoint.x - base.transform.position.x);
		}
		else
		{
			SetFlip(base.CurrentMotion.x);
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
		{
			Monster109EggBullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster109_EggBullet", base.transform.position).GetComponent<Monster109EggBullet>();
			component.Init(myPpt);
			component.transform.localScale = (myPpt.SR_Models[0].flipX ? Vector3.one : new Vector3(-1f, 1f, 1f));
			break;
		}
		case "AttackEnd":
			state = MonsterState.Follow;
			attackCDTimer = 0f;
			break;
		case "Dead":
			myPpt.InvincibleUnregister();
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
