using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UISpellDisable")]
public class UISpellDisable : GameUISingletonMono<UISpellDisable>
{
	public enum CostType
	{
		Crystal,
		Blood
	}

	public UISpellDisableHistory disableHistory;

	public UISpellDisableHistoryApply applyWindow;

	public UISpellDisableSlot pfb_UISpellDisableSlot;

	public UIInfoSpell uiInfoSpell;

	public Animator anima_Self;

	public Animator anima_ResidualCount;

	public Vector3 infoOffset;

	public int disableCostCrystalPerCount;

	public int disableCostBloodPerCount;

	public int defaultFreeDisableCount;

	public int defaultMaxDisableCount;

	public Text text_ResidualCount;

	public Text text_FreeDisableLeftCount;

	private SortTypeSpell sortType = SortTypeSpell.Type;

	public Image sortOrderSpriteRenderer;

	public Sprite sortOrderSprite1;

	public Sprite sortOrderSprite2;

	public List<Text> textSortingTexts;

	public List<GridLayoutGroup> gridLayouts;

	public Scrollbar scrollbarVertical;

	public RectTransform rtsfVerticalLayoutGroup;

	public ScrollRect scrollRect;

	[Header("Confirm")]
	public GameObject panel_Confirm;

	public Image image_ConfirmSpellIcon;

	public Text text_CrystalCost;

	public Text text_BloodCost;

	public Button btn_Crystal;

	public Button btn_Blood;

	[Header("Language")]
	public Text text_Title;

	public Text text_ConfirmTitle;

	public Text text_Or;

	public Text text_ResidualDisabling;

	public Text text_leftDisableFree;

	[Header("Controller")]
	private int gamepadSelectedSlotIndex;

	private int gamepadSelectedSlotType;

	public int widthCount;

	public int heightCount;

	public GameObject ControllerCrystalSelectFrame;

	public GameObject ControllerBloodSelectFrame;

	public List<CostType> costTypes = new List<CostType>();

	private UISpellDisableSlot selectedSlot;

	public int finalFreeDisableCount;

	public int finalMaxDisableCount;

	public int disableCounter;

	public List<UISpellDisableSlot> disableSlots = new List<UISpellDisableSlot>();

	public UpdatButtonShow[] updatebuttonshows;

	public float crystalCostToBloodCostRatio => (float)disableCostBloodPerCount / (float)disableCostCrystalPerCount;

