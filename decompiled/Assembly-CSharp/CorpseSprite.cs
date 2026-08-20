using UnityEngine;

public class CorpseSprite : MonoBehaviour
{
	public SpriteRenderer main;

	public SpriteRenderer shadow;

	public void SetTransform(Vector3 position, float angle)
	{
		main.transform.position = Tool2D.GetLayerPoint(position);
		main.transform.eulerAngles = new Vector3(0f, 0f, angle);
		shadow.transform.position = Tool2D.IgnoreZPoint(position, 1.05f);
		shadow.transform.eulerAngles = new Vector3(0f, 0f, angle);
	}

	public void SetTransformDrop(Vector3 position)
	{
		main.transform.position = Tool2D.GetLayerPoint(position, LayerCorrectType.Corpse);
	}

	public void SetScale(float scale)
	{
		main.transform.localScale = new Vector3(scale, scale, 1f);
		shadow.transform.localScale = new Vector3(scale, scale, 1f);
	}

	public void SetColor(Color color)
	{
		main.color = color;
	}

	public void SetSprite(Sprite sprite)
	{
		main.sprite = sprite;
		shadow.sprite = sprite;
	}

	public void SetAlpha(float alpha)
	{
		Color color = main.color;
		color.a = alpha;
		main.color = color;
	}
}
