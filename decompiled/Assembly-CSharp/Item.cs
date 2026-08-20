using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Item : IComponentData, IQueryTypeParameter
{
	public float storeBaseOffsetY;

	public float priceFactor;

	public UnityObjectRef<ItemMono> itemMono;

	public bool isInitialized;

	public ItemInfo info;

	public Vector2Int belongRoomMapPos;

	public bool isStore;

	public bool isEndless;

	public bool onPick;

	public bool isPickPlaySE;

	public bool onUpdatePrice;

	public bool onRefresh;

	public int curseID;

	public bool onChapter3Reposition;

	public float3 repositionValue;

	public float curse_DisappearDurationTimer;

	public float curse_DisappearTwinkleTimer;

	public float stuckCheckIntervalTimer;

	public bool isStuck;

	public bool isCurseStealthyTarget;

	public int GetPrice(bool considerDiscount)
	{
		int num = 0;
		switch (info.type)
		{
		case ItemType.Wand:
			num = WandConfig.dic[info.id].GetPrice();
			break;
		case ItemType.Spell:
			num = SpellConfig.dic[info.id].priceCoin;
			break;
		case ItemType.Relic:
			num = ((RelicConfig.dic[info.id].dropType != ItemDropType.Epic) ? RelicConfig.dic[info.id].priceCoin : RelicConfig.dic[info.id].priceHP);
			break;
		case ItemType.Potion:
			num = PotionConfig.dic[info.id].priceCoin;
			break;
		case ItemType.Resource:
		case ItemType.RuneWizardRune:
		case ItemType.MaxHp:
			num = ResourceConfig.dic[info.id].priceCoin;
			break;
		default:
			Debug.LogError(info.type);
			break;
		}
		if (isEndless)
		{
			if (info.type != ItemType.Spell)
			{
				num = ((info.type != ItemType.Relic || RelicConfig.dic[info.id].dropType != ItemDropType.Epic) ? Mathf.FloorToInt((float)num * ((float)Mathf.Min(BattleMgr.Inst.CurrentLevel - 1, 30) + 10f) / 10f * 1.3f) : Mathf.FloorToInt((float)num * 1.3f));
			}
			else
			{
				float p = info.id % 10 - 1;
				float num2 = Mathf.Pow(1.5f, p);
				num = Mathf.FloorToInt((float)num * ((float)Mathf.Min(BattleMgr.Inst.CurrentLevel - 1, 30) + 10f) / 10f * num2 * 1.3f);
			}
		}
		if (considerDiscount)
		{
			num = Mathf.FloorToInt((float)num * priceFactor);
		}
		return num;
	}

	public int GetFinalPrice()
	{
		return GetPrice(considerDiscount: true);
	}

	public bool IsAffordable()
	{
		if (info.type == ItemType.Relic)
		{
			if (RelicConfig.dic[info.id].dropType == ItemDropType.Epic)
			{
				if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
				{
					if (playerPpt.unitCfg.maxHP > (float)GetFinalPrice())
					{
						return true;
					}
					return false;
				}
				return false;
			}
			if (GameMgr.InEndlessMode)
			{
				if (PlayerMgr.Inst.BaData.coinCount >= GetFinalPrice())
				{
					return true;
				}
				return false;
			}
			return true;
		}
		if (PlayerMgr.Inst.BaData.coinCount >= GetFinalPrice())
		{
			return true;
		}
		return false;
	}

	public void SetCurse(int id)
	{
		curseID = id;
		if (DataMgr.selectedWorldData != null)
		{
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Curse, id);
		}
	}

	public void SetPriceFactor(float priceFactor)
	{
		if (isStore && this.priceFactor > priceFactor)
		{
			this.priceFactor = priceFactor;
			onUpdatePrice = true;
		}
	}

	public void BackPool()
	{
		switch (info.type)
		{
		case ItemType.Wand:
			PlayerMgr.Inst.BaData.BackWandToPool(info.id);
			break;
		case ItemType.Relic:
			PlayerMgr.Inst.BaData.BackRelicToPool(info.id, 1);
			break;
		default:
			Debug.LogError(info.type);
			break;
		case ItemType.Spell:
		case ItemType.Potion:
		case ItemType.Resource:
		case ItemType.MaxHp:
			break;
		}
		if (curseID != 0)
		{
			PlayerMgr.Inst.BaData.BackCurseToPool(curseID, 1);
		}
	}

	public void Pickup(bool playSE = true)
	{
		onPick = true;
		isPickPlaySE = playSE;
	}
}
