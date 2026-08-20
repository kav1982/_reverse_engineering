using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIMoreInOne")]
public class UIMoreInOne : GameUISingletonMono<UIMoreInOne>
{
	private enum UIState
	{
		Idle,
		Blending,
		BlendingFinish
	}

	public GameObject pfb_UIRecastSpell;

	public GameObject go_BtnPutInMask;

	public GameObject go_BtnBlendMask;

	public GameObject go_BtnClose;

	public GameObject go_MainSlot;

	public Button btn_PutIn;

	public Button btn_Blend;

	public RectTransform rtsf_Spells;

	public RectTransform rtsf_Center;

	public RectTransform rtsf_Mask;

	public RectTransform rtsf_UISPellInfoParent;

	public RectTransform rtsf_SlotsParent;

	public UISlotMoreInOne[] uiSMIO_Waits;

	public UISlotMoreInOne uiSMIO_Result;

	public Animator anima;

	public Text text_Tip;

	public UIInfoSpell uiInfoSpell;

	public UIInfoSpell uiInfoSpell_Hover;

	public Vector2 infoOffset;

	public float3 followOffset;

	public float focusSize;

	public float focusTime;

	[Header("InputChange")]
	public GameObject panel_Arrow;

	public Image image_PutinShortcut;

	public Sprite sprite_PutinShorctutKeyboard;

	public Sprite sprite_PutinShortcutGamepad;

	[Header("LanguageChange")]
	public Text text_Putin;

	public Text text_Blend;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	public List<GameObject> disactiveOnBlendMobile = new List<GameObject>();

	private UIState uiState;

	private List<UIReroll_Spell> uiRSs = new List<UIReroll_Spell>();

	private Dictionary<int, int> dic_Splls = new Dictionary<int, int>();

	private UISlotMoreInOne uiSMIO_Hover;

	private EntityManager ettMgr;

	private Entity so101MoreInOneEtt;

	public int SelectedSpellIndex { get; private set; } = -1;


