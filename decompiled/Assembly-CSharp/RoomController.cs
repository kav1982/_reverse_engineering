using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PlayerLogger.Events;
using Unity.AI.Navigation;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;

public class RoomController : MonoBehaviour
{
	public Transform tsf_Action;

	public Transform tsf_Ground;

	public Transform tsf_Fly;

	public Transform tsf_Boundary;

	public Transform tsf_Thing;

	public NavMeshSurface nms_Action;

	public NavMeshSurface nms_Ground;

	public NavMeshSurface nms_Fly;

	public ColliderCombiner combinerAction;

	public ColliderCombiner combinerGround;

	public ColliderCombiner combinerFly;

	public MucusController mucusCtrller;

	public VenomController venomCtrller;

	public WaterController waterCtrller;

	public FogController fogCtrller;

	[Header("EndlessBoundary")]
	public GameObject go_EndlessBoundaryParent;

	public Transform tsf_EndlessBoundaryUL;

	public Transform tsf_EndlessBoundaryUR;

	public Transform tsf_EndlessBoundaryLU;

	public Transform tsf_EndlessBoundaryLD;

	public Transform tsf_EndlessBoundaryRU;

	public Transform tsf_EndlessBoundaryRD;

	public Transform tsf_EndlessBoundaryD;

	[Header("Chapter3")]
	public float chapter3CheckPositionInterval;

	[Header("MR")]
	public GameObject go_MRParent;

	public MeshRenderer mr_Water;

	public MeshRenderer mr_Mucus;

	public MeshRenderer mr_Venom;

	public int pixelCountPerMeter;

	[HideInInspector]
	public RoomConfig roomCfg;

	public List<Entity> doorEttList = new List<Entity>();

	public List<Entity> accessEttList = new List<Entity>();

	[HideInInspector]
	public List<GameObject> abysses = new List<GameObject>();

	[HideInInspector]
	public List<AccessBase> accesses = new List<AccessBase>();

	[HideInInspector]
	public List<ITrap> traps = new List<ITrap>();

	private Dictionary<Vector2Int, List<ItemInfo>> extraDrops;

	private List<MonsterBorn> monsterBorns = new List<MonsterBorn>();

	private List<HideBoundaryBase> hideBoundarys = new List<HideBoundaryBase>();

	private Action<Vector3> roomFinishDelegate;

	private Action roomRecycleDelegate;

	private Action roomEnterDelegate;

	private Action roomLeaveDelegate;

	private Action rewardSelected;

	private int currentWave = 1;

	private bool whenFinishOpenDoorAndAccess = true;

	private bool whenEnterRoomUpdateThemeMusic = true;

	private float chapter3CheckPositionIntervalTimer;

	private RTCamController rtcam_Water;

	private RTCamController rtcam_Mucus;

	private RTCamController rtcam_Venom;

	private EntityManager ettMgr;

	public List<Vector3> abyssPoints_Dots = new List<Vector3>();

	public List<Entity> targetableEttList = new List<Entity>();

	public List<Entity> monsterEttList = new List<Entity>();

	public List<Entity> TeammateEttList = new List<Entity>();

	public List<Entity> TeammateNotAttackEttList = new List<Entity>();

	public List<Entity> noAttackTriggerDeadEttList = new List<Entity>();

	public List<Entity> levelRewardEttList = new List<Entity>();

	public List<Entity> trapEttList = new List<Entity>();

	private bool hasBossFight;

