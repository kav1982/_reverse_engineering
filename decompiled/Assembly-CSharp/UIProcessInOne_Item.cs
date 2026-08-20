using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProcessInOne_Item : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public enum ProcessItemSlotType
	{
		CategorySlot,
		ProcessSlot
	}

	public ProcessItemSlotType slotType;

	public Text num;

	public ProcessItemData itemData;

	public int count;

	public Image image;

	public Image spellLevel1;

	public Image spellLevel2;

	public void Init(ProcessItemData itemData, ProcessItemSlotType slotType)
	{
		count = 1;
		this.itemData = itemData;
		this.slotType = slotType;
		image.sprite = itemData.GetImage();
		spellLevel1.gameObject.SetActive(value: false);
		spellLevel2.gameObject.SetActive(value: false);
		if (itemData.itemType == ProcessItemData.ProcessItemType.Spell && SpellConfig.dic[itemData.id].level != 1)
		{
			if (SpellConfig.dic[itemData.id].level == 2)
			{
				spellLevel1.gameObject.SetActive(value: true);
			}
			else if (SpellConfig.dic[itemData.id].level == 3)
			{
				spellLevel2.gameObject.SetActive(value: true);
			}
		}
	}

	public void Update()
	{
		num.text = count.ToString();
	}

	public void UpdateShow()
	{
		image.sprite = itemData.GetImage();
		spellLevel1.color = Color.white;
		spellLevel1.color = Color.white;
		switch (itemData.itemType)
		{
		case ProcessItemData.ProcessItemType.Spell:
			if (SpellConfig.dic[itemData.id].level == 1)
			{
				spellLevel1.gameObject.SetActive(value: false);
				spellLevel2.gameObject.SetActive(value: false);
			}
			else if (SpellConfig.dic[itemData.id].level == 2)
			{
				spellLevel1.gameObject.SetActive(value: true);
				spellLevel2.gameObject.SetActive(value: false);
			}
			else if (SpellConfig.dic[itemData.id].level == 2)
			{
				spellLevel1.gameObject.SetActive(value: false);
				spellLevel2.gameObject.SetActive(value: true);
			}
			else if (SpellConfig.dic[itemData.id].level == 3)
			{
				spellLevel1.gameObject.SetActive(value: true);
				spellLevel2.gameObject.SetActive(value: true);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case ProcessItemData.ProcessItemType.Relic:
		case ProcessItemData.ProcessItemType.Potion:
			break;
		}
		switch (slotType)
		{
		case ProcessItemSlotType.CategorySlot:
			image.color = (GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.CanAddSlot(this) ? Color.white : Color.gray);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case ProcessItemSlotType.ProcessSlot:
			break;
		}
	}

	public void CheckShowCanComoundHint()
	{
		if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.canCompoundItems.Contains(itemData))
		{
			if (SpellConfig.dic[itemData.id].level == 1)
			{
				spellLevel1.gameObject.SetActive(value: true);
				spellLevel1.DOFade(0.5f, 1f).From(0f).SetLoops(-1, LoopType.Yoyo)
					.SetUpdate(isIndependentUpdate: true);
			}
			else if (SpellConfig.dic[itemData.id].level == 2)
			{
				spellLevel2.gameObject.SetActive(value: true);
				spellLevel2.DOFade(0.5f, 1f).From(0f).SetLoops(-1, LoopType.Yoyo)
					.SetUpdate(isIndependentUpdate: true);
			}
		}
	}

	private void UpdateCurrentItemInfoShow(ProcessItemData itemData)
	{
		switch (itemData.itemType)
		{
		case ProcessItemData.ProcessItemType.Spell:
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoSpell.gameObject.SetActive(value: true);
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoSpell.UpdateInfo(itemData.id);
			UIMgr.AutoPivot(base.transform.position, GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoSpell.GetComponent<RectTransform>(), new Vector2(1f, 1f), useNewPivot: true, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffset, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffsetAuto);
			break;
		case ProcessItemData.ProcessItemType.Relic:
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoRelic.gameObject.SetActive(value: true);
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoRelic.UpdateInfo(DataMgr.selectedWorldData.battleData9.relicCfgs.First((RelicConfig x) => x.id == itemData.id));
			UIMgr.AutoPivot(base.transform.position, GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoRelic.GetComponent<RectTransform>(), new Vector2(1f, 1f), useNewPivot: true, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffset, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffsetAuto);
			break;
		case ProcessItemData.ProcessItemType.Potion:
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoPotion.gameObject.SetActive(value: true);
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoPotion.UpdateInfo(itemData.id);
			UIMgr.AutoPivot(base.transform.position, GameUISingletonMono<UIProcessInOne_Controller>.Inst.uiinfoPotion.GetComponent<RectTransform>(), new Vector2(1f, 1f), useNewPivot: true, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffset, UIMgr.Inst.UIMenu.uiCurseInfoPositionOffsetAuto);
			break;
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentSelectedItemSlot = this;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentSelectedItemSlot == null || GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentSelectedItemSlot != this)
		{
			UpdateCurrentItemInfoShow(itemData);
			return;
		}
		switch (slotType)
		{
		case ProcessItemSlotType.CategorySlot:
			if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.CanAddSlot(this))
			{
				GameUISingletonMono<UIProcessInOne_Controller>.Inst.HideInfoPanels();
				GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentSelectedItemSlot = null;
				GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.AddSelectedItem(itemData);
			}
			break;
		case ProcessItemSlotType.ProcessSlot:
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.HideInfoPanels();
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentSelectedItemSlot = null;
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.RemoveItem(this);
			break;
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}
}
