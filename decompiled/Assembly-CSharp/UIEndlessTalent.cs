using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIEndlessTalent")]
public class UIEndlessTalent : GameUISingletonMono<UIEndlessTalent>
{
	public Animator Anima;

	public Text Text_Title;

	public Text Text_GearAmount;

	public int maxLevelOfMaxHPAndExtraDamage;

	[Header("Script")]
	public UIEndlessTalentSlot uiETS_GoodsExtraCount;

	public UIEndlessTalentSlot uiETS_SupplyBox;

	public UIEndlessTalentSlot uiETS_Gallery;

	public UIEndlessTalentSlot uiETS_FinishCoin;

	public UIEndlessTalentSlot uiETS_LockMachine;

	public UIEndlessTalentSlot uiETS_HightLevelSpell;

	public UIEndlessTalentSlot uiETS_ProcessSpell;

	public UIEndlessTalentSlot uiETS_MaxHP;

	public UIEndlessTalentSlot uiETS_ExtraDamage;

	[Header("IconImage")]
	public Sprite Icon_GoodsExtraCount;

	public Sprite Icon_SupplyBox;

	public Sprite Icon_Gallery;

	public Sprite Icon_FinishCoin;

	public Sprite Icon_LockMachine;

	public Sprite Icon_HightLevelSpell;

	public Sprite Icon_ProcessSpell;

	public Sprite Icon_MaxHP;

	public Sprite Icon_ExtraDamage;

	[Header("UnlockMore")]
	public Text text_UnlockMore;

	public int[] unlockMoreTime;

	public Vector3[] unlockMoreTextPos;

	private static readonly int Disappear = Animator.StringToHash("Disappear");

	private static readonly int Appear = Animator.StringToHash("Appear");

	private WorldData worldData;

	private EndlessTalentUpgrade endlessTalentUpgrade;

