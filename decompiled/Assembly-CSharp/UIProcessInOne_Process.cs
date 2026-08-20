using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIProcessInOne_Process : MonoBehaviour
{
	[Serializable]
	public class ProcessGeneral
	{
		public UIProcessInOne_Controller.UIProcessInOneType type;

		public GameObject rootObj;

		public Transform rootContainer;

		public Button processButton;
	}

	public const int compoundMateriallv1 = 40201;

	public const int compoundMateriallv2 = 40202;

	public UIProcessInOne_Item ProcessSelectedPrefab;

	public List<UIProcessInOne_Item> processSelectedSlots = new List<UIProcessInOne_Item>();

	public bool Processing;

	public List<ProcessGeneral> generals = new List<ProcessGeneral>();

	public EntityManager entityManager;

	[Header("Reroll")]
	public Text text_Cost;

	public Text currentSelectedItemNum;

	public Text rerollTimeLeft;

	private bool canAfford;

	[Header("Compound")]
	public Text compoundMaterialUse;

	[Header("Sell")]
	[Header("MoreInOne")]
	[Header("RerollRelic")]
	public Text sellTimeLeft;

	public Text totallCost;

	public Entity currentEntity => GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentEntity;

	public ProcessGeneral selectedProcessor => generals.First((ProcessGeneral x) => x.type == GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType);

	public Button currentProcessButton => selectedProcessor.processButton;

	public void Init()
	{
		generals.ForEach(delegate(ProcessGeneral x)
		{
			x.rootObj.SetActive(value: false);
		});
		selectedProcessor.rootObj.SetActive(value: true);
		OnProcessItemChange();
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void Update()
	{
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
		{
			SpecialObj101Reroll_Dots chunkComponentData2 = entityManager.GetChunkComponentData<SpecialObj101Reroll_Dots>(currentEntity);
			currentProcessButton.interactable = !Processing && canAfford && processSelectedSlots.Count > 0 && !chunkComponentData2.isBroken;
			rerollTimeLeft.text = ((!(currentEntity != Entity.Null)) ? "状态良好" : (chunkComponentData2.isBroken ? "已经损坏" : ((chunkComponentData2.useTimer <= chunkComponentData2.fixedUsage) ? "状态良好" : "即将损毁")));
			break;
		}
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
		{
			SpecialObj101MoreInOne_Dots chunkComponentData3 = entityManager.GetChunkComponentData<SpecialObj101MoreInOne_Dots>(currentEntity);
			currentProcessButton.interactable = !chunkComponentData3.isUse && GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Count == GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.numLimit;
			break;
		}
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
			currentProcessButton.interactable = GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Count == GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.numLimit && GameUISingletonMono<UIProcessInOne_Controller>.Inst.rerollReolcCounter == 0;
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			if (currentEntity != Entity.Null)
			{
				SpecialObj21_Dots chunkComponentData = entityManager.GetChunkComponentData<SpecialObj21_Dots>(currentEntity);
				currentProcessButton.interactable = !Processing && processSelectedSlots.Count > 0 && !chunkComponentData.onBroken;
				if (!chunkComponentData.onBroken)
				{
					sellTimeLeft.text = ((chunkComponentData.useTimer <= chunkComponentData.fixedUsage) ? "状态良好" : "即将损毁");
				}
				else
				{
					sellTimeLeft.text = "已经损坏";
				}
			}
			else
			{
				currentProcessButton.interactable = !Processing && processSelectedSlots.Count > 0;
				sellTimeLeft.text = "状态良好";
			}
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			break;
		}
	}

	public void UpdateRerollCost()
	{
		int _cost = 0;
		processSelectedSlots.ForEach(delegate(UIProcessInOne_Item x)
		{
			_cost += GetSpellRerollCost(x.itemData);
		});
		text_Cost.text = _cost.ToString();
		text_Cost.color = ((PlayerMgr.Inst.CoinCount >= _cost) ? Color.green : Color.red);
		canAfford = PlayerMgr.Inst.CoinCount >= _cost;
	}

	private IEnumerator ieRerollSpell()
	{
		List<ProcessItemData> selectedItems = GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem;
		Processing = true;
		int _loopTimer = 0;
		List<int> _levels = selectedItems.Select((ProcessItemData x) => (SpellConfig.dic[x.id].abilityType != SpellAbilityType.DeathAdder) ? 1 : SpellConfig.dic[x.id].level).ToList();
		SpecialObj101Reroll_Dots rerollDotsData = entityManager.GetChunkComponentData<SpecialObj101Reroll_Dots>(currentEntity);
		for (int index = 0; index < selectedItems.Count; index++)
		{
			ProcessItemData processItemData = selectedItems[index];
			int id = processItemData.id;
			if (currentEntity != Entity.Null && rerollDotsData.UseOnce())
			{
				Processing = false;
				yield break;
			}
			PlayerMgr.Inst.ChangeCoin(-GetSpellRerollCost(processItemData));
			processSelectedSlots[index].transform.DOShakePosition(0.3f, 6f, 20).SetUpdate(isIndependentUpdate: true);
			do
			{
				_loopTimer++;
				if (_loopTimer > 100)
				{
					Debug.LogError("!");
					selectedItems[index].id = 10011;
					break;
				}
				selectedItems[index].id = PlayerMgr.Inst.BaData.GetSpellFromPool(_levels[index], SpellConfig.dic[processItemData.id].dropType);
			}
			while (id / 10 == processItemData.id / 10);
			yield return new WaitForSecondsRealtime(0.2f);
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
			yield return new WaitForSecondsRealtime(0.2f);
			bool flag = false;
			SlotData slotData = new SlotData(selectedItems[index].id);
			switch (selectedItems[index].source)
			{
			case ProcessItemData.Source.Bag:
				PlayerMgr.Inst.Slot_RemoveBagSlot(selectedItems[index].SourceID2);
				if (PlayerMgr.Inst.CanBagSpellChange(selectedItems[index].SourceID2, slotData))
				{
					PlayerMgr.Inst.BagSpellChange(selectedItems[index].SourceID2, slotData);
				}
				else
				{
					flag = true;
				}
				break;
			case ProcessItemData.Source.Wand:
				PlayerMgr.Inst.ChangeWandSpell(selectedItems[index].SourceID1, selectedItems[index].spellSource, selectedItems[index].SourceID2, slotData);
				break;
			}
			if (flag)
			{
				Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * 0.02f);
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(slotData), navMeshPointIngoreZ);
			}
		}
		Processing = false;
	}

	private IEnumerator ieSell()
	{
		SpecialObj21_Dots sellDotsData = entityManager.GetChunkComponentData<SpecialObj21_Dots>(currentEntity);
		while (processSelectedSlots.Count > 0)
		{
			Processing = true;
			ProcessItemData currentSell = processSelectedSlots[0].itemData;
			if (currentEntity != Entity.Null && sellDotsData.UseOnce())
			{
				Processing = false;
				yield break;
			}
			entityManager.SetComponentData(currentEntity, sellDotsData);
			PlayerMgr.Inst.ChangeCoin(currentSell.GetCoin());
			processSelectedSlots[0].transform.DOShakePosition(0.3f, 6f, 20).SetUpdate(isIndependentUpdate: true);
			yield return new WaitForSecondsRealtime(0.2f);
			currentSell.DestroyItem();
			RemoveItem(processSelectedSlots[0]);
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
			yield return new WaitForSecondsRealtime(0.2f);
		}
		Processing = false;
	}

	public int GetSpellRerollCost(ProcessItemData item)
	{
		SpellConfig spellConfig = SpellConfig.dic[item.id];
		switch (spellConfig.dropType)
		{
		case ItemDropType.None:
			return 99999999;
		case ItemDropType.Common:
			if (spellConfig.level == 1)
			{
				return 2;
			}
			if (spellConfig.level == 2)
			{
				return 3;
			}
			if (spellConfig.level == 3)
			{
				return 4;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Rare:
			if (spellConfig.level == 1)
			{
				return 3;
			}
			if (spellConfig.level == 2)
			{
				return 4;
			}
			if (spellConfig.level == 3)
			{
				return 5;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Epic:
			if (spellConfig.level == 1)
			{
				return 15;
			}
			if (spellConfig.level == 2)
			{
				return 15;
			}
			if (spellConfig.level == 3)
			{
				return 15;
			}
			Debug.LogError(spellConfig.level);
			return 99999999;
		case ItemDropType.Special:
			return 99999999;
		default:
			Debug.LogError(spellConfig.dropType);
			return 99999999;
		}
	}

	public void Reroll()
	{
		if (!Processing)
		{
			StartCoroutine(ieRerollSpell());
		}
	}

	public void Compound()
	{
		ProcessItemData processItemData = GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.FirstOrDefault((ProcessItemData x) => x.source == ProcessItemData.Source.Wand);
		ProcessItemData processItemData2 = GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem[0];
		if (processItemData != null)
		{
			PlayerMgr.Inst.ChangeWandSpell(processItemData.SourceID1, processItemData.spellSource, processItemData.SourceID2, new SlotData(processItemData.id + 1));
		}
		else
		{
			PlayerMgr.Inst.BagSpellChange(processItemData2.SourceID2, new SlotData(processItemData2.id + 1));
		}
		ProcessItemData finalChangeItem = processItemData ?? processItemData2;
		finalChangeItem.id++;
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Where((ProcessItemData x) => x != finalChangeItem).ToList().ForEach(delegate(ProcessItemData x)
		{
			Debug.Log(x.source);
			x.DestroyItem();
		});
		while (processSelectedSlots.Count > 0)
		{
			RemoveItem(processSelectedSlots[0]);
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}

	public void MoreInOne()
	{
		SpecialObj101MoreInOne_Dots chunkComponentData = entityManager.GetChunkComponentData<SpecialObj101MoreInOne_Dots>(currentEntity);
		UIProcessInOne_Item random = processSelectedSlots.GetRandom();
		random.itemData.id++;
		for (int i = 0; i < processSelectedSlots.Count; i++)
		{
			UIProcessInOne_Item uIProcessInOne_Item = processSelectedSlots[i];
			if (!(uIProcessInOne_Item == random))
			{
				uIProcessInOne_Item.itemData.DestroyItem();
				RemoveItem(uIProcessInOne_Item);
				i--;
			}
		}
		chunkComponentData.isUse = true;
		entityManager.SetComponentData(currentEntity, chunkComponentData);
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.UpdateShowAll();
	}

	public void RerollRelic()
	{
		int _oldID = processSelectedSlots[0].itemData.id;
		RelicConfig relicConfig = DataMgr.selectedWorldData.battleData9.relicCfgs.FirstOrDefault((RelicConfig x) => x.id == _oldID);
		int num = 0;
		int num2 = 0;
		while (true)
		{
			num2++;
			if (num2 >= 100)
			{
				Debug.LogError("超过100次");
				num = 999;
				break;
			}
			num = PlayerMgr.Inst.BaData.GetRelicFromPool(relicConfig.dropType);
			if (num != relicConfig.id && num != 40 && num != 69)
			{
				break;
			}
			PlayerMgr.Inst.BaData.BackRelicToPool(num, 1);
		}
		while (processSelectedSlots.Count > 0)
		{
			processSelectedSlots[0].itemData.DestroyItem();
			RemoveItem(processSelectedSlots[0]);
		}
		PlayerMgr.Inst.ItemCtrller.RelicAdd(num);
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.rerollReolcCounter++;
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.catergorys.CreatCatergorys();
	}

	public void Sell()
	{
		if (!Processing)
		{
			StartCoroutine(ieSell());
		}
	}

	public void DisSelectAll()
	{
		Debug.Log("DisSelectAll");
		processSelectedSlots = new List<UIProcessInOne_Item>();
		selectedProcessor.rootContainer.DestroyAllChildImmediate();
	}

	public bool CanAddSlot(UIProcessInOne_Item uiProcessInOneItem)
	{
		if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.numLimit.HasValue && GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Count >= GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.numLimit)
		{
			return false;
		}
		if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.idOnly != null && !GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.idOnly.Contains(uiProcessInOneItem.itemData.id))
		{
			return false;
		}
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.canCompoundItems.Contains(uiProcessInOneItem.itemData))
			{
				return SpellConfig.dic[uiProcessInOneItem.itemData.id].canCompound;
			}
			return false;
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
			if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Count == 0)
			{
				return SpellConfig.dic[uiProcessInOneItem.itemData.id].canCompound;
			}
			if (SpellConfig.dic[uiProcessInOneItem.itemData.id].level == SpellConfig.dic[GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem[0].id].level)
			{
				return SpellConfig.dic[uiProcessInOneItem.itemData.id].dropType == SpellConfig.dic[GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem[0].id].dropType;
			}
			return false;
		default:
			throw new ArgumentOutOfRangeException();
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			return true;
		}
	}

	public void UpdateSelectedItems()
	{
		processSelectedSlots.ForEach(delegate(UIProcessInOne_Item x)
		{
			x.UpdateShow();
		});
		OnProcessItemChange();
	}

	public void AddSelectedItem(ProcessItemData itemData)
	{
		itemData.selected = true;
		UIProcessInOne_Item uIProcessInOne_Item = UnityEngine.Object.Instantiate(ProcessSelectedPrefab, selectedProcessor.rootContainer);
		uIProcessInOne_Item.Init(itemData, UIProcessInOne_Item.ProcessItemSlotType.ProcessSlot);
		uIProcessInOne_Item.UpdateShow();
		processSelectedSlots.Add(uIProcessInOne_Item);
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			if (itemData.id != 40201 && itemData.id != 40202)
			{
				switch (SpellConfig.dic[itemData.id].level)
				{
				case 1:
					GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.idOnly = new int?[2] { itemData.id, 40201 };
					break;
				case 2:
					GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.idOnly = new int?[2] { itemData.id, 40202 };
					break;
				}
			}
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
			UpdateRerollCost();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			break;
		}
		OnProcessItemChange();
	}

	public void RemoveItem(UIProcessInOne_Item itemSlot)
	{
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			if (GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.processSelectedSlots.Count == 1)
			{
				GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.idOnly = null;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			break;
		}
		itemSlot.itemData.selected = false;
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.processSelectedSlots.Remove(itemSlot);
		UnityEngine.Object.Destroy(itemSlot.gameObject);
		OnProcessItemChange();
	}

	public void OnProcessItemChange()
	{
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			currentProcessButton.interactable = GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.Count == GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.numLimit;
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
			if (processSelectedSlots.Count > 0)
			{
				currentSelectedItemNum.gameObject.SetActive(value: true);
				currentSelectedItemNum.text = "已拥有数量:" + GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.allItemsData.Count((ProcessItemData x) => x.SameItem(processSelectedSlots[0].itemData));
			}
			else
			{
				currentSelectedItemNum.gameObject.SetActive(value: false);
			}
			UpdateRerollCost();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
		{
			int costSum = 0;
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.selectedItem.ForEach(delegate(ProcessItemData x)
			{
				costSum += x.GetCoin();
			});
			totallCost.text = costSum.ToString();
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
			break;
		}
	}
}
