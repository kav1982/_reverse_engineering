using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Teammate3 : Teammate
{
	public enum UnitState
	{
		Idle,
		Attack
	}

	[Space(50f)]
	public VariableFloat attackInterval;

	public float attackEffectOffset;

	[Header("Color")]
	public SpriteRenderer sr;

	public Sprite sprite_ECFrozen;

	public Sprite sprite_ECMucus;

	public Sprite sprite_ECPlayer;

	public Sprite sprite_ECVenom;

	public Sprite sprite_ECVoid;

	public Sprite sprite2_ECFrozen;

	public Sprite sprite2_ECMucus;

	public Sprite sprite2_ECPlayer;

	public Sprite sprite2_ECVenom;

	public Sprite sprite2_ECVoid;

	public GameObject fireIdleEffect;

	public SpriteRenderer sr_Attack;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public GameObject fireAttackEffect;

	public GameObject normalAttackEffect;

	public GameObject voidAttackEffect;

	public float MaxSplitRadiu;

	public float rootPosShift;

	private List<UnitProperty> attackedPpt = new List<UnitProperty>();

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	public Transform BehitTransform;

	private float durationTimer;

	private float attackIntervalTimer;

	[Header("精魄新技能")]
	private const int chainAttackBaseRequirementCount = 4;

	private const float AttackSpeedUpPerLevel = 20f;

	public float chainAttackDetectRange;

	public float chainAttackTentacleSize;

	public float chainAttackFinalDamageRatio;

	private int chainAttackRequirementCount;

	public float chainAttackSpawnInterval;

	private float chainAttackSpawmTimer;

	private int chainAttackCounter;

	private int ChainAttackLevel;

	private List<ChainTentacleSpawnData> chainTentacleList;

	public float ChaseMouseSpeedRatio;

	public float AttackBaseDuration;

	public GameObject shadowObj;

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public UnitState state { get; set; }

	public override void SingleInitialCallback()
	{
		float value = UnityEngine.Random.Range(-100f, 100f);
		sr.material.SetFloat("_TimeOffset", value);
		fireIdleEffect.GetComponent<SpriteRenderer>().material.SetFloat("_TimeOffset", value);
		chainTentacleList = new List<ChainTentacleSpawnData>();
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		shadowObj.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		shadowObj.SetActive(value: true);
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		durationTimer = 0f;
		attackIntervalTimer = 0f;
		state = UnitState.Idle;
		base.Anima.SetTrigger("Idle");
		attackInterval.RandomResult();
		sr.gameObject.SetActive(value: true);
		chainAttackCounter = 0;
		chainAttackSpawmTimer = 0f;
		chainAttackRequirementCount = 4;
		chainTentacleList.Clear();
		sr.material.SetInt(UseGhostEffect, 0);
		sr.material.SetInt(UseFuseShineEffect, 0);
		sr.material.SetFloat(FuseShineProcess, 0f);
		sr_Attack.material.SetInt(UseGhostEffect, 0);
		sr_Attack.material.SetInt(UseFuseShineEffect, 0);
		sr_Attack.material.SetFloat(FuseShineProcess, 0f);
		fireIdleEffect.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		fireAttackEffect.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		BehitTransform.localPosition = Vector3.zero;
		ShowTeammate();
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		state = UnitState.Idle;
		base.Anima.SetTrigger("Idle");
		BehitTransform.localPosition = new Vector3(0f, 0f, -0.25f);
		base.beingControlledByTeammate6 = true;
		ColliderToggle(state: false);
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.transform.eulerAngles = Vector3.zero;
			base.CanMove = true;
			BehitTransform.localPosition = Vector3.zero;
			ShowTeammate();
		}
	}

	public override void OnEnterDelayDeathEvent()
	{
		base.OnEnterDelayDeathEvent();
		if (!(base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f))
		{
			sr.material.SetInt(UseGhostEffect, 1);
			sr_Attack.material.SetInt(UseGhostEffect, 1);
			SummonGhostEffectToggle(state: true);
			ColliderToggle(state: false);
			FreeFromTeammate6();
		}
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		sr.material.SetInt(UseFuseShineEffect, 1);
		sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
		if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
		{
			fireIdleEffect.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
			fireAttackEffect.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		sr_Attack.material.SetInt(UseFuseShineEffect, 1);
		sr_Attack.material.DOFloat(1f, FuseShineProcess, 1.3f);
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		durationTimer = 0f - base.SummonerSpellBase.SpellHoverTime;
		ChainAttackLevel = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level;
		chainAttackRequirementCount = 4 - base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level;
		fireIdleEffect.SetActive(value: false);
		fireAttackEffect.SetActive(value: false);
		normalAttackEffect.SetActive(value: true);
		voidAttackEffect.SetActive(value: false);
		switch (base.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Frozen:
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECFrozen : sprite_ECFrozen);
			sr_Attack.material = mat_ECFrozen;
			break;
		case SpellColorType.Mucus:
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECMucus : sprite_ECMucus);
			sr_Attack.material = mat_ECMucus;
			break;
		case SpellColorType.Fire:
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECPlayer : sprite_ECPlayer);
			sr_Attack.material = mat_ECPlayer;
			fireIdleEffect.SetActive(value: true);
			fireAttackEffect.SetActive(value: true);
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECPlayer : sprite_ECPlayer);
			sr_Attack.material = mat_ECPlayer;
			break;
		case SpellColorType.Venom:
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECVenom : sprite_ECVenom);
			sr_Attack.material = mat_ECVenom;
			break;
		case SpellColorType.Void:
			normalAttackEffect.SetActive(value: false);
			voidAttackEffect.SetActive(value: true);
			sr.sprite = (FusionData.IsFusedUnit ? sprite2_ECVoid : sprite_ECVoid);
			sr_Attack.material = mat_ECVoid;
			break;
		default:
			Debug.LogError(base.SummonerSpellBase.ColorType);
			break;
		}
	}

	public override void Update()
	{
		attackIntervalTimer += Time.deltaTime * (base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio + 0.2f * (float)ChainAttackLevel);
		SummonsTouchMonster();
		myPpt.bodyCenterPoint = base.transform.position + new Vector3(0f, rootPosShift, 0f);
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		UpdateChainTentacleEffect();
		switch (state)
		{
		case UnitState.Idle:
			if (attackIntervalTimer >= attackInterval.result && base.CanMove)
			{
				GetRandomTarget();
				if (targetPpt != null)
				{
					attackIntervalTimer = 0f;
					attackInterval.RandomResult();
					state = UnitState.Attack;
					base.Anima.SetTrigger("Attack");
					base.Anima.speed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio + 0.2f * (float)ChainAttackLevel;
					StartCoroutine(ChangeStateToNormal(attackInterval.result / (base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio + 0.2f * (float)ChainAttackLevel)));
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Attack:
			break;
		}
		durationTimer += Time.deltaTime;
		if (durationTimer >= base.SummonerSpellBase.spellCfg.duration && base.CanMove)
		{
			myPpt.AnnouncedDeath();
		}
	}

	private void UpdateChainTentacleEffect()
	{
		if (ChainAttackLevel <= 0)
		{
			return;
		}
		chainAttackSpawmTimer += Time.deltaTime;
		for (int num = chainTentacleList.Count - 1; num >= 0; num--)
		{
			ChainTentacleSpawnData data = chainTentacleList[num];
			UpdateChainTentaclePosition(data);
		}
		if (chainAttackSpawmTimer >= chainAttackSpawnInterval)
		{
			chainAttackSpawmTimer -= chainAttackSpawnInterval;
			for (int num2 = chainTentacleList.Count - 1; num2 >= 0; num2--)
			{
				ChainTentacleSpawnData data2 = chainTentacleList[num2];
				SpawnChainTentacle(data2);
			}
		}
	}

	private void UpdateChainTentaclePosition(ChainTentacleSpawnData data)
	{
		switch (base.SummonerSpellBase.currentSpellMovement)
		{
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / (base.SummonerSpellBase.CurrentSpeed + 10f)) * Time.deltaTime;
			data.currentAngle += num;
			data.moveDir = Tool2D.GetDir(data.currentAngle + 90f);
			data.currentPoint = Tool2D.IgnoreZPoint(base.transform.position + Tool2D.GetDir(data.currentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius, base.transform.position.z);
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
			Vector3 vector = (data.velocity = Vector3.Lerp(data.velocity, Tool2D.IgnoreZV2ToV1Normal(mousePoint, data.currentPoint) * base.SummonerSpellBase.CurrentSpeed, base.SummonerSpellBase.CurrentSpeed * base.SummonerSpellBase.spellFollowMouseLerp * ChaseMouseSpeedRatio * Time.deltaTime));
			data.moveDir = vector.normalized;
			data.currentPoint += data.velocity * Time.deltaTime;
			break;
		}
		case SpellSpecialMovementType.ChaseEnemy:
			if (base.SummonerSpellBase.SpellFollowHaveTarget)
			{
				data.moveDir = Tool2D.DirMoveTowards(data.moveDir, Tool2D.IgnoreZV2ToV1Normal(base.SummonerSpellBase.spellFollowTargetPpt.transform.position, data.currentPoint), base.SummonerSpellBase.CurrentSpeed * base.SummonerSpellBase.spellFollowTargetRotateSpeed * Time.deltaTime);
				data.velocity = data.moveDir.normalized * base.SummonerSpellBase.CurrentSpeed;
				data.currentPoint += data.velocity * Time.deltaTime;
			}
			else
			{
				base.SummonerSpellBase.spellFollowTargetPpt = base.SummonerSpellBase.GetMiniMalAngleTargetablePpt();
			}
			break;
		default:
			data.currentPoint += data.moveDir * base.SummonerSpellBase.CurrentSpeed * Time.deltaTime;
			break;
		}
		data.remainTime -= Time.deltaTime;
		if (data.remainTime <= 0f)
		{
			chainTentacleList.Remove(data);
		}
	}

	private GameObject SpawnHitTentacleObject(Vector3 Position)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20031 + "/" + 20031 + "_Attack", Position, 0.5f);
	}

	private void SpawnChainTentacle(ChainTentacleSpawnData data)
	{
		GameObject obj = SpawnHitTentacleObject(Tool2D.IgnoreZPoint(data.currentPoint) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, attackEffectOffset) + new Vector3(0f, 0f - attackEffectOffset - 0.01f, 0f));
		obj.GetComponent<Teammate3_Attack>().Initialize(base.SummonerSpellBase);
		float num = base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
		obj.transform.localScale = Vector3.one * num * chainAttackTentacleSize;
		TakeDamageInfo info = new TakeDamageInfo
		{
			criticalChance = base.SummonerSpellBase.GetCriticalChance(),
			wandChargeData = base.SummonerSpellBase.wandChargeData
		};
		int num2 = Mathf.CeilToInt(SpellConfig.dic[base.SummonerSpellBase.spellCfg.id].damage * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.GetSummonValueRatio().damageRatio * base.SummonerSpellBase.finalDamageRatio * chainAttackFinalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(data.currentPoint, chainAttackDetectRange * num, "Monster", "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness");
		if (collidersByTag.Count > 0)
		{
			SEMgr.Inst.teammate3Attack.PlaySE();
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
					spell1002RollBall.TakeDamage(num2);
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster"))
			{
				UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
				base.SummonerSpellBase.ApplyElementEffect(component);
				component.TakeDamage(num2, myPpt, info);
				base.SummonerSpellBase.CheckIfPullCrystalIsValidToAttack(info, component);
				base.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(component.transform.position);
			}
			else
			{
				item.gameObject.GetComponent<UnitProperty>().TakeDamage(num2, myPpt, info);
			}
		}
	}

	private IEnumerator ChangeStateToNormal(float time)
	{
		yield return new WaitForSeconds(time);
		state = UnitState.Idle;
		base.Anima.SetTrigger("Idle");
		base.Anima.speed = 1f;
	}

	private void SpawnNormalTentacle()
	{
		if (!Teammate.needDisableEffect)
		{
			SpawnHitTentacleObject(Tool2D.IgnoreZPoint(targetPpt.transform) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, attackEffectOffset) + new Vector3(0f, 0f - attackEffectOffset - 0.01f, 0f)).GetComponent<Teammate3_Attack>().Initialize(base.SummonerSpellBase);
		}
		TakeDamageInfo info = new TakeDamageInfo
		{
			criticalChance = base.SummonerSpellBase.GetCriticalChance(),
			wandChargeData = base.SummonerSpellBase.wandChargeData
		};
		base.SummonerSpellBase.ApplyVoidEffect(targetPpt);
		targetPpt.TakeDamage(Mathf.CeilToInt(base.SummonerSpellBase.spellCfg.damage * base.SummonerSpellBase.GetSummonValueRatio().damageRatio), myPpt, info);
		base.SummonerSpellBase.ApplyElementEffect(targetPpt);
		base.SummonerSpellBase.virtualRealPosition = targetPpt.transform.position;
		base.SummonerSpellBase.CheckIfPullCrystalIsValidToAttack(info, targetPpt);
		base.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(targetPpt.transform.position);
		SEMgr.Inst.teammate3Attack.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				state = UnitState.Idle;
				base.Anima.SetTrigger("Idle");
				base.Anima.speed = 1f;
				if (base.SummonerSpellBase.spellSplitCount <= 0)
				{
					return;
				}
				attackedPpt.Clear();
				int num = base.SummonerSpellBase.spellSplitCount;
				for (int i = 0; i < base.SummonerSpellBase.spellSplitCount; i++)
				{
					List<UnitProperty> targetablePpts = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts;
					for (int num2 = targetablePpts.Count - 1; num2 >= 0; num2--)
					{
						if (!attackedPpt.Contains(targetablePpts[num2]) && targetablePpts[num2] != targetPpt && Tool2D.IgnoreZDistanceSqr(targetPpt.transform.position, targetablePpts[num2].transform.position) <= MaxSplitRadiu * MaxSplitRadiu)
						{
							if (!Teammate.needDisableEffect)
							{
								base.SummonerSpellBase.GetEffect("Attack", Tool2D.IgnoreZPoint(targetablePpts[num2].transform) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, attackEffectOffset) + new Vector3(0f, 0f - attackEffectOffset - 0.01f, 0f), 0.8f).GetComponent<Teammate3_Attack>().Initialize(base.SummonerSpellBase);
							}
							attackedPpt.Add(targetablePpts[num2]);
							base.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(targetablePpts[num2].transform.position);
							base.SummonerSpellBase.ApplyVoidEffect(targetablePpts[num2]);
							TakeDamageInfo info = new TakeDamageInfo
							{
								criticalChance = base.SummonerSpellBase.GetCriticalChance(),
								wandChargeData = base.SummonerSpellBase.wandChargeData
							};
							targetablePpts[num2].TakeDamage(Mathf.CeilToInt(base.SummonerSpellBase.spellCfg.damage * base.SummonerSpellBase.GetSummonValueRatio().damageRatio * 0.5f), myPpt, info);
							base.SummonerSpellBase.ApplyElementEffect(targetablePpts[num2]);
							SEMgr.Inst.teammate3Attack.PlaySE();
							num--;
							break;
						}
						if (num <= 0)
						{
							break;
						}
					}
					if (num <= 0)
					{
						break;
					}
				}
				if (!Teammate.needDisableEffect)
				{
					for (int j = 0; j < num; j++)
					{
						base.SummonerSpellBase.GetEffect("Attack", Tool2D.IgnoreZPoint(targetPpt.transform.position + UnityEngine.Random.insideUnitSphere * MaxSplitRadiu) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, attackEffectOffset) + new Vector3(0f, 0f - attackEffectOffset - 0.01f, 0f), 0.8f).GetComponent<Teammate3_Attack>().Initialize(base.SummonerSpellBase);
					}
				}
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		if (targetPpt == null || !targetPpt.gameObject.activeSelf)
		{
			state = UnitState.Idle;
			base.Anima.SetTrigger("Idle");
			return;
		}
		UnitProperty randomTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetRandomTargetablePpt();
		if (chainAttackCounter >= chainAttackRequirementCount && randomTargetablePpt != null)
		{
			chainAttackCounter = 0;
			chainTentacleList.Add(new ChainTentacleSpawnData(base.transform.position, Tool2D.IgnoreZPoint(randomTargetablePpt.transform), base.SummonerSpellBase.CurrentSpeed, AttackBaseDuration));
			return;
		}
		if (ChainAttackLevel > 0)
		{
			chainAttackCounter++;
		}
		SpawnNormalTentacle();
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
}
