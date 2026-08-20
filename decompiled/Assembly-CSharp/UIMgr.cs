using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
	public const int MobileCustomControl = 22;

	public const int MobileFoldWandSortingOrder = 21;

	public RectTransform rtsf_Canvas1;

	public RectTransform rtsf_Canvas2;

	public RectTransform rtsf_Canvas3;

	public RectTransform rtsf_Canvas4;

	public RectTransform rtsf_Canvas10;

	public Transform infoHover;

	public Canvas canvas_1;

	public Canvas canvas_3;

	public Canvas canvas5_Entry;

	public Canvas canvas_10;

	public Canvas canvas_10Hover;

	public Canvas canvas11;

	public CanvasScaler canvas_1Scaler;

	public CanvasScaler canvas_10Scaler;

	public CanvasScaler canvasScalerCanvas11;

	[HideInInspector]
	public UISetting uiSetting;

	[HideInInspector]
	[Header("InteractiveBtn")]
	public UIMenu UIMenu;

	[Header("LanguageChange")]
	public Text text_Bag;

	public Text text_GamepadTip1_WandInfo;

	public Text text_GamepadTip2_WandInfo;

	public Text text_GamepadTip1_SpellInfo;

	public Text text_GamepadTip2_SpellInfo;

	public bool showbattleui = true;

	[Header("PlayerDataMgr")]
	public UIPlayerInjured UIPlayerInjured;

	public UIImageDragings uIImageDragings;

	public UIFade uiFade { get; set; }

	public UIFilmBlackEdge uiFilmBlackEdge { get; set; }

	public UIInfoWand uiInfoWandHover { get; set; }

	public UIInfoSpell uiInfoSpellHover { get; set; }

	public UIInfoRelic uiInfoRelicHover { get; set; }

	public UIInfoPotion uiInfoPotionHover { get; set; }

	public UIInfoCurse uiInfoCurseHover { get; set; }

	public GameObject panel_GamepadTip_WandInfo => uiInfoWandHover.panel_GamepadTip_WandInfo;

	public GameObject panel_GamepadTip_SpellInfo => uiInfoSpellHover.panel_GamepadTip_SpellInfo;

	public UISlotWandTips uiSlotWandTips => uiInfoSpellHover.uiSlotWandTips;

	public static UIMgr Inst { get; private set; }

	public PlayerInputType InputType => ControlMgr.Inst.InputType;

	public void Initialize()
	{
		Inst = this;
		if (GameMgr.IsMobile_Static)
		{
			ControlMgr.Inst.ForceSetInputType(PlayerInputType.Keyboard);
		}
		uiSetting = canvas_10Scaler.GetComponentInChildren<UISetting>();
		uiFade = rtsf_Canvas10.GetComponentInChildren<UIFade>();
		uiFilmBlackEdge = rtsf_Canvas10.GetComponentInChildren<UIFilmBlackEdge>();
		uiInfoWandHover = infoHover.GetComponentInChildren<UIInfoWand>();
		uiInfoSpellHover = infoHover.GetComponentInChildren<UIInfoSpell>();
		uiInfoRelicHover = infoHover.GetComponentInChildren<UIInfoRelic>();
		uiInfoPotionHover = infoHover.GetComponentInChildren<UIInfoPotion>();
		uiInfoCurseHover = infoHover.GetComponentInChildren<UIInfoCurse>();
		uiInfoWandHover.gameObject.SetActive(value: false);
		uiInfoSpellHover.gameObject.SetActive(value: false);
		uiInfoRelicHover.gameObject.SetActive(value: false);
		uiInfoPotionHover.gameObject.SetActive(value: false);
		uiInfoCurseHover.gameObject.SetActive(value: false);
		uiInfoRelicHover.GetComponent<RectTransform>().pivot = new Vector2(1f, 1f);
		uiInfoCurseHover.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
		uiInfoSpellHover.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		uiInfoPotionHover.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
		if (GameMgr.IsMobile_Static)
		{
			Vector3 localScale = new Vector3(1.2f, 1.2f, 1f);
			uiInfoSpellHover.transform.localScale = localScale;
			uiInfoRelicHover.transform.localScale = localScale;
			uiInfoCurseHover.transform.localScale = localScale;
			uiInfoPotionHover.transform.localScale = localScale;
		}
		uIImageDragings = canvas_1.GetComponentInChildren<UIImageDragings>();
		UIPlayerInjured = canvas_1.GetComponentInChildren<UIPlayerInjured>();
		ControlMgr.Inst.CursorVisibleSet(set: true);
		GameMgr.Inst.UIPlayerDataMgr.Initialize();
	}

	private void OnEnable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	private void OnDisable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void LanguageChange()
	{
		if (!GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.textBag.text = 1000301.GetText() + "<color=#FFff00>[Tab]</color>";
		}
		if (InputType == PlayerInputType.Gamepad)
		{
			uiInfoWandHover.textGamepadDrag.text = 1000701.GetText();
			uiInfoWandHover.textGamepadThrow.text = 1000702.GetText();
			uiInfoSpellHover.textGamepadDrag.text = 1000701.GetText();
			uiInfoSpellHover.textGamepadThrow.text = 1000702.GetText();
		}
	}

	public void InputChange()
	{
		if (GameMgr.IsMobile_Static)
		{
			panel_GamepadTip_WandInfo.SetActive(value: false);
			panel_GamepadTip_SpellInfo.SetActive(value: false);
			return;
		}
		switch (InputType)
		{
		case PlayerInputType.Keyboard:
			panel_GamepadTip_WandInfo.SetActive(value: false);
			panel_GamepadTip_SpellInfo.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			panel_GamepadTip_WandInfo.SetActive(!IsAnyBuildPanelOpen());
			panel_GamepadTip_SpellInfo.SetActive(!IsAnyBuildPanelOpen());
			break;
		default:
			Debug.LogError(InputType);
			break;
		}
	}

	public bool IsAnyBuildPanelOpen()
	{
		bool num = (bool)UIBattleMgr.Inst && (bool)UIBattleMgr.Inst.uiFinishBuildShow && UIBattleMgr.Inst.uiFinishBuildShow.IsOpen;
		bool flag = (bool)UIMenu && (bool)UIMenu.finishBuildShow && UIMenu.finishBuildShow.IsOpen;
		bool flag2 = (bool)GameUISingletonMono<UI_RankingList>.Inst && (bool)GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow && GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.IsOpen;
		bool flag3 = (bool)GameUISingletonMono<UIEndlessRankingList>.Inst && (bool)GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow && GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.IsOpen;
		return num || flag || flag2 || flag3;
	}

	public bool TryCloseGlobalTopUI()
	{
		if (GameUISingletonMono<UICommonHint>.Inited && GameUISingletonMono<UICommonHint>.Inst.closeButton.activeInHierarchy)
		{
			GameUISingletonMono<UICommonHint>.Inst._Close();
			return true;
		}
		if ((bool)Inst.uiSetting && Inst.uiSetting.IsOpen)
		{
			if ((bool)Inst.UIMenu && Inst.UIMenu.IsOpen)
			{
				if (GameMgr.IsMobile_Static)
				{
					Inst.UIMenu.MobileCloseMenuCategory();
				}
				else
				{
					Inst.UIMenu._ManualToggle(-1);
				}
			}
			else
			{
				Inst.uiSetting.Hide();
			}
			return true;
		}
		if ((bool)Inst.UIMenu)
		{
			if (Inst.UIMenu.Panel_Confirm.activeSelf)
			{
				Inst.UIMenu._MenuQuitNo();
				return true;
			}
			if (GameMgr.IsMobile_Static)
			{
				if (Inst.UIMenu.CurrentCategory == UIMenu.MenuCategory.HandBook)
				{
					Inst.UIMenu._MobilCloseHandBook();
					return true;
				}
				if (Inst.UIMenu.uiGallery.IsOpen || Inst.UIMenu.uIAchievement.IsOpen)
				{
					Inst.UIMenu.MobileCloseMenuCategory();
					return true;
				}
			}
			if (Inst.UIMenu.IsOpen)
			{
				Inst.UIMenu.Hide();
				return true;
			}
		}
		return false;
	}

	private void Start()
	{
		LanguageChange();
		InputChange();
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.MobileUpdateInteractButtonShow();
		}
	}

	public void ShowFindSetUI(int setID)
	{
		if (DataMgr.selectedWorldData.story3NPC4Rescued)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIGetNewSuit"), canvas_1Scaler.transform).GetComponent<UI_GetSuitPopOut>().text.text = 1002104.GetText() + ": " + SetConfig.dic[setID].GetName();
		}
	}

	public void ShowActiveRelicGroupUI(RelicGroupConfig config)
	{
		UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIGetRelicGroup"), canvas_1Scaler.transform).GetComponent<UIRelicGroupActiveTip>().Initialize(config);
	}

	public static void AutoPivotFix(Vector3 targetPosition, RectTransform rect, Vector2 pivotOffset, bool useNewPivot = false, Vector3 positionOffset = default(Vector3), Vector3 positionOffsetAutoAdjust = default(Vector3), bool StickEdge = true, int waitframe = 2, Action action = null)
	{
		Vector3 vector = CamController.Inst.cam_UI.WorldToViewportPoint(targetPosition);
		Vector2 vector2 = positionOffsetAutoAdjust;
		float x;
		if (vector.x > 0.5f)
		{
			x = (useNewPivot ? 1f : (0f - pivotOffset.x + 1f));
			vector2.x *= -1f;
		}
		else
		{
			x = (useNewPivot ? 0f : pivotOffset.x);
		}
		float y;
		if (vector.y < 0.5f)
		{
			y = (useNewPivot ? 0f : (0f - pivotOffset.y + 1f));
			vector2.y *= -1f;
		}
		else
		{
			y = (useNewPivot ? 1f : pivotOffset.y);
		}
		rect.pivot = new Vector2(x, y);
		rect.transform.position = targetPosition + positionOffset + new Vector3(vector2.x, vector2.y, 0f);
	}

	public static void AutoPivot(Vector3 targetPosition, RectTransform rect, Vector2 pivotOffset, bool useNewPivot = false, Vector3 positionOffset = default(Vector3), Vector3 positionOffsetAutoAdjust = default(Vector3), bool StickEdge = true, int waitframe = 2, Action action = null)
	{
		Vector3 vector = CamController.Inst.cam_UI.WorldToViewportPoint(targetPosition);
		Vector2 vector2 = positionOffsetAutoAdjust;
		float x;
		if (vector.x > 0.5f)
		{
			if (useNewPivot)
			{
				x = 1f;
			}
			else
			{
				x = 0f - pivotOffset.x + 1f;
				vector2.x *= -1f;
			}
		}
		else
		{
			x = (useNewPivot ? 0f : pivotOffset.x);
		}
		float y;
		if (vector.y < 0.5f)
		{
			if (useNewPivot)
			{
				y = 0f;
			}
			else
			{
				y = 0f - pivotOffset.y + 1f;
				vector2.y *= -1f;
			}
		}
		else
		{
			y = (useNewPivot ? 1f : pivotOffset.y);
		}
		rect.transform.position = targetPosition + positionOffset + new Vector3(vector2.x, vector2.y, 0f);
		rect.pivot = new Vector2(x, y);
		if (StickEdge)
		{
			if (waitframe == 0)
			{
				InteractiveFollowFitSelf(rect.gameObject, rect.pivot);
				action?.Invoke();
			}
			else if (waitframe > 0)
			{
				Inst.StartCoroutine(Inst.IEInteractiveFollowFitSelf(waitframe, rect.gameObject, scaled: true, rect.pivot, null, null, action));
			}
			else
			{
				Debug.LogError("不能等待负数帧");
			}
		}
	}

	public static void InteractiveFollowFitChild(GameObject panel, Vector2? PivotPosition = null, Vector2? canvasRes = null, Vector3? parentScale = null)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < panel.transform.childCount; i++)
		{
			if (!panel.transform.GetChild(i).gameObject.activeSelf)
			{
				continue;
			}
			if (GameMgr.IsMobile_Static)
			{
				float num3 = panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height * panel.transform.GetChild(i).GetComponent<RectTransform>().localScale.y;
				float num4 = panel.transform.GetChild(i).GetComponent<RectTransform>().rect.width * panel.transform.GetChild(i).GetComponent<RectTransform>().localScale.x;
				float x = panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x;
				float y = panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.y;
				if (y + num3 > num)
				{
					num = y + num3;
				}
				if (x + num4 > num2)
				{
					num2 = x + num4;
				}
			}
			else
			{
				if (panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.y + panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y > num)
				{
					num = panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.y + panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
				}
				if (panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x + panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x > num2)
				{
					num2 = panel.transform.GetChild(i).GetComponent<RectTransform>().localPosition.x + panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;
				}
			}
		}
		StickToDisplayEdge(panel.transform, num, num2, PivotPosition, canvasRes, parentScale);
	}

	public static void InteractiveFollowFitSelf(GameObject panel, Vector2? PivotPosition = null, Vector2? canvasRes = null, Vector3? parentScale = null)
	{
		UIElementFollowFitSelf(panel, panel.GetComponent<RectTransform>(), panel.transform.localScale, PivotPosition, canvasRes, parentScale);
	}

	public static void UIElementFollowFitSelf(GameObject panel, RectTransform appearance, Vector3? scale = null, Vector2? PivotPosition = null, Vector2? canvasRes = null, Vector3? parentScale = null)
	{
		if (scale.HasValue)
		{
			StickToDisplayEdge(panel.transform, appearance.rect.height * scale.Value.y, appearance.rect.width * scale.Value.x, PivotPosition, canvasRes, parentScale);
		}
		else
		{
			StickToDisplayEdge(panel.transform, appearance.sizeDelta.y, appearance.sizeDelta.x, PivotPosition, canvasRes, parentScale);
		}
	}

	private IEnumerator IEInteractiveFollowFitSelf(int waitFrame, GameObject panel, bool scaled = false, Vector2? PivotPosition = null, Vector2? canvasRes = null, Vector3? parentScale = null, Action action = null)
	{
		for (int i = 0; i < waitFrame; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		InteractiveFollowFitSelf(panel, PivotPosition);
		action?.Invoke();
	}

	private static void StickToDisplayEdge(Transform targettransform, float highest, float widest, Vector2? PivotPosition = null, Vector2? canvasRes = null, Vector3? parentScale = null)
	{
		Vector2 vector = default(Vector2);
		Vector2 vector2 = PivotPosition ?? new Vector2(0f, 0f);
		Vector2 vector3 = parentScale ?? ((Vector3)new Vector2(1f, 1f));
		float num;
		float num2;
		if (canvasRes.HasValue)
		{
			num = canvasRes.Value.x;
			num2 = canvasRes.Value.x / canvasRes.Value.y * num;
		}
		else if (GameMgr.IsMobile_Static || GameMgr.IsSteamDeck_Static)
		{
			num = (float)MobileMgr.inst.scalerhight / 2f;
			num2 = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * num;
		}
		else
		{
			num = 540f;
			num2 = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * 540f;
		}
		num /= vector3.x;
		num2 /= vector3.y;
		if (targettransform.localPosition.y + highest * (1f - vector2.y) > num)
		{
			vector.y = num - highest * (1f - vector2.y);
		}
		else if (targettransform.localPosition.y - highest * vector2.y < 0f - num)
		{
			vector.y = highest * vector2.y - num;
		}
		else
		{
			vector.y = targettransform.localPosition.y;
		}
		if (targettransform.localPosition.x + widest * (1f - vector2.x) > num2)
		{
			vector.x = num2 - widest * (1f - vector2.x);
		}
		else if (targettransform.localPosition.x - widest * vector2.x < 0f - num2)
		{
			vector.x = widest * vector2.x - num2;
		}
		else
		{
			vector.x = targettransform.localPosition.x;
		}
		targettransform.localPosition = vector;
	}

	public void NearInteractive(GameObject obj)
	{
	}

	public void UIFlyPos(Canvas canvas, Sprite sprite, float size, Vector3 worldSpawnPosition, Vector3 screenPos, float zoomtime, float displayTime, float speed)
	{
		Camera cam_Main = GameMgr.Inst.cameCtrller.cam_Main;
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(canvas.gameObject.transform);
		gameObject.transform.rotation = Quaternion.identity;
		gameObject.transform.localPosition = cam_Main.WorldToScreenPoint(worldSpawnPosition) - new Vector3(canvas.renderingDisplaySize.x / 2f, canvas.renderingDisplaySize.y / 2f);
		gameObject.transform.localScale = Vector3.zero;
		Image image = gameObject.AddComponent<Image>();
		image.sprite = sprite;
		image.raycastTarget = false;
		StartCoroutine(IEUIfly(gameObject, gameObject.GetComponent<Image>(), size, screenPos, zoomtime, displayTime, speed));
	}

	public void UIFlyPosFromUI(Canvas canvas, Sprite sprite, float size, Vector3 worldSpawnPosition, Vector3 screenPos, float zoomtime, float displayTime, float speed)
	{
		_ = GameMgr.Inst.cameCtrller.cam_UI;
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(canvas.gameObject.transform);
		gameObject.transform.rotation = Quaternion.identity;
		gameObject.transform.position = worldSpawnPosition;
		gameObject.transform.localScale = Vector3.zero;
		Image image = gameObject.AddComponent<Image>();
		image.sprite = sprite;
		image.raycastTarget = false;
		StartCoroutine(IEUIfly(gameObject, gameObject.GetComponent<Image>(), size, screenPos, zoomtime, displayTime, speed));
	}

	private IEnumerator IEUIfly(GameObject obj, Image image, float size, Vector3 FlytoScreen, float zoomtime, float displayTime, float speed)
	{
		float time2 = 0f;
		Camera cam = GameMgr.Inst.cameCtrller.cam_UI;
		yield return new WaitForEndOfFrame();
		Vector3 vector = cam.WorldToScreenPoint(obj.transform.position);
		Vector3 targetPositionScreen = FlytoScreen;
		float xall = targetPositionScreen.x - vector.x;
		for (; time2 < zoomtime; time2 += Time.fixedDeltaTime)
		{
			obj.transform.localScale = Vector3.one * (size * time2) / zoomtime;
			yield return new WaitForEndOfFrame();
		}
		for (time2 = 0f; time2 < displayTime; time2 += Time.fixedDeltaTime)
		{
			yield return new WaitForEndOfFrame();
		}
		while (Vector2.SqrMagnitude((Vector2)cam.WorldToScreenPoint(obj.transform.position) - (Vector2)targetPositionScreen) > speed * speed)
		{
			Vector3 vector2 = cam.WorldToScreenPoint(obj.transform.position);
			image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Sqrt((targetPositionScreen.x - vector2.x) / xall));
			obj.transform.localPosition = obj.transform.localPosition + (Vector3)((Vector2)targetPositionScreen - (Vector2)vector2).normalized * speed;
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.Object.Destroy(obj);
	}

	public static void LoadUI(PlatLoad platLoad)
	{
		if (platLoad.rect == null)
		{
			switch (platLoad.canvasRect)
			{
			case CanvasRect.canvas1:
				platLoad.rect = Inst.rtsf_Canvas1;
				break;
			case CanvasRect.canvas3:
				platLoad.rect = Inst.rtsf_Canvas3;
				break;
			case CanvasRect.canvas5_entry:
				if (!Inst.canvas5_Entry.IsDestroyed())
				{
					platLoad.rect = Inst.canvas5_Entry.GetComponent<RectTransform>();
				}
				break;
			case CanvasRect.canvas5:
				if ((bool)UICampMgr.Inst)
				{
					platLoad.rect = UICampMgr.Inst.rtsfCanvas;
				}
				if ((bool)UIBattleMgr.Inst)
				{
					platLoad.rect = UIBattleMgr.Inst.rtsfCanvas;
				}
				if ((bool)UIGuideMgr.Inst)
				{
					platLoad.rect = UIGuideMgr.Inst.rtsfCanvas;
				}
				break;
			case CanvasRect.canvas10:
				platLoad.rect = Inst.rtsf_Canvas10;
				break;
			}
		}
		if (GameMgr.IsMobile_Static && platLoad.goMobile != null)
		{
			UnityEngine.Object.Instantiate(platLoad.goMobile, platLoad.rect);
		}
		else if (GameMgr.IsSteamDeck_Static && platLoad.goSteamDeck != null)
		{
			UnityEngine.Object.Instantiate(platLoad.goSteamDeck, platLoad.rect);
		}
		else if (platLoad.goPC != null)
		{
			UnityEngine.Object.Instantiate(platLoad.goPC, platLoad.rect);
		}
	}

	public void MoveDownHoverLayer()
	{
		Inst.canvas_10Hover.sortingLayerName = "Default";
		Inst.canvas_10Hover.sortingOrder = 10;
	}

	public void MoveUpHoverLayer()
	{
		Inst.canvas_10Hover.sortingLayerName = "UIOverParticle";
		Inst.canvas_10Hover.sortingOrder = 11;
	}

	public static void TryAdditionalMobileShow(Transform transform, int SearchDepth = 2)
	{
		if (GameMgr.IsMobile_Static)
		{
			UIMobileReturnAndRess uIMobileReturnAndRess = GeneralTool.RecursiveComponentSearchDepth<UIMobileReturnAndRess>(transform, SearchDepth);
			if (uIMobileReturnAndRess != null)
			{
				uIMobileReturnAndRess.Show();
			}
		}
	}

	public static void TryAdditionalMobileHide(Transform transform, int SearchDepth = 2)
	{
		if (GameMgr.IsMobile_Static)
		{
			UIMobileReturnAndRess uIMobileReturnAndRess = GeneralTool.RecursiveComponentSearchDepth<UIMobileReturnAndRess>(transform, SearchDepth);
			if (uIMobileReturnAndRess != null)
			{
				uIMobileReturnAndRess.Hide();
			}
		}
	}
}
