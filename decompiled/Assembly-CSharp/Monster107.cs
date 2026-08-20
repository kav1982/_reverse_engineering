using UnityEngine;

public class Monster107 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		FollowAttack
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("攻击")]
	public Vector3 aimDir;

	public bool isAttacking;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public AIPattern pattern;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams SipBulletSsp;

	public Transform part;

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
		SipBulletSsp = UnitDotsSyncSystem.GetSpellPrototype(90281);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		isAttacking = false;
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
				state = MonsterState.FollowAttack;
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
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.FollowAttack;
				}
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
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.FollowAttack;
					Debug.Log(111123123);
				}
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			break;
		}
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
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (!isAttacking)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero);
			}
			CheckAttack();
			break;
		}
	}

	public void CheckAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (attackCDTimer > attackCD.result && !isAttacking)
		{
			isAttacking = true;
			aimDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position - new Vector3(0f, 0.2f, 0f)), -45f);
			base.Anima.Play("Attack");
		}
	}

	public void ShootTriangle(Vector3 dir)
	{
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Direction = dir;
		sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
		sSPModifier.Speed = spellSpeed + 1f;
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
		ShootSpell(SipBulletSsp);
		for (int i = 1; i < 3; i++)
		{
			sSPModifier.Direction = dir;
			sSPModifier.Speed = spellSpeed + 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position + Tool2D.GetDir(dir, 150f) * 0.25f * i;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
			sSPModifier.Direction = dir;
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position + Tool2D.GetDir(dir, -150f) * 0.25f * i;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
		}
	}

	public void FirstAttack()
	{
		ShootTriangle(Tool2D.GetDir(aimDir, 45f));
		part.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(Tool2D.GetDir(aimDir, 45f).y, Tool2D.GetDir(aimDir, 45f).x) * 57.29578f + 90f);
	}

	public void SecondAttack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Direction = Tool2D.GetDir();
		float num = 3.6f;
		for (int i = 0; i < 25; i++)
		{
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
			sSPModifier.Direction = Tool2D.GetDir(aimDir, (float)i * num);
			sSPModifier.Speed = spellSpeed;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
			if (i == 0 || i == 24)
			{
				ShootTriangle(sSPModifier.Direction);
			}
		}
		isAttacking = false;
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	public void ThirdAttack()
	{
		ShootTriangle(Tool2D.GetDir(aimDir, 45f));
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "FirstAttack":
			FirstAttack();
			break;
		case "SecondAttack":
			SecondAttack();
			break;
		case "ThirdAttack":
			ThirdAttack();
			break;
		}
	}
}
