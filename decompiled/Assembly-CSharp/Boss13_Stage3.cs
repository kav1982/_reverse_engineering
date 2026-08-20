using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss13_Stage3 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		MissileCharge,
		ShootMissile,
		ShootBulletPrepare,
		ShootBullet,
		MegaMissilePrepare,
		MegaMissileAttack,
		MegaDashPrepare,
		MegaDashPrepareFly,
		MegaDash,
		MegaDashAfter,
		MegaSkillsAfter,
		FollowMissileCharge,
		FollowMissileFire,
		Dead,
		DeadAnimation
	}

	[Header("通用属性")]
	public VariableFloat attackCD;

	private float attackCDTimer;

	public float megaSkillExtraCDTime;

	public float megaSkillExtraRestTime;

	public static Boss13_Stage3 Inst;

	public float deadTime;

	public TestController controller;

	[Header("朝向和表现")]
	public MeshRenderer[] meshRenderers;

	[Header("描边")]
	public List<MeshRenderer> ignoreShineRenderers;

	public Texture2D damagedTexture;

	[Header("技能选择")]
	public float followMissileChance;

	public float aroundMissileChance;

	public float shootBulletChance;

	public VariableInt normalSkillCount;

	private float normalSkillCounter;

	private int lastAttackType;

	private int lastMegaAttackType;

	[Header("子弹")]
	public float spellDamage;

	public float spellDuration;

	public float spellSpeed;

	public float spellHeight;

	public float diffuseDistance;

	private float aimAngle;

	[Header("扇形子弹")]
	public float bulletPrepareTime;

	public float bulletSpeed;

	public int bulletCount;

	public float shootBulletSpeedRatio;

	public float bulletAngle;

	public float bulletShootInterval;

	public float bulletRounds;

	public ShockParam shootShock;

	[Header("待机")]
	public VariableFloat idleTime;

	[Header("移动和表现")]
	public Transform modelRotateRoot;

	public Transform modelTsfRoot;

	public Transform modelFloatRoot;

	public Transform modelTiltRoot;

	public Transform modelShakeRoot;

	public Transform shadowRotateRoot;

	public Transform shadowTsfRoot;

	public Transform shadowTiltRoot;

	public Transform shadowShakeRoot;

	public Vector3 lookDir;

	public float turnSpeed;

	public float turnSmoothAngle;

	public VariableFloat moveTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public VariableFloat randomMoveKeepDistance;

	private Vector3 randomMoveTarget;

	public float modelFloatAmplitude;

	public float modelFloatFrequency;

	public float modelFloatBaseHeight;

	private float modelFloatRatio;

	private float modelFloatPhase;

	public float modelMaxTiltAngle;

	public float modelMaxTiltSpeed;

	public float shootPointFront;

	public float shootPointSide;

	private float shootPointAngle;

	private float shootPointMagnitude;

	public float shakeFrequency;

	public float shakeAmplitude;

	public float maxPrepareHeight;

	private float prepareExtraShakeFix;

	public float shootShakeAmplitude;

	public float shootShakeAmplitudeBig;

	public float shootShakeFadeSpeed;

	private float shootExtraShake;

	private Vector2 berlinSeedX;

	private Vector2 berlinSeedY;

	private float nowShakePhase;

	private float prepareExtraHeightFix;

	public float heightRecoverTime;

	public float shootTiltAmplitude;

	private float prepareExtraTiltFix;

	public AnimationCurve PrepareShakeCurve;

	public AnimationCurve PrepareAfterShakeCurve;

	public AnimationCurve megaAttackPrepareShakeCurve;

	public AnimationCurve megaAttackPrepareAfterShakeCurve;

	public AnimationCurve prepareHeightCurve;

	public AnimationCurve prepareAfterHeightCurve;

	public AnimationCurve megaAttackPrepareHeightCurve;

	public AnimationCurve megaAttackPrepareAfterHeightCurve;

	public AnimationCurve prepareAfterTiltCurve;

	public AnimationCurve megaAttackPrepareAfterTiltCurve;

	[Header("延迟导弹")]
	public float delayMissileChargeTime;

	public float delayMissileTime;

	public float delayMissileShootTime;

	public float delayMissileCount;

	public float delayMissileAngleOffset;

	[Header("跟踪导弹")]
	public ParticleSystem leftShootParticle;

	public ParticleSystem rightShootParticle;

	public float bodyRotateSpeed;

	public float followMissileChargeTime;

	public float followMissileShootInterval;

	[Header("超级导弹")]
	public float megaAttackPrepareTime;

	public float megaAttackDampTime;

	public float megaAttackAfterTime;

	private Vector3 megaAttackDampSpeed;

	public float megaMissileAttackTime;

	public float megaMissileShootInterval;

	[Header("死亡雾鲛！")]
	public Boss13DashDamageCheck dashDamageZone;

	public ParticleSystem dashPrepareParticle;

	public ParticleSystem dashContinuedParticle;

	public ParticleSystem dashBubbleParticle;

	public float dashPrepareTime;

	public float dashAfterTime;

	public float soundDelayTime;

	public float dashTimes;

	private float dashTimesCounter;

	public float dashPrepareHeight;

	public float dashPrepareFlyHeight;

	public AnimationCurve dashPrepareHeightCurve;

	public AnimationCurve dashAfterHeightCurve;

	public float dashPrepareDistance;

	public AnimationCurve dashPrepareDistanceCurve;

	public AnimationCurve dashAfterDistanceCurve;

	public float dashPrepareAngle;

	public AnimationCurve dashPrepareAngleCurve;

	public AnimationCurve dashAfterAngleCurve;

	public float dashInOutHeight;

	public AnimationCurve dashInHeightCurve;

	public AnimationCurve dashOutHeightCurve;

	public float dashSpeed;

	public float dashWholeDistance;

	public float dashBulletSpawnInterval;

	public float dashPredictTime;

	public float dashDelayTime;

	public float dashRandomRadius;

	private Vector3 dashDir;

	private float dashDirRotateRight;

	private float dashExtraHeight;

	private float dashExtraTilt;

	public ShockParam megaDashShock;

	public LineRenderer dashWarningLine;

	public LineRenderer dashWarningLine_H;

	private Vector3 roomCenter;

	private float roomWidth;

	private float roomHeight;

	private Vector3 fakeTarget;

	[Header("影子")]
	public MeshRenderer shadow;

	[Header("音效")]
	public AudioSource AS_BackGround;

	[Header("死亡爆炸")]
	public float deadExplosionInterval;

	public float deadExplosionRadius;

	public ShockParam deadExplosionShock;

	public float deadExplosionTime;

	public float deadAllTime;

	public float deadShake;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams ssp;

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
		if (GameMgr.IsMobile_Static)
		{
			dashSpeed *= 0.9f;
			dashBulletSpawnInterval *= 1.1f;
			bulletAngle *= 1.1f;
			delayMissileCount -= 1f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		for (int i = 0; i < ignoreShineRenderers.Count; i++)
		{
			myPpt.RemoveMRFromArray(ignoreShineRenderers[i]);
		}
		myPpt.RemoveMRFromArray(shadow);
		shadow.transform.localScale = new Vector3(Boss13.shadowMRScale.x, Boss13.shadowMRScale.y, 1f);
		shadow.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		Boss13.camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		shadow.material.SetTexture("_MainTex", Boss13.shadowRT);
		shadow.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
		shootPointAngle = Tool2D.IgnoreZAngle(Vector3.right, new Vector3(shootPointFront, shootPointSide, 0f));
		shootPointMagnitude = Vector3.Magnitude(new Vector3(shootPointFront, shootPointSide, 0f));
		berlinSeedX = Tool2D.GetDir();
		berlinSeedY = Tool2D.GetDir(berlinSeedX, 90f);
		if (GameMgr.IsChAge14_Static)
		{
			dashWarningLine = dashWarningLine_H;
		}
		dashWarningLine.widthMultiplier = base.CC_Self.radius * 2.2f;
		dashWarningLine.positionCount = 10;
		normalSkillCounter = 2f;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		Inst = this;
		lookDir = Vector3.left;
		UpdateModel();
		normalSkillCount.RandomResult();
		state = MonsterState.BornIdle;
		attackCDTimer = attackCD.RandomResult() / 2f;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		lastMegaAttackType = UnityEngine.Random.Range(0, 2);
	}

	private Vector3 GetInvisiblePoint(float percent)
	{
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * UnityEngine.Random.Range(3, 7));
		return GetInvisiblePoint(navMeshPointIngoreZ, percent);
	}

	private Vector3 GetInvisiblePoint(Vector3 origin, float percent)
	{
		float num = roomWidth * percent;
		float num2 = roomHeight * percent;
		origin.x = Mathf.Clamp(origin.x, roomCenter.x - num, roomCenter.x + num);
		origin.y = Mathf.Clamp(origin.y, roomCenter.y - num2, roomCenter.y + num2);
		return origin;
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
		AS_BackGround.volume = DataMgr.settingData.GetFinalSound();
	}

	private void ChooseSkill()
	{
		attackCDTimer += Time.deltaTime;
		if (!(attackCDTimer > attackCD.result))
		{
			return;
		}
		normalSkillCounter += 1f;
		if ((float)normalSkillCount.result < normalSkillCounter)
		{
			if (attackCDTimer > attackCD.result + megaSkillExtraCDTime)
			{
				normalSkillCounter = 0f;
				normalSkillCount.RandomResult();
				if (lastMegaAttackType == 0)
				{
					state = MonsterState.MegaMissilePrepare;
					megaAttackDampSpeed = base.CurrentMotion;
					lastMegaAttackType = 1;
				}
				else
				{
					state = MonsterState.MegaDashPrepare;
					megaAttackDampSpeed = base.CurrentMotion;
					lastMegaAttackType = 0;
				}
				attackCDTimer = 0f;
				attackCD.RandomResult();
			}
			return;
		}
		bool flag = false;
		while (!flag)
		{
			int weightRandom = GeneralTool.GetWeightRandom(followMissileChance, aroundMissileChance, shootBulletChance);
			if (lastAttackType != weightRandom)
			{
				switch (weightRandom)
				{
				case 0:
					state = MonsterState.MissileCharge;
					break;
				case 1:
					state = MonsterState.FollowMissileCharge;
					break;
				case 2:
					state = MonsterState.ShootBulletPrepare;
					break;
				}
				flag = true;
			}
			if (flag)
			{
				lastAttackType = weightRandom;
			}
		}
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	private void LateUpdate()
	{
		MeshRenderer[] array = meshRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material.SetColor("_BaseColor", myPpt.BaseColor);
		}
	}

	private void UpdateModel()
	{
		shadow.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		Boss13.camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		modelRotateRoot.localEulerAngles = new Vector3(0f, (0f - Mathf.Atan2(lookDir.y, lookDir.x)) * 57.29578f - 90f, 0f);
		shadowRotateRoot.localEulerAngles = modelRotateRoot.localEulerAngles;
		shadowTsfRoot.position = Tool2D.IgnoreZPoint(base.transform.position, -2079.7f);
		MonsterState monsterState = state;
		if (monsterState == MonsterState.Idle || monsterState == MonsterState.RandomMove || monsterState == MonsterState.Move)
		{
			modelFloatRatio += Time.deltaTime;
		}
		else
		{
			modelFloatRatio -= Time.deltaTime;
		}
		modelFloatRatio = Mathf.Clamp01(modelFloatRatio);
		modelFloatPhase += Time.deltaTime;
		float y = modelFloatBaseHeight + modelFloatRatio * modelFloatAmplitude * Mathf.Sin(modelFloatFrequency * modelFloatPhase * MathF.PI * 2f);
		modelFloatRoot.transform.localPosition = new Vector3(0f, y, 0f);
		float num = Vector3.Dot(base.CurrentMotion, lookDir);
		if (state == MonsterState.MegaMissilePrepare)
		{
			num = Vector3.Dot(megaAttackDampSpeed, lookDir);
		}
		else
		{
			monsterState = state;
			if (monsterState == MonsterState.MegaDashPrepareFly || monsterState == MonsterState.MegaDash || monsterState == MonsterState.MegaDashAfter)
			{
				num = 0f;
			}
		}
		float x = 0f - Mathf.LerpUnclamped(0f, modelMaxTiltAngle, num / modelMaxTiltSpeed) + prepareExtraTiltFix * shootTiltAmplitude + dashExtraTilt;
		if (!base.deadStayed)
		{
			modelTiltRoot.localEulerAngles = new Vector3(x, 0f, 0f);
			shadowTiltRoot.localEulerAngles = new Vector3(x, 0f, 0f);
		}
		else
		{
			modelTiltRoot.localEulerAngles = Vector3.zero;
			shadowTiltRoot.localEulerAngles = Vector3.zero;
		}
		nowShakePhase += Time.deltaTime * shakeFrequency;
		float x2 = Mathf.PerlinNoise(berlinSeedX.x * nowShakePhase, berlinSeedX.y * nowShakePhase) - 0.5f;
		float z = Mathf.PerlinNoise(berlinSeedY.x * nowShakePhase, berlinSeedY.y * nowShakePhase) - 0.5f;
		shootExtraShake = Mathf.Lerp(shootExtraShake, 0f, Time.deltaTime * shootShakeFadeSpeed);
		Vector3 vector = new Vector3(x2, 0f, z) * 2f * (shakeAmplitude * prepareExtraShakeFix + shootExtraShake);
		if (!base.deadStayed)
		{
			modelShakeRoot.localPosition = vector + Vector3.up * (prepareExtraHeightFix * maxPrepareHeight + dashExtraHeight);
			shadowShakeRoot.localPosition = vector;
		}
		else
		{
			Vector3 localPosition = new Vector3(x2, 0f, z) * 2f * deadShake;
			modelShakeRoot.localPosition = localPosition;
			shadowShakeRoot.localPosition = localPosition;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		UpdateModel();
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
				CameraFocusSizeData data = new CameraFocusSizeData(1f, 1, 1000000f);
				CamController.Inst.AddNewCameraFocusRequirement(data);
			}
			if (stateExistTime > 0.5f)
			{
				dashDamageZone.canCollideEnvironment = true;
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Move;
			}
			ChooseSkill();
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				randomMoveTime.RandomResult();
				if (base.HaveTarget)
				{
					randomMoveTarget = base.TargetPoint + Tool2D.GetDir(-ToPointDir(base.TargetPoint), UnityEngine.Random.Range(-30f, 30f)) * randomMoveKeepDistance.RandomResult();
				}
				else
				{
					randomMoveTarget = Tool2D.GetNavMeshPointIngoreZ(GetInvisiblePoint(0.4f));
				}
			}
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, ToPointDir(base.TargetPoint), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			else
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, ToPointDir(randomMoveTarget), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
			}
			GetNavInfo(randomMoveTarget);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
			if (base.HaveTarget && ToTargetDistance() < randomMoveKeepDistance.value1)
			{
				randomMoveTarget = base.TargetPoint - ToPointDir(base.TargetPoint) * randomMoveKeepDistance.RandomResult();
			}
			if (navInfo.allCornerArrived)
			{
				if (base.HaveTarget)
				{
					randomMoveTarget = base.TargetPoint + Tool2D.GetDir(-ToPointDir(base.TargetPoint), UnityEngine.Random.Range(-20f, 20f)) * randomMoveKeepDistance.RandomResult();
				}
				else
				{
					randomMoveTarget = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomMoveRadius);
				}
				GetNavInfo(randomMoveTarget);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget && Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > randomMoveKeepDistance.value2 * randomMoveKeepDistance.value2)
				{
					state = MonsterState.Move;
				}
			}
			ChooseSkill();
			break;
		case MonsterState.Move:
			if (changedState)
			{
				randomMoveKeepDistance.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			ChooseSkill();
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > randomMoveKeepDistance.result * randomMoveKeepDistance.result)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.ShootBulletPrepare:
		{
			ref Vector3 reference24 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.boss13Stage3AttackWarning.PlaySE();
				fakeTarget = GetInvisiblePoint(0.2f);
				reference24 = ToPointDir(fakeTarget);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			prepareExtraShakeFix = PrepareShakeCurve.Evaluate(stateExistTime / bulletPrepareTime);
			if (!base.HaveTarget)
			{
				GetNavInfo(base.transform.position + reference24);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * shootBulletSpeedRatio);
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, reference24, turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * shootBulletSpeedRatio);
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, ToTargetDir(), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			if (stateExistTime > bulletPrepareTime)
			{
				state = MonsterState.ShootBullet;
			}
			break;
		}
		case MonsterState.ShootBullet:
		{
			ref float reference6 = ref varMgr.RegFloat(0);
			ref int reference7 = ref varMgr.RegInt(0);
			ref bool reference8 = ref varMgr.RegBool(0);
			if (changedState)
			{
				reference8 = GeneralTool.ChanceResult(0.5f);
				reference6 = bulletShootInterval;
			}
			prepareExtraShakeFix = PrepareAfterShakeCurve.Evaluate(stateExistTime);
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNavInfo(base.transform.position + lookDir);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * shootBulletSpeedRatio);
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * shootBulletSpeedRatio);
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, ToTargetDir(), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			reference6 += Time.deltaTime;
			if (reference6 > bulletShootInterval)
			{
				if ((float)reference7 >= bulletRounds)
				{
					state = MonsterState.Move;
					break;
				}
				reference7++;
				reference6 -= bulletShootInterval;
				ShootBullet(reference8);
				reference8 = !reference8;
			}
			break;
		}
		case MonsterState.MissileCharge:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage3AttackWarning.PlaySE();
			}
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			SetMove(Vector3.zero);
			prepareExtraShakeFix = PrepareShakeCurve.Evaluate(stateExistTime / delayMissileChargeTime);
			prepareExtraHeightFix = prepareHeightCurve.Evaluate(stateExistTime / delayMissileChargeTime);
			if (stateExistTime > delayMissileChargeTime)
			{
				state = MonsterState.ShootMissile;
			}
			break;
		case MonsterState.ShootMissile:
		{
			ref bool reference9 = ref varMgr.RegBool(0);
			_ = changedState;
			prepareExtraShakeFix = PrepareAfterShakeCurve.Evaluate(stateExistTime);
			prepareExtraHeightFix = prepareAfterHeightCurve.Evaluate(stateExistTime / heightRecoverTime);
			SetMove(Vector3.zero);
			if (stateExistTime > delayMissileShootTime && !reference9)
			{
				CamController.Inst.SetShock(shootShock);
				reference9 = true;
				float num = UnityEngine.Random.Range(0, 360);
				shootExtraShake = shootShakeAmplitudeBig;
				for (int j = 0; (float)j < delayMissileCount; j++)
				{
					SEMgr.Inst.boss13Stage3DelayMissileShoot.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Stage3MissileDelay", base.transform.position).GetComponent<Boss13Stage3Missile>().moveDir = Tool2D.GetDir(num + 360f / delayMissileCount * (float)j + UnityEngine.Random.Range(0f - delayMissileAngleOffset, delayMissileAngleOffset));
				}
			}
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			SetMove(Vector3.zero);
			if (stateExistTime > delayMissileTime)
			{
				state = MonsterState.Move;
			}
			break;
		}
		case MonsterState.FollowMissileCharge:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage3AttackWarning.PlaySE();
			}
			prepareExtraShakeFix = PrepareShakeCurve.Evaluate(stateExistTime / delayMissileChargeTime);
			prepareExtraHeightFix = prepareHeightCurve.Evaluate(stateExistTime / delayMissileChargeTime);
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			SetMove(Vector3.zero);
			if (stateExistTime > followMissileChargeTime)
			{
				state = MonsterState.FollowMissileFire;
			}
			break;
		case MonsterState.FollowMissileFire:
		{
			ref float reference25 = ref varMgr.RegFloat(0);
			ref float reference26 = ref varMgr.RegFloat(1);
			ref float reference27 = ref varMgr.RegFloat(2);
			ref bool reference28 = ref varMgr.RegBool(0);
			ref Vector3 reference29 = ref varMgr.RegV3(0);
			if (changedState)
			{
				reference29 = lookDir;
				reference26 = 0f;
				reference28 = GeneralTool.ChanceResult(0.5f);
				reference27 = GeneralTool.HalfChanceNPOne();
			}
			prepareExtraShakeFix = PrepareAfterShakeCurve.Evaluate(stateExistTime);
			prepareExtraHeightFix = prepareAfterHeightCurve.Evaluate(stateExistTime / heightRecoverTime);
			prepareExtraTiltFix = prepareAfterTiltCurve.Evaluate(stateExistTime / heightRecoverTime);
			SetMove(Vector3.zero);
			reference26 += bodyRotateSpeed * Time.deltaTime;
			reference25 += Time.deltaTime;
			lookDir = Tool2D.GetDir(reference29, reference26 * reference27);
			if (reference26 > 360f)
			{
				state = MonsterState.Move;
			}
			else if (reference25 > followMissileShootInterval)
			{
				Vector3 shootPoint2 = GetShootPoint(reference28);
				PlayShootParticle(reference28);
				CamController.Inst.SetShock(shootShock);
				SEMgr.Inst.boss13Stage3FollowMissileShoot.PlaySE();
				Boss13Stage3FollowMissile component3 = ObjPoolMgr.Inst.GetGO("Prefabs/Units/501351", shootPoint2).GetComponent<Boss13Stage3FollowMissile>();
				component3.onLand = true;
				component3.moveDir = lookDir;
				Boss13Stage3FollowMissile.followMissiles.Add(component3);
				reference28 = !reference28;
				reference25 = 0f;
			}
			break;
		}
		case MonsterState.MegaMissilePrepare:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage3AttackWarningBig.PlaySE();
				megaAttackDampSpeed = base.CurrentMotion;
			}
			prepareExtraHeightFix = megaAttackPrepareHeightCurve.Evaluate(stateExistTime / megaAttackPrepareTime);
			prepareExtraShakeFix = megaAttackPrepareShakeCurve.Evaluate(stateExistTime / megaAttackPrepareTime);
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			base.transform.position = Vector3.SmoothDamp(base.transform.position, roomCenter, ref megaAttackDampSpeed, megaAttackDampTime);
			SyncDotsPosition();
			if (stateExistTime > megaAttackPrepareTime)
			{
				state = MonsterState.MegaMissileAttack;
			}
			break;
		case MonsterState.MegaMissileAttack:
		{
			ref float reference20 = ref varMgr.RegFloat(0);
			ref float reference21 = ref varMgr.RegFloat(1);
			ref bool reference22 = ref varMgr.RegBool(0);
			ref Vector3 reference23 = ref varMgr.RegV3(0);
			if (changedState)
			{
				reference23 = lookDir;
				reference21 = 0f;
				reference22 = GeneralTool.ChanceResult(0.5f);
			}
			prepareExtraHeightFix = megaAttackPrepareAfterHeightCurve.Evaluate(stateExistTime / heightRecoverTime);
			prepareExtraShakeFix = megaAttackPrepareAfterShakeCurve.Evaluate(stateExistTime);
			prepareExtraTiltFix = megaAttackPrepareAfterTiltCurve.Evaluate(stateExistTime / heightRecoverTime);
			SetMove(Vector3.zero);
			if (stateExistTime > megaMissileAttackTime)
			{
				state = MonsterState.MegaSkillsAfter;
				break;
			}
			reference20 += Time.deltaTime;
			if (reference20 > megaMissileShootInterval)
			{
				reference20 = 0f;
				PlayShootParticle(reference22);
				Vector3 shootPoint = GetShootPoint(reference22);
				SEMgr.Inst.boss13Stage2Shoot.PlaySE();
				Boss13Stage3Missile component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Stage3Missile", shootPoint).GetComponent<Boss13Stage3Missile>();
				reference22 = !reference22;
				component2.moveDir = lookDir;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			else
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.GetDir(lookDir, 90f), turnSpeed * Time.deltaTime * 0.2f, turnSmoothAngle);
			}
			break;
		}
		case MonsterState.MegaSkillsAfter:
			SetMove(Vector3.zero);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), turnSpeed * Time.deltaTime, turnSmoothAngle);
			}
			if (stateExistTime > megaSkillExtraRestTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.MegaDashPrepare:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage3AttackWarningBig.PlaySE();
				dashPrepareParticle.Play();
				dashDamageZone.canCollideEnvironment = false;
				dashDamageZone.damageCheck = false;
			}
			SetMove(Vector3.zero);
			lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Vector3.left, turnSpeed * Time.deltaTime, turnSmoothAngle);
			dashExtraHeight = dashPrepareHeight * dashPrepareHeightCurve.Evaluate(stateExistTime / dashPrepareTime);
			prepareExtraShakeFix = megaAttackPrepareShakeCurve.Evaluate(stateExistTime / dashPrepareTime);
			if (stateExistTime > dashPrepareTime)
			{
				state = MonsterState.MegaDashPrepareFly;
			}
			break;
		case MonsterState.MegaDashPrepareFly:
		{
			ref Vector3 reference10 = ref varMgr.RegV3(0);
			ref bool reference11 = ref varMgr.RegBool(0);
			if (changedState)
			{
				reference10 = base.transform.position;
				dashPrepareParticle.Stop();
				dashContinuedParticle.Play();
				SEMgr.Inst.boss13DashBig.PlaySE();
				prepareExtraShakeFix = 0f;
			}
			base.transform.position = reference10 + Vector3.left * dashPrepareDistance * dashPrepareDistanceCurve.Evaluate(stateExistTime / dashPrepareTime);
			SyncDotsPosition();
			dashExtraHeight = dashPrepareHeight + dashPrepareFlyHeight * dashPrepareHeightCurve.Evaluate(stateExistTime / dashPrepareTime);
			lookDir = Tool2D.RotateTowardsAroundZAxisSmooth(lookDir, Vector3.left, turnSpeed * Time.deltaTime, turnSmoothAngle);
			SetMove(Vector3.zero);
			dashExtraTilt = dashPrepareAngle * dashPrepareAngleCurve.Evaluate(stateExistTime / dashPrepareTime);
			if (stateExistTime > dashPrepareTime * 0.3f && !reference11)
			{
				reference11 = true;
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				SetComponentData(componentData2);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: true);
			}
			if (stateExistTime > dashPrepareTime)
			{
				state = MonsterState.MegaDash;
				dashTimesCounter = 0f;
				dashDirRotateRight = GeneralTool.HalfChanceNPOne();
			}
			break;
		}
		case MonsterState.MegaDash:
		{
			ref Vector3 reference12 = ref varMgr.RegV3(0);
			ref Vector3 reference13 = ref varMgr.RegV3(1);
			ref float reference14 = ref varMgr.RegFloat(0);
			ref float reference15 = ref varMgr.RegFloat(1);
			ref float reference16 = ref varMgr.RegFloat(2);
			ref float reference17 = ref varMgr.RegFloat(3);
			ref float reference18 = ref varMgr.RegFloat(4);
			ref bool reference19 = ref varMgr.RegBool(0);
			if (changedState)
			{
				dashBubbleParticle.Play();
				reference19 = false;
				dashContinuedParticle.Play();
				dashExtraTilt = -10f;
				if (dashTimesCounter == 0f)
				{
					dashDir = Tool2D.GetDir(90 * UnityEngine.Random.Range(0, 3));
				}
				else
				{
					dashDir = Tool2D.GetDir(dashDir, 90f * dashDirRotateRight);
				}
				lookDir = dashDir;
				dashTimesCounter += 1f;
				base.CurrentMotion = Vector3.zero;
				Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * dashPredictTime + Tool2D.GetDir(dashDir, 90f) * dashRandomRadius * UnityEngine.Random.value);
				if (Mathf.Abs(dashDir.x) > 0.1f)
				{
					reference12 = new Vector3(roomCenter.x, navMeshPointIngoreZ.y, 0f) - dashWholeDistance * 0.4f * dashDir;
					reference13 = new Vector3(roomCenter.x, navMeshPointIngoreZ.y, 0f) + dashWholeDistance * 0.4f * dashDir;
				}
				else
				{
					reference12 = new Vector3(roomCenter.x, navMeshPointIngoreZ.y, 0f) - dashWholeDistance * 0.4f * dashDir;
					reference13 = new Vector3(roomCenter.x, navMeshPointIngoreZ.y, 0f) + dashWholeDistance * 0.4f * dashDir;
				}
				if (UnitDotsSyncSystem.Raycast(navMeshPointIngoreZ, dashDir, 999f, GameConst.Filter_Wall, out var result))
				{
					reference13 = Tool2D.IgnoreZPoint(result.point);
				}
				if (UnitDotsSyncSystem.Raycast(navMeshPointIngoreZ, -dashDir, 999f, GameConst.Filter_Wall, out var result2))
				{
					reference12 = Tool2D.IgnoreZPoint(result2.point);
				}
				reference17 = (dashWholeDistance - Tool2D.IgnoreZDistance(reference12, reference13)) / 2f;
				base.transform.position = reference12 - dashDir * reference17;
				SyncDotsPosition();
				reference14 = reference17 / dashSpeed;
				reference15 = Tool2D.IgnoreZDistance(reference12, reference13) / dashSpeed;
				for (int k = 0; k < dashWarningLine.positionCount; k++)
				{
					Vector3 rootPoint = Vector3.Lerp(reference12, reference13, (float)k / (float)(dashWarningLine.positionCount - 1));
					rootPoint = Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect);
					dashWarningLine.SetPosition(k, rootPoint);
				}
				dashWarningLine.gameObject.SetActive(value: true);
				stateExistTime -= dashDelayTime;
				dashDamageZone.canCollideEnvironment = true;
				dashDamageZone.damageCheck = true;
				dashDamageZone.hitEntities.Clear();
				dashDamageZone.dashDir = dashDir;
			}
			if (stateExistTime < 0f)
			{
				break;
			}
			if (stateExistTime > soundDelayTime && !reference19)
			{
				reference19 = true;
				SEMgr.Inst.boss13DashBig.PlaySE().pitch = UnityEngine.Random.Range(0.8f, 1.2f);
			}
			base.transform.position += dashDir * dashSpeed * Time.deltaTime;
			SyncDotsPosition();
			if (stateExistTime < reference14)
			{
				dashExtraHeight = dashInOutHeight * dashInHeightCurve.Evaluate(stateExistTime / reference14);
			}
			else if (stateExistTime > reference14 + reference15)
			{
				dashExtraHeight = dashInOutHeight * dashOutHeightCurve.Evaluate((stateExistTime - reference14 - reference15) / reference14);
			}
			else
			{
				reference18 += Time.deltaTime;
				if (reference18 > megaDashShock.time * 0.9f)
				{
					reference18 = 0f;
					CamController.Inst.SetShock(megaDashShock);
				}
				dashExtraHeight = 0f;
				reference16 += Time.deltaTime;
				while (reference16 > dashBulletSpawnInterval)
				{
					reference16 -= dashBulletSpawnInterval;
					float num2 = GeneralTool.HalfChanceNPOne();
					Vector3 dir = Tool2D.GetDir(dashDir, 90f * num2);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13DashBulletStage3", base.transform.position + UnityEngine.Random.Range(0f, 0.1f) * dir).GetComponent<Boss13DashBullet>().moveDir = Tool2D.GetDir(dir, (float)UnityEngine.Random.Range(0, 45) * num2);
				}
			}
			if (stateExistTime > reference15 + reference14 * 2f)
			{
				if (dashTimesCounter >= dashTimes)
				{
					state = MonsterState.MegaDashAfter;
					dashContinuedParticle.Stop();
				}
				else
				{
					state = MonsterState.MegaDash;
				}
			}
			break;
		}
		case MonsterState.MegaDashAfter:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref bool reference2 = ref varMgr.RegBool(0);
			if (changedState)
			{
				reference = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
				lookDir = Vector3.left;
				SEMgr.Inst.boss13DashBig.PlaySE();
				dashWarningLine.gameObject.SetActive(value: false);
				dashDamageZone.damageCheck = false;
			}
			if (stateExistTime > dashAfterTime * 0.7f && !reference2)
			{
				reference2 = true;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				SetComponentData(componentData);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
			}
			base.transform.position = reference + Vector3.right * dashPrepareDistance * dashAfterDistanceCurve.Evaluate(stateExistTime / dashAfterTime);
			SyncDotsPosition();
			dashExtraHeight = dashPrepareFlyHeight * dashAfterHeightCurve.Evaluate(stateExistTime / dashAfterTime);
			dashExtraTilt = dashPrepareAngle * dashAfterAngleCurve.Evaluate(stateExistTime / dashAfterTime);
			if (stateExistTime > dashAfterTime)
			{
				state = MonsterState.MegaSkillsAfter;
			}
			break;
		}
		case MonsterState.Dead:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			ref bool reference4 = ref varMgr.RegBool(0);
			ref bool reference5 = ref varMgr.RegBool(1);
			if (changedState)
			{
				for (int i = 0; i < meshRenderers.Length; i++)
				{
					meshRenderers[i].material.SetTexture(GameConstManaged.baseMapIndex, damagedTexture);
				}
				base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
				SyncDotsPosition();
			}
			if (stateExistTime < deadExplosionTime)
			{
				reference3 += Time.deltaTime;
				if (reference3 > deadExplosionInterval)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10Explosion 1", Tool2D.GetLayerPoint(base.transform.position + Tool2D.GetDir() * UnityEngine.Random.value * deadExplosionRadius + Vector3.back * UnityEngine.Random.value * deadExplosionRadius), Quaternion.identity, Vector3.one * 1.5f, 4f);
					CamController.Inst.SetShock(deadExplosionShock);
					reference3 = 0f;
					SEMgr.Inst.boss13BigExplosion.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
				}
			}
			else if (!reference4)
			{
				CamController.Inst.SetShock(deadExplosionShock);
				SEMgr.Inst.boss13BigExplosion.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13ExplosionLarge", Tool2D.GetLayerPoint(base.transform.position) + Vector3.back * 0.04f, Quaternion.identity, Vector3.one * 1.5f, 4f);
				reference4 = true;
				deadShake = 0f;
			}
			if (stateExistTime > deadAllTime && !reference5)
			{
				reference5 = true;
				if (controller.skipDaveDialogue)
				{
					state = MonsterState.DeadAnimation;
				}
				else if (DataMgr.selectedWorldData.IsDave)
				{
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(310, (Action)delegate
					{
						state = MonsterState.DeadAnimation;
					});
				}
				else
				{
					state = MonsterState.DeadAnimation;
				}
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.DeadAnimation:
			if (changedState)
			{
				Boss13FakeSub component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FakeSub", base.transform.position).GetComponent<Boss13FakeSub>();
				component.lookDir = lookDir;
				component.SetMode(3);
				DotsAnnouncedDeath();
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		CamController.Inst.ClearExtraCameraFocusRequirement();
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		info.stopAnnouncedDeath = true;
	}

	protected override void BossDeadStay()
	{
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
		SyncDotsPosition();
		dashWarningLine.gameObject.SetActive(value: false);
		state = MonsterState.Dead;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		dashContinuedParticle.Stop();
		dashPrepareParticle.Stop();
		dashDamageZone.damageCheck = false;
		dashDamageZone.canCollideEnvironment = false;
		foreach (Boss13Stage3FollowMissile followMissile in Boss13Stage3FollowMissile.followMissiles)
		{
			followMissile.DotsAnnouncedDeath();
		}
	}

	private Vector3 GetShootPoint(bool fromLeft)
	{
		return base.transform.position + Tool2D.GetDir(lookDir.normalized, (float)(fromLeft ? 1 : (-1)) * shootPointAngle) * shootPointMagnitude;
	}

	private void PlayShootParticle(bool fromLeft)
	{
		if (fromLeft)
		{
			leftShootParticle.Play();
		}
		else
		{
			rightShootParticle.Play();
		}
		shootExtraShake = shootShakeAmplitude;
		if (state == MonsterState.ShootBullet)
		{
			shootExtraShake = shootShakeAmplitudeBig;
		}
	}

	private void ShootBullet(bool fromLeft)
	{
		SEMgr.Inst.boss13Stage1Shoot.PlaySE();
		Vector3 shootPoint = GetShootPoint(fromLeft);
		PlayShootParticle(fromLeft);
		CamController.Inst.SetShock(shootShock);
		for (int i = 0; i < bulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13_Bullet", shootPoint).GetComponent<Boss13Bullet>().InitializeSimple(Tool2D.GetDir(lookDir, -0.5f * bulletAngle + bulletAngle / (float)(bulletCount - 1) * (float)i), bulletSpeed);
		}
	}
}
