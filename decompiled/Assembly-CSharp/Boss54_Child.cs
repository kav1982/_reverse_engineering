using UnityEngine;

public class Boss54_Child : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		RandomMove,
		Move,
		Directional,
		LaserCharge,
		Laser,
		LaserAfter,
		StraightBullet,
		TrackBullet,
		Continual,
		DashPrepare,
		Dash,
		DashAfter
	}

	[Header("表现")]
	public SpriteRenderer SR_Born;

	public ParticleSystem bulletParticle;

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("技能")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveRadius;

	public MonsterState skillType;

	private float attackCDTimer;

	[Header("方向子弹")]
	public float directionalAttackCD;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	private SpellSpawnParams ssp;

	[Header("激光")]
	public float laserAttackCD;

	public float laserPredictTime;

	public float laserAimCloseChance;

	public VariableFloat laserAimOffsetClose;

	public VariableFloat laserAimOffsetFar;

	private Boss54_Line thisLaser;

	[Header("直线子弹")]
	public float straightAttackCD;

	public int straightBulletCount;

	public float straightBulletInterval;

	public float straightBulletSpeed;

	public VariableFloat straightBulletOffset;

	public VariableFloat straightBulletSmallOffset;

	private bool straightBulletShooting;

	private int straightBulletCounter;

	public VariableInt blockSpellCount;

	[Header("跟踪子弹")]
	public float trackAttackCD;

	[Header("扫射子弹")]
	public float continualAttackCD;

	public float continualBulletInterval;

	public float continualBulletSpeed;

	public VariableFloat continualBulletTime;

	private bool continualShooting;

	public VariableFloat continualBulletOffset;

	[Header("创人")]
	public float dashAttackCD;

	public float dashSpeed;

	public float dashTime;

	public float dashRedirectDistance;

	public float dashRedirectAngleRange;

	public float dashRedirectPredictTime;

	public float dashWarningLineLength;

	public float dashBlockBulletInterval;

	public Boss54_DamageZone damageZone;

	public LineRenderer warningLine;

	public ParticleSystem dashParticle;

	private Vector3 dashDir;

	private float dashBlockBulletTimer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

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

	public float attackCD => skillType switch
	{
		MonsterState.BornIdle => 5f, 
		MonsterState.Directional => directionalAttackCD, 
		MonsterState.LaserCharge => laserAttackCD, 
		MonsterState.StraightBullet => straightAttackCD, 
		MonsterState.TrackBullet => trackAttackCD, 
		MonsterState.Continual => continualAttackCD, 
		MonsterState.DashPrepare => dashAttackCD, 
		_ => 5f, 
	};

	private void OnEnable()
	{
		base.Anima.Play("Born");
		base.Anima.Update(0f);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		componentData.CanBeTarget = false;
		componentData.InvincibleRegister();
		SetComponentData(componentData);
	}

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90461);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		state = MonsterState.BornIdle;
		skillType = MonsterState.BornIdle;
		warningLine.enabled = false;
	}

	public void SetSkill(MonsterState skill)
	{
		if (skill != MonsterState.DashPrepare && skill != MonsterState.Dash && skill != MonsterState.DashAfter)
		{
			skillType = skill;
			attackCDTimer = attackCD - Random.value;
		}
	}

	public void SyncDashPrepare()
	{
		skillType = MonsterState.BornIdle;
		attackCDTimer = 0f;
		state = MonsterState.DashPrepare;
	}

	public void SyncDash()
	{
		skillType = MonsterState.BornIdle;
		attackCDTimer = 0f;
		state = MonsterState.Dash;
	}

	public void SyncDashAfter()
	{
		skillType = MonsterState.BornIdle;
		attackCDTimer = 0f;
		state = MonsterState.DashAfter;
	}

	public void SyncDashExit()
	{
		skillType = MonsterState.BornIdle;
		attackCDTimer = 0f;
		if (state == MonsterState.DashPrepare || state == MonsterState.Dash || state == MonsterState.DashAfter)
		{
			warningLine.enabled = false;
			damageZone.Close();
			state = MonsterState.RandomMove;
		}
	}

	private void TryAttack()
	{
		if (skillType != 0)
		{
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD)
			{
				attackCDTimer = 0f;
				state = skillType;
			}
		}
	}

	private void KeepMoving()
	{
		if (navInfo.allCornerArrived)
		{
			stateExistTime = 0f;
			randomMoveRadius.RandomResult();
			GetNavInfo(PointWithinRange(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result));
		}
		else
		{
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
		}
	}

	private Vector3 PointWithinRange(Vector3 startPoint)
	{
		return Tool2D.PointWithinRange(startPoint, roomCenterPoint, roomWidth - 2f, roomHeight - 2f);
	}

	public void Initialize()
	{
		base.Anima.Play("Born");
		base.Anima.Update(0f);
		myPpt.RemoveSRFromArray(SR_Born);
		SetSingleFlip(SR_Born, 0f - ToPointDir(Boss54.Inst.transform.position).x, srFlip: false);
		SetFlip(0f - ToPointDir(Boss54.Inst.transform.position).x);
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
			_ = changedState;
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			KeepMoving();
			TryAttack();
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			break;
		case MonsterState.Directional:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			KeepMoving();
			break;
		case MonsterState.LaserCharge:
			if (changedState)
			{
				base.Anima.Play("LaserCharge");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Laser:
			if (changedState)
			{
				base.Anima.Play("Laser");
				thisLaser.SetWarningFinish();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.LaserAfter:
			if (changedState)
			{
				base.Anima.Play("LaserAfter");
			}
			KeepMoving();
			break;
		case MonsterState.StraightBullet:
		{
			ref Vector3 reference3 = ref varMgr.RegV3(0);
			ref float reference4 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("Straight");
				reference3 = ToPointDir(PlayerMgr.Inst.PlayerPoint);
				reference3 = Tool2D.GetDir(reference3, straightBulletOffset.RandomResult());
				straightBulletCounter = 0;
				straightBulletShooting = false;
				reference4 = straightBulletInterval;
				SetFlip(reference3.x);
			}
			if (straightBulletShooting)
			{
				reference4 += Time.deltaTime;
			}
			if (reference4 > straightBulletInterval && straightBulletCounter < straightBulletCount)
			{
				bulletParticle.Play();
				straightBulletCounter++;
				reference4 -= straightBulletInterval;
				SEMgr.Inst.boss54ChildStraight.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_Bullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(reference3, straightBulletSmallOffset.RandomResult()), straightBulletSpeed, myPpt.myEntity);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.TrackBullet:
			if (changedState)
			{
				base.Anima.Play("Track");
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Continual:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref float reference2 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("ContinualCharge");
				continualShooting = false;
				continualBulletTime.RandomResult();
			}
			if (continualShooting)
			{
				reference += Time.deltaTime;
				reference2 += Time.deltaTime;
				if (reference2 > continualBulletInterval)
				{
					reference2 -= continualBulletInterval;
					ContinualAttack();
				}
				if (reference > continualBulletTime.result)
				{
					state = MonsterState.RandomMove;
					break;
				}
			}
			KeepMoving();
			break;
		}
		case MonsterState.DashPrepare:
			if (changedState)
			{
				base.Anima.Play("DashCharge");
				GetNearestTargetPlayerFirst();
				dashDir = Tool2D.GetDir();
				warningLine.enabled = true;
				warningLine.positionCount = 10;
			}
			SetFlip(dashDir.x);
			SetDashWarningLine();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Dash:
			if (changedState)
			{
				base.Anima.Play("Dash");
				warningLine.enabled = false;
				damageZone.Open();
				SetDashVelocity(dashDir);
				dashBlockBulletTimer = 0f;
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				componentData2.IsVelocityDeclice = false;
				componentData2.ImmuneKnockbackRegister();
				SetComponentData(componentData2);
				dashParticle.Play();
			}
			SetFlip(dashDir.x);
			CheckDashOutOfRoom();
			KeepDashVelocity();
			dashBlockBulletTimer += Time.deltaTime;
			if (dashBlockBulletTimer > dashBlockBulletInterval)
			{
				dashBlockBulletTimer -= dashBlockBulletInterval;
				ShootDashBlockBullet();
			}
			break;
		case MonsterState.DashAfter:
			if (changedState)
			{
				base.Anima.Play("DashAfter");
				warningLine.enabled = false;
				damageZone.Close();
				dashParticle.Stop();
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				componentData.IsVelocityDeclice = true;
				componentData.ImmuneKnockbackUnregister();
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
	}

	private void DirectionalAttack()
	{
		bulletParticle.Play();
		for (int i = 0; i < 8; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_Bullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(45 * i), spellSpeed, myPpt.myEntity);
		}
	}

	private void ContinualAttack()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_Bullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(ToPointDir(PlayerMgr.Inst.PlayerPoint), continualBulletOffset.RandomResult()), continualBulletSpeed, myPpt.myEntity);
	}

	private void ShootBlockBullet()
	{
		blockSpellCount.RandomResult();
		for (int i = 0; i < blockSpellCount.result; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BlockBullet", base.transform.position).GetComponent<Boss54Bullet>().InitializeBlock(myPpt.myEntity);
		}
	}

	private void ShootDashBlockBullet()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BlockBulletSlow", base.transform.position).GetComponent<Boss54Bullet>().InitializeBlock(myPpt.myEntity);
	}

	private void CreateLaser()
	{
		thisLaser = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_Line", base.transform.position).GetComponent<Boss54_Line>();
		Tool2D.GetDir();
		Vector3 point = PlayerMgr.Inst.PlayerPointIgnoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * laserPredictTime;
		if (GeneralTool.ChanceResult(laserAimCloseChance))
		{
			point += Tool2D.GetDir() * laserAimOffsetClose.RandomResult();
		}
		else
		{
			point += Tool2D.GetDir() * laserAimOffsetFar.RandomResult();
		}
		SetFlip(ToPointDir(point).x);
		thisLaser.Initialize(this, ToPointDir(point));
	}

	public void LaserFinish()
	{
		if (!myPpt.AlreadyDead && state == MonsterState.Laser)
		{
			state = MonsterState.LaserAfter;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "DashPrepareFinish":
			break;
		case "DashFinish":
			break;
		case "DashAfterFinish":
			break;
		case "BornEffect":
			bulletParticle.Play();
			break;
		case "BornFinish":
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			componentData.CanBeTarget = true;
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			state = MonsterState.RandomMove;
			break;
		}
		case "AttackFinish":
			state = MonsterState.RandomMove;
			break;
		case "Attack":
			SEMgr.Inst.boss54ChildDirectional.PlaySE();
			DirectionalAttack();
			break;
		case "CreateLaser":
			CreateLaser();
			break;
		case "LaserChargeFinish":
			state = MonsterState.Laser;
			break;
		case "LaserFinish":
			state = MonsterState.LaserAfter;
			break;
		case "ShootStraightBullet":
			straightBulletShooting = true;
			ShootBlockBullet();
			break;
		case "ChanceShootTrackBullet":
			if (!GeneralTool.ChanceResult(0.5f))
			{
				bulletParticle.Play();
				SEMgr.Inst.boss54ChildStraight.PlaySE();
				Boss54Bullet component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_TrackBullet", base.transform.position).GetComponent<Boss54Bullet>();
				component2.Initialize(GetTrackBulletDirection(), component2.speed, myPpt.myEntity);
			}
			break;
		case "ShootTrackBullet":
		{
			bulletParticle.Play();
			SEMgr.Inst.boss54ChildStraight.PlaySE();
			Boss54Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_TrackBullet", base.transform.position).GetComponent<Boss54Bullet>();
			component.Initialize(GetTrackBulletDirection(), component.speed, myPpt.myEntity);
			break;
		}
		case "ContinualChargeFinish":
			base.Anima.Play("Continual");
			continualShooting = true;
			break;
		}
	}

	private Vector3 GetTrackBulletDirection()
	{
		Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
		float degree = ((Random.value < 0.5f) ? Random.Range(45f, 135f) : Random.Range(225f, 315f));
		return Tool2D.GetDir(oldDir, degree).normalized;
	}

	private void SetDashVelocity(Vector3 dir)
	{
		dashDir = ((dir.sqrMagnitude > 0f) ? dir.normalized : Tool2D.GetDir());
		base.Rigid.linearVelocity = dashDir * dashSpeed;
		SyncDotsVelocity();
	}

	private void KeepDashVelocity()
	{
		float num = dashSpeed * dashSpeed;
		if (base.Rigid.linearVelocity.sqrMagnitude < num * 0.95f || base.Rigid.linearVelocity.sqrMagnitude > num * 1.05f)
		{
			SetDashVelocity((base.Rigid.linearVelocity.sqrMagnitude > 0f) ? base.Rigid.linearVelocity : dashDir);
		}
	}

	private void SetDashWarningLine()
	{
		for (int i = 0; i < warningLine.positionCount; i++)
		{
			Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * dashWarningLineLength, (float)i / (float)(warningLine.positionCount - 1));
			warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
		}
	}

	private void CheckDashOutOfRoom()
	{
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position);
		Vector3 vector2 = Tool2D.PointWithinRange(vector, roomCenterPoint, roomWidth, roomHeight);
		if (!((vector2 - vector).sqrMagnitude < 0.0001f))
		{
			Vector3 zero = Vector3.zero;
			float num = roomWidth * 0.5f;
			float num2 = roomHeight * 0.5f;
			Vector3 vector3 = vector - roomCenterPoint;
			if (vector3.x > num)
			{
				zero += Vector3.left;
			}
			else if (vector3.x < 0f - num)
			{
				zero += Vector3.right;
			}
			if (vector3.y > num2)
			{
				zero += Vector3.down;
			}
			else if (vector3.y < 0f - num2)
			{
				zero += Vector3.up;
			}
			base.transform.position = vector2;
			SyncDotsPosition();
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget && ToTargetDistance() >= dashRedirectDistance)
			{
				Vector3 point = PlayerMgr.Inst.PlayerPointIgnoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * dashRedirectPredictTime;
				SetDashVelocity(Tool2D.GetDir(ToPointDir(point), Random.Range(0f - dashRedirectAngleRange, dashRedirectAngleRange)));
			}
			else
			{
				SetDashVelocity(Vector3.Reflect(dashDir, zero.normalized));
			}
		}
	}
}
