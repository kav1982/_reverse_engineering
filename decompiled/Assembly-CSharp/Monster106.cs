using UnityEngine;

public class Monster106 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Attack,
		RandomMove,
		Follow,
		Dead
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("攻击")]
	public Vector3 aimDir;

	public float attackDistance;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public GameObject attackEffect;

	[Header("模式")]
	public AIPattern pattern;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams SipBulletSsp;

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
		randomMoveTime.RandomResult();
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		randomMoveTime.RandomResult();
		attackEffect.SetActive(value: false);
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
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
			}
			else
			{
				aimDir = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position - new Vector3(0f, 0.2f, 0f));
				base.Anima.Play("Attack");
			}
			SetMove(Vector3.zero);
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
			GetNearestTargetWithTimer();
			if (stateExistTime > randomMoveTime.result && base.HaveTarget && Tool2D.IgnoreZDistanceSqr(base.TargetPoint, base.transform.position) < attackDistance * attackDistance)
			{
				state = MonsterState.Attack;
				break;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed, isFlip: false);
			}
			break;
		}
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
				SEMgr.Inst.monster12Land.PlaySE();
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Follow:
			break;
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		if (pattern == AIPattern.Pattern1)
		{
			if (base.HaveTarget)
			{
				sSPModifier.Direction = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position - new Vector3(0f, 0.2f, 0f));
			}
			else
			{
				sSPModifier.Direction = aimDir;
			}
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
		}
		else
		{
			if (base.HaveTarget)
			{
				Vector3 vector = ((!(targetPpt.PlayerCtrller != null)) ? targetPpt.UnitBas.CurrentMotion : targetPpt.PlayerCtrller.CurrentMotion);
				float a = Tool2D.IgnoreZDistance(base.TargetPoint, base.transform.position - new Vector3(0f, 0.2f, 0f)) / spellSpeed;
				a = Mathf.Max(a, 0f);
				Vector3 v = vector * a + targetPpt.transform.position;
				aimDir = Tool2D.IgnoreZV2ToV1Normal(v, base.transform.position - new Vector3(0f, 0.2f, 0f));
			}
			sSPModifier.Direction = Tool2D.GetDir(aimDir, -15f);
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
			sSPModifier.Direction = Tool2D.GetDir(aimDir, 15f);
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster106_ExoloreEffect", base.transform.position, 1.2f);
		myPpt.AnnouncedDeath();
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
		case "Attack":
			Attack();
			attackEffect.SetActive(value: false);
			break;
		case "AttackStart":
			attackEffect.SetActive(value: true);
			break;
		case "Dead":
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
