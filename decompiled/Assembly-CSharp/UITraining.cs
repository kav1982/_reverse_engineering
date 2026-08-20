using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UITraining")]
public class UITraining : GameUISingletonMono<UITraining>
{
	public GameObject pfb_UITrainingSlot;

	public GameObject SetActive;

	public Scrollbar scrollbarVertical;

	[Header("Wand")]
	public UIInfoWand uiInfoWand;

	public GridLayoutGroup glg_WandListContent;

	public GameObject WandSelect_Frame;

	public GameObject WandViewPort;

	private int gamepadSelectedSlotIndex_Wand = -1;

	[Header("Spell")]
	public UIInfoSpell uiInfoSpell;

	public GameObject SpellViewPort;

	public List<GameObject> navContainersSpell;

	public GameObject SpellSelect_Frame;

	private int currentSelectedLevel = 1;

	private int gamepadSelectedSpellTypeIndex;

	private int gamepadSelectedSpellSlotIndex = -1;

	public GameObject sortSpellButton;

	[Header("Relic")]
	public UIInfoRelic uiInfoRelic;

	public List<GameObject> navContainersRelic;

	public GameObject RelicSelect_Frame;

	public GameObject RelicViewPort;

	private int gamepadSelectedRelicTypeIndex;

	private int gamepadSelectedRelicSlotIndex = -1;

	public GameObject gameobject_pad_recover;

	public GameObject gameobject_Key_Recover;

	public GameObject gameobject_pad_SwitchLevel;

	public GameObject gameobject_ChangeSpellLevel;

	public GameObject gameobject_pandControllerHint;

	public Animator anima;

	public Toggle toggle1;

	public Toggle toggle2;

	public Toggle toggle3;

	public Vector3 InfoOffsetVector3;

	public Vector3 infoOffset;

	public Vector3 infoWandOffset;

	public Vector3 infoRelicOffset;

	public Image Longpress;

	private UITrainingSlot lastEnteredSlot;

	private int selectedButton;

	public List<Button> Buttons;

	[Header("Language")]
	public Text text_TrainingTitle;

	public Text text_ClickToGetSpell;

	public Text text_GetWand;

	public Text text_GetRelic;

	public Text text_ClearGround;

	public Text text_ClearGround_Gamepad;

	public Text text_longpress;

	public Text text_SwitchSpellLevel;

	[Header("Sorting")]
	private SortTypeSpell sortType = SortTypeSpell.Type;

	public Image sortOrderSpriteRenderer;

	public Sprite sortOrderSprite1;

	public Sprite sortOrderSprite2;

	public List<Text> textSortingTexts;

	public List<Text> textSortingTextsRelic;

	[Header("Gamepad")]
	public int widthCount;

	public int heightCount;

	public ScrollRect scrollRect;

	public Custom_ScrollRect scrollRect_Wand;

	public ScrollRect scrollRectRelic;

	private UITrainingSlot[] uiSpellSlots;

	private UITrainingSlot[] uiWandSlots;

	private UITrainingSlot[] uiRelicSlots;

	public RectTransform rtsfVerticalLayoutGroup;

	public RectTransform rtsfVerticalLayoutGroupRelic;

	public float longpresstime = 1f;

	public float shortpressThreshhold = 0.2f;

	private float _presstime;

	private bool timeadd;

	[Header("ControlShow")]
	public UpdatButtonShow[] updatebuttonshows;

	[Header("法术切换面板")]
	public RectTransform rtsfPanelRight;

	public CanvasGroup canvasGroupPanelRight;

	public float startPosition;

	public float endPosition;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.GamepadLB.performed += PanelSwitchLeft;
		base.inputActions.Player.GamepadRB.performed += PanelSwitchRight;
		base.inputActions.Player.GamepadWest.performed += GamepadWestPerformed;
		base.inputActions.Player.GamepadWest.canceled += GamepadWestCanceled;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed;
		base.inputActions.Player.Drink.performed += GamepadDrinkPerformed;
		base.inputActions.Player.WASD.performed += PanelSwitchPC;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed;
		base.inputActions.Player.GamepadLB.performed -= PanelSwitchLeft;
		base.inputActions.Player.GamepadRB.performed -= PanelSwitchRight;
		base.inputActions.Player.Drink.performed -= GamepadDrinkPerformed;
		base.inputActions.Player.GamepadWest.performed -= GamepadWestPerformed;
		base.inputActions.Player.GamepadWest.canceled -= GamepadWestCanceled;
		base.inputActions.Player.WASD.performed -= PanelSwitchPC;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	protected override IEnumerator OnInit()
	{
		yield return StartCoroutine(StartIE());
	}

