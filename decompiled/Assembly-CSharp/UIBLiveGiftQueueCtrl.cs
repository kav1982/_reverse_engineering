using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBLiveGiftQueueCtrl : MonoBehaviour
{
	public RectTransform rect;

	public CanvasGroup fadeGroup;

	public Text giftNumberText;

	public RectTransform giftNumberRect;

	private int lastGiftNumber = int.MaxValue;

	private void Awake()
	{
		if (BLiveMgr.Inst == null)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (BLiveMgr.Inst.QueueCount > 0)
		{
			Show();
			int queueCount = BLiveMgr.Inst.QueueCount;
			if (queueCount > lastGiftNumber)
			{
				DOTween.Sequence().SetUpdate(isIndependentUpdate: true).Append(giftNumberRect.DOScale(1.5f, 0.05f))
					.Append(giftNumberRect.DOScale(1f, 0.2f));
			}
			lastGiftNumber = queueCount;
			giftNumberText.text = queueCount.ToString();
		}
		else
		{
			Hide();
		}
	}

	private void Hide()
	{
		if (rect.gameObject.activeSelf)
		{
			rect.gameObject.SetActive(value: false);
		}
	}

	private void Show()
	{
		if (!rect.gameObject.activeSelf)
		{
			lastGiftNumber = 0;
			rect.localPosition -= new Vector3(0f, 20f, 0f);
			fadeGroup.alpha = 0f;
			rect.DOLocalMoveY(0f, 0.2f).SetUpdate(isIndependentUpdate: true);
			fadeGroup.DOFade(1f, 0.2f).SetUpdate(isIndependentUpdate: true);
			rect.gameObject.SetActive(value: true);
		}
	}
}
