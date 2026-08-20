using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIServerCode")]
public class UIServerCode : GameUISingletonMono<UIServerCode>
{
	public CanvasGroup canvasGroup;

	public Text text;

	protected override void OnShow(object obj = null)
	{
		base.OnShow(obj);
		if (obj is string text)
		{
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
			this.text.text = text;
		}
	}

	protected override void OnHide()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.DOFade(0f, 0.5f).SetUpdate(isIndependentUpdate: true);
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
