using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MonsterBorn : LayerCorrect
{
	[Space(50f)]
	public Animator animator;

	public AnimaEvent animaEvent;

	public Transform tsf_Model;

	public static Dictionary<int, float> IDScalePair = new Dictionary<int, float>();

	private bool isInitialized;

	private RoomController roomCtrller;

	private RoomObjData objData;

	private float delayTime;

	private List<ItemInfo> dropItemInfos;

	private float delayTimer;

	private bool beBorn;

	private bool curse_IsDoubleEnemy;

	private bool justForShow;

	public static int InRoomCount = 0;

	private EntityManager ettMgr;

	public Action<UnitProperty> OnBorn { get; set; }

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		if (!beBorn)
		{
			delayTimer += Time.deltaTime;
			if (delayTimer >= delayTime)
			{
				beBorn = true;
				animator.SetTrigger("Born");
				SEMgr.Inst.monsterBorn.PlaySE(SEPlayMode.Replay, 1);
			}
		}
	}

	private void AnimaAction(string animaName)
	{
		if (!(animaName == "CreateObj"))
		{
			if (animaName == "Finish")
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else if (!justForShow)
		{
			CreateMonster();
			LevelMgr.Inst.CurrentRoomCtrller.MonsterBornUnRegister(this);
		}
	}

	private float GetMonsterScale()
	{
		if (IDScalePair.ContainsKey(objData.id))
		{
			return IDScalePair[objData.id];
		}
		CapsuleCollider component = ObjPoolMgr.Inst.PreloadGO("Prefabs/Units/" + objData.id, 1f).GetComponent<CapsuleCollider>();
		float num = ((!(component != null)) ? 1f : Mathf.Max(1f, component.radius * 2f));
		IDScalePair.Add(objData.id, num);
		return num;
	}

	private void CreateMonster()
	{
		int num = 1;
		if (curse_IsDoubleEnemy && UnitConfig.map[objData.id].unitType != UnitType.Boss && UnitConfig.map[objData.id].unitType != UnitType.Elite)
		{
			num = 2;
		}
		if (UnitConfig.map[objData.id].isHybirdUnit)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + objData.id, base.transform.position);
				if (num == 2)
				{
					if (i == 0)
					{
						gO.transform.position += new Vector3(-0.2f, 0f, 0f);
					}
					else
					{
						gO.transform.position += new Vector3(0.2f, 0f, 0f);
					}
				}
				UnitProperty component = gO.GetComponent<UnitProperty>();
				if (OnBorn != null)
				{
					OnBorn?.Invoke(component);
				}
				if (dropItemInfos != null)
				{
					EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
					UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(component.myEntity);
					componentData.deadDropItemInfos = DTool.ListToBlobArray(dropItemInfos);
					entityManager.SetComponentData(component.myEntity, componentData);
				}
				gO.GetComponent<IRoomObjExtraData>()?.SetExtraData(objData.extraData1, objData.extraData2, objData.extraData3);
				gO.GetComponent<IRoomCtrller>()?.SetRoomCtrlller(roomCtrller);
				ITrap component2 = gO.GetComponent<ITrap>();
				if (component2 != null)
				{
					roomCtrller.TrapRegister(component2);
				}
			}
			return;
		}
		for (int j = 0; j < num; j++)
		{
			float3 worldPos = base.transform.position;
			if (num == 2)
			{
				if (j == 0)
				{
					worldPos += new float3(-0.2f, 0f, 0f);
				}
				else
				{
					worldPos += new float3(0.2f, 0f, 0f);
				}
			}
			Entity entity = QuickCreateSystem.Inst.CreateUnit(objData.id, worldPos);
			if (dropItemInfos != null)
			{
				Debug.Log("掉东西Dots");
				UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(entity);
				componentData2.deadDropItemInfos = DTool.ListToBlobArray(dropItemInfos);
				ettMgr.SetComponentData(entity, componentData2);
			}
			if (ettMgr.HasComponent<IRoomObjExtraData_Dots>(entity))
			{
				IRoomObjExtraData_Dots componentData3 = ettMgr.GetComponentData<IRoomObjExtraData_Dots>(entity);
				componentData3.data1 = objData.extraData1;
				componentData3.data2 = objData.extraData2;
				componentData3.data3 = objData.extraData3;
				ettMgr.SetComponentData(entity, componentData3);
			}
			if (ettMgr.HasComponent<IRoomCtrller_Dots>(entity))
			{
				IRoomCtrller_Dots componentData4 = ettMgr.GetComponentData<IRoomCtrller_Dots>(entity);
				componentData4.belongRoom.Value = roomCtrller;
				ettMgr.SetComponentData(entity, componentData4);
			}
			if (ettMgr.HasComponent<ITrap_Dots>(entity))
			{
				IRoomCtrller_Dots componentData5 = ettMgr.GetComponentData<IRoomCtrller_Dots>(entity);
				componentData5.belongRoom.Value = roomCtrller;
				ettMgr.SetComponentData(entity, componentData5);
			}
		}
	}

	public void SetForShow(float delayTime)
	{
		if (!isInitialized)
		{
			isInitialized = true;
			animaEvent.DoAction = AnimaAction;
		}
		CorrectLayerOnce();
		this.delayTime = delayTime;
		delayTimer = 0f;
		beBorn = false;
		justForShow = true;
		tsf_Model.localScale = Vector3.one;
	}

	public void Initialize(RoomController roomCtrller, RoomObjData objData, float delayTime, bool immediatelyCreate, bool isDoubleEnemy, List<ItemInfo> dropItemInfos = null)
	{
		if (!isInitialized)
		{
			isInitialized = true;
			animaEvent.DoAction = AnimaAction;
		}
		this.roomCtrller = roomCtrller;
		this.objData = objData;
		this.delayTime = delayTime;
		this.dropItemInfos = dropItemInfos;
		delayTimer = 0f;
		beBorn = false;
		curse_IsDoubleEnemy = isDoubleEnemy;
		justForShow = false;
		if (objData.objType != 0)
		{
			Debug.LogError("monsterBorn不是生出Unit！");
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		switch (DataMgr.selectedWorldData.selectedDifficulty)
		{
		case DifficultyType.Normal:
			if (MultiDifficultyConfig.dic.ContainsKey(objData.id))
			{
				MultiDifficultyConfig multiDifficultyConfig2 = MultiDifficultyConfig.dic[objData.id];
				if (UnityEngine.Random.value <= multiDifficultyConfig2.hardChance)
				{
					objData.id = multiDifficultyConfig2.hardUnitID;
				}
			}
			break;
		case DifficultyType.Hard:
		case DifficultyType.Nightmare1:
		case DifficultyType.Nightmare2:
		case DifficultyType.Nightmare3:
		{
			if (!MultiDifficultyConfig.dic.ContainsKey(objData.id))
			{
				break;
			}
			MultiDifficultyConfig multiDifficultyConfig = MultiDifficultyConfig.dic[objData.id];
			if (UnityEngine.Random.value <= multiDifficultyConfig.hardChance)
			{
				objData.id = multiDifficultyConfig.hardUnitID;
				if (UnityEngine.Random.value <= multiDifficultyConfig.nightmareChance)
				{
					objData.id = multiDifficultyConfig.nightmareUnitID;
				}
			}
			break;
		}
		default:
			Debug.LogError(DataMgr.selectedWorldData.selectedDifficulty);
			break;
		case DifficultyType.Easy:
			break;
		}
		float monsterScale = GetMonsterScale();
		tsf_Model.localScale = new Vector3(monsterScale, monsterScale, 1f);
		if (immediatelyCreate || UnitConfig.map[objData.id].immediatelyBorn)
		{
			beBorn = true;
			CreateMonster();
			roomCtrller.MonsterBornUnRegister(this);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		InRoomCount++;
	}

	private void OnDisable()
	{
		InRoomCount--;
		OnBorn = null;
	}
}
