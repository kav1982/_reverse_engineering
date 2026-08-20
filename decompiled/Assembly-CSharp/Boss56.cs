using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;

public class Boss56 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum Boss56ActionType
	{
		None,
		SummonElite,
		SwitchPhase_SummonToGunBlade,
		GunBlade,
		SwitchPhase_GunBladeToMix,
		Mix
	}

	private enum CommanderGears
	{
		None,
		ScabbardBlade,
		LHandBlade,
		LHandPistol,
		RHandBlade,
		Controller
	}

	public enum AnimationActions
	{
		None,
		WalkNoWeapon,
		WalkWithWeapon,
		UseRemote,
		BladeCommand,
		IdleNoWeapon,
		IdleWithWeapon
	}

	public enum Boss56SkillType
	{
		None = 0,
		E51CannonWave = 1,
		E53RotateBall = 2,
		E50BombWave = 3,
		E57MissileWave = 4,
		E56MissileCombo = 5,
		E56MissileChain = 6,
		E55HexAttackCombo = 7,
		E59LaserRoad = 8,
		E58ThunderEnchantment = 9,
		E52BulletRoadRoller = 10,
		SThrowGrenades = 21,
		SSlashStabSlash = 22,
		SFastDashShoot = 23,
		SStackDonutSlash = 24,
		SGrenadeRingSlash = 25,
		SToCenterCrossSlash = 26,
		SWaveSlash = 27,
		SDashShoot = 29
	}

	public enum Boss56State
	{
		BornIdle,
		PhaseSwitch,
		Battle
	}

	public enum Boss56MoveState
	{
		Idle,
		Close,
		SkillMotion,
		StopMotion
	}

	public static Boss56 Inst;

	private static readonly int WalkNoBlade = Animator.StringToHash("WalkNoBlade");

	public ParticleSystem ChargeParticle;

	[Header("Test Params")]
	public Boss56SkillType TestSkill;

	public bool DisableBossAction;

	public AnimationActions TestActions;

	public Boss56ActionType TestActionType;

	public float TestSwitchPhaseReplayDelay;

	[Header("Boss State")]
	private Boss56SkillType currentSkill;

	private Boss56State bossState;

	private float unitTimer;

	private float skillTimer;

	private float skillEnterTimer;

	public float BornIdleDuration;

	public float Phase2SkillComboBaseCastInterval;

	private bool isPhase2;

	public float PhaseSwitchDuration;

	public int SummonEliteSkillCountToGunBlade;

	public float SummonEliteHpPercentToGunBlade;

	public float SummonToGunBladeDarkenDuration;

	public float SummonToGunBladeRestoreColorDuration;

	public float SummonToGunBladeAfterIdleDelay;

	public int GunBladeSkillCountToMix;

	public int MixSummonEliteSkillWeight = 1;

	public int MixGunBladeSkillWeight = 1;

	private Boss56ActionType currentActionType = Boss56ActionType.SummonElite;

	private Boss56ActionType currentSkillActionType = Boss56ActionType.SummonElite;

	private Boss56ActionType phaseSwitchTargetActionType;

	public ParticleSystem AngerAura;

	private bool summonEliteHpGunBladeSwitchTriggered;

	private Coroutine summonToGunBladeRoutine;

	private Tween summonToGunBladeColorTween;

	private bool phaseSwitchPreviousCcEnabled;

	private bool phaseSwitchPreviousCanBeTarget;

	private bool phaseSwitchPreviousCanTouch;

	private bool isTestingSummonToGunBladeSwitch;

	private bool isWaitingTestSwitchPhaseReplay;

	private readonly List<Color> bossBodySpriteDefaultColors = new List<Color>();

	[Header("Boss Facing")]
	public Transform ModelTransform;

	private bool isFaceRight = true;

	private bool lockCurrentFaceDirection = true;

	private float modelScaleX = 1f;

	public float FaceDirectionChangeDuration;

	[Header("Move Params")]
	public float NormalMoveSpeed;

	[FormerlySerializedAs("P2WalkSpeed")]
	public float GunBladeMoveSpeed;

	public float MixMoveSpeed;

	private Boss56MoveState moveState;

	public float MinClosePlayerDistance;

	public float RecheckTargetInterval;

	private float castSkillStopMotionTimer;

	[Header("Boss装备")]
	public Transform ScabbardBladeTransform;

	public Transform BeltControllerTransform;

	public Transform LHandBladeTransform;

	public Transform LHandPistolTransform;

	public Transform RHandBladeTransform;

	public Transform RHandControllerTransform;

	[Header("Boss闪烁效果")]
	public float FlashDashStartAt;

	public float DashEndRecoverTime;

	public float FlashDashBodyAlpha;

	public AnimationCurve DashBlurEffectCurve;

	public SpriteRenderer BossFlashSprite;

	public List<SpriteRenderer> BossBodySprites;

	public float DashMaxBlurPower;

	public float DashMaxSpeedLinePower;

	public float SpeedUpStartDistance;

	public float SpeedUpMinRatio;

	private float DashingRemainTimer;

	public Sprite Dash_SlowWalkSprite;

	public Sprite Dash_FastWalkSprite;

	public Sprite Dash_FastWalkBackwardSprite;

	[Header("精英51飞机交叉轰炸")]
	public float TCB_ShootInterval;

	public float TCB_DroneSideDistance;

	public float TCB_FirstCallWaitTime;

	public int TCB_TargetWave;

	private int TCB_WaveCount;

	private int TCB_ShootCount;

	private float TCB_ShootTimer;

	private bool TCB_IsHorizontal;

	public float TCB_DroneSpawnDistance;

	private int TCB_DroneDir;

	private Vector3 TCB_CenterPoint;

	public float TCB_EndSkillBonusWaitTime;

	public float TCB_NewWaveBonusWaitTime;

	private GameObject TCB_NewWaveFirstDrone;

	private float TCB_NewWaveChaseTimer;

	private Vector3 TCB_ToCenterVector;

	public float TCB_BombDamageRange;

	public float TCB_CastSkillStopMotionTime;

	[Header("精英50密集火力网")]
	public float TPB_ShootInterval;

	public float TPB_SpawnDistance;

	public float TPB_StartSummonAt;

	public float TPB_SkillDuration;

	public float TPB_BombDamageRange;

	public float TPB_BombSpawnDistance;

	public float TPB_AfterSkillBonusWaitTime;

	public float TPB_LandTime;

	private List<Boss56_Elite50Bomb> TPB_DroneList = new List<Boss56_Elite50Bomb>();

	private bool TPB_IsCannonSpawn;

	public float TPB_FirstWaveDelayShootInterval;

	public float TPB_SecondWaveBonusDelayShootInterval;

	public float TPB_CastSkillStopMotionTime;

	public float TPB_FirstBombToShooterDistanceRatio;

	[Header("多台精英53旋转飞弹")]
	public float TWR_FirstWaveShootTime;

	public float TWR_SecondsWaveShootTime;

	public float TWR_FinalWaveShootTime;

	private List<Boss56Elite53RotateBall> TWR_DroneList = new List<Boss56Elite53RotateBall>();

	private int TWR_SkillStage;

	private float TWR_ShootTimer;

	public float TWR_FWVerticalDistance;

	public float TWR_SWHorizontalDistance;

	public Vector2 TWR_TWSquareLength;

	public float TWR_TWBottomLineLengthRatio;

	public float TWR_SkillDuration;

	public float TWR_EndSkillBonusWaitTime;

	public float TWR_NormalBallRotateSpeed;

	public float TWR_FinalWaveBallRotateSpeed;

	public float TWR_CastSkillStopMotionTime;

	[Header("精英57洗地")]
	public float TTM_SkillDuration;

	public Vector3 TTM_BossShiftPos;

	public Vector3 TTM_E57ShiftPos;

	public float TTM_MechSpawnAt;

	public float TTM_HMechSpawnRadius;

	public float TTM_HMechSpawnBaseDistanceToCenter;

	public float TTM_HMechSpawnAngleGap;

	[FormerlySerializedAs("TTM_HMechSpawnCount")]
	public int TTM_HMechSideSpawnCount;

	public float TTM_AfterSkillBonusWaitTime;

	private List<Boss56Elite57Shooter> TTM_HMechList = new List<Boss56Elite57Shooter>();

	public float TTM_CWStartShootAt;

	public float TTM_CWShootInterval;

	public float TTM_CWMachShootVisualEffectInterval;

	public float TTM_CWSwitchDirectionInterval;

	public float TTM_CWMaxShiftDistance;

	public float TTM_CWToPlayerBaseTime;

	public float TTM_CWWaveDuration;

	public float TTM_CWWaveMoveSpeed;

	public float TTM_CWWaveShootInterval;

	public float TTM_CWDamageRange;

	public float TTM_CWLandDuration;

	public float TTM_CWMaxInitialShift;

	public float TTM_CastSkillStopMotionTime;

	private float TTM_CWShootTimer;

	private float TTM_CWMachShootTimer;

	private float TTM_CWSwitchSideTimer;

	private bool TTM_CWIsShootFromLeftSide;

	private bool TTM_CWIsEvenAttack;

	private List<(float duration, Vector3 currentPos, Vector3 moveDir, float shootTimer, int shootCount)> TTM_CWDataList = new List<(float, Vector3, Vector3, float, int)>();

	[Header("精英56导弹狂欢（已弃用）")]
	public float TMC_StartSummonAt;

	public float TMC_SkillDuration;

	public float TMC_SplitMissileDogToCenterDistance;

	public float TMC_MaxRotateAngle;

	public float TMC_AngleRotateSpeed;

	public float TMC_SplitMissileShootInterval;

	public float TMC_ShootDuration;

	public float TMC_GiantBombDogToCenterDistance;

	public float TMC_GiantBombToCenterDistance;

	public float TMC_GiantBombDelayStartTime;

	public Vector3 TMC_GiantBombShiftPos;

	public float TMC_AfterSkillBonusWaitTime;

	private bool TMC_IsCSpawn;

	private List<Boss56Elite56ComboShooter> TMC_DroneList = new List<Boss56Elite56ComboShooter>();

	[Header("精英56导弹狂欢")]
	public float TDC_StartSummonAt;

	public float TDC_FistWaveStartTime;

	public float TDC_WaveInterval;

	public List<int> TDC_GiantBombWave;

	public int TDC_ShootTotalWave;

	public float TDC_GiantBombToCenterDistance;

	public float TDC_AfterFinalWaveDelayDestroyDogTime;

	public float TDC_AfterSkillBonusWaitTime;

	public float TDC_MissileMaxAngleShift;

	public float TDC_CastSkillStopMotionTime;

	private int TDC_CurrentWave;

	private float TDC_ShootTimer;

	private List<Boss56Elite56DogShooter> TDC_DogList = new List<Boss56Elite56DogShooter>();

	[Header("精英55 六芒射击")]
	public float THC_FirstWaveTime;

	public float THC_SecondWaveTime;

	public float THC_FinalWaveStartMoveTime;

	public float THC_FinalWaveTime;

	public float THC_AfterFinalWaveDelayEndTime;

	public float THC_FirstWaveDistance;

	public float THC_SecondWaveDistance;

	public float THC_FinalWaveDistance;

	public float THC_AfterSkillBonusWaitTime;

	public float THC_CastSkillStopMotionTime;

	private float THC_SpawnWaveTimer;

	private int THC_WaveCounter;

	private float THC_BaseAngle;

	private List<Boss56E55HexShooter> THC_DroneList = new List<Boss56E55HexShooter>();

	[Header("精英59激光阵")]
	public float TLR_WaveSpawnInterval;

	public int TLR_TotalWaveCount;

	public float TLR_RoadBlockPercent;

	public float TLR_DelayRecycleTowerDuration;

	public float TLR_AfterSkillBonusWaitTime;

	public float TLR_LaserWidth;

	public float TLR_LaserGroundAreaExistTime;

	public float TLR_LaserMoveSpeed;

	public float TLR_LaserLife;

	public float TLR_LaserSpawnInterval;

	public float TLR_LaserDistance;

	public float TLR_CastSkillStopMotionTime;

	private int TLR_CurrentWaveCount;

	private bool TLR_IsStartFromLeft;

	private float TLR_WaveTimer;

	private Boss56Elite59LaserTower TLR_TargetTower;

	[Header("精英58地雷阵")]
	public float TBM_SkillDuration;

	public float TBM_MachSpawnAt;

	public float TBM_MachShootPillarAt;

	public float TBM_PillarPillarMineSetRadius;

	public float TBM_FirstThunderBallDelayShootDuration;

	public float TBM_DelayRecycleMachDuration;

	public float TBM_AfterSkillBonusWaitTime;

	public float TBM_MaxAuraRange;

	public float TBM_AuraExpandDuration;

	public float TBM_ThunderBallShootInterval;

	public float TBM_ThunderBallRotateAngleSpeedUpSpeed;

	public float TBM_ThunderBallRotateMaxAngleSpeed;

	public float TBM_ThunderBallStopRotateAt;

	public int TBM_ThunderBallInitialAnglePerShoot;

	public int TBM_WaveBallCount;

	public int TBM_WaveAngle;

	public float TBM_CastSkillStopMotionTime;

	private int TBM_ThunderBallShootCounter;

	private float TBM_ThunderBallCurrentAngle;

	private bool TBM_IsPillarSpawn;

	private float TBM_ShooTimer;

	private Boss56Elite58ThunderMach TBM_Mach;

	private bool TBM_isEvenSkill = true;

	private static bool TBM_IsClockWise = true;

	private static readonly int UseRemote = Animator.StringToHash("UseRemote");

	private static readonly int CommandBlade = Animator.StringToHash("CommandBlade");

	private static readonly int WalkWithBlade = Animator.StringToHash("WalkWithBlade");

	private static readonly int IdleNoWeapon = Animator.StringToHash("IdleNoWeapon");

	private static readonly int Alpha = Shader.PropertyToID("_Alpha");

	private static readonly int BlurAmount = Shader.PropertyToID("_BlurAmount");

	private static readonly int LineStrength = Shader.PropertyToID("_LineStrength");

	private static readonly int Direction = Shader.PropertyToID("_Direction");

	private bool TBM_IsReadyToEnd;

	[Header("精英52连续压路机")]
	public float TRR_SpawnAt;

	public float TRR_SpawnDistance;

	public float TRR_AfterSpawnDelayAttackTime;

	public float TRR_AfterFinalWaveDelayRecycleMachTime;

	public float TRR_AfterSkillBonusWaitTime;

	public int TRR_TotalAttackWave;

	public float TRR_WaveSpawnInterval;

	public float TRR_SideAttackRadius;

	public float TRR_CenterAttackRadius;

	public float TRR_FinalLastWaveRadius;

	public float TRR_FinalWaveScatter;

	public float TRR_FinalWaveBonusHoverTime;

	public float TRR_CastSkillStopMotionTime;

	private bool TRR_SSIsEvenShoot;

	private bool TRR_UseSideAttack;

	private float TRR_ShootTimer;

	private int TRR_CurrentWaveCount;

	private List<Boss56Elite52RoadRoller> TRR_MachList = new List<Boss56Elite52RoadRoller>();

	[Header("扇形雷区")]
	public float STG_FlashToPlayerDistance;

	public float STG_StartThrowAt;

	public int STG_GrenadeCount;

	public float STG_MaxRange;

	public float STG_MaxScatter;

	public float STG_ExplosionRange;

	public float STG_DamageRangeShrink;

	public float STG_ExplosionDamage;

	public int STG_MinSafeAreaCount;

	public int STG_MaxSafeAreaCount;

	public float STG_SafeAreaRadius;

	public float STG_SafeAreaCenterRandomRadius;

	public float STG_GrenadePointRandomRatio = 0.35f;

	public float STG_BaseExplosionDelayTime;

	public float STG_BonusExplosionDelayPerDistance;

	public float STG_MoveToTargetPointDuration;

	public float STG_AfterThrowEndTime;

	public float STG_SkillEndBonusWaitTime;

	private bool STG_IsGrenadeInitialized;

	[Header("斩一下戳一下再斩一下")]
	public float SDL_FSRadius;

	public float SDL_FSAngle;

	public float SDL_FSDelay;

	public float SDL_FSToPlayerDistance;

	public float SDL_EndFSStartDSDelay;

	public float SDL_DSWidth;

	public float SDL_DSOverPlayerDistance;

	public float SDL_DSSlashDelay;

	public float SDL_EndDSStartSSDelay;

	public float SDL_SSRadius;

	public float SDL_SSAngle;

	public float SDL_SSDelay;

	public float SDL_FSDamage;

	public float SDL_DSDamage;

	public float SDL_SSDamage;

	public float SDL_Knockback;

	public bool SDL_DSExpandFromCenter;

	public float SDL_AfterSkillBonusWaitTime;

	public float SDL_StartFSDelay = 0.1f;

	public float SDL_SectorDamageRangeShrink = 0.05f;

	private int SDL_SkillStage;

	private float SDL_StageTimer;

	private float SDL_FSWarningDelay;

	private Vector3 SDL_FSOrigin;

	private Vector3 SDL_FSDir;

	private Vector3 SDL_DSStart;

	private Vector3 SDL_DSEnd;

	private Vector3 SDL_DSDir;

	private float SDL_DSLength;

	private Vector3 SDL_SSOrigin;

	private Vector3 SDL_SSDir;

	[Header("WM状闪烁散弹+直线连射")]
	public float SDS_FirstWaveShootCount;

	public int SDS_FirstWaveFlashCount;

	public float SDS_FirstWaveTeleportDistance;

	public float SDS_FirstWaveTeleportAngleOffset;

	public float SDS_FirstWaveTeleportRandomOffsetAngle;

	public float SDS_FirstWaveRoomEdgeCheckXDistance;

	public float SDS_FirstWaveRoomEdgeCheckYDistance;

	public float SDS_FirstWaveWWidth;

	public float SDS_FirstWaveWHeight;

	public float SDS_FirstWaveShootSectorAngle;

	public float SDS_FirstWaveBulletSpeed;

	public float SDS_FirstWaveShootDelay;

	public float SDS_FirstWaveInterval;

	public float SDS_SecondWaveTeleportDistance;

	public float SDS_SecondWaveStartDelay;

	public int SDS_SecondWaveWarningCount;

	public float SDS_SecondWaveWarningInterval;

	public float SDS_SecondWaveWarningDuration;

	public float SDS_SecondWaveWarningLockPercent;

	public float SDS_SecondWaveBoxLength;

	public float SDS_SecondWaveBoxWidth;

	public float SDS_SecondWaveBoxDamage;

	public bool SDS_SecondWaveBoxExpandFromCenter;

	public float SDS_BoxKnockback;

	public float SDS_BulletSpawnHeight;

	public float SDS_AfterSkillBonusWaitTime;

	private int SDS_SkillStage;

	private float SDS_StageTimer;

	private int SDS_FirstWaveCounter;

	private bool SDS_FirstWaveShot;

	private float SDS_CurrentDashShootDelay;

	private int SDS_SecondWaveCounter;

	private float SDS_SecondWaveTimer;

	private bool SDS_FirstWaveWRotate180;

	private bool SDS_FirstWaveWStartFromRight;

	private const float SDS_FirstWaveMoveSqrThreshold = 0.0001f;

	private const float SDS_TeleportRoomEdgePadding = 0.75f;

	[Header("叠加圆环斩击加直线攻击")]
	public float SCS_FirstTeleportDistance;

	public float SCS_FirstTeleportMinDistance;

	public float SCS_PlayerEdgeCheckXDistance;

	public float SCS_PlayerEdgeCheckYDistance;

	public int SCS_TeleportPointSampleCount = 12;

	public float SCS_TeleportRoomEdgePadding = 0.75f;

	public float SCS_FirstRingStartDelay;

	public float SCS_RingRadius;

	public float SCS_DonutWidth;

	public float SCS_DonutDamageRangeShrink;

	public float SCS_RingsDistance;

	public float SCS_TotalRingMoveDistance;

	public float SCS_RingSpawnInterval;

	public float SCS_RingWarningDuration;

	public float SCS_RingDamage = 10f;

	public float SCS_StartSecondWaveDelay;

	public float SCS_BoxWarningDuration;

	public float SCS_BoxWarningLockPercent = 1f;

	public float SCS_BoxLength;

	public float SCS_BoxWidth;

	public float SCS_BoxDamage = 10f;

	public float SCS_Knockback;

	public bool SCS_BoxExpandFromCenter;

	public float SCS_AfterSkillBonusWaitTime;

	private bool SCS_FirstWaveWarningFinished;

	private bool SCS_SecondWaveFinished;

	[Header("外圈雷+圆斩")]
	public float SGS_TeleportDistance;

	public int SGS_TeleportPointSampleCount = 12;

	public float SGS_TeleportRoomEdgePadding = 0.75f;

	public float SGS_StartThrowDelay;

	public int SGS_OuterGrenadeCount;

	public float SGS_OuterGrenadeInnerRadius;

	public float SGS_OuterGrenadeOuterRadius;

	public int SGS_InnerGrenadeCount;

	public float SGS_InnerGrenadeRadius;

	public float SGS_GrenadeRange;

	public float SGS_OuterGrenadeRange;

	public float SGS_InnerGrenadeRange;

	public float SGS_GrenadeDamage;

	public float SGS_GrenadeBaseExplosionDelayTime;

	public float SGS_GrenadeBonusExplosionDelayPerDistance;

	public float SGS_GrenadeMoveToTargetPointDuration;

	public float SGS_GrenadeThrowSweepDuration;

	public float SGS_InnerGrenadeFuseTime;

	public float SGS_StartSlashDelay;

	public float SGS_SlashRadius;

	public float SGS_SlashAngle = 270f;

	public float SGS_SlashChargeDuration;

	public float SGS_SlashTurnSpeed;

	public float SGS_SlashTrackDuration;

	public float SGS_SlashDamage;

	public float SGS_SlashKnockback;

	public float SGS_SlashDamageRangeShrink;

	public float SGS_AfterSkillBonusWaitTime;

	private bool SGS_IsSkillFinished;

	[Header("居中散弹+中心十字斩")]
	public int SCC_ThirdWaveTeleportCount;

	public float SCC_ThirdWaveTeleportDistance;

	public float SCC_ThirdWaveShootDelay;

	public float SCC_ThirdWaveInterval;

	public float SCC_ThirdWaveShootCount;

	public float SCC_ThirdWaveShootSectorAngle;

	public float SCC_ThirdWaveBaseAngleScatter;

	public int SCC_ThirdWaveShootThickness;

	public float SCC_ThirdWaveThicknessAngleInterval;

	public float SCC_ThirdWaveBulletSpeed;

	public float SCC_ThirdStartFourthDelay;

	public float SCC_FourthWaveWarningDuration;

	public float SCC_FourthWaveBoxLength;

	public float SCC_FourthWaveBoxWidth;

	public float SCC_FourthWaveBaseAngle;

	public float SCC_FourthWaveBoxDamage;

	public bool SCC_FourthWaveBoxExpandFromCenter;

	public float SCC_BoxKnockback;

	public float SCC_BulletSpawnHeight;

	public float SCC_AfterSkillBonusWaitTime;

	private int SCC_SkillStage;

	private float SCC_StageTimer;

	private int SCC_ThirdWaveCounter;

	private bool SCC_ThirdWaveShot;

	private float SCC_CurrentThirdDashShootDelay;

	private Vector3 SCC_ThirdWaveCenter;

	private bool SCC_FourthWaveWarningCreated;

	private Vector3 SCC_FourthWaveCenter;

	[Header("波浪圆弧斩")]
	public float SWS_TeleportDistance;

	public float SWS_MinTeleportDistance;

	public float SWS_MaxTeleportDistance;

	public float SWS_TeleportAngleOffset;

	public float SWS_StartDelay;

	public float SWS_MaxAdvanceDistance;

	public float SWS_AdvancePerSlash;

	public float SWS_RectWidth;

	public float SWS_BaseWidth;

	public float SWS_FinalWidth;

	public float SWS_ArcOuterRadius;

	public float SWS_RingWidth;

	public float SWS_FirstSlashDelay;

	public float SWS_BonusDelayPerSlash;

	public float SWS_Damage;

	public float SWS_Knockback;

	public float SWS_DamageRangeShrink;

	public float SWS_AfterSkillBonusWaitTime;

	private float SWS_StageTimer;

	private bool SWS_WarningCreated;

	private float SWS_EndDelay;

	[Header("滑步狙击弹 +矩形射击")]
	public int SSB_RepeatCount = 3;

	public int SSB_DashCountPerWave = 1;

	public float SSB_SideRadius;

	public float SSB_AfterPlayerDistance;

	public float SSB_DashSpeed;

	public float SSB_BetweenDashDelay;

	public float SSB_DashShootDistanceInterval;

	public float SSB_DashShootDistanceIntervalDecreasePerWave;

	public float SSB_BulletSpeed;

	public float SSB_BulletSpawnHeight;

	public int SSB_FirstWaveBoxCount;

	public int SSB_BonusBoxCountPerWave;

	public float SSB_BoxSectorAngle;

	public float SSB_BoxLength;

	public float SSB_BoxWidth;

	public float SSB_BoxWarningDuration;

	public float SSB_BoxLockDuration;

	public bool SSB_BoxExpandFromCenter;

	public float SSB_BoxDamage;

	public float SSB_BoxKnockback;

	public float SSB_BoxStartDelayAfterDash;

	public float SSB_BetweenWaveDelay;

	public float SSB_AfterSkillBonusWaitTime;

	private bool SSB_IsSkillFinished;

	private int SSB_DashEffectToken;

	private List<Boss56SkillType> Phase2SummonEliteSkillPool = new List<Boss56SkillType>
	{
		Boss56SkillType.E50BombWave,
		Boss56SkillType.E51CannonWave,
		Boss56SkillType.E53RotateBall,
		Boss56SkillType.E57MissileWave,
		Boss56SkillType.E56MissileChain,
		Boss56SkillType.E55HexAttackCombo,
		Boss56SkillType.E59LaserRoad,
		Boss56SkillType.E58ThunderEnchantment,
		Boss56SkillType.E52BulletRoadRoller
	};

	private int Phase2SummonEliteSkillCounter;

	private List<Boss56SkillType> Phase2GunBladeSkillPool = new List<Boss56SkillType>
	{
		Boss56SkillType.SThrowGrenades,
		Boss56SkillType.SSlashStabSlash,
		Boss56SkillType.SFastDashShoot,
		Boss56SkillType.SStackDonutSlash,
		Boss56SkillType.SGrenadeRingSlash,
		Boss56SkillType.SToCenterCrossSlash,
		Boss56SkillType.SWaveSlash,
		Boss56SkillType.SDashShoot
	};

	private int Phase2GunBladeSkillCounter;

	private int Phase2MixSummonEliteSkillCounter;

	private int Phase2MixGunBladeSkillCounter;

	private int Phase2MixSkillCycleCounter;

	public Entity thisEntity { get; set; }

	public override void EveryInitialCallback()
	{
		Inst = this;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		unitTimer = 0f;
		TCB_IsHorizontal = true;
		skillEnterTimer = 0f;
		Phase2SummonEliteSkillPool = GeneralTool.ListShuffle(Phase2SummonEliteSkillPool);
		Phase2GunBladeSkillPool = GeneralTool.ListShuffle(Phase2GunBladeSkillPool);
		CacheBossBodySpriteDefaultColors();
		Phase2SummonEliteSkillCounter = 0;
		Phase2GunBladeSkillCounter = 0;
		Phase2MixSummonEliteSkillCounter = 0;
		Phase2MixGunBladeSkillCounter = 0;
		Phase2MixSkillCycleCounter = 0;
		currentActionType = Boss56ActionType.SummonElite;
		currentSkillActionType = Boss56ActionType.SummonElite;
		phaseSwitchTargetActionType = Boss56ActionType.None;
		summonEliteHpGunBladeSwitchTriggered = false;
		summonToGunBladeRoutine = null;
		summonToGunBladeColorTween?.Kill();
		summonToGunBladeColorTween = null;
		BossEnterState(Boss56State.BornIdle);
		base.Anima.Play("Idle_NoWeapon", 0, 0f);
		TBM_IsClockWise = UnityEngine.Random.Range(0f, 1f) <= 0.5f;
		castSkillStopMotionTimer = 0f;
		ResetDashEffect();
		isPhase2 = true;
	}

	public override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.B))
		{
			PlayAnimation(TestActions);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			SelfTeleport(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * 4f, Dash_SlowWalkSprite);
		}
		if (DashingRemainTimer > 0f)
		{
			DashingRemainTimer -= Time.deltaTime;
		}
		if (!DisableBossAction)
		{
			UpdateTestActionTypeState();
			UpdateBossState();
			TTM_UpdateShootEffect();
		}
	}

	private void UpdateTestActionTypeState()
	{
		if (TestActionType != Boss56ActionType.SwitchPhase_SummonToGunBlade)
		{
			isTestingSummonToGunBladeSwitch = false;
			isWaitingTestSwitchPhaseReplay = false;
		}
		else if (summonToGunBladeRoutine == null && !isWaitingTestSwitchPhaseReplay)
		{
			StartTestSummonToGunBladeSwitch();
		}
	}

	private void CacheBossBodySpriteDefaultColors()
	{
		bossBodySpriteDefaultColors.Clear();
		if (BossBodySprites == null)
		{
			return;
		}
		foreach (SpriteRenderer bossBodySprite in BossBodySprites)
		{
			bossBodySpriteDefaultColors.Add((bossBodySprite == null) ? Color.white : bossBodySprite.color);
		}
	}

	private void HideAllGears()
	{
		LHandBladeTransform.gameObject.SetActive(value: false);
		LHandPistolTransform.gameObject.SetActive(value: false);
		RHandBladeTransform.gameObject.SetActive(value: false);
		RHandControllerTransform.gameObject.SetActive(value: false);
		ScabbardBladeTransform.gameObject.SetActive(value: true);
		BeltControllerTransform.gameObject.SetActive(value: true);
	}

	private void UpdateTargetGear(CommanderGears targetGear, bool isVisible)
	{
		switch (targetGear)
		{
		case CommanderGears.ScabbardBlade:
			ScabbardBladeTransform.gameObject.SetActive(isVisible);
			break;
		case CommanderGears.LHandBlade:
			LHandBladeTransform.gameObject.SetActive(isVisible);
			break;
		case CommanderGears.LHandPistol:
			LHandPistolTransform.gameObject.SetActive(isVisible);
			break;
		case CommanderGears.RHandBlade:
			RHandBladeTransform.gameObject.SetActive(isVisible);
			break;
		case CommanderGears.Controller:
			BeltControllerTransform.gameObject.SetActive(value: true);
			RHandControllerTransform.gameObject.SetActive(value: false);
			break;
		default:
			throw new ArgumentOutOfRangeException("targetGear", targetGear, null);
		case CommanderGears.None:
			break;
		}
	}

	private void PlayAnimation(AnimationActions targetAnima, float playTime = 0f)
	{
		HideAllGears();
		switch (targetAnima)
		{
		case AnimationActions.WalkNoWeapon:
			base.Anima.Play("Walk_NoBlade");
			break;
		case AnimationActions.WalkWithWeapon:
			base.Anima.Play("Walk");
			UpdateTargetGear(CommanderGears.RHandBlade, isVisible: true);
			UpdateTargetGear(CommanderGears.LHandPistol, isVisible: true);
			break;
		case AnimationActions.UseRemote:
			base.Anima.Play("UseRemote", 0, playTime);
			break;
		case AnimationActions.BladeCommand:
			base.Anima.Play("BladeCMD", 0, playTime);
			break;
		case AnimationActions.IdleNoWeapon:
			base.Anima.SetTrigger(IdleNoWeapon);
			break;
		case AnimationActions.IdleWithWeapon:
			base.Anima.Play("Idle_WithBlade", 0, 0f);
			UpdateTargetGear(CommanderGears.RHandBlade, isVisible: true);
			UpdateTargetGear(CommanderGears.LHandPistol, isVisible: true);
			break;
		}
	}

	private void UpdateBossState()
	{
		switch (bossState)
		{
		case Boss56State.BornIdle:
			unitTimer += Time.deltaTime;
			FaceToPlayer();
			UpdateFaceDirection();
			if (unitTimer >= BornIdleDuration)
			{
				BossEnterState(Boss56State.Battle);
			}
			break;
		case Boss56State.Battle:
			UpdateMoveState();
			UpdateFaceDirection();
			UpdateSkillState();
			UpdateBossP2SkillState();
			break;
		case Boss56State.PhaseSwitch:
			if (!IsSummonToGunBladePhaseSwitch() && !isTestingSummonToGunBladeSwitch)
			{
				unitTimer += Time.deltaTime;
				if (unitTimer >= PhaseSwitchDuration)
				{
					currentActionType = phaseSwitchTargetActionType;
					phaseSwitchTargetActionType = Boss56ActionType.None;
					BossEnterState(Boss56State.Battle);
				}
			}
			break;
		}
	}

	private void CastTargetSkill(Boss56SkillType targetSkill)
	{
		currentSkill = targetSkill;
		castSkillStopMotionTimer = 0f;
		switch (targetSkill)
		{
		case Boss56SkillType.E51CannonWave:
			TCB_ShootTimer = 0f - TCB_FirstCallWaitTime;
			TCB_WaveCount = 0;
			TCB_ShootCount = 0;
			TCB_DroneDir = ((UnityEngine.Random.Range(0f, 1f) <= 0.5f) ? 1 : (-1));
			TCB_NewWaveChaseTimer = TCB_NewWaveBonusWaitTime;
			TCB_NewWaveFirstDrone = null;
			castSkillStopMotionTimer = TCB_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			break;
		case Boss56SkillType.E53RotateBall:
			TWR_DroneList.Clear();
			TWR_SkillStage = 0;
			TWR_ShootTimer = 0f;
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint), Dash_SlowWalkSprite);
			castSkillStopMotionTimer = TWR_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.UseRemote);
			break;
		case Boss56SkillType.E50BombWave:
			TPB_IsCannonSpawn = false;
			TPB_DroneList.Clear();
			castSkillStopMotionTimer = TPB_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			break;
		case Boss56SkillType.E57MissileWave:
			TTM_HMechList.Clear();
			TTM_CWShootTimer = 0f;
			TTM_CWDataList.Clear();
			TTM_CWMachShootTimer = 0f;
			TTM_CWSwitchSideTimer = 0f;
			TTM_CWIsShootFromLeftSide = UnityEngine.Random.Range(0f, 1f) <= 0.5f;
			castSkillStopMotionTimer = TTM_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) + new Vector3(0f, TTM_HMechSpawnBaseDistanceToCenter / 2f, 0f), Dash_SlowWalkSprite);
			break;
		case Boss56SkillType.E56MissileCombo:
			TMC_IsCSpawn = false;
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint), Dash_SlowWalkSprite);
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			TMC_DroneList.Clear();
			break;
		case Boss56SkillType.E56MissileChain:
			TDC_CurrentWave = 0;
			TDC_ShootTimer = 0f - TDC_FistWaveStartTime;
			TDC_DogList.Clear();
			castSkillStopMotionTimer = TDC_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			break;
		case Boss56SkillType.E55HexAttackCombo:
			THC_SpawnWaveTimer = 0f;
			THC_WaveCounter = 0;
			THC_BaseAngle = 0f;
			THC_DroneList.Clear();
			castSkillStopMotionTimer = THC_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.UseRemote);
			break;
		case Boss56SkillType.E59LaserRoad:
			TLR_CurrentWaveCount = 0;
			TLR_IsStartFromLeft = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x >= PlayerMgr.Inst.PlayerPoint.x;
			TLR_WaveTimer = 0f;
			TLR_TargetTower = null;
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - new Vector3(0f, 2f, 0f)), Dash_SlowWalkSprite);
			castSkillStopMotionTimer = TLR_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.UseRemote);
			break;
		case Boss56SkillType.E58ThunderEnchantment:
			TBM_ShooTimer = 0f;
			TBM_IsPillarSpawn = false;
			TBM_Mach = null;
			TBM_ThunderBallShootCounter = 0;
			TBM_ThunderBallCurrentAngle = 0f;
			TBM_IsReadyToEnd = false;
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - new Vector3(0f, 2f, 0f)), Dash_SlowWalkSprite);
			castSkillStopMotionTimer = TBM_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.UseRemote);
			break;
		case Boss56SkillType.E52BulletRoadRoller:
			TRR_UseSideAttack = false;
			TRR_ShootTimer = 0f;
			TRR_CurrentWaveCount = 0;
			TRR_MachList.Clear();
			TRR_SSIsEvenShoot = true;
			castSkillStopMotionTimer = TRR_CastSkillStopMotionTime;
			PlayAnimation(AnimationActions.BladeCommand);
			ChargeParticle.Play();
			break;
		case Boss56SkillType.SThrowGrenades:
		{
			skillTimer = 0f;
			STG_IsGrenadeInitialized = false;
			Vector3 vector = ((PlayerMgr.Inst.PlayerCtrller.CurrentMotion == Vector3.zero) ? Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f)) : (-PlayerMgr.Inst.PlayerCtrller.CurrentMotion.normalized));
			Vector3 startPoint = PlayerMgr.Inst.PlayerPoint + vector * STG_FlashToPlayerDistance;
			SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(startPoint), Dash_SlowWalkSprite);
			break;
		}
		case Boss56SkillType.SSlashStabSlash:
			skillTimer = 0f;
			SDL_SkillStage = 0;
			SDL_StageTimer = 0f;
			SDL_FSDir = GetSafeDirection(base.transform.position - PlayerMgr.Inst.PlayerPoint);
			SDL_FSOrigin = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + SDL_FSDir * SDL_FSToPlayerDistance);
			SDL_FSDir = GetSafeDirection(PlayerMgr.Inst.PlayerPoint - SDL_FSOrigin);
			SDL_FSWarningDelay = SelfTeleport(SDL_FSOrigin, Dash_SlowWalkSprite) + SDL_StartFSDelay;
			break;
		case Boss56SkillType.SFastDashShoot:
			skillTimer = 0f;
			SDS_SkillStage = 0;
			SDS_FirstWaveCounter = 0;
			SDS_SecondWaveCounter = 0;
			SDS_SecondWaveTimer = 0f;
			SDS_FirstWaveWRotate180 = UnityEngine.Random.Range(0f, 1f) <= 0.5f;
			SDS_FirstWaveWStartFromRight = UnityEngine.Random.Range(0f, 1f) <= 0.5f;
			SDS_StartFirstWaveDashShoot();
			break;
		case Boss56SkillType.SStackDonutSlash:
			skillTimer = 0f;
			SCS_FirstWaveWarningFinished = false;
			SCS_SecondWaveFinished = false;
			SCS_StartStackDonutSlash();
			break;
		case Boss56SkillType.SGrenadeRingSlash:
			skillTimer = 0f;
			SGS_IsSkillFinished = false;
			SGS_StartGrenadeRingSlash();
			break;
		case Boss56SkillType.SToCenterCrossSlash:
			skillTimer = 0f;
			SCC_StartToCenterCrossSlash();
			break;
		case Boss56SkillType.SWaveSlash:
			SWS_StartWaveSlash();
			break;
		case Boss56SkillType.SDashShoot:
			SSB_StartDashShoot();
			break;
		default:
			throw new ArgumentOutOfRangeException("targetSkill", targetSkill, null);
		case Boss56SkillType.None:
			break;
		}
		MoveEnterState(Boss56MoveState.SkillMotion);
	}

	private float SelfTeleport(Vector3 targetPoint, Sprite targetSprite)
	{
		foreach (SpriteRenderer bossBodySprite in BossBodySprites)
		{
			bossBodySprite.material.SetFloat(Alpha, FlashDashBodyAlpha);
		}
		Vector3 startPoint = base.transform.position.IgnoreZ();
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		Sprite selfTeleportStartSprite = GetSelfTeleportStartSprite(targetSprite);
		Sprite selfTeleportEndSprite = GetSelfTeleportEndSprite(targetSprite, startPoint, targetPoint.IgnoreZ(), playerPointIgnoreZ);
		float num = Tool2D.IgnoreZDistance(base.transform.position, targetPoint);
		float dashSpeedRatio = 1f;
		bool num2 = DashingRemainTimer > 0f;
		if (num < SpeedUpStartDistance)
		{
			dashSpeedRatio *= num / SpeedUpStartDistance;
		}
		BossFlashSprite.sprite = selfTeleportStartSprite;
		BossFlashSprite.enabled = true;
		Vector3 normalized = (targetPoint - base.transform.position).IgnoreZ().normalized;
		BossFlashSprite.material.SetVector(Direction, new Vector4(normalized.x, normalized.y, 0f, 0f));
		DOVirtual.Float((DashingRemainTimer > 0f) ? 0.5f : 0f, 1f, FlashDashStartAt + DashEndRecoverTime, delegate(float t)
		{
			float num3 = DashBlurEffectCurve.Evaluate(t);
			BossFlashSprite.material.SetFloat(BlurAmount, num3 * DashMaxBlurPower * Mathf.Max(0.4f, dashSpeedRatio));
			BossFlashSprite.material.SetFloat(LineStrength, num3 * DashMaxSpeedLinePower);
		}).SetEase(Ease.Linear).OnComplete(delegate
		{
			BossFlashSprite.material.SetFloat(BlurAmount, 0f);
			BossFlashSprite.material.SetFloat(LineStrength, 0f);
		});
		dashSpeedRatio = Mathf.Max(SpeedUpMinRatio, dashSpeedRatio);
		StartCoroutine(FlashDash(targetPoint, dashSpeedRatio, normalized, selfTeleportEndSprite));
		DashingRemainTimer = (FlashDashStartAt + DashEndRecoverTime) * dashSpeedRatio;
		if (!num2)
		{
			return FlashDashStartAt * dashSpeedRatio;
		}
		return 0f;
	}

	private Sprite GetSelfTeleportStartSprite(Sprite defaultSprite)
	{
		if (GetCurrentTeleportActionType() != Boss56ActionType.GunBlade)
		{
			return defaultSprite;
		}
		return Dash_FastWalkSprite;
	}

	private Sprite GetSelfTeleportEndSprite(Sprite defaultSprite, Vector3 startPoint, Vector3 targetPoint, Vector3 lockedPlayerPoint)
	{
		if (GetCurrentTeleportActionType() != Boss56ActionType.GunBlade)
		{
			return defaultSprite;
		}
		if (!IsSameHorizontalSideOfPlayer(startPoint, targetPoint, lockedPlayerPoint))
		{
			return Dash_FastWalkBackwardSprite;
		}
		return Dash_FastWalkSprite;
	}

	private bool IsSameHorizontalSideOfPlayer(Vector3 startPoint, Vector3 targetPoint, Vector3 playerPoint)
	{
		float f = startPoint.x - playerPoint.x;
		float f2 = targetPoint.x - playerPoint.x;
		if (Mathf.Abs(f) <= 0.001f || Mathf.Abs(f2) <= 0.001f)
		{
			return true;
		}
		return Mathf.Sign(f) == Mathf.Sign(f2);
	}

	private void ResetDashEffect()
	{
		foreach (SpriteRenderer bossBodySprite in BossBodySprites)
		{
			bossBodySprite.material.SetFloat(Alpha, 1f);
		}
		BossFlashSprite.enabled = false;
	}

	private IEnumerator FlashDash(Vector3 targetPoint, float DashSpeedRatio, Vector3 toTargetDir, Sprite endDashSprite)
	{
		yield return new WaitForSeconds((DashingRemainTimer > 0f) ? 0f : (FlashDashStartAt * DashSpeedRatio));
		base.transform.position = targetPoint;
		SyncDotsPosition();
		BossFlashSprite.sprite = endDashSprite;
		BossFlashSprite.material.SetVector(Direction, new Vector4(0f - toTargetDir.x, 0f - toTargetDir.y, 0f, 0f));
		yield return new WaitForSeconds(DashEndRecoverTime * DashSpeedRatio);
		ResetDashEffect();
	}

	private void CreateTeammateTeleportEffect(Vector3 spawnPos, Vector3 direction, float scale)
	{
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_TeleportJump", spawnPos.IgnoreZ() + new Vector3(0f, 0f, -0.8f), 0.6f);
		gO.transform.right = direction;
		gO.transform.localScale = Vector3.one * scale;
	}

	private void EndCurrentCastingSkill()
	{
		skillEnterTimer = 0f;
		switch (currentSkill)
		{
		case Boss56SkillType.E51CannonWave:
			skillEnterTimer -= TCB_EndSkillBonusWaitTime;
			break;
		case Boss56SkillType.E53RotateBall:
			skillEnterTimer -= TWR_EndSkillBonusWaitTime;
			break;
		case Boss56SkillType.E50BombWave:
			skillEnterTimer -= TPB_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.E57MissileWave:
			skillEnterTimer -= TTM_AfterSkillBonusWaitTime;
			foreach (Boss56Elite57Shooter tTM_HMech in TTM_HMechList)
			{
				CreateTeammateTeleportEffect(tTM_HMech.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3((!(tTM_HMech.transform.position.x <= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x)) ? 1 : (-1), 0f, 0f), 1.2f);
				ObjPoolMgr.Inst.RecycleGO(tTM_HMech.gameObject);
			}
			break;
		case Boss56SkillType.E56MissileCombo:
			skillEnterTimer -= TMC_AfterSkillBonusWaitTime;
			foreach (Boss56Elite56ComboShooter tMC_Drone in TMC_DroneList)
			{
				ObjPoolMgr.Inst.RecycleGO(tMC_Drone.gameObject);
			}
			break;
		case Boss56SkillType.E56MissileChain:
			skillEnterTimer -= TDC_AfterSkillBonusWaitTime;
			foreach (Boss56Elite56DogShooter tDC_Dog in TDC_DogList)
			{
				CreateTeammateTeleportEffect(tDC_Dog.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3((tDC_Dog.transform.position.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x) ? 1 : (-1), 0f, 0f), 1.2f);
				ObjPoolMgr.Inst.RecycleGO(tDC_Dog.gameObject);
			}
			break;
		case Boss56SkillType.E55HexAttackCombo:
			foreach (Boss56E55HexShooter tHC_Drone in THC_DroneList)
			{
				CreateTeammateTeleportEffect(tHC_Drone.transform.position + new Vector3(0f, 0.3f, 0f), new Vector3((tHC_Drone.transform.position.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x) ? 1 : (-1), 0f, 0f), 1f);
				ObjPoolMgr.Inst.RecycleGO(tHC_Drone.gameObject);
			}
			skillEnterTimer -= THC_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.E59LaserRoad:
			skillEnterTimer -= TLR_AfterSkillBonusWaitTime;
			CreateTeammateTeleportEffect(TLR_TargetTower.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3((TLR_TargetTower.transform.position.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x) ? 1 : (-1), 0f, 0f), 1.2f);
			ObjPoolMgr.Inst.RecycleGO(TLR_TargetTower.gameObject);
			break;
		case Boss56SkillType.E58ThunderEnchantment:
			skillEnterTimer -= TBM_AfterSkillBonusWaitTime;
			TBM_IsClockWise = !TBM_IsClockWise;
			CreateTeammateTeleportEffect(TBM_Mach.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3((TBM_Mach.transform.position.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x) ? 1 : (-1), 0f, 0f), 1.2f);
			ObjPoolMgr.Inst.RecycleGO(TBM_Mach.gameObject);
			break;
		case Boss56SkillType.E52BulletRoadRoller:
		{
			skillEnterTimer -= TRR_AfterSkillBonusWaitTime;
			float num = UnityEngine.Random.Range(0f, 360f);
			foreach (Boss56Elite52RoadRoller tRR_Mach in TRR_MachList)
			{
				CreateTeammateTeleportEffect(tRR_Mach.transform.position + new Vector3(0f, 0.35f, 0f), Tool2D.GetDir(num), 1.1f);
				ObjPoolMgr.Inst.RecycleGO(tRR_Mach.gameObject);
				num += 120f;
			}
			break;
		}
		case Boss56SkillType.SSlashStabSlash:
			skillEnterTimer -= SDL_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SFastDashShoot:
			skillEnterTimer -= SDS_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SStackDonutSlash:
			skillEnterTimer -= SCS_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SGrenadeRingSlash:
			skillEnterTimer -= SGS_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SToCenterCrossSlash:
			skillEnterTimer -= SCC_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SWaveSlash:
			skillEnterTimer -= SWS_AfterSkillBonusWaitTime;
			break;
		case Boss56SkillType.SDashShoot:
			skillEnterTimer -= SSB_AfterSkillBonusWaitTime;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Boss56SkillType.None:
		case Boss56SkillType.SThrowGrenades:
			break;
		}
		skillTimer = 0f;
		currentSkill = Boss56SkillType.None;
		MoveEnterState(Boss56MoveState.Idle);
	}

	private void BossEnterState(Boss56State state)
	{
		bossState = state;
		switch (state)
		{
		case Boss56State.BornIdle:
			isFaceRight = true;
			lockCurrentFaceDirection = false;
			modelScaleX = ModelTransform.localScale.x;
			break;
		case Boss56State.PhaseSwitch:
			unitTimer = 0f;
			currentSkill = Boss56SkillType.None;
			MoveEnterState(Boss56MoveState.Idle);
			if (IsSummonToGunBladePhaseSwitch() && summonToGunBladeRoutine == null)
			{
				summonToGunBladeRoutine = StartCoroutine(SummonToGunBladePhaseSwitchRoutine(isTestLoop: false));
			}
			break;
		case Boss56State.Battle:
			skillEnterTimer = 0f;
			break;
		}
	}

	private void UpdateSkillState()
	{
		if (!isPhase2)
		{
			return;
		}
		UpdateBattleActionTransition();
		if (bossState == Boss56State.Battle && currentSkill == Boss56SkillType.None)
		{
			skillEnterTimer += Time.deltaTime;
			if (skillEnterTimer >= Phase2SkillComboBaseCastInterval)
			{
				BattleCastSkill();
			}
		}
	}

	private IEnumerator TWR_DelayPlayAnimation(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		MoveEnterState(Boss56MoveState.SkillMotion);
		castSkillStopMotionTimer = TWR_CastSkillStopMotionTime;
		PlayAnimation(AnimationActions.BladeCommand);
		ChargeParticle.Play();
	}

	private void BattleCastSkill()
	{
		if (TestSkill != 0 && (Phase2SummonEliteSkillPool.Contains(TestSkill) || Phase2GunBladeSkillPool.Contains(TestSkill)))
		{
			currentSkillActionType = GetActionTypeBySkill(TestSkill);
			CastTargetSkill(TestSkill);
			return;
		}
		switch (GetCurrentActionType())
		{
		case Boss56ActionType.SummonElite:
			CastSkillFromPool(ref Phase2SummonEliteSkillPool, ref Phase2SummonEliteSkillCounter, Boss56ActionType.SummonElite);
			break;
		case Boss56ActionType.GunBlade:
			CastSkillFromPool(ref Phase2GunBladeSkillPool, ref Phase2GunBladeSkillCounter, Boss56ActionType.GunBlade);
			break;
		case Boss56ActionType.Mix:
			CastMixSkill();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Boss56ActionType.None:
		case Boss56ActionType.SwitchPhase_SummonToGunBlade:
		case Boss56ActionType.SwitchPhase_GunBladeToMix:
			break;
		}
	}

	private Boss56ActionType GetCurrentActionType()
	{
		if (TestActionType != 0)
		{
			return TestActionType;
		}
		return currentActionType;
	}

	private Boss56ActionType GetCurrentTeleportActionType()
	{
		if (GetCurrentActionType() != Boss56ActionType.Mix)
		{
			return GetCurrentActionType();
		}
		return currentSkillActionType;
	}

	private float GetCurrentMoveSpeed()
	{
		return GetCurrentActionType() switch
		{
			Boss56ActionType.GunBlade => GunBladeMoveSpeed, 
			Boss56ActionType.Mix => MixMoveSpeed, 
			_ => NormalMoveSpeed, 
		};
	}

	private AnimationActions GetCurrentWalkAnimation()
	{
		if (GetCurrentActionType() != Boss56ActionType.GunBlade)
		{
			return AnimationActions.WalkNoWeapon;
		}
		return AnimationActions.WalkWithWeapon;
	}

	private void CastSkillFromPool(ref List<Boss56SkillType> skillPool, ref int skillCounter, Boss56ActionType skillActionType)
	{
		if (skillPool != null && skillPool.Count > 0)
		{
			if (skillCounter >= skillPool.Count)
			{
				ShuffleSkillPoolForNextCycle(ref skillPool);
				skillCounter = 0;
			}
			currentSkillActionType = skillActionType;
			CastTargetSkill(skillPool[skillCounter]);
			skillCounter++;
		}
	}

	private void CastMixSkill()
	{
		int num = Mathf.Max(0, MixSummonEliteSkillWeight);
		int num2 = Mathf.Max(0, MixGunBladeSkillWeight);
		int num3 = num + num2;
		if (num3 <= 0)
		{
			num = 1;
			num2 = 1;
			num3 = 2;
		}
		int num4 = Phase2MixSkillCycleCounter % num3;
		Phase2MixSkillCycleCounter++;
		if ((num <= 0 || num4 >= num) && num2 > 0)
		{
			CastSkillFromPool(ref Phase2GunBladeSkillPool, ref Phase2MixGunBladeSkillCounter, Boss56ActionType.GunBlade);
		}
		else
		{
			CastSkillFromPool(ref Phase2SummonEliteSkillPool, ref Phase2MixSummonEliteSkillCounter, Boss56ActionType.SummonElite);
		}
	}

	private Boss56ActionType GetActionTypeBySkill(Boss56SkillType targetSkill)
	{
		if (Phase2GunBladeSkillPool.Contains(targetSkill))
		{
			return Boss56ActionType.GunBlade;
		}
		if (Phase2SummonEliteSkillPool.Contains(targetSkill))
		{
			return Boss56ActionType.SummonElite;
		}
		return GetCurrentActionType();
	}

	private void ShuffleSkillPoolForNextCycle(ref List<Boss56SkillType> skillPool)
	{
		if (skillPool == null || skillPool.Count <= 1)
		{
			return;
		}
		List<Boss56SkillType> obj = skillPool;
		Boss56SkillType boss56SkillType = obj[obj.Count - 1];
		skillPool = GeneralTool.ListShuffle(skillPool);
		if (skillPool.Count <= 1 || skillPool[0] != boss56SkillType)
		{
			return;
		}
		for (int i = 1; i < skillPool.Count; i++)
		{
			if (skillPool[i] != boss56SkillType)
			{
				List<Boss56SkillType> obj2 = skillPool;
				List<Boss56SkillType> list = skillPool;
				int index = i;
				Boss56SkillType boss56SkillType2 = skillPool[i];
				Boss56SkillType boss56SkillType3 = skillPool[0];
				Boss56SkillType boss56SkillType5 = (obj2[0] = boss56SkillType2);
				boss56SkillType5 = (list[index] = boss56SkillType3);
				break;
			}
		}
	}

	private void StartPhaseSwitch(Boss56ActionType targetActionType)
	{
		phaseSwitchTargetActionType = targetActionType;
		BossEnterState(Boss56State.PhaseSwitch);
	}

	private void StartTestSummonToGunBladeSwitch()
	{
		isTestingSummonToGunBladeSwitch = true;
		isWaitingTestSwitchPhaseReplay = false;
		currentActionType = Boss56ActionType.SummonElite;
		phaseSwitchTargetActionType = Boss56ActionType.GunBlade;
		currentSkill = Boss56SkillType.None;
		skillTimer = 0f;
		skillEnterTimer = 0f;
		MoveEnterState(Boss56MoveState.Idle);
		HideAllGears();
		base.Anima.Play("Idle_NoWeapon", 0, 0f);
		RestoreBossBodySpriteColorsForTest();
		bossState = Boss56State.PhaseSwitch;
		summonToGunBladeRoutine = StartCoroutine(SummonToGunBladePhaseSwitchRoutine(isTestLoop: true));
	}

	private bool IsSummonToGunBladePhaseSwitch()
	{
		return phaseSwitchTargetActionType == Boss56ActionType.GunBlade;
	}

	private IEnumerator SummonToGunBladePhaseSwitchRoutine(bool isTestLoop)
	{
		SetMove(Vector3.zero, instantLerp: true);
		FaceToPlayer();
		UpdateFaceDirection();
		RegisterSummonToGunBladeProtection();
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		SelfTeleport(navMeshPointIngoreZ, Dash_SlowWalkSprite);
		if (DashingRemainTimer > 0f)
		{
			yield return new WaitForSeconds(DashingRemainTimer);
		}
		SetMove(Vector3.zero, instantLerp: true);
		base.Anima.Play("Idle_SummonToGunBlade", 0, 0f);
		yield return PlaySummonToGunBladeBodyColorRoutine();
		AngerAura?.Play();
		PlayAnimation(AnimationActions.IdleWithWeapon);
		if (SummonToGunBladeAfterIdleDelay > 0f)
		{
			yield return new WaitForSeconds(SummonToGunBladeAfterIdleDelay);
		}
		summonToGunBladeRoutine = null;
		RestoreSummonToGunBladeProtection();
		if (isTestLoop && TestActionType == Boss56ActionType.SwitchPhase_SummonToGunBlade)
		{
			phaseSwitchTargetActionType = Boss56ActionType.None;
			isWaitingTestSwitchPhaseReplay = true;
			yield return new WaitForSeconds(Mathf.Max(0f, TestSwitchPhaseReplayDelay));
			isWaitingTestSwitchPhaseReplay = false;
			if (TestActionType == Boss56ActionType.SwitchPhase_SummonToGunBlade)
			{
				StartTestSummonToGunBladeSwitch();
			}
		}
		else
		{
			currentActionType = Boss56ActionType.GunBlade;
			phaseSwitchTargetActionType = Boss56ActionType.None;
			BossEnterState(Boss56State.Battle);
		}
	}

	private void RestoreBossBodySpriteColorsForTest()
	{
		summonToGunBladeColorTween?.Kill();
		summonToGunBladeColorTween = null;
		if (BossBodySprites == null)
		{
			return;
		}
		for (int i = 0; i < BossBodySprites.Count; i++)
		{
			SpriteRenderer spriteRenderer = BossBodySprites[i];
			if (!(spriteRenderer == null))
			{
				spriteRenderer.color = ((i < bossBodySpriteDefaultColors.Count) ? bossBodySpriteDefaultColors[i] : Color.white);
			}
		}
	}

	private IEnumerator PlaySummonToGunBladeBodyColorRoutine()
	{
		summonToGunBladeColorTween?.Kill();
		int num = BossBodySprites?.Count ?? 0;
		if (num <= 0)
		{
			yield break;
		}
		List<SpriteRenderer> sprites = new List<SpriteRenderer>(num);
		List<Color> sourceColors = new List<Color>(num);
		foreach (SpriteRenderer bossBodySprite in BossBodySprites)
		{
			if (!(bossBodySprite == null))
			{
				sprites.Add(bossBodySprite);
				sourceColors.Add(bossBodySprite.color);
			}
		}
		if (sprites.Count <= 0)
		{
			yield break;
		}
		float duration = Mathf.Max(0.0001f, SummonToGunBladeDarkenDuration);
		summonToGunBladeColorTween = DOVirtual.Float(0f, 1f, duration, delegate(float t)
		{
			for (int k = 0; k < sprites.Count; k++)
			{
				sprites[k].color = Color.Lerp(sourceColors[k], Color.black, t);
			}
		}).SetEase(Ease.Linear);
		yield return summonToGunBladeColorTween.WaitForCompletion();
		float duration2 = Mathf.Max(0.0001f, SummonToGunBladeRestoreColorDuration);
		summonToGunBladeColorTween = DOVirtual.Float(0f, 1f, duration2, delegate(float t)
		{
			for (int j = 0; j < sprites.Count; j++)
			{
				sprites[j].color = Color.Lerp(Color.black, sourceColors[j], t);
			}
		}).SetEase(Ease.Linear);
		yield return summonToGunBladeColorTween.WaitForCompletion();
		for (int i = 0; i < sprites.Count; i++)
		{
			sprites[i].color = sourceColors[i];
		}
		summonToGunBladeColorTween = null;
	}

	private void RegisterSummonToGunBladeProtection()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		phaseSwitchPreviousCcEnabled = base.CC_Self.enabled;
		phaseSwitchPreviousCanBeTarget = componentData.CanBeTarget;
		phaseSwitchPreviousCanTouch = componentData.CanTouch;
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		componentData.InvincibleRegister();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	private void RestoreSummonToGunBladeProtection()
	{
		base.CC_Self.enabled = phaseSwitchPreviousCcEnabled;
		SetDotsCCEnable(phaseSwitchPreviousCcEnabled);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleUnregister();
		componentData.CanBeTarget = phaseSwitchPreviousCanBeTarget;
		componentData.CanTouch = phaseSwitchPreviousCanTouch;
		SetComponentData(componentData);
	}

	private void UpdateBattleActionTransition()
	{
		if (currentSkill != 0 || TestActionType != 0)
		{
			return;
		}
		switch (currentActionType)
		{
		case Boss56ActionType.SummonElite:
			if (ShouldSwitchFromSummonEliteToGunBlade())
			{
				StartPhaseSwitch(Boss56ActionType.GunBlade);
			}
			break;
		case Boss56ActionType.GunBlade:
			if (ShouldSwitchFromGunBladeToMix())
			{
				StartGunBladeToMixPhaseSwitch();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Boss56ActionType.None:
		case Boss56ActionType.Mix:
			break;
		}
	}

	private bool ShouldSwitchFromSummonEliteToGunBlade()
	{
		if (IsSummonEliteHpBelowGunBladeSwitchPercent())
		{
			summonEliteHpGunBladeSwitchTriggered = true;
		}
		int num = ((SummonEliteSkillCountToGunBlade > 0) ? SummonEliteSkillCountToGunBlade : Phase2SummonEliteSkillPool.Count);
		if (num <= 0 || Phase2SummonEliteSkillCounter < num)
		{
			return summonEliteHpGunBladeSwitchTriggered;
		}
		return true;
	}

	private bool IsSummonEliteHpBelowGunBladeSwitchPercent()
	{
		if (summonEliteHpGunBladeSwitchTriggered || SummonEliteHpPercentToGunBlade <= 0f)
		{
			return false;
		}
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (componentData.unitCfg.maxHP <= 0f)
		{
			return false;
		}
		float num = ((SummonEliteHpPercentToGunBlade > 1f) ? (SummonEliteHpPercentToGunBlade / 100f) : SummonEliteHpPercentToGunBlade);
		return componentData.unitCfg.currentHP / componentData.unitCfg.maxHP <= num;
	}

	private bool ShouldSwitchFromGunBladeToMix()
	{
		int num = ((GunBladeSkillCountToMix > 0) ? GunBladeSkillCountToMix : Phase2GunBladeSkillPool.Count);
		if (num > 0)
		{
			return Phase2GunBladeSkillCounter >= num;
		}
		return false;
	}

	private void StartGunBladeToMixPhaseSwitch()
	{
		phaseSwitchTargetActionType = Boss56ActionType.None;
		currentActionType = Boss56ActionType.Mix;
		currentSkillActionType = Boss56ActionType.SummonElite;
		Phase2MixSummonEliteSkillCounter = 0;
		Phase2MixGunBladeSkillCounter = 0;
		Phase2MixSkillCycleCounter = 0;
		MoveEnterState(Boss56MoveState.Idle);
	}

	private void UpdateBossP2SkillState()
	{
		skillTimer += Time.deltaTime;
		switch (currentSkill)
		{
		case Boss56SkillType.E51CannonWave:
			if (TCB_WaveCount >= TCB_TargetWave)
			{
				EndCurrentCastingSkill();
				break;
			}
			TCB_ShootTimer += Time.deltaTime;
			if (TCB_ShootTimer >= TCB_ShootInterval)
			{
				TCB_ShootTimer -= TCB_ShootInterval;
				if (TCB_ShootCount == 0)
				{
					TCB_CenterPoint = PlayerMgr.Inst.PlayerPoint;
				}
				Vector3 vector19 = new Vector3(TCB_IsHorizontal ? 1 : 0, (!TCB_IsHorizontal) ? 1 : 0, 0f) * TCB_DroneDir;
				Vector3 vector20 = TCB_CenterPoint + Tool2D.GetDir(vector19, 90f) * TCB_ShootCount * TCB_DroneSideDistance + vector19 * TCB_DroneSpawnDistance;
				TCB_ToCenterVector = vector19 * TCB_DroneSpawnDistance;
				CreateTeammateTeleportEffect(vector20 + new Vector3(0f, 0.5f, 0f), vector19, 1.5f);
				Boss56Elite51Bomb component12 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_Bombardment", vector20).GetComponent<Boss56Elite51Bomb>();
				component12.InitialData(myPpt.myEntity, -vector19, (TCB_ShootCount == 0) ? TCB_NewWaveBonusWaitTime : 0f, TCB_BombDamageRange);
				if (TCB_ShootCount == 0)
				{
					TCB_NewWaveFirstDrone = component12.gameObject;
				}
				if (TCB_ShootCount != 0)
				{
					vector19 = new Vector3(TCB_IsHorizontal ? 1 : 0, (!TCB_IsHorizontal) ? 1 : 0, 0f) * TCB_DroneDir;
					vector20 = TCB_CenterPoint + Tool2D.GetDir(vector19, -90f) * TCB_ShootCount * TCB_DroneSideDistance + vector19 * TCB_DroneSpawnDistance;
					CreateTeammateTeleportEffect(vector20 + new Vector3(0f, 0.5f, 0f), vector19, 1.5f);
					component12 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_Bombardment", vector20).GetComponent<Boss56Elite51Bomb>();
					component12.InitialData(myPpt.myEntity, -vector19, 0f, TCB_BombDamageRange);
				}
				TCB_ShootCount++;
				TCB_DroneDir *= -1;
				if (TCB_ShootCount >= 2)
				{
					TCB_ShootCount = 0;
					TCB_WaveCount++;
					TCB_IsHorizontal = !TCB_IsHorizontal;
				}
			}
			break;
		case Boss56SkillType.E53RotateBall:
			TWR_ShootTimer += Time.deltaTime;
			if (TWR_SkillStage == 0 && TWR_ShootTimer >= TWR_FirstWaveShootTime)
			{
				TWR_ShootTimer -= TWR_FirstWaveShootTime;
				Vector3 vector6 = new Vector3(0f, TWR_FWVerticalDistance, 0f);
				Boss56Elite53RotateBall component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_RingWalker", base.transform.position + vector6).GetComponent<Boss56Elite53RotateBall>();
				component3.InitializeData(myPpt.myEntity, isClockWiseRotate: true, TWR_NormalBallRotateSpeed);
				TWR_DroneList.Add(component3);
				CreateTeammateTeleportEffect(base.transform.position + vector6, -vector6, 1f);
				vector6 = new Vector3(0f, 0f - TWR_FWVerticalDistance, 0f);
				component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_RingWalker", base.transform.position + vector6).GetComponent<Boss56Elite53RotateBall>();
				component3.InitializeData(myPpt.myEntity, isClockWiseRotate: false, TWR_NormalBallRotateSpeed);
				TWR_DroneList.Add(component3);
				CreateTeammateTeleportEffect(base.transform.position + vector6, -vector6, 1f);
				TWR_SkillStage++;
			}
			if (TWR_SkillStage == 1 && TWR_ShootTimer >= TWR_SecondsWaveShootTime)
			{
				TWR_ShootTimer -= TWR_SecondsWaveShootTime;
				PlayAnimation(AnimationActions.UseRemote);
				Vector3 vector7 = new Vector3(TWR_SWHorizontalDistance, 0f, 0f);
				Boss56Elite53RotateBall component4 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_RingWalker", base.transform.position + vector7).GetComponent<Boss56Elite53RotateBall>();
				component4.InitializeData(myPpt.myEntity, isClockWiseRotate: true, TWR_NormalBallRotateSpeed);
				TWR_DroneList.Add(component4);
				CreateTeammateTeleportEffect(base.transform.position + vector7, -vector7, 1f);
				vector7 = new Vector3(0f - TWR_SWHorizontalDistance, 0f, 0f);
				component4 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_RingWalker", base.transform.position + vector7).GetComponent<Boss56Elite53RotateBall>();
				component4.InitializeData(myPpt.myEntity, isClockWiseRotate: false, TWR_NormalBallRotateSpeed);
				TWR_DroneList.Add(component4);
				CreateTeammateTeleportEffect(base.transform.position + vector7, -vector7, 1f);
				TWR_SkillStage++;
			}
			if (TWR_SkillStage == 2 && TWR_ShootTimer >= TWR_FinalWaveShootTime)
			{
				SelfTeleport(Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(0f, 3f, 0f)), Dash_SlowWalkSprite);
				StartCoroutine(TWR_DelayPlayAnimation(0.4f));
				for (int num8 = 0; num8 < TWR_DroneList.Count; num8++)
				{
					Boss56Elite53RotateBall boss56Elite53RotateBall = TWR_DroneList[num8];
					Vector3 position = base.transform.position;
					switch (num8)
					{
					case 0:
						position += new Vector3((0f - TWR_TWSquareLength.x) / 2f, TWR_TWSquareLength.y / 2f, 0f);
						boss56Elite53RotateBall.ApplyOneShootCount(isClockWise: true, TWR_FinalWaveBallRotateSpeed);
						break;
					case 1:
						position += new Vector3(TWR_TWSquareLength.x / 2f, TWR_TWSquareLength.y / 2f, 0f);
						boss56Elite53RotateBall.ApplyOneShootCount(isClockWise: false, TWR_FinalWaveBallRotateSpeed);
						break;
					case 2:
						position += new Vector3((0f - TWR_TWSquareLength.x) / 2f * TWR_TWBottomLineLengthRatio, (0f - TWR_TWSquareLength.y) / 2f, 0f);
						boss56Elite53RotateBall.ApplyOneShootCount(isClockWise: true, TWR_FinalWaveBallRotateSpeed);
						break;
					case 3:
						position += new Vector3(TWR_TWSquareLength.x / 2f * TWR_TWBottomLineLengthRatio, (0f - TWR_TWSquareLength.y) / 2f, 0f);
						boss56Elite53RotateBall.ApplyOneShootCount(isClockWise: false, TWR_FinalWaveBallRotateSpeed);
						break;
					}
					boss56Elite53RotateBall.transform.DOMove(position, 0.5f);
				}
				TWR_SkillStage++;
			}
			if (TWR_SkillStage != 3 || !(TWR_ShootTimer >= TWR_SkillDuration))
			{
				break;
			}
			foreach (Boss56Elite53RotateBall tWR_Drone in TWR_DroneList)
			{
				CreateTeammateTeleportEffect(tWR_Drone.transform.position, (tWR_Drone.transform.position - Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter)).normalized, 1f);
				ObjPoolMgr.Inst.RecycleGO(tWR_Drone.gameObject);
			}
			TWR_DroneList.Clear();
			EndCurrentCastingSkill();
			break;
		case Boss56SkillType.E50BombWave:
			if (skillTimer <= TPB_StartSummonAt)
			{
				break;
			}
			if (!TPB_IsCannonSpawn)
			{
				TPB_IsCannonSpawn = true;
				Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				float num2 = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
				int num3 = Mathf.CeilToInt((float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height / 2f / TPB_SpawnDistance);
				bool flag = PlayerMgr.Inst.PlayerPoint.x >= centerPoint.x;
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num3; k++)
					{
						Vector3 vector2 = centerPoint + new Vector3(num2 / 2f * (float)(flag ? 1 : (-1)), TPB_SpawnDistance * (float)k, 0f);
						Boss56_Elite50Bomb component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_ProjectileBoomber", vector2).GetComponent<Boss56_Elite50Bomb>();
						CreateTeammateTeleportEffect(vector2, new Vector3(flag ? 1 : (-1), 0f, 0f), 1f);
						component.transform.localScale = new Vector3((!flag) ? 1 : (-1), 1f, 1f);
						List<Vector3> list2 = new List<Vector3>();
						int num4 = Mathf.FloorToInt(num2 / 2f / TPB_BombSpawnDistance) + 1;
						for (int l = 0; l <= num4; l++)
						{
							list2.Add(vector2 + new Vector3((0f - TPB_BombSpawnDistance) * 2f * ((float)l + TPB_FirstBombToShooterDistanceRatio) * (float)(flag ? 1 : (-1)), 0f, 0f));
						}
						component.InitialData(myPpt.myEntity, list2, TPB_ShootInterval, TPB_LandTime, TPB_BombDamageRange, TPB_FirstWaveDelayShootInterval + TPB_SecondWaveBonusDelayShootInterval * (float)j);
						TPB_DroneList.Add(component);
						if (k != 0)
						{
							vector2 = centerPoint + new Vector3(num2 / 2f * (float)(flag ? 1 : (-1)), (0f - TPB_SpawnDistance) * (float)k, 0f);
							component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_ProjectileBoomber", vector2).GetComponent<Boss56_Elite50Bomb>();
							CreateTeammateTeleportEffect(vector2, new Vector3((!flag) ? 1 : (-1), 0f, 0f), 1f);
							component.transform.localScale = new Vector3((!flag) ? 1 : (-1), 1f, 1f);
							list2 = new List<Vector3>();
							for (int m = 0; m <= num4; m++)
							{
								list2.Add(vector2 + new Vector3((0f - TPB_BombSpawnDistance) * 2f * ((float)m + TPB_FirstBombToShooterDistanceRatio) * (float)(flag ? 1 : (-1)), 0f, 0f));
							}
							component.InitialData(myPpt.myEntity, list2, TPB_ShootInterval, TPB_LandTime, TPB_BombDamageRange, TPB_FirstWaveDelayShootInterval + TPB_SecondWaveBonusDelayShootInterval * (float)j);
							TPB_DroneList.Add(component);
						}
					}
					flag = !flag;
				}
			}
			if (!(skillTimer >= TPB_SkillDuration))
			{
				break;
			}
			foreach (Boss56_Elite50Bomb tPB_Drone in TPB_DroneList)
			{
				CreateTeammateTeleportEffect(tPB_Drone.transform.position, new Vector3((tPB_Drone.transform.position.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x) ? 1 : (-1), 0f, 0f), 1f);
				ObjPoolMgr.Inst.RecycleGO(tPB_Drone.gameObject);
			}
			TPB_DroneList.Clear();
			EndCurrentCastingSkill();
			break;
		case Boss56SkillType.E57MissileWave:
			if (skillTimer <= TTM_MechSpawnAt)
			{
				break;
			}
			if (skillTimer >= TTM_SkillDuration)
			{
				EndCurrentCastingSkill();
				break;
			}
			if (TTM_HMechList.Count <= 0)
			{
				Vector3 vector8 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + TTM_E57ShiftPos - new Vector3(0f, TTM_HMechSpawnRadius, 0f);
				for (int num9 = 0; num9 < TTM_HMechSideSpawnCount; num9++)
				{
					Vector3 vector9 = vector8 + Tool2D.GetDir(TTM_HMechSpawnAngleGap * (0.5f + (float)num9)) * TTM_HMechSpawnRadius;
					Boss56Elite57Shooter component5 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_TinymissileShooter", vector9).GetComponent<Boss56Elite57Shooter>();
					CreateTeammateTeleportEffect(vector9 + new Vector3(0f, 0.5f, 0f), new Vector3((!(vector9.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x)) ? 1 : (-1), 0f, 0f), 1.2f);
					component5.Initialdata(myPpt.myEntity);
					TTM_HMechList.Insert(0, component5);
					vector9 = vector8 + Tool2D.GetDir(TTM_HMechSpawnAngleGap * (-0.5f - (float)num9)) * TTM_HMechSpawnRadius;
					component5 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_TinymissileShooter", vector9).GetComponent<Boss56Elite57Shooter>();
					CreateTeammateTeleportEffect(vector9 + new Vector3(0f, 0.5f, 0f), new Vector3((!(vector9.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x)) ? 1 : (-1), 0f, 0f), 1.2f);
					component5.Initialdata(myPpt.myEntity);
					TTM_HMechList.Add(component5);
				}
			}
			if (skillTimer <= TTM_MechSpawnAt + TTM_CWStartShootAt)
			{
				break;
			}
			TTM_CWMachShootTimer += Time.deltaTime;
			if (TTM_CWMachShootTimer >= TTM_CWMachShootVisualEffectInterval)
			{
				TTM_CWMachShootTimer -= TTM_CWMachShootVisualEffectInterval;
				for (int num10 = 0; num10 < 6; num10++)
				{
					TTM_HMechList[num10].CastVerticalAttack(Vector3.zero, Vector3.zero, 0f);
				}
			}
			TTM_CWShootTimer += Time.deltaTime;
			if (TTM_CWShootTimer >= TTM_CWShootInterval)
			{
				TTM_CWShootTimer -= TTM_CWShootInterval;
				Vector3 normalized2 = new Vector3((!TTM_CWIsShootFromLeftSide) ? 1 : (-1), 1f, 0f).normalized;
				Vector3 self = PlayerMgr.Inst.PlayerPoint + normalized2 * TTM_CWToPlayerBaseTime * TTM_CWWaveMoveSpeed + Tool2D.GetDir(normalized2, 90f) * UnityEngine.Random.Range(0f - TTM_CWMaxInitialShift, TTM_CWMaxInitialShift);
				normalized2 *= TTM_CWWaveMoveSpeed;
				TTM_CWDataList.Add((TTM_CWWaveDuration, self.IgnoreZ(), -normalized2, 0f, 3));
				TTM_CWIsEvenAttack = !TTM_CWIsEvenAttack;
				TTM_CWIsShootFromLeftSide = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
			}
			break;
		case Boss56SkillType.E56MissileCombo:
			if (skillTimer <= TMC_StartSummonAt)
			{
				break;
			}
			if (skillTimer >= TMC_SkillDuration)
			{
				EndCurrentCastingSkill();
			}
			else
			{
				if (TMC_IsCSpawn)
				{
					break;
				}
				TMC_IsCSpawn = true;
				for (int num19 = 0; num19 < 4; num19++)
				{
					Vector3 position2 = base.transform.position;
					Vector3 vector17 = Vector3.zero;
					switch (num19)
					{
					case 0:
						vector17 = new Vector3(1f, 1f, 0f).normalized;
						break;
					case 1:
						vector17 = new Vector3(1f, -1f, 0f).normalized;
						break;
					case 2:
						vector17 = new Vector3(-1f, -1f, 0f).normalized;
						break;
					case 3:
						vector17 = new Vector3(-1f, 1f, 0f).normalized;
						break;
					}
					Vector3 targetPoint = position2 + vector17 * TMC_GiantBombToCenterDistance + TMC_GiantBombShiftPos;
					position2 += vector17 * TMC_GiantBombDogToCenterDistance;
					Boss56Elite56ComboShooter component9 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_DogCombo", position2).GetComponent<Boss56Elite56ComboShooter>();
					component9.InitialGiantBombData(myPpt.myEntity, targetPoint, TMC_GiantBombDelayStartTime);
					TMC_DroneList.Add(component9);
				}
				for (int num20 = 0; num20 < 4; num20++)
				{
					Vector3 position3 = base.transform.position;
					Vector3 vector18 = Vector3.zero;
					switch (num20)
					{
					case 0:
						vector18 = new Vector3(0f, 1f, 0f).normalized;
						break;
					case 1:
						vector18 = new Vector3(1f, 0f, 0f).normalized;
						break;
					case 2:
						vector18 = new Vector3(0f, -1f, 0f).normalized;
						break;
					case 3:
						vector18 = new Vector3(-1f, 0f, 0f).normalized;
						break;
					}
					position3 += vector18 * TMC_SplitMissileDogToCenterDistance;
					Boss56Elite56ComboShooter component10 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_DogCombo", position3).GetComponent<Boss56Elite56ComboShooter>();
					component10.InitialSplitMissileData(myPpt.myEntity, TMC_ShootDuration, TMC_AngleRotateSpeed, TMC_MaxRotateAngle, vector18 * -1f, TMC_SplitMissileShootInterval);
					TMC_DroneList.Add(component10);
				}
			}
			break;
		case Boss56SkillType.E56MissileChain:
		{
			if (skillTimer <= TDC_StartSummonAt)
			{
				break;
			}
			if (TDC_DogList.Count == 0)
			{
				for (int n = 0; n < 6; n++)
				{
					Vector3 vector3 = base.transform.position;
					switch (n)
					{
					case 0:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.UpperLeft);
						break;
					case 1:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.UpperRight);
						break;
					case 2:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.LowerLeft);
						break;
					case 3:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.LowerRight);
						break;
					case 4:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleLeft);
						break;
					case 5:
						vector3 = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleRight);
						break;
					}
					Boss56Elite56DogShooter component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_DogCombo", vector3).GetComponent<Boss56Elite56DogShooter>();
					CreateTeammateTeleportEffect(vector3 + new Vector3(0f, 0.5f, 0f), new Vector3((!(vector3.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x)) ? 1 : (-1), 0f, 0f), 1.2f);
					component2.InitialOwnerData(myPpt.myEntity);
					TDC_DogList.Add(component2);
				}
			}
			bool flag2 = false;
			TDC_ShootTimer += Time.deltaTime;
			if (TDC_CurrentWave < TDC_ShootTotalWave && TDC_ShootTimer >= TDC_WaveInterval)
			{
				TDC_ShootTimer -= TDC_WaveInterval;
				for (int num5 = 0; num5 < TDC_DogList.Count; num5++)
				{
					if (TDC_GiantBombWave.Contains(TDC_CurrentWave) && num5 < 4)
					{
						Vector3 centerPoint2 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
						Vector3 vector4 = default(Vector3);
						switch (num5)
						{
						case 0:
							vector4 = new Vector3(1f, 1f, 0f).normalized;
							break;
						case 1:
							vector4 = new Vector3(1f, -1f, 0f).normalized;
							break;
						case 2:
							vector4 = new Vector3(-1f, 1f, 0f).normalized;
							break;
						case 3:
							vector4 = new Vector3(-1f, -1f, 0f).normalized;
							break;
						}
						flag2 = true;
						centerPoint2 += vector4 * TDC_GiantBombToCenterDistance;
						TDC_DogList[num5].ShootGiantBomb(centerPoint2, 0.5f);
					}
					else
					{
						TDC_DogList[num5].ShootSplitBomb();
						TDC_DogList[num5].SetCannonShiftAngle((float)UnityEngine.Random.Range(-1, 2) * TDC_MissileMaxAngleShift);
					}
				}
				TDC_CurrentWave++;
			}
			if (flag2)
			{
				PlayAnimation(AnimationActions.UseRemote);
			}
			if (TDC_CurrentWave >= TDC_ShootTotalWave && TDC_ShootTimer >= TDC_AfterFinalWaveDelayDestroyDogTime)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		case Boss56SkillType.E55HexAttackCombo:
			THC_SpawnWaveTimer += Time.deltaTime;
			if (THC_WaveCounter == 0 && THC_SpawnWaveTimer >= THC_FirstWaveTime)
			{
				THC_SpawnWaveTimer -= THC_FirstWaveTime;
				for (int num14 = 0; num14 < 3; num14++)
				{
					Vector3 vector12 = Tool2D.GetDir(THC_BaseAngle + (float)(120 * num14)) * THC_FirstWaveDistance;
					Vector3 vector13 = PlayerMgr.Inst.PlayerPoint + vector12;
					Boss56E55HexShooter component7 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_TeleShooter", vector13).GetComponent<Boss56E55HexShooter>();
					CreateTeammateTeleportEffect(vector13 + new Vector3(0f, 0.3f, 0f), vector12.normalized, 1f);
					component7.InitialData(myPpt.myEntity, PlayerMgr.Inst.PlayerPoint);
					component7.CastCatchSkill();
					THC_DroneList.Add(component7);
				}
				THC_BaseAngle += 60f;
				THC_WaveCounter++;
			}
			if (THC_WaveCounter == 1 && THC_SpawnWaveTimer >= THC_SecondWaveTime)
			{
				THC_SpawnWaveTimer -= THC_SecondWaveTime;
				for (int num15 = 0; num15 < 3; num15++)
				{
					THC_DroneList[num15].transform.position = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(THC_BaseAngle + 60f + (float)(120 * num15)) * THC_SecondWaveDistance * 2f;
					THC_DroneList[num15].CastHexSkill();
				}
				for (int num16 = 0; num16 < 3; num16++)
				{
					Vector3 vector14 = Tool2D.GetDir(THC_BaseAngle + (float)(120 * num16)) * THC_SecondWaveDistance;
					Vector3 vector15 = PlayerMgr.Inst.PlayerPoint + -vector14;
					Boss56E55HexShooter component8 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_TeleShooter", vector15).GetComponent<Boss56E55HexShooter>();
					CreateTeammateTeleportEffect(vector15 + new Vector3(0f, 0.3f, 0f), vector14.normalized, 1f);
					component8.InitialData(myPpt.myEntity, PlayerMgr.Inst.PlayerPoint);
					component8.CastCatchSkill();
					THC_DroneList.Add(component8);
				}
				THC_WaveCounter++;
			}
			if (THC_WaveCounter == 2 && THC_SpawnWaveTimer >= THC_FinalWaveStartMoveTime)
			{
				THC_SpawnWaveTimer -= THC_FinalWaveStartMoveTime;
				for (int num17 = 0; num17 < 6; num17++)
				{
					Vector3 vector16 = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(60 * num17) * THC_FinalWaveDistance;
					THC_DroneList[num17].transform.DOLocalMove(vector16, THC_FinalWaveTime - THC_FinalWaveStartMoveTime - 0.05f);
					CreateTeammateTeleportEffect(THC_DroneList[num17].transform.position + new Vector3(0f, 0.3f, 0f), (THC_DroneList[num17].transform.position - vector16).normalized, 1f);
				}
				THC_WaveCounter++;
			}
			if (THC_WaveCounter == 3 && THC_SpawnWaveTimer >= THC_FinalWaveTime)
			{
				THC_SpawnWaveTimer -= THC_FinalWaveTime;
				for (int num18 = 0; num18 < 6; num18++)
				{
					THC_DroneList[num18].CastHexSkill();
				}
				THC_WaveCounter++;
			}
			if (THC_WaveCounter == 4 && THC_SpawnWaveTimer >= THC_AfterFinalWaveDelayEndTime)
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss56SkillType.E59LaserRoad:
			if (TLR_TargetTower == null)
			{
				Boss56Elite59LaserTower component11 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_LaserControlTower", Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter)).GetComponent<Boss56Elite59LaserTower>();
				CreateTeammateTeleportEffect(Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter) + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 0f, 0f), 1.2f);
				component11.InitialData(myPpt.myEntity, TLR_LaserWidth, TLR_LaserGroundAreaExistTime, TLR_LaserMoveSpeed, TLR_LaserLife, TLR_RoadBlockPercent, TLR_LaserSpawnInterval, TLR_LaserDistance);
				TLR_TargetTower = component11;
			}
			TLR_WaveTimer += Time.deltaTime;
			if (TLR_WaveTimer >= TLR_WaveSpawnInterval && TLR_CurrentWaveCount < TLR_TotalWaveCount)
			{
				TLR_WaveTimer -= TLR_WaveSpawnInterval;
				TLR_TargetTower.CastLaserRoadSkill(TLR_IsStartFromLeft, TLR_CurrentWaveCount % 2 != 0);
				TLR_IsStartFromLeft = !TLR_IsStartFromLeft;
				TLR_CurrentWaveCount++;
			}
			if (TLR_CurrentWaveCount == TLR_TotalWaveCount && TLR_WaveTimer >= TLR_DelayRecycleTowerDuration)
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss56SkillType.E58ThunderEnchantment:
		{
			if (skillTimer <= TBM_MachSpawnAt)
			{
				break;
			}
			if (TBM_Mach == null)
			{
				Vector3 roomCornerPoint = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter);
				TBM_Mach = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_ThunderMach", roomCornerPoint).GetComponent<Boss56Elite58ThunderMach>();
				CreateTeammateTeleportEffect(roomCornerPoint + new Vector3(0f, 0.5f, 0f), new Vector3((!(roomCornerPoint.x >= Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter).x)) ? 1 : (-1), 0f, 0f), 1.2f);
				TBM_Mach.InitializeData(TBM_PillarPillarMineSetRadius, TBM_ThunderBallRotateAngleSpeedUpSpeed, TBM_ThunderBallRotateMaxAngleSpeed, TBM_ThunderBallStopRotateAt, TBM_MaxAuraRange, TBM_AuraExpandDuration);
			}
			TBM_ShooTimer += Time.deltaTime;
			float num6 = (TBM_IsPillarSpawn ? TBM_ThunderBallShootInterval : TBM_MachShootPillarAt);
			if (TBM_ShooTimer >= num6 && skillTimer < TBM_SkillDuration)
			{
				TBM_ShooTimer -= num6;
				if (TBM_IsPillarSpawn)
				{
					TBM_Mach.ShootThunderBall(TBM_WaveBallCount, TBM_WaveAngle);
					TBM_ThunderBallCurrentAngle += TBM_ThunderBallInitialAnglePerShoot * (TBM_IsClockWise ? 1 : (-1));
					TBM_ThunderBallShootCounter++;
				}
				else
				{
					Vector3 vector5 = Tool2D.GetRoomCornerPoint(MapCornerType.MiddleCenter) + new Vector3(0f, TBM_PillarPillarMineSetRadius * (float)(TBM_isEvenSkill ? 1 : (-1)), 0f);
					List<Vector3> list3 = new List<Vector3>();
					list3.Add(vector5);
					for (int num7 = 0; num7 < 4; num7++)
					{
						list3.Add(vector5 + Tool2D.GetDir(-90 + 60 * num7 * ((!TBM_isEvenSkill) ? 1 : (-1))) * TBM_PillarPillarMineSetRadius * 2f);
					}
					TBM_Mach.StartSpawnPillar(list3, TBM_isEvenSkill);
					TBM_IsPillarSpawn = true;
					TBM_ShooTimer -= TBM_FirstThunderBallDelayShootDuration;
				}
			}
			if (skillTimer >= TBM_SkillDuration)
			{
				if (!TBM_IsReadyToEnd)
				{
					TBM_IsReadyToEnd = true;
					TBM_Mach.ReadyToEnd(TBM_DelayRecycleMachDuration);
					TBM_ShooTimer = 0f;
				}
				if (TBM_ShooTimer >= TBM_DelayRecycleMachDuration)
				{
					EndCurrentCastingSkill();
				}
			}
			break;
		}
		case Boss56SkillType.E52BulletRoadRoller:
			if (skillTimer <= TRR_SpawnAt)
			{
				break;
			}
			if (TRR_MachList.Count <= 0)
			{
				for (int num11 = 0; num11 < 3; num11++)
				{
					Vector3 vector10 = Tool2D.GetDir(120 * num11) * TRR_SpawnDistance;
					Vector3 vector11 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + vector10;
					Boss56Elite52RoadRoller component6 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_RoadRoller", vector11).GetComponent<Boss56Elite52RoadRoller>();
					CreateTeammateTeleportEffect(vector11 + new Vector3(0f, 0.3f, 0f), -vector10, 1.1f);
					component6.InitializeData(myPpt.myEntity);
					TRR_MachList.Add(component6);
				}
				TRR_ShootTimer -= TRR_AfterSpawnDelayAttackTime;
			}
			TRR_ShootTimer += Time.deltaTime;
			if (TRR_CurrentWaveCount < TRR_TotalAttackWave && TRR_ShootTimer >= TRR_WaveSpawnInterval)
			{
				TRR_ShootTimer -= TRR_WaveSpawnInterval;
				int num12 = TRR_TotalAttackWave - TRR_CurrentWaveCount;
				bool flag3 = num12 == 1 || num12 == 2;
				List<(Vector3, float)> list4 = TRR_GetMachAttackPointsList();
				for (int num13 = 0; num13 < TRR_MachList.Count; num13++)
				{
					TRR_MachList[num13].AttackNewTarget(list4[num13].Item1, list4[num13].Item2, flag3 ? TRR_FinalWaveBonusHoverTime : 0f);
				}
				TRR_UseSideAttack = !TRR_UseSideAttack;
				TRR_CurrentWaveCount++;
			}
			if (TRR_CurrentWaveCount >= TRR_TotalAttackWave && TRR_ShootTimer >= TRR_AfterFinalWaveDelayRecycleMachTime)
			{
				EndCurrentCastingSkill();
			}
			break;
		case Boss56SkillType.SThrowGrenades:
			if (skillTimer < STG_StartThrowAt)
			{
				break;
			}
			if (skillTimer >= STG_AfterThrowEndTime)
			{
				EndCurrentCastingSkill();
			}
			if (!STG_IsGrenadeInitialized)
			{
				STG_IsGrenadeInitialized = true;
				Vector3 normalized = Tool2D.IgnoreZV2ToV1(PlayerMgr.Inst.PlayerPoint, base.transform.position).normalized;
				List<Vector4> safeAreas = STG_GenerateSafeAreas(base.transform.position, normalized, STG_MaxRange, STG_MaxScatter);
				List<Vector3> list = STG_GenerateUniformGrenadePoints(base.transform.position, normalized, STG_MaxRange, STG_MaxScatter, STG_GrenadeCount, safeAreas);
				for (int i = 0; i < list.Count; i++)
				{
					Vector3 vector = list[i];
					float num = Tool2D.IgnoreZDistance(base.transform.position, vector);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_CommanderGrenade", base.transform.position.IgnoreZ() + new Vector3(0f, 0f, -0.5f)).GetComponent<Boss56Grenade>().InitialData(STG_BaseExplosionDelayTime + num * STG_BonusExplosionDelayPerDistance, STG_ExplosionRange, STG_ExplosionDamage, vector, STG_MoveToTargetPointDuration, Mathf.Max(0f, STG_ExplosionRange - STG_DamageRangeShrink));
				}
			}
			break;
		case Boss56SkillType.SSlashStabSlash:
			SDL_UpdateSlashStabSlash();
			break;
		case Boss56SkillType.SFastDashShoot:
			SDS_UpdateFastDashShoot();
			break;
		case Boss56SkillType.SStackDonutSlash:
			SCS_UpdateStackDonutSlash();
			break;
		case Boss56SkillType.SGrenadeRingSlash:
			SGS_UpdateGrenadeRingSlash();
			break;
		case Boss56SkillType.SToCenterCrossSlash:
			SCC_UpdateToCenterCrossSlash();
			break;
		case Boss56SkillType.SWaveSlash:
			SWS_UpdateWaveSlash();
			break;
		case Boss56SkillType.SDashShoot:
			SSB_UpdateDashShoot();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Boss56SkillType.None:
			break;
		}
	}

	private void SDS_UpdateFastDashShoot()
	{
		SDS_StageTimer += Time.deltaTime;
		switch (SDS_SkillStage)
		{
		case 0:
		{
			if (!SDS_FirstWaveShot && SDS_StageTimer >= SDS_CurrentDashShootDelay)
			{
				SDS_FirstWaveShot = true;
				SDS_CreateFirstWaveSectorShoot(base.transform.position.IgnoreZ(), GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - base.transform.position.IgnoreZ()));
			}
			float num3 = Mathf.Max(SDS_FirstWaveInterval, SDS_CurrentDashShootDelay);
			if (!(SDS_StageTimer < num3))
			{
				SDS_FirstWaveCounter++;
				if (SDS_FirstWaveCounter < Mathf.Max(1, SDS_FirstWaveFlashCount))
				{
					SDS_StartFirstWaveDashShoot();
				}
				else
				{
					SDS_StartSecondWavePrepare();
				}
			}
			break;
		}
		case 1:
			if (!(SDS_StageTimer < SDS_SecondWaveStartDelay))
			{
				SDS_SkillStage = 2;
				SDS_StageTimer = 0f;
				SDS_SecondWaveTimer = 0f;
			}
			break;
		case 2:
		{
			int num = Mathf.Max(0, SDS_SecondWaveWarningCount);
			if (num <= 0)
			{
				EndCurrentCastingSkill();
				break;
			}
			SDS_SecondWaveTimer += Time.deltaTime;
			if (SDS_SecondWaveCounter < num && (SDS_SecondWaveCounter == 0 || SDS_SecondWaveTimer >= SDS_SecondWaveWarningInterval))
			{
				SDS_SecondWaveTimer = 0f;
				SDS_SecondWaveCounter++;
				SDS_StartLockedBoxWarning();
			}
			float num2 = (float)Mathf.Max(0, num - 1) * SDS_SecondWaveWarningInterval + SDS_SecondWaveWarningDuration;
			if (SDS_SecondWaveCounter >= num && SDS_StageTimer >= num2)
			{
				EndCurrentCastingSkill();
			}
			break;
		}
		}
	}

	private void SSB_StartDashShoot()
	{
		skillTimer = 0f;
		SSB_IsSkillFinished = false;
		PlayAnimation(AnimationActions.BladeCommand);
		StartCoroutine(SSB_DashShootRoutine());
	}

	private void SSB_UpdateDashShoot()
	{
		if (SSB_IsSkillFinished)
		{
			EndCurrentCastingSkill();
		}
	}

	private IEnumerator SSB_DashShootRoutine()
	{
		int repeatCount = Mathf.Max(1, SSB_RepeatCount);
		int dashCountPerWave = Mathf.Max(1, SSB_DashCountPerWave);
		bool hasPreviousDashSide = false;
		Vector3 previousDashSide = Vector3.zero;
		for (int wave = 0; wave < repeatCount; wave++)
		{
			if (currentSkill != Boss56SkillType.SDashShoot)
			{
				yield break;
			}
			for (int dashIndex = 0; dashIndex < dashCountPerWave; dashIndex++)
			{
				Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
				Vector3 startPoint = base.transform.position.IgnoreZ();
				SSB_GetDashLine(startPoint, playerPointIgnoreZ, hasPreviousDashSide, previousDashSide, out var endPoint, out var currentDashSide);
				previousDashSide = currentDashSide;
				hasPreviousDashSide = true;
				yield return SSB_DashMoveAndShoot(startPoint, endPoint, playerPointIgnoreZ, wave);
				if (currentSkill != Boss56SkillType.SDashShoot)
				{
					yield break;
				}
				if (dashIndex < dashCountPerWave - 1 && SSB_BetweenDashDelay > 0f)
				{
					yield return new WaitForSeconds(SSB_BetweenDashDelay);
				}
			}
			if (currentSkill != Boss56SkillType.SDashShoot)
			{
				yield break;
			}
			if (SSB_BoxStartDelayAfterDash > 0f)
			{
				yield return new WaitForSeconds(SSB_BoxStartDelayAfterDash);
			}
			if (currentSkill != Boss56SkillType.SDashShoot)
			{
				yield break;
			}
			yield return SSB_LockedBoxFanRoutine(wave);
			if (wave < repeatCount - 1 && SSB_BetweenWaveDelay > 0f)
			{
				yield return new WaitForSeconds(SSB_BetweenWaveDelay);
			}
		}
		SSB_IsSkillFinished = true;
	}

	private void SSB_GetDashLine(Vector3 startPoint, Vector3 lockedPlayerPoint, bool hasPreviousDashSide, Vector3 previousDashSide, out Vector3 endPoint, out Vector3 currentDashSide)
	{
		Vector3 safeDirection = GetSafeDirection(lockedPlayerPoint - startPoint);
		currentDashSide = (hasPreviousDashSide ? SSB_GetWideTurnSideDirection(previousDashSide, safeDirection) : SSB_GetWallSideDirection(lockedPlayerPoint, safeDirection));
		Vector3 vector = lockedPlayerPoint + currentDashSide * Mathf.Max(0f, SSB_SideRadius);
		Vector3 safeDirection2 = GetSafeDirection(vector - startPoint);
		endPoint = vector + safeDirection2 * Mathf.Max(0f, SSB_AfterPlayerDistance);
	}

	private Vector3 SSB_GetWideTurnSideDirection(Vector3 previousDashSide, Vector3 toPlayer)
	{
		Vector3 dir = Tool2D.GetDir(toPlayer, 90f);
		Vector3 dir2 = Tool2D.GetDir(toPlayer, -90f);
		if (previousDashSide.sqrMagnitude <= 0.0001f)
		{
			return dir;
		}
		float num = Vector3.Angle(previousDashSide, dir);
		float num2 = Vector3.Angle(previousDashSide, dir2);
		if (!(num >= num2))
		{
			return dir2;
		}
		return dir;
	}

	private Vector3 SSB_GetWallSideDirection(Vector3 lockedPlayerPoint, Vector3 toPlayer)
	{
		Vector3 dir = Tool2D.GetDir(toPlayer, 90f);
		Vector3 dir2 = Tool2D.GetDir(toPlayer, -90f);
		float num = Mathf.Max(0f, SSB_SideRadius);
		Vector3 point = lockedPlayerPoint + dir * num;
		Vector3 point2 = lockedPlayerPoint + dir2 * num;
		float num2 = SSB_GetNearestWallDistance(point);
		float num3 = SSB_GetNearestWallDistance(point2);
		if (!(num2 <= num3))
		{
			return dir2;
		}
		return dir;
	}

	private float SSB_GetNearestWallDistance(Vector3 point)
	{
		SDS_GetCurrentRoomRect(out var minX, out var maxX, out var minY, out var maxY);
		float a = Mathf.Min(Mathf.Abs(point.x - minX), Mathf.Abs(maxX - point.x));
		float b = Mathf.Min(Mathf.Abs(point.y - minY), Mathf.Abs(maxY - point.y));
		return Mathf.Min(a, b);
	}

	private IEnumerator SSB_DashMoveAndShoot(Vector3 startPoint, Vector3 endPoint, Vector3 lockedPlayerPoint, int wave)
	{
		Vector3 vector = Tool2D.IgnoreZPoint(endPoint - startPoint);
		float totalDistance = vector.magnitude;
		if (totalDistance <= 0.0001f)
		{
			yield break;
		}
		Vector3 dashDirection = vector / totalDistance;
		float dashSpeed = Mathf.Max(0.0001f, SSB_DashSpeed);
		float duration = totalDistance / dashSpeed;
		float shootInterval = Mathf.Max(0.05f, SSB_DashShootDistanceInterval - SSB_DashShootDistanceIntervalDecreasePerWave * (float)wave);
		float nextShootDistance = shootInterval;
		float movedDistance = 0f;
		SSB_StartDashBlur(dashDirection, duration);
		while (movedDistance < totalDistance)
		{
			if (currentSkill != Boss56SkillType.SDashShoot)
			{
				yield break;
			}
			float num = Mathf.Min(totalDistance - movedDistance, dashSpeed * Time.deltaTime);
			movedDistance += num;
			base.transform.position = startPoint + dashDirection * movedDistance;
			SyncDotsPosition();
			for (; movedDistance >= nextShootDistance; nextShootDistance += shootInterval)
			{
				SSB_CreateBullet(base.transform.position.IgnoreZ(), lockedPlayerPoint);
			}
			yield return null;
		}
		base.transform.position = endPoint.IgnoreZ();
		SyncDotsPosition();
	}

	private void SSB_StartDashBlur(Vector3 dashDirection, float duration)
	{
		int token = ++SSB_DashEffectToken;
		foreach (SpriteRenderer bossBodySprite in BossBodySprites)
		{
			bossBodySprite.material.SetFloat(Alpha, FlashDashBodyAlpha);
		}
		BossFlashSprite.sprite = Dash_FastWalkSprite;
		BossFlashSprite.enabled = true;
		BossFlashSprite.material.SetVector(Direction, new Vector4(dashDirection.x, dashDirection.y, 0f, 0f));
		DashingRemainTimer = Mathf.Max(0f, duration + DashEndRecoverTime);
		DOVirtual.Float(0f, 1f, Mathf.Max(0.0001f, duration + DashEndRecoverTime), delegate(float t)
		{
			float num = DashBlurEffectCurve.Evaluate(t);
			BossFlashSprite.material.SetFloat(BlurAmount, num * DashMaxBlurPower);
			BossFlashSprite.material.SetFloat(LineStrength, num * DashMaxSpeedLinePower);
		}).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (token == SSB_DashEffectToken)
			{
				ResetDashEffect();
			}
		});
	}

	private void SSB_CreateBullet(Vector3 shootOrigin, Vector3 lockedPlayerPoint)
	{
		Vector3 safeDirection = GetSafeDirection(lockedPlayerPoint - shootOrigin);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_Bullet", shootOrigin.IgnoreZ() + new Vector3(0f, 0f, SSB_BulletSpawnHeight)).GetComponent<Boss56Bullet>().Initialize(safeDirection, Mathf.Max(0f, SSB_BulletSpeed), myPpt.myEntity);
	}

	private IEnumerator SSB_LockedBoxFanRoutine(int wave)
	{
		int boxCount = Mathf.Max(1, SSB_FirstWaveBoxCount + SSB_BonusBoxCountPerWave * wave);
		float duration = Mathf.Max(0.0001f, SSB_BoxWarningDuration);
		float lockDuration = Mathf.Clamp(SSB_BoxLockDuration, 0f, duration);
		float length = Mathf.Max(0.0001f, SSB_BoxLength);
		float width = Mathf.Max(0.0001f, SSB_BoxWidth);
		List<BoxWarningArea> warnings = new List<BoxWarningArea>();
		Vector3 origin = base.transform.position.IgnoreZ();
		Vector3 safeDirection = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
		Vector3[] directions = SSB_GetBoxFanDirections(safeDirection, boxCount);
		for (int i = 0; i < boxCount; i++)
		{
			Vector3 self = origin + directions[i] * length * 0.5f;
			BoxWarningArea component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashBoxWarningArea", self.IgnoreZ()).GetComponent<BoxWarningArea>();
			component.Register(directions[i], length, width, duration, SSB_BoxExpandFromCenter);
			warnings.Add(component);
		}
		float timer = 0f;
		while (timer < duration)
		{
			if (currentSkill != Boss56SkillType.SDashShoot)
			{
				yield break;
			}
			if (timer < lockDuration)
			{
				origin = base.transform.position.IgnoreZ();
				safeDirection = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
				directions = SSB_GetBoxFanDirections(safeDirection, boxCount);
				for (int j = 0; j < warnings.Count; j++)
				{
					warnings[j].transform.position = origin + directions[j] * length * 0.5f;
					warnings[j].transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(directions[j].y, directions[j].x) * 57.29578f);
				}
			}
			timer += Time.deltaTime;
			yield return null;
		}
		HashSet<Entity> damagedEntities = new HashSet<Entity>();
		for (int k = 0; k < directions.Length; k++)
		{
			DealSingleBoxDamageDots(origin, directions[k], length, width, Mathf.Max(0f, SSB_BoxDamage), SSB_BoxKnockback, damagedEntities);
		}
	}

	private Vector3[] SSB_GetBoxFanDirections(Vector3 baseDirection, int count)
	{
		Vector3[] array = new Vector3[Mathf.Max(1, count)];
		float num = Mathf.Max(0f, SSB_BoxSectorAngle);
		if (array.Length == 1 || num <= 0f)
		{
			array[0] = GetSafeDirection(baseDirection);
			return array;
		}
		for (int i = 0; i < array.Length; i++)
		{
			float degree = (0f - num) * 0.5f + num / (float)(array.Length - 1) * (float)i;
			array[i] = Tool2D.GetDir(baseDirection, degree);
		}
		return array;
	}

	private void SWS_StartWaveSlash()
	{
		skillTimer = 0f;
		SWS_StageTimer = 0f - SelfTeleport(SWS_GetTeleportPoint(), Dash_SlowWalkSprite);
		SWS_WarningCreated = false;
		SWS_EndDelay = 0f;
		PlayAnimation(AnimationActions.BladeCommand);
	}

	private Vector3 SWS_GetTeleportPoint()
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		Vector3 vector = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		Vector3 safeDirection = GetSafeDirection(playerPointIgnoreZ - vector);
		safeDirection = Tool2D.GetDir();
		float num = ((SWS_MinTeleportDistance > 0f) ? SWS_MinTeleportDistance : SWS_TeleportDistance);
		float num2 = ((SWS_MaxTeleportDistance > 0f) ? SWS_MaxTeleportDistance : SWS_TeleportDistance);
		if (num2 < num)
		{
			float num3 = num2;
			float num4 = num;
			num = num3;
			num2 = num4;
		}
		float num5 = UnityEngine.Random.Range(Mathf.Max(0f, num), Mathf.Max(0f, num2));
		return Tool2D.GetNavMeshPointIngoreZ(playerPointIgnoreZ + safeDirection * num5);
	}

	private void SWS_UpdateWaveSlash()
	{
		SWS_StageTimer += Time.deltaTime;
		if (!SWS_WarningCreated)
		{
			if (SWS_StageTimer < SWS_StartDelay)
			{
				return;
			}
			SWS_WarningCreated = true;
			SWS_CreateWaveSlashWarnings();
		}
		if (SWS_StageTimer >= SWS_EndDelay)
		{
			EndCurrentCastingSkill();
		}
	}

	private void SWS_CreateWaveSlashWarnings()
	{
		float num = Mathf.Max(0.0001f, SWS_AdvancePerSlash);
		float num2 = Mathf.Max(num, SWS_MaxAdvanceDistance);
		int num3 = Mathf.Max(1, Mathf.CeilToInt(num2 / num));
		Vector3 vector = base.transform.position.IgnoreZ();
		Vector3 safeDirection = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - vector);
		Vector3 dir = Tool2D.GetDir(safeDirection, 90f);
		float b = ((SWS_BaseWidth > 0f) ? SWS_BaseWidth : SWS_RectWidth);
		float b2 = ((SWS_FinalWidth > 0f) ? SWS_FinalWidth : SWS_RectWidth);
		b = Mathf.Max(0.0001f, b);
		b2 = Mathf.Max(0.0001f, b2);
		float num4 = ((UnityEngine.Random.value < 0.5f) ? 1f : (-1f));
		float num5 = 0f;
		for (int i = 0; i < num3; i++)
		{
			float num6 = (float)i * num;
			float num7 = Mathf.Min(num2, num6 + num);
			float num8 = Mathf.Lerp(b, b2, num6 / num2) * 0.5f;
			float num9 = Mathf.Lerp(b, b2, num7 / num2) * 0.5f;
			float num10 = num4 * ((i % 2 == 0) ? 1f : (-1f));
			Vector3 startPoint = vector + safeDirection * num6 + dir * (num8 * num10);
			Vector3 endPoint = vector + safeDirection * num7 - dir * (num9 * num10);
			float num11 = Mathf.Max(0.0001f, SWS_FirstSlashDelay + SWS_BonusDelayPerSlash * (float)i);
			num5 = Mathf.Max(num5, num11);
			SWS_CreateSingleWaveSlash(startPoint, endPoint, safeDirection, dir, num11);
		}
		SWS_EndDelay = SWS_StartDelay + num5 + 0.05f;
	}

	private void SWS_CreateSingleWaveSlash(Vector3 startPoint, Vector3 endPoint, Vector3 forward, Vector3 side, float delay)
	{
		Vector3 vector = endPoint - startPoint;
		float num = Mathf.Max(0.0001f, vector.magnitude);
		float num2 = ((SWS_ArcOuterRadius > 0f) ? Mathf.Max(SWS_ArcOuterRadius, num * 0.5f + 0.0001f) : (num * 0.75f));
		float num3 = Mathf.Sqrt(Mathf.Max(0.0001f, num2 * num2 - num * num * 0.25f));
		Vector3 dir = Tool2D.GetDir(vector.normalized, 90f);
		Vector3 vector2 = (startPoint + endPoint) * 0.5f;
		Vector3 vector3 = vector2 + dir * num3;
		Vector3 vector4 = vector2 - dir * num3;
		Vector3 vector5 = ((Vector3.Dot(vector3 - vector2, forward) < Vector3.Dot(vector4 - vector2, forward)) ? vector3 : vector4);
		Vector3 safeDirection = GetSafeDirection(startPoint - vector5);
		Vector3 safeDirection2 = GetSafeDirection(endPoint - vector5);
		Vector3 safeDirection3 = GetSafeDirection(vector2 - vector5);
		float sectorAngle = Mathf.Clamp(Vector3.Angle(safeDirection, safeDirection2), 0.1f, 360f);
		float num4 = Mathf.Max(0f, num2 - Mathf.Max(0.0001f, SWS_RingWidth));
		float innerRadiusRatio = Mathf.Clamp01(num4 / num2);
		bool clockwise = Vector3.Dot(endPoint - startPoint, side) < 0f;
		SectorWarningArea component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashSectorWarningArea", vector5.IgnoreZ()).GetComponent<SectorWarningArea>();
		component.transform.right = safeDirection3;
		component.transform.localScale = Vector3.one * (num2 * 2f);
		component.RegisterDonutAngular(sectorAngle, innerRadiusRatio, delay, clockwise);
		StartCoroutine(SWS_DelayDealWaveSlashDamage(vector5, safeDirection3, num4, num2, sectorAngle, delay));
	}

	private IEnumerator SWS_DelayDealWaveSlashDamage(Vector3 center, Vector3 direction, float innerRadius, float outerRadius, float sectorAngle, float delay)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, delay));
		DealAnnularSectorDamageDots(center, direction, innerRadius, outerRadius, sectorAngle, Mathf.Max(0f, SWS_Damage), SWS_Knockback, SWS_DamageRangeShrink);
	}

	private void DealAnnularSectorDamageDots(Vector3 center, Vector3 direction, float innerRadius, float outerRadius, float angle, float damage, float knockback, float damageRangeShrink)
	{
		float num = Mathf.Max(0f, damageRangeShrink);
		float num2 = Mathf.Max(0f, innerRadius + num);
		float num3 = Mathf.Max(0f, outerRadius - num);
		if (num3 <= num2)
		{
			return;
		}
		float num4 = ((outerRadius > 0.0001f) ? (Mathf.Atan2(num, outerRadius) * 57.29578f) : 0f);
		float halfAngle = Mathf.Max(0f, angle * 0.5f - num4);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(center, num3, GameConst.Filter_MonsterAoe, list);
		HashSet<Entity> hashSet = new HashSet<Entity>();
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			if (hashSet.Contains(entity))
			{
				continue;
			}
			Vector3 dotsDamageCheckPoint = GetDotsDamageCheckPoint(entity, distanceHitResult.point);
			Vector3 direction2 = Tool2D.IgnoreZPoint(dotsDamageCheckPoint - center);
			float magnitude = direction2.magnitude;
			if (magnitude < num2 || magnitude > num3 || !IsTargetInSector(center, direction, num3, halfAngle, dotsDamageCheckPoint))
			{
				continue;
			}
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				hashSet.Add(entity);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = GetSafeDirection(direction2) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
					hashSet.Add(entity);
				}
				break;
			}
		}
	}

	private void SDS_StartFirstWaveDashShoot()
	{
		SDS_StageTimer = 0f;
		SDS_FirstWaveShot = false;
		Vector3 targetPoint = SDS_GetFirstWaveWPoint(SDS_FirstWaveCounter);
		SDS_CurrentDashShootDelay = SelfTeleport(targetPoint, Dash_SlowWalkSprite) + Mathf.Max(0f, SDS_FirstWaveShootDelay);
	}

	private void SDS_StartSecondWavePrepare()
	{
		SDS_SkillStage = 1;
		SDS_StageTimer = 0f - SelfTeleport(SDS_GetFirstWaveWEndMidPoint(), Dash_SlowWalkSprite);
		SDS_SecondWaveCounter = 0;
		SDS_SecondWaveTimer = 0f;
	}

	private Vector3 SDS_GetFirstWaveWPoint(int pointIndex)
	{
		Vector3[] array = SDS_GetFirstWaveWPoints();
		if (array.Length == 0)
		{
			return PlayerMgr.Inst.PlayerPointIgnoreZ;
		}
		return array[Mathf.Abs(pointIndex) % array.Length];
	}

	private Vector3 SDS_GetFirstWaveWEndMidPoint()
	{
		Vector3[] array = SDS_GetFirstWaveWPoints();
		if (array.Length < 2)
		{
			return PlayerMgr.Inst.PlayerPointIgnoreZ;
		}
		int num = Mathf.Max(0, SDS_FirstWaveCounter - 1) % array.Length;
		int num2 = (num + 1) % array.Length;
		return Vector3.Lerp(array[num], array[num2], 0.5f);
	}

	private Vector3[] SDS_GetFirstWaveWPoints()
	{
		float num = Mathf.Max(0f, SDS_FirstWaveWWidth) * 0.5f;
		float num2 = Mathf.Max(0f, SDS_FirstWaveWHeight) * 0.5f;
		float num3 = (SDS_FirstWaveWRotate180 ? (-1f) : 1f);
		float num4 = (SDS_FirstWaveWStartFromRight ? (-1f) : 1f);
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		return new Vector3[5]
		{
			playerPointIgnoreZ + new Vector3((0f - num) * num4, num2 * num3, 0f),
			playerPointIgnoreZ + new Vector3((0f - num) * 0.5f * num4, (0f - num2) * num3, 0f),
			playerPointIgnoreZ + new Vector3(0f, num2 * num3, 0f),
			playerPointIgnoreZ + new Vector3(num * 0.5f * num4, (0f - num2) * num3, 0f),
			playerPointIgnoreZ + new Vector3(num * num4, num2 * num3, 0f)
		};
	}

	private Vector3 SDS_GetTeleportPointAroundPlayer(float distance, bool useFirstWaveOffset)
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		Vector3 vector = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		Vector3 vector2 = GetSafeDirection(vector - playerPointIgnoreZ);
		if (useFirstWaveOffset)
		{
			vector2 = SDS_GetFirstWaveTeleportDirection(playerPointIgnoreZ, vector);
		}
		Vector3 startPoint = SDS_ClampPointInsideCurrentRoom(playerPointIgnoreZ + vector2 * Mathf.Max(0f, distance));
		return SDS_ClampPointInsideCurrentRoom(Tool2D.GetNavMeshPointIngoreZ(startPoint));
	}

	private Vector3 SDS_GetSecondWaveTeleportPoint(float distance)
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		Vector3 vector = Tool2D.IgnoreZPoint(PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
		Vector3 vector2 = ((vector.sqrMagnitude > 0.0001f) ? (-vector.normalized) : Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f)));
		Vector3 startPoint = SDS_ClampPointInsideCurrentRoom(playerPointIgnoreZ + vector2 * Mathf.Max(0f, distance));
		return SDS_ClampPointInsideCurrentRoom(Tool2D.GetNavMeshPointIngoreZ(startPoint));
	}

	private Vector3 SDS_GetFirstWaveTeleportDirection(Vector3 playerPoint, Vector3 roomCenter)
	{
		Vector3 oldDir;
		if (SDS_IsPlayerCloseToRoomEdge(playerPoint))
		{
			oldDir = GetSafeDirection(roomCenter - playerPoint);
		}
		else
		{
			Vector3 vector = Tool2D.IgnoreZPoint(PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
			oldDir = ((vector.sqrMagnitude > 0.0001f) ? vector.normalized : Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f)));
		}
		float num = Mathf.Max(0f, SDS_FirstWaveTeleportRandomOffsetAngle);
		return Tool2D.GetDir(oldDir, UnityEngine.Random.Range(0f - num, num));
	}

	private bool SDS_IsPlayerCloseToRoomEdge(Vector3 playerPoint)
	{
		SDS_GetCurrentRoomRect(out var minX, out var maxX, out var minY, out var maxY);
		float num = Mathf.Max(0f, SDS_FirstWaveRoomEdgeCheckXDistance);
		float num2 = Mathf.Max(0f, SDS_FirstWaveRoomEdgeCheckYDistance);
		bool num3 = num > 0f && (playerPoint.x - minX < num || maxX - playerPoint.x < num);
		bool flag = num2 > 0f && (playerPoint.y - minY < num2 || maxY - playerPoint.y < num2);
		return num3 || flag;
	}

	private Vector3 SDS_ClampPointInsideCurrentRoom(Vector3 point)
	{
		SDS_GetCurrentRoomRect(out var minX, out var maxX, out var minY, out var maxY);
		float num = Mathf.Max(0f, 0.75f);
		if (maxX - minX > num * 2f)
		{
			minX += num;
			maxX -= num;
		}
		if (maxY - minY > num * 2f)
		{
			minY += num;
			maxY -= num;
		}
		point.x = Mathf.Clamp(point.x, minX, maxX);
		point.y = Mathf.Clamp(point.y, minY, maxY);
		point.z = 0f;
		return point;
	}

	private void SDS_GetCurrentRoomRect(out float minX, out float maxX, out float minY, out float maxY)
	{
		Vector3 vector = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		RoomConfig roomCfg = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
		float num = ((roomCfg.theme8Width > 0) ? roomCfg.theme8Width : roomCfg.width);
		float num2 = ((roomCfg.theme8Height > 0) ? roomCfg.theme8Height : roomCfg.height);
		if (num <= 0f)
		{
			num = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		}
		if (num2 <= 0f)
		{
			num2 = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		}
		minX = vector.x - num * 0.5f;
		maxX = vector.x + num * 0.5f;
		minY = vector.y - num2 * 0.5f;
		maxY = vector.y + num2 * 0.5f;
	}

	private void SDS_CreateFirstWaveSectorShoot(Vector3 shootOrigin, Vector3 shootDirection)
	{
		int num = Mathf.Max(0, Mathf.RoundToInt(SDS_FirstWaveShootCount));
		float num2 = Mathf.Max(0f, SDS_FirstWaveShootSectorAngle);
		if (num <= 0)
		{
			return;
		}
		if (num == 1)
		{
			SDS_CreateBullet(shootOrigin, shootDirection, SDS_FirstWaveBulletSpeed);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			float degree = (0f - num2) * 0.5f + num2 / (float)(num - 1) * (float)i;
			Vector3 dir = Tool2D.GetDir(shootDirection, degree);
			SDS_CreateBullet(shootOrigin, dir, SDS_FirstWaveBulletSpeed);
		}
	}

	private void SDS_CreateBullet(Vector3 shootOrigin, Vector3 shootDirection, float bulletSpeed)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_Bullet", shootOrigin.IgnoreZ() + new Vector3(0f, 0f, SDS_BulletSpawnHeight)).GetComponent<Boss56Bullet>().Initialize(GetSafeDirection(shootDirection), Mathf.Max(0f, bulletSpeed), myPpt.myEntity);
	}

	private void CreateCenteredBoxWarning(Vector3 center, Vector3 direction, float length, float width, float duration, bool expandFromCenter)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashBoxWarningArea", center.IgnoreZ()).GetComponent<BoxWarningArea>().Register(direction, length, width, duration, expandFromCenter);
	}

	private void SDS_StartLockedBoxWarning()
	{
		Vector3 origin = base.transform.position.IgnoreZ();
		float duration = Mathf.Max(0.0001f, SDS_SecondWaveWarningDuration);
		float lockPercent = SDS_NormalizePercent(SDS_SecondWaveWarningLockPercent);
		float length = Mathf.Max(0.0001f, SDS_SecondWaveBoxLength);
		float width = Mathf.Max(0.0001f, SDS_SecondWaveBoxWidth);
		StartCoroutine(SDS_LockedBoxWarningRoutine(origin, duration, lockPercent, length, width));
	}

	private float SDS_NormalizePercent(float percent)
	{
		if (percent > 1f)
		{
			percent *= 0.01f;
		}
		return Mathf.Clamp01(percent);
	}

	private IEnumerator SDS_LockedBoxWarningRoutine(Vector3 origin, float duration, float lockPercent, float length, float width)
	{
		Vector3 direction = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
		BoxWarningArea warning = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashBoxWarningArea", origin + direction * length * 0.5f).GetComponent<BoxWarningArea>();
		warning.Register(direction, length, width, duration, SDS_SecondWaveBoxExpandFromCenter);
		float timer = 0f;
		float lockDuration = duration * lockPercent;
		while (timer < duration)
		{
			if (timer < lockDuration)
			{
				direction = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
				warning.transform.position = origin + direction * length * 0.5f;
				warning.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * 57.29578f);
			}
			timer += Time.deltaTime;
			yield return null;
		}
		SDS_FireLockedBoxShot(origin, direction, length, width);
	}

	private void SDS_FireLockedBoxShot(Vector3 origin, Vector3 direction, float length, float width)
	{
		DealBoxDamageDots(origin, direction, length, width, Mathf.Max(0f, SDS_SecondWaveBoxDamage), SDS_BoxKnockback);
	}

	private void SCC_StartToCenterCrossSlash()
	{
		SCC_SkillStage = 0;
		SCC_StageTimer = 0f;
		SCC_ThirdWaveCounter = 0;
		SCC_FourthWaveWarningCreated = false;
		SCC_ThirdWaveCenter = PlayerMgr.Inst.PlayerPointIgnoreZ;
		SCC_StartThirdWaveDashShoot();
	}

	private void SCC_UpdateToCenterCrossSlash()
	{
		SCC_StageTimer += Time.deltaTime;
		switch (SCC_SkillStage)
		{
		case 0:
		{
			if (!SCC_ThirdWaveShot && SCC_StageTimer >= SCC_CurrentThirdDashShootDelay)
			{
				SCC_ThirdWaveShot = true;
				SCC_CreateThirdWaveEvenThickShoot(base.transform.position.IgnoreZ(), GetSafeDirection(SCC_ThirdWaveCenter - base.transform.position.IgnoreZ()));
			}
			float num3 = Mathf.Max(SCC_ThirdWaveInterval, SCC_CurrentThirdDashShootDelay);
			if (!(SCC_StageTimer < num3))
			{
				SCC_ThirdWaveCounter++;
				if (SCC_ThirdWaveCounter < Mathf.Max(1, SCC_ThirdWaveTeleportCount))
				{
					SCC_StartThirdWaveDashShoot();
				}
				else
				{
					SCC_StartFourthWavePrepare();
				}
			}
			break;
		}
		case 1:
		{
			float num = Mathf.Max(0f, SCC_ThirdStartFourthDelay);
			float num2 = Mathf.Max(0.0001f, SCC_FourthWaveWarningDuration);
			if (!(SCC_StageTimer < num))
			{
				if (!SCC_FourthWaveWarningCreated)
				{
					SCC_FourthWaveWarningCreated = true;
					SCC_CreateFourthWaveCrossWarnings();
				}
				if (SCC_StageTimer >= num + num2)
				{
					SCC_DealFourthWaveCrossDamage();
					EndCurrentCastingSkill();
				}
			}
			break;
		}
		}
	}

	private void SCC_StartThirdWaveDashShoot()
	{
		SCC_StageTimer = 0f;
		SCC_ThirdWaveShot = false;
		int num = Mathf.Max(1, SCC_ThirdWaveTeleportCount);
		Vector3 dir = Tool2D.GetDir(360f / (float)num * (float)SCC_ThirdWaveCounter);
		Vector3 targetPoint = SCC_ThirdWaveCenter + dir * Mathf.Max(0f, SCC_ThirdWaveTeleportDistance);
		SCC_CurrentThirdDashShootDelay = SelfTeleport(targetPoint, Dash_SlowWalkSprite) + Mathf.Max(0f, SCC_ThirdWaveShootDelay);
	}

	private void SCC_StartFourthWavePrepare()
	{
		SCC_SkillStage = 1;
		SCC_StageTimer = 0f;
		SCC_FourthWaveWarningCreated = false;
		SCC_FourthWaveCenter = PlayerMgr.Inst.PlayerPointIgnoreZ;
	}

	private void SCC_CreateThirdWaveEvenThickShoot(Vector3 shootOrigin, Vector3 shootDirection)
	{
		int num = Mathf.Max(0, Mathf.RoundToInt(SCC_ThirdWaveShootCount));
		if (num % 2 != 0)
		{
			num++;
		}
		float num2 = Mathf.Max(0f, SCC_ThirdWaveShootSectorAngle);
		int num3 = Mathf.Max(1, SCC_ThirdWaveShootThickness);
		if (num <= 0)
		{
			return;
		}
		float num4 = Mathf.Max(0f, SCC_ThirdWaveBaseAngleScatter);
		if (num4 > 0f)
		{
			shootDirection = Tool2D.GetDir(shootDirection, UnityEngine.Random.Range(0f - num4, num4));
		}
		float num5 = Mathf.Max(0f, SCC_ThirdWaveThicknessAngleInterval);
		for (int i = 0; i < num3; i++)
		{
			float degree = ((float)i - (float)(num3 - 1) * 0.5f) * num5;
			Vector3 dir = Tool2D.GetDir(shootDirection, degree);
			for (int j = 0; j < num; j++)
			{
				float degree2 = (0f - num2) * 0.5f + num2 / (float)(num - 1) * (float)j;
				Vector3 dir2 = Tool2D.GetDir(dir, degree2);
				SCC_CreateBullet(shootOrigin, dir2, SCC_ThirdWaveBulletSpeed);
			}
		}
	}

	private void SCC_CreateBullet(Vector3 shootOrigin, Vector3 shootDirection, float bulletSpeed)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_Bullet", shootOrigin.IgnoreZ() + new Vector3(0f, 0f, SCC_BulletSpawnHeight)).GetComponent<Boss56Bullet>().Initialize(GetSafeDirection(shootDirection), Mathf.Max(0f, bulletSpeed), myPpt.myEntity);
	}

	private void SCC_CreateFourthWaveCrossWarnings()
	{
		float duration = Mathf.Max(0.0001f, SCC_FourthWaveWarningDuration);
		float length = Mathf.Max(0.0001f, SCC_FourthWaveBoxLength);
		float width = Mathf.Max(0.0001f, SCC_FourthWaveBoxWidth);
		Vector3 dir = Tool2D.GetDir(SCC_FourthWaveBaseAngle);
		Vector3 dir2 = Tool2D.GetDir(dir, 90f);
		CreateCenteredBoxWarning(SCC_FourthWaveCenter, dir, length, width, duration, SCC_FourthWaveBoxExpandFromCenter);
		CreateCenteredBoxWarning(SCC_FourthWaveCenter, dir2, length, width, duration, SCC_FourthWaveBoxExpandFromCenter);
	}

	private void SCC_DealFourthWaveCrossDamage()
	{
		float num = Mathf.Max(0.0001f, SCC_FourthWaveBoxLength);
		float width = Mathf.Max(0.0001f, SCC_FourthWaveBoxWidth);
		Vector3 dir = Tool2D.GetDir(SCC_FourthWaveBaseAngle);
		Vector3 dir2 = Tool2D.GetDir(dir, 90f);
		Vector3 firstStartPoint = SCC_FourthWaveCenter - dir * num * 0.5f;
		Vector3 secondStartPoint = SCC_FourthWaveCenter - dir2 * num * 0.5f;
		DealTwoBoxDamageDots(firstStartPoint, dir, secondStartPoint, dir2, num, width, Mathf.Max(0f, SCC_FourthWaveBoxDamage), SCC_BoxKnockback);
	}

	private void SGS_StartGrenadeRingSlash()
	{
		Vector3 targetPoint = SGS_GetTeleportPoint();
		float teleportDelay = SelfTeleport(targetPoint, Dash_SlowWalkSprite);
		StartCoroutine(SGS_GrenadeRingSlashRoutine(teleportDelay));
	}

	private void SGS_UpdateGrenadeRingSlash()
	{
		if (SGS_IsSkillFinished)
		{
			EndCurrentCastingSkill();
		}
	}

	private IEnumerator SGS_GrenadeRingSlashRoutine(float teleportDelay)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, teleportDelay + SGS_StartThrowDelay));
		if (currentSkill == Boss56SkillType.SGrenadeRingSlash)
		{
			Vector3 origin = base.transform.position.IgnoreZ();
			yield return SGS_ThrowGrenadesRoutine(origin);
			yield return new WaitForSeconds(Mathf.Max(0f, SGS_StartSlashDelay));
			if (currentSkill == Boss56SkillType.SGrenadeRingSlash)
			{
				yield return SGS_TrackingSlashRoutine(origin);
			}
		}
	}

	private Vector3 SGS_GetTeleportPoint()
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		float num = Mathf.Max(0f, SGS_TeleportDistance);
		int num2 = Mathf.Max(1, SGS_TeleportPointSampleCount);
		for (int i = 0; i < num2; i++)
		{
			Vector3 dir = Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f));
			Vector3 point = playerPointIgnoreZ + dir * num;
			if (SGS_IsPointInsideCurrentRoom(point))
			{
				return SGS_GetRoomNavPoint(point);
			}
		}
		Vector3 vector = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		Vector3 safeDirection = GetSafeDirection(vector - playerPointIgnoreZ);
		return SGS_GetRoomNavPoint(playerPointIgnoreZ + safeDirection * num);
	}

	private IEnumerator SGS_ThrowGrenadesRoutine(Vector3 origin)
	{
		List<Vector3> points = new List<Vector3>();
		bool clockwise = UnityEngine.Random.value < 0.5f;
		SGS_AddAnnulusGrenadePoints(points, origin, SGS_OuterGrenadeCount, SGS_OuterGrenadeInnerRadius, SGS_OuterGrenadeOuterRadius, clockwise);
		float outerExplosionRadius = SGS_GetGrenadeExplosionRadius(SGS_OuterGrenadeRange);
		float outerThrowInterval = SGS_GetGrenadeThrowInterval(points.Count);
		for (int i = 0; i < points.Count; i++)
		{
			if (currentSkill != Boss56SkillType.SGrenadeRingSlash)
			{
				yield break;
			}
			SGS_CreateGrenade(origin, points[i], outerExplosionRadius);
			if (outerThrowInterval > 0f && i < points.Count - 1)
			{
				yield return new WaitForSeconds(outerThrowInterval);
			}
		}
		points.Clear();
		SGS_AddCircleGrenadePoints(points, origin, SGS_InnerGrenadeCount, SGS_InnerGrenadeRadius);
		float explosionRadius = SGS_GetGrenadeExplosionRadius(SGS_InnerGrenadeRange);
		float fuseTime = SGS_GetInnerGrenadeFuseTime();
		for (int j = 0; j < points.Count; j++)
		{
			if (currentSkill != Boss56SkillType.SGrenadeRingSlash)
			{
				break;
			}
			SGS_CreateGrenade(origin, points[j], explosionRadius, fuseTime);
		}
	}

	private void SGS_AddAnnulusGrenadePoints(List<Vector3> points, Vector3 center, int count, float innerRadius, float outerRadius, bool clockwise)
	{
		count = Mathf.Max(0, count);
		innerRadius = Mathf.Max(0f, innerRadius);
		outerRadius = Mathf.Max(innerRadius, outerRadius);
		if (count > 0 && !(outerRadius <= 0f))
		{
			float num = UnityEngine.Random.Range(0f, 360f);
			float num2 = 360f / (float)count * (clockwise ? (-1f) : 1f);
			float a = innerRadius * innerRadius;
			float b = outerRadius * outerRadius;
			for (int i = 0; i < count; i++)
			{
				float degree = num + (float)i * num2;
				float t = Mathf.Repeat(((float)i + 0.5f) * 0.618034f, 1f);
				float num3 = Mathf.Sqrt(Mathf.Lerp(a, b, t));
				points.Add(center + Tool2D.GetDir(degree) * num3);
			}
		}
	}

	private void SGS_AddCircleGrenadePoints(List<Vector3> points, Vector3 center, int count, float radius)
	{
		count = Mathf.Max(0, count);
		radius = Mathf.Max(0f, radius);
		if (count <= 0)
		{
			return;
		}
		if (radius <= 0f)
		{
			points.Add(center);
			return;
		}
		float num = UnityEngine.Random.Range(0f, 360f);
		float num2 = 360f / (float)count;
		for (int i = 0; i < count; i++)
		{
			float degree = num + (float)i * num2;
			points.Add(center + Tool2D.GetDir(degree) * radius);
		}
	}

	private void SGS_CreateGrenade(Vector3 origin, Vector3 targetPos, float explosionRadius)
	{
		targetPos = Tool2D.IgnoreZPoint(targetPos);
		float num = Tool2D.IgnoreZDistance(origin, targetPos);
		float fuseTime = SGS_GrenadeBaseExplosionDelayTime + num * SGS_GrenadeBonusExplosionDelayPerDistance;
		SGS_CreateGrenade(origin, targetPos, explosionRadius, fuseTime);
	}

	private void SGS_CreateGrenade(Vector3 origin, Vector3 targetPos, float explosionRadius, float fuseTime)
	{
		targetPos = Tool2D.IgnoreZPoint(targetPos);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_CommanderGrenade", origin.IgnoreZ() + new Vector3(0f, 0f, -0.5f)).GetComponent<Boss56Grenade>().InitialData(fuseTime, explosionRadius, SGS_GrenadeDamage, targetPos, SGS_GrenadeMoveToTargetPointDuration);
	}

	private float SGS_GetGrenadeExplosionRadius(float configuredRadius)
	{
		if (!(configuredRadius > 0f))
		{
			return SGS_GrenadeRange;
		}
		return configuredRadius;
	}

	private float SGS_GetGrenadeThrowInterval(int grenadeCount)
	{
		if (grenadeCount <= 1)
		{
			return 0f;
		}
		return Mathf.Max(0f, SGS_GrenadeThrowSweepDuration) / (float)(grenadeCount - 1);
	}

	private float SGS_GetInnerGrenadeFuseTime()
	{
		if (!(SGS_InnerGrenadeFuseTime > 0f))
		{
			return SGS_GrenadeBaseExplosionDelayTime;
		}
		return SGS_InnerGrenadeFuseTime;
	}

	private IEnumerator SGS_TrackingSlashRoutine(Vector3 origin)
	{
		float duration = Mathf.Max(0.0001f, SGS_SlashChargeDuration);
		float radius = Mathf.Max(0.0001f, SGS_SlashRadius);
		float angle = ((SGS_SlashAngle <= 0f) ? 270f : SGS_SlashAngle);
		float trackDuration = ((SGS_SlashTrackDuration > 0f) ? Mathf.Min(SGS_SlashTrackDuration, duration) : duration);
		Vector3 direction = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
		SectorWarningArea warning = CreateSlashSectorWarning(origin, direction, radius, angle, duration);
		float timer = 0f;
		while (timer < duration)
		{
			if (currentSkill != Boss56SkillType.SGrenadeRingSlash)
			{
				yield break;
			}
			if (timer < trackDuration)
			{
				Vector3 safeDirection = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
				direction = SGS_RotateSlashDirection(direction, safeDirection);
			}
			warning.transform.right = direction;
			timer += Time.deltaTime;
			yield return null;
		}
		if (currentSkill == Boss56SkillType.SGrenadeRingSlash)
		{
			DealSectorDamageDots(origin, direction, radius, angle, Mathf.Max(0f, SGS_SlashDamage), SGS_SlashKnockback, SGS_SlashDamageRangeShrink);
			if (currentSkill == Boss56SkillType.SGrenadeRingSlash)
			{
				SGS_IsSkillFinished = true;
			}
		}
	}

	private Vector3 SGS_RotateSlashDirection(Vector3 currentDirection, Vector3 targetDirection)
	{
		float num = Mathf.Max(0f, SGS_SlashTurnSpeed);
		if (num <= 0f)
		{
			return targetDirection;
		}
		float maxRadiansDelta = num * (MathF.PI / 180f) * Time.deltaTime;
		return Vector3.RotateTowards(currentDirection, targetDirection, maxRadiansDelta, 0f).normalized;
	}

	private bool SGS_IsPointInsideCurrentRoom(Vector3 point)
	{
		SGS_GetCurrentRoomUsableRect(out var minX, out var maxX, out var minY, out var maxY);
		if (point.x >= minX && point.x <= maxX && point.y >= minY)
		{
			return point.y <= maxY;
		}
		return false;
	}

	private Vector3 SGS_GetRoomNavPoint(Vector3 point)
	{
		Vector3 startPoint = SGS_ClampPointInsideCurrentRoom(point);
		return SGS_ClampPointInsideCurrentRoom(Tool2D.GetNavMeshPointIngoreZ(startPoint));
	}

	private Vector3 SGS_ClampPointInsideCurrentRoom(Vector3 point)
	{
		SGS_GetCurrentRoomUsableRect(out var minX, out var maxX, out var minY, out var maxY);
		point.x = Mathf.Clamp(point.x, minX, maxX);
		point.y = Mathf.Clamp(point.y, minY, maxY);
		point.z = 0f;
		return point;
	}

	private void SGS_GetCurrentRoomUsableRect(out float minX, out float maxX, out float minY, out float maxY)
	{
		SDS_GetCurrentRoomRect(out minX, out maxX, out minY, out maxY);
		float num = Mathf.Max(0f, SGS_TeleportRoomEdgePadding);
		if (maxX - minX > num * 2f)
		{
			minX += num;
			maxX -= num;
		}
		if (maxY - minY > num * 2f)
		{
			minY += num;
			maxY -= num;
		}
	}

	private void SCS_StartStackDonutSlash()
	{
		Vector3 vector = SCS_GetFirstWaveTeleportPoint();
		Vector3 safeDirection = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - vector);
		float teleportDelay = SelfTeleport(vector, Dash_SlowWalkSprite);
		StartCoroutine(SCS_FirstWaveRoutine(teleportDelay, safeDirection));
	}

	private void SCS_UpdateStackDonutSlash()
	{
		if (SCS_FirstWaveWarningFinished && SCS_SecondWaveFinished)
		{
			EndCurrentCastingSkill();
		}
	}

	private IEnumerator SCS_FirstWaveRoutine(float teleportDelay, Vector3 initialDirection)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, teleportDelay + SCS_FirstRingStartDelay));
		if (currentSkill != Boss56SkillType.SStackDonutSlash)
		{
			yield break;
		}
		Vector3 origin = base.transform.position.IgnoreZ();
		Vector3 direction = GetSafeDirection(initialDirection);
		Vector3 direction2 = PlayerMgr.Inst.PlayerPointIgnoreZ - origin;
		if (direction2.sqrMagnitude > 0.0001f)
		{
			direction = GetSafeDirection(direction2);
		}
		float spacing = Mathf.Max(0.0001f, SCS_RingsDistance);
		float num = Mathf.Max(0f, SCS_TotalRingMoveDistance);
		float warningDuration = Mathf.Max(0.0001f, SCS_RingWarningDuration);
		float spawnInterval = Mathf.Max(0f, SCS_RingSpawnInterval);
		int ringCount = ((SCS_RingsDistance <= 0.0001f) ? 1 : Mathf.Max(1, Mathf.CeilToInt(num / spacing) + 1));
		Vector3 lastRingCenter = origin;
		for (int ringIndex = 0; ringIndex < ringCount; ringIndex++)
		{
			if (currentSkill != Boss56SkillType.SStackDonutSlash)
			{
				break;
			}
			float num2 = spacing * (float)ringIndex;
			Vector3 vector = origin + direction * num2;
			SCS_CreateDonutWarning(vector, warningDuration);
			StartCoroutine(SCS_DelayDealDonutDamage(vector, warningDuration));
			lastRingCenter = vector;
			if (ringIndex < ringCount - 1)
			{
				if (spawnInterval > 0f)
				{
					yield return new WaitForSeconds(spawnInterval);
				}
				else
				{
					yield return null;
				}
			}
		}
		if (currentSkill == Boss56SkillType.SStackDonutSlash)
		{
			Vector3 targetPoint = SCS_GetRoomNavPoint(lastRingCenter + direction * spacing);
			float startDelay = SelfTeleport(targetPoint, Dash_SlowWalkSprite) + Mathf.Max(0f, SCS_StartSecondWaveDelay);
			StartCoroutine(SCS_SecondWaveRoutine(startDelay));
			yield return new WaitForSeconds(warningDuration);
			if (currentSkill == Boss56SkillType.SStackDonutSlash)
			{
				SCS_FirstWaveWarningFinished = true;
			}
		}
	}

	private IEnumerator SCS_SecondWaveRoutine(float startDelay)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, startDelay));
		if (currentSkill != Boss56SkillType.SStackDonutSlash)
		{
			yield break;
		}
		Vector3 origin = base.transform.position.IgnoreZ();
		float duration = Mathf.Max(0.0001f, SCS_BoxWarningDuration);
		float lockDuration = duration * SDS_NormalizePercent(SCS_BoxWarningLockPercent);
		float length = Mathf.Max(0.0001f, SCS_BoxLength);
		float width = Mathf.Max(0.0001f, SCS_BoxWidth);
		Vector3 direction = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
		BoxWarningArea warning = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashBoxWarningArea", origin + direction * length * 0.5f).GetComponent<BoxWarningArea>();
		warning.Register(direction, length, width, duration, SCS_BoxExpandFromCenter);
		float timer = 0f;
		while (timer < duration)
		{
			if (currentSkill != Boss56SkillType.SStackDonutSlash)
			{
				yield break;
			}
			if (timer < lockDuration)
			{
				direction = GetSafeDirection(PlayerMgr.Inst.PlayerPointIgnoreZ - origin);
				warning.transform.position = origin + direction * length * 0.5f;
				warning.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * 57.29578f);
			}
			timer += Time.deltaTime;
			yield return null;
		}
		if (currentSkill == Boss56SkillType.SStackDonutSlash)
		{
			Vector3 targetPoint = SCS_GetRoomNavPoint(origin + direction * length);
			float num = SelfTeleport(targetPoint, Dash_FastWalkSprite);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			SCS_DealBoxDamage(origin, direction, length, width);
			if (currentSkill == Boss56SkillType.SStackDonutSlash)
			{
				SCS_SecondWaveFinished = true;
			}
		}
	}

	private IEnumerator SCS_DelayDealDonutDamage(Vector3 center, float delay)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, delay));
		if (currentSkill == Boss56SkillType.SStackDonutSlash)
		{
			SCS_DealDonutDamageDots(center);
		}
	}

	private void SCS_DealBoxDamage(Vector3 origin, Vector3 direction, float length, float width)
	{
		DealBoxDamageDots(origin, direction, length, width, Mathf.Max(0f, SCS_BoxDamage), SCS_Knockback);
	}

	private Vector3 SCS_GetFirstWaveTeleportPoint()
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		Vector3 roomCenter = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		float num = Mathf.Max(0f, SCS_FirstTeleportDistance);
		if (SCS_IsPlayerCloseToRoomEdge(playerPointIgnoreZ))
		{
			return SCS_GetTeleportPointBetweenPlayerAndCenter(playerPointIgnoreZ, roomCenter, num);
		}
		int num2 = Mathf.Max(1, SCS_TeleportPointSampleCount);
		for (int i = 0; i < num2; i++)
		{
			Vector3 dir = Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f));
			Vector3 point = playerPointIgnoreZ + dir * num;
			if (SCS_IsPointInsideCurrentRoom(point))
			{
				return SCS_GetRoomNavPoint(point);
			}
		}
		return SCS_GetTeleportPointBetweenPlayerAndCenter(playerPointIgnoreZ, roomCenter, num);
	}

	private Vector3 SCS_GetTeleportPointBetweenPlayerAndCenter(Vector3 playerPoint, Vector3 roomCenter, float distance)
	{
		Vector3 vector = roomCenter - playerPoint;
		vector.z = 0f;
		float magnitude = vector.magnitude;
		if (magnitude <= 0.0001f)
		{
			return SCS_GetRoomNavPoint(playerPoint);
		}
		float num = Mathf.Min(Mathf.Max(0f, distance), magnitude);
		float num2 = Mathf.Clamp(SCS_FirstTeleportMinDistance, 0f, num);
		float num3 = ((num > num2) ? UnityEngine.Random.Range(num2, num) : num);
		return SCS_GetRoomNavPoint(playerPoint + vector / magnitude * num3);
	}

	private bool SCS_IsPlayerCloseToRoomEdge(Vector3 playerPoint)
	{
		SCS_GetCurrentRoomUsableRect(out var minX, out var maxX, out var minY, out var maxY);
		float num = Mathf.Max(0f, SCS_PlayerEdgeCheckXDistance);
		float num2 = Mathf.Max(0f, SCS_PlayerEdgeCheckYDistance);
		bool num3 = num > 0f && (playerPoint.x - minX < num || maxX - playerPoint.x < num);
		bool flag = num2 > 0f && (playerPoint.y - minY < num2 || maxY - playerPoint.y < num2);
		return num3 || flag;
	}

	private bool SCS_IsPointInsideCurrentRoom(Vector3 point)
	{
		SCS_GetCurrentRoomUsableRect(out var minX, out var maxX, out var minY, out var maxY);
		if (point.x >= minX && point.x <= maxX && point.y >= minY)
		{
			return point.y <= maxY;
		}
		return false;
	}

	private Vector3 SCS_GetRoomNavPoint(Vector3 point)
	{
		Vector3 startPoint = SCS_ClampPointInsideCurrentRoom(point);
		return SCS_ClampPointInsideCurrentRoom(Tool2D.GetNavMeshPointIngoreZ(startPoint));
	}

	private Vector3 SCS_ClampPointInsideCurrentRoom(Vector3 point)
	{
		SCS_GetCurrentRoomUsableRect(out var minX, out var maxX, out var minY, out var maxY);
		point.x = Mathf.Clamp(point.x, minX, maxX);
		point.y = Mathf.Clamp(point.y, minY, maxY);
		point.z = 0f;
		return point;
	}

	private void SCS_GetCurrentRoomUsableRect(out float minX, out float maxX, out float minY, out float maxY)
	{
		SDS_GetCurrentRoomRect(out minX, out maxX, out minY, out maxY);
		float num = Mathf.Max(0f, SCS_TeleportRoomEdgePadding);
		if (maxX - minX > num * 2f)
		{
			minX += num;
			maxX -= num;
		}
		if (maxY - minY > num * 2f)
		{
			minY += num;
			maxY -= num;
		}
	}

	private void SCS_CreateDonutWarning(Vector3 center, float duration)
	{
		SCS_GetDonutRadii(out var innerRadius, out var outerRadius);
		float innerRadiusRatio = Mathf.Clamp01(innerRadius / outerRadius);
		SectorWarningArea component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashSectorWarningArea", center.IgnoreZ()).GetComponent<SectorWarningArea>();
		component.transform.right = Vector3.right;
		component.transform.localScale = Vector3.one * (outerRadius * 2f);
		component.RegisterDonutAngular(360f, innerRadiusRatio, Mathf.Max(0.0001f, duration), clockwise: false);
	}

	private void SCS_GetDonutRadii(out float innerRadius, out float outerRadius)
	{
		float num = Mathf.Max(0.0001f, SCS_RingRadius);
		float num2 = Mathf.Max(0.0001f, SCS_DonutWidth);
		outerRadius = num;
		innerRadius = Mathf.Max(0f, num - num2);
	}

	private void SCS_DealDonutDamageDots(Vector3 center)
	{
		SCS_GetDonutDamageRadii(out var innerRadius, out var outerRadius);
		if (!(outerRadius <= innerRadius))
		{
			float damage = Mathf.Max(0f, SCS_RingDamage);
			HashSet<Entity> damagedEntities = new HashSet<Entity>();
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(center, outerRadius, GameConst.Filter_MonsterAoe, list);
			for (int i = 0; i < list.Count; i++)
			{
				SCS_TryDealDonutDamageHit(center, innerRadius, outerRadius, damage, damagedEntities, list[i]);
			}
		}
	}

	private void SCS_TryDealDonutDamageHit(Vector3 center, float innerRadius, float outerRadius, float damage, HashSet<Entity> damagedEntities, UnitDotsSyncSystem.DistanceHitResult result)
	{
		Entity entity = result.entity;
		Vector3 point = result.point;
		Vector3 dotsDamageCheckPoint = GetDotsDamageCheckPoint(entity, point);
		Vector3 vector = Tool2D.IgnoreZPoint(point - center);
		Vector3 vector2 = Tool2D.IgnoreZPoint(dotsDamageCheckPoint - center);
		float magnitude = vector.magnitude;
		float magnitude2 = vector2.magnitude;
		float magnitude3 = Tool2D.IgnoreZPoint(dotsDamageCheckPoint - point).magnitude;
		float num = magnitude2 + magnitude3;
		if (magnitude > outerRadius || num < innerRadius || damagedEntities.Contains(entity))
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(entity))
		{
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
			damagedEntities.Add(entity);
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = GetSafeDirection((vector2.sqrMagnitude > 0.0001f) ? vector2 : vector) * SCS_Knockback;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				damagedEntities.Add(entity);
			}
			break;
		}
	}

	private void SCS_GetDonutDamageRadii(out float innerRadius, out float outerRadius)
	{
		SCS_GetDonutRadii(out innerRadius, out outerRadius);
		float num = Mathf.Max(0f, SCS_DonutDamageRangeShrink);
		innerRadius += num;
		outerRadius = Mathf.Max(0f, outerRadius - num);
	}

	private void SDL_UpdateSlashStabSlash()
	{
		SDL_StageTimer += Time.deltaTime;
		switch (SDL_SkillStage)
		{
		case 0:
			if (!(SDL_StageTimer < SDL_FSWarningDelay))
			{
				CreateSlashSectorWarning(SDL_FSOrigin, SDL_FSDir, SDL_FSRadius, SDL_FSAngle, SDL_FSDelay);
				SDL_GotoNextStage();
			}
			break;
		case 1:
			if (!(SDL_StageTimer < SDL_FSDelay))
			{
				CreateSDLSlashEffect("EF_Boss56_SDLSlash1", SDL_FSOrigin, SDL_FSDir, SDL_FSRadius);
				DealSectorDamageDots(SDL_FSOrigin, SDL_FSDir, SDL_FSRadius, SDL_FSAngle, GetSDLResolvedDamage(SDL_FSDamage));
				SDL_GotoNextStage();
			}
			break;
		case 2:
			if (!(SDL_StageTimer < SDL_EndFSStartDSDelay))
			{
				SDL_StartDirectSlash();
				SDL_GotoNextStage();
			}
			break;
		case 3:
			if (!(SDL_StageTimer < SDL_EndDSStartSSDelay))
			{
				SDL_GotoNextStage();
				SDL_SSOrigin = SDL_DSEnd.IgnoreZ();
				SDL_SSDir = GetSafeDirection(PlayerMgr.Inst.PlayerPoint - SDL_SSOrigin);
				CreateSlashSectorWarning(SDL_SSOrigin, SDL_SSDir, SDL_SSRadius, SDL_SSAngle, SDL_SSDelay);
			}
			break;
		case 4:
			if (!(SDL_StageTimer < SDL_SSDelay))
			{
				CreateSDLSlashEffect("EF_Boss56_SDLSlash3", SDL_SSOrigin, SDL_SSDir, SDL_SSRadius);
				DealSectorDamageDots(SDL_SSOrigin, SDL_SSDir, SDL_SSRadius, SDL_SSAngle, GetSDLResolvedDamage(SDL_SSDamage));
				EndCurrentCastingSkill();
			}
			break;
		}
	}

	private void SDL_StartDirectSlash()
	{
		SDL_DSStart = base.transform.position.IgnoreZ();
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		SDL_DSDir = GetSafeDirection(playerPointIgnoreZ - SDL_DSStart);
		SDL_DSEnd = Tool2D.GetNavMeshPointIngoreZ(playerPointIgnoreZ + SDL_DSDir * SDL_DSOverPlayerDistance);
		SDL_DSLength = Tool2D.IgnoreZDistance(SDL_DSStart, SDL_DSEnd);
		SelfTeleport(SDL_DSEnd, Dash_FastWalkSprite);
		CreateSlashBoxWarning(SDL_DSStart, SDL_DSEnd, SDL_DSWidth, SDL_DSSlashDelay, SDL_DSExpandFromCenter);
		StartCoroutine(SDL_DelayDealDirectSlashDamage(SDL_DSStart, SDL_DSDir, SDL_DSLength, SDL_DSWidth, SDL_DSSlashDelay, GetSDLResolvedDamage(SDL_DSDamage)));
	}

	private IEnumerator SDL_DelayDealDirectSlashDamage(Vector3 startPoint, Vector3 direction, float length, float width, float delay, float damage)
	{
		yield return new WaitForSeconds(Mathf.Max(0f, delay));
		DealBoxDamageDots(startPoint, direction, length, width, damage);
	}

	private void SDL_GotoNextStage()
	{
		SDL_SkillStage++;
		SDL_StageTimer = 0f;
	}

	private Vector3 GetSafeDirection(Vector3 direction)
	{
		direction.z = 0f;
		if (direction.sqrMagnitude <= 0.0001f)
		{
			return Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f));
		}
		return direction.normalized;
	}

	private float GetSDLResolvedDamage(float configuredDamage)
	{
		return Mathf.Max(0f, configuredDamage);
	}

	private void CreateSDLSlashEffect(string effectName, Vector3 origin, Vector3 direction, float radius)
	{
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + effectName, origin.IgnoreZ(), 2.5f);
		gO.transform.right = GetSafeDirection(direction);
		gO.transform.localScale = Vector3.one * Mathf.Max(0.0001f, radius);
	}

	private SectorWarningArea CreateSlashSectorWarning(Vector3 origin, Vector3 direction, float radius, float angle, float delay)
	{
		SectorWarningArea component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashSectorWarningArea", origin.IgnoreZ()).GetComponent<SectorWarningArea>();
		component.transform.right = direction;
		component.transform.localScale = Vector3.one * Mathf.Max(0.0001f, radius * 2f);
		component.RegisterRadial(angle, Mathf.Max(0.0001f, delay));
		return component;
	}

	private void CreateSlashBoxWarning(Vector3 startPoint, Vector3 endPoint, float width, float delay, bool expandFromCenter)
	{
		Vector3 safeDirection = GetSafeDirection(endPoint - startPoint);
		float b = Tool2D.IgnoreZDistance(startPoint, endPoint);
		Vector3 self = (startPoint + endPoint) * 0.5f;
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss56_SlashBoxWarningArea", self.IgnoreZ()).GetComponent<BoxWarningArea>().Register(safeDirection, Mathf.Max(0.0001f, b), Mathf.Max(0.0001f, width), Mathf.Max(0.0001f, delay), expandFromCenter);
	}

	private void DealSectorDamage(Vector3 origin, Vector3 direction, float radius, float angle, float damage)
	{
		List<UnitProperty> targets = new List<UnitProperty>();
		AddPlayerTargets(targets, (UnitProperty targetPpt) => IsTargetInSector(origin, direction, radius, angle * 0.5f, targetPpt.transform.position));
		DealDamageToTargets(targets, origin, damage);
	}

	private void DealSectorDamageDots(Vector3 origin, Vector3 direction, float radius, float angle, float damage)
	{
		DealSectorDamageDots(origin, direction, radius, angle, damage, SDL_Knockback, SDL_SectorDamageRangeShrink);
	}

	private void DealCircleDamageDots(Vector3 origin, float radius, float damage, float knockback, float damageRangeShrink)
	{
		float num = Mathf.Max(0f, radius - Mathf.Max(0f, damageRangeShrink));
		if (num <= 0f)
		{
			return;
		}
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(origin, num, GameConst.Filter_MonsterAoe, list);
		HashSet<Entity> hashSet = new HashSet<Entity>();
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			if (hashSet.Contains(entity))
			{
				continue;
			}
			Vector3 direction = Tool2D.IgnoreZPoint(GetDotsDamageCheckPoint(entity, distanceHitResult.point) - origin);
			if (direction.sqrMagnitude > num * num)
			{
				continue;
			}
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				hashSet.Add(entity);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = GetSafeDirection(direction) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
					hashSet.Add(entity);
				}
				break;
			}
		}
	}

	private void DealSectorDamageDots(Vector3 origin, Vector3 direction, float radius, float angle, float damage, float knockback, float damageRangeShrink)
	{
		damageRangeShrink = Mathf.Max(0f, damageRangeShrink);
		float radius2 = Mathf.Max(0f, radius - damageRangeShrink);
		float num = ((radius > 0.0001f) ? (Mathf.Atan2(damageRangeShrink, radius) * 57.29578f) : 0f);
		float halfAngle = Mathf.Max(0f, angle * 0.5f - num);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(origin, radius2, GameConst.Filter_MonsterAoe, list);
		HashSet<Entity> hashSet = new HashSet<Entity>();
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			Vector3 dotsDamageCheckPoint = GetDotsDamageCheckPoint(entity, distanceHitResult.point);
			if (!IsTargetInSector(origin, direction, radius2, halfAngle, dotsDamageCheckPoint) || hashSet.Contains(entity))
			{
				continue;
			}
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				hashSet.Add(entity);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = GetSafeDirection(distanceHitResult.point - origin) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
					hashSet.Add(entity);
				}
				break;
			}
		}
	}

	private Vector3 GetDotsDamageCheckPoint(Entity entity, Vector3 fallbackPoint)
	{
		if (UnitDotsSyncSystem.HasComponent<LocalTransform>(entity))
		{
			return UnitDotsSyncSystem.GetComponentData<LocalTransform>(entity).Position;
		}
		return fallbackPoint;
	}

	private void DealBoxDamageDots(Vector3 startPoint, Vector3 direction, float length, float width, float damage)
	{
		DealBoxDamageDots(startPoint, direction, length, width, damage, SDL_Knockback);
	}

	private void DealBoxDamageDots(Vector3 startPoint, Vector3 direction, float length, float width, float damage, float knockback)
	{
		HashSet<Entity> damagedEntities = new HashSet<Entity>();
		DealSingleBoxDamageDots(startPoint, direction, length, width, damage, knockback, damagedEntities);
	}

	private void DealTwoBoxDamageDots(Vector3 firstStartPoint, Vector3 firstDirection, Vector3 secondStartPoint, Vector3 secondDirection, float length, float width, float damage, float knockback)
	{
		HashSet<Entity> damagedEntities = new HashSet<Entity>();
		DealSingleBoxDamageDots(firstStartPoint, firstDirection, length, width, damage, knockback, damagedEntities);
		DealSingleBoxDamageDots(secondStartPoint, secondDirection, length, width, damage, knockback, damagedEntities);
	}

	private void DealSingleBoxDamageDots(Vector3 startPoint, Vector3 direction, float length, float width, float damage, float knockback, HashSet<Entity> damagedEntities)
	{
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(startPoint + direction * length * 0.5f, Mathf.Sqrt(length * length + width * width) * 0.5f, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			if (!IsTargetInBox(startPoint, direction, length, width, distanceHitResult.point))
			{
				continue;
			}
			Entity entity = distanceHitResult.entity;
			if (damagedEntities.Contains(entity))
			{
				continue;
			}
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				damagedEntities.Add(entity);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = GetSafeDirection(distanceHitResult.point - startPoint) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
					damagedEntities.Add(entity);
				}
				break;
			}
		}
	}

	private void DealBoxDamage(Vector3 startPoint, Vector3 direction, float length, float width, float damage)
	{
		List<UnitProperty> targets = new List<UnitProperty>();
		AddPlayerTargets(targets, (UnitProperty targetPpt) => IsTargetInBox(startPoint, direction, length, width, targetPpt.transform.position));
		DealDamageToTargets(targets, startPoint, damage);
	}

	private void AddPlayerTargets(List<UnitProperty> targets, Func<UnitProperty, bool> checkTarget)
	{
		UnitProperty playerPpt = PlayerMgr.Inst.PlayerPpt;
		if (playerPpt != null && checkTarget(playerPpt))
		{
			targets.Add(playerPpt);
		}
		List<UnitProperty> summonsPpts = PlayerMgr.Inst.summonsPpts;
		for (int i = 0; i < summonsPpts.Count; i++)
		{
			UnitProperty unitProperty = summonsPpts[i];
			if (unitProperty != null && checkTarget(unitProperty))
			{
				targets.Add(unitProperty);
			}
		}
	}

	private bool IsTargetInSector(Vector3 origin, Vector3 direction, float radius, float halfAngle, Vector3 targetPoint)
	{
		radius = Mathf.Max(0f, radius);
		direction = GetSafeDirection(direction);
		Vector3 to = Tool2D.IgnoreZPoint(targetPoint - origin);
		if (to.sqrMagnitude > radius * radius)
		{
			return false;
		}
		return Vector3.Angle(direction, to) <= halfAngle;
	}

	private bool IsTargetInBox(Vector3 startPoint, Vector3 direction, float length, float width, Vector3 targetPoint)
	{
		length = Mathf.Max(0f, length);
		width = Mathf.Max(0f, width);
		direction = GetSafeDirection(direction);
		Vector3 vector = Tool2D.IgnoreZPoint(targetPoint - startPoint);
		float num = Vector3.Dot(vector, direction);
		if (num < 0f || num > length)
		{
			return false;
		}
		return (vector - direction * num).sqrMagnitude <= width * width * 0.25f;
	}

	private void DealDamageToTargets(List<UnitProperty> targets, Vector3 knockbackOrigin, float damage)
	{
		HashSet<UnitProperty> hashSet = new HashSet<UnitProperty>();
		foreach (UnitProperty target in targets)
		{
			if (!(target == null))
			{
				UnitProperty unitProperty = target;
				if (unitProperty.CanTouch && !hashSet.Contains(unitProperty))
				{
					TakeDamageInfo info = new TakeDamageInfo
					{
						damage = damage,
						attackerPpt = myPpt,
						knockbackForce = GetSafeDirection(unitProperty.transform.position - knockbackOrigin) * SDL_Knockback,
						teammateTakeDamageRatio = 4f
					};
					unitProperty.TakeDamage(damage, myPpt, info);
					hashSet.Add(unitProperty);
				}
			}
		}
	}

	private List<Vector3> GeneratePointsInSectorXY(Vector3 origin, Vector3 forwardDir, float radius, float angleDeg, int count, float minDistance, int maxAttempts = 500)
	{
		List<Vector3> list = new List<Vector3>();
		forwardDir.z = 0f;
		forwardDir.Normalize();
		float num = angleDeg * 0.5f;
		int num2 = 0;
		while (list.Count < count && num2 < maxAttempts)
		{
			num2++;
			Vector3 vector = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - num, num), Vector3.forward) * forwardDir;
			float num3 = Mathf.Sqrt(UnityEngine.Random.value) * radius;
			Vector3 vector2 = origin + vector * num3;
			vector2.z = origin.z;
			bool flag = true;
			foreach (Vector3 item in list)
			{
				if (Vector3.Distance(vector2, item) < minDistance)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(vector2);
			}
		}
		return list;
	}

	private List<Vector4> STG_GenerateSafeAreas(Vector3 origin, Vector3 forwardDir, float radius, float angleDeg)
	{
		List<Vector4> list = new List<Vector4>();
		float num = Mathf.Max(0f, STG_ExplosionRange) + Mathf.Max(0f, STG_SafeAreaRadius);
		if (num <= 0f)
		{
			return list;
		}
		int num2 = Mathf.Max(0, STG_MinSafeAreaCount);
		int num3 = Mathf.Max(num2, STG_MaxSafeAreaCount);
		int num4 = UnityEngine.Random.Range(num2, num3 + 1);
		if (num4 <= 0)
		{
			return list;
		}
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		list.Add(new Vector4(playerPointIgnoreZ.x, playerPointIgnoreZ.y, 0f, num));
		forwardDir = GetSafeDirection(Tool2D.IgnoreZPoint(forwardDir));
		float num5 = Mathf.Max(STG_SafeAreaCenterRandomRadius, num * 1.25f);
		float minDistance = Mathf.Min(num * 1.35f, num5 * 0.75f);
		float num6 = Mathf.Max(0f, angleDeg) * 0.5f;
		for (int i = 1; i < num4; i++)
		{
			Vector3 vector = playerPointIgnoreZ;
			bool flag = false;
			for (int j = 0; j < 40; j++)
			{
				Vector3 vector2 = Tool2D.GetDir(UnityEngine.Random.Range(0f, 360f)) * (Mathf.Sqrt(UnityEngine.Random.value) * num5);
				Vector3 vector3 = Tool2D.IgnoreZPoint(playerPointIgnoreZ + vector2);
				if (STG_IsPointInsideSector(vector3, origin, forwardDir, radius, num6) && STG_IsSafeAreaCenterFarEnough(vector3, list, minDistance))
				{
					vector = vector3;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				float degree = ((num4 <= 1) ? 0f : Mathf.Lerp(0f - num6, num6, (float)i / (float)(num4 - 1)));
				vector = Tool2D.IgnoreZPoint(playerPointIgnoreZ + Tool2D.GetDir(forwardDir, degree) * num5);
			}
			list.Add(new Vector4(vector.x, vector.y, 0f, num));
		}
		return list;
	}

	private List<Vector3> STG_GenerateUniformGrenadePoints(Vector3 origin, Vector3 forwardDir, float radius, float angleDeg, int count, List<Vector4> safeAreas)
	{
		List<Vector3> list = new List<Vector3>();
		count = Mathf.Max(0, count);
		radius = Mathf.Max(0f, radius);
		if (count <= 0 || radius <= 0f)
		{
			return list;
		}
		forwardDir.z = 0f;
		forwardDir = GetSafeDirection(forwardDir);
		float num = Mathf.Max(0f, angleDeg) * 0.5f;
		int num2 = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(count)));
		for (int i = 0; i < num2; i++)
		{
			float num3 = (float)i / (float)num2;
			float num4 = (float)(i + 1) / (float)num2;
			float num5 = radius * num3;
			float num6 = radius * num4;
			float num7 = radius * Mathf.Sqrt((num3 * num3 + num4 * num4) * 0.5f);
			float num8 = Mathf.Max(0.25f, num7 / radius);
			int num9 = Mathf.Max(1, Mathf.CeilToInt((float)count * num8 / (float)num2 * 1.5f));
			float num10 = Mathf.Clamp01(STG_GrenadePointRandomRatio);
			float num11 = ((num9 <= 1) ? 0f : (angleDeg / (float)num9));
			for (int j = 0; j < num9; j++)
			{
				float t = ((num9 <= 1) ? 0.5f : Mathf.Clamp01(((float)j + 0.5f + UnityEngine.Random.Range(-0.45f, 0.45f) * num10) / (float)num9));
				float num12 = Mathf.Lerp(0f - num, num, t);
				num12 += UnityEngine.Random.Range(0f - num11, num11) * 0.35f * num10;
				float t2 = Mathf.Clamp01(0.5f + UnityEngine.Random.Range(-0.45f, 0.45f) * num10);
				float num13 = Mathf.Sqrt(Mathf.Lerp(num5 * num5, num6 * num6, t2));
				Vector3 dir = Tool2D.GetDir(forwardDir, num12);
				Vector3 vector = Tool2D.IgnoreZPoint(origin + dir * num13);
				if (!STG_IsPointInSafeAreas(vector, safeAreas))
				{
					list.Add(vector);
				}
			}
		}
		STG_FillMissingUniformGrenadePoints(list, origin, forwardDir, radius, angleDeg, count, safeAreas);
		return STG_PickEvenlySpacedPoints(list, count);
	}

	private void STG_FillMissingUniformGrenadePoints(List<Vector3> points, Vector3 origin, Vector3 forwardDir, float radius, float angleDeg, int targetCount, List<Vector4> safeAreas)
	{
		int num = Mathf.Max(100, targetCount * 20);
		float num2 = Mathf.Max(0f, angleDeg) * 0.5f;
		int num3 = 0;
		while (points.Count < targetCount && num3 < num)
		{
			num3++;
			float degree = UnityEngine.Random.Range(0f - num2, num2);
			float num4 = Mathf.Sqrt(UnityEngine.Random.value) * radius;
			Vector3 vector = Tool2D.IgnoreZPoint(origin + Tool2D.GetDir(forwardDir, degree) * num4);
			if (!STG_IsPointInSafeAreas(vector, safeAreas))
			{
				points.Add(vector);
			}
		}
	}

	private List<Vector3> STG_PickEvenlySpacedPoints(List<Vector3> points, int count)
	{
		List<Vector3> list = new List<Vector3>();
		if (points.Count <= count)
		{
			return points;
		}
		float num = (float)points.Count / (float)count;
		float num2 = UnityEngine.Random.Range(0f, num);
		for (int i = 0; i < count; i++)
		{
			int index = Mathf.Clamp(Mathf.FloorToInt(num2 + (float)i * num), 0, points.Count - 1);
			list.Add(points[index]);
		}
		return list;
	}

	private bool STG_IsSafeAreaCenterFarEnough(Vector3 center, List<Vector4> safeAreas, float minDistance)
	{
		if (safeAreas == null || minDistance <= 0f)
		{
			return true;
		}
		Vector2 vector = new Vector2(center.x, center.y);
		for (int i = 0; i < safeAreas.Count; i++)
		{
			Vector4 vector2 = safeAreas[i];
			if ((vector - new Vector2(vector2.x, vector2.y)).sqrMagnitude < minDistance * minDistance)
			{
				return false;
			}
		}
		return true;
	}

	private bool STG_IsPointInsideSector(Vector3 point, Vector3 origin, Vector3 forwardDir, float radius, float halfAngle)
	{
		Vector3 vector = Tool2D.IgnoreZPoint(point - origin);
		float magnitude = vector.magnitude;
		if (magnitude > Mathf.Max(0f, radius))
		{
			return false;
		}
		if (magnitude <= 0.0001f)
		{
			return true;
		}
		return Vector3.Angle(forwardDir, vector / magnitude) <= halfAngle;
	}

	private bool STG_IsPointInSafeAreas(Vector3 point, List<Vector4> safeAreas)
	{
		if (safeAreas == null)
		{
			return false;
		}
		Vector2 vector = new Vector2(point.x, point.y);
		for (int i = 0; i < safeAreas.Count; i++)
		{
			Vector4 vector2 = safeAreas[i];
			float num = Mathf.Max(0f, vector2.w);
			if ((vector - new Vector2(vector2.x, vector2.y)).sqrMagnitude <= num * num)
			{
				return true;
			}
		}
		return false;
	}

	private void TTM_UpdateShootEffect()
	{
		if (TTM_CWDataList.Count <= 0)
		{
			return;
		}
		bool useShortEffect = true;
		for (int num = TTM_CWDataList.Count - 1; num > 0; num--)
		{
			(float, Vector3, Vector3, float, int) value = TTM_CWDataList[num];
			value.Item4 += Time.deltaTime;
			value.Item2 += value.Item3 * Time.deltaTime;
			value.Item1 -= Time.deltaTime;
			if (value.Item4 >= TTM_CWWaveShootInterval)
			{
				value.Item4 -= TTM_CWWaveShootInterval;
				float num2 = (0f - TTM_CWMaxShiftDistance) * (float)(value.Item5 - 1) / 2f;
				for (int i = 0; i < value.Item5; i++)
				{
					Vector3 vector = value.Item2 + Tool2D.GetDir(value.Item3.normalized, 90f) * (num2 + TTM_CWMaxShiftDistance * (float)i) + new Vector3(0f, 0f, -20f);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_Boss56SpecialLongMissile", vector).GetComponent<Boss56Elite57SpecialLongMissile>().InitialSubBombData(10f, TTM_CWDamageRange, TTM_CWLandDuration, 0f, 40f, useShortEffect);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VShortMarker", vector.IgnoreZ(), TTM_CWLandDuration);
				}
			}
			TTM_CWDataList[num] = value;
			if (value.Item1 <= 0f)
			{
				TTM_CWDataList.RemoveAt(num);
			}
		}
	}

	private List<(Vector3 position, float rotateAngle)> TRR_GetMachAttackPointsList()
	{
		List<(Vector3, float)> list = new List<(Vector3, float)>();
		if (TRR_CurrentWaveCount == TRR_TotalAttackWave - 2)
		{
			float num = UnityEngine.Random.Range(0f, 360f);
			for (int i = 0; i < 3; i++)
			{
				float num2 = num + (float)(120 * i);
				list.Add((PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(num2) * TRR_FinalLastWaveRadius, num2 + 60f));
			}
			return list;
		}
		if (TRR_CurrentWaveCount == TRR_TotalAttackWave - 1)
		{
			float num3 = UnityEngine.Random.Range(0f, 360f);
			for (int j = 0; j < 3; j++)
			{
				list.Add((PlayerMgr.Inst.PlayerPoint, num3 + TRR_FinalWaveScatter * (float)j));
			}
			return list;
		}
		if (TRR_UseSideAttack)
		{
			int num4 = 8;
			float num5 = 360f / (float)num4;
			Vector3 vector = ((PlayerMgr.Inst.PlayerCtrller.CurrentMotion != Vector3.zero) ? PlayerMgr.Inst.PlayerCtrller.CurrentMotion.normalized : UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized);
			float num6 = Mathf.Atan2(vector.y, vector.x) * 57.29578f + num5 / 2f;
			if (num6 < 0f)
			{
				num6 += 360f;
			}
			float num7 = (float)(Mathf.FloorToInt(num6 / num5) % num4) * num5 - 90f;
			for (int k = 0; k < 3; k++)
			{
				list.Add((PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(num7 - num5 + num5 * (float)k) * TRR_SideAttackRadius, num7 + 45f * (float)(k + 1)));
			}
		}
		else
		{
			int num8 = ((!TRR_SSIsEvenShoot) ? 60 : 0);
			for (int l = 0; l < 3; l++)
			{
				int num9 = num8 + 120 * l;
				list.Add((PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(num9) * TRR_CenterAttackRadius, num9 + 90));
			}
			TRR_SSIsEvenShoot = !TRR_SSIsEvenShoot;
		}
		return list;
	}

	private void UpdateMoveState()
	{
		switch (moveState)
		{
		case Boss56MoveState.Idle:
			SetMove(Vector3.zero);
			if (unitTimer < BornIdleDuration)
			{
				unitTimer += Time.deltaTime;
			}
			else if (GetToPlayerDistance() > MinClosePlayerDistance * 1.2f)
			{
				MoveEnterState(Boss56MoveState.Close);
			}
			else
			{
				unitTimer -= BornIdleDuration;
			}
			FaceToPlayer();
			break;
		case Boss56MoveState.Close:
			if (GetToPlayerDistance() > MinClosePlayerDistance)
			{
				SetMove(GetToPlayerDirection() * GetCurrentMoveSpeed());
			}
			else
			{
				MoveEnterState(Boss56MoveState.Idle);
			}
			FaceToPlayer();
			break;
		case Boss56MoveState.StopMotion:
			FaceToPlayer();
			break;
		case Boss56MoveState.SkillMotion:
			switch (currentSkill)
			{
			case Boss56SkillType.None:
				SetMove(Vector3.zero);
				break;
			case Boss56SkillType.E51CannonWave:
			case Boss56SkillType.E50BombWave:
			case Boss56SkillType.E57MissileWave:
			case Boss56SkillType.E56MissileChain:
			case Boss56SkillType.E55HexAttackCombo:
			case Boss56SkillType.E52BulletRoadRoller:
				if (castSkillStopMotionTimer > 0f)
				{
					SetMove(Vector3.zero, instantLerp: true);
					castSkillStopMotionTimer -= Time.deltaTime;
					if (castSkillStopMotionTimer <= 0f)
					{
						MoveEnterState((GetToPlayerDistance() > MinClosePlayerDistance) ? Boss56MoveState.Close : Boss56MoveState.Idle);
					}
				}
				break;
			case Boss56SkillType.E53RotateBall:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.E56MissileCombo:
			case Boss56SkillType.E59LaserRoad:
			case Boss56SkillType.E58ThunderEnchantment:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SThrowGrenades:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SSlashStabSlash:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SFastDashShoot:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SStackDonutSlash:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SGrenadeRingSlash:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SToCenterCrossSlash:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SWaveSlash:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			case Boss56SkillType.SDashShoot:
				SetMove(Vector3.zero, instantLerp: true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		}
	}

	public void SetMove(Vector3 motion, bool instantLerp = false, float motionLerp = 0f)
	{
		float num = ((motionLerp > 0f) ? motionLerp : moveLerp);
		base.CurrentMotion = Tool2D.IgnoreZPoint(base.CurrentMotion);
		base.CurrentMotion = Vector3.Lerp(base.CurrentMotion, motion, instantLerp ? 1f : (num * Time.deltaTime));
	}

	public Vector3 GetToPlayerDirection()
	{
		return (PlayerMgr.Inst.PlayerPoint - base.transform.position).normalized;
	}

	public float GetToPlayerDistance()
	{
		return Tool2D.IgnoreZDistance(base.transform.position, PlayerMgr.Inst.PlayerPoint);
	}

	private void MoveEnterState(Boss56MoveState state)
	{
		moveState = state;
		switch (moveState)
		{
		case Boss56MoveState.Idle:
			PlayAnimation(AnimationActions.IdleNoWeapon);
			break;
		case Boss56MoveState.Close:
			PlayAnimation(GetCurrentWalkAnimation());
			break;
		case Boss56MoveState.SkillMotion:
		case Boss56MoveState.StopMotion:
			break;
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

	private void FaceToPlayer()
	{
		isFaceRight = PlayerMgr.Inst.PlayerPoint.x >= base.transform.position.x;
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
