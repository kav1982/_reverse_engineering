using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIGallery")]
public class UIGallery : GameUI
{
	public enum galleryType
	{
		Menu,
		GalleryTable
	}

	public galleryType type = galleryType.GalleryTable;

	public GameObject rootObj;

	public CanvasGroup CanvasGroup;

	public GameObject SetActive;

	public List<GameObject> goToggleSelected;

	public List<GameObject> goToggleSpellSelected;

	public GameObject pfb_UIGallerySlot;

	public List<GalleryContains> galleryContains;

	public Text text_StatisticsMonster;

	public Text text_StatisticsBoss;

	public Text text_StatisticsWand;

	public Text text_StatisticsSpell;

	public Text text_StatisticsRelic;

	public Text text_StatisticsPotion;

	public Text text_StatisticsCurse;

	public Animator anima;

	[Header("Monster")]
	public Image image_MonsterIcon;

	public Text text_MonsterName;

	public Text text_MonsterDesc;

	[Header("Boss")]
	public Image image_BossIcon;

	public Text text_BossName;

	public Text text_BossDesc;

	[Header("Wand")]
	public Image image_WandIcon;

	public Text text_WandName;

	public Text text_MaxMp;

	public Text text_ShootInterval;

	public Text text_mpRecovery;

	public Text text_CD;

	public Text text_info;

	public GameObject gameobject_Line1;

	public GameObject gameobject_Line2;

	public GameObject gameobject_normalSlotContent;

	public GameObject gameobject_postSlotContent;

	public GameObject content;

	public UISlotWandExternal ui_gamepadcontrolshow;

	[Header("Spell")]
	public Text text_SwitchTextLevel;

	public Image image_SpellIcon;

	public Text text_SpellName;

	public Text text_SpellUseType;

	public Text text_SpellRarity;

	public Text text_SpellInfo;

	public Image image_SpellCost;

	public Text text_SpellCost;

	public Text text_SpellAdditionInfo;

	public Text text_SpellAssistDesc;

	public Toggle toggle_SpellLevel1;

	public Toggle toggle_SpellLevel2;

	public Toggle toggle_SpellLevel3;

	public GameObject spellLine1;

	[Header("Relic")]
	public Image image_RelicIcon;

	public Text text_RelicName;

	public Text text_RelicUseType;

	public Text text_RelicRarity;

	public Text text_RelicInfo;

	public GameObject image_RelicAdditionInfoDelimiter;

	public Text text_RelicAdditionInfo;

	[Header("Potion")]
	public Image image_PotionIcon;

	public Text text_PotionName;

	public Text text_PotionInfo;

	[Header("Curse")]
	public Image image_CurseIcon;

	public Text text_CurseName;

	public Text text_CurseUseType;

	public Text text_CurseRarity;

	public Text text_CurseInfo;

	[Header("LanguageChange")]
	public Text text_MonsterHP;

	public Text text_MonsterKilled;

	public Text text_BossHP;

	public Text text_BossKilled;

	public Text text_RelicMaxLevel;

	public Text text_CurseMaxLevel;

	public Text text_RelicGetTime;

	public Text text_PotionUseTime;

	public Text text_CurseGetTime;

	[Header("Gamepad")]
	public int widthCount;

	public int heightCount;

	public Toggle[] toggles;

	public Transform[] tsf_Contents;

	public Custom_ScrollRect[] scrollRects;

	public UpdatButtonShow[] updatebuttonshows;

	public GameObject gamepadcontrolshow;

	public GameObject KeySwitchControlShow;

	private List<UIGallerySlot> monsterSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> bossSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> spellSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> wandSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> RelicSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> potionSlots = new List<UIGallerySlot>();

	private List<UIGallerySlot> curseSlots = new List<UIGallerySlot>();

	private UIGallerySlot _monsterSlot;

	private UIGallerySlot _bossSlot;

	private UIGallerySlot _spellSlot;

	private UIGallerySlot _wandSlot;

	private UIGallerySlot _relicSlot;

	private UIGallerySlot _potionSlot;

	private UIGallerySlot _curseSlot;

	private List<UnitConfig> _unityListSorted;

	private int _onToggleIndex;

	private bool scrolldown;

	private int[] gamepadSelectedSlotIndexs = new int[7];

	private int[] gamepadSelectedcategoryIndexs = new int[7];

	private UIGallerySlot[] hoveredSlots;

	private bool[] slotLoaded;

	public bool needReload { get; set; }

