using System.Collections.Generic;
using UnityEngine;

public class Spell3110LifeLine : SpellBase
{
	private UnitProperty linkTarget1;

	private UnitProperty linkTarget2;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public LineRenderer fireLine;

	public LayerMask attackLayer;

	public float lineHeight;

	public float lineBaseWidth;

	private bool firstFrameDone;

	private int damage;

	private float lineScale = 1f;

	private SpellBase targetSpellBase;

	private bool linkStart;

	private bool lineEnding;

	public float recycleTime;

	private float distancePocess;

	public float distanceChaseLerpSpeed;

	public float linkEndChaseLerpSpeed;

	private float baseCriticalChance;

	private bool fireEnable;

	private Spell3110LivingTie OwnerTieObject;

	private Spell3110LivingTie TargetTieObject;

	[HideInInspector]
	public Wand casterWand;

	private Vector3 target2LastFramePos = Vector3.zero;

	public int abilityType;

	[HideInInspector]
	public bool autoRecycle = true;

	private bool enableCharge;

	[Header("Color")]
	public LineRenderer sr;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	private Vector3 target1Pos = Vector3.zero;

	private Vector3 target2Pos = Vector3.zero;

	private WandPostSlotChargeData chargeData;

	private float damageInterval { get; set; } = float.PositiveInfinity;


	private float intervalCounter { get; set; }

