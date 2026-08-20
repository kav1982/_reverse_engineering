using System;
using UnityEngine;

public class Spell1022Boomerang : SpellBase, IOnLaunchFromUnitNotPlayer
{
	public float speedLerpValue;

	public float rollingBackStartTime;

	public float lerpSpeedIncreaseSpeed;

	public float rotateMovementDuration;

	private float rotateMovementDurationTimer;

	public float chaseMouseEffectDuration;

	private float chaseMouseEffectDurationTimer;

	public float chaseTargetEffectDuration;

	private float chaseTargetEffectDurationTimer;

	public AnimationCurve fullCurve;

	public SphereCollider HitBox;

	public float hitboxRestartInterval;

	private float hitboxRestartTimer;

	public float safeTime;

	private bool keepFollowOwner = true;

	public float maxDistance;

	private bool hoverStart;

	private bool spellEnd;

	public float spellDurationBuffRatio;

	public float endRecoilRatio;

	public float backStartTimer { get; set; }

	public float extraLerpSpeed { get; set; }

	public override void InitializeCallback()
	{
		backStartTimer = 0f;
		extraLerpSpeed = 0f;
		rotateMovementDurationTimer = 0f;
		hitboxRestartTimer = 0f;
		chaseMouseEffectDurationTimer = 0f;
		chaseTargetEffectDurationTimer = 0f;
		keepFollowOwner = true;
		hoverStart = false;
		spellEnd = false;
		base.penetrateTime += base.spellCfg.int1;
		if (base.SIP.ShootCause is ShootCause.BySplit bySplit)
		{
			base.OwnerSpell = bySplit.Spell.Data.OwnerSpell;
			ownerPpt = bySplit.Spell.Data.ownerPpt;
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
	}

	private Vector3? GetOwnerCurrentPoint()
	{
		if (!keepFollowOwner)
		{
			return null;
		}
		if (ownerPpt != null && ownerPpt.gameObject.activeInHierarchy)
		{
			return ownerPpt.transform.position;
		}
		return null;
	}

	private void UpdatateOwnerState()
	{
		if (ownerPpt != null && !ownerPpt.gameObject.activeInHierarchy)
		{
			keepFollowOwner = false;
		}
	}

	public void ResetOwnerAndShoot()
	{
		extraLerpSpeed = 0f;
		backStartTimer = 0f;
		base.spellCfg.speed = (SpellConfig.dic[base.spellCfg.id].speed + base.bonusSpeed) * base.finalSpeedRatio;
		base.CurrentSpeed = base.spellCfg.speed;
		keepFollowOwner = true;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
	}

	public void CheckHitBoxState()
	{
		hitboxRestartTimer += Time.deltaTime;
		if (!HitBox.gameObject.activeInHierarchy)
		{
			HitBox.gameObject.SetActive(value: true);
		}
		if (hitboxRestartTimer >= hitboxRestartInterval)
		{
			hitboxRestartTimer -= hitboxRestartInterval;
			HitBox.gameObject.SetActive(value: false);
		}
	}

	private void CheckIfSpellToFarThenRecycle()
	{
		if (Vector3.Distance(base.transform.position, ownerPpt.transform.position) >= maxDistance)
		{
			base.spellSplitCount = 0;
			PoolRecycle();
		}
	}

	protected override void OnFallingGround()
	{
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		PlaySE("Shoot");
	}

	public override void Update()
	{
		base.Update();
		CheckHitBoxState();
		UpdatateOwnerState();
		TouchOwnerUnitCheck();
		UpdateHoverState();
		CheckIfSpellToFarThenRecycle();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (!(base.DurationTimer > base.spellCfg.duration) && (!hoverStart || base.SIP.spellIsFall))
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
			hoverStart = true;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer <= base.SpellHoverTime)
		{
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	private void UpdateHoverState()
	{
		if (!hoverStart || !(base.SpellHoverTime > 0f))
		{
			return;
		}
		base.SpellHoverTimer += Time.deltaTime;
		rigid.linearVelocity = Vector3.zero;
		if (base.SpellHoverTimer >= base.SpellHoverTime)
		{
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	private void TouchOwnerUnitCheck()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation || !(ownerPpt != null) || !ownerPpt.gameObject.activeInHierarchy || !(Tool2D.IgnoreZDistance(base.transform.position, ownerPpt.transform.position) <= tsf_Layer.transform.localScale.x * tsf_Layer.transform.localScale.x / 2f) || !(base.DurationTimer >= safeTime) || (base.SIP.spellIsFall && !(base.Height <= 1f)))
		{
			return;
		}
		if (base.SpellHoverTime > 0f)
		{
			hoverStart = true;
			return;
		}
		float num = 1f;
		if (IsSameCamp(UnitType.Player))
		{
			num = SpellShootGroupExtend.GetRecoilRatio(base.shooterWand).CurrentMulRatio;
		}
		ownerPpt.TakeKnockback(base.Direction * base.spellCfg.recoil * endRecoilRatio * num);
		PoolRecycle();
	}

	private float GetMovementEffectDuration(float baseDuration)
	{
		return (baseDuration + base.bonusDuration) * base.finalDurationRatio * spellDurationBuffRatio;
	}

	public override void SpellAroundPlayer()
	{
		if (!hoverStart)
		{
			float num = fullCurve.Evaluate(Mathf.Clamp(rotateMovementDurationTimer / GetMovementEffectDuration(rotateMovementDuration), 0f, 1f)) * base.spellAroundOwnerRadius;
			rotateMovementDurationTimer += Time.deltaTime;
			float num2 = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
			base.spellAroundOwnerCurrentAngle += num2;
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
			Vector3 v = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * num;
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			SpellAroundPlayerUpdateMoveTrigger(num2);
			if (rotateMovementDurationTimer >= GetMovementEffectDuration(rotateMovementDuration))
			{
				hoverStart = true;
			}
		}
	}

	protected override void SpellFollowTarget()
	{
		if (hoverStart)
		{
			return;
		}
		extraLerpSpeed += lerpSpeedIncreaseSpeed * Time.deltaTime;
		float num = ((!(chaseTargetEffectDurationTimer / GetMovementEffectDuration(chaseTargetEffectDuration) > 1f)) ? 1 : 0);
		chaseTargetEffectDurationTimer += Time.deltaTime;
		if (base.SpellFollowHaveTarget && num == 1f)
		{
			base.Direction = Vector3.Lerp(base.Direction, Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime * num), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
		}
		else
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			Vector3? vector = GetOwnerCurrentPoint();
			if (!vector.HasValue)
			{
				vector = base.transform.position + base.Direction;
			}
			base.Direction = Vector3.Lerp(base.Direction, ToPointDir(vector.Value), (base.CurrentSpeed + extraLerpSpeed) * Time.deltaTime * speedLerpValue);
		}
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
	}

	protected override void SpellFollowMouse()
	{
		if (!hoverStart)
		{
			extraLerpSpeed += lerpSpeedIncreaseSpeed * Time.deltaTime;
			float num = ((chaseMouseEffectDurationTimer / GetMovementEffectDuration(chaseMouseEffectDuration) > 1f) ? 1 : 0);
			chaseMouseEffectDurationTimer += Time.deltaTime;
			Vector3 point = PlayerMgr.Inst.GetMousePoint(base.transform.position.z) + (ownerPpt.transform.position - PlayerMgr.Inst.GetMousePoint(base.transform.position.z)) * num;
			Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(point) * base.CurrentSpeed, (base.CurrentSpeed + extraLerpSpeed) * Time.deltaTime * speedLerpValue * 1.5f);
			rigid.linearVelocity = linearVelocity;
			base.Direction = linearVelocity.normalized;
		}
	}

