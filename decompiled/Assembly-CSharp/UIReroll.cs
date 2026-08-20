using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIReroll")]
public class UIReroll : GameUISingletonMono<UIReroll>
{
	public UIReroll_Spell pfb_UIRerollSpell;

	public RectTransform rtsf_BtnMask;

	public RectTransform rtsf_Mask;

	public RectTransform rtsf_Center;

	public RectTransform rtsf_Spells;

	public Animator anima;

	public Text text_Cost;

	public Text text_Tip;

	public UIInfoSpell uiSpellInfo;

	public float3 followOffset;

	public float space;

	public float recastDuration;

	public float focusSize;

	public float focusTime;

	[Header("InputChange")]
	public GameObject panel_Arrow;

	public Image image_Shortcut;

	public Sprite sprite_ShortcutKeyborad;

	public Sprite sprite_ShortcutGamepad;

	[Header("LanguageChange")]
	public Text text_Reroll;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	private List<UIReroll_Spell> uiRSs = new List<UIReroll_Spell>();

	private Dictionary<int, int> dic_Splls = new Dictionary<int, int>();

	private bool isRecasting;

	private float recastDurationTimer;

	private Entity so101RerollEtt;

	private EntityManager ettMgr;

	public int SelectedSpellIndex { get; private set; }

	private UIReroll_Spell SelectedUIRS => uiRSs[SelectedSpellIndex];

