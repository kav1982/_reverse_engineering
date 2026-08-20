using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MobileMgr : MonoBehaviour
{
	public enum ScreenType
	{
		Wide,
		Normal,
		Long
	}

	public enum InteractState
	{
		Other,
		Talk,
		EnterDoor,
		Shoot
	}

	private bool skillDirCache;

	public bool isUsingPetrifaction;

	public bool Generate_CampPlants;

	public static MobileMgr inst;

	public List<CanvasScaler> CanvasScalers;

	public int scalerhight;

	public int scalerwidth;

	public Vector2 ScreenRes;

	public List<float> FocusCamWideInBattle;

	public float FocusCamLong;

	public float FocusCamLongGuide2 = 0.72f;

	public List<float> FocusCamLongInBattle;

	public float FocusCamSteamdeck;

	public float uiBagZoominMaxMobile = 1.8f;

	public float uiBagZoominMaxPC = 1f;

	public int mobileControlDontUpdateCounter;

	private EntityManager ettMgr;

	private bool gamepadPluggedLastFrame;

	private static string _preStr;

	public TopUI topui { get; set; }

	public PluginActivity PluginActivity => PluginActivity.Inst;

	public float screenRatio { get; set; }

	public float screenRatioAdjust => screenRatio / 1.77f;

	public float uiLeftUpZoomout => 0.85f;

	public ScreenType screenType { get; set; }

	public float uiLeftUpZoominMax
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return 1f;
			}
			switch (UIPlayerDataMgr.Inst.uiWands.Count)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				return 1.8f;
			case 5:
				return 1.415f;
			case 6:
				return 1.19f;
			default:
				return 1f;
			}
		}
	}

	public bool cameraFocusWidthFirst { get; private set; }

	public bool gamepadPlugged => GetRealGamepad() != null;

	public static Gamepad GetRealGamepad()
	{
		return Gamepad.all.FirstOrDefault((Gamepad g) => !IsFakeGamepad(g));
	}

	public static bool IsFakeGamepad(Gamepad g)
	{
		if (g == null)
		{
			return true;
		}
		if (string.IsNullOrEmpty(g.description.capabilities))
		{
			return true;
		}
		string text = (g.name + " " + g.displayName + " " + g.layout + " " + g.description.interfaceName + " " + g.description.product + " " + g.description.manufacturer + " " + g.description.capabilities).ToLowerInvariant();
		if (_preStr != text)
		{
			_preStr = text;
			Debug.Log(text);
		}
		if (text.Contains("onscreen") || text.Contains("on-screen"))
		{
			return true;
		}
		if (text.Contains("uinput") || text.Contains("xiaomi") || text.Contains("miui") || text.Contains("virtual"))
		{
			return true;
		}
		return false;
	}

	public void Initialize()
	{
		if (inst == null)
		{
			inst = this;
			ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		cameraFocusWidthFirst = true;
		UpdateScreenRatio();
	}

	public void UpdateScreenRatio()
	{
		if (GameMgr.IsMobile_Static)
		{
			float num = (float)Screen.width / (float)Screen.height;
			if (!(num <= 1.5f))
			{
				if (num <= 1.7777778f)
				{
					screenType = ScreenType.Normal;
					scalerwidth = 1600;
					scalerhight = 815;
				}
				else
				{
					screenType = ScreenType.Long;
					scalerwidth = 1600;
					scalerhight = 700;
				}
			}
			else
			{
				screenType = ScreenType.Wide;
				scalerwidth = 1600;
				scalerhight = 1000;
			}
		}
		else if (GameMgr.IsSteamDeck_Static)
		{
			scalerwidth = 1600;
			scalerhight = 900;
			CamController.Inst.FocusCamSizeRatio = FocusCamSteamdeck;
		}
		else
		{
			scalerwidth = 1920;
			scalerhight = 1080;
		}
		foreach (CanvasScaler canvasScaler in CanvasScalers)
		{
			canvasScaler.referenceResolution = ((GameMgr.IsSteamDeck_Static || GameMgr.IsMobile_Static) ? new Vector2(scalerwidth, scalerhight) : new Vector2(1920f, 1080f));
		}
		inst.ScreenRes = new Vector2Int(Screen.width, Screen.height);
	}

	private void Update()
	{
		gamepadPluggedLastFrame = Gamepad.current != null;
		screenRatio = (float)Screen.width / (float)Screen.height;
		if (mobileControlDontUpdateCounter == 0)
		{
			if (gamepadPlugged)
			{
				HideControl();
			}
			else if (isUsingPetrifaction || ((bool)PlayerMgr.Inst.PlayerCtrller && PlayerMgr.Inst.PlayerCtrller.InAbyss))
			{
				if ((bool)PlayerMgr.Inst.PlayerCtrller && PlayerMgr.Inst.PlayerCtrller.unmovableCounter == 1)
				{
					ShowControl();
				}
				else
				{
					HideControl();
				}
			}
			else if (PlayerMgr.Inst.PlayerCtrller != null && PlayerMgr.Inst.PlayerCtrller.CanMotion)
			{
				ShowControl();
			}
			else
			{
				HideControl();
			}
		}
		UpdateCameraDistance();
		gamepadPluggedLastFrame = gamepadPlugged;
	}

	public void ActiveButtonInteract(InteractState interactState)
	{
		UpdateActiveButton(interactState);
		topui.goRightStick.GetComponent<CanvasGroup>().alpha = 1f;
		topui.goRightStick.GetComponent<CanvasGroup>().blocksRaycasts = true;
		inst.MobileUpdateInteractButtonShow();
	}

	public void UpdateActiveButton(InteractState interactState)
	{
		if (!DataMgr.settingData.Mobiledata.indieInteractButton && (bool)PlayerMgr.Inst.PlayerCtrller && (PlayerMgr.Inst.PlayerCtrller.isHoldMouse0 || !PlayerMgr.Inst.PlayerCtrller.CanMotion || !PlayerMgr.Inst.PlayerCtrller.CanInteractive))
		{
			return;
		}
		Image image = (DataMgr.settingData.Mobiledata.indieInteractButton ? topui.interactImage : topui.attackImage);
		switch (interactState)
		{
		case InteractState.Other:
			image.sprite = topui.spriteOther;
			break;
		case InteractState.Talk:
			image.sprite = topui.spriteTalk;
			break;
		case InteractState.EnterDoor:
			image.sprite = topui.spriteEnderDoor;
			break;
		case InteractState.Shoot:
			if ((bool)GuideMgr.Inst && !GuideMgr.Inst.IsPickedWand)
			{
				return;
			}
			image.sprite = topui.spriteAttack;
			break;
		default:
			image.sprite = topui.spriteAttack;
			break;
		}
		if (DataMgr.settingData.Mobiledata.indieInteractButton)
		{
			TopUI.inst.attackImage.sprite = TopUI.inst.spriteAttack;
		}
	}

	public void MobileUpdateInteractButtonShow()
	{
		CanvasGroup component = topui.goRightStick.GetComponent<CanvasGroup>();
		if (TopUI.inst.adjusting)
		{
			component.alpha = 1f;
			component.blocksRaycasts = true;
		}
		else if ((bool)GuideMgr.Inst && !GuideMgr.Inst.IsPickedWand && (bool)PlayerMgr.Inst)
		{
			if (DataMgr.settingData.Mobiledata.indieInteractButton)
			{
				component.alpha = 0f;
				component.blocksRaycasts = false;
				topui.goIndiActiveButton.SetActive(PlayerMgr.Inst.PlayerCtrller.GetInteractCount() > 0);
				return;
			}
			topui.goIndiActiveButton.SetActive(value: false);
			if (PlayerMgr.Inst.PlayerCtrller.GetInteractCount() <= 0)
			{
				component.alpha = 0f;
				component.blocksRaycasts = false;
			}
		}
		else if (PlayerMgr.Inst != null && PlayerMgr.Inst.PlayerCtrller != null)
		{
			TopUI.inst.goIndiActiveButton.SetActive(DataMgr.settingData.Mobiledata.indieInteractButton);
			component.alpha = 1f;
			component.blocksRaycasts = true;
			if (PlayerMgr.Inst.PlayerCtrller.GetInteractCount() <= 0)
			{
				InteractButtonHide();
			}
			else
			{
				InteractButtonShow();
			}
		}
		else
		{
			topui.goIndiActiveButton.SetActive(value: false);
			component.alpha = 0f;
			component.blocksRaycasts = false;
		}
	}

	private void InteractButtonShow()
	{
		if (DataMgr.settingData.Mobiledata.indieInteractButton)
		{
			topui.goIndiActiveButton.SetActive(value: true);
		}
	}

	private void InteractButtonHide()
	{
		if (DataMgr.settingData.Mobiledata.indieInteractButton)
		{
			topui.goIndiActiveButton.SetActive(value: false);
		}
	}

	public void UpdateMobileButtons()
	{
		UpdateDrinkButton();
		UpdateChangeWandButton();
		UpdateActiveSkillButton();
	}

	public void UpdateDrinkButton()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (PlayerMgr.Inst.GetPotionNum() == 0)
			{
				InactiveButtonDrink();
			}
			else if (PlayerMgr.Inst.GetPotionNum() >= 0)
			{
				ActiveButtonDrink();
			}
			else
			{
				Debug.LogError("药水数量小于0?");
			}
		}
	}

	public void UpdateChangeWandButton()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (DataMgr.selectedWorldData.battleData9 == null || TopUI.inst.adjusting)
			{
				ActiveButtonSwitchWand();
			}
			else if (PlayerMgr.Inst.BaData.wandCfgs.Count == 1)
			{
				InactiveButtonSwitchWand();
			}
			else if (PlayerMgr.Inst.BaData.wandCfgs.Count > 1)
			{
				ActiveButtonSwitchWand();
			}
			else
			{
				Debug.LogError("法杖数量小于0?");
			}
		}
	}

	public void UpdateActiveSkillButton()
	{
		if ((bool)MainMenuMgr.Inst)
		{
			InactiveButtonSkill();
		}
		if (DataMgr.selectedWorldData.battleData9 == null || TopUI.inst.adjusting)
		{
			topui.skillCDImageWithDir.gameObject.SetActive(value: false);
			topui.skillCDImage.gameObject.SetActive(value: false);
			InactiveButtonSkill();
			return;
		}
		(ActiveSkillType, bool) tuple = DataMgr.selectedWorldData.HaveSkillRelic();
		if (tuple.Item2 != skillDirCache)
		{
			skillDirCache = tuple.Item2;
			TopUI.inst.aimSkillObj1.SetActive(!skillDirCache);
			TopUI.inst.aimSkillObj2.SetActive(skillDirCache);
		}
		int item = (int)tuple.Item1;
		if (item != -1)
		{
			topui.activeSkillImage.sprite = topui.skillSprites[item];
			topui.aimActiveSkillImage.sprite = topui.skillSprites[item];
			topui.skillEfImage.sprite = topui.skillSprites[item];
			topui.aimSkillEfImage.sprite = topui.skillSprites[item];
			topui.skillCDImageWithDir.gameObject.SetActive(tuple.Item2);
			topui.skillCDImage.gameObject.SetActive(!tuple.Item2);
			topui.skillCDImage.fillAmount = 0f;
			ActiveButtonSkill();
		}
		else
		{
			topui.skillCDImageWithDir.gameObject.SetActive(value: false);
			topui.skillCDImage.gameObject.SetActive(value: false);
			InactiveButtonSkill();
		}
	}

	public void UpdateSkillCD(float value, string skillCountShow = "", bool interactable = false)
	{
		if (topui.skillCDImage.gameObject.activeInHierarchy)
		{
			topui.skillCDImage.fillAmount = value;
		}
		if (topui.skillCDImageWithDir.gameObject.activeInHierarchy || topui.uI_AimSkill.gameObject.activeInHierarchy)
		{
			topui.skillCDImageWithDir.fillAmount = value;
		}
		topui.skillButton.interactable = interactable;
		topui.skillCount.text = skillCountShow;
		topui.aimSkillCount.text = skillCountShow;
	}

	public void SkillPunch()
	{
		if (GameMgr.IsMobile_Static)
		{
			TopUI.inst.Button_ActiveSkill_Animator.Play("InteractButtonPunch");
			SkillImagePunch(topui.uI_AimSkill.gameObject.activeInHierarchy ? topui.aimSkillEfImage : topui.skillEfImage);
			topui.skillButton.interactable = true;
		}
	}

	public void SkillImagePunch(Image skillImage)
	{
		Color color = topui.skillEfImage.color;
		color.a = 1f;
		skillImage.gameObject.SetActive(value: true);
		skillImage.color = color;
		skillImage.DOFade(0f, 0.7f).SetUpdate(isIndependentUpdate: true).OnComplete(delegate
		{
			topui.skillEfImage.gameObject.SetActive(value: false);
		});
		skillImage.transform.DOScale(new Vector3(3f, 3f, 3f), 0.7f).SetUpdate(isIndependentUpdate: true);
		skillImage.transform.localScale = Vector3.one;
	}

	public void ActiveButtonSkill()
	{
		if (!topui.Button_ActiveSkill_Animator.GetBool("Show"))
		{
			topui.Button_ActiveSkill_Animator.SetBool("Show", value: true);
		}
	}

	private void InactiveButtonSkill()
	{
		if (topui.Button_ActiveSkill_Animator.GetBool("Show"))
		{
			topui.Button_ActiveSkill_Animator.SetBool("Show", value: false);
		}
	}

	public void ActiveButtonDrink()
	{
		if (!topui.Button_Drink_Animator.GetCurrentAnimatorStateInfo(0).IsName("InteractButtonShow"))
		{
			topui.Button_Drink_Animator.SetBool("Show", value: true);
		}
	}

	private void InactiveButtonDrink()
	{
		topui.Button_Drink_Animator.SetBool("Show", value: false);
	}

	public void ActiveButtonSwitchWand()
	{
		if (!topui.Button_SwitchWandAnimator.GetCurrentAnimatorStateInfo(0).IsName("InteractButtonShow"))
		{
			topui.Button_SwitchWandAnimator.SetBool("Show", value: true);
		}
	}

	private void InactiveButtonSwitchWand()
	{
		topui.Button_SwitchWandAnimator.SetBool("Show", value: false);
	}

	public void VirtualStickSet(int i)
	{
		foreach (GameObject item in topui.VirtualStick)
		{
			item.SetActive(value: false);
		}
		topui.VirtualStick[i].SetActive(value: true);
		DataMgr.settingData.VirtualStickType = i;
	}

	public void HideControl()
	{
		if (topui.ControlAnimator.GetBool("Show"))
		{
			CustomStickArea[] customStickAreas = topui.customStickAreas;
			for (int i = 0; i < customStickAreas.Length; i++)
			{
				customStickAreas[i].ForcePointerUp();
			}
		}
		topui.ControlAnimator.SetBool("Show", value: false);
		UIPlayerDataMgr.Inst.OpenBagButton.SetActive(value: false);
	}

	public void ShowControl()
	{
		UIPlayerDataMgr.Inst.OpenBagButton.SetActive(value: true);
		UpdateDrinkButton();
		UpdateChangeWandButton();
		if (gamepadPlugged)
		{
			return;
		}
		if (!topui.ControlAnimator.GetBool("Show"))
		{
			CustomStickArea[] customStickAreas = topui.customStickAreas;
			for (int i = 0; i < customStickAreas.Length; i++)
			{
				customStickAreas[i].ForcePointerUp();
			}
		}
		topui.ControlAnimator.SetBool("Show", value: true);
	}

	public void UpdateCameraDistance()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		float focusCamSizeRatio = CamController.Inst.FocusCamSizeRatio;
		int num = -1;
		bool flag = false;
		if (DataMgr.selectedWorldData.inBattle9)
		{
			num = Mathf.Min(DataMgr.selectedWorldData.battleData9.currentStage, FocusCamLongInBattle.Count - 1);
		}
		else
		{
			flag = true;
		}
		if ((bool)CampMgr.Inst)
		{
			if (screenRatio > 1.7777778f)
			{
				CamController.Inst.FocusCamSizeRatio = FocusCamLong;
			}
			else
			{
				CamController.Inst.FocusCamSizeRatio = FocusCamLong / (screenRatio / 1.9f);
			}
			return;
		}
		float num2 = (float)Screen.width / (float)Screen.height;
		if (num2 <= 1.7777778f)
		{
			if (num2 <= 1.5f)
			{
			}
		}
		else if (!(num2 <= 2.3333333f))
		{
			CamController.Inst.FocusCamSizeRatio = ((num == -1) ? FocusCamLongGuide2 : FocusCamLongInBattle[num]);
			goto IL_0119;
		}
		CamController.Inst.FocusCamSizeRatio = ((num == -1) ? (FocusCamLongGuide2 / (screenRatio / 2.111f)) : (FocusCamLongInBattle[num] / (screenRatio / 2.111f)));
		goto IL_0119;
		IL_0119:
		if (focusCamSizeRatio != CamController.Inst.FocusCamSizeRatio && !flag)
		{
			CamController.Inst.ApplyFocusSize();
		}
	}

	public void SetMobileFocusWidthFirst(bool widthFirst)
	{
		cameraFocusWidthFirst = widthFirst;
	}
}
