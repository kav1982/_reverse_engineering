using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameServer;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UISetting : GameUI
{
	[Serializable]
	public class SettingToggle
	{
		public Toggle toggle;

		public Text text;

		public CanvasGroup canvasGroup;

		public Image additionalImageTransMobile;
	}

	public enum ToggleEnum
	{
		General,
		Control,
		Other,
		MobileController,
		Account
	}

	public enum btnEnum
	{
		w,
		s,
		a,
		d,
		shoot,
		e,
		bag,
		switchwandup,
		switchwanddown,
		wand1,
		wand2,
		wand3,
		wand4,
		wand5,
		wand6,
		usepotion,
		switchpotionup,
		switchpotiondown,
		quickremove,
		Sprint,
		QcantChange,
		EcantChange,
		AcantChange,
		DcantChange,
		QuickPanel,
		KillSummon
	}

	public enum controlEnum
	{
		wasd,
		aim,
		e,
		shoot,
		bag,
		switchwandup,
		switchwanddown,
		menue,
		usepotion,
		switchpotion,
		throwaway,
		moveobj,
		back,
		LB,
		RB,
		LT,
		RT,
		Sprint,
		QuickPanel,
		KillSummon
	}

	public Texture2D emptycursor;

	public Texture2D wandcursor;

	public Texture2D OrigionCursor;

	public Sprite wandcursor_Sprite;

	public Sprite OrigionCursor_Sprite;

	public Sprite pointinBackgroundDisabled;

	public Sprite pointinBackgroundHighlight;

	public Color disabledcolor;

	public Color enableColor;

	public Color pointincolor_background;

	public Color pointoutcolor_background;

	public Color buttonEnable;

	public Color Text_pointin;

	public Color Text_pointout;

	public Color colorSelected_text;

	public Color colorUnselected_text;

	public Color colorSliderBackgroundActive;

	public Color colorSliderBackgroundDisabled;

	public Animator anima;

	public GameObject buttonShowToggleSwitchController;

	public GameObject buttonShowToggleSwitchPC;

	public UpdatButtonShow[] updatButtonShows;

	public CanvasGroup cg_General;

	public CanvasGroup cg_Control;

	public CanvasGroup cg_Other;

	public GameObject go_ResetBtn;

	public RectTransform lineAudio;

	public RectTransform lineVideo;

	public float lineOffset;

	public List<SettingToggle> settingToggles;

	[Header("General")]
	public Text text_Reset;

	public Text text_OK;

	public Text text_Music;

	public Text text_Sound;

	public Text text_mainVolume;

	public Slider slider_MainVolume;

	public Slider slider_Music;

	public Slider slider_Sound;

	public Text Text_ChangeLanguage;

	public Text Text_ChangeRes;

	public Text Text_FrameLimit;

	public Text Text_FullScreen;

	public Text Text_FullScreenButtonShow;

	public Dropdown DropDown_Resolution;

	public Dropdown DropDown_FrameLimit;

	public GameObject gameobjectChangeFrameLimit;

	public Dropdown DropDown_Language;

	public Text Text_video;

	public Text Text_audio;

	public Text Vsync;

	public Text VsyncShow;

	public bool pointDown;

	[Header("controller_nav")]
	private int navCounter;

	public GridLayoutGroup ControlControlleGridLayout;

	private int int_general_select;

	public Text[] text_Generals;

	public UISettingPointin[] pointinGeneral;

	private int int_control_select;

	private int int_other_select;

	[Header("Other")]
	[Space(30f)]
	public List<SettingSlot> settingSlotOthers;

	public Text text_OtherTile;

	public Text text_OtherDesc;

	public Text text_TextFloat;

	public Text DamageFloatButtonShow;

	public Text text_ScreenShock;

	public Text TourMode;

	public Text TourModeButtonShow;

	public Text SafeMode;

	public Text SafeMode_Show;

	public GameObject gameobjectTourMode;

	public Text SpellTransparency;

	public Text textSpellTransparencyIsZero;

	public Transform SpellTransparencyDes_transform;

	public Slider slider_SpellTransparency;

	public Slider slider_ShakeScreen;

	[Header("端游才有")]
	public Text Text_CursorSize;

	public Text Text_HardwareCursor;

	public Text Text_HardwareCursor_Show;

	public Slider slider_SummonTransparency;

	public Text SummonTransparency;

	public Text textSummonTransparencyIsZero;

	public Text HideBattleUI;

	public Text HideBattleUIShow;

	public Text AiSummon;

	public Text AiSummonShow;

	public UISettingDesc AiSummonDes;

	public GameObject gameobject_CursorSize_Slider;

	public GameObject gameobject_HardwareCursor;

	public Slider Slider_MouseCursorSize;

	public Image imageSliderMouseCursorSizeBackground;

	[Header("手游")]
	public Text virtualStickType;

	public Text virtualStickType_Show;

	public Text virtualStickScale;

	public Text virtualStickPosition;

	public Text virtualStickRecover;

	public Text virtualStickRecoverShow;

	public Text virtualStickWeakAutoAimShow;

	public Text virtualStickHalfAutoAimRangeShow;

	public Text individualInteractButtonShow;

	public Text mobileMoveLerpShow;

	public Slider slider_VirtualStickScale;

	public Slider sliderVirtualStickPosition;

	public Slider sliderRightStickSensitive;

	public GameObject gameobjectVirtualStickRecover;

	public GameObject goMobileTestButton;

	public Button btn_OK;

	public GameObject goMobileControllerFrame;

	[Header("ControlChange")]
	public GameObject Text_Using_pad;

	public GameObject Text_Using_key;

	public Text text_usingpad;

	public Text text_usingkey;

	public Text text_longpresspad;

	public Text text_pressDown;

	public Text text_waitpress;

	public CanvasGroup Canvas_changekey;

	public CanvasGroup Canvas_changegamepad;

	public Toggle Toggle_changekey;

	public Toggle Toggle_changegamepad;

	public GameObject Canvas_waitpress;

	public Scrollbar KeyScrollBar;

	public Text Text_KeyAndMouse;

	public Text Text_Controller;

	public int width_offset;

	public int height_offset;

	public int sqare_offset;

	[Header("键鼠按键说明")]
	public Text CC_MoveUp_text;

	public Text CC_MoveDown_text;

	public Text CC_MoveLeft_text;

	public Text CC_MoveRight_text;

	public Text CC_interact_text;

	public Text CC_bag_text;

	public Text CC_shoot_text;

	public Text CC_EachWand_text;

	public Text CC_switchwand_text;

	public Text CC_SwitchPotion_text;

	public Text CC_UsePotion_text;

	public Text CC_SwitchWand_text;

	public Text CC_Sprint_text;

	public Text CC_menu_text;

	public Text CC_QuickPanel_text;

	public Text CC_KillSummon_text;

	[Header("手柄按键说明")]
	public Text CC_Move_text_Pad;

	public Text CC_Aim_MainButtonAim_Pad;

	public Text CC_interact_text_Pad;

	public Text CC_bag_text_Pad;

	public Text CC_shoot_text_Pad;

	public Text CC_switchwandUp_text_Pad;

	public Text CC_switchwandDown_text_Pad;

	public Text CC_UsePotion_text_pad;

	public Text CC_Sprint_text_pad;

	public Text CC_MoveObj_text_pad;

	public Text CC_throw_text_pad;

	public Text CC_back_pad;

	public Text CC_menu_text_Pad;

	public Text CC_QuickPanel_text_Pad;

	public Text CC_KillSummon_text_Pad;

	[Header("按键绑定显示")]
	public Text CC_Longpress;

	public Text CC_MoveUp_MainButtonShow;

	public Text CC_MoveDown_MainButtonShow;

	public Text CC_MoveLeft_MainButtonShow;

	public Text CC_MoveRight_MainButtonShow;

	public Text CC_interact_MainButtonShow;

	public Text CC_bag_MainButtonShow;

	public Text CC_shoot_MainButtonShow;

	public Text CC_UsePotion_MainButtonShow;

	public Text CC_switchwand_MainButtonShow;

	public Text CC_switchpotionUp_MainButtonShow;

	public Text CC_switchpotionDown_MainButtonShow;

	public Text CC_Sprint_ButtonSHow;

	public Text CC_switchwandUP_MainButtonShow;

	public Text CC_switchwandDown_MainButtonShow;

	public Text CC_menu_MainButtonShow;

	public Text CC_switchwand1_MainButtonShow;

	public Text CC_switchwand2_MainButtonShow;

	public Text CC_switchwand3_MainButtonShow;

	public Text CC_switchwand4_MainButtonShow;

	public Text CC_switchwand5_MainButtonShow;

	public Text CC_switchwand6_MainButtonShow;

	public Text CC_QuickPanel_MainButtonShow;

	public Text CC_KillSummon_MainButtonShow;

	[Header("键盘按键显示")]
	public const float defaultKeyHeight = 57.3f;

	private const float minWidth = 51f;

	private const float widthOffset = 30f;

	[Header("手柄绑定显示")]
	public Text CC_Move_MainButtonShow_Pad;

	public Text CC_Aim_MainButtonShow_Pad;

	public Text CC_interact_MainButtonShow_Pad;

	public Text CC_shoot_MainButtonShow_Pad;

	public Text CC_bag_MainButtonShow_Pad;

	public Text CC_switchwandUP_MainButtonShow_Pad;

	public Text CC_switchwandDown_MainButtonShow_Pad;

	public Text CC_UsePotion_MainButtonShow_Pad;

	public Text CC_Drop_MainButtonShow_Pad;

	public Text CC_MoveObj_MainButtonShow_Pad;

	public Text CC_Back_MainButtonShow_Pad;

	public Text CC_Menue_MainButtonShow_Pad;

	public Text CC_QuickPanel_MainButtonShow_Pad;

	public Sprite mouse_left;

	public Sprite mouse_right;

	public Sprite mouse_middleup;

	public Sprite mouse_middledown;

	public Sprite key_Space;

	public Sprite key_UpArrow;

	public Sprite key_DownArrow;

	public Sprite key_LeftArrow;

	public Sprite key_RightArrow;

	public Sprite Controller_LeftStick;

	public Sprite Controller_RightStick;

	public Sprite Controller_LT;

	public Sprite Controller_RT;

	public Sprite Controller_LB;

	public Sprite Controller_RB;

	public Sprite Controller_Start;

	public Sprite Controller_Select;

	public Sprite Controller_Dpad;

	public Sprite controlSprite_Default;

	public Sprite Controller_East;

	public Sprite Controller_North;

	public Sprite Controller_South;

	public Sprite Controller_West;

	public Sprite Controller_East_PS;

	public Sprite Controller_North_PS;

	public Sprite Controller_South_PS;

	public Sprite Controller_West_PS;

	public Sprite Controller_L1;

	public Sprite Controller_L2;

	public Sprite Controller_R1;

	public Sprite Controller_R2;

	public List<KeyUISetting> keycontrol = new List<KeyUISetting>();

	public List<KeyUISetting> controllercontrol = new List<KeyUISetting>();

	public List<KeyUISetting> disabled = new List<KeyUISetting>();

	public static InputActionRebindingExtensions.RebindingOperation PerformInteractiveRebinding;

	[Header("动态隐藏的按钮")]
	public GameObject keyQuickPanel;

	public GameObject keyKillSummon;

	public GameObject ControllerQuickPanel;

	public GameObject ControllerKillSummon;

	[Header("Mobile")]
	public GameObject customMobileControl;

	public GameObject changeVirtualPanel;

	public UIMobileReturnAndRess uiMobileReturnAndRessCustomControl;

	public Slider slider_changeVirtualSize;

	public Slider slider_changeVirtualTransparency;

	[Header("StickToggle")]
	public Toggle toggle0;

	public Toggle toggle1;

	public Toggle toggle2;

	[Header("ResQualityToggle")]
	public List<Toggle> resQualityToggleToggles;

	public List<Text> resQualityToggleToggleTextss;

	[Header("MobileTargetFrameToggle")]
	public List<Toggle> mobileTargetFrameToggleToggles;

	public List<Text> mobileTargetFrameToggleTextss;

	[Header("Account")]
	public Button changeAccountBtn;

	public Button logoutBtn;

	public Button CDKeyBtn;

	public Button serviceBtn;

	public Button userServiceBtn;

	public Button privacyBtn;

	public Button codeBtn;

	public GameObject panelCDKey;

	public InputField inputCDKey;

	public Button CDKeyConfirmBtn;

	public Button closeCDKeyPanelBtn;

	public GameObject serviceUnreadDot;

	[Header("Test")]
	public UISettingMobileToggle uiSettingMobileToggleTest;

	public GameObject gameobjectMobileControlTouch;

	public GameObject gameobjectMobileControlGamepad;

	private EntityManager ettMgr;

	private SettingToggle toggleGeneral => settingToggles[0];

	private SettingToggle toggleControl => settingToggles[1];

	private SettingToggle toggleOther => settingToggles[2];

	private SettingToggle toggleMobileControl => settingToggles[3];

	private SettingToggle toggleMobileAccount => settingToggles[4];

	private void InitControl()
	{
		base.inputActions.Enable();
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.WASD.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadLB.performed += GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.GamepadWest.performed += GamepadDirectMoveObjPerformed;
		if (GameMgr.IsMobile_Static)
		{
			logoutBtn.onClick.AddListener(delegate
			{
				SEMgr.Inst.uiClick.PlaySE();
			});
			changeAccountBtn.onClick.AddListener(delegate
			{
				SEMgr.Inst.uiClick.PlaySE();
			});
			CDKeyBtn.onClick.AddListener(delegate
			{
				SEMgr.Inst.uiClick.PlaySE();
			});
			serviceBtn.onClick.AddListener(delegate
			{
				SEMgr.Inst.uiClick.PlaySE();
			});
			logoutBtn.onClick.AddListener(delegate
			{
				PluginActivity.Inst.CloseAccount();
			});
			changeAccountBtn.onClick.AddListener(delegate
			{
				GameMgr.Inst.RecycleAllPool();
				PluginActivity.Inst.OneSDKLogout(PluginActivity.LogoutReason.Manual);
			});
			closeCDKeyPanelBtn.onClick.AddListener(_Close);
			codeBtn.onClick.AddListener(delegate
			{
				Application.OpenURL("https://beian.miit.gov.cn");
			});
			CDKeyBtn.onClick.AddListener(delegate
			{
				panelCDKey.SetActive(value: true);
			});
			serviceBtn.onClick.AddListener(delegate
			{
				PluginActivity.Inst.ShowCustomerService(fullScreen: true, showToolBar: true);
			});
			CDKeyConfirmBtn.onClick.AddListener(OnCDKeyBtnClick);
			privacyBtn.onClick.AddListener(delegate
			{
				PluginActivity.Inst.ShowPrivacyProtocol();
			});
			userServiceBtn.onClick.AddListener(delegate
			{
				PluginActivity.Inst.ShowUserProtocol();
			});
		}
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange_UIsetting));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void OnDisable()
	{
		base.inputActions.Disable();
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.WASD.performed -= GamepadDirectPerformed;
		base.inputActions.Player.GamepadLB.performed -= GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed -= GamepadRBPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadWest.performed -= GamepadDirectMoveObjPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange_UIsetting));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void GamepadDirectMoveObjPerformed(InputAction.CallbackContext obj)
	{
		if (base.IsOpen && !ControlMgr.Inst.rebinding && !ControlMgr.Inst.InputActionRecovering)
		{
			RestAllControl();
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (ControlMgr.Inst.rebinding || !base.IsOpen)
		{
			return;
		}
		Vector2 vector = context.ReadValue<Vector2>();
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			if (vector == new Vector2(-1f, 0f))
			{
				GamepadLBPerformed(default(InputAction.CallbackContext));
			}
			else if (vector == new Vector2(1f, 0f))
			{
				GamepadRBPerformed(default(InputAction.CallbackContext));
			}
		}
		else
		{
			navCounter = 0;
			movedirection_nav(vector);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.rebinding && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			navCounter = 0;
			if (!(vector == Vector2.zero))
			{
				movedirection_nav(vector);
			}
		}
	}

	private void movedirection_nav(Vector2 _direct, bool PlayAudio = true)
	{
		navCounter++;
		if (cg_Control.alpha == 1f && (GameMgr.IsMobile_Static || Canvas_changegamepad.alpha == 1f))
		{
			if (CustomNavTool.Nav(_direct, ref int_control_select, ControlControlleGridLayout, () => controllercontrol[int_control_select].button.interactable) && navCounter < 4)
			{
				if (_direct == Vector2.left || _direct == Vector2.right)
				{
					movedirection_nav(Vector2.down, PlayAudio: false);
				}
				else
				{
					movedirection_nav(_direct, PlayAudio: false);
				}
				return;
			}
			foreach (KeyUISetting item in controllercontrol)
			{
				item.showtext.color = colorUnselected_text;
			}
			controllercontrol[int_control_select].showtext.color = colorSelected_text;
			if (PlayAudio)
			{
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (_direct == new Vector2(0f, 1f))
		{
			if (cg_General.alpha == 1f)
			{
				pointinGeneral[int_general_select].OnPointerExit(null);
				if (int_general_select == 0)
				{
					int_general_select = text_Generals.Length - 1;
				}
				else
				{
					int_general_select--;
				}
				if (!pointinGeneral[int_general_select].gameObject.activeInHierarchy)
				{
					movedirection_nav(Vector2.up, PlayAudio: false);
					return;
				}
				pointinGeneral[int_general_select].OnPointerEnter(null);
				if (PlayAudio)
				{
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
				if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					goMobileControllerFrame.transform.position = pointinGeneral[int_general_select].transform.position;
				}
			}
			else
			{
				if (cg_Other.alpha != 1f)
				{
					return;
				}
				settingSlotOthers[int_other_select].UISettingPointin.OnPointerExit(null);
				if (int_other_select == 0)
				{
					int_other_select = settingSlotOthers.Count - 1;
				}
				else
				{
					int_other_select--;
				}
				if (!settingSlotOthers[int_other_select].objRoot.activeSelf || !settingSlotOthers[int_other_select].UISettingPointin.enabled || settingSlotOthers[int_other_select].category != Category.Other)
				{
					movedirection_nav(Vector2.up, PlayAudio: false);
					return;
				}
				UpdatePointInTextMobile();
				settingSlotOthers[int_other_select].UISettingPointin.OnPointerEnter(null);
				if (PlayAudio)
				{
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
				if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					goMobileControllerFrame.transform.position = settingSlotOthers[int_other_select].objRoot.transform.position;
				}
			}
		}
		else if (_direct == new Vector2(0f, -1f))
		{
			if (cg_General.alpha == 1f)
			{
				pointinGeneral[int_general_select].OnPointerExit(null);
				if (int_general_select == text_Generals.Length - 1)
				{
					int_general_select = 0;
				}
				else
				{
					int_general_select++;
				}
				if (!pointinGeneral[int_general_select].gameObject.activeInHierarchy)
				{
					movedirection_nav(Vector2.down, PlayAudio: false);
					return;
				}
				if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					goMobileControllerFrame.transform.position = pointinGeneral[int_general_select].transform.position;
				}
				pointinGeneral[int_general_select].OnPointerEnter(null);
				if (PlayAudio)
				{
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
			}
			else
			{
				if (cg_Other.alpha != 1f)
				{
					return;
				}
				settingSlotOthers[int_other_select].UISettingPointin.OnPointerExit(null);
				if (int_other_select == settingSlotOthers.Count - 1)
				{
					int_other_select = 0;
				}
				else
				{
					int_other_select++;
				}
				if (!settingSlotOthers[int_other_select].objRoot.activeSelf || !settingSlotOthers[int_other_select].UISettingPointin.enabled || settingSlotOthers[int_other_select].category != Category.Other)
				{
					movedirection_nav(Vector2.down, PlayAudio: false);
					return;
				}
				Debug.LogWarning(int_other_select + ":" + settingSlotOthers[int_other_select].objRoot.name);
				UpdatePointInTextMobile();
				settingSlotOthers[int_other_select].UISettingPointin.OnPointerEnter(null);
				if (PlayAudio)
				{
					SEMgr.Inst.uiButtonHover_Button.PlaySE();
				}
				if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					goMobileControllerFrame.transform.position = settingSlotOthers[int_other_select].objRoot.transform.position;
				}
			}
		}
		else if (_direct == new Vector2(-1f, 0f))
		{
			if (cg_General.alpha == 1f)
			{
				switch (pointinGeneral[int_general_select].function)
				{
				case UISettingPointin.generalFunctions.ChangeLanguage:
					_LanguageChangeLeftRight(0);
					break;
				case UISettingPointin.generalFunctions.ChangeRes:
					if (GameMgr.IsMobile_Static)
					{
						PreResMobile();
					}
					else if (DropDown_Resolution.value > 0)
					{
						DropDown_Resolution.value--;
					}
					else
					{
						DropDown_Resolution.value = DropDown_Resolution.options.Count - 1;
					}
					break;
				case UISettingPointin.generalFunctions.FrameRateLimit:
					if (GameMgr.IsMobile_Static)
					{
						NextFrameLimitMobile();
					}
					else if (DropDown_FrameLimit.value > 0)
					{
						DropDown_FrameLimit.value--;
					}
					else
					{
						DropDown_FrameLimit.value = DropDown_FrameLimit.options.Count - 1;
					}
					break;
				case UISettingPointin.generalFunctions.FullScreen:
					_FullScreenSwitch(switchRight: false);
					break;
				case UISettingPointin.generalFunctions.ASync:
					_VolueChangeVSync();
					break;
				case UISettingPointin.generalFunctions.MainVolume:
					if (slider_MainVolume.value > 0f)
					{
						slider_MainVolume.value = Mathf.Max(0f, slider_MainVolume.value - 0.1f);
					}
					break;
				case UISettingPointin.generalFunctions.Music:
					if (slider_Music.value > 0f)
					{
						slider_Music.value = Mathf.Max(0f, slider_Music.value - 0.1f);
					}
					break;
				case UISettingPointin.generalFunctions.Sound:
					if (slider_Sound.value > 0f)
					{
						slider_Sound.value = Mathf.Max(0f, slider_Sound.value - 0.1f);
					}
					break;
				}
			}
			else if (cg_Other.alpha == 1f)
			{
				if (settingSlotOthers[int_other_select].UISettingPointin.SliderPointin != null)
				{
					settingSlotOthers[int_other_select].UISettingPointin.slider.value -= 0.1f;
				}
				else if (settingSlotOthers[int_other_select].UISettingMobileToggle.Button.IsInteractable())
				{
					settingSlotOthers[int_other_select].UISettingPointin.ToggleUnityEvent?.Invoke();
				}
			}
		}
		else
		{
			if (!(_direct == new Vector2(1f, 0f)))
			{
				return;
			}
			if (cg_General.alpha == 1f)
			{
				switch (pointinGeneral[int_general_select].function)
				{
				case UISettingPointin.generalFunctions.ChangeLanguage:
					_LanguageChangeLeftRight(1);
					break;
				case UISettingPointin.generalFunctions.ChangeRes:
					if (GameMgr.IsMobile_Static)
					{
						NextResMobile();
					}
					else if (DropDown_Resolution.value < DropDown_Resolution.options.Count - 1)
					{
						DropDown_Resolution.value++;
					}
					else
					{
						DropDown_Resolution.value = 0;
					}
					break;
				case UISettingPointin.generalFunctions.FrameRateLimit:
					if (GameMgr.IsMobile_Static)
					{
						NextFrameLimitMobile();
					}
					else if (DropDown_FrameLimit.value < DropDown_FrameLimit.options.Count - 1)
					{
						DropDown_FrameLimit.value++;
					}
					else
					{
						DropDown_FrameLimit.value = 0;
					}
					break;
				case UISettingPointin.generalFunctions.FullScreen:
					_FullScreenSwitch();
					break;
				case UISettingPointin.generalFunctions.ASync:
					_VolueChangeVSync();
					break;
				case UISettingPointin.generalFunctions.MainVolume:
					if (slider_MainVolume.value < 1f)
					{
						slider_MainVolume.value = Mathf.Min(1f, slider_MainVolume.value + 0.1f);
					}
					break;
				case UISettingPointin.generalFunctions.Music:
					if (slider_Music.value < 1f)
					{
						slider_Music.value = Mathf.Min(1f, slider_Music.value + 0.1f);
					}
					break;
				case UISettingPointin.generalFunctions.Sound:
					if (slider_Sound.value < 1f)
					{
						slider_Sound.value = Mathf.Min(1f, slider_Sound.value + 0.1f);
					}
					break;
				}
			}
			else if (cg_Other.alpha == 1f)
			{
				if (settingSlotOthers[int_other_select].UISettingPointin.SliderPointin != null)
				{
					settingSlotOthers[int_other_select].UISettingPointin.slider.value += 0.1f;
				}
				else if (settingSlotOthers[int_other_select].UISettingMobileToggle.Button.IsInteractable())
				{
					settingSlotOthers[int_other_select].UISettingPointin.ToggleUnityEvent?.Invoke();
				}
			}
		}
	}

	private void UpdatePointInTextMobile()
	{
		if (cg_Other.alpha == 1f)
		{
			OtherTextEnter(settingSlotOthers[int_other_select].objRoot.GetComponent<UISettingDesc>());
		}
	}

	public void ChangeBattleUIShow()
	{
		if (DataMgr.settingData.BattleUIControl)
		{
			DataMgr.settingData.BattleUIControl = false;
			HideBattleUIShow.text = 1000136.GetText();
		}
		else
		{
			DataMgr.settingData.BattleUIControl = true;
			HideBattleUIShow.text = 1000135.GetText();
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	private void GamepadLBPerformed(InputAction.CallbackContext context)
	{
		if (ControlMgr.Inst.InputActionRecovering || ControlMgr.Inst.rebinding || !base.IsOpen)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			if (toggleGeneral.toggle.isOn)
			{
				toggleMobileAccount.toggle.isOn = true;
			}
			else if (toggleMobileAccount.toggle.isOn)
			{
				toggleMobileControl.toggle.isOn = true;
			}
			else if (toggleMobileControl.toggle.isOn)
			{
				toggleOther.toggle.isOn = true;
				goMobileControllerFrame.transform.position = settingSlotOthers[int_other_select].objRoot.transform.position;
			}
			else
			{
				toggleGeneral.toggle.isOn = true;
				goMobileControllerFrame.transform.position = pointinGeneral[int_general_select].transform.position;
			}
			goMobileControllerFrame.gameObject.SetActive(toggleGeneral.toggle.isOn || toggleOther.toggle.isOn);
		}
		else if (toggleGeneral.toggle.isOn)
		{
			toggleOther.toggle.isOn = true;
		}
		else if (toggleControl.toggle.isOn)
		{
			toggleGeneral.toggle.isOn = true;
		}
		else
		{
			toggleControl.toggle.isOn = true;
		}
	}

	private void GamepadRBPerformed(InputAction.CallbackContext context)
	{
		if (ControlMgr.Inst.InputActionRecovering || ControlMgr.Inst.rebinding || !base.IsOpen)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			if (toggleGeneral.toggle.isOn)
			{
				toggleOther.toggle.isOn = true;
				goMobileControllerFrame.transform.position = settingSlotOthers[int_other_select].objRoot.transform.position;
			}
			else if (toggleOther.toggle.isOn)
			{
				toggleMobileControl.toggle.isOn = true;
			}
			else if (toggleMobileControl.toggle.isOn)
			{
				toggleMobileAccount.toggle.isOn = true;
			}
			else
			{
				toggleGeneral.toggle.isOn = true;
				goMobileControllerFrame.transform.position = pointinGeneral[int_general_select].transform.position;
			}
			goMobileControllerFrame.gameObject.SetActive(toggleGeneral.toggle.isOn || toggleOther.toggle.isOn);
		}
		else if (toggleGeneral.toggle.isOn)
		{
			toggleControl.toggle.isOn = true;
		}
		else if (toggleControl.toggle.isOn)
		{
			toggleOther.toggle.isOn = true;
		}
		else
		{
			toggleGeneral.toggle.isOn = true;
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.InputActionRecovering && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen && cg_Control.alpha == 1f && ((GameMgr.IsMobile_Static && (bool)gameobjectMobileControlGamepad) || Canvas_changegamepad.alpha == 1f) && controllercontrol[int_control_select].button.interactable)
		{
			controllercontrol[int_control_select].button.onClick.Invoke();
		}
	}

	private void LanguageChange_UIsetting()
	{
		Text_video.text = 1000141.GetText();
		Text_audio.text = 1000143.GetText();
		toggleGeneral.text.text = 1000139.GetText();
		toggleControl.text.text = 1000102.GetText();
		toggleOther.text.text = 1000138.GetText();
		Vsync.text = 1000109.GetText();
		Text_ChangeLanguage.text = 1000103.GetText();
		Text_ChangeRes.text = 1000142.GetText();
		Text_FrameLimit.text = 1000176.GetText();
		text_Reset.text = 1000104.GetText();
		text_OK.text = 1000105.GetText();
		text_Music.text = 1000106.GetText();
		text_Sound.text = 1000107.GetText();
		Text_FullScreen.text = 1000108.GetText();
		text_mainVolume.text = 1000144.GetText();
		TourMode.text = 1000161.GetText();
		text_TextFloat.text = 1000111.GetText();
		text_ScreenShock.text = 1000113.GetText();
		Text_CursorSize.text = 1000236.GetText();
		Text_HardwareCursor.text = 1000237.GetText();
		if (GameMgr.IsMobile_Static)
		{
			virtualStickType.text = 1003401.GetText();
			virtualStickScale.text = 1003405.GetText();
			virtualStickPosition.text = 1003406.GetText();
		}
		HideBattleUI.text = 1000149.GetText();
		SpellTransparency.text = 1000153.GetText();
		textSpellTransparencyIsZero.text = 1000163.GetText();
		AiSummon.text = 1000159.GetText();
		SummonTransparency.text = 1000158.GetText();
		lineAudio.sizeDelta = new Vector2(lineOffset - Text_audio.preferredWidth, lineAudio.sizeDelta.y);
		lineVideo.sizeDelta = new Vector2(lineOffset - Text_video.preferredWidth, lineVideo.sizeDelta.y);
		SafeMode.text = 1000156.GetText();
		UpdateWindowsModeLanguage();
		TourModeButtonShow.text = (DataMgr.settingData.isTouristMode ? 1000135.GetText() : 1000136.GetText());
		DamageFloatButtonShow.text = (DataMgr.settingData.textFloat ? 1000135.GetText() : 1000136.GetText());
		Text_HardwareCursor_Show.text = (DataMgr.settingData.hardwareCursor ? 1000135.GetText() : 1000136.GetText());
		HideBattleUIShow.text = (DataMgr.settingData.BattleUIControl ? 1000135.GetText() : 1000136.GetText());
		SafeMode_Show.text = (DataMgr.settingData.SafeMode ? 1000135.GetText() : 1000136.GetText());
		VsyncShow.text = (DataMgr.settingData.Vsync ? 1000135.GetText() : 1000136.GetText());
		AiSummonShow.text = (DataMgr.settingData.AiSummon ? 1000135.GetText() : 1000136.GetText());
		if (GameMgr.IsMobile_Static)
		{
			virtualStickWeakAutoAimShow.text = ((DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim) ? 1000135.GetText() : 1000136.GetText());
			virtualStickHalfAutoAimRangeShow.text = (DataMgr.settingData.Mobiledata.halfAutoAimRange ? 1000135.GetText() : 1000136.GetText());
		}
		Language_ChangeControlLanguage();
		DropDown_FrameLimit_SetOnStart();
	}

	private void UpdateWindowsModeLanguage()
	{
		switch (DataMgr.settingData.windowsMode)
		{
		case SettingData.WindowsMode.FullScreen:
			Text_FullScreenButtonShow.text = 1000172.GetText();
			break;
		case SettingData.WindowsMode.Windows:
			Text_FullScreenButtonShow.text = 1000173.GetText();
			break;
		case SettingData.WindowsMode.BoardlessWindows:
			Text_FullScreenButtonShow.text = 1000174.GetText();
			break;
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (GameMgr.IsMobile_Static)
			{
				goMobileControllerFrame.gameObject.SetActive(value: false);
				gameobjectMobileControlTouch.gameObject.SetActive(value: true);
				gameobjectMobileControlGamepad.gameObject.SetActive(value: false);
			}
			else
			{
				Text_Using_key.SetActive(value: true);
				Text_Using_pad.SetActive(value: false);
				Canvas_changegamepad.alpha = 0f;
				Canvas_changegamepad.blocksRaycasts = false;
				Canvas_changekey.alpha = 1f;
				Canvas_changekey.blocksRaycasts = true;
			}
			if (GameMgr.IsSteamDeck_Static)
			{
				break;
			}
			ControlMgr.Inst.usingpad = false;
			buttonShowToggleSwitchController.SetActive(value: false);
			buttonShowToggleSwitchPC.SetActive(value: true);
			foreach (SettingSlot settingSlotOther in settingSlotOthers)
			{
				settingSlotOther.objRoot.SetActive(GameMgr.IsMobile_Static ? settingSlotOther.activeMobile : settingSlotOther.activePCKey);
			}
			btn_OK.gameObject.SetActive(value: true);
			pointinGeneral[int_general_select].OnPointerExit(null);
			settingSlotOthers[int_other_select].UISettingPointin.OnPointerExit(null);
			if (GameMgr.IsMobile_Static)
			{
				goMobileControllerFrame.gameObject.SetActive(value: false);
			}
			break;
		case PlayerInputType.Gamepad:
		{
			ControlMgr.Inst.usingpad = true;
			foreach (SettingSlot settingSlotOther2 in settingSlotOthers)
			{
				settingSlotOther2.objRoot.SetActive(GameMgr.IsMobile_Static ? settingSlotOther2.activeMobile : settingSlotOther2.activePCController);
			}
			buttonShowToggleSwitchController.SetActive(value: true);
			buttonShowToggleSwitchPC.SetActive(value: false);
			UpdatButtonShow[] array = updatButtonShows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateButton();
			}
			btn_OK.gameObject.SetActive(value: false);
			int_control_select = 0;
			int_other_select = 0;
			if (GameMgr.IsMobile_Static)
			{
				goMobileControllerFrame.gameObject.SetActive(value: true);
				gameobjectMobileControlGamepad.gameObject.SetActive(value: true);
				gameobjectMobileControlTouch.gameObject.SetActive(value: false);
				break;
			}
			Text_Using_pad.SetActive(value: true);
			Text_Using_key.SetActive(value: false);
			Canvas_changekey.blocksRaycasts = false;
			Canvas_changegamepad.alpha = 1f;
			Canvas_changegamepad.blocksRaycasts = true;
			Canvas_changekey.alpha = 0f;
			break;
		}
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		ControlChange();
	}

	private void ControlChange()
	{
		UpdatButtonShow[] array = updatButtonShows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateButton();
		}
	}

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (GameMgr.IsMobile_Static)
		{
			toggleControl.toggle.gameObject.SetActive(value: false);
		}
		InitControl();
		toggleGeneral.toggle.isOn = true;
		toggleControl.toggle.isOn = false;
		toggleOther.toggle.isOn = false;
		float mainVolume = DataMgr.settingData.mainvolume;
		slider_MainVolume.value = 0f;
		slider_Music.value = DataMgr.settingData.music;
		slider_Sound.value = DataMgr.settingData.sound;
		Slider_MouseCursorSize.value = DataMgr.settingData.CursorSize;
		GameMgr.Inst.SetWindowsMode();
		InitUISetting();
		SetUISettings();
		_LanguageChange();
		UpdateCursor();
		InputChange();
		_VolueChangeCursorSize();
		SetAllMobileToggle();
		UIMgr.Inst.uiSetting.UpdateControlShow();
		SetAllKeyBackground();
		if (GameMgr.IsMobile_Static)
		{
			_VirtualStickType(DataMgr.settingData.VirtualStickType);
			UIMgr.Inst.uiSetting.goMobileTestButton.SetActive(!ScriptableObjMgr.Inst.testCtrller.UseServer && ScriptableObjMgr.Inst.testCtrller.Shortcut);
		}
		DOTween.Sequence().AppendInterval(0.5f).OnComplete(delegate
		{
			slider_MainVolume.value = mainVolume;
		});
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (base.IsOpen)
			{
				bool hasUnReadMessage = PluginActivity.Inst.HasUnReadMessage;
				serviceUnreadDot?.gameObject.SetActive(hasUnReadMessage);
			}
			if (customMobileControl.activeInHierarchy)
			{
				changeVirtualPanel.gameObject.SetActive(TopUI.inst.currentVirtualStickSizeAdjust != null);
			}
		}
	}

	public new void Show()
	{
		bool active = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.DamageRecordBoard) > 0;
		bool active2 = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.CancelSummon) != 0;
		ShowSettingGeneral();
		SetAllKeyBackground();
		if (!GameMgr.IsMobile_Static)
		{
			keyQuickPanel.SetActive(active);
			keyKillSummon.SetActive(active2);
		}
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		ControllerKillSummon.SetActive(active2);
		ControllerQuickPanel.SetActive(active);
	}

	public void ShowFromMainMenue()
	{
		ShowSettingGeneral();
		anima.SetTrigger("AppearMain");
		UIMgr.TryAdditionalMobileShow(base.transform);
		if (!GameMgr.IsMobile_Static)
		{
			keyQuickPanel.SetActive(value: false);
			ControllerQuickPanel.SetActive(value: false);
			keyKillSummon.SetActive(value: false);
			ControllerKillSummon.SetActive(value: false);
		}
	}

	private void ShowSettingGeneral()
	{
		if (GameMgr.IsMobile_Static && GameMgr.IsUseBiliOneSDK)
		{
			CDKeyBtn.gameObject.SetActive(!CNHCHFKLMOH.CheckCurrentServerTag(CNHCHFKLMOH.PLACCDNLMJH));
			changeAccountBtn.interactable = PluginActivity.ServerLogged;
			logoutBtn.interactable = PluginActivity.ServerLogged;
			CDKeyBtn.interactable = PluginActivity.ServerLogged;
			serviceBtn.interactable = PluginActivity.ServerLogged;
			bool active = PluginActivity.channleID == PluginActivity.ChannleID.B服.ChannleID() || PluginActivity.channleID == PluginActivity.ChannleID.AppleStore.ChannleID();
			serviceBtn.gameObject.SetActive(active);
			logoutBtn.gameObject.SetActive(active);
		}
		int_other_select = 0;
		int_general_select = 0;
		settingSlotOthers.ForEach(delegate(SettingSlot x)
		{
			if (x.objRoot.activeSelf && x.UISettingPointin.enabled)
			{
				x.UISettingPointin.OnPointerExit(null);
			}
		});
		UISettingPointin[] array = pointinGeneral;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnPointerExit(null);
		}
		SetUISettings();
		_ToggleChange(isOn: true);
		_ToggleChange_KeyOrPad();
		SetIsOpen(isOpen: true);
		if (GameMgr.IsMobile_Static && GameMgr.IsUseBiliOneSDK)
		{
			PluginActivity.Inst.GetUnReadMessageToken();
		}
	}

	public override void Hide()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
			{
				UIMgr.Inst.uiSetting.AdjustTopUIEndCustomControl();
				return;
			}
			if ((bool)UIMgr.Inst.UIMenu)
			{
				UIMenu.MenuToggleCanvas menuCanvas = UIMgr.Inst.UIMenu.GetMenuCanvas(UIMenu.MenuCategory.System);
				if (menuCanvas.toggle.isOn)
				{
					menuCanvas.toggle.isOn = false;
				}
			}
			base.Hide();
		}
		else
		{
			base.Hide();
		}
	}

	protected override void OnHide()
	{
		if (anima.GetCurrentAnimatorClipInfo(0)[0].clip.name == "UISetting_Appear")
		{
			anima.SetTrigger("Disappear");
		}
		else if (anima.GetCurrentAnimatorClipInfo(0)[0].clip.name == "UISetting_Appear_Mainmenue")
		{
			anima.SetTrigger("DisappearMain");
		}
		UIMgr.TryAdditionalMobileHide(base.transform);
		DataMgr.SaveSettingData();
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	public void OtherTextEnter(UISettingDesc desc)
	{
		if (desc == null || desc.rtsf_Text == null)
		{
			OtherTextExit();
		}
		else if (desc.rtsf_Text == text_TextFloat.rectTransform)
		{
			text_OtherDesc.text = 1000112.GetText();
			text_OtherTile.text = 1000111.GetText();
		}
		else if (desc.rtsf_Text == Text_CursorSize.rectTransform)
		{
			text_OtherDesc.text = 1000235.GetText();
			text_OtherTile.text = 1000236.GetText();
		}
		else if (desc.rtsf_Text == Text_HardwareCursor.rectTransform)
		{
			text_OtherDesc.text = 1000234.GetText();
			text_OtherTile.text = 1000237.GetText();
		}
		else if (desc.rtsf_Text == HideBattleUI.rectTransform)
		{
			text_OtherDesc.text = 1000150.GetText();
			text_OtherTile.text = 1000149.GetText();
		}
		else if (desc.rtsf_Text == SpellTransparency.rectTransform)
		{
			text_OtherDesc.text = 1000154.GetText();
			text_OtherTile.text = 1000153.GetText();
		}
		else if (desc.rtsf_Text == SafeMode.rectTransform)
		{
			text_OtherDesc.text = 1000157.GetText();
			text_OtherTile.text = 1000156.GetText();
		}
		else if (desc.rtsf_Text == TourMode.rectTransform)
		{
			text_OtherDesc.text = 1000162.GetText();
			text_OtherTile.text = 1000161.GetText();
		}
		else if (desc.rtsf_Text == SummonTransparency.rectTransform)
		{
			text_OtherDesc.text = 1000169.GetText();
			text_OtherTile.text = 1000158.GetText();
		}
		else if (desc.rtsf_Text == AiSummon.rectTransform)
		{
			text_OtherDesc.text = 1000160.GetText();
			text_OtherTile.text = 1000159.GetText();
		}
		else if (GameMgr.IsMobile_Static)
		{
			OtherTextExit();
		}
	}

	public void OtherTextExit()
	{
		text_OtherDesc.text = "";
		text_OtherTile.text = "";
	}

	public void _ToggleChange(bool isOn)
	{
		if (!isOn)
		{
			return;
		}
		settingToggles.ForEach(delegate(SettingToggle x)
		{
			if (x != null && !(x.text == null) && !(x.canvasGroup == null))
			{
				x.text.color = colorUnselected_text;
				x.canvasGroup.alpha = 0f;
				x.canvasGroup.blocksRaycasts = false;
				if ((bool)x.additionalImageTransMobile)
				{
					x.additionalImageTransMobile.enabled = false;
				}
			}
		});
		var anon = settingToggles.Select((SettingToggle toggle, int index) => new { toggle, index }).FirstOrDefault(x => x.toggle?.toggle != null && x.toggle.toggle.isOn);
		if (anon == null)
		{
			return;
		}
		anon.toggle.text.color = Color.white;
		anon.toggle.canvasGroup.alpha = 1f;
		anon.toggle.canvasGroup.blocksRaycasts = true;
		if ((bool)anon.toggle.additionalImageTransMobile)
		{
			anon.toggle.additionalImageTransMobile.enabled = true;
		}
		if (GameMgr.IsMobile_Static)
		{
			GameObject obj = go_ResetBtn;
			int index2 = anon.index;
			obj.SetActive(index2 == 3 || index2 == 2);
		}
		else
		{
			KeyScrollBar.value = 1f;
			GameObject obj2 = go_ResetBtn;
			int index2 = anon.index;
			obj2.SetActive(index2 == 1 || index2 == 2);
			if (ControlMgr.Inst.usingpad)
			{
				pointinGeneral[int_general_select].OnPointerEnter(null);
				if (anon.index == 1)
				{
					foreach (KeyUISetting item in controllercontrol)
					{
						item.showtext.color = colorUnselected_text;
					}
				}
			}
		}
		SEMgr.Inst.uiChangeLabel.PlaySE();
		if (ControlMgr.Inst.InputType != PlayerInputType.Gamepad || GameMgr.IsMobile_Static)
		{
			return;
		}
		if (anon.index == 1)
		{
			if (int_control_select < 0 || int_control_select >= controllercontrol.Count)
			{
				return;
			}
			foreach (KeyUISetting item2 in controllercontrol)
			{
				item2.showtext.color = colorUnselected_text;
			}
			controllercontrol[int_control_select].showtext.color = colorSelected_text;
		}
		else if (anon.index == 0)
		{
			if (int_general_select >= 0 && int_general_select < pointinGeneral.Length && pointinGeneral[int_general_select].gameObject.activeInHierarchy)
			{
				pointinGeneral[int_general_select].OnPointerEnter(null);
			}
		}
		else if (anon.index == 2 && int_other_select >= 0 && int_other_select < settingSlotOthers.Count)
		{
			SettingSlot settingSlot = settingSlotOthers[int_other_select];
			if (settingSlot.objRoot.activeSelf && settingSlot.UISettingPointin.enabled)
			{
				settingSlot.UISettingPointin.OnPointerEnter(null);
			}
		}
	}

	public void _ToggleChange_KeyOrPad()
	{
	}

	public void _ValueChangeMusic()
	{
		DataMgr.settingData.music = slider_Music.value;
		EventMgr.MusicVolumeChange?.Invoke();
	}

	public void _ValueChangeSound()
	{
		DataMgr.settingData.sound = slider_Sound.value;
		EventMgr.SoundVolumeChange?.Invoke();
	}

	public void _VolueChangeMainVolume()
	{
		DataMgr.settingData.mainvolume = slider_MainVolume.value;
		EventMgr.SoundVolumeChange?.Invoke();
		EventMgr.MusicVolumeChange?.Invoke();
	}

	public void _ResChange_DropDown()
	{
		_ResChange(DropDown_Resolution.value);
	}

	public void NextResMobile()
	{
		switch (DataMgr.settingData.resTypeMobile)
		{
		case ResolutionTypeMobile.Res40:
			_MobileChangeResMid(value: true);
			break;
		case ResolutionTypeMobile.Res60:
			_MobileChangeResHigh(value: true);
			break;
		case ResolutionTypeMobile.Res80:
			break;
		}
	}

	public void PreResMobile()
	{
		switch (DataMgr.settingData.resTypeMobile)
		{
		case ResolutionTypeMobile.Res60:
			_MobileChangeResLow(value: true);
			break;
		case ResolutionTypeMobile.Res80:
			_MobileChangeResMid(value: true);
			break;
		case ResolutionTypeMobile.Res40:
			break;
		}
	}

	public void NextFrameLimitMobile()
	{
		mobileTargetFrameToggleToggles[(DataMgr.settingData.MobileTargetFrameRate == MobileTargetFrameRate.Target30) ? 1 : 0].isOn = true;
	}

	private void _ResChange(int type)
	{
		if (GameMgr.IsSteamDeck_Static)
		{
			DataMgr.settingData.resTypeSteamDeck2 = (ResolutionTypeSteamDeck)type;
		}
		else if (GameMgr.IsMobile_Static)
		{
			DataMgr.settingData.resTypeMobile = (ResolutionTypeMobile)type;
		}
		else
		{
			DataMgr.settingData.resType = (ResolutionType)type;
		}
		EventMgr.OnChangeResolution?.Invoke();
		GameMgr.Inst.SetWindowsMode();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _FrameRateChange_DropDown()
	{
		_FrameRateChange((SettingData.FrameLimit)DropDown_FrameLimit.value);
	}

	private void _FrameRateChange(SettingData.FrameLimit type)
	{
		if (DataMgr.settingData.Vsync)
		{
			_VolueChangeVSync();
		}
		DataMgr.settingData.frameLimit = type;
		switch (type)
		{
		case SettingData.FrameLimit.limit30:
			Application.targetFrameRate = 30;
			break;
		case SettingData.FrameLimit.limit60:
			Application.targetFrameRate = 60;
			break;
		case SettingData.FrameLimit.NoLimit:
			Application.targetFrameRate = 999;
			break;
		}
	}

	public void _FrameLimitChangeLeftRight(int i)
	{
		switch (i)
		{
		case 0:
			if (DropDown_FrameLimit.value > 0)
			{
				DropDown_FrameLimit.value--;
			}
			else
			{
				DropDown_FrameLimit.value = DropDown_FrameLimit.options.Count - 1;
			}
			break;
		case 1:
			if (DropDown_FrameLimit.value < DropDown_FrameLimit.options.Count - 1)
			{
				DropDown_FrameLimit.value++;
			}
			else
			{
				DropDown_FrameLimit.value = 0;
			}
			break;
		}
		_FrameRateChange_DropDown();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _ResChangeLeftRight(int i)
	{
		switch (i)
		{
		case 0:
			if (DropDown_Resolution.value > 0)
			{
				DropDown_Resolution.value--;
			}
			else
			{
				DropDown_Resolution.value = DropDown_Resolution.options.Count - 1;
			}
			break;
		case 1:
			if (DropDown_Resolution.value < DropDown_Resolution.options.Count - 1)
			{
				DropDown_Resolution.value++;
			}
			else
			{
				DropDown_Resolution.value = 0;
			}
			break;
		}
		_ResChange_DropDown();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MobileChangeResLow(bool value)
	{
		if (value)
		{
			ResChangeMobile(0);
		}
	}

	public void _MobileChangeResMid(bool value)
	{
		if (value)
		{
			ResChangeMobile(1);
		}
	}

	public void _MobileChangeResHigh(bool value)
	{
		if (value)
		{
			ResChangeMobile(2);
		}
	}

	private void ResChangeMobile(int i)
	{
		DataMgr.settingData.resTypeMobile = (ResolutionTypeMobile)i;
		resQualityToggleToggles[(int)DataMgr.settingData.resTypeMobile].isOn = true;
		resQualityToggleToggleTextss.ForEach(delegate(Text x)
		{
			x.color = Color.grey;
		});
		resQualityToggleToggleTextss[(int)DataMgr.settingData.resTypeMobile].color = Color.white;
		GameMgr.Inst.SetWindowsMode();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MobileChangeTargetFrameRate(int id)
	{
		if (mobileTargetFrameToggleToggles[id].isOn)
		{
			_MobileChangeTargetFrameRateForce(id);
		}
	}

	private void _MobileChangeTargetFrameRateForce(int id)
	{
		DataMgr.settingData.MobileTargetFrameRate = (MobileTargetFrameRate)id;
		mobileTargetFrameToggleToggles[id].isOn = true;
		mobileTargetFrameToggleTextss.ForEach(delegate(Text x)
		{
			x.color = Color.grey;
		});
		mobileTargetFrameToggleTextss[id].color = Color.white;
		switch (id)
		{
		case 0:
			Application.targetFrameRate = 30;
			break;
		case 1:
			Application.targetFrameRate = 60;
			break;
		case 2:
			Application.targetFrameRate = 90;
			break;
		}
		QualitySettings.vSyncCount = 0;
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeVSync()
	{
		if (DataMgr.settingData.Vsync)
		{
			VsyncShow.text = 1000136.GetText();
			DataMgr.settingData.Vsync = false;
			VsyncOff();
			gameobjectChangeFrameLimit.GetComponent<UISettingPointin>().SetEnable();
		}
		else if (!DataMgr.settingData.Vsync)
		{
			VsyncShow.text = 1000135.GetText();
			DataMgr.settingData.Vsync = true;
			VsyncOn();
			gameobjectChangeFrameLimit.GetComponent<UISettingPointin>().SetDisable();
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeVirtualStickScale()
	{
		DataMgr.settingData.Mobiledata.virtualStickScale = slider_VirtualStickScale.value;
		UIMgr.Inst.canvasScalerCanvas11.referenceResolution = new Vector2(UIMgr.Inst.canvasScalerCanvas11.referenceResolution.x, (float)MobileMgr.inst.scalerhight / DataMgr.settingData.Mobiledata.virtualStickScale);
	}

	public void _VolueChangeVirtualStickPosition()
	{
		DataMgr.settingData.Mobiledata.virtualStickPosition = sliderVirtualStickPosition.value;
		UIPlayerDataMgr.Inst.UpdateMobilePosition(DataMgr.settingData.Mobiledata.virtualStickPosition);
	}

	public void _VolueChangeRightStickSensitive()
	{
		DataMgr.settingData.Mobiledata.rightStickSensitiive = 1f - sliderRightStickSensitive.value;
		TopUI.inst.VirtualStickRight.stickDeadZone = DataMgr.settingData.Mobiledata.rightStickSensitiive;
	}

	public void _ToggleChangeVirtualStickRecover()
	{
		if (DataMgr.settingData.Mobiledata.virtualStickRecover)
		{
			virtualStickRecoverShow.text = 1000136.GetText();
			DataMgr.settingData.Mobiledata.virtualStickRecover = false;
			foreach (OnScreenStickCustom item in MobileMgr.inst.topui.VirtualStickComponent)
			{
				item.recoverPosition = false;
			}
		}
		else if (!DataMgr.settingData.Mobiledata.virtualStickRecover)
		{
			virtualStickRecoverShow.text = 1000135.GetText();
			DataMgr.settingData.Mobiledata.virtualStickRecover = true;
			foreach (OnScreenStickCustom item2 in MobileMgr.inst.topui.VirtualStickComponent)
			{
				item2.recoverPosition = true;
			}
		}
		SetMobileToggle(SettingSlotType.VirtualStickRecover, DataMgr.settingData.Mobiledata.virtualStickRecover);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _LanguageChange()
	{
		LanguageType value = (LanguageType)DropDown_Language.value;
		if (DataMgr.settingData.language == value)
		{
			EventMgr.LanguageChange?.Invoke();
			DataMgr.settingData.language = value;
			return;
		}
		DataMgr.settingData.language = value;
		TextConfig.RegetAllText();
		EventMgr.LanguageChange?.Invoke();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _LanguageChange_by_savefile()
	{
		LanguageType value = (LanguageType)DropDown_Language.value;
		if (DataMgr.settingData.language != value)
		{
			DropDown_Language.value = (int)DataMgr.settingData.language;
			TextConfig.RegetAllText();
			EventMgr.LanguageChange?.Invoke();
			SEMgr.Inst.uiClick.PlaySE();
		}
	}

	public void _LanguageChangeLeftRight(int i)
	{
		if (GameMgr.IsMobile_Static || ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			return;
		}
		switch (i)
		{
		case 0:
			if (DropDown_Language.value > 0)
			{
				DropDown_Language.value--;
			}
			else
			{
				DropDown_Language.value = DropDown_Language.options.Count - 1;
			}
			break;
		case 1:
			if (DropDown_Language.value < DropDown_Language.options.Count - 1)
			{
				DropDown_Language.value++;
			}
			else
			{
				DropDown_Language.value = 0;
			}
			break;
		default:
			return;
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _TextFloatChange()
	{
		if (DataMgr.settingData.textFloat)
		{
			DamageFloatButtonShow.text = 1000136.GetText();
			DataMgr.settingData.textFloat = false;
		}
		else if (!DataMgr.settingData.textFloat)
		{
			DamageFloatButtonShow.text = 1000135.GetText();
			DataMgr.settingData.textFloat = true;
		}
		SetMobileToggle(SettingSlotType.DamageFloat, DataMgr.settingData.textFloat);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _UIChangeTourMode()
	{
		if (DataMgr.settingData.isTouristMode)
		{
			TourModeButtonShow.text = 1000136.GetText();
			DataMgr.settingData.isTouristMode = false;
		}
		else
		{
			TourModeButtonShow.text = 1000135.GetText();
			DataMgr.settingData.isTouristMode = true;
		}
		SetMobileToggle(SettingSlotType.TourMode, DataMgr.settingData.isTouristMode);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeHardwareCursor()
	{
		if (DataMgr.settingData.hardwareCursor)
		{
			DataMgr.settingData.hardwareCursor = false;
		}
		else if (!DataMgr.settingData.hardwareCursor)
		{
			DataMgr.settingData.hardwareCursor = true;
		}
		UpdateCursor();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeCursorSize()
	{
		MobileMgr.inst.topui.MouseCursor.transform.localScale = new Vector3(Slider_MouseCursorSize.value, Slider_MouseCursorSize.value, 1f);
		DataMgr.settingData.CursorSize = Slider_MouseCursorSize.value;
	}

	public void _VolueChangeSafeMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			SafeMode_Show.text = 1000136.GetText();
			DataMgr.settingData.SafeMode = false;
		}
		else if (!DataMgr.settingData.SafeMode)
		{
			SafeMode_Show.text = 1000135.GetText();
			DataMgr.settingData.SafeMode = true;
		}
		SetMobileToggle(SettingSlotType.SafeMode, DataMgr.settingData.SafeMode);
		EventMgr.SafeModeStateChange?.Invoke();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeAiSummon()
	{
		if (DataMgr.settingData.AiSummon)
		{
			AiSummonShow.text = 1000136.GetText();
			DataMgr.settingData.AiSummon = false;
		}
		else if (!DataMgr.settingData.AiSummon)
		{
			AiSummonShow.text = 1000135.GetText();
			DataMgr.settingData.AiSummon = true;
		}
		SetMobileToggle(SettingSlotType.AISummon, DataMgr.settingData.AiSummon);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VolueChangeAimType()
	{
		switch (DataMgr.settingData.Mobiledata.aimType)
		{
		case MobileData.AimType.StrongAutoAim:
			DataMgr.settingData.Mobiledata.aimType = MobileData.AimType.WeakAutoAim;
			break;
		case MobileData.AimType.WeakAutoAim:
			DataMgr.settingData.Mobiledata.aimType = MobileData.AimType.StrongAutoAim;
			break;
		}
		virtualStickWeakAutoAimShow.text = ((DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim) ? 1000135.GetText() : 1000136.GetText());
		SetMobileToggle(SettingSlotType.MobileAimType, DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim);
	}

	public void _VolueChangeHalfAutoAimRange()
	{
		DataMgr.settingData.Mobiledata.halfAutoAimRange = !DataMgr.settingData.Mobiledata.halfAutoAimRange;
		virtualStickHalfAutoAimRangeShow.text = (DataMgr.settingData.Mobiledata.halfAutoAimRange ? 1000135.GetText() : 1000136.GetText());
		SetMobileToggle(SettingSlotType.HalfAutoAimRange, DataMgr.settingData.Mobiledata.halfAutoAimRange);
	}

	public void _VolueChangeIndieInteractButton()
	{
		DataMgr.settingData.Mobiledata.indieInteractButton = !DataMgr.settingData.Mobiledata.indieInteractButton;
		SetMobileToggle(SettingSlotType.IndieInteractButton, DataMgr.settingData.Mobiledata.indieInteractButton);
		individualInteractButtonShow.text = (DataMgr.settingData.Mobiledata.indieInteractButton ? 1000135.GetText() : 1000136.GetText());
		MobileMgr.inst.MobileUpdateInteractButtonShow();
	}

	public void _VolumeChangeMobileMobeLerp()
	{
		DataMgr.settingData.Mobiledata.mobileStickMoveLerp = !DataMgr.settingData.Mobiledata.mobileStickMoveLerp;
		SetMobileToggle(SettingSlotType.MoveLerp, DataMgr.settingData.Mobiledata.mobileStickMoveLerp);
		mobileMoveLerpShow.text = (DataMgr.settingData.Mobiledata.mobileStickMoveLerp ? 1000135.GetText() : 1000136.GetText());
	}

	public void _VolueChangeShakeScreenRatio()
	{
		DataMgr.settingData.screenShockRatio = slider_ShakeScreen.value;
	}

	public void _VolumeChangeSpellTranstarency()
	{
		DataMgr.settingData.SpellTransparent = slider_SpellTransparency.value;
		try
		{
			EventMgr.SpellTransparencyChange?.Invoke();
		}
		catch (MissingReferenceException ex)
		{
			Debug.LogWarning("透明度更新出错，这应该是仅在编辑器中才会出现的报错：" + ex);
		}
		textSpellTransparencyIsZero.gameObject.SetActive(DataMgr.settingData.SpellTransparent == 0f);
	}

	public void _VolumeChangeSummonTranstarency()
	{
		DataMgr.settingData.SummonTransparent = slider_SummonTransparency.value;
		if (!(PlayerMgr.Inst != null) || !(PlayerMgr.Inst.ItemCtrller != null) || PlayerMgr.Inst.ItemCtrller.relicCfg_ShowUnitHPUI == null)
		{
			return;
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Relic_ShowUnitHP));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		NativeArray<Relic_ShowUnitHP> nativeArray2 = entityQuery.ToComponentDataArray<Relic_ShowUnitHP>(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Relic_ShowUnitHP componentData = nativeArray2[i];
			componentData.SetTeammateTransparentChange();
			ettMgr.SetComponentData(nativeArray[i], componentData);
		}
	}

	public void _FullScreenSwitch(bool switchRight = true)
	{
		switch (DataMgr.settingData.windowsMode)
		{
		case SettingData.WindowsMode.BoardlessWindows:
			DataMgr.settingData.windowsMode = (switchRight ? SettingData.WindowsMode.Windows : SettingData.WindowsMode.FullScreen);
			break;
		case SettingData.WindowsMode.Windows:
			DataMgr.settingData.windowsMode = (switchRight ? SettingData.WindowsMode.FullScreen : SettingData.WindowsMode.BoardlessWindows);
			break;
		case SettingData.WindowsMode.FullScreen:
			DataMgr.settingData.windowsMode = ((!switchRight) ? SettingData.WindowsMode.Windows : SettingData.WindowsMode.BoardlessWindows);
			break;
		}
		Debug.Log(DataMgr.settingData.windowsMode);
		UpdateWindowsModeLanguage();
		DropDown_Resolution_SetOnStart();
		_ResChange_DropDown();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _OK()
	{
		SEMgr.Inst.uiClick.PlaySE();
		Hide();
	}

	public void _VirtualStickType(int i)
	{
		MobileMgr.inst.VirtualStickSet(i);
		_VirtualStickShow();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _VirtualStickType0(bool set)
	{
		if (set)
		{
			MobileMgr.inst.VirtualStickSet(0);
			_VirtualStickShow();
			SEMgr.Inst.uiClick.PlaySE();
		}
	}

	public void _VirtualStickType1(bool set)
	{
		if (set)
		{
			MobileMgr.inst.VirtualStickSet(1);
			_VirtualStickShow();
			SEMgr.Inst.uiClick.PlaySE();
		}
	}

	public void _VirtualStickType2(bool set)
	{
		if (set)
		{
			MobileMgr.inst.VirtualStickSet(2);
			_VirtualStickShow();
			SEMgr.Inst.uiClick.PlaySE();
		}
	}

	public void _VirtualStickType_ButtonLeft()
	{
		if (GameMgr.IsMobile_Static)
		{
			switch (DataMgr.settingData.VirtualStickType)
			{
			case 0:
				_VirtualStickType(2);
				break;
			case 1:
				_VirtualStickType(0);
				break;
			case 2:
				_VirtualStickType(1);
				break;
			}
		}
	}

	public void _VirtualStickType_ButtonRight()
	{
		switch (DataMgr.settingData.VirtualStickType)
		{
		case 0:
			_VirtualStickType(1);
			break;
		case 1:
			_VirtualStickType(2);
			break;
		case 2:
			_VirtualStickType(0);
			break;
		}
	}

	private void _VirtualStickShow()
	{
		if (DataMgr.settingData.VirtualStickType == 0)
		{
			toggle0.isOn = true;
			toggle1.isOn = false;
			toggle2.isOn = false;
		}
		else if (DataMgr.settingData.VirtualStickType == 1)
		{
			toggle0.isOn = false;
			toggle1.isOn = true;
			toggle2.isOn = false;
		}
		else if (DataMgr.settingData.VirtualStickType == 2)
		{
			toggle0.isOn = false;
			toggle1.isOn = false;
			toggle2.isOn = true;
		}
		if (DataMgr.settingData.VirtualStickType == 1)
		{
			settingSlotOthers.First((SettingSlot x) => x.SettingSlotType == SettingSlotType.VirtualStickRecover).UISettingMobileToggle.DisabtiveToggle();
		}
		else
		{
			settingSlotOthers.First((SettingSlot x) => x.SettingSlotType == SettingSlotType.VirtualStickRecover).UISettingMobileToggle.ActiveToggle();
		}
	}

	private void SetUISettings()
	{
		DropDown_Language.value = (int)DataMgr.settingData.language;
		if (GameMgr.IsMobile_Static)
		{
			if ((int)DataMgr.settingData.resTypeMobile < resQualityToggleToggles.Count)
			{
				ResChangeMobile((int)DataMgr.settingData.resTypeMobile);
			}
			_MobileChangeTargetFrameRateForce((int)DataMgr.settingData.MobileTargetFrameRate);
		}
		else if (GameMgr.IsSteamDeck_Static)
		{
			DropDown_Resolution.value = (int)DataMgr.settingData.resTypeSteamDeck2;
		}
		else
		{
			DropDown_Resolution.value = (int)DataMgr.settingData.resType;
		}
		slider_SpellTransparency.value = DataMgr.settingData.SpellTransparent;
		textSpellTransparencyIsZero.gameObject.SetActive(DataMgr.settingData.SpellTransparent == 0f);
		slider_ShakeScreen.value = DataMgr.settingData.screenShockRatio;
		slider_SummonTransparency.value = DataMgr.settingData.SummonTransparent;
		if (GameMgr.IsMobile_Static)
		{
			slider_VirtualStickScale.value = DataMgr.settingData.Mobiledata.virtualStickScale;
			sliderVirtualStickPosition.value = DataMgr.settingData.Mobiledata.virtualStickPosition;
			sliderRightStickSensitive.value = 1f - DataMgr.settingData.Mobiledata.rightStickSensitiive;
			foreach (OnScreenStickCustom item in MobileMgr.inst.topui.VirtualStickComponent)
			{
				item.recoverPosition = DataMgr.settingData.Mobiledata.virtualStickRecover;
			}
			virtualStickRecoverShow.text = (DataMgr.settingData.Mobiledata.virtualStickRecover ? 1000135.GetText() : 1000136.GetText());
		}
		UpdateWindowsModeLanguage();
		VsyncShow.text = (DataMgr.settingData.Vsync ? 1000135.GetText() : 1000136.GetText());
		TourModeButtonShow.text = (DataMgr.settingData.isTouristMode ? 1000135.GetText() : 1000136.GetText());
		DamageFloatButtonShow.text = (DataMgr.settingData.textFloat ? 1000135.GetText() : 1000136.GetText());
		Text_HardwareCursor_Show.text = (DataMgr.settingData.hardwareCursor ? 1000135.GetText() : 1000136.GetText());
		HideBattleUIShow.text = (DataMgr.settingData.BattleUIControl ? 1000135.GetText() : 1000136.GetText());
		SafeMode_Show.text = (DataMgr.settingData.SafeMode ? 1000135.GetText() : 1000136.GetText());
		AiSummonShow.text = (DataMgr.settingData.AiSummon ? 1000135.GetText() : 1000136.GetText());
		if (GameMgr.IsMobile_Static)
		{
			virtualStickWeakAutoAimShow.text = ((DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim) ? 1000135.GetText() : 1000136.GetText());
			virtualStickHalfAutoAimRangeShow.text = (DataMgr.settingData.Mobiledata.halfAutoAimRange ? 1000135.GetText() : 1000136.GetText());
			individualInteractButtonShow.text = (DataMgr.settingData.Mobiledata.indieInteractButton ? 1000135.GetText() : 1000136.GetText());
			mobileMoveLerpShow.text = (DataMgr.settingData.Mobiledata.mobileStickMoveLerp ? 1000135.GetText() : 1000136.GetText());
		}
		UpdateTourModeButton();
	}

	private void InitUISetting()
	{
		if (!GameMgr.IsSupportVFX)
		{
			settingSlotOthers.ForEach(delegate(SettingSlot x)
			{
				if (x.SettingSlotType == SettingSlotType.DamageFloat)
				{
					Debug.Log("Close DamageFloat");
					x.activeMobile = false;
				}
			});
		}
		if (GameMgr.IsMobile_Static)
		{
			if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				goMobileControllerFrame.gameObject.SetActive(value: true);
			}
			foreach (SettingSlot settingSlotOther in settingSlotOthers)
			{
				settingSlotOther.objRoot.SetActive(settingSlotOther.activeMobile);
			}
		}
		else
		{
			switch (ControlMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				foreach (SettingSlot settingSlotOther2 in settingSlotOthers)
				{
					settingSlotOther2.objRoot.SetActive(settingSlotOther2.activePCKey);
				}
				break;
			case PlayerInputType.Gamepad:
				foreach (SettingSlot settingSlotOther3 in settingSlotOthers)
				{
					settingSlotOther3.objRoot.SetActive(settingSlotOther3.activePCController);
				}
				break;
			}
		}
		DropDown_FrameLimit_SetOnStart();
		DropDown_FrameLimit.RefreshShownValue();
		if (!GameMgr.IsMobile_Static)
		{
			bool vsync = DataMgr.settingData.Vsync;
			DropDown_Resolution_SetOnStart();
			DropDown_Resolution.RefreshShownValue();
			DropDown_FrameLimit.value = (int)DataMgr.settingData.frameLimit;
			DataMgr.settingData.Vsync = !vsync;
			_VolueChangeVSync();
		}
	}

	private void DropDown_Resolution_SetOnStart()
	{
		if (GameMgr.IsMobile_Static)
		{
			DropDown_Resolution.options.Clear();
			for (int i = 0; i < 3; i++)
			{
				Dropdown.OptionData optionData = new Dropdown.OptionData();
				optionData.text = SettingData.GetResolutionCurrentScreen(i).ToString();
				DropDown_Resolution.options.Add(optionData);
			}
			DropDown_Resolution.RefreshShownValue();
		}
		else if (GameMgr.IsSteamDeck_Static)
		{
			DropDown_Resolution.options.Clear();
			for (int j = 0; j <= 1; j++)
			{
				Dropdown.OptionData optionData2 = new Dropdown.OptionData();
				optionData2.text = SettingData.GetResolutionCurrentScreen(j).ToString();
				DropDown_Resolution.options.Add(optionData2);
			}
			DropDown_Resolution.RefreshShownValue();
		}
		else
		{
			DropDown_Resolution.options.Clear();
			for (int k = 0; k <= 6; k++)
			{
				Dropdown.OptionData optionData3 = new Dropdown.OptionData();
				optionData3.text = SettingData.GetResolutionCurrentScreen(k).ToString();
				DropDown_Resolution.options.Add(optionData3);
			}
			DropDown_Resolution.RefreshShownValue();
		}
	}

	private void DropDown_FrameLimit_SetOnStart()
	{
		DropDown_FrameLimit.options.Clear();
		DropDown_FrameLimit.options.Add(new Dropdown.OptionData("30"));
		DropDown_FrameLimit.options.Add(new Dropdown.OptionData("60"));
		DropDown_FrameLimit.options.Add(new Dropdown.OptionData(1000175.GetText()));
		DropDown_FrameLimit.RefreshShownValue();
	}

	private static void SetCursorOrigion()
	{
		if (DataMgr.settingData.hardwareCursor)
		{
			UIMgr.Inst.uiSetting.SetcursorTexture_Cursor(UIMgr.Inst.uiSetting.OrigionCursor);
		}
		else
		{
			UIMgr.Inst.uiSetting.SetcursorTexture_Sprite(UIMgr.Inst.uiSetting.OrigionCursor_Sprite);
		}
	}

	public static void SetCursorNull()
	{
		if (!DataMgr.settingData.hardwareCursor)
		{
			UIMgr.Inst.uiSetting.SetcursorTexture_Cursor(UIMgr.Inst.uiSetting.emptycursor);
		}
		else
		{
			UIMgr.Inst.uiSetting.SetcursorTexture_Cursor(UIMgr.Inst.uiSetting.emptycursor);
		}
	}

	public static void SetCursorWand()
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard || GameMgr.IsSteamDeck_Static)
		{
			if (DataMgr.settingData.hardwareCursor)
			{
				UIMgr.Inst.uiSetting.SetcursorTexture_Cursor(UIMgr.Inst.uiSetting.wandcursor);
				return;
			}
			Debug.Log("SetcursorTexture_Sprite");
			UIMgr.Inst.uiSetting.SetcursorTexture_Sprite(UIMgr.Inst.uiSetting.wandcursor_Sprite);
		}
	}

	public static void VsyncOn()
	{
		QualitySettings.vSyncCount = 1;
	}

	public static void VsyncOff()
	{
		QualitySettings.vSyncCount = 0;
		if (!GameMgr.IsMobile_Static)
		{
			switch (DataMgr.settingData.frameLimit)
			{
			case SettingData.FrameLimit.limit30:
				Application.targetFrameRate = 30;
				break;
			case SettingData.FrameLimit.limit60:
				Application.targetFrameRate = 60;
				break;
			case SettingData.FrameLimit.NoLimit:
				Application.targetFrameRate = 999;
				break;
			}
		}
	}

	public void UpdateControlShow()
	{
		if (!GameMgr.IsMobile_Static)
		{
			CC_MoveUp_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WASD.GetBindingDisplayString(1));
			CC_MoveDown_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WASD.GetBindingDisplayString(2));
			CC_MoveLeft_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WASD.GetBindingDisplayString(3));
			CC_MoveRight_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WASD.GetBindingDisplayString(4));
			CC_interact_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Interact.GetBindingDisplayString(0));
			CC_bag_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Bag.GetBindingDisplayString(0));
			CC_shoot_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Shoot.GetBindingDisplayString(0));
			CC_UsePotion_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Drink.GetBindingDisplayString(0));
			CC_switchwand1_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha1.GetBindingDisplayString(0));
			CC_switchwand2_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha2.GetBindingDisplayString(0));
			CC_switchwand3_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha3.GetBindingDisplayString(0));
			CC_switchwand4_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha4.GetBindingDisplayString(0));
			CC_switchwand5_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha5.GetBindingDisplayString(0));
			CC_switchwand6_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.Alpha6.GetBindingDisplayString(0));
			CC_switchwandUP_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WandUp.GetBindingDisplayString(0));
			CC_switchwandDown_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.WandDown.GetBindingDisplayString(0));
			CC_switchpotionUp_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.PotionUp.GetBindingDisplayString(0));
			CC_switchpotionDown_MainButtonShow.text = ControlMgr.GetKeyDisplayName(base.inputActions.Player.PotionDown.GetBindingDisplayString(0));
			CC_Sprint_ButtonSHow.text = base.inputActions.Player.Sprint.GetBindingDisplayString(0);
			CC_QuickPanel_MainButtonShow.text = base.inputActions.Player.QuickPanel.GetBindingDisplayString(0);
			CC_KillSummon_MainButtonShow.text = base.inputActions.Player.KillSummon.GetBindingDisplayString(0);
			CC_Move_MainButtonShow_Pad.text = base.inputActions.Player.GamepadDirect.GetBindingDisplayString(0);
			CC_Aim_MainButtonShow_Pad.text = base.inputActions.Player.RightStick.GetBindingDisplayString(0);
			CC_interact_MainButtonShow_Pad.text = base.inputActions.Player.Interact.GetBindingDisplayString(1);
			CC_shoot_MainButtonShow_Pad.text = base.inputActions.Player.Shoot.GetBindingDisplayString(1);
			CC_bag_MainButtonShow_Pad.text = base.inputActions.Player.Bag.GetBindingDisplayString(1);
			CC_switchwandUP_MainButtonShow_Pad.text = base.inputActions.Player.WandUp.GetBindingDisplayString(1);
			CC_switchwandDown_MainButtonShow_Pad.text = base.inputActions.Player.WandDown.GetBindingDisplayString(1);
			CC_UsePotion_MainButtonShow_Pad.text = base.inputActions.Player.Drink.GetBindingDisplayString(1);
			CC_Drop_MainButtonShow_Pad.text = base.inputActions.Player.Drop.GetBindingDisplayString(0);
			CC_MoveObj_MainButtonShow_Pad.text = base.inputActions.Player.GamepadWest.GetBindingDisplayString(0);
			CC_Back_MainButtonShow_Pad.text = base.inputActions.Player.GamepadEast.GetBindingDisplayString(0);
			CC_Menue_MainButtonShow_Pad.text = base.inputActions.Player.Pause.GetBindingDisplayString(1);
		}
	}

	public void ShowWaitPress()
	{
		Canvas_waitpress.SetActive(value: true);
	}

	public void HideWaitPress()
	{
		Canvas_waitpress.SetActive(value: false);
	}

	private void Language_ChangeControlLanguage()
	{
		text_waitpress.text = 1000148.GetText();
		text_longpresspad.text = 1000241.GetText();
		text_pressDown.text = 1000242.GetText();
		if (!GameMgr.IsMobile_Static)
		{
			text_usingkey.text = "◆ " + 1000126.GetText() + " ◆";
			text_usingpad.text = "◆ " + 1000127.GetText() + " ◆";
			CC_Longpress.text = 1000238.GetText();
			CC_bag_text.text = 1000119.GetText();
			CC_MoveUp_text.text = 1000114.GetText();
			CC_MoveDown_text.text = 1000115.GetText();
			CC_MoveLeft_text.text = 1000116.GetText();
			CC_MoveRight_text.text = 1000117.GetText();
			CC_interact_text.text = 1000118.GetText();
			CC_bag_text.text = 1000119.GetText();
			CC_shoot_text.text = 1000120.GetText();
			CC_SwitchPotion_text.text = 1000124.GetText();
			CC_switchwand_text.text = 1000122.GetText();
			CC_Sprint_text.text = 1000168.GetText();
			CC_UsePotion_text.text = 1000123.GetText();
			CC_SwitchWand_text.text = 1000121.GetText();
			CC_QuickPanel_text.text = 1000170.GetText();
			CC_KillSummon_text.text = 1000171.GetText();
		}
		CC_Move_text_Pad.text = 1000128.GetText();
		CC_Aim_MainButtonAim_Pad.text = 1000129.GetText();
		CC_interact_text_Pad.text = 1000118.GetText();
		CC_shoot_text_Pad.text = 1000120.GetText();
		CC_bag_text_Pad.text = 1000119.GetText();
		CC_switchwandUp_text_Pad.text = 1000130.GetText();
		CC_switchwandDown_text_Pad.text = 1000131.GetText();
		CC_UsePotion_text_pad.text = 1000124.GetText();
		CC_Sprint_text_pad.text = 1000168.GetText();
		CC_throw_text_pad.text = 1000132.GetText();
		CC_MoveObj_text_pad.text = 1000133.GetText();
		CC_back_pad.text = 1000134.GetText();
		CC_menu_text_Pad.text = 1000147.GetText();
		CC_QuickPanel_text_Pad.text = 1000170.GetText();
		CC_KillSummon_text_Pad.text = 1000171.GetText();
	}

	public void ChangeKey(int enumInt)
	{
		ControlMgr.Inst.rebinding = true;
		Debug.Log("ChangeKey:" + enumInt);
		switch (enumInt)
		{
		case 0:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WASD, ControlMgr.controltype.key, 0);
			break;
		case 2:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WASD, ControlMgr.controltype.key, 2);
			break;
		case 1:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WASD, ControlMgr.controltype.key, 1);
			break;
		case 3:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WASD, ControlMgr.controltype.key, 3);
			break;
		case 5:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Interact, ControlMgr.controltype.key, 5);
			break;
		case 6:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Bag, ControlMgr.controltype.key, 6);
			break;
		case 4:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Shoot, ControlMgr.controltype.key, 4);
			break;
		case 15:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Drink, ControlMgr.controltype.key, 15);
			break;
		case 9:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha1, ControlMgr.controltype.key, 9);
			break;
		case 10:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha2, ControlMgr.controltype.key, 10);
			break;
		case 11:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha3, ControlMgr.controltype.key, 11);
			break;
		case 12:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha4, ControlMgr.controltype.key, 12);
			break;
		case 13:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha5, ControlMgr.controltype.key, 13);
			break;
		case 14:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Alpha6, ControlMgr.controltype.key, 14);
			break;
		case 7:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WandUp, ControlMgr.controltype.key, 7, DeleteFirstCandidate: true);
			break;
		case 8:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WandDown, ControlMgr.controltype.key, 8, DeleteFirstCandidate: true);
			break;
		case 16:
			ControlMgr.Inst.Changekey(base.inputActions.Player.PotionUp, ControlMgr.controltype.key, 16);
			break;
		case 17:
			ControlMgr.Inst.Changekey(base.inputActions.Player.PotionDown, ControlMgr.controltype.key, 17);
			break;
		case 18:
			ControlMgr.Inst.Changekey(base.inputActions.Player.QuickRemove, ControlMgr.controltype.key, 18);
			break;
		case 19:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Sprint, ControlMgr.controltype.key, 19);
			break;
		case 24:
			ControlMgr.Inst.Changekey(base.inputActions.Player.QuickPanel, ControlMgr.controltype.key, 24);
			break;
		case 25:
			ControlMgr.Inst.Changekey(base.inputActions.Player.KillSummon, ControlMgr.controltype.key, 25);
			break;
		case 20:
		case 21:
		case 22:
		case 23:
			break;
		}
	}

	public void ChangeColtroller(int _controlEnum)
	{
		switch (_controlEnum)
		{
		case 0:
			ControlMgr.Inst.Changekey(base.inputActions.Player.LeftStick, ControlMgr.controltype.pad, 0);
			break;
		case 1:
			ControlMgr.Inst.Changekey(base.inputActions.Player.RightStick, ControlMgr.controltype.pad, 1);
			break;
		case 2:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Interact, ControlMgr.controltype.pad, 2);
			break;
		case 3:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Shoot, ControlMgr.controltype.pad, 3);
			break;
		case 4:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Bag, ControlMgr.controltype.pad, 4);
			break;
		case 5:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WandUp, ControlMgr.controltype.pad, 5);
			break;
		case 6:
			ControlMgr.Inst.Changekey(base.inputActions.Player.WandDown, ControlMgr.controltype.pad, 6);
			break;
		case 7:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Pause, ControlMgr.controltype.pad, 7);
			break;
		case 8:
			ControlMgr.Inst.Changekey(base.inputActions.Player.Drink, ControlMgr.controltype.pad, 8);
			break;
		case 9:
			ControlMgr.Inst.Changekey(base.inputActions.Player.PotionDown, ControlMgr.controltype.pad, 9);
			break;
		case 10:
			ControlMgr.Inst.Changekey(base.inputActions.Player.GamepadWest, ControlMgr.controltype.pad, 10);
			break;
		case 11:
			ControlMgr.Inst.Changekey(base.inputActions.Player.GamepadWest, ControlMgr.controltype.pad, 11);
			break;
		case 12:
			ControlMgr.Inst.Changekey(base.inputActions.Player.GamepadEast, ControlMgr.controltype.pad, 12);
			break;
		}
	}

	public void SetAllKeyBackground()
	{
		UIMgr.Inst.uiSetting.SetControllerBackground(0, DataMgr.settingData.controldata.Controler_move);
		UIMgr.Inst.uiSetting.SetControllerBackground(1, DataMgr.settingData.controldata.Controler_aim);
		UIMgr.Inst.uiSetting.SetControllerBackground(2, DataMgr.settingData.controldata.Controler_interact);
		UIMgr.Inst.uiSetting.SetControllerBackground(3, DataMgr.settingData.controldata.Controler_shoot);
		UIMgr.Inst.uiSetting.SetControllerBackground(4, DataMgr.settingData.controldata.Controler_bag);
		UIMgr.Inst.uiSetting.SetControllerBackground(5, DataMgr.settingData.controldata.Controler_wandup);
		UIMgr.Inst.uiSetting.SetControllerBackground(6, DataMgr.settingData.controldata.Controler_wanddown);
		UIMgr.Inst.uiSetting.SetControllerBackground(7, DataMgr.settingData.controldata.Controler_menue);
		UIMgr.Inst.uiSetting.SetControllerBackground(8, DataMgr.settingData.controldata.Controler_usepotion);
		UIMgr.Inst.uiSetting.SetControllerBackground(10, DataMgr.settingData.controldata.Controler_moveObj);
		UIMgr.Inst.uiSetting.SetControllerBackground(11, DataMgr.settingData.controldata.Controler_moveObj);
		UIMgr.Inst.uiSetting.SetControllerBackground(12, DataMgr.settingData.controldata.Controler_back);
		UIMgr.Inst.uiSetting.SetControllerBackground(14, DataMgr.settingData.controldata.Controler_QuickPanel);
		UIMgr.Inst.uiSetting.SetControllerBackground(13, DataMgr.settingData.controldata.Controler_KillSummon);
		if (!GameMgr.IsMobile_Static)
		{
			UIMgr.Inst.uiSetting.SetKeyBackground(0, DataMgr.settingData.controldata.Key_w);
			UIMgr.Inst.uiSetting.SetKeyBackground(1, DataMgr.settingData.controldata.Key_s);
			UIMgr.Inst.uiSetting.SetKeyBackground(2, DataMgr.settingData.controldata.Key_a);
			UIMgr.Inst.uiSetting.SetKeyBackground(3, DataMgr.settingData.controldata.Key_d);
			UIMgr.Inst.uiSetting.SetKeyBackground(4, DataMgr.settingData.controldata.Key_shoot);
			UIMgr.Inst.uiSetting.SetKeyBackground(5, DataMgr.settingData.controldata.Key_e);
			UIMgr.Inst.uiSetting.SetKeyBackground(6, DataMgr.settingData.controldata.Key_bag);
			UIMgr.Inst.uiSetting.SetKeyBackground(7, DataMgr.settingData.controldata.Key_wandup);
			UIMgr.Inst.uiSetting.SetKeyBackground(8, DataMgr.settingData.controldata.Key_wanddown);
			UIMgr.Inst.uiSetting.SetKeyBackground(9, DataMgr.settingData.controldata.Key_wand1);
			UIMgr.Inst.uiSetting.SetKeyBackground(10, DataMgr.settingData.controldata.Key_wand2);
			UIMgr.Inst.uiSetting.SetKeyBackground(11, DataMgr.settingData.controldata.Key_wand3);
			UIMgr.Inst.uiSetting.SetKeyBackground(12, DataMgr.settingData.controldata.Key_wand4);
			UIMgr.Inst.uiSetting.SetKeyBackground(13, DataMgr.settingData.controldata.Key_wand5);
			UIMgr.Inst.uiSetting.SetKeyBackground(14, DataMgr.settingData.controldata.Key_wand6);
			UIMgr.Inst.uiSetting.SetKeyBackground(15, DataMgr.settingData.controldata.Key_usepotion);
			UIMgr.Inst.uiSetting.SetKeyBackground(16, DataMgr.settingData.controldata.Key_potionup);
			UIMgr.Inst.uiSetting.SetKeyBackground(17, DataMgr.settingData.controldata.Key_potiondown);
			UIMgr.Inst.uiSetting.SetKeyBackground(18, DataMgr.settingData.controldata.Key_Sprint);
			UIMgr.Inst.uiSetting.SetKeyBackground(19, DataMgr.settingData.controldata.Key_KillSummon);
			UIMgr.Inst.uiSetting.SetKeyBackground(20, DataMgr.settingData.controldata.Key_QuickPanel);
		}
	}

	private void SetKeyBackground(int i, string imagename = null, string keyname = null)
	{
		Sprite sprite = getkeyimage(imagename);
		keycontrol[i].background_image.sprite = sprite;
		if (sprite == controlSprite_Default)
		{
			keycontrol[i].Keyname.enabled = true;
			float num = keycontrol[i].Keyname.preferredWidth + 30f;
			if (num < 51f)
			{
				num = 51f;
			}
			keycontrol[i].background.sizeDelta = new Vector2(num, 57.3f);
		}
		else
		{
			keycontrol[i].Keyname.enabled = false;
			keycontrol[i].background.sizeDelta = new Vector2(keycontrol[i].background_image.sprite.texture.width, keycontrol[i].background_image.sprite.texture.height);
		}
	}

	private void SetControllerBackground(int i, string imagename = null)
	{
		Sprite sprite = getcontrolimage(imagename);
		if (sprite == controlSprite_Default)
		{
			controllercontrol[i].Keyname.enabled = true;
			float preferredWidth = controllercontrol[i].Keyname.preferredWidth;
			if (controllercontrol[i].Keyname.preferredWidth < controllercontrol[i].background.sizeDelta.y)
			{
				controllercontrol[i].background.sizeDelta = new Vector2(controllercontrol[i].background.sizeDelta.y + (float)sqare_offset, controllercontrol[i].background.sizeDelta.y);
			}
			else
			{
				controllercontrol[i].background.sizeDelta = new Vector2(preferredWidth + (float)width_offset + (float)sqare_offset, controllercontrol[i].background.sizeDelta.y);
			}
		}
		else
		{
			controllercontrol[i].Keyname.enabled = false;
			controllercontrol[i].background_image.sprite = sprite;
			controllercontrol[i].background.sizeDelta = new Vector2(controllercontrol[i].background_image.sprite.texture.width, controllercontrol[i].background_image.sprite.texture.height);
		}
	}

	public Sprite getkeyimage(string name)
	{
		return name switch
		{
			"/Keyboard/upArrow" => key_UpArrow, 
			"/Keyboard/downArrow" => key_DownArrow, 
			"/Keyboard/leftArrow" => key_LeftArrow, 
			"/Keyboard/rightArrow" => key_RightArrow, 
			"/Keyboard/space" => key_Space, 
			"/Mouse/rightButton" => mouse_right, 
			"/Mouse/leftButton" => mouse_left, 
			"/Mouse/scroll/up" => mouse_middleup, 
			"/Mouse/scroll/down" => mouse_middledown, 
			"<Keyboard>/upArrow" => key_UpArrow, 
			"<Keyboard>/downArrow" => key_DownArrow, 
			"<Keyboard>/leftArrow" => key_LeftArrow, 
			"<Keyboard>/rightArrow" => key_RightArrow, 
			"<Keyboard>/space" => key_Space, 
			"<Mouse>/rightButton" => mouse_right, 
			"<Mouse>/leftButton" => mouse_left, 
			"<Mouse>/scroll/up" => mouse_middleup, 
			"<Mouse>/scroll/down" => mouse_middledown, 
			"<Mouse>/Scroll/up" => mouse_middleup, 
			"<Mouse>/Scroll/down" => mouse_middledown, 
			_ => controlSprite_Default, 
		};
	}

	public Sprite getcontrolimage(string name)
	{
		if (name.Split('/').Length == 3)
		{
			return SwitchController(name.Split('/')[2]);
		}
		return SwitchController(name.Split('/')[1]);
		Sprite SwitchController(string controlName)
		{
			switch (controlName)
			{
			case "upArrow":
				return key_UpArrow;
			case "downArrow":
				return key_DownArrow;
			case "leftArrow":
				return key_LeftArrow;
			case "rightArrow":
				return key_RightArrow;
			case "space":
				return key_Space;
			case "rightButton":
				return mouse_right;
			case "leftButton":
				return mouse_left;
			case "scroll/up":
				return mouse_middleup;
			case "scroll/down":
				return mouse_middledown;
			case "rightTrigger":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS || GameMgr.IsSteamDeck_Static)
				{
					return Controller_R2;
				}
				return Controller_RT;
			case "leftTrigger":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS || GameMgr.IsSteamDeck_Static)
				{
					return Controller_L2;
				}
				return Controller_LT;
			case "leftShoulder":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS || GameMgr.IsSteamDeck_Static)
				{
					return Controller_L1;
				}
				return Controller_LB;
			case "rightShoulder":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS || GameMgr.IsSteamDeck_Static)
				{
					return Controller_R1;
				}
				return Controller_RB;
			case "buttonNorth":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
				{
					return Controller_North_PS;
				}
				return Controller_North;
			case "buttonSouth":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
				{
					return Controller_South_PS;
				}
				return Controller_South;
			case "buttonEast":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
				{
					return Controller_East_PS;
				}
				return Controller_East;
			case "buttonWest":
				if (ControlMgr.Inst.GetControllerType() == ControlMgr.controllertype.PS)
				{
					return Controller_West_PS;
				}
				return Controller_West;
			case "start":
				return Controller_Start;
			case "select":
				return Controller_Select;
			case "leftStick":
				return Controller_LeftStick;
			case "rightStick":
				return Controller_RightStick;
			case "leftStickPress":
				return Controller_LeftStick;
			case "rightStickPress":
				return Controller_RightStick;
			default:
				return controlSprite_Default;
			}
		}
	}

	public void RestAllControl()
	{
		if ((GameMgr.IsMobile_Static && (bool)gameobjectMobileControlGamepad) || toggleControl.toggle.isOn)
		{
			if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				controllercontrol[int_control_select].showtext.color = colorUnselected_text;
				controllercontrol[0].showtext.color = colorSelected_text;
			}
			ControlMgr.Inst.ReSetControl();
			ControlMgr.Inst.loadcontrol();
			EventMgr.ControlChange?.Invoke();
			UpdateControlShow();
			SetAllKeyBackground();
		}
		else if (toggleOther.toggle.isOn)
		{
			DamageFloatButtonShow.text = 1000135.GetText();
			DataMgr.settingData.textFloat = true;
			slider_SpellTransparency.value = 1f;
			_VolumeChangeSpellTranstarency();
			slider_SummonTransparency.value = 1f;
			_VolumeChangeSummonTranstarency();
			slider_ShakeScreen.value = 1f;
			_VolueChangeShakeScreenRatio();
			AiSummonShow.text = 1000136.GetText();
			DataMgr.settingData.AiSummon = false;
			if (DataMgr.settingData.SafeMode)
			{
				SafeMode_Show.text = 1000136.GetText();
				DataMgr.settingData.SafeMode = false;
				EventMgr.SafeModeStateChange?.Invoke();
			}
			if (!DataMgr.settingData.hardwareCursor)
			{
				DataMgr.settingData.hardwareCursor = true;
				UpdateCursor();
			}
			if ((bool)UICampMgr.Inst && DataMgr.settingData.isTouristMode)
			{
				TourModeButtonShow.text = 1000136.GetText();
				DataMgr.settingData.isTouristMode = false;
			}
		}
		else if (GameMgr.IsMobile_Static && toggleMobileControl.toggle.isOn)
		{
			toggle0.isOn = true;
			toggle1.isOn = false;
			toggle2.isOn = false;
			DataMgr.settingData.Mobiledata.virtualStickRecover = false;
			_ToggleChangeVirtualStickRecover();
			sliderVirtualStickPosition.value = 0f;
			_VolueChangeVirtualStickPosition();
			slider_VirtualStickScale.value = 1f;
			_VolueChangeVirtualStickScale();
			sliderRightStickSensitive.value = 0.5f;
			_VolueChangeRightStickSensitive();
			DataMgr.settingData.Mobiledata.indieInteractButton = true;
			_VolueChangeIndieInteractButton();
			DataMgr.settingData.Mobiledata.halfAutoAimRange = true;
			_VolueChangeHalfAutoAimRange();
			DataMgr.settingData.Mobiledata.mobileStickMoveLerp = false;
			_VolumeChangeMobileMobeLerp();
			DataMgr.settingData.Mobiledata.aimType = MobileData.AimType.WeakAutoAim;
			_VolueChangeAimType();
		}
		if (GameMgr.IsMobile_Static)
		{
			SetAllMobileToggle();
		}
	}

	public void SetcontrolSelect(int controlselect)
	{
		int_control_select = controlselect;
	}

	public void MobileSetTransparent()
	{
		anima.SetTrigger("MobileSetTransparent");
		UIMgr.TryAdditionalMobileHide(base.transform);
		UIMgr.Inst.UIMenu.anima_Menu.SetTrigger("MobileSetTransparent");
	}

	public void MobileSetTransparentNot()
	{
		anima.SetTrigger("MobileSetTransparentNot");
		UIMgr.TryAdditionalMobileShow(base.transform);
		UIMgr.Inst.UIMenu.anima_Menu.SetTrigger("MobileSetTransparentNot");
	}

	public void AdjustTopUIStart()
	{
		MobileMgr.inst.topui.canvas.sortingLayerName = "UIOverParticle";
		MobileMgr.inst.mobileControlDontUpdateCounter++;
		MobileMgr.inst.ShowControl();
	}

	public void AdjustTopUIEnd()
	{
		MobileMgr.inst.topui.canvas.sortingLayerName = "Default";
		MobileMgr.inst.mobileControlDontUpdateCounter--;
	}

	public void AdjustTopUIStartCustomControl()
	{
		customMobileControl.SetActive(value: true);
		uiMobileReturnAndRessCustomControl.Show();
		MobileMgr.inst.topui.canvas.sortingLayerName = "UIOverParticle";
		MobileMgr.inst.mobileControlDontUpdateCounter++;
		TopUI.inst.AdjustStart();
		MobileMgr.inst.ShowControl();
		MobileMgr.inst.ActiveButtonDrink();
		MobileMgr.inst.ActiveButtonSkill();
		MobileMgr.inst.ActiveButtonSwitchWand();
		TopUI.inst.uI_AimSkill.skillCancleObj.SetActive(value: true);
		TopUI.inst.uiPotionSelectPopOut.potionDropObj.SetActive(value: true);
	}

	public void AdjustTopUIEndCustomControl()
	{
		MobileMgr.inst.mobileControlDontUpdateCounter--;
		customMobileControl.SetActive(value: false);
		uiMobileReturnAndRessCustomControl.Hide();
		MobileMgr.inst.topui.canvas.sortingLayerName = "Default";
		TopUI.inst.VirtualStickComponent.ForEach(delegate(OnScreenStickCustom x)
		{
			x.ResetRecoverPosition();
		});
		TopUI.inst.VirtualStickRight.ResetRecoverPosition();
		UseCustomAdjust();
		MobileMgr.inst.HideControl();
		TopUI.inst.AdjustEnd();
		DataMgr.SaveSettingData();
		TopUI.inst.currentVirtualStickSizeAdjust = null;
		MobileMgr.inst.UpdateMobileButtons();
		TopUI.inst.uI_AimSkill.skillCancleObj.SetActive(value: false);
		TopUI.inst.uiPotionSelectPopOut.potionDropObj.SetActive(value: false);
	}

	public void UseCustomAdjust(bool onStart = false)
	{
		TopUI.inst.AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			if (!onStart)
			{
				x.SaveToSetting();
			}
			x.InitSizeAndPositiion();
		});
		MobileVirtualButtonData mobileVirtualButtonData = DataMgr.settingData.Mobiledata.virtualStickData2[1];
		if (mobileVirtualButtonData.globalPositionx != 999f && mobileVirtualButtonData.globalPositiony != 999f)
		{
			TopUI.inst.guideMobileLeftStick.transform.position = new Vector2(mobileVirtualButtonData.globalPositionx, mobileVirtualButtonData.globalPositiony);
		}
	}

	public void _ResetAllVirtualButton()
	{
		slider_changeVirtualSize.value = 1f;
		slider_changeVirtualTransparency.value = 1f;
		TopUI.inst.AllVirtualStickAdjusts.ForEach(delegate(VirtualStickSizeAdjust x)
		{
			x.SetToDefault();
			x.SaveToSetting();
		});
		UseCustomAdjust();
		DataMgr.SaveSettingData();
	}

	public void _VirtualButtonSizeChange()
	{
		if (!(TopUI.inst.currentVirtualStickSizeAdjust == null))
		{
			TopUI.inst.currentVirtualStickSizeAdjust.SetSize(slider_changeVirtualSize.value * Vector3.one);
		}
	}

	public void _VirtualButtonTransparencyChange()
	{
		if (!(TopUI.inst.currentVirtualStickSizeAdjust == null))
		{
			TopUI.inst.currentVirtualStickSizeAdjust.canvasGroup.alpha = slider_changeVirtualTransparency.value;
		}
	}

	private void UpdateCurseSizeButton()
	{
		if (DataMgr.settingData.hardwareCursor)
		{
			DisableCursorSizeButton();
		}
		else
		{
			EnableCursorSizeButton();
		}
	}

	private void DisableCursorSizeButton()
	{
		Text_HardwareCursor_Show.text = 1000135.GetText();
		Slider_MouseCursorSize.interactable = false;
		imageSliderMouseCursorSizeBackground.color = colorSliderBackgroundDisabled;
		gameobject_CursorSize_Slider.GetComponent<UISettingPointin>().SetDisable();
	}

	private void EnableCursorSizeButton()
	{
		Text_HardwareCursor_Show.text = 1000136.GetText();
		Slider_MouseCursorSize.interactable = true;
		imageSliderMouseCursorSizeBackground.color = colorSliderBackgroundActive;
		gameobject_CursorSize_Slider.GetComponent<UISettingPointin>().SetEnable();
	}

	private void UpdateTourModeButton()
	{
		if (GameMgr.IsMobile_Static)
		{
			SettingSlot settingSlot = settingSlotOthers.First((SettingSlot x) => x.SettingSlotType == SettingSlotType.TourMode);
			if ((bool)UICampMgr.Inst)
			{
				settingSlot.UISettingMobileToggle.ActiveToggle();
			}
			else
			{
				settingSlot.UISettingMobileToggle.DisabtiveToggle();
			}
		}
		else if ((bool)UICampMgr.Inst)
		{
			EnableTourModeButton();
		}
		else
		{
			DisableTourModeButton();
		}
	}

	private void DisableTourModeButton()
	{
		gameobjectTourMode.GetComponent<UISettingPointin>().SetDisable();
	}

	private void EnableTourModeButton()
	{
		gameobjectTourMode.GetComponent<UISettingPointin>().SetEnable();
	}

	public void UpdateCursor()
	{
		if (!GameMgr.IsMobile_Static)
		{
			if (UIMgr.Inst.InputType == PlayerInputType.Keyboard || GameMgr.IsSteamDeck_Static)
			{
				ChangeCursorStyle();
			}
			UpdateCurseSizeButton();
		}
	}

	private void ChangeCursorStyle()
	{
		if ((bool)GuideMgr.Inst)
		{
			if (DataMgr.settingData.hardwareCursor)
			{
				if (GuideMgr.Inst.IsPickedWand)
				{
					SetCursorWand();
				}
				else
				{
					SetCursorOrigion();
				}
				TopUI.inst.MouseCursor.SetActive(value: false);
				return;
			}
			if (GuideMgr.Inst.IsPickedWand)
			{
				SetCursorWand();
			}
			else
			{
				SetCursorOrigion();
			}
			SetcursorTexture_Cursor(emptycursor);
			TopUI.inst.MouseCursor.transform.localScale = new Vector3(Slider_MouseCursorSize.value, Slider_MouseCursorSize.value, 1f);
			TopUI.inst.MouseCursor.SetActive(value: true);
		}
		else if (DataMgr.settingData.hardwareCursor)
		{
			SetcursorTexture_Cursor(wandcursor);
			TopUI.inst.MouseCursor.SetActive(value: false);
		}
		else
		{
			SetcursorTexture_Cursor(emptycursor);
			SetCursorWand();
			TopUI.inst.MouseCursor.transform.localScale = new Vector3(Slider_MouseCursorSize.value, Slider_MouseCursorSize.value, 1f);
			TopUI.inst.MouseCursor.SetActive(value: true);
		}
	}

	private void SetcursorTexture_Cursor(Texture2D texture)
	{
		Cursor.SetCursor(texture, new Vector2(0f, 0f), CursorMode.Auto);
	}

	private void SetcursorTexture_Sprite(Sprite Sprite)
	{
		TopUI.inst.MouseCursor.transform.GetChild(0).GetComponent<Image>().sprite = Sprite;
	}

	private void SetMobileToggle(SettingSlotType settingSlotType, bool value, bool anime = true)
	{
		if (GameMgr.IsMobile_Static)
		{
			settingSlotOthers.First((SettingSlot x) => x.SettingSlotType == settingSlotType).UISettingMobileToggle?.SetToggle(value, anime);
		}
	}

	public void _SwitchTestButton()
	{
		Debug.Log("_SwitchTestButton");
		TopUI.inst.testButton.SetActive(!TopUI.inst.testButton.activeSelf);
		if (GameMgr.IsMobile_Static)
		{
			uiSettingMobileToggleTest.SetToggle(TopUI.inst.testButton.activeSelf, anime: true);
		}
	}

	private void SetAllMobileToggle(bool anime = false)
	{
		SetMobileToggle(SettingSlotType.VirtualStickRecover, DataMgr.settingData.Mobiledata.virtualStickRecover, anime);
		SetMobileToggle(SettingSlotType.DamageFloat, DataMgr.settingData.textFloat, anime);
		SetMobileToggle(SettingSlotType.TourMode, DataMgr.settingData.isTouristMode, anime);
		SetMobileToggle(SettingSlotType.SafeMode, DataMgr.settingData.SafeMode, anime);
		SetMobileToggle(SettingSlotType.AISummon, DataMgr.settingData.AiSummon, anime);
		SetMobileToggle(SettingSlotType.MobileAimType, DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim, anime);
		SetMobileToggle(SettingSlotType.HalfAutoAimRange, DataMgr.settingData.Mobiledata.halfAutoAimRange, anime);
		SetMobileToggle(SettingSlotType.IndieInteractButton, DataMgr.settingData.Mobiledata.indieInteractButton, anime);
		SetMobileToggle(SettingSlotType.MoveLerp, DataMgr.settingData.Mobiledata.mobileStickMoveLerp, anime);
	}

	private void OnCDKeyBtnClick()
	{
		if (!string.IsNullOrEmpty(inputCDKey.text))
		{
			IEnumerator routine = ServerAPI.UseCDKey(inputCDKey.text, OnCDKeyResponse, OnCDKeyErr);
			StartCoroutine(routine);
		}
	}

	private void OnCDKeyErr(UnityWebRequest obj)
	{
		Debug.LogError("UISetting.OnCDKeyErr -> 激活码兑换错误" + obj.error);
	}

	private void OnCDKeyResponse(Response<string[]> obj)
	{
		if (CNHCHFKLMOH.ProcessLogMagicraftServerStatue(obj.code).success)
		{
			Debug.Log("UISetting.OnCDKeyResponse -> 激活码兑换成功");
			ICJNOGPFMAM.KEMAJLGHMEL.OnPurchaseSuccess(obj.data);
		}
	}

	public override void _Close()
	{
		if (GameMgr.IsMobile_Static && panelCDKey.activeInHierarchy)
		{
			inputCDKey.text = string.Empty;
			panelCDKey.SetActive(value: false);
		}
		else
		{
			base._Close();
		}
	}

	public void CloseAll()
	{
		if (panelCDKey.activeInHierarchy)
		{
			inputCDKey.text = string.Empty;
			panelCDKey.SetActive(value: false);
		}
		base._Close();
	}
}
