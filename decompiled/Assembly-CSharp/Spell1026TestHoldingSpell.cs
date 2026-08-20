using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1026TestHoldingSpell : SpellBase, IHoldingSpell, IOnLaunchFromSpellEventHandle, IOnLaunchFromUnitNotPlayer, IOnSpellHijack
{
	private bool shootFromTrigger;

	private bool timeToEndHolding;

	public float shootRecoilPerStage;

	private static readonly List<UnitProperty> unitWhoTakeKnockBackInThisFrameList = new List<UnitProperty>();

	public bool ignorePointUpdate { get; set; }

	public bool IsHolding { get; set; }

	public bool IsSkipHolding { get; set; }

	public float HoldingTime { get; set; }

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		shootFromTrigger = false;
		timeToEndHolding = false;
		ignorePointUpdate = false;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (FromEcho)
		{
			StopHolding();
		}
	}

	public override void Update()
	{
		base.Update();
		if (!IsHolding && !base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		CheckIfNeedShootEarlier();
		if (!IsHolding && !base.SIP.spellIsFall && base.DurationTimer > base.spellCfg.duration)
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
			else
			{
				PoolRecycle();
			}
		}
	}

	private void CheckIfNeedShootEarlier()
	{
		if (!ownerPpt.gameObject.activeInHierarchy)
		{
			timeToEndHolding = true;
		}
	}

	public IEnumerator AutoFire(float time)
	{
		yield return new WaitForSeconds(time);
		timeToEndHolding = true;
	}

	private void UpdatePoint()
	{
		if (ignorePointUpdate || base.isFlyFinish || base.spellCfg.isSplitSpell || shootFromTrigger || base.OwnerSpell != null || base.indirectShootByPlayer)
		{
			return;
		}
		if (isOwnerSpellValid())
		{
			base.transform.position = base.OwnerSpell.transform.position;
		}
		else if (ownerPpt.UnitBas is Teammate52)
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.OwnerTsf.transform.position);
		}
		else if (base.OwnerTsf != null)
		{
			base.transform.position = base.OwnerTsf.transform.position;
		}
		else if (ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			if ((bool)base.shooterWand)
			{
				base.transform.position = base.shooterWand.GetShootPosition();
			}
			else
			{
				base.transform.position = PlayerMgr.Inst.ShootPoint;
			}
		}
		else
		{
			if (!isOwnerpptValid())
			{
				return;
			}
			base.transform.position = Tool2D.IgnoreZPoint(ownerPpt.transform.position);
		}
		UpdateRepeatShootPos();
	}

	private void UpdateFallingPoint()
	{
		if (ignorePointUpdate || base.isFlyFinish || base.spellCfg.isSplitSpell || shootFromTrigger || base.OwnerSpell != null)
		{
			return;
		}
		if (ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			base.SIP.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(PlayerMgr.Inst.PlayerCtrller.transform.position, PlayerMgr.Inst.GetMousePoint());
			base.Direction = base.SIP.finalShootSpatialInfo.Direction;
			float speed = base.spellCfg.speed;
			InitSpeedAndPosition();
			base.spellCfg.speed = speed;
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				UpdateFallingPointWithAround();
			}
		}
		else if (isOwnerpptValid())
		{
			base.SIP.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(ownerPpt.transform.position, base.SIP.finalShootSpatialInfo.Target.Value);
			base.Direction = base.SIP.finalShootSpatialInfo.Direction;
			float speed2 = base.spellCfg.speed;
			InitSpeedAndPosition();
			base.spellCfg.speed = speed2;
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				UpdateFallingPointWithAround();
			}
		}
		UpdateRepeatShootPos();
	}

	private void UpdateFallingPointWithAround()
	{
		float height = base.Height;
		SpellAroundPlayer();
		base.Height = height;
	}

	private void ApplyRecoilToOwnerUnit()
	{
		if (!base.isFlyFinish && !base.spellCfg.isSplitSpell && !shootFromTrigger && !(base.OwnerSpell != null) && !base.indirectShootByPlayer)
		{
			float num = 1f;
			if (IsSameCamp(UnitType.Player))
			{
				num = SpellShootGroupExtend.GetRecoilRatio(base.shooterWand).CurrentMulRatio;
			}
			if (!unitWhoTakeKnockBackInThisFrameList.Contains(ownerPpt))
			{
				unitWhoTakeKnockBackInThisFrameList.Add(ownerPpt);
				ownerPpt.TakeKnockback(-base.Direction * (shootRecoilPerStage * (float)CalculateCurrentStage() * num));
			}
		}
	}

	public int CalculateCurrentStage()
	{
		if (base.ShootData == null)
		{
			return 1;
		}
		return SpellChargeStageCalculator.Spell1026(new SpellChargeStageCalculator.Param(base.ShootData, HoldingTime, base.shooterWand));
	}

	private void UpdateRepeatShootPos()
	{
		base.transform.position += Tool2D.IgnoreZPoint(Tool2D.GetDir(base.Direction, -90f) * (((0f - ((float)base.InitialParameter.multiShootCount - 1f)) / 2f + (float)base.InitialParameter.inMultiShootIndex) * base.InitialParameter.multiShootSpace));
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		unitWhoTakeKnockBackInThisFrameList.Clear();
	}

	private void UpdateDirection()
	{
		if (!base.spellCfg.isSplitSpell && (!(base.OwnerSpell != null) || !isOwnerSpellValid()) && !base.indirectShootByPlayer && !(base.OwnerSpell != null))
		{
			if (ownerPpt.unitCfg.unitType == UnitType.Player)
			{
				base.Direction = Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir));
			}
			else if (base.OwnerTsf != null)
			{
				base.Direction = base.OwnerTsf.right;
			}
		}
		base.Direction *= IsReverseDirection();
		base.transform.right = base.Direction;
	}

	public void StartHolding()
	{
		PlayerSpellRegisterKeepCastingBuff(decreaseMoveSpeed: false);
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		triggerIn.colliderObject.enabled = false;
	}

	public void StopHolding()
	{
		base.overalCriticalChance += GetCriticalChanceByHoldingTime();
		IsHolding = false;
		base.enableAroundPlayer = true;
		enableFollowMouse = true;
		enableFollowTarget = true;
		base.DurationTimer = 0f;
		if (base.SIP.equalScatter)
		{
			base.Direction = GetEqualScatterMultipleShootInitialDirection(base.Direction);
		}
		else
		{
			base.Direction = Tool2D.GetDir(base.Direction, Random.Range((0f - base.wandShootAngle) / 2f, base.wandShootAngle / 2f));
		}
		base.transform.right = base.Direction;
		triggerIn.colliderObject.enabled = true;
		PlayerSpellUnRegisterKeepCastingBuff();
		InitSpeedAndPosition();
		if (base.SIP.spellIsFall)
		{
			UpdateRepeatShootPos();
		}
		ApplySpeedToVelocity();
		if (!base.SIP.spellIsFall)
		{
			ApplyRecoilToOwnerUnit();
		}
		if (base.overalCriticalChance >= base.spellCfg.float3 / 100f && IsSameCamp(UnitType.Player))
		{
			base.spellCfg.damage = Mathf.Ceil(base.spellCfg.damage * base.spellCfg.float2 / 100f);
		}
		((Spell1026Effect)EffectBase).CreateTrail();
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			((Spell1026Effect)EffectBase).CreateShootEffect(Tool2D.GetLayerPoint(base.transform.position));
		}
		PlaySE("ShootStar");
	}

	public void HoldingUpdate()
	{
		if (base.SIP.spellIsFall)
		{
			UpdateFallingPoint();
		}
		else
		{
			UpdatePoint();
			UpdateDirection();
		}
		HoldingTime += Time.deltaTime;
	}

	public bool HoldingCondition()
	{
		if (base.OwnerSpell != null || shootFromTrigger || base.spellCfg.isSplitSpell || timeToEndHolding || base.InitialParameter.shootFromPostSlots || base.indirectShootByPlayer)
		{
			return false;
		}
		if (!IsSameCamp(UnitType.Player))
		{
			return true;
		}
		if (ownerPpt.unitCfg.unitType != 0 && base.DurationTimer >= 5f)
		{
			return false;
		}
		if (ownerPpt.unitCfg.unitType == UnitType.Player && !PlayerMgr.Inst.PlayerCtrller.isHoldMouse0)
		{
			return false;
		}
		return true;
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		((Spell1026TestHoldingSpell)spellBase).HoldingTime = HoldingTime;
		return spellBase;
	}

	private float GetCriticalChanceByHoldingTime()
	{
		return base.spellCfg.float1 / 100f * HoldingTime;
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		shootFromTrigger = triggerOrNull != null;
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		float time = Mathf.Max(0f, (base.spellCfg.float3 / 100f - base.overalCriticalChance) / (base.spellCfg.float1 / 100f));
		if (unit is Teammate5 teammate)
		{
			base.OwnerTsf = teammate.GetShootPositionTranform();
			StartCoroutine(AutoFire(time));
		}
		else if (unit is Teammate5FuseController teammate5FuseController)
		{
			base.OwnerTsf = teammate5FuseController.GetShootPositionTranform();
			StartCoroutine(AutoFire(time));
		}
		else if (unit is Teammate52 teammate2)
		{
			base.OwnerTsf = teammate2.shootPosition;
			if (teammate2.targetWand.passiveChargeCountLimit > 0)
			{
				StartCoroutine(AutoFire(0f));
			}
			else
			{
				StartCoroutine(AutoFire(time));
			}
		}
	}

	public void IOnSpellHijack(UnitProperty onwerPpt)
	{
		StopHolding();
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		((Spell1026Effect)EffectBase).CreateHitEffect(base.transform.position);
	}
}
