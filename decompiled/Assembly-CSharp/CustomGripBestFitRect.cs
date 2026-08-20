using UnityEngine;
using UnityEngine.UI;

public class CustomGripBestFitRect : MonoBehaviour
{
	public Vector2 pivot;

	public GridLayoutGroup gridLayoutGroup;

	private RectTransform rtsf_Self;

	public int minSize;

	public int maxSize;

	public int currentSize;

	public void Layout()
	{
		rtsf_Self = GetComponent<RectTransform>();
		int childCount = base.transform.childCount;
		float width = rtsf_Self.rect.width;
		float height = rtsf_Self.rect.height;
		float num = 0f;
		int constraintCount = 1;
		for (int i = 0; i < childCount; i++)
		{
			float num2 = width / (float)(i + 1);
			if (num2 > (float)maxSize)
			{
				num2 = maxSize;
			}
			else if (num2 < (float)minSize)
			{
				num2 = minSize;
			}
			if (!((float)Mathf.CeilToInt((float)childCount / ((float)i + 1f)) * num2 > height) && num2 > num)
			{
				num = num2;
				constraintCount = i + 1;
			}
		}
		currentSize = maxSize;
		gridLayoutGroup.cellSize = new Vector2(num, num);
		gridLayoutGroup.constraintCount = constraintCount;
		gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
	}
}
