using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UICommonHintRetryOrQuit")]
public class UICommonHintRetryOrQuit : GameUISingletonMono<UICommonHintRetryOrQuit>
{
	public Button retryButton;

	public Button quitButton;

	public GameObject uiRoot;

	public Text Text;

	public CanvasGroup canvasGroup;

	public Action ActionOnReTry;

	public override void Show(object obj = null)
	{
		if (!base.IsOpen)
		{
			base.Show(obj);
			return;
		}
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
		OnShow(obj);
	}

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		quitButton.onClick.RemoveAllListeners();
		quitButton.onClick.AddListener(delegate
		{
			SEMgr.Inst.uiClick.PlaySE();
			Quit();
		});
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		uiRoot.gameObject.SetActive(value: true);
		canvasGroup.alpha = 0f;
		canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		if (obj is Action retryAction2)
		{
			Text.text = "与服务器连接失败，请检查您的网络是否异常";
			SetRetryMethod(retryAction2);
		}
		if (obj is ITuple tuple && tuple.Length == 2 && tuple[0] is string text && tuple[1] is Action retryAction3)
		{
			Text.text = text;
			SetRetryMethod(retryAction3);
		}
		else
		{
			Debug.Log("参数错误");
		}
		void SetRetryMethod(Action retryAction)
		{
			ActionOnReTry = retryAction;
			retryButton.onClick.RemoveAllListeners();
			retryButton.onClick.AddListener(delegate
			{
				SEMgr.Inst.uiClick.PlaySE();
				ActionOnReTry?.Invoke();
			});
		}
	}

	protected override void OnHide()
	{
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
		uiRoot.gameObject.SetActive(value: false);
	}

	public void Quit()
	{
		GameMgr.QuitGame();
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
