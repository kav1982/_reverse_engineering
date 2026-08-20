using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpellDisableHistoryListItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public RectTransform iconGrid;

	public GameObject freeDisableUI;

	public GameObject costDisableUI;

	public Text crystalCostText;

	public Text bloodCostText;

	public GameObject gamepadSelectedFrame;

	private List<int> disableSpells;

	public void Initialize(List<int> spells)
	{
		disableSpells = spells;
		iconGrid.DestroyAllChild();
		foreach (int spell in spells)
		{
			GameObject obj = new GameObject(spell.ToString());
			obj.AddComponent<Image>().sprite = ABResources.LoadAsset<Sprite>("Textures/SpellIcons/" + spell / 10);
			obj.transform.SetParent(iconGrid);
			obj.transform.localScale = Vector3.one;
		}
		int num = CalculateCrystalCost(spells);
		freeDisableUI.SetActive(num == 0);
		costDisableUI.SetActive(num > 0);
		if (num > 0)
		{
			crystalCostText.text = num.ToString();
			float num2 = (float)GameUISingletonMono<UISpellDisable>.Inst.disableCostBloodPerCount / (float)GameUISingletonMono<UISpellDisable>.Inst.disableCostCrystalPerCount;
			bloodCostText.text = Mathf.CeilToInt((float)num * num2).ToString();
		}
	}

	public static int CalculateCrystalCost(List<int> disableSpells)
	{
		if (disableSpells.Count <= GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount)
		{
			return 0;
		}
		int num = disableSpells.Count - GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount;
		int num2 = 0;
		for (int i = 1; i <= num; i++)
		{
			num2 += i * GameUISingletonMono<UISpellDisable>.Inst.disableCostCrystalPerCount;
		}
		return num2;
	}

	public void Click()
	{
		if (freeDisableUI.activeSelf)
		{
			UISpellDisableHistory.Inst.ClearPreviewCost();
			foreach (UISpellDisableSlot item in GameUISingletonMono<UISpellDisable>.Inst.disableSlots.Where((UISpellDisableSlot e) => e.image_Disable.gameObject.activeSelf))
			{
				GameUISingletonMono<UISpellDisable>.Inst.SlotClick(item, playSE: false);
			}
			foreach (UISpellDisableSlot item2 in GameUISingletonMono<UISpellDisable>.Inst.disableSlots.Where((UISpellDisableSlot e) => disableSpells.Contains(e.Level1ID)))
			{
				GameUISingletonMono<UISpellDisable>.Inst.SlotClick(item2, playSE: false);
			}
			SEMgr.Inst.uiSpellDisable_Succeed.PlaySE();
			UISpellDisableHistory.Inst.Hide();
		}
		else
		{
			GameUISingletonMono<UISpellDisable>.Inst.applyWindow.Show(disableSpells);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SEMgr.Inst.uiButtonSwitch.PlaySE();
	}
}
