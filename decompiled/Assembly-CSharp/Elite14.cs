using System;
using System.Collections.Generic;
using UnityEngine;

public class Elite14 : UnitBase
{
	public enum MonsterState
	{
		Summon,
		Silent
	}

	[Serializable]
	public struct UnitChance
	{
		public int id;

		public float chance;
	}

	[Serializable]
	public struct StageSummonTable
	{
		public List<UnitChance> IdChanceList;

		public float baseSummonRate;

		public float minSummonRate;

		public int minRateUnitCount;

		public int maxRateUnitCount;

		public int limitUnitCount;
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("刷怪")]
	public bool isChildTest;

	public bool isBossTest;

	public bool isBossTestOnly;

	public List<float> patternCountMultiplier = new List<float>();

	public List<float> patternDamage = new List<float>();

	public float patternDamageRatio;

	public List<StageSummonTable> stageSummonTables = new List<StageSummonTable>();

	public StageSummonTable currentTable;

	private List<float> currentPatternChances = new List<float>();

	public List<float> stageHpRatio = new List<float>();

	public int currentStage;

	public AnimationCurve SummonSpeedCurve;

	public float summonDelayTime;

	public List<Elite14_Child> children = new List<Elite14_Child>();

	public List<float> childrenAngle = new List<float>();

	public List<float> childrenAngleDelta = new List<float>();

	public float remainSummonHp;

	private float SummonTimer;

	[Header("对象池")]
	public static MiniObjPool MiniPool;

	public static Elite14 Inst;

	[Header("表现")]
	public Transform tsf_Model;

	public Shadow thisShadow;

	[Header("转阶段沉默")]
	public float silentTime;

	private bool secondStageSummoned;

	[Header("尸体管理器，没有碎片不得劲")]
	public List<Corpse> corpses = new List<Corpse>();

	public int maxCorpseCount;

	[Header("杂项")]
	public Vector3 roomCenterPoint;

	public float roomWidth;

	public float roomHeight;

	public bool firstStageDefeated;

