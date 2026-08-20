using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerLogger;
using PlayerLogger.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UISet")]
public class UISet : GameUISingletonMono<UISet>
{
	public bool resetSet = true;

	private bool currentSetsContainsDave;

	public float yoffset;

	public GameObject pfb_Slot;

	public GameObject upgradebutton;

	public GameObject UnlockButton;

	public GameObject UnlockButtonSelect;

	public GameObject upgradebuttonselect;

	public Color UpgradeAvailible;

	public Color UpgradeDisable;

	public Text text_Cost;

	public Text textUnlock;

	public Text textUpgrade;

	public RectTransform rtsf_SlotMotion;

	public Animator anima;

	public UIInfoWand uiInfoWand;

	public UIInfoRelic uiInfoRelic;

	public UIInfoGeneralPure uiInfoGeneralPure;

	public float slotSpace;

	public float slotMoveSpeed;

	public Text OtherInfo;

	[Header("MobileUpgradeCompare")]
	public GameObject upgradeComparePanel;

	public UIInfoWand beforeWand;

	public UIInfoWand afterWand;

	public UIInfoRelic beforeRelic;

	public UIInfoRelic afterRelic;

	public Text text_CostCompare;

	public Text textUpgradeCompare;

	[Header("LanguageChange")]
	public Text text_Title;

	[Header("InputChange")]
	public GameObject go_Keyboard_ShortcutLeft;

	public GameObject go_Keyboard_ShortcutRight;

	public List<UpdatButtonShow> updatButtonShows;

	private bool _isDaveWhenOnOpen;

	private List<SetConfig> _currentSets;

	private List<UISetSlot> slots;

	private UISetSlot _setSlotPre1;

	private UISetSlot _setSlotPre2;

	private UISetSlot _setSlotPost1;

	private UISetSlot _setSlotPost2;

	private int selectedIndex;

	private bool isMove;

	private bool selectupgrade;

	private bool selectunlock;

	private Vector2 slotParentMovePoint;

	private SuitChangeLogger suitChangeLogger;

	private int oriSuitId;

	private List<SetConfig> currentSets => _currentSets ?? (_currentSets = ((GameMgr.IsMobile_Static && !ICJNOGPFMAM.GGPJCCLPBJL) ? SetConfig.list.Where((SetConfig x) => x.id != 11).ToList() : SetConfig.list));

	public SetConfig SelectedSetCfg => currentSets[selectedIndex];

