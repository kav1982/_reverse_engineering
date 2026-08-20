using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

public class Boss52 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum Boss52MoveState
	{
		Idle,
		Close,
		Escape,
		Dash,
		StopMotion
	}

	public enum Boss52State
	{
		BornIdle,
		Phase1,
		PhaseSwitch,
		Phase2
	}

	public enum BossDashType
	{
		None,
		ToOpponentPoint,
		ToPlayerCorner,
		ToCenterPoint
	}

	public static Boss52 Inst;

	private static readonly int ForceStartHover = Animator.StringToHash("ForceStartHover");

	private static readonly int StartMoveClose = Animator.StringToHash("StartMoveClose");

	private static readonly int StartMoveAway = Animator.StringToHash("StartMoveAway");

	private static readonly int StartDash = Animator.StringToHash("StartDash");

	private static readonly int PointTarget = Animator.StringToHash("PointTarget");

	private static readonly int Praise = Animator.StringToHash("Praise");

	private static readonly int PraiseEnd = Animator.StringToHash("PraiseEnd");

	public float TestLevel;

	private float currentAngle;

	private float AngleMove;

	public Boss52SkillType TestSkillType;

	private Boss52SkillType currentCastingSkill;

	public bool DisableBossAction;

	public bool ForceChangeCurrentDifficult;

	private float currentDifficultLevel;

	private float currentSkillCastingTimer;

	private float skillDelayCastTimer;

	private Boss52State bossState;

	public float BornIdleDuration;

	private float idleRemainDuration;

	public ParticleSystem ChargeEffectParticle;

	public ParticleSystem ChargeEffectParticleHead;

	public ParticleSystem AfterChargeEffectParticle;

	public ParticleSystem AfterChargeBodyEffectParticle;

	public ParticleSystem AfterChargeEyeEffectParticle;

	public ParticleSystem ChargeHandEffectParticle;

	public ParticleSystem PointTargetChargeEffectParticle;

	private List<Boss52SkillType> dashToOpponentCornerSkillPool = new List<Boss52SkillType>
	{
		Boss52SkillType.DashSunH,
		Boss52SkillType.DashWideH,
		Boss52SkillType.DashBurstV,
		Boss52SkillType.DashKeepCastV
	};

	private int dashToOpponentCornerSkillPoolCastCount;

	private List<Boss52SkillType> dashToPlayerCornerSkill1Pool = new List<Boss52SkillType>
	{
		Boss52SkillType.CornerDashSectorV,
		Boss52SkillType.CornerDashWallHLerpV
	};

	private int dashToPlayerCornerSkill1PoolCastCount;

	private List<Boss52SkillType> dashToPlayerCornerSkill2Pool = new List<Boss52SkillType>
	{
		Boss52SkillType.MatrixV,
		Boss52SkillType.ThreeCrossBarScanV
	};

	private int dashToPlayerCornerSkill2PoolCastCount;

	private List<Boss52SkillType> dashToCenterSkillPool = new List<Boss52SkillType>
	{
		Boss52SkillType.RingVAndSunH,
		Boss52SkillType.CrossVAndSectorH,
		Boss52SkillType.SpeedLerpV,
		Boss52SkillType.RotateCrossRingsV,
		Boss52SkillType.RotateRingsVAndLockH,
		Boss52SkillType.RingStarVAndWebH
	};

	private int dashToCenterSkillPoolCastCount;

	private List<Boss52SkillType> generalSkillPool = new List<Boss52SkillType>
	{
		Boss52SkillType.RingRotateV,
		Boss52SkillType.InnerRingV
	};

	private int generalSkillPoolCastCount;

	[Header("Boss斥力")]
	public float PushRange;

	public float PushForce;

	public float PushInterval;

	private float pushTimer;

	[Header("场地边缘冲刺参数")]
	public float StartDashDelayInterval;

	private float startDashDelayTimer;

	private float idleDelayMoveTimer;

	public float AfterDashToOpponentCornerIdleInterval;

	public float AfterDashToCornerIdleInterval;

	public float AfterDashToCenterIdleInterval;

	private MapCornerType currentTargetCornerType;

	public float VerticleBorderWidth;

	public float HorizontalBorderWidth;

	private float enterBorderTimer;

	public Vector2 EnterBorderDashTriggerDuration;

	public Vector2 ToOpponentCornerDashCount;

	public Vector2 ToOpponentFinalPointDistanceRange;

	public float ToOpponentFinalPointScatterRange;

	private BossDashType bossDashType;

	public Vector2 ToOpponentMidPointLengthRange;

	public float VerticalDashDistanceRatio;

	private bool forceCastGeneralSkill;

	private bool useToOpponentCornerAsGeneralSkill;

	private bool isDisableUpdateBossAndPlayer;

	[Header("场地角落冲刺参数")]
	public float VerticleBorderWidth_Player;

	public float HorizontalBorderWidth_Player;

	private float enterBorderTimer_Player;

	public Vector2 EnterBorderDashTriggerDuration_Player;

	[Header("场地中心冲刺参数")]
	private float enterCenerTimer_Player;

	public Vector2 EnterCenterDashTriggerDuration_Player;

	[Header("移动参数")]
	public float NormalWalkSpeed;

	public float DashSpeed;

	private Boss52MoveState moveState;

	private List<Vector3> dashTargetPoints = new List<Vector3>();

	public float ChasePlayerDistance;

	public float EscapePlayerDistance;

	public float EnterEscapeDistanceRatio;

	public float EscapeSpeedRatio;

	[Header("面向状态")]
	public Transform ModelTransform;

	private bool isFaceRight = true;

	private bool lockCurrentFaceDirection;

	private float modelScaleX = 1f;

	public float FaceDirectionChangeDuration;

	[Header("普通技能 环形垂直无人机+封缝隙横向无人机")]
	public Vector2 RVSH_RingVerticalDroneCount;

	public Vector2 RVSH_ShootInterval;

	public Vector2 RVSH_HorizontalDroneWidth;

	private float RVSH_ShootTimer;

	public int RVSH_HorizontalDronShotDelayWave;

	public float RVSH_SkillDuration;

	private int RVSH_ShootCounter;

	[Header("小技能 冲刺 井字垂直无人机封锁")]
	public Vector2 WV_BlockWidth;

	public float WV_SpawnInterval;

	public Vector2 WV_DroneSpeed;

	public Vector2 WV_DroneGroundAreaExistDuration;

	public float WV_DroneInitialHightShiftDuration;

	public float WV_DelayCastTime;

	public float WV_SkillDuration;

	[Header("普通技能 垂直曲线开花+横向扇形激光")]
	public Vector2 CVRH_VerticalDroneCount;

	public Vector2 CVRH_RotateAngleSpeed;

	public Vector2 CVRH_ShootInterval;

	private float CVRH_ShootTimer;

	public float CVRH_SkillDuration;

	public float CVRH_StopRotateTime;

	public Vector2 CVRH_DroneGroundAreaExistDuration;

	public float CVRH_DroneInitialHightShiftDuration;

	public Vector2 CVRH_HorizontalSectorAngle;

	public Vector2 CVRH_OnesideHorizontalDroneCount;

	public Vector2 CVRH_HorizontalDroneShootDelayTime;

	private float CVRH_HorizontalDroneShootTimer;

	private bool CVRH_ShootVerticalDrone;

	public Vector2 CVRH_HorizontalDroneBaseDelayShootDuration;

	public Vector2 CVRH_HorizontalDroneWidth;

	[Header("普通技能 垂直螺旋缩圈")]
	public Vector2 RRV_VerticalDroneCount;

	public Vector2 RRV_ShootInterval;

	public Vector2 RRV_RotateReduceRadiusSpeed;

	public Vector2 RRV_RotateSpeed;

	public Vector2 RRV_RotateRadius;

	private float RRV_CastingTimer;

	public int RRV_ShootRound;

	private int RRV_CurrentShootCounter;

	private bool RRV_IsClockWiseRotate;

	public Vector2 RRV_GroundAreaExistDuration;

	public Vector2 RRV_FinalRoundSpeedRatio;

	public Vector2 RRV_FinalRoundCountRatio;

	public Vector2 RRV_AfterFinalRoundDelayEndDuration;

	[Header("小技能 冲刺后 十字扫描垂直交叉无人机")]
	public Vector2 TCBSV_BlockWidth;

	public Vector2 TCBSV_SlideAngle;

	public Vector2 TCBSV_DroneSpeed;

	public Vector2 TCBSV_DroneGroundAreaExistDuration;

	public float TCBSV_DroneInitialHightShiftDuration;

	public float TCBSV_DelayCastTime;

	public float TCBSV_SkillDuration;

	[Header("普通技能 波次垂直变速无人机")]
	public Vector2 SLV_WaveDroneCount;

	public Vector2 SLV_RingDroneCount;

	public Vector2 SLV_DroneInitialSpeed;

	public Vector2 SLV_DroneFinalSpeed;

	public Vector2 SLV_ShootInterval;

	public Vector2 SLV_WaveInterval;

	private float SLV_ShootTimer;

	public Vector2 SLV_SkillWave;

	private int SLV_ShootCount;

	private int SLV_WaveCount;

	private float SLV_BaseAngle;

	private bool SLV_IsClockWiseRotate;

	public float SLV_RotateAngleSpeed;

	public float SLV_LerpTime;

	public float SLV_BaseSpeedDownPerCount;

	public float SLV_BaseStartLerpDelayPerCount;

	public Vector2 SLV_BaseStartLerpTime;

	[Header("小技能 冲刺后 公转垂直追踪无人机 无聊的技能 已弃用")]
	public Vector2 RCV_RingDroneCount;

	public Vector2 RCV_RingDroneRotateRadius;

	public Vector2 RCV_RingDroneRotateSpeed;

	public Vector2 RCV_RingDroneTrailDuration;

	public float RCV_RingDroneRadiusChangeSpeed;

	public Vector2 RCV_CoreSpawnCount;

	public Vector2 RCV_CoreScatter;

	public Vector2 RCV_CoreSpeed;

	public Vector2 RCV_CoreChasePower;

	public Vector2 RCV_CoreDecaySpeed;

	public Vector2 RCV_CoreDuration;

	public float RCV_SkillDuration;

	private bool RCV_IsCastFinish;

	public float RCV_CoreDelayMoveDuration;

	private bool RCV_IsClockWiseRotate;

	[Header("普通技能 公转垂直交错无人机")]
	public Vector2 RCRV_DroneAttackRadius;

	public Vector2 RCRV_DroneWidth;

	public Vector2 RCRV_DroneRotateAngleSpeed;

	public Vector2 RCRV_DroneInitialScatter;

	public Vector2 RCRV_DroneDelayStartTimer;

	public Vector2 RCRV_DroneInitialLerpTime;

	public Vector2 RCRV_DroneLifeDuration;

	public Vector2 RCRV_DroneTrailLength;

	public float RCRV_DroneTrailMaxWidthRatio;

	private bool RCRV_IsCastFinish;

	public float RCRV_SkillDuration;

	[Header("普通技能 分边公转垂直无人机+ 定点横向无人机")]
	public Vector2 RRVLH_DroneAttackRadius;

	public Vector2 RRVLH_DroneWidth;

	public Vector2 RRVLH_DroneRotateAngleSpeed;

	public Vector2 RRVLH_DroneTrailDuration;

	public Vector2 RRVLH_DroneInitialScatter;

	public Vector2 RRVLH_DroneDelayStartTimer;

	public Vector2 RRVLH_DroneInitialLerpTime;

	public Vector2 RRVLH_DroneLifeDuration;

	private bool RRVLH_IsCastFinish;

	public float RRVLH_SkillDuration;

	private bool RRVLH_IsClockWiseRotate;

	public Vector2 RRVLH_HorizontalDroneSpawnInterval;

	private float RRVLH_HorizontalDroneSpawnTimer = -0.5f;

	public Vector2 RRLVH_HorizontalDroneRotateAnglePerShoot;

	private float RRLVH_HorizontalDroneCurrentAngle;

	public Vector2 RRLVH_HorizontalDroneShootCount;

	public Vector2 RRLVH_TimerToHorizontalDroneRadiusRatio;

	[Header("小技能 冲刺时 拼凑粗激光")]
	public Vector2 DWH_LaserWidth;

	public Vector2 DWH_SideDroneCount;

	public Vector2 DWH_BaseDelayShootDuration;

	public Vector2 DWH_SideDroneBonusDelayShootDuration;

	public float DWH_SideDroneAngle;

	public int DWH_FinalRoundBonusLaser;

	public Vector2 DWH_InitFlySpeed;

	[Header("小技能 冲刺时 连续发射垂直激光")]
	public Vector2 DKCV_CastInterval;

	private float DKCV_CastTimer;

	private float DKCV_BaseAngle;

	public float DKCV_RotateAngleSpeed;

	public float DKCV_DroneDelayMoveDuration;

	public float DKCV_DroneMotionLerpDuration;

	public Vector2 DKCV_DroneRadius;

	public Vector2 DKCV_DroneMoveSpeed;

	public float DKCV_LaserDamageRadius;

	public float DKCV_DroneHeightShiftDuration;

	public Vector2 DKCV_RingDroneCount;

	private bool DKCV_IsCenterCast;

	public Vector2 DKCV_CenterCastInterval;

	[Header("小技能 冲刺时 太阳状散射激光")]
	public Vector2 DSH_DroneCount;

	public Vector2 DSH_DroneWidth;

	public Vector2 DSH_DroneDelayShootTime;

	public Vector2 DSH_DroneBonusDelayShootTimePerWave;

	private int DSH_WaveCount;

	[Header("小技能 冲刺时 太阳状散射垂直激光")]
	public Vector2 DBV_DroneCount;

	public Vector2 DBV_RingCount;

	public float DBV_RingSpace;

	public Vector2 DBV_BaseSpeed;

	public Vector2 DBV_InitialBonusSpeedPerRing;

	public float DBV_BonusSpeedDecayDuration;

	public float DBV_HeightShiftDuration;

	public float DBV_DroneDelayMoveDuration;

	public float DBV_DroneMotionLerpDuration;

	private bool onTargetDashPoint;

	private bool finishedFinalDashPoint;

	[Header("小技能 冲刺后 扇形垂直激光和收束横向激光")]
	public Vector2 CDSV_ShootInterval;

	public Vector2 CDSV_TrailDuration;

	public Vector2 CDSV_DroneSpeed;

	public float CDSV_SkillDuration;

	public float CDSV_MaxShiftAngle;

	public Vector2 CDSV_AngleMoveSpeed;

	private float CDSV_CurrentAngle;

	private float CDSV_ShootTimer;

	private bool CDSV_IncreaseShootAngle;

	public float CDSV_DelayCastTime;

	private float CDSV_DelayCastTimer;

	public Vector2 CDSV_HorizontalDroneSpawnInterval;

	private float CDSV_HorizontalDroneSpawnTimer;

	public Vector2 CDSV_HorizontalDroneAngleSpeed;

	private float CDSV_HorizontalDroneCurrentAngle;

	public float CDSV_HorizontalDroneInitialAngle;

	public Vector2 CDSV_HorizontalCastDuration;

	private float CDSV_HorizontalCastTimer;

	[Header("小技能 冲刺后 压缩横向激光+变速垂直激光")]
	public Vector2 RCV_HorizontalShootInterval;

	private float RCV_HorizontalShootTimer;

	public Vector2 RCV_HorizontalInitialDistance;

	private float RCV_HorizontalToCenterDistance;

	public float RCV_HorizontalLerpDuration;

	public Vector2 RCV_HorizontalDelayShootDuration;

	public Vector2 RCV_HorizontalTargetToCenterDistance;

	public Vector2 CDWHLV_VDSpawnInterval;

	private float CDWHLV_VDSpawnTimer;

	public Vector2 CDWHLV_VDFinalSpeed;

	public Vector2 CDWHLV_SkillDuration;

	public Vector2 CDWHLV_MaxScatter;

	private float CDWHLV_CurrentAngle;

	public float CDWHLV_Radius;

	public float CDWHLV_AngleMoveSpeed;

	public Vector2 CDWHLV_RingDroneCount;

	[Header("普通技能 回旋垂直激光 + 网状横向激光")]
	public Vector2 SVWH_VDroneShootCount;

	public Vector2 SVWH_VAngleSpeed;

	public Vector2 SVWH_VMoveSpeed;

	public Vector2 SVWH_VTrailDuration;

	private float SVWH_VShootTimer;

	private List<Boss52VerticalDrone> SVWH_VDronesList = new List<Boss52VerticalDrone>();

	public Vector2 SVWH_HShootInterval;

	private float SVWH_HShootTimer;

	public Vector2 SVWH_HDroneWidth;

	public Vector2 SVWH_HDroneShootDelayDuration;

	public int SVWH_HDroneNextLockIndex;

	private bool SVWH_IsClockWiseRotate;

	private bool SVWH_ReadyToShootNewWave;

	[Header("小技能 内圈垂直激光")]
	public Vector2 IRV_RingDroneCount;

	public Vector2 IRV_AngleMoveSpeed;

	public Vector2 IRV_RingRadius;

	public Vector2 IRV_MoveSpeed;

	public Vector2 IRV_TrailDuration;

	public Vector2 IRV_DroneDuration;

	public float IRV_DroneDelayMoveDuration;

	public float IRV_DroneLerpDuration;

	public Vector2 IRV_DroneShootInterval;

	private float IRV_CurrentBaseAngle;

	private float IRV_ShootTimer;

	private Vector3 IRV_CenterPoint;

	private bool IRV_IsLockPoint;

	private bool IRV_IsClockWise;

	public Vector2 IRV_OneWaveDuration;

	public Vector2 IRV_WaveInterval;

	public Vector2 IRV_WaveCount;

	private float IRV_WaveTimer;

	private float IRV_WaveShootRemainTimer;

	private int IRV_WaveCounter;

	public float IRV_CenterPointMinWidth;

	public Entity thisEntity { get; set; }

	protected override void BossDeadStay()
	{
		base.BossDeadStay();
		Boss52VerticalDrone.IsBoss52Dead = true;
		Boss52HorizontalDrone.IsBoss52Dead = true;
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		BossEnterState(Boss52State.BornIdle);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		Boss52VerticalDrone.IsBoss52Dead = false;
		Boss52HorizontalDrone.IsBoss52Dead = false;
		if (ForceChangeCurrentDifficult)
		{
			currentDifficultLevel = TestLevel;
		}
		if (TestSkillType != 0)
		{
			CastTargetSkill(TestSkillType);
			MoveEnterState(Boss52MoveState.StopMotion);
		}
		currentDifficultLevel = Mathf.Clamp((float)(BattleMgr.Inst.EndlessCurrentLevel - 6) / 6f / 2f, 0f, 1f);
		if (ForceChangeCurrentDifficult)
		{
			currentDifficultLevel = TestLevel;
		}
	}

	private void Start()
	{
		currentAngle = 0f;
	}

	public override void Update()
	{
		base.Update();
		UpdateBossState();
	}

	private void UpdateBossState()
	{
		switch (bossState)
		{
		case Boss52State.BornIdle:
			idleRemainDuration -= Time.deltaTime;
			FaceToPlayer();
			UpdateFaceDirection();
			if (idleRemainDuration <= 0f)
			{
				BossEnterState(Boss52State.Phase1);
			}
			break;
		case Boss52State.Phase1:
		case Boss52State.Phase2:
			UpdateCurrentSkillState();
			UpdateMoveState();
			UpdateFaceDirection();
			UpdatePushEffect();
			break;
		case Boss52State.PhaseSwitch:
			break;
		}
	}

	private void BossEnterState(Boss52State state)
	{
		bossState = state;
		switch (state)
		{
		case Boss52State.BornIdle:
			idleRemainDuration = BornIdleDuration;
			base.Anima.SetTrigger(ForceStartHover);
			startDashDelayTimer = 0f;
			dashTargetPoints.Clear();
			dashToOpponentCornerSkillPool = GeneralTool.ListShuffle(dashToOpponentCornerSkillPool);
			dashToPlayerCornerSkill1Pool = GeneralTool.ListShuffle(dashToPlayerCornerSkill1Pool);
			dashToPlayerCornerSkill2Pool = GeneralTool.ListShuffle(dashToPlayerCornerSkill2Pool);
			dashToCenterSkillPool = GeneralTool.ListShuffle(dashToCenterSkillPool);
			generalSkillPool = GeneralTool.ListShuffle(generalSkillPool);
			dashToOpponentCornerSkillPoolCastCount = 0;
			dashToPlayerCornerSkill1PoolCastCount = 0;
			dashToPlayerCornerSkill2PoolCastCount = 0;
			currentTargetCornerType = MapCornerType.MiddleCenter;
			CDSV_CurrentAngle = 0f;
			CDSV_ShootTimer = 0f;
			CDSV_HorizontalDroneSpawnTimer = 0f;
			CDSV_HorizontalDroneCurrentAngle = 0f;
			CDSV_HorizontalCastTimer = 0f;
			moveState = Boss52MoveState.Idle;
			currentCastingSkill = Boss52SkillType.None;
			currentSkillCastingTimer = 0f;
			currentDifficultLevel = 0f;
			RVSH_ShootTimer = 0f;
			RVSH_ShootCounter = 0;
			idleRemainDuration = 0f;
			CVRH_HorizontalDroneShootTimer = 0f;
			CVRH_ShootTimer = GetCurrentDifficultPatternValue(CVRH_ShootInterval) - 0.5f;
			CVRH_ShootVerticalDrone = false;
			RRV_CastingTimer = 0f;
			RRV_CurrentShootCounter = 0;
			SLV_ShootTimer = 0f;
			SLV_WaveCount = 0;
			SLV_BaseAngle = 0f;
			SLV_ShootCount = 0;
			RCV_IsCastFinish = false;
			RCRV_IsCastFinish = false;
			RRVLH_HorizontalDroneSpawnTimer = -0.5f;
			isFaceRight = true;
			lockCurrentFaceDirection = false;
			modelScaleX = ModelTransform.localScale.x;
			DKCV_CastTimer = 0f;
			DSH_WaveCount = 0;
			isDisableUpdateBossAndPlayer = false;
			CDWHLV_CurrentAngle = 0f;
			break;
		case Boss52State.Phase1:
		case Boss52State.PhaseSwitch:
		case Boss52State.Phase2:
			break;
		}
	}

	private void UpdateMoveState()
	{
		if (DisableBossAction)
		{
			return;
		}
		switch (moveState)
		{
		case Boss52MoveState.Idle:
			SetMove(Vector3.zero);
			if (idleDelayMoveTimer > 0f)
			{
				idleDelayMoveTimer -= Time.deltaTime;
			}
			else
			{
				if (GetToPlayerDistance() >= ChasePlayerDistance)
				{
					MoveEnterState(Boss52MoveState.Close);
				}
				else if (GetToPlayerDistance() <= EscapePlayerDistance * EnterEscapeDistanceRatio)
				{
					MoveEnterState(Boss52MoveState.Escape);
				}
				UpdatePlayerAndBossPositionTimer();
			}
			FaceToPlayer();
			break;
		case Boss52MoveState.Close:
			SetMove(GetToPlayerDirection() * NormalWalkSpeed);
			if (GetToPlayerDistance() <= ChasePlayerDistance)
			{
				MoveEnterState(Boss52MoveState.Idle);
			}
			FaceToPlayer();
			UpdatePlayerAndBossPositionTimer();
			break;
		case Boss52MoveState.Escape:
			SetMove(-GetToPlayerDirection() * NormalWalkSpeed * EscapeSpeedRatio);
			if (GetToPlayerDistance() >= EscapePlayerDistance)
			{
				MoveEnterState(Boss52MoveState.Idle);
			}
			FaceToPlayer();
			UpdatePlayerAndBossPositionTimer();
			break;
		case Boss52MoveState.Dash:
			if (startDashDelayTimer > 0f)
			{
				startDashDelayTimer -= Time.deltaTime;
				if (startDashDelayTimer <= 0f)
				{
					PlayDashSE();
				}
			}
			else if (!finishedFinalDashPoint)
			{
				Vector3 vector = dashTargetPoints[0] - base.transform.position;
				float num = DashSpeed * Time.deltaTime;
				float num2 = Tool2D.IgnoreZDistance(base.transform.position, dashTargetPoints[0]);
				isFaceRight = dashTargetPoints[0].x <= base.transform.position.x;
				if (num2 < num)
				{
					base.transform.position = dashTargetPoints[0];
					dashTargetPoints.RemoveAt(0);
					onTargetDashPoint = true;
					if (dashTargetPoints.Count <= 0)
					{
						finishedFinalDashPoint = true;
					}
					else
					{
						PlayDashSE();
					}
					if (bossDashType == BossDashType.ToOpponentPoint)
					{
						SEMgr.Inst.boss52DashCast.PlaySE();
					}
				}
				else
				{
					SetMove(vector * DashSpeed, instantLerp: true);
				}
				SpawnDashShadow();
			}
			else
			{
				if (bossDashType == BossDashType.ToPlayerCorner)
				{
					MoveEnterState(Boss52MoveState.StopMotion);
					CastDashToPlayerCornerSkill();
				}
				else if (bossDashType == BossDashType.ToCenterPoint)
				{
					MoveEnterState(Boss52MoveState.StopMotion);
					CastDashToCenterSkill();
				}
				else
				{
					MoveEnterState(Boss52MoveState.Idle);
				}
				finishedFinalDashPoint = false;
				bossDashType = BossDashType.None;
				SetMove(Vector3.zero, instantLerp: true);
			}
			break;
		case Boss52MoveState.StopMotion:
			FaceToPlayer();
			break;
		}
		if (moveState == Boss52MoveState.StopMotion || !(idleDelayMoveTimer <= 0f) || isDisableUpdateBossAndPlayer)
		{
			return;
		}
		if (forceCastGeneralSkill && (enterCenerTimer_Player >= GetCurrentDifficultPatternValue(EnterCenterDashTriggerDuration_Player) || enterBorderTimer >= GetCurrentDifficultPatternValue(EnterBorderDashTriggerDuration) || enterBorderTimer_Player >= GetCurrentDifficultPatternValue(EnterBorderDashTriggerDuration_Player)))
		{
			if (useToOpponentCornerAsGeneralSkill)
			{
				bossDashType = BossDashType.ToOpponentPoint;
				MoveEnterState(Boss52MoveState.Dash);
			}
			else
			{
				CastGeneralSkill();
			}
			FinishGeneralSkill();
		}
		else if (enterBorderTimer >= GetCurrentDifficultPatternValue(EnterBorderDashTriggerDuration))
		{
			bossDashType = BossDashType.ToOpponentPoint;
			MoveEnterState(Boss52MoveState.Dash);
		}
		else if (enterCenerTimer_Player >= GetCurrentDifficultPatternValue(EnterCenterDashTriggerDuration_Player))
		{
			bossDashType = BossDashType.ToCenterPoint;
			MoveEnterState(Boss52MoveState.Dash);
			NextActionForceCastGeneralSkill();
		}
		else if (enterBorderTimer_Player >= GetCurrentDifficultPatternValue(EnterBorderDashTriggerDuration_Player))
		{
			bossDashType = BossDashType.ToPlayerCorner;
			MoveEnterState(Boss52MoveState.Dash);
		}
	}

	private void NextActionForceCastGeneralSkill()
	{
		forceCastGeneralSkill = true;
		useToOpponentCornerAsGeneralSkill = !useToOpponentCornerAsGeneralSkill;
	}

	private void FinishGeneralSkill()
	{
		forceCastGeneralSkill = false;
	}

	public void SetMove(Vector3 motion, bool instantLerp = false, float motionLerp = 0f)
	{
		float num = ((motionLerp > 0f) ? motionLerp : moveLerp);
		base.CurrentMotion = Tool2D.IgnoreZPoint(base.CurrentMotion);
		base.CurrentMotion = Vector3.Lerp(base.CurrentMotion, motion, instantLerp ? 1f : (num * Time.deltaTime));
	}

	private void UpdatePlayerAndBossPositionTimer()
	{
		if (!isDisableUpdateBossAndPlayer)
		{
			Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
			if (Mathf.Abs(playerPoint.x - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / 2f - HorizontalBorderWidth_Player || Mathf.Abs(playerPoint.y - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f - VerticleBorderWidth_Player)
			{
				enterBorderTimer_Player += Time.deltaTime;
			}
			else
			{
				enterCenerTimer_Player += Time.deltaTime;
			}
			if (Mathf.Abs(base.transform.position.x - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / 2f - HorizontalBorderWidth || Mathf.Abs(base.transform.position.y - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f - VerticleBorderWidth)
			{
				enterBorderTimer += Time.deltaTime;
			}
		}
	}

	private void FaceToPlayer()
	{
		isFaceRight = PlayerMgr.Inst.PlayerPoint.x >= base.transform.position.x;
	}

	private void MoveEnterState(Boss52MoveState state)
	{
		startDashDelayTimer = 999f;
		moveState = state;
		switch (moveState)
		{
		case Boss52MoveState.Idle:
			base.Anima.SetTrigger(ForceStartHover);
			break;
		case Boss52MoveState.Close:
			base.Anima.SetTrigger(StartMoveClose);
			break;
		case Boss52MoveState.Escape:
			base.Anima.SetTrigger(StartMoveAway);
			break;
		case Boss52MoveState.Dash:
			enterBorderTimer = 0f;
			enterBorderTimer_Player = 0f;
			enterCenerTimer_Player = 0f;
			base.Anima.SetTrigger(StartDash);
			startDashDelayTimer = StartDashDelayInterval;
			SetMove(Vector3.zero, instantLerp: true, 4f);
			onTargetDashPoint = false;
			switch (bossDashType)
			{
			case BossDashType.ToOpponentPoint:
			{
				Vector3 normalized = (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * 0.05f - base.transform.position).normalized;
				Vector3 vector = Tool2D.GetDir(normalized, UnityEngine.Random.Range(0f - ToOpponentFinalPointScatterRange, ToOpponentFinalPointScatterRange)) * UnityEngine.Random.Range(ToOpponentFinalPointDistanceRange.x, ToOpponentFinalPointDistanceRange.y);
				vector.y *= VerticalDashDistanceRatio;
				Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + vector);
				float num = Tool2D.IgnoreZDistance(base.transform.position, navMeshPointIngoreZ);
				int num2 = Mathf.RoundToInt(GetCurrentDifficultPatternValue(ToOpponentCornerDashCount));
				float num3 = ((num2 > 1) ? (1f / (float)num2) : 1f);
				int num4 = ((UnityEngine.Random.Range(0f, 1f) >= 0.5f) ? 1 : (-1));
				for (int i = 0; i < num2; i++)
				{
					if (i == num2 - 1)
					{
						dashTargetPoints.Add(navMeshPointIngoreZ);
						continue;
					}
					num4 *= -1;
					float num5 = ((num2 == 1) ? 1f : (num3 * ((float)(i + 1) + UnityEngine.Random.Range(-0.2f, 0.2f))));
					Vector3 startPoint = base.transform.position + normalized * num * num5 + Tool2D.GetDir(normalized, 90 * num4) * UnityEngine.Random.Range(ToOpponentMidPointLengthRange.x, ToOpponentMidPointLengthRange.y);
					dashTargetPoints.Add(Tool2D.GetNavMeshPointIngoreZ(startPoint));
				}
				idleDelayMoveTimer = AfterDashToOpponentCornerIdleInterval;
				CastDashToOpponentCornerSkill();
				break;
			}
			case BossDashType.ToPlayerCorner:
			{
				currentTargetCornerType = GetPlayerNearestCorner();
				idleDelayMoveTimer = AfterDashToCornerIdleInterval;
				Vector3 roomCornerPoint = GetRoomCornerPoint(currentTargetCornerType);
				dashTargetPoints.Add(Tool2D.GetNavMeshPointIngoreZ(roomCornerPoint));
				break;
			}
			case BossDashType.ToCenterPoint:
				idleDelayMoveTimer = AfterDashToCenterIdleInterval;
				dashTargetPoints.Add(Tool2D.GetNavMeshPointIngoreZ(GetRoomCornerPoint(MapCornerType.MiddleCenter)));
				break;
			}
			StartCoroutine(SpawnDashToTargetAlertMark(0.1f, 0.1f));
			break;
		case Boss52MoveState.StopMotion:
			break;
		}
	}

	private IEnumerator SpawnDashToTargetAlertMark(float initSpawnDelay, float laterSpawnDelay)
	{
		yield return new WaitForSeconds(initSpawnDelay);
		for (int i = 0; i < dashTargetPoints.Count; i++)
		{
			SpawnAlertMark(dashTargetPoints[i], 1f);
			yield return new WaitForSeconds(laterSpawnDelay);
		}
	}

	private MapCornerType GetPlayerNearestCorner()
	{
		Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
		if (playerPoint.x >= LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
		{
			if (playerPoint.y >= LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y)
			{
				return MapCornerType.UpperRight;
			}
			return MapCornerType.LowerRight;
		}
		if (playerPoint.y >= LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y)
		{
			return MapCornerType.UpperLeft;
		}
		return MapCornerType.LowerLeft;
	}

	private void CastDashToOpponentCornerSkill()
	{
		CastTargetSkill(dashToOpponentCornerSkillPool[dashToOpponentCornerSkillPoolCastCount]);
		dashToOpponentCornerSkillPoolCastCount++;
		if (dashToOpponentCornerSkillPoolCastCount >= dashToOpponentCornerSkillPool.Count)
		{
			dashToOpponentCornerSkillPoolCastCount = 0;
			dashToOpponentCornerSkillPool = GeneralTool.ListShuffle(dashToOpponentCornerSkillPool);
		}
	}

	private void CastDashToCenterSkill()
	{
		CastTargetSkill(dashToCenterSkillPool[dashToCenterSkillPoolCastCount]);
		dashToCenterSkillPoolCastCount++;
		if (dashToCenterSkillPoolCastCount >= dashToCenterSkillPool.Count)
		{
			dashToCenterSkillPoolCastCount = 0;
			dashToCenterSkillPool = GeneralTool.ListShuffle(dashToCenterSkillPool);
		}
	}

	private void CastGeneralSkill()
	{
		CastTargetSkill(generalSkillPool[generalSkillPoolCastCount]);
		generalSkillPoolCastCount++;
		if (generalSkillPoolCastCount >= generalSkillPool.Count)
		{
			generalSkillPoolCastCount = 0;
			generalSkillPool = GeneralTool.ListShuffle(generalSkillPool);
		}
	}

	private Vector3 GetCornerToCenterDirection()
	{
		return currentTargetCornerType switch
		{
			MapCornerType.UpperLeft => new Vector3(1f, -1f, 0f).normalized, 
			MapCornerType.UpperRight => new Vector3(-1f, -1f, 0f).normalized, 
			MapCornerType.LowerLeft => new Vector3(1f, 1f, 0f).normalized, 
			MapCornerType.LowerRight => new Vector3(-1f, 1f, 0f).normalized, 
			_ => Vector3.zero, 
		};
	}

	private void CastDashToPlayerCornerSkill()
	{
		base.Anima.SetTrigger(PointTarget);
		CastTargetSkill(dashToPlayerCornerSkill1Pool[dashToPlayerCornerSkill1PoolCastCount]);
		dashToPlayerCornerSkill1PoolCastCount++;
		if (dashToPlayerCornerSkill1PoolCastCount >= dashToPlayerCornerSkill1Pool.Count)
		{
			dashToPlayerCornerSkill1PoolCastCount = 0;
			dashToPlayerCornerSkill1Pool = GeneralTool.ListShuffle(dashToPlayerCornerSkill1Pool);
		}
	}

	private void CastDashToPlayerCornerSkill2()
	{
		base.Anima.SetTrigger(Praise);
		CastTargetSkill(dashToPlayerCornerSkill2Pool[dashToPlayerCornerSkill2PoolCastCount]);
		dashToPlayerCornerSkill2PoolCastCount++;
		if (dashToPlayerCornerSkill2PoolCastCount >= dashToPlayerCornerSkill2Pool.Count)
		{
			dashToPlayerCornerSkill2PoolCastCount = 0;
			dashToPlayerCornerSkill2Pool = GeneralTool.ListShuffle(dashToPlayerCornerSkill2Pool);
		}
	}

	private void UpdatePushEffect()
	{
		pushTimer += Time.deltaTime;
		if (pushTimer < PushInterval)
		{
			return;
		}
		pushTimer = 0f;
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position.IgnoreZ(), PushRange, GameConst.Filter_MonsterAoeNoSpell, list);
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			if (entityManager.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
			{
				LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(distanceHitResult.entity);
				Vector3 normalized = ((Vector3)componentData.Position - base.transform.position).IgnoreZ().normalized;
				UnitProperty_Dots componentData2 = entityManager.GetComponentData<UnitProperty_Dots>(distanceHitResult.entity);
				float num = Tool2D.IgnoreZDistance(base.transform.position, componentData.Position);
				float num2 = Mathf.Pow(1f - num / PushRange, 2f);
				componentData2.TakeKnockback(normalized * PushForce * PushInterval * num2);
				entityManager.SetComponentData(distanceHitResult.entity, componentData2);
			}
		}
	}

	private void UpdateFaceDirection(bool instantLerp = false)
	{
		if (!lockCurrentFaceDirection)
		{
			float num = (isFaceRight ? Mathf.Abs(modelScaleX) : (0f - Mathf.Abs(modelScaleX)));
			if (instantLerp)
			{
				num = Mathf.Lerp(base.transform.localScale.x, num, 10f * Time.deltaTime);
				ModelTransform.localScale = new Vector3(num, ModelTransform.localScale.y, ModelTransform.localScale.z);
			}
			else
			{
				ModelTransform.DOScaleX(num, FaceDirectionChangeDuration);
			}
		}
	}

	private void UpdateCurrentSkillState()
	{
		if (currentCastingSkill == Boss52SkillType.None)
		{
			return;
		}
		if (skillDelayCastTimer > 0f)
		{
			skillDelayCastTimer -= Time.deltaTime;
			return;
		}
		currentSkillCastingTimer += Time.deltaTime;
		switch (currentCastingSkill)
		{
		case Boss52SkillType.RingVAndSunH:
		{
			RVSH_ShootTimer += Time.deltaTime;
			float currentDifficultPatternValue18 = GetCurrentDifficultPatternValue(RVSH_ShootInterval);
			if (RVSH_ShootTimer >= currentDifficultPatternValue18)
			{
				RVSH_ShootTimer -= currentDifficultPatternValue18;
				int num14 = Mathf.CeilToInt(GetCurrentDifficultPatternValue(RVSH_RingVerticalDroneCount));
				int num15 = num14;
				if (RVSH_SkillDuration - currentSkillCastingTimer <= currentDifficultPatternValue18 + 0.2f)
				{
					num15 = Mathf.FloorToInt((float)num15 * 0.7f);
				}
				if (RVSH_SkillDuration - currentSkillCastingTimer <= currentDifficultPatternValue18 * 2f + 0.2f)
				{
					num15 = 0;
				}
				for (int num16 = 0; num16 < num15; num16++)
				{
					Vector3 dir10 = Tool2D.GetDir(currentAngle + (float)num16 * 360f / (float)num15);
					GetVerticalLaserDrone(base.transform.position + dir10 * 0.2f).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.35f, 7f, 2.5f, dir10);
				}
				if (RVSH_ShootCounter >= RVSH_HorizontalDronShotDelayWave)
				{
					for (int num17 = 0; num17 < num14; num17++)
					{
						Vector3 dir11 = Tool2D.GetDir(currentAngle + (float)num17 * 360f / (float)num14);
						Boss52HorizontalDrone component13 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
						float currentDifficultPatternValue19 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
						Vector3 initialMoveDirection = dir11;
						component13.InitDroneData(0.2f, 18f, currentDifficultPatternValue19, 10f, dir11, 1.2f, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 7f, 0f, 0.8f, initialMoveDirection);
					}
				}
				currentAngle += 360f / (float)num14 / 2f;
				RVSH_ShootCounter++;
			}
			if (currentSkillCastingTimer >= RVSH_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.CrossVAndSectorH:
		{
			CVRH_ShootTimer += Time.deltaTime;
			float currentDifficultPatternValue32 = GetCurrentDifficultPatternValue(CVRH_ShootInterval);
			if (CVRH_ShootTimer >= currentDifficultPatternValue32)
			{
				CVRH_ShootTimer -= currentDifficultPatternValue32;
				CVRH_ShootVerticalDrone = true;
				CVRH_HorizontalDroneShootTimer = 0f;
				float num26 = UnityEngine.Random.Range(0f, 360f);
				float currentDifficultPatternValue33 = GetCurrentDifficultPatternValue(CVRH_RotateAngleSpeed);
				int num27 = Mathf.CeilToInt(GetCurrentDifficultPatternValue(CVRH_VerticalDroneCount));
				float currentDifficultPatternValue34 = GetCurrentDifficultPatternValue(CVRH_DroneGroundAreaExistDuration);
				for (int num28 = 0; num28 < num27; num28++)
				{
					Vector3 dir16 = Tool2D.GetDir(num26 + (float)num28 * 360f / (float)num27);
					Boss52VerticalDrone component20 = GetVerticalLaserDrone(base.transform.position + dir16 * 0.2f).GetComponent<Boss52VerticalDrone>();
					component20.InitDroneData(10f, 0.1f, 8f, 4f, dir16, 2.5f, currentDifficultPatternValue34, 10f, 0.1f, 0.1f, 0.2f, CVRH_DroneInitialHightShiftDuration, autoEndRecycle: true, VerticalLaserDroneMotion.CVRH_Rotate);
					component20.CVRH_InitData(currentDifficultPatternValue33, CVRH_StopRotateTime);
					Boss52VerticalDrone component21 = GetVerticalLaserDrone(base.transform.position + dir16 * 0.2f).GetComponent<Boss52VerticalDrone>();
					component21.InitDroneData(10f, 0.1f, 10f, 4f, dir16, 2.5f, currentDifficultPatternValue34, 10f, 0.1f, 0.1f, 0.2f, CVRH_DroneInitialHightShiftDuration, autoEndRecycle: true, VerticalLaserDroneMotion.CVRH_Rotate);
					component21.CVRH_InitData(0f - currentDifficultPatternValue33, CVRH_StopRotateTime);
				}
			}
			if (CVRH_ShootVerticalDrone)
			{
				CVRH_HorizontalDroneShootTimer += Time.deltaTime;
				if (CVRH_HorizontalDroneShootTimer > GetCurrentDifficultPatternValue(CVRH_HorizontalDroneShootDelayTime))
				{
					CVRH_ShootVerticalDrone = false;
					Vector3 normalized = (PlayerMgr.Inst.PlayerPoint - base.transform.position).normalized;
					int num29 = Mathf.CeilToInt(GetCurrentDifficultPatternValue(CVRH_OnesideHorizontalDroneCount));
					bool flag5 = num29 % 2 == 0;
					for (int num30 = 0; num30 < num29; num30++)
					{
						float currentDifficultPatternValue35 = GetCurrentDifficultPatternValue(CVRH_HorizontalSectorAngle);
						Vector3 dir17 = Tool2D.GetDir(normalized, (float)num30 * currentDifficultPatternValue35 + (flag5 ? (currentDifficultPatternValue35 * 0.5f) : 0f));
						Boss52HorizontalDrone component22 = GetHorizontalLaserDrone(base.transform.position + dir17 * 1f).GetComponent<Boss52HorizontalDrone>();
						float delayShootTimer = GetCurrentDifficultPatternValue(CVRH_HorizontalDroneBaseDelayShootDuration) + 0.03f * (float)num30;
						float currentDifficultPatternValue36 = GetCurrentDifficultPatternValue(CVRH_HorizontalDroneWidth);
						Vector3 initialDir5 = dir17;
						float delayLaserTimer6 = 1.9f + 0.03f * (float)Mathf.CeilToInt((float)num30 / 2f);
						Vector3 initialMoveDirection = dir17;
						component22.InitDroneData(delayShootTimer, 18f, currentDifficultPatternValue36, 10f, initialDir5, delayLaserTimer6, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 3f, 0f, 0.8f, initialMoveDirection);
						if (num30 != 0 || flag5)
						{
							dir17 = Tool2D.GetDir(normalized, (float)(-num30) * currentDifficultPatternValue35 - (flag5 ? (currentDifficultPatternValue35 * 0.5f) : 0f));
							Boss52HorizontalDrone component23 = GetHorizontalLaserDrone(base.transform.position + dir17 * 1f).GetComponent<Boss52HorizontalDrone>();
							float delayShootTimer2 = GetCurrentDifficultPatternValue(CVRH_HorizontalDroneBaseDelayShootDuration) + 0.03f * (float)num30;
							float currentDifficultPatternValue37 = GetCurrentDifficultPatternValue(CVRH_HorizontalDroneWidth);
							Vector3 initialDir6 = dir17;
							float delayLaserTimer7 = 1.9f + 0.03f * (float)Mathf.CeilToInt((float)num30 / 2f);
							initialMoveDirection = dir17;
							component23.InitDroneData(delayShootTimer2, 18f, currentDifficultPatternValue37, 10f, initialDir6, delayLaserTimer7, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 3f, 0f, 0.8f, initialMoveDirection);
						}
					}
				}
			}
			if (currentSkillCastingTimer >= CVRH_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.MatrixV:
			if (currentSkillCastingTimer >= WV_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss52SkillType.RingRotateV:
		{
			RRV_CastingTimer += Time.deltaTime;
			float currentDifficultPatternValue11 = GetCurrentDifficultPatternValue(RRV_ShootInterval);
			if (RRV_CastingTimer >= currentDifficultPatternValue11 && RRV_CurrentShootCounter < RRV_ShootRound)
			{
				RRV_CastingTimer = 0f;
				RRV_CurrentShootCounter++;
				float num4 = GetCurrentDifficultPatternValue(RRV_VerticalDroneCount);
				int num5 = UnityEngine.Random.Range(0, 360);
				Vector3 vector2 = PlayerMgr.Inst.PlayerPoint.IgnoreZ();
				bool flag2 = RRV_CurrentShootCounter >= RRV_ShootRound;
				if (flag2)
				{
					num4 = Mathf.CeilToInt(num4 * GetCurrentDifficultPatternValue(RRV_FinalRoundCountRatio));
				}
				float currentDifficultPatternValue12 = GetCurrentDifficultPatternValue(RRV_RotateReduceRadiusSpeed);
				float currentDifficultPatternValue13 = GetCurrentDifficultPatternValue(RRV_RotateRadius);
				SpawnAlertMark(vector2, currentDifficultPatternValue13 / (currentDifficultPatternValue12 * (flag2 ? GetCurrentDifficultPatternValue(RRV_FinalRoundSpeedRatio) : 1f) + 0.2f));
				for (int m = 0; (float)m < num4; m++)
				{
					Vector3 dir7 = Tool2D.GetDir((float)num5 + (float)m * 360f / num4);
					Boss52VerticalDrone component7 = GetVerticalLaserDrone(vector2 + dir7 * currentDifficultPatternValue13).GetComponent<Boss52VerticalDrone>();
					component7.InitDroneData(10f, 0.08f, 7f, 4f, dir7, 2.5f, GetCurrentDifficultPatternValue(RRV_GroundAreaExistDuration), 10f, 0.08f, 0.1f, 0.2f, 0.6f, autoEndRecycle: true, VerticalLaserDroneMotion.RRV_Rotate);
					component7.RRV_InitData(GetCurrentDifficultPatternValue(RRV_RotateSpeed) * (float)(RRV_IsClockWiseRotate ? 1 : (-1)), currentDifficultPatternValue12 * (flag2 ? GetCurrentDifficultPatternValue(RRV_FinalRoundSpeedRatio) : 1f), currentDifficultPatternValue13, vector2);
					if (flag2)
					{
						dir7 = Tool2D.GetDir((float)num5 + ((float)m + 0.5f) * 360f / num4);
						currentDifficultPatternValue13 = GetCurrentDifficultPatternValue(RRV_RotateRadius);
						Boss52VerticalDrone component8 = GetVerticalLaserDrone(vector2 + dir7 * currentDifficultPatternValue13).GetComponent<Boss52VerticalDrone>();
						component8.InitDroneData(10f, 0.08f, 7f, 4f, dir7, 2.5f, GetCurrentDifficultPatternValue(RRV_GroundAreaExistDuration), 10f, 0.08f, 0.1f, 0.2f, 0.6f, autoEndRecycle: true, VerticalLaserDroneMotion.RRV_Rotate);
						component8.RRV_InitData(GetCurrentDifficultPatternValue(RRV_RotateSpeed) * (float)((!RRV_IsClockWiseRotate) ? 1 : (-1)), currentDifficultPatternValue12 * (flag2 ? GetCurrentDifficultPatternValue(RRV_FinalRoundSpeedRatio) : 1f), currentDifficultPatternValue13, vector2);
					}
				}
				RRV_IsClockWiseRotate = !RRV_IsClockWiseRotate;
			}
			if (RRV_CurrentShootCounter >= RRV_ShootRound && RRV_CastingTimer >= GetCurrentDifficultPatternValue(RRV_AfterFinalRoundDelayEndDuration))
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.ThreeCrossBarScanV:
			if (currentSkillCastingTimer >= TCBSV_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss52SkillType.SpeedLerpV:
		{
			float currentDifficultPatternValue20 = GetCurrentDifficultPatternValue(SLV_ShootInterval);
			float currentDifficultPatternValue21 = GetCurrentDifficultPatternValue(SLV_WaveDroneCount);
			float currentDifficultPatternValue22 = GetCurrentDifficultPatternValue(SLV_RingDroneCount);
			float sLV_RotateAngleSpeed = SLV_RotateAngleSpeed;
			float currentDifficultPatternValue23 = GetCurrentDifficultPatternValue(SLV_WaveInterval);
			SLV_ShootTimer -= Time.deltaTime;
			SLV_BaseAngle += sLV_RotateAngleSpeed * Time.deltaTime * (float)(SLV_IsClockWiseRotate ? 1 : (-1));
			if (SLV_ShootTimer <= 0f)
			{
				SLV_ShootTimer += GetCurrentDifficultPatternValue(SLV_ShootInterval);
				for (int num18 = 0; (float)num18 < currentDifficultPatternValue22; num18++)
				{
					Vector3 dir12 = Tool2D.GetDir(SLV_BaseAngle + (float)num18 * 360f / currentDifficultPatternValue22);
					Boss52VerticalDrone component14 = GetVerticalLaserDrone(base.transform.position + dir12 * 0.2f).GetComponent<Boss52VerticalDrone>();
					component14.InitDroneData(10f, 0.15f, 4.5f, GetCurrentDifficultPatternValue(SLV_DroneInitialSpeed) - SLV_BaseSpeedDownPerCount * (float)SLV_ShootCount, dir12, 1.5f, 0f, 0f, 0f, 0.1f, 0.2f, 0.3f, autoEndRecycle: true, VerticalLaserDroneMotion.FSTF_Move);
					component14.FSTF_InitData(GetCurrentDifficultPatternValue(SLV_DroneFinalSpeed), GetCurrentDifficultPatternValue(SLV_BaseStartLerpTime) - (float)SLV_ShootCount * SLV_BaseStartLerpDelayPerCount, SLV_LerpTime);
				}
				SLV_ShootCount++;
				if ((float)SLV_ShootCount >= currentDifficultPatternValue21)
				{
					SLV_ShootTimer = currentDifficultPatternValue23;
					SLV_IsClockWiseRotate = !SLV_IsClockWiseRotate;
					SLV_ShootCount = 0;
					SLV_BaseAngle += sLV_RotateAngleSpeed * currentDifficultPatternValue20 * currentDifficultPatternValue21 * (float)(SLV_IsClockWiseRotate ? 1 : (-1));
					SLV_WaveCount++;
				}
			}
			if ((float)SLV_WaveCount > GetCurrentDifficultPatternValue(SLV_SkillWave))
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.RotateChaseV:
		{
			if (currentSkillCastingTimer >= RCV_SkillDuration)
			{
				EndCurrentCastingSkill();
				break;
			}
			RCV_HorizontalShootTimer += Time.deltaTime;
			if (RCV_HorizontalShootTimer >= GetCurrentDifficultPatternValue(RCV_HorizontalShootInterval))
			{
				RCV_HorizontalShootTimer -= GetCurrentDifficultPatternValue(RCV_HorizontalShootInterval);
				Vector3 cornerToCenterDirection2 = GetCornerToCenterDirection();
				Vector3 dir18 = Tool2D.GetDir(cornerToCenterDirection2, 45f);
				Boss52HorizontalDrone component24 = GetHorizontalLaserDrone(GetRoomCornerPoint(currentTargetCornerType) + dir18 * RCV_HorizontalToCenterDistance - cornerToCenterDirection2 * 2.5f).GetComponent<Boss52HorizontalDrone>();
				float currentDifficultPatternValue38 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
				float currentDifficultPatternValue39 = GetCurrentDifficultPatternValue(RCV_HorizontalDelayShootDuration);
				Vector3 initialMoveDirection = cornerToCenterDirection2;
				component24.InitDroneData(0.1f, 40f, currentDifficultPatternValue38, 10f, cornerToCenterDirection2, currentDifficultPatternValue39, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0.8f, initialMoveDirection);
				dir18 = Tool2D.GetDir(cornerToCenterDirection2, -45f);
				Boss52HorizontalDrone component25 = GetHorizontalLaserDrone(GetRoomCornerPoint(currentTargetCornerType) + dir18 * RCV_HorizontalToCenterDistance - cornerToCenterDirection2 * 2.5f).GetComponent<Boss52HorizontalDrone>();
				float currentDifficultPatternValue40 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
				float currentDifficultPatternValue41 = GetCurrentDifficultPatternValue(RCV_HorizontalDelayShootDuration);
				initialMoveDirection = cornerToCenterDirection2;
				component25.InitDroneData(0.1f, 40f, currentDifficultPatternValue40, 10f, cornerToCenterDirection2, currentDifficultPatternValue41, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0.8f, initialMoveDirection);
			}
			if (RCV_IsCastFinish)
			{
				break;
			}
			RCV_IsCastFinish = true;
			float num31 = UnityEngine.Random.Range(0f, 360f);
			int num32 = Mathf.FloorToInt(GetCurrentDifficultPatternValue(RCV_CoreSpawnCount));
			_ = num32 % 2;
			for (int num33 = 0; num33 < num32; num33++)
			{
				Boss52RCVCore component26 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_RCVLaserCore", base.transform.position).GetComponent<Boss52RCVCore>();
				float currentDifficultPatternValue42 = GetCurrentDifficultPatternValue(RCV_RingDroneCount);
				float num34 = GetCurrentDifficultPatternValue(RCV_CoreScatter) / (float)num32;
				float num35 = (0f - GetCurrentDifficultPatternValue(RCV_CoreScatter)) / 2f + num34 * (float)num33;
				Vector3 dir19 = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), num35);
				component26.InitCoreData(dir19, GetCurrentDifficultPatternValue(RCV_CoreSpeed), GetCurrentDifficultPatternValue(RCV_CoreChasePower), GetCurrentDifficultPatternValue(RCV_CoreDecaySpeed), GetCurrentDifficultPatternValue(RCV_CoreDuration), RCV_CoreDelayMoveDuration, num35);
				for (int num36 = 0; (float)num36 < currentDifficultPatternValue42; num36++)
				{
					Boss52VerticalDrone component27 = GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>();
					float degree4 = num31 + 360f / currentDifficultPatternValue42 * (float)num36;
					Vector3 dir20 = Tool2D.GetDir(degree4);
					component27.InitDroneData(10f, 0.15f, 10f, 1f, dir20, 2f, GetCurrentDifficultPatternValue(RCV_RingDroneTrailDuration), 10f, 0.15f, 0.1f, 0.2f, 0.4f, autoEndRecycle: true, VerticalLaserDroneMotion.RCV_Rotate);
					component27.RCV_InitData(component26.transform, GetCurrentDifficultPatternValue(RCV_RingDroneRotateSpeed), degree4, GetCurrentDifficultPatternValue(RCV_RingDroneRotateRadius), RCV_RingDroneRadiusChangeSpeed, RCV_IsClockWiseRotate);
				}
			}
			RCV_IsClockWiseRotate = !RCV_IsClockWiseRotate;
			break;
		}
		case Boss52SkillType.RotateCrossRingsV:
			if (currentSkillCastingTimer >= RCRV_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			else if (!RCRV_IsCastFinish)
			{
				RCRV_IsCastFinish = true;
				int num23 = Mathf.CeilToInt((float)Mathf.Max(LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width, LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height) / GetCurrentDifficultPatternValue(RCRV_DroneWidth));
				float currentDifficultPatternValue28 = GetCurrentDifficultPatternValue(RCRV_DroneWidth);
				bool flag4 = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
				float currentDifficultPatternValue29 = GetCurrentDifficultPatternValue(RCRV_DroneAttackRadius);
				for (int num24 = 0; num24 < num23; num24++)
				{
					float num25 = ((float)num24 + 0.5f) * currentDifficultPatternValue28;
					float a = Mathf.Min(num25 * 2f * MathF.PI * RCRV_DroneTrailMaxWidthRatio, GetCurrentDifficultPatternValue(RCRV_DroneTrailLength)) / (num25 * 2f * MathF.PI) * 360f / GetCurrentDifficultPatternValue(RCRV_DroneRotateAngleSpeed);
					a = Mathf.Min(a, 360f / GetCurrentDifficultPatternValue(RCRV_DroneRotateAngleSpeed) / 2f);
					Boss52VerticalDrone component18 = GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>();
					float damageRange2 = currentDifficultPatternValue29 * 0.5f;
					float currentDifficultPatternValue30 = GetCurrentDifficultPatternValue(RCRV_DroneLifeDuration);
					Vector3 one2 = Vector3.one;
					float initialSpeed = GetCurrentDifficultPatternValue(RCRV_DroneInitialLerpTime);
					float groundDamageAreaRange = currentDifficultPatternValue29;
					component18.InitDroneData(10f, damageRange2, currentDifficultPatternValue30, 1f, one2, 2f, a, 10f, groundDamageAreaRange, 0.1f, 10f, initialSpeed, autoEndRecycle: true, VerticalLaserDroneMotion.RCRV_Rotate);
					Vector3 dir15 = Tool2D.GetDir(GetToPlayerDirection(), (float)(flag4 ? 1 : (-1)) * GetCurrentDifficultPatternValue(RCRV_DroneInitialScatter));
					component18.RCRV_InitData(currentAngle: Tool2D.GetDegree(dir15.x, dir15.y), followTargetTransform: base.transform, targetRadius: num25, radiusLerpDuration: GetCurrentDifficultPatternValue(RCRV_DroneInitialLerpTime), rotateAngleSpeed: GetCurrentDifficultPatternValue(RCRV_DroneRotateAngleSpeed), StartMoveDelay: GetCurrentDifficultPatternValue(RCRV_DroneDelayStartTimer), isClockwiseRotate: flag4);
					Boss52VerticalDrone component19 = GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>();
					float damageRange3 = currentDifficultPatternValue29 * 0.7f;
					float currentDifficultPatternValue31 = GetCurrentDifficultPatternValue(RCRV_DroneLifeDuration);
					Vector3 one3 = Vector3.one;
					groundDamageAreaRange = GetCurrentDifficultPatternValue(RCRV_DroneInitialLerpTime);
					initialSpeed = currentDifficultPatternValue29;
					component19.InitDroneData(10f, damageRange3, currentDifficultPatternValue31, 1f, one3, 2f, a, 10f, initialSpeed, 0.1f, 10f, groundDamageAreaRange, autoEndRecycle: true, VerticalLaserDroneMotion.RCRV_Rotate);
					dir15 = Tool2D.GetDir(-GetToPlayerDirection(), (float)(flag4 ? 1 : (-1)) * GetCurrentDifficultPatternValue(RCRV_DroneInitialScatter));
					component19.RCRV_InitData(currentAngle: Tool2D.GetDegree(dir15.x, dir15.y), followTargetTransform: base.transform, targetRadius: num25, radiusLerpDuration: GetCurrentDifficultPatternValue(RCRV_DroneInitialLerpTime), rotateAngleSpeed: GetCurrentDifficultPatternValue(RCRV_DroneRotateAngleSpeed), StartMoveDelay: GetCurrentDifficultPatternValue(RCRV_DroneDelayStartTimer), isClockwiseRotate: flag4);
					flag4 = !flag4;
				}
			}
			break;
		case Boss52SkillType.RotateRingsVAndLockH:
			if (currentSkillCastingTimer >= RRVLH_SkillDuration)
			{
				EndCurrentCastingSkill();
				break;
			}
			RRVLH_HorizontalDroneSpawnTimer += Time.deltaTime;
			if (RRVLH_HorizontalDroneSpawnTimer >= GetCurrentDifficultPatternValue(RRVLH_HorizontalDroneSpawnInterval) && currentSkillCastingTimer < GetCurrentDifficultPatternValue(RRVLH_DroneLifeDuration))
			{
				RRVLH_HorizontalDroneSpawnTimer -= GetCurrentDifficultPatternValue(RRVLH_HorizontalDroneSpawnInterval);
				float currentDifficultPatternValue5 = GetCurrentDifficultPatternValue(RRLVH_HorizontalDroneShootCount);
				for (int k = 0; (float)k < currentDifficultPatternValue5; k++)
				{
					Vector3 toPlayerDirection = GetToPlayerDirection();
					Vector3 dir5 = Tool2D.GetDir(RRLVH_HorizontalDroneCurrentAngle + 360f / currentDifficultPatternValue5 * (float)k);
					Boss52HorizontalDrone component5 = GetHorizontalLaserDrone(base.transform.position + dir5 * 2f).GetComponent<Boss52HorizontalDrone>();
					float currentDifficultPatternValue6 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
					float initialSpeed = currentSkillCastingTimer * GetCurrentDifficultPatternValue(RRLVH_TimerToHorizontalDroneRadiusRatio);
					Vector3 initialMoveDirection = dir5;
					component5.InitDroneData(0.2f, 24f, currentDifficultPatternValue6, 10f, toPlayerDirection, 1.2f, 0.1f, 0.5f, 6f, 0.6f, PlayerMgr.Inst.PlayerT, default(Vector3), initialSpeed, 0f, 0.8f, initialMoveDirection);
				}
				RRLVH_HorizontalDroneCurrentAngle += GetCurrentDifficultPatternValue(RRLVH_HorizontalDroneRotateAnglePerShoot);
			}
			if (!RRVLH_IsCastFinish)
			{
				RRVLH_IsCastFinish = true;
				int num3 = Mathf.CeilToInt((float)Mathf.Max(LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width, LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height) / GetCurrentDifficultPatternValue(RRVLH_DroneWidth));
				float currentDifficultPatternValue7 = GetCurrentDifficultPatternValue(RRVLH_DroneWidth);
				float currentDifficultPatternValue8 = GetCurrentDifficultPatternValue(RRVLH_DroneAttackRadius);
				bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
				for (int l = 0; l < num3; l++)
				{
					float targetRadius = ((float)l + 0.5f) * currentDifficultPatternValue7;
					float currentDifficultPatternValue9 = GetCurrentDifficultPatternValue(RRVLH_DroneTrailDuration);
					Boss52VerticalDrone component6 = GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>();
					float damageRange = currentDifficultPatternValue8 * 0.8f;
					float currentDifficultPatternValue10 = GetCurrentDifficultPatternValue(RRVLH_DroneLifeDuration);
					Vector3 one = Vector3.one;
					float initialSpeed = GetCurrentDifficultPatternValue(RRVLH_DroneInitialLerpTime);
					float groundDamageAreaRange = currentDifficultPatternValue8;
					component6.InitDroneData(10f, damageRange, currentDifficultPatternValue10, 1f, one, 2f, currentDifficultPatternValue9, 10f, groundDamageAreaRange, 0.1f, 10f, initialSpeed, autoEndRecycle: true, VerticalLaserDroneMotion.RRVLH_Rotate);
					Vector3 dir6 = Tool2D.GetDir(GetToPlayerDirection(), (float)(flag ? 1 : (-1)) * GetCurrentDifficultPatternValue(RRVLH_DroneInitialScatter));
					component6.RRVLH_InitData(currentAngle: Tool2D.GetDegree(dir6.x, dir6.y), followTargetTransform: base.transform, targetRadius: targetRadius, radiusLerpDuration: GetCurrentDifficultPatternValue(RRVLH_DroneInitialLerpTime), rotateAngleSpeed: GetCurrentDifficultPatternValue(RRVLH_DroneRotateAngleSpeed), StartMoveDelay: GetCurrentDifficultPatternValue(RRVLH_DroneDelayStartTimer), isClockwiseRotate: RRVLH_IsClockWiseRotate);
					flag = !flag;
				}
			}
			break;
		case Boss52SkillType.DashWideH:
		{
			if (!onTargetDashPoint)
			{
				break;
			}
			onTargetDashPoint = false;
			Vector3 toPlayerDirection2 = GetToPlayerDirection();
			float num6 = GetCurrentDifficultPatternValue(DWH_SideDroneCount) + 1f;
			if (dashTargetPoints.Count <= 0)
			{
				num6 += (float)DWH_FinalRoundBonusLaser;
			}
			for (int n = 0; (float)n < num6; n++)
			{
				Boss52HorizontalDrone component9 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
				float currentDifficultPatternValue14 = GetCurrentDifficultPatternValue(DWH_LaserWidth);
				float delayLaserTimer4 = GetCurrentDifficultPatternValue(DWH_BaseDelayShootDuration) + GetCurrentDifficultPatternValue(DWH_SideDroneBonusDelayShootDuration) * (float)n;
				float groundDamageAreaRange = GetCurrentDifficultPatternValue(DWH_InitFlySpeed);
				Vector3 initialMoveDirection = Tool2D.GetDir(toPlayerDirection2, (float)n * DWH_SideDroneAngle);
				component9.InitDroneData(0.1f, 24f, currentDifficultPatternValue14, 10f, toPlayerDirection2, delayLaserTimer4, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), groundDamageAreaRange, 0f, 0.8f, initialMoveDirection);
				if (n > 0)
				{
					Boss52HorizontalDrone component10 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
					float currentDifficultPatternValue15 = GetCurrentDifficultPatternValue(DWH_LaserWidth);
					float delayLaserTimer5 = GetCurrentDifficultPatternValue(DWH_BaseDelayShootDuration) + GetCurrentDifficultPatternValue(DWH_SideDroneBonusDelayShootDuration) * (float)n;
					groundDamageAreaRange = GetCurrentDifficultPatternValue(DWH_InitFlySpeed);
					initialMoveDirection = Tool2D.GetDir(toPlayerDirection2, (float)(-n) * DWH_SideDroneAngle);
					component10.InitDroneData(0.1f, 24f, currentDifficultPatternValue15, 10f, toPlayerDirection2, delayLaserTimer5, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), groundDamageAreaRange, 0f, 0.8f, initialMoveDirection);
				}
			}
			if (dashTargetPoints.Count <= 0)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.DashKeepCastV:
		{
			DKCV_CastTimer += Time.deltaTime;
			if (dashTargetPoints.Count <= 0)
			{
				EndCurrentCastingSkill();
			}
			float num37 = (DKCV_IsCenterCast ? GetCurrentDifficultPatternValue(DKCV_CenterCastInterval) : GetCurrentDifficultPatternValue(DKCV_CastInterval));
			if (!(DKCV_CastTimer < num37))
			{
				DKCV_CastTimer -= num37;
				float currentDifficultPatternValue43 = GetCurrentDifficultPatternValue(DKCV_RingDroneCount);
				for (int num38 = 0; (float)num38 < currentDifficultPatternValue43; num38++)
				{
					Vector3 dir21 = Tool2D.GetDir(DKCV_BaseAngle + (float)(num38 * 360) / currentDifficultPatternValue43);
					Boss52VerticalDrone component28 = GetVerticalLaserDrone(base.transform.position + dir21 * GetCurrentDifficultPatternValue(DKCV_DroneRadius)).GetComponent<Boss52VerticalDrone>();
					component28.InitDroneData(10f, DKCV_LaserDamageRadius, 6f, GetCurrentDifficultPatternValue(DKCV_DroneMoveSpeed), dir21, 1.5f, 0f, 0f, 0f, 0.1f, 0.3f, DKCV_DroneHeightShiftDuration, autoEndRecycle: true, VerticalLaserDroneMotion.DKCV_Move);
					component28.DKCV_InitData(DKCV_DroneDelayMoveDuration, DKCV_DroneMotionLerpDuration);
				}
				DKCV_BaseAngle += DKCV_RotateAngleSpeed;
			}
			break;
		}
		case Boss52SkillType.DashSunH:
			if (onTargetDashPoint)
			{
				onTargetDashPoint = false;
				int num2 = (int)GetCurrentDifficultPatternValue(DSH_DroneCount);
				Vector3 oldDir = ((DSH_WaveCount % 2 == 0) ? ((num2 % 2 != 0) ? GetToPlayerDirection() : Tool2D.GetDir(GetToPlayerDirection(), 360f / (float)num2 / 2f)) : ((num2 % 2 == 0) ? GetToPlayerDirection() : Tool2D.GetDir(GetToPlayerDirection(), 360f / (float)num2 / 2f)));
				for (int j = 0; j < num2; j++)
				{
					Boss52HorizontalDrone component2 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
					Vector3 dir2 = Tool2D.GetDir(oldDir, 360f / (float)num2 * (float)j);
					float currentDifficultPatternValue = GetCurrentDifficultPatternValue(DSH_DroneWidth);
					float delayLaserTimer = GetCurrentDifficultPatternValue(DSH_DroneDelayShootTime) + GetCurrentDifficultPatternValue(DSH_DroneBonusDelayShootTimePerWave) * (float)DSH_WaveCount;
					Vector3 initialMoveDirection = dir2;
					component2.InitDroneData(0.1f, 24f, currentDifficultPatternValue, 10f, dir2, delayLaserTimer, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 4f, 0f, 0.8f, initialMoveDirection);
				}
				DSH_WaveCount++;
				if (dashTargetPoints.Count <= 0)
				{
					EndCurrentCastingSkill();
				}
			}
			break;
		case Boss52SkillType.DashBurstV:
		{
			if (!onTargetDashPoint)
			{
				break;
			}
			onTargetDashPoint = false;
			float currentDifficultPatternValue16 = GetCurrentDifficultPatternValue(DBV_DroneCount);
			float currentDifficultPatternValue17 = GetCurrentDifficultPatternValue(DBV_RingCount);
			Vector3 oldDir2 = GetToPlayerDirection();
			float num7 = 360f / currentDifficultPatternValue16;
			for (int num8 = 0; (float)num8 < currentDifficultPatternValue17; num8++)
			{
				for (int num9 = 0; (float)num9 < currentDifficultPatternValue16; num9++)
				{
					Vector3 dir8 = Tool2D.GetDir(oldDir2, (float)num9 * num7);
					Boss52VerticalDrone component11 = GetVerticalLaserDrone(base.transform.position + dir8 * DBV_RingSpace).GetComponent<Boss52VerticalDrone>();
					component11.InitDroneData(10f, 0.1f, 6f, GetCurrentDifficultPatternValue(DBV_BaseSpeed), dir8, 1.5f, 0f, 0f, 0f, 0.1f, 0.3f, DBV_HeightShiftDuration, autoEndRecycle: true, VerticalLaserDroneMotion.DBV_Move);
					component11.DBV_InitData(GetCurrentDifficultPatternValue(DBV_InitialBonusSpeedPerRing) * (float)num8, DBV_BonusSpeedDecayDuration, DBV_DroneDelayMoveDuration, DBV_DroneMotionLerpDuration);
				}
				oldDir2 = Tool2D.GetDir(oldDir2, num7 / 2f);
			}
			if (dashTargetPoints.Count <= 0)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss52SkillType.CornerDashSectorV:
		{
			if (CDSV_DelayCastTimer >= 0f)
			{
				CDSV_DelayCastTimer -= Time.deltaTime;
				break;
			}
			if (currentSkillCastingTimer >= CDSV_SkillDuration)
			{
				EndCurrentCastingSkill();
				break;
			}
			CDSV_HorizontalDroneCurrentAngle -= GetCurrentDifficultPatternValue(CDSV_HorizontalDroneAngleSpeed) * Time.deltaTime;
			CDSV_HorizontalCastTimer += Time.deltaTime;
			if (CDSV_HorizontalCastTimer <= GetCurrentDifficultPatternValue(CDSV_HorizontalCastDuration))
			{
				CDSV_HorizontalDroneSpawnTimer += Time.deltaTime;
				if (CDSV_HorizontalDroneSpawnTimer >= GetCurrentDifficultPatternValue(CDSV_HorizontalDroneSpawnInterval))
				{
					CDSV_HorizontalDroneSpawnTimer -= GetCurrentDifficultPatternValue(CDSV_HorizontalDroneSpawnInterval);
					Boss52HorizontalDrone component3 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
					Vector3 dir3 = Tool2D.GetDir(GetCornerToCenterDirection(), CDSV_HorizontalDroneCurrentAngle);
					float currentDifficultPatternValue2 = GetCurrentDifficultPatternValue(DSH_DroneWidth);
					Vector3 initialDir2 = dir3;
					float delayLaserTimer2 = GetCurrentDifficultPatternValue(DSH_DroneDelayShootTime) + GetCurrentDifficultPatternValue(DSH_DroneBonusDelayShootTimePerWave) * (float)DSH_WaveCount;
					Vector3 initialMoveDirection = dir3;
					component3.InitDroneData(0.1f, 40f, currentDifficultPatternValue2, 10f, initialDir2, delayLaserTimer2, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 6f, 2f, 1f, initialMoveDirection, 0.1f, 0.15f, disableChargeSE: true);
					Boss52HorizontalDrone component4 = GetHorizontalLaserDrone(base.transform.position).GetComponent<Boss52HorizontalDrone>();
					dir3 = Tool2D.GetDir(GetCornerToCenterDirection(), 0f - CDSV_HorizontalDroneCurrentAngle);
					float currentDifficultPatternValue3 = GetCurrentDifficultPatternValue(DSH_DroneWidth);
					Vector3 initialDir3 = dir3;
					float delayLaserTimer3 = GetCurrentDifficultPatternValue(DSH_DroneDelayShootTime) + GetCurrentDifficultPatternValue(DSH_DroneBonusDelayShootTimePerWave) * (float)DSH_WaveCount;
					initialMoveDirection = dir3;
					component4.InitDroneData(0.1f, 40f, currentDifficultPatternValue3, 10f, initialDir3, delayLaserTimer3, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 6f, 2f, 1f, initialMoveDirection, 0.1f, 0.15f, disableChargeSE: true);
				}
			}
			if (CDSV_CurrentAngle >= CDSV_MaxShiftAngle)
			{
				CDSV_IncreaseShootAngle = false;
			}
			else if (CDSV_CurrentAngle <= 0f - CDSV_MaxShiftAngle)
			{
				CDSV_IncreaseShootAngle = true;
			}
			CDSV_ShootTimer += Time.deltaTime;
			float currentDifficultPatternValue4 = GetCurrentDifficultPatternValue(CDSV_ShootInterval);
			if (CDSV_ShootTimer >= currentDifficultPatternValue4)
			{
				CDSV_ShootTimer -= currentDifficultPatternValue4;
				Vector3 dir4 = Tool2D.GetDir(GetCornerToCenterDirection(), CDSV_CurrentAngle);
				GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, 8f, GetCurrentDifficultPatternValue(CDSV_DroneSpeed), dir4, 1.5f, GetCurrentDifficultPatternValue(CDSV_TrailDuration), 10f, 0.1f, 0.1f, 2.5f, 0.3f);
				dir4 = Tool2D.GetDir(GetCornerToCenterDirection(), 0f - CDSV_CurrentAngle);
				GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, 8f, GetCurrentDifficultPatternValue(CDSV_DroneSpeed), dir4, 1.5f, GetCurrentDifficultPatternValue(CDSV_TrailDuration), 10f, 0.1f, 0.1f, 2.5f, 0.3f);
			}
			CDSV_CurrentAngle += GetCurrentDifficultPatternValue(CDSV_AngleMoveSpeed) * Time.deltaTime * (CDSV_IncreaseShootAngle ? 1f : (-1f));
			break;
		}
		case Boss52SkillType.CornerDashWallHLerpV:
			RCV_HorizontalShootTimer += Time.deltaTime;
			if (RCV_HorizontalShootTimer >= GetCurrentDifficultPatternValue(RCV_HorizontalShootInterval))
			{
				RCV_HorizontalShootTimer -= GetCurrentDifficultPatternValue(RCV_HorizontalShootInterval);
				Vector3 cornerToCenterDirection = GetCornerToCenterDirection();
				Vector3 dir13 = Tool2D.GetDir(cornerToCenterDirection, 45f);
				Boss52HorizontalDrone component15 = GetHorizontalLaserDrone(GetRoomCornerPoint(currentTargetCornerType) + dir13 * RCV_HorizontalToCenterDistance - cornerToCenterDirection * 2.5f).GetComponent<Boss52HorizontalDrone>();
				float currentDifficultPatternValue24 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
				float currentDifficultPatternValue25 = GetCurrentDifficultPatternValue(RCV_HorizontalDelayShootDuration);
				Vector3 initialMoveDirection = cornerToCenterDirection;
				component15.InitDroneData(0.1f, 40f, currentDifficultPatternValue24, 10f, cornerToCenterDirection, currentDifficultPatternValue25, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0.8f, initialMoveDirection, 0.1f, 0.2f, disableChargeSE: true);
				dir13 = Tool2D.GetDir(cornerToCenterDirection, -45f);
				Boss52HorizontalDrone component16 = GetHorizontalLaserDrone(GetRoomCornerPoint(currentTargetCornerType) + dir13 * RCV_HorizontalToCenterDistance - cornerToCenterDirection * 2.5f).GetComponent<Boss52HorizontalDrone>();
				float currentDifficultPatternValue26 = GetCurrentDifficultPatternValue(RVSH_HorizontalDroneWidth);
				float currentDifficultPatternValue27 = GetCurrentDifficultPatternValue(RCV_HorizontalDelayShootDuration);
				initialMoveDirection = cornerToCenterDirection;
				component16.InitDroneData(0.1f, 40f, currentDifficultPatternValue26, 10f, cornerToCenterDirection, currentDifficultPatternValue27, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0.8f, initialMoveDirection, 0.1f, 0.2f, disableChargeSE: true);
			}
			CDWHLV_VDSpawnTimer += Time.deltaTime;
			if (CDWHLV_VDSpawnTimer >= GetCurrentDifficultPatternValue(CDWHLV_VDSpawnInterval) && currentSkillCastingTimer + 2.4f <= GetCurrentDifficultPatternValue(CDWHLV_SkillDuration))
			{
				CDWHLV_VDSpawnTimer -= GetCurrentDifficultPatternValue(CDWHLV_VDSpawnInterval);
				int num19 = ((currentTargetCornerType == MapCornerType.LowerLeft || currentTargetCornerType == MapCornerType.UpperLeft) ? 1 : (-1));
				CDWHLV_CurrentAngle += CDWHLV_AngleMoveSpeed * (float)num19;
				int num20 = (int)GetCurrentDifficultPatternValue(CDWHLV_RingDroneCount);
				float num21 = 360f / (float)num20;
				for (int num22 = 0; num22 < num20; num22++)
				{
					Vector3 spawnPosition = base.transform.position + Tool2D.GetDir(CDWHLV_CurrentAngle + num21 * (float)num22) * CDWHLV_Radius;
					Boss52VerticalDrone component17 = GetVerticalLaserDrone(spawnPosition).GetComponent<Boss52VerticalDrone>();
					bool flag3 = UnityEngine.Random.Range(0f, 1f) >= 0.15f;
					Vector3 dir14 = Tool2D.GetDir(GetCornerToCenterDirection(), UnityEngine.Random.Range(0f - GetCurrentDifficultPatternValue(CDWHLV_MaxScatter), flag3 ? GetCurrentDifficultPatternValue(CDWHLV_MaxScatter) : 0f));
					component17.InitDroneData(10f, 0.1f, 8f, 0f, dir14, 1.5f, 0f, 0f, 0f, 0.1f, 1f, 0.3f, autoEndRecycle: true, VerticalLaserDroneMotion.FSTF_Move);
					component17.FSTF_InitData(GetCurrentDifficultPatternValue(CDWHLV_VDFinalSpeed), 1.2f, 1f);
				}
			}
			if (currentSkillCastingTimer >= GetCurrentDifficultPatternValue(CDWHLV_SkillDuration))
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss52SkillType.RingStarVAndWebH:
		{
			if ((SVWH_VShootTimer > 2f && SVWH_VDronesList.Count > 0 && Tool2D.IgnoreZDistance(base.transform.position, SVWH_VDronesList[0].transform.position) <= 0.5f) || SVWH_VShootTimer > 9f)
			{
				foreach (Boss52VerticalDrone sVWH_VDrones in SVWH_VDronesList)
				{
					sVWH_VDrones.EndDroneAction();
				}
				EndCurrentCastingSkill();
			}
			SVWH_VShootTimer += Time.deltaTime;
			int num10 = (int)GetCurrentDifficultPatternValue(SVWH_VDroneShootCount);
			float num11 = 360f / (float)num10;
			Vector3 dir9 = Tool2D.GetDir(GetToPlayerDirection(), num11 / 2f * (float)((num10 % 2 == 0) ? 1 : 0));
			if (SVWH_ReadyToShootNewWave && SVWH_VShootTimer >= 0f)
			{
				SVWH_ReadyToShootNewWave = false;
				for (int num12 = 0; num12 < num10; num12++)
				{
					Vector3 initialDir4 = Tool2D.GetDir(dir9, num11 * (float)num12).IgnoreZ();
					Boss52VerticalDrone component12 = GetVerticalLaserDrone(base.transform.position).GetComponent<Boss52VerticalDrone>();
					component12.InitDroneData(10f, 0.1f, 360f / GetCurrentDifficultPatternValue(SVWH_VAngleSpeed), GetCurrentDifficultPatternValue(SVWH_VMoveSpeed), initialDir4, 1.5f, GetCurrentDifficultPatternValue(SVWH_VTrailDuration), 10f, 0.2f, 0.1f, 0.5f, 0.1f, autoEndRecycle: true, VerticalLaserDroneMotion.SVWH_Rotate);
					component12.SVWH_InitData(GetCurrentDifficultPatternValue(SVWH_VAngleSpeed) * (float)(SVWH_IsClockWiseRotate ? 1 : (-1)));
					SVWH_VDronesList.Add(component12);
				}
			}
			SVWH_HShootTimer += Time.deltaTime;
			if (SVWH_HShootTimer >= GetCurrentDifficultPatternValue(SVWH_HShootInterval) && SVWH_VDronesList.Count > 0)
			{
				SVWH_HShootTimer -= GetCurrentDifficultPatternValue(SVWH_HShootInterval);
				for (int num13 = 0; num13 < SVWH_VDronesList.Count; num13++)
				{
					Boss52VerticalDrone boss52VerticalDrone = SVWH_VDronesList[num13];
					GetHorizontalLaserDrone(boss52VerticalDrone.transform.position.IgnoreZ()).GetComponent<Boss52HorizontalDrone>().InitDroneData(0.2f, 40f, GetCurrentDifficultPatternValue(SVWH_HDroneWidth), 10f, (SVWH_GetNextDronePosition(num13, SVWH_HDroneNextLockIndex * (SVWH_IsClockWiseRotate ? 1 : (-1))) - boss52VerticalDrone.transform.position.IgnoreZ()).IgnoreZ().normalized, GetCurrentDifficultPatternValue(SVWH_HDroneShootDelayDuration));
				}
			}
			break;
		}
		case Boss52SkillType.InnerRingV:
			if ((float)IRV_WaveCounter >= GetCurrentDifficultPatternValue(IRV_WaveCount))
			{
				EndCurrentCastingSkill();
				break;
			}
			if (!IRV_IsLockPoint)
			{
				IRV_IsLockPoint = true;
				Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
				if (Mathf.Abs(playerPoint.x - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / 2f - IRV_CenterPointMinWidth)
				{
					if (playerPoint.x <= LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
					{
						playerPoint.x = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / 2f + IRV_CenterPointMinWidth;
					}
					else
					{
						playerPoint.x = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / 2f - IRV_CenterPointMinWidth;
					}
				}
				if (Mathf.Abs(playerPoint.y - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y) >= (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f - IRV_CenterPointMinWidth)
				{
					if (playerPoint.y <= LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y)
					{
						playerPoint.y = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f + IRV_CenterPointMinWidth;
					}
					else
					{
						playerPoint.x = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f - IRV_CenterPointMinWidth;
					}
				}
				IRV_CenterPoint = playerPoint;
				SpawnAlertMark(IRV_CenterPoint, GetCurrentDifficultPatternValue(IRV_OneWaveDuration) + 1f);
			}
			IRV_ShootTimer += Time.deltaTime;
			IRV_WaveShootRemainTimer -= Time.deltaTime;
			IRV_CurrentBaseAngle += GetCurrentDifficultPatternValue(IRV_AngleMoveSpeed) * Time.deltaTime * (float)(IRV_IsClockWise ? 1 : (-1));
			if (IRV_ShootTimer >= GetCurrentDifficultPatternValue(IRV_DroneShootInterval) && IRV_WaveShootRemainTimer > 0f)
			{
				IRV_ShootTimer -= GetCurrentDifficultPatternValue(IRV_DroneShootInterval);
				float num = 360f / GetCurrentDifficultPatternValue(IRV_RingDroneCount);
				for (int i = 0; (float)i < GetCurrentDifficultPatternValue(IRV_RingDroneCount); i++)
				{
					Vector3 dir = Tool2D.GetDir(IRV_CurrentBaseAngle + num * (float)i);
					Vector3 vector = IRV_CenterPoint + dir * GetCurrentDifficultPatternValue(IRV_RingRadius);
					Vector3 initialDir = Tool2D.IgnoreZV2ToV1(IRV_CenterPoint, vector);
					Boss52VerticalDrone component = GetVerticalLaserDrone(vector).GetComponent<Boss52VerticalDrone>();
					component.InitDroneData(10f, 0.1f, GetCurrentDifficultPatternValue(IRV_DroneDuration), 0f, initialDir, 1.5f, GetCurrentDifficultPatternValue(IRV_TrailDuration), 10f, 0.1f, 0.1f, 0.5f, 0.1f, autoEndRecycle: true, VerticalLaserDroneMotion.FSTF_Move);
					component.FSTF_InitData(GetCurrentDifficultPatternValue(IRV_MoveSpeed), IRV_DroneDelayMoveDuration, IRV_DroneLerpDuration);
				}
			}
			IRV_WaveTimer += Time.deltaTime;
			if (IRV_WaveTimer >= GetCurrentDifficultPatternValue(IRV_WaveInterval))
			{
				IRV_IsLockPoint = false;
				IRV_WaveTimer = 0f;
				IRV_WaveShootRemainTimer = GetCurrentDifficultPatternValue(IRV_OneWaveDuration);
				IRV_ShootTimer = 0f;
				IRV_WaveCounter++;
				IRV_IsClockWise = !IRV_IsClockWise;
			}
			break;
		case Boss52SkillType.None:
			break;
		}
	}

	private void CastTargetSkill(Boss52SkillType targetSkill)
	{
		currentCastingSkill = targetSkill;
		switch (targetSkill)
		{
		case Boss52SkillType.RingVAndSunH:
		case Boss52SkillType.CrossVAndSectorH:
			skillDelayCastTimer = 0f;
			base.Anima.SetTrigger(Praise);
			break;
		case Boss52SkillType.RotateRingsVAndLockH:
			skillDelayCastTimer = 0.8f;
			base.Anima.SetTrigger(Praise);
			break;
		case Boss52SkillType.RingRotateV:
			RRV_CastingTimer = GetCurrentDifficultPatternValue(RRV_ShootInterval);
			skillDelayCastTimer = 0.5f;
			isDisableUpdateBossAndPlayer = true;
			idleDelayMoveTimer = 1.8f;
			base.Anima.SetTrigger(PointTarget);
			break;
		case Boss52SkillType.RotateCrossRingsV:
			base.Anima.SetTrigger(Praise);
			break;
		case Boss52SkillType.SpeedLerpV:
			base.Anima.SetTrigger(Praise);
			skillDelayCastTimer = 0.8f;
			SLV_BaseAngle = UnityEngine.Random.Range(0, 360);
			break;
		case Boss52SkillType.MatrixV:
			StartCoroutine(WellVerticalLaser(currentTargetCornerType, WV_DelayCastTime));
			break;
		case Boss52SkillType.ThreeCrossBarScanV:
			StartCoroutine(WellSlideVerticalLaser(currentTargetCornerType, TCBSV_DelayCastTime));
			break;
		case Boss52SkillType.RotateChaseV:
			RCV_HorizontalToCenterDistance = GetCurrentDifficultPatternValue(RCV_HorizontalInitialDistance);
			DOTween.To(() => RCV_HorizontalToCenterDistance, delegate(float x)
			{
				RCV_HorizontalToCenterDistance = x;
			}, GetCurrentDifficultPatternValue(RCV_HorizontalTargetToCenterDistance), RCV_HorizontalLerpDuration).SetEase(Ease.OutQuad);
			break;
		case Boss52SkillType.DashKeepCastV:
			DKCV_CastTimer = 0f;
			DKCV_IsCenterCast = forceCastGeneralSkill;
			break;
		case Boss52SkillType.DashSunH:
			DSH_WaveCount = 0;
			break;
		case Boss52SkillType.CornerDashSectorV:
			CDSV_CurrentAngle = CDSV_MaxShiftAngle + UnityEngine.Random.Range(-10f, 10f);
			CDSV_IncreaseShootAngle = false;
			CDSV_ShootTimer = 0f;
			CDSV_HorizontalDroneSpawnTimer = 0f;
			CDSV_HorizontalDroneCurrentAngle = CDSV_HorizontalDroneInitialAngle;
			CDSV_HorizontalCastTimer = 0f;
			CDSV_DelayCastTimer = CDSV_DelayCastTime;
			break;
		case Boss52SkillType.CornerDashWallHLerpV:
			CDWHLV_CurrentAngle = 0f;
			CDWHLV_VDSpawnTimer = 0f;
			RCV_HorizontalToCenterDistance = GetCurrentDifficultPatternValue(RCV_HorizontalInitialDistance);
			DOTween.To(() => RCV_HorizontalToCenterDistance, delegate(float x)
			{
				RCV_HorizontalToCenterDistance = x;
			}, GetCurrentDifficultPatternValue(RCV_HorizontalTargetToCenterDistance), RCV_HorizontalLerpDuration).SetEase(Ease.OutQuad);
			break;
		case Boss52SkillType.RingStarVAndWebH:
			skillDelayCastTimer = 0.3f;
			base.Anima.SetTrigger(Praise);
			SVWH_VShootTimer = -0.5f;
			SVWH_HShootTimer = 0f;
			SVWH_VDronesList.Clear();
			SVWH_IsClockWiseRotate = !SVWH_IsClockWiseRotate;
			SVWH_ReadyToShootNewWave = true;
			break;
		case Boss52SkillType.InnerRingV:
			skillDelayCastTimer = 0.5f;
			IRV_ShootTimer = 0f;
			IRV_CenterPoint = PlayerMgr.Inst.PlayerPoint;
			IRV_IsClockWise = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
			IRV_IsLockPoint = false;
			IRV_WaveTimer = 0f;
			IRV_WaveShootRemainTimer = GetCurrentDifficultPatternValue(IRV_OneWaveDuration);
			IRV_WaveCounter = 0;
			base.Anima.SetTrigger(PointTarget);
			idleDelayMoveTimer = 2.8f;
			isDisableUpdateBossAndPlayer = true;
			break;
		case Boss52SkillType.None:
		case Boss52SkillType.DashWideH:
		case Boss52SkillType.DashBurstV:
			break;
		}
	}

	private void EndCurrentCastingSkill()
	{
		if (currentCastingSkill != 0)
		{
			currentSkillCastingTimer = 0f;
			Boss52SkillType boss52SkillType = currentCastingSkill;
			currentCastingSkill = Boss52SkillType.None;
			switch (boss52SkillType)
			{
			case Boss52SkillType.RingVAndSunH:
				RVSH_ShootTimer = 0f;
				RVSH_ShootCounter = 0;
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.MatrixV:
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.CrossVAndSectorH:
				CVRH_HorizontalDroneShootTimer = 0f;
				CVRH_ShootTimer = GetCurrentDifficultPatternValue(CVRH_ShootInterval) - 0.3f;
				CVRH_ShootVerticalDrone = false;
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.RingRotateV:
				RRV_CastingTimer = 0f;
				RRV_CurrentShootCounter = 0;
				MoveEnterState(Boss52MoveState.Idle);
				isDisableUpdateBossAndPlayer = false;
				idleDelayMoveTimer = 1.2f;
				break;
			case Boss52SkillType.ThreeCrossBarScanV:
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.SpeedLerpV:
				SLV_ShootTimer = 0f;
				SLV_WaveCount = 0;
				SLV_ShootCount = 0;
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.RotateChaseV:
				RCV_IsCastFinish = false;
				CastDashToPlayerCornerSkill2();
				break;
			case Boss52SkillType.RotateCrossRingsV:
				RCRV_IsCastFinish = false;
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.RotateRingsVAndLockH:
				RRVLH_IsCastFinish = false;
				RRVLH_IsClockWiseRotate = !RRVLH_IsClockWiseRotate;
				RRVLH_HorizontalDroneSpawnTimer = -0.5f;
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.CornerDashSectorV:
			case Boss52SkillType.CornerDashWallHLerpV:
				CastDashToPlayerCornerSkill2();
				break;
			case Boss52SkillType.RingStarVAndWebH:
				MoveEnterState(Boss52MoveState.Idle);
				break;
			case Boss52SkillType.InnerRingV:
				MoveEnterState(Boss52MoveState.Idle);
				isDisableUpdateBossAndPlayer = false;
				idleDelayMoveTimer = 2.2f;
				break;
			case Boss52SkillType.None:
			case Boss52SkillType.DashWideH:
			case Boss52SkillType.DashKeepCastV:
			case Boss52SkillType.DashSunH:
			case Boss52SkillType.DashBurstV:
				break;
			}
		}
	}

	public Vector3 SVWH_GetNextDronePosition(int index, int jumpCount = 1)
	{
		if (SVWH_VDronesList.Count <= 0)
		{
			return Vector3.zero;
		}
		int num = index + jumpCount;
		if (num >= SVWH_VDronesList.Count)
		{
			num -= SVWH_VDronesList.Count;
		}
		if (num < 0)
		{
			num += SVWH_VDronesList.Count;
		}
		return SVWH_VDronesList[num].gameObject.transform.position;
	}

	public Vector3 GetToPlayerDirection()
	{
		return (PlayerMgr.Inst.PlayerPoint - base.transform.position).normalized;
	}

	public float GetToPlayerDistance()
	{
		return Tool2D.IgnoreZDistance(base.transform.position, PlayerMgr.Inst.PlayerPoint);
	}

	private GameObject GetVerticalLaserDrone(Vector3 spawnPosition)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", spawnPosition);
	}

	private void SpawnAlertMark(Vector3 spawnPosition, float markDuration)
	{
		SEMgr.Inst.boss52MarkPoint.PlaySE();
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_LockAlert", spawnPosition, markDuration);
	}

	private GameObject GetHorizontalLaserDrone(Vector3 spawnPosition)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_HorizontalLaserDrone", spawnPosition);
	}

	private void SpawnDashShadow()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_DashShadow", base.transform.position).GetComponent<Boss52BodyShadow>().StartFade(0.9f, 0.15f, 0.1f, isFaceRight ? 1 : (-1));
	}

	private IEnumerator RRV_RingVerticalLaser(int spawnCount, float waitTime, float speed, Vector3 startPoint)
	{
		yield return new WaitForSeconds(waitTime);
		for (int i = 0; i < spawnCount; i++)
		{
			Vector3 dir = Tool2D.GetDir(currentAngle + (float)i * 360f / (float)spawnCount);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", startPoint + dir * 0.1f).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.2f, 10f, speed, dir, 2.5f, 0f, 0f, 0f, 0.1f, 0.2f, 0f);
		}
	}

	private MapCornerType GetRandomCorner()
	{
		List<MapCornerType> list = new List<MapCornerType>
		{
			MapCornerType.UpperLeft,
			MapCornerType.UpperRight,
			MapCornerType.LowerLeft,
			MapCornerType.LowerRight
		};
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	private IEnumerator WellVerticalLaser(MapCornerType type, float castDelayTimer)
	{
		yield return new WaitForSeconds(castDelayTimer);
		Vector3 startPos = GetRoomCornerPoint(type);
		float blockWidth = GetCurrentDifficultPatternValue(WV_BlockWidth);
		float firstLaserShift = UnityEngine.Random.Range(0f, blockWidth);
		float horizontalLaserDuration = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / GetCurrentDifficultPatternValue(WV_DroneSpeed) + WV_DroneInitialHightShiftDuration + 0.5f;
		float verticalLaserDuration = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / GetCurrentDifficultPatternValue(WV_DroneSpeed) + WV_DroneInitialHightShiftDuration + 0.5f;
		int horizontalSpawnCount = Mathf.CeilToInt(((float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height - firstLaserShift) / blockWidth);
		int verticalSpawnCount = Mathf.CeilToInt(((float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width - firstLaserShift) / blockWidth);
		float groundAreaExistDuration = GetCurrentDifficultPatternValue(WV_DroneGroundAreaExistDuration);
		float droneSpeed = GetCurrentDifficultPatternValue(WV_DroneSpeed);
		float droneInitialshiftDis = droneSpeed * WV_DroneInitialHightShiftDuration;
		int verticalShootPointMoveDir = 1;
		int horizontalShootPointMoveDir = 1;
		int verticalLaserDir = 1;
		int horizontalLaserDir = 1;
		MapCornerType mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperLeft || mapCornerType == MapCornerType.UpperRight)
		{
			verticalLaserDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperRight || mapCornerType == MapCornerType.LowerRight)
		{
			horizontalLaserDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperRight || mapCornerType == MapCornerType.LowerRight)
		{
			verticalShootPointMoveDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperLeft || mapCornerType == MapCornerType.UpperRight)
		{
			horizontalShootPointMoveDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperRight || mapCornerType == MapCornerType.LowerLeft)
		{
			droneInitialshiftDis *= -1f;
		}
		for (int i = 0; i < Mathf.Max(horizontalSpawnCount, verticalSpawnCount); i++)
		{
			if (i < horizontalSpawnCount)
			{
				Vector3 point = startPos + new Vector3(droneInitialshiftDis, firstLaserShift + (float)i * blockWidth, 0f) * horizontalShootPointMoveDir;
				Vector3 initialDir = new Vector3(1f, 0f, 0f) * horizontalLaserDir;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.2f, horizontalLaserDuration, droneSpeed, initialDir, 2.5f, groundAreaExistDuration, 10f, 0.2f, 0.1f, 0.2f, WV_DroneInitialHightShiftDuration);
			}
			if (i < verticalSpawnCount)
			{
				Vector3 point2 = startPos + new Vector3(firstLaserShift + (float)i * blockWidth, droneInitialshiftDis, 0f) * verticalShootPointMoveDir;
				Vector3 initialDir2 = new Vector3(0f, 1f, 0f) * verticalLaserDir;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point2).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.2f, verticalLaserDuration, droneSpeed, initialDir2, 2.5f, groundAreaExistDuration, 10f, 0.2f, 0.1f, 0.2f, WV_DroneInitialHightShiftDuration);
			}
			yield return new WaitForSeconds(WV_SpawnInterval);
		}
	}

	private IEnumerator WellSlideVerticalLaser(MapCornerType type, float castDelayTimer)
	{
		yield return new WaitForSeconds(castDelayTimer);
		GetRoomCornerPoint(type);
		float blockWidth = GetCurrentDifficultPatternValue(TCBSV_BlockWidth);
		_ = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		float horizontalLaserDuration = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / GetCurrentDifficultPatternValue(TCBSV_DroneSpeed) * 0.7f + TCBSV_DroneInitialHightShiftDuration;
		float verticalLaserDuration = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / GetCurrentDifficultPatternValue(TCBSV_DroneSpeed) * 0.7f + TCBSV_DroneInitialHightShiftDuration;
		int num = Mathf.CeilToInt((float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / blockWidth);
		int num2 = Mathf.CeilToInt((float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width / blockWidth);
		float groundAreaExistDuration = GetCurrentDifficultPatternValue(TCBSV_DroneGroundAreaExistDuration);
		float droneSpeed = GetCurrentDifficultPatternValue(TCBSV_DroneSpeed);
		_ = droneSpeed * TCBSV_DroneInitialHightShiftDuration;
		float droneInitialshiftDis = 0f;
		int verticalLaserDir = 1;
		int horizontalLaserDir = 1;
		MapCornerType mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperLeft || mapCornerType == MapCornerType.UpperRight)
		{
			verticalLaserDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperRight || mapCornerType == MapCornerType.LowerRight)
		{
			horizontalLaserDir = -1;
		}
		mapCornerType = type;
		if (mapCornerType == MapCornerType.UpperRight || mapCornerType == MapCornerType.LowerLeft)
		{
			droneInitialshiftDis *= -1f;
		}
		for (int j = 0; j < Mathf.Max(num, num2); j++)
		{
			if (j < num)
			{
				Vector3 point = ((type == MapCornerType.LowerLeft || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.LowerLeft) + new Vector3(0f - Mathf.Abs(droneInitialshiftDis), 0f, 0f)) : (GetRoomCornerPoint(MapCornerType.LowerRight) + new Vector3(Mathf.Abs(droneInitialshiftDis), 0f, 0f))) + new Vector3(0f, (float)j * blockWidth, 0f);
				Vector3 dir = Tool2D.GetDir(new Vector3(0f, 1f, 0f), (0f - GetCurrentDifficultPatternValue(TCBSV_SlideAngle)) * (float)horizontalLaserDir);
				float num3 = 1f / Mathf.Abs(dir.x);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, horizontalLaserDuration, droneSpeed * num3, dir, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
				point = ((type == MapCornerType.LowerLeft || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperLeft) + new Vector3(0f - Mathf.Abs(droneInitialshiftDis), 0f, 0f)) : (GetRoomCornerPoint(MapCornerType.UpperRight) + new Vector3(Mathf.Abs(droneInitialshiftDis), 0f, 0f))) + new Vector3(0f, (float)(-j) * blockWidth, 0f);
				Vector3 dir2 = Tool2D.GetDir(new Vector3(0f, -1f, 0f), GetCurrentDifficultPatternValue(TCBSV_SlideAngle) * (float)horizontalLaserDir);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, horizontalLaserDuration, droneSpeed * num3, dir2, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			}
			if (j < num2)
			{
				Vector3 obj = ((type == MapCornerType.UpperRight || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperLeft) + new Vector3(0f, Mathf.Abs(droneInitialshiftDis), 0f)) : (GetRoomCornerPoint(MapCornerType.LowerLeft) + new Vector3(0f, 0f - Mathf.Abs(droneInitialshiftDis), 0f)));
				Vector3 dir3 = Tool2D.GetDir(new Vector3(1f, 0f, 0f), GetCurrentDifficultPatternValue(TCBSV_SlideAngle) * (float)verticalLaserDir);
				float num4 = 1f / Mathf.Abs(dir3.y);
				Vector3 point2 = obj + new Vector3((float)j * blockWidth, 0f, 0f);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point2).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, verticalLaserDuration, droneSpeed * num4, dir3, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
				point2 = ((type == MapCornerType.UpperRight || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperRight) + new Vector3(0f, Mathf.Abs(droneInitialshiftDis), 0f)) : (GetRoomCornerPoint(MapCornerType.LowerRight) + new Vector3(0f, 0f - Mathf.Abs(droneInitialshiftDis), 0f))) + new Vector3((float)(-j) * blockWidth, 0f, 0f);
				Vector3 dir4 = Tool2D.GetDir(new Vector3(-1f, 0f, 0f), (0f - GetCurrentDifficultPatternValue(TCBSV_SlideAngle)) * (float)verticalLaserDir);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point2).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, verticalLaserDuration, droneSpeed * num4, dir4, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			}
		}
		for (int i = 0; i < 10; i++)
		{
			Vector3 point3 = ((type == MapCornerType.LowerLeft || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.LowerLeft) + new Vector3(0f - Mathf.Abs(droneInitialshiftDis), 0f, 0f)) : (GetRoomCornerPoint(MapCornerType.LowerRight) + new Vector3(Mathf.Abs(droneInitialshiftDis), 0f, 0f))) + new Vector3((float)i * blockWidth * (float)horizontalLaserDir, 0f, 0f);
			Vector3 dir5 = Tool2D.GetDir(new Vector3(0f, 1f, 0f), (0f - GetCurrentDifficultPatternValue(TCBSV_SlideAngle)) * (float)horizontalLaserDir);
			float num5 = 1f / Mathf.Abs(dir5.x);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point3).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, horizontalLaserDuration, droneSpeed * num5, dir5, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			point3 = ((type == MapCornerType.LowerLeft || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperLeft) + new Vector3(0f - Mathf.Abs(droneInitialshiftDis), 0f, 0f)) : (GetRoomCornerPoint(MapCornerType.UpperRight) + new Vector3(Mathf.Abs(droneInitialshiftDis), 0f, 0f))) + new Vector3((float)i * blockWidth * (float)horizontalLaserDir, 0f, 0f);
			Vector3 dir6 = Tool2D.GetDir(new Vector3(0f, -1f, 0f), GetCurrentDifficultPatternValue(TCBSV_SlideAngle) * (float)horizontalLaserDir);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point3).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, horizontalLaserDuration, droneSpeed * num5, dir6, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			Vector3 obj2 = ((type == MapCornerType.UpperRight || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperLeft) + new Vector3(0f, Mathf.Abs(droneInitialshiftDis), 0f)) : (GetRoomCornerPoint(MapCornerType.LowerLeft) + new Vector3(0f, 0f - Mathf.Abs(droneInitialshiftDis), 0f)));
			dir5 = Tool2D.GetDir(new Vector3(1f, 0f, 0f), GetCurrentDifficultPatternValue(TCBSV_SlideAngle) * (float)verticalLaserDir);
			num5 = 1f / Mathf.Abs(dir5.x);
			point3 = obj2 + new Vector3(0f, (float)i * blockWidth * (float)verticalLaserDir, 0f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point3).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, verticalLaserDuration, droneSpeed * num5, dir5, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			point3 = ((type == MapCornerType.UpperRight || type == MapCornerType.UpperLeft) ? (GetRoomCornerPoint(MapCornerType.UpperRight) + new Vector3(0f, Mathf.Abs(droneInitialshiftDis), 0f)) : (GetRoomCornerPoint(MapCornerType.LowerRight) + new Vector3(0f, 0f - Mathf.Abs(droneInitialshiftDis), 0f))) + new Vector3(0f, (float)i * blockWidth * (float)verticalLaserDir, 0f);
			dir6 = Tool2D.GetDir(new Vector3(-1f, 0f, 0f), (0f - GetCurrentDifficultPatternValue(TCBSV_SlideAngle)) * (float)verticalLaserDir);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_VerticalLaserDrone", point3).GetComponent<Boss52VerticalDrone>().InitDroneData(10f, 0.1f, verticalLaserDuration, droneSpeed * num5, dir6, 1.5f, groundAreaExistDuration, 10f, 0.1f, 0.1f, 0.2f, TCBSV_DroneInitialHightShiftDuration);
			yield return new WaitForSeconds(blockWidth / droneSpeed);
		}
	}

	private Vector3 GetRoomCornerPoint(MapCornerType type)
	{
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		float num = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		float num2 = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		return type switch
		{
			MapCornerType.UpperLeft => centerPoint + new Vector3((0f - num) / 2f, num2 / 2f, 0f), 
			MapCornerType.UpperCenter => centerPoint + new Vector3(0f, num2 / 2f, 0f), 
			MapCornerType.UpperRight => centerPoint + new Vector3(num / 2f, num2 / 2f, 0f), 
			MapCornerType.MiddleLeft => centerPoint + new Vector3((0f - num) / 2f, 0f, 0f), 
			MapCornerType.MiddleCenter => centerPoint, 
			MapCornerType.MiddleRight => centerPoint + new Vector3(num / 2f, 0f, 0f), 
			MapCornerType.LowerLeft => centerPoint + new Vector3((0f - num) / 2f, (0f - num2) / 2f, 0f), 
			MapCornerType.LowerCenter => centerPoint + new Vector3(0f, (0f - num2) / 2f, 0f), 
			MapCornerType.LowerRight => centerPoint + new Vector3(num / 2f, (0f - num2) / 2f, 0f), 
			_ => centerPoint, 
		};
	}

	private float GetCurrentDifficultPatternValue(Vector2 target)
	{
		return target.x + (target.y - target.x) * currentDifficultLevel;
	}

	public void EmitChargeEffectParticle()
	{
		ChargeEffectParticle.Play();
	}

	public void PlayAfterChargeEffectParticle()
	{
		AfterChargeEffectParticle.Play();
		AfterChargeBodyEffectParticle.Play();
		ChargeEffectParticleHead.Play();
		AfterChargeEyeEffectParticle.Play();
	}

	public void StopAfterChargeEffectParticle()
	{
		AfterChargeEffectParticle.Stop();
		AfterChargeBodyEffectParticle.Stop();
		AfterChargeEyeEffectParticle.Stop();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "SpawnDashShadow":
			break;
		case "EmitChargeEffectParticle":
			EmitChargeEffectParticle();
			break;
		case "PlayAfterChargeEffectParticle":
			PlayAfterChargeEffectParticle();
			break;
		case "StopAfterChargeEffectParticle":
			StopAfterChargeEffectParticle();
			break;
		case "EyeStartGlow":
			ChargeEffectParticleHead.Play();
			break;
		case "EyeStayGlow":
			AfterChargeEyeEffectParticle.Play();
			break;
		case "PlayBurstSE":
			SEMgr.Inst.boss52ChargeBurst.PlaySE();
			break;
		case "PlayChargeSE":
			SEMgr.Inst.boss52Charge.PlaySE();
			break;
		case "PlayDashChargeSE":
			SEMgr.Inst.boss52DashCharge.PlaySE();
			break;
		case "PlayHandChargeEffect":
			ChargeHandEffectParticle.Play();
			PointTargetChargeEffectParticle.Play();
			break;
		}
	}

	private void PlayDashSE()
	{
		SEMgr.Inst.boss52Sprint.PlaySE();
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
