using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceInfo : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public Text text_Name;

	public Text text_Type;

	public Text text_Info;

	public float paddingDown;

	public float textBGWidthExtra = 8f;

	public RectTransform textTypeBG;

	public void UpdateInfo(int id, RectTransform rtsf_Curse = null)
	{
		StartCoroutine(UpdateInfoIE(id, rtsf_Curse));
	}

	private IEnumerator UpdateInfoIE(int id, RectTransform rtsf_Curse = null)
	{
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 0f;
		}
		ResourceConfig resourceConfig = ResourceConfig.dic[id];
		text_Name.text = resourceConfig.GetName();
		text_Type.text = 1002201.GetText();
		text_Info.text = resourceConfig.GetInfo();
		yield return null;
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - text_Info.rectTransform.anchoredPosition.y + text_Info.rectTransform.sizeDelta.y + paddingDown);
		if (rtsf_Curse != null)
		{
			rtsf_Curse.anchoredPosition = rtsf_Self.anchoredPosition + new Vector2(0f, rtsf_Self.sizeDelta.y);
		}
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 1f;
		}
	}
}
