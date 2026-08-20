using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoGeneral : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public Text text_Name;

	public Text text_Info;

	public float paddingDown;

	public void UpdateInfo(string name, string info)
	{
		StartCoroutine(UpdateInfoIE(name, info));
	}

	private IEnumerator UpdateInfoIE(string name, string info)
	{
		text_Name.text = name;
		text_Info.text = "◆\u00a0\u200a" + info;
		yield return null;
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - text_Info.rectTransform.anchoredPosition.y + text_Info.rectTransform.sizeDelta.y + paddingDown);
	}
}
