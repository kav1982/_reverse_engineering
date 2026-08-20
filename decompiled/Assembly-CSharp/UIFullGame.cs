using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameServer;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIFullGame")]
public class UIFullGame : GameUISingletonMono<UIFullGame>
{
	[Serializable]
	public class BuyGameConfig
	{
		public Text textCost;

		public Button buyBtn;

		public ServerAPI.ProductItem product;
	}

	public CanvasGroup canvasGroup;

	public Button closeBtn;

	public List<BuyGameConfig> buyGameConfigs;

	private void Awake()
	{
		EventMgr.PlayerDead = (Action)Delegate.Combine(EventMgr.PlayerDead, new Action(OnPlayerDead));
	}

	private void OnPlayerDead()
	{
		UIPlayerDataMgr.Inst.buyGameFX.SetActive(value: false);
	}

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = true;
		PlayerMgr.Inst.PlayerCtrller?.StopMotion();
		TweenerCore<float, float, FloatOptions> tweenerCore = canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			if ((bool)PlayerMgr.Inst.PlayerCtrller)
			{
				TimeScaleMgr.Inst.Pause();
			}
		});
	}

	protected override void OnHide()
	{
		canvasGroup.blocksRaycasts = false;
		PlayerMgr.Inst.PlayerCtrller?.StartMotion();
		TweenerCore<float, float, FloatOptions> tweenerCore = canvasGroup.DOFade(0f, 0.5f).SetUpdate(isIndependentUpdate: true);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			if ((bool)PlayerMgr.Inst.PlayerCtrller)
			{
				TimeScaleMgr.Inst.Recovery();
			}
		});
	}

	protected override void RegistarWhenInit()
	{
		foreach (BuyGameConfig buyGameConfig in buyGameConfigs)
		{
			buyGameConfig.buyBtn.onClick.RemoveAllListeners();
			buyGameConfig.buyBtn.onClick.AddListener(delegate
			{
				OnUnlockBtnClick(buyGameConfig.product);
			});
			string cost = ICJNOGPFMAM.GetCost(buyGameConfig.product);
			buyGameConfig.textCost.text = ((cost != "") ? ("购买：" + cost + "元") : "购买");
		}
		Debug.Log("RegistarWhenInit");
		closeBtn.onClick.RemoveAllListeners();
		closeBtn.onClick.AddListener(OnCloseBtnClick);
	}

	private void OnCloseBtnClick()
	{
		Debug.Log("OnCloseBtnClick");
		Hide();
	}

	private void OnUnlockBtnClick(ServerAPI.ProductItem product)
	{
		if (ICJNOGPFMAM.MIFJADDOODN)
		{
			Hide();
		}
		else
		{
			MobileMgr.inst.PluginActivity.Buy(product);
		}
	}

	private void OnUnlockDeluxeBtnClick()
	{
		if (ICJNOGPFMAM.MIFJADDOODN || ICJNOGPFMAM.FIKDMCBJPCO)
		{
			Hide();
		}
		else
		{
			MobileMgr.inst.PluginActivity.Buy(ServerAPI.ProductItem.HalloweenBundle);
		}
	}

	private void CheckItem()
	{
		if (ICJNOGPFMAM.MIFJADDOODN)
		{
			Hide();
		}
	}

	protected override void RegistarOnlyWhenOpen()
	{
		EventMgr.RoleItemChange = (Action)Delegate.Combine(EventMgr.RoleItemChange, new Action(CheckItem));
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		EventMgr.RoleItemChange = (Action)Delegate.Remove(EventMgr.RoleItemChange, new Action(CheckItem));
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.PlayerDead = (Action)Delegate.Remove(EventMgr.PlayerDead, new Action(OnPlayerDead));
	}
}
