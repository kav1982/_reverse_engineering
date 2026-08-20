using System.Collections;
using UnityEngine;

public class Boss12 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		GunCharge,
		SectorAttack,
		FlyAnimation,
		FallAnimation,
		FLy,
		MissileAttack,
		FalculaAim,
		FalculaLaunch,
		FalculaBack,
		FalculaDrag,
		Dead
	}

	[Header("\u0368\ufffd\ufffd")]
	public float attackCDTime;

	public float attackCDTimer;

	public bool canAttack;

	public bool canHit;

	public float hitCDTime;

	public float hitCDTimer;

	public bool canMove;

	public Transform corpseCenter;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public ShockParam shockParam;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public VariableFloat idleTime;

	public float checkIntervalTime;

	public float checkIntervalTimer;

	[Header("\ufffd\ufffd\ufffd\ufffdƶ\ufffd")]
	public VariableFloat randomMoveTime;

	public Vector3 randomMoveTarget;

	public VariableFloat randomMoveRadius;

	[Header("\ufffdӵ\ufffd")]
	public Transform bulletPivot;

	public Transform gunPivot;

	public SpellInitialParameter normalBullet = new SpellInitialParameter();

	public float normalSpellHeight;

	public float normalSpellSpeed;

	public float normalSpellDuration;

	public int normalSpellDamage;

	public SpellInitialParameter bigBullet = new SpellInitialParameter();

	public float bigSpellHeight;

	public float bigSpellSpeed;

	public float bigSpellDuration;

	public int bigSpellDamage;

	[Header("ɨ\ufffd\ufffd")]
	public bool canTurn;

	public VariableFloat strafeCount;

	public float strafeIntervalTime;

	public float strafeCounter;

	public Animator gunAnimator;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public GameObject chargeEffect;

	public int shotType;

	public int sectorBulletCount;

	public SpriteRenderer shadow;

	[Header("\ufffd\ufffdս")]
	public Transform hitCheckPivot;

	public float hitRadius;

	public float knockBack;

	public LayerMask attackMask;

	[Header("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public Transform dustPivot;

	public float flySpeed;

	public float crushRadius;

	public float flyTime;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public int missileCount;

	public int missileCounter;

	[Header("\ufffd\ufffdצ")]
	public Transform falculaLaunchPivot;

	public Transform falculaDragPivot;

	[Header("״\u032c\ufffd\ufffd")]
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
		state = MonsterState.BornIdle;
		normalBullet.spelldataConfig = SpellConfig.GetConfigCopy(90281);
		normalBullet.spelldataConfig.speed = normalSpellSpeed;
		normalBullet.spelldataConfig.duration = normalSpellDuration;
		normalBullet.spelldataConfig.damage = normalSpellDamage;
		normalBullet.ownerPpt = myPpt;
		bigBullet.spelldataConfig = SpellConfig.GetConfigCopy(90421);
		bigBullet.spelldataConfig.speed = bigSpellSpeed;
		bigBullet.spelldataConfig.duration = bigSpellDuration;
		bigBullet.spelldataConfig.damage = bigSpellDamage;
		bigBullet.ownerPpt = myPpt;
	}

	public override void EveryInitialCallback()
	{
		myPpt.RemoveSRFromArray(shadow);
		shadow.color = new Color(0f, 0f, 0f, 0.4f);
	}

	public override void Update()
	{
		if (base.HaveTarget && canTurn)
		{
			float num = 360f - ToTargetDegree();
			num = Mathf.Round(num / 45f) * 45f;
			base.Anima.SetFloat("TargetAngle", num);
		}
		base.Anima.SetFloat("Speed", base.CurrentMotion.magnitude);
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
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			shotType = 0;
			state = MonsterState.GunCharge;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			strafeCounter = 0f;
			StartCoroutine(Strafe());
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			shotType = 1;
			state = MonsterState.GunCharge;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			base.Anima.Play("TakeOff");
			myPpt.CC_Self.enabled = false;
			myPpt.CanTouch = false;
			state = MonsterState.FlyAnimation;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			state = MonsterState.MissileAttack;
		}
		if (canAttack)
		{
			attackCDTimer += Time.deltaTime;
		}
		if (attackCDTimer >= attackCDTime && canAttack)
		{
			attackCDTimer = 0f;
			switch (Random.Range(0, 11))
			{
			case 0:
				shotType = 0;
				state = MonsterState.GunCharge;
				break;
			case 1:
				shotType = 1;
				state = MonsterState.GunCharge;
				break;
			case 2:
				base.Anima.Play("TakeOff");
				myPpt.CC_Self.enabled = false;
				myPpt.CanTouch = false;
				state = MonsterState.FlyAnimation;
				break;
			case 3:
				state = MonsterState.MissileAttack;
				break;
			default:
				strafeCounter = 0f;
				shotType = Random.Range(0, 2);
				strafeCount.RandomResult();
				attackCDTimer = 0f;
				hitCDTimer = 0f;
				canAttack = false;
				canHit = false;
				StartCoroutine(Strafe());
				break;
			}
		}
		if (canHit)
		{
			hitCDTimer += Time.deltaTime;
		}
		if (canHit && base.HaveTarget && ToTargetDistanceSqr() < 6f && hitCDTimer >= hitCDTime)
		{
			attackCDTimer = 0f;
			hitCDTimer = 0f;
			canAttack = false;
			canHit = false;
			canMove = false;
			base.Anima.Play("Hit");
			SEMgr.Inst.monster12Land.PlaySE();
		}
		else if (canAttack && (!base.HaveTarget || (ToTargetDistanceSqr() > 4f && !canMove)))
		{
			canMove = true;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				canMove = false;
				canAttack = true;
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
				canMove = false;
				canHit = true;
				canAttack = true;
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkIntervalTime)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Move;
				}
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Run");
				randomMoveTime.RandomResult();
				randomMoveTarget = base.transform.position + Tool2D.GetDir() * randomMoveRadius.RandomResult();
				if ((float)(LevelMgr.Inst.CurrentRoomCfg.width / 2) - Tool2D.IgnoreZDistanceSqr(randomMoveTarget, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) < 2f || (float)(LevelMgr.Inst.CurrentRoomCfg.height / 2) - Tool2D.IgnoreZDistanceSqr(randomMoveTarget, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) < 2f)
				{
					randomMoveTarget = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				}
				float num2 = 360f - Tool2D.IgnoreZAngle360(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(randomMoveTarget, base.transform.position));
				num2 = Mathf.Round(num2 / 45f) * 45f;
				base.Anima.SetFloat("TargetAngle", num2);
				canAttack = true;
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
			}
			if (canMove)
			{
				GetNavInfo(randomMoveTarget);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero);
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				randomMoveTarget = base.transform.position + Tool2D.GetDir() * randomMoveRadius.RandomResult();
				base.Anima.SetFloat("TargetAngle", 360f - Tool2D.IgnoreZAngle360(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(randomMoveTarget, base.transform.position)));
				GetNavInfo(randomMoveTarget);
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkIntervalTime)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Move;
				}
			}
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Run");
				canAttack = true;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
			}
			else if (canMove && ToTargetDistanceSqr() > 2f)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero);
			}
			break;
		case MonsterState.GunCharge:
			if (changedState)
			{
				chargeEffect.SetActive(value: true);
				canMove = false;
				canHit = false;
				canAttack = false;
				SEMgr.Inst.monster12Land.PlaySE();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 1.2f)
			{
				chargeEffect.SetActive(value: false);
				switch (shotType)
				{
				case 0:
					SEMgr.Inst.monster12Land.PlaySE();
					bigBullet.shootDirection = Tool2D.IgnoreZV2ToV1Normal(bulletPivot, gunPivot);
					ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + bigBullet.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - normalSpellHeight) + bulletPivot.position).GetComponent<SpellBase>().Initialize(bigBullet);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_FissionShotEffect", bulletPivot.position, 0.5f).transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(bulletPivot, gunPivot));
					state = MonsterState.Idle;
					attackCDTimer = 0f;
					break;
				case 1:
					StartCoroutine(DoubleSector());
					canTurn = false;
					state = MonsterState.SectorAttack;
					break;
				}
			}
			break;
		case MonsterState.SectorAttack:
			if (changedState)
			{
				canMove = false;
				canHit = false;
				canAttack = false;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.FlyAnimation:
			if (changedState)
			{
				canMove = false;
				canHit = false;
				canAttack = false;
				SEMgr.Inst.monster12Land.PlaySE();
			}
			break;
		case MonsterState.FallAnimation:
			if (changedState)
			{
				canMove = false;
				canHit = false;
				canAttack = false;
				SEMgr.Inst.monster12Land.PlaySE();
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.FLy:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				canMove = false;
				canHit = false;
				canAttack = false;
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
			}
			if (!base.HaveTarget)
			{
				reference = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			}
			else
			{
				reference = base.TargetPoint;
			}
			GetNavInfo(reference);
			SetMove(ToPointDir(navInfo.ToGoPoint) * flySpeed);
			if (stateExistTime > flyTime)
			{
				base.Anima.Play("Fall");
				state = MonsterState.FallAnimation;
			}
			break;
		}
		case MonsterState.MissileAttack:
			if (changedState)
			{
				base.Anima.Play("MissileAttack");
				canMove = false;
				canHit = false;
				canAttack = false;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Dead:
			SetMove(Vector3.zero);
			break;
		case MonsterState.FalculaAim:
		case MonsterState.FalculaLaunch:
		case MonsterState.FalculaBack:
		case MonsterState.FalculaDrag:
			break;
		}
	}

	private IEnumerator Strafe()
	{
		int angleOffsetIndex = -2;
		int offsetDir = 1;
		gunAnimator.Play("Fire");
		int strafeType = Random.Range(0, 2);
		SEMgr.Inst.monster12Land.PlaySE();
		while (strafeCounter < strafeCount.result)
		{
			strafeCounter += 1f;
			normalBullet.shootDirection = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(bulletPivot, gunPivot), angleOffsetIndex * 15 + Random.Range(-5, 5));
			if (strafeType == 0)
			{
				angleOffsetIndex += offsetDir;
				if (angleOffsetIndex > 2 || angleOffsetIndex < -2)
				{
					offsetDir *= -1;
				}
			}
			else
			{
				angleOffsetIndex = Random.Range(-2, 3);
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + normalBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - normalSpellHeight) + bulletPivot.position + normalBullet.shootDirection * 0.5f).GetComponent<SpellBase>().Initialize(normalBullet);
			if (base.Anima.GetFloat("TargetAngle") >= 40f && base.Anima.GetFloat("TargetAngle") <= 200f)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_BulletShell", new Vector3(gunPivot.position.x, gunPivot.position.y, 0f) - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(Tool2D.GetDir(normalBullet.shootDirection, -90f) * 1.2f, 3f);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_BulletShell", new Vector3(gunPivot.position.x, gunPivot.position.y, 0f) - new Vector3(0f, 0f, 0.3f)).GetComponent<Corpse>().Initialize(Tool2D.GetDir(normalBullet.shootDirection, 90f) * 1.2f, 3f);
			}
			yield return new WaitForSeconds(strafeIntervalTime);
		}
		gunAnimator.Play("GunIdle");
		strafeCounter = 0f;
		attackCDTimer = 0f;
		canAttack = true;
		canHit = true;
	}

	private IEnumerator DoubleSector()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		Vector3 startDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(bulletPivot, gunPivot), -120 / (sectorBulletCount - 1) * ((sectorBulletCount - 1) / 2));
		float angleOffset = 120 / (sectorBulletCount - 1);
		for (int i = 0; i < sectorBulletCount; i++)
		{
			normalBullet.shootDirection = Tool2D.GetDir(startDir, angleOffset * (float)i);
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + normalBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - normalSpellHeight) + bulletPivot.position).GetComponent<SpellBase>().Initialize(normalBullet);
		}
		yield return new WaitForSeconds(1.5f);
		SEMgr.Inst.monster12Land.PlaySE();
		for (int j = 0; j < sectorBulletCount; j++)
		{
			normalBullet.shootDirection = Tool2D.GetDir(startDir, angleOffset * (float)j);
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + normalBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - normalSpellHeight) + bulletPivot.position).GetComponent<SpellBase>().Initialize(normalBullet);
		}
		yield return new WaitForSeconds(0.5f);
		state = MonsterState.Idle;
		canTurn = true;
		attackCDTimer = 0f;
	}

	protected override void BossDeadStay()
	{
		base.enabled = false;
		base.Rigid.isKinematic = true;
		base.CC_Self.enabled = false;
		myPpt.enabled = false;
		myPpt.CanTouch = false;
		myPpt.CanBeTarget = false;
		myPpt.ChangeColor(myPpt.Color_NormalBody);
		if (base.Anima != null)
		{
			base.Anima.speed = 0f;
		}
		if (base.SAnima != null)
		{
			base.SAnima.timeScale = 0f;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "HitAttack":
		{
			Collider[] array = Physics.OverlapSphere(hitCheckPivot.position, hitRadius, attackMask);
			foreach (Collider collider2 in array)
			{
				UnitProperty component2 = collider2.GetComponent<UnitProperty>();
				TakeDamageInfo takeDamageInfo2 = new TakeDamageInfo();
				takeDamageInfo2.teammateTakeDamageRatio = 3f;
				takeDamageInfo2.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collider2.transform, base.transform) * knockBack;
				switch (collider2.tag)
				{
				case "Player":
				case "Teammate":
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", collider2.transform.position, 1f);
					component2.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo2);
					break;
				case "Brittleness":
					component2.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo2);
					break;
				case "Destructible":
					component2.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo2);
					break;
				}
			}
			break;
		}
		case "CrushAttack":
		{
			CamController.Inst.SetShock(shockParam);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DustDirtySoft", dustPivot.position, 1f);
			Collider[] array = Physics.OverlapSphere(hitCheckPivot.position, crushRadius, attackMask);
			foreach (Collider collider in array)
			{
				UnitProperty component = collider.GetComponent<UnitProperty>();
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.teammateTakeDamageRatio = 3f;
				takeDamageInfo.knockbackForce = Vector3.left.normalized * knockBack;
				switch (collider.tag)
				{
				case "Player":
				case "Teammate":
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", collider.transform.position, 1f);
					component.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo);
					break;
				case "Brittleness":
					component.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo);
					break;
				case "Destructible":
					component.TakeDamage(10f, AttackerType.NothingSpecial, takeDamageInfo);
					break;
				}
			}
			myPpt.CC_Self.enabled = true;
			myPpt.CanTouch = true;
			break;
		}
		case "FlyAnimationEnd":
			state = MonsterState.FLy;
			break;
		case "FallAnimationEnd":
			attackCDTimer = attackCDTime - 0.5f;
			state = MonsterState.Idle;
			break;
		case "HitAnimationEnd":
			canAttack = true;
			canHit = true;
			base.Anima.Play("Run");
			break;
		case "MissileAttack":
		{
			Vector3 point = PlayerMgr.Inst.PlayerPoint;
			if (base.HaveTarget)
			{
				point = base.TargetPoint;
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_Missile", point);
			missileCounter++;
			if (missileCounter >= missileCount)
			{
				missileCounter = 0;
				state = MonsterState.Idle;
				attackCDTimer = 0f;
			}
			break;
		}
		}
	}
}
