using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UICommonHint")]
public class UICommonHint : GameUISingletonMono<UICommonHint>
{
	public GameObject closeButton;

	public GameObject goText;

	public GameObject goImage;

	public GameObject uiRoot;

	public Text Text;

	public CanvasGroup canvasGroup;

	public Action ActionOnClose;

	public override void Show(object obj = null)
	{
		ActionOnClose?.Invoke();
		ActionOnClose = null;
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
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		uiRoot.gameObject.SetActive(value: true);
		goText.gameObject.SetActive(value: false);
		goImage.gameObject.SetActive(value: false);
		canvasGroup.alpha = 0f;
		canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		closeButton.gameObject.SetActive(value: true);
		if (obj is string)
		{
			goText.gameObject.SetActive(value: true);
			Text.text = obj.ToString();
		}
		else if (obj is Sprite sprite)
		{
			goImage.gameObject.SetActive(value: true);
			goImage.GetComponent<Image>().sprite = sprite;
			goImage.GetComponent<Image>().SetNativeSize();
		}
		else if (obj is ITuple tuple && tuple.Length == 2 && tuple[0] is string text && tuple[1] is bool active)
		{
			goText.gameObject.SetActive(value: true);
			Text.text = text;
			closeButton.SetActive(active);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(goImage.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(goText.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(uiRoot.GetComponent<RectTransform>());
	}

	protected override void OnHide()
	{
		Debug.Log("Hide");
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
		uiRoot.gameObject.SetActive(value: false);
		ActionOnClose?.Invoke();
	}

	public override void _Close()
	{
		base._Close();
		SEMgr.Inst.uiClick.PlaySE();
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
