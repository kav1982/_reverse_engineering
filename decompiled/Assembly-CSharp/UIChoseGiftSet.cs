using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIChoseSet")]
public class UIChoseGiftSet : GameUISingletonMono<UIChoseGiftSet>
{
	public float yoffset;

	public GameObject pfb_Slot;

	public GameObject UnlockButton;

	public GameObject UnlockButtonSelect;

	public Color UpgradeAvailible;

	public Color UpgradeDisable;

	public Text textUnlock;

	public RectTransform rtsf_SlotMotion;

	public Animator anima;

	public UIInfoWand uiInfoWand;

	public UIInfoRelic uiInfoRelic;

	public float slotSpace;

	[Header("LanguageChange")]
	public Text text_Title;

	public List<UpdatButtonShow> updatButtonShows;

	public List<Image> imageButtons;

	public List<Image> imageButtonsCantSelect;

	public List<Image> imageButtonsGamepadSelectFrame;

	private bool[] slotCantSelect;

	public Color SkeletonFade;

	private UISetSlot[] slots;

	private int selectedIndex;

	public Text text1;

	public Text text2;

	public Text text3;

	public Text textOtherCanBeUnlock;

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
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.LeftStick.performed -= DirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
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
		if (base.IsOpen)
		{
			switch (selectedIndex)
			{
			case 0:
				UnlockButtonClick(7);
				break;
			case 1:
				UnlockButtonClick(8);
				break;
			case 2:
				UnlockButtonClick(9);
				break;
			}
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
		if (_direct == Vector2.left)
		{
			selectedIndex--;
			selectedIndex = Mathf.Clamp(selectedIndex, 0, 2);
			imageButtonsGamepadSelectFrame[0].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[1].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[2].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[selectedIndex].gameObject.SetActive(value: true);
			UnlockButtonPointIn(selectedIndex);
		}
		else if (_direct == Vector2.right)
		{
			selectedIndex++;
			selectedIndex = Mathf.Clamp(selectedIndex, 0, 2);
			imageButtonsGamepadSelectFrame[0].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[1].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[2].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[selectedIndex].gameObject.SetActive(value: true);
			UnlockButtonPointIn(selectedIndex);
		}
	}

