using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProcessInOne_CatergorySlot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public Sprite spriteAll;

	public Sprite spriteBag;

	public Sprite spriteRelic;

	public Sprite spriteSpell;

	public Sprite spritePotion;

	public Image image;

	public GameObject selectedHighlight;

	public UIProcessInOne_Catergorys.SlotType SlotType;

	public int id;

	public void OnPointerClick(PointerEventData eventData)
	{
		Select();
	}

	public void Init(UIProcessInOne_Controller.UIProcessInOneType processInOneType, UIProcessInOne_Catergorys.SlotType slotType = UIProcessInOne_Catergorys.SlotType.All, int id = 0)
	{
		SlotType = slotType;
		this.id = id;
		switch (slotType)
		{
		case UIProcessInOne_Catergorys.SlotType.All:
			image.sprite = spriteAll;
			break;
		case UIProcessInOne_Catergorys.SlotType.Spell:
			image.sprite = spriteSpell;
			break;
		case UIProcessInOne_Catergorys.SlotType.Bag:
			image.sprite = spriteBag;
			break;
		case UIProcessInOne_Catergorys.SlotType.Wand:
		{
			Sprite sprite3 = (image.sprite = (image.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[DataMgr.selectedWorldData.battleData9.wandCfgs[id].id].GetIconPath())));
			break;
		}
		case UIProcessInOne_Catergorys.SlotType.Relic:
			image.sprite = spriteRelic;
			break;
		case UIProcessInOne_Catergorys.SlotType.Potion:
			image.sprite = spritePotion;
			break;
		default:
			throw new ArgumentOutOfRangeException("slotType", slotType, null);
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.currentItemsSlots.Add(this, new List<UIProcessInOne_Item>());
	}

	public void Select()
	{
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.catergorys.selectedCatergorySlot = this;
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}
}
