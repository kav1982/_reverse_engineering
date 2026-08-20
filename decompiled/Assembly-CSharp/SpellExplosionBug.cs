using System;
using System.Collections.Generic;
using SpriteEffectSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class SpellExplosionBug : MonoBehaviour
{
	private enum ExplosionBugState
	{
		BornIdle,
		Idle,
		RunToTarget,
		ReadyToExplode,
		Suicide
	}

	public SpriteRenderer sprite;

	public VariableFloat idleTime;

	public float recheckInterval;

	public float lifetime;

	private float realLifeTime;

	private float explosionDamage;

	private float damageDetectRadiu;

	public SpriteRenderer sr;

	public SpriteRenderer srFire;

	public SpriteRenderer PropellerSrFire;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public Material mat_ECThunder;

	private float recheckPositionTimer;

	public SpriteEffectAnima ExplosionAnima;

	private ExplosionBugState state;

	private float idleTimer;

	private float recheckIntervalTimer;

	private float targetRecheckTimer;

	public float targetRecheckInterval;

	private float lifetimeTimer;

	private float effectrRadiuscale = 1f;

	private float effectFinalRadiuRatio = 1f;

	private float frozenTime;

	private float burnDamage;

	private float burnTime;

	private float mucusMoveSpeedEffect = 1f;

	private float mucusSpellSpeedEffect = 1f;

	private float mucusTime;

	private float venomTime;

	private float venomApplyCount;

	private float explodeCriticalChance;

	private WandPostSlotChargeData chargeData;

	private float knockBackRatio = 1f;

	private float finalknockBackRatio = 1f;

	private float criticalDamageRatio;

	private float criticalRange;

	private float criticalTargetsCount;

	private float criticalDragForce;

	private SpellColorType colorType;

	private Spell3129VoidExplosion.VoidExplosionData explosionData;

	private UnitProperty targetPpt;

	public float moveSpeed;

	private float lastRefindPathTime;

	private NavMeshPath navPath;

	private int spawnWormCount;

	private SpellBase SummonSpellBase;

	public float explosionBugHpRatio;

	private float bugHp;

	public float BugBaseFloatingHeight;

	public AnimationCurve GeneralFlyHeightCurve;

	private float currentFlyTimer;

	public float GeneralFlyFrequency;

	public float GeneralFlyStrength;

	public float BurnIdleDuration;

	private float burnIdleTimer;

	public VariableFloat InitialPushPowerRange;

	public float PushPowerlerpSpeed;

	private float currentPushPower;

	private Vector3 pushPowerDir;

	public Transform BodyTransform;

	private Vector3 moveVector = Vector3.zero;

	private SpellSpecialMovementType movementType;

	private float currentRotateAngle;

	private float targetRotateRadius;

	private float currentRotateRadius;

	private Vector3 spawnerPosition = Vector3.zero;

	private float currentSpeed;

	public float rotationRecheckTargetInterval;

	public VariableFloat SuicideInitialUpSpeed;

	private float currentUpSpeed;

	public float ReboundUpSpeed;

	public float FallSpeed;

	private int reboundTime;

	private float endThunderHitRadiu;

	private float endThunderDamageRatio = 1f;

	private bool isEndThunderAttack;

	private float radiuRatio = 1f;

	private static readonly Collider[] thunderColliderBuffer = new Collider[256];

	private static readonly HashSet<string> thunderColliderTags = new HashSet<string> { "Monster", "Destructible", "Butterfly", "RollBall", "Brittleness" };

	private float recheckTargetTimer;

	public Transform BugPropellerTrans;

	public SpriteRenderer BugPropellerSprite;

	public float PropellerRotateSpeed;

	private static float wormStackCounter = 0f;

	public float LowFrameBugMaxStackCount;

	private float wormCountDamageRatio = 1f;

	public Sprite NormalSr;

	public Sprite VoidSr;

	private void OnEnable()
	{
		idleTimer = 0f;
		recheckIntervalTimer = 0f;
		lifetimeTimer = 0f;
		state = ExplosionBugState.BornIdle;
		frozenTime = 0f;
		mucusMoveSpeedEffect = 1f;
		mucusSpellSpeedEffect = 1f;
		mucusTime = 0f;
		venomTime = 0f;
		venomApplyCount = 0f;
		recheckPositionTimer = 0f;
		chargeData = null;
		explodeCriticalChance = 0f;
		burnDamage = 0f;
		burnTime = 0f;
		spawnWormCount = 0;
		SummonSpellBase = null;
		bugHp = 0f;
		movementType = SpellSpecialMovementType.Normal;
		spawnerPosition = Vector3.zero;
		currentFlyTimer = UnityEngine.Random.Range(0f, 1f);
		burnIdleTimer = 0f;
		currentPushPower = 0f;
		currentRotateAngle = UnityEngine.Random.Range(0f, 360f);
		targetRotateRadius = 0f;
		currentRotateRadius = 0f;
		currentSpeed = 0f;
		targetRecheckTimer = 0f;
		float value = UnityEngine.Random.Range(-100f, 100f);
		sr.material.SetFloat("_TimeOffset", value);
		srFire.material.SetFloat("_TimeOffset", value);
		navPath = null;
	}

	private float GetLowFrameActSpeed()
	{
		return SummonSpellBase.GetLowFPSTimeScale(10f);
	}

	private void Update()
	{
		lifetimeTimer += Time.deltaTime * GetLowFrameActSpeed();
		if (lifetimeTimer >= realLifeTime && state != ExplosionBugState.ReadyToExplode)
		{
			Enterstate(ExplosionBugState.ReadyToExplode);
		}
		targetRecheckTimer += Time.deltaTime;
		if (targetRecheckTimer >= targetRecheckInterval)
		{
			targetRecheckTimer -= targetRecheckInterval;
			targetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.transform.position);
		}
		if (movementType == SpellSpecialMovementType.Rotation)
		{
			UpdateRotationMovePos();
		}
		else
		{
			UpdateMovePos();
		}
		UpdatePushPowerSpeed();
		recheckIntervalTimer += Time.deltaTime;
		if (movementType == SpellSpecialMovementType.Rotation && recheckIntervalTimer >= rotationRecheckTargetInterval)
		{
			recheckIntervalTimer = 0f;
			RefindTarget();
		}
		if (targetPpt == null || !targetPpt.CanBeTarget || targetPpt.gameObject.activeInHierarchy)
		{
			targetPpt = null;
			recheckTargetTimer += Time.deltaTime;
			if (recheckIntervalTimer >= 0.1f)
			{
				recheckIntervalTimer = 0f;
				RefindTarget();
			}
		}
		StateUpdate();
		UpdateCurrentHeight();
		UpdatePropellerAngle();
		currentFlyTimer += Time.deltaTime * GeneralFlyFrequency;
	}

	private void UpdatePropellerAngle()
	{
		float num = BugPropellerTrans.localEulerAngles.z + PropellerRotateSpeed * Time.deltaTime;
		if (num >= 360f)
		{
			num -= 360f;
		}
		BugPropellerTrans.localEulerAngles = BugPropellerTrans.localEulerAngles.IgnoreZ() + new Vector3(0f, 0f, num);
	}

	private void UpdateMovePos()
	{
		float currentFlySpeed = GetCurrentFlySpeed();
		Vector3 normalized = moveVector.normalized;
		Vector3 vector = moveVector;
		switch (movementType)
		{
		case SpellSpecialMovementType.Normal:
			vector = Vector3.Lerp(b: ((!(targetPpt != null)) ? Vector3.zero : ToPointDir(targetPpt.transform.position)) * currentFlySpeed, a: moveVector, t: 5f * Time.deltaTime);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			if (targetPpt != null)
			{
				normalized = Tool2D.DirMoveTowards(normalized, ToPointDir(targetPpt.transform.position), currentFlySpeed * 15f * Time.deltaTime);
				vector = normalized * currentFlySpeed;
			}
			else
			{
				targetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetablePpt(base.transform.position, normalized);
				normalized = Vector3.Lerp(normalized, Vector3.zero, 5f * Time.deltaTime);
				vector = normalized * currentFlySpeed;
			}
			break;
		case SpellSpecialMovementType.ChaseMouse:
			normalized = ToPointDir(PlayerMgr.Inst.GetMousePoint(base.transform.position.z));
			vector = Vector3.Lerp(moveVector, normalized * currentFlySpeed, currentFlySpeed * Time.deltaTime * 2f);
			break;
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3 normalized2 = moveVector.normalized;
			Vector3 vector2 = ToPointDir(spawnerPosition);
			float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(normalized2, vector2)) - 90f) / 90f;
			Vector3 vector3 = Tool2D.DirMoveTowardsTargetInCounterClockWise(normalized2, vector2, currentFlySpeed * 15f * Time.fixedDeltaTime);
			float num = 0.4f;
			vector = vector3 * currentFlySpeed * Mathf.Lerp(1f - num, 1f + num, t);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		case SpellSpecialMovementType.Rotation:
			break;
		}
		sr.transform.localScale = ((vector.x > 0f) ? (Vector3.one * sr.transform.localScale.y) : (new Vector3(-1f, 1f, 1f) * sr.transform.localScale.y));
		moveVector = vector;
		base.transform.position += moveVector * Time.deltaTime;
	}

	private void UpdateRotationMovePos()
	{
		ExplosionBugState explosionBugState = state;
		if (explosionBugState != ExplosionBugState.ReadyToExplode && explosionBugState != ExplosionBugState.Suicide)
		{
			float num = 360f / (MathF.PI * 2f * currentRotateAngle / GetCurrentFlySpeed()) * Time.deltaTime;
			currentRotateAngle += num * 57.29578f;
			Vector3 v = spawnerPosition + Tool2D.GetDir(currentRotateAngle) * currentRotateRadius;
			sr.transform.localScale = ((v.x >= base.transform.position.x) ? (Vector3.one * sr.transform.localScale.y) : (new Vector3(-1f, 1f, 1f) * sr.transform.localScale.y));
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			if (currentRotateRadius < targetRotateRadius - 0.01f)
			{
				currentRotateRadius += GetCurrentFlySpeed() * 0.7f * Time.deltaTime;
				currentRotateRadius = Mathf.Clamp(currentRotateRadius, 0f, targetRotateRadius);
			}
		}
	}

	private void DealDamageInRange(float damageRatio)
	{
		float damage = explosionDamage * damageRatio;
		foreach (Collider item in GeneralTool.GetCollidersByTag(base.transform.position, damageDetectRadiu, "Monster", "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness"))
		{
			if (item.gameObject.CompareAnyTag("Spell", "RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1002RollBall spell1002RollBall))
				{
					if (componentInParent is Spell1003Butterfly spell1003Butterfly)
					{
						spell1003Butterfly.HitEFAndRecycle();
					}
				}
				else
				{
					spell1002RollBall.TakeDamage(damage);
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster"))
			{
				UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
				CheckDamageEffect(component);
			}
			else
			{
				UnitProperty component2 = item.gameObject.GetComponent<UnitProperty>();
				CheckDamageEffect(component2);
			}
		}
	}

	private void CheckDamageEffect(UnitProperty targetUnitProperty)
	{
		if (!(targetUnitProperty != null))
		{
			return;
		}
		if (explosionData != null)
		{
			targetUnitProperty.SetVoid(explosionData);
		}
		TakeDamageInfo takeDamageInfo = targetUnitProperty.TakeDamage(explosionDamage, AttackerType.NothingSpecial, new TakeDamageInfo
		{
			canRebound = false,
			criticalChance = explodeCriticalChance
		});
		if (!takeDamageInfo.immuneDamage)
		{
			ApplyElementEffectToTarget(targetUnitProperty);
			if (takeDamageInfo.isCriticalDamage && criticalDamageRatio > 0f)
			{
				ApplyPullEffect(targetUnitProperty);
			}
		}
		if (targetUnitProperty.unitCfg.unitType == UnitType.Monster)
		{
			if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.SpellHit)
			{
				chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
			}
			else if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.KillEnemy && takeDamageInfo.isTargetDead)
			{
				chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
			}
			else if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.CriticalHit && takeDamageInfo.isCriticalDamage)
			{
				chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
			}
		}
	}

	public Vector3 ToPointDir(Vector3 point)
	{
		return Tool2D.IgnoreZV2ToV1Normal(point, base.transform.position);
	}

	private void UpdatePushPowerSpeed()
	{
		currentPushPower = Mathf.Lerp(currentPushPower, 0f, PushPowerlerpSpeed * Time.deltaTime);
	}

	private float GetCurrentFlySpeed()
	{
		return currentSpeed * GetLowFrameActSpeed() + currentPushPower;
	}

	private void StateUpdate()
	{
		switch (state)
		{
		case ExplosionBugState.BornIdle:
			burnIdleTimer += Time.deltaTime;
			if (burnIdleTimer >= BurnIdleDuration)
			{
				state = ExplosionBugState.Idle;
			}
			break;
		case ExplosionBugState.Idle:
			if (lifetimeTimer >= realLifeTime)
			{
				Enterstate(ExplosionBugState.RunToTarget);
				break;
			}
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				RefindTarget();
				if ((bool)targetPpt)
				{
					Enterstate(ExplosionBugState.RunToTarget);
				}
				else
				{
					Enterstate(ExplosionBugState.Idle);
				}
			}
			break;
		case ExplosionBugState.RunToTarget:
			if (targetPpt == null)
			{
				RefindTarget();
			}
			if (targetPpt == null && state == ExplosionBugState.RunToTarget)
			{
				Enterstate(ExplosionBugState.Idle);
				idleTime.RandomResult();
				break;
			}
			if (navPath == null && (bool)targetPpt)
			{
				RefindPath(targetPpt.transform.position);
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval && (bool)targetPpt)
			{
				recheckIntervalTimer = 0f;
				RefindPath(targetPpt.transform.position.IgnoreZ());
			}
			if (IsReadyToSuicide())
			{
				Enterstate(ExplosionBugState.ReadyToExplode);
			}
			break;
		case ExplosionBugState.ReadyToExplode:
			if (targetPpt == null)
			{
				RefindTarget();
			}
			if (navPath == null && (bool)targetPpt)
			{
				RefindPath(targetPpt.transform.position);
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval && (bool)targetPpt)
			{
				recheckIntervalTimer = 0f;
				RefindPath(targetPpt.transform.position.IgnoreZ());
			}
			if (BodyTransform.localPosition.y < 0.2f)
			{
				if (reboundTime > 0)
				{
					currentUpSpeed = ReboundUpSpeed;
					BodyTransform.localPosition = new Vector3(0f, 0.2f, 0f);
					float num = 1f;
					int num2 = 1;
					if (GeneralTool.IsLowFpsOptimizeActive(40f))
					{
						num2 = Mathf.Min(Mathf.CeilToInt(40f / GameMgr.Inst.GetFps()), reboundTime);
						num *= (float)num2;
					}
					reboundTime -= num2;
					SuicideExplosion(deathExplosion: false, num);
				}
				else
				{
					state = ExplosionBugState.Suicide;
					SuicideExplosion(deathExplosion: true);
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
			}
			else
			{
				recheckPositionTimer += Time.deltaTime;
				if (recheckPositionTimer >= Time.deltaTime && lifetimeTimer < realLifeTime && (bool)targetPpt)
				{
					recheckPositionTimer = 0f;
					RefindPath(targetPpt.transform.position);
				}
			}
			break;
		case ExplosionBugState.Suicide:
			break;
		}
	}

	private bool IsReadyToSuicide()
	{
		switch (movementType)
		{
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.Rotation:
		case SpellSpecialMovementType.ChaseOwner:
		{
			float num2 = targetPpt.UnitBas.GetBodyColliderRadius() + damageDetectRadiu / 2f;
			return Tool2D.IgnoreZDistanceSqr(targetPpt.transform.position, base.transform.position) < num2 * num2;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			float num = targetPpt.UnitBas.GetBodyColliderRadius() + damageDetectRadiu / 2f;
			return Tool2D.IgnoreZDistanceSqr(PlayerMgr.Inst.GetMousePoint(base.transform.position.z), base.transform.position) < num * num;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void Enterstate(ExplosionBugState newState)
	{
		state = newState;
		switch (newState)
		{
		case ExplosionBugState.BornIdle:
			BodyTransform.localPosition = new Vector3(0f, 0.2f, 0f);
			pushPowerDir = UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized;
			currentPushPower = InitialPushPowerRange.RandomResult();
			moveVector = pushPowerDir * GetCurrentFlySpeed();
			sr.transform.localScale = ((moveVector.x > 0f) ? (Vector3.one * sr.transform.localScale.y) : (new Vector3(-1f, 1f, 1f) * sr.transform.localScale.y));
			break;
		case ExplosionBugState.ReadyToExplode:
			currentUpSpeed = SuicideInitialUpSpeed.RandomResult();
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		case ExplosionBugState.Idle:
		case ExplosionBugState.RunToTarget:
		case ExplosionBugState.Suicide:
			break;
		}
	}

	private void UpdateCurrentHeight()
	{
		switch (state)
		{
		case ExplosionBugState.BornIdle:
		{
			float y2 = Mathf.Lerp(BodyTransform.localPosition.y, BugBaseFloatingHeight + GeneralFlyHeightCurve.Evaluate(currentFlyTimer % 1f) * GeneralFlyStrength, 10f * Time.deltaTime);
			BodyTransform.localPosition = new Vector3(0f, y2, 0f);
			break;
		}
		case ExplosionBugState.Idle:
		case ExplosionBugState.RunToTarget:
			BodyTransform.localPosition = new Vector3(0f, BugBaseFloatingHeight + GeneralFlyHeightCurve.Evaluate(currentFlyTimer % 1f) * GeneralFlyStrength, 0f);
			break;
		case ExplosionBugState.ReadyToExplode:
		{
			currentUpSpeed -= FallSpeed * Time.deltaTime;
			float y = Mathf.Max(BodyTransform.localPosition.y + currentUpSpeed, -0.05f);
			BodyTransform.localPosition = new Vector3(0f, y, 0f);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		case ExplosionBugState.Suicide:
			break;
		}
	}

	private void SuicideExplosion(bool deathExplosion, float explosionExtraFinalDamageRatio = 1f)
	{
		if (GeneralTool.IsLowFpsOptimizeActive(40f))
		{
			float num = Mathf.Min(LowFrameBugMaxStackCount, 40f / (GameMgr.Inst.GetFps() + 1f) * 5f);
			if (!(wormStackCounter + wormCountDamageRatio >= num))
			{
				wormStackCounter += wormCountDamageRatio;
				return;
			}
			wormStackCounter -= num;
			explosionExtraFinalDamageRatio *= num;
			explosionDamage *= num;
		}
		CreateExplosionEffect();
		DealDamageInRange(explosionExtraFinalDamageRatio);
		SEMgr.Inst.teammate7Explosion.PlaySE(SEPlayMode.Replay, 3, 0.12f);
		if (explosionData != null && deathExplosion)
		{
			GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31291, base.transform.position, 2.5f);
			gO.transform.localScale = Vector3.one;
			gO.GetComponent<Spell3129VoidExplosion>().DataInitialize(bugHp, explosionData);
		}
		if (SummonSpellBase.SpellSummonDeathExplodeRange > 0f && deathExplosion)
		{
			GetSummonExplodePrefabPathByColorType getSummonExplodePrefabPathByColorType = new GetSummonExplodePrefabPathByColorType();
			GameObject gO2 = ObjPoolMgr.Inst.GetGO(getSummonExplodePrefabPathByColorType.Get(colorType), base.transform.position, 1f);
			float spellEnhancedSize = GeneralTool.GetSpellEnhancedSize(SummonSpellBase.SpellSummonDeathExplodeRange, SummonSpellBase);
			gO2.transform.localScale = Vector3.one * spellEnhancedSize;
			SEMgr.Inst.relic_SummonsExplode.PlaySE().pitch = UnityEngine.Random.Range(0.7f, 1.3f);
			float damage = GeneralTool.GetSpellEnhancedDamage(bugHp, SummonSpellBase) * SummonSpellBase.SpellSummonDeathExplodeHpDamageRatio * explosionExtraFinalDamageRatio;
			List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, spellEnhancedSize, "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness");
			for (int i = 0; i < collidersByTag.Count; i++)
			{
				if (collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
				{
					SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
					if (!componentInParent.IsSameCamp(UnitType.Player))
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
					continue;
				}
				UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
				if (component != null && component.gameObject.activeSelf)
				{
					TakeDamageInfo info = new TakeDamageInfo
					{
						canRebound = false,
						attackerType = AttackerType.NothingSpecial,
						attackerPpt = PlayerMgr.Inst.PlayerPpt,
						damage = damage
					};
					SummonSpellBase.OutputDamage(component, info, SpellAbilityType.TeammateSacrifice);
				}
			}
		}
		DeathThunderAttack();
	}

	private void RefindTarget()
	{
		targetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.transform.position);
	}

	private void RefindPath(Vector3 target)
	{
		navPath = Tool2D.GetNavMeshPath(base.transform.position, target);
	}

	public void ApplySpellEffect(SpellBase spell, float SummonMaxHp, float spawnCountFinalDamageRatio)
	{
		if (spell != null)
		{
			wormCountDamageRatio = spawnCountFinalDamageRatio;
			explosionDamage = (float)Mathf.CeilToInt(spell.spellCfg.float1 * spell.damageRatio * spell.finalDamageRatio * spawnCountFinalDamageRatio) + spell.SIP.finalDamageExtra;
			damageDetectRadiu = (spell.spellCfg.float3 + spell.SIP.fallExplosionRadius) * spell.radiusRatio * spell.finalRadiusRatio;
			effectrRadiuscale = spell.radiusRatio;
			effectFinalRadiuRatio = spell.finalRadiusRatio;
			realLifeTime = (lifetime + spell.bonusDuration) * spell.finalDurationRatio;
			chargeData = spell.wandChargeData;
			explodeCriticalChance = spell.GetCriticalChance();
			state = ExplosionBugState.BornIdle;
			knockBackRatio = spell.knockbackRatio;
			finalknockBackRatio = spell.finalKnockbackRatio;
			frozenTime += spell.spellFrozenTime;
			mucusMoveSpeedEffect = spell.spellMucusMoveSpeedRatio;
			mucusSpellSpeedEffect = spell.spellMucusSpellSpeedRatio;
			mucusTime += spell.spellMucusTime;
			venomTime += spell.spellVenomTime;
			venomApplyCount = spell.spellVenomOnceCount;
			burnDamage = spell.burnHpRatioPerSeconds;
			burnTime = spell.spellBurnTime;
			criticalDamageRatio = spell.criticalDragDamagePercent;
			criticalRange = spell.criticalDragEffectRadiu;
			criticalTargetsCount = spell.criticalDragApllyToCount;
			criticalDragForce = spell.criticalDragPullForce;
			colorType = spell.ColorType;
			base.transform.localScale = Vector3.one * spell.spellVolumeRatio;
			explosionData = spell.voidExplosionInfo;
			spawnWormCount = spell.SpellSummonAfterDeadSpawnWormCount;
			SummonSpellBase = spell;
			movementType = spell.currentSpellMovement;
			bugHp = Mathf.CeilToInt(SummonMaxHp * explosionBugHpRatio);
			targetRotateRadius = spell.spellAroundOwnerRadius;
			spawnerPosition = spell.transform.position;
			currentSpeed = (moveSpeed + spell.bonusSpeed) * spell.speedRatio * spell.finalSpeedRatio * spell.SpellSummonMoveRatio;
			reboundTime = spell.rebounceTime;
			radiuRatio = spell.radiusRatio * spell.finalRadiusRatio;
			endThunderDamageRatio = spell.endThunderHitPercent;
			endThunderHitRadiu = spell.endThunderHitRadiu;
			isEndThunderAttack = UnityEngine.Random.Range(0f, 1f) < spell.endTHunderHitChance;
			srFire.gameObject.SetActive(value: false);
			PropellerSrFire.gameObject.SetActive(value: false);
			sr.sprite = ((spell.ColorType == SpellColorType.Void) ? VoidSr : NormalSr);
			switch (spell.ColorType)
			{
			case SpellColorType.Frozen:
				if (sr.material != mat_ECFrozen)
				{
					sr.material = mat_ECFrozen;
					BugPropellerSprite.material = mat_ECFrozen;
				}
				break;
			case SpellColorType.Mucus:
				if (sr.material != mat_ECMucus)
				{
					sr.material = mat_ECMucus;
					BugPropellerSprite.material = mat_ECMucus;
				}
				break;
			case SpellColorType.Fire:
				if (sr.material != mat_ECPlayer)
				{
					sr.material = mat_ECPlayer;
					BugPropellerSprite.material = mat_ECPlayer;
				}
				srFire.gameObject.SetActive(value: true);
				PropellerSrFire.gameObject.SetActive(value: true);
				break;
			case SpellColorType.Thunder:
				if (sr.material != mat_ECThunder)
				{
					sr.material = mat_ECThunder;
					BugPropellerSprite.material = mat_ECThunder;
				}
				break;
			case SpellColorType.Player:
				if (sr.material != mat_ECPlayer)
				{
					sr.material = mat_ECPlayer;
					BugPropellerSprite.material = mat_ECPlayer;
				}
				break;
			case SpellColorType.Venom:
				if (sr.material != mat_ECVenom)
				{
					sr.material = mat_ECVenom;
					BugPropellerSprite.material = mat_ECVenom;
				}
				break;
			case SpellColorType.Void:
				if (sr.material != mat_ECVoid)
				{
					sr.material = mat_ECVoid;
					BugPropellerSprite.material = mat_ECVoid;
				}
				break;
			default:
				Debug.LogError(spell.ColorType);
				break;
			}
		}
		Enterstate(ExplosionBugState.BornIdle);
	}

	private void ApplyPullEffect(UnitProperty targetPpt)
	{
		float radius = criticalRange * effectrRadiuscale * effectFinalRadiuRatio;
		Vector3 position = targetPpt.transform.position;
		int num = Mathf.CeilToInt(criticalDamageRatio * explosionDamage);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(position, radius, "Monster");
		collidersByTag = GeneralTool.ListShuffle(collidersByTag);
		int num2 = (int)Mathf.Min(collidersByTag.Count, criticalTargetsCount);
		bool flag = false;
		for (int i = 0; i < collidersByTag.Count && i < num2; i++)
		{
			Collider collider = collidersByTag[i];
			UnitProperty component = collider.gameObject.GetComponent<UnitProperty>();
			if (!(component == null) && !(component == targetPpt))
			{
				flag = true;
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo
				{
					canRebound = false,
					damage = num,
					attackerPpt = PlayerMgr.Inst.PlayerPpt,
					criticalChance = explodeCriticalChance,
					beHitPpt = component
				};
				Spell3101PullCrystal component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, base.transform.position, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
				if (component2.tsf_Layer != null)
				{
					component2.tsf_Layer.localScale = base.transform.localScale;
				}
				component2.SetColor(colorType);
				component2.SetChainTargetTransform(targetPpt.gameObject.transform, collider.transform);
				component.TakeKnockback((targetPpt.transform.position - collider.transform.position).normalized * criticalDragForce * knockBackRatio * finalknockBackRatio);
				component.TakeDamage(explosionDamage, AttackerType.NothingSpecial, takeDamageInfo);
				component2.CreateHitEffect(colorType, collider.transform.position + component2.chainBaseHeight);
				ApplyElementEffectToTarget(takeDamageInfo.beHitPpt);
			}
		}
		if (flag)
		{
			SEMgr.Inst.spell3121Energy.PlaySE().pitch = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	private void CreateExplosionEffect()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20071 + "/" + 20071 + "_BugExplosion_" + colorType, base.transform.position.IgnoreZ(), 1f).transform.localScale = Vector3.one * damageDetectRadiu;
	}

	private void ApplyElementEffectToTarget(UnitProperty targetPpt)
	{
		if (mucusTime > 0f)
		{
			targetPpt.SetMucus(mucusTime, mucusMoveSpeedEffect, mucusSpellSpeedEffect);
		}
		if (venomTime > 0f)
		{
			targetPpt.SetVenom(venomTime, venomApplyCount);
		}
		if (frozenTime > 0f)
		{
			targetPpt.SetFrozen(frozenTime);
		}
		if (burnTime > 0f)
		{
			targetPpt.SetBurn(burnTime, burnDamage);
		}
		if (explosionData != null)
		{
			targetPpt.SetVoid(explosionData);
		}
	}

	public void DeathThunderAttack()
	{
		if (!isEndThunderAttack)
		{
			return;
		}
		float radius = endThunderHitRadiu * radiuRatio;
		Vector3 position = base.transform.position;
		float damage = explosionDamage * endThunderDamageRatio;
		LayerCorrect component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31011, position, quaternion.identity, 1f).GetComponent<LayerCorrect>();
		component.tsf_Layer.localScale = Vector3.one * endThunderHitRadiu / SpellConfig.dic[31011].float1 * radiuRatio;
		SEMgr.Inst.spell3101Hit.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(position, radius, thunderColliderBuffer, thunderColliderTags);
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
				if (!componentInParent.IsSameCamp(UnitType.Player))
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
					canRebound = false
				};
				string path = string.Format("{0}{1}/{2}_Hit", "Prefabs/Spell/", 31011, 31011);
				ObjPoolMgr.Inst.GetGO(path, component2.transform.position + new Vector3(0f, 0.3f, 0f), quaternion.identity, 1f).GetComponent<LayerCorrect>().tsf_Layer.transform.right = (component2.transform.position - component.transform.position).normalized;
				component2.TakeDamage(damage, AttackerType.NothingSpecial, info);
			}
			else if (thunderColliderBuffer[i].CompareTag("Brittleness"))
			{
				TakeDamageInfo info2 = new TakeDamageInfo
				{
					canRebound = false
				};
				component2.TakeDamage(damage, AttackerType.NothingSpecial, info2);
			}
		}
	}
}
