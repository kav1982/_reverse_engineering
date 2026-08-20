using System;
using System.Collections;
using System.Collections.Generic;
using PlayerLogger.Events;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CampMgr : MonoBehaviour
{
	public GameObject go_StartDestroy;

	public Vector3 so36CreatePoint;

	public Vector3 so36CreatePointMobile;

	public AudioSource as_bonfire;

	public Vector3 playerBornPoint;

	public Vector3 npc6PositionStage0;

	public Vector3 campFirePositionMoveDown;

	public Vector3 npc4StoryPosition;

	public Vector3 endlessEntryPos;

	public Vector3 endlessExitPos;

	[SerializeField]
	private Transform tsf_ScarecrowParentPC;

	[SerializeField]
	private Transform tsf_ScarecrowParentMobile;

	public GameObject go_TrainningRoomLight;

	[Header("手游用")]
	public Vector3 Npc1HolloweenPositionOffset;

	public Vector3 Npc3HolloweenPositionOffset;

	public Vector3 Npc1SpringPositionOffset;

	public Vector3 Npc3SpringPositionOffset;

	public Vector3 Npc1SummerPositionOffset;

	public Vector3 Npc3SummerPositionOffset;

	public Vector3 Npc3MobileOffset;

	public Vector3 npc6PositionStage0Mobile;

	public Vector3 Npc6MobileOffset;

	public Vector3 Npc4MobileOffset;

	public int Npc4Stage3SetUnlockCountRequired = 5;

	public Vector3 mobileCamOffset;

	public Vector3 mobileTrainingRoomLightOffset;

	private bool _isDotsInitialized;

	private EntityManager ettMgr;

	public CampSkinBED CurrentCampSkin;

	private bool isCreateScene;

	private Entity ett_EndlessCamp;

	private Entity ett_EndlessGate;

	private Entity ett_EndlessGallery;

	private Entity ett_EndlessRankingList;

	public static CampMgr Inst { get; private set; }

	public Transform tsf_ScarecrowParent
	{
		get
		{
			if (!GameMgr.IsMobileCamp)
			{
				return tsf_ScarecrowParentPC;
			}
			return tsf_ScarecrowParentMobile;
		}
	}

	public bool isDotsInitialized => _isDotsInitialized;

	public NPC1Vivian npc1Vivian { get; set; }

	public NPC2Nimue npc2Nimue { get; set; }

	public NPC3 npc3 { get; set; }

	public NPC4 npc4 { get; set; }

	public NPC5 npc5 { get; set; }

	public NPC6 npc6 { get; set; }

	public NPC9_EndlessGuide npc9 { get; set; }

	private void Awake()
	{
		Inst = this;
		UnityEngine.Object.Destroy(go_StartDestroy);
		DataMgr.selectedWorldData.inBattle9 = false;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		go_TrainningRoomLight.SetActive(value: false);
		if (GameMgr.IsMobileCamp)
		{
			go_TrainningRoomLight.transform.position += mobileTrainingRoomLightOffset;
		}
	}

	private void Start()
	{
		DamageRecordeManager.ClearAllRecorde();
		UIMgr.Inst.uiFade.Show(0f);
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		if (as_bonfire != null && as_bonfire.gameObject.activeInHierarchy)
		{
			as_bonfire.volume = DataMgr.settingData.GetFinalSound();
		}
	}

	private void Update()
	{
		StartHandle();
	}

	private void StartHandle()
	{
		if (isCreateScene)
		{
			return;
		}
		ettMgr.CheckInitialize(ref _isDotsInitialized);
		if (!_isDotsInitialized)
		{
			return;
		}
		using (EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(CampAllEtt)))
		{
			if (entityQuery.CalculateEntityCount() == 0)
			{
				return;
			}
			Entity singletonEntity = entityQuery.GetSingletonEntity();
			CampAllEtt componentData = ettMgr.GetComponentData<CampAllEtt>(singletonEntity);
			DynamicBuffer<CampSkinBED> buffer = ettMgr.GetBuffer<CampSkinBED>(singletonEntity);
			int campSkinType = (int)GameMgr.CampSkinType;
			CampSkinBED campSkinBED = buffer[campSkinType * 2];
			CampSkinBED campSkinBED2 = (GameMgr.IsMobileCamp ? buffer[campSkinType * 2 + 1] : buffer[campSkinType * 2]);
			CurrentCampSkin.ett_AccessL = TryInstantiate(campSkinBED2.ett_AccessL, campSkinBED.ett_AccessL, campSkinBED2.positionOverride_AccessL);
			CurrentCampSkin.ett_AccessR = TryInstantiate(campSkinBED2.ett_AccessR, campSkinBED.ett_AccessR, campSkinBED2.positionOverride_AccessR);
			CurrentCampSkin.ett_AccessR_StoneRoad = TryInstantiate(campSkinBED2.ett_AccessR_StoneRoad, campSkinBED.ett_AccessR_StoneRoad, campSkinBED2.positionOverride_AccessR_StoneRoad);
			CurrentCampSkin.ett_Camp = TryInstantiate(campSkinBED2.ett_Camp, campSkinBED.ett_Camp, campSkinBED2.positionOverride_Camp);
			CurrentCampSkin.ett_Decoration_CampStage0 = TryInstantiate(campSkinBED2.ett_Decoration_CampStage0, campSkinBED.ett_Decoration_CampStage0, campSkinBED2.positionOverride_Decoration_CampStage0);
			CurrentCampSkin.ett_Decoration_CampStage1 = TryInstantiate(campSkinBED2.ett_Decoration_CampStage1, campSkinBED.ett_Decoration_CampStage1, campSkinBED2.positionOverride_Decoration_CampStage1);
			CurrentCampSkin.ett_Decoration_CampStage2 = TryInstantiate(campSkinBED2.ett_Decoration_CampStage2, campSkinBED.ett_Decoration_CampStage2, campSkinBED2.positionOverride_Decoration_CampStage2);
			CurrentCampSkin.ett_Decoration_CampStage3 = TryInstantiate(campSkinBED2.ett_Decoration_CampStage3, campSkinBED.ett_Decoration_CampStage3, campSkinBED2.positionOverride_Decoration_CampStage3);
			CurrentCampSkin.ett_Decoration_NPC4Stage1 = TryInstantiate(campSkinBED2.ett_Decoration_NPC4Stage1, campSkinBED.ett_Decoration_NPC4Stage1, campSkinBED2.positionOverride_Decoration_NPC4Stage1);
			CurrentCampSkin.ett_Decoration_NPC4Stage2 = TryInstantiate(campSkinBED2.ett_Decoration_NPC4Stage2, campSkinBED.ett_Decoration_NPC4Stage2, campSkinBED2.positionOverride_Decoration_NPC4Stage2);
			CurrentCampSkin.ett_Decoration_NPC4Stage3 = TryInstantiate(campSkinBED2.ett_Decoration_NPC4Stage3, campSkinBED.ett_Decoration_NPC4Stage3, campSkinBED2.positionOverride_Decoration_NPC4Stage3);
			CurrentCampSkin.ett_Decoration_Npc6GroundStage1 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6GroundStage1, campSkinBED.ett_Decoration_Npc6GroundStage1, campSkinBED2.positionOverride_Decoration_Npc6GroundStage1);
			CurrentCampSkin.ett_Decoration_Npc6GroundStage2 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6GroundStage2, campSkinBED.ett_Decoration_Npc6GroundStage2, campSkinBED2.positionOverride_Decoration_Npc6GroundStage2);
			CurrentCampSkin.ett_Decoration_Npc6GroundStage3 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6GroundStage3, campSkinBED.ett_Decoration_Npc6GroundStage3, campSkinBED2.positionOverride_Decoration_Npc6GroundStage3);
			CurrentCampSkin.ett_Decoration_Npc6TentacleStage1 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6TentacleStage1, campSkinBED.ett_Decoration_Npc6TentacleStage1, campSkinBED2.positionOverride_Decoration_Npc6TentacleStage1);
			CurrentCampSkin.ett_Decoration_Npc6TentacleStage2 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6TentacleStage2, campSkinBED.ett_Decoration_Npc6TentacleStage2, campSkinBED2.positionOverride_Decoration_Npc6TentacleStage2);
			CurrentCampSkin.ett_Decoration_Npc6TentacleStage3 = TryInstantiate(campSkinBED2.ett_Decoration_Npc6TentacleStage3, campSkinBED.ett_Decoration_Npc6TentacleStage3, campSkinBED2.positionOverride_Decoration_Npc6TentacleStage3);
			CurrentCampSkin.ett_Decoration_Tent1 = TryInstantiate(campSkinBED2.ett_Decoration_Tent1, campSkinBED.ett_Decoration_Tent1, campSkinBED2.positionOverride_Decoration_Tent1);
			CurrentCampSkin.ett_Decoration_Tent2 = TryInstantiate(campSkinBED2.ett_Decoration_Tent2, campSkinBED.ett_Decoration_Tent2, campSkinBED2.positionOverride_Decoration_Tent2);
			CurrentCampSkin.ett_Decoration_WoodStage2 = TryInstantiate(campSkinBED2.ett_Decoration_WoodStage2, campSkinBED.ett_Decoration_WoodStage2, campSkinBED2.positionOverride_Decoration_WoodStage2);
			CurrentCampSkin.ett_Decoration_WoodStage3 = TryInstantiate(campSkinBED2.ett_Decoration_WoodStage3, campSkinBED.ett_Decoration_WoodStage3, campSkinBED2.positionOverride_Decoration_WoodStage3);
			CurrentCampSkin.ett_Leaf_2 = TryInstantiate(campSkinBED2.ett_Leaf_2, campSkinBED.ett_Leaf_2, campSkinBED2.positionOverride_Leaf_2);
			CurrentCampSkin.ett_ResearchTable = TryInstantiate(campSkinBED2.ett_ResearchTable, campSkinBED.ett_ResearchTable, campSkinBED2.positionOverride_ResearchTable);
			CurrentCampSkin.ett_TrainingRoom = TryInstantiate(campSkinBED2.ett_TrainingRoom, campSkinBED.ett_TrainingRoom, campSkinBED2.positionOverride_TrainingRoom);
			CurrentCampSkin.ett_WallR_Access_Training = TryInstantiate(campSkinBED2.ett_WallR_Access_Training, campSkinBED.ett_WallR_Access_Training, campSkinBED2.positionOverride_WallR_Access_Training);
			CurrentCampSkin.ett_灯柱 = TryInstantiate(campSkinBED2.ett_灯柱, campSkinBED.ett_灯柱, campSkinBED2.positionOverride_灯柱);
			CurrentCampSkin.ett_训练房木桩 = TryInstantiate(campSkinBED2.ett_训练房木桩, campSkinBED.ett_训练房木桩, campSkinBED2.positionOverride_训练房木桩);
			CurrentCampSkin.ett_Billboard = TryInstantiate(campSkinBED2.ett_Billboard, campSkinBED.ett_Billboard, campSkinBED2.positionOverride_Billboard);
			CurrentCampSkin.ett_CampMirror = TryInstantiate(campSkinBED2.ett_CampMirror, campSkinBED.ett_CampMirror, campSkinBED2.positionOverride_CampMirror);
			CurrentCampSkin.ett_CampMirrorStand = TryInstantiate(campSkinBED2.ett_CampMirrorStand, campSkinBED.ett_CampMirrorStand, campSkinBED2.positionOverride_CampMirrorStand);
			CurrentCampSkin.ett_CampSkinChanger = TryInstantiate(campSkinBED2.ett_CampSkinChanger, campSkinBED.ett_CampSkinChanger, campSkinBED2.positionOverride_CampSkinChanger);
			CurrentCampSkin.ett_DoorCamp = TryInstantiate(campSkinBED2.ett_DoorCamp, campSkinBED.ett_DoorCamp, campSkinBED2.positionOverride_DoorCamp);
			CurrentCampSkin.ett_Gallery = TryInstantiate(campSkinBED2.ett_Gallery, campSkinBED.ett_Gallery, campSkinBED2.positionOverride_Gallery);
			CurrentCampSkin.ett_GiftSet = TryInstantiate(campSkinBED2.ett_GiftSet, campSkinBED.ett_GiftSet, campSkinBED2.positionOverride_GiftSet);
			CurrentCampSkin.ett_RankingList = TryInstantiate(campSkinBED2.ett_RankingList, campSkinBED.ett_RankingList, campSkinBED2.positionOverride_RankingList);
			CurrentCampSkin.ett_ResourceChanger = TryInstantiate(campSkinBED2.ett_ResourceChanger, campSkinBED.ett_ResourceChanger, campSkinBED2.positionOverride_ResourceChanger);
			CurrentCampSkin.ett_DownWallClose = TryInstantiate(campSkinBED2.ett_DownWallClose, campSkinBED.ett_DownWallClose, campSkinBED2.positionOverride_DownWallClose);
			CurrentCampSkin.ett_DownWallOpen = TryInstantiate(campSkinBED2.ett_DownWallOpen, campSkinBED.ett_DownWallOpen, campSkinBED2.positionOverride_DownWallOpen);
			CurrentCampSkin.ett_AccessD = TryInstantiate(campSkinBED2.ett_AccessD, campSkinBED.ett_AccessD, campSkinBED2.positionOverride_AccessD);
			ett_EndlessCamp = ettMgr.Instantiate(componentData.ett_EndlessCamp);
			ett_EndlessGate = ettMgr.Instantiate(componentData.ett_EndlessGate);
			ett_EndlessGallery = ettMgr.Instantiate(componentData.ett_EndlessGallery);
			ett_EndlessRankingList = ettMgr.Instantiate(componentData.ett_EndlessRankingList);
			isCreateScene = true;
			npc1Vivian = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC1")).GetComponent<NPC1Vivian>();
			npc2Nimue = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC2")).GetComponent<NPC2Nimue>();
			npc3 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC3")).GetComponent<NPC3>();
			npc4 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC4")).GetComponent<NPC4>();
			npc5 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC5")).GetComponent<NPC5>();
			npc6 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC6")).GetComponent<NPC6>();
			npc9 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/NPCs/NPC9")).GetComponent<NPC9_EndlessGuide>();
			if (GameMgr.IsMobileCamp)
			{
				if (GameMgr.CampSkinType == CampSkinType.Halloween)
				{
					npc1Vivian.gameObject.transform.position += Npc1HolloweenPositionOffset;
					npc3.gameObject.transform.position += Npc3HolloweenPositionOffset;
				}
				else if (GameMgr.CampSkinType == CampSkinType.Spring)
				{
					npc1Vivian.gameObject.transform.position += Npc1SpringPositionOffset;
					npc3.gameObject.transform.position += Npc3SpringPositionOffset;
				}
				else if (GameMgr.CampSkinType == CampSkinType.Summer)
				{
					npc1Vivian.gameObject.transform.position += Npc1SummerPositionOffset;
					npc3.gameObject.transform.position += Npc3SummerPositionOffset;
				}
				else
				{
					npc3.gameObject.transform.position += Npc3MobileOffset;
				}
				npc4.gameObject.transform.position += Npc4MobileOffset;
				npc6.gameObject.transform.position += Npc6MobileOffset;
			}
			Spell4019BiAnBladeSystem.HideBlade = false;
			DamageRecordeManager.ClearAllRecorde();
			GameMgr.Inst.AllFunctionReset();
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Easy))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishEasy);
			}
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNormal);
			}
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Hard))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishHard);
			}
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Nightmare1))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare1);
			}
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Nightmare2))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare2);
			}
			if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Nightmare3))
			{
				SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNightmare3);
			}
			PlayerMgr.Inst.CreatePlayer();
			CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
			UIPlayerDataMgr.Inst.UpdateAllInfo();
			Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
			dictionary.Add(Vector2Int.zero, RoomConfig.GetConfig(101));
			dictionary.Add(Vector2Int.right, RoomConfig.GetConfig(101));
			dictionary.Add(Vector2Int.down, RoomConfig.GetConfig(101));
			LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.None, LevelRewardType.None, fadeDisappear: true, StartDelayHandle);
			DataMgr.selectedWorldData.timeuse = 0f;
		}
		Entity TryInstantiate(Entity entity, Entity defaultEntity, float3 mobileOffset)
		{
			Entity srcEntity = ((entity == Entity.Null) ? defaultEntity : entity);
			Entity entity2 = ettMgr.Instantiate(srcEntity);
			if (GameMgr.IsMobileCamp && !mobileOffset.Equals(float3.zero))
			{
				LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(entity2);
				componentData2.Position = mobileOffset;
				ettMgr.SetComponentData(entity2, componentData2);
			}
			return entity2;
		}
	}

	private void StartDelayHandle()
	{
		StartCoroutine(StartDelayHandleIE());
	}

	private IEnumerator StartDelayHandleIE()
	{
		if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			UnityEngine.Object.Instantiate(new GameObject("SeagullSpawner")).AddComponent<SeagullSpawner>();
		}
		PlayerMgr.Inst.InvincibleRegister();
		PlayerMgr.Inst.WandRecreate();
		PlayerMgr.Inst.WandSelect(0);
		PlayerMgr.Inst.AllWandFullMP();
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
		{
			roomCtrller.Value.SetEnterRoomIsUpdateThemeMusic(whenEnterRoomUpdateThemeMusic: false);
		}
		UIPlayerDataMgr.Inst.Show();
		MusicMgr.Inst.UpdateCampBGM();
		if (GameMgr.IsMobileCamp)
		{
			LevelMgr.Inst.CurrentRoomT.position += mobileCamOffset;
			LevelMgr.Inst.CurrentRoomCtrller.Initialize2();
		}
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.AdvancedScarecrow) != 0)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + 3601), GameMgr.IsMobileCamp ? so36CreatePointMobile : so36CreatePoint, Quaternion.identity);
		}
		DataMgr.selectedWorldData.SetFindSet9();
		PlayerMgr.Inst.SetPlayerPoint(playerBornPoint);
		if (ScriptableObjMgr.Inst.testCtrller.DirectEnterScene && ScriptableObjMgr.Inst.testCtrller.enterScene == TestDirectEnterScene.CampTraining)
		{
			PlayerMgr.Inst.SetPlayerPoint(ettMgr.GetComponentData<LocalTransform>(CurrentCampSkin.ett_AccessR).Position);
		}
		if (ScriptableObjMgr.Inst.testCtrller.DirectEnterScene && ScriptableObjMgr.Inst.testCtrller.enterScene == TestDirectEnterScene.EndlessCamp)
		{
			PlayerMgr.Inst.SetPlayerPoint(new float3(0f, 12.5f, 0f));
		}
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC)
		{
			DataMgr.selectedWorldData.haveUsed = true;
			DataMgr.selectedWorldData.firstEnterBattle = false;
			ShowCampObj(openAllNpc: true);
			yield return new WaitForSeconds(1f);
			UIPlaceNameMgr.Inst.Show(PlaceNameType.Camp);
			yield break;
		}
		ShowCampObj(openAllNpc: false);
		if (DataMgr.selectedWorldData.storyKillChapter3BossPickup && !DataMgr.selectedWorldData.storyNormalFinishBackCamp)
		{
			DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
			DataMgr.SaveSelectedWorldData();
			UnlockNPCLogger unlockNPCLogger = new UnlockNPCLogger();
			unlockNPCLogger.npc_id = 6;
			unlockNPCLogger.Report();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_EasyFinishBackCamp"));
			yield break;
		}
		if (DataMgr.selectedWorldData.storyHardBossDropPickup && !DataMgr.selectedWorldData.storyHardFinishBackCamp)
		{
			DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
			DataMgr.SaveSelectedWorldData();
			UnlockNPCLogger unlockNPCLogger2 = new UnlockNPCLogger();
			unlockNPCLogger2.npc_id = 7;
			unlockNPCLogger2.Report();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_NormalFinishBackCamp"));
			yield break;
		}
		if (DataMgr.selectedWorldData.storyFinishHardDropPickup && !DataMgr.selectedWorldData.storyFinishHardBackCamp)
		{
			DataMgr.selectedWorldData.storyFinishHardBackCamp = true;
			DataMgr.SaveSelectedWorldData();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_HardFinishBackCamp"));
			yield break;
		}
		if (DataMgr.selectedWorldData.storyFinishNightmare1 && !DataMgr.selectedWorldData.storyFinishNightmare1BackCamp)
		{
			DataMgr.selectedWorldData.storyFinishNightmare1BackCamp = true;
			DataMgr.SaveSelectedWorldData();
			yield break;
		}
		if (DataMgr.selectedWorldData.storyFinishNightmare2 && !DataMgr.selectedWorldData.storyFinishNightmare2BackCamp)
		{
			DataMgr.selectedWorldData.storyFinishNightmare2BackCamp = true;
			DataMgr.SaveSelectedWorldData();
			yield break;
		}
		if (DataMgr.selectedWorldData.storyFinishNightmare3 && !DataMgr.selectedWorldData.storyFinishNightmare3BackCamp)
		{
			DataMgr.selectedWorldData.storyFinishNightmare3BackCamp = true;
			DataMgr.SaveSelectedWorldData();
			yield break;
		}
		if (!DataMgr.selectedWorldData.story1Finish)
		{
			DataMgr.selectedWorldData.story1Finish = true;
			DataMgr.SaveSelectedWorldData();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story1"));
			yield break;
		}
		if (DataMgr.selectedWorldData.isPlayerDeadBackCamp)
		{
			DataMgr.selectedWorldData.isPlayerDeadBackCamp = false;
			DataMgr.SaveSelectedWorldData();
			if (GameMgr.IsMobileCamp)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/PlayerDeadBackCampMobile"));
			}
			else
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/PlayerDeadBackCamp"));
			}
		}
		else
		{
			if (DataMgr.selectedWorldData.directEnterCampByLoadSave)
			{
				DataMgr.selectedWorldData.directEnterCampByLoadSave = false;
				if (UnityEngine.Random.Range(0f, 1f) > 0.4f && DataMgr.selectedWorldData.TryActiveReEnterGameRandomDialog())
				{
					if ((bool)npc1Vivian)
					{
						npc1Vivian.CheckPlot();
					}
					if ((bool)npc2Nimue)
					{
						npc2Nimue.CheckPlot();
					}
					if ((bool)npc3)
					{
						npc3.CheckPlot();
					}
					if ((bool)npc4)
					{
						npc4.CheckPlot();
					}
					if ((bool)npc5)
					{
						npc5.CheckPlot();
					}
					if ((bool)npc6)
					{
						npc6.CheckPlot();
					}
				}
			}
			yield return new WaitForSeconds(1f);
			UIPlaceNameMgr.Inst.Show(PlaceNameType.Camp);
		}
		if (DataMgr.selectedWorldData.story2Open && !DataMgr.selectedWorldData.story2Finish)
		{
			DataMgr.selectedWorldData.story2Finish = true;
			DataMgr.SaveSelectedWorldData();
			UnlockNPCLogger unlockNPCLogger3 = new UnlockNPCLogger();
			unlockNPCLogger3.npc_id = 3;
			unlockNPCLogger3.Report();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story2"));
		}
		else if (DataMgr.selectedWorldData.story3NPC4Rescued && !DataMgr.selectedWorldData.story3Finish)
		{
			DataMgr.selectedWorldData.story3Finish = true;
			DataMgr.SaveSelectedWorldData();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story3"));
			npc4.transform.position = npc4StoryPosition;
		}
		else if (DataMgr.selectedWorldData.story4NPC5Rescued && !DataMgr.selectedWorldData.story4Finish)
		{
			DataMgr.selectedWorldData.story4Finish = true;
			DataMgr.SaveSelectedWorldData();
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story4")).GetComponent<Story4>().Initialize(DataMgr.selectedWorldData.story4NPC5ForceShow ? 24 : 22);
		}
		else
		{
			yield return null;
		}
	}

	public void SetEttEnable(Entity ett, bool enable)
	{
		if (ett == Entity.Null)
		{
			Debug.Log("Entity is Null");
			return;
		}
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(ett);
		componentData.Position.z = ((!enable) ? 10000 : 0);
		ettMgr.SetComponentData(ett, componentData);
	}

	public void SetEttPos(Entity ett, float3 pos)
	{
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(ett);
		componentData.Position = pos;
		ettMgr.SetComponentData(ett, componentData);
	}

	public void CampStage0SetActive(bool active)
	{
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage0, active);
		as_bonfire.gameObject.SetActive(active);
	}

	public void ShowCampObj(bool openAllNpc)
	{
		CampStage0SetActive(active: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Tent1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Tent2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Billboard, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampMirror, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampMirrorStand, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampSkinChanger, enable: false);
		SetEttEnable(CurrentCampSkin.ett_ResearchTable, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Gallery, enable: false);
		SetEttEnable(CurrentCampSkin.ett_ResourceChanger, enable: false);
		SetEttEnable(CurrentCampSkin.ett_GiftSet, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Leaf_2, enable: true);
		SetEttEnable(CurrentCampSkin.ett_AccessR_StoneRoad, enable: false);
		SetEttEnable(CurrentCampSkin.ett_WallR_Access_Training, enable: true);
		SetEttEnable(CurrentCampSkin.ett_AccessD, enable: false);
		SetEttEnable(ett_EndlessGallery, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage3, enable: false);
		if (GameMgr.IsMobile_Static)
		{
			SetEttEnable(CurrentCampSkin.ett_RankingList, enable: false);
		}
		npc3.Hide();
		npc4.Hide();
		npc5.Hide();
		npc6.Hide();
		npc9.Hide();
		if (DataMgr.selectedWorldData.story1Finish || openAllNpc)
		{
			NPC2DecorateStage();
		}
		if (DataMgr.selectedWorldData.story2Finish || openAllNpc)
		{
			npc3.Show();
			NPC3DecorateStage(1);
		}
		if (DataMgr.selectedWorldData.story3Finish || openAllNpc)
		{
			NPC3DecorateStage(2);
		}
		if (DataMgr.selectedWorldData.story4Finish || openAllNpc)
		{
			NPC3DecorateStage(3);
		}
		int num = 0;
		if (DataMgr.selectedWorldData.story3Finish || openAllNpc)
		{
			num++;
			if (DataMgr.selectedWorldData.canSetUpgrade)
			{
				num++;
			}
			if (DataMgr.selectedWorldData.GetSetUnlockCount() > Npc4Stage3SetUnlockCountRequired)
			{
				num++;
			}
			npc4.Show();
			NPC4DecorateStage(num);
		}
		if (DataMgr.selectedWorldData.story4NPC5Rescued || openAllNpc)
		{
			npc5.Show();
			NPC5DecorateShow();
		}
		if (DataMgr.selectedWorldData.storyKillChapter3BossPickup || openAllNpc)
		{
			npc6.Show();
			int npc6Stage = 0;
			if (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Count >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[1])
			{
				npc6Stage = 1;
			}
			if (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Count >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[2])
			{
				npc6Stage = 2;
			}
			if (DataMgr.selectedWorldData.activateGirlActivatedIDs2.Count >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[3])
			{
				npc6Stage = 3;
			}
			NPC6DecorateStage(npc6Stage);
		}
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Gallery) != 0 || openAllNpc)
		{
			SetEttEnable(CurrentCampSkin.ett_Gallery, enable: true);
		}
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ResourceChanger) != 0 || openAllNpc)
		{
			SetEttEnable(CurrentCampSkin.ett_ResourceChanger, enable: true);
		}
		if ((DataMgr.selectedWorldData.story1Finish && ICJNOGPFMAM.OBKJLONPFGA) || openAllNpc)
		{
			SetEttEnable(CurrentCampSkin.ett_CampSkinChanger, enable: true);
		}
		if (ICJNOGPFMAM.HLMJIJADLNC)
		{
			SetEttEnable(CurrentCampSkin.ett_AccessD, enable: true);
			npc9.Show();
			if (DataMgr.selectedWorldData.endless_LevelOfGallery > 0)
			{
				SetEttEnable(ett_EndlessGallery, enable: true);
			}
			SetEttEnable(CurrentCampSkin.ett_DownWallClose, enable: false);
			SetEttEnable(CurrentCampSkin.ett_DownWallOpen, enable: true);
		}
		else
		{
			SetEttEnable(CurrentCampSkin.ett_DownWallClose, enable: true);
			SetEttEnable(CurrentCampSkin.ett_DownWallOpen, enable: false);
			DataMgr.SaveSelectedWorldData();
		}
	}

	public void CloseAllDecorate()
	{
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Tent2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampMirrorStand, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampSkinChanger, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Leaf_2, enable: true);
		SetEttEnable(CurrentCampSkin.ett_AccessR_StoneRoad, enable: false);
		SetEttEnable(CurrentCampSkin.ett_WallR_Access_Training, enable: true);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage3, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage1, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage2, enable: false);
		SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage3, enable: false);
	}

	public void NPC2DecorateStage(bool show = true)
	{
		SetEttEnable(CurrentCampSkin.ett_Decoration_Tent1, show);
		CampStage0SetActive(show);
		SetEttEnable(CurrentCampSkin.ett_Billboard, show);
		SetEttEnable(CurrentCampSkin.ett_CampMirror, show);
	}

	public void NPC3DecorateStage(int _npc3Stage)
	{
		if (_npc3Stage == 1)
		{
			SetEttEnable(CurrentCampSkin.ett_ResearchTable, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage1, enable: true);
		}
		if (_npc3Stage == 2)
		{
			SetEttEnable(CurrentCampSkin.ett_Decoration_Tent1, enable: false);
			SetEttEnable(CurrentCampSkin.ett_Decoration_Tent2, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage2, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage2, enable: true);
		}
		if (_npc3Stage == 3)
		{
			SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage2, enable: false);
			SetEttEnable(CurrentCampSkin.ett_Decoration_WoodStage3, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_CampStage3, enable: true);
		}
	}

	public void NPC4DecorateStage(int _npc4Stage)
	{
		SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage1, enable: true);
		SetEttEnable(CurrentCampSkin.ett_CampMirror, enable: false);
		SetEttEnable(CurrentCampSkin.ett_CampMirrorStand, enable: true);
		if (_npc4Stage >= 2)
		{
			SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage2, enable: true);
		}
		if (_npc4Stage >= 3)
		{
			SetEttEnable(CurrentCampSkin.ett_Decoration_NPC4Stage3, enable: true);
		}
	}

	public void NPC5DecorateShow(bool show = true)
	{
		SetEttEnable(CurrentCampSkin.ett_Leaf_2, !show);
		SetEttEnable(CurrentCampSkin.ett_AccessR_StoneRoad, show);
		SetEttEnable(CurrentCampSkin.ett_WallR_Access_Training, !show);
		go_TrainningRoomLight.SetActive(show);
	}

	public void NPC6DecorateStage(int _npc6Stage)
	{
		switch (_npc6Stage)
		{
		case 0:
			npc6.transform.position = (GameMgr.IsMobileCamp ? npc6PositionStage0Mobile : npc6PositionStage0);
			npc6.CorrectLayerOnce();
			break;
		case 1:
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage1, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage1, enable: true);
			break;
		case 2:
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage2, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage2, enable: true);
			break;
		case 3:
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6GroundStage3, enable: true);
			SetEttEnable(CurrentCampSkin.ett_Decoration_Npc6TentacleStage3, enable: true);
			break;
		}
	}

	public void DestroyScarecrowPoints()
	{
		tsf_ScarecrowParentPC.DestroyAllChild();
		tsf_ScarecrowParentMobile.DestroyAllChild();
	}

	private void OnDestroy()
	{
		Inst = null;
	}
}
