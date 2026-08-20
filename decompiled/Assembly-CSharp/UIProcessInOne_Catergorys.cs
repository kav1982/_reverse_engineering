using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIProcessInOne_Catergorys : MonoBehaviour
{
	public enum SlotType
	{
		All,
		Spell,
		Bag,
		Wand,
		Relic,
		Potion
	}

	public List<UIProcessInOne_CatergorySlot> catergories = new List<UIProcessInOne_CatergorySlot>();

	public Transform containRoot;

	public UIProcessInOne_CatergorySlot catergorySlotPrefab;

	public UIProcessInOne_CatergorySlot selectedCatergorySlot;

	public void CreatCatergorys()
	{
		containRoot.DestroyAllChildImmediate();
		catergories.Clear();
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOne_Controller.UIProcessInOneType.Compound:
			CreateAllSpellAndCategory();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
			CreateAllSpellAndCategory();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
			CreateAllSpellAndCategory();
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
			CreatACatergorySlot(SlotType.Relic);
			break;
		case UIProcessInOne_Controller.UIProcessInOneType.Sell:
			CreatACatergorySlot(SlotType.All);
			CreatACatergorySlot(SlotType.Spell);
			CreatACatergorySlot(SlotType.Relic);
			CreatACatergorySlot(SlotType.Potion);
			break;
		}
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.InitAllItem();
		catergories[0].Select();
	}

	public void CreatACatergorySlot(SlotType slotType, int id = 0)
	{
		catergories.Add(Object.Instantiate(catergorySlotPrefab, containRoot));
		List<UIProcessInOne_CatergorySlot> list = catergories;
		list[list.Count - 1].Init(GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType, slotType, id);
	}

	private void CreateAllSpellAndCategory()
	{
		CreatACatergorySlot(SlotType.All);
		CreatACatergorySlot(SlotType.Bag);
		(from wandConfig in DataMgr.selectedWorldData.battleData9.wandCfgs.Select((WandConfig x, int index) => new { x, index })
			where wandConfig.x != null
			select wandConfig).ToList().ForEach(x =>
		{
			CreatACatergorySlot(SlotType.Wand, x.index);
		});
	}

	public void UpdateCategoryShow()
	{
		catergories.ForEach(delegate(UIProcessInOne_CatergorySlot x)
		{
			x.selectedHighlight.SetActive(x == selectedCatergorySlot);
		});
	}
}
