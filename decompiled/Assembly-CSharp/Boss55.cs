using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss55 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		RandomMove,
		SwordAttackFollow,
		CrossAttackPrepare,
		CrossAttack,
		CrossAttackAfter,
		SpinAttackPrepare,
		SpinAttack,
		SpinAttackAfter,
		SwordAttackPrepare,
		SwordAttack,
		SwordAttackAfter,
		SmallLaser,
		RotateSmallLaser,
		SpinDashAim,
		SpinDash,
		SpinDashAgain,
		SpinDashAfter,
		RotateLaserPrepare,
		RotateLaser,
		RotateLaserAfter,
		SideSlash,
		SwordFreeAttack,
		CurveSwordAttack,
		SwitchStage,
		RotateSlashPrepare,
		RotateSlash,
		RotateSlashAfter,
		Dead
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("行动")]
	public VariableFloat idleTime;

	public VariableFloat keepDistanceRange;

	public VariableFloat keepDistanceAngleRange;

	public VariableFloat keepDistanceTime;

	public VariableFloat attackCD;

	private float attackCDTimer;

	private float moveTimer;

	[Header("技能")]
	public float crossAttackChance;

	public float spinAttackChance;

	public float smallLaserChance;

	public float swordAttackChance;

	public float freeAttackChance;

	public float spinSlashChance;

	public float sideSlashChance;

	public float freeLaserChance;

	public float curveSlashChance;

	public float rotateLaserChance;

	public float rotateSlashChance;

	public bool isTestingSkill;

	public VariableInt bigSkillInterval;

	private int bigSkillIntervalCounter;

	public MonsterState testSkill;

	private int lastLastSkillIndex;

	private int lastSkillIndex;

	private int lastBigSkillIndex;

	[Header("二阶段")]
	public ParticleSystem switchStageParticle;

	public ParticleSystem switchStageFinishParticle;

	public bool secondStageOverride;

	public int firstStageHalfSwordCount;

	public int secondStageHalfSwordCount;

	[Header("表现")]
	public Transform tsf_Model;

	public ParticleSystem shootBulletParticle;

	[Header("子弹表现")]
	public float bulletTrailInterval;

	public List<Boss55_Bullet> bullets = new List<Boss55_Bullet>();

	[Header("飞剑")]
	public List<Transform> swordPoints = new List<Transform>();

	public List<Boss55_Sword> swords = new List<Boss55_Sword>();

	public static Boss55 Inst;

	[Header("交叉射击")]
	public float crossAttackInterval;

	public float crossAttackDuration;

	public float firstStageCrossAttackAngle;

	public float secondStageCrossAttackAngle;

	public float crossHorizontalOffset;

	public float crossAttackSpeedRatio;

	[Header("环绕射击")]
	public float spinAttackInterval;

	public float spinAttackDuration;

	public float firstStageRotateSpeed;

	public float secondStageRotateSpeed;

	public float spinMoveAngle;

	public float spinSwordDistance;

	private float spinAngle;

	[Header("传送剑刺")]
	public float swordLaunchInterval;

	public float swordAttackCount;

	public float swordAttackDuration;

	public VariableFloat swordLaunchDistance;

	public float swordPredictTime;

	public float swordAttackMinAngle;

	[Header("小激光")]
	public float smallLaserPredictTime;

	public VariableFloat smallLaserPosOffset;

	public VariableFloat smallLaserAimOffset;

	public VariableFloat smallLaserPosOffset2;

	public VariableFloat smallLaserAimOffset2;

	[Header("环绕激光")]
	public VariableFloat rotateLaserRepositionDistance;

	public VariableFloat rotateLaserChangeDirTime;

	public float rotateLaserMoveSpeedRatio;

	public int firstStageRotateLaserHalfCount;

	public int secondStageRotateLaserHalfCount;

	public int firstStageLaserRotateSpeed;

	public int secondStageLaserRotateSpeed;

	public float laserBulletMinAngleInterval;

	public float laserDuration;

	public float laserBulletShootInterval;

	public float laserBulletSpeed;

	public float laserCircleBulletCount;

	public float laserCircleBulletSpeed;

	public float laserCircleBulletInterval;

	public float rotateLaserFromBorderDistance;

	public float smoothLerpTime;

	private Vector3 smoothLerpSpeed;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	[Header("自由攻击")]
	public float freeAttackTime;

	public float freeAttackInterval;

	[Header("旋转剑刃斩")]
	public float firstStageSpinBladeRotateSpeed;

	public float secondStageSpinBladeRotateSpeed;

	public LineRenderer spinDashWarningLine;

	public float spinDashBulletShootAngleInterval;

	public float spinDashBulletShootInverval;

	public float spinDashAfterBulletShootInterval;

	public float spinDashBulletSpeed;

	public float spinDashBulletDistance;

	public float spinDashBulletStartShootSpeed;

	private float spinDashRotateClockwise;

	public Boss55_Spin spin;

	public float spinDashAccleration;

	public float spinDashMaxSpeed;

	public float spinDashMinTime;

	public float spinDashDirRotateSpeed;

	public int firstStageSpinDashCount;

	public int secondStageSpinDashCount;

	private int spinDashCounter;

	private bool spinBladeSpining;

	private Vector3 spinBladeDir;

	[Header("曲线斩")]
	public float curveMiddlePointOffset;

	public float firstStageCurveEndPointOffsetRange;

	public float secondStageCurveEndPointOffsetRange;

	public float curveStandardDistance;

	public float curveStandardTime;

	public float curveMaxExtraTime;

	public float curveMaxExtraDistance;

	private bool curveSlashLeft;

	private bool curveSlashFlip;

	[Header("侧向斩")]
	public float sideSlashDistanceInterval;

	public float sideSlashStartAngle;

	[Header("旋转剑阵")]
	public float rotateSlashMoveSpeedRatio;

	public float rotateSlashRotateSpeed;

	public float rotateSlashShootInterval;

	public float rotateSlashShootCount;

	public VariableFloat rotateSlashBulletSpeed;

	public float rotateSlashDuration;

	public float rotateSlashDistanceInterval;

	private Vector3 rotateSlashBaseDir;

	private float rotateSlashDir;

	private const int rotateSlashGroupCount = 2;

	private const int rotateSlashGroupSwordCount = 6;

	[Header("音效")]
	public AudioSource AS_LaserLoop;

	private List<int> swordIndexPool = new List<int>();

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.CrossAttackPrepare,
		MonsterState.SpinAttackPrepare,
		MonsterState.RotateSmallLaser,
		MonsterState.CurveSwordAttack,
		MonsterState.SpinDashAim,
		MonsterState.SideSlash
	};

	private List<MonsterState> bigSkills = new List<MonsterState>
	{
		MonsterState.RotateLaserPrepare,
		MonsterState.SwordAttackPrepare,
		MonsterState.RotateSlashPrepare
	};

	private List<GlobalParticleEmitParams> bulletTrailParam = new List<GlobalParticleEmitParams>();

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			moveTimer = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public bool canInSecondStage
	{
		get
		{
			UnitConfig unitCfg = GetComponentData<UnitProperty_Dots>(myPpt.myEntity).unitCfg;
			if (!inSecondStage)
			{
				if (!secondStageOverride)
				{
					return unitCfg.currentHP / unitCfg.maxHP < 0.5f;
				}
				return true;
			}
			return false;
		}
	}

	public bool inSecondStage { get; private set; }

	private int nowStageHalfSwordCount
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageHalfSwordCount;
			}
			return secondStageHalfSwordCount;
		}
	}

	private bool swordsAllReady
	{
		get
		{
			for (int i = 0; i < swords.Count; i++)
			{
				if (i % secondStageHalfSwordCount < nowStageHalfSwordCount && swords[i].state != 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	public float crossAttackAngle
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageCrossAttackAngle;
			}
			return secondStageCrossAttackAngle;
		}
	}

	public float spinRotateSpeed
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageRotateSpeed;
			}
			return secondStageRotateSpeed;
		}
	}

	public int rotateLaserHalfCount
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageRotateLaserHalfCount;
			}
			return secondStageRotateLaserHalfCount;
		}
	}

	public float laserRotateSpeed => inSecondStage ? secondStageLaserRotateSpeed : firstStageLaserRotateSpeed;

	public float spinBladeRotateSpeed
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageSpinBladeRotateSpeed;
			}
			return secondStageSpinBladeRotateSpeed;
		}
	}

	public int spinDashCount
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageSpinDashCount;
			}
			return secondStageSpinDashCount;
		}
	}

	public float curveEndPointOffsetRange
	{
		get
		{
			if (!inSecondStage)
			{
				return firstStageCurveEndPointOffsetRange;
			}
			return secondStageCurveEndPointOffsetRange;
		}
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
		AS_LaserLoop.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		spinDashWarningLine.positionCount = 10;
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		swords.Clear();
		for (int i = 0; i < swordPoints.Count; i++)
		{
			Boss55_Sword component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Sword", base.transform.position).GetComponent<Boss55_Sword>();
			swords.Add(component);
			component.Initialize(swordPoints[i]);
			if (i % secondStageHalfSwordCount >= nowStageHalfSwordCount)
			{
				component.state = Boss55_Sword.SwordState.Hide;
			}
		}
		bigSkillIntervalCounter = 0;
		lastLastSkillIndex = -1;
		lastSkillIndex = -1;
		lastBigSkillIndex = -1;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		spinDashWarningLine.enabled = false;
	}

	protected override void SetFlip(float motionX)
	{
		if (Mathf.Abs(motionX) > 0.01f)
		{
			tsf_Model.localScale = new Vector3(Mathf.Sign(motionX), 1f, 1f);
			myPpt.isFlipX = Mathf.Sign(motionX) < 0f;
			for (int i = 0; i < swords.Count; i++)
			{
				swords[i].SetFlip(myPpt.isFlipX);
			}
		}
	}

	public int GetRandomFreeSwordIndex()
	{
		swordIndexPool.Clear();
		int result = -1;
		for (int i = 0; i < swords.Count; i++)
		{
			if (swords[i].state == Boss55_Sword.SwordState.Idle)
			{
				swordIndexPool.Add(i);
			}
		}
		if (swordIndexPool.Count > 0)
		{
			result = swordIndexPool.GetRandom();
		}
		else
		{
			for (int j = 0; j < swords.Count; j++)
			{
				if (swords[j].state == Boss55_Sword.SwordState.DashAfter)
				{
					swordIndexPool.Add(j);
				}
			}
		}
		if (swordIndexPool.Count > 0)
		{
			return swordIndexPool.GetRandom();
		}
		return result;
	}

	private void TryAttack()
	{
		if (!swordsAllReady)
		{
			return;
		}
		attackCDTimer += Time.deltaTime;
		if (bigSkillInterval.result == 0)
		{
			bigSkillInterval.RandomResult();
		}
		if (!(attackCDTimer > attackCD.result))
		{
			return;
		}
		attackCDTimer = 0f;
		attackCD.RandomResult();
		if (canInSecondStage && !inSecondStage)
		{
			inSecondStage = true;
			state = MonsterState.SwitchStage;
			return;
		}
		bigSkillIntervalCounter++;
		if (isTestingSkill)
		{
			state = testSkill;
			return;
		}
		if (bigSkillIntervalCounter > bigSkillInterval.result)
		{
			bigSkillIntervalCounter = 0;
			bigSkillInterval.RandomResult();
			int num;
			for (num = lastBigSkillIndex; num == lastBigSkillIndex; num = ((!inSecondStage) ? GeneralTool.GetWeightRandom(rotateLaserChance, swordAttackChance, 0f) : GeneralTool.GetWeightRandom(rotateLaserChance, swordAttackChance, rotateSlashChance)))
			{
			}
			MonsterState monsterState2 = (state = bigSkills[num]);
			lastBigSkillIndex = num;
			return;
		}
		int num2 = lastSkillIndex;
		while (num2 == lastSkillIndex || num2 == lastLastSkillIndex)
		{
			num2 = ((!inSecondStage) ? GeneralTool.GetWeightRandom(crossAttackChance, spinAttackChance, smallLaserChance, curveSlashChance, 0f, 0f) : GeneralTool.GetWeightRandom(crossAttackChance, spinAttackChance, smallLaserChance, curveSlashChance, spinSlashChance, sideSlashChance));
		}
		MonsterState monsterState4 = (state = skills[num2]);
		lastLastSkillIndex = lastSkillIndex;
		lastSkillIndex = num2;
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
				SetFlip(1f);
			}
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistanceRange, ToTargetDir(), keepDistanceAngleRange.RandomResult() * GeneralTool.HalfChanceNPOne()));
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			KeepDistanceMove();
			TryAttack();
			break;
		case MonsterState.RandomMove:
			if (changedState && navInfo == null)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistanceRange));
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistanceRange));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			TryAttack();
			break;
		case MonsterState.CrossAttackPrepare:
			if (changedState)
			{
				SEMgr.Inst.boss55SendSword.PlaySE();
				base.Anima.Play("CrossAttackPrepare");
				SetAllSwordState(Boss55_Sword.SwordState.Aim);
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			CrossSwordDir(base.TargetPoint);
			break;
		case MonsterState.CrossAttack:
		{
			ref float reference22 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("CrossAttack");
			}
			reference22 += Time.deltaTime;
			if (reference22 > crossAttackInterval)
			{
				reference22 -= crossAttackInterval;
				CrossSwordShoot();
			}
			CrossSwordDir(base.TargetPoint);
			KeepDistanceMove(crossAttackSpeedRatio);
			if (stateExistTime > crossAttackDuration)
			{
				state = MonsterState.CrossAttackAfter;
			}
			break;
		}
		case MonsterState.CrossAttackAfter:
			if (changedState)
			{
				base.Anima.Play("CrossAttackAfter");
				SetAllSwordState(Boss55_Sword.SwordState.BackToIdle);
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			break;
		case MonsterState.SpinAttackPrepare:
			if (changedState)
			{
				SEMgr.Inst.boss55SendSword.PlaySE();
				base.Anima.Play("SpinAttackPrepare");
				SetAllSwordState(Boss55_Sword.SwordState.Aim);
				spinAngle = UnityEngine.Random.Range(0, 360);
			}
			SpinSwordDir(spinAngle);
			KeepDistanceMove(crossAttackSpeedRatio);
			break;
		case MonsterState.SpinAttack:
		{
			ref float reference17 = ref varMgr.RegFloat(0);
			ref float reference18 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("SpinAttack");
				reference18 = GeneralTool.HalfChanceNPOne();
			}
			reference17 += Time.deltaTime;
			if (reference17 > spinAttackInterval)
			{
				reference17 -= spinAttackInterval;
				CrossSwordShoot();
			}
			spinAngle += Time.deltaTime * spinRotateSpeed * reference18;
			SpinSwordDir(spinAngle);
			KeepDistanceMove(crossAttackSpeedRatio);
			if (stateExistTime > spinAttackDuration)
			{
				state = MonsterState.SpinAttackAfter;
			}
			break;
		}
		case MonsterState.SpinAttackAfter:
			if (changedState)
			{
				base.Anima.Play("SpinAttackAfter");
				SetAllSwordState(Boss55_Sword.SwordState.BackToIdle);
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			break;
		case MonsterState.SpinDashAim:
		{
			if (changedState)
			{
				SEMgr.Inst.boss55SendSword.PlaySE();
				base.Anima.Play("SpinDashPrepare");
				spinDashRotateClockwise = GeneralTool.HalfChanceNPOne();
				spin = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Spin" + ((spinDashRotateClockwise > 0f) ? "" : "_Reversed"), base.transform.position).GetComponent<Boss55_Spin>();
				SetAllSwordState(Boss55_Sword.SwordState.Aim);
				spinAngle = UnityEngine.Random.Range(0, 360);
				spinDashWarningLine.enabled = true;
				spinDashCounter = 0;
				spinBladeDir = ToTargetDir();
				SetFlip(ToTargetDir().x);
			}
			float num3 = 50f;
			for (int l = 0; l < spinDashWarningLine.positionCount; l++)
			{
				Vector3 rootPoint2 = Vector3.Lerp(base.transform.position, base.transform.position + spinBladeDir * num3, (float)l / (float)(spinDashWarningLine.positionCount - 1));
				spinDashWarningLine.SetPosition(l, Tool2D.GetLayerPoint(rootPoint2, LayerCorrectType.GroundEffect));
			}
			spinAngle += Time.deltaTime * spinBladeRotateSpeed * spinDashRotateClockwise;
			SpinSlashSwordDir(spinAngle, base.transform.position);
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.SpinDash:
		{
			ref float reference9 = ref varMgr.RegFloat(0);
			ref bool reference10 = ref varMgr.RegBool(0);
			ref bool reference11 = ref varMgr.RegBool(1);
			ref float reference12 = ref varMgr.RegFloat(1);
			ref float reference13 = ref varMgr.RegFloat(2);
			if (changedState)
			{
				base.Anima.Play("SpinDash");
				spinDashWarningLine.enabled = false;
				reference9 = 0f;
				reference10 = false;
				SEMgr.Inst.boss55SpinDash.PlaySE();
				reference11 = Tool2D.PointOnNavMesh(base.transform.position);
				spinDashCounter++;
				reference13 = UnityEngine.Random.Range(0f, 360f);
			}
			spinAngle += Time.deltaTime * spinBladeRotateSpeed * spinDashRotateClockwise;
			SpinSlashSwordDir(spinAngle, base.transform.position);
			if (reference9 > spinDashBulletStartShootSpeed)
			{
				reference12 += Time.deltaTime;
			}
			while (reference12 > spinDashBulletShootInverval)
			{
				reference13 += spinDashBulletShootAngleInterval * spinDashRotateClockwise;
				reference12 -= spinDashBulletShootInverval;
				ShootSpinSingleBullet(Tool2D.GetDir(reference13));
			}
			if (!reference10)
			{
				if (Tool2D.PointOnNavMesh(base.transform.position))
				{
					reference11 = true;
				}
				reference9 += Time.deltaTime * spinDashAccleration;
				reference9 = Mathf.Min(reference9, spinDashMaxSpeed);
				float num2 = reference9 * Time.deltaTime;
				spinBladeDir = Tool2D.RotateTowardsAroundZAxis(spinBladeDir, ToTargetDir(), num2 * spinDashDirRotateSpeed);
				base.transform.position += num2 * spinBladeDir;
				if (!Tool2D.PointOnNavMesh(base.transform.position) & reference11)
				{
					reference10 = true;
				}
			}
			SyncDotsPosition();
			if (reference10 && stateExistTime > spinDashMinTime)
			{
				if (spinDashCounter >= spinDashCount)
				{
					state = MonsterState.SpinDashAfter;
				}
				else
				{
					state = MonsterState.SpinDashAgain;
				}
			}
			break;
		}
		case MonsterState.SpinDashAgain:
		{
			if (changedState)
			{
				base.Anima.Play("SpinDashAgain");
				spinDashWarningLine.enabled = true;
				CrossSwordShoot();
				spinBladeDir = ToTargetDir();
				SetFlip(ToTargetDir().x);
			}
			float num3 = 50f;
			for (int j = 0; j < spinDashWarningLine.positionCount; j++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + spinBladeDir * num3, (float)j / (float)(spinDashWarningLine.positionCount - 1));
				spinDashWarningLine.SetPosition(j, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			spinAngle += Time.deltaTime * spinBladeRotateSpeed * spinDashRotateClockwise;
			SpinSlashSwordDir(spinAngle, base.transform.position);
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDir().x);
			break;
		}
		case MonsterState.SpinDashAfter:
		{
			ref Vector3 reference16 = ref varMgr.RegV3(0);
			if (changedState)
			{
				spin.transform.position = base.transform.position;
				spin.End();
				base.Anima.Play("SpinDashAfter");
				reference16 = base.transform.position;
				spin = null;
				spinBladeSpining = true;
				CrossSwordShoot();
			}
			if (spinBladeSpining)
			{
				spinAngle += Time.deltaTime * spinBladeRotateSpeed * spinDashRotateClockwise;
				SpinSlashSwordDir(spinAngle, reference16);
			}
			base.transform.position = Vector3.SmoothDamp(base.transform.position, reference16, ref smoothLerpSpeed, smoothLerpTime);
			SyncDotsPosition();
			SetFlip(ToPointDir(reference16).x);
			break;
		}
		case MonsterState.SwordAttackPrepare:
			if (changedState)
			{
				base.Anima.Play("SwordAttackPrepare");
				for (int k = 0; k < swords.Count; k++)
				{
					if (k % secondStageHalfSwordCount < nowStageHalfSwordCount)
					{
						swords[k].dashAutoBack = false;
					}
				}
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			break;
		case MonsterState.SwordAttack:
		{
			ref float reference20 = ref varMgr.RegFloat(0);
			ref float reference21 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("SwordAttack");
			}
			reference20 += Time.deltaTime;
			if (reference20 > swordLaunchInterval)
			{
				reference20 -= swordLaunchInterval;
				for (int num4 = 0; (float)num4 < swordAttackCount; num4++)
				{
					int randomFreeSwordIndex2 = GetRandomFreeSwordIndex();
					float num5 = UnityEngine.Random.Range(0f, 360f);
					float num6 = Mathf.Abs(num5 - reference21);
					while (num6 < swordAttackMinAngle || num6 > 360f - swordAttackMinAngle)
					{
						num5 = UnityEngine.Random.Range(0f, 360f);
						num6 = Mathf.Abs(num5 - reference21);
					}
					reference21 = num5;
					Vector3 dash = PlayerMgr.Inst.PlayerPointIgnoreZ + Tool2D.GetDir(num5) * swordLaunchDistance.RandomResult();
					if (randomFreeSwordIndex2 != -1)
					{
						swords[randomFreeSwordIndex2].SetDash(dash);
					}
				}
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			if (stateExistTime > swordAttackDuration)
			{
				state = MonsterState.SwordAttackAfter;
			}
			break;
		}
		case MonsterState.SwordAttackAfter:
			if (changedState)
			{
				base.Anima.Play("SwordAttackAfter");
				for (int m = 0; m < swords.Count; m++)
				{
					swords[m].dashAutoBack = true;
				}
			}
			KeepDistanceMove(crossAttackSpeedRatio);
			break;
		case MonsterState.RotateSlashPrepare:
		{
			ref Vector3 reference14 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.boss55SendSword.PlaySE();
				base.Anima.Play("RotateSlashPrepare");
				rotateSlashBaseDir = ToTargetDir();
				rotateSlashDir = GeneralTool.HalfChanceNPOne();
				reference14 = Tool2D.PointWithinRange(base.transform.position, roomCenterPoint, roomWidth - rotateLaserFromBorderDistance * 2f, roomHeight - rotateLaserFromBorderDistance * 2f);
				SetRotateSlashSwordState(Boss55_Sword.SwordState.RotateSlashAim);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = false;
				SetComponentData(componentData);
				spinAngle = 90f;
			}
			RotateSlashSwordDir(spinAngle);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, reference14, ref smoothLerpSpeed, smoothLerpTime);
			SyncDotsPosition();
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDir().x);
			break;
		}
		case MonsterState.RotateSlash:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("RotateSlash");
				SetRotateSlashSwordState(Boss55_Sword.SwordState.RotateSlash);
				reference = GetBigSkillRandomMovePoint(reference);
				RotateSlashBulletShoot();
			}
			reference2 += Time.deltaTime;
			if (reference2 > rotateSlashShootInterval)
			{
				reference2 -= rotateSlashShootInterval;
				RotateSlashBulletShoot();
			}
			spinAngle += Time.deltaTime * rotateSlashRotateSpeed * rotateSlashDir;
			RotateSlashSwordDir(spinAngle);
			SetMove(ToPointDir(reference) * base.MoveSpeed * rotateSlashMoveSpeedRatio, isFlip: false);
			SetFlip(ToTargetDir().x);
			if (ToPointDistanceSqr(reference) < 0.25f)
			{
				reference = GetBigSkillRandomMovePoint(reference);
			}
			if (stateExistTime > rotateSlashDuration)
			{
				state = MonsterState.RotateSlashAfter;
			}
			break;
		}
		case MonsterState.RotateSlashAfter:
			if (changedState)
			{
				base.Anima.Play("RotateLaserAfter");
				SetRotateSlashSwordState(Boss55_Sword.SwordState.RotateSlashAfter);
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.CanTouch = true;
				SetComponentData(componentData4);
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDir().x);
			break;
		case MonsterState.RotateLaserPrepare:
		{
			ref Vector3 reference19 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.boss55SendSword.PlaySE();
				base.Anima.Play("RotateLaserPrepare");
				SetRotateLaserSwordState(Boss55_Sword.SwordState.LaserAim);
				reference19 = Tool2D.PointWithinRange(base.transform.position, roomCenterPoint, roomWidth - rotateLaserFromBorderDistance * 2f, roomHeight - rotateLaserFromBorderDistance * 2f);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				SetComponentData(componentData2);
			}
			LaserSwordDir(spinAngle);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, reference19, ref smoothLerpSpeed, smoothLerpTime);
			SyncDotsPosition();
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDir().x);
			break;
		}
		case MonsterState.RotateLaser:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			ref float reference4 = ref varMgr.RegFloat(1);
			ref float reference5 = ref varMgr.RegFloat(2);
			ref float reference6 = ref varMgr.RegFloat(3);
			ref float reference7 = ref varMgr.RegFloat(4);
			ref Vector3 reference8 = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("RotateLaser");
				reference3 = GeneralTool.HalfChanceNPOne();
				for (int i = 0; i < swords.Count; i++)
				{
					if (i % secondStageHalfSwordCount < rotateLaserHalfCount)
					{
						swords[i].laser.state = Boss55_Laser.LaserState.Attack;
					}
				}
				ShootLaserCircleBullet();
				rotateLaserChangeDirTime.RandomResult();
				reference8 = GetBigSkillRandomMovePoint(reference8);
			}
			reference6 += Time.deltaTime;
			if (reference6 > laserCircleBulletInterval)
			{
				reference6 -= laserCircleBulletInterval;
				ShootLaserCircleBullet();
			}
			reference4 += Time.deltaTime;
			if (reference4 > laserBulletShootInterval)
			{
				reference4 -= laserBulletShootInterval;
				float num = UnityEngine.Random.Range(0f, 180f / (float)rotateLaserHalfCount);
				if (Mathf.Abs(reference5 - num) < laserBulletMinAngleInterval || Mathf.Abs(reference5 - num) > 180f / (float)rotateLaserHalfCount - laserBulletMinAngleInterval)
				{
					num = reference5 + laserBulletMinAngleInterval * GeneralTool.HalfChanceNPOne();
				}
				reference5 = num;
				ShootLaserBullet(num);
			}
			reference7 += Time.deltaTime;
			if (reference7 > rotateLaserChangeDirTime.result)
			{
				reference7 = 0f;
				rotateLaserChangeDirTime.RandomResult();
				reference3 *= -1f;
			}
			spinAngle += Time.deltaTime * laserRotateSpeed * reference3;
			LaserSwordDir(spinAngle);
			SetMove(ToPointDir(reference8) * base.MoveSpeed * rotateLaserMoveSpeedRatio, isFlip: false);
			SetFlip(ToTargetDir().x);
			if (ToPointDistanceSqr(reference8) < 0.25f)
			{
				reference8 = GetBigSkillRandomMovePoint(reference8);
			}
			if (stateExistTime > laserDuration)
			{
				state = MonsterState.RotateLaserAfter;
			}
			break;
		}
		case MonsterState.RotateLaserAfter:
			if (changedState)
			{
				SEMgr.Inst.elite11LaserEnd.PlaySE();
				base.Anima.Play("RotateLaserAfter");
				SetAllSwordState(Boss55_Sword.SwordState.BackToIdle);
				for (int n = 0; n < swords.Count; n++)
				{
					if (n % secondStageHalfSwordCount < rotateLaserHalfCount)
					{
						swords[n].laser.state = Boss55_Laser.LaserState.Fade;
					}
				}
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = true;
				SetComponentData(componentData3);
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDir().x);
			break;
		case MonsterState.RotateSmallLaser:
			if (changedState)
			{
				base.Anima.Play("RotateSmallLaser");
			}
			KeepDistanceMove();
			SetFlip(ToTargetDir().x);
			break;
		case MonsterState.SwordFreeAttack:
		{
			ref float reference15 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("SideSword");
			}
			KeepDistanceMove();
			reference15 += Time.deltaTime;
			if (reference15 > freeAttackInterval)
			{
				reference15 -= freeAttackInterval;
				int randomFreeSwordIndex = GetRandomFreeSwordIndex();
				if (randomFreeSwordIndex >= 0)
				{
					swords[randomFreeSwordIndex].state = Boss55_Sword.SwordState.FreeMove;
				}
			}
			if (stateExistTime > freeAttackTime)
			{
				SetAllSwordState(Boss55_Sword.SwordState.BackToIdle);
				state = MonsterState.RandomMove;
			}
			break;
		}
		case MonsterState.SmallLaser:
			if (changedState)
			{
				base.Anima.Play("SmallLaser");
			}
			KeepDistanceMove();
			break;
		case MonsterState.SideSlash:
			if (changedState)
			{
				base.Anima.Play("SideSlash");
			}
			KeepDistanceMove();
			break;
		case MonsterState.CurveSwordAttack:
			if (changedState)
			{
				base.Anima.Play("CurveSlash");
				curveSlashLeft = GeneralTool.ChanceResult(0.5f);
			}
			KeepDistanceMove();
			break;
		case MonsterState.SwitchStage:
			if (changedState)
			{
				base.Anima.Play("SwitchStage");
				switchStageParticle.Play();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SwordAttackFollow:
			break;
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < swords.Count; i++)
		{
			swords[i].SyncColor(myPpt.BaseColor);
		}
		for (int num = bullets.Count - 1; num >= 0; num--)
		{
			if (bullets[num] == null || !bullets[num].gameObject.activeSelf)
			{
				bullets.RemoveAt(num);
			}
			else
			{
				float num2 = (bullets[num].transform.position - bullets[num].lastFramePos).magnitude;
				Vector3 vector = bullets[num].lastFramePos;
				if (bulletTrailInterval > 0f)
				{
					while (num2 > bulletTrailInterval)
					{
						num2 -= bulletTrailInterval;
						vector = bullets[num].transform.position - bullets[num].direction * num2;
						bulletTrailParam.Add(new GlobalParticleEmitParams(GlobalParticleType.EF, "EF_Boss55_BulletTrail", Tool2D.GetLayerPoint(vector) + bullets[num].bulletHeight * Vector3.up));
					}
				}
				bullets[num].lastFramePos = vector;
			}
		}
		GlobalParticleEmitSystem.AddEmitParams(bulletTrailParam);
	}

	private void SetAllSwordState(Boss55_Sword.SwordState state)
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < nowStageHalfSwordCount)
			{
				swords[i].state = state;
			}
		}
	}

	private void SetRotateLaserSwordState(Boss55_Sword.SwordState state)
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < rotateLaserHalfCount)
			{
				swords[i].state = state;
				if (state == Boss55_Sword.SwordState.LaserAim)
				{
					swords[i].isAutoLaser = false;
				}
			}
		}
	}

	private void SetRotateSlashSwordState(Boss55_Sword.SwordState state)
	{
		for (int i = 0; i < 2; i++)
		{
			Vector3 dir = Tool2D.GetDir(rotateSlashBaseDir, (float)i * 180f);
			for (int j = 0; j < 6; j++)
			{
				int index = i * 6 + j;
				if (state == Boss55_Sword.SwordState.RotateSlashAim)
				{
					Vector3 startPoint = base.transform.position + dir * (rotateSlashDistanceInterval * (float)j + 1f);
					swords[index].SetRotateSlash(startPoint, base.transform.position, rotateSlashRotateSpeed * rotateSlashDir);
				}
				else
				{
					swords[index].state = state;
				}
			}
		}
	}

	private void RotateSlashSwordDir(float angle)
	{
		for (int i = 0; i < 2; i++)
		{
			Vector3 dir = Tool2D.GetDir(rotateSlashBaseDir, angle + (float)i * 180f);
			for (int j = 0; j < 6; j++)
			{
				int index = i * 6 + j;
				swords[index].SetRotateSlashPose(base.transform.position, dir);
			}
		}
	}

	private void LaserSwordDir(float angle)
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < rotateLaserHalfCount)
			{
				Vector3 dir = Tool2D.GetDir(angle + 180f / (float)rotateLaserHalfCount * (float)(i / secondStageHalfSwordCount * rotateLaserHalfCount + i % secondStageHalfSwordCount));
				Vector3 postion = base.transform.position + dir * spinSwordDistance;
				swords[i].SetBattlePose(postion, dir);
			}
		}
	}

	private void SetSmallLaser(bool firstAttack)
	{
		SEMgr.Inst.boss55SendSword.PlaySE();
		for (int i = 0; i < nowStageHalfSwordCount; i++)
		{
			int index = i + ((!firstAttack) ? secondStageHalfSwordCount : 0);
			float num = (firstAttack ? smallLaserPosOffset.RandomResult() : smallLaserPosOffset2.RandomResult());
			float num2 = (firstAttack ? smallLaserAimOffset.RandomResult() : smallLaserAimOffset2.RandomResult());
			swords[index].state = Boss55_Sword.SwordState.LaserAim;
			swords[index].isAutoLaser = true;
			Vector3 vector = base.transform.position + Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, PlayerMgr.Inst.PlayerPoint), GeneralTool.HalfChanceNPOne() * UnityEngine.Random.Range(45f, 90f)) * num;
			swords[index].SetBattlePose(vector, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * smallLaserPredictTime + Tool2D.GetDir() * num2, vector));
		}
	}

	private Vector3 GetBigSkillRandomMovePoint(Vector3 startPoint)
	{
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < 100; i++)
		{
			vector = base.transform.position + Tool2D.GetDir() * rotateLaserRepositionDistance.RandomResult();
			Vector3 vector2 = Tool2D.PointWithinRange(vector, roomCenterPoint, roomWidth - rotateLaserFromBorderDistance * 2f, roomHeight - rotateLaserFromBorderDistance * 2f);
			if (vector == vector2)
			{
				break;
			}
			if (i == 99)
			{
				vector = vector2;
			}
		}
		return vector;
	}

	private void SetRotateSmallLaser(bool firstAttack)
	{
		SEMgr.Inst.boss55SendSword.PlaySE();
		float rotateDir = GeneralTool.HalfChanceNPOne();
		Vector3 vector = PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * smallLaserPredictTime;
		Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(vector, base.transform.position);
		for (int i = 0; i < nowStageHalfSwordCount; i++)
		{
			int index = i + ((!firstAttack) ? secondStageHalfSwordCount : 0);
			float num = (firstAttack ? smallLaserPosOffset.RandomResult() : smallLaserPosOffset2.RandomResult());
			float num2 = (firstAttack ? smallLaserAimOffset.RandomResult() : smallLaserAimOffset2.RandomResult());
			float num3 = GeneralTool.HalfChanceNPOne();
			float num4 = GeneralTool.HalfChanceNPOne();
			switch (i)
			{
			case 0:
			case 2:
				num3 = -1f;
				break;
			case 1:
			case 3:
				num3 = 1f;
				break;
			}
			switch (i)
			{
			case 0:
			case 1:
				num4 = -1f;
				break;
			case 2:
			case 3:
				num4 = 1f;
				break;
			}
			Vector3 vector2 = base.transform.position + Tool2D.GetDir(oldDir, num3 * UnityEngine.Random.Range(90f, 120f)) * num;
			Vector3 targetOffset = Tool2D.IgnoreZPoint(vector + Tool2D.GetDir(oldDir, num4 * UnityEngine.Random.Range(60f, 120f)) * num2 - vector2);
			swords[index].SetRotateSmallLaser(vector2, targetOffset, rotateDir);
		}
	}

	private void SetSideSlash(bool firstAttack)
	{
		SEMgr.Inst.boss55SendSword.PlaySE();
		Vector3 oldDir = ToTargetDir();
		float num = (firstAttack ? 1f : (-1f));
		float rotateDir = 0f - num;
		Vector3 dir = Tool2D.GetDir(oldDir, sideSlashStartAngle * num);
		Vector3 position = base.transform.position;
		float num2 = UnityEngine.Random.Range(0f, 1f);
		for (int i = 0; i < nowStageHalfSwordCount; i++)
		{
			int index = i + ((!firstAttack) ? secondStageHalfSwordCount : 0);
			Vector3 startPoint = position + dir * sideSlashDistanceInterval * ((float)i + num2);
			swords[index].SetSideSlash(startPoint, position, rotateDir);
		}
	}

	private void KeepDistanceMove(float speedRatio = 1f)
	{
		moveTimer += Time.deltaTime;
		CheckNavInfo();
		bool flag = ToTargetDistance() > keepDistanceRange.value1 && ToTargetDistance() < keepDistanceRange.value2;
		if (navInfo.allCornerArrived || (!flag && moveTimer > keepDistanceTime.result))
		{
			keepDistanceTime.RandomResult();
			moveTimer = 0f;
			GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistanceRange, -ToTargetDir(), keepDistanceAngleRange.RandomResult() * GeneralTool.HalfChanceNPOne()));
		}
		else
		{
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * speedRatio);
		}
		SetFlip(ToTargetDir().x);
	}

	private void LaunchCurveSword(bool isLeft, bool isFlipX)
	{
		Vector3 vector = ToTargetDir();
		Vector3 dir = Tool2D.GetDir(vector, isLeft ? 90 : (-90));
		float num = ToTargetDistance();
		float num2 = curveEndPointOffsetRange / (float)(nowStageHalfSwordCount - 1);
		float num3 = UnityEngine.Random.Range(-0.5f, 0.5f);
		for (int i = 0; i < nowStageHalfSwordCount; i++)
		{
			int index = ((isLeft ^ !isFlipX) ? i : (i + secondStageHalfSwordCount));
			float num4 = num2 * ((float)i - (float)(nowStageHalfSwordCount - 1) / 2f + num3);
			Vector3 vector2 = base.transform.position + (0f - num + num4) * vector / 2f + dir * curveMiddlePointOffset;
			Vector3 dir2 = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, vector2), 90f);
			Vector3 endPoint = PlayerMgr.Inst.PlayerPoint + num4 * dir2;
			Vector3 startPoint = base.transform.position + dir * 2f;
			swords[index].SetCurveSlash(startPoint, vector2, endPoint, Mathf.Lerp(curveStandardTime, curveStandardTime + curveMaxExtraTime, (num - curveStandardDistance) / curveMaxExtraDistance));
		}
	}

	private void CrossSwordShoot()
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < nowStageHalfSwordCount)
			{
				swords[i].CrossShoot();
			}
		}
	}

	private void CrossSwordDir(Vector3 point)
	{
		Vector3 vector = ToPointDir(point);
		Vector3 dir = Tool2D.GetDir(vector, 90f);
		Vector3 targetPoint = base.TargetPoint;
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < nowStageHalfSwordCount)
			{
				float num = ((i < nowStageHalfSwordCount) ? 1 : (-1));
				int num2 = i % secondStageHalfSwordCount;
				Vector3 vector2 = base.transform.position + dir * num * crossHorizontalOffset - vector;
				Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(targetPoint, vector2);
				oldDir = Tool2D.GetDir(oldDir, crossAttackAngle * (1f / (float)(nowStageHalfSwordCount - 1) * (float)num2 - 0.5f));
				vector2 += oldDir;
				swords[i].SetBattlePose(vector2, oldDir);
			}
		}
	}

	private void SpinSwordDir(float angle)
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < nowStageHalfSwordCount)
			{
				Vector3 dir = Tool2D.GetDir(angle + 360f / (float)(nowStageHalfSwordCount * 2) * (float)(i / secondStageHalfSwordCount * nowStageHalfSwordCount + i % secondStageHalfSwordCount));
				Vector3 postion = base.transform.position + dir * spinSwordDistance;
				swords[i].SetBattlePose(postion, dir);
			}
		}
	}

	private void SpinSlashSwordDir(float angle, Vector3 point)
	{
		for (int i = 0; i < swords.Count; i++)
		{
			if (i % secondStageHalfSwordCount < nowStageHalfSwordCount)
			{
				Vector3 dir = Tool2D.GetDir(angle + 360f / (float)(nowStageHalfSwordCount * 2) * (float)(i / secondStageHalfSwordCount * nowStageHalfSwordCount + i % secondStageHalfSwordCount));
				Vector3 postion = point + dir * spinSwordDistance;
				swords[i].ForceSetBattlePose(postion, dir);
			}
		}
	}

	private void ShootLaserBullet(float angle)
	{
		SEMgr.Inst.boss55BulletShootSmall.PlaySE();
		for (int i = 0; i < rotateLaserHalfCount * 2; i++)
		{
			Vector3 dir = Tool2D.GetDir(angle + 180f / (float)rotateLaserHalfCount * (float)i);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(dir, laserBulletSpeed, myPpt.myEntity);
		}
	}

	private void ShootSpinSingleBullet(Vector3 dir)
	{
		SEMgr.Inst.boss55BulletShootSmall.PlaySE(SEPlayMode.Replay, 3, 0.18f);
		Vector3 point = Tool2D.IgnoreZPoint(base.transform.position + dir * spinDashBulletDistance);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", point).GetComponent<Boss55_Bullet>().Initialize(dir, spinDashBulletSpeed, myPpt.myEntity);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Shoot", point, 1f);
	}

	private void ShootLaserCircleBullet()
	{
		shootBulletParticle.Play();
		SEMgr.Inst.boss55CircleBulletShoot.PlaySE();
		float num = UnityEngine.Random.Range(0f, 360f);
		for (int i = 0; (float)i < laserCircleBulletCount; i++)
		{
			Vector3 dir = Tool2D.GetDir(num + 360f / laserCircleBulletCount * (float)i);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(dir, laserCircleBulletSpeed, myPpt.myEntity);
		}
	}

	private void RotateSlashBulletShoot()
	{
		SEMgr.Inst.boss55CircleBulletShoot.PlaySE();
		float num = 360f / rotateSlashShootCount;
		for (int i = 0; (float)i < rotateSlashShootCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(Tool2D.GetDir(num * ((float)i + UnityEngine.Random.Range(0f, 1f))), rotateSlashBulletSpeed.RandomResult(), myPpt.myEntity);
		}
	}

	protected override void BossDeadStay()
	{
		if (spin != null)
		{
			ObjPoolMgr.Inst.RecycleGO(spin.gameObject);
		}
		for (int i = 0; i < swords.Count; i++)
		{
			swords[i].SyncColor(Color.white);
		}
		AS_LaserLoop.Stop();
		spinDashWarningLine.enabled = false;
		BossDeadStayKeepPresents();
		state = MonsterState.Dead;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < swords.Count; i++)
		{
			ObjPoolMgr.Inst.RecycleGO(swords[i].gameObject);
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "SpinDashStopAiming":
			break;
		case "SpinAttack":
			state = MonsterState.SpinAttack;
			break;
		case "CrossAttack":
			state = MonsterState.CrossAttack;
			break;
		case "SwordAttack":
			state = MonsterState.SwordAttack;
			break;
		case "CrossAttackFinish":
			state = MonsterState.Move;
			break;
		case "RotateLaser":
			state = MonsterState.RotateLaser;
			break;
		case "RotateSlash":
			state = MonsterState.RotateSlash;
			break;
		case "RotateSmallLaser":
			SetRotateSmallLaser(firstAttack: true);
			break;
		case "RotateSmallLaserAgain":
			SetRotateSmallLaser(firstAttack: false);
			break;
		case "RotateSmallLaserFinish":
			state = MonsterState.Move;
			break;
		case "SmallLaser":
			SetSmallLaser(firstAttack: true);
			break;
		case "SmallLaserAgain":
			SetSmallLaser(firstAttack: false);
			break;
		case "SmallLaserFinish":
			state = MonsterState.Move;
			break;
		case "SideSlash":
			SetSideSlash(firstAttack: true);
			break;
		case "SideSlashAgain":
			SetSideSlash(firstAttack: false);
			break;
		case "SideSlashFinish":
			state = MonsterState.Move;
			break;
		case "AttackFinish":
			state = MonsterState.Move;
			break;
		case "CurveSword":
			curveSlashFlip = myPpt.isFlipX;
			SEMgr.Inst.boss55LaunchCurveSword.PlaySE();
			LaunchCurveSword(curveSlashLeft, curveSlashFlip);
			curveSlashLeft = !curveSlashLeft;
			break;
		case "CurveSwordAgain":
			SEMgr.Inst.boss55LaunchCurveSword.PlaySE();
			LaunchCurveSword(curveSlashLeft, curveSlashFlip);
			curveSlashLeft = !curveSlashLeft;
			break;
		case "SpinDash":
			state = MonsterState.SpinDash;
			break;
		case "SpinDashSwordBack":
			SetAllSwordState(Boss55_Sword.SwordState.BackToIdle);
			spinBladeSpining = false;
			break;
		case "SummonSword":
		{
			for (int i = 0; i < swords.Count; i++)
			{
				if (i % secondStageHalfSwordCount >= firstStageHalfSwordCount)
				{
					swords[i].state = Boss55_Sword.SwordState.Show;
				}
			}
			break;
		}
		case "SwitchStageParticle":
			switchStageParticle.Stop();
			switchStageFinishParticle.Play();
			break;
		}
	}
}
