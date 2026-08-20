using UnityEngine;

public class Boundary_T1 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_Rail;

	public SpriteRenderer sr_Tile;

	public SpriteRenderer sr_Cliff;

	public SpriteRenderer sr_Lava;

	public SpriteRenderer sr_Lava2;

	[Header("Cliff")]
	public Sprite[] sprite_Cliff_Down;

	public Sprite[] sprite_Cliff_DownCorner;

	public float cliffDownOffsetY;

	[Header("Lava")]
	public Sprite sprite_Lava_Corner_LU;

	public Sprite[] sprite_Lava_Corner_LU2;

	public Sprite sprite_Lava_Corner_RD;

	public Sprite[] sprite_Lava_RD;

	public Sprite[] sprite_Lava_RDL;

	public Sprite[] sprite_Lava_UR;

	public Sprite[] sprite_Lava_URD;

	public float lavaDownOffsetY;

	[Header("Rail")]
	public Sprite[] sprite_Rail_Corner_RD;

	public Sprite[] sprite_Rail_Corner_UR;

	public Sprite[] sprite_Rail_LUR;

	public Sprite[] sprite_Rail_RD;

	public Sprite[] sprite_Rail_UR;

	public Sprite[] sprite_Rail_URD;

	public float railOffset;

	[Header("Tile")]
	public Sprite[] sprite_Tile_Corner_RD;

	public Sprite[] sprite_Tile_Corner_UR;

	public Sprite[] sprite_Tile_LUR;

	public Sprite[] sprite_Tile_RD;

	public Sprite[] sprite_Tile_RDL;

	public Sprite[] sprite_Tile_UR;

	public Sprite[] sprite_Tile_URD;

	public override void Correct(Vector2Data selfPoint, RoomController levelCtrller)
	{
		sr_Tile.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile0);
		sr_Cliff.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 1f)))
		{
			flag2 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, 1f)))
		{
			flag2 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag3 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag3 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, -1f)))
		{
			flag4 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, -1f)))
		{
			flag4 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag5 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag5 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, -1f)))
		{
			flag6 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, -1f)))
		{
			flag6 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag7 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag7 = true;
		}
		if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 1f)))
		{
			flag8 = true;
		}
		else if (levelCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, 1f)))
		{
			flag8 = true;
		}
		if (flag && flag3 && !flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_UR[Random.Range(0, sprite_Tile_UR.Length)];
			sr_Rail.sprite = sprite_Rail_UR[Random.Range(0, sprite_Rail_UR.Length)];
			sr_Rail.transform.position += new Vector3(0f - railOffset, 0f - railOffset, 0f);
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Lava.sprite = sprite_Lava_UR[Random.Range(0, sprite_Lava_UR.Length)];
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_RD[Random.Range(0, sprite_Tile_RD.Length)];
			sr_Rail.sprite = sprite_Rail_RD[Random.Range(0, sprite_Rail_RD.Length)];
			sr_Rail.transform.position += new Vector3(0f - railOffset, railOffset, 0f);
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, lavaDownOffsetY, -0.1f);
			sr_Lava.sprite = sprite_Lava_RD[Random.Range(0, sprite_Lava_RD.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_RD[Random.Range(0, sprite_Tile_RD.Length)];
			sr_Tile.flipX = true;
			sr_Rail.sprite = sprite_Rail_RD[Random.Range(0, sprite_Rail_RD.Length)];
			sr_Rail.transform.position += new Vector3(railOffset, railOffset, 0f);
			sr_Rail.flipX = true;
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Cliff.flipX = true;
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, lavaDownOffsetY, -0.1f);
			sr_Lava.sprite = sprite_Lava_RD[Random.Range(0, sprite_Lava_RD.Length)];
			sr_Lava.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_UR[Random.Range(0, sprite_Tile_UR.Length)];
			sr_Tile.flipX = true;
			sr_Rail.sprite = sprite_Rail_UR[Random.Range(0, sprite_Rail_UR.Length)];
			sr_Rail.transform.position += new Vector3(railOffset, 0f - railOffset, 0f);
			sr_Rail.flipX = true;
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Lava.sprite = sprite_Lava_UR[Random.Range(0, sprite_Lava_UR.Length)];
			sr_Lava.flipX = true;
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_LUR[Random.Range(0, sprite_Tile_LUR.Length)];
			sr_Rail.sprite = sprite_Rail_LUR[Random.Range(0, sprite_Rail_LUR.Length)];
			sr_Rail.transform.position += new Vector3(0f, 0f - railOffset, 0f);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_URD[Random.Range(0, sprite_Tile_URD.Length)];
			sr_Rail.sprite = sprite_Rail_URD[Random.Range(0, sprite_Rail_URD.Length)];
			sr_Rail.transform.position += new Vector3(0f - railOffset, 0f, 0f);
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Lava.sprite = sprite_Lava_URD[Random.Range(0, sprite_Lava_URD.Length)];
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_RDL[Random.Range(0, sprite_Tile_RDL.Length)];
			sr_Rail.sprite = sprite_Rail_LUR[Random.Range(0, sprite_Rail_LUR.Length)];
			sr_Rail.transform.position += new Vector3(0f, railOffset, 0f);
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, lavaDownOffsetY, -0.1f);
			sr_Lava.sprite = sprite_Lava_RDL[Random.Range(0, sprite_Lava_RDL.Length)];
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_URD[Random.Range(0, sprite_Tile_URD.Length)];
			sr_Tile.flipX = true;
			sr_Rail.sprite = sprite_Rail_URD[Random.Range(0, sprite_Rail_URD.Length)];
			sr_Rail.transform.position += new Vector3(railOffset, 0f, 0f);
			sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Lava.sprite = sprite_Lava_URD[Random.Range(0, sprite_Lava_URD.Length)];
			sr_Lava.flipX = true;
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (!flag2)
			{
				sr_Tile.sprite = sprite_Tile_Corner_UR[Random.Range(0, sprite_Tile_Corner_UR.Length)];
				sr_Rail.sprite = sprite_Rail_Corner_UR[Random.Range(0, sprite_Rail_Corner_UR.Length)];
				sr_Rail.transform.position += new Vector3(railOffset, railOffset, 0f);
				sr_Cliff.sprite = sprite_Cliff_DownCorner[Random.Range(0, sprite_Cliff_DownCorner.Length)];
				sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
				sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Lava.sprite = sprite_Lava_Corner_LU;
				sr_Lava.flipX = true;
				sr_Lava2.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, lavaDownOffsetY, -0.1f);
				sr_Lava2.sprite = sprite_Lava_Corner_LU2[Random.Range(0, sprite_Lava_Corner_LU2.Length)];
				sr_Lava2.flipX = true;
			}
			else if (!flag4)
			{
				sr_Tile.sprite = sprite_Tile_Corner_RD[Random.Range(0, sprite_Tile_Corner_RD.Length)];
				sr_Rail.sprite = sprite_Rail_Corner_RD[Random.Range(0, sprite_Rail_Corner_RD.Length)];
				sr_Rail.transform.position += new Vector3(railOffset, 0f - railOffset, -0.01f);
				sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Lava.sprite = sprite_Lava_Corner_RD;
			}
			else if (!flag6)
			{
				sr_Tile.sprite = sprite_Tile_Corner_RD[Random.Range(0, sprite_Tile_Corner_RD.Length)];
				sr_Tile.flipX = true;
				sr_Rail.sprite = sprite_Rail_Corner_RD[Random.Range(0, sprite_Rail_Corner_RD.Length)];
				sr_Rail.transform.position += new Vector3(0f - railOffset, 0f - railOffset, -0.01f);
				sr_Rail.flipX = true;
				sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Lava.sprite = sprite_Lava_Corner_RD;
				sr_Lava.flipX = true;
			}
			else if (!flag8)
			{
				sr_Tile.sprite = sprite_Tile_Corner_UR[Random.Range(0, sprite_Tile_Corner_UR.Length)];
				sr_Tile.flipX = true;
				sr_Rail.sprite = sprite_Rail_Corner_UR[Random.Range(0, sprite_Rail_Corner_UR.Length)];
				sr_Rail.transform.position += new Vector3(0f - railOffset, railOffset, 0f);
				sr_Rail.flipX = true;
				sr_Cliff.sprite = sprite_Cliff_DownCorner[Random.Range(0, sprite_Cliff_DownCorner.Length)];
				sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
				sr_Cliff.flipX = true;
				sr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Lava.sprite = sprite_Lava_Corner_LU;
				sr_Lava2.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, lavaDownOffsetY, -0.1f);
				sr_Lava2.sprite = sprite_Lava_Corner_LU2[Random.Range(0, sprite_Lava_Corner_LU2.Length)];
			}
		}
		Object.Destroy(this);
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Destroy(base.gameObject);
	}
}
