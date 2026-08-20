using UnityEngine;

public class SpecialObj1 : LayerCorrect
{
	[Space(50f)]
	public Sprite[] sprites;

	public SpriteRenderer sr;

	private void Start()
	{
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		if (Random.Range(0, 2) == 0)
		{
			sr.flipX = true;
		}
		Object.Destroy(this);
	}
}
