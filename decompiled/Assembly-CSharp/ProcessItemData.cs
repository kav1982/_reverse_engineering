using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProcessItemData
{
	public enum ProcessItemType
	{
		Spell,
		Relic,
		Potion
	}

	public enum Source
	{
		Bag,
		Wand
	}

	public class ProcessItemDataIdTypeComparer : IEqualityComparer<ProcessItemData>
	{
		public bool Equals(ProcessItemData x, ProcessItemData y)
		{
			if (x != null && y != null && x.id == y.id)
			{
				return x.itemType == y.itemType;
			}
			return false;
		}

		public int GetHashCode(ProcessItemData obj)
		{
			return obj.id.GetHashCode() ^ obj.itemType.GetHashCode();
		}
	}

	public bool selected;

	public int id;

	public ProcessItemType itemType;

	public Source source;

	public WandSlotType spellSource;

	public int SourceID1;

	public int SourceID2;

	public SlotData slotData;

	private IEquatable<ProcessItemData> _equatableImplementation;

	public ProcessItemData(int id, ProcessItemType itemType, Source source = Source.Bag, int sourceID1 = 0, int sourceID2 = 0, WandSlotType spellSource = WandSlotType.Normal, SlotData slotData = null)
	{
		this.id = id;
		this.itemType = itemType;
		this.source = source;
		this.spellSource = spellSource;
		this.slotData = slotData;
		SourceID1 = sourceID1;
		SourceID2 = sourceID2;
	}

	public void DestroyItem()
	{
		Debug.Log(SourceID2);
		switch (itemType)
		{
		case ProcessItemType.Spell:
			switch (source)
			{
			case Source.Bag:
				PlayerMgr.Inst.Slot_RemoveBagSlot(SourceID2);
				break;
			case Source.Wand:
				PlayerMgr.Inst.Slot_RemoveWandSlot(SourceID1, spellSource, SourceID2);
				break;
			}
			break;
		case ProcessItemType.Relic:
			PlayerMgr.Inst.ItemCtrller.RelicRemove(id, 1);
			break;
		case ProcessItemType.Potion:
		{
			for (int i = 0; i < DataMgr.selectedWorldData.battleData9.potionIDs.Count; i++)
			{
				if (DataMgr.selectedWorldData.battleData9.potionIDs[i] == id)
				{
					DataMgr.selectedWorldData.battleData9.potionIDs[i] = 0;
				}
			}
			break;
		}
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.allItemsData.Remove(this);
	}

	public Sprite GetImage()
	{
		return itemType switch
		{
			ProcessItemType.Spell => ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath()), 
			ProcessItemType.Relic => ABResources.LoadAsset<Sprite>(RelicConfig.dic[id].GetIconPath()), 
			ProcessItemType.Potion => ABResources.LoadAsset<Sprite>(PotionConfig.dic[id].GetIconPath()), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public bool SameItem(ProcessItemData other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (id == other.id)
		{
			return itemType == other.itemType;
		}
		return false;
	}

	public int GetCoin()
	{
		return itemType switch
		{
			ProcessItemType.Spell => SpellConfig.dic[id].priceCoin, 
			ProcessItemType.Relic => RelicConfig.dic[id].priceCoin, 
			ProcessItemType.Potion => PotionConfig.dic[id].priceCoin, 
			_ => 0, 
		};
	}
}
