using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UISell")]
public class UISell : GameUISingletonMono<UISell>
{
	public UISell_Item pfb_UISellItem;

	public RectTransform rtsf_BtnMask;

	public RectTransform rtsf_Mask;

	public RectTransform rtsf_Center;

	public RectTransform rtsf_Items;

	public Animator anima;

	public Image image_Price;

	public Text text_Price;

	public Text text_Tip;

	public UIInfoSpell uiInfoSpell;

	public UIInfoRelic uiInfoRelic;

	public UIInfoPotion uiInfoPotion;

	public float3 followOffset;

	public float space;

	public float sellingDuration;

	public float focusSize;

	public float focusTime;

	[Header("InputChange")]
	public GameObject panel_Arrow;

	public Image image_Shortcut;

	public Sprite sprite_ShortcutKeyborad;

	public Sprite sprite_ShortcutGamepad;

	[Header("LanguageChange")]
	public Text text_Sell;

	[Header("ControlChangeUI")]
	public UpdatButtonShow[] updatebuttonshows;

	private Dictionary<int, int> dic_Splls = new Dictionary<int, int>();

	private Dictionary<int, int> dic_Relics = new Dictionary<int, int>();

	private Dictionary<int, int> dic_Potions = new Dictionary<int, int>();

	private List<UISell_Item> uiSIs = new List<UISell_Item>();

	private int SelectedItemIndex;

	private bool isSelling;

	private float sellingDurationTimer;

	private EntityManager ettMgr;

	private Entity so21Ett;

