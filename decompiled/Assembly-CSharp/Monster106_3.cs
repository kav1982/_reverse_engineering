using UnityEngine;

public class Monster106_3 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		FollowAttack,
		RandomMove,
		Dead
	}

	[Header("追击")]
	public float maxFollowDistance;

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("攻击")]
	public bool isAttacking;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	[Header("亡语")]
	public string prefab10601;

	public string prefab10602;

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

	public override void SingleInitialCallback()
	{
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90281);
		sipBullet.spelldataConfig.speed = spellSpeed;
		sipBullet.spelldataConfig.duration = spellDuration;
		sipBullet.spelldataConfig.damage = spellDamage;
		sipBullet.ownerPpt = myPpt;
		sipBullet.shootSpellPreSpells.Add(30121);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		attackCDTimer = 3f;
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
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.FollowAttack:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				isAttacking = false;
				break;
			}
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD.result)
			{
				if (!isAttacking)
				{
					base.Anima.Play("Attack");
					isAttacking = true;
				}
				SetMove(Vector3.zero);
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case MonsterState.RandomMove:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
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
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				Debug.Log(base.HaveTarget);
				if (base.HaveTarget)
				{
					state = MonsterState.FollowAttack;
				}
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
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		float num = 30f;
		for (int i = 0; i < 12; i++)
		{
			sipBullet.shootDirection = Tool2D.GetDir(Vector3.up, (float)i * num + 15f);
			SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position).GetComponent<SpellBase>();
			component.isThroughWall = false;
			component.Initialize(sipBullet);
			component.rebounceTime = 1;
		}
		isAttacking = false;
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			Attack();
			break;
		case "SpawnBullat":
		{
			for (int i = 0; i < Random.Range(3, 5); i++)
			{
				if (Random.Range(0, 2) == 0)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + prefab10601, base.transform.position).transform.position = base.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f), 0f);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + prefab10602, base.transform.position).transform.position = base.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f), 0f);
				}
			}
			break;
		}
		case "Dead":
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