	public void Iniatialize(UnitProperty target1, UnitProperty target2, int dmg, float interval, SpellBase ownerBase)
	{
		linkTarget1 = null;
		linkTarget2 = null;
		damage = 0;
		lineScale = 1f;
		damageInterval = float.PositiveInfinity;
		intervalCounter = 0f;
		lr_Laser.enabled = false;
		lr_Shadow.enabled = false;
		targetSpellBase = null;
		linkStart = true;
		lineEnding = false;
		distancePocess = 0f;
		OwnerTieObject = null;
		TargetTieObject = null;
		firstFrameDone = false;
		ResetMaterial(lr_Laser);
		ResetMaterial(lr_Shadow);
		autoRecycle = true;
		casterWand = null;
		base.spellCfg = new SpellConfig();
		fireEnable = false;
		enableCharge = false;
		chargeData = null;
		ownerPpt = target1;
		linkTarget1 = target1;
		linkTarget2 = target2;
		target2LastFramePos = target2.transform.position;
		damage = dmg;
		base.spellCfg.damage = damage;
		lineScale = Mathf.Pow(ownerBase.damageRatio * ownerBase.finalDamageRatio, 0.3333f);
		damageInterval = interval;
		intervalCounter = 0f;
		targetSpellBase = ownerBase;
		target2LastFramePos = Vector3.zero;
		baseCriticalChance = ownerBase.GetCriticalChance();
		base.overalCriticalChance = ownerBase.GetCriticalChance();
		spellFrozenTime = ownerBase.spellFrozenTime;
		spellMucusMoveSpeedRatio = ownerBase.spellMucusMoveSpeedRatio;
		spellMucusSpellSpeedRatio = ownerBase.spellMucusSpellSpeedRatio;
		spellMucusTime = ownerBase.spellMucusTime;
		spellVenomTime = ownerBase.spellVenomTime;
		spellVenomOnceCount = ownerBase.spellVenomOnceCount;
		base.burnHpRatioPerSeconds = ownerBase.burnHpRatioPerSeconds;
		base.spellBurnTime = ownerBase.spellBurnTime;
		enableCharge = ownerBase.wandChargeData != null;
		base.criticalDragDamagePercent = ownerBase.criticalDragDamagePercent;
		base.criticalDragApllyToCount = ownerBase.criticalDragApllyToCount;
		base.criticalDragEffectRadiu = ownerBase.criticalDragEffectRadiu;
		base.criticalDragPullForce = ownerBase.criticalDragPullForce;
		sr.material = null;
		chargeData = ownerBase.wandChargeData;
		voidExplosionInfo = ownerBase.voidExplosionInfo;
		target1Pos = ((linkTarget1.bodyCenterPoint == Vector3.zero) ? linkTarget1.transform.position : linkTarget1.bodyCenterPoint);
		target2Pos = ((linkTarget2.bodyCenterPoint == Vector3.zero) ? linkTarget2.transform.position : linkTarget2.bodyCenterPoint);
		base.ColorType = ownerBase.ColorType;
		switch (ownerBase.ColorType)
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
		case SpellColorType.Player:
		case SpellColorType.Thunder:
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
			if (sr.material != mat_ECVoid)
			{
				sr.material = mat_ECVoid;
			}
			break;
		case SpellColorType.Fire:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		}
		fireEnable = ownerBase.burnHpRatioPerSeconds > 0f;
		fireLine.gameObject.SetActive(fireEnable);
		OwnerTieObject = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/31101LiveTie", base.transform.position).GetComponent<Spell3110LivingTie>();
		OwnerTieObject.TieStart(ownerBase.ColorType, fireEnable);
		OwnerTieObject.transform.position = linkTarget1.transform.position;
	}

	private new void Update()
	{
		CheckLinkTargetsState();
		DrawLine();
		if (!linkStart)
		{
			TimerCheck();
		}
		if (!firstFrameDone)
		{
			lr_Laser.startWidth = lineScale;
			lr_Shadow.startWidth = lineScale / 2f;
			fireLine.startWidth = lineScale;
			lr_Laser.enabled = true;
			lr_Shadow.enabled = true;
			firstFrameDone = true;
		}
	}

	private new void LateUpdate()
	{
		if (base.gameObject != null)
		{
			if (OwnerTieObject != null && linkTarget1 != null)
			{
				OwnerTieObject.transform.eulerAngles = Vector3.zero;
				OwnerTieObject.transform.position = target1Pos;
			}
			if (TargetTieObject != null && linkTarget2 != null)
			{
				TargetTieObject.transform.eulerAngles = Vector3.zero;
				TargetTieObject.transform.position = target2Pos;
			}
		}
	}

	private void OnDisable()
	{
		CheckLinkTargetsState();
		if (TargetTieObject != null)
		{
			PlayerMgr.Inst.MiniPool.RecycleGO(TargetTieObject.gameObject);
		}
		if (OwnerTieObject != null)
		{
			PlayerMgr.Inst.MiniPool.RecycleGO(OwnerTieObject.gameObject);
		}
	}

	private void DrawLine()
	{
		if (distancePocess < 1f && linkStart)
		{
			distancePocess = Mathf.Lerp(distancePocess, 1f, distanceChaseLerpSpeed / (distancePocess / 2f + 0.5f));
		}
		else if (distancePocess > 0f && lineEnding)
		{
			distancePocess = Mathf.Lerp(distancePocess, 0f, linkEndChaseLerpSpeed);
		}
		if (linkStart && distancePocess >= 0.95f)
		{
			distancePocess = 1f;
			linkStart = false;
			TargetTieObject = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/31101LiveTie", linkTarget2.transform.position).GetComponent<Spell3110LivingTie>();
			if ((bool)linkTarget2.GetComponent<Teammate1>() || linkTarget2.GetComponent<Teammate5>() != null || linkTarget2.GetComponent<Teammate2>() != null)
			{
				TargetTieObject.TieStart(targetSpellBase.ColorType, fireEnable, Spell3110LivingTie.lifeTieType.horizon);
				if (linkTarget2.GetComponent<Teammate2>() != null)
				{
					TargetTieObject.model.transform.localPosition = new Vector3(0f, 0f, -3f);
				}
			}
			else
			{
				TargetTieObject.TieStart(targetSpellBase.ColorType, fireEnable);
			}
			TargetTieObject.transform.position = linkTarget2.transform.position;
		}
		if (lineEnding && distancePocess <= 0.05f)
		{
			distancePocess = 0f;
			OwnerTieObject.TieEnd();
		}
		int num;
		Vector3 vector;
		if (!(linkTarget2 == null) && !linkTarget2.AlreadyDead && !(linkTarget2.gameObject == null))
		{
			num = ((!linkTarget2.gameObject.activeSelf) ? 1 : 0);
			if (num == 0)
			{
				vector = target2Pos;
				goto IL_0229;
			}
		}
		else
		{
			num = 1;
		}
		vector = target2LastFramePos;
		goto IL_0229;
		IL_0229:
		Vector3 vector2 = vector;
		Vector2 vector3 = ((num != 0) ? target2LastFramePos : linkTarget2.transform.position);
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(target1Pos + new Vector3(0f, lineHeight, 0f)));
		lr_Laser.SetPosition(1, Vector3.Lerp(lr_Laser.GetPosition(0), Tool2D.GetLayerPoint(vector2 + new Vector3(0f, lineHeight, 0f)), distancePocess));
		fireLine.SetPosition(0, Tool2D.GetLayerPoint(target1Pos + new Vector3(0f, lineHeight, 0f)) + new Vector3(0f, 0f, 0.1f));
		fireLine.SetPosition(1, Vector3.Lerp(lr_Laser.GetPosition(0), Tool2D.GetLayerPoint(vector2 + new Vector3(0f, lineHeight, 0f)), distancePocess) + new Vector3(0f, 0f, 0.1f));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(target1Pos, 1.05f));
		lr_Shadow.SetPosition(1, Vector3.Lerp(lr_Shadow.GetPosition(0), Tool2D.IgnoreZPoint(vector3, 1.05f), distancePocess));
	}

	private void TimerCheck()
	{
		intervalCounter += Time.deltaTime;
		if (intervalCounter > damageInterval)
		{
			intervalCounter -= damageInterval;
			DealDamageToTargetsBetween();
		}
	}

	public void resetTie(UnitProperty resetPpt1 = null)
	{
		if ((bool)OwnerTieObject && (bool)TargetTieObject)
		{
			OwnerTieObject.gameObject.SetActive(value: true);
			TargetTieObject.gameObject.SetActive(value: true);
		}
	}

	private void DealDamageToTargetsBetween()
	{
		if (!(linkTarget1 != null) || linkTarget1.AlreadyDead || !(linkTarget2 != null) || linkTarget2.AlreadyDead)
		{
			return;
		}
		Vector3 vector = ((linkTarget2 == null || linkTarget2.AlreadyDead || linkTarget2.gameObject == null || !linkTarget2.gameObject.activeSelf) ? target2LastFramePos : target2Pos);
		base.transform.right = linkTarget2.transform.position - linkTarget1.transform.position;
		List<Collider> boxCollidersByTag = GeneralTool.GetBoxCollidersByTag(linkTarget1.transform.position + (vector - linkTarget1.transform.position) / 2f, new Vector3(Vector3.Distance(linkTarget1.gameObject.transform.position, linkTarget2.gameObject.transform.position), lineBaseWidth * targetSpellBase.damageRatio * targetSpellBase.finalDamageRatio, 10f), Quaternion.Euler(base.transform.eulerAngles), attackLayer, "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness");
		if (boxCollidersByTag.Count > 0)
		{
			SEMgr.Inst.PlaySE("SE_Spell" + abilityType + "NormalHit");
		}
		string strSuffix = (GameMgr.IsHarmony_Static ? "HitBlood_H" : "HitBlood");
		for (int i = 0; i < boxCollidersByTag.Count; i++)
		{
			if (boxCollidersByTag[i].tag == "RollBall" || boxCollidersByTag[i].tag == "Butterfly")
			{
				SpellBase componentInParent = boxCollidersByTag[i].GetComponentInParent<SpellBase>();
				if (!componentInParent.IsSameCamp(targetSpellBase))
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						EffectController effectController = GetEffectController(strSuffix, componentInParent.transform.position + new Vector3(0f, lineHeight, 0f), 2f);
						ECSetStickEffect(effectController);
						((Spell1002RollBall)componentInParent).TakeDamage(damage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						GetEffectController(strSuffix, componentInParent.transform.position + new Vector3(0f, lineHeight, 0f), 2f);
						EffectController effectController2 = GetEffectController(strSuffix, componentInParent.transform.position + new Vector3(0f, lineHeight, 0f), 2f);
						ECSetStickEffect(effectController2);
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
					else
					{
						MonoBehaviour.print(componentInParent.spellCfg.abilityType);
					}
				}
			}
			else if (boxCollidersByTag[i].tag == "Monster")
			{
				EffectController effectController3 = GetEffectController(strSuffix, boxCollidersByTag[i].transform.position + new Vector3(0f, lineHeight, 0f), 0.6f);
				ECSetStickEffect(effectController3);
				UnitProperty component = boxCollidersByTag[i].transform.GetComponent<UnitProperty>();
				ApplyVoidEffect(component);
				TakeDamageInfo takeDamageInfo = component.TakeDamage(damage, ownerPpt, new TakeDamageInfo
				{
					criticalChance = baseCriticalChance,
					wandChargeData = targetSpellBase.wandChargeData
				});
				ApplyElementEffect(component);
				if (takeDamageInfo.isCriticalDamage && base.criticalDragDamagePercent > 0f)
				{
					base.virtualRealPosition = takeDamageInfo.beHitPpt.transform.position;
					ActivePullCrystal(takeDamageInfo.beHitPpt);
				}
				if (chargeData == null || !(casterWand != null))
				{
					continue;
				}
				WandConfig wandCfg = casterWand.WandCfg;
				if (wandCfg != null && wandCfg.PostslotSpellHitChargeRatio > 0f)
				{
					casterWand.ChargePostSlots(casterWand.WandCfg.PostslotSpellHitChargeRatio);
					continue;
				}
				if (takeDamageInfo.isTargetDead)
				{
					wandCfg = casterWand.WandCfg;
					if (wandCfg != null && wandCfg.PostslotKillEnemyChargeRatio > 0f)
					{
						casterWand.ChargePostSlots(casterWand.WandCfg.PostslotKillEnemyChargeRatio);
						continue;
					}
				}
				if (takeDamageInfo.isCriticalDamage)
				{
					wandCfg = casterWand.WandCfg;
					if (wandCfg != null && wandCfg.PostslotCriticalHitChargeRatio > 0f)
					{
						casterWand.ChargePostSlots(casterWand.WandCfg.PostslotCriticalHitChargeRatio);
					}
				}
			}
			else
			{
				boxCollidersByTag[i].transform.GetComponent<UnitProperty>().TakeDamage(damage, ownerPpt);
			}
		}
	}

	private void CheckLinkTargetsState()
	{
		if (lineEnding)
		{
			return;
		}
		if (linkTarget2 != null && !linkTarget2.AlreadyDead && linkTarget2.gameObject != null && linkTarget2.gameObject.activeSelf)
		{
			target2LastFramePos = target2Pos;
		}
		if (linkTarget1 == null || linkTarget1.AlreadyDead)
		{
			if (TargetTieObject != null)
			{
				PlayerMgr.Inst.MiniPool.RecycleGO(TargetTieObject.gameObject);
			}
			if (OwnerTieObject != null)
			{
				PlayerMgr.Inst.MiniPool.RecycleGO(OwnerTieObject.gameObject);
			}
			PlayerMgr.Inst.MiniPool.RecycleGO(base.gameObject);
		}
		else if (autoRecycle && (linkTarget2 == null || linkTarget2.AlreadyDead || linkTarget2.gameObject == null || !linkTarget2.gameObject.activeSelf) && !lineEnding)
		{
			if (TargetTieObject != null)
			{
				TargetTieObject.TieEnd();
			}
			linkTarget2 = null;
			if (OwnerTieObject != null)
			{
				OwnerTieObject.TieEnd();
			}
			linkTarget1 = null;
			linkStart = false;
			lineEnding = true;
			if (TargetTieObject != null)
			{
				PlayerMgr.Inst.MiniPool.RecycleGO(TargetTieObject.gameObject, 1f + recycleTime);
			}
			if (OwnerTieObject != null)
			{
				PlayerMgr.Inst.MiniPool.RecycleGO(OwnerTieObject.gameObject, 1f + recycleTime);
			}
			PlayerMgr.Inst.MiniPool.RecycleGO(base.gameObject, 1f + recycleTime);
		}
		else
		{
			target1Pos = ((linkTarget1.bodyCenterPoint == Vector3.zero) ? linkTarget1.transform.position : linkTarget1.bodyCenterPoint);
			target2Pos = ((linkTarget2.bodyCenterPoint == Vector3.zero) ? linkTarget2.transform.position : linkTarget2.bodyCenterPoint);
		}
	}

	public void RecycleEveryThing()
	{
		if (TargetTieObject != null)
		{
			PlayerMgr.Inst.MiniPool.RecycleGO(TargetTieObject.gameObject);
		}
		if (OwnerTieObject != null)
		{
			PlayerMgr.Inst.MiniPool.RecycleGO(OwnerTieObject.gameObject);
		}
		PlayerMgr.Inst.MiniPool.RecycleGO(base.gameObject);
	}

	private void ResetMaterial(LineRenderer lineMaterial)
	{
		Material material = lineMaterial.material;
		material = (lineMaterial.material = Object.Instantiate(material));
	}

	public void ECSetStickEffect(EffectController targetEffect)
	{
		if (targetEffect.go_ECStickFire != null && base.spellBurnTime != 0f)
		{
			bool active = base.spellBurnTime > 0f;
			targetEffect.go_ECStickFire.SetActive(active);
		}
	}
}