	private float startTimer;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	private Vector3 GetSortDir()
	{
		if (children.Count < 3)
		{
			return Tool2D.GetDir();
		}
		children.Sort();
		childrenAngle.Clear();
		childrenAngleDelta.Clear();
		for (int i = 0; i < children.Count; i++)
		{
			float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, children[i].transform.position - roomCenterPoint);
			if (num < 0f)
			{
				num += 360f;
			}
			childrenAngle.Add(num);
		}
		for (int j = 0; j < childrenAngle.Count; j++)
		{
			int num2 = j + 1;
			if (num2 >= childrenAngle.Count)
			{
				num2 = 0;
			}
			float num3 = childrenAngle[j] - childrenAngle[num2];
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			childrenAngleDelta.Add(num3);
		}
		int index = 0;
		float num4 = 0f;
		for (int k = 0; k < childrenAngleDelta.Count; k++)
		{
			if (childrenAngleDelta[k] > num4)
			{
				index = k;
				num4 = childrenAngleDelta[k];
			}
		}
		return Tool2D.GetDir(Vector3.up, childrenAngle[index] - childrenAngleDelta[index] / 2f).normalized;
	}

	private float SummonSingleChild()
	{
		if (children.Count >= currentTable.limitUnitCount)
		{
			return 1f;
		}
		float value = UnityEngine.Random.value;
		float num = Mathf.Lerp(0f, roomWidth, Mathf.Pow(value, 0.5f));
		Vector3 vector = roomCenterPoint + num * Tool2D.GetDir(GetSortDir(), 5f);
		for (int i = 0; i < 30; i++)
		{
			if (Tool2D.PointOnNavMesh(vector))
			{
				break;
			}
			value = UnityEngine.Random.value;
			num = Mathf.Lerp(0f, roomWidth, Mathf.Pow(value, 0.5f));
			vector = roomCenterPoint + num * Tool2D.GetDir(GetSortDir(), 5f);
		}
		if (!Tool2D.PointOnNavMesh(vector))
		{
			vector = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint, num, GetSortDir(), 5f);
		}
		int weightRandom = GeneralTool.GetWeightRandom(currentPatternChances.ToArray());
		Elite14_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + currentTable.IdChanceList[weightRandom].id, vector).GetComponent<Elite14_Child>();
		children.Add(component);
		remainSummonHp -= component.myPpt.unitCfg.maxHP;
		MonsterBorn component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_MonsterBorn", vector, 4f).GetComponent<MonsterBorn>();
		component2.transform.localScale = Vector3.one * component.myPpt.CC_Self.radius * 2.4f;
		component2.SetForShow(0f);
		if (Vector3.SqrMagnitude(PlayerMgr.Inst.PlayerPoint - vector) < 196f)
		{
			SEMgr.Inst.monsterBorn.PlaySE(SEPlayMode.Unique, 2, 4f);
		}
		return patternCountMultiplier[weightRandom];
	}

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		Inst = this;
		state = MonsterState.Summon;
		currentStage = -1;
		ObjPoolMgr.Inst.PreloadGO("Prefabs/Units/301441", 1f, ObjPoolMgr.PreloadType.Unit);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		remainSummonHp = componentData.unitCfg.maxHP;
		tsf_Model.gameObject.SetActive(value: false);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		componentData.ImmuneVoidRegister();
		SetComponentData(componentData);
	}

	private void Summon()
	{
		if (state == MonsterState.Silent || isChildTest || (isBossTest && Elite14_Stage2.Inst == null) || (remainSummonHp <= 0f && Elite14_Stage2.Inst == null) || isBossTestOnly)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < stageHpRatio.Count; i++)
		{
			if (secondStageSummoned)
			{
				num = stageHpRatio.Count - 1;
				continue;
			}
			if (!(remainSummonHp / myPpt.unitCfg.maxHP <= stageHpRatio[i]))
			{
				break;
			}
			num = i;
		}
		if (num != currentStage)
		{
			currentStage = num;
			currentTable = stageSummonTables[num];
			currentPatternChances.Clear();
			for (int j = 0; j < currentTable.IdChanceList.Count; j++)
			{
				currentPatternChances.Add(currentTable.IdChanceList[j].chance);
			}
		}
		float num2 = currentTable.minSummonRate + SummonSpeedCurve.Evaluate((float)(children.Count - currentTable.minRateUnitCount) / (float)currentTable.maxRateUnitCount) * (currentTable.baseSummonRate - currentTable.minSummonRate);
		SummonTimer += Time.deltaTime * num2;
		for (int k = 0; k < 999; k++)
		{
			if (SummonTimer < 0f)
			{
				break;
			}
			SummonTimer -= SummonSingleChild();
		}
	}

	public void ForceSummon(int count)
	{
		if (!isBossTestOnly)
		{
			for (int i = 0; i < count; i++)
			{
				SummonSingleChild();
			}
		}
	}

	public override void Update()
	{
		startTimer += Time.deltaTime;
		if (startTimer >= 0.5f)
		{
			Summon();
		}
		if (isBossTest && !base.deadStayed)
		{
			remainSummonHp = -1f;
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
			info.damage = 100000000f;
			info.ignoreFloatText = true;
			GetComponentData<UnitProperty_Dots>();
			UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
		}
		for (int num = children.Count - 1; num >= 0; num--)
		{
			if (children[num].myPpt.AlreadyDead)
			{
				children.RemoveAt(num);
			}
		}
		if (children.Count <= 0 && remainSummonHp <= 0f && !firstStageDefeated)
		{
			BossDeadStay();
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case MonsterState.Summon:
			_ = changedState;
			break;
		case MonsterState.Silent:
			if (children.Count > 0)
			{
				stateExistTime = 0f;
			}
			if ((stateExistTime > 0f || isBossTest) && !secondStageSummoned)
			{
				GameUISingletonMono<UIBossHP>.HideIfInited();
				secondStageSummoned = true;
				Elite14_Stage2 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/301441", roomCenterPoint).GetComponent<Elite14_Stage2>();
				MonsterBorn component2 = MiniPool.GetGO("Prefabs/EF/EF_Elite14_MonsterBorn", roomCenterPoint, 4f).GetComponent<MonsterBorn>();
				component2.transform.localScale = Vector3.one * component.myPpt.CC_Self.radius * 2.4f;
				component2.SetForShow(0f);
			}
			break;
		}
	}

	protected override void BossDeadStay()
	{
		firstStageDefeated = true;
		base.Rigid.isKinematic = true;
		SyncDotsPosition();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		componentData.unitCfg.currentHP = 1f;
		SetComponentData(componentData);
		if (base.Anima != null)
		{
			base.Anima.speed = 0f;
		}
		if (base.SAnima != null)
		{
			base.SAnima.timeScale = 0f;
		}
		state = MonsterState.Silent;
	}

	public void ChildDieDamage(float damage)
	{
		if (base.enabled)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
			info.damage = damage;
			info.ignoreFloatText = true;
			UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.attackerEntity != myPpt.myEntity)
		{
			info.immuneDamage = true;
		}
	}

	public Vector3 GetRandomRoomPoint()
	{
		return Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(roomWidth * (UnityEngine.Random.value - 0.5f), roomHeight * (UnityEngine.Random.value - 0.5f), 0f));
	}
}
