using System;
using System.Collections;
using System.Collections.Generic;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Entities;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
	public GameObject pfb_RoomController;

	public Transform tsf_PlayerThings;

	public Vector3 firstRoomPoint;

	public int roomAccessExtraDistance;

	[Header("ThemeEnv")]
	public Color[] themeLightColors;

	public Light globalLight;

	public float changeLightTime;

	public RoomFinishLogger RoomFinishLogger;

	private bool playerAccessLocked;

	private bool isGlobalLightChanging;

	private Color originalGlobalLightColor;

	private Color targetGlobalLightColor;

	private float changeColorTimer;

	private EntityManager ettMgr;

	private Entity _crystalDoorPassBuffer;

	public static LevelMgr Inst { get; private set; }

	public Transform CurrentRoomT => CurrentRoomCtrller.transform;

	public RoomConfig CurrentRoomCfg => RoomCfgs[CurrentRoomMapPos];

	public RoomController CurrentRoomCtrller
	{
		get
		{
			if (!RoomCtrllers.ContainsKey(CurrentRoomMapPos))
			{
				return null;
			}
			return RoomCtrllers[CurrentRoomMapPos];
		}
	}

	public Vector2Int CurrentRoomMapPos { get; set; } = Vector2Int.zero;


	public LevelRewardType CurrentRewardType { get; set; }

	public List<LevelRewardType> NextRewardTypes { get; set; }

	public LevelRewardType NextExtraDoorRewardType { get; set; }

	public Dictionary<Vector2Int, RoomConfig> RoomCfgs { get; private set; } = new Dictionary<Vector2Int, RoomConfig>();


	public Dictionary<Vector2Int, RoomController> RoomCtrllers { get; private set; } = new Dictionary<Vector2Int, RoomController>();


	public float BattleStartTime { get; set; }

	private void Awake()
	{
		Inst = this;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		if (isGlobalLightChanging)
		{
			changeColorTimer += Time.deltaTime;
			float num = Mathf.Clamp01(changeColorTimer / changeLightTime);
			globalLight.color = Color.Lerp(originalGlobalLightColor, targetGlobalLightColor, num);
			if (num == 1f)
			{
				isGlobalLightChanging = false;
			}
		}
	}

	public void CreateLevel(Dictionary<Vector2Int, RoomConfig> roomCfgs, LevelRewardType currentRewardType, List<LevelRewardType> nextRewardTypes, LevelRewardType nextExtraDoorRewardType, bool fadeDisappear, Action createLevelFinishAct = null)
	{
		StartCoroutine(CreateLevelIE(roomCfgs, currentRewardType, nextRewardTypes, nextExtraDoorRewardType, fadeDisappear, createLevelFinishAct));
	}

	public void CreateLevel(Dictionary<Vector2Int, RoomConfig> roomCfgs, LevelRewardType currentRewardType, LevelRewardType nextRewardType, LevelRewardType nextExtraDoorRewardType, bool fadeDisappear, Action createLevelFinishAct = null)
	{
		StartCoroutine(CreateLevelIE(roomCfgs, currentRewardType, new List<LevelRewardType> { nextRewardType }, nextExtraDoorRewardType, fadeDisappear, createLevelFinishAct));
	}

	private IEnumerator CreateLevelIE(Dictionary<Vector2Int, RoomConfig> roomCfgs, LevelRewardType currentRewardType, List<LevelRewardType> nextRewardTypes, LevelRewardType nextExtraDoorRewardType, bool fadeDisappear, Action createLevelFinishAct)
	{
		RoomCfgs = roomCfgs;
		CurrentRewardType = currentRewardType;
		NextRewardTypes = nextRewardTypes;
		NextExtraDoorRewardType = nextExtraDoorRewardType;
		CurrentRoomMapPos = Vector2Int.zero;
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			UnityEngine.Object.Destroy(roomCtrller.Value.gameObject);
		}
		RoomCtrllers.Clear();
		BattleStartTime = DataMgr.selectedWorldData.timeuse;
		foreach (KeyValuePair<Vector2Int, RoomConfig> roomCfg in RoomCfgs)
		{
			RoomController component = UnityEngine.Object.Instantiate(pfb_RoomController).GetComponent<RoomController>();
			RoomCtrllers.Add(roomCfg.Key, component);
			component.Initialize(roomCfg.Key, roomCfg.Value);
			Debug.Log("加载关卡ID：" + roomCfg.Value.id);
			float num = ((GuideMgr.Inst != null) ? 40f : 60f);
			component.transform.position = firstRoomPoint + new Vector3((float)roomCfg.Key.x * num, (float)roomCfg.Key.y * num, 0f);
			component.Initialize2();
		}
		yield return null;
		playerAccessLocked = true;
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller2 in RoomCtrllers)
		{
			roomCtrller2.Value.Generate();
		}
		yield return null;
		yield return null;
		CurrentRoomCtrller.RoomEnter();
		CurrentRoomCtrller.fogCtrller.HideDirect();
		PlayerMgr.Inst.SetPlayerPoint(CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Down));
		PlayerMgr.Inst.SummonsThrough();
		PlayerMgr.Inst.ItemCtrller.ItemPointerToPlayer();
		CamController.Inst.CorrectCamera();
		playerAccessLocked = false;
		yield return null;
		if (fadeDisappear)
		{
			UIMgr.Inst.uiFade.Hide();
		}
		createLevelFinishAct?.Invoke();
		if (BattleMgr.Inst != null)
		{
			RoomFinishLogger = new RoomFinishLogger
			{
				cursed_chest = new RoomFinishLogger.CursedChestInfo(),
				locked_chest = new RoomFinishLogger.LockedChestInfo(),
				entry_resources = ResourcesStatus.CreateAuto(),
				entry_equips = PlayerEquips.CreateAuto(),
				side_room = new List<RoomFinishLogger.SideRoomInfo>(),
				rewards = new List<RoomFinishLogger.Reward>(),
				next_room_options = new List<LevelRewardType>()
			};
			for (int i = 0; i < NextRewardTypes.Count; i++)
			{
				RoomFinishLogger.next_room_options.Add(NextRewardTypes[i]);
			}
			foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller3 in RoomCtrllers)
			{
				roomCtrller3.Deconstruct(out var key, out var value);
				Vector2Int vector2Int = key;
				RoomController roomController = value;
				if (!roomController.roomCfg.isFinalRoom)
				{
					FourDir dir = FourDir.Left;
					if (vector2Int == Vector2Int.down)
					{
						dir = FourDir.Down;
					}
					else if (vector2Int == Vector2Int.left)
					{
						dir = FourDir.Left;
					}
					else if (vector2Int == Vector2Int.right)
					{
						dir = FourDir.Right;
					}
					else if (vector2Int == Vector2Int.up)
					{
						dir = FourDir.Up;
					}
					RoomFinishLogger.SideRoomInfo sideRoomInfo = new RoomFinishLogger.SideRoomInfo
					{
						dir = dir,
						id = roomController.roomCfg.id,
						type = roomController.roomCfg.type,
						reward = new List<PlayerLogger.Item>(),
						spend_seconds = 0,
						unlocked = false
					};
					if (sideRoomInfo.type != 0)
					{
						RoomFinishLogger.side_room.Add(sideRoomInfo);
					}
					float enterTime = 0f;
					roomController.RoomEnterRegister(delegate
					{
						enterTime = DataMgr.selectedWorldData.timeuse;
					});
					roomController.RoomLeaveRegister(delegate
					{
						float f = DataMgr.selectedWorldData.timeuse - enterTime;
						sideRoomInfo.spend_seconds += Mathf.CeilToInt(f);
					});
				}
				else
				{
					RoomFinishLogger.current_room = roomController.roomCfg.name;
				}
			}
		}
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		if (GameMgr.CampSkinType == CampSkinType.Summer && (CurrentRewardType == LevelRewardType.Store || CurrentRewardType == LevelRewardType.Process))
		{
			UnityEngine.Object.Instantiate(new GameObject("SeagullSpawner")).AddComponent<SeagullSpawner>().transform.SetParent(CurrentRoomCtrller.transform);
		}
	}

	public void PlayerEnterAccess(FourDir dir)
	{
		StartCoroutine(PlayerEnterAccessIE(dir));
	}

	private IEnumerator PlayerEnterAccessIE(FourDir dir)
	{
		if (playerAccessLocked)
		{
			yield break;
		}
		float num = 0.6f;
		if (PlayerMgr.Inst.PlayerT.localScale.x > 1f)
		{
			num += (PlayerMgr.Inst.PlayerT.localScale.x - 1f) * PlayerMgr.Inst.PlayerPpt.CC_Self.radius;
		}
		bool flag = false;
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			flag = flag || (wand.IsCharging && dir == FourDir.Up);
		}
		if (flag && dir == FourDir.Up)
		{
			Vector3 playerPoint = CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up) + new Vector3(0f, 0f - num);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint);
			yield break;
		}
		UIPlayerDataMgr.Inst.CancelDrag();
		Vector3 vector = Vector3.zero;
		Vector3 playerPoint2 = Vector3.zero;
		RoomController currentRoomCtrller = CurrentRoomCtrller;
		switch (dir)
		{
		case FourDir.Up:
			CurrentRoomMapPos += new Vector2Int(0, 1);
			vector = CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Down);
			playerPoint2 = vector + new Vector3(0f, num, 0f);
			CurrentRoomCtrller.MoveAllTeammates(currentRoomCtrller);
			break;
		case FourDir.Right:
			CurrentRoomMapPos += new Vector2Int(1, 0);
			vector = CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Left);
			playerPoint2 = vector + new Vector3(num, 0f, 0f);
			CurrentRoomCtrller.MoveAllTeammates(currentRoomCtrller);
			break;
		case FourDir.Down:
			CurrentRoomMapPos += new Vector2Int(0, -1);
			vector = CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up);
			playerPoint2 = vector + new Vector3(0f, 0f - num, 0f);
			CurrentRoomCtrller.MoveAllTeammates(currentRoomCtrller);
			break;
		case FourDir.Left:
			CurrentRoomMapPos += new Vector2Int(-1, 0);
			vector = CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Right);
			playerPoint2 = vector + new Vector3(0f - num, 0f, 0f);
			CurrentRoomCtrller.MoveAllTeammates(currentRoomCtrller);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		PlayerMgr.Inst.SetPlayerPoint(playerPoint2);
		DoorPassCrystalRefresh();
		if (dir == FourDir.Up && CurrentRoomCfg.type == RoomType.Boss)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		currentRoomCtrller.RoomLeave();
		currentRoomCtrller.fogCtrller.Show();
		CurrentRoomCtrller.RoomEnter();
		CurrentRoomCtrller.fogCtrller.Hide((FourDir)(0 - dir));
		if (CurrentRoomCfg.isFinalRoom && RoomFinishLogger != null)
		{
			RoomFinishLogger.battle_data.room_type = CurrentRoomCfg.type;
			RoomFinishLogger.battle_data.room_id = CurrentRoomCfg.id;
		}
		PlayerMgr.Inst.SummonsThrough();
		PlayerMgr.Inst.ItemCtrller.ItemPointerToPlayer();
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor != null)
		{
			PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor.StopSprint();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_Stealthy != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_Stealthy.EnterSideRoom(currentRoomCtrller);
		}
		GeneralTool.SyncTeammatesPosition(vector);
		GeneralTool.ResetTeammatesMotion();
		if (dir == FourDir.Up && CurrentRoomCfg.type == RoomType.Boss)
		{
			yield return null;
			yield return null;
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
	}

	public void ChangeGlobalLightColor(RoomThemeType themeType)
	{
		isGlobalLightChanging = true;
		originalGlobalLightColor = globalLight.color;
		targetGlobalLightColor = themeLightColors[(int)themeType];
		if (GameMgr.IsLightErrorDevice)
		{
			targetGlobalLightColor = Color.white;
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_DarkView != null)
		{
			if (GameMgr.IsLightErrorDevice)
			{
				targetGlobalLightColor *= 0.3f;
			}
			else
			{
				targetGlobalLightColor *= 0.1f;
			}
			targetGlobalLightColor.a = 1f;
		}
		changeColorTimer = 0f;
	}

	public void ChangeGlobalLightColor(Color customColor)
	{
		isGlobalLightChanging = true;
		originalGlobalLightColor = globalLight.color;
		targetGlobalLightColor = customColor;
		if (GameMgr.IsLightErrorDevice)
		{
			targetGlobalLightColor = Color.white;
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_DarkView != null)
		{
			if (GameMgr.IsLightErrorDevice)
			{
				targetGlobalLightColor *= 0.3f;
			}
			else
			{
				targetGlobalLightColor *= 0.1f;
			}
			targetGlobalLightColor.a = 1f;
		}
		changeColorTimer = 0f;
	}

	public Color GetThemeLightColor(RoomThemeType themeType)
	{
		if ((int)themeType >= themeLightColors.Length)
		{
			Debug.LogError(themeType);
			return Color.black;
		}
		return themeLightColors[(int)themeType];
	}

	public bool HaveNeighbor(Vector2Int checkLevelMapPoint, int offsetX, int offsetY)
	{
		if (RoomCfgs.ContainsKey(checkLevelMapPoint + new Vector2Int(offsetX, offsetY)))
		{
			return true;
		}
		return false;
	}

	public void AllRoomAllAccessOpen()
	{
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			roomCtrller.Value.AllAccessOpen();
		}
	}

	public void AllRoomAllAccessOpenDirect()
	{
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			roomCtrller.Value.AllAccessOpenDirect();
		}
	}

	public void AllRoomAllAccessClose()
	{
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			roomCtrller.Value.AllAccessClose();
		}
	}

	public void AllRoomAllAccessCloseDirect()
	{
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			roomCtrller.Value.AllAccessCloseDirect();
		}
	}

	public void AllRoomAllDoorUpdateDisplay()
	{
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in RoomCtrllers)
		{
			for (int i = 0; i < roomCtrller.Value.doorEttList.Count; i++)
			{
				DoorBase_Dots componentData = ettMgr.GetComponentData<DoorBase_Dots>(roomCtrller.Value.doorEttList[i]);
				componentData.onUpdateDisplay = true;
				ettMgr.SetComponentData(roomCtrller.Value.doorEttList[i], componentData);
			}
		}
	}

	private DynamicBuffer<Spell4014LaserCrystalSystem.CrystalPassDoorPosFix> GetCrystalDoorPassBuffer()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (_crystalDoorPassBuffer == Entity.Null)
		{
			using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(Spell4014LaserCrystalSystem.CrystalPassDoorPosFix));
			if (!entityQuery.IsEmpty)
			{
				_crystalDoorPassBuffer = entityQuery.GetSingletonEntity();
			}
		}
		return entityManager.GetBuffer<Spell4014LaserCrystalSystem.CrystalPassDoorPosFix>(_crystalDoorPassBuffer);
	}

	private void DoorPassCrystalRefresh()
	{
		GetCrystalDoorPassBuffer().Add(default(Spell4014LaserCrystalSystem.CrystalPassDoorPosFix));
	}
}
