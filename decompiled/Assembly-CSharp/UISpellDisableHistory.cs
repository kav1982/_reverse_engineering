using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISpellDisableHistory : GameUI
{
	public ScrollRect scrollRect;

	public RectTransform window;

	public Image windowBackground;

	public RectTransform content;

	public List<UISpellDisableHistoryListItem> itemList;

	public int gamepadSelectedIndex;

	public (int crystal, int blood) previewCost;

	public UIMobileReturnAndRess mobileReturnAndRess;

	public static UISpellDisableHistory Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}

	protected override void OnShow(object obj = null)
	{
		base.gameObject.SetActive(value: true);
		SEMgr.Inst.uiOpen.PlaySE();
		windowBackground.color = new Color(0f, 0f, 0f, 0f);
		windowBackground.DOFade(0.5f, 0.25f).SetUpdate(isIndependentUpdate: true);
		window.anchoredPosition = new Vector2(0f, 1000f);
		window.DOLocalMoveY(0f, 0.25f).SetUpdate(isIndependentUpdate: true).SetEase(Ease.OutBack);
		if ((bool)mobileReturnAndRess && GameMgr.IsMobile_Static)
		{
			mobileReturnAndRess.Show(this, overrideButtonListener: true);
		}
		previewCost = GetCurrentDisableCost();
		PlayerMgr.Inst.ChangeAncientBlood(previewCost.blood);
		PlayerMgr.Inst.ChangeMagicCrystal(previewCost.crystal);
	}

	protected override void OnHide()
	{
		if ((bool)mobileReturnAndRess && GameMgr.IsMobile_Static)
		{
			mobileReturnAndRess.Hide();
		}
		DOTween.Sequence(this).Append(windowBackground.DOFade(0f, 0.25f)).Join(window.DOLocalMoveY(1000f, 0.25f).SetEase(Ease.InBack))
			.SetUpdate(isIndependentUpdate: true)
			.AppendCallback(delegate
			{
				base.gameObject.SetActive(value: false);
			});
		ClearPreviewCost();
	}

	private static (int crystal, int blood) GetCurrentDisableCost()
	{
		int num = 0;
		int num2 = 0;
		for (int num3 = GameUISingletonMono<UISpellDisable>.Inst.costTypes.Count - 1; num3 >= 0; num3--)
		{
			if (num3 >= GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount)
			{
				if (GameUISingletonMono<UISpellDisable>.Inst.costTypes[num3] == UISpellDisable.CostType.Blood)
				{
					num2 += (num3 - GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount + 1) * GameUISingletonMono<UISpellDisable>.Inst.disableCostBloodPerCount;
				}
				else
				{
					num += (num3 - GameUISingletonMono<UISpellDisable>.Inst.finalFreeDisableCount + 1) * GameUISingletonMono<UISpellDisable>.Inst.disableCostCrystalPerCount;
				}
			}
		}
		return (num, num2);
	}

	public void ClearPreviewCost()
	{
		PlayerMgr.Inst.ChangeAncientBlood(-previewCost.blood);
		PlayerMgr.Inst.ChangeMagicCrystal(-previewCost.crystal);
		previewCost = (0, 0);
	}

	private void OnEnable()
	{
		content.DestroyAllChild();
		itemList.Clear();
		GameObject original = ABResources.LoadPlatformAsset<GameObject>("Prefabs/UI/UISpellDisableHistoryListItem", "Prefabs/UI/UISpellDisableHistoryListItem" + " Variant Mobile");
		for (int num = DataMgr.selectedWorldData.spellDisableHistory2.Count - 1; num >= 0; num--)
		{
			List<int> spells = DataMgr.selectedWorldData.spellDisableHistory2[num];
			GameObject obj = Object.Instantiate(original, content);
			obj.gameObject.SetActive(value: true);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.one;
			UISpellDisableHistoryListItem component = obj.GetComponent<UISpellDisableHistoryListItem>();
			itemList.Add(component);
			component.Initialize(spells);
		}
	}

	public static void SaveDisableHistory()
	{
		List<int> spellDisableCurrentBattle = DataMgr.selectedWorldData.battleData9.spellDisableCurrentBattle;
		if (spellDisableCurrentBattle.Count == 0)
		{
			return;
		}
		for (int i = 0; i < DataMgr.selectedWorldData.spellDisableHistory2.Count; i++)
		{
			if (GeneralTool.ListContentEquals(DataMgr.selectedWorldData.spellDisableHistory2[i], spellDisableCurrentBattle))
			{
				DataMgr.selectedWorldData.spellDisableHistory2.RemoveAt(i);
				i--;
			}
		}
		while (DataMgr.selectedWorldData.spellDisableHistory2.Count > 20)
		{
			DataMgr.selectedWorldData.spellDisableHistory2.RemoveAt(0);
		}
		DataMgr.selectedWorldData.spellDisableHistory2.Add(spellDisableCurrentBattle);
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