	protected override void RegistarWhenInit()
	{
		EventMgr.AncienBloodChange = (Action)Delegate.Combine(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
		EventMgr.MagicCrystalChange = (Action)Delegate.Combine(EventMgr.MagicCrystalChange, new Action(MagicCrystalChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.GamepadWest.performed += GamepadWestPerformed;
		base.inputActions.Player.Drink.performed += GamepadDrinkPerformed;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadWest.performed -= GamepadWestPerformed;
		base.inputActions.Player.Drink.performed -= GamepadDrinkPerformed;
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.AncienBloodChange = (Action)Delegate.Remove(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
		EventMgr.MagicCrystalChange = (Action)Delegate.Remove(EventMgr.MagicCrystalChange, new Action(MagicCrystalChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void GamepadDrinkPerformed(InputAction.CallbackContext obj)
	{
		if (!base.IsOpen || disableHistory.IsOpen || panel_Confirm.activeInHierarchy)
		{
			return;
		}
		disableHistory.Show();
		if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			disableHistory.gamepadSelectedIndex = 0;
			if (disableHistory.itemList.Count > 0)
			{
				disableHistory.itemList[disableHistory.gamepadSelectedIndex].gamepadSelectedFrame.SetActive(value: true);
				GeneralTool.ScrollToPadSelected(scrollRect, disableHistory.content, disableHistory.itemList[disableHistory.gamepadSelectedIndex].GetComponent<RectTransform>(), doTween: false);
			}
		}
	}

	private void InputChange()
	{
		if (!base.IsOpen)
		{
			return;
		}
		ControlChange();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			SlotExit(gridLayouts[gamepadSelectedSlotType].transform.GetChild(gamepadSelectedSlotIndex).GetComponent<UISpellDisableSlot>());
			ControllerCrystalSelectFrame.SetActive(value: false);
			ControllerBloodSelectFrame.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			gamepadSelectedSlotType = 0;
			gamepadSelectedSlotIndex = 0;
			if (!panel_Confirm.activeSelf)
			{
				SlotEnter(gridLayouts[gamepadSelectedSlotType].transform.GetChild(gamepadSelectedSlotIndex).GetComponent<UISpellDisableSlot>());
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen)
		{
			return;
		}
		if (applyWindow.IsOpen)
		{
			applyWindow.applyButton.onClick.Invoke();
		}
		else if (disableHistory.IsOpen)
		{
			disableHistory.itemList[disableHistory.gamepadSelectedIndex].GetComponent<Button>().onClick.Invoke();
		}
		else if (panel_Confirm.activeSelf)
		{
			if (ControllerBloodSelectFrame.activeSelf)
			{
				DisableBlood();
			}
			else
			{
				DisableCrystal();
			}
		}
		else
		{
			SlotClick(gridLayouts[gamepadSelectedSlotType].transform.GetChild(gamepadSelectedSlotIndex).GetComponent<UISpellDisableSlot>());
		}
	}

	private void GamepadWestPerformed(InputAction.CallbackContext context)
	{
		SwitchSortingType();
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDire(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDire(vector);
		}
	}

	private void MoveDire(Vector2 _direct)
	{
		int index = gamepadSelectedSlotType;
		int index2 = gamepadSelectedSlotIndex;
		if (applyWindow.IsOpen)
		{
			if (_direct == Vector2.left)
			{
				applyWindow.slider.value--;
			}
			else if (_direct == Vector2.right)
			{
				applyWindow.slider.value++;
			}
			return;
		}
		if (disableHistory.IsOpen)
		{
			if (disableHistory.itemList.Count != 0)
			{
				disableHistory.itemList[disableHistory.gamepadSelectedIndex].gamepadSelectedFrame.SetActive(value: false);
				if (_direct == Vector2.up)
				{
					disableHistory.gamepadSelectedIndex--;
				}
				else if (_direct == Vector2.down)
				{
					disableHistory.gamepadSelectedIndex++;
				}
				disableHistory.gamepadSelectedIndex = Mathf.Clamp(disableHistory.gamepadSelectedIndex, 0, disableHistory.itemList.Count - 1);
				disableHistory.itemList[disableHistory.gamepadSelectedIndex].gamepadSelectedFrame.SetActive(value: true);
				GeneralTool.ScrollToPadSelected(disableHistory.scrollRect, disableHistory.content, disableHistory.itemList[disableHistory.gamepadSelectedIndex].GetComponent<RectTransform>());
			}
			return;
		}
		if (panel_Confirm.activeSelf)
		{
			if (_direct == Vector2.left)
			{
				if (ControllerBloodSelectFrame.activeSelf)
				{
					ControllerCrystalSelectFrame.SetActive(value: true);
					ControllerBloodSelectFrame.SetActive(value: false);
				}
				else
				{
					ControllerCrystalSelectFrame.SetActive(value: true);
				}
			}
			else if (_direct == Vector2.right)
			{
				if (ControllerCrystalSelectFrame.activeSelf)
				{
					ControllerCrystalSelectFrame.SetActive(value: false);
					ControllerBloodSelectFrame.SetActive(value: true);
				}
				else
				{
					ControllerBloodSelectFrame.SetActive(value: true);
				}
			}
			return;
		}
		if (_direct == Vector2.left)
		{
			if (gamepadSelectedSlotIndex > 0)
			{
				gamepadSelectedSlotIndex--;
			}
		}
		else if (_direct == Vector2.right)
		{
			if (gamepadSelectedSlotIndex < gridLayouts[gamepadSelectedSlotType].transform.childCount - 1)
			{
				gamepadSelectedSlotIndex++;
			}
		}
		else if (_direct == Vector2.up)
		{
			if (gamepadSelectedSlotIndex - widthCount >= 0)
			{
				gamepadSelectedSlotIndex -= widthCount;
			}
			else if (gamepadSelectedSlotType > 0)
			{
				gamepadSelectedSlotType--;
				if (GetIndexHorizon(gamepadSelectedSlotIndex) > GetIndexHorizon(gridLayouts[gamepadSelectedSlotType].transform.childCount - 1))
				{
					gamepadSelectedSlotIndex = gridLayouts[gamepadSelectedSlotType].transform.childCount - 1;
				}
				else if (gridLayouts[gamepadSelectedSlotType].transform.childCount % widthCount != 0)
				{
					gamepadSelectedSlotIndex = widthCount * Mathf.FloorToInt(gridLayouts[gamepadSelectedSlotType].transform.childCount / heightCount) + gamepadSelectedSlotIndex % widthCount;
				}
				else
				{
					gamepadSelectedSlotIndex = widthCount * Mathf.FloorToInt(gridLayouts[gamepadSelectedSlotType].transform.childCount / heightCount - 1) + gamepadSelectedSlotIndex % widthCount;
				}
			}
		}
		else if (_direct == Vector2.down)
		{
			if (gamepadSelectedSlotIndex < gridLayouts[gamepadSelectedSlotType].transform.childCount - widthCount)
			{
				gamepadSelectedSlotIndex += widthCount;
			}
			else if (GetIndexVertical(gamepadSelectedSlotIndex) < GetIndexVertical(gridLayouts[gamepadSelectedSlotType].transform.childCount - 1))
			{
				gamepadSelectedSlotIndex = gridLayouts[gamepadSelectedSlotType].transform.childCount - 1;
			}
			else if (gamepadSelectedSlotType < gridLayouts.Count - 1)
			{
				if (!gridLayouts[gamepadSelectedSlotType + 1].gameObject.activeSelf)
				{
					return;
				}
				gamepadSelectedSlotType++;
				if (gridLayouts[gamepadSelectedSlotType].transform.childCount < gamepadSelectedSlotIndex % widthCount + 1)
				{
					gamepadSelectedSlotIndex = gridLayouts[gamepadSelectedSlotType].transform.childCount - 1;
				}
				else
				{
					gamepadSelectedSlotIndex %= widthCount;
				}
			}
		}
		SlotExit(gridLayouts[index].transform.GetChild(index2).GetComponent<UISpellDisableSlot>());
		SlotEnter(gridLayouts[gamepadSelectedSlotType].transform.GetChild(gamepadSelectedSlotIndex).GetComponent<UISpellDisableSlot>());
		RectTransform component = gridLayouts[gamepadSelectedSlotType].transform.GetChild(gamepadSelectedSlotIndex).GetComponent<RectTransform>();
		RectTransform contentRect = rtsfVerticalLayoutGroup;
		GeneralTool.ScrollToPadSelected(scrollRect, contentRect, component);
		int GetIndexHorizon(int x)
		{
			if ((x + 1) % widthCount == 0)
			{
				return widthCount;
			}
			return (x + 1) % widthCount;
		}
		int GetIndexVertical(int x)
		{
			if ((x + 1) % widthCount == 0)
			{
				return Mathf.FloorToInt(x / widthCount) - 1;
			}
			return Mathf.FloorToInt(x / widthCount);
		}
	}

	private void AncienBloodChange()
	{
		if (DataMgr.selectedWorldData.ancientBloodCount >= 1)
		{
			text_BloodCost.color = Color.green;
		}
		else
		{
			text_BloodCost.color = Color.red;
		}
	}

	private void MagicCrystalChange()
	{
		if (DataMgr.selectedWorldData.ancientBloodCount >= 1)
		{
			text_BloodCost.color = Color.green;
		}
		else
		{
			text_BloodCost.color = Color.red;
		}
	}

	private void LanguageChange()
	{
		text_Title.text = 1003501.GetText();
		text_ConfirmTitle.text = 1003502.GetText();
		text_Or.text = 1003503.GetText();
		text_ResidualDisabling.text = 1003504.GetText() + ":";
		text_leftDisableFree.text = 1003505.GetText() + ":";
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
	}

	private void Init(bool switchSorlt = false)
	{
		disableSlots.Clear();
		if (!switchSorlt)
		{
			DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Clear();
			disableCounter = 0;
		}
		anima_Self.enabled = false;
		finalFreeDisableCount = defaultFreeDisableCount + DataMgr.selectedWorldData.ActivateGirl_ExtraFreeDisableCount();
		finalMaxDisableCount = defaultMaxDisableCount + DataMgr.selectedWorldData.ActivateGirl_ExtraMaxDisableCount();
		foreach (GridLayoutGroup gridLayout in gridLayouts)
		{
			gridLayout.transform.DestroyAllChild();
		}
		int num = 0;
		if (sortType == SortTypeSpell.Rare)
		{
			textSortingTexts[3].gameObject.SetActive(value: false);
			gridLayouts[3].gameObject.SetActive(value: false);
		}
		else
		{
			textSortingTexts[3].gameObject.SetActive(value: true);
			gridLayouts[3].gameObject.SetActive(value: true);
		}
		for (int i = 0; i < SpellConfig.list.Count; i++)
		{
			if (SpellConfig.list[i].level != 1 || SpellConfig.list[i].dropType == ItemDropType.None || SpellConfig.list[i].dropType == ItemDropType.Special)
			{
				continue;
			}
			int index = 0;
			switch (sortType)
			{
			case SortTypeSpell.Rare:
				switch (SpellConfig.list[i].dropType)
				{
				case ItemDropType.Common:
					index = 0;
					break;
				case ItemDropType.Rare:
					index = 1;
					break;
				case ItemDropType.Epic:
					index = 2;
					break;
				case ItemDropType.None:
				case ItemDropType.Special:
					continue;
				}
				break;
			case SortTypeSpell.Type:
				index = SpellConfig.list[i].useType switch
				{
					SpellType.Missile => 0, 
					SpellType.Summon => 1, 
					SpellType.Enhance => 2, 
					SpellType.Passive => 3, 
					_ => 3, 
				};
				break;
			}
			UISpellDisableSlot uISpellDisableSlot = UnityEngine.Object.Instantiate(pfb_UISpellDisableSlot, gridLayouts[index].transform);
			uISpellDisableSlot.Initialize(this, SpellConfig.list[i].id, num);
			num++;
			disableSlots.Add(uISpellDisableSlot);
			if (switchSorlt)
			{
				if (DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Contains(SpellConfig.list[i].id))
				{
					BanSpellDirect(uISpellDisableSlot);
				}
			}
			else if (DataMgr.selectedWorldData.spellDisableFreeIDs3.Contains(SpellConfig.list[i].id))
			{
				SlotClick(uISpellDisableSlot, playSE: false);
			}
		}
		DataMgr.selectedWorldData.battleData9.RandomSpellWandCheck();
		anima_Self.enabled = true;
		AncienBloodChange();
		MagicCrystalChange();
		LanguageChange();
		UpdateResidualCount();
	}

	protected override IEnumerator OnInit()
	{
		Init();
		yield return null;
	}

	public void ExitSelected()
	{
		if (selectedSlot != null)
		{
			selectedSlot.OnPointerExit(null);
			selectedSlot = null;
			uiInfoSpell.gameObject.SetActive(value: false);
		}
	}

	private void UpdateResidualCount()
	{
		text_ResidualCount.text = (finalMaxDisableCount - disableCounter).ToString();
		int num = finalFreeDisableCount - disableCounter;
		text_FreeDisableLeftCount.text = ((num > 0) ? num : 0).ToString();
	}

	protected override void OnShow(object obj = null)
	{
		anima_Self.Play("Show");
		UIMgr.TryAdditionalMobileShow(base.transform);
		CamController.Inst.MouseOffsetPause();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		TimeScaleMgr.Inst.Pause();
		ControllerBloodSelectFrame.SetActive(value: false);
		ControllerCrystalSelectFrame.SetActive(value: false);
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			uiInfoSpell.gameObject.SetActive(value: false);
		}
		SEMgr.Inst.uiOpen.PlaySE();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Crystal);
	}

	public void SlotEnter(UISpellDisableSlot slot)
	{
		slot.Hover();
		if (slot.UnLocked)
		{
			uiInfoSpell.gameObject.SetActive(value: true);
			uiInfoSpell.transform.position = slot.transform.position + infoOffset;
			uiInfoSpell.UpdateInfo(slot.Level1ID);
			StartCoroutine(WaitAndInvokeAction(2, delegate
			{
				UIMgr.InteractiveFollowFitSelf(uiInfoSpell.gameObject, uiInfoSpell.GetComponent<RectTransform>().pivot, null, base.transform.parent.localScale);
			}));
		}
	}

	private IEnumerator WaitAndInvokeAction(int frameWait, Action action)
	{
		for (int i = 0; i < frameWait; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}

	public void SlotExit(UISpellDisableSlot slot)
	{
		slot.Unhover();
		uiInfoSpell.gameObject.SetActive(value: false);
	}

	public void SlotClick(UISpellDisableSlot slot, bool playSE = true, CostType? autoCost = null)
	{
		if (!slot.UnLocked)
		{
			return;
		}
		if (GameMgr.IsMobile_Static && ControlMgr.Inst.usingTouchScreen)
		{
			if (playSE)
			{
				if (selectedSlot != slot)
				{
					selectedSlot?.OnPointerExit(null);
					selectedSlot = slot;
					slot.OnPointerEnter(null);
					return;
				}
			}
			else
			{
				selectedSlot?.OnPointerExit(null);
			}
		}
		selectedSlot = slot;
		if (slot.AlreadyDisable)
		{
			slot.SetEnable();
			if (costTypes.Count > 0)
			{
				if (disableCounter > finalFreeDisableCount)
				{
					switch (costTypes[costTypes.Count - 1])
					{
					case CostType.Crystal:
					{
						int num2 = (disableCounter - finalFreeDisableCount) * disableCostCrystalPerCount;
						PlayerMgr.Inst.ChangeMagicCrystal(num2);
						DataMgr.selectedWorldData.spellDisableCost_Crystal2 -= num2;
						break;
					}
					case CostType.Blood:
					{
						int num = (disableCounter - finalFreeDisableCount) * disableCostBloodPerCount;
						PlayerMgr.Inst.ChangeAncientBlood(num);
						DataMgr.selectedWorldData.spellDisableCost_Blood2 -= num;
						break;
					}
					default:
						Debug.LogError(costTypes[costTypes.Count - 1]);
						break;
					}
				}
				TryAddSpellAllLevelToPool(slot.Level1ID);
				PlayerMgr.Inst.BaData.CheckSpellPoolHaveRarity();
				costTypes.RemoveAt(costTypes.Count - 1);
				DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Remove(slot.Level1ID);
				disableCounter--;
				UpdateResidualCount();
				if (playSE)
				{
					SEMgr.Inst.uiTalentReset.PlaySE();
				}
			}
			else
			{
				Debug.LogError("Ϊʲôû\ufffdл\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdˣ\ufffd");
			}
		}
		else
		{
			if (disableCounter >= finalMaxDisableCount)
			{
				anima_ResidualCount.Play("Shake", 0, 0f);
				if (playSE)
				{
					SEMgr.Inst.uiResearchWrong.PlaySE();
				}
				return;
			}
			if (disableCounter < finalFreeDisableCount)
			{
				PlayerMgr.Inst.BaData.RemoveSpellAllLevelFromPool(slot.Level1ID);
				PlayerMgr.Inst.BaData.CheckSpellPoolHaveRarity();
				disableCounter++;
				slot.SetDisable();
				UpdateResidualCount();
				costTypes.Add(CostType.Crystal);
				DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Add(slot.Level1ID);
				if (playSE)
				{
					SEMgr.Inst.uiSpellDisable_Succeed.PlaySE();
				}
			}
			else if (!autoCost.HasValue)
			{
				SpellConfig spellConfig = SpellConfig.dic[slot.Level1ID];
				panel_Confirm.SetActive(value: true);
				image_ConfirmSpellIcon.sprite = ABResources.LoadAsset<Sprite>(spellConfig.GetIconPath());
				int num3 = (disableCounter - finalFreeDisableCount + 1) * disableCostCrystalPerCount;
				int num4 = (disableCounter - finalFreeDisableCount + 1) * disableCostBloodPerCount;
				text_CrystalCost.text = num3.ToString();
				text_BloodCost.text = num4.ToString();
				if (DataMgr.selectedWorldData.magicCrystalCount >= num3)
				{
					btn_Crystal.interactable = true;
					text_CrystalCost.color = Color.green;
				}
				else
				{
					btn_Crystal.interactable = false;
					text_CrystalCost.color = Color.red;
				}
				if (DataMgr.selectedWorldData.ancientBloodCount >= num4)
				{
					btn_Blood.interactable = true;
					text_BloodCost.color = Color.green;
				}
				else
				{
					btn_Blood.interactable = false;
					text_BloodCost.color = Color.red;
				}
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					ControllerCrystalSelectFrame.SetActive(value: true);
					uiInfoSpell.gameObject.SetActive(value: false);
				}
				if (playSE)
				{
					SEMgr.Inst.uiClick.PlaySE();
				}
			}
			else if (autoCost.Value == CostType.Blood)
			{
				DisableBlood(playSe: false);
			}
			else
			{
				DisableCrystal(playSe: false);
			}
		}
		if (UIMgr.Inst.UIMenu.uiGallery != null && UIMgr.Inst.UIMenu.uiGallery.init)
		{
			UIMgr.Inst.UIMenu.uiGallery.slotInited[2] = false;
		}
		if (GameMgr.IsMobile_Static && !playSE)
		{
			ExitSelected();
		}
	}

	public void BanSpellDirect(UISpellDisableSlot slot)
	{
		slot.SetDisable();
	}

	public void saveSelectedDisable()
	{
		DataMgr.selectedWorldData.spellDisableFreeIDs3.Clear();
		for (int i = 0; i < DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Count && i < finalFreeDisableCount; i++)
		{
			DataMgr.selectedWorldData.spellDisableFreeIDs3.Add(DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle[i]);
		}
	}

	private void TryAddSpellAllLevelToPool(int level1Id)
	{
		if ((level1Id != 10281 || DataMgr.selectedWorldData.selectedSetID == 9) && (level1Id != 10311 || DataMgr.selectedWorldData.selectedSetID == 11))
		{
			PlayerMgr.Inst.BaData.AddSpellAllLevelToPool(level1Id);
		}
	}

	protected override void OnHide()
	{
		anima_Self.Play("Hide");
		UIMgr.TryAdditionalMobileHide(base.transform);
		uiInfoSpell.gameObject.SetActive(value: false);
		CamController.Inst.MouseOffsetContinue();
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		TimeScaleMgr.Inst.Recovery();
		SEMgr.Inst.uiClose.PlaySE();
		saveSelectedDisable();
		ExitSelected();
		DataMgr.selectedWorldData.battleData9.RandomSpellWandCheck();
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Crystal);
	}

	public override void _Close()
	{
		SEMgr.Inst.uiClick.PlaySE();
		Hide();
	}

	public void _ConfirmClose(bool playClickSE = false)
	{
		panel_Confirm.SetActive(value: false);
		ControllerCrystalSelectFrame.SetActive(value: false);
		ControllerBloodSelectFrame.SetActive(value: false);
		if (playClickSE)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
	}

	public void _DisableCrystal()
	{
		DisableCrystal();
	}

	public void DisableCrystal(bool playSe = true)
	{
		int num = 0;
		if (disableCounter >= finalFreeDisableCount)
		{
			num = (disableCounter - finalFreeDisableCount + 1) * disableCostCrystalPerCount;
		}
		if (DataMgr.selectedWorldData.magicCrystalCount >= num)
		{
			PlayerMgr.Inst.ChangeMagicCrystal(-num);
			PlayerMgr.Inst.BaData.RemoveSpellAllLevelFromPool(selectedSlot.Level1ID);
			PlayerMgr.Inst.BaData.CheckSpellPoolHaveRarity();
			DataMgr.selectedWorldData.spellDisableCost_Crystal2 += num;
			disableCounter++;
			selectedSlot.SetDisable();
			_ConfirmClose();
			UpdateResidualCount();
			costTypes.Add(CostType.Crystal);
			DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Add(selectedSlot.Level1ID);
			if (playSe)
			{
				SEMgr.Inst.uiSpellDisable_Succeed.PlaySE();
			}
		}
	}

	public void _DisableBlood()
	{
		DisableBlood();
	}

	public void DisableBlood(bool playSe = true)
	{
		int num = 0;
		if (disableCounter >= finalFreeDisableCount)
		{
			num = (disableCounter - finalFreeDisableCount + 1) * disableCostBloodPerCount;
		}
		if (DataMgr.selectedWorldData.ancientBloodCount >= num)
		{
			PlayerMgr.Inst.ChangeAncientBlood(-num);
			PlayerMgr.Inst.BaData.RemoveSpellAllLevelFromPool(selectedSlot.Level1ID);
			PlayerMgr.Inst.BaData.CheckSpellPoolHaveRarity();
			DataMgr.selectedWorldData.spellDisableCost_Blood2 += num;
			disableCounter++;
			selectedSlot.SetDisable();
			_ConfirmClose();
			UpdateResidualCount();
			costTypes.Add(CostType.Blood);
			DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Add(selectedSlot.Level1ID);
			if (playSe)
			{
				SEMgr.Inst.uiSpellDisable_Succeed.PlaySE();
			}
		}
	}

	public void SwitchSortingType()
	{
		if (base.IsOpen)
		{
			ExitSelected();
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
			gamepadSelectedSlotIndex = 0;
			gamepadSelectedSlotType = 0;
			saveSelectedDisable();
			Init(switchSorlt: true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rtsfVerticalLayoutGroup);
			scrollbarVertical.value = 1f;
		}
	}
}
