using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIProcessInOne_ItemContainer : MonoBehaviour
{
	[Flags]
	public enum FilterSpellType
	{
		None = 0,
		Missile = 1,
		Summon = 2,
		Enhance = 4,
		Passive = 8
	}

	[Flags]
	public enum FilterSpellRarityType
	{
		None = 0,
		Common = 1,
		Rare = 2,
		Epic = 4,
		Special = 8
	}

	public UIProcessInOne_Item itemPrefab;

	public Transform itemContainer;

	public GameObject filterObj;

	public List<ProcessItemData> allItemsData = new List<ProcessItemData>();

	public List<ProcessItemData> currentCategorySlots = new List<ProcessItemData>();

	public Dictionary<UIProcessInOne_CatergorySlot, List<UIProcessInOne_Item>> currentItemsSlots = new Dictionary<UIProcessInOne_CatergorySlot, List<UIProcessInOne_Item>>();

	public HashSet<ProcessItemData> canCompoundItems = new HashSet<ProcessItemData>(new ProcessItemData.ProcessItemDataIdTypeComparer());

	public int compoundMateriallv1;

	public int compoundMateriallv2;

	private int SpellTypeFilterMask = 15;

	private int SpellRarityFilterMask = 7;

	public List<ProcessItemData> selectedItem => allItemsData.Where((ProcessItemData item) => item.selected).ToList();

	public UIProcessInOne_CatergorySlot selectedCatergorySlot => GameUISingletonMono<UIProcessInOne_Controller>.Inst.catergorys.selectedCatergorySlot;

	public int?[] idOnly { get; set; }

	public int? numLimit { get; set; }

	public void _ToggleSpellMissile(bool toggleValue)
	{
		SetTypeFilter(FilterSpellType.Missile, toggleValue);
	}

	public void _ToggleSpellSummon(bool toggleValue)
	{
		SetTypeFilter(FilterSpellType.Summon, toggleValue);
	}

	public void _ToggleSpellEnhance(bool toggleValue)
	{
		SetTypeFilter(FilterSpellType.Enhance, toggleValue);
	}

	public void _ToggleSpellPassive(bool toggleValue)
	{
		SetTypeFilter(FilterSpellType.Passive, toggleValue);
	}

	public void _ToggleSpellCommon(bool toggleValue)
	{
		SetRarityFilter(FilterSpellRarityType.Common, toggleValue);
	}

	public void _ToggleSpellRare(bool toggleValue)
	{
		SetRarityFilter(FilterSpellRarityType.Rare, toggleValue);
	}

	public void _ToggleSpellEpic(bool toggleValue)
	{
		SetRarityFilter(FilterSpellRarityType.Epic, toggleValue);
	}

	public void _ToggleSpellSpecial(bool toggleValue)
	{
		SetRarityFilter(FilterSpellRarityType.Special, toggleValue);
	}

	public void SetTypeFilter(FilterSpellType filterSpellType, bool enable)
	{
		if (enable)
		{
			SpellTypeFilterMask |= (int)filterSpellType;
		}
		else
		{
			SpellTypeFilterMask &= (int)(~filterSpellType);
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}

	public void SetRarityFilter(FilterSpellRarityType rarityType, bool enable)
	{
		if (enable)
		{
			SpellRarityFilterMask |= (int)rarityType;
		}
		else
		{
			SpellRarityFilterMask &= (int)(~rarityType);
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}

	public void _SwitchFilterObj()
	{
		filterObj.gameObject.SetActive(!filterObj.gameObject.activeSelf);
	}

	public bool IsSpellAllowed(SpellType spellType, ItemDropType itemDropType)
	{
		FilterSpellType type2 = FilterSpellType.None;
		FilterSpellRarityType rarityType2 = FilterSpellRarityType.None;
		switch (spellType)
		{
		case SpellType.Missile:
			type2 = FilterSpellType.Missile;
			break;
		case SpellType.Summon:
			type2 = FilterSpellType.Summon;
			break;
		case SpellType.Enhance:
			type2 = FilterSpellType.Enhance;
			break;
		case SpellType.Passive:
			type2 = FilterSpellType.Passive;
			break;
		}
		switch (itemDropType)
		{
		case ItemDropType.Common:
			rarityType2 = FilterSpellRarityType.Common;
			break;
		case ItemDropType.Rare:
			rarityType2 = FilterSpellRarityType.Rare;
			break;
		case ItemDropType.Epic:
			rarityType2 = FilterSpellRarityType.Epic;
			break;
		case ItemDropType.Special:
			rarityType2 = FilterSpellRarityType.Special;
			break;
		}
		return _isSpellAllowed(type2, rarityType2);
		bool _isSpellAllowed(FilterSpellType type, FilterSpellRarityType rarityType)
		{
			bool num = ((uint)SpellTypeFilterMask & (uint)type) != 0;
			bool flag = ((uint)SpellRarityFilterMask & (uint)rarityType) != 0;
			return num && flag;
		}
	}

	public void InitAllItem()
	{
		allItemsData.Clear();
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			AddEverySpell();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
			AddEverySpell();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
			AddEverySpell();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
			AddEveryRelic();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			AddEverySpell();
			AddEveryPotion();
			AddEveryRelic();
			break;
		}
	}

	public void UpdateCurrentSlots()
	{
		switch (selectedCatergorySlot.SlotType)
		{
		case UIProcessInOne_Catergorys.SlotType.All:
			currentCategorySlots = allItemsData;
			break;
		case UIProcessInOne_Catergorys.SlotType.Bag:
			currentCategorySlots = allItemsData.Where((ProcessItemData x) => x.source == ProcessItemData.Source.Bag).ToList();
			break;
		case UIProcessInOne_Catergorys.SlotType.Wand:
			currentCategorySlots = allItemsData.Where((ProcessItemData x) => x.source == ProcessItemData.Source.Wand && x.SourceID1 == selectedCatergorySlot.id).ToList();
			break;
		case UIProcessInOne_Catergorys.SlotType.Relic:
			currentCategorySlots = allItemsData.Where((ProcessItemData x) => x.itemType == ProcessItemData.ProcessItemType.Relic).ToList();
			break;
		case UIProcessInOne_Catergorys.SlotType.Potion:
			currentCategorySlots = allItemsData.Where((ProcessItemData x) => x.itemType == ProcessItemData.ProcessItemType.Potion).ToList();
			break;
		case UIProcessInOne_Catergorys.SlotType.Spell:
			currentCategorySlots = allItemsData.Where((ProcessItemData x) => x.itemType == ProcessItemData.ProcessItemType.Spell).ToList();
			break;
		}
	}

	public void UpdateSlots()
	{
		List<ProcessItemData> list = new List<ProcessItemData>(currentCategorySlots);
		itemContainer.DestroyAllChildImmediate();
		currentItemsSlots[selectedCatergorySlot].Clear();
		list.Sort(delegate(ProcessItemData a, ProcessItemData b)
		{
			bool value = canCompoundItems.Contains(a);
			int num = canCompoundItems.Contains(b).CompareTo(value);
			if (num != 0)
			{
				return num;
			}
			int num2 = a.itemType.CompareTo(b.itemType);
			return (num2 != 0) ? num2 : a.id.CompareTo(b.id);
		});
		UIProcessInOne_Item uIProcessInOne_Item = null;
		foreach (ProcessItemData item in list)
		{
			if (!item.selected)
			{
				if (uIProcessInOne_Item == null || !uIProcessInOne_Item.itemData.SameItem(item))
				{
					UIProcessInOne_Item uIProcessInOne_Item2 = UnityEngine.Object.Instantiate(itemPrefab, itemContainer.transform);
					uIProcessInOne_Item = uIProcessInOne_Item2;
					uIProcessInOne_Item2.Init(item, UIProcessInOne_Item.ProcessItemSlotType.CategorySlot);
					AddToDic(uIProcessInOne_Item2);
				}
				else
				{
					uIProcessInOne_Item.count++;
				}
			}
		}
		currentItemsSlots[selectedCatergorySlot].ForEach(delegate(UIProcessInOne_Item slot)
		{
			slot.UpdateShow();
			if (slot.itemData.itemType == ProcessItemData.ProcessItemType.Spell)
			{
				slot.gameObject.SetActive(IsSpellAllowed(SpellConfig.dic[slot.itemData.id].useType, SpellConfig.dic[slot.itemData.id].dropType));
			}
			else
			{
				slot.gameObject.SetActive(value: true);
			}
		});
	}

	public void AddEveryPotion()
	{
		DataMgr.selectedWorldData.battleData9.potionIDs.ForEach(delegate(int x)
		{
			if (x != 0)
			{
				ProcessItemData item = new ProcessItemData(x, ProcessItemData.ProcessItemType.Potion);
				AddItemDataToList(item);
			}
		});
	}

	public void AddEveryRelic()
	{
		DataMgr.selectedWorldData.battleData9.relicCfgs.ForEach(delegate(RelicConfig x)
		{
			ProcessItemData item = new ProcessItemData(x.id, ProcessItemData.ProcessItemType.Relic);
			AddItemDataToList(item);
		});
	}

	private void AddEverySpell()
	{
		AddBagSpell();
		(from x in DataMgr.selectedWorldData.battleData9.wandCfgs.Select((WandConfig wandcfg, int index) => new { wandcfg, index })
			where x.wandcfg != null
			select x).ToList().ForEach(x =>
		{
			AddWandSpell(x.index);
		});
	}

	private void AddWandSpell(int wandID = -1)
	{
		WandConfig wandconfig = DataMgr.selectedWorldData.battleData9.wandCfgs[wandID];
		(from x in new[]
			{
				wandconfig.postSlots.Select((SlotData slot, int index) => new
				{
					slot = slot,
					index = index,
					isLock = wandconfig.postSlotIsLock[index],
					isPost = true
				}),
				wandconfig.normalSlots.Select((SlotData slot, int index) => new
				{
					slot = slot,
					index = index,
					isLock = wandconfig.normalSlotIsLock[index],
					isPost = false
				})
			}.SelectMany(x => x)
			where x.slot != null && x.slot.id != 0 && !x.isLock
			select x).ToList().ForEach(slotAndIndex =>
		{
			ProcessItemData item = new ProcessItemData(slotAndIndex.slot.id, ProcessItemData.ProcessItemType.Spell, ProcessItemData.Source.Wand, wandID, slotAndIndex.index, slotAndIndex.isPost ? WandSlotType.Post : WandSlotType.Normal);
			AddItemDataToList(item);
		});
	}

	private void AddBagSpell()
	{
		(from idAndSlot in DataMgr.selectedWorldData.battleData9.bagSpellDatas.Select((SlotData x, int index) => new
			{
				index = index,
				slotData = x
			})
			where idAndSlot.slotData != null && idAndSlot.slotData.id != 0
			select new
			{
				idAndSlot.slotData.id,
				idAndSlot.index
			}).ToList().ForEach(idAndIndex =>
		{
			ProcessItemData item = new ProcessItemData(idAndIndex.id, ProcessItemData.ProcessItemType.Spell, ProcessItemData.Source.Bag, 0, idAndIndex.index);
			AddItemDataToList(item);
		});
	}

	private void AddToDic(UIProcessInOne_Item item)
	{
		currentItemsSlots[selectedCatergorySlot].Add(item);
	}

	private void AddItemDataToList(ProcessItemData item)
	{
		allItemsData.Add(item);
	}

	public void GenerateCanCompoundList()
	{
		canCompoundItems.Clear();
		foreach (ProcessItemData item in from x in GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.allItemsData
			group x by new { x.id, x.itemType } into g
			where g.Count() >= 3
			select g.First())
		{
			canCompoundItems.Add(item);
		}
	}
}
