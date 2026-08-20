using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate3FuseController : Teammate
{
	private List<Teammate3FuseBody> tentacleList = new List<Teammate3FuseBody>();

	private float attackIntervalTimer;

	private int tentacleShootGroupIndex;

	private float durationTimer;

	private float realAttackInterval;

	public VariableFloat baseAttackInterval;

	private const float AttackSpeedUpPerLevel = 20f;

	public float rootPosShift;

	private int ChainAttackLevel;

	private int essenceAttackRequirement;

	private const int essenceAttackBaseRequirement = 4;

	public float chainAttackBaseTriggerDistance;

	public float chainAttackDetectRange;

	public float chainAttackTentacleSize;

	public float chainAttackFinalDamageRatio;

	public float chainAttackSpawnInterval;

	private float chainAttackSpawmTimer;

	private int chainAttackCounter;

	private float chainAttackDistance;

	private List<ChainTentacleSpawnData> chainTentacleList = new List<ChainTentacleSpawnData>();

	public float ChaseMouseSpeedRatio;

	public float AttackBaseDuration;

	public Transform tentacleTransform;

	public float distanceBetweenTentacle;

	public Sprite sprite_ECFrozen;

	public Sprite sprite_ECMucus;

	public Sprite sprite_ECPlayer;

	public Sprite sprite_ECVenom;

	public Sprite sprite_ECVoid;

	public Sprite sprite_ECFrozen_R;

	public Sprite sprite_ECMucus_R;

	public Sprite sprite_ECPlayer_R;

	public Sprite sprite_ECVenom_R;

	public Sprite sprite_ECVoid_R;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public float attackEffectOffset;

	private int totalTentacleCount;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public RuntimeAnimatorController LTentacle;

	public RuntimeAnimatorController RTentacle;

	public float BodyDistance;

	public CapsuleCollider CapCollider;

	public Transform BehitTransform;

	public Shadow SelfShadow;

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		attackIntervalTimer = 0f;
		realAttackInterval = baseAttackInterval.RandomResult();
		tentacleShootGroupIndex = 0;
		durationTimer = 0f;
		essenceAttackRequirement = 0;
		totalTentacleCount = 0;
		ColliderToggle(state: true);
		ShowTeammate();
		BehitTransform.localPosition = Vector3.zero;
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		SelfShadow.ShadowGO.SetActive(value: false);
		foreach (Teammate3FuseBody tentacle in tentacleList)
		{
			tentacle.transform.Find("Layer").gameObject.SetActive(value: false);
		}
	}

	public override void ShowTeammate()
	{
		base.transform.eulerAngles = Vector3.zero;
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		SelfShadow.ShadowGO.SetActive(value: true);
		foreach (Teammate3FuseBody tentacle in tentacleList)
		{
			tentacle.transform.Find("Layer").gameObject.SetActive(value: true);
		}
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		base.beingControlledByTeammate6 = true;
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.beingControlledByTeammate6 = false;
			base.CanMove = true;
			ShowTeammate();
		}
	}

	public override void OnEnterDelayDeathEvent()
	{
		base.OnEnterDelayDeathEvent();
		if (base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f)
		{
			return;
		}
		foreach (Teammate3FuseBody tentacle in tentacleList)
		{
			tentacle.idleSr.material.SetInt(UseGhostEffect, 1);
			tentacle.attackSr.material.SetInt(UseGhostEffect, 1);
			SummonGhostEffectToggle(state: true);
			ColliderToggle(state: false);
		}
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		foreach (Teammate3FuseBody tentacle in tentacleList)
		{
			tentacle.idleSr.material.SetInt(UseFuseShineEffect, 1);
			tentacle.idleSr.material.DOFloat(1f, FuseShineProcess, 1.3f);
			if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
			{
				tentacle.fireIdleSr.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
				tentacle.fireAttackSr.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
			}
			tentacle.attackSr.material.SetInt(UseFuseShineEffect, 1);
			tentacle.attackSr.material.DOFloat(1f, FuseShineProcess, 1.3f);
			tentacle.SelfShadow.ShadowGO.SetActive(value: false);
		}
	}

	private void SummonTentacles()
	{
		if (tentacleList.Count < totalTentacleCount)
		{
			int num = totalTentacleCount - tentacleList.Count;
			for (int i = 0; i < num; i++)
			{
				Teammate3FuseBody component = SpawnTentacleObject(base.transform.position, tentacleTransform).GetComponent<Teammate3FuseBody>();
				tentacleList.Add(component);
			}
		}
		Vector3 vector = new Vector3((float)(-FusionData.CurrentFusionLevel / 2) * distanceBetweenTentacle, 0f, 0f);
		for (int j = 0; j < totalTentacleCount; j++)
		{
			Vector3 localPosition = vector + new Vector3(distanceBetweenTentacle * (float)j, 0f, -0.01f * (float)j);
			tentacleList[j].gameObject.SetActive(value: true);
			tentacleList[j].SelfShadow.ShadowGO.SetActive(value: true);
			tentacleList[j].transform.localPosition = localPosition;
			SetTentacleOutLook(tentacleList[j], j != 0);
			if (j == 0)
			{
				tentacleList[j].Anima.runtimeAnimatorController = LTentacle;
			}
		}
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		durationTimer = 0f - base.SummonerSpellBase.SpellHoverTime;
		ChainAttackLevel = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level;
		chainAttackDistance = chainAttackBaseTriggerDistance + base.SummonerSpellBase.spellCfg.speed;
		essenceAttackRequirement = 4 - base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level;
		totalTentacleCount = FusionData.CurrentFusionLevel + 1;
		CapCollider.height = BodyDistance * (float)totalTentacleCount + 0.4f;
		realAttackInterval = baseAttackInterval.RandomResult() / (float)totalTentacleCount;
		SummonTentacles();
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
		if (attackIntervalTimer >= realAttackInterval && base.CanMove)
		{
			GetRandomTarget();
			if (targetPpt != null)
			{
				attackIntervalTimer = 0f;
				tentacleList[tentacleShootGroupIndex].Anima.SetTrigger("Attack");
				tentacleShootGroupIndex++;
				if (tentacleShootGroupIndex >= tentacleList.Count)
				{
					tentacleShootGroupIndex = 0;
					realAttackInterval = baseAttackInterval.RandomResult() / (float)totalTentacleCount;
				}
			}
		}
		durationTimer += Time.deltaTime;
		if (durationTimer >= base.SummonerSpellBase.spellCfg.duration && base.CanMove)
		{
			UnitDead();
		}
	}

	private void UnitDead()
	{
		myPpt.AnnouncedDeath();
	}

	private void SetTentacleOutLook(Teammate3FuseBody body, bool isRightBofy = true)
	{
		body.fireAttackSr.gameObject.SetActive(value: false);
		body.fireIdleSr.gameObject.SetActive(value: false);
		body.normalAttackEffect.SetActive(value: true);
		body.voidAttackEffect.SetActive(value: false);
		switch (base.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Frozen:
			body.idleSr.sprite = (isRightBofy ? sprite_ECFrozen_R : sprite_ECFrozen);
			body.attackSr.material = mat_ECFrozen;
			break;
		case SpellColorType.Mucus:
			body.idleSr.sprite = (isRightBofy ? sprite_ECMucus_R : sprite_ECMucus);
			body.attackSr.material = mat_ECMucus;
			break;
		case SpellColorType.Fire:
			body.idleSr.sprite = (isRightBofy ? sprite_ECPlayer_R : sprite_ECPlayer);
			body.attackSr.material = mat_ECPlayer;
			body.fireAttackSr.gameObject.SetActive(value: true);
			body.fireIdleSr.gameObject.SetActive(value: true);
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			body.idleSr.sprite = (isRightBofy ? sprite_ECPlayer_R : sprite_ECPlayer);
			body.attackSr.material = mat_ECPlayer;
			break;
		case SpellColorType.Venom:
			body.idleSr.sprite = (isRightBofy ? sprite_ECVenom_R : sprite_ECVenom);
			body.attackSr.material = mat_ECVenom;
			break;
		case SpellColorType.Void:
			body.normalAttackEffect.SetActive(value: false);
			body.voidAttackEffect.SetActive(value: true);
			body.idleSr.sprite = (isRightBofy ? sprite_ECVoid_R : sprite_ECVoid);
			body.attackSr.material = mat_ECVoid;
			break;
		default:
			Debug.LogError(base.SummonerSpellBase.ColorType);
			break;
		}
		body.idleSr.material.SetInt(UseGhostEffect, 0);
		body.idleSr.material.SetInt(UseFuseShineEffect, 0);
		body.idleSr.material.SetFloat(FuseShineProcess, 0f);
		body.attackSr.material.SetInt(UseGhostEffect, 0);
		body.attackSr.material.SetInt(UseFuseShineEffect, 0);
		body.attackSr.material.SetFloat(FuseShineProcess, 0f);
		body.fireIdleSr.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		body.fireAttackSr.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		body.Controller = this;
	}

	private GameObject SpawnHitTentacleObject(Vector3 Position)
	{
		return ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 20031 + "/" + 20031 + "_Attack", Position, 0.5f);
	}

	private GameObject SpawnTentacleObject(Vector3 position, Transform parent)
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 20031 + "/" + 20031 + "_FuseBody"), position, quaternion.identity, parent);
	}

	public void TentacleAttackEnemy(Vector3 position)
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, chainAttackDistance, "Monster");
		if (chainAttackCounter >= essenceAttackRequirement && collidersByTag.Count > 0)
		{
			chainAttackCounter = 0;
			chainTentacleList.Add(new ChainTentacleSpawnData(base.transform.position, Tool2D.IgnoreZPoint(collidersByTag[UnityEngine.Random.Range(0, collidersByTag.Count)].transform), base.SummonerSpellBase.CurrentSpeed, AttackBaseDuration));
			return;
		}
		if (ChainAttackLevel > 0)
		{
			chainAttackCounter++;
		}
		SpawnNormalAttack();
	}

	private void SpawnNormalAttack()
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
		base.SummonerSpellBase.ApplyElementEffect(targetPpt);
		targetPpt.TakeDamage(Mathf.CeilToInt(base.SummonerSpellBase.spellCfg.damage * base.SummonerSpellBase.GetSummonValueRatio().damageRatio), myPpt, info);
		base.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(targetPpt.transform.position);
		base.SummonerSpellBase.virtualRealPosition = targetPpt.transform.position;
		base.SummonerSpellBase.CheckIfPullCrystalIsValidToAttack(info, targetPpt);
		SEMgr.Inst.teammate3Attack.PlaySE();
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
				base.SummonerSpellBase.ApplyVoidEffect(component);
				component.TakeDamage(num2, myPpt, info);
				base.SummonerSpellBase.ApplyElementEffect(component);
				base.SummonerSpellBase.CheckIfPullCrystalIsValidToAttack(info, component);
				base.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(component.transform.position);
			}
			else
			{
				item.gameObject.GetComponent<UnitProperty>().TakeDamage(num2, myPpt, info);
			}
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

	private void OnDisable()
	{
		foreach (Teammate3FuseBody tentacle in tentacleList)
		{
			tentacle.gameObject.SetActive(value: false);
		}
	}
}
