using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Guide2Mgr : MonoBehaviour
{
	public enum Guide2State
	{
		WaitFirstShoot,
		WaitPickPotion,
		WaitDrinkPotion,
		WaitEnterRoomY1,
		WaitFirstShoot2,
		WaitPickSpell,
		WaitOpenBag,
		WaitDragSpell,
		WaitCloseBag,
		WaitPickCrystal,
		WaitEnterLevel2,
		WaitPickSpell2,
		WaitOpenBag2,
		WaitUseSpell2,
		WaitKillLevel2AllMonster,
		WaitFinishHD,
		WaitOpenHandBook,
		WaitFinish
	}

	[Header("Story1")]
	public PlayableDirector pd_Story1;

	public float focusSize;

	public float focusTime;

	public Vector3 focusPos;

	public int monsterID;

	public Vector3[] monsterShowPoints;

	public Animator anima_GuideImage2;

	public Animator anima_GuideImage3;

	public Animator anima_GuideImage4;

	public Animator anima_GuideImage6;

	public GameObject go_GuideImage5;

	public GameObject go_DragUI;

	public float mobileCrateChestDelayTime;

	public Vector3 chestDropPoint;

	[Header("Story1After")]
	public int potionID;

	public int monsterID2;

	public Vector3[] monster2ShowPoints;

	public UIGuideSpellDrag uiGuideSpellDrag;

	public int yellowCrystalCount;

	public float yellowCrystalRadius;

	[Header("Audio")]
	public AudioSource[] ass;

	[Header("InputChange")]
	public GameObject go_GuideImage2_Gamepad;

	public GameObject go_GuideImage2_Keyboard;

	public GameObject go_GuideImage2_Gamepad_PS;

	public GameObject go_GuideImage3_Gamepad;

	public GameObject go_GuideImage3_Keyboard;

	public GameObject go_GuideImage3_Gamepad_PS;

	public GameObject go_GuideImage4_Gamepad;

	public GameObject go_GuideImage4_Keyboard;

	public GameObject go_GuideImage4_Gamepad_PS;

	public GameObject go_GuideImage6_Gamepad;

	public GameObject go_GuideImage6_Keyboard;

	public GameObject go_GuideImage6_Gamepad_PS;

	public GameObject go_GuideDrag_Gamepad;

	public GameObject go_GuideDrag_Keyboard;

	public GameObject go_GuideDrag_Mobile;

	public UpdatButtonShow drag;

	public Sprite spriteItemHandbook;

	private MobileGuideStatus mobileGuideStatus;

	private Guide2State _state;

	private GuideStageFinish _guideStageFinishLogger;

	private float _timer;

	private float _mobileTimer;

	private bool room1FirstFinished;

	private bool enterLevel2;

	private float mobileCrateChestDelayTimer;

	private bool isCreatedLevel;

	private EntityManager ettMgr;

	private Entity doorEtt;

	private GameObject gameObjectSpriteSpellOrder;

	public Vector2Int spellOrderShowAt;

	public GameObject prefabImageSpellOrder;

	public static Guide2Mgr Inst { get; set; }

	public bool OpenedHandbook { get; set; }

	public Guide2State state
	{
		get
		{
			return _state;
		}
		private set
		{
			_state = value;
			if (GameMgr.IsMobile_Static)
			{
				bool flag = false;
				switch (value)
				{
				case Guide2State.WaitPickPotion:
					mobileGuideStatus = new MobileGuideStatus(1, "首次射击", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitDrinkPotion:
					mobileGuideStatus = new MobileGuideStatus(2, "捡起药水", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitEnterRoomY1:
					mobileGuideStatus = new MobileGuideStatus(3, "使用药水", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitFirstShoot2:
					mobileGuideStatus = new MobileGuideStatus(4, "进入第二个房间", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitPickSpell:
					flag = true;
					break;
				case Guide2State.WaitOpenBag:
					mobileGuideStatus = new MobileGuideStatus(5, "捡起毒液晶石法术", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitDragSpell:
					mobileGuideStatus = new MobileGuideStatus(6, "打开背包", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitCloseBag:
					mobileGuideStatus = new MobileGuideStatus(7, "安装毒液法术", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitPickCrystal:
					mobileGuideStatus = new MobileGuideStatus(8, "关闭背包", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitEnterLevel2:
					mobileGuideStatus = new MobileGuideStatus(9, "捡起水晶", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitPickSpell2:
					flag = true;
					break;
				case Guide2State.WaitOpenBag2:
					mobileGuideStatus = new MobileGuideStatus(10, "捡起时长强化", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitUseSpell2:
					mobileGuideStatus = new MobileGuideStatus(11, "第二次打开背包", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitKillLevel2AllMonster:
					mobileGuideStatus = new MobileGuideStatus(12, "安装时长强化", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitFinishHD:
					mobileGuideStatus = new MobileGuideStatus(13, "杀死最后一个房间全部怪物", Mathf.CeilToInt(_mobileTimer));
					break;
				case Guide2State.WaitOpenHandBook:
					flag = true;
					break;
				case Guide2State.WaitFinish:
					mobileGuideStatus = new MobileGuideStatus(14, "关闭手册（完成引导）", Mathf.CeilToInt(_mobileTimer));
					break;
				}
				if (!flag)
				{
					PluginActivity.Inst.UploadEvent("guide_flow", JsonConvert.SerializeObject(mobileGuideStatus));
					_mobileTimer = 0f;
				}
			}
			if (_guideStageFinishLogger != null)
			{
				switch (value)
				{
				case Guide2State.WaitPickSpell:
					_guideStageFinishLogger.spend_seconds = Mathf.CeilToInt(_timer);
					_guideStageFinishLogger.Report();
					_guideStageFinishLogger = new GuideStageFinish(2, LevelMgr.Inst.CurrentRoomCfg.id);
					_timer = 0f;
					break;
				case Guide2State.WaitEnterLevel2:
					_guideStageFinishLogger.spend_seconds = Mathf.CeilToInt(_timer);
					_guideStageFinishLogger.Report();
					break;
				case Guide2State.WaitPickSpell2:
					_guideStageFinishLogger = new GuideStageFinish(3, LevelMgr.Inst.CurrentRoomCfg.id);
					_timer = 0f;
					break;
				case Guide2State.WaitKillLevel2AllMonster:
					_guideStageFinishLogger.spend_seconds = Mathf.CeilToInt(_timer);
					_guideStageFinishLogger.Report();
					_guideStageFinishLogger = null;
					Debug.Log("WaitKillLevel2AllMonster-------" + _guideStageFinishLogger);
					break;
				}
			}
		}
	}

	private void Awake()
	{
		Inst = this;
		DataMgr.selectedWorldData.inBattle9 = false;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
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
		MobileMgr.inst.topui.HideAllGuide();
	}

	private void SoundVolumeChange()
	{
		if (ass != null && ass.Length != 0)
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
			anima_GuideImage2.gameObject.SetActive(MobileMgr.inst.gamepadPlugged);
			anima_GuideImage3.gameObject.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage2_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage3_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage4_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage6_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			go_GuideImage2_Keyboard.SetActive(value: false);
			go_GuideImage3_Keyboard.SetActive(value: false);
			go_GuideImage4_Keyboard.SetActive(value: false);
			go_GuideImage6_Keyboard.SetActive(value: false);
			go_GuideImage2_Gamepad_PS.SetActive(value: false);
			go_GuideImage3_Gamepad_PS.SetActive(value: false);
			go_GuideImage4_Gamepad_PS.SetActive(value: false);
			go_GuideImage6_Gamepad_PS.SetActive(value: false);
			uiGuideSpellDrag.UpdatePlatform();
			return;
		}
		drag?.UpdateButton();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			go_GuideImage2_Gamepad.SetActive(value: false);
			go_GuideImage3_Gamepad.SetActive(value: false);
			go_GuideImage4_Gamepad.SetActive(value: false);
			go_GuideImage6_Gamepad.SetActive(value: false);
			go_GuideImage2_Keyboard.SetActive(value: true);
			go_GuideImage3_Keyboard.SetActive(value: true);
			go_GuideImage4_Keyboard.SetActive(value: true);
			go_GuideImage6_Keyboard.SetActive(value: true);
			go_GuideImage2_Gamepad_PS.SetActive(value: false);
			go_GuideImage3_Gamepad_PS.SetActive(value: false);
			go_GuideImage4_Gamepad_PS.SetActive(value: false);
			go_GuideImage6_Gamepad_PS.SetActive(value: false);
			uiGuideSpellDrag.UpdatePlatform();
			break;
		case PlayerInputType.Gamepad:
			go_GuideImage2_Gamepad.SetActive(value: true);
			go_GuideImage3_Gamepad.SetActive(value: true);
			go_GuideImage4_Gamepad.SetActive(value: true);
			go_GuideImage6_Gamepad.SetActive(value: true);
			go_GuideImage2_Keyboard.SetActive(value: false);
			go_GuideImage3_Keyboard.SetActive(value: false);
			go_GuideImage4_Keyboard.SetActive(value: false);
			go_GuideImage6_Keyboard.SetActive(value: false);
			if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
			{
				go_GuideImage2_Gamepad.SetActive(value: false);
				go_GuideImage3_Gamepad.SetActive(value: false);
				go_GuideImage4_Gamepad.SetActive(value: false);
				go_GuideImage6_Gamepad.SetActive(value: false);
				go_GuideImage2_Keyboard.SetActive(value: false);
				go_GuideImage3_Keyboard.SetActive(value: false);
				go_GuideImage4_Keyboard.SetActive(value: false);
				go_GuideImage6_Keyboard.SetActive(value: false);
				go_GuideImage2_Gamepad_PS.SetActive(value: true);
				go_GuideImage3_Gamepad_PS.SetActive(value: true);
				go_GuideImage4_Gamepad_PS.SetActive(value: true);
				go_GuideImage6_Gamepad_PS.SetActive(value: true);
			}
			else
			{
				go_GuideImage2_Gamepad.SetActive(value: true);
				go_GuideImage3_Gamepad.SetActive(value: true);
				go_GuideImage4_Gamepad.SetActive(value: true);
				go_GuideImage6_Gamepad.SetActive(value: true);
				go_GuideImage2_Keyboard.SetActive(value: false);
				go_GuideImage3_Keyboard.SetActive(value: false);
				go_GuideImage4_Keyboard.SetActive(value: false);
				go_GuideImage6_Keyboard.SetActive(value: false);
				go_GuideImage2_Gamepad_PS.SetActive(value: false);
				go_GuideImage3_Gamepad_PS.SetActive(value: false);
				go_GuideImage4_Gamepad_PS.SetActive(value: false);
				go_GuideImage6_Gamepad_PS.SetActive(value: false);
			}
			uiGuideSpellDrag.UpdatePlatform();
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void Start()
	{
		pd_Story1.Stop();
	}

	private void Update()
	{
		StartHandle();
		StateCheck();
	}

	private void StartHandle()
	{
		if (isCreatedLevel)
		{
			return;
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Guide2Ett));
		if (entityQuery.CalculateEntityCount() != 0)
		{
			isCreatedLevel = true;
			PlayerMgr.Inst.CreatePlayer();
			UIPlayerDataMgr.Inst.UpdateAllInfo();
			PlayerMgr.Inst.BaData.wandCfgs = new List<WandConfig> { WandConfig.GetConfig(1) };
			PlayerMgr.Inst.WandRecreate();
			PlayerMgr.Inst.WandSelect(0);
			CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
			UIPlayerDataMgr.Inst.UpdateAllInfo();
			PlayerMgr.Inst.WandRecreate();
			Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
			dictionary.Add(Vector2Int.zero, RoomConfig.GetConfig(106));
			RoomConfig config = RoomConfig.GetConfig(107);
			config.isFinalRoom = true;
			dictionary.Add(Vector2Int.up, config);
			LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.Relic, LevelRewardType.None, fadeDisappear: true, CreateLevelFinishAct);
			SoundVolumeChange();
			InputChange();
			if (GameMgr.IsMobile_Static)
			{
				UIPlayerDataMgr.Inst.goHandBookButton.SetActive(value: false);
			}
		}
	}

	private void CreateLevelFinishAct()
	{
		PlayerMgr.Inst.SetPlayerPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: false);
		PlayerMgr.Inst.HideAndDisableControl();
		UIMgr.Inst.uiFilmBlackEdge.Show(0f);
		CamController.Inst.FocusOn(focusSize, 0f, focusPos);
		pd_Story1.Play();
		ettMgr.DestroyEntity(LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 1)].doorEttList[0]);
		LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 1)].doorEttList.Clear();
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Guide2Ett));
		Guide2Ett singleton = entityQuery.GetSingleton<Guide2Ett>();
		doorEtt = ettMgr.Instantiate(singleton.ett_Door_T3_Guide);
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(doorEtt);
		componentData.Position = LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 1)].GetAccessPoint(FourDir.Up) + new Vector3(0.5f, 1f, 0f);
		ettMgr.SetComponentData(doorEtt, componentData);
		DoorBase_Dots componentData2 = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
		componentData2.rewardType = LevelRewardType.None;
		ettMgr.SetComponentData(doorEtt, componentData2);
		if (ScriptableObjMgr.Inst.testCtrller.Guide2SkipDialogue1)
		{
			UnityEngine.Object.Destroy(pd_Story1.gameObject);
			OnRoom0Finish(PlayerMgr.Inst.PlayerPoint);
			PlayerMgr.Inst.ShowAndEnableControl();
			CamController.Inst.FocusRecover(0f);
			UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
		}
	}

	private void StateCheck()
	{
		if (!isCreatedLevel)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			_mobileTimer += Time.unscaledDeltaTime;
		}
		if (_guideStageFinishLogger != null)
		{
			_timer += Time.deltaTime;
		}
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt) && playerPpt.unitCfg.currentHP < playerPpt.unitCfg.maxHP)
		{
			playerPpt.unitCfg.currentHP = playerPpt.unitCfg.maxHP;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
		}
		switch (state)
		{
		case Guide2State.WaitFirstShoot:
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.topui.guideMobileRightStick.gameObject.SetActive(value: true);
				if (PlayerMgr.Inst.PlayerCtrller.isHoldMouse0)
				{
					if (GameMgr.IsMobile_Static)
					{
						MobileMgr.inst.topui.guideMobileRightStick.gameObject.SetActive(value: false);
					}
					state = Guide2State.WaitPickPotion;
				}
				if (PlayerMgr.Inst.BaData.potionIDs[0] == potionID)
				{
					MobileMgr.inst.topui.guideMobileRightStick.gameObject.SetActive(value: false);
					MobileMgr.inst.topui.guideMobileDrink.gameObject.SetActive(value: true);
					state = Guide2State.WaitDrinkPotion;
					UIPlayerDataMgr.Inst.PotionShow();
					anima_GuideImage3.SetTrigger("Show");
				}
			}
			else
			{
				state = Guide2State.WaitPickPotion;
			}
			break;
		case Guide2State.WaitPickPotion:
			if (PlayerMgr.Inst.BaData.potionIDs[0] == potionID)
			{
				_guideStageFinishLogger = new GuideStageFinish(1, LevelMgr.Inst.CurrentRoomCfg.id);
				_timer = 0f;
				Debug.Log("WaitPickPotion-------" + _guideStageFinishLogger);
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileDrink.gameObject.SetActive(value: true);
				}
				state = Guide2State.WaitDrinkPotion;
				UIPlayerDataMgr.Inst.PotionShow();
				anima_GuideImage3.SetTrigger("Show");
			}
			break;
		case Guide2State.WaitDrinkPotion:
			if (PlayerMgr.Inst.GetPotionNum() == 0)
			{
				state = Guide2State.WaitEnterRoomY1;
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileDrink.gameObject.SetActive(value: false);
				}
			}
			break;
		case Guide2State.WaitEnterRoomY1:
			if (!(LevelMgr.Inst.CurrentRoomMapPos == new Vector2Int(0, 1)))
			{
				break;
			}
			state = Guide2State.WaitFirstShoot2;
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.topui.guideMobileRightStick.gameObject.SetActive(value: true);
				MobileMgr.inst.topui.guideMobileRightStick.textHint.enabled = false;
			}
			LevelMgr.Inst.CurrentRoomCtrller.RoomFinishRegister(OnRoom1Finish);
			{
				foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
				{
					roomCtrller.Value.SetWhenFinishOpenDoorAndAccess(isOpen: false);
				}
				break;
			}
		case Guide2State.WaitFirstShoot2:
			if (LevelMgr.Inst.CurrentRoomMapPos == new Vector2Int(0, 1))
			{
				state = Guide2State.WaitPickSpell;
			}
			break;
		case Guide2State.WaitPickSpell:
			if (PlayerMgr.Inst.PlayerCtrller.isHoldMouse0 && GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.topui.guideMobileRightStick.gameObject.SetActive(value: false);
				MobileMgr.inst.topui.guideMobileRightStick.textHint.enabled = true;
			}
			if (PlayerMgr.Inst.BaData.bagSpellDatas[0] != null && PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 30051)
			{
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameObject.SetActive(!MobileMgr.inst.gamepadPlugged);
					anima_GuideImage4.gameObject.SetActive(MobileMgr.inst.gamepadPlugged);
				}
				state = Guide2State.WaitOpenBag;
				anima_GuideImage4.SetTrigger("Show");
				if (UIPlayerDataMgr.Inst.IsBagOpen)
				{
					UIPlayerDataMgr.Inst.BagOpenOrClose();
				}
			}
			break;
		case Guide2State.WaitOpenBag:
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: false);
					MobileMgr.inst.topui.guideMobileBag.textHint.enabled = false;
				}
				state = Guide2State.WaitDragSpell;
				uiGuideSpellDrag.gameObject.SetActive(value: true);
				uiGuideSpellDrag.StartAnima();
			}
			break;
		case Guide2State.WaitDragSpell:
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				uiGuideSpellDrag.gameObject.SetActive(value: true);
				if (PlayerMgr.Inst.BaData.wandCfgs[0] == null)
				{
					break;
				}
				for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots.Length; j++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots[j] == null || PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots[j].id != 30051)
					{
						continue;
					}
					if (GameMgr.IsMobile_Static)
					{
						state = Guide2State.WaitCloseBag;
						uiGuideSpellDrag.gameObject.SetActive(value: false);
						MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: true);
						continue;
					}
					state = Guide2State.WaitPickCrystal;
					uiGuideSpellDrag.gameObject.SetActive(value: false);
					LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
					for (int k = 0; k < monster2ShowPoints.Length; k++)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + monster2ShowPoints[k]).GetComponent<MonsterBorn>().Initialize(LevelMgr.Inst.CurrentRoomCtrller, new RoomObjData(RoomObjType.Unit, monsterID2), 0f, immediatelyCreate: false, isDoubleEnemy: false);
					}
					foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller2 in LevelMgr.Inst.RoomCtrllers)
					{
						roomCtrller2.Value.SetWhenFinishOpenDoorAndAccess(isOpen: true);
					}
				}
			}
			else
			{
				uiGuideSpellDrag.gameObject.SetActive(value: false);
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: true);
				}
			}
			break;
		case Guide2State.WaitCloseBag:
		{
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: true);
				}
				break;
			}
			state = Guide2State.WaitPickCrystal;
			uiGuideSpellDrag.gameObject.SetActive(value: false);
			LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
			for (int m = 0; m < monster2ShowPoints.Length; m++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + monster2ShowPoints[m]).GetComponent<MonsterBorn>().Initialize(LevelMgr.Inst.CurrentRoomCtrller, new RoomObjData(RoomObjType.Unit, monsterID2), 0f, immediatelyCreate: false, isDoubleEnemy: false);
			}
			{
				foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller3 in LevelMgr.Inst.RoomCtrllers)
				{
					roomCtrller3.Value.SetWhenFinishOpenDoorAndAccess(isOpen: true);
				}
				break;
			}
		}
		case Guide2State.WaitPickCrystal:
			uiGuideSpellDrag.gameObject.SetActive(value: false);
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: false);
			}
			if (DataMgr.selectedWorldData.magicCrystalCount == yellowCrystalCount)
			{
				state = Guide2State.WaitEnterLevel2;
				DoorBase_Dots componentData3 = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
				componentData3.onOpen = true;
				ettMgr.SetComponentData(doorEtt, componentData3);
			}
			else if (DataMgr.selectedWorldData.magicCrystalCount > yellowCrystalCount)
			{
				MonoBehaviour.print("黄水晶大于" + yellowCrystalCount + ",估计存档没清");
				state = Guide2State.WaitEnterLevel2;
				DoorBase_Dots componentData4 = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
				componentData4.onOpen = true;
				ettMgr.SetComponentData(doorEtt, componentData4);
			}
			break;
		case Guide2State.WaitPickSpell2:
		{
			for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.bagSpellDatas[0] != null && (PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 30121 || PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 31031))
				{
					if (GameMgr.IsMobile_Static)
					{
						state = Guide2State.WaitOpenBag2;
						break;
					}
					state = Guide2State.WaitUseSpell2;
					anima_GuideImage6.SetTrigger("Show");
					uiGuideSpellDrag.StartAnima(isMove2: true);
					break;
				}
			}
			break;
		}
		case Guide2State.WaitOpenBag2:
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				state = Guide2State.WaitUseSpell2;
				uiGuideSpellDrag.StartAnima(isMove2: true);
				uiGuideSpellDrag.gameObject.SetActive(value: true);
				MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: false);
				MobileMgr.inst.topui.guideMobileBag.textHint.enabled = false;
			}
			else
			{
				uiGuideSpellDrag.gameObject.SetActive(value: false);
				MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: true);
				MobileMgr.inst.topui.guideMobileBag.textHint.enabled = true;
			}
			break;
		case Guide2State.WaitUseSpell2:
		{
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots.Length; l++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots[l] == null || (PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots[l].id != 30121 && PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots[l].id != 31031))
				{
					continue;
				}
				state = Guide2State.WaitKillLevel2AllMonster;
				uiGuideSpellDrag.gameObject.SetActive(value: false);
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: false);
				}
				if (LevelMgr.Inst.CurrentRoomCfg.id != 108)
				{
					return;
				}
				if (GameMgr.IsMobile_Static && !MobileMgr.inst.gamepadPlugged)
				{
					go_DragUI = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UI/UIMobile/TutorialDragMobile"), UIMgr.Inst.rtsf_Canvas2, worldPositionStays: false);
					go_DragUI.transform.position = TopUI.inst.guideMobileRightStick.transform.position;
					return;
				}
				Transform transform = go_GuideImage5.transform.Find("Layer")?.transform.Find("Canvas");
				if ((bool)transform)
				{
					transform.gameObject.SetActive(value: false);
				}
				go_GuideImage5.SetActive(value: true);
				return;
			}
			if (UIPlayerDataMgr.Inst.IsBagOpen)
			{
				uiGuideSpellDrag.gameObject.SetActive(value: true);
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: false);
				}
				break;
			}
			uiGuideSpellDrag.gameObject.SetActive(value: false);
			if (GameMgr.IsMobile_Static)
			{
				state = Guide2State.WaitOpenBag2;
				MobileMgr.inst.topui.guideMobileBag.gameobjectOrbit.SetActive(value: true);
			}
			break;
		}
		case Guide2State.WaitKillLevel2AllMonster:
			if (PlayerMgr.Inst.PlayerCtrller.isHoldMouse0 && GameMgr.IsMobile_Static && go_DragUI != null && go_DragUI.activeInHierarchy)
			{
				go_DragUI.SetActive(value: false);
				go_GuideImage5.SetActive(value: true);
			}
			if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count != 0)
			{
				break;
			}
			UnityEngine.Object.Destroy(go_GuideImage5);
			if (go_DragUI != null)
			{
				UnityEngine.Object.Destroy(go_DragUI);
			}
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			CamController.Inst.FocusOn(focusSize, focusTime);
			UIPlayerDataMgr.Inst.Hide();
			UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
			{
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(4, (Action)delegate
				{
					CamController.Inst.FocusRecover(focusTime);
					UIMgr.Inst.uiFilmBlackEdge.Hide(focusTime, delegate
					{
						PlayerMgr.Inst.PlayerCtrller.StartMotion();
						UIPlayerDataMgr.Inst.WandShow();
					});
				});
			});
			if (GameMgr.IsMobile_Static)
			{
				state = Guide2State.WaitFinishHD;
			}
			else
			{
				state = Guide2State.WaitFinishHD;
			}
			break;
		case Guide2State.WaitFinishHD:
		{
			(int, int, UIDialogueMgr.HDState) currentHdInfo = GameUISingletonMono<UIDialogueMgr>.Inst.GetCurrentHdInfo();
			if (currentHdInfo.Item1 == spellOrderShowAt.x && currentHdInfo.Item2 == spellOrderShowAt.y && gameObjectSpriteSpellOrder == null)
			{
				gameObjectSpriteSpellOrder = UnityEngine.Object.Instantiate(prefabImageSpellOrder, UIGuideMgr.Inst.canvas.transform);
				gameObjectSpriteSpellOrder.GetComponent<Image>().DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
			}
			else if (currentHdInfo.Item1 == spellOrderShowAt.x && currentHdInfo.Item3 == UIDialogueMgr.HDState.Hide && gameObjectSpriteSpellOrder != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObjectSpriteSpellOrder);
				if (GameMgr.IsMobile_Static)
				{
					state = Guide2State.WaitOpenHandBook;
					UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/LoadEnterScene/Entry/TutorialHandBookMobile"), UIMgr.Inst.rtsf_Canvas2, worldPositionStays: false);
					break;
				}
				DoorBase_Dots componentData2 = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
				componentData2.onOpen = true;
				ettMgr.SetComponentData(doorEtt, componentData2);
				state = Guide2State.WaitFinish;
				UIPlayerDataMgr.Inst.healthTip.SetActive(value: false);
				DataMgr.selectedWorldData.isTriggerTutorialHpShow = true;
			}
			break;
		}
		case Guide2State.WaitOpenHandBook:
			if (OpenedHandbook)
			{
				DoorBase_Dots componentData = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
				componentData.onOpen = true;
				ettMgr.SetComponentData(doorEtt, componentData);
				UIPlayerDataMgr.Inst.goHandBookGuideParticle.gameObject.SetActive(value: false);
				state = Guide2State.WaitFinish;
				UIPlayerDataMgr.Inst.healthTip.SetActive(value: false);
				DataMgr.selectedWorldData.isTriggerTutorialHpShow = true;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case Guide2State.WaitEnterLevel2:
		case Guide2State.WaitFinish:
			break;
		}
	}

	private void OnRoom0Finish(Vector3 lastMonsterPoint)
	{
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, potionID), lastMonsterPoint);
	}

	private void OnRoom1Finish(Vector3 lastMonsterPoint)
	{
		StartCoroutine(OnRoom1FinishIE(lastMonsterPoint));
	}

	private IEnumerator OnRoom1FinishIE(Vector3 lastMonsterPoint)
	{
		if (!room1FirstFinished)
		{
			room1FirstFinished = true;
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, 30051), lastMonsterPoint);
			yield return null;
			LevelMgr.Inst.CurrentRoomCtrller.AllAccessCloseDirect();
			yield break;
		}
		List<ItemInfo> list = new List<ItemInfo>();
		for (int i = 0; i < yellowCrystalCount; i++)
		{
			list.Add(new ItemInfo(ItemType.Resource, 101));
		}
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, DTool.ListToBlobArray(list), lastMonsterPoint, yellowCrystalRadius);
		if (ScriptableObjMgr.Inst.testCtrller.Guide2SkipDialogue2)
		{
			yield break;
		}
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.FocusOn(focusSize, focusTime);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(3, (Action)delegate
			{
				CamController.Inst.FocusRecover(focusTime);
				UIMgr.Inst.uiFilmBlackEdge.Hide(focusTime, delegate
				{
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
				});
			});
		});
	}

	public Guide2State GetGuide2State()
	{
		return state;
	}

	public void PlayerEnterDoor()
	{
		if (!enterLevel2)
		{
			enterLevel2 = true;
			anima_GuideImage2.transform.GetChild(0).gameObject.SetActive(value: false);
			anima_GuideImage3.transform.GetChild(0).gameObject.SetActive(value: false);
			if (PlayerMgr.Inst.ItemCtrller.potion_HoverEFGO != null)
			{
				UnityEngine.Object.Destroy(PlayerMgr.Inst.ItemCtrller.potion_HoverEFGO);
				PlayerMgr.Inst.FlyUnregister();
			}
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			UIMgr.Inst.uiFade.Show(delegate
			{
				PlayerMgr.Inst.FlyRegisterWithAllMate();
				PlayerMgr.Inst.AllWandFullMP();
				GameMgr.Inst.RecycleAllPool();
				foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
				{
					roomCtrller.Value.RoomRecyeleDelegateExecute();
				}
				Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
				RoomConfig config = RoomConfig.GetConfig(108);
				config.isFinalRoom = true;
				dictionary.Add(Vector2Int.zero, config);
				LevelMgr.Inst.CreateLevel(dictionary, LevelRewardType.None, LevelRewardType.Relic, LevelRewardType.None, fadeDisappear: true, delegate
				{
					PlayerMgr.Inst.PlayerCtrller.StartMotion();
					PlayerMgr.Inst.FlyUnregisterWithAllMate();
					ettMgr.DestroyEntity(LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 0)].doorEttList[0]);
					LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 0)].doorEttList.Clear();
					using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Guide2Ett));
					Guide2Ett singleton = entityQuery.GetSingleton<Guide2Ett>();
					doorEtt = ettMgr.Instantiate(singleton.ett_Door_T3_Guide);
					LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(doorEtt);
					componentData.Position = LevelMgr.Inst.RoomCtrllers[new Vector2Int(0, 0)].GetAccessPoint(FourDir.Up) + new Vector3(0.5f, 1f, 0f);
					ettMgr.SetComponentData(doorEtt, componentData);
					DoorBase_Dots componentData2 = ettMgr.GetComponentData<DoorBase_Dots>(doorEtt);
					componentData2.rewardType = LevelRewardType.Relic;
					ettMgr.SetComponentData(doorEtt, componentData2);
					state = Guide2State.WaitPickSpell2;
				});
			});
		}
		else
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			UIMgr.Inst.uiFade.Show(delegate
			{
				GameMgr.Inst.RecycleAllPool();
				DataMgr.selectedWorldData.timeuse = 1f;
				SceneManager.LoadScene("Battle");
			});
		}
	}

	public void OnExitGuide2ToMainMenu()
	{
		Debug.Log("OnExitGuide2ToMainMenu");
		UIPlayerDataMgr.Inst.anima_PlayerInfo.Play("HideDirect");
		UIPlayerDataMgr.Inst.ForceBagClose();
		if ((bool)uiGuideSpellDrag)
		{
			uiGuideSpellDrag.gameObject.SetActive(value: false);
		}
	}

	public void _Story1PlayerShow()
	{
		PlayerMgr.Inst.ShowAndEnableControl();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
		ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", PlayerMgr.Inst.PlayerPoint, 2f);
	}

	public void _Story1PlayerFlipTrue()
	{
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: true);
	}

	public void _Story1PlayerFlipFalse()
	{
		PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: false);
	}

	public void _Story1Finish()
	{
		CamController.Inst.FocusRecover(focusTime);
		UIMgr.Inst.uiFilmBlackEdge.Hide(focusTime, delegate
		{
			LevelMgr.Inst.CurrentRoomCtrller.MaskNoFinish();
			LevelMgr.Inst.CurrentRoomCtrller.RoomFinishRegister(OnRoom0Finish);
			LevelMgr.Inst.CurrentRoomCtrller.AllAccessClose();
			for (int i = 0; i < monsterShowPoints.Length; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterBorn", LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + monsterShowPoints[i]).GetComponent<MonsterBorn>().Initialize(LevelMgr.Inst.CurrentRoomCtrller, new RoomObjData(RoomObjType.Unit, monsterID), 0f, immediatelyCreate: false, isDoubleEnemy: false);
			}
			anima_GuideImage2.SetTrigger("Show");
			UIPlaceNameMgr.Inst.Show(PlaceNameType.Chapter1);
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		});
	}
}