	protected override void SpellNormalTransform()
	{
		if (hoverStart)
		{
			return;
		}
		backStartTimer += Time.deltaTime;
		if (backStartTimer >= rollingBackStartTime)
		{
			extraLerpSpeed += lerpSpeedIncreaseSpeed * Time.deltaTime;
			Vector3? vector = GetOwnerCurrentPoint();
			if (!vector.HasValue)
			{
				vector = base.transform.position + base.Direction;
			}
			Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(vector.Value) * base.CurrentSpeed, (base.CurrentSpeed + extraLerpSpeed) * Time.deltaTime * speedLerpValue);
			rigid.linearVelocity = linearVelocity;
			base.Direction = linearVelocity.normalized;
		}
	}

	public override void PoolRecycle()
	{
		if (!spellEnd)
		{
			spellEnd = true;
			PlaySE("End");
			EffectBase.CreateSpriteEffect("End");
			base.PoolRecycle();
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		hitboxRestartTimer = 0f;
		return base.MakeDamageToUnit(unit);
	}

	protected override bool MakeDamageToSpell(SpellBase spell)
	{
		hitboxRestartTimer = 0f;
		return base.MakeDamageToSpell(spell);
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate52 teammate)
		{
			ownerPpt = teammate.myPpt;
		}
	}

	protected override void OnHitWallAndSolidObj(Collider col)
	{
	}
}
