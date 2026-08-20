using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[GameUISingletonPrefab("UICampSkinChanger")]
public class UICampSkinChanger : GameUISingletonMono<UICampSkinChanger>
{
	public Animator anima;

	public Button btn_Close;

	public Text text_Title;

	public List<UICampSkinSlot> uiCampSkinSlotList;

	private int selectedCampSkinIndex;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed += GamepadDirectPerformed;
		base.inputActions.Player.Interact.performed += GamepadInteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed -= GamepadDirectPerformed;
		base.inputActions.Player.Interact.performed -= GamepadInteractPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	protected override IEnumerator OnInit()
	{
		btn_Close.onClick.AddListener(_Close);
		for (int num = uiCampSkinSlotList.Count - 1; num >= 0; num--)
		{
			uiCampSkinSlotList[num].uiCampSkinChanger = this;
			if (!PlayerHaveCampSkin(uiCampSkinSlotList[num].skinType))
			{
				uiCampSkinSlotList[num].gameObject.SetActive(value: false);
				uiCampSkinSlotList.RemoveAt(num);
			}
		}
		LanguageChange();
		yield return null;
	}

	private void GamepadInteractPerformed(InputAction.CallbackContext obj)
	{
		if (uiCampSkinSlotList.Count > 0)
		{
			UICampSkinClick(uiCampSkinSlotList[selectedCampSkinIndex]);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (GameUISingletonMono<UICampSkinChanger>.StaticIsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirectionNav(vector);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (GameUISingletonMono<UICampSkinChanger>.StaticIsOpen)
		{
			MoveDirectionNav(context.ReadValue<Vector2>());
		}
	}

	private void MoveDirectionNav(Vector2 direct)
	{
		if (uiCampSkinSlotList.Count == 0)
		{
			return;
		}
		if (direct == Vector2.up)
		{
			selectedCampSkinIndex--;
		}
		else
		{
			if (!(direct == Vector2.down))
			{
				return;
			}
			selectedCampSkinIndex++;
		}
		selectedCampSkinIndex = Mathf.Clamp(selectedCampSkinIndex, 0, uiCampSkinSlotList.Count - 1);
		SelectTheme(selectedCampSkinIndex);
	}

	private void LanguageChange()
	{
		text_Title.text = 1004151.GetText();
		for (int i = 0; i < uiCampSkinSlotList.Count; i++)
		{
			uiCampSkinSlotList[i].LanguageChange();
		}
	}

	private bool PlayerHaveCampSkin(CampSkinType campSkinType)
	{
		switch (campSkinType)
		{
		case CampSkinType.Default:
			return true;
		case CampSkinType.Halloween:
			return ICJNOGPFMAM.FIKDMCBJPCO;
		case CampSkinType.Spring:
			return ICJNOGPFMAM.ACPKKMJKOJD;
		case CampSkinType.Summer:
			return ICJNOGPFMAM.BHEHHIFGJOE;
		case CampSkinType.Christmas:
			return ICJNOGPFMAM.MADIIMLEMNP;
		default:
			Debug.LogError(campSkinType);
			return false;
		}
	}

	protected override void OnShow(object obj = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
		selectedCampSkinIndex = uiCampSkinSlotList.FindIndex((UICampSkinSlot slot) => slot.skinType == DataMgr.selectedWorldData.campSkinType);
		SelectTheme(Mathf.Max(selectedCampSkinIndex, 0));
	}

	protected override void OnHide()
	{
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}

	public void SelectTheme(int index)
	{
		if (index >= 0 && index < uiCampSkinSlotList.Count)
		{
			uiCampSkinSlotList.ForEach(delegate(UICampSkinSlot uiSlot)
			{
				uiSlot.OnPointerExit(null);
			});
			uiCampSkinSlotList[index].OnPointerEnter(null);
			selectedCampSkinIndex = index;
		}
	}

	public void UICampSkinClick(UICampSkinSlot uiCampSkinSlot)
	{
		if (DataMgr.selectedWorldData.campSkinType != uiCampSkinSlot.skinType)
		{
			SEMgr.Inst.uiClick.PlaySE();
			DataMgr.selectedWorldData.campSkinType = uiCampSkinSlot.skinType;
			DataMgr.SaveSelectedWorldData();
			GameMgr.Inst.RecycleAllPool();
			SceneManager.LoadScene("Camp");
		}
	}
}