	protected override void OnShow(object obj = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		SEMgr.Inst.uiOpen.PlaySE();
		Anima.SetTrigger(Appear);
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		CamController.Inst.MouseOffsetPause();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Gear);
	}

	public override void _Close()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SEMgr.Inst.uiClick.PlaySE();
		}
		Hide();
	}

	protected override void OnHide()
	{
		StopAllCoroutines();
		Anima.SetTrigger(Disappear);
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		CamController.Inst.MouseOffsetContinue();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Gear);
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.GearChange = (Action)Delegate.Combine(EventMgr.GearChange, new Action(GearChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.GearChange = (Action)Delegate.Remove(EventMgr.GearChange, new Action(GearChange));
	}

	private void LanguageChange()
	{
		Text_Title.text = 1000550.GetText();
		Text_GearAmount.text = worldData.GearCount.ToString();
		uiETS_GoodsExtraCount.Text_Name.text = 1000551.GetText();
		uiETS_GoodsExtraCount.Text_Desc.text = 1000552.GetText();
		uiETS_SupplyBox.Text_Name.text = 1000553.GetText();
		uiETS_SupplyBox.Text_Desc.text = 1000554.GetText();
		uiETS_Gallery.Text_Name.text = 1000555.GetText();
		uiETS_Gallery.Text_Desc.text = 1000556.GetText();
		uiETS_FinishCoin.Text_Name.text = 1000557.GetText();
		uiETS_FinishCoin.Text_Desc.text = 1000558.GetText();
		uiETS_LockMachine.Text_Name.text = 1000559.GetText();
		uiETS_LockMachine.Text_Desc.text = 1000560.GetText();
		uiETS_HightLevelSpell.Text_Name.text = 1000561.GetText();
		uiETS_HightLevelSpell.Text_Desc.text = 1000562.GetText();
		uiETS_ProcessSpell.Text_Name.text = 1000563.GetText();
		uiETS_ProcessSpell.Text_Desc.text = 1000564.GetText();
		uiETS_MaxHP.Text_Name.text = 1000565.GetText();
		uiETS_MaxHP.Text_Desc.text = 1000566.GetText();
		uiETS_ExtraDamage.Text_Name.text = 1000567.GetText();
		uiETS_ExtraDamage.Text_Desc.text = 1000568.GetText();
		UpdateAllTalentUI();
		UpdateSlotsUnlockState();
	}

	protected override IEnumerator OnInit()
	{
		worldData = DataMgr.selectedWorldData;
		endlessTalentUpgrade = ScriptableObjMgr.Inst.EndlessTalentUpgrade;
		uiETS_GoodsExtraCount.Icon.sprite = Icon_GoodsExtraCount;
		uiETS_SupplyBox.Icon.sprite = Icon_SupplyBox;
		uiETS_Gallery.Icon.sprite = Icon_Gallery;
		uiETS_FinishCoin.Icon.sprite = Icon_FinishCoin;
		uiETS_LockMachine.Icon.sprite = Icon_LockMachine;
		uiETS_HightLevelSpell.Icon.sprite = Icon_HightLevelSpell;
		uiETS_ProcessSpell.Icon.sprite = Icon_ProcessSpell;
		uiETS_MaxHP.Icon.sprite = Icon_MaxHP;
		uiETS_ExtraDamage.Icon.sprite = Icon_ExtraDamage;
		uiETS_GoodsExtraCount.Button.onClick.AddListener(UpgradeGoodsExtraCount);
		uiETS_SupplyBox.Button.onClick.AddListener(UpgradeSupplyBox);
		uiETS_Gallery.Button.onClick.AddListener(UpgradeGallery);
		uiETS_FinishCoin.Button.onClick.AddListener(UpgradeFinishCoin);
		uiETS_LockMachine.Button.onClick.AddListener(UpgradeLockMachine);
		uiETS_HightLevelSpell.Button.onClick.AddListener(UpgradeHightLevelSpell);
		uiETS_ProcessSpell.Button.onClick.AddListener(UpgradeProcessSpell);
		uiETS_MaxHP.Button.onClick.AddListener(UpgradeMaxHP);
		uiETS_ExtraDamage.Button.onClick.AddListener(UpgradeExtraDamage);
		NormalizeLevel(ref worldData.endless_LevelOfGoodsExtraCount, endlessTalentUpgrade.goodsExtraCount);
		NormalizeLevel(ref worldData.endless_LevelOfSupplyBox, endlessTalentUpgrade.supplyBox);
		NormalizeLevel(ref worldData.endless_LevelOfGallery, endlessTalentUpgrade.gallery);
		NormalizeLevel(ref worldData.endless_LevelOfFinishCoin, endlessTalentUpgrade.finishCoin);
		NormalizeLevel(ref worldData.endless_LevelOfLcokMachine, endlessTalentUpgrade.lockMachine);
		NormalizeLevel(ref worldData.endless_LevelOfHightLevelSpell, endlessTalentUpgrade.hightLevelSpell);
		NormalizeLevel(ref worldData.endless_LevelOfProcessSpell, endlessTalentUpgrade.processSpell);
		LanguageChange();
		yield return null;
	}

	private void GearChange()
	{
		Text_GearAmount.text = worldData.GearCount.ToString();
		UpdateAllTalentUI();
	}

	private void UpdateAllTalentUI()
	{
		UpdateGoodsExtraCountUI();
		UpdateSupplyBoxUI();
		UpdateGalleryUI();
		UpdateFinishCoinUI();
		UpdateLockMachineUI();
		UpdateHightLevelSpellUI();
		UpdateProcessSpellUI();
		UpdateMaxHPUI();
		UpdateExtraDamageUI();
	}

	private void UpdateSlotsUnlockState()
	{
		int num = worldData.endless_LevelOfGoodsExtraCount + worldData.endless_LevelOfSupplyBox + worldData.endless_LevelOfGallery + worldData.endless_LevelOfFinishCoin + worldData.endless_LevelOfLcokMachine + worldData.endless_LevelOfHightLevelSpell + worldData.endless_LevelOfProcessSpell + worldData.endless_LevelOfMaxHP + worldData.endless_LevelOfExtraDamage;
		if (unlockMoreTime == null || unlockMoreTime.Length < 3 || unlockMoreTextPos == null || unlockMoreTextPos.Length < 3)
		{
			Debug.LogError("unlockMoreTime 或 unlockMoreTextPos 数组长度不足 3！");
			return;
		}
		bool flag = num >= unlockMoreTime[0];
		bool flag2 = num >= unlockMoreTime[1];
		bool flag3 = num >= unlockMoreTime[2];
		uiETS_GoodsExtraCount.gameObject.SetActive(value: true);
		uiETS_SupplyBox.gameObject.SetActive(value: true);
		uiETS_Gallery.gameObject.SetActive(flag);
		uiETS_FinishCoin.gameObject.SetActive(flag);
		uiETS_LockMachine.gameObject.SetActive(flag2);
		uiETS_HightLevelSpell.gameObject.SetActive(flag2);
		uiETS_ProcessSpell.gameObject.SetActive(flag3);
		uiETS_MaxHP.gameObject.SetActive(flag3);
		uiETS_ExtraDamage.gameObject.SetActive(flag3);
		string text = 1000569.GetText();
		if (!flag)
		{
			text_UnlockMore.gameObject.SetActive(value: true);
			text_UnlockMore.rectTransform.localPosition = unlockMoreTextPos[0];
			text_UnlockMore.text = text.Replace("int1", (unlockMoreTime[0] - num).ToString());
		}
		else if (!flag2)
		{
			text_UnlockMore.gameObject.SetActive(value: true);
			text_UnlockMore.rectTransform.localPosition = unlockMoreTextPos[1];
			text_UnlockMore.text = text.Replace("int1", (unlockMoreTime[1] - num).ToString());
		}
		else if (!flag3)
		{
			text_UnlockMore.gameObject.SetActive(value: true);
			text_UnlockMore.rectTransform.localPosition = unlockMoreTextPos[2];
			text_UnlockMore.text = text.Replace("int1", (unlockMoreTime[2] - num).ToString());
		}
		else
		{
			text_UnlockMore.gameObject.SetActive(value: false);
		}
	}

	private void NormalizeLevel(ref int level, TalentUpgaradeAttr[] upgrades)
	{
		if (upgrades == null || upgrades.Length == 0)
		{
			level = 0;
		}
		else
		{
			level = Mathf.Clamp(level, 0, upgrades.Length);
		}
	}

	private void TryUpgrade(ref int level, TalentUpgaradeAttr[] upgrades, Action updateUI)
	{
		NormalizeLevel(ref level, upgrades);
		if (upgrades != null && level < upgrades.Length && worldData.GearCount >= upgrades[level].cost)
		{
			PlayerMgr.Inst.ChangeGear(-upgrades[level].cost);
			SEMgr.Inst.uiClick.PlaySE();
			level++;
			updateUI?.Invoke();
			UpdateSlotsUnlockState();
		}
	}

	private void UpgradeGoodsExtraCount()
	{
		TryUpgrade(ref worldData.endless_LevelOfGoodsExtraCount, endlessTalentUpgrade.goodsExtraCount, UpdateGoodsExtraCountUI);
	}

	private void UpdateGoodsExtraCountUI()
	{
		if (worldData.endless_LevelOfGoodsExtraCount == 0)
		{
			uiETS_GoodsExtraCount.Text_Effect.text = "-";
		}
		else
		{
			uiETS_GoodsExtraCount.Text_Effect.text = "+" + worldData.endless_LevelOfGoodsExtraCount;
		}
		if (worldData.endless_LevelOfGoodsExtraCount == endlessTalentUpgrade.goodsExtraCount.Length)
		{
			uiETS_GoodsExtraCount.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_GoodsExtraCount.Text_GearRequire.text = endlessTalentUpgrade.goodsExtraCount[worldData.endless_LevelOfGoodsExtraCount].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfGoodsExtraCount < endlessTalentUpgrade.goodsExtraCount.Length && worldData.GearCount >= endlessTalentUpgrade.goodsExtraCount[worldData.endless_LevelOfGoodsExtraCount].cost;
		uiETS_GoodsExtraCount.go_BtnMask.SetActive(!flag);
	}

	private void UpgradeSupplyBox()
	{
		TryUpgrade(ref worldData.endless_LevelOfSupplyBox, endlessTalentUpgrade.supplyBox, UpdateSupplyBoxUI);
	}

	private void UpdateSupplyBoxUI()
	{
		if (worldData.endless_LevelOfSupplyBox == 0)
		{
			uiETS_SupplyBox.Text_Effect.text = "-";
		}
		else
		{
			uiETS_SupplyBox.Text_Effect.text = 1002109.GetText();
		}
		if (worldData.endless_LevelOfSupplyBox == endlessTalentUpgrade.supplyBox.Length)
		{
			uiETS_SupplyBox.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_SupplyBox.Text_GearRequire.text = endlessTalentUpgrade.supplyBox[worldData.endless_LevelOfSupplyBox].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfSupplyBox < endlessTalentUpgrade.supplyBox.Length && worldData.GearCount >= endlessTalentUpgrade.supplyBox[worldData.endless_LevelOfSupplyBox].cost;
		uiETS_SupplyBox.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeGallery()
	{
		TryUpgrade(ref worldData.endless_LevelOfGallery, endlessTalentUpgrade.gallery, UpdateGalleryUI);
	}

	private void UpdateGalleryUI()
	{
		if (worldData.endless_LevelOfGallery == 0)
		{
			uiETS_Gallery.Text_Effect.text = "-";
		}
		else
		{
			uiETS_Gallery.Text_Effect.text = 1002109.GetText();
		}
		if (worldData.endless_LevelOfGallery == endlessTalentUpgrade.gallery.Length)
		{
			uiETS_Gallery.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_Gallery.Text_GearRequire.text = endlessTalentUpgrade.gallery[worldData.endless_LevelOfGallery].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfGallery < endlessTalentUpgrade.gallery.Length && worldData.GearCount >= endlessTalentUpgrade.gallery[worldData.endless_LevelOfGallery].cost;
		uiETS_Gallery.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeFinishCoin()
	{
		TryUpgrade(ref worldData.endless_LevelOfFinishCoin, endlessTalentUpgrade.finishCoin, UpdateFinishCoinUI);
	}

	private void UpdateFinishCoinUI()
	{
		if (worldData.endless_LevelOfFinishCoin == 0)
		{
			uiETS_FinishCoin.Text_Effect.text = "-";
		}
		else
		{
			uiETS_FinishCoin.Text_Effect.text = "+" + endlessTalentUpgrade.finishCoin[worldData.endless_LevelOfFinishCoin - 1].value;
		}
		if (worldData.endless_LevelOfFinishCoin == endlessTalentUpgrade.finishCoin.Length)
		{
			uiETS_FinishCoin.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_FinishCoin.Text_GearRequire.text = endlessTalentUpgrade.finishCoin[worldData.endless_LevelOfFinishCoin].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfFinishCoin < endlessTalentUpgrade.finishCoin.Length && worldData.GearCount >= endlessTalentUpgrade.finishCoin[worldData.endless_LevelOfFinishCoin].cost;
		uiETS_FinishCoin.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeLockMachine()
	{
		TryUpgrade(ref worldData.endless_LevelOfLcokMachine, endlessTalentUpgrade.lockMachine, UpdateLockMachineUI);
	}

	private void UpdateLockMachineUI()
	{
		if (worldData.endless_LevelOfLcokMachine == 0)
		{
			uiETS_LockMachine.Text_Effect.text = "-";
		}
		else
		{
			uiETS_LockMachine.Text_Effect.text = 1002109.GetText();
		}
		if (worldData.endless_LevelOfLcokMachine == endlessTalentUpgrade.lockMachine.Length)
		{
			uiETS_LockMachine.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_LockMachine.Text_GearRequire.text = endlessTalentUpgrade.lockMachine[worldData.endless_LevelOfLcokMachine].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfLcokMachine < endlessTalentUpgrade.lockMachine.Length && worldData.GearCount >= endlessTalentUpgrade.lockMachine[worldData.endless_LevelOfLcokMachine].cost;
		uiETS_LockMachine.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeHightLevelSpell()
	{
		TryUpgrade(ref worldData.endless_LevelOfHightLevelSpell, endlessTalentUpgrade.hightLevelSpell, UpdateHightLevelSpellUI);
	}

	private void UpdateHightLevelSpellUI()
	{
		if (worldData.endless_LevelOfHightLevelSpell == 0)
		{
			uiETS_HightLevelSpell.Text_Effect.text = "-";
		}
		else
		{
			uiETS_HightLevelSpell.Text_Effect.text = endlessTalentUpgrade.hightLevelSpell[worldData.endless_LevelOfHightLevelSpell - 1].value + "%";
		}
		if (worldData.endless_LevelOfHightLevelSpell == endlessTalentUpgrade.hightLevelSpell.Length)
		{
			uiETS_HightLevelSpell.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_HightLevelSpell.Text_GearRequire.text = endlessTalentUpgrade.hightLevelSpell[worldData.endless_LevelOfHightLevelSpell].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfHightLevelSpell < endlessTalentUpgrade.hightLevelSpell.Length && worldData.GearCount >= endlessTalentUpgrade.hightLevelSpell[worldData.endless_LevelOfHightLevelSpell].cost;
		uiETS_HightLevelSpell.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeProcessSpell()
	{
		TryUpgrade(ref worldData.endless_LevelOfProcessSpell, endlessTalentUpgrade.processSpell, UpdateProcessSpellUI);
	}

	private void UpdateProcessSpellUI()
	{
		if (worldData.endless_LevelOfProcessSpell == 0)
		{
			uiETS_ProcessSpell.Text_Effect.text = "-";
		}
		else
		{
			uiETS_ProcessSpell.Text_Effect.text = 1002109.GetText();
		}
		if (worldData.endless_LevelOfProcessSpell == endlessTalentUpgrade.processSpell.Length)
		{
			uiETS_ProcessSpell.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			uiETS_ProcessSpell.Text_GearRequire.text = endlessTalentUpgrade.processSpell[worldData.endless_LevelOfProcessSpell].cost.ToString();
		}
		bool flag = worldData.endless_LevelOfProcessSpell < endlessTalentUpgrade.processSpell.Length && worldData.GearCount >= endlessTalentUpgrade.processSpell[worldData.endless_LevelOfProcessSpell].cost;
		uiETS_ProcessSpell.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeMaxHP()
	{
		if (endlessTalentUpgrade.maxHP != null && endlessTalentUpgrade.maxHP.Length != 0)
		{
			int num = endlessTalentUpgrade.maxHP[0].cost * (worldData.endless_LevelOfMaxHP + 1);
			if (worldData.endless_LevelOfMaxHP < maxLevelOfMaxHPAndExtraDamage && worldData.GearCount >= num)
			{
				worldData.endless_LevelOfMaxHP++;
				PlayerMgr.Inst.ChangeGear(-num);
				UpdateMaxHPUI();
				UpdateSlotsUnlockState();
			}
		}
	}

	private void UpdateMaxHPUI()
	{
		if (endlessTalentUpgrade.maxHP == null || endlessTalentUpgrade.maxHP.Length == 0)
		{
			uiETS_MaxHP.go_BtnMask.SetActive(value: true);
			return;
		}
		if (worldData.endless_LevelOfMaxHP == 0)
		{
			uiETS_MaxHP.Text_Effect.text = "-";
		}
		else
		{
			uiETS_MaxHP.Text_Effect.text = "+" + endlessTalentUpgrade.maxHP[0].value * worldData.endless_LevelOfMaxHP;
		}
		if (worldData.endless_LevelOfMaxHP >= maxLevelOfMaxHPAndExtraDamage)
		{
			uiETS_MaxHP.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			int num = endlessTalentUpgrade.maxHP[0].cost * (worldData.endless_LevelOfMaxHP + 1);
			uiETS_MaxHP.Text_GearRequire.text = num.ToString();
		}
		int num2 = endlessTalentUpgrade.maxHP[0].cost * (worldData.endless_LevelOfMaxHP + 1);
		bool flag = worldData.endless_LevelOfMaxHP < maxLevelOfMaxHPAndExtraDamage && worldData.GearCount >= num2;
		uiETS_MaxHP.go_BtnMask.SetActive(!flag);
	}

	public void UpgradeExtraDamage()
	{
		if (endlessTalentUpgrade.extraDamage != null && endlessTalentUpgrade.extraDamage.Length != 0)
		{
			int num = endlessTalentUpgrade.extraDamage[0].cost * (worldData.endless_LevelOfExtraDamage + 1);
			if (worldData.endless_LevelOfExtraDamage < maxLevelOfMaxHPAndExtraDamage && worldData.GearCount >= num)
			{
				worldData.endless_LevelOfExtraDamage++;
				PlayerMgr.Inst.ChangeGear(-num);
				UpdateExtraDamageUI();
				UpdateSlotsUnlockState();
			}
		}
	}

	private void UpdateExtraDamageUI()
	{
		if (endlessTalentUpgrade.extraDamage == null || endlessTalentUpgrade.extraDamage.Length == 0)
		{
			uiETS_ExtraDamage.go_BtnMask.SetActive(value: true);
			return;
		}
		if (worldData.endless_LevelOfExtraDamage == 0)
		{
			uiETS_ExtraDamage.Text_Effect.text = "-";
		}
		else
		{
			uiETS_ExtraDamage.Text_Effect.text = "+" + endlessTalentUpgrade.extraDamage[0].value * worldData.endless_LevelOfExtraDamage + "%";
		}
		if (worldData.endless_LevelOfExtraDamage >= maxLevelOfMaxHPAndExtraDamage)
		{
			uiETS_ExtraDamage.Text_GearRequire.text = 1000512.GetText();
		}
		else
		{
			int num = endlessTalentUpgrade.extraDamage[0].cost * (worldData.endless_LevelOfExtraDamage + 1);
			uiETS_ExtraDamage.Text_GearRequire.text = num.ToString();
		}
		int num2 = endlessTalentUpgrade.extraDamage[0].cost * (worldData.endless_LevelOfExtraDamage + 1);
		bool flag = worldData.endless_LevelOfExtraDamage < maxLevelOfMaxHPAndExtraDamage && worldData.GearCount >= num2;
		uiETS_ExtraDamage.go_BtnMask.SetActive(!flag);
	}
}
