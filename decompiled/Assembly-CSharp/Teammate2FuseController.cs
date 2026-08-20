using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate2FuseController : Teammate
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		RunToTarget
	}

	public class HeadData
	{
		public Teammate2FuseHead head;

		public Teammate2_FuseLeg[] legs;
	}

	public Shadow selfShadow;

	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	public float attackInterval;

	[Header("Leg")]
	public Transform tsf_Motion;

	public float legCheckInterval;

	public float legCancelTargetLengthRatio;

	[Header("Body Move")]
	[Range(0f, 1f)]
	public float[] initialHPRatio;

	public float minBodySize;

	public float maxBodySize;

	public float minMoveRatio;

	public float maxMoveRatio;

	private Vector3 lastFramePosition = Vector3.zero;

	private MonsterState state;

	private float idleTimer;

	private float idleWalkTimer;

	private float checkLegTargetIntervalTimer;

	[HideInInspector]
	public Spell3110LifeLine targetline;

	public float lifeLineHeightShift;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public Transform headCenterTransform;

	private UnitProperty essenceLegsTarget;

	public AnimationCurve essenceLegAttackLerpCurve;

	private float essenceAttackInterval;

	private float essenceAttackTimer;

	private int essenceLegGroupCount;

	private int essenceCurrentAttackingLegGroupIndex;

	private List<HeadData> bodyList = new List<HeadData>();

	public Transform bodyTransform;

	private int bodyCount;

	public float bodyDistance;

	public float legDistance;

	private List<Teammate2_FuseLeg> legs = new List<Teammate2_FuseLeg>();

	private UnitProperty[] legTargetPpt;

	public int NotAttackLegCount;

	private static readonly int fuseIdleAnimation = Animator.StringToHash("fuse");

	private static readonly int normalIdleAnimation = Animator.StringToHash("normal");

	public Transform BehitTransform;

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

	public List<Teammate2_FuseLeg> essenceLegs { get; private set; } = new List<Teammate2_FuseLeg>();


	public List<Teammate2FuseHead> heads { get; private set; } = new List<Teammate2FuseHead>();


	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		state = MonsterState.BornIdle;
		idleTimer = 0f;
		idleWalkTimer = 0f;
		checkLegTargetIntervalTimer = 0f;
		essenceLegsTarget = null;
		essenceLegGroupCount = 0;
		essenceCurrentAttackingLegGroupIndex = 0;
		essenceAttackTimer = 0f;
		bodyList.Clear();
		bornIdleTimer = 0f;
		checkTargetIntervalTimer = 0f;
		BehitTransform.localPosition = Vector3.zero;
		ShowTeammate();
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		foreach (Teammate2_FuseLeg leg in legs)
		{
			leg.HideLegs();
		}
		foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
		{
			essenceLeg.HideLegs();
		}
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		if (legs != null && legs.Count > 0)
		{
			foreach (Teammate2_FuseLeg leg in legs)
			{
				leg.ShowLegs();
			}
		}
		if (essenceLegs != null && essenceLegs.Count > 0)
		{
			foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
			{
				essenceLeg.ShowLegs();
			}
		}
		selfShadow.ShadowGO.SetActive(value: true);
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		essenceLegs.Clear();
		legTargetPpt = new UnitProperty[base.SummonerSpellBase.spellCfg.int1 - NotAttackLegCount];
		bodyCount = FusionData.CurrentFusionLevel;
		lastFramePosition = base.transform.position;
		floatingBationMode = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0;
		InitializeHeadAndLeg();
		CheckDamageUpPassiveEffect();
		int trigger = ((bodyCount >= 1) ? fuseIdleAnimation : normalIdleAnimation);
		base.Anima.SetTrigger(trigger);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * initialHPRatio[base.SummonerSpellBase.InitialWithConfig.level - 1];
		UpdateBodySize();
		if (floatingBationMode)
		{
			int b = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 2 * (FusionData.CurrentFusionLevel + 1);
			essenceLegGroupCount = Mathf.Min(13, b);
			essenceAttackInterval = 2f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio / (float)essenceLegGroupCount;
			essenceAttackTimer = essenceAttackInterval;
		}
		SetSafeMode();
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence != null)
		{
			myPpt.unitCfg.currentHP = PlayerMgr.Inst.ItemCtrller.curseCfg_Pestilence.int1.result;
		}
	}

	public override void LoseTarget()
	{
		base.LoseTarget();
		CancelAllLegTargets();
	}

	private void CancelAllLegTargets()
	{
		MonoBehaviour.print(legTargetPpt.Length + " " + legs.Count);
		for (int i = 0; i < legTargetPpt.Length; i++)
		{
			if (legTargetPpt[i] != null)
			{
				legTargetPpt[i] = null;
				CancelNormalLegTarget(i);
			}
		}
		foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
		{
			essenceLeg.CancelTarget();
		}
	}

	private void CheckDamageUpPassiveEffect()
	{
		float num = 1f;
		float damageUpEffectValue = GetDamageUpEffectValue();
		num += damageUpEffectValue / 100f;
		base.SummonerSpellBase.spellCfg.damage = Mathf.CeilToInt((SpellConfig.dic[base.SummonerSpellBase.spellCfg.id].damage + damageUpEffectValue) * attackInterval * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.InitialParameter.finalDamageExtra * attackInterval * num * 5f);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * initialHPRatio[base.SummonerSpellBase.InitialWithConfig.level - 1];
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
		foreach (HeadData body in bodyList)
		{
			body.head.OnEnterDelayDeathEvent();
		}
		foreach (Teammate2_FuseLeg leg in legs)
		{
			leg.lr_Leg.material.SetInt(UseGhostEffect, 1);
		}
		SummonGhostEffectToggle(state: true);
		ColliderToggle(state: false);
		FreeFromTeammate6();
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		foreach (HeadData body in bodyList)
		{
			body.head.OnEnterFuseStateEvent();
		}
		foreach (Teammate2_FuseLeg leg in legs)
		{
			leg.lr_Leg.material.SetInt(UseFuseShineEffect, 1);
			leg.lr_Leg.material.DOFloat(1f, FuseShineProcess, 1.3f);
			leg.lr_Shadow.gameObject.SetActive(value: false);
			leg.EssencelegSetFuseState();
		}
	}

	private void InitializeHeadAndLeg()
	{
		int num = base.SummonerSpellBase.spellCfg.int1 - NotAttackLegCount;
		int num2 = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 2;
		int num3 = NotAttackLegCount + (FusionData.CurrentFusionLevel + 1) * (num + num2);
		if (legs.Count < num3)
		{
			int num4 = num3 - legs.Count;
			for (int i = 0; i < num4; i++)
			{
				CreateNewLeg();
			}
		}
		int num5 = FusionData.CurrentFusionLevel + 1;
		if (heads.Count < num5)
		{
			int num6 = num5 - heads.Count;
			for (int j = 0; j < num6; j++)
			{
				CreateNewHead();
			}
		}
		foreach (Teammate2_FuseLeg leg in legs)
		{
			leg.gameObject.SetActive(value: false);
		}
		foreach (Teammate2FuseHead head in heads)
		{
			head.gameObject.SetActive(value: false);
		}
		for (int k = 0; k < num5; k++)
		{
			HeadData headData = new HeadData();
			bodyList.Add(headData);
			heads[k].gameObject.SetActive(value: true);
			headData.head = heads[k];
			heads[k].transform.localPosition = new Vector3(0f, bodyDistance * (float)k, -0.02f * (float)k);
			if (num5 > 1 && k == 0)
			{
				heads[k].transform.localPosition += new Vector3(0f, 0.14f, 0f);
			}
			heads[k].Initialize(base.SummonerSpellBase.ColorType, k > 0);
			headData.legs = new Teammate2_FuseLeg[num];
			for (int l = 0; l < num; l++)
			{
				int index = NotAttackLegCount + k * num + l;
				headData.legs[l] = legs[index];
				headData.legs[l].gameObject.SetActive(value: true);
				headData.legs[l].legIndex = l;
				headData.legs[l].legTotalNum = num;
				headData.legs[l].headIndex = k + 1;
				headData.legs[l].Initialize(this, Tool2D.GetDir(360f / (float)num * (float)l + 360f / (float)num / (float)num5 * (float)k));
			}
		}
		for (int m = 0; m < NotAttackLegCount; m++)
		{
			legs[m].gameObject.SetActive(value: true);
			legs[m].legIndex = m;
			legs[m].legTotalNum = base.SummonerSpellBase.spellCfg.int1 - 4;
			legs[m].headIndex = 1;
			legs[m].Initialize(this, Tool2D.GetDir(360f / (float)(base.SummonerSpellBase.spellCfg.int1 - 4) * (float)m + 360f / (float)(base.SummonerSpellBase.spellCfg.int1 - 4) / 2f));
		}
		int num7 = 0;
		for (int n = 0; n < num2 * (FusionData.CurrentFusionLevel + 1); n++)
		{
			int num8 = base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level * 2 * (FusionData.CurrentFusionLevel + 1);
			int index2 = NotAttackLegCount + (FusionData.CurrentFusionLevel + 1) * num + n;
			float num9 = 360f / (float)num8;
			legs[index2].gameObject.SetActive(value: true);
			legs[index2].legIndex = n;
			legs[index2].legTotalNum = num2 * (FusionData.CurrentFusionLevel + 1);
			legs[index2].headIndex = num7;
			num7++;
			if (num7 >= num5)
			{
				num7 = 0;
			}
			legs[index2].Initialize(this, Tool2D.GetDir(90f + num9 * (float)n + UnityEngine.Random.Range((0f - num9) / 4f, num9 / 4f)), isEssenceLeg: true);
			legs[index2].essenceLegAttackLerpCurve = essenceLegAttackLerpCurve;
			legs[index2].essenceAttackDuration = 1f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
			legs[index2].essenceLegDamageRatio = 0.6f;
			essenceLegs.Add(legs[index2]);
			legs[index2].gameObject.SetActive(value: true);
		}
	}

	public void ControldByTeammate6()
	{
		base.Anima.SetTrigger("stop");
		base.CanMove = false;
		BehitTransform.localPosition = new Vector3(0f, -1f, 0f);
		ColliderToggle(state: false);
		base.beingControlledByTeammate6 = true;
		HideTeammate();
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.beingControlledByTeammate6 = false;
			base.Anima.SetTrigger("fuse");
			BehitTransform.localPosition = Vector3.zero;
			base.CanMove = true;
			ShowTeammate();
		}
	}

	private void CreateNewLeg()
	{
		string text = "Prefabs/Spell/" + 20021 + "/" + 20021 + "_Leg";
		legs.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>(text), base.transform.position, quaternion.identity, base.transform).GetComponent<Teammate2_FuseLeg>());
	}

	private void CreateNewHead()
	{
		string text = "Prefabs/Spell/" + 20021 + "/" + 20021 + "_FuseHead";
		heads.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>(text), base.transform.position, quaternion.identity, bodyTransform).GetComponent<Teammate2FuseHead>());
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
			foreach (Teammate2_FuseLeg leg in legs)
			{
				leg.Theme6Reposition(vector);
			}
			foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
			{
				essenceLeg.Theme6Reposition(vector);
				essenceLeg.LegEssenceLockingTarget(isInstanceMove: true);
			}
		}
		CheckTarget();
		CheckLegTarget();
		UpdataEssenceLegAttackState();
		StateMachine();
		myPpt.bodyCenterPoint = base.transform.position + tsf_Motion.transform.localPosition / 2f + new Vector3(0f, lifeLineHeightShift, 0f);
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

	private void CheckLegTarget()
	{
		checkLegTargetIntervalTimer += Time.deltaTime;
		if (checkLegTargetIntervalTimer >= legCheckInterval)
		{
			checkLegTargetIntervalTimer = 0f;
			UpdateNormalLegAttackingTarget();
			UpdateEssenceLegAttackTarget();
		}
	}

	private void UpdateNormalLegAttackingTarget()
	{
		for (int i = 0; i < legTargetPpt.Length; i++)
		{
			if (legTargetPpt[i] != null)
			{
				if (!legTargetPpt[i].gameObject.activeSelf || !legTargetPpt[i].CanBeTarget)
				{
					legTargetPpt[i] = null;
					CancelNormalLegTarget(i);
				}
				else if ((legTargetPpt[i].transform.position - base.transform.position).sqrMagnitude > base.SummonerSpellBase.spellCfg.radius * 2f * base.SummonerSpellBase.spellCfg.radius * 2f && (legTargetPpt[i].transform.position - base.transform.position).sqrMagnitude > base.SummonerSpellBase.spellCfg.radius * base.SummonerSpellBase.spellCfg.radius * 4f * legCancelTargetLengthRatio * legCancelTargetLengthRatio)
				{
					legTargetPpt[i] = null;
					CancelNormalLegTarget(i);
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
			for (int l = 0; l < legTargetPpt.Length; l++)
			{
				if (legTargetPpt[l] == null)
				{
					legTargetPpt[l] = targetablePpts[j];
					SetNormalLegTarget(l, targetablePpts[j]);
					break;
				}
			}
		}
	}

	private void UpdateEssenceLegAttackTarget()
	{
		if (!floatingBationMode || (essenceLegsTarget != null && essenceLegsTarget.gameObject.activeSelf && essenceLegsTarget.CanBeTarget && Vector3.Distance(base.transform.position, essenceLegsTarget.transform.position) < 5f))
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
		foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
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
		essenceAttackTimer += Time.deltaTime;
		if (!(essenceAttackTimer >= essenceAttackInterval) || !(essenceLegsTarget != null))
		{
			return;
		}
		essenceAttackTimer = 0f;
		foreach (Teammate2_FuseLeg item in essenceLegs.Where((Teammate2_FuseLeg e) => e.legIndex % essenceLegGroupCount == essenceCurrentAttackingLegGroupIndex))
		{
			item.LegEssenceAttackStart(essenceLegsTarget);
		}
		essenceCurrentAttackingLegGroupIndex = ++essenceCurrentAttackingLegGroupIndex % essenceLegGroupCount;
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
			foreach (Teammate2_FuseLeg leg in legs)
			{
				leg.Reposition();
				leg.CancelTarget();
			}
			foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
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

	private void SetNormalLegTarget(int index, UnitProperty targetPpt)
	{
		foreach (HeadData body in bodyList)
		{
			body.legs[index].SetTarget(targetPpt);
		}
	}

	private void CancelNormalLegTarget(int index)
	{
		foreach (HeadData body in bodyList)
		{
			body.legs[index].CancelTarget();
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		base.AfterTakeDamage(info);
		UpdateBodySize();
	}

	private void UpdateBodySize()
	{
		tsf_Motion.localScale = Vector3.one * Mathf.Lerp(minBodySize, maxBodySize, myPpt.unitCfg.currentHP / myPpt.unitCfg.maxHP);
	}

	protected override Vector3 GetSummonEffectSize()
	{
		return base.GetSummonEffectSize() * 1.5f;
	}

	public void RecoveryOnce()
	{
		myPpt.HPRecovery(Mathf.CeilToInt(base.SummonerSpellBase.spellCfg.float2));
		UpdateBodySize();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
		floatingBationMode = false;
		CancelAllLegTarget();
	}

	private void CancelAllLegTarget()
	{
		if (legTargetPpt == null)
		{
			return;
		}
		for (int i = 0; i < legTargetPpt.Length; i++)
		{
			CancelNormalLegTarget(i);
		}
		foreach (Teammate2_FuseLeg essenceLeg in essenceLegs)
		{
			essenceLeg.CancelTarget();
		}
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		foreach (HeadData body in bodyList)
		{
			body.head.SetSafeMode(DataMgr.settingData.SafeMode);
			Teammate2_FuseLeg[] array = body.legs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSafeMode();
			}
		}
	}
}
