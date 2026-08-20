using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitProperty : LayerCorrect
{
	public enum Affect_FrozenState
	{
		Normal,
		Frozening,
		FrozenImmune
	}

	private bool isSingleInitialized;

	public UmbrellaShieldController UmbrellaCtrl;

	[HideInInspector]
	public bool isRigidLerp_Dots;

	private bool original_RigidIsKinematic;

	[HideInInspector]
	public bool isFlipX;

	private int unitID;

	[HideInInspector]
	public UnitConfig unitCfg;

	public Entity myEntity;

	public int entityIndex;

	public bool entityValid;

	private GameObject blive_summonerName;

	private Text blive_summonerNameText;

	private int flyCounter;

	private int invincibleCounter;

	private int immuneKnockbackCounter;

	private float affect_SpikesTimer;

	private int immnueMucusCounter;

	private EffectController ec_Deceleration;

	private bool affect_IsMucusHit;

	private int immnueVenomCounter;

	private float affect_VenomDurationTimer;

	private float affect_VenomInjureTimer;

	private float affect_burnAttackIntervalTimer;

	private float affect_burnHPRatio;

	private float affect_burnHPRatioPerHit;

	private int immuneVoidCounter;

	public Spell3129VoidExplosion.VoidExplosionData voidExplosionData;

	private Spell3129VoidEffect ec_VoidEffect;

	private Vector3 affect_AbyssPoint;

	private float fallAbyssSpeed;

	private float affect_FrozenTimer;

	private GameObject go_FrozenEF;

	private float affect_ReverseMoveTimer;

	private EffectController ec_ReverseMove;

	private int immuneGroundAffectTimer;

	private float stuckCheckIntervalTimer;

	private bool isStuck;

	private float playerInvincibleFrameTimer;

	private float playerInvincibleFrameTwinkleTimer;

	private float playerInvincibleFrameTwinkleAlpha = 1f;

	private Vector3 beHitDir;

	private bool isBeHitColor;

	private float beHitColorTimer;

	private bool isBeHit;

	private bool beHitOut = true;

	private int beHitTimer;

	private GameObject damageReciveIncreseEffectGO;

	private float curse_MonsterRecoveryTimer;

	private Vector3 unitBodyCenterPoint = Vector3.zero;

	public SpriteRenderer[] SR_Models { get; set; }

	public MeshRenderer[] MR_Models { get; set; }

	public Transform[] MR_FlipRoots { get; set; }

	public Vector3[] MR_Models_LocalScale { get; set; }

	public bool[] MR_Models_NeedRebound { get; set; }

	public CapsuleCollider CC_Self { get; private set; }

	public BoxCollider BC_Self { get; private set; }

	public Animator Anima { get; private set; }

	public SkeletonAnimation SAnima { get; set; }

	public Rigidbody Rigid { get; private set; }

	public PlayerController PlayerCtrller { get; private set; }

	public UnitBase UnitBas { get; private set; }

	public Transform Tsf_BeHit { get; private set; }

	public bool AlreadyDead { get; private set; }

	public bool IsVelocityDeclice { get; set; } = true;


	public Color Color_NormalBody { get; set; } = Color.white;


	public Color BaseColor { get; protected set; } = Color.white;


	public bool CanTouch { get; set; } = true;


	public bool IsFly => flyCounter > 0;

	public bool IsInvincible => invincibleCounter > 0;

	public bool IsImmuneKnockback => immuneKnockbackCounter > 0;

	public bool IsImmuneMucus => immnueMucusCounter > 0;

	public bool affect_IsMucusDecelerate { get; set; }

	public float affect_MucusHitTimer { get; protected set; }

	public float affect_MucusMoveSpeedRatio { get; set; } = 1f;


	public float affect_MucusSpellSpeedRatio { get; set; } = 1f;


	public bool IsImmuneVenom => immnueVenomCounter > 0;

	public float affect_VenomCurrentStack { get; protected set; }

	public float affect_burnDurationTimer { get; protected set; }

	public bool IsImmuneVoid => immuneVoidCounter > 0;

	public float voidEffectTimer { get; protected set; }

	public bool Affect_InAbyss { get; set; }

	public Affect_FrozenState FronzenState { get; private set; }

	public float affect_FrozenTime { get; protected set; }

	public bool affect_IsReverseMove => affect_ReverseMoveTimer > 0f;

	public bool IsImmuneGroundAffect => immuneGroundAffectTimer > 0;

	private bool IsPlayerInInvincibleFrame => playerInvincibleFrameTimer > 0f;

	public float DamageReciveIncresePercent { get; private set; }

	public float damageReciveIncresePercentTimer { get; private set; }

	public bool isUnitDead { get; private set; }

	public Vector3 bodyCenterPoint
	{
		get
		{
			return unitBodyCenterPoint;
		}
		set
		{
			unitBodyCenterPoint = value;
		}
	}

	public bool CanBeTarget { get; set; } = true;


	public float MoveSpeed => unitCfg.moveSpeed * MoveSpeedRatio;

	public float MoveSpeedRatio
	{
		get
		{
			float num = 1f;
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_EnemyAddMove != null && unitCfg.IsSameCamp(UnitType.Monster))
			{
				num *= (float)(PlayerMgr.Inst.ItemCtrller.curseCfg_EnemyAddMove.int1.result + 100) / 100f;
			}
			if (unitCfg.unitType == UnitType.Player)
			{
				if (PlayerMgr.Inst.BaData != null)
				{
					num += PlayerMgr.Inst.BaData.extraMoveSpeedRatio;
				}
				if (PlayerMgr.Inst.ItemCtrller.curse_IsReverseMove)
				{
					num = 0f - num;
				}
				if (PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceMoveSpeed != null)
				{
					num -= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_ReduceMoveSpeed.int1.result / 100f;
				}
				if (PlayerMgr.Inst.ItemCtrller.relicCfg_ImmuneSurface != null)
				{
					num += (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ImmuneSurface.int1.result / 100f;
				}
				if (PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed != null)
				{
					num += (float)PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed.RelicCfg.int1.result / 100f;
				}
				if (PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing != null)
				{
					num += PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing.ExtraMoveSpeed;
				}
				if (PlayerMgr.Inst.ItemCtrller.relic_InjuredAddMoveSpeed != null)
				{
					num *= PlayerMgr.Inst.ItemCtrller.relic_InjuredAddMoveSpeed.MoveSpeedRatio;
				}
				if (PlayerMgr.Inst.ItemCtrller.relic_Variator != null)
				{
					num *= PlayerMgr.Inst.ItemCtrller.relic_Variator.MoveRate;
				}
				if (PlayerMgr.Inst.ItemCtrller.curse_Shackle != null)
				{
					num *= PlayerMgr.Inst.ItemCtrller.curse_Shackle.MoveRatio;
				}
				num *= PlayerMgr.Inst.PlayerMoveSpeedRatioFromWandAbility();
			}
			if (affect_IsReverseMove)
			{
				num = 0f - num;
			}
			if (affect_IsMucusDecelerate)
			{
				num *= affect_MucusMoveSpeedRatio;
			}
			return num;
		}
	}

	public event Action OnEnterDelayDeathEvent;

	public event Action OnEnterFuseStageEvent;

	public override void OnEnable()
	{
		base.OnEnable();
		EveryInitialize();
	}

	public void SingleInitial()
	{
		if (isSingleInitialized)
		{
			return;
		}
		isSingleInitialized = true;
		unitID = int.Parse(base.gameObject.name.Substring(0, base.gameObject.name.Length - 7));
		unitCfg = UnitConfig.map[unitID];
		if (unitCfg.unitType == UnitType.Player)
		{
			PlayerCtrller = GetComponent<PlayerController>();
			SAnima = PlayerCtrller.layerC_PlayerRTRoot.GetComponentInChildren<SkeletonAnimation>(includeInactive: true);
			PlayerCtrller.layerC_PlayerRTRoot.transform.SetParent(null);
			PlayerCtrller.layerC_PlayerRTRoot.transform.localScale = Vector3.one;
		}
		else
		{
			UnitBas = GetComponent<UnitBase>();
			SAnima = GetComponentInChildren<SkeletonAnimation>(includeInactive: true);
		}
		SR_Models = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		MR_Models = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		MR_Models_LocalScale = new Vector3[MR_Models.Length];
		MR_Models_NeedRebound = new bool[MR_Models.Length];
		for (int i = 0; i < MR_Models.Length; i++)
		{
			MR_Models_NeedRebound[i] = MR_Models[i].material.shader == GameConstManaged.unitReboundShader;
		}
		CheckMRRebound();
		CC_Self = GetComponent<CapsuleCollider>();
		BC_Self = GetComponent<BoxCollider>();
		Anima = GetComponentInChildren<Animator>();
		Rigid = GetComponent<Rigidbody>();
		Tsf_BeHit = base.transform.Find("Layer/BeHit");
		if (Tsf_BeHit == null)
		{
			Debug.LogWarning(base.name + " no tsf_BeHit");
		}
		List<Transform> list = new List<Transform>();
		for (int j = 0; j < MR_Models.Length; j++)
		{
			if (MR_Models[j].transform.parent != null && MR_Models[j].transform.parent.name == "Flip")
			{
				list.Add(MR_Models[j].transform.parent);
			}
		}
		MR_FlipRoots = list.ToArray();
		if (Rigid != null)
		{
			original_RigidIsKinematic = Rigid.isKinematic;
		}
		if (unitCfg.inGallery)
		{
			switch (unitCfg.unitType)
			{
			case UnitType.Monster:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Monster, unitCfg.id);
				break;
			case UnitType.Elite:
			case UnitType.Boss:
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Boss, unitCfg.id);
				break;
			case UnitType.Player:
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
			case UnitType.Brittleness:
				break;
			}
		}
	}

	private void EveryInitialize()
	{
		SingleInitial();
		float num = 1f;
		if (!GameMgr.InEndlessMode)
		{
			switch (DataMgr.selectedWorldData.selectedDifficulty)
			{
			case DifficultyType.Nightmare1:
				if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 1)
				{
					num = 1.06f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 2)
				{
					num = 1.12f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 3)
				{
					num = 1.18f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 4)
				{
					num = 1.24f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 5)
				{
					num = 1.3f;
				}
				break;
			case DifficultyType.Nightmare2:
				if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 1)
				{
					num = 1.08f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 2)
				{
					num = 1.21f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 3)
				{
					num = 1.34f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 4)
				{
					num = 1.47f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 5)
				{
					num = 1.6f;
				}
				break;
			case DifficultyType.Nightmare3:
				if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 1)
				{
					num = 1.2f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 2)
				{
					num = 1.4f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 3)
				{
					num = 1.6f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 4)
				{
					num = 1.8f;
				}
				else if ((DataMgr.selectedWorldData.battleData9.currentStage + 1) / 2 == 5)
				{
					num = 2f;
				}
				break;
			default:
				Debug.LogError(DataMgr.selectedWorldData.selectedDifficulty);
				break;
			case DifficultyType.Easy:
			case DifficultyType.Normal:
			case DifficultyType.Hard:
				break;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_InvalidCurse != null)
		{
			num += (float)PlayerMgr.Inst.ItemCtrller.curseCfg_InvalidCurse.int1.result / 100f;
		}
		unitCfg = UnitConfig.map[unitID];
		unitCfg.maxHP *= num;
		unitCfg.currentHP = unitCfg.maxHP;
		if (Rigid != null)
		{
			Rigid.isKinematic = original_RigidIsKinematic;
			if (!Rigid.isKinematic)
			{
				Rigid.linearVelocity = Vector3.zero;
			}
		}
		AlreadyDead = false;
		IsVelocityDeclice = true;
		Color_NormalBody = Color.white;
		SetFlip(isFlip: false);
		CanTouch = true;
		flyCounter = 0;
		invincibleCounter = 0;
		immuneKnockbackCounter = 0;
		affect_SpikesTimer = 0f;
		immnueMucusCounter = 0;
		if (ec_Deceleration != null)
		{
			ec_Deceleration.gameObject.SetActive(value: false);
		}
		affect_IsMucusHit = false;
		affect_IsMucusDecelerate = false;
		affect_MucusHitTimer = 0f;
		affect_MucusMoveSpeedRatio = 1f;
		affect_MucusSpellSpeedRatio = 1f;
		immnueVenomCounter = 0;
		affect_VenomDurationTimer = 0f;
		affect_VenomInjureTimer = 0f;
		affect_VenomCurrentStack = 0f;
		affect_burnDurationTimer = 0f;
		affect_burnAttackIntervalTimer = 0f;
		affect_burnHPRatio = 0f;
		affect_burnHPRatioPerHit = 0f;
		Affect_InAbyss = false;
		voidExplosionData = null;
		voidEffectTimer = 0f;
		if (ec_VoidEffect != null)
		{
			ec_VoidEffect.gameObject.SetActive(value: false);
		}
		FronzenState = Affect_FrozenState.Normal;
		affect_FrozenTime = 0f;
		affect_FrozenTimer = 0f;
		if (go_FrozenEF != null)
		{
			go_FrozenEF.gameObject.SetActive(value: false);
		}
		if (Anima != null)
		{
			Anima.speed = 1f;
		}
		if (SAnima != null)
		{
			SAnima.timeScale = 1f;
		}
		affect_ReverseMoveTimer = 0f;
		if (ec_ReverseMove != null)
		{
			ec_ReverseMove.gameObject.SetActive(value: false);
		}
		immuneGroundAffectTimer = 0;
		stuckCheckIntervalTimer = 0f;
		isStuck = false;
		playerInvincibleFrameTimer = 0f;
		playerInvincibleFrameTwinkleTimer = 0f;
		playerInvincibleFrameTwinkleAlpha = 1f;
		isBeHitColor = false;
		beHitColorTimer = 0f;
		isBeHit = false;
		beHitOut = true;
		beHitTimer = 0;
		if (Tsf_BeHit != null)
		{
			Tsf_BeHit.localPosition = Vector3.zero;
		}
		DamageReciveIncresePercent = 0f;
		damageReciveIncresePercentTimer = 0f;
		curse_MonsterRecoveryTimer = 0f;
		unitBodyCenterPoint = Vector3.zero;
		CanBeTarget = true;
		isUnitDead = false;
		this.OnEnterDelayDeathEvent = null;
		this.OnEnterFuseStageEvent = null;
		if (ScriptableObjMgr.Inst.testCtrller.MasterSwitch && ScriptableObjMgr.Inst.testCtrller.battleBossHpDivide > 1f && !unitCfg.IsSameCamp(UnitType.Player))
		{
			unitCfg.maxHP /= ScriptableObjMgr.Inst.testCtrller.battleBossHpDivide;
			unitCfg.maxHP = Mathf.Max(unitCfg.maxHP, 1f);
			unitCfg.currentHP = unitCfg.maxHP;
		}
		if (GameMgr.IsMobile_Static)
		{
			unitCfg.moveSpeed *= 0.8f;
		}
		myEntity = Entity.Null;
		if (unitCfg.isHybirdUnit)
		{
			UnitDotsSyncSystem.CreateEntity(this);
		}
		switch (unitCfg.unitType)
		{
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			UnitBas.EveryInitial();
			break;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			UnitBas.EveryInitial();
			break;
		default:
			Debug.LogError(unitCfg.unitType);
			break;
		case UnitType.Player:
			break;
		}
		ChangeColor(Color.white);
	}

	private void Update()
	{
		if (unitCfg.id != 800001)
		{
			entityIndex = myEntity.Index;
			entityValid = UnitBas.EntityIsValid(myEntity);
		}
		else
		{
			entityIndex = PlayerMgr.Inst.PlayerEtt.Index;
			entityValid = true;
		}
		if (entityValid)
		{
			Affect();
			TimerProcess();
			UpdateDamageReciveIncreseEffectPosition();
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		CheckMRRebound();
	}

	private void CheckVoidColorEffect(TakeDamageInfo targetInfo = null)
	{
		if (voidExplosionData == null || unitCfg.unitType == UnitType.Player)
		{
			return;
		}
		float num = voidExplosionData.InstantKillRatio;
		if (unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Boss)
		{
			num /= 2f;
		}
		if (unitCfg.currentHP / unitCfg.maxHP < num && !IsInvincible)
		{
			unitCfg.currentHP = 0f;
			TakeDamageInfo takeDamageInfo = targetInfo ?? new TakeDamageInfo();
			if (!takeDamageInfo.stopAnnouncedDeath)
			{
				takeDamageInfo.isTargetDead = true;
				AnnouncedDeath(takeDamageInfo);
			}
		}
	}

	private void Affect()
	{
		if (affect_IsMucusHit)
		{
			affect_IsMucusDecelerate = true;
			affect_MucusHitTimer -= Time.deltaTime;
			if (affect_MucusHitTimer <= 0f)
			{
				ClearMucusState();
				UpdateBodyColor();
			}
		}
		if (!IsFly && !IsImmuneGroundAffect)
		{
			if (CC_Self != null)
			{
				GeneralTool.HaveCollider(base.transform.position, base.transform.localScale.x * CC_Self.radius, "Mucus", LayerMgr.InvisibleMask);
			}
			else if (BC_Self != null)
			{
				GeneralTool.HaveColliderBox(base.transform.position, base.transform.localScale.x * BC_Self.size / 2f, "Mucus", LayerMgr.InvisibleMask);
			}
		}
		if (!IsFly && !IsImmuneMucus && !IsImmuneGroundAffect && affect_IsMucusHit)
		{
			affect_IsMucusDecelerate = true;
		}
		bool flag = false;
		if (!IsFly && !IsImmuneVenom && !IsImmuneGroundAffect && !(affect_VenomDurationTimer > 0f) && (bool)GeneralTool.HaveColliderInCurrentBuffer("Venom"))
		{
			flag = true;
			SetVenom(2f, 2f);
		}
		if (affect_VenomDurationTimer > 0f)
		{
			affect_VenomDurationTimer -= Time.deltaTime;
			if (affect_VenomDurationTimer <= 0f)
			{
				ClearVenomState();
				UpdateBodyColor();
			}
		}
		affect_VenomInjureTimer += Time.deltaTime;
		if (affect_VenomDurationTimer > 0f && affect_VenomInjureTimer >= 1f)
		{
			affect_VenomInjureTimer = 0f;
			if (!flag && !IsFly && !IsImmuneVenom && (bool)GeneralTool.HaveColliderInCurrentBuffer("Venom"))
			{
				SetVenom(2f, 2f);
			}
			TakeDamage(affect_VenomCurrentStack, AttackerType.Venom, new TakeDamageInfo
			{
				beHitColor = false,
				considerUmbrella = true
			});
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Poisoning", tsf_Layer.position, 1f);
		}
		if (!IsFly && !unitCfg.immuneSpike && !IsImmuneGroundAffect)
		{
			affect_SpikesTimer += Time.deltaTime;
			if (affect_SpikesTimer >= 0.33f && GeneralTool.HaveColliderInCurrentBuffer("GroundNeedle") != null)
			{
				affect_SpikesTimer = 0f;
				TakeDamage(3f, AttackerType.NothingSpecial, new TakeDamageInfo
				{
					isTrapDamage = true
				});
				if (!GameMgr.IsHarmony_Static)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", base.transform.position, 1f);
				}
			}
		}
		if (voidEffectTimer > 0f || (voidExplosionData != null && voidExplosionData.ConstVoidEffect))
		{
			voidEffectTimer -= Time.deltaTime;
			if (!voidExplosionData.ConstVoidEffect)
			{
				ChangeColor(GameConst.color_BodyVoid);
			}
			if (voidEffectTimer <= 0f && !voidExplosionData.ConstVoidEffect)
			{
				ClearVoidState();
				UpdateBodyColor();
			}
			CheckVoidColorEffect();
		}
		if (affect_burnDurationTimer > 0f)
		{
			affect_burnDurationTimer -= Time.deltaTime;
			if (affect_burnDurationTimer <= 0f)
			{
				ClearBurnState();
				UpdateBodyColor();
			}
		}
		if (affect_burnDurationTimer > 0f)
		{
			affect_burnAttackIntervalTimer += Time.deltaTime;
			if (affect_burnAttackIntervalTimer >= 0.33f)
			{
				affect_burnAttackIntervalTimer = 0f;
				TakeDamage(Mathf.CeilToInt(unitCfg.maxHP * affect_burnHPRatioPerHit * 0.33f), AttackerType.Burn, new TakeDamageInfo
				{
					beHitColor = false
				});
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Burn", tsf_Layer.position, 1f);
			}
		}
		if (Affect_InAbyss)
		{
			if (base.transform.position != affect_AbyssPoint)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, affect_AbyssPoint, 4f * Time.deltaTime);
			}
			float num = base.transform.localScale.x - Time.deltaTime * fallAbyssSpeed;
			if (num < 0f)
			{
				if (unitCfg.unitType == UnitType.Player)
				{
					Affect_InAbyss = false;
					UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(myEntity);
					componentData.Affect_InAbyss = false;
					UnitDotsSyncSystem.SetComponentData(componentData, myEntity);
					base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position);
					LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(myEntity);
					PlayerMgr.Inst.ChangeBodySize();
					componentData2.Position = base.transform.position;
					componentData2.Scale = base.transform.localScale.x;
					UnitDotsSyncSystem.SetComponentData(componentData2, myEntity);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_AbyssTeleport", base.transform.position, 3f);
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = 10f;
					info.ignoreRelicDodge = true;
					info.isTrapDamage = true;
					UnitDotsSyncSystem.AddTakeDamageRequest(myEntity, info);
					PlayerCtrller.InAbyss = false;
					PlayerCtrller.StartMotion();
				}
			}
			else
			{
				base.transform.localScale = Vector3.one * num;
			}
		}
		switch (FronzenState)
		{
		case Affect_FrozenState.Frozening:
			affect_FrozenTimer += Time.deltaTime;
			if (affect_FrozenTimer >= affect_FrozenTime)
			{
				ClearFrozenState();
				FronzenState = Affect_FrozenState.FrozenImmune;
				UpdateBodyColor();
			}
			break;
		case Affect_FrozenState.FrozenImmune:
			affect_FrozenTimer += Time.deltaTime;
			if (affect_FrozenTimer >= 2f)
			{
				affect_FrozenTimer = 0f;
				FronzenState = Affect_FrozenState.Normal;
			}
			break;
		default:
			Debug.LogError(FronzenState);
			break;
		case Affect_FrozenState.Normal:
			break;
		}
		if (affect_IsReverseMove)
		{
			affect_ReverseMoveTimer -= Time.deltaTime;
			if (affect_ReverseMoveTimer <= 0f)
			{
				affect_ReverseMoveTimer = 0f;
			}
		}
	}

	private void BeHitColor()
	{
		if (!isBeHitColor)
		{
			return;
		}
		beHitColorTimer -= Time.deltaTime;
		if (beHitColorTimer < 0f)
		{
			beHitColorTimer = 0f;
			isBeHitColor = false;
			Color color = Color.white;
			if (affect_IsMucusHit)
			{
				color = GameConst.color_BodyMucus;
			}
			if (affect_burnDurationTimer > 0f)
			{
				color = GameConst.color_BodyBurn;
			}
			if (affect_VenomDurationTimer > 0f)
			{
				color = GameConst.color_BodyVenom;
			}
			if (FronzenState == Affect_FrozenState.Frozening)
			{
				color = GameConst.color_BodyFrozen;
			}
			ChangeColor(color);
		}
	}

	private void VelocityDecline()
	{
		if (!(Rigid == null) && IsVelocityDeclice && Rigid.linearVelocity != Vector3.zero)
		{
			Rigid.linearVelocity = Vector3.Lerp(Rigid.linearVelocity, Vector3.zero, 5f * Time.deltaTime);
		}
	}

	private void BeHitShake()
	{
		if (!isBeHit)
		{
			return;
		}
		if (beHitOut)
		{
			float num = 6f * unitCfg.beHitRatio * Time.deltaTime;
			if ((Tsf_BeHit.localPosition + beHitDir * num).sqrMagnitude >= (beHitDir * 0.2f * unitCfg.beHitRatio).sqrMagnitude)
			{
				Tsf_BeHit.localPosition = beHitDir * 0.2f * unitCfg.beHitRatio;
				beHitOut = false;
			}
			else
			{
				Tsf_BeHit.localPosition += beHitDir * num;
			}
			return;
		}
		Tsf_BeHit.localPosition = Vector3.MoveTowards(Tsf_BeHit.localPosition, Vector3.zero, 6f * Time.deltaTime * unitCfg.beHitRatio);
		if (Tsf_BeHit.localPosition == Vector3.zero)
		{
			beHitTimer++;
			if (beHitTimer >= 1)
			{
				isBeHit = false;
			}
			else
			{
				beHitOut = true;
			}
		}
	}

	private void CheckStuck()
	{
		if (!unitCfg.isCheckStuck)
		{
			return;
		}
		stuckCheckIntervalTimer += Time.deltaTime;
		if (!(stuckCheckIntervalTimer >= 2.5f) && (unitCfg.id != 800001 || !(stuckCheckIntervalTimer >= 0.5f)))
		{
			return;
		}
		stuckCheckIntervalTimer = 0f;
		if (NavMesh.SamplePosition(Tool2D.IgnoreZPoint(base.transform.position, 4.35f), out var _, 0.5f, 8))
		{
			isStuck = false;
		}
		else if (isStuck)
		{
			isStuck = false;
			NavMeshPath navMeshPath = Tool2D.GetNavMeshPath(LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Down), base.transform.position);
			if (navMeshPath.corners.Length != 0)
			{
				if (unitCfg.id == 800001)
				{
					PlayerMgr.Inst.SetPlayerPoint(Tool2D.IgnoreZPoint(navMeshPath.corners[navMeshPath.corners.Length - 1]));
					return;
				}
				base.transform.position = Tool2D.IgnoreZPoint(navMeshPath.corners[navMeshPath.corners.Length - 1]);
				LocalTransform componentData = UnitBas.GetComponentData<LocalTransform>();
				componentData.Position = base.transform.position;
				UnitBas.SetComponentData(componentData);
			}
		}
		else
		{
			isStuck = true;
		}
	}

	private void TimerProcess()
	{
		if (damageReciveIncresePercentTimer > 0f)
		{
			damageReciveIncresePercentTimer -= Time.deltaTime;
			if (damageReciveIncresePercentTimer <= 0f)
			{
				DamageReciveIncresePercent = 0f;
				damageReciveIncresePercentTimer = 0f;
				RecycleDamageReciveIncressEffect();
			}
		}
		if (playerInvincibleFrameTimer > 0f)
		{
			playerInvincibleFrameTimer -= Time.deltaTime;
			if (playerInvincibleFrameTimer < 0f)
			{
				playerInvincibleFrameTwinkleTimer = 0f;
				playerInvincibleFrameTwinkleAlpha = 1f;
				ChangeAlpha(playerInvincibleFrameTwinkleAlpha);
			}
			else
			{
				playerInvincibleFrameTwinkleTimer += Time.deltaTime;
				if (playerInvincibleFrameTwinkleTimer >= 0.05f)
				{
					playerInvincibleFrameTwinkleTimer = 0f;
					playerInvincibleFrameTwinkleAlpha = ((playerInvincibleFrameTwinkleAlpha == 1f) ? 0.3f : 1f);
					ChangeAlpha(playerInvincibleFrameTwinkleAlpha);
				}
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_MonsterRecover != null && unitCfg.IsSameCamp(UnitType.Monster) && unitCfg.id != 301401 && unitCfg.currentHP < unitCfg.maxHP)
		{
			curse_MonsterRecoveryTimer += Time.deltaTime;
			if (curse_MonsterRecoveryTimer >= 1f)
			{
				curse_MonsterRecoveryTimer = 0f;
				HPRecovery(PlayerMgr.Inst.ItemCtrller.curseCfg_MonsterRecover.int1.result);
			}
		}
	}

	public void CheckMRRebound()
	{
	}

	public TakeDamageInfo TakeDamage(SpellBase spellBase, TakeDamageInfo info = null)
	{
		TakeDamageInfo takeDamageInfo = info;
		if (takeDamageInfo == null)
		{
			takeDamageInfo = new TakeDamageInfo();
		}
		takeDamageInfo.teammateTakeDamageRatio = spellBase.TeammateTakDamageRatio;
		takeDamageInfo.attackerPpt = spellBase.ownerPpt;
		if (takeDamageInfo.damage == 0f)
		{
			takeDamageInfo.damage = spellBase.spellCfg.damage;
		}
		takeDamageInfo.spellBase = spellBase;
		takeDamageInfo.criticalChance += spellBase.GetCriticalChance();
		if (takeDamageInfo.knockbackForce == Vector3.zero)
		{
			if (spellBase.CurrentSpeed == 0f)
			{
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(base.transform.position, spellBase.transform.position) * spellBase.spellCfg.knockback;
			}
			else
			{
				takeDamageInfo.knockbackForce = spellBase.Direction * spellBase.spellCfg.knockback;
			}
		}
		FinalTakeDamage(takeDamageInfo);
		return takeDamageInfo;
	}

	public TakeDamageInfo TakeDamage(float damage, AttackerType attackerType, TakeDamageInfo info = null)
	{
		TakeDamageInfo takeDamageInfo = info;
		if (takeDamageInfo == null)
		{
			takeDamageInfo = new TakeDamageInfo();
		}
		takeDamageInfo.damage = damage;
		takeDamageInfo.attackerType = attackerType;
		FinalTakeDamage(takeDamageInfo);
		return takeDamageInfo;
	}

	public TakeDamageInfo TakeDamage(float damage, UnitProperty attackerPpt, TakeDamageInfo info = null)
	{
		TakeDamageInfo takeDamageInfo = info;
		if (takeDamageInfo == null)
		{
			takeDamageInfo = new TakeDamageInfo();
		}
		takeDamageInfo.damage = damage;
		takeDamageInfo.attackerPpt = attackerPpt;
		FinalTakeDamage(takeDamageInfo);
		return takeDamageInfo;
	}

	private void FinalTakeDamage(TakeDamageInfo info)
	{
		info.beHitPpt = this;
		if (UnitBas != null)
		{
			UnitBas.BeforeTakeDamage(info);
		}
		if (IsInvincible)
		{
			info.immuneDamage = true;
		}
		if (IsImmuneKnockback)
		{
			info.knockbackForce = Vector3.zero;
		}
		float damage = info.damage;
		switch (unitCfg.unitType)
		{
		case UnitType.Player:
		{
			if (info.considerPlayerInInvincibleFrame && IsPlayerInInvincibleFrame)
			{
				info.knockbackForce = Vector3.zero;
				info.immuneDamage = true;
			}
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_TargetedTrap != null && info.isTrapDamage)
			{
				info.damage = info.damage * (float)(PlayerMgr.Inst.ItemCtrller.curseCfg_TargetedTrap.int1.result + 100) / 100f;
			}
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_Dodge != null && info.considerRelicDodge && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_Dodge.int1.result / 100f)
			{
				info.immuneDamage = true;
				info.knockbackForce = Vector3.zero;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002001.GetText(), UITextFloatType.Normal, base.transform.position);
			}
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceDamage != null && info.considerRelicOrCurseDamageRatioChange)
			{
				info.damage = Mathf.CeilToInt(info.damage * (1f - (float)PlayerMgr.Inst.ItemCtrller.relicCfg_ReduceDamage.int1.result / 100f));
			}
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_PowerfulMan != null && info.considerRelicOrCurseDamageRatioChange)
			{
				info.damage = Mathf.CeilToInt(info.damage * (1f + (float)PlayerMgr.Inst.ItemCtrller.relicCfg_PowerfulMan.int2.result / 100f));
			}
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_Vulnerability != null && info.considerRelicOrCurseDamageRatioChange)
			{
				info.damage = Mathf.CeilToInt(info.damage * (1f + (float)PlayerMgr.Inst.ItemCtrller.curseCfg_Vulnerability.int1.result / 100f));
			}
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_MoreMoneyMoreInjured != null && info.considerRelicOrCurseDamageRatioChange)
			{
				float num = (float)(PlayerMgr.Inst.CoinCount / PlayerMgr.Inst.ItemCtrller.curseCfg_MoreMoneyMoreInjured.int1.result * PlayerMgr.Inst.ItemCtrller.curseCfg_MoreMoneyMoreInjured.int2.result) / 100f;
				if (num > PlayerMgr.Inst.ItemCtrller.curseCfg_MoreMoneyMoreInjured.float1.result / 100f)
				{
					num = PlayerMgr.Inst.ItemCtrller.curseCfg_MoreMoneyMoreInjured.float1.result / 100f;
				}
				info.damage = Mathf.CeilToInt(info.damage * (1f + num));
			}
			if (info.playerTakeDamageRatio != 1f)
			{
				info.damage = Mathf.CeilToInt(info.damage * info.playerTakeDamageRatio);
			}
			float num2 = 1f;
			if (info.isUndifferDamage)
			{
				num2 = ((PlayerMgr.Inst.ItemCtrller.relicCfg_MaxUndifferDamage == null) ? 0.3333f : Mathf.Min(num2, PlayerMgr.Inst.ItemCtrller.relicCfg_MaxUndifferDamage.float1.result / 100f));
			}
			if (info.spellBase != null)
			{
				num2 = Mathf.Min(num2, info.spellBase.undifferDamageRatio);
			}
			if (num2 != 1f)
			{
				info.damage = Mathf.CeilToInt(info.damage * num2);
			}
			break;
		}
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_TargetedTrap != null && info.isTrapDamage)
			{
				info.damage = Mathf.CeilToInt(info.damage * (float)(PlayerMgr.Inst.ItemCtrller.curseCfg_TargetedTrap.int1.result + 100) / 100f);
			}
			if (info.teammateTakeDamageRatio != 1f)
			{
				info.damage = Mathf.CeilToInt(info.damage * info.teammateTakeDamageRatio);
			}
			break;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_EnemyReduceDamage != null && !info.isPercentageDamage)
			{
				info.damage = Mathf.CeilToInt(info.damage * (1f - (float)PlayerMgr.Inst.ItemCtrller.curseCfg_EnemyReduceDamage.int1.result / 100f));
			}
			break;
		default:
			Debug.LogError(unitCfg.unitType);
			break;
		}
		if (DamageReciveIncresePercent > 0f && !info.isPercentageDamage)
		{
			info.damage *= DamageReciveIncresePercent;
		}
		if (info.immuneDamage || info.damage <= 0f)
		{
			TakeKnockback(info.knockbackForce);
			return;
		}
		if (info.criticalChance > 0f && UnityEngine.Random.value <= info.criticalChance)
		{
			info.isCriticalDamage = true;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_AddCriticalDamage != null && info.attackerPpt != null && info.attackerPpt.unitCfg.IsSameCamp(UnitType.Player))
			{
				info.damage = Mathf.CeilToInt(info.damage / 100f * (float)PlayerMgr.Inst.ItemCtrller.relicCfg_AddCriticalDamage.int1.result);
			}
			else
			{
				info.damage *= 2f;
			}
		}
		if (base.gameObject.tag == "Monster" && ((info.spellBase != null && info.spellBase.wandChargeData != null) || (info.wandChargeData != null && info.wandChargeData.chargeTargetWand.WandCfg != null && info.wandChargeData.chargeTargetWand.WandCfg.postSlots.Length != 0)))
		{
			WandConfig targetShooterWandConfigDataFromTakeDamageInfo = WandPostSlotTrigger.GetTargetShooterWandConfigDataFromTakeDamageInfo(info);
			if (targetShooterWandConfigDataFromTakeDamageInfo != null)
			{
				if (targetShooterWandConfigDataFromTakeDamageInfo.PostslotSpellHitChargeRatio > 0f)
				{
					WandPostSlotTrigger.PostSlotSpellHitTriggerCheck(targetShooterWandConfigDataFromTakeDamageInfo);
				}
				if (info.damage >= 45f && targetShooterWandConfigDataFromTakeDamageInfo.PostslotHighDamageChargeRatio > 0f)
				{
					WandPostSlotTrigger.PostSlotHighDamageTriggerCheck(targetShooterWandConfigDataFromTakeDamageInfo);
				}
				if (info.isCriticalDamage && targetShooterWandConfigDataFromTakeDamageInfo.PostslotCriticalHitChargeRatio > 0f)
				{
					WandPostSlotTrigger.PostSlotSpellCriticalHitTriggerCheck(targetShooterWandConfigDataFromTakeDamageInfo);
				}
			}
		}
		else if (base.gameObject.tag == "Player" && info.postSlotSpellTakeDamageTrigger)
		{
			WandPostSlotTrigger.PostSlotSpellTakeDamageTriggerCheck();
		}
		bool flag = false;
		if (info.considerUmbrella && (bool)UmbrellaCtrl && UmbrellaCtrl.CanUnderDamage)
		{
			info.damage = UmbrellaCtrl.UnderDamage(info.damage);
			if (info.damage <= 0f)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			TakeKnockback(info.knockbackForce);
			if (unitCfg.shieldTemp > 0f)
			{
				float shieldTemp = unitCfg.shieldTemp;
				if (info.damage > unitCfg.shieldTemp)
				{
					if (unitCfg.unitType == UnitType.Player)
					{
						PlayerMgr.Inst.ChangeShieldTemp(0f - unitCfg.shieldTemp, TextFloatQueueType.None);
					}
					else
					{
						unitCfg.shieldTemp = 0f;
					}
					info.damage -= shieldTemp;
				}
				else
				{
					if (unitCfg.unitType == UnitType.Player)
					{
						PlayerMgr.Inst.ChangeShieldTemp(0f - info.damage, TextFloatQueueType.DirectFloat);
					}
					else
					{
						unitCfg.shieldTemp -= info.damage;
					}
					info.damage = 0f;
				}
			}
			if (info.damage > 0f && unitCfg.shield > 0f)
			{
				float shield = unitCfg.shield;
				if (info.damage > unitCfg.shield)
				{
					if (unitCfg.unitType == UnitType.Player)
					{
						PlayerMgr.Inst.ChangeShield(0f - unitCfg.shield, TextFloatQueueType.None);
					}
					else
					{
						unitCfg.shield = 0f;
					}
					info.damage -= shield;
				}
				else
				{
					if (unitCfg.unitType == UnitType.Player)
					{
						PlayerMgr.Inst.ChangeShield(0f - info.damage, TextFloatQueueType.DirectFloat);
					}
					else
					{
						unitCfg.shield -= info.damage;
					}
					info.damage = 0f;
				}
			}
			if (info.damage > 0f)
			{
				unitCfg.currentHP -= info.damage;
				if ((DataMgr.settingData.textFloat || unitCfg.unitType == UnitType.Player) && info.isFloatText)
				{
					UITextFloat component = ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>();
					if (info.isCriticalDamage)
					{
						component.Initialize(info.damage.ToStringDamage(), UITextFloatType.Critical, base.transform.position);
					}
					else if (info.attackerType == AttackerType.Venom)
					{
						component.Initialize(damage.ToStringDamage(), UITextFloatType.Poison, base.transform.position);
					}
					else if (info.attackerType == AttackerType.Burn)
					{
						component.Initialize(damage.ToStringDamage(), UITextFloatType.Burn, base.transform.position);
					}
					else if (unitCfg.unitType == UnitType.Player || unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack)
					{
						component.Initialize(info.damage.ToStringDamage(), UITextFloatType.PlayerTakeDamage, base.transform.position);
					}
					else
					{
						component.Initialize(info.damage.ToStringDamage(), UITextFloatType.Damage, base.transform.position);
					}
				}
			}
		}
		if (unitCfg.currentHP <= 0f && unitCfg.shield == 0f)
		{
			if ((unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack) && UnitBas != null && UnitBas.SummonerSpellBase != null && UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime > 0f)
			{
				StartCoroutine(SetTempDeathImmute(new TeammateAnnounceDeathInfo
				{
					delayDeathTime = UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime
				}, info));
			}
			else if (unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.ItemCtrller.relic_Resurgence != null)
			{
				PlayerMgr.Inst.ItemCtrller.relic_Resurgence.TriggerResurgence();
			}
			else if (!info.stopAnnouncedDeath)
			{
				info.isTargetDead = true;
				AnnouncedDeath(info);
				if (unitCfg.unitType == UnitType.Player && info.isUndifferDamage && info.attackerPpt != null && info.attackerPpt.unitCfg.IsSameCamp(UnitType.Player))
				{
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.KillSelf);
				}
			}
		}
		else
		{
			if (!flag)
			{
				unitCfg.beHitSE.Value.PlaySE();
				if (info.beHitColor)
				{
					SetBeHitColor();
				}
				if (info.beHitShake)
				{
					if (info.spellBase != null)
					{
						TakeBeHit(info.spellBase.Direction);
					}
					else
					{
						TakeBeHit(Tool2D.GetDir());
					}
				}
			}
			switch (unitCfg.unitType)
			{
			case UnitType.Player:
				if (flag)
				{
					break;
				}
				UIPlayerDataMgr.Inst.UpdateHP();
				if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
				{
					SEMgr.Inst.injured_Huang.PlaySE();
				}
				else if (PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow == null)
				{
					if (DataMgr.selectedWorldData.playerLook == PlayerLook.Frog)
					{
						SEMgr.Inst.injured_Frog.PlaySE();
					}
					else if (DataMgr.selectedWorldData.playerLook == PlayerLook.Horse)
					{
						SEMgr.Inst.injured_Horse.PlaySE();
					}
					else
					{
						SEMgr.Inst.injured_Player.PlaySE();
					}
				}
				else
				{
					SEMgr.Inst.injured_Player.PlaySE();
				}
				if (!GameMgr.IsHarmony_Static)
				{
					UIPlayerDataMgr.Inst.UIPlayerInjuredPlay();
				}
				if (info.considerPlayerInInvincibleFrame)
				{
					PlayerIntoInvisibleFrame();
				}
				break;
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
				UnitBas.AfterTakeDamage(info);
				break;
			case UnitType.Monster:
			case UnitType.Elite:
			case UnitType.Boss:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				if (PlayerMgr.Inst.ItemCtrller.relicCfg_Seckill != null && info.attackerPpt != null && info.attackerPpt.unitCfg.IsSameCamp(UnitType.Player) && UnityEngine.Random.value <= PlayerMgr.Inst.ItemCtrller.relicCfg_Seckill.float1.result / 100f)
				{
					if (unitCfg.unitType == UnitType.Monster)
					{
						TakeDamage(9999f, info.attackerPpt);
						ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_Seckill", Tool2D.IgnoreZPoint(base.transform), 2f);
					}
					else if (unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Boss)
					{
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002002.GetText(), UITextFloatType.Normal, base.transform.position);
						ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_Seckill", Tool2D.IgnoreZPoint(base.transform), 2f);
					}
				}
				UnitBas.AfterTakeDamage(info);
				break;
			default:
				Debug.LogError(unitCfg.unitType);
				break;
			}
		}
		CheckVoidColorEffect(info);
	}

	public void SetMucus(float time, float moveSpeedRatio, float spellSpeedRatio, bool changeColor = true)
	{
		if (!IsImmuneMucus && !IsInvincible)
		{
			float num = time;
			if (unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Boss)
			{
				num *= unitCfg.frozenTimeRatio;
			}
			if (affect_MucusHitTimer < num)
			{
				affect_MucusHitTimer = num;
			}
			affect_MucusMoveSpeedRatio = Mathf.Clamp(moveSpeedRatio, 0f, 1f);
			affect_MucusSpellSpeedRatio = Mathf.Clamp(spellSpeedRatio, 0f, 1f);
		}
	}

	public void SetVenom(float duration, float applyCount)
	{
		if (!IsImmuneVenom && !IsInvincible)
		{
			if (affect_VenomDurationTimer == 0f)
			{
				ChangeColor(GameConst.color_BodyVenom);
				affect_VenomInjureTimer = 0.5f;
			}
			if (affect_VenomDurationTimer < duration + 0.5f)
			{
				affect_VenomDurationTimer = duration + 0.5f;
			}
			affect_VenomCurrentStack += applyCount;
			if (!DataMgr.selectedWorldData.FindSet8 && unitCfg.id != 10501 && unitCfg.IsSameCamp(UnitType.Monster) && affect_VenomCurrentStack >= (float)SetConfig.dic[8].unlockInt1)
			{
				DataMgr.selectedWorldData.SetFindSet8();
			}
		}
	}

	public void GetVenomInfo(out float duration, out int applyCount)
	{
		duration = affect_VenomDurationTimer;
		applyCount = (int)affect_VenomCurrentStack;
	}

	public void ClearVenom()
	{
		affect_VenomDurationTimer = 0.0001f;
	}

	public void SetBurn(float duration, float burnRatio)
	{
		affect_burnDurationTimer = Mathf.Max(affect_burnDurationTimer, duration);
		float num = burnRatio / 100f;
		if (unitCfg.unitType == UnitType.Boss || unitCfg.unitType == UnitType.Elite || unitCfg.unitType == UnitType.Player)
		{
			num *= 0.1f;
		}
		if (unitCfg.id == 10501)
		{
			num *= 1E-05f;
		}
		affect_burnHPRatio = Mathf.Max(affect_burnHPRatio, num);
		affect_burnHPRatioPerHit = Mathf.Max(affect_burnHPRatioPerHit, num);
	}

	public void GetBurnInfo(out float duration, out float burnRatio)
	{
		duration = affect_burnDurationTimer;
		burnRatio = affect_burnHPRatio * 100f;
		if (unitCfg.unitType == UnitType.Boss || unitCfg.unitType == UnitType.Elite)
		{
			burnRatio /= 0.1f;
		}
		if (unitCfg.unitType == UnitType.Player)
		{
			burnRatio /= 0.05f;
		}
		if (unitCfg.id == 10501)
		{
			burnRatio /= 1E-05f;
		}
	}

	public void SetVoid(Spell3129VoidExplosion.VoidExplosionData voidData)
	{
		if (unitCfg.unitType != UnitType.NotAttack && !IsImmuneVoid)
		{
			voidEffectTimer = 3f;
			if (voidExplosionData == null)
			{
				voidExplosionData = voidData;
			}
			else
			{
				voidExplosionData.ExplosionRange = Mathf.Max(voidExplosionData.ExplosionRange, voidData.ExplosionRange);
				voidExplosionData.HpToDmgRatio = Mathf.Max(voidExplosionData.HpToDmgRatio, voidData.HpToDmgRatio);
				voidExplosionData.InstantKillRatio = Mathf.Max(voidExplosionData.InstantKillRatio, voidData.InstantKillRatio);
			}
			voidExplosionData.ConstVoidEffect = voidExplosionData.ConstVoidEffect || voidData.ConstVoidEffect;
			_ = voidExplosionData.ConstVoidEffect;
		}
	}

	public void SetFrozen(float time)
	{
		if (!(unitCfg.frozenTimeRatio <= 0f) && !IsInvincible && FronzenState == Affect_FrozenState.Normal)
		{
			FronzenState = Affect_FrozenState.Frozening;
			ChangeColor(GameConst.color_BodyFrozen);
			switch (unitCfg.unitType)
			{
			case UnitType.Player:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				PlayerCtrller.SetFrozen();
				break;
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
			case UnitType.Monster:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				UnitBas.SetFrozen();
				break;
			case UnitType.Elite:
			case UnitType.Boss:
				affect_FrozenTime = time * unitCfg.frozenTimeRatio;
				UnitBas.SetFrozen();
				break;
			default:
				Debug.LogError(unitCfg.unitType);
				break;
			}
		}
	}

	public void PurifyAbnormalState()
	{
		ClearFrozenState();
		ClearBurnState();
		ClearMucusState();
		ClearVenomState();
		ClearVoidState();
		ResetBodyColor();
	}

	public void ClearFrozenState(bool tempImmume = false)
	{
		FronzenState = Affect_FrozenState.Normal;
		if (tempImmume)
		{
			FronzenState = Affect_FrozenState.FrozenImmune;
		}
		if (unitCfg.unitType == UnitType.Player)
		{
			PlayerCtrller.SetUnfrozen();
		}
		else
		{
			UnitBas.SetUnfrozen();
		}
		affect_FrozenTimer = 0f;
	}

	public void ClearBurnState()
	{
		affect_burnDurationTimer = 0f;
		affect_burnAttackIntervalTimer = 0f;
		affect_burnHPRatio = 0f;
		affect_burnHPRatioPerHit = 0f;
	}

	public void ClearVoidState()
	{
		if ((bool)ec_VoidEffect)
		{
			ec_VoidEffect.gameObject.SetActive(value: false);
		}
		voidExplosionData = null;
		voidEffectTimer = 0f;
	}

	public void ClearMucusState()
	{
		affect_MucusHitTimer = 0f;
		affect_IsMucusHit = false;
		affect_IsMucusDecelerate = false;
		affect_MucusMoveSpeedRatio = 1f;
		affect_MucusSpellSpeedRatio = 1f;
	}

	public void ClearVenomState()
	{
		affect_VenomDurationTimer = 0f;
		affect_VenomCurrentStack = 0f;
	}

	public void ResetBodyColor()
	{
		ChangeColor(Color.white);
	}

	public void UpdateBodyColor()
	{
		Color color = Color.white;
		if (affect_IsMucusHit)
		{
			color = GameConst.color_BodyMucus;
		}
		if (FronzenState == Affect_FrozenState.Frozening)
		{
			color = GameConst.color_BodyFrozen;
		}
		if (affect_VenomDurationTimer > 0f)
		{
			color = GameConst.color_BodyVenom;
		}
		if (affect_burnDurationTimer > 0f)
		{
			color = GameConst.color_BodyBurn;
		}
		if (voidEffectTimer > 0f)
		{
			color = GameConst.color_BodyVoid;
		}
		if (isBeHitColor)
		{
			color = ((!GameMgr.IsHarmony_Static) ? GameConst.color_BeHit : GameConst.color_BeHit_H);
		}
		ChangeColor(color);
	}

	public void TakeKnockback(Vector3 force)
	{
		if (!(Rigid != null))
		{
			return;
		}
		if (unitCfg.unitType == UnitType.Player)
		{
			PlayerCtrller.TakeKnockback(force * Mathf.Max(0f, unitCfg.knockbackRatio));
			return;
		}
		if (!Rigid.isKinematic)
		{
			Rigid.linearVelocity += force * unitCfg.knockbackRatio;
		}
		if (UnitDotsSyncSystem.EntityIsValid(myEntity))
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.entityMgr.GetComponentData<UnitProperty_Dots>(myEntity);
			componentData.TakeKnockback(force * unitCfg.knockbackRatio);
			UnitDotsSyncSystem.entityMgr.SetComponentData(myEntity, componentData);
		}
	}

	public void TakeBeHit(Vector3 backwardDir)
	{
		isBeHit = true;
		beHitOut = true;
		Tsf_BeHit.localPosition = Vector3.zero;
		beHitDir = Tsf_BeHit.InverseTransformDirection(backwardDir);
	}

	public void RemoveSRFromArray(SpriteRenderer srToRemove)
	{
		List<SpriteRenderer> list = new List<SpriteRenderer>();
		for (int i = 0; i < SR_Models.Length; i++)
		{
			if (SR_Models[i] != srToRemove)
			{
				list.Add(SR_Models[i]);
			}
		}
		SR_Models = list.ToArray();
	}

	public void RemoveMRFromArray(MeshRenderer srToRemove)
	{
		List<MeshRenderer> list = new List<MeshRenderer>();
		List<Vector3> list2 = new List<Vector3>();
		List<bool> list3 = new List<bool>();
		for (int i = 0; i < MR_Models.Length; i++)
		{
			if (MR_Models[i] != srToRemove)
			{
				list.Add(MR_Models[i]);
				list2.Add(MR_Models_LocalScale[i]);
				list3.Add(MR_Models_NeedRebound[i]);
			}
		}
		MR_Models = list.ToArray();
		MR_Models_LocalScale = list2.ToArray();
		MR_Models_NeedRebound = list3.ToArray();
	}

	public void ChangeColor(Color color)
	{
		if (BaseColor == color)
		{
			return;
		}
		BaseColor = color;
		for (int i = 0; i < SR_Models.Length; i++)
		{
			UnitType unitType = unitCfg.unitType;
			if (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack || unitCfg.id == 301441)
			{
				color.a = SR_Models[i].color.a;
			}
			SR_Models[i].color = color;
			SR_Models[i].material.SetColor(GameConstManaged.shaderColorIndex, color);
		}
		for (int j = 0; j < MR_Models.Length; j++)
		{
			MR_Models[j].material.SetColor(GameConstManaged.shaderColorIndex, color);
			MR_Models[j].material.SetColor(GameConstManaged.shaderBaseColorIndex, color);
		}
	}

	public void ChangeAlpha(float alpha)
	{
		if (BaseColor.a != alpha)
		{
			Color baseColor = BaseColor;
			baseColor.a = alpha;
			BaseColor = baseColor;
			for (int i = 0; i < SR_Models.Length; i++)
			{
				SR_Models[i].color = BaseColor;
				SR_Models[i].material.SetColor(GameConstManaged.shaderColorIndex, BaseColor);
			}
			for (int j = 0; j < MR_Models.Length; j++)
			{
				MR_Models[j].material.SetColor(GameConstManaged.shaderColorIndex, BaseColor);
				SR_Models[j].material.SetColor(GameConstManaged.shaderBaseColorIndex, BaseColor);
			}
		}
	}

	public void SetBeHitColor()
	{
		if (unitCfg.isBehitColor && unitCfg.unitType != 0)
		{
			isBeHitColor = true;
			beHitColorTimer = 0.1f;
			if (GameMgr.IsHarmony_Static)
			{
				ChangeColor(GameConst.color_BeHit_H);
			}
			else
			{
				ChangeColor(GameConst.color_BeHit);
			}
		}
	}

	public void PlayerIntoInvisibleFrame()
	{
		playerInvincibleFrameTimer = 0.33f;
		playerInvincibleFrameTwinkleAlpha = 0.3f;
		ChangeAlpha(playerInvincibleFrameTwinkleAlpha);
	}

	public void SetFlip(bool isFlip)
	{
		if (isFlip != isFlipX)
		{
			isFlipX = isFlip;
			for (int i = 0; i < SR_Models.Length; i++)
			{
				SR_Models[i].flipX = isFlip;
				SR_Models[i].material.SetFloat(GameConstManaged.shaderFlipXIndex, (!isFlip) ? 1 : (-1));
			}
			for (int j = 0; j < MR_FlipRoots.Length; j++)
			{
				Vector3 localPosition = MR_FlipRoots[j].localPosition;
				localPosition.x = 0f - localPosition.x;
				MR_FlipRoots[j].localPosition = localPosition;
			}
			for (int k = 0; k < MR_Models.Length; k++)
			{
				Vector3 localScale = MR_Models[k].transform.localScale;
				localScale.x = Mathf.Abs(localScale.x) * (float)((!isFlip) ? 1 : (-1));
				MR_Models[k].transform.localScale = localScale;
			}
		}
	}

	public void SetReverseMove(float duration)
	{
		affect_ReverseMoveTimer = duration;
	}

	public void Translate(Vector3 motion)
	{
		if (!Affect_InAbyss)
		{
			base.transform.Translate(motion);
		}
	}

	public void FallinAbyss(Vector3 fallPoint)
	{
		if (Affect_InAbyss)
		{
			Debug.LogError("单位已经在深渊里了，为什么还在掉入深渊");
			return;
		}
		Affect_InAbyss = true;
		affect_AbyssPoint = fallPoint;
		if (unitCfg.unitType == UnitType.Player)
		{
			PlayerCtrller.InAbyss = true;
			PlayerCtrller.StopMotion();
			fallAbyssSpeed = base.transform.localScale.x;
		}
		else
		{
			UnitBas.FallAbyss();
			fallAbyssSpeed = 1f;
		}
		SEMgr.Inst.fallinAbyss.PlaySE();
	}

	public void HPRecovery(float hp, bool textFloat = true)
	{
		if (unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.ItemCtrller.relic_GreedSeed != null && PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer < PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result)
		{
			float num = hp;
			PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer += (int)hp;
			if (PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer >= PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result)
			{
				hp = PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer - PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result;
				PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer = PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.GetName() + 1002008.GetText(), UITextFloatType.Recover, PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.transform.position);
				PlayerMgr.Inst.ChangeHPMax(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int2.result);
				HPRecovery(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int2.result, textFloat: false);
				PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.HideSelf();
			}
			else
			{
				hp = 0f;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.GetName() + "+" + (num - hp), UITextFloatType.Recover, PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.transform.position);
			}
			if (hp > 0f)
			{
				unitCfg.currentHP = Mathf.Min(unitCfg.currentHP + hp, unitCfg.maxHP);
				PlayerCtrller.TextFloatQueueAdd("+" + hp, UITextFloatType.Recover);
			}
		}
		else
		{
			unitCfg.currentHP = Mathf.Min(unitCfg.currentHP + hp, unitCfg.maxHP);
			if ((DataMgr.settingData.textFloat || unitCfg.unitType == UnitType.Player) && textFloat)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + Mathf.Ceil(hp), UITextFloatType.Recover, base.transform.position);
			}
			if (unitCfg.unitType == UnitType.Player)
			{
				UIPlayerDataMgr.Inst.UpdateHP();
				EventMgr.PlayerHPOrShiledChange?.Invoke();
			}
		}
	}

	public void BonusTakeDamageRatioRegister(float ratioInPercent, float effectDuration)
	{
		DamageReciveIncresePercent = 1f + Mathf.Max(0f, ratioInPercent / 100f);
		damageReciveIncresePercentTimer = effectDuration;
		if (damageReciveIncresePercentTimer > 0f)
		{
			CreateDamageReciveIncressEffect();
		}
	}

	public void ResetRegister()
	{
		flyCounter = 0;
		immnueMucusCounter = 0;
		immnueVenomCounter = 0;
		unitCfg.immuneAbyss = false;
		unitCfg.immuneSpike = false;
	}

	public void FlyRegister()
	{
		flyCounter++;
		if (affect_IsMucusDecelerate)
		{
			affect_IsMucusDecelerate = false;
			ec_Deceleration.gameObject.SetActive(value: false);
		}
	}

	public void FlyRegisterWithMate()
	{
		FlyRegister();
		if (unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack)
		{
			for (int i = 0; i < UnitBas.mateSummonsPpts.Count; i++)
			{
				UnitBas.mateSummonsPpts[i].FlyRegisterWithMate();
			}
			for (int j = 0; j < UnitBas.mateSummonsNotAttackPpts.Count; j++)
			{
				UnitBas.mateSummonsNotAttackPpts[j].FlyRegisterWithMate();
			}
		}
	}

	public void FlyUnregister()
	{
		flyCounter--;
		if (flyCounter < 0)
		{
			flyCounter = 0;
			Debug.LogError("为什么飞翔计数器会小于0");
		}
	}

	public void FlyUnregisterWithMate()
	{
		FlyUnregister();
		if (unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack)
		{
			for (int i = 0; i < UnitBas.mateSummonsPpts.Count; i++)
			{
				UnitBas.mateSummonsPpts[i].FlyUnregisterWithMate();
			}
			for (int j = 0; j < UnitBas.mateSummonsNotAttackPpts.Count; j++)
			{
				UnitBas.mateSummonsNotAttackPpts[j].FlyUnregisterWithMate();
			}
		}
	}

	public void InvincibleRegister()
	{
		invincibleCounter++;
		UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(myEntity);
		componentData.InvincibleRegister();
		UnitDotsSyncSystem.SetComponentData(componentData, myEntity);
	}

	public void InvincibleUnregister()
	{
		invincibleCounter--;
		if (invincibleCounter < 0)
		{
			invincibleCounter = 0;
			Debug.LogWarning("为什么无敌计数器会小于0");
		}
		UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(myEntity);
		componentData.InvincibleUnregister();
		UnitDotsSyncSystem.SetComponentData(componentData, myEntity);
	}

	public void ImmuneKnockbackRegister_Dots()
	{
		UnitProperty_Dots componentData = UnitBas.GetComponentData<UnitProperty_Dots>();
		componentData.ImmuneKnockbackRegister();
		UnitBas.SetComponentData(componentData);
	}

	public void ImmuneKnockbackUnregister_Dots()
	{
		UnitProperty_Dots componentData = UnitBas.GetComponentData<UnitProperty_Dots>();
		componentData.ImmuneKnockbackUnregister();
		UnitBas.SetComponentData(componentData);
	}

	public IEnumerator SetTempDeathImmute(TeammateAnnounceDeathInfo deathInfo, TakeDamageInfo damageInfo)
	{
		this.OnEnterDelayDeathEvent?.Invoke();
		isUnitDead = true;
		InvincibleRegister();
		CanBeTarget = false;
		if (deathInfo.setOneHp)
		{
			unitCfg.currentHP = 1f;
		}
		yield return new WaitForSeconds(deathInfo.delayDeathTime);
		InvincibleUnregister();
		if (unitCfg.deadEF == "EF_Dead_Blood")
		{
			unitCfg.deadEF = "EF_Dead_Ghost";
		}
		AnnouncedDeath(damageInfo);
	}

	public void StartFuseProgress()
	{
		if (UnitBas is Teammate teammate)
		{
			teammate.FusionData.IsFusing = true;
		}
		this.OnEnterFuseStageEvent?.Invoke();
	}

	public virtual void SetUnitInvisibleState(bool state)
	{
		if (tsf_Layer.gameObject.activeSelf == state)
		{
			tsf_Layer.gameObject.SetActive(!state);
			Transform transform = base.gameObject.transform.Find("ShadowCircleTheme6(Clone)");
			if ((bool)transform)
			{
				transform.gameObject.SetActive(!state);
			}
		}
		if ((bool)UmbrellaCtrl)
		{
			if (state)
			{
				UmbrellaCtrl.Hide();
			}
			else
			{
				UmbrellaCtrl.Show();
			}
		}
	}

	public void TeammateAnnounceDeath(TeammateAnnounceDeathInfo info)
	{
		if (info.isInstanceDeath)
		{
			AnnouncedDeath(null, instanceDeath: true);
		}
		else if (UnitBas.SummonerSpellBase != null && (UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime > 0f || info.delayDeathTime > 0f) && (unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack))
		{
			isUnitDead = true;
			info.delayDeathTime = ((info.delayDeathTime > 0f) ? info.delayDeathTime : UnitBas.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime);
			StartCoroutine(SetTempDeathImmute(info, null));
		}
		else
		{
			AnnouncedDeath();
		}
	}

	public void RemoveFromOwnerList()
	{
		if (UnitBas.SummonerSpellBase != null)
		{
			if (UnitBas.SummonerSpellBase.ownerPpt.tag == "Player")
			{
				PlayerMgr.Inst.SummonsUnregister(this);
			}
			else
			{
				UnitBas.SummonerSpellBase.ownerPpt.UnitBas.MateSummonsUnregister(this);
			}
		}
	}

	public void AnnouncedDeath_Dots()
	{
		UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(myEntity);
		componentData.AnnouncedDeath(myEntity);
		UnitDotsSyncSystem.SetComponentData(componentData, myEntity);
	}

	public void AnnouncedDeath(TakeDamageInfo info = null, bool instanceDeath = false)
	{
		if (!AlreadyDead)
		{
			AlreadyDead = true;
			if (info == null)
			{
				info = new TakeDamageInfo();
			}
			info.beHitPpt = this;
			unitCfg.currentHP = 0f;
			if (unitCfg.unitType == UnitType.Player)
			{
				PlayerMgr.Inst.SummonsAllDead();
			}
			else if (unitCfg.unitType == UnitType.Teammate || unitCfg.unitType == UnitType.TeammateNotAttack)
			{
				unitCfg.currentHP = Mathf.Max(unitCfg.currentHP, UnitBas.SummonerSpellBase ? (unitCfg.maxHP * UnitBas.SummonerSpellBase.SpellSummonInstantDeathHpRatio) : 0f);
				UnitBas.MateSummonsAllDead(instanceDeath);
				UnitBas.DeathExplodeSpawnCheck(info, unitCfg.currentHP);
			}
			if (info.beHitPpt.unitCfg.triggerDeadEvent && unitCfg.unitType == UnitType.Monster)
			{
				WandPostSlotTrigger.PostSlotKillEnemyTriggerCheck(info);
			}
			if ((info.beHitPpt.unitCfg.triggerDeadEvent || Spell3129VoidExplosion.specialVoidExplosionTriggerableUnitIdList.Contains(info.beHitPpt.unitCfg.id)) && voidExplosionData != null)
			{
				GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31291, base.transform.position, 2.5f);
				gO.transform.localScale = Vector3.one;
				gO.GetComponent<Spell3129VoidExplosion>().DataInitialize(unitCfg.maxHP, voidExplosionData);
			}
			RecycleDamageReciveIncressEffect();
			switch (unitCfg.unitType)
			{
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
				RemoveFromOwnerList();
				break;
			default:
				Debug.LogError(unitCfg.unitType);
				break;
			case UnitType.Player:
			case UnitType.Monster:
			case UnitType.Elite:
			case UnitType.Boss:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				break;
			}
		}
	}

	public void SetUnitConfig(UnitConfig unitCfg)
	{
		this.unitCfg = unitCfg;
	}

	public void RegetSR()
	{
		StartCoroutine(ReGetSRIE());
	}

	private IEnumerator ReGetSRIE()
	{
		yield return null;
		SR_Models = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
	}

	public void ShowBLiveSummonerName(string summonerName)
	{
		if (blive_summonerName == null)
		{
			blive_summonerName = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIBLiveEnemyName"), tsf_Layer.position + new Vector3(0f, unitCfg.relicShowHPUIHight - 0.2f, 0f) * tsf_Layer.lossyScale.y, Quaternion.identity, tsf_Layer);
			blive_summonerNameText = blive_summonerName.GetComponentInChildren<Text>();
		}
		blive_summonerName.SetActive(value: true);
		blive_summonerNameText.text = summonerName;
	}

	public virtual Vector3 GetSpellRotateFollowPoint()
	{
		if (unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
		{
			return PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position + (PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.position - PlayerMgr.Inst.PlayerPoint).normalized;
		}
		return base.transform.position;
	}

	private void OnDisable()
	{
		RecycleDamageReciveIncressEffect();
		if ((bool)blive_summonerName)
		{
			blive_summonerName.SetActive(value: false);
		}
	}

	private void UpdateDamageReciveIncreseEffectPosition()
	{
		if ((bool)damageReciveIncreseEffectGO)
		{
			damageReciveIncreseEffectGO.transform.position = base.transform.position;
		}
	}

	private void RecycleDamageReciveIncressEffect()
	{
		if ((bool)damageReciveIncreseEffectGO)
		{
			ParticleSystem[] componentsInChildren = damageReciveIncreseEffectGO.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stop();
			}
			ObjPoolMgr.Inst.RecycleGO(damageReciveIncreseEffectGO, 1f);
			damageReciveIncreseEffectGO = null;
		}
	}

	private void CreateDamageReciveIncressEffect()
	{
		if (!damageReciveIncreseEffectGO && !AlreadyDead)
		{
			if (GameMgr.IsHarmony_Static)
			{
				damageReciveIncreseEffectGO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_BonusTakeDamageH", base.transform.position);
			}
			else
			{
				damageReciveIncreseEffectGO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_BonusTakeDamage", base.transform.position);
			}
		}
	}
}
