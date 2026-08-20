using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIUnlockNewResearch : MonoBehaviour
{
	public float scaleStart = 1.2f;

	public Text title;

	public Image researchImage;

	public CanvasGroup canvasGroup;

	public RectTransform scrollRect;

	public void Initialize(ResearchConfig config)
	{
		title.text = "已解锁:" + config.GetName();
		researchImage.sprite = ABResources.LoadAsset<Sprite>("Textures/ResearchIcons/" + config.icon);
		float x = scrollRect.sizeDelta.x;
		scrollRect.localScale = Vector3.one * scaleStart;
		scrollRect.sizeDelta = new Vector2(0f, scrollRect.sizeDelta.y);
		if (!GameMgr.IsMobile_Static)
		{
			base.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 400f);
		}
		canvasGroup.alpha = 0f;
		DOTween.Sequence(base.gameObject).Append(canvasGroup.DOFade(1f, 0.8f)).Join(scrollRect.DOSizeDelta(new Vector2(x, scrollRect.sizeDelta.y), 0.5f))
			.Join(scrollRect.DOScale(Vector3.one, 0.5f))
			.AppendInterval(1.6f)
			.Append(canvasGroup.DOFade(0f, 0.4f))
			.AppendCallback(delegate
			{
				Object.Destroy(base.gameObject);
			});
	}
}
