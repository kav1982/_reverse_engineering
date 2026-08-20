using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate2 : Teammate
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		RunToTarget
	}

	public Shadow selfShadow;

	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	public float attackInterval;

	[Header("Leg")]
	public Teammate2_Leg pfb_Leg;

	public Transform tsf_Motion;

	public float legCheckInterval;

	public float legCancelTargetLengthRatio;

	[Range(0f, 1f)]
	[Header("Body Move")]
	public float[] initialHPRatio;

	public float minBodySize;

	public float maxBodySize;

	public float minMoveRatio;

	public float maxMoveRatio;

	[Header("Color")]
	public SpriteRenderer sr;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public GameObject fireEffect;

	[Header("Safe Mode")]
	public Sprite originSprite;

	public Sprite originFuseHeadSprite;

	public Sprite safeModeSprite;

	public Sprite safeFuseHeadSprite;

	public Sprite originSpriteVoid;

	public Sprite safeModeSpriteVoid;

	private Vector3 lastFramePosition = Vector3.zero;

	private MonsterState state;

	private float idleTimer;

	private float idleWalkTimer;

	private float checkLegTargetIntervalTimer;

	private UnitProperty[] legTargetPpt;

	private List<int> noAttackLegIndexs = new List<int>();

	[HideInInspector]
	public Spell3110LifeLine targetline;

	public float lifeLineHeightShift;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public Transform headCenterTransform;

	private UnitProperty essenceLegsTarget;

	public AnimationCurve essenceLegAttackLerpCurve;

	private int essenceLegGroupCount;

	public Vector3 lastFrameMovement { get; set; } = Vector3.zero;


	public bool floatingBationMode { get; private set; }

	public float FinalMoveSpeed
	{
		get
		{
			if (!floatingBationMode)
			{
				return GetSummonUnitRealMoveSpeed() * Mathf.Lerp(maxMoveRatio, minMoveRatio, myPpt.unitCfg.currentHP / myPpt.unitCfg.maxHP);
			}
			return GetSummonUnitRealMoveSpeed();
		}
	}

	public Teammate2_Leg[] legs { get; private set; }

	public List<Teammate2_Leg> essenceLegs { get; private set; } = new List<Teammate2_Leg>();


	protected override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
		floatingBationMode = false;
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			if ((bool)base.SummonerSpellBase && base.SummonerSpellBase.ColorType == SpellColorType.Void)
			{
				sr.sprite = safeModeSpriteVoid;
			}
			else
			{
				sr.sprite = safeModeSprite;
			}
		}
		else if ((bool)base.SummonerSpellBase && base.SummonerSpellBase.ColorType == SpellColorType.Void)
		{
			sr.sprite = originSpriteVoid;
		}
		else
		{
			sr.sprite = originSprite;
		}
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		state = MonsterState.BornIdle;
		idleTimer = 0f;
		idleWalkTimer = 0f;
		checkLegTargetIntervalTimer = 0f;
		essenceLegsTarget = null;
		essenceLegGroupCount = 0;
		StartRecycleLegs();
		noAttackLegIndexs = new List<int>();
		base.Anima.SetTrigger("normal");
		ShowTeammate();
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		Teammate2_Leg[] array = legs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].HideLegs();
		}
		foreach (Teammate2_Leg essenceLeg in essenceLegs)
		{
			essenceLeg.HideLegs();
		}
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		if (legs != null && legs.Length != 0)
		{
			Teammate2_Leg[] array = legs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ShowLegs();
			}
		}
		if (essenceLegs != null && essenceLegs.Count > 0)
		{
			foreach (Teammate2_Leg essenceLeg in essenceLegs)
			{
				essenceLeg.ShowLegs();
			}
		}
		selfShadow.ShadowGO.SetActive(value: true);
	}

	public void ControldByTeammate6()
	{
		base.Anima.SetTrigger("stop");
		base.CanMove = false;
		ColliderToggle(state: false);
		base.beingControlledByTeammate6 = true;
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.beingControlledByTeammate6 = false;
			base.Anima.SetTrigger("normal");
			base.CanMove = true;
			ShowTeammate();
		}
	}

	private void StartRecycleLegs()
	{
		if (legs != null && legs.Length != 0)
		{
			for (int i = 0; i < legs.Length; i++)
			{
				if (legs[i] != null)
				{
					legs[i].gameObject.SetActive(value: false);
					UnityEngine.Object.Destroy(legs[i].gameObject);
				}
			}
			for (int j = 0; j < legTargetPpt.Length; j++)
			{
				if (legTargetPpt[j] != null)
				{
					legTargetPpt[j] = null;
				}
			}
		}
		if (essenceLegs.Count > 0)
		{
			essenceLegs.Reverse();
			foreach (Teammate2_Leg essenceLeg in essenceLegs)
			{
				essenceLeg.gameObject.SetActive(value: false);
				UnityEngine.Object.Destroy(essenceLeg.gameObject);
			}
		}
		essenceLegs.Clear();
	}

	private void SpawnLegs()
	{
		for (int i = 0; i < base.SummonerSpellBase.spellCfg.int1; i++)
		{
			legs[i] = UnityEngine.Object.Instantiate(pfb_Leg, base.transform);
			legs[i].gameObject.SetActive(value: false);
			legs[i].legIndex = i;
			legTargetPpt[i] = null;
			if (base.SummonerSpellBase.spellCfg.int1 % 4 == 0)
			{
				if (i % (base.SummonerSpellBase.spellCfg.int1 / 4) == 0)
				{
					noAttackLegIndexs.Add(i);
				}
			}
			else if (i > 0 && i % (base.SummonerSpellBase.spellCfg.int1 / 4) == 0)
			{
				noAttackLegIndexs.Add(i);
			}
			legs[i].gameObject.SetActive(value: true);
		}
		if (floatingBationMode)
		{
			int num = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 2;
			essenceLegGroupCount = Mathf.Min(13, num);
			for (int j = 0; j < num; j++)
			{
				Teammate2_Leg teammate2_Leg = UnityEngine.Object.Instantiate(pfb_Leg, base.transform);
				teammate2_Leg.gameObject.SetActive(value: false);
				teammate2_Leg.legIndex = j;
				essenceLegs.Add(teammate2_Leg);
				teammate2_Leg.gameObject.SetActive(value: true);
			}
		}
	}

	private GameObject SpawnBodyObject(Vector3 position, Transform parent)
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 20021 + "/" + 20021 + "_FuseHead"), position, quaternion.identity, parent);
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		legTargetPpt = new UnitProperty[base.SummonerSpellBase.spellCfg.int1];
		legs = new Teammate2_Leg[base.SummonerSpellBase.spellCfg.int1];
		lastFramePosition = base.transform.position;
		floatingBationMode = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0;
		SpawnLegs();
		float num = 1f;
		float damageUpEffectValue = GetDamageUpEffectValue();
		num += damageUpEffectValue / 100f;
		base.SummonerSpellBase.spellCfg.damage = Mathf.CeilToInt((SpellConfig.dic[base.SummonerSpellBase.spellCfg.id].damage + damageUpEffectValue) * attackInterval * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.InitialParameter.finalDamageExtra * attackInterval * num * 5f);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * initialHPRatio[base.SummonerSpellBase.InitialWithConfig.level - 1];
		UpdateBodySize();
		fireEffect.SetActive(value: false);
		switch (base.SummonerSpellBase.ColorType)
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
		default:
			Debug.LogError(base.SummonerSpellBase.ColorType);
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		}
		fireEffect.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		sr.material.SetInt(UseGhostEffect, 0);
		GeneralTool.InitialSpriteMaterial(sr);
		SetSafeMode();
	}

	public float GetDamageUpEffectValue()
	{
		float result = 0f;
		Wand shooterWand = base.SummonerSpellBase.shooterWand;
		if ((object)shooterWand != null && shooterWand.WandCfg != null)
		{
			result = base.SummonerSpellBase.shooterWand.MaxMP * base.SummonerSpellBase.spellCfg.float1 / 100f;
		}
		return result;
	}

	private void LateUpdate()
	{
		if (Tool2D.IgnoreZDistance(base.transform.position, lastFramePosition) <= 1f)
		{
			lastFrameMovement = base.transform.position - lastFramePosition;
		}
		else
		{
			lastFrameMovement = Vector3.zero;
		}
		lastFramePosition = base.transform.position;
	}

	public override void OnEnterDelayDeathEvent()
	{
		base.OnEnterDelayDeathEvent();
		if (base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f)
		{
			return;
		}
		sr.material.SetInt(UseGhostEffect, 1);
		SummonGhostEffectToggle(state: true);
		Teammate2_Leg[] array = legs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].lr_Leg.material.SetInt(UseGhostEffect, 1);
		}
		foreach (Teammate2_Leg essenceLeg in essenceLegs)
		{
			essenceLeg.lr_Leg.material.SetInt(UseGhostEffect, 1);
		}
		ColliderToggle(state: false);
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		sr.material.SetInt(UseFuseShineEffect, 1);
		sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
		if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
		{
			fireEffect.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		Teammate2_Leg[] array = legs;
		foreach (Teammate2_Leg obj in array)
		{
			obj.lr_Leg.material.SetInt(UseFuseShineEffect, 1);
			obj.lr_Leg.material.DOFloat(1f, FuseShineProcess, 1.3f);
			obj.lr_Shadow.gameObject.SetActive(value: false);
		}
		foreach (Teammate2_Leg essenceLeg in essenceLegs)
		{
			essenceLeg.lr_Leg.material.SetInt(UseFuseShineEffect, 1);
			essenceLeg.lr_Leg.material.DOFloat(1f, FuseShineProcess, 1.3f);
			essenceLeg.lr_Shadow.gameObject.SetActive(value: false);
			essenceLeg.EssencelegSetFuseState();
		}
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public override void Update()
	{
		SummonsTouchMonster();
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation && base.CanMove)
		{
			float num = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / GetSummonUnitRealMoveSpeed()) * Time.deltaTime;
			base.SummonerSpellBase.spellAroundOwnerCurrentAngle += num;
			Vector3 vector = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius - base.transform.position;
			base.transform.position += vector;
			base.SummonerSpellBase.SpellAroundPlayerUpdateMoveTrigger(num);
			for (int i = 0; i < legs.Length; i++)
			{
				legs[i].Theme6Reposition(vector);
			}
			foreach (Teammate2_Leg essenceLeg in essenceLegs)
			{
				essenceLeg.Theme6Reposition(vector);
				essenceLeg.LegEssenceLockingTarget();
			}
		}
		CheckTarget();
		CheckLegTarget();
		UpdataEssenceLegAttackState();
		StateMachine();
		myPpt.bodyCenterPoint = base.transform.position + tsf_Motion.transform.localPosition / 2f + new Vector3(0f, lifeLineHeightShift, 0f);
	}

	public override void LoseTarget()
	{
		base.LoseTarget();
		CancelAllLegTargets();
	}

	public override void SummonsThrough()
	{
		if (base.SummonerSpellBase.ownerPpt.unitCfg.unitType == UnitType.Player || (base.SummonerSpellBase.shooterWand != null && base.SummonerSpellBase.shooterWand.passiveAutoWand))
		{
			base.SummonerSpellBase.gameObject.SetActive(value: true);
			if (targetline != null)
			{
				targetline.gameObject.SetActive(value: true);
				targetline.resetTie();
			}
		}
		else
		{
			base.SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount = 0;
			myPpt.ClearVoidState();
		}
		base.SummonsThrough();
		if (base.SummonerSpellBase.ownerPpt.unitCfg.unitType == UnitType.Player || (base.SummonerSpellBase.shooterWand != null && base.SummonerSpellBase.shooterWand.passiveAutoWand))
		{
			base.transform.position = PlayerMgr.Inst.PlayerPoint;
			for (int i = 0; i < legs.Length; i++)
			{
				legs[i].Reposition();
				legs[i].CancelTarget();
			}
			foreach (Teammate2_Leg essenceLeg in essenceLegs)
			{
				essenceLeg.Reposition();
				essenceLeg.CancelTarget();
			}
			if (targetline != null)
			{
				targetline.gameObject.SetActive(value: true);
				targetline.resetTie();
			}
		}
		else
		{
			myPpt.AnnouncedDeath(new TakeDamageInfo
			{
				isPlayDeadSE = false,
				isCreateDeadEF = false,
				isTeammateThrough = true
			});
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		if (base.SummonerSpellBase.spellAroundOwnerRadius == 0f)
		{
			for (int i = 0; i < legs.Length; i++)
			{
				legs[i].Theme6Reposition(changeValue);
			}
			{
				foreach (Teammate2_Leg essenceLeg in essenceLegs)
				{
					essenceLeg.Theme6Reposition(changeValue);
					essenceLeg.LegEssenceLockingTarget();
				}
				return;
			}
		}
		for (int j = 0; j < legs.Length; j++)
		{
			legs[j].transform.position += changeValue;
			legs[j].Theme6Reposition(changeValue);
		}
		foreach (Teammate2_Leg essenceLeg2 in essenceLegs)
		{
			essenceLeg2.transform.position += changeValue;
			essenceLeg2.Theme6Reposition(changeValue);
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		base.AfterTakeDamage(info);
		UpdateBodySize();
	}

	private void CheckTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime * base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		if (checkTargetIntervalTimer >= 1f)
		{
			checkTargetIntervalTimer = 0f;
			GetNearestTarget();
			if (targetPpt != null)
			{
				idleTimer = 0f;
				idleWalkTimer = 0f;
				state = MonsterState.RunToTarget;
			}
		}
	}

	private void CancelAllLegTargets()
	{
		for (int i = 0; i < legTargetPpt.Length; i++)
		{
			if (legTargetPpt[i] != null)
			{
				legTargetPpt[i] = null;
				legs[i].CancelTarget();
			}
		}
		foreach (Teammate2_Leg essenceLeg in essenceLegs)
		{
			essenceLeg.CancelTarget();
		}
	}

	private void CheckHead1LegTarget()
	{
		for (int i = 0; i < legTargetPpt.Length; i++)
		{
			if (legTargetPpt[i] != null)
			{
				if (!legTargetPpt[i].gameObject.activeInHierarchy || !legTargetPpt[i].CanBeTarget)
				{
					legTargetPpt[i] = null;
					legs[i].CancelTarget();
				}
				else if ((legTargetPpt[i].transform.position - base.transform.position).sqrMagnitude > base.SummonerSpellBase.spellCfg.radius * 2f * base.SummonerSpellBase.spellCfg.radius * 2f && (legTargetPpt[i].transform.position - base.transform.position).sqrMagnitude > base.SummonerSpellBase.spellCfg.radius * base.SummonerSpellBase.spellCfg.radius * 4f * legCancelTargetLengthRatio * legCancelTargetLengthRatio)
				{
					legTargetPpt[i] = null;
					legs[i].CancelTarget();
				}
			}
		}
		List<UnitProperty> targetablePpts = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts;
		for (int j = 0; j < targetablePpts.Count; j++)
		{
			if (!targetablePpts[j].CanBeTarget || !((targetablePpts[j].transform.position - base.transform.position).sqrMagnitude < base.SummonerSpellBase.spellCfg.radius * 2f * base.SummonerSpellBase.spellCfg.radius * 2f))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k < legTargetPpt.Length; k++)
			{
				if (legTargetPpt[k] == targetablePpts[j])
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			for (int l = 0; l < legs.Length; l++)
			{
				if (!noAttackLegIndexs.Contains(l) && legTargetPpt[l] == null)
				{
					legTargetPpt[l] = targetablePpts[j];
					legs[l].SetTarget(targetablePpts[j]);
					break;
				}
			}
		}
	}

	private void CheckLegTarget()
	{
		checkLegTargetIntervalTimer += Time.deltaTime;
		if (!(checkLegTargetIntervalTimer >= legCheckInterval))
		{
			return;
		}
		checkLegTargetIntervalTimer = 0f;
		CheckHead1LegTarget();
		if (!floatingBationMode || (essenceLegsTarget != null && essenceLegsTarget.gameObject.activeInHierarchy && essenceLegsTarget.CanBeTarget && Vector3.Distance(base.transform.position, essenceLegsTarget.transform.position) < 5f))
		{
			return;
		}
		essenceLegsTarget = null;
		UnitProperty[] array = legTargetPpt;
		foreach (UnitProperty unitProperty in array)
		{
			if (unitProperty != null && unitProperty.gameObject.activeSelf && unitProperty.CanBeTarget)
			{
				essenceLegsTarget = unitProperty;
				break;
			}
		}
		foreach (Teammate2_Leg essenceLeg in essenceLegs)
		{
			if ((bool)essenceLegsTarget)
			{
				essenceLeg.SetEssenceTarget(essenceLegsTarget);
			}
			else
			{
				essenceLeg.CancelTarget();
			}
		}
	}

	private void UpdataEssenceLegAttackState()
	{
	}

	private void StateMachine()
	{
		if (!base.CanMove)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f && state == MonsterState.BornIdle)
			{
				idleTime.RandomResult();
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				state = MonsterState.IdleWalk;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				idleWalkTime.RandomResult();
			}
			break;
		case MonsterState.IdleWalk:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * FinalMoveSpeed);
				CheckNavInfo();
			}
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				state = MonsterState.Idle;
				idleTime.RandomResult();
			}
			break;
		case MonsterState.RunToTarget:
			if (base.HaveTarget)
			{
				if (ToTargetDistanceSqr() < base.SummonerSpellBase.spellCfg.radius * base.SummonerSpellBase.spellCfg.radius)
				{
					GetNavInfo(base.TargetPoint - ToTargetDir() * base.SummonerSpellBase.spellCfg.radius);
					SetMove(ToPointDir(navInfo.ToGoPoint) * FinalMoveSpeed);
				}
				else
				{
					GetNavInfo(base.TargetPoint);
					SetMove(ToPointDir(navInfo.ToGoPoint) * FinalMoveSpeed);
				}
			}
			else
			{
				state = MonsterState.Idle;
				idleTime.RandomResult();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void UpdateBodySize()
	{
		tsf_Motion.localScale = Vector3.one * Mathf.Lerp(minBodySize, maxBodySize, myPpt.unitCfg.currentHP / myPpt.unitCfg.maxHP);
	}

	public void RecoveryOnce()
	{
		myPpt.HPRecovery(Mathf.CeilToInt(base.SummonerSpellBase.spellCfg.float2));
		UpdateBodySize();
	}

	protected override Vector3 GetSummonEffectSize()
	{
		return base.GetSummonEffectSize() * 1.5f;
	}
}
