using System.Collections.Generic;
using UnityEngine;

public class Tile_T0_Tile0 : TileBase
{
	[Space(50f)]
	[Range(0f, 1f)]
	public float variationChance;

	public Sprite[] sprites;

	public SpriteRenderer sr;

	[Header("Tile1")]
	[Range(0f, 1f)]
	public float tile1Chance;

	public Sprite[] sprites_Tile1;

	public SpriteRenderer sr_Tile1;

	public int tile1CellWidth;

	public override void TileCorrect(Vector2Data selfPoint, List<Vector2Data> otherTilePoints)
	{
		if (Random.value <= variationChance)
		{
			sr.sprite = sprites[Random.Range(0, sprites.Length)];
		}
		if (selfPoint.x % (float)tile1CellWidth == 0f && selfPoint.y % (float)tile1CellWidth == 0f && Random.value <= tile1Chance)
		{
			sr_Tile1.sprite = sprites_Tile1[Random.Range(0, sprites_Tile1.Length)];
			sr_Tile1.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile1);
			sr_Tile1.transform.position += new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0f);
		}
	}
}