	public void RestOnNextStar()
	{
		resetSet = true;
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += DirectPerformed;
		base.inputActions.Player.LeftStick.performed += DirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.WASD.performed += DirectPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.LeftStick.performed -= DirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.WASD.performed -= DirectPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	public void Upgrade()
	{
		if (GameMgr.IsMobile_Static)
		{
			upgradeComparePanel.gameObject.SetActive(value: true);
			text_CostCompare.text = text_Cost.text;
			text_CostCompare.color = text_Cost.color;
			UpdateInfoMobileCompare();
		}
		else
		{
			_Upgrade();
		}
	}

	private void _Upgrade()
	{
		if (DataMgr.selectedWorldData.ancientBloodCount < currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1])
		{
			return;
		}
		PlayerMgr.Inst.ChangeAncientBlood(-currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1]);
		DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id]++;
		SEMgr.Inst.uiTalentUnlock.PlaySE();
		if (SelectedSetCfg.upgradeCosts.Length >= DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] && DataMgr.selectedWorldData.canSetUpgrade && DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] < SelectedSetCfg.WandIDs.Length)
		{
			upgradebutton.SetActive(value: true);
			UnlockButton.SetActive(value: false);
			if (DataMgr.selectedWorldData.ancientBloodCount >= currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1])
			{
				text_Cost.color = UpgradeAvailible;
			}
			else
			{
				text_Cost.color = UpgradeDisable;
			}
			text_Cost.text = currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1].ToString();
			UpdateInfoNextlevel();
		}
		else
		{
			upgradebutton.SetActive(value: false);
			selectupgrade = false;
			upgradebuttonselect.SetActive(value: false);
			UpdateInfo();
		}
		PlayerMgr.Inst.RefreshPlayer();
		DataMgr.SaveSelectedWorldData();
	}

	public void MobileUpgrade()
	{
		_Upgrade();
		upgradeComparePanel.gameObject.SetActive(value: false);
	}

	public void UnLock()
	{
		if (DataMgr.selectedWorldData.ancientBloodCount >= currentSets[selectedIndex].unlockCost)
		{
			int relicID = DataMgr.selectedWorldData.GetSelectedSetCfg().relicID;
			PlayerMgr.Inst.ChangeAncientBlood(-currentSets[selectedIndex].unlockCost);
			UnlockSet(selectedIndex);
			UpdateInfo();
			if (DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] < SelectedSetCfg.WandIDs.Length)
			{
				UpdateInfoNextlevel();
			}
			else
			{
				UpdateInfo();
			}
			if (upgradebutton.activeSelf)
			{
				upgradebuttonselect.SetActive(value: true);
				selectupgrade = true;
			}
			if (relicID != 0)
			{
				PlayerMgr.Inst.ItemCtrller.RelicRemove(relicID);
			}
			PlayerMgr.Inst.RefreshPlayer();
			DataMgr.SaveSelectedWorldData();
		}
	}

	public void UnlockSet(int Setindex)
	{
		if (!DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(Setindex + 1))
		{
			DataMgr.selectedWorldData.setUnlockedSets.Add(Setindex + 1, 1);
			DataMgr.selectedWorldData.selectedSetID = Setindex + 1;
			SEMgr.Inst.uiTalentUnlock.PlaySE();
			UpdateSet();
		}
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (upgradeComparePanel.activeInHierarchy && GameMgr.IsMobile_Static)
		{
			MobileUpgrade();
		}
		else if (selectupgrade)
		{
			Upgrade();
		}
		else if (selectunlock)
		{
			UnLock();
		}
	}

	private void DirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (upgradeComparePanel.gameObject.activeInHierarchy)
		{
			return;
		}
		if (_direct == Vector2.left)
		{
			leftswitch();
		}
		else if (_direct == Vector2.right)
		{
			rightswitch();
		}
		else if (_direct == Vector2.down)
		{
			if (UIMgr.Inst.InputType != PlayerInputType.Gamepad)
			{
				return;
			}
			if (upgradebutton.activeSelf)
			{
				if (!selectupgrade)
				{
					selectupgrade = true;
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
					upgradebuttonselect.SetActive(value: true);
					UpdateInfoNextlevel();
				}
			}
			else if (UnlockButton.activeSelf && !selectunlock)
			{
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
				selectunlock = true;
				UnlockButtonSelect.SetActive(value: true);
				UpdateInfoIfUnlock();
			}
		}
		else
		{
			if (!(_direct == Vector2.up) || UIMgr.Inst.InputType != PlayerInputType.Gamepad)
			{
				return;
			}
			if (upgradebutton.activeSelf)
			{
				if (selectupgrade)
				{
					selectupgrade = false;
					upgradebuttonselect.SetActive(value: false);
					UpdateInfoCurrentLevel();
				}
			}
			else if (UnlockButton.activeSelf && selectunlock)
			{
				selectunlock = false;
				UnlockButtonSelect.SetActive(value: false);
				UpdateInfoHide();
			}
		}
	}

	private void LanguageChange()
	{
		text_Title.text = 1002102.GetText();
		textUnlock.text = 1002106.GetText();
		textUpgrade.text = 1002105.GetText();
		textUpgradeCompare.text = textUpgrade.text;
		if (slots == null)
		{
			return;
		}
		foreach (UISetSlot slot in slots)
		{
			if ((bool)slot.GetComponent<UISetSlot>())
			{
				slot.GetComponent<UISetSlot>().Resetname();
			}
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			foreach (UpdatButtonShow updatButtonShow in updatButtonShows)
			{
				updatButtonShow.UpdateButton();
			}
			if (upgradebutton.activeSelf)
			{
				if (selectupgrade)
				{
					selectupgrade = false;
					upgradebuttonselect.SetActive(value: false);
					UpdateInfoCurrentLevel();
				}
			}
			else if (UnlockButton.activeSelf && selectunlock)
			{
				selectunlock = false;
				UnlockButtonSelect.SetActive(value: false);
				UpdateInfoHide();
			}
			go_Keyboard_ShortcutLeft.SetActive(value: true);
			go_Keyboard_ShortcutRight.SetActive(value: true);
			selectupgrade = false;
			upgradebuttonselect.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			go_Keyboard_ShortcutLeft.SetActive(value: false);
			go_Keyboard_ShortcutRight.SetActive(value: false);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		InputChange();
		UpdateSet();
		yield return null;
	}

	private IEnumerator UpdateInfoIE()
	{
		yield return null;
		UpdateInfo();
	}

	private void Update()
	{
		if (isMove)
		{
			rtsf_SlotMotion.anchoredPosition = Vector3.MoveTowards((Vector3)rtsf_SlotMotion.anchoredPosition, (Vector3)slotParentMovePoint, slotMoveSpeed * Time.deltaTime);
			if (rtsf_SlotMotion.anchoredPosition == slotParentMovePoint)
			{
				isMove = false;
			}
		}
	}

	private Vector3 GetMovePoint()
	{
		return new Vector2((float)((currentSets.Count - 1) / 2) * slotSpace - (float)selectedIndex * slotSpace, 0f);
	}

	public void UpdateInfoNextlevel()
	{
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoGeneralPure.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		if (SelectedSetCfg.WandIDs.Length != 0)
		{
			uiInfoWand.gameObject.SetActive(value: true);
			uiInfoWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id]]]);
		}
		if (SelectedSetCfg.relicID != 0)
		{
			uiInfoRelic.gameObject.SetActive(value: true);
			RelicConfig config = RelicConfig.GetConfig(SelectedSetCfg.relicID);
			config.level = DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] + 1;
			uiInfoRelic.UpdateInfo(config);
		}
	}

	public void UpdateInfoCurrentLevel()
	{
		if (SelectedSetCfg.WandIDs.Length != 0)
		{
			uiInfoWand.gameObject.SetActive(value: true);
			uiInfoWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1]]);
		}
		if (SelectedSetCfg.relicID != 0)
		{
			uiInfoRelic.gameObject.SetActive(value: true);
			RelicConfig config = RelicConfig.GetConfig(SelectedSetCfg.relicID);
			config.level = DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id];
			uiInfoRelic.UpdateInfo(config);
		}
	}

	public void UpdateInfoMobileCompare()
	{
		if (SelectedSetCfg.WandIDs.Length != 0)
		{
			beforeRelic.gameObject.SetActive(value: false);
			afterRelic.gameObject.SetActive(value: false);
			beforeWand.gameObject.SetActive(value: true);
			afterWand.gameObject.SetActive(value: true);
			beforeWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1]]);
			afterWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id]]]);
		}
		if (SelectedSetCfg.relicID != 0)
		{
			beforeRelic.gameObject.SetActive(value: true);
			afterRelic.gameObject.SetActive(value: true);
			beforeWand.gameObject.SetActive(value: false);
			afterWand.gameObject.SetActive(value: false);
			RelicConfig config = RelicConfig.GetConfig(SelectedSetCfg.relicID);
			config.level = DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id];
			uiInfoRelic.UpdateInfo(config);
			beforeRelic.UpdateInfo(config);
			RelicConfig config2 = RelicConfig.GetConfig(SelectedSetCfg.relicID);
			config2.level = DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] + 1;
			uiInfoRelic.UpdateInfo(config2);
			afterRelic.UpdateInfo(config2);
		}
	}

	public void UpdateInfoIfUnlock()
	{
		if (SelectedSetCfg.WandIDs.Length != 0)
		{
			uiInfoWand.gameObject.SetActive(value: true);
			uiInfoWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[0]]);
		}
		if (SelectedSetCfg.relicID != 0)
		{
			uiInfoRelic.gameObject.SetActive(value: true);
			RelicConfig config = RelicConfig.GetConfig(SelectedSetCfg.relicID);
			config.level = 1;
			uiInfoRelic.UpdateInfo(config);
		}
	}

	public void UpdateInfoHide()
	{
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoGeneralPure.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
	}

	private void UpdateInfo()
	{
		UpdateSlotActive();
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoGeneralPure.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		UnlockButton.SetActive(value: false);
		UnlockButtonSelect.SetActive(value: false);
		selectunlock = false;
		upgradebutton.SetActive(value: false);
		upgradebuttonselect.SetActive(value: false);
		selectupgrade = false;
		OtherInfo.text = "";
		if (DataMgr.selectedWorldData.IsSetUnlocked(SelectedSetCfg.id))
		{
			if (SelectedSetCfg.WandIDs.Length != 0)
			{
				uiInfoWand.gameObject.SetActive(value: true);
				uiInfoWand.UpdateInfo(WandConfig.dic[SelectedSetCfg.WandIDs[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1]]);
			}
			if (SelectedSetCfg.relicID != 0)
			{
				uiInfoRelic.gameObject.SetActive(value: true);
				RelicConfig config = RelicConfig.GetConfig(SelectedSetCfg.relicID);
				config.level = DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id];
				uiInfoRelic.UpdateInfo(config);
			}
			if (SelectedSetCfg.upgradeCosts.Length >= DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] && DataMgr.selectedWorldData.canSetUpgrade)
			{
				upgradebutton.SetActive(value: true);
				UnlockButton.SetActive(value: false);
				if (DataMgr.selectedWorldData.ancientBloodCount >= currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1])
				{
					text_Cost.color = UpgradeAvailible;
				}
				else
				{
					text_Cost.color = UpgradeDisable;
				}
				text_Cost.text = currentSets[selectedIndex].upgradeCosts[DataMgr.selectedWorldData.setUnlockedSets[SelectedSetCfg.id] - 1].ToString();
			}
			else
			{
				upgradebutton.SetActive(value: false);
				selectupgrade = false;
				upgradebuttonselect.SetActive(value: false);
			}
		}
		else
		{
			UnlockButton.SetActive(value: false);
			uiInfoGeneralPure.gameObject.SetActive(value: true);
			uiInfoGeneralPure.UpdateInfo(SelectedSetCfg.GetUnlockDesc());
			upgradebutton.SetActive(value: false);
			selectupgrade = false;
			upgradebuttonselect.SetActive(value: false);
			if (SelectedSetCfg.id == 6)
			{
				OtherInfo.text = 1002108.GetText() + ": " + DataMgr.selectedWorldData.set6KillCounter;
			}
			else if (SelectedSetCfg.id == 9)
			{
				OtherInfo.text = 1002104.GetText() + ": " + DataMgr.selectedWorldData.GetSetUnlockCount();
			}
			else if (SelectedSetCfg.id == 10)
			{
				int num = 0;
				foreach (KeyValuePair<int, int> galleryPotionUseTime in DataMgr.selectedWorldData.galleryPotionUseTimes)
				{
					num += galleryPotionUseTime.Value;
				}
				OtherInfo.text = 1002112.GetText() + ": " + num;
			}
		}
		for (int i = 0; i < rtsf_SlotMotion.transform.childCount; i++)
		{
			if ((bool)rtsf_SlotMotion.transform.GetChild(i).GetComponent<UISetSlot>())
			{
				rtsf_SlotMotion.transform.GetChild(i).GetComponent<UISetSlot>().CheckSelect();
			}
		}
	}

	public void UpdateSlotActive()
	{
		foreach (UISetSlot slot in slots)
		{
			slot.gameObject.SetActive(value: false);
		}
		_setSlotPre1.gameObject.SetActive(value: false);
		_setSlotPre2.gameObject.SetActive(value: false);
		_setSlotPost1.gameObject.SetActive(value: false);
		_setSlotPost2.gameObject.SetActive(value: false);
		if (selectedIndex == 0)
		{
			slots[selectedIndex + 1].gameObject.SetActive(value: true);
			_setSlotPre1.gameObject.SetActive(value: true);
		}
		else if (selectedIndex == slots.Count - 1)
		{
			slots[selectedIndex - 1].gameObject.SetActive(value: true);
			_setSlotPost1.gameObject.SetActive(value: true);
		}
		else
		{
			slots[selectedIndex - 1].gameObject.SetActive(value: true);
			slots[selectedIndex + 1].gameObject.SetActive(value: true);
		}
		slots[selectedIndex].gameObject.SetActive(value: true);
	}

	public void UpdateSet()
	{
		currentSetsContainsDave = ICJNOGPFMAM.GGPJCCLPBJL;
		rtsf_SlotMotion.DestroyAllChild();
		slots = new List<UISetSlot>();
		for (int i = 0; i < currentSets.Count; i++)
		{
			if (currentSets[i].id != 11 || !GameMgr.IsMobile_Static || ICJNOGPFMAM.GGPJCCLPBJL)
			{
				UISetSlot component = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
				component.Initialize(this, currentSets[i].id, isFake: false);
				slots.Add(component);
				float x = (float)(-(currentSets.Count - 1) / 2) * slotSpace + (float)i * slotSpace;
				((RectTransform)slots[i].transform).localPosition = new Vector3(x, 0f, 0f);
				if (currentSets[i].id == DataMgr.selectedWorldData.selectedSetID)
				{
					selectedIndex = i;
				}
			}
		}
		_setSlotPre1 = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		UISetSlot setSlotPre = _setSlotPre1;
		List<SetConfig> list = currentSets;
		setSlotPre.Initialize(this, list[list.Count - 1].id, isFake: true);
		float x2 = (float)(-(currentSets.Count - 1) / 2) * slotSpace - slotSpace;
		((RectTransform)_setSlotPre1.transform).localPosition = new Vector3(x2, 0f, 0f);
		_setSlotPre2 = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		UISetSlot setSlotPre2 = _setSlotPre2;
		List<SetConfig> list2 = currentSets;
		setSlotPre2.Initialize(this, list2[list2.Count - 2].id, isFake: true);
		float x3 = (float)(-(currentSets.Count - 1) / 2) * slotSpace - slotSpace * 2f;
		((RectTransform)_setSlotPre2.transform).localPosition = new Vector3(x3, 0f, 0f);
		_setSlotPost1 = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		_setSlotPost1.Initialize(this, currentSets[0].id, isFake: true);
		float x4 = (float)(-(currentSets.Count - 1) / 2) * slotSpace + (float)currentSets.Count * slotSpace;
		((RectTransform)_setSlotPost1.transform).localPosition = new Vector3(x4, 0f, 0f);
		_setSlotPost2 = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		_setSlotPost2.Initialize(this, currentSets[1].id, isFake: true);
		float x5 = (float)(-(currentSets.Count - 1) / 2) * slotSpace + (float)currentSets.Count * slotSpace + slotSpace;
		((RectTransform)_setSlotPost2.transform).localPosition = new Vector3(x5, 0f, 0f);
		rtsf_SlotMotion.anchoredPosition = GetMovePoint();
		slots[selectedIndex].zoomin(yoffset);
	}

	protected override void OnShow(object obj = null)
	{
		upgradeComparePanel.gameObject.SetActive(value: false);
		_currentSets = ((GameMgr.IsMobile_Static && !ICJNOGPFMAM.GGPJCCLPBJL) ? SetConfig.list.Where((SetConfig x) => x.id != 11).ToList() : SetConfig.list);
		_isDaveWhenOnOpen = DataMgr.selectedWorldData.IsDave;
		oriSuitId = SelectedSetCfg.id;
		suitChangeLogger = new SuitChangeLogger
		{
			before_unlocked = Suit.CreateAuto()
		};
		suitChangeLogger.AutoRecordBeforeResources();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		if (resetSet || (GameMgr.IsMobile_Static && currentSetsContainsDave != ICJNOGPFMAM.GGPJCCLPBJL))
		{
			UpdateSet();
			resetSet = false;
		}
		InputChange();
		StartCoroutine(UpdateInfoIE());
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
		slots[selectedIndex].ShowDes();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Blood);
	}

	protected override void OnHide()
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && upgradebutton.activeSelf && selectupgrade)
		{
			selectupgrade = false;
			upgradebuttonselect.SetActive(value: false);
			UpdateInfoCurrentLevel();
		}
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
		suitChangeLogger.after_unlocked = Suit.CreateAuto();
		suitChangeLogger.AutoRecordAfterResourcesAndFlow();
		suitChangeLogger.Report();
		if (oriSuitId != SelectedSetCfg.id && GameMgr.IsMobile_Static)
		{
			JObject jObject = new JObject { ["suit_id"] = SelectedSetCfg.id };
			MobileMgr.inst.PluginActivity.UploadEvent("suit_change", jObject.ToString(Formatting.None));
		}
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Blood);
		if (_isDaveWhenOnOpen != DataMgr.selectedWorldData.IsDave)
		{
			MusicMgr.Inst.UpdateCampBGM();
		}
		CheckDaveTalk();
	}

	public void CheckDaveTalk()
	{
		if (DataMgr.selectedWorldData.IsDave && !DataMgr.selectedWorldData.daveMirrorTalk)
		{
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, 938);
			DataMgr.selectedWorldData.selectedSetID = 1;
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShowCommon(301, delegate
			{
				DataMgr.selectedWorldData.selectedSetID = 11;
			});
			DataMgr.selectedWorldData.daveMirrorTalk = true;
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

	public override void Hide()
	{
		if (upgradeComparePanel.gameObject.activeInHierarchy)
		{
			upgradeComparePanel.gameObject.SetActive(value: false);
		}
		else
		{
			base.Hide();
		}
	}

	public void leftswitch()
	{
		slots[selectedIndex].zoomout(yoffset);
		if (selectedIndex == 0)
		{
			List<UISetSlot> list = slots;
			list[list.Count - 1].zoomin(yoffset);
		}
		else
		{
			slots[selectedIndex - 1].zoomin(yoffset);
		}
		int relicID = DataMgr.selectedWorldData.GetSelectedSetCfg().relicID;
		selectedIndex--;
		if (selectedIndex < 0)
		{
			selectedIndex = slots.Count - 1;
			rtsf_SlotMotion.anchoredPosition = GetMovePoint() + new Vector3(0f - slotSpace, 0f);
		}
		isMove = true;
		slotParentMovePoint = GetMovePoint();
		SEMgr.Inst.uiSwitch.PlaySE();
		if (DataMgr.selectedWorldData.selectedSetID != SelectedSetCfg.id && DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(SelectedSetCfg.id))
		{
			if (relicID != 0)
			{
				PlayerMgr.Inst.ItemCtrller.RelicRemove(relicID);
			}
			DataMgr.selectedWorldData.selectedSetID = SelectedSetCfg.id;
			PlayerMgr.Inst.RefreshPlayer();
			foreach (Wand wand in PlayerMgr.Inst.Wands)
			{
				wand.UpdateHandDisplay();
			}
			PlayerMgr.Inst.PlayerCtrller.SetToNormalAnime();
		}
		UpdateInfo();
	}

	public void rightswitch()
	{
		slots[selectedIndex].zoomout(yoffset);
		if (selectedIndex == slots.Count - 1)
		{
			slots[0].zoomin(yoffset);
		}
		else
		{
			slots[selectedIndex + 1].zoomin(yoffset);
		}
		int relicID = DataMgr.selectedWorldData.GetSelectedSetCfg().relicID;
		selectedIndex++;
		if (selectedIndex >= slots.Count)
		{
			selectedIndex = 0;
			rtsf_SlotMotion.anchoredPosition = GetMovePoint() + new Vector3(slotSpace, 0f);
		}
		isMove = true;
		slotParentMovePoint = GetMovePoint();
		SEMgr.Inst.uiSwitch.PlaySE();
		if (DataMgr.selectedWorldData.selectedSetID != SelectedSetCfg.id && DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(SelectedSetCfg.id))
		{
			if (relicID != 0)
			{
				PlayerMgr.Inst.ItemCtrller.RelicRemove(relicID);
			}
			DataMgr.selectedWorldData.selectedSetID = SelectedSetCfg.id;
			PlayerMgr.Inst.RefreshPlayer();
			for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
			{
				PlayerMgr.Inst.Wands[i].UpdateHandDisplay();
			}
			PlayerMgr.Inst.PlayerCtrller.SetToNormalAnime();
		}
		UpdateInfo();
	}
}
