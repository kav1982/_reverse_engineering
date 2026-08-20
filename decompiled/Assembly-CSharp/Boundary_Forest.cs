using UnityEngine;

public class Boundary_Forest : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public GameObject go_Tile;

	public Sprite[] sprite_CornerRightDown;

	public Sprite[] sprite_CornerUpRight;

	public Sprite[] sprite_LeftUpRight;

	public Sprite[] sprite_RightDown;

	public Sprite[] sprite_RightDownLeft;

	public Sprite[] sprite_UpRight;

	public Sprite[] sprite_UpRightDown;

	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_DetailLeftUpRight;

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
		go_Tile.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Tile0);
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
			sr.sprite = sprite_UpRight[Random.Range(0, sprite_UpRight.Length)];
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_RightDown[Random.Range(0, sprite_RightDown.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RightDown[Random.Range(0, sprite_RightDown.Length)];
			sr.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_UpRight[Random.Range(0, sprite_UpRight.Length)];
			sr.flipX = true;
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LeftUpRight[Random.Range(0, sprite_LeftUpRight.Length)];
			RandomDetail(selfPoint, levelCtrller, pfb_DetailLeftUpRight, 0f);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_UpRightDown[Random.Range(0, sprite_UpRightDown.Length)];
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RightDownLeft[Random.Range(0, sprite_RightDownLeft.Length)];
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_UpRightDown[Random.Range(0, sprite_UpRightDown.Length)];
			sr.flipX = true;
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.transform.position += new Vector3(0f, 0f, -0.01f);
			if (!flag2)
			{
				sr.sprite = sprite_CornerUpRight[Random.Range(0, sprite_CornerUpRight.Length)];
			}
			else if (!flag4)
			{
				sr.sprite = sprite_CornerRightDown[Random.Range(0, sprite_CornerRightDown.Length)];
			}
			else if (!flag6)
			{
				sr.sprite = sprite_CornerRightDown[Random.Range(0, sprite_CornerRightDown.Length)];
				sr.flipX = true;
			}
			else if (!flag8)
			{
				sr.sprite = sprite_CornerUpRight[Random.Range(0, sprite_CornerUpRight.Length)];
				sr.flipX = true;
			}
		}
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Destroy(sr.gameObject);
		Object.Destroy(go_Tile);
	}
}
