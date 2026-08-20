using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class UIHandbookVideoTextCtrl : MonoBehaviour
{
	[Serializable]
	public class TextAction
	{
		public float time;

		public RectTransform rect;

		public TextActionType action;
	}

	public enum TextActionType
	{
		Show,
		Hide,
		Show_Fade,
		Hide_Fade,
		Show_Fly
	}

	public List<TextAction> textActions = new List<TextAction>();

	private UIImageVideoPlayer vp;

	private float lastFrameVideoTime;

	private void OnEnable()
	{
		ClearState();
	}

	private void OnDisable()
	{
		foreach (TextAction textAction in textActions)
		{
			DOTween.Kill(textAction.rect, complete: true);
		}
	}

	private void Update()
	{
		float currentTime = vp.Time;
		if (currentTime > lastFrameVideoTime)
		{
			foreach (TextAction item in textActions.Where((TextAction action) => action.time >= lastFrameVideoTime && action.time < currentTime))
			{
				Action(item);
			}
		}
		lastFrameVideoTime = currentTime;
	}

	public void BindVideoPlayer(UIImageVideoPlayer videoPlayer)
	{
		vp = videoPlayer;
	}

	public void ClearState()
	{
		HideAll();
		lastFrameVideoTime = 0f;
	}

	private void HideAll()
	{
		foreach (TextAction textAction in textActions)
		{
			if ((bool)textAction.rect)
			{
				textAction.rect.gameObject.SetActive(value: false);
			}
		}
	}

	private void Action(TextAction action)
	{
		RectTransform rect = action.rect;
		switch (action.action)
		{
		case TextActionType.Hide:
			rect.gameObject.SetActive(value: false);
			break;
		case TextActionType.Show:
			rect.gameObject.SetActive(value: true);
			break;
		case TextActionType.Show_Fade:
		{
			rect.gameObject.SetActive(value: true);
			CanvasGroup component3 = rect.GetComponent<CanvasGroup>();
			component3.alpha = 0f;
			DOTween.Sequence(rect).Append(component3.DOFade(1f, 0.33f)).SetUpdate(isIndependentUpdate: true);
			break;
		}
		case TextActionType.Hide_Fade:
		{
			CanvasGroup component2 = rect.GetComponent<CanvasGroup>();
			DOTween.Sequence(rect).Append(component2.DOFade(0f, 0.33f)).AppendCallback(delegate
			{
				rect.gameObject.SetActive(value: false);
			})
				.SetUpdate(isIndependentUpdate: true);
			break;
		}
		case TextActionType.Show_Fly:
		{
			Vector3 localPosition = rect.localPosition;
			Vector3 localPosition2 = localPosition;
			localPosition2.y -= 20f;
			rect.localPosition = localPosition2;
			CanvasGroup component = rect.GetComponent<CanvasGroup>();
			component.alpha = 0f;
			rect.gameObject.SetActive(value: true);
			DOTween.Sequence(rect).Append(rect.DOLocalMoveY(localPosition.y, 0.33f)).Join(component.DOFade(1f, 0.33f))
				.SetUpdate(isIndependentUpdate: true);
			break;
		}
		}
	}
}
