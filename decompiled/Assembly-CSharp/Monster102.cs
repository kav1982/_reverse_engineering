using UnityEngine;

public class Monster102 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		FollowAttack,
		IdleAttack,
		Hurt
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("追击")]
	public float maxFollowDistance;

	[Header("攻击")]
	public Vector3 aimDir;

	public bool isAttacking;

	public VariableFloat attackOffset;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public int bulletCount;

	public int shootAmount;

	[Header("瞄准")]
	public Animator gunAnima;

	public Transform gun;

	public Transform bulletPivot;

	private Vector2 gunDir;

	private float gunAngle;

	public Transform gunShadowParent;

	public SpriteRenderer gunShadow;

	[Header("翻转")]
	public SpriteRenderer bodyRender;

	public Sprite frontSprite;

	public Sprite backSprite;

	[Header("受伤")]
	public Sprite hurtSprite;

	public float hurtTime;

	public float hurtHealthPercent;

	public float hurtCounter;

	public float hurtRecoverSpeedPercent;

	[Header("尸体图片")]
	public Sprite corpseSprite;

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
		myPpt.RemoveSRFromArray(gunShadow);
		gunShadow.color = new Color(0f, 0f, 0f, 0.3f);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		bulletCount = 0;
		isAttacking = false;
	}

	public override void Update()
	{
		gunShadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(gunShadowParent.transform.position - new Vector3(0f, 0.2f, 0f)), LayerCorrectType.Shadow);
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
		hurtCounter -= Time.deltaTime * hurtRecoverSpeedPercent * myPpt.unitCfg.maxHP;
		hurtCounter = Mathf.Max(hurtCounter, 0f);
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
				base.Anima.Play("Move");
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
				}
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed, isFlip: false);
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
				base.Anima.Play("Move");
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
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) < maxFollowDistance * maxFollowDistance)
			{
				state = MonsterState.IdleAttack;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			gun.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, gun.position)) + new Vector3(0f, 0f, 90f);
			CheckAttack();
			break;
		case MonsterState.IdleAttack:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > maxFollowDistance * maxFollowDistance)
			{
				state = MonsterState.FollowAttack;
				break;
			}
			gun.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, gun.position)) + new Vector3(0f, 0f, 90f);
			CheckAttack();
			break;
		case MonsterState.Hurt:
			if (changedState)
			{
				bodyRender.sprite = hurtSprite;
				base.Anima.Play("Hurt", 0, 0f);
				hurtCounter = 0f;
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > hurtTime)
			{
				state = MonsterState.FollowAttack;
				bodyRender.sprite = frontSprite;
			}
			break;
		}
	}

	public void CheckAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (!(attackCDTimer > attackCD.result) || isAttacking)
		{
			return;
		}
		isAttacking = true;
		aimDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
		if (pattern == AIPattern.Pattern1)
		{
			if (bulletCount < 5)
			{
				gunAnima.Play("NormalAttack");
			}
			else
			{
				gunAnima.Play("LastAttack");
			}
		}
		else if (pattern == AIPattern.Pattern2)
		{
			gunAnima.Play("UziAttack");
		}
		else if (bulletCount < 3)
		{
			gunAnima.Play("FourAttack");
		}
		else
		{
			gunAnima.Play("LastAttack");
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Direction = aimDir;
		if (base.HaveTarget)
		{
			if (pattern == AIPattern.Pattern3)
			{
				Vector3 vector = ((!(targetPpt.PlayerCtrller != null)) ? targetPpt.UnitBas.CurrentMotion : targetPpt.PlayerCtrller.CurrentMotion);
				float a = Tool2D.IgnoreZDistance(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)) / spellSpeed * 0.5f;
				a = Mathf.Max(a, 0f);
				Vector3 v = vector * a + targetPpt.transform.position;
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(v, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
			}
			else
			{
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
			}
		}
		sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + bulletPivot.position;
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
		ShootSpell(SipBulletSsp);
		bulletCount++;
		isAttacking = false;
		if (pattern == AIPattern.Pattern1)
		{
			attackCD.RandomResult();
			if (bulletCount >= shootAmount)
			{
				bulletCount = 0;
			}
			attackCDTimer = 0f;
		}
		else if (pattern == AIPattern.Pattern2)
		{
			if (bulletCount >= shootAmount)
			{
				gunAnima.Play("Idle");
				attackCD.RandomResult();
				bulletCount = 0;
				attackCDTimer = 0f;
			}
		}
		else if (bulletCount >= shootAmount)
		{
			attackCD.RandomResult();
			bulletCount = 0;
			attackCDTimer = 0f;
		}
	}

	public void ThrowBulletShell()
	{
		if (pattern == AIPattern.Pattern2)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster102_BulletShell", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(new Vector3(0f - gun.localScale.x, 0f, 0f), 3f);
			return;
		}
		for (int i = 0; i < shootAmount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster102_BulletShell", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(new Vector3(0f - gun.localScale.x, 0f, 0f), 3f);
		}
	}

	protected override void SetFlip(float motionX)
	{
		if (base.HaveTarget)
		{
			if (base.TargetPoint.x < base.transform.position.x)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
				gun.localScale = new Vector3(-1f, -1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
				gun.localScale = new Vector3(1f, 1f, 1f);
			}
			if (base.TargetPoint.y > base.transform.position.y)
			{
				bodyRender.sprite = backSprite;
			}
			else
			{
				bodyRender.sprite = frontSprite;
			}
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		hurtCounter += info.damage;
		if (hurtCounter > myPpt.unitCfg.maxHP * hurtHealthPercent)
		{
			if (state == MonsterState.Hurt)
			{
				stateExistTime -= hurtTime;
				stateExistTime = Mathf.Max(stateExistTime, 0f);
			}
			else
			{
				state = MonsterState.Hurt;
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
		Corpse component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Monster102", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>();
		component.sr.sprite = corpseSprite;
		component.Initialize(base.Rigid.linearVelocity, 1.5f);
	}
}
