using UnityEngine;

public class SpecialObj26 : LayerCorrect
{
	[Space(50f)]
	public Sprite[] sprites;

	public SpriteRenderer sr;

	public override void OnEnable()
	{
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		sr.transform.rotation = Tool2D.GetRotation();
		base.OnEnable();
	}
}
