using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell4013ArcaneBlade : SpellBase
{
	public const float HammerBaseLength = 1.5f;

	public Transform DistanceCalculateTransform;

	public Transform HammerCenterTransform;

	public Transform HammerShadowCenterTransform;

	public float hammerBaseSpriteBaseWidthAndLength;

	public float hammerMinWidthRatio;

	public float triailXposShiftConvertRatio;

	public float emberBaseRadiu;

	public float emberShiftConvertRatio;

	public float baseKnockBack;

	private List<Collider> hitTargetsList = new List<Collider>();

	public float ClearHitTargetListTime;

	public float ChaseEnemyMovementClearHitTargetListTime;

	private float clearHitTargetListTimer;

	public ShockParam shock;

	public float hammerYHitHeight;

	public float hammerBaseYHitHeight;

	private Transform followingTransform;

	private bool hammerEnable;

	public float defaultRotateAngleSpeed;

	public float defaultRoatateSpeedMultiplier;

	public float rotateBaseDistanceBetweenBody;

	public float AngleBetweenHammerInChaseMouseEnemyMovement;

	public float BuffRecalculateTime;

	private float buffRecalculateTimer;

	public float RecheckDamageTime;

	private float recheckDamageTimer;

	public BoxCollider hitBox;

	public float thunderAttackInterval;

	private float thunderAttackTimer;

	public float FallDamageInterval;

	private static float fallDamageTimer;

	private static bool fallDealDamageInThisFrame;

	private static bool updateFallDealDamageInThisFrame;

	public float FallHammerHeight;

	public AnimationCurve FallCurve;

	public float SEPlayInterval;

	public List<GameObject> SplitHammerList { get; set; } = new List<GameObject>();


	public int SplitCount { get; set; }

	public int SplitIndex { get; set; }

	private float hitEffectSize => Mathf.Min(base.transform.localScale.x, 4f);

	protected override void InitSpeedAndPositionWithFall()
	{
		base.InitSpeedAndPositionWithoutFall();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		clearHitTargetListTimer = 0f;
		hammerEnable = false;
		buffRecalculateTimer = 0f;
	}

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		base.spellCfg.abilityType = SpellAbilityType.RuneHammer;
		base.spellCfg.knockback = baseKnockBack * base.knockbackRatio * base.finalKnockbackRatio;
		followingTransform = null;
		recheckDamageTimer = 0f;
		base.CanBeCapture = false;
		SplitHammerList.Clear();
		SplitCount = 0;
		SplitIndex = 0;
		thunderAttackTimer = 0f;
		spellEndTeleport = false;
		base.transform.localScale = Vector3.one * Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f) * base.spellVolumeRatio;
		UpdateStaticWandAngleShiftListCount();
		UpdateHitBoxPos();
		fallDamageTimer = 0f;
		fallDealDamageInThisFrame = false;
		updateFallDealDamageInThisFrame = false;
		base.spellCfg.radius = 1.5f * base.radiusRatio * base.finalRadiusRatio;
		if (base.SIP.spellIsFall)
		{
			HammerCenterTransform.localPosition = Vector3.zero;
			HammerShadowCenterTransform.localPosition = Vector3.zero;
		}
		triggerIn.gameObject.SetActive(!base.SIP.spellIsFall);
		if (base.endThunderHitPercent > 0f && base.ColorType == SpellColorType.Player)
		{
			base.ColorType = SpellColorType.Thunder;
		}
		tsf_Layer.gameObject.SetActive(value: false);
	}

	public void SetSplitHammerData(int index, int count)
	{
		SplitIndex = index;
		SplitCount = count;
		base.spellCfg.isSplitSpell = true;
	}

	public void RecycleAllSplitHammer()
	{
		if (SplitHammerList.Count <= 0)
		{
			return;
		}
		for (int num = SplitHammerList.Count; num > 0; num--)
		{
			if (!SplitHammerList[num - 1].IsDestroyed())
			{
				SplitHammerList[num - 1].transform.parent = null;
				SplitHammerList[num - 1].GetComponent<Spell4013ArcaneBlade>().PoolRecycle();
			}
		}
		SplitHammerList.Clear();
	}

	private void UpdateHitBoxPos()
	{
		float num = Mathf.Max(hammerMinWidthRatio, base.radiusRatio * base.finalRadiusRatio) - 1f;
		hitBox.center = new Vector3(1.5f + num * triailXposShiftConvertRatio, 0f, 0f);
		DistanceCalculateTransform.localPosition = new Vector3(1.5f + num * triailXposShiftConvertRatio, 0f, 0f);
	}

	private void UpdateVisualSwitch()
	{
		bool flag = true;
		tsf_Layer.gameObject.SetActive(flag);
		if (flag)
		{
			if (base.SIP.spellIsFall)
			{
				((Spell4013Effect)EffectBase).SpawnFallHammer();
			}
			else
			{
				((Spell4013Effect)EffectBase).SpawnHammer();
			}
		}
		else
		{
			((Spell4013Effect)EffectBase).HideHammer();
		}
		hammerEnable = flag;
	}

	public override void Update()
	{
		base.Update();
		UpdateAndLockOveralAngle();
		RestartTrigger();
		thunderAttackTimer += Time.deltaTime;
		UpdateFallDamageTimer();
		UpdateVisualSwitch();
		UpdateStaticWandAngleShift();
		if (!followingTransform.IsDestroyed() && !(followingTransform == null))
		{
			if (!base.SIP.spellIsFall)
			{
				UpdateSelfAndHammerOutlookPosition();
			}
			else
			{
				UpdateFallHammerOutlookPosition();
			}
			FallHammerDealDamage();
			UpdateHammerBuffEffect();
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		UnlockOveralAngle();
		UpdateHitListState();
		UpdateVisualSwitch();
		ResetFallDealDamageInThisFrame();
		tsf_Layer.localPosition = new Vector3(0f, 0f, tsf_Layer.localPosition.z);
		if (!base.SIP.spellIsFall)
		{
			Vector3 position = HammerShadowCenterTransform.position;
			position = new Vector3(position.x, position.y, Mathf.Abs(position.z));
			HammerShadowCenterTransform.position = Tool2D.GetLayerPoint(position, LayerCorrectType.Shadow);
		}
	}

	private void UpdateFallDamageTimer()
	{
		if (base.SIP.spellIsFall && !updateFallDealDamageInThisFrame)
		{
			fallDamageTimer += Time.deltaTime;
			updateFallDealDamageInThisFrame = true;
			if (!(fallDamageTimer < FallDamageInterval))
			{
				fallDealDamageInThisFrame = true;
			}
		}
	}

	private void ResetFallDealDamageInThisFrame()
	{
		updateFallDealDamageInThisFrame = false;
		if (fallDealDamageInThisFrame)
		{
			fallDamageTimer = 0f;
			fallDealDamageInThisFrame = false;
		}
	}

	private void FallHammerDealDamage()
	{
		if (base.SIP.spellIsFall && fallDealDamageInThisFrame && (base.spellCfg.isSplitSpell || SplitHammerList.Count <= 0))
		{
			MakeFallingGroundDamageToAround(base.transform.position);
			if (Random.Range(0f, 1f) < base.endTHunderHitChance)
			{
				EndThunderAttackCheck(thunderOnly: false, Tool2D.IgnoreZPoint(base.transform.position));
			}
			PlaySE("FallHitSE", SEPlayInterval);
			CamController.Inst.SetShock(shock);
		}
	}

	private void RestartTrigger()
	{
		if (!base.SIP.spellIsFall)
		{
			recheckDamageTimer += Time.deltaTime;
			if (!triggerIn.gameObject.activeInHierarchy)
			{
				triggerIn.gameObject.SetActive(value: true);
			}
			if (recheckDamageTimer >= RecheckDamageTime)
			{
				recheckDamageTimer -= RecheckDamageTime;
				triggerIn.gameObject.SetActive(value: false);
			}
		}
	}

	private void UpdateHitListState()
	{
		clearHitTargetListTimer += Time.deltaTime;
		float num = ((base.currentSpellMovement != SpellSpecialMovementType.ChaseEnemy) ? ClearHitTargetListTime : ChaseEnemyMovementClearHitTargetListTime);
		if (clearHitTargetListTimer >= num)
		{
			clearHitTargetListTimer -= num;
			hitTargetsList.Clear();
		}
	}

	public float GetFallCurveProgress()
	{
		return FallCurve.Evaluate(Mathf.Clamp(fallDamageTimer / FallDamageInterval, 0f, 1f));
	}

	private void UpdateFallHammerOutlookPosition()
	{
		float degree = (0f - AngleBetweenHammerInChaseMouseEnemyMovement) * (float)(base.SIP.multiShootCount - 1) / 2f + AngleBetweenHammerInChaseMouseEnemyMovement * (float)base.SIP.inMultiShootIndex;
		if (base.spellCfg.isSplitSpell)
		{
			degree = (0f - AngleBetweenHammerInChaseMouseEnemyMovement) * (float)(SplitCount - 1) / 2f + AngleBetweenHammerInChaseMouseEnemyMovement * (float)SplitIndex;
		}
		Vector3 vector = new Vector3(0f, FallHammerHeight * GetFallCurveProgress(), GetFallCurveProgress() * -5f);
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
		{
			if (!(followingTransform == null))
			{
				base.transform.position = Tool2D.IgnoreZPoint(followingTransform.position);
				base.transform.right = Tool2D.GetDir(followingTransform.right, degree);
				Vector3 v = (base.spellCfg.isSplitSpell ? followingTransform.position : GetAroundTargetBasePoint()) + Tool2D.GetDir(followingTransform.right, degree) * base.spellCfg.radius + vector;
				base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			}
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
		{
			Vector3 v2 = (base.spellCfg.isSplitSpell ? followingTransform.position : GetHammerAroundOwnerPoint());
			base.transform.position = Tool2D.IgnoreZPoint(v2);
			if (GetNearestTargetablePpt(base.transform.position) != null)
			{
				base.transform.right = Tool2D.DirMoveTowards(base.transform.right, Tool2D.GetDir(ToPointDir(GetNearestTargetablePpt(base.transform.position).transform), degree), 12f * spellFollowTargetRotateSpeed * base.SIP.speedDecreaseRatio * Time.deltaTime);
			}
			else
			{
				base.transform.right = Tool2D.DirMoveTowards(base.transform.right, Tool2D.GetDir(followingTransform.right, degree), 12f * spellFollowTargetRotateSpeed * base.SIP.speedDecreaseRatio * Time.deltaTime);
			}
			base.transform.position += Tool2D.IgnoreZPoint(base.transform.right * base.spellCfg.radius) + vector;
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellAroundOwnerCurrentAngle = CalculateRotationDegree();
			base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
			Vector3 vector2 = (base.spellCfg.isSplitSpell ? followingTransform.position : GetHammerAroundOwnerPoint());
			base.transform.position = Tool2D.IgnoreZPoint(vector2 + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle).normalized * base.spellAroundOwnerRadius) + vector;
			if (base.spellCfg.isSplitSpell)
			{
				base.transform.position += new Vector3(0f, 0.75f * base.radiusRatio * base.finalRadiusRatio, 0f);
			}
		}
		else
		{
			base.spellAroundOwnerCurrentAngle = CalculateRotationDegree();
			base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
			Vector3 vector3 = (base.spellCfg.isSplitSpell ? followingTransform.position : GetHammerAroundOwnerPoint());
			base.transform.position = Tool2D.IgnoreZPoint(vector3 + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle).normalized * base.spellCfg.radius) + vector;
			if (base.spellCfg.isSplitSpell)
			{
				base.transform.position += new Vector3(0f, 0.75f * base.radiusRatio * base.finalRadiusRatio, 0f);
			}
		}
	}

	private void UpdateSelfAndHammerOutlookPosition()
	{
		float degree = (0f - AngleBetweenHammerInChaseMouseEnemyMovement) * (float)(base.SIP.multiShootCount - 1) / 2f + AngleBetweenHammerInChaseMouseEnemyMovement * (float)base.SIP.inMultiShootIndex;
		if (base.spellCfg.isSplitSpell)
		{
			degree = (0f - AngleBetweenHammerInChaseMouseEnemyMovement) * (float)(SplitCount - 1) / 2f + AngleBetweenHammerInChaseMouseEnemyMovement * (float)SplitIndex;
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
		{
			if (!(followingTransform == null))
			{
				base.transform.position = Tool2D.IgnoreZPoint(followingTransform.position);
				base.transform.right = Tool2D.GetDir(followingTransform.right, degree);
				HammerCenterTransform.localPosition = Vector3.zero;
				HammerShadowCenterTransform.position = base.transform.position - new Vector3(0f, 0f, 0.5f);
			}
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
		{
			Vector3 v = (base.spellCfg.isSplitSpell ? followingTransform.position : GetHammerAroundOwnerPoint());
			base.transform.position = Tool2D.IgnoreZPoint(v);
			if (GetNearestTargetablePpt(base.transform.position) != null)
			{
				base.transform.right = Tool2D.DirMoveTowards(base.transform.right, Tool2D.GetDir(ToPointDir(GetNearestTargetablePpt(base.transform.position).transform), degree), 12f * spellFollowTargetRotateSpeed * Time.deltaTime);
			}
			else
			{
				base.transform.right = Tool2D.DirMoveTowards(base.transform.right, Tool2D.GetDir(followingTransform.right, degree), 12f * spellFollowTargetRotateSpeed * Time.deltaTime);
			}
			base.transform.position += Tool2D.IgnoreZPoint(base.transform.right * 0.1f);
			HammerCenterTransform.position = base.transform.position + new Vector3(0f, 0.3f, 0f);
			HammerCenterTransform.localPosition = Tool2D.IgnoreZPoint(HammerCenterTransform.localPosition);
			HammerShadowCenterTransform.position = base.transform.position;
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellAroundOwnerCurrentAngle = CalculateRotationDegree();
			base.transform.right = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle);
			Vector3 v2 = (base.spellCfg.isSplitSpell ? followingTransform.position : GetAroundTargetBasePoint()) + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius * 0.8f;
			base.transform.position = Tool2D.IgnoreZPoint(v2, base.transform.position.z);
			HammerCenterTransform.position = base.transform.position + new Vector3(0f, 0.3f, 0f);
			HammerCenterTransform.localPosition = Tool2D.IgnoreZPoint(HammerCenterTransform.localPosition);
			HammerShadowCenterTransform.position = base.transform.position;
		}
		else
		{
			base.spellAroundOwnerCurrentAngle = CalculateRotationDegree();
			base.transform.right = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle);
			Vector3 vector = (base.spellCfg.isSplitSpell ? followingTransform.position : GetHammerAroundOwnerPoint());
			base.transform.position = Tool2D.IgnoreZPoint(vector + base.transform.right * rotateBaseDistanceBetweenBody);
			HammerCenterTransform.position = base.transform.position + new Vector3(0f, 0.3f, 0f);
			HammerCenterTransform.localPosition = Tool2D.IgnoreZPoint(HammerCenterTransform.localPosition);
			HammerShadowCenterTransform.position = base.transform.position;
		}
	}

	public Vector3 GetHammerAroundOwnerPoint()
	{
		Vector3 result = ownerPpt.transform.position;
		if (ownerPpt.unitCfg.unitType == UnitType.Player && (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null || PlayerMgr.Inst.SelectedWand.WandCfg.specialAbility == WandAbility.LongWand || PlayerMgr.Inst.SelectedWand.WandCfg.specialAbility == WandAbility.LongWandAndSpellBreaker))
		{
			result = GetAroundTargetBasePoint();
		}
		return result;
	}

	public override Vector3 GetAroundTargetBasePoint()
	{
		if (base.SIP.tags.Contains(SpellTag.Twine))
		{
			if (base.OwnerSpell != null && !base.isOwnerSpellEnd)
			{
				lastAroundTargetPosition = ((base.OwnerSpell.spellCfg.abilityType == SpellAbilityType.HighPressureWasher) ? ((Spell1019HighPressureWasherRemaster)base.OwnerSpell).FirstWaterPosition : base.OwnerSpell.transform.position);
			}
			return lastAroundTargetPosition;
		}
		if (!base.spellCfg.isSplitSpell && ownerPpt != null && base.OwnerSpell == null && ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			if (PlayerMgr.Inst.SelectedWand.WandCfg != null && !PlayerMgr.Inst.SelectedWand.passiveAutoWand)
			{
				return PlayerMgr.Inst.SelectedWand.GetShootPosition();
			}
			return GetPlayerBaseShootPoint();
		}
		if (base.OwnerTsf != null)
		{
			return base.OwnerTsf.transform.position;
		}
		if (base.OwnerPoint != Vector3.zero)
		{
			return base.OwnerPoint;
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

	public float GetHammerHitBoxdistance()
	{
		float num = 0f;
		if (!hammerEnable)
		{
			return hammerMinWidthRatio;
		}
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
			num += rotateBaseDistanceBetweenBody;
			break;
		case SpellSpecialMovementType.ChaseMouse:
			num += Tool2D.IgnoreZDistance(Tool2D.IgnoreZPoint(followingTransform.position), base.transform.position);
			break;
		case SpellSpecialMovementType.Rotation:
			num += base.spellAroundOwnerRadius * 0.8f;
			break;
		}
		_ = (hammerYHitHeight - hammerBaseYHitHeight) / 2f;
		new Vector2(base.transform.right.x, base.transform.right.y);
		return num + Tool2D.IgnoreZDistance(base.transform.position, Tool2D.IgnoreZPoint(DistanceCalculateTransform.position));
	}

	public void UpdateHammerBaseAttackDamage(float baseDamage)
	{
		base.spellCfg.damage = Mathf.Ceil(baseDamage * base.damageRatio * base.finalDamageRatio + base.SIP.finalDamageExtra);
	}

	public void UpdateHammerBuffEffect()
	{
		if (base.finishedInitialize)
		{
			buffRecalculateTimer += Time.deltaTime;
			if (!(buffRecalculateTimer < BuffRecalculateTime))
			{
				buffRecalculateTimer -= BuffRecalculateTime;
				SpellInitialParameter applyWandAllEnhanceEffectSIP = base.shooterWand.GetApplyWandAllEnhanceEffectSIP(40131);
				base.bonusSpeed = applyWandAllEnhanceEffectSIP.bounsSpeed;
				base.spellCfg.speed = (0f + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
				base.transform.localScale = Vector3.one * Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f) * base.spellVolumeRatio;
			}
		}
	}

	public void SetHammerFollowTransform(Transform trans)
	{
		followingTransform = trans;
	}

	public override void TriggerIn(Collider other)
	{
		if (SplitHammerList.Count > 0)
		{
			return;
		}
		Vector3 value = Tool2D.IgnoreZPoint((other.transform.position - base.transform.position).normalized);
		Vector3 value2 = other.transform.position + new Vector3(0f, 0.3f, 0f) + Tool2D.IgnoreZPoint(Random.insideUnitSphere * 0.2f);
		if (other.gameObject.CompareAnyTag("RollBall", "Butterfly"))
		{
			SpellBase componentInParent = other.gameObject.GetComponentInParent<SpellBase>();
			if (IsSameCamp(componentInParent))
			{
				return;
			}
			if (!(componentInParent is Spell1003Butterfly))
			{
				if (componentInParent is Spell1002RollBall)
				{
					MakeDamageToSpell(componentInParent);
					EffectBase.ManualCreateEffect("Hit", hitEffectSize, value2, value);
					PlaySE("HitSE", SEPlayInterval);
					CamController.Inst.SetShock(shock);
				}
			}
			else
			{
				MakeDamageToSpell(componentInParent);
			}
		}
		else if (other.gameObject.CompareAnyTag("Monster"))
		{
			recheckDamageTimer = 0f;
			UnitProperty component = other.gameObject.GetComponent<UnitProperty>();
			TakeDamageInfo info = new TakeDamageInfo
			{
				canRebound = false
			};
			if (!IsSameCamp(component.unitCfg.unitType))
			{
				OutputDamage(other.gameObject, info);
				EffectBase.ManualCreateEffect("Hit", hitEffectSize, value2, value);
				PlaySE("HitSE", SEPlayInterval);
				CamController.Inst.SetShock(shock);
				float num = (base.spellCfg.isSplitSpell ? base.spellCfg.knockback : (base.spellCfg.knockback * 0.35f));
				component.TakeKnockback(Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(other.transform, base.transform), Random.Range(-30f, 30f)) * num);
				if (Random.Range(0f, 1f) < base.endTHunderHitChance && thunderAttackTimer > thunderAttackInterval)
				{
					thunderAttackTimer = 0f;
					EndThunderAttackCheck(thunderOnly: false, Tool2D.IgnoreZPoint(DistanceCalculateTransform.position));
				}
			}
		}
		else if (other.gameObject.CompareAnyTag("Destructible"))
		{
			recheckDamageTimer = 0f;
			TakeDamageInfo info2 = new TakeDamageInfo
			{
				canRebound = false,
				knockbackForce = Tool2D.IgnoreZV2ToV1Normal(other.transform, base.transform) * base.spellCfg.knockback
			};
			EffectBase.ManualCreateEffect("Hit", hitEffectSize, value2, value);
			CamController.Inst.SetShock(shock);
			OutputDamage(other.gameObject, info2);
			PlaySE("HitSE", SEPlayInterval);
		}
		else if (!other.gameObject.CompareAnyTag("SolidObj", "Player", "Teammate"))
		{
			TakeDamageInfo info3 = new TakeDamageInfo
			{
				canRebound = false,
				knockbackForce = Tool2D.IgnoreZV2ToV1Normal(other.transform, base.transform) * base.spellCfg.knockback
			};
			OutputDamage(other.gameObject, info3);
		}
	}

	public float CalculateRotationDegree()
	{
		float num = (base.SIP.spellIsFall ? (Spell4013StaticOveralAngleCalculator.OveralAngle / 2f) : Spell4013StaticOveralAngleCalculator.OveralAngle);
		float num2 = GetWandInitialIndependentAngle() + GetStaticWandAngleShift() + GetWandHammerIndexAngleShift() + GetHammerSplitIndexAnlgeShift();
		float num3 = num * base.SIP.speedDecreaseRatio + num2;
		if (base.spellCfg.isSplitSpell)
		{
			num3 *= -1f;
		}
		if (base.SIP.reverseDirection)
		{
			num3 *= -1f;
		}
		return num3;
	}

	private void UpdateAndLockOveralAngle()
	{
		if (!Spell4013StaticOveralAngleCalculator.UpdatedOveralAngleInThisFrame)
		{
			Spell4013StaticOveralAngleCalculator.UpdatedOveralAngleInThisFrame = true;
			Spell4013StaticOveralAngleCalculator.OveralAngle += defaultRotateAngleSpeed * Time.deltaTime;
		}
	}

	private void UnlockOveralAngle()
	{
		Spell4013StaticOveralAngleCalculator.UpdatedOveralAngleInThisFrame = false;
	}

	private float GetWandInitialIndependentAngle()
	{
		List<Wand> list = new List<Wand>();
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg != null && wand.passiveRuneHammerEnable)
			{
				list.Add(wand);
			}
		}
		return 360f / (float)list.Count * (float)list.IndexOf(base.shooterWand);
	}

	private void UpdateStaticWandAngleShiftListCount()
	{
		if (Spell4013StaticOveralAngleCalculator.GetWandAngleShiftList().All(((Wand wand, float angle) value) => value.wand != base.shooterWand))
		{
			Spell4013StaticOveralAngleCalculator.AddNewAngleData(base.shooterWand, 0f);
		}
	}

	private void UpdateStaticWandAngleShift()
	{
		if (!(base.spellCfg.speed > 0f) || base.spellCfg.isSplitSpell)
		{
			return;
		}
		List<(Wand, float)> wandAngleShiftList = Spell4013StaticOveralAngleCalculator.GetWandAngleShiftList();
		for (int i = 0; i < wandAngleShiftList.Count; i++)
		{
			if (wandAngleShiftList[i].Item1 == base.shooterWand)
			{
				wandAngleShiftList[i] = (wandAngleShiftList[i].Item1, wandAngleShiftList[i].Item2 + defaultRotateAngleSpeed / defaultRoatateSpeedMultiplier * base.spellCfg.speed * Time.deltaTime / (float)base.SIP.multiShootCount);
			}
		}
	}

	public float GetHarmmerCirleRoundTime()
	{
		return 360f / (defaultRotateAngleSpeed + defaultRotateAngleSpeed / defaultRoatateSpeedMultiplier * base.spellCfg.speed);
	}

	private float GetStaticWandAngleShift()
	{
		foreach (var (wand, result) in Spell4013StaticOveralAngleCalculator.GetWandAngleShiftList())
		{
			if (wand == base.shooterWand)
			{
				return result;
			}
		}
		return 0f;
	}

	private float GetWandHammerIndexAngleShift()
	{
		if (base.SIP.multiShootCount == 0)
		{
			return 0f;
		}
		return 360f / (float)base.SIP.multiShootCount * (float)base.SIP.inMultiShootIndex;
	}

	private float GetHammerSplitIndexAnlgeShift()
	{
		if (SplitCount <= 0)
		{
			return 0f;
		}
		return 360f / (float)SplitCount * (float)SplitIndex;
	}

	public override void PoolRecycle()
	{
		base.endThunderHitPercent = 0f;
		base.PoolRecycle();
	}
}
