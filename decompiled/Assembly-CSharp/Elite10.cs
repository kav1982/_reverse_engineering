using System;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Elite10 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		DougeBefore,
		SideDouge,
		SideDougeAfter,
		SideDougeAttack,
		Blast,
		KnockBefore,
		Knock,
		ContinueKnockBefore,
		ContinueKnock,
		ContinueKnockAfter,
		SecondStageFly,
		SecondStageDrop,
		FlyDashBefore,
		FlyDash,
		DashBackToGround,
		FlyDashGround,
		FlyBombPrepare,
		FlyBomb,
		FlyBackToGround,
		Drop,
		MissileBefore,
		Missile,
		MissileFinish
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("站立")]
	public VariableFloat idleTime;

	[Header("飞行")]
	public float SlowDownLerp;

	public float flyingFloatAmplitude;

	public float flySkillPunishTime;

	private float extraIdleTime;

	private Vector3 flyDiration;

	[Header("飞行判定点")]
	public Transform tsf_FootPoint;

	public Transform tsf_WingPoint;

	public Transform tsf_Model;

	public Transform tsf_HeightWave;

	public float immumeHeight;

	private float originalFrozenTimeRatio;

	[Header("轰炸")]
	public float bombRotateSpeed;

	public float maxBombTime;

	public float trackBombTime;

	public float bombPointOffset;

	public Transform tsf_FirePoint;

	public int bombCount;

	public float bombWaveFrequency;

	public float bombPathWidth;

	public int secondStageBombCount;

	public float secondStageBombWaveFrequency;

	public float secondStageBombPathWidth;

	public float bombInterval;

	public float bombKeepDistance;

	public float bombSpeed;

	public ParticleSystem bombParticle;

	public ParticleSystem bombParticle_H;

	[Header("起飞前摇")]
	public float beforeFlyTime;

	[Header("导弹")]
	public int missileCount;

	public int secondStageMissileCount;

	public float missileInterval;

	public float missileSpeed;

	public float MissileTime;

	public VariableFloat missileRadius;

	public VariableFloat missileRadius1;

	public VariableFloat missileKeepDistance;

	public VariableFloat randomFlyRadius;

	public float missileKeepAngle;

	public float missileFlySpeedRatio;

	public VariableFloat missilePredictTime;

	[Header("闪避")]
	public float dougsStartTime;

	public VariableFloat dougeKeepDistance;

	public VariableFloat dougeRadius;

	public AnimationCurve dougeSpeedCurve;

	public float dougeAgainChance;

	public int maxDougeTime;

	private int dougeTimeCounter;

	[Header("闪避飞镖")]
	public int arrowCount;

	public int secondStageArrowCount;

	public float arrowAngle;

	public float arrowMinAngle;

	public VariableFloat arrowDistanceRange;

	[Header("下砸")]
	public float knockWaveCount;

	public float knockWaveCountLow;

	public float knockWaveAngle;

	public float knockChance;

	public ShockParam knockShock;

	public ParticleSystem knockParticle;

	public ParticleSystem knockParticle_H;

	[Header("连续下砸")]
	public float continueKnockMaxDistance;

	public float continueKnockKeepDistance;

	public int continueKnockTime;

	public AnimationCurve knockSpeedCurve;

	private int continueKnockCounter;

	[Header("俯冲冲撞")]
	public float dashBeforeDistance;

	public float dashDistance;

	public float dashAfterDistance;

	public AnimationCurve dashBeforeSpeedCurve;

	public AnimationCurve dashSpeedCurve;

	public AnimationCurve dashAfterSpeedCurve;

	public Elite10_DamageZone thisDamageZone;

	public int maxDashTime;

	public float dashAgainChance;

	public LineRenderer warningLine;

	public LineRenderer warningLine_H;

	public float dashMineInterval;

	public float dashMineOffset;

	private bool dashBackFinish;

	[HideInInspector]
	public Vector3 dashDir;

	private int dashTimeCounter;

	private Vector3 warningStartPoint;

	private Vector3 warningEndPoint;

	[Header("冲撞子弹")]
	public float spellHeight;

	public VariableFloat spellSpeed;

	public VariableFloat spellDuration;

	public VariableInt spellDamage;

	public float blockBulletCount;

	public float blockBulletInterval;

	private SpellSpawnParams ssp;

	[Header("分裂弹")]
	public ParticleSystem chargeParticle;

	public ParticleSystem blastParticle;

	public ParticleSystem chargeParticle_H;

	public ParticleSystem blastParticle_H;

	[Header("翅膀")]
	public Elite10_Wing pfb_WingR;

	public Elite10_Wing pfb_WingL;

	private Elite10_Wing wingR;

	private Elite10_Wing wingL;

	[Header("表现")]
	public Transform tsf_FlipRoot;

	public ParticleSystem dropParticle;

	public ParticleSystem dropParticleLarge;

	[Header("技能概率和限制")]
	public float bombChance;

	public float missileChance;

	public float dashChance;

	public float dougeChance;

	public float continueKnockChance;

	public float dougeAttackRange;

	public float dougeFinishChance;

	public float dougeAttackChance;

	public float dougeDashChance;

	public float dougeAttackDashChance;

	public float dougeBlastChance;

	public float knockMaxDistance;

	public float secondStageKnockMaxDistance;

	private MonsterState lastEndSkill;

	private MonsterState lastDougeSkill;

	private int skillCounter;

	[Header("二阶段")]
	public float secondStageRatio;

	public bool inSecondStage;

	public Shadow thisShadow;

	private float nowShadowScaleRatio;

	public float scaleTime;

	private float originShadowScale;

	public Light2D warningLight;

	public float originWarningLightRadius;

	private bool secondStageFlyStarted;

	private bool secondStageDropped;

	public ParticleSystem warningParticle;

	public ParticleSystem warningParticleH;

	[Header("镜头控制")]
	private CameraFocusSizeData camFocusData;

	[Header("隐身处理")]
	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public static MiniObjPool MiniPool;

	public static Elite10 Inst;

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

	private Vector3 bombPoint => base.transform.position + flyDiration.normalized * bombPointOffset;

	public override void SingleInitialCallback()
	{
		wingR = UnityEngine.Object.Instantiate(pfb_WingR, base.transform).GetComponent<Elite10_Wing>();
		wingL = UnityEngine.Object.Instantiate(pfb_WingL, base.transform).GetComponent<Elite10_Wing>();
		wingR.SingleInitial(this, Tool2D.GetDir(-80f), isLeft: false);
		wingL.SingleInitial(this, Tool2D.GetDir(80f), isLeft: true);
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90201);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
		if (GameMgr.IsChAge14_Static)
		{
			bombParticle = bombParticle_H;
			chargeParticle = chargeParticle_H;
			blastParticle = blastParticle_H;
			knockParticle = knockParticle_H;
			warningLine = warningLine_H;
			warningLight.color = Color.magenta;
		}
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName += "_HX";
			base.SAnima.Initialize(overwrite: true);
			warningParticle = warningParticleH;
		}
		warningLine.positionCount = 10;
		if (GameMgr.IsMobile_Static)
		{
			bombSpeed *= 1.25f;
			missileInterval *= 1.25f;
			bombInterval *= 1.25f;
			bombWaveFrequency *= 0.8f;
			secondStageBombWaveFrequency *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		wingR.EveryInitial();
		wingL.EveryInitial();
		warningLine.enabled = false;
		state = MonsterState.BornIdle;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCfg.theme8Height;
		camFocusData = new CameraFocusSizeData(0f, 1, 1000000f);
		CamController.Inst.AddNewCameraFocusRequirement(camFocusData);
	}

	protected override void SetFlip(float motionX)
	{
		if (motionX > 0f)
		{
			tsf_FlipRoot.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (motionX < 0f)
		{
			tsf_FlipRoot.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public unsafe override void Update()
	{
		if (base.CurrentHPRatio < secondStageRatio && !inSecondStage && state == MonsterState.Idle)
		{
			state = MonsterState.SecondStageFly;
		}
		if (state == MonsterState.FlyBomb || state == MonsterState.FlyBombPrepare || state == MonsterState.Missile || state == MonsterState.MissileBefore || state == MonsterState.FlyDashBefore || state == MonsterState.FlyDash)
		{
			camFocusData.extraFocusSize = CamController.Inst.GetSpecificFocusSize((ToPointDistance(PlayerMgr.Inst.PlayerPoint) + 6f) * ToPointDir(PlayerMgr.Inst.PlayerPoint), 0.33f, 6f);
		}
		else
		{
			camFocusData.extraFocusSize = 0f;
		}
		float num = tsf_FootPoint.position.y - base.transform.position.y;
		tsf_HeightWave.localPosition = new Vector3(0f, Mathf.Lerp(0f, flyingFloatAmplitude, num / 2f) * ((state == MonsterState.Missile) ? 0.2f : 1f) * Mathf.Sin(wingL.nowPhase + MathF.PI / 2f));
		tsf_Model.localPosition = -new Vector3(0f, 0f, Mathf.Max(0f, num) * 0.01f);
		base.CC_Self.center = new Vector3(0f, 0f, 0f - num);
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData.ColliderPtr;
		CapsuleGeometry geometry = colliderPtr->Geometry;
		float height = base.CC_Self.height;
		Vector3 center = base.CC_Self.center;
		geometry.Vertex0 = center - new Vector3(0f, 0f, height / 2f);
		geometry.Vertex1 = center + new Vector3(0f, 0f, height / 2f);
		geometry.Radius = base.CC_Self.radius;
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData);
		UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
		if (num > immumeHeight)
		{
			componentData2.CanTouch = false;
			componentData2.unitCfg.frozenTimeRatio = 0f;
		}
		else
		{
			componentData2.CanTouch = true;
			componentData2.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
		}
		SetComponentData(componentData2);
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
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				SetWingState(Elite10_Wing.WingState.Idle);
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				if (base.SAnima.AnimationState.GetCurrent(0).Animation.Name != "Idle")
				{
					base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				}
				idleTime.RandomResult();
				SetWingState(Elite10_Wing.WingState.Idle);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTargetPlayerFirst();
				checkTargetIntervalTimer = 0f;
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			extraIdleTime -= Time.deltaTime;
			if (extraIdleTime > 0f)
			{
				stateExistTime = 0f;
			}
			if (stateExistTime > idleTime.result)
			{
				ChooseSkill();
			}
			break;
		case MonsterState.FlyDashBefore:
		{
			ref Vector3 reference17 = ref varMgr.RegV3(0);
			ref Vector3 reference18 = ref varMgr.RegV3(1);
			ref float reference19 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.elite10Roar3.PlaySE();
				SkillCount(MonsterState.FlyDashBefore);
				SetWingState(Elite10_Wing.WingState.Flap);
				base.SAnima.AnimationState.SetAnimation(0, "StartFly", loop: false);
				base.Anima.Play("DashPrepare");
				base.Anima.Update(0f);
				reference19 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				GetNearestTargetPlayerFirst();
				reference17 = Tool2D.IgnoreZPoint(base.transform.position);
				dashDir = ToPointDir(roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 5f));
				if (base.HaveTarget)
				{
					dashDir = ToTargetDir();
				}
				reference18 = reference17 - dashDir * dashBeforeDistance;
				warningStartPoint = reference18;
				SetFlip(dashDir.x);
			}
			base.transform.position = Vector3.Lerp(reference17, reference18, dashBeforeSpeedCurve.Evaluate((stateExistTime - beforeFlyTime) / (reference19 - beforeFlyTime)));
			SyncDotsPosition();
			break;
		}
		case MonsterState.FlyDash:
		{
			ref float reference9 = ref varMgr.RegFloat(0);
			ref Vector3 reference10 = ref varMgr.RegV3(0);
			ref Vector3 reference11 = ref varMgr.RegV3(1);
			ref bool reference12 = ref varMgr.RegBool(0);
			ref float reference13 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.Glide);
				base.Anima.Play("Dash");
				base.SAnima.AnimationState.SetAnimation(0, "Dash", loop: false);
				base.Anima.Update(0f);
				reference9 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				reference10 = base.transform.position;
				reference11 = warningEndPoint;
				SetFlip(dashDir.x);
			}
			if (thisDamageZone.CC.enabled && !reference12)
			{
				reference12 = true;
			}
			if (reference12)
			{
				reference13 += Time.deltaTime;
				if (reference13 > blockBulletInterval)
				{
					ShootBlockBullet();
					reference13 -= blockBulletInterval;
				}
			}
			base.transform.position = Vector3.Lerp(reference10, reference11, dashBeforeSpeedCurve.Evaluate(stateExistTime / reference9));
			SyncDotsPosition();
			break;
		}
		case MonsterState.DashBackToGround:
		{
			ref Vector3 reference23 = ref varMgr.RegV3(0);
			ref Vector3 reference24 = ref varMgr.RegV3(1);
			if (changedState)
			{
				dashBackFinish = false;
				SetWingState(Elite10_Wing.WingState.Flap);
				reference23 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
				reference24 = Vector3.zero;
				if (ToPointDistanceSqr(reference23) < 0.25f)
				{
					state = MonsterState.FlyDashGround;
					break;
				}
				base.SAnima.AnimationState.SetAnimation(0, "DashBackToGround", loop: false);
				base.Anima.Play("DashBackToGround");
			}
			reference23 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
			reference24 = Vector3.Lerp(reference24, ToPointDir(reference23) * base.MoveSpeed * Mathf.Lerp(0.01f, 1f, ToPointDistanceSqr(reference23)), SlowDownLerp * Time.deltaTime);
			SetMove(reference24);
			if (ToPointDistanceSqr(reference23) < 0.25f && reference24.magnitude / base.MoveSpeed < 0.2f && dashBackFinish)
			{
				state = MonsterState.Drop;
			}
			break;
		}
		case MonsterState.FlyDashGround:
		{
			ref float reference22 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				if (GeneralTool.ChanceResult(knockChance) && (!base.HaveTarget || (base.HaveTarget && ToTargetDistance() < knockMaxDistance)))
				{
					state = MonsterState.KnockBefore;
					break;
				}
				SetWingState(Elite10_Wing.WingState.Flap);
				base.SAnima.AnimationState.SetAnimation(0, "DashAfter", loop: false);
				base.Anima.Play("DashAfter");
				base.Anima.Update(0f);
				reference22 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.DougeBefore:
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.FlapOnce);
				base.SAnima.AnimationState.SetAnimation(0, "DougeBefore", loop: false);
				base.Anima.Play("DougeBefore");
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.SideDouge:
		{
			ref Vector3 reference14 = ref varMgr.RegV3(0);
			ref Vector3 reference15 = ref varMgr.RegV3(2);
			ref float reference16 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Douge", loop: false);
				base.Anima.Play("SideDouge");
				base.Anima.Update(0f);
				reference16 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				reference15 = base.transform.position;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					reference14 = Tool2D.GetNavMeshPoint(base.TargetPoint, dougeKeepDistance, Tool2D.GetDir(-ToTargetDir(), GeneralTool.ChanceResult(0.5f) ? (-60) : 60), 1f);
				}
				else
				{
					reference14 = Tool2D.GetNavMeshPoint(base.transform.position, dougeKeepDistance);
				}
				if (base.HaveTarget && ToPointDistance(reference14) > dougeRadius.value2)
				{
					reference14 = Tool2D.GetNavMeshPoint(base.transform.position + ToPointDir(reference14).normalized * dougeRadius.value2);
				}
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			else
			{
				SetFlip((reference14 - reference15).x);
			}
			base.transform.position = Vector3.Lerp(reference15, reference14, dougeSpeedCurve.Evaluate((stateExistTime - dougsStartTime) / (reference16 - dougsStartTime)));
			SyncDotsPosition();
			break;
		}
		case MonsterState.SideDougeAfter:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "DougeAfter", loop: false);
				SetWingState(Elite10_Wing.WingState.Idle);
				base.Anima.Play("SideDougeAfter");
			}
			break;
		case MonsterState.SideDougeAttack:
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.Idle);
				if (inSecondStage)
				{
					base.SAnima.AnimationState.SetAnimation(0, "DougeAttack2", loop: false);
					base.Anima.Play("SideDougeAttack2");
				}
				else
				{
					base.SAnima.AnimationState.SetAnimation(0, "DougeAttack", loop: false);
					base.Anima.Play("SideDougeAttack");
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.SecondStageFly:
			if (changedState)
			{
				SEMgr.Inst.elite10Transform.PlaySE();
				base.SAnima.AnimationState.SetAnimation(0, "SecondStageFly", loop: false);
				base.Anima.Play("SecondStageFly");
				originShadowScale = thisShadow.shadowScale;
				nowShadowScaleRatio = 1f;
			}
			if (secondStageFlyStarted)
			{
				nowShadowScaleRatio -= Time.deltaTime / scaleTime;
				nowShadowScaleRatio = Mathf.Max(0f, nowShadowScaleRatio);
				thisShadow.SetScale(nowShadowScaleRatio * originShadowScale);
			}
			break;
		case MonsterState.SecondStageDrop:
			if (changedState)
			{
				if (GameMgr.IsHarmony_Static)
				{
					base.SAnima.initialSkinName = "Elite10_2_HX";
				}
				else
				{
					base.SAnima.initialSkinName = "Elite10_2";
				}
				warningParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
				warningParticle.Play();
				base.SAnima.Initialize(overwrite: true);
				base.SAnima.AnimationState.SetAnimation(0, "SecondStageDrop", loop: false);
				base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				SyncDotsPosition();
				base.Anima.Play("SecondStageDrop");
				thisShadow.Show();
				nowShadowScaleRatio = 0f;
				wingL.SwitchedStage = true;
				wingR.SwitchedStage = true;
			}
			if (!secondStageDropped)
			{
				nowShadowScaleRatio += Time.deltaTime / scaleTime;
				nowShadowScaleRatio = Mathf.Min(1f, nowShadowScaleRatio);
				thisShadow.SetScale(nowShadowScaleRatio * originShadowScale);
				warningLight.pointLightOuterRadius = nowShadowScaleRatio * originWarningLightRadius;
			}
			else
			{
				nowShadowScaleRatio -= Time.deltaTime * 2f;
				nowShadowScaleRatio = Mathf.Max(0f, nowShadowScaleRatio);
				warningLight.pointLightOuterRadius = nowShadowScaleRatio * originWarningLightRadius;
			}
			break;
		case MonsterState.Blast:
			if (changedState)
			{
				GetNearestTarget();
				base.SAnima.AnimationState.SetAnimation(0, "Attack", loop: false);
				base.Anima.Play("Blast");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.KnockBefore:
			if (changedState)
			{
				SEMgr.Inst.elite10Roar4.PlaySE();
				base.SAnima.AnimationState.SetAnimation(0, "KnockBefore", loop: false);
				base.Anima.Play("KnockBefore");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Knock:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Knock", loop: false);
				base.Anima.Play("Knock");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.ContinueKnockBefore:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "ContinueKnockBefore", loop: false);
				base.Anima.Play("ContinueKnockBefore");
				SkillCount(MonsterState.ContinueKnockBefore);
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			else
			{
				GetNearestTarget();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.ContinueKnock:
		{
			ref Vector3 reference25 = ref varMgr.RegV3(0);
			ref Vector3 reference26 = ref varMgr.RegV3(1);
			ref float reference27 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.elite10Roar4.PlaySE();
				SetWingState(Elite10_Wing.WingState.FlapOnce);
				reference25 = base.transform.position;
				GetNearestTarget();
				reference26 = base.transform.position + Tool2D.GetDir() * UnityEngine.Random.Range(continueKnockKeepDistance, continueKnockMaxDistance);
				reference26 = Tool2D.GetNavMeshPoint(reference26);
				if (base.HaveTarget)
				{
					reference26 = Tool2D.GetNavMeshPoint(base.TargetPoint, continueKnockKeepDistance, -ToTargetDir(), 90f);
					if (ToPointDistance(reference26) > continueKnockMaxDistance)
					{
						reference26 = ToPointDir(reference26) * continueKnockMaxDistance + base.transform.position;
					}
				}
				base.SAnima.AnimationState.SetAnimation(0, "ContinueKnock", loop: false);
				base.Anima.Play("ContinueKnock", 0, 0f);
				base.Anima.Update(0f);
				reference27 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			else
			{
				SetFlip((reference26 - reference25).x);
			}
			base.transform.position = Vector3.Lerp(reference25, reference26, knockSpeedCurve.Evaluate(stateExistTime / reference27));
			SyncDotsPosition();
			break;
		}
		case MonsterState.ContinueKnockAfter:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "ContinueKnockAfter", loop: false);
				base.Anima.Play("ContinueKnockAfter");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			else
			{
				GetNearestTarget();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.FlyBombPrepare:
		{
			ref Vector3 reference3 = ref varMgr.RegV3(0);
			ref Vector3 reference4 = ref varMgr.RegV3(1);
			if (changedState)
			{
				SEMgr.Inst.elite10Roar1.PlaySE();
				SkillCount(MonsterState.FlyBombPrepare);
				SetWingState(Elite10_Wing.WingState.Flap);
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					reference4 = Tool2D.GetDir(new Vector3((ToTargetDir().x > 0f) ? 1 : (-1), 0f, 0f), 10f);
					reference3 = base.TargetPointIgnoreZ - reference4 * bombKeepDistance;
				}
				else
				{
					reference4 = ToPointDir(roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 5f));
					reference3 = roomCenterPoint - reference4 * bombKeepDistance;
				}
				base.SAnima.AnimationState.SetAnimation(0, "StartFly", loop: false);
				if (inSecondStage)
				{
					base.Anima.Play("StartFlySecondStage");
				}
				else
				{
					base.Anima.Play("StartFly");
				}
			}
			if (stateExistTime > beforeFlyTime)
			{
				if (base.HaveTarget)
				{
					reference3 = base.TargetPointIgnoreZ - reference4 * bombKeepDistance;
				}
				SetMove(ToPointDir(reference3) * base.MoveSpeed * 1.5f * Mathf.Lerp(0f, 1f, ToPointDistanceSqr(reference3) / 9f), isFlip: false);
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
				else
				{
					SetFlip(reference4.x);
				}
			}
			break;
		}
		case MonsterState.FlyBomb:
		{
			ref float reference20 = ref varMgr.RegFloat(0);
			ref float reference21 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				extraIdleTime += flySkillPunishTime;
				reference21 = UnityEngine.Random.Range(0f, MathF.PI * 2f);
				base.SAnima.AnimationState.SetAnimation(0, "Fly", loop: false);
				base.Anima.Play("Fly");
				SetWingState(Elite10_Wing.WingState.Glide);
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					flyDiration = ToTargetDir();
				}
				else
				{
					flyDiration = ToPointDir(roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 5f));
				}
			}
			SetMove(base.MoveSpeed * flyDiration);
			reference20 += Time.deltaTime;
			if (reference20 > bombInterval)
			{
				reference20 = 0f;
				ShootBullet(reference21);
			}
			if (stateExistTime < trackBombTime && base.HaveTarget)
			{
				flyDiration = Tool2D.DirMoveTowards(flyDiration, ToTargetDir(), base.MoveSpeed * bombRotateSpeed * Time.deltaTime);
			}
			if (stateExistTime > maxBombTime)
			{
				state = MonsterState.FlyBackToGround;
			}
			break;
		}
		case MonsterState.FlyBackToGround:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref Vector3 reference2 = ref varMgr.RegV3(1);
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.Flap);
				reference = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
				reference2 = flyDiration * base.MoveSpeed;
			}
			reference = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
			reference2 = Vector3.Lerp(reference2, ToPointDir(reference) * base.MoveSpeed * Mathf.Lerp(0.01f, 1f, ToPointDistanceSqr(reference)), SlowDownLerp * Time.deltaTime);
			SetMove(reference2);
			if (ToPointDistanceSqr(reference) < 0.25f && reference2.magnitude / base.MoveSpeed < 0.2f)
			{
				if (GeneralTool.ChanceResult(knockChance) && (!base.HaveTarget || (base.HaveTarget && ToTargetDistance() < knockMaxDistance)))
				{
					state = MonsterState.Knock;
				}
				else
				{
					state = MonsterState.Drop;
				}
			}
			break;
		}
		case MonsterState.Drop:
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.FlapOnce);
				base.SAnima.AnimationState.SetAnimation(0, "Drop", loop: false);
				base.Anima.Play("Drop");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.MissileBefore:
		{
			ref Vector3 reference8 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.elite10Roar2.PlaySE();
				SetWingState(Elite10_Wing.WingState.Flap);
				SkillCount(MonsterState.MissileBefore);
				base.SAnima.AnimationState.SetAnimation(0, "StartFly", loop: false);
				base.Anima.Play("MissileStart");
				reference8 = Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius);
			}
			if (stateExistTime > beforeFlyTime)
			{
				if (base.HaveTarget)
				{
					if (ToTargetDistanceSqr() < Mathf.Pow(missileKeepDistance.value1, 2f) || ToTargetDistanceSqr() > Mathf.Pow(missileKeepDistance.value2, 2f))
					{
						reference8 = Tool2D.GetNavMeshPoint(base.TargetPoint, missileKeepDistance, -ToTargetDir(), missileKeepAngle);
					}
					else if (ToPointDistanceSqr(reference8) < 0.04f)
					{
						reference8 = Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius);
					}
					SetMove(ToPointDir(reference8) * base.MoveSpeed * missileFlySpeedRatio, isFlip: false);
					SetFlip(ToTargetDir().x);
				}
				else
				{
					if (ToPointDistanceSqr(reference8) < 0.04f)
					{
						reference8 = Tool2D.GetNavMeshPoint(base.transform.position, 5f);
					}
					SetMove(ToPointDir(reference8) * base.MoveSpeed * missileFlySpeedRatio);
				}
			}
			if (stateExistTime > MissileTime)
			{
				state = MonsterState.MissileFinish;
			}
			break;
		}
		case MonsterState.Missile:
		{
			ref float reference5 = ref varMgr.RegFloat(0);
			ref Vector3 reference6 = ref varMgr.RegV3(0);
			ref Vector3 reference7 = ref varMgr.RegV3(1);
			if (changedState)
			{
				extraIdleTime += flySkillPunishTime;
				GetNearestTarget();
				reference7 = base.transform.position + Tool2D.GetDir() * missileKeepDistance.RandomResult();
				base.SAnima.AnimationState.SetAnimation(0, "Missile", loop: true);
				base.Anima.Play("Missile");
				reference6 = Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius);
			}
			reference5 += Time.deltaTime;
			if (reference5 > missileInterval && stateExistTime < MissileTime)
			{
				reference5 -= missileInterval;
				ShootMissile(reference7);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				reference7 = base.transform.position + Mathf.Clamp(ToTargetDistance(), missileKeepDistance.value1, missileKeepDistance.value2) * ToTargetDir();
				if (ToTargetDistanceSqr() < Mathf.Pow(missileKeepDistance.value1, 2f) || ToTargetDistanceSqr() > Mathf.Pow(missileKeepDistance.value2, 2f))
				{
					reference6 = Tool2D.GetNavMeshPoint(base.TargetPoint, missileKeepDistance, new Vector3(0f - ToTargetDir().x, 0f, 0f), missileKeepAngle);
				}
				else if (ToPointDistanceSqr(reference6) < 0.04f)
				{
					reference6 = Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius);
				}
				SetMove(ToPointDir(reference6) * base.MoveSpeed * missileFlySpeedRatio, isFlip: false);
				SetFlip(ToTargetDir().x);
			}
			else
			{
				if (ToPointDistanceSqr(reference6) < 0.04f)
				{
					reference6 = Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius);
				}
				SetMove(ToPointDir(reference6) * base.MoveSpeed * missileFlySpeedRatio);
				reference7 = base.transform.position + base.CurrentMotion.normalized * missileKeepDistance.result;
			}
			if (stateExistTime > MissileTime + 0.5f)
			{
				if (GeneralTool.ChanceResult(knockChance) && (!base.HaveTarget || (base.HaveTarget && ToTargetDistance() < knockMaxDistance)))
				{
					state = MonsterState.Knock;
				}
				else
				{
					state = MonsterState.MissileFinish;
				}
			}
			break;
		}
		case MonsterState.MissileFinish:
			if (changedState)
			{
				SetWingState(Elite10_Wing.WingState.FlapOnce);
				base.SAnima.AnimationState.SetAnimation(0, "Drop", loop: false);
				base.Anima.Play("MissileFinish");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Move:
			break;
		}
	}

	public void SetWingState(Elite10_Wing.WingState wingState)
	{
		wingL.state = wingState;
		wingR.state = wingState;
	}

	public void ShootBullet(float startPhase)
	{
		bombParticle.Play();
		SEMgr.Inst.elite10Bomb.PlaySE();
		Vector3 normalized = Tool2D.GetDir(flyDiration, 90f).normalized;
		if (inSecondStage)
		{
			for (int i = 0; i < secondStageBombCount; i++)
			{
				MiniPool.GetGO("Prefabs/EF/EF_Elite10_Bullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x, 0f, 0f - (tsf_FirePoint.position - base.transform.position).y), 5f).GetComponent<Elite10_Bullet>().Initialize(targetPoint: bombPoint + Mathf.Sin((float)(i * 2) * MathF.PI / (float)secondStageBombCount + Time.timeSinceLevelLoad * 360f * secondStageBombWaveFrequency * (MathF.PI / 180f) + startPhase) * normalized * secondStageBombPathWidth / 2f, flyTime: bombSpeed);
			}
		}
		else
		{
			for (int j = 0; j < bombCount; j++)
			{
				MiniPool.GetGO("Prefabs/EF/EF_Elite10_Bullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x, 0f, 0f - (tsf_FirePoint.position - base.transform.position).y), 5f).GetComponent<Elite10_Bullet>().Initialize(targetPoint: bombPoint + Mathf.Sin((float)(j * 2) * MathF.PI / (float)bombCount + Time.timeSinceLevelLoad * 360f * bombWaveFrequency * (MathF.PI / 180f) + startPhase) * normalized * bombPathWidth / 2f, flyTime: bombSpeed);
			}
		}
	}

	public void ShootWave(float speed, float angle = 0f)
	{
		knockParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position) + new Vector3(0f, 0f, -1f);
		knockParticle.Play();
		CamController.Inst.SetShock(knockShock);
		SEMgr.Inst.monster26BigLand.PlaySE();
		Vector3 oldDir = Tool2D.GetDir();
		if (base.HaveTarget)
		{
			oldDir = ToTargetDir();
		}
		for (int i = 0; (float)i < knockWaveCount; i++)
		{
			Vector3 dir = Tool2D.GetDir(oldDir, angle + (float)(i * 360) / knockWaveCount - knockWaveAngle / 2f);
			MiniPool.GetGO("Prefabs/EF/EF_Elite10_Wave" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + dir.normalized).GetComponent<Elite10_Wave>().Initialize(dir, speed);
		}
	}

	public void ShootWaveLow(float speed, float angle = 0f)
	{
		knockParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position) + new Vector3(0f, 0f, -1f);
		knockParticle.Play();
		CamController.Inst.SetShock(knockShock);
		SEMgr.Inst.monster26BigLand.PlaySE();
		Vector3 oldDir = Tool2D.GetDir();
		if (base.HaveTarget)
		{
			oldDir = ToTargetDir();
		}
		for (int i = 0; (float)i < knockWaveCountLow; i++)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite10_Wave" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Elite10_Wave>().Initialize(Tool2D.GetDir(oldDir, angle + (float)(i * 360) / knockWaveCountLow - knockWaveAngle / 2f), speed);
		}
	}

	public void ShootArrow(float offset = 0f)
	{
		SEMgr.Inst.elite10DodgeAttack.PlaySE();
		Vector3 oldDir = Tool2D.GetDir();
		float num = arrowDistanceRange.RandomResult();
		if (base.HaveTarget)
		{
			Vector3 targetPointIgnoreZ = base.TargetPointIgnoreZ;
			if (base.HaveTarget)
			{
				if (GetComponentData<UnitProperty_Dots>(targetEntity).unitCfg.unitType == UnitType.Player)
				{
					targetPointIgnoreZ += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * missilePredictTime.RandomResult();
				}
				else
				{
					targetPointIgnoreZ += (Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion * missileSpeed * missilePredictTime.RandomResult();
				}
			}
			oldDir = ToPointDir(targetPointIgnoreZ);
			num = Mathf.Clamp(ToTargetDistance(), arrowDistanceRange.value1, arrowDistanceRange.value2) + offset;
		}
		float num2 = Mathf.Lerp(arrowAngle - arrowMinAngle, 0f, (num - arrowDistanceRange.value1) / (arrowDistanceRange.value2 - arrowDistanceRange.value1)) + arrowMinAngle;
		SetFlip(oldDir.x);
		for (int i = 0; i < arrowCount; i++)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite10_Arrow" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position, 5f).GetComponent<Elite10_Arrow>().Initialize(Tool2D.GetDir(oldDir, ((float)i + UnityEngine.Random.Range(-0.5f, 0.5f)) * num2 / (float)(arrowCount - 1) - num2 / 2f), num);
		}
	}

	public void ShootMissile(Vector3 missilePoint)
	{
		SEMgr.Inst.elite10Missile.PlaySE();
		float num = (inSecondStage ? secondStageMissileCount : missileCount);
		for (int i = 0; (float)i < num; i++)
		{
			Elite10_Bullet component = MiniPool.GetGO("Prefabs/EF/EF_Elite10_Missile" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x, 0f, 0f - (tsf_FirePoint.position - base.transform.position).y), 5f).GetComponent<Elite10_Bullet>();
			Vector3 targetPoint = missilePoint + Tool2D.GetDir() * missileRadius.RandomResult();
			if (!base.HaveTarget)
			{
				targetPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) * (float)LevelMgr.Inst.CurrentRoomCfg.theme8Width, UnityEngine.Random.Range(-0.5f, 0.5f) * (float)LevelMgr.Inst.CurrentRoomCfg.theme8Height, 0f);
			}
			if (i >= 1)
			{
				targetPoint = missilePoint + Tool2D.GetDir() * missileRadius1.RandomResult();
			}
			if (base.HaveTarget)
			{
				if (GetComponentData<UnitProperty_Dots>(targetEntity).unitCfg.unitType == UnitType.Player)
				{
					targetPoint += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * missileSpeed * missilePredictTime.RandomResult();
				}
				else
				{
					targetPoint += (Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion * missileSpeed * missilePredictTime.RandomResult();
				}
			}
			component.Initialize(missileSpeed, targetPoint, isMissile: true);
		}
		if (inSecondStage)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite10_Missile" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x, 0f, 0f - (tsf_FirePoint.position - base.transform.position).y), 5f).GetComponent<Elite10_Bullet>().Initialize(targetPoint: LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) * (float)LevelMgr.Inst.CurrentRoomCfg.theme8Width, UnityEngine.Random.Range(-0.5f, 0.5f) * (float)LevelMgr.Inst.CurrentRoomCfg.theme8Height, 0f), flyTime: missileSpeed, isMissile: true);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.SecondStageFly || state == MonsterState.SecondStageDrop)
		{
			info.immuneDamage = true;
		}
	}

	private void ChooseSkill()
	{
		int weightRandom = GeneralTool.GetWeightRandom(bombChance, missileChance, dashChance, dougeChance, 0f);
		if (inSecondStage)
		{
			weightRandom = GeneralTool.GetWeightRandom(bombChance, missileChance, dashChance, dougeChance, continueKnockChance);
		}
		if (weightRandom == 0 && !SkillUsed(MonsterState.FlyBombPrepare))
		{
			SkillCount(MonsterState.FlyBombPrepare);
			state = MonsterState.FlyBombPrepare;
			return;
		}
		if (weightRandom == 1 && !SkillUsed(MonsterState.MissileBefore))
		{
			SkillCount(MonsterState.MissileBefore);
			state = MonsterState.MissileBefore;
			return;
		}
		if (weightRandom == 2 && !SkillUsed(MonsterState.FlyDashBefore))
		{
			dashTimeCounter = 0;
			state = MonsterState.FlyDashBefore;
			return;
		}
		switch (weightRandom)
		{
		case 3:
			dougeTimeCounter = 0;
			state = MonsterState.DougeBefore;
			break;
		case 4:
			if (!SkillUsed(MonsterState.ContinueKnockBefore))
			{
				SkillCount(MonsterState.ContinueKnockBefore);
				state = MonsterState.ContinueKnockBefore;
			}
			break;
		}
	}

	private void ShootBlockBullet()
	{
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; (float)i < blockBulletCount; i++)
		{
			sSPModifier.Speed = spellSpeed.RandomResult();
			sSPModifier.Duration = spellDuration.RandomResult();
			sSPModifier.Damage = spellDamage.RandomResult();
			sSPModifier.Direction = Tool2D.GetDir();
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	private void ChooseDougeSkill()
	{
		bool flag = false;
		while (!flag)
		{
			int weightRandom = GeneralTool.GetWeightRandom(dougeAttackChance, dougeDashChance, dougeBlastChance, dougeFinishChance);
			if (weightRandom == 0 && ((base.HaveTarget && ToTargetDistance() < dougeAttackRange) || !base.HaveTarget))
			{
				state = MonsterState.SideDougeAttack;
				lastDougeSkill = state;
				flag = true;
			}
			else if (weightRandom == 1 && lastDougeSkill != MonsterState.FlyDashBefore)
			{
				state = MonsterState.FlyDashBefore;
				SkillCount(MonsterState.FlyDashBefore);
				lastDougeSkill = state;
				flag = true;
			}
			else if (weightRandom == 2 && lastDougeSkill != MonsterState.Blast)
			{
				state = MonsterState.Blast;
				lastDougeSkill = state;
				flag = true;
			}
		}
	}

	private bool SkillUsed(MonsterState thisSkill)
	{
		if (skillCounter >= 2 && thisSkill == lastEndSkill)
		{
			return true;
		}
		return false;
	}

	private void SkillCount(MonsterState thisSkill)
	{
		if (lastEndSkill != thisSkill)
		{
			lastEndSkill = thisSkill;
			skillCounter = 1;
		}
		else
		{
			skillCounter++;
		}
	}

	protected override void BossDeadStay()
	{
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
		SyncDotsPosition();
		base.deadStayed = true;
		base.Anima.Play("Die");
		base.SAnima.AnimationState.SetAnimation(0, "Death", loop: false);
		base.SAnima.Update(1f);
		SEMgr.Inst.elite10Dead.PlaySE();
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		chargeParticle.Stop();
		CamController.Inst.ClearExtraCameraFocusRequirement();
		wingL.EveryInitial();
		wingR.EveryInitial();
	}

	public void SetAfterAttack(float time)
	{
		extraIdleTime = time;
		state = MonsterState.Idle;
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Blast":
		{
			chargeParticle.Stop();
			blastParticle.Play();
			Vector3 diration = (flyDiration = ToPointDir(roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 5f)));
			SEMgr.Inst.elite10Attack.PlaySE();
			GetNearestTarget();
			if (base.HaveTarget)
			{
				diration = base.TargetPointIgnoreZ - (base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x / 2f, 0f, 0f));
				diration.Normalize();
			}
			MiniPool.GetGO("Prefabs/EF/EF_Elite10_Blast" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + new Vector3((tsf_FirePoint.position - base.transform.position).x, 0f, 0f)).GetComponent<Elite10_Blast>().Initialize(diration, this);
			break;
		}
		case "BlastFinish":
			state = MonsterState.Idle;
			break;
		case "BlastPrepare":
			chargeParticle.Play();
			SEMgr.Inst.elite10Charge.PlaySE();
			break;
		case "wingDropLong":
			SetWingState(Elite10_Wing.WingState.DropLong);
			break;
		case "WingFlapOnce":
			SetWingState(Elite10_Wing.WingState.FlapOnce);
			break;
		case "DashPrepareAim":
		{
			if (base.HaveTarget)
			{
				if (targetEntity != PlayerMgr.Inst.PlayerEtt)
				{
					dashDir = ToPointDir(base.TargetPoint + 0.3f * (Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion);
				}
				else
				{
					dashDir = ToPointDir(base.TargetPoint + 0.3f * PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
				}
			}
			dashDir = Tool2D.GetDir(dashDir, UnityEngine.Random.Range(-8f, 8f));
			warningEndPoint = warningStartPoint + dashDir * dashDistance;
			for (int i = 0; i < warningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(warningStartPoint, warningEndPoint, (float)i / (float)(warningLine.positionCount - 1));
				warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			warningLine.enabled = true;
			break;
		}
		case "DashPrepareFinish":
			state = MonsterState.FlyDash;
			break;
		case "DashFinish":
			warningLine.enabled = false;
			if (GeneralTool.ChanceResult(dashAgainChance) && dashTimeCounter < maxDashTime && !SkillUsed(MonsterState.FlyDashBefore) && inSecondStage)
			{
				dashTimeCounter++;
				state = MonsterState.FlyDashBefore;
			}
			else
			{
				state = MonsterState.DashBackToGround;
			}
			break;
		case "DashAfterFinish":
			state = MonsterState.Idle;
			break;
		case "DashBackFinish":
			dashBackFinish = true;
			break;
		case "DashDamageStart":
			thisDamageZone.Open();
			break;
		case "DashDamageFinish":
			thisDamageZone.Close();
			break;
		case "KnockBeforeFinish":
			state = MonsterState.Knock;
			break;
		case "ContinueKnockBeforeFinish":
			state = MonsterState.ContinueKnock;
			continueKnockCounter = 0;
			break;
		case "ContinueKnockFinish":
			continueKnockCounter++;
			if (continueKnockCounter >= continueKnockTime)
			{
				state = MonsterState.ContinueKnockAfter;
			}
			else
			{
				state = MonsterState.ContinueKnock;
			}
			break;
		case "ContinueKnock":
		{
			float num3 = UnityEngine.Random.Range(0, 180);
			ShootWave(8f, num3);
			ShootWave(5f, 15f + num3);
			break;
		}
		case "Knock":
		{
			float num2 = UnityEngine.Random.Range(0, 180);
			if (inSecondStage)
			{
				ShootWave(5f, 15f + num2);
			}
			ShootWave(8f, num2);
			break;
		}
		case "KnockFinish":
			state = MonsterState.Idle;
			break;
		case "DougeBeforeFinish":
			state = MonsterState.SideDouge;
			break;
		case "DougeFinish":
			if (GeneralTool.ChanceResult(dougeAgainChance) && dougeTimeCounter < maxDougeTime - 1)
			{
				dougeTimeCounter++;
				state = MonsterState.DougeBefore;
			}
			else
			{
				ChooseDougeSkill();
			}
			break;
		case "DougeAfterFinish":
			state = MonsterState.Idle;
			break;
		case "DougeAttack2":
			GetNearestTarget();
			ShootArrow(2f);
			break;
		case "DougeAttack":
			GetNearestTarget();
			if (inSecondStage)
			{
				ShootArrow(2f);
			}
			else
			{
				ShootArrow(2f);
			}
			break;
		case "DougeAttackCancel":
			if ((float)UnityEngine.Random.Range(0, 1) < dougeAttackDashChance && !SkillUsed(MonsterState.FlyDashBefore))
			{
				state = MonsterState.FlyDashBefore;
				SkillCount(MonsterState.FlyDashBefore);
			}
			break;
		case "DougeAttackFinish":
			SkillCount(MonsterState.DougeBefore);
			state = MonsterState.Idle;
			break;
		case "MissileStartFinish":
			state = MonsterState.Missile;
			break;
		case "StartFlyFinish":
			state = MonsterState.FlyBomb;
			break;
		case "Drop":
			dropParticle.Play();
			SEMgr.Inst.elite10DropSmall.PlaySE();
			break;
		case "DropFinish":
			state = MonsterState.Idle;
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "SecondStageFlyStarted":
			dropParticleLarge.Play();
			secondStageFlyStarted = true;
			break;
		case "SecondStageFlyFinish":
			state = MonsterState.SecondStageDrop;
			break;
		case "SecondStageDropDown":
		{
			secondStageDropped = true;
			SEMgr.Inst.elite10Transform2.PlaySE();
			float num = UnityEngine.Random.Range(0, 180);
			ShootWave(5f, 15f + num);
			ShootWave(8f, num);
			knockMaxDistance = secondStageKnockMaxDistance;
			arrowCount = secondStageArrowCount;
			break;
		}
		case "SecondStageDropFinish":
			state = MonsterState.Idle;
			inSecondStage = true;
			break;
		}
	}

	private void OnDisable()
	{
		CamController.Inst.SetSpecificFocusSize(0f);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		CamController.Inst.SetSpecificFocusSize(0f);
		base.AfterDead(ref info);
		UnityEngine.Object.Destroy(wingL.gameObject);
		UnityEngine.Object.Destroy(wingR.gameObject);
	}
}