	private IEnumerator StartIE()
	{
		anima.enabled = false;
		foreach (GameObject item in navContainersSpell)
		{
			item.transform.DestroyAllChildImmediate();
		}
		List<UITrainingSlot> list = new List<UITrainingSlot>();
		foreach (SpellConfig item2 in SpellConfig.list)
		{
			if (item2.level != 1 || item2.dropType == ItemDropType.None)
			{
				continue;
			}
			int num = 0;
			switch (sortType)
			{
			case SortTypeSpell.Rare:
				switch (item2.dropType)
				{
				case ItemDropType.None:
					num = 3;
					break;
				case ItemDropType.Common:
					num = 0;
					break;
				case ItemDropType.Rare:
					num = 1;
					break;
				case ItemDropType.Epic:
					num = 2;
					break;
				case ItemDropType.Special:
					num = 3;
					break;
				default:
					Debug.LogError(item2.dropType);
					break;
				}
				break;
			case SortTypeSpell.Type:
				num = item2.useType switch
				{
					SpellType.Missile => 0, 
					SpellType.Summon => 1, 
					SpellType.Enhance => 2, 
					SpellType.Passive => 3, 
					_ => 3, 
				};
				break;
			}
			UITrainingSlot component = UnityEngine.Object.Instantiate(pfb_UITrainingSlot, navContainersSpell[num].transform).GetComponent<UITrainingSlot>();
			component.InitializeSpellOrRelic(this, GalleryCategory.Spell, item2.id, navContainersSpell[num].transform.childCount - 1, num, currentSelectedLevel);
			list.Add(component);
		}
		uiSpellSlots = list.ToArray();
		glg_WandListContent.transform.DestroyAllChild();
		list.Clear();
		int num2 = 0;
		for (int i = 0; i < WandConfig.list.Count; i++)
		{
			if (1 <= WandConfig.list[i].dropStage && WandConfig.list[i].dropStage <= 20)
			{
				UITrainingSlot component2 = UnityEngine.Object.Instantiate(pfb_UITrainingSlot, glg_WandListContent.transform).GetComponent<UITrainingSlot>();
				component2.InitializeWand(this, GalleryCategory.Wand, WandConfig.list[i].id, num2);
				num2++;
				list.Add(component2);
			}
		}
		uiWandSlots = list.ToArray();
		foreach (GameObject item3 in navContainersRelic)
		{
			item3.transform.DestroyAllChildImmediate();
		}
		list.Clear();
		int num3 = 0;
		foreach (RelicConfig item4 in RelicConfig.list)
		{
			int num4 = 0;
			RelicConfig relicConfig = item4;
			if (relicConfig.dropType != ItemDropType.Special && relicConfig.abilityType != RelicAbilityType.MagicThing && relicConfig.abilityType != RelicAbilityType.RestartKey && relicConfig.abilityType != RelicAbilityType.RandomLevelUp && relicConfig.dropType != 0)
			{
				switch (relicConfig.dropType)
				{
				case ItemDropType.None:
					num4 = 0;
					break;
				case ItemDropType.Common:
					num4 = 0;
					break;
				case ItemDropType.Rare:
					num4 = 1;
					break;
				case ItemDropType.Epic:
					num4 = 2;
					break;
				}
				UITrainingSlot component3 = UnityEngine.Object.Instantiate(pfb_UITrainingSlot, navContainersRelic[num4].transform).GetComponent<UITrainingSlot>();
				component3.InitializeSpellOrRelic(this, GalleryCategory.Relic, relicConfig.id, navContainersRelic[num4].transform.childCount - 1, num4);
				num3++;
				list.Add(component3);
			}
		}
		uiRelicSlots = list.ToArray();
		yield return null;
		LanguageChange();
		anima.enabled = true;
		uiInfoSpell.gameObject.SetActive(value: false);
		SpellViewPort.SetActive(value: true);
		WandViewPort.SetActive(value: false);
		SpellSelect_Frame.SetActive(value: true);
		WandSelect_Frame.SetActive(value: false);
		RelicSelect_Frame.SetActive(value: false);
		gameobject_ChangeSpellLevel.SetActive(value: true);
	}

	protected override void OnShow(object obj = null)
	{
		StartCoroutine(GameMgr.Inst.WaitAndInvokeAction(1, InputChange));
		uiInfoWand.gameObject.SetActive(value: false);
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
		UpdateRelicCanGet();
	}

