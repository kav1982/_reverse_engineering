using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIFinishBuildShow : MonoBehaviour
{
	[Serializable]
	public class buidSideAdjust
	{
		public float uiwandSize;

		public float uiwandIntervel;

		public int uiwandLayoutTop;
	}

	[Serializable]
	public class curseRelicLayoutStep
	{
		public int numOver;

		public int paddingTop;

		public Vector2 CellSize;

		public Vector2 Spacing;
	}

	public enum RecordUIFrom
	{
		Menu,
		RankingListSteam,
		RankingListLocal,
		FinishDrop
	}

	private enum GamepadNavType
	{
		Relic,
		Curse,
		Button,
		Spell,
		Bag
	}

	public enum BuildShowType
	{
		Normal,
		Min
	}

	public BuildShowType showType;

	public Image BG;

	public float maxWandWidth;

	public float maxWandWidthOffset1;

	public float maxWandWidthOffset2;

	public float buildBGoffset;

	public float maxBagWidth;

	public float maxBagWidthOffset1;

	public float maxBagWidthOffset2;

	public VerticalLayoutGroup verticalLayout;

	public Animator anima;

	private InputActions inputActions;

	public GameObject gameobject_RelicContent;

	public GameObject gamobject_CurseContent;

	public GameObject gameobject_HideCurseText;

	public GameObject gameobject_WandContent;

	public GameObject pfb_RelicDefaultInSet;

	public Transform tsfRelicHuangPortraitRoot;

	private GameObject gameobject_UIHuangRelic;

	public GameObject gameobject_SpinePortrait;

	public GameObject deleteRecordButton;

	public GameObject deleteRecordFrame;

	public GameObject gameobject_BuildNotSupport;

	public GameObject SavePicture_Frame;

	public GameObject gameobjectCloseButton;

	public GameObject gameobjectSaveScreenshootButton;

	public Transform rtsf_BagSpell;

	public Transform rtsf_background;

	public Vector2 backgroundSizedAdd;

	public GameObject pfb_UISlot;

	public UILayout uiLayout_Spell;

	public Text text_CompleteTime;

	public Text Steamname;

	public Text BuildNotSupport;

	public Text Difficulty;

	public Text textMoveSpeed;

	public Text textDamageBuff;

	public Text textBasicProperty;

	public List<UIWand> uiWands = new List<UIWand>();

	public float wandDetailSpace;

	[Header("prefab")]
	public GameObject pfb_UISlotPotion;

	public GameObject relicprefab;

	public GameObject curseprefab;

	public GameObject wandprefab;

	public RecordUIFrom recordUIFrom;

	private UIRelic.UIRelicShowType uiRelicShowType;

	public bool IsFromLocalRankingList;

	public int recordindex;

	[Header("HP")]
	public RectTransform rtsf_HP2;

	public Slider slider_HP2;

	public TextMeshProUGUI tmp_HP;

	[Header("Shield")]
	public RectTransform rtsf_Shield;

	public TextMeshProUGUI tmp_Shield;

	public RectTransform rtsf_ShieldTemp;

	public TextMeshProUGUI tmp_ShieldTemp;

	[Header("Shield")]
	public RectTransform rtsf_Potion;

	[Space(50f)]
	public Text text_CoinCount;

	public Text text_KeyCount;

	public Text SavePicture;

	public Text DeleteRecort;

	[Header("Potion")]
	public CustomGripBestFitRect potionBestFitRect;

	[Header("RelicCurse")]
	public List<buidSideAdjust> buildLayoutAdjusts;

	public List<curseRelicLayoutStep> curseLayouts;

	public List<curseRelicLayoutStep> relicLayouts;

	public GridLayoutGroup curseLaytoutGroup;

	public GridLayoutGroup relicLaytoutGroup;

	[Header("Gamepad")]
	public List<UIRelic> uiRelics = new List<UIRelic>();

	public List<UICurse> uicurses = new List<UICurse>();

	private GamepadNavType gamepadNavType;

	public int gamepadNavIndex = -1;

	[Header("背包法术UI")]
	public GameObject bagSpellObj;

	public GameObject uiWandParent;

	public List<UIGamePadNav> uiBagSlots = new List<UIGamePadNav>();

	public List<List<UIGamePadNav>> uiSpellSlots = new List<List<UIGamePadNav>>();

	private UIGamePadNav lastWand;

	private UIGamePadNav firstBagSlot;

	private UIGamePadNav currentNavWand;

	public FinishGameBuild currentShowBuild { get; set; }

	public bool IsOpen { get; private set; }

	public bool closing { get; private set; }

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		inputActions.Player.GamepadEast.performed += GamepadBack;
		inputActions.Player.Interact.performed += InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	public void TryClickRelic()
	{
		if (!GameMgr.IsMobile_Static || recordUIFrom != 0 || !MobileMgr.inst.gamepadPlugged)
		{
			return;
		}
		switch (gamepadNavType)
		{
		case GamepadNavType.Relic:
			if (gamepadNavIndex != -1 && uiRelics.Count > gamepadNavIndex)
			{
				uiRelics[gamepadNavIndex].SwitchRelicShow();
			}
			break;
		case GamepadNavType.Curse:
		case GamepadNavType.Button:
			break;
		}
	}

	private void OnDisable()
	{
		inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		inputActions.Player.GamepadEast.performed -= GamepadBack;
		inputActions.Player.Interact.performed -= InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (!IsOpen)
		{
			return;
		}
		if (recordUIFrom == RecordUIFrom.FinishDrop)
		{
			if (SavePicture_Frame.activeInHierarchy)
			{
				SteamScreenshotsTest.Inst.ScreenShoot();
			}
			else if (deleteRecordFrame.activeSelf)
			{
				_Close();
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			return;
		}
		if (recordUIFrom == RecordUIFrom.RankingListSteam || recordUIFrom == RecordUIFrom.RankingListLocal)
		{
			if (SavePicture_Frame.activeSelf)
			{
				GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.SaveScreenShoot();
			}
			else if (deleteRecordFrame.activeSelf)
			{
				GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.DeleteThisBuildRecord();
			}
		}
		else
		{
			if (recordUIFrom != 0)
			{
				return;
			}
			switch (gamepadNavType)
			{
			case GamepadNavType.Relic:
				if (gamepadNavIndex != -1 && uiRelics.Count > gamepadNavIndex)
				{
					uiRelics[gamepadNavIndex].Click();
				}
				break;
			case GamepadNavType.Curse:
			case GamepadNavType.Button:
				break;
			}
		}
	}

	private void UpdateInfobuild(FinishGameBuild build)
	{
		HPShieldCheck(build.PlayerConfig.currentHP, build.PlayerConfig.maxHP, build.PlayerConfig.shield, build.PlayerConfig.shieldTemp);
		UpdateHP(build.PlayerConfig.currentHP, build.PlayerConfig.maxHP);
		UpdateShield(build.PlayerConfig.shield);
		UpdateShieldTemp(build.PlayerConfig.shieldTemp);
		UpdateKey(build.keyCount);
		UpdateCoin(build.coinCount);
		UpdatePotion(build.potionIDs);
	}

	private void HPShieldCheck(float CurrentHP, float MaxHP, float Shield, float ShieldTemp)
	{
		float num = 0f;
		float num2 = 20f;
		if (Shield > 0f)
		{
			rtsf_Shield.gameObject.SetActive(value: true);
			num += num2;
		}
		else
		{
			rtsf_Shield.gameObject.SetActive(value: false);
		}
		if (ShieldTemp > 0f)
		{
			rtsf_ShieldTemp.gameObject.SetActive(value: true);
			num += num2;
		}
		else
		{
			rtsf_ShieldTemp.gameObject.SetActive(value: false);
		}
		if (MaxHP == 0f)
		{
			float x = 291f;
			rtsf_HP2.sizeDelta = new Vector2(x, rtsf_Shield.sizeDelta.y);
		}
		else
		{
			float num3 = 291f - num;
			float num4 = num3 * MaxHP / (MaxHP + Shield + ShieldTemp);
			rtsf_HP2.sizeDelta = new Vector2(num4, rtsf_Shield.sizeDelta.y);
			float num5 = num4 + num2 / 2f;
			if (Shield > 0f)
			{
				float num6 = num3 * Shield / (MaxHP + Shield + ShieldTemp);
				rtsf_Shield.sizeDelta = new Vector2(num6, rtsf_Shield.sizeDelta.y);
				rtsf_Shield.anchoredPosition = new Vector2(num5, rtsf_Shield.anchoredPosition.y);
				num5 = num5 + num6 + num2 / 2f;
			}
			if (ShieldTemp > 0f)
			{
				float x2 = num3 * ShieldTemp / (MaxHP + Shield + ShieldTemp);
				rtsf_ShieldTemp.sizeDelta = new Vector2(x2, rtsf_Shield.sizeDelta.y);
				rtsf_ShieldTemp.anchoredPosition = new Vector2(num5, rtsf_Shield.anchoredPosition.y);
			}
		}
		if (CurrentHP == 0f)
		{
			slider_HP2.value = 1f;
			return;
		}
		float num7 = CurrentHP / MaxHP;
		if (!Mathf.Approximately(slider_HP2.value, num7))
		{
			slider_HP2.value = num7;
		}
	}

	private void UpdateHP(float current, float max)
	{
		if (current == 0f)
		{
			tmp_HP.text = "?? / ??";
		}
		else
		{
			tmp_HP.text = current.ToString("F0") + "/" + max;
		}
	}

	private void UpdatePotion(List<int> potionIDs)
	{
		rtsf_Potion.transform.DestroyAllChildImmediate();
		for (int i = 0; i < potionIDs.Count; i++)
		{
			UISlotPotion component = UnityEngine.Object.Instantiate(pfb_UISlotPotion, rtsf_Potion).GetComponent<UISlotPotion>();
			AspectRatioFitter aspectRatioFitter = component.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
			aspectRatioFitter.aspectRatio = 1f;
			component.gameObject.AddComponent<LayoutElement>().preferredWidth = 75f;
			component.Initialize(potionIDs[i], isFromBuild: true);
			component.UpdateInfo();
		}
		potionBestFitRect.Layout();
	}

	private void UpdateShield(float count)
	{
		tmp_Shield.text = count.ToString("F0");
	}

	private void UpdateShieldTemp(float count)
	{
		tmp_ShieldTemp.text = count.ToString("F0");
	}

	private void UpdateCoin(int count)
	{
		text_CoinCount.text = count.ToString();
	}

	private void UpdateKey(int count)
	{
		text_KeyCount.text = count.ToString();
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			switch (recordUIFrom)
			{
			case RecordUIFrom.Menu:
				gameobjectCloseButton.SetActive(value: false);
				break;
			case RecordUIFrom.RankingListSteam:
				gameobjectCloseButton.SetActive(value: true);
				break;
			case RecordUIFrom.RankingListLocal:
				gameobjectCloseButton.SetActive(value: true);
				break;
			case RecordUIFrom.FinishDrop:
				gameobjectCloseButton.SetActive(value: true);
				break;
			}
			SavePicture_Frame.SetActive(value: false);
			deleteRecordFrame.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			gameobjectCloseButton.SetActive(value: false);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		_ = IsOpen;
	}

	private void GamepadBack(InputAction.CallbackContext context)
	{
		if ((!GameMgr.IsMobile_Static || recordUIFrom != 0) && IsOpen)
		{
			Hide();
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			if (!GameMgr.IsMobile_Static)
			{
				movedirection_nav(vector);
			}
		}
	}

	public void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (IsOpen && !GameMgr.IsMobile_Static)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			movedirection_nav(direct);
		}
	}

	private bool NavToRelic()
	{
		gamepadNavIndex = 0;
		if (uiRelics.Count > 0)
		{
			gamepadNavType = GamepadNavType.Relic;
			gamepadNavIndex = 0;
			uiRelics[gamepadNavIndex].OnPointerEnter(null);
		}
		else if (uicurses.Count > 0)
		{
			gamepadNavType = GamepadNavType.Curse;
			gamepadNavIndex = 0;
			uicurses[gamepadNavIndex].OnPointerEnter(null);
		}
		else
		{
			if (recordUIFrom == RecordUIFrom.Menu)
			{
				return false;
			}
			if (deleteRecordFrame.activeInHierarchy && gameobjectSaveScreenshootButton.activeInHierarchy)
			{
				gamepadNavType = GamepadNavType.Button;
				SavePicture_Frame.SetActive(value: true);
				deleteRecordFrame.SetActive(value: false);
			}
			else
			{
				gamepadNavType = GamepadNavType.Button;
				SavePicture_Frame.SetActive(value: false);
				deleteRecordFrame.SetActive(value: true);
			}
		}
		return true;
	}

	public void movedirection_nav(Vector2 direct)
	{
		if (currentNavWand == null)
		{
			gamepadNavType = GamepadNavType.Spell;
			gamepadNavIndex = 0;
			currentNavWand = firstBagSlot;
			currentNavWand.OnSelect(null);
			return;
		}
		switch (gamepadNavType)
		{
		case GamepadNavType.Spell:
		{
			UIGamePadNav preNaveWand = currentNavWand;
			UIGamePadNav uIGamePadNav = currentNavWand.NavTo(direct, null, null, delegate
			{
				if (preNaveWand == lastWand && NavToRelic())
				{
					preNaveWand.OnDeselect(null);
					currentNavWand = lastWand;
				}
			});
			if (uIGamePadNav != null)
			{
				currentNavWand = uIGamePadNav;
			}
			break;
		}
		case GamepadNavType.Relic:
			if (direct == Vector2.left)
			{
				if (gamepadNavIndex > 0)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex--;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
			}
			else if (direct == Vector2.right)
			{
				if (gamepadNavIndex < uiRelics.Count - 1)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex++;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
			}
			else if (direct == Vector2.down)
			{
				if (gamepadNavIndex + GetElementsPerRow(relicLaytoutGroup) < uiRelics.Count - 1)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex += GetElementsPerRow(relicLaytoutGroup);
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (gamepadNavIndex != uiRelics.Count - 1)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = uiRelics.Count - 1;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (uicurses.Count > 0)
				{
					gamepadNavType = GamepadNavType.Curse;
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = 0;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (recordUIFrom != 0)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = 0;
					gamepadNavType = GamepadNavType.Button;
					if (!SavePicture_Frame.activeInHierarchy)
					{
						SavePicture_Frame.SetActive(value: true);
					}
					else if (!deleteRecordFrame.activeInHierarchy)
					{
						deleteRecordFrame.SetActive(value: true);
					}
				}
			}
			else if (direct == Vector2.up)
			{
				if (gamepadNavIndex == 0)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					NavToLastWand();
				}
				else if (gamepadNavIndex - GetElementsPerRow(relicLaytoutGroup) > 0)
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex -= GetElementsPerRow(relicLaytoutGroup);
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
				else
				{
					uiRelics[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = 0;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
			}
			break;
		case GamepadNavType.Curse:
			if (direct == Vector2.left)
			{
				if (gamepadNavIndex > 0)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex--;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
			}
			else if (direct == Vector2.right)
			{
				if (gamepadNavIndex < uicurses.Count - 1)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex++;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
			}
			else if (direct == Vector2.down)
			{
				if (gamepadNavIndex + GetElementsPerRow(curseLaytoutGroup) < uicurses.Count - 1)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex += GetElementsPerRow(curseLaytoutGroup);
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (gamepadNavIndex != uicurses.Count - 1)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = uicurses.Count - 1;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (recordUIFrom != 0)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = 0;
					gamepadNavType = GamepadNavType.Button;
					if (gameobjectSaveScreenshootButton.activeInHierarchy && !SavePicture_Frame.activeInHierarchy)
					{
						SavePicture_Frame.SetActive(value: true);
					}
					else if (deleteRecordButton.activeInHierarchy && !deleteRecordFrame.activeInHierarchy)
					{
						deleteRecordFrame.SetActive(value: true);
					}
				}
			}
			else if (direct == Vector2.up)
			{
				if (gamepadNavIndex - GetElementsPerRow(curseLaytoutGroup) > 0)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex -= GetElementsPerRow(curseLaytoutGroup);
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (gamepadNavIndex != 0)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = 0;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (uiRelics.Count > 0)
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					gamepadNavIndex = uiRelics.Count - 1;
					gamepadNavType = GamepadNavType.Relic;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
				else
				{
					uicurses[gamepadNavIndex].OnPointerExit(null);
					NavToLastWand();
				}
			}
			break;
		case GamepadNavType.Button:
			if (!(anima.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f))
			{
				break;
			}
			if (direct == Vector2.left)
			{
				if (deleteRecordFrame.activeInHierarchy && gameobjectSaveScreenshootButton.activeInHierarchy)
				{
					SavePicture_Frame.SetActive(value: true);
					deleteRecordFrame.SetActive(value: false);
				}
			}
			else if (direct == Vector2.right)
			{
				if (SavePicture_Frame.activeInHierarchy && deleteRecordButton.activeInHierarchy)
				{
					SavePicture_Frame.SetActive(value: false);
					deleteRecordFrame.SetActive(value: true);
				}
			}
			else if (direct == Vector2.up)
			{
				if (SavePicture_Frame.activeInHierarchy)
				{
					SavePicture_Frame.SetActive(value: false);
				}
				else if (deleteRecordFrame.activeInHierarchy)
				{
					deleteRecordFrame.SetActive(value: false);
				}
				if (uicurses.Count > 0)
				{
					gamepadNavType = GamepadNavType.Curse;
					gamepadNavIndex = uicurses.Count - 1;
					uicurses[gamepadNavIndex].OnPointerEnter(null);
				}
				else if (uiRelics.Count > 0)
				{
					gamepadNavType = GamepadNavType.Relic;
					gamepadNavIndex = uiRelics.Count - 1;
					uiRelics[gamepadNavIndex].OnPointerEnter(null);
				}
				else
				{
					NavToLastWand();
				}
			}
			break;
		}
	}

	public void NavToLastWand()
	{
		gamepadNavType = GamepadNavType.Spell;
		gamepadNavIndex = 0;
		lastWand.OnSelect(null);
		currentNavWand = lastWand;
	}

	public void Show2()
	{
		IsOpen = true;
		base.gameObject.SetActive(value: true);
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		ReSetGamepadNav();
		anima.Play("show2");
	}

	public void Show()
	{
		IsOpen = true;
		base.gameObject.SetActive(value: true);
		UIPlayerDataMgr.Inst.BagClose();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		StartCoroutine(Delay_Open());
		anima.Play("show");
		ReSetGamepadNav();
		InputChange();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		if (recordUIFrom == RecordUIFrom.FinishDrop)
		{
			BG.gameObject.SetActive(value: true);
			BG.color = new Color(0f, 0f, 0f, 0f);
			BG.DOFade(0.8f, 0.5f).SetUpdate(isIndependentUpdate: true);
		}
		else
		{
			BG.gameObject.SetActive(value: false);
		}
	}

	private void ReSetGamepadNav()
	{
		gamepadNavType = GamepadNavType.Bag;
		gamepadNavIndex = -1;
	}

	private IEnumerator Refresh()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (!GameMgr.IsMobile_Static)
		{
			RefreshUI();
		}
	}

	public void UpdateBuildInfoFinishBattle(FinishGameBuild build, RecordUIFrom recordUIFrom, int index = 0, string Score = "", bool localRankingList = false)
	{
		StartCoroutine(Refresh());
		currentShowBuild = build;
		this.recordUIFrom = recordUIFrom;
		InputChange();
		IsFromLocalRankingList = localRankingList;
		gameobject_BuildNotSupport.SetActive(value: false);
		switch (this.recordUIFrom)
		{
		case RecordUIFrom.Menu:
			DeleteRecort.text = 1000105.GetText();
			SavePicture.text = 1003201.GetText();
			deleteRecordButton.SetActive(value: false);
			gamepadNavType = GamepadNavType.Bag;
			gameobjectSaveScreenshootButton.SetActive(value: false);
			gameobjectCloseButton.SetActive(value: false);
			uiRelicShowType = UIRelic.UIRelicShowType.PlayerStatus;
			break;
		case RecordUIFrom.RankingListSteam:
			DeleteRecort.text = 1000105.GetText();
			SavePicture.text = 1003201.GetText();
			deleteRecordButton.SetActive(value: false);
			gamepadNavType = GamepadNavType.Bag;
			uiRelicShowType = UIRelic.UIRelicShowType.LeaderBoard;
			break;
		case RecordUIFrom.RankingListLocal:
			DeleteRecort.text = 1003202.GetText();
			SavePicture.text = 1003201.GetText();
			deleteRecordButton.SetActive(value: true);
			gamepadNavType = GamepadNavType.Bag;
			uiRelicShowType = UIRelic.UIRelicShowType.LeaderBoard;
			break;
		case RecordUIFrom.FinishDrop:
			DeleteRecort.text = 1000105.GetText();
			SavePicture.text = 1003201.GetText();
			deleteRecordButton.SetActive(value: true);
			gamepadNavType = GamepadNavType.Bag;
			uiRelicShowType = UIRelic.UIRelicShowType.Build;
			if (GameMgr.IsMobile_Static)
			{
				gameobjectSaveScreenshootButton.SetActive(value: false);
				deleteRecordButton.SetActive(value: false);
			}
			break;
		}
		if (!SteamManager.Initialized)
		{
			gameobjectSaveScreenshootButton.SetActive(value: false);
		}
		recordindex = index;
		UpdaterelicCurse();
		if (showType != BuildShowType.Min)
		{
			UpdateWand();
			UpdateInfobuild(currentShowBuild);
			UpdatePlayerInfo(Score);
			UpdateSkeletonGraphic();
		}
	}

	private void RefreshUI()
	{
		Debug.Log("RefreshUI");
		gamepadNavIndex = 0;
		uiBagSlots.Clear();
		uiSpellSlots.Clear();
		currentNavWand = null;
		firstBagSlot = null;
		lastWand = null;
		List<UIWand> list = uiWandParent.transform.GetComponentsInChildren<UIWand>().ToList();
		for (int i = 0; i < list.Count; i++)
		{
			UIWand uIWand = list[i];
			Transform transform = uIWand.transform.Find("Panel_Spells");
			uiSpellSlots.Add((transform.transform.childCount > 0) ? transform.GetComponentsInChildren<UIGamePadNav>().ToList() : new List<UIGamePadNav>());
			uiSpellSlots[i].Insert(0, uIWand.transform.Find("UIWandEvent").gameObject.GetComponent<UIGamePadNav>());
		}
		List<List<UIGamePadNav>> list2 = uiSpellSlots;
		lastWand = list2[list2.Count - 1][0];
		uiBagSlots = bagSpellObj.GetComponentsInChildren<UIGamePadNav>().ToList();
		firstBagSlot = uiBagSlots[0];
		int count = uiBagSlots.Count;
		int count2 = uiSpellSlots[0].Count;
		for (int j = 0; j < count; j++)
		{
			UIGamePadNav uIGamePadNav = uiBagSlots[j];
			UIGamePadNav left = ((j == 0) ? uiBagSlots[count - 1] : uiBagSlots[j - 1]);
			UIGamePadNav right = ((j == count - 1) ? uiBagSlots[0] : uiBagSlots[j + 1]);
			UIGamePadNav up = null;
			UIGamePadNav uIGamePadNav2 = null;
			int index = ((j + 1 > count2 - 1) ? (count2 - 1) : (j + 1));
			uIGamePadNav2 = uiSpellSlots[0][index];
			uIGamePadNav.SetNav(up, uIGamePadNav2, left, right);
		}
		for (int k = 0; k < uiSpellSlots.Count; k++)
		{
			int num = 0;
			foreach (UIGamePadNav item in uiSpellSlots[k])
			{
				int count3 = uiSpellSlots[k].Count;
				UIGamePadNav uIGamePadNav3 = item;
				UIGamePadNav uIGamePadNav4 = item;
				UIGamePadNav down = null;
				UIGamePadNav uIGamePadNav5 = null;
				uIGamePadNav3 = ((num == 0) ? uiSpellSlots[k][count3 - 1] : uiSpellSlots[k][num - 1]);
				uIGamePadNav4 = ((num == count3 - 1) ? uiSpellSlots[k][0] : uiSpellSlots[k][num + 1]);
				if (k == 0)
				{
					if (num == 0 || num == 1)
					{
						uIGamePadNav5 = uiBagSlots[0];
					}
					else
					{
						int num2 = num - 1;
						num2 = ((num2 > uiBagSlots.Count - 1) ? (uiBagSlots.Count - 1) : num2);
						uIGamePadNav5 = uiBagSlots[num2];
					}
				}
				else
				{
					int count4 = uiSpellSlots[k - 1].Count;
					int index2 = ((num > count4 - 1) ? (count4 - 1) : num);
					uIGamePadNav5 = uiSpellSlots[k - 1][index2];
				}
				if (k + 1 >= uiSpellSlots.Count)
				{
					if (num != 0)
					{
						down = uiSpellSlots[k][0];
					}
				}
				else
				{
					int count5 = uiSpellSlots[k + 1].Count;
					int index3 = ((num > count5 - 1) ? (count5 - 1) : num);
					down = uiSpellSlots[k + 1][index3];
				}
				item.SetNav(uIGamePadNav5, down, uIGamePadNav3, uIGamePadNav4);
				num++;
			}
		}
		UIMgr.Inst.InputChange();
	}

	private void UpdatePlayerInfo(string Score = "")
	{
		textMoveSpeed.text = 1006102.GetText() + ": " + ((currentShowBuild.moveSpeed == 0f) ? 5.5f : currentShowBuild.moveSpeed);
		textDamageBuff.text = 1006103.GetText() + ": " + (int)(currentShowBuild.damageRatio * 100f) + "%";
		textBasicProperty.text = 1006104.GetText();
		Steamname.text = currentShowBuild.username;
		switch (currentShowBuild.Difficulty)
		{
		case 0:
			Difficulty.text = 1003203.GetText() + ": " + 1002601.GetText();
			break;
		case 1:
			Difficulty.text = 1003203.GetText() + ": " + 1002602.GetText();
			break;
		case 2:
			Difficulty.text = 1003203.GetText() + ": " + 1002603.GetText();
			break;
		case 3:
			Difficulty.text = 1003203.GetText() + ": " + 1002605.GetText();
			break;
		case 4:
			Difficulty.text = 1003203.GetText() + ": " + 1002606.GetText();
			break;
		case 5:
			Difficulty.text = 1003203.GetText() + ": " + 1002607.GetText();
			break;
		}
		if (recordUIFrom == RecordUIFrom.Menu && PlayerMgr.Inst.BaData.curseIDs.Contains(29))
		{
			HPShieldCheck(60f, 60f, 20f, 20f);
			text_CoinCount.text = "??";
			text_KeyCount.text = "??";
			tmp_HP.text = "?? / ??";
			tmp_Shield.text = "??";
			tmp_ShieldTemp.text = "??";
			textMoveSpeed.text = 1006102.GetText() + ": ???";
			textDamageBuff.text = 1006103.GetText() + ": ???";
		}
		if (currentShowBuild.timeuse != 0f)
		{
			if (currentShowBuild.timeuse == -1f)
			{
				text_CompleteTime.text = 1003111.GetText() + ": 00:00:00 ";
				return;
			}
			int num = (int)currentShowBuild.timeuse / 3600;
			int num2 = (int)(currentShowBuild.timeuse % 3600f) / 60;
			int num3 = (int)currentShowBuild.timeuse % 60;
			text_CompleteTime.text = 1003111.GetText() + ": " + $"{num:D2}:{num2:D2}:{num3:D2}";
		}
		else
		{
			gameobject_BuildNotSupport.SetActive(value: true);
			BuildNotSupport.text = 1003207.GetText();
			text_CompleteTime.text = 1003111.GetText() + ": " + Score;
		}
	}

	private void UpdateWand()
	{
		uiWands.Clear();
		int num = ((recordUIFrom == RecordUIFrom.Menu) ? UIPlayerDataMgr.Inst.uiWands.Count : currentShowBuild.wandCfgs.Count);
		buidSideAdjust buidSideAdjust = ((buildLayoutAdjusts.Count >= num) ? buildLayoutAdjusts[num - 1] : buildLayoutAdjusts[6]);
		while (gameobject_WandContent.transform.childCount > 1)
		{
			UnityEngine.Object.DestroyImmediate(gameobject_WandContent.transform.GetChild(1).gameObject);
		}
		gameobject_WandContent.transform.GetChild(0).localScale = new Vector3(buidSideAdjust.uiwandSize, buidSideAdjust.uiwandSize, 1f);
		for (int i = 0; i < currentShowBuild.wandCfgs.Count; i++)
		{
			UIWand component = UnityEngine.Object.Instantiate(wandprefab, gameobject_WandContent.transform).GetComponent<UIWand>();
			component.InitializeBuild(currentShowBuild, i);
			component.UpdateInfoBuild(currentShowBuild, i);
			uiWands.Add(component);
			component.rtsf_Self.anchoredPosition = new Vector2(0f, (float)(-i) * wandDetailSpace);
			component.uiWandEvent.SetDrag(drag: false);
			component.SetSpellDrag(drag: false);
		}
		StartCoroutine(waitScale(buidSideAdjust));
		StartCoroutine(Updatebag(currentShowBuild, buidSideAdjust));
	}

	private void UpdaterelicCurse()
	{
		uiRelics.Clear();
		uicurses.Clear();
		updateRelicAndCurseLaytout(currentShowBuild.relicCfgs.Count, currentShowBuild.curseIDs.Count);
		gameobject_RelicContent.transform.DestroyAllChild();
		for (int i = 0; i < currentShowBuild.relicCfgs.Count; i++)
		{
			UIRelic component = UnityEngine.Object.Instantiate(relicprefab, gameobject_RelicContent.transform).GetComponent<UIRelic>();
			uiRelics.Add(component);
			component.InitializeToBuildShow(i, currentShowBuild, uiRelicShowType);
		}
		gamobject_CurseContent.transform.DestroyAllChild();
		gameobject_HideCurseText.SetActive(value: false);
		for (int j = 0; j < currentShowBuild.curseIDs.Count && j <= 55; j++)
		{
			UICurse component2 = UnityEngine.Object.Instantiate(curseprefab, gamobject_CurseContent.transform).GetComponent<UICurse>();
			uicurses.Add(component2);
			component2.Initialize(j, currentShowBuild);
		}
	}

	public void UpdateSkeletonGraphic()
	{
		if (currentShowBuild.selectedSetID == 9)
		{
			gameobject_SpinePortrait.SetActive(value: false);
			if (gameobject_UIHuangRelic == null)
			{
				gameobject_UIHuangRelic = UnityEngine.Object.Instantiate(pfb_RelicDefaultInSet, tsfRelicHuangPortraitRoot);
			}
			else
			{
				gameobject_UIHuangRelic.SetActive(value: true);
			}
			return;
		}
		gameobject_SpinePortrait.SetActive(value: true);
		if (gameobject_UIHuangRelic != null)
		{
			gameobject_UIHuangRelic.SetActive(value: false);
		}
		RecordUIFrom recordUIFrom = this.recordUIFrom;
		bool ignoreDisableRelicSkin = recordUIFrom == RecordUIFrom.RankingListSteam || recordUIFrom == RecordUIFrom.RankingListLocal;
		PlayerSkinMgr.Inst.SetSkinButBuild(gameobject_SpinePortrait.GetComponent<SkeletonGraphic>().Skeleton, currentShowBuild, gameobject_SpinePortrait, ignoreDisableRelicSkin);
		gameobject_SpinePortrait.GetComponent<SkeletonGraphic>().Update();
	}

	private void updateRelicAndCurseLaytout(int relicCount, int curseCount)
	{
		for (int num = relicLayouts.Count - 1; num > 0; num--)
		{
			if (relicCount > relicLayouts[num].numOver)
			{
				relicLaytoutGroup.padding.top = relicLayouts[num].paddingTop;
				relicLaytoutGroup.cellSize = relicLayouts[num].CellSize;
				relicLaytoutGroup.spacing = relicLayouts[num].Spacing;
				break;
			}
		}
		for (int num2 = curseLayouts.Count - 1; num2 > 0; num2--)
		{
			if (curseCount > curseLayouts[num2].numOver)
			{
				curseLaytoutGroup.padding.top = curseLayouts[num2].paddingTop;
				curseLaytoutGroup.cellSize = curseLayouts[num2].CellSize;
				curseLaytoutGroup.spacing = curseLayouts[num2].Spacing;
				break;
			}
		}
	}

	private void Hide()
	{
		TryExitCurrentSelect();
		StopAllCoroutines();
		closing = true;
		anima.Play("hide");
		gameobject_BuildNotSupport.SetActive(value: false);
		UIPlayerDataMgr.Inst.HideAllInfoPanel();
		if (recordUIFrom == RecordUIFrom.FinishDrop)
		{
			BG.DOFade(0f, 0.5f).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void TryExitCurrentSelect()
	{
		if (gamepadNavIndex != -1 && uiRelics.Count > gamepadNavIndex)
		{
			uiRelics[gamepadNavIndex].OnPointerExit(null);
		}
	}

	public void HideImmediate()
	{
		StopAllCoroutines();
		closing = false;
		IsOpen = false;
		if (base.gameObject.activeSelf)
		{
			anima.Play("HideDirect");
		}
		gameobject_BuildNotSupport.SetActive(value: false);
		UIPlayerDataMgr.Inst.HideAllInfoPanel();
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator Delay_Open()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		IsOpen = true;
	}

	private IEnumerator Updatebag(FinishGameBuild build, buidSideAdjust currentAdjust)
	{
		rtsf_BagSpell.DestroyAllChild();
		for (int i = 0; i < build.bagCount; i++)
		{
			UnityEngine.Object.Instantiate(pfb_UISlot, rtsf_BagSpell).GetComponent<UISlotBag>().Initialize(i, build);
		}
		yield return new WaitForEndOfFrame();
		uiLayout_Spell.Layout();
		float x = rtsf_BagSpell.GetChild(rtsf_BagSpell.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x;
		if (x * currentAdjust.uiwandSize > maxBagWidth)
		{
			float num = (maxBagWidth + maxBagWidthOffset1) / (x * currentAdjust.uiwandSize + maxBagWidthOffset2);
			rtsf_BagSpell.localScale = new Vector3(num, num, 1f);
			if (rtsf_background != null)
			{
				rtsf_background.GetComponent<RectTransform>().sizeDelta = rtsf_BagSpell.GetComponent<RectTransform>().sizeDelta + backgroundSizedAdd;
				rtsf_background.localScale = new Vector3(num, num, 1f);
			}
		}
		else
		{
			rtsf_BagSpell.localScale = new Vector3(1f, 1f, 1f);
			if (rtsf_background != null)
			{
				rtsf_background.GetComponent<RectTransform>().sizeDelta = rtsf_BagSpell.GetComponent<RectTransform>().sizeDelta + backgroundSizedAdd;
				rtsf_background.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		verticalLayout.spacing = currentAdjust.uiwandIntervel;
		verticalLayout.padding.top = currentAdjust.uiwandLayoutTop;
	}

	private IEnumerator waitScale(buidSideAdjust currentAdjust)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < uiWands.Count; i++)
		{
			uiWands[i].transform.localScale = new Vector3(currentAdjust.uiwandSize, currentAdjust.uiwandSize, 1f);
			if (uiWands[i].rtsf_Spells.childCount != 0)
			{
				float x = uiWands[i].rtsf_Spells.GetChild(uiWands[i].rtsf_Spells.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x;
				if ((x - maxWandWidthOffset1 + maxWandWidthOffset2) * currentAdjust.uiwandSize > maxWandWidth)
				{
					float num = (maxWandWidth / currentAdjust.uiwandSize + maxWandWidthOffset1) / (x + maxWandWidthOffset2);
					uiWands[i].rtsf_Spells.localScale = new Vector3(num, num, 1f);
					uiWands[i].rtsf_SlotsBG.localScale = new Vector3(num, num, 1f);
					uiWands[i].buidlBG.sizeDelta = new Vector2((maxWandWidth + buildBGoffset) / currentAdjust.uiwandSize, uiWands[i].buidlBG.sizeDelta.y);
				}
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(gameobject_WandContent.GetComponent<RectTransform>());
	}

	private int GetElementsPerRow(GridLayoutGroup gridLayoutGroup)
	{
		float width = gridLayoutGroup.GetComponent<RectTransform>().rect.width;
		float num = gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x;
		return Mathf.FloorToInt((width + gridLayoutGroup.spacing.x) / num);
	}

	public void CloseFinish()
	{
		closing = false;
		IsOpen = false;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		base.gameObject.SetActive(value: false);
	}

	public void _Close()
	{
		SavePicture_Frame.SetActive(value: false);
		deleteRecordFrame.SetActive(value: false);
		SEMgr.Inst.uiClick.PlaySE();
		Hide();
	}

	public void SaveScreenShoot()
	{
		if (SteamManager.Initialized)
		{
			SteamScreenshotsTest.Inst.ScreenShoot();
		}
	}

	public void DeleteThisBuildRecord()
	{
		if (IsFromLocalRankingList)
		{
			DataMgr.finishGameBuilds.finishGameBuilds.RemoveAt(recordindex);
			_Close();
			GameUISingletonMono<UI_RankingList>.Inst.UpdateLocalLeaderBoards();
			DataMgr.SaveBuildDatas();
			DataMgr.SaveBuildBackUp();
		}
		else
		{
			_Close();
			DataMgr.SaveBuildDatas();
			DataMgr.SaveBuildBackUp();
		}
	}
}
