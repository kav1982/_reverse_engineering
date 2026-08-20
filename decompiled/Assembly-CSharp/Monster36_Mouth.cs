using UnityEngine;

public class Monster36_Mouth : UnitBase
{
	private enum MonsterState
	{
		None,
		BornIdle,
		Idle,
		Move,
		Chase,
		Attack
	}

	private MonsterState state = MonsterState.BornIdle;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	private float bornWaitTimer;

	public float bornIdleTime;

	private float idleTimer;

	public VariableFloat idleTime;

	private Vector3 moveDir;

	public VariableFloat moveTime;

	private float moveTimer;

	public int fogCount;

	public float attackDistance;

	public float attackRestTime;

	private float attackRestTimer;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float spellHeight;

	private void Start()
	{
		spellCfg1 = SpellConfig.GetConfigCopy(10021);
		spellCfg1.speed = spellSpeed;
		spellCfg1.duration = spellDuration;
		spellCfg1.damage = spellDamage;
	}

	public override void Update()
	{
		changedState = false;
		preState = tempState;
		tempState = state;
		if (state == MonsterState.None)
		{
			state = MonsterState.BornIdle;
		}
		if (preState != state)
		{
			changedState = true;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		attackRestTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				bornWaitTimer = 0f;
				base.Anima.Play("Monster36_MouthFloating");
			}
			bornWaitTimer += Time.deltaTime;
			if (bornWaitTimer > bornIdleTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Monster36_MouthFloating");
				idleTime.RandomResult();
				idleTimer = 0f;
			}
			idleTimer += Time.deltaTime;
			if (idleTimer > idleTime.result)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			CheckTarget();
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Monster36_MouthFloating");
				moveDir = Tool2D.GetDir();
				moveTime.RandomResult();
				moveTimer = 0f;
			}
			moveTimer += Time.deltaTime;
			if (moveTimer > moveTime.result)
			{
				state = MonsterState.Idle;
			}
			CheckTarget();
			SetMove(moveDir * myPpt.unitCfg.moveSpeed);
			break;
		case MonsterState.Chase:
			if (changedState)
			{
				base.Anima.Play("Monster36_MouthFloating");
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
				}
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
			moveDir = ToPointDir(targetPpt.transform.position);
			SetMove(moveDir * myPpt.unitCfg.moveSpeed);
			if ((targetPpt.transform.position - base.transform.position).sqrMagnitude < attackDistance * attackDistance && attackRestTimer > attackRestTime)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Monster36_MouthAttack");
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	private void CheckTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if ((double)checkTargetIntervalTimer >= 0.2)
		{
			GetNearestTarget();
		}
		if (base.HaveTarget)
		{
			state = MonsterState.Chase;
		}
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		info.immuneDamage = true;
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "ShootBullet"))
		{
			if (animaName == "AttackFinish")
			{
				state = MonsterState.Chase;
				attackRestTimer = 0f;
			}
		}
		else if (targetPpt != null)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellCfg1.prefab, base.transform.position + new Vector3(0f, 0f, 0f - spellHeight)).GetComponent<Spell1002RollBall>().Initialize(myPpt, Tool2D.IgnoreZPoint(targetPpt.transform.position - base.transform.position).normalized, spellCfg1, null, 30051);
		}
	}
}
