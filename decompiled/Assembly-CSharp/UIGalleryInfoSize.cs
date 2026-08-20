using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIGalleryInfoSize : MonoBehaviour
{
	public float MinHeight;

	public RectTransform ChildRect;

	private RectTransform _rect;

	private LayoutGroup _layout;

	private void Awake()
	{
		_rect = GetComponent<RectTransform>();
		_layout = GetComponentInParent<LayoutGroup>();
	}

	private void Update()
	{
		if (!(ChildRect == null))
		{
			float y = Mathf.Max(ChildRect.rect.height, MinHeight);
			Vector2 sizeDelta = _rect.sizeDelta;
			sizeDelta.y = y;
			_rect.sizeDelta = sizeDelta;
			if ((bool)_layout)
			{
				_layout.enabled = !_layout.enabled;
			}
		}
	}
}
