using System;
using UnityEngine;

public class Spell1029DimensionTraveller : SpellBase
{
	public float chaseMovementEffectRatio;

	public float mouseMovementEffectRatio;

	public float rotateMovementSpeedRatio;

	public SphereCollider HitBox;

	public float hitboxRestartInterval;

	private float hitboxRestartTimer;

	public Transform shadowTrans;

	private bool isDrainingMp;

	private float MpBonusDamage;

	private float MpBonusDuration;

	private bool isTwineSpell => base.SIP.tags.Contains(SpellTag.Twine);

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
		isThroughWall = true;
		base.penetrateTime = 100000;
		spellFollowTargetRotateSpeed *= chaseMovementEffectRatio;
		spellFollowMouseLerp *= mouseMovementEffectRatio;
		hitboxRestartTimer = 0f;
		HitBox.radius = base.spellCfg.radius;
		isDrainingMp = false;
		base.transform.localScale = Vector3.one;
		MpBonusDamage = 0f;
		MpBonusDuration = 0f;
		RaiseDamageByMaxMp();
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		((Spell1029Effect)EffectBase).SpawnNewTrail();
		PlaySE("ShootBall", 0.1f);
		shadowTrans.localScale = Vector3.one * base.radiusRatio / 1.2f;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateCurrentPos();
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		spellBase.spellCfg.damage += MpBonusDamage * 0.33f;
		spellBase.spellCfg.duration += MpBonusDuration * 0.5f;
		return spellBase;
	}

	protected override void UpdateUpSpeedWithFalling()
	{
		if (!(Mathf.Abs(base.CurrentUpSpeed) > InFallingReboundingGravity * 1.5f))
		{
			base.UpdateUpSpeedWithFalling();
		}
	}

	public override void Update()
	{
		base.Update();
		UpdateColliderState();
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration && !base.SIP.spellIsFall)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (isDrainingMp)
		{
			DrainMp();
			isDrainingMp = false;
		}
	}

	public override void PoolRecycle()
	{
		PlaySE("Hit", 0.1f);
		EffectBase.ManualCreateEffect("Disappear");
		base.PoolRecycle();
	}

	public override void SpellAroundPlayer()
	{
		float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
		base.spellAroundOwnerCurrentAngle += num * rotateMovementSpeedRatio;
		base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
		Vector3 v = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		SpellAroundPlayerUpdateMoveTrigger(num);
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		hitboxRestartTimer = 0f;
		return base.OnHitUnit(unit);
	}

	private void UpdateColliderState()
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

	private void RaiseDamageByMaxMp()
	{
		if (IsSameCamp(UnitType.Player) && !base.spellCfg.isSplitSpell)
		{
			isDrainingMp = true;
			float currentMP = base.shooterWand.CurrentMP;
			MpBonusDamage = currentMP / base.spellCfg.float1 * base.spellCfg.float2 * base.damageRatio * base.finalDamageRatio;
			MpBonusDuration = currentMP / base.spellCfg.float1 * base.spellCfg.float3 * base.finalDurationRatio;
			base.spellCfg.damage += MpBonusDamage;
			base.spellCfg.duration += MpBonusDuration;
		}
	}

	private void DrainMp()
	{
		base.shooterWand.CostMp(base.shooterWand.CurrentMP);
	}

	private void UpdateCurrentPos()
	{
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			Vector2 vector = GameMgr.Inst.cameCtrller.cam_Main.ViewportToWorldPoint(new Vector2(0f, 0f));
			Vector2 vector2 = GameMgr.Inst.cameCtrller.cam_Main.ViewportToWorldPoint(new Vector2(1f, 1f));
			if (base.transform.position.x <= vector.x - base.spellCfg.radius - 0.5f)
			{
				((Spell1029Effect)EffectBase).SpawnNewTrail();
				base.transform.position = new Vector3(vector2.x + base.spellCfg.radius, base.transform.position.y, base.transform.position.z);
				SpawnTravelEffect();
			}
			else if (base.transform.position.x >= vector2.x + base.spellCfg.radius + 0.5f)
			{
				((Spell1029Effect)EffectBase).SpawnNewTrail();
				base.transform.position = new Vector3(vector.x - base.spellCfg.radius, base.transform.position.y, base.transform.position.z);
				SpawnTravelEffect();
			}
			else if (base.transform.position.y <= vector.y - base.spellCfg.radius - 0.5f)
			{
				((Spell1029Effect)EffectBase).SpawnNewTrail();
				base.transform.position = new Vector3(base.transform.position.x, vector2.y + base.spellCfg.radius, base.transform.position.z);
				SpawnTravelEffect();
			}
			else if (base.transform.position.y >= vector2.y + base.spellCfg.radius + 0.5f)
			{
				((Spell1029Effect)EffectBase).SpawnNewTrail();
				base.transform.position = new Vector3(base.transform.position.x, vector.y - base.spellCfg.radius, base.transform.position.z);
				SpawnTravelEffect();
			}
		}
	}

	private void SpawnTravelEffect()
	{
		SpellEffectBase effectBase = EffectBase;
		Vector3? position = base.transform.position;
		Vector3? rotation = base.Direction;
		effectBase.ManualCreateEffect("Travel", null, position, rotation);
		PlaySE("Travel", 0.2f);
	}

	protected override bool TryFallingRebound()
	{
		if (base.DurationTimer > base.spellCfg.duration)
		{
			return false;
		}
		EffectBase.ManualCreateEffect("Disappear");
		((Spell1029Effect)EffectBase).RecycleTrail();
		base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, (0f - FallInitialHeight) * 2f);
		CorrectLayerOnce();
		PlaySE("Travel", 0.2f);
		((Spell1029Effect)EffectBase).SpawnNewTrail();
		return true;
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (!base.SIP.spellIsFall)
		{
			base.CreateHitEffect(position, rotation);
		}
	}

	private void OnDisable()
	{
		((Spell1029Effect)EffectBase).RecycleTrail();
	}
}
