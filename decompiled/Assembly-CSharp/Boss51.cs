using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss51 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		JumpPrepare,
		Jump,
		JumpAfter,
		CannonCharge,
		Cannon,
		CannonAfter,
		StrafeCharge,
		Strafe,
		StarfeAfter,
		DashCharge,
		DashWarningLine,
		Dash,
		DashAfter,
		MoveStrafeCharge,
		MoveStrafe,
		MoveStrafeAfter,
		LineAttackPrepare,
		LineAttack,
		SplitFirePrepare,
		SplitFire,
		Dead
	}

	[Header("行动")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public static Boss51 Inst;

	public VariableFloat idleTime;

	public VariableFloat attackCdTime;

	private float attackCdTimer;

	[Header("通用表现")]
	public Transform tsf_Model;

	public Boss51_Hand hand;

	[Header("技能配置")]
	public float jumpChance;

	public float paraMissileChance;

	public float strafeChance;

	public float dashChance;

	public float moveStrafeChance;

	public float lineAttackChance;

	public float splitFireChance;

	private int lastSkillIndex;

	public MonsterState testingState;

	public bool testingSkill;

	[Header("火焰时间配置")]
	public float quickFireChance;

	public int slowFireMaxCount;

	public int quickFireMaxCount;

	private int sameTypeFireMaxCounter;

	private bool lastIsQuickFire;

	[Header("火焰榴弹")]
	public ParticleSystem cannonParticle;

	public float paraMissileShootInterval;

	public VariableInt paraMissileCount;

	public VariableFloat paramissileOffset;

	public float paraMissileFarChance;

	public VariableFloat paramissileOffsetFar;

	public float paraMissileFlyTime;

	public float doubleParaChance;

	[Header("跳砸，不要近战砸地了")]
	public float jumpPrepareTime;

	public float jumpTime;

	public ShockParam meleeShockParam;

	public float jumpAttackDamage;

	public float jumpAttackKnockBack;

	public float jumpAttackRadius;

	public float jumpGravity;

	public int jumpBulletCount;

	public VariableFloat jumpBulletTime;

	public VariableFloat jumpBulletSpeed;

	private WarningArea warningArea;

	private Vector3 jumpTargetPoint;

	[Header("冲刺")]
	public ParticleSystem dashPrepareParticle;

	public ParticleSystem dashParticle;

	public float dashChargeTime;

	public Vector3 dashDir;

	public LineRenderer dashWarningLine;

	public VariableFloat dashTime;

	public float dashSpeed;

	public VariableFloat dashAmount;

	public float dashWarningLineTime;

	private Vector3 dashStartPivot;

	private int dashAmountCounter;

	public float dashBulletInterval;

	public VariableFloat dashBulletTime;

	public VariableFloat dashBulletSpeed;

	[Header("火焰子弹")]
	public VariableFloat strafeAmount;

	public float strafeChargeTime;

	public float strafeInterval;

	private float strafeIntervalTimer;

	public int strafeBulletAmount;

	public int strafeSectorCount;

	public float strafeSafeAngle;

	public float strafeBulletSpeed;

	public ParticleSystem strafeParticle;

	public ParticleSystem strafePrepareParticle;

	public float strafeParaBulletCount;

	public VariableFloat strafeParaBulletTime;

	public VariableFloat strafeParaBulletSpeed;

	[Header("移动扫射")]
	public float strafeBulletHeight;

	public float moveStrafeDuration;

	public float moveStrafeAngle;

	public VariableFloat moveStrafeBulletSpeed;

	private float moveStrafeShootIntervalTimer;

	public float moveStrafeShootInterval;

	public float moveStrafeMoveSpeedFactor;

	public ParticleSystem flameThrowerParticle;

	[Header("火焰波动")]
	public float LineAttackRadius;

	public VariableFloat LineAttackAimOffset;

	public ShockParam lineAttackShock;

	public ShockParam lineAttackExplodeShock;

	public VariableInt lineAttackBulletCount;

	[Header("分裂火柱")]
	public float splitFireInterval;

	public int splitFireCount;

	public VariableFloat splitFireOffsetRange;

	[Header("音效")]
	public AudioSource AS_FlameThrower;

	public AudioSource AS_Lava;

	private float asFadeValue;

	private float finalSoundValue;

	[Header("二阶段")]
	public bool secondStageOverride;

	private bool enterSecondStage;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private List<int> fireSkills = new List<int> { 0, 1, 3, 5 };

	private int lastFireSkillIndex = -1;

	private int lastNonFireSkillIndex = -1;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private float lastStrafeAngle;

	private float lastOffsetAngle;

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

	public Entity thisEntity { get; set; }

	public bool isSecondStage
	{
		get
		{
			UnitConfig unitCfg = GetComponentData<UnitProperty_Dots>(myPpt.myEntity).unitCfg;
			if (!secondStageOverride)
			{
				return unitCfg.currentHP / unitCfg.maxHP < 0.5f;
			}
			return true;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		dashWarningLine.positionCount = 10;
		lastSkillIndex = GeneralTool.GetWeightRandom(jumpChance, paraMissileChance, strafeChance, dashChance, moveStrafeChance);
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		attackCdTime.RandomResult();
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		finalSoundValue = DataMgr.settingData.GetFinalSound();
		SetVolumes();
	}

	private void SetVolumes()
	{
		AS_FlameThrower.volume = finalSoundValue * asFadeValue;
		AS_Lava.volume = finalSoundValue * asFadeValue;
	}

	protected override void SetFlip(float motionX)
	{
		if (Mathf.Abs(motionX) > 0.01f)
		{
			tsf_Model.localScale = new Vector3(Mathf.Sign(motionX), 1f, 1f);
		}
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
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Idle;
			}
			ChooseSkill();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Move;
				break;
			}
			ChooseSkill();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			ChooseSkill();
			SetMove(base.MoveSpeed * Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
			break;
		case MonsterState.JumpPrepare:
			if (changedState)
			{
				base.Anima.Play("JumpPrepare");
				GetNearestTargetPlayerFirst();
				jumpTargetPoint = (base.HaveTarget ? base.TargetPoint : (roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 10f)));
				warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), jumpTargetPoint).GetComponent<WarningArea>();
				warningArea.Initialize(jumpAttackRadius, 1f, zoomDirect: false);
			}
			if (base.HaveTarget)
			{
				jumpTargetPoint = base.TargetPoint;
			}
			jumpTargetPoint = Tool2D.PointWithinRange(jumpTargetPoint, roomCenterPoint, roomWidth - 3f, roomHeight - 3f);
			warningArea.transform.position = jumpTargetPoint;
			warningArea.tsf_Fill.localScale = Vector3.one * stateExistTime / (jumpPrepareTime + jumpTime) * jumpAttackRadius * 2f;
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			break;
		case MonsterState.Jump:
			if (changedState)
			{
				base.Anima.Play("Jump");
				PhysicsCollider pc = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc, 8192u, DTool.GetCollidesWith(8192u));
				SetComponentData(pc);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = false;
				SetComponentData(componentData);
				float upForce = jumpTime * jumpGravity * -0.5f;
				JumpStart_Dots(upForce, jumpGravity);
				Vector3 linearVelocity = ToPointDistance(jumpTargetPoint) / jumpTime * ToPointDir(jumpTargetPoint).normalized;
				base.Rigid.linearVelocity = linearVelocity;
				SyncDotsVelocity();
			}
			warningArea.transform.position = jumpTargetPoint;
			warningArea.tsf_Fill.localScale = Vector3.one * (stateExistTime + jumpPrepareTime) / (jumpPrepareTime + jumpTime) * jumpAttackRadius * 2f;
			SetMove(Vector3.zero, isFlip: false);
			if (base.isFalling && base.transform.position.z >= 0f)
			{
				state = MonsterState.JumpAfter;
			}
			break;
		case MonsterState.JumpAfter:
			if (changedState)
			{
				base.Anima.Play("JumpAfter");
				KnockGround();
				PhysicsCollider pc2 = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc2, 2048u, DTool.GetCollidesWith(2048u));
				SetComponentData(pc2);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = true;
				SetComponentData(componentData2);
				JumpStop_Dots();
				ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.CannonCharge:
			if (changedState)
			{
				base.Anima.Play("CannonPrepare");
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			break;
		case MonsterState.Cannon:
		{
			ref int reference6 = ref varMgr.RegInt(0);
			ref float reference7 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("Cannon");
				paraMissileCount.RandomResult();
			}
			reference7 += Time.deltaTime;
			if (reference7 > paraMissileShootInterval)
			{
				reference7 = 0f;
				reference6++;
				ShootCannon();
				if (GeneralTool.ChanceResult(doubleParaChance))
				{
					ShootCannon();
				}
				if (reference6 > paraMissileCount.result)
				{
					state = MonsterState.CannonAfter;
				}
			}
			SetMove(base.MoveSpeed * moveStrafeMoveSpeedFactor * Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
			break;
		}
		case MonsterState.CannonAfter:
			if (changedState)
			{
				base.Anima.Play("CannonAfter");
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			break;
		case MonsterState.StrafeCharge:
			if (changedState)
			{
				strafePrepareParticle.Play();
				base.Anima.Play("StrafePrepare");
				strafeIntervalTimer = 0f;
				strafeAmount.RandomResult();
				SEMgr.Inst.boss51StrafePrepare.PlaySE();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Strafe:
		{
			ref int reference5 = ref varMgr.RegInt(0);
			if (changedState)
			{
				base.Anima.Play("Strafe");
				Strafe(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
				reference5++;
				strafeParticle.Play();
			}
			strafeIntervalTimer += Time.deltaTime;
			if (strafeIntervalTimer > strafeInterval)
			{
				if ((float)reference5 >= strafeAmount.result)
				{
					state = MonsterState.StarfeAfter;
					break;
				}
				strafeIntervalTimer = 0f;
				Strafe(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
				reference5++;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.StarfeAfter:
			if (changedState)
			{
				base.Anima.Play("StrafeAfter");
				strafeParticle.Stop();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.DashCharge:
			if (changedState)
			{
				base.Anima.Play("DashPrepare");
				dashDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), UnityEngine.Random.Range(-20f, 20f));
				dashAmountCounter = 0;
				dashAmount.RandomResult();
				dashTime.RandomResult();
				AS_Lava.Play();
			}
			asFadeValue = Mathf.Min(1f, stateExistTime / 0.5f);
			SetVolumes();
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Dash:
		{
			ref float reference4 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("Dash");
				dashWarningLine.enabled = false;
				dashStartPivot = base.transform.position;
				SEMgr.Inst.boss51LavaDrop.PlaySE();
				CreateGroundFire(base.transform.position + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 2f), isLineFire: false, isDashFire: true);
			}
			reference4 += Time.deltaTime;
			while (reference4 > dashBulletInterval)
			{
				reference4 -= dashBulletInterval;
				ShootSingleParaBullet(dashBulletSpeed, dashBulletTime);
			}
			SetMove(dashSpeed * dashDir);
			if (Tool2D.IgnoreZDistance(dashStartPivot, base.transform.position) >= 3f)
			{
				dashStartPivot = base.transform.position;
				SEMgr.Inst.boss51LavaDrop.PlaySE();
				CreateGroundFire(base.transform.position + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 2f), isLineFire: false, isDashFire: true);
			}
			if (stateExistTime > dashTime.result)
			{
				if ((float)dashAmountCounter < dashAmount.result)
				{
					dashAmountCounter++;
					state = MonsterState.DashWarningLine;
				}
				else
				{
					state = MonsterState.DashAfter;
				}
			}
			break;
		}
		case MonsterState.DashWarningLine:
		{
			if (changedState)
			{
				base.Anima.Play("DashAim");
				dashWarningLine.enabled = true;
				dashDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), UnityEngine.Random.Range(-20f, 20f));
				dashTime.RandomResult();
				dashParticle.Play();
			}
			for (int i = 0; i < dashWarningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * dashSpeed * dashTime.result, (float)i / (float)(dashWarningLine.positionCount - 1));
				dashWarningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.DashAfter:
			if (changedState)
			{
				base.Anima.Play("DashAfter");
				dashParticle.Stop();
			}
			asFadeValue = Mathf.Max(0f, (0.5f - stateExistTime) / 0.5f);
			SetVolumes();
			if (asFadeValue == 0f)
			{
				AS_Lava.Stop();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.MoveStrafeCharge:
		{
			if (changedState)
			{
				base.Anima.Play("MoveStrafePrepare");
				SEMgr.Inst.boss51FlameThrowerStart.PlaySE();
				AS_FlameThrower.Play();
				hand.state = Boss51_Hand.HandState.Attack;
			}
			Vector3 targetDir = Tool2D.IgnoreZV2ToV1Normal(v2: new Vector3(flameThrowerParticle.transform.position.x, base.transform.position.y + flameThrowerParticle.transform.position.y - hand.tsf_Hand.position.y, base.transform.position.y - hand.transform.position.y), v1: PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * ToPointDistance(PlayerMgr.Inst.PlayerPoint) / moveStrafeBulletSpeed.value1);
			hand.SetTargetDir(targetDir);
			if (stateExistTime > strafeChargeTime)
			{
				state = MonsterState.MoveStrafe;
				break;
			}
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.MoveStrafe:
		{
			if (changedState)
			{
				base.Anima.Play("MoveStrafe");
				moveStrafeShootIntervalTimer = 0f;
				AS_FlameThrower.Play();
				flameThrowerParticle.Play();
			}
			asFadeValue = Mathf.Min(1f, stateExistTime / 0.5f);
			SetVolumes();
			Vector3 targetDir = Tool2D.IgnoreZV2ToV1Normal(v2: new Vector3(flameThrowerParticle.transform.position.x, base.transform.position.y + flameThrowerParticle.transform.position.y - hand.tsf_Hand.position.y, base.transform.position.y - hand.transform.position.y), v1: PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * ToPointDistance(PlayerMgr.Inst.PlayerPoint) / moveStrafeBulletSpeed.value2);
			Tool2D.IgnoreZAngleWithSign(Vector3.up, targetDir);
			hand.SetTargetDir(targetDir);
			moveStrafeShootIntervalTimer += Time.deltaTime;
			if (moveStrafeShootIntervalTimer > moveStrafeShootInterval)
			{
				moveStrafeShootIntervalTimer = 0f;
				MoveShoot(targetDir);
			}
			if (stateExistTime > moveStrafeDuration)
			{
				state = MonsterState.MoveStrafeAfter;
			}
			SetMove(base.MoveSpeed * moveStrafeMoveSpeedFactor * Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
			break;
		}
		case MonsterState.MoveStrafeAfter:
			if (changedState)
			{
				base.Anima.Play("MoveStrafeAfter");
				moveStrafeShootIntervalTimer = 0f;
				flameThrowerParticle.Stop();
				hand.state = Boss51_Hand.HandState.Idle;
			}
			asFadeValue = Mathf.Max(0f, (0.5f - stateExistTime) / 0.5f);
			SetVolumes();
			if (asFadeValue == 0f)
			{
				AS_FlameThrower.Stop();
			}
			SetMove(base.MoveSpeed * moveStrafeMoveSpeedFactor * Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
			break;
		case MonsterState.LineAttackPrepare:
			if (changedState)
			{
				base.Anima.Play("LineAttackPrepare");
			}
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.LineAttack:
			if (changedState)
			{
				base.Anima.Play("LineAttack");
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
				SEMgr.Inst.boss51LineAttack.PlaySE();
				CamController.Inst.SetShock(lineAttackShock);
				Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(Tool2D.GetDir() * LineAttackAimOffset.RandomResult() + PlayerMgr.Inst.PlayerPoint);
				float num = Tool2D.IgnoreZDistance(navMeshPointIngoreZ, base.transform.position);
				Boss51_LineFire component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_LineFire", base.transform.position).GetComponent<Boss51_LineFire>();
				float num2 = num / (component.distanceInterval / component.timeInterval);
				Vector3 direction = Tool2D.IgnoreZV2ToV1Normal(navMeshPointIngoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * num2, base.transform.position);
				component.Initialize(base.transform.position, direction);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SplitFirePrepare:
			if (changedState)
			{
				base.Anima.Play("SplitFirePrepare");
			}
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SplitFire:
		{
			ref int reference = ref varMgr.RegInt(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			ref bool reference3 = ref varMgr.RegBool(0);
			if (changedState)
			{
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
				SEMgr.Inst.boss51LineAttack.PlaySE();
				CamController.Inst.SetShock(lineAttackShock);
				base.Anima.Play("SplitFire");
				reference3 = GeneralTool.ChanceResult(0.5f);
			}
			reference2 += Time.deltaTime;
			if (reference2 > splitFireInterval && reference < splitFireCount)
			{
				reference2 -= splitFireInterval;
				Vector3 startPoint = PlayerMgr.Inst.PlayerPointIgnoreZ + Tool2D.GetDir() * splitFireOffsetRange.RandomResult();
				startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_CrossBurstFire", startPoint).GetComponent<Boss51_CrossBurstFire>().Initialize(reference3);
				reference++;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
				JumpStop_Dots();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
	}

	public void ChooseSkill()
	{
		attackCdTimer += Time.deltaTime;
		if (!(attackCdTimer > attackCdTime.result))
		{
			return;
		}
		attackCdTimer = 0f;
		attackCdTime.RandomResult();
		if (testingSkill)
		{
			state = testingState;
			return;
		}
		if (!enterSecondStage && isSecondStage)
		{
			enterSecondStage = true;
			state = MonsterState.StrafeCharge;
			lastSkillIndex = 2;
			return;
		}
		bool flag = false;
		while (!flag)
		{
			int weightRandom = GeneralTool.GetWeightRandom(0f, paraMissileChance, 0f, dashChance, moveStrafeChance, lineAttackChance, splitFireChance);
			if (isSecondStage)
			{
				weightRandom = GeneralTool.GetWeightRandom(jumpChance, paraMissileChance, strafeChance, dashChance, moveStrafeChance, lineAttackChance, splitFireChance);
			}
			if (weightRandom != lastSkillIndex && (fireSkills.Contains(weightRandom) ^ fireSkills.Contains(lastSkillIndex)) && (!fireSkills.Contains(weightRandom) || lastFireSkillIndex != weightRandom) && (fireSkills.Contains(weightRandom) || lastNonFireSkillIndex != weightRandom))
			{
				lastSkillIndex = weightRandom;
				if (fireSkills.Contains(weightRandom))
				{
					lastFireSkillIndex = weightRandom;
				}
				if (!fireSkills.Contains(weightRandom))
				{
					lastNonFireSkillIndex = weightRandom;
				}
				flag = true;
				switch (weightRandom)
				{
				case 0:
					state = MonsterState.JumpPrepare;
					break;
				case 1:
					state = MonsterState.CannonCharge;
					break;
				case 2:
					state = MonsterState.StrafeCharge;
					break;
				case 3:
					state = MonsterState.DashCharge;
					break;
				case 4:
					state = MonsterState.MoveStrafeCharge;
					break;
				case 5:
					state = MonsterState.LineAttackPrepare;
					break;
				case 6:
					state = MonsterState.SplitFirePrepare;
					break;
				}
				break;
			}
		}
	}

	public void ShootSingleParaBullet(VariableFloat speed, VariableFloat time, Vector3 point, float height)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_Bullet", Tool2D.IgnoreZPoint(point, 0f - height)).GetComponent<Boss51Bullet>().InitializePara(Tool2D.GetDir(), speed.RandomResult(), time.RandomResult());
	}

	public void ShootSingleParaBullet(VariableFloat speed, VariableFloat time)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_Bullet", Tool2D.IgnoreZPoint(base.transform.position, -1f)).GetComponent<Boss51Bullet>().InitializePara(Tool2D.GetDir(), speed.RandomResult(), time.RandomResult());
	}

	public void KnockGround()
	{
		CamController.Inst.SetShock(meleeShockParam);
		for (int i = 0; i < jumpBulletCount; i++)
		{
			ShootSingleParaBullet(jumpBulletSpeed, jumpBulletTime);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster313_GroundFireBig", base.transform.position);
		SEMgr.Inst.boss51JumpGround.PlaySE();
		SEMgr.Inst.boss51JumpGroundFire.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, jumpAttackRadius, GameConst.Filter_MonsterAoe, distanceHits);
		foreach (UnitDotsSyncSystem.DistanceHitResult distanceHit in distanceHits)
		{
			Entity entity = distanceHit.entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Inst.myPpt.myEntity);
			info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHit.point, base.transform.position) * jumpAttackKnockBack;
			info.damage = jumpAttackDamage;
			switch (layer)
			{
			case 512u:
			case 2097152u:
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			case 32768u:
			case 131072u:
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			}
		}
	}

	public void ShootCannon()
	{
		cannonParticle.Play();
		SEMgr.Inst.boss51Cannon.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1f);
		Boss51_ParaMissile component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_ParaMissile", base.transform.position + new Vector3(0f, 0f, -1f)).GetComponent<Boss51_ParaMissile>();
		float num = (GeneralTool.ChanceResult(paraMissileFarChance) ? paramissileOffsetFar.RandomResult() : paramissileOffset.RandomResult());
		Vector3 startPoint = PlayerMgr.Inst.PlayerPoint + paraMissileFlyTime * UnityEngine.Random.value * PlayerMgr.Inst.PlayerCtrller.CurrentMotion + Tool2D.GetDir() * num;
		component.InitializeCannon(endPoint: Tool2D.GetNavMeshPointIngoreZ(startPoint), startPoint: component.transform.position, time: paraMissileFlyTime, master: thisEntity);
	}

	public void CreateGroundFire(Vector3 position, bool isLineFire = false, bool isDashFire = false)
	{
		position = Tool2D.IgnoreZPoint(position);
		bool flag = GeneralTool.ChanceResult(quickFireChance);
		if (flag == lastIsQuickFire)
		{
			if ((flag && sameTypeFireMaxCounter > quickFireMaxCount) || (!flag && sameTypeFireMaxCounter > slowFireMaxCount))
			{
				flag = !flag;
				sameTypeFireMaxCounter = 0;
			}
			else
			{
				sameTypeFireMaxCounter++;
			}
		}
		else
		{
			sameTypeFireMaxCounter = 0;
		}
		if (isLineFire)
		{
			SEMgr.Inst.boss51LineAttackSingle.PlaySE();
			lineAttackBulletCount.RandomResult();
			for (int i = 0; i < lineAttackBulletCount.result; i++)
			{
				ShootSingleParaBullet(dashBulletSpeed, dashBulletTime, position, 0f);
			}
			CamController.Inst.SetShock(lineAttackExplodeShock);
			UnitDotsSyncSystem.GetCollidersInRange(position, LineAttackRadius, GameConst.Filter_MonsterAoe, distanceHits);
			foreach (UnitDotsSyncSystem.DistanceHitResult distanceHit in distanceHits)
			{
				Entity entity = distanceHit.entity;
				uint layer = UnitDotsSyncSystem.GetLayer(entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHit.point, position) * jumpAttackKnockBack;
				info.damage = jumpAttackDamage;
				switch (layer)
				{
				case 512u:
				case 2097152u:
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
					break;
				case 32768u:
				case 131072u:
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
					break;
				}
			}
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster313_GroundFire" + (isLineFire ? "Middle" : "") + (isDashFire ? "Dash" : ""), position).GetComponent<Monster313_GroundFire>().SetDuration(flag);
	}

	public void Strafe(Vector3 aimDir)
	{
		SEMgr.Inst.boss51Strafe.PlaySE();
		float num = 360f / (float)strafeBulletAmount;
		UnityEngine.Random.Range(1, strafeBulletAmount - 1);
		float num2 = 360f / (float)strafeSectorCount;
		Vector3 dir = Tool2D.GetDir(lastStrafeAngle += UnityEngine.Random.Range(num2 * 0.25f, num2 * 0.75f));
		for (int i = 0; i < strafeBulletAmount; i++)
		{
			if (!(num * (float)i % num2 < strafeSafeAngle))
			{
				Vector3 dir2 = Tool2D.GetDir(dir, num * (float)i);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_Bullet", base.transform.position + Vector3.back * strafeBulletHeight).GetComponent<Boss51Bullet>().InitializeSimple(dir2, strafeBulletSpeed);
			}
		}
		for (int j = 0; (float)j < strafeParaBulletCount; j++)
		{
			ShootSingleParaBullet(strafeParaBulletSpeed, strafeParaBulletTime);
		}
	}

	public void MoveShoot(Vector3 aimDir)
	{
		float num = UnityEngine.Random.Range((0f - moveStrafeAngle) / 2f, moveStrafeAngle / 2f);
		if (Mathf.Abs(lastOffsetAngle - num) < 30f)
		{
			num = lastOffsetAngle + Mathf.Abs(num - lastOffsetAngle) * 30f;
		}
		Vector3 point = new Vector3(flameThrowerParticle.transform.position.x, base.transform.position.y + flameThrowerParticle.transform.position.y - hand.tsf_Hand.position.y, 0f - strafeBulletHeight);
		Vector3 dir = Tool2D.GetDir(aimDir, UnityEngine.Random.Range((0f - moveStrafeAngle) / 2f, moveStrafeAngle / 2f));
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_Bullet", point).GetComponent<Boss51Bullet>().InitializeSimple(dir, moveStrafeBulletSpeed.RandomResult());
	}

	protected override void BossDeadStay()
	{
		state = MonsterState.Dead;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		if (warningArea != null)
		{
			ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
		}
		dashWarningLine.enabled = false;
		AS_FlameThrower.Stop();
		AS_Lava.Stop();
		dashParticle.Stop();
		strafeParticle.Stop();
		flameThrowerParticle.Stop();
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (state == MonsterState.Dash && UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(myPpt.myEntity)) == 65536)
		{
			stateExistTime = 9f;
		}
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ChangeStageFinish":
			state = MonsterState.Move;
			break;
		case "JumpPrepareFinish":
			state = MonsterState.Jump;
			break;
		case "JumpAfterFinish":
			state = MonsterState.Move;
			break;
		case "DashPrepareParticle":
			dashPrepareParticle.Play();
			SEMgr.Inst.boss51FlameThrowerStart.PlaySE();
			break;
		case "DashPrepareFinish":
			state = MonsterState.DashWarningLine;
			break;
		case "DashAimFinish":
			state = MonsterState.Dash;
			break;
		case "StrafePrepareFinish":
			state = MonsterState.Strafe;
			break;
		case "StrafeFinish":
			state = MonsterState.Move;
			break;
		case "MoveStrafeParpareFinish":
			state = MonsterState.MoveStrafe;
			break;
		case "CannonPrepareFinish":
			state = MonsterState.Cannon;
			break;
		case "LineAttackFinish":
			state = MonsterState.LineAttack;
			break;
		case "SplitFireFinish":
			state = MonsterState.SplitFire;
			break;
		}
	}
}
