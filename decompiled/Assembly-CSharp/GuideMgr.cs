using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GuideMgr : MonoBehaviour
{
	public enum GuideMgrState
	{
		WaitEnterStory1,
		Idle,
		WaitFirstMove,
		WaitEnterForestY1,
		WaitEnterForestY2,
		Finished
	}

	public GameObject go_StartToDestroy;

	public PlayableDirector pd_Story1;

	public PlayableDirector pd_Story2;

	public PlayableDirector pd_Story3;

	public PlayableDirector pd_Story4;

	public GameObject go_BathRoomCorridorLight;

	public Animator anima_FogBedroom;

	public Animator anima_FogBathroom;

	public float camFocusSize;

	public float camFocusTime;

	public float enterStory1WaitTime;

	public float enterStory1FocusTime;

	[Header("Story1Bedroom")]
	public Vector3 story1FocusPos;

	public Transform tsf_Story1Computer;

	public Transform tsf_Story1Player;

	public Vector2 story1PlayerPoint;

	public float story1ComputerMDOffset;

	public Animator anima_GuideImage1;

	public Color color_Room1GlobalLightColor;

	public Color color_Room2GlobalLightColor;

	public Light light_Room2RealGlobalLight;

	public Vector3 room1CampPos;

	public Vector3 room2CampPos;

	[Header("Story2Pee")]
	public Vector3 story2FocusPos;

	public RestroomLight restroomLight;

	public Transform tsf_Story2PlayerPee;

	public float story2DialogueOffset;

	[Header("Story3PeeThrough")]
	public Transform tsf_Story3PlayerPee;

	public Transform tsf_Story3Pee;

	public int story3TextID1;

	public int story3TextID2;

	public int story3TextID3;

	public float story3TextID2Delay;

	public float story3TextID3Delay;

	[Header("Story4WandInStone")]
	public WandInStoneMono wandInStoneMono;

	public Vector3 playerWalkPoint;

	public Vector3 playerShowPoint;

	public float pickWandFocusSize;

	public float pickWandFocusTime;

	public float pickWandFocusSize2;

	public float pickWandFocusTime2;

	public float pickWandFocusTime3;

	public ShockParam shockParam;

	public float guideDoorFocusTime;

	public float guideDoorFocusTime2;

	public float guideDoorOpenWaitTime;

	public GameObject go_OpenDoorEF;

	[Header("EnterDoor")]
	public float enterDoorWaitTime;

	public float enterDoorFocusSize;

	public float enterDoorFocusTime;

	public float enterDoorFocusPoint;

	[Header("InputChange")]
	public GameObject go_GuideImage_Gamepad;

	public GameObject go_GuideImage_Keyboard;

	[Header("Audio")]
	public AudioSource[] ass;

	private GuideMgrState state;

	private float enterStory1WaitTimer;

	private AccessTriggerGuideRoomType currentRooomType;

	private bool isInteractedToilet;

	private Color initialRoom2RealGlobalLight;

	private Vector3 targetCamPos;

	private Color targetGlobalLightColor;

	private bool _isDotsInitialized;

	private bool _isCampdestroyed;

	private EntityManager ettMgr;

	public static GuideMgr Inst { get; private set; }

	public bool IsPickedWand { get; private set; }

	private void Awake()
	{
		Inst = this;
		DataMgr.selectedWorldData.inBattle9 = false;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		currentRooomType = AccessTriggerGuideRoomType.Bedroom;
		initialRoom2RealGlobalLight = light_Room2RealGlobalLight.color;
		light_Room2RealGlobalLight.color = Color.black;
		targetGlobalLightColor = color_Room1GlobalLightColor;
		UnityEngine.Object.Destroy(go_StartToDestroy);
		if (GameMgr.IsMobileCamp)
		{
			go_OpenDoorEF.transform.position += Vector3.down * 1.5f;
		}
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void SoundVolumeChange()
	{
		if (ass.Length != 0)
		{
			for (int i = 0; i < ass.Length; i++)
			{
				ass[i].volume = DataMgr.settingData.GetFinalSound();
			}
		}
	}

	private void InputChange()
	{
		if (GameMgr.IsMobile_Static)
		{
			go_GuideImage_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage_Keyboard.SetActive(value: false);
			return;
		}
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (!go_GuideImage_Gamepad.IsDestroyed())
			{
				go_GuideImage_Gamepad.SetActive(value: false);
				go_GuideImage_Keyboard.SetActive(value: true);
			}
			break;
		case PlayerInputType.Gamepad:
			if (!go_GuideImage_Gamepad.IsDestroyed())
			{
				go_GuideImage_Gamepad.SetActive(value: true);
				go_GuideImage_Keyboard.SetActive(value: false);
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void Start()
	{
		PlayerMgr.Inst.CreatePlayer();
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.BlackEye);
		PlayerMgr.Inst.BaData.wandMaxCount = 1;
		PlayerMgr.Inst.BaData.wandCfgs = new List<WandConfig>();
		PlayerMgr.Inst.BaData.wandCfgs.Add(null);
		UIPlayerDataMgr.Inst.UpdateAllInfo();
		PlayerMgr.Inst.WandRecreate();
		PlayerMgr.Inst.WandSelect(0);
		CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
		SoundVolumeChange();
		InputChange();
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		dictionary.Add(Vector2Int.zero, RoomConfig.GetConfig(102));
		LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.None, LevelRewardType.None, fadeDisappear: true, CreateLevelFinishAct);
		pd_Story1.Stop();
		pd_Story2.Stop();
		pd_Story3.Stop();
		pd_Story4.Stop();
	}

	private void CreateLevelFinishAct()
	{
		if (ScriptableObjMgr.Inst.testCtrller.GuideSkipStory1Computer)
		{
			state = GuideMgrState.Idle;
			tsf_Story1Player.gameObject.SetActive(value: false);
			PlayerMgr.Inst.SetPlayerPoint(story1PlayerPoint);
		}
		else
		{
			ControlMgr.Inst.CursorVisibleSet(set: false);
			PlayerMgr.Inst.HideAndDisableControl();
		}
		MusicMgr.Inst.ForcePlayMusic("");
		MusicMgr.Inst.ForcePlayAmbient("Ambinet_Guide");
		anima_FogBedroom.Play("HideDirect");
		anima_FogBathroom.Play("ShowDirect");
		foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
		{
			UnityEngine.Object.Destroy(roomCtrller.Value.transform.Find("ThemeSpecialize(Clone)").gameObject);
		}
	}

	private void Update()
	{
		ettMgr.CheckInitialize(ref _isDotsInitialized);
		if (!_isDotsInitialized)
		{
			return;
		}
		if (!_isCampdestroyed)
		{
			using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(GuideCampDestroyTag));
			if (entityQuery.CalculateEntityCount() > 0)
			{
				_isCampdestroyed = true;
				NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
				NativeArray<GuideCampDestroyTag> nativeArray2 = entityQuery.ToComponentDataArray<GuideCampDestroyTag>(Allocator.Temp);
				for (int i = 0; i < nativeArray.Length; i++)
				{
					if (nativeArray2[i].isMobile != GameMgr.IsMobileCamp)
					{
						Debug.Log(nativeArray[i].Index);
						ettMgr.DestroyEntity(nativeArray[i]);
					}
				}
				nativeArray.Dispose();
				nativeArray2.Dispose();
			}
		}
		StateCheck();
		CampAndLight();
	}

	private void StateCheck()
	{
		switch (state)
		{
		case GuideMgrState.WaitEnterStory1:
			enterStory1WaitTimer += Time.deltaTime;
			if (enterStory1WaitTimer >= enterStory1WaitTime)
			{
				state = GuideMgrState.Idle;
				CamController.Inst.FocusOn(camFocusSize, enterStory1FocusTime, story1FocusPos);
				pd_Story1.Play();
			}
			break;
		case GuideMgrState.WaitFirstMove:
			if (GameMgr.IsMobile_Static && ControlMgr.Inst.GetInputWASD() != Vector2.zero && MobileMgr.inst.topui.guideMobileLeftStick.gameObject.activeSelf)
			{
				MobileMgr.inst.topui.guideMobileLeftStick.gameObject.SetActive(value: false);
			}
			break;
		case GuideMgrState.WaitEnterForestY1:
			if (LevelMgr.Inst.CurrentRoomMapPos == new Vector2Int(0, 1))
			{
				state = GuideMgrState.WaitEnterForestY2;
				StartCoroutine(EnterForestY1());
				if (ScriptableObjMgr.Inst.testCtrller.GuideStandToWand)
				{
					PlayerMgr.Inst.PlayerT.position = LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Up);
				}
			}
			break;
		case GuideMgrState.WaitEnterForestY2:
			if (LevelMgr.Inst.CurrentRoomMapPos == new Vector2Int(0, 2))
			{
				state = GuideMgrState.Finished;
				StartCoroutine(EnterForestY2());
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case GuideMgrState.Idle:
		case GuideMgrState.Finished:
			break;
		}
	}

	private IEnumerator EnterForestY1()
	{
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 2)].gameObject.SetActive(value: false);
		yield return new WaitForSeconds(story3TextID2Delay);
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(story3TextID2, PlayerMgr.Inst.PlayerT);
	}

	private IEnumerator EnterForestY2()
	{
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 1)].gameObject.SetActive(value: false);
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 2)].gameObject.SetActive(value: true);
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 2)].nms_Action.BuildNavMesh();
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 2)].nms_Ground.BuildNavMesh();
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 2)].nms_Fly.BuildNavMesh();
		yield return new WaitForSeconds(story3TextID3Delay);
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(story3TextID3, PlayerMgr.Inst.PlayerT);
	}

	private void CampAndLight()
	{
		if (!isInteractedToilet)
		{
			LevelMgr.Inst.CurrentRoomT.position = Vector3.Lerp(LevelMgr.Inst.CurrentRoomT.position, targetCamPos, 10f * Time.deltaTime);
			LevelMgr.Inst.CurrentRoomCtrller.Initialize2();
			LevelMgr.Inst.globalLight.color = Color.Lerp(LevelMgr.Inst.globalLight.color, targetGlobalLightColor, 10f * Time.deltaTime);
			if (currentRooomType == AccessTriggerGuideRoomType.Bedroom)
			{
				light_Room2RealGlobalLight.color = Color.Lerp(light_Room2RealGlobalLight.color, Color.black, 10f * Time.deltaTime);
			}
			else
			{
				light_Room2RealGlobalLight.color = Color.Lerp(light_Room2RealGlobalLight.color, initialRoom2RealGlobalLight, 10f * Time.deltaTime);
			}
		}
	}

	public void EnterRoom(AccessTriggerGuideRoomType belongRoomType)
	{
		if (belongRoomType == AccessTriggerGuideRoomType.Bedroom)
		{
			currentRooomType = AccessTriggerGuideRoomType.Bathroom;
			anima_FogBedroom.Play("Show");
			anima_FogBathroom.Play("HideDirect");
			go_BathRoomCorridorLight.SetActive(value: false);
			targetGlobalLightColor = color_Room2GlobalLightColor;
			targetCamPos = room2CampPos;
			restroomLight.SetInBathroom(inBathroom: true);
		}
		else
		{
			currentRooomType = AccessTriggerGuideRoomType.Bedroom;
			anima_FogBedroom.Play("HideDirect");
			anima_FogBathroom.Play("Show");
			go_BathRoomCorridorLight.SetActive(value: true);
			targetGlobalLightColor = color_Room1GlobalLightColor;
			targetCamPos = room1CampPos;
			restroomLight.SetInBathroom(inBathroom: false);
		}
	}

	public void UseToilet()
	{
		isInteractedToilet = true;
		if (ScriptableObjMgr.Inst.testCtrller.GuideSkipStory2Toilet)
		{
			_Story2Through();
			return;
		}
		tsf_Story2PlayerPee.gameObject.SetActive(value: true);
		PlayerMgr.Inst.HideAndDisableControl();
		pd_Story2.Play();
		ControlMgr.Inst.CursorVisibleSet(set: false);
		CamController.Inst.FocusOn(camFocusSize, camFocusTime, story2FocusPos);
		restroomLight.StopAnima();
	}

	public void EnterDoor()
	{
		StartCoroutine(EnterDoorIE());
	}

	private IEnumerator EnterDoorIE()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", PlayerMgr.Inst.PlayerPoint, 2f);
		SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
		PlayerMgr.Inst.HideAndDisableControl();
		yield return new WaitForSeconds(enterDoorWaitTime);
		UIMgr.Inst.uiFilmBlackEdge.Show(enterDoorFocusTime);
		CamController.Inst.FocusOn(enterDoorFocusSize, enterDoorFocusTime, playerShowPoint);
		yield return new WaitForSeconds(enterDoorFocusTime);
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(2, EnterGameHDFinish);
	}

	private void EnterGameHDFinish()
	{
		UIMgr.Inst.uiFade.Show(delegate
		{
			GameMgr.Inst.RecycleAllPool();
			CamController.Inst.FocusRecover(0f);
			UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
			SceneManager.LoadScene("Guide2");
		});
	}

	public void _Story1ComputerTalk(int textID)
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(textID, tsf_Story1Computer, story1ComputerMDOffset);
	}

	public void _Story1PlayerTalk(int textID)
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(textID, tsf_Story1Player, story2DialogueOffset, isYFlip: true);
	}

	public void _Story1PlayerStand()
	{
		PlayerMgr.Inst.SetPlayerPoint(story1PlayerPoint);
		PlayerMgr.Inst.ShowAndEnableControl();
		tsf_Story1Player.gameObject.SetActive(value: false);
		CamController.Inst.FocusRecover(camFocusTime);
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			ControlMgr.Inst.CursorVisibleSet(set: true);
			ControlMgr.Inst.CursorLockstateSet(CursorLockMode.None);
		}
	}

	public void _Story1GuideImage1Appear()
	{
		anima_GuideImage1.SetTrigger("Show");
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.topui.guideMobileLeftStick.gameObject.SetActive(value: true);
			state = GuideMgrState.WaitFirstMove;
		}
	}

	public void _Story2PlayerTalk(int textID)
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(textID, tsf_Story2PlayerPee.transform, story2DialogueOffset, isYFlip: true);
	}

	public void _Story2Through()
	{
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		dictionary.Add(new Vector2Int(0, 0), RoomConfig.GetConfig(104));
		dictionary.Add(new Vector2Int(0, 1), RoomConfig.GetConfig(105));
		dictionary.Add(new Vector2Int(0, 2), RoomConfig.GetConfig(103));
		UnityEngine.Object.Destroy(pd_Story1.gameObject);
		UnityEngine.Object.Destroy(pd_Story2.gameObject);
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(GuideRoom));
		NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
		ettMgr.DestroyEntity(entities);
		entities.Dispose();
		LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.None, LevelRewardType.None, fadeDisappear: true, delegate
		{
			CamController.Inst.FocusOn(camFocusSize, 0f);
			pd_Story3.Play();
		});
	}

	public void _Story3PlayerCorrect()
	{
		if (ScriptableObjMgr.Inst.testCtrller.GuideSkipStory3Pee)
		{
			state = GuideMgrState.WaitEnterForestY1;
			PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Normal);
			UnityEngine.Object.Destroy(pd_Story3.gameObject);
		}
		else
		{
			PlayerMgr.Inst.SetPlayerPoint(tsf_Story3PlayerPee.position);
			tsf_Story3Pee.gameObject.SetActive(value: true);
		}
	}

	public void _Story3Show()
	{
		PlayerMgr.Inst.ShowAndEnableControl();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Amaze);
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(story3TextID1, PlayerMgr.Inst.PlayerT);
	}

	public void _Story3PlayerFlipFalse()
	{
		PlayerMgr.Inst.PlayerPpt.SetFlip(isFlip: false);
	}

	public void _Story3PlayerFlipTrue()
	{
		PlayerMgr.Inst.PlayerPpt.SetFlip(isFlip: true);
	}

	public void _Story3PlayerRecovery()
	{
		state = GuideMgrState.WaitEnterForestY1;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Normal);
		CamController.Inst.FocusRecover(camFocusTime);
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			ControlMgr.Inst.CursorVisibleSet(set: true);
			ControlMgr.Inst.CursorLockstateSet(CursorLockMode.None);
		}
	}

	public void _Story4Focus2()
	{
		CamController.Inst.FocusOn(pickWandFocusSize2, pickWandFocusTime2);
	}

	public void _Story4Focus3()
	{
		IsPickedWand = true;
		CamController.Inst.FocusOn(pickWandFocusSize, pickWandFocusTime3);
	}

	public void _Story4CamShock()
	{
		CamController.Inst.SetShock(shockParam);
	}

	public void _Story4Finish()
	{
		UnityEngine.Object.Destroy(pd_Story4.gameObject);
		PlayerMgr.Inst.ShowAndEnableControl();
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: false);
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Amaze);
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(1, Story4HDEvent, Story4HDFinish);
		PlayerMgr.Inst.WandPickUp(WandConfig.GetConfig(1));
		PlayerMgr.Inst.WandSelect(0);
	}

	public void _Story4SetCursorPickWand()
	{
		UISetting.SetCursorWand();
	}

	private void Story4HDEvent(string eventStr)
	{
		if (eventStr == "e1")
		{
			PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Normal);
		}
		else
		{
			Debug.LogError(eventStr);
		}
	}

	private void Story4HDFinish()
	{
		StartCoroutine(Story4HDFinishIE());
	}

	private IEnumerator Story4HDFinishIE()
	{
		using EntityQuery _query = ettMgr.CreateEntityQuery(typeof(DoorCampGuide));
		Entity _doorCampGuideEtt = _query.GetSingletonEntity();
		float3 position = ettMgr.GetComponentData<LocalToWorld>(_doorCampGuideEtt).Position;
		CamController.Inst.FocusOn(pickWandFocusSize, guideDoorFocusTime, position);
		yield return new WaitForSeconds(guideDoorFocusTime + guideDoorFocusTime2);
		DoorCampGuide singleton = _query.GetSingleton<DoorCampGuide>();
		singleton.onHideMask = true;
		ettMgr.SetComponentData(_doorCampGuideEtt, singleton);
		yield return new WaitForSeconds(guideDoorOpenWaitTime);
		CamController.Inst.FocusRecover(pickWandFocusTime);
		UIMgr.Inst.uiFilmBlackEdge.Hide(pickWandFocusTime, delegate
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		});
	}
}
