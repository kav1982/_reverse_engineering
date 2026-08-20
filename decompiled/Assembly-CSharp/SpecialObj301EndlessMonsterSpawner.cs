using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpecialObj301EndlessMonsterSpawner : InteractiveObj, IRoomObjExtraData
{
	[Header("交互相关")]
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public Sprite sr_Enable;

	public Sprite sr_Disable;

	public float itemOffset;

	private Vector3 roomCenter;

	private float roomWidth;

	private float roomHeight;

	public static SpecialObj301EndlessMonsterSpawner Inst;

	public List<SpecialObj302EndlessStore> storeBasePoints = new List<SpecialObj302EndlessStore>();

	public List<Entity> storeItems = new List<Entity>();

	private EntityManager entityManager;

	public int unpickedCoinCount;

	[Header("boss测试")]
	public List<int> bossIDs;

	public float bossFightDuration;

	[Header("精英测试")]
	public List<int> eliteIDs;

	private List<UnitProperty> existingBosses = new List<UnitProperty>();

	private List<UnitProperty> existingElites = new List<UnitProperty>();

	[Header("刷怪相关")]
	public EndlessBattleSpawnInfo.StageSpawnInfo currentStageSpawnInfo;

	[HideInInspector]
	public List<EndlessBattleSpawnInfo.DropCounts> dropCounts;

	private EndlessBattleSpawnInfo.EndlessUnitType nextSpawnID;

	private float nextSpawnTime;

	private int[] spawnID;

	private float[] spawnChances;

	private float spawnInterval;

	private float spawnTimer;

	private float stageTimer;

	private Entity interactiveEntity;

	private bool frame1Initialized;

	private bool frame2Initialized;

	private List<int> groupSummonIDs = new List<int>();

	private List<int> groupSummonCounts = new List<int>();

	private List<float> chestMonsterSpawnTime = new List<float>();

	private static List<ItemInfo> lockedItemInfos = new List<ItemInfo>();

	private List<ItemInfo> itemInfo = new List<ItemInfo>();

	private List<Vector3> randomSummonPoints = new List<Vector3>();

	[Header("单位测试波次配置")]
	public bool useTestStage
	{
		get
		{
			if (ScriptableObjMgr.staticTestCtrller.UseEndlessTestStage)
			{
				return testCtrller.endlessTestType != TestController.EndlessTestType.Boss;
			}
			return false;
		}
	}

	public EndlessBattleSpawnInfo.StageSpawnInfo testStageSpawnInfo => ScriptableObjMgr.staticTestCtrller.endlessWaveInfo;

	public static int CurrentLevel => BattleMgr.Inst.CurrentLevel;

	public static int CurrentStage => BattleMgr.Inst.EndlessCurrentStage;

	public bool HaveSpellProcessor => DataMgr.selectedWorldData.battleData9.endlessExtraProcessorLevel.Contains(CurrentLevel);

	public bool StageFinished { get; private set; }

	public float RemainTime
	{
		get
		{
			if (!StageFinished)
			{
				return currentStageSpawnInfo.duration - stageTimer;
			}
			return 0f;
		}
	}

	public float percentDamageReduce => Mathf.Lerp(0f, 0.9f, (float)(CurrentLevel - 6) / 12f);

	public bool isBossFight { get; private set; }

	public bool isEliteFight { get; private set; }

	public float hpRatioFix { get; private set; }

	public float knockBackRatioFix { get; private set; }

	public float frozenTimeRatioFix { get; private set; }

	private TestController testCtrller => ScriptableObjMgr.staticTestCtrller;

	private bool isBossTest
	{
		get
		{
			if (testCtrller.UseEndlessTestStage)
			{
				return testCtrller.endlessTestType == TestController.EndlessTestType.Boss;
			}
			return false;
		}
	}

	private bool isEliteTest
	{
		get
		{
			if (testCtrller.UseEndlessTestStage)
			{
				return testCtrller.endlessTestType == TestController.EndlessTestType.Elite;
			}
			return false;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(CreateStoreItems));
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(OnPlayerDead));
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(CreateStoreItems));
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(OnPlayerDead));
	}

	private void OnDestroy()
	{
		GameUISingletonMono<UIEndlessBattle>.HideIfInited();
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 != 0f)
		{
			StageFinished = true;
			lockedItemInfos.Clear();
		}
	}

	public void OnPlayerDead()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		if (DataMgr.selectedWorldData.endless_LevelOfMaxHP > 0 && PlayerMgr.Inst.ItemCtrller.relic_EndlessExtraMaxHP == null)
		{
			for (int i = 0; i < DataMgr.selectedWorldData.endless_LevelOfMaxHP; i++)
			{
				PlayerMgr.Inst.ItemCtrller.RelicAdd(930);
			}
		}
		if (DataMgr.selectedWorldData.endless_LevelOfExtraDamage > 0 && PlayerMgr.Inst.ItemCtrller.relic_EndlessExtraDamage == null)
		{
			for (int j = 0; j < DataMgr.selectedWorldData.endless_LevelOfExtraDamage; j++)
			{
				PlayerMgr.Inst.ItemCtrller.RelicAdd(929);
			}
		}
		if (CurrentLevel == 0)
		{
			StageFinished = true;
		}
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		outline.enabled = false;
		InteractiveObjType type = ((CurrentLevel == 0) ? InteractiveObjType.SpecialObj301EndlessMonsterSpawnerLevel0 : InteractiveObjType.SpecialObj301EndlessMonsterSpawner);
		interactiveEntity = RegisterDotsInteractiveObj(CC, type);
		Inst = this;
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		main.sprite = sr_Disable;
		GameUISingletonMono<UIEndlessBattle>.ShowInit();
		if (StageFinished)
		{
			SetDotsObjLayer(interactiveEntity, isOpen: true);
			main.sprite = sr_Enable;
			return;
		}
		SetDotsObjLayer(interactiveEntity, isOpen: false);
		dropCounts = EndlessUnitConfig.unitDropCounts;
		float num = 25f;
		hpRatioFix = GameConstManaged.GetEndlessHpRatio(CurrentLevel);
		knockBackRatioFix = 1f * Mathf.Pow(0.05f, Mathf.Max(num * 2f, CurrentLevel) / num);
		frozenTimeRatioFix = 1f * Mathf.Pow(0.5f, Mathf.Max(num * 2f, CurrentLevel) / num);
	}

	public void Frame1Initialze()
	{
		if (StageFinished)
		{
			return;
		}
		EventMgr.EndlessStageStart();
		if (CurrentLevel == 0)
		{
			DataMgr.selectedWorldData.battleData9.endlessMonsterIDs.Clear();
		}
		isBossFight = (CurrentLevel % 5 == 1 && CurrentLevel > 1) || isBossTest;
		int index = Mathf.Clamp(CurrentLevel - 1, 0, RawEndlessWaveConfig.list.Count - 1);
		if (!isBossFight)
		{
			EndlessBattleSpawnInfo.StageSpawnInfo originSpawnInfo = EndlessBattleSpawnInfo.IDListToSpawnInfo(DataMgr.selectedWorldData.battleData9.endlessMonsterIDs, CurrentLevel);
			currentStageSpawnInfo = RawEndlessWaveConfig.rawSpawnInfoList[index].GetEndlessWaveConfig(originSpawnInfo);
		}
		else
		{
			currentStageSpawnInfo = new EndlessBattleSpawnInfo.StageSpawnInfo
			{
				dropCount = RawEndlessWaveConfig.rawSpawnInfoList[index].dropCount,
				duration = bossFightDuration
			};
			if (testCtrller.endlessTestType == TestController.EndlessTestType.Boss)
			{
				currentStageSpawnInfo.duration = testCtrller.endlessWaveInfo.duration;
			}
		}
		if (!isBossFight && !useTestStage)
		{
			DataMgr.selectedWorldData.battleData9.endlessMonsterIDs = EndlessBattleSpawnInfo.SpawnInfoToIDList(currentStageSpawnInfo);
		}
		if (useTestStage)
		{
			currentStageSpawnInfo = testStageSpawnInfo.Copy();
		}
		int count = currentStageSpawnInfo.spawnChances.Count;
		spawnID = new int[count];
		spawnChances = new float[count];
		for (int i = 0; i < currentStageSpawnInfo.spawnChances.Count; i++)
		{
			spawnID[i] = (int)currentStageSpawnInfo.spawnChances[i].unitType;
			spawnChances[i] = currentStageSpawnInfo.spawnChances[i].chance;
		}
		if (DataMgr.selectedWorldData.endless_LevelOfSupplyBox > 0)
		{
			float num = currentStageSpawnInfo.duration;
			while (num > 0f && GeneralTool.ChanceResult(0.3f))
			{
				num -= 20f;
				int count2 = chestMonsterSpawnTime.Count;
				chestMonsterSpawnTime.Add(UnityEngine.Random.Range(20 * count2, Mathf.Min(20 * (count2 + 1), currentStageSpawnInfo.duration - 15f)));
			}
		}
		List<int> endlessBossIDPool;
		int currentStage;
		if (isBossFight)
		{
			endlessBossIDPool = DataMgr.selectedWorldData.battleData9.endlessBossIDPool;
			if (endlessBossIDPool.Count == 0)
			{
				goto IL_028d;
			}
			if (endlessBossIDPool.Count == 0)
			{
				currentStage = CurrentStage;
				if (currentStage == 2 || currentStage == 4 || currentStage == 6)
				{
					goto IL_028d;
				}
			}
			goto IL_035a;
		}
		goto IL_03f5;
		IL_028d:
		switch (CurrentStage)
		{
		case 1:
		case 2:
			endlessBossIDPool.Add(505001);
			endlessBossIDPool.Add(505101);
			break;
		case 3:
		case 4:
			endlessBossIDPool.Add(505201);
			endlessBossIDPool.Add(505501);
			break;
		case 5:
		case 6:
			endlessBossIDPool.Add(505401);
			break;
		default:
			endlessBossIDPool.Add(505001);
			endlessBossIDPool.Add(505101);
			endlessBossIDPool.Add(505201);
			endlessBossIDPool.Add(505401);
			endlessBossIDPool.Add(505501);
			break;
		}
		GeneralTool.RandomizeList(endlessBossIDPool);
		currentStage = CurrentStage;
		if (currentStage == 2 || currentStage == 4 || currentStage == 6)
		{
			endlessBossIDPool.RemoveAt(0);
		}
		goto IL_035a;
		IL_035a:
		int num2 = endlessBossIDPool[0];
		endlessBossIDPool.RemoveAt(0);
		if (isBossTest && UnitConfig.map.ContainsKey(testCtrller.endlessTestBossID))
		{
			num2 = testCtrller.endlessTestBossID;
		}
		UnitProperty component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + num2, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint).GetComponent<UnitProperty>();
		SetPptFix(component);
		existingBosses.Add(component);
		LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
		goto IL_03f5;
		IL_03f5:
		List<int> endlessEliteAppearLevel = DataMgr.selectedWorldData.battleData9.endlessEliteAppearLevel;
		int num3 = 5 * (CurrentStage - 1) + 1;
		if ((endlessEliteAppearLevel.Count == 0 || endlessEliteAppearLevel[0] < num3) && CurrentStage > 2)
		{
			endlessEliteAppearLevel.Clear();
			currentStage = CurrentStage;
			if (currentStage == 3 || currentStage == 4)
			{
				endlessEliteAppearLevel.Add(num3 + UnityEngine.Random.Range(2, 4));
			}
			else
			{
				int item = num3 + UnityEngine.Random.Range(1, 5);
				endlessEliteAppearLevel.Add(item);
				while (endlessEliteAppearLevel.Contains(item))
				{
					item = num3 + UnityEngine.Random.Range(1, 5);
				}
				endlessEliteAppearLevel.Add(item);
			}
		}
		isEliteFight = endlessEliteAppearLevel.Contains(CurrentLevel) || isEliteTest;
		if (isEliteFight)
		{
			int id = eliteIDs.GetRandom();
			if (isEliteTest && UnitConfig.map.ContainsKey(testCtrller.endlessTestEliteID))
			{
				id = testCtrller.endlessTestEliteID;
			}
			SummonSingle(id, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint, 0f, 2.5f);
			for (int j = 0; j < currentStageSpawnInfo.spawnChances.Count; j++)
			{
				if (currentStageSpawnInfo.spawnChances[j].unitGroup == EndlessBattleSpawnInfo.EndlessUnitGroup.E_Shooter)
				{
					currentStageSpawnInfo.spawnChances.RemoveAt(j);
					break;
				}
			}
			spawnChances = new float[count];
			for (int k = 0; k < currentStageSpawnInfo.spawnChances.Count; k++)
			{
				spawnID[k] = (int)currentStageSpawnInfo.spawnChances[k].unitType;
				spawnChances[k] = currentStageSpawnInfo.spawnChances[k].chance;
			}
		}
		List<int> endlessExtraProcessorLevel = DataMgr.selectedWorldData.battleData9.endlessExtraProcessorLevel;
		num3 = 5 * (CurrentStage - 1) + 1;
		if (endlessExtraProcessorLevel.Count == 0 || endlessExtraProcessorLevel[0] < num3)
		{
			endlessExtraProcessorLevel.Clear();
			if (CurrentStage > 1 && DataMgr.selectedWorldData.endless_LevelOfProcessSpell > 0)
			{
				endlessExtraProcessorLevel.Add(num3 + UnityEngine.Random.Range(1, 3));
			}
			endlessExtraProcessorLevel.Add(5 * CurrentStage + 1 - 1);
		}
		StageFinished = false;
		nextSpawnTime = 0f;
		spawnTimer = 0f;
		if (!isBossFight)
		{
			GetNextSummon();
		}
	}

	public void SpawnElite(EndlessSpawnEffectSystem.CreateEndlessMonsterRequest request)
	{
		UnitProperty component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + request.monsterID, request.monsterPosition).GetComponent<UnitProperty>();
		SetPptFix(component);
		existingElites.Add(component);
		LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
	}

	public void SetPptFix(UnitProperty ppt)
	{
		ppt.unitCfg.maxHP *= hpRatioFix;
		ppt.unitCfg.maxHP = Mathf.Floor(ppt.unitCfg.maxHP);
		ppt.unitCfg.currentHP *= hpRatioFix;
		ppt.unitCfg.currentHP = Mathf.Floor(ppt.unitCfg.currentHP);
		ppt.unitCfg.knockbackRatio *= knockBackRatioFix;
		ppt.unitCfg.frozenTimeRatio *= frozenTimeRatioFix;
		UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(ppt.myEntity);
		componentData.unitCfg = ppt.unitCfg;
		entityManager.SetComponentData(ppt.myEntity, componentData);
	}

	public override void Select()
	{
		outline.enabled = true;
	}

	public override void Unselect()
	{
		outline.enabled = false;
	}

	public override void Interact()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.levelRewardEttList.Count <= 0)
		{
			DoorBase_Dots doorBase = default(DoorBase_Dots);
			doorBase.rewardType = LevelRewardType.None;
			BattleMgr.Inst.PlayerEnterDoor(doorBase);
			SetDotsObjLayer(interactiveEntity, isOpen: false);
			main.sprite = sr_Disable;
			ClearStoreItems();
		}
	}

	public void OnStageFinish(Vector3 position)
	{
		main.sprite = sr_Enable;
		CheckStorePoints();
		itemInfo = OutputMgr.GetEndlessStore();
		LevelMgr.Inst.CurrentRoomCtrller.KillAllMonster();
		LevelMgr.Inst.CurrentRoomCtrller.OnRoomFinish(position);
		SetDotsObjLayer(interactiveEntity, isOpen: true);
		Debug.Log("StageFinish!!!");
		EventMgr.EndlessStageClear();
		if (DataMgr.selectedWorldData.endless_LevelOfFinishCoin > 0)
		{
			int endlessFinishCoinCount = DataMgr.selectedWorldData.GetEndlessFinishCoinCount();
			PlayerMgr.Inst.ChangeCoin(endlessFinishCoinCount);
			QuickCreateSystem.Inst.CreateTextFloatVFX(endlessFinishCoinCount, UITextFloatType.DropCoin, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.5f, 0f));
		}
	}

	private void GetNextSummon()
	{
		nextSpawnID = currentStageSpawnInfo.spawnChances[GeneralTool.GetWeightRandom(spawnChances)].unitType;
		bool flag = false;
		for (int i = 0; i < dropCounts.Count; i++)
		{
			if (dropCounts[i].unitID == (int)nextSpawnID)
			{
				nextSpawnTime = dropCounts[i].dropCount;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			int num = (int)nextSpawnID;
			Debug.LogError("缺少指定id的掉落数量：" + num);
		}
	}

	private void CheckStorePoints()
	{
		storeBasePoints.Sort();
		switch ((DataMgr.selectedWorldData.endless_LevelOfGoodsExtraCount != 0) ? ScriptableObjMgr.Inst.EndlessTalentUpgrade.goodsExtraCount[DataMgr.selectedWorldData.endless_LevelOfGoodsExtraCount - 1].value : 0)
		{
		case 1:
		{
			for (int num2 = storeBasePoints.Count - 1; num2 >= 0; num2--)
			{
				if (storeBasePoints[num2].index == 3)
				{
					storeBasePoints[num2].Hide();
					storeBasePoints.RemoveAt(num2);
				}
				else if (num2 < 3)
				{
					storeBasePoints[num2].transform.position += Vector3.right * 1.5f;
				}
			}
			break;
		}
		case 0:
		{
			for (int num = storeBasePoints.Count - 1; num >= 0; num--)
			{
				if (storeBasePoints[num].index == 7 || storeBasePoints[num].index == 3)
				{
					storeBasePoints[num].Hide();
					storeBasePoints.RemoveAt(num);
				}
				else
				{
					storeBasePoints[num].transform.position += Vector3.right * 1.5f;
				}
			}
			break;
		}
		}
	}

	public void RefreshStoreItems()
	{
		itemInfo = OutputMgr.GetEndlessStore();
		SpecialObj313EndlessStoreLocker.Inst.SetLock(locked: false);
		ClearStoreItems();
		CreateStoreItems();
	}

	private void ClearStoreItems()
	{
		for (int i = 0; i < storeItems.Count; i++)
		{
			if (entityManager.HasComponent<LocalTransform>(storeItems[i]))
			{
				Item componentData = entityManager.GetComponentData<Item>(storeItems[i]);
				componentData.BackPool();
				componentData.Pickup(playSE: false);
				entityManager.SetComponentData(storeItems[i], componentData);
			}
		}
		storeItems.Clear();
	}

	private void CreateStoreItems()
	{
		storeItems.Clear();
		if (lockedItemInfos.Count > 0)
		{
			for (int i = 0; i < lockedItemInfos.Count; i++)
			{
				if (lockedItemInfos[i].id == 0)
				{
					storeItems.Add(Entity.Null);
					continue;
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_EndlessStoreRefresh", storeBasePoints[i].transform.position, 3f);
				Entity entity = QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, lockedItemInfos[i], storeBasePoints[i].transform.position + new Vector3(0f, 0f, 0f - itemOffset), isStore: true, isEndless: true);
				InteractiveObj_Dots componentData = entityManager.GetComponentData<InteractiveObj_Dots>(entity);
				componentData.uiOffset += new float3(0f, itemOffset, 0f);
				entityManager.SetComponentData(entity, componentData);
				Item componentData2 = entityManager.GetComponentData<Item>(entity);
				componentData2.belongRoomMapPos = new Vector2Int(1, 0);
				entityManager.SetComponentData(entity, componentData2);
				storeItems.Add(entity);
				if (lockedItemInfos[i].type == ItemType.Wand)
				{
					DataMgr.selectedWorldData.battleData9.RemoveWandFromPool(lockedItemInfos[i].id);
				}
				if (lockedItemInfos[i].type == ItemType.Relic)
				{
					DataMgr.selectedWorldData.battleData9.RemoveRelicFromPool(lockedItemInfos[i].id);
				}
			}
		}
		else
		{
			for (int j = 0; j < storeBasePoints.Count; j++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_EndlessStoreRefresh", storeBasePoints[j].transform.position, 3f);
				Entity entity2 = QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, itemInfo[j], storeBasePoints[j].transform.position + new Vector3(0f, 0f, 0f - itemOffset), isStore: true, isEndless: true);
				InteractiveObj_Dots componentData3 = entityManager.GetComponentData<InteractiveObj_Dots>(entity2);
				componentData3.uiOffset += new float3(0f, itemOffset, 0f);
				entityManager.SetComponentData(entity2, componentData3);
				Item componentData4 = entityManager.GetComponentData<Item>(entity2);
				componentData4.belongRoomMapPos = new Vector2Int(1, 0);
				entityManager.SetComponentData(entity2, componentData4);
				storeItems.Add(entity2);
			}
		}
		UnlockStoreItems();
	}

	public void LockStoreItems()
	{
		for (int i = 0; i < storeBasePoints.Count; i++)
		{
			storeBasePoints[i].SetLock(locked: true);
		}
		lockedItemInfos.Clear();
		bool flag = true;
		for (int j = 0; j < storeBasePoints.Count; j++)
		{
			if (entityManager.HasComponent<Item>(storeItems[j]))
			{
				Item componentData = entityManager.GetComponentData<Item>(storeItems[j]);
				lockedItemInfos.Add(componentData.info);
				flag = false;
			}
			else
			{
				lockedItemInfos.Add(default(ItemInfo));
			}
		}
		if (flag)
		{
			lockedItemInfos.Clear();
		}
	}

	public void UnlockStoreItems()
	{
		for (int i = 0; i < storeBasePoints.Count; i++)
		{
			storeBasePoints[i].SetLock(locked: false);
		}
		lockedItemInfos.Clear();
	}

	public void ItemBought(Entity entity)
	{
		if (lockedItemInfos.Count == 0)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < storeItems.Count; i++)
		{
			if (storeItems[i] == entity)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			lockedItemInfos[num] = default(ItemInfo);
		}
	}

	private IEnumerator CreateEliteDrop(Vector3 position)
	{
		yield return new WaitForSeconds(2f);
		int num = Mathf.FloorToInt(30f * ((float)(BattleMgr.Inst.CurrentLevel - 1) + 10f) / 10f);
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(TextFloatVFXBED));
		Entity singletonEntity = entityQuery.GetSingletonEntity();
		DynamicBuffer<TextFloatVFXBED> buffer = entityManager.GetBuffer<TextFloatVFXBED>(singletonEntity);
		if (GameMgr.IsSupportVFX)
		{
			buffer.Add(new TextFloatVFXBED
			{
				number = num,
				worldPos = position,
				type = UITextFloatType.GetCoin
			});
		}
		else
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + 1, UITextFloatType.GetCoin, position);
		}
		SEMgr.Inst.itemPick_Coin.PlaySE();
		PlayerMgr.Inst.ChangeCoin(num);
		PlayerMgr.Inst.ChangeGear(num);
	}

	private IEnumerator CreateBossDrop(Vector3 position)
	{
		yield return new WaitForSeconds(2f);
		int num = Mathf.CeilToInt((float)currentStageSpawnInfo.dropCount * 1.2f);
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(TextFloatVFXBED));
		Entity singletonEntity = entityQuery.GetSingletonEntity();
		DynamicBuffer<TextFloatVFXBED> buffer = entityManager.GetBuffer<TextFloatVFXBED>(singletonEntity);
		if (GameMgr.IsSupportVFX)
		{
			buffer.Add(new TextFloatVFXBED
			{
				number = num,
				worldPos = position,
				type = UITextFloatType.GetCoin
			});
		}
		else
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + 1, UITextFloatType.GetCoin, position);
		}
		SEMgr.Inst.itemPick_Coin.PlaySE();
		PlayerMgr.Inst.ChangeCoin(num);
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetEliteOrBossItemInfos(), position, 1f);
		StageFinished = true;
		OnStageFinish(position);
		MusicMgr.Inst.UpdateThemeMusic();
	}

	private void Update()
	{
		if (StageFinished)
		{
			return;
		}
		if (!frame1Initialized)
		{
			frame1Initialized = true;
			Frame1Initialze();
			return;
		}
		stageTimer += Time.deltaTime;
		if (stageTimer >= currentStageSpawnInfo.duration && frame1Initialized)
		{
			stageTimer = Mathf.Min(stageTimer, currentStageSpawnInfo.duration);
			if (!isBossFight)
			{
				StageFinished = true;
				OnStageFinish(PlayerMgr.Inst.PlayerPointIgnoreZ);
				return;
			}
			for (int i = 0; i < existingBosses.Count; i++)
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = float.MaxValue;
				info.ignoreBeHitColor = true;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(existingBosses[i].myEntity, info);
			}
		}
		if (!isBossFight)
		{
			if (isEliteFight && existingElites.Count > 0)
			{
				for (int num = existingElites.Count - 1; num >= 0; num--)
				{
					if (existingElites[num].UnitBas.deadStayed)
					{
						StartCoroutine(CreateEliteDrop(existingElites[num].transform.position));
						existingElites.RemoveAt(num);
					}
				}
			}
			for (int num2 = chestMonsterSpawnTime.Count - 1; num2 >= 0; num2--)
			{
				if (stageTimer > chestMonsterSpawnTime[num2] && !StageFinished)
				{
					Vector3 point = roomCenter + new Vector3(roomWidth * UnityEngine.Random.Range(-0.5f, 0.5f), roomHeight * UnityEngine.Random.Range(-0.5f, 0.5f));
					SummonSingle(132001, point);
					chestMonsterSpawnTime.RemoveAt(num2);
				}
			}
			if (stageTimer < currentStageSpawnInfo.duration)
			{
				spawnTimer += Time.deltaTime * (float)currentStageSpawnInfo.dropCount / currentStageSpawnInfo.duration;
			}
			if (spawnTimer > nextSpawnTime && LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Count < 200)
			{
				spawnTimer -= nextSpawnTime;
				int num3 = (int)nextSpawnID;
				Vector3 vector = roomCenter + new Vector3(roomWidth * UnityEngine.Random.Range(-0.5f, 0.5f), roomHeight * UnityEngine.Random.Range(-0.5f, 0.5f));
				while (Tool2D.IgnoreZDistanceSqr(vector, PlayerMgr.Inst.PlayerPoint) < 9f)
				{
					vector = roomCenter + new Vector3(roomWidth * UnityEngine.Random.Range(-0.5f, 0.5f), roomHeight * UnityEngine.Random.Range(-0.5f, 0.5f));
				}
				if (GameConstManaged.endlessGroupSummonType.Contains(nextSpawnID))
				{
					if (groupSummonIDs.Contains(num3))
					{
						int index = groupSummonIDs.IndexOf(num3);
						groupSummonCounts[index]--;
						if (groupSummonCounts[index] <= 0)
						{
							groupSummonIDs.RemoveAt(index);
							groupSummonCounts.RemoveAt(index);
						}
					}
					else
					{
						int num4 = (int)Mathf.Clamp(UnityEngine.Random.Range(0f, 1f) * (float)CurrentLevel / 2f * currentStageSpawnInfo.GetSummonChance(nextSpawnID), 1f, 10f);
						RandomizeSummonPoints(num4, vector);
						for (int j = 0; j < num4; j++)
						{
							SummonSingle(num3, randomSummonPoints[j], UnityEngine.Random.Range(0f, 0.5f));
						}
						if (num4 > 1)
						{
							groupSummonIDs.Add(num3);
							groupSummonCounts.Add(num4 - 1);
						}
						GetNextSummon();
					}
				}
				else
				{
					SummonSingle(num3, vector);
					GetNextSummon();
				}
			}
		}
		if (isBossFight && existingBosses.Count > 0 && existingBosses[0].UnitBas.deadStayed)
		{
			Vector3 doorToWalkablePoint = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(existingBosses[0].transform.position);
			StartCoroutine(CreateBossDrop(doorToWalkablePoint));
			existingBosses.RemoveAt(0);
		}
	}

	public void SummonSingle(int id, Vector3 point, float delay = 0f, float size = 1f)
	{
		Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("EF_EndlessMonsterBorn", Tool2D.PointWithinRange(point, roomCenter, roomWidth - 2f, roomHeight - 2f));
		EndlessSpawnEffect componentData = entityManager.GetComponentData<EndlessSpawnEffect>(entity);
		componentData.Initialize(id, size, delay);
		entityManager.SetComponentData(entity, componentData);
	}

	private void RandomizeSummonPoints(int summonCount, Vector3 centerPoint)
	{
		randomSummonPoints.Clear();
		int i;
		for (i = 2; i * i < summonCount * 2; i++)
		{
		}
		int num = summonCount;
		while (num > 0)
		{
			Vector3 item = new Vector3(UnityEngine.Random.Range(0, i), UnityEngine.Random.Range(0, i));
			if (!randomSummonPoints.Contains(item))
			{
				randomSummonPoints.Add(item);
				num--;
			}
		}
		centerPoint = Tool2D.PointWithinRange(centerPoint, roomCenter, roomWidth - (float)i, roomHeight - (float)i);
		for (int j = 0; j < randomSummonPoints.Count; j++)
		{
			randomSummonPoints[j] -= new Vector3((i - 1) / 2, (i - 1) / 2, 0f);
			randomSummonPoints[j] *= 1.2f;
			randomSummonPoints[j] += centerPoint;
			randomSummonPoints[j] += Tool2D.GetDir() * 0.3f;
		}
	}
}
