using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate4 : Teammate
{
	[Space(50f)]
	public Transform tsf_Pillars;

	public SpriteRenderer srs;

	public Sprite sprite1;

	public Sprite sprite2;

	public Sprite sprite3;

	[Range(0f, 1f)]
	public float sprite2HPRatio;

	[Range(0f, 1f)]
	public float sprite3HPRatio;

	public float width;

	public float SummonCount;

	public float ApplyDebuffToRoundMonsterInterval = 0.15f;

	public float ApplyVenomToRoundMonsterInterval = 1f;

	private float applyDebuffCounter;

	private float applyVenomCounter;

	public float ApplyDamageInterval;

	public float dealDamageBaseRange;

	private float dealDamageTimer;

	private float selfHealingCounter;

	public Material[] pillarColor;

	public GameObject[] pillarParticles;

	public SpriteRenderer firePillarOutlineSprite;

	private bool isFirePillar;

	public Sprite[] pillarDamageStageSprite;

	public float chaseMouseLerpSpeedRatio;

	public Transform pillarCenterTransform;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	[Header("精魄新技能")]
	private const float heavyCrashActiveInterval = 2.5f;

	private const float heavyCrashBaseRange = 1.5f;

	public float heavyCrashHpDamageRatio;

	public float heavyCrashDamageRatioUpPerLevel;

	public float heavyCrashPullForce;

	private float heavyCrashTimer;

	private float heavyCrashReckeckTimer;

	[Header("精魄悬浮效果")]
	public float baseFloatHeight;

	public float extraFloatHeight;

	public float floatingPillarLerpSpeed;

	private float pillarFloatTimer;

	public Shadow selfShadow;

	public Shadow selfShadow2;

	private float currentFloatingLerpSpeed;

	[Header("融合额外墙体")]
	public Transform tsf_Pillar2;

	public float fuseWallDistance;

	public Transform tsf_FuseWall;

	public SpriteRenderer fuseWallSprite;

	private static readonly int RotateAngle = Shader.PropertyToID("_RotateAngle");

	public LayerMask attackLayer;

	private Vector2 wallDirection = Vector2.zero;

	public SpriteRenderer srs2;

	public SpriteRenderer firePillar2OutlineSprite;

	public GameObject[] pillar2Particles;

	public Sprite[] fuseWallDamageSprites;

	public Material[] fusedWallMaterial;

	public Material[] fusedFullWallMaterial;

	public SpriteRenderer fuseWallSprite2;

	public Sprite[] fuseWallDamageSprites2;

	public ParticleSystem fuseWallParticle;

	public Material[] fuseWallParticleMaterial;

	public LineRenderer fusedWallLineShadow;

	public GameObject attackSize;

	public Transform rotateTrans;

	public Transform BehitTransform;

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		BehitTransform.localPosition = Vector3.zero;
		ShowTeammate();
	}

	public override void HideTeammate()
	{
		selfShadow.ShadowGO.SetActive(value: false);
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		selfShadow.ShadowGO.SetActive(value: true);
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		base.Anima.speed = 1f;
		BehitTransform.localPosition = new Vector3(0f, 0f, -0.25f);
		tsf_Pillars.localPosition = Vector3.Lerp(tsf_Pillars.localPosition, Vector3.zero, currentFloatingLerpSpeed * Time.deltaTime);
		myPpt.bodyCenterPoint = Tool2D.IgnoreZPoint(pillarCenterTransform.position);
		tsf_Pillars.localPosition = Vector3.zero;
		ShadowSizeShift();
		base.beingControlledByTeammate6 = true;
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.beingControlledByTeammate6 = false;
			BehitTransform.localPosition = Vector3.zero;
			base.transform.eulerAngles = Vector3.zero;
			base.CanMove = true;
			ShowTeammate();
		}
	}

	public override void OnEnterDelayDeathEvent()
	{
		base.OnEnterDelayDeathEvent();
		if (!(base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f))
		{
			srs.material.SetInt(UseGhostEffect, 1);
			if (FusionData.IsFusedUnit)
			{
				srs2.material.SetInt(UseGhostEffect, 1);
				fuseWallSprite.material.SetInt(UseGhostEffect, 1);
			}
			SummonGhostEffectToggle(state: true);
			ColliderToggle(state: false);
			FreeFromTeammate6();
		}
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		srs.material.SetInt(UseFuseShineEffect, 1);
		srs.material.DOFloat(1f, FuseShineProcess, 1.3f);
		if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
		{
			firePillarOutlineSprite.material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		foreach (Transform item in pillarParticles[4].transform)
		{
			ParticleSystem component = item.GetComponent<ParticleSystem>();
			if ((bool)component)
			{
				component.Stop();
			}
		}
		foreach (Transform item2 in pillarParticles[5].transform)
		{
			ParticleSystem component2 = item2.GetComponent<ParticleSystem>();
			if ((bool)component2)
			{
				component2.Stop();
			}
		}
		selfShadow.ShadowGO.SetActive(value: false);
		if (FusionData.IsFusedUnit)
		{
			selfShadow2.ShadowGO.SetActive(value: false);
		}
	}

	private void UpdateFloatingEffect()
	{
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && base.CanMove)
		{
			pillarFloatTimer += Time.deltaTime;
			float num = baseFloatHeight + extraFloatHeight * Mathf.Sin(pillarFloatTimer * 2f);
			if (FusionData.IsFusedUnit)
			{
				float num2 = base.transform.localScale.x * fuseWallDistance / 2f;
				float num3 = wallDirection.x * num2;
				float num4 = wallDirection.y * num2;
				tsf_FuseWall.localPosition = Vector3.Lerp(tsf_FuseWall.localPosition, new Vector3(0f, 0.5f + num, 0f), currentFloatingLerpSpeed * Time.deltaTime);
				tsf_Pillars.localPosition = Vector3.Lerp(tsf_Pillars.localPosition, new Vector3(0f - num3, 0f - num4 + num, 0f), currentFloatingLerpSpeed * Time.deltaTime);
				tsf_Pillar2.localPosition = Vector3.Lerp(tsf_Pillar2.localPosition, new Vector3(num3, num4 + num, 0f), currentFloatingLerpSpeed * Time.deltaTime);
				myPpt.bodyCenterPoint = Tool2D.IgnoreZPoint(pillarCenterTransform.position);
				UpdateFloatingFusingWallShadowPosition(num * currentFloatingLerpSpeed / floatingPillarLerpSpeed);
				FusionShadowSizeShift(num);
			}
			else
			{
				tsf_Pillars.localPosition = Vector3.Lerp(tsf_Pillars.localPosition, new Vector3(0f, baseFloatHeight + extraFloatHeight * Mathf.Sin(pillarFloatTimer * 2f), 0f), currentFloatingLerpSpeed * Time.deltaTime);
				myPpt.bodyCenterPoint = Tool2D.IgnoreZPoint(pillarCenterTransform.position);
				ShadowSizeShift();
			}
		}
	}

	private void ShadowSizeShift()
	{
		selfShadow.ShadowGO.transform.localScale = Vector3.one * selfShadow.shadowScale * (1f - tsf_Pillars.localPosition.y * 0.5f);
	}

	private void FusionShadowSizeShift(float floatingHeight)
	{
		selfShadow.ShadowGO.transform.localScale = Vector3.one * selfShadow.shadowScale * (1f - floatingHeight * 0.5f);
		selfShadow2.ShadowGO.transform.localScale = Vector3.one * selfShadow2.shadowScale * (1f - floatingHeight * 0.5f);
	}

	private void UpdateHeavyCrashTimer()
	{
		if (base.beingControlledByTeammate6 || base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level <= 0)
		{
			return;
		}
		heavyCrashTimer += Time.deltaTime * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		heavyCrashReckeckTimer += Time.deltaTime;
		if (heavyCrashTimer < 2.5f || heavyCrashReckeckTimer < 0.1f)
		{
			return;
		}
		heavyCrashReckeckTimer = 0f;
		if (!CheckIfEnemyInEssenceAttackRange(GetHeavyCrashRange()))
		{
			heavyCrashTimer = 0f;
			currentFloatingLerpSpeed = 0f;
			if (FusionData.IsFusedUnit)
			{
				float num = wallDirection.x * base.transform.localScale.x * fuseWallDistance / 2f;
				float num2 = wallDirection.y * base.transform.localScale.x * fuseWallDistance / 2f;
				tsf_Pillars.DOLocalMove(new Vector3(0f - num, 0f - num2, 0f), 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
				tsf_Pillar2.DOLocalMove(new Vector3(num, num2, 0f), 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
				tsf_FuseWall.DOLocalMove(new Vector3(0f, 0.5f, 0f), 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
			}
			else
			{
				tsf_Pillars.DOLocalMove(Vector3.zero, 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
			}
			StartCoroutine(HeavyCrashDealDamageAndPullForceToAllEnemyInRange(0.03f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio));
		}
	}

	private bool CheckIfEnemyInEssenceAttackRange(float radiu)
	{
		float num = base.transform.localScale.x * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		return (FusionData.IsFusedUnit ? GeneralTool.GetBoxCollidersByTag(base.transform.position, new Vector3((fuseWallDistance + 1.5f) * num, 1.5f * num, 10f), Quaternion.Euler(new Vector3(wallDirection.x, wallDirection.y, 0f)), attackLayer, "Monster") : GeneralTool.GetCollidersByTag(base.transform.position, radiu, "Monster")).Count <= 0;
	}

	private IEnumerator HeavyCrashDealDamageAndPullForceToAllEnemyInRange(float time)
	{
		yield return new WaitForSeconds(time);
		SEMgr.Inst.teammate4HeavyAttack.PlaySE();
		float num = 1.5f * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		List<Collider> essenceHeavyHitTargetList = GetEssenceHeavyHitTargetList(num);
		int num2 = Mathf.CeilToInt(myPpt.unitCfg.maxHP * (heavyCrashHpDamageRatio * (1f + (float)(base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level - 1) / 2f)) * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra * 0.5f);
		base.SummonerSpellBase.GetEffect("HeavyCrashEffect_" + base.SummonerSpellBase.ColorType, Tool2D.IgnoreZPoint(tsf_Pillars.position), 0.8f).transform.localScale = Vector3.one * num;
		if (FusionData.IsFusedUnit)
		{
			base.SummonerSpellBase.GetEffect("HeavyCrashEffect_" + base.SummonerSpellBase.ColorType, Tool2D.IgnoreZPoint(tsf_Pillar2.transform.position), 0.8f).transform.localScale = Vector3.one * num;
		}
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo
		{
			damage = num2,
			canRebound = false
		};
		foreach (Collider item in essenceHeavyHitTargetList.Where((Collider e) => e.gameObject.activeInHierarchy))
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
				UnitProperty component = item.GetComponent<UnitProperty>();
				base.SummonerSpellBase.spellCfg.damage = takeDamageInfo.damage;
				base.SummonerSpellBase.ApplyVoidEffect(component);
				base.SummonerSpellBase.OutputDamage(component, takeDamageInfo, SpellAbilityType.TeammateSprite);
				component.TakeKnockback((base.transform.position - component.transform.position).normalized * heavyCrashPullForce);
			}
			else
			{
				base.SummonerSpellBase.OutputDamage(item.gameObject, takeDamageInfo);
			}
		}
		yield return new WaitForSeconds(2.5f - 0.9f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio);
		DOTween.To(() => currentFloatingLerpSpeed, delegate(float x)
		{
			currentFloatingLerpSpeed = x;
		}, floatingPillarLerpSpeed, 0.5f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		base.transform.up = base.SummonerSpellBase.Direction;
		base.transform.rotation = quaternion.identity;
		isFirePillar = false;
		heavyCrashTimer = 2.5f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio - 0.6f;
		pillarFloatTimer = UnityEngine.Random.Range(0f, 10f);
		currentFloatingLerpSpeed = floatingPillarLerpSpeed;
		if (FusionData.IsFusedUnit)
		{
			srs2.flipX = false;
			firePillarOutlineSprite.flipX = false;
			srs.flipX = true;
			firePillar2OutlineSprite.flipX = true;
			firePillar2OutlineSprite.sprite = pillarDamageStageSprite[0];
		}
		else
		{
			srs.flipX = UnityEngine.Random.Range(0, 2) == 0;
			firePillarOutlineSprite.flipX = srs.flipX;
			firePillarOutlineSprite.sprite = pillarDamageStageSprite[0];
		}
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level <= 0)
		{
			tsf_Pillars.localPosition = Vector3.zero;
			if (FusionData.IsFusedUnit)
			{
				tsf_Pillar2.localPosition = Vector3.zero;
				tsf_FuseWall.localPosition = Vector3.zero;
			}
		}
		for (int i = 0; i < pillarParticles.Length; i++)
		{
			pillarParticles[i].SetActive(value: false);
		}
		if (FusionData.IsFusedUnit)
		{
			for (int j = 0; j < pillarParticles.Length; j++)
			{
				pillar2Particles[j].SetActive(value: false);
			}
		}
		switch (base.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Player:
			srs.material = pillarColor[0];
			pillarParticles[0].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[0];
				fuseWallSprite.material = fusedWallMaterial[0];
				fuseWallSprite2.material = fusedFullWallMaterial[0];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[0];
				pillar2Particles[0].SetActive(value: true);
			}
			break;
		case SpellColorType.Mucus:
			srs.material = pillarColor[1];
			pillarParticles[1].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[1];
				fuseWallSprite.material = fusedWallMaterial[1];
				fuseWallSprite2.material = fusedFullWallMaterial[1];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[1];
				pillar2Particles[1].SetActive(value: true);
			}
			break;
		case SpellColorType.Frozen:
			srs.material = pillarColor[2];
			pillarParticles[2].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[2];
				fuseWallSprite.material = fusedWallMaterial[2];
				fuseWallSprite2.material = fusedFullWallMaterial[2];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[2];
				pillar2Particles[2].SetActive(value: true);
			}
			break;
		case SpellColorType.Venom:
			srs.material = pillarColor[3];
			pillarParticles[3].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[3];
				fuseWallSprite.material = fusedWallMaterial[3];
				fuseWallSprite2.material = fusedFullWallMaterial[3];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[3];
				pillar2Particles[3].SetActive(value: true);
			}
			break;
		case SpellColorType.Fire:
			srs.material = pillarColor[4];
			pillarParticles[4].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[4];
				fuseWallSprite.material = fusedWallMaterial[4];
				fuseWallSprite2.material = fusedFullWallMaterial[4];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[4];
				pillar2Particles[4].SetActive(value: true);
			}
			isFirePillar = true;
			break;
		case SpellColorType.Void:
			srs.material = pillarColor[5];
			pillarParticles[5].SetActive(value: true);
			if (FusionData.IsFusedUnit)
			{
				srs2.material = pillarColor[5];
				fuseWallSprite.material = fusedWallMaterial[5];
				fuseWallSprite2.material = fusedFullWallMaterial[5];
				fuseWallParticle.GetComponent<Renderer>().material = fuseWallParticleMaterial[5];
				pillar2Particles[5].SetActive(value: true);
			}
			break;
		}
		srs.material.SetInt(UseGhostEffect, 0);
		srs.material.SetInt(UseFuseShineEffect, 0);
		srs.material.SetFloat(FuseShineProcess, 0f);
		firePillarOutlineSprite.material.SetFloat(FuseShineProcess, 0f);
		GeneralTool.InitialSpriteMaterial(srs);
		GeneralTool.InitialSpriteMaterial(firePillarOutlineSprite);
		selfShadow.ShadowGO.SetActive(value: true);
		if (FusionData.IsFusedUnit)
		{
			srs2.material.SetInt(UseGhostEffect, 0);
			srs2.material.SetInt(UseFuseShineEffect, 0);
			srs2.material.SetFloat(FuseShineProcess, 0f);
			fuseWallSprite.material.SetInt(UseGhostEffect, 0);
			firePillar2OutlineSprite.material.SetFloat(FuseShineProcess, 0f);
			GeneralTool.InitialSpriteMaterial(srs2);
			GeneralTool.InitialSpriteMaterial(firePillar2OutlineSprite);
			GeneralTool.InitialSpriteMaterial(fuseWallSprite);
			GeneralTool.InitialSpriteMaterial(fuseWallSprite2);
			fuseWallParticle.Stop();
			UpdateFuseWallPosition();
			fuseWallParticle.Play();
		}
		applyDebuffCounter = ApplyDebuffToRoundMonsterInterval;
		applyVenomCounter = ApplyVenomToRoundMonsterInterval;
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && base.SummonerSpellBase.SIP.radiuDecreaseRatio < 1f)
		{
			float spellRadiusToDamageRatio = GeneralTool.GetSpellRadiusToDamageRatio(GetHeavyCrashRange(), base.SummonerSpellBase.SIP.radiuDecreaseRatio, base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio);
			base.SummonerSpellBase.damageRatio += spellRadiusToDamageRatio;
		}
	}

	public float GetHeavyCrashRange()
	{
		return 1.5f * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
	}

	private void UpdateFuseWallPosition(Vector2 dir = default(Vector2))
	{
		Vector2 vector = (wallDirection = ((dir == default(Vector2)) ? ((Vector2)Tool2D.GetDir()) : dir.normalized));
		float value = Tool2D.GetDegree(vector) - 90f;
		tsf_FuseWall.transform.localPosition = new Vector3(0f, 0.5f, 0f);
		fuseWallSprite.material.SetFloat(RotateAngle, value);
		fuseWallSprite2.material.SetFloat(RotateAngle, value);
		tsf_Pillars.transform.localPosition = -vector * fuseWallDistance / 2f;
		tsf_Pillar2.transform.localPosition = vector * fuseWallDistance / 2f;
		base.transform.right = wallDirection;
		rotateTrans.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f - base.transform.eulerAngles.z));
		ParticleSystem.ShapeModule shape = fuseWallParticle.shape;
		shape.rotation = new Vector3(0f, 0f, base.transform.eulerAngles.z);
		float num = base.transform.localScale.x * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		attackSize.transform.right = vector;
		attackSize.transform.localScale = new Vector3((fuseWallDistance + 1.5f) * num, 1.5f * num, 10f);
	}

	private void UpdateFloatingFusingWallShadowPosition()
	{
		if (FusionData.IsFusedUnit)
		{
			fusedWallLineShadow.SetPosition(0, Tool2D.IgnoreZPoint(selfShadow.ShadowGO.transform.position) + new Vector3(0f, 0f, 900f));
			fusedWallLineShadow.SetPosition(1, Tool2D.IgnoreZPoint(selfShadow2.ShadowGO.transform.position) + new Vector3(0f, 0f, 900f));
		}
	}

	private void UpdateFloatingFusingWallShadowPosition(float height)
	{
		if (FusionData.IsFusedUnit)
		{
			selfShadow.ShadowGO.transform.position = Tool2D.IgnoreZPoint(tsf_Pillars.transform.position) + new Vector3(0f, 0f - height, 900f);
			selfShadow2.ShadowGO.transform.position = Tool2D.IgnoreZPoint(tsf_Pillar2.transform.position) + new Vector3(0f, 0f - height, 900f);
		}
	}

	public override void Update()
	{
		base.Update();
		DebuffUpdate();
		UpdatAttackTimer();
		UpdateHeavyCrashTimer();
		if (FusionData.IsFusedUnit && (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation || base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy || base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.ChaseMouse))
		{
			UpdateFuseWallPosition(base.SummonerSpellBase.Direction);
		}
		if (base.CanMove)
		{
			switch (base.SummonerSpellBase.currentSpellMovement)
			{
			case SpellSpecialMovementType.ChaseEnemy:
				if (base.SummonerSpellBase.spellFollowTargetPpt != null && base.SummonerSpellBase.spellFollowTargetPpt.isActiveAndEnabled)
				{
					base.SummonerSpellBase.Direction = Tool2D.DirMoveTowards(base.SummonerSpellBase.Direction, ToPointDir(base.SummonerSpellBase.spellFollowTargetPpt.transform), base.SummonerSpellBase.CurrentSpeed * base.SummonerSpellBase.spellFollowTargetRotateSpeed * Time.deltaTime);
					base.Rigid.linearVelocity = base.SummonerSpellBase.Direction * base.SummonerSpellBase.CurrentSpeed;
				}
				else
				{
					base.SummonerSpellBase.spellFollowTargetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetRandomTargetablePpt();
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
			{
				Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
				base.SummonerSpellBase.Direction = Vector3.Lerp(base.SummonerSpellBase.Direction, ToPointDir(mousePoint).normalized, base.SummonerSpellBase.CurrentSpeed * Time.deltaTime * base.SummonerSpellBase.spellFollowMouseLerp * chaseMouseLerpSpeedRatio);
				base.Rigid.linearVelocity = base.SummonerSpellBase.Direction * base.SummonerSpellBase.CurrentSpeed;
				break;
			}
			case SpellSpecialMovementType.Rotation:
			{
				float num = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / GetSummonUnitRealMoveSpeed()) * Time.deltaTime;
				base.SummonerSpellBase.spellAroundOwnerCurrentAngle += num;
				base.SummonerSpellBase.Direction = Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle + 90f);
				Vector3 v = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
				base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
				base.SummonerSpellBase.SpellAroundPlayerUpdateMoveTrigger(num);
				break;
			}
			}
		}
		selfHealingCounter += Time.deltaTime;
		if (selfHealingCounter > 1f)
		{
			selfHealingCounter -= 1f;
			if (myPpt.unitCfg.currentHP < myPpt.unitCfg.maxHP)
			{
				myPpt.unitCfg.currentHP = Mathf.Min(myPpt.unitCfg.maxHP, Mathf.CeilToInt(myPpt.unitCfg.currentHP + base.SummonerSpellBase.spellCfg.float1));
			}
		}
		if (base.CurrentHPRatio < sprite3HPRatio)
		{
			if (!(srs.sprite != sprite3))
			{
				return;
			}
			srs.sprite = sprite3;
			if (FusionData.IsFusedUnit)
			{
				srs2.sprite = sprite3;
				fuseWallSprite.sprite = fuseWallDamageSprites[2];
				fuseWallSprite2.sprite = fuseWallDamageSprites2[2];
			}
			if (isFirePillar)
			{
				firePillarOutlineSprite.sprite = pillarDamageStageSprite[2];
				if (FusionData.IsFusedUnit)
				{
					firePillar2OutlineSprite.sprite = pillarDamageStageSprite[2];
				}
			}
		}
		else if (base.CurrentHPRatio < sprite2HPRatio)
		{
			if (!(srs.sprite != sprite2))
			{
				return;
			}
			srs.sprite = sprite2;
			if (FusionData.IsFusedUnit)
			{
				srs2.sprite = sprite2;
				fuseWallSprite.sprite = fuseWallDamageSprites[1];
				fuseWallSprite2.sprite = fuseWallDamageSprites2[1];
			}
			if (isFirePillar)
			{
				firePillarOutlineSprite.sprite = pillarDamageStageSprite[1];
				if (FusionData.IsFusedUnit)
				{
					firePillar2OutlineSprite.sprite = pillarDamageStageSprite[1];
				}
			}
		}
		else
		{
			if (!(srs.sprite != sprite1))
			{
				return;
			}
			srs.sprite = sprite1;
			if (FusionData.IsFusedUnit)
			{
				srs2.sprite = sprite1;
				fuseWallSprite.sprite = fuseWallDamageSprites[0];
				fuseWallSprite2.sprite = fuseWallDamageSprites2[0];
			}
			if (isFirePillar)
			{
				firePillarOutlineSprite.sprite = pillarDamageStageSprite[0];
				if (FusionData.IsFusedUnit)
				{
					firePillar2OutlineSprite.sprite = pillarDamageStageSprite[0];
				}
			}
		}
	}

	private void DebuffUpdate()
	{
		applyDebuffCounter += Time.deltaTime;
		if (applyDebuffCounter >= ApplyDebuffToRoundMonsterInterval)
		{
			applyDebuffCounter = 0f;
			ApplyDebuffToRoundMonster(base.SummonerSpellBase.spellCfg.float2);
		}
		applyVenomCounter += Time.deltaTime;
		if (applyVenomCounter >= ApplyVenomToRoundMonsterInterval)
		{
			applyVenomCounter = 0f;
			ApplyVenomToRoundMonster(base.SummonerSpellBase.spellCfg.float2);
		}
	}

	private void UpdatAttackTimer()
	{
		dealDamageTimer += Time.deltaTime;
		if (dealDamageTimer >= ApplyDamageInterval)
		{
			dealDamageTimer -= ApplyDamageInterval;
			ApplyDamageToEnemyInRange(dealDamageBaseRange * myPpt.tsf_Layer.localScale.x);
		}
	}

	private void LateUpdate()
	{
		if (base.SummonerSpellBase.Direction != Vector3.zero)
		{
			base.Rigid.linearVelocity = base.SummonerSpellBase.Direction * GetSummonUnitRealMoveSpeed();
		}
		UpdateFloatingFusingWallShadowPosition(0.1f);
		UpdateFloatingEffect();
		UpdateFloatingFusingWallShadowPosition();
		myPpt.bodyCenterPoint = base.transform.position;
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "DisappearFinish")
		{
			myPpt.AnnouncedDeath();
		}
		else
		{
			Debug.LogError(animaName);
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

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Wall"))
		{
			base.SummonerSpellBase.Direction = Vector3.zero;
		}
	}

	private void ApplyVenomToRoundMonster(float radius)
	{
		if (!(base.SummonerSpellBase.spellVenomTime <= 0f))
		{
			List<Collider> hitTargetsList = GetHitTargetsList(radius);
			for (int i = 0; i < hitTargetsList.Count; i++)
			{
				hitTargetsList[i].GetComponent<UnitProperty>().SetVenom(base.SummonerSpellBase.spellVenomTime, base.SummonerSpellBase.spellVenomOnceCount);
			}
		}
	}

	private void ApplyDamageToEnemyInRange(float radius)
	{
		if (base.SummonerSpellBase.spellCfg.float3 <= 0f)
		{
			return;
		}
		List<Collider> hitTargetsList = GetHitTargetsList(radius);
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo
		{
			damage = Mathf.CeilToInt(myPpt.unitCfg.maxHP * base.SummonerSpellBase.spellCfg.float3 / 100f * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra * ApplyDamageInterval),
			canRebound = false
		};
		for (int i = 0; i < hitTargetsList.Count; i++)
		{
			UnitProperty component = hitTargetsList[i].GetComponent<UnitProperty>();
			base.SummonerSpellBase.spellCfg.damage = takeDamageInfo.damage;
			base.SummonerSpellBase.OutputDamage(component, takeDamageInfo);
			if (!GameMgr.IsHarmony_Static)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", component.transform.position, 0.5f);
			}
		}
	}

	private List<Collider> GetHitTargetsList(float radiu)
	{
		if (!FusionData.IsFusedUnit)
		{
			return GeneralTool.GetCollidersByTag(base.transform.position, radiu, "Monster");
		}
		return GeneralTool.GetBoxCollidersByTag(base.transform.position, new Vector3(fuseWallDistance * base.transform.localScale.x, base.transform.localScale.x, 10f), Quaternion.Euler(new Vector3(wallDirection.x, wallDirection.y, 0f)), attackLayer, "Monster");
	}

	private List<Collider> GetEssenceHeavyHitTargetList(float EffectRange)
	{
		float num = base.transform.localScale.x * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		if (!FusionData.IsFusedUnit)
		{
			return GeneralTool.GetCollidersByTag(base.transform.position, EffectRange, "Monster", "Destructible", "SolidObj", "RollBall", "Butterfly", "Brittleness");
		}
		return GeneralTool.GetBoxCollidersByTag(base.transform.position, new Vector3((fuseWallDistance + 1.5f) * num, 1.5f * num, 10f), Quaternion.FromToRotation(Vector3.right, (Vector3)wallDirection), attackLayer, "Monster", "Destructible", "SolidObj", "RollBall", "Butterfly", "Brittleness");
	}

	private void ApplyDebuffToRoundMonster(float radius)
	{
		List<Collider> hitTargetsList = GetHitTargetsList(radius);
		for (int i = 0; i < hitTargetsList.Count; i++)
		{
			UnitProperty component = hitTargetsList[i].GetComponent<UnitProperty>();
			if (base.SummonerSpellBase.spellFrozenTime > 0f)
			{
				component.SetFrozen(base.SummonerSpellBase.spellFrozenTime);
			}
			if (base.SummonerSpellBase.spellMucusTime > 0f)
			{
				component.SetMucus(base.SummonerSpellBase.spellMucusTime, base.SummonerSpellBase.spellMucusMoveSpeedRatio, base.SummonerSpellBase.spellMucusSpellSpeedRatio);
			}
			if (base.SummonerSpellBase.burnHpRatioPerSeconds > 0f)
			{
				component.SetBurn(base.SummonerSpellBase.spellBurnTime, base.SummonerSpellBase.burnHpRatioPerSeconds);
			}
			if (base.SummonerSpellBase.voidExplosionInfo != null)
			{
				component.SetVoid(base.SummonerSpellBase.voidExplosionInfo);
			}
		}
	}
}
