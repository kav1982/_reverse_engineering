using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotBag : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public Image image_BG;

	public Image image_IconOutline;

	public Image image_Icon;

	public Image image_Star1;

	public Image image_Star2;

	public Image image_RingNormal;

	public Image image_Link;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	public float hoverScale;

	public GameObject imageRingMobileNearest;

	[Header("Relic_Pandora")]
	public GameObject go_Relic_PandoraBoxRoot;

	public RectTransform rtsf_PandoraBoxRotate;

	public float relicPandoraBoxRotateSpeed;

	public static float lackSlotAlertTimer;

	public static bool lackSlotAlertTimerUpdateInThisFrame;

	private bool focusingThisSlot;

	private bool isFromBuild;

	private FinishGameBuild finishGameBuild;

	[Header("手游")]
	private Vector3 pointedInPosition;

	private bool needShowLink
	{
		get
		{
			if (SpellDat == null)
			{
				return false;
			}
			if (SpellDat.isSealSlot)
			{
				int num = BagIndex + 1;
				SlotData[] array = PlayerMgr.Inst.BaData.bagSpellDatas.ToArray();
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

	public int BagIndex { get; private set; }

	public SlotData SpellDat
	{
		get
		{
			if (isFromBuild)
			{
				if (finishGameBuild.bagSpellDatas == null || finishGameBuild.bagSpellDatas.Count <= BagIndex)
				{
					return null;
				}
				return finishGameBuild.bagSpellDatas[BagIndex];
			}
			return PlayerMgr.Inst.BaData.bagSpellDatas[BagIndex];
		}
	}

	public SpellConfig SpellCfg => SpellConfig.dic[SpellDat.id];

	private bool isDragging => UIPlayerDataMgr.Inst.uiSlotBag_Drag == this;

	public void Initialize(int bagIndex, FinishGameBuild finishGameBuild = null)
	{
		if (finishGameBuild == null)
		{
			isFromBuild = false;
		}
		else
		{
			isFromBuild = true;
			this.finishGameBuild = finishGameBuild;
		}
		BagIndex = bagIndex;
		UpdateInfo();
	}

	private void Awake()
	{
		UIGamePadNav uIGamePadNav = base.transform.AddComponent<UIGamePadNav>();
		uIGamePadNav.OnDeselectAction = OnPointerExit;
		uIGamePadNav.OnSelectAction = OnPointerEnter;
	}

	private void Update()
	{
		JustAnotherFunctionMakeBy永宁();
		PandoraBoxRotate();
	}

	private void JustAnotherFunctionMakeBy永宁()
	{
		if (isFromBuild)
		{
			return;
		}
		if (!UIPlayerDataMgr.Inst.IsDraging && Input.GetKey(KeyCode.LeftShift) && focusingThisSlot)
		{
			QuickChange();
		}
		if (lackSlotAlertTimerUpdateInThisFrame)
		{
			if (lackSlotAlertTimer >= 0f)
			{
				lackSlotAlertTimer -= Time.deltaTime;
			}
			lackSlotAlertTimerUpdateInThisFrame = false;
		}
	}

	private void PandoraBoxRotate()
	{
		if (go_Relic_PandoraBoxRoot.activeSelf)
		{
			rtsf_PandoraBoxRotate.Rotate(0f, 0f, relicPandoraBoxRotateSpeed * Time.unscaledDeltaTime);
		}
	}

	private void LateUpdate()
	{
		if (!isFromBuild)
		{
			lackSlotAlertTimerUpdateInThisFrame = true;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				lackSlotAlertTimer = 0f;
			}
			if (PlayerMgr.Inst.BaData != null && PlayerMgr.Inst.BaData.bagSpellDatas != null && SpellDat == null && image_Icon != null && image_Icon.gameObject != null && image_Icon.gameObject.activeSelf)
			{
				image_Icon.gameObject.SetActive(value: false);
			}
		}
	}

	private void BagDisplayLinkedSealSlots()
	{
		if (SpellDat != null && !SpellDat.isSealSlot)
		{
			int slotCost = SpellConfig.dic[SpellDat.id].slotCost;
			for (int i = 1; i < slotCost; i++)
			{
				PlayerMgr.Inst.BaData.bagSpellDatas[BagIndex + i] = PlayerMgr.Inst.CreateSealedSlotData(SpellDat);
				UIPlayerDataMgr.Inst.UpdateBag(BagIndex + i);
			}
		}
	}

	public void UpdateInfo()
	{
		image_Icon.gameObject.SetActive(value: false);
		image_Icon.color = Color.white;
		image_Star1.gameObject.SetActive(value: false);
		image_Star2.gameObject.SetActive(value: false);
		image_RingNormal.gameObject.SetActive(value: false);
		image_Link.gameObject.SetActive(value: false);
		if (SpellDat != null)
		{
			if (needShowLink)
			{
				image_Link.gameObject.SetActive(needShowLink);
				image_Link.color = linkColor;
			}
			if (SpellDat.isSealSlot)
			{
				SpellConfig finalConfig = SpellDat.sealSlotOwner.GetFinalConfig();
				image_RingNormal.gameObject.SetActive(value: true);
				image_RingNormal.color = linkColor;
				image_Icon.sprite = ABResources.LoadAsset<Sprite>(finalConfig.GetIconPath());
				image_IconOutline.sprite = image_Icon.sprite;
				image_Icon.gameObject.SetActive(value: true);
				Color color = image_Icon.color;
				color.a = 0.4f;
				image_Icon.color = color;
			}
			if (SpellDat.id != 0 && !SpellDat.isSealSlot)
			{
				image_Icon.gameObject.SetActive(value: true);
				image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellCfg.GetIconPath());
				image_IconOutline.sprite = image_Icon.sprite;
				if (SpellCfg.level >= 2)
				{
					image_Star1.gameObject.SetActive(value: true);
				}
				if (SpellCfg.level >= 3)
				{
					image_Star2.gameObject.SetActive(value: true);
				}
				image_RingNormal.gameObject.SetActive(value: true);
				switch (SpellCfg.useType)
				{
				case SpellType.Missile:
				case SpellType.Summon:
					image_RingNormal.color = GameConst.color_SpellRingTypeMissle;
					break;
				case SpellType.Enhance:
					image_RingNormal.color = GameConst.color_SpellRingTypeEnhance;
					break;
				case SpellType.Passive:
					image_RingNormal.color = GameConst.color_SpellRingTypePassive;
					break;
				default:
					Debug.LogError(SpellCfg.useType);
					break;
				}
			}
		}
		BagDisplayLinkedSealSlots();
	}

	public bool GetFocusState()
	{
		return focusingThisSlot;
	}

	public void Hover()
	{
		if ((bool)image_BG)
		{
			image_BG.sprite = sprite_Hover;
		}
		if ((bool)image_Icon.transform)
		{
			image_Icon.transform.localScale = Vector3.one * hoverScale;
		}
	}

	public void Unhover()
	{
		image_BG.sprite = sprite_Unhover;
		if (!image_Icon.IsDestroyed())
		{
			image_Icon.transform.localScale = Vector3.one;
		}
	}

	public void HideIcon()
	{
		image_Icon.gameObject.SetActive(value: false);
		image_RingNormal.gameObject.SetActive(value: false);
		image_Link.gameObject.SetActive(value: false);
		int num = BagIndex + 1;
		while (true)
		{
			UISlotBag uISlotBag = UIPlayerDataMgr.Inst.GetUISlotBag(num);
			if (!(uISlotBag == null) && uISlotBag.SpellDat != null && uISlotBag.SpellDat.isSealSlot)
			{
				uISlotBag.HideIcon();
				num++;
				continue;
			}
			break;
		}
	}

	public void ShowIcon()
	{
		image_Icon.gameObject.SetActive(value: true);
		image_RingNormal.gameObject.SetActive(value: true);
		if (needShowLink)
		{
			image_Link.gameObject.SetActive(value: true);
		}
		int num = BagIndex + 1;
		while (true)
		{
			UISlotBag uISlotBag = UIPlayerDataMgr.Inst.GetUISlotBag(num);
			if (!(uISlotBag == null) && uISlotBag.SpellDat != null && uISlotBag.SpellDat.isSealSlot)
			{
				uISlotBag.ShowIcon();
				num++;
				continue;
			}
			break;
		}
	}

	public void SetPandoraBoxEffect(bool pandoraEffect)
	{
		go_Relic_PandoraBoxRoot.SetActive(pandoraEffect);
		Unhover();
	}

	public void SelectChangeSlot()
	{
		if (SpellDat != null && !SpellDat.isSealSlot)
		{
			UIPlayerDataMgr.Inst._doubleClickthr = UIPlayerDataMgr.Inst.doubleClickthr;
			UIPlayerDataMgr.Inst.isChangingSpell = true;
			UIPlayerDataMgr.Inst.uislotBagSelected = this;
			image_Icon.GetComponent<RectTransform>().anchoredPosition = UIPlayerDataMgr.Inst.slotSelectedOffset;
			image_IconOutline.gameObject.SetActive(value: true);
			image_Icon.transform.localScale = UIPlayerDataMgr.Inst.slotSelectedScale * Vector3.one;
			UIPlayerDataMgr.Inst.TryUpdateDropArea(open: true, highlight: false);
			UIPlayerDataMgr.Inst.goMobileDropAreaHighLighted.gameObject.SetActive(value: false);
		}
	}

	public void QuickChange()
	{
		if (isFromBuild || SpellDat == null || SpellDat.isSealSlot)
		{
			return;
		}
		int selectedWandIndex = PlayerMgr.Inst.SelectedWandIndex;
		if (selectedWandIndex < 0)
		{
			return;
		}
		if (!PlayerMgr.Inst.CheckIfSpellOverSizeToPutInWand(PlayerMgr.Inst.SelectedWandCfg, WandSlotType.Normal, SpellDat.id) && !PlayerMgr.Inst.CheckIfSpellOverSizeToPutInWand(PlayerMgr.Inst.SelectedWandCfg, WandSlotType.Post, SpellDat.id))
		{
			Debug.Log("法杖格子不足");
			return;
		}
		WandSlotType slotType = WandSlotType.Normal;
		int num = -1;
		WandSlotType[] array = new WandSlotType[2]
		{
			WandSlotType.Normal,
			WandSlotType.Post
		};
		foreach (WandSlotType wandSlotType in array)
		{
			int num2 = PlayerMgr.Inst.SelectedWandCfg.GetSlotsData(wandSlotType).Bag_GetFirstCanSetWithPushSlotIndex(PlayerMgr.Inst.SelectedWandCfg.GetSlotsLockState(wandSlotType), SpellDat);
			if (num2 >= 0)
			{
				num = num2;
				slotType = wandSlotType;
				break;
			}
		}
		if (num >= 0)
		{
			int id = SpellDat.id;
			if (SpellDat != null && !SpellDat.isSealSlot)
			{
				int slotCost = SpellConfig.dic[SpellDat.id].slotCost;
				for (int j = 1; j < slotCost; j++)
				{
					SlotData slotData = PlayerMgr.Inst.BaData.bagSpellDatas[BagIndex + j];
					if (slotData != null && slotData.id != 0)
					{
						MonoBehaviour.print("这不对啊 为什么会再占用格子里找到其他法术的信息?");
						continue;
					}
					PlayerMgr.Inst.BaData.bagSpellDatas[BagIndex + j] = null;
					UIPlayerDataMgr.Inst.UpdateBag(BagIndex + j);
				}
			}
			Vector3 position = UIPlayerDataMgr.Inst.WandGetUISlot(selectedWandIndex, num, slotType).transform.position;
			PlayerMgr.Inst.Slot_SwapSlotBetweenBagAndWand(BagIndex, selectedWandIndex, slotType, num);
			OnPointerExit(null);
			OnPointerEnter(null);
			if (GameMgr.IsMobile_Static)
			{
				UISpellFly component = ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UISpellFly", base.transform.position, (RectTransform)UIMgr.Inst.canvas11.transform).GetComponent<UISpellFly>();
				component.Initialize(id, UIPlayerDataMgr.Inst.WandGetUISlot(selectedWandIndex, num, slotType), position);
				component.spellCanvas.sortingOrder = 200;
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UISpellFly", base.transform.position).GetComponent<UISpellFly>().Initialize(id, UIPlayerDataMgr.Inst.WandGetUISlot(selectedWandIndex, num, slotType), position);
			}
		}
		else if (!UIPlayerDataMgr.Inst.IsDraging && Input.GetKey(KeyCode.LeftShift))
		{
			if (lackSlotAlertTimer <= 0f)
			{
				lackSlotAlertTimer = 1f;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002014.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint + new Vector3(-0.5f, 0.8f, 0f));
			}
		}
		else
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002014.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint + new Vector3(-0.5f, 0.8f, 0f));
		}
		if (num >= 0)
		{
			UISlotWand uISlotWand = UIPlayerDataMgr.Inst.WandGetUISlot(selectedWandIndex, num, slotType);
			uISlotWand.SetIcon(state: false);
			uISlotWand.image_MimicError.gameObject.SetActive(value: false);
		}
		if (PlayerMgr.Inst.SelectedWand != null && PlayerMgr.Inst.SelectedWand.gameObject.activeInHierarchy)
		{
			PlayerMgr.Inst.SelectedWand.ResetAndRecheck();
		}
	}

	public void SetHighLight()
	{
		imageRingMobileNearest.gameObject.SetActive(value: true);
	}

	public void SetUnHighLight()
	{
		imageRingMobileNearest.gameObject.SetActive(value: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UISlotBagEnter(this);
		focusingThisSlot = true;
		if (eventData != null)
		{
			pointedInPosition = eventData.position;
		}
		if (!UIPlayerDataMgr.Inst.IsDraging)
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
		UIPlayerDataMgr.Inst.UISlotBagExit(this);
		focusingThisSlot = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!isFromBuild && !GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.UISlotBagDragBegin(this);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!isFromBuild && GameMgr.IsMobile_Static)
		{
			if (Vector3.Distance(pointedInPosition, (Vector3)eventData.position) > UIPlayerDataMgr.Inst.uiBag.transform.localScale.x * 50f / 3f && !isDragging)
			{
				UIPlayerDataMgr.Inst.UISlotBagDragBegin(this);
			}
			if (isDragging)
			{
				UIPlayerDataMgr.Inst.FindNearestSlot(eventData);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!isFromBuild)
		{
			UIPlayerDataMgr.Inst.UISlotBagDragEnd();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (UIPlayerDataMgr.Inst.IsDraging)
		{
			return;
		}
		if (GameMgr.IsMobile_Static && UIPlayerDataMgr.Inst._timeShortClick > 0f)
		{
			if (!UIPlayerDataMgr.Inst.isChangingSpell)
			{
				SelectChangeSlot();
			}
			else if (UIPlayerDataMgr.Inst.uislotBagSelected == this && UIPlayerDataMgr.Inst._doubleClickthr > 0f)
			{
				UIPlayerDataMgr.Inst.ExChangeSlot(UIPlayerDataMgr.Inst.uislotWandSelected, UIPlayerDataMgr.Inst.uislotBagSelected, null, this);
				QuickChange();
			}
			else
			{
				UIPlayerDataMgr.Inst.ExChangeSlot(UIPlayerDataMgr.Inst.uislotWandSelected, UIPlayerDataMgr.Inst.uislotBagSelected, null, this);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			QuickChange();
		}
	}
}
