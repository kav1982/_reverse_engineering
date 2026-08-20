using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class SpellBase : LayerCorrect
{
	[HideInInspector]
	public Rigidbody rigid;

	[HideInInspector]
	public TriggerIn triggerIn;

	[HideInInspector]
	public SphereCollider sc_Rebound;

	[HideInInspector]
	public BoxCollider bc_Rebound;

	[HideInInspector]
	public Shadow shadow;

	protected SpellEffectBase EffectBase;

	[HideInInspector]
	public UnitProperty ownerPpt;

	[HideInInspector]
	public float spellMucusTime;

	[HideInInspector]
	public float spellMucusMoveSpeedRatio = 1f;

	[HideInInspector]
	public float spellMucusSpellSpeedRatio = 1f;

	[HideInInspector]
	public float spellVenomTime;

	[HideInInspector]
	public float spellVenomOnceCount;

	[HideInInspector]
	public float spellFrozenTime;

	private static readonly Collider[] thunderColliderBuffer = new Collider[256];

	private static readonly HashSet<string> thunderColliderTags = new HashSet<string> { "Monster", "Destructible", "Butterfly", "RollBall", "Brittleness" };

	[HideInInspector]
	public Spell3129VoidExplosion.VoidExplosionData voidExplosionInfo;

	protected bool enableNormalTransform = true;

	protected bool enableFollowMouse = true;

	[HideInInspector]
	public float spellFollowMouseLerp;

	protected bool enableFollowTarget = true;

	[HideInInspector]
	public float spellFollowTargetRotateSpeed;

	[HideInInspector]
	public UnitProperty spellFollowTargetPpt;

	[HideInInspector]
	public float lifelineDamage;

	[HideInInspector]
	public float lifelineDamageInterval = float.PositiveInfinity;

	public bool createReboundEffect = true;

	public bool CanChangeTeam = true;

	[HideInInspector]
	public bool isThroughWall;

	public readonly List<GameObject> refractedTargets = new List<GameObject>();

	private ISpellTriggerController _triggerCtrl;

	public int effectPrefix;

	[HideInInspector]
	public List<int> preSpellIDs;

	[HideInInspector]
	public bool FromEcho;

	private bool secondFrameDataInitialized;

	protected Vector3 lastAroundTargetPosition;

	private bool firstFrameDataInitialized;

	private bool isSingleInitialized;

	protected bool spellEndTeleport;

	private bool _isFlyFinish;

	private bool isOwnerDead;

	protected int lastOutputDamageFrame;

	private float aroundOwnerRadiuToSpeedRatio = 1f;

	private bool emitFlyFinishEvent;

	public readonly SpellEvent<SpellBase> OnFlyFinishIgnoreRecycle = new SpellEvent<SpellBase>();

	public readonly SpellEvent<SpellBase> OnWillRecycleIgnoreRecycle = new SpellEvent<SpellBase>();

	private bool isReboundOriginalEnable;

	protected int halfLifeTeleportCount;

	private readonly HashSet<string> fallingGroundDamageTargetsTags = new HashSet<string> { "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness", "Spell" };

	private readonly Collider[] fallingGroundDamageTargetsBuffer = new Collider[512];

	public float damageRatio { get; set; } = 1f;


	public float finalDamageRatio { get; set; } = 1f;


	public float bonusSpeed { get; set; }

	public float speedRatio { get; set; } = 1f;


	public float finalSpeedRatio { get; set; } = 1f;


	public float radiusRatio { get; set; } = 1f;


	public float finalRadiusRatio { get; set; } = 1f;


	public float knockbackRatio { get; set; } = 1f;


	public float finalKnockbackRatio { get; set; } = 1f;


	public float bonusDuration { get; set; }

	public float finalDurationRatio { get; set; } = 1f;


	public float _angle { get; set; }

	public float wandShootAngle { get; set; }

	public float overalCriticalChance { get; set; }

	public Vector3 Direction { get; set; }

	public float spellVolumeRatio { get; set; } = 1f;


	public Vector3 originalShootDirection { get; set; } = Vector3.zero;


	public float ColliderRadius { get; private set; }

	public SpellConfig spellCfg { get; set; }

	public SpellInitialParameter InitialParameter { get; set; }

	public SpellShootData ShootData { get; set; }

	public SpellInitialParameter SIP => InitialParameter;

	public Transform OwnerTsf { get; set; }

	public Vector3 OwnerPoint { get; set; }

	public SpellBase OwnerSpell { get; set; }

	public Wand shooterWand { get; set; }

	public float endThunderHitRadiu { get; set; }

	public float endThunderHitPercent { get; set; }

	public float endTHunderHitChance { get; set; }

	public float spellBurnTime { get; set; }

	public float burnHpRatioPerSeconds { get; set; }

	public bool SpellFollowHaveTarget
	{
		get
		{
			if (spellFollowTargetPpt != null && spellFollowTargetPpt.gameObject.activeSelf && spellFollowTargetPpt.CanBeTarget)
			{
				return true;
			}
			return false;
		}
	}

	public bool enableAroundPlayer { get; protected set; } = true;


	public float spellAroundOwnerRadius { get; set; }

	public float spellAroundOwnerCurrentAngle { get; set; }

	public float criticalDragDamagePercent { get; set; }

	public float criticalDragEffectRadiu { get; set; }

	public int criticalDragApllyToCount { get; set; }

	public float criticalDragPullForce { get; set; }

	public bool shouldCameraShock { get; set; } = true;


	public bool CanBeCapture { get; set; } = true;


	public int SpellSummonAfterDeadSpawnWormCount { get; set; }

	public float SpellSummonInstantDeathHpRatio { get; set; }

	public float SpellSummonDeathExplodeRange { get; set; }

	public float SpellSummonDeathExplodeHpDamageRatio { get; set; }

	public float SpellSummonHPRatio { get; protected set; } = 1f;


	public float SpellSummonGainOwnerHpRatio { get; protected set; }

	public float SpellSUmmonFinalHpRatio { get; protected set; } = 1f;


	public float SpellSummonMoveRatio { get; set; } = 1f;


	public float SpellSummonHPRecover { get; protected set; }

	public float SpellSummonHPFixDropAmount { get; set; }

	public float SpellSummonAttackSpeedRatio { get; set; } = 1f;


	public int spellSplitCount { get; set; }

	public float SpellHoverTime { get; protected set; }

	public float SpellHoverTimer { get; protected set; }

	public float extraReboundTime { get; set; }

	public float reboundAddTime { get; set; }

	public int rebounceTime { get; set; }

	public float lightningChainDamage { get; set; }

	public bool InFallRebounding { get; protected set; }

	protected virtual float InFallingReboundingGravity => 43f;

	protected virtual float FallingReboundForce => Mathf.Clamp((0f - CurrentUpSpeed) * 0.7f, InFallingReboundingGravity / 3f, 9999f);

	protected virtual float FallSpeedRatio => 1.2f;

	protected virtual float FallInitialHeight => 7f;

	protected virtual bool fallSpellIgnoreTriggerIn => SIP.spellIsFall;

	public int remainRefractCount { get; set; }

	protected virtual Vector3 checkNextRefractionPosition => base.transform.position;

	public ISpellTriggerController TriggerCtrl
	{
		get
		{
			if (_triggerCtrl == null)
			{
				InitTriggerController(null, null);
			}
			return _triggerCtrl;
		}
		set
		{
			_triggerCtrl = value;
		}
	}

	public SpellColorType ColorType { get; set; }

	public SpellSpecialMovementType currentSpellMovement { get; set; }

	public SpellConfig level1Cfg { get; protected set; }

	public SpellConfig currentLevelPureCfg { get; protected set; }

	public SpellConfig InitialWithConfig { get; private set; }

	public bool finishedInitialize { get; protected set; }

	public bool CreateFromMiniPool { get; set; }

	public bool controlByRemoteWand { get; set; }

	public bool spellKeepCastBuffApplied { get; protected set; }

	public bool indirectShootByPlayer { get; set; }

	public bool isFlyFinish
	{
		get
		{
			return _isFlyFinish;
		}
		protected set
		{
			if (!_isFlyFinish && value && !emitFlyFinishEvent)
			{
				OnFlyFinishIgnoreRecycle.Invoke(this);
				emitFlyFinishEvent = true;
			}
			_isFlyFinish = value;
		}
	}

	public bool isOwnerSpellEnd { get; set; }

	public float TeammateTakDamageRatio { get; set; } = 1f;


	public float undifferDamageRatio { get; set; } = 1f;


	public float DurationTimer { get; protected set; }

	public Vector3 virtualRealPosition { get; set; }

	public float Height
	{
		get
		{
			return 0f - base.transform.position.z;
		}
		set
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 0f - value);
		}
	}

	public int penetrateTime { get; set; }

	public float CurrentSpeed { get; set; }

	public float CurrentUpSpeed { get; set; }

	public WandPostSlotChargeData wandChargeData { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		finishedInitialize = false;
		CreateFromMiniPool = false;
	}

	private void SingleInitial()
	{
		if (!isSingleInitialized)
		{
			EffectBase = GetComponent<SpellEffectBase>();
			isSingleInitialized = true;
			rigid = GetComponent<Rigidbody>();
			triggerIn = GetComponentInChildren<TriggerIn>();
			if (triggerIn != null)
			{
				triggerIn.Initialize(TriggerIn);
			}
			shadow = GetComponent<Shadow>();
			if (shadow != null && tsf_Layer != null)
			{
				shadow.CreateShadow();
				shadow.ShadowGO.transform.parent = tsf_Layer;
			}
			sc_Rebound = GetComponent<SphereCollider>();
			bc_Rebound = GetComponent<BoxCollider>();
			if (sc_Rebound != null)
			{
				isReboundOriginalEnable = sc_Rebound.enabled;
				ColliderRadius = sc_Rebound.radius;
			}
			else if (bc_Rebound != null)
			{
				isReboundOriginalEnable = bc_Rebound.enabled;
				ColliderRadius = (bc_Rebound.size.x + bc_Rebound.size.y) / 4f;
			}
			SingleInitialCallback();
		}
	}

	public void Initialize(UnitProperty ownerPp, Vector3 dir, SpellConfig cfg, Wand casterWand = null, params int[] preSpellIds)
	{
		FinalInitialize(new SpellInitialParameter(ownerPp, dir, 0, cfg, casterWand, preSpellIds.ToList()));
	}

	public void Initialize(UnitProperty ownerPp, Vector3 dir, int shootSpellID, float wandAngl, List<int> preSpellIds, Wand casterWand = null)
	{
		InitialWithConfig = null;
		FinalInitialize(new SpellInitialParameter(ownerPp, dir, shootSpellID, SpellConfig.GetConfigCopy(shootSpellID), casterWand, preSpellIds));
	}

	public void Initialize(SpellInitialParameter initialParameter)
	{
		if (initialParameter.spelldataConfig != null)
		{
			InitialWithConfig = initialParameter.spelldataConfig.Copy();
		}
		FinalInitialize(initialParameter.Copy());
	}

	public void InitializeV2(SpellInitialParameter initialParameter, SpellShootData shootData)
	{
		if (initialParameter.spelldataConfig != null)
		{
			InitialWithConfig = initialParameter.spelldataConfig.Copy();
		}
		FinalInitialize(initialParameter.Copy(), shootData);
	}

	protected virtual void FinalInitialize(SpellInitialParameter initialParameter, SpellShootData shootData = null)
	{
		SingleInitial();
		emitFlyFinishEvent = false;
		CurrentSpeed = 0f;
		CurrentUpSpeed = 0f;
		ColorType = SpellColorType.Player;
		DurationTimer = 0f;
		shouldCameraShock = true;
		isFlyFinish = false;
		wandShootAngle = 0f;
		originalShootDirection = Vector3.zero;
		undifferDamageRatio = 1f;
		if (tsf_Layer != null)
		{
			tsf_Layer.localScale = Vector3.one;
		}
		if (sc_Rebound != null)
		{
			sc_Rebound.enabled = isReboundOriginalEnable;
		}
		else if (bc_Rebound != null)
		{
			bc_Rebound.enabled = isReboundOriginalEnable;
		}
		spellKeepCastBuffApplied = false;
		penetrateTime = 0;
		SpellHoverTime = 0f;
		SpellHoverTimer = 0f;
		spellMucusTime = 0f;
		voidExplosionInfo = null;
		bonusDuration = 0f;
		finalDurationRatio = 1f;
		spellMucusMoveSpeedRatio = 1f;
		spellMucusSpellSpeedRatio = 1f;
		spellVenomTime = 0f;
		spellVenomOnceCount = 0f;
		spellFrozenTime = 0f;
		spellAroundOwnerRadius = 0f;
		spellAroundOwnerCurrentAngle = 0f;
		spellFollowMouseLerp = 0f;
		spellFollowTargetRotateSpeed = 0f;
		spellFollowTargetPpt = null;
		rebounceTime = 0;
		spellSplitCount = 0;
		SpellSummonGainOwnerHpRatio = 0f;
		SpellSummonHPRatio = 1f;
		SpellSUmmonFinalHpRatio = 1f;
		SpellSummonMoveRatio = 1f;
		SpellSummonHPRecover = 0f;
		SpellSummonHPFixDropAmount = 0f;
		SpellSummonAttackSpeedRatio = 1f;
		SpellSummonAfterDeadSpawnWormCount = 0;
		SpellSummonInstantDeathHpRatio = 0f;
		SpellSummonDeathExplodeRange = 0f;
		SpellSummonDeathExplodeHpDamageRatio = 0f;
		extraReboundTime = 0f;
		reboundAddTime = 0f;
		spellVolumeRatio = 1f;
		enableAroundPlayer = true;
		enableFollowMouse = true;
		enableFollowTarget = true;
		enableNormalTransform = true;
		indirectShootByPlayer = false;
		criticalDragDamagePercent = 0f;
		criticalDragEffectRadiu = 0f;
		criticalDragApllyToCount = 0;
		criticalDragPullForce = 0f;
		virtualRealPosition = Vector3.zero;
		firstFrameDataInitialized = false;
		secondFrameDataInitialized = false;
		currentSpellMovement = SpellSpecialMovementType.Normal;
		aroundOwnerRadiuToSpeedRatio = 1f;
		spellBurnTime = 0f;
		burnHpRatioPerSeconds = 0f;
		endThunderHitRadiu = 0f;
		endThunderHitPercent = 0f;
		endTHunderHitChance = 0f;
		lifelineDamage = 0f;
		lifelineDamageInterval = float.PositiveInfinity;
		overalCriticalChance = 0f;
		controlByRemoteWand = false;
		lightningChainDamage = 0f;
		spellEndTeleport = false;
		isOwnerSpellEnd = false;
		isOwnerDead = false;
		CanChangeTeam = true;
		CanBeCapture = true;
		OwnerSpell = null;
		OwnerTsf = null;
		OwnerPoint = Vector3.zero;
		shooterWand = null;
		isThroughWall = false;
		damageRatio = 1f;
		speedRatio = 1f;
		radiusRatio = 1f;
		knockbackRatio = 1f;
		bonusSpeed = 0f;
		finalDamageRatio = 1f;
		finalSpeedRatio = 1f;
		finalRadiusRatio = 1f;
		finalKnockbackRatio = 1f;
		shooterWand = ((initialParameter.shooterWand != null) ? initialParameter.shooterWand : null);
		wandChargeData = null;
		InFallRebounding = false;
		InitialParameter = initialParameter;
		preSpellIDs = initialParameter.shootSpellPreSpells.Copy();
		spellCfg = initialParameter.spelldataConfig.Copy();
		InitialWithConfig = initialParameter.initializedSpellCfg?.Copy();
		ShootData = null;
		Direction = initialParameter.shootDirection;
		originalShootDirection = initialParameter.originShootDirection;
		damageRatio += initialParameter.extraDamageRatio;
		finalDamageRatio *= initialParameter.finalDamageRatio;
		radiusRatio += initialParameter.extraSizeRatio;
		finalRadiusRatio *= initialParameter.finalSizeRatio;
		knockbackRatio += initialParameter.extraKnockBackRatio;
		finalKnockbackRatio *= initialParameter.finalKnockBackRatio;
		bonusDuration += initialParameter.extraDuration;
		finalDurationRatio *= initialParameter.finalDurationRatio;
		speedRatio += initialParameter.extraSpeedRatio;
		finalSpeedRatio *= initialParameter.finalSpeedRatio;
		bonusSpeed += initialParameter.bounsSpeed;
		undifferDamageRatio *= initialParameter.undifferDamageRatio;
		spellVolumeRatio *= initialParameter.SpellVolumeRatio;
		lightningChainDamage = initialParameter.lightningChainDamage;
		lifelineDamage = initialParameter.lifeLineData.damage;
		lifelineDamageInterval = initialParameter.lifeLineData.Interval;
		SpellSummonAfterDeadSpawnWormCount += initialParameter.parasiteWormData.parasiteCount;
		SpellSummonHPFixDropAmount += initialParameter.parasiteWormData.summonHpDropPerSecond;
		SpellSummonInstantDeathHpRatio = initialParameter.selfSacrificeData.InstantDeathHpPercent;
		SpellSummonDeathExplodeRange = initialParameter.selfSacrificeData.ExplodeRange;
		SpellSummonDeathExplodeHpDamageRatio = initialParameter.selfSacrificeData.ExplodeHpDamageRatio;
		SpellSummonAttackSpeedRatio += initialParameter.summonExtraAttackSpeedRatio;
		SpellSummonMoveRatio += initialParameter.summonExtraMoveSpeedRatio;
		SpellSummonGainOwnerHpRatio += initialParameter.SpellSummonGainOwnerHpRatio;
		SpellSummonHPRatio += initialParameter.summonHpRatio.CurrentAddRatioStartZero;
		SpellSUmmonFinalHpRatio *= initialParameter.summonHpRatio.CurrentMulRatio;
		SpellSummonHPRecover += initialParameter.summonHpRecover;
		OwnerTsf = initialParameter.OwnerTsf;
		OwnerSpell = initialParameter.OwnerSpell;
		OwnerPoint = initialParameter.OnwerPoint;
		ownerPpt = initialParameter.ownerPpt;
		spellEndTeleport = initialParameter.spellEndTeleport;
		halfLifeTeleportCount = initialParameter.HalfLifeTeleportCount;
		FromEcho = false;
		if ((bool)rigid)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		refractedTargets.Clear();
		if (initialParameter.RefractionInfo.HasValue)
		{
			remainRefractCount = initialParameter.RefractionInfo.Value.count;
		}
		else
		{
			remainRefractCount = 0;
		}
		if (OwnerSpell != null)
		{
			indirectShootByPlayer = true;
			shouldCameraShock = false;
		}
		if ((bool)shooterWand && shooterWand.passiveRandomPosShoot)
		{
			indirectShootByPlayer = true;
			OwnerPoint = base.transform.position;
		}
		level1Cfg = SpellConfig.dic[spellCfg.id / 10 * 10 + 1];
		currentLevelPureCfg = SpellConfig.dic[spellCfg.id];
		_angle = spellCfg.angle;
		if (spellCfg.playShootSE)
		{
			PlayShootSound();
		}
		bool flag = IsSameCamp(UnitType.Player);
		if (initialParameter.shootFromSpellGroupV2)
		{
			ColorType = initialParameter.ColorType;
			ApplyShootGroupV2EnhanceEffect();
		}
		else
		{
			List<SpellColorType> list = new List<SpellColorType>();
			if (preSpellIDs != null)
			{
				if (preSpellIDs != null)
				{
					ApplyOldShootGroupEnhanceEffect();
				}
				using IEnumerator<int> enumerator = preSpellIDs.Select((int id) => id / 10).GetEnumerator();
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case 3014:
						if (!list.Contains(SpellColorType.Frozen))
						{
							list.Add(SpellColorType.Frozen);
						}
						break;
					case 3111:
						if (!list.Contains(SpellColorType.Fire))
						{
							list.Add(SpellColorType.Fire);
						}
						break;
					case 3005:
						if (!list.Contains(SpellColorType.Venom))
						{
							list.Add(SpellColorType.Venom);
						}
						break;
					case 3004:
						if (!list.Contains(SpellColorType.Mucus))
						{
							list.Add(SpellColorType.Mucus);
						}
						break;
					case 3129:
						if (!list.Contains(SpellColorType.Void))
						{
							list.Add(SpellColorType.Void);
						}
						break;
					}
				}
			}
			ColorType = ((list.Count > 0) ? list[UnityEngine.Random.Range(0, list.Count)] : SpellColorType.Player);
			if (!flag && ColorType == SpellColorType.Player)
			{
				ColorType = SpellColorType.Monster;
			}
		}
		InitialParameter.shootSpellPreSpells = preSpellIDs.Copy();
		if (flag && PlayerMgr.Inst.ItemCtrller.curse_IsReverseKnockback)
		{
			spellCfg.knockback = 0f - spellCfg.knockback;
		}
		if (OwnerSpell == null && ownerPpt != null && !ownerPpt.IsImmuneMucus)
		{
			finalSpeedRatio *= ownerPpt.affect_MucusSpellSpeedRatio;
		}
		float num = 0f;
		if (sc_Rebound != null)
		{
			num = ColliderRadius;
		}
		else if (bc_Rebound != null)
		{
			num = bc_Rebound.size.y;
		}
		if (base.transform.position.z > 0f - num - 0.05f)
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 0f - num - 0.05f);
		}
		_angle += initialParameter.extraScatter;
		_angle = Mathf.Clamp(_angle, 0f, 360f);
		wandShootAngle = _angle;
		if (initialParameter.zeroAngleShift)
		{
			_angle = 0f;
		}
		if (spellFollowTargetRotateSpeed > 0f)
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		if (rebounceTime > 0)
		{
			if (sc_Rebound != null)
			{
				sc_Rebound.enabled = true;
			}
			else if (bc_Rebound != null)
			{
				bc_Rebound.enabled = true;
			}
		}
		if (IsSameCamp(UnitType.Player) && (PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall || ((bool)shooterWand && shooterWand.passiveRandomPosShoot)))
		{
			isThroughWall = true;
			if (sc_Rebound != null)
			{
				sc_Rebound.enabled = false;
			}
			else if (bc_Rebound != null)
			{
				bc_Rebound.enabled = false;
			}
		}
		if (flag && PlayerMgr.Inst.ItemCtrller.relicCfg_SpellKnockback != null)
		{
			knockbackRatio += (float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellKnockback.int1.result / 100f;
		}
		if (!initialParameter.shootFromSpellGroupV2)
		{
			originalShootDirection = Direction;
			Direction = Tool2D.GetDir(Direction, UnityEngine.Random.Range((0f - _angle) / 2f, _angle / 2f));
		}
		radiusRatio = Mathf.Max(radiusRatio, 0.1f);
		if (initialParameter.shootFromSpellGroupV2)
		{
			foreach (int extraEnhanceId in initialParameter.extraEnhanceIds)
			{
				ApplyEnhanceSpell(extraEnhanceId);
			}
			if (SIP.finalMovementType == SpellSpecialMovementType.Rotation)
			{
				currentSpellMovement = SIP.finalMovementType;
			}
		}
		if (currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (SIP.RotationMovementInfo.HasValue)
			{
				spellAroundOwnerRadius = (SIP.RotationMovementInfo?.rotationRadiu).Value;
				bonusDuration += (SIP.RotationMovementInfo?.extraDuration).Value;
				bonusSpeed += (SIP.RotationMovementInfo?.extraSpeed).Value;
			}
			if (spellAroundOwnerRadius <= 0f)
			{
				spellAroundOwnerRadius = 3f;
			}
			spellAroundOwnerRadius *= radiusRatio * finalRadiusRatio;
			spellAroundOwnerRadius *= InitialParameter.aroundCasterRadiuRatio * aroundOwnerRadiuToSpeedRatio;
			spellAroundOwnerCurrentAngle = UnityEngine.Random.Range(0, 360);
		}
		spellCfg.damage = Mathf.Ceil(spellCfg.damage * damageRatio * finalDamageRatio) + initialParameter.finalDamageExtra;
		spellCfg.speed = (spellCfg.speed + bonusSpeed) * speedRatio * finalSpeedRatio;
		spellCfg.radius = spellCfg.radius * radiusRatio * finalRadiusRatio;
		spellCfg.knockback = spellCfg.knockback * knockbackRatio * finalKnockbackRatio;
		spellCfg.duration = (spellCfg.duration + bonusDuration) * finalDurationRatio;
		if (currentSpellMovement == SpellSpecialMovementType.Rotation && !SIP.tags.Contains(SpellTag.Twine))
		{
			spellCfg.speed *= SIP.aroundCasterRadiuRatio * aroundOwnerRadiuToSpeedRatio;
		}
		if (!(this is IHoldingSpell))
		{
			InitSpeedAndPosition();
		}
		overalCriticalChance = spellCfg.criticalChance / 100f + InitialParameter.extraCriticalChance;
		UpdateSizeByDamageAndVolumeRatio();
		wandChargeData = initialParameter.WandPostSlotChargeData;
		ShootData = shootData;
		if (shootData != null && shootData.SubGroup != null)
		{
			InitTriggerController(shootData.Triggers, shootData.SubGroup);
		}
		else
		{
			InitTriggerController(null, null);
		}
		if (this is IHoldingSpell holdingSpell && !holdingSpell.IsHolding)
		{
			holdingSpell.IsHolding = true;
			holdingSpell.HoldingTime = 0f;
			holdingSpell.StartHolding();
			if (holdingSpell.NeedSkipHolding(this))
			{
				holdingSpell.IsSkipHolding = true;
				_ = (bool)SIP.ChargeStar;
				holdingSpell.StopHolding();
			}
			else
			{
				holdingSpell.IsSkipHolding = false;
			}
		}
		Relic_Reaper relic_Reaper = PlayerMgr.Inst.ItemCtrller.relic_Reaper;
		if (relic_Reaper != null && IsSameCamp(UnitType.Player))
		{
			spellVenomOnceCount *= (int)relic_Reaper.RelicCfg.float1.result;
		}
		InitializeCallback();
		finishedInitialize = true;
		TryActionOnStartTriggerOnInitialize();
	}

	protected virtual void TryActionOnStartTriggerOnInitialize()
	{
		if (TriggerCtrl != null && TriggerCtrl.HasOnStartTrigger())
		{
			TriggerCtrl.TryTriggerOnStart();
		}
	}

	protected virtual void PlayShootSound()
	{
		PlaySE("Shoot");
	}

	protected virtual void InitSpeedAndPosition()
	{
		if (!SIP.spellIsFall)
		{
			InitSpeedAndPositionWithoutFall();
		}
		else
		{
			InitSpeedAndPositionWithFall();
		}
	}

	protected virtual void InitSpeedAndPositionWithoutFall()
	{
		CurrentSpeed = spellCfg.speed;
		CurrentUpSpeed = spellCfg.upSpeed;
	}

	protected virtual void InitSpeedAndPositionWithFall()
	{
		float num = spellCfg.speed * FallSpeedRatio;
		CurrentUpSpeed = 0f - num;
		CurrentSpeed = num / GameConst.spellFallAngleTan;
		spellCfg.speed = CurrentSpeed;
		spellCfg.upSpeed = CurrentUpSpeed;
		Height = FallInitialHeight;
		ShootSpellSpatialInfo finalShootSpatialInfo = SIP.finalShootSpatialInfo;
		if (finalShootSpatialInfo == null || !finalShootSpatialInfo.Target.HasValue)
		{
			Debug.LogWarning("坠落法术却没有攻击目标点的信息？");
			return;
		}
		Vector3? target = SIP.finalShootSpatialInfo.Target;
		Vector3 direction = SIP.finalShootSpatialInfo.Direction;
		Vector3 vector = target.Value + -direction.normalized * (Height / GameConst.spellFallAngleTan);
		Vector3 position = base.transform.position;
		if (spellCfg.isSplitSpell)
		{
			position.x = SIP.finalShootSpatialInfo.Start.x;
			position.y = SIP.finalShootSpatialInfo.Start.y;
			position.z = 0f;
		}
		else
		{
			position.x = vector.x;
			position.y = vector.y;
			position.z = 0f - Height;
		}
		base.transform.position = position;
		if (spellCfg.isSplitSpell)
		{
			Height = 0f;
			CurrentUpSpeed = FallingReboundForce;
			InFallRebounding = true;
		}
	}

	protected virtual void InitTriggerController(SlotData[] triggers, SpellShootGroup group)
	{
		if (_triggerCtrl == null)
		{
			_triggerCtrl = new SpellTriggerController(this);
		}
		if (triggers == null || triggers.Length == 0 || group == null || (object)shooterWand == null)
		{
			_triggerCtrl.Disable();
		}
		else
		{
			_triggerCtrl.InitAndEnable(triggers, group);
		}
	}

	public virtual void SingleInitialCallback()
	{
	}

	public virtual void InitializeCallback()
	{
	}

	public virtual void OnFirstFrame()
	{
	}

	public virtual void OnSecondFrame()
	{
	}

	public virtual void Update()
	{
		if (!finishedInitialize)
		{
			return;
		}
		if (!firstFrameDataInitialized)
		{
			firstFrameDataInitialized = true;
			OnFirstFrame();
		}
		else if (!secondFrameDataInitialized)
		{
			secondFrameDataInitialized = true;
			OnSecondFrame();
		}
		HalfLifeRandomTeleport();
		UpdateOwnerTsfData();
		UpdateOwnerSpellData();
		if (this is IHoldingSpell holdingSpell && holdingSpell.IsHolding)
		{
			if (!holdingSpell.HoldingCondition())
			{
				holdingSpell.StopHolding();
			}
			else
			{
				holdingSpell.HoldingUpdate();
			}
		}
		if (!isFlyFinish)
		{
			if (currentSpellMovement == SpellSpecialMovementType.Rotation && enableAroundPlayer)
			{
				SpellAroundPlayer();
			}
			else if (currentSpellMovement == SpellSpecialMovementType.ChaseMouse && enableFollowMouse)
			{
				SpellFollowMouse();
			}
			else if (currentSpellMovement == SpellSpecialMovementType.ChaseEnemy && enableFollowTarget)
			{
				SpellFollowTarget();
			}
			else if (currentSpellMovement == SpellSpecialMovementType.Normal && enableNormalTransform)
			{
				SpellNormalTransform();
			}
		}
	}

	protected virtual bool HalfLifeRandomTeleportRequirementCheck()
	{
		if (halfLifeTeleportCount > 0 && DurationTimer >= spellCfg.duration / 2f)
		{
			return !SIP.spellIsFall;
		}
		return false;
	}

	protected virtual bool HalfLifeRandomTeleport()
	{
		if (!HalfLifeRandomTeleportRequirementCheck())
		{
			return false;
		}
		halfLifeTeleportCount--;
		SpawnHalfLifeTeleportEffect(base.transform.position);
		base.transform.position += UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * UnityEngine.Random.Range(3f, 5f) * radiusRatio * finalRadiusRatio;
		base.transform.position = new Vector3(0f, 0f, base.transform.position.z) + Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
		OwnerPoint = base.transform.position;
		indirectShootByPlayer = true;
		SpawnHalfLifeTeleportEffect(base.transform.position);
		Direction = Tool2D.GetDir();
		spellCfg.duration += 2f;
		CurrentSpeed += 2f;
		if ((bool)rigid)
		{
			rigid.linearVelocity = Direction * CurrentSpeed;
		}
		return true;
	}

	protected virtual void SpawnHalfLifeTeleportEffect(Vector3 pos)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_HalfLifeTeleport", pos.IgnoreZ() + new Vector3(0f, 0.3f, -0.3f), 0.5f);
	}

	protected virtual void HeightFixedUpdate()
	{
		if (spellCfg.gravity != 0f)
		{
			CurrentUpSpeed += spellCfg.gravity * Time.deltaTime;
		}
		if (CurrentUpSpeed != 0f)
		{
			float height = Height;
			Height += CurrentUpSpeed * Time.deltaTime;
			if (Height < 0f)
			{
				Height = 0f;
			}
			if (Height <= 0f && height > 0f && SIP.spellIsFall)
			{
				OnFallingGround();
			}
		}
	}

	public override void FixedUpdate()
	{
		if (finishedInitialize)
		{
			if (SIP.spellIsFall)
			{
				UpdateUpSpeedWithFalling();
			}
			HeightFixedUpdate();
			TriggerCtrl?.TryTriggerOnHit(out var newSpells);
			if ((bool)rigid && currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				float moveDistance = rigid.linearVelocity.magnitude * Time.deltaTime;
				TriggerCtrl?.UpdateMoveTrigger(moveDistance, base.transform.position, Direction, out newSpells);
			}
			if (!isFlyFinish && currentSpellMovement == SpellSpecialMovementType.ChaseOwner && enableFollowTarget)
			{
				SpellFollowOwner();
			}
			base.FixedUpdate();
		}
	}

	public float GetLowFPSTimeScale(float scale = 7f)
	{
		return (!GeneralTool.IsLowFpsOptimizeActive(30f)) ? 1 : Mathf.CeilToInt(scale * (1f - GameMgr.Inst.GetFps() / 30f));
	}

	private void UpdateOwnerTsfData()
	{
		if (OwnerTsf != null && (OwnerTsf.gameObject.activeSelf || controlByRemoteWand))
		{
			OwnerPoint = OwnerTsf.position;
		}
		else
		{
			OwnerTsf = null;
		}
	}

	private void UpdateOwnerSpellData()
	{
		if (!isOwnerSpellEnd && OwnerSpell != null && !OwnerSpell.gameObject.activeInHierarchy)
		{
			isOwnerSpellEnd = true;
		}
	}

	protected bool isOwnerSpellValid()
	{
		if (OwnerSpell == null)
		{
			return false;
		}
		if (isOwnerSpellEnd)
		{
			return false;
		}
		return true;
	}

	protected bool isOwnerpptValid()
	{
		if (ownerPpt == null)
		{
			return false;
		}
		if (isOwnerDead)
		{
			return false;
		}
		return true;
	}

	public void ApplyVoidEffect(UnitProperty targetPpt)
	{
		if (voidExplosionInfo != null)
		{
			targetPpt.SetVoid(voidExplosionInfo);
		}
	}

	public void ApplyElementEffect(UnitProperty targetPpt)
	{
		if (spellMucusTime > 0f)
		{
			targetPpt.SetMucus(spellMucusTime, spellMucusMoveSpeedRatio, spellMucusSpellSpeedRatio);
		}
		if (spellVenomTime > 0f)
		{
			targetPpt.SetVenom(spellVenomTime, spellVenomOnceCount);
		}
		if (spellFrozenTime > 0f)
		{
			targetPpt.SetFrozen(spellFrozenTime);
		}
		if (spellBurnTime > 0f)
		{
			targetPpt.SetBurn(spellBurnTime, burnHpRatioPerSeconds);
		}
		if (voidExplosionInfo != null)
		{
			targetPpt.SetVoid(voidExplosionInfo);
		}
	}

	public virtual void SpellAroundPlayer()
	{
		float num = 360f / (MathF.PI * 2f * spellAroundOwnerRadius / CurrentSpeed) * Time.deltaTime;
		spellAroundOwnerCurrentAngle += num;
		Direction = Tool2D.GetDir(spellAroundOwnerCurrentAngle + 90f);
		Vector3 v = GetAroundTargetBasePoint() + Tool2D.GetDir(spellAroundOwnerCurrentAngle) * spellAroundOwnerRadius;
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		SpellAroundPlayerUpdateMoveTrigger(num);
	}

	public virtual void SpellAroundPlayerUpdateMoveTrigger(float angleOffset)
	{
		float moveDistance = spellAroundOwnerRadius * 2f * MathF.PI * angleOffset / 360f;
		TriggerCtrl?.UpdateMoveTrigger(moveDistance, base.transform.position, Direction, out var _);
	}

	public virtual Vector3 GetAroundTargetBasePoint()
	{
		if (SIP.tags.Contains(SpellTag.Twine))
		{
			if (OwnerSpell != null && !isOwnerSpellEnd)
			{
				lastAroundTargetPosition = ((OwnerSpell.spellCfg.abilityType == SpellAbilityType.HighPressureWasher) ? ((Spell1019HighPressureWasherRemaster)OwnerSpell).FirstWaterPosition : OwnerSpell.transform.position);
			}
			return lastAroundTargetPosition;
		}
		if (OwnerTsf != null)
		{
			return OwnerTsf.transform.position;
		}
		if (!spellCfg.isSplitSpell && ownerPpt != null && OwnerSpell == null && ownerPpt.unitCfg.unitType == UnitType.Player && !indirectShootByPlayer)
		{
			if ((bool)shooterWand && shooterWand.WandCfg != null && (shooterWand.WandCfg.specialAbility == WandAbility.LongWand || shooterWand.WandCfg.specialAbility == WandAbility.LongWandAndSpellBreaker || PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null))
			{
				return shooterWand.GetShootPosition();
			}
			return GetPlayerBaseShootPoint();
		}
		if (OwnerPoint != Vector3.zero)
		{
			return OwnerPoint;
		}
		if (ownerPpt == null)
		{
			return base.transform.position;
		}
		if (ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			return PlayerMgr.Inst.PlayerCtrller.tsf_WandPoint.position;
		}
		return ownerPpt.transform.position;
	}

	public virtual Vector3 GetPlayerBaseShootPoint()
	{
		return PlayerMgr.Inst.PlayerPoint;
	}

	private void ApplyOldShootGroupEnhanceEffect()
	{
		for (int i = 0; i <= preSpellIDs.Count - 1; i++)
		{
			SpellConfig spellConfig = SpellConfig.dic[preSpellIDs[i]];
			ApplyEnhanceSpell(spellConfig.id);
		}
	}

	private void ApplyShootGroupV2EnhanceEffect()
	{
		List<SpellSpecialMovementType> list = new List<SpellSpecialMovementType>();
		float num = 1f;
		for (int i = 0; i <= preSpellIDs.Count - 1; i++)
		{
			SpellConfig spellConfig = SpellConfig.dic[preSpellIDs[i]];
			switch (spellConfig.abilityType)
			{
			case SpellAbilityType.DeathInfect:
				if (voidExplosionInfo == null)
				{
					voidExplosionInfo = new Spell3129VoidExplosion.VoidExplosionData();
				}
				voidExplosionInfo.ExplosionRange = spellConfig.float1 * radiusRatio * finalRadiusRatio;
				voidExplosionInfo.HpToDmgRatio += spellConfig.float2 / 100f;
				voidExplosionInfo.InstantKillRatio = Mathf.Max(spellConfig.float3 / 100f, voidExplosionInfo.InstantKillRatio);
				break;
			case SpellAbilityType.MucusCrystal:
				spellMucusTime = Mathf.Max(spellConfig.float1);
				spellMucusMoveSpeedRatio *= spellConfig.float2 / 100f;
				spellMucusSpellSpeedRatio *= spellConfig.float3 / 100f;
				break;
			case SpellAbilityType.VenomCrystal:
				spellVenomTime = Mathf.Max(spellConfig.float1, spellVenomTime);
				spellVenomOnceCount += spellConfig.int2;
				break;
			case SpellAbilityType.Penetrate:
				penetrateTime += spellConfig.int1;
				break;
			case SpellAbilityType.SpellHover:
				SpellHoverTime += spellConfig.float1;
				break;
			case SpellAbilityType.AroundOwner:
				currentSpellMovement = SpellSpecialMovementType.Rotation;
				spellAroundOwnerRadius = spellConfig.float3;
				break;
			case SpellAbilityType.AroundMouse:
				currentSpellMovement = SpellSpecialMovementType.ChaseMouse;
				spellFollowMouseLerp = spellConfig.float3;
				break;
			case SpellAbilityType.RandomRotationRadiu:
				spellAroundOwnerRadius = 3f;
				num = UnityEngine.Random.Range(spellConfig.float1, spellConfig.float2);
				currentSpellMovement = SpellSpecialMovementType.Rotation;
				aroundOwnerRadiuToSpeedRatio *= num;
				break;
			case SpellAbilityType.FollowTarget:
				currentSpellMovement = SpellSpecialMovementType.ChaseEnemy;
				spellFollowTargetRotateSpeed += spellConfig.float1;
				break;
			case SpellAbilityType.FollowOwner:
				currentSpellMovement = SpellSpecialMovementType.ChaseOwner;
				spellFollowTargetRotateSpeed += spellConfig.float1;
				break;
			case SpellAbilityType.Rebound:
				rebounceTime += spellConfig.int1;
				extraReboundTime += spellConfig.int1;
				reboundAddTime = Mathf.Max(reboundAddTime, spellConfig.float1);
				break;
			case SpellAbilityType.SpellSplit:
				spellSplitCount += spellConfig.int1;
				break;
			case SpellAbilityType.Frozen:
				spellFrozenTime += spellConfig.float1;
				break;
			case SpellAbilityType.ThunderCrystal:
				endThunderHitRadiu = Mathf.Max(endThunderHitRadiu, spellConfig.float1);
				endThunderHitPercent += spellConfig.float2 / 100f;
				endTHunderHitChance = Mathf.Max(endTHunderHitChance, spellConfig.float3 / 100f);
				break;
			case SpellAbilityType.PullForceCrystal:
				criticalDragDamagePercent = Mathf.Max((float)spellConfig.int1 / 100f, criticalDragDamagePercent);
				criticalDragApllyToCount += spellConfig.int2;
				criticalDragPullForce = Mathf.Max(spellConfig.float2, criticalDragPullForce);
				criticalDragEffectRadiu = Mathf.Max(spellConfig.float1, criticalDragEffectRadiu);
				break;
			case SpellAbilityType.FireCrystal:
				spellBurnTime = Mathf.Max(spellBurnTime, spellConfig.float2);
				burnHpRatioPerSeconds += spellConfig.int1;
				break;
			}
		}
		if (list.Count > 0)
		{
			currentSpellMovement = GeneralTool.ListShuffle(list)[0];
		}
		spellAroundOwnerRadius *= num;
	}

	protected virtual void SpellFollowMouse()
	{
		Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(mousePoint) * CurrentSpeed, CurrentSpeed * Time.deltaTime * spellFollowMouseLerp);
		rigid.linearVelocity = linearVelocity;
		Direction = linearVelocity.normalized;
	}

	protected virtual void SpellFollowTarget()
	{
		if (SpellFollowHaveTarget)
		{
			Direction = Tool2D.DirMoveTowards(Direction, ToPointDir(spellFollowTargetPpt.transform), CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
			rigid.linearVelocity = Direction * CurrentSpeed;
		}
		else
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
	}

	protected virtual void SpellFollowOwner()
	{
		float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(Direction, GetSpellFollowToOwnerDirection())) - 90f) / 90f;
		Direction = Tool2D.DirMoveTowardsTargetInCounterClockWise(Direction, GetSpellFollowToOwnerDirection(), CurrentSpeed * spellFollowTargetRotateSpeed * Time.fixedDeltaTime);
		float num = 0.4f;
		rigid.linearVelocity = Direction * CurrentSpeed * Mathf.Lerp(1f - num, 1f + num, t);
	}

	protected virtual Vector3? GetSpellFollowToOwnerPoint()
	{
		if (OwnerTsf != null && OwnerTsf.position != Vector3.zero)
		{
			return OwnerTsf.position;
		}
		if (OwnerSpell == null && ownerPpt != null)
		{
			return ownerPpt.transform.position;
		}
		if (OwnerPoint != Vector3.zero)
		{
			return OwnerPoint;
		}
		return null;
	}

	protected virtual Vector3 GetSpellFollowToOwnerDirection()
	{
		Vector3? spellFollowToOwnerPoint = GetSpellFollowToOwnerPoint();
		if (spellFollowToOwnerPoint.HasValue)
		{
			return ToPointDir(spellFollowToOwnerPoint.Value);
		}
		return Direction;
	}

	protected virtual float GetEqualScatterMultipleShootInitialDirectionAngleShift()
	{
		if (SIP.multiShootCount <= 1)
		{
			return 0f;
		}
		return (0f - wandShootAngle) / 2f + wandShootAngle / (float)(SIP.multiShootCount - 1) * (float)SIP.inMultiShootIndex;
	}

	protected virtual Vector3 GetEqualScatterMultipleShootInitialDirection(Vector3 dir)
	{
		return Tool2D.GetDir(dir, GetEqualScatterMultipleShootInitialDirectionAngleShift());
	}

	protected virtual void SpellNormalTransform()
	{
	}

	public virtual void ApplySpeedToVelocity()
	{
		if (currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = Direction * CurrentSpeed;
		}
	}

	public float GetCriticalChance()
	{
		return overalCriticalChance;
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		if (!collision.gameObject.CompareTag("Wall"))
		{
			return;
		}
		rebounceTime--;
		if (createReboundEffect && currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_TransparentRebound", Tool2D.GetLayerPoint(base.transform), 1f).transform.localScale = Vector3.one * base.transform.localScale.x * ColliderRadius * 0.8f;
		}
		if (extraReboundTime > 0f)
		{
			extraReboundTime--;
			spellCfg.duration += reboundAddTime;
		}
		if (rebounceTime <= 0)
		{
			if (sc_Rebound != null)
			{
				sc_Rebound.enabled = false;
			}
			else if (bc_Rebound != null)
			{
				bc_Rebound.enabled = false;
			}
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			Direction = rigid.linearVelocity.normalized;
		}
	}

	public virtual void TriggerIn(Collider other)
	{
		if (fallSpellIgnoreTriggerIn)
		{
			return;
		}
		if (other.gameObject.CompareAnyTag("Player", "Teammate", "Monster"))
		{
			OnHitUnit(other.GetComponent<UnitProperty>());
		}
		else if (other.gameObject.CompareAnyTag("Wall", "SolidObj"))
		{
			OnHitWallAndSolidObj(other);
		}
		else if (other.CompareTag("Destructible"))
		{
			OnHitDestructible(other.GetComponent<UnitProperty>());
		}
		else if (other.CompareTag("Brittleness"))
		{
			OnHitBrittleness(other.gameObject);
		}
		else if (other.gameObject.CompareAnyTag("RollBall", "Butterfly", "Spell"))
		{
			SpellBase componentInParent = other.GetComponentInParent<SpellBase>();
			if ((bool)componentInParent)
			{
				OnHitSpell(componentInParent);
			}
		}
		else
		{
			OnHitUnknownTagObject(other);
		}
	}

	public virtual TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		switch (targetGO.tag)
		{
		case "Player":
		case "Teammate":
		case "Monster":
		case "Destructible":
		case "SolidObj":
		case "Brittleness":
			return OutputDamage(targetGO.GetComponent<UnitProperty>(), info, damageRecordeType);
		default:
			return info;
		}
	}

	protected virtual TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
		if (SIP.spellIsFall)
		{
			takeDamageInfo.canRebound = false;
		}
		return takeDamageInfo;
	}

	public virtual TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		switch (unitPpt.gameObject.tag)
		{
		case "Player":
		case "Teammate":
		case "Monster":
		case "Destructible":
		case "SolidObj":
		case "Brittleness":
		{
			if (info == null)
			{
				info = CreateDefaultTakeDamageInfo(unitPpt);
			}
			info.wandChargeData = wandChargeData;
			string text = unitPpt.gameObject.tag;
			if (text == "Monster" || text == "Destructible")
			{
				TriggerCtrl?.AddHitTriggerPoint(unitPpt.transform.position);
			}
			if (voidExplosionInfo != null)
			{
				unitPpt.SetVoid(voidExplosionInfo);
			}
			TakeDamageInfo takeDamageInfo = unitPpt.TakeDamage(this, info);
			if (!info.immuneDamage)
			{
				ApplyElementEffect(unitPpt);
			}
			lastOutputDamageFrame = Time.frameCount;
			CheckIfPullCrystalIsValidToAttack(takeDamageInfo, unitPpt);
			if (takeDamageInfo.isPlayHitSE && unitPpt.gameObject.tag != "Brittleness")
			{
				PlaySE("Hit");
			}
			return takeDamageInfo;
		}
		default:
			return info;
		}
	}

	public void CheckIfPullCrystalIsValidToAttack(TakeDamageInfo _info, UnitProperty unitPpt)
	{
		if (_info.isCriticalDamage && criticalDragDamagePercent > 0f && criticalDragEffectRadiu > 0f && (_info.beHitPpt.CompareTag("Player") || _info.beHitPpt.CompareTag("Monster") || _info.beHitPpt.CompareTag("Teammate")))
		{
			ActivePullCrystal(unitPpt);
		}
	}

	protected virtual List<UnitProperty> GetPullCrystalTargets(UnitProperty ignoreTarget)
	{
		float radius = criticalDragEffectRadiu * radiusRatio * finalRadiusRatio;
		Vector3 center = ((virtualRealPosition == Vector3.zero) ? base.transform.position : virtualRealPosition);
		int num = criticalDragApllyToCount;
		List<UnitProperty> list = (from e in LevelMgr.Inst.CurrentRoomCtrller.GetTargetableInCircle(center, radius)
			where e != ignoreTarget && e.unitCfg.id != 101721
			select e).ToList();
		while (list.Count > num)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		return list;
	}

	protected virtual void UpdateSizeByDamageAndVolumeRatio()
	{
		if (SpellConfig.dic[spellCfg.id].damage != 0f)
		{
			float num = spellCfg.damage / level1Cfg.damage;
			float num2 = Mathf.Pow(num, 0.3333f);
			if (Math.Abs(num - 1f) >= 0.01f)
			{
				base.transform.localScale = Vector3.one * num2 * spellVolumeRatio;
			}
			else
			{
				base.transform.localScale = Vector3.one * spellVolumeRatio;
			}
		}
	}

	protected void ActivePullCrystal(UnitProperty targetPpt)
	{
		if (IsSameCamp(UnitType.Player).LogIfNot("只有玩家阵营才能触发引力效果"))
		{
			return;
		}
		List<UnitProperty> pullCrystalTargets = GetPullCrystalTargets(targetPpt);
		float num = (spellCfg.isDPS ? spellCfg.DPSDamageInterval : 1f);
		float damage = Mathf.Ceil(criticalDragDamagePercent * spellCfg.damage * num);
		if (pullCrystalTargets.Count > 0)
		{
			SEMgr.Inst.spell3121Energy.PlaySE().pitch = UnityEngine.Random.Range(0.5f, 1.5f);
		}
		foreach (UnitProperty item in pullCrystalTargets)
		{
			TakeDamageInfo info = new TakeDamageInfo
			{
				canRebound = false,
				damage = damage,
				attackerPpt = ownerPpt
			};
			ApplyVoidEffect(item);
			Spell3101PullCrystal component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, base.transform.position, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
			if (component.tsf_Layer != null)
			{
				component.tsf_Layer.localScale = base.transform.localScale;
			}
			component.SetColor(ColorType);
			component.SetChainTargetTransform(targetPpt.gameObject.transform, item.transform);
			float num2 = criticalDragPullForce * knockbackRatio * finalKnockbackRatio;
			Vector3 normalized = (targetPpt.transform.position - item.transform.position).normalized;
			item.TakeKnockback(num2 * normalized);
			item.TakeDamage(this, info);
			ApplyElementEffect(item);
			component.CreateHitEffect(ColorType, item.transform.position);
		}
	}

	public void EndThunderAttackCheck(bool thunderOnly = true, Vector3 hitPos = default(Vector3), int specifyDamage = 0)
	{
		if ((ColorType != SpellColorType.Thunder && thunderOnly) || endThunderHitPercent <= 0f)
		{
			return;
		}
		float radius = endThunderHitRadiu * radiusRatio * finalRadiusRatio;
		Vector3 vector = ((virtualRealPosition == Vector3.zero) ? base.transform.position : virtualRealPosition);
		if (hitPos != default(Vector3))
		{
			vector = hitPos;
		}
		float num = ((specifyDamage > 0) ? ((float)specifyDamage) : spellCfg.damage);
		float damage = Mathf.Ceil(endThunderHitPercent * num);
		LayerCorrect component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31011, vector, quaternion.identity, 1f).GetComponent<LayerCorrect>();
		component.tsf_Layer.localScale = Vector3.one * endThunderHitRadiu / SpellConfig.dic[31011].float1 * (radiusRatio * finalRadiusRatio);
		SEMgr.Inst.spell3101Hit.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(vector, radius, thunderColliderBuffer, thunderColliderTags);
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			UnitProperty component2 = thunderColliderBuffer[i].gameObject.GetComponent<UnitProperty>();
			if (!(thunderColliderBuffer[i] != null))
			{
				continue;
			}
			if (thunderColliderBuffer[i].CompareTag("RollBall") || thunderColliderBuffer[i].CompareTag("Butterfly"))
			{
				SpellBase componentInParent = thunderColliderBuffer[i].GetComponentInParent<SpellBase>();
				if (!componentInParent.IsSameCamp(this))
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(damage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
					else
					{
						MonoBehaviour.print(componentInParent.spellCfg.abilityType);
					}
				}
			}
			else if (thunderColliderBuffer[i] != null && thunderColliderBuffer[i].gameObject.activeInHierarchy && component2 != null)
			{
				TakeDamageInfo info = new TakeDamageInfo
				{
					canRebound = false,
					damage = damage
				};
				string path = string.Format("{0}{1}/{2}_Hit", "Prefabs/Spell/", 31011, 31011);
				ObjPoolMgr.Inst.GetGO(path, component2.transform.position + new Vector3(0f, 0.3f, 0f), quaternion.identity, 1f).GetComponent<LayerCorrect>().tsf_Layer.transform.right = (component2.transform.position - component.transform.position).normalized;
				component2.TakeDamage(this, info);
			}
			else if (thunderColliderBuffer[i].CompareTag("Brittleness"))
			{
				OutputDamage(thunderColliderBuffer[i].gameObject, new TakeDamageInfo
				{
					canRebound = false,
					damage = damage
				}, SpellAbilityType.ThunderCrystal);
			}
		}
	}

	public virtual void HitEFAndRecycle()
	{
		CreateHitEffect();
		PlaySE("Hit");
		PoolRecycle();
	}

	public virtual void PoolRecycle()
	{
		OnWillRecycleIgnoreRecycle.Invoke(this);
		EndThunderAttackCheck();
		OnNeedTriggerOnOver();
		PlayerSpellUnRegisterKeepCastingBuff();
		TrySpellTeleport();
		TrySplit();
		OwnerTsf = null;
		OwnerPoint = Vector3.zero;
		TriggerCtrl?.TryTriggerOnHit(out var _);
		if (!CreateFromMiniPool)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else
		{
			PlayerMgr.Inst.MiniPool.RecycleGO(base.gameObject);
		}
	}

	private void TrySpellTeleport()
	{
		if (spellEndTeleport && !spellCfg.isSplitSpell)
		{
			Spell3116EndTeleportSystem.Teleport(ownerPpt, (virtualRealPosition == Vector3.zero) ? base.transform.position : virtualRealPosition);
		}
	}

	protected virtual Vector3 GetSplitSpellPosition(Vector3 splitDirection)
	{
		if (SIP.spellIsFall)
		{
			return base.transform.position;
		}
		float num = ((spellCfg.radius != 0f) ? spellCfg.radius : (0.5f * Mathf.Min(1f, radiusRatio * finalRadiusRatio)));
		return base.transform.position + splitDirection * num;
	}

	protected virtual SpellConfig CreateSplitSpellConfig()
	{
		SpellConfig obj = ((InitialWithConfig == null) ? SpellConfig.GetConfigCopy(spellCfg.id) : InitialWithConfig.Copy());
		obj.isSplitSpell = true;
		return obj;
	}

	protected virtual SpellInitialParameter CreateSplitSpellInitialParameter(SpellConfig config, Vector3 shootDirection)
	{
		SpellInitialParameter spellInitialParameter = InitialParameter.Copy();
		spellInitialParameter.spelldataConfig = config;
		spellInitialParameter.finalDamageRatio *= 0.35f;
		spellInitialParameter.finalKnockBackRatio *= 0.5f;
		spellInitialParameter.finalSizeRatio *= 0.8f;
		if (!SIP.spellIsFall)
		{
			spellInitialParameter.finalSpeedRatio *= 0.5f;
		}
		spellInitialParameter.lightningChainDamage = Mathf.Ceil(spellInitialParameter.lightningChainDamage * 0.5f);
		spellInitialParameter.shootDirection = shootDirection;
		spellInitialParameter.originShootDirection = shootDirection;
		spellInitialParameter.ShootCause = new ShootCause.BySplit(this);
		if (SIP.finalShootSpatialInfo != null)
		{
			spellInitialParameter.finalShootSpatialInfo = (SIP.finalShootSpatialInfo.Target.HasValue ? ShootSpellSpatialInfo.ToPoint(base.transform.position, base.transform.position + shootDirection) : ShootSpellSpatialInfo.ByDirection(base.transform.position, spellInitialParameter.originShootDirection));
			spellInitialParameter.originShootSpatialInfo = spellInitialParameter.finalShootSpatialInfo;
		}
		return spellInitialParameter;
	}

	protected virtual SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		Vector3 normalized = Tool2D.GetDir(baseAngle + 360f / (float)spellSplitCount * (float)index).normalized;
		Vector3 splitSpellPosition = GetSplitSpellPosition(normalized);
		SpellInitialParameter spellInitialParameter = CreateSplitSpellInitialParameter(splitCfg, normalized);
		if (GeneralTool.IsLowFpsOptimizeActive(40f))
		{
			int num = ((!(GameMgr.Inst.GetFps() >= 10f)) ? 1 : Mathf.Max(1, Mathf.FloorToInt((float)spellSplitCount * GameMgr.Inst.GetFps() / 40f)));
			float num2 = (float)spellSplitCount / (float)num;
			spellInitialParameter.finalDamageRatio *= num2;
			spellInitialParameter.splitFinalDamageRatio *= num2;
		}
		SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellCfg.prefab, splitSpellPosition).GetComponent<SpellBase>();
		spellInitialParameter.SplitIndex = index;
		if (ShootData != null)
		{
			SpellShootData spellShootData = ShootData.Copy(ShootData.OwnerGroup);
			spellShootData.SubGroup = null;
			component.InitializeV2(spellInitialParameter, spellShootData);
		}
		else
		{
			component.Initialize(spellInitialParameter);
		}
		component.shooterWand = shooterWand;
		component.wandChargeData = wandChargeData;
		component.spellSplitCount = 0;
		component.spellCfg.isSplitSpell = true;
		component.shouldCameraShock = false;
		if (currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			component.OwnerTsf = null;
			component.OwnerPoint = splitSpellPosition + normalized * 1.5f;
			component.spellAroundOwnerCurrentAngle = baseAngle + 360f / (float)spellSplitCount * (float)index;
		}
		return component;
	}

	protected virtual IEnumerable<SpellBase> TrySplit()
	{
		if (spellSplitCount == 0 || spellCfg.isSplitSpell)
		{
			return Array.Empty<SpellBase>();
		}
		SpellConfig splitCfg = CreateSplitSpellConfig();
		int num = UnityEngine.Random.Range(0, 360);
		List<SpellBase> list = new List<SpellBase>();
		int num2 = (GeneralTool.IsLowFpsOptimizeActive(40f) ? Mathf.FloorToInt((float)spellSplitCount * GameMgr.Inst.GetFps() / 40f) : spellSplitCount);
		if (GeneralTool.IsLowFpsOptimizeActive(10f))
		{
			num2 = 1;
		}
		for (int i = 0; i < num2; i++)
		{
			list.Add(CreateSplitSpell(splitCfg, num, i));
		}
		if (SIP.tags.Contains(SpellTag.Twine))
		{
			foreach (SpellBase item in list)
			{
				item.lastAroundTargetPosition = base.transform.position;
			}
		}
		CreateSplitSpellLightningChain(list);
		return list;
	}

	protected virtual void CreateSplitSpellLightningChain(List<SpellBase> spells)
	{
		Wand wand = PlayerMgr.Inst.Wands.Where((Wand e) => (object)e != null && e.WandCfg != null).FirstOrDefault((Wand e) => e.WandCfg == SIP.shooterWandCfg);
		if (lightningChainDamage > 0f && (object)wand != null)
		{
			Spell3007ChainSystem.CreateChains(spells.ToArray(), wand);
		}
	}

	public void ChangeCurrentSpeed(float targetValue)
	{
		CurrentSpeed = targetValue;
		rigid.linearVelocity = rigid.linearVelocity.normalized * targetValue;
	}

	public void ChangeGravity(float targetValue)
	{
		spellCfg.gravity = targetValue;
	}

	public void ChangeVelocityZ(float targetValue)
	{
		CurrentUpSpeed = targetValue;
	}

	protected virtual void OnFallingGround()
	{
		CreateHitEffect();
		EffectBase.PlayFallingGroundSound();
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}

	public EffectController GetEffectController(string strSuffix, Vector3 point, float duration = 0f)
	{
		return GetEffectController(strSuffix, point, Quaternion.identity, duration);
	}

	public EffectController GetEffectController(string strSuffix, Vector3 point, Quaternion rotation, float duration = 0f)
	{
		EffectController component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + effectPrefix + "/" + effectPrefix + strSuffix, point, rotation).GetComponent<EffectController>();
		if (component.tsf_Layer != null)
		{
			component.tsf_Layer.localScale = base.transform.localScale;
		}
		component.ECStartEffect();
		component.ECChangeColor(ColorType);
		if (duration != 0f)
		{
			component.ECRecycle(duration);
		}
		return component;
	}

	public GameObject GetEffect(string strSuffix, Vector3 point, float duration = 0f)
	{
		return GetEffect(strSuffix, point, Quaternion.identity, duration);
	}

	public GameObject GetEffect(string strSuffix, Vector3 point, Quaternion rotation, float duration = 0f)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + effectPrefix + "/" + effectPrefix + "_" + strSuffix, point, rotation, Vector3.one * base.transform.localScale.x, duration);
	}

	public Vector3 ToPointDir(Vector3 point)
	{
		return Tool2D.IgnoreZV2ToV1Normal(point, base.transform.position);
	}

	public Vector3 ToPointDir(Transform targetT)
	{
		return ToPointDir(Tool2D.IgnoreZPoint(targetT));
	}

	public UnitProperty GetNearestTargetablePpt(Vector3 startPosition, bool checkWall = false)
	{
		switch (ownerPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			return LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(startPosition, checkWall);
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			return PlayerMgr.Inst.GetNearestPpt(startPosition, checkWall);
		default:
			Debug.LogError(ownerPpt.unitCfg.unitType);
			return null;
		}
	}

	public virtual UnitProperty GetMiniMalAngleTargetablePpt(bool checkWall = false)
	{
		switch (ownerPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			return LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetablePpt(base.transform.position, Direction, checkWall);
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			return PlayerMgr.Inst.GetMinimalAngleTargetablePpt(base.transform.position, Direction, checkWall);
		default:
			Debug.LogError(ownerPpt.unitCfg.unitType);
			return null;
		}
	}

	protected virtual float GetRefractionRadius()
	{
		(int, float)? refractionInfo = SIP.RefractionInfo;
		if (!refractionInfo.HasValue)
		{
			return 0f;
		}
		return SIP.RefractionInfo.Value.radius * radiusRatio;
	}

	[CanBeNull]
	protected virtual UnitProperty GetNextRefractionTargetablePpt()
	{
		(int, float)? refractionInfo = SIP.RefractionInfo;
		if (!refractionInfo.HasValue)
		{
			return null;
		}
		UnitType unitType = ownerPpt.unitCfg.unitType;
		if (unitType != 0 && unitType != UnitType.Teammate && unitType != UnitType.TeammateNotAttack)
		{
			return null;
		}
		UnitProperty nearestTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(checkNextRefractionPosition, GetRefractionRadius(), refractedTargets.ToArray());
		if ((bool)nearestTargetablePpt)
		{
			return nearestTargetablePpt;
		}
		if (refractedTargets.Count == 0)
		{
			return null;
		}
		List<GameObject> list = refractedTargets;
		GameObject item = list[list.Count - 1];
		refractedTargets.Clear();
		refractedTargets.Add(item);
		return LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(checkNextRefractionPosition, GetRefractionRadius(), refractedTargets.ToArray());
	}

	public void CreateCircleElementGroundEffect(float SpellEffectRadiu = 0f, Vector3 spawnPos = default(Vector3))
	{
		float radius = ((SpellEffectRadiu == 0f) ? spellCfg.radius : SpellEffectRadiu);
		Vector3 v = ((spawnPos == default(Vector3)) ? base.transform.position : spawnPos);
		if (ColorType == SpellColorType.Mucus)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(v), radius);
		}
		if (ColorType == SpellColorType.Venom)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(v), radius, spellVenomTime);
		}
		if (ColorType == SpellColorType.Frozen)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(v), radius);
		}
	}

	public bool IsSameCamp(SpellBase spell)
	{
		return IsSameCamp(spell.ownerPpt.unitCfg.unitType);
	}

	public bool IsSameCamp(UnitType unitType)
	{
		switch (ownerPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			if (unitType != 0 && unitType != UnitType.Teammate)
			{
				return unitType == UnitType.TeammateNotAttack;
			}
			return true;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
			if (unitType != UnitType.Monster && unitType != UnitType.Elite && unitType != UnitType.Boss)
			{
				return unitType == UnitType.WillAttack;
			}
			return true;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (unitType == UnitType.NotAttack || unitType == UnitType.Brittleness)
			{
				return true;
			}
			return false;
		default:
			Debug.LogError($"判断不了的单位类型 {unitType}");
			return true;
		}
	}

	public bool IsSameCamp(UnitProperty unit)
	{
		return IsSameCamp(unit.unitCfg.unitType);
	}

	protected void PlayerSpellRegisterKeepCastingBuff(bool decreaseMoveSpeed = true)
	{
		if (ownerPpt.unitCfg.unitType == UnitType.Player && !spellCfg.isSplitSpell && !spellKeepCastBuffApplied && !indirectShootByPlayer)
		{
			spellKeepCastBuffApplied = true;
		}
	}

	protected void PlayerSpellUnRegisterKeepCastingBuff()
	{
		_ = spellKeepCastBuffApplied;
	}

	public virtual AudioSource PlaySE(string suffix, float SEInterval = 0.05f)
	{
		SEMgr inst = SEMgr.Inst;
		int abilityType = (int)spellCfg.abilityType;
		return inst.PlaySE("SE_Spell" + abilityType + suffix, SEPlayMode.Replay, 3, SEInterval);
	}

	public AudioSource PlayLoopSE(string suffix, float time)
	{
		SEMgr inst = SEMgr.Inst;
		int abilityType = (int)spellCfg.abilityType;
		return inst.PlayLoopSE("SE_Spell" + abilityType + suffix, time);
	}

	public float IsReverseDirection()
	{
		return (!InitialParameter.reverseDirection) ? 1 : (-1);
	}

	public SummonUnitPropertyValueFix GetSummonValueRatio()
	{
		return new SummonUnitPropertyValueFix(this);
	}

	public virtual void ChangeTeamToPlayer()
	{
		ownerPpt = PlayerMgr.Inst.PlayerPpt;
		ColorType = SpellColorType.Player;
		Collider component = GetComponentInChildren<TriggerIn>().GetComponent<Collider>();
		if ((bool)component)
		{
			component.enabled = false;
			component.enabled = true;
		}
		EffectBase.FlushColor();
		SpellConfig spellConfig = SpellConfig.dic[spellCfg.id];
		if (spellCfg.duration < spellConfig.duration)
		{
			spellCfg.duration = spellConfig.duration;
		}
		DurationTimer = 0f;
	}

	protected virtual float GetLowFpsSpellSplitCount(float countPower = 1f, float lowFPsThreshold = 40f)
	{
		return (!(GameMgr.Inst.GetFps() >= 10f)) ? 1 : Mathf.Max(1, Mathf.FloorToInt((float)spellSplitCount * Mathf.Pow(GameMgr.Inst.GetFps() / lowFPsThreshold, countPower)));
	}

	public virtual void ChangeTeamToMonster(UnitProperty monsterPpt)
	{
		ownerPpt = monsterPpt;
		ColorType = SpellColorType.Monster;
		Collider component = GetComponentInChildren<TriggerIn>().GetComponent<Collider>();
		if ((bool)component)
		{
			component.enabled = false;
			component.enabled = true;
		}
		EffectBase.FlushColor();
	}

	public virtual void OnNeedTriggerOnOver()
	{
		if (TriggerCtrl != null)
		{
			TriggerCtrl.TryTriggerOnOver(base.transform.position, Direction, out var _);
		}
	}

	public virtual void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (EffectBase.SpriteEffects.Any((SpellSpriteEffectSettings e) => e.Name == "Hit"))
		{
			EffectBase.CreateSpriteEffect("Hit", position, rotation);
		}
		else if (EffectBase.Effects.Any((SpellEffectSettings e) => e.Name == "Hit"))
		{
			EffectBase.ManualCreateEffect("Hit");
		}
	}

	public void CreateHitEffectLookAt(Vector3 lookAt, Vector3? position = null)
	{
		CreateHitEffect(position, Quaternion.LookRotation(lookAt) * Quaternion.Euler(0f, -90f, 0f));
	}

	protected virtual string GetRelationResourcePath(string postfixs)
	{
		return string.Format("{0}{1}/{2}_{3}", "Prefabs/Spell/", effectPrefix, effectPrefix, postfixs);
	}

	public virtual void ApplyEnhanceSpell(int spellId)
	{
		if (!SpellConfig.dic.ContainsKey(spellId))
		{
			Debug.LogError($"不存在的法术ID {spellCfg.id}");
			return;
		}
		SpellConfig spellConfig = SpellConfig.dic[spellId];
		switch (spellConfig.abilityType)
		{
		case SpellAbilityType.DeathInfect:
			if (voidExplosionInfo == null)
			{
				voidExplosionInfo = new Spell3129VoidExplosion.VoidExplosionData();
			}
			voidExplosionInfo.ExplosionRange = spellConfig.float1 * radiusRatio * finalRadiusRatio;
			voidExplosionInfo.HpToDmgRatio += spellConfig.float2 / 100f;
			voidExplosionInfo.InstantKillRatio = Mathf.Max(spellConfig.float3 / 100f, voidExplosionInfo.InstantKillRatio);
			break;
		case SpellAbilityType.MucusCrystal:
			spellMucusTime = Mathf.Max(spellConfig.float1);
			spellMucusMoveSpeedRatio *= spellConfig.float2 / 100f;
			spellMucusSpellSpeedRatio *= spellConfig.float3 / 100f;
			break;
		case SpellAbilityType.VenomCrystal:
			spellVenomTime = Mathf.Max(spellConfig.float1, spellVenomTime);
			spellVenomOnceCount += spellConfig.int2;
			break;
		case SpellAbilityType.Penetrate:
			penetrateTime += spellConfig.int1;
			break;
		case SpellAbilityType.SpellHover:
			SpellHoverTime += spellConfig.float1;
			break;
		case SpellAbilityType.AroundOwner:
			bonusSpeed += spellConfig.float2;
			currentSpellMovement = SpellSpecialMovementType.Rotation;
			spellAroundOwnerRadius = spellConfig.float3;
			break;
		case SpellAbilityType.AroundMouse:
			bonusSpeed += spellConfig.float1;
			currentSpellMovement = SpellSpecialMovementType.ChaseMouse;
			spellFollowMouseLerp = spellConfig.float3;
			break;
		case SpellAbilityType.FollowTarget:
			currentSpellMovement = SpellSpecialMovementType.ChaseEnemy;
			spellFollowTargetRotateSpeed += spellConfig.float1;
			break;
		case SpellAbilityType.FollowOwner:
			currentSpellMovement = SpellSpecialMovementType.ChaseOwner;
			spellFollowTargetRotateSpeed += spellConfig.float1;
			break;
		case SpellAbilityType.Rebound:
			rebounceTime += spellConfig.int1;
			extraReboundTime += spellConfig.int1;
			reboundAddTime = Mathf.Max(reboundAddTime, spellConfig.float1);
			break;
		case SpellAbilityType.SpellSplit:
			spellSplitCount += spellConfig.int1;
			break;
		case SpellAbilityType.Frozen:
			spellFrozenTime += spellConfig.float1;
			break;
		case SpellAbilityType.PullForceCrystal:
			criticalDragDamagePercent = Mathf.Max((float)spellConfig.int1 / 100f, criticalDragDamagePercent);
			criticalDragApllyToCount += spellConfig.int2;
			criticalDragPullForce = Mathf.Max(spellConfig.float2, criticalDragPullForce);
			criticalDragEffectRadiu = Mathf.Max(spellConfig.float1, criticalDragEffectRadiu);
			break;
		case SpellAbilityType.PowerSavingMode:
			damageRatio *= spellConfig.float3 / 100f;
			SpellSUmmonFinalHpRatio *= spellConfig.float3 / 100f;
			break;
		case SpellAbilityType.EnhanceSpeedValue:
			bonusSpeed += spellConfig.float1;
			break;
		case SpellAbilityType.EnhanceSummonHPRecover:
			SpellSummonHPRatio += spellConfig.float2 / 100f;
			SpellSummonHPRecover += spellConfig.float1;
			break;
		case SpellAbilityType.ParasiticWorm:
			SpellSummonAfterDeadSpawnWormCount += spellConfig.int2;
			SpellSummonHPFixDropAmount += spellConfig.float1;
			SpellSummonAttackSpeedRatio += spellConfig.float2 / 100f;
			SpellSummonMoveRatio += spellConfig.float3 / 100f;
			break;
		case SpellAbilityType.LifeLine:
			lifelineDamage += spellConfig.float1;
			lifelineDamageInterval = Mathf.Min(lifelineDamageInterval, spellConfig.float2);
			break;
		case SpellAbilityType.FireCrystal:
			damageRatio += spellConfig.float1 / 100f;
			spellBurnTime = Mathf.Max(spellBurnTime, spellConfig.float2);
			burnHpRatioPerSeconds += spellConfig.int1;
			break;
		case SpellAbilityType.ThunderCrystal:
			endThunderHitRadiu = Mathf.Max(endThunderHitRadiu, spellConfig.float1);
			endThunderHitPercent += spellConfig.float2 / 100f;
			break;
		case SpellAbilityType.SpellEndTeleport:
			spellEndTeleport = true;
			break;
		}
	}

	protected virtual bool TryRefractOrPenetrate(params GameObject[] hits)
	{
		if (currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (penetrateTime > 0)
			{
				penetrateTime--;
				return true;
			}
			if (remainRefractCount > 0)
			{
				remainRefractCount--;
				return true;
			}
		}
		else
		{
			if (remainRefractCount > 0)
			{
				TryRefract(hits);
				return true;
			}
			if (penetrateTime > 0)
			{
				penetrateTime--;
				return true;
			}
		}
		return false;
	}

	public virtual void TryRefractOrPenetrateOrRecycleOnHitTarget(params GameObject[] hits)
	{
		if (!TryRefractOrPenetrate(hits))
		{
			PoolRecycle();
		}
	}

	[CanBeNull]
	protected virtual UnitProperty TryRefract(params GameObject[] hitTarget)
	{
		refractedTargets.AddRange(hitTarget.Where((GameObject e) => !refractedTargets.Contains(e)));
		(int, float)? refractionInfo = SIP.RefractionInfo;
		if (!refractionInfo.HasValue)
		{
			return null;
		}
		if (remainRefractCount <= 0)
		{
			return null;
		}
		remainRefractCount--;
		UnitProperty nextRefractionTargetablePpt = GetNextRefractionTargetablePpt();
		if (!nextRefractionTargetablePpt)
		{
			return null;
		}
		Direction = Tool2D.IgnoreZV2ToV1Normal(nextRefractionTargetablePpt.transform.position, checkNextRefractionPosition);
		if ((bool)rigid)
		{
			rigid.linearVelocity = rigid.linearVelocity.magnitude * Direction;
		}
		return nextRefractionTargetablePpt;
	}

	public virtual float GetFallingGroundDamageRadius()
	{
		return spellCfg.radius + SIP.fallExplosionRadius * radiusRatio * finalRadiusRatio;
	}

	protected IEnumerable<Collider> GetFallingGroundDamageTargets()
	{
		return GetFallingGroundDamageTargets(base.transform.position);
	}

	protected IEnumerable<Collider> GetFallingGroundDamageTargets(Vector3 position)
	{
		float fallingGroundDamageRadius = GetFallingGroundDamageRadius();
		int count = GeneralTool.GetCollidersNonAlloc(position.IgnoreZ(), fallingGroundDamageRadius, fallingGroundDamageTargetsBuffer, fallingGroundDamageTargetsTags);
		for (int i = 0; i < count; i++)
		{
			yield return fallingGroundDamageTargetsBuffer[i];
		}
	}

	protected virtual void MakeFallingGroundDamageToAround(Vector3? position = null)
	{
		Vector3 valueOrDefault = position.GetValueOrDefault();
		if (!position.HasValue)
		{
			valueOrDefault = base.transform.position;
			position = valueOrDefault;
		}
		foreach (Collider fallingGroundDamageTarget in GetFallingGroundDamageTargets(position.Value))
		{
			AOETriggerIn(fallingGroundDamageTarget);
		}
		EffectBase.CreateFallingExplosion(position);
	}

	protected virtual void UpdateUpSpeedWithFalling()
	{
		if (InFallRebounding)
		{
			CurrentUpSpeed -= Time.deltaTime * InFallingReboundingGravity;
		}
	}

	protected virtual bool TryFallingRefract()
	{
		if (remainRefractCount <= 0 || lastOutputDamageFrame != Time.frameCount)
		{
			return false;
		}
		TryRefract();
		CurrentUpSpeed = FallingReboundForce;
		InFallRebounding = true;
		return true;
	}

	protected virtual bool TryFallingRebound()
	{
		if (rebounceTime <= 0 || !(CurrentUpSpeed < 0f))
		{
			return false;
		}
		CurrentUpSpeed = FallingReboundForce;
		InFallRebounding = true;
		rebounceTime--;
		return true;
	}

	protected virtual bool OnFallingGroundTryRebound()
	{
		if ((bool)GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(base.transform), 0.1f, "Abyss", "Abyss"))
		{
			return false;
		}
		if (!TryFallingRefract())
		{
			return TryFallingRebound();
		}
		return true;
	}

	protected virtual void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			PoolRecycle();
		}
	}

	protected virtual TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		refractedTargets.Add(unit.gameObject);
		return OutputDamage(unit);
	}

	protected virtual bool OnAOEHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		MakeDamageToUnit(unit);
		return true;
	}

	protected virtual bool OnHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		MakeDamageToUnit(unit);
		CreateHitEffect();
		TryRefractOrPenetrateOrRecycleOnHitTarget(unit.gameObject);
		return true;
	}

	protected virtual void MakeDamageToDestructible(UnitProperty go)
	{
		if (!IsSameCamp(go))
		{
			OutputDamage(go);
		}
	}

	protected virtual bool OnAOEHitDestructible(UnitProperty go)
	{
		if (IsSameCamp(go))
		{
			return false;
		}
		MakeDamageToDestructible(go);
		return true;
	}

	protected virtual bool OnHitDestructible(UnitProperty go)
	{
		if (IsSameCamp(go))
		{
			return false;
		}
		MakeDamageToDestructible(go);
		CreateHitEffect();
		TryRefractOrPenetrateOrRecycleOnHitTarget(go.gameObject);
		return true;
	}

	protected virtual void MakeDamageToBrittleness(GameObject go)
	{
		OutputDamage(go);
	}

	protected virtual bool OnAOEHitBrittleness(GameObject go)
	{
		MakeDamageToBrittleness(go);
		return true;
	}

	protected virtual bool OnHitBrittleness(GameObject go)
	{
		MakeDamageToBrittleness(go);
		return true;
	}

	protected virtual bool MakeDamageToSpell(SpellBase spell)
	{
		if (!(spell is Spell1003Butterfly spell1003Butterfly))
		{
			if (spell is Spell1002RollBall spell1002RollBall)
			{
				spell1002RollBall.TakeDamage(spellCfg.damage);
				return true;
			}
			return false;
		}
		spell1003Butterfly.Break();
		return true;
	}

	protected virtual bool OnAOEHitSpell(SpellBase spell)
	{
		if (IsSameCamp(spell))
		{
			return false;
		}
		if (spell is Spell1003Butterfly || spell is Spell1002RollBall)
		{
			MakeDamageToSpell(spell);
			return true;
		}
		return false;
	}

	protected virtual bool OnAOEHitWallAndSolidObj(Collider col)
	{
		return false;
	}

	protected virtual bool OnHitSpell(SpellBase spell)
	{
		if (IsSameCamp(spell))
		{
			return false;
		}
		if (!(spell is Spell1003Butterfly))
		{
			if (spell is Spell1002RollBall)
			{
				CreateHitEffect();
				MakeDamageToSpell(spell);
				TryRefractOrPenetrateOrRecycleOnHitTarget(spell.gameObject);
				return true;
			}
			return false;
		}
		MakeDamageToSpell(spell);
		return true;
	}

	protected virtual void OnHitWallAndSolidObj(Collider col)
	{
		if (!isThroughWall && currentSpellMovement != SpellSpecialMovementType.Rotation && rebounceTime <= 0)
		{
			HitEFAndRecycle();
		}
	}

	protected virtual void OnHitUnknownTagObject(Collider col)
	{
	}

	protected bool AOETriggerIn(Collider col)
	{
		if (col.gameObject.CompareAnyTag("Player", "Teammate", "Monster"))
		{
			return OnAOEHitUnit(col.GetComponent<UnitProperty>());
		}
		if (col.gameObject.CompareAnyTag("Brittleness"))
		{
			return OnAOEHitBrittleness(col.gameObject);
		}
		if (col.gameObject.CompareAnyTag("Destructible"))
		{
			return OnAOEHitDestructible(col.GetComponent<UnitProperty>());
		}
		if (col.gameObject.CompareAnyTag("SolidObj", "Wall"))
		{
			return OnAOEHitWallAndSolidObj(col);
		}
		if (col.gameObject.CompareAnyTag("Butterfly", "RollBall", "Spell"))
		{
			return OnAOEHitSpell(col.GetComponentInParent<SpellBase>());
		}
		Debug.LogWarning($"{base.gameObject} 的 AOE 伤害不能处理击中 Tag 为 {col.tag} 的物体 {col}");
		return false;
	}

	protected virtual Vector3 GetFallingViewDirection()
	{
		return new Vector2(Direction.x * CurrentSpeed, CurrentUpSpeed + Direction.y * CurrentSpeed).normalized;
	}

	protected virtual void OnDrawGizmos()
	{
	}
}
