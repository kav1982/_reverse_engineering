using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1027SuperNova : SpellBase, IHoldingSpell, IOnLaunchFromSpellEventHandle, IOnLaunchFromUnitNotPlayer, IOnSpellHijack
{
	private bool shootFromTrigger;

	private bool timeToEndHolding;

	public ShockParam shockParam;

	public List<float> effectStageThreshold;

	public float AfterShootDelayRecycleTime;

	public AudioSource LoopAS;

	public AudioSource StartAS;

	private bool readyToExplode;

	private static readonly Collider[] detectTargetBuffer = new Collider[256];

	public LayerMask detectLayer;

	public GameObject SelfShadow;

	private static readonly HashSet<string> detectTags = new HashSet<string> { "Monster", "Destructible", "SolidObj", "RollBall", "Butterfly" };

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
		base.CanBeCapture = false;
		LoopAS.enabled = !base.spellCfg.isSplitSpell && base.SIP.inMultiShootIndex == 0;
		StartAS.enabled = base.SIP.inMultiShootIndex == 0;
		SoundVolumeChange();
		if (base.SIP.spellIsFall)
		{
			UpdateFallingPoint();
		}
		SelfShadow.SetActive(base.SIP.spellIsFall);
	}

	private void SoundVolumeChange()
	{
		StartAS.volume = DataMgr.settingData.GetFinalSound();
		LoopAS.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (!base.SIP.spellIsFall)
		{
			enableFollowMouse = false;
			base.enableAroundPlayer = false;
			enableFollowTarget = false;
			base.currentSpellMovement = SpellSpecialMovementType.Normal;
		}
		else
		{
			EffectBase.ManualCreateEffect("FallStarTrail");
		}
		if (!base.spellCfg.isSplitSpell && !base.OwnerSpell)
		{
			EffectBase.ManualCreateEffect("Charge");
		}
		if (ownerPpt.unitCfg.unitType == UnitType.Player && !base.OwnerSpell && !base.spellCfg.isSplitSpell && !FromEcho && !base.indirectShootByPlayer)
		{
			PlayerMgr.Inst.PlayerCtrller.CastLockRegister();
		}
		if (FromEcho)
		{
			StopHolding();
		}
	}

	public override void SingleInitialCallback()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		base.SingleInitialCallback();
	}

	private void OnDestroy()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	public override void Update()
	{
		base.Update();
		CheckIfNeedShootEarlier();
		SelfShadow.transform.localPosition = new Vector3(0f, 0f, 900f);
		if (!IsHolding && base.DurationTimer > base.spellCfg.duration)
		{
			PoolRecycle();
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!IsHolding && base.SIP.spellIsFall)
		{
			HoldingTime += Time.deltaTime;
		}
		if (readyToExplode)
		{
			readyToExplode = false;
			Explosion();
		}
	}

	protected override void MakeFallingGroundDamageToAround(Vector3? position = null)
	{
		Explosion();
	}

	private void Explosion()
	{
		HoldingTime = Mathf.Min(6f, HoldingTime);
		int num = GetCurrentChargeLevel();
		if (base.spellCfg.isSplitSpell)
		{
			num--;
		}
		num = Mathf.Max(num, 1);
		num = ((!GeneralTool.IsLowFpsOptimizeActive(100f) || !(GameMgr.Inst.GetFps() / 30f <= UnityEngine.Random.Range(0.33f, 1f))) ? num : 0);
		((Spell1027Effect)EffectBase).SetEffectLevel(num);
		base.transform.right = base.Direction;
		if (!base.SIP.spellIsFall)
		{
			PlayerSpellUnRegisterKeepCastingBuff();
			EffectBase.ManualRecycleEffect("Charge");
		}
		base.spellCfg.damage = Mathf.Ceil(SpellConfig.dic[base.spellCfg.id].damage * base.damageRatio * base.finalDamageRatio * (1f + base.spellCfg.float1 / 100f * HoldingTime) + base.SIP.finalDamageExtra);
		SpellEffectBase effectBase = EffectBase;
		Vector3? position = base.transform.position;
		effectBase.ManualCreateEffect("Explosion", null, position);
		DealDamageToAllEnemyInMap();
		ScreenShake();
		PlaySE("Explosion" + GetCurrentChargeLevel());
		LoopAS.enabled = false;
		StartAS.enabled = false;
		if (!base.SIP.spellIsFall)
		{
			StartCoroutine(DelayRecycle(AfterShootDelayRecycleTime));
		}
	}

	protected override void OnFallingGround()
	{
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			EffectBase.ManualRecycleEffect("Charge");
			EffectBase.ManualRecycleEffect("FallStarTrail");
			SelfShadow.SetActive(value: false);
			StartCoroutine(DelayRecycle(AfterShootDelayRecycleTime));
		}
	}

	private void CheckIfNeedShootEarlier()
	{
		if (!ownerPpt.gameObject.activeInHierarchy || HoldingTime >= base.spellCfg.float2)
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
				base.transform.position = base.shooterWand.GetShootPosition() + new Vector3(0f, 0f, -0.3f);
			}
			else
			{
				base.transform.position = PlayerMgr.Inst.ShootPoint + new Vector3(0f, 0f, -0.3f);
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

	private void UpdateRepeatShootPos()
	{
		base.transform.position += Tool2D.IgnoreZPoint(Tool2D.GetDir(base.Direction, -90f) * (((0f - ((float)base.InitialParameter.multiShootCount - 1f)) / 2f + (float)base.InitialParameter.inMultiShootIndex) * base.InitialParameter.multiShootSpace));
	}

	private void DealDamageToAllEnemyInMap()
	{
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, 30f, detectTargetBuffer, detectTags, detectLayer);
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			AOETriggerIn(detectTargetBuffer[i]);
		}
		foreach (UnitProperty item in LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Copy())
		{
			if (item.CanBeTarget)
			{
				OnAOEHitUnit(item);
			}
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		Vector3 value = Tool2D.IgnoreZPoint((unit.transform.position - base.transform.position).normalized);
		Vector3 value2 = unit.transform.position + new Vector3(0f, 0.3f, 0f);
		if ((!base.spellCfg.isSplitSpell && base.SIP.inMultiShootIndex == Mathf.RoundToInt((float)base.SIP.multiShootCount / 2f)) || (base.spellCfg.isSplitSpell && base.SIP.SplitIndex == 0))
		{
			SpellEffectBase effectBase = EffectBase;
			Vector3? position = value2;
			Vector3? rotation = value;
			effectBase.ManualCreateEffect("Hit", null, position, rotation);
		}
		return base.MakeDamageToUnit(unit);
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
		PlayerSpellRegisterKeepCastingBuff();
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		readyToExplode = false;
	}

	public void StopHolding()
	{
		IsHolding = false;
		base.enableAroundPlayer = true;
		enableFollowMouse = true;
		enableFollowTarget = true;
		base.DurationTimer = 0f;
		if (!base.SIP.spellIsFall)
		{
			readyToExplode = true;
		}
		if (base.SIP.spellIsFall)
		{
			PlayerSpellUnRegisterKeepCastingBuff();
			InitSpeedAndPositionWithFall();
			if (base.currentSpellMovement == SpellSpecialMovementType.Normal || base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
			{
				rigid.linearVelocity = base.Direction * base.CurrentSpeed;
			}
		}
	}

	private IEnumerator DelayRecycle(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		PoolRecycle();
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
		base.spellCfg.radius = 4f;
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		((Spell1027SuperNova)spellBase).HoldingTime = HoldingTime;
		return spellBase;
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		shootFromTrigger = triggerOrNull != null;
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		float @float = base.spellCfg.float2;
		if (unit is Teammate5 teammate)
		{
			base.OwnerTsf = teammate.GetShootPositionTranform();
			StartCoroutine(AutoFire(@float));
		}
		else if (unit is Teammate5FuseController teammate5FuseController)
		{
			base.OwnerTsf = teammate5FuseController.GetShootPositionTranform();
			StartCoroutine(AutoFire(@float));
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
				StartCoroutine(AutoFire(@float));
			}
		}
	}

	public void IOnSpellHijack(UnitProperty onwerPpt)
	{
		StopHolding();
	}

	private void ScreenShake()
	{
		ShockParam shockParam = this.shockParam;
		float num = Mathf.Min(2f, HoldingTime * 0.4f);
		if (ownerPpt.unitCfg.unitType != 0 || (bool)base.OwnerSpell)
		{
			num *= 0.3f;
		}
		shockParam.radius *= 1f + num;
		shockParam.speed *= 1f + num * 0.4f;
		shockParam.time *= 1f + num * 0.2f;
		CamController.Inst.SetShock(shockParam, new Vector3(0f, -1f, 0f));
	}

	public int GetCurrentChargeLevel()
	{
		foreach (float item in effectStageThreshold)
		{
			if (HoldingTime < item)
			{
				return effectStageThreshold.IndexOf(item);
			}
		}
		if (!base.spellCfg.isSplitSpell)
		{
			return 4;
		}
		return 3;
	}
}
