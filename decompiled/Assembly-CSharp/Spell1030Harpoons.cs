using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spell1030Harpoons : SpellBase
{
	public enum HarpoonsState
	{
		Shooting,
		Holding,
		HookHolding,
		PullingBack
	}

	public float baseRopeLength;

	public float speedToLengthRatio;

	public float durationTolengthRatio;

	private float remainingRopeLength;

	private Vector3 lastFramePosition = Vector3.zero;

	private bool endingPullingBackRope;

	private Vector3 ownerLastFramePosition = Vector3.zero;

	public Spell1030Rope ropeScript;

	public List<Material> ropeMaterial;

	public List<Sprite> tipSprites;

	public List<Sprite> SharpTipSprites;

	public SpriteRenderer HarpoonsTipSpriteRenderer;

	public SpriteRenderer HarpoonsTipShadowSpriteRenderer;

	private float lightningHarpoonsDamageRatio;

	private float lightningHarpoonsDetectRange;

	public GameObject HarpoonsTip;

	public Transform HarpoonsTipShadowTrans;

	private float currentSpellAroundOwnerRadius;

	private float stickDamage;

	private float stickDamageInterval = 0.2f;

	private float stickDamageTimer;

	private float stickDurationLeft;

	private UnitProperty hookingTarget;

	public float hookingTargetMucusSlowRatio;

	public float hookingTargetMucusBaseDuration;

	public float lightningChainActiveInterval;

	private float lightningChainActiveTimer;

	private bool initializeDone;

	public float StickingPullForce;

	public float ApplyPullForceThreshold;

	public float FarDistanceMaxPullForceRatio;

	private static readonly List<UnitProperty> PullForceAppliedTargetsInCurrentFrame = new List<UnitProperty>();

	private static readonly Dictionary<UnitProperty, List<Spell1030Harpoons>> CurrentHitTargets = new Dictionary<UnitProperty, List<Spell1030Harpoons>>();

	public float NormalHarpoonChainWidth;

	public float FuseHarpoonChainWidth;

	public LineRenderer HarpoonRope;

	public LineRenderer HarpoonRopeShadow;

	private float ignoreWallDistance = 1f;

	public SphereCollider Hitbox;

	private Vector3 harpoonsTipShift = Vector3.zero;

	public float ropeLength { get; private set; }

	public HarpoonsState currentState { get; private set; }

	public float ropeTravelDistance { get; private set; }

	public Vector3 initialFallPosition { get; private set; } = Vector3.zero;


	protected override float FallingReboundForce => Mathf.Clamp((0f - base.CurrentUpSpeed) * 0.3f, InFallingReboundingGravity / 3f, 9999f);

	public bool IsFuseHarpoon { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		initializeDone = false;
	}

	public override void InitializeCallback()
	{
		ignoreWallDistance = Hitbox.radius * 2f * base.transform.localScale.x * 1.5f;
		harpoonsTipShift = UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * UnityEngine.Random.Range(0f, 0.2f);
		HarpoonsTip.SetActive(value: true);
		if (base.spellCfg.isSplitSpell)
		{
			ApplyHarpoonsBonusEffect();
		}
	}

	protected override float GetLowFpsSpellSplitCount(float countPower = 1f, float lowFPsThreshold = 40f)
	{
		return base.GetLowFpsSpellSplitCount(3f, 60f);
	}

	protected override void UpdateSizeByDamageAndVolumeRatio()
	{
		if (SpellConfig.dic[base.spellCfg.id].damage != 0f)
		{
			float num = base.spellCfg.damage / base.level1Cfg.damage;
			float num2 = Mathf.Pow(num, 0.1f);
			if (Math.Abs(num - 1f) >= 0.01f)
			{
				base.transform.localScale = Vector3.one * num2 * base.spellVolumeRatio;
			}
			else
			{
				base.transform.localScale = Vector3.one * base.spellVolumeRatio;
			}
		}
	}

	public void PassiveHarpoonsShoot(Vector3 shootDir)
	{
		PlaySE("HarpoonShoot");
		enableFollowTarget = true;
		if (shootDir != default(Vector3))
		{
			base.Direction = shootDir.normalized;
		}
		base.Direction = Tool2D.GetDir(base.Direction, UnityEngine.Random.Range(0f - base._angle, base._angle));
		if (base.SIP.reverseDirection)
		{
			base.Direction *= -1f;
		}
		base.spellCfg.speed = (36f + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
		base.CurrentSpeed = base.spellCfg.speed;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		base.Direction = base.Direction.normalized;
		ApplyHarpoonsBonusEffect();
	}

	private void ApplyHarpoonsBonusEffect()
	{
		ropeLength = baseRopeLength + base.bonusDuration * durationTolengthRatio * base.finalDurationRatio + base.bonusSpeed * speedToLengthRatio * base.speedRatio;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy || base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
		{
			ropeLength *= 1.2f;
		}
		remainingRopeLength = ropeLength;
		endingPullingBackRope = false;
		ApplySpeedToVelocity();
		lastFramePosition = base.transform.position;
		ownerLastFramePosition = GetAroundTargetBasePoint();
		EnterState(HarpoonsState.Shooting);
		triggerIn.colliderObject.enabled = true;
		currentSpellAroundOwnerRadius = 0f;
		lightningHarpoonsDamageRatio = 0f;
		ropeTravelDistance = 0f;
		lightningChainActiveTimer = 0f;
		hookingTarget = null;
		stickDurationLeft = (3f + base.bonusDuration) * base.finalDurationRatio;
		spellMucusTime = Mathf.Max(spellMucusTime, hookingTargetMucusBaseDuration);
		spellMucusMoveSpeedRatio *= hookingTargetMucusSlowRatio;
		if (PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead != null)
		{
			base.spellCfg.damage += SpellConfig.dic[base.spellCfg.id].damage * base.damageRatio * base.finalDamageRatio * (float)PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead.int1.result / 100f;
			base.burnHpRatioPerSeconds += PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead.float1.result;
			base.spellBurnTime = Mathf.Max(2f, base.spellBurnTime);
			if (base.ColorType == SpellColorType.Player)
			{
				base.ColorType = SpellColorType.Fire;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_FrozenHarpoonHead != null)
		{
			spellFrozenTime += PlayerMgr.Inst.ItemCtrller.relic_FrozenHarpoonHead.float1.result;
			if (base.ColorType == SpellColorType.Player)
			{
				base.ColorType = SpellColorType.Frozen;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_PoisonousHarpoonHead != null)
		{
			spellVenomOnceCount += PlayerMgr.Inst.ItemCtrller.relic_PoisonousHarpoonHead.level;
			spellVenomTime = Mathf.Max(3f, spellVenomTime);
			if (base.ColorType == SpellColorType.Player)
			{
				base.ColorType = SpellColorType.Venom;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead != null)
		{
			base.spellCfg.damage += SpellConfig.dic[base.spellCfg.id].damage * base.damageRatio * base.finalDamageRatio * (float)PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead.int1.result / 100f;
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_LightningHarpoonHead != null)
		{
			lightningHarpoonsDamageRatio = PlayerMgr.Inst.ItemCtrller.relic_LightningHarpoonHead.float1.result / 100f;
			lightningHarpoonsDetectRange = PlayerMgr.Inst.ItemCtrller.relic_LightningHarpoonHead.float2.result;
		}
		Material material = null;
		List<Sprite> list = (IsFuseHarpoon ? SharpTipSprites : tipSprites);
		switch (base.ColorType)
		{
		case SpellColorType.Frozen:
			material = ropeMaterial[0];
			HarpoonsTipSpriteRenderer.sprite = list[0];
			HarpoonsTipShadowSpriteRenderer.sprite = list[0];
			break;
		case SpellColorType.Mucus:
			material = ropeMaterial[1];
			HarpoonsTipSpriteRenderer.sprite = list[1];
			HarpoonsTipShadowSpriteRenderer.sprite = list[1];
			break;
		case SpellColorType.Player:
			material = ropeMaterial[2];
			HarpoonsTipSpriteRenderer.sprite = list[2];
			HarpoonsTipShadowSpriteRenderer.sprite = list[2];
			break;
		case SpellColorType.Venom:
			material = ropeMaterial[3];
			HarpoonsTipSpriteRenderer.sprite = list[3];
			HarpoonsTipShadowSpriteRenderer.sprite = list[3];
			break;
		case SpellColorType.Fire:
			material = ropeMaterial[4];
			HarpoonsTipSpriteRenderer.sprite = list[4];
			HarpoonsTipShadowSpriteRenderer.sprite = list[4];
			break;
		case SpellColorType.Thunder:
			material = ropeMaterial[5];
			HarpoonsTipSpriteRenderer.sprite = list[5];
			HarpoonsTipShadowSpriteRenderer.sprite = list[5];
			break;
		case SpellColorType.Void:
			material = ropeMaterial[6];
			HarpoonsTipSpriteRenderer.sprite = list[6];
			HarpoonsTipShadowSpriteRenderer.sprite = list[6];
			break;
		default:
			material = ropeMaterial[2];
			HarpoonsTipSpriteRenderer.sprite = list[2];
			HarpoonsTipShadowSpriteRenderer.sprite = list[2];
			break;
		}
		HarpoonRope.startWidth = (IsFuseHarpoon ? FuseHarpoonChainWidth : NormalHarpoonChainWidth);
		HarpoonRopeShadow.startWidth = (IsFuseHarpoon ? FuseHarpoonChainWidth : NormalHarpoonChainWidth);
		ropeScript.ActiveChain(material, IsFuseHarpoon);
		base.spellCfg.duration = (3f + base.bonusDuration) * base.finalDurationRatio;
		stickDamage = base.spellCfg.damage * stickDamageInterval * base.spellCfg.float3;
		initializeDone = true;
	}

	private void EnterState(HarpoonsState state)
	{
		currentState = state;
		switch (state)
		{
		case HarpoonsState.Holding:
			rigid.linearVelocity = Vector3.zero;
			break;
		case HarpoonsState.PullingBack:
			if ((bool)hookingTarget)
			{
				if (CurrentHitTargets.ContainsKey(hookingTarget))
				{
					if (CurrentHitTargets[hookingTarget].Count == 1)
					{
						hookingTarget.TakeKnockback(Tool2D.IgnoreZV2ToV1(GetAroundTargetBasePoint(), base.transform.position).normalized * base.spellCfg.knockback * 0.5f);
					}
					CurrentHitTargets[hookingTarget].Remove(this);
				}
				OutputDamage(hookingTarget, new TakeDamageInfo
				{
					canRebound = false,
					damage = base.spellCfg.damage * 2f
				});
			}
			UpdateStateEffect();
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		case HarpoonsState.Shooting:
		case HarpoonsState.HookHolding:
			break;
		}
	}

	public override void Update()
	{
		base.Update();
		if (!initializeDone)
		{
			return;
		}
		UpdateStateEffect();
		CalculateRemaingRopeLength();
		if (!base.isFlyFinish)
		{
			return;
		}
		if ((double)Tool2D.IgnoreZDistance(GetAroundTargetBasePoint(), base.transform.position) < 0.1 + (double)(base.CurrentSpeed * Time.deltaTime))
		{
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
			base.transform.position = GetAroundTargetBasePoint();
			HarpoonsTip.SetActive(value: false);
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	private void UpdateStateEffect()
	{
		switch (currentState)
		{
		case HarpoonsState.Holding:
			endingPullingBackRope = true;
			EnterState(HarpoonsState.PullingBack);
			break;
		case HarpoonsState.HookHolding:
			base.Direction = Vector3.zero;
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
			if (hookingTarget != null)
			{
				float num = Tool2D.IgnoreZDistance(base.transform.position, GetAroundTargetBasePoint());
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					num = Tool2D.IgnoreZDistance(hookingTarget.transform.position, GetAroundTargetBasePoint()) * 2f;
				}
				if (!hookingTarget.isUnitDead && hookingTarget.gameObject.activeInHierarchy && !(num >= ropeLength) && hookingTarget.CanBeTarget && !hookingTarget.IsInvincible)
				{
					UnitBase unitBas = hookingTarget.UnitBas;
					if (!(unitBas is Monster17) && !(unitBas is Monster17_Hat))
					{
						base.transform.position = hookingTarget.transform.position.IgnoreZ() + new Vector3(0f, 0f, -0.3f) + harpoonsTipShift - (base.transform.position - GetAroundTargetBasePoint()).normalized * base.transform.localScale.x / 5f;
						break;
					}
				}
				EnterState(HarpoonsState.Holding);
			}
			else
			{
				Debug.Log("û\ufffd\ufffdĿ\ufffd\ufffd \ufffdջ\ufffd");
				EnterState(HarpoonsState.Holding);
			}
			break;
		case HarpoonsState.PullingBack:
			if (base.endThunderHitPercent > 0f)
			{
				EndThunderAttackCheck();
				base.endThunderHitPercent = 0f;
			}
			if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				base.Direction = Tool2D.IgnoreZV2ToV1(GetAroundTargetBasePoint(), base.transform.position).normalized;
				rigid.linearVelocity = base.Direction * base.CurrentSpeed;
			}
			if ((double)Tool2D.IgnoreZDistance(GetAroundTargetBasePoint(), base.transform.position) < 0.3 + (double)(base.CurrentSpeed * Time.deltaTime))
			{
				base.isFlyFinish = true;
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					rigid.linearVelocity = Vector3.zero;
					base.CurrentSpeed = 0f;
					base.transform.position = GetAroundTargetBasePoint();
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case HarpoonsState.Shooting:
			break;
		}
	}

	private void HoldingTipPullTarget()
	{
		if (currentState == HarpoonsState.HookHolding && !(hookingTarget == null) && hookingTarget.gameObject.activeInHierarchy && hookingTarget.CanBeTarget && !PullForceAppliedTargetsInCurrentFrame.Contains(hookingTarget) && !(Tool2D.IgnoreZDistance(hookingTarget.transform.position, GetAroundTargetBasePoint()) < ropeLength * ApplyPullForceThreshold) && !(hookingTarget.unitCfg.knockbackRatio <= 0f))
		{
			float num = Mathf.Clamp(Tool2D.IgnoreZDistance(hookingTarget.transform.position, GetAroundTargetBasePoint()) / ropeLength, 0f, 1f);
			PullForceAppliedTargetsInCurrentFrame.Add(hookingTarget);
			hookingTarget.TakeKnockback(Tool2D.IgnoreZV2ToV1(GetAroundTargetBasePoint(), base.transform.position).normalized * StickingPullForce / hookingTarget.unitCfg.knockbackRatio * Time.deltaTime * (1f + num) * FarDistanceMaxPullForceRatio);
		}
	}

	private void HoldingTipAttack()
	{
		if (currentState != HarpoonsState.HookHolding)
		{
			return;
		}
		stickDurationLeft -= Time.deltaTime * GetLowFPSTimeScale();
		lightningChainActiveTimer += Time.deltaTime;
		if (stickDurationLeft <= 0f)
		{
			stickDamageTimer = stickDamageInterval;
		}
		stickDamageTimer += Time.deltaTime * GetLowFPSTimeScale();
		if (!(stickDamageTimer < stickDamageInterval))
		{
			int num = Mathf.FloorToInt(stickDamageTimer / stickDamageInterval);
			stickDamageTimer -= stickDamageInterval * (float)num;
			if (lightningHarpoonsDamageRatio > 0f && lightningChainActiveTimer >= lightningChainActiveInterval)
			{
				lightningChainActiveTimer -= lightningChainActiveInterval;
				ApplyLightningChainToTarget(hookingTarget);
			}
			if (spellVenomTime > 0f)
			{
				hookingTarget.SetVenom(spellVenomTime, spellVenomOnceCount * (float)(num - 1));
			}
			OutputDamage(hookingTarget, new TakeDamageInfo
			{
				canRebound = false,
				damage = stickDamage * (float)num
			});
			float num2 = (GeneralTool.IsLowFpsOptimizeActive(60f) ? Mathf.Pow(GameMgr.Inst.GetFps() / 60f, 5f) : 1f);
			if (UnityEngine.Random.Range(0f, 1f) <= num2)
			{
				SpellEffectBase effectBase = EffectBase;
				Vector3? position = hookingTarget.transform.position + new Vector3(0f, 0.3f, -0.3f);
				effectBase.ManualCreateEffect("HoldHit", null, position);
			}
			if (stickDurationLeft <= 0f)
			{
				EnterState(HarpoonsState.Holding);
			}
			SEMgr inst = SEMgr.Inst;
			int abilityType = (int)base.spellCfg.abilityType;
			inst.PlaySE("SE_Spell" + abilityType + "loopHit", SEPlayMode.Replay, 5);
		}
	}

	protected override void SpellFollowMouse()
	{
		if (Tool2D.IgnoreZDistance(PlayerMgr.Inst.GetMousePoint(base.transform.position.z), base.transform.position) <= 0.3f)
		{
			rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, Vector3.zero, base.CurrentSpeed / 2f * Time.deltaTime);
		}
		else
		{
			base.SpellFollowMouse();
		}
	}

	public override void SpellAroundPlayer()
	{
		base.SpellAroundPlayer();
		float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime * 0.7f;
		base.spellAroundOwnerCurrentAngle += num;
		switch (currentState)
		{
		case HarpoonsState.Shooting:
		case HarpoonsState.Holding:
		case HarpoonsState.HookHolding:
			currentSpellAroundOwnerRadius = Mathf.Clamp(currentSpellAroundOwnerRadius + base.CurrentSpeed / 10f * Time.deltaTime, 0f, base.spellAroundOwnerRadius);
			break;
		case HarpoonsState.PullingBack:
			currentSpellAroundOwnerRadius = Mathf.Clamp(currentSpellAroundOwnerRadius - base.CurrentSpeed / 5f * Time.deltaTime, 0f, base.spellAroundOwnerRadius);
			if (currentSpellAroundOwnerRadius <= 0.6f)
			{
				base.isFlyFinish = true;
				currentSpellAroundOwnerRadius = 0f;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
		Vector3 v = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * currentSpellAroundOwnerRadius;
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		SpellAroundPlayerUpdateMoveTrigger(num);
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (PullForceAppliedTargetsInCurrentFrame.Count > 0)
		{
			PullForceAppliedTargetsInCurrentFrame.Clear();
		}
		if (initializeDone)
		{
			UpdateHarpoonsTimer();
			UpdateHarpoonsTipAngle();
			HoldingTipAttack();
			HoldingTipPullTarget();
			TryRemoveHookingTargetList();
		}
	}

	private void UpdateHarpoonsTimer()
	{
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer >= base.spellCfg.duration && currentState == HarpoonsState.Shooting)
		{
			EnterState(HarpoonsState.Holding);
		}
	}

	private void UpdateHarpoonsTipAngle()
	{
		Vector3 zero = Vector3.zero;
		switch (currentState)
		{
		case HarpoonsState.Shooting:
		case HarpoonsState.Holding:
			zero = new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed);
			break;
		case HarpoonsState.HookHolding:
			zero = (base.transform.position - GetAroundTargetBasePoint()).normalized;
			break;
		case HarpoonsState.PullingBack:
			zero = ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? ((Vector3)new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed)) : (base.transform.position - GetAroundTargetBasePoint()).normalized);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		float z = Vector2.SignedAngle(Vector2.right, (Vector2)zero);
		Quaternion rotation = Quaternion.Euler(0f, 0f, z);
		tsf_Layer.rotation = rotation;
		HarpoonsTipShadowTrans.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, 1.05f);
		HarpoonsTip.transform.localPosition = HarpoonsTip.transform.localPosition.IgnoreZ() - new Vector3(0f, 0f, tsf_Layer.position.z);
	}

	private void CalculateRemaingRopeLength()
	{
		if (!endingPullingBackRope && currentState != HarpoonsState.HookHolding)
		{
			Vector3 aroundTargetBasePoint = GetAroundTargetBasePoint();
			float num = Tool2D.IgnoreZDistance(lastFramePosition, base.transform.position);
			float num2 = Tool2D.IgnoreZDistance(ownerLastFramePosition, aroundTargetBasePoint);
			remainingRopeLength -= num + num2;
			ropeTravelDistance += num + num2;
			lastFramePosition = base.transform.position;
			ownerLastFramePosition = aroundTargetBasePoint;
			if (remainingRopeLength <= 0f && base.currentSpellMovement != SpellSpecialMovementType.Rotation && !hookingTarget)
			{
				EnterState(HarpoonsState.Holding);
			}
		}
	}

	public override Vector3 GetAroundTargetBasePoint()
	{
		if (base.OwnerSpell != null)
		{
			return base.OwnerPoint;
		}
		if (ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			return base.shooterWand.GetShootPosition();
		}
		return base.GetAroundTargetBasePoint();
	}

	public override void TryRefractOrPenetrateOrRecycleOnHitTarget(params GameObject[] hits)
	{
		if (!TryRefractOrPenetrate(hits))
		{
			StartHoldingOrPullingBack(isWall: false);
		}
	}

	protected override void OnHitWallAndSolidObj(Collider col)
	{
		if (!(ropeTravelDistance <= ignoreWallDistance) && !isThroughWall && base.currentSpellMovement != SpellSpecialMovementType.Rotation && base.rebounceTime <= 0)
		{
			StartHoldingOrPullingBack(isWall: true);
		}
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		MakeDamageToUnit(unit);
		CreateHitEffect(unit.transform.position + new Vector3(0f, 0.3f, -0.3f));
		TryRefractOrPenetrateOrRecycleOnHitTarget(unit.gameObject);
		return true;
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		SpellEffectBase effectBase = EffectBase;
		Vector3? position2 = position.Value;
		effectBase.ManualCreateEffect("DirectHit", null, position2);
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			EnterState(HarpoonsState.Holding);
		}
	}

	protected override void HeightFixedUpdate()
	{
		if (base.CurrentUpSpeed != 0f)
		{
			float height = base.Height;
			base.Height += base.CurrentUpSpeed * Time.deltaTime;
			if (base.Height < 0f)
			{
				base.Height = 0f;
			}
			if (base.Height <= 0f && height > 0f && base.SIP.spellIsFall)
			{
				OnFallingGround();
			}
		}
	}

	protected new virtual void OnFallingGround()
	{
		EffectBase.PlayFallingGroundSound();
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}

	protected override void UpdateUpSpeedWithFalling()
	{
	}

	private void TryRemoveHookingTargetList()
	{
		if ((bool)hookingTarget && (!hookingTarget.gameObject.activeInHierarchy || hookingTarget.isUnitDead) && CurrentHitTargets.ContainsKey(hookingTarget))
		{
			CurrentHitTargets.Remove(hookingTarget);
		}
	}

	public override void PoolRecycle()
	{
		TryRemoveHookingTargetList();
		base.PoolRecycle();
	}

	private void StartHoldingOrPullingBack(bool isWall)
	{
		if (currentState == HarpoonsState.Shooting)
		{
			if (isWall)
			{
				EnterState(HarpoonsState.Holding);
			}
			else
			{
				EnterState(HarpoonsState.HookHolding);
			}
			sc_Rebound.enabled = false;
			triggerIn.colliderObject.enabled = false;
		}
	}

	private void ApplyLightningChainToTarget(UnitProperty targetAttachPpt)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 10301 + "LightningChain", base.transform.position, quaternion.identity).GetComponent<Spell1030HarpoonsLightingchain>().LightningChainDataIniatialize(ownerPpt, targetAttachPpt, base.spellCfg.damage, lightningHarpoonsDetectRange, lightningHarpoonsDamageRatio, base.shooterWand, this);
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		refractedTargets.Add(unit.gameObject);
		TakeDamageInfo result = OutputDamage(unit, new TakeDamageInfo
		{
			canRebound = false,
			criticalChance = base.overalCriticalChance
		});
		if (lightningHarpoonsDamageRatio > 0f)
		{
			ApplyLightningChainToTarget(unit);
		}
		if (base.penetrateTime <= 0)
		{
			hookingTarget = unit;
			CurrentHitTargets.TryAdd(hookingTarget, new List<Spell1030Harpoons>());
			if (!CurrentHitTargets[hookingTarget].Contains(this))
			{
				CurrentHitTargets[hookingTarget].Add(this);
			}
		}
		return result;
	}
}
