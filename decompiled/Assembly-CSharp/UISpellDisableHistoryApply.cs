using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISpellDisableHistoryApply : GameUI
{
	public RectTransform window;

	public Image windowBackground;

	public Text bloodCostText;

	public Text crystalCostText;

	public Slider slider;

	public Button applyButton;

	public Text applyButtonText;

	private List<int> disableSpells;

	private int totalCrystalCost;

	public static UISpellDisableHistoryApply Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}

	protected override void OnShow(object obj = null)
	{
		if (obj is List<int> spells)
		{
			OnShow(spells);
		}
	}

	private void OnShow(List<int> spells)
	{
		disableSpells = spells;
		totalCrystalCost = UISpellDisableHistoryListItem.CalculateCrystalCost(spells);
		slider.maxValue = spells.Count - GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount;
		slider.value = Mathf.FloorToInt(slider.maxValue / 2f);
		UpdateUI();
		base.gameObject.SetActive(value: true);
		windowBackground.color = new Color(0f, 0f, 0f, 0f);
		windowBackground.DOFade(0.5f, 0.25f).SetUpdate(isIndependentUpdate: true);
		window.anchoredPosition = new Vector2(0f, 1000f);
		window.DOLocalMoveY(0f, 0.25f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack);
		LayoutRebuilder.ForceRebuildLayoutImmediate(applyButton.GetComponent<RectTransform>());
	}

	private (int blood, int crystal) GetCost()
	{
		int num = 0;
		for (int i = 1; i <= disableSpells.Count - GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount && (float)i <= slider.value; i++)
		{
			int num2 = i * GameUISingletonMono<UISpellDisable>.Inst.disableCostBloodPerCount;
			num += num2;
		}
		int item = Mathf.RoundToInt((float)totalCrystalCost - (float)num / GameUISingletonMono<UISpellDisable>.Inst.crystalCostToBloodCostRatio);
		return (num, item);
	}

	public void UpdateUI()
	{
		(int, int) cost = GetCost();
		crystalCostText.text = cost.Item2.ToString();
		bloodCostText.text = cost.Item1.ToString();
		crystalCostText.color = ((cost.Item2 <= DataMgr.selectedWorldData.magicCrystalCount) ? Color.white : Color.red);
		bloodCostText.color = ((cost.Item1 <= DataMgr.selectedWorldData.ancientBloodCount) ? Color.white : Color.red);
		applyButton.interactable = DataMgr.selectedWorldData.ancientBloodCount >= cost.Item1 && DataMgr.selectedWorldData.magicCrystalCount >= cost.Item2;
		applyButtonText.text = (applyButton.interactable ? 1003510 : 1003511).GetText();
	}

	protected override void OnHide()
	{
		DOTween.Sequence(this).Append(windowBackground.DOFade(0f, 0.25f)).Join(window.DOLocalMoveY(1000f, 0.25f).SetEase(Ease.InBack))
			.SetUpdate(isIndependentUpdate: true)
			.AppendCallback(delegate
			{
				base.gameObject.SetActive(value: false);
			});
	}

	public void ApplyButtonClick()
	{
		(int, int) cost = GetCost();
		if (DataMgr.selectedWorldData.ancientBloodCount < cost.Item1 || DataMgr.selectedWorldData.magicCrystalCount < cost.Item2)
		{
			return;
		}
		UISpellDisableHistory.Inst.ClearPreviewCost();
		foreach (UISpellDisableSlot item in GameUISingletonMono<UISpellDisable>.Inst.disableSlots.Where((UISpellDisableSlot e) => e.image_Disable.gameObject.activeSelf))
		{
			GameUISingletonMono<UISpellDisable>.Inst.SlotClick(item, playSE: false);
		}
		int finalFreeDisableCount = GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount;
		for (int i = 0; i < disableSpells.Count; i++)
		{
			int spellId = disableSpells[i];
			UISpellDisableSlot slot = GameUISingletonMono<UISpellDisable>.Inst.disableSlots.First((UISpellDisableSlot e) => e.Level1ID == spellId);
			UISpellDisable.CostType value = UISpellDisable.CostType.Crystal;
			if (i >= finalFreeDisableCount)
			{
				value = (((float)(i - finalFreeDisableCount) < slider.value) ? UISpellDisable.CostType.Blood : UISpellDisable.CostType.Crystal);
			}
			GameUISingletonMono<UISpellDisable>.Inst.SlotClick(slot, playSE: false, value);
		}
		SEMgr.Inst.uiSpellDisable_Succeed.PlaySE();
		Hide();
		UISpellDisableHistory.Inst.Hide();
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}
}