	public UIReroll_Spell SelectedUIRS
	{
		get
		{
			if (SelectedSpellIndex != -1)
			{
				return uiRSs[SelectedSpellIndex];
			}
			return null;
		}
	}

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
		base.inputActions.Player.GamepadWest.performed += GamepadWestPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.LeftStick.performed -= DirectPerformed_Stick;
		base.inputActions.Player.WASD.performed -= DirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.GamepadWest.performed -= GamepadWestPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && uiState == UIState.Idle)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void DirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen && uiState == UIState.Idle)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (_direct == new Vector2(-1f, 0f))
		{
			InteractLeft();
		}
		else if (_direct == new Vector2(1f, 0f))
		{
			InteractRight();
		}
	}

	public void InteractLeft()
	{
		if (SelectedSpellIndex > 0)
		{
			SelectedSpellIndex--;
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	public void InteractRight()
	{
		if (SelectedSpellIndex < uiRSs.Count - 1)
		{
			SelectedSpellIndex++;
			for (int i = 0; i < uiRSs.Count; i++)
			{
				uiRSs[i].SetMove(SelectedSpellIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && uiState == UIState.Idle)
		{
			if (!go_BtnPutInMask.activeSelf)
			{
				_PutIn();
			}
			else if (!go_BtnBlendMask.activeSelf && ControlMgr.Inst.InputType == PlayerInputType.Keyboard)
			{
				_Blend();
			}
		}
	}

	private void GamepadWestPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && uiState == UIState.Idle && !go_BtnBlendMask.activeSelf)
		{
			_Blend();
		}
	}

	private void LanguageChange()
	{
		text_Putin.text = 1000905.GetText();
		text_Blend.text = 1000906.GetText();
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
		Entity entity = (so101MoreInOneEtt = (Entity)obj);
		disactiveOnBlendMobile.ForEach(delegate(GameObject x)
		{
			x.SetActive(value: true);
		});
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		if (GameMgr.IsMobile_Static)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101MoreInOneEtt);
			CamController.Inst.FocusOn(focusSize, focusTime, componentData.Position + UIBattleMgr.Inst.UIProcessPositionDefault * CamController.Inst.FocusCamSizeRatio);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, focusTime);
		}
		uiState = UIState.Idle;
		for (int i = 0; i < uiSMIO_Waits.Length; i++)
		{
			uiSMIO_Waits[i].Reset();
			uiSMIO_Waits[i].gameObject.SetActive(value: true);
		}
		uiSMIO_Result.Reset();
		uiSMIO_Result.gameObject.SetActive(value: false);
		btn_PutIn.gameObject.SetActive(value: true);
		btn_Blend.gameObject.SetActive(value: true);
		go_BtnPutInMask.SetActive(value: true);
		go_BtnBlendMask.SetActive(value: true);
		go_BtnClose.SetActive(value: true);
		go_MainSlot.SetActive(value: true);
		rtsf_Spells.gameObject.SetActive(value: true);
		uiInfoSpell.gameObject.SetActive(value: true);
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			panel_Arrow.SetActive(value: true);
		}
		uiRSs.Clear();
		dic_Splls.Clear();
		rtsf_Spells.DestroyAllChild();
		SelectedSpellIndex = -1;
		for (int j = 0; j < PlayerMgr.Inst.BaData.bagSpellDatas.Count; j++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[j] != null && !PlayerMgr.Inst.BaData.bagSpellDatas[j].isSealSlot)
			{
				if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.bagSpellDatas[j].id))
				{
					dic_Splls[PlayerMgr.Inst.BaData.bagSpellDatas[j].id]++;
				}
				else
				{
					dic_Splls.Add(PlayerMgr.Inst.BaData.bagSpellDatas[j].id, 1);
				}
			}
		}
		for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs.Count; k++)
		{
			if (PlayerMgr.Inst.BaData.wandCfgs[k] == null)
			{
				continue;
			}
			for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots.Length; l++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].isSealSlot && !PlayerMgr.Inst.BaData.wandCfgs[k].normalSlotIsLock[l])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].id, 1);
					}
				}
			}
			for (int m = 0; m < PlayerMgr.Inst.BaData.wandCfgs[k].postSlots.Length; m++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m] != null && !PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m].isSealSlot && !PlayerMgr.Inst.BaData.wandCfgs[k].postSlotIsLock[m])
				{
					if (dic_Splls.ContainsKey(PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m].id))
					{
						dic_Splls[PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m].id]++;
					}
					else
					{
						dic_Splls.Add(PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m].id, 1);
					}
				}
			}
		}
		foreach (KeyValuePair<int, int> dic_Spll in dic_Splls)
		{
			UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
			component.totalHideSpace = 4;
			uiRSs.Add(component);
			component.Initialize(uiRSs.Count - 1, dic_Spll.Key, dic_Spll.Value);
		}
		if (uiRSs.Count > 0)
		{
			SelectedSpellIndex = 0;
		}
		UpdateInfo();
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
		if (base.IsOpen)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so101MoreInOneEtt);
			if (GameMgr.IsMobile_Static)
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(componentData.Position + followOffset);
			}
			else
			{
				rtsf_Center.anchoredPosition = GeneralTool.WorldToCanvasLocalPoint(componentData.Position + followOffset);
			}
			rtsf_Mask.anchoredPosition = rtsf_Center.anchoredPosition;
			rtsf_UISPellInfoParent.anchoredPosition = rtsf_Center.anchoredPosition;
		}
	}

	private void UpdateInfo()
	{
		btn_PutIn.interactable = false;
		btn_Blend.interactable = false;
		go_BtnPutInMask.SetActive(value: true);
		go_BtnBlendMask.SetActive(value: true);
		text_Tip.gameObject.SetActive(value: false);
		uiInfoSpell.gameObject.SetActive(value: false);
		int num = 0;
		bool flag = false;
		for (int i = 0; i < uiSMIO_Waits.Length; i++)
		{
			if (uiSMIO_Waits[i].ID == 0)
			{
				if (!flag)
				{
					flag = true;
				}
			}
			else if (num == 0)
			{
				num = uiSMIO_Waits[i].ID;
			}
		}
		if (uiRSs.Count == 0)
		{
			if (!flag)
			{
				btn_Blend.interactable = true;
				go_BtnBlendMask.SetActive(value: false);
			}
			return;
		}
		uiInfoSpell.gameObject.SetActive(value: true);
		uiInfoSpell.UpdateInfo(new SlotData(SelectedUIRS.SpellID));
		if (flag)
		{
			if (num == 0)
			{
				if (SelectedSpellCfg.canCompound)
				{
					btn_PutIn.interactable = true;
					go_BtnPutInMask.SetActive(value: false);
				}
				else
				{
					text_Tip.gameObject.SetActive(value: true);
					text_Tip.text = 1000907.GetText();
				}
			}
			else if (SelectedSpellCfg.id == 40202)
			{
				text_Tip.gameObject.SetActive(value: true);
				text_Tip.text = 1000907.GetText();
			}
			else if (SpellConfig.dic[num].level == SelectedSpellCfg.level && SpellConfig.dic[num].dropType == SelectedSpellCfg.dropType)
			{
				btn_PutIn.interactable = true;
				go_BtnPutInMask.SetActive(value: false);
			}
			else
			{
				text_Tip.gameObject.SetActive(value: true);
				text_Tip.text = 1000908.GetText();
			}
		}
		else
		{
			btn_Blend.interactable = true;
			go_BtnBlendMask.SetActive(value: false);
		}
	}

	public void BackSpell(int id)
	{
		if (SelectedUIRS == null)
		{
			dic_Splls.Add(id, 1);
			UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
			component.Initialize(SelectedSpellIndex + 1, id, 1);
			uiRSs.Add(component);
			SelectedSpellIndex = 0;
		}
		else if (SelectedUIRS.SpellID == id)
		{
			dic_Splls[id]++;
			SelectedUIRS.ChangeCount(dic_Splls[id]);
		}
		else
		{
			bool num = dic_Splls.ContainsKey(id);
			UIReroll_Spell component2 = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, rtsf_Spells).GetComponent<UIReroll_Spell>();
			if (num)
			{
				dic_Splls[id]++;
				int num2 = -1;
				for (int i = 0; i < uiRSs.Count; i++)
				{
					if (uiRSs[i].SpellID == id)
					{
						num2 = i;
						break;
					}
				}
				UIReroll_Spell uIReroll_Spell = uiRSs[num2];
				uIReroll_Spell.Fly(component2);
				uiRSs.Remove(uIReroll_Spell);
				if (num2 < SelectedSpellIndex)
				{
					component2.Initialize(SelectedSpellIndex - 1, id, 1);
					uiRSs.Insert(SelectedSpellIndex - 1, component2);
					for (int j = 0; j <= SelectedSpellIndex; j++)
					{
						uiRSs[j].ChangeIndex(j, SelectedSpellIndex);
					}
				}
				else
				{
					component2.Initialize(SelectedSpellIndex + 1, id, 1);
					uiRSs.Insert(SelectedSpellIndex + 1, component2);
					for (int k = SelectedSpellIndex + 1; k < uiRSs.Count; k++)
					{
						uiRSs[k].ChangeIndex(k, SelectedSpellIndex);
					}
				}
			}
			else
			{
				dic_Splls.Add(id, 1);
				component2.Initialize(SelectedSpellIndex + 1, id, 1);
				uiRSs.Insert(SelectedSpellIndex + 1, component2);
				for (int l = SelectedSpellIndex + 1; l < uiRSs.Count; l++)
				{
					uiRSs[l].ChangeIndex(l, SelectedSpellIndex);
				}
			}
		}
		UpdateInfo();
	}

	public void BackSpellGamepadEast()
	{
		_Close();
	}

	public override void _Close()
	{
		if (uiState == UIState.Idle)
		{
			for (int num = uiSMIO_Waits.Length - 1; num >= 0; num--)
			{
				if (uiSMIO_Waits[num].ID != 0)
				{
					uiSMIO_Waits[num].OnPointerClick(null);
					return;
				}
			}
		}
		if (uiState != UIState.Blending)
		{
			Hide();
		}
	}

	public void BlendingFinish()
	{
		if (uiState != UIState.BlendingFinish)
		{
			uiState = UIState.BlendingFinish;
			go_BtnClose.SetActive(value: true);
			uiSMIO_Result.gameObject.SetActive(value: true);
			uiSMIO_Result.image_Icon.gameObject.SetActive(value: true);
			uiSMIO_Result.image_Icon.transform.position = uiSMIO_Result.transform.position;
			uiInfoSpell.gameObject.SetActive(value: true);
			uiInfoSpell.UpdateInfo(new SlotData(uiSMIO_Result.ID));
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIMoreInOneFinish"), uiSMIO_Result.transform).transform.localPosition = Vector3.zero;
		}
	}

	public void UISlotMoreInOneEnter(UISlotMoreInOne uiSlotMoreInOne)
	{
		if (uiSMIO_Hover != null)
		{
			uiSMIO_Hover.Unhover();
		}
		uiSMIO_Hover = uiSlotMoreInOne;
		uiSMIO_Hover.Hover();
		if (uiSlotMoreInOne.ID != 0 && uiSlotMoreInOne.State == UISlotMoreInOneState.Idle)
		{
			uiInfoSpell_Hover.gameObject.SetActive(value: true);
			if (base.gameObject.activeInHierarchy)
			{
				uiInfoSpell_Hover.UpdateInfo(new SlotData(uiSMIO_Hover.ID));
			}
			uiInfoSpell_Hover.rtsf_Self.anchoredPosition = rtsf_SlotsParent.anchoredPosition + uiSMIO_Hover.rtsf_Self.anchoredPosition + infoOffset;
		}
	}

	public void UISlotMoreInOneExit(UISlotMoreInOne uiSlotMoreInOne)
	{
		uiSMIO_Hover = null;
		uiSlotMoreInOne.Unhover();
		uiInfoSpell_Hover.gameObject.SetActive(value: false);
	}

	protected override void OnHide()
	{
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		so101MoreInOneEtt = Entity.Null;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(focusTime);
	}

	public void _PutIn()
	{
		SEMgr.Inst.uiFly.PlaySE();
		for (int i = 0; i < uiSMIO_Waits.Length; i++)
		{
			if (uiSMIO_Waits[i].ID == 0)
			{
				uiSMIO_Waits[i].SetSpell(SelectedSpellCfg.id);
				break;
			}
		}
		if (SelectedUIRS.Count == 1)
		{
			UIReroll_Spell selectedUIRS = SelectedUIRS;
			dic_Splls.Remove(SelectedUIRS.SpellID);
			uiRSs.Remove(selectedUIRS);
			UnityEngine.Object.Destroy(selectedUIRS.gameObject);
			if (uiRSs.Count == 0)
			{
				SelectedSpellIndex = -1;
			}
			else if (uiRSs.Count == SelectedSpellIndex)
			{
				SelectedSpellIndex--;
				for (int j = 0; j < uiRSs.Count; j++)
				{
					uiRSs[j].ChangeIndex(j, SelectedSpellIndex);
				}
			}
			else
			{
				for (int k = SelectedSpellIndex; k < uiRSs.Count; k++)
				{
					uiRSs[k].ChangeIndex(k, SelectedSpellIndex);
				}
			}
		}
		else
		{
			dic_Splls[SelectedUIRS.SpellID]--;
			SelectedUIRS.ChangeCount(dic_Splls[SelectedUIRS.SpellID]);
		}
		UpdateInfo();
	}

	public void _Blend()
	{
		uiState = UIState.Blending;
		rtsf_Spells.gameObject.SetActive(value: false);
		btn_PutIn.gameObject.SetActive(value: false);
		btn_Blend.gameObject.SetActive(value: false);
		go_MainSlot.SetActive(value: false);
		go_BtnClose.SetActive(value: false);
		disactiveOnBlendMobile.ForEach(delegate(GameObject x)
		{
			x.SetActive(value: false);
		});
		uiInfoSpell.gameObject.SetActive(value: false);
		panel_Arrow.SetActive(value: false);
		for (int i = 0; i < uiSMIO_Waits.Length; i++)
		{
			uiSMIO_Waits[i].Blending();
		}
		SpecialObj101MoreInOne_Dots componentData = ettMgr.GetComponentData<SpecialObj101MoreInOne_Dots>(so101MoreInOneEtt);
		componentData.isUse = true;
		ettMgr.SetComponentData(so101MoreInOneEtt, componentData);
		int num = uiSMIO_Waits[UnityEngine.Random.Range(0, uiSMIO_Waits.Length)].ID + 1;
		uiSMIO_Result.GetSpellDirect(num);
		uiSMIO_Result.image_Icon.gameObject.SetActive(value: false);
		bool flag = false;
		List<(int, int, WandSlotType)> list = new List<(int, int, WandSlotType)>();
		UISlotMoreInOne[] array = uiSMIO_Waits;
		foreach (UISlotMoreInOne uISlotMoreInOne in array)
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					if (PlayerMgr.Inst.BaData.bagSpellDatas[num2] != null && PlayerMgr.Inst.BaData.bagSpellDatas[num2].id == uISlotMoreInOne.ID)
					{
						PlayerMgr.Inst.BagSpellChange(num2, null);
						flag = true;
						break;
					}
					num2++;
					continue;
				}
				for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs.Count; k++)
				{
					WandConfig wandConfig = PlayerMgr.Inst.BaData.wandCfgs[k];
					if (wandConfig == null)
					{
						continue;
					}
					int num3 = 0;
					while (num3 < wandConfig.normalSlots.Length)
					{
						if (wandConfig.normalSlots[num3] == null || wandConfig.normalSlotIsLock[num3] || wandConfig.normalSlots[num3].id != uISlotMoreInOne.ID)
						{
							num3++;
							continue;
						}
						goto IL_0217;
					}
					int num4 = 0;
					while (num4 < wandConfig.postSlots.Length)
					{
						if (wandConfig.postSlots[num4] == null || wandConfig.postSlotIsLock[num4] || wandConfig.postSlots[num4].id != uISlotMoreInOne.ID)
						{
							num4++;
							continue;
						}
						goto IL_0284;
					}
					continue;
					IL_0284:
					PlayerMgr.Inst.ChangeWandSpell(k, WandSlotType.Post, num4, null);
					list.Add((k, num4, WandSlotType.Post));
					break;
					IL_0217:
					PlayerMgr.Inst.ChangeWandSpell(k, WandSlotType.Normal, num3, null);
					list.Add((k, num3, WandSlotType.Normal));
					break;
				}
				break;
			}
		}
		SlotData slotData = new SlotData(num);
		bool flag2 = false;
		if (flag)
		{
			PlayerMgr.Inst.SpellPick(slotData);
		}
		else
		{
			foreach (var (wandIndex, spellIndex, slotType) in list)
			{
				if (PlayerMgr.Inst.CanChangeInWandSpell(wandIndex, slotType, spellIndex, slotData))
				{
					PlayerMgr.Inst.ChangeWandSpell(wandIndex, slotType, spellIndex, slotData);
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				PlayerMgr.Inst.SpellPick(slotData);
			}
		}
		SEMgr.Inst.so101_Blend.PlaySE();
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, num);
		if (SpellConfig.dic[num].level == 3)
		{
			SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.GetLevel3Spell);
		}
	}
}
