using UnityEngine;

public class SpecialObj27 : LayerCorrect
{
	[Space(50f)]
	public Sprite[] sprites;

	public SpriteRenderer sr;

	public override void OnEnable()
	{
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		base.OnEnable();
	}
}
