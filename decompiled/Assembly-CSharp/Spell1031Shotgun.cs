using UnityEngine;

public class Spell1031Shotgun : SpellBase
{
	public static int HitCount;

	private static float ShotgunAroundOwnerAngle;

	public float minAngle;

	public ShockParam shock;

	public float BaseMaxRange;

	public float BaseRangeUPPerLevel;

	private float finalShootRange;

	private float traveledDistance;

	public float BonusSpeedToRangeRatio;

	public float BonusDurationToRangeRatio;

	private Vector3 lastFramePosition;

	public float MinSEPlayInterval;

	private bool hadShootShock;

	protected override float FallingReboundForce => Mathf.Max(base.CurrentUpSpeed * -0.6f, 0.2f * InFallingReboundingGravity);

	public override void InitializeCallback()
	{
		hadShootShock = false;
		finalShootRange = (BaseMaxRange + BaseRangeUPPerLevel * (float)base.spellCfg.level) * base.SIP.finalSpeedRatio + base.SIP.bounsSpeed * BonusSpeedToRangeRatio + base.SIP.extraDuration * base.SIP.finalDurationRatio * BonusDurationToRangeRatio;
		traveledDistance = 0f;
		lastFramePosition = base.transform.position;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellAroundOwnerCurrentAngle = ShotgunAroundOwnerAngle + 360f / (float)base.spellCfg.shootCount * (float)base.InitialParameter.inSpellShootIndex;
		}
		if (base.SIP.inSpellShootIndex == 0 && !base.spellCfg.isSplitSpell)
		{
			SEMgr inst = SEMgr.Inst;
			int abilityType = (int)base.spellCfg.abilityType;
			inst.PlaySE("SE_Spell" + abilityType + "DirectShoot", SEPlayMode.Replay, 5, MinSEPlayInterval);
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		float num = Mathf.Max(base._angle, minAngle);
		if (!base.spellCfg.isSplitSpell)
		{
			base.Direction = Tool2D.GetDir(base.originalShootDirection, (0f - num) / 2f + num / (float)(base.spellCfg.shootCount - 1) * (float)base.InitialParameter.inSpellShootIndex) * IsReverseDirection();
		}
		ApplySpeedToVelocity();
	}

	public override void Update()
	{
		base.Update();
		if (base.shouldCameraShock && !hadShootShock)
		{
			hadShootShock = true;
			CamController.Inst.SetShock(shock, -base.Direction);
		}
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation && !base.SIP.spellIsFall)
		{
			traveledDistance += Tool2D.IgnoreZDistance(lastFramePosition, base.transform.position);
			lastFramePosition = base.transform.position;
			if (traveledDistance >= finalShootRange)
			{
				base.DurationTimer = base.spellCfg.duration;
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration) || base.SIP.spellIsFall)
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			if (!base.SIP.spellIsFall)
			{
				EffectBase.ManualCreateEffect("Hit", 1f, tsf_Layer.position, base.Direction);
			}
			base.isFlyFinish = true;
			rigid.linearVelocity *= 0.001f;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime * 2f);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (!IsSameCamp(unit))
		{
			HitCount++;
		}
		return base.OnHitUnit(unit);
	}

	protected override float GetLowFpsSpellSplitCount(float countPower = 1f, float lowFPsThreshold = 40f)
	{
		return base.GetLowFpsSpellSplitCount(3f, 60f);
	}

	protected override bool OnAOEHitUnit(UnitProperty unit)
	{
		if (!IsSameCamp(unit))
		{
			HitCount++;
		}
		return base.OnAOEHitUnit(unit);
	}

	protected override void InitSpeedAndPosition()
	{
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.speed *= 0.7f;
			if (base.SIP.originShootSpatialInfo != null && base.SIP.finalShootSpatialInfo != null)
			{
				base.SIP.finalShootSpatialInfo = base.SIP.originShootSpatialInfo;
			}
		}
		base.InitSpeedAndPosition();
	}

	protected override void OnFallingGround()
	{
		PlaySE("Hit");
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}
}
