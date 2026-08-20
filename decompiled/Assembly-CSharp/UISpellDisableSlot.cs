using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpellDisableSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image image_BG;

	public Image image_Icon;

	public Image image_Disable;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	private UISpellDisable uiSpellDisable;

	public int Level1ID { get; private set; }

	public bool UnLocked { get; private set; }

	public bool AlreadyDisable { get; private set; }

	public int Index { get; private set; }

	public void Initialize(UISpellDisable uiSpellDisable, int spellID, int index)
	{
		this.uiSpellDisable = uiSpellDisable;
		Level1ID = spellID;
		Index = index;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[spellID].GetIconPath());
		for (int i = 0; i < DataMgr.selectedWorldData.galleryUnlockedSpells.Count; i++)
		{
			if (DataMgr.selectedWorldData.galleryUnlockedSpells[i] / 10 * 10 + 1 == Level1ID)
			{
				UnLocked = true;
				break;
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
		{
			UnLocked = true;
		}
		if (UnLocked)
		{
			image_Icon.color = Color.white;
		}
		else
		{
			image_Icon.color = Color.black;
		}
	}

	public void Hover()
	{
		image_BG.sprite = sprite_Hover;
	}

	public void Unhover()
	{
		image_BG.sprite = sprite_Unhover;
	}

	public void SetDisable()
	{
		AlreadyDisable = true;
		image_Disable.gameObject.SetActive(value: true);
	}

	public void SetEnable()
	{
		AlreadyDisable = false;
		image_Disable.gameObject.SetActive(value: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData == null || !GameMgr.IsMobile_Static)
		{
			uiSpellDisable.SlotEnter(this);
			if (eventData != null)
			{
				SEMgr.Inst.uiButtonSwitch.PlaySE();
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData == null || !GameMgr.IsMobile_Static)
		{
			uiSpellDisable.SlotExit(this);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		uiSpellDisable.SlotClick(this);
	}
}
