using UnityEngine;

public class Tile_T10_Tile3 : LayerCorrect
{
	[Space(50f)]
	public Sprite[] sprites;

	public SpriteRenderer sr;

	public override void OnEnable()
	{
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		sr.flipX = Random.Range(0, 2) == 0;
		base.OnEnable();
	}
}
