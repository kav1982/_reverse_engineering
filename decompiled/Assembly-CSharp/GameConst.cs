using Unity.Physics;
using UnityEngine;

public class GameConst
{
	public const uint Layer_None = 0u;

	public const uint Layer_All = uint.MaxValue;

	public const uint Layer_Default = 1u;

	public const uint Layer_RelicWithMonsterAndSpell = 8u;

	public const uint Layer_NavGround = 64u;

	public const uint Layer_Invisible = 128u;

	public const uint Layer_Wall = 256u;

	public const uint Layer_Player = 512u;

	public const uint Layer_Abyss = 1024u;

	public const uint Layer_Monster = 2048u;

	public const uint Layer_Monster_Fly = 4096u;

	public const uint Layer_Monster_Ghost = 8192u;

	public const uint Layer_SpellRebound = 16384u;

	public const uint Layer_Brittleness = 32768u;

	public const uint Layer_Cliff = 65536u;

	public const uint Layer_Destructible = 131072u;

	public const uint Layer_Item = 262144u;

	public const uint Layer_NavAction = 524288u;

	public const uint Layer_Corpse = 1048576u;

	public const uint Layer_Teammate_Fly = 2097152u;

	public const uint Layer_NavFly = 4194304u;

	public const uint Layer_MonsterSpell = 8388608u;

	public const uint Layer_PlayerSpell = 16777216u;

	public const uint Layer_InteractiveObj = 33554432u;

	public const uint Layer_TriggerForPlayer = 67108864u;

	public const uint Layer_RelicWithMonster = 134217728u;

	public const uint Layer_T6Boundary = 268435456u;

	public const uint Layer_Model = 536870912u;

	public const uint Layer_CollidesWithAll = 1073741824u;

