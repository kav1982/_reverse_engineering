using UnityEngine;

public class Boundary_T5 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_Stone;

	public SpriteRenderer sr_Tile;

	public SpriteRenderer sr_Cliff;

	public SpriteRenderer sr_Water;

	public SpriteRenderer sr_Water2;

	[Header("Tile")]
	public Sprite[] sprite_Tile_Corner_RD;

	public Sprite[] sprite_Tile_Corner_UR;

	public Sprite[] sprite_Tile_LUR;

	public Sprite[] sprite_Tile_RD;

	public Sprite[] sprite_Tile_RDL;

	public Sprite[] sprite_Tile_UR;

	public Sprite[] sprite_Tile_URD;

	[Header("Stone")]
	public Sprite[] sprite_Stone_Corner_RD;

	public Sprite[] sprite_Stone_Corner_UR;

	public Sprite[] sprite_Stone_LUR;

	public Sprite[] sprite_Stone_RD;

	public Sprite[] sprite_Stone_UR;

	public Sprite[] sprite_Stone_URD;

	public float stoneOffset;

	[Header("Cliff")]
	public Sprite[] sprite_Cliff_Down;

	public Sprite[] sprite_Cliff_DownCorner;

	public float cliffDownOffsetY;

	[Header("Water")]
	public Sprite[] sprite_Water_RD;

	public Sprite[] sprite_Water_RDL;

	public Sprite[] sprite_Water_UR;

	public Sprite[] sprite_Water_URD;

	public Sprite sprite_Water_Corner_LU;

	public Sprite[] sprite_Water_Corner_LU2;

	public Sprite sprite_Water_Corner_RD;

	public float waterDownOffsetY;

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
			sr_Stone.sprite = sprite_Stone_UR[Random.Range(0, sprite_Stone_UR.Length)];
			sr_Stone.transform.position += new Vector3(0f - stoneOffset, 0f - stoneOffset, 0f);
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Water.sprite = sprite_Water_UR[Random.Range(0, sprite_Water_UR.Length)];
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_RD[Random.Range(0, sprite_Tile_RD.Length)];
			sr_Stone.sprite = sprite_Stone_RD[Random.Range(0, sprite_Stone_RD.Length)];
			sr_Stone.transform.position += new Vector3(0f - stoneOffset, stoneOffset, 0f);
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, waterDownOffsetY, -0.1f);
			sr_Water.sprite = sprite_Water_RD[Random.Range(0, sprite_Water_RD.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_RD[Random.Range(0, sprite_Tile_RD.Length)];
			sr_Tile.flipX = true;
			sr_Stone.sprite = sprite_Stone_RD[Random.Range(0, sprite_Stone_RD.Length)];
			sr_Stone.transform.position += new Vector3(stoneOffset, stoneOffset, 0f);
			sr_Stone.flipX = true;
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Cliff.flipX = true;
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, waterDownOffsetY, -0.1f);
			sr_Water.sprite = sprite_Water_RD[Random.Range(0, sprite_Water_RD.Length)];
			sr_Water.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_UR[Random.Range(0, sprite_Tile_UR.Length)];
			sr_Tile.flipX = true;
			sr_Stone.sprite = sprite_Stone_UR[Random.Range(0, sprite_Stone_UR.Length)];
			sr_Stone.transform.position += new Vector3(stoneOffset, 0f - stoneOffset, 0f);
			sr_Stone.flipX = true;
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Water.sprite = sprite_Water_UR[Random.Range(0, sprite_Water_UR.Length)];
			sr_Water.flipX = true;
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_LUR[Random.Range(0, sprite_Tile_LUR.Length)];
			sr_Stone.sprite = sprite_Stone_LUR[Random.Range(0, sprite_Stone_LUR.Length)];
			sr_Stone.transform.position += new Vector3(0f, 0f - stoneOffset, 0f);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_URD[Random.Range(0, sprite_Tile_URD.Length)];
			sr_Stone.sprite = sprite_Stone_URD[Random.Range(0, sprite_Stone_URD.Length)];
			sr_Stone.transform.position += new Vector3(0f - stoneOffset, 0f, 0f);
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Water.sprite = sprite_Water_URD[Random.Range(0, sprite_Water_URD.Length)];
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_RDL[Random.Range(0, sprite_Tile_RDL.Length)];
			sr_Stone.sprite = sprite_Stone_LUR[Random.Range(0, sprite_Stone_LUR.Length)];
			sr_Stone.transform.position += new Vector3(0f, stoneOffset, 0f);
			sr_Cliff.sprite = sprite_Cliff_Down[Random.Range(0, sprite_Cliff_Down.Length)];
			sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, waterDownOffsetY, -0.1f);
			sr_Water.sprite = sprite_Water_RDL[Random.Range(0, sprite_Water_RDL.Length)];
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_URD[Random.Range(0, sprite_Tile_URD.Length)];
			sr_Tile.flipX = true;
			sr_Stone.sprite = sprite_Stone_URD[Random.Range(0, sprite_Stone_URD.Length)];
			sr_Stone.transform.position += new Vector3(stoneOffset, 0f, 0f);
			sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
			sr_Water.sprite = sprite_Water_URD[Random.Range(0, sprite_Water_URD.Length)];
			sr_Water.flipX = true;
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (!flag2)
			{
				sr_Tile.sprite = sprite_Tile_Corner_UR[Random.Range(0, sprite_Tile_Corner_UR.Length)];
				sr_Stone.sprite = sprite_Stone_Corner_UR[Random.Range(0, sprite_Stone_Corner_UR.Length)];
				sr_Stone.transform.position += new Vector3(stoneOffset, stoneOffset, 0f);
				sr_Cliff.sprite = sprite_Cliff_DownCorner[Random.Range(0, sprite_Cliff_DownCorner.Length)];
				sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
				sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Water.sprite = sprite_Water_Corner_LU;
				sr_Water.flipX = true;
				sr_Water2.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, waterDownOffsetY, -0.1f);
				sr_Water2.sprite = sprite_Water_Corner_LU2[Random.Range(0, sprite_Water_Corner_LU2.Length)];
				sr_Water2.flipX = true;
			}
			else if (!flag4)
			{
				sr_Tile.sprite = sprite_Tile_Corner_RD[Random.Range(0, sprite_Tile_Corner_RD.Length)];
				sr_Stone.sprite = sprite_Stone_Corner_RD[Random.Range(0, sprite_Stone_Corner_RD.Length)];
				sr_Stone.transform.position += new Vector3(stoneOffset, 0f - stoneOffset, 0f);
				sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Water.sprite = sprite_Water_Corner_RD;
			}
			else if (!flag6)
			{
				sr_Tile.sprite = sprite_Tile_Corner_RD[Random.Range(0, sprite_Tile_Corner_RD.Length)];
				sr_Tile.flipX = true;
				sr_Stone.sprite = sprite_Stone_Corner_RD[Random.Range(0, sprite_Stone_Corner_RD.Length)];
				sr_Stone.transform.position += new Vector3(0f - stoneOffset, 0f - stoneOffset, 0f);
				sr_Stone.flipX = true;
				sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Water.sprite = sprite_Water_Corner_RD;
				sr_Water.flipX = true;
			}
			else if (!flag8)
			{
				sr_Tile.sprite = sprite_Tile_Corner_UR[Random.Range(0, sprite_Tile_Corner_UR.Length)];
				sr_Tile.flipX = true;
				sr_Stone.sprite = sprite_Stone_Corner_UR[Random.Range(0, sprite_Stone_Corner_UR.Length)];
				sr_Stone.transform.position += new Vector3(0f - stoneOffset, stoneOffset, 0f);
				sr_Stone.flipX = true;
				sr_Cliff.sprite = sprite_Cliff_DownCorner[Random.Range(0, sprite_Cliff_DownCorner.Length)];
				sr_Cliff.transform.position += new Vector3(0f, cliffDownOffsetY, 0f);
				sr_Cliff.flipX = true;
				sr_Water.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
				sr_Water.sprite = sprite_Water_Corner_LU;
				sr_Water2.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Cliff) + new Vector3(0f, waterDownOffsetY, -0.1f);
				sr_Water2.sprite = sprite_Water_Corner_LU2[Random.Range(0, sprite_Water_Corner_LU2.Length)];
			}
		}
		Object.Destroy(this);
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Destroy(base.gameObject);
	}
}
