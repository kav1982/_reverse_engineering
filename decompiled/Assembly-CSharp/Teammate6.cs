using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;
using _Scripts.Units;

public class Teammate6 : Teammate
{
	public class HoldingTeammateData
	{
		public int SoulBombDamage;

		public UnitProperty BombPpt;

		public GameObject BombObject;

		public Vector3 BombTargetEndPosition = Vector3.zero;

		public Teammate6BombState state;

		public Vector3 direction = Vector3.zero;

		public Spell2006CannonBarrelController targetCannonScript;

		public float duration = 3f;

		public float currentRotationAngle;

		public UnitProperty chasingTarget;

		public Transform TargetHookTransform;

		public GameObject BombTrailGameObject;

		public GameObject BombOutlookObject;
	}

	public enum Teammate6BombState
	{
		Holding_Barrel,
		Holding_BackUpAmmo,
		Shooting,
		Hook_Backing,
		QuickReloading
	}

	private enum Teammate6State
	{
		Idle,
		Move,
		SeekingAmmo,
		ReadyToShootFindingTarget,
		CloseAttack,
		QuickReload,
		LoadingMagazine
	}

	[Header("SoulBomb")]
	public int DefaultSoulBombNumCount;

	private int soulBombLimit;

	private List<HoldingTeammateData> holdingBombDataList = new List<HoldingTeammateData>();

	public float soulBombRefillInterval;

	private float soulBombRefillTimer;

	private List<UnitProperty> lockList = new List<UnitProperty>();

	private float SoulBombHPToDamageRatio;

	public float SoulBombBaseExplosionRadiu;

	private float soulBombExplosionRadiu;

	private int onceShootCount = 1;

	private Vector3 bombShootPosition;

	public float FlyingBombBaseSpeed;

	private float flyingBombSpeed;

	[Header("PickUp")]
	public float PickUpBaseRange;

	private float pickUpRange;

	public float PickUpSpeedUpRatio;

	private UnitProperty nearestTeammate;

	public float nearestTeammateRecheckInterval;

	private float nearTeammateRecheckTimer;

	public float fuseBonusPickUpRangePerLevel;

	[Header("CloseRangeAttack")]
	public int ClosrRangeAttackBaseDamage;

	public float CloseRangeAttackBaseRange;

	private int closeRangeAttackDamage;

	private float closeRangeAttackRange;

	[Header("EssenceSpirit")]
	public float HookDetectBaseRange;

	public float HookProcessTime;

	private float hookDetectRange;

	public Transform BackUpAmmoTransform;

	public Transform HookTransform;

	private List<(GameObject Hook, UnitProperty Target, HoldingTeammateData data)> HookTeammateDataList = new List<(GameObject, UnitProperty, HoldingTeammateData)>();

	public Transform QuickReloadHandTransform;

	private bool acting;

	private bool moving;

	public Transform ModelTransform;

	private Teammate6State currentState;

	private bool barrelLockingTarget;

	public Transform BarrelTransform;

	public Transform BarrelInnerTransform;

	public Transform ProgressTransform;

	private List<Spell2006CannonBarrelController> CannonControllerList = new List<Spell2006CannonBarrelController>();

	[HideInInspector]
	public float shootChargeProgress;

	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	private float idleWalkTimer;

	private float idleTimer;

	public float FindNewTartgetInterval;

	private float findNewTargetTimer;

	public float AttackPushForce;

	public ShockParam closeAttackShock;

	public ShockParam BombExplosionShock;

	private static readonly List<UnitProperty> AllHoldingTeammates = new List<UnitProperty>();

	private static readonly List<Teammate6> teammate6List = new List<Teammate6>();

	public SkeletonAnimation SAnimaHand;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public Shadow ShadowScript;

	private Material bodyMaterial;