	protected override void OnHide()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			SlotExit(lastEnteredSlot);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		case PlayerInputType.Gamepad:
			break;
		}
		uiInfoSpell.gameObject.SetActive(value: false);
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		SetActive.SetActive(value: false);
		ExitAllSlot();
		SEMgr.Inst.uiClose.PlaySE();
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void PanelSwitchPC(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			if (vector == Vector2.left)
			{
				PanelSwitchLeft(default(InputAction.CallbackContext));
			}
			else if (vector == Vector2.right)
			{
				PanelSwitchRight(default(InputAction.CallbackContext));
			}
		}
	}

	private void GamepadDrinkPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			SEMgr.Inst.uiSwitch.PlaySE();
			if (toggle1.isOn)
			{
				toggle2.isOn = true;
				currentSelectedLevel = 2;
			}
			else if (toggle2.isOn)
			{
				toggle3.isOn = true;
				currentSelectedLevel = 3;
			}
			else if (toggle3.isOn)
			{
				toggle1.isOn = true;
				currentSelectedLevel = 1;
			}
			else
			{
				Debug.LogError("!");
			}
			for (int i = 0; i < uiSpellSlots.Length; i++)
			{
				uiSpellSlots[i].UpdateLevel(currentSelectedLevel);
			}
		}
	}

	private void GamepadWestCanceled(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			timeadd = false;
			if (_presstime <= shortpressThreshhold)
			{
				Debug.Log("GamepadWestCanceled");
				SwitchSortingType();
			}
			_presstime = 0f;
		}
	}

	private void GamepadWestPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			timeadd = true;
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen)
		{
			return;
		}
		if (SpellViewPort.activeSelf)
		{
			int index = gamepadSelectedSpellTypeIndex;
			int num = gamepadSelectedSpellSlotIndex;
			Vector2 vector = context.ReadValue<Vector2>();
			MoveDire(vector);
			if (vector != Vector2.zero)
			{
				if (num != -1)
				{
					SlotExit(navContainersSpell[index].transform.GetChild(num).GetComponent<UITrainingSlot>());
				}
				SlotEnter(navContainersSpell[gamepadSelectedSpellTypeIndex].transform.GetChild(gamepadSelectedSpellSlotIndex).GetComponent<UITrainingSlot>());
			}
			return;
		}
		if (RelicViewPort.activeSelf)
		{
			int index2 = gamepadSelectedRelicTypeIndex;
			int num2 = gamepadSelectedRelicSlotIndex;
			Vector2 vector2 = context.ReadValue<Vector2>();
			MoveDire(vector2);
			if (vector2 != Vector2.zero)
			{
				if (num2 != -1)
				{
					SlotExit(navContainersRelic[index2].transform.GetChild(num2).GetComponent<UITrainingSlot>());
				}
				SlotEnter(navContainersRelic[gamepadSelectedRelicTypeIndex].transform.GetChild(gamepadSelectedRelicSlotIndex).GetComponent<UITrainingSlot>());
			}
			return;
		}
		int num3 = gamepadSelectedSlotIndex_Wand;
		Vector2 vector3 = context.ReadValue<Vector2>();
		MoveDire(vector3);
		if (vector3 != Vector2.zero)
		{
			if (num3 != -1)
			{
				SlotExit(uiWandSlots[num3]);
			}
			SlotEnter(uiWandSlots[gamepadSelectedSlotIndex_Wand]);
			Mathf.CeilToInt((float)uiWandSlots.Length / (float)widthCount);
			int currentRow = Mathf.CeilToInt(((float)gamepadSelectedSlotIndex_Wand + 1f) / (float)widthCount);
			scrollRect_Wand.ScrollUpdate(currentRow);
		}
	}

	private void FixedUpdate()
	{
		if (!gameobject_pad_recover.activeSelf)
		{
			return;
		}
		if (timeadd)
		{
			_presstime += Time.fixedDeltaTime;
			if (_presstime >= longpresstime)
			{
				_Recover();
				timeadd = false;
			}
		}
		else if (_presstime != 0f)
		{
			_presstime = 0f;
		}
		if (_presstime > shortpressThreshhold || _presstime == 0f)
		{
			Longpress.fillAmount = _presstime / longpresstime;
		}
	}

	private void PanelSwitchLeft(InputAction.CallbackContext context)
	{
		SlotExit(lastEnteredSlot);
		if (selectedButton == 0)
		{
			selectedButton = Buttons.Count - 1;
		}
		else
		{
			selectedButton--;
		}
		Buttons[selectedButton].onClick.Invoke();
	}

	private void PanelSwitchRight(InputAction.CallbackContext context)
	{
		SlotExit(lastEnteredSlot);
		if (selectedButton == Buttons.Count - 1)
		{
			selectedButton = 0;
		}
		else
		{
			selectedButton++;
		}
		Buttons[selectedButton].onClick.Invoke();
	}

	public void ExitAllSlot()
	{
		SlotExit(lastEnteredSlot);
		uiInfoRelic.gameObject.SetActive(value: false);
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoSpell.gameObject.SetActive(value: false);
		gamepadSelectedSpellSlotIndex = -1;
		gamepadSelectedRelicSlotIndex = -1;
		gamepadSelectedSlotIndex_Wand = -1;
	}

	public void OnScroll()
	{
		if (ControlMgr.Inst.isScreenTouching)
		{
			ExitAllSlot();
		}
	}

	private void OnClickSelectPreClose()
	{
		sortSpellButton.SetActive(value: false);
		SpellViewPort.SetActive(value: false);
		WandViewPort.SetActive(value: false);
		SpellSelect_Frame.SetActive(value: false);
		WandSelect_Frame.SetActive(value: false);
		gameobject_ChangeSpellLevel.SetActive(value: false);
		uiInfoSpell.gameObject.SetActive(value: false);
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		RelicSelect_Frame.SetActive(value: false);
		RelicViewPort.SetActive(value: false);
	}

	private void MoveDire(Vector2 _direct)
	{
		if (SpellViewPort.activeSelf)
		{
			CustomNavTool.Nav(_direct, ref gamepadSelectedSpellSlotIndex, ref gamepadSelectedSpellTypeIndex, navContainersSpell.Select((GameObject x) => x.transform).ToList(), widthCount);
			GeneralTool.ScrollToPadSelected(scrollRect, rtsfVerticalLayoutGroup, navContainersSpell[gamepadSelectedSpellTypeIndex].transform.GetChild(gamepadSelectedSpellSlotIndex).GetComponent<RectTransform>());
		}
		else if (RelicViewPort.activeSelf)
		{
			CustomNavTool.Nav(_direct, ref gamepadSelectedRelicSlotIndex, ref gamepadSelectedRelicTypeIndex, navContainersRelic.Select((GameObject x) => x.transform).ToList(), widthCount);
			GeneralTool.ScrollToPadSelected(scrollRectRelic, rtsfVerticalLayoutGroupRelic, navContainersRelic[gamepadSelectedRelicTypeIndex].transform.GetChild(gamepadSelectedRelicSlotIndex).GetComponent<RectTransform>());
		}
		else if (gamepadSelectedSlotIndex_Wand == -1)
		{
			gamepadSelectedSlotIndex_Wand = 0;
		}
		else if (_direct == Vector2.left)
		{
			gamepadSelectedSlotIndex_Wand--;
		}
		else if (_direct == Vector2.right)
		{
			if ((gamepadSelectedSlotIndex_Wand + 1) % widthCount != 0)
			{
				gamepadSelectedSlotIndex_Wand++;
				gamepadSelectedSlotIndex_Wand = Mathf.Min(gamepadSelectedSlotIndex_Wand, uiWandSlots.Length - 1);
			}
		}
		else if (_direct == Vector2.up)
		{
			if (gamepadSelectedSlotIndex_Wand - widthCount >= 0)
			{
				gamepadSelectedSlotIndex_Wand -= widthCount;
			}
		}
		else if (_direct == Vector2.down && gamepadSelectedSlotIndex_Wand != uiWandSlots.Length - 1 && gamepadSelectedSlotIndex_Wand < uiWandSlots.Length - widthCount)
		{
			gamepadSelectedSlotIndex_Wand += widthCount;
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			if (SpellViewPort.activeSelf)
			{
				SlotClick(navContainersSpell[gamepadSelectedSpellTypeIndex].transform.GetChild(gamepadSelectedSpellSlotIndex).GetComponent<UITrainingSlot>());
			}
			else if (RelicViewPort.activeSelf)
			{
				SlotClick(navContainersRelic[gamepadSelectedRelicTypeIndex].transform.GetChild(gamepadSelectedRelicSlotIndex).GetComponent<UITrainingSlot>());
			}
			else
			{
				SlotClick(uiWandSlots[gamepadSelectedSlotIndex_Wand]);
			}
		}
	}

	private void LanguageChange()
	{
		text_TrainingTitle.text = 1002406.GetText();
		text_GetWand.text = 1002402.GetText();
		text_GetRelic.text = 1002410.GetText();
		text_ClickToGetSpell.text = 1002401.GetText();
		text_longpress.text = 1000240.GetText() + ":";
		text_SwitchSpellLevel.text = 1000413.GetText() + ":";
		text_ClearGround.text = 1002403.GetText();
		text_ClearGround_Gamepad.text = 1002403.GetText() + ":";
		switch (sortType)
		{
		case SortTypeSpell.Rare:
			textSortingTexts[0].text = 1001601.GetText();
			textSortingTexts[1].text = 1001602.GetText();
			textSortingTexts[2].text = 1001603.GetText();
			textSortingTexts[3].text = 1001604.GetText();
			textSortingTexts[0].color = GameConst.color_RarityCommon;
			textSortingTexts[1].color = GameConst.color_RarityRare;
			textSortingTexts[2].color = GameConst.color_RarityEpic;
			textSortingTexts[3].color = GameConst.color_RaritySpecial;
			break;
		case SortTypeSpell.Type:
			textSortingTexts[0].text = 1002205.GetText();
			textSortingTexts[1].text = 1002206.GetText();
			textSortingTexts[2].text = 1002207.GetText();
			textSortingTexts[3].text = 1002208.GetText();
			textSortingTexts[0].color = GameConst.color_SpellUseTypeMissle;
			textSortingTexts[1].color = GameConst.color_SpellUseTypeMissle;
			textSortingTexts[2].color = GameConst.color_SpellUseTypeEnhance;
			textSortingTexts[3].color = GameConst.color_SpellUseTypePassive;
			break;
		}
		textSortingTextsRelic[0].text = 1001601.GetText();
		textSortingTextsRelic[1].text = 1001602.GetText();
		textSortingTextsRelic[2].text = 1001603.GetText();
		textSortingTextsRelic[3].text = 1001604.GetText();
		textSortingTextsRelic[0].color = GameConst.color_RarityCommon;
		textSortingTextsRelic[1].color = GameConst.color_RarityRare;
		textSortingTextsRelic[2].color = GameConst.color_RarityEpic;
		textSortingTextsRelic[3].color = GameConst.color_RaritySpecial;
	}

	private void InputChange()
	{
		if (!base.IsOpen)
		{
			return;
		}
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			gameobject_pad_recover.SetActive(value: false);
			gameobject_pad_SwitchLevel.SetActive(value: false);
			gameobject_Key_Recover.SetActive(value: true);
			gameobject_pandControllerHint.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			gameobject_pad_recover.SetActive(value: true);
			gameobject_pad_SwitchLevel.SetActive(value: true);
			gameobject_Key_Recover.SetActive(value: false);
			gameobject_pandControllerHint.SetActive(value: true);
			if (SpellViewPort.activeSelf)
			{
				if (gamepadSelectedSpellTypeIndex != -1 && gamepadSelectedSpellSlotIndex != -1)
				{
					SlotEnter(navContainersSpell[gamepadSelectedSpellTypeIndex].transform.GetChild(gamepadSelectedSpellSlotIndex).GetComponent<UITrainingSlot>());
				}
			}
			else if (SpellViewPort.activeSelf)
			{
				if (gamepadSelectedRelicSlotIndex != -1)
				{
					SlotEnter(navContainersRelic[gamepadSelectedRelicTypeIndex].transform.GetChild(gamepadSelectedRelicSlotIndex).GetComponent<UITrainingSlot>());
				}
			}
			else if (gamepadSelectedSlotIndex_Wand != -1)
			{
				SlotEnter(uiWandSlots[gamepadSelectedSlotIndex_Wand]);
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	public void OnClickSelectWand()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		gamepadSelectedSlotIndex_Wand = -1;
		if (lastEnteredSlot != null)
		{
			SlotExit(lastEnteredSlot);
		}
		if (SpellSelect_Frame.activeInHierarchy)
		{
			rtsfPanelRight.DOAnchorPosX(startPosition, 0.5f);
			canvasGroupPanelRight.DOFade(0f, 0.5f);
		}
		OnClickSelectPreClose();
		WandViewPort.SetActive(value: true);
		WandSelect_Frame.SetActive(value: true);
	}

	public void OnClickSelectRelic()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		gamepadSelectedRelicSlotIndex = -1;
		if (lastEnteredSlot != null)
		{
			SlotExit(lastEnteredSlot);
		}
		if (SpellSelect_Frame.activeInHierarchy)
		{
			rtsfPanelRight.DOAnchorPosX(startPosition, 0.5f);
			canvasGroupPanelRight.DOFade(0f, 0.5f);
		}
		OnClickSelectPreClose();
		RelicViewPort.SetActive(value: true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(navContainersRelic[0].transform.parent.GetComponent<RectTransform>());
		RelicSelect_Frame.SetActive(value: true);
	}

	public void OnClickSelectSpell()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
		gamepadSelectedSpellSlotIndex = -1;
		if (lastEnteredSlot != null)
		{
			SlotExit(lastEnteredSlot);
		}
		if (!SpellSelect_Frame.activeInHierarchy)
		{
			rtsfPanelRight.DOAnchorPosX(endPosition, 0.5f);
			canvasGroupPanelRight.DOFade(1f, 0.5f);
		}
		OnClickSelectPreClose();
		sortSpellButton.SetActive(value: true);
		SpellViewPort.SetActive(value: true);
		SpellSelect_Frame.SetActive(value: true);
		gameobject_ChangeSpellLevel.SetActive(value: true);
	}

	private void UpdateRelicCanGet()
	{
		UITrainingSlot[] array = uiRelicSlots;
		foreach (UITrainingSlot uITrainingSlot in array)
		{
			if (UIPlayerDataMgr.Inst.CurrentRelicLevel(uITrainingSlot.ID) >= RelicConfig.dic[uITrainingSlot.ID].maxCount)
			{
				uITrainingSlot.cantGetImage.SetActive(value: true);
			}
			else
			{
				uITrainingSlot.cantGetImage.SetActive(value: false);
			}
		}
	}

	public void SlotEnter(UITrainingSlot slot)
	{
		lastEnteredSlot = slot;
		switch (slot.Category)
		{
		case GalleryCategory.Spell:
			gamepadSelectedSpellSlotIndex = slot.Index;
			gamepadSelectedSpellTypeIndex = slot.indexType;
			slot.Hover();
			if (!slot.Unlocked)
			{
				break;
			}
			uiInfoSpell.gameObject.SetActive(value: true);
			uiInfoSpell.transform.position = slot.transform.position + infoOffset;
			StartCoroutine(GameMgr.Inst.WaitAndInvokeAction(2, delegate
			{
				uiInfoSpell.canvasGroup.alpha = 1f;
				UIMgr.InteractiveFollowFitSelf(uiInfoSpell.gameObject, uiInfoSpell.GetComponent<RectTransform>().pivot, null, SetActive.transform.localScale);
			}));
			if (SpellConfig.dic[slot.ID].dropType == ItemDropType.Common || SpellConfig.dic[slot.ID].dropType == ItemDropType.Rare || slot.ID == 10171)
			{
				if (SpellConfig.dic[slot.ID].abilityType == SpellAbilityType.SpellEmbryo)
				{
					uiInfoSpell.UpdateInfo(slot.ID + Mathf.Min(2, currentSelectedLevel) - 1);
				}
				else
				{
					uiInfoSpell.UpdateInfo(slot.ID + currentSelectedLevel - 1);
				}
			}
			else
			{
				uiInfoSpell.UpdateInfo(slot.ID);
			}
			break;
		case GalleryCategory.Wand:
			gamepadSelectedSlotIndex_Wand = slot.Index;
			slot.Hover();
			if (slot.Unlocked)
			{
				uiInfoWand.gameObject.SetActive(value: true);
				uiInfoWand.transform.position = slot.transform.position + infoWandOffset;
				uiInfoWand.UpdateInfo(WandConfig.dic[slot.ID]);
				StartCoroutine(GameMgr.Inst.WaitAndInvokeAction(3, delegate
				{
					uiInfoWand.canvasGroup.alpha = 1f;
					UIMgr.InteractiveFollowFitSelf(uiInfoWand.gameObject, uiInfoWand.GetComponent<RectTransform>().pivot, null, SetActive.transform.localScale);
				}));
			}
			break;
		case GalleryCategory.Relic:
			gamepadSelectedRelicSlotIndex = slot.Index;
			gamepadSelectedRelicTypeIndex = slot.indexType;
			slot.Hover();
			if (slot.Unlocked)
			{
				uiInfoRelic.gameObject.SetActive(value: true);
				uiInfoRelic.transform.position = slot.transform.position + infoRelicOffset;
				uiInfoRelic.UpdateInfo(RelicConfig.dic[slot.ID]);
				StartCoroutine(GameMgr.Inst.WaitAndInvokeAction(2, delegate
				{
					UIMgr.InteractiveFollowFitSelf(uiInfoRelic.gameObject, uiInfoRelic.GetComponent<RectTransform>().pivot, null, SetActive.transform.localScale);
				}));
			}
			break;
		default:
			Debug.LogError(slot.Category);
			break;
		}
	}

	public void SlotExit(UITrainingSlot slot)
	{
		if (!(slot == null))
		{
			slot.Unhover();
			switch (slot.Category)
			{
			case GalleryCategory.Spell:
				uiInfoSpell.gameObject.SetActive(value: false);
				break;
			case GalleryCategory.Wand:
				uiInfoWand.gameObject.SetActive(value: false);
				break;
			case GalleryCategory.Relic:
				uiInfoRelic.gameObject.SetActive(value: false);
				break;
			default:
				Debug.LogError(slot.Category);
				break;
			}
		}
	}

	public void SlotClick(UITrainingSlot slot)
	{
		if (GameMgr.IsMobile_Static)
		{
			switch (slot.Category)
			{
			case GalleryCategory.Spell:
				if (gamepadSelectedSpellSlotIndex != slot.Index || gamepadSelectedSpellTypeIndex != slot.indexType)
				{
					gamepadSelectedSpellSlotIndex = slot.Index;
					gamepadSelectedSpellTypeIndex = slot.indexType;
					if (lastEnteredSlot != null)
					{
						SlotExit(lastEnteredSlot);
					}
					SlotEnter(slot);
					return;
				}
				break;
			case GalleryCategory.Wand:
				if (gamepadSelectedSlotIndex_Wand != slot.Index)
				{
					gamepadSelectedSlotIndex_Wand = slot.Index;
					if (lastEnteredSlot != null)
					{
						SlotExit(lastEnteredSlot);
					}
					SlotEnter(slot);
					return;
				}
				break;
			case GalleryCategory.Relic:
				if (gamepadSelectedRelicSlotIndex != slot.Index)
				{
					gamepadSelectedRelicSlotIndex = slot.Index;
					gamepadSelectedRelicTypeIndex = slot.indexType;
					if (lastEnteredSlot != null)
					{
						SlotExit(lastEnteredSlot);
					}
					SlotEnter(slot);
					return;
				}
				break;
			default:
				Debug.LogError(slot.Category);
				break;
			}
		}
		else if (!UIPlayerDataMgr.Inst.IsBagOpen)
		{
			UIPlayerDataMgr.Inst.BagOpen();
		}
		switch (slot.Category)
		{
		case GalleryCategory.Spell:
		{
			if (!slot.Unlocked)
			{
				break;
			}
			SEMgr.Inst.uiClick.PlaySE();
			if (SpellConfig.dic[slot.ID].dropType == ItemDropType.Common || SpellConfig.dic[slot.ID].dropType == ItemDropType.Rare || slot.ID == 10171)
			{
				Vector3 worldPoint = ((!GameMgr.IsMobile_Static) ? UIPlayerDataMgr.Inst.image_BagBtn.transform.position : UIPlayerDataMgr.Inst.image_BagBtn.transform.position);
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
				int id = slot.ID + currentSelectedLevel - 1;
				if (id == 40203)
				{
					id = 40202;
				}
				PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, id, RollRewardFly.DropType.Spell, slot.transform.position, localPoint + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
				{
					PlayerMgr.Inst.SpellPick(new SlotData(id));
					if (GameMgr.IsMobile_Static)
					{
						UIPlayerDataMgr.Inst.BagShakeButton();
					}
				}, isUI: true, dropOnEnd: false, null, CamController.Inst.cam_UI);
				break;
			}
			Vector3 position = UIPlayerDataMgr.Inst.image_BagBtn.transform.position;
			Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(null, position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint2, null, out var localPoint2);
			PlayerMgr.Inst.ItemCtrller.UIRewardFly(UIMgr.Inst.canvas_3, slot.ID, RollRewardFly.DropType.Spell, slot.transform.position, localPoint2 + new Vector2(-10f, -10f), null, useParticleColor: true, delegate
			{
				PlayerMgr.Inst.SpellPick(new SlotData(slot.ID));
				if (GameMgr.IsMobile_Static)
				{
					UIPlayerDataMgr.Inst.BagShakeButton();
				}
			}, isUI: true, dropOnEnd: false, null, CamController.Inst.cam_UI);
			break;
		}
		case GalleryCategory.Wand:
			if (slot.Unlocked)
			{
				SEMgr.Inst.uiClick.PlaySE();
				WandConfig wandConfig = WandConfig.GetConfig(slot.ID).Copy();
				PlayerMgr.Inst.ItemCtrller.AddRewardFly(wandConfig.id, RollRewardFly.DropType.Wand, slot.transform.position, CamController.Inst.cam_UI, wandConfig);
			}
			break;
		case GalleryCategory.Relic:
			if (slot.Unlocked && UIPlayerDataMgr.Inst.CurrentRelicLevel(slot.ID) < RelicConfig.dic[slot.ID].maxCount)
			{
				SEMgr.Inst.uiClick.PlaySE();
				PlayerMgr.Inst.ItemCtrller.AddRewardFly(slot.ID, RollRewardFly.DropType.Relic, slot.transform.position, CamController.Inst.cam_UI);
				UpdateRelicCanGet();
			}
			break;
		default:
			Debug.LogError(slot.Category);
			break;
		}
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}

	public void _PlayBtnInSE()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
	}

	public void SwitchSortingType()
	{
		if (base.IsOpen && SpellViewPort.activeSelf)
		{
			if (sortType == SortTypeSpell.Type)
			{
				sortType = SortTypeSpell.Rare;
				sortOrderSpriteRenderer.sprite = sortOrderSprite2;
			}
			else
			{
				sortType = SortTypeSpell.Type;
				sortOrderSpriteRenderer.sprite = sortOrderSprite1;
			}
			gamepadSelectedSpellSlotIndex = 0;
			gamepadSelectedSpellTypeIndex = 0;
			StartCoroutine(StartIE());
			LayoutRebuilder.ForceRebuildLayoutImmediate(rtsfVerticalLayoutGroup);
			scrollbarVertical.value = 1f;
			for (int i = 0; i < uiSpellSlots.Length; i++)
			{
				uiSpellSlots[i].UpdateLevel(currentSelectedLevel);
			}
		}
	}

	public void _GetWand()
	{
		List<int> canDropWandIDs = WandConfig.GetCanDropWandIDs();
		PlayerMgr.Inst.WandPickUp(WandConfig.GetConfig(canDropWandIDs[UnityEngine.Random.Range(0, canDropWandIDs.Count)]));
		SEMgr.Inst.uiTrainingGetWand.PlaySE();
	}

	public void _Recover()
	{
		GameMgr.Inst.playerMgr.SummonsAllDead(instanceDeath: true, clearAllAutoWand: false);
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SpellConfigComponentData));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			if (!entityManager.HasComponent<UnitProperty_Dots>(item))
			{
				World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(item);
			}
		}
		PlayerMgr.Inst.summonsPpts.Clear();
		PlayerMgr.Inst.summonsNotAttackPpts.Clear();
		LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.Clear();
		LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.Clear();
		UnityEngine.Object.Destroy(PlayerMgr.Inst.MiniPool.gameObject);
		PlayerMgr.Inst.MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool")).GetComponent<MiniObjPool>();
		for (int num = PlayerMgr.Inst.BaData.relicCfgs.Count - 1; num >= 0; num--)
		{
			PlayerMgr.Inst.ItemCtrller.RelicRemove(PlayerMgr.Inst.BaData.relicCfgs[num].id, 9999);
		}
		PlayerMgr.Inst.BaData.relicCfgs.Clear();
		UIPlayerDataMgr.Inst.RelicUpdate();
		UpdateRelicCanGet();
		PlayerMgr.Inst.RefreshPlayer();
		AccessCampSystem.ClearPoolInCamp();
		SEMgr.Inst.uiTrainingClearGround.PlaySE();
	}

	public void _ToggleChange()
	{
		if (toggle1.isOn)
		{
			currentSelectedLevel = 1;
		}
		else if (toggle2.isOn)
		{
			currentSelectedLevel = 2;
		}
		else if (toggle3.isOn)
		{
			currentSelectedLevel = 3;
		}
		else
		{
			Debug.LogError("!");
		}
		for (int i = 0; i < uiSpellSlots.Length; i++)
		{
			if (SpellConfig.dic[uiSpellSlots[i].ID].abilityType == SpellAbilityType.SpellEmbryo)
			{
				uiSpellSlots[i].UpdateLevel(Mathf.Min(currentSelectedLevel, 2));
			}
			else
			{
				uiSpellSlots[i].UpdateLevel(currentSelectedLevel);
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			ExitAllSlot();
		}
	}

	public void _SwitchSpellSound()
	{
		SEMgr.Inst.uiClick.PlaySE();
	}
}
