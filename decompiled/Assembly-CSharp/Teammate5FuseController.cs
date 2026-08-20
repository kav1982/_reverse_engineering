using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate5FuseController : Teammate, Spell1009BackMP.ICanManaBack
{
	public enum T5ShootPointPos
	{
		middle,
		left,
		right
	}

	private enum MonsterState
	{
		BornSpellSelectPhase,
		Idle,
		BattleLockTarget,
		CastSpell
	}

	public Shadow selfShadow;

	public UnitProperty selfPpt;

	public float recheckInterval;

	private SpellShootGroup subShootGroup;

	private UnitProperty Owner;

	public float followOnwerDistance;

	private float manaGenerateSpeedRatio = 1f;

	private float spellManaCost;

	private float attackDistance;

	public float inDangerFallbackSpeedRatio;

	private bool isRotation;

	private float MinRecoil;

	private bool castingSpell;

	private bool castingLongliveSpell;

	private float castingSkillRemainingTimer;

	private bool Looping;

	private bool OpeningBook;

	public float lowCostCloseBookDelayTime;

	private float lowCostCloseBookDelayTimer;

	public float idleCheckEnemyInRangeTime;

	private float idleCheckEnemyInRangeTimer;

	public float idleFollowOwnerTimeCheck;

	private float idleFollowOwnerCheckTimer;

	public float floatShiftHeight;

	public float floatBaseHeight;

	public float heightFloatSpeed;

	public float heightShiftDownScale;

	private float floatHeightCounter;

	[HideInInspector]
	public Vector3 lastFrameDirection;

	private float moveSpeedRatio = 1f;

	private bool haveShootableSpell;

	public SpriteRenderer progressBar;

	public float showProcessMinMPRecoverRatio;

	private bool lowCostSpell;

	private bool castingLowCostSpell;

	private AnimatorClipInfo[] animatorinfo;

	public Transform shootPosition;

	public float lifeLineShiftHeight;

	private MonsterState state;

	private float recheckIntervalTimer;

	private bool setOnce;

	public GameObject visualObject;

	private bool attackEnding;

	private List<Spell1005PreFirework> _preFireworks;

	private List<Spell1015ArcaneNova> _arcaneNovas;

	public GameObject glowFaint;

	private float shootRecoil;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private const float minTeleportCd = 0.6f;

	public VariableFloat teleportPositionDistanceRatio;

	public float teleportTime;

	public Transform teleTrans;

	public AnimationCurve TeleportInCurve;

	public AnimationCurve TeleportOutCurve;

	private float teleportTimer;

	private bool isEssenceAttack;

	private int essenceShootCount;

	private int essenceShootIndex;

	private int essenceShootRequirementShootTime;

	private int essenceTeleportRequirementShootTime;

	private int shootCounter;

	private float afterteleportDelayTimer;

	private bool enableTp = true;

	private List<Teammate5FuseBody> bodyList = new List<Teammate5FuseBody>();

	private Vector3 fuseShootPosition = Vector3.zero;

	public Transform summonTransfrom;

	public RuntimeAnimatorController RBook;

	public float bookDistance;

	public CapsuleCollider bookCollider;

	public float cooliderWidthPerBook;

	public float hitBoxBaseRadiu;

	public new Animator Anima;

	public GameObject fireEffect;

	public SpriteRenderer sr;

	public SpriteRenderer srVoid;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public float WidthRatioPerBook;

	public float currentMana { get; private set; }

	public float manaGenerateSpeedPerSecond { get; set; }

	public List<(SpellBase spellBase, T5ShootPointPos shootPositionMode)> currentCastingSpell { get; set; } = new List<(SpellBase, T5ShootPointPos)>();


	private bool shootleft { get; set; }

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		haveShootableSpell = false;
		attackEnding = false;
		setOnce = false;
		state = MonsterState.BornSpellSelectPhase;
		recheckIntervalTimer = 0f;
		lowCostSpell = false;
		castingLowCostSpell = false;
		floatHeightCounter = 0f;
		lastFrameDirection = Vector3.zero;
		moveSpeedRatio = 1f;
		Owner = null;
		currentMana = 0f;
		lowCostCloseBookDelayTimer = 0f;
		manaGenerateSpeedPerSecond = 0f;
		manaGenerateSpeedRatio = 1f;
		spellManaCost = 0f;
		attackDistance = 0f;
		isRotation = false;
		MinRecoil = float.NegativeInfinity;
		castingSpell = false;
		castingLongliveSpell = false;
		castingSkillRemainingTimer = 0f;
		currentCastingSpell = new List<(SpellBase, T5ShootPointPos)>();
		Looping = false;
		OpeningBook = false;
		idleCheckEnemyInRangeTimer = 0f;
		idleFollowOwnerCheckTimer = 0f;
		progressBar.gameObject.SetActive(value: false);
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		selfShadow.ShadowGO.SetActive(value: true);
		glowFaint.SetActive(value: true);
		navAreaMask = 32;
		shootRecoil = 0f;
		teleportTimer = 0.35f;
		teleTrans.localScale = Vector3.one;
		isEssenceAttack = false;
		essenceShootCount = 0;
		essenceShootIndex = 0;
		essenceShootRequirementShootTime = 9;
		essenceTeleportRequirementShootTime = 9;
		shootCounter = 0;
		afterteleportDelayTimer = 0f;
		enableTp = true;
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		SetColor(base.SummonerSpellBase.ColorType);
		SetBookNum(FusionData.CurrentFusionLevel + 1);
		SpawnBody();
		bookCollider.height = cooliderWidthPerBook * (float)FusionData.CurrentFusionLevel * myPpt.tsf_Layer.localScale.x;
		bookCollider.radius = hitBoxBaseRadiu * myPpt.tsf_Layer.localScale.x;
		essenceShootCount = (1 + base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 4) * (FusionData.CurrentFusionLevel + 1);
		shootCounter = 0;
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		selfShadow.ShadowGO.SetActive(value: true);
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		ColliderToggle(state: false);
		base.beingControlledByTeammate6 = true;
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.transform.eulerAngles = Vector3.zero;
			base.CanMove = true;
			attackEnding = false;
			state = MonsterState.BornSpellSelectPhase;
			recheckIntervalTimer = 0f;
			castingLowCostSpell = false;
			castingSpell = false;
			castingLongliveSpell = false;
			currentMana = spellManaCost;
			castingSkillRemainingTimer = 0f;
			Looping = false;
			OpeningBook = false;
			glowFaint.SetActive(value: true);
			ShowTeammate();
		}
	}

	public void SetColor(SpellColorType type)
	{
		fireEffect.SetActive(value: false);
		sr.enabled = true;
		srVoid.enabled = false;
		switch (type)
		{
		case SpellColorType.Frozen:
			if (sr.material != mat_ECFrozen)
			{
				sr.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (sr.material != mat_ECMucus)
			{
				sr.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			fireEffect.SetActive(value: true);
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Player:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (sr.material != mat_ECVenom)
			{
				sr.material = mat_ECVenom;
			}
			break;
		case SpellColorType.Void:
			sr.enabled = false;
			srVoid.enabled = true;
			if (srVoid.material != mat_ECVoid)
			{
				srVoid.material = mat_ECVoid;
			}
			srVoid.material.SetInt(UseGhostEffect, 0);
			srVoid.material.SetInt(UseFuseShineEffect, 0);
			srVoid.material.SetFloat(FuseShineProcess, 0f);
			GeneralTool.InitialSpriteMaterial(srVoid);
			break;
		default:
			Debug.LogError(type);
			break;
		}
		sr.material.SetInt(UseGhostEffect, 0);
		sr.material.SetInt(UseFuseShineEffect, 0);
		sr.material.SetFloat(FuseShineProcess, 0f);
		GeneralTool.InitialSpriteMaterial(sr);
	}

	public void SetBookNum(int count)
	{
		SpriteRenderer spriteRenderer = ((base.SummonerSpellBase.ColorType == SpellColorType.Void) ? srVoid : sr);
		spriteRenderer.size = new Vector2(WidthRatioPerBook * (1f + (float)(count - 1) / 2f), spriteRenderer.size.y);
		fireEffect.GetComponent<SpriteRenderer>().size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
	}

	private void SpawnBody()
	{
		string text = "Prefabs/Spell/" + 20051 + "/" + 20051 + "_FuseBodyMain";
		int num = FusionData.CurrentFusionLevel + 1;
		if (bodyList.Count < num)
		{
			int num2 = num - bodyList.Count;
			for (int i = 0; i < num2; i++)
			{
				bodyList.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>(text), base.transform.position, quaternion.identity, summonTransfrom).GetComponent<Teammate5FuseBody>());
			}
		}
		foreach (Teammate5FuseBody body in bodyList)
		{
			body.gameObject.SetActive(value: false);
		}
		Vector3 vector = new Vector3((float)(-FusionData.CurrentFusionLevel) / 2f * bookDistance, 0f, 0f);
		for (int j = 0; j < num; j++)
		{
			Vector3 localPosition = vector + new Vector3(bookDistance * (float)j, 0f, -0.01f * (float)j);
			bodyList[j].gameObject.SetActive(value: true);
			bodyList[j].transform.localPosition = localPosition;
			bodyList[j].SetColor(base.SummonerSpellBase.ColorType);
			bodyList[j].SetBookNum(FusionData.CurrentFusionLevel + 1);
		}
	}

	public override void OnEnterDelayDeathEvent()
	{
		base.OnEnterDelayDeathEvent();
		if (base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f)
		{
			return;
		}
		sr.material.SetInt(UseGhostEffect, 1);
		srVoid.material.SetInt(UseGhostEffect, 1);
		foreach (Teammate5FuseBody body in bodyList)
		{
			body.sr.material.SetInt(UseGhostEffect, 1);
		}
		SummonGhostEffectToggle(state: true);
		ColliderToggle(state: false);
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		if (base.SummonerSpellBase.ColorType != SpellColorType.Void)
		{
			sr.material.SetInt(UseFuseShineEffect, 1);
			sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		else
		{
			srVoid.material.SetInt(UseFuseShineEffect, 1);
			srVoid.material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		fireEffect.SetActive(value: false);
		foreach (Teammate5FuseBody body in bodyList)
		{
			body.sr.material.SetInt(UseFuseShineEffect, 1);
			body.sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
			if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
			{
				body.fireEffect.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
			}
		}
		selfShadow.ShadowGO.SetActive(value: false);
		progressBar.gameObject.SetActive(value: false);
		glowFaint.SetActive(value: false);
	}

	private void TryTeleport()
	{
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && base.SummonerSpellBase.currentSpellMovement != SpellSpecialMovementType.Rotation && GetLaunchGroup() != null)
		{
			teleportTimer -= Time.deltaTime;
			afterteleportDelayTimer -= Time.deltaTime;
			if (!(teleportTimer > 0f) && base.HaveTarget && enableTp)
			{
				StartCoroutine("Teleport", teleportTime);
				teleportTimer = 0.6f;
			}
		}
	}

	public IEnumerator Teleport(float Time)
	{
		enableTp = false;
		afterteleportDelayTimer = Time + 0.2f;
		yield return new WaitForSeconds(0.01f);
		if (base.SummonerSpellBase.ColorType != SpellColorType.Void)
		{
			foreach (Teammate5FuseBody body in bodyList)
			{
				DOTween.Sequence().Append(teleTrans.DOScale(0f, Time).SetEase(TeleportInCurve)).Join(body.sr.material.DOFloat(1f, "_TwirlProcess", Time))
					.Append(teleTrans.DOScale(1f, Time).SetEase(TeleportOutCurve))
					.Join(body.sr.material.DOFloat(0f, "_TwirlProcess", Time));
			}
		}
		base.SummonerSpellBase.GetEffect("TeleportIn", Tool2D.IgnoreZPoint(base.transform.position + new Vector3(0f, 0.4f, 0f)), 0.5f);
		yield return new WaitForSeconds(Time);
		teleportTimer = 0.6f;
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint + Tool2D.GetDir() * attackDistance * teleportPositionDistanceRatio.RandomResult());
		base.SummonerSpellBase.GetEffect("TeleportIn", Tool2D.IgnoreZPoint(base.transform.position + new Vector3(0f, 0.4f, 0f)), 0.5f);
		GetNearestTarget();
	}

	public override void Update()
	{
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation && base.CanMove)
		{
			float angleOffset = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / GetSummonUnitRealMoveSpeed()) * Time.deltaTime;
			base.SummonerSpellBase.SpellAroundPlayerUpdateMoveTrigger(angleOffset);
			Vector3 v = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		}
		if (!setOnce)
		{
			setOnce = true;
			SetAll();
		}
		if (currentMana < spellManaCost)
		{
			currentMana += manaGenerateSpeedPerSecond * manaGenerateSpeedRatio * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio * Time.deltaTime;
		}
		TryTeleport();
		if (Looping && !attackEnding && castingLowCostSpell && !CheckSummonIsLimitReached())
		{
			GetNearestTarget();
			if (targetPpt != null && base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
			{
				lowCostCloseBookDelayTimer = 0f;
				if (afterteleportDelayTimer <= 0f && currentMana >= spellManaCost && castingSkillRemainingTimer <= 0f)
				{
					CastSpell();
					currentMana -= spellManaCost;
				}
			}
			else
			{
				lowCostCloseBookDelayTimer += Time.deltaTime;
				if (lowCostCloseBookDelayTimer >= lowCostCloseBookDelayTime)
				{
					Anima.SetTrigger("AttackEnd");
					Looping = false;
					attackEnding = true;
					lowCostCloseBookDelayTimer = 0f;
				}
			}
		}
		if (spellManaCost > manaGenerateSpeedPerSecond * showProcessMinMPRecoverRatio * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio)
		{
			progressBar.material.SetFloat("_progress", 0.365f - currentMana / spellManaCost * 0.365f * 2f);
		}
		if (castingLongliveSpell && castingSkillRemainingTimer > 0f && Looping)
		{
			castingSkillRemainingTimer -= Time.deltaTime;
			if (castingSkillRemainingTimer < 0f)
			{
				Looping = false;
				Anima.SetTrigger("AttackEnd");
			}
		}
		if (!castingSpell && !castingLongliveSpell)
		{
			currentCastingSpell.Clear();
		}
		floatHeightCounter += Time.deltaTime * heightFloatSpeed;
		SummonsTouchMonster();
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornSpellSelectPhase:
			SetMove(Vector3.zero);
			state = MonsterState.Idle;
			Anima.SetTrigger("Idle");
			Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
			break;
		case MonsterState.Idle:
			idleCheckEnemyInRangeTimer += Time.deltaTime;
			if (idleCheckEnemyInRangeTimer >= idleCheckEnemyInRangeTime)
			{
				idleCheckEnemyInRangeTimer -= idleCheckEnemyInRangeTime;
				CheckTarget();
			}
			else
			{
				if (state == MonsterState.BattleLockTarget)
				{
					break;
				}
				idleFollowOwnerCheckTimer += Time.deltaTime;
				if (idleFollowOwnerCheckTimer >= idleFollowOwnerTimeCheck)
				{
					idleFollowOwnerCheckTimer -= idleFollowOwnerTimeCheck;
					if (Owner != null && Vector3.Distance(Owner.transform.position, base.transform.position) > followOnwerDistance * base.SummonerSpellBase.SpellSummonMoveRatio)
					{
						GetNavInfo(Owner.transform.position + Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere + Vector3.one) * moveSpeedRatio * base.SummonerSpellBase.SpellSummonMoveRatio);
						SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed());
					}
					else
					{
						SetMove(Vector3.zero, isFlip: false);
					}
				}
			}
			break;
		case MonsterState.BattleLockTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				idleFollowOwnerCheckTimer += Time.deltaTime;
				if (idleFollowOwnerCheckTimer >= idleFollowOwnerTimeCheck)
				{
					idleFollowOwnerCheckTimer -= idleFollowOwnerTimeCheck;
					state = MonsterState.Idle;
				}
				break;
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval)
			{
				recheckIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (!base.HaveTarget && !(targetPpt != null))
			{
				break;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				idleFollowOwnerCheckTimer = 0f;
				break;
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval)
			{
				recheckIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (!base.HaveTarget || !haveShootableSpell || CheckSummonIsLimitReached())
			{
				break;
			}
			if (isRotation && ToTargetDistanceSqr() < attackDistance * attackDistance - 1f && base.SummonerSpellBase.spellAroundOwnerRadius <= 0f)
			{
				GetNavInfo(base.transform.position + (base.transform.position - base.TargetPoint));
				SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed() * moveSpeedRatio * inDangerFallbackSpeedRatio);
			}
			else if (ToTargetDistanceSqr() < attackDistance * attackDistance)
			{
				SetMove(Vector3.zero, isFlip: false);
				animatorinfo = Anima.GetCurrentAnimatorClipInfo(0);
				if (base.gameObject != null && base.gameObject.activeInHierarchy && OpeningBook && animatorinfo[0].clip.name == "Idle")
				{
					Anima.SetTrigger("AttackStart");
				}
				if (!(currentMana >= spellManaCost) || castingLongliveSpell || castingSpell || !(castingSkillRemainingTimer <= 0f) || Looping || attackEnding)
				{
					break;
				}
				if (!lowCostSpell || !castingLowCostSpell)
				{
					Anima.SetTrigger("AttackStart");
					currentMana -= spellManaCost;
					OpeningBook = true;
					Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
					if (lowCostSpell && SpellGroupShootDurationCalculator.GetMaxCastDuration(GetLaunchGroup(), base.SummonerSpellBase.shooterWand) <= 0f)
					{
						castingLowCostSpell = true;
					}
				}
				lastFrameDirection = ToTargetDir();
				castingSpell = true;
				currentMana = 0f;
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * GetSummonUnitRealMoveSpeed() * moveSpeedRatio);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.CastSpell:
			break;
		}
	}

	private void LateUpdate()
	{
		Vector3 vector = base.transform.position + GetFloatingHeight();
		selfPpt.tsf_Layer.position = Tool2D.GetLayerPoint(vector);
		myPpt.bodyCenterPoint = vector + new Vector3(0f, lifeLineShiftHeight, 0f);
		shootPosition.localPosition = GetBookFloatingHeight();
		ShadowSizeShift();
		progressBar.flipX = false;
		UpdateCurrentCastingSpell();
		shootPosition.transform.right = ((targetPpt != null) ? ToTargetDir() : lastFrameDirection);
	}

	private void UpdateCurrentCastingSpell()
	{
		if (currentCastingSpell.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < currentCastingSpell.Count; i++)
		{
			if (currentCastingSpell[i].spellBase.spellCfg.abilityType == SpellAbilityType.DisintegrationRay)
			{
				if (targetPpt != null)
				{
					currentCastingSpell[i].spellBase.GetComponent<Spell1011DisintegrationRay>().SetLaserOwnerData(fuseShootPosition, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position) - new Vector3(0f, 0f, -0.3f));
					lastFrameDirection = ToTargetDir();
				}
				else
				{
					currentCastingSpell[i].spellBase.GetComponent<Spell1011DisintegrationRay>().SetLaserOwnerData(fuseShootPosition + new Vector3(0f, 0f, -0.3f), lastFrameDirection);
				}
			}
			else if (currentCastingSpell[i].spellBase.spellCfg.abilityType == SpellAbilityType.HighPressureWasher)
			{
				if (targetPpt != null)
				{
					currentCastingSpell[i].spellBase.GetComponent<Spell1019HighPressureWasherRemaster>().SetLaserOwnerData(fuseShootPosition, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position) - new Vector3(0f, 0.15f, 0f));
					currentCastingSpell[i].spellBase.GetComponent<Spell1019HighPressureWasherRemaster>().shootPosPositionShift = new Vector3(0f, 0.02f, 0f);
					lastFrameDirection = ToTargetDir();
				}
				else
				{
					currentCastingSpell[i].spellBase.GetComponent<Spell1019HighPressureWasherRemaster>().SetLaserOwnerData(fuseShootPosition, lastFrameDirection);
					currentCastingSpell[i].spellBase.GetComponent<Spell1019HighPressureWasherRemaster>().shootPosPositionShift = new Vector3(0f, 0.02f, 0f);
				}
			}
			else if (currentCastingSpell[i].spellBase is Spell1016Dash spell1016Dash && base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				Vector3 v = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
				base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
				spell1016Dash.independentRotateFollowPoint = Tool2D.IgnoreZPoint(v, base.transform.position.z) + new Vector3(0f, 0.3f, 0f);
			}
		}
	}

	private void CastSpell()
	{
		if (base.beingControlledByTeammate6)
		{
			return;
		}
		GetNearestTarget();
		foreach (Teammate5FuseBody body in bodyList)
		{
			Vector3 pos = Tool2D.IgnoreZPoint(body.transform.position) + GetFloatingHeight() + new Vector3(0f, 0f, -0.1f);
			CastSpellAtTargetPoint(pos);
		}
		if (!isEssenceAttack)
		{
			shootCounter++;
			if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && shootCounter >= essenceShootRequirementShootTime)
			{
				isEssenceAttack = true;
			}
			if (!enableTp && shootCounter >= essenceTeleportRequirementShootTime)
			{
				afterteleportDelayTimer = 0.5f;
				enableTp = true;
			}
		}
		else
		{
			isEssenceAttack = false;
			shootCounter -= essenceShootRequirementShootTime;
		}
		essenceShootIndex = 0;
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "BookOpenAndReadyToAttack":
			OpeningBook = false;
			CastSpell();
			Looping = true;
			Anima.SetTrigger("Attack_Loop");
			break;
		case "CheckIfValidToEndAttack":
			if (!(castingSkillRemainingTimer > 0f) && !lowCostSpell)
			{
				Anima.SetTrigger("AttackEnd");
				Looping = false;
			}
			break;
		case "AttackEndAndBackToNormal":
			AttackEndAndBookClose();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void SummonsThrough()
	{
		if (SummonMayThroughMap())
		{
			SummonFollowOwnerThroughMap();
			return;
		}
		base.SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount = 0;
		base.SummonsThrough();
		base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = 0f;
		myPpt.ClearVoidState();
		myPpt.AnnouncedDeath(new TakeDamageInfo
		{
			isPlayDeadSE = false,
			isCreateDeadEF = false,
			isTeammateThrough = true
		});
	}

	private void CheckTarget()
	{
		GetNearestTarget();
		if (targetPpt != null)
		{
			state = MonsterState.BattleLockTarget;
		}
	}

	private void AttackEndAndBookClose()
	{
		castingSkillRemainingTimer = 0f;
		castingSpell = false;
		if (castingLongliveSpell)
		{
			moveSpeedRatio /= 0.6f;
		}
		castingLongliveSpell = false;
		castingLowCostSpell = false;
		Anima.SetTrigger("Idle");
		foreach (Teammate5FuseBody body in bodyList)
		{
			body.Anima.SetTrigger("Idle");
		}
		attackEnding = false;
		Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
	}

	public void SetMPRegenSpeed(float speed)
	{
		manaGenerateSpeedPerSecond = speed;
	}

	public float GetMPRegenSpeed()
	{
		return manaGenerateSpeedPerSecond;
	}

	public override SpellShootGroup GetLaunchGroup()
	{
		return subShootGroup;
	}

	public override ShootSpellSpatialInfo GetLaunchSpellSpatialInfo()
	{
		Vector3 start = fuseShootPosition;
		Vector3 launchSpellTargetPosition = GetLaunchSpellTargetPosition();
		launchSpellTargetPosition.z = start.z;
		return ShootSpellSpatialInfo.ToPoint(start, launchSpellTargetPosition);
	}

	public Transform GetShootPositionTranform()
	{
		return shootPosition;
	}

	private void CastSpellAtTargetPoint(Vector3 pos)
	{
		if (targetPpt != null)
		{
			lastFrameDirection = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint + new Vector3(0f, 0f - floatBaseHeight, 0f), base.transform.position);
		}
		if (base.SummonerSpellBase.wandChargeData != null)
		{
			WandPostSlotTrigger.PostSlotCastSpellTriggerCheck(WandPostSlotTrigger.GetTargetShooterWandConfigDataFromSPellBase(base.SummonerSpellBase));
		}
		fuseShootPosition = pos;
		int num = ((!isEssenceAttack) ? 1 : essenceShootCount);
		for (int i = 0; i < num; i++)
		{
			SpellBase[] array = Launch().ToArray();
			foreach (SpellBase item in array)
			{
				currentCastingSpell.Add((item, T5ShootPointPos.middle));
			}
			essenceShootIndex++;
		}
		float maxCastDuration = SpellGroupShootDurationCalculator.GetMaxCastDuration(GetLaunchGroup(), base.SummonerSpellBase.shooterWand);
		if (maxCastDuration > 0f)
		{
			if (!castingLongliveSpell)
			{
				moveSpeedRatio *= 0.6f;
			}
			castingLongliveSpell = true;
			castingSkillRemainingTimer = Mathf.Max(castingSkillRemainingTimer, maxCastDuration);
		}
		myPpt.TakeKnockback(Tool2D.IgnoreZPoint(lastFrameDirection) * -1f * UnityEngine.Random.Range(0.9f, 1.1f) * shootRecoil);
		if (!castingLongliveSpell)
		{
			state = MonsterState.BattleLockTarget;
		}
	}

	private Vector3 GetBookFloatingHeight()
	{
		Vector3 zero = Vector3.zero;
		if (!castingLongliveSpell)
		{
			zero += new Vector3(0f, 0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight);
		}
		else
		{
			zero += new Vector3(0f, 0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight / 2f);
		}
		if (zero.z > 0f)
		{
			zero.z *= heightShiftDownScale;
		}
		zero.z -= floatBaseHeight;
		zero.y += floatBaseHeight;
		return zero;
	}

	private Vector3 GetFloatingHeight()
	{
		Vector3 zero = Vector3.zero;
		if (!base.CanMove)
		{
			return new Vector3(0f, 0f, floatShiftHeight * 0.5f - floatBaseHeight);
		}
		if (!castingLongliveSpell)
		{
			zero += new Vector3(0f, 0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight);
		}
		else
		{
			zero += new Vector3(0f, 0f, Mathf.Sin(floatHeightCounter) * floatShiftHeight / 2f);
		}
		if (zero.z > 0f)
		{
			zero.z *= heightShiftDownScale;
		}
		zero.z -= floatBaseHeight;
		return zero;
	}

	public override Vector3 GetLaunchSpellTargetPosition()
	{
		if ((object)targetPpt == null)
		{
			return base.transform.position;
		}
		GetNearestTarget();
		if (isEssenceAttack)
		{
			float num = Tool2D.IgnoreZDistance(targetPpt.transform.position, base.transform.position);
			float num2 = Mathf.Min(60 * base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level, 360);
			float num3 = num2 / (float)essenceShootCount;
			float degree = (0f - num2) / 2f + (float)essenceShootIndex * num3;
			Vector3 dir = Tool2D.GetDir((targetPpt.transform.position - base.transform.position).normalized, degree);
			return base.transform.position + dir * num;
		}
		return targetPpt.transform.position;
	}

	private void ShadowSizeShift()
	{
		selfShadow.ShadowGO.transform.localScale = Vector3.one * selfShadow.shadowScale * (1f + Mathf.Sin(floatHeightCounter) * 0.15f);
	}

	private void SetAll()
	{
		if (base.SummonerSpellBase.ShootData.SubGroup == null)
		{
			subShootGroup = null;
		}
		else
		{
			subShootGroup = base.SummonerSpellBase.ShootData.SubGroup.Copy(base.SummonerSpellBase.ShootData.SubGroup.OwnerShootData);
			subShootGroup.OwnerShootData = null;
		}
		Owner = PlayerMgr.Inst.PlayerPpt;
		animatorinfo = Anima.GetCurrentAnimatorClipInfo(0);
		if (GetLaunchGroup() != null)
		{
			attackDistance = MathF.Min(float.PositiveInfinity, SpellGroupAttackDistanceCalculator.GetMinAttackDistance(GetLaunchGroup(), base.SummonerSpellBase.shooterWand, getMaxDistance: true));
			isRotation = SpellGroupAttackDistanceCalculator.GetShootGroupMovementType(GetLaunchGroup(), base.SummonerSpellBase.shooterWand) == SpellSpecialMovementType.Rotation;
			if (!isRotation)
			{
				attackDistance *= 0.9f;
			}
			if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				attackDistance = 20f;
			}
			spellManaCost = MathF.Min(float.PositiveInfinity, GetLaunchGroup().GetGroupManaCost(1f));
			currentMana = spellManaCost;
			if (base.SummonerSpellBase.shooterWand != null && base.SummonerSpellBase.shooterWand.WandCfg != null && base.SummonerSpellBase.shooterWand.WandCfg.specialAbility == WandAbility.LowerSummonManaCost)
			{
				spellManaCost *= base.SummonerSpellBase.shooterWand.WandCfg.float1 / 100f;
			}
			shootRecoil = ((MinRecoil >= GetLaunchGroup().GetGroupHighestRecoil()) ? 0f : GetLaunchGroup().GetGroupHighestRecoil());
			haveShootableSpell = GetLaunchGroup().Shoots.Length != 0;
		}
		else
		{
			attackDistance = float.PositiveInfinity;
			spellManaCost = float.PositiveInfinity;
			shootRecoil = 0f;
			haveShootableSpell = false;
		}
		if (haveShootableSpell && spellManaCost >= manaGenerateSpeedPerSecond * showProcessMinMPRecoverRatio * base.SummonerSpellBase.SpellSummonAttackSpeedRatio)
		{
			progressBar.gameObject.SetActive(value: true);
		}
		else
		{
			lowCostSpell = true;
		}
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
	}

	private bool CheckSummonIsLimitReached()
	{
		if (!DataMgr.settingData.AiSummon)
		{
			return false;
		}
		if (subShootGroup == null)
		{
			return false;
		}
		return (from e in subShootGroup.GetAllSlotData()
			select e.GetFinalConfig()).Any(base.MateSummonsIsLimitReached);
	}

	public void ManaDrain(float Amount)
	{
		currentMana -= Amount;
		currentMana = Mathf.Min(currentMana, 0f);
	}

	private void OnDisable()
	{
		if (_preFireworks != null)
		{
			foreach (Spell1005PreFirework item in _preFireworks.Where((Spell1005PreFirework e) => e.gameObject.activeSelf && e.ownerPpt == selfPpt))
			{
				item.ownerPpt = null;
			}
			_preFireworks = null;
		}
		if (_arcaneNovas == null)
		{
			return;
		}
		foreach (Spell1015ArcaneNova item2 in _arcaneNovas.Where((Spell1015ArcaneNova e) => e.gameObject.activeSelf && e.ownerPpt == selfPpt))
		{
			item2.ownerPpt = null;
		}
		_arcaneNovas = null;
	}

	public void ManaBackFromSpell1009(float mana)
	{
		currentMana += mana;
	}
}