	public Material bodyBaseMaterial;

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		bodyMaterial = UnityEngine.Object.Instantiate(bodyBaseMaterial);
		base.SAnima.CustomMaterialOverride.Add(bodyBaseMaterial, bodyMaterial);
		SAnimaHand.CustomMaterialOverride.Add(bodyBaseMaterial, bodyMaterial);
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		lockList.Clear();
		soulBombRefillTimer = soulBombRefillInterval - 0.3f;
		holdingBombDataList.Clear();
		soulBombLimit = DefaultSoulBombNumCount;
		onceShootCount = 1;
		currentState = Teammate6State.SeekingAmmo;
		nearTeammateRecheckTimer = 0f;
		soulBombExplosionRadiu = 0f;
		idleWalkTimer = 0f;
		idleTimer = 0f;
		acting = false;
		moving = false;
		base.Anima.SetTrigger("Idle");
		base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
		SAnimaHand.AnimationState.SetAnimation(0, "Idle", loop: true);
		base.Anima.Play("Idle");
		bombShootPosition = base.transform.position;
		HookTeammateDataList.Clear();
		HideAllCannon();
		barrelLockingTarget = false;
		base.SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(UseGhostEffect, 0f);
		base.SAnima.CustomMaterialOverride[bodyBaseMaterial].SetInt(UseFuseShineEffect, 0);
		base.SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(0, FuseShineProcess);
		ShadowScript.ShadowGO.SetActive(value: true);
	}

	public override void Frame1InitialCallback()
	{
		base.Frame1InitialCallback();
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		float num = base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
		float num2 = base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio;
		_ = base.SummonerSpellBase.SIP.finalDamageExtra;
		SoulBombHPToDamageRatio = base.SummonerSpellBase.spellCfg.float3;
		closeRangeAttackDamage = Mathf.CeilToInt((float)ClosrRangeAttackBaseDamage * num2);
		closeRangeAttackRange = CloseRangeAttackBaseRange * num * base.transform.localScale.x;
		soulBombExplosionRadiu = SoulBombBaseExplosionRadiu * num;
		pickUpRange = (PickUpBaseRange + fuseBonusPickUpRangePerLevel * (float)FusionData.CurrentFusionLevel) * num;
		hookDetectRange = HookDetectBaseRange * num;
		onceShootCount = ((!base.SummonerSpellBase.shooterWand) ? 1 : base.SummonerSpellBase.shooterWand.GetWandOnceShootCountWithEnhance());
		base.Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		base.SAnima.timeScale = base.Anima.speed;
		SAnimaHand.timeScale = base.Anima.speed;
		flyingBombSpeed = FlyingBombBaseSpeed * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		InitialBodySkin();
		base.Anima.SetTrigger("Idle");
		ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
		ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
		base.Anima.Play("Idle");
		CannonDataInitialize();
		if (teammate6List.Count <= 0 || (float)(FusionData.CurrentFusionLevel + 1) * base.SummonerSpellBase.finalDamageRatio * base.SummonerSpellBase.damageRatio > (float)(teammate6List[0].FusionData.CurrentFusionLevel + 1) * teammate6List[0].SummonerSpellBase.finalDamageRatio * teammate6List[0].SummonerSpellBase.damageRatio)
		{
			teammate6List.Insert(0, this);
		}
		else
		{
			teammate6List.Add(this);
		}
		ShadowScript.ShadowGO.transform.localScale = Vector3.one * 1.5f * Mathf.Abs(myPpt.tsf_Layer.localScale.x);
	}

	private void InitialBodySkin()
	{
		base.SAnima.initialSkinName = GetSkinName("Teammate6_0");
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level <= 0)
		{
			SAnimaHand.initialSkinName = GetSkinName("Teammate6_1");
		}
		else
		{
			SAnimaHand.initialSkinName = GetSkinName("Teammate6_2");
		}
		base.SAnima.Initialize(overwrite: true);
		SAnimaHand.Initialize(overwrite: true);
	}

	private string GetSkinName(string baseName)
	{
		string text = baseName;
		switch (base.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Frozen:
			text += "_Frozen";
			break;
		case SpellColorType.Mucus:
			text += "_Mucus";
			break;
		case SpellColorType.Venom:
			text += "_Venom";
			break;
		case SpellColorType.Fire:
			text += "_Fire";
			break;
		case SpellColorType.Thunder:
			text += "_Thunder";
			break;
		case SpellColorType.Void:
			text += "_Void";
			break;
		}
		return text;
	}

	private void HideAllCannon()
	{
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.gameObject.SetActive(value: false);
		}
	}

	private bool targetTeammateIsUnderSomebodyControl(UnitProperty target)
	{
		return AllHoldingTeammates.Contains(target);
	}

	private void CannonDataInitialize()
	{
		int num = FusionData.CurrentFusionLevel + 1 - CannonControllerList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = SpawnCannon();
				CannonControllerList.Add(gameObject.GetComponent<Spell2006CannonBarrelController>());
			}
		}
		for (int j = 0; j <= FusionData.CurrentFusionLevel; j++)
		{
			CannonControllerList[j].gameObject.SetActive(value: true);
			if (j % 2 == 0)
			{
				CannonControllerList[j].transform.localPosition = new Vector3(0f, 0.3f + 0.6f * (float)j / 2f, 0.03f * (float)j / 2f);
			}
			else
			{
				CannonControllerList[j].transform.localPosition = new Vector3(0.25f, 0.5f + 0.6f * (float)(j - 1) / 2f, 0.05f + 0.03f * (float)(j - 1) / 2f);
			}
			if (j % 2 == 0)
			{
				CannonControllerList[j].TattooToggle(toggle: true);
			}
			if (j <= 3 && j == FusionData.CurrentFusionLevel)
			{
				CannonControllerList[j].HandToggle(toggle: true);
			}
			CannonControllerList[j].BarrelInitialize(base.SummonerSpellBase.ColorType, this);
		}
	}

	private GameObject SpawnCannon()
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 20061 + "/" + 20061 + "_CannonBarrel"), BarrelTransform);
	}

	private void UpdateCannonDirection()
	{
		if (barrelLockingTarget)
		{
			BarrelInnerTransform.transform.right = Tool2D.DirMoveTowards(BarrelInnerTransform.transform.right, Tool2D.IgnoreZV2ToV1(bombShootPosition, base.transform.position).normalized * ModelTransform.localScale.x, 240f * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio * Time.deltaTime);
			return;
		}
		BarrelTransform.localEulerAngles = Vector3.zero;
		BarrelInnerTransform.transform.right = Tool2D.DirMoveTowards(BarrelInnerTransform.right, BarrelTransform.right, 480f * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio * Time.deltaTime);
	}

	public override void Update()
	{
		SummonsTouchMonster();
		base.Update();
		if (!base.IsLocked)
		{
			UpdateCannonTattooState();
			CheckNearestTeammate();
			UpdateMainState();
			UpdateSummonFaceDirection();
			UpdatePassiveFinalHp();
			RecordSelfHoldingTeammate();
			UpdateCannonDirection();
			UpdateBodyTranparency();
		}
	}

	public void UpdateBodyTranparency()
	{
		float value = ((base.beingControlledByTeammate6 || base.SAnima.CustomMaterialOverride[bodyBaseMaterial].GetInt(UseFuseShineEffect) > 0) ? 0f : DataMgr.settingData.SummonTransparent);
		base.SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(Transparency, value);
	}

	private void LateUpdate()
	{
		UpdateHoldingTeammateState();
		for (int i = 0; i < holdingBombDataList.Count; i++)
		{
			HoldingTeammateData holdingTeammateData = holdingBombDataList[i];
			if (holdingTeammateData.state == Teammate6BombState.Holding_BackUpAmmo)
			{
				UpdateBackUpAmmoHoldingTeammateState(holdingTeammateData);
			}
		}
		AllHoldingTeammates.Clear();
	}

	public bool IsTargetTeammate6CatchAble(UnitProperty target)
	{
		if (SpellConfig.dic[target.UnitBas.SummonerSpellBase.spellCfg.id].abilityType != SpellAbilityType.Summon6)
		{
			return true;
		}
		Teammate6 item = (Teammate6)target.UnitBas;
		return teammate6List.IndexOf(item) > teammate6List.IndexOf(this);
	}

	private void RecordSelfHoldingTeammate()
	{
		foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
		{
			AllHoldingTeammates.Add(holdingBombData.BombPpt);
		}
	}

	private void UpdateCannonTattooState()
	{
		float x = ProgressTransform.localScale.x;
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.UpdateTattooProgress(x);
		}
	}

	public void ChangeSpineAnimationState(SkeletonAnimation targetSanima, string targetAnimationName, bool isLoop)
	{
		targetSanima.AnimationState.SetAnimation(0, targetAnimationName, isLoop);
	}

	private void UpdateMainState()
	{
		findNewTargetTimer += Time.deltaTime;
		if (base.beingControlledByTeammate6)
		{
			return;
		}
		switch (currentState)
		{
		case Teammate6State.Idle:
			GetNearestTarget();
			if ((bool)targetPpt)
			{
				currentState = Teammate6State.SeekingAmmo;
			}
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				currentState = Teammate6State.Move;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				idleWalkTime.RandomResult();
				base.Anima.SetTrigger("Move");
				ChangeSpineAnimationState(base.SAnima, "Walk", isLoop: true);
				ChangeSpineAnimationState(SAnimaHand, "Walk", isLoop: true);
			}
			break;
		case Teammate6State.Move:
			if (!targetPpt)
			{
				GetNearestTarget();
			}
			if ((bool)targetPpt)
			{
				currentState = Teammate6State.SeekingAmmo;
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed());
				CheckNavInfo();
			}
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				currentState = Teammate6State.Idle;
				idleTime.RandomResult();
				base.Anima.SetTrigger("Idle");
				ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
				ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
			}
			break;
		case Teammate6State.SeekingAmmo:
			if (acting)
			{
				break;
			}
			if (CheckIfCannonHasValidAmmo())
			{
				currentState = Teammate6State.ReadyToShootFindingTarget;
				break;
			}
			if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && CheckBackUpAmmoState())
			{
				currentState = Teammate6State.QuickReload;
				break;
			}
			if (nearestTeammate != null && Tool2D.IgnoreZDistanceSqr(base.transform.position, nearestTeammate.transform.position) >= pickUpRange * pickUpRange && base.SummonerSpellBase.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				if (!moving)
				{
					moving = true;
					base.Anima.SetTrigger("Run");
					base.SAnima.AnimationState.SetAnimation(0, "Walk", loop: true);
					SAnimaHand.AnimationState.SetAnimation(0, "Walk", loop: true);
				}
				MoveToNearestTeammate();
			}
			else
			{
				moving = false;
				base.Anima.SetTrigger("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaHand.AnimationState.SetAnimation(0, "Idle", loop: true);
				SetMove(Vector3.zero);
			}
			FindSacrificebleTeammate();
			if (lockList.Count > 0 && !base.beingControlledByTeammate6)
			{
				if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0)
				{
					base.Anima.SetTrigger("throwHook");
					base.SAnima.AnimationState.SetAnimation(0, "ThrowHookAfter", loop: false);
					SAnimaHand.AnimationState.SetAnimation(0, "ThrowHookAfter", loop: false);
					SetMove(Vector3.zero);
					acting = true;
					ThrowHookChangeFaceDirection();
				}
				else if (Tool2D.IgnoreZDistanceSqr(base.transform.position, nearestTeammate.transform.position) <= pickUpRange * pickUpRange)
				{
					base.Anima.SetTrigger("Catch");
					base.SAnima.AnimationState.SetAnimation(0, "PickUpTeammate1", loop: false);
					SAnimaHand.AnimationState.SetAnimation(0, "PickUpTeammate1", loop: false);
					SetMove(Vector3.zero);
					acting = true;
				}
			}
			if (!nearestTeammate)
			{
				moving = false;
				currentState = Teammate6State.CloseAttack;
			}
			break;
		case Teammate6State.CloseAttack:
			if (acting)
			{
				SetMove(Vector3.zero);
				break;
			}
			if ((bool)nearestTeammate)
			{
				moving = false;
				currentState = Teammate6State.SeekingAmmo;
				base.Anima.SetTrigger("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaHand.AnimationState.SetAnimation(0, "Idle", loop: true);
				break;
			}
			if (!targetPpt || findNewTargetTimer >= FindNewTartgetInterval)
			{
				GetNearestTarget();
			}
			if ((bool)targetPpt && targetPpt.gameObject.activeInHierarchy && !base.beingControlledByTeammate6)
			{
				float num = 0.3f * base.transform.localScale.x + closeRangeAttackRange / 2f + targetPpt.UnitBas.GetBodyColliderRadius();
				if (Tool2D.IgnoreZDistanceSqr(base.transform.position, targetPpt.transform.position) < num * num)
				{
					base.Anima.SetTrigger("CloseAttack");
					ChangeSpineAnimationState(base.SAnima, "CloseRangeAttacking", isLoop: true);
					ChangeSpineAnimationState(SAnimaHand, "CloseRangeAttacking", isLoop: true);
					acting = true;
					moving = false;
					SetMove(Vector3.zero);
				}
				else if (base.SummonerSpellBase.currentSpellMovement != SpellSpecialMovementType.Rotation)
				{
					if (!moving)
					{
						moving = true;
						base.Anima.SetTrigger("Move");
						ChangeSpineAnimationState(base.SAnima, "Walk", isLoop: true);
						ChangeSpineAnimationState(SAnimaHand, "Walk", isLoop: true);
					}
					MoveToNearestTarget();
				}
			}
			else
			{
				moving = false;
				targetPpt = null;
				SetMove(Vector3.zero);
				currentState = Teammate6State.Idle;
			}
			break;
		case Teammate6State.QuickReload:
		{
			if (acting)
			{
				SetMove(Vector3.zero);
				break;
			}
			if (!targetPpt || !targetPpt.CanBeTarget || findNewTargetTimer > FindNewTartgetInterval)
			{
				GetNearestTarget();
			}
			int num2 = 0;
			if ((bool)targetPpt && targetPpt.gameObject.activeInHierarchy)
			{
				for (int i = 0; i < holdingBombDataList.Count; i++)
				{
					HoldingTeammateData holdingTeammateData = holdingBombDataList[i];
					if (holdingTeammateData.state == Teammate6BombState.Holding_BackUpAmmo)
					{
						holdingTeammateData.targetCannonScript.CannonLoadBackUpAmmo();
						holdingTeammateData.state = Teammate6BombState.Holding_Barrel;
						num2++;
					}
					if (num2 > FusionData.CurrentFusionLevel)
					{
						break;
					}
				}
				if (num2 > 0)
				{
					base.Anima.SetTrigger("FastReload");
					acting = true;
					break;
				}
				moving = false;
				targetPpt = null;
				SetMove(Vector3.zero);
				currentState = Teammate6State.Idle;
			}
			else
			{
				moving = false;
				targetPpt = null;
				SetMove(Vector3.zero);
				currentState = Teammate6State.Idle;
			}
			break;
		}
		case Teammate6State.ReadyToShootFindingTarget:
			if (acting)
			{
				SetMove(Vector3.zero);
				if ((bool)targetPpt && targetPpt.gameObject.activeInHierarchy)
				{
					bombShootPosition = targetPpt.transform.position;
				}
				else
				{
					GetNearestTarget();
				}
				break;
			}
			if (!targetPpt || !targetPpt.CanBeTarget || findNewTargetTimer > FindNewTartgetInterval)
			{
				GetNearestTarget();
			}
			if ((bool)targetPpt && targetPpt.gameObject.activeInHierarchy)
			{
				base.Anima.SetTrigger("Shoot");
				ChangeSpineAnimationState(base.SAnima, "Shoot", isLoop: false);
				ChangeSpineAnimationState(SAnimaHand, "Shoot", isLoop: false);
				acting = true;
			}
			else
			{
				moving = false;
				targetPpt = null;
				SetMove(Vector3.zero);
				currentState = Teammate6State.Idle;
				UpdateSummonFaceDirection();
			}
			CheckAmmoState();
			break;
		case Teammate6State.LoadingMagazine:
			SetMove(Vector3.zero);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation && base.CanMove)
		{
			SetMove(Vector3.zero);
			float num3 = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / GetSummonUnitRealMoveSpeed()) * Time.deltaTime;
			base.SummonerSpellBase.spellAroundOwnerCurrentAngle += num3;
			if ((bool)base.SummonerSpellBase.ownerPpt && targetTeammateIsUnderSomebodyControl(base.SummonerSpellBase.ownerPpt))
			{
				base.SummonerSpellBase.ownerPpt = PlayerMgr.Inst.PlayerPpt;
			}
			Vector3 v = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			base.SummonerSpellBase.SpellAroundPlayerUpdateMoveTrigger(num3);
		}
		if (findNewTargetTimer >= FindNewTartgetInterval)
		{
			findNewTargetTimer = 0f;
		}
	}

	private bool CheckBackUpAmmoState()
	{
		foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
		{
			if (holdingBombData.state == Teammate6BombState.Holding_BackUpAmmo)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckIfCannonHasValidAmmo()
	{
		foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
		{
			if (holdingBombData.state == Teammate6BombState.Holding_Barrel)
			{
				return true;
			}
		}
		return false;
	}

	private void ThrowHook()
	{
		int num = (FusionData.CurrentFusionLevel + 1) * base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 4;
		foreach (UnitProperty @lock in lockList)
		{
			GameObject gameObject = SpawnHook();
			gameObject.GetComponent<Spell2006RopeController>().InitialHookEffect(HookTransform, @lock.transform, HookProcessTime / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio);
			HookTeammateDataList.Add((gameObject, @lock, null));
			num--;
			if (num <= 0)
			{
				break;
			}
		}
	}

	private void HookCatchTargets()
	{
		for (int i = 0; i < HookTeammateDataList.Count; i++)
		{
			(GameObject, UnitProperty, HoldingTeammateData) tuple = HookTeammateDataList[i];
			UnitProperty item = tuple.Item2;
			if (!item.isUnitDead && item.gameObject.activeInHierarchy && !targetTeammateIsUnderSomebodyControl(item))
			{
				HoldingTeammateData holdingTeammateData = new HoldingTeammateData();
				holdingTeammateData.SoulBombDamage = Mathf.CeilToInt(item.unitCfg.maxHP * SoulBombHPToDamageRatio * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio);
				holdingTeammateData.BombPpt = item;
				holdingTeammateData.BombObject = item.gameObject;
				holdingTeammateData.state = Teammate6BombState.Hook_Backing;
				holdingTeammateData.TargetHookTransform = tuple.Item1.GetComponent<Spell2006RopeController>().HookCenterTransform;
				item.UnitBas.SummonerSpellBase.SpellSummonHPFixDropAmount = 0f;
				(int, int) cannonData = GetCannonData(i);
				holdingTeammateData.targetCannonScript = CannonControllerList[cannonData.Item1];
				holdingTeammateData.BombOutlookObject = SpawnTeammateBall(item, item.UnitBas.SummonerSpellBase.ColorType);
				item.RemoveFromOwnerList();
				HookTeammateDataList[i] = (tuple.Item1, tuple.Item2, holdingTeammateData);
				holdingBombDataList.Add(holdingTeammateData);
				AddNewHoldingTeammate(item);
			}
		}
	}

	private (int cannonIndex, int inCannonIndex) GetCannonData(int bombIndex)
	{
		int num = FusionData.CurrentFusionLevel + 1;
		return (bombIndex % num, Mathf.CeilToInt((float)Mathf.Max(0, bombIndex + 1) / (float)num) - 1);
	}

	private GameObject SpawnHook()
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_Hook");
	}

	private GameObject SpawnTeammateBall(UnitProperty ppt, SpellColorType color)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_TeammateBall");
	}

	private void CheckNearestTeammate()
	{
		if (nearestTeammate == null || !nearestTeammate.gameObject.activeInHierarchy || CheckTargetTeammateDistanceIsCatchable(nearestTeammate))
		{
			nearestTeammate = GetNearestTeammate();
		}
		nearTeammateRecheckTimer += Time.deltaTime;
		if (nearTeammateRecheckTimer >= nearestTeammateRecheckInterval)
		{
			nearTeammateRecheckTimer = 0f;
			nearestTeammate = GetNearestTeammate();
		}
	}

	private void MoveToNearestTeammate()
	{
		float num = ((currentState == Teammate6State.SeekingAmmo) ? 1.2f : 1f);
		GetNavInfo(nearestTeammate.transform.position + (nearestTeammate.transform.position - base.transform.position).normalized * pickUpRange);
		SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed() * num);
	}

	private void MoveToNearestTarget()
	{
		GetNavInfo(targetPpt.transform.position);
		SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed());
	}

	private void UpdatePassiveFinalHp()
	{
		float currentHPRatio = base.CurrentHPRatio;
		int num = 0;
		UnitProperty ownerPpt = base.SummonerSpellBase.ownerPpt;
		float num2 = 1f;
		float spellSummonGainOwnerHpRatio = base.SummonerSpellBase.SpellSummonGainOwnerHpRatio;
		if (base.SummonerSpellBase.ShootData?.Spell != null)
		{
			num2 += (float)base.SummonerSpellBase.ShootData.Spell.specialInt * base.SummonerSpellBase.spellCfg.float1 / 100f;
		}
		UnitBase unitBas = ownerPpt.UnitBas;
		num = ((!(unitBas is Teammate5) && !(unitBas is Teammate5FuseController)) ? Mathf.CeilToInt(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP * spellSummonGainOwnerHpRatio) : Mathf.CeilToInt(ownerPpt.unitCfg.maxHP * spellSummonGainOwnerHpRatio));
		myPpt.unitCfg.maxHP = Mathf.CeilToInt((UnitConfig.map[base.SummonerSpellBase.spellCfg.summonID].maxHP + (float)num) * base.SummonerSpellBase.SpellSummonHPRatio * base.SummonerSpellBase.SpellSUmmonFinalHpRatio * (float)(FusionData.CurrentFusionLevel + 1) * num2);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * currentHPRatio;
	}

	private float GetPassiveFinalDamageRatio()
	{
		return 1f;
	}

	private void CloseRangeAttack()
	{
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position + (base.TargetPoint - base.transform.position).normalized * closeRangeAttackRange / 2f);
		int hitDamage = Mathf.CeilToInt((float)(closeRangeAttackDamage * (FusionData.CurrentFusionLevel + 1)) * GetPassiveFinalDamageRatio() + base.SummonerSpellBase.SIP.finalDamageExtra);
		CreateMeleeAttackEffect(vector, closeRangeAttackRange);
		DealDamageToTargetInRange(vector, hitDamage, closeRangeAttackRange);
		SEMgr.Inst.teammate6MeleeAttack.PlaySE(SEPlayMode.Replay, 3, 0.2f);
	}

	private UnitProperty GetNearestTeammate()
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, 20f, "Teammate");
		UnitProperty result = null;
		float num = 999f;
		foreach (Collider item in collidersByTag)
		{
			UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
			if ((bool)component && !(component.UnitBas is Teammate4WallHItBox) && component.UnitBas.SummonerSpellBase.spellCfg.abilityType != SpellAbilityType.WandSpirit && IsTargetTeammate6CatchAble(component) && !component.isUnitDead && CheckTargetTeammateDistanceIsCatchable(component))
			{
				float num2 = Tool2D.IgnoreZDistanceSqr(base.transform.position, component.transform.position);
				if (num2 < num)
				{
					result = component;
					num = num2;
				}
			}
		}
		return result;
	}

	private bool CheckTargetTeammateDistanceIsCatchable(UnitProperty targetPpt)
	{
		if (targetTeammateIsUnderSomebodyControl(targetPpt))
		{
			return false;
		}
		Vector3 position = targetPpt.transform.position;
		return Tool2D.IgnoreZDistanceSqr(Tool2D.GetNavMeshPointIngoreZ(position + (position - base.transform.position).normalized * pickUpRange * 0.8f), position) <= pickUpRange * pickUpRange;
	}

	private void CheckAmmoState()
	{
		if (holdingBombDataList.Count <= 0)
		{
			currentState = Teammate6State.SeekingAmmo;
		}
	}

	private void UpdateSummonFaceDirection()
	{
		switch (currentState)
		{
		case Teammate6State.Idle:
			if ((bool)nearestTeammate && holdingBombDataList.Count <= 0)
			{
				ModelTransform.localScale = ((nearestTeammate.transform.position.x >= base.transform.position.x) ? Vector3.one : new Vector3(-1f, 1f, 1f));
				return;
			}
			break;
		case Teammate6State.SeekingAmmo:
			if (acting)
			{
				return;
			}
			if ((bool)nearestTeammate)
			{
				ModelTransform.localScale = ((nearestTeammate.transform.position.x >= base.transform.position.x) ? Vector3.one : new Vector3(-1f, 1f, 1f));
				return;
			}
			break;
		case Teammate6State.ReadyToShootFindingTarget:
			ModelTransform.localScale = ((bombShootPosition.x >= base.transform.position.x) ? Vector3.one : new Vector3(-1f, 1f, 1f));
			return;
		case Teammate6State.QuickReload:
			GetNearestTarget();
			if (targetPpt != null)
			{
				ModelTransform.localScale = ((targetPpt.transform.position.x - base.transform.position.x >= 0f) ? Vector3.one : new Vector3(-1f, 1f, 1f));
			}
			return;
		}
		if (myPpt.UnitBas.CurrentMotion.x > 0.05f)
		{
			ModelTransform.localScale = Vector3.one;
		}
		else if (myPpt.UnitBas.CurrentMotion.x < -0.05f)
		{
			ModelTransform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	private void ThrowHookChangeFaceDirection()
	{
		if ((bool)nearestTeammate)
		{
			ModelTransform.localScale = ((nearestTeammate.transform.position.x >= base.transform.position.x) ? Vector3.one : new Vector3(-1f, 1f, 1f));
		}
	}

	private void SpawnExplosion(HoldingTeammateData data)
	{
		Vector3 position = data.BombObject.transform.position;
		CreateBombExplosionEffect(position, soulBombExplosionRadiu);
		DealDamageToTargetInRange(position, Mathf.CeilToInt((float)data.SoulBombDamage * GetPassiveFinalDamageRatio() + base.SummonerSpellBase.SIP.finalDamageExtra), -1f, checkThunder: true);
		SpawnBombExplosionScreenShake(position);
		SEMgr.Inst.teammate6RangeShoot.PlaySE();
	}

	private void DealDamageToTargetInRange(Vector3 targetPos, int hitDamage, float radiu = -1f, bool checkThunder = false)
	{
		float num = ((radiu > 0f) ? radiu : soulBombExplosionRadiu);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(Tool2D.IgnoreZPoint(targetPos), num, "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness");
		hitDamage = Mathf.CeilToInt((float)hitDamage * (1f + GeneralTool.GetSpellRadiusToDamageRatio(num, base.SummonerSpellBase.SIP.radiuDecreaseRatio, base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio)));
		if (checkThunder && UnityEngine.Random.Range(0f, 1f) <= base.SummonerSpellBase.endTHunderHitChance)
		{
			base.SummonerSpellBase.EndThunderAttackCheck(thunderOnly: false, targetPos, hitDamage);
		}
		foreach (Collider item in collidersByTag.Where((Collider e) => e.gameObject.activeInHierarchy))
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
					spell1002RollBall.TakeDamage(hitDamage);
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster"))
			{
				UnitProperty component = item.GetComponent<UnitProperty>();
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo
				{
					damage = hitDamage,
					canRebound = false,
					criticalChance = base.SummonerSpellBase.overalCriticalChance + (float)base.SummonerSpellBase.ShootData.Spell.specialInt / 100f
				};
				base.SummonerSpellBase.ApplyVoidEffect(component);
				base.SummonerSpellBase.OutputDamage(component, takeDamageInfo);
				component.TakeKnockback((component.transform.position - targetPos).normalized * AttackPushForce);
				if (takeDamageInfo.isTargetDead && base.SummonerSpellBase.ShootData?.Spell != null && !base.SummonerSpellBase.IsSameCamp(takeDamageInfo.beHitPpt.unitCfg.unitType) && takeDamageInfo.beHitPpt.unitCfg.triggerDeadEvent)
				{
					KillSomeOne();
				}
			}
			else
			{
				TakeDamageInfo info = new TakeDamageInfo
				{
					damage = hitDamage,
					canRebound = false,
					criticalChance = base.SummonerSpellBase.overalCriticalChance + (float)base.SummonerSpellBase.ShootData.Spell.specialInt / 100f
				};
				base.SummonerSpellBase.OutputDamage(item.gameObject, info);
			}
		}
	}

	public void KillSomeOne()
	{
		if (base.SummonerSpellBase.ShootData?.Spell != null)
		{
			base.SummonerSpellBase.ShootData.Spell.specialInt++;
		}
	}

	private void FindSacrificebleTeammate()
	{
		soulBombRefillTimer += Time.deltaTime;
		if (soulBombRefillTimer < soulBombRefillInterval && base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level <= 0)
		{
			return;
		}
		soulBombRefillTimer = 0f;
		lockList.Clear();
		float radius = ((base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0) ? hookDetectRange : pickUpRange);
		List<UnitProperty> list = new List<UnitProperty>();
		foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
		{
			list.Add(holdingBombData.BombPpt);
		}
		foreach (Collider item in GeneralTool.GetCollidersByTag(base.transform.position, radius, "Teammate"))
		{
			UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
			if ((bool)component && !(component.UnitBas is Teammate4WallHItBox) && ((component.UnitBas.SummonerSpellBase.spellCfg.abilityType != SpellAbilityType.WandSpirit) & IsTargetTeammate6CatchAble(component)) && !component.isUnitDead && !list.Contains(component))
			{
				lockList.Add(component);
			}
		}
	}

	private void AddNewHoldingTeammate(UnitProperty targetPpt)
	{
		UnitBase unitBas = targetPpt.UnitBas;
		if (unitBas is Teammate teammate && teammate.SummonerSpellBase.spellCfg.useType == SpellType.Summon)
		{
			base.SummonerSpellBase.isOwnerSpellEnd = true;
		}
		if (unitBas is Teammate1 teammate2)
		{
			teammate2.ControldByTeammate6();
		}
		else if (unitBas is Teammate1FuseController teammate1FuseController)
		{
			teammate1FuseController.ControldByTeammate6();
		}
		else if (unitBas is Teammate2 teammate3)
		{
			teammate3.ControldByTeammate6();
		}
		else if (unitBas is Teammate2FuseController teammate2FuseController)
		{
			teammate2FuseController.ControldByTeammate6();
		}
		else if (unitBas is Teammate3 teammate4)
		{
			teammate4.ControldByTeammate6();
		}
		else if (unitBas is Teammate3FuseController teammate3FuseController)
		{
			teammate3FuseController.ControldByTeammate6();
		}
		else if (unitBas is Teammate4 teammate5)
		{
			teammate5.ControldByTeammate6();
		}
		else if (unitBas is Teammate4FuseController teammate4FuseController)
		{
			teammate4FuseController.ControldByTeammate6();
		}
		else if (unitBas is Teammate5 teammate6)
		{
			teammate6.ControldByTeammate6();
		}
		else if (unitBas is Teammate5FuseController teammate5FuseController)
		{
			teammate5FuseController.ControldByTeammate6();
		}
		else if (unitBas is Teammate6 teammate7)
		{
			teammate7.ControldByTeammate6();
		}
		else if (unitBas is Teammate7 teammate8)
		{
			teammate8.ControldByTeammate6();
		}
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		ColliderToggle(state: false);
		base.beingControlledByTeammate6 = true;
		ShadowScript.ShadowGO.SetActive(value: false);
		base.Anima.SetTrigger("Idle");
		ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
		ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
		acting = false;
		FreeAllHoldingTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.beingControlledByTeammate6 = false;
			ShadowScript.ShadowGO.SetActive(value: true);
			base.transform.eulerAngles = Vector3.zero;
			base.CanMove = true;
			acting = false;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		teammate6List.Remove(this);
	}

	private void CathchTargetTeammateInRange()
	{
		SEMgr.Inst.teammate6RangeLoad.PlaySE();
		float radius = PickUpBaseRange * base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, radius, "Teammate");
		List<UnitProperty> list = new List<UnitProperty>();
		int num = 0;
		foreach (Collider item in collidersByTag)
		{
			UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
			if ((bool)component && !(component.UnitBas is Teammate4WallHItBox) && ((component.UnitBas.SummonerSpellBase.spellCfg.abilityType != SpellAbilityType.WandSpirit) & IsTargetTeammate6CatchAble(component)) && !component.isUnitDead && !list.Contains(component) && num < FusionData.CurrentFusionLevel + 1 && !targetTeammateIsUnderSomebodyControl(component))
			{
				HoldingTeammateData holdingTeammateData = new HoldingTeammateData();
				holdingTeammateData.SoulBombDamage = Mathf.CeilToInt(component.unitCfg.maxHP * SoulBombHPToDamageRatio * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio);
				holdingTeammateData.BombPpt = component;
				holdingTeammateData.BombObject = component.gameObject;
				holdingTeammateData.targetCannonScript = CannonControllerList[num];
				holdingTeammateData.BombOutlookObject = SpawnTeammateBall(component, component.UnitBas.SummonerSpellBase.ColorType);
				component.UnitBas.SummonerSpellBase.SpellSummonHPFixDropAmount = 0f;
				holdingBombDataList.Add(holdingTeammateData);
				component.RemoveFromOwnerList();
				list.Add(component);
				AddNewHoldingTeammate(component);
				num++;
				if (holdingBombDataList.Count > FusionData.CurrentFusionLevel)
				{
					return;
				}
			}
		}
		foreach (UnitProperty @lock in lockList)
		{
			if (!@lock.isUnitDead && !list.Contains(@lock) && !targetTeammateIsUnderSomebodyControl(@lock))
			{
				HoldingTeammateData holdingTeammateData2 = new HoldingTeammateData();
				holdingTeammateData2.SoulBombDamage = Mathf.CeilToInt(@lock.unitCfg.maxHP * SoulBombHPToDamageRatio * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio);
				holdingTeammateData2.BombPpt = @lock;
				holdingTeammateData2.BombObject = @lock.gameObject;
				@lock.UnitBas.SummonerSpellBase.SpellSummonHPFixDropAmount = 0f;
				holdingBombDataList.Add(holdingTeammateData2);
				holdingTeammateData2.targetCannonScript = CannonControllerList[num];
				list.Add(@lock);
				AddNewHoldingTeammate(@lock);
				num++;
				if (holdingBombDataList.Count > FusionData.CurrentFusionLevel)
				{
					break;
				}
			}
		}
	}

	private void UpdateHoldingTeammateState()
	{
		for (int i = 0; i < holdingBombDataList.Count; i++)
		{
			HoldingTeammateData holdingTeammateData = holdingBombDataList[i];
			UnitProperty nearestTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(holdingTeammateData.BombObject.transform.position);
			if (holdingTeammateData.BombPpt.isUnitDead || !holdingTeammateData.BombObject.activeInHierarchy || holdingTeammateData.duration <= 0f || (holdingTeammateData.state == Teammate6BombState.Shooting && nearestTargetablePpt != null && Tool2D.IgnoreZDistanceSqr(nearestTargetablePpt.transform, holdingTeammateData.BombObject.transform) <= soulBombExplosionRadiu * soulBombExplosionRadiu * 0.4f * 0.4f))
			{
				if (holdingTeammateData.state == Teammate6BombState.Shooting)
				{
					SpawnExplosion(holdingTeammateData);
					if ((bool)holdingTeammateData.BombTrailGameObject)
					{
						ObjPoolMgr.Inst.RecycleGO(holdingTeammateData.BombTrailGameObject, 0.6f);
					}
				}
				ObjPoolMgr.Inst.RecycleGO(holdingTeammateData.BombOutlookObject);
				holdingBombDataList.Remove(holdingTeammateData);
				holdingTeammateData.BombPpt.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
				{
					isInstanceDeath = false
				});
				continue;
			}
			UnitBase unitBas = holdingTeammateData.BombPpt.UnitBas;
			switch (holdingTeammateData.state)
			{
			case Teammate6BombState.QuickReloading:
				UpdateQuickReloadingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Hook_Backing:
				UpdateHookBackingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Holding_BackUpAmmo:
				UpdateBackUpAmmoHoldingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Holding_Barrel:
				UpdateBarrelHoldingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Shooting:
				if (((Teammate)unitBas).beingControlledByTeammate6)
				{
					holdingTeammateData.duration -= Time.deltaTime;
					UpdateShootingTeammateRotateionEffect(holdingTeammateData);
					UpdateShootingTeammateState(holdingTeammateData);
					UpdateShootingTeammateTrailState(holdingTeammateData);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void UpdateQuickReloadingTeammateState(HoldingTeammateData data)
	{
		UnitProperty bombPpt = data.BombPpt;
		UnitBase unitBas = bombPpt.UnitBas;
		if (unitBas is Teammate1 || unitBas is Teammate1FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate2 || unitBas is Teammate2FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate3)
		{
			bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position + new Vector3(-0.5f * ModelTransform.localScale.x, 0.3f, -0.25f));
		}
		else if (unitBas is Teammate3FuseController)
		{
			bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate4)
		{
			Transform transform = bombPpt.transform.Find("Layer");
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position + new Vector3(-0.3f * ModelTransform.localScale.x, 0.3f * bombPpt.transform.localScale.x, 0f));
		}
		else if (unitBas is Teammate4FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate5 || unitBas is Teammate5FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate6)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		else if (unitBas is Teammate7)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(QuickReloadHandTransform.position);
		}
		if (data.BombOutlookObject != null)
		{
			data.BombOutlookObject.transform.position = bombPpt.transform.position;
		}
	}

	private void UpdateHookBackingTeammateState(HoldingTeammateData data)
	{
		if ((bool)data.TargetHookTransform && data.TargetHookTransform.gameObject.activeInHierarchy)
		{
			UnitProperty bombPpt = data.BombPpt;
			UnitBase unitBas = bombPpt.UnitBas;
			Transform targetHookTransform = data.TargetHookTransform;
			if (unitBas is Teammate1 || unitBas is Teammate1FuseController)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate2 || unitBas is Teammate2FuseController)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate3)
			{
				bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position + new Vector3(-0.5f * ModelTransform.localScale.x, 0.3f, -0.25f));
			}
			else if (unitBas is Teammate3FuseController)
			{
				bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate4)
			{
				Transform transform = bombPpt.transform.Find("Layer");
				transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position + new Vector3(-0.3f * ModelTransform.localScale.x, 0.3f * bombPpt.transform.localScale.x, 0f));
			}
			else if (unitBas is Teammate4FuseController)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate5 || unitBas is Teammate5FuseController)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate6)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			else if (unitBas is Teammate7)
			{
				bombPpt.transform.position = Tool2D.IgnoreZPoint(targetHookTransform.position);
			}
			if (data.BombOutlookObject != null)
			{
				data.BombOutlookObject.transform.position = bombPpt.transform.position;
			}
		}
	}

	private void UpdateShootingTeammateRotateionEffect(HoldingTeammateData data)
	{
		UnitProperty bombPpt = data.BombPpt;
		UnitBase unitBas = bombPpt.UnitBas;
		SpellSpecialMovementType currentSpellMovement = base.SummonerSpellBase.currentSpellMovement;
		if ((uint)currentSpellMovement <= 4u)
		{
			if (unitBas is Teammate3 || unitBas is Teammate4)
			{
				bombPpt.transform.right = Tool2D.GetDir(data.direction.normalized, -90f);
			}
			return;
		}
		throw new ArgumentOutOfRangeException();
	}

	private void UpdateShootingTeammateTrailState(HoldingTeammateData data)
	{
		if ((bool)data.BombTrailGameObject)
		{
			data.BombTrailGameObject.transform.position = data.BombObject.transform.position;
		}
	}

	private void UpdateShootingTeammateState(HoldingTeammateData data)
	{
		SpellSpecialMovementType spellSpecialMovementType = base.SummonerSpellBase.currentSpellMovement;
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation && base.SummonerSpellBase.isOwnerSpellEnd)
		{
			spellSpecialMovementType = SpellSpecialMovementType.Normal;
		}
		switch (spellSpecialMovementType)
		{
		case SpellSpecialMovementType.Normal:
			data.BombObject.transform.position += data.direction * flyingBombSpeed * Time.deltaTime;
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			if (data.chasingTarget != null && data.chasingTarget.isActiveAndEnabled)
			{
				data.direction = Tool2D.DirMoveTowards(data.direction, Tool2D.IgnoreZV2ToV1(data.chasingTarget.transform.position, data.BombObject.transform.position), flyingBombSpeed * base.SummonerSpellBase.spellFollowTargetRotateSpeed * Time.deltaTime).normalized;
				data.BombObject.transform.position += data.direction * flyingBombSpeed * Time.deltaTime;
			}
			else
			{
				data.chasingTarget = LevelMgr.Inst.CurrentRoomCtrller.GetRandomTargetablePpt();
			}
			break;
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
			data.direction = Vector3.Lerp(data.direction, Tool2D.IgnoreZV2ToV1(mousePoint, data.BombObject.transform.position).normalized, flyingBombSpeed * Time.deltaTime * base.SummonerSpellBase.spellFollowMouseLerp);
			data.BombObject.transform.position += data.direction * flyingBombSpeed * Time.deltaTime;
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / flyingBombSpeed) * Time.deltaTime;
			data.currentRotationAngle += num;
			data.direction = Tool2D.GetDir(data.currentRotationAngle + 90f);
			Vector3 v = base.transform.position + Tool2D.GetDir(data.currentRotationAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
			data.BombObject.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
			data.direction = Tool2D.DirMoveTowards(data.direction, Tool2D.IgnoreZV2ToV1(base.transform.position, data.BombObject.transform.position), flyingBombSpeed * base.SummonerSpellBase.spellFollowTargetRotateSpeed * Time.deltaTime).normalized;
			data.BombObject.transform.position += data.direction * flyingBombSpeed * Time.deltaTime;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (data.BombOutlookObject != null)
		{
			data.BombOutlookObject.transform.position = data.BombObject.transform.position;
		}
	}

	private void UpdateBackUpAmmoHoldingTeammateState(HoldingTeammateData data)
	{
		UnitProperty bombPpt = data.BombPpt;
		bombPpt.transform.position = data.targetCannonScript.BackUpTeammateBombTransform.position;
		bombPpt.transform.right = data.targetCannonScript.BackUpTeammateBombTransform.right;
		data.BombOutlookObject.transform.position = bombPpt.transform.position;
		data.BombOutlookObject.transform.right = bombPpt.transform.right;
	}

	private void UpdateBarrelHoldingTeammateState(HoldingTeammateData data)
	{
		UnitProperty bombPpt = data.BombPpt;
		UnitBase unitBas = bombPpt.UnitBas;
		Transform teammateBombPosition = data.targetCannonScript.TeammateBombPosition;
		if (data.BombOutlookObject != null)
		{
			data.BombOutlookObject.transform.position = data.targetCannonScript.TeammateBombPosition.position;
			data.BombOutlookObject.transform.right = data.targetCannonScript.transform.right;
			float num = Mathf.Abs(data.BombOutlookObject.transform.localScale.x);
			data.BombOutlookObject.transform.localScale = ((ModelTransform.localScale.x == 1f) ? (Vector3.one * num) : (new Vector3(-1f, 1f, 1f) * num));
		}
		if (unitBas is Teammate1 || unitBas is Teammate1FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate2 || unitBas is Teammate2FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate3)
		{
			bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position + new Vector3(-0.5f * ModelTransform.localScale.x, 0.3f, -0.25f));
		}
		else if (unitBas is Teammate3FuseController)
		{
			bombPpt.transform.localScale = new Vector3(bombPpt.transform.localScale.x, bombPpt.transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate4)
		{
			Transform transform = bombPpt.transform.Find("Layer");
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position + new Vector3(-0.3f * ModelTransform.localScale.x, 0.3f * bombPpt.transform.localScale.x, 0f));
		}
		else if (unitBas is Teammate4FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate5 || unitBas is Teammate5FuseController)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate6)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
		else if (unitBas is Teammate7)
		{
			bombPpt.transform.position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
		}
	}

	private void CreateShooteFireEffect(Vector3 spawnPosition, Vector3 direction)
	{
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_CannonFire_" + base.SummonerSpellBase.ColorType, spawnPosition, 0.6f);
		gO.transform.right = direction;
		gO.transform.localScale = Vector3.one * base.transform.localScale.x;
	}

	private void CreateMeleeAttackEffect(Vector3 spawnPosition, float range)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_MeleeAttack_" + base.SummonerSpellBase.ColorType, spawnPosition, 2f).transform.localScale = Vector3.one * range;
	}

	private void CreateBombExplosionEffect(Vector3 spawnPosition, float range)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_SoulBomb_" + base.SummonerSpellBase.ColorType, spawnPosition, 2f).transform.localScale = Vector3.one * range;
	}

	private GameObject CreateBombTrailEffect(Vector3 spawnPosition)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20061 + "/" + 20061 + "_BombTrail_" + base.SummonerSpellBase.ColorType, spawnPosition);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "CloseAttack":
			CloseRangeAttack();
			SpawnCloseAttackScreenShake();
			break;
		case "CloseAttackEnd":
			CheckNearestTeammate();
			if ((bool)nearestTeammate || !targetPpt || !targetPpt.gameObject.activeInHierarchy || Tool2D.IgnoreZDistanceSqr(base.transform.position, targetPpt.transform.position) > closeRangeAttackRange * closeRangeAttackRange * 0.5f)
			{
				base.Anima.SetTrigger("Idle");
				ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
				ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
			}
			acting = false;
			break;
		case "BarrelLockTarget":
			barrelLockingTarget = true;
			break;
		case "ShootBomb":
			barrelLockingTarget = false;
			{
				foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
				{
					if (holdingBombData.state == Teammate6BombState.Holding_Barrel)
					{
						holdingBombData.state = Teammate6BombState.Shooting;
						holdingBombData.BombTargetEndPosition = bombShootPosition;
						holdingBombData.currentRotationAngle = UnityEngine.Random.Range(0, 360);
						holdingBombData.chasingTarget = targetPpt;
						Vector3 position = holdingBombData.BombObject.transform.position;
						position = Tool2D.IgnoreZPoint(holdingBombData.targetCannonScript.ShootPosition);
						holdingBombData.BombObject.transform.position = position;
						holdingBombData.direction = Tool2D.IgnoreZV2ToV1(bombShootPosition, position).normalized;
						holdingBombData.duration = 3f + base.SummonerSpellBase.bonusDuration;
						CreateShooteFireEffect(holdingBombData.targetCannonScript.ShootPosition.position, holdingBombData.targetCannonScript.transform.right);
						holdingBombData.BombTrailGameObject = CreateBombTrailEffect(position);
					}
				}
				break;
			}
		case "ShootEnd":
			acting = false;
			soulBombRefillTimer = soulBombRefillInterval - 0.1f;
			if ((bool)GetNearestTeammate())
			{
				currentState = Teammate6State.SeekingAmmo;
			}
			else
			{
				currentState = Teammate6State.Move;
			}
			base.Anima.SetTrigger("Idle");
			ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
			ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
			break;
		case "ThrowHook":
			if (!base.beingControlledByTeammate6)
			{
				ThrowHook();
			}
			break;
		case "HookCatchTarget":
			if (!base.beingControlledByTeammate6)
			{
				HookCatchTargets();
			}
			break;
		case "HookBackToOwner":
			foreach (var hookTeammateData in HookTeammateDataList)
			{
				ObjPoolMgr.Inst.RecycleGO(hookTeammateData.Hook);
				if (!hookTeammateData.Target.isUnitDead && hookTeammateData.Target.gameObject.activeInHierarchy && hookTeammateData.data != null)
				{
					hookTeammateData.data.state = Teammate6BombState.Holding_BackUpAmmo;
				}
			}
			HookTeammateDataList.Clear();
			lockList.Clear();
			acting = false;
			if (holdingBombDataList.Count > 0)
			{
				currentState = Teammate6State.QuickReload;
			}
			break;
		case "QuickReloadFinish":
			acting = false;
			if (holdingBombDataList.Count > 0)
			{
				currentState = Teammate6State.ReadyToShootFindingTarget;
			}
			break;
		case "CatchTeammate":
			if (!base.beingControlledByTeammate6)
			{
				CathchTargetTeammateInRange();
			}
			break;
		case "CatchEnd":
			lockList.Clear();
			acting = false;
			if (holdingBombDataList.Count > 0)
			{
				currentState = Teammate6State.ReadyToShootFindingTarget;
			}
			break;
		default:
			Debug.LogError(animaName);
			break;
		case "LoadCannon":
			break;
		case "QuickReloadReady":
			break;
		}
	}

	private void SpawnBombExplosionScreenShake(Vector3 explosionPos)
	{
		ShockParam bombExplosionShock = BombExplosionShock;
		Vector3 normalized = (explosionPos - PlayerMgr.Inst.PlayerPpt.transform.position).normalized;
		CamController.Inst.SetShock(bombExplosionShock, normalized);
	}

	private void SpawnCloseAttackScreenShake()
	{
		ShockParam shockParam = closeAttackShock;
		float num = Mathf.Clamp((float)base.SummonerSpellBase.ShootData.Spell.specialInt / 100f, 0f, 5f);
		shockParam.radius *= 1f + num;
		shockParam.speed *= 1f + num * 0.4f;
		shockParam.time *= 1f + num * 0.2f;
		CamController.Inst.SetShock(shockParam, new Vector3(0f, -1f, 0f));
	}

	private void OnDisable()
	{
		base.Anima.SetTrigger("Idle");
		ChangeSpineAnimationState(base.SAnima, "Idle", isLoop: true);
		ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
		FreeAllHoldingTeammate();
		foreach (var hookTeammateData in HookTeammateDataList)
		{
			ObjPoolMgr.Inst.RecycleGO(hookTeammateData.Hook);
		}
	}

	private void FreeAllHoldingTeammate()
	{
		for (int i = 0; i < holdingBombDataList.Count; i++)
		{
			HoldingTeammateData holdingTeammateData = holdingBombDataList[i];
			SpawnExplosion(holdingTeammateData);
			holdingTeammateData.BombPpt.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
			{
				isInstanceDeath = false
			});
			ObjPoolMgr.Inst.RecycleGO(holdingTeammateData.BombOutlookObject);
		}
		holdingBombDataList.Clear();
	}

	public override void SummonsThrough()
	{
		base.SummonsThrough();
		SummonFollowOwnerThroughMap();
		moving = false;
		acting = false;
		foreach (HoldingTeammateData holdingBombData in holdingBombDataList)
		{
			if (holdingBombData != null && holdingBombData.BombOutlookObject != null && !holdingBombData.BombOutlookObject.IsDestroyed())
			{
				holdingBombData.BombOutlookObject.SetActive(value: true);
			}
		}
		currentState = Teammate6State.Idle;
	}
}
