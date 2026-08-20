using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotWand : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public Image image_Slot_Normal;

	public Image image_Slot_Post;

	public Image image_SpellIcon;

	public Image image_SpellIconOutline;

	public Image image_SpellStar1;

	public Image image_SpellStar2;

	public Image image_Ring_Normal;

	public Image image_Ring_Post;

	public Image image_Preshoot_Normal;

	public Image image_Preshoot_Post;

	public Image image_Lock;

	public Image image_PostTypeIcon;

	public Image image_Link;

	public Image image_UnableToCastSlotSpellAlert;

	public Image image_MimicError;

	public Image image_Unused;

	public Material mat_MimicIcon;

	public Material mat_SharedSpellIcon;

	public GameObject ui_tips;

	[Header("Hover")]
	public Sprite sprite_Normal_Hover;

	public Sprite sprite_Normal_Unhover;

	public Sprite sprite_Post_Hover;

	public Sprite sprite_Post_Unhover;

	public float hoverScale;

	private bool canDrag = true;

	public bool isFromBuild;

	private FinishGameBuild _build;

	public float UpgradeStarHueAdjust;

	public GameObject imageRingMobileNearest;

	public GameObject imageRingMobileNearestPost;

	public bool focusingThisSlot;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	private Vector3 pointedInPosition;

	private static readonly int HueAdjust = Shader.PropertyToID("_HueAdjust");

	private ProfilerMarker pm1 = new ProfilerMarker("pm1");

	public FinishGameBuild build
	{
		get
		{
			return _build;
		}
		set
		{
			_build = value;
		}
	}

	private bool needShowLink
	{
		get
		{
			if (!isFromBuild)
			{
				return needShowLinkBuild();
			}
			return needShowLinkBuild(build, WandIndex);
		}
	}

	private Color linkColor
	{
		get
		{
			if (SpellDat == null)
			{
				return default(Color);
			}
			if (!SpellDat.isSealSlot)
			{
				return SpellCfg.useType.SlotColor();
			}
			return SpellDat.sealSlotOwner.GetFinalConfig().useType.SlotColor();
		}
	}

	public int WandIndex { get; private set; }

	public int SpellIndex { get; private set; }

	public WandSlotType SlotType { get; private set; }

	public WandConfig WandCfg => PlayerMgr.Inst.BaData.wandCfgs[WandIndex];

	public SlotData SpellDat
	{
		get
		{
			if (build == null)
			{
				if ((object)PlayerMgr.Inst == null)
				{
					return null;
				}
				if (PlayerMgr.Inst.BaData == null)
				{
					return null;
				}
				if (WandIndex >= PlayerMgr.Inst.BaData.wandCfgs.Count)
				{
					return null;
				}
				WandConfig wandConfig = PlayerMgr.Inst.BaData.wandCfgs[WandIndex];
				if (wandConfig == null)
				{
					return null;
				}
				SlotData[] slotsData = wandConfig.GetSlotsData(SlotType);
				if (SpellIndex >= slotsData.Length)
				{
					return null;
				}
				return slotsData[SpellIndex];
			}
			SlotData[] slotsData2 = build.wandCfgs[WandIndex].GetSlotsData(SlotType);
			if (SpellIndex >= slotsData2.Length)
			{
				return null;
			}
			return slotsData2[SpellIndex];
		}
	}

	public SpellConfig SpellCfg
	{
		get
		{
			if (SpellDat != null && !SpellDat.isSealSlot)
			{
				return SpellConfig.dic[SpellDat.id];
			}
			return null;
		}
	}

	public bool IsSlotLock
	{
		get
		{
			if (isFromBuild)
			{
				return build.wandCfgs[WandIndex].IsSlotLock(SlotType, SpellIndex);
			}
			return WandCfg.IsSlotLock(SlotType, SpellIndex);
		}
	}

	public bool isSlotSeal
	{
		get
		{
			if (SpellDat != null)
			{
				return SpellDat.isSealSlot;
			}
			return false;
		}
	}

	private bool isDragging => UIPlayerDataMgr.Inst.uiSlotWand_Drag == this;

	private void Awake()
	{
		UIGamePadNav uIGamePadNav = base.transform.AddComponent<UIGamePadNav>();
		uIGamePadNav.OnDeselectAction = OnPointerExit;
		uIGamePadNav.OnSelectAction = OnPointerEnter;
	}

	private bool needShowLinkBuild(FinishGameBuild build = null, int wandindex = 0)
	{
		if (SpellDat == null)
		{
			return false;
		}
		if (SpellDat.isAllFieldSharedSpell)
		{
			return false;
		}
		if (SpellDat.isSealSlot && !SpellDat.isAllFieldSharedSpell)
		{
			int num = SpellIndex + 1;
			if (WandIndex >= PlayerMgr.Inst.Wands.Count)
			{
				return false;
			}
			if (PlayerMgr.Inst.Wands[WandIndex] == null)
			{
				return false;
			}
			WandConfig wandConfig = null;
			Wand wand = PlayerMgr.Inst.Wands[WandIndex];
			SlotData[] array = null;
			if (build != null)
			{
				wandConfig = build.wandCfgs[wandindex];
				if (wandConfig == null)
				{
					return false;
				}
			}
			array = ((build == null) ? wand.WandCfg.GetSlotsData(SlotType) : wandConfig.GetSlotsData(SlotType));
			if (!array.Bag_IndexCheck(num))
			{
				return false;
			}
			SlotData slotData = array[num];
			if (slotData == null || !slotData.isSealSlot)
			{
				return false;
			}
			return true;
		}
		return SpellDat.GetFinalSlotCost() > 1;
	}

	public void Initialize(int wandIndex, int spellIndex, WandSlotType slotType, FinishGameBuild build = null)
	{
		WandIndex = wandIndex;
		SpellIndex = spellIndex;
		SlotType = slotType;
		image_MimicError.gameObject.SetActive(value: false);
		if (build == null)
		{
			UpdateInfo();
			return;
		}
		isFromBuild = true;
		_build = build;
		UpdateInfo(build, wandIndex);
	}

	private void Update()
	{
		if (isFromBuild)
		{
			return;
		}
		if (!UIPlayerDataMgr.Inst.IsDraging && Input.GetKey(KeyCode.LeftShift) && focusingThisSlot)
		{
			QuickChange();
		}
		if (SpellCfg != null && SpellCfg.abilityType == SpellAbilityType.Mimic)
		{
			image_MimicError.gameObject.SetActive(SpellDat.mimicSpellID == 0);
			if (image_MimicError.gameObject.activeSelf)
			{
				if (WandCfg.GetValidSlotsData(SlotType == WandSlotType.Normal, SlotType == WandSlotType.Post)[^1] == SpellDat)
				{
					UIPlayerDataMgr.Inst.uiSlotWandTips.text_MimicError.text = 1000708.GetText();
				}
				else
				{
					UIPlayerDataMgr.Inst.uiSlotWandTips.text_MimicError.text = 1000709.GetText();
				}
			}
		}
		else
		{
			image_MimicError.gameObject.SetActive(value: false);
		}
	}

	public bool GetFocusState()
	{
		return focusingThisSlot;
	}

	public void UpdateInfo(FinishGameBuild build = null, int wandindex = 0)
	{
		image_Slot_Normal.gameObject.SetActive(value: false);
		image_Slot_Post.gameObject.SetActive(value: false);
		image_SpellIcon.gameObject.SetActive(value: false);
		image_SpellIcon.color = Color.white;
		image_SpellStar1.gameObject.SetActive(value: false);
		image_SpellStar2.gameObject.SetActive(value: false);
		image_Ring_Normal.gameObject.SetActive(value: false);
		image_Ring_Post.gameObject.SetActive(value: false);
		image_Lock.gameObject.SetActive(value: false);
		image_PostTypeIcon.gameObject.SetActive(value: false);
		image_Preshoot_Post.gameObject.SetActive(value: false);
		image_Link.gameObject.SetActive(value: false);
		if (needShowLink)
		{
			image_Link.gameObject.SetActive(value: true);
			image_Link.color = linkColor;
		}
		if (isSlotSeal)
		{
			SlotData sealSlotOwner = SpellDat.sealSlotOwner;
			SpellConfig finalConfig = sealSlotOwner.GetFinalConfig();
			if (SlotType == WandSlotType.Normal)
			{
				image_Ring_Normal.color = linkColor;
				image_Ring_Normal.gameObject.SetActive(value: true);
			}
			else if (SlotType == WandSlotType.Post)
			{
				image_Ring_Post.color = linkColor;
				image_Ring_Post.gameObject.SetActive(value: true);
			}
			if (SpellConfig.dic[sealSlotOwner.id].abilityType == SpellAbilityType.Mimic)
			{
				SetIconEffect(WandSlotIconVisualEffect.Mimic);
			}
			if (SpellDat.isAllFieldSharedSpell)
			{
				SetIconEffect(WandSlotIconVisualEffect.AllFieldEnhance);
			}
			image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(finalConfig.GetIconPath());
			if (GameMgr.IsMobile_Static)
			{
				image_SpellIconOutline.sprite = image_SpellIcon.sprite;
			}
			Color color = image_SpellIcon.color;
			color.a = 0.4f;
			image_SpellIcon.color = color;
			image_SpellIcon.gameObject.SetActive(value: true);
		}
		switch (SlotType)
		{
		case WandSlotType.Normal:
			image_Slot_Normal.gameObject.SetActive(value: true);
			break;
		case WandSlotType.Post:
			image_Slot_Post.gameObject.SetActive(value: true);
			image_PostTypeIcon.gameObject.SetActive(value: true);
			if (isFromBuild)
			{
				image_PostTypeIcon.sprite = build.wandCfgs[wandindex].GetPostSlotIcon();
			}
			else
			{
				image_PostTypeIcon.sprite = WandCfg.GetPostSlotIcon();
			}
			if (build != null)
			{
				image_Preshoot_Post.gameObject.SetActive(value: false);
			}
			else
			{
				image_Preshoot_Post.gameObject.SetActive(value: true);
			}
			break;
		default:
			Debug.LogError(SlotType);
			break;
		}
		if (SpellDat != null && !SpellDat.isSealSlot)
		{
			image_SpellIcon.gameObject.SetActive(value: true);
			image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(SpellCfg.GetIconPath());
			if (GameMgr.IsMobile_Static)
			{
				image_SpellIconOutline.sprite = image_SpellIcon.sprite;
			}
			if (SpellCfg.level >= 2)
			{
				image_SpellStar1.gameObject.SetActive(value: true);
			}
			if (SpellCfg.level >= 3)
			{
				image_SpellStar2.gameObject.SetActive(value: true);
			}
			GeneralTool.InitialImageMaterial(image_SpellStar1);
			GeneralTool.InitialImageMaterial(image_SpellStar2);
			image_SpellStar1.material.SetFloat(HueAdjust, 0f);
			image_SpellStar2.material.SetFloat(HueAdjust, 0f);
			switch (SpellCfg.level)
			{
			case 1:
				if (SpellDat.slotSpellExtraLevel >= 1)
				{
					image_SpellStar1.material.SetFloat(HueAdjust, UpgradeStarHueAdjust);
				}
				if (SpellDat.slotSpellExtraLevel >= 2)
				{
					image_SpellStar2.material.SetFloat(HueAdjust, UpgradeStarHueAdjust);
				}
				break;
			case 2:
				if (SpellDat.slotSpellExtraLevel >= 1)
				{
					image_SpellStar2.material.SetFloat(HueAdjust, UpgradeStarHueAdjust);
				}
				break;
			}
			Image image = null;
			switch (SlotType)
			{
			case WandSlotType.Normal:
				image = image_Ring_Normal;
				image_Ring_Normal.gameObject.SetActive(value: true);
				break;
			case WandSlotType.Post:
				image = image_Ring_Post;
				image_Ring_Post.gameObject.SetActive(value: true);
				break;
			default:
				Debug.LogError(SlotType);
				break;
			}
			if (image != null)
			{
				switch (SpellCfg.useType)
				{
				case SpellType.Missile:
				case SpellType.Summon:
					image.color = GameConst.color_SpellRingTypeMissle;
					break;
				case SpellType.Enhance:
					image.color = GameConst.color_SpellRingTypeEnhance;
					break;
				case SpellType.Passive:
					image.color = GameConst.color_SpellRingTypePassive;
					break;
				default:
					Debug.LogError(SpellCfg.useType);
					break;
				}
			}
		}
		if (IsSlotLock)
		{
			image_Lock.gameObject.SetActive(value: true);
		}
		ResetIconImageState();
	}

	public void ManaNotEnoughCastSpell(bool state)
	{
		image_UnableToCastSlotSpellAlert.gameObject.SetActive(state);
	}

	public void SetIcon(SpellConfig spellConf, int mimicFinalLevel)
	{
		if (spellConf != null && !isSlotSeal)
		{
			image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(spellConf.GetIconPath());
			if (GameMgr.IsMobile_Static)
			{
				image_SpellIconOutline.sprite = image_SpellIcon.sprite;
			}
			image_SpellStar1.gameObject.SetActive(value: false);
			image_SpellStar2.gameObject.SetActive(value: false);
			switch (mimicFinalLevel)
			{
			case 2:
				image_SpellStar1.gameObject.SetActive(value: true);
				break;
			case 3:
				image_SpellStar1.gameObject.SetActive(value: true);
				image_SpellStar2.gameObject.SetActive(value: true);
				break;
			}
		}
		else
		{
			image_SpellIcon.gameObject.SetActive(value: false);
			image_SpellStar1.gameObject.SetActive(value: false);
			image_SpellStar2.gameObject.SetActive(value: false);
		}
	}

	public void SetIconBuild(SpellConfig spellConf, int mimicFinalLevel, FinishGameBuild build, int wandindex)
	{
		if (spellConf != null && !isSlotSeal)
		{
			image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(spellConf.GetIconPath());
			if (GameMgr.IsMobile_Static)
			{
				image_SpellIconOutline.sprite = image_SpellIcon.sprite;
			}
			image_SpellStar1.gameObject.SetActive(value: false);
			image_SpellStar2.gameObject.SetActive(value: false);
			switch (mimicFinalLevel)
			{
			case 2:
				image_SpellStar1.gameObject.SetActive(value: true);
				break;
			case 3:
				image_SpellStar1.gameObject.SetActive(value: true);
				image_SpellStar2.gameObject.SetActive(value: true);
				break;
			}
		}
		else
		{
			image_SpellIcon.gameObject.SetActive(value: false);
			image_SpellStar1.gameObject.SetActive(value: false);
			image_SpellStar2.gameObject.SetActive(value: false);
		}
	}

	public void SetIcon(bool state)
	{
		image_SpellIcon.gameObject.SetActive(state);
		image_SpellStar1.gameObject.SetActive(state);
		image_SpellStar2.gameObject.SetActive(state);
	}

	public void UpdatePostSlotCharge(float percent)
	{
		image_Preshoot_Post.fillAmount = percent;
	}

	public void SetIconEffect(WandSlotIconVisualEffect type)
	{
		switch (type)
		{
		case WandSlotIconVisualEffect.Normal:
			image_SpellIcon.material = null;
			break;
		case WandSlotIconVisualEffect.Mimic:
			image_SpellIcon.material = mat_MimicIcon;
			break;
		case WandSlotIconVisualEffect.AllFieldEnhance:
			image_SpellIcon.material = mat_SharedSpellIcon;
			break;
		case WandSlotIconVisualEffect.EnchantLevel:
			break;
		}
	}

	public void ResetIconImageState()
	{
		if (build == null)
		{
			if (SpellDat != null && !SpellDat.isSealSlot)
			{
				image_SpellIcon.material = null;
				SpellConfig spellConf = ((SpellDat.mimicSpellID != 0) ? SpellConfig.dic[SpellDat.mimicSpellID] : SpellConfig.dic[SpellDat.id]);
				SetIcon(spellConf, SpellDat.GetFinalLevel());
				SetIconEffect((SpellDat.mimicSpellID != 0) ? WandSlotIconVisualEffect.Mimic : WandSlotIconVisualEffect.Normal);
				if (SpellDat.isAllFieldSharedSpell)
				{
					SetIconEffect(WandSlotIconVisualEffect.AllFieldEnhance);
				}
			}
		}
		else if (SpellDat != null && !SpellDat.isSealSlot)
		{
			image_SpellIcon.material = null;
			SpellConfig spellConf2 = ((SpellDat.mimicSpellID != 0) ? SpellConfig.dic[SpellDat.mimicSpellID] : SpellConfig.dic[SpellDat.id]);
			SetIconBuild(spellConf2, SpellDat.GetFinalLevel(), build, WandIndex);
			SetIconEffect((SpellDat.mimicSpellID != 0) ? WandSlotIconVisualEffect.Mimic : WandSlotIconVisualEffect.Normal);
			if (SpellDat.isAllFieldSharedSpell)
			{
				SetIconEffect(WandSlotIconVisualEffect.AllFieldEnhance);
			}
		}
	}

	public void Hover()
	{
		switch (SlotType)
		{
		case WandSlotType.Normal:
			image_Slot_Normal.sprite = sprite_Normal_Hover;
			break;
		case WandSlotType.Post:
			image_Slot_Post.sprite = sprite_Post_Hover;
			break;
		default:
			Debug.LogError(SlotType);
			break;
		}
		image_SpellIcon.transform.localScale = Vector3.one * hoverScale;
		(from e in GetComponentsInChildren<UISlotWandTipBase>()
			where e.gameObject.activeInHierarchy
			select e).Action(delegate(UISlotWandTipBase e)
		{
			e.Show();
		});
	}

	public void Unhover()
	{
		(from e in GetComponentsInChildren<UISlotWandTipBase>()
			where e.gameObject.activeInHierarchy
			select e).Action(delegate(UISlotWandTipBase e)
		{
			e.Hide();
		});
		switch (SlotType)
		{
		case WandSlotType.Normal:
			image_Slot_Normal.sprite = sprite_Normal_Unhover;
			break;
		case WandSlotType.Post:
			image_Slot_Post.sprite = sprite_Post_Unhover;
			break;
		default:
			Debug.LogError(SlotType);
			break;
		}
		image_SpellIcon.transform.localScale = Vector3.one;
	}

	public void HideIcon()
	{
		image_SpellIcon.gameObject.SetActive(value: false);
		image_Ring_Normal.gameObject.SetActive(value: false);
		image_Ring_Post.gameObject.SetActive(value: false);
		image_Link.gameObject.SetActive(value: false);
		image_Preshoot_Normal.color = new Color(1f, 1f, 1f, 0f);
		image_Preshoot_Post.color = new Color(1f, 1f, 1f, 0f);
		UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandGetUISlot(WandIndex, SpellIndex + 1, SlotType);
		if ((object)uISlotWand != null)
		{
			SlotData spellDat = uISlotWand.SpellDat;
			if (spellDat != null && spellDat.isSealSlot)
			{
				uISlotWand.HideIcon();
			}
		}
	}

	public void ShowIcon()
	{
		image_SpellIcon.gameObject.SetActive(value: true);
		if (SlotType == WandSlotType.Normal)
		{
			image_Ring_Normal.gameObject.SetActive(value: true);
		}
		else
		{
			image_Ring_Post.gameObject.SetActive(value: true);
		}
		if (needShowLink)
		{
			image_Link.gameObject.SetActive(value: true);
		}
		image_Preshoot_Normal.color = new Color(1f, 1f, 1f, 1f);
		image_Preshoot_Post.color = new Color(1f, 1f, 1f, 1f);
		int spellIndex = SpellIndex + 1;
		UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandGetUISlot(WandIndex, spellIndex, SlotType);
		if (!(uISlotWand == null) && uISlotWand.SpellDat != null && uISlotWand.SpellDat.isSealSlot)
		{
			uISlotWand.ShowIcon();
		}
	}

	public void ShowPreshootHint()
	{
		image_Preshoot_Normal.gameObject.SetActive(value: true);
	}

	public void HidePreshootHint()
	{
		image_Preshoot_Normal.gameObject.SetActive(value: false);
	}

	public void ShowUnusedHint(Wand.UnusedEnhanceType type)
	{
		image_Unused.GetComponent<UiSlotUnusedAlert>().Type = type;
		image_Unused.gameObject.SetActive(value: true);
	}

	public void HideUnusedHint()
	{
		image_Unused.gameObject.SetActive(value: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UISlotWandEnter(this);
		focusingThisSlot = true;
		if (eventData != null)
		{
			pointedInPosition = eventData.position;
		}
		if (!UIPlayerDataMgr.Inst.IsDraging && canDrag)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				QuickChange();
			}
			if (GameMgr.IsMobile_Static)
			{
				UIPlayerDataMgr.Inst._timeShortClick = UIPlayerDataMgr.Inst.timeShortClick;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UISlotWandExit(this);
		focusingThisSlot = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static && canDrag)
		{
			ResetIconImageState();
			UIPlayerDataMgr.Inst.UISlotWandDragBegin(this);
			ui_tips.SetActive(value: false);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static && canDrag)
		{
			if (Vector3.Distance(pointedInPosition, (Vector3)eventData.position) > base.transform.parent.transform.localScale.x * 50f / 3f && !isDragging)
			{
				ResetIconImageState();
				UIPlayerDataMgr.Inst.UISlotWandDragBegin(this);
				ui_tips.SetActive(value: false);
			}
			if (isDragging)
			{
				UIPlayerDataMgr.Inst.FindNearestSlot(eventData);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (canDrag)
		{
			focusingThisSlot = false;
			ResetIconImageState();
			UIPlayerDataMgr.Inst.UISlotWandDragEnd();
			ui_tips.SetActive(value: true);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (UIPlayerDataMgr.Inst.IsDraging || !canDrag)
		{
			return;
		}
		if (GameMgr.IsMobile_Static && UIPlayerDataMgr.Inst._timeShortClick > 0f)
		{
			if (!UIPlayerDataMgr.Inst.isChangingSpell)
			{
				SelectChangeSlot();
			}
			else if (UIPlayerDataMgr.Inst.uislotWandSelected == this && UIPlayerDataMgr.Inst._doubleClickthr > 0f)
			{
				UIPlayerDataMgr.Inst.ExChangeSlot(UIPlayerDataMgr.Inst.uislotWandSelected, UIPlayerDataMgr.Inst.uislotBagSelected, this, null);
				QuickChange();
			}
			else
			{
				UIPlayerDataMgr.Inst.ExChangeSlot(UIPlayerDataMgr.Inst.uislotWandSelected, UIPlayerDataMgr.Inst.uislotBagSelected, this, null);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			QuickChange();
		}
	}

	public void SelectChangeSlot()
	{
		if (SpellDat != null && !SpellDat.isSealSlot && !IsSlotLock)
		{
			UIPlayerDataMgr.Inst._doubleClickthr = UIPlayerDataMgr.Inst.doubleClickthr;
			UIPlayerDataMgr.Inst.isChangingSpell = true;
			UIPlayerDataMgr.Inst.uislotWandSelected = this;
			image_SpellIcon.GetComponent<RectTransform>().anchoredPosition = UIPlayerDataMgr.Inst.slotSelectedOffset;
			image_SpellIconOutline.gameObject.SetActive(value: true);
			image_SpellIcon.transform.localScale = UIPlayerDataMgr.Inst.slotSelectedScale * Vector3.one;
			UIPlayerDataMgr.Inst.TryUpdateDropArea(open: true, highlight: false);
			UIPlayerDataMgr.Inst.goMobileDropAreaHighLighted.gameObject.SetActive(value: false);
		}
	}

	public void QuickChange()
	{
		if (IsSlotLock || isSlotSeal || SpellDat == null)
		{
			return;
		}
		PlayerMgr.Inst.Wands[WandIndex].ReleaseCharge();
		if (SpellConfig.dic[SpellDat.id].abilityType == SpellAbilityType.ManaTendril)
		{
			SpellDat.specialInt = 0;
		}
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			int firstCanPushSlotDataIntoBagIndex = PlayerMgr.Inst.GetFirstCanPushSlotDataIntoBagIndex(SpellDat);
			if (firstCanPushSlotDataIntoBagIndex != -1)
			{
				ResetIconImageState();
				int id = SpellDat.id;
				PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(firstCanPushSlotDataIntoBagIndex, WandIndex, SlotType, SpellIndex);
				OnPointerExit(null);
				OnPointerEnter(null);
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UISpellFly", base.transform.position).GetComponent<UISpellFly>().Initialize(id, UIPlayerDataMgr.Inst.GetUISlotBag(firstCanPushSlotDataIntoBagIndex));
				break;
			}
		}
	}

	public void SetDrag(bool setdrag)
	{
		canDrag = setdrag;
	}

	public void SetHighLight()
	{
		imageRingMobileNearestPost.SetActive(SlotType == WandSlotType.Post);
		imageRingMobileNearest.gameObject.SetActive(SlotType != WandSlotType.Post);
	}

	public void SetUnHighLight()
	{
		imageRingMobileNearestPost.gameObject.SetActive(value: false);
		imageRingMobileNearest.gameObject.SetActive(value: false);
	}
}
