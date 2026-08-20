using UnityEngine;

public class SpecialObj31 : LayerCorrect
{
	[Space(50f)]
	public float offset;

	public Sprite[] sprites;

	public SpriteRenderer sr;

	public SpriteRenderer sr_CenterEditor;

	public override void OnEnable()
	{
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		Object.Destroy(sr_CenterEditor.gameObject);
		base.OnEnable();
	}
}