	public static readonly CollisionFilter Filter_PlayerCollider = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 512u,
		CollidesWith = 101122816u
	};

	public static readonly CollisionFilter Filter_PlayerTrigger = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 512u,
		CollidesWith = 1082360576u
	};

	public static readonly CollisionFilter Filter_None = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 0u,
		CollidesWith = 0u
	};

	public static readonly CollisionFilter Filter_Wall = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 256u
	};

	public static readonly CollisionFilter Filter_Border = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 65792u
	};

	public static readonly CollisionFilter Filter_Friendly = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 2097664u
	};

	public static readonly CollisionFilter Filter_Laser = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 2261760u
	};

	public static readonly CollisionFilter Filter_MonsterEffectBullet = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073743872u,
		CollidesWith = 19038976u
	};

	public static readonly CollisionFilter Filter_MonsterEffectBulletNoSpell = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 2261760u
	};

	public static readonly CollisionFilter Filter_MonsterGroundWave = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073743872u,
		CollidesWith = 19104512u
	};

	public static readonly CollisionFilter Filter_MonsterAoe = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073743872u,
		CollidesWith = 19038720u
	};

	public static readonly CollisionFilter Filter_MonsterAoeNoSpell = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 2261504u
	};

	public static readonly CollisionFilter Filter_MonsterAoeUndiffer = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073776640u,
		CollidesWith = 27441664u
	};

	public static readonly Color color_RarityCommon = Color.white;

	public static readonly Color color_RarityRare = new Color(0f, 0.7f, 1f, 1f);

	public static readonly Color color_RarityEpic = new Color(1f, 0.7f, 0f, 1f);

	public static readonly Color color_RaritySpecial = new Color(1f, 0f, 1f, 1f);

	public static readonly Color color_BeHit_H = new Color(0.33f, 0.33f, 0.33f, 1f);

	public static readonly Color color_BeHit = new Color(1f, 0f, 0f, 1f);

	public static readonly Color color_BeHit_Player = new Color(1f, 1f, 1f, 0f);

	public static readonly Color color_BodyVenom = new Color(0f, 1f, 0f, 1f);

	public static readonly Color color_BodyMucus = new Color(1f, 1f, 0f, 1f);

	public static readonly Color color_BodyFrozen = new Color(0.25f, 0.5f, 1f, 1f);

	public static readonly Color color_BodyBurn = new Color(1f, 0.75f, 0.75f, 1f);

	public static readonly Color color_BodyVoid = new Color(0.3f, 0.3f, 0.3f, 1f);

	public static readonly Color color_SpellRingTypeMissle = new Color(0f, 0.4706f, 0.9686f, 1f);

	public static readonly Color color_SpellRingTypeEnhance = new Color(0.8666f, 0.6392f, 0f, 1f);

	public static readonly Color color_SpellRingTypePassive = new Color(0.7176f, 0.3019f, 0.2588f, 1f);

	public static readonly Color color_SpellUseTypeMissle = new Color(0.4235f, 0.8156f, 1f, 1f);

	public static readonly Color color_SpellUseTypeEnhance = new Color(1f, 0.9137f, 0.1843f, 1f);

	public static readonly Color color_SpellUseTypePassive = new Color(1f, 0.4078f, 0.3647f, 1f);

	public static readonly Color color_InfoGrey = new Color(0.6f, 0.6f, 0.6f, 1f);

	public static string htmlColor_InfoGrey = "<color=#999999>";

	public static string colorSpellDes = "<color=#cfbf8f>";

	public static string colorRelicGroupDesc = "<color=#cfbf8f>";

	public static Vector3 doorOffsetAlone = new Vector3(0.5f, 1f, 0f);

	public static Vector3 doorOffsetDoubleLeft = new Vector3(-1f, 1f, 0f);

	public static Vector3 doorOffsetDoubleRight = new Vector3(2f, 1f, 0f);

	public static float doorOffsetX = 3f;

	public static float doorOffsetY = 1f;

	public const int activateGirlExtraFreeDisableCount = 3;

	public const int activateGirlExtraMaxDisableCount = 8;

	public const int chaosCoreRewardThroughEasy = 1;

	public const int chaosCoreRewardThroughNormal = 2;

	public const int chaosCoreRewardThroughHard = 4;

	public const int chapter4MonsterDamageRatio = 15;

	public const int chapter3TeammateDamageRatio = 2;

	public const int chapter4TeammateDamageRatio = 3;

	public const int chapter5TeammateDamageRatio = 4;

	public const int criticalDamageMultiplier = 2;

	public const int damageOfAbyssForPlayer = 10;

	public const int damageOfMoveSpike = 10;

	public const int damageOfPlayerTouchMonster = 6;

	public const int damageOfSpecialObj4 = 10;

	public const int damageOfSpike = 3;

	public const int damageOfTeamateTouchMonsterEndless = 3;

	public const int damageOfVenom = 3;

	public const int doorShortcutPassLevelCount = 4;

	public const int endlessDamageReduceStartStage = 6;

	public const int endlessDamageReduceFinishStage = 18;

	public const int endlessEliteDropCount = 30;

	public const int endlessGalleryAppearChapter = 500;

	public const int endlessHighLevelSpellStartAppearStage = 3;

	public const int endlessHighLevelSpellMaxStages = 3;

	public const int endlessStage = 300;

	public const int endlessStage1ExtraWave = 1;

	public const int endlessStageWaves = 5;

	public const int endlessMonsterSpawner = 30101;

	public const int forceRoomRelic = 5;

	public const int forceRoomCoin = 5;

	public const int forceRoomSpell = 5;

	public const int forceRoomProcess = 11;

	public const int forceRoomStore = 11;

	public const int setPharmacistPotionUseCount = 200;

	public const int rerollCost_CommonLevel1 = 2;

	public const int rerollCost_CommonLevel2 = 3;

	public const int rerollCost_CommonLevel3 = 4;

	public const int rerollCost_RareLevel1 = 3;

	public const int rerollCost_RareLevel2 = 4;

	public const int rerollCost_RareLevel3 = 5;

	public const int rerollCost_EpicLevel1 = 15;

	public const int rerollCost_EpicLevel2 = 15;

	public const int rerollCost_EpicLevel3 = 15;

	public const int testStageBegin = 50;

	public const int totalObjLayer = 10;

	public const int totalTileLayer = 10;

	public const int parasiteWormId = 705101;

	public const int fuseSummonId = 31151;

	public const int maxPostSlotShootCountInOneFrame = 10;

	public const int wandStageOfGuideFlag = 200;

	public const int SpellSummonExplosionBug = 705401;

	public const int LightningHarpoonsMaximumAttackCount = 4;

	public const float abyssDropSpeed = 4f;

	public const float angleToRadiuRatio = 0.04f;

	public const float beHitBackwardAmount = 0.1f;

	public const float beHitBackwardSpeed = 2.5f;

	public const float beHitColorTime = 0.1f;

	public const float bossDeadCreateEffectTime = 2f;

	public const float bossDeadCreateEffectIntervalTime = 0.2f;

	public const float burnDamageRatioToPlayer = 0.05f;

	public const float chaseEnemyStopDistanceSqr = 0.040000003f;

	public const float decelerationMoveRatioPlayer = 0.6f;

	public const float defaultShadowAlpha = 0.4f;

	public const float difficultyHPRatio_Nightmare1_C1 = 1.06f;

	public const float difficultyHPRatio_Nightmare1_C2 = 1.12f;

	public const float difficultyHPRatio_Nightmare1_C3 = 1.18f;

	public const float difficultyHPRatio_Nightmare1_C4 = 1.24f;

	public const float difficultyHPRatio_Nightmare1_C5 = 1.3f;

	public const float difficultyHPRatio_Nightmare2_C1 = 1.08f;

	public const float difficultyHPRatio_Nightmare2_C2 = 1.21f;

	public const float difficultyHPRatio_Nightmare2_C3 = 1.34f;

	public const float difficultyHPRatio_Nightmare2_C4 = 1.47f;

	public const float difficultyHPRatio_Nightmare2_C5 = 1.6f;

	public const float difficultyHPRatio_Nightmare3_C1 = 1.2f;

	public const float difficultyHPRatio_Nightmare3_C2 = 1.4f;

	public const float difficultyHPRatio_Nightmare3_C3 = 1.6f;

	public const float difficultyHPRatio_Nightmare3_C4 = 1.8f;

	public const float difficultyHPRatio_Nightmare3_C5 = 2f;

	public const float discountRatio = 0.7f;

	public const float findNearestTargetAngleToDis = 0.1f;

	public const float endlessDaveRelicChance = 0.3f;

	public const float endlessLostCastleRuneChance = 0.3f;

	public const float endlessRareRelicChance = 0.2f;

	public const float endlessRareSpellChance = 1.2f;

	public const float endlessEpicRelicChance = 0.03f;

	public const float endlessEpicSpellChance = 0.03f;

	public const float endlessHighLevelSpellChance = 0.04f;

	public const float endlessMonster316SpeedBuffRatio = 1.2f;

	public const float endlessMonster316DamageBuffRatio = 1f;

	public const float endlessMaxDamageReduce = 0.9f;

	public const float endlessSpellPriceMultPerLevel = 1.5f;

	public const float endlessItemPriceRatio = 1.3f;

	public const float heightOfLava0 = 1.4f;

	public const float heightOfLava1 = 1.39f;

	public const float heightOfLava2 = 1.38f;

	public const float heightOfCliff = 1.37f;

	public const float heightOfLava3 = 1.36f;

	public const float heightOfTile0 = 1.35f;

	public const float heightOfTile1 = 1.34f;

	public const float heightOfTile2 = 1.33f;

	public const float heightOfTile3 = 1.32f;

	public const float heightOfTile4 = 1.31f;

	public const float heightOfBoundaryAO = 1.3f;

	public const float heightOfTile5_AboveAO = 1.29f;

	public const float heightOfTile6_AboveAO = 1.28f;

	public const float heightOfTile7_AboveAO = 1.27f;

	public const float heightOfTile8_AboveAO = 1.26f;

	public const float heightOfTile9_AboveAO = 1.25f;

	public const float heightOfExplosionTrace = 1.24f;

	public const float heightOfAccessOpen = 1.23f;

	public const float heightOfSO13 = 1.22f;

	public const float heightOfSO7 = 1.21f;

	public const float heightOfSO15 = 1.2f;

	public const float heightOfCorpse = 1.19f;

	public const float heightOfWater = 1.18f;

	public const float heightOfBlood = 1.17f;

	public const float heightOfMucus = 1.16f;

	public const float heightOfVenom = 1.15f;

	public const float heightOfVenomBubble = 1.14f;

	public const float heightOfSO38 = 1.13f;

	public const float heightOfGroundEffectLower = 1.12f;

	public const float heightOfElite7Trap = 1.11f;

	public const float heightOfWarningArea = 1.1f;

	public const float heightOfSpecialObj8 = 1.09f;

	public const float heightOfGroundEffect = 1.08f;

	public const float heightOfT6Door = 1.07f;

	public const float heightOfShackle = 1.06f;

	public const float heightOfShadow = 1.05f;

	public const float heightOfTreeRoot = 1.04f;

	public const float heightOfBoundaryLow = 1.03f;

	public const float heightOfSlimeOnGround = 1.02f;

	public const float heightOfSO29 = 1.01f;

	public const float heightOfEndlessBoundary = 1f;

	public const float heightOfChapter3Up = 0.9f;

	public const float heightOfChapter3Right = 0.9f;

	public const float heightOfChapter3Down = 0.9f;

	public const float heightOfChapter3Left = 0.9f;

	public const float heightOfBoundaryHigh = -1f;

	public const float heightOfChapter1Leaf = -1.01f;

	public const float heightOfGhost = -1.02f;

	public const float heightOfChapter3Boundary = -2f;

	public const float heightOfRoomParticle = -2.01f;

	public const float heightOfFog = -50f;

	public const float heightOfRTBlood = -105f;

	public const float heightOfRTWater = -110f;

	public const float heightOfRTMucus = -120f;

	public const float heightOfRTVenom = -130f;

	public const float heightOfRTPlayer = -150f;

	public const float heightOfRTElite7Trap = -160f;

	public const float heightOfRTBoss3Stage2 = -170f;

	public const float heightOfRTBoss10Shadow = -2080f;

	public const float heightOfUnderUnitSpellLRBonus = 0.1f;

	public const float heightRatio = 0.01f;

	public const float intervalOfCheckTarget = 1f;

	public const float intervalOfCheckNavInfo = 0.1f;

	public const float intervalOfPlayerInvincibleFrame = 0.33f;

	public const float intervalOfPlayerInvincibleFrameTwinkle = 0.05f;

	public const float playerInvincibleFrameTwinkleAlpha = 0.3f;

	public const float intervalOfSpikesInjury = 0.33f;

	public const float intervalOfTouchInjury = 0.33f;

	public const float intervalOfVenomInjury = 1f;

	public const float intervalOfBurnInjury = 0.33f;

	public const float itemRadius = 0.25f;

	public const float levelRewardInterval = 1f;

	public const float potionHolyWaterGetChance = 0.05f;

	public const float radiusOfSpellSplit = 0.5f;

	public const float damageOfSpellSplit = 0.33f;

	public const float radiusOfRotatingSplitSpell = 1.5f;

	public const float ratioOfScatterAngleToSpreadDistance = 0.06f;

	public const float rigidDeclineLerp = 5f;

	public const float roomOffset = 60f;

	public const float roomOffsetGuide = 40f;

	public const float shootSlowdownMinRate = 0.6f;

	public const float shootSlowdownDuration = 0.2f;

	public const float shootSlowdownLerp = 25f;

	public const float spellSizePow = 0.3333f;

	public const float summonSizePow = 0.5f;

	public const float slotLackAlertInterval = 1f;

	public const float spellAttackDistanceRatio = 0.9f;

	public const float spellMinRadiuRatio = 0.1f;

	public const float superNovaMaxDuration = 6f;

	public const float teammateFuseDissolveDuration = 1.3f;

	public const float timeOfFrozenImmune = 2f;

	public const float timeOfHideRoomAppearDelay = 0.1f;

	public const float timeOfInverseMove = 0.5f;

	public const float timeOfSpellDisappear = 0.2f;

	public const float timeOfUISpellFly = 0.1f;

	public const float timeOfUnitBornIdle = 0.5f;

	public const float timeOfUsePotion = 1f;

	public const float timeOfLandVenomCommonDuration = 6f;

	public const float touristModeDamageRatio = 0.5f;

	public const float undifferDamageRatio = 0.3333f;

	public const float unitBaseMoveLerp = 10f;

	public const float unitPathFindingInterval = 0.1f;

	public const float minWandCoodDown = 0.015f;

	public const float maxWandCoodDownAtLowFPS = 0.15f;

	public const float PcLowFPSOptimizeActiveThresholdFPS = 35f;

	public const float PcLowFPSOptimizeActiveMinFPS = 10f;

	public const float PcLowFPSDefaultMaxBonusTimeScale = 3f;

	public const float PcMaxOptimizeFPSThreshold = 10f;

	public const float MobileLowFPSOptimizeActiveThresholdFps = 20f;

	public const float MobileLowFPSOptimizeActiveMinFps = 10f;

	public const float MobileLowFPSDefaultMaxBonusTimeScale = 5f;

	public const float MobileMaxOptimizeFPSThreshold = 5f;

	public const float attackPullBackTimeSlowScale = 0.05f;

	public const float attackPullBackTimeSlowDuration = 0.18f;

	public const float attackPullBackTimeSlowFadeSpeed = 5f;

	public const float attackPullBackCameraFocusRatio = -0.3f;

	public const float attackPullBackCameraFocusEffectDuration = 0.16f;

	public const float attackPullBackCameraFocusProgressDuration = 0.02f;

	public const float VoidExplosionStickTime = 3f;

	public const float splitSpellDamageRatio = 0.35f;

	public const float splitSpellRangeRatio = 0.8f;

	public const float splitSpellAttributeRatio = 0.5f;

	public const float halfLifeTeleportBonusDuration = 2f;

	public const float falfLifeTeleportBonusSpeed = 2f;

	public const float diveSuitOpenChestExtraHarpoonsBaseChance = 0.4f;

	public const float diveSuitOpenChestExtraHarpoonsBonusChance = 0.13f;

	public const float diveSuitRareHarpoonsChance = 0.5f;

	public const float diveSuitLevel2BonusHarpoonsChance = 0.5f;

	public const float ThunderAuraOutRangeEnemyDetectRangeRatio = 1.5f;

	public const float ManaCoinCostPercentage = 0.2f;

	public const float SpiritMaxRotateAroundPlayerLerpRatio = 2f;

	public const float WandSpiritAroundPlayerRadius = 1.2f;

	public const float WandSpiritAroundBackToPlayerDistance = 4f;

	public const float harpoonsMaxHoldDuration = 3f;

	public const float daveShotGunBaseRange = 3.5f;

	public const float daveShotGunSpeedToRangeRatio = 1f;

	public const float daveShotGunTimeToRangeRatio = 1.5f;

	public const float BladeFadeInTime = 0.18f;

	public const float BladeAfterShootMinLifeTime = 2f;

	public const float BladeRotateAroundOwnerBaseRadius = 1.5f;

	public const int SurplusBladeThreshold = 40;

	public const float SurplusBladesBonusRadius = 0.005f;

	public const float SurplusBladesMaxRadius = 3f;

	public const float BladeAroundOwenrLerpRatio = 6f;

	public const float BladeSpawnDisMinRatio = 0.7f;

	public const float BladeSpawnDisMaxRatio = 1.3f;

	public const float BladeRecheckTargetInterval = 0.1f;

	public const float BladeLockRotateLerpSpeed = 60f;

	public const float RotateMovementBladeMaxShiftRadius = 0.4f;

	public const float BladeBaseHeight = 0.65f;

	public const float BoBoMoveAnimationPeriod = 0.4f;

	public const float BoBoAttackInterval = 0.15f;

	public const float BoBoAttackCoolDown = 0.8f;

	public const float BoBoAttackMouseOpenDuration = 0.12f;

	public const float BoBoIdleWalkRandomMinRange = 1f;

	public const float BoBoIdleWalkRandomMaxRange = 3f;

	public const float BoBoIdleWalkMinCoolDownInterval = 1f;

	public const float BoBoIdleWalkMaxCoolDownInterval = 2f;

	public const float BoBoIdleWalkMoveSpeedRatio = 0.3f;

	public const float BossKeIdleWalkMoveSpeedRatio = 0.5f;

	public const float BossKeAttackInterval = 0.35f;

	public const float BossKeLegMoveSpeedRatio = 2.2f;

	public const float BossKeIdleWalkMinCoolDownInterval = 2f;

	public const float BossKeIdleWalkMaxCoolDownInterval = 3f;

	public const float BossKeIdleWalkRandomMinRange = 1f;

	public const float BossKeIdleWalkRandomMaxRange = 3f;

	public const float BossKeMinScale = 0.8f;

	public const float BossKeMaxScale = 1.5f;

	public const float TentacleAttackingHoldingTime = 0.4f;

	public const float GrimoireFloatingHeightRatio = 0.15f;

	public const float GrimoireFloatingBaseHeight = 0.4f;

	public const float GrimoireFloatingAttackingBaseHeight = 0.3f;

	public const float GrimoireFloatingBaseHeightShiftSpeed = 6f;

	public const float GrimoireFloatingSpeed = 3.5f;

	public const float GrimoireOpenCloseBookTime = 0.3f;

	public const float teammate6EachKillDamageUpRatio = 0.01f;

	public const float teammate6EachKillHpUpRatio = 0.01f;

	public const float teammate6IdleMinInterval = 1f;

	public const float teammate6IdleMaxInterval = 2f;

	public const float teammate6IdleWalkMinDuration = 2f;

	public const float teammate6IdleWalkMaxDuration = 3f;

	public const float teammate6IdleWalkMinDistance = 2f;

	public const float teammate6IdleWalkMaxDistance = 5f;

	public const float teammate6IdleWalkSpeedRatio = 0.4f;

	public const float teammate6RecheckTargetInterval = 0.1f;

	public const float teammate6CloseAttackRange = 2.8f;

	public const float teammate6MeleeAttackDamage = 96f;

	public const float teammate6BombBaseRange = 3.8f;

	public const float teammate7ChildBugHpInheritRatio = 0.3f;

	public const float RedRuneBonusCriticalChancePerRune = 1.5f;

	public const int GreenRuneBonusHpPerRune = 3;

	public const int BlueRuneBonusMpPerRune = 5;

	public const float RedRuneAOEChargeRequire = 10f;

	public const int RedRuneBaseDamage = 15;

	public const float RedRuneTriggerInterval = 0.08f;

	public const float RedRuneLV3ExtraDamagePerRune = 0.1f;

	public const float RedRuneLV5AOERuneBaseRange = 2.4f;

	public const float GreenRuneChargeDuration = 3f;

	public const int GreenRuneMaxChargeCount = 5;

	public const int GreenRuneLV2DecreaseDamageReceiveRequireCount = 7;

	public const int GreenRuneLV3BonusExplosionOnTeammateRequireCount = 10;

	public const float GreenRuneLV4MaxHpToDamageRatio = 0.4f;

	public const int GreenRuneLV5SummonTeammateExplosionRequireCount = 10;

	public const float BlueRuneLV3MaxMPToDamageRatio = 0.2f;

	public const float BlueRuneStartChaseTimer = 0.2f;

	public const float BlueRuneLV1MpGenEffect = 0.5f;

	public const int SuperBlueRuneCounterRequire = 2;

	public const float BlueRuneTargetTakeDamageUpRatio = 5f;

	public const float BlueRuneTargetTakeDamageUpDuration = 5f;

	private const float spellFallAngle = 75f;

	public static readonly float spellFallAngleCos = Mathf.Cos(1.3089969f);

	public static readonly float spellFallAngleTan = Mathf.Tan(1.3089969f);

	public const float spellFallHeight = 7f;

	public const float spellFallOffset = 2f;

	public const int groundVenomApplyStack = 2;

	public const int groundVenomMaxStack = 99999999;

	public const float groundVenomDuration = 2f;

	public const float groundMucusMoveSpeedRatio = 0.6f;

	public const float BoboBulletGravity = 13f;

	public const float BoboBulletInitialUpSpeed = -4f;

	public const int navAreaMaskAction = 8;

	public const int navAreaMaskGround = 16;

	public const int navAreaMaskFly = 32;

	public const float navAreaMaskZOffsetAction = 4.35f;

	public const float navAreaMaskZOffsetGround = -0.05f;

	public const float navAreaMaskZOffsetFly = 4.25f;

	public const float navAreaMeshZOffsetAction = 4.4f;

	public const float navAreaMeshZOffsetGround = 0f;

	public const float navAreaMeshZOffsetFly = 4.3f;

	public const float stuckCheckInterval = 2.5f;

	public const float stuckCheckIntervalPlayer = 1f;

	public const float stuckCheckIntervalItem = 10f;

	public const float WandShootSpellLowFPSThreshold = 40f;

	public const float ArcaneNovaLowFPSThreshold = 30f;

	public const float MeteorTraceLowFPSThreshold = 60f;

	public const float MeteorTraceStopSpawnTraceFPSThreshold = 10f;

	public const float SplitLowFPSThreshold = 40f;

	public const float ThunderAuraLowFPSThreshold = 30f;

	public const float ThunderAuraLowFPSMaxTimeScale = 10f;

	public const float GhostFireLowFPSMaxTimeScale = 20f;

	public const float GeneralSpellLowFPSMaxTimeScale = 5f;

	public const float GeneralSpellLowFPSThreshold = 30f;

	public const float ExplosionBugLowFPSMaxTimeScale = 10f;

	public const int DefaultMaxCrystalPerWand = 15;

	public const int DefaultMaxSplitCrystalPerWand = 30;

	public const int DefaultMaxHammerPerWand = 64;

	public const int DefaultMaxSplitHammerPerWand = 128;

	public const int beHitTime = 1;

	public const float beHitDistance = 0.2f;

	public const float beHitSpeed = 6f;

	public const float playerBodySizePow = 0.5f;

	public const float playerGamepadAimDistanceSearch = 12f;

	public const float playerGamepadAimDistanceMax = 8f;

	public const float playerMobileDistanceSearchSqr = 230f;

	public const float playerMobileAutoAimAngle = 60f;

	public const float playerGamepadAutoAimAngleToDisSqr = 0.05f;

	public const float playerMinBodySize = 0.0001f;

	public const float playerMinMoveSpeed = 1f;

	public const float wandSwitchCoolingTime = 0.2f;

	public const float wandSlotNormalFrameSize = 64f;

	public const int wandSlotTexurePixelsPerUnit = 100;

	public const float wandSlotMoveDistanceMoveUpbound = 5f;

	public const int wandPostSlotHighDamageThreshold = 45;

	public const int wandPostSlotHighManaThreshold = 30;

	public const string wandSpellBreakerHeadPrefabName = "SpellBreakWandHead";

	public const int theme6GroundHeight = 17;

	public const int theme6GroundWidth = 27;

	public const int theme6RoomMaxHeight = 13;

	public const int theme6RoomMaxWidth = 23;

	public const float theme8CornerTangentRatio = 0.56f;

	public const float theme8ColliderHeight = 20f;

	public const int theme8GroundMaxHeight = 46;

	public const int theme8GroundMaxWidth = 46;

	public const int theme8GroundMinHeight = 4;

	public const int theme8GroundMinWidth = 8;

	public const int theme14GroundMaxHeight = 46;

	public const int theme14GroundMaxWidth = 46;

	public const int theme14GroundMinHeight = 2;

	public const int theme14GroundMinWidth = 2;

	public const int theme26GroundMaxHeight = 46;

	public const int theme26GroundMaxWidth = 46;

	public const int theme26GroundMinHeight = 2;

	public const int theme26GroundMinWidth = 2;

	public const string sceneNameCamp = "Camp";

	public const string sceneNameBattle = "Battle";

	public const string sceneNameEntry = "Entry";

	public const string sceneNameGuide = "Guide";

	public const string sceneNameGuide2 = "Guide2";

	public const string sceneNameMainMenu = "MainMenu";

	public const string sceneNameEasyFinishBackHome = "EasyFinishBackHome";

	public const string sceneNameNPC7Appearance = "NPC7Appearance";

	public const string emptyScene = "emptyscene";

	public const string ambinet_Guide = "Ambinet_Guide";

	public const string bgm_BossChapter5 = "BGM_BossChapter5";

	public const string bgm_Camp = "BGM_Camp";

	public const string bgm_Camp_Dave = "BGM_Camp_Dave";

	public const string bgm_Camp_Endless = "BGM_Camp_Endless";

	public const string bgm_Camp_Halloween = "BGM_Camp_Halloween";

	public const string bgm_Camp_LunarNewYear = "BGM_Camp_LunarNewYear";

	public const string bgm_Camp_Summer = "BGM_Camp_Summer";

	public const string bgm_MainMenu = "BGM_MainMenu";

	public const string bgm_SpringDave = "BGM_Spring_Dave";

	public const float SummonSpiritEssence_Teammate1_BombChance = 0.2f;

	public const float SummonSpiritEssence_Teammate1_BombChanceUpPerLevel = 0.1f;

	public const float SummonSpiritEssence_Teammate1_BombBaseRatio = 5f;

	public const float SummonSpiritEssence_Teammate1_BombBaseRatioUpPerLevel = 15f;

	public const float SummonSpiritEssence_Teammate1_BombBaseRange = 2.2f;

	public const float SummonSpiritEssence_Teammate1_BombCloseAttackRange = 1.5f;

	public const float SummonSpiritEssence_Teammate1_BombCloseAttackLandTime = 0.35f;

	public const float SummonSpiritEssence_Teammate1_BombLandDistanceSpeedRatio = 7.5f;

	public const float SummonSpiritEssence_Teammate1_BoBoBombShootPrepareInterval = 0.6f;

	public const float SummonSpiritEssence_Teammate1_BombMinLandTime = 0.8f;

	public const float SummonSpiritEssence_Teammate1_BombMaxLandTime = 1.1f;

	public const float SummonSpiritEssence_Teammate1_BombMinHeightRange = 1.5f;

	public const float SummonSpiritEssence_Teammate1_BombMaxHeightRange = 2.6f;

	public const int SummonSpiritEssence_Teammate2_MaxLegGroupCount = 13;

	public const float SummonSpiritEssence_Teammate2_HitBaseDuration = 0.25f;

	public const float SummonSpiritEssence_Teammate2_HitDamageRatio = 60f;

	public const int SummonSpiritEssence_Teammate3_AttackCountRequirement = 4;

	public const float SummonSpiritEssence_Teammate3_AttackSpeedUp = 20f;

	public const float SummonSpiritEssence_Teammate3_TentacleSpawnerBaseDuration = 1.75f;

	public const float SummonSpiritEssence_Teammate3_TentacleSpawnerAttackInterval = 0.06f;

	public const float SummonSpiritEssence_Teammate3_TentacleSpawnerAttackRange = 0.7f;

	public const float SummonSpiritEssence_Teammate3_TentacleSpawnerAttackDamageRatio = 0.8f;

	public const float SummonSpiritEssence_Teammate4_AttackBaseCd = 2.5f;

	public const float SummonSpiritEssence_Teammate4_AttackBaseEffectRange = 1.5f;

	public const float SummonSpiritEssence_Teammate4_AttackDamageHpRatio = 100f;

	public const float SummonSpiritEssence_Teammate4_AttackDamageHpRatioUpPerLevel = 20f;

	public const float SummonSpiritEssence_Teammate5_BaseTeleportCd = 3f;

	public const float SummonSpiritEssence_Teammate5_MinTeleportCd = 0.6f;

	public const float SummonSpiritEssence_Teammate5_IntialTeleportCooldown = 0.2f;

	public const float SummonSpiritEssence_Teammate5_TeleportStartTime = 0.35f;

	public const float SummonSpiritEssence_Teammate5_TeleportCdDecreasePerLevel = 0.5f;

	public const int SummonSpiritEssence_Teammate5_TeleportAttackCountNeed = 9;

	public const int SummonSpiritEssence_Teammate5_ChargeAttackCountNeed = 9;

	public const int SummonSpiritEssence_Teammate6_HookCountPerLevel = 4;

	public const int SummonSpiritEssence_Teammate7_SpawnWormCountPerLevel = 8;

	public const float SummonSpiritEssence_Teammate7_SpawnExplosionBaseEffectRange = 3.5f;

	public const float SummonSpiritEssence_Teammate7_SpawnExplosionHpDamageRatio = 180f;

	public const int Teammate1SummonLimit = 5;

	public const int Teammate2SummonLimit = 1;

	public const int Teammate3SummonLimit = 1000;

	public const int Teammate4SummonLimit = 24;

	public const int Teammate5SummonLimit = 3;

	public const int Teammate6SummonLimit = 1;

	public const int Teammate7SummonLimit = 2;

	public const int PCMaxMultiCountInOneFrameL1 = 60;

	public const int PCMaxMultiCountInOneFrameL2 = 80;

	public const int MobileMaxMultiCountInOneFrameL1 = 40;

	public const int MobileMaxMultiCountInOneFrameL2 = 60;

	public const int L1MaxMultiCount = 2;

	public const int L2MaxMultiCount = 1;

	public const int PCMaxSplitCountInOneFrameL1 = 90;

	public const int PCMaxSplitCountInOneFrameL2 = 120;

	public const int MobileMaxSplitCountInOneFrameL1 = 45;

	public const int MobileMaxSplitCountInOneFrameL2 = 60;

	public const int L1MaxSplitCount = 3;

	public const int L2MaxSplitCount = 1;

	public const int PCMaxWormCountInOneFrameL1 = 60;

	public const int PCMaxWormCountInOneFrameL2 = 75;

	public const int MobileMaxWormCountInOneFrameL1 = 30;

	public const int MobileMaxWormCountInOneFrameL2 = 45;

	public const int L1MaxWormCount = 3;

	public const int L2MaxWormCount = 1;

	public const int PCMaxTwineCountInOneFrameL1 = 150;

	public const int PCMaxTwineCountInOneFrameL2 = 180;

	public const int MobileMaxTwineCountInOneFrameL1 = 100;

	public const int MobileMaxTwineCountInOneFrameL2 = 120;

	public const int L1MaxTwineCount = 2;

	public const int L2MaxTwineCount = 1;

	public const int PCMaxOverSplitCountInOneFrameL1 = 28;

	public const int PCMaxOverSplitCountInOneFrameL2 = 36;

	public const int MobileMaxOverSplitCountInOneFrameL1 = 16;

	public const int MobileMaxOverSplitCountInOneFrameL2 = 24;

	public const int L1MaxOverSplitCount = 2;

	public const int L2MaxOverSplitCount = 1;

	public const string fuseTeammate1ID = "700111";

	public const string fuseTeammate2ID = "700211";

	public const string fuseTeammate3ID = "700311";

	public const string fuseTeammate4ID = "700411";

	public const string fuseTeammate5ID = "700511";

	public const string fuseTeammate6ID = "700601";

	public const string fuseTeammate7ID = "700701";
}
