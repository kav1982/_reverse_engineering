using UnityEngine;
using UnityEngine.UI;

public class UIMP : MonoBehaviour
{
	public Image image_Icon;

	public Color color_Have;

	public Color color_NoHave;

	public Color color_Temp;

	public void SetHave()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		image_Icon.color = color_Have;
	}

	public void SetNoHave()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		image_Icon.color = color_NoHave;
	}

	public void SetTemp()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		image_Icon.color = color_Temp;
	}

	public void SetHide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
