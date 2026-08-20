using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPotion : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public Image image_Icon;

	public Text text_Name;

	public Text text_Type;

	public Text text_Info;

	public float textBGWidthExtra = 8f;

	public RectTransform textTypeBG;

	public float paddingDown;

	public void UpdateInfo(int id, RectTransform rtsf_Curse = null, bool isFromBuild = false)
	{
		StartCoroutine(UpdateInfoIE(id, rtsf_Curse, isFromBuild));
	}

	private IEnumerator UpdateInfoIE(int id, RectTransform rtsf_Curse = null, bool isFromBuild = false)
	{
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 0f;
		}
		PotionConfig potionConfig = PotionConfig.dic[id];
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(potionConfig.GetIconPath());
		text_Name.text = potionConfig.GetName();
		text_Type.text = 1002209.GetText();
		text_Info.text = potionConfig.GetInfo();
		yield return null;
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - text_Info.rectTransform.anchoredPosition.y + text_Info.rectTransform.sizeDelta.y + paddingDown);
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 1f;
		}
	}
}
