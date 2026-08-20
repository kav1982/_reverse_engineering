using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRelicGroupActiveTip : MonoBehaviour
{
	public float scaleStart = 1.2f;

	public Text title;

	public CanvasGroup canvasGroup;

	public RectTransform scrollRect;

	public RectTransform iconsLayout;

	public GameObject iconTemplate;

	public void Initialize(RelicGroupConfig config)
	{
		title.text = 1006402.GetText() + config.GetName();
		int[] items = config.items;
		foreach (int num in items)
		{
			GameObject obj = Object.Instantiate(iconTemplate, iconsLayout);
			obj.SetActive(value: true);
			obj.GetComponent<Image>().sprite = ABResources.LoadAsset<Sprite>("Textures/RelicIcons/" + num);
		}
		float x = scrollRect.sizeDelta.x;
		scrollRect.localScale = Vector3.one * scaleStart;
		scrollRect.sizeDelta = new Vector2(0f, scrollRect.sizeDelta.y);
		DOTween.Sequence(base.gameObject).Append(scrollRect.DOSizeDelta(new Vector2(x, scrollRect.sizeDelta.y), 0.5f)).Join(scrollRect.DOScale(Vector3.one, 0.5f))
			.AppendInterval(1.6f)
			.Append(canvasGroup.DOFade(0f, 0.4f))
			.AppendCallback(delegate
			{
				Object.Destroy(base.gameObject);
			});
	}
}