	private CollisionFilter CheckWallFilter = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 256u
	};

	public Vector2Int MapPos { get; private set; }

	public Vector3 CenterPoint { get; private set; }

	public bool IsFinish { get; private set; }

	public Vector2 RoomScale { get; private set; }

	public List<UnitProperty> TargetablePpts { get; private set; } = new List<UnitProperty>();


	public List<UnitProperty> MonsterPpts { get; private set; } = new List<UnitProperty>();


	private bool HaveWave => currentWave < 10;

	public bool AllLevelRewardPicked => levelRewardEttList.Count == 0;

	public Dictionary<Vector2Data, BoundaryBase> boundaryBase1Dic { get; private set; } = new Dictionary<Vector2Data, BoundaryBase>();


	public bool isPlayerDropBlood { get; private set; }

	public bool enableEnterNextRoom { get; private set; }

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		Chapter3RepositionCheck();
		if (rtcam_Venom != null)
		{
			if (LevelMgr.Inst.CurrentRoomCtrller == this)
			{
				rtcam_Venom.MaxFps = 20;
			}
			else
			{
				rtcam_Venom.MaxFps = 0;
			}
		}
		if (AllLevelRewardPicked && whenFinishOpenDoorAndAccess)
		{
			enableEnterNextRoom = true;
		}
	}

	public void CheckAllEttRecords()
	{
		CheckListElementsValid(targetableEttList);
		CheckListElementsValid(monsterEttList);
		CheckListElementsValid(noAttackTriggerDeadEttList);
		CheckListElementsValid(TeammateEttList);
		CheckListElementsValid(TeammateNotAttackEttList);
	}

	private void CheckListElementsValid(List<Entity> list)
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!UnitDotsSyncSystem.EntityIsValid(list[num]))
			{
				list.RemoveAt(num);
			}
		}
	}

	private void Chapter3RepositionCheck()
	{
		if (!(LevelMgr.Inst.CurrentRoomCtrller == this) || (roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1))
		{
			return;
		}
		chapter3CheckPositionIntervalTimer += Time.deltaTime;
		if (!(chapter3CheckPositionIntervalTimer >= chapter3CheckPositionInterval))
		{
			return;
		}
		chapter3CheckPositionIntervalTimer = 0f;
		foreach (Entity targetableEtt in targetableEttList)
		{
			if (!ettMgr.HasComponent<LocalTransform>(targetableEtt))
			{
				continue;
			}
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(targetableEtt);
			if (!componentData.unitCfg.theme6Reposition)
			{
				continue;
			}
			float3 f = GetChapter3RepositionChangeValue(ettMgr.GetComponentData<LocalToWorld>(targetableEtt).Position);
			if (!DTool.IsTotallySame(in f, in float3.zero))
			{
				if (componentData.unitCfg.isHybirdUnit)
				{
					ettMgr.GetComponentObject<UnitPptReference>(targetableEtt).unitPpt.UnitBas.Theme6Reposition(f);
					continue;
				}
				UnitBase_Dots componentData2 = ettMgr.GetComponentData<UnitBase_Dots>(targetableEtt);
				componentData2.onChapter3Reposition = true;
				componentData2.repositionValue = f;
				ettMgr.SetComponentData(targetableEtt, componentData2);
			}
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Item));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			Item componentData3 = ettMgr.GetComponentData<Item>(item);
			if (componentData3.belongRoomMapPos == MapPos)
			{
				float3 f2 = GetChapter3RepositionChangeValue(ettMgr.GetComponentData<LocalTransform>(item).Position);
				if (!DTool.IsTotallySame(in f2, in float3.zero))
				{
					componentData3.onChapter3Reposition = true;
					componentData3.repositionValue = f2;
					ettMgr.SetComponentData(item, componentData3);
				}
			}
		}
		using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(SpecialObj4Chapter3Reposition));
		foreach (Entity item2 in entityQuery2.ToEntityArray(Allocator.Temp))
		{
			if (ettMgr.GetComponentData<IRoomCtrller_Dots>(item2).belongRoom.Value.MapPos == LevelMgr.Inst.CurrentRoomMapPos)
			{
				SpecialObj4Chapter3Reposition componentData4 = ettMgr.GetComponentData<SpecialObj4Chapter3Reposition>(item2);
				float3 f3 = GetChapter3RepositionChangeValue(ettMgr.GetComponentData<LocalTransform>(item2).Position);
				if (!DTool.IsTotallySame(in f3, in float3.zero))
				{
					componentData4.onChapter3Reposition = true;
					componentData4.repositionValue = f3;
					ettMgr.SetComponentData(item2, componentData4);
				}
			}
		}
		TryRepositionTargets(TeammateEttList);
		TryRepositionTargets(TeammateNotAttackEttList);
		float3 f4 = GetChapter3RepositionChangeValue(ettMgr.GetComponentData<LocalToWorld>(PlayerMgr.Inst.PlayerEtt).Position);
		if (!DTool.IsTotallySame(in f4, in float3.zero))
		{
			if (PlayerMgr.Inst.inDashSpell)
			{
				PlayerMgr.Inst.inDashSpellAccessT6 = true;
			}
			PlayerMgr.Inst.PlayerCtrller.Theme6Reposition(f4);
		}
	}

	private void TryRepositionTargets(List<Entity> targetList)
	{
		foreach (Entity target in targetList)
		{
			if (!ettMgr.HasComponent<LocalTransform>(target))
			{
				continue;
			}
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(target);
			float3 f = GetChapter3RepositionChangeValue(componentData.Position);
			if (DTool.IsTotallySame(in f, in float3.zero))
			{
				continue;
			}
			float3 @float = componentData.Position + f;
			ettMgr.SetComponentData(target, new LocalTransform
			{
				Position = @float,
				Scale = componentData.Scale,
				Rotation = componentData.Rotation
			});
			if (ettMgr.HasComponent<Spell2002Data>(target))
			{
				DynamicBuffer<LegsData> buffer = ettMgr.GetBuffer<LegsData>(target);
				for (int i = 0; i < buffer.Length; i++)
				{
					LegsData value = buffer[i];
					value.MoveToEndPoint = @float;
					value.CurrentEndPoint = @float;
					value.MoveBeforeEndPoint = @float;
					buffer[i] = value;
				}
			}
		}
	}

	public Vector3 GetChapter3RepositionChangeValue(Transform repositionT)
	{
		if (repositionT.position.x < CenterPoint.x - RoomScale.x / 2f)
		{
			return new Vector3(RoomScale.x, 0f, 0f);
		}
		if (repositionT.position.x > CenterPoint.x + RoomScale.x / 2f)
		{
			return new Vector3(0f - RoomScale.x, 0f, 0f);
		}
		if (repositionT.position.y < CenterPoint.y - RoomScale.y / 2f)
		{
			return new Vector3(0f, RoomScale.y, 0f);
		}
		if (repositionT.position.y > CenterPoint.y + RoomScale.y / 2f)
		{
			return new Vector3(0f, 0f - RoomScale.y, 0f);
		}
		return Vector3.zero;
	}

	public float3 GetChapter3RepositionChangeValue(float3 position)
	{
		if (position.x < CenterPoint.x - RoomScale.x / 2f)
		{
			return new float3(RoomScale.x, 0f, 0f);
		}
		if (position.x > CenterPoint.x + RoomScale.x / 2f)
		{
			return new float3(0f - RoomScale.x, 0f, 0f);
		}
		if (position.y < CenterPoint.y - RoomScale.y / 2f)
		{
			return new float3(0f, RoomScale.y, 0f);
		}
		if (position.y > CenterPoint.y + RoomScale.y / 2f)
		{
			return new float3(0f, 0f - RoomScale.y, 0f);
		}
		return float3.zero;
	}

	private void CreateAccess(SceneEttBED sceneEtts, FourDir dir)
	{
		bool needKey = false;
		Entity entity;
		float3 position;
		switch (dir)
		{
		default:
			entity = ettMgr.Instantiate(sceneEtts.ett_AccessU);
			position = base.transform.position + roomCfg.accessUp.GetVector3() + new Vector3(0f, 1f, 0f);
			break;
		case FourDir.Down:
			entity = ettMgr.Instantiate(sceneEtts.ett_AccessD);
			position = base.transform.position + roomCfg.accessDown.GetVector3() + new Vector3(0f, -1f, 0f);
			break;
		case FourDir.Right:
			entity = ettMgr.Instantiate(sceneEtts.ett_AccessR);
			position = base.transform.position + roomCfg.accessRight.GetVector3() + new Vector3(1f, 0f, 0f);
			goto IL_0173;
		case FourDir.Left:
			{
				entity = ettMgr.Instantiate(sceneEtts.ett_AccessL);
				position = base.transform.position + roomCfg.accessLeft.GetVector3() + new Vector3(-1f, 0f, 0f);
				goto IL_0173;
			}
			IL_0173:
			if (MapPos.x == 0 && (roomCfg.type == RoomType.Monster || roomCfg.type == RoomType.Shortcut))
			{
				int num = 0;
				num = ((dir != FourDir.Right) ? LevelMgr.Inst.RoomCfgs[MapPos + new Vector2Int(-1, 0)].id : LevelMgr.Inst.RoomCfgs[MapPos + new Vector2Int(1, 0)].id);
				if (num != 221 && num != 222)
				{
					needKey = true;
				}
			}
			break;
		}
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
		componentData.Position = position;
		ettMgr.SetComponentData(entity, componentData);
		AccessBase_Dots componentData2 = ettMgr.GetComponentData<AccessBase_Dots>(entity);
		componentData2.Dir = dir;
		componentData2.roomType = roomCfg.type;
		componentData2.themeType = roomCfg.themeType;
		componentData2.needKey = needKey;
		ettMgr.SetComponentData(entity, componentData2);
		accessEttList.Add(entity);
	}

	private void RemoveBoundaryOr2(Vector2Data v2Data)
	{
		if (roomCfg.boundarys.Contains(v2Data))
		{
			roomCfg.boundarys.Remove(v2Data);
		}
		if (roomCfg.boundary2s.Contains(v2Data))
		{
			roomCfg.boundary2s.Remove(v2Data);
		}
	}

	private void AddBoundary(Vector2Data v2Data)
	{
		if (!roomCfg.boundarys.Contains(v2Data))
		{
			roomCfg.boundarys.Add(v2Data);
		}
	}

	private void AddBoundary2(Vector2Data v2Data)
	{
		if (!roomCfg.boundary2s.Contains(v2Data))
		{
			roomCfg.boundary2s.Add(v2Data);
		}
	}

	private void CreateEmptyObstacle(Vector2Data v2Data)
	{
		UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/EmptyObstacle"), base.transform.position + v2Data.GetVector3(), Quaternion.identity, tsf_Action);
	}

	private void IndexOfHaveObjWave()
	{
		if (currentWave >= 10)
		{
			return;
		}
		for (int i = currentWave; i < 10; i++)
		{
			if (roomCfg.allObjList[i].Count > 0)
			{
				currentWave = i;
				return;
			}
		}
		currentWave = 10;
	}

	public void CreateUnit(Vector3 worldPosition, int id, Action<UnitProperty> onBorn = null)
	{
		MonsterBorn component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", worldPosition).GetComponent<MonsterBorn>();
		MonsterBornRegister(component);
		RoomObjData objData = new RoomObjData(RoomObjType.Unit, id);
		component.Initialize(this, objData, 0f, immediatelyCreate: false, isDoubleEnemy: false);
		if (onBorn != null)
		{
			component.OnBorn = (Action<UnitProperty>)Delegate.Combine(component.OnBorn, onBorn);
		}
	}

	private void CreateWave(bool immediatelyCreate)
	{
		if (currentWave >= 10)
		{
			Debug.LogError("!");
		}
		if (roomCfg.allObjList[currentWave].Count == 0)
		{
			Debug.LogError("!");
		}
		bool isDoubleEnemy = false;
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_DoubleEnemy != null && UnityEngine.Random.value * 100f <= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_DoubleEnemy.int1.result)
		{
			isDoubleEnemy = true;
		}
		for (int i = 0; i < roomCfg.allObjList[currentWave].Count; i++)
		{
			if (roomCfg.allObjList[currentWave][i].objType == RoomObjType.Unit)
			{
				MonsterBorn component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", base.transform.position + roomCfg.allObjList[currentWave][i].GetFinalVector3()).GetComponent<MonsterBorn>();
				MonsterBornRegister(component);
				if (GameMgr.IsHarmony_Static)
				{
					ref int id = ref roomCfg.allObjList[currentWave][i].id;
					if (id / 100 == 1041)
					{
						id = 102300 + id - 104100;
					}
					if (GameMgr.IsChAge14_Static && id / 100 == 1056)
					{
						if (id == 105601 || id == 105602)
						{
							id = 101500 + id - 105600;
						}
						else
						{
							id = 101520 + id - 105600;
						}
					}
				}
				if (!immediatelyCreate && (roomCfg.allObjList[currentWave][i].id == 102001 || roomCfg.allObjList[currentWave][i].id == 102002 || roomCfg.allObjList[currentWave][i].id == 102003 || roomCfg.allObjList[currentWave][i].id == 102004 || roomCfg.allObjList[currentWave][i].id == 102041 || roomCfg.allObjList[currentWave][i].id == 102042 || roomCfg.allObjList[currentWave][i].id == 102043 || roomCfg.allObjList[currentWave][i].id == 102044))
				{
					float num = Monster20.bornEffectIntervalLarge;
					float num2 = Monster20.bornEffectCountLarge;
					if (roomCfg.allObjList[currentWave][i].id == 102001 || roomCfg.allObjList[currentWave][i].id == 102002 || roomCfg.allObjList[currentWave][i].id == 102041 || roomCfg.allObjList[currentWave][i].id == 102042)
					{
						num = Monster20.bornEffectInterval;
						num2 = Monster20.bornEffectCount;
					}
					if (roomCfg.allObjList[currentWave][i].extraData1 >= 1f)
					{
						num2 = roomCfg.allObjList[currentWave][i].extraData1;
					}
					float num3 = num2 * num;
					Vector3 dir = Tool2D.GetDir(roomCfg.allObjList[currentWave][i].extraData2);
					if (LevelMgr.Inst.CurrentRoomCfg.isFlipped)
					{
						dir.x = 0f - dir.x;
					}
					for (int j = 1; (float)j < num3 + 1f; j++)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", base.transform.position + roomCfg.allObjList[currentWave][i].GetFinalVector3() + dir * j).GetComponent<MonsterBorn>().SetForShow((float)i * roomCfg.monsterBornInterval);
					}
				}
				List<ItemInfo> dropItemInfos = null;
				if (extraDrops != null && extraDrops.ContainsKey(new Vector2Int(currentWave, i)))
				{
					dropItemInfos = extraDrops[new Vector2Int(currentWave, i)];
				}
				component.Initialize(this, roomCfg.allObjList[currentWave][i], (float)i * roomCfg.monsterBornInterval, immediatelyCreate, isDoubleEnemy, dropItemInfos);
			}
			else
			{
				Debug.Log("当前波次：" + currentWave);
				Debug.Log("创建位置：" + roomCfg.allObjList[currentWave][i].GetFinalVector3());
				Debug.LogError("levelID:" + roomCfg.id + " 理论上不允许在波次中刷出Unit以外的obj");
			}
		}
	}

	public void Initialize(Vector2Int mapPoint, RoomConfig roomCfg)
	{
		MapPos = mapPoint;
		this.roomCfg = roomCfg;
	}

	public void Initialize2()
	{
		CenterPoint = base.transform.position + new Vector3((float)(roomCfg.localMinX + roomCfg.localMaxX) / 2f, (float)(roomCfg.localMinY + roomCfg.localMaxY) / 2f, 0f);
		RoomScale = new Vector2(roomCfg.localMaxX - roomCfg.localMinX + 1, roomCfg.localMaxY - roomCfg.localMinY + 1);
		if (roomCfg.themeType == RoomThemeType.Theme14_CustomRectangleEmptyScene || roomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || roomCfg.themeType == RoomThemeType.theme27_Store_Dave || roomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || roomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			CenterPoint = base.transform.position;
			RoomScale = new Vector2(roomCfg.themeEmptyWidth, roomCfg.themeEmptyHeight);
		}
		else if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			CenterPoint = base.transform.position;
			RoomScale = new Vector2(roomCfg.theme6Width, roomCfg.theme6Height);
		}
		else if (roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			CenterPoint = base.transform.position;
			RoomScale = new Vector2(roomCfg.theme8Width, roomCfg.theme8Height);
		}
	}

	public void Generate()
	{
		fogCtrller.Initialize(this);
		IndexOfHaveObjWave();
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllSceneEtt));
		AllSceneEtt singleton = entityQuery.GetSingleton<AllSceneEtt>();
		using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(SceneEttBED));
		SceneEttBED sceneEtts = entityQuery2.GetSingletonBuffer<SceneEttBED>()[(int)roomCfg.themeType];
		BlobAssetReference<BlobArray<Vector2Data>> allTile0Position = default(BlobAssetReference<BlobArray<Vector2Data>>);
		int themeType = (int)roomCfg.themeType;
		string text = "Prefabs/Scene/Theme" + themeType;
		themeType = (int)roomCfg.themeType;
		string text2 = "Prefabs/Scene/Theme" + themeType + "_H";
		if (DataMgr.selectedWorldData.inBattle9 && roomCfg.isFinalRoom)
		{
			List<ItemInfo> extraDrop = OutputMgr_Dots.GetExtraDrop(LevelMgr.Inst.CurrentRewardType);
			if (extraDrop.Count > 0)
			{
				int num = 0;
				List<int> list = new List<int>();
				for (int i = 1; i < roomCfg.allObjList.Count; i++)
				{
					if (roomCfg.allObjList[i].Count > 0)
					{
						num += roomCfg.allObjList[i].Count;
						list.Add(i);
					}
				}
				if (list.Count > 0)
				{
					extraDrops = new Dictionary<Vector2Int, List<ItemInfo>>();
					if (num < extraDrop.Count)
					{
						for (int j = 0; j < extraDrop.Count; j++)
						{
							int num2 = list[UnityEngine.Random.Range(0, list.Count)];
							int y = UnityEngine.Random.Range(0, roomCfg.allObjList[num2].Count);
							if (extraDrops.ContainsKey(new Vector2Int(num2, y)))
							{
								extraDrops[new Vector2Int(num2, y)].Add(extraDrop[j]);
								continue;
							}
							extraDrops[new Vector2Int(num2, y)] = new List<ItemInfo> { extraDrop[j] };
						}
					}
					else
					{
						for (int k = 0; k < extraDrop.Count; k++)
						{
							int num3 = 0;
							while (true)
							{
								num3++;
								if (num3 >= 100)
								{
									Debug.LogWarning("??");
									break;
								}
								int num4 = list[UnityEngine.Random.Range(0, list.Count)];
								int y2 = UnityEngine.Random.Range(0, roomCfg.allObjList[num4].Count);
								if (!extraDrops.ContainsKey(new Vector2Int(num4, y2)))
								{
									extraDrops.Add(new Vector2Int(num4, y2), new List<ItemInfo> { extraDrop[k] });
									break;
								}
							}
						}
					}
				}
				else
				{
					MonoBehaviour.print("level ID:" + roomCfg.id + " 没有任何怪物，所以无法投放额外掉落物");
				}
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			UnityEngine.Object.Destroy(go_MRParent);
		}
		else
		{
			Vector2Int vector2Int = new Vector2Int((int)(RoomScale.x * (float)pixelCountPerMeter), (int)(RoomScale.y * (float)pixelCountPerMeter));
			RenderTexture renderTexture = new RenderTexture(vector2Int.x, vector2Int.y, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D16_UNorm);
			RenderTexture renderTexture2 = new RenderTexture(vector2Int.x, vector2Int.y, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D16_UNorm);
			RenderTexture renderTexture3 = new RenderTexture(vector2Int.x, vector2Int.y, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D16_UNorm);
			rtcam_Water = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), base.transform).GetComponent<RTCamController>();
			rtcam_Mucus = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), base.transform).GetComponent<RTCamController>();
			rtcam_Venom = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), base.transform).GetComponent<RTCamController>();
			rtcam_Venom.MaxFps = 20;
			rtcam_Water.cam.transform.parent.position = Tool2D.IgnoreZPoint(CenterPoint, -110f);
			rtcam_Mucus.cam.transform.parent.position = Tool2D.IgnoreZPoint(CenterPoint, -120f);
			rtcam_Venom.cam.transform.parent.position = Tool2D.IgnoreZPoint(CenterPoint, -130f);
			rtcam_Water.cam.targetTexture = renderTexture;
			rtcam_Mucus.cam.targetTexture = renderTexture2;
			rtcam_Venom.cam.targetTexture = renderTexture3;
			rtcam_Water.cam.orthographicSize = RoomScale.y / 2f;
			rtcam_Mucus.cam.orthographicSize = RoomScale.y / 2f;
			rtcam_Venom.cam.orthographicSize = RoomScale.y / 2f;
			rtcam_Water.MaxFps = 0;
			rtcam_Mucus.MaxFps = 0;
			mr_Water.transform.position = Tool2D.IgnoreZPoint(CenterPoint, 1.18f);
			mr_Mucus.transform.position = Tool2D.IgnoreZPoint(CenterPoint, 1.16f);
			mr_Venom.transform.position = Tool2D.IgnoreZPoint(CenterPoint, 1.15f);
			mr_Water.transform.localScale = Tool2D.IgnoreZPoint(RoomScale, 1f);
			mr_Mucus.transform.localScale = Tool2D.IgnoreZPoint(RoomScale, 1f);
			mr_Venom.transform.localScale = Tool2D.IgnoreZPoint(RoomScale, 1f);
			mr_Water.material.SetTexture("_MainTex", renderTexture);
			mr_Mucus.material.SetTexture("_MainTex", renderTexture2);
			mr_Venom.material.SetTexture("_MainTex", renderTexture3);
			venomCtrller.Initialize(this);
		}
		List<Vector2Data> list2 = new List<Vector2Data>();
		List<Vector2Data> list3 = new List<Vector2Data>();
		List<Vector2Data> list4 = new List<Vector2Data>();
		if (roomCfg.themeType != RoomThemeType.Theme14_CustomRectangleEmptyScene)
		{
			if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1 || roomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || roomCfg.themeType == RoomThemeType.theme27_Store_Dave || roomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || roomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
			{
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 0, 1))
				{
					CreateAccess(sceneEtts, FourDir.Up);
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 1, 0))
				{
					CreateAccess(sceneEtts, FourDir.Right);
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, -1, 0))
				{
					CreateAccess(sceneEtts, FourDir.Left);
				}
			}
			else if (roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
			{
				float num5 = Mathf.Min(roomCfg.theme8Width, roomCfg.theme8Height);
				float num6 = Mathf.Max(roomCfg.theme8Width, roomCfg.theme8Height);
				float num7 = Mathf.Min(roomCfg.theme8CornerRadius, num5 / 2f);
				float num8 = Mathf.Abs(roomCfg.theme8Width - roomCfg.theme8Height);
				float value = num7 / (num8 * num8 / 15f + num6 - num8);
				mr_Water.material.SetFloat("_RoundedRectangleRadius", value);
				mr_Mucus.material.SetFloat("_RoundedRectangleRadius", value);
				mr_Venom.material.SetFloat("_RoundedRectangleRadius", value);
			}
			else
			{
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 0, 1))
				{
					for (int l = 0; l < LevelMgr.Inst.roomAccessExtraDistance; l++)
					{
						roomCfg.allTileList[0].Add(roomCfg.accessUp + new Vector2Data(0f, l + 1));
						roomCfg.allTileList[0].Add(roomCfg.accessUp + new Vector2Data(1f, l + 1));
					}
					CreateAccess(sceneEtts, FourDir.Up);
					for (int m = 1; m < LevelMgr.Inst.roomAccessExtraDistance; m++)
					{
						CreateEmptyObstacle(roomCfg.accessUp + new Vector2Data(0f, m + 1));
						CreateEmptyObstacle(roomCfg.accessUp + new Vector2Data(1f, m + 1));
					}
					for (int n = 0; n < LevelMgr.Inst.roomAccessExtraDistance; n++)
					{
						RemoveBoundaryOr2(roomCfg.accessUp + new Vector2Data(0f, n + 1));
						RemoveBoundaryOr2(roomCfg.accessUp + new Vector2Data(1f, n + 1));
					}
					for (int num9 = 0; num9 < LevelMgr.Inst.roomAccessExtraDistance + 1; num9++)
					{
						AddBoundary(roomCfg.accessUp + new Vector2Data(-1f, num9 + 1));
						AddBoundary(roomCfg.accessUp + new Vector2Data(2f, num9 + 1));
						list4.Add(roomCfg.accessUp + new Vector2Data(-1f, num9 + 1));
						list4.Add(roomCfg.accessUp + new Vector2Data(2f, num9 + 1));
						if (num9 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list2.Add(roomCfg.accessUp + new Vector2Data(-1f, num9 + 1));
							list2.Add(roomCfg.accessUp + new Vector2Data(2f, num9 + 1));
						}
					}
					for (int num10 = 0; num10 < LevelMgr.Inst.roomAccessExtraDistance + 1; num10++)
					{
						AddBoundary2(roomCfg.accessUp + new Vector2Data(-2f, num10 + 1));
						AddBoundary2(roomCfg.accessUp + new Vector2Data(3f, num10 + 1));
						if (num10 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list3.Add(roomCfg.accessUp + new Vector2Data(-2f, num10 + 1));
							list3.Add(roomCfg.accessUp + new Vector2Data(3f, num10 + 1));
						}
					}
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 1, 0))
				{
					for (int num11 = 0; num11 < LevelMgr.Inst.roomAccessExtraDistance; num11++)
					{
						roomCfg.allTileList[0].Add(roomCfg.accessRight + new Vector2Data(num11 + 1, 0f));
						roomCfg.allTileList[0].Add(roomCfg.accessRight + new Vector2Data(num11 + 1, 1f));
					}
					CreateAccess(sceneEtts, FourDir.Right);
					for (int num12 = 1; num12 < LevelMgr.Inst.roomAccessExtraDistance; num12++)
					{
						CreateEmptyObstacle(roomCfg.accessRight + new Vector2Data(num12 + 1, 0f));
						CreateEmptyObstacle(roomCfg.accessRight + new Vector2Data(num12 + 1, 1f));
					}
					for (int num13 = 0; num13 < LevelMgr.Inst.roomAccessExtraDistance; num13++)
					{
						RemoveBoundaryOr2(roomCfg.accessRight + new Vector2Data(num13 + 1, 0f));
						RemoveBoundaryOr2(roomCfg.accessRight + new Vector2Data(num13 + 1, 1f));
					}
					for (int num14 = 0; num14 < LevelMgr.Inst.roomAccessExtraDistance + 1; num14++)
					{
						AddBoundary(roomCfg.accessRight + new Vector2Data(num14 + 1, -1f));
						AddBoundary(roomCfg.accessRight + new Vector2Data(num14 + 1, 2f));
						list4.Add(roomCfg.accessRight + new Vector2Data(num14 + 1, -1f));
						list4.Add(roomCfg.accessRight + new Vector2Data(num14 + 1, 2f));
						if (num14 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list2.Add(roomCfg.accessRight + new Vector2Data(num14 + 1, -1f));
							list2.Add(roomCfg.accessRight + new Vector2Data(num14 + 1, 2f));
						}
					}
					for (int num15 = 0; num15 < LevelMgr.Inst.roomAccessExtraDistance + 1; num15++)
					{
						AddBoundary2(roomCfg.accessRight + new Vector2Data(num15 + 1, -2f));
						AddBoundary2(roomCfg.accessRight + new Vector2Data(num15 + 1, 3f));
						if (num15 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list3.Add(roomCfg.accessRight + new Vector2Data(num15 + 1, -2f));
							list3.Add(roomCfg.accessRight + new Vector2Data(num15 + 1, 3f));
						}
					}
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 0, -1))
				{
					for (int num16 = 0; num16 < LevelMgr.Inst.roomAccessExtraDistance; num16++)
					{
						roomCfg.allTileList[0].Add(roomCfg.accessDown + new Vector2Data(0f, -num16 - 1));
						roomCfg.allTileList[0].Add(roomCfg.accessDown + new Vector2Data(1f, -num16 - 1));
					}
					CreateAccess(sceneEtts, FourDir.Down);
					for (int num17 = 1; num17 < LevelMgr.Inst.roomAccessExtraDistance; num17++)
					{
						CreateEmptyObstacle(roomCfg.accessDown + new Vector2Data(0f, -num17 - 1));
						CreateEmptyObstacle(roomCfg.accessDown + new Vector2Data(1f, -num17 - 1));
					}
					for (int num18 = 0; num18 < LevelMgr.Inst.roomAccessExtraDistance; num18++)
					{
						RemoveBoundaryOr2(roomCfg.accessDown + new Vector2Data(0f, -num18 - 1));
						RemoveBoundaryOr2(roomCfg.accessDown + new Vector2Data(1f, -num18 - 1));
					}
					for (int num19 = 0; num19 < LevelMgr.Inst.roomAccessExtraDistance + 1; num19++)
					{
						AddBoundary(roomCfg.accessDown + new Vector2Data(-1f, -num19 - 1));
						AddBoundary(roomCfg.accessDown + new Vector2Data(2f, -num19 - 1));
						list4.Add(roomCfg.accessDown + new Vector2Data(-1f, -num19 - 1));
						list4.Add(roomCfg.accessDown + new Vector2Data(2f, -num19 - 1));
						if (num19 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list2.Add(roomCfg.accessDown + new Vector2Data(-1f, -num19 - 1));
							list2.Add(roomCfg.accessDown + new Vector2Data(2f, -num19 - 1));
						}
					}
					for (int num20 = 0; num20 < LevelMgr.Inst.roomAccessExtraDistance + 1; num20++)
					{
						AddBoundary2(roomCfg.accessDown + new Vector2Data(-2f, -num20 - 1));
						AddBoundary2(roomCfg.accessDown + new Vector2Data(3f, -num20 - 1));
						if (num20 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list3.Add(roomCfg.accessDown + new Vector2Data(-2f, -num20 - 1));
							list3.Add(roomCfg.accessDown + new Vector2Data(3f, -num20 - 1));
						}
					}
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, -1, 0))
				{
					for (int num21 = 0; num21 < LevelMgr.Inst.roomAccessExtraDistance; num21++)
					{
						roomCfg.allTileList[0].Add(roomCfg.accessLeft + new Vector2Data(-num21 - 1, 0f));
						roomCfg.allTileList[0].Add(roomCfg.accessLeft + new Vector2Data(-num21 - 1, 1f));
					}
					CreateAccess(sceneEtts, FourDir.Left);
					for (int num22 = 1; num22 < LevelMgr.Inst.roomAccessExtraDistance; num22++)
					{
						CreateEmptyObstacle(roomCfg.accessLeft + new Vector2Data(-num22 - 1, 0f));
						CreateEmptyObstacle(roomCfg.accessLeft + new Vector2Data(-num22 - 1, 1f));
					}
					for (int num23 = 0; num23 < LevelMgr.Inst.roomAccessExtraDistance; num23++)
					{
						RemoveBoundaryOr2(roomCfg.accessLeft + new Vector2Data(-num23 - 1, 0f));
						RemoveBoundaryOr2(roomCfg.accessLeft + new Vector2Data(-num23 - 1, 1f));
					}
					for (int num24 = 0; num24 < LevelMgr.Inst.roomAccessExtraDistance + 1; num24++)
					{
						AddBoundary(roomCfg.accessLeft + new Vector2Data(-num24 - 1, -1f));
						AddBoundary(roomCfg.accessLeft + new Vector2Data(-num24 - 1, 2f));
						list4.Add(roomCfg.accessLeft + new Vector2Data(-num24 - 1, -1f));
						list4.Add(roomCfg.accessLeft + new Vector2Data(-num24 - 1, 2f));
						if (num24 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list2.Add(roomCfg.accessLeft + new Vector2Data(-num24 - 1, -1f));
							list2.Add(roomCfg.accessLeft + new Vector2Data(-num24 - 1, 2f));
						}
					}
					for (int num25 = 0; num25 < LevelMgr.Inst.roomAccessExtraDistance + 1; num25++)
					{
						AddBoundary2(roomCfg.accessLeft + new Vector2Data(-num25 - 1, -2f));
						AddBoundary2(roomCfg.accessLeft + new Vector2Data(-num25 - 1, 3f));
						if (num25 == LevelMgr.Inst.roomAccessExtraDistance)
						{
							list3.Add(roomCfg.accessLeft + new Vector2Data(-num25 - 1, -2f));
							list3.Add(roomCfg.accessLeft + new Vector2Data(-num25 - 1, 3f));
						}
					}
				}
			}
		}
		List<GameObject> list5 = new List<GameObject>();
		List<GameObject> list6 = new List<GameObject>();
		List<GameObject> list7 = new List<GameObject>();
		Tile_T8 tile_T = null;
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavAction"), CenterPoint, Quaternion.identity, tsf_Action).transform.localScale = new Vector3(27f, 17f, 1f);
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGround"), CenterPoint, Quaternion.identity, tsf_Ground).transform.localScale = new Vector3(27f, 17f, 1f);
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFly"), CenterPoint, Quaternion.identity, tsf_Fly).transform.localScale = new Vector3(27f, 17f, 1f);
			UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text + "/Tile0", text2 + "/Tile0"), base.transform.position, Quaternion.identity, base.transform).GetComponent<Tile_T6>().TileCorrect(this);
		}
		else if (roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			tile_T = UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text + "/Tile0", text2 + "/Tile0"), base.transform.position, Quaternion.identity, base.transform).GetComponent<Tile_T8>();
			tile_T.TileCorrect(this);
			for (int num26 = 1; num26 < roomCfg.allTileList.Count; num26++)
			{
				if (roomCfg.allTileList[num26].Count != 0)
				{
					Entity srcEntity = num26 switch
					{
						2 => sceneEtts.ett_Tile2, 
						3 => sceneEtts.ett_Tile3, 
						4 => sceneEtts.ett_Tile4, 
						5 => sceneEtts.ett_Tile5, 
						6 => sceneEtts.ett_Tile6, 
						7 => sceneEtts.ett_Tile7, 
						8 => sceneEtts.ett_Tile8, 
						9 => sceneEtts.ett_Tile9, 
						_ => sceneEtts.ett_Tile1, 
					};
					NativeArray<Entity> outputEntities = new NativeArray<Entity>(roomCfg.allTileList[num26].Count, Allocator.Temp);
					ettMgr.Instantiate(srcEntity, outputEntities);
					BlobAssetReference<BlobArray<Vector2Data>> allTilePosition = DTool.ListToBlobArray(roomCfg.allTileList[num26]);
					for (int num27 = 0; num27 < outputEntities.Length; num27++)
					{
						TileBase_Dots componentData = ettMgr.GetComponentData<TileBase_Dots>(outputEntities[num27]);
						componentData.roomPosition = base.transform.position;
						componentData.selfPosition = roomCfg.allTileList[num26][num27];
						componentData.allTilePosition = allTilePosition;
						ettMgr.SetComponentData(outputEntities[num27], componentData);
					}
				}
			}
		}
		else if (roomCfg.themeType == RoomThemeType.Theme14_CustomRectangleEmptyScene || roomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || roomCfg.themeType == RoomThemeType.theme27_Store_Dave || roomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || roomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavAction"), CenterPoint, Quaternion.identity, tsf_Action).transform.localScale = new Vector3(roomCfg.themeEmptyWidth, roomCfg.themeEmptyHeight, 1f);
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGround"), CenterPoint, Quaternion.identity, tsf_Ground).transform.localScale = new Vector3(roomCfg.themeEmptyWidth, roomCfg.themeEmptyHeight, 1f);
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFly"), CenterPoint, Quaternion.identity, tsf_Fly).transform.localScale = new Vector3(roomCfg.themeEmptyWidth, roomCfg.themeEmptyHeight, 1f);
		}
		else
		{
			for (int num28 = 0; num28 < roomCfg.allTileList.Count; num28++)
			{
				if (roomCfg.allTileList[num28].Count == 0)
				{
					continue;
				}
				Entity srcEntity2 = num28 switch
				{
					1 => sceneEtts.ett_Tile1, 
					2 => sceneEtts.ett_Tile2, 
					3 => sceneEtts.ett_Tile3, 
					4 => sceneEtts.ett_Tile4, 
					5 => sceneEtts.ett_Tile5, 
					6 => sceneEtts.ett_Tile6, 
					7 => sceneEtts.ett_Tile7, 
					8 => sceneEtts.ett_Tile8, 
					9 => sceneEtts.ett_Tile9, 
					_ => sceneEtts.ett_Tile0, 
				};
				NativeArray<Entity> outputEntities2 = new NativeArray<Entity>(roomCfg.allTileList[num28].Count, Allocator.Temp);
				ettMgr.Instantiate(srcEntity2, outputEntities2);
				BlobAssetReference<BlobArray<Vector2Data>> blobAssetReference = DTool.ListToBlobArray(roomCfg.allTileList[num28]);
				for (int num29 = 0; num29 < outputEntities2.Length; num29++)
				{
					if (num28 == 0)
					{
						Vector3 position = base.transform.position + roomCfg.allTileList[num28][num29].GetVector3();
						list5.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavAction"), position, Quaternion.identity, tsf_Action));
						list6.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGround"), position, Quaternion.identity, tsf_Ground));
						list7.Add(UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFly"), position, Quaternion.identity, tsf_Fly));
					}
					TileBase_Dots componentData2 = ettMgr.GetComponentData<TileBase_Dots>(outputEntities2[num29]);
					componentData2.roomPosition = base.transform.position;
					componentData2.selfPosition = roomCfg.allTileList[num28][num29];
					componentData2.allTilePosition = blobAssetReference;
					ettMgr.SetComponentData(outputEntities2[num29], componentData2);
				}
				if (num28 == 0)
				{
					allTile0Position = blobAssetReference;
				}
			}
		}
		_ = text + "/Door";
		_ = text2 + "/Door";
		for (int num30 = 0; num30 < roomCfg.allObjList[0].Count; num30++)
		{
			Entity entity = Entity.Null;
			GameObject gameObject = null;
			switch (roomCfg.allObjList[0][num30].objType)
			{
			case RoomObjType.Unit:
				if (UnitConfig.map[roomCfg.allObjList[0][num30].id].unitType == UnitType.Monster || UnitConfig.map[roomCfg.allObjList[0][num30].id].unitType == UnitType.Elite || UnitConfig.map[roomCfg.allObjList[0][num30].id].unitType == UnitType.Boss)
				{
					Debug.LogWarning("levelID:" + roomCfg.id + "  Obj0不应该有怪物");
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", base.transform.position + roomCfg.allObjList[0][num30].GetFinalVector3()).GetComponent<MonsterBorn>().Initialize(this, roomCfg.allObjList[0][num30], 0f, immediatelyCreate: true, isDoubleEnemy: false);
				}
				break;
			case RoomObjType.SpecialObj:
				switch (roomCfg.allObjList[0][num30].id)
				{
				case 11:
					if (!roomCfg.isFinalRoom || (roomCfg.type == RoomType.Boss && DataMgr.selectedWorldData.battleData9.currentStage == 6 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy) || (roomCfg.type == RoomType.Boss && DataMgr.selectedWorldData.battleData9.currentStage == 8 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Normal) || roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle || LevelMgr.Inst.NextRewardTypes == null)
					{
						break;
					}
					if (LevelMgr.Inst.NextRewardTypes.Count == 1)
					{
						Entity entity3 = ettMgr.Instantiate(sceneEtts.ett_Door);
						LocalTransform componentData5 = ettMgr.GetComponentData<LocalTransform>(entity3);
						componentData5.Position = base.transform.position + roomCfg.accessUp.GetVector3() + GameConst.doorOffsetAlone;
						ettMgr.SetComponentData(entity3, componentData5);
						DoorBase_Dots componentData6 = ettMgr.GetComponentData<DoorBase_Dots>(entity3);
						componentData6.rewardType = LevelMgr.Inst.NextRewardTypes[0];
						ettMgr.SetComponentData(entity3, componentData6);
						doorEttList.Add(entity3);
					}
					else if (LevelMgr.Inst.NextRewardTypes.Count == 2)
					{
						for (int num31 = 0; num31 < 2; num31++)
						{
							Entity entity4 = ettMgr.Instantiate(sceneEtts.ett_Door);
							LocalTransform componentData7 = ettMgr.GetComponentData<LocalTransform>(entity4);
							componentData7.Position = base.transform.position + roomCfg.accessUp.GetVector3();
							if (num31 == 0)
							{
								componentData7.Position += GameConst.doorOffsetDoubleLeft.GetFloat3();
							}
							else
							{
								componentData7.Position += GameConst.doorOffsetDoubleRight.GetFloat3();
							}
							ettMgr.SetComponentData(entity4, componentData7);
							DoorBase_Dots componentData8 = ettMgr.GetComponentData<DoorBase_Dots>(entity4);
							componentData8.rewardType = LevelMgr.Inst.NextRewardTypes[num31];
							ettMgr.SetComponentData(entity4, componentData8);
							doorEttList.Add(entity4);
						}
					}
					else if (LevelMgr.Inst.NextRewardTypes.Count == 3)
					{
						for (int num32 = 0; num32 < 3; num32++)
						{
							Entity entity5 = ettMgr.Instantiate(sceneEtts.ett_Door);
							LocalTransform componentData9 = ettMgr.GetComponentData<LocalTransform>(entity5);
							componentData9.Position = base.transform.position + roomCfg.accessUp.GetVector3() + new Vector3((float)(-(LevelMgr.Inst.NextRewardTypes.Count - 1)) / 2f * GameConst.doorOffsetX + GameConst.doorOffsetX * (float)num32 + 0.5f, GameConst.doorOffsetY, 0f);
							ettMgr.SetComponentData(entity5, componentData9);
							DoorBase_Dots componentData10 = ettMgr.GetComponentData<DoorBase_Dots>(entity5);
							componentData10.rewardType = LevelMgr.Inst.NextRewardTypes[num32];
							ettMgr.SetComponentData(entity5, componentData10);
							doorEttList.Add(entity5);
						}
					}
					else
					{
						Debug.LogError("为什么门会大于3个，或为0个" + roomCfg.id);
					}
					break;
				case 21:
				{
					Vector3 vector2 = base.transform.position + roomCfg.allObjList[0][num30].GetFinalVector3();
					entity = ((!roomCfg.generateRO) ? QuickCreateSystem.Inst.CreateSpecialObj(IDMgr.GetObstacleID(roomCfg.themeType), vector2) : QuickCreateSystem.Inst.CreateSpecialObj(OutputMgr.GetRRO(), vector2));
					break;
				}
				case 31:
					if (LevelMgr.Inst.NextExtraDoorRewardType != LevelRewardType.None && LevelMgr.Inst.NextRewardTypes.Count != 1)
					{
						Entity entity2 = ettMgr.Instantiate(sceneEtts.ett_Door);
						LocalTransform componentData3 = ettMgr.GetComponentData<LocalTransform>(entity2);
						componentData3.Position = base.transform.position + roomCfg.extraDoor.GetVector3() + GameConst.doorOffsetAlone;
						ettMgr.SetComponentData(entity2, componentData3);
						DoorBase_Dots componentData4 = ettMgr.GetComponentData<DoorBase_Dots>(entity2);
						componentData4.rewardType = LevelMgr.Inst.NextExtraDoorRewardType;
						componentData4.isExtraDoor = true;
						ettMgr.SetComponentData(entity2, componentData4);
						doorEttList.Add(entity2);
					}
					break;
				case 3601:
					if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.AdvancedScarecrow) != 0)
					{
						gameObject = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + 3601), base.transform.position + roomCfg.allObjList[0][num30].GetFinalVector3(), Quaternion.identity, tsf_Thing);
					}
					break;
				default:
				{
					if (SpecialObjConfig.dic[roomCfg.allObjList[0][num30].id].isHybirdSO)
					{
						gameObject = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + roomCfg.allObjList[0][num30].id), base.transform.position + roomCfg.allObjList[0][num30].GetFinalVector3(), Quaternion.identity, tsf_Thing);
						break;
					}
					Vector3 vector = base.transform.position + roomCfg.allObjList[0][num30].GetFinalVector3();
					entity = QuickCreateSystem.Inst.CreateSpecialObj(roomCfg.allObjList[0][num30].id, vector);
					break;
				}
				case 12:
				case 13:
				case 14:
					break;
				}
				break;
			default:
				Debug.LogError(roomCfg.allObjList[0][num30].objType);
				break;
			}
			if (gameObject != null)
			{
				gameObject.GetComponent<IRoomObjExtraData>()?.SetExtraData(roomCfg.allObjList[0][num30].extraData1, roomCfg.allObjList[0][num30].extraData2, roomCfg.allObjList[0][num30].extraData3);
				gameObject.GetComponent<IRoomCtrller>()?.SetRoomCtrlller(this);
				ITrap component = gameObject.GetComponent<ITrap>();
				if (component != null)
				{
					TrapRegister(component);
				}
			}
			else if (entity != Entity.Null)
			{
				if (ettMgr.HasComponent<IRoomObjExtraData_Dots>(entity))
				{
					IRoomObjExtraData_Dots componentData11 = ettMgr.GetComponentData<IRoomObjExtraData_Dots>(entity);
					componentData11.data1 = roomCfg.allObjList[0][num30].extraData1;
					componentData11.data2 = roomCfg.allObjList[0][num30].extraData2;
					componentData11.data3 = roomCfg.allObjList[0][num30].extraData3;
					ettMgr.SetComponentData(entity, componentData11);
				}
				if (ettMgr.HasComponent<IRoomCtrller_Dots>(entity))
				{
					IRoomCtrller_Dots componentData12 = ettMgr.GetComponentData<IRoomCtrller_Dots>(entity);
					componentData12.belongRoom.Value = this;
					ettMgr.SetComponentData(entity, componentData12);
				}
				if (ettMgr.HasComponent<ITrap_Dots>(entity))
				{
					trapEttList.Add(entity);
					ITrap_Dots componentData13 = ettMgr.GetComponentData<ITrap_Dots>(entity);
					componentData13.belongRoom.Value = this;
					ettMgr.SetComponentData(entity, componentData13);
				}
			}
		}
		Entity ett_Boundary = sceneEtts.ett_Boundary;
		Entity ett_Boundary2 = sceneEtts.ett_Boundary2;
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme14_CustomRectangleEmptyScene || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1 || roomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || roomCfg.themeType == RoomThemeType.theme27_Store_Dave || roomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || roomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			UnityEngine.Object.Destroy(go_EndlessBoundaryParent.gameObject);
			if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
			{
				UnityEngine.Object.Destroy(go_EndlessBoundaryParent.gameObject);
				if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
				{
					UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text + "/Boundary", text2 + "/Boundary"), CenterPoint, Quaternion.identity, tsf_Boundary).GetComponent<BoundaryBase>().Correct(Vector2Data.Zero, this);
				}
			}
		}
		else
		{
			for (int num33 = 0; num33 < list3.Count; num33++)
			{
				if (roomCfg.boundarys.Contains(list3[num33]))
				{
					roomCfg.boundarys.Remove(list3[num33]);
				}
			}
			for (int num34 = 0; num34 < list3.Count; num34++)
			{
				if (roomCfg.boundary2s.Contains(list3[num34]))
				{
					roomCfg.boundary2s.Remove(list3[num34]);
				}
			}
			BlobAssetReference<BlobArray<Vector2Data>> allBoundary1Position = DTool.ListToBlobArray(roomCfg.boundarys);
			BlobAssetReference<BlobArray<Vector2Data>> allBoundary2Position = DTool.ListToBlobArray(roomCfg.boundary2s);
			List<Vector2Data> list8 = new List<Vector2Data>();
			if (roomCfg.isFinalRoom && LevelMgr.Inst.NextRewardTypes != null)
			{
				if (LevelMgr.Inst.NextRewardTypes.Count >= 1)
				{
					list8.Add(roomCfg.accessUp + new Vector2Data(-1f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(0f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(1f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(2f, 1f));
				}
				if (LevelMgr.Inst.NextRewardTypes.Count >= 2)
				{
					list8.Add(roomCfg.accessUp + new Vector2Data(-3f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(-2f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(3f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(4f, 1f));
				}
				if (LevelMgr.Inst.NextRewardTypes.Count >= 3)
				{
					list8.Add(roomCfg.accessUp + new Vector2Data(-5f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(-4f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(5f, 1f));
					list8.Add(roomCfg.accessUp + new Vector2Data(6f, 1f));
				}
			}
			if (LevelMgr.Inst.NextExtraDoorRewardType != LevelRewardType.None && roomCfg.extraDoor != Vector2Data.Up1000)
			{
				list8.Add(roomCfg.extraDoor + new Vector2Data(-1f, 1f));
				list8.Add(roomCfg.extraDoor + new Vector2Data(0f, 1f));
				list8.Add(roomCfg.extraDoor + new Vector2Data(1f, 1f));
				list8.Add(roomCfg.extraDoor + new Vector2Data(2f, 1f));
			}
			NativeArray<Entity> outputEntities3 = new NativeArray<Entity>(allBoundary1Position.Value.Length, Allocator.Temp);
			ettMgr.Instantiate(ett_Boundary, outputEntities3);
			for (int num35 = 0; num35 < allBoundary1Position.Value.Length; num35++)
			{
				BoundaryBase_Dots componentData14 = ettMgr.GetComponentData<BoundaryBase_Dots>(outputEntities3[num35]);
				componentData14.roomID = roomCfg.id;
				componentData14.shouldCreateDetail = ((!list8.Contains(allBoundary1Position.Value[num35])) ? true : false);
				componentData14.roomPosition = base.transform.position;
				componentData14.selfPosition = allBoundary1Position.Value[num35];
				componentData14.allBoundary1Position = allBoundary1Position;
				componentData14.allBoundary2Position = allBoundary2Position;
				componentData14.allTile0Position = allTile0Position;
				ettMgr.SetComponentData(outputEntities3[num35], componentData14);
				if (ettMgr.HasComponent<BoundaryT2RoomCtrller>(outputEntities3[num35]))
				{
					BoundaryT2RoomCtrller componentData15 = EntityManagerManagedComponentExtensions.GetComponentData<BoundaryT2RoomCtrller>(ettMgr, outputEntities3[num35]);
					componentData15.roomCtrller = this;
					componentData15.accessPositionL = roomCfg.accessLeft;
					componentData15.accessPositionR = roomCfg.accessRight;
				}
			}
			NativeArray<Entity> outputEntities4 = new NativeArray<Entity>(allBoundary2Position.Value.Length, Allocator.Temp);
			ettMgr.Instantiate(ett_Boundary2, outputEntities4);
			for (int num36 = 0; num36 < allBoundary2Position.Value.Length; num36++)
			{
				BoundaryBase_Dots componentData16 = ettMgr.GetComponentData<BoundaryBase_Dots>(outputEntities4[num36]);
				componentData16.roomID = roomCfg.id;
				componentData16.roomPosition = base.transform.position;
				componentData16.selfPosition = allBoundary2Position.Value[num36];
				if (roomCfg.type == RoomType.Boss && (componentData16.selfPosition.x < (float)(roomCfg.localMinX - 2) || componentData16.selfPosition.x > (float)(roomCfg.localMaxX + 2)))
				{
					componentData16.dontCreateIronChain = true;
				}
				ettMgr.SetComponentData(outputEntities4[num36], componentData16);
			}
			if (roomCfg.themeType == RoomThemeType.Theme1_Chapter2_Cliff || roomCfg.themeType == RoomThemeType.Theme5_Chapter1_Water || roomCfg.themeType == RoomThemeType.Theme13_Guide_Forest || roomCfg.themeType == RoomThemeType.Theme19_Chapter2_Shortcut2)
			{
				UnityEngine.Object.Destroy(go_EndlessBoundaryParent.gameObject);
			}
			else
			{
				int num37 = 1;
				for (int num38 = roomCfg.localMinX - num37; num38 <= roomCfg.localMaxX + num37; num38++)
				{
					for (int num39 = roomCfg.localMinY - num37; num39 <= roomCfg.localMaxY + num37; num39++)
					{
						if (!roomCfg.boundarys.Contains(new Vector2Data(num38, num39)) && !roomCfg.boundary2s.Contains(new Vector2Data(num38, num39)) && !roomCfg.allTileList[0].Contains(new Vector2Data(num38, num39)))
						{
							Entity entity6 = ettMgr.Instantiate(singleton.ett_OuterBoundary);
							LocalTransform componentData17 = ettMgr.GetComponentData<LocalTransform>(entity6);
							componentData17.Position = base.transform.position + new Vector2Data(num38, num39).GetVector3();
							ettMgr.SetComponentData(entity6, componentData17);
						}
					}
				}
				go_EndlessBoundaryParent.gameObject.SetActive(value: true);
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 0, 1))
				{
					tsf_EndlessBoundaryUL.position = GetAccessCenterPoint(FourDir.Up) + new Vector3(-1f, 1f, 0f);
					tsf_EndlessBoundaryUR.position = GetAccessCenterPoint(FourDir.Up) + new Vector3(1f, 1f, 0f);
				}
				else
				{
					tsf_EndlessBoundaryUL.position = CenterPoint + new Vector3(0f, RoomScale.y / 2f + (float)num37);
					tsf_EndlessBoundaryUR.position = tsf_EndlessBoundaryUL.position;
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, -1, 0))
				{
					tsf_EndlessBoundaryLU.position = GetAccessCenterPoint(FourDir.Left) + new Vector3(-1f, 1f, 0f);
					tsf_EndlessBoundaryLD.position = GetAccessCenterPoint(FourDir.Left) + new Vector3(-1f, -1f, 0f);
				}
				else
				{
					tsf_EndlessBoundaryLU.position = CenterPoint + new Vector3((0f - RoomScale.x) / 2f - (float)num37, 0f);
					tsf_EndlessBoundaryLD.position = tsf_EndlessBoundaryLU.position;
				}
				if (LevelMgr.Inst.HaveNeighbor(MapPos, 1, 0))
				{
					tsf_EndlessBoundaryRU.position = GetAccessCenterPoint(FourDir.Right) + new Vector3(1f, 1f, 0f);
					tsf_EndlessBoundaryRD.position = GetAccessCenterPoint(FourDir.Right) + new Vector3(1f, -1f, 0f);
				}
				else
				{
					tsf_EndlessBoundaryRU.position = CenterPoint + new Vector3(RoomScale.x / 2f + (float)num37, 0f);
					tsf_EndlessBoundaryRD.position = tsf_EndlessBoundaryRU.position;
				}
				tsf_EndlessBoundaryD.position = CenterPoint + new Vector3(0f, (0f - RoomScale.y) / 2f - (float)num37);
			}
		}
		if (roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme7_Chapter4_Store && roomCfg.themeType != RoomThemeType.Theme8_Chapter4 && roomCfg.themeType != RoomThemeType.Theme9_Chapter4_2 && roomCfg.themeType != RoomThemeType.Theme12_Chapter5_2 && roomCfg.themeType != RoomThemeType.Theme14_CustomRectangleEmptyScene && roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss && roomCfg.themeType != RoomThemeType.Theme21_Chapter5_Store && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1 && roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle)
		{
			combinerAction.Combine("ActionCollider").transform.SetParent(tsf_Action);
			combinerGround.Combine("GroundCollider").transform.SetParent(tsf_Ground);
			combinerFly.Combine("FlyCollider").transform.SetParent(tsf_Fly);
			for (int num40 = list5.Count - 1; num40 >= 0; num40--)
			{
				UnityEngine.Object.Destroy(list5[num40]);
				UnityEngine.Object.Destroy(list6[num40]);
				UnityEngine.Object.Destroy(list7[num40]);
			}
		}
		UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text + "/ThemeSpecialize", text2 + "/ThemeSpecialize"), base.transform.position, Quaternion.identity, base.transform).GetComponent<ThemeSpecialize>().Initialize(this);
		if (roomCfg.type != RoomType.Boss)
		{
			return;
		}
		string text3 = text + "/HideBoundary";
		string nameH = text2 + "/HideBoundary";
		if (LevelMgr.Inst.HaveNeighbor(MapPos, -1, 0))
		{
			HideBoundaryBase component2 = UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text3, nameH), GetAccessPoint(FourDir.Left) + new Vector3(-1f, 0f, 0f), Quaternion.identity, base.transform).GetComponent<HideBoundaryBase>();
			component2.Initialize(this, FourDir.Left);
			hideBoundarys.Add(component2);
		}
		if (LevelMgr.Inst.HaveNeighbor(MapPos, 1, 0))
		{
			HideBoundaryBase component3 = UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text3, nameH), GetAccessPoint(FourDir.Right) + new Vector3(1f, 0f, 0f), Quaternion.identity, base.transform).GetComponent<HideBoundaryBase>();
			component3.Initialize(this, FourDir.Right);
			hideBoundarys.Add(component3);
		}
		if (!(tile_T != null))
		{
			return;
		}
		if (hideBoundarys.Count == 0)
		{
			if ((bool)BattleMgr.Inst && BattleMgr.Inst.CurrentStage == 8)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadHarmonizableAsset<GameObject>(text3, nameH)).GetComponent<HideBoundary_Theme8>().InitializeT8(tile_T);
			}
		}
		else
		{
			((HideBoundary_Theme8)hideBoundarys[0]).InitializeT8(tile_T);
		}
	}

	public void RoomEnter()
	{
		enableEnterNextRoom = false;
		nms_Action.BuildNavMesh();
		nms_Ground.BuildNavMesh();
		nms_Fly.BuildNavMesh();
		if (go_MRParent != null)
		{
			go_MRParent.SetActive(value: true);
		}
		if (whenEnterRoomUpdateThemeMusic)
		{
			MusicMgr.Inst.UpdateThemeMusic();
		}
		CamController.Inst.ClearExtraCameraFocusRequirement();
		CamController.Inst.FocusRecover(0f);
		if (roomCfg.overrideThemeColor == Vector4Data.Zero)
		{
			LevelMgr.Inst.ChangeGlobalLightColor(roomCfg.themeType);
		}
		else
		{
			LevelMgr.Inst.ChangeGlobalLightColor(roomCfg.overrideThemeColor.GetColor());
		}
		if (DataMgr.selectedWorldData.isTriggerTutorialHpShow)
		{
			if (GameMgr.IsMobile_Static)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/LoadEnterScene/Entry/TutorialHpShowMobile"), UIMgr.Inst.rtsf_Canvas2, worldPositionStays: false);
			}
			else
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/LoadEnterScene/Entry/TutorialHpShowPC"), UIMgr.Inst.rtsf_Canvas2, worldPositionStays: false);
			}
		}
		if (!IsFinish && DataMgr.selectedWorldData.inBattle9 && !ScriptableObjMgr.Inst.testCtrller.SkipAllStoryMixed)
		{
			if (!DataMgr.selectedWorldData.storyMixedFirstEncounterElite && BattleMgr.Inst.CurrentStage == 1 && BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[0] - 1)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstEncounterElite);
			}
			if (!DataMgr.selectedWorldData.storyMixedFirstEnterChapter2 && BattleMgr.Inst.CurrentStage == 3 && BattleMgr.Inst.CurrentLevel == 0)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstArriveChapter2);
			}
			if (!DataMgr.selectedWorldData.storyMixedFirstEnterChapter3 && BattleMgr.Inst.CurrentStage == 5 && BattleMgr.Inst.CurrentLevel == 0)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstArriveChapter3);
			}
			if (!DataMgr.selectedWorldData.storyMixedFirstEnterChapter4 && BattleMgr.Inst.CurrentStage == 7 && BattleMgr.Inst.CurrentLevel == 0)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstArriveChapter4);
			}
			if (!DataMgr.selectedWorldData.storyMixedFirstEnterChapter5 && BattleMgr.Inst.CurrentStage == 9 && BattleMgr.Inst.CurrentLevel == 0)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstArriveChapter5);
			}
			if (!DataMgr.selectedWorldData.storyMixedFirstEnterBloodRoom && roomCfg.type == RoomType.BloodRelic)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstEnterBloodRoom, LevelMgr.Inst.CurrentRoomCtrller);
			}
		}
		if (BattleMgr.Inst != null && BattleMgr.Inst.CurrentLevel == 0)
		{
			if (BattleMgr.Inst.CurrentStage == 3)
			{
				DataMgr.selectedWorldData.isReachChatper2 = true;
			}
			else if (BattleMgr.Inst.CurrentStage == 5)
			{
				DataMgr.selectedWorldData.isReachChatper3 = true;
			}
			else if (BattleMgr.Inst.CurrentStage == 7)
			{
				DataMgr.selectedWorldData.isReachChatper4 = true;
			}
			else if (BattleMgr.Inst.CurrentStage == 9)
			{
				DataMgr.selectedWorldData.isReachChatper5 = true;
			}
		}
		if (IsFinish)
		{
			if (whenFinishOpenDoorAndAccess)
			{
				AllDoorOpenDirect();
				LevelMgr.Inst.AllRoomAllAccessOpenDirect();
			}
		}
		else
		{
			if (ScriptableObjMgr.Inst.testCtrller.CommandLine)
			{
				CommandLineMgr.Inst.PrintLog("关卡：" + roomCfg.name + "(" + roomCfg.id + ") 已开始");
				CommandLineMgr.Inst.SetDebugValue("gameTime", Time.time);
				RoomFinishRegister(delegate
				{
					string content = "关卡：" + roomCfg.name + "(" + roomCfg.id + ") 已结束，用时 " + (Time.time - (float)CommandLineMgr.Inst.GetDebugValue("gameTime"));
					CommandLineMgr.Inst.PrintLog(content);
				});
			}
			if (HaveWave)
			{
				LevelMgr.Inst.AllRoomAllAccessCloseDirect();
				CreateWave(immediatelyCreate: true);
				currentWave++;
				IndexOfHaveObjWave();
			}
			else if (!GameMgr.InEndlessMode || (roomCfg.type != RoomType.Monster && roomCfg.type != RoomType.Boss))
			{
				IsFinish = true;
				RoomFinishDelegateExecute(base.transform.position);
				if (whenFinishOpenDoorAndAccess)
				{
					AllDoorOpenDirect();
					LevelMgr.Inst.AllRoomAllAccessOpenDirect();
				}
			}
		}
		if (roomEnterDelegate != null)
		{
			roomEnterDelegate();
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(IRoomCtrller_Dots));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			IRoomCtrller_Dots componentData = ettMgr.GetComponentData<IRoomCtrller_Dots>(item);
			if (componentData.belongRoom.Value == this)
			{
				componentData.onRoomEnter = true;
				ettMgr.SetComponentData(item, componentData);
			}
		}
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			CamController.Inst.CorrectCamera();
		}
		if (roomCfg.id == 1005)
		{
			if (!DataMgr.selectedWorldData.battleData9.isMeetProducer)
			{
				DataMgr.selectedWorldData.battleData9.isMeetProducer = true;
				DataMgr.SaveSelectedWorldData();
			}
		}
		else if (roomCfg.id == 1011 && DataMgr.settingData.isTouristMode)
		{
			int wandFromPool = PlayerMgr.Inst.BaData.GetWandFromPool(200);
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Wand, wandFromPool), BattleMgr.Inst.guideFlagPoint);
		}
		if (DataMgr.selectedWorldData.battleData9.currentStage == 7)
		{
			DataMgr.selectedWorldData.SetFindSet4();
		}
	}

	public void MoveAllTeammates(RoomController lastroom)
	{
		List<Entity> list2 = new List<Entity>();
		List<Entity> list3 = new List<Entity>();
		foreach (Entity teammateEtt in lastroom.TeammateEttList)
		{
			TryAddList(teammateEtt, list2);
		}
		foreach (Entity teammateNotAttackEtt in lastroom.TeammateNotAttackEttList)
		{
			TryAddList(teammateNotAttackEtt, list3);
		}
		TeammateEttList = list2;
		TeammateNotAttackEttList = list3;
		ClearAllTeammates(lastroom);
		void TryAddList(Entity entity, List<Entity> list)
		{
			if (ettMgr.HasComponent<TeammateData>(entity) && ettMgr.HasComponent<TeammateDeadTag>(entity))
			{
				TeammateData componentData = ettMgr.GetComponentData<TeammateData>(entity);
				if (componentData.TeammateType == TeammateType.teammate2 || componentData.TeammateType == TeammateType.teammate6 || componentData.SummonFollowOwnerThroughMapChance > 0f)
				{
					list.Add(entity);
				}
				else
				{
					ettMgr.SetComponentEnabled<TeammateDeadTag>(entity, value: true);
				}
			}
		}
	}

	public void ClearAllTeammates(RoomController targetRoom)
	{
		targetRoom.TeammateEttList.Clear();
		targetRoom.TeammateNotAttackEttList.Clear();
	}

	public void RoomLeave()
	{
		for (int num = monsterEttList.Count - 1; num >= 0; num--)
		{
			Entity entity = monsterEttList[num];
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(entity);
			if (componentData.id / 100 == 1001)
			{
				componentData.AnnouncedDeath(entity);
				monsterEttList.Remove(entity);
				targetableEttList.Remove(entity);
				ettMgr.SetComponentData(entity, componentData);
			}
		}
		if (roomLeaveDelegate != null)
		{
			roomLeaveDelegate();
		}
		nms_Action.RemoveData();
		nms_Ground.RemoveData();
		nms_Fly.RemoveData();
		if (go_MRParent != null)
		{
			go_MRParent.SetActive(value: false);
		}
	}

	public void CamWaterRenderOnce()
	{
		if (rtcam_Water != null)
		{
			rtcam_Water.RenderOnce();
		}
	}

	public void CamMucusRenderOnce()
	{
		if (rtcam_Mucus != null)
		{
			rtcam_Mucus.RenderOnce();
		}
	}

	public void CamVenomRenderOnce()
	{
		if (rtcam_Venom != null)
		{
			rtcam_Venom.RenderOnce();
		}
	}

	public void UnitRegister(Entity ett)
	{
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(ett);
		switch (componentData.unitCfg.unitType)
		{
		case UnitType.Teammate:
			TeammateEttList.Add(ett);
			break;
		case UnitType.TeammateNotAttack:
			TeammateNotAttackEttList.Add(ett);
			break;
		case UnitType.Monster:
			targetableEttList.Add(ett);
			monsterEttList.Add(ett);
			break;
		case UnitType.Elite:
			targetableEttList.Add(ett);
			monsterEttList.Add(ett);
			hasBossFight = true;
			if (BattleMgr.Inst.CurrentStage == 9 || BattleMgr.Inst.CurrentStage == 10)
			{
				if (ScriptableObjMgr.Inst.testCtrller.BattleSkipBossShow)
				{
					MusicMgr.Inst.ForcePlayMusic(GameConstManaged.bgm_Boss);
				}
				else if (componentData.unitCfg.id != 301441)
				{
					GameUISingletonMono<UIBossShow>.ShowInit(ett);
				}
			}
			else if (!GameMgr.InEndlessMode)
			{
				MusicMgr.Inst.ForcePlayMusic(GameConstManaged.bgm_Boss);
			}
			if (!GameMgr.InEndlessMode)
			{
				GameUISingletonMono<UIBossHP>.ShowInit(ett);
			}
			break;
		case UnitType.Boss:
			targetableEttList.Add(ett);
			monsterEttList.Add(ett);
			hasBossFight = true;
			if (componentData.unitCfg.id != 509901 && componentData.unitCfg.id != 500901 && componentData.unitCfg.id != 501001 && componentData.unitCfg.id != 501301 && componentData.unitCfg.id != 501321)
			{
				GameUISingletonMono<UIBossHP>.ShowInit(ett);
			}
			if (ScriptableObjMgr.Inst.testCtrller.BattleSkipBossShow)
			{
				if (BattleMgr.Inst.CurrentStage == 10 && (componentData.unitCfg.id == 509901 || componentData.unitCfg.id == 500621))
				{
					MusicMgr.Inst.ForcePlayMusic("BGM_BossChapter5");
				}
				else
				{
					MusicMgr.Inst.ForcePlayMusic(GameConstManaged.bgm_Boss);
				}
			}
			else if (componentData.unitCfg.id != 509901 && componentData.unitCfg.id != 500901 && componentData.unitCfg.id != 501001 && componentData.unitCfg.id != 501301 && componentData.unitCfg.id != 501321)
			{
				GameUISingletonMono<UIBossShow>.ShowInit(ett);
			}
			break;
		case UnitType.WillAttack:
			targetableEttList.Add(ett);
			break;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (componentData.unitCfg.triggerDeadEvent)
			{
				noAttackTriggerDeadEttList.Add(ett);
			}
			break;
		default:
			Debug.LogError(componentData.unitCfg.unitType);
			break;
		case UnitType.Player:
			break;
		}
	}

	public void UnitUnregister(Entity ett)
	{
		if (!ettMgr.HasComponent<UnitProperty_Dots>(ett))
		{
			return;
		}
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(ett);
		switch (componentData.unitCfg.unitType)
		{
		case UnitType.Player:
			return;
		case UnitType.Teammate:
			TeammateEttList.Remove(ett);
			return;
		case UnitType.TeammateNotAttack:
			TeammateNotAttackEttList.Remove(ett);
			return;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
			monsterEttList.Remove(ett);
			targetableEttList.Remove(ett);
			break;
		case UnitType.WillAttack:
			targetableEttList.Remove(ett);
			return;
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			if (componentData.unitCfg.triggerDeadEvent)
			{
				noAttackTriggerDeadEttList.Remove(ett);
			}
			return;
		default:
			Debug.LogError(componentData.unitCfg.unitType);
			break;
		}
		if (!IsFinish && !GameMgr.InEndlessMode)
		{
			if (monsterEttList.Count + monsterBorns.Count == 0 && !HaveWave)
			{
				OnRoomFinish(ettMgr.GetComponentData<LocalTransform>(ett).Position);
			}
			else if (monsterEttList.Count + monsterBorns.Count <= roomCfg.leastCountToBorn && HaveWave)
			{
				CreateWave(immediatelyCreate: false);
				currentWave++;
				IndexOfHaveObjWave();
			}
		}
	}

	public void OnRoomFinish(Vector3 finishDropPoint)
	{
		if (IsFinish)
		{
			return;
		}
		IsFinish = true;
		if (hasBossFight)
		{
			MusicMgr.Inst.UpdateThemeMusic();
			if (!GameMgr.InEndlessMode)
			{
				SEMgr.Inst.bossFinish.PlaySE();
			}
		}
		Vector3 zero = Vector3.zero;
		float3 @float = Tool2D.IgnoreZPoint(finishDropPoint);
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			if (@float.x < CenterPoint.x - (float)(roomCfg.theme6Width / 2) + 3.1f)
			{
				@float.x = CenterPoint.x - (float)(roomCfg.theme6Width / 2) + 3.1f;
			}
			else if (@float.x > CenterPoint.x + (float)(roomCfg.theme6Width / 2) - 3.1f)
			{
				@float.x = CenterPoint.x + (float)(roomCfg.theme6Width / 2) - 3.1f;
			}
			if (@float.y < CenterPoint.y - (float)(roomCfg.theme6Height / 2) + 3.1f)
			{
				@float.y = CenterPoint.y - (float)(roomCfg.theme6Height / 2) + 3.1f;
			}
			else if (@float.y > CenterPoint.y + (float)(roomCfg.theme6Height / 2) - 3.1f)
			{
				@float.y = CenterPoint.y + (float)(roomCfg.theme6Height / 2) - 3.1f;
			}
		}
		if (!GameMgr.InEndlessMode)
		{
			switch (LevelMgr.Inst.CurrentRewardType)
			{
			case LevelRewardType.None:
				if (DataMgr.selectedWorldData.inBattle9)
				{
					zero = GetDoorToWalkablePoint(@float);
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr.GetChapter1Floor0(), zero);
					if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
					{
						zero = GetDoorToWalkablePoint(@float + (float3)UnityEngine.Random.insideUnitSphere.IgnoreZ());
						Entity ett3 = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.RuneWizardRune, OutputMgr_Dots.GetLevelReward(LevelRewardType.RuneWizardRune), zero);
						LevelRewardRegister(ett3);
					}
				}
				break;
			case LevelRewardType.Spell:
			case LevelRewardType.Relic:
			case LevelRewardType.MaxHP:
			case LevelRewardType.Coin:
				if (roomCfg.isFinalRoom)
				{
					zero = GetDoorToWalkablePoint(@float);
					Entity ett2 = QuickCreateSystem.Inst.CreateLevelReward(LevelMgr.Inst.CurrentRewardType, OutputMgr_Dots.GetLevelReward(LevelMgr.Inst.CurrentRewardType), zero);
					LevelRewardRegister(ett2);
				}
				else
				{
					Debug.LogError("!");
				}
				break;
			case LevelRewardType.Elite:
			{
				if (BattleMgr.Inst.CurrentStage == 9 || BattleMgr.Inst.CurrentStage == 10)
				{
					StartCoroutine(CreateChapter5EliteReward(@float));
					break;
				}
				zero = GetDoorToWalkablePoint(@float);
				LevelRewardType rewardType = ((PlayerMgr.Inst.BaData.currentStage != 1) ? LevelRewardType.Spell : LevelRewardType.Wand);
				Entity ett = QuickCreateSystem.Inst.CreateLevelReward(rewardType, OutputMgr_Dots.GetLevelReward(LevelMgr.Inst.CurrentRewardType), zero);
				LevelRewardRegister(ett);
				QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetEliteOrBossItemInfos(), Tool2D.GetNavMeshPointIngoreZ(@float), 2f);
				if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
				{
					ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.RuneWizardRune, OutputMgr_Dots.GetLevelReward(LevelRewardType.RuneWizardRune), GetDoorToWalkablePoint(zero + UnityEngine.Random.insideUnitSphere.IgnoreZ() * 2f));
					LevelRewardRegister(ett);
				}
				break;
			}
			case LevelRewardType.Boss:
				StartCoroutine(KillBoss(@float));
				break;
			case LevelRewardType.Shortcut:
				StartCoroutine(CraeteShortcutReward(@float));
				break;
			default:
				Debug.LogError(LevelMgr.Inst.CurrentRewardType);
				break;
			case LevelRewardType.Wand:
			case LevelRewardType.Store:
			case LevelRewardType.Process:
			case LevelRewardType.Spring:
				break;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_MedicineKit != null)
		{
			zero = GetDoorToWalkablePoint(@float);
			PlayerMgr.Inst.ItemCtrller.relic_MedicineKit.OnFinishBattle(zero, this);
		}
		if (whenFinishOpenDoorAndAccess)
		{
			LevelMgr.Inst.AllRoomAllAccessOpen();
			if (AllLevelRewardPicked)
			{
				AllDoorOpen();
			}
		}
		ClearBattleField(hasBossFight || GameMgr.InEndlessMode);
		RoomFinishDelegateExecute(@float);
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_DeathBet != null)
		{
			if (isPlayerDropBlood)
			{
				PlayerMgr.Inst.ChangeHPMax(-PlayerMgr.Inst.ItemCtrller.curseCfg_DeathBet.int1.result);
			}
			else
			{
				PlayerMgr.Inst.ChangeHPMax(PlayerMgr.Inst.ItemCtrller.curseCfg_DeathBet.int2.result);
				PlayerMgr.Inst.PlayerPpt.HPRecovery(PlayerMgr.Inst.ItemCtrller.curseCfg_DeathBet.int2.result, textFloat: false);
			}
		}
		if (!ScriptableObjMgr.Inst.testCtrller.SkipAllStoryMixed && DataMgr.selectedWorldData.inBattle9)
		{
			if (!DataMgr.selectedWorldData.storyMixedFirstFinishLevel)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.FirstFinishLevel, Tool2D.IgnoreZPoint(@float));
			}
			if (DataMgr.selectedWorldData.enterBattleTime > 1 && !DataMgr.selectedWorldData.storyMixedSecondEnterBattle && DataMgr.selectedWorldData.battleData9.currentLevel > 1)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/StoryMixed")).GetComponent<StoryMixed>().Initialize(StoryMixedType.SecondEnterBattle, Tool2D.IgnoreZPoint(@float));
			}
		}
		if (DataMgr.selectedWorldData.inBattle9 && DataMgr.selectedWorldData.battleData9 != null && roomCfg.type == RoomType.Boss && DataMgr.selectedWorldData.battleData9.currentStage == 4)
		{
			DataMgr.selectedWorldData.SetFindSet3();
		}
	}

	public void ClearBattleField(bool clearBullet)
	{
		venomCtrller.RecycleAllVenom();
		mucusCtrller.Clear();
		SetAllTrapInvalid();
		if (!clearBullet)
		{
			return;
		}
		EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SpellConfigComponentData));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		NativeArray<SpellConfigComponentData> nativeArray2 = entityQuery.ToComponentDataArray<SpellConfigComponentData>(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			UnitType shooterType = nativeArray2[i].ShooterType;
			if (shooterType == UnitType.Monster || shooterType == UnitType.Elite || shooterType == UnitType.Boss)
			{
				ettMgr.SetComponentEnabled<SpellDestroyTag>(nativeArray[i], value: true);
			}
		}
		nativeArray.Dispose();
		nativeArray2.Dispose();
		entityQuery.Dispose();
		UnitDotsSyncSystem.shootSpellParamList.Clear();
	}

	private IEnumerator CraeteShortcutReward(Vector3 unitPoint)
	{
		Entity ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Spell, OutputMgr_Dots.GetLevelReward(LevelRewardType.Spell), GetDoorToWalkablePoint(unitPoint + Tool2D.GetDir(45f) * 1f));
		LevelRewardRegister(ett);
		yield return null;
		ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Relic, OutputMgr_Dots.GetLevelReward(LevelRewardType.Relic), GetDoorToWalkablePoint(unitPoint + Tool2D.GetDir(-45f) * 1f));
		LevelRewardRegister(ett);
		yield return null;
		ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Coin, OutputMgr_Dots.GetLevelReward(LevelRewardType.Coin), GetDoorToWalkablePoint(unitPoint + Tool2D.GetDir(135f) * 1f));
		LevelRewardRegister(ett);
		yield return null;
		ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.MaxHP, OutputMgr_Dots.GetLevelReward(LevelRewardType.MaxHP), GetDoorToWalkablePoint(unitPoint + Tool2D.GetDir(225f) * 1f));
		LevelRewardRegister(ett);
	}

	private IEnumerator CreateChapter5EliteReward(Vector3 unitPoint)
	{
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetEliteOrBossItemInfos(), Tool2D.GetNavMeshPointIngoreZ(unitPoint), 2f);
		List<LevelRewardType> _types = new List<LevelRewardType>
		{
			LevelRewardType.Spell,
			LevelRewardType.Relic,
			LevelRewardType.Coin,
			LevelRewardType.MaxHP
		};
		_types.RemoveAt(UnityEngine.Random.Range(0, 4));
		_types.RemoveAt(UnityEngine.Random.Range(0, 3));
		_types.Upset();
		Entity ett = QuickCreateSystem.Inst.CreateLevelReward(_types[0], OutputMgr_Dots.GetLevelReward(_types[0]), GetDoorToWalkablePoint(unitPoint + new Vector3(-1f, 0f, 0f)));
		LevelRewardRegister(ett);
		yield return null;
		ett = QuickCreateSystem.Inst.CreateLevelReward(_types[1], OutputMgr_Dots.GetLevelReward(_types[1]), GetDoorToWalkablePoint(unitPoint + new Vector3(1f, 0f, 0f)));
		LevelRewardRegister(ett);
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
		{
			yield return null;
			Entity ett2 = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.RuneWizardRune, OutputMgr_Dots.GetLevelReward(LevelRewardType.RuneWizardRune), GetDoorToWalkablePoint(unitPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ() * 2f));
			LevelRewardRegister(ett2);
		}
	}

	private IEnumerator KillBoss(Vector3 _unitPoint)
	{
		Vector3 _dropCenterPoint = GetDoorToWalkablePoint(_unitPoint);
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetEliteOrBossItemInfos(), _dropCenterPoint, 3f);
		if (DataMgr.selectedWorldData.battleData9.currentStage == 6)
		{
			if (!DataMgr.selectedWorldData.storyKillChapter3BossPickup)
			{
				Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("BattleFinishDrop", _dropCenterPoint);
				BattleFinishDrop componentData = ettMgr.GetComponentData<BattleFinishDrop>(entity);
				componentData.type = DataMgr.selectedWorldData.selectedDifficulty;
				ettMgr.SetComponentData(entity, componentData);
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy)
			{
				QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint));
				UIBattleMgr.Inst.PopoutCurrentFinishBuild();
			}
			else
			{
				Entity ett = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Wand, OutputMgr_Dots.GetLevelReward(LevelMgr.Inst.CurrentRewardType), _dropCenterPoint);
				LevelRewardRegister(ett);
			}
		}
		else if (DataMgr.selectedWorldData.battleData9.currentStage == 8)
		{
			if (!DataMgr.selectedWorldData.storyHardBossDropPickup)
			{
				Entity entity2 = QuickCreateSystem.Inst.CreateMixedEtt("BattleFinishDrop", _dropCenterPoint);
				BattleFinishDrop componentData2 = ettMgr.GetComponentData<BattleFinishDrop>(entity2);
				componentData2.type = DataMgr.selectedWorldData.selectedDifficulty;
				ettMgr.SetComponentData(entity2, componentData2);
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Normal)
			{
				QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint));
				UIBattleMgr.Inst.PopoutCurrentFinishBuild();
			}
			else
			{
				Entity ett2 = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Wand, OutputMgr_Dots.GetLevelReward(LevelMgr.Inst.CurrentRewardType), _dropCenterPoint);
				LevelRewardRegister(ett2);
			}
		}
		else if (DataMgr.selectedWorldData.battleData9.currentStage == 10)
		{
			if (!DataMgr.selectedWorldData.storyFinishHardDropPickup)
			{
				Entity entity3 = QuickCreateSystem.Inst.CreateMixedEtt("BattleFinishDrop", _dropCenterPoint);
				BattleFinishDrop componentData3 = ettMgr.GetComponentData<BattleFinishDrop>(entity3);
				componentData3.type = DataMgr.selectedWorldData.selectedDifficulty;
				ettMgr.SetComponentData(entity3, componentData3);
			}
			else
			{
				QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint));
				UIBattleMgr.Inst.PopoutCurrentFinishBuild();
				switch (DataMgr.selectedWorldData.selectedDifficulty)
				{
				case DifficultyType.Nightmare1:
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare1);
					break;
				case DifficultyType.Nightmare2:
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare2);
					break;
				case DifficultyType.Nightmare3:
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare3);
					break;
				}
			}
		}
		else
		{
			Entity ett3 = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.Wand, OutputMgr_Dots.GetLevelReward(LevelMgr.Inst.CurrentRewardType), _dropCenterPoint);
			LevelRewardRegister(ett3);
		}
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
		{
			yield return null;
			Entity ett4 = QuickCreateSystem.Inst.CreateLevelReward(LevelRewardType.RuneWizardRune, OutputMgr_Dots.GetLevelReward(LevelRewardType.RuneWizardRune), GetDoorToWalkablePoint(_dropCenterPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ() * 2f));
			LevelRewardRegister(ett4);
		}
		yield return new WaitForSeconds(0.1f);
		if (!isPlayerDropBlood || PlayerMgr.Inst.ItemCtrller.relic_FollowObj_BloodKey != null)
		{
			HideBoundaryDisappear();
		}
		else if (DataMgr.selectedWorldData.battleData9.currentStage == 10 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
		{
			HideBoundaryDisappear();
		}
	}

	public void MonsterBornRegister(MonsterBorn born)
	{
		monsterBorns.Add(born);
	}

	public void MonsterBornUnRegister(MonsterBorn born)
	{
		monsterBorns.Remove(born);
	}

	public void AbyssRegister(GameObject abyssGO)
	{
		abysses.Add(abyssGO);
	}

	public void LevelRewardRegister(Entity ett)
	{
		levelRewardEttList.Add(ett);
	}

	public void LevelRewardUnregister(Entity ett)
	{
		levelRewardEttList.Remove(ett);
		if (levelRewardEttList.Count == 0)
		{
			AllDoorOpen();
		}
	}

	public void TrapRegister(ITrap iTrap)
	{
		traps.Add(iTrap);
	}

	public void SetAllTrapInvalid()
	{
		for (int i = 0; i < traps.Count; i++)
		{
			traps[i].SetTrapInvalid();
		}
		for (int j = 0; j < trapEttList.Count; j++)
		{
			ITrap_Dots componentData = ettMgr.GetComponentData<ITrap_Dots>(trapEttList[j]);
			componentData.onInvalid = true;
			ettMgr.SetComponentData(trapEttList[j], componentData);
		}
	}

	public void RoomRecyeleDelegateExecute()
	{
		if (roomRecycleDelegate != null)
		{
			roomRecycleDelegate();
		}
	}

	public void RoomRecycleRegister(Action method)
	{
		roomRecycleDelegate = (Action)Delegate.Combine(roomRecycleDelegate, method);
	}

	public void RoomFinishRegister(Action<Vector3> method)
	{
		roomFinishDelegate = (Action<Vector3>)Delegate.Combine(roomFinishDelegate, method);
	}

	private void RoomFinishDelegateExecute(Vector3 pos)
	{
		if (roomFinishDelegate != null)
		{
			roomFinishDelegate(pos);
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(IRoomCtrller_Dots));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			IRoomCtrller_Dots componentData = ettMgr.GetComponentData<IRoomCtrller_Dots>(item);
			if (componentData.belongRoom.Value == this)
			{
				componentData.onRoomFinish = true;
				componentData.roomFinishPos = pos;
				ettMgr.SetComponentData(item, componentData);
			}
		}
	}

	public void RoomEnterRegister(Action method)
	{
		roomEnterDelegate = (Action)Delegate.Combine(roomEnterDelegate, method);
	}

	public void RoomLeaveRegister(Action method)
	{
		roomLeaveDelegate = (Action)Delegate.Combine(roomLeaveDelegate, method);
	}

	public void MaskNoFinish()
	{
		IsFinish = false;
	}

	public void MaskFinish()
	{
		IsFinish = true;
	}

	public void SetWhenFinishOpenDoorAndAccess(bool isOpen)
	{
		whenFinishOpenDoorAndAccess = isOpen;
	}

	public void HideBoundaryDisappear()
	{
		foreach (HideBoundaryBase hideBoundary in hideBoundarys)
		{
			hideBoundary.Disappear();
		}
		if (LevelMgr.Inst.RoomFinishLogger != null)
		{
			RoomFinishLogger.SideRoomInfo sideRoomInfo = LevelMgr.Inst.RoomFinishLogger.side_room.FirstOrDefault((RoomFinishLogger.SideRoomInfo e) => e.type == RoomType.BloodRelic);
			if (sideRoomInfo != null)
			{
				sideRoomInfo.unlocked = true;
			}
		}
	}

	public void AllAccessOpen()
	{
		if (roomCfg.type == RoomType.Boss && isPlayerDropBlood && PlayerMgr.Inst.ItemCtrller.relic_FollowObj_BloodKey == null && BattleMgr.Inst.CurrentStage != 10)
		{
			return;
		}
		for (int i = 0; i < accessEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(accessEttList[i]))
			{
				AccessBase_Dots componentData = ettMgr.GetComponentData<AccessBase_Dots>(accessEttList[i]);
				componentData.onOpen = true;
				ettMgr.SetComponentData(accessEttList[i], componentData);
			}
		}
	}

	public void AllAccessOpenDirect()
	{
		if (roomCfg.type == RoomType.Boss && isPlayerDropBlood && PlayerMgr.Inst.ItemCtrller.relic_FollowObj_BloodKey == null)
		{
			return;
		}
		for (int i = 0; i < accessEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(accessEttList[i]))
			{
				AccessBase_Dots componentData = ettMgr.GetComponentData<AccessBase_Dots>(accessEttList[i]);
				componentData.onOpenDirect = true;
				ettMgr.SetComponentData(accessEttList[i], componentData);
			}
		}
	}

	public void AllAccessClose()
	{
		for (int i = 0; i < accessEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(accessEttList[i]))
			{
				AccessBase_Dots componentData = ettMgr.GetComponentData<AccessBase_Dots>(accessEttList[i]);
				componentData.onClose = true;
				ettMgr.SetComponentData(accessEttList[i], componentData);
			}
		}
	}

	public void AllAccessCloseDirect()
	{
		for (int i = 0; i < accessEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(accessEttList[i]))
			{
				AccessBase_Dots componentData = ettMgr.GetComponentData<AccessBase_Dots>(accessEttList[i]);
				componentData.onCloseDirect = true;
				ettMgr.SetComponentData(accessEttList[i], componentData);
			}
		}
	}

	public void AllDoorOpen()
	{
		for (int i = 0; i < doorEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(doorEttList[i]))
			{
				DoorBase_Dots componentData = ettMgr.GetComponentData<DoorBase_Dots>(doorEttList[i]);
				componentData.onOpen = true;
				ettMgr.SetComponentData(doorEttList[i], componentData);
			}
		}
	}

	public void AllDoorOpenDirect()
	{
		for (int i = 0; i < doorEttList.Count; i++)
		{
			if (ettMgr.HasComponent<LocalTransform>(doorEttList[i]))
			{
				DoorBase_Dots componentData = ettMgr.GetComponentData<DoorBase_Dots>(doorEttList[i]);
				componentData.onOpenDirect = true;
				ettMgr.SetComponentData(doorEttList[i], componentData);
			}
		}
	}

	public UnitProperty GetNearestTargetablePpt(Vector3 checkPoint, bool checkWall = false)
	{
		if (TargetablePpts.Count == 0)
		{
			return null;
		}
		UnitProperty result = null;
		float num = 100000000f;
		for (int i = 0; i < TargetablePpts.Count; i++)
		{
			if (!TargetablePpts[i].CanBeTarget)
			{
				continue;
			}
			float num2 = Tool2D.IgnoreZDistanceSqr(checkPoint, TargetablePpts[i].transform.position);
			if (!(num2 < num))
			{
				continue;
			}
			if (checkWall)
			{
				UnityEngine.Ray ray = new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(TargetablePpts[i].transform.position, checkPoint));
				if (Physics.Raycast(ray, out var hitInfo, 100000000f, LayerMask.GetMask("Wall")))
				{
					if (num2 < (ray.origin - hitInfo.point).sqrMagnitude)
					{
						result = TargetablePpts[i];
						num = num2;
					}
				}
				else
				{
					result = TargetablePpts[i];
					num = num2;
				}
			}
			else
			{
				result = TargetablePpts[i];
				num = num2;
			}
		}
		return result;
	}

	public IEnumerable<Entity> GetTargetableInCircle_Dots(Vector3 center, float radius, bool checkWall = false)
	{
		if (targetableEttList.Count == 0)
		{
			yield break;
		}
		foreach (Entity targetableEtt in targetableEttList)
		{
			if (ettMgr.GetComponentData<UnitProperty_Dots>(targetableEtt).CanBeTarget)
			{
				LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(targetableEtt);
				if (!(Tool2D.IgnoreZDistanceSqr(center, componentData.Position) > radius * radius) && (!checkWall || !UnitDotsSyncSystem.Raycast(new UnityEngine.Ray(center, Tool2D.IgnoreZV2ToV1Normal(componentData.Position, center)), radius, GameConst.Filter_Wall)))
				{
					yield return targetableEtt;
				}
			}
		}
	}

	public IEnumerable<UnitProperty> GetTargetableInCircle(Vector3 center, float radius, bool checkWall = false)
	{
		if (TargetablePpts.Count == 0)
		{
			yield break;
		}
		foreach (UnitProperty targetablePpt in TargetablePpts)
		{
			if (targetablePpt.CanBeTarget && !(Tool2D.IgnoreZDistanceSqr(center, targetablePpt.transform.position) > radius * radius) && (!checkWall || !Physics.Raycast(new UnityEngine.Ray(center, Tool2D.IgnoreZV2ToV1Normal(targetablePpt.transform.position, center)), out var _, radius, LayerMask.GetMask("Wall"))))
			{
				yield return targetablePpt;
			}
		}
	}

	[CanBeNull]
	public UnitProperty GetNearestTargetablePpt(Vector3 center, float radius, GameObject[] ignoreGos, bool checkWall = false)
	{
		if (TargetablePpts.Count == 0)
		{
			return null;
		}
		if (ignoreGos == null)
		{
			ignoreGos = Array.Empty<GameObject>();
		}
		float num = float.MaxValue;
		UnitProperty result = null;
		foreach (UnitProperty item in TargetablePpts.Where((UnitProperty e) => e.CanBeTarget))
		{
			float num2 = Tool2D.IgnoreZDistanceSqr(center, item.transform.position);
			if (!(num2 > radius * radius) && !(num2 > num) && !ignoreGos.Contains(item.gameObject) && (!checkWall || !Physics.Raycast(new UnityEngine.Ray(center, Tool2D.IgnoreZV2ToV1Normal(item.transform.position, center)), out var _, radius, LayerMask.GetMask("Wall"))))
			{
				num = num2;
				result = item;
			}
		}
		return result;
	}

	[CanBeNull]
	public UnitProperty GetRandomTargetablePpt()
	{
		if (TargetablePpts.Count == 0)
		{
			return null;
		}
		List<UnitProperty> list = new List<UnitProperty>();
		for (int i = 0; i < TargetablePpts.Count; i++)
		{
			if (TargetablePpts[i].CanBeTarget)
			{
				list.Add(TargetablePpts[i]);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public Entity GetNearestTargetableEntity(Vector3 checkPoint, bool checkWall = false)
	{
		if (targetableEttList.Count == 0)
		{
			return Entity.Null;
		}
		Entity result = Entity.Null;
		float num = 100000000f;
		for (int i = 0; i < targetableEttList.Count; i++)
		{
			Entity entity = targetableEttList[i];
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
			if (!ettMgr.GetComponentData<UnitProperty_Dots>(entity).CanBeTarget)
			{
				continue;
			}
			float num2 = Tool2D.IgnoreZDistanceSqr(checkPoint, componentData.Position);
			if (!(num2 < num))
			{
				continue;
			}
			if (checkWall)
			{
				Vector3 direction = Tool2D.IgnoreZV2ToV1Normal(componentData.Position, checkPoint);
				if (UnitDotsSyncSystem.Raycast(checkPoint, direction, 999f, CheckWallFilter, out var result2))
				{
					if ((result2.point - checkPoint).sqrMagnitude > num2)
					{
						result = entity;
						num = num2;
					}
				}
				else
				{
					result = entity;
					num = num2;
				}
			}
			else
			{
				result = entity;
				num = num2;
			}
		}
		return result;
	}

	public Entity GetNearestFriendlyEntity(Vector3 checkPoint, bool checkWall = false)
	{
		Entity result = Entity.Null;
		float num = 100000000f;
		if (PlayerMgr.Inst.PlayerCtrller.IsVisible && ettMgr.HasComponent<LocalTransform>(PlayerMgr.Inst.PlayerEtt) && ettMgr.HasComponent<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt))
		{
			Entity playerEtt = PlayerMgr.Inst.PlayerEtt;
			Vector3 vector = ettMgr.GetComponentData<LocalTransform>(PlayerMgr.Inst.PlayerEtt).Position;
			if (ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt).CanBeTarget)
			{
				float num2 = Tool2D.IgnoreZDistanceSqr(checkPoint, vector);
				if (checkWall)
				{
					Vector3 direction = Tool2D.IgnoreZV2ToV1Normal(vector, checkPoint);
					if (UnitDotsSyncSystem.Raycast(checkPoint, direction, 999f, CheckWallFilter, out var result2))
					{
						if ((result2.point - checkPoint).sqrMagnitude > num2)
						{
							result = playerEtt;
							num = num2;
						}
					}
					else
					{
						result = playerEtt;
						num = num2;
					}
				}
				else
				{
					result = playerEtt;
					num = Tool2D.IgnoreZDistanceSqr(vector, checkPoint);
				}
			}
		}
		if (TeammateEttList.Count == 0)
		{
			return result;
		}
		for (int num3 = TeammateEttList.Count - 1; num3 >= 0; num3--)
		{
			Entity entity = TeammateEttList[num3];
			if (!ettMgr.HasComponent<LocalTransform>(entity) || !ettMgr.HasComponent<UnitProperty_Dots>(entity))
			{
				TeammateEttList.RemoveAt(num3);
			}
			else
			{
				LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
				if (ettMgr.GetComponentData<UnitProperty_Dots>(entity).CanBeTarget)
				{
					float num4 = Tool2D.IgnoreZDistanceSqr(checkPoint, componentData.Position);
					if (num4 < num)
					{
						if (checkWall)
						{
							Vector3 direction2 = Tool2D.IgnoreZV2ToV1Normal(componentData.Position, checkPoint);
							if (UnitDotsSyncSystem.Raycast(checkPoint, direction2, 999f, CheckWallFilter, out var result3))
							{
								if ((result3.point - checkPoint).sqrMagnitude > num4)
								{
									result = entity;
									num = num4;
								}
							}
							else
							{
								result = entity;
								num = num4;
							}
						}
						else
						{
							result = entity;
							num = num4;
						}
					}
				}
			}
		}
		return result;
	}

	public Entity GetMinimalAngleTargetableEntity(Vector3 checkPoint, Vector3 fromDir, bool checkWall = false)
	{
		if (TargetablePpts.Count == 0)
		{
			return Entity.Null;
		}
		Entity result = Entity.Null;
		float f = 100000000f;
		for (int i = 0; i < targetableEttList.Count; i++)
		{
			Entity entity = targetableEttList[i];
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
			if (!ettMgr.GetComponentData<UnitProperty_Dots>(entity).CanBeTarget)
			{
				continue;
			}
			float num = Tool2D.IgnoreZAngle(fromDir, (Vector3)componentData.Position - checkPoint);
			if (!(Mathf.Abs(num) < Mathf.Abs(f)))
			{
				continue;
			}
			if (checkWall)
			{
				if (!Physics.Raycast(new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(componentData.Position, checkPoint)), num, LayerMask.GetMask("Wall")))
				{
					result = entity;
					f = num;
				}
			}
			else
			{
				result = entity;
				f = num;
			}
		}
		return result;
	}

	public UnitProperty GetMinimalAngleTargetablePpt(Vector3 checkPoint, Vector3 fromDir, bool checkWall = false)
	{
		if (TargetablePpts.Count == 0)
		{
			return null;
		}
		UnitProperty result = null;
		float f = 100000000f;
		for (int i = 0; i < TargetablePpts.Count; i++)
		{
			if (!TargetablePpts[i].CanBeTarget)
			{
				continue;
			}
			float num = Tool2D.IgnoreZAngle(fromDir, TargetablePpts[i].transform.position - checkPoint);
			if (!(Mathf.Abs(num) < Mathf.Abs(f)))
			{
				continue;
			}
			if (checkWall)
			{
				if (!Physics.Raycast(new UnityEngine.Ray(checkPoint, Tool2D.IgnoreZV2ToV1Normal(TargetablePpts[i].transform.position, checkPoint)), num, LayerMask.GetMask("Wall")))
				{
					result = TargetablePpts[i];
					f = num;
				}
			}
			else
			{
				result = TargetablePpts[i];
				f = num;
			}
		}
		return result;
	}

	public bool CurrentRoomHasValidTarget()
	{
		return TargetablePpts.Count > 0;
	}

	public Vector3 GetAccessPoint(FourDir dir)
	{
		if (roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || roomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || roomCfg.themeType == RoomThemeType.Theme8_Chapter4 || roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || roomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || roomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || roomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1 || roomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			return GetAccessCenterPoint(dir);
		}
		Vector3 result = Vector3.zero;
		switch (dir)
		{
		case FourDir.Up:
			if (roomCfg.accessUp == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 RoomID:" + roomCfg.id);
			}
			else
			{
				result = base.transform.position + roomCfg.accessUp.GetVector3();
			}
			break;
		case FourDir.Right:
			if (roomCfg.accessRight == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 RoomID:" + roomCfg.id);
			}
			else
			{
				result = base.transform.position + roomCfg.accessRight.GetVector3();
			}
			break;
		case FourDir.Down:
			if (roomCfg.accessDown == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 RoomID:" + roomCfg.id);
			}
			else
			{
				result = base.transform.position + roomCfg.accessDown.GetVector3();
			}
			break;
		case FourDir.Left:
			if (roomCfg.accessLeft == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 RoomID:" + roomCfg.id);
			}
			else
			{
				result = base.transform.position + roomCfg.accessLeft.GetVector3();
			}
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		return result;
	}

	public Vector3 GetAccessCenterPoint(FourDir dir)
	{
		Vector3 result = Vector3.zero;
		switch (dir)
		{
		case FourDir.Up:
			if (roomCfg.accessUp == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 roomConfigID:" + roomCfg.id);
			}
			else
			{
				result = ((roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? ((roomCfg.themeType != RoomThemeType.Theme7_Chapter4_Store && roomCfg.themeType != RoomThemeType.Theme8_Chapter4 && roomCfg.themeType != RoomThemeType.Theme9_Chapter4_2 && roomCfg.themeType != RoomThemeType.Theme12_Chapter5_2 && roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss && roomCfg.themeType != RoomThemeType.Theme21_Chapter5_Store && roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle) ? (base.transform.position + roomCfg.accessUp.GetVector3() + new Vector3(0.5f, 0f)) : (base.transform.position + new Vector3(0f, (float)roomCfg.theme8Height / 2f))) : (base.transform.position + roomCfg.accessUp.GetVector3()));
			}
			break;
		case FourDir.Right:
			if (roomCfg.accessRight == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 roomConfigID:" + roomCfg.id);
			}
			else
			{
				result = ((roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1 && roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle) ? ((roomCfg.themeType != RoomThemeType.Theme7_Chapter4_Store && roomCfg.themeType != RoomThemeType.Theme8_Chapter4 && roomCfg.themeType != RoomThemeType.Theme9_Chapter4_2 && roomCfg.themeType != RoomThemeType.Theme12_Chapter5_2 && roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss && roomCfg.themeType != RoomThemeType.Theme21_Chapter5_Store) ? (base.transform.position + roomCfg.accessRight.GetVector3() + new Vector3(0f, 0.5f)) : (base.transform.position + new Vector3((float)roomCfg.theme8Width / 2f, 0f))) : (base.transform.position + roomCfg.accessRight.GetVector3()));
			}
			break;
		case FourDir.Down:
			if (roomCfg.accessDown == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 roomConfigID:" + roomCfg.id);
			}
			else
			{
				result = ((roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? ((roomCfg.themeType != RoomThemeType.Theme7_Chapter4_Store && roomCfg.themeType != RoomThemeType.Theme8_Chapter4 && roomCfg.themeType != RoomThemeType.Theme9_Chapter4_2 && roomCfg.themeType != RoomThemeType.Theme12_Chapter5_2 && roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss && roomCfg.themeType != RoomThemeType.Theme21_Chapter5_Store && roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle) ? (base.transform.position + roomCfg.accessDown.GetVector3() + new Vector3(0.5f, 0f)) : (base.transform.position + roomCfg.accessDown.GetVector3())) : (base.transform.position + roomCfg.accessDown.GetVector3()));
			}
			break;
		case FourDir.Left:
			if (roomCfg.accessLeft == Vector2Data.Up1000)
			{
				Debug.LogError(dir.ToString() + "方向的通道不存在 roomConfigID:" + roomCfg.id);
			}
			else
			{
				result = ((roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1 && roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle) ? ((roomCfg.themeType != RoomThemeType.Theme7_Chapter4_Store && roomCfg.themeType != RoomThemeType.Theme8_Chapter4 && roomCfg.themeType != RoomThemeType.Theme9_Chapter4_2 && roomCfg.themeType != RoomThemeType.Theme12_Chapter5_2 && roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss && roomCfg.themeType != RoomThemeType.Theme21_Chapter5_Store) ? (base.transform.position + roomCfg.accessLeft.GetVector3() + new Vector3(0f, 0.5f)) : (base.transform.position + new Vector3((float)(-roomCfg.theme8Width) / 2f, 0f))) : (base.transform.position + roomCfg.accessLeft.GetVector3()));
			}
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		return result;
	}

	public Vector3 GetDoorToWalkablePoint(Vector3 point)
	{
		NavMeshPath navMeshPath = null;
		if (roomCfg.accessDown != Vector2Data.Up1000)
		{
			navMeshPath = Tool2D.GetNavMeshPath(GetAccessCenterPoint(FourDir.Down), point);
		}
		else if (roomCfg.accessUp != Vector2Data.Up1000)
		{
			navMeshPath = Tool2D.GetNavMeshPath(GetAccessCenterPoint(FourDir.Up), point);
		}
		else if (roomCfg.accessLeft != Vector2Data.Up1000)
		{
			navMeshPath = Tool2D.GetNavMeshPath(GetAccessCenterPoint(FourDir.Left), point);
		}
		else if (roomCfg.accessRight != Vector2Data.Up1000)
		{
			navMeshPath = Tool2D.GetNavMeshPath(GetAccessCenterPoint(FourDir.Right), point);
		}
		else
		{
			Debug.LogError("!");
		}
		return Tool2D.IgnoreZPoint(navMeshPath.corners[navMeshPath.corners.Length - 1]);
	}

	public void PlayerDropBlood()
	{
		if (!isPlayerDropBlood)
		{
			isPlayerDropBlood = true;
		}
	}

	public void SetEnterRoomIsUpdateThemeMusic(bool whenEnterRoomUpdateThemeMusic)
	{
		this.whenEnterRoomUpdateThemeMusic = whenEnterRoomUpdateThemeMusic;
	}

	public void KillAllMonster()
	{
		for (int i = 0; i < monsterEttList.Count; i++)
		{
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(monsterEttList[i]);
			componentData.AnnouncedDeath(monsterEttList[i]);
			ettMgr.SetComponentData(monsterEttList[i], componentData);
		}
	}

	public void KillAllMonster2()
	{
		for (int i = 0; i < monsterEttList.Count; i++)
		{
			DynamicBuffer<TakeDamageInfo_Dots> buffer = ettMgr.GetBuffer<TakeDamageInfo_Dots>(monsterEttList[i]);
			TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			elem.damage = Mathf.Pow(10f, 37f);
			buffer.Add(elem);
		}
	}
}
