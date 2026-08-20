using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;

public class Wand : LayerCorrect
{
	public enum UnusedEnhanceType
	{
		LeftNoSpell,
		RightNoSpell
	}

	[Space(50f)]
	public Transform tsf_ShootPoint;

	public Animator animator;

	public Transform WandBladeCenterTransform;

	[Header("Wand")]
	public Transform tsf_WandRoot;

	public SpriteRenderer sr_Wand;

	public SpriteRenderer sr_WandShadow;

	public Transform tsf_SpecialWandRoot;

	public SpriteRenderer sr_SpecialWand;

	public SpriteRenderer sr_SpecialWandShadow;

	[Header("Hand")]
	public SpriteRenderer sr_Hand;

	public Sprite sprite_HandNormal;

	public Sprite sprite_HandTVMan;

	public Sprite sprite_HandFrog;

	public Sprite sprite_HandTap;

	public Sprite sprite_HandSpring;

	public static bool wandHoldingFlyEffectApplying;

	private float _currentMp;

	private bool rechecked;

	private float lastShowNoSpellTryShootUITime;

	private float passiveBonusMaxMp;

	[HideInInspector]
	public SpellSpecialMovementType spellFinalMovementType;

	[HideInInspector]
	public float passiveRandomMinRotateRadiuRatio = 1f;

	[HideInInspector]
	public float passiveRandomMaxRotateRadiuRatio = 1f;

	[HideInInspector]
	public float passiveWandMaxMpRatio = 1f;

	private float passiveBonusMpGen;

	[HideInInspector]
	public float passiveDamageRatio = 1f;

	[HideInInspector]
	public bool passiveChargeEnable;

	[HideInInspector]
	public int passiveChargeCountLimit;

	private float passiveChargeInterval = float.PositiveInfinity;

	[HideInInspector]
	public bool passiveAutoWand;

	[HideInInspector]
	public Entity PassiveWandSpiritEntity = Entity.Null;

	private float passiveMpGenRatio = 1f;

	[HideInInspector]
	public float passiveMpCostCorrection = 1f;

	[HideInInspector]
	public AutoWandData passiveAutoWandShooterData;

	[HideInInspector]
	public float passiveWandCoolDownAddSubAmount;

	[HideInInspector]
	public float passiveWandCoolDownTimeRatio = 1f;

	[HideInInspector]
	public float passiveWandShootIntervalAddSubAmount;

	[HideInInspector]
	public float passiveEchoShootChance;

	[HideInInspector]
	public float passiveEchoFreeShootChance;

	[HideInInspector]
	public bool passiveEqualAngleDistribution;

	[HideInInspector]
	public bool passiveUmbrellaEnable;

	[HideInInspector]
	public bool passiveUmbrellaMpFull;

	[HideInInspector]
	public List<GameObject> passiveLaserCrystal = new List<GameObject>();

	[HideInInspector]
	public List<Spell4019BiAnLethalBlade> passiveBiAnBlade = new List<Spell4019BiAnLethalBlade>();

	[HideInInspector]
	public int passiveBiAnBladeShootCount = 1;

	[HideInInspector]
	public int passiveBiAnBladeTotalCount = 1;

	[HideInInspector]
	public int DaveHarpoonsSpellCount;

	[HideInInspector]
	public int FinalShootCount;

	[HideInInspector]
	public bool passiveRuneHammerEnable;

	[HideInInspector]
	public bool passiveLaserCrystalEnable;

	[FormerlySerializedAs("BiAnBladeEnable")]
	[HideInInspector]
	public bool passiveBiAnBladeEnable;

	[HideInInspector]
	public bool passiveDaveHarpoonsEnable;

	[HideInInspector]
	public int ExtraNormalSlot;

	[HideInInspector]
	public int ExtraPostSlot;

	[HideInInspector]
	public bool PassiveTransStoneEnable;

	[HideInInspector]
	public bool passiveRandomPosShoot;

	private float RandomPosShootAngleShift;

	private float RandomPosShootAngleShiftPerShoot;

	private float RandomPosShootRadius;

	private float passiveManaToPostRatio;

	private bool passiveEnablaManaRegen = true;

	private Entity spellBreakerEntity = Entity.Null;

	[HideInInspector]
	public int passiveRedRuneCount;

	[HideInInspector]
	public int passiveGreenRuneCount;

	[HideInInspector]
	public int passiveBlueRuneCount;

	private float redRuneShootCounter;

	private bool isTriggerRedRuneInThisFrame;

	private float redRuneCoolDown;

	private float passiveGreenRuneChargeTimer;

	private int greenRuneLV5SummonCount;

	private SpellShootGroup GreenRuneGroup;

	[HideInInspector]
	public List<Entity> GreenRuneList = new List<Entity>();

	private float blueRuneCharge;

	private int blueRuneRemainCount;

	private int blueRuneTriggerCounter;

	private SpellShootGroup blueRuneGroup;

	[HideInInspector]
	public float LaserCrystalPowerRatio = 1f;

	[HideInInspector]
	public int LaserCrystalFinalSplitCount;

	[HideInInspector]
	public int LaserCrystalFinalMultiCount;

	[HideInInspector]
	public int LaserCrystalCount;

	[HideInInspector]
	public float LaserCrystalThunderChance;

	private float healNearbyTeammateTimer;

	private float autoFullManaTimer;

	public List<SpellShootGroup> shootGroups = new List<SpellShootGroup>();

	[HideInInspector]
	public List<SpellShootGroup> postSlotShootGroups = new List<SpellShootGroup>();

	private float maxManaInLastFrame;

	private Spell4004ChargeController chargeAura;

	public readonly List<Spell4004ChargeStars> ChargeStars = new List<Spell4004ChargeStars>();

	private int lastCreateHammerFrame;

	private int lastCreateLaserCrystalFrame;

	private int lastCreateBiAnBladeFrame;

	private Entity _bladeWandDataEntity;

	private int lastCreateUmbrellaFrame;

	private Entity _listenerEntity;

	private int currentSpellGroupsIndexMobile;

	private float refreshInterval = 0.8f;

	private float refreshTimer;

	public int WandIndex { get; private set; }

	public WandConfig WandCfg
	{
		get
		{
			if (PlayerMgr.Inst.BaData == null)
			{
				return null;
			}
			if (WandIndex >= PlayerMgr.Inst.BaData.wandCfgs.Count)
			{
				return null;
			}
			return PlayerMgr.Inst.BaData.wandCfgs[WandIndex];
		}
	}