	private UISell_Item SelectedUISI => uiSIs[SelectedItemIndex];

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
		if (SelectedItemIndex > 0)
		{
			SelectedItemIndex--;
			uiInfoSpell.gameObject.SetActive(value: false);
			uiInfoRelic.gameObject.SetActive(value: false);
			uiInfoPotion.gameObject.SetActive(value: false);
			switch (SelectedUISI.ItemType)
			{
			case UISellItemType.Spell:
				uiInfoSpell.gameObject.SetActive(value: true);
				uiInfoSpell.UpdateInfo(SelectedUISI.ItemID);
				break;
			case UISellItemType.Relic:
				uiInfoRelic.gameObject.SetActive(value: true);
				uiInfoRelic.UpdateInfo(PlayerMgr.Inst.ItemCtrller.GetRelicConfig(SelectedUISI.ItemID));
				break;
			case UISellItemType.Potion:
				uiInfoPotion.gameObject.SetActive(value: true);
				uiInfoPotion.UpdateInfo(SelectedUISI.ItemID);
				break;
			default:
				Debug.LogError(SelectedUISI.ItemType);
				break;
			}
			for (int i = 0; i < uiSIs.Count; i++)
			{
				uiSIs[i].SetMove(SelectedItemIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	public void InteractRight()
	{
		if (SelectedItemIndex < uiSIs.Count - 1)
		{
			SelectedItemIndex++;
			uiInfoSpell.gameObject.SetActive(value: false);
			uiInfoRelic.gameObject.SetActive(value: false);
			uiInfoPotion.gameObject.SetActive(value: false);
			switch (SelectedUISI.ItemType)
			{
			case UISellItemType.Spell:
				uiInfoSpell.gameObject.SetActive(value: true);
				uiInfoSpell.UpdateInfo(SelectedUISI.ItemID);
				break;
			case UISellItemType.Relic:
				uiInfoRelic.gameObject.SetActive(value: true);
				uiInfoRelic.UpdateInfo(PlayerMgr.Inst.ItemCtrller.GetRelicConfig(SelectedUISI.ItemID));
				break;
			case UISellItemType.Potion:
				uiInfoPotion.gameObject.SetActive(value: true);
				uiInfoPotion.UpdateInfo(SelectedUISI.ItemID);
				break;
			default:
				Debug.LogError(SelectedUISI.ItemType);
				break;
			}
			for (int i = 0; i < uiSIs.Count; i++)
			{
				uiSIs[i].SetMove(SelectedItemIndex);
			}
			UpdateInfo();
			SEMgr.Inst.uiSwitch.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen && !rtsf_BtnMask.gameObject.activeSelf)
		{
			_Sell();
		}
	}

	private void LanguageChange()
	{
		text_Sell.text = 1000910.GetText();
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

	protected override void OnShow(object obj = null)
	{
		if (!(obj is Entity))
		{
			return;
		}
		Entity entity = (so21Ett = (Entity)obj);
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		if (GameMgr.IsMobile_Static)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so21Ett);
			CamController.Inst.FocusOn(focusSize, focusTime, componentData.Position + UIBattleMgr.Inst.UIProcessPositionDefault * CamController.Inst.FocusCamSizeRatio);
		}
		else
		{
			CamController.Inst.FocusOn(focusSize, focusTime);
		}
		rtsf_Items.DestroyAllChild();
		uiSIs.Clear();
		dic_Splls.Clear();
		dic_Relics.Clear();
		dic_Potions.Clear();
		SelectedItemIndex = 0;
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
		for (int m = 0; m < PlayerMgr.Inst.BaData.relicCfgs.Count; m++)
		{
			dic_Relics.Add(PlayerMgr.Inst.BaData.relicCfgs[m].id, PlayerMgr.Inst.BaData.relicCfgs[m].level);
		}
		for (int n = 0; n < PlayerMgr.Inst.BaData.potionIDs.Count; n++)
		{
			if (PlayerMgr.Inst.BaData.potionIDs[n] != 0)
			{
				if (dic_Potions.ContainsKey(PlayerMgr.Inst.BaData.potionIDs[n]))
				{
					dic_Potions[PlayerMgr.Inst.BaData.potionIDs[n]]++;
				}
				else
				{
					dic_Potions.Add(PlayerMgr.Inst.BaData.potionIDs[n], 1);
				}
			}
		}
		foreach (KeyValuePair<int, int> dic_Spll in dic_Splls)
		{
			if (dic_Spll.Key != 0)
			{
				UISell_Item uISell_Item = UnityEngine.Object.Instantiate(pfb_UISellItem, rtsf_Items);
				uISell_Item.Initialize(uiSIs.Count, UISellItemType.Spell, dic_Spll.Key, dic_Spll.Value);
				uiSIs.Add(uISell_Item);
			}
		}
		foreach (KeyValuePair<int, int> dic_Relic in dic_Relics)
		{
			UISell_Item uISell_Item2 = UnityEngine.Object.Instantiate(pfb_UISellItem, rtsf_Items);
			uISell_Item2.Initialize(uiSIs.Count, UISellItemType.Relic, dic_Relic.Key, dic_Relic.Value);
			uiSIs.Add(uISell_Item2);
		}
		foreach (KeyValuePair<int, int> dic_Potion in dic_Potions)
		{
			UISell_Item uISell_Item3 = UnityEngine.Object.Instantiate(pfb_UISellItem, rtsf_Items);
			uISell_Item3.Initialize(uiSIs.Count, UISellItemType.Potion, dic_Potion.Key, dic_Potion.Value);
			uiSIs.Add(uISell_Item3);
		}
		UpdateInfo();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Coin);
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
		Selling();
		Following();
	}

	private void Selling()
	{
		if (!isSelling)
		{
			return;
		}
		sellingDurationTimer += Time.deltaTime;
		if (sellingDurationTimer >= sellingDuration)
		{
			isSelling = false;
			if (uiSIs.Count > 0)
			{
				rtsf_BtnMask.gameObject.SetActive(value: false);
			}
		}
	}

	private void Following()
	{
		if (so21Ett != Entity.Null)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(so21Ett);
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

	private int GetPrice()
	{
		int num = 0;
		switch (SelectedUISI.ItemType)
		{
		case UISellItemType.Spell:
			num = SpellConfig.dic[SelectedUISI.ItemID].priceCoin;
			break;
		case UISellItemType.Relic:
			num = RelicConfig.dic[SelectedUISI.ItemID].priceCoin;
			break;
		case UISellItemType.Potion:
			num = PotionConfig.dic[SelectedUISI.ItemID].priceCoin;
			break;
		default:
			Debug.LogError(SelectedUISI.ItemType);
			break;
		}
		if (GameMgr.InEndlessMode)
		{
			num = Mathf.FloorToInt((float)num * (1f + (float)(BattleMgr.Inst.CurrentLevel / 10)) * 0.5f);
		}
		return num;
	}

	private void UpdateInfo()
	{
		rtsf_BtnMask.gameObject.SetActive(value: true);
		uiInfoSpell.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		uiInfoPotion.gameObject.SetActive(value: false);
		image_Price.gameObject.SetActive(value: false);
		if (uiSIs.Count > 0)
		{
			rtsf_BtnMask.gameObject.SetActive(value: false);
			switch (SelectedUISI.ItemType)
			{
			case UISellItemType.Spell:
				uiInfoSpell.gameObject.SetActive(value: true);
				uiInfoSpell.UpdateInfo(SelectedUISI.ItemID);
				break;
			case UISellItemType.Relic:
				uiInfoRelic.gameObject.SetActive(value: true);
				uiInfoRelic.UpdateInfo(PlayerMgr.Inst.ItemCtrller.GetRelicConfig(SelectedUISI.ItemID));
				break;
			case UISellItemType.Potion:
				uiInfoPotion.gameObject.SetActive(value: true);
				uiInfoPotion.UpdateInfo(SelectedUISI.ItemID);
				break;
			default:
				Debug.LogError(SelectedUISI.ItemType);
				break;
			}
			image_Price.gameObject.SetActive(value: true);
			text_Price.text = GetPrice().ToString();
		}
	}

	protected override void OnHide()
	{
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		so21Ett = Entity.Null;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(focusTime);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Coin);
	}

	public void _Sell()
	{
		SEMgr.Inst.so2101_Recycle.PlaySE();
		int price = GetPrice();
		PlayerMgr.Inst.ChangeCoin(price);
		ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + price, UITextFloatType.GetCoin, PlayerMgr.Inst.PlayerPointIgnoreZ);
		switch (SelectedUISI.ItemType)
		{
		case UISellItemType.Spell:
		{
			bool flag = false;
			for (int j = 0; j < PlayerMgr.Inst.BaData.bagSpellDatas.Count; j++)
			{
				if (PlayerMgr.Inst.BaData.bagSpellDatas[j] != null && PlayerMgr.Inst.BaData.bagSpellDatas[j].id == SelectedUISI.ItemID)
				{
					PlayerMgr.Inst.Slot_RemoveBagSlot(j);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			for (int k = 0; k < PlayerMgr.Inst.BaData.wandCfgs.Count; k++)
			{
				if (PlayerMgr.Inst.BaData.wandCfgs[k] == null)
				{
					continue;
				}
				for (int l = 0; l < PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots.Length; l++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l] != null && !PlayerMgr.Inst.BaData.wandCfgs[k].normalSlotIsLock[l] && PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].id == SelectedUISI.ItemID && !PlayerMgr.Inst.BaData.wandCfgs[k].normalSlots[l].isAllFieldSharedSpell)
					{
						PlayerMgr.Inst.ChangeWandSpell(k, WandSlotType.Normal, l, null);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int m = 0; m < PlayerMgr.Inst.BaData.wandCfgs[k].postSlots.Length; m++)
				{
					if (PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m] != null && !PlayerMgr.Inst.BaData.wandCfgs[k].postSlotIsLock[m] && PlayerMgr.Inst.BaData.wandCfgs[k].postSlots[m].id == SelectedUISI.ItemID)
					{
						PlayerMgr.Inst.ChangeWandSpell(k, WandSlotType.Post, m, null);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			break;
		}
		case UISellItemType.Relic:
			PlayerMgr.Inst.ItemCtrller.RelicRemove(SelectedUISI.ItemID, 1);
			break;
		case UISellItemType.Potion:
		{
			for (int i = 0; i < PlayerMgr.Inst.BaData.potionIDs.Count; i++)
			{
				if (SelectedUISI.ItemID == PlayerMgr.Inst.BaData.potionIDs[i])
				{
					PlayerMgr.Inst.ItemCtrller.PotionRemove(i);
				}
			}
			break;
		}
		default:
			Debug.LogError(SelectedUISI.ItemType);
			break;
		}
		switch (SelectedUISI.ItemType)
		{
		case UISellItemType.Spell:
			if (dic_Splls[SelectedUISI.ItemID] - 1 == 0)
			{
				dic_Splls.Remove(SelectedUISI.ItemID);
				UnityEngine.Object.Destroy(SelectedUISI.gameObject);
				uiSIs.RemoveAt(SelectedItemIndex);
				if (SelectedItemIndex == uiSIs.Count)
				{
					SelectedItemIndex--;
				}
				for (int num = 0; num < uiSIs.Count; num++)
				{
					uiSIs[num].ChangeIndex(num, SelectedItemIndex);
				}
			}
			else
			{
				dic_Splls[SelectedUISI.ItemID]--;
				SelectedUISI.ChangeCount(dic_Splls[SelectedUISI.ItemID]);
			}
			break;
		case UISellItemType.Relic:
			if (dic_Relics[SelectedUISI.ItemID] - 1 == 0)
			{
				dic_Relics.Remove(SelectedUISI.ItemID);
				UnityEngine.Object.Destroy(SelectedUISI.gameObject);
				uiSIs.RemoveAt(SelectedItemIndex);
				if (SelectedItemIndex == uiSIs.Count)
				{
					SelectedItemIndex--;
				}
				for (int num2 = 0; num2 < uiSIs.Count; num2++)
				{
					uiSIs[num2].ChangeIndex(num2, SelectedItemIndex);
				}
			}
			else
			{
				dic_Relics[SelectedUISI.ItemID]--;
				SelectedUISI.ChangeCount(dic_Relics[SelectedUISI.ItemID]);
			}
			break;
		case UISellItemType.Potion:
			if (dic_Potions[SelectedUISI.ItemID] - 1 == 0)
			{
				dic_Potions.Remove(SelectedUISI.ItemID);
				UnityEngine.Object.Destroy(SelectedUISI.gameObject);
				uiSIs.RemoveAt(SelectedItemIndex);
				if (SelectedItemIndex == uiSIs.Count)
				{
					SelectedItemIndex--;
				}
				for (int n = 0; n < uiSIs.Count; n++)
				{
					uiSIs[n].ChangeIndex(n, SelectedItemIndex);
				}
			}
			else
			{
				dic_Potions[SelectedUISI.ItemID]--;
				SelectedUISI.ChangeCount(dic_Potions[SelectedUISI.ItemID]);
			}
			break;
		default:
			Debug.LogError(SelectedUISI.ItemType);
			break;
		}
		SpecialObj21_Dots componentData = ettMgr.GetComponentData<SpecialObj21_Dots>(so21Ett);
		bool num3 = componentData.UseOnce();
		ettMgr.SetComponentData(so21Ett, componentData);
		if (num3)
		{
			sellingDurationTimer = 0f;
			isSelling = true;
			UpdateInfo();
		}
		else
		{
			_Close();
		}
	}
}
