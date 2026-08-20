using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameServer;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UISuit")]
public class UISuit : GameUISingletonMono<UISuit>
{
	[Serializable]
	public class SuitPurchaseConfig
	{
		public Button purchaseBtn;

		public Text text;

		public GameObject des;

		public Image imageHighlightButton;

		public ServerAPI.ProductItem product;

		public Image imageTitle;

		public Image imageMain;
	}

	public Text textCost;

	public GameObject gameobjectCantBuyHint;

	public GameObject duplacatePurchaseHint;

	public CanvasGroup canvasGroup;

	public Text unlockTMP;

	public Button unlockBtn;

	public Button closeBtn;

	public Color textColorNormal;

	public Color textColorSelected;

	public List<SuitPurchaseConfig> allSuitPurchaseConfigs;

	[Header("PurchaseWarning")]
	public Button purchaseStillButton;

	public Button canclePurchaseButton;

	public GameObject purchaseWarning;

	private void LanguageChange()
	{
		unlockTMP.text = 1006903.GetText();
	}

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = true;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		TweenerCore<float, float, FloatOptions> tweenerCore = canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			TimeScaleMgr.Inst.Pause();
		});
		LanguageChange();
		allSuitPurchaseConfigs.ForEach(delegate(SuitPurchaseConfig x)
		{
			x.purchaseBtn.onClick.RemoveAllListeners();
			x.purchaseBtn.onClick.AddListener(delegate
			{
				UpdateMerchandise(x.product);
			});
		});
		UpdateMerchandise(allSuitPurchaseConfigs[0].product);
	}

	public void UpdateMerchandise(ServerAPI.ProductItem product)
	{
		string cost = ICJNOGPFMAM.GetCost(product);
		textCost.text = ((cost != "") ? ("￥" + cost + "  购买") : "购买");
		allSuitPurchaseConfigs.ForEach(delegate(SuitPurchaseConfig x)
		{
			bool flag2 = x.product == product;
			x.imageMain.gameObject.SetActive(flag2);
			x.text.color = (flag2 ? textColorSelected : textColorNormal);
			x.imageHighlightButton.gameObject.SetActive(flag2);
			x.imageTitle.gameObject.SetActive(flag2);
			x.des.gameObject.SetActive(flag2);
			if (flag2)
			{
				x.imageTitle.gameObject.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 1, 0.5f).SetUpdate(isIndependentUpdate: true);
			}
		});
		bool flag = ICJNOGPFMAM.CanBuyItem(product);
		unlockBtn.interactable = flag;
		unlockBtn.onClick.RemoveAllListeners();
		purchaseStillButton.onClick.RemoveAllListeners();
		gameobjectCantBuyHint.SetActive(!flag);
		if (flag && ICJNOGPFMAM.DuplicateItem(product))
		{
			purchaseStillButton.onClick.AddListener(delegate
			{
				OnUnlockBtnClick(product);
			});
			unlockBtn.onClick.AddListener(delegate
			{
				OnUnlockBtnDuplicateBlick(product);
			});
		}
		else
		{
			unlockBtn.onClick.AddListener(delegate
			{
				OnUnlockBtnClick(product);
			});
		}
	}

	public override void Hide()
	{
		if (purchaseWarning.activeInHierarchy)
		{
			purchaseWarning.SetActive(value: false);
		}
		else
		{
			base.Hide();
		}
	}

	protected override void OnHide()
	{
		canvasGroup.blocksRaycasts = false;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		TweenerCore<float, float, FloatOptions> tweenerCore = canvasGroup.DOFade(0f, 0.5f).SetUpdate(isIndependentUpdate: true);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			TimeScaleMgr.Inst.Recovery();
		});
	}

	protected override void RegistarWhenInit()
	{
		closeBtn.onClick.AddListener(OnCloseBtnClick);
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		canclePurchaseButton.onClick.RemoveAllListeners();
		canclePurchaseButton.onClick.AddListener(delegate
		{
			purchaseWarning.gameObject.SetActive(value: false);
		});
	}

	private void OnCloseBtnClick()
	{
		Hide();
	}

	public void OnUnlockBtnDuplicateBlick(ServerAPI.ProductItem product)
	{
		purchaseWarning.SetActive(value: true);
	}

	private void OnUnlockBtnClick(ServerAPI.ProductItem product)
	{
		purchaseWarning.gameObject.SetActive(value: false);
		if (!ICJNOGPFMAM.CanBuyItem(product))
		{
			Hide();
		}
		else
		{
			MobileMgr.inst.PluginActivity.Buy(product);
		}
	}

	private void OnPurchase()
	{
		UpdateMerchandise(allSuitPurchaseConfigs.First((SuitPurchaseConfig x) => x.imageHighlightButton.gameObject.activeInHierarchy).product);
		if (ICJNOGPFMAM.IMFNIOLONJP)
		{
			Hide();
		}
	}

	protected override void RegistarOnlyWhenOpen()
	{
		EventMgr.RoleItemChange = (Action)Delegate.Combine(EventMgr.RoleItemChange, new Action(OnPurchase));
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		EventMgr.RoleItemChange = (Action)Delegate.Remove(EventMgr.RoleItemChange, new Action(OnPurchase));
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}
}