	private SpellConfig SelectedSpellCfg => SpellConfig.dic[SelectedUIRS.SpellID];

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += DirectPerformed;
		base.inputActions.Player.LeftStick.performed += DirectPerformed_Stick;
		base.inputActions.Player.WASD.performed += DirectPerformed;
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.LeftStick.performed -= DirectPerformed_Stick;
		base.inputActions.Player.WASD.performed -= DirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirection(direct);
		}
	}

	private void DirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirection(vector);
		}
	}

	private void MoveDirection(Vector2 _direct)
	{
		if (_direct.x < 0f)
		{
			InteractLeft();
		}
		else if (_direct.x > 0f)
		{
			InteractRight();
		}
	}

	public void InteractLeft()
	{
		if (SelectedSpellIndex > 0)
		{
			SelectedSpellIndex--;
			uiSpellInfo.UpdateInfo(SelectedUIRS.SpellID);
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			CheckCost();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	public void InteractRight()
	{
		if (SelectedSpellIndex < uiRSs.Count - 1)
		{
			SelectedSpellIndex++;
			uiSpellInfo.UpdateInfo(SelectedUIRS.SpellID);
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			CheckCost();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !rtsf_BtnMask.gameObject.activeSelf)
		{
			_Reroll();
		}
	}

	private void LanguageChange()
	{
		text_Reroll.text = 1000904.GetText();
	}

	private void InputChange()
	{
		ControlChange();
		if (!GameMgr.IsMobile_Static)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				panel_Arrow.SetActive(value: true);
				break;
			case PlayerInputType.Gamepad:
				panel_Arrow.SetActive(value: false);
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			}
		}
	}

	private void ControlChange()
	{
		if (!GameMgr.IsMobile_Static)
		{
			UpdatButtonShow[] array = updatebuttonshows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateButton();
			}
		}
	}

	protected override IEnumerator OnInit()
	{
		LanguageChange();
		InputChange();
		ControlChange();
		yield return null;
	}

	protected override void OnShow(object obj)
	{
		if (!(obj is Entity))
		{
			return;
		}
		Entity entity = (so101RerollEtt = (Entity)obj);
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		if (GameMgr.IsMobile_Static)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101RerollEtt);
			CamController.Inst.FocusOn(focusSize, focusTime, componentData.Position + UIBattleMgr.Inst.UIProcessPositionDefault * CamController.Inst.FocusCamSizeRatio);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, focusTime);
		}
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		rtsf_Spells.DestroyAllChild();
		uiRSs.Clear();
		dic_Splls.Clear();
		SelectedSpellIndex = 0;
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] != null)
			{
				if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.bagSpellDatas[i].id))
				{
					dic_Splls[PlayerMgr.Inst.BaData.bagSpellDatas[i].id]++;
				}
				else
				{
					dic_Splls.Add(PlayerMgr.Inst.BaData.bagSpellDatas[i].id, 1);
				}
			}
		}
		for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
		{
			if (PlayerMgr.Inst.BaData.wandCfgs[j] == null)
			{
				continue;
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots.Length; k++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlotIsLock[k])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id, 1);
					}
				}
			}
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[j].postSlots.Length; l++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlotIsLock[l])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id, 1);
					}
				}
			}
		}
		foreach (KeyValuePair<int, int> dic_Spll in dic_Splls)
		{
			if (dic_Spll.Key != 0)
			{
				UIReroll_Spell uIReroll_Spell = UnityEngine.Object.Instantiate(pfb_UIRerollSpell, rtsf_Spells);
				uiRSs.Add(uIReroll_Spell);
				uIReroll_Spell.Initialize(uiRSs.Count - 1, dic_Spll.Key, dic_Spll.Value);
			}
		}
		if (uiRSs.Count == 0)
		{
			uiSpellInfo.gameObject.SetActive(value: false);
		}
		else
		{
			uiSpellInfo.gameObject.SetActive(value: true);
			uiSpellInfo.UpdateInfo(uiRSs[0].SpellID);
		}
		CheckCost();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Coin);
	}

	protected override void OnHide()
	{
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(focusTime);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Coin);
	}

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(Hide));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(Hide));
	}

	private void Update()
	{
		Rerolling();
		Following();
	}

	private void Rerolling()
	{
		if (isRecasting)
		{
			recastDurationTimer += Time.deltaTime;
			if (recastDurationTimer >= recastDuration)
			{
				isRecasting = false;
				CheckCost();
			}
		}
	}

	private void Following()
	{
		if (base.IsOpen)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101RerollEtt);
			if (GameMgr.IsMobile_Static)
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(componentData.Position + followOffset);
			}
			else
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(componentData.Position + followOffset);
			}
			rtsf_Mask.anchoredPosition = rtsf_Center.anchoredPosition;
		}
	}

	private int GetCostFinal()
	{
		if (GameMgr.InEndlessMode)
		{
			return Mathf.FloorToInt((float)GetCost() * ((float)BattleMgr.Inst.CurrentLevel + 10f) / 10f);
		}
		return GetCost();
	}

	private int GetCost()
	{
		SpellConfig spellConfig = SpellConfig.dic[SelectedUIRS.SpellID];
		switch (spellConfig.dropType)
		{
		case ItemDropType.None:
			return 99999999;
		case ItemDropType.Common:
			if (spellConfig.level == 1)
			{
				return 2;
			}
			if (spellConfig.level == 2)
			{
				return 3;
			}
			if (spellConfig.level == 3)
			{
				return 4;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Rare:
			if (spellConfig.level == 1)
			{
				return 3;
			}
			if (spellConfig.level == 2)
			{
				return 4;
			}
			if (spellConfig.level == 3)
			{
				return 5;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Epic:
			if (spellConfig.level == 1)
			{
				return 15;
			}
			if (spellConfig.level == 2)
			{
				return 15;
			}
			if (spellConfig.level == 3)
			{
				return 15;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Special:
			return 99999999;
		default:
			Debug.LogError(spellConfig.dropType);
			return 99999999;
		}
	}

	private void CheckCost()
	{
		text_Tip.gameObject.SetActive(value: false);
		if (uiRSs.Count == 0)
		{
			text_Cost.text = "0";
			rtsf_BtnMask.gameObject.SetActive(value: true);
			return;
		}
		if (SelectedSpellCfg.dropType == ItemDropType.Special)
		{
			text_Tip.gameObject.SetActive(value: true);
			text_Tip.text = 1000909.GetText();
			text_Cost.text = "";
			rtsf_BtnMask.gameObject.SetActive(value: true);
			return;
		}
		int costFinal = GetCostFinal();
		text_Cost.text = costFinal.ToString();
		if (PlayerMgr.Inst.CoinCount >= costFinal)
		{
			text_Cost.color = Color.green;
			rtsf_BtnMask.gameObject.SetActive(value: false);
		}
		else
		{
			text_Cost.color = Color.red;
			rtsf_BtnMask.gameObject.SetActive(value: true);
		}
	}

	public void _Reroll()
	{
		if (SelectedSpellCfg.dropType == ItemDropType.Special)
		{
			return;
		}
		PlayerMgr.Inst.ChangeCoin(-GetCostFinal());
		SEMgr.Inst.so101_Reroll.PlaySE();
		int num = 0;
		int num2 = 0;
		int level = SelectedSpellCfg.level;
		if (SelectedSpellCfg.abilityType == SpellAbilityType.DeathAdder)
		{
			level = 1;
		}
		do
		{
			num2++;
			if (num2 > 100)
			{
				Debug.LogError("!");
				num = 10011;
				break;
			}
			num = PlayerMgr.Inst.BaData.GetSpellFromPool(level, SelectedSpellCfg.dropType);
		}
		while (num / 10 == SelectedSpellCfg.id / 10);
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] != null && PlayerMgr.Inst.BaData.bagSpellDatas[i].id == SelectedUIRS.SpellID)
			{
				PlayerMgr.Inst.Slot_RemoveBagSlot(i);
				SlotData slotData = new SlotData(num);
				if (PlayerMgr.Inst.CanBagSpellChange(i, slotData))
				{
					PlayerMgr.Inst.BagSpellChange(i, slotData);
				}
				else
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(slotData), PlayerMgr.Inst.PlayerPoint);
					flag2 = true;
				}
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < PlayerMgr.Inst.BaData.wandCfgs.Count; j++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[j] == null)
				{
					continue;
				}
				for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots.Length; k++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlotIsLock[k] && PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].id == SelectedUIRS.SpellID && !PlayerMgr.Inst.BaData.wandCfgs[j].normalSlots[k].isAllFieldSharedSpell)
					{
						int num3 = k;
						if (PlayerMgr.Inst.Wands[j].GetWandAllFieldEnhanceSpell().Count > 0)
						{
							num3--;
						}
						PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Normal, k, null);
						SlotData slotData2 = new SlotData(num);
						if (PlayerMgr.Inst.CanChangeInWandSpell(j, WandSlotType.Normal, num3, slotData2))
						{
							PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Normal, num3, slotData2);
						}
						else
						{
							QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(slotData2), PlayerMgr.Inst.PlayerPoint);
							flag2 = true;
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[j].postSlots.Length; l++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[j].postSlotIsLock[l] && PlayerMgr.Inst.BaData.wandCfgs[j].postSlots[l].id == SelectedUIRS.SpellID)
					{
						PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Post, l, null);
						SlotData slotData3 = new SlotData(num);
						if (PlayerMgr.Inst.CanChangeInWandSpell(j, WandSlotType.Post, l, slotData3))
						{
							PlayerMgr.Inst.ChangeWandSpell(j, WandSlotType.Post, l, slotData3);
						}
						else
						{
							QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(slotData3), PlayerMgr.Inst.PlayerPoint);
							flag2 = true;
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		int num4 = dic_Splls[SelectedUIRS.SpellID] - 1;
		bool flag3 = dic_Splls.ContainsKey(num);
		if (num4 == 0)
		{
			dic_Splls.Remove(SelectedSpellCfg.id);
			SelectedUIRS.ChangeID(num);
			uiSpellInfo.UpdateInfo(SelectedSpellCfg.id);
			if (flag3)
			{
				dic_Splls[num]++;
				int num5 = -1;
				for (int m = 0; m < uiRSs.Count; m++)
				{
					if (uiRSs[m].SpellID == num && m != SelectedSpellIndex)
					{
						num5 = m;
						break;
					}
				}
				UIReroll_Spell uIReroll_Spell = uiRSs[num5];
				uIReroll_Spell.Fly(SelectedUIRS);
				uiRSs.Remove(uIReroll_Spell);
				if (num5 < SelectedSpellIndex)
				{
					SelectedSpellIndex--;
					for (int n = 0; n < uiRSs.Count; n++)
					{
						uiRSs[n].ChangeIndex(n, SelectedSpellIndex);
					}
				}
				else
				{
					for (int num6 = SelectedSpellIndex; num6 < uiRSs.Count; num6++)
					{
						uiRSs[num6].ChangeIndex(num6, SelectedSpellIndex);
					}
				}
			}
			else
			{
				dic_Splls.Add(num, 1);
			}
		}
		else
		{
			dic_Splls[SelectedSpellCfg.id]--;
			SelectedUIRS.ChangeCount(dic_Splls[SelectedSpellCfg.id]);
			UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRerollSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
			if (flag3)
			{
				dic_Splls[num]++;
				int num7 = -1;
				for (int num8 = 0; num8 < uiRSs.Count; num8++)
				{
					if (uiRSs[num8].SpellID == num)
					{
						num7 = num8;
						break;
					}
				}
				UIReroll_Spell uIReroll_Spell2 = uiRSs[num7];
				uIReroll_Spell2.Fly(component);
				uiRSs.Remove(uIReroll_Spell2);
				if (num7 < SelectedSpellIndex)
				{
					component.Initialize(SelectedSpellIndex - 1, num, 1);
					uiRSs.Insert(SelectedSpellIndex - 1, component);
					for (int num9 = 0; num9 <= SelectedSpellIndex; num9++)
					{
						uiRSs[num9].ChangeIndex(num9, SelectedSpellIndex);
					}
				}
				else
				{
					component.Initialize(SelectedSpellIndex + 1, num, 1);
					uiRSs.Insert(SelectedSpellIndex + 1, component);
					for (int num10 = SelectedSpellIndex + 1; num10 < uiRSs.Count; num10++)
					{
						uiRSs[num10].ChangeIndex(num10, SelectedSpellIndex);
					}
				}
			}
			else
			{
				dic_Splls.Add(num, 1);
				component.Initialize(SelectedSpellIndex + 1, num, 1);
				uiRSs.Insert(SelectedSpellIndex + 1, component);
				for (int num11 = SelectedSpellIndex + 1; num11 < uiRSs.Count; num11++)
				{
					uiRSs[num11].ChangeIndex(num11, SelectedSpellIndex);
				}
			}
		}
		if (!GameMgr.InEndlessMode)
		{
			SpecialObj101Reroll_Dots componentData = ettMgr.GetComponentData<SpecialObj101Reroll_Dots>(so101RerollEtt);
			bool num12 = componentData.UseOnce();
			ettMgr.SetComponentData(so101RerollEtt, componentData);
			if (num12 || flag2)
			{
				Hide();
			}
			else
			{
				rtsf_BtnMask.gameObject.SetActive(value: true);
				text_Cost.text = GetCostFinal().ToString();
				isRecasting = true;
				recastDurationTimer = 0f;
			}
		}
		else if (flag2)
		{
			Hide();
		}
		else
		{
			rtsf_BtnMask.gameObject.SetActive(value: true);
			text_Cost.text = GetCostFinal().ToString();
			isRecasting = true;
			recastDurationTimer = 0f;
		}
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, num);
	}
}