	public float MaxMP
	{
		get
		{
			if (WandCfg == null)
			{
				return 0f;
			}
			int num = WandCfg.maxMP;
			if (PlayerMgr.Inst.BaData != null)
			{
				num += PlayerMgr.Inst.BaData.mpMax;
			}
			num += Mathf.RoundToInt(passiveBonusMaxMp);
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_MaxMP != null)
			{
				num += PlayerMgr.Inst.ItemCtrller.relicCfg_MaxMP.int1.result;
			}
			if (PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.TryGetValue(3, out var value))
			{
				num += value.int1.result;
			}
			if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
			{
				num += PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard.GetRuneWizardSetBonusMP();
			}
			float num2 = 1f;
			num2 *= PlayerMgr.Inst.MaxMpRatioFromWandAbility();
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_LoseMPLimit != null)
			{
				num2 *= 1f - (float)PlayerMgr.Inst.ItemCtrller.curseCfg_LoseMPLimit.int1.result / 100f;
			}
			num2 *= passiveWandMaxMpRatio;
			num = Mathf.CeilToInt((float)num * num2);
			return num;
		}
	}

	public float CurrentMP
	{
		get
		{
			return _currentMp;
		}
		set
		{
			if (passiveUmbrellaMpFull && (value < UmbrellaShieldController.GetWandOneHpAsMp(MaxMP) || MaxMP <= 0f))
			{
				passiveUmbrellaMpFull = false;
			}
			if (!passiveUmbrellaMpFull && value >= MaxMP && MaxMP > 0f)
			{
				passiveUmbrellaMpFull = true;
			}
			_currentMp = Mathf.Clamp(value, 0f, MaxMP);
		}
	}

	public float ShootIntervalTimer { get; set; }

	public float CoolingTimer { get; set; }

	public float WandShootInterval
	{
		get
		{
			float num = 0f;
			if (WandCfg != null)
			{
				num = WandCfg.shootInterval;
			}
			num += passiveWandShootIntervalAddSubAmount;
			num = Mathf.Max(num, 0f);
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_SlowWand != null && num > 0f)
			{
				num *= 1f + (float)PlayerMgr.Inst.ItemCtrller.curseCfg_SlowWand.int1.result / 100f;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus != null && num > 0f)
			{
				num *= 1f - PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus.Cfg.floatTimer;
			}
			return num;
		}
	}

	public float WandCoolDown
	{
		get
		{
			float num = 0f;
			if (WandCfg != null)
			{
				num = WandCfg.coolDown;
			}
			num += passiveWandCoolDownAddSubAmount;
			num = Mathf.Max(0f, num);
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_SlowWand != null && num > 0f)
			{
				num *= 1f + (float)PlayerMgr.Inst.ItemCtrller.curseCfg_SlowWand.int1.result / 100f;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus != null && num > 0f)
			{
				num *= 1f - PlayerMgr.Inst.ItemCtrller.relic_StayAndFocus.Cfg.floatTimer;
			}
			return num * passiveWandCoolDownTimeRatio;
		}
	}

	[HideInInspector]
	public float passiveOwnerSpeedUpRatio { get; set; }

	public bool EnableOverFlowCriticalChanceConvertToDamage
	{
		get
		{
			if (passiveRedRuneCount > 0)
			{
				return PlayerMgr.Inst.GetRuneEffectLevel(PlayerMgr.Inst.GetPlayerRuneCount().RedRune) >= 3;
			}
			return false;
		}
	}

	public SpellShootGroup currentShootGroup
	{
		get
		{
			if (shootGroups.Count != 0)
			{
				return shootGroups[currentSpellGroupsIndex];
			}
			return null;
		}
	}

	private int currentSpellGroupsIndex { get; set; }

	private float PostSlotCurrentCharge { get; set; }

	private float PostSlotMaxCharge { get; set; }

	public bool IsCharging => chargeAura;

	public Vector3 ShootPosition => GetShootPosition();

	public Vector3 ShootTargetPosition
	{
		get
		{
			if (!passiveAutoWand)
			{
				return PlayerMgr.Inst.GetMousePoint();
			}
			return passiveAutoWandShooterData.wandObjectScript.lastFrameTargetPosition;
		}
	}

	public Vector3 ShootDirection
	{
		get
		{
			if (!passiveAutoWand || passiveAutoWandShooterData == null)
			{
				return PlayerMgr.Inst.PlayerDir;
			}
			return passiveAutoWandShooterData.shootDirection;
		}
	}

	public SpellShootGroup currentShootGroupMobile
	{
		get
		{
			if (shootGroups.Count != 0)
			{
				return shootGroups[currentSpellGroupsIndexMobile];
			}
			return null;
		}
	}

	public Vector3 GetShootPosition()
	{
		if (tsf_ShootPoint == null)
		{
			return PlayerMgr.Inst.PlayerPoint;
		}
		Vector3 result = ((passiveAutoWand && passiveAutoWandShooterData != null) ? passiveAutoWandShooterData.shootPosition : tsf_ShootPoint.position);
		WandConfig wandCfg = WandCfg;
		bool flag;
		if (wandCfg != null)
		{
			WandAbility specialAbility = wandCfg.specialAbility;
			if ((uint)(specialAbility - 25) <= 1u)
			{
				flag = true;
				goto IL_0060;
			}
		}
		flag = false;
		goto IL_0060;
		IL_0060:
		if (flag)
		{
			result += ShootDirection * WandCfg.float1 * 0.9f * PlayerMgr.Inst.BaData.bodySize;
		}
		return result;
	}

	private void Update()
	{
		ShootIntervalTimer -= PlayerMgr.Inst.PlayerDeltaTime;
		CoolingTimer -= PlayerMgr.Inst.PlayerDeltaTime;
		if (CurrentMP < MaxMP && passiveEnablaManaRegen)
		{
			CurrentMP += GetWandMpRecoverSpeed() * PlayerMgr.Inst.PlayerDeltaTime;
			if (CurrentMP > MaxMP)
			{
				CurrentMP = MaxMP;
			}
		}
		if (WandCfg == null)
		{
			return;
		}
		ChargeControlUpdate();
		PostSlotTriggerStateUpdate();
		if (redRuneCoolDown >= 0f)
		{
			redRuneCoolDown -= Time.deltaTime;
		}
		UpdateGreenRuneChargeState();
		UpdateBlueRuneChargeState();
		if (WandCfg != null)
		{
			if (PlayerMgr.Inst.WandCheckSlotCount(WandIndex))
			{
				ResetAndRecheck();
			}
			if (Math.Abs(maxManaInLastFrame - MaxMP) > 0.01f)
			{
				maxManaInLastFrame = MaxMP;
				ResetAndRecheck();
			}
		}
		UpdateSpellBreakerHeadTransform();
		UI_WandManaPercentUpdate();
		UI_PostSlotIconPercentUpdate();
		UI_UpdateNoManaWarning();
		UI_UpdateUnusedWarning();
		UI_UpdateWandFlipState();
		if (GameMgr.IsMobile_Static)
		{
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				UI_UpdatePreShootHintMobile();
			}
		}
		else
		{
			UI_UpdatePreShootHint();
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		UpdateShadowPosition();
		if (!passiveAutoWand)
		{
			WandPostSlotTrigger.PostSlotStandTriggerCheck(this);
		}
		if (isTriggerRedRuneInThisFrame)
		{
			isTriggerRedRuneInThisFrame = false;
			redRuneCoolDown = 0.08f;
		}
		UpdateTeammateHealTimer();
		UpdatePeriodAutoFullManaTimer();
		bool flag = false;
		for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
		{
			if (PlayerMgr.Inst.SelectedWand == PlayerMgr.Inst.Wands[i] && PlayerMgr.Inst.Wands[i] != null && PlayerMgr.Inst.Wands[i].WandCfg != null && PlayerMgr.Inst.Wands[i].WandCfg.specialAbility == WandAbility.HoldingWandCharacterFly && !PlayerMgr.Inst.Wands[i].passiveAutoWand)
			{
				if (!wandHoldingFlyEffectApplying)
				{
					PlayerMgr.Inst.FlyRegister();
					wandHoldingFlyEffectApplying = true;
				}
				flag = true;
				break;
			}
		}
		if (!flag && wandHoldingFlyEffectApplying)
		{
			PlayerMgr.Inst.FlyUnregister();
			wandHoldingFlyEffectApplying = false;
		}
	}

	public void CheckWandManaRelatePassiveEffect()
	{
		passiveBonusMaxMp = 0f;
		passiveWandMaxMpRatio = 1f;
		passiveBonusMpGen = 0f;
		float passiveWandSpiritHighestManaGenSpeedRatio = 0f;
		Action<SlotData, SpellConfig, bool> action = delegate(SlotData slot, SpellConfig spell, bool isPostSlot)
		{
			if (isPostSlot)
			{
				ExtraPostSlot += spell.slotNumModifyValue;
			}
			else
			{
				ExtraNormalSlot += spell.slotNumModifyValue;
			}
			passiveWandCoolDownAddSubAmount += spell.coolDownAddSubRevise;
			passiveWandShootIntervalAddSubAmount += spell.shootIntervalAddSubRevise;
			switch (spell.abilityType)
			{
			case SpellAbilityType.EmptyContainer:
				passiveWandMaxMpRatio += spell.float1 / 100f;
				break;
			case SpellAbilityType.ManaEssence:
				passiveBonusMpGen += spell.float1;
				break;
			case SpellAbilityType.ChargeMode:
				passiveMpCostCorrection *= spell.mpCostMulDivCorrection / 100f;
				break;
			case SpellAbilityType.WandSpirit:
				passiveWandSpiritHighestManaGenSpeedRatio = Mathf.Max(passiveWandSpiritHighestManaGenSpeedRatio, spell.float1 / 100f);
				break;
			case SpellAbilityType.ManaTendril:
				passiveMpGenRatio *= 1f + spell.float1 / 100f * ((float)slot.specialInt + 1f);
				passiveWandMaxMpRatio += spell.float2 / 100f * ((float)slot.specialInt + 1f);
				break;
			case SpellAbilityType.Umbrella:
				passiveBonusMaxMp += (float)spell.int2;
				break;
			}
		};
		SlotData[] validSlotsData = WandCfg.GetValidSlotsData(normal: true, post: false);
		foreach (SlotData slotData in validSlotsData)
		{
			action(slotData, slotData.GetFinalConfig(), arg3: false);
		}
		validSlotsData = WandCfg.GetValidSlotsData(normal: false, post: true);
		foreach (SlotData slotData2 in validSlotsData)
		{
			action(slotData2, slotData2.GetFinalConfig(), arg3: true);
		}
		if (passiveAutoWand)
		{
			passiveMpGenRatio *= passiveWandSpiritHighestManaGenSpeedRatio;
		}
		CurrentMP = CurrentMP;
	}

	private void CheckWandSlotPassiveEffect(bool refreshAutoSpells = true)
	{
		passiveBonusMaxMp = 0f;
		passiveWandMaxMpRatio = 1f;
		passiveBonusMpGen = 0f;
		passiveChargeEnable = false;
		passiveChargeCountLimit = 0;
		passiveChargeInterval = float.PositiveInfinity;
		passiveMpCostCorrection = 1f;
		passiveMpGenRatio = 1f;
		passiveWandCoolDownTimeRatio = 1f;
		passiveWandCoolDownAddSubAmount = 0f;
		passiveWandShootIntervalAddSubAmount = 0f;
		ExtraNormalSlot = 0;
		ExtraPostSlot = 0;
		passiveDamageRatio = 1f;
		passiveEchoShootChance = 0f;
		passiveEchoFreeShootChance = 0f;
		passiveEqualAngleDistribution = false;
		bool flag = passiveAutoWand;
		passiveAutoWand = false;
		bool umbrellaEnableLastTime = passiveUmbrellaEnable;
		passiveUmbrellaEnable = false;
		passiveRuneHammerEnable = false;
		passiveLaserCrystalEnable = false;
		passiveBiAnBladeEnable = false;
		passiveDaveHarpoonsEnable = false;
		passiveBiAnBladeShootCount = 1;
		passiveOwnerSpeedUpRatio = 0f;
		DaveHarpoonsSpellCount = 0;
		float passiveWandSpiritHighestManaGenSpeedRatio = 0f;
		WandCfg.ResetPostSlot();
		PassiveTransStoneEnable = false;
		passiveRandomMinRotateRadiuRatio = 1f;
		passiveRandomMaxRotateRadiuRatio = 1f;
		spellFinalMovementType = SpellSpecialMovementType.Normal;
		passiveRandomPosShoot = false;
		RandomPosShootAngleShiftPerShoot = 0f;
		RandomPosShootRadius = 0f;
		passiveManaToPostRatio = 0f;
		passiveEnablaManaRegen = true;
		passiveRedRuneCount = 0;
		redRuneCoolDown = 0f;
		redRuneShootCounter = 0f;
		passiveGreenRuneCount = 0;
		passiveGreenRuneChargeTimer = 0f;
		greenRuneLV5SummonCount = 0;
		GreenRuneGroup = null;
		passiveBlueRuneCount = 0;
		blueRuneGroup = null;
		blueRuneCharge = 0f;
		blueRuneRemainCount = 0;
		blueRuneTriggerCounter = 0;
		FinalShootCount = 1;
		Action<SlotData, SpellConfig, bool> action = delegate(SlotData slot, SpellConfig spell, bool isPostSlot)
		{
			if (isPostSlot)
			{
				ExtraPostSlot += spell.slotNumModifyValue;
			}
			else
			{
				ExtraNormalSlot += spell.slotNumModifyValue;
			}
			passiveWandCoolDownAddSubAmount += spell.coolDownAddSubRevise;
			passiveWandShootIntervalAddSubAmount += spell.shootIntervalAddSubRevise;
			switch (spell.abilityType)
			{
			case SpellAbilityType.EmptyContainer:
				passiveWandMaxMpRatio += spell.float1 / 100f;
				break;
			case SpellAbilityType.ManaEssence:
				passiveBonusMpGen += spell.float1;
				break;
			case SpellAbilityType.ChargeMode:
				passiveChargeEnable = true;
				passiveMpCostCorrection *= spell.mpCostMulDivCorrection / 100f;
				passiveChargeCountLimit += spell.int1;
				passiveChargeInterval = Mathf.Min(passiveChargeInterval, spell.float2);
				break;
			case SpellAbilityType.WandSpirit:
				passiveWandSpiritHighestManaGenSpeedRatio = Mathf.Max(passiveWandSpiritHighestManaGenSpeedRatio, spell.float1 / 100f);
				passiveAutoWand = true;
				break;
			case SpellAbilityType.ForceCoolDown:
				passiveWandCoolDownTimeRatio *= spell.coolDownRatio;
				break;
			case SpellAbilityType.UltimateExtender:
				passiveDamageRatio *= spell.float2 / 100f;
				break;
			case SpellAbilityType.EchoRune:
				passiveEchoShootChance += spell.float1;
				passiveEchoFreeShootChance = Mathf.Max(passiveEchoFreeShootChance, spell.float2 / 100f);
				break;
			case SpellAbilityType.ManaInterface:
				passiveWandMaxMpRatio *= spell.float1 / 100f;
				passiveBonusMpGen -= spell.float2;
				break;
			case SpellAbilityType.EqualDistributionAngle:
				passiveEqualAngleDistribution = true;
				break;
			case SpellAbilityType.ManaTendril:
				passiveMpGenRatio += spell.float1 / 100f * ((float)slot.specialInt + 1f);
				passiveWandMaxMpRatio += spell.float2 / 100f * ((float)slot.specialInt + 1f);
				break;
			case SpellAbilityType.Umbrella:
				passiveUmbrellaEnable = true;
				passiveBonusMaxMp += (float)spell.int2;
				if (!umbrellaEnableLastTime)
				{
					passiveUmbrellaMpFull = false;
				}
				break;
			case SpellAbilityType.RuneHammer:
				passiveRuneHammerEnable = true;
				passiveOwnerSpeedUpRatio += spell.float2 / 100f;
				break;
			case SpellAbilityType.LaserBeam:
				passiveLaserCrystalEnable = true;
				break;
			case SpellAbilityType.BiAnLethalBlade:
				passiveBiAnBladeEnable = true;
				break;
			case SpellAbilityType.DaveHarpoons:
				passiveDaveHarpoonsEnable = true;
				DaveHarpoonsSpellCount++;
				break;
			case SpellAbilityType.PostSlotExtenderMove:
				WandCfg.PostslotMoveChargeRatio += spell.float1;
				PassiveTransStoneEnable = true;
				break;
			case SpellAbilityType.PostSlotExtenderStand:
				WandCfg.PostslotStandChargeRatio += spell.float1;
				PassiveTransStoneEnable = true;
				break;
			case SpellAbilityType.PostSlotExtenderTime:
				WandCfg.PostslotTimeChargeRatio += spell.float1;
				PassiveTransStoneEnable = true;
				break;
			case SpellAbilityType.PostSlotExtenderCastSpell:
				WandCfg.PostslotCastSpellChargeRatio += spell.float1;
				PassiveTransStoneEnable = true;
				break;
			case SpellAbilityType.RandomPosFocusMouse:
				passiveRandomPosShoot = true;
				RandomPosShootRadius = spell.float1;
				RandomPosShootAngleShiftPerShoot = spell.float2;
				break;
			case SpellAbilityType.ManaToPostChargeRatio:
				passiveManaToPostRatio += spell.float3 / 100f;
				passiveEnablaManaRegen = false;
				break;
			case SpellAbilityType.RedRune:
				passiveRedRuneCount++;
				break;
			case SpellAbilityType.GreenRune:
				passiveGreenRuneCount++;
				break;
			case SpellAbilityType.BlueRune:
				passiveBlueRuneCount++;
				break;
			case SpellAbilityType.AllFieldEnhance:
			case SpellAbilityType.SpellEmbryo:
				break;
			}
		};
		SlotData[] validSlotsData = WandCfg.GetValidSlotsData(normal: true, post: false);
		foreach (SlotData slotData in validSlotsData)
		{
			action(slotData, slotData.GetFinalConfig(), arg3: false);
		}
		validSlotsData = WandCfg.GetValidSlotsData(normal: false, post: true);
		foreach (SlotData slotData2 in validSlotsData)
		{
			action(slotData2, slotData2.GetFinalConfig(), arg3: true);
		}
		if (passiveAutoWand)
		{
			passiveMpGenRatio *= passiveWandSpiritHighestManaGenSpeedRatio;
		}
		CurrentMP = CurrentMP;
		if (flag && !passiveAutoWand)
		{
			PlayerMgr.Inst.CancelAutoControlWand(this);
		}
		else if (passiveAutoWand && PlayerMgr.Inst.Wands.Contains(this))
		{
			PlayerMgr.Inst.SpawnAutoControlWand(this);
			Display_UpdateShowOrHide();
		}
		if (passiveGreenRuneCount > 0)
		{
			GreenRuneGroup = GetApplyWandAllEnhanceEffectShootGroup(40261);
		}
		if (passiveBlueRuneCount > 0)
		{
			blueRuneGroup = GetApplyWandAllEnhanceEffectShootGroup(40271);
		}
		if (refreshAutoSpells)
		{
			RefreshAutoSpell(passiveRuneHammerEnable, ref lastCreateHammerFrame, SpellAbilityType.RuneHammer, typeof(Spell4013RuneHammerData));
			RefreshLaserBeam();
			RefreshBiAnBlades();
			RefreshUmbrella();
		}
	}

	private void SpawnAutoSpell(ref int lastSpawnFrame, SpellAbilityType abilityType)
	{
		if (lastSpawnFrame == Time.frameCount)
		{
			return;
		}
		lastSpawnFrame = Time.frameCount;
		IEnumerable<SlotData> enumerable = from e in WandCfg.GetValidSlotsData(normal: true, post: true)
			where e.GetFinalConfig().abilityType == abilityType
			select e;
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		if (passiveAutoWand)
		{
			builder.ApplyShooterEntity(PassiveWandSpiritEntity, PassiveWandSpiritEntity);
		}
		foreach (SlotData item in enumerable)
		{
			ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(item.GetFinalId()), ShootSpellSpatialInfo.ToPoint(ShootPosition, ShootPosition + ShootDirection), builder, 0f);
		}
	}

	private DynamicBuffer<BladeWandSingletonData> GetBladeDotsWandDataBuffer()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (_bladeWandDataEntity == Entity.Null)
		{
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(BladeWandSingletonData));
			if (!entityQuery.IsEmpty)
			{
				_bladeWandDataEntity = entityQuery.GetSingletonEntity();
			}
		}
		return entityManager.GetBuffer<BladeWandSingletonData>(_bladeWandDataEntity);
	}

	private void CalcLightningChainFinalStats(out float totalDamage, out int totalPenetrate)
	{
		totalDamage = 0f;
		totalPenetrate = 0;
		if (WandCfg == null)
		{
			return;
		}
		SlotData[] validSlotsData = WandCfg.GetValidSlotsData(normal: true, post: true);
		foreach (SlotData slotData in validSlotsData)
		{
			if (slotData != null && !slotData.isSealSlot && slotData.GetFinalConfig().abilityType == SpellAbilityType.LightningChain)
			{
				SpellShootData applyWandAllEnhanceEffectShootData = GetApplyWandAllEnhanceEffectShootData(slotData.GetFinalId());
				SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
				builder.ApplySpellShootDataEffect(applyWandAllEnhanceEffectShootData);
				SpellInitialParameter spellInitialParameter = builder.Build(ShootSpellSpatialInfo.ToPoint(Vector3.zero, Vector3.right));
				float lightningChainDamage = spellInitialParameter.lightningChainDamage;
				int penetrateCount = spellInitialParameter.PenetrateCount;
				totalDamage += lightningChainDamage;
				totalPenetrate = math.max(totalPenetrate, penetrateCount);
			}
		}
	}

	public int ShootDaveHarpoonsDots(int maxCount)
	{
		if (maxCount <= 0)
		{
			return 0;
		}
		SlotData slotData = (from e in WandCfg.GetValidSlotsData(normal: true, post: true)
			where e.GetFinalConfig().abilityType == SpellAbilityType.DaveHarpoons
			select e).ElementAt(0);
		SpellShootGroup applyWandAllEnhanceEffectShootGroup = GetApplyWandAllEnhanceEffectShootGroup(slotData.GetFinalId());
		int maxMeantimeShootCount = applyWandAllEnhanceEffectShootGroup.Shoots[0].GetMaxMeantimeShootCount(WandCfg.shootCount);
		maxMeantimeShootCount = math.min(maxCount, maxMeantimeShootCount);
		SpellInitialParameter.Builder sipBuilder = CreateSIPBuilder(fromPostSlots: false);
		(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, 0f);
		ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
		float reverseCopyShootRate = ((PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy != null) ? ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy.int1.result / 100f) : 0f);
		for (int i = 0; i < maxMeantimeShootCount; i++)
		{
			ShootSpellGroup(applyWandAllEnhanceEffectShootGroup, spatialInfo, sipBuilder, reverseCopyShootRate);
		}
		return maxMeantimeShootCount;
	}

	public void ShootDaveHarpoonsDotsAim(int maxCount, float extraScatterMulti)
	{
		if (maxCount <= 0)
		{
			TopUI.inst.uI_AimSkill.useSkillDir = false;
			return;
		}
		SlotData slotData = (from e in WandCfg.GetValidSlotsData(normal: true, post: true)
			where e.GetFinalConfig().abilityType == SpellAbilityType.DaveHarpoons
			select e).ElementAt(0);
		SpellShootGroup applyWandAllEnhanceEffectShootGroup = GetApplyWandAllEnhanceEffectShootGroup(slotData.GetFinalId());
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
		{
			sip.finalDamageRatio *= maxCount;
			sip.extraScatter /= extraScatterMulti;
		};
		(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, 0f);
		ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
		float reverseCopyShootRate = ((PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy != null) ? ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy.int1.result / 100f) : 0f);
		ShootSpellGroup(applyWandAllEnhanceEffectShootGroup, spatialInfo, builder, reverseCopyShootRate);
		TopUI.inst.uI_AimSkill.useSkillDir = false;
	}

	private void SpawnLaserCrystal()
	{
		if (lastCreateLaserCrystalFrame == Time.frameCount)
		{
			return;
		}
		LaserCrystalCount = 0;
		lastCreateLaserCrystalFrame = Time.frameCount;
		SlotData[] array = (from e in WandCfg.GetValidSlotsData(normal: true, post: true)
			where e.GetFinalConfig().abilityType == SpellAbilityType.LaserBeam
			select e).ToArray();
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		if (passiveAutoWand)
		{
			builder.ApplyShooterEntity(PassiveWandSpiritEntity, PassiveWandSpiritEntity);
		}
		int wandMultishootEnhanceCount = GetWandMultishootEnhanceCount();
		(int, bool) wandSplitshootEnhanceData = GetWandSplitshootEnhanceData();
		float num = 1f;
		int num2 = array.Length;
		int num3 = (int)math.floor(15f / (float)array.Length);
		if (num3 < 1)
		{
			num3 = math.max(num3, 1);
		}
		if (num3 < wandMultishootEnhanceCount)
		{
			num *= (float)wandMultishootEnhanceCount / (float)num3;
		}
		wandMultishootEnhanceCount = math.min(wandMultishootEnhanceCount, num3);
		num2 *= wandMultishootEnhanceCount;
		int x = (int)math.floor(45f / (float)num2);
		if (wandSplitshootEnhanceData.Item1 > 0)
		{
			x = math.min(x, wandSplitshootEnhanceData.Item1);
			if (wandSplitshootEnhanceData.Item1 > x)
			{
				num *= (float)wandSplitshootEnhanceData.Item1 / (float)x;
			}
		}
		else
		{
			x = 0;
		}
		LaserCrystalPowerRatio = num;
		LaserCrystalFinalSplitCount = x;
		LaserCrystalFinalMultiCount = wandMultishootEnhanceCount;
		LaserCrystalCount = array.Length;
		LaserCrystalThunderChance = GetWandThunderEnhanceData();
		SlotData[] array2 = array;
		foreach (SlotData slotData in array2)
		{
			for (int j = 0; j < wandMultishootEnhanceCount; j++)
			{
				ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(slotData.GetFinalId()), ShootSpellSpatialInfo.ToPoint(ShootPosition, ShootPosition + ShootDirection), builder, 0f);
			}
		}
	}

	private void SpawnBiAnBlade()
	{
		if (lastCreateBiAnBladeFrame == Time.frameCount || MaxMP <= 0f)
		{
			return;
		}
		lastCreateBiAnBladeFrame = Time.frameCount;
		IEnumerable<SlotData> source = from e in WandCfg.GetValidSlotsData(normal: true, post: true)
			where e.GetFinalConfig().abilityType == SpellAbilityType.BiAnLethalBlade
			select e;
		int num = (int)math.ceil(MaxMP / 25f);
		int wandMultishootEnhanceCount = GetWandMultishootEnhanceCount();
		(int, bool) wandSplitshootEnhanceData = GetWandSplitshootEnhanceData();
		int num2 = (passiveBiAnBladeTotalCount = num * wandMultishootEnhanceCount * wandSplitshootEnhanceData.Item1 * source.Count());
		float num3 = math.min(512f, num2);
		float bladePowerRatio = (float)num2 / num3;
		if (wandSplitshootEnhanceData.Item2)
		{
			bladePowerRatio *= 0.35f;
		}
		SpellShootGroup spellShootGroup = new SpellShootGroup();
		for (int i = 0; (float)i < num3; i++)
		{
			SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
			{
				sip.finalDamageRatio *= bladePowerRatio;
			};
			spellShootGroup = GetApplyWandAllEnhanceEffectShootGroup(40191);
			ShootSpellGroup(spellShootGroup, ShootSpellSpatialInfo.ToPoint(ShootPosition, ShootPosition + ShootDirection), builder, 0f);
		}
		int maxMeantimeShootCount = spellShootGroup.Shoots[0].GetMaxMeantimeShootCount(WandCfg.shootCount);
		DynamicBuffer<BladeWandSingletonData> bladeDotsWandDataBuffer = GetBladeDotsWandDataBuffer();
		for (int num4 = bladeDotsWandDataBuffer.Length - 1; num4 >= 0; num4--)
		{
			if (bladeDotsWandDataBuffer[num4].WandId == WandIndex)
			{
				bladeDotsWandDataBuffer.RemoveAt(num4);
			}
		}
		CalcLightningChainFinalStats(out var totalDamage, out var totalPenetrate);
		bladeDotsWandDataBuffer.Add(new BladeWandSingletonData
		{
			WandId = WandIndex,
			ShootCount = maxMeantimeShootCount,
			LightningChainDamage = totalDamage,
			LightningChainPenetrate = totalPenetrate
		});
		StartCoroutine(GetBiAnBladeCount(num2));
	}

	private int GetWandMultishootEnhanceCount()
	{
		int num = 1;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig finalConfig = wandAllEnhance.GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.Multishot)
			{
				num += finalConfig.int1;
			}
		}
		return num;
	}

	private bool IsWandHasFallEffectEnhance()
	{
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			if (wandAllEnhance.GetFinalConfig().abilityType == SpellAbilityType.Fall)
			{
				return true;
			}
		}
		return false;
	}

	private (int splitCount, bool hasSplitEffect) GetWandSplitshootEnhanceData()
	{
		int num = 0;
		bool item = false;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig finalConfig = wandAllEnhance.GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.SpellSplit)
			{
				num += finalConfig.int1;
				item = true;
			}
		}
		num = math.max(1, num);
		return (num, item);
	}

	private float GetWandThunderEnhanceData()
	{
		float num = 0f;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig finalConfig = wandAllEnhance.GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.ThunderCrystal && num < finalConfig.float3)
			{
				num = finalConfig.float3;
			}
		}
		return num;
	}

	private IEnumerator GetBiAnBladeCount(int bladeCount)
	{
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		passiveBiAnBladeTotalCount = bladeCount;
	}

	public void ClearAutoSpell(Type autoSpellTagType)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		EntityCommandBuffer entityCommandBuffer = entityManager.World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(new EntityQueryDesc
		{
			All = new ComponentType[3]
			{
				ComponentType.ReadOnly<SpellConfigComponentData>(),
				new ComponentType(autoSpellTagType, ComponentType.AccessMode.ReadOnly),
				ComponentType.ReadOnly<SpellComponentData>()
			}
		});
		using NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.TempJob);
		using NativeArray<SpellComponentData> nativeArray2 = entityQuery.ToComponentDataArray<SpellComponentData>(Allocator.TempJob);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			if (nativeArray2[i].Wand.Value == this)
			{
				entityCommandBuffer.DestroyEntity(nativeArray[i]);
			}
		}
	}

	public void RefreshAutoSpell(bool autoSpellEnable, ref int lastSpawnFrame, SpellAbilityType abilityType, Type type)
	{
		ClearAutoSpell(type);
		if (autoSpellEnable)
		{
			SpawnAutoSpell(ref lastSpawnFrame, abilityType);
		}
	}

	public void RefreshHammer()
	{
		RefreshAutoSpell(passiveRuneHammerEnable, ref lastCreateHammerFrame, SpellAbilityType.RuneHammer, typeof(Spell4013RuneHammerData));
	}

	public void RefreshBiAnBlades()
	{
		passiveBiAnBladeTotalCount = 1;
		ClearAutoSpell(typeof(Spell4019BiAnBladeData));
		if (passiveBiAnBladeEnable)
		{
			SpawnBiAnBlade();
		}
	}

	public void RefreshLaserBeam()
	{
		ClearAutoSpell(typeof(Spell4014LaserCrystalData));
		AudioSource value;
		bool flag = SEMgr.Inst.loopSEs.TryGetValue("SE_Spell4014Loop", out value);
		if (flag)
		{
			value.mute = true;
		}
		if (passiveLaserCrystalEnable)
		{
			if (flag)
			{
				value.mute = false;
			}
			SpawnLaserCrystal();
		}
		else
		{
			LaserCrystalCount = 0;
		}
	}

	public void RefreshUmbrella()
	{
		ClearAutoSpell(typeof(Spell4012MagicShieldData));
		if (passiveUmbrellaEnable)
		{
			SpawnUmbrella();
		}
	}

	private void SpawnUmbrella()
	{
		if (lastCreateUmbrellaFrame != Time.frameCount)
		{
			lastCreateUmbrellaFrame = Time.frameCount;
			IEnumerable<SlotData> source = from e in WandCfg.GetValidSlotsData(normal: true, post: true)
				where e.GetFinalConfig().abilityType == SpellAbilityType.Umbrella
				select e;
			SpellShootGroup applyWandAllEnhanceEffectShootGroup = GetApplyWandAllEnhanceEffectShootGroup(source.ElementAt(0).GetFinalId());
			ShootSpellGroup(applyWandAllEnhanceEffectShootGroup, ShootSpellSpatialInfo.ToPoint(ShootPosition, ShootPosition + ShootDirection), CreateSIPBuilder(fromPostSlots: false), 0f);
			UmbrellaShieldController umbrellaCtrl = PlayerMgr.Inst.PlayerPpt.UmbrellaCtrl;
			if (!umbrellaCtrl.dicForEcsEntities.ContainsKey(this))
			{
				umbrellaCtrl.dicForEcsEntities.Add(this, Entity.Null);
			}
			else
			{
				umbrellaCtrl.dicForEcsEntities[this] = Entity.Null;
			}
		}
	}

	public SpellShootData GetApplyWandAllEnhanceEffectShootData(int spellId)
	{
		return GetApplyWandAllEnhanceEffectShootGroup(spellId).Shoots[0];
	}

	public SpellShootGroup GetApplyWandAllEnhanceEffectShootGroup(int spellId)
	{
		SpellShootGroup spellShootGroup = new SpellShootGroup();
		SpellShootData spellShootData = new SpellShootData(new SlotData
		{
			id = spellId
		}, spellShootGroup)
		{
			EnhanceList = GetWandAllEnhanceList().ToArray()
		};
		spellShootGroup.Shoots = new SpellShootData[1] { spellShootData };
		return spellShootGroup;
	}

	public SpellInitialParameter GetApplyWandAllEnhanceEffectSIP(int spellId, bool calculateSplitAsNormalEffect = false)
	{
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		SpellShootData applyWandAllEnhanceEffectShootData = GetApplyWandAllEnhanceEffectShootData(spellId);
		builder.ApplySpellShootDataEffect(applyWandAllEnhanceEffectShootData);
		SpellInitialParameter spellInitialParameter = builder.Build(ShootSpellSpatialInfo.ToPoint(Vector3.zero, Vector3.zero));
		spellInitialParameter.multiShootCount = applyWandAllEnhanceEffectShootData.GetSpellMultiShootData().count;
		if (calculateSplitAsNormalEffect && PlayerMgr.Inst.SpellHasSplitEffect(applyWandAllEnhanceEffectShootData.EnhanceList))
		{
			spellInitialParameter.finalDamageRatio *= 0.33f;
		}
		return spellInitialParameter;
	}

	public int GetWandOnceShootCountWithEnhance()
	{
		if (WandCfg == null)
		{
			return 1;
		}
		int num = WandCfg.shootCount;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig spellConfig = SpellConfig.dic[wandAllEnhance.id];
			SpellAbilityType abilityType = spellConfig.abilityType;
			if (abilityType == SpellAbilityType.Volley || abilityType == SpellAbilityType.TotalScattering)
			{
				num += spellConfig.int1;
			}
		}
		return num;
	}

	public int GetWandSplitCountWithEnhance()
	{
		if (WandCfg == null)
		{
			return 0;
		}
		int num = 0;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig spellConfig = SpellConfig.dic[wandAllEnhance.id];
			if (spellConfig.abilityType == SpellAbilityType.SpellSplit)
			{
				num += spellConfig.int1;
			}
		}
		return num;
	}

	public float GetWandMpRecoverSpeed()
	{
		if (WandCfg == null)
		{
			return 0f;
		}
		float num = (float)WandCfg.mpRecovery + PlayerMgr.Inst.BaData.mpRecovery + passiveBonusMpGen;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_MPRecovery != null)
		{
			num += (float)PlayerMgr.Inst.ItemCtrller.relicCfg_MPRecovery.int1.result;
		}
		if (passiveBlueRuneCount > 0)
		{
			int item = PlayerMgr.Inst.GetPlayerRuneCount().BlueRune;
			if (PlayerMgr.Inst.GetRuneEffectLevel(item) >= 1)
			{
				num += (float)item * 0.5f;
			}
		}
		num += PlayerMgr.Inst.MpGainAmountFixFromWandAbility();
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_LoseMPRecovery != null)
		{
			num *= 1f - (float)PlayerMgr.Inst.ItemCtrller.curseCfg_LoseMPRecovery.int1.result / 100f;
		}
		if (Math.Abs(passiveMpGenRatio - 1f) > 0.01f)
		{
			num *= passiveMpGenRatio;
		}
		switch (WandCfg.specialAbility)
		{
		case WandAbility.HoldWandDifferentManaGenRatio:
			num = ((!(PlayerMgr.Inst.SelectedWand == this) || passiveAutoWand) ? (num * (WandCfg.float2 / 100f)) : (num * (WandCfg.float1 / 100f)));
			break;
		case WandAbility.StandMpGenUp:
			if (PlayerMgr.Inst.PlayerCtrller.isStandInLastFrame && !PlayerMgr.Inst.inDashSpell)
			{
				num *= WandCfg.float1 / 100f;
			}
			break;
		case WandAbility.PeriodRecoverAllMp:
			num = 0f;
			break;
		}
		return num;
	}

	public void Initialize(int index)
	{
		WandIndex = index;
		if (WandCfg == null)
		{
			Display_Hide();
		}
		else
		{
			Display_UpdateShowOrHide();
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_SpellKnockback != null && tsf_Layer.Find(RelicAbilityType.SpellKnockback.ToString()) == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SpellKnockback"), tsf_Layer);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.name = RelicAbilityType.SpellKnockback.ToString();
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_PostSlotMoreEfficiency != null && tsf_Layer.Find(RelicAbilityType.PostSlotMoreEfficiency.ToString()) == null)
		{
			GameObject obj2 = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_PostSlotMoreEfficiency"), tsf_Layer);
			obj2.transform.localPosition = Vector3.zero;
			obj2.transform.localRotation = Quaternion.identity;
			obj2.name = RelicAbilityType.PostSlotMoreEfficiency.ToString();
		}
	}

	public float GetCurrentManaPercent()
	{
		if (MaxMP <= 0f)
		{
			return 0f;
		}
		return CurrentMP / MaxMP;
	}

	public void ResetWandSlotState()
	{
		if (WandCfg != null)
		{
			if (WandCfg.transIntoPostslotData.Length != 0)
			{
				PassiveTransPostBackToNormalSlot();
				PassiveRemoveTransSlotFromPostSlot();
				WandCfg.transIntoPostslotData = Array.Empty<SlotData>();
				WandCfg.transIntoPostslotLockData = Array.Empty<bool>();
			}
			RemoveAllFieldSharedSpell();
		}
	}

	public void ProcessSharedSpell(List<SlotData> sharedSpell, bool refreshAutoSpell = true)
	{
		if (WandCfg != null)
		{
			bool flag = false;
			if (WandCfg.transIntoPostslotData.Length != 0)
			{
				PassiveTransPostBackToNormalSlot();
				PassiveRemoveTransSlotFromPostSlot();
				flag = true;
			}
			RemoveAllFieldSharedSpell();
			if (sharedSpell.Count > 0)
			{
				InsertAllFieldSharedSpell(sharedSpell.Copy());
			}
			if (flag)
			{
				UpdatePassiveTransIntoPostSlotData();
				PassiveRemoveTransSlotFromNormalSlot();
				PassiveAddTransSlotIntoPostSlot();
				UIPlayerDataMgr.Inst.WandUpdate(WandIndex);
			}
			CheckSpellListForMimicEffect(WandSlotType.Normal);
			CheckSpellListForMimicEffect(WandSlotType.Post);
			WandCfg.normalSlots = SimpleCheckTargetSlotsMimicEffect(WandCfg.normalSlots);
			CheckSpellListForManaTendrilEffect(WandSlotType.Normal);
			CheckSpellListForManaTendrilEffect(WandSlotType.Post);
			CheckSpellListForLevelEnhanceEffect(WandSlotType.Normal);
			CheckSpellListForLevelEnhanceEffect(WandSlotType.Post);
			CheckWandSlotPassiveEffect(refreshAutoSpell);
			RecalculateSpellGroups();
			UIPlayerDataMgr.Inst.WandUpdate(WandIndex);
		}
	}

	public void ResetAndRecheck(bool refreshAutoSpells = true)
	{
		if (WandCfg == null)
		{
			return;
		}
		PostSlotCurrentCharge = 0f;
		rechecked = true;
		UI_HidePreShootHint();
		if (IsCharging)
		{
			ReleaseCharge();
		}
		if ((bool)PlayerMgr.Inst.ItemCtrller.relic_MirrorOfSoul && !this.CheckWandEnableMirrorOfSoul())
		{
			this.ApplyMirrorOfSoulToWand();
		}
		List<SlotData> allWandPassiveAllFieldEnhanceSharedSpell = PlayerMgr.Inst.GetAllWandPassiveAllFieldEnhanceSharedSpell();
		if (WandCfg != null && !WandCfg.IsAllfieldSharedSpellSame(allWandPassiveAllFieldEnhanceSharedSpell))
		{
			WandCfg.AllfieldSharedSpellList = allWandPassiveAllFieldEnhanceSharedSpell;
			{
				foreach (Wand wand in PlayerMgr.Inst.Wands)
				{
					wand.ProcessSharedSpell(allWandPassiveAllFieldEnhanceSharedSpell.Select((SlotData e) => e.Copy()).ToList(), refreshAutoSpells);
				}
				return;
			}
		}
		CheckSpellListForMimicEffect(WandSlotType.Normal);
		CheckSpellListForMimicEffect(WandSlotType.Post);
		CheckSpellListForManaTendrilEffect(WandSlotType.Normal);
		CheckSpellListForManaTendrilEffect(WandSlotType.Post);
		CheckSpellListForLevelEnhanceEffect(WandSlotType.Normal);
		CheckSpellListForLevelEnhanceEffect(WandSlotType.Post);
		CheckWandSlotPassiveEffect(refreshAutoSpells);
		RecalculateSpellGroups();
		UIPlayerDataMgr.Inst.WandUpdate(WandIndex);
	}

	private void TryCreateSpellBreakerHead()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.Exists(spellBreakerEntity) || WandCfg == null || WandCfg.specialAbility != WandAbility.LongWandAndSpellBreaker)
		{
			return;
		}
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpellSingleton));
		Entity srcEntity = entityQuery.GetSingleton<SpellSingleton>().Prefabs["SpellBreakWandHead"];
		spellBreakerEntity = entityManager.Instantiate(srcEntity);
		entityManager.SetName(spellBreakerEntity, "SpellBreakWandHead");
	}

	private void TryDestroySpellBreakerHead()
	{
		if (!(spellBreakerEntity == Entity.Null))
		{
			World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(spellBreakerEntity);
			spellBreakerEntity = Entity.Null;
		}
	}

	private void UpdateSpellBreakerHeadTransform()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<LocalTransform>(spellBreakerEntity))
		{
			Entity entity = spellBreakerEntity;
			float3 position = ShootPosition;
			float2 dir = ((float3)ShootDirection).xy;
			entityManager.SetComponentData(entity, LocalTransform.FromPositionRotation(position, DTool.DirectionToRotation(in dir)));
		}
	}

	public void InsertAllFieldSharedSpell(List<SlotData> sharedSpell)
	{
		(SlotData[], bool[]) normalSlotDataAfterCombineSharedSpell = GetNormalSlotDataAfterCombineSharedSpell(sharedSpell);
		WandCfg.normalSlots = normalSlotDataAfterCombineSharedSpell.Item1;
		WandCfg.normalSlotIsLock = normalSlotDataAfterCombineSharedSpell.Item2;
	}

	public (SlotData[] slotData, bool[] lockData) GetNormalSlotDataAfterCombineSharedSpell(List<SlotData> sharedSpell)
	{
		List<SlotData> list = new List<SlotData>();
		List<bool> list2 = new List<bool>();
		list.AddRange(sharedSpell);
		foreach (SlotData item in list)
		{
			item.isAllFieldSharedSpell = true;
			if (SpellConfig.dic[item.id].abilityType == SpellAbilityType.ManaTendril)
			{
				item.specialInt = 0;
			}
			list2.Add(item: true);
		}
		for (int i = 0; i < list.Count; i++)
		{
			UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, i, WandSlotType.Normal, WandSlotIconVisualEffect.AllFieldEnhance);
		}
		list.AddRange(WandCfg.normalSlots);
		list2.AddRange(WandCfg.normalSlotIsLock);
		return (list.ToArray(), list2.ToArray());
	}

	public void RemoveAllFieldSharedSpell()
	{
		if (WandCfg == null || WandCfg.normalSlots.Length == 0)
		{
			return;
		}
		int num = 0;
		SlotData[] normalSlots = WandCfg.normalSlots;
		SlotData[] array = normalSlots;
		foreach (SlotData slotData in array)
		{
			if (slotData == null || !slotData.isAllFieldSharedSpell)
			{
				break;
			}
			num++;
		}
		WandCfg.normalSlots = normalSlots.Skip(num).Take(normalSlots.Length - num).ToArray();
		WandCfg.normalSlotIsLock = WandCfg.normalSlotIsLock.Skip(num).Take(normalSlots.Length - num).ToArray();
	}

	public int GetAllFieldSharedSpellLengthInNormalSlots()
	{
		if (WandCfg == null || WandCfg.normalSlots.Length == 0)
		{
			return 0;
		}
		int num = 0;
		SlotData[] normalSlots = WandCfg.normalSlots;
		foreach (SlotData slotData in normalSlots)
		{
			if (slotData == null || !slotData.isAllFieldSharedSpell)
			{
				break;
			}
			num++;
		}
		return num;
	}

	public List<SlotData> GetWandAllFieldEnhanceSpell()
	{
		if (WandCfg == null)
		{
			return new List<SlotData>();
		}
		List<SlotData> list = new List<SlotData>();
		bool flag = false;
		int num = 0;
		List<SlotData> list2 = new List<SlotData>();
		list2.AddRange(WandCfg.GetSlotsData(WandSlotType.Normal));
		list2.AddRange(WandCfg.GetSlotsData(WandSlotType.Post));
		foreach (SlotData item in list2)
		{
			if (item == null || item.isSealSlot || item.isAllFieldSharedSpell)
			{
				continue;
			}
			if (flag && item.GetConfigIgnoreMimic().abilityType != SpellAbilityType.AllFieldEnhance)
			{
				SlotData slotData = item.Copy();
				if (slotData.GetLevelIgnoreMimic() > num)
				{
					slotData.id = slotData.id - slotData.id % 10 + num;
				}
				slotData.mimicSpellID = 0;
				list.Add(slotData);
				flag = false;
				num = 0;
			}
			else if (item.GetConfigIgnoreMimic().abilityType == SpellAbilityType.AllFieldEnhance)
			{
				flag = true;
				num = Mathf.Max(item.GetFinalConfig().level, num);
			}
		}
		return list;
	}

	public bool IsTransIntoPostDataChanged()
	{
		int num = WandCfg.normalSlots.Length + WandCfg.transIntoPostslotData.Length;
		int num2 = WandConfig.dic[WandCfg.id].normalSlots.Length;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot != null)
		{
			num2 += PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot.int1.result;
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_LessWandMoreSlot != null)
		{
			num2 += PlayerMgr.Inst.ItemCtrller.relicCfg_LessWandMoreSlot.int2.result;
		}
		if (ExtraNormalSlot > 0)
		{
			num2 += ExtraNormalSlot;
		}
		num2 += GetAllFieldSharedSpellCount();
		int num3 = WandCfg.postSlots.Length - WandCfg.transIntoPostslotData.Length;
		int num4 = WandConfig.dic[WandCfg.id].postSlots.Length;
		if (ExtraPostSlot > 0)
		{
			num4 += ExtraPostSlot;
		}
		SlotData[] item = GetPassiveNewTransIntoPostSlotData().slotData;
		bool flag = WandCfg.transIntoPostslotData.Length != 0 && GetFirstTransStoneIndex(GetPassiveTransPostBackToNormalSlot().slotData) != WandCfg.normalSlots.Length;
		if (item.Length != WandCfg.transIntoPostslotData.Length)
		{
			flag = true;
		}
		if (!flag && GetPassiveNewTransIntoPostSlotData().slotData.Length == WandCfg.transIntoPostslotData.Length && num == num2 && num3 == num4)
		{
			if (!PassiveTransStoneEnable)
			{
				return WandCfg.transIntoPostslotData.Length != 0;
			}
			return false;
		}
		return true;
	}

	public void UpdatePassiveTransIntoPostSlotData()
	{
		(SlotData[], bool[]) passiveNewTransIntoPostSlotData = GetPassiveNewTransIntoPostSlotData(useNormalSlot: true);
		WandCfg.transIntoPostslotData = passiveNewTransIntoPostSlotData.Item1;
		WandCfg.transIntoPostslotLockData = passiveNewTransIntoPostSlotData.Item2;
	}

	public (SlotData[] slotData, bool[] lockData) GetPassiveNewTransIntoPostSlotData(bool useNormalSlot = false)
	{
		SlotData[] array;
		bool[] source;
		if (useNormalSlot)
		{
			array = SimpleCheckTargetSlotsMimicEffect(WandCfg.normalSlots);
			source = WandCfg.normalSlotIsLock;
		}
		else
		{
			(SlotData[], bool[]) passiveTransPostBackToNormalSlot = GetPassiveTransPostBackToNormalSlot(useCurrentPostSlot: true);
			passiveTransPostBackToNormalSlot.Item1 = SimpleCheckTargetSlotsMimicEffect(passiveTransPostBackToNormalSlot.Item1);
			(array, source) = passiveTransPostBackToNormalSlot;
		}
		int num = 0;
		num = GetFirstTransStoneIndex(array);
		SlotData[] item = ((num >= 0) ? array.Skip(num).Take(array.Length - num).ToArray() : Array.Empty<SlotData>());
		bool[] item2 = ((num >= 0) ? source.Skip(num).Take(array.Length - num).ToArray() : Array.Empty<bool>());
		return (item, item2);
	}

	public int GetFirstTransStoneIndex(SlotData[] targetArray)
	{
		for (int i = 0; i < targetArray.Length; i++)
		{
			SlotData slotData = targetArray[i];
			if (slotData != null && !slotData.isSealSlot)
			{
				SpellConfig finalConfig = slotData.GetFinalConfig();
				if (finalConfig.abilityType == SpellAbilityType.PostSlotExtenderMove || finalConfig.abilityType == SpellAbilityType.PostSlotExtenderStand || finalConfig.abilityType == SpellAbilityType.PostSlotExtenderCastSpell || finalConfig.abilityType == SpellAbilityType.PostSlotExtenderTime)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void PassiveTransPostBackToNormalSlot()
	{
		(SlotData[], bool[]) passiveTransPostBackToNormalSlot = GetPassiveTransPostBackToNormalSlot();
		WandCfg.normalSlots = passiveTransPostBackToNormalSlot.Item1;
		WandCfg.normalSlotIsLock = passiveTransPostBackToNormalSlot.Item2;
	}

	public void PassiveRemoveTransSlotFromPostSlot()
	{
		(SlotData[], bool[]) passiveBeforeTransPostSlotData = GetPassiveBeforeTransPostSlotData();
		WandCfg.postSlots = passiveBeforeTransPostSlotData.Item1;
		WandCfg.postSlotIsLock = passiveBeforeTransPostSlotData.Item2;
		if (WandCfg.postSlots.Length == 0)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < WandCfg.postSlots.Length; i++)
		{
			SlotData slotData = WandCfg.postSlots[i];
			if (slotData == null || !slotData.isSealSlot)
			{
				break;
			}
			WandCfg.postSlots[i] = null;
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		for (int num = WandCfg.normalSlots.Length - 1; num >= 0; num--)
		{
			SlotData slotData2 = WandCfg.normalSlots[num];
			if (slotData2 != null && !slotData2.isSealSlot)
			{
				SlotData data = WandCfg.normalSlots[num];
				WandCfg.normalSlots.Bag_RemoveSlot(num);
				if (WandCfg.normalSlots.Bag_CanSetSlotWithPush(WandCfg.normalSlotIsLock, data, num))
				{
					WandCfg.normalSlots.Bag_SetSlotWithPush(WandCfg.normalSlotIsLock, data, num);
				}
				else
				{
					PlayerMgr.Inst.SpellPick(slotData2);
				}
				break;
			}
		}
	}

	public void PassiveRemoveTransSlotFromNormalSlot()
	{
		(SlotData[], bool[]) passiveAfterTransNormalSlotData = GetPassiveAfterTransNormalSlotData();
		WandCfg.normalSlots = passiveAfterTransNormalSlotData.Item1;
		WandCfg.normalSlotIsLock = passiveAfterTransNormalSlotData.Item2;
	}

	public void PassiveAddTransSlotIntoPostSlot()
	{
		(SlotData[], bool[]) passiveTransNormalIntoToPostSlot = GetPassiveTransNormalIntoToPostSlot();
		WandCfg.postSlots = passiveTransNormalIntoToPostSlot.Item1;
		WandCfg.postSlotIsLock = passiveTransNormalIntoToPostSlot.Item2;
	}

	public int GetAllFieldSharedSpellCount()
	{
		return PlayerMgr.Inst.GetAllWandPassiveAllFieldEnhanceSharedSpell().Count;
	}

	public (SlotData[] slotData, bool[] lockData) GetPassiveTransPostBackToNormalSlot(bool useCurrentPostSlot = false)
	{
		int num = WandCfg.normalSlots.Length + WandCfg.transIntoPostslotData.Length;
		SlotData[] normalSlots = WandCfg.normalSlots;
		bool[] normalSlotIsLock = WandCfg.normalSlotIsLock;
		SlotData[] array = new SlotData[num];
		bool[] array2 = new bool[num];
		for (int i = 0; i < normalSlots.Length; i++)
		{
			array[i] = normalSlots[i];
			array2[i] = normalSlotIsLock[i];
		}
		if (useCurrentPostSlot)
		{
			for (int j = 0; j < WandCfg.transIntoPostslotData.Length; j++)
			{
				array[normalSlots.Length + j] = WandCfg.transIntoPostslotData[j];
				array2[normalSlotIsLock.Length + j] = WandCfg.transIntoPostslotLockData[j];
			}
		}
		else
		{
			for (int k = 0; k < WandCfg.transIntoPostslotData.Length; k++)
			{
				array[normalSlots.Length + k] = WandCfg.postSlots[k];
				array2[normalSlotIsLock.Length + k] = WandCfg.postSlotIsLock[k];
			}
		}
		return (array, array2);
	}

	public (SlotData[] slotData, bool[] lockData) GetPassiveBeforeTransPostSlotData()
	{
		SlotData[] item = WandCfg.postSlots.Skip(WandCfg.transIntoPostslotData.Length).Take(WandCfg.postSlots.Length - WandCfg.transIntoPostslotData.Length).ToArray();
		bool[] item2 = WandCfg.postSlotIsLock.Skip(WandCfg.transIntoPostslotData.Length).Take(WandCfg.postSlots.Length - WandCfg.transIntoPostslotData.Length).ToArray();
		return (item, item2);
	}

	public (SlotData[] slotData, bool[] lockData) GetPassiveAfterTransNormalSlotData()
	{
		int num = WandCfg.normalSlots.Length - WandCfg.transIntoPostslotData.Length;
		if (num > WandCfg.normalSlots.Length)
		{
			Debug.LogError("为什么能将超过当前普通格子的格子转移过去 有问题");
			return (Array.Empty<SlotData>(), Array.Empty<bool>());
		}
		SlotData[] item = WandCfg.normalSlots.Take(num).ToArray();
		bool[] item2 = WandCfg.normalSlotIsLock.Take(num).ToArray();
		return (item, item2);
	}

	public (SlotData[] slotData, bool[] lockData) GetPassiveTransNormalIntoToPostSlot()
	{
		int num = WandCfg.postSlots.Length + WandCfg.transIntoPostslotData.Length;
		SlotData[] postSlots = WandCfg.postSlots;
		bool[] postSlotIsLock = WandCfg.postSlotIsLock;
		SlotData[] array = new SlotData[num];
		bool[] array2 = new bool[num];
		for (int i = 0; i < WandCfg.transIntoPostslotData.Length; i++)
		{
			array[i] = WandCfg.transIntoPostslotData[i];
			array2[i] = WandCfg.transIntoPostslotLockData[i];
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			int num2 = WandCfg.transIntoPostslotData.Length + j;
			array[num2] = postSlots[j];
			array2[num2] = postSlotIsLock[j];
		}
		return (array, array2);
	}

	public bool HaveEnoughManaToShootAnyGroup()
	{
		if (!shootGroups.Any())
		{
			return false;
		}
		return shootGroups.Any((SpellShootGroup e) => MaxMP + PlayerMgr.Inst.MaxManaAmountFromWandAbility() >= e.GetGroupManaCost_FinalPlayerValue(this));
	}

	public void StartCharge()
	{
		chargeAura = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_WandChargeMagicCircle", base.transform.position).GetComponent<Spell4004ChargeController>();
		chargeAura.ECStartEffect();
		SEMgr.Inst.spell4004Loop.PlayLoopSE(float.PositiveInfinity);
		TryChargeOnce();
	}

	public void ReleaseCharge()
	{
		if (IsCharging)
		{
			if (ShootPassiveCharge(fromEcho: false) > 0)
			{
				EnterNextGroup(setCoolDownOrInterval: true);
				SEMgr.Inst.spell4004Shoot.PlaySE();
			}
			TryUseWandAbility_ChanceInstentCoolDownAndFullMana_FullMana();
			CancelCharge();
		}
	}

	public void CancelCharge()
	{
		if (!IsCharging)
		{
			return;
		}
		SEMgr.Inst.loopSEDurations[SEMgr.Inst.spell4004Loop] = 0.1f;
		chargeAura.ECStopEffect();
		ObjPoolMgr.Inst.RecycleGO(chargeAura.gameObject, 1f);
		chargeAura = null;
		foreach (Spell4004ChargeStars chargeStar in ChargeStars)
		{
			ObjPoolMgr.Inst.RecycleGO(chargeStar.gameObject);
		}
		ChargeStars.Clear();
	}

	private void ChargeOnce()
	{
		Vector3 point = passiveAutoWandShooterData?.currentPosition ?? base.transform.position;
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_WandChargeStars", point);
		Transform followedTransform = ((passiveAutoWandShooterData != null) ? passiveAutoWandShooterData.wandObjectScript.transform : tsf_ShootPoint);
		float num = UnityEngine.Random.Range(0.9f * (1f + (float)(ChargeStars.Count + 1) * 0.04f), 1.35f * (1f + (float)(ChargeStars.Count + 1) * 0.07f));
		Vector2 rotateVector = UnityEngine.Random.insideUnitCircle.normalized * num;
		Vector3 shiftPos = ((passiveAutoWandShooterData != null) ? new Vector3(0f, 0.5f, 0f) : Vector3.zero);
		Spell4004ChargeStars star = gO.GetComponent<Spell4004ChargeStars>();
		star.Initialized(this, followedTransform, rotateVector, shiftPos);
		SpellShootGroup spellShootGroup = currentShootGroup.Copy(currentShootGroup.OwnerShootData);
		spellShootGroup.Shoots = spellShootGroup.Shoots.Where((SpellShootData e) => e.Spell.GetFinalConfig().abilityType.IsChargingSpell()).ToArray();
		if (spellShootGroup.Shoots.Length != 0)
		{
			SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
			{
				parameter.ChargeStar = star;
				parameter.Shooter = star.Entity;
			};
			ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(star.transform.position, ShootTargetPosition, ShootDirection);
			ShootNormalSlotsSpellGroup(spellShootGroup, spatialInfo, builder);
		}
		ChargeStars.Add(star);
		PlayChargeOnceSound();
	}

	private void PlayChargeOnceSound()
	{
		if (ChargeStars.Count < passiveChargeCountLimit)
		{
			AudioSource audioSource = null;
			audioSource = ((!passiveAutoWand) ? SEMgr.Inst.spell4004Success.PlaySE() : SEMgr.Inst.spell4004Success.PlaySE(passiveAutoWandShooterData.shootPosition));
			audioSource.pitch = Mathf.Min(1f + (float)ChargeStars.Count * 0.1f, 2f);
		}
		else if (passiveAutoWand)
		{
			SEMgr.Inst.spell4004Finish.PlaySE(passiveAutoWandShooterData.shootPosition);
		}
		else
		{
			SEMgr.Inst.spell4004Finish.PlaySE();
		}
	}

	private void UpdateShadowPosition()
	{
		float num = Mathf.Abs(PlayerMgr.Inst.PlayerPoint.z) - PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.localPosition.z + 0.3f;
		sr_WandShadow.transform.position = Tool2D.IgnoreZPoint(sr_Wand.transform.position + new Vector3(0f, 0f - num, 0f), 1.05f);
		sr_SpecialWandShadow.transform.position = Tool2D.IgnoreZPoint(sr_SpecialWand.transform.position + new Vector3(0f, 0f - num, 0f), 1.05f);
	}

	public bool CheckCurrentMpEnough(float needCost)
	{
		return CurrentMP + PlayerMgr.Inst.CurrentManaAmountFromWandAbility(this) >= needCost;
	}

	public bool CheckMaxMpEnough(float needCost)
	{
		return MaxMP + PlayerMgr.Inst.MaxManaAmountFromWandAbility(this) >= needCost;
	}

	private bool CanShootIgnoreMp()
	{
		if (currentShootGroup == null)
		{
			return false;
		}
		bool result = rechecked && ShootIntervalTimer <= 0f && CoolingTimer <= 0f;
		if (passiveAutoWand)
		{
			return result;
		}
		if (PlayerMgr.Inst.PlayerCtrller.IsKeepCasting && !PlayerMgr.Inst.inDashSpell)
		{
			return false;
		}
		if (PlayerMgr.Inst.PlayerCtrller.isDashOverHeat && currentShootGroup.HasShootableSpell(SpellAbilityType.Dash, deepSearch: false))
		{
			return false;
		}
		return result;
	}

	private bool CanShootCurrentGroup()
	{
		if (!CanShootIgnoreMp())
		{
			return false;
		}
		if (!passiveAutoWand && !HaveEnoughManaToShootAnyGroup() && currentShootGroup != null)
		{
			UI_ShowMPWarning();
			return false;
		}
		float groupManaCost_FinalPlayerValue = currentShootGroup.GetGroupManaCost_FinalPlayerValue(this);
		if (!CheckMaxMpEnough(groupManaCost_FinalPlayerValue))
		{
			EnterNextGroup(setCoolDownOrInterval: false);
			return false;
		}
		return CheckCurrentMpEnough(groupManaCost_FinalPlayerValue);
	}

	public void FreeShoot()
	{
		bool flag = CoolingTimer > 0f;
		float groupManaCost_FinalPlayerValue = currentShootGroup.GetGroupManaCost_FinalPlayerValue(this);
		if (CheckMaxMpEnough(groupManaCost_FinalPlayerValue))
		{
			TryUseWandAbility_ChanceInstentCoolDownAndFullMana_FullMana();
			if (shootGroups.Count > 0)
			{
				Shoot(fromEcho: false);
				EnterNextGroup(setCoolDownOrInterval: true);
			}
			if (!flag)
			{
				CoolingTimer = 0f;
			}
			ShootIntervalTimer = 0f;
		}
	}

	public bool TryShoot(bool fromEcho = false)
	{
		if (shootGroups.Count == 0 && !fromEcho && !passiveAutoWand)
		{
			UI_ShowNoShootableSpell();
			return false;
		}
		if (currentShootGroup != null && PlayerMgr.Inst.PlayerCtrller.isDashOverHeat && currentShootGroup.HasShootableSpell(SpellAbilityType.Dash, deepSearch: false))
		{
			EnterNextGroup(setCoolDownOrInterval: true);
		}
		if (!CanShootCurrentGroup() || PlayerMgr.Inst.PlayerCtrller.castSpellLock)
		{
			return false;
		}
		if (!fromEcho || !(UnityEngine.Random.Range(0f, 1f) <= passiveEchoFreeShootChance))
		{
			float groupManaCost_FinalPlayerValue = currentShootGroup.GetGroupManaCost_FinalPlayerValue(this);
			CostMp(groupManaCost_FinalPlayerValue);
		}
		TryUseWandAbility_ChanceInstentCoolDownAndFullMana_FullMana();
		Shoot(fromEcho);
		PassiveTryShootBiAnBlade_Dots();
		EnterNextGroup(setCoolDownOrInterval: true);
		return true;
	}

	private DynamicBuffer<BladeShootListenerData> GetBladeDotsListenerBuffer()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (_listenerEntity == Entity.Null)
		{
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(BladeShootListenerData));
			if (!entityQuery.IsEmpty)
			{
				_listenerEntity = entityQuery.GetSingletonEntity();
			}
		}
		return entityManager.GetBuffer<BladeShootListenerData>(_listenerEntity);
	}

	public void PassiveTryShootBiAnBlade_Dots()
	{
		if (passiveBiAnBladeEnable)
		{
			GetBladeDotsListenerBuffer().Add(new BladeShootListenerData
			{
				ShootingWandId = WandIndex,
				EventType = 0
			});
		}
	}

	public WandPostSlotChargeData BuildPostSlotChargeData()
	{
		return new WandPostSlotChargeData(WandCfg.postSlotTriggerType, this, WandCfg, WandCfg.PostSlotTriggerChargeRatio);
	}

	private void ShootSpellGroup(SpellShootGroup group, ShootSpellSpatialInfo spatialInfo, SpellInitialParameter.Builder sipBuilder, float reverseCopyShootRate)
	{
		ShootSpellUtils.ShootSpellGroup(group.Copy(), spatialInfo, sipBuilder, reverseCopyShootRate);
	}

	public SpellInitialParameter.Builder CreateSIPBuilder(bool fromPostSlots, bool fromEcho = false)
	{
		SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
		builder.ApplyUnitEffect(PlayerMgr.Inst.PlayerEtt);
		using EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerController_Dots));
		Entity singletonEntity = entityQuery.GetSingletonEntity();
		Entity entity = (passiveAutoWand ? PassiveWandSpiritEntity : singletonEntity);
		builder.ApplyShooterEntity(entity, entity);
		WandPostSlotChargeData wandChargeData = null;
		if (fromPostSlots)
		{
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
			{
				sip.shootFromPostSlots = true;
			};
		}
		else
		{
			wandChargeData = BuildPostSlotChargeData();
		}
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
		{
			sip.shootFromEcho = fromEcho;
		};
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
		{
			sip.IsIgnoreWall = passiveRandomPosShoot || sip.IsIgnoreWall;
		};
		builder.ApplyWandEffect(this, wandChargeData);
		if (passiveAutoWand && passiveAutoWandShooterData != null)
		{
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
			{
				sip.ownerPpt = passiveAutoWandShooterData.wandPpt;
			};
		}
		return builder;
	}

	public (Vector3 finalShootPos, Vector3 finalTargetPos, Vector3 finalTargetDir) ModifyShootPosData(Vector3 shootPos, Vector3 targetPos, Vector3 targetDir, float extraAngle)
	{
		if (passiveRandomPosShoot)
		{
			shootPos = PlayerMgr.Inst.GetMousePoint() + Tool2D.GetDir(RandomPosShootAngleShift).normalized * RandomPosShootRadius;
			targetDir = Tool2D.IgnoreZV2ToV1(PlayerMgr.Inst.GetMousePoint(), shootPos);
			targetPos = PlayerMgr.Inst.GetMousePoint();
			SpawnLongRangeCastEffect(shootPos);
		}
		if (WandCfg.specialAbility == WandAbility.FourDirShoot)
		{
			targetDir = Tool2D.GetDir(targetDir, extraAngle);
			shootPos += targetDir * 0.5f;
		}
		return (shootPos, targetPos, targetDir);
	}

	public void SpawnLongRangeCastEffect(Vector3 pos)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_LongRangeCastEffect", pos, 0.7f);
	}

	private void Shoot(bool fromEcho)
	{
		if (!fromEcho)
		{
			WandExtend.TryTriggerEchoEffect(this);
		}
		if (WandCfg.specialAbility == WandAbility.FourDirShoot)
		{
			for (int i = 0; i < 4; i++)
			{
				int angle = 90 * (i + 1) + 45;
				SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false, fromEcho);
				builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
				{
					parameter.FourDirWandAngle = angle;
				};
				(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, angle);
				ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
				ShootNormalSlotsSpellGroup(currentShootGroup, spatialInfo, builder);
			}
		}
		else
		{
			SpellInitialParameter.Builder parameterBuilder = CreateSIPBuilder(fromPostSlots: false, fromEcho);
			(Vector3, Vector3, Vector3) tuple2 = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, 0f);
			ShootSpellSpatialInfo spatialInfo2 = ShootSpellSpatialInfo.ToPoint(tuple2.Item1, tuple2.Item2, tuple2.Item3);
			ShootNormalSlotsSpellGroup(currentShootGroup, spatialInfo2, parameterBuilder);
		}
		ApplyShootGroupRecoil(new SpellShootGroup[1] { currentShootGroup });
		if (passiveRandomPosShoot)
		{
			RandomPosShootAngleShift -= RandomPosShootAngleShiftPerShoot;
		}
	}

	public void ShootPassiveChargeOne(Spell4004ChargeStars star)
	{
		if (!ChargeStars.Contains(star))
		{
			Debug.LogError("不能释放不在这个法杖上的星星");
			return;
		}
		SpellShootGroup spellShootGroup = currentShootGroup.Copy(currentShootGroup.OwnerShootData);
		spellShootGroup.Shoots = spellShootGroup.Shoots.Where((SpellShootData e) => !e.Spell.GetFinalConfig().abilityType.IsChargingSpell()).ToArray();
		if (spellShootGroup.Shoots.Length != 0)
		{
			if (WandCfg.specialAbility == WandAbility.FourDirShoot)
			{
				for (int i = 0; i < 4; i++)
				{
					int angle = 90 * (i + 1) + 45;
					(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, angle);
					ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(star.transform.position, tuple.Item2, tuple.Item3);
					SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
					builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
					{
						parameter.ChargeStar = star.GetComponent<Spell4004ChargeStars>();
						parameter.Shooter = parameter.ChargeStar.Entity;
						parameter.FourDirWandAngle = angle;
					};
					ShootNormalSlotsSpellGroup(spellShootGroup, spatialInfo, builder);
				}
			}
			else
			{
				SpellInitialParameter.Builder builder2 = CreateSIPBuilder(fromPostSlots: false);
				builder2.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
				{
					parameter.ChargeStar = star.GetComponent<Spell4004ChargeStars>();
					parameter.Shooter = parameter.ChargeStar.Entity;
				};
				ShootSpellSpatialInfo spatialInfo2 = ShootSpellSpatialInfo.ToPoint(star.transform.position, ShootTargetPosition, ShootDirection);
				ShootNormalSlotsSpellGroup(spellShootGroup, spatialInfo2, builder2);
			}
		}
		if (passiveBiAnBladeEnable)
		{
			PassiveTryShootBiAnBlade_Dots();
		}
		star.Release();
		ChargeStars.Remove(star);
	}

	private int ShootPassiveCharge(bool fromEcho)
	{
		if (!IsCharging)
		{
			Debug.LogError("没有蓄力，怎么蓄力射击呢？");
		}
		if (ChargeStars.Count == 0)
		{
			return 0;
		}
		if (!fromEcho)
		{
			WandExtend.TryTriggerEchoEffect(this);
		}
		int count = ChargeStars.Count;
		Spell4004ChargeStars[] array = ChargeStars.ToArray();
		foreach (Spell4004ChargeStars star in array)
		{
			ShootPassiveChargeOne(star);
		}
		ApplyShootGroupRecoil(new SpellShootGroup[1] { currentShootGroup });
		return count;
	}

	private void TryShootPostSpells()
	{
		if (!(PostSlotCurrentCharge < PostSlotMaxCharge) && !(PostSlotMaxCharge <= 0f) && postSlotShootGroups.Count != 0)
		{
			ShootPostSlotsSpell();
		}
	}

	private void ShootPostSlotsSpell()
	{
		if (PostSlotCurrentCharge < PostSlotMaxCharge)
		{
			Debug.LogWarning("后置槽充能不足，但仍然想要释放法术，这应该算 bug 了");
		}
		WandExtend.TryTriggerEchoEffect(this);
		int num = 0;
		int num2 = Mathf.Max(10, Mathf.FloorToInt(PostSlotCurrentCharge / PostSlotMaxCharge / 2f));
		float reverseCopyShootRate = ((PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy != null) ? ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy.int1.result / 100f) : 0f);
		while (true)
		{
			foreach (SpellShootGroup postSlotShootGroup in postSlotShootGroups)
			{
				if (WandCfg.specialAbility == WandAbility.FourDirShoot)
				{
					for (int i = 0; i < 4; i++)
					{
						int angle = 90 * (i + 1) + 45;
						SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: true);
						if (passiveAutoWand && PassiveWandSpiritEntity != Entity.Null)
						{
							builder.ApplyShooterEntity(PassiveWandSpiritEntity, PassiveWandSpiritEntity);
						}
						builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
						{
							parameter.FourDirWandAngle = angle;
						};
						(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(ShootPosition, ShootTargetPosition, ShootDirection, angle);
						ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
						ShootSpellGroup(postSlotShootGroup, spatialInfo, builder, reverseCopyShootRate);
					}
				}
				else
				{
					SpellInitialParameter.Builder builder2 = CreateSIPBuilder(fromPostSlots: true);
					if (passiveAutoWand && PassiveWandSpiritEntity != Entity.Null)
					{
						builder2.ApplyShooterEntity(PassiveWandSpiritEntity, PassiveWandSpiritEntity);
					}
					ShootSpellSpatialInfo spatialInfo2 = ShootSpellSpatialInfo.ToPoint(ShootPosition, ShootTargetPosition, ShootDirection);
					ShootSpellGroup(postSlotShootGroup, spatialInfo2, builder2, reverseCopyShootRate);
				}
			}
			PostSlotCurrentCharge -= PostSlotMaxCharge;
			if (PostSlotCurrentCharge < PostSlotMaxCharge || num >= num2)
			{
				break;
			}
			num++;
		}
		if (passiveBiAnBladeEnable)
		{
			PassiveTryShootBiAnBlade_Dots();
		}
		ApplyShootGroupRecoil(postSlotShootGroups.ToArray());
	}

	private void ShootNormalSlotsSpellGroup(SpellShootGroup group, ShootSpellSpatialInfo spatialInfo, SpellInitialParameter.Builder parameterBuilder)
	{
		float reverseCopyShootRate = ((PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy != null) ? ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy.int1.result / 100f) : 0f);
		ShootSpellGroup(group, spatialInfo, parameterBuilder, reverseCopyShootRate);
		WandPostSlotTrigger.PostSlotCastSpellTriggerCheck(WandCfg);
	}

	private void ApplyShootGroupRecoil(SpellShootGroup[] targetGroups)
	{
		float num = targetGroups.GetMultipleGroupRecoil_FinalPlayerValue(this);
		if (passiveRandomPosShoot)
		{
			num = 0f;
		}
		if (num == 0f)
		{
			return;
		}
		int num2;
		Entity entity;
		if (passiveAutoWand)
		{
			num2 = ((PassiveWandSpiritEntity != Entity.Null) ? 1 : 0);
			if (num2 != 0)
			{
				entity = PassiveWandSpiritEntity;
				goto IL_004f;
			}
		}
		else
		{
			num2 = 0;
		}
		entity = PlayerMgr.Inst.PlayerEtt;
		goto IL_004f;
		IL_004f:
		Entity entity2 = entity;
		Vector3 vector = ((num2 != 0) ? (-passiveAutoWandShooterData.wandObjectScript.wandRotateTransform.transform.right * num) : (-PlayerMgr.Inst.PlayerDir * num));
		UnitProperty_Dots componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<UnitProperty_Dots>(entity2);
		componentData.TakeKnockback(vector);
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity2, componentData);
	}

	public void EnterNextGroup(bool setCoolDownOrInterval)
	{
		currentSpellGroupsIndex++;
		if (currentSpellGroupsIndex >= shootGroups.Count)
		{
			if (setCoolDownOrInterval)
			{
				float fps = GameMgr.Inst.GetFps();
				float num2 = (CoolingTimer = (GeneralTool.IsLowFpsOptimizeActive(40f) ? Mathf.Max(Mathf.Max(0.015f, WandCoolDown), 0.08f * (fps / 40f)) : Mathf.Max(0.015f, WandCoolDown)));
			}
			TryUseWandAbility_ChangeInstentCoolDownAndFullMana_InstentCoolDown();
			currentSpellGroupsIndex = 0;
		}
		else if (setCoolDownOrInterval)
		{
			float num4 = (ShootIntervalTimer = (ShootIntervalTimer = WandShootInterval));
		}
		UI_UpdatePreShootHint();
	}

	public float GetManaToPostChargeEffect()
	{
		if (WandCfg == null)
		{
			return 0f;
		}
		if (WandCfg.postSlots.Length == 0)
		{
			return 0f;
		}
		return passiveManaToPostRatio * GetWandMpRecoverSpeed();
	}

	public void CostMp(float cost)
	{
		if (passiveBlueRuneCount > 0)
		{
			blueRuneCharge += math.min(cost, CurrentMP);
		}
		if (cost <= CurrentMP)
		{
			CurrentMP -= cost;
			return;
		}
		float cost2 = cost - CurrentMP;
		CurrentMP = 0f;
		PlayerMgr.Inst.CostMpFromWandAbility(this, cost2);
	}

	public void GainMP(float amount)
	{
		CurrentMP = Mathf.Clamp(CurrentMP + amount, 0f, MaxMP);
	}

	private bool CanMimic(WandSlotType mimicSlotType, int mimicIndex, SpellConfig target)
	{
		if (WandCfg.GetSlotsData(mimicSlotType)[mimicIndex].isAllFieldSharedSpell)
		{
			return true;
		}
		if (target.abilityType == SpellAbilityType.AllFieldEnhance)
		{
			return false;
		}
		int slotCost = target.slotCost;
		SlotData[] self = WandCfg.GetSlotsData(mimicSlotType).Bag_DeepCopy();
		self.Bag_RemoveSlot(mimicIndex);
		return self.Bag_SpaceCountOfSideThroughSpell(WandCfg.GetSlotsLockState(mimicSlotType), mimicIndex) >= slotCost;
	}

	public void CheckSpellListForManaTendrilEffect(WandSlotType slotType)
	{
		if (WandCfg == null)
		{
			return;
		}
		SlotData[] slotsData = WandCfg.GetSlotsData(slotType);
		bool[] slotsLockState = WandCfg.GetSlotsLockState(slotType);
		for (int i = 0; i < slotsData.Length; i++)
		{
			SlotData slotData = slotsData[i];
			if (slotData != null && !slotData.isSealSlot && slotData.GetFinalConfig().abilityType == SpellAbilityType.ManaTendril && !slotData.isAllFieldSharedSpell)
			{
				SlotData slotData2 = slotData.Copy();
				slotsData.Bag_RemoveSlot(i);
				int num = (slotData2.specialInt = slotsData.Bag_GetRightSpace(slotsLockState, i));
				slotsData.Bag_SetSlot(slotsLockState, slotData2, i);
			}
		}
	}

	private void CheckSpellListForMimicEffect(WandSlotType slotType)
	{
		if (WandCfg == null)
		{
			return;
		}
		SlotData[] slotsData = WandCfg.GetSlotsData(slotType);
		SpellConfig spellConfig = null;
		for (int num = slotsData.Length - 1; num >= 0; num--)
		{
			SlotData slotData = slotsData[num];
			if (slotData != null && !slotData.isSealSlot)
			{
				SpellConfig configIgnoreMimic = slotData.GetConfigIgnoreMimic();
				if (configIgnoreMimic.abilityType != SpellAbilityType.Mimic)
				{
					spellConfig = configIgnoreMimic;
				}
				else if (spellConfig == null || !CanMimic(slotType, num, spellConfig))
				{
					slotData.mimicSpellID = 0;
					if (slotsData.Bag_GetSlotSize(num) > 1)
					{
						slotsData.Bag_ClearSeal(num);
						UIPlayerDataMgr.Inst.WandUpdate(WandIndex);
					}
					UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, num, slotType, slotData.isAllFieldSharedSpell ? WandSlotIconVisualEffect.AllFieldEnhance : WandSlotIconVisualEffect.Normal);
					if ((bool)uISlotWand)
					{
						uISlotWand.SetIcon(SpellConfig.dic[31091], slotData.GetLevelIgnoreMimic());
					}
				}
				else
				{
					int num2 = (slotData.mimicSpellID = SlotData.GetMimicSpellLevel(configIgnoreMimic.id, spellConfig.id));
					bool[] slotsLockState = WandCfg.GetSlotsLockState(slotType);
					if (slotsData.Bag_GetSlotSize(num) != slotData.GetFinalSlotCost() && !slotData.isAllFieldSharedSpell)
					{
						slotsData.Bag_RemoveSlot(num);
						slotsData.Bag_SetSlotWithPush(slotsLockState, slotData, num);
						UIPlayerDataMgr.Inst.WandUpdate(WandIndex);
					}
					UISlotWand uISlotWand2 = UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, num, slotType, WandSlotIconVisualEffect.Mimic);
					if ((bool)uISlotWand2)
					{
						uISlotWand2.SetIcon(spellConfig, slotData.GetFinalLevel());
					}
				}
			}
		}
	}

	private void CheckSpellListForLevelEnhanceEffect(WandSlotType slotType)
	{
		if (WandCfg == null)
		{
			return;
		}
		SlotData[] slotsData = WandCfg.GetSlotsData(slotType);
		SlotData slotData = null;
		int slotIndex = 0;
		bool flag = false;
		foreach (SlotData slotData2 in slotsData)
		{
			if (slotData2 != null && !slotData2.isSealSlot)
			{
				slotData2.slotSpellExtraLevel = 0;
			}
		}
		for (int j = 0; j < slotsData.Length; j++)
		{
			SlotData slotData3 = slotsData[j];
			if (slotData3 == null || slotData3.isSealSlot)
			{
				continue;
			}
			SpellConfig finalConfig = slotData3.GetFinalConfig();
			if (flag)
			{
				slotData3.slotSpellExtraLevel++;
				flag = false;
				ApllyEnhanceLevelToTargetSLot(j, slotType, finalConfig, slotData3.GetFinalLevel());
			}
			if (finalConfig.abilityType == SpellAbilityType.SpellLevelEnhance)
			{
				if (slotData != null)
				{
					slotData.slotSpellExtraLevel++;
					ApllyEnhanceLevelToTargetSLot(slotIndex, slotType, slotData.GetConfigIgnoreMimic(), slotData.GetFinalLevel());
				}
				slotData3.slotSpellExtraLevel++;
				ApllyEnhanceLevelToTargetSLot(j, slotType, finalConfig, slotData3.GetFinalLevel());
				flag = true;
			}
			slotData = slotsData[j];
			slotIndex = j;
		}
	}

	private void ApllyEnhanceLevelToTargetSLot(int slotIndex, WandSlotType slotType, SpellConfig spellCfg, int finalLevel)
	{
		UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, slotIndex, slotType, WandSlotIconVisualEffect.Normal);
		if ((bool)uISlotWand)
		{
			uISlotWand.SetIcon(spellCfg, finalLevel);
		}
	}

	public SlotData[] SimpleCheckTargetSlotsMimicEffect(SlotData[] targetArray)
	{
		SlotData[] array = targetArray.Copy();
		SpellConfig spellConfig = null;
		for (int num = array.Length - 1; num >= 0; num--)
		{
			SlotData slotData = array[num];
			if (slotData != null && !slotData.isSealSlot)
			{
				SpellConfig configIgnoreMimic = slotData.GetConfigIgnoreMimic();
				if (configIgnoreMimic.abilityType != SpellAbilityType.Mimic)
				{
					spellConfig = configIgnoreMimic;
				}
				else if (slotData.isAllFieldSharedSpell)
				{
					if (spellConfig != null)
					{
						int num2 = (slotData.mimicSpellID = SlotData.GetMimicSpellLevel(configIgnoreMimic.id, spellConfig.id));
						UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, num, WandSlotType.Normal, WandSlotIconVisualEffect.Mimic);
						if ((bool)uISlotWand)
						{
							uISlotWand.SetIcon(spellConfig, slotData.GetFinalLevel());
						}
					}
					else
					{
						slotData.mimicSpellID = 0;
						UISlotWand uISlotWand2 = UIPlayerDataMgr.Inst.WandSetSlotIconVisualEffect(WandIndex, num, WandSlotType.Normal, slotData.isAllFieldSharedSpell ? WandSlotIconVisualEffect.AllFieldEnhance : WandSlotIconVisualEffect.Normal);
						if ((bool)uISlotWand2)
						{
							uISlotWand2.SetIcon(SpellConfig.dic[31091], slotData.GetLevelIgnoreMimic());
						}
					}
				}
			}
		}
		return targetArray;
	}

	private void ChargeControlUpdate()
	{
		if (IsCharging)
		{
			UpdateChargeAuraPosition();
			SEMgr.Inst.loopSEDurations[SEMgr.Inst.spell4004Loop] += 0.1f;
			TryChargeOnce();
		}
	}

	public void TryChargeOnce()
	{
		if (CanShootCurrentGroup() && ChargeStars.Count < passiveChargeCountLimit)
		{
			float groupManaCost_FinalPlayerValue = currentShootGroup.GetGroupManaCost_FinalPlayerValue(this);
			CostMp(groupManaCost_FinalPlayerValue);
			ChargeOnce();
			ShootIntervalTimer = WandShootInterval;
		}
		if (ChargeStars.Count >= passiveChargeCountLimit)
		{
			chargeAura.WandFullCharge();
		}
	}

	public (int RedRune, int GreenRune, int BlueRune) GetWandRuneCount()
	{
		if (WandCfg == null)
		{
			return (0, 0, 0);
		}
		return (passiveRedRuneCount, passiveGreenRuneCount, passiveBlueRuneCount);
	}

	private void UpdateChargeAuraPosition()
	{
		chargeAura.transform.position = ShootPosition;
		if (passiveAutoWand)
		{
			chargeAura.transform.position += new Vector3(0f, 0.2f, 0f);
		}
	}

	private void PostSlotTriggerStateUpdate()
	{
		if (WandCfg == null)
		{
			Debug.LogWarning("没有 CFG 的法杖不能更新后置格子");
		}
		else if (WandCfg.postSlots.Length != 0)
		{
			if (!passiveAutoWand)
			{
				WandPostSlotTrigger.PostSlotMoveDistanceTriggerCheck(this);
			}
			WandPostSlotTrigger.PostSlotTimeTriggerCheck(this);
			TryShootPostSpells();
		}
	}

	private float GetWandPostSlotChargePercent()
	{
		if (WandCfg != null && WandCfg.postSlots.Length != 0 && PostSlotMaxCharge <= 0f)
		{
			return 0f;
		}
		return Mathf.Clamp(PostSlotCurrentCharge / PostSlotMaxCharge, 0f, 1f);
	}

	public void ChargePostSlots(float amount)
	{
		if ((!PlayerMgr.Inst || PlayerMgr.Inst.PlayerCtrller.isFrozen || (bool)PlayerMgr.Inst.ItemCtrller.potion_Petrifaction || PlayerMgr.Inst.PlayerCtrller.CanMotion) && PostSlotMaxCharge > 0f)
		{
			PostSlotCurrentCharge += amount * (PlayerMgr.Inst.GetPostSlotChargeEfficiency(WandCfg) + GetManaToPostChargeEffect());
		}
	}

	public float GetWandMpCorrection()
	{
		float num = 1f;
		num *= (float)WandCfg.costCorrection / 100f;
		num *= passiveMpCostCorrection;
		num *= PlayerMgr.Inst.MpCostRatioFromWandAbility();
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_CostCorrection != null)
		{
			num *= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_CostCorrection.int1.result / 100f;
		}
		return num;
	}

	public float GetWandAllEnhanceMpCorrection()
	{
		float num = 1f;
		foreach (SlotData wandAllEnhance in GetWandAllEnhanceList())
		{
			SpellConfig finalConfig = wandAllEnhance.GetFinalConfig();
			if (finalConfig.mpCostMulDivCorrection != 0f)
			{
				num *= finalConfig.mpCostMulDivCorrection / 100f;
			}
		}
		return num;
	}

	private void RecalculateSpellGroups()
	{
		shootGroups.Clear();
		shootGroups = SpellGroupParser.Parse(WandCfg.normalSlots, WandCfg.shootCount).ToList();
		currentSpellGroupsIndex = 0;
		postSlotShootGroups.Clear();
		postSlotShootGroups = SpellGroupParser.Parse(WandCfg.postSlots, WandCfg.shootCount).ToList();
		PostSlotCurrentCharge = 0f;
		PostSlotMaxCharge = postSlotShootGroups.Sum((SpellShootGroup e) => e.GetGroupManaCost_FinalPlayerValue(this));
	}

	public List<SlotData> GetWandAllEnhanceList()
	{
		List<SlotData> list = new List<SlotData>();
		foreach (SlotData item in WandCfg.normalSlots.Where((SlotData e) => e != null && !e.isSealSlot))
		{
			if (item.GetFinalConfig().useType == SpellType.Enhance)
			{
				list.Add(item);
			}
		}
		foreach (SlotData item2 in WandCfg.postSlots.Where((SlotData e) => e != null && !e.isSealSlot))
		{
			if (item2.GetFinalConfig().useType == SpellType.Enhance)
			{
				list.Add(item2);
			}
		}
		return list;
	}

	public SpellShootData GetShootDataByShootableSpell(SlotData slotData)
	{
		SpellType useType = slotData.GetConfigIgnoreMimic().useType;
		if (useType != 0 && useType != SpellType.Summon)
		{
			Debug.LogWarning("不能获取不可射击法术的 ShootData");
			return null;
		}
		Queue<SpellShootData> queue = new Queue<SpellShootData>();
		foreach (SpellShootData item2 in shootGroups.SelectMany((SpellShootGroup e) => e.Shoots))
		{
			queue.Enqueue(item2);
		}
		foreach (SpellShootData item3 in postSlotShootGroups.SelectMany((SpellShootGroup e) => e.Shoots))
		{
			queue.Enqueue(item3);
		}
		SpellShootData result;
		while (queue.TryDequeue(out result))
		{
			if (result.Spell == slotData)
			{
				return result;
			}
			if (result.SubGroup != null)
			{
				SpellShootData[] shoots = result.SubGroup.Shoots;
				foreach (SpellShootData item in shoots)
				{
					queue.Enqueue(item);
				}
			}
		}
		return null;
	}

	public IEnumerable<SlotData> ResizeSlots(WandSlotType slotType, int newSize)
	{
		List<SlotData> list = new List<SlotData>();
		SlotData[] array = WandCfg.GetSlotsData(slotType);
		bool[] array2 = WandCfg.GetSlotsLockState(slotType);
		if (newSize == array.Length)
		{
			return list;
		}
		bool flag = array.Any((SlotData e) => e != null && !e.isSealSlot && e.IsPostSlotExtender());
		GetAllFieldSharedSpellLengthInNormalSlots();
		while (newSize < array.Length && array.Bag_SpaceCount(array2) > 0 && !flag)
		{
			int num = 0;
			bool flag2 = false;
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j])
				{
					num = j + 1;
				}
			}
			int i;
			for (i = num; i < array.Length; i++)
			{
				if (flag2)
				{
					break;
				}
				if (array[i] != null || array2[i])
				{
					if (i == array.Length - 1)
					{
						flag2 = true;
					}
					continue;
				}
				array = array.Where((SlotData _, int index) => index != i).ToArray();
				array2 = array2.Where((bool _, int index) => index != i).ToArray();
				break;
			}
			if (flag2)
			{
				break;
			}
		}
		if (newSize < array.Length)
		{
			for (int k = newSize; k < array.Length; k++)
			{
				SlotData slotData = array[k];
				if (slotData != null && !array[k].isAllFieldSharedSpell)
				{
					int num2 = k;
					if (slotData.isSealSlot)
					{
						num2 = array.Bag_GetOwnerSlotIndex(num2);
					}
					list.Add(array[num2]);
					array.Bag_RemoveSlot(num2);
				}
			}
		}
		Array.Resize(ref array, newSize);
		Array.Resize(ref array2, newSize);
		WandCfg.SetSlotsData(slotType, array);
		WandCfg.SetSlotsLockState(slotType, array2);
		return list;
	}

	public void TryUseWandAbility_ChanceInstentCoolDownAndFullMana_FullMana()
	{
		if (WandCfg.specialAbility == WandAbility.ChanceInstentCoolDownAndFullMana && (float)UnityEngine.Random.Range(0, 100) < WandCfg.float1)
		{
			CurrentMP = MaxMP;
		}
	}

	private void TryUseWandAbility_ChangeInstentCoolDownAndFullMana_InstentCoolDown()
	{
		if (WandCfg.specialAbility == WandAbility.ChanceInstentCoolDownAndFullMana && (float)UnityEngine.Random.Range(0, 100) < WandCfg.float2)
		{
			CoolingTimer = 0f;
		}
	}

	public float GetPassiveRotateRadiuRatio()
	{
		return UnityEngine.Random.Range(passiveRandomMaxRotateRadiuRatio, passiveRandomMinRotateRadiuRatio);
	}

	public void UpdateTeammateHealTimer()
	{
		if (WandCfg != null && WandCfg.specialAbility == WandAbility.HealNearByTeammate && (!(PlayerMgr.Inst.SelectedWand != this) || passiveAutoWand))
		{
			healNearbyTeammateTimer += Time.deltaTime;
			if (!(healNearbyTeammateTimer < WandCfg.float3))
			{
				healNearbyTeammateTimer -= WandCfg.float3;
				Vector3 healCenterPoint = (passiveAutoWand ? passiveAutoWandShooterData.currentPosition : PlayerMgr.Inst.PlayerPoint);
				HealNearByTeammate(healCenterPoint, WandCfg.int1, WandCfg.float1, WandCfg.float2 * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false)));
			}
		}
	}

	private void UpdatePeriodAutoFullManaTimer()
	{
		if (WandCfg != null && WandCfg.specialAbility == WandAbility.PeriodRecoverAllMp)
		{
			autoFullManaTimer += Time.deltaTime;
			if (!(autoFullManaTimer < WandCfg.float1))
			{
				autoFullManaTimer -= WandCfg.float1;
				GainMP(MaxMP);
			}
		}
	}

	private void HealNearByTeammate(Vector3 healCenterPoint, int healPoint, float healPercent, float healRange)
	{
		GeneralTool.TryHealTargetTeammates(healCenterPoint, healPoint, healPercent, healRange, LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList);
		GeneralTool.TryHealTargetTeammates(healCenterPoint, healPoint, healPercent, healRange, LevelMgr.Inst.CurrentRoomCtrller.TeammateNotAttackEttList);
	}

	private void UpdateGreenRuneChargeState()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		for (int num = GreenRuneList.Count - 1; num >= 0; num--)
		{
			Entity entity = GreenRuneList[num];
			if (!entityManager.HasComponent<LocalTransform>(entity) || entityManager.IsComponentEnabled<SpellDestroyTag>(entity))
			{
				GreenRuneList.RemoveAt(num);
			}
		}
		if (passiveGreenRuneCount <= 0 || GreenRuneList.Count >= 5)
		{
			return;
		}
		using (entityManager.CreateEntityQuery(typeof(DynamicOptimizeData)))
		{
			float deltaTime = Time.deltaTime;
			passiveGreenRuneChargeTimer += deltaTime;
			if (passiveGreenRuneChargeTimer >= 3f)
			{
				TrySpawnGreenRuneBall();
				passiveGreenRuneChargeTimer -= 3f;
			}
		}
	}

	public void TrySpawnGreenRuneBall(bool isForceSpawn = false, float3 forceSpawnPos = default(float3))
	{
		if (WandCfg == null || (passiveGreenRuneCount <= 0 && !isForceSpawn))
		{
			return;
		}
		SpawnRuneBall(isForceSpawn, forceSpawnPos);
		int item = PlayerMgr.Inst.GetPlayerRuneCount().GreenRune;
		int runeEffectLevel = PlayerMgr.Inst.GetRuneEffectLevel(item);
		int num = (int)math.floor((float)item / 10f);
		if (IsWandHasFallEffectEnhance() && runeEffectLevel >= 2 && num > 0)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			EntityQueryDesc entityQueryDesc = new EntityQueryDesc();
			entityQueryDesc.All = new ComponentType[2]
			{
				typeof(TeammateData),
				typeof(LocalTransform)
			};
			entityQueryDesc.Disabled = new ComponentType[1] { typeof(SpellDestroyTag) };
			EntityQueryDesc entityQueryDesc2 = entityQueryDesc;
			NativeArray<Entity> nativeArray = entityManager.CreateEntityQuery(entityQueryDesc2).ToEntityArray(Allocator.Temp);
			for (int num2 = nativeArray.Length - 1; num2 > 0; num2--)
			{
				int num3 = UnityEngine.Random.Range(0, num2 + 1);
				int index = num2;
				int index2 = num3;
				Entity entity = nativeArray[num3];
				Entity entity2 = nativeArray[num2];
				Entity entity4 = (nativeArray[index] = entity);
				entity4 = (nativeArray[index2] = entity2);
			}
			for (int i = 0; i < math.min(nativeArray.Length, num); i++)
			{
				SpawnRuneBall(isForceSpawn, forceSpawnPos, isFallBonusSpawn: true, entityManager.GetComponentData<LocalTransform>(nativeArray[i]).Position);
			}
		}
	}

	private void SpawnRuneBall(bool isForceSpawn, float3 forceSpawnPos, bool isFallBonusSpawn = false, float3 fallBonusSpawnTargetPos = default(float3))
	{
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		float3 @float = (isForceSpawn ? forceSpawnPos : ((float3)PlayerMgr.Inst.PlayerPoint));
		float3 float2 = @float;
		if (IsWandHasFallEffectEnhance())
		{
			float2 = ((!isFallBonusSpawn) ? ((float3)(passiveAutoWand ? passiveAutoWandShooterData.targetPosition : PlayerMgr.Inst.GetMousePoint())) : fallBonusSpawnTargetPos);
		}
		(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(@float, float2, ShootDirection, 0f);
		ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
		int multiShootCount = GetWandMultishootEnhanceCount();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
		{
			sip.spelldataConfig.int2 = multiShootCount;
			sip.spelldataConfig.int1 = (isForceSpawn ? 1 : 0);
		};
		ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(40261), spatialInfo, builder, 0f);
	}

	public void RecordLV5GreenRuneSummonCount()
	{
		greenRuneLV5SummonCount++;
		if (greenRuneLV5SummonCount >= 10)
		{
			greenRuneLV5SummonCount -= 10;
			TrySpawnGreenRuneBall(isForceSpawn: true, PlayerMgr.Inst.PlayerPoint);
		}
	}

	private void UpdateBlueRuneChargeState()
	{
		if (passiveBlueRuneCount <= 0)
		{
			return;
		}
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		float @float = SpellConfig.dic[40271].float2;
		int wandMultishootEnhanceCount = GetWandMultishootEnhanceCount();
		if (blueRuneCharge >= @float)
		{
			int num = Mathf.Max(1, Mathf.FloorToInt(blueRuneCharge / @float));
			if (PlayerMgr.Inst.GetRuneEffectLevel(PlayerMgr.Inst.GetPlayerRuneCount().BlueRune) >= 4)
			{
				blueRuneTriggerCounter++;
			}
			blueRuneRemainCount += 2 * num * wandMultishootEnhanceCount;
			blueRuneCharge -= num * (int)@float;
		}
		if (blueRuneRemainCount <= 0)
		{
			return;
		}
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(DynamicOptimizeData));
		Vector3 shootPos = ((passiveAutoWand && passiveAutoWandShooterData != null) ? passiveAutoWandShooterData.shootPosition : (PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.3f, 0f)));
		Vector3 targetPos = (passiveAutoWand ? passiveAutoWandShooterData.targetPosition : PlayerMgr.Inst.GetMousePoint());
		(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(shootPos, targetPos, ShootDirection, 0f);
		ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
		float shootPower = 1f;
		int num2 = Mathf.Min(3, blueRuneRemainCount);
		int num3 = 0;
		if (blueRuneRemainCount - num2 >= 100)
		{
			num3 = Mathf.FloorToInt((float)(blueRuneRemainCount - num2) * 0.05f);
			shootPower *= 1f + (float)num3 / (float)num2;
		}
		int num4 = num2 + num3;
		blueRuneRemainCount -= num4;
		float manaReGenRatio = (float)num4 / (float)num2;
		int num5 = (GameMgr.IsMobile_Static ? 30 : 60);
		float lowFrameDamageIntervalTimeScale = entityQuery.GetSingleton<DynamicOptimizeData>().GetLowFrameDamageIntervalTimeScale(num5, 10f, 5f);
		int num6 = Mathf.CeilToInt((float)num2 / lowFrameDamageIntervalTimeScale);
		shootPower *= (float)num2 / (float)num6;
		manaReGenRatio *= (float)num2 / (float)num6;
		for (int i = 0; i < num6; i++)
		{
			SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
			{
				sip.extraScatter = 0f;
				sip.finalDamageRatio *= shootPower;
				sip.spelldataConfig.float3 = manaReGenRatio;
				sip.spelldataConfig.int2 = 0;
				sip.spelldataConfig.int3 = 0;
			};
			ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(40271), spatialInfo, builder, 0f);
		}
		if (blueRuneTriggerCounter < 2)
		{
			return;
		}
		blueRuneTriggerCounter -= 2;
		num5 = (GameMgr.IsMobile_Static ? 30 : 60);
		lowFrameDamageIntervalTimeScale = entityQuery.GetSingleton<DynamicOptimizeData>().GetLowFrameDamageIntervalTimeScale(num5, 10f, 2.5f);
		int num7 = Mathf.CeilToInt((float)wandMultishootEnhanceCount / lowFrameDamageIntervalTimeScale);
		shootPower = (float)wandMultishootEnhanceCount / (float)num7;
		for (int j = 0; j < num7; j++)
		{
			SpellInitialParameter.Builder builder2 = CreateSIPBuilder(fromPostSlots: false);
			builder2.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
			{
				sip.extraScatter = 0f;
				sip.finalDamageRatio *= shootPower * 1.5f;
				sip.spelldataConfig.float3 = manaReGenRatio;
				sip.spelldataConfig.int2 = (int)(5f * shootPower * 100f);
				sip.spelldataConfig.int3 = 1;
			};
			ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(40271), spatialInfo, builder2, 0f);
		}
	}

	public void TryShootRedRune(float3 targetPoint, bool isCriticalHit)
	{
		int redRuneCount = PlayerMgr.Inst.GetPlayerRuneCount().RedRune;
		int runeLevel = PlayerMgr.Inst.GetRuneEffectLevel(redRuneCount);
		if (redRuneCoolDown > 0f || (UnityEngine.Random.Range(0f, 1f) > 0.2f && (runeLevel < 1 || !isCriticalHit)))
		{
			return;
		}
		isTriggerRedRuneInThisFrame = true;
		(Vector3, Vector3, Vector3) tuple = ModifyShootPosData(targetPoint, (Vector3)targetPoint + ShootDirection * 0.1f, ShootDirection, 0f);
		ShootSpellSpatialInfo spatialInfo = ShootSpellSpatialInfo.ToPoint(tuple.Item1, tuple.Item2, tuple.Item3);
		bool isAOESlash = redRuneShootCounter >= 10f;
		bool isSuperSlash = runeLevel >= 2 && (UnityEngine.Random.Range(0f, 1f) <= 0.2f || isAOESlash);
		if (redRuneShootCounter >= 10f)
		{
			redRuneShootCounter -= 10f;
		}
		if (runeLevel >= 4 && !isAOESlash)
		{
			redRuneShootCounter += 1f;
		}
		SpellInitialParameter.Builder builder = CreateSIPBuilder(fromPostSlots: false);
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter sip)
		{
			sip.extraScatter = 0f;
			sip.spelldataConfig.float2 = (isAOESlash ? 1 : 0);
			sip.spelldataConfig.float3 = (isSuperSlash ? ((float)redRuneCount * 0.1f) : 0f);
			sip.spelldataConfig.int1 = redRuneCount;
			sip.spelldataConfig.int2 = runeLevel;
			if (!sip.spellIsFall)
			{
				sip.finalMovementType = SpellSpecialMovementType.Normal;
			}
		};
		ShootSpellGroup(GetApplyWandAllEnhanceEffectShootGroup(40251), spatialInfo, builder, 0f);
	}

	public SlotData[] GetUnusedEnhanceSlotData(out UnusedEnhanceType[] types)
	{
		List<SlotData> list = new List<SlotData>();
		List<UnusedEnhanceType> list2 = new List<UnusedEnhanceType>();
		(SlotData[], UnusedEnhanceType[]) tuple = GetUnusedSlotsAndType(WandCfg.GetValidSlotsData(normal: true, post: false));
		list.AddRange(tuple.Item1);
		list2.AddRange(tuple.Item2);
		tuple = GetUnusedSlotsAndType(WandCfg.GetValidSlotsData(normal: false, post: true));
		list.AddRange(tuple.Item1);
		list2.AddRange(tuple.Item2);
		types = list2.ToArray();
		return list.ToArray();
		(SlotData[], UnusedEnhanceType[]) GetUnusedSlotsAndType(SlotData[] slots)
		{
			if (passiveRuneHammerEnable || passiveLaserCrystalEnable || passiveUmbrellaEnable || passiveBiAnBladeEnable || passiveDaveHarpoonsEnable)
			{
				return (Array.Empty<SlotData>(), Array.Empty<UnusedEnhanceType>());
			}
			List<SlotData> list3 = new List<SlotData>();
			List<UnusedEnhanceType> list4 = new List<UnusedEnhanceType>();
			int num = slots.Length + 1;
			int num2 = -1;
			for (int i = 0; i < slots.Length; i++)
			{
				SpellType useType = slots[i].GetFinalConfig().useType;
				if (useType == SpellType.Summon || useType == SpellType.Missile)
				{
					num = i;
					break;
				}
			}
			for (int num3 = slots.Length - 1; num3 >= 0; num3--)
			{
				SpellType useType = slots[num3].GetFinalConfig().useType;
				if (useType == SpellType.Summon || useType == SpellType.Missile)
				{
					num2 = num3;
					break;
				}
			}
			for (int j = 0; j < slots.Length; j++)
			{
				if ((slots[j].GetConfigIgnoreMimic().abilityType != SpellAbilityType.Mimic || slots[j].mimicSpellID != 0) && slots[j].GetFinalConfig().useType == SpellType.Enhance)
				{
					if (slots[j].IsTrigger())
					{
						if (j < num)
						{
							list3.Add(slots[j]);
							list4.Add(UnusedEnhanceType.LeftNoSpell);
						}
						else if (j > num2)
						{
							list3.Add(slots[j]);
							list4.Add(UnusedEnhanceType.RightNoSpell);
						}
					}
					else if (slots[j].GetFinalConfig().abilityType != SpellAbilityType.SpellLevelEnhance && j > num2)
					{
						list3.Add(slots[j]);
						list4.Add(UnusedEnhanceType.RightNoSpell);
					}
				}
			}
			return (list3.ToArray(), list4.ToArray());
		}
	}

	public void Display_UpdateShowOrHide()
	{
		if (WandCfg == null)
		{
			Debug.LogWarning("法杖的 config 为空，怎么更新显示状态？");
			Display_Hide();
		}
		else if ((bool)sr_Wand)
		{
			tsf_WandRoot.gameObject.SetActive(value: true);
			tsf_SpecialWandRoot.gameObject.SetActive(value: false);
			sr_Wand.sprite = ABResources.LoadAsset<Sprite>(WandCfg.GetIconPath());
			if (WandCfg.specialAbility == WandAbility.LongWand || WandCfg.specialAbility == WandAbility.LongWandAndSpellBreaker || GameConstManaged.SpecialLongWandIdList.Contains(WandCfg.id))
			{
				tsf_WandRoot.gameObject.SetActive(value: false);
				tsf_SpecialWandRoot.gameObject.SetActive(value: true);
				sr_SpecialWand.sprite = ABResources.LoadAsset<Sprite>(WandCfg.GetIconPath() + "L");
				sr_SpecialWandShadow.sprite = sr_SpecialWand.sprite;
			}
			if (GameConstManaged.SpecialLongWandIdList.Contains(WandCfg.id))
			{
				tsf_SpecialWandRoot.transform.localPosition = new Vector3(0f, 0f, 0.002f);
			}
			else
			{
				tsf_SpecialWandRoot.transform.localPosition = Vector3.zero;
			}
			sr_WandShadow.sprite = sr_Wand.sprite;
			UpdateHandDisplay();
			if (PlayerMgr.Inst.SelectedWandIndex == WandIndex && !passiveAutoWand)
			{
				Display_Show();
			}
			else
			{
				Display_Hide();
			}
		}
	}

	public void Display_Show(bool playChoiceAnimation = true)
	{
		tsf_Layer.gameObject.SetActive(value: true);
		tsf_ShootPoint.gameObject.SetActive(value: true);
		if (passiveAutoWand)
		{
			TryDestroySpellBreakerHead();
		}
		else
		{
			TryCreateSpellBreakerHead();
		}
		if (playChoiceAnimation)
		{
			animator.SetTrigger("Choice");
		}
	}

	public void Display_Hide()
	{
		tsf_Layer.gameObject.SetActive(value: false);
		tsf_ShootPoint.gameObject.SetActive(value: false);
		if (passiveAutoWand)
		{
			TryCreateSpellBreakerHead();
		}
		else
		{
			TryDestroySpellBreakerHead();
		}
	}

	public void UpdateHandDisplay()
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null && PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons == null)
		{
			SpriteRenderer spriteRenderer = sr_Hand;
			Sprite sprite;
			switch (DataMgr.selectedWorldData.playerLook)
			{
			case PlayerLook.TVMan:
			case PlayerLook.SnowMan:
				sprite = sprite_HandTVMan;
				break;
			case PlayerLook.Frog:
				sprite = sprite_HandFrog;
				break;
			case PlayerLook.TapTap:
				sprite = sprite_HandTap;
				break;
			case PlayerLook.Horse:
				sprite = sprite_HandSpring;
				break;
			default:
				sprite = sprite_HandNormal;
				break;
			}
			spriteRenderer.sprite = sprite;
		}
		else
		{
			sr_Hand.sprite = sprite_HandNormal;
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			sr_Hand.enabled = false;
		}
		else
		{
			sr_Hand.enabled = true;
		}
	}

	private void UI_UpdateNoManaWarning()
	{
		UIWand uIWand = UIPlayerDataMgr.Inst.uiWands[WandIndex];
		if (!uIWand)
		{
			Debug.LogWarning($"为什么没有 {WandIndex} 号法杖的 UI？");
			return;
		}
		int num = 0;
		bool[] array = new bool[WandCfg.normalSlots.Length];
		foreach (SpellShootGroup shootGroup in shootGroups)
		{
			if (!(shootGroup.GetGroupManaCost_FinalPlayerValue(this) <= MaxMP + PlayerMgr.Inst.MaxManaAmountFromWandAbility()))
			{
				int[] array2 = (from e in shootGroup.GetCanShootSlotData().Where(delegate(SlotData e)
					{
						SpellType useType = e.GetFinalConfig().useType;
						return useType == SpellType.Missile || useType == SpellType.Summon;
					})
					select Array.IndexOf(WandCfg.normalSlots, e) into e
					where e >= 0
					select e).ToArray();
				foreach (int num2 in array2)
				{
					array[num2] = true;
				}
				num++;
			}
		}
		uIWand.UpdateNoMana(array, num > 0 && num == shootGroups.Count);
	}

	private void UI_UpdateUnusedWarning()
	{
		UIWand uIWand = UIPlayerDataMgr.Inst.uiWands[WandIndex];
		if (!uIWand)
		{
			Debug.LogWarning($"为什么没有 {WandIndex} 号法杖的 UI？");
			return;
		}
		UnusedEnhanceType[] types;
		SlotData[] unusedEnhanceSlotData = GetUnusedEnhanceSlotData(out types);
		Dictionary<SlotData, UnusedEnhanceType> dictionary = new Dictionary<SlotData, UnusedEnhanceType>();
		for (int i = 0; i < unusedEnhanceSlotData.Length; i++)
		{
			dictionary.Add(unusedEnhanceSlotData[i], types[i]);
		}
		uIWand.UpdateUnused(dictionary);
	}

	private void UI_ShowMPWarning()
	{
		SEMgr.Inst.PlaySE("SE_WandSlotLackMana", SEPlayMode.Unique);
		UIPlayerDataMgr.Inst.MPWarning();
	}

	private void UI_WandManaPercentUpdate()
	{
		if (MaxMP > 0f)
		{
			UIPlayerDataMgr.Inst.UpdateWandManaPercent(WandIndex, CurrentMP / MaxMP);
		}
		else
		{
			UIPlayerDataMgr.Inst.UpdateWandManaPercent(WandIndex, 0f);
		}
	}

	private void UI_PostSlotIconPercentUpdate()
	{
		if (WandCfg == null || WandCfg.postSlots.Length == 0)
		{
			return;
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < WandCfg.postSlots.Length; i++)
		{
			if (WandCfg.postSlots[i] != null)
			{
				list.Add(i);
			}
			else
			{
				list2.Add(i);
			}
		}
		UIPlayerDataMgr.Inst.WandPostSlotUpdate(WandIndex, list, GetWandPostSlotChargePercent());
		UIPlayerDataMgr.Inst.WandPostSlotUpdate(WandIndex, list2, 0f);
	}

	private void UI_HidePreShootHint()
	{
		UIPlayerDataMgr.Inst.uiWands[WandIndex].HideAllPreshoot();
	}

	private void UI_UpdateWandFlipState()
	{
		int num = ((!(PlayerMgr.Inst.GetMousePoint().x <= PlayerMgr.Inst.PlayerPoint.x)) ? 1 : (-1));
		tsf_WandRoot.localScale = new Vector3(num, 1f, 1f);
	}

	private void UI_UpdatePreShootHint()
	{
		if (currentShootGroup == null || CurrentMP + PlayerMgr.Inst.CurrentManaAmountFromWandAbility(this) < currentShootGroup.GetGroupManaCost_FinalPlayerValue(this))
		{
			UI_HidePreShootHint();
			return;
		}
		UIWand uIWand = UIPlayerDataMgr.Inst.uiWands[WandIndex];
		SlotData[] allSlotData = currentShootGroup.GetAllSlotData();
		List<int> list = new List<int>();
		for (int i = 0; i < WandCfg.normalSlots.Length; i++)
		{
			if (allSlotData.Contains(WandCfg.normalSlots[i]))
			{
				list.Add(i);
			}
		}
		uIWand.UpdatePreshoot(list.ToArray());
	}

	private void UI_UpdatePreShootHintMobile()
	{
		refreshTimer += Time.unscaledDeltaTime;
		if (refreshTimer > refreshInterval)
		{
			currentSpellGroupsIndexMobile++;
			if (currentSpellGroupsIndexMobile >= shootGroups.Count)
			{
				currentSpellGroupsIndexMobile = 0;
			}
			UI_SetPreShootHintMobile();
			refreshTimer = 0f;
		}
	}

	private void UI_SetPreShootHintMobile()
	{
		if (currentShootGroupMobile == null || MaxMP + PlayerMgr.Inst.CurrentManaAmountFromWandAbility(this) < currentShootGroupMobile.GetGroupManaCost_FinalPlayerValue(this) || PlayerMgr.Inst.SelectedWand != this)
		{
			UI_HidePreShootHint();
			return;
		}
		UIWand uIWand = UIPlayerDataMgr.Inst.uiWands[WandIndex];
		SlotData[] allSlotData = currentShootGroupMobile.GetAllSlotData();
		List<int> list = new List<int>();
		for (int i = 0; i < WandCfg.normalSlots.Length; i++)
		{
			if (allSlotData.Contains(WandCfg.normalSlots[i]))
			{
				list.Add(i);
			}
		}
		uIWand.UpdatePreshoot(list.ToArray());
	}

	private void UI_ShowNoShootableSpell()
	{
		if (Time.time - lastShowNoSpellTryShootUITime > 0.5f)
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002019.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.8f, 0f));
			lastShowNoSpellTryShootUITime = Time.time;
		}
	}
}
