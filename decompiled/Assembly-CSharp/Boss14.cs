using UnityEngine;

public class Boss14 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Attack
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("行动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float sight;

	public float chaseDistance;

	public float attackRadius;

	public VariableFloat attackCD;

	public float attackCDTimer;

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

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
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
		attackCDTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < sight * sight)
			{
				state = MonsterState.Move;
			}
			else if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < sight * sight)
			{
				state = MonsterState.Move;
				break;
			}
			if (stateExistTime > randomMoveTime.result || navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			SetFlip(ToTargetDir().x);
			if (ToTargetDistanceSqr() > chaseDistance * chaseDistance)
			{
				state = MonsterState.Idle;
			}
			else if (attackCDTimer > attackCD.result && ToTargetDistanceSqr() < attackRadius * attackRadius)
			{
				attackCDTimer = 0f;
				attackCD.RandomResult();
				state = MonsterState.Attack;
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "AttackFinish"))
		{
			_ = animaName == "Attack";
		}
		else
		{
			state = MonsterState.Idle;
		}
	}
}
