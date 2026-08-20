using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : GameUI
{
	public enum MenuCategory
	{
		BiliWikiMobile,
		System,
		Status,
		HandBook,
		Gallery,
		BackMainMenu,
		QuitGame,
		Achievement
	}

	[Serializable]
	public class MenuToggleCanvas
	{
		public bool enableOnMobile;

		public bool enableOnPC;

		public CanvasGroup canvasGroup;

		public Text text;

		public Toggle toggle;

		public MenuCategory type;

		public bool Enabled
		{
			get
			{
				if (!GameMgr.IsMobile_Static)
				{
					return enableOnPC;
				}
				return enableOnMobile;
			}
			set
			{
				if (GameMgr.IsMobile_Static)
				{
					enableOnMobile = value;
				}
				else
				{
					enableOnPC = value;
				}
			}
		}
	}

	private enum loadLevelType
	{
		Finished,
		Current,
		Following
	}

	public GameObject bWikiButton;

	private MenuCategory currentCategory;

	private MenuCategory selectedCategory;

	public List<MenuToggleCanvas> menuTogglesContain;

	public UIMenuType menuType;

	public Animator anima_Menu;

	public GameObject Panel_Confirm;

	public Text text_ConfirmTitle;

	public UpdatButtonShow[] updatButtonShows;

	public RectTransform toggleMainRect;

	public HorizontalLayoutGroup toggleMainLayout;

	public float toggleMainWidth3;

	public float toggleMainWidth4;

	[Header("Gamepad")]
	public Button[] btn_Menus;

	public Button btn_Yes;

	public Button btn_No;

	public GameObject go_ConfirmSelection;

	[Header("LanguageChange")]
	public Text text_Continue;

	public Text text_Setting;

	public Text text_Back1;

	public Text text_Back2;

	public Text text_ConfirmYes;

	public Text text_ConfirmNo;

	public Text text_BugReport;

	[Header("Canvas")]
	public CanvasGroup canvasConfirm;

	[Header("MainToggle")]
	private int menuSelectedIndex;

	public GameObject UIRelic_Fade;

	public GameObject UICurse_Fade;

	[Header("system")]
	public float systemButtonBGWidthOffset = 100f;

	public RectTransform[] imageSystemBackgrounds;

	public Button ReportBug_Button;

	public Text DifficultyShow;

	public Text textTimeUseShow;

	private string timeusetext;

	private string hour;

	private string minute;

	private string second;

	private string currentDifficulty;

	[Header("Status")]
	public Vector3 uiRelicInfoPositionOffset;

	public Vector3 uiCurseInfoPositionOffset;

	public Vector3 uiRelicInfoPositionOffsetAuto;

	public Vector3 uiCurseInfoPositionOffsetAuto;

	[Header("Handbook")]
	public Vector3 dotPositionOffset;

	public Image HandbookDot;

	public UIHandbook uihandbook;

	public Custom_ScrollRect recthandbook;

	public GameObject recthandbook_content;

	private bool isBack1;

	[Header("Gallery")]
	public Image GalleryDot;

	public GameObject galleryButton;

	public UIGallery uiGallery;

	[Header("Achievement")]
	public UIAchievement uIAchievement;

	[Header("LevelShow")]
	public Transform tsfLevelShow;

	public Sprite levelShowSpriteDot1;

	public Sprite levelShowSpriteDot2;

	public Sprite levelShowConnection1;

	public Sprite levelShowConnection2;

	public Sprite levelShowPlayerMark;

	public Sprite levelShowboss;

	public Sprite levelShowElite;

	public Sprite levelShowBossH;

	public Sprite levelShowEliteH;

	public float intervel1;

	public float intervel2;

	public float sizeDot = 2f;

	public float sizeConnection = 2f;

	public float sizeBossIcon = 2f;

	public Tweener tweenerMoveY;

	private float lastLevel = -1f;

	private float lastStage = -1f;

	public float pointerSize = 1f;

	public float movetoPosition = 50f;

	public float originalPosition = 40f;

	[Header("FinishBuildShow")]
	public UIFinishBuildShow finishBuildShow;

	private EntityManager ettMgr;

	public MenuCategory CurrentCategory => currentCategory;

	private MenuToggleCanvas CurrentMenuContent => menuTogglesContain.First((MenuToggleCanvas x) => x.type == currentCategory);

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (tweenerMoveY != null)
		{
			tweenerMoveY.Kill();
		}
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(UpdateButtonShow));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += DirectPerformed;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.GamepadLB.performed += GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed += DirectPerformed;
		base.inputActions.Player.GamepadWest.performed += GamepadWestPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.GamepadLB.performed -= GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed -= GamepadRBPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.WASD.performed -= DirectPerformed;
		base.inputActions.Player.GamepadWest.performed -= GamepadWestPerformed;
	}

	private void GamepadWestPerformed(InputAction.CallbackContext obj)
	{
		if (!Panel_Confirm.activeSelf && GetMenuCanvas(MenuCategory.HandBook).canvasGroup.alpha != 1f && GameMgr.IsMobile_Static)
		{
			finishBuildShow.TryClickRelic();
		}
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(UpdateButtonShow));
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (CanControllerDir())
		{
			Vector2 direct = context.ReadValue<Vector2>();
			if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
			{
				KeyBoardDirectPerformedmove(direct);
			}
			else
			{
				DirectPerformedmove(direct);
			}
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && CanControllerDir())
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			DirectPerformedmove(vector);
		}
	}

	private void KeyBoardDirectPerformedmove(Vector2 _direct)
	{
		if (_direct == Vector2.left)
		{
			GamepadLBPerformed(default(InputAction.CallbackContext));
		}
		else if (_direct == Vector2.right)
		{
			GamepadRBPerformed(default(InputAction.CallbackContext));
		}
	}

	public MenuToggleCanvas GetMenuCanvas(MenuCategory canvaCategory)
	{
		return menuTogglesContain.First((MenuToggleCanvas x) => x.type == canvaCategory);
	}

	public void DirectPerformedmove(Vector2 _direct)
	{
		if (Panel_Confirm.activeSelf)
		{
			if (_direct == Vector2.left)
			{
				go_ConfirmSelection.transform.position = ((go_ConfirmSelection.transform.position == btn_Yes.transform.position) ? btn_No.transform.position : btn_Yes.transform.position);
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
			else if (_direct == Vector2.right && Panel_Confirm.activeSelf)
			{
				go_ConfirmSelection.transform.position = ((go_ConfirmSelection.transform.position == btn_Yes.transform.position) ? btn_No.transform.position : btn_Yes.transform.position);
				SEMgr.Inst.uiButtonHover_Button.PlaySE();
			}
		}
		else if (GetMenuCanvas(MenuCategory.HandBook).canvasGroup.alpha == 1f)
		{
			if (_direct == Vector2.down)
			{
				recthandbook_content.transform.GetChild(uihandbook.Selectindex).GetComponent<UIHandbookSlot>().OnPointerExit(null);
				for (int i = uihandbook.Selectindex + 1; i <= recthandbook_content.transform.childCount - 1; i++)
				{
					if (recthandbook_content.transform.GetChild(i).gameObject.activeSelf)
					{
						uihandbook.Selectindex = i;
						break;
					}
				}
				if (uihandbook.Selectindex < recthandbook_content.transform.childCount)
				{
					recthandbook_content.transform.GetChild(uihandbook.Selectindex).GetComponent<UIHandbookSlot>().OnPointerEnter(null);
				}
				recthandbook.ScrollUpdate(uihandbook.Selectindex);
			}
			else
			{
				if (!(_direct == Vector2.up))
				{
					return;
				}
				recthandbook_content.transform.GetChild(uihandbook.Selectindex).GetComponent<UIHandbookSlot>().OnPointerExit(null);
				for (int num = uihandbook.Selectindex - 1; num >= 0; num--)
				{
					if (recthandbook_content.transform.GetChild(num).gameObject.activeSelf)
					{
						uihandbook.Selectindex = num;
						break;
					}
				}
				recthandbook_content.transform.GetChild(uihandbook.Selectindex).GetComponent<UIHandbookSlot>().OnPointerEnter(null);
				recthandbook.ScrollUpdate(uihandbook.Selectindex, slideDirection: false);
			}
		}
		else if (GameMgr.IsMobile_Static)
		{
			finishBuildShow.movedirection_nav(_direct);
		}
		else
		{
			if (GetMenuCanvas(MenuCategory.System).canvasGroup.alpha != 1f)
			{
				return;
			}
			if (_direct == Vector2.up)
			{
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Normal");
				menuSelectedIndex--;
				if (menuSelectedIndex < 0)
				{
					menuSelectedIndex = btn_Menus.Length - 1;
				}
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
			}
			else if (_direct == Vector2.down)
			{
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Normal");
				menuSelectedIndex++;
				if (menuSelectedIndex > btn_Menus.Length - 1)
				{
					menuSelectedIndex = 0;
				}
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
			}
		}
	}

	private void GamepadLBPerformed(InputAction.CallbackContext context)
	{
		if (CanSwitchRlRb())
		{
			if (UIMgr.Inst.InputType != 0)
			{
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Normal");
			}
			menuSelectedIndex = 0;
			TogglesNavLeft();
		}
	}

	private void GamepadRBPerformed(InputAction.CallbackContext context)
	{
		if (CanSwitchRlRb())
		{
			if (UIMgr.Inst.InputType != 0)
			{
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Normal");
			}
			menuSelectedIndex = 0;
			TogglesNavRight();
		}
	}

	private void TogglesNavLeft()
	{
		if (GameMgr.IsMobile_Static)
		{
			GetMenuCanvas(selectedCategory).toggle.animator.SetTrigger("Normal");
		}
		for (int i = 1; i < menuTogglesContain.Count; i++)
		{
			int index = (CurrentNavID() + menuTogglesContain.Count - i) % menuTogglesContain.Count;
			if (menuTogglesContain[index].Enabled && menuTogglesContain[index].toggle.gameObject.activeInHierarchy)
			{
				if (GameMgr.IsMobile_Static)
				{
					selectedCategory = menuTogglesContain[index].type;
					menuTogglesContain[index].toggle.animator.SetTrigger("Highlighted");
				}
				else
				{
					menuTogglesContain[index].toggle.isOn = true;
				}
				break;
			}
		}
	}

	private int CurrentNavID()
	{
		MenuCategory currentNav = (GameMgr.IsMobile_Static ? selectedCategory : currentCategory);
		return menuTogglesContain.Select((MenuToggleCanvas item, int index) => new { item, index }).First(x => x.item.type == currentNav).index;
	}

	private void TogglesNavRight()
	{
		if (GameMgr.IsMobile_Static)
		{
			GetMenuCanvas(selectedCategory).toggle.animator.SetTrigger("Normal");
		}
		for (int i = 1; i < menuTogglesContain.Count; i++)
		{
			int index = (CurrentNavID() + menuTogglesContain.Count + i) % menuTogglesContain.Count;
			if (menuTogglesContain[index].Enabled && menuTogglesContain[index].toggle.gameObject.activeInHierarchy)
			{
				if (GameMgr.IsMobile_Static)
				{
					selectedCategory = menuTogglesContain[index].type;
					menuTogglesContain[index].toggle.animator.SetTrigger("Highlighted");
				}
				else
				{
					menuTogglesContain[index].toggle.isOn = true;
				}
				break;
			}
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || UIMgr.Inst.uiSetting.IsOpen || (finishBuildShow.IsOpen && !GameMgr.IsMobile_Static) || uiGallery.IsOpen || !base.IsOpen)
		{
			return;
		}
		if (Panel_Confirm.activeSelf)
		{
			if (go_ConfirmSelection.transform.position == btn_Yes.transform.position)
			{
				_MenuQuitYes();
			}
			else
			{
				_MenuQuitNo();
			}
		}
		else if (GameMgr.IsMobile_Static)
		{
			GetMenuCanvas(selectedCategory).toggle.isOn = true;
		}
		else
		{
			switch (menuSelectedIndex)
			{
			case 0:
				_MenuContinue();
				break;
			case 1:
				_MenuSetting();
				break;
			case 2:
				_MenuBack1();
				break;
			case 3:
				_MenuBack2();
				break;
			case 4:
				ReportBug_Button.onClick.Invoke();
				break;
			default:
				Debug.LogError(menuSelectedIndex);
				break;
			}
		}
		GameMgr.Inst.controlMgr.Lastinput = context;
	}

	private bool CanControllerDir()
	{
		if (!base.IsOpen)
		{
			return false;
		}
		if (GameMgr.IsMobile_Static && !CanSwitchRlRb())
		{
			return false;
		}
		return true;
	}

	private bool CanSwitchRlRb()
	{
		if (base.IsOpen && (UIMgr.Inst.uiSetting.IsOpen || Panel_Confirm.activeSelf))
		{
			return false;
		}
		if (GameMgr.IsMobile_Static)
		{
			if (GetMenuCanvas(MenuCategory.HandBook).canvasGroup.alpha == 0f && GetMenuCanvas(MenuCategory.Gallery).canvasGroup.alpha == 0f)
			{
				return !uIAchievement.IsOpen;
			}
			return false;
		}
		return true;
	}

	private void LanguageChange()
	{
		text_Continue.text = 1000201.GetText();
		text_Setting.text = 1000002.GetText();
		text_ConfirmYes.text = 1000208.GetText();
		text_ConfirmNo.text = 1000209.GetText();
		if (!GameMgr.IsMobile_Static)
		{
			GetMenuCanvas(MenuCategory.System).text.text = 1000221.GetText();
			GetMenuCanvas(MenuCategory.Status).text.text = 1000224.GetText();
			GetMenuCanvas(MenuCategory.HandBook).text.text = 1000222.GetText();
			GetMenuCanvas(MenuCategory.Gallery).text.text = 1000417.GetText();
			text_BugReport.text = 1000006.GetText();
		}
		switch (menuType)
		{
		case UIMenuType.Guide:
			text_Back1.text = 1000202.GetText();
			text_Back2.text = 1000203.GetText();
			break;
		case UIMenuType.Battle:
			text_Back1.text = 1000204.GetText();
			text_Back2.text = 1000202.GetText();
			break;
		default:
			Debug.LogError(menuType);
			break;
		}
		uiLevelShow();
		uiDifficultyShow();
		timeusetext = 1000239.GetText();
		hour = 1000013.GetText();
		minute = 1000014.GetText();
		second = 1000019.GetText();
		currentDifficulty = "";
		float num = 0f;
		num = ((text_Continue.preferredWidth > num) ? text_Continue.preferredWidth : num);
		num = ((text_Setting.preferredWidth > num) ? text_Setting.preferredWidth : num);
		num = ((text_Back1.preferredWidth > num) ? text_Back1.preferredWidth : num);
		num = ((text_Back2.preferredWidth > num) ? text_Back2.preferredWidth : num);
		RectTransform[] array = imageSystemBackgrounds;
		foreach (RectTransform rectTransform in array)
		{
			rectTransform.sizeDelta = new Vector2(num + systemButtonBGWidthOffset, rectTransform.sizeDelta.y);
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			btn_Menus[menuSelectedIndex].animator.SetTrigger("Normal");
			go_ConfirmSelection.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
			btn_Menus[menuSelectedIndex].GetComponent<UIButtonEvent>().SKipOnceSE();
			go_ConfirmSelection.SetActive(value: true);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		UpdateButtonShow();
	}

	private void UpdateButtonShow()
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
		LanguageChange();
		InputChange();
		if (menuType == UIMenuType.Battle)
		{
			tsfLevelShow.gameObject.SetActive(value: true);
			DifficultyShow.gameObject.SetActive(value: true);
			textTimeUseShow.gameObject.SetActive(value: true);
		}
		selectedCategory = menuTogglesContain[0].type;
		if (GameMgr.IsUseBiliOneSDK && !GameMgr.IsMobile_Static)
		{
			ReportBug_Button.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (menuType == UIMenuType.Battle)
		{
			if ((int)DataMgr.selectedWorldData.timeuse / 3600 > 0)
			{
				textTimeUseShow.text = currentDifficulty + timeusetext + ": " + (int)DataMgr.selectedWorldData.timeuse / 3600 + hour + " " + (int)(DataMgr.selectedWorldData.timeuse % 3600f) / 60 + minute + " " + (int)DataMgr.selectedWorldData.timeuse % 3600 % 60 + second;
			}
			else
			{
				textTimeUseShow.text = currentDifficulty + timeusetext + ": " + (int)(DataMgr.selectedWorldData.timeuse % 3600f) / 60 + minute + " " + (int)DataMgr.selectedWorldData.timeuse % 3600 % 60 + second;
			}
		}
	}

	public void _ToggleChange(bool isOn)
	{
		if (!isOn)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
		if (GameMgr.IsMobile_Static)
		{
			if (!CanSwitchRlRb())
			{
				HideLastCanvas();
			}
		}
		else
		{
			HideLastCanvas();
		}
		MenuToggleCanvas menuToggleCanvas = menuTogglesContain.FirstOrDefault((MenuToggleCanvas x) => (bool)x.toggle && x.toggle.isOn);
		if (menuToggleCanvas != null)
		{
			currentCategory = menuToggleCanvas.type;
			Select(currentCategory);
		}
		SEMgr.Inst.uiChangeLabel.PlaySE();
	}

	public void _MobilCloseHandBook()
	{
		if ((bool)Guide2Mgr.Inst)
		{
			Guide2Mgr.Inst.OpenedHandbook = true;
		}
		UIPlayerDataMgr.Inst.HideAllInfoPanel();
		MobileCloseMenuCategory();
		_MenuContinue();
		currentCategory = MenuCategory.BiliWikiMobile;
	}

	public void MobileOpenMenuCategory(int index)
	{
		MenuCategory category = (MenuCategory)index;
		if (menuTogglesContain.FirstOrDefault((MenuToggleCanvas x) => (bool)x.toggle && x.toggle.isOn && x.type == category) != null)
		{
			currentCategory = category;
			Select(category);
		}
	}

	public void MobileCloseMenuCategory()
	{
		HideLastCanvas();
	}

	public void MobileCloseMenu()
	{
		currentCategory = MenuCategory.BiliWikiMobile;
		_MenuContinue();
	}

	public void Select(MenuCategory category)
	{
		switch (category)
		{
		case MenuCategory.BiliWikiMobile:
			_MenuBWiki();
			break;
		case MenuCategory.System:
			if (GameMgr.IsMobile_Static)
			{
				_MenuSetting();
			}
			else
			{
				ShowSystem();
			}
			break;
		case MenuCategory.Status:
			ShowStatus();
			break;
		case MenuCategory.HandBook:
			ShowHandBook();
			break;
		case MenuCategory.Gallery:
			ShowGallery();
			break;
		case MenuCategory.BackMainMenu:
			_MenuBack1();
			break;
		case MenuCategory.QuitGame:
			_MenuBack2();
			break;
		case MenuCategory.Achievement:
			ShowAchievement();
			break;
		default:
			Debug.LogError(category);
			break;
		}
	}

	private void HideLastCanvas()
	{
		switch (currentCategory)
		{
		case MenuCategory.System:
			UIMgr.Inst.uiSetting.Hide();
			break;
		case MenuCategory.Gallery:
			uiGallery.Hide();
			break;
		case MenuCategory.Achievement:
			uIAchievement.Hide();
			break;
		default:
			Debug.LogError(currentCategory);
			break;
		case MenuCategory.BiliWikiMobile:
		case MenuCategory.Status:
		case MenuCategory.HandBook:
		case MenuCategory.BackMainMenu:
		case MenuCategory.QuitGame:
			break;
		}
		MenuToggleCanvas menuCanvas = GetMenuCanvas(currentCategory);
		if ((bool)menuCanvas.canvasGroup)
		{
			UIMgr.TryAdditionalMobileShow(menuCanvas.canvasGroup.transform);
			menuCanvas.canvasGroup.alpha = 0f;
			menuCanvas.canvasGroup.blocksRaycasts = false;
			menuCanvas.canvasGroup.interactable = false;
		}
	}

	public void ShowGallery()
	{
		ShowCanvas(MenuCategory.Gallery);
		uiGallery.ShowInitFromMenu();
	}

	private void ShowStatus()
	{
		if (!GameMgr.IsMobile_Static)
		{
			ReportBug_Button.animator.SetTrigger("Normal");
		}
		if (ControlMgr.Inst.usingpad)
		{
			btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
		}
		if (!GameMgr.IsMobile_Static)
		{
			ShowCanvas(MenuCategory.Status);
		}
		finishBuildShow.gameObject.SetActive(value: true);
		StartCoroutine(UpdateStatus(show: true));
	}

	public void ShowSystem()
	{
		if (!GameMgr.IsMobile_Static)
		{
			ReportBug_Button.animator.SetTrigger("Normal");
		}
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
		}
		ShowCanvas(MenuCategory.System);
		uiLevelShow();
		uiDifficultyShow();
		if (GameMgr.IsMobile_Static)
		{
			ShowStatus();
		}
		else if (finishBuildShow.IsOpen)
		{
			finishBuildShow.HideImmediate();
		}
	}

	private void ShowHandBook()
	{
		currentCategory = MenuCategory.HandBook;
		ShowCanvas(MenuCategory.HandBook);
		finishBuildShow.HideImmediate();
		if (!DataMgr.selectedWorldData.OpenHandbookOnce)
		{
			DataMgr.selectedWorldData.OpenHandbookOnce = true;
			HandbookDot.enabled = false;
		}
		if (!GameMgr.IsMobile_Static && ControlMgr.Inst.InputType != 0)
		{
			recthandbook_content.transform.GetChild(uihandbook.Selectindex).GetComponent<UIHandbookSlot>().OnPointerEnter(null);
		}
	}

	private void ShowAchievement()
	{
		uIAchievement.Show();
	}

	public void ShowHandBookMobile(int id)
	{
		ShowUIMenuHandbook();
		DOTween.Sequence().AppendInterval(0.2f).OnComplete(delegate
		{
			uihandbook.ShowAndSlideToCenter(id);
		})
			.SetUpdate(isIndependentUpdate: true);
	}

	private void ShowCanvas(MenuCategory category)
	{
		MenuToggleCanvas menuCanvas = GetMenuCanvas(category);
		if ((bool)menuCanvas.canvasGroup)
		{
			menuCanvas.canvasGroup.alpha = 1f;
			menuCanvas.canvasGroup.blocksRaycasts = true;
			menuCanvas.canvasGroup.interactable = true;
			UIMgr.TryAdditionalMobileShow(menuCanvas.canvasGroup.transform);
		}
	}

	private IEnumerator UpdateStatus(bool show = false)
	{
		yield return new WaitForEndOfFrame();
		if (show)
		{
			finishBuildShow.Show2();
		}
		FinishGameBuild finishGameBuild = DataMgr.WorlddataToBuildData(DataMgr.selectedWorldData);
		if (menuType == UIMenuType.Guide)
		{
			finishGameBuild.timeuse = -1f;
		}
		finishBuildShow.UpdateBuildInfoFinishBattle(finishGameBuild, UIFinishBuildShow.RecordUIFrom.Menu, -1);
	}

	public void ShowUIMenu()
	{
		if (GameMgr.IsMobile_Static && ControlMgr.Inst.usingpad)
		{
			currentCategory = MenuCategory.BiliWikiMobile;
		}
		else
		{
			currentCategory = MenuCategory.System;
		}
		Show();
	}

	public void ShowUIMenuHandbook()
	{
		currentCategory = MenuCategory.HandBook;
		Select(currentCategory);
		Show();
	}

	protected override void OnShow(object obj = null)
	{
		if (GameMgr.IsMobile_Static && ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK)
		{
			bWikiButton.SetActive(PluginActivity.channleID == PluginActivity.ChannleID.B服.ChannleID());
		}
		UIMgr.TryAdditionalMobileShow(base.transform, 3);
		float num;
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Gallery) != 0)
		{
			toggleMainRect.sizeDelta = new Vector2(toggleMainWidth4, toggleMainRect.sizeDelta.y);
			GetMenuCanvas(MenuCategory.Gallery).Enabled = true;
			galleryButton.gameObject.SetActive(value: true);
			num = (toggleMainRect.sizeDelta.x - toggleMainLayout.spacing * 3f) / 4f - 40f;
		}
		else
		{
			toggleMainRect.sizeDelta = new Vector2(toggleMainWidth3, toggleMainRect.sizeDelta.y);
			galleryButton.gameObject.SetActive(value: false);
			GetMenuCanvas(MenuCategory.Gallery).Enabled = false;
			num = (toggleMainRect.sizeDelta.x - toggleMainLayout.spacing * 2f) / 3f - 40f;
		}
		UIMgr.Inst.MoveUpHoverLayer();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima_Menu.SetTrigger("Show");
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		if (!GameMgr.IsMobile_Static || !UIPlayerDataMgr.Inst.IsBagOpen)
		{
			TimeScaleMgr.Inst.Pause();
		}
		if (ControlMgr.Inst.usingpad)
		{
			if (GameMgr.IsMobile_Static)
			{
				GetMenuCanvas(selectedCategory).toggle.animator.SetTrigger("Highlighted");
			}
			else
			{
				btn_Menus[menuSelectedIndex].animator.SetTrigger("Highlighted");
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			ShowSystem();
			UIPlayerDataMgr.Inst.LowerWandEventOrder();
		}
		else
		{
			_ToggleChange(isOn: true);
		}
		if (DataMgr.selectedWorldData.MenuGalleryDot)
		{
			GalleryDot.enabled = true;
			float preferredWidth = GetMenuCanvas(MenuCategory.Gallery).text.preferredWidth;
			if (!GameMgr.IsMobile_Static)
			{
				GalleryDot.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(((preferredWidth < num) ? preferredWidth : num) / 2f, 0f, 0f) + dotPositionOffset;
			}
		}
		else
		{
			GalleryDot.enabled = false;
		}
		if (!GameMgr.IsMobile_Static)
		{
			if (!DataMgr.selectedWorldData.OpenHandbookOnce)
			{
				HandbookDot.enabled = true;
				float preferredWidth2 = GetMenuCanvas(MenuCategory.HandBook).text.preferredWidth;
				HandbookDot.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(((preferredWidth2 < num) ? preferredWidth2 : num) / 2f, 0f, 0f) + dotPositionOffset;
			}
			else
			{
				HandbookDot.enabled = false;
			}
		}
	}

	private void uiLevelShow()
	{
		if (tsfLevelShow.gameObject.activeSelf && BattleMgr.Inst != null && DataMgr.selectedWorldData.battleData9 != null && DataMgr.selectedWorldData.battleData9.currentStage <= BattleMgr.Inst.stageLevelsCount.Length)
		{
			if (lastLevel != (float)DataMgr.selectedWorldData.battleData9.currentLevel || lastStage != (float)DataMgr.selectedWorldData.battleData9.currentStage)
			{
				lastLevel = DataMgr.selectedWorldData.battleData9.currentLevel;
				lastStage = DataMgr.selectedWorldData.battleData9.currentStage;
				UpdateLevelShow(DataMgr.selectedWorldData.battleData9.currentLevel, DataMgr.selectedWorldData.battleData9.currentStage);
			}
			else
			{
				tweenerMoveY?.Restart();
			}
		}
	}

	private void UpdateLevelShow(int currentLevel, int stage)
	{
		int num = BattleMgr.Inst.stageLevelsCount[stage - 1];
		int count = -1;
		tsfLevelShow.DestroyAllChild();
		if (currentLevel == 0)
		{
			loadLevel(loadLevelType.Current);
			for (int i = 1; i < num; i++)
			{
				count++;
				loadLevel(loadLevelType.Following, LoadPoint: true, isElite(stage, i));
			}
			LoadBossOrElite();
		}
		else if (currentLevel != num)
		{
			if (stage % 2 == 1)
			{
				loadLevel(loadLevelType.Finished);
			}
			for (int j = 1; j < currentLevel; j++)
			{
				count++;
				loadLevel(loadLevelType.Finished, LoadPoint: true, isElite(stage, j));
			}
			loadLevel(loadLevelType.Current, LoadPoint: true, isElite(stage, currentLevel));
			for (int k = currentLevel + 1; k < num; k++)
			{
				count++;
				loadLevel(loadLevelType.Following, LoadPoint: true, isElite(stage, k));
			}
			LoadBossOrElite();
		}
		else
		{
			if (stage % 2 == 1)
			{
				loadLevel(loadLevelType.Finished);
			}
			for (int l = 1; l < currentLevel; l++)
			{
				count++;
				loadLevel(loadLevelType.Finished, LoadPoint: true, isElite(stage, l));
			}
			LoadBossOrElite(bossAnimate: true);
		}
		void LoadBossOrElite(bool bossAnimate = false)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = tsfLevelShow;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = levelShowSpriteDot1;
			image.color = Color.clear;
			image.SetNativeSize();
			GameObject gameObject2 = new GameObject();
			gameObject2.transform.parent = gameObject.transform;
			Image image2;
			if (DataMgr.selectedWorldData.battleData9.currentStage % 2 == 1)
			{
				image2 = gameObject2.AddComponent<Image>();
				image2.sprite = (GameMgr.IsHarmony_Static ? levelShowEliteH : levelShowElite);
			}
			else
			{
				image2 = gameObject2.AddComponent<Image>();
				image2.sprite = (GameMgr.IsHarmony_Static ? levelShowBossH : levelShowboss);
			}
			image2.SetNativeSize();
			gameObject2.transform.localScale = new Vector3(sizeBossIcon, sizeBossIcon, sizeBossIcon);
			gameObject2.transform.localPosition = new Vector3(2f, 0f, 0f);
			if (bossAnimate)
			{
				GameObject gameObject3 = new GameObject();
				gameObject3.transform.parent = gameObject.transform;
				Image image3 = gameObject3.AddComponent<Image>();
				image3.sprite = levelShowPlayerMark;
				image3.SetNativeSize();
				gameObject3.transform.localScale = new Vector3(pointerSize, pointerSize, pointerSize);
				gameObject3.transform.localPosition = new Vector3(3f, originalPosition, 0f);
				if (tweenerMoveY == null)
				{
					tweenerMoveY = gameObject3.transform.DOLocalMoveY(movetoPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
				}
				else
				{
					tweenerMoveY.Kill();
					tweenerMoveY = gameObject3.transform.DOLocalMoveY(movetoPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
				}
			}
		}
		static bool isElite(int stage, int level)
		{
			foreach (BattleMgr.overrideElite item in BattleMgr.Inst.overrideElitesLevelShow)
			{
				if (item.stage == stage && item.level == level)
				{
					return true;
				}
			}
			return false;
		}
		void loadLevel(loadLevelType loadLevelType, bool LoadPoint = true, bool forceElite = false)
		{
			GameObject gameObject4 = new GameObject();
			gameObject4.transform.parent = tsfLevelShow;
			gameObject4.transform.localScale = new Vector3(sizeDot, sizeDot, sizeDot);
			Image image4 = gameObject4.AddComponent<Image>();
			image4.sprite = levelShowSpriteDot1;
			image4.color = Color.clear;
			image4.SetNativeSize();
			GameObject gameObject5 = new GameObject();
			gameObject5.transform.parent = gameObject4.transform;
			gameObject5.transform.localScale = new Vector3(sizeDot, sizeDot, sizeDot);
			GameObject gameObject6 = new GameObject();
			gameObject6.transform.parent = gameObject4.transform;
			gameObject6.transform.localScale = new Vector3(sizeConnection, sizeConnection, sizeConnection);
			Image image5 = gameObject5.AddComponent<Image>();
			Image image6 = gameObject6.AddComponent<Image>();
			switch (loadLevelType)
			{
			case loadLevelType.Finished:
				image5.sprite = levelShowSpriteDot1;
				image6.sprite = levelShowConnection1;
				break;
			case loadLevelType.Current:
			{
				if (!LoadPoint)
				{
					image5.sprite = levelShowSpriteDot1;
					image5.color = Color.clear;
				}
				else
				{
					image5.sprite = levelShowSpriteDot1;
				}
				image6.sprite = levelShowConnection2;
				GameObject gameObject7 = new GameObject();
				gameObject7.transform.parent = gameObject4.transform;
				Image image7 = gameObject7.AddComponent<Image>();
				image7.sprite = levelShowPlayerMark;
				image7.SetNativeSize();
				gameObject7.transform.localScale = new Vector3(pointerSize, pointerSize, pointerSize);
				gameObject7.transform.localPosition = new Vector3(0f, originalPosition, 0f);
				if (tweenerMoveY == null)
				{
					tweenerMoveY = gameObject7.transform.DOLocalMoveY(movetoPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
				}
				else
				{
					tweenerMoveY.Kill();
					tweenerMoveY = gameObject7.transform.DOLocalMoveY(movetoPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
				}
				break;
			}
			case loadLevelType.Following:
				image5.sprite = levelShowSpriteDot2;
				image6.sprite = levelShowConnection2;
				break;
			}
			if (forceElite)
			{
				image5.sprite = (GameMgr.IsHarmony_Static ? levelShowEliteH : levelShowElite);
			}
			image5.SetNativeSize();
			image6.SetNativeSize();
			gameObject4.transform.localPosition = new Vector3((float)count * intervel1, 0f, 0f);
			gameObject6.transform.localPosition = new Vector3(intervel2, 0f, 0f);
		}
	}

	private void uiDifficultyShow()
	{
		if (tsfLevelShow.gameObject.activeSelf && (bool)BattleMgr.Inst)
		{
			switch (DataMgr.selectedWorldData.selectedDifficulty)
			{
			case DifficultyType.Easy:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002601.GetText();
				break;
			case DifficultyType.Normal:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002602.GetText();
				break;
			case DifficultyType.Hard:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002603.GetText();
				break;
			case DifficultyType.Nightmare1:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002605.GetText();
				break;
			case DifficultyType.Nightmare2:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002606.GetText();
				break;
			case DifficultyType.Nightmare3:
				DifficultyShow.text = 1003203.GetText() + ": " + 1002607.GetText();
				break;
			}
		}
	}

	protected override void OnHide()
	{
		uihandbook.ClearMp4GameObject();
		UIMgr.Inst.MoveDownHoverLayer();
		finishBuildShow.HideImmediate();
		uiGallery.Hide();
		UIPlayerDataMgr.Inst.HideAllInfoPanel();
		UIPlayerDataMgr.Inst.RelicUpdateAppearanceIcon();
		anima_Menu.SetTrigger("Hide");
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		if (!GameMgr.IsMobile_Static || !UIPlayerDataMgr.Inst.IsBagOpen)
		{
			TimeScaleMgr.Inst.Recovery();
		}
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.RaiseWandEventOrder();
		}
		if (tweenerMoveY != null)
		{
			tweenerMoveY.Pause();
		}
	}

	public void HandBookClickFold(int selectindex)
	{
		UIHandbookSlot uIHandbookSlot = uihandbook.Slots[selectindex];
		if (uIHandbookSlot.HandbookCfg != null)
		{
			return;
		}
		SEMgr.Inst.uiClick.PlaySE();
		if (!uIHandbookSlot.fold)
		{
			uIHandbookSlot.fold = true;
			uIHandbookSlot.text.color = uihandbook.colorHandbookSlotWhenFold;
			uIHandbookSlot.Rotate_Fold_Arrow_Right();
			for (int i = selectindex + 1; i < uihandbook.Slots.Count && uihandbook.Slots[i].HandbookCfg != null; i++)
			{
				uihandbook.Slots[i].gameObject.SetActive(value: false);
			}
			recthandbook.ScrollCountActive();
		}
		else
		{
			uIHandbookSlot.fold = false;
			uIHandbookSlot.text.color = uihandbook.colorHandbookSlotWhenExpand;
			uIHandbookSlot.Rotate_Fold_Arrow_Down();
			for (int j = selectindex + 1; j < uihandbook.Slots.Count && uihandbook.Slots[j].HandbookCfg != null; j++)
			{
				uihandbook.Slots[j].gameObject.SetActive(value: true);
			}
			recthandbook.ScrollCountActive();
		}
	}

	public void _BackGroundClick()
	{
		Debug.Log("_BackGroundClick");
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
	}

	public void _MenuContinue()
	{
		Hide();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MenuSetting()
	{
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
		UIMgr.Inst.uiSetting.Show();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MenuBWiki()
	{
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
		Application.OpenURL("https://wiki.biligame.com/magicraft/%E9%A6%96%E9%A1%B5");
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MenuBack1()
	{
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
		isBack1 = true;
		switch (menuType)
		{
		case UIMenuType.Guide:
			text_ConfirmTitle.text = 1000205.GetText();
			break;
		case UIMenuType.Battle:
			text_ConfirmTitle.text = 1000210.GetText();
			break;
		default:
			Debug.LogError(menuType);
			break;
		}
		Panel_Confirm.SetActive(value: true);
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _MenuBack2()
	{
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.HideAllInfoPanel();
		}
		isBack1 = false;
		SEMgr.Inst.uiClick.PlaySE();
		if (!isBack1 && GameMgr.IsMobile_Static && ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK && menuType == UIMenuType.Guide)
		{
			DataMgr.SaveSelectedWorldData();
			DataMgr.SaveWorldDataBackup();
			PluginActivity.Inst.SDKQuitGame();
			return;
		}
		switch (menuType)
		{
		case UIMenuType.Guide:
			text_ConfirmTitle.text = 1000206.GetText();
			break;
		case UIMenuType.Battle:
			if (DataMgr.selectedWorldData.battleData9.currentStage == 1 && DataMgr.selectedWorldData.battleData9.currentLevel <= 2)
			{
				text_ConfirmTitle.text = 1000205.GetText();
			}
			else
			{
				text_ConfirmTitle.text = 1000211.GetText();
			}
			break;
		default:
			Debug.LogError(menuType);
			break;
		}
		Panel_Confirm.SetActive(value: true);
	}

	public void _MenuQuitYes()
	{
		if (!canvasConfirm.interactable)
		{
			Debug.Log("Return");
			return;
		}
		SEMgr.Inst.uiClick.PlaySE();
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SpellDashDriverSingleton));
		if (!entityQuery.IsEmpty)
		{
			Entity singletonEntity = entityQuery.GetSingletonEntity();
			SpellDashDriverSingleton componentData = ettMgr.GetComponentData<SpellDashDriverSingleton>(singletonEntity);
			if (componentData.IsShooterDriving(PlayerMgr.Inst.PlayerEtt))
			{
				componentData.DashRemainingTime = 0f;
				componentData.ShooterDriveEnd(PlayerMgr.Inst.PlayerEtt);
				UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
				componentData2.FlyUnregister();
				componentData2.InvincibleUnregister();
				ettMgr.SetComponentData(singletonEntity, componentData);
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData2);
				CamController.Inst.playerInRotateDash = false;
				PlayerMgr.Inst.PlayerCtrller.OnPlayerDashEnd();
			}
		}
		if (isBack1 && menuType == UIMenuType.Battle)
		{
			Panel_Confirm.SetActive(value: false);
			Hide();
			TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.FromUI);
			elem.damage = 1f;
			elem.ignoreFloatText = true;
			ettMgr.GetBuffer<TakeDamageInfo_Dots>(PlayerMgr.Inst.PlayerEtt).Add(elem);
			return;
		}
		if (!isBack1 && ScriptableObjMgr.Inst.testCtrller.isBW && menuType == UIMenuType.Battle)
		{
			Panel_Confirm.SetActive(value: false);
			Hide();
			TakeDamageInfo_Dots elem2 = TakeDamageInfo_Dots.NewInfo(AttackerType.FromUI);
			elem2.damage = 1f;
			elem2.ignoreFloatText = true;
			ettMgr.GetBuffer<TakeDamageInfo_Dots>(PlayerMgr.Inst.PlayerEtt).Add(elem2);
			return;
		}
		canvasConfirm.interactable = false;
		UIMgr.Inst.uiFade.Show(delegate
		{
			canvasConfirm.interactable = true;
			GameMgr.Inst.DestroyAllTeammate();
			GameMgr.Inst.ClearAllPool();
			GameMgr.Inst.AllFunctionReset();
			if (isBack1)
			{
				switch (menuType)
				{
				case UIMenuType.Guide:
					if (Guide2Mgr.Inst != null)
					{
						Guide2Mgr.Inst.OnExitGuide2ToMainMenu();
					}
					TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
					SceneManager.LoadScene("MainMenu");
					UISetting.SetCursorWand();
					break;
				default:
					Debug.LogError(menuType);
					break;
				case UIMenuType.Battle:
					break;
				}
			}
			else
			{
				switch (menuType)
				{
				case UIMenuType.Guide:
					DataMgr.SaveSelectedWorldData();
					DataMgr.SaveWorldDataBackup();
					GameMgr.QuitGame();
					break;
				case UIMenuType.Battle:
					TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
					SceneManager.LoadScene("MainMenu");
					break;
				default:
					Debug.LogError(menuType);
					break;
				}
			}
		});
	}

	public void _MenuQuitNo()
	{
		Panel_Confirm.SetActive(value: false);
		if (GameMgr.IsMobile_Static && (currentCategory == MenuCategory.QuitGame || currentCategory == MenuCategory.BackMainMenu))
		{
			HideLastCanvas();
		}
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _BugReport()
	{
		if (DataMgr.settingData.language == LanguageType.ChineseS)
		{
			Application.OpenURL("https://docs.qq.com/sheet/DVmVic2N6VkRPZmpD?tab=BB08J2");
		}
		else
		{
			Application.OpenURL("https://discord.gg/9TQTkH8pmj");
		}
	}

	public void _ManualToggle(int i)
	{
		if (i == -1)
		{
			menuTogglesContain.ForEach(delegate(MenuToggleCanvas toggle)
			{
				if (toggle.toggle != null)
				{
					toggle.toggle.isOn = false;
				}
			});
			return;
		}
		menuTogglesContain.ForEach(delegate(MenuToggleCanvas toggle)
		{
			if (toggle.toggle != null)
			{
				toggle.toggle.isOn = toggle == menuTogglesContain[i];
			}
		});
	}

	public void _CloseHandBook()
	{
		if ((bool)Guide2Mgr.Inst)
		{
			Guide2Mgr.Inst.OpenedHandbook = true;
		}
		_Close();
	}
}
