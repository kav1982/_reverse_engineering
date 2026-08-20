using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEndlessGallerySlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
	public Image image_BG;

	public Image image_Icon;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	private UIEndlessGallery uiEndlessGallery;

	public int UnitID { get; private set; }

	public bool IsUnlocked { get; private set; }

	public void Initialize(UIEndlessGallery uiEndlessGallery, int unitID, bool isUnlocked)
	{
		this.uiEndlessGallery = uiEndlessGallery;
		UnitID = unitID;
		IsUnlocked = isUnlocked;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(UnitConfig.map[unitID].GetIconPath());
		image_Icon.color = (IsUnlocked ? Color.white : Color.black);
	}

	public void Hover()
	{
		image_BG.sprite = sprite_Hover;
	}

	public void Unhover()
	{
		image_BG.sprite = sprite_Unhover;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			if (eventData != null)
			{
				return;
			}
			OnPointerClick(null);
		}
		uiEndlessGallery.SlotEnter(this);
		if (eventData != null)
		{
			SEMgr.Inst.uiButtonSwitch.PlaySE();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			uiEndlessGallery.SlotEnter(this);
			if (eventData != null)
			{
				SEMgr.Inst.uiButtonSwitch.PlaySE();
			}
		}
	}
}
