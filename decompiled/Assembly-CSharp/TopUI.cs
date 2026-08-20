using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
	public GameObject testButton;

	public static TopUI inst;

	public Text showDistance;

	public Canvas canvas;

	public List<GameObject> VirtualStick = new List<GameObject>();

	public List<OnScreenStickCustom> VirtualStickComponent = new List<OnScreenStickCustom>();

	public OnScreenStickCustom VirtualStickRight;

	public List<VirtualStickSizeAdjust> AllVirtualStickAdjusts = new List<VirtualStickSizeAdjust>();

	public GameObject rightStickBG;

	private Vector3 rightStickDefaultPos;

	[HideInInspector]
	public VirtualStickSizeAdjust currentVirtualStickSizeAdjust;

	public List<GameObject> disableOnAdjust;

	public GameObject topUIMobile;

	public List<GameObject> Mobileenable;

	public List<GameObject> Mobiledisable;

	public Image activeSkillImage;

	public Image aimActiveSkillImage;

	public Image skillCDImage;

	public Image skillCDImageWithDir;

	public GameObject aimSkillObj1;

	public GameObject aimSkillObj2;

	public UI_AimSkill uI_AimSkill;

	public Image potionImage;

	public Image wandImage;

	public Sprite wandDefultSprite;

	public Sprite potionSlotEmpty;

	public Sprite potionSlotFull;

	public Sprite potionSlotSelected;

	public GameObject postionDots;

	public Transform postionDotsRoot;

	public Transform wandDotsRoot;

	private int potionConfigHash;

	private int wandConfigHash;

	public Sprite[] skillSprites;

	public Animator killSummonButtonObj;

	public Animator damageInfoButtonObj;

	[Header("TestButton")]
	public Toggle ToggleMoveLerp;

	public List<GameObject> CloesdRoomController;

	public GameObject goRightStick;

	public GameObject goIndiActiveButton;

	public Animator ControlAnimator;

	public Image attackImage;

	public Image interactImage;

	[FormerlySerializedAs("interactOther")]
	public Sprite spriteOther;

	[FormerlySerializedAs("interactTalk")]
	public Sprite spriteTalk;

	[FormerlySerializedAs("interactAttack")]
	public Sprite spriteAttack;

	public Sprite spriteEnderDoor;

	public Animator Button_Drink_Animator;

	[FormerlySerializedAs("Button_ChangeWandAnimator")]
	public Animator Button_SwitchWandAnimator;

	public CustomStickArea[] customStickAreas;

	public Animator Button_ActiveSkill_Animator;

	public Button skillButton;

	public Image skillEfImage;

	public Image aimSkillEfImage;

	public Text skillCount;

	public Text aimSkillCount;

	public Dropdown ObjectGroup;

	public GameObject MouseCursor;

	[Header("教学引导")]
	public UIBagParticleOrbit guideMobileDrink;

	public UIBagParticleOrbit guideMobileRightStick;

	public UIBagParticleOrbit guideMobileLeftStick;

	public RectTransform rectPotionCenter;

	public RectTransform rectWandCenter;

	public UIPotionSelectPopOut uiPotionSelectPopOut;

	public GameObject mobilePotionDragTutorial;

	public GameObject mobileWandDragTutorial;

	private bool lastCancelSummonState;

	private bool lastDamageRecordState;

	private int preSelectedWandID;

	private List<int> currentPotionID = new List<int>();

	public GameObject goMenuButton => UIPlayerDataMgr.Inst.goMenuButton;

	public UIBagParticleOrbit guideMobileBag => UIPlayerDataMgr.Inst.guideMobileBag;

	public UIBagParticleOrbit guideMobileHandbook => UIPlayerDataMgr.Inst.guideMobileHandBook;

	public bool adjusting { get; set; }

	private void Awake()
	{
		if (inst == null)
		{
			inst = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		foreach (GameObject item in Mobileenable)
		{
			item.SetActive(!GameMgr.IsSteamDeck_Static && GameMgr.IsMobile_Static);
		}
		foreach (GameObject item2 in Mobiledisable)
		{
			item2.SetActive(GameMgr.IsSteamDeck_Static || !GameMgr.IsMobile_Static);
		}
		showDistance.text = CamController.Inst.FocusCamSizeRatio.ToString();
		Initialize();
		AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			x.SetDefaultLocalPosition();
		});
		UIMgr.Inst.uiSetting.UseCustomAdjust(onStart: true);
	}

	public void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (!adjusting)
			{
				UpdateResearchButtons();
			}
			UpdateMobileWandUI();
		}
	}

	private void UpdateResearchButtons(bool forceUpdate = false)
	{
		bool flag = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.CancelSummon) != 0;
		bool flag2 = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.DamageRecordBoard) != 0;
		if (flag != lastCancelSummonState || forceUpdate)
		{
			killSummonButtonObj.SetBool("Show", flag);
			lastCancelSummonState = flag;
		}
		if (flag2 != lastDamageRecordState || forceUpdate)
		{
			damageInfoButtonObj.SetBool("Show", flag2);
			lastDamageRecordState = flag2;
		}
	}

	public void UpdateMobilePositionUI()
	{
		List<int> potionIDs = PlayerMgr.Inst.BaData.potionIDs;
		int num = ComputeHash(potionIDs, PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex);
		if (potionConfigHash != num)
		{
			potionConfigHash = num;
			if (PlayerMgr.Inst.ItemCtrller.SelectedPotionID > 0)
			{
				potionImage.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[PlayerMgr.Inst.ItemCtrller.SelectedPotionID].GetIconPath());
			}
			UpdateDotsSurroundUI(postionDotsRoot, potionIDs, new List<int> { 0 }, PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex);
		}
	}

	public static int ComputeHash(List<int> ids, int selectedIndex)
	{
		int num = 17;
		if (ids != null)
		{
			foreach (int id in ids)
			{
				num = num * 31 + id;
			}
		}
		return num * 31 + selectedIndex;
	}

	public void UpdateMobileWandUI()
	{
		if (!PlayerMgr.Inst)
		{
			return;
		}
		UpdateSelectedWand();
		if (DataMgr.selectedWorldData.battleData9 == null)
		{
			return;
		}
		currentPotionID.Clear();
		foreach (WandConfig wandCfg in DataMgr.selectedWorldData.battleData9.wandCfgs)
		{
			currentPotionID.Add(wandCfg?.id ?? 0);
		}
		int num = ComputeHash(currentPotionID, PlayerMgr.Inst.SelectedWandIndex);
		if (wandConfigHash != num)
		{
			wandConfigHash = num;
			UpdateDotsSurroundUI(wandDotsRoot, currentPotionID, new List<int> { 0 }, PlayerMgr.Inst.SelectedWandIndex);
		}
	}

	public void UpdateSelectedWand()
	{
		if (PlayerMgr.Inst.SelectedWand != null && PlayerMgr.Inst.SelectedWandCfg != null)
		{
			int id = PlayerMgr.Inst.SelectedWandCfg.id;
			if (preSelectedWandID != id || !(wandImage.sprite != wandDefultSprite))
			{
				wandImage.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[PlayerMgr.Inst.SelectedWandCfg.id].GetIconPath());
				preSelectedWandID = id;
			}
		}
		else
		{
			wandImage.sprite = wandDefultSprite;
		}
	}

	public void SetSelectedWandDirty()
	{
		preSelectedWandID = -1;
	}

	private void UpdateDotsSurroundUI(Transform dotsRoot, List<int> ids, List<int> emptyID, int selectedIndex)
	{
		int num = 90;
		int num2 = 20;
		int num3 = 65;
		dotsRoot.DestroyAllChildImmediate();
		if (ids.Count == 1)
		{
			return;
		}
		for (int i = 0; i < ids.Count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(postionDots, dotsRoot);
			obj.SetActive(value: true);
			Image component = obj.GetComponent<Image>();
			int num4 = num + num2 * i;
			obj.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)num3 * Mathf.Cos((float)num4 * (MathF.PI / 180f)), (float)num3 * Mathf.Sin((float)num4 * (MathF.PI / 180f)));
			if (!emptyID.Contains(ids[i]))
			{
				component.sprite = ((i == selectedIndex) ? potionSlotSelected : potionSlotFull);
			}
			else
			{
				component.sprite = potionSlotEmpty;
			}
		}
	}

	public void Initialize()
	{
		UpdateLanguage();
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(UpdateLanguage));
	}

	public void OnDisable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(UpdateLanguage));
	}

	public void UpdateLanguage()
	{
		if (GameMgr.IsMobile_Static)
		{
			guideMobileBag.textHint.text = 1004003.GetText();
			guideMobileDrink.textHint.text = 1004002.GetText().Replace("\\n", "\n");
			guideMobileRightStick.textHint.text = 1004001.GetText();
		}
	}

	public void AdjustStart()
	{
		adjusting = true;
		AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			x.gameObject.SetActive(value: true);
		});
		AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			x.EndAdjusting();
		});
		disableOnAdjust.ForEach(delegate(GameObject x)
		{
			x.SetActive(value: false);
		});
		killSummonButtonObj.SetBool("Show", value: true);
		damageInfoButtonObj.SetBool("Show", value: true);
		goIndiActiveButton.SetActive(DataMgr.settingData.Mobiledata.indieInteractButton);
	}

	public void AdjustEnd()
	{
		adjusting = false;
		AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			x.gameObject.SetActive(value: false);
		});
		disableOnAdjust.ForEach(delegate(GameObject x)
		{
			x.SetActive(value: true);
		});
		UpdateResearchButtons(forceUpdate: true);
	}

	public void HideAllGuide()
	{
		if (guideMobileBag != null)
		{
			guideMobileBag.gameObject.SetActive(value: false);
		}
		if (guideMobileDrink != null)
		{
			guideMobileDrink.gameObject.SetActive(value: false);
		}
		if (guideMobileRightStick != null)
		{
			guideMobileRightStick.gameObject.SetActive(value: false);
		}
		if (guideMobileLeftStick != null)
		{
			guideMobileLeftStick.gameObject.SetActive(value: false);
		}
		if (guideMobileHandbook != null)
		{
			guideMobileHandbook.gameObject.SetActive(value: false);
		}
	}

	public void OpenClosePlayerController(bool b)
	{
		if (!(Time.timeSinceLevelLoad < 0.5f))
		{
			GameMgr.Inst.playerMgr.PlayerCtrller.gameObject.SetActive(b);
		}
	}

	public void OpenClosePlayerRigidbody(int i = 0)
	{
		switch (i)
		{
		case 0:
			PlayerMgr.Inst.PlayerGO.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
			break;
		case 1:
			PlayerMgr.Inst.PlayerGO.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
			break;
		case 2:
			PlayerMgr.Inst.PlayerGO.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			break;
		case 3:
			PlayerMgr.Inst.PlayerGO.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			break;
		}
	}

	public void PlayerLightQuality(int i)
	{
	}

	public void LowFixDeltaTime(bool b)
	{
		if (b)
		{
			TimeScaleMgr.Inst.OverrideFixDeltaTime(0.04f);
		}
		else
		{
			TimeScaleMgr.Inst.OverrideFixDeltaTime(0.02f);
		}
	}

	public void ToogleRoomControler(bool b)
	{
		if (Time.timeSinceLevelLoad < 0.5f)
		{
			return;
		}
		if (!b)
		{
			if (GameObject.Find("RoomController(Clone)").gameObject != null)
			{
				do
				{
					Debug.Log("Find");
					CloesdRoomController.Add(GameObject.Find("RoomController(Clone)").gameObject);
					GameObject.Find("RoomController(Clone)").gameObject.SetActive(value: false);
				}
				while (GameObject.Find("RoomController(Clone)").gameObject != null);
			}
			return;
		}
		Debug.Log("Clear");
		foreach (GameObject item in CloesdRoomController)
		{
			item.SetActive(value: true);
		}
		CloesdRoomController.Clear();
	}

	public void TestButtonT()
	{
		LevelMgr.Inst.CurrentRoomCtrller.KillAllMonster();
	}

	public void TestButtonY()
	{
		LevelMgr.Inst.CurrentRoomCtrller.KillAllMonster2();
	}

	public void TestButtonR()
	{
		GameMgr.Inst.TestButtonReloadLevel();
	}

	public void TestButtonL()
	{
		UIMgr.Inst.uiSetting._LanguageChangeLeftRight(1);
	}

	public void TestButtonG()
	{
		GameUISingletonMono<UITraining>.ShowInit();
	}

	public void TestButtonI()
	{
	}

	public void TestButtonCreateCharacter()
	{
		MobileMgr.inst.PluginActivity.CreateCharacter();
	}

	public void TestButtonNotifyZone()
	{
		MobileMgr.inst.PluginActivity.NotifyZone();
	}

	public void TestButtonPayTest()
	{
		MobileMgr.inst.PluginActivity.PayTest();
	}

	public void TestButtoBackQuote()
	{
		CommandLineMgr.Inst.TestButtonBackQuote();
	}

	public void Shoot()
	{
		if (attackImage.sprite == spriteAttack || DataMgr.settingData.Mobiledata.indieInteractButton)
		{
			GameMgr.Inst.playerMgr.PlayerCtrller.SetShootPerformedIfHanveShootGroup(shoot: true);
		}
		else
		{
			UIInteractiveObjMgr.Inst.Interact();
		}
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
	}

	public void NotShoot()
	{
		if (attackImage.sprite == spriteAttack || DataMgr.settingData.Mobiledata.indieInteractButton)
		{
			GameMgr.Inst.playerMgr.PlayerCtrller.SetShootPerformedIfHanveShootGroup(shoot: false);
			if ((bool)PlayerMgr.Inst && (bool)PlayerMgr.Inst.SelectedWand)
			{
				PlayerMgr.Inst.PlayerCtrller.WandChargeEffect(chargeStart: false);
			}
			PlayerMgr.Inst.PlayerCtrller.CastLockUnRegister();
		}
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
	}

	public void _KillSummon()
	{
		UIKillSummonButton.StaticKillSummon();
	}

	public void _OpenQuickPanel()
	{
		GameMgr.OpenQuickPanel();
	}
}