	private void LanguageChange()
	{
		text_Title.text = 1006303.GetText();
		textUnlock.text = 1002106.GetText();
		if (slots != null)
		{
			UISetSlot[] array = slots;
			foreach (UISetSlot uISetSlot in array)
			{
				if ((bool)uISetSlot.GetComponent<UISetSlot>())
				{
					uISetSlot.GetComponent<UISetSlot>().Resetname(GiftSet: true);
				}
			}
		}
		text1.text = (slotCantSelect[0] ? 1006301.GetText() : 1006302.GetText());
		text2.text = (slotCantSelect[1] ? 1006301.GetText() : 1006302.GetText());
		text3.text = (slotCantSelect[2] ? 1006301.GetText() : 1006302.GetText());
		text1.color = (slotCantSelect[0] ? Color.green : Color.red);
		text2.color = (slotCantSelect[1] ? Color.green : Color.red);
		text3.color = (slotCantSelect[2] ? Color.green : Color.red);
		textOtherCanBeUnlock.text = 1006306.GetText();
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			imageButtonsGamepadSelectFrame[0].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[1].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[2].gameObject.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			selectedIndex = 0;
			imageButtonsGamepadSelectFrame[0].gameObject.SetActive(value: true);
			imageButtonsGamepadSelectFrame[1].gameObject.SetActive(value: false);
			imageButtonsGamepadSelectFrame[2].gameObject.SetActive(value: false);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	protected override IEnumerator OnInit()
	{
		InputChange();
		CreateSet();
		yield return null;
	}

	public void UnlockButtonClick(int index)
	{
		bool flag = false;
		switch (index)
		{
		case 7:
			if (slotCantSelect[0])
			{
				DataMgr.selectedWorldData.SetFindSet7();
				flag = true;
			}
			break;
		case 8:
			if (slotCantSelect[1])
			{
				DataMgr.selectedWorldData.SetFindSet8();
				flag = true;
			}
			break;
		case 9:
			if (slotCantSelect[2])
			{
				DataMgr.selectedWorldData.ForceSetFindSet9();
				flag = true;
			}
			break;
		}
		if (flag)
		{
			DataMgr.selectedWorldData.useGift = true;
			DataMgr.SaveSelectedWorldData();
			Hide();
			CampMgr.Inst.SetEttEnable(CampMgr.Inst.CurrentCampSkin.ett_GiftSet, enable: false);
			UnityEngine.Object.Destroy(this);
			GameUISingletonMono<UIChoseGiftSet>.DestroyUI(2f);
		}
	}

	public void UnlockButtonPointIn(int index)
	{
		Image[] componentsInChildren = slots[2].GetComponentsInChildren<Image>();
		if (!slotCantSelect[index])
		{
			return;
		}
		UpdateInfoCurrentLevel(index + 6);
		for (int i = 0; i < imageButtons.Count(); i++)
		{
			if (i != index)
			{
				imageButtons[i].DOFade(0.3f, 0.5f);
				slots[i].text_Name.DOFade(0.3f, 0.5f);
				slots[i].sGraphic.DOColor(SkeletonFade, 0.5f);
			}
			else
			{
				imageButtons[i].DOFade(1f, 0.5f);
				slots[i].text_Name.DOFade(1f, 0.5f);
				slots[i].sGraphic.DOColor(Color.white, 0.5f);
			}
		}
		if (index == 2)
		{
			Image[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].DOColor(Color.white, 0.5f);
			}
		}
		else
		{
			Image[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].DOColor(SkeletonFade, 0.5f);
			}
		}
	}

	public void UnlockButtonPointOut()
	{
		Image[] componentsInChildren = slots[2].GetComponentsInChildren<Image>();
		for (int i = 0; i < imageButtons.Count(); i++)
		{
			imageButtons[i].DOFade(1f, 0.5f);
			slots[i].text_Name.DOFade(1f, 0.5f);
			slots[i].sGraphic.DOColor(Color.white, 0.5f);
		}
		Image[] array = componentsInChildren;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].DOColor(Color.white, 0.5f);
		}
	}

	public void UpdateInfoCurrentLevel(int index)
	{
		if (SetConfig.list[index].WandIDs.Length != 0)
		{
			uiInfoWand.gameObject.SetActive(value: true);
			uiInfoWand.UpdateInfo(WandConfig.dic[SetConfig.list[index].WandIDs[0]], null, ItemIsStore: true, ChangeAlpha: false);
		}
		if (SetConfig.list[index].relicID != 0)
		{
			uiInfoRelic.gameObject.SetActive(value: true);
			RelicConfig config = RelicConfig.GetConfig(SetConfig.list[index].relicID);
			config.level = 1;
			uiInfoRelic.UpdateInfo(config);
		}
	}

	private void CreateSet()
	{
		rtsf_SlotMotion.DestroyAllChild();
		slots = new UISetSlot[3];
		slots[0] = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		slots[0].Initialize(null, SetConfig.list[6].id, isFake: false, GiftSet: true);
		float x = 0f - slotSpace;
		((RectTransform)slots[0].transform).localPosition = new Vector3(x, 0f, 0f);
		slots[1] = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		slots[1].Initialize(null, SetConfig.list[7].id, isFake: false, GiftSet: true);
		float x2 = 0f;
		((RectTransform)slots[1].transform).localPosition = new Vector3(x2, 0f, 0f);
		slots[2] = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_SlotMotion).GetComponent<UISetSlot>();
		slots[2].Initialize(null, SetConfig.list[8].id, isFake: false, GiftSet: true);
		float x3 = slotSpace;
		((RectTransform)slots[2].transform).localPosition = new Vector3(x3, 0f, 0f);
	}

	protected override void OnShow(object obj = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		InputChange();
		anima.SetTrigger("Appear");
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
		slotCantSelect = new bool[3];
		slotCantSelect[0] = !DataMgr.selectedWorldData.IsSetUnlocked(7);
		slotCantSelect[1] = !DataMgr.selectedWorldData.IsSetUnlocked(8);
		slotCantSelect[2] = !DataMgr.selectedWorldData.IsSetUnlocked(9);
		imageButtonsCantSelect[0].gameObject.SetActive(!slotCantSelect[0]);
		imageButtonsCantSelect[1].gameObject.SetActive(!slotCantSelect[1]);
		imageButtonsCantSelect[2].gameObject.SetActive(!slotCantSelect[2]);
		LanguageChange();
		if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			StartCoroutine(Delay_PointInFirst());
		}
	}

	protected override void OnHide()
	{
		UnlockButtonPointOut();
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		SEMgr.Inst.uiClose.PlaySE();
	}

	public IEnumerator Delay_PointInFirst()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		UnlockButtonPointIn(0);
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}
}
