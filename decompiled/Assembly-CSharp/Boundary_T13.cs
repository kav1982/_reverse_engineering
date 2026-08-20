using UnityEngine;

public class Boundary_T13 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_AO;

	public SpriteRenderer sr_Low;

	public SpriteRenderer sr_Boundary;

	public SpriteRenderer sr_Grass;

	public GameObject go_Tile;

	[Header("Boundary")]
	public Sprite[] sprite_Corner_RD;

	public Sprite[] sprite_Corner_UR;

	public Sprite[] sprite_LUR;

	public Sprite[] sprite_RD;

	public Sprite[] sprite_RDL;

	public Sprite[] sprite_UR;

	public Sprite[] sprite_URD;

	[Header("Grass")]
	public Sprite[] sprite_Grass_UR;

	public Sprite[] sprite_Grass_URD;

	[Header("Low")]
	public Sprite sprite_Low_LeftUpRight;

	public Sprite sprite_Low_UpRight;

	[Header("AO")]
	public Sprite[] sprite_AO_LUR;

	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_RDL;

	public Sprite[] sprite_AO_UR;

	public Sprite[] sprite_AO_URD;

	public override void Correct(Vector2Data selfPoint, RoomController levelCtrller)
	{
		sr_AO.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.BoundaryAO) + new Vector3(0f, 0f, Random.Range(-0.1f, 0.1f));
		go_Tile.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Tile0);
		sr_Boundary.transform.position += new Vector3(0f, 0f, Random.Range(-0.049f, 0.049f));
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
			sr_Boundary.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr_Grass.sprite = sprite_Grass_UR[Random.Range(0, sprite_Grass_UR.Length)];
			sr_Low.sprite = sprite_Low_UpRight;
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Boundary.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Boundary.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_Boundary.flipX = true;
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr_Boundary.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr_Boundary.flipX = true;
			sr_Grass.sprite = sprite_Grass_UR[Random.Range(0, sprite_Grass_UR.Length)];
			sr_Grass.flipX = true;
			sr_Low.sprite = sprite_Low_UpRight;
			sr_Low.flipX = true;
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Boundary.sprite = sprite_LUR[Random.Range(0, sprite_LUR.Length)];
			sr_Grass.sprite = sprite_Grass_URD[Random.Range(0, sprite_Grass_URD.Length)];
			sr_Low.sprite = sprite_Low_LeftUpRight;
			sr_AO.sprite = sprite_AO_LUR[Random.Range(0, sprite_AO_LUR.Length)];
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Boundary.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Boundary.sprite = sprite_RDL[Random.Range(0, sprite_RDL.Length)];
			sr_AO.sprite = sprite_AO_RDL[Random.Range(0, sprite_AO_RDL.Length)];
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Boundary.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_Boundary.flipX = true;
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (!flag2)
			{
				sr_Boundary.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
			}
			else if (!flag4)
			{
				sr_Boundary.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
			}
			else if (!flag6)
			{
				sr_Boundary.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
				sr_Boundary.flipX = true;
			}
			else if (!flag8)
			{
				sr_Boundary.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
				sr_Boundary.flipX = true;
			}
		}
		Object.Destroy(this);
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Destroy(go_Tile);
	}
}
