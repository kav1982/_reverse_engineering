using System.Collections.Generic;
using UnityEngine;

public class Tile_T1_Tile0 : TileBase
{
	[Space(50f)]
	[Range(0f, 1f)]
	public float variationChance;

	public Sprite[] sprites;

	public SpriteRenderer sr;

	[Range(0f, 1f)]
	[Header("Tile1")]
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
		if (Random.value <= tile1Chance && selfPoint.x % (float)tile1CellWidth == 0f && selfPoint.y % (float)tile1CellWidth == 0f && otherTilePoints.Contains(selfPoint + new Vector2Data(0f, 1f)) && otherTilePoints.Contains(selfPoint + new Vector2Data(1f, 1f)) && otherTilePoints.Contains(selfPoint + new Vector2Data(1f, 0f)))
		{
			sr_Tile1.sprite = sprites_Tile1[Random.Range(0, sprites_Tile1.Length)];
			sr_Tile1.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile1);
		}
	}
}
