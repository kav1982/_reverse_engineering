using UnityEngine;

public class SpecialObj20 : MonoBehaviour
{
	public SpriteRenderer sr;

	public Sprite[] sprites;

	public Sprite[] spritesH;

	public VariableFloat offset;

	private void Start()
	{
		if (GameMgr.IsHarmony_Static && spritesH != null)
		{
			sr.sprite = spritesH[Random.Range(0, spritesH.Length)];
		}
		else
		{
			sr.sprite = sprites[Random.Range(0, sprites.Length)];
		}
		sr.flipX = ((Random.Range(0, 2) == 0) ? true : false);
		Vector3 rootPoint = base.transform.position + Tool2D.GetDir() * offset.RandomResult();
		sr.transform.position = Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.Cliff) + new Vector3(0f, 0f, -0.2f);
		Object.Destroy(this);
	}
}