	public bool[] slotInited { get; set; }

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Drink.performed += GamepaDrinkPerformed;
		if (type == galleryType.GalleryTable)
		{
			base.inputActions.Player.GamepadLB.performed += GamepadLBPerformed;
			base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
			base.inputActions.Player.WASD.performed += WASDPerformed;
		}
		else
		{
			base.inputActions.Player.GamepadLT.performed += GamepadLBPerformed;
			base.inputActions.Player.GamepadRT.performed += GamepadRBPerformed;
			base.inputActions.Player.KeyboardQ.performed += QPerformed;
			base.inputActions.Player.KeyboardE.performed += EPerformed;
		}
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		if (type == galleryType.GalleryTable)
		{
			base.inputActions.Player.GamepadLB.performed -= GamepadLBPerformed;
			base.inputActions.Player.GamepadRB.performed -= GamepadRBPerformed;
			base.inputActions.Player.WASD.performed -= WASDPerformed;
		}
		else
		{
			base.inputActions.Player.GamepadLT.performed -= GamepadLBPerformed;
			base.inputActions.Player.GamepadRT.performed -= GamepadRBPerformed;
			base.inputActions.Player.KeyboardQ.performed -= QPerformed;
			base.inputActions.Player.KeyboardE.performed -= EPerformed;
		}
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Drink.performed -= GamepaDrinkPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void GamepaDrinkPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen && GetListPanel(GalleryCategory.Spell).gameObject.activeSelf)
		{
			SEMgr.Inst.uiSwitch.PlaySE();
			if (toggle_SpellLevel1.isOn)
			{
				toggle_SpellLevel2.isOn = true;
			}
			else if (toggle_SpellLevel2.isOn)
			{
				toggle_SpellLevel3.isOn = true;
			}
			else if (toggle_SpellLevel3.isOn)
			{
				toggle_SpellLevel1.isOn = true;
			}
		}
	}

	private void GamepadLBPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			SwitchLeft();
		}
	}

	private void GamepadRBPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			SwitchRight();
		}
	}

	private void EPerformed(InputAction.CallbackContext obj)
	{
		if (base.IsOpen)
		{
			SwitchRight();
		}
	}

	private void QPerformed(InputAction.CallbackContext obj)
	{
		if (base.IsOpen)
		{
			SwitchLeft();
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen)
		{
			return;
		}
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].isOn)
			{
				_onToggleIndex = i;
				break;
			}
		}
		Vector2 direct = context.ReadValue<Vector2>();
		Directioninput(direct);
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !base.IsOpen)
		{
			return;
		}
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].isOn)
			{
				_onToggleIndex = i;
				break;
			}
		}
		Vector2 vector = context.ReadValue<Vector2>();
		vector = ControlMgr.Inst.RampVector2(vector, ControlMgr.rampType.FourDirectionNotPrecise);
		Directioninput(vector);
	}

	private void Directioninput(Vector2 _direct)
	{
		int num = gamepadSelectedSlotIndexs[_onToggleIndex];
		int num2 = gamepadSelectedcategoryIndexs[_onToggleIndex];
		CustomNavTool.Nav(_direct, ref gamepadSelectedSlotIndexs[_onToggleIndex], ref gamepadSelectedcategoryIndexs[_onToggleIndex], galleryContains[_onToggleIndex].GridLayoutGroups, widthCount);
		GeneralTool.ScrollToPadSelected(galleryContains[_onToggleIndex].PanelLists.GetComponentInChildren<ScrollRect>(), galleryContains[_onToggleIndex].ContentRoot.GetComponent<RectTransform>(), galleryContains[_onToggleIndex].GridLayoutGroups[gamepadSelectedcategoryIndexs[_onToggleIndex]].transform.GetChild(gamepadSelectedSlotIndexs[_onToggleIndex]).GetComponent<RectTransform>());
		if (num != gamepadSelectedSlotIndexs[_onToggleIndex] || num2 != gamepadSelectedcategoryIndexs[_onToggleIndex])
		{
			SlotEnter(galleryContains[_onToggleIndex].GridLayoutGroups[gamepadSelectedcategoryIndexs[_onToggleIndex]].transform.GetChild(gamepadSelectedSlotIndexs[_onToggleIndex]).GetComponent<UIGallerySlot>());
		}
	}

	private void WASDPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			if (vector == Vector2.left)
			{
				SwitchLeft();
			}
			else if (vector == Vector2.right)
			{
				SwitchRight();
			}
		}
	}

	private void SwitchLeft()
	{
		for (int num = toggles.Length - 1; num >= 0; num--)
		{
			if (toggles[num].isOn)
			{
				if (num == 0)
				{
					toggles[toggles.Length - 1].isOn = true;
				}
				else
				{
					toggles[num - 1].isOn = true;
				}
				break;
			}
		}
	}

	private void SwitchRight()
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].isOn)
			{
				if (i == toggles.Length - 1)
				{
					toggles[0].isOn = true;
				}
				else
				{
					toggles[i + 1].isOn = true;
				}
				break;
			}
		}
	}

	private void SwitchSpellSort()
	{
		GalleryContains obj = galleryContains.First((GalleryContains x) => x.galleryType == GalleryCategory.Spell);
		obj.groupType = ((obj.groupType == GalleryContains.GalleryGroupType.SpellType) ? GalleryContains.GalleryGroupType.Rare : GalleryContains.GalleryGroupType.SpellType);
	}

	private void LanguageChange()
	{
		text_SwitchTextLevel.text = 1000413.GetText() + ": ";
		if (base.init && (bool)tsf_Contents[_onToggleIndex] && tsf_Contents[_onToggleIndex].childCount >= gamepadSelectedSlotIndexs[_onToggleIndex] + 1)
		{
			Transform child = tsf_Contents[_onToggleIndex].GetChild(gamepadSelectedSlotIndexs[_onToggleIndex]);
			if ((bool)child && (bool)child.GetComponent<Text>())
			{
				child.GetComponent<Text>().text = 1000413.GetText();
			}
		}
		if (_monsterSlot != null)
		{
			_monsterSlot.OnPointerEnter(null);
		}
		if (_bossSlot != null)
		{
			_bossSlot.OnPointerEnter(null);
		}
		if (_spellSlot != null)
		{
			_spellSlot.OnPointerEnter(null);
		}
		if (_wandSlot != null)
		{
			_wandSlot.OnPointerEnter(null);
		}
		if (_relicSlot != null)
		{
			_relicSlot.OnPointerEnter(null);
		}
		if (_potionSlot != null)
		{
			_potionSlot.OnPointerEnter(null);
		}
		if (_curseSlot != null)
		{
			_curseSlot.OnPointerEnter(null);
		}
		galleryContains.ForEach(delegate(GalleryContains x)
		{
			x.UpdateLanguate();
		});
	}

	private void InputChange()
	{
		UpdatButtonShow[] array = updatebuttonshows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			gamepadcontrolshow.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
		{
			gamepadcontrolshow.SetActive(value: true);
			if (hoveredSlots == null)
			{
				break;
			}
			for (int j = 0; j < hoveredSlots.Length; j++)
			{
				if (hoveredSlots[j] != null)
				{
					gamepadSelectedSlotIndexs[j] = hoveredSlots[j].transform.GetSiblingIndex();
				}
			}
			break;
		}
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	protected override IEnumerator OnInit()
	{
		_unityListSorted = UnitConfig.ShallowCopyListSorted();
		galleryContains.ForEach(delegate(GalleryContains x)
		{
			x.Init();
		});
		LanguageChange();
		CreateUIIE();
		yield return null;
	}

	public void ShowInitFromMenu()
	{
		if (needReload || !base.init)
		{
			UIMgr.Inst.UIMenu.GalleryDot.enabled = false;
			DataMgr.selectedWorldData.MenuGalleryDot = false;
			ShowInit();
		}
		else
		{
			Debug.Log("ShowInitFromMenu");
			UIMgr.Inst.UIMenu.GalleryDot.enabled = false;
			DataMgr.selectedWorldData.MenuGalleryDot = false;
			Show();
		}
	}

	public void ShowInitFromIneract()
	{
		ShowInit();
	}

	private void CreateUIIE()
	{
		rootObj.SetActive(value: true);
		anima.enabled = false;
		CanvasGroup.alpha = 0f;
		hoveredSlots = new UIGallerySlot[toggles.Length];
		gamepadSelectedSlotIndexs = new int[toggles.Length];
		slotLoaded = new bool[toggles.Length];
		slotInited = new bool[toggles.Length];
		CanvasGroup.alpha = 1f;
		Toggle[] array = toggles;
		foreach (Toggle toggle in array)
		{
			if (toggle.isOn)
			{
				toggle.onValueChanged.Invoke(arg0: true);
			}
		}
		needReload = false;
		InputChange();
		anima.enabled = true;
	}

	private void loadMonsterBossSlots()
	{
		slotLoaded[0] = true;
		slotLoaded[1] = true;
		monsterSlots.Clear();
		bossSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Monster).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		GetGridlayoutGroups(GalleryCategory.Boss).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		foreach (UnitConfig item in _unityListSorted)
		{
			int index = Mathf.Clamp((int)(item.appearChapter / 10f) - 1, 0, 4);
			if (item.inGallery && item.appearChapter < 500f)
			{
				if (item.unitType == UnitType.Monster || item.unitType == UnitType.WillAttack || item.unitType == UnitType.NotAttack || item.unitType == UnitType.Brittleness)
				{
					UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Monster)[index].transform).GetComponent<UIGallerySlot>();
					monsterSlots.Add(component);
				}
				else if (item.unitType == UnitType.Elite)
				{
					UIGallerySlot component2 = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Boss)[index].transform).GetComponent<UIGallerySlot>();
					bossSlots.Add(component2);
					component2.Initialize(this, GalleryCategory.Boss, item.id);
				}
			}
		}
		foreach (UnitConfig item2 in _unityListSorted)
		{
			int index2 = Mathf.Clamp((int)(item2.appearChapter / 10f) - 1, 0, 4);
			if (item2.inGallery && item2.appearChapter < 500f && item2.unitType == UnitType.Boss && (item2.id != 509901 || DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(item2.id)))
			{
				UIGallerySlot component3 = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Boss)[index2].transform).GetComponent<UIGallerySlot>();
				bossSlots.Add(component3);
			}
		}
	}

	private void loadSpellSlots()
	{
		slotLoaded[2] = true;
		spellSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Spell).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		foreach (SpellConfig item in SpellConfig.list)
		{
			if (item.level == 1 && item.dropType != 0)
			{
				UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Spell)[(int)item.useType].transform).GetComponent<UIGallerySlot>();
				spellSlots.Add(component);
			}
		}
	}

	private void loadWandSlots()
	{
		slotLoaded[3] = true;
		wandSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Wand).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		foreach (WandConfig item in WandConfig.list)
		{
			if (1 <= item.dropStage && item.dropStage <= 20)
			{
				UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Wand)[item.dropStage / 2].transform).GetComponent<UIGallerySlot>();
				wandSlots.Add(component);
			}
		}
	}

	private void loadRelicSlots()
	{
		slotLoaded[4] = true;
		RelicSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Relic).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		foreach (RelicConfig item in RelicConfig.list.Where((RelicConfig t) => t.dropType != ItemDropType.None))
		{
			UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Relic)[(int)(item.dropType - 1)].transform).GetComponent<UIGallerySlot>();
			RelicSlots.Add(component);
		}
	}

	private void loadPotionSlots()
	{
		slotLoaded[5] = true;
		potionSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Potion).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		for (int num = PotionConfig.list.Count - 1; num >= 0; num--)
		{
			UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Potion)[0].transform).GetComponent<UIGallerySlot>();
			potionSlots.Add(component);
		}
	}

	private void loadCurseSlots()
	{
		slotLoaded[6] = true;
		curseSlots.Clear();
		GetGridlayoutGroups(GalleryCategory.Curse).ForEach(delegate(LayoutGroup x)
		{
			x.transform.DestroyAllChildImmediate();
		});
		foreach (CurseConfig item in CurseConfig.list)
		{
			if (item.dropType != 0)
			{
				UIGallerySlot component = UnityEngine.Object.Instantiate(pfb_UIGallerySlot, GetGridlayoutGroups(GalleryCategory.Curse)[(int)(item.dropType - 1)].transform).GetComponent<UIGallerySlot>();
				curseSlots.Add(component);
			}
		}
	}

	private void UpdateSpellSlotBan(UIGallerySlot _slots, int i)
	{
		if (type != 0)
		{
			return;
		}
		if ((bool)BattleMgr.Inst)
		{
			if (DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle.Contains(SpellConfig.list[i].id))
			{
				_slots.SpellBaned.gameObject.SetActive(value: true);
			}
			else
			{
				_slots.SpellBaned.gameObject.SetActive(value: false);
			}
		}
		else
		{
			_slots.SpellBaned.gameObject.SetActive(value: false);
		}
	}

	private void UpdateMonsterBossEliteSlots()
	{
		slotInited[0] = true;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < _unityListSorted.Count; i++)
		{
			UnitConfig unitConfig = _unityListSorted[i];
			if (!unitConfig.inGallery || !(unitConfig.appearChapter < 500f))
			{
				continue;
			}
			if (unitConfig.unitType == UnitType.Monster || unitConfig.unitType == UnitType.WillAttack || unitConfig.unitType == UnitType.NotAttack)
			{
				monsterSlots[num].Initialize(this, GalleryCategory.Monster, unitConfig.id);
				num++;
				if (DataMgr.selectedWorldData.galleryUnlockedMonsters.Contains(unitConfig.id))
				{
					num3++;
				}
			}
			else if (unitConfig.unitType == UnitType.Elite)
			{
				bossSlots[num2].Initialize(this, GalleryCategory.Boss, unitConfig.id);
				num2++;
				if (DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(unitConfig.id))
				{
					num4++;
				}
			}
		}
		for (int j = 0; j < _unityListSorted.Count; j++)
		{
			UnitConfig unitConfig2 = _unityListSorted[j];
			if (unitConfig2.inGallery && unitConfig2.appearChapter < 500f && unitConfig2.unitType == UnitType.Boss && (unitConfig2.id != 509901 || DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(unitConfig2.id)))
			{
				bossSlots[num2].Initialize(this, GalleryCategory.Boss, unitConfig2.id);
				num2++;
				if (DataMgr.selectedWorldData.galleryUnlockedBosses.Contains(unitConfig2.id))
				{
					num4++;
				}
			}
		}
		monsterSlots[0].OnPointerEnter(null);
		bossSlots[0].OnPointerEnter(null);
		text_StatisticsMonster.text = num3 + "/" + num;
		text_StatisticsBoss.text = num4 + "/" + num2;
	}

	private void UpdateWandSlots()
	{
		slotInited[3] = true;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < WandConfig.list.Count; i++)
		{
			if (1 <= WandConfig.list[i].dropStage && WandConfig.list[i].dropStage <= 20)
			{
				wandSlots[num].Initialize(this, GalleryCategory.Wand, WandConfig.list[i].id);
				num++;
				if (DataMgr.selectedWorldData.galleryUnlockedWands.Contains(WandConfig.list[i].id))
				{
					num2++;
				}
			}
		}
		wandSlots[0].OnPointerEnter(null);
		text_StatisticsWand.text = num2 + "/" + num;
	}

	private void UpdateRelicSlots()
	{
		slotInited[4] = true;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < RelicConfig.list.Count; i++)
		{
			if (RelicConfig.list[i].dropType != 0)
			{
				RelicSlots[num].Initialize(this, GalleryCategory.Relic, RelicConfig.list[i].id);
				num++;
				if (DataMgr.selectedWorldData.galleryUnlockedRelics.Contains(RelicConfig.list[i].id))
				{
					num2++;
				}
			}
		}
		RelicSlots[0].OnPointerEnter(null);
		text_StatisticsRelic.text = num2 + "/" + num;
	}

	private void UpdatePotionSlots()
	{
		slotInited[5] = true;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < PotionConfig.list.Count; i++)
		{
			potionSlots[num].Initialize(this, GalleryCategory.Potion, PotionConfig.list[i].id);
			num++;
			if (DataMgr.selectedWorldData.galleryUnlockedPotions.Contains(PotionConfig.list[i].id))
			{
				num2++;
			}
		}
		potionSlots[0].OnPointerEnter(null);
		text_StatisticsPotion.text = num2 + "/" + num;
	}

	private void UpdateCurseSlots()
	{
		slotInited[6] = true;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < CurseConfig.list.Count; i++)
		{
			if (CurseConfig.list[i].dropType != 0)
			{
				curseSlots[num].Initialize(this, GalleryCategory.Curse, CurseConfig.list[i].id);
				num++;
				if (DataMgr.selectedWorldData.galleryUnlockedCurses.Contains(CurseConfig.list[i].id))
				{
					num2++;
				}
			}
		}
		curseSlots[0].OnPointerEnter(null);
		text_StatisticsCurse.text = num2 + "/" + num;
	}

	private void UpdateSpellSlots()
	{
		slotInited[2] = true;
		int num = 0;
		for (int i = 0; i < SpellConfig.list.Count; i++)
		{
			if (SpellConfig.list[i].level == 1 && SpellConfig.list[i].dropType != 0)
			{
				spellSlots[num].Initialize(this, GalleryCategory.Spell, SpellConfig.list[i].id);
				UpdateSpellSlotBan(spellSlots[num], i);
				num++;
			}
		}
		spellSlots[0].OnPointerEnter(null);
		_SpellToggleValueChangeFromStart();
	}

	private void UpdateLayOut(GalleryCategory category)
	{
		galleryContains.ForEach(delegate(GalleryContains x)
		{
			x.PanelLists.SetActive(value: false);
		});
		galleryContains.ForEach(delegate(GalleryContains x)
		{
			x.PanelInfos.SetActive(value: false);
		});
		GetListPanel(category).SetActive(value: true);
		GetInfoPanel(category).SetActive(value: true);
		GetConstSizeFiltersGroup(category).ForEach(delegate(ContentSizeFitter x)
		{
			x.enabled = true;
			LayoutRebuilder.MarkLayoutForRebuild(x.transform as RectTransform);
		});
		GetGridlayoutGroups(category).ForEach(delegate(LayoutGroup x)
		{
			x.enabled = true;
			LayoutRebuilder.MarkLayoutForRebuild(x.transform as RectTransform);
		});
		LayoutRebuilder.MarkLayoutForRebuild(GetContentRoot(category).transform as RectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(GetContentRoot(category).transform as RectTransform);
	}

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.uiChangeLabel.PlaySE();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		InputChange();
		UpdateCurrentInfoLayout();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		if (type == galleryType.GalleryTable)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.PlayerCtrller.StopFace(PlayerMgr.Inst.PlayerDir.x < 0f);
		}
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		if (type == galleryType.GalleryTable)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
	}

	public void UpdateCurrentInfoLayout()
	{
		for (int i = 0; i < goToggleSelected.Count; i++)
		{
			if (!goToggleSelected[i].gameObject.activeSelf)
			{
				continue;
			}
			switch (i)
			{
			case 0:
				if (!slotLoaded[0])
				{
					loadMonsterBossSlots();
				}
				if (!slotInited[0])
				{
					UpdateMonsterBossEliteSlots();
				}
				UpdateLayOut(GalleryCategory.Monster);
				if (_monsterSlot != null)
				{
					_monsterSlot.OnPointerEnter(null);
				}
				break;
			case 1:
				if (!slotLoaded[1])
				{
					loadMonsterBossSlots();
				}
				if (!slotInited[1])
				{
					UpdateMonsterBossEliteSlots();
				}
				UpdateLayOut(GalleryCategory.Boss);
				if (_bossSlot != null)
				{
					_bossSlot.OnPointerEnter(null);
				}
				break;
			case 2:
				if (!slotLoaded[2])
				{
					loadSpellSlots();
				}
				if (!slotInited[2])
				{
					UpdateSpellSlots();
				}
				UpdateLayOut(GalleryCategory.Spell);
				if (_spellSlot != null)
				{
					_spellSlot.OnPointerEnter(null);
				}
				break;
			case 3:
				if (!slotLoaded[3])
				{
					loadWandSlots();
				}
				if (!slotInited[3])
				{
					UpdateWandSlots();
				}
				UpdateLayOut(GalleryCategory.Wand);
				if (_wandSlot != null)
				{
					_wandSlot.OnPointerEnter(null);
				}
				break;
			case 4:
				if (!slotLoaded[4])
				{
					loadRelicSlots();
				}
				if (!slotInited[4])
				{
					UpdateRelicSlots();
				}
				UpdateLayOut(GalleryCategory.Relic);
				if (_relicSlot != null)
				{
					_relicSlot.OnPointerEnter(null);
				}
				break;
			case 5:
				if (!slotLoaded[5])
				{
					loadPotionSlots();
				}
				if (!slotInited[5])
				{
					UpdatePotionSlots();
				}
				UpdateLayOut(GalleryCategory.Potion);
				if (_potionSlot != null)
				{
					_potionSlot.OnPointerEnter(null);
				}
				break;
			case 6:
				if (!slotLoaded[6])
				{
					loadCurseSlots();
				}
				if (!slotInited[6])
				{
					UpdateCurseSlots();
				}
				UpdateLayOut(GalleryCategory.Curse);
				if (_curseSlot != null)
				{
					_curseSlot.OnPointerEnter(null);
				}
				break;
			}
		}
	}

	public void SlotEnter(UIGallerySlot slot)
	{
		if (slot == null)
		{
			return;
		}
		if (hoveredSlots[(int)slot.Category] != null)
		{
			hoveredSlots[(int)slot.Category].Unhover();
		}
		hoveredSlots[(int)slot.Category] = slot;
		hoveredSlots[(int)slot.Category].Hover();
		switch (slot.Category)
		{
		case GalleryCategory.Monster:
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			image_MonsterIcon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[slot.Level1ID].GetModelPath());
			_monsterSlot = slot;
			if (slot.IsLocked)
			{
				image_MonsterIcon.color = Color.black;
				text_MonsterName.text = "???";
				text_MonsterKilled.gameObject.SetActive(value: false);
				text_MonsterHP.text = "";
				text_MonsterKilled.gameObject.SetActive(value: false);
				text_MonsterDesc.text = "";
				break;
			}
			image_MonsterIcon.color = Color.white;
			text_MonsterName.text = UnitConfig.map[slot.Level1ID].GetName();
			if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
			{
				text_MonsterName.text += $"({slot.Level1ID})";
			}
			text_MonsterKilled.gameObject.SetActive(value: true);
			text_MonsterKilled.text = 1000409.GetText() + ": ";
			text_MonsterHP.text = 1000407.GetText() + ": ";
			if (UnitConfig.map[slot.Level1ID].maxHP == 100000000f)
			{
				text_MonsterHP.text += "???";
			}
			else
			{
				text_MonsterHP.text += UnitConfig.map[slot.Level1ID].maxHP.ToString("F0");
			}
			if (DataMgr.selectedWorldData.galleryKilledMonsterCounts.ContainsKey(slot.Level1ID))
			{
				text_MonsterKilled.text += DataMgr.selectedWorldData.galleryKilledMonsterCounts[slot.Level1ID];
			}
			else
			{
				text_MonsterKilled.text += "0";
			}
			text_MonsterDesc.text = UnitConfig.map[slot.Level1ID].GetDesc();
			text_MonsterDesc.text = GeneralTool.FormatTextIfPublishTest(text_MonsterDesc, text_MonsterDesc.text);
			break;
		case GalleryCategory.Boss:
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			image_BossIcon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[slot.Level1ID].GetModelPath());
			_bossSlot = slot;
			if (slot.IsLocked)
			{
				image_BossIcon.color = Color.black;
				text_BossName.text = "???";
				text_BossKilled.text = "";
				text_BossDesc.text = "";
				text_BossHP.text = "";
			}
			else
			{
				image_BossIcon.color = Color.white;
				text_BossName.text = UnitConfig.map[slot.Level1ID].GetName();
				if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
				{
					text_BossName.text += $"({slot.Level1ID})";
				}
				text_BossHP.text = 1000407.GetText() + ": ";
				text_BossKilled.text = 1000409.GetText() + ": ";
				text_BossHP.text += UnitConfig.map[slot.Level1ID].maxHP.ToString("F0");
				if (DataMgr.selectedWorldData.galleryKilledBossCounts.ContainsKey(slot.Level1ID))
				{
					text_BossKilled.text += DataMgr.selectedWorldData.galleryKilledBossCounts[slot.Level1ID];
				}
				else
				{
					text_BossKilled.text += "0";
				}
				text_BossDesc.text = UnitConfig.map[slot.Level1ID].GetDesc();
			}
			text_BossDesc.text = GeneralTool.FormatTextIfPublishTest(text_BossDesc, text_BossDesc.text);
			break;
		case GalleryCategory.Spell:
		{
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[slot.Level1ID].GetIconPath());
			_spellSlot = slot;
			if (slot.IsLocked)
			{
				image_SpellIcon.color = Color.black;
				text_SpellUseType.text = "";
				text_SpellUseType.color = Color.black;
				text_SpellRarity.text = "";
				text_SpellRarity.color = Color.black;
				text_SpellName.text = "???";
				spellLine1.gameObject.SetActive(value: false);
				image_SpellCost.gameObject.SetActive(value: false);
				text_SpellCost.text = "";
				text_SpellAdditionInfo.text = "";
				text_SpellAssistDesc.text = "";
				text_SpellName.text = "";
				text_SpellInfo.text = "";
				break;
			}
			image_SpellIcon.color = Color.white;
			SpellConfig spellConfig;
			if (SpellConfig.dic[slot.Level1ID].dropType == ItemDropType.Common || SpellConfig.dic[slot.Level1ID].dropType == ItemDropType.Rare || slot.Level1ID == 10171)
			{
				if (toggle_SpellLevel1.isOn)
				{
					spellConfig = SpellConfig.dic[slot.Level1ID];
				}
				else if (toggle_SpellLevel2.isOn)
				{
					spellConfig = SpellConfig.dic[slot.Level1ID + 1];
				}
				else if (toggle_SpellLevel3.isOn)
				{
					spellConfig = ((slot.Level1ID != 40201) ? SpellConfig.dic[slot.Level1ID + 2] : SpellConfig.dic[slot.Level1ID]);
				}
				else
				{
					Debug.LogError("!");
					spellConfig = SpellConfig.dic[slot.Level1ID];
				}
			}
			else
			{
				spellConfig = SpellConfig.dic[slot.Level1ID];
			}
			switch (spellConfig.useType)
			{
			case SpellType.Missile:
				text_SpellUseType.text = 1002205.GetText();
				text_SpellUseType.color = GameConst.color_SpellUseTypeMissle;
				image_SpellCost.gameObject.SetActive(value: true);
				text_SpellCost.gameObject.SetActive(value: true);
				text_SpellCost.text = spellConfig.mpCost.ToString();
				break;
			case SpellType.Summon:
				text_SpellUseType.text = 1002206.GetText();
				text_SpellUseType.color = GameConst.color_SpellUseTypeMissle;
				image_SpellCost.gameObject.SetActive(value: true);
				text_SpellCost.gameObject.SetActive(value: true);
				text_SpellCost.text = spellConfig.mpCost.ToString();
				break;
			case SpellType.Enhance:
				text_SpellUseType.text = 1002207.GetText();
				text_SpellUseType.color = GameConst.color_SpellUseTypeEnhance;
				image_SpellCost.gameObject.SetActive(value: false);
				text_SpellCost.gameObject.SetActive(value: false);
				break;
			case SpellType.Passive:
				text_SpellUseType.text = 1002208.GetText();
				text_SpellUseType.color = GameConst.color_SpellUseTypePassive;
				image_SpellCost.gameObject.SetActive(value: false);
				text_SpellCost.gameObject.SetActive(value: false);
				break;
			default:
				Debug.LogError(spellConfig.useType);
				break;
			}
			switch (spellConfig.dropType)
			{
			case ItemDropType.None:
				text_SpellRarity.text = "???";
				text_SpellRarity.color = Color.black;
				break;
			case ItemDropType.Common:
				text_SpellRarity.text = 1001601.GetText();
				text_SpellRarity.color = GameConst.color_RarityCommon;
				break;
			case ItemDropType.Rare:
				text_SpellRarity.text = 1001602.GetText();
				text_SpellRarity.color = GameConst.color_RarityRare;
				break;
			case ItemDropType.Epic:
				text_SpellRarity.text = 1001603.GetText();
				text_SpellRarity.color = GameConst.color_RarityEpic;
				break;
			case ItemDropType.Special:
				text_SpellRarity.text = 1001604.GetText();
				text_SpellRarity.color = GameConst.color_RaritySpecial;
				break;
			default:
				Debug.LogError(spellConfig.dropType);
				break;
			}
			text_SpellName.text = spellConfig.GetName();
			text_SpellInfo.text = spellConfig.GetInfo(1f, "◆\u00a0\u200a");
			string des = spellConfig.GetDes(1f, "◆\u00a0\u200a", "", "◆\u00a0\u200a");
			text_SpellInfo.text = GeneralTool.FormatTextIfPublishTest(text_SpellInfo, text_SpellInfo.text);
			text_SpellAdditionInfo.text = des;
			text_SpellAdditionInfo.text = GeneralTool.FormatTextIfPublishTest(text_SpellAdditionInfo, text_SpellAdditionInfo.text);
			text_SpellAssistDesc.text = spellConfig.GetAssistDesc();
			text_SpellAssistDesc.text = GeneralTool.FormatTextIfPublishTest(text_SpellAssistDesc, text_SpellAssistDesc.text);
			if (string.IsNullOrEmpty(text_SpellInfo.text))
			{
				text_SpellInfo.gameObject.SetActive(value: false);
			}
			else
			{
				text_SpellInfo.gameObject.SetActive(value: true);
			}
			if (string.IsNullOrEmpty(text_SpellAdditionInfo.text))
			{
				text_SpellAdditionInfo.gameObject.SetActive(value: false);
			}
			else
			{
				text_SpellAdditionInfo.gameObject.SetActive(value: true);
			}
			if (!string.IsNullOrEmpty(text_SpellInfo.text) && !string.IsNullOrEmpty(text_SpellAdditionInfo.text))
			{
				spellLine1.SetActive(value: true);
			}
			else
			{
				spellLine1.SetActive(value: false);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(text_SpellInfo.transform.parent.GetComponent<RectTransform>());
			break;
		}
		case GalleryCategory.Wand:
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			StartCoroutine(WandInfoUpdate(slot));
			break;
		case GalleryCategory.Relic:
		{
			image_RelicIcon.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[slot.Level1ID].GetIconPath());
			_relicSlot = slot;
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			if (slot.IsLocked)
			{
				image_RelicIcon.color = Color.black;
				text_RelicRarity.text = "";
				text_RelicRarity.color = Color.black;
				text_RelicName.text = "???";
				text_RelicInfo.text = "";
				text_RelicMaxLevel.text = "";
				text_RelicGetTime.text = "";
				text_RelicUseType.text = "";
				text_RelicAdditionInfo.text = "";
				image_RelicAdditionInfoDelimiter.SetActive(value: false);
				text_RelicMaxLevel.gameObject.SetActive(value: false);
				text_RelicGetTime.gameObject.SetActive(value: false);
				break;
			}
			image_RelicIcon.color = Color.white;
			RelicConfig relicConfig = RelicConfig.dic[slot.Level1ID];
			switch (relicConfig.dropType)
			{
			case ItemDropType.None:
				text_RelicRarity.text = "???";
				text_RelicRarity.color = Color.black;
				break;
			case ItemDropType.Common:
				text_RelicRarity.text = 1001601.GetText();
				text_RelicRarity.color = GameConst.color_RarityCommon;
				break;
			case ItemDropType.Rare:
				text_RelicRarity.text = 1001602.GetText();
				text_RelicRarity.color = GameConst.color_RarityRare;
				break;
			case ItemDropType.Epic:
				text_RelicRarity.text = 1001603.GetText();
				text_RelicRarity.color = GameConst.color_RarityEpic;
				break;
			case ItemDropType.Special:
				text_RelicRarity.text = 1001604.GetText();
				text_RelicRarity.color = GameConst.color_RaritySpecial;
				break;
			default:
				Debug.LogError(relicConfig.dropType);
				break;
			}
			text_RelicName.text = relicConfig.GetName(haveLevel: false);
			text_RelicUseType.text = 1002202.GetText();
			if (GameMgr.IsMobile_Static)
			{
				text_RelicInfo.text = relicConfig.GetInfo(includeExtraInfo: false, upgrade: false);
			}
			else
			{
				text_RelicInfo.text = relicConfig.GetInfo(includeExtraInfo: false, upgrade: false);
			}
			text_RelicInfo.text = GeneralTool.FormatTextIfPublishTest(text_RelicInfo, text_RelicInfo.text);
			text_RelicAdditionInfo.text = relicConfig.GetAdditionInfo().Trim();
			text_RelicAdditionInfo.text = GeneralTool.FormatTextIfPublishTest(text_RelicAdditionInfo, text_RelicAdditionInfo.text);
			image_RelicAdditionInfoDelimiter.SetActive(text_RelicAdditionInfo.text.Length > 0);
			text_RelicMaxLevel.text = 1000415.GetText() + ": ";
			text_RelicGetTime.text = 1000410.GetText() + ": ";
			if (relicConfig.id == 40)
			{
				text_RelicMaxLevel.text = 1000416.GetText() + ": ";
			}
			else
			{
				text_RelicMaxLevel.text = 1000415.GetText() + ": ";
			}
			text_RelicMaxLevel.gameObject.SetActive(value: true);
			text_RelicMaxLevel.text += relicConfig.maxCount;
			if (DataMgr.selectedWorldData.galleryRelicGetTimes.ContainsKey(slot.Level1ID))
			{
				text_RelicGetTime.text += DataMgr.selectedWorldData.galleryRelicGetTimes[slot.Level1ID];
			}
			else
			{
				text_RelicGetTime.text += "0";
			}
			break;
		}
		case GalleryCategory.Potion:
			_potionSlot = slot;
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			image_PotionIcon.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[slot.Level1ID].GetIconPath());
			if (slot.IsLocked)
			{
				image_PotionIcon.color = Color.black;
				text_PotionName.text = "???";
				text_PotionInfo.text = "";
				text_PotionUseTime.text = "";
				break;
			}
			image_PotionIcon.color = Color.white;
			text_PotionName.text = PotionConfig.dic[slot.Level1ID].GetName();
			if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
			{
				text_PotionName.text += $"({slot.Level1ID})";
			}
			text_PotionInfo.text = PotionConfig.dic[slot.Level1ID].GetInfo();
			text_PotionUseTime.text = 1000412.GetText() + ": ";
			if (DataMgr.selectedWorldData.galleryPotionUseTimes.ContainsKey(slot.Level1ID))
			{
				text_PotionUseTime.text += DataMgr.selectedWorldData.galleryPotionUseTimes[slot.Level1ID];
			}
			else
			{
				text_PotionUseTime.text += "0";
			}
			break;
		case GalleryCategory.Curse:
		{
			_curseSlot = slot;
			if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				gamepadSelectedSlotIndexs[_onToggleIndex] = slot.transform.GetSiblingIndex();
			}
			image_CurseIcon.sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[slot.Level1ID].GetIconPath());
			if (slot.IsLocked)
			{
				image_CurseIcon.color = Color.black;
				text_CurseRarity.text = "";
				text_CurseRarity.color = Color.black;
				text_CurseName.text = "???";
				text_CurseInfo.text = "";
				text_CurseGetTime.text = "";
				text_CurseUseType.text = "";
				text_CurseMaxLevel.text = "";
				text_CurseMaxLevel.gameObject.SetActive(value: false);
				break;
			}
			image_CurseIcon.color = Color.white;
			switch (CurseConfig.dic[slot.Level1ID].dropType)
			{
			case ItemDropType.None:
				text_CurseRarity.text = "???";
				text_CurseRarity.color = Color.black;
				break;
			case ItemDropType.Common:
				text_CurseRarity.text = 1001601.GetText();
				text_CurseRarity.color = GameConst.color_RarityCommon;
				break;
			case ItemDropType.Rare:
				text_CurseRarity.text = 1001602.GetText();
				text_CurseRarity.color = GameConst.color_RarityRare;
				break;
			case ItemDropType.Epic:
				text_CurseRarity.text = 1001603.GetText();
				text_CurseRarity.color = GameConst.color_RarityEpic;
				break;
			case ItemDropType.Special:
				text_SpellRarity.text = 1001604.GetText();
				text_SpellRarity.color = GameConst.color_RaritySpecial;
				break;
			default:
				Debug.LogError(CurseConfig.dic[slot.Level1ID].dropType);
				break;
			}
			CurseConfig curseConfig = CurseConfig.dic[slot.Level1ID];
			text_CurseName.text = curseConfig.GetName();
			if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
			{
				text_CurseName.text += $"({slot.Level1ID})";
			}
			text_CurseUseType.text = 1002210.GetText();
			text_CurseInfo.text = curseConfig.GetInfo();
			text_CurseGetTime.text = 1000410.GetText() + ": ";
			if (DataMgr.selectedWorldData.galleryCurseGetTimes.TryGetValue(slot.Level1ID, out var value))
			{
				text_CurseGetTime.text += value;
			}
			else
			{
				text_CurseGetTime.text += "0";
			}
			text_CurseMaxLevel.text = 1000415.GetText() + ": ";
			text_CurseMaxLevel.gameObject.SetActive(value: true);
			text_CurseMaxLevel.text += curseConfig.count;
			break;
		}
		default:
			Debug.LogError(slot.Category);
			break;
		}
	}

	private IEnumerator WandInfoUpdate(UIGallerySlot slot)
	{
		image_WandIcon.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[slot.Level1ID].GetIconPath());
		WandConfig wandConfig = WandConfig.dic[slot.Level1ID];
		_wandSlot = slot;
		if (slot.IsLocked)
		{
			image_WandIcon.color = Color.black;
			text_WandName.text = "???";
			text_MaxMp.text = "";
			text_mpRecovery.text = "";
			text_ShootInterval.text = "";
			text_CD.text = "";
			text_info.text = "";
			gameobject_Line1.SetActive(value: false);
			gameobject_Line2.SetActive(value: false);
			text_info.enabled = true;
			LayoutRebuilder.ForceRebuildLayoutImmediate(text_info.gameObject.GetComponent<RectTransform>());
			gameobject_normalSlotContent.transform.DestroyAllChild();
			gameobject_postSlotContent.transform.DestroyAllChild();
			yield return null;
			LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_normalSlotContent.gameObject.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_postSlotContent.gameObject.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_postSlotContent.transform.parent.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
			yield break;
		}
		gameobject_Line2.SetActive(value: true);
		image_WandIcon.color = Color.white;
		text_WandName.text = WandConfig.dic[slot.Level1ID].GetName();
		if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
		{
			text_WandName.text += $"({slot.Level1ID})";
		}
		string text = ((float)wandConfig.maxMP + wandConfig.GetExtraMaxMP()).ToString("F0");
		text_MaxMp.text = 14000202.GetText(forceApplyAlogia: true);
		Text text2 = text_MaxMp;
		text2.text = text2.text + ": " + text;
		string text3 = GeneralTool.FloatToRetainDecimals((float)wandConfig.mpRecovery + wandConfig.GetExtraMPRecovery(), 1);
		text_mpRecovery.text = 14000203.GetText(forceApplyAlogia: true);
		Text text4 = text_mpRecovery;
		text4.text = text4.text + ": " + text3;
		string text5 = GeneralTool.FloatToRetainDecimals(wandConfig.shootInterval + wandConfig.GetExtraShootInterval(), 2);
		text_ShootInterval.text = 14000204.GetText(forceApplyAlogia: true);
		Text text6 = text_ShootInterval;
		text6.text = text6.text + ": " + text5;
		string text7 = GeneralTool.FloatToRetainDecimals(wandConfig.coolDown + wandConfig.GetExtraCoolDown(), 2);
		text_CD.text = 14000205.GetText(forceApplyAlogia: true);
		Text text8 = text_CD;
		text8.text = text8.text + ": " + text7;
		text_info.text = GeneralTool.FormatTextIfPublishTest(text_info, wandConfig.GetInfo());
		if (text_info.text.Length != 0)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(text_info.gameObject.GetComponent<RectTransform>());
			gameobject_Line1.SetActive(value: true);
			text_info.gameObject.SetActive(value: true);
		}
		else
		{
			gameobject_Line1.SetActive(value: false);
			text_info.gameObject.SetActive(value: false);
		}
		gameobject_normalSlotContent.transform.DestroyAllChild();
		gameobject_postSlotContent.transform.DestroyAllChild();
		for (int i = 0; i < wandConfig.normalSlots.Length; i++)
		{
			UnityEngine.Object.Instantiate(ui_gamepadcontrolshow, gameobject_normalSlotContent.GetComponent<RectTransform>()).Initialize(wandConfig, i, WandSlotType.Normal);
		}
		for (int j = 0; j < wandConfig.postSlots.Length; j++)
		{
			UnityEngine.Object.Instantiate(ui_gamepadcontrolshow, (GameMgr.IsMobile_Static ? gameobject_normalSlotContent : gameobject_postSlotContent).GetComponent<RectTransform>()).Initialize(wandConfig, j, WandSlotType.Post);
		}
		yield return null;
		LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_normalSlotContent.gameObject.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_postSlotContent.gameObject.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_postSlotContent.transform.parent.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
	}

	public override void _Close()
	{
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		Hide();
	}

	public void _ToggleValueChange()
	{
		foreach (GameObject item in goToggleSelected)
		{
			item.SetActive(value: false);
		}
		if (GameMgr.IsMobile_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			for (int i = 0; i < toggles.Length; i++)
			{
				if (toggles[i].isOn)
				{
					_onToggleIndex = i;
					break;
				}
			}
		}
		if (base.IsOpen)
		{
			SEMgr.Inst.uiRewardRelicHover.PlaySE();
		}
	}

	public void _SwitchSpellSound()
	{
		SEMgr.Inst.uiSwitch.PlaySE();
	}

	public void _SpellToggleValueChange()
	{
		foreach (GameObject item in goToggleSpellSelected)
		{
			item.SetActive(value: false);
		}
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			Debug.LogError(_onToggleIndex);
			if (tsf_Contents[_onToggleIndex].childCount > 0 && tsf_Contents[_onToggleIndex].GetChild(gamepadSelectedSlotIndexs[_onToggleIndex]).GetComponent<UIGallerySlot>() != null)
			{
				SlotEnter(tsf_Contents[_onToggleIndex].GetChild(gamepadSelectedSlotIndexs[_onToggleIndex]).GetComponent<UIGallerySlot>());
			}
		}
		_SpellToggleValueChangeFromStart();
	}

	public void _SpellToggleValueChangeFromStart()
	{
		int num = 0;
		for (int i = 0; i < spellSlots.Count; i++)
		{
			if (toggle_SpellLevel1.isOn)
			{
				spellSlots[i].SpellUpdate(1);
			}
			else if (toggle_SpellLevel2.isOn)
			{
				spellSlots[i].SpellUpdate(2);
			}
			else if (toggle_SpellLevel3.isOn)
			{
				if (SpellConfig.dic[spellSlots[i].Level1ID].abilityType == SpellAbilityType.SpellEmbryo)
				{
					spellSlots[i].SpellUpdate(2);
				}
				else
				{
					spellSlots[i].SpellUpdate(3);
				}
			}
			else
			{
				Debug.LogError("?");
			}
			if (!spellSlots[i].IsLocked)
			{
				num++;
			}
		}
		text_StatisticsSpell.text = num + "/" + spellSlots.Count;
	}

	public GameObject GetContentRoot(GalleryCategory category)
	{
		return galleryContains.FirstOrDefault((GalleryContains x) => x.galleryType == category)?.ContentRoot;
	}

	public GameObject GetListPanel(GalleryCategory category)
	{
		return galleryContains.FirstOrDefault((GalleryContains x) => x.galleryType == category)?.PanelLists;
	}

	public GameObject GetInfoPanel(GalleryCategory category)
	{
		return galleryContains.FirstOrDefault((GalleryContains x) => x.galleryType == category)?.PanelInfos;
	}

	public List<LayoutGroup> GetGridlayoutGroups(GalleryCategory category)
	{
		return galleryContains.FirstOrDefault((GalleryContains x) => x.galleryType == category)?.GridLayoutGroups;
	}

	public List<ContentSizeFitter> GetConstSizeFiltersGroup(GalleryCategory category)
	{
		return galleryContains.FirstOrDefault((GalleryContains x) => x.galleryType == category)?.ContentSizeFitters;
	}
}
