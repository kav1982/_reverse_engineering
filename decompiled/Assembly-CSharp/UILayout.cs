using UnityEngine;

public class UILayout : MonoBehaviour
{
	public UILayoutType type;

	public bool reverseDir;

	public Vector2 padding;

	public Vector2 childSize;

	public Vector2 space;

	public float selfWidthOffset;

	private RectTransform rtsf_Self;

	public void Layout(bool includeInactive = true)
	{
		if (rtsf_Self == null)
		{
			rtsf_Self = (RectTransform)base.transform;
		}
		if (base.transform.childCount == 0)
		{
			rtsf_Self.sizeDelta = Vector2.zero;
			return;
		}
		switch (type)
		{
		case UILayoutType.Horizontal:
		{
			int num3 = 0;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				RectTransform rectTransform2 = (RectTransform)base.transform.GetChild(j);
				if (includeInactive || (!includeInactive && rectTransform2.gameObject.activeSelf))
				{
					rectTransform2.sizeDelta = new Vector2(childSize.x, childSize.y);
					if (reverseDir)
					{
						float x = padding.x + (float)(base.transform.childCount - num3 - 1) * (space.x + childSize.x);
						rectTransform2.anchoredPosition = new Vector2(x, 0f - padding.y);
					}
					else
					{
						float x2 = padding.x + (float)num3 * (space.x + childSize.x);
						rectTransform2.anchoredPosition = new Vector2(x2, 0f - padding.y);
					}
					num3++;
				}
			}
			float y3 = childSize.y + padding.y * 2f;
			rtsf_Self.sizeDelta = new Vector2(childSize.x * (float)num3 + space.x * (float)(num3 - 1) + padding.x * 2f + selfWidthOffset, y3);
			break;
		}
		case UILayoutType.Vertical:
		{
			int num = 0;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (includeInactive || (!includeInactive && rectTransform.gameObject.activeSelf))
				{
					rectTransform.sizeDelta = new Vector2(childSize.x, childSize.y);
					if (reverseDir)
					{
						float y = 0f - padding.y - (float)(base.transform.childCount - num - 1) * (space.y + childSize.y);
						rectTransform.anchoredPosition = new Vector2(padding.x, y);
					}
					else
					{
						float y2 = 0f - padding.y - (float)num * (space.y + childSize.y);
						rectTransform.anchoredPosition = new Vector2(padding.x, y2);
					}
					num++;
				}
			}
			float num2 = childSize.x + padding.x * 2f;
			rtsf_Self.sizeDelta = new Vector2(num2 + selfWidthOffset, childSize.y * (float)num + space.y * (float)(num - 1) + padding.y * 2f);
			break;
		}
		default:
			Debug.LogError(type);
			break;
		}
	}
}
