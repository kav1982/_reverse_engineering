using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoGeneralPure : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public Text text_Info;

	public float paddingDown;

	public void UpdateInfo(string info)
	{
		StartCoroutine(UpdateInfoIE(info));
	}

	private IEnumerator UpdateInfoIE(string info)
	{
		text_Info.text = info;
		yield return null;
		rtsf_Self.sizeDelta = new Vector2(text_Info.rectTransform.anchoredPosition.x + text_Info.rectTransform.sizeDelta.x + paddingDown, rtsf_Self.sizeDelta.y);
	}
}
