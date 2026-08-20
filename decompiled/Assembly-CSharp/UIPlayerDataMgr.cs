using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerDataMgr : MonoBehaviour
{
	public enum ResourceUIPop
	{
		Crystal,
		Blood,
		Cores,
		Coin,
		Gear
	}

	public GameObject panel_DownLeft;

	public Animator anima_PlayerInfo;

	public RectTransform rtstCoinAndKey;

	public Text text_CoinCount;

	public Text text_KeyCount;

	public Text text_MagicCrystalCount;

	public Text text_AncientBloodCount;

	public Text text_ChaosCoreCount;

	public Text text_GearCount;

	public GameObject panel_AncientBlood;

	public GameObject panel_ChaosCore;

	public GameObject panel_Crystal;

	public GameObject panel_Key;

	public GameObject panel_Gear;

	public GameObject panel_Gear_Mobile;

	public Text textBag;

	public RectTransform rtsf_ActiveRelicUIRoot;

	public float[] skillUIOffsetMobile;

	public UIPlayerInfoLeftDown playerinfoNormal;

	public UIPlayerInfoLeftDown playerinfoDave;

	private UIPlayerInfoLeftDown currentUILeftDown;

	[Header("BagSpell")]
	public Image image_BagBtn;

	public Sprite sprite_BagOpen;

	public Sprite sprite_BagClose;

	public Sprite sprite_MobileBagOpenFull;

	public Sprite sprite_MobileBagCloseFull;

	public GameObject pfb_UISlot;

	public RectTransform rtsf_BagSpell;

	public UILayout uiLayout_Spell;

	[Header("BagSpellBG")]
	public Image image_BagSpellBG;

	public Sprite sprite_BagSpellBGDefault;

	public Sprite sprite_BagSpellBGReaper;

	[Header("WandDetail")]
	public GameObject pfb_UIWandDetail;

	public RectTransform rtsf_Wand;

	public float wandDetailSpace;

	[Header("Potion")]
	public UIPotionsController uiPotionsCtrller;

	public Vector3 infoYOffsetSpell;

	public Vector3 infoYOffsetWand;

	public Vector3 infoYOffsetBlessing;

	public Vector3 infoYOffsetCurse;

	public Vector3 gamepad_DragWandOffset;

	public Vector3 gamepad_DragSlotOffset;

	[Header("Relic")]
	public GameObject pfb_UIRelic;

	public GridLayoutGroup uiLayout_Relic;

	[Header("Curse")]
	public GameObject pfb_UICurse;

	public UILayout uiLayout_Curse;

	public float curseUIAreaWidth = 1750f;

	public Canvas canvasCores;

	public Canvas canvasBlood;

	public Canvas canvasCrystal;

	public Canvas canvasCoin;

	public Canvas canvasGear;

	public GameObject fullGameBtn;

	public GameObject buyGameFX;

	public GameObject buySuitBtn;

	private List<UISlotBag> uiSlot_Bags = new List<UISlotBag>();

	[HideInInspector]
	public List<UIWand> uiWands = new List<UIWand>();

	private List<UIWandEvent> uiWandEvents = new List<UIWandEvent>();

	[HideInInspector]
	[Space(50f)]
	public UISlotBag uiSlotBag_Drag;

	[HideInInspector]
	public UISlotWand uiSlotWand_Drag;

	[HideInInspector]
	public UISlotPotion uiSlotPotion_drag;

	[HideInInspector]
	public UIWand uiWand_Drag;

	private WandConfig wandCfg_Drag;

	private UISlotBag uiSlotBag_Hover;

	private UISlotWand uiSlotWand_Hover;

	private UISlotWandExternal uiSlotExternal_Hover;

	private UISlotPotion uiSlotPotion_Hover;

	private PointerEventData pointerUpEventData = new PointerEventData(EventSystem.current);

	public bool isHoverHP;

	public bool isHoverMP;

	private float? dragWandMana;

	private InputActions inputActions;

	private int gamepadBagOrWandIndex = -1;

	private int gamepadSlotIndex;

	private bool gamepadSlotIndexInTidyUpBtn;

	private int gamepadBagOrWandIndexDragBefore;

	private int gamepadSlotIndexDragBefore;

	private float gamepadDragDuration;

	private const float gamepadClickTime = 0.3f;

	private const float gamepadHoldingMoveStartCD = 0.2f;

	private const float gamepadHoldingMoveCD = 0.05f;

	private List<int> flyingRelicIdOnlyNew = new List<int>();

	private List<int> flyingRelicId = new List<int>();

	public List<UpdatButtonShow> updatButtonShows;

	[Header("调整背包法杖大小自适应参数")]
	public float adjustBagSize = -110f;

	public float adjustBagSize2 = 50f;

	public float adjustWandSize = 130f;

	public float adjustWandSize1 = 64f;

	public float adjustWandSize2 = -97.8f;

	public float adjustWandSize3 = -67.7f;

	[Header("移动端需要")]
	public GameObject goHandBookGuideParticle;

	public GameObject goHandBookButton;

	public GameObject goHandBookButtonFake;

	public GameObject goMobileDropArea;

	public GameObject goMobileDropAreaHighLighted;

	public GameObject goMenuButton;

	public GameObject OpenBagButton;

	public RectTransform uiLeftUp;

	public GameObject TidyUpPenel;

	public RectTransform rectTidyUpPenel;

	public RectTransform uiWand;

	public RectTransform uiBag;

	public RectTransform uiBagButton1;

	public RectTransform uiBagButton2;

	public CanvasGroup uiCrystalCountCanvasGroup;

	public CanvasGroup uiBloodCountCanvasGroup;

	public CanvasGroup uiCoreCountCanvasGroup;

	public CanvasGroup uiCoinAndKeyCanvasGroup;

	public CanvasGroup uiGearCountCanvasGroup;

	private float showTimeCrystal;

	private float showTimeBlood;

	private float showTimeCore;

	private float showTimeGear;

	private Vector3 menuePosition;

	public Vector2 wandOffsetStartMobile = new Vector2(0f, -30f);

	public bool isChangingSpell;

	[HideInInspector]
	public UISlotBag uislotBagSelected;

	[HideInInspector]
	public UISlotWand uislotWandSelected;

	public Canvas CanvasLeftUp;

	public Canvas CanvasWandOnly;

	public float timeShortClick = 0.3f;

	public float _timeShortClick;

	public float doubleClickthr = 0.2f;

	public float _doubleClickthr;

	public Animator uiPlayerInfoBGAnimator;

	public CanvasGroup uiPlayerInfoBG;

	public Vector2 uiWandPositionBagClosed;

	public Vector2 uiWandPositionBagOpen;

	private float bagScaleClosed = 1.5f;

	private float bagScaleOpened = 2.2f;

	public Vector2 bagPositionClosed;

	public Vector2 bagPositionOpened;

	public RectTransform rectUIResource;

	public Vector2 UIResourcePositionMobileWithCurse;

	public Vector2 UIResourcePositionMobileWithOutCuse;

	public Canvas foldWandCanvas;

	public Transform mobileFoldWandButton;

	public Vector3 mobildFoldWandOffset;

	public Vector3 mobildFoldWandOffsetNoWandShown;

	[Header("位置调整")]
	public GameObject playerdata_HPMP;

	public GameObject playerdata_UpLeft;

	public GameObject playerdata_UpRight;

	public GameObject playerdata_DownResource;

	public GameObject playerdata_DownRight;

	private float playerdata_HPMP_X;

	private float playerdata_UpLeft_X;

	private float playerdata_UpRight_X;

	private float playerdata_DownResource_X;

	private float playerdata_DownRight_X;

	private float playerdata_dropArea_X;

	private float positionAdjustRatio = 150f;

	[Header("UISpellInfo位置自动调整")]
	public Vector2 uiSpellInfPivotOffsetPercent = new Vector2(0f, 1f);

	public Vector2 uiSpellinfoYOffsetSpellMobileAutoMirror;

	public Vector2 uiSpellinfoYOffsetSpellMobile;

	public Vector2 slotSelectedOffset;

	public float slotSelectedScale = 1.2f;

	[Header("UIWand位置自动调整")]
	public Vector2 uiWandfPivotOffset = new Vector2(0f, 1f);

	public Vector2 uiWandfOffsetMobileAutoMirror;

	public Vector2 uiWandOffsetMobile;

	[Header("教学引导")]
	public UIBagParticleOrbit guideMobileBag;

	public UIBagParticleOrbit guideMobileHandBook;

	private Vector3 bagButtonPosition;

	public GameObject healthTip;

	public float mobileWarnMPIntervalTimer;

	public List<Component> hideResourceComponent;

	private UnitProperty_Dots playerPptDots;

	private (Vector2 direct, float startTime)? _holdingDpad;

	private float _holdingDpadMoveCD;

	private UISlotWand currentHighlightSlotWand { get; set; }

	private UISlotBag currentHighlightSlotBag { get; set; }

	private bool ShowEndlessInfo
	{
		get
		{
			if (ICJNOGPFMAM.HLMJIJADLNC)
			{
				if (!(CampMgr.Inst != null) || LevelMgr.Inst.CurrentRoomMapPos.y >= 0)
				{
					if (BattleMgr.Inst != null)
					{
						return BattleMgr.Inst.CurrentStage == 300;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public UISlotWandTips uiSlotWandTips => UIMgr.Inst.uiSlotWandTips;

	public UIInfoWand uiInfoWandHover => UIMgr.Inst.uiInfoWandHover;

	public UIInfoSpell uiInfoSpellHover => UIMgr.Inst.uiInfoSpellHover;

	public UIInfoRelic uiInfoRelicHover => UIMgr.Inst.uiInfoRelicHover;

	public UIInfoPotion uiInfoPotionHover => UIMgr.Inst.uiInfoPotionHover;

	public UIInfoCurse uiInfoCurseHover => UIMgr.Inst.uiInfoCurseHover;

	public Image image_SlotDraging => UIMgr.Inst.uIImageDragings.image_SlotDraging;

	public Image image_SlotDragingStar1 => UIMgr.Inst.uIImageDragings.image_SlotDragingStar1;

	public Image image_SlotDragingStar2 => UIMgr.Inst.uIImageDragings.image_SlotDragingStar2;

	public Image image_WandDraging => UIMgr.Inst.uIImageDragings.image_WandDraging;

	public Image image_PotionDraging => UIMgr.Inst.uIImageDragings.image_PotionDraging;

	public float curseUISize => GameMgr.IsMobile_Static ? 60 : 50;

	public static UIPlayerDataMgr Inst { get; private set; }

	public bool IsBagOpen => rtsf_BagSpell.gameObject.activeSelf;

	public bool IsDraging
	{
		get
		{
			if (!(uiSlotBag_Drag != null) && !(uiSlotWand_Drag != null) && !(uiSlotPotion_drag != null))
			{
				return uiWand_Drag != null;
			}
			return true;
		}
	}

	public UICurse uiCurse_Hover { get; private set; }

	public UIRelic uiRelic_Hover { get; private set; }

	public Canvas CanvasDrag => UIMgr.Inst.uIImageDragings.CanvasDrag;

	public Canvas CanvasWandDrag => UIMgr.Inst.uIImageDragings.CanvasWandDrag;

	public float currentBattleUIOffset { get; set; }

	public int CurrentRelicLevel(int relicID)
	{
		RelicConfig relicConfig = PlayerMgr.Inst.ItemCtrller.GetRelicConfig(relicID);
		if (relicConfig != null)
		{
			return relicConfig.level + GetFlyingRelicCountById(relicID);
		}
		return GetFlyingRelicCountById(relicID);
	}

	public int GetFlyingRelicCountById(int id)
	{
		int num = 0;
		foreach (int item in flyingRelicId)
		{
			if (item == id)
			{
				num++;
			}
		}
		return num;
	}

	private void BagPerformed(InputAction.CallbackContext context)
	{
		if (!anima_PlayerInfo.GetCurrentAnimatorStateInfo(0).IsName("HideDirect") && !anima_PlayerInfo.GetCurrentAnimatorStateInfo(0).IsName("Hide") && !anima_PlayerInfo.GetCurrentAnimatorStateInfo(0).IsName("PotionShow") && !anima_PlayerInfo.GetCurrentAnimatorStateInfo(0).IsName("PotionHide"))
		{
			BagOpenOrClose();
		}
	}

	private void GamepadDpadPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && DataMgr.selectedWorldData.battleData9 != null && IsBagOpen && !HaveUIOpen())
		{
			Vector2 direct = context.ReadValue<Vector2>();
			GamepadDpadMove(direct);
			GamepadDpadHoldingStart(direct);
		}
	}

	private void GamepadDpadCanceled(InputAction.CallbackContext context)
	{
		GamepadDpadHoldingCancel();
	}

	private void GamepadDpadMove(Vector2 direct)
	{
		ClampGamepadIndex();
		if (direct == Vector2.up)
		{
			if (gamepadBagOrWandIndex == -1)
			{
				if (gamepadSlotIndexInTidyUpBtn)
				{
					gamepadSlotIndexInTidyUpBtn = false;
					UITidyupButton.Inst.GamepadUnHover();
				}
				uiSlot_Bags[gamepadSlotIndex].OnPointerExit(null);
				if (uiSlotBag_Drag != null || uiSlotWand_Drag != null)
				{
					for (int num = uiWands.Count - 1; num >= 0; num--)
					{
						if (uiWands[num].WandCfg != null && uiWands[num].AllSlotCount > 0)
						{
							gamepadBagOrWandIndex = num;
							gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gamepadSlotIndex));
							break;
						}
					}
				}
				else
				{
					gamepadBagOrWandIndex = uiWands.Count - 1;
					gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gamepadSlotIndex));
				}
				if (gamepadBagOrWandIndex == -1)
				{
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
				}
				else if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
				return;
			}
			gamepadBagOrWandIndex = Mathf.Clamp(gamepadBagOrWandIndex, -1, uiWands.Count - 1);
			if (gamepadSlotIndex == -1)
			{
				uiWandEvents[gamepadBagOrWandIndex].OnPointerExit(null);
			}
			else
			{
				uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerExit(null);
			}
			if (uiWand_Drag != null)
			{
				gamepadBagOrWandIndex--;
				if (gamepadBagOrWandIndex == -1)
				{
					gamepadBagOrWandIndex = uiWands.Count - 1;
				}
				uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				return;
			}
			if (uiSlotBag_Drag != null || uiSlotWand_Drag != null)
			{
				int gridIndexFromActual = uiWands[gamepadBagOrWandIndex].GetGridIndexFromActual(gamepadSlotIndex);
				bool flag = false;
				for (int num2 = gamepadBagOrWandIndex - 1; num2 >= 0; num2--)
				{
					if (uiWands[num2].WandCfg != null && uiWands[num2].AllSlotCount > 0)
					{
						flag = true;
						gamepadBagOrWandIndex = num2;
						gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gridIndexFromActual));
						uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
						break;
					}
				}
				if (!flag)
				{
					gamepadBagOrWandIndex = -1;
					if (gridIndexFromActual > uiSlot_Bags.Count - 1)
					{
						gamepadSlotIndex = uiSlot_Bags.Count - 1;
					}
					else if (gridIndexFromActual < 0)
					{
						gamepadSlotIndex = 0;
					}
					else
					{
						gamepadSlotIndex = gridIndexFromActual;
					}
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
				}
				return;
			}
			int gridIndexFromActual2 = uiWands[gamepadBagOrWandIndex].GetGridIndexFromActual(gamepadSlotIndex);
			gamepadBagOrWandIndex--;
			if (gamepadBagOrWandIndex == -1)
			{
				if (gridIndexFromActual2 > uiSlot_Bags.Count - 1)
				{
					gamepadSlotIndex = uiSlot_Bags.Count - 1;
				}
				else if (gridIndexFromActual2 < 0)
				{
					gamepadSlotIndex = 0;
				}
				else
				{
					gamepadSlotIndex = gridIndexFromActual2;
				}
				uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
			}
			else
			{
				gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gridIndexFromActual2));
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
			}
		}
		else if (direct == Vector2.down)
		{
			if (gamepadBagOrWandIndex == -1)
			{
				if (gamepadSlotIndexInTidyUpBtn)
				{
					gamepadSlotIndexInTidyUpBtn = false;
					UITidyupButton.Inst.GamepadUnHover();
				}
				uiSlot_Bags[gamepadSlotIndex].OnPointerExit(null);
				gamepadBagOrWandIndex++;
				gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gamepadSlotIndex));
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
				return;
			}
			if (gamepadSlotIndex == -1)
			{
				uiWandEvents[gamepadBagOrWandIndex].OnPointerExit(null);
			}
			else
			{
				uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerExit(null);
			}
			if (uiWand_Drag != null)
			{
				gamepadBagOrWandIndex++;
				if (gamepadBagOrWandIndex >= uiWands.Count)
				{
					gamepadBagOrWandIndex = 0;
				}
				uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				return;
			}
			if (uiSlotBag_Drag != null || uiSlotWand_Drag != null)
			{
				int gridIndexFromActual3 = uiWands[gamepadBagOrWandIndex].GetGridIndexFromActual(gamepadSlotIndex);
				bool flag2 = false;
				for (int i = gamepadBagOrWandIndex + 1; i < uiWands.Count; i++)
				{
					if (uiWands[i].WandCfg != null && uiWands[i].AllSlotCount > 0)
					{
						flag2 = true;
						gamepadBagOrWandIndex = i;
						gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gridIndexFromActual3));
						uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
						break;
					}
				}
				if (!flag2)
				{
					gamepadBagOrWandIndex = -1;
					if (gridIndexFromActual3 > uiSlot_Bags.Count - 1)
					{
						gamepadSlotIndex = uiSlot_Bags.Count - 1;
					}
					else if (gridIndexFromActual3 < 0)
					{
						gamepadSlotIndex = 0;
					}
					else
					{
						gamepadSlotIndex = gridIndexFromActual3;
					}
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
				}
				return;
			}
			int gridIndexFromActual4 = uiWands[gamepadBagOrWandIndex].GetGridIndexFromActual(gamepadSlotIndex);
			gamepadBagOrWandIndex++;
			if (gamepadBagOrWandIndex >= uiWands.Count)
			{
				gamepadBagOrWandIndex = -1;
			}
			if (gamepadBagOrWandIndex == -1)
			{
				if (gridIndexFromActual4 > uiSlot_Bags.Count - 1)
				{
					gamepadSlotIndex = uiSlot_Bags.Count - 1;
				}
				else if (gridIndexFromActual4 < 0)
				{
					gamepadSlotIndex = 0;
				}
				else
				{
					gamepadSlotIndex = gridIndexFromActual4;
				}
				uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
			}
			else
			{
				gamepadSlotIndex = Mathf.Min(uiWands[gamepadBagOrWandIndex].AllSlotCount - 1, uiWands[gamepadBagOrWandIndex].GetActualIndexFromGrid(gridIndexFromActual4));
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
			}
		}
		else if (direct == Vector2.left)
		{
			if (gamepadBagOrWandIndex == -1)
			{
				if (gamepadSlotIndexInTidyUpBtn)
				{
					gamepadSlotIndexInTidyUpBtn = false;
					UITidyupButton.Inst.GamepadUnHover();
					gamepadSlotIndex = uiSlot_Bags.Count - 1;
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
					return;
				}
				gamepadSlotIndex = Mathf.Clamp(gamepadSlotIndex, 0, uiSlot_Bags.Count - 1);
				uiSlot_Bags[gamepadSlotIndex].OnPointerExit(null);
				gamepadSlotIndex--;
				if (IsDraging && gamepadSlotIndex < 0)
				{
					gamepadSlotIndex = uiSlot_Bags.Count - 1;
				}
				if (gamepadSlotIndex >= 0)
				{
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
					return;
				}
				gamepadSlotIndex++;
				UITidyupButton.Inst.GamepadHover();
				gamepadSlotIndexInTidyUpBtn = true;
			}
			else
			{
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerExit(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerExit(null);
				}
				gamepadSlotIndex--;
				if (gamepadSlotIndex < -1)
				{
					gamepadSlotIndex = uiWands[gamepadBagOrWandIndex].AllSlotCount - 1;
				}
				if (gamepadSlotIndex == -1 && (uiSlotBag_Drag != null || uiSlotWand_Drag != null))
				{
					gamepadSlotIndex = uiWands[gamepadBagOrWandIndex].AllSlotCount - 1;
				}
				else if (uiWand_Drag != null)
				{
					gamepadSlotIndex = -1;
				}
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
			}
		}
		else
		{
			if (!(direct == Vector2.right))
			{
				return;
			}
			if (gamepadBagOrWandIndex == -1)
			{
				if (gamepadSlotIndexInTidyUpBtn)
				{
					gamepadSlotIndexInTidyUpBtn = false;
					UITidyupButton.Inst.GamepadUnHover();
					gamepadSlotIndex = 0;
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
					return;
				}
				gamepadSlotIndex = Mathf.Clamp(gamepadSlotIndex, 0, uiSlot_Bags.Count - 1);
				uiSlot_Bags[gamepadSlotIndex].OnPointerExit(null);
				gamepadSlotIndex++;
				if (IsDraging && gamepadSlotIndex >= uiSlot_Bags.Count)
				{
					gamepadSlotIndex = 0;
				}
				if (gamepadSlotIndex < uiSlot_Bags.Count)
				{
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
					return;
				}
				gamepadSlotIndex--;
				UITidyupButton.Inst.GamepadHover();
				gamepadSlotIndexInTidyUpBtn = true;
			}
			else
			{
				gamepadSlotIndex = Mathf.Clamp(gamepadSlotIndex, -1, uiWands[gamepadBagOrWandIndex].GetUIAllUISlot().Length - 1);
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerExit(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerExit(null);
				}
				gamepadSlotIndex++;
				if (gamepadSlotIndex >= uiWands[gamepadBagOrWandIndex].AllSlotCount)
				{
					gamepadSlotIndex = -1;
				}
				if (gamepadSlotIndex == -1 && (uiSlotBag_Drag != null || uiSlotWand_Drag != null))
				{
					gamepadSlotIndex = 0;
				}
				else if (uiWand_Drag != null)
				{
					gamepadSlotIndex = -1;
				}
				if (gamepadSlotIndex == -1)
				{
					uiWandEvents[gamepadBagOrWandIndex].OnPointerEnter(null);
				}
				else
				{
					uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).OnPointerEnter(null);
				}
			}
		}
	}

	private void GamepadDpadHoldingUpdate()
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || DataMgr.selectedWorldData.battleData9 == null || !IsBagOpen || HaveUIOpen())
		{
			return;
		}
		(Vector2, float)? holdingDpad = _holdingDpad;
		if (holdingDpad.HasValue && !(Time.time - _holdingDpad.Value.startTime < 0.2f))
		{
			_holdingDpadMoveCD -= Time.deltaTime;
			if (_holdingDpadMoveCD <= 0f)
			{
				GamepadDpadMove(_holdingDpad.Value.direct);
				_holdingDpadMoveCD = 0.05f;
			}
		}
	}

	private void GamepadDpadHoldingStart(Vector2 direct)
	{
		_holdingDpadMoveCD = 0.05f;
		_holdingDpad = (direct, Time.time);
	}

	private void GamepadDpadHoldingCancel()
	{
		_holdingDpad = null;
	}

	private void ClampGamepadIndex()
	{
		gamepadBagOrWandIndex = Mathf.Clamp(gamepadBagOrWandIndex, -1, PlayerMgr.Inst.Wands.Count - 1);
		if (gamepadBagOrWandIndex == -1)
		{
			gamepadSlotIndex = Mathf.Clamp(gamepadSlotIndex, 0, PlayerMgr.Inst.BaData.bagSpellDatas.Count - 1);
			return;
		}
		int num = uiWands[gamepadBagOrWandIndex].GetUIAllUISlot().Length;
		gamepadSlotIndex = Mathf.Clamp(gamepadSlotIndex, -1, num - 1);
	}

	private void GamepadWestStarted(InputAction.CallbackContext context)
	{
		if ((PlayerMgr.Inst != null && PlayerMgr.Inst.PlayerCtrller != null && !PlayerMgr.Inst.PlayerCtrller.CanInteractive && !IsBagOpen) || UIMgr.Inst.InputType != PlayerInputType.Gamepad || (GameMgr.IsMobile_Static && !IsBagOpen) || DataMgr.selectedWorldData.battleData9 == null || HaveUIOpen())
		{
			return;
		}
		if (gamepadSlotIndexInTidyUpBtn)
		{
			UITidyupButton.Inst.OnClick();
			return;
		}
		gamepadBagOrWandIndexDragBefore = gamepadBagOrWandIndex;
		gamepadSlotIndexDragBefore = gamepadSlotIndex;
		gamepadDragDuration = Time.realtimeSinceStartup;
		if (gamepadBagOrWandIndex == -1)
		{
			UISlotBagDragBegin(uiSlot_Bags[gamepadSlotIndex]);
		}
		else if (gamepadSlotIndex == -1)
		{
			UIWandEventDragBegin(uiWands[gamepadBagOrWandIndex]);
		}
		else
		{
			UISlotWandDragBegin(uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex));
		}
	}

	private void GamepadWestCanceled(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || DataMgr.selectedWorldData.battleData9 == null || !IsBagOpen || !IsDraging || HaveUIOpen())
		{
			return;
		}
		if (gamepadBagOrWandIndexDragBefore == gamepadBagOrWandIndex && gamepadSlotIndexDragBefore == gamepadSlotIndex)
		{
			if (Time.realtimeSinceStartup - gamepadDragDuration < 0.3f)
			{
				if (gamepadBagOrWandIndex == -1)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uiSlotBag_Drag.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
					PlayerMgr.Inst.Slot_RemoveBagSlot(uiSlotBag_Drag.BagIndex);
				}
				else if (gamepadSlotIndex == -1)
				{
					if (wandCfg_Drag != null)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, wandCfg_Drag, PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
					}
				}
				else
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uiSlotWand_Drag.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
					PlayerMgr.Inst.ChangeWandSpell(gamepadBagOrWandIndex, gamepadSlotIndex, null);
				}
			}
			else if (gamepadBagOrWandIndex == -1)
			{
				uiSlotBag_Drag.UpdateInfo();
			}
			else if (gamepadSlotIndex == -1)
			{
				PlayerMgr.Inst.WandReset(gamepadBagOrWandIndex, wandCfg_Drag);
			}
			else
			{
				uiSlotWand_Drag.UpdateInfo();
			}
		}
		else if (gamepadBagOrWandIndex == -1)
		{
			if (gamepadBagOrWandIndexDragBefore == -1)
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndBag(gamepadSlotIndex, gamepadSlotIndexDragBefore);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
			else if (gamepadSlotIndexDragBefore != -1)
			{
				var (slotType, wandSlotIndex) = PlayerMgr.Inst.WandSlotIndex2SlotType(gamepadBagOrWandIndexDragBefore, gamepadSlotIndexDragBefore);
				if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(gamepadSlotIndex, gamepadBagOrWandIndexDragBefore, slotType, wandSlotIndex))
				{
					PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(gamepadSlotIndex, gamepadBagOrWandIndexDragBefore, slotType, wandSlotIndex);
					SEMgr.Inst.uiSlotPut.PlaySE();
				}
				else
				{
					WandUpdate(uiSlotWand_Drag.WandIndex);
				}
				uiSlotWand_Drag.UpdateInfo();
			}
		}
		else if (gamepadBagOrWandIndexDragBefore == -1)
		{
			if (uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).IsSlotLock)
			{
				uiSlotBag_Drag.UpdateInfo();
			}
			else
			{
				(WandSlotType type, int indexInTheType) tuple2 = PlayerMgr.Inst.WandSlotIndex2SlotType(gamepadBagOrWandIndex, gamepadSlotIndex);
				WandSlotType item = tuple2.type;
				int item2 = tuple2.indexInTheType;
				WandConfig wandCfg = PlayerMgr.Inst.Wands[gamepadBagOrWandIndex].WandCfg;
				SlotData[] slotsData = wandCfg.GetSlotsData(item);
				bool[] slotsLockState = wandCfg.GetSlotsLockState(item);
				SlotData slotData = slotsData[item2];
				if (slotData != null && slotData.isSealSlot)
				{
					int num = slotsData.Bag_GetOwnerSlotIndex(item2);
					if (slotsData[num].CheckCanOverrideInSlots(slotsData, slotsLockState, item2, isBag: false))
					{
						slotsData[num].OverrideInSlots(slotsData, slotsLockState, item2, isBag: false);
					}
				}
				if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(gamepadSlotIndexDragBefore, gamepadBagOrWandIndex, item, item2))
				{
					PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(gamepadSlotIndexDragBefore, gamepadBagOrWandIndex, item, item2);
					SEMgr.Inst.uiSlotPut.PlaySE();
				}
				uiSlotBag_Drag.UpdateInfo();
			}
		}
		else if (gamepadSlotIndex == -1)
		{
			if (uiWands[gamepadBagOrWandIndex].WandCfg == null)
			{
				PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[gamepadBagOrWandIndexDragBefore]);
				PlayerMgr.Inst.WandReset(gamepadBagOrWandIndex, wandCfg_Drag);
				PlayerMgr.Inst.Wands[gamepadBagOrWandIndex].CurrentMP = dragWandMana.GetValueOrDefault();
			}
			else
			{
				PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[gamepadBagOrWandIndexDragBefore]);
				PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[gamepadBagOrWandIndex]);
				PlayerMgr.Inst.WandReset(gamepadBagOrWandIndexDragBefore, uiWands[gamepadBagOrWandIndex].WandCfg);
				PlayerMgr.Inst.WandReset(gamepadBagOrWandIndex, wandCfg_Drag);
				PlayerMgr.Inst.Wands[gamepadBagOrWandIndex].CurrentMP = dragWandMana.GetValueOrDefault();
			}
		}
		else
		{
			(WandSlotType type, int indexInTheType) tuple3 = PlayerMgr.Inst.WandSlotIndex2SlotType(gamepadBagOrWandIndex, gamepadSlotIndex);
			WandSlotType item3 = tuple3.type;
			int item4 = tuple3.indexInTheType;
			(WandSlotType type, int indexInTheType) tuple4 = PlayerMgr.Inst.WandSlotIndex2SlotType(gamepadBagOrWandIndexDragBefore, gamepadSlotIndexDragBefore);
			WandSlotType item5 = tuple4.type;
			int item6 = tuple4.indexInTheType;
			WandConfig wandCfg2 = PlayerMgr.Inst.Wands[gamepadBagOrWandIndex].WandCfg;
			SlotData[] slotsData2 = wandCfg2.GetSlotsData(item3);
			bool[] slotsLockState2 = wandCfg2.GetSlotsLockState(item3);
			SlotData slotData = slotsData2[item4];
			if (slotData != null && slotData.isSealSlot)
			{
				int num2 = slotsData2.Bag_GetOwnerSlotIndex(item4);
				if (slotsData2[num2].CheckCanOverrideInSlots(slotsData2, slotsLockState2, item4, isBag: false))
				{
					slotsData2[num2].OverrideInSlots(slotsData2, slotsLockState2, item4, isBag: false);
				}
			}
			SlotData[] slotsData3 = PlayerMgr.Inst.Wands[gamepadBagOrWandIndexDragBefore].WandCfg.GetSlotsData(item5);
			bool[] slotsLockState3 = PlayerMgr.Inst.Wands[gamepadBagOrWandIndexDragBefore].WandCfg.GetSlotsLockState(item5);
			uiSlotWand_Drag.SpellDat.OnWillLeaveSlots(slotsData3, slotsLockState3, isBag: false);
			if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenWandAndWand(gamepadBagOrWandIndex, item3, item4, gamepadBagOrWandIndexDragBefore, item5, item6))
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenWandAndWand(gamepadBagOrWandIndex, item3, item4, gamepadBagOrWandIndexDragBefore, item5, item6);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
			else
			{
				WandUpdate(uiSlotWand_Drag.WandIndex);
			}
			uiSlotWand_Drag.UpdateInfo();
		}
		uiSlotBag_Drag = null;
		uiSlotWand_Drag = null;
		uiWand_Drag = null;
		wandCfg_Drag = null;
		image_SlotDraging.gameObject.SetActive(value: false);
		image_WandDraging.gameObject.SetActive(value: false);
	}

	private void InputChange()
	{
		if (PlayerMgr.Inst.BaData == null)
		{
			return;
		}
		_holdingDpad = null;
		ClampGamepadIndex();
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (gamepadBagOrWandIndex == -1)
			{
				if (uiSlot_Bags.Count != 0)
				{
					uiSlot_Bags[gamepadSlotIndex].OnPointerExit(null);
				}
			}
			else if (gamepadSlotIndex == -1)
			{
				uiWandEvents[gamepadBagOrWandIndex].OnPointerExit(null);
			}
			else
			{
				UIWand uIWand = uiWands[gamepadBagOrWandIndex];
				if ((object)uIWand == null)
				{
					uIWand = uiWands[0];
					gamepadBagOrWandIndex = 0;
				}
				uIWand?.GetUISlotByAllIndex(gamepadSlotIndex).OnPointerExit(null);
			}
			gamepadBagOrWandIndex = -1;
			gamepadSlotIndex = 0;
			break;
		case PlayerInputType.Gamepad:
			if (MainMenuMgr.Inst != null)
			{
				Debug.Log("主菜单运行到这里会报错");
				if (PlayerMgr.Inst.PlayerCtrller != null && PlayerMgr.Inst.PlayerCtrller.CanMotion && !rtsf_BagSpell.IsDestroyed() && IsBagOpen)
				{
					uiSlot_Bags[gamepadSlotIndex].OnPointerEnter(null);
				}
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
		ChontrolChange();
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			menuePosition = goMenuButton.transform.GetComponent<RectTransform>().anchoredPosition;
			playerdata_UpLeft_X = playerdata_UpLeft.transform.localPosition.x;
			playerdata_UpRight_X = playerdata_UpRight.transform.localPosition.x;
			playerdata_DownRight_X = playerdata_DownRight.transform.localPosition.x;
			playerdata_HPMP_X = playerdata_HPMP.transform.localPosition.x;
			playerdata_dropArea_X = goMobileDropArea.transform.localPosition.x;
		}
		bagButtonPosition = image_BagBtn.transform.localPosition;
		InputChange();
		MobileUpdateResource();
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			uiCoinAndKeyCanvasGroup.alpha = ((!Guide2Mgr.Inst) ? 1 : 0);
		}
		MobileUpdateResource();
		currentUILeftDown = playerinfoNormal;
		playerinfoDave.gameObject.SetActive(currentUILeftDown == playerinfoDave);
		playerinfoNormal.gameObject.SetActive(currentUILeftDown == playerinfoNormal);
		if (PlayerMgr.Inst.TryGetPlayerPpt(out playerPptDots))
		{
			currentUILeftDown.PlayerCfg = playerPptDots.unitCfg;
			GamepadDpadHoldingUpdate();
			Drag();
			HoverCheck();
			currentUILeftDown.UpdateHP();
			currentUILeftDown.HPShieldCheck();
			currentUILeftDown.MPCheck();
			UpdateSelectedWandHighlight();
			if (HaveUIOpen())
			{
				CancelDrag();
			}
			if (GameMgr.IsMobile_Static)
			{
				rectUIResource.anchoredPosition = ((uiLayout_Curse.transform.childCount > 0) ? UIResourcePositionMobileWithCurse : UIResourcePositionMobileWithOutCuse);
				UpdateFoldButtonUI();
				UpdateBagImage();
			}
		}
	}

	private void MobileUpdateResource()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		if (_timeShortClick > 0f)
		{
			_timeShortClick -= Time.unscaledDeltaTime;
		}
		if (_doubleClickthr > 0f)
		{
			_doubleClickthr -= Time.unscaledDeltaTime;
		}
		if (BattleMgr.Inst != null || Guide2Mgr.Inst != null)
		{
			if (showTimeCrystal >= 0f)
			{
				showTimeCrystal -= Time.deltaTime;
			}
			if (showTimeBlood >= 0f)
			{
				showTimeBlood -= Time.deltaTime;
			}
			if (showTimeCore >= 0f)
			{
				showTimeCore -= Time.deltaTime;
			}
			if (showTimeGear >= 0f)
			{
				showTimeGear -= Time.deltaTime;
			}
			uiCrystalCountCanvasGroup.alpha = ((showTimeCrystal >= 1f) ? 1f : showTimeCrystal);
			uiBloodCountCanvasGroup.alpha = ((showTimeBlood >= 1f) ? 1f : showTimeBlood);
			uiCoreCountCanvasGroup.alpha = ((showTimeCore >= 1f) ? 1f : showTimeCore);
			uiGearCountCanvasGroup.alpha = ((showTimeGear >= 1f) ? 1f : showTimeGear);
		}
		else
		{
			uiCrystalCountCanvasGroup.alpha = 1f;
			uiBloodCountCanvasGroup.alpha = 1f;
			uiCoreCountCanvasGroup.alpha = 1f;
			uiGearCountCanvasGroup.alpha = 1f;
		}
	}

	private void Drag()
	{
		RectTransform rectTransform = null;
		if (image_SlotDraging.gameObject.activeSelf)
		{
			rectTransform = image_SlotDraging.rectTransform;
		}
		else if (image_WandDraging.gameObject.activeSelf)
		{
			rectTransform = image_WandDraging.rectTransform;
		}
		else if (image_PotionDraging.gameObject.activeSelf)
		{
			rectTransform = image_PotionDraging.rectTransform;
		}
		if (rectTransform == null)
		{
			return;
		}
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
		{
			Vector2 vector = Input.mousePosition;
			if ((GameMgr.IsMobile_Static && ControlMgr.Inst.usingTouchScreen) || GameMgr.IsSteamDeck_Static)
			{
				rectTransform.anchoredPosition = GeneralTool.ScreenPositionToCanvasPosition(vector, UIMgr.Inst.canvas_10, CamController.Inst.cam_UI);
				break;
			}
			vector.x = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * 1080f * (vector.x / (float)Screen.width - 0.5f);
			vector.y = UIMgr.Inst.canvas_1Scaler.referenceResolution.y * (vector.y / (float)Screen.height - 0.5f);
			rectTransform.anchoredPosition = vector;
			break;
		}
		case PlayerInputType.Gamepad:
			if (gamepadBagOrWandIndex == -1)
			{
				rectTransform.transform.position = uiSlot_Bags[gamepadSlotIndex].transform.position + gamepad_DragSlotOffset;
			}
			else if (gamepadSlotIndex == -1)
			{
				rectTransform.transform.position = uiWandEvents[gamepadBagOrWandIndex].transform.position + gamepad_DragWandOffset;
			}
			else
			{
				rectTransform.transform.position = uiWands[gamepadBagOrWandIndex].GetUISlotByAllIndex(gamepadSlotIndex).transform.position + gamepad_DragSlotOffset;
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void HoverCheck()
	{
		if (uiInfoSpellHover.gameObject.activeSelf && uiSlotBag_Hover == null && uiSlotWand_Hover == null)
		{
			if (uiSlotExternal_Hover == null)
			{
				uiInfoSpellHover.gameObject.SetActive(value: false);
			}
			else if (!uiSlotExternal_Hover.gameObject.activeInHierarchy)
			{
				uiSlotExternal_Hover = null;
				uiInfoSpellHover.gameObject.SetActive(value: false);
			}
		}
		if (uiInfoCurseHover.gameObject.activeSelf && uiCurse_Hover == null)
		{
			uiInfoCurseHover.gameObject.SetActive(value: false);
		}
	}

	public void BagShakeButton()
	{
		image_BagBtn.transform.localPosition = bagButtonPosition;
		image_BagBtn.GetComponent<RectTransform>().DOShakePosition(0.3f, 6f, 20).SetUpdate(isIndependentUpdate: true)
			.OnComplete(delegate
			{
				image_BagBtn.transform.localPosition = bagButtonPosition;
			});
	}

	public void Initialize()
	{
		Inst = this;
		rtsf_BagSpell.gameObject.SetActive(value: false);
		image_SlotDraging.gameObject.SetActive(value: false);
		image_WandDraging.gameObject.SetActive(value: false);
		image_PotionDraging.gameObject.SetActive(value: false);
		image_BagBtn.sprite = sprite_BagClose;
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Bag.performed += BagPerformed;
		inputActions.Player.GamepadDpad.performed += GamepadDpadPerformed;
		inputActions.Player.GamepadDpad.canceled += GamepadDpadCanceled;
		inputActions.Player.GamepadWest.performed += GamepadWestStarted;
		inputActions.Player.GamepadWest.canceled += GamepadWestCanceled;
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ChontrolChange));
		EventMgr.RoleItemChange = (Action)Delegate.Combine(EventMgr.RoleItemChange, new Action(OnRoleItemChange));
		UpdateBagImage();
		OnRoleItemChange();
	}

	public void OnDestroy()
	{
		inputActions.Player.Bag.performed -= BagPerformed;
		inputActions.Player.GamepadDpad.performed -= GamepadDpadPerformed;
		inputActions.Player.GamepadDpad.canceled -= GamepadDpadCanceled;
		inputActions.Player.GamepadWest.performed -= GamepadWestStarted;
		inputActions.Player.GamepadWest.canceled -= GamepadWestCanceled;
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ChontrolChange));
		EventMgr.RoleItemChange = (Action)Delegate.Remove(EventMgr.RoleItemChange, new Action(OnRoleItemChange));
	}

	private void ChontrolChange()
	{
		foreach (UpdatButtonShow updatButtonShow in updatButtonShows)
		{
			updatButtonShow.UpdateButton();
		}
	}

	private void OnRoleItemChange()
	{
		bool haveGame = ICJNOGPFMAM.MIFJADDOODN;
		bool haveAllCampSkin = ICJNOGPFMAM.IMFNIOLONJP;
		if (GameMgr.IsMobile_Static)
		{
			fullGameBtn.SetActive(!haveGame);
			buySuitBtn.SetActive(haveGame && !haveAllCampSkin);
			buyGameFX.SetActive(value: false);
		}
	}

	public bool HaveUIOpen()
	{
		if (UIMgr.Inst.uiSetting.IsOpen)
		{
			return true;
		}
		if (UIMgr.Inst.uiFade.IsOpen)
		{
			return true;
		}
		if (GameUISingletonMono<UIChapterThrough>.StaticIsOpen)
		{
			return true;
		}
		switch (SceneManager.GetActiveScene().name)
		{
		case "Guide":
		case "Guide2":
			if (UIGuideMgr.Inst.uiMenu.IsOpen)
			{
				return true;
			}
			return false;
		case "Camp":
			if (UICampMgr.Inst.uiMenu.IsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UITalent>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIResearch>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UISet>.StaticIsOpen)
			{
				return true;
			}
			if (UICampMgr.Inst.uiGallery != null && UICampMgr.Inst.uiGallery.IsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UITraining>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UICampMirror>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UICampSkinChanger>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIActivateGirl>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIResourceChanger>.StaticIsOpen)
			{
				return true;
			}
			return false;
		case "Battle":
			if (GameUISingletonMono<UIBossShow>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UILevelReward>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UICompound>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIReroll>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIMoreInOne>.StaticIsOpen)
			{
				return true;
			}
			if (UIBattleMgr.Inst.uiMenu.IsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UIRerollRelic>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UISell>.StaticIsOpen)
			{
				return true;
			}
			if (GameUISingletonMono<UISpellDisable>.StaticIsOpen)
			{
				return true;
			}
			return false;
		default:
			return true;
		}
	}

	public void Show()
	{
		if (UIMgr.Inst.showbattleui)
		{
			anima_PlayerInfo.SetTrigger("Appear");
			TryUpdateCoinKeyLayoutIfMobile();
		}
	}

	public void ShowDirect()
	{
		if (UIMgr.Inst.showbattleui)
		{
			anima_PlayerInfo.SetTrigger("AppearDirect");
		}
	}

	public void Hide()
	{
		anima_PlayerInfo.SetTrigger("Disappear");
	}

	public void HideDirect()
	{
		anima_PlayerInfo.SetTrigger("DisappearDirect");
	}

	public void WandShow()
	{
		anima_PlayerInfo.SetTrigger("WandAppear");
	}

	public void PotionShow()
	{
		anima_PlayerInfo.SetTrigger("PotionAppear");
	}

	public void UpdateAllInfo()
	{
		UpdateHP();
		UpdateShield();
		UpdateShieldTemp();
		UpdateCoin();
		UpdateKey();
		UpdateGear();
		UpdateMagicCrystal();
		UpdateAncientBlood();
		UpdateChaosCore();
		WandReset();
		UpdateBag();
		uiPotionsCtrller.CheckCountAndUpdateAllUI();
		RelicUpdate();
		CurseUpdate();
		TopUI.inst.SetSelectedWandDirty();
		RecorrectHPMPShieldWidthDirect();
	}

	public void UpdateCoin()
	{
		text_CoinCount.text = "×" + PlayerMgr.Inst.BaData.coinCount;
		TryUpdateCoinKeyLayoutIfMobile();
	}

	public void UpdateKey()
	{
		if (!ShowEndlessInfo)
		{
			panel_Key.SetActive(value: true);
			text_KeyCount.text = "×" + PlayerMgr.Inst.BaData.keyCount;
		}
		else
		{
			panel_Key.SetActive(value: false);
		}
	}

	private void TryUpdateCoinKeyLayoutIfMobile()
	{
		if (GameMgr.IsMobile_Static)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(rtstCoinAndKey);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rtstCoinAndKey);
		}
	}

	public void UpdateGear()
	{
		if (ShowEndlessInfo)
		{
			panel_Gear.SetActive(value: true);
			if (GameMgr.IsMobile_Static && (bool)panel_Gear_Mobile)
			{
				panel_Gear_Mobile.SetActive(value: true);
			}
			text_GearCount.text = "×" + DataMgr.selectedWorldData.GearCount;
			MobileShowResource(4);
		}
		else
		{
			if (GameMgr.IsMobile_Static && (bool)panel_Gear_Mobile)
			{
				panel_Gear_Mobile.SetActive(value: false);
			}
			panel_Gear.SetActive(value: false);
		}
	}

	public void UpdateMagicCrystal()
	{
		if (!ShowEndlessInfo)
		{
			panel_Crystal.SetActive(value: true);
			text_GearCount.text = "×" + DataMgr.selectedWorldData.GearCount;
			MobileShowResource(1);
		}
		else
		{
			panel_Crystal.SetActive(value: false);
		}
		text_MagicCrystalCount.text = "×" + DataMgr.selectedWorldData.magicCrystalCount;
	}

	public void UpdateAncientBlood()
	{
		if (DataMgr.selectedWorldData.hadBlood && !ShowEndlessInfo)
		{
			panel_AncientBlood.SetActive(value: true);
			text_AncientBloodCount.text = "×" + DataMgr.selectedWorldData.ancientBloodCount;
			MobileShowResource(2);
		}
		else
		{
			panel_AncientBlood.SetActive(value: false);
		}
	}

	public void UpdateChaosCore()
	{
		if (DataMgr.selectedWorldData.hadCore && !ShowEndlessInfo)
		{
			panel_ChaosCore.SetActive(value: true);
			text_ChaosCoreCount.text = "×" + DataMgr.selectedWorldData.chaosCoreCount;
			MobileShowResource(3);
		}
		else
		{
			panel_ChaosCore.SetActive(value: false);
		}
	}

	public void MouseHoverHPMP(UIPlayerSliderType sliderType, bool isHover)
	{
		switch (sliderType)
		{
		case UIPlayerSliderType.HP:
			isHoverHP = isHover;
			currentUILeftDown.UpdateHP();
			break;
		case UIPlayerSliderType.MP:
			isHoverMP = isHover;
			break;
		default:
			Debug.LogError(sliderType);
			break;
		}
	}

	public void WandReset()
	{
		rtsf_Wand.DestroyAllChild();
		uiInfoWandHover.gameObject.SetActive(value: false);
		uiWands.Clear();
		uiWandEvents.Clear();
		for (int i = 0; i < PlayerMgr.Inst.BaData.wandCfgs.Count; i++)
		{
			UIWand component = UnityEngine.Object.Instantiate(pfb_UIWandDetail, rtsf_Wand).GetComponent<UIWand>();
			component.Initialize(i);
			uiWands.Add(component);
			uiWandEvents.Add(component.uiWandEvent);
		}
		ResetWandUIScaleFit();
		if (GameMgr.IsMobile_Static)
		{
			MobileUpdateWandFold();
			DOVirtual.DelayedCall(0.2f, Inst.MobileUpdateWandFold);
			DOVirtual.DelayedCall(1f, Inst.MobileUpdateWandFold);
		}
		else
		{
			UpdateWandLayout();
		}
	}

	public void UpdateWandLayout()
	{
		int num = 0;
		for (int i = 0; i < uiWands.Count; i++)
		{
			if (!uiWands[i].CanvasGroup || Mathf.Approximately(uiWands[i].CanvasGroup.alpha, 1f))
			{
				num++;
			}
			if (GameMgr.IsMobile_Static)
			{
				if (GameUISingletonMono<UILevelReward>.StaticIsOpen && GameUISingletonMono<UILevelReward>.Inst.type == LevelRewardType.Wand && GameUISingletonMono<UILevelReward>.Inst.isShowingWand)
				{
					rtsf_Wand.anchoredPosition = uiWandPositionBagOpen;
					uiWands[i].rtsf_Self.anchoredPosition = new Vector2(40f, (float)(-(num - 1)) * wandDetailSpace) + wandOffsetStartMobile;
				}
				else if (!IsBagOpen)
				{
					rtsf_Wand.anchoredPosition = uiWandPositionBagClosed;
					uiWands[i].rtsf_Self.anchoredPosition = new Vector2((float)(num - 1) * wandDetailSpace, 0f);
				}
				else
				{
					rtsf_Wand.anchoredPosition = uiWandPositionBagOpen;
					uiWands[i].rtsf_Self.anchoredPosition = new Vector2(0f, (float)(-(num - 1)) * wandDetailSpace) + wandOffsetStartMobile;
				}
			}
			else
			{
				uiWands[i].rtsf_Self.anchoredPosition = new Vector2(0f, (float)(-i) * wandDetailSpace);
			}
		}
		ResetWandUIScaleFit();
	}

	public void ResetWandUIScaleFit()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		if (IsBagOpen)
		{
			uiBagButton1.localScale = Vector3.one * bagScaleOpened;
			uiBagButton1.anchoredPosition = bagPositionOpened;
			return;
		}
		uiBagButton1.localScale = Vector3.one * bagScaleClosed;
		uiBagButton1.anchoredPosition = bagPositionClosed;
		foreach (UIWand uiWand in uiWands)
		{
			uiWand.rtsf_Spells.localScale = new Vector3(1f, 1f, 1f);
			uiWand.rtsf_SlotsBG.localScale = new Vector3(1f, 1f, 1f);
		}
		Inst.uiWand.localScale = new Vector3(MobileMgr.inst.uiLeftUpZoomout, MobileMgr.inst.uiLeftUpZoomout, 1f);
		ChangeUiBagScale(MobileMgr.inst.uiBagZoominMaxMobile);
	}

	public void WandUpdate(int wandIndex)
	{
		if ((bool)uiWands[wandIndex])
		{
			uiWands[wandIndex].UpdateInfo();
		}
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateChangeWandButton();
		}
	}

	public void WandPostSlotUpdate(int wandIndex, List<int> PostShootIndexs, float chargerPercent)
	{
		uiWands[wandIndex].UpdatePostShoot(PostShootIndexs, chargerPercent);
	}

	[CanBeNull]
	public UISlotWand WandSetSlotIconVisualEffect(int wandIndex, int spellIndex, WandSlotType slotType, WandSlotIconVisualEffect type)
	{
		UISlotWand uISlot = uiWands[wandIndex].GetUISlot(slotType, spellIndex);
		if (!uISlot)
		{
			return null;
		}
		uISlot.SetIconEffect(type);
		return uISlot;
	}

	public UISlotWand[] GetWandUISlotWands(int wandIndex)
	{
		return uiWands[wandIndex].GetUIAllUISlot();
	}

	public void UpdateWandManaPercent(int wandIndex, float percent)
	{
		uiWands[wandIndex].UpdateManaPercent(percent);
	}

	[CanBeNull]
	public UISlotWand WandGetUISlot(int wandIndex, int spellIndex, WandSlotType slotType)
	{
		if (wandIndex < 0)
		{
			return null;
		}
		return uiWands[wandIndex].GetUISlot(slotType, spellIndex);
	}

	public void UIWandEventEnter(UIWand uiwd)
	{
		if (image_SlotDraging.gameObject.activeSelf || image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		if (image_WandDraging.gameObject.activeSelf)
		{
			uiwd.UpdateManaPercent(0f);
			uiwd.Hover();
			return;
		}
		uiwd.Hover();
		if (GameMgr.IsMobile_Static)
		{
			if (Inst.IsBagOpen)
			{
				ShowWandInfo();
			}
			else
			{
				ShowWandInfo();
			}
		}
		else
		{
			ShowWandInfo();
		}
		void ShowWandInfo()
		{
			if (uiwd.WandCfg != null)
			{
				uiInfoWandHover.rtsf_Self.position = uiwd.image_Icon.transform.position + infoYOffsetWand;
				uiInfoWandHover.gameObject.SetActive(value: true);
				uiInfoWandHover.UpdateInfo(PlayerMgr.Inst.Wands[uiwd.WandIndex], null, ChangeAlpha: false);
				uiInfoWandHover.canvasGroup.alpha = 0f;
				if (GameMgr.IsMobile_Static)
				{
					UIMgr.AutoPivot(uiwd.transform.position, uiInfoWandHover.GetComponent<RectTransform>(), uiWandfPivotOffset, useNewPivot: false, uiWandOffsetMobile, uiWandfOffsetMobileAutoMirror, StickEdge: true, 2, delegate
					{
						uiInfoWandHover.canvasGroup.alpha = 1f;
					});
				}
				else
				{
					StartCoroutine(WaitAndInvokeAction(2, delegate
					{
						uiInfoWandHover.canvasGroup.alpha = 1f;
					}));
				}
			}
		}
	}

	public void UIWandEventEnterBuild(UIWand uiwd, WandConfig wandconfig)
	{
		if (image_SlotDraging.gameObject.activeSelf || image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		if (image_WandDraging.gameObject.activeSelf)
		{
			uiwd.UpdateManaPercent(0f);
			uiwd.Hover();
			return;
		}
		uiwd.Hover();
		if (wandconfig == null)
		{
			return;
		}
		uiInfoWandHover.rtsf_Self.position = uiwd.image_Icon.transform.position + infoYOffsetWand;
		uiInfoWandHover.gameObject.SetActive(value: true);
		uiInfoWandHover.UpdateInfo(wandconfig, null, ItemIsStore: false, ChangeAlpha: false);
		uiInfoWandHover.canvasGroup.alpha = 0f;
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.AutoPivot(uiwd.transform.position, uiInfoWandHover.GetComponent<RectTransform>(), uiWandfPivotOffset, useNewPivot: false, uiWandOffsetMobile, uiWandfOffsetMobileAutoMirror, StickEdge: true, 2, delegate
			{
				uiInfoWandHover.canvasGroup.alpha = 1f;
			});
		}
		else
		{
			StartCoroutine(WaitAndInvokeAction(2, delegate
			{
				uiInfoWandHover.canvasGroup.alpha = 1f;
			}));
		}
	}

	public void UIWandEventExit(UIWand uiwd)
	{
		uiwd.Unhover();
		uiInfoWandHover.gameObject.SetActive(value: false);
	}

	public void UIWandEventDragBegin(UIWand uiWand)
	{
		if (uiWand.WandCfg != null && !IsDraging && !HaveUIOpen())
		{
			TryUpdateDropArea(open: true, highlight: false);
			uiWand_Drag = uiWand;
			wandCfg_Drag = uiWand.WandCfg;
			uiInfoWandHover.gameObject.SetActive(value: false);
			image_WandDraging.gameObject.SetActive(value: true);
			image_WandDraging.sprite = ABResources.LoadAsset<Sprite>(wandCfg_Drag.GetIconPath());
			dragWandMana = PlayerMgr.Inst.Wands[uiWand.WandIndex].CurrentMP;
			PlayerMgr.Inst.Wands[uiWand.WandIndex].ResetWandSlotState();
			PlayerMgr.Inst.WandReset(uiWand.WandIndex, null);
			Inst.MobileUpdateWandFold();
		}
	}

	public void UIWandEventDragEnd()
	{
		if (!image_WandDraging.gameObject.activeSelf)
		{
			return;
		}
		pointerUpEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerUpEventData, list);
		UIWandEvent uIWandEvent = null;
		for (int i = 0; i < list.Count; i++)
		{
			uIWandEvent = list[i].gameObject.GetComponent<UIWandEvent>();
			if (uIWandEvent != null)
			{
				break;
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			if (uIWandEvent == null)
			{
				uIWandEvent = uiWand_Drag.uiWandEvent;
			}
			if (list.Any((RaycastResult t) => t.gameObject == goMobileDropArea.gameObject))
			{
				uIWandEvent = null;
			}
		}
		if (uIWandEvent == null)
		{
			Wand wand = PlayerMgr.Inst.Wands[uiWand_Drag.WandIndex];
			if (wand.IsCharging)
			{
				wand.ReleaseCharge();
			}
			PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[uiWand_Drag.WandIndex]);
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, wandCfg_Drag, navMeshPointIngoreZ);
		}
		else if (uIWandEvent.uiWand.WandCfg == null)
		{
			PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[uiWand_Drag.WandIndex]);
			PlayerMgr.Inst.WandReset(uIWandEvent.uiWand.WandIndex, wandCfg_Drag);
			PlayerMgr.Inst.Wands[uIWandEvent.uiWand.WandIndex].CurrentMP = dragWandMana.GetValueOrDefault();
			dragWandMana = null;
		}
		else
		{
			PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[uiWand_Drag.WandIndex]);
			PlayerMgr.Inst.CancelAutoControlWand(PlayerMgr.Inst.Wands[uIWandEvent.uiWand.WandIndex]);
			float currentMP = PlayerMgr.Inst.Wands[uIWandEvent.uiWand.WandIndex].CurrentMP;
			ExchangeWandPosition(uiWand_Drag.WandIndex, wandCfg_Drag, uIWandEvent.uiWand.WandIndex, uIWandEvent.uiWand.WandCfg);
			PlayerMgr.Inst.Wands[uiWand_Drag.WandIndex].CurrentMP = currentMP;
			PlayerMgr.Inst.Wands[uIWandEvent.uiWand.WandIndex].CurrentMP = dragWandMana.GetValueOrDefault();
		}
		Inst.MobileUpdateWandFold();
		DOVirtual.DelayedCall(0.2f, MobileUpdateWandFold);
		image_WandDraging.gameObject.SetActive(value: false);
		uiWand_Drag = null;
	}

	private void WandAutoSpellRefresh(Wand wand)
	{
		wand.RefreshHammer();
		wand.RefreshLaserBeam();
		wand.RefreshBiAnBlades();
		wand.RefreshUmbrella();
	}

	private void ExchangeWandPosition(int index1, WandConfig cfg1, int index2, WandConfig cfg2)
	{
		PlayerMgr.Inst.CancelWandAutoSpell(PlayerMgr.Inst.Wands[index1]);
		PlayerMgr.Inst.CancelWandAutoSpell(PlayerMgr.Inst.Wands[index2]);
		PlayerMgr.Inst.WandSetConfigWithoutRefresh(index1, cfg2);
		PlayerMgr.Inst.WandSetConfigWithoutRefresh(index2, cfg1);
		PlayerMgr.Inst.WandRefreshDataByIndex(index1);
		PlayerMgr.Inst.WandRefreshDataByIndex(index2);
		WandAutoSpellRefresh(PlayerMgr.Inst.Wands[index1]);
		WandAutoSpellRefresh(PlayerMgr.Inst.Wands[index2]);
	}

	public void UpdateBag(int index = -1)
	{
		StartCoroutine(UpdateBagIE(index));
	}

	private IEnumerator UpdateBagIE(int index = -1)
	{
		if (index >= uiSlot_Bags.Count)
		{
			yield break;
		}
		if (index == -1)
		{
			if (uiSlot_Bags.Count != PlayerMgr.Inst.BaData.bagCount)
			{
				rtsf_BagSpell.DestroyAllChild();
				uiSlot_Bags.Clear();
				for (int i = 0; i < PlayerMgr.Inst.BaData.bagCount; i++)
				{
					UISlotBag component = UnityEngine.Object.Instantiate(pfb_UISlot, rtsf_BagSpell).GetComponent<UISlotBag>();
					component.Initialize(i);
					uiSlot_Bags.Add(component);
				}
				yield return null;
				uiLayout_Spell.Layout();
			}
			else
			{
				for (int j = 0; j < uiSlot_Bags.Count; j++)
				{
					uiSlot_Bags[j].UpdateInfo();
				}
			}
		}
		else
		{
			uiSlot_Bags[index].UpdateInfo();
		}
	}

	public UISlotBag GetUISlotBag(int slotIndex)
	{
		if (slotIndex >= uiSlot_Bags.Count)
		{
			return null;
		}
		return uiSlot_Bags[slotIndex];
	}

	public void MobileUIBagOpenOrClose()
	{
		if ((bool)PlayerMgr.Inst.PlayerCtrller && PlayerMgr.Inst.PlayerCtrller.CanMotion)
		{
			BagOpenOrClose();
		}
	}

	public void MobileUICloseIfOpen()
	{
		if (IsBagOpen)
		{
			BagClose();
		}
	}

	public void BagOpenOrClose()
	{
		SEMgr.Inst.UIBagOpenClose.PlaySE();
		if (IsBagOpen)
		{
			BagClose();
		}
		else
		{
			BagOpen();
		}
	}

	public void BagClose()
	{
		if (!UIMgr.Inst.UIMenu.IsOpen && !UIMgr.Inst.uiSetting.IsOpen)
		{
			ForceBagClose();
		}
	}

	public void ForceBagClose()
	{
		if (!IsBagOpen)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			CanvasLeftUp.sortingOrder = 1;
			CanvasLeftUp.overrideSorting = false;
			CanvasDrag.sortingOrder = 1;
			CanvasWandDrag.sortingOrder = 1;
			uiPlayerInfoBGAnimator.Play("UIPlayerInfoBGHide");
			GameMgr.Inst.playerMgr.PlayerCtrller.StartMotion();
			TimeScaleMgr.Inst.Recovery();
			uiBagButton1.localScale = Vector3.one * bagScaleClosed;
			uiBagButton1.anchoredPosition = bagPositionClosed;
			MobileChangeSpellRecover();
			ResetDragMobileHighlight();
		}
		rtsf_BagSpell.gameObject.SetActive(value: false);
		UpdateBagImage();
		MobileUpdateWandFold();
		foreach (UIWand uiWand in uiWands)
		{
			uiWand.Close();
		}
		UpdateWandLayout();
		ResetWandUIScaleFit();
		if (uiSlotBag_Drag != null)
		{
			uiSlotBag_Drag.UpdateInfo();
			uiSlotBag_Drag = null;
			image_SlotDraging.gameObject.SetActive(value: false);
		}
		if (uiSlotWand_Drag != null)
		{
			uiSlotWand_Drag.UpdateInfo();
			uiSlotWand_Drag = null;
			image_SlotDraging.gameObject.SetActive(value: false);
		}
		uiInfoSpellHover.gameObject.SetActive(value: false);
		if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			uiInfoWandHover.gameObject.SetActive(value: false);
		}
		if (uiSlotWand_Hover != null)
		{
			uiSlotWand_Hover.focusingThisSlot = false;
			UISlotWandExit(uiSlotWand_Hover);
		}
	}

	public void BagOpen()
	{
		if (UIMgr.Inst.UIMenu.IsOpen || UIMgr.Inst.uiSetting.IsOpen)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			CanvasLeftUp.overrideSorting = true;
			CanvasLeftUp.sortingOrder = 4;
			CanvasDrag.sortingOrder = 5;
			CanvasWandDrag.sortingOrder = 5;
			uiPlayerInfoBGAnimator.Play("UIPlayerInfoBGShow");
			GameMgr.Inst.playerMgr.PlayerCtrller.StopMotion();
			TimeScaleMgr.Inst.Pause();
			uiBagButton1.localScale = Vector3.one * bagScaleOpened;
			uiBagButton1.anchoredPosition = bagPositionOpened;
		}
		rtsf_BagSpell.gameObject.SetActive(value: true);
		image_BagBtn.sprite = sprite_BagOpen;
		foreach (UISlotBag uiSlot_Bag in uiSlot_Bags)
		{
			uiSlot_Bag.Unhover();
		}
		uiWands.ForEach(delegate(UIWand x)
		{
			x.Open();
		});
		MobileUpdateWandFold();
		UpdateWandLayout();
		UpdateBagUiSizeMobile();
		UpdateBagImage();
	}

	public void UpdateBagImage()
	{
		image_BagBtn.sprite = (IsBagOpen ? sprite_BagOpen : sprite_BagClose);
		if (GameMgr.IsMobile_Static && PlayerMgr.Inst.IsBag100Full)
		{
			image_BagBtn.sprite = (IsBagOpen ? sprite_MobileBagOpenFull : sprite_MobileBagCloseFull);
		}
	}

	public void UpdateBagUiSizeMobile()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (!Inst.IsBagOpen)
			{
				return;
			}
			if (uiWands.All((UIWand x) => x.rtsf_Spells.childCount == 0))
			{
				uiWand.localScale = new Vector3(MobileMgr.inst.uiLeftUpZoominMax, MobileMgr.inst.uiLeftUpZoominMax, 1f);
			}
			else
			{
				foreach (UIWand uiWand in uiWands)
				{
					float num = 0f;
					if (uiWand.rtsf_Spells.childCount != 0)
					{
						float x2 = uiWand.rtsf_Spells.GetChild(uiWand.rtsf_Spells.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x;
						if (x2 > num)
						{
							num = x2;
						}
						num += adjustWandSize1;
						float num2 = (UIMgr.Inst.rtsf_Canvas1.rect.width - playerdata_UpLeft.transform.localPosition.x + adjustWandSize2 + adjustWandSize3 * MobileMgr.inst.uiLeftUpZoominMax + currentBattleUIOffset * positionAdjustRatio) / (num * MobileMgr.inst.uiLeftUpZoominMax);
						if (num2 < 1f)
						{
							this.uiWand.localScale = new Vector3(MobileMgr.inst.uiLeftUpZoominMax, MobileMgr.inst.uiLeftUpZoominMax, 1f);
							uiWand.rtsf_Spells.localScale = new Vector3(num2, num2, 1f);
							uiWand.rtsf_SlotsBG.localScale = new Vector3(num2, num2, 1f);
						}
						else
						{
							this.uiWand.localScale = new Vector3(MobileMgr.inst.uiLeftUpZoominMax, MobileMgr.inst.uiLeftUpZoominMax, 1f);
							uiWand.rtsf_Spells.localScale = new Vector3(1f, 1f, 1f);
							uiWand.rtsf_SlotsBG.localScale = new Vector3(1f, 1f, 1f);
						}
						uiWand.UpdateWandBG();
					}
				}
			}
			float num3 = rtsf_BagSpell.GetChild(rtsf_BagSpell.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x + adjustWandSize + playerdata_UpLeft.transform.localPosition.x + adjustBagSize2;
			float num4 = (UIMgr.Inst.rtsf_Canvas1.rect.width + adjustBagSize) / num3;
			rectTidyUpPenel.anchoredPosition = new Vector2(num3, rectTidyUpPenel.anchoredPosition.y);
			ChangeUiBagScale((num4 < MobileMgr.inst.uiBagZoominMaxMobile) ? num4 : MobileMgr.inst.uiBagZoominMaxMobile);
			return;
		}
		foreach (UIWand uiWand2 in uiWands)
		{
			float num5 = 0f;
			if (uiWand2.rtsf_Spells.childCount != 0)
			{
				float x3 = uiWand2.rtsf_Spells.GetChild(uiWand2.rtsf_Spells.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x;
				if (x3 > num5)
				{
					num5 = x3;
				}
			}
			num5 += adjustWandSize1;
			float num6 = (UIMgr.Inst.rtsf_Canvas1.rect.width + adjustWandSize2) / num5;
			if (num6 < 1f)
			{
				uiWand2.rtsf_Spells.localScale = new Vector3(num6, num6, 1f);
				uiWand2.rtsf_SlotsBG.localScale = new Vector3(num6, num6, 1f);
			}
			else
			{
				uiWand2.rtsf_Spells.localScale = new Vector3(1f, 1f, 1f);
				uiWand2.rtsf_SlotsBG.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		float num7 = rtsf_BagSpell.GetChild(rtsf_BagSpell.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x + adjustWandSize + playerdata_UpLeft.transform.localPosition.x;
		float num8 = (UIMgr.Inst.rtsf_Canvas1.rect.width + adjustBagSize) / num7;
		if (num8 < MobileMgr.inst.uiBagZoominMaxPC)
		{
			ChangeUiBagScale(num8);
		}
		else
		{
			ChangeUiBagScale(MobileMgr.inst.uiBagZoominMaxPC);
		}
	}

	private void ChangeUiBagScale(float scale)
	{
		Inst.uiBag.localScale = new Vector3(scale, scale, 1f);
		TidyUpPenel.transform.localScale = new Vector3(scale, scale, 1f);
	}

	public void LowerWandEventOrder()
	{
		for (int i = 0; i < uiWands.Count; i++)
		{
			uiWands[i].uiWandEvent.GetComponent<Canvas>().sortingOrder = 1;
		}
	}

	public void RaiseWandEventOrder()
	{
		for (int i = 0; i < uiWands.Count; i++)
		{
			uiWands[i].uiWandEvent.GetComponent<Canvas>().sortingOrder = 20;
		}
	}

	public void BagCheckRelicPandorasBoxImage()
	{
		int num = 0;
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox != null)
		{
			num = PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox.int1.result;
		}
		for (int i = 0; i < uiSlot_Bags.Count; i++)
		{
			uiSlot_Bags[i].SetPandoraBoxEffect(i < num);
		}
	}

	public void BagBGToDefault()
	{
		image_BagSpellBG.sprite = sprite_BagSpellBGDefault;
	}

	public void BagBGToReaper()
	{
		image_BagSpellBG.sprite = sprite_BagSpellBGReaper;
	}

	public void RelicUpdate()
	{
		RelicUpdateIE();
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateActiveSkillButton();
		}
	}

	private void RelicUpdateIE()
	{
		uiLayout_Relic.transform.DestroyAllChild();
		uiInfoRelicHover.gameObject.SetActive(value: false);
		for (int i = 0; i < PlayerMgr.Inst.BaData.relicCfgs.Count; i++)
		{
			UnityEngine.Object.Instantiate(pfb_UIRelic, uiLayout_Relic.transform).GetComponent<UIRelic>().Initialize(i);
		}
	}

	public void RelicUpdateAppearanceIcon()
	{
		for (int i = 0; i < uiLayout_Relic.transform.childCount; i++)
		{
			UIRelic component = uiLayout_Relic.transform.GetChild(i).GetComponent<UIRelic>();
			component.disableRelicShowTip.SetActive(DataMgr.settingData.DisableRelicSkins.Contains(component.RelicCfg.id));
		}
	}

	public void RelicFlyCountNewAdd(int id)
	{
		foreach (int item in flyingRelicIdOnlyNew)
		{
			if (item == id)
			{
				return;
			}
		}
		flyingRelicIdOnlyNew.Add(id);
	}

	public void RelicFlyCountAdd(int id)
	{
		flyingRelicId.Add(id);
	}

	public void RelicFlyCountSub(int id)
	{
		flyingRelicId.Remove(id);
	}

	public void RelicFlyCountNewSub(int id)
	{
		for (int i = 0; i < flyingRelicIdOnlyNew.Count; i++)
		{
			if (flyingRelicIdOnlyNew[i] == id)
			{
				flyingRelicIdOnlyNew.RemoveAt(i);
				break;
			}
		}
	}

	private Vector3 CalculateNewItemPosition(GridLayoutGroup layout, GameObject prefab, int count)
	{
		List<GameObject> list = new List<GameObject>();
		layout.enabled = true;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab, layout.transform);
			gameObject.transform.SetParent(layout.transform, worldPositionStays: false);
			list.Add(gameObject);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
		Vector3 position = list[list.Count - 1].transform.position;
		for (int j = 0; j < count; j++)
		{
			UnityEngine.Object.Destroy(list[j].gameObject);
		}
		return position;
	}

	public Vector3 GetNextRelicPosition(int relicID)
	{
		for (int i = 0; i < flyingRelicIdOnlyNew.Count; i++)
		{
			if (flyingRelicIdOnlyNew[i] == relicID)
			{
				return CalculateNewItemPosition(uiLayout_Relic, pfb_UIRelic, i + 1);
			}
		}
		return CalculateNewItemPosition(uiLayout_Relic, pfb_UIRelic, flyingRelicIdOnlyNew.Count);
	}

	public Vector3 GetObtainedRelicPosition(int num)
	{
		return uiLayout_Relic.transform.GetChild(num).position;
	}

	public void UIRelicEnter(UIRelic uiRelic)
	{
		uiRelic_Hover = uiRelic;
		uiRelic.Hover();
		uiInfoRelicHover.gameObject.SetActive(value: true);
		uiInfoRelicHover.GetComponent<RectTransform>().pivot = new Vector2(1f, 1f);
		uiInfoRelicHover.UpdateInfo(uiRelic.RelicCfg, null, upgrade: false, showHideSkinTip: true, showRelicGroupInfo: true);
		uiInfoRelicHover.transform.position = uiRelic.transform.position + infoYOffsetBlessing;
		StartCoroutine(WaitAndInvokeAction(2, delegate
		{
			UIMgr.InteractiveFollowFitSelf(uiInfoRelicHover.gameObject, uiInfoRelicHover.GetComponent<RectTransform>().pivot);
		}));
	}

	public UIInfoRelic UIRelicEnterBuildShow(UIRelic uiRelic, RelicConfig config, bool showSkinText)
	{
		uiRelic_Hover = uiRelic;
		uiRelic.Hover();
		uiInfoRelicHover.gameObject.SetActive(value: true);
		uiInfoRelicHover.UpdateInfo(config, null, upgrade: false, showSkinText, showRelicGroupInfo: true);
		UIMgr.AutoPivotFix(uiRelic.transform.position, uiInfoRelicHover.GetComponent<RectTransform>(), new Vector2(1f, 1f), useNewPivot: true, UIMgr.Inst.UIMenu.uiRelicInfoPositionOffset, UIMgr.Inst.UIMenu.uiRelicInfoPositionOffsetAuto);
		return uiInfoRelicHover;
	}

	public void UIRelicExit()
	{
		if ((bool)uiRelic_Hover)
		{
			UIRelicExit(uiRelic_Hover);
		}
	}

	public void UIRelicExit(UIRelic uiRelic)
	{
		uiRelic_Hover = null;
		uiRelic.Unhover();
		uiInfoRelicHover.gameObject.SetActive(value: false);
	}

	public void CurseUpdate()
	{
		StartCoroutine(CurseUpdateIE());
	}

	private IEnumerator CurseUpdateIE()
	{
		uiLayout_Curse.transform.DestroyAllChild();
		uiInfoCurseHover.gameObject.SetActive(value: false);
		for (int i = 0; i < PlayerMgr.Inst.BaData.curseIDs.Count; i++)
		{
			UnityEngine.Object.Instantiate(pfb_UICurse, uiLayout_Curse.transform).GetComponent<UICurse>().Initialize(i);
		}
		if ((float)PlayerMgr.Inst.BaData.curseIDs.Count * curseUISize > curseUIAreaWidth)
		{
			uiLayout_Curse.space = new Vector2(curseUIAreaWidth / (float)PlayerMgr.Inst.BaData.curseIDs.Count - curseUISize, uiLayout_Curse.space.y);
			uiLayout_Curse.childSize = new Vector2(curseUISize + uiLayout_Curse.space.x / 2f, curseUISize + uiLayout_Curse.space.x / 2f);
			uiLayout_Curse.space = new Vector2(curseUIAreaWidth / (float)PlayerMgr.Inst.BaData.curseIDs.Count - uiLayout_Curse.childSize.x, uiLayout_Curse.space.y);
		}
		else
		{
			uiLayout_Curse.space = Vector2.zero;
			uiLayout_Curse.childSize = new Vector2(curseUISize, curseUISize);
		}
		yield return null;
		if (!GameMgr.IsMobile_Static || uiLayout_Curse.transform.childCount < 9)
		{
			uiLayout_Curse.Layout();
			yield break;
		}
		int childCount = uiLayout_Curse.transform.childCount;
		for (int j = 1; j < childCount; j++)
		{
			Vector2 anchoredPosition = ((RectTransform)uiLayout_Curse.transform.GetChild(j).transform).anchoredPosition;
			anchoredPosition.x = 450 / childCount * j;
			((RectTransform)uiLayout_Curse.transform.GetChild(j).transform).anchoredPosition = anchoredPosition;
		}
	}

	public void UICurseEnter(UICurse uiCurse, int overrideIdFromBuild = -1)
	{
		uiCurse_Hover = uiCurse;
		uiCurse.Hover();
		uiInfoCurseHover.gameObject.SetActive(value: true);
		uiInfoCurseHover.canvasGroup.alpha = 0f;
		uiInfoCurseHover.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
		if (overrideIdFromBuild != -1)
		{
			uiInfoCurseHover.UpdateInfo(overrideIdFromBuild, isPlayerHad: true, uiCurse.buildCurseLevel);
			UIMgr.AutoPivot(uiCurse.transform.position, uiInfoCurseHover.GetComponent<RectTransform>(), new Vector2(1f, 1f), useNewPivot: true, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffset, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffsetAuto);
		}
		else
		{
			uiInfoCurseHover.UpdateInfo(uiCurse.ID, isPlayerHad: true);
			uiInfoCurseHover.transform.position = uiCurse.transform.position + infoYOffsetCurse;
		}
		StartCoroutine(WaitAndInvokeAction(2, delegate
		{
			uiInfoCurseHover.canvasGroup.alpha = 1f;
			UIMgr.InteractiveFollowFitSelf(uiInfoCurseHover.gameObject, uiInfoCurseHover.GetComponent<RectTransform>().pivot);
		}));
	}

	public void UICurseExit()
	{
		if (uiCurse_Hover != null)
		{
			UICurseExit(uiCurse_Hover);
		}
	}

	public void UICurseExit(UICurse uiCurse)
	{
		uiCurse_Hover = null;
		uiCurse.Unhover();
		uiInfoCurseHover.gameObject.SetActive(value: false);
	}

	public void HideAllInfoPanel()
	{
		uiInfoCurseHover.gameObject.SetActive(value: false);
		uiInfoRelicHover.gameObject.SetActive(value: false);
		if ((bool)uiCurse_Hover)
		{
			uiCurse_Hover.Unhover();
			uiCurse_Hover = null;
		}
		if ((bool)uiRelic_Hover)
		{
			uiRelic_Hover.Unhover();
			uiRelic_Hover = null;
		}
		UIMgr.Inst.uiInfoSpellHover.gameObject.SetActive(value: false);
		UIMgr.Inst.uiInfoWandHover.gameObject.SetActive(value: false);
		UIMgr.Inst.uiInfoRelicHover.gameObject.SetActive(value: false);
		UIMgr.Inst.uiInfoCurseHover.gameObject.SetActive(value: false);
	}

	public void UISlotBagExitall()
	{
		for (int i = 0; i < uiSlot_Bags.Count; i++)
		{
			if (uiSlot_Bags[i].GetFocusState())
			{
				uiSlot_Bags[i].OnPointerExit(null);
			}
		}
		for (int j = 0; j < uiWands.Count; j++)
		{
			uiWands[j].PointoutAllSlots();
		}
	}

	public void UISlotBagEnter(UISlotBag slot)
	{
		if (image_WandDraging.gameObject.activeSelf || image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		if (image_SlotDraging.gameObject.activeSelf)
		{
			slot.Hover();
			return;
		}
		uiSlotBag_Hover = slot;
		slot.Hover();
		if (slot.SpellDat == null || slot.SpellDat.isSealSlot)
		{
			return;
		}
		uiInfoSpellHover.gameObject.SetActive(value: true);
		uiInfoSpellHover.UpdateInfo(slot.SpellDat, null, changeAlpha: false);
		uiInfoSpellHover.canvasGroup.alpha = 0f;
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.AutoPivot(slot.transform.position, uiInfoSpellHover.GetComponent<RectTransform>(), uiSpellInfPivotOffsetPercent, useNewPivot: false, uiSpellinfoYOffsetSpellMobile, uiSpellinfoYOffsetSpellMobileAutoMirror, StickEdge: true, 2, delegate
			{
				uiInfoSpellHover.canvasGroup.alpha = 1f;
			});
			return;
		}
		uiInfoSpellHover.transform.position = slot.transform.position + infoYOffsetSpell;
		StartCoroutine(WaitAndInvokeAction(2, delegate
		{
			uiInfoSpellHover.canvasGroup.alpha = 1f;
			UIMgr.InteractiveFollowFitSelf(uiInfoSpellHover.gameObject, uiInfoSpellHover.GetComponent<RectTransform>().pivot);
		}));
	}

	public void UISlotBagExit(UISlotBag slot)
	{
		uiSlotBag_Hover = null;
		slot.Unhover();
		uiInfoSpellHover.gameObject.SetActive(value: false);
	}

	public void UISlotBagDragBegin(UISlotBag slot)
	{
		if (IsBagOpen && slot.SpellDat != null && !slot.SpellDat.isSealSlot && !(uiSlotBag_Drag != null) && !(uiSlotWand_Drag != null))
		{
			uiSlotBag_Drag = slot;
			uiSlotBag_Drag.HideIcon();
			MobileChangeSpellRecover();
			slot.image_IconOutline.gameObject.SetActive(value: false);
			uiInfoSpellHover.gameObject.SetActive(value: false);
			image_SlotDraging.gameObject.SetActive(value: true);
			image_SlotDragingStar1.gameObject.SetActive(value: false);
			image_SlotDragingStar2.gameObject.SetActive(value: false);
			image_SlotDraging.sprite = ABResources.LoadAsset<Sprite>(slot.SpellCfg.GetIconPath());
			if (slot.SpellCfg.level >= 2)
			{
				image_SlotDragingStar1.gameObject.SetActive(value: true);
			}
			if (slot.SpellCfg.level >= 3)
			{
				image_SlotDragingStar2.gameObject.SetActive(value: true);
			}
			TryUpdateDropArea(open: true, highlight: false);
		}
	}

	public void UISlotBagDragEnd()
	{
		if (!image_SlotDraging.gameObject.activeSelf)
		{
			return;
		}
		pointerUpEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerUpEventData, list);
		UISlotBag uISlotBag = null;
		UISlotWand uISlotWand = null;
		if (GameMgr.IsMobile_Static)
		{
			uISlotBag = currentHighlightSlotBag;
			uISlotWand = currentHighlightSlotWand;
		}
		else
		{
			for (int i = 0; i < list.Count; i++)
			{
				uISlotBag = list[i].gameObject.GetComponent<UISlotBag>();
				uISlotWand = list[i].gameObject.GetComponent<UISlotWand>();
				if (uISlotBag != null || uISlotWand != null)
				{
					break;
				}
			}
		}
		if (uISlotBag != null)
		{
			if (uISlotBag == uiSlotBag_Drag)
			{
				uiSlotBag_Drag.UpdateInfo();
			}
			else
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndBag(uISlotBag.BagIndex, uiSlotBag_Drag.BagIndex);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
		}
		else if (uISlotWand != null)
		{
			if (uISlotWand.IsSlotLock)
			{
				uiSlotBag_Drag.UpdateInfo();
			}
			else
			{
				WandConfig wandCfg = PlayerMgr.Inst.Wands[uISlotWand.WandIndex].WandCfg;
				SlotData[] slotsData = wandCfg.GetSlotsData(uISlotWand.SlotType);
				bool[] slotsLockState = wandCfg.GetSlotsLockState(uISlotWand.SlotType);
				SlotData slotData = slotsData[uISlotWand.SpellIndex];
				if (slotData != null && slotData.isSealSlot)
				{
					int num = slotsData.Bag_GetOwnerSlotIndex(uISlotWand.SpellIndex);
					if (slotsData[num].CheckCanOverrideInSlots(slotsData, slotsLockState, uISlotWand.SpellIndex, isBag: false))
					{
						slotsData[num].OverrideInSlots(slotsData, slotsLockState, uISlotWand.SpellIndex, isBag: false);
					}
				}
				if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(uiSlotBag_Drag.BagIndex, uISlotWand.WandIndex, uISlotWand.SlotType, uISlotWand.SpellIndex))
				{
					PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(uiSlotBag_Drag.BagIndex, uISlotWand.WandIndex, uISlotWand.SlotType, uISlotWand.SpellIndex);
					SEMgr.Inst.uiSlotPut.PlaySE();
				}
				else
				{
					uiSlotBag_Drag.ShowIcon();
				}
				uiSlotBag_Drag.UpdateInfo();
			}
		}
		else
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uiSlotBag_Drag.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
			PlayerMgr.Inst.Slot_RemoveBagSlot(uiSlotBag_Drag.BagIndex);
		}
		if (PlayerMgr.Inst.SelectedWand != null && PlayerMgr.Inst.SelectedWand.gameObject.activeInHierarchy)
		{
			PlayerMgr.Inst.SelectedWand.ResetAndRecheck();
		}
		image_SlotDraging.gameObject.SetActive(value: false);
		uiSlotBag_Drag = null;
		ResetDragMobileHighlight();
	}

	public void UISlotBagDragCancel()
	{
		if (image_SlotDraging.gameObject.activeSelf)
		{
			image_SlotDraging.gameObject.SetActive(value: false);
			uiSlotBag_Drag.ShowIcon();
			uiSlotBag_Drag = null;
			ResetDragMobileHighlight();
		}
	}

	public void UISlotWandEnter(UISlotWand slot)
	{
		if (image_WandDraging.gameObject.activeSelf || image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		if (image_SlotDraging.gameObject.activeSelf)
		{
			slot.Hover();
			return;
		}
		uiSlotWand_Hover = slot;
		slot.Hover();
		if ((slot.SpellDat == null && slot.SlotType == WandSlotType.Normal) || (slot.SpellDat != null && slot.SpellDat.isSealSlot))
		{
			return;
		}
		uiInfoSpellHover.gameObject.SetActive(value: true);
		uiInfoSpellHover.UpdateInfo(slot, slot.SpellDat, null, changeAlpha: false);
		uiInfoSpellHover.canvasGroup.alpha = 0f;
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.AutoPivot(slot.transform.position, uiInfoSpellHover.GetComponent<RectTransform>(), uiSpellInfPivotOffsetPercent, useNewPivot: false, uiSpellinfoYOffsetSpellMobile, uiSpellinfoYOffsetSpellMobileAutoMirror, StickEdge: true, 2, delegate
			{
				uiInfoSpellHover.canvasGroup.alpha = 1f;
			});
			return;
		}
		uiInfoSpellHover.transform.position = slot.transform.position + infoYOffsetSpell;
		StartCoroutine(WaitAndInvokeAction(2, delegate
		{
			uiInfoSpellHover.canvasGroup.alpha = 1f;
			UIMgr.InteractiveFollowFitSelf(uiInfoSpellHover.gameObject, uiInfoSpellHover.GetComponent<RectTransform>().pivot);
		}));
	}

	public void UISlotWandExit(UISlotWand slot)
	{
		uiSlotWand_Hover = null;
		slot.Unhover();
		uiInfoSpellHover.gameObject.SetActive(value: false);
	}

	public void UISlotWandDragBegin(UISlotWand slot)
	{
		if (IsBagOpen && slot.SpellDat != null && !slot.isSlotSeal && !(uiSlotBag_Drag != null) && !(uiSlotWand_Drag != null) && !slot.IsSlotLock)
		{
			MobileChangeSpellRecover();
			uiSlotWand_Drag = slot;
			uiSlotWand_Drag.HideIcon();
			uiSlotWand_Drag.image_SpellIconOutline.gameObject.SetActive(value: false);
			uiInfoSpellHover.gameObject.SetActive(value: false);
			image_SlotDraging.gameObject.SetActive(value: true);
			image_SlotDragingStar1.gameObject.SetActive(value: false);
			image_SlotDragingStar2.gameObject.SetActive(value: false);
			image_SlotDraging.sprite = ABResources.LoadAsset<Sprite>(slot.SpellCfg.GetIconPath());
			if (slot.SpellCfg.level >= 2)
			{
				image_SlotDragingStar1.gameObject.SetActive(value: true);
			}
			if (slot.SpellCfg.level >= 3)
			{
				image_SlotDragingStar2.gameObject.SetActive(value: true);
			}
			TryUpdateDropArea(open: true, highlight: false);
		}
	}

	public void UISlotWandDragEnd()
	{
		if (!image_SlotDraging.gameObject.activeSelf)
		{
			return;
		}
		pointerUpEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerUpEventData, list);
		UISlotBag uISlotBag = null;
		UISlotWand uISlotWand = null;
		if (GameMgr.IsMobile_Static)
		{
			uISlotBag = currentHighlightSlotBag;
			uISlotWand = currentHighlightSlotWand;
		}
		else
		{
			for (int i = 0; i < list.Count; i++)
			{
				uISlotBag = list[i].gameObject.GetComponent<UISlotBag>();
				uISlotWand = list[i].gameObject.GetComponent<UISlotWand>();
				if (uISlotBag != null || uISlotWand != null)
				{
					break;
				}
			}
		}
		if (uISlotBag != null)
		{
			if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(uISlotBag.BagIndex, uiSlotWand_Drag.WandIndex, uiSlotWand_Drag.SlotType, uiSlotWand_Drag.SpellIndex))
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(uISlotBag.BagIndex, uiSlotWand_Drag.WandIndex, uiSlotWand_Drag.SlotType, uiSlotWand_Drag.SpellIndex);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
			else
			{
				Inst.WandUpdate(uiSlotWand_Drag.WandIndex);
			}
		}
		else if (uISlotWand != null)
		{
			if (uISlotWand == uiSlotWand_Drag)
			{
				uiSlotWand_Drag.UpdateInfo();
				uiSlotWand_Drag.ShowIcon();
			}
			else if (uISlotWand.IsSlotLock)
			{
				uiSlotWand_Drag.UpdateInfo();
				uiSlotWand_Drag.ShowIcon();
			}
			else
			{
				WandConfig wandCfg = PlayerMgr.Inst.Wands[uISlotWand.WandIndex].WandCfg;
				SlotData[] slotsData = wandCfg.GetSlotsData(uISlotWand.SlotType);
				bool[] slotsLockState = wandCfg.GetSlotsLockState(uISlotWand.SlotType);
				SlotData slotData = slotsData[uISlotWand.SpellIndex];
				if (slotData != null && slotData.isSealSlot)
				{
					int num = slotsData.Bag_GetOwnerSlotIndex(uISlotWand.SpellIndex);
					if (slotsData[num].CheckCanOverrideInSlots(slotsData, slotsLockState, uISlotWand.SpellIndex, isBag: false))
					{
						slotsData[num].OverrideInSlots(slotsData, slotsLockState, uISlotWand.SpellIndex, isBag: false);
					}
				}
				int wandIndex = uiSlotWand_Drag.WandIndex;
				WandSlotType slotType = uiSlotWand_Drag.SlotType;
				SlotData[] slotsData2 = PlayerMgr.Inst.Wands[wandIndex].WandCfg.GetSlotsData(slotType);
				bool[] slotsLockState2 = PlayerMgr.Inst.Wands[wandIndex].WandCfg.GetSlotsLockState(slotType);
				uiSlotWand_Drag.SpellDat.OnWillLeaveSlots(slotsData2, slotsLockState2, isBag: false);
				if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenWandAndWand(uISlotWand.WandIndex, uISlotWand.SlotType, uISlotWand.SpellIndex, uiSlotWand_Drag.WandIndex, uiSlotWand_Drag.SlotType, uiSlotWand_Drag.SpellIndex))
				{
					PlayerMgr.Inst.Slot_SwapSlotBetweenWandAndWand(uISlotWand.WandIndex, uISlotWand.SlotType, uISlotWand.SpellIndex, uiSlotWand_Drag.WandIndex, uiSlotWand_Drag.SlotType, uiSlotWand_Drag.SpellIndex);
					SEMgr.Inst.uiSlotPut.PlaySE();
				}
				uiSlotWand_Drag.UpdateInfo();
				uiSlotWand_Drag.ShowIcon();
			}
		}
		else
		{
			if (SpellConfig.dic[uiSlotWand_Drag.SpellDat.id].abilityType == SpellAbilityType.ManaTendril)
			{
				uiSlotWand_Drag.SpellDat.specialInt = 0;
			}
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uiSlotWand_Drag.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
			PlayerMgr.Inst.ChangeWandSpell(uiSlotWand_Drag.WandIndex, uiSlotWand_Drag.SlotType, uiSlotWand_Drag.SpellIndex, null);
		}
		image_SlotDraging.gameObject.SetActive(value: false);
		uiSlotWand_Drag = null;
		ResetDragMobileHighlight();
	}

	public void UISlotWandDragCancel()
	{
		if (image_SlotDraging.gameObject.activeSelf)
		{
			image_SlotDraging.gameObject.SetActive(value: false);
			uiSlotWand_Drag.ShowIcon();
			uiSlotWand_Drag = null;
			ResetDragMobileHighlight();
		}
	}

	public void ResetDragMobileHighlight()
	{
		currentHighlightSlotBag?.SetUnHighLight();
		currentHighlightSlotWand?.SetUnHighLight();
		currentHighlightSlotBag = null;
		currentHighlightSlotWand = null;
		TryUpdateDropArea(open: false, highlight: false);
		goMobileDropAreaHighLighted.gameObject.SetActive(value: false);
	}

	public void TryUpdateDropArea(bool open, bool highlight)
	{
		if (GameMgr.IsMobile_Static && ControlMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			goMobileDropArea.gameObject.SetActive(open);
			goMobileDropAreaHighLighted.gameObject.SetActive(highlight);
		}
		else
		{
			goMobileDropArea.gameObject.SetActive(value: false);
			goMobileDropAreaHighLighted.gameObject.SetActive(value: false);
		}
	}

	public void UpdateDropAreaHighLight(PointerEventData eventData)
	{
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		TryUpdateDropArea(open: true, list.Any((RaycastResult t) => t.gameObject == goMobileDropArea.gameObject));
	}

	public void FindNearestSlot(PointerEventData eventData)
	{
		float num = float.MaxValue;
		UnityEngine.Object @object = null;
		foreach (UISlotBag uiSlot_Bag in uiSlot_Bags)
		{
			Vector3 worldPoint = GetRectWorldCenter((RectTransform)uiSlot_Bag.transform);
			float num2 = Vector2.Distance(RectTransformUtility.WorldToScreenPoint(CamController.Inst.cam_UI, worldPoint), eventData.position);
			if (num2 < num)
			{
				num = num2;
				@object = uiSlot_Bag;
			}
		}
		foreach (UIWand uiWand in uiWands)
		{
			UISlotWand[] uIAllUISlot = uiWand.GetUIAllUISlot();
			foreach (UISlotWand uISlotWand in uIAllUISlot)
			{
				Vector3 worldPoint2 = GetRectWorldCenter((RectTransform)uISlotWand.transform);
				float num3 = Vector2.Distance(RectTransformUtility.WorldToScreenPoint(CamController.Inst.cam_UI, worldPoint2), eventData.position);
				if (num3 < num)
				{
					num = num3;
					@object = uISlotWand;
				}
			}
		}
		currentHighlightSlotBag?.SetUnHighLight();
		currentHighlightSlotWand?.SetUnHighLight();
		if (@object is UISlotBag uISlotBag)
		{
			uISlotBag.SetHighLight();
			currentHighlightSlotBag = uISlotBag;
			currentHighlightSlotWand = null;
		}
		else if (@object is UISlotWand uISlotWand2)
		{
			uISlotWand2.SetHighLight();
			currentHighlightSlotWand = uISlotWand2;
			currentHighlightSlotBag = null;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		if (list.Any((RaycastResult t) => t.gameObject == goMobileDropArea.gameObject))
		{
			currentHighlightSlotBag?.SetUnHighLight();
			currentHighlightSlotWand?.SetUnHighLight();
			currentHighlightSlotBag = null;
			currentHighlightSlotWand = null;
			goMobileDropAreaHighLighted.gameObject.SetActive(value: true);
		}
		else
		{
			goMobileDropAreaHighLighted.gameObject.SetActive(value: false);
		}
		static Vector3 GetRectWorldCenter(RectTransform rect)
		{
			return rect.TransformPoint(RectTransformUtility.CalculateRelativeRectTransformBounds(rect).center);
		}
	}

	public bool ExChangeSlot(UISlotWand dragStartwand, UISlotBag dragStartBag, UISlotWand dragEndWand, UISlotBag dratEndBag, bool fullCheck = false)
	{
		if ((dragStartwand == null && dragStartBag == null) || (dragStartwand != null && dragStartBag != null) || (dragEndWand == null && dratEndBag == null) || (dragEndWand != null && dratEndBag != null))
		{
			Debug.LogError("错误法术交换");
			return false;
		}
		if (dragStartwand != null)
		{
			if (fullCheck)
			{
				if (dragStartwand.SpellDat == null || dragStartwand.isSlotSeal)
				{
					return false;
				}
				if (uiSlotBag_Drag != null || uiSlotWand_Drag != null)
				{
					return false;
				}
				if (dragStartwand.IsSlotLock)
				{
					return false;
				}
			}
			if (dragEndWand != null)
			{
				if (dragEndWand == dragStartwand)
				{
					dragStartwand.UpdateInfo();
					dragStartwand.ShowIcon();
				}
				else if (dragEndWand.IsSlotLock)
				{
					dragStartwand.UpdateInfo();
					dragStartwand.ShowIcon();
				}
				else
				{
					if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenWandAndWand(dragEndWand.WandIndex, dragEndWand.SlotType, dragEndWand.SpellIndex, dragStartwand.WandIndex, dragStartwand.SlotType, dragStartwand.SpellIndex))
					{
						PlayerMgr.Inst.Slot_SwapSlotBetweenWandAndWand(dragEndWand.WandIndex, dragEndWand.SlotType, dragEndWand.SpellIndex, dragStartwand.WandIndex, dragStartwand.SlotType, dragStartwand.SpellIndex);
						SEMgr.Inst.uiSlotPut.PlaySE();
					}
					dragStartwand.UpdateInfo();
				}
			}
			else if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(dratEndBag.BagIndex, dragStartwand.WandIndex, dragStartwand.SlotType, dragStartwand.SpellIndex))
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(dratEndBag.BagIndex, dragStartwand.WandIndex, dragStartwand.SlotType, dragStartwand.SpellIndex);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
			else
			{
				Inst.WandUpdate(dragStartwand.WandIndex);
			}
		}
		else if (dragStartBag != null)
		{
			if (fullCheck)
			{
				if (dragStartBag.SpellDat == null)
				{
					return false;
				}
				if (dragStartBag.SpellDat.isSealSlot)
				{
					return false;
				}
				if (uiSlotBag_Drag != null || uiSlotWand_Drag != null)
				{
					return false;
				}
			}
			if (dragEndWand != null)
			{
				if (dragEndWand.IsSlotLock)
				{
					uiSlotBag_Drag.UpdateInfo();
				}
				else
				{
					if (PlayerMgr.Inst.Slot_CanSwapSlotBetweenBagAndWand(dragStartBag.BagIndex, dragEndWand.WandIndex, dragEndWand.SlotType, dragEndWand.SpellIndex))
					{
						PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(dragStartBag.BagIndex, dragEndWand.WandIndex, dragEndWand.SlotType, dragEndWand.SpellIndex);
						SEMgr.Inst.uiSlotPut.PlaySE();
					}
					dragStartBag.UpdateInfo();
				}
			}
			else if (dratEndBag == uiSlotBag_Drag)
			{
				dragStartBag.UpdateInfo();
			}
			else
			{
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndBag(dratEndBag.BagIndex, dragStartBag.BagIndex);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
		}
		MobileChangeSpellRecover();
		ResetDragMobileHighlight();
		return true;
	}

	public void MobileChangeSpellRecover()
	{
		if (uislotBagSelected != null)
		{
			uislotBagSelected.image_Icon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			uislotBagSelected.image_IconOutline.gameObject.SetActive(value: false);
			uislotBagSelected.image_IconOutline.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			uislotBagSelected.image_Icon.transform.localScale = Vector3.one;
			uislotBagSelected.image_IconOutline.transform.localScale = Vector3.one;
		}
		if (uislotWandSelected != null)
		{
			uislotWandSelected.image_SpellIcon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			uislotWandSelected.image_SpellIconOutline.gameObject.SetActive(value: false);
			uislotWandSelected.image_SpellIconOutline.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			uislotWandSelected.image_SpellIcon.transform.localScale = Vector3.one;
			uislotWandSelected.image_SpellIconOutline.transform.localScale = Vector3.one;
		}
		uislotWandSelected = null;
		uislotBagSelected = null;
		isChangingSpell = false;
	}

	public void UISlotWandExternalEnter(UISlotWandExternal slot)
	{
		if (image_SlotDraging.gameObject.activeSelf || image_WandDraging.gameObject.activeSelf || image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		uiSlotExternal_Hover = slot;
		slot.Hover();
		if ((slot.SlotType == WandSlotType.Normal && slot.SlotDat == null) || slot.SlotDat == null)
		{
			return;
		}
		uiInfoSpellHover.canvasGroup.alpha = 0f;
		uiInfoSpellHover.gameObject.SetActive(value: true);
		uiInfoSpellHover.UpdateInfoExternal(slot, slot.SlotDat, null, changeAlpha: false);
		if (GameMgr.IsMobile_Static)
		{
			UIMgr.AutoPivot(slot.transform.position, uiInfoSpellHover.GetComponent<RectTransform>(), uiSpellInfPivotOffsetPercent, useNewPivot: false, uiSpellinfoYOffsetSpellMobile, uiSpellinfoYOffsetSpellMobileAutoMirror, StickEdge: true, 2, delegate
			{
				uiInfoSpellHover.canvasGroup.alpha = 1f;
			});
			return;
		}
		uiInfoSpellHover.transform.position = slot.transform.position + infoYOffsetSpell;
		StartCoroutine(WaitAndInvokeAction(2, delegate
		{
			uiInfoSpellHover.canvasGroup.alpha = 1f;
			UIMgr.InteractiveFollowFitSelf(uiInfoSpellHover.gameObject, uiInfoSpellHover.GetComponent<RectTransform>().pivot);
		}));
	}

	private IEnumerator WaitAndInvokeAction(int frameWait, Action action)
	{
		for (int i = 0; i < frameWait; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}

	public void UISlotWandExternalExit(UISlotWandExternal slotExternal)
	{
		uiSlotExternal_Hover = null;
		slotExternal.Unhover();
		uiInfoSpellHover.gameObject.SetActive(value: false);
	}

	public void UISlotPotionDragBegin(UISlotPotion slot)
	{
		if (slot.ID != 0)
		{
			uiSlotPotion_drag = slot;
			uiInfoWandHover.gameObject.SetActive(value: false);
			image_PotionDraging.gameObject.SetActive(value: true);
			image_PotionDraging.sprite = ABResources.LoadAsset<Sprite>("Textures/PotionIcons/" + slot.ID);
		}
	}

	public void UISlotPotionDragEnd()
	{
		if (!image_PotionDraging.gameObject.activeSelf)
		{
			return;
		}
		pointerUpEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerUpEventData, list);
		UISlotPotion uISlotPotion = null;
		for (int i = 0; i < list.Count; i++)
		{
			uISlotPotion = list[i].gameObject.GetComponent<UISlotPotion>();
			if (uISlotPotion != null)
			{
				break;
			}
		}
		if (uISlotPotion == null)
		{
			DropPotion(uiSlotPotion_drag.Index, uiSlotPotion_drag.ID);
		}
		else
		{
			int iD = uISlotPotion.ID;
			PlayerMgr.Inst.ItemCtrller.PotionChange(uISlotPotion.Index, uiSlotPotion_drag.ID);
			PlayerMgr.Inst.ItemCtrller.PotionChange(uiSlotPotion_drag.Index, iD);
		}
		image_PotionDraging.gameObject.SetActive(value: false);
		uiSlotPotion_drag = null;
	}

	public void DropPotion(int index, int id)
	{
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, id), navMeshPointIngoreZ);
		PlayerMgr.Inst.ItemCtrller.PotionChange(index, 0);
	}

	public void DropSelectedSlot()
	{
		if (uislotBagSelected == null && uislotWandSelected == null)
		{
			return;
		}
		if (uislotBagSelected != null)
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uislotBagSelected.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
			PlayerMgr.Inst.Slot_RemoveBagSlot(uislotBagSelected.BagIndex);
		}
		else if (uislotWandSelected != null)
		{
			if (SpellConfig.dic[uislotWandSelected.SpellDat.id].abilityType == SpellAbilityType.ManaTendril)
			{
				uislotWandSelected.SpellDat.specialInt = 0;
			}
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(uislotWandSelected.SpellDat), PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
			PlayerMgr.Inst.ChangeWandSpell(uislotWandSelected.WandIndex, uislotWandSelected.SlotType, uislotWandSelected.SpellIndex, null);
		}
		MobileChangeSpellRecover();
		ResetDragMobileHighlight();
	}

	public void UpdateMobilePosition(float xoffset)
	{
		playerdata_HPMP.transform.localPosition = new Vector3(playerdata_HPMP_X - xoffset * positionAdjustRatio, playerdata_HPMP.transform.localPosition.y, 0f);
		playerdata_UpLeft.transform.localPosition = new Vector3(playerdata_UpLeft_X - xoffset * positionAdjustRatio, playerdata_UpLeft.transform.localPosition.y, 0f);
		playerdata_UpRight.transform.localPosition = new Vector3(playerdata_UpRight_X + xoffset * positionAdjustRatio, playerdata_UpRight.transform.localPosition.y, 0f);
		playerdata_DownRight.transform.localPosition = new Vector3(playerdata_DownRight_X + xoffset * positionAdjustRatio, playerdata_DownRight.transform.localPosition.y, 0f);
		goMobileDropArea.transform.localPosition = new Vector3(playerdata_dropArea_X + 2f * xoffset * positionAdjustRatio, goMobileDropArea.transform.localPosition.y, 0f);
		currentBattleUIOffset = xoffset;
		Inst.UpdateBagUiSizeMobile();
	}

	public void UISlotPotionDragCancel()
	{
		if (image_PotionDraging.gameObject.activeSelf)
		{
			image_PotionDraging.gameObject.SetActive(value: false);
			uiSlotPotion_drag.OnCancelDrag();
			uiSlotPotion_drag = null;
		}
	}

	public void UISlotPotionInfoUpdate()
	{
		if (uiSlotPotion_Hover != null)
		{
			uiInfoPotionHover.UpdateInfo(uiSlotPotion_Hover.ID, null, uiSlotPotion_Hover.isFromBuild);
		}
	}

	private void UpdateSelectedWandHighlight()
	{
		for (int i = 0; i < uiWands.Count; i++)
		{
			if (i == PlayerMgr.Inst.SelectedWandIndex && uiWands[i] != null)
			{
				Wand wand = PlayerMgr.Inst.Wands[i];
				if ((object)wand != null && wand.WandCfg != null)
				{
					uiWands[i].Select();
					continue;
				}
			}
			uiWands[i].Unselect();
		}
	}

	public void UIPlayerInjuredPlay()
	{
		UIMgr.Inst.UIPlayerInjured.animator.SetTrigger("Injured");
	}

	public void ClearDragData()
	{
		uiSlotBag_Drag = null;
		uiSlotWand_Drag = null;
		uiSlotPotion_drag = null;
		uiWand_Drag = null;
		wandCfg_Drag = null;
		image_SlotDraging.gameObject.SetActive(value: false);
		image_WandDraging.gameObject.SetActive(value: false);
		image_PotionDraging.gameObject.SetActive(value: false);
	}

	public void CancelDrag()
	{
		if ((bool)uiSlotPotion_drag)
		{
			UISlotPotionDragCancel();
		}
		if ((bool)uiSlotBag_Drag)
		{
			UISlotBagDragCancel();
		}
		if ((bool)uiSlotWand_Drag)
		{
			UISlotWandDragCancel();
		}
	}

	public void ResourceUISetToDefault(ResourceUIPop popUp, float delay = 0.5f)
	{
		switch (popUp)
		{
		case ResourceUIPop.Crystal:
			CanvasSortingSetToDefault(canvasCrystal);
			break;
		case ResourceUIPop.Blood:
			CanvasSortingSetToDefault(canvasBlood);
			break;
		case ResourceUIPop.Cores:
			CanvasSortingSetToDefault(canvasCores);
			break;
		case ResourceUIPop.Coin:
			CanvasSortingSetToDefault(canvasCoin);
			break;
		case ResourceUIPop.Gear:
			CanvasSortingSetToDefault(canvasGear);
			break;
		}
	}

	public void ResetAllResourceLayer()
	{
		CanvasSortingSetToDefault(canvasCrystal);
		CanvasSortingSetToDefault(canvasBlood);
		CanvasSortingSetToDefault(canvasCores);
		CanvasSortingSetToDefault(canvasCoin);
		CanvasSortingSetToDefault(canvasGear);
	}

	public void ResourceUIPopUp(ResourceUIPop popUp)
	{
		switch (popUp)
		{
		case ResourceUIPop.Crystal:
			CanvasSortingPopUp(canvasCrystal);
			break;
		case ResourceUIPop.Blood:
			CanvasSortingPopUp(canvasBlood);
			break;
		case ResourceUIPop.Cores:
			CanvasSortingPopUp(canvasCores);
			break;
		case ResourceUIPop.Coin:
			CanvasSortingPopUp(canvasCoin);
			break;
		case ResourceUIPop.Gear:
			CanvasSortingPopUp(canvasGear);
			break;
		}
	}

	private void CanvasSortingPopUp(Canvas canvas)
	{
		canvas.overrideSorting = true;
	}

	private void CanvasSortingSetToDefault(Canvas canvas)
	{
		canvas.overrideSorting = false;
	}

	public void MobileShowResource(int resourceType)
	{
		if (GameMgr.IsMobile_Static && (!(BattleMgr.Inst == null) || !(Guide2Mgr.Inst == null)))
		{
			switch (resourceType)
			{
			case 1:
				showTimeCrystal = 2f;
				break;
			case 2:
				showTimeBlood = 2f;
				break;
			case 3:
				showTimeCore = 2f;
				break;
			case 4:
				showTimeGear = 2f;
				break;
			}
		}
	}

	public void MenuShakeButton()
	{
		if (GameMgr.IsMobile_Static)
		{
			goMenuButton.GetComponent<RectTransform>().DOShakePosition(0.3f, 16f, 20).SetUpdate(isIndependentUpdate: true)
				.OnComplete(delegate
				{
					goMenuButton.transform.GetComponent<RectTransform>().anchoredPosition = menuePosition;
				});
		}
	}

	public void FoldWandButton()
	{
		DataMgr.settingData.Mobiledata.wandFolded = !DataMgr.settingData.Mobiledata.wandFolded;
		MobileUpdateWandFold();
	}

	private void UpdateFoldButtonUI()
	{
		bool flag = GameUISingletonMono<UILevelReward>.StaticIsOpen && GameUISingletonMono<UILevelReward>.Inst.type == LevelRewardType.Wand && GameUISingletonMono<UILevelReward>.Inst.isShowingWand;
		bool flag2 = !IsBagOpen && DataMgr.settingData.Mobiledata.canFoldWand && uiWands.Count > 2;
		mobileFoldWandButton.gameObject.SetActive(!flag && flag2);
		if (mobileFoldWandButton.gameObject.activeInHierarchy)
		{
			if (DataMgr.settingData.Mobiledata.wandFolded && uiWands.Count > 2)
			{
				mobileFoldWandButton.transform.localScale = new Vector3(1f, 1f, 1f);
				for (int num = uiWands.Count - 1; num >= 0; num--)
				{
					if (uiWands[num].image_BG_Frame_Select.gameObject.activeInHierarchy)
					{
						mobileFoldWandButton.position = uiWand.transform.GetChild(num).position + mobildFoldWandOffset;
						break;
					}
					if (num == 0)
					{
						mobileFoldWandButton.position = uiWand.transform.position + mobildFoldWandOffsetNoWandShown;
					}
				}
			}
			else
			{
				mobileFoldWandButton.transform.localScale = new Vector3(-1f, 1f, 1f);
				Transform obj = mobileFoldWandButton;
				List<UIWand> list = uiWands;
				obj.position = list[list.Count - 1].transform.position + mobildFoldWandOffset;
			}
		}
		if (TopUI.inst.uiPotionSelectPopOut.UIObj.activeInHierarchy)
		{
			foldWandCanvas.sortingOrder = 1;
		}
		else if ((bool)PlayerMgr.Inst.PlayerCtrller && PlayerMgr.Inst.PlayerCtrller.CanMotion)
		{
			foldWandCanvas.sortingOrder = 21;
		}
		else
		{
			foldWandCanvas.sortingOrder = 1;
		}
	}

	public void MobileUpdateWandFold()
	{
		if (!GameMgr.IsMobile_Static)
		{
			return;
		}
		if (GameUISingletonMono<UILevelReward>.StaticIsOpen && GameUISingletonMono<UILevelReward>.Inst.type == LevelRewardType.Wand && GameUISingletonMono<UILevelReward>.Inst.isShowingWand)
		{
			uiWands.ForEach(delegate(UIWand x)
			{
				x.CanvasGroup.alpha = 1f;
				x.CanvasGroup.blocksRaycasts = true;
				x.uiWandEvent.graphicRaycaster.enabled = x.CanvasGroup.alpha == 1f;
			});
		}
		else if (IsBagOpen)
		{
			uiWands.ForEach(delegate(UIWand x)
			{
				x.CanvasGroup.alpha = 1f;
				x.CanvasGroup.blocksRaycasts = true;
				x.uiWandEvent.graphicRaycaster.enabled = x.CanvasGroup.alpha == 1f;
			});
		}
		else if (DataMgr.settingData.Mobiledata.wandFolded && uiWands.Count > 2)
		{
			uiWands.ForEach(delegate(UIWand x)
			{
				x.CanvasGroup.alpha = (x.image_BG_Frame_Select.gameObject.activeSelf ? 1 : 0);
				x.CanvasGroup.blocksRaycasts = x.CanvasGroup.alpha == 1f;
				x.uiWandEvent.graphicRaycaster.enabled = x.CanvasGroup.alpha == 1f;
			});
		}
		else
		{
			uiWands.ForEach(delegate(UIWand x)
			{
				x.CanvasGroup.alpha = 1f;
				x.CanvasGroup.blocksRaycasts = true;
				x.uiWandEvent.graphicRaycaster.enabled = x.CanvasGroup.alpha == 1f;
			});
		}
		UpdateWandLayout();
	}

	public void UpdateShieldTemp()
	{
		currentUILeftDown.UpdateShieldTemp();
	}

	public void RecorrectHPMPShieldWidthDirect()
	{
		currentUILeftDown.RecorrectHPMPShieldWidthDirect();
	}

	public void UpdateShield()
	{
		currentUILeftDown.UpdateShield();
	}

	public void UpdateHP()
	{
		currentUILeftDown.UpdateHP();
	}

	public void UpdateMP()
	{
		currentUILeftDown.UpdateMP();
	}

	public void MPWarning()
	{
		currentUILeftDown.MPWarning();
	}

	public void _MobileOpenHandBook()
	{
		if ((bool)Guide2Mgr.Inst && !Guide2Mgr.Inst.OpenedHandbook)
		{
			UIMgr.Inst.UIMenu.ShowHandBookMobile(201);
		}
		else
		{
			UIMgr.Inst.UIMenu.ShowUIMenuHandbook();
		}
	}

	public void SetFullGameUIActive(bool isActive)
	{
		fullGameBtn.SetActive(isActive);
	}

	public void SetBuySuitBtnActive(bool isActive)
	{
		buySuitBtn.SetActive(isActive);
	}

	public void HideResource()
	{
		hideResourceComponent.ForEach(delegate(Component x)
		{
			if (x is CanvasGroup canvasGroup)
			{
				canvasGroup.alpha = 0f;
			}
			else
			{
				x.gameObject.SetActive(value: false);
			}
		});
	}

	public void ShowResource()
	{
		hideResourceComponent.ForEach(delegate(Component x)
		{
			if (x is CanvasGroup canvasGroup)
			{
				canvasGroup.alpha = 1f;
			}
			else
			{
				x.gameObject.SetActive(value: true);
			}
		});
	}

	public void OpenFullGameUI()
	{
		GameUISingletonMono<UIFullGame>.Inst.Show();
	}

	public void OpenBuySuitUI()
	{
		GameUISingletonMono<UISuit>.Inst.Show();
	}

	public void PausePerform()
	{
		if (!ControlMgr.Inst.InputActionRecovering && (GameMgr.IsMobile_Static || GameMgr.IsSteamDeck_Static || ControlMgr.Inst.CursorVisible || UIMgr.Inst.UIMenu.IsOpen || UIMgr.Inst.InputType != 0) && !GameUISingletonMono<UIDialogueMgr>.StaticIsOpen)
		{
			if (UIMgr.Inst.UIMenu.Panel_Confirm.activeSelf)
			{
				UIMgr.Inst.UIMenu._MenuQuitNo();
			}
			else if (UIMgr.Inst.UIMenu.IsOpen)
			{
				UIMgr.Inst.UIMenu.Hide();
			}
			else
			{
				UIMgr.Inst.UIMenu.ShowUIMenu();
			}
		}
	}
}
