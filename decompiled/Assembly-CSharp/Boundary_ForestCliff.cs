using UnityEngine;

public class Boundary_ForestCliff : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_Tile;

	public SpriteRenderer sr_Cliff;

	public SpriteRenderer sr_Rail;

	public Sprite[] sprite_Cliff_CornerRightDown;

	public Sprite[] sprite_Cliff_CornerUpRight;

	public Sprite[] sprite_Cliff_RightDown;

	public Sprite[] sprite_Cliff_RightDownLeft;

	public Sprite[] sprite_Cliff_UpRightDown;

	public Sprite sprite_Rail_CornerRightDown;

	public Sprite sprite_Rail_CornerUpRight;

	public Sprite sprite_Rail_LeftUpRight;

	public Sprite sprite_Rail_RightDown;

	public Sprite sprite_Rail_UpRight;

	public Sprite sprite_Rail_UpRightDown;

	public Sprite[] sprite_Tile_CornerUpRight;

	public Sprite[] sprite_Tile_LeftUpRight;

	public Sprite[] sprite_Tile_UpRight;

	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_DetailLeftUpRight;

	public GameObject[] pfb_DetailRightDownLeft;

	public GameObject[] pfb_DetailUpRightDown;

	private void RandomDetail(Vector2Data selfPoint, RoomController levelCtrller, GameObject[] prefabs, float rotate)
	{
		if (selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(-2f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(-1f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(0f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(1f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(2f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(3f, 1f))
		{
			return;
		}
		bool flag = true;
		for (int i = -detailMinInterval; i <= detailMinInterval; i++)
		{
			for (int j = -detailMinInterval; j <= detailMinInterval; j++)
			{
				if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(i, j)) && levelCtrller.boundaryBase1Dic[selfPoint + new Vector2Data(i, j)].HaveDetail)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag && Random.value <= detailChange)
		{
			base.HaveDetail = true;
			Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)], base.transform.position, Quaternion.identity, base.transform.parent).transform.rotation = Tool2D.GetRotation(rotate);
		}
	}

	public override void Correct(Vector2Data selfPoint, RoomController levelCtrller)
	{
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
			sr_Tile.sprite = sprite_Tile_UpRight[Random.Range(0, sprite_Tile_UpRight.Length)];
			sr_Cliff.sprite = sprite_Cliff_UpRightDown[Random.Range(0, sprite_Cliff_UpRightDown.Length)];
			sr_Rail.sprite = sprite_Rail_UpRight;
			sr_Rail.transform.position += new Vector3(-0.25f, -0.25f, 0f);
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_UpRight[Random.Range(0, sprite_Tile_UpRight.Length)];
			sr_Tile.transform.rotation = Tool2D.GetRotation(270f);
			sr_Cliff.sprite = sprite_Cliff_RightDown[Random.Range(0, sprite_Cliff_RightDown.Length)];
			sr_Rail.sprite = sprite_Rail_RightDown;
			sr_Rail.transform.position += new Vector3(-0.25f, 0f, 0f);
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_UpRight[Random.Range(0, sprite_Tile_UpRight.Length)];
			sr_Tile.transform.rotation = Tool2D.GetRotation(180f);
			sr_Cliff.sprite = sprite_Cliff_RightDown[Random.Range(0, sprite_Cliff_RightDown.Length)];
			sr_Cliff.flipX = true;
			sr_Rail.sprite = sprite_Rail_RightDown;
			sr_Rail.transform.position += new Vector3(0.25f, 0f, 0f);
			sr_Rail.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_UpRight[Random.Range(0, sprite_Tile_UpRight.Length)];
			sr_Tile.transform.rotation = Tool2D.GetRotation(90f);
			sr_Cliff.sprite = sprite_Cliff_UpRightDown[Random.Range(0, sprite_Cliff_UpRightDown.Length)];
			sr_Cliff.flipX = true;
			sr_Rail.sprite = sprite_Rail_UpRight;
			sr_Rail.transform.position += new Vector3(0.25f, -0.25f, 0f);
			sr_Rail.flipX = true;
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_LeftUpRight[Random.Range(0, sprite_Tile_LeftUpRight.Length)];
			sr_Rail.sprite = sprite_Rail_LeftUpRight;
			sr_Rail.transform.position += new Vector3(0f, -0.25f, 0f);
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Tile.sprite = sprite_Tile_LeftUpRight[Random.Range(0, sprite_Tile_LeftUpRight.Length)];
			sr_Tile.transform.localRotation = Tool2D.GetRotation(270f);
			sr_Cliff.sprite = sprite_Cliff_UpRightDown[Random.Range(0, sprite_Cliff_UpRightDown.Length)];
			sr_Rail.sprite = sprite_Rail_UpRightDown;
			sr_Rail.transform.position += new Vector3(-0.25f, 0f, 0f);
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_LeftUpRight[Random.Range(0, sprite_Tile_LeftUpRight.Length)];
			sr_Tile.transform.localRotation = Tool2D.GetRotation(180f);
			sr_Cliff.sprite = sprite_Cliff_RightDownLeft[Random.Range(0, sprite_Cliff_RightDownLeft.Length)];
			sr_Rail.sprite = sprite_Rail_LeftUpRight;
			sr_Rail.transform.position += new Vector3(0f, 0.25f, 0f);
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Tile.sprite = sprite_Tile_LeftUpRight[Random.Range(0, sprite_Tile_LeftUpRight.Length)];
			sr_Tile.transform.localRotation = Tool2D.GetRotation(90f);
			sr_Cliff.sprite = sprite_Cliff_UpRightDown[Random.Range(0, sprite_Cliff_UpRightDown.Length)];
			sr_Cliff.flipX = true;
			sr_Rail.sprite = sprite_Rail_UpRightDown;
			sr_Rail.transform.position += new Vector3(0.25f, 0f, 0f);
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr_Tile.transform.position += new Vector3(0f, 0f, -0.01f);
			if (!flag2)
			{
				sr_Tile.sprite = sprite_Tile_CornerUpRight[Random.Range(0, sprite_Tile_CornerUpRight.Length)];
				sr_Cliff.sprite = sprite_Cliff_CornerUpRight[Random.Range(0, sprite_Cliff_CornerUpRight.Length)];
				sr_Rail.sprite = sprite_Rail_CornerUpRight;
				sr_Rail.transform.position += new Vector3(0.25f, 0.25f, -0.01f);
			}
			else if (!flag4)
			{
				sr_Tile.sprite = sprite_Tile_CornerUpRight[Random.Range(0, sprite_Tile_CornerUpRight.Length)];
				sr_Tile.transform.rotation = Tool2D.GetRotation(270f);
				sr_Cliff.sprite = sprite_Cliff_CornerRightDown[Random.Range(0, sprite_Cliff_CornerRightDown.Length)];
				sr_Rail.sprite = sprite_Rail_CornerRightDown;
				sr_Rail.transform.position += new Vector3(0.25f, -0.25f, 0f);
			}
			else if (!flag6)
			{
				sr_Tile.sprite = sprite_Tile_CornerUpRight[Random.Range(0, sprite_Tile_CornerUpRight.Length)];
				sr_Tile.transform.rotation = Tool2D.GetRotation(180f);
				sr_Cliff.sprite = sprite_Cliff_CornerRightDown[Random.Range(0, sprite_Cliff_CornerRightDown.Length)];
				sr_Cliff.flipX = true;
				sr_Rail.sprite = sprite_Rail_CornerRightDown;
				sr_Rail.transform.position += new Vector3(-0.25f, -0.25f, 0f);
				sr_Rail.flipX = true;
			}
			else if (!flag8)
			{
				sr_Tile.sprite = sprite_Tile_CornerUpRight[Random.Range(0, sprite_Tile_CornerUpRight.Length)];
				sr_Tile.transform.rotation = Tool2D.GetRotation(90f);
				sr_Cliff.sprite = sprite_Cliff_CornerUpRight[Random.Range(0, sprite_Cliff_CornerUpRight.Length)];
				sr_Cliff.flipX = true;
				sr_Rail.sprite = sprite_Rail_CornerUpRight;
				sr_Rail.transform.position += new Vector3(-0.25f, 0.25f, -0.01f);
				sr_Rail.flipX = true;
			}
		}
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Destroy(sr_Tile.gameObject);
		Object.Destroy(sr_Cliff.gameObject);
		Object.Destroy(sr_Rail.gameObject);
	}
}
