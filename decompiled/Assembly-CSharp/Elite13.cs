using System;
using System.Collections.Generic;
using Spine.Unity;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Elite13 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Lightning,
		GreatLightning,
		BounceBullet,
		SentinelBefore,
		Sentinel,
		Circle,
		CircleReposition,
		CircleAgain,
		Straight,
		StraightAfter,
		ThunderStormBefore,
		ThunderStorm,
		ThunderStormAfter,
		LightningDashPrepare,
		LightningDash,
		LightningDashAfter
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("选技能")]
	public VariableFloat ActCD;

	private float actCDTimer;

	public float bounceBulletChance;

	public float straightChainChance;

	public float circleChainChance;

	public float sentinelChance;

	public float lightningChance;

	public float dashChance;

	[Header("移动")]
	public float maxKeepDistance;

	public VariableFloat repositionRadius;

	public VariableFloat repositionTime;

	[Header("空闲")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	private float afterAttackTime;

	[Header("落雷")]
	public VariableInt lightningCount;

	public VariableFloat lightningRange;

	public float lightningPredictTime;

	public ParticleSystem lightningParticle;

	public ParticleSystem lightningParticle_H;

	public Transform tsf_Horn;

	[Header("反弹子弹追击")]
	public float bounceBulletAngle;

	public float secondStageBounceBulletAngle;

	public float bounceBulletRounds;

	public float secondStageBounceBulletRounds;

	public VariableInt bounceBulletCount;

	public VariableInt secondStageBounceBulletCount;

	private float bounceBulletRoundsCounter;

	public float bounceBulletSpeed;

	public Vector3 bounceBulletAimDir;

	private bool bounceBulletAiming;

	[Header("反弹子弹坐地")]
	public float bounceBulletKnockCount;

	[Header("延迟子弹")]
	public float sentinelTimeInterval;

	public VariableFloat sentinelBulletCount;

	public VariableFloat secondStageSentinelBulletCount;

	[Header("闪电急行")]
	public float dashBackwardDistance;

	public AnimationCurve dashBackwardCurve;

	public int maxBounceTime;

	public float maxDashTime;

	public float dashSpeedRatio;

	public float minAimDistance;

	public VariableFloat dashBulletCount;

	public VariableFloat dashBulletSpeed;

	public GameObject model;

	public Elite13_DamageZone damageZone;

	public ParticleSystem dashChargeParticle;

	public ParticleSystem dashParticle;

	public ParticleSystem dashAfterParticle;

	public ParticleSystem dashChargeParticle_H;

	public ParticleSystem dashParticle_H;

	public ParticleSystem dashAfterParticle_H;

	public LayerMask wallMask;

	public LineRenderer lr_warning;

	public LineRenderer lr_warning_H;

	public ShockParam knockWallShock;

	private Vector3 noTargetDashDir;

	private int bounceTimeCounter;

	[Header("直线闪电链")]
	public AnimationCurve straightSpeedCurve;

	public float straightMoveSpeed;

	public Vector3 straightDir;

	public bool isStraightAgain;

	private Vector3 nextStraightDir;

	[Header("环状闪电链")]
	public ShockParam lightningChainShock;

	public int circleMaxCount;

	private int circleCounter;

	public VariableFloat circleKeepDistance;

	public VariableFloat circleRepositionDistance;

	public AnimationCurve repositionCurve;

	[Header("转阶段打雷")]
	public ShockParam secondStageShock;

	public ShockParam secondStageContinueShock;

	public float thunderInterval;

	public float thunderTime;

	public ParticleSystem secondStageParticle;

	public ParticleSystem secondStageParticle_H;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	[Header("眼睛，一个不知所谓的设计")]
	public int eyeCount;

	public float eyeRadius;

	public float eyeRotateSpeed;

	public float attackEyeRotateSpeed;

	public Transform tsf_eyeRootPoint;

	public float eyeHeight;

	private List<Elite13_Eye> eyes = new List<Elite13_Eye>();

	private float eyeAngle;

	[Header("二阶段")]
	public float secondStageRatio;

	public bool inSecondStage;

	[Header("spine")]
	public List<SkeletonAnimation> SAnimas = new List<SkeletonAnimation>();

	public SkeletonAnimation SAnima_Head;

	public SkeletonAnimation SAnima_Body;

	public SpriteRenderer SR_Neck;

	public SpriteRenderer SR_Back;

	[Header("音效")]
	public AudioSource as_DashLoop;

	[Header("和谐")]
	public Sprite neck_H;

	public Sprite back_H;

	public static Elite13 Inst;

	public static MiniObjPool MiniPool;

	private MonsterState lastSkill;

	private bool thisFrameKnockedWall;

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
		as_DashLoop.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		lr_warning.enabled = false;
		if (GameMgr.IsHarmony_Static)
		{
			SAnima_Body.initialSkinName += "_HX";
			SAnima_Body.Initialize(overwrite: true);
			SAnima_Head.initialSkinName += "_HX";
			SAnima_Head.Initialize(overwrite: true);
			lightningParticle = lightningParticle_H;
			dashParticle = dashParticle_H;
			dashChargeParticle = dashChargeParticle_H;
			dashAfterParticle = dashAfterParticle_H;
			lightningParticle = lightningParticle_H;
			secondStageParticle = secondStageParticle_H;
			SR_Neck.sprite = neck_H;
			SR_Back.sprite = back_H;
			lr_warning = lr_warning_H;
			lr_warning.enabled = false;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		lr_warning.positionCount = 10;
		lr_warning.enabled = false;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCfg.width;
		roomHeight = LevelMgr.Inst.CurrentRoomCfg.height;
		ActCD.RandomResult();
		for (int i = 0; i < eyeCount; i++)
		{
			Elite13_Eye component = MiniPool.GetGO("Prefabs/EF/EF_Elite13_Eye", base.transform.position).GetComponent<Elite13_Eye>();
			component.Initialize(this);
			eyes.Add(component);
		}
		SR_Back.enabled = false;
	}

	public Vector3 GetEyePos(Elite13_Eye eye)
	{
		return base.transform.position + eyeRadius * Tool2D.GetDir(eyeAngle + (float)(360 / eyeCount * eyes.IndexOf(eye)));
	}

	public Vector3 GetBounceBulletEyePos(Elite13_Eye eye)
	{
		return base.transform.position + eyeRadius * Tool2D.GetDir(bounceBulletAimDir, -60 + 120 / eyeCount * eyes.IndexOf(eye));
	}

	private Elite13_Eye GetNearlestEye(Vector3 dir)
	{
		for (int i = 0; i < eyeCount; i++)
		{
			if (Mathf.Abs(Tool2D.GetAngleBetweenTwoDirection(Tool2D.GetDir(eyeAngle + (float)(360 / eyeCount * i)), dir)) <= 30f)
			{
				return eyes[i];
			}
		}
		return eyes[0];
	}

	private void SAnimaSet(string animaName, bool loop, float timeScale = 1f)
	{
		SAnima_Body.timeScale = timeScale;
		SAnima_Head.timeScale = timeScale;
		SAnima_Body.AnimationState.SetAnimation(0, animaName, loop);
		SAnima_Head.AnimationState.SetAnimation(0, animaName, loop);
	}

	private void LateUpdate()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	protected override void SetFlip(float motionX)
	{
		for (int i = 0; i < SAnimas.Count; i++)
		{
			SAnimas[i].transform.localScale = new Vector3(Mathf.Abs(SAnimas[i].transform.localScale.x) * (float)((!(motionX < 0f)) ? 1 : (-1)), SAnimas[i].transform.localScale.y, SAnimas[i].transform.localScale.z);
		}
	}

	public override void Update()
	{
		if (SAnima_Head.timeScale != base.SAnima.timeScale || SAnima_Body.timeScale != base.SAnima.timeScale)
		{
			SAnima_Head.timeScale = base.SAnima.timeScale;
			SAnima_Body.timeScale = base.SAnima.timeScale;
		}
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
		eyeAngle += Time.deltaTime * eyeRotateSpeed;
		if (eyeAngle > 360f)
		{
			eyeAngle -= 360f;
		}
		eyeHeight = tsf_eyeRootPoint.position.y - base.transform.position.y;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				SAnimaSet("Idle", loop: true);
			}
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
				IdleTime.RandomResult();
				SAnimaSet("Idle", loop: true);
			}
			if (stateExistTime > IdleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
				break;
			}
			SetMove(Vector3.zero);
			TryAttack();
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				SAnimaSet("Move", loop: true);
				randomMoveRadius.RandomResult();
				randomMoveTime.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
				break;
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			TryAttack();
			break;
		case MonsterState.Move:
		{
			_ = ref varMgr.RegBool(0);
			ref bool reference4 = ref varMgr.RegBool(1);
			ref float reference5 = ref varMgr.RegFloat(0);
			_ = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("Move");
				SAnimaSet("Move", loop: true);
				repositionTime.RandomResult();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, repositionRadius));
				}
			}
			if (afterAttackTime > 0f)
			{
				afterAttackTime -= Time.deltaTime;
			}
			else
			{
				TryAttack();
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
			if (ToTargetDistanceSqr() > maxKeepDistance * maxKeepDistance && !reference4)
			{
				reference4 = true;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius));
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				break;
			}
			reference5 += Time.deltaTime;
			if (navInfo.allCornerArrived || reference5 > repositionTime.result)
			{
				repositionTime.RandomResult();
				reference5 = 0f;
				reference4 = false;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius, -ToTargetDir(), 60f));
			}
			else
			{
				Debug.DrawLine(base.transform.position, navInfo.ToGoPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		}
		case MonsterState.Lightning:
		{
			ref Vector3 reference12 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.elite13Roar4.PlaySE();
				base.Anima.Play("Lightning");
				SAnimaSet("Lightning", loop: true);
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
				reference12 = (roomCenterPoint - base.transform.position).normalized;
			}
			lightningParticle.transform.position = tsf_Horn.transform.position + new Vector3(0f, 0f, -0.2f);
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * 0.33f);
			}
			else
			{
				SetMove(reference12 * base.MoveSpeed * 0.33f);
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
		}
		case MonsterState.BounceBullet:
		{
			ref Vector3 reference11 = ref varMgr.RegV3(0);
			if (changedState)
			{
				bounceBulletAimDir = Tool2D.GetDir();
				base.Anima.Play("BounceBullet");
				SAnimaSet("BounceBullet", loop: true);
				bounceBulletRoundsCounter = 0f;
				reference11 = (roomCenterPoint - base.transform.position).normalized;
				bounceBulletAiming = true;
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * 0.33f, isFlip: false);
				if (bounceBulletAiming)
				{
					bounceBulletAimDir = ToTargetDir();
				}
			}
			else
			{
				SetMove(reference11 * base.MoveSpeed * 0.33f, isFlip: false);
				SetFlip(reference11.x);
			}
			break;
		}
		case MonsterState.SentinelBefore:
			if (changedState)
			{
				base.Anima.Play("Sentinel");
				SAnimaSet("Sentinel", loop: true);
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Sentinel:
		{
			ref Vector3 reference6 = ref varMgr.RegV3(0);
			ref float reference7 = ref varMgr.RegFloat(0);
			ref float reference8 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				reference6 = Tool2D.GetDir();
				reference7 = ((!GeneralTool.ChanceResult(0.5f)) ? 1 : (-1));
				ShootSentinel();
			}
			reference8 += Time.deltaTime;
			if (reference8 > sentinelTimeInterval)
			{
				reference8 -= sentinelTimeInterval;
				ShootSentinel();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				reference6 = Tool2D.GetDir(ToTargetDir(), 80f * reference7);
			}
			else
			{
				reference6 = Tool2D.GetDir(ToPointDir(roomCenterPoint), 80f * reference7);
			}
			SetMove(ToPointDir(Tool2D.GetNavMeshPointIngoreZ(base.transform.position + reference6 * base.MoveSpeed)) * base.MoveSpeed * 2f);
			break;
		}
		case MonsterState.Straight:
		{
			ref float reference9 = ref varMgr.RegFloat(0);
			ref Vector3 reference10 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.elite13Roar2.PlaySE();
				if (!inSecondStage)
				{
					reference10 = roomCenterPoint - base.transform.position;
					reference10 = new Vector3(reference10.x / Mathf.Abs(reference10.x), reference10.y / Mathf.Abs(reference10.y), 0f);
					if (GeneralTool.ChanceResult(0.5f))
					{
						straightDir = new Vector3(reference10.x, 0f, 0f);
					}
					else
					{
						straightDir = new Vector3(0f, reference10.y, 0f);
					}
					base.Anima.Play("Straight");
				}
				else if (!isStraightAgain)
				{
					isStraightAgain = true;
					reference10 = roomCenterPoint - base.transform.position;
					reference10 = new Vector3(reference10.x / Mathf.Abs(reference10.x), reference10.y / Mathf.Abs(reference10.y), 0f);
					if (GeneralTool.ChanceResult(0.5f))
					{
						straightDir = new Vector3(reference10.x, 0f, 0f);
					}
					else
					{
						straightDir = new Vector3(0f, reference10.y, 0f);
					}
					reference10 -= straightDir;
					nextStraightDir = reference10;
					base.Anima.Play("Straight");
				}
				else
				{
					isStraightAgain = false;
					straightDir = nextStraightDir;
					base.Anima.Play("StraightAgain");
				}
				if (straightDir.x != 0f)
				{
					SAnimaSet("Straight1", loop: false);
				}
				else if (straightDir.y > 0f)
				{
					SAnimaSet("Straight2", loop: false);
				}
				else
				{
					SAnimaSet("Straight3", loop: false);
				}
				base.Anima.Update(0f);
				reference9 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			}
			SetMove(straightDir * straightMoveSpeed * straightSpeedCurve.Evaluate(stateExistTime / reference9), isFlip: false);
			SetFlip(straightDir.x);
			break;
		}
		case MonsterState.StraightAfter:
			if (changedState)
			{
				base.Anima.Play("StraightAfter");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Circle:
			if (changedState)
			{
				base.Anima.Play("Circle");
				SAnimaSet("Circle", loop: false);
				GetNearestTargetPlayerFirst();
				SEMgr.Inst.elite13Roar1.PlaySE();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.CircleReposition:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref Vector3 reference2 = ref varMgr.RegV3(1);
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("CircleReposition");
				SAnimaSet("Circle", loop: false);
				base.Anima.Update(0f);
				reference3 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				reference2 = base.transform.position;
				reference = Tool2D.GetNavMeshPoint(base.transform.position, circleRepositionDistance);
				GetNearestTargetPlayerFirst();
			}
			base.transform.position = Vector3.Lerp(reference2, reference, repositionCurve.Evaluate(stateExistTime / reference3));
			SyncDotsPosition();
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.CircleAgain:
			if (changedState)
			{
				base.Anima.Play("CircleAgain");
				GetNearestTargetPlayerFirst();
				SEMgr.Inst.elite13Roar1.PlaySE();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.ThunderStormBefore:
			if (changedState)
			{
				SEMgr.Inst.elite13DashCharge.PlaySE();
				base.Anima.Play("ThunderStormBefore");
				SAnimaSet("ThunderStormBefore", loop: false);
				dashChargeParticle.Play();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.ThunderStorm:
		{
			ref float reference13 = ref varMgr.RegFloat(0);
			ref float reference14 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				CamController.Inst.SetShock(secondStageShock);
				SEMgr.Inst.elite13ThunderStorm.PlaySE();
				SEMgr.Inst.elite13DashCharge.PlaySE();
				SEMgr.Inst.elite13Roar3.PlaySE();
				base.Anima.Play("ThunderStorm");
				SAnimaSet("ThunderStorm", loop: true);
				dashChargeParticle.Stop();
				secondStageParticle.Play();
				SR_Back.enabled = true;
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				SetComponentData(componentData3);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			if (stateExistTime < thunderTime)
			{
				reference14 += Time.deltaTime;
				if (reference14 > secondStageContinueShock.time)
				{
					CamController.Inst.SetShock(secondStageContinueShock);
					reference14 = 0f;
				}
				reference13 += Time.deltaTime;
				if (reference13 > thunderInterval)
				{
					reference13 -= thunderInterval;
					Vector3 startPoint = roomCenterPoint + new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f) * roomWidth, UnityEngine.Random.Range(-0.4f, 0.4f) * roomHeight, 0f);
					startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
					CallSingleLightning(startPoint);
				}
				SetMove(Vector3.zero, isFlip: false);
			}
			else
			{
				state = MonsterState.ThunderStormAfter;
			}
			break;
		}
		case MonsterState.ThunderStormAfter:
			if (changedState)
			{
				base.Anima.Play("ThunderStormAfter");
				SAnimaSet("ThunderStormAfter", loop: false);
				secondStageParticle.Stop();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.LightningDashPrepare:
		{
			ref float reference15 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("Dash");
				SAnimaSet("Dash", loop: false);
				SEMgr.Inst.elite13DashCharge.PlaySE();
				dashChargeParticle.Play();
				SEMgr.Inst.elite13Roar5.PlaySE();
				noTargetDashDir = Tool2D.GetDir();
				reference15 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
				lr_warning.enabled = true;
				SetWarning(base.transform.position, ToTargetDir());
			}
			else
			{
				SetFlip(noTargetDashDir.x);
				lr_warning.enabled = true;
				SetWarning(base.transform.position, noTargetDashDir);
			}
			base.transform.position += (base.HaveTarget ? ToTargetDir() : noTargetDashDir) * Time.deltaTime * dashBackwardDistance * dashBackwardCurve.Evaluate(stateExistTime / reference15);
			SyncDotsPosition();
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.LightningDash:
			if (changedState)
			{
				SEMgr.Inst.elite13DashStart.PlaySE();
				as_DashLoop.Play();
				model.SetActive(value: false);
				damageZone.Open();
				dashChargeParticle.Stop();
				dashParticle.Play();
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				componentData2.IsVelocityDeclice = false;
				componentData2.ImmuneKnockbackRegister();
				componentData2.InvincibleRegister();
				SetComponentData(componentData2);
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					base.Rigid.linearVelocity = ToTargetDir() * base.MoveSpeed * dashSpeedRatio;
				}
				else
				{
					base.Rigid.linearVelocity = noTargetDashDir * base.MoveSpeed * dashSpeedRatio;
				}
				SyncDotsVelocity();
				SetWarning(base.transform.position, base.Rigid.linearVelocity);
				lr_warning.enabled = true;
				bounceTimeCounter = 0;
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_KnockWall" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
			}
			thisFrameKnockedWall = false;
			if (base.Rigid.linearVelocity.sqrMagnitude < base.MoveSpeed * base.MoveSpeed * dashSpeedRatio * dashSpeedRatio * 0.95f || base.Rigid.linearVelocity.sqrMagnitude > base.MoveSpeed * base.MoveSpeed * dashSpeedRatio * dashSpeedRatio * 1.05f)
			{
				base.Rigid.linearVelocity = base.Rigid.linearVelocity.normalized * base.MoveSpeed * dashSpeedRatio;
				SyncDotsVelocity();
			}
			if (stateExistTime > maxDashTime || bounceTimeCounter >= maxBounceTime)
			{
				lr_warning.enabled = true;
				state = MonsterState.LightningDashAfter;
			}
			else
			{
				SetWarning(base.transform.position, base.Rigid.linearVelocity);
			}
			break;
		case MonsterState.LightningDashAfter:
			if (changedState)
			{
				SEMgr.Inst.elite13DashEnd.PlaySE();
				as_DashLoop.Stop();
				lr_warning.enabled = false;
				damageZone.Close();
				dashParticle.Stop();
				dashAfterParticle.Play();
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				componentData.IsVelocityDeclice = true;
				componentData.ImmuneKnockbackUnregister();
				componentData.InvincibleUnregister();
				SetComponentData(componentData);
				model.SetActive(value: true);
				base.Anima.Play("DashAfter");
				SAnimaSet("DashAfter", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.GreatLightning:
			break;
		}
	}

	private MonsterState RandomSkill()
	{
		int weightRandom = GeneralTool.GetWeightRandom(0f, bounceBulletChance, sentinelChance, circleChainChance, straightChainChance, 0f);
		if (inSecondStage)
		{
			weightRandom = GeneralTool.GetWeightRandom(lightningChance, bounceBulletChance, sentinelChance, circleChainChance, straightChainChance, dashChance);
		}
		return (new MonsterState[6]
		{
			MonsterState.Lightning,
			MonsterState.BounceBullet,
			MonsterState.SentinelBefore,
			MonsterState.Circle,
			MonsterState.Straight,
			MonsterState.LightningDashPrepare
		})[weightRandom];
	}

	private void ChooseSkill()
	{
		MonsterState monsterState;
		for (monsterState = RandomSkill(); monsterState == lastSkill; monsterState = RandomSkill())
		{
		}
		state = monsterState;
		lastSkill = monsterState;
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		if (state == MonsterState.ThunderStormBefore || state == MonsterState.ThunderStorm || state == MonsterState.ThunderStormAfter)
		{
			info.immuneDamage = true;
		}
	}

	private void SetAfterAttack(float AfterAttackTime = 1f)
	{
		state = MonsterState.Move;
		afterAttackTime = AfterAttackTime;
	}

	private void TryAttack()
	{
		if (base.CurrentHPRatio < secondStageRatio && !inSecondStage)
		{
			state = MonsterState.ThunderStormBefore;
			return;
		}
		actCDTimer += Time.deltaTime;
		if (actCDTimer > ActCD.result)
		{
			ChooseSkill();
			actCDTimer = 0f;
			ActCD.RandomResult();
		}
	}

	public void CallLightning()
	{
		lightningParticle.Play();
		SEMgr.Inst.elite13CallLightning.PlaySE();
		lightningCount.RandomResult();
		Vector3 vector = Tool2D.GetNavMeshPoint(roomCenterPoint + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) * roomWidth, UnityEngine.Random.Range(-0.5f, 0.5f) * roomHeight, 0f));
		GetNearestTargetPlayerFirst();
		if (base.HaveTarget)
		{
			vector = base.TargetPointIgnoreZ;
			if (targetEntity == PlayerMgr.Inst.PlayerEtt)
			{
				vector += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * lightningPredictTime;
			}
		}
		for (int i = 0; i < lightningCount.result; i++)
		{
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(vector + Tool2D.GetDir() * lightningRange.RandomResult());
			CallSingleLightning(navMeshPointIngoreZ);
		}
	}

	public void CallSingleLightning(Vector3 position)
	{
		MiniPool.GetGO("Prefabs/EF/EF_Elite13_Lightning" + (GameMgr.IsHarmony_Static ? " H" : ""), position).GetComponent<Elite13_Lightning>().Initialize();
	}

	public void ShootBounceBullet()
	{
		SEMgr.Inst.elite13Ball.PlaySE();
		if (!inSecondStage)
		{
			bounceBulletCount.RandomResult();
			for (int i = 0; i < bounceBulletCount.result; i++)
			{
				Vector3 dir = Tool2D.GetDir(bounceBulletAimDir, (0f - bounceBulletAngle) / 2f + bounceBulletAngle / (float)(bounceBulletCount.result - 1) * ((float)i + UnityEngine.Random.Range(-0.5f, 0.5f)));
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_BounceBullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + Tool2D.V3MultV3(dir, new Vector3(1f, 0.5f, 1f)) * 1.5f).GetComponent<Elite13_BounceBall>().Initialize(dir, bounceBulletSpeed);
			}
		}
		else
		{
			secondStageBounceBulletCount.RandomResult();
			for (int j = 0; j < secondStageBounceBulletCount.result; j++)
			{
				Vector3 dir2 = Tool2D.GetDir(bounceBulletAimDir, (0f - secondStageBounceBulletAngle) / 2f + secondStageBounceBulletAngle / (float)(secondStageBounceBulletCount.result - 1) * ((float)j + UnityEngine.Random.Range(-0.5f, 0.5f)));
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_BounceBullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + Tool2D.V3MultV3(dir2, new Vector3(1f, 0.5f, 1f)) * 1.5f).GetComponent<Elite13_BounceBall>().Initialize(dir2, bounceBulletSpeed);
			}
		}
	}

	private void ShootSentinel()
	{
		SEMgr.Inst.elite13Sentinel.PlaySE();
		if (!inSecondStage)
		{
			sentinelBulletCount.RandomResult();
			for (int i = 0; (float)i < sentinelBulletCount.result; i++)
			{
				Vector3 dir = Tool2D.GetDir();
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_Sentinel" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_Sentinel>().Initialize(useGivenDir: true, dir);
			}
		}
		else
		{
			secondStageSentinelBulletCount.RandomResult();
			for (int j = 0; (float)j < secondStageSentinelBulletCount.result; j++)
			{
				Vector3 dir2 = Tool2D.GetDir();
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_Sentinel" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_Sentinel>().Initialize(useGivenDir: true, dir2);
			}
		}
	}

	unsafe void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (state == MonsterState.LightningDash && GetComponentData<PhysicsCollider>(collision.GetOtherEntity(myPpt.myEntity)).ColliderPtr->GetCollisionFilter().BelongsTo == 256 && !thisFrameKnockedWall)
		{
			thisFrameKnockedWall = true;
			bounceTimeCounter++;
			MiniPool.GetGO("Prefabs/EF/EF_Elite13_KnockWall" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
			SEMgr.Inst.e1ite13DashBounce.PlaySE();
			GetNearestTargetPlayerFirst();
			Vector3 oldDir = -collision.GetNormalFrom(myPpt.myEntity);
			if (base.HaveTarget && ToTargetDistance() > minAimDistance)
			{
				base.Rigid.linearVelocity = ToTargetDir() * base.MoveSpeed * dashSpeedRatio;
				SyncDotsVelocity();
			}
			CamController.Inst.SetShock(knockWallShock);
			dashBulletCount.RandomResult();
			Vector3 position = base.transform.position;
			for (int i = 0; (float)i < dashBulletCount.result; i++)
			{
				MiniPool.GetGO("Prefabs/EF/EF_Elite13_SmallArrow" + (GameMgr.IsHarmony_Static ? " H" : ""), position).GetComponent<Elite13_Arrow>().Initialize(Tool2D.GetDir(oldDir, UnityEngine.Random.Range(-80, 80)), dashBulletSpeed.RandomResult());
			}
		}
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	private void SetWarning(Vector3 startPoint, Vector3 diration)
	{
		if (UnitDotsSyncSystem.Raycast(startPoint, diration, 999f, GameConst.Filter_Wall, out var result))
		{
			Vector3 b = Tool2D.IgnoreZPoint(result.point);
			for (int i = 0; i < lr_warning.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(startPoint, b, (float)i / (float)(lr_warning.positionCount - 1));
				lr_warning.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
		}
	}

	protected override void BossDeadStay()
	{
		SEMgr.Inst.elite13Roar3.PlaySE();
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsVelocity();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		base.Anima.Play("Die");
		for (int i = 0; i < SAnimas.Count; i++)
		{
			SAnimas[i].AnimationState.SetAnimation(0, "DashAfter", loop: false);
			SAnimas[i].Update(0.7f);
			SAnimas[i].timeScale = 0f;
		}
		myPpt.ChangeColor(componentData.baseColor);
		LateUpdate();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "DashPrepareFinish":
			state = MonsterState.LightningDash;
			break;
		case "DashAfterFinish":
			state = MonsterState.Move;
			break;
		case "Circle":
			CamController.Inst.SetShock(lightningChainShock);
			MiniPool.GetGO("Prefabs/EF/EF_Elite13_GroundImpact" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
			SEMgr.Inst.elite13Knock.PlaySE();
			MiniPool.GetGO("Prefabs/EF/EF_Elite13_ChainGenerator", base.transform.position).GetComponent<Elite13_ChainGenerator>().InitializeCircle(base.transform.position, this);
			break;
		case "CircleFinish":
			state = MonsterState.Move;
			break;
		case "CircleReposition":
			if (inSecondStage)
			{
				circleCounter = 1;
				state = MonsterState.CircleReposition;
			}
			break;
		case "CircleRepositionFinish":
			state = MonsterState.CircleAgain;
			break;
		case "CircleAgainReposition":
			circleCounter++;
			if (circleCounter < circleMaxCount)
			{
				state = MonsterState.CircleReposition;
			}
			break;
		case "CircleAgainFinish":
			SetAfterAttack(1.5f);
			break;
		case "Straight":
		{
			SEMgr.Inst.elite13StraightRelease.PlaySE();
			Vector3 startPoint = roomCenterPoint - new Vector3(roomWidth * straightDir.x * 0.45f, roomHeight * straightDir.y * 0.45f, 0f);
			MiniPool.GetGO("Prefabs/EF/EF_Elite13_ChainGeneratorStraight", base.transform.position).GetComponent<Elite13_ChainGenerator>().InitializeStraight(startPoint, straightDir, this);
			break;
		}
		case "StraightFinish":
			if (inSecondStage)
			{
				state = MonsterState.Straight;
			}
			else
			{
				state = MonsterState.StraightAfter;
			}
			break;
		case "StraightAgainFinish":
			state = MonsterState.StraightAfter;
			break;
		case "StraightAfterFinish":
			if (inSecondStage)
			{
				SetAfterAttack(1.5f);
			}
			else
			{
				state = MonsterState.Move;
			}
			break;
		case "SentinelStart":
			state = MonsterState.Sentinel;
			break;
		case "SentinelFinish":
			state = MonsterState.Move;
			break;
		case "Lightning":
			CallLightning();
			break;
		case "LightningFinish":
			SetAfterAttack();
			break;
		case "BounceBulletConfirm":
			if (base.HaveTarget)
			{
				bounceBulletAimDir = ToTargetDir();
			}
			bounceBulletAiming = false;
			break;
		case "BounceBullet":
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetFlip(ToPointDir(navInfo.ToGoPoint).x);
			}
			ShootBounceBullet();
			break;
		case "BounceBulletFinish":
			bounceBulletRoundsCounter += 1f;
			if ((bounceBulletRoundsCounter >= bounceBulletRounds && !inSecondStage) || (bounceBulletRoundsCounter >= secondStageBounceBulletRounds && inSecondStage))
			{
				SetAfterAttack(0f);
				break;
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetFlip(ToPointDir(navInfo.ToGoPoint).x);
			}
			base.Anima.Play("BounceBullet", 0, 0f);
			break;
		case "ThunderBeforeFinish":
			state = MonsterState.ThunderStorm;
			break;
		case "ThunderAfterFinish":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			inSecondStage = true;
			SetAfterAttack(0f);
			break;
		}
		}
	}
}
