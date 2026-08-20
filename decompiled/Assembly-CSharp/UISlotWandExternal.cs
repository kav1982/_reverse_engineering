using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotWandExternal : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IPointerClickHandler
{
	public Image image_Slot_Normal;

	public Image image_Slot_Post;

	public Image image_Ring_Normal;

	public Image image_Ring_Post;

	public Image image_PostTypeIcon;

	public Image image_SpellIcon;

	public Image image_SpellLevel2Star;

	public Image image_SpellLevel3Star;

	public Image image_Lock;

	public float hoverScale;

	public WandConfig WandCfg { get; private set; }

	public WandSlotType SlotType { get; private set; }

	public int Index { get; private set; }

	public SlotData SlotDat
	{
		get
		{
			switch (SlotType)
			{
			case WandSlotType.Normal:
				return WandCfg.normalSlots[Index];
			case WandSlotType.Post:
				return WandCfg.postSlots[Index];
			default:
				Debug.LogError(SlotType);
				return null;
			}
		}
	}

	public SpellConfig SpellCfg
	{
		get
		{
			if (SlotDat != null && !SlotDat.isSealSlot)
			{
				return SpellConfig.dic[SlotDat.id];
			}
			return null;
		}
	}

	public void Initialize(WandConfig wandCfg, int index, WandSlotType slotType)
	{
		WandCfg = wandCfg;
		SlotType = slotType;
		Index = index;
		switch (slotType)
		{
		case WandSlotType.Normal:
			image_Slot_Normal.gameObject.SetActive(value: true);
			break;
		case WandSlotType.Post:
			image_Slot_Post.gameObject.SetActive(value: true);
			image_PostTypeIcon.gameObject.SetActive(value: true);
			image_PostTypeIcon.sprite = wandCfg.GetPostSlotIcon();
			break;
		default:
			Debug.LogWarning(slotType);
			break;
		}
		if (WandCfg.IsSlotLock(slotType, index))
		{
			image_Lock.gameObject.SetActive(value: true);
		}
		if (SlotDat == null || SlotDat.isSealSlot)
		{
			return;
		}
		image_SpellIcon.gameObject.SetActive(value: true);
		image_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(SpellCfg.GetIconPath());
		if (SpellCfg.level >= 2)
		{
			image_SpellLevel2Star.gameObject.SetActive(value: true);
		}
		if (SpellCfg.level >= 3)
		{
			image_SpellLevel3Star.gameObject.SetActive(value: true);
		}
		Image image = null;
		switch (slotType)
		{
		case WandSlotType.Normal:
			image_Ring_Normal.gameObject.SetActive(value: true);
			image = image_Ring_Normal;
			break;
		case WandSlotType.Post:
			image_Ring_Post.gameObject.SetActive(value: true);
			image = image_Ring_Post;
			break;
		default:
			Debug.LogWarning(slotType);
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

	public void Hover()
	{
		image_SpellIcon.transform.localScale = Vector3.one * hoverScale;
	}

	public void Unhover()
	{
		image_SpellIcon.transform.localScale = Vector3.one;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UISlotWandExternalEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UISlotWandExternalExit(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			UIRewardWand component = base.transform.parent.parent.parent.GetComponent<UIRewardWand>();
			if ((bool)component)
			{
				component.OnPointerClick(eventData);
			}
		}
	}
}
