using UnityEngine;

public class Monster103 : UnitBase
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
	public bool isAttacking;

	public VariableFloat attackOffset;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public LineRenderer aimLine;

	public LineRenderer aimLineShadow;

	public float aimTime;

	public float aimTimer;

	[Header("瞄准")]
	public Animator gunAnima;

	public Transform gun;

	public Transform bulletPivot;

	private Vector2 gunDir;

	private float gunAngle;

	public Vector3 gunOriginPos;

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
		gunShadow.color = new Color(0f, 0f, 0f, 0.4f);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		aimLine.gameObject.SetActive(value: false);
		aimLineShadow.gameObject.SetActive(value: false);
	}

	public override void Update()
	{
		gunShadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(gunShadowParent.transform.position - new Vector3(0f, 0.2f, 0f)), LayerCorrectType.Shadow);
		base.Update();
		if (myPpt.Affect_InAbyss && aimLine.gameObject.activeSelf)
		{
			aimLine.gameObject.SetActive(value: false);
			aimLineShadow.gameObject.SetActive(value: false);
		}
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
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.GetNearestPpt(base.transform.position).transform.position) < maxFollowDistance * maxFollowDistance)
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
			LaserAim();
			gun.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, gun.position)) + new Vector3(0f, 0f, 90f);
			if (gun.transform.eulerAngles.z > 0f && gun.transform.eulerAngles.z < 180f)
			{
				gun.localPosition = gunOriginPos + new Vector3(0f, 0f, 0.15f);
			}
			else
			{
				gun.localPosition = gunOriginPos;
			}
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
		if (!(attackCDTimer > attackCD.result))
		{
			return;
		}
		if (!isAttacking)
		{
			isAttacking = true;
			aimTimer = 0f;
			aimLine.gameObject.SetActive(value: true);
			aimLineShadow.gameObject.SetActive(value: true);
		}
		if (isAttacking)
		{
			aimTimer += Time.deltaTime;
			LaserAim();
			if (aimTimer > aimTime)
			{
				gunAnima.Play("ReadyAttack");
				aimTimer = 0f;
			}
		}
	}

	public void LaserAim()
	{
		if (!base.HaveTarget)
		{
			GetNearestTarget();
		}
		if (!base.HaveTarget)
		{
			state = MonsterState.RandomMove;
			aimLine.gameObject.SetActive(value: false);
			aimLineShadow.gameObject.SetActive(value: false);
		}
		Vector3 vector = Tool2D.IgnoreZPoint(bulletPivot.position - new Vector3(0f, 0f, 0.01f)) + new Vector3(0f, -0.2f, -0.2f);
		aimLine.SetPosition(0, Tool2D.GetLayerPoint(vector));
		if (Physics.Raycast(new Ray(vector, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, vector)), out var hitInfo, 9999f, LayerMask.GetMask("Destructible", "Wall", "Brittleness")))
		{
			if (ToPointDistanceSqr(hitInfo.point) < ToPointDistanceSqr(base.TargetPoint))
			{
				aimLine.SetPosition(1, Tool2D.GetLayerPoint(hitInfo.point));
			}
			else
			{
				aimLine.SetPosition(1, Tool2D.GetLayerPoint(base.TargetPoint));
			}
		}
		else
		{
			aimLine.SetPosition(1, Tool2D.GetLayerPoint(base.TargetPoint));
		}
		aimLineShadow.SetPosition(0, aimLine.GetPosition(0) - new Vector3(0f, 0.2f, 0f));
		aimLineShadow.SetPosition(1, aimLine.GetPosition(1) - new Vector3(0f, 0.2f, 0f));
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Speed = spellSpeed;
		if (base.HaveTarget)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position - new Vector3(0f, 0.2f, 0f)), attackOffset.RandomResult());
		}
		sSPModifier.SpawnPosition = bulletPivot.position - new Vector3(0f, 0.2f, 0f);
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
		ShootSpell(SipBulletSsp);
		isAttacking = false;
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	public void ThrowBulletShell()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster102_BulletShell", base.transform.position - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(new Vector3(0f - gun.localScale.x, 0f, 0f), 3f);
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
