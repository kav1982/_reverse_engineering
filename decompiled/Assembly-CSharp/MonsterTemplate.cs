using UnityEngine;

public class MonsterTemplate : UnitBase
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
	public VariableFloat IdleTime;

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
				base.Anima.Play("MonsterT_Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("MonsterT_Move");
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < sight * sight)
			{
				state = MonsterState.Move;
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
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("MonsterT_Idle");
			}
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("MonsterT_Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
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
			if (attackCDTimer > attackCD.result && base.HaveTarget && ToTargetDistanceSqr() < attackRadius * attackRadius)
			{
				attackCDTimer = 0f;
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("MonsterT_Attack");
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
