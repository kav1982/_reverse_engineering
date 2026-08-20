using UnityEngine;

public class Monster101 : UnitBase
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

	private SpellInitialParameter deadBullet = new SpellInitialParameter();

	public float deadSpellHeight;

	public float deadSpellSpeed;

	public float deadSpellDuration;

	public int deadSpellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	[Header("瞄准")]
	public Animator gunAnima;

	public Vector3 gunOriginPos;

	public Transform gun;

	public Transform gunShadowParent;

	public Transform bulletPivot;

	public SpriteRenderer gunShadow;

	private Vector2 gunDir;

	private float gunAngle;

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

	[Header("死亡")]
	public Sprite corpseSprite;

	public Sprite transitionSprite;

	public Sprite bangSprite;

	public float bangProbability;

	public bool isDying;

	public GameObject deadEffect;

	public AIPattern pattern;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams SipBulletSsp;

	private SpellSpawnParams DeadBulletSsp;

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
		DeadBulletSsp = UnitDotsSyncSystem.GetSpellPrototype(90281);
		sSPModifier = UnitBase.GetSSPModifier(in DeadBulletSsp);
		sSPModifier.Speed = deadSpellSpeed;
		sSPModifier.Duration = deadSpellDuration;
		sSPModifier.Damage = deadSpellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref DeadBulletSsp);
		myPpt.RemoveSRFromArray(gunShadow);
		gunShadow.color = new Color(0f, 0f, 0f, 0.4f);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		isDying = false;
		bodyRender.sprite = frontSprite;
		gun.gameObject.SetActive(value: true);
		bodyRender.gameObject.SetActive(value: true);
		attackCD.RandomResult();
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
		if (isDying)
		{
			SetMove(Vector3.zero);
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
			if (gun.transform.eulerAngles.z > 0f && gun.transform.eulerAngles.z < 180f)
			{
				gun.localPosition = gunOriginPos + new Vector3(0f, 0f, 0.15f);
			}
			else
			{
				gun.localPosition = gunOriginPos;
			}
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
			if (gun.transform.eulerAngles.z > 0f && gun.transform.eulerAngles.z < 180f)
			{
				gun.localPosition = gunOriginPos + new Vector3(0f, 0f, 0.15f);
			}
			else
			{
				gun.localPosition = gunOriginPos;
			}
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
		if (attackCDTimer > attackCD.result && !isAttacking)
		{
			aimDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
			isAttacking = true;
			if (pattern == AIPattern.Pattern1)
			{
				gunAnima.Play("NormalAttack");
			}
			else if (pattern == AIPattern.Pattern2)
			{
				gunAnima.Play("DoubleAttack");
			}
			else
			{
				gunAnima.Play("QuickAttack");
			}
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		if (base.HaveTarget)
		{
			if (pattern == AIPattern.Pattern3)
			{
				Vector3 vector = ((!(targetPpt.PlayerCtrller != null)) ? targetPpt.UnitBas.CurrentMotion : targetPpt.PlayerCtrller.CurrentMotion);
				float a = Tool2D.IgnoreZDistance(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)) / spellSpeed;
				a = Mathf.Max(a, 0f);
				Vector3 v = vector * a + targetPpt.transform.position;
				aimDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(v, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
			}
			else
			{
				aimDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
			}
		}
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		if (pattern == AIPattern.Pattern3)
		{
			for (int i = 0; i < 5; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(aimDir, -12.5f + (float)(i * 5));
				switch (i)
				{
				case 0:
				case 4:
					sSPModifier.Speed = spellSpeed - 0.6f;
					break;
				case 1:
				case 3:
					sSPModifier.Speed = spellSpeed - 0.3f;
					break;
				default:
					sSPModifier.Speed = spellSpeed;
					break;
				}
				sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + bulletPivot.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
		}
		else
		{
			for (int j = 0; j < 5; j++)
			{
				sSPModifier.Direction = Tool2D.GetDir(aimDir, -25f + (float)(j * 10));
				sSPModifier.Speed = spellSpeed;
				sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + bulletPivot.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
		}
		isAttacking = false;
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	public void ThrowBulletShell()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster101_BulletShell", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(new Vector3(0f - gun.localScale.x, 0f, 0f), 3f);
	}

	protected override void SetFlip(float motionX)
	{
		if (base.HaveTarget && !isDying)
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
		if (isDying)
		{
			return;
		}
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

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if ((float)Random.Range(0, 100) < bangProbability)
		{
			info.stopAnnouncedDeath = true;
			bodyRender.sprite = transitionSprite;
			base.Anima.Play("Dead");
			isDying = true;
			myPpt.InvincibleRegister();
			gun.gameObject.SetActive(value: false);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
		if (!isDying)
		{
			Corpse component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Monster102", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>();
			component.sr.sprite = corpseSprite;
			component.Initialize(base.Rigid.linearVelocity, 1.5f);
		}
	}

	public void Bang()
	{
		if (pattern == AIPattern.Pattern3)
		{
			Vector3 dir = Tool2D.GetDir();
			for (int i = 0; i < 13; i++)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in DeadBulletSsp);
				sSPModifier.Direction = Tool2D.GetDir(dir, Random.Range(0f, 360f));
				sSPModifier.Speed = spellSpeed - (float)Random.Range(2, 4);
				sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
				sSPModifier.ApplyToSSP(ref DeadBulletSsp);
				ShootSpell(DeadBulletSsp);
			}
		}
		else
		{
			UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in SipBulletSsp);
			Vector3 dir2 = Tool2D.GetDir(-90f);
			for (int j = 0; j < 6; j++)
			{
				sSPModifier2.Direction = Tool2D.GetDir(dir2, j * 60);
				sSPModifier2.Speed = spellSpeed;
				sSPModifier2.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
				sSPModifier2.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Dead"))
		{
			if (animaName == "ChangeBangSprite")
			{
				bodyRender.sprite = bangSprite;
			}
		}
		else
		{
			deadEffect.gameObject.SetActive(value: true);
			bodyRender.gameObject.SetActive(value: false);
			Bang();
		}
	}
}
