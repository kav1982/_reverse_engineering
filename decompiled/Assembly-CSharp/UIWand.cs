using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIWand : MonoBehaviour
{
	public UISlotWand pfb_UISlotWand;

	public GameObject pfb_UISpecialSlotBlocker;

	public RectTransform rtsf_Self;

	public RectTransform rtsf_Spells;

	public RectTransform rtsf_SlotsBG;

	public Image image_Icon;

	public CanvasGroup cg;

	public UILayout uiLayout;

	public Text text_Shortcut;

	public UIWandEvent uiWandEvent;

	public float spellBGExtraWidth;

	public float spellBGExtraWidthScaled;

	public float hoverScale;

	[Header("Highlight")]
	public Image image_ShortcutBG;

	public Image image_BG;

	public Image image_BG_Frame_Select;

	public float image_BG_Frame_Select_RotateSpeed = -130f;

	public Sprite sprite_ShortcutSelect;

	public Sprite sprite_ShortcutUnselect;

	public RectTransform buidlBG;

	public GameObject Shortcut;

	public float buidlBGWidthOffset;

	public Image manaPercentImage;

	public GameObject manaAlertImage;

	public GameObject slotManaAlertObj;

	private UISlotWand[] uiSlot_Normals;

	private UISlotWand[] uiSlot_Posts;

	public WandConfig WandCfgFromBuild;

	public CanvasGroup CanvasGroup;

	public int WandIndex { get; private set; }

	public WandConfig WandCfg
	{
		get
		{
			if (PlayerMgr.Inst.BaData.wandCfgs.Count <= WandIndex)
			{
				return null;
			}
			return PlayerMgr.Inst.BaData.wandCfgs[WandIndex];
		}
	}

	public int AllSlotCount => uiSlot_Normals.Length + uiSlot_Posts.Length;

	private void Update()
	{
		image_BG_Frame_Select.transform.Rotate(0f, 0f, Time.deltaTime * image_BG_Frame_Select_RotateSpeed);
	}

	public void Initialize(int index)
	{
		WandIndex = index;
		text_Shortcut.text = (index + 1).ToString();
		manaPercentImage.material = Object.Instantiate(manaPercentImage.material);
		slotManaAlertObj.GetComponent<UIInfoSpell>().SetManaLackAlertInfo();
		UpdateInfo();
	}

	public void UpdateInfo()
	{
		StartCoroutine(UpdateInfoIE());
	}

	private IEnumerator UpdateInfoIE()
	{
		rtsf_Spells.DestroyAllChild();
		UpdateManaPercent(0f);
		if (WandCfg == null)
		{
			uiSlot_Normals = new UISlotWand[0];
			uiSlot_Posts = new UISlotWand[0];
			image_Icon.gameObject.SetActive(value: false);
			manaAlertImage.SetActive(value: false);
			manaPercentImage.enabled = false;
		}
		else
		{
			uiSlot_Normals = new UISlotWand[WandCfg.normalSlots.Length];
			uiSlot_Posts = new UISlotWand[WandCfg.postSlots.Length];
			image_Icon.gameObject.SetActive(value: true);
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(WandCfg.GetIconPath());
			manaPercentImage.enabled = true;
			if (WandCfg.normalSlots.Length != 0)
			{
				for (int i = 0; i < WandCfg.normalSlots.Length; i++)
				{
					uiSlot_Normals[i] = Object.Instantiate(pfb_UISlotWand, rtsf_Spells);
					uiSlot_Normals[i].Initialize(WandIndex, i, WandSlotType.Normal);
				}
			}
			if (WandCfg.postSlots.Length != 0)
			{
				if (WandCfg.normalSlots.Length != 0)
				{
					Object.Instantiate(pfb_UISpecialSlotBlocker, rtsf_Spells);
				}
				for (int j = 0; j < WandCfg.postSlots.Length; j++)
				{
					uiSlot_Posts[j] = Object.Instantiate(pfb_UISlotWand, rtsf_Spells);
					uiSlot_Posts[j].Initialize(WandIndex, j, WandSlotType.Post);
				}
			}
		}
		UISlotWand[] array = uiSlot_Normals;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: false);
		}
		array = uiSlot_Posts;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: false);
		}
		yield return null;
		uiLayout.Layout();
		array = uiSlot_Normals;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: true);
		}
		array = uiSlot_Posts;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: true);
		}
		if ((bool)UIBattleMgr.Inst && UIBattleMgr.Inst.uiFinishBuildShow.IsOpen)
		{
			Open();
		}
		else if ((bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen)
		{
			Open();
		}
		else if (UIPlayerDataMgr.Inst.IsBagOpen)
		{
			Open();
		}
		else
		{
			Close();
		}
		yield return null;
		UIPlayerDataMgr.Inst.UpdateBagUiSizeMobile();
		UpdateWandBG();
	}

	private IEnumerator UpdateInfoIEBuild(FinishGameBuild build, int index)
	{
		rtsf_Spells.DestroyAllChild();
		UpdateManaPercent(0f);
		if (WandCfgFromBuild == null)
		{
			uiSlot_Normals = new UISlotWand[0];
			uiSlot_Posts = new UISlotWand[0];
			image_Icon.gameObject.SetActive(value: false);
			manaAlertImage.SetActive(value: false);
			manaPercentImage.enabled = false;
		}
		else
		{
			buidlBG.gameObject.SetActive(value: true);
			uiSlot_Normals = new UISlotWand[WandCfgFromBuild.normalSlots.Length];
			uiSlot_Posts = new UISlotWand[WandCfgFromBuild.postSlots.Length];
			image_Icon.gameObject.SetActive(value: true);
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(WandCfgFromBuild.GetIconPath());
			manaPercentImage.enabled = true;
			if (WandCfgFromBuild.normalSlots.Length != 0)
			{
				for (int i = 0; i < WandCfgFromBuild.normalSlots.Length; i++)
				{
					uiSlot_Normals[i] = Object.Instantiate(pfb_UISlotWand, rtsf_Spells);
					uiSlot_Normals[i].Initialize(index, i, WandSlotType.Normal, build);
				}
			}
			if (WandCfgFromBuild.postSlots.Length != 0)
			{
				if (WandCfgFromBuild.normalSlots.Length != 0)
				{
					Object.Instantiate(pfb_UISpecialSlotBlocker, rtsf_Spells);
				}
				for (int j = 0; j < WandCfgFromBuild.postSlots.Length; j++)
				{
					uiSlot_Posts[j] = Object.Instantiate(pfb_UISlotWand, rtsf_Spells);
					uiSlot_Posts[j].Initialize(index, j, WandSlotType.Post, build);
				}
			}
		}
		UISlotWand[] array = uiSlot_Normals;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: false);
		}
		array = uiSlot_Posts;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: false);
		}
		yield return null;
		uiLayout.Layout();
		rtsf_SlotsBG.sizeDelta = new Vector2(rtsf_Spells.sizeDelta.x + spellBGExtraWidth + spellBGExtraWidthScaled / rtsf_SlotsBG.localScale.x, rtsf_SlotsBG.sizeDelta.y);
		buidlBG.sizeDelta = new Vector2(rtsf_SlotsBG.sizeDelta.x * rtsf_SlotsBG.localScale.x + buidlBGWidthOffset, buidlBG.sizeDelta.y);
		array = uiSlot_Normals;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: true);
		}
		array = uiSlot_Posts;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].gameObject.SetActive(value: true);
		}
		if ((bool)UIBattleMgr.Inst && UIBattleMgr.Inst.uiFinishBuildShow.IsOpen)
		{
			OpenBuild();
		}
		else if ((bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen)
		{
			OpenBuild();
		}
		else if (((bool)UIBattleMgr.Inst && UIBattleMgr.Inst.uiMenu.IsOpen) || ((bool)UICampMgr.Inst && UICampMgr.Inst.uiMenu.IsOpen) || ((bool)UIGuideMgr.Inst && UIGuideMgr.Inst.uiMenu.IsOpen))
		{
			OpenBuild();
		}
		else if (UIPlayerDataMgr.Inst.IsBagOpen)
		{
			Open();
		}
		else
		{
			Close();
		}
	}

	public void UpdateWandBG()
	{
		rtsf_SlotsBG.sizeDelta = new Vector2(rtsf_Spells.sizeDelta.x + spellBGExtraWidth + spellBGExtraWidthScaled / rtsf_SlotsBG.localScale.x, rtsf_SlotsBG.sizeDelta.y);
		buidlBG.sizeDelta = new Vector2(rtsf_SlotsBG.sizeDelta.x * rtsf_SlotsBG.localScale.x + buidlBGWidthOffset, buidlBG.sizeDelta.y);
	}

	public void InitializeBuild(FinishGameBuild build, int index)
	{
		WandIndex = index;
		text_Shortcut.text = (index + 1).ToString();
		uiWandEvent.GetComponent<UIWandEvent>().isFromBuild = true;
		manaPercentImage.material = Object.Instantiate(manaPercentImage.material);
		WandCfgFromBuild = build.wandCfgs[index];
		slotManaAlertObj.GetComponent<UIInfoSpell>().SetManaLackAlertInfo();
	}

	public void UpdateInfoBuild(FinishGameBuild build, int index)
	{
		if ((bool)Shortcut)
		{
			Shortcut.SetActive(value: false);
		}
		StartCoroutine(UpdateInfoIEBuild(build, index));
	}

	public void Open()
	{
		if (WandCfg == null)
		{
			rtsf_SlotsBG.gameObject.SetActive(value: false);
		}
		else
		{
			rtsf_SlotsBG.gameObject.SetActive(value: true);
		}
		cg.alpha = 1f;
		cg.interactable = true;
		cg.blocksRaycasts = true;
		rtsf_Spells.gameObject.SetActive(value: true);
	}

	public void OpenBuild()
	{
		rtsf_SlotsBG.gameObject.SetActive(value: false);
		cg.alpha = 1f;
		cg.interactable = true;
		cg.blocksRaycasts = true;
	}

	public void Close()
	{
		rtsf_SlotsBG.gameObject.SetActive(value: false);
		cg.alpha = 0f;
		cg.interactable = false;
		cg.blocksRaycasts = false;
		rtsf_Spells.gameObject.SetActive(value: false);
	}

	public void Select()
	{
		image_ShortcutBG.sprite = sprite_ShortcutSelect;
		image_BG_Frame_Select.gameObject.SetActive(value: true);
	}

	public void Unselect()
	{
		image_ShortcutBG.sprite = sprite_ShortcutUnselect;
		image_BG_Frame_Select.gameObject.SetActive(value: false);
	}

	public void Hover()
	{
		image_BG.transform.localScale = Vector3.one * hoverScale;
	}

	public void Unhover()
	{
		image_BG.transform.localScale = Vector3.one;
	}

	public void UpdatePreshoot(int[] slotIndex)
	{
		for (int i = 0; i < uiSlot_Normals.Length; i++)
		{
			if (slotIndex.Contains(i))
			{
				uiSlot_Normals[i].ShowPreshootHint();
			}
			else
			{
				uiSlot_Normals[i].HidePreshootHint();
			}
		}
	}

	public void HideAllPreshoot()
	{
		UISlotWand[] array = uiSlot_Normals;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].HidePreshootHint();
		}
	}

	public void UpdatePostShoot(List<int> postSlotIndexs, float chargePercent)
	{
		if (postSlotIndexs == null || postSlotIndexs.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < uiSlot_Posts.Length; i++)
		{
			if (postSlotIndexs.Contains(i))
			{
				uiSlot_Posts[i].UpdatePostSlotCharge(chargePercent);
			}
		}
	}

	public void UpdateNoMana(bool[] noManaPreSlot, bool noManaAll)
	{
		if (uiSlot_Normals.Length != noManaPreSlot.Length)
		{
			Debug.LogWarning($"更新魔力不足图标的传入参数长度不正确   {noManaPreSlot.Length} != {uiSlot_Normals.Length}");
			return;
		}
		manaAlertImage.SetActive(noManaAll);
		for (int i = 0; i < noManaPreSlot.Length; i++)
		{
			uiSlot_Normals[i].ManaNotEnoughCastSpell(noManaPreSlot[i]);
		}
	}

	public void UpdateUnused(Dictionary<SlotData, Wand.UnusedEnhanceType> unusedSlots)
	{
		UISlotWand[] array = uiSlot_Normals;
		foreach (UISlotWand uISlotWand in array)
		{
			if (unusedSlots.Keys.ToArray().Contains(uISlotWand.SpellDat))
			{
				uISlotWand.ShowUnusedHint(unusedSlots[uISlotWand.SpellDat]);
			}
			else
			{
				uISlotWand.HideUnusedHint();
			}
		}
		array = uiSlot_Posts;
		foreach (UISlotWand uISlotWand2 in array)
		{
			if (unusedSlots.Keys.ToArray().Contains(uISlotWand2.SpellDat))
			{
				uISlotWand2.ShowUnusedHint(unusedSlots[uISlotWand2.SpellDat]);
			}
			else
			{
				uISlotWand2.HideUnusedHint();
			}
		}
	}

	[CanBeNull]
	public UISlotWand GetUISlot(WandSlotType slotType, int slotIndex)
	{
		switch (slotType)
		{
		case WandSlotType.Normal:
			return uiSlot_Normals.GetOrDefault(slotIndex);
		case WandSlotType.Post:
			return uiSlot_Posts.GetOrDefault(slotIndex);
		default:
			Debug.LogError(slotType);
			return null;
		}
	}

	public void UpdateManaPercent(float percent)
	{
		manaPercentImage.material.SetFloat("_Percent", percent * 0.9f);
	}

	public UISlotWand[] GetUIAllUISlot()
	{
		UISlotWand[] array = new UISlotWand[uiSlot_Normals.Length + uiSlot_Posts.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (i < uiSlot_Normals.Length)
			{
				array[i] = uiSlot_Normals[i];
			}
			else
			{
				array[i] = uiSlot_Posts[i - uiSlot_Normals.Length];
			}
		}
		return array;
	}

	public UISlotWand GetUISlotByAllIndex(int slotIndex)
	{
		if (slotIndex < uiSlot_Normals.Length)
		{
			return uiSlot_Normals[slotIndex];
		}
		if (slotIndex < uiSlot_Normals.Length + uiSlot_Posts.Length)
		{
			return uiSlot_Posts[slotIndex - uiSlot_Normals.Length];
		}
		Debug.LogError("下标超过所有slot数量的总和，或该法杖格子数为0");
		return null;
	}

	public int GetGridIndexFromActual(int actualIndex)
	{
		if (actualIndex < uiSlot_Normals.Length)
		{
			return actualIndex;
		}
		if (actualIndex < AllSlotCount)
		{
			if (uiSlot_Normals.Length != 0)
			{
				actualIndex++;
			}
			return actualIndex;
		}
		Debug.LogError("下标超过所有slot数量的总和，或该法杖格子数为0");
		return 0;
	}

	public int GetActualIndexFromGrid(int gridIndex)
	{
		if (WandCfg == null)
		{
			return -1;
		}
		if (gridIndex < uiSlot_Normals.Length)
		{
			return gridIndex;
		}
		if (uiSlot_Normals.Length != 0)
		{
			gridIndex--;
		}
		return gridIndex;
	}

	public void PointoutAllSlots()
	{
		UISlotWand[] array = uiSlot_Normals;
		foreach (UISlotWand uISlotWand in array)
		{
			if (uISlotWand.GetFocusState())
			{
				uISlotWand.OnPointerExit(null);
			}
		}
		array = uiSlot_Posts;
		foreach (UISlotWand uISlotWand2 in array)
		{
			if (uISlotWand2.GetFocusState())
			{
				uISlotWand2.OnPointerExit(null);
			}
		}
	}

	public void SetSpellDrag(bool drag)
	{
		for (int i = 0; i < rtsf_Spells.childCount; i++)
		{
			if ((bool)rtsf_Spells.GetChild(i).GetComponent<UISlotWand>())
			{
				rtsf_Spells.GetChild(i).GetComponent<UISlotWand>().SetDrag(drag);
			}
		}
	}
}
