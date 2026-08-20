using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate4FuseController : Teammate
{
	public enum FusePillarHpState
	{
		highHp,
		HalfHp,
		LowHp
	}

	private List<Teammate4FusePillar> fusePillarList = new List<Teammate4FusePillar>();

	private List<Teammate4FuseWall> fuseWallList = new List<Teammate4FuseWall>();

	private List<Teammate4WallHItBox> fuseWallHitBoxList = new List<Teammate4WallHItBox>();

	public Transform bodyTransform;

	public Transform hitBoxTransform;

	private int totalPillarCount;

	private float CurrentRotateAngle;

	public float fuseWallDistance;

	private FusePillarHpState currentStage;

	public float[] hpStageThreshold;

	public float chaseMouseLerpSpeedRatio;

	private bool receiveDamageInThisFrame;

	public float ApplyDebuffToRoundMonsterInterval = 0.15f;

	public float ApplyVenomToRoundMonsterInterval = 1f;

	private float applyDebuffCounter;

	private float applyVenomCounter;

	public float ApplyDamageInterval;

	public float dealDamageBaseRange;

	private float dealDamageTimer;

	public LayerMask attackLayer;

	[Header("精魄悬浮效果")]
	public float baseFloatHeight;

	public float extraFloatHeight;

	public float floatingPillarLerpSpeed;

	private float pillarFloatTimer;

	private float currentFloatingLerpSpeed;

	[Header("精魄新技能")]
	private const float heavyCrashActiveInterval = 2.5f;

	private const float heavyCrashBaseRange = 1.5f;

	public float heavyCrashHpDamageRatio;

	public float heavyCrashDamageRatioUpPerLevel;

	public float heavyCrashPullForce;

	private float heavyCrashTimer;

	private float heavyCrashReckeckTimer;

	public Transform BehitTransform;

	private new float CurrentHPRatio => myPpt.unitCfg.currentHP / myPpt.unitCfg.maxHP;

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		ColliderToggle(state: true);
		pillarFloatTimer = 0f;
		currentFloatingLerpSpeed = 0f;
		heavyCrashTimer = 0f;
		heavyCrashReckeckTimer = 0f;
		ShowTeammate();
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		foreach (Teammate4FuseWall fuseWall in fuseWallList)
		{
			fuseWall.GetComponent<Teammate4FuseWall>().ShowTeammate();
		}
		foreach (Teammate4FusePillar fusePillar in fusePillarList)
		{
			fusePillar.GetComponent<Teammate4FusePillar>().ShowTeammate();
		}
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		base.Anima.speed = 1f;
		BehitTransform.localPosition = new Vector3(0f, 0f, -0.25f);
		foreach (Teammate4FuseWall fuseWall in fuseWallList)
		{
			fuseWall.GetComponent<Teammate4FuseWall>().HideTeammate();
		}
		foreach (Teammate4FusePillar fusePillar in fusePillarList)
		{
			fusePillar.GetComponent<Teammate4FusePillar>().HideTeammate();
		}
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
		if (base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f)
		{
			return;
		}
		foreach (Teammate4FusePillar fusePillar in fusePillarList)
		{
			fusePillar.OnEnterDelayDeathEvent();
		}
		foreach (Teammate4FuseWall fuseWall in fuseWallList)
		{
			fuseWall.OnEnterDelayDeathEvent();
		}
		SummonGhostEffectToggle(state: true);
		ColliderToggle(state: false);
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		foreach (Teammate4FusePillar fusePillar in fusePillarList)
		{
			fusePillar.OnEnterFuseStateEvent();
		}
		foreach (Teammate4FuseWall fuseWall in fuseWallList)
		{
			fuseWall.OnEnterFuseStateEvent();
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
		UpdatePillarHpState();
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
		case SpellSpecialMovementType.Normal:
			break;
		}
	}

	private void LateUpdate()
	{
		if (base.SummonerSpellBase.Direction != Vector3.zero)
		{
			base.Rigid.linearVelocity = base.SummonerSpellBase.Direction * GetSummonUnitRealMoveSpeed();
		}
		receiveDamageInThisFrame = false;
		UpdateFloatingEffect(base.SummonerSpellBase.Direction);
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
		float num = base.transform.localScale.x * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		if (GetHitTargetsList(new Vector2((fuseWallDistance + 1.5f) * num, 1.5f * num)).Count <= 0)
		{
			return;
		}
		heavyCrashTimer = 0f;
		currentFloatingLerpSpeed = 0f;
		for (int i = 0; i < totalPillarCount; i++)
		{
			fusePillarList[i].bodyRoot.DOLocalMove(new Vector3(0f, 0f, 0f), 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
			if (totalPillarCount == 2 && i == 1)
			{
				break;
			}
			fuseWallList[i].bodyRoot.DOLocalMove(new Vector3(0f, 0f, 0f), 0.05f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio).SetEase(Ease.OutCubic);
		}
		StartCoroutine(HeavyCrashDealDamageAndPullForceToAllEnemyInRange(0.03f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio));
	}

	private IEnumerator HeavyCrashDealDamageAndPullForceToAllEnemyInRange(float time)
	{
		yield return new WaitForSeconds(time);
		SEMgr.Inst.teammate4HeavyAttack.PlaySE();
		float num = 1.5f * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		List<Collider> essenceHeavyHitTargetList = GetEssenceHeavyHitTargetList();
		int num2 = Mathf.CeilToInt(myPpt.unitCfg.maxHP * (heavyCrashHpDamageRatio * (1f + (float)(base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level - 1) / 2f)) * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra * 0.5f);
		for (int i = 0; i < totalPillarCount; i++)
		{
			base.SummonerSpellBase.GetEffect("HeavyCrashEffect_" + base.SummonerSpellBase.ColorType, Tool2D.IgnoreZPoint(fusePillarList[i].transform.position), 0.8f).transform.localScale = Vector3.one * num;
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
				base.SummonerSpellBase.ApplyElementEffect(component);
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

	private List<Collider> GetEssenceHeavyHitTargetList()
	{
		float num = base.transform.localScale.x * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
		List<Collider> list = new List<Collider>();
		foreach (Teammate4WallHItBox fuseWallHitBox in fuseWallHitBoxList)
		{
			if (fuseWallHitBox.gameObject.activeInHierarchy)
			{
				Vector2 vector = new Vector2((fuseWallDistance + 1.5f) * num, 1.5f * num);
				list = list.Union(GeneralTool.GetBoxCollidersByTag(Tool2D.IgnoreZPoint(fuseWallHitBox.transform.position), new Vector3(vector.x, vector.y, 10f), Quaternion.Euler(Tool2D.IgnoreZPoint(Tool2D.GetDir(fuseWallHitBox.transform.right, 90f))), attackLayer, "Monster", "Destructible", "SolidObj", "RollBall", "Butterfly", "Brittleness")).ToList();
				continue;
			}
			return list;
		}
		return list;
	}

	private void UpdateFloatingEffect(Vector2 dir = default(Vector2))
	{
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level <= 0)
		{
			return;
		}
		pillarFloatTimer += Time.deltaTime;
		float num = baseFloatHeight + extraFloatHeight * Mathf.Sin(pillarFloatTimer * 2f);
		for (int i = 0; i < totalPillarCount; i++)
		{
			fusePillarList[i].bodyRoot.localPosition = Vector3.Lerp(fusePillarList[i].bodyRoot.localPosition, new Vector3(0f, num, 0f), currentFloatingLerpSpeed * Time.deltaTime);
			fusePillarList[i].selfShadow.ShadowGO.transform.localScale = Vector3.one * fusePillarList[i].selfShadow.shadowScale * (1f - num * 0.5f);
			if (totalPillarCount != 2 || i != 1)
			{
				fuseWallList[i].bodyRoot.localPosition = Vector3.Lerp(fuseWallList[i].bodyRoot.localPosition, new Vector3(0f, num, 0f), currentFloatingLerpSpeed * Time.deltaTime);
				continue;
			}
			break;
		}
	}

	private void UpdatAttackTimer()
	{
		dealDamageTimer += Time.deltaTime;
		if (dealDamageTimer >= ApplyDamageInterval)
		{
			dealDamageTimer -= ApplyDamageInterval;
			ApplyDamageToEnemyInRange();
		}
	}

	private void ApplyDamageToEnemyInRange()
	{
		if (base.SummonerSpellBase.spellCfg.float3 <= 0f)
		{
			return;
		}
		List<Collider> hitTargetsList = GetHitTargetsList();
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

	private void DebuffUpdate()
	{
		applyDebuffCounter += Time.deltaTime;
		if (applyDebuffCounter >= ApplyDebuffToRoundMonsterInterval)
		{
			applyDebuffCounter = 0f;
			ApplyDebuffToRoundMonster();
		}
		applyVenomCounter += Time.deltaTime;
		if (applyVenomCounter >= ApplyVenomToRoundMonsterInterval)
		{
			applyVenomCounter = 0f;
			ApplyVenomToRoundMonster(base.SummonerSpellBase.spellCfg.float2);
		}
	}

	private void ApplyDebuffToRoundMonster()
	{
		List<Collider> hitTargetsList = GetHitTargetsList();
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

	private void ApplyVenomToRoundMonster(float radius)
	{
		if (!(base.SummonerSpellBase.spellVenomTime <= 0f))
		{
			List<Collider> hitTargetsList = GetHitTargetsList();
			for (int i = 0; i < hitTargetsList.Count; i++)
			{
				hitTargetsList[i].GetComponent<UnitProperty>().SetVenom(base.SummonerSpellBase.spellVenomTime, base.SummonerSpellBase.spellVenomOnceCount);
			}
		}
	}

	private List<Collider> GetHitTargetsList(Vector2 boxSize = default(Vector2))
	{
		List<Collider> list = new List<Collider>();
		foreach (Teammate4WallHItBox fuseWallHitBox in fuseWallHitBoxList)
		{
			if (fuseWallHitBox.gameObject.activeInHierarchy)
			{
				Vector2 vector = ((boxSize == default(Vector2)) ? new Vector2(fuseWallDistance * base.transform.localScale.x, base.transform.localScale.x * 0.66f) : boxSize);
				list = list.Union(GeneralTool.GetBoxCollidersByTag(Tool2D.IgnoreZPoint(fuseWallHitBox.transform.position), new Vector3(vector.x, vector.y, 10f), Quaternion.Euler(Tool2D.IgnoreZPoint(Tool2D.GetDir(fuseWallHitBox.transform.right, 90f))), attackLayer, "Monster")).ToList();
				continue;
			}
			return list;
		}
		return list;
	}

	private void UpdatePillarHpState()
	{
		FusePillarHpState fusePillarHpState = currentStage;
		fusePillarHpState = ((!(CurrentHPRatio >= hpStageThreshold[0])) ? ((CurrentHPRatio >= hpStageThreshold[1]) ? FusePillarHpState.HalfHp : FusePillarHpState.LowHp) : FusePillarHpState.highHp);
		if (fusePillarHpState != currentStage)
		{
			currentStage = fusePillarHpState;
			for (int i = 0; i < totalPillarCount; i++)
			{
				fusePillarList[i].UpdatePillarDamagePercent(currentStage);
			}
			int num = Mathf.Min(totalPillarCount, fuseWallList.Count);
			for (int j = 0; j < num; j++)
			{
				fuseWallList[j].UpdatePillarDamagePercent(currentStage);
			}
		}
	}

	private void SpawnPillarAndWall()
	{
		if (fusePillarList.Count < totalPillarCount)
		{
			int num = totalPillarCount - fusePillarList.Count;
			for (int i = 0; i < num; i++)
			{
				Teammate4FusePillar component = SpawnFusePillarObject(base.transform.position, bodyTransform).GetComponent<Teammate4FusePillar>();
				fusePillarList.Add(component);
			}
		}
		if (fuseWallList.Count < totalPillarCount)
		{
			int num2 = ((totalPillarCount > 2) ? (totalPillarCount - fuseWallList.Count) : (totalPillarCount - fuseWallList.Count - 1));
			for (int j = 0; j < num2; j++)
			{
				Teammate4FuseWall component2 = SpawnFuseWallObject(base.transform.position, bodyTransform).GetComponent<Teammate4FuseWall>();
				fuseWallList.Add(component2);
			}
		}
		if (fuseWallHitBoxList.Count < totalPillarCount)
		{
			int num3 = ((totalPillarCount > 2) ? (totalPillarCount - fuseWallHitBoxList.Count) : (totalPillarCount - fuseWallHitBoxList.Count - 1));
			for (int k = 0; k < num3; k++)
			{
				Teammate4WallHItBox component3 = SpawnFuseWallHitBoxObject(base.transform.position, bodyTransform).GetComponent<Teammate4WallHItBox>();
				component3.mainBody = this;
				fuseWallHitBoxList.Add(component3);
			}
		}
		foreach (Teammate4FusePillar fusePillar in fusePillarList)
		{
			fusePillar.gameObject.SetActive(value: false);
		}
		foreach (Teammate4FuseWall fuseWall in fuseWallList)
		{
			fuseWall.gameObject.SetActive(value: false);
		}
		foreach (Teammate4WallHItBox fuseWallHitBox in fuseWallHitBoxList)
		{
			fuseWallHitBox.gameObject.SetActive(value: false);
		}
		float pillarDistanceToCenterPoint = GetPillarDistanceToCenterPoint();
		float num4 = ((totalPillarCount > 2) ? GetWallDistanceToCenterPoint(pillarDistanceToCenterPoint) : 0f);
		float num5 = 360f / (float)totalPillarCount;
		float num6 = num5 / 2f;
		for (int l = 0; l < totalPillarCount; l++)
		{
			float num7 = CurrentRotateAngle + num5 * (float)l;
			fusePillarList[l].gameObject.SetActive(value: true);
			fusePillarList[l].bodyRoot.localPosition = Vector3.zero;
			fusePillarList[l].transform.localPosition = Tool2D.GetDir(num7) * pillarDistanceToCenterPoint;
			fusePillarList[l].transform.localPosition += new Vector3(0f, 0f, fusePillarList[l].transform.localPosition.y);
			SetPillarOutlook(fusePillarList[l]);
			if (totalPillarCount != 2 || l != 1)
			{
				float num8 = num7 + num6;
				fuseWallList[l].gameObject.SetActive(value: true);
				fuseWallList[l].bodyRoot.localPosition = Vector3.zero;
				fuseWallList[l].transform.localPosition = Tool2D.GetDir(num7 + num6) * num4;
				fuseWallList[l].transform.localPosition += new Vector3(0f, 0f, fuseWallList[l].transform.localPosition.y);
				SetWallOutlook(fuseWallList[l], num8);
				fuseWallHitBoxList[l].gameObject.SetActive(value: true);
				fuseWallHitBoxList[l].transform.localPosition = fuseWallList[l].transform.localPosition;
				fuseWallHitBoxList[l].transform.right = Tool2D.GetDir(num8 + 90f);
				fuseWallHitBoxList[l].myPpt.unitCfg.maxHP = myPpt.unitCfg.maxHP;
				fuseWallHitBoxList[l].myPpt.unitCfg.currentHP = myPpt.unitCfg.currentHP;
				continue;
			}
			break;
		}
	}

	private void UpdateFuseWallPosition(Vector2 dir = default(Vector2))
	{
		float pillarDistanceToCenterPoint = GetPillarDistanceToCenterPoint();
		float num = ((totalPillarCount > 2) ? GetWallDistanceToCenterPoint(pillarDistanceToCenterPoint) : 0f);
		float num2 = 360f / (float)totalPillarCount;
		float num3 = num2 / 2f;
		Vector2 dir2 = ((dir == default(Vector2)) ? ((Vector2)Tool2D.GetDir()) : dir.normalized);
		CurrentRotateAngle = Tool2D.GetDegree(dir2);
		for (int i = 0; i < totalPillarCount; i++)
		{
			float num4 = CurrentRotateAngle + num2 * (float)i;
			fusePillarList[i].transform.localPosition = Tool2D.GetDir(num4) * pillarDistanceToCenterPoint;
			fusePillarList[i].transform.localPosition += new Vector3(0f, 0f, fusePillarList[i].transform.localPosition.y);
			if (totalPillarCount != 2 || i != 1)
			{
				float num5 = num4 + num3;
				fuseWallList[i].transform.localPosition = Tool2D.GetDir(num4 + num3) * num;
				fuseWallList[i].transform.localPosition += new Vector3(0f, 0f, fuseWallList[i].transform.localPosition.y);
				fuseWallList[i].UpdataWallAngle(num5);
				fuseWallHitBoxList[i].transform.localPosition = Tool2D.GetDir(num4 + num3) * num;
				fuseWallHitBoxList[i].transform.right = Tool2D.GetDir(num5 + 90f);
				continue;
			}
			break;
		}
	}

	private float GetPillarDistanceToCenterPoint()
	{
		if (totalPillarCount == 2)
		{
			return fuseWallDistance / 2f;
		}
		return fuseWallDistance / 2f / Mathf.Sin(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f));
	}

	private float GetWallDistanceToCenterPoint(float toPillarDistance)
	{
		if (totalPillarCount == 2)
		{
			return fuseWallDistance / 2f;
		}
		return toPillarDistance * Mathf.Cos(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f));
	}

	private void SetPillarOutlook(Teammate4FusePillar pillar)
	{
		pillar.PillarInitialize(base.SummonerSpellBase.ColorType);
		pillar.UpdatePillarDamagePercent(currentStage);
	}

	private void SetWallOutlook(Teammate4FuseWall wall, float initialAngle)
	{
		wall.initialAngle = initialAngle;
		wall.WallInitialize(base.SummonerSpellBase.ColorType);
		wall.UpdataWallAngle(initialAngle);
		wall.RestartWallParticle();
		wall.UpdatePillarDamagePercent(currentStage);
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		totalPillarCount = FusionData.CurrentFusionLevel + 1;
		CurrentRotateAngle = UnityEngine.Random.Range(0f, 360f);
		receiveDamageInThisFrame = false;
		currentFloatingLerpSpeed = floatingPillarLerpSpeed;
		heavyCrashTimer = 2.5f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio - 0.6f;
		pillarFloatTimer = UnityEngine.Random.Range(0f, 10f);
		currentFloatingLerpSpeed = floatingPillarLerpSpeed;
		SpawnPillarAndWall();
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && base.SummonerSpellBase.SIP.radiuDecreaseRatio < 1f)
		{
			float num = 1f + GeneralTool.GetSpellRadiusToDamageRatio(GetHeavyCrashRange(), base.SummonerSpellBase.SIP.radiuDecreaseRatio, base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio);
			base.SummonerSpellBase.finalDamageRatio *= num;
		}
	}

	public float GetHeavyCrashRange()
	{
		return 1.5f * (1f + base.SummonerSpellBase.SIP.extraSizeRatio) * base.SummonerSpellBase.SIP.finalSizeRatio;
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

	public void TakeDamageFromHitBox(TakeDamageInfo info)
	{
		if (!receiveDamageInThisFrame)
		{
			if ((bool)info.spellBase)
			{
				myPpt.TakeDamage(info.spellBase, info);
			}
			else
			{
				myPpt.TakeDamage(info.damage, AttackerType.NothingSpecial);
			}
			receiveDamageInThisFrame = true;
		}
	}

	private GameObject SpawnFusePillarObject(Vector3 position, Transform parent)
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 20041 + "/" + 20041 + "_FusePillar"), position, quaternion.identity, parent);
	}

	private GameObject SpawnFuseWallObject(Vector3 position, Transform parent)
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 20041 + "/" + 20041 + "_FuseWall"), position, quaternion.identity, parent);
	}

	private GameObject SpawnFuseWallHitBoxObject(Vector3 position, Transform parent)
	{
		return PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Units/" + 700412, position, hitBoxTransform);
	}
}
