using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spell4014LaserCrystal : SpellBase
{
	private float currentDamageAndManaCostRatio = 1f;

	private float costRatioUpSpeedPerSecond;

	private float shootBaseManaCost;

	private float attackTimer;

	private bool stopShootingUntilShootWandFullMana;

	private int whichRing;

	private int inRingIndex;

	private int ringTotalCrystalNum;

	private int wandIndex;

	private int crystalWandCount = 1;

	private bool alwaysAttack;

	public LayerMask AttackLayer;

	private List<Collider> hitTargets = new List<Collider>();

	[Space(50f)]
	public AudioSource as_Loop;

	private float audioVolumnRatio;

	public float audioBasePitch;

	public float audioTimeToPitchRatio;

	public Transform CenterTransform;

	public float BaseDetectRadiu;

	private float splitAngleShift;

	public float restartWandManaThreshold;

	public float stopFireWandManaThreshold;

	public float aroundOwnerBaseRadiu;

	public float LaserNodeAroundCenterDistance;

	public float ball2shift;

	public float ball1Yshift;

	public float ball2Yshift;

	public float ball12XRatio;

	public float CrystalMovelerpTime;

	public float LaserAttackBaseWidth;

	public float LaserAttackWidthRatioUpPerSecond;

	public float LaserAttackMaxWidth;

	public float LaserAttackSingleMoveDistance;

	public float LaserBaselength;

	public Vector3 LaserAttackHeightShift;

	public float LaserNormalMovementChaseAngleSpeed;

	public float LaserNormalChaseSpeedUpRatio;

	private float laserAttackBaseDamage;

	public float SplitCrystalRotateBaseRadiu;

	public float SplitCrystalRotateSpeed;

	private float splitCrystalRotateRadiu;

	private Spell4014LaserCrystal splitSpellOwner;

	private Transform followTransform;

	private float currentScatter;

	public float thunderAttackInterval;

	private float thunderAttackTimer;

	private AutoWandData autowand;

	private List<Vector3> hitPoints = new List<Vector3>();

	private Vector3 currentCheckPoint = Vector3.zero;

	private List<Vector3> bcpList = new List<Vector3>();

	public string HitSEName;

	private static List<Spell4014LaserCrystal> fullCrystalList = new List<Spell4014LaserCrystal>();

	private static readonly RaycastHit[] targetRaycastHitBuffer = new RaycastHit[128];

	private static readonly Collider[] detectTargetBuffer = new Collider[256];

	public float fallBaseDetectRange;

	public float fallBaseMoveSpeed;

	public float fallBaseHeight;

	private Vector3 currentFrameFallSpellHitPos = Vector3.zero;

	private Vector3 fallChaseCurrentEndPoint = Vector3.zero;

	public float FallChaseTargetNoTargetSpeedUp;

	private float fallChaseTargetNoTargetStackSpeed;

	private float crystalPowerRatio = 1f;

	private static readonly HashSet<string> detectTags = new HashSet<string> { "RollBall", "Butterfly", "Brittleness", "Monster", "Wall", "Destructible" };

	private static readonly HashSet<string> fallDetectTags = new HashSet<string> { "RollBall", "Butterfly", "Brittleness", "Monster", "Destructible" };

	public UnitProperty targetPpt { get; set; }

	public bool attackedInThisFrame { get; set; }

	public float attackTotalTime { get; set; }

	public int SplitCount { get; set; }

	public List<GameObject> SplitCrystalList { get; set; } = new List<GameObject>();


	public List<Spell4014LaserCrystal> SplitCrystalScriptList { get; set; } = new List<Spell4014LaserCrystal>();


	public int SplitIndex { get; set; }

	public bool CrystalIsAttacking
	{
		get
		{
			if (!targetPpt)
			{
				return alwaysAttack;
			}
			return true;
		}
	}

	protected override Vector3 checkNextRefractionPosition => currentCheckPoint;

	public List<Vector3> rayNodes { get; set; } = new List<Vector3>();


	public Vector3 fallMoveVector { get; set; } = Vector3.zero;


	public List<Vector3> fallShadowRayNodes { get; set; } = new List<Vector3>();


	public Vector3 endPos { get; set; } = Vector3.zero;


	protected override void InitSpeedAndPositionWithFall()
	{
		base.InitSpeedAndPositionWithoutFall();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		CenterTransform.localPosition = Vector3.zero;
		audioVolumnRatio = 0f;
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		crystalPowerRatio = 1f;
	}

	public void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		as_Loop.Stop();
		fullCrystalList.Remove(this);
	}

	private void SoundVolumeChange()
	{
		audioVolumnRatio = Mathf.Lerp(audioVolumnRatio, (CrystalIsAttacking && attackedInThisFrame) ? 1f : 0f, 0.15f);
		as_Loop.volume = DataMgr.settingData.GetFinalSound() * audioVolumnRatio;
		as_Loop.pitch = Mathf.Min(audioBasePitch + attackTotalTime / audioTimeToPitchRatio, 2f);
	}

	private void UpdateOveralSoundVolume()
	{
		if (fullCrystalList.Count <= 0)
		{
			Debug.LogWarning("为啥队列里没东西 不应该是这样的");
		}
		else
		{
			if (fullCrystalList[0] == null || fullCrystalList[0] != this)
			{
				return;
			}
			float num = attackTotalTime;
			bool flag = attackedInThisFrame;
			if (!as_Loop.isPlaying)
			{
				as_Loop.Play();
			}
			foreach (Spell4014LaserCrystal fullCrystal in fullCrystalList)
			{
				if (!(fullCrystal == null) && !(fullCrystal == this))
				{
					if (fullCrystal.attackedInThisFrame)
					{
						flag = true;
					}
					num = Mathf.Max(num, fullCrystal.attackTotalTime);
					fullCrystal.as_Loop.Stop();
				}
			}
			audioVolumnRatio = Mathf.Lerp(audioVolumnRatio, flag ? 1f : 0f, 0.15f);
			as_Loop.volume = DataMgr.settingData.GetFinalSound() * audioVolumnRatio;
			as_Loop.pitch = Mathf.Min(audioBasePitch + num / audioTimeToPitchRatio, 2f);
		}
	}

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		SplitCrystalList.Clear();
		SplitCrystalScriptList.Clear();
		CenterTransform.localPosition = Vector3.zero;
		targetPpt = null;
		currentDamageAndManaCostRatio = 1f;
		costRatioUpSpeedPerSecond = 0f;
		SplitCount = 0;
		SplitIndex = 0;
		attackTimer = 0f;
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		whichRing = 0;
		inRingIndex = 0;
		ringTotalCrystalNum = 0;
		attackTotalTime = 0f;
		wandIndex = 0;
		splitAngleShift = 0f;
		crystalWandCount = 1;
		followTransform = null;
		attackedInThisFrame = false;
		alwaysAttack = false;
		base.spellCfg.abilityType = SpellAbilityType.LaserBeam;
		stopShootingUntilShootWandFullMana = false;
		currentScatter = UnityEngine.Random.Range((0f - base.wandShootAngle) / 2f, base.wandShootAngle / 2f);
		splitCrystalRotateRadiu = SplitCrystalRotateBaseRadiu * base.radiusRatio * base.finalRadiusRatio;
		splitSpellOwner = null;
		spellEndTeleport = false;
		thunderAttackTimer = 0f;
		alwaysAttack = true;
		laserAttackBaseDamage = 0f;
		autowand = null;
		if (base.endThunderHitPercent > 0f && base.ColorType == SpellColorType.Player)
		{
			base.ColorType = SpellColorType.Thunder;
		}
		fullCrystalList.Add(this);
		SetCrystalInitialPos();
		if (base.SIP.spellIsFall)
		{
			AddFallHorizontalMoveSpeed();
			fallMoveVector = Vector3.zero;
			fallChaseCurrentEndPoint = base.transform.position;
			fallChaseTargetNoTargetStackSpeed = 100f;
		}
	}

	public override void Update()
	{
		base.Update();
		thunderAttackTimer += Time.deltaTime;
		UpdateTurrentRotateAngle();
		if (base.spellCfg.isSplitSpell)
		{
			SplitCrystalMove();
		}
		else
		{
			UpdateTurrentPosition();
		}
		UpdateLaserPoints();
		DealDamgeToHitTargets();
		UpdateTotalAttackTimer();
		UpdateBuffRatio();
		UpdateOveralSoundVolume();
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		Spell4014StaticAngleCalculator.UpdatedAngleInThisFrame = false;
	}

	public void SetSplitSpellOwner(Spell4014LaserCrystal spellOwner)
	{
		splitSpellOwner = spellOwner;
	}

	public void SetCrystalEffectPowerRatio(float ratio)
	{
		crystalPowerRatio = ratio;
		spellVenomOnceCount *= ratio;
	}

	private void UpdateBodySize()
	{
		if (base.spellCfg.isSplitSpell)
		{
			base.transform.localScale = Vector3.one * 0.6f * base.spellVolumeRatio;
		}
		else
		{
			base.transform.localScale = Vector3.one * base.spellVolumeRatio;
		}
	}

	public override void OnFirstFrame()
	{
		if (SplitCrystalList.Count <= 0)
		{
			EffectBase.ManualCreateEffect("Spell");
			EffectBase.ManualCreateEffect("Laser");
			EffectBase.ManualCreateEffect("Start");
			EffectBase.ManualCreateEffect("End");
			EffectBase.ManualCreateEffect("Shadow");
			UpdateBodySize();
		}
		foreach (GameObject splitCrystal in SplitCrystalList)
		{
			SplitCrystalScriptList.Add(splitCrystal.GetComponent<Spell4014LaserCrystal>());
		}
		((Spell4014Effect)EffectBase).ClearCrystalTrail();
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (base.spellCfg.isSplitSpell)
			{
				base.spellAroundOwnerCurrentAngle = 360f / (float)SplitCount * (float)SplitIndex;
			}
			else
			{
				base.spellAroundOwnerCurrentAngle = GetCurrentSpellAroundOwnerAngle();
			}
		}
	}

	private float GetCurrentSpellAroundOwnerAngle()
	{
		float num = 360f / (float)crystalWandCount * (float)wandIndex;
		float num2 = 360f / (float)ringTotalCrystalNum * (float)inRingIndex;
		float num3 = (float)whichRing * 120f;
		return 0f + (num2 + num + num3);
	}

	public void SetCrystalFollowTransform(Transform trans, AutoWandData data)
	{
		followTransform = trans;
		autowand = data;
	}

	private void UpdateTotalAttackTimer()
	{
		if (CrystalIsAttacking && attackedInThisFrame)
		{
			attackTotalTime += Time.deltaTime;
		}
		else
		{
			attackTotalTime = Mathf.Max(1f, attackTotalTime - Time.deltaTime);
		}
	}

	private void UpdateFireState()
	{
		float num = base.shooterWand.CurrentMP + PlayerMgr.Inst.CurrentManaAmountFromWandAbility(base.shooterWand);
		float num2 = base.shooterWand.CurrentMP + PlayerMgr.Inst.MaxManaAmountFromWandAbility(base.shooterWand);
		float num3 = num / num2;
		if (stopShootingUntilShootWandFullMana && num3 >= restartWandManaThreshold)
		{
			stopShootingUntilShootWandFullMana = false;
		}
	}

	private void UpdateBuffRatio()
	{
		if (CrystalIsAttacking && attackedInThisFrame)
		{
			currentDamageAndManaCostRatio += costRatioUpSpeedPerSecond * Time.deltaTime;
			return;
		}
		currentDamageAndManaCostRatio -= costRatioUpSpeedPerSecond * Mathf.Pow(0.5f, base.spellCfg.level) * Time.deltaTime;
		currentDamageAndManaCostRatio = Mathf.Max(1f, currentDamageAndManaCostRatio);
	}

	private void CheckShooterWandManaPercent()
	{
		if (base.shooterWand.GetCurrentManaPercent() < stopFireWandManaThreshold)
		{
			stopShootingUntilShootWandFullMana = true;
		}
	}

	public void SetLaserLevelBaseData(int laserLevel)
	{
		SpellConfig spellConfig = SpellConfig.dic[40140 + laserLevel];
		spellConfig.level = laserLevel;
		base.spellCfg.level = laserLevel;
		if (base.spellCfg.isSplitSpell)
		{
			base.finalDamageRatio *= 0.35f;
		}
		base.spellCfg.damage = Mathf.Ceil((float)spellConfig.int1 * base.damageRatio * base.finalDamageRatio) + base.SIP.finalDamageExtra;
		laserAttackBaseDamage = base.spellCfg.damage;
		shootBaseManaCost = spellConfig.float1 * base.shooterWand.GetWandMpCorrection() * GetEnhanceMpCorrection();
		costRatioUpSpeedPerSecond = spellConfig.float2 / 100f;
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.radius = base.SIP.fallExplosionRadius * base.radiusRatio * base.finalRadiusRatio;
		}
		else
		{
			base.spellCfg.radius = BaseDetectRadiu * base.radiusRatio * base.finalRadiusRatio;
		}
		base.spellCfg.DPSDamageInterval = spellConfig.DPSDamageInterval;
	}

	private float GetEnhanceMpCorrection()
	{
		float num = 1f;
		for (int i = 0; i <= preSpellIDs.Count - 1; i++)
		{
			SpellConfig spellConfig = SpellConfig.dic[preSpellIDs[i]];
			if (spellConfig.mpCostMulDivCorrection != 0f)
			{
				num *= spellConfig.mpCostMulDivCorrection / 100f;
			}
		}
		num /= Mathf.Max(1f, base.SIP.multiShootCount);
		return num / Mathf.Max(1f, SplitCount);
	}

	public void SetSplitCrystalData(int index, int count)
	{
		SplitIndex = index;
		SplitCount = count;
		splitAngleShift = 360f / (float)count * (float)index;
		base.spellCfg.isSplitSpell = true;
		if (splitSpellOwner != null)
		{
			SplitCrystalMove();
		}
	}

	public void RecycleAllSplitCrystal()
	{
		if (SplitCrystalList.Count <= 0)
		{
			return;
		}
		for (int num = SplitCrystalList.Count; num > 0; num--)
		{
			if (!SplitCrystalList[num - 1].IsDestroyed())
			{
				SplitCrystalList[num - 1].transform.parent = null;
				SplitCrystalList[num - 1].GetComponent<Spell4014LaserCrystal>().PoolRecycle();
			}
		}
		SplitCrystalList.Clear();
	}

	public override void PoolRecycle()
	{
		base.endThunderHitPercent = 0f;
		base.PoolRecycle();
	}

	private void SplitCrystalMove()
	{
		if ((bool)splitSpellOwner)
		{
			CenterTransform.localPosition = Vector3.zero;
			float num = 360f / (MathF.PI * 2f * splitCrystalRotateRadiu / SplitCrystalRotateSpeed) * Time.deltaTime;
			base.spellAroundOwnerCurrentAngle += num;
			float num2 = ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? base.spellAroundOwnerRadius : splitCrystalRotateRadiu);
			Vector3 v = splitSpellOwner.CenterTransform.position + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + splitAngleShift) * num2;
			if (base.SIP.spellIsFall && base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				v = splitSpellOwner.transform.position + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + splitAngleShift) * num2;
			}
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			CenterTransform.localPosition = Vector3.zero;
			if (base.SIP.spellIsFall && base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				CenterTransform.localPosition = new Vector3(0f, fallBaseHeight + 2.3f, 0f - fallBaseHeight);
			}
		}
	}

	private int GetCurrentDamage()
	{
		return Mathf.CeilToInt(laserAttackBaseDamage * crystalPowerRatio * base.spellCfg.DPSDamageInterval * currentDamageAndManaCostRatio);
	}

	private Vector3 GetOwnerBasePoint()
	{
		Vector3 position = ownerPpt.transform.position;
		if (!followTransform)
		{
			return position + LaserAttackHeightShift;
		}
		return Tool2D.IgnoreZPoint(followTransform.position);
	}

	public void SetCrystalInitialPos()
	{
		base.transform.position = GetOwnerBasePoint();
	}

	public void AddFallHorizontalMoveSpeed()
	{
		base.CurrentSpeed += fallBaseMoveSpeed * base.speedRatio * base.finalSpeedRatio;
	}

	private Vector3 GetRingPointShiftPos()
	{
		float num = aroundOwnerBaseRadiu * base.radiusRatio * base.finalRadiusRatio;
		float overalAnlge = Spell4014StaticAngleCalculator.OveralAnlge;
		float num2 = 360f / (float)crystalWandCount * (float)wandIndex;
		float num3 = 360f / (float)ringTotalCrystalNum * (float)inRingIndex;
		overalAnlge += num3 + num2;
		Vector3 result = Vector3.zero;
		switch (whichRing)
		{
		case 0:
			result = new Vector3(Mathf.Cos(overalAnlge * (MathF.PI / 180f)) * num * ball12XRatio, (0f - Mathf.Cos((overalAnlge + ball1Yshift) * (MathF.PI / 180f))) * num, (0f - Mathf.Sin(overalAnlge * (MathF.PI / 180f))) * num * 2.5f);
			break;
		case 1:
			overalAnlge += ball2shift;
			result = new Vector3(Mathf.Cos(overalAnlge * (MathF.PI / 180f)) * num * ball12XRatio, (0f - Mathf.Cos((overalAnlge + ball2Yshift) * (MathF.PI / 180f))) * num, (0f - Mathf.Sin(overalAnlge * (MathF.PI / 180f))) * num * 2.5f);
			break;
		case 2:
			result = new Vector3((0f - Mathf.Cos(overalAnlge * (MathF.PI / 180f))) * num, Mathf.Sin(overalAnlge * (MathF.PI / 180f)) * num * 0.45f, Mathf.Sin(overalAnlge * (MathF.PI / 180f)) * num * 2.5f);
			break;
		}
		return result;
	}

	private void UpdateTurrentPosition(bool isInstancemove = false)
	{
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.ChaseMouse:
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3 vector = base.transform.position;
			if (base.SIP.spellIsFall)
			{
				switch (base.currentSpellMovement)
				{
				case SpellSpecialMovementType.ChaseEnemy:
				{
					Vector3 position = base.transform.position;
					if (base.SpellFollowHaveTarget)
					{
						position = spellFollowTargetPpt.transform.position;
						fallMoveVector = Tool2D.DirMoveTowards(fallMoveVector.IgnoreZ(), ToPointDir(position.IgnoreZ()), base.CurrentSpeed * spellFollowTargetRotateSpeed * 3f * Time.deltaTime);
						vector += fallMoveVector * base.CurrentSpeed * Time.deltaTime;
					}
					else
					{
						spellFollowTargetPpt = GetNearestTargetablePpt(base.transform.position);
						position = GetOwnerBasePoint();
						fallMoveVector = Tool2D.IgnoreZV2ToV1Normal(position, base.transform.position);
						vector = Vector3.Lerp(vector, position, base.CurrentSpeed / 3f * Time.deltaTime);
					}
					break;
				}
				case SpellSpecialMovementType.ChaseMouse:
				{
					Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
					fallMoveVector = Vector3.Lerp(fallMoveVector, ToPointDir(mousePoint) * (base.CurrentSpeed + 2f), base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp);
					vector += fallMoveVector * Time.deltaTime;
					break;
				}
				case SpellSpecialMovementType.Normal:
				case SpellSpecialMovementType.ChaseOwner:
					vector = GetOwnerBasePoint();
					break;
				}
			}
			else
			{
				vector = GetOwnerBasePoint();
			}
			base.transform.position = Tool2D.IgnoreZPoint(vector);
			Spell4014StaticAngleCalculator.UpdateOveralAngle();
			Vector3 ringPointShiftPos = GetRingPointShiftPos();
			if (base.SIP.spellIsFall && !base.spellCfg.isSplitSpell)
			{
				ringPointShiftPos += new Vector3(0f, fallBaseHeight, 0f - fallBaseHeight);
			}
			if (isInstancemove)
			{
				CenterTransform.localPosition = ringPointShiftPos;
			}
			else
			{
				CenterTransform.DOLocalMove(ringPointShiftPos, CrystalMovelerpTime);
			}
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			Vector3 obj = (followTransform ? followTransform.position : ownerPpt.transform.position);
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / (base.CurrentSpeed + 1f)) * Time.deltaTime;
			base.spellAroundOwnerCurrentAngle += num;
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
			Vector3 v = obj + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			CenterTransform.localPosition = ((base.SIP.spellIsFall && !base.spellCfg.isSplitSpell) ? new Vector3(0f, fallBaseHeight, 0f - fallBaseHeight) : Vector3.zero);
			break;
		}
		}
	}

	public void SetRotateRingData(int whichRing, int inRingIndex, int ringTotalCrystalNum)
	{
		this.whichRing = whichRing;
		this.inRingIndex = inRingIndex;
		this.ringTotalCrystalNum = ringTotalCrystalNum;
		UpdateTurrentPosition(isInstancemove: true);
	}

	public void SetCrystalIndexData(int wandIndex, int crystalWandCount)
	{
		this.wandIndex = wandIndex;
		this.crystalWandCount = crystalWandCount;
	}

	private void UpdateTurrentRotateAngle()
	{
		if (autowand == null)
		{
			if (ownerPpt == null || !ownerPpt.isActiveAndEnabled)
			{
				return;
			}
			base.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(Tool2D.IgnoreZPoint(PlayerMgr.Inst.ShootPoint), ownerPpt.transform.position), currentScatter);
		}
		else
		{
			if (!autowand.wandObject.gameObject.activeInHierarchy)
			{
				return;
			}
			base.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(Tool2D.IgnoreZPoint(PlayerMgr.Inst.GetMousePoint()), Tool2D.IgnoreZPoint(autowand.wandObject.transform.position)), currentScatter);
		}
		if (base.SIP.reverseDirection)
		{
			base.Direction *= -1f;
		}
	}

	public float GetCurrentLaserWidth()
	{
		float num = Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f);
		return Mathf.Clamp((LaserAttackBaseWidth + LaserAttackWidthRatioUpPerSecond * attackTotalTime) * num, LaserAttackBaseWidth, LaserAttackMaxWidth * base.spellVolumeRatio) * base.spellVolumeRatio;
	}

	private void ClearRefractionDataWhenUpdateLaserPoints()
	{
		(int, float)? refractionInfo = base.SIP.RefractionInfo;
		if (refractionInfo.HasValue)
		{
			base.remainRefractCount = base.SIP.RefractionInfo.Value.count;
		}
		else
		{
			base.remainRefractCount = 0;
		}
		refractedTargets.Clear();
	}

	private void UpdateLaserPoints()
	{
		ClearRefractionDataWhenUpdateLaserPoints();
		rayNodes.Clear();
		hitTargets.Clear();
		hitPoints.Clear();
		fallShadowRayNodes.Clear();
		if (!CrystalIsAttacking)
		{
			return;
		}
		if (base.SIP.spellIsFall)
		{
			UpdateLaserNodeInFallMovement();
			return;
		}
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Rotation:
			UpdateLaserNodesInRotationMovement();
			break;
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.ChaseMouse:
		case SpellSpecialMovementType.ChaseOwner:
			NewUpdateLaserNodesInChaseMovement();
			break;
		}
	}

	public float GetFinalFallDetectRange()
	{
		return fallBaseDetectRange * base.radiusRatio * base.finalRadiusRatio;
	}

	public void UpdateLaserNodeInFallMovement()
	{
		Vector3 item = CenterTransform.position + new Vector3(0f, -0.3f, -0.3f);
		endPos = base.transform.position.IgnoreZ();
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.ChaseEnemy:
			if (base.SpellFollowHaveTarget)
			{
				if (Tool2D.IgnoreZDistance(base.transform.position, spellFollowTargetPpt.transform.position) >= GetFinalFallDetectRange())
				{
					endPos += Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, base.transform.position) * GetFinalFallDetectRange();
				}
				else
				{
					endPos = spellFollowTargetPpt.transform.position;
				}
				fallChaseTargetNoTargetStackSpeed = 0f;
			}
			else
			{
				GetNearestTargetablePpt(base.transform.position);
				endPos = CenterTransform.position + new Vector3(0f, 0f - fallBaseHeight, fallBaseHeight);
				fallChaseTargetNoTargetStackSpeed += FallChaseTargetNoTargetSpeedUp * Time.deltaTime;
			}
			endPos = Vector3.Lerp(fallChaseCurrentEndPoint, endPos, (base.CurrentSpeed + fallChaseTargetNoTargetStackSpeed) * Time.deltaTime);
			fallChaseCurrentEndPoint = endPos;
			break;
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint();
			if (Tool2D.IgnoreZDistance(base.transform.position, mousePoint) >= GetFinalFallDetectRange())
			{
				endPos += Tool2D.IgnoreZV2ToV1Normal(mousePoint, base.transform.position) * GetFinalFallDetectRange();
			}
			else
			{
				endPos = PlayerMgr.Inst.GetMousePoint();
			}
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
			endPos = CenterTransform.position + new Vector3(0f, 0f - fallBaseHeight, fallBaseHeight);
			break;
		case SpellSpecialMovementType.Rotation:
			endPos = base.transform.position.IgnoreZ();
			break;
		}
		rayNodes.Add(item);
		rayNodes.Add(endPos);
		fallShadowRayNodes.Add(CenterTransform.position + new Vector3(0f, 0f - fallBaseHeight, fallBaseHeight));
		fallShadowRayNodes.Add(endPos);
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(endPos.IgnoreZ(), GetFallingGroundDamageRadius(), detectTargetBuffer, fallDetectTags, AttackLayer);
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			hitTargets.Add(detectTargetBuffer[i]);
		}
		currentFrameFallSpellHitPos = endPos;
	}

	private UnitProperty GetNearestAngleTargetAblePpt(Vector3 startPos)
	{
		return LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetablePpt(startPos, base.Direction);
	}

	private void NewUpdateLaserNodesInChaseMovement()
	{
		Vector3 vector = Tool2D.IgnoreZPoint(CenterTransform.position + base.Direction * LaserNodeAroundCenterDistance) + new Vector3(0f, 0f, -0.3f);
		Vector3 dir = base.Direction;
		float num = LaserBaselength + base.bonusSpeed;
		rayNodes.Add(vector);
		int num2 = 0;
		bool flag = false;
		currentCheckPoint = vector;
		bcpList.Clear();
		int num3 = base.penetrateTime;
		float num4 = 1f;
		hitTargets.Clear();
		bool flag2 = true;
		bool flag3 = false;
		refractedTargets.Clear();
		while (flag2)
		{
			num2++;
			if (num2 >= 500)
			{
				Debug.LogError("死循环 请检查");
				break;
			}
			switch (base.currentSpellMovement)
			{
			case SpellSpecialMovementType.ChaseMouse:
			{
				Vector3 shootWorldPoint = PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint;
				List<Vector3> list = rayNodes;
				Vector3 b = Tool2D.IgnoreZV2ToV1Normal(shootWorldPoint, list[list.Count - 1]);
				dir = Vector3.Lerp(dir, b, spellFollowMouseLerp);
				break;
			}
			case SpellSpecialMovementType.ChaseEnemy:
				spellFollowTargetPpt = GetNearestAngleTargetAblePpt(vector);
				if (base.SpellFollowHaveTarget && !flag)
				{
					Vector3 vector2 = Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, rayNodes[rayNodes.Count - 1]);
					dir = Tool2D.DirMoveTowards(dir, vector2, 0.5f * spellFollowTargetRotateSpeed);
					if (dir == vector2)
					{
						flag = true;
					}
				}
				break;
			case SpellSpecialMovementType.Normal:
				spellFollowTargetPpt = GetNearestAngleTargetAblePpt(vector);
				if (GetNextRefractionTargetablePpt() != null)
				{
					spellFollowTargetPpt = GetNextRefractionTargetablePpt();
				}
				if (base.SpellFollowHaveTarget)
				{
					Vector3 dir3 = Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, rayNodes[rayNodes.Count - 1]);
					dir = Tool2D.DirMoveTowards(dir, dir3, LaserNormalMovementChaseAngleSpeed * (1f + Mathf.Min(3f, attackTotalTime * LaserNormalChaseSpeedUpRatio)));
				}
				break;
			case SpellSpecialMovementType.ChaseOwner:
				spellFollowTargetPpt = ownerPpt;
				if (base.SpellFollowHaveTarget)
				{
					Vector3 dir2 = Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, rayNodes[rayNodes.Count - 1]);
					dir = Tool2D.DirMoveTowardsTargetInCounterClockWise(dir, dir2, 0.1f * spellFollowTargetRotateSpeed);
				}
				break;
			}
			float num5 = LaserAttackSingleMoveDistance * num4;
			if (num5 > num)
			{
				num5 = num;
			}
			int num6 = GeneralTool.RaycastNonAlloc(Tool2D.IgnoreZPoint(rayNodes[rayNodes.Count - 1]) + new Vector3(0f, 0f, -0.3f), dir, num5, targetRaycastHitBuffer, detectTags, AttackLayer);
			Vector3 vector3 = rayNodes[rayNodes.Count - 1] + dir * num5;
			for (int i = 0; i < num6; i++)
			{
				switch (targetRaycastHitBuffer[i].collider.tag)
				{
				case "Monster":
					if (!hitTargets.Contains(targetRaycastHitBuffer[i].collider) && !(targetRaycastHitBuffer[i].transform.GetComponent<UnitBase>() is Monster17_Hat))
					{
						flag3 = true;
						hitTargets.Add(targetRaycastHitBuffer[i].collider);
						Vector3 dir4 = dir;
						if (LaserTryRefraction(targetRaycastHitBuffer[i].collider.gameObject, ref dir4))
						{
							vector3 = targetRaycastHitBuffer[i].point + dir * 0.2f;
							dir = Tool2D.IgnoreZPoint(dir4);
							continue;
						}
						num3--;
						if (num3 < 0)
						{
							flag2 = false;
							vector3 = targetRaycastHitBuffer[i].point;
						}
					}
					goto default;
				case "Destructible":
					flag3 = true;
					if (!hitTargets.Contains(targetRaycastHitBuffer[i].collider))
					{
						hitTargets.Add(targetRaycastHitBuffer[i].collider);
					}
					if (LaserTryRefraction(targetRaycastHitBuffer[i].collider.gameObject, ref dir))
					{
						vector3 = targetRaycastHitBuffer[i].point;
						continue;
					}
					num3--;
					if (num3 < 0)
					{
						flag2 = false;
						vector3 = targetRaycastHitBuffer[i].point;
					}
					goto default;
				case "Wall":
					if (!isThroughWall)
					{
						flag2 = false;
						vector3 = targetRaycastHitBuffer[i].point;
					}
					goto default;
				case "Butterfly":
				case "RollBall":
					if (!targetRaycastHitBuffer[i].collider.GetComponentInParent<SpellBase>().IsSameCamp(this))
					{
						flag3 = true;
						if (!hitTargets.Contains(targetRaycastHitBuffer[i].collider))
						{
							hitTargets.Add(targetRaycastHitBuffer[i].collider);
						}
					}
					goto default;
				case "Brittleness":
					flag3 = true;
					if (!hitTargets.Contains(targetRaycastHitBuffer[i].collider))
					{
						hitTargets.Add(targetRaycastHitBuffer[i].collider);
					}
					goto default;
				default:
					if (flag2)
					{
						continue;
					}
					break;
				}
				break;
			}
			num -= Vector3.Distance(rayNodes[rayNodes.Count - 1], vector3);
			rayNodes.Add(vector3);
			if (num <= 0.3f)
			{
				flag2 = false;
			}
		}
		if (!flag3)
		{
			hitTargets.Clear();
		}
	}

	private bool LaserTryRefraction(GameObject testTarget, ref Vector3 dir)
	{
		if (!refractedTargets.Contains(testTarget) && (bool)TryRefract(testTarget))
		{
			dir = base.Direction;
			return true;
		}
		return false;
	}

	private void DealDamgeToHitTargets()
	{
		if (PlayerMgr.Inst.CurrentManaAmountFromWandAbility(base.shooterWand) + base.shooterWand.CurrentMP <= GetManaCost() || stopShootingUntilShootWandFullMana || hitTargets.Count <= 0)
		{
			attackedInThisFrame = false;
		}
		else
		{
			if (!CrystalIsAttacking || SplitCrystalList.Count > 0)
			{
				return;
			}
			attackTimer += Time.deltaTime;
			if (attackTimer < base.spellCfg.DPSDamageInterval)
			{
				return;
			}
			attackTimer = 0f;
			attackedInThisFrame = true;
			base.spellCfg.damage = GetCurrentDamage();
			base.shooterWand.CostMp(GetManaCost());
			if (0f >= UnityEngine.Random.Range(0f, 1f))
			{
				foreach (Vector3 hitPoint in hitPoints)
				{
					CreateHitEffect(hitPoint);
				}
			}
			foreach (Collider hitTarget in hitTargets)
			{
				AOETriggerIn(hitTarget);
			}
			PlaySE(HitSEName);
			if (base.SIP.spellIsFall)
			{
				EffectBase.CreateFallingExplosion(currentFrameFallSpellHitPos.IgnoreZ());
			}
		}
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		return new TakeDamageInfo
		{
			canRebound = false
		};
	}

	protected override bool OnAOEHitUnit(UnitProperty unit)
	{
		bool result = base.OnAOEHitUnit(unit);
		ThunderCheck(unit.transform.position);
		return result;
	}

	protected override Vector3 GetSpellFollowToOwnerDirection()
	{
		Vector3 ownerBasePoint = GetOwnerBasePoint();
		if (ownerBasePoint != default(Vector3))
		{
			return Tool2D.IgnoreZPoint(CenterTransform.position - ownerBasePoint).normalized;
		}
		return base.Direction;
	}

	private void UpdateLaserNodesInRotationMovement()
	{
		float num = MathF.PI * base.spellAroundOwnerRadius * base.spellAroundOwnerRadius;
		int num2 = Mathf.Max(20, Mathf.CeilToInt(num * 1.5f));
		float distance = Mathf.Sin(360f / (float)num2 / 2f * (MathF.PI / 180f)) * base.spellAroundOwnerRadius * 2f;
		int num3 = base.penetrateTime;
		Vector3 vector = (base.spellCfg.isSplitSpell ? Tool2D.IgnoreZPoint(splitSpellOwner.CenterTransform.position) : GetOwnerBasePoint());
		hitTargets.Clear();
		bool flag = true;
		bool flag2 = false;
		for (int i = 0; i <= num2; i++)
		{
			Vector3 vector2 = vector + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 360f / (float)num2 * (float)i) * base.spellAroundOwnerRadius;
			Vector3 vector3 = vector + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 360f / (float)num2 * (float)(i + 1)) * base.spellAroundOwnerRadius;
			Vector3 normalized = Tool2D.IgnoreZPoint(vector3 - vector2).normalized;
			int num4 = GeneralTool.RaycastNonAlloc(vector2, normalized, distance, targetRaycastHitBuffer, detectTags, AttackLayer);
			for (int j = 0; j < num4; j++)
			{
				switch (targetRaycastHitBuffer[j].collider.tag)
				{
				case "Monster":
					if (!hitTargets.Contains(targetRaycastHitBuffer[j].collider) || targetRaycastHitBuffer[j].transform.GetComponent<UnitBase>() is Monster17_Hat)
					{
						flag2 = true;
						hitTargets.Add(targetRaycastHitBuffer[j].collider);
						num3--;
						if (num3 < 0)
						{
							flag = false;
							vector3 = targetRaycastHitBuffer[j].point;
						}
					}
					break;
				case "Destructible":
					flag2 = true;
					hitTargets.Add(targetRaycastHitBuffer[j].collider);
					num3--;
					if (num3 < 0)
					{
						flag = false;
						vector3 = targetRaycastHitBuffer[j].point;
					}
					break;
				case "Butterfly":
				case "RollBall":
				case "Brittleness":
					flag2 = true;
					if (!hitTargets.Contains(targetRaycastHitBuffer[j].collider))
					{
						hitTargets.Add(targetRaycastHitBuffer[j].collider);
					}
					break;
				}
				if (!flag)
				{
					break;
				}
			}
			rayNodes.Add(vector3);
			if (!flag)
			{
				break;
			}
		}
		if (!flag2)
		{
			hitTargets.Clear();
		}
	}

	private float GetManaCost()
	{
		return shootBaseManaCost * crystalPowerRatio * base.spellCfg.DPSDamageInterval * currentDamageAndManaCostRatio;
	}

	private void ThunderCheck(Vector3 pos)
	{
		if (UnityEngine.Random.Range(0f, 1f) < base.endTHunderHitChance && thunderAttackTimer > thunderAttackInterval)
		{
			thunderAttackTimer = 0f;
			EndThunderAttackCheck(thunderOnly: false, Tool2D.IgnoreZPoint(pos));
		}
	}

	private void CreateHitEffect(Vector3 pos)
	{
		float value = Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f) * (1f + LaserAttackWidthRatioUpPerSecond * attackTimer * 10f) * base.SIP.SpellVolumeRatio;
		EffectBase.ManualCreateEffect("Hit", position: pos + LaserAttackHeightShift, scale: value);
	}
}
