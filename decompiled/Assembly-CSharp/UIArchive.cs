using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIArchive : MonoBehaviour
{
	public GameObject mainUI;

	public CanvasGroup CanvasGroup;

	public bool canInteract = true;

	public UIArchiveSlot[] uiArchiveSlots;

	public Animator anima;

	[Header("AreYouSure")]
	public GameObject panel_AreYouSure;

	public GameObject go_SureSelection;

	public Button btn_Yes;

	public Button btn_No;

	[Header("Lanugage")]
	public Text text_ConfirmTitle;

	public Text text_Yes;

	public Text text_No;

	public Text text_ConfirmTitle_Panel_Skip;

	public Text text_Yes_Panel_Skip;

	public Text text_No_Panel_Skip;

	[Header("Skip")]
	public GameObject Panel_Skip;

	public GameObject go_SureSelection_Panel_Skip;

	public Button btnPanel_Skip_Yes;

	public Button btnPanel_Skip_No;

	private InputActions inputActions;

	private int selectedDataIndex;

	private int deleteDataIndex = -1;

	public bool IsOpen { get; private set; }

	public bool IsAreYouSureOpen => panel_AreYouSure.activeSelf;

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		inputActions.Player.Interact.performed += InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChance));
		if (GameMgr.IsMobile_Static)
		{
			switch (MobileMgr.inst.screenType)
			{
			case MobileMgr.ScreenType.Wide:
				mainUI.transform.localScale = Vector3.one * (1f * MobileMgr.inst.screenRatio / 1.5f);
				break;
			case MobileMgr.ScreenType.Normal:
				mainUI.transform.localScale = Vector3.one * (1f * MobileMgr.inst.screenRatio / 1.77f);
				break;
			case MobileMgr.ScreenType.Long:
				mainUI.transform.localScale = Vector3.one * (1f * MobileMgr.inst.screenRatio / 2.11f);
				break;
			}
		}
	}

	private void OnDisable()
	{
		inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		inputActions.Player.Interact.performed -= InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChance));
	}

	public void OpenCloudArchive()
	{
		PluginActivity.Inst.OpenArchiveManager();
		_Close();
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDire(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDire(vector);
		}
	}

	private void MoveDire(Vector2 _direct)
	{
		if (_direct == Vector2.up)
		{
			if (!panel_AreYouSure.activeSelf && deleteDataIndex != -1)
			{
				selectedDataIndex = deleteDataIndex;
				deleteDataIndex = -1;
				uiArchiveSlots[selectedDataIndex].Select();
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (_direct == Vector2.down)
		{
			if (!panel_AreYouSure.activeSelf && deleteDataIndex == -1 && uiArchiveSlots[selectedDataIndex].SelfData.haveUsed)
			{
				deleteDataIndex = selectedDataIndex;
				uiArchiveSlots[selectedDataIndex].SelectDelete();
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (_direct == Vector2.left)
		{
			if (Panel_Skip.activeSelf)
			{
				if (go_SureSelection_Panel_Skip.transform.position == btnPanel_Skip_No.transform.position)
				{
					go_SureSelection_Panel_Skip.transform.position = btnPanel_Skip_Yes.transform.position;
				}
				else
				{
					go_SureSelection_Panel_Skip.transform.position = btnPanel_Skip_No.transform.position;
				}
			}
			else if (panel_AreYouSure.activeSelf)
			{
				if (go_SureSelection.transform.position == btn_Yes.transform.position)
				{
					go_SureSelection.transform.position = btn_No.transform.position;
				}
				else
				{
					go_SureSelection.transform.position = btn_Yes.transform.position;
				}
			}
			else
			{
				uiArchiveSlots[selectedDataIndex].Unselect();
				selectedDataIndex--;
				if (selectedDataIndex < 0)
				{
					selectedDataIndex = uiArchiveSlots.Length - 1;
				}
				if (deleteDataIndex == -1)
				{
					uiArchiveSlots[selectedDataIndex].Select();
				}
				else if (uiArchiveSlots[selectedDataIndex].SelfData.haveUsed)
				{
					uiArchiveSlots[selectedDataIndex].SelectDelete();
					deleteDataIndex = selectedDataIndex;
				}
				else
				{
					uiArchiveSlots[selectedDataIndex].Select();
					deleteDataIndex = -1;
				}
			}
			SEMgr.Inst.uiButtonHover_Button.PlaySE();
		}
		else
		{
			if (!(_direct == Vector2.right))
			{
				return;
			}
			if (Panel_Skip.activeSelf)
			{
				if (go_SureSelection_Panel_Skip.transform.position == btnPanel_Skip_No.transform.position)
				{
					go_SureSelection_Panel_Skip.transform.position = btnPanel_Skip_Yes.transform.position;
				}
				else
				{
					go_SureSelection_Panel_Skip.transform.position = btnPanel_Skip_No.transform.position;
				}
			}
			else if (panel_AreYouSure.activeSelf)
			{
				if (go_SureSelection.transform.position == btn_Yes.transform.position)
				{
					go_SureSelection.transform.position = btn_No.transform.position;
				}
				else
				{
					go_SureSelection.transform.position = btn_Yes.transform.position;
				}
			}
			else
			{
				uiArchiveSlots[selectedDataIndex].Unselect();
				selectedDataIndex++;
				if (selectedDataIndex >= uiArchiveSlots.Length)
				{
					selectedDataIndex = 0;
				}
				if (deleteDataIndex == -1)
				{
					uiArchiveSlots[selectedDataIndex].Select();
				}
				else if (uiArchiveSlots[selectedDataIndex].SelfData.haveUsed)
				{
					uiArchiveSlots[selectedDataIndex].SelectDelete();
					deleteDataIndex = selectedDataIndex;
				}
				else
				{
					uiArchiveSlots[selectedDataIndex].Select();
					deleteDataIndex = -1;
				}
			}
			SEMgr.Inst.uiButtonHover_Button.PlaySE();
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || !IsOpen)
		{
			return;
		}
		if (Panel_Skip.activeSelf)
		{
			if (go_SureSelection_Panel_Skip.transform.position == btnPanel_Skip_Yes.transform.position)
			{
				NewGameSkip();
			}
			else
			{
				NewGameDontSkip();
			}
		}
		else if (panel_AreYouSure.activeSelf)
		{
			if (go_SureSelection.transform.position == btn_Yes.transform.position)
			{
				_DeleteYes();
				uiArchiveSlots[selectedDataIndex].Select();
				deleteDataIndex = -1;
			}
			else
			{
				_DeleteNo();
			}
		}
		else if (deleteDataIndex == -1)
		{
			uiArchiveSlots[selectedDataIndex].OnPointerDown(null);
		}
		else
		{
			DeleteData(deleteDataIndex);
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
		{
			for (int i = 0; i < uiArchiveSlots.Length; i++)
			{
				if (uiArchiveSlots[i].go_SelectDelete.activeSelf)
				{
					uiArchiveSlots[i].Select();
				}
			}
			go_SureSelection.SetActive(value: false);
			go_SureSelection_Panel_Skip.SetActive(value: false);
			break;
		}
		case PlayerInputType.Gamepad:
			go_SureSelection.SetActive(value: true);
			go_SureSelection_Panel_Skip.SetActive(value: true);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void LanguageChance()
	{
		text_ConfirmTitle_Panel_Skip.text = 1000015.GetText();
		text_Yes_Panel_Skip.text = 1000208.GetText();
		text_No_Panel_Skip.text = 1000209.GetText();
		text_ConfirmTitle.text = 1000007.GetText();
		text_Yes.text = 1000208.GetText();
		text_No.text = 1000209.GetText();
	}

	public void LoadSavefile()
	{
		DataMgr.LoadWorldData();
		for (int i = 0; i < uiArchiveSlots.Length; i++)
		{
			uiArchiveSlots[i].DataIndex = uiArchiveSlots[i].transform.GetSiblingIndex();
		}
		for (int j = 0; j < uiArchiveSlots.Length; j++)
		{
			uiArchiveSlots[j].Initialize();
		}
		uiArchiveSlots[0].Select();
		InputChange();
		LanguageChance();
	}

	private void Start()
	{
		LoadSavefile();
	}

	public void Show()
	{
		anima.SetTrigger("Show");
		IsOpen = true;
		if ((bool)UIMainMenuMgr.Inst)
		{
			UIMainMenuMgr.Inst.HideParticle();
		}
		if (DataMgr.OverlayCurrentSaveData())
		{
			LoadSavefile();
		}
	}

	public void SlotOnClick(int selectIndex)
	{
		uiArchiveSlots[selectedDataIndex].Unselect();
		selectedDataIndex = selectIndex;
		uiArchiveSlots[selectedDataIndex].Select();
		DataMgr.SetSelectedWorldData(selectedDataIndex);
	}

	public void DeleteData(int deleteIndex)
	{
		deleteDataIndex = deleteIndex;
		panel_AreYouSure.SetActive(value: true);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _Close()
	{
		anima.SetTrigger("Hide");
		IsOpen = false;
		SEMgr.Inst.uiClick.PlaySE();
		if ((bool)UIMainMenuMgr.Inst)
		{
			UIMainMenuMgr.Inst.ShowParticle();
		}
	}

	public void _DeleteYes()
	{
		panel_AreYouSure.SetActive(value: false);
		DataMgr.DeleteWorldData(deleteDataIndex);
		uiArchiveSlots[deleteDataIndex].UpdateInfoLanguangeAndLayout();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _DeleteNo()
	{
		panel_AreYouSure.SetActive(value: false);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void PanelSkipCLose()
	{
		UIMainMenuMgr.Inst.uiArchive.Panel_Skip.SetActive(value: false);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void NewGameSkip()
	{
		uiArchiveSlots[selectedDataIndex].NewGameSkip();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void NewGameDontSkip()
	{
		uiArchiveSlots[selectedDataIndex].NewGameDontSkip();
		SEMgr.Inst.uiClick.PlaySE();
	}
}
