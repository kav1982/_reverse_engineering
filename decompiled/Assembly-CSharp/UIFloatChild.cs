using UnityEngine;

public class UIFloatChild : MonoBehaviour
{
	public RectTransform Child;

	private RectTransform self;

	private void Start()
	{
		self = GetComponent<RectTransform>();
	}

	private void Update()
	{
		float x = self.rect.width * self.localScale.x + self.anchoredPosition.x;
		Vector2 anchoredPosition = Child.anchoredPosition;
		anchoredPosition.x = x;
		Child.anchoredPosition = anchoredPosition;
	}

	private void OnEnable()
	{
		Child.gameObject.SetActive(value: true);
	}

	private void OnDisable()
	{
		Child.gameObject.SetActive(value: false);
	}
}
