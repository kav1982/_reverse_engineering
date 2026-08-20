using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpecialObj16 : MonoBehaviour
{
	public Transform tsf_Layer;

	public SpriteRenderer sr;

	public Sprite sprite_Big;

	public Sprite sprite_Small;

	public Light2D light_Big;

	public Light2D light_Small;

	public float scaleChangeMin;

	public float scaleChangeMax;

	private void Start()
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile2);
		if (Random.Range(0, 2) == 0)
		{
			sr.sprite = sprite_Big;
			light_Small.enabled = true;
			Object.Destroy(light_Big.gameObject);
		}
		else
		{
			sr.sprite = sprite_Small;
			light_Big.enabled = true;
			Object.Destroy(light_Small.gameObject);
		}
		sr.flipX = Random.value > 0.5f;
		tsf_Layer.localScale *= Random.Range(scaleChangeMin, scaleChangeMax);
		Object.Destroy(this);
	}
}
